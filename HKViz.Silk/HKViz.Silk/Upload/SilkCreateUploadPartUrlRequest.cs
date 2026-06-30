using System;
using HKViz.Shared;

namespace HKViz.Silk.Upload;

[Serializable]
public class SilkCreateUploadPartUrlRequest {
    public string modVersion = "";
    public string game = GameFlavor.GAME_ID;

    public string ingameAuthId = "";
    public string localRunId = "";
    public int partNumber;

    // -- shared --
    // metadata, so it can easily be displayed in the UI without parsing recording files
    public float? playTime;
    public long firstUnixSeconds;
    public long lastUnixSeconds;

    // bools
    public bool? unlockedCompletionRate;

    // ints
    public int? completionPercentage;
    public int? maxHealth;
    public int? geo;
    public int? permadeathMode;

    // strings
    public string? lastScene;

    // -- game specific --
    public string? gameVersion;
    public string? mapZone;

    // bools
    public bool? endingAct2Regular;
    public bool? endingAct2Cursed;
    public bool? endingAct2SoulSnare;
    public bool? endingAct3;
    public bool? isAct3;

    // ints
    public int? shellShards;
    public int? silkSpoolParts;
    public int? extraRestZones;
    public int? belltownHouseColour;
    
    // strings
    public string? currentCrestId;
}