using System;
using System.IO;
using BepInEx.Logging;
using GlobalEnums;
using HKViz.Shared;
using HKViz.Shared.Upload;
using HKViz.Silk.GameData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HKViz.Silk.Recording;

public class RunFiles(string localRunId, long currentRunPart, UploadManager uploadManager, ManualLogSource logger) {
    public string LocalRunId { get; } = localRunId;
    public long CurrentRunPart { get; private set; } = currentRunPart;
    public long currentFileFirstUnixSeconds { get; private set; } = DateTimeUtils.GetUnixSeconds();

    private const float SwitchFileAfterSeconds = 60f * 10L; // 5 minutes
    
    private float _lastPartCreatedTime = 0f;
    private bool _isClosed = false;
    
    private BinaryWriter? _writer;
    
    public void NextFileIfNeeded() {
        if (_isClosed) return;
        if (_writer == null || Time.unscaledTime - _lastPartCreatedTime > SwitchFileAfterSeconds) {
            NextFile();
        }
    }
    
    public float NextPartInSeconds => SwitchFileAfterSeconds - (Time.unscaledTime - _lastPartCreatedTime);

    public void NextFile() {
        if (_isClosed) return;
        var previousPartFirstUnixMillis = currentFileFirstUnixSeconds;
        var previousRunPart = CurrentRunPart;
        var oldWriter = _writer;
        if (oldWriter != null) {
            CurrentRunPart++;
        }
        currentFileFirstUnixSeconds = DateTimeUtils.GetUnixSeconds();
        var newPath = RunFilePaths.GetRecordingPath(LocalRunId, CurrentRunPart, GameManager.instance.profileID);
        while (File.Exists(newPath)) {
            CurrentRunPart++;
            newPath = RunFilePaths.GetRecordingPath(LocalRunId, CurrentRunPart, GameManager.instance.profileID);
        }
        _writer = new BinaryWriter(File.Open(newPath, FileMode.Create, FileAccess.Write, FileShare.Read));
        _lastPartCreatedTime = Time.unscaledTime;
        // TODO add new file to global storage of known files
        //  even before upload. so upload can happen after crash
        FinishFile(oldWriter, previousRunPart, previousPartFirstUnixMillis);
    }

    public void Close() {
        if (_isClosed) return;
        _isClosed = true;
        FinishFile(_writer, CurrentRunPart, currentFileFirstUnixSeconds);
        _writer = null;
    }

    private void FinishFile(BinaryWriter? writer, long partNumber, long partFirstUnixMillis) {
        if (writer == null) {
            return;
        }
        writer.Close();

        // TODO should use logic like Hollow Knight, where previous value from writer is loaded
        // string? pd(string key) {
            // var hasValue = PlayerDataWriter.Instance.previousPlayerData.TryGetValue(key, out var value);
            //return hasValue ? value : null;
        // }
        
        uploadManager.QueueFile(new UploadQueueEntry {
            localRunId = LocalRunId,
            partNumber = (int)partNumber,
            profileId = GameManager.instance.profileID,

            hkVersion = PlayerData.instance.version,
            playTime = PlayerData.instance.playTime,
            maxHealth = PlayerData.instance.maxHealth,
            mpReserveMax = PlayerData.instance.silkSpoolParts,
            geo = PlayerData.instance.geo,
            dreamOrbs = PlayerData.instance.ShellShards,
            permadeathMode = PlayerData.instance.permadeathMode switch {
                PermadeathModes.Off => 0,
                PermadeathModes.On => 1,
                PermadeathModes.Dead => 2,
                _ => -1,
            },
            mapZone = PlayerData.instance.mapZone.ToString(),
            killedHollowKnight = false,
            killedFinalBoss = false,
            killedVoidIdol = false,
            completionPercentage = Mathf.RoundToInt(PlayerData.instance.completionPercentage),
            unlockedCompletionRate = PlayerData.instance.ConstructedFarsight,

            dreamNailUpgraded = false,
            lastScene = "", // TODO

            firstUnixSeconds = partFirstUnixMillis,
            lastUnixSeconds = DateTimeUtils.GetUnixSeconds(),
        });
    }

    public void WriteSceneChange(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write scene change but writer is null");
            return;
        }
        var sceneName = scene.name;
        var hasId = SilkSongScenes.Scenes.TryGetValue(sceneName.ToLowerInvariant(), out var id);

        if (hasId) {
            writer.Write(mode == LoadSceneMode.Additive ? WriteEntryType.SceneChangeAddShort : WriteEntryType.SceneChangeSingleShort);
            writer.Write(id);
        } else {
            writer.Write(mode == LoadSceneMode.Additive ? WriteEntryType.SceneChangeAddLong : WriteEntryType.SceneChangeSingleLong);
            writer.WriteStringCompat(sceneName);
        }
    }

    public void WriteHeroLocation(Vector2 pos) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write hero location but writer is null");
            return;
        }
        
        writer.Write(WriteEntryType.HeroLocation);
        writer.WriteVector2(pos);
    }
}
