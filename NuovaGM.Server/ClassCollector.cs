using CitizenFX.Core;
using FivemPlayerlistServer;
using NuovaGM.Server.banking;
using NuovaGM.Server.Businesses;
using NuovaGM.Server.Clothestores;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Server.Lavori.Whitelistati;
using NuovaGM.Server.manager;
using NuovaGM.Server.Meteo_New;
using NuovaGM.Server.Veicoli;

namespace NuovaGM.Server
{
	static class ClassCollector
	{
		public static async void Init()
		{
			ConfigServer.Init();
			while (ConfigServer.Conf == null) await BaseScript.Delay(0);
			DiscordWhitelist.Init();
			ServerManager.Init();
			ServerEntrance.Init();
			Eventi.Init();
			Funzioni.Init();
			ChatMain.Init();
			ChatEvents.Init();
			BankingServer.Init();
			PompeDiBenzinaServer.Init();
			NegozioAbitiServer.Init();
			PoliziaServer.Init();
			MediciServer.Init();
			FPLServer.Init();
			FuelServer.Init();
			VeicoliServer.Init();
			Meteo.Init();
			Orario.Init();
			Coda.Init();
		}
	}
}
