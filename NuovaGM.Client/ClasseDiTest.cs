using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Ingresso;
using TheLastPlanet.Client.Core.Personaggio;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.MenuNativo.PauseMenu;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client
{
	static class ClasseDiTest
	{
		public static async void Init()
		{
			Client.Instance.AddTick(test);
		}

		private static async void AttivaMenu()
		{
			UIMenu Test = new UIMenu("Test", "test", new System.Drawing.PointF(700, 300));
			HUD.MenuPool.Add(Test);

			UIMenuItem b = new UIMenuItem("ResetTime");
			UIMenuItem c = new UIMenuItem("SetTime1");
			UIMenuItem d = new UIMenuItem("SetTime2");
			UIMenuItem e = new UIMenuItem("SetTime3");
			Test.AddItem(b);
			Test.AddItem(c);
			Test.AddItem(d);
			Test.AddItem(e);

			Test.OnItemSelect += async (menu, item, index) =>
			{
				if (item == b)
					NetworkClearClockTimeOverride();
				else if (item == c)
					AdvanceClockTimeTo(23, 0, 0);
				else if (item == d)
					SetClockTime(15, 0, 0);
				else if (item == e)
					NetworkOverrideClockTime(12, 0, 0);

			};
			Test.Visible = true;
		}

		/*
		static TabView b = new TabView("New");
		static List<UIMenuItem> Players = new List<UIMenuItem>()
				{
					new UIMenuItem(Game.Player.Name)
				};
		static TabInteractiveListItem item2 = new TabInteractiveListItem("Item 2", Players);
		*/
		public static async Task test()
		{
			if (Game.Player.GetPlayerData() != null)
			{
				HUD.DrawText(0.35f, 0.7f, JsonConvert.SerializeObject(Game.Player.State["Istanza"]));
				HUD.DrawText(0.35f, 0.725f, JsonConvert.SerializeObject(Game.Player.State["Pausa"]));
			}
			/*
			b.ProcessControls();
			b.Update();
			item2.ProcessControls();
			*/
			if (Input.IsControlJustPressed(Control.DropWeapon, PadCheck.Any, ControlModifier.Shift))
			{
				/*
				b.Tabs.Clear();
				TabItem item1 = new TabItem("Item 1");
				List<MissionInformation> missions = new List<MissionInformation>()
				{
					new MissionInformation("Mission Info", new List<Tuple<string, string>>()
					{
						new Tuple<string, string>("Mission title", "Mission subtitle")
					})
				};
				TabMissionSelectItem item3 = new TabMissionSelectItem("Mission control to Major Tom", missions);
				b.AddTab(item1);
				b.AddTab(item2);
				b.AddTab(item3);
				b.Visible = true;
				*/
			}
		}
	}
}
