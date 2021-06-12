using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		public static List<Bucket> Buckets = new();

		public static void Init()
		{
			Buckets.Add(new Bucket(0, "Lobby") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict });
			Buckets.Add(new Bucket(1000, "RolePlay") { PopulationEnabled = true, LockdownMode = BucketLockdownMode.relaxed });
			Buckets.Add(new Bucket(2000, "Minigames") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict });
			Buckets.Add(new Bucket(3000, "Gare") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict });
			Buckets.Add(new Bucket(4000, "Negozio") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict });
			Buckets.Add(new Bucket(5000, "FreeRoam") { PopulationEnabled = true, LockdownMode = BucketLockdownMode.strict });

			Server.Instance.Events.Mount("lprp:addPlayerToBucket", new Action<ClientId, int>(AddPlayerToBucket));
			Server.Instance.Events.Mount("lprp:addEntityToBucket", new Action<int, int>(AddEntityToBucket));
			Server.Instance.Events.Mount("lprp:richiediContoBuckets", new Func<ClientId, Task<Dictionary<int, int>>>(CountPlayers));
			Server.Instance.Events.Mount("lprp:checkSeGiaDentro", new Func<ClientId, int, Task<bool>>(CheckIn));
		}

		private static void AddPlayerToBucket(ClientId player, int id)
		{
			foreach (Bucket bucket in Buckets)
			{
				int Key = Buckets.IndexOf(bucket);
				if (Key != id && bucket.Players.Contains(player.Player)) bucket.Players.Remove(player.Player);
				if (Key == id) bucket.AddPlayer(player.Player);
			}
		}

		private static void AddEntityToBucket(int entity, int id)
		{
			Bucket bucket = Buckets[id];
			bucket?.AddEntity(entity);
		}

		private static async Task<Dictionary<int, int>> CountPlayers(ClientId player)
		{
			Dictionary<int, int> result = new() { [1] = 0, [2] = 0, [3] = 0 };
			foreach (Bucket buck in Buckets)
			{
				int Key = Buckets.IndexOf(buck);
				if (Key == 0) continue;
				result[Key] = buck.Players.Count;
			}
			return result;
		}

		private static async Task<bool> CheckIn(ClientId player, int id)
		{
			return Buckets[id].Players.Contains(player.Player);
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
			return Players.SingleOrDefault(x => x.Handle == player.Handle);
		}

		public Entity GetEntity(Entity entity)
		{
			return Entities.SingleOrDefault(x => x.Handle == entity.Handle || x.NetworkId == entity.NetworkId);
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