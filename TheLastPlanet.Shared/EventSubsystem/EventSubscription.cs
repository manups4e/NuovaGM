using System;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Shared.Internal.Events
{
    
    public class EventSubscription
    {
        public Snowflake Id { get; set; }
        public string Endpoint { get; set; }
        public Delegate Delegate { get; set; }

        public EventSubscription(string endpoint, Delegate @delegate)
		{
			Id = Snowflake.Next();
            Endpoint = endpoint;
            Delegate = @delegate;
        }
    }
}