using GlobalEnums;
using InControl;

namespace HKViz {
    internal class GameManagerWriter {
        private readonly RecordingFileManager recoding = RecordingFileManager.Instance;

        private static GameManagerWriter? _instance;
        public static GameManagerWriter Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new GameManagerWriter();
                return _instance;
            }
        }

        private GameManager gm;
        private GameState? previousGameState;
        private string? previousActiveDeviceName;

        public void InitFromLocalSave() {
            previousActiveDeviceName = null;
            previousGameState = null;
        }

        public void WriteChangedFields(long unixMillis) {
            if (!recoding.isRecording) return;

            gm ??= GameManager.instance;

            if (gm.gameState != previousGameState) {
                recoding.WriteEntry(RecordingPrefixes.GAME_STATE, gm.gameState switch {
                    GameState.INACTIVE => "i",
                    GameState.MAIN_MENU => "m",
                    GameState.LOADING => "l",
                    GameState.ENTERING_LEVEL => "e",
                    GameState.PLAYING => "p",
                    GameState.PAUSED => "a",
                    GameState.EXITING_LEVEL => "x",
                    GameState.CUTSCENE => "c",
                    GameState.PRIMER => "r",
                    _ => gm.gameState.ToString(),
                }, unixMillis: unixMillis);
                previousGameState = gm.gameState;
            }

            var activeDeviceName = InputManager.ActiveDevice.Name;
            if (previousActiveDeviceName != activeDeviceName) {
                recoding.WriteEntry(RecordingPrefixes.ACTIVE_INPUT_DEVICE, activeDeviceName, unixMillis: unixMillis);
                previousActiveDeviceName = activeDeviceName;
            }
        }
    }
}
