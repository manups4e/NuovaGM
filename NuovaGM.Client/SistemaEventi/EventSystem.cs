using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Shared.SistemaEventi;

namespace TheLastPlanet.Client.SistemaEventi
{
	public class EventSystem
	{
		public List<EventAttachment> Attachments { get; } = new();
		public List<EventRequest> PendingRequests { get; } = new();

		public EventSystem()
		{
			Client.Instance.AddEventHandler("1d446f5702fcd00055ac8b8544479b0e", new Action<string>(payload =>
			{
				Event wrapped = JsonConvert.DeserializeObject<Event>(payload.ToString());

				switch (wrapped.Type)
				{
					case EventType.Request:
					{
						bool firewall = true;

						foreach (EventAttachment attachment in Attachments.Where(self => self.Target == wrapped.Target))
						{
							if (attachment.Callback.GetType() == typeof(AsyncEventCallback))
							{
								wrapped.Type = EventType.Response;
								wrapped.Metadata.Write("__response", JsonConvert.SerializeObject(((AsyncEventCallback)attachment.Callback).AsyncTask(wrapped.Metadata)));
								Send(wrapped);
							}
							else
							{
								wrapped.Type = EventType.Response;
								wrapped.Metadata.Write("__response", JsonConvert.SerializeObject(attachment.Callback.Task(wrapped.Metadata)));
								Send(wrapped);
							}

							firewall = false;

							break;
						}

						if (firewall) Log.Printa(LogType.Error, $"[{wrapped.Seed}] [{wrapped.Target}] [FIREWALL] Request did not get managed by the attacher.");

						break;
					}
					case EventType.Response:
					{
						List<EventRequest> found = PendingRequests.Where(self => self.Seed == wrapped.Seed).ToList();
						found.ForEach(self => self.Callback.Task(wrapped.Metadata));
						PendingRequests.RemoveAll(self => self.Seed == wrapped.Seed);

						break;
					}
					case EventType.Send:
					{
						foreach (EventAttachment attachment in Attachments.Where(self => self.Target == wrapped.Target))
							if (attachment.Callback.GetType() == typeof(AsyncEventCallback))
							{
#pragma warning disable 4014
								((AsyncEventCallback)attachment.Callback).AsyncTask(wrapped.Metadata);
#pragma warning restore 4014
							}
							else
							{
								attachment.Callback.Task(wrapped.Metadata);
							}

						break;
					}
				}
			}));
		}

		public void Send(string target, params object[] payloads)
		{
			Send(Construct(target, payloads));
		}

		public void Send(string target)
		{
			Send(Construct(target, null));
		}

		public void Send(Event wrapped)
		{
			Log.Printa(LogType.Debug, $"[{wrapped.Seed}] [{wrapped.Target}] Dispatching `{wrapped.Type}` operation to the server-side.");
			BaseScript.TriggerServerEvent("1d446f5702fcd00055ac8b8544479b0e", Cache.Player.ServerId, JsonConvert.SerializeObject(wrapped));
		}

		public Event Construct(string target, object[] payloads)
		{
			Event wrapped = new() { Target = target, Sender = Cache.Player.ServerId };
			if (payloads != null) WriteMetadata(wrapped, payloads);

			return wrapped;
		}

		public void WriteMetadata(Event wrapped, IEnumerable<object> payloads)
		{
			int index = 0;

			foreach (object payload in payloads)
			{
				wrapped.Metadata.Write(index, payload);
				index++;
			}
		}

		public async Task<T> Request<T>(string target, params object[] payloads) where T : new()
		{
			T response = default;
			bool completed = false;
			EventRequest wrapped = new(new EventCallback(metadata =>
			{
				//Log.Printa(LogType.Debug, $"[{metadata.Inherit}] Got request response from server-side with metadata {JsonConvert.SerializeObject(metadata)}");

				try
				{
					response = JsonConvert.DeserializeObject<T>(metadata.Find<string>("__response"));
				}
				catch (Exception e)
				{
					Log.Printa(LogType.Error, $"[{metadata.Inherit}] Event request response returned an invalid type.\n " + e);
				}

				completed = true;

				return response;
			})) { Target = target, Sender = API.GetPlayerServerId(API.PlayerId()) };
			if (payloads != null && payloads.Length > 0) WriteMetadata(wrapped, payloads);
			Send(wrapped);
			PendingRequests.Add(wrapped);
			while (!completed) await BaseScript.Delay(10);

			return response;
		}

		public async Task<T> Request<T>(string target) where T : new()
		{
			return await Request<T>(target, null);
		}

		public void Attach(string target, EventCallback callback)
		{
			Attachments.Add(new EventAttachment(target, callback));
		}
	}
}