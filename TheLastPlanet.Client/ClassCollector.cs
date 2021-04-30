using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Client.RolePlay.Proprietà.Appartamenti.Case;
using System.Threading.Tasks;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Core;
using System.Linq;
using TheLastPlanet.Client.RolePlay.Core;

namespace TheLastPlanet.Client
{
	internal static class ClassCollector
	{
		public static async Task Init()
		{
			ClasseDiTest.Init(); // da rimouvere
			Core.BucketChooser.MainChooser.Init();
			DecorationClass.DichiaraDecor();
			await ConfigClient.Init();
			await CoreInitializer.LogInInitializer();
			await SessionCache.Cache.Loaded();
			RolePlay.Banking.BankingClient.Init();
			RolePlay.Businesses.PompeDiBenzinaClient.Init();
			PublicTraffic.Init();
			RolePlay.Core.CharCreation.Creator.Init();
			RolePlay.Core.CharCreation.CamerasFirstTime.Init();
			RolePlay.Core.Status.Death.Init();
			RolePlay.Core.Status.StatsNeeds.Init();
			Voice.Init();
			RolePlay.Lavori.Whitelistati.Polizia.PoliziaMainClient.Init();
			RolePlay.Lavori.Whitelistati.Medici.MediciMainClient.Init();
			RolePlay.Lavori.Generici.Pescatore.PescatoreClient.Init();
			RolePlay.Lavori.Generici.Cacciatore.CacciatoreClient.Init();
			RolePlay.Lavori.Generici.Rimozione.RimozioneClient.Init();
			RolePlay.Lavori.Generici.Taxi.TaxiClient.Init();
			//			Lavori.JobManager.Init();
			Manager.ClientManager.Init();
			Manager.DevManager.Init();
			RolePlay.Proprietà.Manager.Init();
			AppartamentiClient.Init();
			RolePlay.Negozi.BarberClient.Init();
			RolePlay.Negozi.NegozioAbitiClient.Init();
			RolePlay.Negozi.NegoziBusiness.Init();
			RolePlay.Negozi.NegoziClient.Init();
			ListaPlayers.FivemPlayerlist.Init();
			RolePlay.Veicoli.FuelClient.Init();
			RolePlay.Veicoli.VehicleDamage.Init();
			RolePlay.Veicoli.VeicoliClient.Init();
			RolePlay.Veicoli.Treni.Init();
			RolePlay.Veicoli.EffettiRuote.Init();
			RolePlay.Veicoli.VehHud.Init();

			//Giostre.MontagneRusse.Init();
			//Giostre.RuotaPanoramica.Init();
			//Giostre.Funivia.Init();
			IPLInstance.Init();
			RolePlay.Proprietà.Hotel.Hotels.Init();
			RolePlay.Interactions.Macchinette.Init();
			RolePlay.Interactions.Docce.Init();
			RolePlay.Interactions.PickupsClient.Init();
			RolePlay.Interactions.OggettiGenerici.Init();
			RolePlay.Interactions.PrimaPersonaObbligatoria.Init();
			InternalGameEvents.Init();
			RolePlay.Sport.Yoga.Init();

			//Telefono.PhoneMainClient.Init();
			//CodaControl.CodaAdminPanel.Init();
			RolePlay.Lavori.Whitelistati.VenditoreAuto.CarDealer.Init();
			RolePlay.Lavori.Whitelistati.VenditoreCase.HouseDealer.Init();
			PauseMenu.Init();
			Minimap.Init();
			TickController.Init();
			InputHandler.Init();

			//TEST
			//KeyMappingsTest.Init();
			await Task.FromResult(0);
		}
	}
}