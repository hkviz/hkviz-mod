namespace HKViz;


internal static class HkVizHollowConstants {
    public static readonly string RECORDER_FILE_VERSION = "1.6.1";
    public static string GetVersion() => typeof(HkVizHollowConstants).Assembly.GetName().Version.ToString();
}
