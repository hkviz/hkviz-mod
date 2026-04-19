using System;
using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class ToolCrestExtraction(ExtractionFiles extractionFiles, LocalizationExtraction localizationExtraction, ManualLogSource logger) {
    private void Log(string message) {
        logger.LogInfo(message);
    }

    private void LogError(string message, Exception? ex = null) {
        logger.LogError(message);
        if (ex != null)
            logger.LogError(ex);
    }

    public ToolCrestExportData? Extract() {
        try {
            Log("Started tool crest extraction.");

            var crests = ToolItemManager.GetAllCrests();
            var data = new ToolCrestExportData {
                All = [],
            };

            foreach (var crest in crests) {
                if (crest == null)
                    continue;

                var crestId = crest.name;
                string? displayName = null;
                if (!string.IsNullOrEmpty(crest.DisplayName.Sheet) && !string.IsNullOrEmpty(crest.DisplayName.Key)) {
                    displayName = localizationExtraction.RequestExport(crest.DisplayName);
                }

                var crestData = new ToolCrestData {
                    Id = crestId, // same as crest.name
                    DisplayName = displayName,
                    CrestSprite = crest.CrestSprite.ToSpriteInfoSafe(logger, $"ToolCrest:{crestId}:CrestSprite"),
                };
                data.All.Add(crestData);
            }

            extractionFiles.ExportJson("tool-crest-export.json", data);
            Log($"Finished tool crest extraction. Exported {data.All.Count} crest entries.");

            return data;
        }
        catch (Exception ex) {
            LogError($"Fatal error during tool crest extraction: {ex.Message}", ex);
            return null;
        }
    }
}
