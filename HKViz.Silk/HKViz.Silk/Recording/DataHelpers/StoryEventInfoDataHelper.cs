namespace HKViz.Silk.Recording.DataHelpers;

public static class StoryEventInfoDataHelper {
    public static bool Equals(PlayerStory.EventInfo left, PlayerStory.EventInfo right) =>
        left.EventType == right.EventType
        && string.Equals(left.SceneName, right.SceneName, System.StringComparison.Ordinal)
        && left.PlayTime == right.PlayTime;

    public static PlayerStory.EventInfo Copy(PlayerStory.EventInfo value) => value;

}

