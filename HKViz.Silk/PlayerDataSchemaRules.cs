using System;
using System.Collections.Generic;

namespace HKViz.Silk.PlayerDataSchema;

internal static class PlayerDataSchemaRules {
    private static readonly HashSet<string> IgnoredPlayerDataFieldNamesSet = new(StringComparer.Ordinal) {
        "mapBoolList",
        "currentBossSequence",
    };

    private static readonly HashSet<string> FrequentChangePlayerDataFieldNamesSet = new(StringComparer.Ordinal) {
        "FisherWalkerIdleTimeLeft",
        "FisherWalkerTimer",
    };

    private static readonly HashSet<string> IgnoredHeroStateFieldNamesSet = new(StringComparer.Ordinal) {
        "boolFieldAccessOptimizer",
        "fieldCache",
        "invulnerabilitySources",
    };

    private static readonly HashSet<string> FlattenedWrapperTypeNamesSet = new(StringComparer.Ordinal) {
        "HeroItemsState",
        "CollectionGramaphone.PlayingInfo",
    };

    private static readonly HashSet<string> NamedMapTypeNamesSet = new(StringComparer.Ordinal) {
        "CollectableItemsData",
        "CollectableRelicsData",
        "CollectableMementosData",
        "MateriumItemsData",
        "QuestCompletionData",
        "QuestRumourData",
        "ToolItemLiquidsData",
        "ToolItemsData",
        "ToolCrestsData",
        "FloatingCrestSlotsData",
        "EnemyJournalKillData",
    };

    public static bool IsIgnoredPlayerDataFieldName(string fieldName) =>
        IgnoredPlayerDataFieldNamesSet.Contains(fieldName);

    public static bool IsFrequentChangePlayerDataFieldName(string fieldName) =>
        FrequentChangePlayerDataFieldNamesSet.Contains(fieldName);

    public static bool IsIgnoredHeroStateFieldName(string fieldName) =>
        IgnoredHeroStateFieldNamesSet.Contains(fieldName);

    public static bool IsFlattenedWrapperTypeName(string fullTypeName) =>
        FlattenedWrapperTypeNamesSet.Contains(NormalizeTypeName(fullTypeName));

    public static bool IsNamedMapTypeName(string fullTypeName) =>
        NamedMapTypeNamesSet.Contains(NormalizeTypeName(fullTypeName));

    public static bool IsSteelQuestSpotArrayTypeName(string fullTypeName) =>
        NormalizeTypeName(fullTypeName) == "SteelSoulQuestSpot.Spot[]";

    public static bool IsWrappedVector2ListArrayTypeName(string fullTypeName) =>
        NormalizeTypeName(fullTypeName) == "WrappedVector2List[]";

    public static string NormalizeTypeName(string fullTypeName) {
        if (string.IsNullOrEmpty(fullTypeName)) {
            return string.Empty;
        }

        string normalized = fullTypeName.StartsWith("global::", StringComparison.Ordinal)
            ? fullTypeName.Substring("global::".Length)
            : fullTypeName;
        return normalized.Replace('/', '.');
    }
}


