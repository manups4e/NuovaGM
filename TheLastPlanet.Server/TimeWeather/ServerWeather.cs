using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;


namespace TheLastPlanet.Server.TimeWeather
{
    static class ServerWeather
    {
        public static SharedWeather Weather;
        private static long _timer = 0;
        private static SharedTimer WeatherTimer;
        public static async void Init()
        {
            EventDispatcher.Mount("changeWeatherWithParams", new Action<int, bool, bool>(CambiaMeteoConParams));
            EventDispatcher.Mount("changeWeatherDynamic", new Action<bool>(CambiaMeteoDinamico));
            EventDispatcher.Mount("changeWeather", new Action<bool>(CambiaMeteo));
            EventDispatcher.Mount("SyncWeatherForMe", new Action<PlayerClient, bool>(SyncMeteoPerMe));
            Weather = new SharedWeather()
            {
                CurrentWeather = ConfigShared.SharedConfig.Main.Weather.ss_default_weather,
                WeatherTimer = ConfigShared.SharedConfig.Main.Weather.ss_weather_timer * 60,
                RainTimer = ConfigShared.SharedConfig.Main.Weather.ss_rain_timeout * 60,
                RandomWindDirection = Funzioni.RandomFloatInRange(0, 359),
                WindSpeed = Funzioni.RandomFloatInRange(0, 12),
            };

            WeatherTimer = new(1000);
            Server.Instance.AddTick(Conteggio);
        }

        private static void SyncMeteoPerMe([FromSource] PlayerClient p, bool startup)
        {
            Weather.StartUp = startup;
            EventDispatcher.Send(p, "tlg:getMeteo", Weather);
            Weather.StartUp = false;
        }

        private static void CambiaMeteoConParams(int meteo, bool black, bool startup)
        {
            Weather.CurrentWeather = meteo;
            Weather.WeatherTimer = ConfigShared.SharedConfig.Main.Weather.ss_weather_timer * 60;
            Weather.Blackout = black;
            Server.Instance.ServerState.Set("Meteo", Weather.ToBytes(), true);
        }

        private static void CambiaMeteoDinamico(bool dynamic)
        {
            ConfigShared.SharedConfig.Main.Weather.ss_enable_dynamic_weather = dynamic;
            Server.Instance.ServerState.Set("Meteo", Weather.ToBytes(), true);
        }

        public static void CambiaMeteo(bool startup)
        {
            if (startup) Weather.StartUp = startup;
            byte[] bytes = Weather.ToBytes();
            Server.Instance.ServerState.Set("Meteo", bytes, true);
        }

        public static async Task Conteggio()
        {
            try
            {
                await BaseScript.Delay(1000);
                long tt = API.GetGameTimer();
                Random rand = new Random((int)tt);
                if (Weather.DynamicMeteo)
                {
                    Weather.WeatherTimer--;
                    if (Weather.RainPossible) Weather.RainTimer = -1;
                    else Weather.RainTimer--;
                    if (Weather.WeatherTimer == 0)
                    {
                        if (ConfigShared.SharedConfig.Main.Weather.ss_enable_dynamic_weather)
                        {
                            List<int> currentOptions = ConfigShared.SharedConfig.Main.Weather.ss_weather_Transition[Weather.CurrentWeather];
                            Weather.CurrentWeather = currentOptions[rand.Next(currentOptions.Count - 1)];
                            if (ConfigShared.SharedConfig.Main.Weather.ss_reduce_rain_chance)
                                foreach (int p in currentOptions)
                                    if (p == 7 || p == 8)
                                        Weather.CurrentWeather = currentOptions[rand.Next(currentOptions.Count - 1)];

                            if (Weather.RainPossible == false)
                                while (Weather.CurrentWeather == 7 || Weather.CurrentWeather == 8)
                                    Weather.CurrentWeather = currentOptions[rand.Next(currentOptions.Count - 1)];

                            if (Weather.CurrentWeather == 7 || Weather.CurrentWeather == 8)
                            {
                                Weather.RainTimer = ConfigShared.SharedConfig.Main.Weather.ss_rain_timeout * 60;
                                Weather.RainPossible = false;
                            }
                            Weather.WeatherTimer = ConfigShared.SharedConfig.Main.Weather.ss_weather_timer * 60;
                        }
                    }
                    if (Weather.RainTimer == 0)
                        Weather.RainPossible = true;
                }
                if (tt - _timer > 600000)
                {
                    Weather.RandomWindDirection = Funzioni.RandomFloatInRange(0, 359);
                    Weather.WindSpeed = Funzioni.RandomFloatInRange(0, 12);
                    _timer = tt;
                }
                CambiaMeteo(false);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e + e.StackTrace);
            }
        }
    }
}