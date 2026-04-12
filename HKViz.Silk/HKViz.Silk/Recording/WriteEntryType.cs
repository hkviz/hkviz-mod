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
    PlayerDataWrappedVector2ListAppend = 0x20,
}
