using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Shared.Veicoli;
using System.Threading.Tasks;

namespace TheLastPlanet.Server.Core
{
	public class User
	{
		[JsonIgnore]
		public string source;
		public string group;
		public int group_level;
		public int char_current;
		public long playTime;
		public DateTime lastConnection;
		public Identifiers identifiers = new Identifiers();
		public Status status = new Status();
		[JsonIgnore]
		public PlayerStateBags StatiPlayer;
		public List<Char_data> char_data = new List<Char_data>();
		public User() { }
		[JsonIgnore]
		public Player p;
		public User(Player player, dynamic result)
		{

			identifiers.steam = License.GetLicense(player, Identifier.Steam);
			identifiers.license = License.GetLicense(player, Identifier.License);
			identifiers.discord = License.GetLicense(player, Identifier.Discord);
			identifiers.fivem = License.GetLicense(player, Identifier.Fivem);
			identifiers.ip = License.GetLicense(player, Identifier.Ip);
			lastConnection = DateTime.Now;
			source = player.Handle;
			char_current = result.char_current;
			group = result.group;
			group_level = result.group_level;
			playTime = result.playTime;
			p = player;
			StatiPlayer = new PlayerStateBags(player);
			char_data = (result.char_data as string).Deserialize<List<Char_data>>();
		}

		public User(dynamic result)
		{
			lastConnection = DateTime.Now;
			//source = player.Handle;
			char_current = result.char_current;
			group = result.group;
			group_level = result.group_level;
			playTime = result.playTime;
			//p = player;
			char_data = (result.char_data as string).Deserialize<List<Char_data>>();
		}

		[JsonIgnore]
		public Char_data CurrentChar
		{
			get
			{
				return char_data.FirstOrDefault(x => x.id - 1 == char_current - 1);
			}
		}

		[JsonIgnore]
		public string FullName
		{
			get { return CurrentChar.info.firstname + " " + CurrentChar.info.lastname; }
		}

		[JsonIgnore]
		public string DOB
		{
			get { return CurrentChar.info.dateOfBirth; }
		}

		[JsonIgnore]
		public bool DeathStatus
		{
			get { return CurrentChar.is_dead; }
			set { CurrentChar.is_dead = value; }
		}

		[JsonIgnore]
		public int Money
		{
			get { return CurrentChar.finance.money; }
			set
			{
				int var = value - CurrentChar.finance.money;
				CurrentChar.finance.money += var;
				if (var < 0)
				{
					if (CurrentChar.finance.money < 0)
						CurrentChar.finance.money = 0;
				}
				p.TriggerEvent("lprp:changeMoney", var);
				p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
			}
		}

		[JsonIgnore]
		public int Bank
		{
			get { return CurrentChar.finance.bank; }
			set
			{
				int var = value - CurrentChar.finance.bank;
				CurrentChar.finance.bank += var;
				if (var < 0)
					p.TriggerEvent("lprp:rimuoviBank", var);
				p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
			}
		}

		[JsonIgnore]
		public int DirtyMoney
		{
			get { return CurrentChar.finance.dirtyCash; }
			set
			{
				int var = value - CurrentChar.finance.dirtyCash;
				CurrentChar.finance.dirtyCash += var;
				if (var < 0)
				{
					if (CurrentChar.finance.dirtyCash < 0)
						CurrentChar.finance.dirtyCash = 0;
				}
				p.TriggerEvent("lprp:changeDirty", var);
				p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
			}
		}

		public void SetJob(string job, int grade)
		{
			CurrentChar.job.name = job;
			CurrentChar.job.grade = grade;
			p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
		}

		public void SetGang(string job, int grade)
		{
			CurrentChar.gang.name = job;
			CurrentChar.gang.grade = grade;
			p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
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
			for (int i = 0; i < char_data.Count; i++)
				if (char_data[i].id == charId)
					return char_data[i].inventory;
			return null;
		}

