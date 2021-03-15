using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Impostazioni.Shared.Configurazione.Generici;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.Veicoli;
using System.Threading.Tasks;

namespace TheLastPlanet.Server.Core.PlayerChar
{
	public class User : BasePlayerShared
	{
		[JsonIgnore] public string source;

		public User()
		{
		}

		[JsonIgnore] public Player Player;
		[JsonIgnore] public DateTime LastSaved;

		public User(Player player, BasePlayerShared result)
		{
			lastConnection = DateTime.Now;
			source = player.Handle;
			char_current = result.char_current;
			UserID = result.UserID;
			group = result.group;
			group_level = result.group_level;
			playTime = result.playTime;
			Player = player;
			StatiPlayer = new PlayerStateBags(player);
			char_data = result.char_data;
			LastSaved = DateTime.Now;
		}

		public User(Player player, dynamic result)
		{
			lastConnection = DateTime.Now;
			source = player.Handle;
			char_current = result.char_current;
			UserID = result.UserID;
			group = result.group;
			group_level = (UserGroup)result.group_level;
			playTime = result.playTime;
			Player = player;
			StatiPlayer = new PlayerStateBags(player);
			Characters = (result.char_data as string).DeserializeFromJson<List<Char_data>>();
			LastSaved = DateTime.Now;
		}

		public User(dynamic result)
		{
			lastConnection = DateTime.Now;
			//source = player.Handle;
			char_current = result.char_current;
			group = result.group;
			group_level = (UserGroup)result.group_level;
			playTime = result.playTime;
			//p = player;
			Characters = (result.char_data as string).DeserializeFromJson<List<Char_data>>();
			LastSaved = DateTime.Now;
		}

		[JsonIgnore]
		public Char_data CurrentChar
		{
			get { return Characters.FirstOrDefault(x => x.id - 1 == char_current - 1); }
		}

		[JsonIgnore] public string FullName => CurrentChar.info.firstname + " " + CurrentChar.info.lastname;

		[JsonIgnore] public string DOB => CurrentChar.info.dateOfBirth;

		[JsonIgnore]
		public bool DeathStatus
		{
			get => CurrentChar.is_dead;
			set => CurrentChar.is_dead = value;
		}

		[JsonIgnore]
		public int Money
		{
			get => CurrentChar.finance.money;
			set
			{
				int var = value - CurrentChar.finance.money;
				CurrentChar.finance.money += var;
				if (var < 0)
					if (CurrentChar.finance.money < 0)
						CurrentChar.finance.money = 0;
				Player.TriggerEvent("lprp:changeMoney", var);
				Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
			}
		}

		[JsonIgnore]
		public int Bank
		{
			get => CurrentChar.finance.bank;
			set
			{
				int var = value - CurrentChar.finance.bank;
				CurrentChar.finance.bank += var;
				if (var < 0) Player.TriggerEvent("lprp:rimuoviBank", var);
				Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
			}
		}

		[JsonIgnore]
		public int DirtyMoney
		{
			get => CurrentChar.finance.dirtyCash;
			set
			{
				int var = value - CurrentChar.finance.dirtyCash;
				CurrentChar.finance.dirtyCash += var;
				if (var < 0)
					if (CurrentChar.finance.dirtyCash < 0)
						CurrentChar.finance.dirtyCash = 0;
				Player.TriggerEvent("lprp:changeDirty", var);
				Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
			}
		}

		public void SetJob(string job, int grade)
		{
			CurrentChar.job.name = job;
			CurrentChar.job.grade = grade;
			Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
		}

		public void SetGang(string job, int grade)
		{
			CurrentChar.gang.name = job;
			CurrentChar.gang.grade = grade;
			Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
		}

		public Tuple<bool, Inventory> getInventoryItem(string item)
		{
			for (int i = 0; i < CurrentChar.inventory.Count; i++)
				if (CurrentChar.inventory[i].item == item)
					return new Tuple<bool, Inventory>(true, CurrentChar.inventory[i]);

			return new Tuple<bool, Inventory>(false, null);
		}

