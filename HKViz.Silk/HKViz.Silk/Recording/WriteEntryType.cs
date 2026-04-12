namespace HKViz.Silk.Recording;

public static class WriteEntryType {
    // 
    public const byte HeroLocation = 0x01;
    
    // Scene
    public const byte SceneChangeSingleShort = 0x02;
    public const byte SceneChangeSingleLong = 0x03;
    public const byte SceneChangeAddShort = 0x04;
    public const byte SceneChangeAddLong = 0x05;
    public const byte SceneBoundary = 0x06;
    
    // Time
    public const byte TimestampFull = 0x07;
    public const byte TimestampBackwards = 0x08;
    public const byte TimestampAddByte = 0x09; // <= 256ms --> should almost always use this if we have >= 4 write fps
    public const byte TimestampAddShort = 0x10; // < 65,536 --> even if paused for roughly a minute can use diff
}
