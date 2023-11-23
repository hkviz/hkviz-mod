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
using Core.FsmUtil;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using System.Linq;

namespace AnalyticsRecorder {
    public class AnalyticsRecorderMod : Mod, ILocalSettings<LocalSettings>, ICustomMenuMod, IGlobalSettings<GlobalSettings> {
        private static float WRITE_PERIOD_SECONDS = .5f;
        private static string RECORDER_FILE_VERSION = "0.0.0";

        private static AnalyticsRecorderMod? _instance;

        private RecordingFileManager recording = RecordingFileManager.Instance;
        private RecordingSerializer serializer = RecordingSerializer.Instance;

        internal static AnalyticsRecorderMod Instance {
            get {
                if (_instance == null) {
                    throw new InvalidOperationException($"An instance of {nameof(AnalyticsRecorderMod)} was never constructed");
                }
                return _instance;
            }
        }

        public bool ToggleButtonInsideMenu { get;  }

        private Transform? knight;
        private int profileId = -1;
        private float lastFreqWriteTime = 0;

        private string previousPositionString = "";

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public AnalyticsRecorderMod() : base("HKViz") {
            _instance = this;
            MainMenuUI.Instance.Initialize();

            ModHooks.HeroUpdateHook += HeroUpdateHook;
            ModHooks.SavegameLoadHook += SavegameLoadHook;
            ModHooks.NewGameHook += NewGameHook;

            ModHooks.AttackHook += ModHooks_AttackHook;

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
            On.GameMap.SetManualTilemap += GameMap_SetManualTilemap;
            On.GameMap.GetTilemapDimensions += GameMap_GetTilemapDimensions;

            PlayerDataWriter.Instance.SetupHooks();
            HeroControllerWriter.Instance.SetupHooks();

            InitFsm();

        }

        private void ModHooks_AttackHook(GlobalEnums.AttackDirection obj) {
            // throw new NotImplementedException();
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
            recording.WriteEntryPrefix(RecordingPrefixes.ROOM_DIMENSIONS);
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
            if (newScene.name == "Menu_Title") {
                // player went back to menu, so recorder should stop.
                recording.StopRecorder();
            } else {
                recording.WriteEntry("S", newScene.name);
            }
        }

        // called when a save state is loaded or a new game is started
        private void InitializeRecorder() {
            recording.StartRecorder();

            profileId = GameManager.instance.profileID;

            HeroControllerWriter.Instance.InitializeRun();

            recording.WriteEntry(RecordingPrefixes.RECORDING_FILE_VERSION, RECORDER_FILE_VERSION);

            // TODO instead log with playerData from start
            // recording.WriteEntry("profile-id", profileId.ToString());
            recording.WriteEntry(RecordingPrefixes.HOLLOWKNIGHT_VERSION, Application.version);
            recording.WriteEntry(RecordingPrefixes.HZVIZ_MOD_VERSION, GetVersion());
        }


        private void NewGameHook() {
            InitializeRecorder();
        }

        private void SavegameLoadHook(int playerIndex) {
            InitializeRecorder();
        }

        private void InitKnight() {
            knight = GameObject.Find("Knight").transform;

            Log("|" + String.Join("|", knight.gameObject.LocateMyFSM("Nail Arts").FsmStates.Select(it => it.Name).ToList()) + "|");
            Log("|" + String.Join("|", knight.gameObject.LocateMyFSM("Spell Control").FsmStates.Select(it => it.Name).ToList()) + "|");
            Log("|" + String.Join("|", knight.gameObject.LocateMyFSM("Superdash").FsmStates.Select(it => it.Name).ToList()) + "|");
        }

        private void InitFsm() {
            // ---- SPELLS ----
            // fireball
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Spell Control",
                StateName: "Fireball Antic"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.SPELL_FIREBALL, facingDirectionChar());
            });

            // spell down
            Hooks.HookStateExited(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Spell Control",
                StateName: "Quake Antic"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.SPELL_DOWN);
            });

            // spell up
            Hooks.HookStateExited(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Spell Control",
                StateName: "Scream Antic1"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.SPELL_UP);
            });
            Hooks.HookStateExited(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Spell Control",
                StateName: "Scream Antic2"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.SPELL_UP);
            });

            // ----- NAIL ARTS -----
            // cyclone
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Nail Arts",
                StateName: "Cyclone Start"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.NAIL_ART_CYCLONE, facingDirectionChar());
            });
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Nail Arts",
                StateName: "Cyclone End"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.NAIL_ART_CYCLONE, "0");
            });
            // dslash
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Nail Arts",
                StateName: "DSlash Start"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.NAIL_ART_D_SLASH, facingDirectionChar());
            });
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Nail Arts",
                StateName: "D Slash End"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.NAIL_ART_D_SLASH, "0");
            });
            // gslash
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Nail Arts",
                StateName: "Flash 2"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.NAIL_ART_G_SLASH, facingDirectionChar());
            });
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Nail Arts",
                StateName: "G Slash End"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.NAIL_ART_G_SLASH, "0");
            });

            // ----- SUPER DASH ----
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Superdash",
                StateName: "G Right"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.SUPER_DASH, "r");
            });
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Superdash",
                StateName: "G Left"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.SUPER_DASH, "l");
            });
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Superdash",
                StateName: "Regain Control"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.SUPER_DASH, "0");
            });
        }

        private string facingDirectionChar() => HeroController.instance.cState.facingRight ? "r" : "l";

        private void HeroUpdateHook() {
            if (GameManager.instance.isPaused) return;

            if (knight == null) { // if destroyed needs to find new player
                InitKnight();
            }

            if (knight != null) {
                var time = Time.time;
                if (time - lastFreqWriteTime > WRITE_PERIOD_SECONDS) {
                    recording.WriteEntryPrefix(RecordingPrefixes.PLAYER_POSITION);
                    var currentPositionString = serializer.serialize(new Vector2(knight.position.x, knight.position.y), "0.00"); // no z position needed
                    if (previousPositionString == currentPositionString) {
                        recording.Write("=");
                    } else {
                        recording.Write(currentPositionString);
                        previousPositionString = currentPositionString;
                    }
                    recording.WriteNL();

                    lastFreqWriteTime = time;
                }
            }

            if (Input.GetKeyDown(KeyCode.J)) {
                MapExport.Instance.Export();
                PlayerDataExport.Instance.Export();
                HeroControllerExport.Instance.Export();
            }
        }

        public override void Initialize() {
            Log("Initializing");

            // put additional initialization logic here

            Log("Initialized");
        }

        void ILocalSettings<LocalSettings>.OnLoadLocal(LocalSettings s) {
            // TODO for new game initialize with defaults from config.
            Log("Loading local settings" + s);
            PlayerDataWriter.Instance.InitFromLocalSave(s.previousPlayerData);
        }

        LocalSettings ILocalSettings<LocalSettings>.OnSaveLocal() {
            Log("Save local settings");
            return new LocalSettings {
                previousPlayerData = PlayerDataWriter.Instance.previousPlayerData,
            };
        }

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
            => HKVizModUI.Instance.GetMenuScreen(modListMenu, toggleDelegates);

        public void OnLoadGlobal(GlobalSettings s) {
            GlobalSettingsManager.Instance.InitializeFromSavedSettings(s);
            HKVizAuthManager.Instance.GlobalSettingsLoaded();
        }

        public GlobalSettings OnSaveGlobal() {
            return GlobalSettingsManager.Settings;
        }
    }
}
