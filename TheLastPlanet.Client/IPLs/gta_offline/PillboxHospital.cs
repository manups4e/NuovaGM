﻿using System.Collections.Generic;

namespace TheLastPlanet.Client.IPLs.gtav
{
    public class PillboxHospital
    {
        private bool _enabled = false;
        public List<string> ipl = new List<string>() { "rc12b_default" };
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                IplManager.EnableIpl(ipl, _enabled);
            }
        }
    }
}
