using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuovaGM.Shared;

namespace NuovaGM.Server.TimeWeather
{
	static class Meteo
	{
		public static int currentWeather;
		private static bool blackout = false;
		private static bool rainPossible = false;
		public static int weatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
		private static int rainTimer = ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60;

		public static void Init()
		{
			Server.Instance.RegisterEventHandler("changeWeatherWithParams", new Action<int, bool, bool>(CambiaMeteoConParams));
			Server.Instance.RegisterEventHandler("changeWeatherDynamic", new Action<bool>(CambiaMeteoDinamico));
			Server.Instance.RegisterEventHandler("changeWeather", new Action<bool>(CambiaMeteo));
			Server.Instance.RegisterEventHandler("changeWeatherForMe", new Action<Player, bool>(CambiaMeteoPerMe));
			Server.Instance.RegisterTickHandler(Conteggio);
			currentWeather = ConfigShared.SharedConfig.Main.Meteo.ss_default_weather;
			weatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
			rainTimer = ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60;
		}

		private static void CambiaMeteoPerMe([FromSource]Player p, bool startup)
		{
			p.TriggerEvent("lprp:getMeteo", currentWeather, blackout, startup);
		}

//		[EventHandler("changeWeatherWithParams")]
		private static void CambiaMeteoConParams(int meteo, bool black, bool startup)
		{
			currentWeather = meteo;
			weatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
			blackout = black;
			BaseScript.TriggerClientEvent("lprp:getMeteo", currentWeather, blackout, startup);
		}

		private static void CambiaMeteoDinamico(bool dynamic)
		{
			ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather = dynamic;
			BaseScript.TriggerClientEvent("CambiaMeteoDinamicoPerTutti", dynamic);
		}

		private static void CambiaMeteo(bool startup)
		{
			BaseScript.TriggerClientEvent("lprp:getMeteo", currentWeather, blackout, startup);
		}

		public static async Task Conteggio()
		{
			await BaseScript.Delay(1000);
			weatherTimer--;
			if (rainPossible) rainTimer = -1;
			else rainTimer--;
			if (weatherTimer == 0)
			{
				if (ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather)
				{
					List<int> currentOptions = ConfigShared.SharedConfig.Main.Meteo.ss_weather_Transition[currentWeather];
					currentWeather = currentOptions[new Random(DateTime.Now.Millisecond).Next(currentOptions.Count - 1)];
					if (ConfigShared.SharedConfig.Main.Meteo.ss_reduce_rain_chance)
						foreach (var p in currentOptions)
							if (p == 7 || p == 8)
								currentWeather = currentOptions[new Random(DateTime.Now.Millisecond).Next(currentOptions.Count - 1)];

					if (rainPossible == false)
						while (currentWeather == 7 || currentWeather == 8)
							currentWeather = currentOptions[new Random(DateTime.Now.Millisecond).Next(currentOptions.Count - 1)];

					if (currentWeather == 7 || currentWeather == 8)
					{
						rainTimer = ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60;
						rainPossible = false;
					}
					BaseScript.TriggerEvent("changeWeather", false);
					weatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
				}
			}
			if (rainTimer == 0)
				rainPossible = true;
		}
	}
}