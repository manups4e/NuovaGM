using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.gmPrincipale;
using CitizenFX.Core.UI;

namespace NuovaGM.Client.Meteo_new
{
	static class Orario
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
			Client.GetInstance.RegisterEventHandler("UpdateFromServerTime", new Action<int, long, bool, bool>(SetTime));
		}

		public static async void SetTime(int serverSecondOfDay, long serverDate, bool isTimeFrozen, bool cambio = false)
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
			if (NEWsecondOfDay < secondOfDay - 400 || NEWsecondOfDay > secondOfDay + 400)
				secondOfDay = NEWsecondOfDay;
			await BaseScript.Delay(32);
			if (!Frozen)
			{
				float gameSecond = 33.33f;
				_timeBuffer += (float)Math.Round(33.0f / gameSecond, 4);
				if(_timeBuffer >= 1.0f)
				{
					int skipSeconds = (int)Math.Floor(_timeBuffer);
					_timeBuffer -= skipSeconds;
					secondOfDay += skipSeconds;
					if (secondOfDay >= 86400)
						secondOfDay %= 86400;
				}
			}
			h = (int)Math.Floor(secondOfDay / 3600f);
			m = (int)Math.Floor((secondOfDay - (h * 3600f)) / 60);
			s = secondOfDay - (h * 3600) - (m * 60);
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
				if (Game.PlayerPed.IsInVehicle())
					Game.PlayerPed.IsPositionFrozen = true;
				else
					Game.PlayerPed.IsPositionFrozen = true;
				await BaseScript.Delay(2000);
				NetworkOverrideClockTime(h, m, s);
				await BaseScript.Delay(1950);
				if (Game.PlayerPed.IsInVehicle())
					Game.PlayerPed.IsPositionFrozen = false;
				else
					Game.PlayerPed.IsPositionFrozen = false;
				Screen.Fading.FadeIn(800);
				Cambio = false;
			}
			else 
				NetworkOverrideClockTime(h, m, s);
		}
	}
}
