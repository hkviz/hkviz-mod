using System;
using BepInEx;
using HKViz.Shared.Recording;
using HKViz.Silk.Extraction;
using HKViz.Silk.Recording;
using HKViz.Silk.SaveData;
using HKViz.Silk.UI;
using HKViz.Silk.Upload;
using Silksong.DataManager;
using Silksong.ModMenu.Plugin;
using Silksong.ModMenu.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;

namespace HKViz.Silk;

[BepInAutoPlugin(id: "org.hkviz.silk")]
[BepInDependency("org.silksong-modding.datamanager")]
[BepInDependency("org.silksong-modding.i18n")]
[BepInDependency(Silksong.ModMenu.ModMenuPlugin.Id)]
public partial class HkVizSilkPlugin : BaseUnityPlugin, ISaveDataMod<SaveDataRun>, IGlobalDataMod<SaveDataGlobal>, IRecordingManager, IModMenuCustomMenu {
    private Extractor _extractor;

    private RunWriter? CurrentRunWriter { get; set; }
    private HkVizMenuScreen menuScreen;
    private HkVizSharedInstances sharedInstances;
    private SilkUploadManager uploadManager = null!;

    private void Awake() {
        try {
            sharedInstances = new HkVizSharedInstances(
                new SilkLogger(Logger),
                (serverApi, authManager, logger) => uploadManager = new SilkUploadManager(serverApi, authManager, logger),
                this
            );
            HkVizSharedInstances.CreateInstance(sharedInstances);
            HkVizSharedInstances.Instance!.Initialize();
            menuScreen = new HkVizMenuScreen(sharedInstances.authManager, uploadManager);

            // I think I should not have to call this given auto plugin
            // but does not run otherwise.
            new Harmony(Id).PatchAll();

            _extractor = new Extractor(Logger);
            SceneManager.sceneLoaded += HandleSceneLoaded;

            Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
        } catch (Exception ex) {
            Logger.LogError($"Error during {Name} ({Id}) initialization:");
            Logger.LogError(ex.ToString());
        }
    }

    private void Update() {
        // TODO dont compile in release
        if (Input.GetKeyDown(KeyCode.F8)) {
            if (SceneManager.GetActiveScene().name == "Menu_Title") {
                Logger.LogInfo("Menu save state extraction triggered");
                _extractor.ExtractMenuSaveSlots();
            } else {
                Logger.LogInfo("Ingame map view extraction triggered");
                _extractor.ExtractIngameMap();
            }
            
        } else if (Input.GetKeyDown(KeyCode.F8) && Input.GetKey(KeyCode.RightArrow)) {
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
                localRunId: currentRunWriter.LocalRunId.ToString(),
                nextRunPart: currentRunWriter.GetNextRunPart()
            );
        }
        set {
            CurrentRunWriter?.Close();
            Guid localRunId = value == null ? Guid.NewGuid() : Guid.Parse(value.LocalRunId);
            long nextRunPart = value?.NextRunPart ?? 0L;
            CurrentRunWriter = new RunWriter(localRunId, nextRunPart, uploadManager, Logger);
        }
    }

    public bool IsRecording => CurrentRunWriter != null;
    public float NextPartInSeconds => CurrentRunWriter?.NextPartInSeconds ?? 0f;
    public string ModMenuName() {
        return "HKViz";
    }

    public AbstractMenuScreen BuildCustomMenu() {
        return menuScreen.BuildCustomMenu();
    }

    public SaveDataGlobal? GlobalData {
        get =>
            new(
                authId: sharedInstances.authManager.AuthId,
                userName: sharedInstances.authManager.UserName,
                autoUpload: true,
                showLoginButtonInMainMenu: true,
                queuedUploadFiles: uploadManager.queuedFiles,
                failedUploadFiles: uploadManager.failedUploads,
                finishedUploadFiles: uploadManager.finishedUploadFiles
            );
        set {
            sharedInstances.authManager.InitializeFromGlobalSettings(value?.authId, value?.userName);
            uploadManager.AddUploadEntries(
                value?.queuedUploadFiles,
                value?.failedUploadFiles,
                value?.finishedUploadFiles
            );
        }
    }
}
