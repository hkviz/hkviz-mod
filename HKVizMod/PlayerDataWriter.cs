using MapChanger;
using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HKViz {

    internal class PlayerDataWriter: Loggable {
        private RecordingFileManager recording = RecordingFileManager.Instance;
        private RecordingSerializer serializer = RecordingSerializer.Instance;

        public static HashSet<string> notLoggedFields = new() {
            // potentially we could log charms, but remove the default ids since those are duplicated into their own vars, or not log the individual vars
            "equippedCharms",
            "mapZoneBools",
            "isInvincible",
            "hazardRespawnLocation",
            "hazardRespawnFacingRight",
            "previousDarkness",
            "disablePause",
            "environmentType",
        };

        private static PlayerDataWriter? _instance;
        public static PlayerDataWriter Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new PlayerDataWriter();
                return _instance;
            }
        }

        public Dictionary<string, string> previousPlayerData = new Dictionary<string, string>();

        private bool isRecording = false;

        private static FieldInfo[] playerDataFields = typeof(PlayerData)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        public void InitFromLocalSave(Dictionary<string, string> previousPlayerData) {
            // at the start of a game all previous values are initialized with the default
            // since those are just assumed when parsing. This saves storing the defaults
            // at each run file.
            var combinedFromDefaultsAndSave = PlayerDataFieldInfos.fields
                .Where(it => it.Value.name != "version" && !it.Value.notLogged)
                .ToDictionary(it => it.Value.name, it => it.Value.defaultValue);

            foreach (var field in previousPlayerData) {
                combinedFromDefaultsAndSave[field.Key] = field.Value;
            }
            previousPlayerData = combinedFromDefaultsAndSave;

            isRecording = true;
            Log("init from local save");
        }

        internal void SetupHooks() {
            recording.BeforeCloseLastSessionFile += Recording_BeforeWriterClose;

            // using Settings does sadly not work, since some vars are directly mutated. 
            // a bit to difficult to track. Now just chceking differences each frame.
            /*
            // TODO add for incrememts and similar
            // On.PlayerData.SetBenchRespawn_RespawnMarker_string_int
            // On.PlayerData.SetBenchRespawn_string_string_bool
            // On.PlayerData.SetBenchRespawn_string_string_int_bool
            On.PlayerData.SetBool += PlayerData_SetBool;
            // On.PlayerData.SetBoolSwappedArgs
            On.PlayerData.SetFloat += PlayerData_SetFloat;
            // On.PlayerData.SetFloatSwappedArgs
            // On.PlayerData.SetHazardRespawn_HazardRespawnMarker
            // On.PlayerData.SetHazardRespawn_Vector3_bool
            On.PlayerData.SetInt += PlayerData_SetInt;
            // On.PlayerData.SetIntSwappedArgs
            On.PlayerData.SetString += PlayerData_SetString;
            // On.PlayerData.SetStringSwappedArgs
            On.PlayerData.SetVector3 += PlayerData_SetVector3;
            // On.PlayerData.SetVector3SwappedArgs
            */
        }

        private void Recording_BeforeWriterClose() {
            isRecording = false;
        }

        public void WriteChangedValues(long unixMillis) {
            if (!isRecording) return;
            CheckAllPlayerData(unixMillis);
        }

        private void CheckAllPlayerData(long unixMillis) {
            foreach (var field in playerDataFields) {
                var currentValue = field.GetValue(PlayerData.instance);
                WritePlayerData(
                    self: PlayerData.instance, 
                    fieldName: field.Name,
                    valueString: serializer.serializeUntyped(currentValue),
                    unixMillis: unixMillis,
                    writeIncremental: currentValue is IList
                );
            }
        }

        private void WritePlayerData(
            PlayerData self, 
            string fieldName, 
            string valueString,
            long unixMillis,
            bool writeIncremental = false
        ) {
            if (self != PlayerData.instance) return;
            if (notLoggedFields.Contains(fieldName)) return;

            if(previousPlayerData.TryGetValue(fieldName, out string previousValue) && previousValue == valueString) {
                // already wrote current value. No need to write again
                return;
            }

            // Log("Write into dict");
            previousPlayerData[fieldName] = valueString;

            if (writeIncremental && valueString is not null && previousValue is not null && previousValue != "") {
                valueString = valueString.Replace(previousValue, "::");
            }

            // write:
            var hasShortName = PlayerDataFieldInfos.fields.TryGetValue(fieldName, out var fieldInfo);
            var prefixKey = hasShortName ? RecordingPrefixes.PLAYER_DATA_SHORTNAME + fieldInfo.shortCode : RecordingPrefixes.PLAYER_DATA_LONGNAME + fieldName;

            recording.WriteEntryPrefix(prefixKey, unixMillis: unixMillis);
            recording.Write(valueString);
            recording.WriteNL();
        }
    }
}
