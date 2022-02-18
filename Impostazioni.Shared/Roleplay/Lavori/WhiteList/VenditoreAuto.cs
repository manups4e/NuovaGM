using System.Collections.Generic;

namespace Impostazioni.Shared.Roleplay.Lavori.WhiteList
{
    public class ConfigVenditoriAuto
    {
        public ConfigurazioneVendAuto Config { get; set; }

        Dictionary<string, JobGrade> Gradi { get; set; }

        public Dictionary<string, List<VeicoloCatalogoVenditore>> Catalogo { get; set; }
    }

    public class VeicoloCatalogoVenditore
    {
        public string name;
        public int price;
        public string description;
    }

}