namespace HKViz.Silk.Recording;

public static class WriteEntryType {
    public const byte HeroLocation = 0x01;
    public const byte SceneChangeSingleShort = 0x02;
    public const byte SceneChangeSingleLong = 0x03;
    public const byte SceneChangeAddShort = 0x04;
    public const byte SceneChangeAddLong = 0x05;
    public const byte SceneBoundary = 0x06;
}