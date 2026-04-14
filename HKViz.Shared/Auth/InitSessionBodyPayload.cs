using System;

namespace HKViz.Shared.Auth;

[Serializable]
internal class InitSessionBodyPayload {
    public string modVersion;
    public string game = GameFlavor.GAME_ID;
}
