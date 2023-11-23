using System.IO;
using UnityEngine;

namespace AnalyticsRecorder {
    internal class StoragePaths {
        public static string GetUserFilePath(string suffix, int? profileId = null) {
            string recodingFileName = $"user{profileId ?? GameManager.instance.profileID}.{suffix}";
            return Path.Combine(Application.persistentDataPath, recodingFileName);
        }

        public static string GetRecordingPath(int partNumber, int? profileId = null) {
            return GetUserFilePath($"part{partNumber}.analytics.hkrun", profileId);
        }
    }
}
