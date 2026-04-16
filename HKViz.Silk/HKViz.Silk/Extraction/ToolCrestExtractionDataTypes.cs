using System;
using System.Collections.Generic;

namespace HKViz.Silk.Extraction;

[Serializable]
public class ToolCrestData {
    public string? Name { get; set; }
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public SpriteInfo? CrestSprite { get; set; }
}

[Serializable]
public class ToolCrestExportData {
    public List<ToolCrestData>? Crests { get; set; }
}
