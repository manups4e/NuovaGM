﻿namespace TheLastPlanet.Client.IPLs.gta_online
{
    public class HLApartment2 : OnlineApartament
    {
        private bool _enabled = false;
        public string ipl = "mpbusiness_int_placement_interior_v_mp_apt_h_01_milo__1";
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                IplManager.EnableIpl(ipl, _enabled);
            }
        }

        public HLApartment2()
        {
            InteriorId = 146177;
            Strip = new Style(InteriorId, "Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C");
            Booze = new Style(InteriorId, "Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C");
            Smoke = new Style(InteriorId, "Apart_Hi_Smoke_A", "Apart_Hi_Smoke_B", "Apart_Hi_Smoke_C");
        }

        public override void LoadDefault()
        {
            Enabled = true;
            base.LoadDefault();
        }
    }
}
