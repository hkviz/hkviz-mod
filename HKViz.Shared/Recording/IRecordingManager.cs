namespace HKViz.Shared.Recording;

public interface IRecordingManager {
    public bool IsRecording { get; }
    public float NextPartInSeconds { get; }
}
