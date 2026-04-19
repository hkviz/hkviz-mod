using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class Extractor {
    private readonly MapExtraction _mapExtractor;
    private readonly LocalizationExtraction _localizationExtraction;
    private readonly SaveSlotBackgroundsExtraction _saveSlotBackgroundsExtractor;
    private readonly CollectableExtraction _collectableExtraction;
    private readonly CollectableRelicExtraction _collectableRelicExtraction;
    private readonly ToolCrestExtraction _toolCrestExtraction;
    private readonly EnemyJournalExtraction _enemyJournalExtraction;
    private readonly ToolItemExtraction _toolItemExtraction;
    private readonly QuestExtraction _questExtraction;
    private readonly MateriumExtraction _materiumExtraction;
    private readonly TransitionGateExtraction _transitionGateExtraction;

    public Extractor(ManualLogSource logger) {
        var extractionFiles = new ExtractionFiles(logger);
        _localizationExtraction = new LocalizationExtraction(extractionFiles, logger);
        _mapExtractor = new MapExtraction(extractionFiles, _localizationExtraction, logger);
        _saveSlotBackgroundsExtractor = new SaveSlotBackgroundsExtraction(extractionFiles, _localizationExtraction, logger);
        _collectableExtraction = new CollectableExtraction(extractionFiles, _localizationExtraction, logger);
        _collectableRelicExtraction = new CollectableRelicExtraction(extractionFiles, _localizationExtraction, logger);
        _toolCrestExtraction = new ToolCrestExtraction(extractionFiles, _localizationExtraction, logger);
        _enemyJournalExtraction = new EnemyJournalExtraction(extractionFiles, _localizationExtraction, logger);
        _toolItemExtraction = new ToolItemExtraction(extractionFiles, _localizationExtraction, logger);
        _questExtraction = new QuestExtraction(extractionFiles, _localizationExtraction, logger);
        _materiumExtraction = new MateriumExtraction(extractionFiles, _localizationExtraction, logger);
        _transitionGateExtraction = new TransitionGateExtraction(extractionFiles, logger);
    }
    

    public void ExtractIngameMap() {
        _mapExtractor.Extract();
        _collectableExtraction.Extract();
        _collectableRelicExtraction.Extract();
        _toolCrestExtraction.Extract();
        _toolItemExtraction.Extract();
        _enemyJournalExtraction.Extract();
        _questExtraction.Extract();
        _materiumExtraction.Extract();
        _transitionGateExtraction.Extract();
        _localizationExtraction.Extract();
    }

    public void ExtractMenuSaveSlots() {
        _saveSlotBackgroundsExtractor.Extract();
        _localizationExtraction.Extract();
    }
}
