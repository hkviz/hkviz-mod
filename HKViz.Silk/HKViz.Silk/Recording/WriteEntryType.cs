namespace HKViz.Silk.Recording;

public enum WriteEntryType : byte {
    // Hero
    HeroLocation = 0x01,

    // Scene
    SceneChangeSingleShort = 0x02,
    SceneChangeSingleLong  = 0x03,
    SceneChangeAddShort    = 0x04,
    SceneChangeAddLong     = 0x05,
    SceneBoundary          = 0x06,

    // Time
    TimestampFull          = 0x07,
    TimestampBackwards     = 0x08,
    TimestampAddByte       = 0x09, // <= 256ms --> should almost always use this if we have >= 4 write fps
    TimestampAddShort      = 0x10, // < 65,536 --> even if paused for roughly a minute can use diff
    
    // HeroControllerStates
    // TODO
    
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
    PlayerDataIntList   = 0x14,
    PlayerDataStringList = 0x15, // used for lists and arrays
    PlayerDataStringSet  = 0x16,
}
