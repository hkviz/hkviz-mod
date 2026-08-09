using System;
using System.Collections.Generic;
using BepInEx.Logging;
using GlobalEnums;
using HKViz.Shared;
using HKViz.Silk.Recording.Patch;
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
    private readonly SceneDataWriter sceneDataWriter;
    private readonly EnemyWriter enemyWriter;
    private readonly SilkUploadManager uploadManager;
    private readonly ManualLogSource logger;

    private readonly int profileId;

    private bool _closeRequested;

    public RunWriter(
        Guid localRunId,
        long nextRunPart,
        SilkUploadManager uploadManager,
        ManualLogSource logger
    ) {
        LocalRunId = localRunId;
        this.logger = logger;
        this.uploadManager = uploadManager;
        runFiles = new RunFiles(localRunId, nextRunPart, uploadManager, logger);
        heroLocationWriter = new HeroLocationWriter(runFiles);
        playerDataWriter = new PlayerDataWriter(runFiles);
        sceneDataWriter = new SceneDataWriter(runFiles);
        enemyWriter = new EnemyWriter(runFiles);
        runFiles.OnNewFileCreated += OnNewFileCreated;
        runFiles.OnFileFinished += OnFileFinished;

        profileId = GameManager.instance.profileID;

        GameMapLevelReadyPatch.OnGameMapLevelReady += OnGameMapLevelReady;

        // initializing writer indirect by faking a change to the current scene
        // bit odd, but we get the benefit that each file begins with a scene.
        HandleSceneChange(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnNewFileCreated(bool isFirstFile) {
        if (!isFirstFile) return;

        playerDataWriter.WriteAll();
        sceneDataWriter.WriteAll();
    }

    // bracket the async restore point fetch with these; keeps Close() from cutting off the file
    // (or rotating away) while a backfill is still in flight
    public void BeginBackfill() {
        runFiles.BackfillPending = true;
    }

    public void EndBackfill() {
        runFiles.BackfillPending = false;
        if (_closeRequested) {
            CloseNow();
        }
    }

    // Writes restore point history for a save recorded for the first time (oldest first, each
    // bracketed by RestorePointStart/Finish). Called async, once live recording is usually already
    // running, so:
    // - uses throwaway PlayerDataWriter/SceneDataWriter, never the live ones, so it can't clobber
    //   their diff caches (still diffs restore points against each other, for file size)
    // - writeLiveRuntimeState: false, since heroState_*/gameManager_GameState reflect the live game
    //   right now, not this historical snapshot
    // - Reset/SetupExistingPlayerData mirror what GameManager.SetLoadedGameData does for any loaded
    //   save; both are plain instance methods, no singleton involved
    public void BackfillRestorePoints(IReadOnlyList<FetchDataRequest<RestorePointData>.FetchResult> restorePoints) {
        if (isClosed) return;

        var backfillPlayerDataWriter = new PlayerDataWriter(runFiles);
        var backfillSceneDataWriter = new SceneDataWriter(runFiles);

        foreach (var result in restorePoints) {
            var restorePointData = result.loadedObject;
            var saveGameData = restorePointData?.saveGameData;
            var playerData = saveGameData?.playerData;
            if (restorePointData == null || saveGameData == null || playerData == null) continue;

            playerData.ResetNonSerializableFields();
            playerData.SetupExistingPlayerData();

            runFiles.Update();
            runFiles.WriteRestorePointStart((ushort)result.sourceData.number, restorePointData.date);
            backfillPlayerDataWriter.WriteChanged(true, playerData, writeLiveRuntimeState: false);
            backfillSceneDataWriter.WriteChanged(saveGameData.sceneData);
            runFiles.WriteRestorePointFinish();
        }

        logger.LogInfo($"Backfilled {restorePoints.Count} restore point(s), profile {profileId}");
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
        if (isClosed || _closeRequested) return;
        if (runFiles.BackfillPending) {
            _closeRequested = true;
            return;
        }
        CloseNow();
    }

    private void CloseNow() {
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
        bool frequencyUpdate = heroLocationWriter.Update();
        playerDataWriter.WriteChanged(frequencyUpdate);
        enemyWriter.WriteChanged(frequencyUpdate);
        
        sceneDataWriter.WriteChanged();
    }
}
