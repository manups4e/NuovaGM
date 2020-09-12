using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using Newtonsoft.Json;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuovaGM.Client
{
	public class Client : BaseScript
	{
		public static Client Instance { get; protected set; }
		public ExportDictionary GetExports { get { return Exports; } }
		public PlayerList GetPlayers { get { return Players; } }
		public static Configurazione Impostazioni = null;
		private static Dictionary<int, Delegate> ServerCallbacks = new Dictionary<int, Delegate>();
		private static int CurrentRequestId = 0;
		public Client()
		{
			Instance = this;
			ClassCollector.Init();
		}

		#region ServerCallbacks

		/// <summary>
		/// Triggera un callback che va dichiarato anche lato server
		/// </summary>
		/// <typeparam name="T1">Tipo di parametro che ritornerà dal server</typeparam>
		/// <param name="eventName">Nome del callback</param>
		/// <param name="callBack">Un nuovo Action<>() da eseguire</param>
		public void TriggerServerCallback<T1>(string eventName, Action<T1> callBack) => new NetworkMethod<T1>(eventName, callBack).InvokeNoArgs();
		/// <summary>
		/// Triggera un callback che va dichiarato anche lato server
		/// </summary>
		/// <typeparam name="T1">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T2">Tipo di parametro che ritornerà dal server</typeparam>
		/// <param name="eventName">Nome del callback</param>
		/// <param name="callBack">Un nuovo Action<>() da eseguire</param>
		public void TriggerServerCallback<T1, T2>(string eventName, Action<T1, T2> callBack) => new NetworkMethod<T1, T2>(eventName, callBack).InvokeNoArgs();
		/// <summary>
		/// Triggera un callback che va dichiarato anche lato server
		/// </summary>
		/// <typeparam name="T1">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T2">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T3">Tipo di parametro che ritornerà dal server</typeparam>
		/// <param name="eventName">Nome del callback</param>
		/// <param name="callBack">Un nuovo Action<>() da eseguire</param>
		public void TriggerServerCallback<T1, T2, T3>(string eventName, Action<T1, T2, T3> callBack) => new NetworkMethod<T1, T2, T3>(eventName, callBack).InvokeNoArgs();
		/// <summary>
		/// Triggera un callback che va dichiarato anche lato server
		/// </summary>
		/// <typeparam name="T1">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T2">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T3">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T4">Tipo di parametro che ritornerà dal server</typeparam>
		/// <param name="eventName">Nome del callback</param>
		/// <param name="callBack">Un nuovo Action<>() da eseguire</param>
		public void TriggerServerCallback<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callBack) => new NetworkMethod<T1, T2, T3, T4>(eventName, callBack).InvokeNoArgs();

		/// <summary>
		/// Triggera un callback che va dichiarato anche lato server
		/// </summary>
		/// <typeparam name="T1">Tipo di parametro che ritornerà dal server</typeparam>
		/// <param name="eventName">Nome del callback</param>
		/// <param name="callBack">Un nuovo Action<>() da eseguire</param>
		public void TriggerServerCallback<T1>(string eventName, Action<T1> callBack, T1 val1) => new NetworkMethod<T1>(eventName, callBack).Invoke(val1);
		/// <summary>
		/// Triggera un callback che va dichiarato anche lato server
		/// </summary>
		/// <typeparam name="T1">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T2">Tipo di parametro che ritornerà dal server</typeparam>
		/// <param name="eventName">Nome del callback</param>
		/// <param name="callBack">Un nuovo Action<>() da eseguire</param>
		public void TriggerServerCallback<T1, T2>(string eventName, Action<T1, T2> callBack, T1 val1, T2 val2) => new NetworkMethod<T1, T2>(eventName, callBack).Invoke(val1, val2);
		/// <summary>
		/// Triggera un callback che va dichiarato anche lato server
		/// </summary>
		/// <typeparam name="T1">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T2">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T3">Tipo di parametro che ritornerà dal server</typeparam>
		/// <param name="eventName">Nome del callback</param>
		/// <param name="callBack">Un nuovo Action<>() da eseguire</param>
		public void TriggerServerCallback<T1, T2, T3>(string eventName, Action<T1, T2, T3> callBack, T1 val1, T2 val2, T3 val3) => new NetworkMethod<T1, T2, T3>(eventName, callBack).Invoke(val1, val2, val3);
		/// <summary>
		/// Triggera un callback che va dichiarato anche lato server
		/// </summary>
		/// <typeparam name="T1">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T2">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T3">Tipo di parametro che ritornerà dal server</typeparam>
		/// <typeparam name="T4">Tipo di parametro che ritornerà dal server</typeparam>
		/// <param name="eventName">Nome del callback</param>
		/// <param name="callBack">Un nuovo Action<>() da eseguire</param>
		public void TriggerServerCallback<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callBack, T1 val1, T2 val2, T3 val3, T4 val4) => new NetworkMethod<T1, T2, T3, T4>(eventName, callBack).Invoke(val1, val2, val3, val4);
		/// <summary>
		/// Triggera un callback che va dichiarato anche lato server
		/// </summary>
		/// <typeparam name="T1">Tipo di parametro che ritornerà dal server</typeparam>
		/// <param name="eventName">Nome del callback</param>
		/// <param name="callBack">Un nuovo Action<>() da eseguire</param>

		#endregion

		/// <summary>
		/// registra un evento client (TriggerEvent)
		/// </summary>
		/// <param name="eventName">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void AddEventHandler(string eventName, Delegate action) => EventHandlers[eventName] += action;

		/// <summary>
		/// Rimuove un evento client (TriggerEvent)
		/// </summary>
		/// <param name="eventName">Nome evento</param>
		/// <param name="action">Azione legata all'evento</param>
		public void DeAddEventHandler(string eventName, Delegate action) => EventHandlers[eventName] -= action;

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
		public void AddTick(Func<Task> onTick) => Tick += onTick;

		/// <summary>
		/// Rimuove la funzione OnTick
		/// </summary>
		/// <param name="onTick"></param>
		public void RemoveTick(Func<Task> onTick) => Tick -= onTick;


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
		/// <param name="handler">Una nuova Action<int source, List<dynamic> args, string rawCommand></param>
		/// <param name="restricted">tutti o solo chi può?</param>
		public void AddCommand(string commandName, InputArgument handler) => API.RegisterCommand(commandName, handler, false);
	}
}
