using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Logging;
using TeamCherry.Localization;

namespace HKViz.Silk.Extraction;

public class CollectableExtraction(ExtractionFiles extractionFiles, LocalizationExtraction localizationExtraction, ManualLogSource logger) {
    private static readonly CollectableItem.ReadSource[] AllReadSources = (CollectableItem.ReadSource[])Enum.GetValues(typeof(CollectableItem.ReadSource));

    private void Log(string message) {
        logger.LogInfo(message);
    }

    public CollectableExportData? Extract() {
        Log("Started collectable extraction.");

        var collectables = GetAllCollectables();
        CollectableExportData data = new() {
            All = [],
        };

        foreach (CollectableItem item in collectables) {
            if (item == null) {
                continue;
            }

            var displayNameKeysBySource = BuildDisplayNameKeysBySource(item);
            var descriptionKeysBySource = BuildDescriptionKeysBySource(item);
            var extraDescriptionKeys = BuildExtraDescriptionKeys(item);
            var useResponseDescriptionKeys = BuildUseResponseDescriptionKeys(item);
            var useResponses = BuildUseResponses(item);

            var iconInventory = TryGetIcon(item, CollectableItem.ReadSource.Inventory);
            var iconPopup = TryGetIcon(item, CollectableItem.ReadSource.GetPopup);
            var iconTiny = TryGetIcon(item, CollectableItem.ReadSource.Tiny);
            var iconShop = TryGetIcon(item, CollectableItem.ReadSource.Shop);
            var iconTakePopup = TryGetIcon(item, CollectableItem.ReadSource.TakePopup);

            var useResponseTextOverride = item.UseResponseTextOverride;
            var useResponseTextOverrideKey = TryRequestExport(useResponseTextOverride);

            var itemData = new CollectableData {
                Id = item.name, // same as collectable.name
                RuntimeType = item.GetType().Name,

                DisplayNameKey = GetPrimaryKey(displayNameKeysBySource),
                DescriptionKey = GetPrimaryKey(descriptionKeysBySource),
                UseResponseTextOverrideKey = useResponseTextOverrideKey,
                DisplayNameKeysBySource = HasAnyKeys(displayNameKeysBySource) ? displayNameKeysBySource : null,
                DescriptionKeysBySource = HasAnyKeys(descriptionKeysBySource) ? descriptionKeysBySource : null,
                ExtraDescriptionKeys = extraDescriptionKeys.Count > 0 ? extraDescriptionKeys : null,
                UseResponseDescriptionKeys = useResponseDescriptionKeys.Count > 0 ? useResponseDescriptionKeys : null,
                UseResponses = useResponses.Count > 0 ? useResponses : null,

                IconInventory = iconInventory.ToSpriteInfoSafe(logger, $"Collectable:{item.name}:IconInventory"),
                IconPopup = iconPopup.ToSpriteInfoSafe(logger, $"Collectable:{item.name}:IconPopup"),
                IconTiny = iconTiny.ToSpriteInfoSafe(logger, $"Collectable:{item.name}:IconTiny"),
                IconShop = iconShop.ToSpriteInfoSafe(logger, $"Collectable:{item.name}:IconShop"),
                IconTakePopup = iconTakePopup.ToSpriteInfoSafe(logger, $"Collectable:{item.name}:IconTakePopup"),

                DisplayAmount = item.DisplayAmount,
                IsConsumable = item.IsConsumable(),
                IsVisibleWithBareInventory = item.IsVisibleWithBareInventory,
                HideInShopCounters = item.HideInShopCounters,
                TakeItemOnConsume = item.TakeItemOnConsume,
            };

            data.All.Add(itemData);
        }

        extractionFiles.ExportJson("collectable-export.json", data);
        Log($"Finished collectable extraction. Exported {data.All.Count} collectable entries.");

        return data;
    }

