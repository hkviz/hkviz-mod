using HKViz.Shared.Upload;

namespace HKViz.Upload;

public class HollowUploadPathResolver: IUploadPathResolver {
    public string GetPath(UploadQueueEntry entry) {
        return StoragePaths.GetRecordingPath(
            partNumber: entry.partNumber,
            localRunId: entry.localRunId,
            profileId: entry.profileId
        );
    }
}
