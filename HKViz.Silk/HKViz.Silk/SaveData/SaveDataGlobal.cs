using System.Collections.Generic;
using HKViz.Shared.Upload;

namespace HKViz.Silk.SaveData;

public class SaveDataGlobal {
    public readonly string? authId = null;
    public readonly string? userName = null;
    public readonly bool autoUpload = true;
    public readonly bool showLoginButtonInMainMenu = true;
    public readonly List<UploadQueueEntry> queuedUploadFiles;
    public readonly List<UploadQueueEntry> failedUploadFiles;
    public readonly List<UploadQueueEntry> finishedUploadFiles;

    public SaveDataGlobal(
        string? authId,
        string? userName,
        bool autoUpload,
        bool showLoginButtonInMainMenu,
        List<UploadQueueEntry> queuedUploadFiles,
        List<UploadQueueEntry> failedUploadFiles,
        List<UploadQueueEntry> finishedUploadFiles
    ) {
        this.authId = authId;
        this.userName = userName;
        this.autoUpload = autoUpload;
        this.showLoginButtonInMainMenu = showLoginButtonInMainMenu;
        this.queuedUploadFiles = queuedUploadFiles;
        this.failedUploadFiles = failedUploadFiles;
        this.finishedUploadFiles = finishedUploadFiles;
    }
}
