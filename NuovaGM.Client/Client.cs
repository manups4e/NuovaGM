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

		protected static string SerializeObject(object o)
		{
			if (o == null)
				return null;

			return JsonConvert.SerializeObject(o);
		}
		protected static T DeserializeObject<T>(string text)
		{
			if (text == null)
				return default(T);

			return JsonConvert.DeserializeObject<T>(text);
		}

		public void TriggerServerCallback<T1>(string eventName, Action<T1> callBack)
		{
			NetworkMethod<T1> net = new NetworkMethod<T1>(eventName, callBack);
			net.InvokeNoArgs();
		}
		public void TriggerServerCallback<T1, T2>(string eventName, Action<T1, T2> callBack)
		{
			NetworkMethod<T1, T2> net = new NetworkMethod<T1, T2>(eventName, callBack);
			net.InvokeNoArgs();
		}
		public void TriggerServerCallback<T1, T2, T3>(string eventName, Action<T1, T2, T3> callBack)
		{
			NetworkMethod<T1, T2, T3> net = new NetworkMethod<T1, T2, T3>(eventName, callBack);
			net.InvokeNoArgs();
		}
		public void TriggerServerCallback<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callBack)
		{
			NetworkMethod<T1, T2, T3, T4> net = new NetworkMethod<T1, T2, T3, T4>(eventName, callBack);
			net.InvokeNoArgs();

		}
		public void TriggerServerCallback<T1, T2>(string eventName, Action<T1, T2> callBack, T1 val1, T2 val2)
		{
			NetworkMethod<T1, T2> net = new NetworkMethod<T1, T2>(eventName, callBack);
			net.Invoke(val1, val2);

		}
		public void TriggerServerCallback<T1, T2, T3>(string eventName, Action<T1, T2, T3> callBack, T1 val1, T2 val2, T3 val3)
		{
			NetworkMethod<T1, T2, T3> net = new NetworkMethod<T1, T2, T3>(eventName, callBack);
			net.Invoke(val1, val2, val3);

		}
		public void TriggerServerCallback<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callBack, T1 val1, T2 val2, T3 val3, T4 val4)
		{
			NetworkMethod<T1, T2, T3, T4> net = new NetworkMethod<T1, T2, T3, T4>(eventName, callBack);
			net.Invoke(val1, val2, val3, val4);

		}

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
