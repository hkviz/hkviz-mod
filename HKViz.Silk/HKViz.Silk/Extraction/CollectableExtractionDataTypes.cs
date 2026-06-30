using System;
using System.Collections.Generic;

namespace HKViz.Silk.Extraction;

[Serializable]
public class CollectableData {
    public string? Id { get; set; }
    public string? RuntimeType { get; set; }

    public string? DisplayNameKey { get; set; }
    public string? DescriptionKey { get; set; }
    public string? UseResponseTextOverrideKey { get; set; }
    public List<string>? UseResponseDescriptionKeys { get; set; }

    public Dictionary<string, List<string>>? DisplayNameKeysBySource { get; set; }
    public Dictionary<string, List<string>>? DescriptionKeysBySource { get; set; }
    public List<string>? ExtraDescriptionKeys { get; set; }

    public SpriteInfo? IconInventory { get; set; }
    public SpriteInfo? IconPopup { get; set; }
    public SpriteInfo? IconTiny { get; set; }
    public SpriteInfo? IconShop { get; set; }
    public SpriteInfo? IconTakePopup { get; set; }

    public bool DisplayAmount { get; set; }
    public bool IsConsumable { get; set; }
    public bool IsVisibleWithBareInventory { get; set; }
    public bool HideInShopCounters { get; set; }
    public bool TakeItemOnConsume { get; set; }
    public List<CollectableUseResponseData>? UseResponses { get; set; }
}

[Serializable]
public class CollectableUseResponseData {
    public string? SourceKind { get; set; }
    public int? StateIndex { get; set; }

    public string? UseType { get; set; }
    public int Amount { get; set; }
    public int AmountRangeStart { get; set; }
    public int AmountRangeEnd { get; set; }
    public string? DescriptionKey { get; set; }
}

[Serializable]
public class CollectableExportData {
    public List<CollectableData>? All { get; set; }
}

