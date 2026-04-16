using System;
using System.IO;
using BepInEx.Logging;
using GlobalEnums;
using HKViz.Shared;
using HKViz.Silk.GameData;
using HKViz.Silk.Upload;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HKViz.Silk.Recording;

public class RunFiles(Guid localRunId, long currentRunPart, SilkUploadManager uploadManager, ManualLogSource logger) {
    public event Action? OnNewFileCreated;

    public Guid LocalRunId { get; } = localRunId;
    public long CurrentRunPart { get; private set; } = currentRunPart;
    public long currentFileFirstUnixSeconds { get; private set; } = DateTimeUtils.GetUnixSeconds();

    private const float SwitchFileAfterSeconds = 60f * 10L; // 5 minutes
    
    private float _lastPartCreatedTime;
    private bool _isClosed;
    private bool _wroteTimeForUpdate;
    private long _lastWrittenTime;
    
    private string previousScene;
    
    private BinaryWriter? _writer;

    public event Action<(long runPart, long partFirstUnixMillis, string previousScene)>? OnFileFinished;


    public void NextFileIfNeeded() {
        if (_isClosed) return;
        if (_writer == null || Time.unscaledTime - _lastPartCreatedTime > SwitchFileAfterSeconds) {
            NextFile();
        }
    }
    
    public float NextPartInSeconds => SwitchFileAfterSeconds - (Time.unscaledTime - _lastPartCreatedTime);

    public void NextFile() {
        if (_isClosed) return;
        var oldScene = previousScene;
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
        FinishFile(oldWriter, previousRunPart, previousPartFirstUnixMillis, oldScene);
    }

    public void Close() {
        if (_isClosed) return;
        _isClosed = true;
        FinishFile(_writer, CurrentRunPart, currentFileFirstUnixSeconds, previousScene);
        _writer = null;
    }

    private void FinishFile(BinaryWriter? writer, long partNumber, long partFirstUnixMillis, string previousScene) {
        if (writer == null) {
            return;
        }
        writer.Close();

        // TODO should use logic like Hollow Knight, where previous value from writer is loaded
        // string? pd(string key) {
            // var hasValue = PlayerDataWriter.Instance.previousPlayerData.TryGetValue(key, out var value);
            //return hasValue ? value : null;
        // }
        
        OnFileFinished?.Invoke((runPart: partNumber, partFirstUnixMillis: partFirstUnixMillis, previousScene: previousScene));
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
        previousScene = sceneName;
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
        writer.WriteEntryType(WriteEntryType.PlayerDataIntListFull);
        writer.Write(fieldId);
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
        writer.WriteEntryType(WriteEntryType.PlayerDataIntListFull);
        writer.Write(fieldId);
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
        writer.WriteEntryType(WriteEntryType.PlayerDataIntListDelta);
        writer.Write(fieldId);
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
        writer.WriteEntryType(WriteEntryType.PlayerDataStringListFull);
        writer.Write(fieldId);
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
        writer.WriteEntryType(WriteEntryType.PlayerDataStringListDelta);
        writer.Write(fieldId);
        writer.Write(collectionLength);
        writer.Write(changedIndices.Length);
        for (int i = 0; i < changedIndices.Length; i++) {
            writer.Write(changedIndices[i]);
            writer.WriteStringCompat(changedValues[i]);
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
        writer.WriteEntryType(WriteEntryType.PlayerDataStringSetDelta);
        writer.Write(fieldId);
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
        writer.WriteEntryType(WriteEntryType.PlayerDataStringSetFull);
        writer.Write(fieldId);

        int count = values?.Count ?? 0;
        writer.Write(count);
        if (values == null) {
            return;
        }

        foreach (string value in values) {
            writer.WriteStringCompat(value ?? string.Empty);
        }
    }

    public void WritePlayerDataNamedMapFullChange<TData>(
        ushort fieldId,
        System.Collections.Generic.IReadOnlyDictionary<string, TData>? values,
        Action<BinaryWriter, TData> writeValue
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data named map full but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataNamedMapFull);
        writer.Write(fieldId);

        int count = values?.Count ?? 0;
        writer.Write(count);
        if (values == null || count == 0) {
            return;
        }

        System.Collections.Generic.List<string> names = new(values.Keys);
        names.Sort(StringComparer.Ordinal);
        for (int i = 0; i < names.Count; i++) {
            string name = names[i];
            writer.WriteStringCompat(name);
            writeValue(writer, values[name]);
        }
    }

    public void WritePlayerDataNamedMapDeltaChange<TData>(
        ushort fieldId,
        System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<string, TData>> upserts,
        System.Collections.Generic.IReadOnlyList<string> removed,
        Action<BinaryWriter, TData> writeValue
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data named map delta but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataNamedMapDelta);
        writer.Write(fieldId);

        writer.Write(upserts.Count);
        for (int i = 0; i < upserts.Count; i++) {
            writer.WriteStringCompat(upserts[i].Key);
            writeValue(writer, upserts[i].Value);
        }

        writer.Write(removed.Count);
        for (int i = 0; i < removed.Count; i++) {
            writer.WriteStringCompat(removed[i]);
        }
    }

    public void WritePlayerDataStoryEventListFullChange(ushort fieldId, System.Collections.Generic.IReadOnlyList<PlayerStory.EventInfo>? values) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data story event list full but writer is null");
            return;
        }

        int count = values?.Count ?? 0;
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataStoryEventListFull);
        writer.Write(fieldId);
        writer.Write(count);

        if (values == null) {
            return;
        }

        for (int i = 0; i < count; i++) {
            writer.WriteStoryEventInfoData(values[i]);
        }
    }

    public void WritePlayerDataStoryEventListDeltaChange(
        ushort fieldId,
        int listLength,
        int[] changedIndices,
        PlayerStory.EventInfo[] changedValues
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data story event list delta but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataStoryEventListDelta);
        writer.Write(fieldId);
        writer.Write(listLength);
        writer.Write(changedIndices.Length);
        for (int i = 0; i < changedIndices.Length; i++) {
            writer.Write(changedIndices[i]);
            writer.WriteStoryEventInfoData(changedValues[i]);
        }
    }

    public void WritePlayerDataWrappedVector2ListFullChange(ushort fieldId, WrappedVector2List[]? values) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data wrapped vector2 list full but writer is null");
            return;
        }

        WrappedVector2List[] data = values ?? Array.Empty<WrappedVector2List>();
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataWrappedVector2ListFull);
        writer.Write(fieldId);
        writer.Write(data.Length);
        for (int i = 0; i < data.Length; i++) {
            writer.WriteWrappedVector2List(data[i]);
        }
    }

    public void WritePlayerDataWrappedVector2ListDeltaChange(
        ushort fieldId,
        int listLength,
        int[] changedIndices,
        WrappedVector2List[] changedValues
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data wrapped vector2 list delta but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataWrappedVector2ListDelta);
        writer.Write(fieldId);
        writer.Write(listLength);
        writer.Write(changedIndices.Length);
        for (int i = 0; i < changedIndices.Length; i++) {
            writer.Write(changedIndices[i]);
            writer.WriteWrappedVector2List(changedValues[i]);
        }
    }

    public void WritePlayerDataWrappedVector2ListAppendChange(ushort fieldId, int oldLength, WrappedVector2List[] appendedValues) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data wrapped vector2 list append but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataWrappedVector2ListAppend);
        writer.Write(fieldId);
        writer.Write(oldLength);
        writer.Write(appendedValues.Length);
        for (int i = 0; i < appendedValues.Length; i++) {
            writer.WriteWrappedVector2List(appendedValues[i]);
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
