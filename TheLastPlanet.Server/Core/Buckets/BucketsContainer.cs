using System;
using System.Collections.Generic;
using System.Text;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Server.Core.Buckets
{
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
