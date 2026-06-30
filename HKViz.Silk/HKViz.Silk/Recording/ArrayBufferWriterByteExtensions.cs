using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HKViz.Silk.Recording;

public static class ArrayBufferWriterByteExtensions {

    public static void WriteEntryType(this ArrayBufferWriter<byte> self, WriteEntryType value) {
        var span = self.GetSpan(1);
        span[0] = (byte)value;
        self.Advance(1);
    }
    
    // Numbers
    public static void WriteByte(this ArrayBufferWriter<byte> self, byte value) {
        var span = self.GetSpan(1);
        span[0] = value;
        self.Advance(1);
    }

    public static void WriteUShort(this ArrayBufferWriter<byte> self, ushort value) {
        var span = self.GetSpan(2);
        BinaryPrimitives.WriteUInt16LittleEndian(span, value);
        self.Advance(2);
    }

    public static void WriteUShortAt(this ArrayBufferWriter<byte> self, ushort value, int offset) {
        var buffer = self.GetMemory().Span;
        BinaryPrimitives.WriteUInt16LittleEndian(
            buffer.Slice(offset, 2),
            value
        );
    }

    public static void WriteInt(this ArrayBufferWriter<byte> self, int value) {
        var span = self.GetSpan(4);
        BinaryPrimitives.WriteInt32LittleEndian(span, value);
        self.Advance(4);
    }

    public static void WriteFloat(this ArrayBufferWriter<byte> self, float value) {
        var span = self.GetSpan(4);
        BitConverter.TryWriteBytes(span, value);
        self.Advance(4);
    }
    
    // Strings
    public static void WriteString(this ArrayBufferWriter<byte> self, string value) {
        // write length
        int byteCount = Encoding.UTF8.GetByteCount(value);
        var span = self.GetSpan(4);
        BinaryPrimitives.WriteInt32LittleEndian(span, byteCount);
        self.Advance(4);
        
        // write bytes
        var dest = self.GetSpan(byteCount);
        Encoding.UTF8.GetBytes(value, dest);
        self.Advance(byteCount);
    }
    
    public static void WriteIdOrString(this ArrayBufferWriter<byte> self, Dictionary<string, ushort> valueToId, string? value) {
        if (value == null) {
            // (case 0) null
            self.WriteUShort(ushort.MaxValue);
            return;
        }
        if (value.Length == 0) {
            // (case 1) empty string
            self.WriteUShort(ushort.MaxValue - 1);
            return;
        }
        bool hasId = valueToId.TryGetValue(value, out ushort id);
        if (hasId && id != 0) {
            // (case 2) has id
            // check for zero for safety. Should never be zero if the valueToId maps are created 
            // correctly. If they are not, it will write the string for id zero, bc otherwise file would be corrupted.
            self.WriteUShort(id);
        } else {
            // (case 3) no id
            self.WriteUShort(0);
            self.WriteString(value);
        }
    }

    // Vectors
    public static void WriteVector2(this ArrayBufferWriter<byte> writer, Vector2 value) {
        writer.WriteFloat(value.x);
        writer.WriteFloat(value.y);
    }

    public static void WriteVector3(this ArrayBufferWriter<byte> writer, Vector3 value) {
        writer.WriteFloat(value.x);
        writer.WriteFloat(value.y);
        writer.WriteFloat(value.z);
    }

    public static void WriteVector4(this ArrayBufferWriter<byte> writer, Vector4 value) {
        writer.WriteFloat(value.x);
        writer.WriteFloat(value.y);
        writer.WriteFloat(value.z);
        writer.WriteFloat(value.w);
    }
}
