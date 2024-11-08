﻿global using CitizenFX.Core;
global using CitizenFX.Core.Native;
global using FxEvents;
global using FxEvents.Shared;
global using TheLastPlanet.Shared;
global using static CitizenFX.Core.Native.API;
using FxEvents.Shared.Snowflakes;
using Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;

namespace TheLastPlanet.Server
{
    public class Server : BaseScript
    {
        public static Log Logger { get; set; }
        public static Server Instance { get; protected set; }
        public ExportDictionary GetExports => Exports;
        public PlayerList GetPlayers => Players;
        public List<PlayerClient> Clients = new();
        public static Configurazione Impostazioni { get; set; }
        public static bool Debug { get; set; }
        public Request WebRequest { get; set; }
        public StateBag ServerState => GlobalState;
        public StateBagsHandler StateBagsHandler { get; set; }

        public Server()
        {
            EventDispatcher.Initalize("qIFBYn6qv7ZxbGLT7uzpFHa1wPCpmIHbDTWGJ8fy", "QNrAF12UC1qOvnhL6JEShdEdNiCyASUbbNpvyZPG", "Pi5V5nvCki0BcwppyczIfgy3ZZCJPqaYAeQsLZOs");
            Instance = this;
            Logger = new Log();
#if DEBUG
            SetConvarReplicated("DEBUG", "1");
            Debug = true;
#else
			SetConvarReplicated("DEBUG", "0");
#endif
            SnowflakeGenerator.Create(new Random().NextShort(100, 200));
            SetConvarServerInfo("sv_projectName", "^2THE ^0LAST ^1GALAXY.");
            SetConvarServerInfo("sv_projectDesc", "^5A server to tame them, a server to find them, a server to seize them and in the videogame chain them!");
            SetConvarServerInfo("tags", "RolePlay, GTAO style, MultiMode");
            SetGameType("Multi-mode");
            SetMapName("The Last Galaxy");
            StartServer();
        }

        private async void StartServer()
        {
            StateBagsHandler = new StateBagsHandler();
            WebRequest = new();
            await ClassCollector.Init();
        }

        /// <summary>
        /// registra un evento (TriggerEvent)
        /// </summary>
        /// <param name="name">Nome evento</param>
        /// <param name="action">Azione legata all'evento</param>
        public void AddEventHandler(string eventName, Delegate action)
        {
            EventHandlers[eventName] += action;
        }

        /// <summary>
        /// registra un evento (TriggerEvent)
        /// </summary>
        /// <param name="name">Nome evento</param>
        /// <param name="action">Azione legata all'evento</param>
        public void RemoveEventHandler(string eventName, Delegate action)
        {
            EventHandlers[eventName] -= action;
        }

        /// <summary>
        /// Chiama il db ed esegue una Query con risultato dynamic
        /// </summary>
        /// <param name="query">Testo della query</param>
        /// <param name="parameters">Parametri da passare</param>
        /// <returns>dynamic List if more than one or a dynamic object if only one</returns>
        public async Task<dynamic> Query(string query, object parameters = null)
        {
            return await MySQL.QueryListAsync(query, parameters);
        }

        /// <summary>
        /// Esegue una query sul db modificandone il contenuto
        /// </summary>
        /// <param name="query">Testo della query</param>
        /// <param name="parameters">Parametri da passare</param>
        /// <returns></returns>
        public async Task Execute(string query, object parameters = null)
        {
            await MySQL.ExecuteAsync(query, parameters);
        }

        /// <summary>
        /// Registra una funzione OnTick
        /// </summary>
        /// <param name="action"></param>
        public void AddTick(Func<Task> onTick)
        {
            Tick += onTick;
        }

        /// <summary>
        /// Rimuove la funzione OnTick
        /// </summary>
        /// <param name="action"></param>
        public void RemoveTick(Func<Task> onTick)
        {
            Tick -= onTick;
        }

        /// <summary>
        /// registra un export, Registered GetExports still have to be defined in the __resource.lua file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void RegisterExport(string name, Delegate action)
        {
            GetExports.Add(name, action);
        }

        /// <summary>
        /// registra un comando di chat
        /// </summary>
        /// <param name="commandName">Nome comando</param>
        /// <param name="handler">Una nuova Action<int source, List<dynamic> args, string rawCommand</param>
        /// <param name="restricted">tutti o solo chi può?</param>
        //public void AddCommand(string commandName, InputArgument handler, bool restricted) => API.RegisterCommand(commandName, handler, restricted);
        public void AddCommand(string commandName, Delegate handler, ServerMode modalita, UserGroup restricted = UserGroup.User, ChatSuggestion suggestion = null)
        {
            //API.RegisterCommand(commandName, handler, restricted);
            ChatServer.Commands.Add(new ChatCommand(commandName, restricted, modalita, handler));

            if (suggestion != null)
            {
                suggestion.name = "/" + commandName;
                ChatServer.Suggestions.Add(suggestion);
            }
        }
    }
}