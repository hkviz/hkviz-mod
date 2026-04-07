using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class Extractor(ManualLogSource logger) {
    private readonly MapExtraction _mapExtractor = new(logger);

    public void Extract() {
        _mapExtractor.Extract();
    }
}
