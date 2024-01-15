using Core.FsmUtil;
using HKMirror.Reflection;
using Modding;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HKViz {
    public class HKVizMod : Mod, ILocalSettings<LocalSettings>, ICustomMenuMod, IGlobalSettings<GlobalSettings> {
        private static HKVizMod? _instance;

        internal static HKVizMod Instance {
            get {
                if (_instance == null) {
                    throw new InvalidOperationException($"An instance of {nameof(HKVizMod)} was never constructed");
                }
                return _instance;
            }
        }

        private RecordingFileManager recording = RecordingFileManager.Instance;
        private RecordingSerializer serializer = RecordingSerializer.Instance;

        public bool ToggleButtonInsideMenu { get; }

        private int profileId = -1;

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public HKVizMod() : base("HKViz") {
            _instance = this;
            BehaviourManager.Instance.Initialize();
            MainMenuUI.Instance.Initialize();
            UploadManager.Instance.Initialize();
            RecordingFileManager.Instance.Initialize();

            ModHooks.HeroUpdateHook += HeroUpdateHook;
            ModHooks.SavegameLoadHook += SavegameLoadHook;
            ModHooks.NewGameHook += NewGameHook;

            ModHooks.AttackHook += ModHooks_AttackHook;

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
            On.GameMap.SetManualTilemap += GameMap_SetManualTilemap;
            On.GameMap.GetTilemapDimensions += GameMap_GetTilemapDimensions;
            On.GameManager.ReturnToMainMenu += GameManager_ReturnToMainMenu;

            PlayerDataWriter.Instance.SetupHooks();
            HeroControllerWriter.Instance.SetupHooks();
            PlayerPositionWriter.Instance.Ininitialize();
            EnemyWriter.Instance.SetupHooks();

            InitFsm();
        }

        private System.Collections.IEnumerator GameManager_ReturnToMainMenu(On.GameManager.orig_ReturnToMainMenu orig, GameManager self, GameManager.ReturnToMainMenuSaveModes saveMode, System.Action<bool> callback) {
            // will switch to next part --> next time uses next one.
            RecordingFileManager.Instance.OnReturnToMenu();

            return orig(self, saveMode, callback);
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
            recording.Write(serializer.serialize(new Vector2(rmap.originOffsetX, rmap.originOffsetY)));
            recording.WriteSep();
            recording.Write(serializer.serialize(new Vector2(rmap.sceneWidth, rmap.sceneHeight)));
            recording.WriteNL();
        }

        private void ActiveSceneChanged(Scene oldScene, Scene newScene) {
            // only switch file here, so a scene event will always be the first thing inside a recording part.
            recording.SwitchToNextPartIfNessessary();
            PlayerPositionWriter.Instance.ResetFrequency();
            EnemyWriter.Instance.ActiveSceneChanged(oldScene, newScene);
            if (newScene.name != "Menu_Title") {
                recording.WriteEntry(RecordingPrefixes.SCENE_CHANGE, newScene.name);
                RecordingFileManager.Instance.lastScene = newScene.name;
            } else {
                UploadManager.Instance.RetryFailedUploads();
            }
        }

        // called when a save state is loaded or a new game is started
        private void InitializeRecorder() {
            recording.StartRecorder();

            profileId = GameManager.instance.profileID;

            HeroControllerWriter.Instance.InitializeRun();

            // TODO instead log with playerData from start
            // recording.WriteEntry("profile-id", profileId.ToString());
            recording.WriteEntry(RecordingPrefixes.HZVIZ_MOD_VERSION, GetVersion());
            recording.WriteEntry(
                RecordingPrefixes.MODDING_INFO,
                GameObject.FindObjectOfType<ModVersionDraw>().drawString
                    .ReplaceNewLines(";")
                    .Replace(" : ", ":")
                    .Replace(": ", ":")
            );
        }


        private void NewGameHook() {
            InitializeRecorder();
        }

        private void SavegameLoadHook(int playerIndex) {
            InitializeRecorder();
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
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Dream Nail",
                StateName: "Slash"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.DREAM_NAIL_SLASH);
            });
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Dream Nail",
                StateName: "Warp Effect"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.DREAM_NAIL_GATE_WARP);
            });
            Hooks.HookStateEntered(new FSMData(
                // GameObjectName: knight.gameObject.name,
                FsmName: "Dream Nail",
                StateName: "Set"
            ), a => {
                recording.WriteEntry(RecordingPrefixes.DREAM_NAIL_SET_GATE);
            });
        }

        private string facingDirectionChar() => HeroController.instance.cState.facingRight ? "r" : "l";

        private void HeroUpdateHook() {
            var unixMillis = recording.GetUnixMillis();

            GameManagerWriter.Instance.WriteChangedFields(unixMillis);
            PlayerPositionWriter.Instance.WritePositionsIfNeeded(unixMillis);
            HeroControllerWriter.Instance.WriteChangedStates(unixMillis);
            PlayerDataWriter.Instance.WriteChangedValues(unixMillis);


            //if (Input.GetKeyDown(KeyCode.J)) {
            //    MapExport.Instance.Export();
            //    PlayerDataExport.Instance.Export();
            //    HeroControllerExport.Instance.Export();
            //}
        }

        public override void Initialize() {
            Log("Initializing");

            // put additional initialization logic here
            BehaviourManager.Instance.gameObject.AddComponent<HKVizIMGUI>();

            Log("Initialized");
        }

        void ILocalSettings<LocalSettings>.OnLoadLocal(LocalSettings s) {
            // TODO for new game initialize with defaults from config.
            Log("Loading local settings" + s);
            PlayerDataWriter.Instance.InitFromLocalSave(s.previousPlayerData);
            RecordingFileManager.Instance.InitFromLocalSave(s);
            GameManagerWriter.Instance.InitFromLocalSave();
            // InitializeRecorder();
        }

        LocalSettings ILocalSettings<LocalSettings>.OnSaveLocal() {
            Log("Save local settings");
            return new LocalSettings {
                previousPlayerData = PlayerDataWriter.Instance.previousPlayerData,
                currentPart = RecordingFileManager.Instance.currentPart,
                localRunId = RecordingFileManager.Instance.localRunId
            };
        }

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
            => HKVizModUI.Instance.GetMenuScreen(modListMenu, toggleDelegates);

        public void OnLoadGlobal(GlobalSettings s) {
            GlobalSettingsManager.Instance.InitializeFromSavedSettings(s);
            HKVizAuthManager.Instance.GlobalSettingsLoaded();
            UploadManager.Instance.GlobalSettingsLoaded();
            MainMenuUI.Instance.GlobalSettingsLoaded();
        }

        public GlobalSettings OnSaveGlobal() {
            UploadManager.Instance.GlobalSettingsBeforeSave();
            return GlobalSettingsManager.Instance.GetForSave();
        }
    }
}
