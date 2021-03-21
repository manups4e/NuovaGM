﻿using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gta_online
{
	public class HLApartment5
	{
		public class Style
		{
			public string A;
			public string B;
			public string C;

			public Style(string a, string b, string c)
			{
				A = a;
				B = b;
				C = c;
			}

			public void Enable(string details, bool state, bool refresh = true)
			{
				IplManager.SetIplPropState(InteriorId, details, state, refresh);
			}
		}

		public static int InteriorId = 147201;
		private static bool _enabled = false;
		public static string ipl = "mpbusiness_int_placement_interior_v_mp_apt_h_01_milo__4";
		public static bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				IplManager.EnableIpl(ipl, _enabled);
			}
		}

		public static Style Strip = new Style("Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C");
		public static Style Booze = new Style("Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C");
		public static Style Smoke = new Style("Apart_Hi_Smoke_A", "Apart_Hi_Smoke_B", "Apart_Hi_Smoke_C");

		public static void LoadDefault()
		{
			Enabled = true;
			Strip.Enable(Strip.A, false);
			Strip.Enable(Strip.B, false);
			Strip.Enable(Strip.C, false);
			Booze.Enable(Booze.A, false);
			Booze.Enable(Booze.B, false);
			Booze.Enable(Booze.C, false);
			Smoke.Enable(Smoke.A, false);
			Smoke.Enable(Smoke.B, false);
			Smoke.Enable(Smoke.C, false);
			API.RefreshInterior(InteriorId);
		}
	}
}