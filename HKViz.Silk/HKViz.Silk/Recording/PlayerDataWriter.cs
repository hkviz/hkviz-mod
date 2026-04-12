using System;
using System.Collections.Generic;
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
        int oldLength = oldValues.Length;
        int newLength = newValues.Length;
        int overlapLength = Math.Min(oldLength, newLength);

        int changedCount = 0;
        for (int i = 0; i < overlapLength; i++) {
            if (oldValues[i] != newValues[i]) {
                changedCount++;
            }
        }

        if (newLength > overlapLength) {
            changedCount += newLength - overlapLength;
        }

        if (changedCount == 0 && oldLength == newLength) {
            return;
        }

        int fullPayloadBytes = sizeof(byte) + sizeof(int) + (newLength * sizeof(int));
        int deltaPayloadBytes = sizeof(byte) + sizeof(int) + sizeof(int) + (changedCount * sizeof(int) * 2);
        if (deltaPayloadBytes < fullPayloadBytes) {
            int[] changedIndices = new int[changedCount];
            int[] changedValues = new int[changedCount];
            int writeIndex = 0;

            for (int i = 0; i < overlapLength; i++) {
                if (oldValues[i] == newValues[i]) {
                    continue;
                }

                changedIndices[writeIndex] = i;
                changedValues[writeIndex] = newValues[i];
                writeIndex++;
            }

            for (int i = overlapLength; i < newLength; i++) {
                changedIndices[writeIndex] = i;
                changedValues[writeIndex] = newValues[i];
                writeIndex++;
            }

            runFiles.WritePlayerDataIntArrayDeltaChange(fieldId, newLength, changedIndices, changedValues);
        } else {
            runFiles.WritePlayerDataIntArrayFullChange(fieldId, newValues);
        }

        UpdateIntArraySnapshot(ref oldValue, newValue);
    }

    public void WriteIntListIfChanged(
        ushort fieldId,
        ref List<int>? oldValue,
        List<int>? newValue
    ) {
        List<int> oldValues = oldValue ?? [];
        List<int> newValues = newValue ?? [];
        int oldLength = oldValues.Count;
        int newLength = newValues.Count;
        int overlapLength = Math.Min(oldLength, newLength);

        int changedCount = 0;
        for (int i = 0; i < overlapLength; i++) {
            if (oldValues[i] != newValues[i]) {
                changedCount++;
            }
        }

        if (newLength > overlapLength) {
            changedCount += newLength - overlapLength;
        }

        if (changedCount == 0 && oldLength == newLength) {
            return;
        }

        int fullPayloadBytes = sizeof(byte) + sizeof(int) + (newLength * sizeof(int));
        int deltaPayloadBytes = sizeof(byte) + sizeof(int) + sizeof(int) + (changedCount * sizeof(int) * 2);
        if (deltaPayloadBytes < fullPayloadBytes) {
            int[] changedIndices = new int[changedCount];
            int[] changedValues = new int[changedCount];
            int writeIndex = 0;

            for (int i = 0; i < overlapLength; i++) {
                if (oldValues[i] == newValues[i]) {
                    continue;
                }

                changedIndices[writeIndex] = i;
                changedValues[writeIndex] = newValues[i];
                writeIndex++;
            }

            for (int i = overlapLength; i < newLength; i++) {
                changedIndices[writeIndex] = i;
                changedValues[writeIndex] = newValues[i];
                writeIndex++;
            }

            runFiles.WritePlayerDataIntArrayDeltaChange(fieldId, newLength, changedIndices, changedValues);
        } else {
            runFiles.WritePlayerDataIntListFullChange(fieldId, newValues);
        }

        UpdateIntListSnapshot(ref oldValue, newValue);
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

    private static void UpdateIntListSnapshot(
        ref List<int>? oldValue,
        List<int>? newValue
    ) {
        if (newValue is null || newValue.Count == 0) {
            oldValue = null;
            return;
        }

        if (oldValue is { Count: var oldCount } existing && oldCount == newValue.Count) {
            for (int i = 0; i < newValue.Count; i++) {
                existing[i] = newValue[i];
            }
            return;
        }

        oldValue = new List<int>(newValue);
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

    public void WriteStringListIfChanged(
        ushort fieldId,
        ref List<string>? oldValue,
        List<string>? newValue
    ) {
        CompareStringCollections(fieldId, ref oldValue, newValue);
    }

    public void WriteStringSetIfChanged(
        ushort fieldId,
        ref HashSet<string>? oldValue,
        HashSet<string>? newValue
    ) {
        HashSet<string> oldSet = oldValue ?? [];
        HashSet<string> newSet = newValue ?? [];

        if (oldSet.Count == newSet.Count && oldSet.SetEquals(newSet)) {
            return;  // No changes
        }

        // For sets, compute added and removed elements
        List<string> added = [];
        List<string> removed = [];

        foreach (string item in newSet) {
            if (!oldSet.Contains(item)) {
                added.Add(item);
            }
        }

        foreach (string item in oldSet) {
            if (!newSet.Contains(item)) {
                removed.Add(item);
            }
        }

        // Delta vs full write decision
        int deltaPayloadEstimate = sizeof(byte) + sizeof(int) * 2 + (added.Count + removed.Count) * 32;
        int fullPayloadEstimate = sizeof(byte) + sizeof(int) + newSet.Count * 32;

        if (deltaPayloadEstimate < fullPayloadEstimate && (added.Count > 0 || removed.Count > 0)) {
            runFiles.WritePlayerDataStringSetDeltaChange(fieldId, added, removed);
        } else {
            runFiles.WritePlayerDataStringSetFullChange(fieldId, newSet);
        }

        UpdateStringSetSnapshot(ref oldValue, newSet);
    }

    private void CompareStringCollections(
        ushort fieldId,
        ref List<string>? oldValue,
        List<string>? newValue
    ) {
        List<string> oldList = oldValue ?? [];
        List<string> newList = newValue ?? [];

        int oldCount = oldList.Count;
        int newCount = newList.Count;
        int overlapCount = Math.Min(oldCount, newCount);

        int changedCount = 0;
        for (int i = 0; i < overlapCount; i++) {
            if (!string.Equals(oldList[i], newList[i], StringComparison.Ordinal)) {
                changedCount++;
            }
        }

        if (newCount > overlapCount) {
            changedCount += newCount - overlapCount;
        }

        if (changedCount == 0 && oldCount == newCount) {
            return;  // No changes
        }

        // Decide between delta and full write based on payload size estimation
        // For lists, delta is better when only a few items changed
        if (changedCount <= newCount / 2 || (newCount > overlapCount && changedCount == newCount - overlapCount)) {
            // Delta write: report only changed indices and new appended items
            int[] changedIndices = new int[changedCount];
            string[] changedValues = new string[changedCount];
            int writeIndex = 0;

            for (int i = 0; i < overlapCount; i++) {
                if (!string.Equals(oldList[i], newList[i], StringComparison.Ordinal)) {
                    changedIndices[writeIndex] = i;
                    changedValues[writeIndex] = newList[i] ?? string.Empty;
                    writeIndex++;
                }
            }

            for (int i = overlapCount; i < newCount; i++) {
                changedIndices[writeIndex] = i;
                changedValues[writeIndex] = newList[i] ?? string.Empty;
                writeIndex++;
            }

            runFiles.WritePlayerDataStringCollectionDeltaChange(fieldId, newCount, changedIndices, changedValues);
        } else {
            // Full write
            string[] newArray = newList.ToArray();
            runFiles.WritePlayerDataStringCollectionFullChange(fieldId, newArray);
        }

        UpdateStringListSnapshot(ref oldValue, newList);
    }

    private static void UpdateStringListSnapshot(
        ref List<string>? oldValue,
        List<string>? newValue
    ) {
        if (newValue is null || newValue.Count == 0) {
            oldValue = null;
            return;
        }

        if (oldValue is { Count: var oldCount } existing && oldCount == newValue.Count) {
            // Update in-place
            for (int i = 0; i < newValue.Count; i++) {
                existing[i] = newValue[i];
            }
            return;
        }

        // Create new snapshot
        oldValue = new List<string>(newValue);
    }

    private static void UpdateStringSetSnapshot(
        ref HashSet<string>? oldValue,
        HashSet<string>? newValue
    ) {
        if (newValue is null || newValue.Count == 0) {
            oldValue = null;
            return;
        }

        if (oldValue == null) {
            oldValue = new HashSet<string>(newValue, StringComparer.Ordinal);
            return;
        }

        oldValue.Clear();
        oldValue.UnionWith(newValue);
    }
}
