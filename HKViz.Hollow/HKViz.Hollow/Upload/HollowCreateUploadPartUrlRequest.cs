using System;
using HKViz.Shared;

namespace HKViz.Upload;

[Serializable]
public class HollowCreateUploadPartUrlRequest {
    public string modVersion = "";
    public string game = GameFlavor.GAME_ID;

    public string ingameAuthId = "";
    public string localRunId = "";
    public int partNumber;

    public string? hkVersion;

    // -- shared --
    // metadata, so it can easily be displayed in the UI without parsing recording files
    public float? playTime;
    public long firstUnixSeconds;
    public long lastUnixSeconds;

    public bool? unlockedCompletionRate;
    public int? completionPercentage;
    public int? maxHealth;
    public int? geo;
    public int? permadeathMode;
    public string? lastScene;

    // -- game specific --
    public string? mapZone;
    
    // bool
    public bool? killedHollowKnight;
    public bool? killedFinalBoss;
    public bool? killedVoidIdol;
    public bool? dreamNailUpgraded;
    
    // int
    public int? dreamOrbs;
    public int? mpReserveMax;
}