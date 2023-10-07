using System.Collections.Generic;

namespace Settings.Shared.Roleplay.Jobs.WhiteList
{
    public class ConfigPolice
    {
        public PoliceSettings Config { get; set; }

        public Dictionary<string, JobGrade> Grades { get; set; }
    }
}