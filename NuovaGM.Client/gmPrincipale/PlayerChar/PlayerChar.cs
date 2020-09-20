using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Utility;
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
		public string lastConnection;
		public Status status = new Status();
		public stanziato Istanza = new stanziato();
		public List<Char_data> char_data = new List<Char_data>();
		public dynamic data;
		public PlayerChar() { }
		public PlayerChar(dynamic result)
		{
			char_current = result.char_current;
			lastConnection = DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss");
			source = Game.Player.ServerId;
			group = result.group;
			group_level = result.group_level;
			playTime = result.playTime;
			char_data = (result.char_data as string).Deserialize<List<Char_data>>();
			status = new Status();
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
		public int Money { get { return CurrentChar.finance.money; } }

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
					return new Tuple<bool, Inventory, Item>(true, CurrentChar.inventory[i], ConfigShared.SharedConfig.Main.Generici.ItemList[item]);
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

		public bool hasWeapon(WeaponHash weaponName)
		{
			return CurrentChar.weapons.Any(x => Funzioni.HashInt(x.name) == (int)weaponName);
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

	public class stanziato
	{
		public bool Stanziato;
		public int NetIdProprietario;
		public bool IsProprietario;
		public string Instance;
		/// <summary>
		/// Istanza generica
		/// </summary>
		public void Istanzia()
		{
			Game.PlayerPed.SetDecor("PlayerStanziato", true);
			Game.PlayerPed.SetDecor("PlayerStanziatoInIstanza", Game.PlayerPed.NetworkId);
			Stanziato = true;
			NetIdProprietario = Game.PlayerPed.NetworkId;
			IsProprietario = true;
			Instance = "null";
			BaseScript.TriggerServerEvent("lprp:istanzia", Stanziato, NetIdProprietario, IsProprietario, Instance);
		}
		/// <summary>
		/// Istanza generica specificando quale Istanza
		/// </summary>
		public void Istanzia(string Instance)
		{
			Game.PlayerPed.SetDecor("PlayerStanziato", true);
			Game.PlayerPed.SetDecor("PlayerStanziatoInIstanza", Game.PlayerPed.NetworkId);
			Stanziato = true;
			NetIdProprietario = Game.PlayerPed.NetworkId;
			IsProprietario = true;
			this.Instance = Instance;
			BaseScript.TriggerServerEvent("lprp:istanzia", Stanziato, NetIdProprietario, IsProprietario, this.Instance);
		}
		/// <summary>
		/// Istanza specifica
		/// </summary>
		public void Istanzia(int NetIdProprietario, string Instance)
		{
			Game.PlayerPed.SetDecor("PlayerStanziato", true);
			Stanziato = true;
			var propr = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.Character.NetworkId == NetIdProprietario).Character;
			Game.PlayerPed.SetDecor("PlayerStanziatoInIstanza", propr.NetworkId);
			this.NetIdProprietario = NetIdProprietario;
			IsProprietario = false;
			this.Instance = Instance;
			BaseScript.TriggerServerEvent("lprp:istanzia", Stanziato, this.NetIdProprietario, IsProprietario, this.Instance);
		}

		/// <summary>
		/// Rimuovi da istanza
		/// </summary>
		public void RimuoviIstanza()
		{
			Game.PlayerPed.SetDecor("PlayerStanziato", false);
			Game.PlayerPed.SetDecor("PlayerStanziatoInIstanza", 0);
			Stanziato = false;
			NetIdProprietario = 0;
			IsProprietario = false;
			Instance = null;
			BaseScript.TriggerServerEvent("lprp:rimuoviIstanza");
		}
	}
}