using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

namespace HKViz {

    [System.Serializable]
    public class SpriteInfo {
        public string name;
        public Vector2 size;
        public Vector4 padding;

        public static SpriteInfo fromSprite(Sprite? sprite, string fallbackName) {
            var spriteName = sprite.name;
            if (spriteName == null || spriteName == "") spriteName = fallbackName;

            var size = sprite == null ? Vector2.zero : new Vector2(sprite.rect.width, sprite.rect.height);
            var padding = sprite == null ? Vector4.zero : DataUtility.GetPadding(sprite);

            return new SpriteInfo() {
                name = spriteName,
                size = size,
                padding = padding,
            };
        }
    }

    [System.Serializable]
    public class MapRoomData {
        public string sceneName;
        public SpriteInfo spriteInfo;
        public SpriteInfo? roughSpriteInfo;
        public string gameObjectName;
        public string mapZone;
        public Vector4 origColor;
        public ExportBounds visualBounds;
        public ExportBounds playerPositionBounds;
        public string sprite;
        public string? spriteRough;
        public bool hasRoughVersion;
        public List<TextData> texts;
    }

    [System.Serializable]
    public record TextData(
        string objectPath,
        string convoName,
        string sheetName,
        Vector3 position,
        float fontSize,
        float fontWeight,
        ExportBounds bounds,
        Vector4 origColor
    );

    [System.Serializable]
    public record ExportBounds(
        Vector3 min,
        Vector3 max
    ) {
        public static ExportBounds fromBounds(UnityEngine.Bounds bounds, UnityEngine.Vector3 substract) {
            return new ExportBounds(
                min: bounds.min - substract,
                max: bounds.max - substract
            );
        }
    }

    [System.Serializable]
    public record MapData(
        List<MapRoomData> rooms,
        List<TextData> areaNames
    );
}
