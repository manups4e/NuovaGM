using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_executive
{
	public class ExecApartment2
	{
		public class StyleExec
		{
			public class ExecApartTheme
			{
				public class Theme
				{
					public int InteriorId;
					public string Ipl;
					public Theme(int inter, string ipl)
					{
						InteriorId = inter;
						Ipl = ipl;
					}
				}

				public static Theme Modern = new Theme(227585, "apa_v_mp_h_01_b");
				public static Theme Moody = new Theme(228353, "apa_v_mp_h_02_b");
				public static Theme Vibrant = new Theme(229121, "apa_v_mp_h_03_b");
				public static Theme Sharp = new Theme(229889, "apa_v_mp_h_04_b");
				public static Theme Monochrome = new Theme(230657, "apa_v_mp_h_05_b");
				public static Theme Seductive = new Theme(231425, "apa_v_mp_h_06_b");
				public static Theme Regal = new Theme(232193, "apa_v_mp_h_07_b");
				public static Theme Aqua = new Theme(232961, "apa_v_mp_h_08_b");

			}

			public ExecApartTheme Theme = new ExecApartTheme();

			public void Set(ExecApartTheme.Theme style, bool refresh)
			{
				Clear();
				CurrentInteriorId = style.InteriorId;
				IplManager.EnableIpl(style.Ipl, true);
				if (refresh) API.RefreshInterior(CurrentInteriorId);
			}
			public void Clear()
			{
				IplManager.SetIplPropState(ExecApartTheme.Modern.InteriorId, new List<string>() { "Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Modern.InteriorId, new List<string>() { "Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Modern.InteriorId, new List<string>() { "Apart_Hi_Smokes_A", "Apart_Hi_Smokes_B", "Apart_Hi_Smokes_C" }, false);
				IplManager.EnableIpl(ExecApartTheme.Modern.Ipl, false);

				IplManager.SetIplPropState(ExecApartTheme.Moody.InteriorId, new List<string>() { "Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Moody.InteriorId, new List<string>() { "Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Moody.InteriorId, new List<string>() { "Apart_Hi_Smokes_A", "Apart_Hi_Smokes_B", "Apart_Hi_Smokes_C" }, false);
				IplManager.EnableIpl(ExecApartTheme.Moody.Ipl, false);

				IplManager.SetIplPropState(ExecApartTheme.Vibrant.InteriorId, new List<string>() { "Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Vibrant.InteriorId, new List<string>() { "Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Vibrant.InteriorId, new List<string>() { "Apart_Hi_Smokes_A", "Apart_Hi_Smokes_B", "Apart_Hi_Smokes_C" }, false);
				IplManager.EnableIpl(ExecApartTheme.Vibrant.Ipl, false);

				IplManager.SetIplPropState(ExecApartTheme.Sharp.InteriorId, new List<string>() { "Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Sharp.InteriorId, new List<string>() { "Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Sharp.InteriorId, new List<string>() { "Apart_Hi_Smokes_A", "Apart_Hi_Smokes_B", "Apart_Hi_Smokes_C" }, false);
				IplManager.EnableIpl(ExecApartTheme.Sharp.Ipl, false);

				IplManager.SetIplPropState(ExecApartTheme.Monochrome.InteriorId, new List<string>() { "Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Monochrome.InteriorId, new List<string>() { "Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Monochrome.InteriorId, new List<string>() { "Apart_Hi_Smokes_A", "Apart_Hi_Smokes_B", "Apart_Hi_Smokes_C" }, false);
				IplManager.EnableIpl(ExecApartTheme.Monochrome.Ipl, false);

				IplManager.SetIplPropState(ExecApartTheme.Seductive.InteriorId, new List<string>() { "Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Seductive.InteriorId, new List<string>() { "Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Seductive.InteriorId, new List<string>() { "Apart_Hi_Smokes_A", "Apart_Hi_Smokes_B", "Apart_Hi_Smokes_C" }, false);
				IplManager.EnableIpl(ExecApartTheme.Seductive.Ipl, false);

				IplManager.SetIplPropState(ExecApartTheme.Regal.InteriorId, new List<string>() { "Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Regal.InteriorId, new List<string>() { "Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Regal.InteriorId, new List<string>() { "Apart_Hi_Smokes_A", "Apart_Hi_Smokes_B", "Apart_Hi_Smokes_C" }, false);
				IplManager.EnableIpl(ExecApartTheme.Regal.Ipl, false);

				IplManager.SetIplPropState(ExecApartTheme.Aqua.InteriorId, new List<string>() { "Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Aqua.InteriorId, new List<string>() { "Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C" }, false);
				IplManager.SetIplPropState(ExecApartTheme.Aqua.InteriorId, new List<string>() { "Apart_Hi_Smokes_A", "Apart_Hi_Smokes_B", "Apart_Hi_Smokes_C" }, false);
				IplManager.EnableIpl(ExecApartTheme.Aqua.Ipl, false);
			}
		}

		public class MainStyle
		{
			public string NoneSmoke = "";
			public string A;
			public string B;
			public string C;

			public MainStyle(string a, string b, string c)
			{
				A = a;
				B = b;
				C = c;
			}

			public void Enable(string details, bool state, bool refresh = true)
			{
				IplManager.SetIplPropState(CurrentInteriorId, details, state, refresh);
			}

			public void Set(string smoke, bool refresh = true)
			{
				if (smoke != "")
				{
					if (smoke.Contains("Smoke"))
					{
						Clear(false);
						IplManager.SetIplPropState(CurrentInteriorId, smoke, true, refresh);
					}
				}
				else
				{
					if (refresh) API.RefreshInterior(CurrentInteriorId);
				}
			}
			public void Clear(bool refresh)
			{
				IplManager.SetIplPropState(CurrentInteriorId, Smoke.A, false);
				IplManager.SetIplPropState(CurrentInteriorId, Smoke.B, false);
				IplManager.SetIplPropState(CurrentInteriorId, Smoke.C, false);
			}

		}

		public static int CurrentInteriorId = -1;
		public static StyleExec Style = new StyleExec();
		public static MainStyle Strip = new MainStyle("Apart_Hi_Strip_A", "Apart_Hi_Strip_B", "Apart_Hi_Strip_C");
		public static MainStyle Booze = new MainStyle("Apart_Hi_Booze_A", "Apart_Hi_Booze_B", "Apart_Hi_Booze_C");
		public static MainStyle Smoke = new MainStyle("Apart_Hi_Smokes_A", "Apart_Hi_Smokes_B", "Apart_Hi_Smokes_C");

		public static void LoadDefault()
		{
			Style.Set(StyleExec.ExecApartTheme.Modern, true);
			Strip.Enable(Strip.A, false);
			Strip.Enable(Strip.B, false);
			Strip.Enable(Strip.C, false);
			Booze.Enable(Booze.A, false);
			Booze.Enable(Booze.B, false);
			Booze.Enable(Booze.C, false);
			Smoke.Set(Smoke.NoneSmoke);
		}
	}
}
