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
		[JsonIgnore] public PlayerStateBags StatiPlayer = new PlayerStateBags();
		public List<Char_data> char_data = new List<Char_data>();
		public dynamic data;
		public Vector4 posizione = Vector4.Zero;
		public PlayerChar() { }

		public PlayerChar(dynamic result)
		{
			char_current = result.char_current;
			lastConnection = DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss");
			source = Cache.Player.ServerId;
			group = result.group;
			group_level = (UserGroup)result.group_level;
			playTime = result.playTime;
			char_data = (result.char_data as string).Deserialize<List<Char_data>>();
			status = new Status();
		}

		public Char_data CurrentChar => char_data.FirstOrDefault(x => x.id - 1 == char_current - 1);

		[JsonIgnore] public string FullName => CurrentChar.info.firstname + " " + CurrentChar.info.lastname;

		[JsonIgnore] public string DoB => CurrentChar.info.dateOfBirth;

		[JsonIgnore] public bool DeathStatus => CurrentChar.is_dead;

		[JsonIgnore] public int Money => CurrentChar.finance.money;

		[JsonIgnore] public int Bank => CurrentChar.finance.bank;

		[JsonIgnore] public int DirtyMoney => CurrentChar.finance.dirtyCash;

		[JsonIgnore] public List<Inventory> Inventory => getCharInventory();

		public Tuple<bool, Inventory, Item> getInventoryItem(string item)
		{
			foreach (Inventory t in CurrentChar.inventory)
				if (t.item == item)
					return new Tuple<bool, Inventory, Item>(true, t, ConfigShared.SharedConfig.Main.Generici.ItemList[item]);

			return new Tuple<bool, Inventory, Item>(false, null, null);
		}

		private List<Inventory> getCharInventory() { return getCharInventory(char_current); }

		private List<Inventory> getCharInventory(int charId) { return (from t in char_data where t.id == charId select t.inventory).FirstOrDefault(); }

		public List<Weapons> getCharWeapons(int charId) { return (from t in char_data where t.id == charId select t.weapons).FirstOrDefault(); }

		public bool hasWeapon(string weaponName) { return CurrentChar.weapons.Any(x => x.name == weaponName); }

		public bool hasWeapon(WeaponHash weaponName) { return CurrentChar.weapons.Any(x => Funzioni.HashInt(x.name) == (int)weaponName); }

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

			return weapon != null && weapon.tint == tint;
		}

		public bool hasWeaponComponent(string weaponName, string weaponComponent)
		{
			Weapons weapon = getWeapon(weaponName).Item2;

			return weapon != null && weapon.components.Any(x => x.name == weaponComponent);
		}

		public bool hasLicense(string license) { return CurrentChar.licenze.Any(x => x.name == license); }
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
		[JsonIgnore] private Player player = Cache.Player;

		private bool _inPausa;
		private bool _svenuto;
		private bool _ammanettato;
		private bool _inCasa;
		private bool _inServizio;
		private bool _finDiVita;
		private bool _adminSpecta;
		private bool _inVeh;

		public bool InPausa
		{
			get => _inPausa;
			set
			{
				_inPausa = value;
				dynamic p = player.State["PlayerStates"];
				p.InPausa = value;
				player.State.Set("PlayerStates", p, true);
			}
		}

		public bool Svenuto
		{
			get => _svenuto;
			set
			{
				_svenuto = value;
				dynamic p = player.State["PlayerStates"];
				p.Svenuto = value;
				player.State.Set("PlayerStates", p, true);
			}
		}

		public bool Ammanettato
		{
			get => _ammanettato;
			set
			{
				_ammanettato = value;
				dynamic p = player.State["PlayerStates"];
				p.Ammanettato = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public bool InCasa
		{
			get => _inCasa;
			set
			{
				_inCasa = value;
				dynamic p = player.State["PlayerStates"];
				p.InCasa = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public bool InServizio
		{
			get => _inServizio;
			set
			{
				_inServizio = value;
				dynamic p = player.State["PlayerStates"];
				p.InServizio = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public bool FinDiVita
		{
			get => _finDiVita;
			set
			{
				_finDiVita = value;
				dynamic p = player.State["PlayerStates"];
				p.FinDiVita = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public bool AdminSpecta
		{
			get => _adminSpecta;
			set
			{
				_adminSpecta = value;
				dynamic p = player.State["PlayerStates"];
				p.AdminSpecta = value;
				player.State.Set("PlayerStates", p, true);
			}
		}

		public bool InVeicolo
		{
			get => _inVeh;
			set
			{
				_inVeh = value;
				dynamic p = player.State["PlayerStates"];
				p.InVeicolo = value;
				player.State.Set("PlayerStates", p, true);
			}
		}

		public Istanza Istanza = new Istanza();
	}

	public class Istanza
	{
		[JsonIgnore] private Player player = Cache.Player;
		public bool Stanziato
		{
			get => player.State["PlayerStates"].Istanza.Stanziato;
			set
			{
				dynamic p = player.State["PlayerStates"];
				p.Istanza.Stanziato = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public int ServerIdProprietario
		{
			get => player.State["PlayerStates"].Istanza.ServerIdProprietario;
			set
			{
				dynamic p = player.State["PlayerStates"];
				p.Istanza.ServerIdProprietario = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public bool IsProprietario
		{
			get => player.State["PlayerStates"].Istanza.IsProprietario;
			set
			{
				dynamic p = player.State["PlayerStates"];
				p.Istanza.IsProprietario = value;
				player.State.Set("PlayerStates", p, true);
			}
		}

		public string Instance
		{
			get => player.State["PlayerStates"].Istanza.Instance;
			set
			{
				dynamic p = player.State["PlayerStates"];
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
			if (!Stanziato) return;
			if (Instance != instance) Instance = instance;
		}

		/// <summary>
		/// Cambia Proprietario dell'istanza
		/// </summary>
		/// <param name="netId">networkId del proprietario</param>
		public void CambiaIstanza(int netId)
		{
			if (!Stanziato) return;
			if (ServerIdProprietario != netId) ServerIdProprietario = netId;
		}
	}
}