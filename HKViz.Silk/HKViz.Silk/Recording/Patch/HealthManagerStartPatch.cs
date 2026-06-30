using System;
using HarmonyLib;
using UnityEngine;

namespace HKViz.Silk.Recording.Patch;

[HarmonyPatch(typeof(HealthManager), nameof(HealthManager.Start))]
internal static class HealthManagerStartPatch {
    static void Postfix(HealthManager __instance) {
        Debug.Log("[hkviz] HealthManagerStartPatch called");
        On?.Invoke(__instance);
    }

    public static event Action<HealthManager>? On;
}