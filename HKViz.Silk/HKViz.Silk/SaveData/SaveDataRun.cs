namespace HKViz.Silk.SaveData;

public class SaveDataRun(string localRunId, long nextRunPart) {
    public string LocalRunId { get; } = localRunId;
    public long NextRunPart { get; } = nextRunPart;
}
