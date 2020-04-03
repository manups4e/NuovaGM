using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.Interactions
{
	class PickupsServer
	{
		public static List<OggettoRaccoglibile> Pickups = new List<OggettoRaccoglibile>();
		public static void Init()									  
		{
			Server.Instance.RegisterEventHandler("lprp:removeInventoryItemWithPickup", new Action<Player, string, int>(RemoveInventoryItemWithPickup));
			Server.Instance.RegisterEventHandler("lprp:removeWeaponWithPickup", new Action<Player, string>(RemoveWeaponWithPickup));
			Server.Instance.RegisterEventHandler("lprp:removeAccountWithPickup", new Action<Player, string, int>(RemoveAccountWithPickup));
			Server.Instance.RegisterEventHandler("lprp:onPickup", new Action<Player, int>(OnPickup));
		}

		public static void CreatePickupItem(Inventory oggetto, int count, string label, User user)
		{
			OggettoRaccoglibile pickup = new OggettoRaccoglibile(Pickups.Count, oggetto.item, count, SharedScript.ItemList[oggetto.item].prop, 0, label, user.getCoords.ToArray());
			Pickups.Add(pickup);
			BaseScript.TriggerClientEvent("lprp:createPickupInventory", JsonConvert.SerializeObject(pickup), user.p.Handle);
		}

		public static void CreatePickupWeapon(Weapons oggetto, string label, User user)
		{
			OggettoArmaRaccoglibile arma = new OggettoArmaRaccoglibile(Pickups.Count, oggetto.name, 1, (ObjectHash)0, 0, label, user.getCoords.ToArray(), oggetto.components, oggetto.tint, oggetto.ammo);
			Pickups.Add(arma);
			BaseScript.TriggerClientEvent("lprp:createPickupWeapon", JsonConvert.SerializeObject(arma), user.p.Handle);
		}

		public static void CreatePickupAccount(string name, int count, string label, User user)
		{
			ObjectHash oggetto = 0;
			switch (count) {
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
			OggettoRaccoglibile soldo = new OggettoRaccoglibile(Pickups.Count, name, count, oggetto, 0, label, user.getCoords.ToArray(), "account");
			Pickups.Add(soldo);
			BaseScript.TriggerClientEvent("lprp:createPickupAccount", JsonConvert.SerializeObject(soldo), user.p.Handle);
		}

		private static void RemoveInventoryItemWithPickup([FromSource] Player player, string item, int count)
		{
			User user = Server.PlayerList[player.Handle];
			Tuple<bool, Inventory> oggetto = user.getInventoryItem(item);
			if (oggetto.Item1)
			{
				if (oggetto.Item2.amount > 0)
				{
					user.removeInventoryItem(item, count);
					string label = $"{oggetto.Item2.item} [{count}]";
					CreatePickupItem(oggetto.Item2, count, label, user);
				}
				else
					user.showNotification("Non hai oggetti come questo nell'inventario!");
			}
		}

		private static void RemoveWeaponWithPickup([FromSource] Player player, string weapon)
		{
			User user = Server.PlayerList[player.Handle];
			if (user.hasWeapon(weapon))
			{
				Tuple<int, Weapons> arma = user.getWeapon(weapon);
				user.removeWeapon(weapon);
				string label = Funzioni.GetWeaponLabel((uint)GetHashKey(weapon));
				CreatePickupWeapon(arma.Item2, label, user);
			}
		}

		private static void RemoveAccountWithPickup([FromSource] Player player, string name, int amount)
		{
			string label = "";
			User user = Server.PlayerList[player.Handle];
			switch (name) {
				case "soldi":
					user.Money -= amount;
					label = $"Soldi contanti [{amount}]";
					break;
				case "soldi_sporchi":
					user.DirtyMoney -= amount;
					label = $"Soldi sporchi [{amount}]";
					break;
			}
			CreatePickupAccount(name, amount, label, user);
		}

		private static void OnPickup([FromSource] Player source, int id)
		{
			User user = Server.PlayerList[source.Handle];
			var pickup = Pickups[id];
			bool success = false;
			if (pickup.type == "item")
			{
				//aggiungere controllo se può portarlo
				user.addInventoryItem(pickup.name, pickup.amount, SharedScript.ItemList[pickup.name].peso);
				success = true;
			}
			else if (pickup.type == "weapon")
			{
				var arma = pickup as OggettoArmaRaccoglibile;
				if (user.hasWeapon(arma.name))
					user.showNotification("Hai già quest'arma!!!");
				else
				{
					success = true;
					user.addWeapon(arma.name, arma.ammo);
					if(arma.tintIndex != 0)
						user.addWeaponTint(arma.name, arma.tintIndex);
					foreach (var comp in arma.componenti)
						user.addWeaponComponent(arma.name, comp.name);

				}
			}
			else if (pickup.type == "account")
			{
				success = true;
				if (pickup.name == "soldi")
					user.Money += pickup.amount;
				else if (pickup.name == "soldi_sporchi")
					user.DirtyMoney += pickup.amount;
			}

			if (success)
			{
				Pickups[id] = null;
				BaseScript.TriggerClientEvent("lprp:removePickup", id);
			}
		}
	}
}
