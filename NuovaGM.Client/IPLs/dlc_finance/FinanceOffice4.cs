using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_finance
{
	public class FinanceOffice4
	{
		public class OfficeSafeDoors
		{
			public int HashL;
			public int HashR;
		}

		public class OfficeFinanceStyle
		{
			public class Theme
			{
				public int InteriorId;
				public string Ipl;
				public string Safe;
				public Theme(int inter, string ipl, string safe)
				{
					InteriorId = inter;
					Ipl = ipl;
					Safe = safe;
					ThemeInternal.Add(this);
				}
			}
			public Theme Warm = new Theme(243201, "ex_sm_15_office_01a", "ex_prop_safedoor_office1a");
            public Theme Classical = new Theme(243457, "ex_sm_15_office_01b", "ex_prop_safedoor_office1b");
            public Theme Vintage = new Theme(243713, "ex_sm_15_office_01c", "ex_prop_safedoor_office1c");
            public Theme Contrast = new Theme(243969, "ex_sm_15_office_02a", "ex_prop_safedoor_office2a");
            public Theme Rich = new Theme(244225, "ex_sm_15_office_02b", "ex_prop_safedoor_office2a");
            public Theme Cool = new Theme(244481, "ex_sm_15_office_02c", "ex_prop_safedoor_office2a");
            public Theme Ice = new Theme(244737, "ex_sm_15_office_03a", "ex_prop_safedoor_office3a");
            public Theme Conservative = new Theme(244993, "ex_sm_15_office_03b", "ex_prop_safedoor_office3a");
            public Theme Polished = new Theme(245249, "ex_sm_15_office_03c", "ex_prop_safedoor_office3c");

			public void Set(Theme style, bool refresh = false)
			{
				Clear();
				CurrentInteriorId = style.InteriorId;
				CurrentSafeDoors.HashL = API.GetHashKey(style.Safe + "_l");
				CurrentSafeDoors.HashR = API.GetHashKey(style.Safe + "_r");
				IplManager.EnableIpl(style.Ipl, true);
				if (refresh) API.RefreshInterior(CurrentInteriorId);
			}
			public void Clear()
			{
				foreach (var themeValue in ThemeInternal)
				{
					foreach (var swagValue in SwagInternal)
					{
						IplManager.SetIplPropState(themeValue.InteriorId, swagValue.A, false);
						IplManager.SetIplPropState(themeValue.InteriorId, swagValue.B, false);
						IplManager.SetIplPropState(themeValue.InteriorId, swagValue.C, false);
					}
					IplManager.SetIplPropState(themeValue.InteriorId, "office_chairs", false, false);
					IplManager.SetIplPropState(themeValue.InteriorId, "office_booze", false, true);
					CurrentSafeDoors = new OfficeSafeDoors();
					IplManager.EnableIpl(themeValue.Ipl, false);
				}
			}
		}

		public class OfficeCassaForte
		{
			public float DoorHeadingL = 188f;
			public Vector3 Position = new Vector3(-1372.905f, -462.08f, 72.05f);
			public bool IsLeftDoorOpen = false;
			public bool IsRightDoorOpen = false;

			public void Open(string side)
			{
				if (side.ToLower() == "left" || side.ToLower() == "sinistra") IsLeftDoorOpen = true;
				else if (side.ToLower() == "right" || side.ToLower() == "destra") IsRightDoorOpen = true;
				else
					Log.Printa(LogType.Debug, "Direzioni solo destra e sinistra (left right)");
			}
			public void Close(string side)
			{
				if (side.ToLower() == "left" || side.ToLower() == "sinistra") IsLeftDoorOpen = false;
				else if (side.ToLower() == "right" || side.ToLower() == "destra") IsRightDoorOpen = false;
				else
					Log.Printa(LogType.Debug,"Direzioni solo destra e sinistra (left right)");
			}
			public async void SetDoorState(string doorSide, bool open)
			{
				int doorHandle = 0;
				float heading = DoorHeadingL;
				if (doorSide.ToLower() == "left" || doorSide.ToLower() == "sinistra")
				{
					doorHandle = await GetDoorHandle(CurrentSafeDoors.HashL);
					if (open) heading -= 90;
				}
				else if (doorSide.ToLower() == "right" || doorSide.ToLower() == "destra")
				{
					doorHandle = await GetDoorHandle(CurrentSafeDoors.HashR);
					if (open) heading += 90;
				}

				if (doorHandle == 0)
					Log.Printa(LogType.Debug,"Errore nell'handle della porta");
				API.SetEntityHeading(doorHandle, heading);
			}

			public async Task<int> GetDoorHandle(int doorHash)
			{
				int timeout = 4;
				int doorHandle = API.GetClosestObjectOfType(Position.X, Position.Y, Position.Z, 5.0f, (uint)doorHash, false, false, false);

				while (doorHandle == 0)
				{
					await BaseScript.Delay(25);
					doorHandle = API.GetClosestObjectOfType(Position.X, Position.Y, Position.Z, 5.0f, (uint)doorHash, false, false, false);
					timeout--;
					if (timeout <= 0)
						break;
				}
				return doorHandle;
			}
		}

		public class OfficeSwag
		{
			public class Cashing
			{
				public string A = "cash_set_01"; public string B = "cash_set_02"; public string C = "cash_set_03"; public string D = "cash_set_04"; public string E = "cash_set_05";
				public string F = "cash_set_06"; public string G = "cash_set_07"; public string H = "cash_set_08"; public string I = "cash_set_09"; public string J = "cash_set_10";
				public string K = "cash_set_11"; public string L = "cash_set_12"; public string M = "cash_set_13"; public string N = "cash_set_14"; public string O = "cash_set_15";
				public string P = "cash_set_16"; public string Q = "cash_set_17"; public string R = "cash_set_18"; public string S = "cash_set_19"; public string T = "cash_set_20";
				public string U = "cash_set_21"; public string V = "cash_set_22"; public string W = "cash_set_23"; public string X = "cash_set_24";
			}
			public class Swagging
			{
				public string A;
				public string B;
				public string C;
				public Swagging(string a, string b, string c)
				{
					A = a;
					B = b;
					C = c;
					SwagInternal.Add(this);
				}
			}
			public Cashing Cash = new Cashing();
			public Swagging BoozeCigs = new Swagging ("swag_booze_cigs", "swag_booze_cigs2", "swag_booze_cigs3");
			public Swagging Counterfeit = new Swagging ("swag_counterfeit", "swag_counterfeit2", "swag_counterfeit3");
			public Swagging DrugBags = new Swagging ("swag_drugbags", "swag_drugbags2", "swag_drugbags3");
			public Swagging DrugStatue = new Swagging ("swag_drugstatue", "swag_drugstatue2", "swag_drugstatue3");
			public Swagging Electronic = new Swagging ("swag_electronic", "swag_electronic2", "swag_electronic3");
			public Swagging FurCoats = new Swagging ("swag_furcoats", "swag_furcoats2", "swag_furcoats3");
			public Swagging Gems = new Swagging ("swag_gems", "swag_gems2", "swag_gems3");
			public Swagging Guns = new Swagging ("swag_guns", "swag_guns2", "swag_guns3");
			public Swagging Ivory = new Swagging ("swag_ivory", "swag_ivory2", "swag_ivory3");
			public Swagging Jewel = new Swagging ("swag_jewelwatch", "swag_jewelwatch2", "swag_jewelwatch3");
			public Swagging Med = new Swagging ("swag_med", "swag_med2", "swag_med3");
			public Swagging Painting = new Swagging ("swag_art", "swag_art2", "swag_art3");
			public Swagging Pills = new Swagging ("swag_pills", "swag_pills2", "swag_pills3");
			public Swagging Silver = new Swagging ("swag_silver", "swag_silver2", "swag_silver3");

			public void Enable(string style, bool state, bool refresh)
			{
				IplManager.SetIplPropState(CurrentInteriorId, style, state, refresh);
			}
		}

		public class OfficeChairs
		{
			public string Off = "";
			public string On = "office_chairs";
			public void Set(string chair, bool refresh)
			{
				Clear(false);
				if (chair != "")
					IplManager.SetIplPropState(CurrentInteriorId, chair, true, refresh);
				else
				{
					if (refresh) API.RefreshInterior(CurrentInteriorId);
				}
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(CurrentInteriorId, On, false, refresh);
			}
		}

		public class OfficeBooze
		{
			public string Off = "";
			public string On = "office_booze";
			public void Set(string booze, bool refresh)
			{
				Clear(false);
				if (booze != "")
					IplManager.SetIplPropState(CurrentInteriorId, booze, true, refresh);
				else
				{
					if (refresh) API.RefreshInterior(CurrentInteriorId);
				}
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(CurrentInteriorId, On, false, refresh);
			}
		}

		public static int CurrentInteriorId = -1;
		private static List<OfficeSwag.Swagging> SwagInternal = new List<OfficeSwag.Swagging>();
		private static List<OfficeFinanceStyle.Theme> ThemeInternal = new List<OfficeFinanceStyle.Theme>();
		public static OfficeSafeDoors CurrentSafeDoors = new OfficeSafeDoors();
		public static OfficeFinanceStyle Style = new OfficeFinanceStyle();
		public static OfficeCassaForte Safe = new OfficeCassaForte();
		public static OfficeSwag Swag = new OfficeSwag();
		public static OfficeChairs Chairs = new OfficeChairs();
		public static OfficeBooze Booze = new OfficeBooze();
		public static FinanceOffice4 Instance;

		public static void LoadDefault()
		{
			Style.Set(Style.Warm);
			Chairs.Set(Chairs.On, true);
			Instance = new FinanceOffice4();
		}

	}
}
