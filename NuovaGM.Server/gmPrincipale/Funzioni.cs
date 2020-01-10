﻿using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace NuovaGM.Server.gmPrincipale
{
	public static class Funzioni
	{
		public static void Init()
		{
			Server.GetInstance.RegisterTickHandler(Salvataggio);
		}

		public static Player GetPlayerFromId(int id)
		{
			foreach (Player p in Server.GetInstance.GetPlayers)
			{
				if (p.Handle == "" + id)
					return p;
			}
			return null;
		}
		public static Player GetPlayerFromId(string id)
		{
			foreach (Player p in Server.GetInstance.GetPlayers)
			{
				if (p.Handle == id)
					return p;
			}
			return null;
		}

		public static async Task Salvataggio()
		{
			if (ServerEntrance.PlayerList.Count > 0)
			{
				await BaseScript.Delay(ConfigServer.Conf.Main.SalvataggioTutti * 60000);
				foreach (Player player in Server.GetInstance.GetPlayers)
				{
					string name = player.Name;
					if (ServerEntrance.PlayerList.ContainsKey(player.Handle))
					{
						var ped = ServerEntrance.PlayerList[player.Handle];
						if (ped.status.spawned)
						{
							BaseScript.TriggerClientEvent(player, "lprp:mostrasalvataggio");
							await Server.GetInstance.Execute("UPDATE `users` SET `Name` = @name, `group` = @gr, `group_level` = @level, `playTime` = @time, `char_current` = @current, `char_data` = @data WHERE `discord` = @id", new
							{
								name = player.Name,
								gr = ped.group,
								level = ped.group_level,
								time = ped.playTime,
								current = ped.char_current,
								data = JsonConvert.SerializeObject(ped.char_data),
								id = ped.identifiers.discord
							});
							Log.Printa(LogType.Info, "Salvato personaggio: '" + ServerEntrance.PlayerList[player.Handle].FullName + "' appartenente a '" + name + "' - " + ServerEntrance.PlayerList[player.Handle].identifiers.discord);
							BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Salvato personaggio: '" + ServerEntrance.PlayerList[player.Handle].FullName + "' appartenente a '" + name + "' - " + ServerEntrance.PlayerList[player.Handle].identifiers.discord);
							await Task.FromResult(0);
						}
					}
				}
				BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", JsonConvert.SerializeObject(ServerEntrance.PlayerList));
			}
			else
				await BaseScript.Delay(10000);
		}

		public static bool IsPlayerAndHasPermission(int player, int level)
		{
			if (player != 0)
			{
				Player p = GetPlayerFromId(player);
				User Char = GetUserFromPlayerId(p.Handle);
				if (Char.group_level >= level) return true;
			}
			return false;
		}


		public static DateTime TimeStamp2DateTime(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		public static double DateTime2TimeStamp(DateTime dateTime)
		{
			return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		public static User GetUserFromPlayerId(string id)
		{
			return ServerEntrance.PlayerList[id];
		}

	}
}