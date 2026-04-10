using BepInEx.Logging;
using HKViz.Shared.Upload;
using UnityEngine.SceneManagement;

namespace HKViz.Silk.Recording;

public class RunWriter {
    public string LocalRunId { get; }

    private bool isClosed = false;
    private readonly RunFiles runFiles;
    private readonly HeroLocationWriter heroLocationWriter;
    private readonly ManualLogSource logger;
    
    public RunWriter(string localRunId, long nextRunPart, UploadManager uploadManager, ManualLogSource logger) {
        LocalRunId = localRunId;
        this.logger = logger;
        runFiles = new RunFiles(localRunId, nextRunPart, uploadManager, logger);
        heroLocationWriter = new HeroLocationWriter(runFiles);
    }

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
    
    public float NextPartInSeconds => runFiles.NextPartInSeconds;

    public long GetNextRunPart() {
        return runFiles.CurrentRunPart + 1L;
    }

    public void Update() {
        if (isClosed) return;
        var gm = GameManager._instance;
        if (!gm || !gm.IsGamePaused()) return;
        heroLocationWriter.Update();
    }
}
