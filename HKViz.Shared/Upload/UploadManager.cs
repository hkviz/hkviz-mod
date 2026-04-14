using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HKViz.Shared.Auth;
using UnityEngine;
using UnityEngine.Networking;

namespace HKViz.Shared.Upload;

public class UploadManager {
    
    public readonly List<UploadQueueEntry> queuedFiles = [];
    public readonly List<UploadQueueEntry> failedUploads = [];
    public readonly List<UploadQueueEntry> finishedUploadFiles = [];
    private readonly WaitForSeconds WAIT_1SEC = new (1);

    private bool uploadInProgress = false;

    public event Action? QueuesChanged;

    private readonly ServerApi serverApi;
    private readonly AuthManager authManager;
    private readonly IHkVizLogger _logger;
    private readonly IUploadPathResolver uploadPathResolver;
    
    public UploadManager(ServerApi serverApi, AuthManager authManager, IHkVizLogger logger, IUploadPathResolver uploadPathResolver) {
        this.serverApi = serverApi;
        this.authManager = authManager;
        this._logger = logger;
        this.uploadPathResolver = uploadPathResolver;

        authManager.StateChanged += AuthStateChanged;
    }

    private void AuthStateChanged(LoginState state) {
        if (state == LoginState.LOGGED_IN) {
            RetryFailedUploads();
        }
    }

    public void Initialize() {
        UploadCoro().StartGlobal();
    }

    public void QueueFile(UploadQueueEntry entry) {
        queuedFiles.Add(entry);
    }

    private IEnumerator UploadCoro() {
        while (true) {
            yield return WAIT_1SEC;

            var authId = authManager.AuthId;
            if (!uploadInProgress && authId != null && queuedFiles.Count > 0) {
                UploadFirstFileInQueue();
            }
        }
    }

    private void UploadFirstFileInQueue() {
        // some nested callbacks ahead :)
        uploadInProgress = true;
        var authId = authManager.AuthId;
        if (authId == null) {
            _logger.LogError("AuthId is null, cannot upload");
            uploadInProgress = false;
            return;
        }
        var queueEntry = queuedFiles[0];
        var request =
        serverApi.ApiPost<CreateUploadPartUrlRequest, CreateUploadPartUrlResponse>(
            path: "ingameupload/part/init",
            body: new CreateUploadPartUrlRequest {
                modVersion = HkVizSharedConstants.GetModVersion(),

                ingameAuthId = authId,
                localRunId = queueEntry.localRunId,
                partNumber = queueEntry.partNumber,


                hkVersion = queueEntry.hkVersion,
                playTime = queueEntry.playTime,
                maxHealth = queueEntry.maxHealth,
                mpReserveMax = queueEntry.mpReserveMax,
                geo = queueEntry.geo,
                dreamOrbs = queueEntry.dreamOrbs,
                permadeathMode = queueEntry.permadeathMode,
                mapZone = queueEntry.mapZone,
                killedHollowKnight = queueEntry.killedHollowKnight,
                killedFinalBoss = queueEntry.killedFinalBoss,
                killedVoidIdol = queueEntry.killedVoidIdol,
                completionPercentage = queueEntry.completionPercentage,
                unlockedCompletionRate = queueEntry.unlockedCompletionRate,
                dreamNailUpgraded = queueEntry.dreamNailUpgraded,
                lastScene = queueEntry.lastScene,

                firstUnixSeconds = queueEntry.firstUnixSeconds,
                lastUnixSeconds = queueEntry.lastUnixSeconds,
            },
            onSuccess: initResponse => {
                if (initResponse.alreadyFinished) {
                    _logger.LogInfo("Upload was already marked complete in Backend. Will not try to upload again");
                    OnSuccess();
                } else {

                    try {
                        var uploadUrl = initResponse.signedUploadUrl;
                        if (uploadUrl == null) {
                            _logger.LogError("Signed upload URL is null");
                            OnError(null);
                            return;
                        }
                        var filePath = uploadPathResolver.GetPath(queueEntry);
                        if (filePath == null) {
                            _logger.LogError("Upload file path is null");
                            OnError(null);
                            return;
                        }
                        if (!File.Exists(filePath)) {
                            _logger.LogError($"Upload file does not exist: {filePath}");
                            OnError(null);
                            return;
                        }
                        serverApi.R2Upload(
                            uploadUrl, 
                            filePath,
                            onSuccess: () => {
                                serverApi.ApiPost<MarkUploadPartFinishedRequest, Empty>(
                                    path: "ingameupload/part/finish",
                                    body: new MarkUploadPartFinishedRequest {
                                        ingameAuthId = authId,
                                        localRunId = queueEntry.localRunId,
                                        fileId = initResponse.fileId,
                                    },
                                    onSuccess: response => OnSuccess(),
                                    onError: OnError
                                ).StartGlobal();
                            },
                            onError: OnError
                        ).StartGlobal();
                    } catch (Exception ex) {
                        _logger.LogError("R2 upload failed");
                        _logger.LogError(ex);
                        OnError(null);
                    }
                }
            },
            onError: OnError
        ).StartGlobal();


        void OnError(UnityWebRequest? req) {
            failedUploads.Add(queueEntry);
            queuedFiles.Remove(queueEntry);
            uploadInProgress = false;
            _logger.LogInfo("Error while uploading to " + (req?.url ?? "unknown URL"));
            _logger.LogInfo($"{req?.result}");
            QueuesChanged?.Invoke();
        }

        void OnSuccess() {
            _logger.LogInfo($"Successfully uploaded recording part {queueEntry.profileId} {queueEntry.partNumber}");
            queueEntry.finishedUploadAtUnixSeconds = DateTimeUtils.GetUnixSeconds();
            finishedUploadFiles.Add(queueEntry);
            queuedFiles.Remove(queueEntry);
            uploadInProgress = false;
            QueuesChanged?.Invoke();
        }
    }

