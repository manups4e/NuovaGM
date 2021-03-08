using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.MenuNativo.PauseMenu;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client
{
	internal static class ClasseDiTest
	{
		public static void Init()
		{
			Client.Instance.AddTick(test);
			Client.Instance.AddEventHandler("TestEvent", new Action<byte[]>(TestEvent));
		}

		private static void TestEvent(byte[] param)
		{
			string pp = param.DeserializeBytes<string>();
			Log.Printa(LogType.Debug, pp);
		}

		private static async void AttivaMenu()
		{
			UIMenu Test = new("Test", "test", new System.Drawing.PointF(700, 300));
			HUD.MenuPool.Add(Test);
			UIMenuItem b = new("ResetTime");
			UIMenuItem c = new("SetTime1");
			UIMenuItem d = new("SetTime2");
			UIMenuItem e = new("SetTime3");
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
				else if (item == e) NetworkOverrideClockTime(12, 0, 0);
			};
			Test.Visible = true;
		}

		/*
		static TabView b = new TabView("New");
		static List<UIMenuItem> players = new List<UIMenuItem>()
				{
					new UIMenuItem(Cache.Player.Name)
				};
		static TabInteractiveListItem item2 = new TabInteractiveListItem("Item 2", GetPlayers);
		*/
		private static int timer = 0;

		public static async Task test()
		{
			if (Cache.Char != null)
			{
				//				Log.Printa(LogType.Debug, JsonConvert.SerializeObject(Cache.Char.StatiPlayer.Istanza));
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