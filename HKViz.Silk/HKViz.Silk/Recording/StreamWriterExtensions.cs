using System.IO;
using System.Text;
using UnityEngine;

namespace HKViz.Silk.Recording;

public static class BinaryWriterExtensions {
    public static void WriteVector3(this BinaryWriter writer, Vector3 value) {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }

    public static void WriteVector2(this BinaryWriter writer, Vector2 value) {
        writer.Write(value.x);
        writer.Write(value.y);
    }

    public static void WriteStringCompat(this BinaryWriter writer, string value) {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }

    public static void WriteStringArray(this BinaryWriter writer, string[] values) {
        writer.Write(values.Length);
        for (int i = 0; i < values.Length; i++) {
            writer.WriteStringCompat(values[i] ?? string.Empty);
        }
    }

    public static void WriteEntryType(this BinaryWriter writer, WriteEntryType value) {
        writer.Write((byte)value);
    }
}
