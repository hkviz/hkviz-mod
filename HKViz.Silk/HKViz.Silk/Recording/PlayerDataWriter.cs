using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HKViz.Silk.Recording.DataHelpers;

namespace HKViz.Silk.Recording;

public partial class PlayerDataWriter(RunFiles runFiles) {
    public partial void WriteAll();
    public partial void WriteChanged(bool writeFrequentChangeFields);

    public void WriteBoolIfChanged(ushort fieldId, bool oldValue, bool newValue) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataBoolChange(fieldId, newValue);
        }
    }

    public void WriteIntIfChanged(ushort fieldId, int oldValue, int newValue) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataIntChange(fieldId, newValue);
        }
    }

    public void WriteEnumIfChanged(ushort fieldId, ushort oldValue, ushort newValue) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataEnumChange(fieldId, newValue);
        }
    }

    public void WriteULongIfChanged(ushort fieldId, ulong oldValue, ulong newValue) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataULongChange(fieldId, newValue);
        }
    }

    public void WriteVector3IfChanged(ushort fieldId, Vector3 oldValue, Vector3 newValue) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataVector3Change(fieldId, newValue);
        }
    }

    public void WriteVector2IfChanged(ushort fieldId, Vector2 oldValue, Vector2 newValue) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataVector2Change(fieldId, newValue);
        }
    }

    public void WriteIntArrayIfChanged(ushort fieldId, ref int[]? oldValue, int[]? newValue) {
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

    public void WriteIntListIfChanged(ushort fieldId, ref List<int>? oldValue, List<int>? newValue) {
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

    public void WriteFloatIfChanged(ushort fieldId, float oldValue, float newValue) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataFloatChange(fieldId, newValue);
        }
    }

    public void WriteStringIfChanged(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
        string? oldValue,
        string? newValue
    ) {
        if (!string.Equals(oldValue, newValue, StringComparison.Ordinal)) {
            runFiles.WritePlayerDataStringChange(fieldId, valueToId, newValue ?? string.Empty);
        }
    }

    public void WriteGuidIfChanged(ushort fieldId, Guid oldValue, Guid newValue) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataGuidChange(fieldId, newValue);
        }
    }

    public void WriteStringListIfChanged(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
        ref List<string>? oldValue,
        List<string>? newValue
    ) {
        CompareStringCollections(fieldId, valueToId, ref oldValue, newValue);
    }

    public void WriteStringSetIfChanged(
        ushort fieldId, 
        Dictionary<string, ushort> valueToId,
        ref HashSet<string>? oldValue,
        HashSet<string>? newValue
    ) {
        HashSet<string> oldSet = oldValue ?? [];
        HashSet<string> newSet = newValue ?? [];

        if (oldSet.Count == newSet.Count && oldSet.SetEquals(newSet)) {
            return;
        }

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

        int deltaPayloadEstimate = sizeof(byte) + sizeof(int) * 2 + (added.Count + removed.Count) * 32;
        int fullPayloadEstimate = sizeof(byte) + sizeof(int) + newSet.Count * 32;

        if (deltaPayloadEstimate < fullPayloadEstimate && (added.Count > 0 || removed.Count > 0)) {
            runFiles.WritePlayerDataStringSetDeltaChange(fieldId, valueToId, added, removed);
        } else {
            runFiles.WritePlayerDataStringSetFullChange(fieldId, valueToId, newSet);
        }

        UpdateStringSetSnapshot(ref oldValue, newSet);
    }

    public void WriteStoryEventListIfChanged(ushort fieldId, ref List<PlayerStory.EventInfo>? oldValue, List<PlayerStory.EventInfo>? newValue) {
        List<PlayerStory.EventInfo> oldList = oldValue ?? [];
        List<PlayerStory.EventInfo> newList = newValue ?? [];

        int oldCount = oldList.Count;
        int newCount = newList.Count;
        int overlapCount = Math.Min(oldCount, newCount);

        int changedCount = 0;
        for (int i = 0; i < overlapCount; i++) {
            if (!StoryEventInfoDataHelper.Equals(oldList[i], newList[i])) {
                changedCount++;
            }
        }

        if (newCount > overlapCount) {
            changedCount += newCount - overlapCount;
        }

        if (changedCount == 0 && oldCount == newCount) {
            return;
        }

        int fullPayloadEstimate = sizeof(byte) + sizeof(int) + (newCount * 48);
        int deltaPayloadEstimate = sizeof(byte) + sizeof(int) + sizeof(int) + (changedCount * (sizeof(int) + 48));
        if (deltaPayloadEstimate < fullPayloadEstimate) {
            int[] changedIndices = new int[changedCount];
            PlayerStory.EventInfo[] changedValues = new PlayerStory.EventInfo[changedCount];
            int writeIndex = 0;

            for (int i = 0; i < overlapCount; i++) {
                if (StoryEventInfoDataHelper.Equals(oldList[i], newList[i])) {
                    continue;
                }

                changedIndices[writeIndex] = i;
                changedValues[writeIndex] = StoryEventInfoDataHelper.Copy(newList[i]);
                writeIndex++;
            }

            for (int i = overlapCount; i < newCount; i++) {
                changedIndices[writeIndex] = i;
                changedValues[writeIndex] = StoryEventInfoDataHelper.Copy(newList[i]);
                writeIndex++;
            }

            runFiles.WritePlayerDataStoryEventListDeltaChange(fieldId, newCount, changedIndices, changedValues);
        } else {
            runFiles.WritePlayerDataStoryEventListFullChange(fieldId, newList);
        }

        UpdateStoryEventListSnapshot(ref oldValue, newList);
    }

    public void WritePlacedMarkersIfChanged(ushort fieldId, ref WrappedVector2List[]? oldValue, WrappedVector2List[]? newValue) {
        WrappedVector2List[] oldList = oldValue ?? Array.Empty<WrappedVector2List>();
        WrappedVector2List[] newList = newValue ?? Array.Empty<WrappedVector2List>();

        int oldCount = oldList.Length;
        int newCount = newList.Length;
        int overlapCount = Math.Min(oldCount, newCount);

        int changedCount = 0;
        for (int i = 0; i < overlapCount; i++) {
            if (!WrappedVector2ListDataHelper.Equals(oldList[i], newList[i])) {
                changedCount++;
            }
        }

        if (newCount > overlapCount) {
            changedCount += newCount - overlapCount;
        }

        if (changedCount == 0 && oldCount == newCount) {
            return;
        }

        int fullPayloadEstimate = sizeof(byte) + sizeof(int) + (newCount * 36);
        int deltaPayloadEstimate = sizeof(byte) + sizeof(int) + sizeof(int) + (changedCount * (sizeof(int) + 36));
        if (deltaPayloadEstimate < fullPayloadEstimate) {
            int[] changedIndices = new int[changedCount];
            WrappedVector2List[] changedValues = new WrappedVector2List[changedCount];
            int writeIndex = 0;

            for (int i = 0; i < overlapCount; i++) {
                if (WrappedVector2ListDataHelper.Equals(oldList[i], newList[i])) {
                    continue;
                }

                changedIndices[writeIndex] = i;
                changedValues[writeIndex] = WrappedVector2ListDataHelper.Copy(newList[i]) ?? new WrappedVector2List();
                writeIndex++;
            }

            for (int i = overlapCount; i < newCount; i++) {
                changedIndices[writeIndex] = i;
                changedValues[writeIndex] = WrappedVector2ListDataHelper.Copy(newList[i]) ?? new WrappedVector2List();
                writeIndex++;
            }

            runFiles.WritePlayerDataWrappedVector2ListDeltaChange(fieldId, newCount, changedIndices, changedValues);
        } else {
            runFiles.WritePlayerDataWrappedVector2ListFullChange(fieldId, newList);
        }

        oldValue = WrappedVector2ListDataHelper.CopyArray(newList);
    }

    public void WriteNamedMapChanges<TData>(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
        ref Dictionary<string, TData>? oldValue,
        IEnumerable<KeyValuePair<string, TData>>? newValues,
        Func<TData, TData, bool> equals,
        Func<TData, TData> copy,
        Action<BinaryWriter, TData> writeValue,
        bool writeAll
    ) {
        Dictionary<string, TData> oldSnapshot = oldValue ?? new Dictionary<string, TData>(StringComparer.Ordinal);
        Dictionary<string, TData> newSnapshot = new(StringComparer.Ordinal);
        List<KeyValuePair<string, TData>> upserts = [];
        List<string> removed = [];

        IEnumerable<KeyValuePair<string, TData>> sourceValues = newValues ?? Array.Empty<KeyValuePair<string, TData>>();

        foreach (KeyValuePair<string, TData> entry in sourceValues) {
            TData copied = copy(entry.Value);
            newSnapshot[entry.Key] = copied;
            if (!oldSnapshot.TryGetValue(entry.Key, out TData oldEntry) || !equals(oldEntry, entry.Value)) {
                upserts.Add(new KeyValuePair<string, TData>(entry.Key, copied));
            }
        }

        foreach (KeyValuePair<string, TData> entry in oldSnapshot) {
            if (!newSnapshot.ContainsKey(entry.Key)) {
                removed.Add(entry.Key);
            }
        }

        if (!writeAll && upserts.Count == 0 && removed.Count == 0 && oldSnapshot.Count == newSnapshot.Count) {
            return;
        }

        upserts.Sort((left, right) => string.CompareOrdinal(left.Key, right.Key));
        removed.Sort(StringComparer.Ordinal);

        int fullPayloadEstimate = newSnapshot.Count * 32;
        int deltaPayloadEstimate = (upserts.Count * 32) + (removed.Count * 8);
        if (!writeAll && deltaPayloadEstimate < fullPayloadEstimate) {
            runFiles.WritePlayerDataNamedMapDeltaChange(fieldId, valueToId, upserts, removed, writeValue);
        } else {
            runFiles.WritePlayerDataNamedMapFullChange(fieldId, valueToId, newSnapshot, writeValue);
        }

        oldValue = newSnapshot;
    }

    private static void UpdateIntArraySnapshot(ref int[]? oldValue, int[]? newValue) {
        if (newValue is null) {
            if (oldValue is not { Length: 0 }) {
                oldValue = [];
            }
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

    private static void UpdateIntListSnapshot(ref List<int>? oldValue, List<int>? newValue) {
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

    private static void UpdateStringListSnapshot(ref List<string>? oldValue, List<string>? newValue) {
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

        oldValue = new List<string>(newValue);
    }

    private static void UpdateStringSetSnapshot(ref HashSet<string>? oldValue, HashSet<string>? newValue) {
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

    private static void UpdateStoryEventListSnapshot(ref List<PlayerStory.EventInfo>? oldValue, List<PlayerStory.EventInfo>? newValue) {
        if (newValue is null || newValue.Count == 0) {
            oldValue = null;
            return;
        }

        if (oldValue is { Count: var oldCount } existing && oldCount == newValue.Count) {
            for (int i = 0; i < newValue.Count; i++) {
                existing[i] = StoryEventInfoDataHelper.Copy(newValue[i]);
            }
            return;
        }

        oldValue = new List<PlayerStory.EventInfo>(newValue.Count);
        for (int i = 0; i < newValue.Count; i++) {
            oldValue.Add(StoryEventInfoDataHelper.Copy(newValue[i]));
        }
    }

    private void CompareStringCollections(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
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
            return;
        }

        if (changedCount <= newCount / 2 || (newCount > overlapCount && changedCount == newCount - overlapCount)) {
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

            runFiles.WritePlayerDataStringCollectionDeltaChange(fieldId, valueToId, newCount, changedIndices, changedValues);
        } else {
            runFiles.WritePlayerDataStringCollectionFullChange(fieldId, valueToId, newList.ToArray());
        }

        UpdateStringListSnapshot(ref oldValue, newList);
    }
}
