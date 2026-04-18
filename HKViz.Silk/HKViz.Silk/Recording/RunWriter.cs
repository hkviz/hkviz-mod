using System;
using BepInEx.Logging;
using GlobalEnums;
using HKViz.Shared;
using HKViz.Silk.Upload;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HKViz.Silk.Recording;

public class RunWriter {
    public Guid LocalRunId { get; }

    private bool isClosed = false;
    private readonly RunFiles runFiles;
    private readonly HeroLocationWriter heroLocationWriter;
    private readonly PlayerDataWriter playerDataWriter;
    private readonly SilkUploadManager uploadManager;
    private readonly ManualLogSource logger;

    private readonly int profileId;
    
    public RunWriter(Guid localRunId, long nextRunPart, SilkUploadManager uploadManager, ManualLogSource logger) {
        LocalRunId = localRunId;
        this.logger = logger;
        this.uploadManager = uploadManager;
        runFiles = new RunFiles(localRunId, nextRunPart, uploadManager, logger);
        heroLocationWriter = new HeroLocationWriter(runFiles);
        playerDataWriter = new PlayerDataWriter(runFiles);
        runFiles.OnNewFileCreated += OnNewFileCreated;
        runFiles.OnFileFinished += OnFileFinished;
        
        profileId = GameManager.instance.profileID;

        GameMapLevelReadyPatch.OnGameMapLevelReady += OnGameMapLevelReady;
    }

    private void OnNewFileCreated(bool isFirstFile) {
        if (isFirstFile) {
            playerDataWriter.WriteAll();
        }
    }

    private void OnFileFinished((long runPart, long partFirstUnixMillis, string previousScene) file) {
        var completedEndings = playerDataWriter.PreviousValueCompletedEndings;
        uploadManager.QueueFile(new SilkUploadQueueEntry {
            localRunId = LocalRunId.ToString(),
            partNumber = (int)file.runPart,
            profileId = profileId,
    
            gameVersion = PlayerData.instance.version,

            // -- shared --
            playTime = PlayerData.instance.playTime,
            firstUnixSeconds = file.partFirstUnixMillis,
            lastUnixSeconds = DateTimeUtils.GetUnixSeconds(),

            unlockedCompletionRate = PlayerData.instance.ConstructedFarsight,
            completionPercentage = Mathf.RoundToInt(PlayerData.instance.completionPercentage),
            maxHealth = PlayerData.instance.maxHealth,
            geo = PlayerData.instance.geo,
            permadeathMode = PlayerData.instance.permadeathMode switch {
                PermadeathModes.Off => 0,
                PermadeathModes.On => 1,
                PermadeathModes.Dead => 2,
                _ => -1,
            },
            lastScene = file.previousScene,

            // -- game specific --
            endingAct2Regular = (PlayerData.instance.CompletedEndings & SaveSlotCompletionIcons.CompletionState.Act2Regular) != 0,
            endingAct2Cursed = (PlayerData.instance.CompletedEndings & SaveSlotCompletionIcons.CompletionState.Act2Cursed) != 0,
            endingAct2SoulSnare = (PlayerData.instance.CompletedEndings & SaveSlotCompletionIcons.CompletionState.Act2SoulSnare) != 0,
            endingAct3 = (PlayerData.instance.CompletedEndings & SaveSlotCompletionIcons.CompletionState.Act3Ending) != 0,
            isAct3 = PlayerData.instance.blackThreadWorld,
            
            mapZone = PlayerData.instance.mapZone.ToString(),
            shellShards = PlayerData.instance.ShellShards,
            silkSpoolParts = PlayerData.instance.silkSpoolParts,
            extraRestZones = (int)PlayerData.instance.extraRestZone,
            belltownHouseColour = (int)PlayerData.instance.BelltownHouseColour,

            currentCrestId = PlayerData.instance.CurrentCrestID,
        });
    }

    public void Close() {
        if (isClosed) return;
        isClosed = true;
        runFiles.Close();
        runFiles.OnNewFileCreated -= OnNewFileCreated;
        GameMapLevelReadyPatch.OnGameMapLevelReady -= OnGameMapLevelReady;
    }

    private void OnGameMapLevelReady(Vector2 size) {
        if (isClosed) return;
        runFiles.WriteSceneBoundary(size);
    }

    public void HandleSceneChange(Scene scene, LoadSceneMode mode) {
        logger.LogDebug($"Scene changed to {scene.name} with mode {mode}");
        // only called with scene != menu --> in-game scene
        runFiles.NextFileIfNeeded();
        runFiles.WriteSceneChange(scene, mode);
    }
    
    public float NextPartInSeconds => runFiles.NextPartInSeconds;

    public long GetNextRunPart() {
        return runFiles.CurrentRunPart + 1L;
    }

    public void Update() {
        if (isClosed) return;
        var gm = GameManager._instance;
        if (!gm || gm.IsGamePaused()) return;
        runFiles.Update();
        bool writeFrequentChangeFields = heroLocationWriter.Update();
        playerDataWriter.WriteChanged(writeFrequentChangeFields);
    }
}
