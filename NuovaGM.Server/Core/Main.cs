using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core
{
	public static class Main
	{
		private static long _starttick;

		public static void Init()
		{
			Server.Instance.AddTick(Orario);
			Server.Instance.AddTick(PlayTime);
			_starttick = GetGameTimer();
		}

		private static async Task Orario()
		{
			try
			{
				long tick = GetGameTimer();
				double uptimeDay = Math.Floor((double)(tick - _starttick) / 86400000);
				double uptimeHour = Math.Floor((double)(tick - _starttick) / 3600000) % 24;
				double uptimeMinute = Math.Floor((double)(tick - _starttick) / 60000) % 60;
				ExecuteCommand($"sets Attivo \"{uptimeDay} giorni {uptimeHour} Ore {uptimeMinute} Minuti \"");
				await BaseScript.Delay(60000);
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e.ToString() + e.StackTrace);
			}
		}

		private static async Task PlayTime()
		{
			try
			{
				await BaseScript.Delay(60000);
				if (Server.PlayerList.Count > 0)
					foreach (KeyValuePair<string, User> user in Server.PlayerList)
						if (user.Value.status.spawned)
							user.Value.playTime += 60;
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e.ToString() + e.StackTrace);
			}
		}
	}
}