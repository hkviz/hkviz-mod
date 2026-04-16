using System;
using HKViz.Shared.Upload;
using HKViz.Upload;

namespace HKViz.Upload;

[Serializable]
public class HollowUploadQueueEntry : IUploadQueueEntry<HollowCreateUploadPartUrlRequest> {
    public string localRunId;
    public int partNumber;
    public int profileId;

    // metadata, so it can easily be displayed in the UI without parsing recording files
    public string? hkVersion;
    public float? playTime;
    public int? maxHealth;
    public int? mpReserveMax;
    public int? geo;
    public int? dreamOrbs;
    public int? permadeathMode;
    public string? mapZone;
    public bool? killedHollowKnight;
    public bool? killedFinalBoss;
    public bool? killedVoidIdol;
    public int? completionPercentage;
    public bool? unlockedCompletionRate;
    public bool? dreamNailUpgraded;
    public string? lastScene;

    public long firstUnixSeconds;
    public long lastUnixSeconds;

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

            hkVersion = hkVersion,
            playTime = playTime,
            maxHealth = maxHealth,
            mpReserveMax = mpReserveMax,
            geo = geo,
            dreamOrbs = dreamOrbs,
            permadeathMode = permadeathMode,
            mapZone = mapZone,
            killedHollowKnight = killedHollowKnight,
            killedFinalBoss = killedFinalBoss,
            killedVoidIdol = killedVoidIdol,
            completionPercentage = completionPercentage,
            unlockedCompletionRate = unlockedCompletionRate,
            dreamNailUpgraded = dreamNailUpgraded,
            lastScene = lastScene,
            firstUnixSeconds = firstUnixSeconds,
            lastUnixSeconds = lastUnixSeconds,
        };
    }
}