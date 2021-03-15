using System.Threading.Tasks;
using Impostazioni.Server.Configurazione.Coda;
using Impostazioni.Server.Configurazione.Main;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Server
{
	static class ConfigServer
	{
		public static async Task Init()
		{
			ServerSession.Impostazioni = new Configurazione();
			ConfigShared.SharedConfig = new SharedConfig();
		}
	}

	public class Configurazione
	{
		public ConfPrincipale Main = new ConfPrincipale();
		public ConfigCoda Coda = new ConfigCoda();
	}
}