using UnityEngine;

namespace HKViz.Shared;

public static class GameFlavor {
#if MOD_HOLLOW
    public const string GAME_ID = "hollow";
#elif MOD_SILK
    public const string GAME_ID = "silk";
#endif
}

