using System;
using UnityEngine;

namespace HKViz.Silk.Recording.Behaviours;

public class HKVizHealthManagerBehaviour : MonoBehaviour {
    public ushort id;
    public HealthManager healthManager;
    public EnemyDeathEffects? deathEffects;
    public Transform trans;

    public int? lastWrittenHp = null;
    public Vector3? lastWrittenPosition = null;


    void OnDestroy() {
        OnDestroyed?.Invoke(this);
    }

    public static event Action<HKVizHealthManagerBehaviour>? OnDestroyed;
}
