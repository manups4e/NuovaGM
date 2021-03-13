using System;
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
					if (API.GetResourceMetadata(API.GetCurrentResourceName(), "enable_debug_prints_for_events", 0).ToLower() == "true")
					{
						err = "-- [DEBUG] -- ";
						colore = "^5";
					}
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

			await BaseScript.Delay(1); // aspettiamo 1ms per gestire tutti i SIGSEGV che appaiono a causa della mancata attesa dopo il Database
			Debug.WriteLine($"{colore}{incipit} {err} {text}.^7");
		}
	}
}