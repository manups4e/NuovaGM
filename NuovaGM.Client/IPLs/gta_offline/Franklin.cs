using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gtav
{
	public class Franklin
	{
		public class FranklinStyle
		{
			public string Empty = "";
			public string Unpacking = "franklin_unpacking";
			public List<string> Settled = new List<string>() { "franklin_unpacking", "franklin_settled" };
			public string CardBoxes = "showhome_only";

			public void Set(string style, bool refresh = true)
			{
				Clear(false);
				if (style != "")
					IplManager.SetIplPropState(InteriorId, style, true, refresh);
				else
				{
					if (refresh) API.RefreshInterior(InteriorId);
				}
			}

			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, Settled, false, refresh);
				IplManager.SetIplPropState(InteriorId, Unpacking, false, refresh);
				IplManager.SetIplPropState(InteriorId, CardBoxes, false, refresh);
			}
		}

		public class FranklinGlassDoor
		{
			public string Opened = "unlocked";
			public string Closed = "locked";
			public void Set(string door, bool refresh = true)
			{
				Clear(false);
				IplManager.SetIplPropState(InteriorId, door, true, refresh);
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, Opened, false, refresh);
				IplManager.SetIplPropState(InteriorId, Closed, false, refresh);
			}
		}

		public class FranklinDetails
		{
			public string Flyer = "progress_flyer"; // Mountain flyer on the kitchen counter
			public string Tux = "progress_tux";     // Tuxedo suit in the wardrobe
			public string Tshirt = "progress_tshirt";   // "I <3 LS" tshirt on the bed
			public string Bong = "bong_and_wine";       // Bong on the table

			public void Enable(string details, bool state, bool refresh = true)
			{
				IplManager.SetIplPropState(InteriorId, details, state, refresh);
			}
		}


		public static int InteriorId = 206849;
		public static FranklinStyle Style = new FranklinStyle();
		public static FranklinGlassDoor GlassDoor = new FranklinGlassDoor();
		public static FranklinDetails Details = new FranklinDetails();

		public static void LoadDefault()
		{
			Style.Set(Style.Empty);
			GlassDoor.Set(GlassDoor.Opened);
			Details.Enable(Details.Flyer, false);
			Details.Enable(Details.Tux, false);
			Details.Enable(Details.Tshirt, false);
			Details.Enable(Details.Bong, false);
			API.RefreshInterior(InteriorId);
		}
	}
}
