using System;
using System.Collections.Generic;

namespace HKViz.Silk.Extraction;

[Serializable]
public class TransitionGateData {
    public string? Id { get; set; }
}

[Serializable]
public class TransitionGateExportData {
    public List<TransitionGateData>? All { get; set; }
}


