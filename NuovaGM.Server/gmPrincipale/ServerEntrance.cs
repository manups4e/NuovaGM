﻿using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.gmPrincipale
{
	public static class ServerEntrance
	{

		public static void Init()
		{
			Server.Instance.RegisterEventHandler("lprp:setupUser", new Action<Player>(setupUser));
			Server.Instance.RegisterTickHandler(Orario);
			Server.Instance.RegisterTickHandler(PlayTime);
		}

		public static async void setupUser([FromSource] Player player)
		{
			await BaseScript.Delay(0);
			string handle = player.Handle;
			dynamic result = await Server.Instance.Query($"SELECT * FROM users WHERE discord = @disc", new
			{
				disc = License.GetLicense(player, Identifier.Discord)
			});
			if (result != null)
			{
				string stringa = JsonConvert.SerializeObject(result);
				if (stringa != "[]" && stringa != "{}" && stringa != null)
				{
					Server.PlayerList.TryAdd(handle, new User(player, result[0]));
					string playerino = JsonConvert.SerializeObject(Server.PlayerList[handle]);
					BaseScript.TriggerClientEvent(player, "lprp:setupClientUser", playerino);
				}
				else
				{
					await Server.Instance.Execute($"INSERT INTO users (discord, license, Name, group, playTime, last_connection, char_current, char_data) VALUES (@disc, @lice, @name, @group, @time, @last, @current, @data)", new
					{
						disc = License.GetLicense(player, Identifier.Discord),
						lice = License.GetLicense(player, Identifier.License),
						name = GetPlayerName(handle),
						group = "normal",
						time = 0,
						last = DateTime.Now,
						current = 0,
						data = "{}"
					});
					dynamic created = await Server.Instance.Query($"SELECT * FROM users WHERE discord = @discord", new
					{
						discord = License.GetLicense(player, Identifier.Discord)
					});
					Server.PlayerList.TryAdd(handle, new User(player, created[0]));
					string playerino = JsonConvert.SerializeObject(Server.PlayerList[handle]);
					BaseScript.TriggerClientEvent(player, "lprp:setupClientUser", playerino);
				}
			}
			EntratoMaProprioSulSerio(player);
		}

		public static async void EntratoMaProprioSulSerio(Player player)
		{
			await BaseScript.Delay(0);
			await Server.Instance.Execute($"UPDATE users SET last_connection = @last WHERE discord = @id", new { last = DateTime.Now, id = License.GetLicense(player, Identifier.Discord) });
		}

		public static long starttick = GetGameTimer();
		public static async Task Orario()
		{
			await BaseScript.Delay(0);
			long tick = GetGameTimer();
			double uptimeDay = Math.Floor((double)(tick - starttick) / 86400000);
			double uptimeHour = Math.Floor((double)(tick - starttick) / 3600000) % 24;
			double uptimeMinute = Math.Floor((double)(tick - starttick) / 60000) % 60;
			ExecuteCommand($"sets Attivo \"{uptimeDay} giorni {uptimeHour} Ore {uptimeMinute} Minuti \"");
			await BaseScript.Delay(60000);
		}

		private static async Task PlayTime()
		{
			await BaseScript.Delay(60000);
			if (Server.PlayerList.Count > 0)
			{
				foreach (var user in Server.PlayerList)
				{
					if (user.Value.status.spawned)
						user.Value.playTime += 60;
				}
			}
		}
	}
}
