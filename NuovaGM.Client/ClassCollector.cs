using CitizenFX.Core;
using NuovaGM.Client.Interactions;
using NuovaGM.Client.IPLs;
using NuovaGM.Client.Meteo_new;

namespace NuovaGM.Client
{
	static class ClassCollector
	{
		public static async void Init()
		{
			ConfigClient.Init();
			while (ConfigClient.Conf == null) await BaseScript.Delay(0);
			Meteo.Init();
			Orario.Init();
			gmPrincipale.Main.Init();

			Lavori.Whitelistati.Polizia.PoliziaMainClient.Init();
			Lavori.JobManager.Init();

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
			Giostre.Funivia.Init();

			Proprietà.Hotel.Hotels.Init();

			Macchinette.Init();
			Spazzatura.Init();

			TickController.Init();
			DamageEvents.Init();

			IPLInstance.Init();

			Docce.Init();
			Letti.Init();

			DivaniEPosizioniSedute.Init();
		}
	}
}
