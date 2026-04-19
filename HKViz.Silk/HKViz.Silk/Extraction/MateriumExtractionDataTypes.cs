using System;
using System.Collections.Generic;

namespace HKViz.Silk.Extraction;

[Serializable]
public class MateriumData {
    public string? Id { get; set; }
    public string? RuntimeType { get; set; }

    public string? DisplayNameKey { get; set; }
    public string? DescriptionKey { get; set; }

    public SpriteInfo? Icon { get; set; }

    public bool IsRequiredForCompletion { get; set; }
    public bool IsCollected { get; set; }
    public bool IsSeen { get; set; }
    public bool IsVisibleInCollection { get; set; }
}

[Serializable]
public class MateriumExportData {
    public bool ConstructedMaterium { get; set; }
    public List<MateriumData>? All { get; set; }
}

