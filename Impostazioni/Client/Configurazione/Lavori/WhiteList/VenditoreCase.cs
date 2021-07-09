using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Shared;

namespace Impostazioni.Client.Configurazione.Lavori.WhiteList
{
    public class ConfigVenditoriCase
    {
        public ConfigurazioneVendCase Config{ get; set; }

        public Dictionary<string, JobGrade> Gradi{ get; set; }
    }
}