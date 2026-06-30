using System;

namespace HKViz.Shared.Upload;

public interface IUploadManager {
    public event Action? QueuesChanged;

    public void Initialize();
    public void RetryFailedUploads();
    public void DeleteAlreadyUploadedFiles();

    public int QueuedFilesQueueCount();
    public int FinishedUploadFilesQueueCount();
    public int FailedUploadsQueueCount();
}

