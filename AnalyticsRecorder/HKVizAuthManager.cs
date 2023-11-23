using MapChanger;
using Modding;
using Newtonsoft.Json;
using Satchel.BetterMenus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

namespace AnalyticsRecorder {
    [Serializable]
    internal class InitSessionBodyPayload {
        public string name;
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

    internal class HKVizAuthManager: Loggable {
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
                GameManager.instance.StartCoroutine(ApiDelete<Empty>(
                    path: "ingameauth/" + AuthId,
                    onSuccess: data => {
                        Log("Deleted session in backend");
                    },
                    onError: request => {
                        Log("Session in backend not deleted");
                        Log(request.error);
                        Log(request.downloadHandler.text);
                    }
                ));
            }
        }

        private void InitSessionToken() {
            State = LoginState.LOADING_LOGIN_URL;
            GameManager.instance.StartCoroutine(ApiPost<InitSessionBodyPayload, InitSessionResult>(
                path: "ingameauth/init",
                body: new() {
                    name = SystemInfo.deviceName.Substring(0, Math.Min(SystemInfo.deviceName.Length, 255)),
                },
                onSuccess: data => {
                    AuthId = data.id;
                    Log("Got session token " + AuthId);
                    Application.OpenURL(Constants.LOGIN_URL + data.urlId);
                    State = LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER;
                },
                onError: request => {
                    Log(request.error);
                    Log(request.downloadHandler.text);
                    State = LoginState.LOADING_LOGIN_URL_FAILED;
                }
            ));
        }

        private void CheckSessionState(bool fromSettings) {
            GameManager.instance.StartCoroutine(ApiGet<SessionInfo>(
                path: "ingameauth/" + AuthId,
                onSuccess: data => {
                    AuthId = data.id;
                    Log("Got session info " + data.id + " " + data.user?.id + " " + data.user?.name);
                    //Application.OpenURL(Constants.LOGIN_URL + sessionId);
                    if (data.user != null) {
                        UserName = data.user.name;
                        State = LoginState.LOGGED_IN;
                    } else if (fromSettings) {
                        State = LoginState.NOT_LOGGED_IN;
                    }
                },
                onError: request => {
                    Log(request.error);
                    Log(request.downloadHandler.text);
                    State = LoginState.LOADING_AUTH_STATE_FAILED;
                }
            ));
        }

        private IEnumerator ApiPost<TBody, TResponse>(string path, TBody body, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            var json = Json.Stringify(body);
            var url = Constants.API_URL + path;

            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            return WrapWWW(request, onSuccess, onError);
        }

        private IEnumerator ApiGet<TResponse>(string path, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            var url = Constants.API_URL + path;
            Log(url);
            var request = UnityWebRequest.Get(url);
            return WrapWWW(request, onSuccess, onError);
        }

        private IEnumerator ApiDelete<TResponse>(string path, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            var url = Constants.API_URL + path;
            Log(url);
            var request = UnityWebRequest.Delete(url);
            return WrapWWW(request, onSuccess, onError);
        }


        private IEnumerator WrapWWW<TResponse>(UnityWebRequest request, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            yield return request.SendWebRequest();
            Debug.Log("Status Code: " + request.responseCode);

            if (request.result != UnityWebRequest.Result.Success) {
                Log(request.error);
                Log(request.downloadHandler.text);
                onError(request);
            } else {
                var result = Json.Parse<TResponse>(request.downloadHandler.text) ?? throw new Exception("No response data");
                onSuccess(result);
            }

            request.uploadHandler.Dispose();
            request.downloadHandler.Dispose();
            request.Dispose();
        }

        public ButtonSimpleState GetLoginButtonState(bool justTitle) {
            ButtonSimpleState btnState = State switch {
                LoginState.NOT_LOGGED_IN => new(
                    $"Login to {Constants.WEBSITE_URL}",
                    "So analytics files can be uploaded automatically",
                    btn => Login()
                ),
                LoginState.LOADING_LOGIN_URL => new(
                    "Cancel login",
                    $"Waiting for {Constants.WEBSITE_URL}",
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
                    $"Could not load login status from ${Constants.WEBSITE_URL}",
                    btn => Login()
                ),
                LoginState.LOGGED_IN => new(
                    justTitle ? "Logged in as " + UserName : "Logout",
                    "Logged in as " + UserName,
                    justTitle ? btn => { } : btn => Logout()
                ),
            };

            return btnState;
        }

        public record ButtonSimpleState(string name, string description, Action<UnityEngine.UI.MenuButton> action);
    }
}
