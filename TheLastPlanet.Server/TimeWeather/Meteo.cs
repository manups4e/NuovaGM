using CitizenFX.Core;
using Logger;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TheLastPlanet.Server.TimeWeather
{
	static class Meteo
	{
		public static int CurrentWeather;
		private static bool _blackout;
		private static bool _rainPossible;
		public static int WeatherTimer;
		private static int _rainTimer;

		public static void Init()
		{
			CurrentWeather = ConfigShared.SharedConfig.Main.Meteo.ss_default_weather;
			WeatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
			_rainTimer = ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60;
			ServerSession.Instance.AddEventHandler("changeWeatherWithParams", new Action<int, bool, bool>(CambiaMeteoConParams));
			ServerSession.Instance.AddEventHandler("changeWeatherDynamic", new Action<bool>(CambiaMeteoDinamico));
			ServerSession.Instance.AddEventHandler("changeWeather", new Action<bool>(CambiaMeteo));
			ServerSession.Instance.AddEventHandler("changeWeatherForMe", new Action<Player, bool>(CambiaMeteoPerMe));
			ServerSession.Instance.AddTick(Conteggio);
		}

		private static void CambiaMeteoPerMe([FromSource]Player p, bool startup)
		{
			p.TriggerEvent("lprp:getMeteo", CurrentWeather, _blackout, startup);
		}

//		[EventHandler("changeWeatherWithParams")]
		private static void CambiaMeteoConParams(int meteo, bool black, bool startup)
		{
			CurrentWeather = meteo;
			WeatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
			_blackout = black;
			BaseScript.TriggerClientEvent("lprp:getMeteo", CurrentWeather, _blackout, startup);
		}

		private static void CambiaMeteoDinamico(bool dynamic)
		{
			ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather = dynamic;
			BaseScript.TriggerClientEvent("CambiaMeteoDinamicoPerTutti", dynamic);
		}

		private static void CambiaMeteo(bool startup)
		{
			BaseScript.TriggerClientEvent("lprp:getMeteo", CurrentWeather, _blackout, startup);
		}

		public static async Task Conteggio()
		{
			try
			{
				await BaseScript.Delay(1000);
				WeatherTimer--;
				if (_rainPossible) _rainTimer = -1;
				else _rainTimer--;
				if (WeatherTimer == 0)
				{
					if (ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather)
					{
						List<int> currentOptions = ConfigShared.SharedConfig.Main.Meteo.ss_weather_Transition[CurrentWeather];
						CurrentWeather = currentOptions[new Random(DateTime.Now.Millisecond).Next(currentOptions.Count - 1)];
						if (ConfigShared.SharedConfig.Main.Meteo.ss_reduce_rain_chance)
							foreach (int p in currentOptions)
								if (p == 7 || p == 8)
									CurrentWeather = currentOptions[new Random(DateTime.Now.Millisecond).Next(currentOptions.Count - 1)];

						if (_rainPossible == false)
							while (CurrentWeather == 7 || CurrentWeather == 8)
								CurrentWeather = currentOptions[new Random(DateTime.Now.Millisecond).Next(currentOptions.Count - 1)];

						if (CurrentWeather == 7 || CurrentWeather == 8)
						{
							_rainTimer = ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60;
							_rainPossible = false;
						}
						BaseScript.TriggerEvent("changeWeather", false);
						WeatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
					}
				}
				if (_rainTimer == 0)
					_rainPossible = true;
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e + e.StackTrace);
			}
		}
	}
}