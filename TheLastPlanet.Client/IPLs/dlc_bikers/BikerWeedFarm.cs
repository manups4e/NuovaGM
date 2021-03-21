﻿using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_bikers
{
	public class BikerWeedFarm
	{
		public static int InteriorId = 247297;
		private static bool _enabled = false;
		public static string ipl = "bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo";
		public static bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				IplManager.EnableIpl(ipl, _enabled);
			}
		}

		public class WeedStyle
		{
			public string Basic = "weed_standard_equip";
			public string Upgrade = "weed_upgrade_equip";
			public void Set(string style, bool refresh = true)
			{
				Clear(false);
				IplManager.SetIplPropState(InteriorId, style, true, refresh);
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, new List<string>() { "weed_standard_equip", "weed_upgrade_equip" }, false, refresh);
			}
		}

		public class WeedSecurity
		{
			public string Basic = "weed_low_security";
			public string Upgrade = "weed_security_upgrade";
			public void Set(string security, bool refresh = true)
			{
				Clear(false);
				IplManager.SetIplPropState(InteriorId, security, true, refresh);
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, new List<string>() { "weed_low_security", "weed_security_upgrade" }, false, refresh);
			}
		}
		public class WeedPlants
		{
			public static string A;
			public static string B;
			public static string C;
			public static string D;
			public static string E;
			public static string F;
			public class WStage
			{
				public string Small;
				public string Medium;
				public string Full;
				public WStage(string sm, string med, string full)
				{
					Small = sm;
					Medium = med;
					Full = full;
				}
				public void Set(string stage, bool refresh = true)
				{
					Clear(false);
					IplManager.SetIplPropState(InteriorId, stage, true, refresh);
				}
				public void Clear(bool refresh)
				{
					List<string> Lista = new List<string>();
					Lista.Add(Small);
					Lista.Add(Medium);
					Lista.Add(Full);
					IplManager.SetIplPropState(InteriorId, Lista, false, refresh);
				}
			}
			public class WLight
			{
				public string Basic;
				public string Upgrade;
				public WLight(string bas, string upg)
				{
					Basic = bas;
					Upgrade = upg;
				}
				public void Set(string light, bool refresh = true)
				{
					Clear(false);
					IplManager.SetIplPropState(InteriorId, light, true, refresh);
				}
				public void Clear(bool refresh)
				{
					List<string> Lista = new List<string>();
					Lista.Add(Basic);
					Lista.Add(Upgrade);
					IplManager.SetIplPropState(InteriorId, Lista, false, refresh);
				}
			}

			public class WHose
			{
				public string Hose;
				public WHose(string hose) { Hose = hose; }
				public void Enable(bool state, bool refresh = true)
				{
					IplManager.SetIplPropState(InteriorId, Hose, state, refresh);
				}
			}

			public WeedPlants(string a, string b, string c, string d, string e, string f)
			{
				A = a;
				B = b;
				C = c;
				D = d;
				E = e;
				F = f;
			}
			public WStage Stage = new WStage(A, B, C);
			public WLight Light = new WLight(D, E);
			public WHose Hose = new WHose(F);

			public void Set(string stage, string upgrade, bool refresh = true)
			{
				Stage.Set(stage, false);
				Light.Set(upgrade, false);
				Hose.Enable(true, true);
			}
			public void Clear()
			{
				Stage.Clear(true);
				Light.Clear(true);
				Hose.Enable(false, true);
			}
		}

		public class WeedDetails
		{
			public string Production = "weed_production";	// Weed on the tables
			public string Fans = "weed_set_up";			// Fans + mold buckets
			public string Drying = "weed_drying";			// Drying weed hooked to the ceiling
			public string Chairs = "weed_chairs";           // Chairs at the tables

			public void Enable(string detail, bool state, bool refresh = true)
			{
				IplManager.SetIplPropState(InteriorId, detail, state, refresh);
			}
		}

		public static WeedStyle Style = new WeedStyle();
		public static WeedSecurity Security = new WeedSecurity();
		public static WeedPlants Plant1 = new WeedPlants("weed_growtha_stage1", "weed_growtha_stage2", "weed_growtha_stage3", "light_growtha_stage23_standard", "light_growtha_stage23_upgrade", "weed_hosea");
		public static WeedPlants Plant2 = new WeedPlants("weed_growthb_stage1", "weed_growthb_stage2", "weed_growthb_stage3", "light_growthb_stage23_standard", "light_growthb_stage23_upgrade", "weed_hoseb");
		public static WeedPlants Plant3 = new WeedPlants("weed_growthc_stage1", "weed_growthc_stage2", "weed_growthc_stage3", "light_growthc_stage23_standard", "light_growthc_stage23_upgrade", "weed_hosec");
		public static WeedPlants Plant4 = new WeedPlants("weed_growthd_stage1", "weed_growthd_stage2", "weed_growthd_stage3", "light_growthd_stage23_standard", "light_growthd_stage23_upgrade", "weed_hosed");
		public static WeedPlants Plant5 = new WeedPlants("weed_growthe_stage1", "weed_growthe_stage2", "weed_growthe_stage3", "light_growthe_stage23_standard", "light_growthe_stage23_upgrade", "weed_hosee");
		public static WeedPlants Plant6 = new WeedPlants("weed_growthf_stage1", "weed_growthf_stage2", "weed_growthf_stage3", "light_growthf_stage23_standard", "light_growthf_stage23_upgrade", "weed_hosef");
		public static WeedPlants Plant7 = new WeedPlants("weed_growthg_stage1", "weed_growthg_stage2", "weed_growthg_stage3", "light_growthg_stage23_standard", "light_growthg_stage23_upgrade", "weed_hoseg");
		public static WeedPlants Plant8 = new WeedPlants("weed_growthh_stage1", "weed_growthh_stage2", "weed_growthh_stage3", "light_growthh_stage23_standard", "light_growthh_stage23_upgrade", "weed_hoseh");
		public static WeedPlants Plant9 = new WeedPlants("weed_growthi_stage1", "weed_growthi_stage2", "weed_growthi_stage3", "light_growthi_stage23_standard", "light_growthi_stage23_upgrade", "weed_hosei");
		public static WeedDetails Details = new WeedDetails();

		public static void LoadDefault()
		{
			Enabled = true;

			Style.Set(Style.Upgrade);

			Security.Set(Security.Basic);

			Details.Enable(Details.Drying, false);
			Details.Enable(Details.Chairs, false);
			Details.Enable(Details.Production, false);
			Details.Enable(Details.Drying, true);
			Details.Enable(Details.Chairs, true);
			Details.Enable(Details.Production, true);

			Plant1.Set(Plant1.Stage.Medium, Plant1.Light.Basic);
			Plant2.Set(Plant2.Stage.Full, Plant2.Light.Basic);
			Plant3.Set(Plant3.Stage.Medium, Plant3.Light.Basic);
			Plant4.Set(Plant4.Stage.Full, Plant4.Light.Basic);
			Plant5.Set(Plant5.Stage.Medium, Plant5.Light.Basic);
			Plant6.Set(Plant6.Stage.Full, Plant6.Light.Basic);
			Plant7.Set(Plant7.Stage.Medium, Plant7.Light.Basic);
			Plant8.Set(Plant8.Stage.Full, Plant8.Light.Basic);
			Plant9.Set(Plant9.Stage.Full, Plant9.Light.Basic);

			API.RefreshInterior(InteriorId);
		}
	}
}