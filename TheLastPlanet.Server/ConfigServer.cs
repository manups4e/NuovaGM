﻿using Impostazioni.Server.Configurazione.Coda;
using Impostazioni.Server.Configurazione.Main;
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
            EventDispatcher.Mount("Config.CallClientConfig", new Func<PlayerClient, ModalitaServer, Task<string>>(ClientConfigCallback));
            EventDispatcher.Mount("Config.CallSharedConfig", new Func<PlayerClient, ModalitaServer, Task<string>>(ClientSharedCallback));
            EventDispatcher.Mount("tlg:getWeaponsConfig", new Func<Task<List<SharedWeapon>>>(GiveWeaponsToClient));

            await Task.FromResult(0);
        }

        private static async Task<List<SharedWeapon>> GiveWeaponsToClient()
        {
            return Weapons;
        }

        private static async Task<string> ClientSharedCallback([FromSource] PlayerClient client, ModalitaServer type)
        {
            if (type == ModalitaServer.Roleplay)
            {
                return Resources.SharedConfig;
            }
            return null;
        }
        private static async Task<string> ClientConfigCallback([FromSource] PlayerClient client, ModalitaServer type)
        {
            switch (type)
            {
                case ModalitaServer.Lobby:
                    return LoadResourceFile(GetCurrentResourceName(), "configs/Client_Lobby.json");
                case ModalitaServer.Roleplay:
                    return Resources.Client_RolePlay;
                //return LoadResourceFile(GetCurrentResourceName(), "configs/Client_RolePlay.json");
                case ModalitaServer.Minigiochi:
                    return LoadResourceFile(GetCurrentResourceName(), "configs/Client_Minigiochi.json");
                /*
            case ModalitaServer.Gare:
                return LoadResourceFile(GetCurrentResourceName(), "configs/Client_Gare.json");
                */
                case ModalitaServer.Negozio:
                    return LoadResourceFile(GetCurrentResourceName(), "configs/Client_Negozio.json");
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
        public ConfPrincipale Main = new();
        public ConfigCoda Coda = new();
    }
}