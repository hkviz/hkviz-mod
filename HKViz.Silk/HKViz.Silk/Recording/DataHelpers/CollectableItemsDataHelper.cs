using System.IO;

namespace HKViz.Silk.Recording.DataHelpers;

public static class CollectableItemsDataHelper {
    public static bool Equals(CollectableItemsData.Data left, CollectableItemsData.Data right) =>
        left.Amount == right.Amount && left.IsSeenMask == right.IsSeenMask && left.AmountWhileHidden == right.AmountWhileHidden;

    public static CollectableItemsData.Data Copy(CollectableItemsData.Data value) => value;

    public static void Write(BinaryWriter writer, CollectableItemsData.Data value) {
        writer.Write(value.Amount);
        writer.Write(value.IsSeenMask);
        writer.Write(value.AmountWhileHidden);
    }
}



