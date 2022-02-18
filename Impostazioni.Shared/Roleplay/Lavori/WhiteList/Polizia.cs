using System.Collections.Generic;

namespace Impostazioni.Shared.Roleplay.Lavori.WhiteList
{
    public class ConfigPolizia
    {
        public ConfigurazionePolizia Config { get; set; }

        public Dictionary<string, JobGrade> Gradi { get; set; }
    }
}