    private Dictionary<string, List<string>> BuildDisplayNameKeysBySource(CollectableItem item) {
        var keys = CreateSourceMap();
        AddBaseDisplayAndDescriptionFields(item, keys, null);

        if (item is CollectableItemStack stackItem) {
            AddStackDisplayNameKeys(stackItem, keys);
        }

        if (item is CollectableItemPlayerDataStack playerDataStackItem) {
            AddPlayerDataStackDisplayNameKeys(playerDataStackItem, keys);
        }

        if (item is CollectableItemGrower growerItem) {
            AddGrowerDisplayNameKeys(growerItem, keys);
        }

        if (item is CollectableItemStates statesItem) {
            AddStatesDisplayNameKeys(statesItem, keys);
        }

        if (item is CollectableItemQuestDisplay questDisplayItem) {
            AddQuestDisplayNameKeys(questDisplayItem, keys);
        }

        if (item is CollectableItemRelicType relicTypeItem) {
            AddRelicTypeDisplayNameKeys(relicTypeItem, keys);
        }

        return keys;
    }

    private Dictionary<string, List<string>> BuildDescriptionKeysBySource(CollectableItem item) {
        var keys = CreateSourceMap();
        AddBaseDisplayAndDescriptionFields(item, null, keys);

        if (item is CollectableItemStack stackItem) {
            AddStackDescriptionKeys(stackItem, keys);
        }

        if (item is CollectableItemGrower growerItem) {
            AddGrowerDescriptionKeys(growerItem, keys);
        }

        if (item is CollectableItemStates statesItem) {
            AddStatesDescriptionKeys(statesItem, keys);
        }

        if (item is CollectableItemQuestDisplay questDisplayItem) {
            AddQuestDescriptionKeys(questDisplayItem, keys);
        }

        if (item is CollectableItemRelicType relicTypeItem) {
            AddRelicTypeDescriptionKeys(relicTypeItem, keys);
        }

        if (item.GetType().Name == "CollectableItemMemento" && TryReadLocalisedField(item, "extraDesc", out var extraDesc)) {
            foreach (var source in AllReadSources.Where(x => x != CollectableItem.ReadSource.Shop)) {
                AddKey(keys, source, extraDesc);
            }
        }

        if (item is CollectableItemPlayerDataStack playerDataStackItem) {
            AddPlayerDataStackDescriptionKeys(playerDataStackItem, keys);
        }

        return keys;
    }

    private List<string> BuildExtraDescriptionKeys(CollectableItem item) {
        var keys = new List<string>();

        if (item.GetType().Name == "CollectableItemMemento" && TryReadLocalisedField(item, "extraDesc", out var extraDesc)) {
            AddKey(keys, extraDesc);
        }

        if (item is CollectableItemStates statesItem) {
            foreach (var append in ReadFieldEnumerable(statesItem, "appends")) {
                if (TryReadLocalisedField(append, "Text", out var text)) {
                    AddKey(keys, text);
                }
            }
        }

        if (item is CollectableItemPlayerDataStack playerDataStackItem && TryReadLocalisedField(playerDataStackItem, "stackDescHeader", out var stackDescHeader)) {
            AddKey(keys, stackDescHeader);
        }

        return keys;
    }

    private List<string> BuildUseResponseDescriptionKeys(CollectableItem item) {
        var keys = new List<string>();

        AddUseResponseDescriptionKeys(keys, ReadFieldEnumerable(item, "useResponses"));

        if (item is CollectableItemGrower growerItem) {
            foreach (var state in ReadFieldEnumerable(growerItem, "states")) {
                AddUseResponseDescriptionKeys(keys, ReadFieldEnumerable(state, "UseResponses"));
            }
        }

        return keys;
    }

    private List<CollectableUseResponseData> BuildUseResponses(CollectableItem item) {
        var responses = new List<CollectableUseResponseData>();

        AddUseResponses(responses, ReadFieldEnumerable(item, "useResponses"), "Item", null);

        if (item is CollectableItemGrower growerItem) {
            var stateIndex = 0;
            foreach (var state in ReadFieldEnumerable(growerItem, "states")) {
                AddUseResponses(responses, ReadFieldEnumerable(state, "UseResponses"), "GrowerState", stateIndex);
                stateIndex++;
            }
        }

        return responses;
    }

    private void AddUseResponses(List<CollectableUseResponseData> target, IEnumerable<object> useResponses, string sourceKind, int? stateIndex) {
        foreach (var response in useResponses) {
            var responseData = BuildUseResponseData(response, sourceKind, stateIndex);
            if (responseData != null) {
                target.Add(responseData);
            }
        }
    }

