using CitizenFX.Core;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Lavori;
using TheLastPlanet.Client.Proprietà.Hotel;
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
	internal static class ConfigClient
	{
		public static async Task Init()
		{
			//var config = await Client.Instance.Eventi.Request<KeyValuePair<Configurazione, SharedConfig>>("lprp:configurazione");
			Client.Impostazioni = new Configurazione();
			ConfigShared.SharedConfig = new SharedConfig();
			await Task.FromResult(0);
		}
	}

	public class Configurazione
	{
		public ConfPrincipale Main = new ConfPrincipale();
		public ConfigVeicoli Veicoli = new ConfigVeicoli();
		public ConfigLavori Lavori = new ConfigLavori();
		public ConfigNegozi Negozi = new ConfigNegozi();
		public ConfigProprieta Proprieta = new ConfigProprieta();
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

	public class ConfigLavori
	{
		public ConfigPolizia Polizia = new ConfigPolizia();
		public ConfigMedici Medici = new ConfigMedici();
		public ConfigVenditoriAuto VenditoriAuto = new ConfigVenditoriAuto();
		public ConfigVenditoriCase VenditoriCase = new ConfigVenditoriCase();
		public LavoriGenerici Generici = new LavoriGenerici();
	}

	public class ConfigVeicoli
	{
		public VeicoliAffitto veicoliAff = new();
		public ConfVeicoli DanniVeicoli = new();
	}

	public class ConfigNegozi
	{
		public ConfigNegoziAbiti Abiti = new ConfigNegoziAbiti();
		public ConfigNegoziBarbieri Barbieri = new ConfigNegoziBarbieri();
		public ConfigNegoziGenerici NegoziGenerici = new ConfigNegoziGenerici();
	}
	
	public class ConfigProprieta
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