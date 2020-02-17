using CitizenFX.Core;
using Newtonsoft.Json;
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
			Server.GetInstance.RegisterEventHandler("lprp:finishCharServer", new Action<Player, string>(FinishChar));
			Server.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action<Player>(Spawnato));
			Server.GetInstance.RegisterEventHandler("lprp:setDeathStatus", new Action<Player, bool>(deathStatus));
			Server.GetInstance.RegisterEventHandler("onPlayerDropped", new Action<Player, string>(Dropped));
			Server.GetInstance.RegisterEventHandler("lprp:dropPlayer", new Action<Player, string>(Drop));
			Server.GetInstance.RegisterEventHandler("lprp:kickPlayer", new Action<string, string, int>(Kick));
			Server.GetInstance.RegisterEventHandler("lprp:updateCurChar", new Action<Player, string, dynamic, float>(UpdateChar));
			Server.GetInstance.RegisterEventHandler("lprp:CheckPing", new Action<Player>(Ping));
			Server.GetInstance.RegisterEventHandler("lprp:checkAFK", new Action<Player>(AFK));
			Server.GetInstance.RegisterEventHandler("lprp:payFine", new Action<Player, int>(PayFine));
			Server.GetInstance.RegisterEventHandler("lprp:givemoney", new Action<Player, int>(GiveMoney));
			Server.GetInstance.RegisterEventHandler("lprp:removemoney", new Action<Player, int>(RemoveMoney));
			Server.GetInstance.RegisterEventHandler("lprp:givebank", new Action<Player, int>(GiveBank));
			Server.GetInstance.RegisterEventHandler("lprp:removebank", new Action<Player, int>(RemoveBank));
			Server.GetInstance.RegisterEventHandler("lprp:givedirty", new Action<Player, int>(GiveDirty));
			Server.GetInstance.RegisterEventHandler("lprp:removedirty", new Action<Player, int>(RemoveDirty));
			Server.GetInstance.RegisterEventHandler("lprp:givedirty", new Action<Player, int>(GiveDirty));
			Server.GetInstance.RegisterEventHandler("lprp:addIntenvoryItem", new Action<Player, string, int>(AddInventory));
			Server.GetInstance.RegisterEventHandler("lprp:removeIntenvoryItem", new Action<Player, string, int>(RemoveInventory));
			Server.GetInstance.RegisterEventHandler("lprp:addWeapon", new Action<Player, string, int>(AddWeapon));
			Server.GetInstance.RegisterEventHandler("lprp:removeWeapon", new Action<Player, string>(RemoveWeapon));
			Server.GetInstance.RegisterEventHandler("lprp:addWeaponComponent", new Action<Player, string, string>(AddWeaponComp));
			Server.GetInstance.RegisterEventHandler("lprp:removeWeaponComponent", new Action<Player, string, string>(RemoveWeaponComp));
			Server.GetInstance.RegisterEventHandler("lprp:addWeaponTint", new Action<Player, string, int>(AddWeaponTint));
			Server.GetInstance.RegisterEventHandler("lprp:removeItemsDeath", new Action<Player>(removeItemsDeath));
			Server.GetInstance.RegisterEventHandler("lprp:serverlog", new Action<string>(ServerLog));
			Server.GetInstance.RegisterEventHandler("lprp:salvaPlayer", new Action<Player>(SalvaPlayer));
			Server.GetInstance.RegisterEventHandler("lprp:getPlayers", new Action<NetworkCallbackDelegate>(GetPlayers));
			Server.GetInstance.RegisterEventHandler("lprp:getDBPlayers", new Action<NetworkCallbackDelegate>(GetDBPlayers));
			Server.GetInstance.RegisterEventHandler("lprp:givemoneytochar", new Action<string, int, int>(GiveMoneyToChar));
			Server.GetInstance.RegisterEventHandler("lprp:removemoneytochar", new Action<string, int, int>(RemoveMoneyToChar));
			Server.GetInstance.RegisterEventHandler("lprp:givebanktochar", new Action<string, int, int>(GiveBankToChar));
			Server.GetInstance.RegisterEventHandler("lprp:removebanktochar", new Action<string, int, int>(RemoveBankToChar));
			Server.GetInstance.RegisterEventHandler("lprp:givedirtytochar", new Action<string, int, int>(GiveDirtyToChar));
			Server.GetInstance.RegisterEventHandler("lprp:removedirtytochar", new Action<string, int, int>(RemoveDirtyToChar));
			Server.GetInstance.RegisterEventHandler("lprp:givedirtytochar", new Action<string, int, int>(GiveDirtyToChar));
			Server.GetInstance.RegisterEventHandler("lprp:addIntenvoryItemtochar", new Action<string, int, string, int>(AddInventoryToChar));
			Server.GetInstance.RegisterEventHandler("lprp:removeIntenvoryItemtochar", new Action<string, int, string, int>(RemoveInventoryToChar));
			Server.GetInstance.RegisterEventHandler("lprp:addWeapontochar", new Action<string, int, string, int>(AddWeaponToChar));
			Server.GetInstance.RegisterEventHandler("lprp:removeWeapontochar", new Action<string, int, string>(RemoveWeaponToChar));
			Server.GetInstance.RegisterEventHandler("lprp:addWeaponComponenttochar", new Action<string, int, string, string>(AddWeaponCompToChar));
			Server.GetInstance.RegisterEventHandler("lprp:removeWeaponComponenttochar", new Action<string, int, string, string>(RemoveWeaponCompToChar));
			Server.GetInstance.RegisterEventHandler("lprp:addWeaponTinttochar", new Action<string, int, string, int>(AddWeaponTintToChar));
			Server.GetInstance.RegisterEventHandler("lprp:bannaPlayer", new Action<string, string, long, int>(BannaPlayer));
		}

		public static void FinishChar([FromSource] Player p, string data)
		{
			try
			{
				Char_data Char = JsonConvert.DeserializeObject<Char_data>(data);
				ServerEntrance.PlayerList[p.Handle].char_data.Add(Char);
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
			if (player.Ping >= ConfigServer.Conf.Main.PingMax)
			{
				player.Drop("Ping troppo alto (Limite: " + ConfigServer.Conf.Main.PingMax + ", tuo ping: " + player.Ping + ")");
			}
		}

		public static void AFK([FromSource] Player p)
		{
			p.Drop("Last Planet Shield 2.0:\nSei stato rilevato per troppo tempo in AFK");
		}

		public static void UpdateChar([FromSource] Player player, string type, dynamic data, float h)
		{
			if (type == "char_current")
			{
				ServerEntrance.PlayerList[player.Handle].char_current = (int)data;
			}
			else if (type == "char_data")
			{
				ServerEntrance.PlayerList[player.Handle].char_data = JsonConvert.DeserializeObject<List<Char_data>>(data);
			}
			else if (type == "status")
			{
				ServerEntrance.PlayerList[player.Handle].status.spawned = (bool)data;
			}
			else if (type == "charlocation")
			{
				ServerEntrance.PlayerList[player.Handle].CurrentChar.location.x = data.X;
				ServerEntrance.PlayerList[player.Handle].CurrentChar.location.y = data.Y;
				ServerEntrance.PlayerList[player.Handle].CurrentChar.location.z = data.Z;
				ServerEntrance.PlayerList[player.Handle].CurrentChar.location.h = h;
			}
			else if (type == "skin")
			{
				ServerEntrance.PlayerList[player.Handle].CurrentChar.skin = JsonConvert.DeserializeObject<Skin>(data);
			}
			else if (type == "needs")
			{
				ServerEntrance.PlayerList[player.Handle].CurrentChar.needs = JsonConvert.DeserializeObject<Needs>(data);
			}
			else if (type == "skill")
			{
				ServerEntrance.PlayerList[player.Handle].CurrentChar.statistiche = JsonConvert.DeserializeObject<Statistiche>(data);
			}
			else if (type == "chardressing")
			{
				ServerEntrance.PlayerList[player.Handle].CurrentChar.dressing = JsonConvert.DeserializeObject<Dressing>(data);
			}
			else if (type == "weapons")
			{
				ServerEntrance.PlayerList[player.Handle].CurrentChar.weapons = JsonConvert.DeserializeObject<List<Weapons>>(data);
			}
			else if (type == "job")
			{
				ServerEntrance.PlayerList[player.Handle].CurrentChar.job = JsonConvert.DeserializeObject<Job>(data);
			}
			else if (type == "gang")
			{
				ServerEntrance.PlayerList[player.Handle].CurrentChar.gang = JsonConvert.DeserializeObject<Gang>(data);
			}
			else if (type == "group")
			{
				ServerEntrance.PlayerList[player.Handle].group = data;
			}
			else if (type == "groupL")
			{
				ServerEntrance.PlayerList[player.Handle].group_level = data;
			}
			string _char_data = JsonConvert.SerializeObject(ServerEntrance.PlayerList[player.Handle].char_data);
			BaseScript.TriggerClientEvent(player, "lprp:sendUserInfo", _char_data, ServerEntrance.PlayerList[player.Handle].char_current, ServerEntrance.PlayerList[player.Handle].group);
			BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", JsonConvert.SerializeObject(ServerEntrance.PlayerList));
		}

		public static void deathStatus([FromSource] Player source, bool value)
		{
			ServerEntrance.PlayerList[source.Handle].DeathStatus = value;
		}

		public static void PayFine([FromSource] Player source, int amount)
		{
			var player = ServerEntrance.PlayerList[source.Handle];
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
			Log.Printa(LogType.Info, ServerEntrance.PlayerList[source.Handle].FullName + "(" + source.Name + ") e' entrato in citta'");
			BaseScript.TriggerEvent("lprp:serverLog", ServerEntrance.PlayerList[source.Handle].FullName + "(" + source.Name + ") è entrato in città");
			foreach (Player player in Server.GetInstance.GetPlayers.ToList())
				if (player.Handle != source.Handle)
					BaseScript.TriggerClientEvent(player, "lprp:ShowNotification", "~g~" + ServerEntrance.PlayerList[source.Handle].FullName + " (" + source.Name + ")~w~ è entrato in città");
			BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", JsonConvert.SerializeObject(ServerEntrance.PlayerList));
		}

		public static void Dropped([FromSource] Player player, string reason)
		{
			var now = DateTime.Now;
			if (reason != "")
			{
				if (reason == "Timed out after 10 seconds.")
				{
					Log.Printa(LogType.Info, GetPlayerName(player.Handle) + " e' crashato.");
					BaseScript.TriggerEvent("lprp:serverLog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " " + player.Name + " e' crashato.");
					BaseScript.TriggerClientEvent("lprp:ShowNotification", "~r~" + player.Name + "~w~ è crashato.");
				}
				else if (reason == "Disconnected." || reason == "Exited.")
				{
					Log.Printa(LogType.Info, player.Name + " si e' disconnesso.");
					BaseScript.TriggerEvent("lprp:serverLog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " " + player.Name + " si e' disconnesso.");
					BaseScript.TriggerClientEvent("lprp:ShowNotification", "~r~" + player.Name + "~w~ si e' disconnesso.");
				}
			}
			else
			{
				Log.Printa(LogType.Info, player.Name + " e' uscito.");
				BaseScript.TriggerEvent("lprp:serverLog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " " + player.Name + " e' uscito.");
				BaseScript.TriggerClientEvent("lprp:ShowNotification", "~r~" + player.Name + "~w~ e' uscito.");
			}
		}

		public static async void SalvaPlayer([FromSource] Player player)
		{
			string name = player.Name;
			if (ServerEntrance.PlayerList.ContainsKey(player.Handle))
			{
				var ped = ServerEntrance.PlayerList[player.Handle];
				if (ped.status.spawned)
				{
					player.TriggerEvent("lprp:mostrasalvataggio");
					Funzioni.SalvaPersonaggio(player);
					Log.Printa(LogType.Info, "Salvato personaggio: '" + ServerEntrance.PlayerList[player.Handle].FullName + "' appartenente a '" + name + "' tramite telefono");
					BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Salvato personaggio: '" + ServerEntrance.PlayerList[player.Handle].FullName + "' appartenente a '" + name + "' - " + ServerEntrance.PlayerList[player.Handle].identifiers.discord + ", tramite telefono");
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
			var player = ServerEntrance.PlayerList[source.Handle];
			List<Inventory> inventory = player.getCharInventory(player.char_current);
			List<Weapons> weapons = player.getCharWeapons(player.char_current);
			int money = player.Money;
			int dirty = player.DirtyMoney;

			for (int i = 0; i < inventory.Count; i++)
			{
				player.removeInventoryItem(inventory[i].item, inventory[i].amount);
			}

			for (int i = 0; i < weapons.Count; i++)
			{
				player.removeWeapon(weapons[i].name);
			}

			player.Money -= money;
			player.DirtyMoney -= dirty;
		}

		public static void GiveMoney([FromSource]Player source, int amount)
		{
			var player = ServerEntrance.PlayerList[source.Handle];
			player.Money += (amount);
		}

		public static void RemoveMoney([FromSource]Player source, int amount)
		{
			if (amount != 0)
			{
				var player = ServerEntrance.PlayerList[source.Handle];
				player.Money -= amount;
			}
			else
				source.Drop("Rilevata possibile modifica ai valori di gioco");
		}

		public static void GiveBank([FromSource]Player source, int amount)
		{
			var player = ServerEntrance.PlayerList[source.Handle];
			player.Bank += amount;
		}

		public static void RemoveBank([FromSource]Player source, int amount)
		{
			var player = ServerEntrance.PlayerList[source.Handle];
			player.Bank -= amount;
		}

		public static void GiveDirty([FromSource]Player source, int amount)
		{
			var player = ServerEntrance.PlayerList[source.Handle];
			player.DirtyMoney += amount;
		}

		public static void RemoveDirty([FromSource]Player source, int amount)
		{
			var player = ServerEntrance.PlayerList[source.Handle];
			player.DirtyMoney -= amount;
		}

		public static void AddInventory([FromSource]Player source, string item, int amount)
		{
			ServerEntrance.PlayerList[source.Handle].addInventoryItem(item, amount);
		}

		public static void RemoveInventory([FromSource]Player source, string item, int amount)
		{
			ServerEntrance.PlayerList[source.Handle].removeInventoryItem(item, amount);
		}

		public static void AddWeapon([FromSource]Player source, string weaponName, int ammo)
		{
			ServerEntrance.PlayerList[source.Handle].addWeapon(weaponName, ammo);
		}
		public static void RemoveWeapon([FromSource]Player source, string weaponName)
		{
			ServerEntrance.PlayerList[source.Handle].removeWeapon(weaponName);
		}

		public static void AddWeaponComp([FromSource]Player source, string weaponName, string weaponComponent)
		{
			ServerEntrance.PlayerList[source.Handle].addWeaponComponent(weaponName, weaponComponent);
		}
		public static void RemoveWeaponComp([FromSource]Player source, string weaponName, string weaponComponent)
		{
			ServerEntrance.PlayerList[source.Handle].removeWeaponComponent(weaponName, weaponComponent);
		}
		public static void AddWeaponTint([FromSource]Player source, string weaponName, int tint)
		{
			ServerEntrance.PlayerList[source.Handle].addWeaponTint(weaponName, tint);
		}

		public static void GetPlayers(NetworkCallbackDelegate CB)
		{
			CB.Invoke(JsonConvert.SerializeObject(ServerEntrance.PlayerList));
		}

		public static async void GetDBPlayers(NetworkCallbackDelegate CB)
		{
			dynamic result = await Server.GetInstance.Query($"SELECT * FROM `users`");
			CB.Invoke(JsonConvert.SerializeObject(result));
		}


		public static void GiveMoneyToChar(string target, int charId, int amount)
		{
			var player = ServerEntrance.PlayerList[target];
			player.Money += (amount);
		}

		public static void RemoveMoneyToChar(string target, int charId, int amount)
		{
			if (amount != 0)
			{
				var player = ServerEntrance.PlayerList[target];
				player.Money -= amount;
			}
		}

		public static void GiveBankToChar(string target, int charId, int amount)
		{
			var player = ServerEntrance.PlayerList[target];
			player.Bank += amount;
		}

		public static void RemoveBankToChar(string target, int charId, int amount)
		{
			var player = ServerEntrance.PlayerList[target];
			player.Bank -= amount;
		}

		public static void GiveDirtyToChar(string target, int charId, int amount)
		{
			var player = ServerEntrance.PlayerList[target];
			player.DirtyMoney += amount;
		}

		public static void RemoveDirtyToChar(string target, int charId, int amount)
		{
			var player = ServerEntrance.PlayerList[target];
			player.DirtyMoney -= amount;
		}

		public static void AddInventoryToChar(string target, int charId, string item, int amount)
		{
			ServerEntrance.PlayerList[target].addInventoryItem(item, amount);
		}

		public static void RemoveInventoryToChar(string target, int charId, string item, int amount)
		{
			ServerEntrance.PlayerList[target].removeInventoryItem(item, amount);
		}

		public static void AddWeaponToChar(string target, int charId, string weaponName, int ammo)
		{
			ServerEntrance.PlayerList[target].addWeapon(weaponName, ammo);
		}
		public static void RemoveWeaponToChar(string target, int charId, string weaponName)
		{
			ServerEntrance.PlayerList[target].removeWeapon(weaponName);
		}

		public static void AddWeaponCompToChar(string target, int charId, string weaponName, string weaponComponent)
		{
			ServerEntrance.PlayerList[target].addWeaponComponent(weaponName, weaponComponent);
		}
		public static void RemoveWeaponCompToChar(string target, int charId, string weaponName, string weaponComponent)
		{
			ServerEntrance.PlayerList[target].removeWeaponComponent(weaponName, weaponComponent);
		}
		public static void AddWeaponTintToChar(string target, int charId, string weaponName, int tint)
		{
			ServerEntrance.PlayerList[target].addWeaponTint(weaponName, tint);
		}

		private static async void BannaPlayer(string target, string motivazione, long tempodiban, int banner)
		{
			DateTime TempoBan = new DateTime(tempodiban);
			Player Target = Funzioni.GetPlayerFromId(target);
			Player Banner = Funzioni.GetPlayerFromId(banner);
			await Server.GetInstance.Execute("INSERT INTO bans (`discord`, `license`, `Name`, `DataFine`, `Banner`, `Motivazione`) VALUES (@disc, @lice, @nome, @datafine, @banner, @motivation)", new
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

		private static async void Kick(string target, string motivazione, int kicker)
		{
			Player Target = Funzioni.GetPlayerFromId(target);
			Player Kicker = Funzioni.GetPlayerFromId(kicker);
			Log.Printa(LogType.Warning, $"Il player {Kicker.Name} ha kickato {Target.Name} fuori dal server, Motivazione: {motivazione}");
			BaseScript.TriggerEvent("lprp:serverLog", $"Il player {Kicker.Name} ha kickato {Target.Name} fuori dal server, Motivazione: {motivazione}");
			Target.Drop($"SHIELD 2.0 Sei stato allontanato dal server:\nMotivazione: {motivazione},\nKickato da: {Kicker.Name}");
		}
	}
}
