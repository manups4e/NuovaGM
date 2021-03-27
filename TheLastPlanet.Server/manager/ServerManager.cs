using CitizenFX.Core;
using TheLastPlanet.Server.Core;
using System;
using CitizenFX.Core.Native;
using Logger;
using System.Linq;
using Newtonsoft.Json;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Server.manager
{
	static class ServerManager
	{
		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:manager:TeletrasportaDaMe", new Action<int, Vector3>(TippaDaMe));
			Server.Instance.AddEventHandler("entityCreated", new Action<int>(EntityCreating));
		}

		private static void TippaDaMe(int source, Vector3 coords)
		{
			Funzioni.GetPlayerFromId(source).TriggerEvent("lprp:manager:TeletrasportaDaMe", coords);
		}

		private static async void EntityCreating(int entity)
		{
			try
			{
				if (entity != 0 && API.DoesEntityExist(entity))
				{
					EntityCreated ent = new(entity);
					if (API.HasVehicleBeenOwnedByPlayer(entity))
					{
						await BaseScript.Delay(1000);
						Log.Printa(LogType.Debug, ent.ToString());
						if (ent.Decor != null && ent.Decor != Snowflake.Empty) return;
						else
						{
							Log.Printa(LogType.Warning, $"Il Player {ent.Owner.Name} ha spawnato un entità con un mod Menu");
							ent.Owner.Drop("Hai spawnato un veicolo vietato");
							//drop player;
						}
					}
				}
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Debug, e.ToString());
			}
		}
	}

	public class EntityCreated
	{
		public int Handle { get; set; }
		public Type Type { get; set; }
		public Player Owner { get; set; }
		public Player FirstOwner { get; set; }
		public Entity Entity { get; set; }
		public int PopulationType { get; set; }
		public Snowflake Decor { get; set; }
		public EntityCreated(int entity)
		{
			Handle = entity;
			Entity = Entity.FromHandle(Handle);
			PopulationType = API.GetEntityPopulationType(Handle);
			Type = Entity.Type == 1 ? typeof(Ped) : Entity.Type == 2 ? typeof(Vehicle) : Entity.Type == 3 ? typeof(Prop) : null;
			Owner = Server.Instance.GetPlayers.FirstOrDefault(x=>x.Handle == API.NetworkGetEntityOwner(Handle).ToString());
			FirstOwner = Server.Instance.GetPlayers.FirstOrDefault(x => x.Handle == API.NetworkGetFirstEntityOwner(Handle).ToString());
			Decor = Entity.State["decor"] != null ? Snowflake.Parse(Convert.ToUInt64(Entity.State["decor"].decorator)) : Snowflake.Empty;
		}

		public override string ToString()
		{
			return $"Handle = {Handle}, owner = {Owner.Name}, tipo = {Type.Name}, decor = {Decor.ToInt64()}, coordinate = {Entity.Position}";
		}
	}
}