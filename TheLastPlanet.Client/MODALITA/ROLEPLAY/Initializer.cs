using System.Threading.Tasks;
using TheLastPlanet.Client.AdminAC;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Banking;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Businesses;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.CharCreation;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà.Appartamenti.Case;

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
			Lavori.Whitelistati.Polizia.PoliziaMainClient.Init();
			Lavori.Whitelistati.Medici.MediciMainClient.Init();
			Lavori.Generici.Pescatore.PescatoreClient.Init();
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
			Interactions.Macchinette.Init();
			Interactions.Docce.Init();
			Interactions.PickupsClient.Init();
			Interactions.OggettiGenerici.Init();
			Interactions.PrimaPersonaObbligatoria.Init();
			Sport.Yoga.Init();

			//Telefono.PhoneMainClient.Init();
			//CodaControl.CodaAdminPanel.Init();
			Lavori.Whitelistati.VenditoreAuto.CarDealer.Init();
			Lavori.Whitelistati.VenditoreCase.HouseDealer.Init();
			PauseMenu.Init();
			TickController.Init();
			await Task.FromResult(0);
		}

		public static async Task Stop()
		{
			//ClasseDiTest.Stop(); // da rimouvere
			DecorationClass.Stop();
			BankingClient.Stop();
			PompeDiBenzinaClient.Stop();
			PublicTraffic.Stop();
			Creator.Stop();
			CamerasFirstTime.Stop();
			Core.Status.Death.Stop();
			Core.Status.StatsNeeds.Stop();
			Lavori.Whitelistati.Polizia.PoliziaMainClient.Stop();
			Lavori.Whitelistati.Medici.MediciMainClient.Stop();
			Lavori.Generici.Pescatore.PescatoreClient.Stop();
			Lavori.Generici.Cacciatore.CacciatoreClient.Stop();
			Lavori.Generici.Rimozione.RimozioneClient.Stop();
			Lavori.Generici.Taxi.TaxiClient.Stop();
			Proprietà.Manager.Stop();
			AppartamentiClient.Stop();
			Negozi.BarberClient.Stop();
			Negozi.NegozioAbitiClient.Stop();
			NegoziBusiness.Stop();
			Negozi.NegoziClient.Stop();
			Veicoli.FuelClient.Stop();
			Veicoli.VehicleDamage.Stop();
			Veicoli.VeicoliClient.Stop();
			Veicoli.Treni.Stop();
			Veicoli.EffettiRuote.Stop();
			Veicoli.VehHud.Stop();
			IPLInstance.Stop(); // da finire
			Proprietà.Hotel.Hotels.Stop();
			Interactions.Macchinette.Stop();
			Interactions.Docce.Stop();
			Interactions.PickupsClient.Stop();
			Interactions.OggettiGenerici.Stop();
			Interactions.PrimaPersonaObbligatoria.Stop();
			Sport.Yoga.Stop();
			Lavori.Whitelistati.VenditoreAuto.CarDealer.Stop();
			Lavori.Whitelistati.VenditoreCase.HouseDealer.Stop();
			PauseMenu.Stop();
			Minimap.Stop();
			TickController.Stop();
			await CoreInitializer.LogInStop();
			await Task.FromResult(0);
		}
	}
}
