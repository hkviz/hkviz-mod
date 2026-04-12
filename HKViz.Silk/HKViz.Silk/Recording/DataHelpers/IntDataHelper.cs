using System.IO;

namespace HKViz.Silk.Recording.DataHelpers;

public static class IntDataHelper {
    public static bool Equals(int left, int right) => left == right;

    public static int Copy(int value) => value;

    public static void Write(BinaryWriter writer, int value) {
        writer.Write(value);
    }
}

