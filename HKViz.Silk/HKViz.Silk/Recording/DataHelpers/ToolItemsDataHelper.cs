namespace HKViz.Silk.Recording.DataHelpers;

public static class ToolItemsDataHelper {
    public static bool Equals(ToolItemsData.Data left, ToolItemsData.Data right) =>
        left.IsUnlocked == right.IsUnlocked && left.IsHidden == right.IsHidden && left.HasBeenSeen == right.HasBeenSeen && left.HasBeenSelected == right.HasBeenSelected && left.AmountLeft == right.AmountLeft;

    public static ToolItemsData.Data Copy(ToolItemsData.Data value) => value;

}



