using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

namespace HKViz.Silk.Extraction;

public enum SilkMapState {
    Hidden = 0,
    Rough = 1,
    Full = 2,
}

public enum SilkPlayerDataTestType {
    Bool = 0,
    Int = 1,
    Float = 2,
    Enum = 3,
    String = 4,
}

public enum SilkNumTestType {
    Equal = 0,
    NotEqual = 1,
    LessThan = 2,
    MoreThan = 3,
}

public enum SilkStringTestType {
    Equal = 0,
    NotEqual = 1,
    Contains = 2,
    NotContains = 3,
}

[Serializable]
public class Vector2Data {
    public float X { get; set; }

    public float Y { get; set; }

    public Vector2Data() {
    }

    public Vector2Data(float x, float y) {
        X = x;
        Y = y;
    }

    public static Vector2Data FromVector2(Vector2 v) => new(v.x, v.y);
}

[Serializable]
public class Vector3Data {
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Vector3Data() {
    }

    public Vector3Data(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }

    public static Vector3Data FromVector3(Vector3 v) => new(v.x, v.y, v.z);
}

[Serializable]
public class Vector4Data {
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }

    public Vector4Data() {
    }

    public Vector4Data(float x, float y, float z, float w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static Vector4Data FromVector4(Vector4 v) => new(v.x, v.y, v.z, v.w);

    public static Vector4Data FromColor(Color c) => new(c.r, c.g, c.b, c.a);
}

[Serializable]
public class SpriteInfo {
    public string? Name { get; set; }
    public string? NameShort { get; set; }
    public Vector2Data? Size { get; set; }
    public Vector4Data? Padding { get; set; }

    public static SpriteInfo FromSprite(Sprite sprite) {
        // name alone is not unique, so some data is added to make it hopefully unique
        // this could break with future SilkSong updates. 
        // We can only use data available in the Unity runtime + in the python extraction scripts
        int verticesCount = sprite.vertices.Length;
        int width = Mathf.RoundToInt(sprite.rect.width);
        int height = Mathf.RoundToInt(sprite.rect.height);
        int physicsShapeCount = sprite.GetPhysicsShapeCount();
        var uniqueName = $"{sprite.name}_{verticesCount}_{width}x{height}_{physicsShapeCount}";

        return new SpriteInfo {
            Name = uniqueName,
            NameShort = sprite.name,
            Size = new Vector2Data(sprite.rect.width, sprite.rect.height),
            Padding = Vector4Data.FromVector4(
                DataUtility.GetPadding(sprite)), // Vector4Data.FromVector4(sprite.border),
        };
    }
}

[Serializable]
public class ExportBounds {
    public Vector3Data? Min { get; set; }
    public Vector3Data? Max { get; set; }

    public static ExportBounds FromBounds(Bounds bounds, Vector3 offset) {
        return new ExportBounds {
            Min = Vector3Data.FromVector3(bounds.min - offset),
            Max = Vector3Data.FromVector3(bounds.max - offset),
        };
    }
}

[Serializable]
public class PlayerDataTestEntryData {
    public SilkPlayerDataTestType Type { get; set; }
    public string? FieldName { get; set; }
    public bool? BoolValue { get; set; }
    public SilkNumTestType? NumType { get; set; }
    public int? IntValue { get; set; }
    public float? FloatValue { get; set; }
    public string? StringValue { get; set; }
    public SilkStringTestType? StringType { get; set; }
}

[Serializable]
public class PlayerDataTestGroupData {
    public List<PlayerDataTestEntryData>? Tests { get; set; }
}

[Serializable]
public class PlayerDataTestData {
    public string? PlayerDataOverrideType { get; set; }
    public List<PlayerDataTestGroupData>? TestGroups { get; set; }

    public static PlayerDataTestData? FromPlayerDataTest(PlayerDataTest? test) {
        if (test?.TestGroups == null || test.TestGroups.Length == 0)
            return null;
        
        
        var data = new PlayerDataTestData {
            TestGroups = [],
        };

        foreach (var group in test.TestGroups) {
            var groupData = new PlayerDataTestGroupData {
                Tests = [],
            };

            if (group.Tests != null) {
                foreach (var testEntry in group.Tests) {
                    var entryData = new PlayerDataTestEntryData {
                        Type = (SilkPlayerDataTestType)testEntry.Type,
                        FieldName = testEntry.FieldName,
                    };

                    // Only populate relevant fields based on type
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

            if (groupData.Tests.Count > 0)
                data.TestGroups.Add(groupData);
        }

        return data.TestGroups?.Count > 0 ? data : null;
    }
}

[Serializable]
public class SpriteConditionData {
    public SpriteInfo? Sprite { get; set; }
    public PlayerDataTestData? Condition { get; set; }
}

[Serializable]
public class ColorConditionData {
    public Vector4Data? Color { get; set; }
    public PlayerDataTestData? Condition { get; set; }
}

[Serializable]
public class TextData {
    public string? ObjectPath { get; set; }
    public string? ConvoName { get; set; }
    public string? SheetName { get; set; }
    public Vector3Data? Position { get; set; }
    public int FontSize { get; set; }
    public int FontWeight { get; set; }
    public ExportBounds? Bounds { get; set; }
    public Vector4Data? OrigColor { get; set; }
}

[Serializable]
public class MapRoomData {
    public string? SceneName { get; set; }
    public string? GameObjectName { get; set; }
    public string? MapZone { get; set; }
    public bool HasSpriteRenderer { get; set; }
    public Vector4Data? OrigColor { get; set; }
    public ExportBounds? VisualBounds { get; set; }
    public ExportBounds? PlayerPositionBounds { get; set; }
    public int SortingOrder { get; set; }
    public float PositionZ { get; set; }
    public String? MappedParent { get; set; }

    // Sprite data - includes all sprite information
    public SpriteInfo? InitialSprite { get; set; }
    public SpriteInfo? FullSprite { get; set; }
    // public SpriteInfo? RendererSprite { get; set; }
    public SpriteConditionData[]? AltFullSprites { get; set; }

    public ColorConditionData[]? AltColors { get; set; }
    public SilkMapState InitialState { get; set; }
    public bool UnmappedNoBounds { get; set; }
    public bool ExcludeBounds { get; set; }
    public PlayerDataTestData? HideCondition { get; set; }
    public string[]? MappedIfAllMapped { get; set; }
    public List<TextData>? Texts { get; set; }
}

[Serializable]
public class MapData {
    public List<MapRoomData>? Rooms { get; set; }

    public List<TextData>? AreaNames { get; set; }
}
