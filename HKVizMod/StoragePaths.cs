using System.IO;
using UnityEngine;

namespace HKViz {
    internal static class StoragePaths {
        private static string GetRunFolderPath() {
            return Path.Combine(Application.persistentDataPath, "hkviz-recordings");
        }

        public static string GetUserFilePath(string suffix, int? profileId = null) {
            string recodingFileName = $"user{profileId ?? GameManager.instance.profileID}.{suffix}";
            return Path.Combine(Application.persistentDataPath, recodingFileName);
        }

        public static string GetRecordingPath(int partNumber, string localRunId, int? profileId = null) {
            var folder = MakeAndGetRunFolderIfNotExist();
            return Path.Combine(folder, $"user{profileId ?? GameManager.instance.profileID}.id_{localRunId}.part{partNumber}.hkpart");
        }

        private static string MakeAndGetRunFolderIfNotExist() {
            var path = GetRunFolderPath();
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(GetRunFolderPath());
            }
            return path;
        }
    }
}
