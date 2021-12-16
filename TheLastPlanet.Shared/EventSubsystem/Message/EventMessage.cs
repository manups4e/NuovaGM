using TheLastPlanet.Shared.Internal.Events.Payload;
using TheLastPlanet.Shared.Snowflakes;
using System.Collections.Generic;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared.Internal.Events.Message
{
    [Serialization]
    public partial class EventMessage : IMessage
    {
        public Snowflake Id { get; set; }
        public string? Signature { get; set; }
        public string? Endpoint { get; set; }
        public EventFlowType Flow { get; set; }
        public IEnumerable<EventParameter> Parameters { get; set; }
        public EventMessage() { }
        public EventMessage(string endpoint, EventFlowType flow, IEnumerable<EventParameter> parameters)
        {
            Id = Snowflake.Next();
            Endpoint = endpoint;
            Flow = flow;
            Parameters = parameters;
        }
        public override string ToString() => Endpoint;
    }
}