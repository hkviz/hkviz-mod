namespace HKViz {
    internal static class Constants {
        public static string WEBSITE_DISPLAY_LINK = "hkviz.org";
        public static string WEBSITE_LINK = "https://www.hkviz.org";

        // ----- PRODUCTION -----
        public static string API_URL = "https://www.hkviz.org/api/rest/";
        public static string LOGIN_URL = "https://www.hkviz.org/ingameauth/";
        public static string API_URL_SUFFIX = "";

        // ----- LOCAL -----
        //public static string API_URL = "http://localhost:3000/api/rest/";
        //public static string LOGIN_URL = "http://localhost:3000/ingameauth/";
        //public static string API_URL_SUFFIX = "";

        public static string RECORDER_FILE_VERSION = "1.6.0";

        public static string GetVersion() => typeof(Constants).Assembly.GetName().Version.ToString();
    }
}
