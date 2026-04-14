using HKViz.Shared.Auth;
using HKViz.Shared.Upload;
using Silksong.ModMenu.Elements;
using Silksong.ModMenu.Screens;

namespace HKViz.Silk.UI;

public class HkVizMenuScreen {
    private readonly AuthManager authManager;
    private readonly UploadManager uploadManager;
    
    private TextButton? loginButton;
    

    public HkVizMenuScreen(AuthManager authManager, UploadManager uploadManager) {
        this.authManager = authManager;
        this.uploadManager = uploadManager;
        authManager.StateChanged += AuthStateChanged;
    }

    private void AuthStateChanged(LoginState state) {
        UpdateLoginButton();
    }

    private void UpdateLoginButton() {
        var buttonState = authManager.GetLoginButtonState(true);
        if (loginButton == null || !loginButton.ButtonText) return;
        loginButton.ButtonText.SetLocalizedText(buttonState.name);
        loginButton.OnSubmit = buttonState.action;
    }

    public AbstractMenuScreen BuildCustomMenu() {
        SimpleMenuScreen screen = new("HKViz");
        loginButton = new TextButton(I18N.Get(I18NKey.LOGIN_BUTTON_TEXT));
        screen.Add(loginButton);
        
        var retryUploadsButton = new TextButton(I18N.Get(I18NKey.RETRY_UPLOADS_BUTTON_TEXT));
        retryUploadsButton.OnSubmit = () => uploadManager.RetryFailedUploads();
        screen.Add(retryUploadsButton);
        
        screen.OnShow += _ => {
            UpdateLoginButton();
        };

        return screen;
    }
}