		public List<Inventory> getCharInventory(int charId)
		{
			for (int i = 0; i < Characters.Count; i++)
				if (Characters[i].id == charId)
					return Characters[i].inventory;

			return null;
		}

		public void addInventoryItem(string item, int amount, float weight)
		{
			bool vero = getInventoryItem(item).Item1;
			Inventory checkedItem = getInventoryItem(item).Item2;

			if (vero)
			{
				checkedItem.amount += amount;

				if (checkedItem.amount == ConfigShared.SharedConfig.Main.Generici.ItemList[item].max)
				{
					checkedItem.amount = ConfigShared.SharedConfig.Main.Generici.ItemList[item].max;
					Player.TriggerEvent("lprp:ShowNotification", "HAI GIA' IL MASSIMO DI ~w~" + ConfigShared.SharedConfig.Main.Generici.ItemList[item].label + "~w~!");
				}
			}
			else
			{
				CurrentChar.inventory.Add(new Inventory(item, amount, weight));
			}

			Player.TriggerEvent("lprp:ShowNotification", "Hai ricevuto " + amount + " " + ConfigShared.SharedConfig.Main.Generici.ItemList[item].label + "!");
			Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
		}

		public void removeInventoryItem(string item, int amount)
		{
			bool vero = getInventoryItem(item).Item1;
			Inventory checkedItem = getInventoryItem(item).Item2;

			if (vero)
			{
				checkedItem.amount -= amount;
				if (checkedItem.amount <= 0) CurrentChar.inventory.Remove(checkedItem);
			}
			else
			{
				CurrentChar.inventory.ToList().Remove(checkedItem);
			}

			Player.TriggerEvent("lprp:ShowNotification", amount + " " + ConfigShared.SharedConfig.Main.Generici.ItemList[item].label + " ti sono stati rimossi/e!");
			Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
		}

		public List<Weapons> getCharWeapons(int charId)
		{
			for (int i = 0; i < Characters.Count; i++)
				if (Characters[i].id == charId)
					return Characters[i].weapons;

			return null;
		}

		public void addWeapon(string weaponName, int ammo)
		{
			if (!hasWeapon(weaponName))
			{
				CurrentChar.weapons.Add(new Weapons(weaponName, ammo, new List<Components>(), 0));
				Player.TriggerEvent("lprp:addWeapon", weaponName, ammo);
				Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
			}
		}

		public void updateWeaponAmmo(string weaponName, int ammo)
		{
			Tuple<int, Weapons> weapon = getWeapon(weaponName);
			if (weapon.Item2.ammo > ammo) CurrentChar.weapons[weapon.Item1].ammo = ammo;
		}

		public void removeWeapon(string weaponName)
		{
			Log.Printa(LogType.Debug, "index = " + getWeapon(weaponName).Item1);
			Log.Printa(LogType.Debug, JsonConvert.SerializeObject(getWeapon(weaponName).Item2));

			if (hasWeapon(weaponName))
			{
				CurrentChar.weapons.Remove(getWeapon(weaponName).Item2);
				Player.TriggerEvent("lprp:removeWeapon", weaponName);
				Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
			}
		}

		public void addWeaponComponent(string weaponName, string weaponComponent)
		{
			int num = getWeapon(weaponName).Item1;

			if (hasWeaponComponent(weaponName, weaponComponent))
			{
				Player.TriggerEvent("lprp:possiediArma", weaponName, weaponComponent);
			}
			else
			{
				CurrentChar.weapons[num].components.Add(new Components(weaponComponent, true));
				Player.TriggerEvent("lprp:addWeaponComponent", weaponName, weaponComponent);
				Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
			}
		}

