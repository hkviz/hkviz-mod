using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace HKViz.Silk.Extraction;

public class MapExtraction(ExtractionFiles extractionFiles, LocalizationExtraction localizationExtraction, ManualLogSource logger) {
    private void Log(string message) {
        logger.LogInfo(message);
    }

    private void LogError(string message, Exception? ex = null) {
        logger.LogError(message);
        if (ex != null)
            logger.LogError(ex);
    }

    public MapData? Extract() {
        try {
            Log("Started Silksong map export.");

            // Ensure output directory exists

            var rooms = new List<MapRoomData>();
            GameObject? gameMapGo = GameObject.Find("Game_Map_Hornet(Clone)");

            if (!gameMapGo) {
                LogError("Could not find Game_Map_Hornet(Clone) in the scene!");
                return null;
            }
            WriteMapHierarchyLog(gameMapGo);

            Log($"Found Game_Map at position {gameMapGo.transform.position}");
            var gameMapPos = gameMapGo.transform.position;

            // Find all GameMapScene components
            var gameMapScenes = gameMapGo.GetComponentsInChildren<GameMapScene>(true);
            Log($"Found {gameMapScenes.Length} GameMapScene components");

            foreach (var mapScene in gameMapScenes) {
                try {
                    var roomData = ExportMapScene(mapScene, gameMapPos, gameMapGo.transform);
                    if (roomData != null) {
                        rooms.Add(roomData);
                    }
                }
                catch (Exception ex) {
                    LogError(
                        $"Error exporting GameMapScene '{mapScene.gameObject.name}': {ex.Message} : {ex.StackTrace}",
                        ex);
                }
            }

            Log($"Exported {rooms.Count} rooms");

            // Extract area names from the map
            var areaNames = ExtractAreaNames(gameMapGo, gameMapPos);
            Log($"Extracted {areaNames.Count} area name labels");

            var mapData = new MapData {
                Rooms = rooms,
                AreaNames = areaNames,
            };


            // Write to file
            extractionFiles.ExportJson("map-export.json", mapData);

            return mapData;
        }
        catch (Exception ex) {
            LogError($"Fatal error during map export: {ex.Message}", ex);
            return null;
        }
    }

    private void WriteMapHierarchyLog(GameObject mapRoot) {
        try {
            var builder = new StringBuilder();
            builder.AppendLine("HKViz Map Hierarchy Snapshot");
            builder.AppendLine($"Timestamp: {DateTime.UtcNow:O}");
            builder.AppendLine($"Root: {mapRoot.name}");
            builder.AppendLine();

            AppendTransformTree(builder, mapRoot.transform, 0);

            extractionFiles.ExportText("map-hierarchy-components.txt", builder.ToString());
        }
        catch (Exception ex) {
            LogError($"Failed to write map hierarchy snapshot", ex);
        }
    }

    private static void AppendTransformTree(StringBuilder builder, Transform node, int depth) {
        var go = node.gameObject;
        var indent = new string(' ', depth * 2);

        builder.Append(indent)
            .Append("- ")
            .Append(go.name)
            .Append(" | activeSelf=")
            .Append(go.activeSelf)
            .Append(", activeInHierarchy=")
            .Append(go.activeInHierarchy)
            .Append(", childCount=")
            .Append(node.childCount)
            .Append(" | components: ");

        var components = go.GetComponents<Component>();
        if (components.Length == 0) {
            builder.Append("<none>");
        }
        else {
            for (var i = 0; i < components.Length; i++) {
                if (i > 0)
                    builder.Append(", ");

                var component = components[i];
                builder.Append(component == null ? "<missing-script>" : component.GetType().Name);
            }
        }

        builder.AppendLine();

        for (var i = 0; i < node.childCount; i++) {
            AppendTransformTree(builder, node.GetChild(i), depth + 1);
        }
    }

