using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gtav
{
	public class StripClub
	{
		public static int InteriorId = 197121;

		public static string Mess = "V_19_Trevor_Mess";
		public static void Enable(bool state)
		{
			IplManager.SetIplPropState(InteriorId, Mess, state, true);
		}

		public static void LoadDefault()
		{
			Enable(false);
		}

	}
}
