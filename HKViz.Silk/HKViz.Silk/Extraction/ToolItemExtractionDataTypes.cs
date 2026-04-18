using System;
using System.Collections.Generic;

namespace HKViz.Silk.Extraction;

[Serializable]
public class ToolItemData {
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Type { get; set; }
    public SpriteInfo? ToolSprite { get; set; }
    public PlayerDataTestData? AlternateUnlockedTest { get; set; }
}

[Serializable]
public class ToolItemExportData {
    public List<ToolItemData>? All { get; set; }
}

