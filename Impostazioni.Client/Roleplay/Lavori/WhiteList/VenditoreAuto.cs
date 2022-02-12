using System.Collections.Generic;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori;

namespace Impostazioni.Client.Configurazione.Lavori.WhiteList
{
    public class ConfigVenditoriAuto
    {
        public ConfigurazioneVendAuto Config { get; set; }

        Dictionary<string, JobGrade> Gradi { get; set; }

        public Dictionary<string, List<VeicoloCatalogoVenditore>> Catalogo { get; set; }
    }
}