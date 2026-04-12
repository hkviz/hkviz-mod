using System;
using UnityEngine;

namespace HKViz.Silk.Recording;

public partial class PlayerDataWriter(RunFiles runFiles) {

    public partial void WriteAll();
    public partial void WriteChanged();

    public void WriteBoolIfChanged(
        ushort fieldId,
        bool oldValue,
        bool newValue
    ) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataBoolChange(
                fieldId: fieldId,
                value: newValue
            );
        }
    }

    public void WriteIntIfChanged(
        ushort fieldId,
        int oldValue,
        int newValue
    ) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataIntChange(
                fieldId: fieldId,
                value: newValue
            );
        }
    }

    public void WriteEnumIfChanged(
        ushort fieldId,
        ushort oldValue,
        ushort newValue
    ) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataEnumChange(
                fieldId: fieldId,
                value: newValue
            );
        }
    }

    public void WriteULongIfChanged(
        ushort fieldId,
        ulong oldValue,
        ulong newValue
    ) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataULongChange(
                fieldId: fieldId,
                value: newValue
            );
        }
    }

    public void WriteVector3IfChanged(
        ushort fieldId,
        Vector3 oldValue,
        Vector3 newValue
    ) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataVector3Change(
                fieldId: fieldId,
                value: newValue
            );
        }
    }

    public void WriteVector2IfChanged(
        ushort fieldId,
        Vector2 oldValue,
        Vector2 newValue
    ) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataVector2Change(
                fieldId: fieldId,
                value: newValue
            );
        }
    }

    public void WriteIntArrayIfChanged(
        ushort fieldId,
        ref int[]? oldValue,
        int[]? newValue
    ) {
        int[] oldValues = oldValue ?? Array.Empty<int>();
        int[] newValues = newValue ?? Array.Empty<int>();

        if (oldValues.Length != newValues.Length) {
            runFiles.WritePlayerDataIntArrayFullChange(fieldId, newValues);
            UpdateIntArraySnapshot(ref oldValue, newValue);
            return;
        }

        int[] changedIndices = new int[newValues.Length];
        int[] changedValues = new int[newValues.Length];
        int changedCount = 0;

        for (int i = 0; i < newValues.Length; i++) {
            if (oldValues[i] == newValues[i]) {
                continue;
            }

            changedIndices[changedCount] = i;
            changedValues[changedCount] = newValues[i];
            changedCount++;
        }

        if (changedCount == 0) {
            return;
        }

        int fullPayloadBytes = sizeof(byte) + sizeof(int) + (newValues.Length * sizeof(int));
        int deltaPayloadBytes = sizeof(byte) + sizeof(int) + sizeof(int) + (changedCount * sizeof(int) * 2);
        if (deltaPayloadBytes < fullPayloadBytes) {
            int[] indicesToWrite = new int[changedCount];
            int[] valuesToWrite = new int[changedCount];
            Array.Copy(changedIndices, indicesToWrite, changedCount);
            Array.Copy(changedValues, valuesToWrite, changedCount);
            runFiles.WritePlayerDataIntArrayDeltaChange(fieldId, newValues.Length, indicesToWrite, valuesToWrite);
        } else {
            runFiles.WritePlayerDataIntArrayFullChange(fieldId, newValues);
        }

        UpdateIntArraySnapshot(ref oldValue, newValue);
    }

    private static void UpdateIntArraySnapshot(
        ref int[]? oldValue,
        int[]? newValue
    ) {
        if (newValue is null) {
            oldValue = Array.Empty<int>();
            return;
        }

        if (oldValue is { Length: var oldLength } existing && oldLength == newValue.Length) {
            Array.Copy(newValue, existing, newValue.Length);
            return;
        }

        int[] snapshot = new int[newValue.Length];
        Array.Copy(newValue, snapshot, newValue.Length);
        oldValue = snapshot;
    }

    public void WriteFloatIfChanged(
        ushort fieldId,
        float oldValue,
        float newValue
    ) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataFloatChange(
                fieldId: fieldId,
                value: newValue
            );
        }
    }

    public void WriteStringIfChanged(
        ushort fieldId,
        string? oldValue,
        string? newValue
    ) {
        if (!string.Equals(oldValue, newValue, StringComparison.Ordinal)) {
            runFiles.WritePlayerDataStringChange(
                fieldId: fieldId,
                value: newValue ?? string.Empty
            );
        }
    }

    public void WriteGuidIfChanged(
        ushort fieldId,
        Guid oldValue,
        Guid newValue
    ) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataGuidChange(
                fieldId: fieldId,
                value: newValue
            );
        }
    }
}
