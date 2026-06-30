using HKViz.Shared.Upload;

namespace HKViz.Upload;

public class HollowUploadPathResolver: IUploadPathResolver<HollowUploadQueueEntry> {
    public string GetPath(HollowUploadQueueEntry entry) {
        return StoragePaths.GetRecordingPath(
            partNumber: entry.partNumber,
            localRunId: entry.localRunId,
            profileId: entry.profileId
        );
    }
}
