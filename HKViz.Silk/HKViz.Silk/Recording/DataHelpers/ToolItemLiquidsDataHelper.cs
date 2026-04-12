namespace HKViz.Silk.Recording.DataHelpers;

public static class ToolItemLiquidsDataHelper {
	public static bool Equals(ToolItemLiquidsData.Data left, ToolItemLiquidsData.Data right) =>
		left.RefillsLeft == right.RefillsLeft && left.SeenEmptyState == right.SeenEmptyState && left.UsedExtra == right.UsedExtra;

	public static ToolItemLiquidsData.Data Copy(ToolItemLiquidsData.Data value) => value;

}

