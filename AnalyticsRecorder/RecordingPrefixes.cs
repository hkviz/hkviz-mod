using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsRecorder {
    internal static class RecordingPrefixes {
        // must not be the start of another prefix
        public static readonly string PLAYER_DATA_SHORTNAME = "$";
        public static readonly string PLAYER_DATA_LONGNAME = "§";

        public static readonly string HERO_CONTROLER_STATE_SHORTNAME = "%";
        public static readonly string HERO_CONTROLER_STATE_LONGNAME = "&";

        // safe to have other prefixes with same start
        public static readonly string PLAYER_POSITION = ""; // no prefix = position
        public static readonly string ROOM_DIMENSIONS = "S";
        public static readonly string SCENE_CHANGE = "s";
        public static readonly string HZVIZ_MOD_VERSION = "vizmodv";
        public static readonly string HOLLOWKNIGHT_VERSION = "hkv";
        public static readonly string RECORDING_FILE_VERSION = "rfv";
        public static readonly string RECORDING_ID = "rid";
        public static readonly string SESSION_END = "send";

        public static readonly string SPELL_FIREBALL = "sfb";
        public static readonly string SPELL_UP = "sup";
        public static readonly string SPELL_DOWN = "sdn";
    }
}
