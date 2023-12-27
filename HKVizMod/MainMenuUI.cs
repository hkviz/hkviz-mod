using Modding;
using Satchel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HKViz {
    internal class MainMenuUI : Loggable {
        private static MainMenuUI? instance;
        public static MainMenuUI Instance { get { 
                if (instance == null) {
                    instance = new MainMenuUI();
                }
                return instance; 
            } 
        }

        private UnityEngine.UI.Text? mainMenuLoginButtonText;
        private MenuButton? mainMenuLoginButton;


        public void Initialize() {
            On.MenuButtonList.Start += MenuButtonList_Start;
            On.MenuButtonList.OnEnable += MenuButtonList_OnEnable;

            HKVizAuthManager.Instance.StateChanged += AuthStateChanged;
            Log("main menu init");
        }

        public void GlobalSettingsLoaded() {
            CheckButtonVisibility();
        }

        private void MenuButtonList_OnEnable(On.MenuButtonList.orig_OnEnable orig, MenuButtonList self) {
            CheckButtonVisibility();
        }

        private void AuthStateChanged(HKVizAuthManager.LoginState obj) {
            Log("Auth change " + obj + " b" + mainMenuLoginButton?.name);
            var btnState = HKVizAuthManager.Instance.GetLoginButtonState(justTitle: true);
            if (mainMenuLoginButtonText != null && !mainMenuLoginButtonText.IsDestroyed()) {
                mainMenuLoginButtonText.text = btnState.name;
            }
            if (mainMenuLoginButton != null && !mainMenuLoginButton.IsDestroyed()) {
                mainMenuLoginButton.submitAction = btnState.action;
            }
            CheckButtonVisibility();
        }

        private void MenuButtonList_Start(On.MenuButtonList.orig_Start orig, MenuButtonList self) {
            if (self.gameObject.name == "MainMenuButtons") {
                var loginButton = GameObject.Instantiate(self.transform.GetChild(0).gameObject, self.transform);
                loginButton.RemoveComponent<AutoLocalizeTextUI>();
                loginButton.transform.SetSiblingIndex(4);
                loginButton.name = "Login Button";

                mainMenuLoginButton = loginButton.GetComponent<MenuButton>();
                mainMenuLoginButton.buttonType = MenuButton.MenuButtonType.CustomSubmit;
                mainMenuLoginButton.proceed = false;
                mainMenuLoginButton.flashEffect = null;
                var eventTrigger = loginButton.GetComponent<EventTrigger>();
                eventTrigger.triggers.Clear();

                mainMenuLoginButtonText = loginButton.GetComponentInChildren<UnityEngine.UI.Text>();
                mainMenuLoginButtonText.text = "Login";

                // changing entries of MenuButtons:
                var entries = self.GetFieldByReflection("entries") as System.Collections.IList;

                var entryType = typeof(MenuButtonList).GetNestedType("Entry", BindingFlags.NonPublic);
                var entry = Activator.CreateInstance(entryType, true);
                entry.SetFieldByReflection("selectable", mainMenuLoginButton.GetComponent<Selectable>());


                var newEntries = Array.CreateInstance(entryType, entries.Count + 1);
                entries.CopyTo(newEntries, 0);
                newEntries.SetValue(entries[entries.Count - 1], newEntries.Length - 1); 
                newEntries.SetValue(entry, newEntries.Length - 2);

                self.SetFieldByReflection("entries", newEntries);

                AuthStateChanged(HKVizAuthManager.Instance.State);
                CheckButtonVisibilityNextFrame().StartGlobal();
            }

            orig.Invoke(self);
        }

        private void CheckButtonVisibility() {
            Log("Check visibility " +  HKVizAuthManager.Instance.State);
            mainMenuLoginButton?.gameObject?.SetActive(
                GlobalSettingsManager.Settings.showLoginButtonInMainMenu &&
                HKVizAuthManager.Instance.State != HKVizAuthManager.LoginState.LOGGED_IN
            );
        }

        private IEnumerator CheckButtonVisibilityNextFrame() {
            yield return null;
            CheckButtonVisibility();

        }
    }
}
