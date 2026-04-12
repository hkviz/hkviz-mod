using System.IO;

namespace HKViz.Silk.Recording.DataHelpers;

public static class QuestCompletionDataHelper {
	public static bool Equals(QuestCompletionData.Completion left, QuestCompletionData.Completion right) =>
		left.HasBeenSeen == right.HasBeenSeen
		&& left.IsAccepted == right.IsAccepted
		&& left.CompletedCount == right.CompletedCount
		&& left.IsCompleted == right.IsCompleted
		&& left.WasEverCompleted == right.WasEverCompleted;

	public static QuestCompletionData.Completion Copy(QuestCompletionData.Completion value) => value;

	public static void Write(BinaryWriter writer, QuestCompletionData.Completion value) {
		writer.Write(value.HasBeenSeen);
		writer.Write(value.IsAccepted);
		writer.Write(value.CompletedCount);
		writer.Write(value.IsCompleted);
		writer.Write(value.WasEverCompleted);
	}
}

