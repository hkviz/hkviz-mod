using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Modding.Converters;
using UnityEngine;

namespace HKViz.Converters {
    /// <inheritdoc />
    public class Vector4Converter : Modding.Converters.JsonConverter<Vector4>
    {
        /// <inheritdoc />
        public override Vector4 ReadJson(Dictionary<string, object> token, object existingValue) {
            float x = Convert.ToSingle(token["x"]);
            float y = Convert.ToSingle(token["y"]);
            float z = Convert.ToSingle(token["z"]);
            float w = Convert.ToSingle(token["w"]);
            return new Vector4(x, y, z, w);
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Vector4 value) {
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WritePropertyName("w");
            writer.WriteValue(value.w);
        }
    }
}