using System.IO;
using System.Text;
using UnityEngine;

namespace HKViz.Silk.Recording;

public static class BinaryWriterExtensions {
    public static void WriteBool(this BinaryWriter writer, bool value) {
        writer.Write(value);
    }

    public static void WriteInt(this BinaryWriter writer, int value) {
        writer.Write(value);
    }

    public static void WriteCollectableItemsData(this BinaryWriter writer, global::CollectableItemsData.Data value) {
        writer.Write(value.Amount);
        writer.Write(value.IsSeenMask);
        writer.Write(value.AmountWhileHidden);
    }

    public static void WriteCollectableRelicsData(this BinaryWriter writer, global::CollectableRelicsData.Data value) {
        writer.Write(value.IsCollected);
        writer.Write(value.IsDeposited);
        writer.Write(value.HasSeenInRelicBoard);
    }

    public static void WriteCollectableMementosData(this BinaryWriter writer, global::CollectableMementosData.Data value) {
        writer.Write(value.IsDeposited);
        writer.Write(value.HasSeenInRelicBoard);
    }

    public static void WriteQuestRumourData(this BinaryWriter writer, global::QuestRumourData.Data value) {
        writer.Write(value.HasBeenSeen);
        writer.Write(value.IsAccepted);
    }

    public static void WriteQuestCompletionData(this BinaryWriter writer, global::QuestCompletionData.Completion value) {
        writer.Write(value.HasBeenSeen);
        writer.Write(value.IsAccepted);
        writer.Write(value.CompletedCount);
        writer.Write(value.IsCompleted);
        writer.Write(value.WasEverCompleted);
    }

    public static void WriteMateriumItemsData(this BinaryWriter writer, global::MateriumItemsData.Data value) {
        writer.Write(value.IsCollected);
        writer.Write(value.HasSeenInRelicBoard);
    }

    public static void WriteToolItemLiquidsData(this BinaryWriter writer, global::ToolItemLiquidsData.Data value) {
        writer.Write(value.RefillsLeft);
        writer.Write(value.SeenEmptyState);
        writer.Write(value.UsedExtra);
    }

    public static void WriteToolItemsData(this BinaryWriter writer, global::ToolItemsData.Data value) {
        writer.Write(value.IsUnlocked);
        writer.Write(value.IsHidden);
        writer.Write(value.HasBeenSeen);
        writer.Write(value.HasBeenSelected);
        writer.Write(value.AmountLeft);
    }

    public static void WriteToolCrestsSlotData(this BinaryWriter writer, global::ToolCrestsData.SlotData value) {
        writer.WriteStringCompat(value.EquippedTool);
        writer.Write(value.IsUnlocked);
    }

    public static void WriteToolCrestsData(this BinaryWriter writer, global::ToolCrestsData.Data value) {
        writer.Write(value.IsUnlocked);
        writer.Write(value.Slots?.Count ?? 0);
        if (value.Slots != null) {
            for (int i = 0; i < value.Slots.Count; i++) {
                writer.WriteToolCrestsSlotData(value.Slots[i]);
            }
        }
        writer.Write(value.DisplayNewIndicator);
    }

    public static void WriteEnemyJournalKillData(this BinaryWriter writer, global::EnemyJournalKillData.KillData value) {
        writer.Write(value.Kills);
        writer.Write(value.HasBeenSeen);
    }

    public static void WriteStoryEventInfoData(this BinaryWriter writer, global::PlayerStory.EventInfo value) {
        writer.Write((int)value.EventType);
        writer.WriteStringCompat(value.SceneName);
        writer.Write(value.PlayTime);
    }

    public static void WriteWrappedVector2List(this BinaryWriter writer, global::WrappedVector2List? value) {
        var items = value?.List;
        int count = items?.Count ?? 0;
        writer.Write(count);
        if (count == 0) {
            return;
        }

        for (int i = 0; i < count; i++) {
            writer.WriteVector2(items![i]);
        }
    }

    public static void WriteVector3(this BinaryWriter writer, Vector3 value) {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }

    public static void WriteVector2(this BinaryWriter writer, Vector2 value) {
        writer.Write(value.x);
        writer.Write(value.y);
    }

    public static void WriteStringCompat(this BinaryWriter writer, string? value) {
        var bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }

    public static void WriteStringArray(this BinaryWriter writer, string[] values) {
        writer.Write(values.Length);
        for (int i = 0; i < values.Length; i++) {
            writer.WriteStringCompat(values[i]);
        }
    }

    public static void WriteEntryType(this BinaryWriter writer, WriteEntryType value) {
        writer.Write((byte)value);
    }
}
