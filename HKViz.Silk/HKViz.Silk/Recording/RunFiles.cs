using System;
using System.IO;
using BepInEx.Logging;
using GlobalEnums;
using HKViz.Shared;
using HKViz.Shared.Upload;
using HKViz.Silk.GameData;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

namespace HKViz.Silk.Recording;

public class RunFiles(Guid localRunId, long currentRunPart, UploadManager uploadManager, ManualLogSource logger) {
    public event Action? OnNewFileCreated;

    public Guid LocalRunId { get; } = localRunId;
    public long CurrentRunPart { get; private set; } = currentRunPart;
    public long currentFileFirstUnixSeconds { get; private set; } = DateTimeUtils.GetUnixSeconds();

    private const float SwitchFileAfterSeconds = 60f * 10L; // 5 minutes
    
    private float _lastPartCreatedTime = 0f;
    private bool _isClosed = false;
    private bool _wroteTimeForUpdate = false;
    private long _lastWrittenTime = 0;
    
    private BinaryWriter? _writer;

    private const byte IntArrayModeFull = 0;
    private const byte IntArrayModeDelta = 1;
    private const byte StringCollectionModeFull = 0;
    private const byte StringCollectionModeDelta = 1;
    private const byte StringSetModeFull = 0;
    private const byte StringSetModeDelta = 1;

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
        var newPath = RunFilePaths.GetRecordingPath(LocalRunId, CurrentRunPart, GameManager.instance.profileID);
        while (File.Exists(newPath)) {
            CurrentRunPart++;
            newPath = RunFilePaths.GetRecordingPath(LocalRunId, CurrentRunPart, GameManager.instance.profileID);
        }
        _writer = new BinaryWriter(File.Open(newPath, FileMode.Create, FileAccess.Write, FileShare.Read));
        _lastPartCreatedTime = Time.unscaledTime;
        _lastWrittenTime = 0;
        _wroteTimeForUpdate = false;

        // header
        WriteHeader(_writer);
        currentFileFirstUnixSeconds = DateTimeUtils.GetUnixSeconds();
        WriteTimeIfChanged(_writer);
        OnNewFileCreated?.Invoke();


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
            localRunId = LocalRunId.ToString(),
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

    public void WriteHeader(BinaryWriter writer) {
        writer.Write(HKVizSilkConstants.FileVersion);
        writer.Write(LocalRunId.ToByteArray());
        writer.Write(CurrentRunPart);
    }

    public void Update() {
        _wroteTimeForUpdate = false;
    }

    public void WriteTimeIfChanged(BinaryWriter writer) {
        if (_wroteTimeForUpdate) {
            // already written
            return;
        }

        long timestamp = DateTimeUtils.GetUnixMillis();

        long deltaMillis = timestamp - _lastWrittenTime;
        switch (deltaMillis) {
            case 0:
                // nothing to write
                break;
            case < 0:
                // decreased timestamp, should rarely happen
                writer.WriteEntryType(WriteEntryType.TimestampBackwards);
                writer.Write(timestamp);
                break;
            case <= byte.MaxValue:
                writer.WriteEntryType(WriteEntryType.TimestampAddByte);
                writer.Write((byte)deltaMillis);
                break;
            case <= ushort.MaxValue:
                // short timestamp
                writer.WriteEntryType(WriteEntryType.TimestampAddShort);
                writer.Write((ushort)deltaMillis);
                break;
            default:
                // full timestamp
                writer.WriteEntryType(WriteEntryType.TimestampFull);
                writer.Write(timestamp);
                break;
        }
        _lastWrittenTime = timestamp;
        _wroteTimeForUpdate = true;
    }

