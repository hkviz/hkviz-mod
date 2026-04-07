using System.IO;
using Silksong.DataManager;

namespace HKViz.Silk.Recording;

public static class RunFilePaths {
    private static string GetRunFolderPath() {
        return Path.Combine(DataPaths.GlobalDataDir, "org.hkviz.silk.recordings");
    }

    public static string GetRecordingPath(string localRunId, long partNumber) {
        var folder = MakeAndGetRunFolderIfNotExist();
        return Path.Combine(folder, $"user{GameManager.instance.profileID}.id_{localRunId}.part{partNumber}.hkviz-silk");
    }

    private static string MakeAndGetRunFolderIfNotExist() {
        var path = GetRunFolderPath();
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        return path;
    }
}