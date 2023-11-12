using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        private StreamWriter? writer;

        // called when a save state is loaded or a new game is started
        public void StartRecorder() {
            StopRecorder();

            var recordingPath = StoragePaths.GetRecordingPath();

            bool existed = File.Exists(recordingPath);
            writer = new StreamWriter(recordingPath, append: true);

            if (!existed) {
                WriteEntry("recording-id", Guid.NewGuid().ToString());
            }
        }
        public void StopRecorder() {
            try {
                if (writer != null) {
                    BeforeWriterClose?.Invoke();
                    WriteEntry("session-end", "");
                    writer.Close();
                }
            } finally {
                writer = null;
            }
        }


        public void WriteEntryPrefix(string eventType) {
            // adding timestamp to each line
            writer?.Write(eventType);
            WriteSep();
            writer?.Write(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            writer?.Write(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            WriteSep();
        }

        public void WriteEntry(string eventType, string content) {
            WriteEntryPrefix(eventType);
            Write(content);
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
