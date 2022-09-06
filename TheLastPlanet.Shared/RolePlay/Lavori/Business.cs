using FxEvents.Shared.Attributes;

namespace TheLastPlanet.Shared
{
    [Serialization]
    public partial class Business
    {
        public string Seed { get; set; }
        public long Balance { get; set; }
        public long Registered { get; set; }
    }
}
