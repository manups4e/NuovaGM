using System;
using Newtonsoft.Json;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared.Internal.Events.Payload
{
    [Serialization]
    public partial class EventParameter
    {
        public byte[] Data { get; set; }

        public EventParameter(byte[] data)
        {
            Data = data;
        }
    }
}