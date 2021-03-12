using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_doomsday
{
	public class DoomsdayAvenger
	{
		public static int InteriorId = 262145;
		private static bool _enabled = false;
		public static string ipl = "xm_x17dlc_int_placement_interior_9_x17dlc_int_01_milo_";
		public static bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				IplManager.EnableIpl(ipl, _enabled);
			}
		}

		public enum Colors
		{
			Utility = 1, Expertise = 2, Altitude = 3,
			Power = 4, Authority = 5, Influence = 6,
			Order = 7, Empire = 8, Supremacy = 9
		}

		public class DWalls
		{
			public void SetColor(Colors color, bool refresh = true)
			{
				API.SetInteriorEntitySetColor(InteriorId, "shell_tint", (int)color);
				if (refresh) API.RefreshInterior(InteriorId);
			}
		}

		public class DTurret
		{
			public string None = "";
			public string Back = "control_1";
			public string Left = "control_2";
			public string Right = "control_3";
			public void Set(string turret, Colors color, bool refresh = true)
			{
				Clear(false);
				if (turret != None)
				{
					IplManager.SetIplPropState(InteriorId, turret, true, refresh);
					API.SetInteriorEntitySetColor(InteriorId, turret, (int)color);
				}
				else
				{
					if (refresh) API.RefreshInterior(InteriorId);
				}
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, new List<string>() { "control_1", "control_2","control_3" }, false, refresh);
			}
		}

		public class DWeaponsMod
		{
			public string Off = "";
			public string On = "weapons_mod";
			public void Set(string mod, Colors color, bool refresh = true)
			{
				Clear(false);
				if (mod != Off)
				{
					IplManager.SetIplPropState(InteriorId, mod, true, refresh);
					API.SetInteriorEntitySetColor(InteriorId, mod, (int)color);
				}
				else
				{
					if (refresh) API.RefreshInterior(InteriorId);
				}
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, "weapons_mod", false, refresh);
			}
		}

		public class DVehicleMod
		{
			public string Off = "";
			public string On = "vehicle_mod";
			public void Set(string mod, Colors color, bool refresh = true)
			{
				Clear(false);
				if (mod != Off)
				{
					IplManager.SetIplPropState(InteriorId, mod, true, refresh);
					API.SetInteriorEntitySetColor(InteriorId, mod, (int)color);
				}
				else
				{
					if (refresh) API.RefreshInterior(InteriorId);
				}
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(InteriorId, "vehicle_mod", false, refresh);
			}
		}

		public class DDetails
		{
			public string Golden = "gold_bling";
			public void Enable(string detail, bool state = true, bool refresh = true)
			{
				IplManager.SetIplPropState(InteriorId, detail, state, refresh);

			}
		}

		public static DWalls Walls = new DWalls();
		public static DTurret Turret = new DTurret();
		public static DWeaponsMod WeaponsMod = new DWeaponsMod();
		public static DVehicleMod VehicleMod = new DVehicleMod();
		public static DDetails Details = new DDetails();

		public static void LoadDefault()
		{
			Enabled = true;

			Walls.SetColor(Colors.Expertise);

			Turret.Set(Turret.Back, (Colors)1, false);
			WeaponsMod.Set(WeaponsMod.On, (Colors)1, false);
			VehicleMod.Set(VehicleMod.On, (Colors)1, false);

			Details.Enable(Details.Golden, false, false);

			API.RefreshInterior(InteriorId);
		}

	}
}
