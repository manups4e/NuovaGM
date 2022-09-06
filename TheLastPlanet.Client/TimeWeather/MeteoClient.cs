using System;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.TimeWeather
{
    internal static class MeteoClient
    {
        public static SharedWeather Meteo { get; set; }
        public static int OldWeather;
        public static bool Transitioning = false;
        private static float _windCostantDirectRad = 51.4285714286f;
        private static ParticleEffectsAssetNetworked snowFx;

        public static void Init()
        {
            EventDispatcher.Mount("tlg:getMeteo", new Action<SharedWeather>(SetMeteo));
            Client.Instance.StateBagsHandler.OnWeatherChange += SetMeteo;
        }

        public static void Stop()
        {
            EventDispatcher.Unmount("tlg:getMeteo");
            Client.Instance.StateBagsHandler.OnWeatherChange -= SetMeteo;
        }

        public static void SetMeteoPerMe(Weather met, bool black, bool startup)
        {
            World.TransitionToWeather(met, startup ? 15f : 45f);
            SetBlackout(black);
        }

        public static async void SetMeteo(SharedWeather meteo)
        {
            if (Meteo != meteo)
            {
                Meteo = meteo;
                if (meteo.CurrentWeather != (int)World.Weather)
                {
                    World.TransitionToWeather((Weather)Meteo.CurrentWeather, Meteo.StartUp ? 1f : 45f);
                    if (Meteo.CurrentWeather == 10 || Meteo.CurrentWeather == 11 || Meteo.CurrentWeather == 12 || Meteo.CurrentWeather == 13)
                    {
                        SetForceVehicleTrails(true);
                        SetForcePedFootstepsTracks(true);
                        ForceSnowPass(true);
                        RequestScriptAudioBank("ICE_FOOTSTEPS", false);
                        RequestScriptAudioBank("SNOW_FOOTSTEPS", false);
                        World.CloudHat = CloudHat.Snowy;
                        snowFx = new ParticleEffectsAssetNetworked("core_snow");
                        await snowFx.Request(1000);
                        while (!snowFx.IsLoaded) await BaseScript.Delay(0);
                        snowFx.SetNextCall();
                    }
                    else
                    {
                        SetForceVehicleTrails(false);
                        SetForcePedFootstepsTracks(false);
                        ForceSnowPass(false);
                        ReleaseScriptAudioBank();
                        if (snowFx is not null && snowFx.IsLoaded) snowFx.MarkAsNoLongerNeeded();
                    }
                }
            }

            if ((Meteo.CurrentWeather == 10 || Meteo.CurrentWeather == 11 || Meteo.CurrentWeather == 12 || Meteo.CurrentWeather == 13) && !IPLs.IplManager.Global.IsAnyInteriorActive)
            {
                if (snowFx is null)
                {
                    snowFx = new ParticleEffectsAssetNetworked("core_snow");
                    await snowFx.Request(1000);
                }
                if (!snowFx.IsLoaded) await snowFx.Request(1000);
                snowFx.SetNextCall();
            }

            SetBlackout(Meteo.Blackout);
            SetWindDirection(Meteo.RandomWindDirection);
            //SetWind(ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_Mult[newWeather] + 0.1f * ConfigShared.SharedConfig.Main.Meteo.ss_wind_speed_max);
            SetWindSpeed(Meteo.WindSpeed);
        }
    }
}