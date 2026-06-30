using System;
using System.Collections.Generic;
using BepInEx.Logging;

namespace HKViz.Silk.Extraction;

public class SceneDataExtraction(ExtractionFiles extractionFiles, ManualLogSource logger) {
    public void Extract() {
        var data = new SceneDataExportData();

        var instance = SceneData._instance;

        AppendSceneDataEntries(instance.persistentBools, data.Bool);
        AppendSceneDataEntries(instance.persistentInts, data.Int);
        AppendSceneDataEntries(instance.geoRocks, data.GeoRock);

        extractionFiles.ExportJson("scene-data-export.json", data);
        logger.LogDebug("Finished SceneData extraction. ");
    }

    private void AppendSceneDataEntries<TValue, TSerialize>(
        SceneData.PersistentItemDataCollection<TValue, TSerialize> data,
        List<SceneDataFieldData> list
    ) where TSerialize : SceneData.SerializableItemData<TValue>, new() {
        foreach (var (scene, sceneData) in data.scenes) {
            foreach (var (key, value) in sceneData) {
                list.Add(new SceneDataFieldData {
                    SceneName = scene,
                    Id = key,
                });
            }
        }
    }
}
