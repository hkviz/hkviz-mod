using System;
using HarmonyLib;
using UnityEngine;

namespace HKViz.Silk.Recording.Patch;

[HarmonyPatch(typeof(GameMap), nameof(GameMap.LevelReady))]
internal static class GameMapLevelReadyPatch {
    static void Postfix(GameMap __instance) {
        var size = __instance.currentSceneSize;
        OnGameMapLevelReady?.Invoke(size);
    }

    public static event Action<Vector2>? OnGameMapLevelReady;
}
