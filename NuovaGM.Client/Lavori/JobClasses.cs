using Newtonsoft.Json;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;

namespace TheLastPlanet.Client.Lavori
{
	public class ConfigPolizia
	{
		public ConfigurazionePolizia Config = new ConfigurazionePolizia();
		public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
	}

	public class ConfigMedici
	{
		public ConfigurazioneMedici Config = new ConfigurazioneMedici();
		public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
	}

	public class ConfigVenditoriAuto
	{
		public ConfigurazioneVendAuto Config = new ConfigurazioneVendAuto();
		public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
		public Dictionary<string, List<VeicoloCatalogoVenditore>> Catalogo = new Dictionary<string, List<VeicoloCatalogoVenditore>>();
	}

	public class ConfigVenditoriCase
	{
		public ConfigurazioneVendCase Config = new ConfigurazioneVendCase();
		public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
	}



	public class VeicoloCatalogoVenditore
	{
		public string name;
		public int price;
		public string description;
	}

	public class LavoriGenerici
	{
		public Pescatori Pescatore = new Pescatori();
		public Cacciatori Cacciatore = new Cacciatori();
		public Towing Rimozione = new Towing();
		public Tassisti Tassista = new Tassisti();
	}
}
