using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.Core
{
	public enum BucketLockdownMode
	{
		strict,
		relaxed,
		inactive
	}

	internal static class BucketsHandler
	{
		public static List<Bucket> Buckets;

		public static void Init()
		{
			Buckets = new List<Bucket>() { new(0, "RolePlay") { PopulationEnabled = true, LockdownMode = BucketLockdownMode.relaxed }, new(1, "LogIn") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict }, new(2, "Minigames chooser") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict } };
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
		public BucketLockdownMode LockdownMode
		{
			set => _setBucketLockdownMode(value);
		}
		public bool PopulationEnabled
		{
			set => _enablePopulation(value);
		}

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

		private void _setBucketLockdownMode(BucketLockdownMode mode)
		{
			switch (mode)
			{
				case BucketLockdownMode.strict:
					API.SetRoutingBucketEntityLockdownMode(Id, "strict");

					break;
				case BucketLockdownMode.relaxed:
					API.SetRoutingBucketEntityLockdownMode(Id, "relaxed");

					break;
				case BucketLockdownMode.inactive:
					API.SetRoutingBucketEntityLockdownMode(Id, "inactive");

					break;
			}
		}

		private void _enablePopulation(bool enabled)
		{
			API.SetRoutingBucketPopulationEnabled(Id, enabled);
		}
	}
}