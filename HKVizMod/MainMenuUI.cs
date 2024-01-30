using HKMirror.Reflection;
using Modding;
using Satchel;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace HKViz {
    internal class MainMenuUI : Loggable {
        private static MainMenuUI? instance;
        public static MainMenuUI Instance {
            get {
                if (instance == null) {
                    instance = new MainMenuUI();
                }
                return instance;
            }
        }

        private UnityEngine.UI.Text? mainMenuLoginButtonText;
        private MenuButton? mainMenuLoginButton;
        private object? loginButtonMenuButtonListEntry;
        private MenuButtonList mainMenuList;

        public void Initialize() {
            On.MenuButtonList.Start += MenuButtonList_Start;
            On.MenuButtonList.OnEnable += MenuButtonList_OnEnable;

            HKVizAuthManager.Instance.StateChanged += AuthStateChanged;
            Log("main menu init");
        }

        public void GlobalSettingsLoaded() {
            mainMenuList?.Reflect()?.Start();
        }

        private void MenuButtonList_OnEnable(On.MenuButtonList.orig_OnEnable orig, MenuButtonList self) {
            mainMenuList?.Reflect()?.Start();
        }

        private void AuthStateChanged(HKVizAuthManager.LoginState obj) {
            Log("Auth change " + obj + " b" + mainMenuLoginButton?.name);
            mainMenuList?.Reflect()?.Start();
        }

        private void MenuButtonList_Start(On.MenuButtonList.orig_Start orig, MenuButtonList self) {
            if (self.gameObject.name == "MainMenuButtons") {
                mainMenuList = self;
                CreateLoginButtonGOIfNeeded();

                // changing entries of MenuButtons:
                var entries = (Array)self.GetFieldByReflection("entries");

                var entryType = typeof(MenuButtonList).GetNestedType("Entry", BindingFlags.NonPublic);
                if (loginButtonMenuButtonListEntry == null) {
                    var entry = Activator.CreateInstance(entryType, true);
                    loginButtonMenuButtonListEntry = entry;
                }
                loginButtonMenuButtonListEntry.SetFieldByReflection("selectable", mainMenuLoginButton.GetComponent<Selectable>());

                var alreadyInList = entries.OfType<object>().Any(e => e == loginButtonMenuButtonListEntry);
                var showButton = showLoginButtonInMainMenu();

                var newEntries = entries;

                if (!alreadyInList && showButton) { 
                    // add button
                    newEntries = Array.CreateInstance(entryType, entries.Length + 1);
                    entries.CopyTo(newEntries, 0);
                    newEntries.SetValue((entries as IList)[entries.Length - 1], newEntries.Length - 1);
                    newEntries.SetValue(loginButtonMenuButtonListEntry, newEntries.Length - 2);
                } else if (alreadyInList && !showButton) {
                    // remove button
                    newEntries = Array.CreateInstance(entryType, entries.Length - 1);
                    entries.OfType<object>()
                        .Where(it => it != loginButtonMenuButtonListEntry)
                        .ToArray()
                        .CopyTo(newEntries, 0);
                }

                self.SetFieldByReflection("entries", newEntries);

                mainMenuLoginButton.gameObject.SetActive(showButton);

                var btnState = HKVizAuthManager.Instance.GetLoginButtonState(justTitle: true);
                if (mainMenuLoginButtonText != null && !mainMenuLoginButtonText.IsDestroyed()) {
                    mainMenuLoginButtonText.text = btnState.name;
                }
                if (mainMenuLoginButton != null && !mainMenuLoginButton.IsDestroyed()) {
                    mainMenuLoginButton.submitAction = btnState.action;
                }

                // hkvizMainMenuList.StartCoroutine(CheckButtonVisibilityNextFrame());
            }

            orig.Invoke(self);
        }

        private void CreateLoginButtonGOIfNeeded() {
            var hkvizMainMenuList = mainMenuList.GetComponent<HKVizMainMenuList>();
            if (hkvizMainMenuList == null) {
                hkvizMainMenuList = mainMenuList.gameObject.AddComponent<HKVizMainMenuList>();
                hkvizMainMenuList.loginButton = GameObject.Instantiate(mainMenuList.transform.GetChild(0).gameObject, mainMenuList.transform);
                hkvizMainMenuList.loginButton.RemoveComponent<AutoLocalizeTextUI>();
                hkvizMainMenuList.loginButton.transform.SetSiblingIndex(4);
                hkvizMainMenuList.loginButton.name = "Login Button";

                mainMenuLoginButton = hkvizMainMenuList.loginButton.GetComponent<MenuButton>();
                mainMenuLoginButton.buttonType = MenuButton.MenuButtonType.CustomSubmit;
                mainMenuLoginButton.proceed = false;
                mainMenuLoginButton.flashEffect = null;

                var eventTrigger = hkvizMainMenuList.loginButton.GetComponent<EventTrigger>();
                eventTrigger.triggers.Clear();

                mainMenuLoginButtonText = hkvizMainMenuList.loginButton.GetComponentInChildren<UnityEngine.UI.Text>();
                mainMenuLoginButtonText.text = "Login";
            }
        }

        private bool showLoginButtonInMainMenu() => (
                GlobalSettingsManager.SettingsOfCurrentUser.showLoginButtonInMainMenu &&
                HKVizAuthManager.Instance.State != HKVizAuthManager.LoginState.LOGGED_IN
            );

        //private void CheckButtonVisibility() {
        //    Log("Check visibility " + HKVizAuthManager.Instance.State);
        //    mainMenuLoginButton?.gameObject?.SetActive(
        //        GlobalSettingsManager.Settings.showLoginButtonInMainMenu &&
        //        HKVizAuthManager.Instance.State != HKVizAuthManager.LoginState.LOGGED_IN
        //    );
        //}

        //private IEnumerator CheckButtonVisibilityNextFrame() {
        //    yield return null;
        //    CheckButtonVisibility();

        //}

        private class HKVizMainMenuList : MonoBehaviour {
            public GameObject loginButton;
        }
    }
}
