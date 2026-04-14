using System;
using HKViz.Shared;
using Modding;

namespace HKViz;

public class HollowLogger(Loggable logger): IHkVizLogger {
    public void LogInfo(string message) {
        logger.Log(message);
    }

    public void LogError(string message) {
        logger.LogError(message);
    }

    public void LogError(Exception ex) {
        logger.LogError(ex);
    }
}