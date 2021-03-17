namespace TheLastPlanet.Shared.SistemaEventi
{
	public class EventAttachment
	{
		public string Target { get; set; }
		public EventCallback Callback { get; set; }

		public EventAttachment(string target, EventCallback callback)
		{
			Target = target;
			Callback = callback;
		}
	}
}