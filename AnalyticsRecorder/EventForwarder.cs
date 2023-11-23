using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnalyticsRecorder {
    internal class EventForwarder: MonoBehaviour {
        private static EventForwarder _instance;

        private bool callFocusNextFrame = false;
        private bool lastFocusValue = false;

        public event Action<bool>? ApplicationFocusChanged;

        public static EventForwarder Instance {
            get {
                if ( _instance == null ) {
                    _instance = GameManager.instance.gameObject.AddComponent<EventForwarder>();
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
    }
}
