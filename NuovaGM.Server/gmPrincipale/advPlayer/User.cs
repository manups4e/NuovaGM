using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using NuovaGM.Shared;

namespace NuovaGM.Server.gmPrincipale
{
	public class User
	{
		public string source;
		public string group;
		public int group_level;
		public int char_current;
		public long playTime;
		public DateTime lastConnection;
		public Identifiers identifiers = new Identifiers();
		public Status status = new Status();
		public List<Char_data> char_data = new List<Char_data>();
		public JContainer data;
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
			data = JsonConvert.DeserializeObject<JContainer>(result.char_data);

			for (int i = 0; i < data.Count; i++)
				char_data.Add(new Char_data(data[i] as JContainer));
		}

		[JsonIgnore]
		public Char_data CurrentChar
		{
			get
			{
				return char_data.FirstOrDefault(x => x.id - 1 == char_current - 1);
				/*
				for (int i = 0; i < char_data.Count; i++)
				{
					if ((char_current - 1) == char_data[i].id - 1)
					{
						return char_data[i];
					}
				}
				return null;
				*/
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
				if (var > 0)
					p.TriggerEvent("lprp:ShowNotification", "~g~$" + var + " ~w~ricevuti!");
				else if (var < 0)
				{
					if (CurrentChar.finance.money < 0)
						CurrentChar.finance.money = 0;
					p.TriggerEvent("lprp:ShowNotification", "~r~$" + (var * -1) + " ~w~rimossi!");
				}
				p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
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
				if (var > 0)
					p.TriggerEvent("lprp:ShowNotification", "~g~$" + var + " ~w~ricevuti in banca!");
				else if (var < 0)
					p.TriggerEvent("lprp:ShowNotification", "~r~$" + (var * -1) + " ~w~rimossi dalla banca!");
				p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
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
				if (var > 0)
					p.TriggerEvent("lprp:ShowNotification", "~g~$" + var + " ~w~sporchi ricevuti!");
				else if (var < 0)
				{
					if (CurrentChar.finance.dirtyCash < 0)
						CurrentChar.finance.dirtyCash = 0;
					p.TriggerEvent("lprp:ShowNotification", "~r~$" + (var * -1) + " ~w~ sporchi rimossi!");
				}
				p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
			}
		}

		public void SetJob(string job, int grade)
		{
			CurrentChar.job.name = job;
			CurrentChar.job.grade = grade;
			p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
		}

		public void SetGang(string job, int grade)
		{
			CurrentChar.gang.name = job;
			CurrentChar.gang.grade = grade;
			p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
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
				if (checkedItem.amount == SharedScript.ItemList[item].max)
				{
					checkedItem.amount = SharedScript.ItemList[item].max;
					p.TriggerEvent("lprp:ShowNotification", "HAI GIA' IL MASSIMO DI ~w~" + SharedScript.ItemList[item].label + "~w~!");

				}
			}
			else
				CurrentChar.inventory.Add(new Inventory(item, amount, weight));
			p.TriggerEvent("lprp:ShowNotification", "Hai ricevuto " + amount + " " + SharedScript.ItemList[item].label + "!");
			p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
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
				CurrentChar.inventory.Remove(checkedItem);

			p.TriggerEvent("lprp:ShowNotification", amount + " " + SharedScript.ItemList[item].label + " ti sono stati rimossi/e!");
			p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
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
				p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
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
			Weapons weapon = getWeapon(weaponName).Item2;
			if (weapon != null)
				CurrentChar.weapons.Remove(weapon);
			p.TriggerEvent("lprp:removeWeapon", weaponName);
			p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
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
				p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
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
						p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
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
					p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
				}
			}
		}

		public bool hasWeapon(string weaponName)
		{
			return CurrentChar.weapons.Any(x => x.name == weaponName);
			/*
			for (int i = 0; i < CurrentChar.weapons.Count; i++)
			{
				if (CurrentChar.weapons[i].name == weaponName)
				{
					return true;
				}
			}
			return false;
			*/
			}

		public Tuple<int, Weapons> getWeapon(string weaponName)
		{
			for (int i = 0; i < CurrentChar.weapons.Count; i++)
				if (CurrentChar.weapons[i].name == weaponName)
					return new Tuple<int, Weapons>(i, CurrentChar.weapons[i]);
			return new Tuple<int, Weapons>(0, null);
		}

		public bool hasWeaponTint(string weaponName, int tint)
		{
			Weapons weapon = getWeapon(weaponName).Item2;
			if (weapon == null)
				return false;

			return weapon.tint == tint;
			/*
			if (weapon.tint == tint)
			{
				Debug.WriteLine("arma = " + weapon.tint + ", tint parametro = " + tint);
				return true;
			}
			return false;
			*/
		}

		public bool hasWeaponComponent(string weaponName, string weaponComponent)
		{
			Weapons weapon = getWeapon(weaponName).Item2;
			if (weapon == null)
				return false;

			return weapon.components.Any(x => x.name == weaponComponent);
			/*
			for (int i = 0; i < weapon.components.Count; i++)
			{
				if (weapon.components[i].name == weaponComponent)
				{
					return true;
				}
			}
			return false;
			*/
		}

		[JsonIgnore]
		public Vector3 getCoords { get { return new Vector3(CurrentChar.location.x, CurrentChar.location.y, CurrentChar.location.z); } }

		public void giveLicense(string license, string mittente)
		{
			Licenses licenza = new Licenses(license, DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss"), mittente);
			CurrentChar.licenze.Add(licenza);
			p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
		}

		public void removeLicense(string license)
		{
			foreach (var licen in CurrentChar.licenze)
				if (licen.name == license)
					CurrentChar.licenze.Remove(licen);
				else Server.Printa(LogType.Warning, $"Il player {p.Name} non ha una licenza con nome '{license}'");
			p.TriggerEvent("lprp:sendUserInfo", JsonConvert.SerializeObject(char_data), char_current, group);
		}

		public void showNotification(string text)
		{
			p.TriggerEvent("lprp:showNotification", text);
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
}