using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace HKViz {
    internal class RecordingSerializer {
        public static CultureInfo cultureInfo = new CultureInfo("en-US");

        private static RecordingSerializer? _instance;
        public static RecordingSerializer Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new RecordingSerializer();
                return _instance;
            }
        }

        public string serializePosition2D(Vector3 value) => serialize(new Vector2(value.x * 10, value.y * 10), "0");

        public string serialize(Vector2 value, string? format = null) => $"{serialize(value.x, format)},{serialize(value.y, format)}";
        public string serialize(Vector3 value, string? format = null)
            => $"{serialize(value.x, format)},{serialize(value.y, format)},{serialize(value.z, format)}";
        public string serialize(float value, string? format = null) => format is null ? value.ToString(cultureInfo) : value.ToString(format, cultureInfo);
        public string serialize(bool value) => value ? "1" : "0";
        public string serialize(int value) => value.ToString(cultureInfo);

        public string serialize(string value) => value;
        public string serialize(List<string> value) => string.Join(",", value);
        public string serialize(List<int> value) => string.Join(",", value);
        public string serialize(List<Vector3> value, string? format = null) => string.Join(",", value.Select(it => serialize(it, format)));

        public string serialize(BossSequenceDoor.Completion value) {
            return (
                serialize(value.canUnlock) +
                serialize(value.unlocked) +
                serialize(value.completed) +
                serialize(value.allBindings) +
                serialize(value.noHits) +
                serialize(value.boundNail) +
                serialize(value.boundShell) +
                serialize(value.boundCharms) +
                serialize(value.boundSoul) +
                ";" +
                serialize(value.viewedBossSceneCompletions)
            );
        }

        public string serialize(BossStatue.Completion value) {
            return (
                serialize(value.hasBeenSeen) +
                serialize(value.isUnlocked) +
                serialize(value.completedTier1) +
                serialize(value.completedTier2) +
                serialize(value.completedTier3) +
                serialize(value.seenTier3Unlock) +
                serialize(value.usingAltVersion)
            );
        }

        public string serialize(BossSequenceController.BossSequenceData value) {
            return (
                serialize((int)value.bindings) +
                ";" +
                serialize(value.bossSequenceName)
            // intentionally not everything included, as might change to much
            );
        }

        public string serializeUntyped(object? value) => value switch {
            null => "null",
            Vector3 v => serialize(v),
            Vector2 v => serialize(v),
            float v => serialize(v),
            bool v => serialize(v),
            int v => serialize(v),
            string v => serialize(v),
            List<string> v => serialize(v),
            List<int> v => serialize(v),
            List<Vector3> v => serialize(v),
            BossSequenceDoor.Completion v => serialize(v),
            BossStatue.Completion v => serialize(v),
            BossSequenceController.BossSequenceData v => serialize(v),
            _ => value.ToString(),
        };

    }
}
