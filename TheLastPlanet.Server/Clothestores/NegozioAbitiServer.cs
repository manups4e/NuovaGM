using CitizenFX.Core;
using Logger;
using System;
using TheLastPlanet.Server.Core.PlayerChar;

namespace TheLastPlanet.Server.Clothestores
{
	internal static class NegozioAbitiServer
	{
		public static void Init()
		{
			ServerSession.Instance.AddEventHandler("lprp:abiti:compra", new Action<Player, int, int>(Compra));
			ServerSession.Instance.AddEventHandler("lprp:barbiere:compra", new Action<Player, int, int>(CompraBrb));
		}

		private static void Compra([FromSource] Player p, int price, int num)
		{
			User user;
			ServerSession.PlayerList.TryGetValue(p.Handle, out user);

			if (num == 1)
			{
				user.Money -= price;
				Log.Printa(LogType.Info, $"Il personaggio {user.FullName}, appartenente a {p.Name} ha speso {price} in un negozio d'abiti");
				BaseScript.TriggerEvent("lprp:serverLog", $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss")} - Il personaggio {user.FullName}, appartenente a {p.Name} ha speso {price} in un negozio d'abiti");
			}
			else if (num == 2)
			{
				user.Bank -= price;
				Log.Printa(LogType.Info, $"Il personaggio {user.FullName}, appartenente a {p.Name} ha speso {price} in un negozio d'abiti");
				BaseScript.TriggerEvent("lprp:serverLog", $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss")} - Il personaggio {user.FullName}, appartenente a {p.Name} ha speso {price} in un negozio d'abiti");
			}
		}

		private static void CompraBrb([FromSource] Player p, int price, int num)
		{
			User user;
			ServerSession.PlayerList.TryGetValue(p.Handle, out user);

			if (num == 1)
			{
				user.Money -= price;
				Log.Printa(LogType.Info, $"Il personaggio {user.FullName}, appartenente a {p.Name} ha speso {price} in un barbiere");
				BaseScript.TriggerEvent("lprp:serverLog", $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss")} - Il personaggio {user.FullName}, appartenente a {p.Name} ha speso {price} in un barbiere");
			}
			else if (num == 2)
			{
				user.Bank -= price;
				Log.Printa(LogType.Info, $"Il personaggio {user.FullName}, appartenente a {p.Name} ha speso {price} in un barbiere");
				BaseScript.TriggerEvent("lprp:serverLog", $"{DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss")} - Il personaggio {user.FullName}, appartenente a {p.Name} ha speso {price} in un barbiere");
			}
		}
	}
}