using System;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using Impostazioni.Server.Configurazione.Coda;
using Impostazioni.Server.Configurazione.Main;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server
{
	static class ConfigServer
	{
		public static async Task Init()
		{
			string jsonServerConfig = API.LoadResourceFile(API.GetCurrentResourceName(), "configs/ServerConfig.json");
			Server.Impostazioni = jsonServerConfig.FromJson<Configurazione>();
			ConfigShared.SharedConfig = new SharedConfig();
			Server.Instance.Events.Mount("Config.CallClientConfig", new Func<ClientId, int, Task<string>>(ClientConfigCallback));

			await Task.FromResult(0);
		}

		private static async Task<string> ClientConfigCallback(ClientId client, int type)
		{
			switch (type)
			{
				case 0:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_Lobby.json");
				case 1:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_RolePlay.json");
				case 2:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_Minigiochi.json");
				case 3:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_Gare.json");
				case 4:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_Negozio.json");
				case 5:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_FreeRoam.json");
				default:
			return "";
			}
		}

	}

	public class Configurazione
	{
		public ConfPrincipale Main = new ConfPrincipale();
		public ConfigCoda Coda = new ConfigCoda();
	}
}