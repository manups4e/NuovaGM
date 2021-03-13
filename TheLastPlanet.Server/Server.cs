﻿using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Logger;
using TheLastPlanet.Shared;
using TheLastPlanet.Server.SistemaEventi;
using TheLastPlanet.Shared.Snowflake;

// ReSharper disable All

namespace TheLastPlanet.Server
{
	public class Server : BaseScript
	{
		public static ConcurrentDictionary<string, User> PlayerList = new ConcurrentDictionary<string, User>();
		public static Server Instance { get; protected set; }
		public ExportDictionary GetExports => Exports;
		public PlayerList GetPlayers => Players;
		public static Configurazione Impostazioni = null;
		public static Dictionary<string, Action<Player, Delegate, dynamic>> ServerCallbacks = new Dictionary<string, Action<Player, Delegate, dynamic>>();
		public EventSystem Eventi;

		public Server()
		{
			SnowflakeGenerator.Create(2);
			Instance = this;
			Eventi = new();
			ClassCollector.Init();
		}

		/// <summary>
		/// registra un evento (TriggerEvent)
		/// </summary>
		/// <param name="name">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void AddEventHandler(string eventName, Delegate action) => EventHandlers[eventName] += action;

		/// <summary>
		/// registra un evento (TriggerEvent)
		/// </summary>
		/// <param name="name">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void RemoveEventHandler(string eventName, Delegate action) => EventHandlers[eventName] -= action;

		/// <summary>
		/// Chiama il db ed esegue una Query con risultato dynamic
		/// </summary>
		/// <param name="query">Testo della query</param>
		/// <param name="parameters">Parametri da passare</param>
		/// <returns>dynamic List if more than one or a dynamic object if only one</returns>
		public async Task<dynamic> Query(string query, object parameters = null) => await MySQL.QueryListAsync(query, parameters);

		/// <summary>
		/// Esegue una query sul db modificandone il contenuto
		/// </summary>
		/// <param name="query">Testo della query</param>
		/// <param name="parameters">Parametri da passare</param>
		/// <returns></returns>
		public async Task Execute(string query, object parameters = null) => await MySQL.ExecuteAsync(query, parameters);

		/// <summary>
		/// Registra una funzione OnTick
		/// </summary>
		/// <param name="action"></param>
		public void AddTick(Func<Task> onTick) => Tick += onTick;

		/// <summary>
		/// Rimuove la funzione OnTick
		/// </summary>
		/// <param name="action"></param>
		public void RemoveTick(Func<Task> onTick) => Tick -= onTick;

		/// <summary>
		/// registra un export, Registered GetExports still have to be defined in the __resource.lua file
		/// </summary>
		/// <param name="name"></param>
		/// <param name="action"></param>
		public void RegisterExport(string name, Delegate action) => GetExports.Add(name, action);

		/// <summary>
		/// registra un comando di chat
		/// </summary>
		/// <param name="commandName">Nome comando</param>
		/// <param name="handler">Una nuova Action<int source, List<dynamic> args, string rawCommand</param>
		/// <param name="restricted">tutti o solo chi può?</param>
		//public void AddCommand(string commandName, InputArgument handler, bool restricted) => API.RegisterCommand(commandName, handler, restricted);
		public void AddCommand(string commandName, Delegate handler, UserGroup restricted = UserGroup.User, ChatSuggestion suggestion = null)
		{
			//API.RegisterCommand(commandName, handler, restricted);
			ChatServer.Commands.Add(new ChatCommand(commandName, restricted, handler));

			if (suggestion != null)
			{
				suggestion.name = "/" + commandName;
				ChatServer.Suggestions.Add(suggestion);
			}
		} 
	}
}