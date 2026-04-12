namespace HKViz.Silk.Recording.DataHelpers;

public static class CollectableMementosDataHelper {
    public static bool Equals(CollectableMementosData.Data left, CollectableMementosData.Data right) =>
        left.IsDeposited == right.IsDeposited && left.HasSeenInRelicBoard == right.HasSeenInRelicBoard;

    public static CollectableMementosData.Data Copy(CollectableMementosData.Data value) => value;

}



