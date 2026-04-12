namespace HKViz.Silk.Recording.DataHelpers;

public static class CollectableRelicsDataHelper {
    public static bool Equals(CollectableRelicsData.Data left, CollectableRelicsData.Data right) =>
        left.IsCollected == right.IsCollected && left.IsDeposited == right.IsDeposited && left.HasSeenInRelicBoard == right.HasSeenInRelicBoard;

    public static CollectableRelicsData.Data Copy(CollectableRelicsData.Data value) => value;

}



