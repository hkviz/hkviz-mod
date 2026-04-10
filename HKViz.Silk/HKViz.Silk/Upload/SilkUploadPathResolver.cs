using HKViz.Shared.Upload;
using HKViz.Silk.Recording;

namespace HKViz.Silk.Upload;

public class SilkUploadPathResolver: IUploadPathResolver {
    public string GetPath(UploadQueueEntry entry) {
        return RunFilePaths.GetRecordingPath(
            partNumber: entry.partNumber,
            localRunId: entry.localRunId
        );
    }
}
