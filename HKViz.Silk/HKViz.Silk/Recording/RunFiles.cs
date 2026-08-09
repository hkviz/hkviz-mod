using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using BepInEx.Logging;
using HKViz.Shared;
using HKViz.Silk.GameData;
using HKViz.Silk.Recording.Behaviours;
using HKViz.Silk.Upload;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HKViz.Silk.Recording;

public class RunFiles(Guid localRunId, long currentRunPart, SilkUploadManager uploadManager, ManualLogSource logger) {
    public event Action<bool>? OnNewFileCreated;

    public Guid LocalRunId { get; } = localRunId;
    public long CurrentRunPart { get; private set; } = currentRunPart;
    public long currentFileFirstUnixSeconds { get; private set; } = DateTimeUtils.GetUnixSeconds();

    private const float SwitchFileAfterSeconds = 60f * 10L; // 5 minutes
    
    private float _lastPartCreatedTime = float.NegativeInfinity;
    private bool _isClosed;
    private bool _wroteTimeForUpdate;
    private long _lastWrittenTime;

    private string previousScene;

    private BinaryWriter? _writer;
    private ArrayBufferWriter<byte> _entryBuffer = new();

    private bool firstHeaderInSession = true;

    // while true, holds off rotating to a new file, so backfill always lands in the first file
    public bool BackfillPending { get; set; }

    public event Action<(long runPart, long partFirstUnixMillis, string previousScene)>? OnFileFinished;


    public void NextFileIfNeeded() {
        if (_isClosed || BackfillPending) return;
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
        OnNewFileCreated?.Invoke(firstHeaderInSession);
        firstHeaderInSession = false;


        // TODO add new file to global storage of known files
        //  even before upload. so upload can happen after crash
        FinishFile(oldWriter, previousRunPart, previousPartFirstUnixMillis, oldScene);
    }

    public void Close() {
        if (_isClosed) return;
        _isClosed = true;
        _writer?.WriteEntryType(WriteEntryType.SessionEnd);
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
        if (firstHeaderInSession) {
            writer.WriteEntryType(WriteEntryType.SessionStart);
        }
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

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(mode == LoadSceneMode.Additive ? WriteEntryType.SceneChangeAdd : WriteEntryType.SceneChangeSingle);
        writer.WriteIdOrStringCompat(SilkSongScenes.SCENES, sceneName);
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

    public void WritePlayerDataIntListFullChange(ushort fieldId, IReadOnlyList<int>? value) {
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

    public void WritePlayerDataStringCollectionFullChange(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
        string[] values
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data string collection full but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataStringListFull);
        writer.Write(fieldId);
        writer.WriteIdOrStringArray(valueToId, values);
    }

    public void WritePlayerDataStringCollectionDeltaChange(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
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
            writer.WriteIdOrStringCompat(valueToId, changedValues[i]);
        }
    }

