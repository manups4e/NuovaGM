using System;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
    [Serialization]
    public partial class WorldEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan CountdownTime { get; set; }
        public TimeSpan EventTime { get; set; }
        public float EventXpMultiplier { get; set; } = 1.0f;
        public bool IsActive { get; set; } = false;
        public bool IsStarted { get; set; } = false;
    }
}
