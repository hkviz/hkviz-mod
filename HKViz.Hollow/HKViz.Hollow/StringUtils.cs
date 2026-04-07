using System;

namespace HKViz {
    internal static class StringUtils {
        public static string ReplaceNewLines(this String value, String replacer) => value
            .Replace("\r\n", replacer)
            .Replace("\n", replacer)
            .Replace("\r", replacer);
    }
}
