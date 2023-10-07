using Settings.Server.Configurazione.Coda;
using Settings.Server.Configurazione.Main;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Server.Properties;


namespace TheLastPlanet.Server
{
    static class ConfigServer
    {
        public static List<SharedWeapon> Weapons = new List<SharedWeapon>();
        public static async Task Init()
        {
            Server.Impostazioni = Resources.ServerConfig.FromJson<Configurazione>();
            ConfigShared.SharedConfig = Resources.SharedConfig.FromJson<SharedConfig>();
            Weapons = Resources.Weapons.FromJson<List<SharedWeapon>>();
            EventDispatcher.Mount("Config.CallClientConfig", new Func<PlayerClient, ServerMode, Task<string>>(ClientConfigCallback));
            EventDispatcher.Mount("Config.CallSharedConfig", new Func<PlayerClient, ServerMode, Task<string>>(ClientSharedCallback));
            EventDispatcher.Mount("tlg:getWeaponsConfig", new Func<Task<List<SharedWeapon>>>(GiveWeaponsToClient));

            await Task.FromResult(0);
        }

        private static async Task<List<SharedWeapon>> GiveWeaponsToClient()
        {
            return Weapons;
        }

        private static async Task<string> ClientSharedCallback([FromSource] PlayerClient client, ServerMode type)
        {
            if (type == ServerMode.Roleplay)
            {
                return Resources.SharedConfig;
            }
            return null;
        }
        private static async Task<string> ClientConfigCallback([FromSource] PlayerClient client, ServerMode type)
        {
            switch (type)
            {
                case ServerMode.Lobby:
                    return LoadResourceFile(GetCurrentResourceName(), "configs/Client_Lobby.json");
                case ServerMode.Roleplay:
                    return Resources.Client_RolePlay;
                //return LoadResourceFile(GetCurrentResourceName(), "configs/Client_RolePlay.json");
                case ServerMode.Minigames:
                    return LoadResourceFile(GetCurrentResourceName(), "configs/Client_Minigiochi.json");
                /*
            case ModalitaServer.Gare:
                return LoadResourceFile(GetCurrentResourceName(), "configs/Client_Gare.json");
                */
                case ServerMode.Store:
                    return LoadResourceFile(GetCurrentResourceName(), "configs/Client_Negozio.json");
                case ServerMode.FreeRoam:
                    return Resources.Client_FreeRoam;
                //return API.LoadResourceFile(API.GetCurrentResourceName(), "configs/Client_FreeRoam.json");
                default:
                    return string.Empty;
            }
        }

    }

    public class Configurazione
    {
        public ConfPrincipale Main = new();
        public ConfigQueue Coda = new();
    }
}