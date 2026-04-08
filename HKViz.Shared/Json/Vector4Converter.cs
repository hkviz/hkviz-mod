using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System;

namespace HKViz.Shared.Json;

internal class Vector4Converter : JsonConverter<Vector4>
{
    public override Vector4 ReadJson(JsonReader reader, Type objectType, Vector4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // Parse the JSON object
        var obj = JObject.Load(reader);

        float x = obj["x"]?.Value<float>() ?? 0f;
        float y = obj["y"]?.Value<float>() ?? 0f;
        float z = obj["z"]?.Value<float>() ?? 0f;
        float w = obj["w"]?.Value<float>() ?? 0f;

        return new Vector4(x, y, z, w);
    }

    public override void WriteJson(JsonWriter writer, Vector4 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("x");
        writer.WriteValue(value.x);

        writer.WritePropertyName("y");
        writer.WriteValue(value.y);

        writer.WritePropertyName("z");
        writer.WriteValue(value.z);

        writer.WritePropertyName("w");
        writer.WriteValue(value.w);

        writer.WriteEndObject();
    }
}
