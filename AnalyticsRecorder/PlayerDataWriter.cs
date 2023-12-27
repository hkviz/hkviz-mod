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

namespace AnalyticsRecorder {
    internal record struct PlayerDataQueueValue(
        string fieldName,
        string valueString,
        float timestamp,
        // unix millis are the same for all checked values within a frame. (Even if the unix millis would change within the check)
        // this allows us to easily find all changes within a frame in the data
        long unixMillis,
        bool writeIncremental
    );

    internal class PlayerDataWriter: Loggable {
        // a queue might creates some problems when some values change with each other
        // since the other value might not change with the same timeframe in the file.
        // therefore I have set the delay to zero for now. 
        private static readonly float WRITE_QUEUE_DELAY_SECONDS = 0;  // .1f;

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
        private List<PlayerDataQueueValue> playerDataQueueValues = new List<PlayerDataQueueValue>();

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
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
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
            // write all remaining queue values when recording stops
            while (playerDataQueueValues.Count > 0) {
                WriteFirstFromQueue();
            }
        }

        private void WriteFirstFromQueue() {
            var d = playerDataQueueValues[0];

            var hasShortName = PlayerDataFieldInfos.fields.TryGetValue(d.fieldName, out var fieldInfo);

            var prefixKey = hasShortName ? RecordingPrefixes.PLAYER_DATA_SHORTNAME + fieldInfo.shortCode : RecordingPrefixes.PLAYER_DATA_LONGNAME + d.fieldName;

            // Log("Write pd " + d.fieldName + ": " + d.valueString);
            playerDataQueueValues.RemoveAt(0);
            recording.WriteEntryPrefix(prefixKey, unixMillis: d.unixMillis);
            recording.Write(d.valueString);
            recording.WriteNL();
        }

        private void ModHooks_HeroUpdateHook() {
            var writeUntil = Time.unscaledTime - WRITE_QUEUE_DELAY_SECONDS;
            while (playerDataQueueValues.Count > 0 && playerDataQueueValues[0].timestamp <= writeUntil) {
                WriteFirstFromQueue();
            }

            if (!isRecording) return;
            CheckAllPlayerData();
        }

        private void CheckAllPlayerData() {
            var unixMillis = recording.GetUnixMillis();
            foreach (var field in playerDataFields) {
                var currentValue = field.GetValue(PlayerData.instance);
                QueuePlayerData(
                    self: PlayerData.instance, 
                    fieldName: field.Name,
                    valueString: serializer.serializeUntyped(currentValue),
                    unixMillis: unixMillis,
                    writeIncremental: currentValue is IList
                );
            }
        }

        private void QueuePlayerData(
            PlayerData self, 
            string fieldName, 
            string valueString,
            long unixMillis,
            bool writeIncremental = false
        ) {
            if (self != PlayerData.instance) return;
            if (notLoggedFields.Contains(fieldName)) return;

            // Log("Queue pd" + fieldName + ": " + valueString);
            if(previousPlayerData.TryGetValue(fieldName, out string previousValue) && previousValue == valueString) {
                // already wrote current value. No need to write again
                return;
            }

            // Log("Write into dict");
            previousPlayerData[fieldName] = valueString;

            if (writeIncremental && valueString is not null && previousValue is not null && previousValue != "") {
                valueString = valueString.Replace(previousValue, "::");
            }

            for (int i = 0; i < playerDataQueueValues.Count; i++) {
                var d = playerDataQueueValues[i];
                if (d.fieldName == fieldName) {
                    playerDataQueueValues.RemoveAt(i);
                    break; // since queue can not contain duplicates, break here is ok
                }
            }
            playerDataQueueValues.Add(new PlayerDataQueueValue(
                fieldName: fieldName, 
                valueString: valueString, 
                timestamp: Time.unscaledTime,
                unixMillis: unixMillis,
                writeIncremental: writeIncremental
            ));
        }

        /*
        private void PlayerData_SetBool(On.PlayerData.orig_SetBool orig, PlayerData self, string boolName, bool value) {
            orig(self, boolName, value);
            QueuePlayerData(self, boolName, serializer.serialize(value));
        }

        private void PlayerData_SetFloat(On.PlayerData.orig_SetFloat orig, PlayerData self, string floatName, float value) {
            orig(self, floatName, value);
            QueuePlayerData(self, floatName, serializer.serialize(value));
        }
        private void PlayerData_SetInt(On.PlayerData.orig_SetInt orig, PlayerData self, string intName, int value) {
            orig(self, intName, value);
            QueuePlayerData(self, intName, serializer.serialize(value));
        }
        private void PlayerData_SetString(On.PlayerData.orig_SetString orig, PlayerData self, string stringName, string value) {
            orig(self, stringName, value);
            QueuePlayerData(self, stringName, serializer.serializeUntyped(value));
        }
        private void PlayerData_SetVector3(On.PlayerData.orig_SetVector3 orig, PlayerData self, string vectorName, UnityEngine.Vector3 value) {
            orig(self, vectorName, value);
            QueuePlayerData(self, vectorName, serializer.serialize(value));
        }
        */
    }
}
