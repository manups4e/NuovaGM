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
			System.Globalization.CultureInfo Ita = new System.Globalization.CultureInfo("it-IT");
			if (tipo == LogType.Info)
			{
				string testo = $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss", Ita)} -- [INFO] -- ";
				Console.WriteLine(testo + text, Color.LimeGreen);
			}
			else if (tipo == LogType.Debug)
			{
				string testo = $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss", Ita)} -- [DEBUG] -- ";
				Console.WriteLine(testo + text, Color.Cyan);
			}
			else if (tipo == LogType.Warning)
			{
				string testo = $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss", Ita)} -- [ATTENZIONE] -- ";
				Console.WriteLine(testo + text, Color.DarkOrange);
			}
			else if (tipo == LogType.Error)
			{
				string testo = $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss", Ita)} -- [ERRORE] -- ";
				Console.WriteLine(testo + text, Color.OrangeRed);
			}
			else if (tipo == LogType.Fatal)
			{
				string testo = $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss", Ita)} -- [FATALE] -- ";
				Console.WriteLine(testo + text, Color.Yellow);
			}
		}
	}
}
