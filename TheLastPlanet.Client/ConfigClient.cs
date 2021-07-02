﻿using CitizenFX.Core;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.RolePlay.Lavori;
using TheLastPlanet.Client.RolePlay.Proprietà.Hotel;
using TheLastPlanet.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostazioni.Client.Configurazione.Lavori.WhiteList;
using Impostazioni.Client.Configurazione.Main;
using Impostazioni.Client.Configurazione.Negozi.Abiti;
using Impostazioni.Client.Configurazione.Negozi.Barbieri;
using Impostazioni.Client.Configurazione.Negozi.Generici;
using Impostazioni.Client.Configurazione.Veicoli;

namespace TheLastPlanet.Client
{
	public class Configurazione
	{
		public RolePlayConfig RolePlay { get; private set; }
		public FreeRoamConfig FreeRoam { get; private set; }

		public void LoadConfig(ModalitaServer id, string jsonConfig)
		{
			switch (id)
			{
				case ModalitaServer.Lobby:
					break;
				case ModalitaServer.Roleplay:
					RolePlay = jsonConfig.FromJson<RolePlayConfig>();
					break;
				case ModalitaServer.Minigiochi:
					break;
				case ModalitaServer.Gare:
					break;
				case ModalitaServer.Negozio:
					break;
				case ModalitaServer.FreeRoam:
					FreeRoam = jsonConfig.FromJson<FreeRoamConfig>();
					break;
			}
		}
	}

	public class FreeRoamConfig
	{
		public ConfigPrincipaleFR Main { get; set; }
		public OutfitCreatorFR Outfits { get; set; }
	}

	public class ConfigPrincipaleFR
	{
		public string NomeServer { get; set; }
		public string DiscordAppId { get; set; }
		public string DiscordRichPresenceAsset { get; set; }
		public bool KickWarning { get; set; }
		public int AFKCheckTime { get; set; }

		public int AfkTimeout { get; set; }
		public int AutoSavingInterval { get; set; }
		public int PingThreshold { get; set; }
		public int MaxPlayerNameLength { get; set; }
		public bool EnableVoiceChat { get; set; }
		public int DiscordNotificationInterval { get; set; }
		public int MaxMenuOptionCount { get; set; }
		public int SpawnProtectionTime { get; set; }

		public int KillstreakTimeout { get; set; }

		public int MinPrestigeRank { get; set; }
		public int MaxPrestige { get; set; }
		public float PrestigeBonus { get; set; }

		public int CrewInvitationTimeout { get; set; }

		public int SpecialVehicleMinRank { get; set; }

		public List<string> BlackListVehicles { get; set; }

		public GTA2Cam Gta2Cam { get; set; }

		public SettingsSpawn Spawn { get; set; }
		public List<int> Ranks { get; set; }
		public MarkerSettings PlaceMarker { get; set; }

		public int KdRatioMinStat { get; set; }

		public int RewardNotificationTime { get; set; }

		public int CashPerKill { get; set; }
		public int CashPerKillstreak { get; set; }
		public int MaxCashPerKillstreak { get; set; }
		public int CashPerHeadshot { get; set; }
		public int CashPerMission { get; set; }
		public int CashPerFaction { get; set; }
		public int CashPerMelee { get; set; }

		public int ExpPerKill { get; set; }
		public int ExpPerKillstreak { get; set; }
		public int MaxExpPerKillstreak { get; set; }
		public int ExpPerHeadshot { get; set; }
		public int ExpPerMission { get; set; }
		public int ExpPerFaction { get; set; }
		public int ExpPerMelee { get; set; }

		public int ChallengeRequestTimeout { get; set; }

		public EventSettings Event { get; set; }
		public GunSettings Gun { get; set; }
		public StockPilingSettings StockPiling { get; set; }
		public SharpShooterSettings SharpShooter { get; set; }
		public KingOfTheCastleSettings Castle { get; set; }
		public HotPropertySettings Property { get; set; }
		public HuntTheBeastSettings HuntTheBeast { get; set; }
		public float SellWeaponRatio;
		public Dictionary<string, List<string>> AmmuNationWeapons { get; set; }
		public Dictionary<string, AmmunationRefillingWeaponsSettings> AmmuNationRefillAmmo { get; set; }
		public Dictionary<string, AmmunationSpecialAmmoSettings> AmmuNationSpecialAmmo { get; set; }
		public MissionsSettings Mission { get; set; }
		public SpecialCargoMissionSettings Cargo { get; set; }
		public HeistMissionSettings Heist { get; set; }
		public VelocityMissionSettings Velocity { get; set; }
		public MostWantedSettings MostWanted { get; set; }
		public AssetRecoveryMissionSettings AssetRecovery { get; set; }
		public HeadHunterSettings HeadHunter { get; set; }
		public CratesSettings Crate { get; set; }
		public List<WeaponTintSettings> WeaponTints { get; set; }
	}

	public class OutfitCreatorFR
	{
		public OutfitSexCreator Maschio { get; set; }
		public OutfitSexCreator Femmina { get; set; }
	}
	public class OutfitSexCreator
	{
		public List<Completo> Aviazione { get; set; }
		public List<Completo> Spiaggia { get; set; }
		public List<Completo> Biker { get; set; }
		public List<Completo> Combattimento { get; set; }
		public List<Completo> Costumi { get; set; }
		public List<Completo> Colpo { get; set; }
		public List<Completo> Gara { get; set; }
		public List<Completo> Lavoro { get; set; }
	}

