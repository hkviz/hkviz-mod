using System;
using System.Collections.Generic;

namespace HKViz {
    // one user global settings per steam user
    [Serializable]
    public class UserGlobalSettings {
        public string? authId = null;
        public string? userName = null;
        public bool autoUpload = true;
        public bool showLoginButtonInMainMenu = true;
        public List<UploadQueueEntry> queuedUploadFiles = new List<UploadQueueEntry>();
        public List<UploadQueueEntry> failedUploadFiles = new List<UploadQueueEntry>();
        public List<UploadQueueEntry> finishedUploadFiles = new List<UploadQueueEntry>();
    }

    // inherits from userGlobal settings, for backwards compatibility to mod versions <= 1.5 where steam users where not considered
    [Serializable]
    public class GlobalSettings : UserGlobalSettings {
        // for settings since mod version 1.6 global settings are stored per steam user, so the login is not shared between them
        public Dictionary<string, UserGlobalSettings> perUser = new Dictionary<string, UserGlobalSettings>();
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
        public static UserGlobalSettings SettingsOfCurrentUser {
            get {
                var userId = GameLauncherUser.Instance.GetUserId();
                if (Instance._settings.perUser.TryGetValue(userId, out var userSettings)) {
                    return userSettings;
                } else {
                    var created = Instance.MakeNewGlobalSettingsOrGetFromDeprecated();
                    Instance._settings.perUser[userId] = created;
                    return created;
                }
            }
        }
        public GlobalSettings GetForSave() {
            return _settings;
        }

        private UserGlobalSettings MakeNewGlobalSettingsOrGetFromDeprecated() {
            return new UserGlobalSettings {
                authId = _settings.authId,
                userName = _settings.userName,
                autoUpload = _settings.autoUpload,
                showLoginButtonInMainMenu = _settings.showLoginButtonInMainMenu,
                queuedUploadFiles = _settings.queuedUploadFiles,
                failedUploadFiles = _settings.failedUploadFiles,
                finishedUploadFiles = _settings.finishedUploadFiles,
            };
        }

        public void InitializeFromSavedSettings(GlobalSettings settings) {
            _settings = settings;
            if (!_settings.perUser.ContainsKey(GameLauncherUser.Instance.GetUserId())) {
                // data from mod <= 1.5 is moved to per user settings
                _settings.perUser[GameLauncherUser.Instance.GetUserId()] = MakeNewGlobalSettingsOrGetFromDeprecated();
                // data is removed from global settings, so when switching user after it has been moved to a steam user
                // it is not used for any further steam users
                _settings.authId = null;
                _settings.userName = null;
                _settings.autoUpload = true;
                _settings.showLoginButtonInMainMenu = true;
                _settings.queuedUploadFiles = new List<UploadQueueEntry>();
                _settings.failedUploadFiles = new List<UploadQueueEntry>();
                _settings.finishedUploadFiles = new List<UploadQueueEntry>();
            }
        }
    }
}
