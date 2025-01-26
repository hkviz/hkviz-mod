using Core.FsmUtil;
using HutongGames.PlayMaker.Actions;
using Modding;
using Satchel;
using System;
using System.Linq;
using UnityEngine;

namespace HKViz {
    internal class PlayerHealthWriter: Loggable {
        private RecordingFileManager recording = RecordingFileManager.Instance;
        private RecordingSerializer serializer = RecordingSerializer.Instance;

        private static PlayerHealthWriter? instance;
        public static PlayerHealthWriter Instance {
            get {
                if (instance == null) {
                    instance = new PlayerHealthWriter();
                }
                return instance;
            }
        }

        int healthPreHeal = 0;


        public void InitFsm() {

            // ----- FOCUSING / HEALING ----
            Hooks.HookStateExited(new FSMData(
                FsmName: "Spell Control",
                StateName: "Focus Start"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.FOCUS_START);
            });

            Hooks.HookStateExitedViaTransition(new FSMData(
                FsmName: "Spell Control",
                StateName: "Focus"
            ), (fms, transition) => {
                // transition tells us if the focus was successful, canceled releasing the button, taking damage or sth else.
                // if it is completed, the write happens in another hook, since here variables for health increase are not set yet.
                if (transition == "FOCUS COMPLETED") {
                    return; // this case is handled in the next hook
                }
                var transitionShort = transition switch {
                    // "FOCUS COMPLETED" => , // success // already returned
                    "HERO DAMAGED" => "d", // canceled by taking damage
                    "BUTTON UP" => "c", // canceled by player releasing key
                    _ => transition,
                };
                recording.WriteEntry(RecordingPrefixes.FOCUS_CANCELED, transitionShort);
            });

            Hooks.HookStateEntered(new FSMData(
                FsmName: "Spell Control",
                StateName: "Focus Heal"
            ), (fsm) => {
                healthPreHeal = PlayerData.instance.health;
            });
        }

        internal void InitNewKnight(Transform knight) {
            var fsm = knight.gameObject.LocateMyFSM("Spell Control");
            var foundState = fsm.TryGetState("Focus Heal", out var healState);
            if (!foundState) {
                LogError("HKViz unable to locate Focus Heal state in Spell Control");
                return;
            }

            // Wait Action should always be the last one in an unmodded game
            // this search hopefully makes it a bit more recilient to other mods.
            var waitIndex = -1;
            for(var i=0; i<healState.Actions.Length; i++) {
                var action = healState.Actions[i];
                if (action is Wait) {
                    waitIndex = i;
                    break;
                }
            }

            Action successfulHealAction = () => {

                // here healing success is handled, since vars are set:
                var theoreticallyHealedMasks = fsm.GetIntVariable("Health Increase")?.Value ?? 0;

                var healthPostHeal = HeroController.instance.playerData.health;
                //var actualHealed = healthPostHeal - healthPreHeal;

                //Log("Success heal theoreticallyHealedMasks=" + theoreticallyHealedMasks + " healthPostHeal=" + healthPostHeal +
                //    " actualHealed=" + actualHealed + " healthPreHeal=" + healthPreHeal
                //    );

                recording.WriteEntryPrefix(RecordingPrefixes.FOCUS_SUCCESS);
                recording.Write(serializer.serialize(theoreticallyHealedMasks));
                recording.WriteSep();
                recording.Write(serializer.serialize(healthPreHeal));
                recording.WriteSep();
                recording.Write(serializer.serialize(healthPostHeal));
                recording.WriteNL();
            };

            if (waitIndex >= 0) {
                //Log("Initialized heal recording before wait. At action index" + waitIndex);
                healState.InsertCustomAction(successfulHealAction, waitIndex);
            } else {
                Log("Could not find WAIT action in healing fsm, heal recording may be less accurate");
                healState.AddCustomAction(successfulHealAction);
            }

            //Log(String.Join(", ", healState.Actions.Select(it => it.GetType().Name).ToArray()));
        }
    }
}
