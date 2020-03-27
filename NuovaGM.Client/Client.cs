using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;
// ReSharper disable All

namespace NuovaGM.Client
{
	public enum LogType
	{
		Info,
		Debug,
		Warning,
		Error,
		Fatal
	}

	public class Client : BaseScript
	{
		public static Client GetInstance { get; protected set; }
		public ExportDictionary GetExports { get { return Exports; } }
		public PlayerList GetPlayers { get { return Players; } }

		public Client()
		{
			GetInstance = this;
			ClassCollector.Init();
		}

		/// <summary>
		/// registra un evento client (TriggerEvent)
		/// </summary>
		/// <param name="eventName">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void RegisterEventHandler(string eventName, Delegate action) => EventHandlers[eventName] += action;

		/// <summary>
		/// Manda in console un messaggio colorato in base alla gravità della situazione
		/// </summary>
		/// <param name="tipo">La gravità della situazione</param>
		/// <param name="text">Testo del messaggio</param>
		public static void Printa(LogType tipo, string text)
		{
			string err = "-- [INFO] -- ";
			string incipit = $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss")}";
			string colore = "^2";
			switch (tipo)
			{
				case LogType.Info:
					err = "-- [INFO] -- ";
					colore = "^2";
					break;
				case LogType.Debug:
					err = "-- [DEBUG] -- ";
					colore = "^5";
					break;
				case LogType.Warning:
					err = "-- [ATTENZIONE] --";
					colore = "^3";
					break;
				case LogType.Error:
					err = "-- [ERRORE] --";
					colore = "^1";
					break;
				case LogType.Fatal:
					err = "-- [FATALE] --";
					colore = "^9";
					break;
			}
			Debug.WriteLine($"^4{incipit} {colore}{err} {text}.^7");
		}


		/// <summary>
		/// Rimuove un evento client (TriggerEvent)
		/// </summary>
		/// <param name="eventName">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void DeregisterEventHandler(string eventName, Delegate action) => EventHandlers[eventName] -= action;

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
				RegisterEventHandler(string.Concat("__cfx_nui:", name), action);
			}
			catch (Exception ex)
			{
				Printa(LogType.Error, ex.ToString());
			}
		}

		/// <summary>
		/// Registra una funzione OnTick
		/// </summary>
		/// <param name="onTick"></param>
		public void RegisterTickHandler(Func<Task> onTick) => Tick += onTick;

		/// <summary>
		/// Rimuove la funzione OnTick
		/// </summary>
		/// <param name="onTick"></param>
		public void DeregisterTickHandler(Func<Task> onTick) => Tick -= onTick;


		/// <summary>
		/// registra un export, Registered exports still have to be defined in the fxmanifest.lua file
		/// </summary>
		/// <param name="name"></param>
		/// <param name="action"></param>
		public void RegisterExport(string name, Delegate action) => Exports.Add(name, action);

		/// <summary>
		/// registra un comando di chat
		/// </summary>
		/// <param name="commandName">Nome comando</param>
		/// <param name="handler">Una nuova Action<int source, List<dynamic> args, string rawCommand</param>
		/// <param name="restricted">tutti o solo chi può?</param>
		public void AddCommand(string commandName, InputArgument handler, bool restricted) => API.RegisterCommand(commandName, handler, restricted);
	}
}
