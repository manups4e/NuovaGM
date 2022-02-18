using System.Collections.Generic;

namespace Impostazioni.Shared.Roleplay.Lavori.WhiteList
{
    public class ConfigVenditoriCase
    {
        public ConfigurazioneVendCase Config { get; set; }

        public Dictionary<string, JobGrade> Gradi { get; set; }
    }
}