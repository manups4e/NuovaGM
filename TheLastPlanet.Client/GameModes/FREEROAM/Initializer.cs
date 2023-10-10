using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.FREEROAM.Managers;
using TheLastPlanet.Client.GameMode.FREEROAM.Scripts.Negozi;
using TheLastPlanet.Client.GameMode.FREEROAM.Scripts.PauseMenu;
using TheLastPlanet.Client.GameMode.FREEROAM.Spawner;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Client.TimeWeather;

namespace TheLastPlanet.Client.GameMode.FREEROAM
{
    class Initializer
    {
        public static async Task Init()
        {
            IPLInstance.Init();
            FreeRoamLogin.Inizializza();
            HudManager.Init();
            ExperienceManager.Init();
            WorldEventsManager.Init();
            PlayerBlipsHandler.Init();
            BaseEventsFreeRoam.Init();
            PlayerTags.Init();
            WeatherClient.Init();
            TimeClient.Init();
            PauseMenuFreeroam.Init();
            WeaponShops.Init();
            // TODO: ADD STATISTICS HANDLING
            // TODO: ADD BETTER DEATH HANDLING?
            SetAmbientPedsDropMoney(true);
            await Task.FromResult(0);
        }

        public static async Task Stop()
        {
            WeatherClient.Stop();
            TimeClient.Stop();
            AccessingEvents.FreeRoamLeave(PlayerCache.MyPlayer);
            HudManager.Stop();
            WeaponShops.Stop();
            // TODO: SAME AS ABOVE
            SetAmbientPedsDropMoney(false);
            await Task.FromResult(0);
        }
    }
}
