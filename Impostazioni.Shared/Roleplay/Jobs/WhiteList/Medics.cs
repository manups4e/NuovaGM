﻿using System.Collections.Generic;

namespace Settings.Shared.Roleplay.Jobs.WhiteList
{
    public class ConfigMedics
    {
        public MedicsSettings Config { get; set; }
        public Dictionary<string, JobGrade> Grades { get; set; }
    }
}