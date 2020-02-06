using NuovaGM.Shared;
using System.Collections.Generic;

namespace NuovaGM.Client
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
}
