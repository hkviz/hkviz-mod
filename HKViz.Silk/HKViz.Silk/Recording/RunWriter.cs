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

    // Call before starting the async restore point fetch, and EndBackfill() once it's resolved
    // (whether or not it found anything to backfill) - this keeps Close() from tearing down the
    // file out from under an in-flight backfill if the player quits to menu (or starts a new game)
    // while the fetch is still running. Close() gets deferred instead of skipped, so a quick exit
    // never permanently loses the restore point history - it's the only chance we get at it.
    public void BeginBackfill() {
        runFiles.BackfillPending = true;
    }

    public void EndBackfill() {
        runFiles.BackfillPending = false;
        if (_closeRequested) {
            CloseNow();
        }
    }

    // Backfills history for a save that's being recorded for the first time. Called asynchronously,
    // whenever the game's restore point fetch resolves - by that point live recording (this
    // RunWriter, including playerDataWriter/sceneDataWriter above) is typically already running, so
    // this must not touch their previous-value caches: doing so would make the next live diff
    // compare against a restore point's historical values instead of the actual last-seen live
    // values. Each restore point is instead written (oldest first, as its own bracketed frame)
    // through throwaway PlayerDataWriter/SceneDataWriter instances that only exist for this call -
    // they still diff restore points against each other (for file size), just never against the
    // live writers. Physical position in the file doesn't matter: the parser reconstructs
    // chronological order from the RestorePointStart/Finish brackets.
    // ResetNonSerializableFields/SetupExistingPlayerData mirror the same calls the game itself makes
    // on any loaded save (GameManager.SetLoadedGameData) to fix up legacy/missing fields on
    // older-schema PlayerData - they're plain instance methods, not tied to PlayerData.instance, so
    // no global singleton needs to be touched here.
    // writeLiveRuntimeState: false skips the heroState_*/gameManager_GameState fields, since those
    // reflect live HeroController/GameManager singletons rather than anything in the restore point's
    // PlayerData - writing them here would just repeat whatever the game is doing right now (not the
    // historical state) on every backfilled frame, for no benefit.
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
