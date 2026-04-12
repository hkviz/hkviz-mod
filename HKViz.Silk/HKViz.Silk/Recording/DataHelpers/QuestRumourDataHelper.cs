using System.IO;

namespace HKViz.Silk.Recording.DataHelpers;

public static class QuestRumourDataHelper {
    public static bool Equals(QuestRumourData.Data left, QuestRumourData.Data right) =>
        left.HasBeenSeen == right.HasBeenSeen && left.IsAccepted == right.IsAccepted;

    public static QuestRumourData.Data Copy(QuestRumourData.Data value) => value;

    public static void Write(BinaryWriter writer, QuestRumourData.Data value) {
        writer.Write(value.HasBeenSeen);
        writer.Write(value.IsAccepted);
    }
}



