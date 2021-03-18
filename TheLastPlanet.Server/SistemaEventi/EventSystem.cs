using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.SistemaEventi;

namespace TheLastPlanet.Server.SistemaEventi
{
	public class EventSystem
	{
		public List<EventAttachment> Attachments { get; } = new();
		public List<EventRequest> PendingRequests { get; } = new();

		public EventSystem()
		{
			ServerSession.Instance.AddEventHandler("1d446f5702fcd00055ac8b8544479b0e", new Action<int, string>((handle, payload) =>
			{
				Event wrapped = JsonConvert.DeserializeObject<Event>(payload.ToString());
				wrapped.Sender = handle;
				wrapped.Metadata.Sender = handle;

				try
				{
					switch (wrapped.Type)
					{
						case EventType.Request:
						{
							bool firewall = true;

							foreach (EventAttachment attachment in Attachments.Where(self => self.Target == wrapped.Target))
							{
								if (attachment.Callback.GetType() == typeof(AsyncEventCallback))
								{
									Task.Factory.StartNew(async () =>
									{
										wrapped.Type = EventType.Response;
										wrapped.Metadata.Write("__response", (await ((AsyncEventCallback)attachment.Callback).AsyncTask(wrapped.Metadata)).ToJson());
										Send(wrapped, handle.ToString());
									});
								}
								else
								{
									wrapped.Type = EventType.Response;
									wrapped.Metadata.Write("__response", (attachment.Callback.Task(wrapped.Metadata)).ToJson());
									Send(wrapped, handle.ToString());
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
									Task.Factory.StartNew(async () =>
									{
										await ((AsyncEventCallback)attachment.Callback).AsyncTask(wrapped.Metadata);
									});
								else
									attachment.Callback.Task(wrapped.Metadata);

							break;
						}
					}
				}
				catch (Exception ex)
				{
					Log.Printa(LogType.Error, $"[Events] An error occured when handling event `{wrapped.Target}`: {ex}");
				}
			}));
		}

		public void Send(string target, string handle, params object[] payloads)
		{
			Send(Construct(target, Convert.ToInt32(handle), payloads), handle);
		}

		public void Send(string target, params object[] payloads)
		{
			Send(Construct(target, -1, payloads), "-1");
		}

		public void Send(string target, string handle)
		{
			Send(Construct(target, Convert.ToInt32(handle), null), handle);
		}

		public void Send(string target)
		{
			Send(Construct(target, -1, null), "-1");
		}

		public void Send(Event wrapped, string handle)
		{
			if (handle != "-1")
			{
				Player player = Funzioni.GetPlayerFromId(handle);
				//Log.Printa(LogType.Debug, $"[{wrapped.Seed}] [{wrapped.Target}] Dispatching `{wrapped.Type}` operation to the client `{handle}`.");
				player.TriggerEvent("1d446f5702fcd00055ac8b8544479b0e", JsonConvert.SerializeObject(wrapped));
			}
			else
			{
				//Log.Printa(LogType.Debug, $"[{wrapped.Seed}] [{wrapped.Target}] Dispatching `{wrapped.Type}` operation to every client.");
				BaseScript.TriggerClientEvent("1d446f5702fcd00055ac8b8544479b0e", JsonConvert.SerializeObject(wrapped));
			}
		}

		public Event Construct(string target, int handle, object[] payloads)
		{
			Event wrapped = new() { Target = target, Sender = -1 };
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

		public async Task<T> Request<T>(string target, string handle, params object[] payloads) where T : new()
		{
			T response = default;
			ThreadLock task = new();
			EventRequest wrapped = new(new EventCallback(metadata =>
			{
				//Log.Printa(LogType.Debug, $"[{metadata.Inherit}] Got request response from client `{handle}` with metadata {JsonConvert.SerializeObject(metadata)}");

				try
				{
					response = JsonConvert.DeserializeObject<T>(metadata.Find<string>("__response"));
				}
				catch (Exception)
				{
					Log.Printa(LogType.Error, $"[{metadata.Inherit}] Event request response returned an invalid type.");
				}

				task.Unlock();

				return response;
			})) { Target = target, Sender = -1 };
			if (payloads != null && payloads.Length > 0) WriteMetadata(wrapped, payloads);
			Send(wrapped, handle);
			PendingRequests.Add(wrapped);
			await task.Wait();

			return response;
		}

		public async Task<T> Request<T>(string target, string handle) where T : new()
		{
			return await Request<T>(target, handle, null);
		}

		public void Attach(string target, EventCallback callback)
		{
			//Log.Printa(LogType.Debug, $"[Events] Attaching callback to `{target}`");
			Attachments.Add(new EventAttachment(target, callback));
		}
	}
}