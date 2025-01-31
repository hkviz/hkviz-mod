using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace HKViz {
    [Serializable]
    public class UploadQueueEntry {
        public string localRunId;
        public int partNumber;
        public int profileId;

        // meta data, so it can easily be displayed in the UI without parsing recording files
        public string? hkVersion;
        public float? playTime;
        public int? maxHealth;
        public int? mpReserveMax;
        public int? geo;
        public int? dreamOrbs;
        public int? permadeathMode;
        public string? mapZone;
        public bool? killedHollowKnight;
        public bool? killedFinalBoss;
        public bool? killedVoidIdol;
        public int? completionPercentage;
        public bool? unlockedCompletionRate;
        public bool? dreamNailUpgraded;
        public string? lastScene;

        public long firstUnixSeconds;
        public long lastUnixSeconds;

        public long finishedUploadAtUnixSeconds;
    }


    internal class UploadManager : Loggable {
        [Serializable]
        private class CreateUploadPartUrlRequest {
            public string modVersion;

            public string ingameAuthId;
            public string localRunId;
            public int partNumber;

            // meta data, so it can easily be displayed in the UI without parsing recording files
            public string? hkVersion;
            public float? playTime;
            public int? maxHealth;
            public int? mpReserveMax;
            public int? geo;
            public int? dreamOrbs;
            public int? permadeathMode;
            public string? mapZone;
            public bool? killedHollowKnight;
            public bool? killedFinalBoss;
            public bool? killedVoidIdol;
            public int? completionPercentage;
            public bool? unlockedCompletionRate;
            public bool? dreamNailUpgraded;
            public string? lastScene;

            public long firstUnixSeconds;
            public long lastUnixSeconds;
        }
        [Serializable]
        private class CreateUploadPartUrlResponse {
            public string fileId;
            public string runId;
            public string signedUploadUrl;
            public bool alreadyFinished;
        }

        [Serializable]
        private class MarkUploadPartFinishedRequest {
            public string ingameAuthId;
            public string localRunId;
            public string fileId;
        }

        private static UploadManager instance;
        public static UploadManager Instance {
            get {
                instance ??= new UploadManager();
                return instance;
            }
        }

        private List<UploadQueueEntry> queuedFiles = new List<UploadQueueEntry>();
        private List<UploadQueueEntry> failedUploads = new List<UploadQueueEntry>();
        private List<UploadQueueEntry> finishedUploadFiles = new List<UploadQueueEntry>();
        private WaitForSeconds WAIT_1SEC = new WaitForSeconds(1);

        private bool uploadInProgress = false;

        public event Action? QueuesChanged;

        public void Initialize() {
            UploadCoro().StartGlobal();
        }

        public void QueueFile(UploadQueueEntry entry) {
            queuedFiles.Add(entry);
        }

        public void GlobalSettingsBeforeSave() {
            GlobalSettingsManager.SettingsOfCurrentUser.queuedUploadFiles = queuedFiles;
            GlobalSettingsManager.SettingsOfCurrentUser.failedUploadFiles = failedUploads;
            GlobalSettingsManager.SettingsOfCurrentUser.finishedUploadFiles = finishedUploadFiles;
        }

        public void GlobalSettingsLoaded() {
            Log("GS loaded upload manager" + (GlobalSettingsManager.SettingsOfCurrentUser.queuedUploadFiles.Count + "-" + GlobalSettingsManager.SettingsOfCurrentUser.failedUploadFiles.Count));
            queuedFiles.AddRange(GlobalSettingsManager.SettingsOfCurrentUser.queuedUploadFiles);
            failedUploads.AddRange(GlobalSettingsManager.SettingsOfCurrentUser.failedUploadFiles);
            finishedUploadFiles.AddRange(GlobalSettingsManager.SettingsOfCurrentUser.finishedUploadFiles);
            QueuesChanged?.Invoke();
        }

        private IEnumerator UploadCoro() {
            while (true) {
                yield return WAIT_1SEC;

                var authId = HKVizAuthManager.Instance.AuthId;
                if (!uploadInProgress && authId != null && queuedFiles.Count > 0) {
                    UploadFirstFileInQueue();
                }
            }
        }

        private void UploadFirstFileInQueue() {
            // some nested callbacks ahead :)
            uploadInProgress = true;
            var authId = HKVizAuthManager.Instance.AuthId;
            var queueEntry = queuedFiles[0];
            var request =
            ServerApi.Instance.ApiPost<CreateUploadPartUrlRequest, CreateUploadPartUrlResponse>(
                path: "ingameupload/part/init",
                body: new CreateUploadPartUrlRequest() {
                    modVersion = Constants.GetVersion(),

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
                        Log("Upload was already marked complete in Backend. Will not try to upload again");
                        OnSuccess();
                    } else {

                        try {
                            ServerApi.Instance.R2Upload(
                                initResponse.signedUploadUrl, StoragePaths.GetRecordingPath(
                                    partNumber: queueEntry.partNumber,
                                    localRunId: queueEntry.localRunId,
                                    profileId: queueEntry.profileId
                                ),
                                onSuccess: () => {
                                    ServerApi.Instance.ApiPost<MarkUploadPartFinishedRequest, Empty>(
                                        path: "ingameupload/part/finish",
                                        body: new MarkUploadPartFinishedRequest() {
                                            ingameAuthId = authId,
                                            localRunId = queueEntry.localRunId,
                                            fileId = initResponse.fileId
                                        },
                                        onSuccess: response => OnSuccess(),
                                        onError: OnError
                                    ).StartGlobal();
                                },
                                onError: OnError
                            ).StartGlobal();
                        } catch (Exception ex) {
                            LogError("R2 upload failed");
                            LogError(ex);
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
                Log("Error while uploading to " + req.url);
                Log($"{req.result}");
                QueuesChanged?.Invoke();
            }

            void OnSuccess() {
                Log($"Successfully uploaded recording part {queueEntry.profileId} {queueEntry.partNumber}");
                queueEntry.finishedUploadAtUnixSeconds = RecordingFileManager.Instance.GetUnixSeconds();
                finishedUploadFiles.Add(queueEntry);
                queuedFiles.Remove(queueEntry);
                uploadInProgress = false;
                QueuesChanged?.Invoke();
            }
        }

        internal void DeleteAlreadyUploadedFiles() {
            for (int i = finishedUploadFiles.Count - 1; i >= 0; i--) {
                var queueEntry = finishedUploadFiles[i];
                var path = StoragePaths.GetRecordingPath(
                        partNumber: queueEntry.partNumber,
                        localRunId: queueEntry.localRunId,
                        profileId: queueEntry.profileId
                    );
                try {
                    File.Delete(path);
                    finishedUploadFiles.Remove(queueEntry);
                    QueuesChanged?.Invoke();
                } catch (Exception ex) {
                    Log($"Could not delete already uploaded analytics file {path} {ex.Message}");
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
    }
}
