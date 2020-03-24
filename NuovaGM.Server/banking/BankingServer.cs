using CitizenFX.Core;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;
using System;
using System.Linq;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.banking
{
	static class BankingServer
	{
		public static void Init()
		{
			Server.Instance.RegisterEventHandler("lprp:banking:sendMoney", new Action<Player, string, int>(SendMoney));
			Server.Instance.RegisterEventHandler("lprp:banking:atmwithdraw", new Action<Player, int>(Ritira));
			Server.Instance.RegisterEventHandler("lprp:banking:atmdeposit", new Action<Player, int>(Deposita));
		}

		public static void SendMoney([FromSource]Player player, string name, int amount)
		{
			User user = ServerEntrance.PlayerList[player.Handle];
			if (user.Bank >= amount)
			{
				foreach (Player p in Server.Instance.GetPlayers.ToList())
				{
					if (ServerEntrance.PlayerList[p.Handle].FullName.ToLower() == name.ToLower())
					{
						user.Bank -= amount;
						ServerEntrance.PlayerList[p.Handle].Bank += amount;
						BaseScript.TriggerClientEvent(p, "lprp:banking:transactionstatus", true, user.Bank.ToString());
						BaseScript.TriggerEvent("lprp:serverLog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $" -- Il personaggio '{user.FullName}' [{GetPlayerName(player.Handle)}] ha inviato ${amount} a '{ServerEntrance.PlayerList[p.Handle].FullName}' [{GetPlayerName(p.Handle)}]");
					}
					else
					{
						BaseScript.TriggerClientEvent(p, "lprp:banking:transactionstatus", false, "Persona non trovata!");
					}
				}
			}
			else
			{
				BaseScript.TriggerClientEvent(player, "lprp:banking:transactionstatus", false, "I tuoi fondi bancari non coprono la transazione!");
			}
		}

		public static void Ritira([FromSource]Player p, int amount)
		{
			if (amount > 0)
			{
				User user = ServerEntrance.PlayerList[p.Handle];
				int bal = user.Bank;
				int newamt = bal - amount;
				Debug.WriteLine("bal = " + bal);
				Debug.WriteLine("newamt = " + newamt);
				if (bal >= amount)
				{
					user.Money += amount;
					Server.Printa(LogType.Info, $"Il personaggio '{user.FullName}' [{GetPlayerName(p.Handle)}] ha depositato {amount}$");
					BaseScript.TriggerEvent("lprp:serverLog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $" -- Il personaggio '{user.FullName}' [{GetPlayerName(p.Handle)}] ha depositato {amount}$");
					user.Bank -= amount;
					BaseScript.TriggerClientEvent(p, "lprp:banking:transactionstatus", true, newamt.ToString());
				}
				else
				{
					BaseScript.TriggerClientEvent(p, "lprp:banking:transactionstatus", false, "Non hai abbastanza fondi nel tuo conto corrente per questa transazione.");
				}
			}
			else
			{
				BaseScript.TriggerClientEvent(p, "lprp:banking:transactionstatus", false, "Devi inserire un valore positivo.");
			}
		}
		public static void Deposita([FromSource]Player p, int amount)
		{
			if (amount > 0)
			{
				User user = ServerEntrance.PlayerList[p.Handle];
				int money = user.Money;
				int bankmoney = user.Bank;
				int newamt = bankmoney + amount;
				if (amount <= money)
				{
					user.Money -= amount;
					Server.Printa(LogType.Info, $"Il personaggio '{user.FullName}' [{GetPlayerName(p.Handle)}] ha depositato {amount}$");
					BaseScript.TriggerEvent("lprp:serverLog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $" -- Il personaggio '{user.FullName}' [{GetPlayerName(p.Handle)}] ha depositato {amount}$");
					user.Bank += amount;
					BaseScript.TriggerClientEvent(p, "lprp:banking:transactionstatus", true, newamt.ToString());
				}
				else
				{
					BaseScript.TriggerClientEvent(p, "lprp:banking:transactionstatus", false, "Non hai abbastanza soldi nel tuo portafoglio per questa transazione.");
				}
			}
			else
			{
				BaseScript.TriggerClientEvent(p, "lprp:banking:transactionstatus", false, "Devi inserire un valore positivo.");
			}
		}

	}
}
