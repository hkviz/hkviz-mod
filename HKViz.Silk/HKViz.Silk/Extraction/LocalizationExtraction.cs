using System;
using System.Collections.Generic;
using GlobalEnums;
using TeamCherry.Localization;

namespace HKViz.Silk.Extraction;

public class LocalizedStringExportRequest(string sheet, string key) {
    public string sheet = sheet;
    public string key = key;
}

public class LocalizationExtraction(ExtractionFiles extractionFiles) {
    public static string SHEET_MAP_ZONES = "Map Zones";
    
    private static List<T> GetEnumList<T>()
    {
        T[] array = (T[])Enum.GetValues(typeof(T));
        List<T> list = new List<T>(array);
        return list;
    }

    private List<LocalizedStringExportRequest> queuedRequests = [];
    
    public string RequestExport(string sheet, string key) {
        queuedRequests.Add(new LocalizedStringExportRequest(sheet, key));
        return sheet + "." + key;
    }
    
    public void Extract() {
        foreach (MapZone zone in GetEnumList<MapZone>()) {
            RequestExport(SHEET_MAP_ZONES, zone.ToString());
        }
        

        var previousLang = Language.CurrentLanguage();
        foreach (string language in Language.GetLanguages()) {
            var langTranslations = new Dictionary<string, string>();
            Language.SwitchLanguage(language);
            foreach (var request in queuedRequests) {
                langTranslations[request.sheet + "." + request.key] = Language.Get(request.key, request.sheet);
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
        queuedRequests.Clear();
    }
}
