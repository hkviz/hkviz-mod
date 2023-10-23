using Modding;
using System;
using UnityEngine;
using TeamCherry;
using Modding.Utils;
using Modding.Menu;
using System.IO;
using HutongGames.PlayMaker.Actions;
using Mono.Security.X509.Extensions;
using UnityEngine.SceneManagement;
using HKMirror.Reflection.SingletonClasses;
using HKMirror.Reflection;

namespace AnalyticsRecorder {
    public class AnalyticsRecorderMod : Mod {
        private static float WRITE_PERIOD_SECONDS = 1;

        private static AnalyticsRecorderMod? _instance;

        internal static AnalyticsRecorderMod Instance {
            get {
                if (_instance == null) {
                    throw new InvalidOperationException($"An instance of {nameof(AnalyticsRecorderMod)} was never constructed");
                }
                return _instance;
            }
        }

        private Transform? knight;
        private int profileId = -1;
        private StreamWriter? writer;
        private float lastFreqWriteTime = 0;

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public AnalyticsRecorderMod() : base("AnalyticsRecorder") {
            _instance = this;

            ModHooks.HeroUpdateHook += HeroUpdateHook;
            ModHooks.SavegameLoadHook += SavegameLoadHook;
            ModHooks.NewGameHook += NewGameHook;

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
            On.GameMap.SetManualTilemap += GameMap_SetManualTilemap;
            On.GameMap.GetTilemapDimensions += GameMap_GetTilemapDimensions;

            
        }

        private void GameMap_GetTilemapDimensions(On.GameMap.orig_GetTilemapDimensions orig, GameMap self) {
            orig(self);
            WriteRoomTilemap(self);
        }

        private void GameMap_SetManualTilemap(On.GameMap.orig_SetManualTilemap orig, GameMap self, float offsetX, float offsetY, float width, float height) {
            orig(self, offsetX, offsetY, width, height);
            WriteRoomTilemap(self);
        }

        private void WriteRoomTilemap(GameMap map) {
            var rmap = map.Reflect();
            WriteEntryPrefix("room-dimensions");
            Write(rmap.originOffsetX.ToString("0.00"));
            WriteSep();
            Write(rmap.originOffsetY.ToString("0.00"));
            WriteSep();
            Write(rmap.sceneWidth.ToString("0.00"));
            WriteSep();
            Write(rmap.sceneHeight.ToString("0.00"));
        }

        private void ActiveSceneChanged(Scene oldScene, Scene newScene) {
            if (writer != null) {
                WriteEntry("S", newScene.name);
            }

            //var sceneBounds = GameObject.Find("Map Scene Region");
            //if (sceneBounds == null) {
            //    TeamCherry.y.
            //}
        }

        private string GetRecordingPath() {
            string recodingFileName = $"user{profileId}.analytics-recording.dat";
            return Path.Combine(Application.persistentDataPath, recodingFileName);
        }

        // called when a save state is loaded or a new game is started
        private void InitializeRecorder() {
            StopRecorder();

            profileId = GameManager.instance.profileID;
            var recordingPath = GetRecordingPath();

            bool existed = File.Exists(recordingPath);
            writer = new StreamWriter(recordingPath, append: true);

            if (!existed) {
                WriteEntry("recording-id",  Guid.NewGuid().ToString());
            }
            WriteEntry("profile-id", profileId.ToString());
            WriteEntry("mod-version", GetVersion());
        }

        private void StopRecorder() {
            try {
                if (writer != null) {
                    WriteEntry("session-end", "");
                    writer.Close();
                }
            } finally {
                writer = null;
            }
        }


        private void NewGameHook() {
            InitializeRecorder();
        }

        private void SavegameLoadHook(int playerIndex) {
            InitializeRecorder();
        }

        private void HeroUpdateHook() {
            if (knight == null) { // if destroyed needs to find new player
                knight = GameObject.Find("Knight").transform;
            }          

            if (knight != null) {
                var time = Time.time;
                if (time - lastFreqWriteTime > WRITE_PERIOD_SECONDS) {
                    WriteEntryPrefix("P");
                    Write(knight.position.x.ToString("0.00"));
                    WriteSep();
                    Write(knight.position.y.ToString("0.00"));
                    WriteNL();

                    lastFreqWriteTime = time;
                }
            }
        }

        public override void Initialize() {
            Log("Initializing");

            // put additional initialization logic here

            Log("Initialized");
        }

        private void WriteEntryPrefix(string eventType) {
            // adding timestamp to each line
            writer?.Write(eventType);
            WriteSep();
            writer?.Write(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            WriteSep();
        }

        private void WriteEntry(string eventType, string content) {
            WriteEntryPrefix(eventType);
            Write(content);
            WriteNL();
        }

        private void Write(string content) {
            writer?.Write(content);
        }

        private void WriteNL() {
            writer?.WriteLine();
        }

        private void WriteSep() {
            writer?.Write(";");
        }
    }
}
