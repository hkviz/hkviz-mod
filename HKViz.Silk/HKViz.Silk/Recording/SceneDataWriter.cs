using System;
using System.Collections.Generic;

namespace HKViz.Silk.Recording;

public class SceneDataWriter(RunFiles runFiles) {
    public void WriteAll() {
        Write(true);
    }

    public void WriteChanged() {
        Write(false);
    }

    private readonly Dictionary<string, Dictionary<string, bool>> _previousBool = [];
    private readonly Dictionary<string, Dictionary<string, int>> _previousInt = [];
    private readonly Dictionary<string, Dictionary<string, int>> _previousGeoRock = [];


    private void Write(bool all) {
        SceneData instance = SceneData._instance;
        if (instance == null) {
            return;
        }
        
        WriteCollection(instance.persistentBools, _previousBool, runFiles.WriteSceneDataBool, all);
        WriteCollection(instance.persistentInts, _previousInt, runFiles.WriteSceneDataInt, all);
        WriteCollection(instance.geoRocks, _previousGeoRock, runFiles.WriteSceneDataGeoRock, all);
    }
    
    private void WriteCollection<TValue, TSerialize>(
        SceneData.PersistentItemDataCollection<TValue, TSerialize> data,
        Dictionary<string, Dictionary<string, TValue>> previous,
        Action<string, string, TValue> write,
        bool all
    ) where TValue : IEquatable<TValue> where TSerialize : SceneData.SerializableItemData<TValue>, new() {
        foreach (var (scene, sceneData) in data.scenes) {
            var previousHasScene = previous.TryGetValue(scene, out var previousSceneData);
            if (!previousHasScene) {
                previousSceneData = new Dictionary<string, TValue>();
                previous[scene] = previousSceneData;
            }

            foreach (var (key, value) in sceneData) {
                var previousHasValue = previousSceneData!.TryGetValue(key, out var previousValue);
                if (all || !previousHasValue || !value.Value.Equals(previousValue)) {
                    previousSceneData[key] = value.Value;
                    write(scene, key, value.Value);
                }
            }
        }
        
    }
}