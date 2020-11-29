using CitizenFX.Core;
using Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Server.TimeWeather
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
			Server.Instance.AddEventHandler("freezeTime", new Action<bool>(FreezeTime));
			Server.Instance.AddEventHandler("UpdateFromCommandTime", new Action<int>(Update));
			Server.Instance.AddTick(SetTime);
			Server.Instance.AddTick(UpdateTime);
		}

		private static void Update(int time)
		{
			secondOfDay = time;
			BaseScript.TriggerClientEvent("UpdateFromServerTime", secondOfDay, Date.Ticks, frozen, true);
		}

		private static void FreezeTime(bool freeze) => frozen = freeze;

		public static async Task SetTime()
		{
			await BaseScript.Delay(1000);
			Date = DateTime.Now;
			BaseScript.TriggerClientEvent("UpdateFromServerTime", secondOfDay, Date.Ticks, frozen, false);
		}

		public static async Task UpdateTime()
		{
			await BaseScript.Delay(33);
			if (!frozen)
			{
				_timeBuffer += 0.9900f;
				if (_timeBuffer > 1f)
				{
					secondOfDay += (int)Math.Floor(_timeBuffer);
					_timeBuffer -= (int)Math.Floor(_timeBuffer);
					if (secondOfDay > 86399) secondOfDay = 0;
				}
			}
		}
	}
}
