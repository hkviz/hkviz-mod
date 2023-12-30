using System;
using System.Collections.Generic;

namespace HKViz {
    [Serializable]
    internal class LocalSettings {
        public Dictionary<string, string> previousPlayerData = new Dictionary<string, string>();
        public string? localRunId;
        public int currentPart = 1;
    }
}
