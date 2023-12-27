﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsRecorder {
    internal static class BaseNConverter {
        public static string ConvertTo(int @base, string chars, int value) {
            string result = "";

            while (value > 0) {
                result = chars[value % @base] + result; // use StringBuilder for better performance
                value /= @base;
            }

            return result;
        }

        public static string ConvertToBase36(int value)
            => ConvertTo(
                @base: 36,
                chars: "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                value: value
            );
    }
}
