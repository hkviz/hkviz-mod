using System.Collections.Generic;
using HKViz.Silk.Recording.Behaviours;
using HKViz.Silk.Recording.Patch;

namespace HKViz.Silk.Recording;

public class EnemyWriter {
    private RunFiles runFiles;

    private ushort nextId = 0;
    private List<HKVizHealthManagerBehaviour> healthManagers = [];
    private Dictionary<HealthManager, HKVizHealthManagerBehaviour> healthManagerToBehaviour = [];

    public EnemyWriter(RunFiles runFiles) {
        this.runFiles = runFiles;

        HealthManagerStartPatch.On += OnHealthManagerStart;
        HealthManagerTakeDamagePatch.On += OnHealthManagerDamaged;
        HKVizHealthManagerBehaviour.OnDestroyed += OnHealthManagerDestroyed;
    }

    private void OnHealthManagerStart(HealthManager healthManager) {
        if (healthManager.gameObject.GetComponent<HKVizHealthManagerBehaviour>() != null) {
            return;
        }
        var hkvizBehaviour = healthManager.gameObject.AddComponent<HKVizHealthManagerBehaviour>();
        var deathEffects = healthManager.GetComponent<EnemyDeathEffects>();
        ushort id = nextId;
        nextId = unchecked((ushort)(nextId + 1)); // overflows back to zero
        hkvizBehaviour.deathEffects = deathEffects;
        hkvizBehaviour.id = id;
        hkvizBehaviour.healthManager = healthManager;
        hkvizBehaviour.trans = healthManager.transform;
        healthManagers.Add(hkvizBehaviour);
        healthManagerToBehaviour[healthManager] = hkvizBehaviour;

        var journalName = deathEffects.journalRecord.name;
        
        runFiles.WriteEnemyStart(id, journalName, healthManager.hp);
    }

    private void OnHealthManagerDestroyed(HKVizHealthManagerBehaviour behaviour) {
        healthManagers.Remove(behaviour);
        healthManagerToBehaviour.Remove(behaviour.healthManager);
    }

    private void OnHealthManagerDamaged(HealthManager healthManager, HitInstance hit) {
        var behaviour = healthManagerToBehaviour[healthManager];
        if (behaviour == null) {
            return;
        }
        runFiles.WriteEnemyTakeDamage(behaviour, hit);
    }

    public void WriteChanged(bool frequencyUpdate) {
        if (frequencyUpdate) {
            runFiles.WriteEnemyLocationDiff(healthManagers);
        }
        runFiles.WriteEnemyHpDiff(healthManagers);
    }
}
