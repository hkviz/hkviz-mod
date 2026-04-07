using UnityEngine;

namespace HKViz.Shared;

public static class PlatformFlavor {
#if MOD_HOLLOW
    public const string Name = "Hollow";
    public const bool IsHollow = true;
    public const bool IsSilk = false;
#elif MOD_SILK
    public const string Name = "Silk";
    public const bool IsHollow = false;
    public const bool IsSilk = true;
#else
    public const string Name = "Unknown";
    public const bool IsHollow = false;
    public const bool IsSilk = false;
#endif
    
    private static Vector3 _vector3 = new Vector3(1, 2, 3);
    private static Vector2 _vector2 = new Vector2(1, 2);
}

