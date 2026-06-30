

namespace HKViz.Shared.Upload;

public interface IUploadQueueEntry<out TUploadRequest> {
    public long FinishedUploadAtUnixSeconds { get; set; }
    public string LocalRunId  { get; }
    public int ProfileId  { get; }
    public long PartNumber { get; }

    public TUploadRequest ToUploadRequest(string ingameAuthId, string modVersion);
}
