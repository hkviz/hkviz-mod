using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class TransitionGateExtraction(ExtractionFiles extractionFiles, ManualLogSource logger) {
    private void Log(string message) {
        logger.LogInfo(message);
    }

    public TransitionGateExportData Extract() {
        Log("Started transition gate extraction.");

        TransitionGateExportData gateData = new() { All = [] };
        TransitionGateExportData respawnPointData = new() { All = [] };

        Dictionary<string, SceneTeleportMap.SceneInfo>? teleportMap = SceneTeleportMap.GetTeleportMap();
        if (teleportMap == null) {
            Log("SceneTeleportMap not available; exporting empty transition gate list.");
            extractionFiles.ExportJson("transition-gate-export.json", gateData);
            extractionFiles.ExportJson("respawn-point-export.json", respawnPointData);
            return gateData;
        }

        HashSet<string> uniqueGateIds = new(StringComparer.Ordinal);
        HashSet<string> uniqueRespawnPointIds = new(StringComparer.Ordinal);

        foreach (var sceneInfo in teleportMap.Values) {
            if (sceneInfo == null || sceneInfo.TransitionGates == null) {
                continue;
            }

            foreach (var gateId in sceneInfo.TransitionGates) {
                if (!string.IsNullOrWhiteSpace(gateId)) {
                    uniqueGateIds.Add(gateId);
                }
            }

            foreach (var respawnPointId in sceneInfo.RespawnPoints) {
                if (!string.IsNullOrWhiteSpace(respawnPointId)) {
                    uniqueRespawnPointIds.Add(respawnPointId);
                }
            }
        }

        foreach (var gateId in uniqueGateIds.OrderBy(x => x, StringComparer.Ordinal)) {
            gateData.All.Add(new TransitionGateData {
                Id = gateId,
            });
        }

        foreach (var respawnPointId in uniqueRespawnPointIds.OrderBy(x => x, StringComparer.Ordinal)) {
            respawnPointData.All.Add(new TransitionGateData {
                Id = respawnPointId,
            });
        }

        extractionFiles.ExportJson("transition-gate-export.json", gateData);
        extractionFiles.ExportJson("respawn-point-export.json", respawnPointData);
        Log($"Finished transition gate extraction. Exported {gateData.All.Count} gate entries and {respawnPointData.All.Count} respawn point entries.");

        return gateData;
    }
}


