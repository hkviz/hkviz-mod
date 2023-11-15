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


        public void ExportPlayerData() {
            Log("Started playerData export");
            var fields = typeof(PlayerData)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Select((it, index) => new PlayerDataField {
                    name = it.Name,
                    type = it.FieldType.Name,
                    shortCode = Base36Converter.ConvertTo(index+1),
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
                            shortCode = "{{field.Value.shortCode}}"
                        },
                        """);
                }
            }


            Log("Finished playerData export");
        }
    }
}
