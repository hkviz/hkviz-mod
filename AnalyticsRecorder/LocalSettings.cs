using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsRecorder {
    [Serializable]
    internal class LocalSettings {
        public Dictionary<string, string> previousPlayerData = new Dictionary<string, string>();
    }
}
