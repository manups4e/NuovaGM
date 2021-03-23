using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Server.Interactions;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Impostazioni.Shared.Configurazione.Generici;
using TheLastPlanet.Shared.SistemaEventi;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Server.Core.PlayerChar;

namespace TheLastPlanet.Server.Core
{
	internal static class Eventi
	{
		private static int EarlyRespawnFineAmount = 5000;

		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:finishCharServer", new Action<Player, string>(FinishChar));
			Server.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action<Player>(Spawnato));
			Server.Instance.AddEventHandler("lprp:setDeathStatus", new Action<Player, bool>(deathStatus));
			Server.Instance.AddEventHandler("playerDropped", new Action<Player, string>(Dropped));
			Server.Instance.AddEventHandler("lprp:dropPlayer", new Action<Player, string>(Drop));
			Server.Instance.AddEventHandler("lprp:kickPlayer", new Action<string, string, int>(Kick));
			//ServerSession.Instance.AddEventHandler("lprp:updateCurChar", new Action<Player, string, dynamic, float>(UpdateChar));
			Server.Instance.AddEventHandler("lprp:CheckPing", new Action<Player>(Ping));
			Server.Instance.AddEventHandler("lprp:checkAFK", new Action<Player>(AFK));
			Server.Instance.AddEventHandler("lprp:payFine", new Action<Player, int>(PayFine));
			Server.Instance.AddEventHandler("lprp:givemoney", new Action<Player, int>(GiveMoney));
			Server.Instance.AddEventHandler("lprp:removemoney", new Action<Player, int>(RemoveMoney));
			Server.Instance.AddEventHandler("lprp:givebank", new Action<Player, int>(GiveBank));
			Server.Instance.AddEventHandler("lprp:removebank", new Action<Player, int>(RemoveBank));
			Server.Instance.AddEventHandler("lprp:removedirty", new Action<Player, int>(RemoveDirty));
			Server.Instance.AddEventHandler("lprp:givedirty", new Action<Player, int>(GiveDirty));
			Server.Instance.AddEventHandler("lprp:addIntenvoryItem", new Action<Player, string, int, float>(AddInventory));
			Server.Instance.AddEventHandler("lprp:removeIntenvoryItem", new Action<Player, string, int>(RemoveInventory));
			Server.Instance.AddEventHandler("lprp:addWeapon", new Action<Player, string, int>(AddWeapon));
			Server.Instance.AddEventHandler("lprp:removeWeapon", new Action<Player, string>(RemoveWeapon));
			Server.Instance.AddEventHandler("lprp:addWeaponComponent", new Action<Player, string, string>(AddWeaponComp));
			Server.Instance.AddEventHandler("lprp:removeWeaponComponent", new Action<Player, string, string>(RemoveWeaponComp));
			Server.Instance.AddEventHandler("lprp:addWeaponTint", new Action<Player, string, int>(AddWeaponTint));
			Server.Instance.AddEventHandler("lprp:removeItemsDeath", new Action<Player>(removeItemsDeath));
			Server.Instance.AddEventHandler("lprp:salvaPlayer", new Action<Player>(SalvaPlayer));
			Server.Instance.AddEventHandler("lprp:givemoneytochar", new Action<string, int, int>(GiveMoneyToChar));
			Server.Instance.AddEventHandler("lprp:removemoneytochar", new Action<string, int, int>(RemoveMoneyToChar));
			Server.Instance.AddEventHandler("lprp:givebanktochar", new Action<string, int, int>(GiveBankToChar));
			Server.Instance.AddEventHandler("lprp:removebanktochar", new Action<string, int, int>(RemoveBankToChar));
			Server.Instance.AddEventHandler("lprp:givedirtytochar", new Action<string, int, int>(GiveDirtyToChar));
			Server.Instance.AddEventHandler("lprp:removedirtytochar", new Action<string, int, int>(RemoveDirtyToChar));
			Server.Instance.AddEventHandler("lprp:givedirtytochar", new Action<string, int, int>(GiveDirtyToChar));
			Server.Instance.AddEventHandler("lprp:addIntenvoryItemtochar", new Action<string, int, string, int, float>(AddInventoryToChar));
			Server.Instance.AddEventHandler("lprp:removeIntenvoryItemtochar", new Action<string, int, string, int>(RemoveInventoryToChar));
			Server.Instance.AddEventHandler("lprp:addWeapontochar", new Action<string, int, string, int>(AddWeaponToChar));
			Server.Instance.AddEventHandler("lprp:removeWeapontochar", new Action<string, int, string>(RemoveWeaponToChar));
			Server.Instance.AddEventHandler("lprp:addWeaponComponenttochar", new Action<string, int, string, string>(AddWeaponCompToChar));
			Server.Instance.AddEventHandler("lprp:removeWeaponComponenttochar", new Action<string, int, string, string>(RemoveWeaponCompToChar));
			Server.Instance.AddEventHandler("lprp:addWeaponTinttochar", new Action<string, int, string, int>(AddWeaponTintToChar));
			Server.Instance.AddEventHandler("lprp:bannaPlayer", new Action<string, string, bool, long, int>(BannaPlayer));
			Server.Instance.AddEventHandler("lprp:giveLicense", new Action<Player, string>(GiveLicense));
			Server.Instance.AddEventHandler("lprp:giveLicenseToChar", new Action<Player, int, string>(GiveLicenseToChar));
			Server.Instance.AddEventHandler("lprp:removeLicense", new Action<Player, string>(RemoveLicense));
			Server.Instance.AddEventHandler("lprp:removeLicenseToChar", new Action<Player, int, string>(RemoveLicenseToChar));
			Server.Instance.AddEventHandler("lprp:updateWeaponAmmo", new Action<Player, string, int>(AggiornaAmmo));
			Server.Instance.AddEventHandler("lprp:giveInventoryItemToPlayer", new Action<Player, int, string, int>(GiveItemToOtherPlayer));
			Server.Instance.AddEventHandler("lprp:giveWeaponToPlayer", new Action<Player, int, string, int>(GiveWeaponToOtherPlayer));
			Server.Instance.SistemaEventi.Attach("lprp:callPlayers", new AsyncEventCallback( async a => 
			{
				User user = Funzioni.GetUserFromPlayerId(a.Sender);
				var pos = a.Find<Position>(0);
				user.CurrentChar.Posizione = pos;
				TimeSpan time = (DateTime.Now - user.LastSaved);
				if (time.Minutes > 10)
				{
					BaseScript.TriggerClientEvent(user.Player, "lprp:mostrasalvataggio");
					await user.SalvaPersonaggio();
					Log.Printa(LogType.Info, "Salvato personaggio: '" + user.FullName + "' appartenente a '" + user.Player.Name + "' - " + user.identifiers.Discord);
				}
				return Server.PlayerList;
			}));
			Server.Instance.SistemaEventi.Attach("lprp:callDBPlayers", new AsyncEventCallback(async a => (await MySQL.QueryListAsync<User>("select * from users")).ToDictionary(p => p.Player.Handle)));
		}

		public static void FinishChar([FromSource] Player p, string data)
		{
			try
			{
				Char_data Char = data.FromJson<Char_data>();
				User user = Funzioni.GetUserFromPlayerId(p.Handle);
				user.Characters.Add(Char);
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, $"{e.Message}");
			}
		}

		public static void Drop([FromSource] Player player, string reason)
		{
			player.Drop(reason);
		}

		public static void Ping([FromSource] Player player)
		{
			if (player.Ping >= Server.Impostazioni.Main.PingMax) player.Drop("Ping troppo alto (Limite: " + Server.Impostazioni.Main.PingMax + ", tuo ping: " + player.Ping + ")");
		}

		public static void AFK([FromSource] Player p)
		{
			p.Drop("Last Planet Shield 2.0:\nSei stato rilevato per troppo tempo in AFK");
		}

		public static void deathStatus([FromSource] Player source, bool value)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).DeathStatus = value;
		}

		public static void PayFine([FromSource] Player source, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);

			if (amount == EarlyRespawnFineAmount)
			{
				player.Money -= EarlyRespawnFineAmount;
			}
			else
			{
				DateTime now = DateTime.Now;
				Log.Printa(LogType.Warning, "il player " + GetPlayerName(source.Handle) + " ha usato un lua executor / CheatEngine per modificare il valore da pagare alla morte!");
				BaseScript.TriggerEvent("lprp:serverLog", now.ToString("dd/MM/yyyy, HH:mm:ss") + "--il player " + GetPlayerName(source.Handle) + " ha usato un lua executor / CheatEngine per modificare il valore da pagare alla morte!");
				DropPlayer(source.Handle, "LastPlanet Shield [Sospetto Modding]: entra in discord a chiarire perfavore!");
				// AGGIUNGERE AVVISO DISCORD! ;)
			}
		}

		public static void Spawnato([FromSource] Player source)
		{
			User user = Funzioni.GetUserFromPlayerId(source.Handle);
			Log.Printa(LogType.Info, user.FullName + "(" + source.Name + ") e' entrato in città'");
			foreach (var player in from Player player in Server.Instance.GetPlayers.ToList() where player.Handle != source.Handle select player)
				player.TriggerEvent("lprp:ShowNotification", "~g~" + user.FullName + " (" + source.Name + ")~w~ è entrato in città");

			BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", Server.PlayerList.ToJson());
			source.TriggerEvent("lprp:createMissingPickups", PickupsServer.Pickups.ToJson());
			user.status.Spawned = true;
		}

		public static async void Dropped([FromSource] Player player, string reason)
		{
			Player p = player;
			string name = p.Name;
			string handle = p.Handle;
			DateTime now = DateTime.Now;
			string text = name + " e' uscito.";

			if (reason != "")
				text = reason switch
				{
					"Timed out after 10 seconds." => name + " e' crashato.",
					"Disconnected." or "Exited." => name + " si e' disconnesso.",
					_ => name + " si e' disconnesso: " + reason,
				};
			if (Server.PlayerList.ContainsKey(handle))
			{
				Server.PlayerList.TryGetValue(handle, out User ped);

				var disc = ped.identifiers.Discord;
				if (ped.status.Spawned)
				{
					await ped.SalvaPersonaggio();
					Log.Printa(LogType.Info, "Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' all'uscita dal gioco -- Discord:" + disc);
				}
				else
					Log.Printa(LogType.Info, "Il Player '" + name + "' - " + disc + " è uscito dal server senza selezionare un personaggio");

				Server.PlayerList.TryRemove(handle, out ped);
			}

			Log.Printa(LogType.Info, text);
			BaseScript.TriggerClientEvent("lprp:ShowNotification", "~r~" + text);
		}

		public static async void SalvaPlayer([FromSource] Player player)
		{
			await BaseScript.Delay(0);
			string name = player.Name;

			if (Server.PlayerList.ContainsKey(player.Handle))
			{
				User ped = Funzioni.GetUserFromPlayerId(player.Handle);

				if (ped.status.Spawned)
				{
					player.TriggerEvent("lprp:mostrasalvataggio");
					await ped.SalvaPersonaggio();
					Log.Printa(LogType.Info, "Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' tramite telefono");
				}
			}

			await Task.FromResult(0);
		}

		public static void removeItemsDeath([FromSource] Player source)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
			int money = player.Money;
			int dirty = player.DirtCash;
			foreach (Inventory inv in player.CurrentChar.Inventory.ToList()) player.removeInventoryItem(inv.Item, inv.Amount);
			foreach (Weapons inv in player.CurrentChar.Weapons.ToList()) player.removeWeapon(inv.name);
			player.Money -= money;
			player.DirtCash -= dirty;
		}

		public static void GiveMoney([FromSource] Player source, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.Money += amount;
		}

		public static void RemoveMoney([FromSource] Player source, int amount)
		{
			if (amount != 0)
			{
				User player = Funzioni.GetUserFromPlayerId(source.Handle);
				player.Money -= amount;
			}
			else
			{
				source.Drop("Rilevata possibile modifica ai valori di gioco");
			}
		}

		public static void GiveBank([FromSource] Player source, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.Bank += amount;
		}

		public static void RemoveBank([FromSource] Player source, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.Bank -= amount;
		}

		public static void GiveDirty([FromSource] Player source, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.DirtCash += amount;
		}

		public static void RemoveDirty([FromSource] Player source, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.DirtCash -= amount;
		}

		public static void AddInventory([FromSource] Player source, string item, int amount, float peso)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).addInventoryItem(item, amount, peso > 0 ? peso : ConfigShared.SharedConfig.Main.Generici.ItemList[item].peso);
		}

		public static void RemoveInventory([FromSource] Player source, string item, int amount)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).removeInventoryItem(item, amount);
		}

		public static void AddWeapon([FromSource] Player source, string weaponName, int ammo)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).addWeapon(weaponName, ammo);
		}

		public static void RemoveWeapon([FromSource] Player source, string weaponName)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).removeWeapon(weaponName);
		}

		public static void AddWeaponComp([FromSource] Player source, string weaponName, string weaponComponent)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).addWeaponComponent(weaponName, weaponComponent);
		}

		public static void RemoveWeaponComp([FromSource] Player source, string weaponName, string weaponComponent)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).removeWeaponComponent(weaponName, weaponComponent);
		}

		public static void AddWeaponTint([FromSource] Player source, string weaponName, int tint)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).addWeaponTint(weaponName, tint);
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
			Funzioni.GetUserFromPlayerId(target).addInventoryItem(item, amount, peso > 0 ? peso : ConfigShared.SharedConfig.Main.Generici.ItemList[item].peso);
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

		private static async void BannaPlayer(string target, string motivazione, bool temporaneo, long tempodiban, int banner)
		{
			DateTime TempoBan = new DateTime(tempodiban);
			Player Target = Funzioni.GetPlayerFromId(target);
			List<string> Tokens = new List<string>();
			for (int i = 0; i < GetNumPlayerTokens(target); i++) Tokens.Add(GetPlayerToken(target, i));
			RequestResponse pippo = await Discord.BotDiscordHandler.InviaAlBotERicevi(new
			{
				tipo = "BannaPlayer",
				RichiestaInterna = new
				{
					Banner = banner > 0 ? Funzioni.GetPlayerFromId(banner).Name : "Last Planet Shield 2.0",
					Bannato = Target.Name,
					IdMember = Target.Identifiers["discord"],
					Motivazione = motivazione,
					Temporaneo = temporaneo,
					DataFine = tempodiban,
					Tokens = Tokens.ToJson()
				}
			});

			if (pippo.content.FromJson<bool>())
			{
				Log.Printa(LogType.Warning, $"{(banner > 0 ? $"Il player {Funzioni.GetPlayerFromId(banner).Name}" : "L'anticheat")} ha bannato {Target.Name}, data di fine {TempoBan.ToString("yyyy-MM-dd HH:mm:ss")}");
				BaseScript.TriggerEvent("lprp:serverLog", $"{(banner > 0 ? $"Il player {Funzioni.GetPlayerFromId(banner).Name}" : "L'anticheat")} ha bannato {Target.Name}, data di fine {TempoBan.ToString("yyyy-MM-dd HH:mm:ss")}");
				//Target.Drop($"SHIELD 2.0 Sei stato bannato dal server:\nMotivazione: {motivazione},\nBannato da: {(banner > 0 ? Funzioni.GetPlayerFromId(banner).Name : "Sistema anticheat")}"); // modificare con introduzione in stile anticheat
			}
			else
			{
				Log.Printa(LogType.Error, "Ban fallito ");
			}
		}

		private static void Kick(string target, string motivazione, int kicker)
		{
			Player Target = Funzioni.GetPlayerFromId(target);
			Player Kicker = Funzioni.GetPlayerFromId(kicker);
			Log.Printa(LogType.Warning, $"Il player {Kicker.Name} ha kickato {Target.Name} fuori dal server, Motivazione: {motivazione}");
			BaseScript.TriggerEvent("lprp:serverLog", $"Il player {Kicker.Name} ha kickato {Target.Name} fuori dal server, Motivazione: {motivazione}");
			Target.Drop($"SHIELD 2.0 Sei stato allontanato dal server:\nMotivazione: {motivazione},\nKickato da: {Kicker.Name}");
		}

		private static void GiveLicense([FromSource] Player source, string license)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
		}

		private static void GiveLicenseToChar([FromSource] Player source, int target, string license)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
		}

		private static void RemoveLicense([FromSource] Player source, string license)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
		}

		private static void RemoveLicenseToChar([FromSource] Player source, int target, string license)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
		}

		private static void AggiornaAmmo([FromSource] Player source, string weaponName, int ammo)
		{
			User user = Funzioni.GetUserFromPlayerId(source.Handle);
			user.updateWeaponAmmo(weaponName, ammo);
		}

		private static void GiveItemToOtherPlayer([FromSource] Player source, int target, string itemName, int amount)
		{
			User player = Funzioni.GetUserFromPlayerId(source.Handle);
			User targetPlayer = Funzioni.GetUserFromPlayerId("" + target);
			player.removeInventoryItem(itemName, amount);
			player.showNotification($"Hai dato {amount} di {ConfigShared.SharedConfig.Main.Generici.ItemList[itemName].label} a {targetPlayer.FullName}");
			targetPlayer.addInventoryItem(itemName, amount, ConfigShared.SharedConfig.Main.Generici.ItemList[itemName].peso);
			targetPlayer.Player.TriggerEvent("lprp:riceviOggettoAnimazione");
			targetPlayer.showNotification($"Hai ricevuto {amount} di {ConfigShared.SharedConfig.Main.Generici.ItemList[itemName].label} da {player.FullName}");
		}

		private static void GiveWeaponToOtherPlayer([FromSource] Player source, int target, string weaponName, int ammo)
		{
			User player = source.GetCurrentChar();
			User targetPlayer = Funzioni.GetPlayerFromId(target).GetCurrentChar();
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
				targetPlayer.Player.TriggerEvent("lprp:riceviOggettoAnimazione");
				targetPlayer.showNotification($"Hai ricevuto un'arma con {ammo} munizioni da {player.FullName}");
			}
		}
	}
}