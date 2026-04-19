using System.Collections.Generic;
using System.Reflection;
using BepInEx.Logging;
using TeamCherry.Localization;

namespace HKViz.Silk.Extraction;

public class CollectableRelicExtraction(ExtractionFiles extractionFiles, LocalizationExtraction localizationExtraction, ManualLogSource logger) {
    private void Log(string message) {
        logger.LogInfo(message);
    }

    public CollectableRelicExportData? Extract() {
        Log("Started collectable relic extraction.");

        var relics = GetAllRelics();
        CollectableRelicExportData data = new() {
            All = [],
        };

        foreach (CollectableRelic relic in relics) {
            if (relic == null) {
                continue;
            }

            var relicType = relic.RelicType;
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var collectionIcon = relic.GetCollectionIcon();
            var popupIcon = relic.GetPopupIcon();
            var relicTypeInventoryIcon = relicType != null ? relicType.GetIcon(CollectableItem.ReadSource.Inventory) : null;

            string? typeNameKey = null;
            string? typeDescriptionKey = null;
            string? appendDescriptionKey = null;
            if (relicType != null) {
                var relicTypeType = relicType.GetType();

                var typeNameField = relicTypeType.GetField("typeName", flags);
                if (typeNameField?.GetValue(relicType) is LocalisedString typeNameValue && !string.IsNullOrEmpty(typeNameValue.Sheet) && !string.IsNullOrEmpty(typeNameValue.Key)) {
                    typeNameKey = localizationExtraction.RequestExport(typeNameValue);
                }

                var typeDescriptionField = relicTypeType.GetField("typeDescription", flags);
                if (typeDescriptionField?.GetValue(relicType) is LocalisedString typeDescriptionValue && !string.IsNullOrEmpty(typeDescriptionValue.Sheet) && !string.IsNullOrEmpty(typeDescriptionValue.Key)) {
                    typeDescriptionKey = localizationExtraction.RequestExport(typeDescriptionValue);
                }

                var appendDescriptionField = relicTypeType.GetField("appendDescription", flags);
                if (appendDescriptionField?.GetValue(relicType) is LocalisedString appendDescriptionValue && !string.IsNullOrEmpty(appendDescriptionValue.Sheet) && !string.IsNullOrEmpty(appendDescriptionValue.Key)) {
                    appendDescriptionKey = localizationExtraction.RequestExport(appendDescriptionValue);
                }
            }

            CollectableRelicData relicData = new() {
                Id = relic.name, // same as relic.name
                RuntimeType = relic.GetType().Name,

                TypeNameKey = typeNameKey,
                TypeDescriptionKey = typeDescriptionKey,
                AppendDescriptionKey = appendDescriptionKey,

                CollectionIcon = collectionIcon.ToSpriteInfoSafe(logger, $"CollectableRelic:{relic.name}:CollectionIcon"),
                PopupIcon = popupIcon.ToSpriteInfoSafe(logger, $"CollectableRelic:{relic.name}:PopupIcon"),

                IsInInventory = relic.IsInInventory,
                IsPlayable = relic.IsPlayable,
                PlaySyncedAudioSource = relic.PlaySyncedAudioSource,
                WillSendPlayEvent = relic.WillSendPlayEvent,

                RelicTypeId = relicType ? relicType.name : null,
                RelicTypeInventoryIcon = relicTypeInventoryIcon.ToSpriteInfoSafe(logger, $"CollectableRelic:{relic.name}:RelicTypeInventoryIcon"),
            };

            data.All.Add(relicData);
        }

        extractionFiles.ExportJson("collectable-relic-export.json", data);
        Log($"Finished collectable relic extraction. Exported {data.All.Count} relic entries.");

        return data;
    }

    private List<CollectableRelic> GetAllRelics() {
        var manager = ManagerSingleton<CollectableRelicManager>.Instance;
        if (!manager) {
            Log("CollectableRelicManager instance not found; exporting empty list.");
            return [];
        }

        var relics = CollectableRelicManager.GetAllRelics();
        if (relics == null) {
            return [];
        }

        List<CollectableRelic> result = [];
        foreach (var relic in relics) {
            if (relic != null) {
                result.Add(relic);
            }
        }

        return result;
    }

}

