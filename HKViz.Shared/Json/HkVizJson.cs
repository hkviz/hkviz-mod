using Newtonsoft.Json;

namespace HKViz.Shared.Json;


internal static class HkVizJson {
    private static readonly JsonConverter[] JsonConverter = [
        new Vector2Converter(),
        new Vector3Converter(),
        new Vector4Converter(),
    ];

    public static string Stringify(object obj) {
        return JsonConvert.SerializeObject(obj, Formatting.Indented, JsonConverter);
    }

    public static T? Parse<T>(string json) {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
