using BepInEx.Logging;
using UnityEngine.SceneManagement;

namespace HKViz.Silk.Recording;

public class RunWriter(string localRunId, long nextRunPart, ManualLogSource logger) {
    public string LocalRunId { get; } = localRunId;

    private bool isClosed = false;
    private readonly RunFiles runFiles = new(localRunId, nextRunPart, logger);

    private ManualLogSource logger = logger;

    public void Close() {
        if (isClosed) return;
        isClosed = true;
        runFiles.Close();
    }

    public void HandleSceneChange(Scene scene, LoadSceneMode mode) {
        logger.LogDebug($"Scene changed to {scene.name} with mode {mode}");
        // only called with scene != menu --> in-game scene
        runFiles.NextFileIfNeeded();
        runFiles.WriteSceneChange(scene, mode);
    }

    public long GetNextRunPart() {
        return runFiles.CurrentRunPart + 1L;
    }

    public void Update() {
        if (!GameManager.instance.IsGamePaused()) return;
    }
}
