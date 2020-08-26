using CitizenFX.Core;
using FivemPlayerlistServer;
using NuovaGM.Server.banking;
using NuovaGM.Server.Businesses;
using NuovaGM.Server.Clothestores;
using NuovaGM.Server.Discord;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Server.Interactions;
using NuovaGM.Server.Lavori.Whitelistati;
using NuovaGM.Server.manager;
using NuovaGM.Server.Veicoli;
using System;

namespace NuovaGM.Server
{
	static class ClassCollector
	{
		public static async void Init()
		{
			RequestInternal.Init();
			await ConfigServer.Init();
			while (Server.Impostazioni == null) await BaseScript.Delay(0);
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
			GiostreServer.Init();
			//			Telefoni.TelefonoMainServer.Init();
			TimeWeather.Meteo.Init();
			TimeWeather.Orario.Init();
			PickupsServer.Init();
			NuovaCoda.Init();
			ServerManager.Init();
			BotDiscordHandler.Init();
		}
	}
}
