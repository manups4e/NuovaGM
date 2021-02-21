using CitizenFX.Core;
using Logger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core
{
	static class ChatServer
	{
		public static List<ChatCommand> Commands = new List<ChatCommand>();
		public static ConcurrentDictionary<string, int> commands = new ConcurrentDictionary<string, int>()
		{
			["annuncio"] = 2,
			["givemoney"] = 2,
			["givebank"] = 2,
			["givedirty"] = 2,
			["removemoney"] = 2,
			["removebank"] = 2,
			["removedirty"] = 2,
			["setmoney"] = 2,
			["giveitem"] = 2,
			["removeitem"] = 2,
			["giveweapon"] = 2,
			["removeweapon"] = 2,
			["setjob"] = 2,
			["setgang"] = 2,
			["revive"] = 2, //0 perché non esistono i medici
			["tp"] = 2,
			["tpalplayer"] = 2,
			["jail"] = 2,
			["unjail"] = 2,
			["car"] = 2, //0 perché non esistono le auto personali
			["dv"] = 2, //0 perché non esistono le auto personali
			["suicidati"] = 2,
			["spawnped"] = 2,
			["spawnobject"] = 2,
			["delgun"] = 2,
			["kick"] = 2,
			["ban"] = 2,
			["tempban"] = 2,
			["ooc"] = 0,
			["me"] = 0,
			["do"] = 0,
			["pol"] = 0,
			["pil"] = 0,
			["med"] = 0,
			["mec"] = 0,
			["twt"] = 0,
			["news"] = 0,
			["aiuto"] = 0,
			["lprphelp"] = 0,
			["setgroup"] = 3,
			["salvatutti"] = 2,
			["sviluppatore"] = 5,
			["cambiaora"] = 3,
			["bloccatempo"] = 3,
			["setmeteo"] = 3,
			["dailicenza"] = 3,
			["rimuovilicenza"] = 3,
		};
		public static void Init()
		{
			Server.Instance.AddEventHandler("chatMessage", new Action<int, string, string>(chatMessage));
			Server.Instance.AddEventHandler("consoleCommand", new Action<string, string>(ConsoleCommand));
		}

		public static void chatMessage(int id, string name, string message)
		{
			Player p = Funzioni.GetPlayerFromId(id);
			User user = p.GetCurrentChar();
			if ((int)user.group_level > -1)
			{
				if (message.StartsWith("/"))
				{
					string fullCommand = message.Replace("/", "");
					string[] command = fullCommand.Split(' ');
					string cmd = command[0];
					if (Commands.Any(x => x.CommandName.ToLower() == cmd.ToLower()))
					{
						ChatCommand comm = Commands.FirstOrDefault(x => x.CommandName.ToLower() == cmd.ToLower());
						if (user.group_level >= comm.Restriction)
						{
							comm.Source = p;
							comm.rawCommand = message;
							if (command.Length > 1)
								comm.Args = command.Skip(0).ToList();
							else comm.Args = new List<string>();
							comm.Action.DynamicInvoke(p, comm.Args, comm.rawCommand);
						}
					}
					chatCommandEntered(p, message);
				}
				else
					BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", Convert.ToInt32(p.Handle), /*user.FullName + ":",*/ message);
				CancelEvent();
			}
		}

		private static void ConsoleCommand(string name, string command)
		{

		}

		public static void chatCommandEntered(Player sender, string input)
		{
			var data = DateTime.Now;
			string fullCommand = input.Replace("/", "");
			string[] command = fullCommand.Split(' ');
			string cmd = command[0];
			int adminLevel = 0;
			command = command.Skip(0).ToArray();
			cmd = cmd.ToLower();
			var user = Funzioni.GetUserFromPlayerId(sender.Handle);
			if (Commands.Any(x => x.CommandName.ToLower() == cmd.ToLower()))
			{
				ChatCommand comm = Commands.FirstOrDefault(x => x.CommandName.ToLower() == cmd.ToLower());
				{
					if (user.group_level >= comm.Restriction)
					{
						string txt = "";
						if (command.Length > 1)
							txt = $"Comando: /{cmd} invocato da {sender.Name} con testo: {fullCommand.Substring(cmd.Length)}";
						else
							txt = $"Comando: /{cmd} invocato da {sender.Name}";
						Log.Printa(LogType.Info, txt);
						BaseScript.TriggerEvent("lprp:serverLog", data.ToString("dd/MM/yyyy, HH:mm:ss") + " -- " + txt);
					}
					else
					{
						user.showNotification("Non hai i permessi per usare questo comando!");
						Log.Printa(LogType.Warning, sender.Name + " ha provato a usare il comando " + cmd + ".");
						BaseScript.TriggerEvent("lprp:serverLog", data.ToString("dd/MM/yyyy, HH:mm:ss") + " -- " + sender.Name + " ha provato a usare il comando " + cmd + ".");
					} 
				}
			}
			else
			{
				user.showNotification("Hai inserito un comando non valido!");
				Log.Printa(LogType.Warning, sender.Name + " ha inserito un comando non valido: " + cmd);
				BaseScript.TriggerEvent("lprp:serverLog", data.ToString("dd/MM/yyyy, HH:mm:ss") + " -- " + sender.Name + " ha inserito un comando non valido: " + cmd + ".");
			}
		}
	}
}
