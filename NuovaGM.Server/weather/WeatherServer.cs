using CitizenFX.Core;
using System;
using System.Threading.Tasks;

namespace NuovaGM.Server.weather
{
	public static class WeatherServer
	{
		public static int lastWeatherType = -1;
		public static bool rainedRecently = false;
		public static int rainWait = 4800000;
		public static int rainWaitLow = 800000;
		public static int rainWaitHigh = 1000000;
		public static int waitLow = 900000;
		public static int waitHigh = 2700000;
		public static bool snowPersist = false;
		public static int wid;

		public static async void Init()
		{
			await BaseScript.Delay(5000);
			snowPersist = ConfigServer.Conf.Main.snowPersist;
			initWeather();
			Server.GetInstance.RegisterTickHandler(SyncWeather);
			Server.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action<Player>(PlayerSpawned));
		}

		public static void PlayerSpawned([FromSource] Player p)
		{
			initWeatherForPlayer(p);
		}
		public static int RandomNumber(int min, int max)
		{
			Random random = new Random(DateTime.Now.Millisecond);
			return random.Next(min, max);
		}

		public static async Task doTimer()
		{
			if (!snowPersist)
			{
				lastWeatherType = wid;
				await BaseScript.Delay(getRandomWaitTime());
				sendWeatherEvents(true, -1);
			}
		}

		public static void sendWeatherEvents(bool randomize, int widd)
		{
			if (!snowPersist)
			{
				BaseScript.TriggerClientEvent("lprp:weather:setWeatherTrans", lastWeatherType);
				if (randomize)
				{
					wid = getRandomWeatherType();
				}
				else if (widd > -1)
				{
					wid = widd;
				}

				doTimer();
			}
		}

		public static async void startRainTimeout()
		{
			await BaseScript.Delay(rainWait);
			rainedRecently = false;
		}

		public static int getRandomWeatherType()
		{
			int newwt = RandomNumber(1, 8);
			newwt = RandomNumber(1, 8);
			newwt = RandomNumber(1, 8);
			if (rainedRecently && newwt == 5 || newwt == 6 || newwt == 7 || newwt == 8 || newwt == 9)
			{
				newwt = RandomNumber(1, 4);
				newwt = RandomNumber(1, 4);
			}
			if (newwt == lastWeatherType)
			{
				newwt = RandomNumber(1, 8);
				newwt = RandomNumber(1, 8);
				newwt = RandomNumber(1, 8);
			}
			if (!rainedRecently && newwt == 5 || newwt == 6 || newwt == 7 || newwt == 8 || newwt == 9)
			{
				rainedRecently = true;
				startRainTimeout();
			}
			Log.Printa(LogType.Info, "Meteo Random: " + newwt);
			return newwt;
		}

		public static int getRandomWaitTime()
		{
			int wt;

			if (wid == 5 || wid == 6 || wid == 7 || wid == 8 || wid == 9)
			{
				wt = RandomNumber(rainWaitLow, rainWaitHigh);
			}
			else
			{
				wt = RandomNumber(waitLow, waitHigh);
			}
			return wt;
		}

		public static void initWeather()
		{
			if (!snowPersist)
			{
				wid = getRandomWeatherType();
				doTimer();
			}
		}

		public static void sendSnowEvents()
		{
			if (snowPersist)
			{
				Log.Printa(LogType.Info, "Attivando la never per tutti");
				BaseScript.TriggerClientEvent("lprp:weather:setSnowPersist", true);
			}
			else
			{
				sendWeatherEvents(true, -1);
			}
		}

		public static void initWeatherForPlayer(Player p)
		{
			if (!snowPersist)
			{
				BaseScript.TriggerClientEvent(p, "lprp:weather:setWeatherNow", lastWeatherType);
			}
			else
			{
				BaseScript.TriggerClientEvent(p, "lprp:weather:setSnowPersist", true);
			}
		}


		public static async Task SyncWeather()
		{
			await BaseScript.Delay(3600000);
			BaseScript.TriggerClientEvent("lprp:weather:setWeatherTrans", lastWeatherType);
		}
	}
}