using Newtonsoft.Json;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;

namespace NuovaGM.Client.Lavori
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

	public class LavoriGenerici
	{
		public Pescatori Pescatore = new Pescatori();
	}
}
