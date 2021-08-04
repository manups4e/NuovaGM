using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using CitizenFX.Core.UI;

namespace TheLastPlanet.Client.RolePlay.TimeWeather
{
	internal static class Orario
	{
		public static bool Frozen = false;
		private static int NEWsecondOfDay = 0;
		private static int secondOfDay = 5000;
		private static DateTime ClientDate;
		private static float _timeBuffer = 0;
		public static int h = 0;
		public static int m = 0;
		public static int s = 0;
		private static bool Cambio = false;

		public static async void Init()
		{
			Client.Instance.AddEventHandler("UpdateFromServerTime", new Action<int, long, bool, bool>(SetTime));
			//Client.Instance.AddTick(AggiornaTempo);
		}

		public static async void Stop()
		{
			Client.Instance.RemoveEventHandler("UpdateFromServerTime", new Action<int, long, bool, bool>(SetTime));
			Client.Instance.RemoveTick(AggiornaTempo);
		}

		public static void SetTime(int serverSecondOfDay, long serverDate, bool isTimeFrozen, bool cambio = false)
		{
			Frozen = isTimeFrozen;
			NEWsecondOfDay = serverSecondOfDay;
			ClientDate = new DateTime(serverDate); // ricordarmi di passare i ticks (long)
			Cambio = cambio;
		}

		private static bool firsttick;

		public static async Task AggiornaTempo()
		{
			if (!firsttick)
			{
				secondOfDay = NEWsecondOfDay;
				firsttick = true;
			}

			if (NEWsecondOfDay < secondOfDay - 400 || NEWsecondOfDay > secondOfDay + 400) secondOfDay = NEWsecondOfDay;
			await BaseScript.Delay(33);
			{
				_timeBuffer += 0.9900f;

				if (_timeBuffer > 1f)
				{
					_timeBuffer -= (int)Math.Floor(_timeBuffer);
					secondOfDay += (int)Math.Floor(_timeBuffer);
					if (secondOfDay > 86399) secondOfDay = 0;
				}

				h = (int)Math.Floor(secondOfDay / 3600f);
				m = (int)Math.Floor((secondOfDay - h * 3600f) / 60);
				s = secondOfDay - h * 3600 - m * 60;
			}

			if (Cambio)
			{
				HUD.ShowLoadingSavingNotificationWithTime("Aggiornamento orario del server tra 3 secondi", LoadingSpinnerType.Clockwise1, 1000);
				await BaseScript.Delay(1000);
				HUD.ShowLoadingSavingNotificationWithTime("Aggiornamento orario del server tra 2 secondi", LoadingSpinnerType.Clockwise1, 1000);
				await BaseScript.Delay(1000);
				HUD.ShowLoadingSavingNotificationWithTime("Aggiornamento orario del server tra 1 secondo", LoadingSpinnerType.Clockwise1, 1000);
				await BaseScript.Delay(1000);
				NetworkClearClockTimeOverride();
				HUD.ShowLoadingSavingNotificationWithTime("Aggiornamento orario del server in corso...", LoadingSpinnerType.Clockwise1, 10000);
				await BaseScript.Delay(2000);
				Screen.Fading.FadeOut(800);
				if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InVeicolo)
					Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsPositionFrozen = true;
				else
					Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;
				await BaseScript.Delay(2000);
				AdvanceClockTimeTo(h, m, s);
				await BaseScript.Delay(1950);
				if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InVeicolo)
					Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsPositionFrozen = false;
				else
					Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
				Screen.Fading.FadeIn(800);
				Cambio = false;
			}
			else
			{
				NetworkOverrideClockTime(h, m, s);
			}
		}
	}
}