		public void addInventoryItem(string item, int amount, float weight)
		{
			bool vero = getInventoryItem(item).Item1;
			var checkedItem = getInventoryItem(item).Item2;
			if (vero)
			{
				checkedItem.amount += amount;
				if (checkedItem.amount == ConfigShared.SharedConfig.Main.Generici.ItemList[item].max)
				{
					checkedItem.amount = ConfigShared.SharedConfig.Main.Generici.ItemList[item].max;
					p.TriggerEvent("lprp:ShowNotification", "HAI GIA' IL MASSIMO DI ~w~" + ConfigShared.SharedConfig.Main.Generici.ItemList[item].label + "~w~!");

				}
			}
			else
				CurrentChar.inventory.Add(new Inventory(item, amount, weight));
			p.TriggerEvent("lprp:ShowNotification", "Hai ricevuto " + amount + " " + ConfigShared.SharedConfig.Main.Generici.ItemList[item].label + "!");
			p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
		}

		public void removeInventoryItem(string item, int amount)
		{
			bool vero = getInventoryItem(item).Item1;
			var checkedItem = getInventoryItem(item).Item2;

			if (vero)
			{
				checkedItem.amount -= amount;
				if (checkedItem.amount <= 0)
					CurrentChar.inventory.Remove(checkedItem);
			}
			else
				CurrentChar.inventory.ToList().Remove(checkedItem);

			p.TriggerEvent("lprp:ShowNotification", amount + " " + ConfigShared.SharedConfig.Main.Generici.ItemList[item].label + " ti sono stati rimossi/e!");
			p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
		}

		public List<Weapons> getCharWeapons(int charId)
		{
			for (int i = 0; i < char_data.Count; i++)
			{
				if (char_data[i].id == charId)
				{
					return char_data[i].weapons;
				}
			}
			return null;
		}

		public void addWeapon(string weaponName, int ammo)
		{
			if (!hasWeapon(weaponName))
			{
				CurrentChar.weapons.Add(new Weapons(weaponName, ammo, new List<Components>(), 0));
				p.TriggerEvent("lprp:addWeapon", weaponName, ammo);
				p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
			}
		}

		public void updateWeaponAmmo(string weaponName, int ammo)
		{
			var weapon = getWeapon(weaponName);
			if (weapon.Item2.ammo > ammo)
				CurrentChar.weapons[weapon.Item1].ammo = ammo;
		}

		public void removeWeapon(string weaponName)
		{
			Log.Printa(LogType.Debug, "index = " + getWeapon(weaponName).Item1);
			Log.Printa(LogType.Debug, JsonConvert.SerializeObject(getWeapon(weaponName).Item2));
			if (hasWeapon(weaponName))
			{
				CurrentChar.weapons.Remove(getWeapon(weaponName).Item2);
				p.TriggerEvent("lprp:removeWeapon", weaponName);
				p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
			}
		}

		public void addWeaponComponent(string weaponName, string weaponComponent)
		{
			int num = getWeapon(weaponName).Item1;
			if (hasWeaponComponent(weaponName, weaponComponent))
				p.TriggerEvent("lprp:possiediArma", weaponName, weaponComponent);
			else
			{
				CurrentChar.weapons[num].components.Add(new Components(weaponComponent, true));
				p.TriggerEvent("lprp:addWeaponComponent", weaponName, weaponComponent);
				p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
			}
		}

		public void removeWeaponComponent(string weaponName, string weaponComponent)
		{
			int num = getWeapon(weaponName).Item1; Weapons weapon = getWeapon(weaponName).Item2;
			if (weapon != null)
			{
				for (int i = 0; i < CurrentChar.weapons[num].components.Count; i++)
				{
					if (CurrentChar.weapons[num].components[i].name == weaponComponent)
					{
						CurrentChar.weapons[num].components.RemoveAt(i);
						p.TriggerEvent("lprp:removeWeaponComponent", weaponName, weaponComponent);
						p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
					}
				}
			}
		}

		public void addWeaponTint(string weaponName, int tint)
		{
			int num = getWeapon(weaponName).Item1; Weapons weapon = getWeapon(weaponName).Item2;
			if (weapon != null)
			{
				if (hasWeaponTint(weaponName, tint))
					p.TriggerEvent("lprp:possiediTinta", weaponName, tint);
				else
				{
					CurrentChar.weapons[num].tint = tint;
					p.TriggerEvent("lprp:addWeaponTint", weaponName, tint);
					p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
				}
			}
		}

