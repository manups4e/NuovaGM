using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Linq;
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
			foreach (Player p in Server.GetInstance.GetPlayers.ToList())
			{
				if (p.Handle == "" + id)
					return p;
			}
			return null;
		}
		public static Player GetPlayerFromId(string id)
		{
			foreach (Player p in Server.GetInstance.GetPlayers.ToList())
			{
				if (p.Handle == id)
					return p;
			}
			return null;
		}

		public static async void SalvaPersonaggio(Player player)
		{
			var ped = GetUserFromPlayerId(player.Handle);
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
		}

		public static async Task Salvataggio()
		{
			if (ServerEntrance.PlayerList.Count > 0)
			{
				await BaseScript.Delay(ConfigServer.Conf.Main.SalvataggioTutti * 60000);
				foreach (Player player in Server.GetInstance.GetPlayers.ToList())
				{
					string name = player.Name;
					if (ServerEntrance.PlayerList.ContainsKey(player.Handle))
					{
						var ped = ServerEntrance.PlayerList[player.Handle];
						if (ped.status.spawned)
						{
							BaseScript.TriggerClientEvent(player, "lprp:mostrasalvataggio");
							SalvaPersonaggio(player);
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

		private static Random random = new Random();
		public static float RandomFloatInRange(float minimum, float maximum)
		{
			return (float)random.NextDouble() * (maximum - minimum) + minimum;
		}

	}

	public static class RandomExtensionMethods
	{
		/// <summary>
		/// Returns a random long from min (inclusive) to max (exclusive)
		/// </summary>
		/// <param name="random">The given random instance</param>
		/// <param name="min">The inclusive minimum bound</param>
		/// <param name="max">The exclusive maximum bound.  Must be greater than min</param>
		public static long NextLong(this Random random, long min, long max)
		{
			if (max <= min)
			{
				throw new ArgumentOutOfRangeException("max", "max must be > min!");
			}

			//Working with ulong so that modulo works correctly with values > long.MaxValue
			ulong uRange = (ulong)(max - min);

			//Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
			//for more information.
			//In the worst case, the expected number of calls is 2 (though usually it's
			//much closer to 1) so this loop doesn't really hurt performance at all.
			ulong ulongRand;
			do
			{
				byte[] buf = new byte[8];
				random.NextBytes(buf);
				ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
			} while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

			return (long)(ulongRand % uRange) + min;
		}

		/// <summary>
		/// Returns a random long from 0 (inclusive) to max (exclusive)
		/// </summary>
		/// <param name="random">The given random instance</param>
		/// <param name="max">The exclusive maximum bound.  Must be greater than 0</param>
		public static long NextLong(this Random random, long max)
		{
			return random.NextLong(0, max);
		}

		/// <summary>
		/// Returns a random long over all possible values of long (except long.MaxValue, similar to
		/// random.Next())
		/// </summary>
		/// <param name="random">The given random instance</param>
		public static long NextLong(this Random random)
		{
			return random.NextLong(long.MinValue, long.MaxValue);
		}
	}
}