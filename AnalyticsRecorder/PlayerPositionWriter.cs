using Modding;
using Satchel.Futils.Serialiser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnalyticsRecorder {
    internal class PlayerPositionWriter {
        private static float WRITE_PERIOD_SECONDS = .5f;

        private RecordingFileManager recording = RecordingFileManager.Instance;
        private RecordingSerializer serializer = RecordingSerializer.Instance;

        private static PlayerPositionWriter instance;
        public static PlayerPositionWriter Instance {
            get {
                if (instance == null) {
                    instance = new PlayerPositionWriter();
                }
                return instance;
            }
        }


        private Transform? knight;
        private int profileId = -1;
        private float lastFreqWriteTime = 0;

        private string previousPositionString = "";

        public void Ininitialize() {
            ModHooks.HeroUpdateHook += HeroUpdateHook;
            recording.AfterSwitchedFile += Recording_AfterSwitchedFile;
        }

        private void Recording_AfterSwitchedFile() {
            previousPositionString = "";
        }

        private void HeroUpdateHook() {
            var unixMillis = recording.GetUnixMillis();

            if (GameManager.instance.isPaused) return;

            if (knight == null) { // if destroyed needs to find new player
                InitKnight();
            }

            if (knight != null) {
                var time = Time.time;
                if (time - lastFreqWriteTime > WRITE_PERIOD_SECONDS) {
                    recording.WriteEntryPrefix(RecordingPrefixes.ENTITY_POSITIONS, unixMillis: unixMillis);
                    var currentPositionString = serializer.serializePosition2D(knight.position); 
                    if (previousPositionString == currentPositionString) { // TODO reset previousPositionString when switching to new file
                        recording.Write("=");
                    } else {
                        recording.Write(currentPositionString);
                        previousPositionString = currentPositionString;
                    }

                    // in the same line all enemy positions are logged
                    EnemyWriter.Instance.WriteEnemyPositions(unixMillis: unixMillis);

                    recording.WriteNL();

                    lastFreqWriteTime = time;
                }
            }
        }
        private void InitKnight() {
            knight = GameObject.Find("Knight").transform;

            //Log("|" + String.Join("|", knight.gameObject.LocateMyFSM("Nail Arts").FsmStates.Select(it => it.Name).ToList()) + "|");
            //Log("|" + String.Join("|", knight.gameObject.LocateMyFSM("Spell Control").FsmStates.Select(it => it.Name).ToList()) + "|");
            //Log("|" + String.Join("|", knight.gameObject.LocateMyFSM("Superdash").FsmStates.Select(it => it.Name).ToList()) + "|");
        }
    }
}