    public void WriteSceneChange(Scene scene, LoadSceneMode mode) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write scene change but writer is null");
            return;
        }
        var sceneName = scene.name;
        var hasId = SilkSongScenes.Scenes.TryGetValue(sceneName.ToLowerInvariant(), out var id);

        WriteTimeIfChanged(writer);
        if (hasId) {
            writer.WriteEntryType(mode == LoadSceneMode.Additive ? WriteEntryType.SceneChangeAddShort : WriteEntryType.SceneChangeSingleShort);
            writer.Write(id);
        } else {
            writer.WriteEntryType(mode == LoadSceneMode.Additive ? WriteEntryType.SceneChangeAddLong : WriteEntryType.SceneChangeSingleLong);
            writer.WriteStringCompat(sceneName);
        }
    }

    public void WriteHeroLocation(Vector2 pos) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write hero location but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.HeroLocation);
        writer.WriteVector2(pos);
        
    }

    public void WriteSceneBoundary(Vector2 size) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write scene boundary but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.SceneBoundary);
        writer.WriteVector2(size);
    }

    public void WritePlayerDataBoolChange(ushort fieldId, bool value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data bool but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataBool);
        ushort packed = (ushort)((fieldId << 1) | (value ? 1 : 0));
        writer.Write(packed);
    }
    
    public void WritePlayerDataFloatChange(ushort fieldId, float value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data float but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataFloat);
        writer.Write(fieldId);
        writer.Write(value);
    }
    
    public void WritePlayerDataIntChange(ushort fieldId, int value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data int but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataInt);
        writer.Write(fieldId);
        writer.Write(value);
    }

    public void WritePlayerDataEnumChange(ushort fieldId, ushort value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data enum but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataEnum);
        writer.Write(fieldId);
        writer.Write(value);
    }

    public void WritePlayerDataULongChange(ushort fieldId, ulong value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data ulong but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataULong);
        writer.Write(fieldId);
        writer.Write(value);
    }

    public void WritePlayerDataVector3Change(ushort fieldId, Vector3 value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data vector3 but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataVector3);
        writer.Write(fieldId);
        writer.WriteVector3(value);
    }

    public void WritePlayerDataVector2Change(ushort fieldId, Vector2 value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data vector2 but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataVector2);
        writer.Write(fieldId);
        writer.WriteVector2(value);
    }

    public void WritePlayerDataIntArrayFullChange(ushort fieldId, int[]? value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data int array full but writer is null");
            return;
        }

        int[] values = value ?? Array.Empty<int>();
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataIntList);
        writer.Write(fieldId);
        writer.Write(IntArrayModeFull);
        writer.Write(values.Length);
        for (int i = 0; i < values.Length; i++) {
            writer.Write(values[i]);
        }
    }

    public void WritePlayerDataIntListFullChange(ushort fieldId, System.Collections.Generic.IReadOnlyList<int>? value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data int list full but writer is null");
            return;
        }

        int count = value?.Count ?? 0;
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataIntList);
        writer.Write(fieldId);
        writer.Write(IntArrayModeFull);
        writer.Write(count);
        if (value == null) {
            return;
        }

        for (int i = 0; i < count; i++) {
            writer.Write(value[i]);
        }
    }

    public void WritePlayerDataIntArrayDeltaChange(
        ushort fieldId,
        int arrayLength,
        int[] changedIndices,
        int[] changedValues
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data int array delta but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataIntList);
        writer.Write(fieldId);
        writer.Write(IntArrayModeDelta);
        writer.Write(arrayLength);
        writer.Write(changedIndices.Length);
        for (int i = 0; i < changedIndices.Length; i++) {
            writer.Write(changedIndices[i]);
            writer.Write(changedValues[i]);
        }
    }

    public void WritePlayerDataStringCollectionFullChange(ushort fieldId, string[] values) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data string collection full but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataStringList);
        writer.Write(fieldId);
        writer.Write(StringCollectionModeFull);
        writer.WriteStringArray(values);
    }

    public void WritePlayerDataStringCollectionDeltaChange(
        ushort fieldId,
        int collectionLength,
        int[] changedIndices,
        string[] changedValues
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data string collection delta but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataStringList);
        writer.Write(fieldId);
        writer.Write(StringCollectionModeDelta);
        writer.Write(collectionLength);
        writer.Write(changedIndices.Length);
        for (int i = 0; i < changedIndices.Length; i++) {
            writer.Write(changedIndices[i]);
            writer.WriteStringCompat(changedValues[i] ?? string.Empty);
        }
    }

    public void WritePlayerDataStringSetDeltaChange(
        ushort fieldId,
        System.Collections.Generic.List<string> added,
        System.Collections.Generic.List<string> removed
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data string set delta but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataStringSet);
        writer.Write(fieldId);
        writer.Write(StringSetModeDelta);
        writer.Write(added.Count);
        for (int i = 0; i < added.Count; i++) {
            writer.WriteStringCompat(added[i] ?? string.Empty);
        }
        writer.Write(removed.Count);
        for (int i = 0; i < removed.Count; i++) {
            writer.WriteStringCompat(removed[i] ?? string.Empty);
        }
    }

    public void WritePlayerDataStringSetFullChange(
        ushort fieldId,
        System.Collections.Generic.ICollection<string>? values
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data string set full but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataStringSet);
        writer.Write(fieldId);
        writer.Write(StringSetModeFull);

        int count = values?.Count ?? 0;
        writer.Write(count);
        if (values == null) {
            return;
        }

        foreach (string value in values) {
            writer.WriteStringCompat(value ?? string.Empty);
        }
    }

    public void WritePlayerDataStringChange(ushort fieldId, string value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data string but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataString);
        writer.Write(fieldId);
        writer.WriteStringCompat(value);
    }

    public void WritePlayerDataGuidChange(ushort fieldId, Guid value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data guid but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataGuid);
        writer.Write(fieldId);
        writer.Write(value.ToByteArray());
    }
}
