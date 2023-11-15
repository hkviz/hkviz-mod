using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace AnalyticsRecorder {
    internal class RecordingFileManager {
        private static RecordingFileManager? _instance;
        public static RecordingFileManager Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new RecordingFileManager();
                return _instance;
            }
        }

        public event Action? BeforeWriterClose;

        private long previousUnixMillis = 0;
        private float previousFullTimestampTime = -9999999999;
        private static float MAX_TIME_BETWEEN_FULL_TIME = 60; // maximum 1 minute without a full timestamp, just increase inbetween


        private StreamWriter? writer;

        // called when a save state is loaded or a new game is started
        public void StartRecorder() {
            StopRecorder();

            var recordingPath = StoragePaths.GetRecordingPath();

            bool existed = File.Exists(recordingPath);
            writer = new StreamWriter(recordingPath, append: true);

            if (!existed) {
                WriteEntry(RecordingPrefixes.RECORDING_ID, Guid.NewGuid().ToString());
            }
        }
        public void StopRecorder() {
            try {
                if (writer != null) {
                    BeforeWriterClose?.Invoke();
                    WriteEntry(RecordingPrefixes.SESSION_END);
                    writer.Close();
                }
            } finally {
                writer = null;
            }
        }
        
        private void WriteTimeInfo() {
            var nowMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
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


        public void WriteEntryPrefix(string eventType, bool withSeperator = true) {
            writer?.Write(eventType);
            WriteTimeInfo(); // timeinfo contains = or + which is uses as seperator between eventType and time
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
