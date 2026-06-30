using System;
using System.Collections.Generic;
using System.Reflection;

namespace HKViz.Silk.Extraction;

public static class PlayerDataTestMapperExtensions {
    public static PlayerDataTestData? ToExportData(this PlayerDataTest? test) {
        if (test?.TestGroups == null || test.TestGroups.Length == 0) {
            return null;
        }

        PlayerDataTestData data = new() {
            TestGroups = [],
        };

        foreach (PlayerDataTest.TestGroup group in test.TestGroups) {
            PlayerDataTestGroupData groupData = new() {
                Tests = [],
            };

            if (group.Tests != null) {
                foreach (PlayerDataTest.Test testEntry in group.Tests) {
                    PlayerDataTestEntryData entryData = new() {
                        Type = (SilkPlayerDataTestType)testEntry.Type,
                        FieldName = testEntry.FieldName,
                    };

                    switch (testEntry.Type) {
                        case PlayerDataTest.TestType.Bool:
                            entryData.BoolValue = testEntry.BoolValue;
                            break;
                        case PlayerDataTest.TestType.Int:
                            entryData.NumType = (SilkNumTestType?)testEntry.NumType;
                            entryData.IntValue = testEntry.IntValue;
                            break;
                        case PlayerDataTest.TestType.Float:
                            entryData.NumType = (SilkNumTestType?)testEntry.NumType;
                            entryData.FloatValue = testEntry.FloatValue;
                            break;
                        case PlayerDataTest.TestType.String:
                            entryData.StringType = (SilkStringTestType?)testEntry.StringType;
                            entryData.StringValue = testEntry.StringValue;
                            break;
                        case PlayerDataTest.TestType.Enum:
                            entryData.IntValue = testEntry.IntValue;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"Unsupported PlayerDataTest type: {testEntry.Type}");
                    }

                    groupData.Tests.Add(entryData);
                }
            }

            if (groupData.Tests.Count > 0) {
                data.TestGroups.Add(groupData);
            }
        }

        return data.TestGroups?.Count > 0 ? data : null;
    }
}

public static class ToolItemMapperExtensions {
    private const BindingFlags InstanceBindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase;
    private static readonly Dictionary<string, FieldInfo?> FieldCache = new(StringComparer.Ordinal);

    public static PlayerDataTest? GetAlternateUnlockedTest(this ToolItem tool) {
        if (tool == null) {
            return null;
        }

        return tool.GetPrivateField<PlayerDataTest>("alternateUnlockedTest");
    }

    private static T? GetPrivateField<T>(this ToolItem obj, string fieldName) {
        string key = $"{typeof(ToolItem).Name}.{fieldName}";

        if (!FieldCache.TryGetValue(key, out FieldInfo? field)) {
            field = typeof(ToolItem).GetField(fieldName, InstanceBindings);
            FieldCache[key] = field;
        }

        if (field == null) {
            return default;
        }

        try {
            return (T?)field.GetValue(obj);
        }
        catch {
            return default;
        }
    }
}

