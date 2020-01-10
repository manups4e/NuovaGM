using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.IPLs.gtav
{
	public class LesterFactory
	{
		public class LesterDetails
		{
			public string BluePrint = "V_53_Agency_Blueprint";   // Blueprint on the office desk
			public string Bag = "V_35_KitBag";                   // Bag under the office desk
			public string FireMan = "V_35_Fireman";              // Firemans helmets in the office
			public string Armour = "V_35_Body_Armour";           // Body armor in storage
			public string GasMask = "Jewel_Gasmasks";                // Gas mask and suit in storage
			public string JanitorStuff = "v_53_agency _overalls";  // Janitor stuff in the storage(yes, there is a whitespace)

			public void Enable(string details, bool state, bool refresh = true)
			{
				IplManager.SetIplPropState(InteriorId, details, state, refresh);
			}
		}

		public static int InteriorId = 92674;
		public static LesterDetails Details = new LesterDetails();

		public static void LoadDefault()
		{
			Details.Enable(Details.BluePrint, false);
			Details.Enable(Details.Bag, false);
			Details.Enable(Details.FireMan, false);
			Details.Enable(Details.Armour, false);
			Details.Enable(Details.GasMask, false);
			Details.Enable(Details.JanitorStuff, false);
			API.RefreshInterior(InteriorId);
		}
	}
}
