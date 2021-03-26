using CitizenFX.Core;
using Logger;
using TheLastPlanet.Server.Core;
using System;
using System.Linq;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Shared.SistemaEventi;
using System.Collections.Generic;
using TheLastPlanet.Server.Core.PlayerChar;
using System.Threading.Tasks;
using TheLastPlanet.Server.Internal.Events;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.banking
{
	internal static class BankingServer
	{
		public static void Init()
		{
			Server.Instance.Events.Mount("lprp:banking:sendMoney", new Func<ClientId, string, int, Task<KeyValuePair<bool, string>>>(SendMoney));
			Server.Instance.Events.Mount("lprp:banking:atmwithdraw", new Func<ClientId, int, Task<KeyValuePair<bool, string>>>(Withdraw));
			Server.Instance.Events.Mount("lprp:banking:atmdeposit", new Func<ClientId, int, Task<KeyValuePair<bool, string>>>(Deposit));
		}

		private static async Task<KeyValuePair<bool, string>> SendMoney(ClientId source, string name, int amount)
		{
			User user = source.User;
			if (user.Bank >= amount)
			{
				foreach (var p in Server.Instance.Clients)
				{
					if (name.ToLower() == p.User.FullName.ToLower())
					{
						user.Bank -= amount;
						p.User.Bank += amount;
						Log.Printa(LogType.Info, $"Il personaggio '{user.FullName}' [{source.Player.Name}] ha inviato ${amount} a '{p.User.FullName}' [{p.Player.Name}]");
						return new KeyValuePair<bool, string>(true, user.Bank.ToString());
					}
				}
				return new KeyValuePair<bool, string>(false, "Utente non trovato");
			}
			return new KeyValuePair<bool, string>(false, "I tuoi fondi bancari non coprono la transazione!");
		}

		private static async Task<KeyValuePair<bool, string>> Withdraw(ClientId source, int amount)
		{
			var player = source.Player;
			if (amount > 0)
			{
				User user = player.GetCurrentChar();
				int bal = user.Bank;
				int newamt = bal - amount;

				if (bal >= amount)
				{
					user.Money += amount;
					Log.Printa(LogType.Info, $"Il personaggio '{user.FullName}' [{player.Name}] ha depositato {amount}$");
					user.Bank -= amount;
					return new KeyValuePair<bool, string>(true, newamt.ToString());
				}
				return new KeyValuePair<bool, string>(false, "Non hai abbastanza fondi nel tuo conto corrente per questa transazione.");
			}
			return new KeyValuePair<bool, string>(false, "Devi inserire un valore positivo.");
		}

		private static async Task<KeyValuePair<bool, string>> Deposit(ClientId source, int amount)
		{
			var player = source.Player;
			if (amount > 0)
			{
				User user = player.GetCurrentChar();
				int money = user.Money;
				int bankmoney = user.Bank;
				int newamt = bankmoney + amount;

				if (amount <= money)
				{
					user.Money -= amount;
					Log.Printa(LogType.Info, $"Il personaggio '{user.FullName}' [{player.Name}] ha depositato {amount}$");
					user.Bank += amount;
					return new KeyValuePair<bool, string>(true, newamt.ToString());
				}
				return new KeyValuePair<bool, string>(false, "Non hai abbastanza soldi nel tuo portafoglio per questa transazione.");
			}
			return new KeyValuePair<bool, string>(false, "Devi inserire un valore positivo.");

		}
	}
}