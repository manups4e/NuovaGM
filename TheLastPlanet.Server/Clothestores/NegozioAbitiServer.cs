using CitizenFX.Core;
using Logger;
using System;
using System.Linq;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;

namespace TheLastPlanet.Server.Clothestores
{
	internal static class NegozioAbitiServer
	{
		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:abiti:compra", new Action<Player, int, int>(Compra));
			Server.Instance.AddEventHandler("lprp:barbiere:compra", new Action<Player, int, int>(CompraBrb));
		}

		private static void Compra([FromSource] Player p, int price, int num)
		{
			var client = Funzioni.GetClientFromPlayerId(int.Parse(p.Handle));
			if (client != null)
			{
				if (num == 1)
				{
					client.User.Money -= price;
					Log.Printa(LogType.Info, $"Il personaggio {client.User.FullName}, appartenente a {p.Name} ha speso {price} in un negozio d'abiti");
				}
				else if (num == 2)
				{
					client.User.Bank -= price;
					Log.Printa(LogType.Info, $"Il personaggio {client.User.FullName}, appartenente a {p.Name} ha speso {price} in un negozio d'abiti");
				}
			}
		}

		private static void CompraBrb([FromSource] Player p, int price, int num)
		{
			var client = Funzioni.GetClientFromPlayerId(int.Parse(p.Handle));
			if (client != null)
			{
				if (num == 1)
				{
					client.User.Money -= price;
					Log.Printa(LogType.Info, $"Il personaggio {client.User.FullName}, appartenente a {p.Name} ha speso {price} in un barbiere");
				}
				else if (num == 2)
				{
					client.User.Bank -= price;
					Log.Printa(LogType.Info, $"Il personaggio {client.User.FullName}, appartenente a {p.Name} ha speso {price} in un barbiere");
				}
			}
		}
	}
}