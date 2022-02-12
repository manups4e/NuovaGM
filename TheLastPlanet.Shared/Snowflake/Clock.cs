using System;

namespace TheLastPlanet.Shared.Snowflakes
{

    public static class Clock
    {
        public static long GetMilliseconds() => (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }
}