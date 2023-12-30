using System.Collections.Generic;

namespace HKViz {
    public class GlobalSettings {
        public string? authId = null;
        public string? userName = null;
        public bool autoUpload = true;
        public bool showLoginButtonInMainMenu = true;
        public List<UploadQueueEntry> queuedUploadFiles = new List<UploadQueueEntry>();
        public List<UploadQueueEntry> failedUploadFiles = new List<UploadQueueEntry>();
        public List<UploadQueueEntry> finishedUploadFiles = new List<UploadQueueEntry>();
    }
    internal class GlobalSettingsManager {
        private static GlobalSettingsManager instance;
        public static GlobalSettingsManager Instance {
            get {
                if (instance == null) {
                    instance = new GlobalSettingsManager();
                }
                return instance;
            }
        }

        private GlobalSettings _settings = new GlobalSettings();
        public static GlobalSettings Settings => Instance._settings;

        public GlobalSettings GetForSave() {
            return Settings;
        }

        public void InitializeFromSavedSettings(GlobalSettings settings) {
            _settings = settings;
        }
    }
}
