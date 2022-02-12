using System.Threading.Tasks;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Client.MODALITA.FREEROAM.Managers;
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
            ExperienceManager.Init();
            WorldEventsManager.Init();
            PlayerBlipsHandler.Init();
            BaseEventsFreeRoam.Init();
            PlayerTags.Init();
            MeteoClient.Init();
            OrarioClient.Init();
            MenuPausa.Init();
            //AGGIUNGERE GESTIONE METEO
            //AGGIUNGERE GESTIONE ORARIO
            //AGGIUNGERE GESTIONE STATISTICHE
            //AGGIUNGERE GESTIONE MORTE (SE POSSIBILE SERVERSIDE)
            //Death.Init();
            await Task.FromResult(0);
        }

        public static async Task Stop()
        {
            IPLInstance.Stop();
            ExperienceManager.Stop();
            WorldEventsManager.Stop();
            PlayerBlipsHandler.Stop();
            BaseEventsFreeRoam.Stop();
            PlayerTags.Stop();
            MeteoClient.Stop();
            OrarioClient.Stop();
            MenuPausa.Stop();

            //AGGIUNGERE GESTIONE STATISTICHE
            //AGGIUNGERE GESTIONE MORTE (SE POSSIBILE SERVERSIDE)

            await Task.FromResult(0);
        }
    }
}
