using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Shared;
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
					/*
			[0] = new Bucket(0, "LOBBY") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false },
			[1000] = new Bucket(1000, "ROLEPLAY") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true },
			[4000] = new Bucket(4000, "NEGOZIO") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false },
			[5000] = new Bucket(5000, "FREEROAM") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true },
			*/

		public static BucketsContainer Lobby = new BucketsContainer(ModalitaServer.Lobby, new Bucket(0, "LOBBY") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false });
		public static BucketsContainer Negozio = new BucketsContainer(ModalitaServer.Negozio, new Bucket(4000, "NEGOZI") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false });
		public static BucketsContainer RolePlay = new BucketsContainer(ModalitaServer.Roleplay, new Bucket(1000, "ROLEPLAY") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
		public static BucketsContainer FreeRoam = new BucketsContainer(ModalitaServer.FreeRoam, new Bucket(5000, "FREEROAM") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
		public static BucketsContainer Gare = new BucketsContainer(ModalitaServer.Gare, new List<Bucket>());
		public static BucketsContainer Minigiochi = new BucketsContainer(ModalitaServer.Minigiochi, new List<Bucket>());

		public static void Init()
		{
			Server.Instance.Events.Mount("lprp:addPlayerToBucket", new Action<ClientId, ModalitaServer>(AddPlayerToBucket));
			Server.Instance.Events.Mount("lprp:checkSeGiaDentro", new Func<ClientId, ModalitaServer, Task<bool>>(CheckIn));
			Server.Instance.Events.Mount("lprp:addEntityToBucket", new Action<int, ModalitaServer>(AddEntityToBucket));
			Server.Instance.Events.Mount("lprp:richiediContoBuckets", new Func<ClientId, Task<Dictionary<ModalitaServer, int>>>(CountPlayers));
		}

		/// <summary>
		/// Aggiunge un player al bucket rimuovendolo dagli altri buckets
		/// </summary>
		/// <param name="player">Player da aggiungere</param>
		/// <param name="id">Id del bucket</param>
		private static void AddPlayerToBucket(ClientId player, ModalitaServer id)
		{
			switch (id)
			{
				case ModalitaServer.Lobby:
					if (RolePlay.Bucket.Players.Contains(player.Player)) RolePlay.Bucket.Players.Remove(player.Player);
					else if (FreeRoam.Bucket.Players.Contains(player.Player)) FreeRoam.Bucket.Players.Remove(player.Player);
					else if (Gare.Buckets.Any(x=> x.Players.Contains(player.Player))) Gare.Buckets.FirstOrDefault(x => x.Players.Contains(player.Player)).Players.Remove(player.Player);
					else if (Minigiochi.Buckets.Any(x=> x.Players.Contains(player.Player))) Minigiochi.Buckets.FirstOrDefault(x => x.Players.Contains(player.Player)).Players.Remove(player.Player);
					else if (Negozio.Bucket.Players.Contains(player.Player)) Negozio.Bucket.Players.Remove(player.Player);
					Lobby.Bucket.AddPlayer(player.Player);
					break;
				case ModalitaServer.Roleplay:
					if (Lobby.Bucket.Players.Contains(player.Player)) Lobby.Bucket.Players.Remove(player.Player);
					RolePlay.Bucket.AddPlayer(player.Player);
					break;
				case ModalitaServer.FreeRoam:
					if (Lobby.Bucket.Players.Contains(player.Player)) Lobby.Bucket.Players.Remove(player.Player);
					FreeRoam.Bucket.Players.Add(player.Player);
					break;
				case ModalitaServer.Gare:
					if (Lobby.Bucket.Players.Contains(player.Player)) Lobby.Bucket.Players.Remove(player.Player);
					break;
				case ModalitaServer.Minigiochi:
					if (Lobby.Bucket.Players.Contains(player.Player)) Lobby.Bucket.Players.Remove(player.Player);
					break;
			}
		}

		/// <summary>
		/// Aggiunge un Entity al bucket rimuovendolo dagli altri buckets
		/// </summary>
		/// <param name="entity">Entity network ID</param>
		/// <param name="id">ID del bucket</param>
		private static void AddEntityToBucket(int entity, ModalitaServer id)
		{
			Entity ent = Entity.FromNetworkId(entity);
			switch (id)
			{
				case ModalitaServer.Roleplay:
					if (Lobby.Bucket.Entities.Contains(ent)) Lobby.Bucket.Entities.Remove(ent);
					RolePlay.Bucket.AddEntity(ent);
					break;
				case ModalitaServer.FreeRoam:
					if (Lobby.Bucket.Entities.Contains(ent)) Lobby.Bucket.Entities.Remove(ent);
					FreeRoam.Bucket.Entities.Add(ent);
					break;
				case ModalitaServer.Gare:
					if (Lobby.Bucket.Entities.Contains(ent)) Lobby.Bucket.Entities.Remove(ent);
					break;
				case ModalitaServer.Minigiochi:
					if (Lobby.Bucket.Entities.Contains(ent)) Lobby.Bucket.Entities.Remove(ent);
					break;
			}
		}

		private static async Task<Dictionary<ModalitaServer, int>> CountPlayers(ClientId player)
		{
			Dictionary<ModalitaServer, int> result = new()
			{
				[ModalitaServer.Lobby] = Lobby.GetTotalPlayers(),
				[ModalitaServer.FreeRoam] = FreeRoam.GetTotalPlayers(),
				[ModalitaServer.Roleplay] = RolePlay.GetTotalPlayers(),
				[ModalitaServer.Gare] = Gare.GetTotalPlayers(),
				[ModalitaServer.Minigiochi] = Minigiochi.GetTotalPlayers(),
			};
			return result;
		}

		private static async Task<bool> CheckIn(ClientId player, ModalitaServer id)
		{
			switch (id)
			{
				case ModalitaServer.Lobby:
					return Lobby.Bucket.Players.Contains(player.Player);
				case ModalitaServer.Roleplay:
					return RolePlay.Bucket.Players.Contains(player.Player);
				case ModalitaServer.FreeRoam:
					return FreeRoam.Bucket.Players.Contains(player.Player);
				case ModalitaServer.Gare:
					return Gare.Buckets.Any(x=>x.Players.Contains(player.Player));
				case ModalitaServer.Minigiochi:
					return Minigiochi.Buckets.Any(x => x.Players.Contains(player.Player));
				default:
					return true;
			}
		}
	}

	public class Bucket
	{
		public int Id;
		public string Name;
		public List<Player> Players = new();
		public List<Entity> Entities = new();
		public int TotalPlayers => Players.Count;
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
				if (API.GetPlayerRoutingBucket(player.Handle) != (int)Id) API.SetPlayerRoutingBucket(player.Handle, (int)Id);
			}
		}

		public void AddEntity(Entity entity)
		{
			if (!Entities.Contains(entity))
			{
				Entities.Add(entity);
				if (API.GetEntityRoutingBucket(entity.Handle) != (int)Id) API.SetEntityRoutingBucket(entity.Handle, (int)Id);
			}
		}

		public async void AddEntity(int entityNetworkId)
		{
			if (Entities.All(x => x.Handle != entityNetworkId))
			{
				Entity ent = Entity.FromNetworkId(entityNetworkId);
				while (ent == null) await BaseScript.Delay(0);
				Entities.Add(ent);
				if (API.GetEntityRoutingBucket(ent.Handle) != (int)Id) API.SetEntityRoutingBucket(ent.Handle, (int)Id);
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
					API.SetRoutingBucketEntityLockdownMode((int)Id, "strict");

					break;
				case BucketLockdownMode.relaxed:
					API.SetRoutingBucketEntityLockdownMode((int)Id, "relaxed");

					break;
				case BucketLockdownMode.inactive:
					API.SetRoutingBucketEntityLockdownMode((int)Id, "inactive");

					break;
			}
		}

		private void _enablePopulation(bool enabled)
		{
			API.SetRoutingBucketPopulationEnabled((int)Id, enabled);
		}

		public ModalitaServer GetBucketGameMode()
		{
			switch (Id)
			{
				case int n when (n >= 0 && n <= 999):
					return ModalitaServer.Lobby;
				case int n when (n >= 1000 && n <= 1999):
					return ModalitaServer.Roleplay;
				case int n when (n >= 2000 && n <= 2999):
					return ModalitaServer.Minigiochi;
				case int n when (n >= 3000 && n <= 3999):
					return ModalitaServer.Gare;
				case int n when (n >= 4000 && n <= 4999):
					return ModalitaServer.Negozio;
				case int n when (n >= 5000 && n <= 5999):
					return ModalitaServer.FreeRoam;
				default:
					return ModalitaServer.UNKNOWN;
			}
		}
	}

	public class BucketsContainer
	{
		public ModalitaServer Modalita { get; set; }
		public Bucket Bucket { get; set; }
		public List<Bucket> Buckets { get; set; }

		public BucketsContainer(ModalitaServer modalitaServer, Bucket bucket)
		{
			Modalita = modalitaServer;
			Bucket = bucket;
		}
		public BucketsContainer(ModalitaServer modalitaServer, List<Bucket> buckets)
		{
			Modalita = modalitaServer;
			Buckets = buckets;
		}

		public int GetTotalPlayers()
		{
			int total = 0;
			if (Buckets != null)
				foreach (Bucket b in Buckets)
					total += b.TotalPlayers;
			if (Bucket != null)
				total += Bucket.TotalPlayers;
			return total;
		}
	}

}