	public class RolePlayConfig
	{
		public ConfPrincipaleRP Main { get; set; }
		public ConfigVeicoliRP Veicoli { get; set; }
		public ConfigLavoriRP Lavori { get; set; }
		public ConfigNegoziRP Negozi { get; set; }
		public ConfigProprietaRP Proprieta { get; set; }
	}

	public class ClientConfigKVP
	{
		public bool ModCinema { get; set; }
		public float LetterBox{ get; set; }
		public string Filtro{ get; set; }
		public float FiltroStrenght{ get; set; }
		public bool MiniMappaAttiva{ get; set; }
		public int DimensioniMinimappa{ get; set; }
		public bool MiniMappaInAuto{ get; set; }
		public bool MostraContattiTelefonoInMappa{ get; set; } // da valutare
		public bool ForzaPrimaPersona_Mira{ get; set; }
		public bool ForzaPrimaPersona_InCopertura{ get; set; }
		public bool ForzaPrimaPersona_InAuto{ get; set; }

		public ClientConfigKVP()
		{
			ModCinema = false;
			LetterBox = 0f;
			Filtro = "None";
			FiltroStrenght = 0.5f;
			MiniMappaAttiva = true;
			DimensioniMinimappa = 0;
			MiniMappaInAuto = true;
			MostraContattiTelefonoInMappa = false;
			ForzaPrimaPersona_Mira = false;
			ForzaPrimaPersona_InCopertura = false;
			ForzaPrimaPersona_InAuto = false;
		}
	}

	public class ConfigLavoriRP
	{
		public ConfigPolizia Polizia { get; set; }
		public ConfigMedici Medici { get; set; }
		public ConfigVenditoriAuto VenditoriAuto { get; set; }
		public ConfigVenditoriCase VenditoriCase { get; set; }
		public LavoriGenerici Generici { get; set; }
	}

	public class ConfigVeicoliRP
	{
		public VeicoliAffitto veicoliAff { get; set; }
		public ConfVeicoli DanniVeicoli { get; set; }
	}

	public class ConfigNegoziRP
	{
		public ConfigNegoziAbiti Abiti { get; set; }
		public ConfigNegoziBarbieri Barbieri { get; set; }
		public ConfigNegoziGenerici NegoziGenerici { get; set; }
	}

	public class ConfigProprietaRP
	{
		public List<Hotel> hotels { get; set; }
		public Dictionary<string, ConfigCase> Appartamenti { get; set; }
		public ConfigGarages Garages { get; set; }
	}

	public class ConfigAppartamenti
	{
		public Dictionary<string, ConfigCase> LowEnd { get; set; }
		public Dictionary<string, ConfigCase> MidEnd { get; set; }
		public Dictionary<string, ConfigCase> HighEnd { get; set; }
	}

	public class ConfigGarages
	{
		public ConfigGarage LowEnd { get; set; }
		public ConfigGarage MidEnd4 { get; set; }
		public ConfigGarage MidEnd6 { get; set; }
		public ConfigGarage HighEnd { get; set; }
		public Dictionary<string, Garages> Garages { get; set; }
		// aggiungere uffici
	}

	public class ConfigGarage
	{
		public Vector3 Pos { get; set; }
		public int NVehs { get; set; }
		public Vector4 OutMarker { get; set; }
		public Vector4 ModifyMarker { get; set; }
		public Vector3[] ModifyCam { get; set; } = new Vector3[2];
		public Vector4 SpawnInLocation { get; set; }
		public List<Vector4> PosVehs { get; set; }
	}

	public class Garages
	{
		public string Label { get; set; }
		public int tipo{ get; set; }
		public int VehCapacity{ get; set; }
		public Vector3 MarkerEntrata{ get; set; }
		public Vector3 MarkerUscita{ get; set; }
		public Vector3 SpawnDentro{ get; set; }
		public Vector4 SpawnFuori{ get; set; }
		public ConfigCaseCamExt TelecameraFuori { get; set; }
		public ConfigCaseCamExt TelecameraModificaDentro { get; set; }
		public int Price { get; set; }
	}

	public class ConfigCase
	{
		public string Label { get; set; }
		public int VehCapacity{ get; set; }
		public int Tipo{ get; set; }
		public Vector3 MarkerEntrata{ get; set; }
		public Vector3 MarkerUscita{ get; set; }
		public Vector3 SpawnDentro{ get; set; }
		public Vector3 SpawnFuori{ get; set; }
		public ConfigCaseCamExt TelecameraFuori { get; set; }
		public ConfigCaseCamInt TelecameraDentro { get; set; }
		public List<string> Ipls { get; set; }
		public string Gateway { get; set; }
		public bool Is_single{ get; set; }
		public bool Is_room{ get; set; }
		public bool Is_gateway{ get; set; }
		public bool TettoIncluso{ get; set; }
		public Vector3 MarkerTetto{ get; set; }
		public Vector3 SpawnTetto{ get; set; }
		public bool GarageIncluso{ get; set; }
		public Vector3 MarkerGarageEsterno{ get; set; }
		public Vector3 MarkerGarageInterno{ get; set; }
		public Vector3 SpawnGarageAPiediDentro{ get; set; }
		public Vector4 SpawnGarageInVehFuori{ get; set; }
		public int Price{ get; set; }
		public int Stile { get; set; } = 0;
		public bool Strip { get; set; }
		public bool Booze{ get; set; }
		public bool Smoke{ get; set; }
	}

	public class ConfigCaseCamInt
	{
		public ConfigCaseCamExt Interno { get; set; }
		public ConfigCaseCamExt Bagno { get; set; }
		public ConfigCaseCamExt Garage { get; set; }
	}

	public class ConfigCaseCamExt
	{
		public Vector3 pos { get; set; }
		public Vector3 guarda { get; set; }
	}
}