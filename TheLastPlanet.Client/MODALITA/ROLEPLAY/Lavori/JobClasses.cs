using Newtonsoft.Json;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using Impostazioni.Client.Configurazione.Lavori.Generici;

namespace TheLastPlanet.Client.RolePlay.Lavori
{
	public class _ConfigPolizia
	{
		public ConfigurazionePolizia Config = new ConfigurazionePolizia();
		public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
	}

	public class _ConfigMedici
	{
		public ConfigurazioneMedici Config = new ConfigurazioneMedici();
		public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
	}

	public class _ConfigVenditoriAuto
	{
		public ConfigurazioneVendAuto Config = new ConfigurazioneVendAuto();
		public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
		public Dictionary<string, List<VeicoloCatalogoVenditore>> Catalogo = new Dictionary<string, List<VeicoloCatalogoVenditore>>();
	}

	public class _ConfigVenditoriCase
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