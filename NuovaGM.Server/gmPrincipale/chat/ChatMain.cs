using CitizenFX.Core;
using Logger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.gmPrincipale
{
	static class ChatMain
	{
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
			["revive"] = 2,
			["tp"] = 2,
			["tpalplayer"] = 2,
			["jail"] = 2,
			["unjail"] = 2,
			["car"] = 2,
			["dv"] = 2,
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
		}

		public static void chatMessage(int id, string name, string message)
		{
			var user = Funzioni.GetUserFromPlayerId(id.ToString());
			if (user.group_level > -1)
			{
				Player p = Funzioni.GetPlayerFromId(id);
				if (message.Substring(0, 1) != "/")
				{
					BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", Convert.ToInt32(p.Handle), user.FullName + ":", message, 00, 102, 255);
					CancelEvent();
				}
				else
					CancelEvent();
			}
		}

		public static void chatCommandEntered(Player sender, string input)
		{
			var data = DateTime.Now;
			string fullCommand = input.Replace("/", "");
			string[] command = fullCommand.Split(' ');
			string cmd = command[0];
			bool goodArgument = false;
			int adminLevel = 0;
			command = command.Where(o => o != cmd).ToArray();
			cmd = cmd.ToLower();

			var user = Funzioni.GetUserFromPlayerId(sender.Handle);
			if (user.group_level > -1)
				adminLevel = user.group_level;
			if (adminLevel >= commands[cmd])
			{
				string msg = "";
				for (int i = 0; i < command.Length; i++)
					msg += " " + command[i];
				Log.Printa(LogType.Info, "Comando: /" + cmd + " invocato da " + sender.Name + " con testo:" + msg + ".");
				BaseScript.TriggerEvent("lprp:serverLog", data.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Comando: /" + cmd + " invocato da " + sender.Name + " con testo:" + msg + ".");
			}
			else
			{
				BaseScript.TriggerClientEvent(sender, "lprp:ShowNotification", "Non hai i permessi per usare questo comando!");
				Log.Printa(LogType.Warning, sender.Name + " ha provato a usare il comando " + cmd + ".");
				BaseScript.TriggerEvent("lprp:serverLog", data.ToString("dd/MM/yyyy, HH:mm:ss") + " -- " + sender.Name + " ha provato a usare il comando " + cmd + ".");
			}
		}
	}
}
