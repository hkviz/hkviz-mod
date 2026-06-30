using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HKViz.Shared.Auth;
using UnityEngine;
using UnityEngine.Networking;

namespace HKViz.Shared.Upload;

public abstract class UploadManager<TQueueEntry, TUploadRequest> : IUploadManager where TQueueEntry : IUploadQueueEntry<TUploadRequest> {
    
    public readonly List<TQueueEntry> queuedFiles = [];
    public readonly List<TQueueEntry> failedUploads = [];
    public readonly List<TQueueEntry> finishedUploadFiles = [];
    private readonly WaitForSeconds WAIT_1SEC = new (1);

    private bool uploadInProgress;

    public event Action? QueuesChanged;

    private readonly ServerApi serverApi;
    private readonly AuthManager authManager;
    private readonly IHkVizLogger _logger;
    private readonly IUploadPathResolver<TQueueEntry> uploadPathResolver;
    
    public UploadManager(
        ServerApi serverApi, 
        AuthManager authManager, 
        IHkVizLogger logger, 
        IUploadPathResolver<TQueueEntry> uploadPathResolver
    ) {
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

    public void QueueFile(TQueueEntry entry) {
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
        var uploadRequest = queueEntry.ToUploadRequest(
            ingameAuthId: authId,
            modVersion: HkVizSharedConstants.GetModVersion()
        );
        serverApi.ApiPost<TUploadRequest, CreateUploadPartUrlResponse>(
            path: "ingameupload/part/init",
            body: uploadRequest,
            onSuccess: initResponse => {
                if (initResponse.alreadyFinished) {
                    _logger.LogInfo("Upload was already marked complete in Backend. Will not try to upload again");
                    OnSuccess();
                } else {

                    try {
                        var uploadUrl = initResponse.signedUploadUrl;
                        var filePath = uploadPathResolver.GetPath(queueEntry);
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
                                        localRunId = queueEntry.LocalRunId,
                                        fileId = initResponse.fileId,
                                    },
                                    onSuccess: _ => OnSuccess(),
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
            _logger.LogInfo($"Successfully uploaded recording part {queueEntry.ProfileId} {queueEntry.PartNumber}");
            queueEntry.FinishedUploadAtUnixSeconds = DateTimeUtils.GetUnixSeconds();
            finishedUploadFiles.Add(queueEntry);
            queuedFiles.Remove(queueEntry);
            uploadInProgress = false;
            QueuesChanged?.Invoke();
        }
    }

    public void DeleteAlreadyUploadedFiles() {
        for (int i = finishedUploadFiles.Count - 1; i >= 0; i--) {
            var queueEntry = finishedUploadFiles[i];
            var path = uploadPathResolver.GetPath(queueEntry);
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

    public int QueuedFilesQueueCount() => queuedFiles.Count;
    public int FinishedUploadFilesQueueCount() => finishedUploadFiles.Count;
    public int FailedUploadsQueueCount() => failedUploads.Count;

    public void RetryFailedUploads() {
        queuedFiles.AddRange(failedUploads);
        failedUploads.Clear();
        QueuesChanged?.Invoke();
    }

    public void AddUploadEntries(
        List<TQueueEntry>? queuedFilesToAdd,
        List<TQueueEntry>? failedUploadsToAdd,
        List<TQueueEntry>? finishedUploadFilesToAdd
    ) {
        if (queuedFilesToAdd != null) {
            queuedFiles.AddRange(queuedFilesToAdd);
        }
        if (failedUploadsToAdd != null) {
            failedUploads.AddRange(failedUploadsToAdd);
        }

        if (finishedUploadFilesToAdd != null) {
            finishedUploadFiles.AddRange(finishedUploadFilesToAdd);
        }
        QueuesChanged?.Invoke();
    }
}