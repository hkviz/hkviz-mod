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

        public static string ToString(Object obj) {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonConverter);
        }
    }
}
