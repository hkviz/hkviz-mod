using AnalyticsRecorder.Converters;
using Modding.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsRecorder {
    internal static class Json {

        public static JsonConverter[] jsonConverter = new JsonConverter[] {
            new Vector2Converter(),
            new Vector3Converter(),
            new Vector4Converter(),
        };

        public static string Stringify(Object obj) {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonConverter);
        }

        public static T? Parse<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
