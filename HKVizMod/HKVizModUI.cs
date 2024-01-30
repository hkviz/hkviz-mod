using Modding;
using Satchel.BetterMenus;
using System;

namespace HKViz {
    internal class HKVizModUI : Loggable {
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
        private MenuButton? OpenHKVIzButton;
        private MenuButton? DeleteUploadedFilesButton;
        private MenuButton? RetryUploadsButton;
        private MenuButton? MainMenuLoginButton;
        //private HorizontalOption? AutoUploadOption;
        private HorizontalOption? MainMenuLoginButtonOption;

        public event Action? BeforeMenuShowed;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? modtoggledelegates) {
            if (MenuRef == null) {
                LoginButton = new MenuButton(
                                name: "Login to HKViz",
                                description: "So analytics files can be uploaded automatically",
                                submitAction: (_) => HKVizAuthManager.Instance.Login()
                            );

                OpenHKVIzButton = new MenuButton(
                    name: $"Open {Constants.WEBSITE_DISPLAY_LINK}",
                    description: "View your gameplay analytics in your browser",
                    submitAction: (_) => UnityEngine.Application.OpenURL(Constants.WEBSITE_LINK)
                );

                DeleteUploadedFilesButton = new MenuButton(
                    name: $"Delete already uploaded analytics files",
                    description: $"No data is lost, since it is already stored on {Constants.WEBSITE_DISPLAY_LINK}",
                    submitAction: (_) => {
                        UploadManager.Instance.DeleteAlreadyUploadedFiles();
                    }
                );

                RetryUploadsButton = new MenuButton(
                    name: $"Retry failed uploads",
                    description: $"",
                    submitAction: (_) => {
                        UploadManager.Instance.RetryFailedUploads();
                    }
                );


                //AutoUploadOption = new HorizontalOption(
                //    name: "Auto upload",
                //    description: "Upload run data once going to the menu and every few minutes",
                //    values: new[] { "On", "Off" },
                //    applySetting: index => {
                //        GlobalSettingsManager.Settings.autoUpload = index == 0;
                //    },
                //    loadSetting: () => GlobalSettingsManager.Settings.autoUpload ? 0 : 1
                //);

                MainMenuLoginButtonOption = new HorizontalOption(
                    name: "Main Menu login button",
                    description: "Display a login button in the main menu, when not signed in",
                    values: new[] { "On", "Off" },
                    applySetting: index => {
                        GlobalSettingsManager.SettingsOfCurrentUser.showLoginButtonInMainMenu = index == 0;
                    },
                    loadSetting: () => GlobalSettingsManager.SettingsOfCurrentUser.showLoginButtonInMainMenu ? 0 : 1
                );


                MenuRef = new Menu(
                    name: "HKViz",
                    elements: new Element[] {
                        
                        // Disabled for now, since I did not fix all implications arising from this.
                        // The main one being that a runfile might be created in another session
                        // therefore variables stored from the beginning of the session would need to be remembered 
                        // over multiple sessions (currently this is the start time of a runfile)
                        //AutoUploadOption,
                        OpenHKVIzButton,
                        RetryUploadsButton,
                        DeleteUploadedFilesButton,
                        LoginButton,
                        MainMenuLoginButtonOption,
                    }
                );

                HKVizAuthManager.Instance.StateChanged += state => LoginStateChanged(state);
                UploadManager.Instance.QueuesChanged += Instance_FinishedQueuesChanged;
                LoginStateChanged(HKVizAuthManager.Instance.State, update: false);
                UpdateQueueButtons(update: false);
            }

            return MenuRef.GetMenuScreen(modListMenu);
        }

        private void Instance_FinishedQueuesChanged() {
            UpdateQueueButtons();
        }

        private void UpdateQueueButtons(bool update = true) {
            if (RetryUploadsButton != null) {
                RetryUploadsButton.isVisible = UploadManager.Instance.FailedUploadsQueueCount() > 0;
                RetryUploadsButton.Description = $"Retry {UploadManager.Instance.FailedUploadsQueueCount()} failed upload files";
            }
            if (DeleteUploadedFilesButton != null) {
                DeleteUploadedFilesButton.isVisible = UploadManager.Instance.FinishedUploadFilesQueueCount() > 0;
            }

            if (update) {
                RetryUploadsButton?.Update();
                DeleteUploadedFilesButton?.Update();
                MenuRef?.Update();
            }
        }

        private void LoginStateChanged(HKVizAuthManager.LoginState state, bool update = true) {
            var btnState = HKVizAuthManager.Instance.GetLoginButtonState(justTitle: false);

            if (LoginButton != null) {
                LoginButton.Name = btnState.name;
                LoginButton.Description = btnState.description;
                LoginButton.SubmitAction = btn => {
                    Log("Auth button clicked");
                    btnState.action(btn);
                };
            }

            //if (AutoUploadOption != null) {
            //    AutoUploadOption.isVisible = state == HKVizAuthManager.LoginState.LOGGED_IN;
            //}

            if (MainMenuLoginButton != null) {
                MainMenuLoginButton.isVisible = state != HKVizAuthManager.LoginState.LOGGED_IN;
            }

            if (update) {
                LoginButton?.Update();
                //AutoUploadOption?.Update();
                MainMenuLoginButtonOption?.Update();
                MenuRef?.Update();
            }
        }
    }
}
