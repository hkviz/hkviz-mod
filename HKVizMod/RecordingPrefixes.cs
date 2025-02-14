﻿namespace HKViz {
    internal static class RecordingPrefixes {
        // must not be the start of another prefix
        public static readonly string PLAYER_DATA_SHORTNAME = "$";
        public static readonly string PLAYER_DATA_LONGNAME = "§";

        public static readonly string HERO_CONTROLLER_STATE_SHORTNAME = "%";
        public static readonly string HERO_CONTROLLER_STATE_LONGNAME = "&";

        // safe to have other prefixes with same start
        public static readonly string ENTITY_POSITIONS = ""; // no prefix = positions
        public static readonly string ROOM_DIMENSIONS = "S";
        public static readonly string SCENE_CHANGE = "s";
        public static readonly string HZVIZ_MOD_VERSION = "vizmodv";
        public static readonly string MODDING_INFO = "mi";
        public static readonly string RECORDING_FILE_VERSION = "rfv";
        public static readonly string RECORDING_ID = "rid";
        public static readonly string SESSION_END = "send";

        public static readonly string SPELL_FIREBALL = "sfb";
        public static readonly string SPELL_UP = "sup";
        public static readonly string SPELL_DOWN = "sdn";

        public static readonly string FOCUS_START = "f";
        public static readonly string FOCUS_CANCELED = "Fc";
        public static readonly string FOCUS_SUCCESS = "F";

        public static readonly string NAIL_ART_CYCLONE = "nc";
        public static readonly string NAIL_ART_D_SLASH = "nd";
        public static readonly string NAIL_ART_G_SLASH = "ng";

        public static readonly string SUPER_DASH = "sd";

        public static readonly string DREAM_NAIL_SLASH = "dn";
        public static readonly string DREAM_NAIL_GATE_WARP = "dnw";
        public static readonly string DREAM_NAIL_SET_GATE = "dns";

        public static readonly string ENEMY_START = "e";
        public static readonly string ENEMY_HEALTH = "E";

        public static readonly string TAKE_DAMAGE_CALLED = "d";
        public static readonly string TAKE_HEALTH_CALLED = "h";

        public static readonly string GAME_STATE = "gs";
        public static readonly string ACTIVE_INPUT_DEVICE = "aid";
    }
}
