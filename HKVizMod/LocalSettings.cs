using System;
using System.Collections.Generic;

namespace HKViz {
    [Serializable]
    internal class UserLocalSettings {
        public Dictionary<string, string> previousPlayerData = new Dictionary<string, string>();
        public string? localRunId;
        public int currentPart = 1;
    }

    // inherits from user settings for backwards compatibility of mod versions <= 1.5
    [Serializable]
    internal class LocalSettings: UserLocalSettings {
        public Dictionary<string, UserLocalSettings> perUser = new Dictionary<string, UserLocalSettings>();

        public UserLocalSettings FromDeprecatedFields() {
            return new UserLocalSettings() {
                previousPlayerData = this.previousPlayerData,
                localRunId = this.localRunId,
                currentPart = this.currentPart,
            };
        }

        public void ResetDeprecatedSettings() {
            previousPlayerData = new Dictionary<string, string>();
            localRunId = null;
            currentPart = 1;
        }
    }
}