    private static bool IsAreaNameObject(GameObject gameObject) {
        if (gameObject == null)
            return false;

        return gameObject.name.IndexOf("Area Name", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static string GetObjectPath(GameObject target, Transform relativeTo) {
        var builder = new StringBuilder();
        Transform? current = target.transform;

        while (current != null && current != relativeTo) {
            if (builder.Length > 0)
                builder.Insert(0, "/");

            builder.Insert(0, current.name);
            current = current.parent;
        }

        return builder.ToString();
    }

    private static string? ReadStringMember(object? instance, string memberName) {
        if (instance == null)
            return null;

        var type = instance.GetType();
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                   BindingFlags.IgnoreCase;

        var field = type.GetField(memberName, flags);
        if (field != null)
            return field.GetValue(instance) as string;

        var property = type.GetProperty(memberName, flags);
        if (property != null)
            return property.GetValue(instance) as string;

        return null;
    }

    private static ExportBounds BuildTextBounds(TMProOld.TMP_Text tmp, Vector3 gameMapPos) {
        try {
            tmp.ForceMeshUpdate();

            var localBounds = tmp.textBounds;
            if (localBounds.size.sqrMagnitude > 0f) {
                var worldMin = tmp.transform.TransformPoint(localBounds.min);
                var worldMax = tmp.transform.TransformPoint(localBounds.max);
                var worldBounds = new Bounds((worldMin + worldMax) * 0.5f, worldMax - worldMin);
                return ExportBounds.FromBounds(worldBounds, gameMapPos);
            }

            var renderer = tmp.GetComponent<Renderer>();
            if (renderer != null)
                return ExportBounds.FromBounds(renderer.bounds, gameMapPos);
        }
        catch {
            // Fall back below if TMP bounds are unavailable for inactive objects.
        }

        var fallback = new Bounds(tmp.transform.position, Vector3.zero);
        return ExportBounds.FromBounds(fallback, gameMapPos);
    }

    private TextData? CreateTextData(SetTextMeshProGameText setText, Transform mapRoot, Vector3 gameMapPos) {
        if (setText == null || setText.gameObject == null)
            return null;

        var go = setText.gameObject;
        var tmp = go.GetComponent<TMProOld.TextMeshPro>();
        if (tmp == null) {
            Log($"Skipping '{go.name}': no TMP text component found.");
            return null;
        }

        var localizedString = setText.text;
        
        var key = localizationExtraction.RequestExport(localizedString.Sheet, localizedString.Key);

        return new TextData {
            ObjectPath = GetObjectPath(go, mapRoot),
            TextKey = key,
            Position = Vector3Data.FromVector3(go.transform.position - gameMapPos),
            FontSize = Mathf.RoundToInt(tmp.fontSize),
            FontWeight = ReadFontWeightAsInt(tmp),
            Bounds = BuildTextBounds(tmp, gameMapPos),
            OrigColor = Vector4Data.FromColor(tmp.color),
        };
    }


    private static int ReadFontWeightAsInt(TMProOld.TMP_Text tmp) {
        try {
            // fontWeight may be present on concrete TMP types; try direct access first
            var prop = tmp.GetType().GetProperty("fontWeight",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null) {
                var val = prop.GetValue(tmp);
                if (val is int i)
                    return i;
                if (val != null)
                    return (int)val;
            }

            // Fallback: try to read fontStyle or FontWeight enum
            var field = tmp.GetType().GetField("fontWeight",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (field != null) {
                var val = field.GetValue(tmp);
                if (val is int fi)
                    return fi;
                if (val != null)
                    return (int)val;
            }
        }
        catch {
            // ignore
        }

        return 0;
    }

    private List<TextData> ExtractAreaNameTexts(Transform searchRoot, Transform mapRoot, Vector3 gameMapPos) {
        var result = new List<TextData>();
        var dedupe = new HashSet<string>(StringComparer.Ordinal);

        // First, try the typed lookup. This is the preferred path and should work when
        // the SetTextMeshProGameText type is loadable in the current AppDomain.
        try {
            var typed = searchRoot.GetComponentsInChildren<SetTextMeshProGameText>(true);
            Log($"Typed lookup: found {typed.Length} SetTextMeshProGameText components.");
            foreach (var setText in typed) {
                if (setText == null) {
                    Log($"Area Name lookup: skipping null component in '${setText?.gameObject.name}'.");
                    continue;
                }

                if (!IsAreaNameObject(setText.gameObject)) {
                    Log($"Area Name lookup: skipping '{setText.gameObject.name}' (not an area name object).");
                    continue;
                }

                var textData = CreateTextData(setText, mapRoot, gameMapPos);
                if (textData == null || string.IsNullOrEmpty(textData.ObjectPath)) {
                    Log($"Typed lookup: skipped component on '{setText.gameObject.name}' (no text data).");
                    continue;
                }

                var key = $"{textData.ObjectPath}|{textData.TextKey}";
                if (dedupe.Add(key))
                    result.Add(textData);
            }

            if (result.Count > 0)
                return result;
        }
        catch (Exception ex) {
            LogError("Typed lookup for SetTextMeshProGameText failed.", ex);
        }

        return result;
    }

    private List<TextData> ExtractAreaNames(GameObject mapRoot, Vector3 gameMapPos) {
        return ExtractAreaNameTexts(mapRoot.transform, mapRoot.transform, gameMapPos);
    }

    private static string DeriveMapZone(Transform roomTransform, Transform mapRoot, string sceneName) {
        // First, try to find parent zone by traversing hierarchy
        var current = roomTransform.parent;
        while (current != null && current != mapRoot) {
            if (current.parent == mapRoot) {
                var parentZoneName = current.name;
                return parentZoneName switch {
                    // Normalize special zone names
                    "Dust Maze" => "Dust Maze",
                    "Blasted_Steps" => "Blasted_Steps",
                    "Coral_Caves" => "Coral_Caves",
                    "Song_Gate" => "Song_Gate",
                    "Main Quest Pins" => "Main Quest Pins",
                    "Flea Tracker Markers" => "Flea Tracker Markers",
                    _ => parentZoneName
                };
            }

            current = current.parent;
        }

        // Parse zone from scene name patterns using first part
        if (string.IsNullOrEmpty(sceneName)) return "Unknown";
        var parts = sceneName.Split('_');
        if (parts.Length <= 0) return "Unknown";
        var firstPart = parts[0];
        var knownZones = new Dictionary<string, string> {
            { "Tut", "Tut" },
            { "Bonetown", "Bonetown" },
            { "Bone", "Bone" },
            { "Crawl", "Crawl" },
            { "Dock", "Dock" },
            { "Weave", "Weavehome" },
            { "Ant", "Ant" },
            { "Bone_East", "Wilds" },
            { "Greymoor", "Greymoor" },
            { "Dust", "Dust" },
            { "Shadow", "Swamp" },
            { "Wisp", "Wisp" },
            { "Aqueduct", "Aqueduct" },
            { "Belltown", "Belltown" },
            { "Bellshrine", "Belltown" },
            { "Shellwood", "Shellwood" },
            { "Coral", "Blasted_Steps" },
            { "Slab", "Slab" },
            { "Peak", "Peak" },
            { "Song", "Song" },
            { "Under", "Under" },
            { "Library", "Library" },
            { "Cog", "Cog" },
            { "Ward", "Ward" },
            { "Hang", "Hang" },
            { "Arborium", "Arborium" },
            { "Cradle", "Cradle" },
            { "Clover", "Clover" },
            { "Abyss", "Abyss" },
            { "Room", "Unknown" },
            { "Bellway", "Unknown" },
            { "Surface", "Surface" }
        };

        return knownZones.GetValueOrDefault(firstPart, "Unknown");
    }

    /// <summary>
    /// Exports a single GameMapScene component to MapRoomData.
    /// </summary>
    private MapRoomData? ExportMapScene(GameMapScene mapScene, Vector3 gameMapPos, Transform mapRoot) {
        if (mapScene == null || mapScene.gameObject == null)
            return null;

        var roomData = new MapRoomData {
            // Basic info
            GameObjectName = mapScene.gameObject.name,
            SceneName = mapScene.Name ?? mapScene.gameObject.name,
            HasSpriteRenderer = mapScene.hasSpriteRenderer
        };

        if (mapScene.mappedParent != null) {
            roomData.MappedParent = mapScene.mappedParent.gameObject.name;
        }

        roomData.MapZone = DeriveMapZone(mapScene.transform, mapRoot, roomData.SceneName);
        if (string.IsNullOrEmpty(roomData.MapZone) || roomData.MapZone == "Unknown") {
            roomData.MapZone = "Unknown";
        }
        
        // TODO export GameMapScene.BoundsSprite?

        // Get SpriteRenderer for bounds calculation
        var spriteRenderer = mapScene.GetComponent<SpriteRenderer>();


        roomData.InitialSprite = mapScene.initialSprite.ToSpriteInfoSafe(
            logger,
            $"MapScene:{roomData.SceneName ?? mapScene.gameObject.name}:InitialSprite");
        
        // Export sprite information
        roomData.FullSprite = mapScene.fullSprite.ToSpriteInfoSafe(
            logger,
            $"MapScene:{roomData.SceneName ?? mapScene.gameObject.name}:FullSprite"); //, mapScene.gameObject.name);

        // Export alternative sprites with conditions
        if (mapScene.altFullSprites != null && mapScene.altFullSprites.Length > 0) {
            var altSpritesWithConditions = new List<SpriteConditionData>();
            foreach (var altSprite in mapScene.altFullSprites) {
                if (altSprite.Sprite == null) continue;
                var spriteInfo = altSprite.Sprite.ToSpriteInfoSafe(
                    logger,
                    $"MapScene:{roomData.SceneName ?? mapScene.gameObject.name}:AltFullSprite");
                if (spriteInfo != null) {
                    var spriteCondition = new SpriteConditionData {
                        Sprite = spriteInfo, // , mapScene.gameObject.name + "_Alt"),
                        Condition = PlayerDataTestData.FromPlayerDataTest(altSprite.Condition)
                    };
                    altSpritesWithConditions.Add(spriteCondition);
                }
            }

            if (altSpritesWithConditions.Count > 0)
                roomData.AltFullSprites = altSpritesWithConditions.ToArray();
        }

        // Export alternative colors with conditions
        if (mapScene.altColors != null && mapScene.altColors.Length > 0) {
            var altColorsWithConditions = new List<ColorConditionData>();
            foreach (var altColor in mapScene.altColors) {
                if (altColor.Condition != null && PlayerDataTestData.FromPlayerDataTest(altColor.Condition) != null) {
                    var colorCondition = new ColorConditionData {
                        Color = Vector4Data.FromColor(altColor.Color),
                        Condition = PlayerDataTestData.FromPlayerDataTest(altColor.Condition)
                    };
                    altColorsWithConditions.Add(colorCondition);
                }
            }

            if (altColorsWithConditions.Count > 0)
                roomData.AltColors = altColorsWithConditions.ToArray();
        }

        // Export state information
        roomData.InitialState = mapScene.InitialState switch {
            GameMapScene.States.Hidden => SilkMapState.Hidden,
            GameMapScene.States.Rough => SilkMapState.Rough,
            GameMapScene.States.Full => SilkMapState.Full,
            _ => SilkMapState.Hidden
        };

        roomData.UnmappedNoBounds = mapScene.unmappedNoBounds;
        roomData.ExcludeBounds = mapScene.excludeBounds;

        // Export hide condition if present
        if (mapScene.hideCondition != null) {
            var hideConditionData = PlayerDataTestData.FromPlayerDataTest(mapScene.hideCondition);
            if (hideConditionData != null) {
                roomData.HideCondition = hideConditionData;
            }
        }

        // Export mapped parent references
        if (mapScene.mappedIfAllMapped != null && mapScene.mappedIfAllMapped.Length > 0) {
            roomData.MappedIfAllMapped = mapScene.mappedIfAllMapped
                .Where(x => x != null)
                .Select(x => x.gameObject.name)
                .ToArray();
        }

        // Export bounds + render order
        roomData.PositionZ = mapScene.transform.position.z;
        if (spriteRenderer != null) {
            roomData.SortingOrder = spriteRenderer.sortingOrder;
            roomData.VisualBounds = ExportBounds.FromBounds(spriteRenderer.bounds, gameMapPos);
            roomData.PlayerPositionBounds = ExportBounds.FromBounds(spriteRenderer.bounds, gameMapPos);
            roomData.OrigColor = Vector4Data.FromColor(spriteRenderer.color);
        }


        // Legacy compatibility fields
        roomData.Texts = ExtractAreaNameTexts(mapScene.transform, mapRoot, gameMapPos);

        return roomData;
    }
}

/// <summary>
/// Extension methods for reflection-based access to internal GameMapScene fields
/// </summary>
public static class GameMapSceneExtensions {
    private static readonly Dictionary<string, FieldInfo?> FieldCache = new Dictionary<string, FieldInfo?>();

    public static T? GetPrivateField<T>(this GameMapScene obj, string fieldName) {
        var key = $"{typeof(GameMapScene).Name}.{fieldName}";

        if (!FieldCache.TryGetValue(key, out var field)) {
            field = typeof(GameMapScene).GetField(
                fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase
            );
            FieldCache[key] = field;
        }

        if (field == null)
            return default;

        try {
            return (T?)field.GetValue(obj);
        }
        catch {
            return default;
        }
    }

    public static void SetPrivateField<T>(this GameMapScene obj, string fieldName, T? value) {
        var key = $"{typeof(GameMapScene).Name}.{fieldName}";

        if (!FieldCache.TryGetValue(key, out var field)) {
            field = typeof(GameMapScene).GetField(
                fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase
            );
            FieldCache[key] = field;
        }

        if (field != null) {
            try {
                field.SetValue(obj, value);
            }
            catch {
                // Silently fail
            }
        }
    }
}