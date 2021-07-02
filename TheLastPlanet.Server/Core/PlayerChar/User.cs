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
using TheLastPlanet.Shared.Snowflakes;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Server.Core.PlayerChar
{
	[Serialization]
	public partial class User : BasePlayerShared
	{
		[Ignore][JsonIgnore] public string source;

		public User()
		{
		}

		[Ignore][JsonIgnore] public DateTime LastSaved;

		public User(Player player, BasePlayerShared result)
		{
			lastConnection = DateTime.Now;
			source = player.Handle;
			ID = result.ID;
			PlayerID = result.PlayerID;
			group = result.group;
			group_level = result.group_level;
			playTime = result.playTime;
			Player = player;
			char_data = result.char_data;
			LastSaved = DateTime.Now;
			Identifiers.Steam = Player.GetLicense(Identifier.Steam);
			Identifiers.License = Player.GetLicense(Identifier.License);
			Identifiers.Discord = Player.GetLicense(Identifier.Discord);
			Identifiers.Fivem = Player.GetLicense(Identifier.Fivem);
			Identifiers.Ip = Player.GetLicense(Identifier.Ip);
		}

		public User(Player player, dynamic result)
		{
			lastConnection = DateTime.Now;
			source = player.Handle;
			ID = result.UserID;
			group = result.group;
			group_level = (UserGroup)result.group_level;
			playTime = result.playTime;
			Player = player;
			StatiPlayer = new PlayerStateBags(player);
			Characters = (result.char_data as string).FromJson<List<Char_data>>();
			LastSaved = DateTime.Now;
		}

		public User(dynamic result)
		{
			lastConnection = DateTime.Now;
			//source = player.Handle;
			group = result.group;
			group_level = (UserGroup)result.group_level;
			playTime = result.playTime;
			//p = player;
			Characters = (result.char_data as string).FromJson<List<Char_data>>();
			LastSaved = DateTime.Now;
		}

		[Ignore][JsonIgnore] public string FullName => CurrentChar.Info.firstname + " " + CurrentChar.Info.lastname;

		[Ignore][JsonIgnore] public string DOB => CurrentChar.Info.dateOfBirth;

		[Ignore][JsonIgnore]
		public bool DeathStatus
		{
			get => CurrentChar.is_dead;
			set { CurrentChar.is_dead = value; StatiPlayer.Svenuto = true; }
		}

		[Ignore][JsonIgnore]
		public int Money
		{
			get => CurrentChar.Finance.Money;
			set
			{
				int var = value - CurrentChar.Finance.Money;
				CurrentChar.Finance.Money += var;
				if (var < 0)
					if (CurrentChar.Finance.Money < 0)
						CurrentChar.Finance.Money = 0;
				Player.TriggerEvent("lprp:changeMoney", var);
			}
		}

		[Ignore][JsonIgnore]
		public int Bank
		{
			get => CurrentChar.Finance.Bank;
			set
			{
				int var = value - CurrentChar.Finance.Bank;
				CurrentChar.Finance.Bank += var;
				if (var < 0) Player.TriggerEvent("lprp:rimuoviBank", var);
			}
		}

		[Ignore][JsonIgnore]
		public int DirtCash
		{
			get => CurrentChar.Finance.DirtyCash;
			set
			{
				int var = value - CurrentChar.Finance.DirtyCash;
				CurrentChar.Finance.DirtyCash += var;
				if (var < 0)
					if (CurrentChar.Finance.DirtyCash < 0)
						CurrentChar.Finance.DirtyCash = 0;
				Player.TriggerEvent("lprp:changeDirty", var);
			}
		}

		public void SetJob(string job, int grade)
		{
			CurrentChar.Job.Name = job;
			CurrentChar.Job.Grade = grade;
		}

		public void SetGang(string job, int grade)
		{
			CurrentChar.Gang.Name = job;
			CurrentChar.Gang.Grade = grade;
		}

		public Tuple<bool, Inventory> getInventoryItem(string item)
		{
			for (int i = 0; i < CurrentChar.Inventory.Count; i++)
				if (CurrentChar.Inventory[i].Item == item)
					return new Tuple<bool, Inventory>(true, CurrentChar.Inventory[i]);

			return new Tuple<bool, Inventory>(false, null);
		}

		public List<Inventory> getCharInventory(uint charId)
		{
			for (int i = 0; i < Characters.Count; i++)
				if (Characters[i].CharID == charId)
					return Characters[i].Inventory;

			return null;
		}

		public void addInventoryItem(string item, int amount, float weight)
		{
			bool vero = getInventoryItem(item).Item1;
			Inventory checkedItem = getInventoryItem(item).Item2;

			if (vero)
			{
				checkedItem.Amount += amount;

				if (checkedItem.Amount == ConfigShared.SharedConfig.Main.Generici.ItemList[item].max)
				{
					checkedItem.Amount = ConfigShared.SharedConfig.Main.Generici.ItemList[item].max;
					Player.TriggerEvent("lprp:ShowNotification", "HAI GIA' IL MASSIMO DI ~w~" + ConfigShared.SharedConfig.Main.Generici.ItemList[item].label + "~w~!");
				}
			}
			else
			{
				CurrentChar.Inventory.Add(new Inventory(item, amount, weight));
			}

			Player.TriggerEvent("lprp:ShowNotification", "Hai ricevuto " + amount + " " + ConfigShared.SharedConfig.Main.Generici.ItemList[item].label + "!");
		}

		public void removeInventoryItem(string item, int amount)
		{
			bool vero = getInventoryItem(item).Item1;
			Inventory checkedItem = getInventoryItem(item).Item2;

			if (vero)
			{
				checkedItem.Amount -= amount;
				if (checkedItem.Amount <= 0) CurrentChar.Inventory.Remove(checkedItem);
			}
			else
			{
				CurrentChar.Inventory.ToList().Remove(checkedItem);
			}

			Player.TriggerEvent("lprp:ShowNotification", amount + " " + ConfigShared.SharedConfig.Main.Generici.ItemList[item].label + " ti sono stati rimossi/e!");
		}

		public List<Weapons> getCharWeapons(uint charId)
		{
			for (int i = 0; i < Characters.Count; i++)
				if (Characters[i].CharID == charId)
					return Characters[i].Weapons;

			return null;
		}

		public void addWeapon(string weaponName, int ammo)
		{
			if (!hasWeapon(weaponName))
			{
				CurrentChar.Weapons.Add(new Weapons(weaponName, ammo, new List<Components>(), 0));
				Player.TriggerEvent("lprp:addWeapon", weaponName, ammo);

			}
		}

		public void updateWeaponAmmo(string weaponName, int ammo)
		{
			Tuple<int, Weapons> weapon = getWeapon(weaponName);
			if (weapon.Item2.ammo > ammo) CurrentChar.Weapons[weapon.Item1].ammo = ammo;
		}

		public void removeWeapon(string weaponName)
		{
			Server.Logger.Debug("index = " + getWeapon(weaponName).Item1);
			Server.Logger.Debug(JsonConvert.SerializeObject(getWeapon(weaponName).Item2));

			if (hasWeapon(weaponName))
			{
				CurrentChar.Weapons.Remove(getWeapon(weaponName).Item2);
				Player.TriggerEvent("lprp:removeWeapon", weaponName);

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
				CurrentChar.Weapons[num].components.Add(new Components(weaponComponent, true));
				Player.TriggerEvent("lprp:addWeaponComponent", weaponName, weaponComponent);

			}
		}

		public void removeWeaponComponent(string weaponName, string weaponComponent)
		{
			int num = getWeapon(weaponName).Item1;
			Weapons weapon = getWeapon(weaponName).Item2;

			if (weapon != null)
				for (int i = 0; i < CurrentChar.Weapons[num].components.Count; i++)
					if (CurrentChar.Weapons[num].components[i].name == weaponComponent)
					{
						CurrentChar.Weapons[num].components.RemoveAt(i);
						Player.TriggerEvent("lprp:removeWeaponComponent", weaponName, weaponComponent);
		
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
					CurrentChar.Weapons[num].tint = tint;
					Player.TriggerEvent("lprp:addWeaponTint", weaponName, tint);
	
				}
			}
		}

		public bool hasWeapon(string weaponName)
		{
			return CurrentChar.Weapons.Any(x => x.name == weaponName);
		}

		public Tuple<int, Weapons> getWeapon(string weaponName)
		{
			Weapons weapon = CurrentChar.Weapons.FirstOrDefault(x => x.name == weaponName);

			return weapon != null ? new Tuple<int, Weapons>(CurrentChar.Weapons.IndexOf(weapon), weapon) : new Tuple<int, Weapons>(0, null);
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

		[Ignore][JsonIgnore] public Vector3 getCoords => CurrentChar.Posizione.ToVector3;

		public void giveLicense(string license, string mittente)
		{
			Licenses licenza = new(license, DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss"), mittente);
			CurrentChar.Licenze.Add(licenza);
		}

		public void removeLicense(string license)
		{
			foreach (Licenses licen in CurrentChar.Licenze)
				if (licen.name == license)
					CurrentChar.Licenze.Remove(licen);
				else
					Server.Logger.Warning( $"Il player {Player.Name} non ha una licenza con nome '{license}'");
		}

		public List<OwnedVehicle> GetCharVehicles()
		{
			return CurrentChar.Veicoli;
		}

		public void AddExperience(int experiencePoints)
		{
			var nextLevelTotalXp = Experience.NextLevelExperiencePoints(FreeRoamChar.Level);

			if (FreeRoamChar.TotalXp + experiencePoints >= nextLevelTotalXp)
			{
				int remainder = FreeRoamChar.TotalXp + experiencePoints - nextLevelTotalXp;
				FreeRoamChar.Level++;
				if (remainder > 0)
					AddExperience(remainder);
			}
			else
			{
				FreeRoamChar.TotalXp += experiencePoints;
			}


		}

		public void UpdateCurrentAttempt(int eventId, float currentAttempt)
		{
			try
			{
				var data = PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
				if (data != null)
				{
					data.CurrentAttempt = currentAttempt;
					if (currentAttempt > data.BestAttempt)
						data.BestAttempt = currentAttempt;
				}
				else
					Server.Logger.Warning($"Data for Event {eventId} does not exist for Player {Player.Name}");
			}
			catch (Exception e)
			{
				Server.Logger.Error(e.ToString());
			}
		}

		public void showNotification(string text)
		{
			Player.TriggerEvent("lprp:ShowNotification", text);
		}

		public async Task SalvaPersonaggio()
		{
			try
			{
				await MySQL.ExecuteAsync("call SalvaPersonaggio(@gr, @level, @time, @current, @mon, @bank, @dirty, @weap, @invent, @location, @job, @jgrade, @gang, @ggrade, @skin, @dress, @needs, @stats, @dead, @id)", new
				{
					gr = group,
					level = group_level,
					time = playTime,
					current = CurrentChar.CharID,
					mon = Money,
					bank = Bank,
					dirty = DirtCash,
					weap = CurrentChar.Weapons.ToJson(),
					invent = CurrentChar.Inventory.ToJson(),
					location = CurrentChar.Posizione.ToJson(),
					job = CurrentChar.Job.Name,
					jgrade = CurrentChar.Job.Grade,
					gang = CurrentChar.Gang.Name,
					ggrade = CurrentChar.Gang.Grade,
					skin = CurrentChar.Skin.ToJson(),
					dress = CurrentChar.Dressing.ToJson(),
					needs = CurrentChar.Needs.ToJson(),
					stats = CurrentChar.Statistiche.ToJson(),
					dead = CurrentChar.is_dead,
					id = ID,
				});
				LastSaved = DateTime.Now;
			}
			catch(Exception e)
			{
				Server.Logger.Error( e.ToString());
			}
			await Task.FromResult(0);
		}

	}

	[Serialization]
	public partial class PlayerStateBags
	{
		[Ignore][JsonIgnore] 
		private Player player;
		public bool InPausa
		{
			get => player.Character.State["PlayerStates"].InPausa;
			set => player.Character.State["PlayerStates"].InPausa = value;
		}

		public bool Svenuto
		{
			get => player.Character.State["PlayerStates"].Svenuto;
			set => player.Character.State["PlayerStates"].Svenuto = value;
		}

		public bool Ammanettato
		{
			get => player.Character.State["PlayerStates"].Ammanettato;
			set => player.Character.State["PlayerStates"].Ammanettato = value;
		}
		public bool InCasa
		{
			get => player.Character.State["PlayerStates"].InCasa;
			set => player.Character.State["PlayerStates"].InCasa = value;
		}
		public bool InServizio
		{
			get => player.Character.State["PlayerStates"].InServizio;
			set => player.Character.State["PlayerStates"].InServizio = value;
		}
		public bool FinDiVita
		{
			get => player.Character.State["PlayerStates"].FinDiVita;
			set => player.Character.State["PlayerStates"].FinDiVita = value;
		}
		public bool AdminSpecta
		{
			get => player.Character.State["PlayerStates"].AdminSpecta;
			set => player.Character.State["PlayerStates"].AdminSpecta = value;
		}
		public bool InVeicolo
		{
			get => player.Character.State["PlayerStates"].InVeicolo;
			set => player.Character.State["PlayerStates"].InVeicolo = value;
		}
		public Istanza Istanza;

		public PlayerStateBags(Player pl)
		{
			player = pl;
			Istanza = new Istanza(pl);
			object baseBag = new
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
			player.Character.State.Set("PlayerStates", baseBag, true);
		}
	}

	[Serialization]
	public partial class Istanza
	{
		[Ignore][JsonIgnore]
		private Player player;

		public Istanza(Player pl)
		{
			player = pl;
		}

		public bool Stanziato
		{
			get => player.Character.State["PlayerStates"].Istanza.Stanziato;
			set => player.Character.State["PlayerStates"].Istanza.Stanziato = value;
		}
		public int ServerIdProprietario
		{
			get => player.Character.State["PlayerStates"].Istanza.ServerIdProprietario;
			set => player.Character.State["PlayerStates"].Istanza.ServerIdProprietario = value;
		}
		public bool IsProprietario
		{
			get => player.Character.State["PlayerStates"].Istanza.IsProprietario;
			set => player.Character.State["PlayerStates"].Istanza.IsProprietario = value;
		}

		public string Instance
		{
			get => player.Character.State["PlayerStates"].Istanza.Instance;
			set => player.Character.State["PlayerStates"].Istanza.Instance = value;
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