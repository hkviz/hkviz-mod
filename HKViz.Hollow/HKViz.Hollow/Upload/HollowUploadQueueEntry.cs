using System;
using HKViz.Shared.Upload;

namespace HKViz.Upload;

[Serializable]
public class HollowUploadQueueEntry : IUploadQueueEntry<HollowCreateUploadPartUrlRequest> {
    public string localRunId = "";
    public int partNumber;
    public int profileId;

    // -- shared --
    // metadata, so it can easily be displayed in the UI without parsing recording files
    public float? playTime;
    public long firstUnixSeconds;
    public long lastUnixSeconds;

    public string? hkVersion;

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

    public long finishedUploadAtUnixSeconds;

    
    
    
    public long FinishedUploadAtUnixSeconds {
        get => finishedUploadAtUnixSeconds;
        set => finishedUploadAtUnixSeconds = value;
    }
    public string LocalRunId => localRunId;
    public int ProfileId => profileId;
    public long PartNumber => partNumber;


    public HollowCreateUploadPartUrlRequest ToUploadRequest(string ingameAuthId, string modVersion) {
        return new HollowCreateUploadPartUrlRequest {
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

            // -- game specific --
            hkVersion = hkVersion,
            mapZone = mapZone,
            killedHollowKnight = killedHollowKnight,
            killedFinalBoss = killedFinalBoss,
            killedVoidIdol = killedVoidIdol,
            dreamNailUpgraded = dreamNailUpgraded,
            dreamOrbs = dreamOrbs,
            mpReserveMax = mpReserveMax,
        };
    }
}