		public void removeWeaponComponent(string weaponName, string weaponComponent)
		{
			int num = getWeapon(weaponName).Item1;
			Weapons weapon = getWeapon(weaponName).Item2;

			if (weapon != null)
				for (int i = 0; i < CurrentChar.weapons[num].components.Count; i++)
					if (CurrentChar.weapons[num].components[i].name == weaponComponent)
					{
						CurrentChar.weapons[num].components.RemoveAt(i);
						Player.TriggerEvent("lprp:removeWeaponComponent", weaponName, weaponComponent);
						Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, @group);
					}
		}

		public void addWeaponTint(string weaponName, int tint)
		{
			int num = getWeapon(weaponName).Item1;
			Weapons weapon = getWeapon(weaponName).Item2;

			if (weapon != null)
			{
				if (hasWeaponTint(weaponName, tint))
				{
					Player.TriggerEvent("lprp:possiediTinta", weaponName, tint);
				}
				else
				{
					CurrentChar.weapons[num].tint = tint;
					Player.TriggerEvent("lprp:addWeaponTint", weaponName, tint);
					Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
				}
			}
		}

		public bool hasWeapon(string weaponName)
		{
			return CurrentChar.weapons.Any(x => x.name == weaponName);
		}

		public Tuple<int, Weapons> getWeapon(string weaponName)
		{
			Weapons weapon = CurrentChar.weapons.FirstOrDefault(x => x.name == weaponName);

			return weapon != null ? new Tuple<int, Weapons>(CurrentChar.weapons.IndexOf(weapon), weapon) : new Tuple<int, Weapons>(0, null);
		}

		public bool hasWeaponTint(string weaponName, int tint)
		{
			Weapons weapon = getWeapon(weaponName).Item2;

			return weapon != null && weapon.tint == tint;
		}

		public bool hasWeaponComponent(string weaponName, string weaponComponent)
		{
			Weapons weapon = getWeapon(weaponName).Item2;

			return weapon != null && weapon.components.Any(x => x.name == weaponComponent);
		}

		[JsonIgnore] public Vector3 getCoords => CurrentChar.location.position;

		public void giveLicense(string license, string mittente)
		{
			Licenses licenza = new(license, DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss"), mittente);
			CurrentChar.licenze.Add(licenza);
			Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
		}

		public void removeLicense(string license)
		{
			foreach (Licenses licen in CurrentChar.licenze)
				if (licen.name == license)
					CurrentChar.licenze.Remove(licen);
				else
					Log.Printa(LogType.Warning, $"Il player {Player.Name} non ha una licenza con nome '{license}'");
			Player.TriggerEvent("lprp:sendUserInfo", Characters.SerializeToJson(includeEverything: true), char_current, group);
		}

		public List<OwnedVehicle> GetCharVehicles()
		{
			return CurrentChar.Veicoli;
		}

		public void showNotification(string text)
		{
			Player.TriggerEvent("lprp:ShowNotification", text);
		}

		public async Task SalvaPersonaggio()
		{
			try
			{
				await MySQL.ExecuteAsync("UPDATE `users` SET `Name` = @name, `group` = @gr, `group_level` = @level, `playTime` = @time, `char_current` = @current, `char_data` = @data WHERE `discord` = @id", new
				{
					name = Player.Name,
					gr = group,
					level = group_level,
					time = playTime,
					current = char_current,
					data = Characters.SerializeToJson(),
					id = identifiers.Discord
				});
			}
			catch(Exception e)
			{
				Log.Printa(LogType.Error, e.ToString());
			}
			await Task.FromResult(0);
		}

	}

	public class Status
	{
		public bool connected = true;
		public bool spawned = false;
	}

	public class PlayerStateBags
	{
		private Player player;
		public bool InPausa
		{
			get => player.State["PlayerStates"].InPausa;
			set => player.State["PlayerStates"].InPausa = value;
		}

		public bool Svenuto
		{
			get => player.State["PlayerStates"].Svenuto;
			set => player.State["PlayerStates"].Svenuto = value;
		}

