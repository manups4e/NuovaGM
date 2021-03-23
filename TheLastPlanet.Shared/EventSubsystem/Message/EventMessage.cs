using System.Linq;

using TheLastPlanet.Shared.Internal.Events.Payload;
using TheLastPlanet.Shared;
using Newtonsoft.Json;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Shared.Internal.Events.Message
{
    
    public class EventMessage : ISerializable
    {
        public Snowflake Id { get; set; }
        public string Signature { get; set; }
        public string Endpoint { get; set; }
        public EventMethodType MethodType { get; set; }
        public string Parameters { get; set; }
        public object[] PreservedParameters { get; set; }

        public EventMessage(string endpoint, EventMethodType methodType, object[] parameters)
        {
            Id = Snowflake.Next();
            Endpoint = endpoint;
            MethodType = methodType;
            Parameters = parameters.Select(self => new EventParameter(self)).ToJson();
            PreservedParameters = parameters;
        }

        [JsonConstructor]
        public EventMessage(Snowflake id, string signature, string endpoint, EventMethodType methodType, string parameters)
        {
            Id = id;
            Signature = signature;
            Endpoint = endpoint;
            MethodType = methodType;
            Parameters = parameters;
        }

        public string Serialize()
        {
            return this.ToJson();
        }

        public static EventMessage Deserialize(string serialized)
        {
            return serialized.FromJson<EventMessage>();
        }
    }
}