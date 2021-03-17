using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Impostazioni.Shared.Configurazione.Generici;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.PlayerChar;

namespace TheLastPlanet.Client.Core.PlayerChar
{
	public class User : BasePlayerShared
	{
		public int source;
		public Vector4 posizione = Vector4.Zero;

		public User(dynamic result)
		{
			source = SessionCache.Cache.MyPlayer.Player.ServerId;
			group = result.group;
			group_level = (UserGroup)result.group_level;
			playTime = result.playTime;
			Characters = (result.char_data as string).DeserializeFromJson<List<Char_data>>();
			status = new Shared.PlayerChar.Status();
		}

		public User()
		{
			StatiPlayer = new PlayerStateBags();
		}

		[JsonIgnore] public string FullName => CurrentChar.info.firstname + " " + CurrentChar.info.lastname;

		[JsonIgnore] public string DoB => CurrentChar.info.dateOfBirth;

		[JsonIgnore] public bool DeathStatus => CurrentChar.is_dead;

		[JsonIgnore] public int Money => CurrentChar.finance.money;

		[JsonIgnore] public int Bank => CurrentChar.finance.bank;

		[JsonIgnore] public int DirtyMoney => CurrentChar.finance.dirtyCash;

		[JsonIgnore] public List<Inventory> Inventory => GetCharInventory();

		public Tuple<bool, Inventory, Item> GetInventoryItem(string item)
		{
			foreach (Inventory t in CurrentChar.inventory)
				if (t.item == item)
					return new Tuple<bool, Inventory, Item>(true, t, ConfigShared.SharedConfig.Main.Generici.ItemList[item]);

			return new Tuple<bool, Inventory, Item>(false, null, null);
		}

		public List<Inventory> GetCharInventory()
		{
			return CurrentChar.inventory;
		}

		public List<Weapons> GetCharWeapons()
		{
			return CurrentChar.weapons;
		}

		public bool HasWeapon(string weaponName)
		{
			return CurrentChar.weapons.Any(x => x.name == weaponName);
		}

		public bool HasWeapon(WeaponHash weaponName)
		{
			return CurrentChar.weapons.Any(x => Funzioni.HashInt(x.name) == (int)weaponName);
		}

		public Tuple<int, Weapons> GetWeapon(string weaponName)
		{
			for (int i = 0; i < CurrentChar.weapons.Count; i++)
				if (CurrentChar.weapons[i].name == weaponName)
					return new Tuple<int, Weapons>(i, CurrentChar.weapons[i]);

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
			return CurrentChar.licenze.Any(x => x.name == license);
		}
	}

	public class PlayerStateBags
	{
		[JsonIgnore] private Player player = SessionCache.Cache.MyPlayer.Player;

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

		public Istanza Istanza = new();
	}

	public class Istanza
	{
		[JsonIgnore] private Player player = SessionCache.Cache.MyPlayer.Player;
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