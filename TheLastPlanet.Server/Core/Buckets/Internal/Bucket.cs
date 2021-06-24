using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.Core.Buckets
{
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
		public List<ClientId> Players = new();
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
			ID = id;
			Name = name;
		}

		public virtual void AddPlayer(ClientId client)
		{
			if (Players.Contains(client)) return;
			Players.Add(client);
			if (API.GetPlayerRoutingBucket(client.Handle.ToString()) != ID) API.SetPlayerRoutingBucket(client.Handle.ToString(), ID);
		}

		public virtual void AddEntity(Entity entity)
		{
			if (Entities.Contains(entity)) return;
			Entities.Add(entity);
			if (API.GetEntityRoutingBucket(entity.Handle) != ID) API.SetEntityRoutingBucket(entity.Handle, ID);
		}

		public async virtual void AddEntity(int entityNetworkId)
		{
			if (Entities.Any(x => x.NetworkId == entityNetworkId)) return;
			Entity ent = Entity.FromNetworkId(entityNetworkId);
			while (ent == null) await BaseScript.Delay(0);
			Entities.Add(ent);
			if (API.GetEntityRoutingBucket(ent.Handle) != ID) API.SetEntityRoutingBucket(ent.Handle, ID);
		}

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

		public ModalitaServer GetBucketGameMode()
		{
			switch (ID)
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
}
