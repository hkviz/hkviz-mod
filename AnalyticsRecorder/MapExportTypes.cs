using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Sprites;
using UnityEngine;

namespace AnalyticsRecorder {

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
    }

    [System.Serializable]
    public class ExportBounds {
        public Vector3 min;
        public Vector3 max;

        public static ExportBounds fromBounds(UnityEngine.Bounds bounds, UnityEngine.Vector3 substract) {
            return new ExportBounds {
                min = bounds.min - substract,
                max = bounds.max - substract,
            };
        }
    }

    [System.Serializable]
    public class MapData {
        public List<MapRoomData> rooms;
    }
}
