using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Server.TimeWeather
{
	static class Orario
	{
		private static DateTime Date = DateTime.Now;
		private static float _timeBuffer;
//		private static int h;
//		private static int m;
//		private static int s;
		public static int secondOfDay = (new Random().Next(0, 23) * 3600) + (new Random().Next(0, 59) * 60) + new Random().Next(0, 59);

		public static bool frozen = false;

		public static void Init()
		{
			Server.Instance.RegisterEventHandler("freezeTime", new Action<bool>(FreezeTime));
			Server.Instance.RegisterEventHandler("UpdateFromCommandTime", new Action<int>(Update));
			Server.Instance.RegisterTickHandler(SetTime);
			Server.Instance.RegisterTickHandler(UpdateTime);
		}

		private static void Update(int time)
		{
			try
			{
				secondOfDay = time;
				BaseScript.TriggerClientEvent("UpdateFromServerTime", secondOfDay, Date.Ticks, frozen, true);
			}
			catch (Exception e)
			{
				Server.Printa(LogType.Error, e.ToString() + e.StackTrace);
			}
		}

		private static void FreezeTime(bool freeze) => frozen = freeze;

		public static async Task SetTime()
		{
			await BaseScript.Delay(1000);
			BaseScript.TriggerClientEvent("UpdateFromServerTime", secondOfDay, Date.Ticks, frozen, false);
		}

		public static async Task UpdateTime()
		{
			try
			{
				await BaseScript.Delay(33);
				Date = DateTime.Now;
				if (!frozen)
				{
					_timeBuffer += 0.9900f;
					if (_timeBuffer > 1f)
					{
						_timeBuffer -= (int)Math.Floor(_timeBuffer);
						secondOfDay += (int)Math.Floor(_timeBuffer);
						if (secondOfDay > 86399)
							secondOfDay = 0;
					}
				}
				//h = (int)Math.Floor(secondOfDay / 3600f);
				//m = (int)Math.Floor((secondOfDay - (h * 3600)) / 60f);
				//s = secondOfDay - (h * 3600) - (m * 60);
			}
			catch (Exception e)
			{
				Server.Printa(LogType.Error, e.ToString() + e.StackTrace);
			}
		}
	}
}
