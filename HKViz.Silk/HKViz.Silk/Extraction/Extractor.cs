using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class Extractor {
    private readonly ExtractionFiles _extractionFiles;
    private readonly MapExtraction _mapExtractor;
    private readonly LocalizationExtraction _localizationExtraction;

    public Extractor(ManualLogSource logger) {
        _extractionFiles = new ExtractionFiles(logger);
        _localizationExtraction = new LocalizationExtraction(_extractionFiles);
        _mapExtractor = new MapExtraction(_extractionFiles, _localizationExtraction, logger);
    }
    
    public void Extract() {
        _mapExtractor.Extract();
        _localizationExtraction.Extract();
    }
}
