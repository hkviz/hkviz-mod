using System;
using HarmonyLib;
using UnityEngine;

namespace HKViz.Silk.Recording;

[HarmonyPatch(typeof(GameMap), nameof(GameMap.LevelReady))]
internal static class GameMapLevelReadyPatch {
    static void Postfix(GameMap __instance) {
        Debug.Log("[silk] LevelReady postfix called");
        var size = __instance.currentSceneSize;
        Debug.Log($"[silk] width: {size.x}, height: {size.y}");
        OnGameMapLevelReady?.Invoke(size);
    }

    public static event Action<Vector2>? OnGameMapLevelReady;
}
