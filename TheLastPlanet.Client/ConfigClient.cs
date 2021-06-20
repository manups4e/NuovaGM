using CitizenFX.Core;
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
		public string NomeServer;
		public string DiscordAppId;
		public string DiscordRichPresenceAsset;
		public bool KickWarning;
		public int AFKCheckTime;

		public int AfkTimeout;
		public int AutoSavingInterval;
		public int PingThreshold;
		public int MaxPlayerNameLength;
		public bool EnableVoiceChat;
		public int DiscordNotificationInterval;
		public int MaxMenuOptionCount;
		public int SpawnProtectionTime;

		public int KillstreakTimeout;

		public int MinPrestigeRank;
		public int MaxPrestige;
		public float PrestigeBonus;

		public int CrewInvitationTimeout;

		public int SpecialVehicleMinRank;

		public List<string> BlackListVehicles = new List<string>();

		public GTA2Cam Gta2Cam = new GTA2Cam();

		public SettingsSpawn Spawn = new SettingsSpawn();
		public List<int> Ranks = new List<int>();
		public MarkerSettings PlaceMarker = new MarkerSettings();

		public int KdRatioMinStat;

		public int RewardNotificationTime;

		public int CashPerKill;
		public int CashPerKillstreak;
		public int MaxCashPerKillstreak;
		public int CashPerHeadshot;
		public int CashPerMission;
		public int CashPerFaction;
		public int CashPerMelee;

		public int ExpPerKill;
		public int ExpPerKillstreak;
		public int MaxExpPerKillstreak;
		public int ExpPerHeadshot;
		public int ExpPerMission;
		public int ExpPerFaction;
		public int ExpPerMelee;

		public int ChallengeRequestTimeout;

		public EventSettings Event = new EventSettings();
		public GunSettings Gun = new GunSettings();
		public StockPilingSettings StockPiling = new StockPilingSettings();
		public SharpShooterSettings SharpShooter = new SharpShooterSettings();
		public KingOfTheCastleSettings Castle = new KingOfTheCastleSettings();
		public HotPropertySettings Property = new HotPropertySettings();
		public HuntTheBeastSettings HuntTheBeast = new HuntTheBeastSettings();
		public float SellWeaponRatio;
		public Dictionary<string, List<string>> AmmuNationWeapons = new Dictionary<string, List<string>>();
		public Dictionary<string, AmmunationRefillingWeaponsSettings> AmmuNationRefillAmmo = new Dictionary<string, AmmunationRefillingWeaponsSettings>();
		public Dictionary<string, AmmunationSpecialAmmoSettings> AmmuNationSpecialAmmo = new Dictionary<string, AmmunationSpecialAmmoSettings>();
		public MissionsSettings Mission = new MissionsSettings();
		public SpecialCargoMissionSettings Cargo = new SpecialCargoMissionSettings();
		public HeistMissionSettings Heist = new HeistMissionSettings();
		public VelocityMissionSettings Velocity = new VelocityMissionSettings();
		public MostWantedSettings MostWanted = new MostWantedSettings();
		public AssetRecoveryMissionSettings AssetRecovery = new AssetRecoveryMissionSettings();
		public HeadHunterSettings HeadHunter = new HeadHunterSettings();
		public CratesSettings Crate = new CratesSettings();
		public List<WeaponTintSettings> WeaponTints = new List<WeaponTintSettings>();
	}

	public class OutfitCreatorFR
	{
		public OutfitSexCreator Maschio = new OutfitSexCreator();
		public OutfitSexCreator Femmina = new OutfitSexCreator();
	}
	public class OutfitSexCreator
	{
		public List<Completo> Aviazione = new List<Completo>();
		public List<Completo> Spiaggia = new List<Completo>();
		public List<Completo> Biker = new List<Completo>();
		public List<Completo> Combattimento = new List<Completo>();
		public List<Completo> Costumi = new List<Completo>();
		public List<Completo> Colpo = new List<Completo>();
		public List<Completo> Gara = new List<Completo>();
		public List<Completo> Lavoro = new List<Completo>();
	}

	public class RolePlayConfig
	{
		public ConfPrincipaleRP Main = new ConfPrincipaleRP();
		public ConfigVeicoliRP Veicoli = new ConfigVeicoliRP();
		public ConfigLavoriRP Lavori = new ConfigLavoriRP();
		public ConfigNegoziRP Negozi = new ConfigNegoziRP();
		public ConfigProprietaRP Proprieta = new ConfigProprietaRP();
	}

	public class ClientConfigKVP
	{
		public bool ModCinema;
		public float LetterBox;
		public string Filtro;
		public float FiltroStrenght;
		public bool MiniMappaAttiva;
		public int DimensioniMinimappa;
		public bool MiniMappaInAuto;
		public bool MostraContattiTelefonoInMappa; // da valutare
		public bool ForzaPrimaPersona_Mira;
		public bool ForzaPrimaPersona_InCopertura;
		public bool ForzaPrimaPersona_InAuto;

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
		public ConfigPolizia Polizia = new ConfigPolizia();
		public ConfigMedici Medici = new ConfigMedici();
		public ConfigVenditoriAuto VenditoriAuto = new ConfigVenditoriAuto();
		public ConfigVenditoriCase VenditoriCase = new ConfigVenditoriCase();
		public LavoriGenerici Generici = new LavoriGenerici();
	}

	public class ConfigVeicoliRP
	{
		public VeicoliAffitto veicoliAff = new();
		public ConfVeicoli DanniVeicoli = new();
	}

	public class ConfigNegoziRP
	{
		public ConfigNegoziAbiti Abiti = new ConfigNegoziAbiti();
		public ConfigNegoziBarbieri Barbieri = new ConfigNegoziBarbieri();
		public ConfigNegoziGenerici NegoziGenerici = new ConfigNegoziGenerici();
	}

	public class ConfigProprietaRP
	{
		public List<Hotel> hotels = new List<Hotel>();
		public Dictionary<string, ConfigCase> Appartamenti = new Dictionary<string, ConfigCase>();
		public ConfigGarages Garages = new ConfigGarages();
	}

	public class ConfigAppartamenti
	{
		public Dictionary<string, ConfigCase> LowEnd = new Dictionary<string, ConfigCase>();
		public Dictionary<string, ConfigCase> MidEnd = new Dictionary<string, ConfigCase>();
		public Dictionary<string, ConfigCase> HighEnd = new Dictionary<string, ConfigCase>();
	}

	public class ConfigGarages
	{
		public ConfigGarage LowEnd = new ConfigGarage();
		public ConfigGarage MidEnd4 = new ConfigGarage();
		public ConfigGarage MidEnd6 = new ConfigGarage();
		public ConfigGarage HighEnd = new ConfigGarage();
		public Dictionary<string, Garages> Garages = new Dictionary<string, Garages>();
		// aggiungere uffici
	}

	public class ConfigGarage
	{
		public Vector3 Pos;
		public int NVehs;
		public Vector4 OutMarker;
		public Vector4 ModifyMarker;
		public Vector3[] ModifyCam = new Vector3[2];
		public Vector4 SpawnInLocation;
		public List<Vector4> PosVehs = new List<Vector4>();
	}

	public class Garages
	{
		public string Label;
		public int tipo;
		public int VehCapacity;
		public Vector3 MarkerEntrata;
		public Vector3 MarkerUscita;
		public Vector3 SpawnDentro;
		public Vector4 SpawnFuori;
		public ConfigCaseCamExt TelecameraFuori = new ConfigCaseCamExt();
		public ConfigCaseCamExt TelecameraModificaDentro = new ConfigCaseCamExt();
		public int Price;
	}

	public class ConfigCase
	{
		public string Label;
		public int VehCapacity;
		public int Tipo;
		public Vector3 MarkerEntrata;
		public Vector3 MarkerUscita;
		public Vector3 SpawnDentro;
		public Vector3 SpawnFuori;
		public ConfigCaseCamExt TelecameraFuori = new ConfigCaseCamExt();
		public ConfigCaseCamInt TelecameraDentro = new ConfigCaseCamInt();
		public List<string> Ipls = new List<string>();
		public string Gateway;
		public bool Is_single;
		public bool Is_room;
		public bool Is_gateway;
		public bool TettoIncluso;
		public Vector3 MarkerTetto;
		public Vector3 SpawnTetto;
		public bool GarageIncluso;
		public Vector3 MarkerGarageEsterno;
		public Vector3 MarkerGarageInterno;
		public Vector3 SpawnGarageAPiediDentro;
		public Vector4 SpawnGarageInVehFuori;
		public int Price;
		public int Stile = 0;
		public bool Strip;
		public bool Booze;
		public bool Smoke;
	}

	public class ConfigCaseCamInt
	{
		public ConfigCaseCamExt Interno = new ConfigCaseCamExt();
		public ConfigCaseCamExt Bagno = new ConfigCaseCamExt();
		public ConfigCaseCamExt Garage = new ConfigCaseCamExt();
	}

	public class ConfigCaseCamExt
	{
		public Vector3 pos;
		public Vector3 guarda;
	}
}