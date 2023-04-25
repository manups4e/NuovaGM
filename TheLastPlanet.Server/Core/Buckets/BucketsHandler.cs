﻿using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Core.Buckets;


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
        public static RaceBucketContainer Gare;
        public static BucketsContainer Minigiochi;

        public static void Init()
        {
            EventDispatcher.Mount("tlg:addPlayerToBucket", new Action<PlayerClient, ModalitaServer>(AddPlayerToBucket));
            EventDispatcher.Mount("tlg:checkSeGiaDentro", new Func<PlayerClient, ModalitaServer, Task<bool>>(CheckIn));
            EventDispatcher.Mount("tlg:addEntityToBucket", new Action<int, ModalitaServer>(AddEntityToBucket));
            EventDispatcher.Mount("tlg:richiediContoBuckets", new Func<PlayerClient, Task<Dictionary<ModalitaServer, int>>>(CountPlayers));
            Lobby = new(ModalitaServer.Lobby, new Bucket(0, "LOBBY") { LockdownMode = BucketLockdownMode.inactive, PopulationEnabled = false });
            Negozio = new(ModalitaServer.Negozio, new Bucket(4000, "NEGOZI") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false });
            RolePlay = new(ModalitaServer.Roleplay, new Bucket(1000, "ROLEPLAY") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
            FreeRoam = new(ModalitaServer.FreeRoam, new Bucket(5000, "FREEROAM") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
            Gare = new(ModalitaServer.Gare, new Bucket(3000, "GARE") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
            Minigiochi = new(ModalitaServer.Minigiochi, new List<Bucket>());
        }

        /// <summary>
        /// Aggiunge un player al bucket rimuovendolo dagli altri buckets
        /// </summary>
        /// <param name="player">Player da aggiungere</param>
        /// <param name="id">Id del bucket</param>
        private static void AddPlayerToBucket([FromSource] PlayerClient player, ModalitaServer id)
        {
            List<PlayerClient> clients = null;
            RemovePlayerFromBucket(player, player.Status.PlayerStates.Modalita, "");
            switch (id)
            {
                case ModalitaServer.Lobby:
                    Lobby.AddPlayer(player);
                    clients = Lobby.Bucket.Players;
                    break;
                case ModalitaServer.Roleplay:
                    RolePlay.AddPlayer(player);
                    clients = RolePlay.Bucket.Players;
                    break;
                case ModalitaServer.FreeRoam:
                    FreeRoam.AddPlayer(player);
                    clients = FreeRoam.Bucket.Players;
                    break;
                case ModalitaServer.Gare:
                    Gare.AddPlayer(player);
                    clients = Gare.Bucket.Players;
                    break;
                case ModalitaServer.Minigiochi:
                    break;
            }

            player.SetState($"{player.Status.PlayerStates._name}:Modalita", id);
            EventDispatcher.Send(clients, "tlg:onPlayerEntrance", player);
            UpdateBucketsCount();
            player.Status.Clear();
        }
            
        private static void RemovePlayerFromBucket([FromSource] PlayerClient player, ModalitaServer id, string reason)
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

        private static async Task<Dictionary<ModalitaServer, int>> CountPlayers([FromSource] PlayerClient player)
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

        private static async Task<bool> CheckIn([FromSource] PlayerClient player, ModalitaServer id)
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
                    return Gare.Bucket.Players.Contains(player);
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

            EventDispatcher.Send(Lobby.Bucket.Players, "tlg:SetBucketsPlayers", result);
        }

    }
}