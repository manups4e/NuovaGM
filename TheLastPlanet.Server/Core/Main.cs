using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Logger;
using TheLastPlanet.Server.Core.PlayerChar;
using System.Linq;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Server.Core
{
	public static class Main
	{
		private static DateTime Now;
		public static void Init()
		{
			Task.Run(Orario_Playtime);
			Now = DateTime.Now;
		}

		private static async Task Orario_Playtime()
		{
			try
			{
				while (true)
				{
					await BaseScript.Delay(60000);
					if (Server.Instance.Clients.Count > 0)
					{
						foreach (var user in from user in Server.Instance.Clients where user.Player is not null && user.User is not null && user.User.Status.Spawned select user)
							user.User.playTime += 60;
					}
					var ora = DateTime.Now - Now;
					await BaseScript.Delay(0);
					SetConvarServerInfo("Attivo da:", $"{ora.Days} giorni {ora.Hours} Ore {ora.Minutes} Minuti");
				}
			}
			catch (Exception e)
			{
				Server.Logger.Error(e.ToString() + e.StackTrace);
			}
		}
	}
}