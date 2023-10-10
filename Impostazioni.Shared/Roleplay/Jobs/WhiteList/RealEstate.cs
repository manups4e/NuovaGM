using System.Collections.Generic;

namespace Settings.Shared.Roleplay.Jobs.WhiteList
{
    public class ConfigHouseDealer
    {
        public RealEstateSettings Config { get; set; }

        public Dictionary<string, JobGrade> Grades { get; set; }
    }
}