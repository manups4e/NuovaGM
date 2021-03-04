using System;
using System.IO;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using MsgPack.Serialization;
using System.Linq;

namespace TheLastPlanet.Server.Core.PlayerJoining
{
	internal static class NewServerEntrance
	{
		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:setupUser", new Action<Player>(SetupUser));
		}

		public static async void SetupUser([FromSource] Player player)
		{
			try
			{
				string handle = player.Handle;

				if (Server.PlayerList.ContainsKey(handle)) return;
				string procedure = "call IngressoPlayer(@disc, @lice, @name)";
				User user = new User(player, await MySQL.QuerySingleAsync<dynamic>(procedure, new { disc = Convert.ToInt64(player.GetLicense(Identifier.Discord)), lice = player.GetLicense(Identifier.License), name = player.Name }));
				Server.PlayerList.TryAdd(handle, user);
				player.TriggerEvent("lprp:setupClientUser", await user.Serialize());
				EntratoMaProprioSulSerio(player);
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e.ToString());
			}
		}

		private static async void EntratoMaProprioSulSerio(Player player)
		{
			await Server.Instance.Execute($"UPDATE users SET last_connection = @last WHERE discord = @id", new { last = DateTime.Now, id = player.GetLicense(Identifier.Discord) });
			await BaseScript.Delay(0);
		}
	}
}