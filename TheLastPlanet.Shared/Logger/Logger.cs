using CitizenFX.Core.Native;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Logger
{
    public class Log
    {
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

        public Log() { }

#if SERVER
		public async Task Writer(string err, string text)
		{
			string incipit = $"{DateTime.Now:dd/MM/yyyy, HH:mm}";
			try
			{
				
				using var _writer = File.AppendText($"Logs\\Server__{DateTime.Now:dd-MM-yyyy}.log");
				await _writer.WriteLineAsync($"{incipit} {err} {text}");
				_writer.Close();
			}
			catch (Exception e)
			{
				CitizenFX.Core.Debug.WriteLine($"{LIGHT_RED}{incipit} -- [ERRORE] -- {e}.^7");
			}
			await Task.FromResult(0);
		}
#endif

        /// <summary>
        /// Manda in console un messaggio di informazione
        /// </summary>
        /// <param name="text">Testo del messaggio</param>
        public async void Info(string text)
        {
            string incipit = $"{DateTime.Now:dd/MM/yyyy, HH:mm}";
            string err = "-- [INFO] -- ";
            string colore = LIGHT_GREEN;
            CitizenFX.Core.Debug.WriteLine($"{colore}{incipit} {err} {text}.^7");
#if SERVER
			await Writer(err, text);
#endif
        }

        /// <summary>
        /// Manda in console un messaggio di Debug
        /// </summary>
        /// <param name="text">Testo del messaggio</param>
        public async void Debug(string text)
        {
            if (API.GetConvarInt("DEBUG", 0) == 0) return;
            string incipit = $"{DateTime.Now:dd/MM/yyyy, HH:mm}";
            string err = "-- [DEBUG] -- ";
            string colore = PURPLE;
            CitizenFX.Core.Debug.WriteLine($"{colore}{incipit} {err} {text}.^7");
            /*
            #if SERVER
                        await Writer(err, text);
            #endif
            */
        }

        /// <summary>
        /// Manda in console un messaggio di Avviso
        /// </summary>
        /// <param name="text">Testo del messaggio</param>
        public async void Warning(string text)
        {
            string incipit = $"{DateTime.Now:dd/MM/yyyy, HH:mm}";
            string err = "-- [ATTENZIONE] --";
            string colore = YELLOW;
            CitizenFX.Core.Debug.WriteLine($"{colore}{incipit} {err} {text}.^7");
#if SERVER
			await Writer(err, text);
#endif
        }

        /// <summary>
        /// Manda in console un messaggio di Errore
        /// </summary>
        /// <param name="text">Testo del messaggio</param>
        public async void Error(string text)
        {
            string incipit = $"{DateTime.Now:dd/MM/yyyy, HH:mm}";
            string err = "-- [ERRORE] -- ";
            string colore = LIGHT_RED;
            CitizenFX.Core.Debug.WriteLine($"{colore}{incipit} {err} {text}.^7");
#if SERVER
			await Writer(err, text);
#endif
        }

        /// <summary>
        /// Manda in console un messaggio di Errore FATALE
        /// </summary>
        /// <param name="text">Testo del messaggio</param>
        public async void Fatal(string text)
        {
            string incipit = $"{DateTime.Now:dd/MM/yyyy, HH:mm}";
            string err = "-- [FATALE] -- ";
            string colore = DARK_RED;
            CitizenFX.Core.Debug.WriteLine($"{colore}{incipit} {err} {text}.^7");
#if SERVER
			await Writer(err, text);
#endif
        }
    }
}