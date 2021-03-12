using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_gunrunning
{
	public class GunrunningBunker
	{
		public static int InteriorId = 258561;
		public class BInterior
		{
			private static bool _enabled = false;
			public static string ipl = "gr_grdlc_interior_placement_interior_1_grdlc_int_02_milo_";
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

		public class BExterior
		{
			private static bool _enabled = false;
			public static List<string> ipl = new List<string>()
			{
                "gr_case0_bunkerclosed",	// Desert: 848.6175, 2996.567, 45.81612
                "gr_case1_bunkerclosed",	// SmokeTree: 2126.785, 3335.04, 48.21422
                "gr_case2_bunkerclosed",	// Scrapyard: 2493.654, 3140.399, 51.28789
                "gr_case3_bunkerclosed",	// Oilfields: 481.0465, 2995.135, 43.96672
                "gr_case4_bunkerclosed",	// RatonCanyon: -391.3216, 4363.728, 58.65862
                "gr_case5_bunkerclosed",	// Grapeseed: 1823.961, 4708.14, 42.4991
                "gr_case6_bunkerclosed",	// Farmhouse: 1570.372, 2254.549, 78.89397
                "gr_case7_bunkerclosed",	// Paletto: -783.0755, 5934.686, 24.31475
                "gr_case9_bunkerclosed",	// Route68: 24.43542, 2959.705, 58.35517
                "gr_case10_bunkerclosed",	// Zancudo: -3058.714, 3329.19, 12.5844
                "gr_case11_bunkerclosed"	// Great Ocean Highway: -3180.466, 1374.192, 19.9597
            };
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

		public class BStyle
		{
			public string Default = "Bunker_Style_A";
			public string Blue = "Bunker_Style_B";
			public string Yellow = "Bunker_Style_C";
			public void Set(string style, bool refresh = true)
			{
				Clear(false);
				IplManager.SetIplPropState(InteriorId, style, true, refresh);
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, new List<string>() { "Bunker_Style_A", "Bunker_Style_B", "Bunker_Style_C" }, false, refresh);
			}
		}

		public class BTier
		{
			public string Default = "standard_bunker_set";
			public string Upgrade = "upgrade_bunker_set";

			public void Set(string tier, bool refresh = true)
			{
				Clear(false);
				IplManager.SetIplPropState(InteriorId, tier, true, refresh);
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, new List<string>() { "standard_bunker_set", "upgrade_bunker_set" }, false, refresh);
			}
		}

		public class BSecurity
		{
			public string NoEntryGate = "";
			public string Default = "standard_security_set";
			public string Upgrade = "security_upgrade";

			public void Set(string security, bool refresh = true)
			{
				Clear(false);
				if (security != NoEntryGate)
					IplManager.SetIplPropState(InteriorId, security, true, refresh);
				else
				{
					if (refresh) API.RefreshInterior(InteriorId);
				}
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, new List<string>() { "standard_security_set", "security_upgrade" }, false, refresh);
			}
		}

		public class BDetails
		{
			public string Office = "Office_Upgrade_set";				// Office interior
			public string OfficeLocked = "Office_blocker_set";		// Metal door blocking access to the office
			public string Locker = "gun_locker_upgrade";              // Locker next to the office door
			public string RangeLights = "gun_range_lights";			// Lights next to the shooting range
			public string RangeWall = "gun_wall_blocker";				// Wall blocking access to the shooting range
			public string RangeLocked = "gun_range_blocker_set";      // Metal door blocking access to the shooting range
			public string Schematics = "Gun_schematic_set";			// Gun schematic on the table and whiteboard
			public void Enable(string detail, bool state, bool refresh = true)
			{
				IplManager.SetIplPropState(InteriorId, detail, state, refresh);
			}
		}

		public static BInterior Interior = new BInterior();
		public static BExterior Exterior = new BExterior();
		public static BStyle Style = new BStyle();
		public static BTier Tier = new BTier();
		public static BSecurity Security = new BSecurity();
		public static BDetails Details = new BDetails();

		public static void LoadDefault()
		{
			Interior.Enabled = true;
			Exterior.Enabled = true;

			Style.Set(Style.Default);
			Tier.Set(Tier.Default);
			Security.Set(Security.Default);
			Details.Enable(Details.Office, true);
			Details.Enable(Details.OfficeLocked, false);
			Details.Enable(Details.Locker, true);
			Details.Enable(Details.RangeLights, true);
			Details.Enable(Details.RangeWall, false);
			Details.Enable(Details.RangeLocked, false);
			Details.Enable(Details.Schematics, false);

			API.RefreshInterior(InteriorId);
		}
	}
}