    private CollectableUseResponseData? BuildUseResponseData(object response, string sourceKind, int? stateIndex) {
        var descriptionKey = TryReadLocalisedField(response, "Description", out var description) ? TryRequestExport(description) : null;
        var useType = ReadFieldValue(response, "UseType")?.ToString();

        var amount = 0;
        if (ReadFieldValue(response, "Amount") is int amountValue) {
            amount = amountValue;
        }

        var amountRangeStart = 0;
        var amountRangeEnd = 0;
        var amountRange = ReadFieldValue(response, "AmountRange");
        if (amountRange != null) {
            if (ReadFieldValue(amountRange, "Start") is int startValue) {
                amountRangeStart = startValue;
            }

            if (ReadFieldValue(amountRange, "End") is int endValue) {
                amountRangeEnd = endValue;
            }
        }

        return new CollectableUseResponseData {
            SourceKind = sourceKind,
            StateIndex = stateIndex,
            UseType = useType,
            Amount = amount,
            AmountRangeStart = amountRangeStart,
            AmountRangeEnd = amountRangeEnd,
            DescriptionKey = descriptionKey,
        };
    }

    private void AddUseResponseDescriptionKeys(List<string> keys, IEnumerable<object> useResponses) {
        foreach (var response in useResponses) {
            if (TryReadLocalisedField(response, "Description", out var description)) {
                AddKey(keys, description);
            }
        }
    }

    private void AddBaseDisplayAndDescriptionFields(CollectableItem item, Dictionary<string, List<string>>? displayMap, Dictionary<string, List<string>>? descriptionMap) {
        if (TryReadLocalisedField(item, "displayName", out var displayName)) {
            foreach (var source in AllReadSources) {
                if (displayMap != null) {
                    AddKey(displayMap, source, displayName);
                }
            }
        }

        if (TryReadLocalisedField(item, "description", out var description)) {
            foreach (var source in AllReadSources) {
                if (descriptionMap != null) {
                    AddKey(descriptionMap, source, description);
                }
            }
        }
    }

    private void AddStackDisplayNameKeys(CollectableItemStack item, Dictionary<string, List<string>> target) {
        if (TryReadLocalisedField(item, "singleDisplayName", out var singleDisplayName)) {
            foreach (var source in AllReadSources) {
                AddKey(target, source, singleDisplayName);
            }
        }

        if (TryReadLocalisedField(item, "pluralDisplayName", out var pluralDisplayName)) {
            AddKey(target, CollectableItem.ReadSource.Inventory, pluralDisplayName);
            AddKey(target, CollectableItem.ReadSource.TakePopup, pluralDisplayName);
        }

        if (TryReadLocalisedField(item, "allDisplayName", out var allDisplayName)) {
            AddKey(target, CollectableItem.ReadSource.Inventory, allDisplayName);
            AddKey(target, CollectableItem.ReadSource.TakePopup, allDisplayName);
        }
    }

    private void AddStackDescriptionKeys(CollectableItemStack item, Dictionary<string, List<string>> target) {
        if (TryReadLocalisedField(item, "singleDescription", out var singleDescription)) {
            foreach (var source in AllReadSources) {
                AddKey(target, source, singleDescription);
            }
        }

        if (TryReadLocalisedField(item, "pluralDescription", out var pluralDescription)) {
            AddKey(target, CollectableItem.ReadSource.Inventory, pluralDescription);
            AddKey(target, CollectableItem.ReadSource.TakePopup, pluralDescription);
        }

        if (TryReadLocalisedField(item, "allDescription", out var allDescription)) {
            AddKey(target, CollectableItem.ReadSource.Inventory, allDescription);
            AddKey(target, CollectableItem.ReadSource.TakePopup, allDescription);
        }
    }

    private void AddGrowerDisplayNameKeys(CollectableItemGrower item, Dictionary<string, List<string>> target) {
        var states = ReadFieldEnumerable(item, "states").ToList();
        foreach (var source in AllReadSources) {
            if (source == CollectableItem.ReadSource.GetPopup || source == CollectableItem.ReadSource.Shop) {
                var last = states.LastOrDefault();
                if (last != null && TryReadLocalisedField(last, "DisplayName", out var popupDisplayName)) {
                    AddKey(target, source, popupDisplayName);
                }
                continue;
            }

            foreach (var state in states) {
                if (TryReadLocalisedField(state, "DisplayName", out var stateDisplayName)) {
                    AddKey(target, source, stateDisplayName);
                }
            }
        }
    }

