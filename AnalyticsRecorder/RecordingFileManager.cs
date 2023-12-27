using Modding;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace AnalyticsRecorder {
    internal class RecordingFileManager : Loggable {
        private static RecordingFileManager? _instance;
        public static RecordingFileManager Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new RecordingFileManager();
                return _instance;
            }
        }

        public event Action? BeforeCloseLastSessionFile;
        public event Action? AfterSwitchedFile;

        private long previousUnixMillis = 0;
        private float previousFullTimestampTime = -9999999999;
        private static float MAX_TIME_BETWEEN_FULL_TIME = 60; // maximum 1 minute without a full timestamp, just increase inbetween

        public string? localRunId;
        public int currentPart = 1;

        private float lastPartCreatedTime = 0;
        private float switchFileAfterSeconds = 60 * 5; // 2 minutes
        private bool isRecording = false;


        private StreamWriter? writer;

        public void Initialize() {
            // SwitchFileCoro().StartGlobal();
        }

        //private IEnumerator SwitchFileCoro() {
        //    while (true) {
        //        if (isRecording && (Time.unscaledTime - lastPartCreatedTime) > switchFileAfterSeconds ) {
        //            SwitchToNextPart();
        //        }
        //        // Log("SW " +  currentPart + isRecording);
        //        yield return null;
        //    }
        //}

        public void SwitchToNextPartIfNessessary() {
            if (isRecording && (Time.unscaledTime - lastPartCreatedTime) > switchFileAfterSeconds) {
                SwitchToNextPart();
            }
        }

        // called when a save state is loaded or a new game is started
        public void StartRecorder() {
            OnReturnToMenu();
            isRecording = true;
            StartPart();
        }

        public void OnReturnToMenu() {
            isRecording = false;
            try {
                if (writer != null) {
                    Log("hkviz recording stopped");
                    SwitchToNextPart();
                    BeforeCloseLastSessionFile?.Invoke();
                    WriteEntry(RecordingPrefixes.SESSION_END);
                }
            } finally {
                writer = null;
            }
            
        }

        public void InitFromLocalSave(LocalSettings localSettings) {
            Log("Init from local save in reocding file managergerger");
            localRunId = localSettings.localRunId ?? Guid.NewGuid().ToString();
            currentPart = localSettings.currentPart;
        }

        public void FinishPart(int partNumber, StreamWriter? writer) {
            writer?.Close();
            if (GlobalSettingsManager.Settings.autoUpload) {
                UploadManager.Instance.QueueFile(new UploadQueueEntry {
                    localRunId = localRunId,
                    partNumber = partNumber,
                    profileId = GameManager.instance.profileID,
                });
            }
        }

        public void StartPart() {
            Log("Start part" + currentPart);
            lastPartCreatedTime = Time.unscaledTime;
            var recordingPath = StoragePaths.GetRecordingPath(currentPart);

            bool existed = File.Exists(recordingPath);
            Log(recordingPath);
            writer = new StreamWriter(recordingPath, append: true);
            previousFullTimestampTime = -9999999999;

            if (!existed) {
                WriteEntry(RecordingPrefixes.RECORDING_ID, localRunId);
            }
            AfterSwitchedFile?.Invoke();
        }

        public void SwitchToNextPart() {
            if (!GlobalSettingsManager.Settings.autoUpload) {
                Log($"{nameof(SwitchToNextPart)} should not be called when autoUpload is not enabled");
            }
            var previousWriter = writer;
            var previousPart = currentPart;

            currentPart++;
            StartPart();
            FinishPart(previousPart, previousWriter);
        }

        public long GetUnixMillis() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


        private void WriteTimeInfo(long? unixTimeMillis) {
            var nowMillis = unixTimeMillis ?? GetUnixMillis();
            var diff = nowMillis - previousUnixMillis;
            var unscaledTime = Time.unscaledTime;

            if ((unscaledTime - previousFullTimestampTime) > MAX_TIME_BETWEEN_FULL_TIME) {
                previousFullTimestampTime = Time.unscaledTime;
                writer?.Write("=");
                writer?.Write(nowMillis.ToString());
            } else if(diff != 0) {
                writer?.Write("+");
                writer?.Write(diff.ToString());
            }

            previousUnixMillis = nowMillis;
        }


        public void WriteEntryPrefix(string eventType, bool withSeperator = true, long? unixMillis = null) {
            writer?.Write(eventType);
            WriteTimeInfo(unixMillis); // timeinfo contains = or + which is uses as seperator between eventType and time
            if (withSeperator) {
                WriteSep();
            }
        }

        public void WriteEntry(string eventType, string? content = null) {
            WriteEntryPrefix(eventType, withSeperator: content != null);
            if (content != null) {
                Write(content);
            }
            WriteNL();
        }

        public void Write(string content) {
            writer?.Write(content);
        }

        public void WriteNL() {
            writer?.WriteLine();
        }

        public void WriteSep() {
            writer?.Write(";");
        }
    }
}
