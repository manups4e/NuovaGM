using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NuovaGM.Client.gmPrincipale.Personaggio
{
	public class PlayerChar
	{
		public int source;
		private int playerId;
		public string group;
		public int group_level;
		public int char_current;
		public long playTime;
		public Identifiers identifiers;
		public bool ammanettato;
		public bool InCasa;
		public bool InServizio;
		public bool Stanziato;
		public string lastConnection;
		public Status status = new Status();
		public List<Char_data> char_data = new List<Char_data>();
		public dynamic data;
		public PlayerChar() { }
		public PlayerChar(JObject result)
		{
			char_current = result.Value<int>("char_current");
			lastConnection = DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss");
			source = 0;
			group = result.Value<string>("group");
			group_level = result.Value<int>("group_level");
			playTime = result.Value<long>("playTime");
			data = JsonConvert.DeserializeObject<JContainer>(result.Value<string>("char_data"));
			status = new Status();
			ammanettato = false;
			InCasa = false;
			InServizio = false;
			Stanziato = false;
			playerId = Game.Player.Handle;
			for (int i = 0; i < data.Count; i++)
			{
				char_data.Add(new Char_data(data[i] as JContainer));
			}
		}
		public PlayerChar(dynamic result)
		{
			char_current = result.char_current;
			lastConnection = DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss");
			source = Game.Player.ServerId;
			group = result.group;
			group_level = result.group_level;
			playTime = result.playTime;
			data = JsonConvert.DeserializeObject<JContainer>(result.char_data);
			status = new Status();

			for (int i = 0; i < data.Count; i++)
			{
				char_data.Add(new Char_data(data[i] as JContainer));
			}
		}

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
		public string FullName { get { return CurrentChar.info.firstname + " " + CurrentChar.info.lastname; } }

		[JsonIgnore]
		public string DOB { get { return CurrentChar.info.dateOfBirth; } }

		[JsonIgnore]
		public bool DeathStatus { get { return CurrentChar.is_dead; } }

		[JsonIgnore]
		public int Money { get { return CurrentChar.finance.cash; } }

		[JsonIgnore]
		public int Bank { get { return CurrentChar.finance.bank; } }

		[JsonIgnore]
		public int DirtyMoney { get { return CurrentChar.finance.dirtyCash; } }

		public Tuple<bool, Inventory, Item> getInventoryItem(string item)
		{
			for (int i = 0; i < CurrentChar.inventory.Count; i++)
			{
				if (CurrentChar.inventory[i].item == item)
				{
					return new Tuple<bool, Inventory, Item>(true, CurrentChar.inventory[i], SharedScript.ItemList[item]);
				}
			}
			return new Tuple<bool, Inventory, Item>(false, null, null);
		}

		public List<Inventory> getCharInventory(int charId)
		{
			for (int i = 0; i < char_data.Count; i++)
			{
				if (char_data[i].id == charId)
				{
					return char_data[i].inventory;
				}
			}
			return null;
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


		public bool hasWeapon(string weaponName)
		{
			return CurrentChar.weapons.Any(x => x.name == weaponName);
		}

		public Tuple<int, Weapons> getWeapon(string weaponName)
		{
			for (int i = 0; i < CurrentChar.weapons.Count; i++)
			{
				if (CurrentChar.weapons[i].name == weaponName)
				{
					return new Tuple<int, Weapons>(i, CurrentChar.weapons[i]);
				}
			}
			return new Tuple<int, Weapons>(0, null);
		}

		public bool hasWeaponTint(string weaponName, int tint)
		{
			Weapons weapon = getWeapon(weaponName).Item2;
			if (weapon == null)
			{
				return false;
			}

			return weapon.tint == tint;
		}

		public bool hasWeaponComponent(string weaponName, string weaponComponent)
		{
			Weapons weapon = getWeapon(weaponName).Item2;
			if (weapon == null)
			{
				return false;
			}

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

		public bool hasLicense(string license)
		{
			return CurrentChar.licenze.Any(x => x.name == license);
		}
	}

	public class Identifiers
	{
		public string steam;
		public string license;
		public string discord;
	}

	public class Status
	{
		public bool connected = true;
		public bool spawned = false;
	}
}