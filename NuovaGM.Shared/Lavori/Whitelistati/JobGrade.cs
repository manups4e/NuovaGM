using System.Collections.Generic;

namespace NuovaGM.Shared
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
		public List<Ospedale> Ospedali = new List<Ospedale>();
	}


	public class WhiteListed
	{
		public Dictionary<string, JobGrade> Polizia = new Dictionary<string, JobGrade>();
		public Dictionary<string, JobGrade> Medico = new Dictionary<string, JobGrade>();
		public Dictionary<string, JobGrade> Meccanico = new Dictionary<string, JobGrade>();

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
		public List<float[]> Spogliatoio = new List<float[]>();
		public List<float[]> Armerie = new List<float[]>();
		public List<Autorizzati> VeicoliAutorizzati = new List<Autorizzati>();
		public List<Autorizzati> ElicotteriAutorizzati = new List<Autorizzati>();
		public List<Autorizzati> ArmiAutorizzate = new List<Autorizzati>();
		public List<SpawnerSpawn> Veicoli = new List<SpawnerSpawn>();
		public List<SpawnerSpawn> Elicotteri = new List<SpawnerSpawn>();
		public List<float[]> AzioniCapo = new List<float[]>();
	}

	public class Ospedale
	{
		public BlipLavoro Blip = new BlipLavoro();
		public List<float[]> Spogliatoio = new List<float[]>();
		public List<float[]> Farmacia = new List<float[]>();
		public List<float[]> IngressoVisitatori = new List<float[]>();
		public List<Autorizzati> VeicoliAutorizzati = new List<Autorizzati>();
		public List<Autorizzati> ElicotteriAutorizzati = new List<Autorizzati>();
		public List<SpawnerSpawn> Veicoli = new List<SpawnerSpawn>();
		public List<SpawnerSpawn> Elicotteri = new List<SpawnerSpawn>();
		public List<float[]> AzioniCapo = new List<float[]>();
	}

	public class BlipLavoro
	{
		public float[] Coords = new float[3];
		public int Sprite;
		public int Display;
		public float Scale;
		public int Color;
		public string Nome;
	}
	public class SpawnerSpawn
	{
		public float[] SpawnerMenu = new float[3];
		public List<SpawnPoints> SpawnPoints = new List<SpawnPoints>();
		public List<float[]> Deleters = new List<float[]>();
	}

	public class SpawnPoints
	{
		public float[] Coords = new float[3];
		public float Heading;
		public float Radius;
	}
}