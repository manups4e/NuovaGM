using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;
using System.Drawing;
using Console = Colorful.Console;
using System.Globalization;
using NuovaGM.Server.gmPrincipale;
using System.Collections.Generic;
using System.Collections.Concurrent;
// ReSharper disable All

namespace NuovaGM.Server
{
	public class Server : BaseScript
	{
		public static ConcurrentDictionary<string, User> PlayerList = new ConcurrentDictionary<string, User>();
		public static Server Instance { get; protected set; }
		public ExportDictionary GetExports { get { return Exports; } }
		public PlayerList GetPlayers { get { return Players; } }
		public static Configurazione Impostazioni = null;

		public Server()
		{
			EventHandlers["onResourceStop"] += new Action<string>(OnResourceStop);
			Instance = this;
			ClassCollector.Init();
		}

		/// <summary>
		/// registra un evento client (TriggerEvent)
		/// </summary>
		/// <param name="name">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void AddEventHandler(string eventName, Delegate action) => EventHandlers[eventName] += action;

		/// <summary>
		/// Chiama il db ed esegue una Query con risultato dynamic
		/// </summary>
		/// <param name="query">Testo della query</param>
		/// <param name="parameters">Parametri da passare</param>
		/// <returns>dynamic List if more than one or a dynamic object if only one</returns>
		public async Task<dynamic> Query(string query, object parameters = null) => await MySQL.QueryAsync(query, parameters);

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
		/// registra un export, Registered exports still have to be defined in the __resource.lua file
		/// </summary>
		/// <param name="name"></param>
		/// <param name="action"></param>
		public void RegisterExport(string name, Delegate action) => Exports.Add(name, action);

		private void OnResourceStop(string resourceName)
		{
			if (API.GetCurrentResourceName() != resourceName) return;
			API.ExecuteCommand("restart config");
		}

		/// <summary>
		/// registra un comando di chat
		/// </summary>
		/// <param name="commandName">Nome comando</param>
		/// <param name="handler">Una nuova Action<int source, List<dynamic> args, string rawCommand</param>
		/// <param name="restricted">tutti o solo chi può?</param>
		public void AddCommand(string commandName, InputArgument handler, bool restricted) => API.RegisterCommand(commandName, handler, restricted);

	}
}
