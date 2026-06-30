using BepInEx.Logging;
using System.Reflection;
using TeamCherry.Localization;

namespace HKViz.Silk.Extraction;

public class QuestExtraction(ExtractionFiles extractionFiles, LocalizationExtraction localizationExtraction, ManualLogSource logger) {
    private void Log(string message) {
        logger.LogInfo(message);
    }

    public QuestExportData Extract() {
        Log("Started quest extraction.");

        QuestExportData data = new() {
            All = [],
        };

        foreach (BasicQuestBase quest in QuestManager.GetAllQuests()) {
            if (quest == null) {
                continue;
            }

            string? displayNameKey = null;
            if (!string.IsNullOrEmpty(quest.DisplayName.Sheet) && !string.IsNullOrEmpty(quest.DisplayName.Key)) {
                displayNameKey = localizationExtraction.RequestExport(quest.DisplayName);
            }

            QuestType? questType = quest.QuestType;
            string? typeDisplayNameKey = null;
            Vector4Data? typeTextColor = null;
            QuestTypeIconsData? typeIcons = null;

            if (questType != null) {
                var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var displayNameField = questType.GetType().GetField("displayName", flags);
                if (displayNameField?.GetValue(questType) is LocalisedString typeDisplayName
                    && !string.IsNullOrEmpty(typeDisplayName.Sheet)
                    && !string.IsNullOrEmpty(typeDisplayName.Key)) {
                    typeDisplayNameKey = localizationExtraction.RequestExport(typeDisplayName);
                }

                typeTextColor = Vector4Data.FromColor(questType.TextColor);
                typeIcons = new QuestTypeIconsData {
                    Icon = questType.Icon.ToSpriteInfoSafe(logger, $"Quest:{quest.name}:TypeIcon"),
                    CanCompleteIcon = questType.CanCompleteIcon.ToSpriteInfoSafe(logger, $"Quest:{quest.name}:TypeCanCompleteIcon"),
                    LargeIcon = questType.LargeIcon.ToSpriteInfoSafe(logger, $"Quest:{quest.name}:TypeLargeIcon"),
                    LargeIconGlow = questType.LargeIconGlow.ToSpriteInfoSafe(logger, $"Quest:{quest.name}:TypeLargeIconGlow"),
                };
            }

            QuestData questData = new() {
                Id = quest.name,
                RuntimeType = quest.GetType().Name,
                DisplayNameKey = displayNameKey,
                TypeDisplayNameKey = typeDisplayNameKey,
                TypeTextColor = typeTextColor,
                TypeIcons = typeIcons,
            };

            data.All.Add(questData);
        }

        extractionFiles.ExportJson("quest-export.json", data);
        Log($"Finished quest extraction. Exported {data.All.Count} quest entries.");

        return data;
    }
}



