global using Main = TheLastPlanet.Client.GameMode.ROLEPLAY.Core.Main;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Banking;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Businesses;
using TheLastPlanet.Client.GameMode.ROLEPLAY.CharCreation;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Core;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Interactions;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Inventory;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Properties.Appartamenti.Case;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Client.RolePlay.MenuPausa;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY
{
    internal static class Initializer
    {
        public static async Task Init()
        {
            //ClasseDiTest.Init(); // da rimouvere
            DecorationClass.DeclareDecors();
            await CoreInitializer.LogInInitializer();
            await Cache.PlayerCache.Loaded();
            BankingClient.Init();
            GasStationsClient.Init();
            PublicTraffic.Init();
            Creator.Init();
            CamerasFirstTime.Init();
            Core.Status.Death.Init();
            Core.Status.StatsNeeds.Init();
            Voice.Init();
            BaseInventory.Init();
            Jobs.Whitelisted.Police.PoliceMainClient.Init();
            Jobs.Whitelisted.Medics.MedicsMainClient.Init();
            //Lavori.Generici.Pescatore.PescatoreClient.Init();
            Jobs.Generics.Hunt.HunterClient.Init();
            Jobs.Generics.Towing.TowingClient.Init();
            Jobs.Generics.Taxi.TaxiClient.Init();
            Properties.Manager.Init();
            ApartmentsClient.Init();
            Shops.BarberClient.Init();
            Shops.ClotheShopsMain.Init();
            BusinessShops.Init();
            Shops.ShopsClient.Init();
            Vehicles.FuelClient.Init();
            Vehicles.VehicleDamage.Init();
            Vehicles.VehiclesClient.Init();
            Vehicles.Trains.Init();
            Vehicles.WheelsEffects.Init();
            Vehicles.VehHud.Init();

            //Giostre.MontagneRusse.Init();
            //Giostre.RuotaPanoramica.Init();
            //Giostre.Funivia.Init();
            IPLInstance.Init();
            Properties.Hotel.HotelsClient.Init();
            VendingMachines.Init();
            Showers.Init();
            PickupsClient.Init();
            OggettiGenerici.Init();
            ForcedFirstPersonPOV.Init();
            RainHUD.Init();
            Sport.Yoga.Init();

            //Telefono.PhoneMainClient.Init();
            //CodaControl.CodaAdminPanel.Init();
            Jobs.Whitelisted.CarDealer.CarDealer.Init();
            Jobs.Whitelisted.HouseDealer.HouseDealer.Init();
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
            BusinessShops.Stop();
            await CoreInitializer.LogInStop();
            await Task.FromResult(0);
        }
    }
}
