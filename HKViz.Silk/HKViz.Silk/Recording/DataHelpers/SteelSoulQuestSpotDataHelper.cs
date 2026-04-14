using System.Collections.Generic;

namespace HKViz.Silk.Recording.DataHelpers;

public static class SteelSoulQuestSpotDataHelper {
    public static IEnumerable<KeyValuePair<string, bool>> EnumerateBySceneName(this SteelSoulQuestSpot.Spot[]? spots) {
        if (spots == null) {
            yield break;
        }

        for (int i = 0; i < spots.Length; i++) {
            SteelSoulQuestSpot.Spot spot = spots[i];
            if (string.IsNullOrEmpty(spot.SceneName)) {
                continue;
            }

            yield return new KeyValuePair<string, bool>(spot.SceneName, spot.IsSeen);
        }
    }
}


