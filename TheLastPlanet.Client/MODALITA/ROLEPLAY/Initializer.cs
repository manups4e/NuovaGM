using System.Threading.Tasks;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Banking;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Businesses;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.CharCreation;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Inventario;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà.Appartamenti.Case;
using TheLastPlanet.Client.RolePlay.MenuPausa;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY
{
    internal static class Initializer
    {
        public static async Task Init()
        {
            //ClasseDiTest.Init(); // da rimouvere
            DecorationClass.DichiaraDecor();
            await CoreInitializer.LogInInitializer();
            await Cache.PlayerCache.Loaded();
            BankingClient.Init();
            PompeDiBenzinaClient.Init();
            PublicTraffic.Init();
            Creator.Init();
            CamerasFirstTime.Init();
            Core.Status.Death.Init();
            Core.Status.StatsNeeds.Init();
            Voice.Init();
            BaseInventory.Init();
            Lavori.Whitelistati.Polizia.PoliziaMainClient.Init();
            Lavori.Whitelistati.Medici.MediciMainClient.Init();
            //Lavori.Generici.Pescatore.PescatoreClient.Init();
            Lavori.Generici.Cacciatore.CacciatoreClient.Init();
            Lavori.Generici.Rimozione.RimozioneClient.Init();
            Lavori.Generici.Taxi.TaxiClient.Init();
            Proprietà.Manager.Init();
            AppartamentiClient.Init();
            Negozi.BarberClient.Init();
            Negozi.NegozioAbitiClient.Init();
            NegoziBusiness.Init();
            Negozi.NegoziClient.Init();
            Veicoli.FuelClient.Init();
            Veicoli.VehicleDamage.Init();
            Veicoli.VeicoliClient.Init();
            Veicoli.Treni.Init();
            Veicoli.EffettiRuote.Init();
            Veicoli.VehHud.Init();

            //Giostre.MontagneRusse.Init();
            //Giostre.RuotaPanoramica.Init();
            //Giostre.Funivia.Init();
            IPLInstance.Init();
            Proprietà.Hotel.Hotels.Init();
            Macchinette.Init();
            Docce.Init();
            PickupsClient.Init();
            OggettiGenerici.Init();
            PrimaPersonaObbligatoria.Init();
            Pioggia.Init();
            Sport.Yoga.Init();

            //Telefono.PhoneMainClient.Init();
            //CodaControl.CodaAdminPanel.Init();
            Lavori.Whitelistati.VenditoreAuto.CarDealer.Init();
            Lavori.Whitelistati.VenditoreCase.HouseDealer.Init();
            PauseMenu.Init();
            TickController.Init();
            MapLooking.Init();
            await Task.FromResult(0);
        }

        public static async Task Stop()
        {
            //ClasseDiTest.Stop(); // da rimouvere
            AccessingEvents.RoleplayLeave(PlayerCache.MyPlayer);
            DecorationClass.Stop();
            Creator.Stop();
            CamerasFirstTime.Stop();
            //Lavori.Generici.Pescatore.PescatoreClient.Stop();
            NegoziBusiness.Stop();
            await CoreInitializer.LogInStop();
            await Task.FromResult(0);
        }
    }
}
