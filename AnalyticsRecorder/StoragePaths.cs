using System.IO;
using UnityEngine;

namespace AnalyticsRecorder {
    internal class StoragePaths {
        public static string GetUserFilePath(string suffix) {
            string recodingFileName = $"user{GameManager.instance.profileID}.{suffix}";
            return Path.Combine(Application.persistentDataPath, recodingFileName);
        }

        public static string GetRecordingPath() {
            return GetUserFilePath("analytics.hkrun");
        }
    }
}
