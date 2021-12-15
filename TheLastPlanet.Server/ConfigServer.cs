using System;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using Impostazioni.Server.Configurazione.Coda;
using Impostazioni.Server.Configurazione.Main;
using TheLastPlanet.Server.Properties;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server
{
	static class ConfigServer
	{
		public static async Task Init()
		{
			string jsonServerConfig = Resources.ServerConfig;
			Server.Impostazioni = jsonServerConfig.FromJson<Configurazione>();
			ConfigShared.SharedConfig = new SharedConfig();
			Server.Instance.Events.Mount("Config.CallClientConfig", new Func<ClientId, ModalitaServer, Task<string>>(ClientConfigCallback));

			await Task.FromResult(0);
		}

		private static async Task<string> ClientConfigCallback(ClientId client, ModalitaServer type)
		{
			switch (type)
			{
				case ModalitaServer.Lobby:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_Lobby.json");
				case ModalitaServer.Roleplay:
					return Resources.Client_RolePlay;
					//return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_RolePlay.json");
				case ModalitaServer.Minigiochi:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_Minigiochi.json");
				case ModalitaServer.Gare:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_Gare.json");
				case ModalitaServer.Negozio:
					return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_Negozio.json");
				case ModalitaServer.FreeRoam:
					return Resources.Client_FreeRoam;
					//return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_FreeRoam.json");
				default:
			return string.Empty;
			}
		}

	}

	public class Configurazione
	{
		public ConfPrincipale Main = new ConfPrincipale();
		public ConfigCoda Coda = new ConfigCoda();
	}
}