﻿using CitizenFX.Core;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core
{
	internal static class ChatServer
	{
		public static List<ChatCommand> Commands = new List<ChatCommand>();
		public static List<ChatSuggestion> Suggestions = new List<ChatSuggestion>();

		public static void Init()
		{
			ServerSession.Instance.AddEventHandler("chatMessage", new Action<int, string, string>(chatMessage));
			ServerSession.Instance.AddEventHandler("consoleCommand", new Action<string, string>(ConsoleCommand));
			ServerSession.Instance.AddEventHandler("lprp:chat:commands", new Action<Player>(SendComms));
		}

		public static void chatMessage(int id, string name, string message)
		{
			Player p = Funzioni.GetPlayerFromId(id);
			User user = p.GetCurrentChar();

			if ((int)user.group_level <= -1) return;
			if (!user.status.Spawned && user.group_level <= UserGroup.Helper) return;

			if (message.StartsWith("/"))
			{
				string fullCommand = message.Replace("/", "");
				string[] command = fullCommand.Split(' ');
				string cmd = command[0];
				ChatCommand comm = null;
				if (Commands.Any(x => x.CommandName.ToLower() == cmd.ToLower())) comm = Commands.FirstOrDefault(x => x.CommandName.ToLower() == cmd.ToLower());

				if (comm != null)
					if (user.group_level >= comm.Restriction)
					{
						comm.Source = p;
						comm.rawCommand = message;
						if (command.Length > 1)
							comm.Args = command.Skip(1).ToList();
						else
							comm.Args = new List<string>();
						comm.Action.DynamicInvoke(p, comm.Args, comm.rawCommand);
					}

				chatCommandEntered(p, fullCommand, command, cmd, comm);
			}
			else
			{
				BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", Convert.ToInt32(p.Handle), /*user.FullName + ":",*/ message);
			}

			CancelEvent();
		}

		private static void ConsoleCommand(string name, string command) { }

		public static void chatCommandEntered(Player sender, string fullCommand, string[] command, string cmd, ChatCommand comm)
		{
			DateTime data = DateTime.Now;
			User user = Funzioni.GetUserFromPlayerId(sender.Handle);

			if (comm != null)
			{
				if (user.group_level >= comm.Restriction)
				{
					string txt;
					if (command.Length > 1)
						txt = $"Comando: /{cmd} invocato da {sender.Name} con testo: {fullCommand.Substring(cmd.Length + 1)}";
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
			else
			{
				user.showNotification("Hai inserito un comando non valido!");
				Log.Printa(LogType.Warning, sender.Name + " ha inserito un comando non valido: " + cmd);
				BaseScript.TriggerEvent("lprp:serverLog", data.ToString("dd/MM/yyyy, HH:mm:ss") + " -- " + sender.Name + " ha inserito un comando non valido: " + cmd + ".");
			}
		}

		private static void SendComms([FromSource] Player p)
		{
			List<object> suggestions = new List<object>();

			foreach (ChatSuggestion sug in Suggestions)
			{
				List<object> paramss = new List<object>();
				foreach (SuggestionParam par in sug.@params) paramss.Add(new { par.name, par.help });
				suggestions.Add(new { sug.name, sug.help, @params = paramss });
			}

			p.TriggerEvent("chat:addSuggestions", suggestions);
		}
	}
}