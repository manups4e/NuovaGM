using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Impostazioni.Shared.Configurazione.Generici;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server.Interactions;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.RolePlay.Core
{
	public static class EventiRolePlay
	{
		public static void Init()
		{
			Server.Instance.Events.Mount("tlg:roleplay:finishCharServer", new Action<ClientId, string>(FinishChar));
			Server.Instance.Events.Mount("tlg:roleplay:onPlayerSpawn", new Action<ClientId>(Spawnato));
			Server.Instance.Events.Mount("lprp:setDeathStatus", new Action<ClientId, bool>(deathStatus));
			Server.Instance.Events.Mount("lprp:payFine", new Action<ClientId, int>(PayFine));
			Server.Instance.Events.Mount("lprp:givemoney", new Action<ClientId, int>(GiveMoney));
			Server.Instance.Events.Mount("lprp:removemoney", new Action<ClientId, int>(RemoveMoney));
			Server.Instance.Events.Mount("lprp:givebank", new Action<ClientId, int>(GiveBank));
			Server.Instance.Events.Mount("lprp:removebank", new Action<ClientId, int>(RemoveBank));
			Server.Instance.Events.Mount("lprp:removedirty", new Action<ClientId, int>(RemoveDirty));
			Server.Instance.Events.Mount("lprp:givedirty", new Action<ClientId, int>(GiveDirty));
			Server.Instance.Events.Mount("lprp:addIntenvoryItem",
				new Action<ClientId, string, int, float>(AddInventory));
			Server.Instance.Events.Mount("lprp:removeIntenvoryItem",
				new Action<ClientId, string, int>(RemoveInventory));
			Server.Instance.Events.Mount("lprp:addWeapon", new Action<ClientId, string, int>(AddWeapon));
			Server.Instance.Events.Mount("lprp:removeWeapon", new Action<ClientId, string>(RemoveWeapon));
			Server.Instance.Events.Mount("lprp:addWeaponComponent",
				new Action<ClientId, string, string>(AddWeaponComp));
			Server.Instance.Events.Mount("lprp:removeWeaponComponent",
				new Action<ClientId, string, string>(RemoveWeaponComp));
			Server.Instance.Events.Mount("lprp:addWeaponTint", new Action<ClientId, string, int>(AddWeaponTint));
			Server.Instance.Events.Mount("lprp:removeItemsDeath", new Action<ClientId>(removeItemsDeath));
			Server.Instance.Events.Mount("lprp:salvaPlayer", new Action<ClientId>(SalvaPlayer));
			Server.Instance.Events.Mount("lprp:givemoneytochar", new Action<string, int, int>(GiveMoneyToChar));
			Server.Instance.Events.Mount("lprp:removemoneytochar", new Action<string, int, int>(RemoveMoneyToChar));
			Server.Instance.Events.Mount("lprp:givebanktochar", new Action<string, int, int>(GiveBankToChar));
			Server.Instance.Events.Mount("lprp:removebanktochar", new Action<string, int, int>(RemoveBankToChar));
			Server.Instance.Events.Mount("lprp:givedirtytochar", new Action<string, int, int>(GiveDirtyToChar));
			Server.Instance.Events.Mount("lprp:removedirtytochar", new Action<string, int, int>(RemoveDirtyToChar));
			Server.Instance.Events.Mount("lprp:givedirtytochar", new Action<string, int, int>(GiveDirtyToChar));
			Server.Instance.Events.Mount("lprp:addIntenvoryItemtochar",
				new Action<string, int, string, int, float>(AddInventoryToChar));
			Server.Instance.Events.Mount("lprp:removeIntenvoryItemtochar",
				new Action<string, int, string, int>(RemoveInventoryToChar));
			Server.Instance.Events.Mount("lprp:addWeapontochar", new Action<string, int, string, int>(AddWeaponToChar));
			Server.Instance.Events.Mount("lprp:removeWeapontochar",
				new Action<string, int, string>(RemoveWeaponToChar));
			Server.Instance.Events.Mount("lprp:addWeaponComponenttochar",
				new Action<string, int, string, string>(AddWeaponCompToChar));
			Server.Instance.Events.Mount("lprp:removeWeaponComponenttochar",
				new Action<string, int, string, string>(RemoveWeaponCompToChar));
			Server.Instance.Events.Mount("lprp:addWeaponTinttochar",
				new Action<string, int, string, int>(AddWeaponTintToChar));
			Server.Instance.Events.Mount("lprp:giveLicense", new Action<ClientId, string>(GiveLicense));
			Server.Instance.Events.Mount("lprp:giveLicenseToChar",
				new Action<ClientId, int, string>(GiveLicenseToChar));
			Server.Instance.Events.Mount("lprp:removeLicense", new Action<ClientId, string>(RemoveLicense));
			Server.Instance.Events.Mount("lprp:removeLicenseToChar",
				new Action<ClientId, int, string>(RemoveLicenseToChar));
			Server.Instance.Events.Mount("lprp:updateWeaponAmmo", new Action<ClientId, string, int>(AggiornaAmmo));
			Server.Instance.Events.Mount("lprp:giveInventoryItemToPlayer",
				new Action<ClientId, int, string, int>(GiveItemToOtherPlayer));
			Server.Instance.Events.Mount("lprp:giveWeaponToPlayer",
				new Action<ClientId, int, string, int>(GiveWeaponToOtherPlayer));
			Server.Instance.Events.Mount("lprp:callDBPlayers", new Func<ClientId, Task<Dictionary<string, User>>>(async (a) =>
				(await MySQL.QueryListAsync<User>("select * from users")).ToDictionary(p => a.Player.Handle)));
		}

		public static void FinishChar(ClientId client, string data)
		{
			try
			{
				Char_data Char = data.FromJson<Char_data>();
				client.User.Characters.Add(Char);
			}
			catch (Exception e)
			{
				Server.Logger.Error($"{e.Message}");
			}
		}

		public static void deathStatus(ClientId source, bool value)
		{
			source.User.DeathStatus = value;
			source.User.Status.RolePlayStates.FinDiVita = value;
		}

		public static void PayFine(ClientId source, int amount)
		{
			User player = source.User;

			if (amount == 5000)
				player.Money -= 5000;
			else
			{
				DateTime now = DateTime.Now;
				Server.Logger.Warning($"Il player {source.Player.Name} ha usato un lua executor / CheatEngine per modificare il valore da pagare alla morte!");
				source.Player.Drop("LastPlanet Shield [Sospetto Modding]: entra in discord a chiarire perfavore!");
				// AGGIUNGERE AVVISO DISCORD! ;)
			}
		}

		public static void Spawnato(ClientId source)
		{
			User user = source.User;
			Server.Logger.Info($"{user.FullName} ({source.Player.Name} è entrato in città");
			foreach (var client in from ClientId client in BucketsHandler.RolePlay.Bucket.Players where client.Handle != source.Handle select client)
				client.Player.TriggerEvent("lprp:ShowNotification","~g~" + user.FullName + " (" + source.Player.Name + ")~w~ è entrato in città");
			source.Player.TriggerEvent("lprp:createMissingPickups", PickupsServer.Pickups.ToJson());
			user.Status.Spawned = true;
		}

		//TODO: DA CAMBIARE CON NUOVO METODO
		public static async void SalvaPlayer(ClientId client)
		{
			await BaseScript.Delay(0);
			string name = client.Player.Name;

			User user = client.User;

			if (user.Status.Spawned)
			{
				client.Player.TriggerEvent("lprp:mostrasalvataggio");
				BucketsHandler.RolePlay.SalvaPersonaggioRoleplay(client);
				Server.Logger.Info("Salvato personaggio: '" + user.FullName + "' appartenente a '" + name +"' tramite telefono");
			}
			await Task.FromResult(0);
		}

		public static void removeItemsDeath(ClientId source)
		{
			User player = source.User;
			int money = player.Money;
			int dirty = player.DirtCash;
			foreach (Inventory inv in player.CurrentChar.Inventory.ToList())
				player.removeInventoryItem(inv.Item, inv.Amount);
			foreach (Weapons inv in player.CurrentChar.Weapons.ToList()) player.removeWeapon(inv.name);
			player.Money -= money;
			player.DirtCash -= dirty;
		}

		public static void GiveMoney(ClientId source, int amount)
		{
			User player = source.User;
			player.Money += amount;
		}

		public static void RemoveMoney(ClientId source, int amount)
		{
			if (amount > 0)
				source.User.Money -= amount;
			else
				source.Player.Drop("Rilevata possibile modifica ai valori di gioco");
		}

		public static void GiveBank(ClientId source, int amount)
		{
			source.User.Bank += amount;
		}

		public static void RemoveBank(ClientId source, int amount)
		{
			source.User.Bank -= amount;
		}

		public static void GiveDirty(ClientId source, int amount)
		{
			source.User.DirtCash += amount;
		}

		public static void RemoveDirty(ClientId source, int amount)
		{
			source.User.DirtCash -= amount;
		}

		public static void AddInventory(ClientId source, string item, int amount, float peso)
		{
			source.User.addInventoryItem(item, amount,peso > 0 ? peso : ConfigShared.SharedConfig.Main.Generici.ItemList[item].peso);
		}

		public static void RemoveInventory(ClientId source, string item, int amount)
		{
			source.User.removeInventoryItem(item, amount);
		}

		public static void AddWeapon(ClientId source, string weaponName, int ammo)
		{
			source.User.addWeapon(weaponName, ammo);
		}

		public static void RemoveWeapon(ClientId source, string weaponName)
		{
			source.User.removeWeapon(weaponName);
		}

		public static void AddWeaponComp(ClientId source, string weaponName, string weaponComponent)
		{
			source.User.addWeaponComponent(weaponName, weaponComponent);
		}

		public static void RemoveWeaponComp(ClientId source, string weaponName, string weaponComponent)
		{
			source.User.removeWeaponComponent(weaponName, weaponComponent);
		}

		public static void AddWeaponTint(ClientId source, string weaponName, int tint)
		{
			source.User.addWeaponTint(weaponName, tint);
		}

		public static void GiveMoneyToChar(string target, int charId, int amount)
		{
			if (amount < 1) return;
			User player = Funzioni.GetUserFromPlayerId(target);
			player.Money += amount;
		}

		public static void RemoveMoneyToChar(string target, int charId, int amount)
		{
			if (amount < 1) return;
			User player = Funzioni.GetUserFromPlayerId(target);
			player.Money -= amount;
		}

		public static void GiveBankToChar(string target, int charId, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(target);
			player.Bank += amount;
		}

		public static void RemoveBankToChar(string target, int charId, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(target);
			player.Bank -= amount;
		}

		public static void GiveDirtyToChar(string target, int charId, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(target);
			player.DirtCash += amount;
		}

		public static void RemoveDirtyToChar(string target, int charId, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(target);
			player.DirtCash -= amount;
		}

		public static void AddInventoryToChar(string target, int charId, string item, int amount, float peso)
		{
			Funzioni.GetUserFromPlayerId(target).addInventoryItem(item, amount,
				peso > 0 ? peso : ConfigShared.SharedConfig.Main.Generici.ItemList[item].peso);
		}

		public static void RemoveInventoryToChar(string target, int charId, string item, int amount)
		{
			Funzioni.GetUserFromPlayerId(target).removeInventoryItem(item, amount);
		}

		public static void AddWeaponToChar(string target, int charId, string weaponName, int ammo)
		{
			Funzioni.GetUserFromPlayerId(target).addWeapon(weaponName, ammo);
		}

		public static void RemoveWeaponToChar(string target, int charId, string weaponName)
		{
			Funzioni.GetUserFromPlayerId(target).removeWeapon(weaponName);
		}

		public static void AddWeaponCompToChar(string target, int charId, string weaponName, string weaponComponent)
		{
			Funzioni.GetUserFromPlayerId(target).addWeaponComponent(weaponName, weaponComponent);
		}

		public static void RemoveWeaponCompToChar(string target, int charId, string weaponName, string weaponComponent)
		{
			Funzioni.GetUserFromPlayerId(target).removeWeaponComponent(weaponName, weaponComponent);
		}

		public static void AddWeaponTintToChar(string target, int charId, string weaponName, int tint)
		{
			Funzioni.GetUserFromPlayerId(target).addWeaponTint(weaponName, tint);
		}
		
		private static void GiveLicense(ClientId source, string license)
		{
			User player = source.User;
		}

		private static void GiveLicenseToChar(ClientId source, int target, string license)
		{
			User player = source.User;
		}

		private static void RemoveLicense(ClientId source, string license)
		{
			User player = source.User;
		}

		private static void RemoveLicenseToChar(ClientId source, int target, string license)
		{
			User player = source.User;
		}

		private static void AggiornaAmmo(ClientId source, string weaponName, int ammo)
		{
			source.User.updateWeaponAmmo(weaponName, ammo);
		}

		private static void GiveItemToOtherPlayer(ClientId source, int target, string itemName, int amount)
		{
			User player = source.User;
			ClientId targetClient = Funzioni.GetClientFromPlayerId(target);
			User targetPlayer = targetClient.User;
			player.removeInventoryItem(itemName, amount);
			player.showNotification($"Hai dato {amount} di {ConfigShared.SharedConfig.Main.Generici.ItemList[itemName].label} a {targetPlayer.FullName}");
			targetPlayer.addInventoryItem(itemName, amount, ConfigShared.SharedConfig.Main.Generici.ItemList[itemName].peso);
			targetClient.TriggerSubsystemEvent("lprp:riceviOggettoAnimazione");
			targetPlayer.showNotification($"Hai ricevuto {amount} di {ConfigShared.SharedConfig.Main.Generici.ItemList[itemName].label} da {player.FullName}");
		}

		private static void GiveWeaponToOtherPlayer(ClientId source, int target, string weaponName, int ammo)
		{
			User player = source.User;
			ClientId targetClient = Funzioni.GetClientFromPlayerId(target);
			User targetPlayer = targetClient.User;
			Tuple<int, Weapons> weapon = player.getWeapon(weaponName);
			Weapons arma = weapon.Item2;

			if (targetPlayer.hasWeapon(weaponName))
			{
				player.showNotification($"{player.FullName} ha già quest'arma!");
			}
			else
			{
				player.removeWeapon(weaponName);
				player.showNotification($"Hai dato la tua arma a {targetPlayer.FullName}");
				targetPlayer.addWeapon(weaponName, ammo);
				foreach (Components comp in arma.components) targetPlayer.addWeaponComponent(weaponName, comp.name);
				if (arma.tint != 0) targetPlayer.addWeaponTint(weaponName, arma.tint);
				targetClient.TriggerSubsystemEvent("lprp:riceviOggettoAnimazione");
				targetPlayer.showNotification($"Hai ricevuto un'arma con {ammo} munizioni da {player.FullName}");
			}
		}
	}
}