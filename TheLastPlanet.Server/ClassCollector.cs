using FivemPlayerlistServer;
using System.Threading.Tasks;
using TheLastPlanet.Server.Apartments;
using TheLastPlanet.Server.banking;
using TheLastPlanet.Server.Businesses;
using TheLastPlanet.Server.Clothestores;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Server.Core.PlayerJoining;
using TheLastPlanet.Server.Discord;
using TheLastPlanet.Server.FreeRoam.Scripts.FreeroamEvents;
using TheLastPlanet.Server.Interactions;
using TheLastPlanet.Server.Jobs.Whitelisted;
using TheLastPlanet.Server.manager;
using TheLastPlanet.Server.RolePlay.Core;
using TheLastPlanet.Server.Vehicles;

namespace TheLastPlanet.Server
{
    internal static class ClassCollector
    {
        public static async Task Init()
        {
            BucketsHandler.Init();
            NewServerEntrance.Init();
            await ConfigServer.Init();
            while (Server.Impostazioni == null) await BaseScript.Delay(0);
            ServerManager.Init();
            Main.Init();
            Events.Init();
            RolePlayEvents.Init();
            EntityCreation.Init();
            ChatServer.Init();
            ChatEvents.Init();
            BankingServer.Init();
            GasStationsServer.Init();
            ClotheShopsServer.Init();
            CarDealerServer.Init();
            HouseDealerServer.Init();
            PoliziaServer.Init();
            MedicsServer.Init();
            PlayerListServer.Init();
            ApartmentsServer.Init();
            FuelServer.Init();
            VeicoliServer.Init();
            LunaParkServer.Init();
            //Telefoni.TelefonoMainServer.Init();
            TimeWeather.ServerWeather.Init();
            //TimeWeather.OrarioServer.Init();
            PickupsServer.Init();
            //NuovaCoda.Init();
            ServerManager.Init();
            BotDiscordHandler.Init();
            WorldEventsManager.Init();
            VehicleManager.Init();
            //PlayerBlipsHandler.Init();
            BaseEventsFreeRoam.Init();
            FreeRoamEvents.Init();
            await Task.FromResult(0);
        }
    }
}