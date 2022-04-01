using System;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
    [Serialization]
    public partial class ServerTime
    {
        public TimeSpan TimeOfDay { get; set; }
        public DateTime Date { get; set; }
        public int SecondOfDay { get; set; }
        public bool Frozen { get; set; }
    }

    [Serialization]
    public partial class SharedWeather
    {
        public int CurrentWeather { get; set; }
        public bool Blackout { get; set; } = false;
        public bool RainPossible { get; set; } = false;
        public int WeatherTimer { get; set; }
        public int RainTimer { get; set; }
        public float RandomWindDirection { get; set; }
        public bool DynamicMeteo { get; set; } = true;
        public bool StartUp { get; set; } = false;
        public float WindSpeed { get; set; }

        public SharedWeather() { }
    }
}
