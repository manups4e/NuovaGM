﻿global using CitizenFX.Core;
global using CitizenFX.Core.Native;
global using CitizenFX.Core.UI;
global using ScaleformUI;
global using static CitizenFX.Core.Native.API;
global using TheLastPlanet.Shared;
global using TheLastPlanet.Client.Cache;
global using TheLastPlanet.Client.Core.PlayerChar;
global using TheLastPlanet.Client.Core.Utility;
global using TheLastPlanet.Client.Core.Utility.HUD;
using Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.AdminAC;
using TheLastPlanet.Client.Core.Ingresso;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.Internal.Events;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Client
{
    public class Client : BaseScript
    {
        public static Log Logger;
        public static Client Instance { get; protected set; }
        public ExportDictionary GetExports => Exports;
        public PlayerList GetPlayers => Players;
        public static Configurazione Impostazioni = new Configurazione();
        public ClientGateway Events { get; set; }
        public List<ClientId> Clients = new();
        public NuiManager NuiManager = new();
        public StateBagsHandler StateBagsHandler { get; set; }
        public StateBag ServerState => GlobalState;
        public Client() { Inizializza(); }

        private async void Inizializza()
        {
            Logger = new();
            SnowflakeGenerator.Create(1);
            Instance = this;
            Events = new();
            HUD.Init();
            ClasseDiTest.Init(); // da rimuovere
            ClientManager.Init();
            DevManager.Init();
            InputHandler.Init();
            Minimap.Init();
            ListaPlayers.FivemPlayerlist.Init();
            InternalGameEvents.Init();
            ServerJoining.Init();
            StateBagsHandler = new StateBagsHandler();
            GestionePlayersDecors.Init();
        }

        /// <summary>
        /// registra un evento client (TriggerEvent)
        /// </summary>
        /// <param name="eventName">Nome evento</param>
        /// <param name="action">Azione legata all'evento</param>
        public void AddEventHandler(string eventName, Delegate action)
        {
            EventHandlers[eventName] += action;
        }

        /// <summary>
        /// Rimuove un evento client (TriggerEvent)
        /// </summary>
        /// <param name="eventName">Nome evento</param>
        /// <param name="action">Azione legata all'evento</param>
        public void RemoveEventHandler(string eventName, Delegate action)
        {
            EventHandlers[eventName] -= action;
        }

        /*
		/// <summary>
		/// Registra un evento NUI/CEF 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="action"></param>
		public void RegisterNuiEventHandler(string name, Delegate action)
		{
			try
			{
				API.RegisterNuiCallbackType(name);
				AddEventHandler(string.Concat("__cfx_nui:", name), action);
			}
			catch (Exception ex)
			{
				Logger.Error(ex.ToString());
			}
		}
		*/
        /// <summary>
        /// Registra una funzione OnTick
        /// </summary>
        /// <param name="onTick"></param>
        public void AddTick(Func<Task> onTick) { Tick += onTick; }

        /// <summary>
        /// Rimuove la funzione OnTick
        /// </summary>
        /// <param name="onTick"></param>
        public void RemoveTick(Func<Task> onTick) { Tick -= onTick; }

        /// <summary>
        /// registra un export, Registered Exports still have to be defined in the fxmanifest.lua file
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
        /// <param name="handler">Una nuova Action<int source, List<dynamic> args, string rawCommand></param>
        /// <param name="restricted">tutti o solo chi può?</param>
        public void AddCommand(string commandName, InputArgument handler) { API.RegisterCommand(commandName, handler, false); }
    }
}