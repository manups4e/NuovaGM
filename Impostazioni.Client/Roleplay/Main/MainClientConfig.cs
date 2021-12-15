using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Impostazioni.Client.Configurazione.Lavori.WhiteList;
using Impostazioni.Client.Configurazione.Negozi.Abiti;
using Impostazioni.Client.Configurazione.Negozi.Barbieri;
using Impostazioni.Client.Configurazione.Negozi.Generici;
using Impostazioni.Client.Configurazione.Veicoli;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà.Hotel;
using TheLastPlanet.Shared;

namespace Impostazioni.Client.Configurazione.Main
{
    public class ConfPrincipaleRP
    {
        public string NomeServer { get; set; }
        public string DiscordAppId { get; set; }
        public string DiscordRichPresenceAsset { get; set; }
        public bool PassengerDriveBy { get; set; }
        public bool KickWarning { get; set; }
        public int AFKCheckTime { get; set; }
        public Vector4 Firstcoords { get; set; }
        public int ReviveReward { get; set; }
        public bool EarlyRespawn { get; set; }
        public bool EarlyRespawnFine { get; set; }
        public int EarlyRespawnFineAmount { get; set; }
        public int EarlySpawnTimer { get; set; }
        public int BleedoutTimer { get; set; }
        public bool EscludiMirino { get; set; }
        public List<WeaponHash> ScopedWeapons { get; set; }
        public Dictionary<long, float> recoils { get; set; }

        public List<string> pickupList { get; set; }

        public List<GenericPeds> stripClub { get; set; }
        public List<GenericPeds> blackMarket { get; set; }
        public List<GenericPeds> illegal_weapon_extra_shop { get; set; }
        public float baseTraffic { get; set; }
        public float divMultiplier { get; set; }
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
		public float LetterBox { get; set; }
		public string Filtro { get; set; }
		public float FiltroStrenght { get; set; }
		public bool MiniMappaAttiva { get; set; }
		public int DimensioniMinimappa { get; set; }
		public bool MiniMappaInAuto { get; set; }
		public bool MostraContattiTelefonoInMappa { get; set; } // da valutare
		public bool ForzaPrimaPersona_Mira { get; set; }
		public bool ForzaPrimaPersona_InCopertura { get; set; }
		public bool ForzaPrimaPersona_InAuto { get; set; }

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
		public Position Pos { get; set; }
		public int NVehs { get; set; }
		public Position OutMarker { get; set; }
		public Position ModifyMarker { get; set; }
		public Position[] ModifyCam { get; set; } = new Position[2];
		public Position SpawnInLocation { get; set; }
		public List<Position> PosVehs { get; set; }
	}

	public class Garages
	{
		public string Label { get; set; }
		public int tipo { get; set; }
		public int VehCapacity { get; set; }
		public Position MarkerEntrata { get; set; }
		public Position MarkerUscita { get; set; }
		public Position SpawnDentro { get; set; }
		public Position SpawnFuori { get; set; }
		public ConfigCaseCamExt TelecameraFuori { get; set; }
		public ConfigCaseCamExt TelecameraModificaDentro { get; set; }
		public int Price { get; set; }
	}

	public class ConfigCase
	{
		public string Label { get; set; }
		public int VehCapacity { get; set; }
		public int Tipo { get; set; }
		public Position MarkerEntrata { get; set; }
		public Position MarkerUscita { get; set; }
		public Position SpawnDentro { get; set; }
		public Position SpawnFuori { get; set; }
		public ConfigCaseCamExt TelecameraFuori { get; set; }
		public ConfigCaseCamInt TelecameraDentro { get; set; }
		public List<string> Ipls { get; set; }
		public string Gateway { get; set; }
		public bool Is_single { get; set; }
		public bool Is_room { get; set; }
		public bool Is_gateway { get; set; }
		public bool TettoIncluso { get; set; }
		public Position MarkerTetto { get; set; }
		public Position SpawnTetto { get; set; }
		public bool GarageIncluso { get; set; }
		public Position MarkerGarageEsterno { get; set; }
		public Position MarkerGarageInterno { get; set; }
		public Position SpawnGarageAPiediDentro { get; set; }
		public Position SpawnGarageInVehFuori { get; set; }
		public int Price { get; set; }
		public int Stile { get; set; } = 0;
		public bool Strip { get; set; }
		public bool Booze { get; set; }
		public bool Smoke { get; set; }
	}

	public class ConfigCaseCamInt
	{
		public ConfigCaseCamExt Interno { get; set; }
		public ConfigCaseCamExt Bagno { get; set; }
		public ConfigCaseCamExt Garage { get; set; }
	}

	public class ConfigCaseCamExt
	{
		public Position pos { get; set; }
		public Position guarda { get; set; }
	}
}