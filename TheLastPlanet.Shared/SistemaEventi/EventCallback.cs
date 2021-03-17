using System;

namespace TheLastPlanet.Shared.SistemaEventi
{
	public class EventCallback
	{
		public Func<EventMetadata, object> Task { get; set; }

		public EventCallback(Func<EventMetadata, object> task)
		{
			Task = task;
		}
	}
}