using HKMirror.Reflection;
using Modding;
using Satchel.Futils.Serialiser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MapChanger.SpriteManager;

namespace HKViz {
    internal class EnemyWriter : Loggable {
        private RecordingFileManager recording = RecordingFileManager.Instance;
        private RecordingSerializer serializer = RecordingSerializer.Instance;
        private int previousEnemyId = 1;

        private static EnemyWriter? _instance;
        public static EnemyWriter Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new EnemyWriter();
                return _instance;
            }
        }

        public List<HKVizHealthManagerInfo> existingHealthManagers = new List<HKVizHealthManagerInfo>();

        public void SetupHooks() {
            On.HealthManager.Start += HealthManager_Start;
            On.HealthManager.TakeDamage += HealthManager_TakeDamage;
        }

        public void WriteEnemyPositions(long unixMillis) {
            for(int i = 0; i<existingHealthManagers.Count; i++) {
                var info = existingHealthManagers[i];

                recording.WriteSep();
                recording.Write(info.id);
                recording.WriteSep();

                var currentPositionString = serializer.serializePosition2D(info.trans.position); // no z position needed
                if (info.previousPositionString == currentPositionString) { // TODO reset previousPositionString when switching to new file
                    recording.Write("=");
                } else {
                    recording.Write(currentPositionString);
                    info.previousPositionString = currentPositionString;
                }
            }
        }

        private void HealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            orig(self, hitInstance);
            var info = self.GetComponent<HKVizHealthManagerInfo>();
            recording.WriteEntryPrefix(RecordingPrefixes.ENEMY_HEALTH);
            recording.Write(info.id);
            recording.WriteSep();
            recording.Write(self.hp.ToString());
            recording.WriteSep();
            recording.Write(hitInstance.AttackType switch {
                AttackTypes.Nail => "n",
                AttackTypes.Generic => "g",
                AttackTypes.Spell => "s",
                AttackTypes.Acid => "a",
                AttackTypes.Splatter => "p",
                AttackTypes.RuinsWater => "r",
                AttackTypes.SharpShadow => "h",
                AttackTypes.NailBeam => "b",
                _ => hitInstance.AttackType.ToString(),
            });
            if (hitInstance.SpecialType != SpecialTypes.None) {
                recording.WriteSep();
                recording.Write(hitInstance.SpecialType switch {
                    SpecialTypes.None => throw new NotImplementedException(),
                    SpecialTypes.Acid => "a",
                    _ => hitInstance.SpecialType.ToString(),
                });
            }
            recording.WriteNL();
        }

        private void HealthManager_Start(On.HealthManager.orig_Start orig, HealthManager self) {
            orig(self);
            var info = self.gameObject.AddComponent<HKVizHealthManagerInfo>();
            var deathEffects = self.GetComponent<EnemyDeathEffects>();
            info.healthManager = self;
            info.id = (++previousEnemyId).ToString();
            info.trans = self.transform;
            existingHealthManagers.Add(info);
            recording.WriteEntryPrefix(RecordingPrefixes.ENEMY_START);
            recording.Write(info.id);
            recording.WriteSep();
            recording.Write(deathEffects?.Reflect()?.playerDataName ?? "0"+ self.gameObject.name);
            recording.WriteSep();
            recording.Write(self.hp.ToString());
            recording.WriteNL();
        }

        internal void ResetIdCounterIfSceneChanged(Scene scene) {

        }

        internal void ActiveSceneChanged(Scene oldScene, Scene newScene) {
            previousEnemyId = 1;
        }
    }

    public class HKVizHealthManagerInfo: MonoBehaviour {
        public string id;
        public HealthManager healthManager;
        public Transform trans;
        public string? previousPositionString;


        void OnDestroy() {
            EnemyWriter.Instance.existingHealthManagers.Remove(this);
        }
    }
}