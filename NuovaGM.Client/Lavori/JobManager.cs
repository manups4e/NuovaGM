using Newtonsoft.Json;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;

namespace NuovaGM.Client.Lavori
{
	static class JobManager
	{
		public static ConfigPolizia Polizia = new ConfigPolizia();
		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:lavori:polizia", new Action<string, string>(ConfiguraPolizia));
		}
		private static void ConfiguraPolizia(string JsonConfig, string JsonGradi)
		{
			Polizia.Config = JsonConvert.DeserializeObject<ConfigurazionePolizia>(JsonConfig);
			Polizia.Gradi = JsonConvert.DeserializeObject<Dictionary<string, JobGrade>>(JsonGradi);
		}
	}
}
