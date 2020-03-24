using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;
using System.Drawing;
using Console = Colorful.Console;
using System.Globalization;

namespace NuovaGM.Server
{
	public enum LogType
	{
		Info,
		Debug,
		Warning,
		Error,
		Fatal
	}
	
	public class Server : BaseScript
	{
		public static Server Instance { get; protected set; }
		public ExportDictionary GetExports { get { return Exports; } }
		public PlayerList GetPlayers { get { return Players; } }

		public Server()
		{
			EventHandlers["onResourceStop"] += new Action<string>(OnResourceStop);
			Instance = this;
			ClassCollector.Init();
		}

		public static void Printa(LogType tipo, string text)
		{
			string err = "-- [INFO] -- ";
			string incipit = $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss", new CultureInfo("it-IT"))}";
			Color colore = Color.LimeGreen;
			switch (tipo)
			{
				case LogType.Info:
					err = "-- [INFO] -- ";
					colore = Color.LimeGreen;
					break;
				case LogType.Debug:
					err = "-- [DEBUG] -- ";
					colore = Color.Cyan;
					break;
				case LogType.Warning:
					err = "-- [ATTENZIONE] --";
					colore = Color.DarkOrange;
					break;
				case LogType.Error:
					err = "-- [ERRORE] --";
					colore = Color.OrangeRed;
					break;
				case LogType.Fatal:
					err = "-- [FATALE] --";
					colore = Color.Yellow;
					break;
			}
			Console.WriteLine($"{incipit} {err} {text}", colore);
		}

		/// <summary>
		/// registra un evento client (TriggerEvent)
		/// </summary>
		/// <param name="name">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void RegisterEventHandler(string eventName, Delegate action) => EventHandlers[eventName] += action;

		/// <summary>
		/// Chiama il db ed esegue una Query con risultato dynamic
		/// </summary>
		/// <param name="query">Testo della query</param>
		/// <param name="parameters">Parametri da passare</param>
		/// <returns>dynamic List if more than one or a dynamic object if only one</returns>
		public async Task<dynamic> Query(string query, object parameters = null)
		{
			return await MySQL.QueryAsync(query, parameters);
		}

		/// <summary>
		/// Esegue una query sul db modificandone il contenuto
		/// </summary>
		/// <param name="query">Testo della query</param>
		/// <param name="parameters">Parametri da passare</param>
		/// <returns></returns>
		public async Task Execute(string query, object parameters = null) => await MySQL.ExecuteAsync(query, parameters);

		/// <summary>
		/// Rimuove un evento client (TriggerEvent)
		/// </summary>
		/// <param name="name">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void DeregisterEventHandler(string eventName, Delegate action) => EventHandlers[eventName] -= action;

		/// <summary>
		/// Registra una funzione OnTick
		/// </summary>
		/// <param name="action"></param>
		public void RegisterTickHandler(Func<Task> onTick) => Tick += onTick;

		/// <summary>
		/// Rimuove la funzione OnTick
		/// </summary>
		/// <param name="action"></param>
		public void DeregisterTickHandler(Func<Task> onTick) => Tick -= onTick;


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
