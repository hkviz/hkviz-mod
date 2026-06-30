using System;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;
using BepInEx.Logging;
using Object = UnityEngine.Object;

namespace HKViz.Silk.Extraction;

public class SaveSlotBackgroundsExtraction(ExtractionFiles extractionFiles, LocalizationExtraction localizationExtraction, ManualLogSource logger) {
    
    private void Log(string message) {
        logger.LogInfo(message);
    }

    private void LogError(string message, Exception? ex = null) {
        logger.LogError(message);
        if (ex != null)
            logger.LogError(ex);
    }

    public SaveSlotBackgroundsData? Extract() {
        try {
            Log("Started SaveSlotBackgrounds extraction.");
            
            var saveSlotBackgrounds = Object.FindObjectOfType<SaveSlotBackgrounds>();
            if (saveSlotBackgrounds == null) {
                LogError("Could not find SaveSlotBackgrounds instance in the scene!");
                return null;
            }

            var data = new SaveSlotBackgroundsData {
                AreaBackgrounds = ExtractAreaBackgrounds(saveSlotBackgrounds),
                ExtraAreaBackgrounds = ExtractExtraAreaBackgrounds(saveSlotBackgrounds),
                BellhomeBackgrounds = ExtractBellhomeBackgrounds(saveSlotBackgrounds)
            };

            extractionFiles.ExportJson("save-slot-backgrounds.json", data);
            Log("Finished SaveSlotBackgrounds extraction.");
            
            return data;
        } catch (Exception ex) {
            LogError($"Fatal error during SaveSlotBackgrounds extraction: {ex.Message}", ex);
            return null;
        }
    }

    private Dictionary<string, AreaBackgroundData> ExtractAreaBackgrounds(SaveSlotBackgrounds saveSlotBackgrounds) {
        return ExtractAreaBackgroundsGeneric(saveSlotBackgrounds, "areaBackgrounds", typeof(MapZone));
    }

    private Dictionary<string, AreaBackgroundData> ExtractExtraAreaBackgrounds(SaveSlotBackgrounds saveSlotBackgrounds) {
        return ExtractAreaBackgroundsGeneric(saveSlotBackgrounds, "extraAreaBackgrounds", typeof(ExtraRestZones));
    }

    private Dictionary<string, AreaBackgroundData> ExtractAreaBackgroundsGeneric(SaveSlotBackgrounds saveSlotBackgrounds, string fieldName, Type enumType) {
        var result = new Dictionary<string, AreaBackgroundData>();
        
        try {
            var field = typeof(SaveSlotBackgrounds).GetField(fieldName, 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field == null) {
                LogError($"Could not find {fieldName} field");
                return result;
            }

            var backgrounds = field.GetValue(saveSlotBackgrounds) as SaveSlotBackgrounds.AreaBackground[];
            if (backgrounds == null || backgrounds.Length == 0) {
                Log($"No backgrounds found in {fieldName}");
                return result;
            }

            var enumValues = Enum.GetValues(enumType);
            for (int i = 0; i < backgrounds.Length && i < enumValues.Length; i++) {
                var bg = backgrounds[i];
                if (bg == null)
                    continue;

                var enumName = enumValues.GetValue(i)?.ToString();
                if (string.IsNullOrEmpty(enumName))
                    continue;
                
                string? nameOverrideKey = null;
                
                if (!string.IsNullOrEmpty(bg.NameOverride.Sheet) && !string.IsNullOrEmpty(bg.NameOverride.Key)) {
                    nameOverrideKey = localizationExtraction.RequestExport(bg.NameOverride);
                }
                
                var bgData = new AreaBackgroundData {
                    NameOverride = nameOverrideKey,
                    Act3OverlayOptOut = bg.Act3OverlayOptOut,
                    BackgroundImage = bg.BackgroundImage.ToSpriteInfoSafe(logger, $"SaveSlotBackgrounds:{fieldName}:{enumName}:BackgroundImage"),
                    Act3BackgroundImage = bg.Act3BackgroundImage.ToSpriteInfoSafe(logger, $"SaveSlotBackgrounds:{fieldName}:{enumName}:Act3BackgroundImage")
                };
                
                result[enumName] = bgData;
            }
        } catch (Exception ex) {
            LogError($"Error extracting backgrounds from {fieldName}: {ex.Message}", ex);
        }

        return result;
    }

    private Dictionary<string, SpriteInfo> ExtractBellhomeBackgrounds(SaveSlotBackgrounds saveSlotBackgrounds) {
        var result = new Dictionary<string, SpriteInfo>();
        
        try {
            var field = typeof(SaveSlotBackgrounds).GetField("bellhomeBackgrounds", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field == null) {
                LogError("Could not find bellhomeBackgrounds field");
                return result;
            }

            var sprites = field.GetValue(saveSlotBackgrounds) as Sprite[];
            if (sprites == null || sprites.Length == 0) {
                Log("No bellhome backgrounds found");
                return result;
            }

            var colours = (BellhomePaintColours[])Enum.GetValues(typeof(BellhomePaintColours));
            for (int i = 0; i < sprites.Length && i < colours.Length; i++) {
                var sprite = sprites[i];
                if (sprite == null)
                    continue;

                var colourName = colours[i].ToString();
                var spriteInfo = sprite.ToSpriteInfoSafe(logger, $"SaveSlotBackgrounds:Bellhome:{colourName}");
                if (spriteInfo != null) {
                    result[colourName] = spriteInfo;
                }
            }
        } catch (Exception ex) {
            LogError($"Error extracting bellhome backgrounds: {ex.Message}", ex);
        }

        return result;
    }
}

