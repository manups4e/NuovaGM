using CitizenFX.Core;
using FivemPlayerlistServer;
using TheLastPlanet.Server.Appartamenti;
using TheLastPlanet.Server.banking;
using TheLastPlanet.Server.Businesses;
using TheLastPlanet.Server.Clothestores;
using TheLastPlanet.Server.Discord;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerJoining;
using TheLastPlanet.Server.Interactions;
using TheLastPlanet.Server.Lavori.Whitelistati;
using TheLastPlanet.Server.manager;
using TheLastPlanet.Server.Veicoli;
using System.Threading.Tasks;
using TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode;
using TheLastPlanet.Server.Core.Buckets;

namespace TheLastPlanet.Server
{
	internal static class ClassCollector
	{
		public static async Task Init()
		{
			BucketsHandler.Init();
			RequestInternal.Init();
			await ConfigServer.Init();
			while (Server.Impostazioni == null) await BaseScript.Delay(0);
			NewServerEntrance.Init();
			ServerManager.Init();
			Main.Init();
			Eventi.Init();
			EntityCreation.Init();
			ChatServer.Init();
			ChatEvents.Init();
			BankingServer.Init();
			PompeDiBenzinaServer.Init();
			NegozioAbitiServer.Init();
			CarDealerServer.Init();
			HouseDealerServer.Init();
			PoliziaServer.Init();
			MediciServer.Init();
			FPLServer.Init();
			AppartamentiServer.Init();
			FuelServer.Init();
			VeicoliServer.Init();
			GiostreServer.Init();
			//Telefoni.TelefonoMainServer.Init();
			TimeWeather.Meteo.Init();
			TimeWeather.Orario.Init();
			PickupsServer.Init();
			//NuovaCoda.Init();
			ServerManager.Init();
			BotDiscordHandler.Init();
			WorldEventsManager.Init();
			VehicleManager.Init();
			await Task.FromResult(0);
		}
	}
}