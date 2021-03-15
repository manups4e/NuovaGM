using CitizenFX.Core;
using Logger;
using TheLastPlanet.Server.Core;
using System;
using System.Linq;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Shared.SistemaEventi;
using System.Collections.Generic;
using TheLastPlanet.Server.Core.PlayerChar;

namespace TheLastPlanet.Server.banking
{
	internal static class BankingServer
	{
		public static void Init()
		{
			ServerSession.Instance.SistemaEventi.Attach("lprp:banking:sendMoney", new EventCallback(meta => 
			{
				var player = Funzioni.GetPlayerFromId(meta.Sender);
				User user = player.GetCurrentChar();
				string name = meta.Find<string>(0);
				int amount = meta.Find<int>(1);
				if (user.Bank >= amount)
				{
					foreach (var p in ServerSession.PlayerList)
					{
						if (user.FullName.ToLower() == name.ToLower())
						{
							user.Bank -= amount;
							p.Value.Bank += amount;
							Log.Printa(LogType.Info, $"Il personaggio '{user.FullName}' [{player.Name}] ha inviato ${amount} a '{p.Value.FullName}' [{p.Value.Player.Name}]");
							return new KeyValuePair<bool, string>(true, user.Bank.ToString());
						}
					}
					return new KeyValuePair<bool, string>(false, "Utente non trovato");
				}
				return new KeyValuePair<bool, string>(false, "I tuoi fondi bancari non coprono la transazione!");
			}));
			ServerSession.Instance.SistemaEventi.Attach("lprp:banking:atmwithdraw", new EventCallback(meta =>
			{
				var player = Funzioni.GetPlayerFromId(meta.Sender);
				int amount = meta.Find<int>(0);
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
						return new KeyValuePair<bool, string> (true, newamt.ToString());
					}
					return new KeyValuePair<bool, string>(false, "Non hai abbastanza fondi nel tuo conto corrente per questa transazione.");
				}
				return new KeyValuePair<bool, string> (false, "Devi inserire un valore positivo.");

			}));
			ServerSession.Instance.SistemaEventi.Attach("lprp:banking:atmdeposit", new EventCallback(meta =>
			{
				var player = Funzioni.GetPlayerFromId(meta.Sender);
				int amount = meta.Find<int>(0);
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
					return new KeyValuePair<bool, string> (false, "Non hai abbastanza soldi nel tuo portafoglio per questa transazione.");
				}
				return new KeyValuePair<bool, string> (false, "Devi inserire un valore positivo.");

			}));
		}
	}
}