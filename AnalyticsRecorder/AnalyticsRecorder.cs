using Modding;
using System;

namespace AnalyticsRecorder {
    public class AnalyticsRecorderMod : Mod {
        private static AnalyticsRecorderMod? _instance;

        internal static AnalyticsRecorderMod Instance {
            get {
                if (_instance == null) {
                    throw new InvalidOperationException($"An instance of {nameof(AnalyticsRecorderMod)} was never constructed");
                }
                return _instance;
            }
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public AnalyticsRecorderMod() : base("AnalyticsRecorder") {
            _instance = this;
        }

        public override void Initialize() {
            Log("Initializing");

            // put additional initialization logic here

            Log("Initialized");
        }
    }
}
