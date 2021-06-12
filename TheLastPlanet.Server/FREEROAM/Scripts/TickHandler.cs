using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;

namespace TheLastPlanet.Server.Scripts
{
	static class TickHandler
	{
		public static void Init()
		{
			Server.Instance.AddTick(Salvataggio);
		}

		private static async Task Salvataggio()
		{
			await BaseScript.Delay(600000);
			foreach (var p in BucketsHandler.Buckets[5].Players)
			{
				var user = Funzioni.GetUserFromPlayerId(p.Handle);
				if (user != null && user.status.Spawned)
					await user.SalvaPersonaggio();
			}
		}
	}
}
