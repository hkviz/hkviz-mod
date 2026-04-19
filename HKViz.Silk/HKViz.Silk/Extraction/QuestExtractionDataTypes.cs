using System;
using System.Collections.Generic;

namespace HKViz.Silk.Extraction;

[Serializable]
public class QuestData {
    public string? Id { get; set; }
    public string? RuntimeType { get; set; }
    public string? DisplayNameKey { get; set; }
    public string? TypeDisplayNameKey { get; set; }
    public Vector4Data? TypeTextColor { get; set; }
    public QuestTypeIconsData? TypeIcons { get; set; }
}

[Serializable]
public class QuestTypeIconsData {
    public SpriteInfo? Icon { get; set; }
    public SpriteInfo? CanCompleteIcon { get; set; }
    public SpriteInfo? LargeIcon { get; set; }
    public SpriteInfo? LargeIconGlow { get; set; }
}

[Serializable]
public class QuestExportData {
    public List<QuestData>? All { get; set; }
}


