using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Shared;

namespace Impostazioni.Client.Configurazione.Lavori.WhiteList
{
    public class ConfigMedici
    {
        public ConfigurazioneMedici Config { get; set; }
        public Dictionary<string, JobGrade> Gradi { get; set; }
    }
}