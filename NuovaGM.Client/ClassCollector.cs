using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Client.Proprietà.Appartamenti.Case;
using System.Threading.Tasks;

namespace TheLastPlanet.Client
{
	static class ClassCollector
	{
		public static async void Init()
		{
			await DecorationClass.DichiaraDecor();
			await ConfigClient.Init();
			Core.Main.Init();
			Core.NuovoIngresso.NuovoIngresso.Init();
			Core.Utility.HUD.HUD.Init();
			Core.Utility.Eventi.Init();
			Core.Discord.Init();
			TimeWeather.Meteo.Init();
			TimeWeather.Orario.Init();
			while (Game.Player.GetPlayerData() == null) await BaseScript.Delay(0);

			Banking.BankingClient.Init();

			Businesses.PompeDiBenzinaClient.Init();

			Core.Utility.PublicTraffic.Init();
			Core.MenuGm.Menus.Init();
			Core.MenuGm.CamerasFirstTime.Init();
			Core.Status.Death.Init();
			Core.Status.StatsNeeds.Init();
			Core.Voice.Init();

			Lavori.Whitelistati.Polizia.PoliziaMainClient.Init();
			Lavori.Whitelistati.Medici.MediciMainClient.Init();
			Lavori.Generici.Pescatore.PescatoreClient.Init();
			Lavori.Generici.Cacciatore.CacciatoreClient.Init();
			Lavori.Generici.Rimozione.RimozioneClient.Init();
			Lavori.Generici.Taxi.TaxiClient.Init();
//			Lavori.JobManager.Init();

			Manager.ClientManager.Init();
			Manager.DevManager.Init();

			Proprietà.Manager.Init();
			AppartamentiClient.Init();
			Negozi.BarberClient.Init();
			Negozi.NegozioAbitiClient.Init();
			Negozi.NegoziBusiness.Init();
			Negozi.NegoziClient.Init();

			ListaPlayers.FivemPlayerlist.Init();

			Veicoli.FuelClient.Init();
			Veicoli.VehicleDamage.Init();
			Veicoli.VeicoliClient.Init();
			Veicoli.Treni.Init();
			Veicoli.EffettiRuote.Init();
			Veicoli.VehHUD.Init();

			//Giostre.MontagneRusse.Init();
			//Giostre.RuotaPanoramica.Init();
			//Giostre.Funivia.Init();

			IPLInstance.Init();
			Proprietà.Hotel.Hotels.Init();

			Interactions.Macchinette.Init();
			Interactions.Docce.Init();
			Interactions.PickupsClient.Init();
			Interactions.OggettiGenerici.Init();

			DamageEvents.Init();

			Sport.Yoga.Init();

			//Telefono.PhoneMainClient.Init();

			ClasseDiTest.Init(); // da rimouvere
			CodaControl.CodaAdminPanel.Init();

			Lavori.Whitelistati.VenditoreAuto.CarDealer.Init();
			Lavori.Whitelistati.VenditoreCase.HouseDealer.Init();

			TickController.Init();
		}
	}
}
