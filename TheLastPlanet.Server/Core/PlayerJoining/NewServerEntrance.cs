using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using TheLastPlanet.Server.SistemaEventi;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.SistemaEventi;

namespace TheLastPlanet.Server.Core.PlayerJoining
{
	internal static class NewServerEntrance
	{
		public static void Init()
		{
			Server.Instance.Eventi.Attach("lprp:setupUser", new AsyncEventCallback(async a =>
			{
				try
				{
					var player = Funzioni.GetPlayerFromId(a.Sender);
					var handle = player.Handle;

					if (Server.PlayerList.ContainsKey(handle)) return Server.PlayerList[handle];
					const string procedure = "call IngressoPlayer(@disc, @lice, @name)";
					User user = new(player, await MySQL.QuerySingleAsync<BasePlayerShared>(procedure, new { disc = Convert.ToInt64(player.GetLicense(Identifier.Discord)), lice = player.GetLicense(Identifier.License), name = player.Name }));
					await BaseScript.Delay(1);
					Server.PlayerList.TryAdd(handle, user);
					EntratoMaProprioSulSerio(player);
					return user;
				}
				catch (Exception e)
				{
					Log.Printa(LogType.Error, e.ToString());

					return null;
				}
			}));
		}

		private static async void EntratoMaProprioSulSerio(Player player)
		{
			await Server.Instance.Execute($"UPDATE users SET last_connection = @last WHERE discord = @id", new { last = DateTime.Now, id = player.GetLicense(Identifier.Discord) });
		}
	}
}