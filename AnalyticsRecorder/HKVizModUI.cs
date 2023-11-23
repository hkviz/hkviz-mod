using Modding;
using Satchel.BetterMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsRecorder {
    internal class HKVizModUI: Loggable {
        private static HKVizModUI instance;

        public static HKVizModUI Instance {
            get {
                if (instance == null) {
                    instance = new HKVizModUI();
                }
                return instance;
            }
        }


        private Menu? MenuRef;
        private MenuButton? LoginButton;
        private MenuButton? MainMenuLoginButton;
        private HorizontalOption? AutoUploadOption;
        private HorizontalOption? MainMenuLoginButtonOption;

        public event Action? BeforeMenuShowed;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? modtoggledelegates) {
            if (MenuRef == null) {
                LoginButton = new MenuButton(
                                name: "Login to " + Constants.WEBSITE_URL,
                                description: "So analytics files can be uploaded automatically",
                                submitAction: (_) => HKVizAuthManager.Instance.Login()
                            );

                AutoUploadOption = new HorizontalOption(
                     name: "Auto upload",
                    description: "Upload run data once going to the menu and every few minutes",
                    values: new[] { "On", "Off" },
                    applySetting: index => {
                        GlobalSettingsManager.Settings.autoUpload = index == 0;
                    },
                    loadSetting: () => GlobalSettingsManager.Settings.autoUpload ? 0 : 1
                );

                MainMenuLoginButtonOption = new HorizontalOption(
                    name: "Main Menu login button",
                    description: "Display a login button in the main menu, when not signed in",
                    values: new[] { "On", "Off" },
                    applySetting: index => {
                        GlobalSettingsManager.Settings.showLoginButtonInMainMenu = index == 0;
                    },
                    loadSetting: () => GlobalSettingsManager.Settings.showLoginButtonInMainMenu ? 0 : 1
                );



                MenuRef = new Menu(
                    name: "HKViz",
                    elements: new Element[] {
                        AutoUploadOption,
                        MainMenuLoginButtonOption,
                        LoginButton,
                    }
                );
                HKVizAuthManager.Instance.StateChanged += state => LoginStateChanged(state);
                LoginStateChanged(HKVizAuthManager.Instance.State, update: false);
            }

            return MenuRef.GetMenuScreen(modListMenu);
        }

        private void LoginStateChanged(HKVizAuthManager.LoginState state, bool update = true) {
            var btnState = HKVizAuthManager.Instance.GetLoginButtonState(justTitle: false);

            LoginButton.Name = btnState.name;
            LoginButton.Description = btnState.description;
            LoginButton.SubmitAction = btn => {
                Log("Auth button clicked");
                btnState.action(btn);
            };

            AutoUploadOption.isVisible = state == HKVizAuthManager.LoginState.LOGGED_IN;

            if (update) {
                LoginButton.Update();
                AutoUploadOption.Update();
                MenuRef.Update();
            }
        }
    }
}
