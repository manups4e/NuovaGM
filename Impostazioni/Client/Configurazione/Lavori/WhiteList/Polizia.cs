using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Shared;

namespace Impostazioni.Client.Configurazione.Lavori.WhiteList
{
    public class ConfigPolizia
    {
		public ConfigurazionePolizia Config { get; set; }

		public Dictionary<string, JobGrade> Gradi { get; set; }
    }
}