		public bool Ammanettato
		{
			get => player.State["PlayerStates"].Ammanettato;
			set => player.State["PlayerStates"].Ammanettato = value;
		}
		public bool InCasa
		{
			get => player.State["PlayerStates"].InCasa;
			set => player.State["PlayerStates"].InCasa = value;
		}
		public bool InServizio
		{
			get => player.State["PlayerStates"].InServizio;
			set => player.State["PlayerStates"].InServizio = value;
		}
		public bool FinDiVita
		{
			get => player.State["PlayerStates"].FinDiVita;
			set => player.State["PlayerStates"].FinDiVita = value;
		}
		public bool AdminSpecta
		{
			get => player.State["PlayerStates"].AdminSpecta;
			set => player.State["PlayerStates"].AdminSpecta = value;
		}
		public bool InVeicolo
		{
			get => player.State["PlayerStates"].InVeicolo;
			set => player.State["PlayerStates"].InVeicolo = value;
		}
		public Istanza Istanza;

		public PlayerStateBags(Player pl)
		{
			player = pl;
			Istanza = new Istanza(pl);
			var baseBag = new
			{
				InPausa = false,
				Svenuto = false,
				Istanza = new { Stanziato = false, ServerIdProprietario = 0, IsProprietario = false, Instance = string.Empty },
				Ammanettato = false,
				InCasa = false,
				InServizio = false,
				FinDiVita = false,
				AdminSpecta = false,
				InVeicolo = false
			};
			player.State.Set("PlayerStates", baseBag, true);
		}
	}

	public class Istanza
	{
		private Player player;

		public Istanza(Player pl)
		{
			player = pl;
		}

		public bool Stanziato
		{
			get => player.State["PlayerStates"].Istanza.Stanziato;
			set => player.State["PlayerStates"].Istanza.Stanziato = value;
		}
		public int ServerIdProprietario
		{
			get => player.State["PlayerStates"].Istanza.ServerIdProprietario;
			set => player.State["PlayerStates"].Istanza.ServerIdProprietario = value;
		}
		public bool IsProprietario
		{
			get => player.State["PlayerStates"].Istanza.IsProprietario;
			set => player.State["PlayerStates"].Istanza.IsProprietario = value;
		}

		public string Instance
		{
			get => player.State["PlayerStates"].Istanza.Instance;
			set => player.State["PlayerStates"].Istanza.Instance = value;
		}

		/// <summary>
		/// Istanza generica
		/// </summary>
		public void Istanzia()
		{
			Stanziato = true;
			ServerIdProprietario = Convert.ToInt32(player.Handle);
			IsProprietario = true;
			Instance = string.Empty;
		}

		/// <summary>
		/// Istanza generica specificando quale Istanza
		/// </summary>
		public void Istanzia(string Instance)
		{
			Stanziato = true;
			ServerIdProprietario = Convert.ToInt32(player.Handle);
			IsProprietario = true;
			this.Instance = Instance;
		}

		/// <summary>
		/// Istanza specifica
		/// </summary>
		public void Istanzia(int ServerId, string Instance)
		{
			Stanziato = true;
			ServerIdProprietario = ServerId;
			IsProprietario = true;
			this.Instance = Instance;
		}

		/// <summary>
		/// Rimuovi da istanza
		/// </summary>
		public void RimuoviIstanza()
		{
			Stanziato = false;
			ServerIdProprietario = 0;
			IsProprietario = false;
			Instance = string.Empty;
		}

		/// <summary>
		/// Cambia Istanza con una nuova (es. casa e garage)
		/// </summary>
		/// <param name="instance">Specifica quale istanza</param>
		public void CambiaIstanza(string instance)
		{
			if (Stanziato)
				if (Instance != instance)
					Instance = instance;
		}

		/// <summary>
		/// Cambia Proprietario dell'istanza
		/// </summary>
		/// <param name="netId">networkId del proprietario</param>
		public void CambiaIstanza(int netId)
		{
			if (Stanziato)
				if (ServerIdProprietario != netId)
					ServerIdProprietario = netId;
		}
	}
}