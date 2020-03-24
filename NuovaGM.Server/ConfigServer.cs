using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Shared;
using NuovaGM.Shared.Weather;
using System;

namespace NuovaGM.Server
{
	static class ConfigServer
	{
		private static string ConfigClient;
		private static string ConfigShared;
		public static Configurazione Conf = null;
		public static void Init()
		{
			Server.Instance.RegisterEventHandler("lprp:RiceviConfig", new Action<dynamic>(Configurazione));
			Server.Instance.RegisterEventHandler("lprp:riavvioApp", new Action<Player>(InviaAlClient));
			BaseScript.TriggerEvent("lprp:chiamaConfigServer");
		}

		private static void Configurazione(dynamic JsonConfig)
		{
			ConfigShared = JsonConvert.SerializeObject(JsonConfig.Shared);
			ConfigClient = JsonConvert.SerializeObject(JsonConfig.Client);
			string ConfigServer = JsonConvert.SerializeObject(JsonConfig.Server);
			Conf = JsonConvert.DeserializeObject<Configurazione>(ConfigServer);
			Shared.ConfigShared.SharedConfig = JsonConvert.DeserializeObject<SharedConfig>(ConfigShared);
		}

		private static async void InviaAlClient([FromSource] Player p)
		{
			p.TriggerEvent("lprp:ConfigurazioneClient", ConfigClient, ConfigShared);
			BaseScript.TriggerEvent("lprp:caricaStazioniGasServer");
		}
	}

	public class Configurazione
	{
		public ConfPrincipale Main = new ConfPrincipale();
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
	}
}
