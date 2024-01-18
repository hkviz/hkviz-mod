using HKMirror.Hooks.ILHooks;
using HKViz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Modding;
using System.Collections;
using System.Diagnostics.Contracts;
using static Mono.Security.X509.X520;
using GlobalEnums;

namespace HKViz {
    internal class ModWriter {
        private record ModWrittenState(
            string Name,
            string Version,
            bool Enabled,
            string Error
        );

        private static ModWriter _instance;

        public static ModWriter Instance {
            get {
                if (_instance == null) {
                    _instance = new ModWriter();
                }
                return _instance;
            }
        }

        private RecordingFileManager recording = RecordingFileManager.Instance;
        private RecordingSerializer serializer = RecordingSerializer.Instance;

        private List<ModWrittenState> writtenStates = new List<ModWrittenState>();

        private Assembly moddingAssembly;
        private Type modLoaderType;
        private PropertyInfo modLoaderModInstanceProperty;

        private Type modInstanceType;
        private FieldInfo modInstanceNameField;
        private FieldInfo modInstanceModField;
        private FieldInfo modInstanceEnabledField;
        private FieldInfo modInstanceErrorField;

        public ModWriter() {
            moddingAssembly = typeof(IMod).Assembly;
            modLoaderType = moddingAssembly.GetType("Modding.ModLoader");
            modLoaderModInstanceProperty = modLoaderType.GetProperty("ModInstances", BindingFlags.Static | BindingFlags.Public);

            modInstanceType = modLoaderType.GetNestedType("ModInstance");
            modInstanceNameField = modInstanceType.GetField("Name", BindingFlags.Public | BindingFlags.Instance);
            modInstanceModField = modInstanceType.GetField("Mod", BindingFlags.Public | BindingFlags.Instance);
            modInstanceEnabledField = modInstanceType.GetField("Enabled", BindingFlags.Public | BindingFlags.Instance);
            modInstanceErrorField = modInstanceType.GetField("Error", BindingFlags.Public | BindingFlags.Instance);
        }

        private GameState previousGameState;

        public void OnKnightUpdate() {
            var currentGameState = GameManager.instance.gameState;
            if (currentGameState != previousGameState && currentGameState == GameState.PAUSED) {
                WriteModsIfChanged();
            }
            previousGameState = currentGameState;
        }

        public void OnRecordingInit() {
            writtenStates.Clear();
            WriteModsIfChanged();
        }

        public void WriteModsIfChanged() {
            if (!recording.isRecording) return;

            var mods = modLoaderModInstanceProperty.GetValue(modLoaderType, null) as IEnumerable;

            var newWriteStates = new List<ModWrittenState>();
            foreach (var modInstance in mods) {
                var name = (string)modInstanceNameField.GetValue(modInstance);
                var mod = (IMod)modInstanceModField.GetValue(modInstance);
                var version = mod.GetVersion();
                var enabled = (bool)modInstanceEnabledField.GetValue(modInstance);
                var error = modInstanceErrorField.GetValue(modInstance)?.ToString();
                var errorShort = error switch {
                    "Construct" => "c",
                    "Duplicate" => "d",
                    "Initialize" => "i",
                    "Unload" => "u",
                    null => "n",
                    _ => error,
                };

                newWriteStates.Add(new ModWrittenState(
                    Name: name,
                    Version: version,
                    Enabled: enabled,
                    Error: errorShort
                ));
            }

            if (!newWriteStates.Equals(writtenStates)) {
                writtenStates = newWriteStates;
                recording.WriteEntryPrefix(RecordingPrefixes.MODDING_INFO);

                recording.Write("Modding API");
                recording.Write(":");
                recording.Write(ModHooks.ModVersion);

                foreach (var mod in writtenStates) {
                    recording.WriteSep();
                    recording.Write(mod.Name);
                    recording.Write(":");
                    recording.Write(mod.Version);
                    recording.Write(":");
                    recording.Write(serializer.serialize(mod.Enabled));
                    recording.Write(mod.Error);
                }
            }
        }
    }
}