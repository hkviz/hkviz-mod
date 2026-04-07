using System;

namespace HKViz {
    internal static class ReflectionUtils {
        public static object GetFieldByReflection(this Object self, string fieldName) {
            return self.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .GetValue(self);
        }
        public static void SetFieldByReflection(this Object self, string fieldName, object value) {
            self.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .SetValue(self, value);
        }

    }
}
