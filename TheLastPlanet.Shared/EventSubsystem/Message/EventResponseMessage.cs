using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Shared.Internal.Events.Message
{
    
    public class EventResponseMessage : ISerializable
    {
        public Snowflake Id { get; set; }
        public string Signature { get; set; }
        public string Serialized { get; set; }

        public EventResponseMessage(Snowflake id, string signature, string serialized)
        {
            Id = id;
            Signature = signature;
            Serialized = serialized;
        }

        public string Serialize()
        {
            return this.ToJson();
        }

        public static EventResponseMessage Deserialize(string serialized)
        {
            return serialized.FromJson<EventResponseMessage>();
        }
    }
}