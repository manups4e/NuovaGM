﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Shared;
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

		public static BucketsContainer Lobby = new BucketsContainer(ModalitaServer.Lobby, new Bucket(0, "LOBBY") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false });
		public static BucketsContainer Negozio = new BucketsContainer(ModalitaServer.Negozio, new Bucket(4000, "NEGOZI") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false });
		public static BucketsContainer RolePlay = new BucketsContainer(ModalitaServer.Roleplay, new Bucket(1000, "ROLEPLAY") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
		public static BucketsContainer FreeRoam = new BucketsContainer(ModalitaServer.FreeRoam, new FreeRoamBucket(5000, "FREEROAM") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
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
					if (RolePlay.Bucket.Players.Contains(player)) RolePlay.Bucket.Players.Remove(player);
					else if (FreeRoam.Bucket.Players.Contains(player)) FreeRoam.Bucket.Players.Remove(player);
					else if (Gare.Buckets.Any(x=> x.Players.Contains(player))) Gare.Buckets.FirstOrDefault(x => x.Players.Contains(player)).Players.Remove(player);
					else if (Minigiochi.Buckets.Any(x=> x.Players.Contains(player))) Minigiochi.Buckets.FirstOrDefault(x => x.Players.Contains(player)).Players.Remove(player);
					else if (Negozio.Bucket.Players.Contains(player)) Negozio.Bucket.Players.Remove(player);
					Lobby.Bucket.AddPlayer(player);
					break;
				case ModalitaServer.Roleplay:
					if (Lobby.Bucket.Players.Contains(player)) Lobby.Bucket.Players.Remove(player);
					RolePlay.Bucket.AddPlayer(player);
					break;
				case ModalitaServer.FreeRoam:
					if (Lobby.Bucket.Players.Contains(player)) Lobby.Bucket.Players.Remove(player);
					FreeRoam.Bucket.Players.Add(player);
					break;
				case ModalitaServer.Gare:
					if (Lobby.Bucket.Players.Contains(player)) Lobby.Bucket.Players.Remove(player);
					break;
				case ModalitaServer.Minigiochi:
					if (Lobby.Bucket.Players.Contains(player)) Lobby.Bucket.Players.Remove(player);
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
	}
}