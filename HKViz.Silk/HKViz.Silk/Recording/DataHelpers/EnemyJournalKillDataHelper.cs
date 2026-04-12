using System.IO;

namespace HKViz.Silk.Recording.DataHelpers;

public static class EnemyJournalKillDataHelper {
    public static bool Equals(EnemyJournalKillData.KillData left, EnemyJournalKillData.KillData right) =>
        left.Kills == right.Kills && left.HasBeenSeen == right.HasBeenSeen;

    public static EnemyJournalKillData.KillData Copy(EnemyJournalKillData.KillData value) => value;

    public static void Write(BinaryWriter writer, EnemyJournalKillData.KillData value) {
        writer.Write(value.Kills);
        writer.Write(value.HasBeenSeen);
    }
}



