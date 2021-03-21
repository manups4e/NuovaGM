﻿using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gta_online
{
	public class GTAOHouseLow1
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

			public void Set(string smoke, bool refresh)
			{
				if (smoke != "")
				{
					if (smoke.Contains("Smoke"))
					{
						Clear(false);
						IplManager.SetIplPropState(InteriorId, smoke, true, refresh);
					}
				}
				else
				{
					if (refresh) API.RefreshInterior(InteriorId);
				}
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, Smoke.A, false);
				IplManager.SetIplPropState(InteriorId, Smoke.B, false);
				IplManager.SetIplPropState(InteriorId, Smoke.C, false);
			}
		}

		public static int InteriorId = 149761;
		public static Style Strip = new Style("Studio_Lo_Strip_A", "Studio_Lo_Strip_B", "Studio_Lo_Strip_C");
		public static Style Booze = new Style("Studio_Lo_Booze_A", "Studio_Lo_Booze_B", "Studio_Lo_Booze_C");
		public static Style Smoke = new Style("Studio_Lo_Smoke_A", "Studio_Lo_Smoke_B", "Studio_Lo_Smoke_C");

		public static void LoadDefault()
		{
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