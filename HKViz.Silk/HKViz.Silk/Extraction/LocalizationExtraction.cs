using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx.Logging;
using GlobalEnums;
using TeamCherry.Localization;
using Newtonsoft.Json;

namespace HKViz.Silk.Extraction;

public class LocalizationExtraction( ExtractionFiles extractionFiles, ManualLogSource logger) {
    public static string SHEET_MAP_ZONES = "Map Zones";
    private static string KEYS_PERSIST_FILE = "_localization_keys_keep.json";
    
    private static List<T> GetEnumList<T>()
    {
        T[] array = (T[])Enum.GetValues(typeof(T));
        List<T> list = new List<T>(array);
        return list;
    }

    private HashSet<string> exportKeys = [];
    
    private void Log(string message) {
        logger.LogInfo($"[LocalizationExtraction] {message}");
    }

    private void LoadPersistedKeys() {
        try {
            var outputPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "HKViz",
                KEYS_PERSIST_FILE
            );
            
            if (File.Exists(outputPath)) {
                var json = File.ReadAllText(outputPath);
                var keys = JsonConvert.DeserializeObject<List<string>>(json) ?? [];
                keys.ForEach(k => exportKeys.Add(k));
                Log($"Loaded {exportKeys.Count} persisted localization keys from {KEYS_PERSIST_FILE}");
            } else {
                Log($"No persisted keys file found at {outputPath}, starting fresh");
            }
        } catch (Exception ex) {
            // If loading fails, just start fresh
            logger.LogError($"Failed to load persisted keys, starting fresh: ");
            logger.LogError(ex.ToString());
        }
    }

    private void SavePersistedKeys() {
        try {
            var outputPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "HKViz",
                KEYS_PERSIST_FILE
            );
            
            var outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);
            
            var keys = exportKeys.OrderBy(k => k).ToList();
            var json = JsonConvert.SerializeObject(keys, Formatting.Indented);
            File.WriteAllText(outputPath, json);
            Log($"Saved {keys.Count} persisted localization keys to {KEYS_PERSIST_FILE}");
        } catch (Exception ex) {
            // Silently fail if we can't save the persist file
            Log($"Warning: Failed to save persisted keys: {ex.Message}");
        }
    }
    
    public string RequestExport(string sheet, string key) {
        var keyStr = sheet + "." + key;
        exportKeys.Add(keyStr);
        return keyStr;
    }

    public string RequestExport(LocalisedString s) {
        return RequestExport(s.Sheet, s.Key);
    }
    
    public void Extract() {
        Log("Starting localization extraction");
        
        // Load previously persisted keys
        LoadPersistedKeys();
        var keysBeforeQueuing = exportKeys.Count;
        
        foreach (MapZone zone in GetEnumList<MapZone>()) {
            RequestExport(SHEET_MAP_ZONES, zone.ToString());
        }
        
        Log($"Export keys grew from {keysBeforeQueuing} to {exportKeys.Count}");
        Log($"Exporting localization for {exportKeys.Count} total keys");

        var previousLang = Language.CurrentLanguage();
        foreach (string language in Language.GetLanguages()) {
            var langTranslations = new Dictionary<string, string>();
            Language.SwitchLanguage(language);
            foreach (var keyStr in exportKeys) {
                var parts = keyStr.Split('.');
                if (parts.Length == 2) {
                    var sheet = parts[0];
                    var key = parts[1];
                    langTranslations[keyStr] = Language.Get(key, sheet);
                }
            }
            extractionFiles.ExportJson($"localization-{language}.json", langTranslations);
            
            var langAllTranslations = new Dictionary<string, string>();
            foreach (var sheet in Language.GetSheets()) {
                foreach (var key in Language.GetKeys(sheet)) {
                    langAllTranslations[sheet + "." + key] = Language.Get(key, sheet);
                }
            }
            extractionFiles.ExportJson($"localization-all-{language}.json", langAllTranslations);
        }
        Language.SwitchLanguage(previousLang);
        
        // Save persisted keys for next run
        SavePersistedKeys();
    }
}
