using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Core.Buckets;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.Core.Buckets
{
	internal static class BucketsHandler
	{
		/*
				[0] = new Bucket(0, "LOBBY") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false },
				[1000] = new Bucket(1000, "ROLEPLAY") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true },
				[4000] = new Bucket(4000, "NEGOZIO") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false },
				[5000] = new Bucket(5000, "FREEROAM") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true },
		*/

		public static LobbyBucketsContainer Lobby;
		public static BucketsContainer Negozio;
		public static RolePlayBucketsContainer RolePlay;
		public static FreeRoamBucketContainer FreeRoam;
		public static BucketsContainer Gare;
		public static BucketsContainer Minigiochi;

		public static void Init()
		{
			Server.Instance.Events.Mount("tlg:addPlayerToBucket", new Action<ClientId, ModalitaServer>(AddPlayerToBucket));
			Server.Instance.Events.Mount("tlg:checkSeGiaDentro", new Func<ClientId, ModalitaServer, Task<bool>>(CheckIn));
			Server.Instance.Events.Mount("tlg:addEntityToBucket", new Action<int, ModalitaServer>(AddEntityToBucket));
			Server.Instance.Events.Mount("tlg:richiediContoBuckets", new Func<ClientId, Task<Dictionary<ModalitaServer, int>>>(CountPlayers));
			Lobby = new(ModalitaServer.Lobby, new Bucket(0, "LOBBY") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false });
			Negozio = new(ModalitaServer.Negozio, new Bucket(4000, "NEGOZI") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false });
			RolePlay = new(ModalitaServer.Roleplay, new Bucket(1000, "ROLEPLAY") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
			FreeRoam = new(ModalitaServer.FreeRoam, new Bucket(5000, "FREEROAM") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
			Gare = new(ModalitaServer.Gare, new List<Bucket>());
			Minigiochi = new(ModalitaServer.Minigiochi, new List<Bucket>());
	}

	/// <summary>
	/// Aggiunge un player al bucket rimuovendolo dagli altri buckets
	/// </summary>
	/// <param name="player">Player da aggiungere</param>
	/// <param name="id">Id del bucket</param>
	private static void AddPlayerToBucket(ClientId player, ModalitaServer id)
		{
			RemovePlayerFromBucket(player, player.User.Status.PlayerStates.Modalita, "");
			switch (id)
			{
				case ModalitaServer.Lobby:
					Lobby.AddPlayer(player);
					break;
				case ModalitaServer.Roleplay:
					RolePlay.AddPlayer(player);
					break;
				case ModalitaServer.FreeRoam:
					FreeRoam.AddPlayer(player);
					break;
				case ModalitaServer.Gare:
					break;
				case ModalitaServer.Minigiochi:
					break;
			}
			player.User.Status.PlayerStates.Modalita = id;
			UpdateBucketsCount();
		}

		private static void RemovePlayerFromBucket(ClientId player, ModalitaServer id, string reason)
		{
			switch (id)
			{
				case ModalitaServer.Lobby:
					Lobby.RemovePlayer(player, reason);
					break;
				case ModalitaServer.Roleplay:
					RolePlay.RemovePlayer(player, reason);
					break;
				case ModalitaServer.FreeRoam:
					FreeRoam.RemovePlayer(player, reason);
					break;
				case ModalitaServer.Gare:
					break;
				case ModalitaServer.Minigiochi:
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
			if (Lobby.Bucket.Entities.Contains(ent)) Lobby.Bucket.Entities.Remove(ent);
			switch (id)
			{
				case ModalitaServer.Roleplay:
					RolePlay.Bucket.AddEntity(ent);
					break;
				case ModalitaServer.FreeRoam:
					FreeRoam.Bucket.Entities.Add(ent);
					break;
				case ModalitaServer.Gare:
					break;
				case ModalitaServer.Minigiochi:
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
					return Lobby.Bucket.Players.Contains(player);
				case ModalitaServer.Roleplay:
					return RolePlay.Bucket.Players.Contains(player);
				case ModalitaServer.FreeRoam:
					return FreeRoam.Bucket.Players.Contains(player);
				case ModalitaServer.Gare:
					return Gare.Buckets.Any(x=>x.Players.Contains(player));
				case ModalitaServer.Minigiochi:
					return Minigiochi.Buckets.Any(x => x.Players.Contains(player));
				default:
					return true;
			}
		}

		public static void UpdateBucketsCount()
		{
			Dictionary<ModalitaServer, int> result = new()
			{
				[ModalitaServer.Lobby] = Lobby.GetTotalPlayers(),
				[ModalitaServer.FreeRoam] = FreeRoam.GetTotalPlayers(),
				[ModalitaServer.Roleplay] = RolePlay.GetTotalPlayers(),
				[ModalitaServer.Gare] = Gare.GetTotalPlayers(),
				[ModalitaServer.Minigiochi] = Minigiochi.GetTotalPlayers(),
			};

			Server.Instance.Events.Send(Lobby.Bucket.Players, "tlg:SetBucketsPlayers", result);
		}

	}
}