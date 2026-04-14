using System;

namespace HKViz.Shared;

public static class DateTimeUtils {
    public static long GetUnixMillis() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public static long GetUnixSeconds() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
}
