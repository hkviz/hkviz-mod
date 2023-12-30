using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HKViz {
    internal record HeroControllerQueueValue(string fieldName, string valueString, float timestamp);

    internal class HeroControllerWriter : Loggable {
        private RecordingFileManager recording = RecordingFileManager.Instance;

        private static HeroControllerWriter? _instance;
        public static HeroControllerWriter Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new HeroControllerWriter();
                return _instance;
            }
        }

        private static FieldInfo[] stateFields = typeof(global::HeroControllerStates)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        public static HashSet<String> notLoggedStates = new() {
            "facingRight",
            "onGround",
            "falling",
            "touchingWall",
            "wallSliding",
            "transitioning",
            "lookingUp",
            "lookingDown",
            "recoilingRight",
            "recoilingLeft",
            "dashCooldown",
            "backDashCooldown",
            "wasOnGround",
        };

        public static HashSet<String> onlyTruthLoggedStates = new() {
            "attacking",
            "altAttack",
            "upAttacking",
            "downAttacking",
            "jumping",
            "wallJumping",
            "doubleJumping",
            "shadowDashing",
            "dashing",
            "bouncing",
            "freezeCharge", // this always becomes false in the next frame, even when it takes longer
            "superDashing", // this always becomes false in the next frame. Is false while super dashing
        };

        // default all states false, so no writing at beginning of game.
        private Dictionary<string, bool> previousHeroControllerStates = stateFields
            .ToDictionary(it => it.Name, it => false);

        internal void InitializeRun() {
            previousHeroControllerStates.Clear();

            foreach (var stateField in stateFields) {
                previousHeroControllerStates[stateField.Name] = false;
            }
        }

        internal void SetupHooks() {
            //On.HeroControllerStates.SetState += HeroControllerStates_SetState;
        }

        public void WriteChangedStates(long unixMillis) {
            foreach (var field in stateFields) {
                var currentVal = (field.GetValue(HeroController.instance.cState) as bool?).GetValueOrDefault();
                WriteStateIfChanged(field.Name, currentVal, unixMillis);
            }
        }

        //private void HeroControllerStates_SetState(On.HeroControllerStates.orig_SetState orig, global::HeroControllerStates self, string stateName, bool value) {
        //    if (self != HeroController.instance.cState) return;
        //    WriteStateIfChanged(stateName, value);
        //}

        private void WriteStateIfChanged(string stateName, bool value, long unixMills) {
            if (previousHeroControllerStates.TryGetValue(stateName, out var previous) && previous == value) {
                return;
            }
            previousHeroControllerStates[stateName] = value;

            if (!value && onlyTruthLoggedStates.Contains(stateName)) return;
            if (notLoggedStates.Contains(stateName)) return;

            var hasShortName = HeroControllerStateInfos.stats.TryGetValue(stateName, out var statInfo);

            var prefixKey = hasShortName ? RecordingPrefixes.HERO_CONTROLLER_STATE_SHORTNAME + statInfo.shortCode : RecordingPrefixes.HERO_CONTROLLER_STATE_LONGNAME + stateName;

            // Log($"Write hero controller state {stateName} {value}");
            recording.WriteEntryPrefix(prefixKey, unixMillis: unixMills);
            recording.Write(value ? "1" : "0");
            recording.WriteNL();
        }
    }
}
