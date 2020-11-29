using CitizenFX.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TheLastPlanet.Shared
{
	public class ConfigurazionePolizia
	{
		public bool AbilitaBlipVolanti;
		public bool AbilitaTimerManette;
		public int TimerManette;
		public int MaxInServizio;
		public bool PuoPlaccare;
		public bool AbilitaUsoCani;
		public List<CaniPolizia> Cani = new List<CaniPolizia>();
		public List<string> ModificheAutorizzate = new List<string>();
		public List<StazioniDiPolizia> Stazioni = new List<StazioniDiPolizia>();

	}

	public class ConfigurazioneMedici
	{
		public bool AbilitaBlipVolanti;
		public List<Ospedale> Ospedali = new List<Ospedale>();
	}

	public class ConfigurazioneVendAuto
	{
		public Vector3 BossActions;
		public Vector3 MenuVendita;
	}
	public class ConfigurazioneVendCase
	{
		public Vector3 BossActions;
		public Vector3 Ingresso;
		public Vector3 Uscita;
		public Vector3 Dentro;
		public Vector3 Fuori;
		public Vector3 Actions;
	}

	public class Pescatori
	{
		public bool TempoPescaDinamico;
		public int TempoFisso;
		public int PrezzoVenditaPesce;
		public int PrezzoVenditaAltro;
		public List<Vector3> LuoghiVendita = new List<Vector3>();
		//public float[] AffittoBarca = new float[3];
		public Vector4 SpawnBarca;
		public List<string> Barche = new List<string>();
		public PesciPescati Pesci = new PesciPescati();
	}

	public class PesciPescati
	{
		public List<string> facile = new List<string>();
		public List<string> medio = new List<string>();
		public List<string> avanzato = new List<string>();
	}

	public class Cacciatori
	{
		public Vector3 inizioCaccia;
		public Vector3 zonaDiCaccia;
		public float limiteArea;
	}
	public class Tassisti
	{
		public Vector3 PosAccettazione;
		public Vector3 PosDepositoVeicolo;
		public Vector3 PosRitiroVeicolo;
		public Vector4 PosSpawnVeicolo;
		public float PrezzoModifier;
		public float pickupRange;
		public List<Vector3> jobCoords;
	}

	public class Towing
	{
		public Vector3 InizioLavoro;
		public List<Vector3> PuntiDespawn = new List<Vector3>();
		public List<string> VeicoliDaRimorchiare = new List<string>();
		public List<Vector4> SpawnVeicoli = new List<Vector4>();
	}

	public class WhiteListed
	{
		public ConcurrentDictionary<string, JobGrade> Polizia = new ConcurrentDictionary<string, JobGrade>();
		public ConcurrentDictionary<string, JobGrade> Medico = new ConcurrentDictionary<string, JobGrade>();
		public ConcurrentDictionary<string, JobGrade> Meccanico = new ConcurrentDictionary<string, JobGrade>();

	}

	public class JobGrade
	{
		public int Id;
		public int Stipendio;
		public AbitiLavoro Vestiti;
	}

	public class CaniPolizia
	{
		public string Nome;
		public string Model;
	}

	public class Autorizzati
	{
		public string Nome;
		public string Model;
		public List<int> GradiAutorizzati = new List<int>();
	}

	public class AbitiLavoro
	{
		public AbitiLav Maschio;
		public AbitiLav Femmina;
	}
	public class AbitiLav
	{
		public ComponentDrawables Abiti = new ComponentDrawables();
		public ComponentDrawables TextureVestiti = new ComponentDrawables();
		public PropIndices Accessori = new PropIndices();
		public PropIndices TexturesAccessori = new PropIndices();
	}

	public class StazioniDiPolizia
	{
		public BlipLavoro Blip = new BlipLavoro();
		public List<Vector3> Spogliatoio = new List<Vector3>();
		public List<Vector3> Armerie = new List<Vector3>();
		public List<Autorizzati> VeicoliAutorizzati = new List<Autorizzati>();
		public List<Autorizzati> ElicotteriAutorizzati = new List<Autorizzati>();
		public List<Autorizzati> ArmiAutorizzate = new List<Autorizzati>();
		public List<SpawnerSpawn> Veicoli = new List<SpawnerSpawn>();
		public List<SpawnerSpawn> Elicotteri = new List<SpawnerSpawn>();
		public List<Vector3> AzioniCapo = new List<Vector3>();
	}

	public class Ospedale
	{
		public BlipLavoro Blip = new BlipLavoro();
		public List<Vector3> Spogliatoio = new List<Vector3>();
		public List<Vector3> Farmacia = new List<Vector3>();
		public List<Vector3> IngressoVisitatori = new List<Vector3>();
		public List<Vector3> UscitaVisitatori = new List<Vector3>();
		public List<Autorizzati> VeicoliAutorizzati = new List<Autorizzati>();
		public List<Autorizzati> ElicotteriAutorizzati = new List<Autorizzati>();
		public List<SpawnerSpawn> Veicoli = new List<SpawnerSpawn>();
		public List<SpawnerSpawn> Elicotteri = new List<SpawnerSpawn>();
		public List<Vector3> AzioniCapo = new List<Vector3>();
	}

	public class BlipLavoro
	{
		public Vector3 Coords;
		public int Sprite;
		public int Display;
		public float Scale;
		public int Color;
		public string Nome;
	}
	public class SpawnerSpawn
	{
		public Vector3 SpawnerMenu;
		public List<SpawnPoints> SpawnPoints = new List<SpawnPoints>();
		public List<Vector3> Deleters = new List<Vector3>();
	}

	public class SpawnPoints
	{
		public Vector3 Coords;
		public float Heading;
		public float Radius;
	}
}