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
using TheLastPlanet.Server.RolePlay.Core;
using TheLastPlanet.Server.FREEROAM;
using TheLastPlanet.Server.FREEROAM.Scripts.EventiFreemode;

namespace TheLastPlanet.Server
{
	internal static class ClassCollector
	{
		public static async Task Init()
		{
			BucketsHandler.Init();
			RequestInternal.Init();
			NewServerEntrance.Init();
			await ConfigServer.Init();
			while (Server.Impostazioni == null) await BaseScript.Delay(0);
			ServerManager.Init();
			Main.Init();
			Eventi.Init();
			EventiRolePlay.Init();
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
			PlayerListServer.Init();
			AppartamentiServer.Init();
			FuelServer.Init();
			VeicoliServer.Init();
			GiostreServer.Init();
			//Telefoni.TelefonoMainServer.Init();
			TimeWeather.MeteoServer.Init();
			TimeWeather.OrarioServer.Init();
			PickupsServer.Init();
			//NuovaCoda.Init();
			ServerManager.Init();
			BotDiscordHandler.Init();
			WorldEventsManager.Init();
			VehicleManager.Init();
			PlayerBlipsHandler.Init();
			BaseEventsFreeRoam.Init();
			EventiFreeRoam.Init();
			await Task.FromResult(0);
		}
	}
}