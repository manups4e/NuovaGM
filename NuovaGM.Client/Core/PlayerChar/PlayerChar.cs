using CitizenFX.Core;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core.Native;

namespace TheLastPlanet.Client.Core.Personaggio
{
	public class PlayerChar
	{
		public int source;
		private int playerId;
		public string group;
		public UserGroup group_level;
		public int char_current;
		public long playTime;
		public Identifiers identifiers;
		public string lastConnection;
		public Status status = new Status();
		[JsonIgnore]
		public PlayerStateBags StatiPlayer = new PlayerStateBags();
		public List<Char_data> char_data = new List<Char_data>();
		public dynamic data;
		public Vector4 posizione = Vector4.Zero;
		public PlayerChar() { }
		public PlayerChar(dynamic result)
		{
			char_current = result.char_current;
			lastConnection = DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss");
			source = Game.Player.ServerId;
			group = result.group;
			group_level = (UserGroup)result.group_level;
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

		[JsonIgnore]
		public List<Inventory> Inventory { get { return getCharInventory(); } }

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

		private List<Inventory> getCharInventory()
		{
			return getCharInventory(char_current);
		}
		private List<Inventory> getCharInventory(int charId)
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

	public class PlayerStateBags
	{
		[JsonIgnore]
		Player player = Game.Player;
		public bool InPausa
		{
			get
			{
				return player.State["PlayerStates"].InPausa;
			}
			set
			{
				var p = player.State["PlayerStates"];
				p.InPausa = value;
				player.State.Set("PlayerStates", p, true);
			}
		}

		public bool Svenuto
		{
			get
			{
				return player.State["PlayerStates"].Svenuto;
			}
			set
			{
				var p = player.State["PlayerStates"];
				p.Svenuto = value;
				player.State.Set("PlayerStates", p, true);
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
				var p = player.State["PlayerStates"];
				p.Ammanettato = value;
				player.State.Set("PlayerStates", p, true);
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
				var p = player.State["PlayerStates"];
				p.InCasa = value;
				player.State.Set("PlayerStates", p, true);
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
				var p = player.State["PlayerStates"];
				p.InServizio = value;
				player.State.Set("PlayerStates", p, true);
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
				var p = player.State["PlayerStates"];
				p.FinDiVita = value;
				player.State.Set("PlayerStates", p, true);
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
				var p = player.State["PlayerStates"];
				p.AdminSpecta = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public Istanza Istanza = new Istanza();

	}
	public class Istanza
	{
		[JsonIgnore]
		Player player = Game.Player;
		public bool Stanziato { 
			get 
			{
				return player.State["PlayerStates"].Istanza.Stanziato;
			}
			set 
			{
				var p = player.State["PlayerStates"];
				p.Istanza.Stanziato = value;
				player.State.Set("PlayerStates", p, true);
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
				var p = player.State["PlayerStates"];
				p.Istanza.ServerIdProprietario = value;
				player.State.Set("PlayerStates", p, true);
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
				var p = player.State["PlayerStates"];
				p.Istanza.IsProprietario = value;
				player.State.Set("PlayerStates", p, true);
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
				var p = player.State["PlayerStates"];
				p.Istanza.Instance = value;
				player.State.Set("PlayerStates", p, true);
			}
		}

		/// <summary>
		/// Istanza generica
		/// </summary>
		public void Istanzia()
		{
			Stanziato = true;
			ServerIdProprietario = player.ServerId;
			IsProprietario = true;
			Instance = string.Empty;
		}
		/// <summary>
		/// Istanza generica specificando quale Istanza
		/// </summary>
		public void Istanzia(string Instance)
		{
			Stanziato = true;
			ServerIdProprietario = player.ServerId;
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
			if(Stanziato)
				if(Instance != instance)
					Instance = instance;
		}
		/// <summary>
		/// Cambia Proprietario dell'istanza
		/// </summary>
		/// <param name="netId">networkId del proprietario</param>
		public void CambiaIstanza(int netId)
		{
			if (Stanziato)
				if(ServerIdProprietario != netId)
					ServerIdProprietario = netId;
		}
	}
}