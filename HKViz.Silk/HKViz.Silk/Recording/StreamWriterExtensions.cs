using System.Collections.Generic;
using System.IO;
using System.Text;
using HKViz.Silk.GameData;
using UnityEngine;

namespace HKViz.Silk.Recording;

public static class BinaryWriterExtensions {
    public static void WriteBool(this BinaryWriter writer, bool value) {
        writer.Write(value);
    }

    public static void WriteInt(this BinaryWriter writer, int value) {
        writer.Write(value);
    }

    public static void WriteCollectableItemsData(this BinaryWriter writer, CollectableItemsData.Data value) {
        writer.Write(value.Amount);
        writer.Write(value.IsSeenMask);
        writer.Write(value.AmountWhileHidden);
    }

    public static void WriteCollectableRelicsData(this BinaryWriter writer, CollectableRelicsData.Data value) {
        writer.Write(value.IsCollected);
        writer.Write(value.IsDeposited);
        writer.Write(value.HasSeenInRelicBoard);
    }

    public static void WriteCollectableMementosData(this BinaryWriter writer, CollectableMementosData.Data value) {
        writer.Write(value.IsDeposited);
        writer.Write(value.HasSeenInRelicBoard);
    }

    public static void WriteQuestRumourData(this BinaryWriter writer, QuestRumourData.Data value) {
        writer.Write(value.HasBeenSeen);
        writer.Write(value.IsAccepted);
    }

    public static void WriteQuestCompletionData(this BinaryWriter writer, QuestCompletionData.Completion value) {
        writer.Write(value.HasBeenSeen);
        writer.Write(value.IsAccepted);
        writer.Write(value.CompletedCount);
        writer.Write(value.IsCompleted);
        writer.Write(value.WasEverCompleted);
    }

    public static void WriteMateriumItemsData(this BinaryWriter writer, MateriumItemsData.Data value) {
        writer.Write(value.IsCollected);
        writer.Write(value.HasSeenInRelicBoard);
    }

    public static void WriteToolItemLiquidsData(this BinaryWriter writer, ToolItemLiquidsData.Data value) {
        writer.Write(value.RefillsLeft);
        writer.Write(value.SeenEmptyState);
        writer.Write(value.UsedExtra);
    }

    public static void WriteToolItemsData(this BinaryWriter writer, ToolItemsData.Data value) {
        writer.Write(value.IsUnlocked);
        writer.Write(value.IsHidden);
        writer.Write(value.HasBeenSeen);
        writer.Write(value.HasBeenSelected);
        writer.Write(value.AmountLeft);
    }

    public static void WriteToolCrestsSlotData(this BinaryWriter writer, ToolCrestsData.SlotData value) {
        // TODO provide ids
        writer.WriteIdOrStringCompat(new(), value.EquippedTool);
        writer.Write(value.IsUnlocked);
    }

    public static void WriteToolCrestsData(this BinaryWriter writer, ToolCrestsData.Data value) {
        writer.Write(value.IsUnlocked);
        writer.Write(value.Slots?.Count ?? 0);
        if (value.Slots != null) {
            for (int i = 0; i < value.Slots.Count; i++) {
                writer.WriteToolCrestsSlotData(value.Slots[i]);
            }
        }
        writer.Write(value.DisplayNewIndicator);
    }

    public static void WriteEnemyJournalKillData(this BinaryWriter writer, EnemyJournalKillData.KillData value) {
        writer.Write(value.Kills);
        writer.Write(value.HasBeenSeen);
    }

    public static void WriteStoryEventInfoData(this BinaryWriter writer, PlayerStory.EventInfo value) {
        writer.Write((int)value.EventType);
        writer.WriteIdOrStringCompat(SilkSongScenes.SCENES, value.SceneName);
        writer.Write(value.PlayTime);
    }

    public static void WriteWrappedVector2List(this BinaryWriter writer, WrappedVector2List? value) {
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

    /// <summary>
    /// Write a string, where the value is usually known to be a certain value of a set,
    /// but could also not be.
    /// 
    /// If the value is known / is part of valueToId, only the id is written, unless the id is zero.
    /// Make sure the id can never be zero, or get less storage efficiency.
    /// (ushort)id
    ///
    /// If the value is not known, the written data is
    /// (ushort)0
    /// (int)length of value
    /// (value as utf8 bytes)
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="valueToId"></param>
    /// <param name="value"></param>
    public static void WriteIdOrStringCompat(this BinaryWriter writer, Dictionary<string, ushort> valueToId, string? value) {
        if (value == null) {
            // (case 0) null
            writer.Write(ushort.MaxValue);
            return;
        }
        if (value.Length == 0) {
            // (case 1) empty string
            writer.Write((ushort)(ushort.MaxValue - 1));
            return;
        }
        bool hasId = valueToId.TryGetValue(value, out ushort id);
        if (hasId && id != 0) {
            // (case 2) has id
            // check for zero for safety. Should never be zero if the valueToId maps are created 
            // correctly. If they are not, it will write the string for id zero, bc otherwise file would be corrupted.
            writer.Write(id);
        } else {
            // (case 3) no id
            writer.Write((ushort)0);
            writer.WriteStringCompat(value);
        }
    }

    public static void WriteIdOrStringArray(
        this BinaryWriter writer,
        Dictionary<string, ushort> valueToId,
        string[] values
    ) {
        writer.Write(values.Length);
        for (int i = 0; i < values.Length; i++) {
            writer.WriteIdOrStringCompat(valueToId, values[i]);
        }
    }

    public static void WriteEntryType(this BinaryWriter writer, WriteEntryType value) {
        writer.Write((byte)value);
    }
}
