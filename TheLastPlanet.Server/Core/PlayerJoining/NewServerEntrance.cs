using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.SistemaEventi;
using TheLastPlanet.Shared.Snowflake;

namespace TheLastPlanet.Server.Core.PlayerJoining
{
	internal static class NewServerEntrance
	{
		public static void Init()
		{
			ServerSession.Instance.SistemaEventi.Attach("lprp:setupUser", new AsyncEventCallback(async a =>
			{
				try
				{
					var player = Funzioni.GetPlayerFromId(a.Sender);
					var handle = player.Handle;

					if (ServerSession.PlayerList.ContainsKey(handle)) return ServerSession.PlayerList[handle];
					const string procedure = "call IngressoPlayer(@disc, @lice, @name)";
					User user = new(player, await MySQL.QuerySingleAsync<BasePlayerShared>(procedure, new
					{
						disc = Convert.ToInt64(player.GetLicense(Identifier.Discord)),
						lice = player.GetLicense(Identifier.License), name = player.Name
					}));
					await BaseScript.Delay(1);
					ServerSession.PlayerList.TryAdd(handle, user);
					EntratoMaProprioSulSerio(player);
					return user;
				}
				catch (Exception e)
				{
					Log.Printa(LogType.Error, e.ToString());
					return null;
				}
			}));

			ServerSession.Instance.SistemaEventi.Attach("lprp:RequestLoginInfo", new AsyncEventCallback( async a =>
			{
				string query = "SELECT CharID, info, money, bank FROM personaggi WHERE UserID = @id";
				IEnumerable<LogInInfo> info = await MySQL.QueryListAsync<LogInInfo>(query, new
				{
					id = a.Find<int>(0)
				});
				return info.ToList();
			}));

			ServerSession.Instance.SistemaEventi.Attach("lprp:anteprimaChar", new AsyncEventCallback(async a =>
			{
				string query = "SELECT skin, dressing FROM personaggi WHERE	CharID = @id";
				SkinAndDress res = await MySQL.QuerySingleAsync<SkinAndDress>(query, new
				{
					id = a.Find<ulong>(0)
				});

				Log.Printa(LogType.Debug, res.SerializeToJson());

				return res;
			}));

			ServerSession.Instance.SistemaEventi.Attach("lprp:Select_Char", new AsyncEventCallback(async a =>
			{
				string query = "SELECT * FROM personaggi WHERE CharID = @id";
				Char_data res = await MySQL.QuerySingleAsync<Char_data>(query, new
				{
					id = a.Find<ulong>(0)
				});
				User user = Funzioni.GetUserFromPlayerId(a.Sender);
				user.CurrentChar = res;
				return res;
			}));
		}

		private static async void EntratoMaProprioSulSerio(Player player)
		{
			await ServerSession.Instance.Execute($"UPDATE users SET last_connection = @last WHERE discord = @id",
				new {last = DateTime.Now, id = player.GetLicense(Identifier.Discord)});
		}
	}
}
