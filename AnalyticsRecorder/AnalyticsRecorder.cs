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

        private string GetUserFilePath(string suffix) {
            string recodingFileName = $"user{profileId}.{suffix}.dat";
            return Path.Combine(Application.persistentDataPath, recodingFileName);
        }

        private string GetRecordingPath() {
            return GetUserFilePath("analytics-recording");
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

            if (Input.GetKeyDown(KeyCode.J)) {
                ExportMap();
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

        public MapData ExportMap() {

            var imports = new StringBuilder();

            var rooms = new List<MapRoomData>();

            foreach (var m in GameObject.Find("Game_Map(Clone)").GetComponentsInChildren<RoomSprite>(true)) {
                var rsd = m.Rsd;



                //Log(m.Rsd?.MapZone);
                //Log(m.Rsd?.SceneName);
                //Log(m.OrigColor);

                var spriteRenderer = m.GetComponent<SpriteRenderer>();

                //Log(spriteRenderer.bounds);
                //Log(spriteRenderer.sprite.texture.GetPixels32());

                var mapZone = rsd.MapZone.ToString();
                var sceneName = rsd.SceneName;
                var gameObjectName = m.name;
                var roughMapRoom = m.GetComponent<RoughMapRoom>();
                var hasCorniferVersion = roughMapRoom != null;
                // if there is a cornifer version we can always get the not corniger version from roughMapRoom.
                var spriteName = roughMapRoom?.fullSprite?.name ?? spriteRenderer?.sprite?.name;
                // for more maps mod, we have to use the gameObject name, since it does not declare sprite names.
                if (spriteName == null || spriteName == "") {
                    spriteName = gameObjectName;
                }

                imports.AppendLine($"import {spriteName.ToLower()} from '../assets/areas/{spriteName}.png';");
                if (hasCorniferVersion) {
                    imports.AppendLine($"import {spriteName.ToLower()}_cornifer from '../assets/areas/{spriteName}_Cornifer.png';");
                }

                rooms.Add(new MapRoomData() {
                    sceneName = sceneName,
                    spriteName = spriteName,
                    gameObjectName = gameObjectName,
                    mapZone = mapZone,
                    origColor = m.OrigColor,
                    boundsMin = spriteRenderer.bounds.min,
                    boundsMax = spriteRenderer.bounds.max,
                    hasCorniferVersion = hasCorniferVersion,
                    sprite = $"IMPORT_STRING_START:{spriteName.ToLower()}:IMPORT_STRING_END",
                    spriteCorniger = hasCorniferVersion ? $"IMPORT_STRING_START:{spriteName.ToLower()}_cornifer:IMPORT_STRING_END" : null,
                });
            }

            var mapData = new MapData() {
                rooms = rooms,
            };
            var json = JsonConvert.SerializeObject(mapData, Formatting.Indented, jsonConverter);
            var js = json
                .Replace("\"IMPORT_STRING_START:", "")
                .Replace(":IMPORT_STRING_END\"", "")
            ;


            using (var mapWriter = new StreamWriter(GetUserFilePath("map-export"))) {
                mapWriter.Write(js);
            }

            using (var importWriter = new StreamWriter(GetUserFilePath("map-imports"))) {
                importWriter.Write(imports.ToString());
            }

            return mapData;
        }

        public static JsonConverter[] jsonConverter = new JsonConverter[] {
            new Vector2Converter(),
            new Vector3Converter(),
            new Vector4Converter(),
        };
    }

    [System.Serializable]
    public class MapRoomData {
        public string sceneName;
        public string spriteName;
        public string gameObjectName;
        public string mapZone;
        public Vector4 origColor;
        public Vector3 boundsMin;
        public Vector3 boundsMax;
        public string sprite;
        public string spriteCorniger;
        public bool hasCorniferVersion;
    }

    [System.Serializable]
    public class MapData {
        public List<MapRoomData> rooms;
    }
}