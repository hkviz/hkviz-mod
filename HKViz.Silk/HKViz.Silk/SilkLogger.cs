using System;
using BepInEx.Logging;
using HKViz.Shared;

namespace HKViz.Silk;

public class SilkLogger(ManualLogSource logger): IHkVizLogger {
    public void LogInfo(string message) {
        logger.LogInfo(message);
    }

    public void LogError(string message) {
        logger.LogError(message);
    }

    public void LogError(Exception ex) {
        logger.LogError(ex);
        logger.LogError(ex.StackTrace);
    }
}
