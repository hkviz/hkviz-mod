namespace HKViz.Silk.Recording.DataHelpers;

public static class MateriumItemsDataHelper {
	public static bool Equals(MateriumItemsData.Data left, MateriumItemsData.Data right) =>
		left.IsCollected == right.IsCollected && left.HasSeenInRelicBoard == right.HasSeenInRelicBoard;

	public static MateriumItemsData.Data Copy(MateriumItemsData.Data value) => value;

}

