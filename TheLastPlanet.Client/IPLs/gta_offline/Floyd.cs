using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gtav
{
	public class Floyd
	{
		public class FloydStyle
		{
			public List<string> Normal = new List<string>();
			public List<string> MessedUp = new List<string>();
			public string Jammed;
			public string JammedOnTable;
			public void Set(List<string> style, bool refresh = true)
			{
				Clear(false);
				IplManager.SetIplPropState(InteriorId, style, true, refresh);
			}

			public void Clear(bool refresh, int style = 1)
			{
				if (style == 1)
				{
					IplManager.SetIplPropState(InteriorId, Normal, false, refresh);
					IplManager.SetIplPropState(InteriorId, MessedUp, false, refresh);
				}
				else if (style == 2)
				{
					IplManager.SetIplPropState(InteriorId, Normal, false, refresh);
					IplManager.SetIplPropState(InteriorId, Jammed, false, refresh);
					IplManager.SetIplPropState(InteriorId, JammedOnTable, false, refresh);
				}
			}

		}

		public static int InteriorId = 171777;
		public static FloydStyle Style = new FloydStyle()
		{
			Normal = new List<string>() { "swap_clean_apt", "layer_debra_pic", "layer_whiskey", "swap_sofa_A" },
			MessedUp = new List<string>() { "layer_mess_A", "layer_mess_B", "layer_mess_C", "layer_sextoys_a", "swap_sofa_B", "swap_wade_sofa_A", "layer_wade_shit", "layer_torture" },
		};
		public static FloydStyle MrJam = new FloydStyle()
		{
			Normal = new List<string>() { "swap_mrJam_A" },
			Jammed = "swap_mrJam_B",
			JammedOnTable = "swap_mrJam_C"
		};

		public static void LoadDefault()
		{
			Style.Set(Style.Normal, false);
			MrJam.Set(MrJam.Normal, false);
			API.RefreshInterior(InteriorId);
		}
	}

}
