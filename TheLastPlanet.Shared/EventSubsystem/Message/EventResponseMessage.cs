using TheLastPlanet.Shared.Internal.Events.Attributes;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Shared.Internal.Events.Message
{
    [Serialization]
    public partial class EventResponseMessage : IMessage
    {
        public Snowflake Id { get; set; }
        public string Endpoint { get; set; }
        public string? Signature { get; set; }
        public byte[]? Data { get; set; }

        public EventResponseMessage(Snowflake id, string endpoint, string? signature, byte[]? data)
        {
            Id = id;
            Endpoint = endpoint;
            Signature = signature;
            Data = data;
        }

        public override string ToString() => Id.ToString();
    }
}