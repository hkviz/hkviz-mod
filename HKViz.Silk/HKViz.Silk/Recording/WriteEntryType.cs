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
    PlayerDataBool       = 0x11,
    PlayerDataInt        = 0x12,
    PlayerDataFloat      = 0x13,
    PlayerDataString      = 0x14,
    PlayerDataGuid        = 0x15,
}