    public void WritePlayerDataStringSetDeltaChange(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
        List<string> added,
        List<string> removed
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
            writer.WriteIdOrStringCompat(valueToId, added[i] ?? string.Empty);
        }
        writer.Write(removed.Count);
        for (int i = 0; i < removed.Count; i++) {
            writer.WriteIdOrStringCompat(valueToId, removed[i] ?? string.Empty);
        }
    }

    public void WritePlayerDataStringSetFullChange(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
        ICollection<string>? values
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
            writer.WriteIdOrStringCompat(valueToId, value ?? string.Empty);
        }
    }

    public void WritePlayerDataNamedMapFullChange<TData>(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
        IReadOnlyDictionary<string, TData>? values,
        Action<BinaryWriter, TData> writeValue
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug($"Tried to write player data named map full but writer is null (fieldId={fieldId}, values count={values?.Count ?? 0})");
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

        List<string> names = new(values.Keys);
        names.Sort(StringComparer.Ordinal);
        for (int i = 0; i < names.Count; i++) {
            string name = names[i];
            writer.WriteIdOrStringCompat(valueToId, name);
            writeValue(writer, values[name]);
        }
    }

    public void WritePlayerDataNamedMapDeltaChange<TData>(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
        IReadOnlyList<KeyValuePair<string, TData>> upserts,
        IReadOnlyList<string> removed,
        Action<BinaryWriter, TData> writeValue
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug($"Tried to write player data named map delta but writer is null (fieldId={fieldId}, upserts={upserts.Count}, removed={removed.Count})");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataNamedMapDelta);
        writer.Write(fieldId);

        writer.Write(upserts.Count);
        for (int i = 0; i < upserts.Count; i++) {
            writer.WriteIdOrStringCompat(valueToId, upserts[i].Key);
            writeValue(writer, upserts[i].Value);
        }

        writer.Write(removed.Count);
        for (int i = 0; i < removed.Count; i++) {
            writer.WriteIdOrStringCompat(valueToId, removed[i]);
        }
    }

    public void WritePlayerDataStoryEventListFullChange(ushort fieldId, IReadOnlyList<PlayerStory.EventInfo>? values) {
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

    public void WritePlayerDataStringChange(
        ushort fieldId,
        Dictionary<string, ushort> valueToId,
        string value
    ) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write player data string but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.PlayerDataString);
        writer.Write(fieldId);
        writer.WriteIdOrStringCompat(valueToId, value);
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
    
    public void WriteSceneDataBool(string sceneName, string key, bool value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write scene data bool but writer is null");
            return;
        }
        logger.LogDebug("WriteSceneDataBool");
        // TODO could pack scene booleans per scenes into bytes?
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.SceneDataBool);
        writer.WriteIdOrStringCompat(SilkSongScenes.SCENES, sceneName);
        writer.WriteIdOrStringCompat(SilkSongSceneDataBoolIds.VALUE_TO_ID, key);
        writer.WriteBool(value);
    }

    public void WriteSceneDataInt(string sceneName, string key, int value) {
        WriteSceneDataIntInternal(WriteEntryType.SceneDataInt, SilkSongSceneDataIntIds.VALUE_TO_ID, sceneName, key, value);
    }

    public void WriteSceneDataGeoRock(string sceneName, string key, int value) {
        WriteSceneDataIntInternal(WriteEntryType.SceneDataGeoRock, SilkSongSceneDataGeoRockIds.VALUE_TO_ID, sceneName, key, value);
    }

    private void WriteSceneDataIntInternal(WriteEntryType type, Dictionary<string, ushort> fieldNameLookup, string sceneName, string key, int value) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write scene data int but writer is null");
            return;
        }
        WriteTimeIfChanged(writer);
        writer.WriteEntryType(type);
        writer.WriteIdOrStringCompat(SilkSongScenes.SCENES, sceneName);
        writer.WriteIdOrStringCompat(fieldNameLookup, key);
        writer.WriteInt(value);
    }

    public void WriteRestorePointStart(ushort number, string date) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write restore point start but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        writer.WriteEntryType(WriteEntryType.RestorePointStart);
        writer.Write(number);
        writer.WriteStringCompat(date ?? string.Empty);
    }

    public void WriteRestorePointFinish() {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write restore point finish but writer is null");
            return;
        }

        writer.WriteEntryType(WriteEntryType.RestorePointFinish);
    }

    public void WriteEnemyStart(ushort id, string journalName, int healthManagerHp) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write enemy start but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        _entryBuffer.Clear();
        _entryBuffer.WriteEntryType(WriteEntryType.EnemyStart);
        _entryBuffer.WriteUShort(id);
        _entryBuffer.WriteIdOrString(SilkSongEnemyJournalIds.VALUE_TO_ID, journalName);
        _entryBuffer.WriteInt(healthManagerHp);
        writer.Write(_entryBuffer.WrittenSpan);
    }

    public void WriteEnemyLocationDiff(List<HKVizHealthManagerBehaviour> healthManagers) {
        var writer = _writer;
        if (writer == null) {
            return;
        }
        WriteTimeIfChanged(writer);
        _entryBuffer.Clear();
        _entryBuffer.WriteEntryType(WriteEntryType.EnemyLocations);
        var lengthOffset = _entryBuffer.WrittenCount;
        _entryBuffer.WriteUShort(0); // length - filled later
        ushort count = 0;
        foreach (var behaviour in healthManagers) {
            var currPosition = behaviour.trans.position;
            if (behaviour.lastWrittenPosition == currPosition) continue;
            _entryBuffer.WriteVector2(currPosition);
            behaviour.lastWrittenPosition = currPosition;
            count++;
        }

        if (count == 0) {
            _entryBuffer.Clear();
            // no change -> no write
            return;
        }
        _entryBuffer.WriteUShortAt(count, lengthOffset);
        writer.Write(_entryBuffer.WrittenSpan);
    }
    
    public void WriteEnemyHpDiff(List<HKVizHealthManagerBehaviour> healthManagers) {
        var writer = _writer;
        if (writer == null) {
            return;
        }
        WriteTimeIfChanged(writer);
        _entryBuffer.Clear();
        _entryBuffer.WriteEntryType(WriteEntryType.EnemyHps);
        var lengthOffset = _entryBuffer.WrittenCount;
        _entryBuffer.WriteUShort(0); // length - filled later
        ushort count = 0;
        foreach (var behaviour in healthManagers) {
            var currHp = behaviour.healthManager.hp;
            if (behaviour.lastWrittenHp == currHp) continue;
            _entryBuffer.WriteInt(currHp);
            behaviour.lastWrittenHp = currHp;
            count++;
        }

        if (count == 0) {
            _entryBuffer.Clear();
            // no change -> no write
            return;
        }
        _entryBuffer.WriteUShortAt(count, lengthOffset);
        writer.Write(_entryBuffer.WrittenSpan);
    }

    // Writes a HitInstance for the EnemyTakeDamage entry. Several HitInstance fields are
    // intentionally dropped because they're derivable from AttackType/RepresentingTool, only
    // relevant for target types we don't track, or purely cosmetic. See the per-field comments
    // below for the reasoning on every field, including the dropped ones.
    public void WriteEnemyTakeDamage(HKVizHealthManagerBehaviour behaviour, HitInstance hit) {
        var writer = _writer;
        if (writer == null) {
            logger.LogDebug("Tried to write enemy damaged but writer is null");
            return;
        }

        WriteTimeIfChanged(writer);
        _entryBuffer.Clear();
        _entryBuffer.WriteEntryType(WriteEntryType.EnemyTakeDamage);
        _entryBuffer.WriteUShort(behaviour.id);

        // AttackType: the core categorizer of the hit (Nail, Spell, Lava, ...). Everything marked
        // "derivable" below is derived against this (plus RepresentingTool), so it has to be kept.
        _entryBuffer.WriteByte((byte)hit.AttackType);

        // RepresentingTool: which crest/tool/skill dealt the hit, if any. Kept both for its own
        // sake (tool usage stats) and as the lookup key a reader could use to derive
        // ToolDamageFlags/PoisonDamageTicks/ZapDamageTicks/IsUsingNeedleDamageMult from static
        // tool data instead of storing them redundantly per hit (see dropped fields below).
        var representingTool = hit.RepresentingTool == null ? null : hit.RepresentingTool.name;
        _entryBuffer.WriteIdOrString(SilkSongToolItemIds.VALUE_TO_ID, representingTool);

        // SpecialType: orthogonal flag set (RapidBullet/RapidBomb/Piercer/Heavy/...) used for
        // block-direction and projectile behaviour. Not expressible via AttackType - the would-be
        // matching AttackTypes values (Piercer_OBSOLETE/RapidBullet_OBSOLETE) are obsolete - so
        // this is real, non-derivable data. A byte is enough: the highest flag bit is 0x80.
        _entryBuffer.WriteByte((byte)hit.SpecialType);

        // SilkGeneration: how much silk this hit grants (Full/FirstHit/None). Matters for silk
        // economy analysis and isn't implied by AttackType (e.g. a regular nail hit can still be
        // tagged None for hitting non-silk-granting objects).
        _entryBuffer.WriteByte((byte)hit.SilkGeneration);

        // DamageDealt: final computed damage, depends on player stats/charms at hit time, so it
        // can't be reconstructed from AttackType/Tool alone. A ushort is plenty of range: the
        // only outlier value in the game is the 9999 instant-kill sentinel
        // (EnemyKillEventListener), nowhere near ushort.MaxValue (65535).
        _entryBuffer.WriteUShort((ushort)Math.Clamp(hit.DamageDealt, 0, ushort.MaxValue));

        // DamageScalingLevel: nail/tool upgrade tier used for damage scaling. A small bounded
        // count of upgrades, fits comfortably in a byte instead of a full int.
        _entryBuffer.WriteByte((byte)Math.Clamp(hit.DamageScalingLevel, 0, byte.MaxValue));

        // StunDamage: amount added to the target's stagger meter. Kept as a float rather than
        // shrunk further: CheatManager.ForceStun sets it to float.MaxValue, and in normal play
        // it's a small fractional value where precision matters for the stagger math.
        _entryBuffer.WriteFloat(hit.StunDamage);

        // MagnitudeMultiplier / Multiplier: knockback/damage multipliers. Continuous values not
        // bounded by AttackType or Tool, kept as float.
        _entryBuffer.WriteFloat(hit.MagnitudeMultiplier);
        _entryBuffer.WriteFloat(hit.Multiplier);

        // Direction: ExtraUpDirection (a hard override used e.g. for forced-upward pogo hits) is
        // folded into the base Direction here instead of being its own nullable field, since it's
        // only ever consumed as "ExtraUpDirection ?? Direction". CircleDirection/MoveDirection/
        // MoveAngle are dropped: they only change the *effective* direction for homing/
        // velocity-based attacks, and resolving them needs the source's live transform/rigidbody
        // at hit time (GetActualDirection), which we don't have here. We accept the (rare)
        // precision loss for those attacks rather than threading Source through the recorder.
        _entryBuffer.WriteFloat(hit.ExtraUpDirection ?? hit.Direction);

        // Bit-packed bools - each is real per-hit state that doesn't follow from AttackType/Tool:
        //  - IsFirstHit: first hit of this swing/combo vs. a repeat/tick hit.
        //  - IsHeroDamage: hit dealt by the player vs. an enemy/hazard.
        //  - IgnoreInvulnerable: bypasses the target's invulnerability window.
        //  - IgnoreNailPosition: skips nail-position-based effects (e.g. non-positional sources).
        //  - IsManualTrigger: triggered by script/FSM rather than a normal collision.
        //  - CanWeakHit / ForceNotWeakHit: weak-hit eligibility and its override - independent.
        //  - NonLethal: hit can't reduce the target below 1 HP.
        //  - RageHit / CriticalHit / HunterCombo: depend on the player's build/combo state at the
        //    moment of the hit, not on the attack type itself.
        ushort boolPack = 0;
        if (hit.IsFirstHit) boolPack |= 1 << 0;
        if (hit.IsHeroDamage) boolPack |= 1 << 1;
        if (hit.IgnoreInvulnerable) boolPack |= 1 << 2;
        if (hit.IgnoreNailPosition) boolPack |= 1 << 3;
        if (hit.IsManualTrigger) boolPack |= 1 << 4;
        if (hit.CanWeakHit) boolPack |= 1 << 5;
        if (hit.ForceNotWeakHit) boolPack |= 1 << 6;
        if (hit.NonLethal) boolPack |= 1 << 7;
        if (hit.RageHit) boolPack |= 1 << 8;
        if (hit.CriticalHit) boolPack |= 1 << 9;
        if (hit.HunterCombo) boolPack |= 1 << 10;
        _entryBuffer.WriteUShort(boolPack);

        // --- Fields intentionally not written ---
        // NailElement / NailImbuement: forced to None/null for every AttackType except Nail/Fire
        // (AttackType.Fire always sets NailElement = Fire; NailImbuement is only ever non-null
        // for the player's own nail attacks). The only remaining variance (active nail-imbuement
        // charm during an AttackType.Nail hit) is loadout state, not per-hit data.
        // IsUsingNeedleDamageMult: an inspector toggle fixed per (AttackType, Tool), not per hit -
        // can be looked up from static attack/tool data instead of stored every hit.
        // ToolDamageFlags: equals RepresentingTool.DamageFlags whenever a tool is involved (and
        // is ~always None otherwise) - derivable from the RepresentingTool written above.
        // PoisonDamageTicks / ZapDamageTicks: equal RepresentingTool.PoisonDamageTicks /
        // .ZapDamageTicks gated by whether the Poison Pouch / Zap charm is equipped - derivable
        // from RepresentingTool plus charm-equip state (tracked separately), not per-hit data.
        // IsNailTag: IsNailDamage already treats AttackType == Nail as nail damage on its own;
        // this flag only matters for the rare non-Nail attack that should still count as nail
        // damage, which we accept losing for now.
        // UseCorpseDirection/CorpseDirection, UseBouncePodDirection/BouncePodDirection/
        // CanTriggerBouncePod, UseCorpseMagnitudeMult/CorpseMagnitudeMultiplier,
        // UseCurrencyMagnitudeMult/CurrencyMagnitudeMult: all only affect non-regular target types
        // (corpses, bounce pods, currency drops), which we don't track/visualize.
        // CircleDirection/MoveDirection/MoveAngle/ExtraUpDirection: see Direction comment above.
        // SlashEffectOverrides / HitEffectsType: purely cosmetic VFX selection, irrelevant to
        // analytics/replay.
        // Source: the GameObject that dealt the hit; not meaningfully serializable here - "who
        // dealt the hit" is already covered by IsHeroDamage + AttackType + RepresentingTool.

        writer.Write(_entryBuffer.WrittenSpan);
    }
}
