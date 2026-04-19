using System;
using System.Collections.Generic;

namespace HKViz.Silk.Extraction;

[Serializable]
public class CollectableRelicData {
    public string? Id { get; set; }
    public string? RuntimeType { get; set; }
    public string? TypeNameKey { get; set; }
    public string? TypeDescriptionKey { get; set; }
    public string? AppendDescriptionKey { get; set; }

    public SpriteInfo? CollectionIcon { get; set; }
    public SpriteInfo? PopupIcon { get; set; }

    public bool? IsInInventory { get; set; }
    public bool? IsPlayable { get; set; }
    public bool? PlaySyncedAudioSource { get; set; }
    public bool? WillSendPlayEvent { get; set; }

    public string? RelicTypeId { get; set; }
    public SpriteInfo? RelicTypeInventoryIcon { get; set; }
}

[Serializable]
public class CollectableRelicExportData {
    public List<CollectableRelicData>? All { get; set; }
}

