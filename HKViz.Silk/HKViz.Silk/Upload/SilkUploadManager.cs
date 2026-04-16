using HKViz.Shared;
using HKViz.Shared.Auth;
using HKViz.Shared.Upload;

namespace HKViz.Silk.Upload;

public class SilkUploadManager : UploadManager<SilkUploadQueueEntry, SilkCreateUploadPartUrlRequest> {
    public SilkUploadManager(ServerApi serverApi, AuthManager authManager, IHkVizLogger logger)
        : base(serverApi, authManager, logger, new SilkUploadPathResolver()) {
    }
}