    internal void DeleteAlreadyUploadedFiles() {
        for (int i = finishedUploadFiles.Count - 1; i >= 0; i--) {
            var queueEntry = finishedUploadFiles[i];
            var path = uploadPathResolver.GetPath(queueEntry);
            if (path == null) {
                _logger.LogError("Upload file path is null for finished upload entry");
                finishedUploadFiles.Remove(queueEntry);
                QueuesChanged?.Invoke();
                continue;
            }
            if (!File.Exists(path)) {
                _logger.LogError($"Upload file does not exist at path: {path}. It may have been already deleted or moved.");
                continue;
            }
            try {
                File.Delete(path);
                finishedUploadFiles.Remove(queueEntry);
                QueuesChanged?.Invoke();
            } catch (Exception ex) {
                _logger.LogInfo($"Could not delete already uploaded analytics file {path} {ex.Message}");
            }
        }
    }

    internal int QueuedFilesQueueCount() => queuedFiles.Count;
    internal int FinishedUploadFilesQueueCount() => finishedUploadFiles.Count;
    internal int FailedUploadsQueueCount() => failedUploads.Count;

    internal void RetryFailedUploads() {
        queuedFiles.AddRange(failedUploads);
        failedUploads.Clear();
        QueuesChanged?.Invoke();
    }

    public void AddUploadEntries(
        List<UploadQueueEntry>? queuedFiles,
        List<UploadQueueEntry>? failedUploads,
        List<UploadQueueEntry>? finishedUploadFiles
    ) {
        if (queuedFiles != null) {
            this.queuedFiles.AddRange(queuedFiles);
        }
        if (failedUploads != null) {
            this.failedUploads.AddRange(failedUploads);
        }

        if (finishedUploadFiles != null) {
            this.finishedUploadFiles.AddRange(finishedUploadFiles);
        }
        QueuesChanged?.Invoke();
    }
}