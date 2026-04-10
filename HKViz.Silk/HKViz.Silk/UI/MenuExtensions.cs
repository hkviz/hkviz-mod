using Silksong.ModMenu.Elements;
using UnityEngine.UI;

namespace HKViz.Silk.UI;

// copied from (and removed preview extension syntax :c)
// https://github.com/silksong-modding/Silksong.ModMenu/blob/main/Silksong.ModMenu/Elements/LocalizedTextExtensions.cs

public static class LocalizedTextExtensions
{
    public static LocalizedText GetLocalizedText(this Text self) {
        if (self.TryGetComponent<AutoLocalizeTextUI>(out var auto) && !auto.text.IsEmpty)
            return auto.text;

        return self.text;
    }
    
    public static void SetLocalizedText(this Text self, LocalizedText value) {
        if (value.IsLocalized) {
            if (!self.TryGetComponent<AutoLocalizeTextUI>(out var auto))
            {
                using (self.gameObject.TempInactive())
                {
                    auto = self.gameObject.AddComponent<AutoLocalizeTextUI>();
                    auto.textField = self;
                }
            }

            auto.text = value.Localized;
            auto.RefreshTextFromLocalization();
        } else {
            if (self.TryGetComponent<AutoLocalizeTextUI>(out var auto))
                UnityEngine.Object.Destroy(auto);
            self.text = value.Text;
        }
    }
}
