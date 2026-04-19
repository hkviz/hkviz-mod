using System.Linq;
using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class EnemyJournalExtraction(ExtractionFiles extractionFiles, LocalizationExtraction localizationExtraction, ManualLogSource logger) {
    private void Log(string message) {
        logger.LogInfo(message);
    }

    public EnemyJournalExportData? Extract() {
        Log("Started enemy journal extraction.");

        var enemies = EnemyJournalManager.GetAllEnemies();
        EnemyJournalExportData data = new() {
            All = [],
        };

        foreach (EnemyJournalRecord record in enemies) {
            if (record == null) {
                continue;
            }

            string recordId = record.name;

            string? displayName = null;
            if (!string.IsNullOrEmpty(record.DisplayName.Sheet) && !string.IsNullOrEmpty(record.DisplayName.Key)) {
                displayName = localizationExtraction.RequestExport(record.DisplayName);
            }

            string? description = null;
            if (!string.IsNullOrEmpty(record.Description.Sheet) && !string.IsNullOrEmpty(record.Description.Key)) {
                description = localizationExtraction.RequestExport(record.Description);
            }

            string? notes = null;
            if (!string.IsNullOrEmpty(record.Notes.Sheet) && !string.IsNullOrEmpty(record.Notes.Key)) {
                notes = localizationExtraction.RequestExport(record.Notes);
            }

            var iconSprite = record.IconSprite;
            var enemySprite = record.EnemySprite;

            EnemyJournalData enemyData = new() {
                Id = recordId, // same as record.name
                DisplayName = displayName,
                Description = description,
                Notes = notes,
                RecordType = record.RecordType.ToString(),
                IconSprite = iconSprite.ToSpriteInfoSafe(logger, $"EnemyJournal:{recordId}:IconSprite"),
                EnemySprite = enemySprite.ToSpriteInfoSafe(logger, $"EnemyJournal:{recordId}:EnemySprite"),
                KillsRequired = record.KillsRequired,
                IsAlwaysUnlocked = record.IsAlwaysUnlocked,
                IsRequiredForCompletion = record.IsRequiredForCompletion,
                CompleteOthers = record.CompleteOthers?
                    .Where(other => other != null)
                    .Select(other => other.name)
                    .ToList(),
            };

            data.All.Add(enemyData);
        }

        extractionFiles.ExportJson("enemy-journal-export.json", data);
        Log($"Finished enemy journal extraction. Exported {data.All.Count} enemy entries.");

        return data;
    }
}

