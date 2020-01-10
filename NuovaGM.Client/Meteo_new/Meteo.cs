using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;

namespace NuovaGM.Client.Meteo_new
{
	public static class Meteo
	{
		public static int CurrentWeather;
		public static int OldWeather;
		public static bool Transitioning = false;
		private static float _windCostantDirectRad = 51.4285714286f;

		private static float RandomWindDirection { get { return Funzioni.GetRandomInt(0, 359) / _windCostantDirectRad; } }

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:getMeteo", new Action<int, bool, bool>(SetMeteo));
			CurrentWeather = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_default_weather;
		}

		public static async void SetMeteo(int newWeather, bool blackout, bool startup)
		{
			Transitioning = false;
			if (newWeather != CurrentWeather)
			{
				OldWeather = CurrentWeather;
				CurrentWeather = newWeather;
				if (startup)
					World.TransitionToWeather((Weather)CurrentWeather, 1f);
				else
				{
					World.TransitionToWeather((Weather)CurrentWeather, 45f);
					Transitioning = true;
				}
				await BaseScript.Delay(100);
				Transitioning = false;
			}

			if (CurrentWeather == 10 || CurrentWeather == 11 || CurrentWeather == 12 || CurrentWeather == 13)
			{
				SetForceVehicleTrails(true);
				SetForcePedFootstepsTracks(true);
				RequestScriptAudioBank("SNOW_FOOTSTEPS", false);
				while (!HasNamedPtfxAssetLoaded("core_snow"))
				{
					await BaseScript.Delay(0);
					RequestNamedPtfxAsset("core_snow");
				}
				World.CloudHat = CloudHat.Snowy;
			}
			else
			{
				SetForceVehicleTrails(false);
				SetForcePedFootstepsTracks(false);
			}

			SetBlackout(blackout);
			SetWindDirection(RandomWindDirection);
			SetWind(Shared.ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_Mult[newWeather] + 0.1f * Shared.ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_max);
			SetWindSpeed(Shared.ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_Mult[newWeather] + 0.1f * Shared.ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_max);
		}

		public static async Task PulisciVeicolo()
		{
			await BaseScript.Delay(30000);
			if (!Transitioning)
				if (CurrentWeather == 7 || CurrentWeather == 8)
					Game.PlayerPed.CurrentVehicle.DirtLevel -= 0.1f;
		}
	}
}
