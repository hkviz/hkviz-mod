using System;
using System.Collections.Generic;

namespace HKViz.Silk.Extraction;

[Serializable]
public class SceneDataFieldData {
    public string SceneName;
    public string Id;
}

[Serializable]
public class SceneDataExportData {
    public List<SceneDataFieldData> Int { get; set; } = [];
    public List<SceneDataFieldData> Bool { get; set; } = [];
    public List<SceneDataFieldData> GeoRock { get; set; } = [];
}

