using Settings.Shared.Roleplay.Jobs;
using Settings.Shared.Roleplay.Jobs.Generici;
using Settings.Shared.Roleplay.Jobs.WhiteList;
using System.Collections.Generic;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori
{
    public class _ConfigPolizia
    {
        public PoliceSettings Config = new PoliceSettings();
        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
    }

    public class _ConfigMedici
    {
        public MedicsSettings Config = new MedicsSettings();
        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
    }

    public class _ConfigVenditoriAuto
    {
        public CarDealerSettings Config = new CarDealerSettings();
        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
        public Dictionary<string, List<VehiclesCatalog>> Catalogo = new Dictionary<string, List<VehiclesCatalog>>();
    }

    public class _ConfigVenditoriCase
    {
        public RealEstateSettings Config = new RealEstateSettings();
        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>();
    }

    public class ConfigLavoriGenerici
    {
        public Disoccupato Disoccupato = new Disoccupato();
        public Pescatori Pescatore = new Pescatori();
        public Cacciatori Cacciatore = new Cacciatori();
        public Towing Rimozione = new Towing();
        public Tassisti Tassista = new Tassisti();
    }
}