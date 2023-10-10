using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public static BucketsContainer Store;
        public static RolePlayBucketsContainer RolePlay;
        public static FreeRoamBucketContainer FreeRoam;
        public static RaceBucketContainer Races;
        public static BucketsContainer Minigames;

        public static void Init()
        {
            EventDispatcher.Mount("tlg:addPlayerToBucket", new Action<PlayerClient, ServerMode>(AddPlayerToBucket));
            EventDispatcher.Mount("tlg:checkSeGiaDentro", new Func<PlayerClient, ServerMode, Task<bool>>(CheckIn));
            EventDispatcher.Mount("tlg:addEntityToBucket", new Action<int, ServerMode>(AddEntityToBucket));
            EventDispatcher.Mount("tlg:richiediContoBuckets", new Func<PlayerClient, Task<Dictionary<ServerMode, int>>>(CountPlayers));
            Lobby = new(ServerMode.Lobby, new Bucket(0, "LOBBY") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false });
            Store = new(ServerMode.Store, new Bucket(4000, "STORES") { LockdownMode = BucketLockdownMode.strict, PopulationEnabled = false });
            RolePlay = new(ServerMode.Roleplay, new Bucket(1000, "ROLEPLAY") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
            FreeRoam = new(ServerMode.FreeRoam, new Bucket(5000, "FREEROAM") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
            Races = new(ServerMode.Races, new Bucket(3000, "RACES") { LockdownMode = BucketLockdownMode.relaxed, PopulationEnabled = true });
            Minigames = new(ServerMode.Minigames, new List<Bucket>());
        }

        /// <summary>
        /// Adds a player to the bucket by removing it from the other buckets
        /// </summary>
        /// <param name="player">Player to add</param>
        /// <param name="id">Id of the bucket</param>
        private static void AddPlayerToBucket([FromSource] PlayerClient player, ServerMode id)
        {
            List<PlayerClient> clients = null;
            RemovePlayerFromBucket(player, player.Status.PlayerStates.Mode, "");
            switch (id)
            {
                case ServerMode.Lobby:
                    Lobby.AddPlayer(player);
                    clients = Lobby.Bucket.Players;
                    break;
                case ServerMode.Roleplay:
                    RolePlay.AddPlayer(player);
                    clients = RolePlay.Bucket.Players;
                    break;
                case ServerMode.FreeRoam:
                    FreeRoam.AddPlayer(player);
                    clients = FreeRoam.Bucket.Players;
                    break;
                case ServerMode.Races:
                    Races.AddPlayer(player);
                    clients = Races.Bucket.Players;
                    break;
                case ServerMode.Minigames:
                    break;
            }

            player.SetState($"{player.Status.PlayerStates._name}:ServerMode", id);
            EventDispatcher.Send(clients, "tlg:onPlayerEntrance", player);
            UpdateBucketsCount();
            player.Status.Clear();
        }

        private static void RemovePlayerFromBucket([FromSource] PlayerClient player, ServerMode id, string reason)
        {
            switch (id)
            {
                case ServerMode.Lobby:
                    Lobby.RemovePlayer(player, reason);
                    break;
                case ServerMode.Roleplay:
                    RolePlay.RemovePlayer(player, reason);
                    break;
                case ServerMode.FreeRoam:
                    FreeRoam.RemovePlayer(player, reason);
                    break;
                case ServerMode.Races:
                    break;
                case ServerMode.Minigames:
                    break;
            }
        }


        /// <summary>
        /// Adds an Entity to the bucket by removing it from the other buckets
        /// </summary>
        /// <param name="entity">Entity network ID</param>
        /// <param name="id">Bucket ID</param>
        private static void AddEntityToBucket(int entity, ServerMode id)
        {
            Entity ent = Entity.FromNetworkId(entity);
            if (Lobby.Bucket.Entities.Contains(ent)) Lobby.Bucket.Entities.Remove(ent);
            switch (id)
            {
                case ServerMode.Roleplay:
                    RolePlay.Bucket.AddEntity(ent);
                    break;
                case ServerMode.FreeRoam:
                    FreeRoam.Bucket.Entities.Add(ent);
                    break;
                case ServerMode.Races:
                    break;
                case ServerMode.Minigames:
                    break;
            }
        }

        private static async Task<Dictionary<ServerMode, int>> CountPlayers([FromSource] PlayerClient player)
        {
            Dictionary<ServerMode, int> result = new()
            {
                [ServerMode.Lobby] = Lobby.GetTotalPlayers(),
                [ServerMode.FreeRoam] = FreeRoam.GetTotalPlayers(),
                [ServerMode.Roleplay] = RolePlay.GetTotalPlayers(),
                [ServerMode.Races] = Races.GetTotalPlayers(),
                [ServerMode.Minigames] = Minigames.GetTotalPlayers(),
            };
            return result;
        }

        private static async Task<bool> CheckIn([FromSource] PlayerClient player, ServerMode id)
        {
            switch (id)
            {
                case ServerMode.Lobby:
                    return Lobby.Bucket.Players.Contains(player);
                case ServerMode.Roleplay:
                    return RolePlay.Bucket.Players.Contains(player);
                case ServerMode.FreeRoam:
                    return FreeRoam.Bucket.Players.Contains(player);
                case ServerMode.Races:
                    return Races.Bucket.Players.Contains(player);
                case ServerMode.Minigames:
                    return Minigames.Buckets.Any(x => x.Players.Contains(player));
                default:
                    return true;
            }
        }

        public static void UpdateBucketsCount()
        {
            Dictionary<ServerMode, int> result = new()
            {
                [ServerMode.Lobby] = Lobby.GetTotalPlayers(),
                [ServerMode.FreeRoam] = FreeRoam.GetTotalPlayers(),
                [ServerMode.Roleplay] = RolePlay.GetTotalPlayers(),
                [ServerMode.Races] = Races.GetTotalPlayers(),
                [ServerMode.Minigames] = Minigames.GetTotalPlayers(),
            };

            EventDispatcher.Send(Lobby.Bucket.Players, "tlg:SetBucketsPlayers", result);
        }

    }
}