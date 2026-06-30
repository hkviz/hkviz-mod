using System;
using HKViz.Shared.Upload;

namespace HKViz.Silk.Upload;

[Serializable]
public class SilkUploadQueueEntry : IUploadQueueEntry<SilkCreateUploadPartUrlRequest> {
    public string localRunId = "";
    public int partNumber;
    public int profileId;

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

    public long finishedUploadAtUnixSeconds;

    
    
    
    public long FinishedUploadAtUnixSeconds {
        get => finishedUploadAtUnixSeconds;
        set => finishedUploadAtUnixSeconds = value;
    }
    public string LocalRunId => localRunId;
    public int ProfileId => profileId;
    public long PartNumber => partNumber;


    public SilkCreateUploadPartUrlRequest ToUploadRequest(string ingameAuthId, string modVersion) {
        return new SilkCreateUploadPartUrlRequest {
            modVersion = modVersion,
            ingameAuthId = ingameAuthId,
            localRunId = localRunId,
            partNumber = partNumber,

            // -- shared --
            playTime = playTime,
            firstUnixSeconds = firstUnixSeconds,
            lastUnixSeconds = lastUnixSeconds,

            unlockedCompletionRate = unlockedCompletionRate,
            completionPercentage = completionPercentage,
            maxHealth = maxHealth,
            geo = geo,
            permadeathMode = permadeathMode,
            lastScene = lastScene,
            
            // strings
            gameVersion = gameVersion,

            // -- game specific --
            mapZone = mapZone,

            // bools
            endingAct2Regular = endingAct2Regular,
            endingAct2Cursed = endingAct2Cursed,
            endingAct2SoulSnare = endingAct2SoulSnare,
            endingAct3 = endingAct3,
            isAct3 = isAct3,

            // ints
            shellShards = shellShards,
            silkSpoolParts = silkSpoolParts,
            extraRestZones = extraRestZones,
            belltownHouseColour = belltownHouseColour,
            
            // strings
            currentCrestId = currentCrestId,
        };
    }
}