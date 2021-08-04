using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Impostazioni.Shared.Configurazione.Generici;
using Newtonsoft.Json;
using TheLastPlanet.Client.Cache;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events.Attributes;
using TheLastPlanet.Shared.PlayerChar;

namespace TheLastPlanet.Client.Core.PlayerChar
{
	[Serialization]
	public partial class User : BasePlayerShared
	{
		[Ignore]
		[JsonIgnore]
		public Position Posizione
		{
			get => CurrentChar.Posizione;
			set
			{
				CurrentChar.Posizione = new(value.X, value.Y, value.Z, value.Heading);
			}
		}

		public User(BasePlayerShared result)
		{
			ID = result.ID;
			PlayerID = result.PlayerID;
			group = result.group;
			group_level = result.group_level;
			playTime = result.playTime;
			Player = Game.Player;
			char_data = result.char_data;
			Identifiers = result.Identifiers;
			Status = new(Game.Player);
		}

		public User() { }

		[Ignore] [JsonIgnore] public string FullName => CurrentChar.Info.firstname + " " + CurrentChar.Info.lastname;

		[Ignore] [JsonIgnore] public string DoB => CurrentChar.Info.dateOfBirth;

		[Ignore] [JsonIgnore] public bool DeathStatus => CurrentChar.is_dead;

		[Ignore] [JsonIgnore] public int Money => CurrentChar.Finance.Money;

		[Ignore] [JsonIgnore] public int Bank => CurrentChar.Finance.Bank;

		[Ignore] [JsonIgnore] public int DirtyCash => CurrentChar.Finance.DirtyCash;

		[Ignore] [JsonIgnore] public List<Inventory> Inventory => GetCharInventory();

		public Tuple<bool, Inventory, Item> GetInventoryItem(string item)
		{
			foreach (Inventory t in CurrentChar.Inventory.Where(t => t.Item == item)) return new Tuple<bool, Inventory, Item>(true, t, ConfigShared.SharedConfig.Main.Generici.ItemList[item]);
			return new Tuple<bool, Inventory, Item>(false, null, null);
		}

		public List<Inventory> GetCharInventory()
		{
			return CurrentChar.Inventory;
		}

		public List<Weapons> GetCharWeapons()
		{
			return CurrentChar.Weapons;
		}

		public bool HasWeapon(string weaponName)
		{
			return CurrentChar.Weapons.Any(x => x.name == weaponName);
		}

		public bool HasWeapon(WeaponHash weaponName)
		{
			return CurrentChar.Weapons.Any(x => Funzioni.HashInt(x.name) == (int)weaponName);
		}

		public Tuple<int, Weapons> GetWeapon(string weaponName)
		{
			for (int i = 0; i < CurrentChar.Weapons.Count; i++)
				if (CurrentChar.Weapons[i].name == weaponName)
					return new Tuple<int, Weapons>(i, CurrentChar.Weapons[i]);

			return new Tuple<int, Weapons>(0, null);
		}

		public bool HasWeaponTint(string weaponName, int tint)
		{
			Weapons weapon = GetWeapon(weaponName).Item2;

			return weapon != null && weapon.tint == tint;
		}

		public bool HasWeaponComponent(string weaponName, string weaponComponent)
		{
			Weapons weapon = GetWeapon(weaponName).Item2;

			return weapon != null && weapon.components.Any(x => x.name == weaponComponent);
		}

		public bool HasLicense(string license)
		{
			return CurrentChar.Licenze.Any(x => x.name == license);
		}
	}
}