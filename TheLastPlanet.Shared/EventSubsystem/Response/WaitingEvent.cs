using System;

using TheLastPlanet.Shared.Internal.Events.Message;

namespace TheLastPlanet.Shared.Internal.Events.Response
{
    
    public class WaitingEvent
    {
        public EventMessage Message { get; set; }
        public Action<string> Callback { get; set; }

        public WaitingEvent(EventMessage message, Action<string> callback)
        {
            Message = message;
            Callback = callback;
        }
    }
}