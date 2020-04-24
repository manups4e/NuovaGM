using CitizenFX.Core;
using NuovaGM.Client.IPLs;
using System.Threading.Tasks;

namespace NuovaGM.Client
{
	static class ClassCollector
	{
		public static async void Init()
		{
			await DecorationClass.DichiaraDecor();
			await ConfigClient.Init();
			gmPrincipale.Main.Init();

			Banking.BankingClient.Init();

			Businesses.PompeDiBenzinaClient.Init();

			gmPrincipale.Utility.Eventi.Init();
			gmPrincipale.Utility.HUD.HUD.Init();
			gmPrincipale.Utility.PublicTraffic.Init();
			gmPrincipale.MenuGm.Menus.Init();
			gmPrincipale.MenuGm.CamerasFirstTime.Init();
			gmPrincipale.Status.Death.Init();
			gmPrincipale.Status.StatsNeeds.Init();
			gmPrincipale.Voice.Init();
			gmPrincipale.Discord.Init();
			gmPrincipale.NuovoIngresso.NuovoIngresso.Init();




			TimeWeather.Meteo.Init();
			TimeWeather.Orario.Init();

			Lavori.Whitelistati.Polizia.PoliziaMainClient.Init();
			Lavori.Whitelistati.Medici.MediciMainClient.Init();
			Lavori.Generici.Pescatore.PescatoreClient.Init();
			Lavori.Generici.Cacciatore.CacciatoreClient.Init();
			Lavori.Generici.Rimozione.RimozioneClient.Init();
//			Lavori.JobManager.Init();

			Manager.ClientManager.Init();
			Manager.DevManager.Init();

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

			Giostre.MontagneRusse.Init();
			Giostre.RuotaPanoramica.Init();
//			Giostre.Funivia.Init();

			IPLInstance.Init();
			Proprietà.Hotel.Hotels.Init();

			Interactions.Macchinette.Init();
			//Interactions.Spazzatura.Init();
			Interactions.Docce.Init();
			Interactions.Letti.Init();
			Interactions.PickupsClient.Init();
			//Interactions.DivaniEPosizioniSedute.Init();
			Interactions.OggettiGenerici.Init();

			DamageEvents.Init();

			Telefono.PhoneMainClient.Init();

			ClasseDiTest.Init(); // da rimouvere
			CodaControl.CodaAdminPanel.Init();
			TickController.Init();
		}
	}
}
