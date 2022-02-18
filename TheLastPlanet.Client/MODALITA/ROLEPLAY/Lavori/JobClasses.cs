using Impostazioni.Shared.Roleplay.Lavori.Generici;
using Impostazioni.Shared.Roleplay.Lavori.WhiteList;
using System.Collections.Generic;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori
{
    public class _ConfigPolizia
    {
        public ConfigurazionePolizia Config = new ConfigurazionePolizia();
        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
    }

    public class _ConfigMedici
    {
        public ConfigurazioneMedici Config = new ConfigurazioneMedici();
        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
    }

    public class _ConfigVenditoriAuto
    {
        public ConfigurazioneVendAuto Config = new ConfigurazioneVendAuto();
        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
        public Dictionary<string, List<VeicoloCatalogoVenditore>> Catalogo = new Dictionary<string, List<VeicoloCatalogoVenditore>>();
    }

    public class _ConfigVenditoriCase
    {
        public ConfigurazioneVendCase Config = new ConfigurazioneVendCase();
        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
    }

    public class LavoriGenerici
    {
        public Pescatori Pescatore = new Pescatori();
        public Cacciatori Cacciatore = new Cacciatori();
        public Towing Rimozione = new Towing();
        public Tassisti Tassista = new Tassisti();
    }
}