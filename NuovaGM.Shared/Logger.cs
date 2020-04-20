using System;
using System.Drawing;
using CitizenFX.Core;
#if SERVER
using Console = Colorful.Console;
#endif
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
		public static void Printa(LogType tipo, string text)
		{
			string err = "-- [INFO] -- ";
			string incipit = $"{DateTime.Now:dd/MM/yyyy, HH:mm:ss}";
#if CLIENT
			string colore = "^2";
#elif SERVER
			Color colore = Color.LimeGreen;
#endif
			switch (tipo)
			{
				case LogType.Info:
					err = "-- [INFO] -- ";
#if CLIENT
					colore = "^2";
#elif SERVER
					colore = Color.LimeGreen;
#endif
					break;
				case LogType.Debug:
					err = "-- [DEBUG] -- ";
#if CLIENT
					colore = "^5";
#elif SERVER
					colore = Color.Cyan;
#endif
					break;
				case LogType.Warning:
					err = "-- [ATTENZIONE] --";
#if CLIENT
					colore = "^3";
#elif SERVER
					colore = Color.DarkOrange;
#endif
					break;
				case LogType.Error:
					err = "-- [ERRORE] --";
#if CLIENT
					colore = "^1";
#elif SERVER
					colore = Color.OrangeRed;
#endif
					break;
				case LogType.Fatal:
					err = "-- [FATALE] --";
#if CLIENT
					colore = "^9";
#elif SERVER
					colore = Color.Yellow;
#endif
					break;
			}
#if CLIENT
			Debug.WriteLine($"^4{incipit} {colore}{err} {text}.^7");
#elif SERVER
			Console.WriteLine($"{incipit} {err} {text}", colore);
#endif
		}
	}
}