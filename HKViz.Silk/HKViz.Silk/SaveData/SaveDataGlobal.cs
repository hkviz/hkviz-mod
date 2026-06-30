using System.Collections.Generic;
using HKViz.Silk.Upload;

namespace HKViz.Silk.SaveData;

public class SaveDataGlobal {
    public readonly string? authId;
    public readonly string? userName;
    public readonly bool autoUpload;
    public readonly bool showLoginButtonInMainMenu;
    public readonly List<SilkUploadQueueEntry> queuedUploadFiles;
    public readonly List<SilkUploadQueueEntry> failedUploadFiles;
    public readonly List<SilkUploadQueueEntry> finishedUploadFiles;

    public SaveDataGlobal(
        string? authId,
        string? userName,
        bool autoUpload,
        bool showLoginButtonInMainMenu,
        List<SilkUploadQueueEntry> queuedUploadFiles,
        List<SilkUploadQueueEntry> failedUploadFiles,
        List<SilkUploadQueueEntry> finishedUploadFiles
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
