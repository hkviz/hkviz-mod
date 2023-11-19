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
    internal class HeroControllerStat {
        public string name;
        public string shortCode;
        public bool notPartOfLog;
        public bool onlyTruthLogged;
    }

    internal class HeroControllerExport : Loggable {


        private static HeroControllerExport? _instance;
        public static HeroControllerExport Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new HeroControllerExport();
                return _instance;
            }
        }


        public void Export() {
            Log("Started hero controller export");
            var fields = typeof(global::HeroControllerStates)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Select((it, index) => new HeroControllerStat {
                    name = it.Name,
                    shortCode = Base36Converter.ConvertTo(index+1),
                    notPartOfLog = HeroControllerWriter.notLoggedStates.Contains(it.Name),
                    onlyTruthLogged = HeroControllerWriter.onlyTruthLoggedStates.Contains(it.Name),
                })
                .ToDictionary(it => it.name, it => it);

            var json = Json.Stringify(fields);
            using (var writer = new StreamWriter(StoragePaths.GetUserFilePath("hero-controller-export.txt"))) {
                writer.Write(json);
            }

            using (var writer = new StreamWriter(StoragePaths.GetUserFilePath("hero-controller-export-cs.txt"))) {
                foreach(var field in fields) {
                    writer.WriteLine($$"""
                        ["{{field.Key}}"] = new HeroControllerStat {
                            name = "{{field.Value.name}}",
                            shortCode = "{{field.Value.shortCode}}",
                            notPartOfLog = {{field.Value.notPartOfLog.ToString().ToLower()}},
                            onlyTruthLogged = {{field.Value.onlyTruthLogged.ToString().ToLower()}},
                        },
                        """);
                }
            }


            Log("Finished hero controller export");
        }
    }
}
