using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server.Internal.Events;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.SistemaEventi;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Server.Core.PlayerJoining
{
	internal static class NewServerEntrance
	{
		public static void Init()
		{
			Server.Instance.Events.Mount("lprp:setupUser", new Func<ClientId, Task<User>>(SetupUser));
			Server.Instance.Events.Mount("lprp:RequestLoginInfo", new Func<ulong, Task<List<LogInInfo>>>(LogInfo));
			Server.Instance.Events.Mount("lprp:anteprimaChar", new Func<ulong, Task<SkinAndDress>>(PreviewChar));
			Server.Instance.Events.Mount("lprp:Select_Char", new Func<ClientId, ulong, Task<Char_data>>(LoadChar));
		}

		private static async void EntratoMaProprioSulSerio(Player player)
		{
			await Server.Instance.Execute($"UPDATE users SET last_connection = @last WHERE discord = @id",
				new {last = DateTime.Now, id = player.GetLicense(Identifier.Discord)});
		}

		private static async Task<User>SetupUser(ClientId source)
		{
			try
			{
				var player = source.Player;
				var handle = player.Handle;

				if (Server.PlayerList.ContainsKey(handle)) return Server.PlayerList[handle];
				const string procedure = "call IngressoPlayer(@disc, @lice, @name)";
				User user = new(player, await MySQL.QuerySingleAsync<BasePlayerShared>(procedure, new
				{
					disc = Convert.ToInt64(player.GetLicense(Identifier.Discord)),
					lice = player.GetLicense(Identifier.License),
					name = player.Name
				}));
				await BaseScript.Delay(1);
				Server.PlayerList.TryAdd(handle, user);
				EntratoMaProprioSulSerio(player);
				return user;
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e.ToString());
				return default;
			}
		}

		private static async Task<List<LogInInfo>>LogInfo(ulong id)
		{
			var query = "SELECT CharID, info, money, bank FROM personaggi WHERE UserID = @id";
			var info = await MySQL.QueryListAsync<LogInInfo>(query, new { id });
			return info.ToList();
		}

		private static async Task<SkinAndDress> PreviewChar(ulong id)
		{
			string query = "SELECT skin, dressing FROM personaggi WHERE	CharID = @id";
			SkinAndDress res = await MySQL.QuerySingleAsync<SkinAndDress>(query, new { id });
			return res;
		}

		private static async Task<Char_data> LoadChar(ClientId source, ulong id)
		{
			string query = "SELECT * FROM personaggi WHERE CharID = @id";
			User user = source.Player.GetCurrentChar();
			Char_Metadata res = await MySQL.QuerySingleAsync<Char_Metadata>(query, new { id });
			user.CurrentChar = new Char_data()
			{
				Info = res.info.FromJson<Info>(),
				Finance = new Finance(res.money, res.bank, res.dirtyCash),
				Posizione = res.location.FromJson<Position>(),
				Job = new Job(res.job, res.job_grade),
				Gang = new Gang(res.gang, res.gang_grade),
				Skin = res.skin.FromJson<Skin>(),
				Inventory = res.inventory.FromJson<List<Inventory>>(),
				Weapons = res.weapons.FromJson<List<Weapons>>(),
				Dressing = res.dressing.FromJson<Dressing>(),
				Needs = res.needs.FromJson<Needs>(),
				Statistiche = res.statistiche.FromJson<Statistiche>(),
			};
			return user.CurrentChar;
		}
	}
}
