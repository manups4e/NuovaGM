using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.IPLs.gtav
{
	public class Simeon
	{
		private static bool _enabled = false;
		public static List<string> ipl = new List<string>() { "shr_int" };
		public static bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				IplManager.EnableIpl(ipl, _enabled);
			}
		}

		public class SimeonStyle
		{
			public string Normal = "csr_beforeMission";
			public string NoGlass = "csr_inMission";
			public string Destroyed = "csr_afterMissionA";
			public string Fixed = "csr_afterMissionB";

			public void Set(string style, bool refresh = true)
			{
				Clear(false);
				IplManager.SetIplPropState(InteriorId, style, true, refresh);
			}

			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, Normal, false, refresh);
				IplManager.SetIplPropState(InteriorId, NoGlass, false, refresh);
				IplManager.SetIplPropState(InteriorId, Destroyed, false, refresh);
				IplManager.SetIplPropState(InteriorId, Fixed, false, refresh);
			}
		}

		public class SimeonShutter
		{
			public string None = "";
			public string Opened = "shutter_open";
			public string Closed = "shutter_closed";
			public void Set(string shutter, bool refresh = true)
			{
				Clear(false);
				IplManager.SetIplPropState(InteriorId, shutter, true, refresh);
			}

			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, None, false, refresh);
				IplManager.SetIplPropState(InteriorId, Opened, false, refresh);
				IplManager.SetIplPropState(InteriorId, Closed, false, refresh);
			}
		}

		public static int InteriorId = 7170;
		public static SimeonStyle Style = new SimeonStyle();
		public static SimeonShutter Shutter = new SimeonShutter();

		public static void LoadDefault()
		{
			Enabled = true;
			Style.Set(Style.Normal);
			Shutter.Set(Shutter.Opened);
			API.RefreshInterior(InteriorId);
		}
	}
}
