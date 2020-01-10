using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;

namespace NuovaGM.Client.weather
{
	static class WeatherClient
	{
		private static bool _snowPersist;

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:weather:setWeatherTrans", new Action<int>(SetWeatherTrans));
			Client.GetInstance.RegisterEventHandler("lprp:weather:setWeatherNow", new Action<int>(setWeatherNow));
			Client.GetInstance.RegisterEventHandler("lprp:weather:setSnowPersist", new Action<bool>(setSnowPersist));
		}

		public static void SetWeatherTrans(int weatherType)
		{
			if (_snowPersist || !(weatherType.GetType() == typeof(int)))
			{
				return;
			}

			World.TransitionToWeather((Weather)weatherType, 45f);
		}

		public static void setWeatherNow(int weatherType)
		{
			if (_snowPersist || !(weatherType.GetType() == typeof(int)))
			{
				return;
			}

			World.TransitionToWeather((Weather)weatherType, 10f);

		}
		public static void setSnowPersist(bool persist)
		{
			_snowPersist = persist;
			Debug.Write("Settaggio neve persistente = " + _snowPersist.ToString());
			if (!_snowPersist)
			{
				return;
			}

			World.TransitionToWeather((Weather)13, 10f);
			Function.Call((Hash)5532655643731181536L, true);
			Function.Call((Hash)5841822839668843328L, true);
		}
	}
}
