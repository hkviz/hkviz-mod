using System;
using BepInEx;
using HKViz.Shared;
using HKViz.Silk.Extraction;
using HKViz.Silk.Recording;
using HKViz.Silk.SaveData;
using Silksong.DataManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HKViz.Silk;

[BepInAutoPlugin(id: "org.hkviz.silk")]
[BepInDependency("org.silksong-modding.datamanager")]
public partial class HkVizSilkPlugin : BaseUnityPlugin, ISaveDataMod<SaveDataRun> {
    private Extractor _extractor;

    private ServerApi _serverApi;

    private RunWriter? CurrentRunWriter { get; set; }
    
    private void Awake() {
        _serverApi = new ServerApi(Logger.LogInfo);
        
        _extractor = new Extractor(Logger);
        SceneManager.sceneLoaded += HandleSceneLoaded;

        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
    }

    private void Update() {
        // TODO dont compile in release
        if (Input.GetKeyDown(KeyCode.F8)) {
            Logger.LogInfo("Data extraction triggered");
            _extractor.Extract();
        }
        
        CurrentRunWriter?.Update();
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name is "Menu_Title") { // or "Quit_To_Menu") {
            CurrentRunWriter?.Close();
            CurrentRunWriter = null;
            // TODO upload retry + if not just did (since 2 scenes?)
            // UploadManager.Instance.RetryFailedUploads();
            return;
        }

        CurrentRunWriter?.HandleSceneChange(scene, mode);
    }

    public SaveDataRun? SaveData {
        get {
            var currentRunWriter = CurrentRunWriter;
            if (currentRunWriter == null) return null;
            return new SaveDataRun(
                localRunId: currentRunWriter.LocalRunId,
                nextRunPart: currentRunWriter.GetNextRunPart()
            );
        }
        set {
            CurrentRunWriter?.Close();
            string localRunId = value == null ? Guid.NewGuid().ToString() : value.LocalRunId;
            long nextRunPart = value?.NextRunPart ?? 0L;
            CurrentRunWriter = new RunWriter(localRunId, nextRunPart, Logger);
        }
    }
}
