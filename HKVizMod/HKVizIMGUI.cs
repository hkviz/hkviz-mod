using System;
using UnityEngine;

namespace HKViz {
    internal class HKVizIMGUI : MonoBehaviour {

        private RecordingFileManager recording = RecordingFileManager.Instance;
        private UploadManager uploadManager = UploadManager.Instance;
        private GameManager gm;
        private HeroController hero;

        private GUIStyle style = new GUIStyle() {
            wordWrap = true,
        };

        private string SecondsToDurationFormatted(float seconds) {
            var secondsInt = (int)seconds;
            var m = secondsInt / 60;
            var s = secondsInt % 60;

            var sStr = s == 0 ? null : s == 1 ? "1 second" : $"{s} seconds";
            var mStr = m == 0 ? null : m == 1 ? "1 minute" : $"{m} minutes";

            if (mStr != null && sStr != null) {
                return $"{mStr} {sStr}";
            } else {
                return sStr ?? mStr ?? "Now";
            }
        }

        public void OnGUI() {
            if (gm == null) {
                gm = GameManager.instance;
            }
            if (hero == null) {
                hero = HeroController.instance;
            }

            if ((!gm.isPaused || hero.cState.transitioning) && recording.isRecording) {
                return;
            }


            style.fontSize = (Screen.height / 1080) * (Screen.width / 1920) * 30;

            var areaWidth = Screen.width / 4;
            var areaHeight = Screen.height / 4;
            GUILayout.BeginArea(new Rect(
                x: (Screen.width - areaWidth) / 2,
                y: (Screen.height - areaHeight) - style.fontSize,
                width: areaWidth,
                height: areaHeight
            ), style);


            GUILayout.FlexibleSpace();
            var failedCount = uploadManager.FailedUploadsQueueCount();
            var queuedFiles = uploadManager.QueuedFilesQueueCount();

            //var prevSize = GUI.skin.label.fontSize;
            //GUI.skin.label.fontSize = (Screen.height / 1080) * 16;

            if (failedCount > 0) {
                style.normal.textColor = Color.red;
                var fileNr = failedCount == 1 ? "One file" : $"{failedCount} HKViz files";
                var text = failedCount == 1 ?
                    "HKViz: One file could not be uploaded. It can be retried from: Options > Mods > HKViz > Retry failed uploads" :
                    $"HKViz: {failedCount} files could not be uploaded. They can be retried from: Options > Mods > HKViz > Retry failed uploads";
                GUILayout.Label(text, style);
            }
            if (queuedFiles > 0) {
                style.normal.textColor = Color.white;
                GUILayout.Label(queuedFiles == 1 ? "HKViz: Upload in progress... One file left." : $"HKViz: Upload in progress... {queuedFiles} files left", style);
            } else if (recording.isRecording) {
                style.normal.textColor = Color.white;
                var nextUploadS = RecordingFileManager.Instance.GetNextPartInSeconds();

                if (nextUploadS == 0) {
                    GUILayout.Label($"HKViz: Next upload when leaving this room", style);
                } else {
                    var nextUploadFormat = SecondsToDurationFormatted(nextUploadS);
                    GUILayout.Label($"HKViz: Next upload in {nextUploadFormat} or when going to the main menu", style);
                }
            }

            if (HKVizVersionChecker.Instance.checkResponse?.show == true) {
                style.normal.textColor = HKVizVersionChecker.Instance.checkResponse.color switch {
                    "red" => Color.red,
                    "blue" => Color.blue,
                    "cyan" => Color.cyan,
                    "green" => Color.green,
                    _ => Color.white,
                };
                GUILayout.Label($"HKViz: {HKVizVersionChecker.Instance.checkResponse.message}", style);
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
