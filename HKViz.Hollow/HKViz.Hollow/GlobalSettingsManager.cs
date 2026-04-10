using System;
using System.Collections.Generic;
using HKViz.Shared.Auth;
using HKViz.Shared.Upload;
using Modding;

namespace HKViz;

// one user global settings per steam user
[Serializable]
public class UserGlobalSettings {
    public string? authId = null;
    public string? userName = null;
    public bool autoUpload = true;
    public bool showLoginButtonInMainMenu = true;
    public List<UploadQueueEntry> queuedUploadFiles = [];
    public List<UploadQueueEntry> failedUploadFiles = [];
    public List<UploadQueueEntry> finishedUploadFiles = [];
}

// inherits from userGlobal settings, for backwards compatibility to mod versions <= 1.5 where steam users where not considered
[Serializable]
public class GlobalSettings : UserGlobalSettings {
    // for settings since mod version 1.6 global settings are stored per steam user, so the login is not shared between them
    public Dictionary<string, UserGlobalSettings> perUser = new Dictionary<string, UserGlobalSettings>();
}
internal class GlobalSettingsManager: Loggable {
    private static GlobalSettingsManager instance;
    public static GlobalSettingsManager Instance {
        get {
            if (instance == null) {
                instance = new GlobalSettingsManager();
            }
            return instance;
        }
    }

    private GlobalSettings _settings = new();
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

    private UploadManager uploadManager = HkVizSharedInstances.Instance!.uploadManager;
    private AuthManager authManager = HkVizSharedInstances.Instance.authManager;

    public GlobalSettingsManager() {
        authManager.StateChanged += AuthManager_StateChanged;
    }

    private void AuthManager_StateChanged(LoginState obj) {
        authManager.StateChanged += AuthStateChanged;
    }

    private void AuthStateChanged(LoginState obj) {
        //if (_state == LoginState.LOGGED_IN) {
        SettingsOfCurrentUser.userName = authManager.UserName;
        SettingsOfCurrentUser.authId = authManager.AuthId;
        //} else {
        //    GlobalSettingsManager.Settings.userName = null;
        //    GlobalSettingsManager.Settings.authId = null;
        //}
    }

    public GlobalSettings GetForSave() {
        SettingsOfCurrentUser.queuedUploadFiles = uploadManager.queuedFiles;
        SettingsOfCurrentUser.failedUploadFiles = uploadManager.failedUploads;
        SettingsOfCurrentUser.finishedUploadFiles = uploadManager.finishedUploadFiles;
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
            _settings.queuedUploadFiles = [];
            _settings.failedUploadFiles = [];
            _settings.finishedUploadFiles = [];
        }

        Log("GS loaded upload manager" + (SettingsOfCurrentUser.queuedUploadFiles.Count + "-" + SettingsOfCurrentUser.failedUploadFiles.Count));
        authManager.InitializeFromGlobalSettings(SettingsOfCurrentUser.authId, SettingsOfCurrentUser.userName);
        uploadManager.AddUploadEntries(
            queuedFiles: SettingsOfCurrentUser.queuedUploadFiles,
            failedUploads: SettingsOfCurrentUser.failedUploadFiles,
            finishedUploadFiles: SettingsOfCurrentUser.finishedUploadFiles
        );
    }
}
