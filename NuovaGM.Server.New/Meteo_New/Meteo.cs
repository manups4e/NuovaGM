using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuovaGM.Shared.Weather;
using CitizenFX.Core.Native;
using Newtonsoft.Json;

namespace NuovaGM.Server.Meteo_New
{
	static class Meteo
	{
		public static int currentWeather;
		private static bool blackout = false;
		private static bool rainPossible = false;
		public static int weatherTimer = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
		private static int rainTimer = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60;

		public static void Init()
		{
			Server.GetInstance.RegisterEventHandler("changeWeatherWithParams", new Action<int, bool, bool>(CambiaMeteoConParams));
			Server.GetInstance.RegisterEventHandler("changeWeatherDynamic", new Action<bool>(CambiaMeteoDinamico));
			Server.GetInstance.RegisterEventHandler("changeWeather", new Action<bool>(CambiaMeteo));
			Server.GetInstance.RegisterEventHandler("changeWeatherForMe", new Action<Player, bool>(CambiaMeteoPerMe));
			Server.GetInstance.RegisterTickHandler(Conteggio);
			currentWeather = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_default_weather;
			weatherTimer = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
			rainTimer = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60;
		}

		private static async void CambiaMeteoPerMe([FromSource]Player p, bool startup)
		{
			p.TriggerEvent("lprp:getMeteo", currentWeather, blackout, startup);
		}

		private static async void CambiaMeteoConParams(int meteo, bool black, bool startup)
		{
			currentWeather = meteo;
			weatherTimer = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
			blackout = black;
			BaseScript.TriggerClientEvent("lprp:getMeteo", currentWeather, blackout, startup);
		}

		private static async void CambiaMeteoDinamico(bool dynamic)
		{
			Shared.ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather = dynamic;
			BaseScript.TriggerClientEvent("CambiaMeteoDinamicoPerTutti", dynamic);
		}

		private static async void CambiaMeteo(bool startup)
		{
			BaseScript.TriggerClientEvent("lprp:getMeteo", currentWeather, blackout, startup);
		}

		public static async Task Conteggio()
		{
			await BaseScript.Delay(1000);
			weatherTimer --;
			if (rainPossible) rainTimer = -1;
			else rainTimer--;
			if (weatherTimer == 0)
			{
				if (Shared.ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather)
				{
					PushNextWeather();
					weatherTimer = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
				}
			}
			if (rainTimer == 0)
				rainPossible = true;
		}

		private static async void PushNextWeather()
		{
			bool reduced;
			int reducedW;
			List<int> currentOptions = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_weather_Transition[currentWeather];
			currentWeather = currentOptions[new Random(DateTime.Now.Millisecond).Next(currentOptions.Count - 1)];
			if (Shared.ConfigShared.SharedConfig.Main.Meteo.ss_reduce_rain_chance)
			{
				foreach (var p in currentOptions)
				{
					if (p == 7 || p == 8)
					{
						currentWeather = currentOptions[new Random(DateTime.Now.Millisecond).Next(currentOptions.Count - 1)];
						reduced = true;
						reducedW = p;
					}
				}
			}

			if (rainPossible == false)
			{
				while (currentWeather == 7 || currentWeather == 8)
					currentWeather = currentOptions[new Random(DateTime.Now.Millisecond).Next(currentOptions.Count - 1)];
			}

			if (currentWeather == 7 || currentWeather == 8)
			{
				rainTimer = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60;
				rainPossible = false;
			}
			BaseScript.TriggerEvent("changeWeather", false);
		}
	}
}
