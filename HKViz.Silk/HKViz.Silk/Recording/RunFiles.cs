using System.IO;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HKViz.Silk.Recording;

public class RunFiles(string localRunId, long currentRunPart, ManualLogSource logger) {
    public string LocalRunId { get; } = localRunId;
    public long CurrentRunPart { get; private set; } = currentRunPart;

    private const float SwitchFileAfterSeconds = 60f * 10L; // 5 minutes
    
    private float _lastPartCreatedTime = 0f;
    private bool _isClosed = false;
    
    private StreamWriter? _writer;
    
    public void NextFileIfNeeded() {
        if (_isClosed) return;
        if (_writer == null || Time.unscaledTime - _lastPartCreatedTime > SwitchFileAfterSeconds) {
            NextFile();
        }
    }
    
    public float NextPartInSeconds => SwitchFileAfterSeconds - (Time.unscaledTime - _lastPartCreatedTime);

    public void NextFile() {
        if (_isClosed) return;
        var oldWriter = _writer;
        if (oldWriter != null) {
            CurrentRunPart++;
        }
        var newPath = RunFilePaths.GetRecordingPath(LocalRunId, CurrentRunPart);
        while (File.Exists(newPath)) {
            CurrentRunPart++;
            newPath = RunFilePaths.GetRecordingPath(LocalRunId, CurrentRunPart);
        }
        _writer = new StreamWriter(newPath);
        _lastPartCreatedTime = Time.unscaledTime;
        // TODO add new file to global storage of known files
        FinishFile(oldWriter);
    }

    public void Close() {
        if (_isClosed) return;
        _isClosed = true;
        FinishFile(_writer);
        _writer = null;
    }

    private void FinishFile(StreamWriter? writer) {
        writer?.Close();
        // TODO mark old file as closed -> ready for upload
    }

    public void WriteSceneChange(Scene scene, LoadSceneMode mode) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write scene change but writer is null");
            return;
        }
        writer.Write(scene.name);
        writer.Write(";");
        writer.WriteLine(mode.ToString());
    }
}
