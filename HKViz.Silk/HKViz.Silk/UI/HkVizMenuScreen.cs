using HKViz.Shared.Auth;
using Silksong.ModMenu.Elements;
using Silksong.ModMenu.Screens;

namespace HKViz.Silk.UI;

public class HkVizMenuScreen {
    private readonly AuthManager authManager;
    
    private TextButton? loginButton;
    

    public HkVizMenuScreen(AuthManager authManager) {
        this.authManager = authManager;
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
        
        screen.OnShow += _ => {
            UpdateLoginButton();
        };

        return screen;
    }
}