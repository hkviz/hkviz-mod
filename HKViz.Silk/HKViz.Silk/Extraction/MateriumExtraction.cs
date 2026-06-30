using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class MateriumExtraction(ExtractionFiles extractionFiles, LocalizationExtraction localizationExtraction, ManualLogSource logger) {
    private void Log(string message) {
        logger.LogInfo(message);
    }

    public MateriumExportData Extract() {
        Log("Started materium extraction.");

        MateriumExportData data = new() {
            ConstructedMaterium = MateriumItemManager.ConstructedMaterium,
            All = [],
        };

        var manager = ManagerSingleton<MateriumItemManager>.Instance;
        if (!manager || manager.MasterList == null) {
            Log("MateriumItemManager or master list not found; exporting empty list.");
            extractionFiles.ExportJson("materium-export.json", data);
            return data;
        }

        foreach (MateriumItem item in manager.MasterList) {
            if (item == null) {
                continue;
            }

            string? displayNameKey = null;
            if (!string.IsNullOrEmpty(item.DisplayName.Sheet) && !string.IsNullOrEmpty(item.DisplayName.Key)) {
                displayNameKey = localizationExtraction.RequestExport(item.DisplayName);
            }

            string? descriptionKey = null;
            if (!string.IsNullOrEmpty(item.Description.Sheet) && !string.IsNullOrEmpty(item.Description.Key)) {
                descriptionKey = localizationExtraction.RequestExport(item.Description);
            }

            MateriumData itemData = new() {
                Id = item.name,
                RuntimeType = item.GetType().Name,
                DisplayNameKey = displayNameKey,
                DescriptionKey = descriptionKey,
                Icon = item.Icon.ToSpriteInfoSafe(logger, $"Materium:{item.name}:Icon"),
                IsRequiredForCompletion = item.IsRequiredForCompletion,
                IsCollected = item.IsCollected,
                IsSeen = item.IsSeen,
                IsVisibleInCollection = item.IsVisibleInCollection(),
            };

            data.All.Add(itemData);
        }

        extractionFiles.ExportJson("materium-export.json", data);
        Log($"Finished materium extraction. Exported {data.All.Count} materium entries.");

        return data;
    }
}

