using CitizenFX.Core;
using Logger;
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
			Server.Instance.AddEventHandler("lprp:setupUser", new Action<Player>(setupUser));
			Server.Instance.AddTick(Orario);
			Server.Instance.AddTick(PlayTime);
		}

		public static async void setupUser([FromSource] Player player)
		{
			await BaseScript.Delay(0);
			string handle = player.Handle;
			dynamic result = await Server.Instance.Query($"SELECT * FROM users WHERE discord = @disc", new
			{
				disc = License.GetLicense(player, Identifier.Discord)
			});
			await BaseScript.Delay(0);
			if (result != null)
			{
				string stringa = JsonConvert.SerializeObject(result);
				if (stringa != "[]" && stringa != "{}" && stringa != null)
				{
					User user = new User(player, result[0]);
					Server.PlayerList.TryAdd(handle, user);
					string playerino = user.Serialize();
					player.TriggerEvent("lprp:setupClientUser", playerino);
				}
				else
				{
					await Server.Instance.Execute($"INSERT INTO `users` VALUES (@disc, @lice, @name, @group, @level, @time, @last, @current, @data)", new
					{
						disc = License.GetLicense(player, Identifier.Discord),
						lice = License.GetLicense(player, Identifier.License),
						name = GetPlayerName(handle),
						group = "normal",
						level = 0,
						time = 0,
						last = DateTime.Now,
						current = 0,
						data = (new object[] { }).Serialize()
					});
					await BaseScript.Delay(0);

					dynamic created = await Server.Instance.Query($"SELECT * FROM users WHERE discord = @discord", new
					{
						discord = License.GetLicense(player, Identifier.Discord)
					});
					await BaseScript.Delay(0);
					User user = new User(player, created[0]);
					Server.PlayerList.TryAdd(handle, user);
					string playerino = user.Serialize();
					player.TriggerEvent("lprp:setupClientUser", playerino);
				}
			}
			EntratoMaProprioSulSerio(player);
		}

		public static async void EntratoMaProprioSulSerio(Player player)
		{
			await Server.Instance.Execute($"UPDATE users SET last_connection = @last WHERE discord = @id", new { last = DateTime.Now, id = License.GetLicense(player, Identifier.Discord) });
			await BaseScript.Delay(0);
		}

		public static long starttick = GetGameTimer();
		public static async Task Orario()
		{
			try
			{
				long tick = GetGameTimer();
				double uptimeDay = Math.Floor((double)(tick - starttick) / 86400000);
				double uptimeHour = Math.Floor((double)(tick - starttick) / 3600000) % 24;
				double uptimeMinute = Math.Floor((double)(tick - starttick) / 60000) % 60;
				ExecuteCommand($"sets Attivo \"{uptimeDay} giorni {uptimeHour} Ore {uptimeMinute} Minuti \"");
				await BaseScript.Delay(60000);
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e.ToString() + e.StackTrace);
			}
		}

		private static async Task PlayTime()
		{
			try
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
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e.ToString() + e.StackTrace);
			}
		}
	}
}
