using Modding;
using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace HKViz {
    internal class RecordingFileManager : Loggable {
        private static string RECORDER_FILE_VERSION = "1.5.0";
        private static RecordingFileManager? _instance;
        public static RecordingFileManager Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new RecordingFileManager();
                return _instance;
            }
        }

        private RecordingSerializer serializer = RecordingSerializer.Instance;

        public event Action? BeforeCloseLastSessionFile;
        public event Action? AfterSwitchedFile;

        private long previousUnixMillis = 0;
        private float previousFullTimestampTime = -9999999999;
        private static float MAX_TIME_BETWEEN_FULL_TIME = 60; // maximum 1 minute without a full timestamp, just increase inbetween

        public string? localRunId;
        public int currentPart = 1;

        public string? lastScene = null;

        private float lastPartCreatedTime = 0;
        private float switchFileAfterSeconds = 60 * 5; // 5 minutes
        public bool isRecording { get; private set; } = false;

        private long currentFileFirstUnixSeconds = 0;


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
        public float GetNextPartInSeconds() => Math.Max(0, switchFileAfterSeconds - GetLastPartAgoSeconds());
        public float GetLastPartAgoSeconds() => (Time.unscaledTime - lastPartCreatedTime);

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

        public void InitFromLocalSave(UserLocalSettings localSettings) {
            Log("Init from local save in reocding file managergerger");
            localRunId = localSettings.localRunId ?? Guid.NewGuid().ToString();
            currentPart = localSettings.currentPart;
        }

        public void FinishPart(int partNumber, StreamWriter? writer, long partFirstUnixMillis) {
            string? pd(string key) {
                var hasValue = PlayerDataWriter.Instance.previousPlayerData.TryGetValue(key, out var value);
                return hasValue ? value : null;
            }

            bool? pdBool(string key) {
                var str = pd(key);
                if (str == null) return null;
                return str == "1";
            }

            int? pdInt(string key) {
                var str = pd(key);
                if (str == null) return null;
                if (int.TryParse(str, NumberStyles.Integer, RecordingSerializer.cultureInfo, out var value)) return value;
                return null;
            }
            float? pdFloat(string key) {
                var str = pd(key);
                if (str == null) return null;
                if (float.TryParse(str, NumberStyles.Number, RecordingSerializer.cultureInfo, out var value)) return value;
                return null;
            }


            writer?.Close();
            if (GlobalSettingsManager.SettingsOfCurrentUser.autoUpload) {
                UploadManager.Instance.QueueFile(new UploadQueueEntry {
                    localRunId = localRunId,
                    partNumber = partNumber,
                    profileId = GameManager.instance.profileID,

                    hkVersion = pd("version"),
                    playTime = pdFloat("playTime"),
                    maxHealth = pdInt("maxHealth"),
                    mpReserveMax = pdInt("MPReserveMax"),
                    geo = pdInt("geo"),
                    dreamOrbs = pdInt("dreamOrbs"),
                    permadeathMode = pdInt("permadeathMode"),
                    mapZone = pd("mapZone"),
                    killedHollowKnight = pdBool("killedHollowKnight"),
                    killedFinalBoss = pdBool("killedFinalBoss"),
                    killedVoidIdol = (pdBool("killedVoidIdol_1") ?? false) || (pdBool("killedVoidIdol_2") ?? false),
                    completionPercentage = pdInt("completionPercentage"),
                    unlockedCompletionRate = pdBool("unlockedCompletionRate"),



                    dreamNailUpgraded = pdBool("dreamNailUpgraded"),
                    lastScene = lastScene,

                    firstUnixSeconds = partFirstUnixMillis,
                    lastUnixSeconds = GetUnixSeconds(),
                });
            }
        }

        public void StartPart() {
            Log("Start part" + currentPart);
            currentFileFirstUnixSeconds = GetUnixSeconds();
            lastPartCreatedTime = Time.unscaledTime;
            var recordingPath = StoragePaths.GetRecordingPath(currentPart, localRunId: localRunId);

            bool existed = File.Exists(recordingPath);
            Log(recordingPath);
            writer = new StreamWriter(recordingPath, append: true);
            previousFullTimestampTime = -9999999999;

            // always write new line at beginning, bc last session might have failed finishing writing
            // and we would just continue writing on the errored line.
            WriteNL();
            WriteEntry(RecordingPrefixes.RECORDING_ID, localRunId);
            WriteEntry(RecordingPrefixes.RECORDING_FILE_VERSION, RECORDER_FILE_VERSION);
            //}
            AfterSwitchedFile?.Invoke();
        }

        public void SwitchToNextPart() {
            if (!GlobalSettingsManager.SettingsOfCurrentUser.autoUpload) {
                Log($"{nameof(SwitchToNextPart)} should not be called when autoUpload is not enabled");
            }
            var previousWriter = writer;
            var previousPart = currentPart;
            var previousPartFirstUnixMillis = currentFileFirstUnixSeconds;

            currentPart++;
            StartPart();
            FinishPart(previousPart, previousWriter, previousPartFirstUnixMillis);
        }

        public long GetUnixMillis() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public long GetUnixSeconds() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();


        private void WriteTimeInfo(long? unixTimeMillis) {
            var nowMillis = unixTimeMillis ?? GetUnixMillis();
            var diff = nowMillis - previousUnixMillis;
            var unscaledTime = Time.unscaledTime;

            if ((unscaledTime - previousFullTimestampTime) > MAX_TIME_BETWEEN_FULL_TIME) {
                previousFullTimestampTime = Time.unscaledTime;
                writer?.Write("=");
                writer?.Write(nowMillis.ToString());
            } else if (diff != 0) {
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

        public void WriteEntry(string eventType, string? content = null, long? unixMillis = null) {
            WriteEntryPrefix(eventType, withSeperator: content != null, unixMillis: unixMillis);
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
