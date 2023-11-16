using AnalyticsRecorder.Converters;
using MapChanger.MonoBehaviours;
using Modding;
using Modding.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AnalyticsRecorder {
    [System.Serializable]
    internal class PlayerDataField {
        public string name;
        public string type;
        public string shortCode;
        public string defaultValue;
        public bool notLogged;
    }

    internal class PlayerDataExport : Loggable {


        private static PlayerDataExport? _instance;
        public static PlayerDataExport Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new PlayerDataExport();
                return _instance;
            }
        }


        public void Export() {
            Log("Started playerData export");
            // creating a fake new playerdata object to get initial values
            PlayerData fakePlayerData = (PlayerData)Activator.CreateInstance(typeof(PlayerData), true);
            MethodInfo setupNewPlayerData = typeof(PlayerData).GetMethod("SetupNewPlayerData", BindingFlags.NonPublic | BindingFlags.Instance);
            setupNewPlayerData.Invoke(fakePlayerData, new object[0]);


            var fields = typeof(PlayerData)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Select((it, index) => new PlayerDataField {
                    name = it.Name,
                    type = it.FieldType.Name,
                    shortCode = Base36Converter.ConvertTo(index + 1),
                    defaultValue = RecordingSerializer.Instance.serializeUntyped(it.GetValue(fakePlayerData)),
                    notLogged = PlayerDataWriter.notLoggedFields.Contains(it.Name),
                })
                .ToDictionary(it => it.name, it => it);


            var json = Json.ToString(fields);
            using (var writer = new StreamWriter(StoragePaths.GetUserFilePath("player-data-export.txt"))) {
                writer.Write(json);
            }

            using (var writer = new StreamWriter(StoragePaths.GetUserFilePath("player-data-export-cs.txt"))) {
                foreach(var field in fields) {
                    writer.WriteLine($$"""
                        ["{{field.Key}}"] = new PlayerDataField {
                            name = "{{field.Value.name}}",    
                            type = "{{field.Value.type}}",
                            shortCode = "{{field.Value.shortCode}}",
                            defaultValue = "{{field.Value.defaultValue}}",
                            notLogged = {{field.Value.notLogged.ToString().ToLower()}}},
                        """);
                }
            }


            Log("Finished playerData export");
        }
    }
}
