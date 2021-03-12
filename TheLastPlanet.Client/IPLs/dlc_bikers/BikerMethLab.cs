using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_bikers
{
	public class BikerMethLab
	{
		public static int InteriorId = 247041;
		private static bool _enabled = false;
		public static string ipl = "bkr_biker_interior_placement_interior_2_biker_dlc_int_ware01_milo";
		public static bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				IplManager.EnableIpl(ipl, _enabled);
			}
		}

		public class MethStyle
		{
			public string None = "";
			public string Empty = "meth_lab_empty";
			public List<string> Basic = new List<string>() { "meth_lab_basic", "meth_lab_setup" };
			public List<string> Upgrade = new List<string>() { "meth_lab_upgrade", "meth_lab_setup" };
			public void Set(string style, bool refresh = true)
			{
				Clear(false);
				if (style != None)
					IplManager.SetIplPropState(InteriorId, style, true, refresh);
				else
				{
					if (refresh) API.RefreshInterior(InteriorId);
				}
			}
			public void Set(List<string> style, bool refresh = true)
			{
				Clear(false);
				IplManager.SetIplPropState(InteriorId, style, true, refresh);
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, Empty, false, refresh);
				IplManager.SetIplPropState(InteriorId, Basic, false, refresh);
				IplManager.SetIplPropState(InteriorId, Upgrade, false, refresh);
			}
		}

		public class MethSecurity
		{
			public string None = "";
			public string Upgrade = "meth_lab_security_high";
			public void Set(string style, bool refresh = true)
			{
				Clear(false);
				if (style != None)
					IplManager.SetIplPropState(InteriorId, style, true, refresh);
				else
				{
					if (refresh) API.RefreshInterior(InteriorId);
				}
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, Upgrade, false, refresh);
			}
		}
		public class MethDetails
		{
			public string Production = "meth_lab_production";
			public void Enable(string detail, bool state, bool refresh = true)
			{
				IplManager.SetIplPropState(InteriorId, detail, state, refresh);
			}
		}

		public static MethStyle Style = new MethStyle();
		public static MethSecurity Security = new MethSecurity();
		public static MethDetails Details = new MethDetails();

		public static void LoadDefault()
		{
			Enabled = true;
			Style.Set(Style.Empty);
			Security.Set(Security.None);
			Details.Enable(Details.Production, false);
			API.RefreshInterior(InteriorId);
		}
	}
}
