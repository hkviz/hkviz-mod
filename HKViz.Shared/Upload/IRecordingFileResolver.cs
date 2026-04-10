namespace HKViz.Shared.Upload;

public interface IUploadPathResolver {
    public string GetPath(UploadQueueEntry entry);
}
