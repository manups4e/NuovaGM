using System;
using System.Drawing;
using Console = Colorful.Console;

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

	public static class Log
	{
		public static void Init()
		{
			Server.GetInstance.RegisterEventHandler("PrintToConsole", new Action<int, string>(Printa));
		}

		public static void Printa(LogType tipo, string messaggio)
		{
			CommandWriteLine(tipo, messaggio);
		}

		private static void Printa(int tipo, string messaggio) // usato col triggerserverevent
		{
			CommandWriteLine((LogType)tipo, messaggio);
		}

		private static void CommandWriteLine(LogType tipo, string text)
		{
			string err = "-- [INFO] -- ";
			string incipit = $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss", Server.Ita)}";
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
	}
}
