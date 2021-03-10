using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.TimeWeather
{
	internal static class Meteo
	{
		public static int CurrentWeather;
		public static int OldWeather;
		public static bool Transitioning = false;
		public static bool BlackOut = false;
		private static float _windCostantDirectRad = 51.4285714286f;
		private static Prop xMas;
		private static int blockArea;
		private static float RandomWindDirection => Funzioni.GetRandomInt(0, 359) / _windCostantDirectRad;

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:getMeteo", new Action<int, bool, bool>(SetMeteo));
			Client.Instance.AddEventHandler("CambiaMeteoDinamicoPerTutti", new Action<bool>(SetDynamic));
		}

		public static void SetDynamic(bool dynamic) { ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather = dynamic; }

		public static async void SetMeteo(int newWeather, bool blackout, bool startup)
		{
			Transitioning = false;
			BlackOut = blackout;

			if (newWeather != CurrentWeather)
			{
				OldWeather = CurrentWeather;
				CurrentWeather = newWeather;

				if (startup)
				{
					World.TransitionToWeather((Weather)CurrentWeather, 1f);
				}
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
				ForceSnowPass(true);
				RequestScriptAudioBank("ICE_FOOTSTEPS", false);
				RequestScriptAudioBank("SNOW_FOOTSTEPS", false);
				RequestNamedPtfxAsset("core_snow");
				while (!HasNamedPtfxAssetLoaded("core_snow")) await BaseScript.Delay(0);
				UseParticleFxAssetNextCall("core_snow");
				World.CloudHat = CloudHat.Snowy;
			}
			else
			{
				SetForceVehicleTrails(false);
				SetForcePedFootstepsTracks(false);
				ForceSnowPass(false);
				ReleaseScriptAudioBank();
			}

			SetBlackout(BlackOut);
			SetWindDirection(RandomWindDirection);
			SetWind(ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_Mult[newWeather] + 0.1f * ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_max);
			SetWindSpeed(ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_Mult[newWeather] + 0.1f * ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_max);
		}

		public static async Task PulisciVeicolo()
		{
			await BaseScript.Delay(30000);
			if (!Transitioning)
				if (CurrentWeather == 7 || CurrentWeather == 8)
					Cache.Cache.MyPlayer.Ped.CurrentVehicle.DirtLevel -= 0.1f;
		}
	}
}