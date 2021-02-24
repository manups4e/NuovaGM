﻿using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheLastPlanet.Server
{
	static class ConfigServer
	{
		private static string ConfigClient;
		private static string ConfigShared;
		public static async Task Init()
		{
			Server.Instance.AddEventHandler("lprp:RiceviConfig", new Action<dynamic>(Configurazione));
			Server.Instance.AddEventHandler("lprp:riavvioApp", new Action<Player>(InviaAlClient));
			BaseScript.TriggerEvent("lprp:chiamaConfigServer");
			await BaseScript.Delay(1000);
			while (Server.Impostazioni == null)
			{
				BaseScript.TriggerEvent("lprp:chiamaConfigServer");
				await BaseScript.Delay(5000);
			}
		}

		private static void Configurazione(dynamic JsonConfig)
		{
			ConfigShared = JsonConvert.SerializeObject(JsonConfig.Shared);
			ConfigClient = JsonConvert.SerializeObject(JsonConfig.Client);
			string ConfigServer = JsonConvert.SerializeObject(JsonConfig.Server);
			Server.Impostazioni = ConfigServer.Deserialize<Configurazione>();
			Shared.ConfigShared.SharedConfig = ConfigShared.Deserialize<SharedConfig>();
		}

		private static void InviaAlClient([FromSource] Player p)
		{
			p.TriggerEvent("lprp:ConfigurazioneClient", ConfigClient, ConfigShared);
			BaseScript.TriggerEvent("lprp:caricaStazioniGasServer");
		}
	}

	public class Configurazione
	{
		public ConfPrincipale Main = new ConfPrincipale();
		public ConfigCoda Coda = new ConfigCoda();
	}

	public class ConfPrincipale
	{
		public string NomeServer;
		public string WebHookLog;
		public string WebHookAnticheat;
		public string notWhitelisted;
		public bool EnableAntiSpam;
		public int PlayersToStartRocade;
		public int PingMax;
		public int SalvataggioTutti;
		public int rainWait;
		public int rainWaitLow;
		public int rainWaitHigh;
		public int waitLow;
		public int waitHigh;
		public bool snowPersist;
		public int RentPricePompeDiBenzina;
		public int RentPriceNegozi;
		public string RuoloWhitelistato;
		public string DiscordToken;
		public string GuildId;
		public Dictionary<string, string> BadWords = new Dictionary<string, string>();
	}

	public class ConfigCoda
	{
		public ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
		public List<string> permessi = new List<string>();
		public bool whitelistonly;
		public int loadTime;
		public int graceTime;
		public int queueGraceTime;
		public bool contoCodaFineNomeServer;
		public bool allowSymbols;
		public bool stateChangeMessages;
	}
}
