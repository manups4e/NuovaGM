﻿using CitizenFX.Core;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using Impostazioni.Shared.Configurazione.Generici;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.Interactions
{
	internal class PickupsServer
	{
		public static List<OggettoRaccoglibile> Pickups = new List<OggettoRaccoglibile>();

		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:removeInventoryItemWithPickup", new Action<Player, string, int>(RemoveInventoryItemWithPickup));
			Server.Instance.AddEventHandler("lprp:removeWeaponWithPickup", new Action<Player, string>(RemoveWeaponWithPickup));
			Server.Instance.AddEventHandler("lprp:removeAccountWithPickup", new Action<Player, string, int>(RemoveAccountWithPickup));
			Server.Instance.AddEventHandler("lprp:onPickup", new Action<Player, int>(OnPickup));
		}

		public static void CreatePickup(Inventory oggetto, int count, string label, ClientId user)
		{
			OggettoRaccoglibile pickup = new OggettoRaccoglibile(Pickups.Count, oggetto.Item, count, ConfigShared.SharedConfig.Main.Generici.ItemList[oggetto.Item].prop, 0, label, user.Ped.Position.ToPosition());
			Pickups.Add(pickup);
			BaseScript.TriggerClientEvent("lprp:createPickup", pickup.ToJson(), user.Player.Handle);
		}

		public static void CreatePickup(Weapons oggetto, string label, ClientId user)
		{
			OggettoRaccoglibile arma = new OggettoRaccoglibile(Pickups.Count, oggetto.name, oggetto.ammo, (ObjectHash)0, 0, label, user.Ped.Position.ToPosition(), "weapon", oggetto.components, oggetto.tint);
			Pickups.Add(arma);
			BaseScript.TriggerClientEvent("lprp:createPickup", arma.ToJson(), user.Player.Handle);
		}

		public static void CreatePickup(string name, int count, string label, ClientId user)
		{
			ObjectHash oggetto = 0;

			switch (count)
			{
				case int a when a < 101:
					oggetto = ObjectHash.prop_cash_pile_01;

					break;
				case int a when a > 100 && a < 501:
					oggetto = ObjectHash.prop_anim_cash_pile_02;

					break;
				case int a when a > 500 && a < 1001:
					oggetto = ObjectHash.prop_cash_case_01;

					break;
				case int a when a > 1000 && a < 3001:
					oggetto = ObjectHash.prop_cash_case_02;

					break;
				case int a when a > 3000:
					oggetto = ObjectHash.prop_cash_dep_bag_01;

					break;
			}

			OggettoRaccoglibile soldo = new OggettoRaccoglibile(Pickups.Count, name, count, oggetto, 0, label, user.Ped.Position.ToPosition(), "account");
			Pickups.Add(soldo);
			BaseScript.TriggerClientEvent("lprp:createPickup", soldo.ToJson(), user.Player.Handle);
		}

		private static void RemoveInventoryItemWithPickup([FromSource] Player player, string item, int count)
		{
			ClientId client = Funzioni.GetClientFromPlayerId(player.Handle);
			User user = client?.User ?? player.GetCurrentChar();
			Tuple<bool, Inventory> oggetto = user.getInventoryItem(item);

			if (oggetto.Item1)
			{
				if (oggetto.Item2.Amount > 0)
				{
					user.removeInventoryItem(item, count);
					string label = $"{oggetto.Item2.Item} [{count}]";
					CreatePickup(oggetto.Item2, count, label, client);
				}
				else
				{
					user.showNotification("Non hai oggetti come questo nell'inventario!");
				}
			}
		}

		private static void RemoveWeaponWithPickup([FromSource] Player player, string weapon)
		{
			ClientId client = Funzioni.GetClientFromPlayerId(player.Handle);
			User user = client?.User ?? player.GetCurrentChar();

			if (user.hasWeapon(weapon))
			{
				Tuple<int, Weapons> arma = user.getWeapon(weapon);
				user.removeWeapon(weapon);
				string label = Funzioni.GetWeaponLabel((uint)GetHashKey(weapon));
				CreatePickup(arma.Item2, label, client);
			}
		}

		private static void RemoveAccountWithPickup([FromSource] Player player, string name, int amount)
		{
			string label = "";
			ClientId client = Funzioni.GetClientFromPlayerId(player.Handle);
			User user = client?.User ?? player.GetCurrentChar();

			switch (name)
			{
				case "soldi":
					user.Money -= amount;
					label = $"Soldi contanti [{amount}]";

					break;
				case "soldi_sporchi":
					user.DirtCash -= amount;
					label = $"Soldi sporchi [{amount}]";

					break;
			}

			CreatePickup(name, amount, label, client);
		}

		private static void OnPickup([FromSource] Player source, int id)
		{
			User user = source.GetCurrentChar();
			OggettoRaccoglibile pickup = Pickups[id];
			bool success = false;

			switch (pickup.type)
			{
				case "item":
					//aggiungere controllo se può portarlo
					user.addInventoryItem(pickup.name, pickup.amount, ConfigShared.SharedConfig.Main.Generici.ItemList[pickup.name].peso);
					success = true;

					break;
				case "weapon":
					if (user.hasWeapon(pickup.name))
					{
						user.showNotification("Hai già quest'arma!");
					}
					else
					{
						success = true;
						user.addWeapon(pickup.name, pickup.amount);
						if (pickup.tintIndex != 0) user.addWeaponTint(pickup.name, pickup.tintIndex);
						foreach (Components comp in pickup.componenti) user.addWeaponComponent(pickup.name, comp.name);
					}

					break;
				case "account":
					success = true;
					if (pickup.name == "soldi")
						user.Money += pickup.amount;
					else if (pickup.name == "soldi_sporchi") user.DirtCash += pickup.amount;

					break;
			}

			if (success)
			{
				Pickups[id] = null;
				BaseScript.TriggerClientEvent("lprp:removePickup", id);
			}
		}
	}
}