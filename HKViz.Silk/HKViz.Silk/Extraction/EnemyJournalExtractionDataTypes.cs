using System;
using System.Collections.Generic;

namespace HKViz.Silk.Extraction;

[Serializable]
public class EnemyJournalData {
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? RecordType { get; set; }
    public SpriteInfo? IconSprite { get; set; }
    public SpriteInfo? EnemySprite { get; set; }
    public int KillsRequired { get; set; }
    public bool IsAlwaysUnlocked { get; set; }
    public bool IsRequiredForCompletion { get; set; }
    public List<string>? CompleteOthers { get; set; }
}

[Serializable]
public class EnemyJournalExportData {
    public List<EnemyJournalData>? All { get; set; }
}

