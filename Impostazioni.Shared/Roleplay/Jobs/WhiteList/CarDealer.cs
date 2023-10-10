using System.Collections.Generic;

namespace Settings.Shared.Roleplay.Jobs.WhiteList
{
    public class ConfigCarDealer
    {
        public CarDealerSettings Config { get; set; }

        Dictionary<string, JobGrade> Settings { get; set; }

        public Dictionary<string, List<VehiclesCatalog>> Catalog { get; set; }
    }

    public class VehiclesCatalog
    {
        public string Name;
        public int Price;
        public string Description;
    }

}