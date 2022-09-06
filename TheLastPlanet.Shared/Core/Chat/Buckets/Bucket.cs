﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Collections.Generic;
using System.Linq;

#if SERVER
using TheLastPlanet.Server;
#endif
namespace TheLastPlanet.Shared.Core.Buckets
{
    public delegate void PlayerJoining(PlayerClient client);
    public delegate void PlayerLeft(PlayerClient client, string reason);

    public enum BucketLockdownMode
    {
        strict,
        relaxed,
        inactive
    }

    public class Bucket
    {
        public int ID;
        public string Name;
        public List<PlayerClient> Players = new();
        public List<Entity> Entities = new();
        public int TotalPlayers => Players.Count;
        private BucketLockdownMode _lockdownMode;
        private bool _populationEnabled;

        public event PlayerJoining OnPlayerJoin;
        public event PlayerLeft OnPlayerLeft;

        public BucketLockdownMode LockdownMode
        {
            get => _lockdownMode;
#if SERVER
            set
            {
                _lockdownMode = value;
                _setBucketLockdownMode(value);
            }
#endif
        }

        public bool PopulationEnabled
        {
            get => _populationEnabled;
#if SERVER
            set
            {
                _populationEnabled = value;
                _enablePopulation(value);
            }
#endif
        }

        public Bucket(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public virtual void AddPlayer(PlayerClient client)
        {
            if (Players.Any(x => x.Handle == client.Handle)) return;
            Players.Add(client);
#if SERVER
            if (API.GetPlayerRoutingBucket(client.Handle.ToString()) != ID) API.SetPlayerRoutingBucket(client.Handle.ToString(), ID);
#endif
            OnPlayerJoin?.Invoke(client);
        }

        public virtual void RemovePlayer(PlayerClient client, string reason = "")
        {
            if (!Players.Any(x => x.Handle == client.Handle)) return;
            Players.Remove(Players.FirstOrDefault(x => x.Handle == client.Handle));
            OnPlayerLeft?.Invoke(client, reason);
        }

        public virtual void AddEntity(Entity entity)
        {
            if (Entities.Contains(entity)) return;
            Entities.Add(entity);
#if SERVER
            if (API.GetEntityRoutingBucket(entity.Handle) != ID) API.SetEntityRoutingBucket(entity.Handle, ID);
#endif
        }

        public async virtual void AddEntity(int entityNetworkId)
        {
            if (Entities.Any(x => x.NetworkId == entityNetworkId)) return;
            Entity ent = Entity.FromNetworkId(entityNetworkId);
            while (ent == null) await BaseScript.Delay(0);
            Entities.Add(ent);
#if SERVER
            if (API.GetEntityRoutingBucket(ent.Handle) != ID) API.SetEntityRoutingBucket(ent.Handle, ID);
#endif
        }

#if SERVER

        private void _setBucketLockdownMode(BucketLockdownMode mode)
        {
            switch (mode)
            {
                case BucketLockdownMode.strict:
                    API.SetRoutingBucketEntityLockdownMode(ID, "strict");

                    break;
                case BucketLockdownMode.relaxed:
                    API.SetRoutingBucketEntityLockdownMode(ID, "relaxed");

                    break;
                case BucketLockdownMode.inactive:
                    API.SetRoutingBucketEntityLockdownMode(ID, "inactive");

                    break;
            }
        }

        private void _enablePopulation(bool enabled)
        {
            API.SetRoutingBucketPopulationEnabled(ID, enabled);
        }
#endif
        public ModalitaServer GetBucketGameMode()
        {
            switch (ID)
            {
                case var n when (n >= 0 && n <= 999):
                    return ModalitaServer.Lobby;
                case var n when (n >= 1000 && n <= 1999):
                    return ModalitaServer.Roleplay;
                case var n when (n >= 2000 && n <= 2999):
                    return ModalitaServer.Minigiochi;
                case var n when (n >= 3000 && n <= 3999):
                    return ModalitaServer.Gare;
                case var n when (n >= 4000 && n <= 4999):
                    return ModalitaServer.Negozio;
                case var n when (n >= 5000 && n <= 5999):
                    return ModalitaServer.FreeRoam;
                default:
                    return ModalitaServer.UNKNOWN;
            }
        }

#if SERVER

        public void TriggerClientEvent(PlayerClient client, string endpoint, params object[] args)
        {
            if (Players.Contains(client))
                EventDispatcher.Send(client, endpoint, args);
            else
                Server.Server.Logger.Warning($"Buckets:TriggerClientEvent, client {client} not in the list for {Name}!");
        }

        public void TriggerClientEvent(string endpoint, params object[] args)
        {
            EventDispatcher.Send(Players.ToList(), endpoint, args);
        }
#endif
    }
}
