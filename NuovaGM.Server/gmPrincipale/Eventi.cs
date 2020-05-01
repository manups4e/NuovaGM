using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using NuovaGM.Server.Interactions;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.gmPrincipale
{
	static class Eventi
	{
		static int EarlyRespawnFineAmount = 5000;
		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:finishCharServer", new Action<Player, string>(FinishChar));
			Server.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action<Player>(Spawnato));
			Server.Instance.AddEventHandler("lprp:setDeathStatus", new Action<Player, bool>(deathStatus));
			Server.Instance.AddEventHandler("playerDropped", new Action<Player, string>(Dropped));
			Server.Instance.AddEventHandler("lprp:dropPlayer", new Action<Player, string>(Drop));
			Server.Instance.AddEventHandler("lprp:kickPlayer", new Action<string, string, int>(Kick));
			Server.Instance.AddEventHandler("lprp:updateCurChar", new Action<Player, string, dynamic, float>(UpdateChar));
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
			Server.Instance.AddEventHandler("lprp:serverlog", new Action<string>(ServerLog));
			Server.Instance.AddEventHandler("lprp:salvaPlayer", new Action<Player>(SalvaPlayer));
			Server.Instance.AddEventHandler("lprp:getPlayers", new Action<NetworkCallbackDelegate>(GetPlayers));
			Server.Instance.AddEventHandler("lprp:getDBPlayers", new Action<NetworkCallbackDelegate>(GetDBPlayers));
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
			Server.Instance.AddEventHandler("lprp:bannaPlayer", new Action<string, string, long, int>(BannaPlayer));
			Server.Instance.AddEventHandler("lprp:giveLicense", new Action<Player, string>(GiveLicense));
			Server.Instance.AddEventHandler("lprp:giveLicenseToChar", new Action<Player, int, string>(GiveLicenseToChar));
			Server.Instance.AddEventHandler("lprp:removeLicense", new Action<Player, string>(RemoveLicense));
			Server.Instance.AddEventHandler("lprp:removeLicenseToChar", new Action<Player, int, string>(RemoveLicenseToChar));
			Server.Instance.AddEventHandler("lprp:updateWeaponAmmo", new Action<Player, string, int>(AggiornaAmmo));
			Server.Instance.AddEventHandler("lprp:giveInventoryItemToPlayer", new Action<Player, int, string, int>(GiveItemToOtherPlayer));
			Server.Instance.AddEventHandler("lprp:giveWeaponToPlayer", new Action<Player, int, string, int>(GiveWeaponToOtherPlayer));
		}

		public static void FinishChar([FromSource] Player p, string data)
		{
			try
			{
				Char_data Char = JsonConvert.DeserializeObject<Char_data>(data);
				User user = Funzioni.GetUserFromPlayerId(p.Handle);
				user.char_data.Add(Char);
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, $"{ e.Message}");
			}

		}

		public static void Drop([FromSource]Player player, string reason)
		{
			player.Drop(reason);
		}

		public static void Ping([FromSource] Player player)
		{
			if (player.Ping >= Server.Impostazioni.Main.PingMax)
			{
				player.Drop("Ping troppo alto (Limite: " + Server.Impostazioni.Main.PingMax + ", tuo ping: " + player.Ping + ")");
			}
		}

		public static void AFK([FromSource] Player p)
		{
			p.Drop("Last Planet Shield 2.0:\nSei stato rilevato per troppo tempo in AFK");
		}

		public static void UpdateChar([FromSource] Player player, string type, dynamic data, float h)
		{
			User user = Funzioni.GetUserFromPlayerId(player.Handle);
			if (type == "char_current")
			{
				user.char_current = (int)data;
			}
			else if (type == "char_data")
			{
				user.char_data = JsonConvert.DeserializeObject<List<Char_data>>(data);
			}
			else if (type == "status")
			{
				user.status.spawned = (bool)data;
			}
			else if (type == "charlocation")
			{
				user.CurrentChar.location.position = data;
				user.CurrentChar.location.h = h;
			}
			else if (type == "skin")
			{
				user.CurrentChar.skin = JsonConvert.DeserializeObject<Skin>(data);
			}
			else if (type == "needs")
			{
				user.CurrentChar.needs = JsonConvert.DeserializeObject<Needs>(data);
			}
			else if (type == "skill")
			{
				user.CurrentChar.statistiche = JsonConvert.DeserializeObject<Statistiche>(data);
			}
			else if (type == "chardressing")
			{
				user.CurrentChar.dressing = JsonConvert.DeserializeObject<Dressing>(data);
			}
			else if (type == "weapons")
			{
				user.CurrentChar.weapons = JsonConvert.DeserializeObject<List<Weapons>>(data);
			}
			else if (type == "job")
			{
				user.CurrentChar.job = JsonConvert.DeserializeObject<Job>(data);
			}
			else if (type == "gang")
			{
				user.CurrentChar.gang = JsonConvert.DeserializeObject<Gang>(data);
			}
			else if (type == "group")
			{
				user.group = data;
			}
			else if (type == "groupL")
			{
				user.group_level = data;
			}
			string _char_data = JsonConvert.SerializeObject(user.char_data);
			BaseScript.TriggerClientEvent(player, "lprp:sendUserInfo", _char_data, user.char_current, user.group);
			BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", JsonConvert.SerializeObject(Server.PlayerList));
		}

		public static void deathStatus([FromSource] Player source, bool value)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).DeathStatus = value;
		}

		public static void PayFine([FromSource] Player source, int amount)
		{
			var player = Funzioni.GetUserFromPlayerId(source.Handle);
			if (amount == EarlyRespawnFineAmount)
			{
				player.Money -= EarlyRespawnFineAmount;
			}
			else
			{
				var now = DateTime.Now;
				Log.Printa(LogType.Warning, "il player " + GetPlayerName(source.Handle) + " ha usato un lua executor / CheatEngine per modificare il valore da pagare alla morte!");
				BaseScript.TriggerEvent("lprp:serverLog", now.ToString("dd/MM/yyyy, HH:mm:ss") + "--il player " + GetPlayerName(source.Handle) + " ha usato un lua executor / CheatEngine per modificare il valore da pagare alla morte!");
				DropPlayer(source.Handle, "LastPlanet Shield [Sospetto Modding]: entra in discord a chiarire perfavore!");
				// AGGIUNGERE AVVISO DISCORD! ;)
			}
		}

		public static void Spawnato([FromSource] Player source)
		{
			var user = Funzioni.GetUserFromPlayerId(source.Handle);
			Log.Printa(LogType.Info, user.FullName + "(" + source.Name + ") e' entrato in citta'");
			BaseScript.TriggerEvent("lprp:serverLog",user.FullName + "(" + source.Name + ") è entrato in città");
			foreach (Player player in Server.Instance.GetPlayers.ToList())
				if (player.Handle != source.Handle)
					player.TriggerEvent("lprp:ShowNotification", "~g~" +user.FullName + " (" + source.Name + ")~w~ è entrato in città");
			BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", JsonConvert.SerializeObject(Server.PlayerList));
			source.TriggerEvent("lprp:createMissingPickups", JsonConvert.SerializeObject(PickupsServer.Pickups));
		}

		public static async void Dropped([FromSource] Player player, string reason)
		{
			Player p = player;
			string name = p.Name;
			string handle = p.Handle;
			var now = DateTime.Now;
			string text = name + " e' uscito.";
			if (reason != "")
			{
				if (reason == "Timed out after 10 seconds.")
					text = name + " e' crashato.";
				else if (reason == "Disconnected." || reason == "Exited.")
					text = name + " si e' disconnesso.";
				else
					text = name + " si e' disconnesso: " + reason;
			}
			if (Server.PlayerList.ContainsKey(handle))
			{
				User ped;
				Server.PlayerList.TryGetValue(handle, out ped);
				if (ped.status.spawned)
				{
					await Funzioni.SalvaPersonaggio(p);
					Log.Printa(LogType.Info, "Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' all'uscita dal gioco -- Discord:" + ped.identifiers.discord);
					BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' all'uscita dal gioco -- Discord:" + ped.identifiers.discord);
				}
				else
				{
					Log.Printa(LogType.Info, "Il Player'" + name + "' - " + ped.identifiers.discord + " è uscito dal server senza selezionare un personaggio");
					BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Il Player'" + name + "' - " + ped.identifiers.discord + " è uscito dal server senza selezionare un personaggio");
				}
				Server.PlayerList.TryRemove(handle, out ped);
			}
			Log.Printa(LogType.Info, text);
			BaseScript.TriggerEvent("lprp:serverLog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " " + text);
			BaseScript.TriggerClientEvent("lprp:ShowNotification", "~r~" + text);
			BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", JsonConvert.SerializeObject(Server.PlayerList));
		}

		public static async void SalvaPlayer([FromSource] Player player)
		{
			await BaseScript.Delay(0);
			string name = player.Name;
			if (Server.PlayerList.ContainsKey(player.Handle))
			{
				var ped = Funzioni.GetUserFromPlayerId(player.Handle);
				if (ped.status.spawned)
				{
					player.TriggerEvent("lprp:mostrasalvataggio");
					await Funzioni.SalvaPersonaggio(player);
					Log.Printa(LogType.Info, "Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' tramite telefono");
					BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' - " + ped.identifiers.discord + ", tramite telefono");
				}
			}
			await Task.FromResult(0);
		}

		public static void ServerLog(string txt)
		{
			using (StreamWriter w = File.AppendText("Log Del Server.txt"))
			{
				w.WriteLine(txt);
			}
		}

		public static void removeItemsDeath([FromSource] Player source)
		{
			var player = Funzioni.GetUserFromPlayerId(source.Handle);
			int money = player.Money;
			int dirty = player.DirtyMoney;

			foreach(var inv in player.CurrentChar.inventory)
				player.removeInventoryItem(inv.item, inv.amount);

			foreach (var inv in player.CurrentChar.weapons)
				player.removeWeapon(inv.name);

			player.Money -= money;
			player.DirtyMoney -= dirty;
		}

		public static void GiveMoney([FromSource]Player source, int amount)
		{
			var player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.Money += (amount);
		}

		public static void RemoveMoney([FromSource]Player source, int amount)
		{
			if (amount != 0)
			{
				var player = Funzioni.GetUserFromPlayerId(source.Handle);
				player.Money -= amount;
			}
			else
				source.Drop("Rilevata possibile modifica ai valori di gioco");
		}

		public static void GiveBank([FromSource]Player source, int amount)
		{
			var player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.Bank += amount;
		}

		public static void RemoveBank([FromSource]Player source, int amount)
		{
			var player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.Bank -= amount;
		}

		public static void GiveDirty([FromSource]Player source, int amount)
		{
			var player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.DirtyMoney += amount;
		}

		public static void RemoveDirty([FromSource]Player source, int amount)
		{
			var player = Funzioni.GetUserFromPlayerId(source.Handle);
			player.DirtyMoney -= amount;
		}

		public static void AddInventory([FromSource]Player source, string item, int amount, float peso)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).addInventoryItem(item, amount, peso>0?peso:SharedScript.ItemList[item].peso);
		}

		public static void RemoveInventory([FromSource]Player source, string item, int amount)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).removeInventoryItem(item, amount);
		}

		public static void AddWeapon([FromSource]Player source, string weaponName, int ammo)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).addWeapon(weaponName, ammo);
		}

		public static void RemoveWeapon([FromSource]Player source, string weaponName)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).removeWeapon(weaponName);
		}

		public static void AddWeaponComp([FromSource]Player source, string weaponName, string weaponComponent)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).addWeaponComponent(weaponName, weaponComponent);
		}
		public static void RemoveWeaponComp([FromSource]Player source, string weaponName, string weaponComponent)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).removeWeaponComponent(weaponName, weaponComponent);
		}
		public static void AddWeaponTint([FromSource]Player source, string weaponName, int tint)
		{
			Funzioni.GetUserFromPlayerId(source.Handle).addWeaponTint(weaponName, tint);
		}

		public static void GetPlayers(NetworkCallbackDelegate CB)
		{
			CB.Invoke(JsonConvert.SerializeObject(Server.PlayerList));
		}

		public static async void GetDBPlayers(NetworkCallbackDelegate CB)
		{
			dynamic result = await Server.Instance.Query($"SELECT * FROM users");
			CB.Invoke(JsonConvert.SerializeObject(result));
		}


		public static void GiveMoneyToChar(string target, int charId, int amount)
		{
			var player = Funzioni.GetUserFromPlayerId(target);
			player.Money += (amount);
		}

		public static void RemoveMoneyToChar(string target, int charId, int amount)
		{
			if (amount != 0)
			{
				var player = Funzioni.GetUserFromPlayerId(target);
				player.Money -= amount;
			}
		}

		public static void GiveBankToChar(string target, int charId, int amount)
		{
			var player = Funzioni.GetUserFromPlayerId(target);
			player.Bank += amount;
		}

		public static void RemoveBankToChar(string target, int charId, int amount)
		{
			var player = Funzioni.GetUserFromPlayerId(target);
			player.Bank -= amount;
		}

		public static void GiveDirtyToChar(string target, int charId, int amount)
		{
						var player = Funzioni.GetUserFromPlayerId(target);
			player.DirtyMoney += amount;
		}

		public static void RemoveDirtyToChar(string target, int charId, int amount)
		{
						var player = Funzioni.GetUserFromPlayerId(target);
			player.DirtyMoney -= amount;
		}

		public static void AddInventoryToChar(string target, int charId, string item, int amount, float peso)
		{
			Funzioni.GetUserFromPlayerId(target).addInventoryItem(item, amount, peso>0?peso:SharedScript.ItemList[item].peso);
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

		private static async void BannaPlayer(string target, string motivazione, long tempodiban, int banner)
		{
			await BaseScript.Delay(0);
			DateTime TempoBan = new DateTime(tempodiban);
			Player Target = Funzioni.GetPlayerFromId(target);
			Player Banner = Funzioni.GetPlayerFromId(banner);
			await Server.Instance.Execute("INSERT INTO bans (`discord`, `license`, `Name`, `DataFine`, `Banner`, `Motivazione`) VALUES (@disc, @lice, @nome, @datafine, @banner, @motivation)", new
			{
				disc = License.GetLicense(Target, Identifier.Discord),
				lice = License.GetLicense(Target, Identifier.License),
				nome = Target.Name,
				datafine = TempoBan.ToString("yyyy-MM-dd HH:mm:ss"),
				banner = Banner.Name,
				motivation = motivazione
			});
			Log.Printa(LogType.Warning, $"Il player {Banner.Name} ha bannato {Target.Name}, data di fine {TempoBan.ToString("yyyy-MM-dd HH:mm:ss")}");
			BaseScript.TriggerEvent("lprp:serverLog", $"Il player {Banner.Name} ha bannato {Target.Name}, data di fine {TempoBan.ToString("yyyy-MM-dd HH:mm:ss")}");
			Target.Drop($"SHIELD 2.0 Sei stato bannato dal server:\nMotivazione: {motivazione},\nBannato da: {Banner.Name}"); // modificare con introduzione in stile anticheat
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
			User targetPlayer = Funzioni.GetUserFromPlayerId(""+target);

			player.removeInventoryItem(itemName, amount);
			player.showNotification($"Hai dato {amount} di {SharedScript.ItemList[itemName].label} a {targetPlayer.FullName}");
			targetPlayer.addInventoryItem(itemName, amount, SharedScript.ItemList[itemName].peso);
			targetPlayer.p.TriggerEvent("lprp:riceviOggettoAnimazione");
			targetPlayer.showNotification($"Hai ricevuto {amount} di {SharedScript.ItemList[itemName].label} da {player.FullName}");
		}

		private static  void GiveWeaponToOtherPlayer([FromSource] Player source, int target, string weaponName, int ammo)
		{
			User player = source.GetCurrentChar();
			User targetPlayer = Funzioni.GetPlayerFromId(target).GetCurrentChar();
			var weapon = player.getWeapon(weaponName);
			var arma = weapon.Item2;
			if (targetPlayer.hasWeapon(weaponName))
				player.showNotification($"{player.FullName} ha già quest'arma!");
			else
			{
				player.removeWeapon(weaponName);
				player.showNotification($"Hai dato la tua arma a {targetPlayer.FullName}");
				targetPlayer.addWeapon(weaponName, ammo);
				foreach (var comp in arma.components) targetPlayer.addWeaponComponent(weaponName, comp.name);
				if (arma.tint != 0) targetPlayer.addWeaponTint(weaponName, arma.tint);
				targetPlayer.p.TriggerEvent("lprp:riceviOggettoAnimazione");
				targetPlayer.showNotification($"Hai ricevuto un'arma con {ammo} munizioni da {player.FullName}");
			}
		}

	}
}
