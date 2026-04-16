using System;
using System.Collections.Generic;
using System.IO;
using BepInEx.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace HKViz.Silk.Extraction;

public class ExtractionFiles(ManualLogSource logger) {

    public void ExportJson<T>(string fileName, T data) {
        var jsonSettings = new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Include,
            Converters = new List<JsonConverter> {
                new StringEnumConverter()
            },
            ContractResolver = new DefaultContractResolver {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
        var json = JsonConvert.SerializeObject(data, Formatting.Indented, jsonSettings);
        ExportText(fileName, json);
    }

    public void ExportText(string fileName, string text) {

        var outputPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "HKViz",
            fileName
        );

        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
            Directory.CreateDirectory(outputDir);

        File.WriteAllText(outputPath, text);
        logger.LogInfo($"Export saved to: {outputPath}");
    }
}