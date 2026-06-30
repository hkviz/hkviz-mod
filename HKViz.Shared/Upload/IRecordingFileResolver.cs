namespace HKViz.Shared.Upload;

public interface IUploadPathResolver<in TQueueEntry> {
    public string GetPath(TQueueEntry entry);
}
