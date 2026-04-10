using TeamCherry.Localization;
using TeamCherry.Localization.Platform;

namespace HKViz.Silk;

public static class I18N {
    private const string Sheet = $"Mods.{HkVizSilkPlugin.Id}";

    public static LocalisedString Get(string key) {
        return new LocalisedString(Sheet, key);
    }
    
    public static string GetString(string key) {
        return Language.Get(key, Sheet);
    }
}
