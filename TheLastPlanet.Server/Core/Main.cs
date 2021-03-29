using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Logger;
using TheLastPlanet.Server.Core.PlayerChar;
using System.Linq;

namespace TheLastPlanet.Server.Core
{
	public static class Main
	{
		private static long _starttick;
		private static DateTime Now;
		public static void Init()
		{
			Server.Instance.AddTick(Orario);
			Server.Instance.AddTick(PlayTime);
			_starttick = GetGameTimer();
			Now = DateTime.Now;
		}

		private static async Task Orario()
		{
			try
			{
				var ora = DateTime.Now - Now;
				SetConvarServerInfo("Attivo da:",$"{ora.Days} giorni {ora.Hours} Ore {ora.Minutes} Minuti");
				await BaseScript.Delay(60000);
			}
			catch (Exception e)
			{
				Server.Logger.Error( e.ToString() + e.StackTrace);
			}
		}

		private static async Task PlayTime()
		{
			try
			{
				await BaseScript.Delay(60000);
				if (Server.Instance.Clients.Count > 0)
				{
					foreach (var user in from user in Server.Instance.Clients where user.User.status.Spawned select user)
						user.User.playTime += 60;
				}
			}
			catch (Exception e)
			{
				Server.Logger.Error( e.ToString() + e.StackTrace);
			}
		}
	}
}