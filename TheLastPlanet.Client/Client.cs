﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.SistemaEventi;
using TheLastPlanet.Shared.Snowflake;

namespace TheLastPlanet.Client
{
	public class Client : BaseScript
	{
		public static Client Instance { get; protected set; }
		public ExportDictionary GetExports => Exports;
		public PlayerList GetPlayers => Players;
		public static Configurazione Impostazioni = null;
		public EventSystem Eventi;
		public Client() { Inizializza(); }

		private async void Inizializza()
		{
			SnowflakeGenerator.Create(1);
			Instance = this;
			Eventi = new EventSystem();
			await ClassCollector.Init();
		}
		
		/// <summary>
		/// registra un evento client (TriggerEvent)
		/// </summary>
		/// <param name="eventName">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void AddEventHandler(string eventName, Delegate action) { EventHandlers[eventName] += action; }

		/// <summary>
		/// Rimuove un evento client (TriggerEvent)
		/// </summary>
		/// <param name="eventName">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void DeAddEventHandler(string eventName, Delegate action) { EventHandlers[eventName] -= action; }

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
				Log.Printa(LogType.Error, ex.ToString());
			}
		}

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