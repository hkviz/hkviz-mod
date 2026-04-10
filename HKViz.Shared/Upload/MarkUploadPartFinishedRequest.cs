using System;

namespace HKViz.Shared.Upload;

[Serializable]
internal class MarkUploadPartFinishedRequest {
    public string ingameAuthId;
    public string game = GameFlavor.GAME_ID;
    public string localRunId;
    public string fileId;
}
