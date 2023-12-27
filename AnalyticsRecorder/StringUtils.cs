using HKMirror.Reflection.InstanceClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsRecorder {
    internal static class StringUtils {
        public static string ReplaceNewLines(this String value, String replacer) => value
            .Replace("\r\n", replacer)
            .Replace("\n", replacer)
            .Replace("\r", replacer);
    }
}
