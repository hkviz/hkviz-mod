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
            LOGGED_IN,
        }

        public string? SessionId;
        public string? UserName;

        private LoginState _state = LoginState.NOT_LOGGED_IN;
        public LoginState State {
            get {
                return State;
            }
            private set {
                _state = value;
                StateChanged?.Invoke(value);
            }
        }

        public event Action<LoginState>? StateChanged;

        private HKVizAuthManager() {
            //Application.focusChanged += Application_focusChanged;
        }

        private void Application_focusChanged(bool focused) {
            if (focused && State == LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER) {
                CheckSessionState();
            }
        }

        public void Login() {
            InitSessionToken();
        }

        public void CancelLogin() {
            State = LoginState.NOT_LOGGED_IN;
            SessionId = null;
        }

        private void InitSessionToken() {
            State = LoginState.LOADING_LOGIN_URL;
            GameManager.instance.StartCoroutine(ApiPost<InitSessionBodyPayload, InitSessionResult>(
                path: "ingamesession/init",
                body: new() {
                    name = SystemInfo.deviceName.Substring(0, Math.Min(SystemInfo.deviceName.Length, 255)),
                },
                onSuccess: data => {
                    SessionId = data.id;
                    Log("Got session token " + SessionId);
                    Application.OpenURL(Constants.LOGIN_URL + SessionId);
                    State = LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER;
                },
                onError: request => {
                    Debug.Log(request.error);
                    Debug.Log(request.downloadHandler.text);
                    State = LoginState.LOADING_LOGIN_URL_FAILED;
                }
            ));
        }

        private void CheckSessionState() {
            GameManager.instance.StartCoroutine(ApiGet<SessionInfo>(
                path: "ingamesession/" + SessionId,
                onSuccess: data => {
                    SessionId = data.id;
                    Log("Got session info " + data.id + " " + data.user?.id + " " + data.user?.name);
                    //Application.OpenURL(Constants.LOGIN_URL + sessionId);
                    if (data.user != null) {
                        State = LoginState.LOGGED_IN;
                        UserName = data.user?.name;
                    }
                },
                onError: request => {
                    Debug.Log(request.error);
                    Debug.Log(request.downloadHandler.text);
                }
            ));
        }

        private IEnumerator ApiPost<TBody, TResponse>(string path, TBody body, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            var json = Json.Stringify(body);
            var url = Constants.API_URL + path;

            var request = new UnityWebRequest(url, "POST");
            //var json = Json.Stringify(body);
            Log(json);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            return WrapWWW(request, onSuccess, onError);
        }

        private IEnumerator ApiGet<TResponse>(string path, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            var url = Constants.API_URL + path;
            var request = UnityWebRequest.Get(url);
            return WrapWWW(request, onSuccess, onError);
        }


        private IEnumerator WrapWWW<TResponse>(UnityWebRequest request, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            yield return request.SendWebRequest();
            Debug.Log("Status Code: " + request.responseCode);

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);
                onError(request);
            } else {
                Log("Got session token " + request.downloadHandler.text);
                var result = Json.Parse<TResponse>(request.downloadHandler.text) ?? throw new Exception("No response data");
                onSuccess(result);
            }

            request.uploadHandler.Dispose();
            request.downloadHandler.Dispose();
            request.Dispose();
        }
    }
}
