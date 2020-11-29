using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gta_online
{
	public class HLApartment4
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

		public static int InteriorId = 146945;
		private static bool _enabled = false;
		public static string ipl = "mpbusiness_int_placement_interior_v_mp_apt_h_01_milo__3";
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
			Strip.Enable(Strip.A, true);
			Strip.Enable(Strip.B, true);
			Strip.Enable(Strip.C, true);
			Booze.Enable(Booze.A, true);
			Booze.Enable(Booze.B, true);
			Booze.Enable(Booze.C, true);
			Smoke.Enable(Smoke.A, true);
			Smoke.Enable(Smoke.B, true);
			Smoke.Enable(Smoke.C, true);
			API.RefreshInterior(InteriorId);
		}
	}
}
