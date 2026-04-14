using System;

namespace HKViz.Shared.Upload;

[Serializable]
public class UploadQueueEntry {
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
}