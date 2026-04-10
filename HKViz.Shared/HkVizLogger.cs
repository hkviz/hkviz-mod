using System;

namespace HKViz.Shared;

public interface IHkVizLogger {
    public void LogInfo(string message);
    public void LogError(string message);
    public void LogError(Exception ex);
}