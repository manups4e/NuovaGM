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
		public static Dictionary<int, Bucket> Buckets = new();

		public static void Init()
		{
			Buckets.Add(0, new Bucket(0, "Lobby") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict });
			Buckets.Add(1, new Bucket(1, "RolePlay") { PopulationEnabled = true, LockdownMode = BucketLockdownMode.relaxed });
			Buckets.Add(2, new Bucket(2, "Minigames") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict });
			Buckets.Add(3, new Bucket(3, "Gare") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict });
			Buckets.Add(4, new Bucket(4, "Negozio") { PopulationEnabled = false, LockdownMode = BucketLockdownMode.strict });
			Buckets.Add(5, new Bucket(4, "FreeRoam") { PopulationEnabled = true, LockdownMode = BucketLockdownMode.strict });

			Server.Instance.Events.Mount("lprp:addPlayerToBucket", new Action<ClientId, int>(AddPlayerToBucket));
			Server.Instance.Events.Mount("lprp:addEntityToBucket", new Action<int, int>(AddEntityToBucket));
			Server.Instance.Events.Mount("lprp:richiediContoBuckets", new Func<ClientId, Task<Dictionary<int, int>>>(CountPlayers));
			Server.Instance.Events.Mount("lprp:checkSeGiaDentro", new Func<ClientId, int, Task<bool>>(CheckIn));
		}

		private static void AddPlayerToBucket(ClientId player, int id)
		{
			foreach (KeyValuePair<int, Bucket> bucket in Buckets)
			{
				if (bucket.Key != id && bucket.Value.Players.Contains(player.Player)) bucket.Value.Players.Remove(player.Player);
				if (bucket.Key == id) bucket.Value.AddPlayer(player.Player);
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

			foreach (KeyValuePair<int, Bucket> buck in Buckets)
			{
				if (buck.Key == 0) continue;
				result[buck.Key] = buck.Value.Players.Count;
			}

			return result;
		}

		private static async Task<bool> CheckIn(ClientId player, int id)
		{
			Bucket bucket = Buckets[id];
			Player p = bucket.Players.FirstOrDefault(x => x.Handle == player.Handle.ToString());

			return p != null;
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