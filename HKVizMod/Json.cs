using HKViz.Converters;
using Modding.Converters;
using Newtonsoft.Json;
using System;

namespace HKViz {
    internal static class Json {
        private static readonly JsonConverter[] JsonConverter = [
            new Vector2Converter(),
            new Vector3Converter(),
            new Vector4Converter()
        ];

        public static string Stringify(Object obj) {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, JsonConverter);
        }

        public static T? Parse<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
