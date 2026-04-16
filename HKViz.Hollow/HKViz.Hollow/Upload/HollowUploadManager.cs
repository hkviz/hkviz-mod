using HKViz.Shared;
using HKViz.Shared.Auth;
using HKViz.Shared.Upload;

namespace HKViz.Upload;

public class HollowUploadManager : UploadManager<HollowUploadQueueEntry, HollowCreateUploadPartUrlRequest> {
    public HollowUploadManager(ServerApi serverApi, AuthManager authManager, IHkVizLogger logger)
        : base(serverApi, authManager, logger, new HollowUploadPathResolver()) {
    }
}

