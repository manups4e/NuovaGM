using System.Threading.Tasks;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Client.MODALITA.FREEROAM.Managers;
using TheLastPlanet.Client.MODALITA.FREEROAM.Scripts.Negozi;
using TheLastPlanet.Client.MODALITA.FREEROAM.Scripts.PauseMenu;
using TheLastPlanet.Client.MODALITA.FREEROAM.Spawner;
using TheLastPlanet.Client.TimeWeather;

namespace TheLastPlanet.Client.MODALITA.FREEROAM
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
            MeteoClient.Init();
            OrarioClient.Init();
            MenuPausa.Init();
            Armerie.Init();
            //AGGIUNGERE GESTIONE STATISTICHE
            //AGGIUNGERE GESTIONE MORTE (SE POSSIBILE SERVERSIDE)
            SetAmbientPedsDropMoney(true);
            await Task.FromResult(0);
        }

        public static async Task Stop()
        {
            MeteoClient.Stop();
            OrarioClient.Stop();
            AccessingEvents.FreeRoamLeave(PlayerCache.MyPlayer);
            HudManager.Stop();
            Armerie.Stop();
            //AGGIUNGERE GESTIONE STATISTICHE
            //AGGIUNGERE GESTIONE MORTE (SE POSSIBILE SERVERSIDE)
            SetAmbientPedsDropMoney(false);
            await Task.FromResult(0);
        }
    }
}
