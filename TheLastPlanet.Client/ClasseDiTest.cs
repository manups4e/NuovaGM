using CitizenFX.Core;
using CitizenFX.Core.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Cache;
using TheLastPlanet.Client.Core.Utility.HUD;
using ScaleformUI;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client
{
	internal static class ClasseDiTest
	{
		// TODO: PROGRESSBARS IN NATIVEUI.. DA FINIRE E MIGLIORARE
		public static void Init()
		{
			AttivaMenu();
		}

		public static void Stop()
		{
		}

		private static async void AttivaMenu()
		{
			UIMenu test = new("Main Title", "I'm a subtitle", new System.Drawing.PointF(20, 20), false);
			test.AddItem(new UIMenuItem("UIMenuItem", "Description!!"));
			test.AddItem(new UIMenuListItem("UIMenuListItem", new List<dynamic>() { "this", "is", "my", "list", 12, 13453, 542545, 2452345324 }, 0, "Description!!"));
			test.AddItem(new UIMenuCheckboxItem("UIMenuCheckboxItem", UIMenuCheckboxStyle.Tick, false, "Description!!"));
			test.AddItem(new UIMenuSliderItem("UIMenuSliderItem", "Description!!", 10, 1, 0, false));
			test.AddItem(new UIMenuSliderItem("UIMenuSliderItem", "Description!!", 10, 1, 0, true));
			test.AddItem(new UIMenuProgressItem("UIMenuProgressItem", 10, 0, "Description!!"));
			test.MenuItems[0].SetRightLabel("Right Label");
			test.MenuItems[0].AddPanel(new UIMenuColorPanel("Title", ColorPanelType.Hair));
			test.MenuItems[1].AddPanel(new UIMenuPercentagePanel("Title", "0%", "100%"));
			test.MenuItems[2].AddPanel(new UIMenuGridPanel("Top", "Left", "Right", "Bottom", new System.Drawing.PointF(0.5f, 0.5f)));
			test.MenuItems[2].AddPanel(new UIMenuGridPanel("Left", "Right", new System.Drawing.PointF(0.5f, 0.5f)));
			test.MenuItems[3].AddPanel(new UIMenuStatisticsPanel());
			(test.MenuItems[3].Panels[0] as UIMenuStatisticsPanel).AddStatistics("Statistic 1", 50);
			HUD.MenuPool.Add(test);
			test.Visible = true;
		}
	}
}