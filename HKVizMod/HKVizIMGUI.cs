using UnityEngine;

namespace HKViz {
    internal class HKVizIMGUI : MonoBehaviour {

        private RecordingFileManager recording = RecordingFileManager.Instance;
        private UploadManager uploadManager = UploadManager.Instance;
        private GameManager gm;

        private GUIStyle style = new GUIStyle() {
            wordWrap = true,
        };

        public void OnGUI() {
            style.fontSize = (Screen.height / 1080) * (Screen.width / 1920) * 30;

            var areaWidth = Screen.width / 4;
            var areaHeight = Screen.height / 4;
            GUILayout.BeginArea(new Rect(
                x: (Screen.width - areaWidth) / 2,
                y: (Screen.height - areaHeight) - style.fontSize,
                width: areaWidth,
                height: areaHeight
            ), style);

            gm ??= GameManager.instance;
            if (!gm.isPaused && recording.isRecording) {
                return;
            }

            GUILayout.FlexibleSpace();
            var failedCount = uploadManager.FailedUploadsQueueCount();
            var queuedFiles = uploadManager.QueuedFilesQueueCount();

            //var prevSize = GUI.skin.label.fontSize;
            //GUI.skin.label.fontSize = (Screen.height / 1080) * 16;

            if (failedCount > 0) {
                style.normal.textColor = Color.red;
                var fileNr = failedCount == 1 ? "One HKViz file" : $"{failedCount} HKViz files";
                GUILayout.Label($"HKViz: {fileNr} could not be uploaded. You can retry them from the settings", style);
            }
            if (queuedFiles > 0) {
                style.normal.textColor = Color.white;
                GUILayout.Label(queuedFiles == 1 ? "HKViz: Upload in progress. One file left." : $"HKViz: Upload in progress. {queuedFiles} files left", style);
            }

            var loginState = HKVizAuthManager.Instance.State;
            if (loginState == HKVizAuthManager.LoginState.LOADING_LOGIN_URL) {
                style.normal.textColor = Color.white;
                GUILayout.Label($"HKViz: ...Loading login url...");
            } else if (loginState == HKVizAuthManager.LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER) {
                style.normal.textColor = Color.white;
                GUILayout.Label($"HKViz: Please login inside the opened browser window");
            }

            if (HKVizAuthManager.Instance.ShowLoginSuccess) {
                style.normal.textColor = Color.green;
                GUILayout.Label($"HKViz: Login successful");
            }

            //GUI.skin.label.fontSize = prevSize;

            GUILayout.EndArea();
        }
    }
}
