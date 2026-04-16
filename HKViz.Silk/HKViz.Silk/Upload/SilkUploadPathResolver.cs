using System;
using HKViz.Shared.Upload;
using HKViz.Silk.Recording;

namespace HKViz.Silk.Upload;

public class SilkUploadPathResolver: IUploadPathResolver<SilkUploadQueueEntry> {
    public string GetPath(SilkUploadQueueEntry entry) {
        return RunFilePaths.GetRecordingPath(
            partNumber: entry.partNumber,
            localRunId: Guid.Parse(entry.localRunId),
            profileId: entry.profileId
        );
    }
}
