namespace HKViz.Shared;

#if MOD_SILK
using HKViz.Silk;
#endif

internal static class HkVizSharedConstants {
    public static string WEBSITE_DISPLAY_LINK = "hkviz.org";
    public static string WEBSITE_LINK = "https://www.hkviz.org";

    // ----- PRODUCTION -----
    public static string API_URL = "https://www.hkviz.org/api/rest/";
    public static string LOGIN_URL = "https://www.hkviz.org/ingameauth/";
    public static string API_URL_SUFFIX = "";

    // ----- LOCAL -----
    // public static string API_URL = "http://localhost:3000/api/rest/";
    // public static string LOGIN_URL = "http://localhost:3000/ingameauth/";
    // public static string API_URL_SUFFIX = "";
    
    #if MOD_SILK
    public static string GetModVersion() => HkVizSilkPlugin.Version;
    #elif MOD_HOLLOW
    public static string GetModVersion() => typeof(HkVizSharedConstants).Assembly.GetName().Version.ToString();
    #endif
}