    private void AddGrowerDescriptionKeys(CollectableItemGrower item, Dictionary<string, List<string>> target) {
        var states = ReadFieldEnumerable(item, "states").ToList();
        foreach (var source in AllReadSources) {
            if (source == CollectableItem.ReadSource.GetPopup || source == CollectableItem.ReadSource.Shop) {
                var last = states.LastOrDefault();
                if (last != null && TryReadLocalisedField(last, "Description", out var popupDescription)) {
                    AddKey(target, source, popupDescription);
                }
                continue;
            }

            foreach (var state in states) {
                if (TryReadLocalisedField(state, "Description", out var stateDescription)) {
                    AddKey(target, source, stateDescription);
                }
            }
        }
    }

    private void AddStatesDisplayNameKeys(CollectableItemStates item, Dictionary<string, List<string>> target) {
        foreach (var state in ReadFieldEnumerable(item, "states")) {
            if (TryReadLocalisedField(state, "DisplayName", out var displayName)) {
                foreach (var source in AllReadSources) {
                    AddKey(target, source, displayName);
                }
            }
        }
    }

    private void AddStatesDescriptionKeys(CollectableItemStates item, Dictionary<string, List<string>> target) {
        foreach (var state in ReadFieldEnumerable(item, "states")) {
            if (TryReadLocalisedField(state, "Description", out var description)) {
                foreach (var source in AllReadSources) {
                    AddKey(target, source, description);
                }
            }

            if (TryReadLocalisedField(state, "DescriptionExtra", out var descriptionExtra)) {
                foreach (var source in AllReadSources) {
                    AddKey(target, source, descriptionExtra);
                }
            }
        }

        foreach (var append in ReadFieldEnumerable(item, "appends")) {
            if (TryReadLocalisedField(append, "Text", out var appendText)) {
                foreach (var source in AllReadSources) {
                    AddKey(target, source, appendText);
                }
            }
        }
    }

    private void AddQuestDisplayNameKeys(CollectableItemQuestDisplay item, Dictionary<string, List<string>> target) {
        foreach (var state in ReadFieldEnumerable(item, "states")) {
            if (TryReadLocalisedField(state, "DisplayName", out var displayName)) {
                foreach (var source in AllReadSources) {
                    AddKey(target, source, displayName);
                }
            }
        }

        if (TryReadLocalisedField(item, "pickupDisplayName", out var pickupDisplayName)) {
            AddKey(target, CollectableItem.ReadSource.GetPopup, pickupDisplayName);
            AddKey(target, CollectableItem.ReadSource.TakePopup, pickupDisplayName);
        }
    }

    private void AddQuestDescriptionKeys(CollectableItemQuestDisplay item, Dictionary<string, List<string>> target) {
        foreach (var state in ReadFieldEnumerable(item, "states")) {
            if (TryReadLocalisedField(state, "Description", out var description)) {
                foreach (var source in AllReadSources) {
                    AddKey(target, source, description);
                }
            }
        }
    }

    private void AddRelicTypeDisplayNameKeys(CollectableItemRelicType item, Dictionary<string, List<string>> target) {
        if (TryReadLocalisedField(item, "typeName", out var typeName)) {
            foreach (var source in AllReadSources) {
                AddKey(target, source, typeName);
            }
        }
    }

    private void AddRelicTypeDescriptionKeys(CollectableItemRelicType item, Dictionary<string, List<string>> target) {
        if (TryReadLocalisedField(item, "typeDescription", out var typeDescription)) {
            foreach (var source in AllReadSources) {
                AddKey(target, source, typeDescription);
            }
        }

        if (TryReadLocalisedField(item, "appendDescription", out var appendDescription)) {
            foreach (var source in AllReadSources) {
                AddKey(target, source, appendDescription);
            }
        }
    }

    private void AddPlayerDataStackDisplayNameKeys(CollectableItemPlayerDataStack item, Dictionary<string, List<string>> target) {
        foreach (var stackItem in ReadFieldEnumerable(item, "stackItems")) {
            if (TryReadLocalisedField(stackItem, "Name", out var name)) {
                AddKey(target, CollectableItem.ReadSource.GetPopup, name);
                AddKey(target, CollectableItem.ReadSource.TakePopup, name);
            }
        }
    }

