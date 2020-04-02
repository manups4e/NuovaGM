using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Server.Meteo_New
{
	static class Orario
	{
		private static DateTime Date = DateTime.Now;
		private static Random rand = new Random(DateTime.Now.Millisecond);
		private static float _timeBuffer = 0;
		private static int h = 0;
		private static int m = 0;
		private static int s = 0;
		public static int secondOfDay = (rand.Next(0, 23) * 3600) + (rand.Next(0, 59) * 60) + 0;
		public static bool frozen = false;

		public static void Init()
		{
			Server.Instance.RegisterEventHandler("freezeTime", new Action<bool>(FreezeTime));
			Server.Instance.RegisterEventHandler("UpdateFromCommandTime", new Action<int>(Update));
			Server.Instance.RegisterTickHandler(SetTime);
			Server.Instance.RegisterTickHandler(UpdateTime);
		}

		private async static void Update(int time)
		{
			secondOfDay = time;
			BaseScript.TriggerClientEvent("UpdateFromServerTime", secondOfDay, Date.Ticks, frozen, true);
		}

		private static async void FreezeTime(bool freeze) => frozen = freeze;

		public static async Task SetTime()
		{
			await BaseScript.Delay(1000);
			BaseScript.TriggerClientEvent("UpdateFromServerTime", secondOfDay, Date.Ticks, frozen, false);
		}

		public static async Task UpdateTime()
		{
			await BaseScript.Delay(32);
			Date = DateTime.Now;
			if (!frozen)
			{
				float gameSecond = 33.33f;
				_timeBuffer += (float)Math.Round(33f / gameSecond, 4);
				if (_timeBuffer > 1f)
				{
					int skipSeconds = (int)Math.Floor(_timeBuffer);
					_timeBuffer -= skipSeconds;
					secondOfDay += skipSeconds;
					if (secondOfDay > 86400)
						secondOfDay %= 86400;
				}
			}
			h = (int)Math.Floor(secondOfDay / 3600f);
			m = (int)Math.Floor((secondOfDay - (h * 3600)) / 60f);
			s = secondOfDay - (h * 3600) - (m * 60);
			secondOfDay = (h * 3600) + (m * 60) + s;
		}
	}
}
