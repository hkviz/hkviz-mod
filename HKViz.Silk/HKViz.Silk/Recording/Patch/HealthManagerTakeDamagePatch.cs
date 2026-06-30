using System;
using HarmonyLib;
using UnityEngine;

namespace HKViz.Silk.Recording.Patch;

[HarmonyPatch(typeof(HealthManager), nameof(HealthManager.TakeDamage))]
internal static class HealthManagerTakeDamagePatch {
    static void Postfix(HealthManager __instance, HitInstance hitInstance) {
        Debug.Log("[hkviz] HealthManagerTakeDamagePatch called");
        On?.Invoke(__instance, hitInstance);
    }

    public static event Action<HealthManager, HitInstance>? On;
}