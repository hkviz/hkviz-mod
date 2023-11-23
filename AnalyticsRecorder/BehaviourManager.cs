using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnalyticsRecorder {
    internal class BehaviourManager: MonoBehaviour {
        private static BehaviourManager _instance;

        private bool callFocusNextFrame = false;
        private bool lastFocusValue = false;

        public event Action<bool>? ApplicationFocusChanged;

        public static BehaviourManager Instance {
            get {
                if ( _instance == null ) {
                    var foreverEmpty = new GameObject("HkViz Behviours");
                    UnityEngine.Object.DontDestroyOnLoad(foreverEmpty);
                    _instance = foreverEmpty.AddComponent<BehaviourManager>();
                }
                return _instance;
            }
        }

        public void OnApplicationFocus(bool focused) {
            callFocusNextFrame = true;
            lastFocusValue = focused;
        }

        private void Update() {
            if (callFocusNextFrame) {
                ApplicationFocusChanged?.Invoke(lastFocusValue);
                callFocusNextFrame = false;
            }
        }

        public void Initialize() {
            // noop
        }
    }

    internal static class CoroutineExtensions {
        public static Coroutine StartGlobal(this System.Collections.IEnumerator coro) => BehaviourManager.Instance.StartCoroutine(coro); 
    }
}