    private void AddPlayerDataStackDescriptionKeys(CollectableItemPlayerDataStack item, Dictionary<string, List<string>> target) {
        if (TryReadLocalisedField(item, "stackDescHeader", out var stackDescHeader)) {
            foreach (var source in AllReadSources) {
                AddKey(target, source, stackDescHeader);
            }
        }

        foreach (var stackItem in ReadFieldEnumerable(item, "stackItems")) {
            if (TryReadLocalisedField(stackItem, "Name", out var name)) {
                foreach (var source in AllReadSources) {
                    AddKey(target, source, name);
                }
            }
        }
    }

    private static Dictionary<string, List<string>> CreateSourceMap() {
        var map = new Dictionary<string, List<string>>();
        foreach (var source in AllReadSources) {
            map[source.ToString()] = [];
        }
        return map;
    }

    private void AddKey(Dictionary<string, List<string>> map, CollectableItem.ReadSource source, LocalisedString localisedString) {
        var key = TryRequestExport(localisedString);
        if (key == null) {
            return;
        }

        var sourceKey = source.ToString();
        if (!map.TryGetValue(sourceKey, out var list)) {
            list = [];
            map[sourceKey] = list;
        }

        if (!list.Contains(key)) {
            list.Add(key);
        }
    }

    private void AddKey(List<string> list, LocalisedString localisedString) {
        var key = TryRequestExport(localisedString);
        if (key != null && !list.Contains(key)) {
            list.Add(key);
        }
    }

    private string? TryRequestExport(LocalisedString localisedString) {
        if (string.IsNullOrEmpty(localisedString.Sheet) || string.IsNullOrEmpty(localisedString.Key)) {
            return null;
        }

        return localizationExtraction.RequestExport(localisedString);
    }

    private static bool TryReadLocalisedField(object target, string fieldName, out LocalisedString localisedString) {
        var field = GetFieldInHierarchy(target.GetType(), fieldName);
        if (field?.GetValue(target) is LocalisedString value) {
            localisedString = value;
            return true;
        }

        localisedString = default;
        return false;
    }

    private static object? ReadFieldValue(object target, string fieldName) {
        var field = GetFieldInHierarchy(target.GetType(), fieldName);
        return field?.GetValue(target);
    }

    private static IEnumerable<object> ReadFieldEnumerable(object target, string fieldName) {
        var field = GetFieldInHierarchy(target.GetType(), fieldName);
        var enumerable = field?.GetValue(target) as IEnumerable;
        if (enumerable == null) {
            return [];
        }

        return enumerable.Cast<object>().Where(x => x != null);
    }

    private static string? GetPrimaryKey(Dictionary<string, List<string>> keysBySource) {
        if (keysBySource.TryGetValue(CollectableItem.ReadSource.Inventory.ToString(), out var inventoryKeys) && inventoryKeys.Count > 0) {
            return inventoryKeys[0];
        }

        return keysBySource.Values.SelectMany(x => x).FirstOrDefault();
    }

    private static bool HasAnyKeys(Dictionary<string, List<string>> keysBySource) {
        return keysBySource.Values.Any(x => x.Count > 0);
    }

    private static FieldInfo? GetFieldInHierarchy(Type type, string fieldName) {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        Type? current = type;

        while (current != null) {
            var field = current.GetField(fieldName, flags);
            if (field != null) {
                return field;
            }

            current = current.BaseType;
        }

        return null;
    }

    private UnityEngine.Sprite? TryGetIcon(CollectableItem item, CollectableItem.ReadSource readSource) {
        try {
            return item.GetIcon(readSource);
        }
        catch (Exception ex) {
            logger.LogWarning(
                $"[CollectableExtraction] Failed GetIcon for item '{item.name}' (type='{item.GetType().Name}', readSource='{readSource}'): {ex.Message}");
            return null;
        }
    }

    private List<CollectableItem> GetAllCollectables() {
        var manager = ManagerSingleton<CollectableItemManager>.Instance;
        if (!manager) {
            Log("CollectableItemManager instance not found; exporting empty list.");
            return [];
        }

        var all = manager.GetAllCollectables();
        if (all == null) {
            return [];
        }

        if (all is IEnumerable<CollectableItem> typedEnumerable) {
            return typedEnumerable.Where(item => item != null).ToList();
        }

        if (all is IEnumerable rawEnumerable) {
            return rawEnumerable.Cast<object>()
                .OfType<CollectableItem>()
                .Where(item => item != null)
                .ToList();
        }

        return [];
    }


}

