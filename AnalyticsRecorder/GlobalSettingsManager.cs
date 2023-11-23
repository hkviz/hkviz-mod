using MapChanger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AnalyticsRecorder.UploadManager;

namespace AnalyticsRecorder {
    public class GlobalSettings {
        public string? authId = null;
        public string? userName = null;
        public bool autoUpload = true;
        public bool showLoginButtonInMainMenu = true;
        public List<UploadQueueEntry> queuedUploadFiles = new List<UploadQueueEntry>();
        public List<UploadQueueEntry> failedUploadFiles = new List<UploadQueueEntry>();
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
            BeforeSave?.Invoke();
            return Settings;
        }

        public void InitializeFromSavedSettings(GlobalSettings settings) {
            _settings = settings;
            SettingsLoaded?.Invoke();
        }

        public event Action? SettingsLoaded;
        public event Action? BeforeSave;

    }
}
