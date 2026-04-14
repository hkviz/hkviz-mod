using System;
using UnityEngine;
using UnityEngine.UI;

namespace HKViz.Shared.Auth;

public class AuthManager {

    public string? AuthId { get; private set; }
    public string? UserName { get; private set; }

    private float loggedInAt = -100000;
    private bool isUserTriggeredLoginFlow = false;
    private float displayLoginSuccessForSeconds = 4;
    public bool ShowLoginSuccess => (Time.unscaledTime - loggedInAt) < displayLoginSuccessForSeconds;

    private LoginState _state = LoginState.NOT_LOGGED_IN;


    public LoginState State {
        get => _state;
        private set {
            _state = value;

            StateChanged?.Invoke(value);
        }
    }

    private readonly ServerApi serverApi;
    private readonly IHkVizLogger _logger;
    
    public event Action<LoginState>? StateChanged;

    public AuthManager(ServerApi serverApi, IHkVizLogger logger) {
        this.serverApi = serverApi;
        this._logger = logger;
        Application.focusChanged += Application_focusChanged;
    }

    public void InitializeFromGlobalSettings(string? authId, string? userName) {
        AuthId = authId;
        UserName = userName;
        CheckSessionState(fromSettings: true);
    }

    private void Application_focusChanged(bool focused) {
        // Log("Focus changed");
        if (focused && State == LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER) {
            CheckSessionState(fromSettings: false);
        }
    }

    public void Login() {
        _logger.LogInfo("Login");
        InitSessionToken();
    }

    public void Logout() {
        State = LoginState.NOT_LOGGED_IN;
        AuthId = null;

        if (AuthId != null) {
            serverApi.ApiDelete<Empty>(
                path: "ingameauth/" + AuthId,
                onSuccess: data => {
                    _logger.LogInfo("Deleted session in backend");
                },
                onError: request => {
                    _logger.LogInfo("Session in backend not deleted");
                    _logger.LogInfo(request.error);
                    _logger.LogInfo(request.downloadHandler.text);
                }
            ).StartGlobal();
        }
    }

    private void InitSessionToken() {
        State = LoginState.LOADING_LOGIN_URL;
        serverApi.ApiPost<InitSessionBodyPayload, InitSessionResult>(
            path: "ingameauth/init",
            body: new InitSessionBodyPayload {
                modVersion = HkVizSharedConstants.GetModVersion(),
            },
            onSuccess: data => {
                AuthId = data.id;
                _logger.LogInfo("Got session token " + AuthId);
                isUserTriggeredLoginFlow = true;
                Application.OpenURL(HkVizSharedConstants.LOGIN_URL + data.urlId);
                State = LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER;
            },
            onError: request => {
                _logger.LogInfo(request.error);
                _logger.LogInfo(request.downloadHandler.text);
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

        serverApi.ApiGet<SessionInfo>(
            path: "ingameauth/" + AuthId,
            onSuccess: data => {
                AuthId = data.id;
                _logger.LogInfo("Got session info " + data.user?.id + " " + data.user?.name);
                //Application.OpenURL(Constants.LOGIN_URL + sessionId);
                if (data.user != null) {
                    UserName = data.user.name;
                    State = LoginState.LOGGED_IN;
                    loggedInAt = Time.unscaledTime;
                    if (fromSettings) {
                        // not showing success message on game load
                        displayLoginSuccessForSeconds = -1000;
                    }
                } else if (fromSettings) {
                    State = LoginState.NOT_LOGGED_IN;
                }
            },
            onError: request => {
                _logger.LogInfo(request.error);
                _logger.LogInfo(request.downloadHandler.text);
                State = LoginState.LOADING_AUTH_STATE_FAILED;
            }
        ).StartGlobal();
    }

    public ButtonSimpleState GetLoginButtonState(bool justTitle) {
        ButtonSimpleState btnState = State switch {
            LoginState.NOT_LOGGED_IN => new ButtonSimpleState(
                $"Login to HKViz",
                "So analytics files can be uploaded automatically",
                Login
            ),
            LoginState.LOADING_LOGIN_URL => new ButtonSimpleState(
                "Cancel login",
                $"Waiting for {HkVizSharedConstants.WEBSITE_DISPLAY_LINK}",
                Logout
            ),
            LoginState.LOADING_LOGIN_URL_FAILED => new ButtonSimpleState(
                "Login failed. Try again?",
                "Could not initialize login flow",
                Login
            ),
            LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER => new ButtonSimpleState(
                "Cancel login",
                "Login using the browser tab that opened.",
                Logout
            ),
            LoginState.LOADING_AUTH_STATE_FAILED => new ButtonSimpleState(
                "Login failed. Try again?",
                $"Could not load login status from {HkVizSharedConstants.WEBSITE_DISPLAY_LINK}",
                Login
            ),
            LoginState.LOGGED_IN => new ButtonSimpleState(
                justTitle ? "Logged in as " + UserName : "Logout",
                "Logged in as " + UserName,
                Logout
            ),
        };

        return btnState;
    }

    public class ButtonSimpleState(string name, string description, Action action) {
        public string name { get; private set; } = name;
        public string description { get; private set; } = description;
        public Action action { get; private set; } = action;
    }
}
