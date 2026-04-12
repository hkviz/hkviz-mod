using System;
using System.IO;
using UnityEngine;

namespace HKViz.Silk.Recording.DataHelpers;

public static class WrappedVector2ListDataHelper {
    public static bool Equals(WrappedVector2List? left, WrappedVector2List? right) {
        if (left == null || right == null) {
            return left == null && right == null;
        }

        var leftItems = left.List;
        var rightItems = right.List;
        int leftCount = leftItems?.Count ?? 0;
        int rightCount = rightItems?.Count ?? 0;
        if (leftCount != rightCount) {
            return false;
        }

        for (int i = 0; i < leftCount; i++) {
            Vector2 leftValue = leftItems![i];
            Vector2 rightValue = rightItems![i];
            if (leftValue != rightValue) {
                return false;
            }
        }

        return true;
    }

    public static WrappedVector2List? Copy(WrappedVector2List? value) {
        if (value == null) {
            return null;
        }

        WrappedVector2List copy = new();
        copy.List.Clear();
        if (value.List != null) {
            for (int i = 0; i < value.List.Count; i++) {
                copy.List.Add(value.List[i]);
            }
        }
        return copy;
    }

    public static WrappedVector2List[] CopyArray(WrappedVector2List[]? value) {
        if (value == null || value.Length == 0) {
            return Array.Empty<WrappedVector2List>();
        }

        WrappedVector2List[] copy = new WrappedVector2List[value.Length];
        for (int i = 0; i < value.Length; i++) {
            copy[i] = Copy(value[i]) ?? new WrappedVector2List();
        }

        return copy;
    }

    public static void Write(BinaryWriter writer, WrappedVector2List? value) {
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
}


