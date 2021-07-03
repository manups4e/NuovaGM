using CitizenFX.Core;
using TheLastPlanet.Server.Core;
using System;
using CitizenFX.Core.Native;
using Logger;
using System.Linq;
using Newtonsoft.Json;
using TheLastPlanet.Shared.Snowflakes;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.manager
{
	static class ServerManager
	{
		public static void Init()
		{
			Server.Instance.Events.Mount("manager:TeletrasportaDaMe", new Action<ClientId, int>(TippaDaMe));
			//Server.Instance.AddEventHandler("entityCreated", new Action<int>(EntityCreating));
		}

		private static void TippaDaMe(ClientId client, int source)
		{
			var player = Funzioni.GetClientFromPlayerId(source);
			player.Ped.Position = client.Ped.Position + new Vector3(0, 1f, 0);
			player.Ped.Rotation = client.Ped.Rotation + new Vector3(0, 0, 180f);
		}

		private static async void EntityCreating(int entity)
		{
			try
			{
				if (entity != 0 && API.DoesEntityExist(entity))
				{
					EntityCreated ent = new(entity);
					if (ent.PopulationType == PopulationType.Unknown ||
						ent.PopulationType == PopulationType.RandomAmbient ||
						ent.PopulationType == PopulationType.RandomParked ||
						ent.PopulationType == PopulationType.RandomPatrol ||
						ent.PopulationType == PopulationType.RandomPermanent ||
						ent.PopulationType == PopulationType.RandomScenario) return;
					await BaseScript.Delay(5000);
					Server.Logger.Debug(ent.ToString());
					if (ent.Decor != Snowflake.Empty) return;
					if (ent.Type == typeof(Vehicle))
					{
					}
					else if (ent.Type == typeof(Prop))
					{
					}
					else if (ent.Type == typeof(Ped))
					{
					}
					Server.Logger.Warning( $"Il Player {ent.Owner.Name} ha spawnato un entità con un mod Menu");
					//ent.Owner.Drop($"Hai spawnato un {ent.Type.Name} vietato [{(ObjectHash)ent.Entity.Model}]");
					API.DeleteEntity(ent.Handle);
				}
			}
			catch (Exception e)
			{
				Server.Logger.Debug(e.ToString());
			}
		}
	}

	public enum PopulationType
	{
		Unknown = 0,
		RandomPermanent,
		RandomParked,
		RandomPatrol,
		RandomScenario,
		RandomAmbient,
		Permanent,
		Mission,
		Replay,
		Cache,
		Tool
	}

	public class EntityCreated
	{
		public int Handle { get; set; }
		public Type Type { get; set; }
		public Player Owner { get; set; }
		public Player FirstOwner { get; set; }
		public Entity Entity { get; set; }
		public PopulationType PopulationType { get; set; }
		public Snowflake Decor { get; set; }
		public EntityCreated(int entity)
		{
			Handle = entity;
			Entity = Entity.FromHandle(Handle);
			PopulationType = (PopulationType)API.GetEntityPopulationType(Handle);
			Type = Entity.Type == 1 ? typeof(Ped) : Entity.Type == 2 ? typeof(Vehicle) : Entity.Type == 3 ? typeof(Prop) : null;
			Owner = Server.Instance.GetPlayers.FirstOrDefault(x=>x.Handle == API.NetworkGetEntityOwner(Handle).ToString());
			FirstOwner = Server.Instance.GetPlayers.FirstOrDefault(x => x.Handle == API.NetworkGetFirstEntityOwner(Handle).ToString());
			Decor = Entity.State["decor"] != null ? Snowflake.Parse(Convert.ToUInt64(Entity.State["decor"].decorator)) : Snowflake.Empty;
		}

		public override string ToString()
		{
			return $"Handle = {Handle}, PopulationType = {PopulationType}, owner = {Owner.Name}, tipo = {Type.Name}, decor = {Decor.ToInt64()}, coordinate = {Entity.Position}";
		}
	}
}