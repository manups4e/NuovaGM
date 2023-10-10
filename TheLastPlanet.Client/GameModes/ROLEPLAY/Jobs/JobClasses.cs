using Settings.Shared.Roleplay.Jobs;
using Settings.Shared.Roleplay.Jobs.Generics;
using Settings.Shared.Roleplay.Jobs.WhiteList;
using System.Collections.Generic;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs
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
        public Dictionary<string, JobGrade> Grades = new Dictionary<string, JobGrade>();
        public Dictionary<string, List<VehiclesCatalog>> Catalogue = new Dictionary<string, List<VehiclesCatalog>>();
    }

    public class _ConfigVenditoriCase
    {
        public RealEstateSettings Config = new RealEstateSettings();
        public Dictionary<string, JobGrade> Grades = new Dictionary<string, JobGrade>();
    }

    public class ConfigLavoriGenerici
    {
        public Unemployed Unemployed = new Unemployed();
        public Fishermen Fisherman = new Fishermen();
        public Hunters Hunter = new Hunters();
        public Towing Towing = new Towing();
        public Tassisti TaxiDriver = new Tassisti();
    }
}