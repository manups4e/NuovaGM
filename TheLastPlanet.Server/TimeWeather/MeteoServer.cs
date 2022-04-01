using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.TimeWeather
{
    static class MeteoServer
    {
        public static SharedWeather Meteo;
        private static long _timer = 0;
        private static SharedTimer WeatherTimer;
        public static async void Init()
        {
            Server.Instance.Events.Mount("changeWeatherWithParams", new Action<int, bool, bool>(CambiaMeteoConParams));
            Server.Instance.Events.Mount("changeWeatherDynamic", new Action<bool>(CambiaMeteoDinamico));
            Server.Instance.Events.Mount("changeWeather", new Action<bool>(CambiaMeteo));
            Server.Instance.Events.Mount("SyncWeatherForMe", new Action<ClientId, bool>(SyncMeteoPerMe));
            Meteo = new SharedWeather()
            {
                CurrentWeather = ConfigShared.SharedConfig.Main.Meteo.ss_default_weather,
                WeatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60,
                RainTimer = ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60,
                RandomWindDirection = Funzioni.RandomFloatInRange(0, 359),
                WindSpeed = Funzioni.RandomFloatInRange(0, 12),
            };

            WeatherTimer = new(1000);
            Server.Instance.AddTick(Conteggio);
        }

        private static void SyncMeteoPerMe(ClientId p, bool startup)
        {
            Meteo.StartUp = startup;
            Server.Instance.Events.Send(p, "tlg:getMeteo", Meteo);
            Meteo.StartUp = false;
        }

        private static void CambiaMeteoConParams(int meteo, bool black, bool startup)
        {
            Meteo.CurrentWeather = meteo;
            Meteo.WeatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
            Meteo.Blackout = black;
            Server.Instance.ServerState.Set("Meteo", Meteo.ToBytes(), true);
        }

        private static void CambiaMeteoDinamico(bool dynamic)
        {
            ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather = dynamic;
            Server.Instance.ServerState.Set("Meteo", Meteo.ToBytes(), true);
        }

        public static void CambiaMeteo(bool startup)
        {
            if (startup) Meteo.StartUp = startup;
            var bytes = Meteo.ToBytes();
            Server.Instance.ServerState.Set("Meteo", bytes, true);
        }

        public static async Task Conteggio()
        {
            try
            {
                await BaseScript.Delay(1000);
                long tt = API.GetGameTimer();
                var rand = new Random((int)tt);
                if (Meteo.DynamicMeteo)
                {
                    Meteo.WeatherTimer--;
                    if (Meteo.RainPossible) Meteo.RainTimer = -1;
                    else Meteo.RainTimer--;
                    if (Meteo.WeatherTimer == 0)
                    {
                        if (ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather)
                        {
                            List<int> currentOptions = ConfigShared.SharedConfig.Main.Meteo.ss_weather_Transition[Meteo.CurrentWeather];
                            Meteo.CurrentWeather = currentOptions[rand.Next(currentOptions.Count - 1)];
                            if (ConfigShared.SharedConfig.Main.Meteo.ss_reduce_rain_chance)
                                foreach (int p in currentOptions)
                                    if (p == 7 || p == 8)
                                        Meteo.CurrentWeather = currentOptions[rand.Next(currentOptions.Count - 1)];

                            if (Meteo.RainPossible == false)
                                while (Meteo.CurrentWeather == 7 || Meteo.CurrentWeather == 8)
                                    Meteo.CurrentWeather = currentOptions[rand.Next(currentOptions.Count - 1)];

                            if (Meteo.CurrentWeather == 7 || Meteo.CurrentWeather == 8)
                            {
                                Meteo.RainTimer = ConfigShared.SharedConfig.Main.Meteo.ss_rain_timeout * 60;
                                Meteo.RainPossible = false;
                            }
                            Meteo.WeatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
                        }
                    }
                    if (Meteo.RainTimer == 0)
                        Meteo.RainPossible = true;
                }
                if (tt - _timer > 600000)
                {
                    Meteo.RandomWindDirection = Funzioni.RandomFloatInRange(0, 359);
                    Meteo.WindSpeed = Funzioni.RandomFloatInRange(0, 12);
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