		public bool hasWeapon(string weaponName)
		{
			return CurrentChar.weapons.Any(x => x.name == weaponName);
		}

		public Tuple<int, Weapons> getWeapon(string weaponName)
		{
			var weapon = CurrentChar.weapons.FirstOrDefault(x => x.name == weaponName);
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

		[JsonIgnore]
		public Vector3 getCoords { get { return CurrentChar.location.position; } }

		public void giveLicense(string license, string mittente)
		{
			Licenses licenza = new Licenses(license, DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss"), mittente);
			CurrentChar.licenze.Add(licenza);
			p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
		}

		public void removeLicense(string license)
		{
			foreach (var licen in CurrentChar.licenze)
				if (licen.name == license)
					CurrentChar.licenze.Remove(licen);
				else Log.Printa(LogType.Warning, $"Il player {p.Name} non ha una licenza con nome '{license}'");
			p.TriggerEvent("lprp:sendUserInfo", char_data.Serialize(includeEverything: true), char_current, group);
		}

		public List<OwnedVehicle> GetCharVehicles()
		{
			return CurrentChar.Veicoli;
		}

		public void showNotification(string text)
		{
			p.TriggerEvent("lprp:ShowNotification", text);
		}
	}

	public class Identifiers
	{
		public string steam;
		public string license;
		public string discord;
		public string fivem;
		public string ip;
	}

	public class Status
	{
		public bool connected = true;
		public bool spawned = false;
	}

	public class PlayerStateBags
	{
		Player player;
		public bool InPausa
		{
			get
			{
				return player.State["PlayerStates"].InPausa;
			}
			set
			{
				player.State["PlayerStates"].InPausa = value;
			}
		}
		public bool Ammanettato
		{
			get
			{
				return player.State["PlayerStates"].Ammanettato;
			}
			set
			{
				player.State["PlayerStates"].Ammanettato = value;
			}
		}
		public bool InCasa
		{
			get
			{
				return player.State["PlayerStates"].InCasa;
			}
			set
			{
				player.State["PlayerStates"].InCasa = value;
			}
		}
		public bool InServizio
		{
			get
			{
				return player.State["PlayerStates"].InServizio;
			}
			set
			{
				player.State["PlayerStates"].InServizio = value;
			}
		}
		public bool FinDiVita
		{
			get
			{
				return player.State["PlayerStates"].FinDiVita;
			}
			set
			{
				player.State["PlayerStates"].FinDiVita = value;
			}
		}
		public bool AdminSpecta
		{
			get
			{
				return player.State["PlayerStates"].AdminSpecta;
			}
			set
			{
				player.State["PlayerStates"].AdminSpecta = value;
			}
		}
		public Istanza Istanza;

		public PlayerStateBags(Player pl)
		{
			player = pl;
			Istanza = new Istanza(pl);
			var baseBag = new
			{
				InPausa = false,
				Istanza = new
				{
					Stanziato = false,
					ServerIdProprietario = 0,
					IsProprietario = false,
					Instance = string.Empty,
				},
				Ammanettato = false,
				InCasa = false,
				InServizio = false,
				FinDiVita = false,
				AdminSpecta = false,
			};
			player.State.Set("PlayerStates", baseBag, true);
		}
	}
	
	public class Istanza
	{
		Player player;
		public Istanza(Player pl)
		{
			player = pl;
		}
		public bool Stanziato
		{
			get
			{
				return player.State["PlayerStates"].Istanza.Stanziato;
			}
			set
			{
				player.State["PlayerStates"].Istanza.Stanziato = value;
			}
		}
		public int ServerIdProprietario
		{
			get
			{
				return player.State["PlayerStates"].Istanza.ServerIdProprietario;
			}
			set
			{
				player.State["PlayerStates"].Istanza.ServerIdProprietario = value;
			}
		}
		public bool IsProprietario
		{
			get
			{
				return player.State["PlayerStates"].Istanza.IsProprietario;
			}
			set
			{
				player.State["PlayerStates"].Istanza.IsProprietario = value;
			}
		}

		public string Instance
		{
			get
			{
				return player.State["PlayerStates"].Istanza.Instance;
			}
			set
			{
				player.State["PlayerStates"].Istanza.Instance = value;
			}
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