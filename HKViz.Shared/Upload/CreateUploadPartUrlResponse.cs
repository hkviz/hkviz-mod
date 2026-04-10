using System;

namespace HKViz.Shared.Upload;

[Serializable]
internal class CreateUploadPartUrlResponse {
    public string fileId;
    public string runId;
    public string signedUploadUrl;
    public bool alreadyFinished;
}