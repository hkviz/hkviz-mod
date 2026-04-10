using Modding;
using Satchel.BetterMenus;
using System;
using HKViz.Shared;
using HKViz.Shared.Auth;
using HKViz.Shared.Upload;

namespace HKViz {
    internal class HkVizModUI : Loggable {
        private static HkVizModUI? instance;

        public static HkVizModUI Instance {
            get {
                instance ??= new HkVizModUI();
                return instance;
            }
        }

        private AuthManager authManager = HkVizSharedInstances.Instance!.authManager;
        private UploadManager uploadManager = HkVizSharedInstances.Instance.uploadManager;

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
                                submitAction: (_) => authManager.Login()
                            );

                OpenHKVIzButton = new MenuButton(
                    name: $"Open {HkVizSharedConstants.WEBSITE_DISPLAY_LINK}",
                    description: "View your gameplay analytics in your browser",
                    submitAction: (_) => UnityEngine.Application.OpenURL(HkVizSharedConstants.WEBSITE_LINK)
                );

                DeleteUploadedFilesButton = new MenuButton(
                    name: $"Delete already uploaded analytics files",
                    description: $"No data is lost, since it is already stored on {HkVizSharedConstants.WEBSITE_DISPLAY_LINK}",
                    submitAction: (_) => {
                        uploadManager.DeleteAlreadyUploadedFiles();
                    }
                );

                RetryUploadsButton = new MenuButton(
                    name: $"Retry failed uploads",
                    description: $"",
                    submitAction: (_) => {
                        uploadManager.RetryFailedUploads();
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
                    values: ["On", "Off"],
                    applySetting: index => {
                        GlobalSettingsManager.SettingsOfCurrentUser.showLoginButtonInMainMenu = index == 0;
                    },
                    loadSetting: () => GlobalSettingsManager.SettingsOfCurrentUser.showLoginButtonInMainMenu ? 0 : 1
                );


                MenuRef = new Menu(
                    name: "HKViz",
                    elements: [
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
                    ]
                );

                authManager.StateChanged += state => LoginStateChanged(state);
                uploadManager.QueuesChanged += Instance_FinishedQueuesChanged;
                LoginStateChanged(authManager.State, update: false);
                UpdateQueueButtons(update: false);
            }

            return MenuRef.GetMenuScreen(modListMenu);
        }

        private void Instance_FinishedQueuesChanged() {
            UpdateQueueButtons();
        }

        private void UpdateQueueButtons(bool update = true) {
            if (RetryUploadsButton != null) {
                RetryUploadsButton.isVisible = uploadManager.FailedUploadsQueueCount() > 0;
                RetryUploadsButton.Description = $"Retry {uploadManager.FailedUploadsQueueCount()} failed upload files";
            }
            if (DeleteUploadedFilesButton != null) {
                DeleteUploadedFilesButton.isVisible = uploadManager.FinishedUploadFilesQueueCount() > 0;
            }

            if (!update) return;
            RetryUploadsButton?.Update();
            DeleteUploadedFilesButton?.Update();
            MenuRef?.Update();
        }

        private void LoginStateChanged(LoginState state, bool update = true) {
            var btnState = authManager.GetLoginButtonState(justTitle: false);

            if (LoginButton != null) {
                LoginButton.Name = btnState.name;
                LoginButton.Description = btnState.description;
                LoginButton.SubmitAction = _ => {
                    Log("Auth button clicked");
                    btnState.action();
                };
            }

            //if (AutoUploadOption != null) {
            //    AutoUploadOption.isVisible = state == HKVizAuthManager.LoginState.LOGGED_IN;
            //}

            if (MainMenuLoginButton != null) {
                MainMenuLoginButton.isVisible = state != LoginState.LOGGED_IN;
            }

            if (!update) return;
            LoginButton?.Update();
            //AutoUploadOption?.Update();
            MainMenuLoginButtonOption?.Update();
            MenuRef?.Update();
        }
    }
}
