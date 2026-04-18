using System;
using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class ToolItemExtraction(ExtractionFiles extractionFiles, LocalizationExtraction localizationExtraction, ManualLogSource logger) {
    private void Log(string message) {
        logger.LogInfo(message);
    }

    private void LogError(string message, Exception? ex = null) {
        logger.LogError(message);
        if (ex != null) {
            logger.LogError(ex);
        }
    }

    public ToolItemExportData? Extract() {
        try {
            Log("Started tool item extraction.");

            var tools = ToolItemManager.GetAllTools();
            ToolItemExportData data = new() {
                Tools = [],
            };

            foreach (ToolItem tool in tools) {
                if (tool == null) {
                    continue;
                }

                string toolId = tool.name;
                string? displayName = null;
                if (!string.IsNullOrEmpty(tool.DisplayName.Sheet) && !string.IsNullOrEmpty(tool.DisplayName.Key)) {
                    displayName = localizationExtraction.RequestExport(tool.DisplayName);
                }

                var toolSprite = tool.GetInventorySprite(ToolItem.IconVariants.Default);

                ToolItemData toolData = new() {
                    Name = tool.name,
                    Id = toolId,
                    DisplayName = displayName,
                    Type = tool.Type.ToString(),
                    ToolSprite = toolSprite ? SpriteInfo.FromSprite(toolSprite) : null,
                    AlternateUnlockedTest = tool.GetAlternateUnlockedTest().ToExportData(),
                };

                data.Tools.Add(toolData);
            }

            extractionFiles.ExportJson("tool-item-export.json", data);
            Log($"Finished tool item extraction. Exported {data.Tools.Count} tool entries.");

            return data;
        }
        catch (Exception ex) {
            LogError($"Fatal error during tool item extraction: {ex.Message}", ex);
            return null;
        }
    }
}


