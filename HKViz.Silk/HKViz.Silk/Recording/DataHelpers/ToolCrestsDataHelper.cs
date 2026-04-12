using System.Collections.Generic;
using System.IO;

namespace HKViz.Silk.Recording.DataHelpers;

public static class ToolCrestsDataHelper {
    public static bool EqualsSlotData(ToolCrestsData.SlotData left, ToolCrestsData.SlotData right) =>
        string.Equals(left.EquippedTool, right.EquippedTool, System.StringComparison.Ordinal) && left.IsUnlocked == right.IsUnlocked;

    public static ToolCrestsData.SlotData CopySlotData(ToolCrestsData.SlotData value) => value;

    public static void WriteSlotData(BinaryWriter writer, ToolCrestsData.SlotData value) {
        writer.WriteStringCompat(value.EquippedTool);
        writer.Write(value.IsUnlocked);
    }

    public static bool Equals(ToolCrestsData.Data left, ToolCrestsData.Data right) {
        if (left.IsUnlocked != right.IsUnlocked || left.DisplayNewIndicator != right.DisplayNewIndicator) {
            return false;
        }

        List<ToolCrestsData.SlotData>? leftSlots = left.Slots;
        List<ToolCrestsData.SlotData>? rightSlots = right.Slots;
        if (leftSlots == null || rightSlots == null) {
            return leftSlots == null && rightSlots == null;
        }

        if (leftSlots.Count != rightSlots.Count) {
            return false;
        }

        for (int i = 0; i < leftSlots.Count; i++) {
            if (!EqualsSlotData(leftSlots[i], rightSlots[i])) {
                return false;
            }
        }

        return true;
    }

    public static ToolCrestsData.Data Copy(ToolCrestsData.Data value) {
        ToolCrestsData.Data copy = value;
        if (value.Slots != null) {
            copy.Slots = new List<ToolCrestsData.SlotData>(value.Slots);
        }

        return copy;
    }

    public static void Write(BinaryWriter writer, ToolCrestsData.Data value) {
        writer.Write(value.IsUnlocked);
        writer.Write(value.Slots?.Count ?? 0);
        if (value.Slots != null) {
            for (int i = 0; i < value.Slots.Count; i++) {
                WriteSlotData(writer, value.Slots[i]);
            }
        }
        writer.Write(value.DisplayNewIndicator);
    }
}



