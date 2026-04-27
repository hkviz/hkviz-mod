namespace HKViz.Silk.Recording;

public enum WriteEntryType : byte {
    // Header
    // does not use entry types
    
    // File Meta
    SessionStart           = 0x03,
    SessionEnd             = 0x05,
    
    
    // Hero
    HeroLocation           = 0x01,

    // Scene
    SceneChangeSingle      = 0x02,
    SceneChangeAdd         = 0x04,
    SceneBoundary          = 0x06,

    // Time
    TimestampFull          = 0x07,
    TimestampBackwards     = 0x08,
    TimestampAddByte       = 0x09, // <= 256ms --> should almost always use this if we have >= 4 write fps
    TimestampAddShort      = 0x10, // < 65,536 --> even if paused for roughly a minute can use diff
    
    // PlayerData
    PlayerDataBool       = 0x0A,
    PlayerDataInt        = 0x0B,
    PlayerDataFloat      = 0x0C,
    PlayerDataString     = 0x0D,
    PlayerDataGuid       = 0x0E,
    PlayerDataEnum       = 0x0F,
    PlayerDataULong      = 0x11,
    PlayerDataVector3    = 0x12,
    PlayerDataVector2    = 0x13,
    PlayerDataIntListFull = 0x14,
    PlayerDataIntListDelta = 0x15,
    PlayerDataStringListFull = 0x16,
    PlayerDataStringListDelta = 0x17,
    PlayerDataStringSetFull = 0x18,
    PlayerDataStringSetDelta = 0x19,
    PlayerDataNamedMapFull = 0x1A,
    PlayerDataNamedMapDelta = 0x1B,
    PlayerDataStoryEventListFull = 0x1C,
    PlayerDataStoryEventListDelta = 0x1D,
    PlayerDataWrappedVector2ListFull = 0x1E,
    PlayerDataWrappedVector2ListDelta = 0x1F,
    
    // SceneData
    SceneDataBool       = 0x21,
    SceneDataInt        = 0x22,
    SceneDataGeoRock    = 0x23,
    
    // Freed up:
    // PlayerDataWrappedVector2ListAppend = 0x20,
    
    
    
    
    
    // MISSING FROM HOLLOW KNIGHT MOD:
    
    // public static readonly string ENTITY_POSITIONS = ""; // no prefix = positions
    //
    // public static readonly string SPELL_FIREBALL = "sfb";
    // public static readonly string SPELL_UP = "sup";
    // public static readonly string SPELL_DOWN = "sdn";
    //
    // public static readonly string FOCUS_START = "f";
    // public static readonly string FOCUS_CANCELED = "Fc";
    // public static readonly string FOCUS_SUCCESS = "F";
    //
    // public static readonly string NAIL_ART_CYCLONE = "nc";
    // public static readonly string NAIL_ART_D_SLASH = "nd";
    // public static readonly string NAIL_ART_G_SLASH = "ng";
    //
    // public static readonly string SUPER_DASH = "sd";
    //
    // public static readonly string DREAM_NAIL_SLASH = "dn";
    // public static readonly string DREAM_NAIL_GATE_WARP = "dnw";
    // public static readonly string DREAM_NAIL_SET_GATE = "dns";
    //
    // public static readonly string ENEMY_START = "e";
    // public static readonly string ENEMY_HEALTH = "E";
    //
    // public static readonly string TAKE_DAMAGE_CALLED = "d";
    // public static readonly string TAKE_HEALTH_CALLED = "h";
    //
    // public static readonly string GAME_STATE = "gs";
    
    
    // WONT DO
    // public static readonly string ACTIVE_INPUT_DEVICE = "aid";
    // public static readonly string MODDING_INFO = "mi";
}
