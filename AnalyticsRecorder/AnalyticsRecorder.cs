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
using MapChanger.MonoBehaviours;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Drawing;
using Modding.Converters;
using AnalyticsRecorder.Converters;
using System.Security.Policy;
using System.Text;
using UnityEngine.Sprites;

namespace AnalyticsRecorder {
    public class AnalyticsRecorderMod : Mod {
        private static float WRITE_PERIOD_SECONDS = 1;
        private static string RECORDER_FILE_VERSION = "0.0.0";

        private static AnalyticsRecorderMod? _instance;

        private RecordingFileManager recording = RecordingFileManager.Instance;

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

            PlayerDataWriter.Instance.SetupHooks();
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
            recording.WriteEntryPrefix("room-dimensions");
            recording.Write(rmap.originOffsetX.ToString("0.00"));
            recording.WriteSep();
            recording.Write(rmap.originOffsetY.ToString("0.00"));
            recording.WriteSep();
            recording.Write(rmap.sceneWidth.ToString("0.00"));
            recording.WriteSep();
            recording.Write(rmap.sceneHeight.ToString("0.00"));
            recording.WriteNL();
        }

        private void ActiveSceneChanged(Scene oldScene, Scene newScene) {
            recording.WriteEntry("S", newScene.name);
        }

        // called when a save state is loaded or a new game is started
        private void InitializeRecorder() {
            recording.StartRecorder();

            profileId = GameManager.instance.profileID;

            recording.WriteEntry("recorder-file-version", RECORDER_FILE_VERSION);
            recording.WriteEntry("profile-id", profileId.ToString());
            recording.WriteEntry("application-version", Application.version);
            recording.WriteEntry("mod-version", GetVersion());
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
                    recording.WriteEntryPrefix("P");
                    recording.Write(knight.position.x.ToString("0.00"));
                    recording.WriteSep();
                    recording.Write(knight.position.y.ToString("0.00"));
                    recording.WriteNL();

                    lastFreqWriteTime = time;
                }
            }

            if (Input.GetKeyDown(KeyCode.J)) {
                MapExport.Instance.ExportMap();
            }
        }

        public override void Initialize() {
            Log("Initializing");

            // put additional initialization logic here

            Log("Initialized");
        }
    }
}