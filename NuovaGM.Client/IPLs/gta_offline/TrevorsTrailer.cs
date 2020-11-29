﻿using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gtav
{
	public class TrevorsTrailer
	{
		public class TrevorsInterior
		{
			public string Tidy = "trevorstrailertidy";
			public string Trash = "TrevorsTrailerTrash";
			public void Set(string interior)
			{
				Clear();
				IplManager.EnableIpl(interior, true);
			}
			public void Clear()
			{
				IplManager.EnableIpl(Tidy, false);
				IplManager.EnableIpl(Trash, false);
			}
		}

		public class TrevorsDetails
		{
			public string CopHelmet = "V_26_Trevor_Helmet3";	  // Cop helmet in the closet
			public string Briefcase = "V_24_Trevor_Briefcase3"; // Briefcase in the main room
			public string MichaelStuff = "V_26_Michael_Stay3";  // Michael's suit hanging on the window

			public void Enable(string details, bool state, bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, details, state, refresh);
			}
		}

		public static int InteriorId = 2562;
		public static TrevorsInterior Interior = new TrevorsInterior();
		public static TrevorsDetails Details = new TrevorsDetails();
		
		public static void LoadDefault()
		{
			Interior.Set(Interior.Trash);
			Details.Enable(Details.CopHelmet, false, false);
			Details.Enable(Details.Briefcase, false, false);
			Details.Enable(Details.MichaelStuff, false, false);
			API.RefreshInterior(InteriorId);
		}
	}
}
