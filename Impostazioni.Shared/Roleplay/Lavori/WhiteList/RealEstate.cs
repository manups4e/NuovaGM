using System.Collections.Generic;

namespace Settings.Shared.Roleplay.Jobs.WhiteList
{
    public class ConfigVenditoriCase
    {
        public RealEstateSettings Config { get; set; }

        public Dictionary<string, JobGrade> Grades { get; set; }
    }
}