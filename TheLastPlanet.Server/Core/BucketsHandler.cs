using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.Core
{
	internal static class BucketsHandler
	{
		public static List<Bucket> Buckets = new() { new Bucket(0, "RolePlay"), new Bucket(1, "LogIn"), new Bucket(2, "Minigames chooser") };

		public static void Init()
		{
			Server.Instance.Events.Mount("lprp:addPlayerToBucket", new Action<ClientId, int>(AddPlayerToBucket));
			Server.Instance.Events.Mount("lprp:addEntityToBucket", new Action<int, int>(AddEntityToBucket));
		}

		private static void AddPlayerToBucket(ClientId player, int id)
		{
			Bucket bucket = Buckets.SingleOrDefault(x => x.Id == id);
			bucket?.AddPlayer(player.Player);
		}

		private static void AddEntityToBucket(int entity, int id)
		{
			Bucket bucket = Buckets.SingleOrDefault(x => x.Id == id);
			bucket?.AddEntity(entity);
		}
	}

	public class Bucket
	{
		public int Id;
		public string Name;
		public List<Player> Players = new();
		public List<Entity> Entities = new();

		public Bucket(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public void AddPlayer(Player player)
		{
			if (!Players.Contains(player))
			{
				Players.Add(player);
				if (API.GetPlayerRoutingBucket(player.Handle) != Id) API.SetPlayerRoutingBucket(player.Handle, Id);
			}
		}

		public void AddEntity(Entity entity)
		{
			if (!Entities.Contains(entity))
			{
				Entities.Add(entity);
				if (API.GetEntityRoutingBucket(entity.Handle) != Id) API.SetEntityRoutingBucket(entity.Handle, Id);
			}
		}

		public async void AddEntity(int entityNetworkId)
		{
			if (Entities.All(x => x.Handle != entityNetworkId))
			{
				Entity ent = Entity.FromNetworkId(entityNetworkId);
				while (ent == null) await BaseScript.Delay(0);
				Entities.Add(ent);
				if (API.GetEntityRoutingBucket(ent.Handle) != Id) API.SetEntityRoutingBucket(ent.Handle, Id);
			}
		}

		public Player GetPlayer(Player player)
		{
			return Players.FirstOrDefault(x => x.Handle == player.Handle);
		}

		public Entity GetEntity(Entity entity)
		{
			return Entities.FirstOrDefault(x => x.Handle == entity.Handle);
		}
	}
}
