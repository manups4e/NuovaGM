using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Impostazioni.Shared.Configurazione.Generici;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.PlayerChar;

namespace TheLastPlanet.Client.Core.PlayerChar
{
	public class User : BasePlayerShared
	{
		public int source;
		
		[JsonIgnore]
		public Position Posizione
		{
			get => CurrentChar.Posizione;
			set
			{
				CurrentChar.Posizione.X = value.X;
				CurrentChar.Posizione.Y = value.Y;
				CurrentChar.Posizione.Z = value.Z;
				CurrentChar.Posizione.Heading = value.Heading;
			}
		}

		public User(BasePlayerShared result)
		{
			lastConnection = result.lastConnection;
			ID = result.ID;
			PlayerID = result.PlayerID;
			group = result.group;
			group_level = result.group_level;
			playTime = result.playTime;
			Player = Game.Player;
			char_data = result.char_data;
			Identifiers = result.Identifiers;
			StatiPlayer = new PlayerStateBags();
		}

		public User() { }

		[JsonIgnore] public string FullName => CurrentChar.Info.firstname + " " + CurrentChar.Info.lastname;

		[JsonIgnore] public string DoB => CurrentChar.Info.dateOfBirth;

		[JsonIgnore] public bool DeathStatus => CurrentChar.is_dead;

		[JsonIgnore] public int Money => CurrentChar.Finance.Money;

		[JsonIgnore] public int Bank => CurrentChar.Finance.Bank;

		[JsonIgnore] public int DirtyCash => CurrentChar.Finance.DirtyCash;

		[JsonIgnore] public List<Inventory> Inventory => GetCharInventory();

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

	public class PlayerStateBags
	{
		[JsonIgnore]
		private Player player = Game.Player;

		public Istanza Istanza = new();

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
	}

	public class Istanza
	{
		[JsonIgnore] 
		private Player player = Game.Player;

		private bool _stanziato;
		private int _idProprietario;
		private bool _isProprietario;
		private string _instance;

		public Istanza()
		{
			Stanziato = false;
			ServerIdProprietario = -1;
			IsProprietario = false;
			Instance = "";
		}

		public bool Stanziato
		{
			get => _stanziato;
			set
			{
				dynamic p = player.State["PlayerStates"];
				p.Istanza.Stanziato = value;
				_stanziato = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public int ServerIdProprietario
		{
			get => _idProprietario;
			set
			{
				dynamic p = player.State["PlayerStates"];
				p.Istanza.ServerIdProprietario = value;
				_idProprietario = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public bool IsProprietario
		{
			get => _isProprietario;
			set
			{
				dynamic p = player.State["PlayerStates"];
				p.Istanza.IsProprietario = value;
				_isProprietario = value;
				player.State.Set("PlayerStates", p, true);
			}
		}
		public string Instance
		{
			get => _instance;
			set
			{
				dynamic p = player.State["PlayerStates"];
				p.Istanza.Instance = value;
				_instance = value;
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