using System;
using System.Collections.Generic;
using GlobalEnums;

namespace HKViz.Silk.Extraction;

[Serializable]
public class AreaBackgroundData {
    public string? NameOverride { get; set; }
    public bool Act3OverlayOptOut { get; set; }
    public SpriteInfo? BackgroundImage { get; set; }
    public SpriteInfo? Act3BackgroundImage { get; set; }
}

[Serializable]
public class SaveSlotBackgroundsData {
    public Dictionary<string, AreaBackgroundData>? AreaBackgrounds { get; set; }
    public Dictionary<string, AreaBackgroundData>? ExtraAreaBackgrounds { get; set; }
    public Dictionary<string, SpriteInfo>? BellhomeBackgrounds { get; set; }
}

