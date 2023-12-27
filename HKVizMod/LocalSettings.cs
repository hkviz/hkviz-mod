using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKViz {
    [Serializable]
    internal class LocalSettings {
        public Dictionary<string, string> previousPlayerData = new Dictionary<string, string>();
        public string? localRunId;
        public int currentPart = 1;
    }
}
