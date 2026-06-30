using System;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.Sprites;

namespace HKViz.Silk.Extraction;

public static class SpriteInfoExtensions {
    public static SpriteInfo ToSpriteInfo(this Sprite sprite) {
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
            Padding = Vector4Data.FromVector4(DataUtility.GetPadding(sprite)), // =sprite.border?
        };
    }

    public static SpriteInfo? ToSpriteInfoSafe(this Sprite? sprite, ManualLogSource logger, string context) {
        if (sprite == null) {
            return null;
        }

        try {
            return sprite.ToSpriteInfo();
        }
        catch (Exception ex) {
            logger.LogWarning($"[SpriteInfo] Failed to export sprite in '{context}' (sprite='{sprite.name}'): {ex.Message}");
            return null;
        }
    }
}

