using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using CitizenFX.Core.UI;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.TimeWeather
{
	public static class OrarioClient
	{
		private static int NEWsecondOfDay = 0;
		private static float _timeBuffer = 0;
		private static bool _cambio = false;
		public static int h = 0, m = 0, s = 0;
		public static ServerTime TempoOrario { get; set; }

		public static async void Init()
		{
			TempoOrario = new ServerTime();
			Client.Instance.Events.Mount("UpdateFromCommandTime", new Action<int>(CambiaOra));
			Client.Instance.AddTick(AggiornaTempo);
			Client.Instance.StateBagsHandler.OnTimeChange += CambiaTempo;
		}

		public static async void Stop()
		{
			Client.Instance.RemoveTick(AggiornaTempo);
		}

		public static void CambiaTempo(ServerTime tempo)
		{
			if (_cambio) return;
			TempoOrario = tempo;
		}

		public static async Task AggiornaTempo()
		{
			if (_cambio) return; 
			if (!TempoOrario.Frozen)
			{
				await BaseScript.Delay(33);
				_timeBuffer += 0.9900f;

				if (_timeBuffer > 1f)
				{
					_timeBuffer -= (int)Math.Floor(_timeBuffer);
					TempoOrario.SecondOfDay += (int)Math.Floor(_timeBuffer);
					if (TempoOrario.SecondOfDay > 86399) TempoOrario.SecondOfDay = 0;
				}

				h = (int)Math.Floor(TempoOrario.SecondOfDay / 3600f);
				m = (int)Math.Floor((TempoOrario.SecondOfDay - h * 3600f) / 60);
				s = TempoOrario.SecondOfDay - h * 3600 - m * 60;
			}
			NetworkOverrideClockTime(h, m, s);
		}

		public static async void CambiaOra(int tempo)
        {
			_cambio = true;
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
			TempoOrario.SecondOfDay = tempo;
			h = (int)Math.Floor(tempo / 3600f);
			m = (int)Math.Floor((tempo - h * 3600f) / 60);
			s = tempo - h * 3600 - m * 60;
			AdvanceClockTimeTo(h, m, s);
			await BaseScript.Delay(1950);
			if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InVeicolo)
				Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsPositionFrozen = false;
			else
				Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
			Screen.Fading.FadeIn(800);
			_cambio = false;
		}
	}
}