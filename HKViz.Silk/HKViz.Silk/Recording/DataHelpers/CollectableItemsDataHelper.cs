namespace HKViz.Silk.Recording.DataHelpers;

public static class CollectableItemsDataHelper {
    public static bool Equals(CollectableItemsData.Data left, CollectableItemsData.Data right) =>
        left.Amount == right.Amount && left.IsSeenMask == right.IsSeenMask && left.AmountWhileHidden == right.AmountWhileHidden;

    public static CollectableItemsData.Data Copy(CollectableItemsData.Data value) => value;

}



