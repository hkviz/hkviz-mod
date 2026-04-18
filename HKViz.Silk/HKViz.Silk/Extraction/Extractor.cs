using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class Extractor {
    private readonly ExtractionFiles _extractionFiles;
    private readonly MapExtraction _mapExtractor;
    private readonly LocalizationExtraction _localizationExtraction;
    private readonly SaveSlotBackgroundsExtraction _saveSlotBackgroundsExtractor;
    private readonly ToolCrestExtraction _toolCrestExtraction;
    private readonly ToolItemExtraction _toolItemExtraction;

    public Extractor(ManualLogSource logger) {
        _extractionFiles = new ExtractionFiles(logger);
        _localizationExtraction = new LocalizationExtraction(_extractionFiles, logger);
        _mapExtractor = new MapExtraction(_extractionFiles, _localizationExtraction, logger);
        _saveSlotBackgroundsExtractor = new SaveSlotBackgroundsExtraction(_extractionFiles, _localizationExtraction, logger);
        _toolCrestExtraction = new ToolCrestExtraction(_extractionFiles, _localizationExtraction, logger);
        _toolItemExtraction = new ToolItemExtraction(_extractionFiles, _localizationExtraction, logger);
    }
    

    public void ExtractIngameMap() {
        _mapExtractor.Extract();
        _toolCrestExtraction.Extract();
        _toolItemExtraction.Extract();
        _localizationExtraction.Extract();
    }

    public void ExtractMenuSaveSlots() {
        _saveSlotBackgroundsExtractor.Extract();
        _localizationExtraction.Extract();
    }
}
