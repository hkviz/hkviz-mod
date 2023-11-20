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

        public event Action? BeforeMenuShowed;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? modtoggledelegates) {
            if (MenuRef == null) {
                LoginButton = new MenuButton(
                                name: "Login to " + Constants.WEBSITE_URL,
                                description: "So analytics files can be uploaded and visualized",
                                submitAction: (_) => HKVizAuthManager.Instance.Login()
                            );

                MenuRef = new Menu(
                    name: "HKViz",
                    elements: new Element[] {
                        LoginButton
                    }
                );

                HKVizAuthManager.Instance.StateChanged += LoginStateChanged;
                //LoginStateChanged(HKVizAuthManager.Instance.State);
            }

            return MenuRef.GetMenuScreen(modListMenu);
        }

        private void LoginStateChanged(HKVizAuthManager.LoginState state) {
            LoginButton.Name = state switch {
                HKVizAuthManager.LoginState.NOT_LOGGED_IN => $"Login to {Constants.WEBSITE_URL}",
                HKVizAuthManager.LoginState.LOADING_LOGIN_URL => $"Cancel login",
                HKVizAuthManager.LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER => "Cancel login",
                HKVizAuthManager.LoginState.LOADING_LOGIN_URL_FAILED => "Login failed. Try again?",
                HKVizAuthManager.LoginState.LOGGED_IN => "Logout",
            };
            LoginButton.Description = state switch {
                HKVizAuthManager.LoginState.NOT_LOGGED_IN => "So analytics files can be uploaded and visualized",
                HKVizAuthManager.LoginState.LOADING_LOGIN_URL => $"Waiting for {Constants.WEBSITE_URL}",
                HKVizAuthManager.LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER => "Please login inside the opened browser tab",
                HKVizAuthManager.LoginState.LOADING_LOGIN_URL_FAILED => "Could not initialize login flow",
                HKVizAuthManager.LoginState.LOGGED_IN => "Logged in as "+ HKVizAuthManager.Instance.UserName,
            };

            LoginButton.SubmitAction = state switch {
                HKVizAuthManager.LoginState.NOT_LOGGED_IN => btn => HKVizAuthManager.Instance.Login(),
                HKVizAuthManager.LoginState.LOADING_LOGIN_URL => btn => HKVizAuthManager.Instance.CancelLogin(),
                HKVizAuthManager.LoginState.LOADING_LOGIN_URL_FAILED => btn => HKVizAuthManager.Instance.CancelLogin(),
                HKVizAuthManager.LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER => btn => HKVizAuthManager.Instance.Login(),
                HKVizAuthManager.LoginState.LOGGED_IN => throw new NotImplementedException(),
            };
            LoginButton.Update();
        }
    }
}
