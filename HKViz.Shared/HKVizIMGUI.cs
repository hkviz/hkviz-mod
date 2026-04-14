using HKViz.Shared.Auth;
using HKViz.Shared.Recording;
using HKViz.Shared.Upload;
using UnityEngine;

namespace HKViz.Shared;

internal class HkVizIMGUI : MonoBehaviour {
    private static readonly Color MutedRed = new Color32(218, 93, 97, 255);
    private static readonly Color MutedWhite = new Color32(240, 239, 234, 255);
    private static readonly Color MutedBlue = new Color32(93, 148, 222, 255);
    private static readonly Color MutedCyan = new Color32(73, 220, 231, 255);
    private static readonly Color MutedGreen = new Color32(109, 206, 104, 255);

    private readonly IRecordingManager recordingManager = HkVizSharedInstances.Instance!.recordingManager;
    private readonly UploadManager uploadManager = HkVizSharedInstances.Instance.uploadManager;
    private readonly AuthManager authManager = HkVizSharedInstances.Instance.authManager;
    private readonly VersionChecker versionChecker = HkVizSharedInstances.Instance.versionChecker;
    private GameManager? gm;
    private HeroController? hero;

    private GUIStyle style = new() {
        wordWrap = true,
        richText = true,
    };

    private string SecondsToDurationFormatted(float seconds) {
        var secondsInt = (int)seconds;
        var m = secondsInt / 60;
        var s = secondsInt % 60;

        var sStr = s switch {
            0 => null,
            1 => "1 second",
            _ => $"{s} seconds",
        };
        var mStr = m switch {
            0 => null,
            1 => "1 minute",
            _ => $"{m} minutes",
        };
        
        if (mStr != null && sStr != null) {
            return $"{mStr} {sStr}";
        }

        return sStr ?? mStr ?? "Now";
    }

    public void OnGUI() {
        if (!gm) {
            gm = GameManager.UnsafeInstance;
        }

        if (!hero) {
            hero = HeroController.UnsafeInstance;
        }

        var isPaused = gm && gm.isPaused;
        var isTransitioning = hero && hero.cState.transitioning;

        if ((!isPaused || isTransitioning) && recordingManager.IsRecording) {
            return;
        }


        var fontScale = Mathf.Min(
            Screen.height / 1080f,
            Screen.width / 1920f
        );
        style.fontSize = Mathf.Clamp(Mathf.RoundToInt(25 * fontScale), 12, 72);

        var areaWidth = Screen.width / 4f;
        var areaHeight = Screen.height / 4f;
        GUILayout.BeginArea(new Rect(
            x: (Screen.width - areaWidth) / 2f,
            y: (Screen.height - areaHeight) - style.fontSize,
            width: areaWidth,
            height: areaHeight
        ), style);


        GUILayout.FlexibleSpace();
        var failedCount = uploadManager.FailedUploadsQueueCount();
        var queuedFiles = uploadManager.QueuedFilesQueueCount();

        if (failedCount > 0) {
            style.normal.textColor = MutedRed;
            var text = failedCount == 1 ?
                "<b>HKViz</b>: One file could not be uploaded. It can be retried from: <i>Options > Mods > HKViz > Retry failed uploads</i>" :
                $"<b>HKViz</b>: {failedCount} files could not be uploaded. They can be retried from: <i>Options > Mods > HKViz > Retry failed uploads</i>";
            GUILayout.Label(text, style);
        }
        if (queuedFiles > 0) {
            style.normal.textColor = MutedWhite;
            GUILayout.Label(queuedFiles == 1 ? "<b>HKViz</b>: Upload in progress... One file left." : $"<b>HKViz</b>: Upload in progress... {queuedFiles} files left", style);
        } else if (recordingManager.IsRecording) {
            style.normal.textColor = MutedWhite;
            var nextUploadS = recordingManager.NextPartInSeconds;

            if (nextUploadS == 0) {
                GUILayout.Label($"<b>HKViz</b>: Next upload when leaving this room", style);
            } else {
                var nextUploadFormat = SecondsToDurationFormatted(nextUploadS);
                GUILayout.Label($"<b>HKViz</b>: Next upload in {nextUploadFormat} or when going to the main menu", style);
            }
        }

        if (versionChecker.checkResponse?.show == true) {
            style.normal.textColor = versionChecker.checkResponse.color switch {
                "red" => MutedRed,
                "blue" => MutedBlue,
                "cyan" => MutedCyan,
                "green" => MutedGreen,
                _ => MutedWhite,
            };
            GUILayout.Label($"<b>HKViz</b>: {versionChecker.checkResponse.message}", style);
        }

        var loginState = authManager.State;
        if (loginState == LoginState.LOADING_LOGIN_URL) {
            style.normal.textColor = MutedWhite;
            GUILayout.Label($"<b>HKViz</b>: ...Loading login url...", style);
        } else if (loginState == LoginState.WAITING_FOR_USER_LOGIN_IN_BROWSER) {
            style.normal.textColor = MutedWhite;
            GUILayout.Label($"<b>HKViz</b>: Please login inside the opened browser window", style);
        }

        if (authManager.ShowLoginSuccess) {
            style.normal.textColor = MutedGreen;
            GUILayout.Label($"<b>HKViz</b>: Login successful", style);
        }

        GUILayout.EndArea();
    }
}
