using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.SistemaEventi;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client
{
	internal static class ClasseDiTest
	{
		public static void Init()
		{
			ClientSession.Instance.AddTick(test);
			ClientSession.Instance.AddEventHandler("test", new Action<byte[]>(Prova));
		}

		private static void Prova(byte[] param)
		{
			try
			{
				
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e.ToString());
				Log.Printa(LogType.Error, e.StackTrace);
				
			}
		}
		private static void AttivaMenu()
		{
			UIMenu test = new("Test", "test", new System.Drawing.PointF(700, 300));
			HUD.MenuPool.Add(test);
			UIMenuItem b = new("ResetTime");
			UIMenuItem c = new("SetTime1");
			UIMenuItem d = new("SetTime2");
			UIMenuItem e = new("SetTime3");
			test.AddItem(b);
			test.AddItem(c);
			test.AddItem(d);
			test.AddItem(e);
			test.OnItemSelect += async (menu, item, index) =>
			{
				if (item == b)
					NetworkClearClockTimeOverride();
				else if (item == c)
					AdvanceClockTimeTo(23, 0, 0);
				else if (item == d)
					SetClockTime(15, 0, 0);
				else if (item == e) NetworkOverrideClockTime(12, 0, 0);
			};
			test.Visible = true;
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
			await SessionCache.Cache.Loaded();
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
				User Test = await ClientSession.Instance.SistemaEventi.Request<User>("chiamaTest", null);
				Log.Printa(LogType.Debug, Test.ToJson());
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