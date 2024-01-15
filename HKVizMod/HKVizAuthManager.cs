using Modding;
using System;
using UnityEngine;

namespace HKViz {
    [Serializable]
    internal class InitSessionBodyPayload {
        public string modVersion;
    }

    [Serializable]
    internal class InitSessionResult {
        public string id;
        public string urlId;
    }

    [Serializable]
    internal class UserInfo {
        public string id;
        public string name;

    }

    [Serializable]
    internal class SessionInfo {
        public string id;
        public string name;
        public UserInfo? user;

    }

    [Serializable]
    internal class Empty { }

    internal class HKVizAuthManager : Loggable {
        private static HKVizAuthManager instance;

        public static HKVizAuthManager Instance {
            get {
                if (instance == null) {
                    instance = new HKVizAuthManager();
                }
                return instance;
            }
        }

        public enum LoginState {
            NOT_LOGGED_IN,
            LOADING_LOGIN_URL,
            LOADING_LOGIN_URL_FAILED,
            WAITING_FOR_USER_LOGIN_IN_BROWSER,
            LOADING_AUTH_STATE_FAILED,
            LOGGED_IN,
        }

        public string? AuthId;
        public string? UserName;

        private float loggedInAt = -100000;
        private bool isUserTriggeredLoginFlow = false;
        private float displayLoginSuccessForSeconds = 4;
        public bool ShowLoginSuccess => (Time.unscaledTime - loggedInAt) < displayLoginSuccessForSeconds;

        public string GetVersion() => GetType().Assembly.GetName().Version.ToString();


        private LoginState _state = LoginState.NOT_LOGGED_IN;


        public LoginState State {
            get {
                return _state;
            }
            private set {
                _state = value;

                //if (_state == LoginState.LOGGED_IN) {
                GlobalSettingsManager.Settings.userName = UserName;
                GlobalSettingsManager.Settings.authId = AuthId;
                //} else {
                //    GlobalSettingsManager.Settings.userName = null;
                //    GlobalSettingsManager.Settings.authId = null;
                //}

                StateChanged?.Invoke(value);
            }
        }

        public event Action<LoginState>? StateChanged;

        private HKVizAuthManager() {
            Application.focusChanged += Application_focusChanged;
        }

        public void GlobalSettingsLoaded() {
            AuthId = GlobalSettingsManager.Settings.authId;
            UserName = GlobalSettingsManager.Settings.userName;
            CheckSessionState(fromSettings: true);
        }

        private void Application_focusChanged(bool focused) {
            Log("Focus changed");
            if (focused && State == LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER) {
                CheckSessionState(fromSettings: false);
            }
        }

        public void Login() {
            Log("Login");
            InitSessionToken();
        }

        public void Logout() {
            State = LoginState.NOT_LOGGED_IN;
            AuthId = null;

            if (AuthId != null) {
                ServerApi.Instance.ApiDelete<Empty>(
                    path: "ingameauth/" + AuthId,
                    onSuccess: data => {
                        Log("Deleted session in backend");
                    },
                    onError: request => {
                        Log("Session in backend not deleted");
                        Log(request.error);
                        Log(request.downloadHandler.text);
                    }
                ).StartGlobal();
            }
        }

        private void InitSessionToken() {
            State = LoginState.LOADING_LOGIN_URL;
            ServerApi.Instance.ApiPost<InitSessionBodyPayload, InitSessionResult>(
                path: "ingameauth/init",
                body: new() {
                    modVersion = GetVersion(),
                },
                onSuccess: data => {
                    AuthId = data.id;
                    Log("Got session token " + AuthId);
                    isUserTriggeredLoginFlow = true;
                    Application.OpenURL(Constants.LOGIN_URL + data.urlId);
                    State = LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER;
                },
                onError: request => {
                    Log(request.error);
                    Log(request.downloadHandler.text);
                    State = LoginState.LOADING_LOGIN_URL_FAILED;
                }
            ).StartGlobal();
        }

        private void CheckSessionState(bool fromSettings) {
            if (AuthId == null) {
                UserName = null;
                State = LoginState.NOT_LOGGED_IN;
                return;
            }

            ServerApi.Instance.ApiGet<SessionInfo>(
                path: "ingameauth/" + AuthId,
                onSuccess: data => {
                    AuthId = data.id;
                    Log("Got session info " + data.id + " " + data.user?.id + " " + data.user?.name);
                    //Application.OpenURL(Constants.LOGIN_URL + sessionId);
                    if (data.user != null) {
                        UserName = data.user.name;
                        State = LoginState.LOGGED_IN;
                        UploadManager.Instance.RetryFailedUploads();
                        loggedInAt = Time.unscaledTime;
                    } else if (fromSettings) {
                        State = LoginState.NOT_LOGGED_IN;
                    }
                },
                onError: request => {
                    Log(request.error);
                    Log(request.downloadHandler.text);
                    State = LoginState.LOADING_AUTH_STATE_FAILED;
                }
            ).StartGlobal();
        }

        public ButtonSimpleState GetLoginButtonState(bool justTitle) {
            ButtonSimpleState btnState = State switch {
                LoginState.NOT_LOGGED_IN => new(
                    $"Login to HKViz",
                    "So analytics files can be uploaded automatically",
                    btn => Login()
                ),
                LoginState.LOADING_LOGIN_URL => new(
                    "Cancel login",
                    $"Waiting for {Constants.WEBSITE_DISPLAY_LINK}",
                    btn => Logout()
                ),
                LoginState.LOADING_LOGIN_URL_FAILED => new(
                    "Login failed. Try again?",
                    "Could not initialize login flow",
                    btn => Login()
                ),
                LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER => new(
                    "Cancel login",
                    "Please login inside the opened browser tab",
                     btn => Logout()
                ),

                LoginState.LOADING_AUTH_STATE_FAILED => new(
                    "Login failed. Try again?",
                    $"Could not load login status from ${Constants.WEBSITE_DISPLAY_LINK}",
                    btn => Login()
                ),
                LoginState.LOGGED_IN => new(
                    justTitle ? "Logged in as " + UserName : "Logout",
                    "Logged in as " + UserName,
                    justTitle ? btn => { }
                : btn => Logout()
                ),
            };

            return btnState;
        }

        public record ButtonSimpleState(string name, string description, Action<UnityEngine.UI.MenuButton> action);
    }
}
