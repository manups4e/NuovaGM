using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.IPLs.dlc_import_export
{
	public class ImportVehicleWarehouse
	{
		public class VehUpper
		{
			public static int InteriorId = 252673;
			private static bool _enabled = false;
			public static string ipl = "imp_impexp_interior_placement_interior_1_impexp_intwaremed_milo_";
			public bool Enabled
			{
				get { return _enabled; }
				set
				{
					_enabled = value;
					IplManager.EnableIpl(ipl, _enabled);
				}
			}

			public class UpperStyle
			{
				public string Basic = "basic_style_set";
				public string Branded = "branded_style_set";
				public string Urban = "urban_style_set";
				public void Set(string style, bool refresh = true)
				{
					Clear(false);
					IplManager.SetIplPropState(InteriorId, style, true, refresh);
				}
				public void Clear(bool refresh)
				{
					IplManager.SetIplPropState(InteriorId, new List<string>() { "basic_style_set", "branded_style_set", "urban_style_set" }, false, refresh);
				}
			}

			public class UpperDetails
			{
				public string FloorHatch = "car_floor_hatch";
	            public string DoorBlocker = "door_blocker";       // Invisible wall
				public void Enable(string detail, bool state, bool refresh = true)
				{
					IplManager.SetIplPropState(InteriorId, detail, state, refresh);
				}
			}
			public UpperStyle Style = new UpperStyle();
			public UpperDetails Details = new UpperDetails();
		}

		public class VehLower
		{
			public static int InteriorId = 253185;
			private static bool _enabled = false;
			public static string ipl = "imp_impexp_interior_placement_interior_3_impexp_int_02_milo_";
			public bool Enabled
			{
				get { return _enabled; }
				set
				{
					_enabled = value;
					IplManager.EnableIpl(ipl, _enabled);
				}
			}

			public class LowerDetails
			{
				public class DPumps
				{
					public string pump1 = "pump_01";
					public string pump2 = "pump_02";
					public string pump3 = "pump_03";
					public string pump4 = "pump_04";
					public string pump5 = "pump_05";
					public string pump6 = "pump_06";
					public string pump7 = "pump_07";
					public string pump8 = "pump_08";
				}
				public DPumps Pumps = new DPumps();
				public void Enable(string detail, bool state = true, bool refresh = true)
				{
					IplManager.SetIplPropState(InteriorId, detail, state, refresh);
				}
			}
			public LowerDetails Details = new LowerDetails();
		}

		public static VehUpper Upper = new VehUpper();
		public static VehLower Lower = new VehLower();

		public static void LoadDefault()
		{
			Upper.Enabled = true;
			Upper.Style.Set(Upper.Style.Branded);
			Upper.Details.Enable(Upper.Details.FloorHatch, true);
			Upper.Details.Enable(Upper.Details.DoorBlocker, false);
			API.RefreshInterior(VehUpper.InteriorId);

			Lower.Enabled = true;
			Lower.Details.Enable(Lower.Details.Pumps.pump1);
			Lower.Details.Enable(Lower.Details.Pumps.pump2);
			Lower.Details.Enable(Lower.Details.Pumps.pump3);
			Lower.Details.Enable(Lower.Details.Pumps.pump4);
			Lower.Details.Enable(Lower.Details.Pumps.pump5);
			Lower.Details.Enable(Lower.Details.Pumps.pump6);
			Lower.Details.Enable(Lower.Details.Pumps.pump7);
			Lower.Details.Enable(Lower.Details.Pumps.pump8);
		}

	}
}
