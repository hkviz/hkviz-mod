using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HKViz {
    internal class HKVizIMGUI: MonoBehaviour {

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

            var prevColor = GUI.color;
            //var prevSize = GUI.skin.label.fontSize;
            //GUI.skin.label.fontSize = (Screen.height / 1080) * 16;

            if (failedCount > 0) {
                style.normal.textColor = Color.red;
                var fileNr = failedCount == 1 ? "One HKViz file" : $"{failedCount} HKViz files";
                GUILayout.Label($"{fileNr} could not be uploaded. You can retry them from the settings", style);
            }
            if (queuedFiles > 0) {
                style.normal.textColor = Color.green;
                GUILayout.Label(queuedFiles == 1 ? "HKViz upload in progress. One file left." : $"HKViz Upload in progress. {queuedFiles} files left", style);
            }
            GUI.color = prevColor;
            //GUI.skin.label.fontSize = prevSize;

            GUILayout.EndArea();
        }
    }
}
