using System;
using System.IO;
using CitizenFX.Core;
using CitizenFX.Core.Native;
#if SERVER
using TheLastPlanet.Server;
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
#if SERVER
		private static StreamWriter _writer;
		static Log()
		{
			_writer = File.AppendText($"Logs\\Server__{DateTime.Now:dd-MM-yyyy}.log");
			Server.Instance.AddEventHandler("onResourceStop", new Action<string>(Stop));
		}

		private static void Stop(string resname)
		{
			if (resname == API.GetCurrentResourceName())
				_writer.Close();
		}
#endif

		// Colors
		public const string LIGHT_RED = "^1";
		public const string LIGHT_GREEN = "^2";
		public const string YELLOW = "^3";
		public const string DARK_BLUE = "^4";
		public const string LIGHT_BLUE = "^5";
		public const string PURPLE = "^6";
		public const string WHITE = "^7";
		public const string DARK_RED = "^8";
		public const string PINK = "^9";

		// Other formatting
		public const string BOLD = "^*";
		public const string UNDERLINE = "^_";
		public const string STRIKETHROUGH = "^~";
		public const string UNDERLINE_STRIKETHROUGH = "^=";
		public const string UNDERLINE_STRIKETHROUGH_BOLD = "*^=";
		public const string UNDERLINE_BOLD = "*=";

		// Reset
		public const string RESET = "^r";

		/// <summary>
		/// Manda in console un messaggio colorato in base alla gravità della situazione
		/// </summary>
		/// <param name="tipo">La gravità della situazione</param>
		/// <param name="text">Testo del messaggio</param>
		public static async void Printa(LogType tipo, string text)
		{
			string err = "-- [INFO] -- ";
			string incipit = $"{DateTime.Now:dd/MM/yyyy, HH:mm}";
			string colore = LIGHT_GREEN;
			switch (tipo)
			{
				case LogType.Info:
					err = "-- [INFO] -- ";
					colore = LIGHT_GREEN;
					break;
				case LogType.Debug:
					if (API.GetConvarInt("DEBUG", 0) == 0) return;
					err = "-- [DEBUG] -- ";
					colore = PURPLE;
					break;
				case LogType.Warning:
					err = "-- [ATTENZIONE] --";
					colore = YELLOW;
					break;
				case LogType.Error:
					err = "-- [ERRORE] --";
					colore = LIGHT_RED;
					break;
				case LogType.Fatal:
					err = "-- [FATALE] --";
					colore = PINK;
					break;
			}
			Debug.WriteLine($"{colore}{incipit} {err} {text}.^7");
#if SERVER
			try
			{
				if (tipo != LogType.Debug)
					await _writer.WriteLineAsync($"{incipit} {err} {text}");
			}
			catch(Exception e)
			{
				Debug.WriteLine($"^1{incipit} -- [ERRORE] -- {e}.^7");
			}
#endif
		}


	}
}