using System;
using System.IO;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Logger
{
	public enum LogType
	{
		Info,
		Debug,
		Warning,
		Error,
		Fatal
	}

	public static class Log
	{
		/// <summary>
		/// Manda in console un messaggio colorato in base alla gravità della situazione
		/// </summary>
		/// <param name="tipo">La gravità della situazione</param>
		/// <param name="text">Testo del messaggio</param>
		public static async void Printa(LogType tipo, string text)
		{
			string err = "-- [INFO] -- ";
			string incipit = $"{DateTime.Now:dd/MM/yyyy, HH:mm:ss}";
			string colore = "^2";
			switch (tipo)
			{
				case LogType.Info:
					err = "-- [INFO] -- ";
					colore = "^2";
					break;
				case LogType.Debug:
					if (API.GetConvarInt("DEBUG", 0) == 0) return;
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
			Debug.WriteLine($"{colore}{incipit} {err} {text}.^7");
#if SERVER
			try
			{
				if (tipo != LogType.Debug)
				{
					using StreamWriter w = File.AppendText($"Logs\\Server__{DateTime.Now:dd-MM-yyyy}.log");
					await w.WriteLineAsync($"{incipit} {err} {text}");
				}
			}
			catch(Exception e)
			{
				Debug.WriteLine($"^1{incipit} -- [ERRORE] -- {e}.^7");
			}
#endif
		}
	}
}