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
            Log("Started map export. This should run at the beginning of the game, after buying the first map, but before buying the quill");

            var imports = new StringBuilder();

            var rooms = new List<MapRoomData>();

            foreach (var m in GameObject.Find("Game_Map(Clone)").GetComponentsInChildren<RoomSprite>(true)) {
                var rsd = m.Rsd;



                //Log(m.Rsd?.MapZone);
                //Log(m.Rsd?.SceneName);
                //Log(m.OrigColor);

                var selfSpriteRenderer = m.GetComponent<SpriteRenderer>();

                //Log(spriteRenderer.bounds);
                //Log(spriteRenderer.sprite.texture.GetPixels32());

                var mapZone = rsd.MapZone.ToString();
                var sceneName = rsd.SceneName;
                var gameObjectName = m.name;
                var roughMapRoom = m.GetComponent<RoughMapRoom>();

                // we can not use the generic GetComponent, since the type is internal
                var isAdditionalMap = m.GetComponent("AdditionalMaps.MonoBehaviours.MappedCustomRoom") != null;
                var additionalMapSpriteRenderer = isAdditionalMap ? m.transform.GetChild(0)?.GetComponent<SpriteRenderer>() : null;

                // if there is a cornifer version we can always get the not cornifer version from roughMapRoom.
                Sprite? sprite = additionalMapSpriteRenderer?.sprite ?? roughMapRoom?.fullSprite ?? selfSpriteRenderer?.sprite;
                var spriteInfo = SpriteInfo.fromSprite(sprite, m.gameObject.name);

                if (m.gameObject.name == "Fungus3_48_bot") {
                    // speical case, since sprite name is duplicated in game of 47. 
                    spriteInfo.name = "Fungus3_48_bottom";
                }
                if (m.gameObject.name == "Ruins2_10") {
                    spriteInfo.name = "Ruins2_10";
                }

                var visualSpriteRenderer = additionalMapSpriteRenderer ?? selfSpriteRenderer;



                bool hasCorniferVersion = (roughMapRoom != null && 
                        roughMapRoom.fullSprite.name != null && // addional map mod ignored wrong rough maps
                        roughMapRoom.fullSprite.name != "" && // addional map mod ignored wrong rough maps
                        roughMapRoom.fullSprite.name != selfSpriteRenderer?.sprite?.name); // rough maps accidentally left in the game? using same sprite ignored.

                var roughSprite = hasCorniferVersion ? selfSpriteRenderer?.sprite : null;
                var roughSpriteInfo = roughSprite != null ? SpriteInfo.fromSprite(roughSprite, m.gameObject.name + "_Cornifer") : null;



                imports.AppendLine($"import {spriteInfo.name.ToLower()} from '../assets/areas/{spriteInfo.name}.png';");
                if (roughSpriteInfo != null) {
                    imports.AppendLine($"import {roughSpriteInfo.name.ToLower()} from '../assets/areas/{roughSpriteInfo.name}.png';");
                }

                rooms.Add(new MapRoomData() {
                    sceneName = sceneName,
                    spriteInfo = spriteInfo,
                    roughSpriteInfo = roughSpriteInfo,
                    gameObjectName = gameObjectName,
                    mapZone = mapZone,
                    origColor = m.OrigColor,
                    visualBounds = ExportBounds.fromBounds(visualSpriteRenderer.bounds),
                    playerPositionBounds = ExportBounds.fromBounds(selfSpriteRenderer.bounds),
                    hasRoughVersion = hasCorniferVersion,
                    sprite = $"IMPORT_STRING_START:{spriteInfo.name.ToLower()}:IMPORT_STRING_END",
                    spriteRough = roughSpriteInfo != null ? $"IMPORT_STRING_START:{roughSpriteInfo.name?.ToLower()}:IMPORT_STRING_END" : null,
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

            Log("Finished map export");
            return mapData;
        }

        public static JsonConverter[] jsonConverter = new JsonConverter[] {
            new Vector2Converter(),
            new Vector3Converter(),
            new Vector4Converter(),
        };
    }

    [System.Serializable]
    public class SpriteInfo {
        public string name;
        public Vector2 size;
        public Vector4 padding; 

        public static SpriteInfo fromSprite(Sprite? sprite, string fallbackName) {
            var spriteName = sprite.name;
            if (spriteName == null || spriteName == "") spriteName = fallbackName;

            var size = sprite == null ? Vector2.zero : new Vector2(sprite.rect.width, sprite.rect.height);
            var padding = sprite == null ? Vector4.zero : DataUtility.GetPadding(sprite);

            return new SpriteInfo() {
                name = spriteName,
                size = size,
                padding = padding,
            };
        }
    }

    [System.Serializable]
    public class MapRoomData {
        public string sceneName;
        public SpriteInfo spriteInfo;
        public SpriteInfo? roughSpriteInfo;
        public string gameObjectName;
        public string mapZone;
        public Vector4 origColor;
        public ExportBounds visualBounds;
        public ExportBounds playerPositionBounds;
        public string sprite;
        public string? spriteRough;
        public bool hasRoughVersion;
    }

    [System.Serializable]
    public class ExportBounds {
        public Vector3 min;
        public Vector3 max;

        public static ExportBounds fromBounds(UnityEngine.Bounds bounds) {
            return new ExportBounds {
                min = bounds.min,
                max = bounds.max,
            };
        }
    }

    [System.Serializable]
    public class MapData {
        public List<MapRoomData> rooms;
    }
}