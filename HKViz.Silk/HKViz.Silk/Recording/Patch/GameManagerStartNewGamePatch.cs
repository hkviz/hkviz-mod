using System;
using HarmonyLib;
using UnityEngine;

namespace HKViz.Silk.Recording.Patch;

[HarmonyPatch(typeof(GameManager), nameof(GameManager.StartNewGame))]
internal static class GameManagerStartNewGamePatch {
    static void Postfix(GameManager __instance, bool permadeathMode, bool bossRushMode) {
        Debug.Log("[hkviz] StartNewGame called");
        OnStartNewGame?.Invoke();
    }

    public static event Action? OnStartNewGame;
}
