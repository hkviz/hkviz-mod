using System;

namespace HKViz.Shared.Upload;

[Serializable]
internal class MarkUploadPartFinishedRequest {
    public string ingameAuthId;
    public string localRunId;
    public string fileId;
}
