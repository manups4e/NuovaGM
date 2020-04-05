using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.Veicoli
{
	static class FuelServer
	{
		public static void Init()
		{
			Server.Instance.RegisterEventHandler("lprp:fuel:payForFuel", new Action<Player, int, float, float>(PayForFuel));
			Server.Instance.RegisterEventHandler("lprp:fuel:buytanker", new Action<Player, string>(BuyTanker));
			Server.Instance.RegisterEventHandler("lprp:fuel:buyfuelfortanker", new Action<Player, int, int>(BuyFuelForTanker));
			Server.Instance.RegisterEventHandler("lprp:getDecor", new Action<Player>(RespondDecor));
		}

		public static void RespondDecor([FromSource]Player p)
		{
			BaseScript.TriggerClientEvent(p, "lprp:fuel:setFuelDecor", "NuovaGMRPFUEL20192020!!yeah!?#@", 65.0f);
		}

		public static async void PayForFuel([FromSource] Player p, int stationindex, float addedfuel, float fuelval)
		{
			User player = Server.PlayerList[p.Handle];
			int sidx = stationindex;
			float fuelCost;
			dynamic result = await Server.Instance.Query($"SELECT `fuelprice` FROM `businesses` WHERE `stationindex` = @idx", new { idx = sidx });
			int money = player.Money;
			int bank = player.Bank;
			fuelCost = result[0].fuelprice;
			int price = (int)Math.Ceiling(addedfuel * fuelCost);
			if (money >= price)
			{
				player.Money -= price;
				Server.Printa(LogType.Info, "Il personaggio " + player.FullName + " del player " + GetPlayerName(player.source) + " ha pagato " + price + "$ per fare carburante.");
				BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio " + player.FullName + " del player " + GetPlayerName(player.source) + " ha pagato " + price + "$ per fare carburante.");
				p.TriggerEvent("lprp:fuel:addfueltovehicle", true, "Hai pagato ~b~$ " + price + "~w~ per fare carburante.", fuelval);
				BaseScript.TriggerEvent("lprp:businesses:addmoneytostation", sidx, price);
				BaseScript.TriggerEvent("lprp:businesses:removefuelfromstation", stationindex, addedfuel);
			}
			else
			{
				if (bank >= price)
				{
					player.Bank -= price;
					Server.Printa(LogType.Info, "Il personaggio " + player.FullName + " del player " + GetPlayerName(player.source) + " ha pagato " + price + "$ per fare carburante.");
					BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio " + player.FullName + " del player " + GetPlayerName(player.source) + " ha pagato " + price + "$ per fare carburante.");
					BaseScript.TriggerClientEvent(p, "lprp:fuel:addfueltovehicle", true, "Hai pagato ~b~$ " + price + "~w~ per fare carburante.", fuelval);
					BaseScript.TriggerEvent("lprp:businesses:addmoneytostation", sidx, price);
					BaseScript.TriggerEvent("lprp:businesses:removefuelfromstation", stationindex, addedfuel);
				}
				else
					BaseScript.TriggerClientEvent(p, "lprp:fuel:addfueltovehicle", false, "Non hai abbastanza soldi nel portafoglio o in banca per fare carburante.");
			}
		}

		public static void BuyTanker([FromSource] Player p, string Json)
		{
			User user = Server.PlayerList[p.Handle];
			Tanker t = JsonConvert.DeserializeObject<Tanker>(Json);
			int amount = (int)Math.Ceiling(t.ppu * t.fuelForTanker);
			if (user.Money >= amount)
			{
				user.Money -= amount;
				Server.Printa(LogType.Info, "Il personaggio {user.FullName} [{GetPlayerName(p.Handle)}] ha pagato {amount}$ per una cisterna di carburante.");
				BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $" -- Il personaggio {user.FullName} [{GetPlayerName(p.Handle)}] ha pagato {amount}$ per una cisterna di carburante.");
				BaseScript.TriggerClientEvent(p, "lprp:fuel:buytanker", true, JsonConvert.SerializeObject(t));
			}
			else
			{
				BaseScript.TriggerClientEvent(p, "lprp:fuel:buytanker", false, "Non puoi permetterti di comprare la tanica di benzina.");
			}
		}

		public static void BuyFuelForTanker([FromSource] Player p, int cost, int fuel)
		{
			User user = Server.PlayerList[p.Handle];
			if (user.Money >= cost)
			{
				user.Money -= cost;
				Server.Printa(LogType.Info, $"Il personaggio {user.FullName} [{GetPlayerName(p.Handle)}] ha pagato {cost}$ per riempire tutta la cisterna di carburante.");
				BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"--Il personaggio {user.FullName} [{GetPlayerName(p.Handle)}] ha pagato {cost}$ per riempire tutta la cisterna di carburante.");
				BaseScript.TriggerClientEvent(p, "lprp:fuel:buyfuelfortanker", true, fuel.ToString());
			}
			else
			{
				BaseScript.TriggerClientEvent(p, "lprp:fuel:buyfuelfortanker", false, "Non hai abbastanza soldi per questo acquisto.");
			}
		}

	}

	public class Tanker
	{
		public Vector3 pos;
		public float heading;
		public Vector3 t;
		public float theading;
		public float ppu;
		public int fuelForTanker;
		public Tanker(Vector3 p, float h, Vector3 t, float th, float ppu, int fuel)
		{
			pos = p;
			heading = h;
			this.t = t;
			theading = th;
			this.ppu = ppu;
			fuelForTanker = fuel;
		}
	}
}
