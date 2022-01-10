using CitizenFX.Core;
using CitizenFX.Core.Native;
using ScaleformUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client
{
	internal static class ClasseDiTest
	{
		public static void Init()
		{
			Client.Instance.AddTick(TestTick);
		}

		static bool pp = false;
		static Marker dummyMarker = new(MarkerType.VerticalCylinder, WorldProbe.CrossairRaycastResult.HitPosition.ToPosition(), Colors.Blue);
		private static async Task TestTick()
        {
			//Client.Logger.Debug(IplManager.Global.ToJson());
			if (Input.IsControlJustPressed(Control.Detonate, PadCheck.Keyboard, ControlModifier.Shift) && !HUD.MenuPool.IsAnyMenuOpen)
			{
				TestMenu();
				//AttivaMenu();
			}
		}

		public static void Stop()
		{
			Client.Instance.RemoveTick(TestTick);
		}

		static UIMenu menu = new("Menu Test", "Test");

		private static void TestMenu()
        {
			menu = new("Menu Test", "Test", new PointF(50, 50));
			HUD.MenuPool.Add(menu);
			UIMenuItem item = new UIMenuItem("Item 1");
			UIMenuListItem item1 = new UIMenuListItem("Item 2", new() { " "}, 0);
			UIMenuCheckboxItem item2 = new UIMenuCheckboxItem("Item 3", false);
			UIMenuProgressItem item3 = new UIMenuProgressItem("Item 4", 100, 0);
			UIMenuSliderItem item4 = new UIMenuSliderItem("Item 5");
			menu.AddItem(item);
			menu.AddItem(item1);
			menu.AddItem(item2);
			menu.AddItem(item3);
			menu.AddItem(item4);
			menu.OnMenuStateChanged += (old, _new, type) =>
			{
				if (type == MenuState.Opened)
					Client.Instance.AddTick(testtick);
				else if (type == MenuState.Closed)
					Client.Instance.RemoveTick(testtick);
			};

			menu.Visible = true;
		}

		private static async Task testtick()
        {
			foreach(var item in menu.MenuItems)
            {
				if (string.IsNullOrWhiteSpace(item.Label) || item.Label.StartsWith("Item"))
                {
					item.Label = "L";
					item.Description = "L";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "L")
                {
					item.Label = "LO";
					item.Description = "LO";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LO")
				{
					item.Label = "LOC";
					item.Description = "LOC";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOC")
				{
					item.Label = "LOCA";
					item.Description = "LOCA";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCA")
				{
					item.Label = "LOCAL";
					item.Description = "LOCAL";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCAL")
				{
					item.Label = "LOCALH";
					item.Description = "LOCALH";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALH")
				{
					item.Label = "LOCALHO";
					item.Description = "LOCALHO";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHO")
				{
					item.Label = "LOCALHOS";
					item.Description = "LOCALHOS";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOS")
				{
					item.Label = "LOCALHOST";
					item.Description = "LOCALHOST";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST")
				{
					item.Label = "LOCALHOST ";
					item.Description = "LOCALHOST ";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST ")
				{
					item.Label = "LOCALHOST I";
					item.Description = "LOCALHOST I";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST I")
				{
					item.Label = "LOCALHOST IS";
					item.Description = "LOCALHOST IS";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS")
				{
					item.Label = "LOCALHOST IS ";
					item.Description = "LOCALHOST IS ";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS ")
				{
					item.Label = "LOCALHOST IS M";
					item.Description = "LOCALHOST IS M";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS M")
				{
					item.Label = "LOCALHOST IS MY";
					item.Description = "LOCALHOST IS MY";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY")
				{
					item.Label = "LOCALHOST IS MY ";
					item.Description = "LOCALHOST IS MY ";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY ")
				{
					item.Label = "LOCALHOST IS MY F";
					item.Description = "LOCALHOST IS MY F";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY F")
				{
					item.Label = "LOCALHOST IS MY FR";
					item.Description = "LOCALHOST IS MY FR";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY FR")
				{
					item.Label = "LOCALHOST IS MY FRI";
					item.Description = "LOCALHOST IS MY FRI";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY FRI")
				{
					item.Label = "LOCALHOST IS MY FRIE";
					item.Description = "LOCALHOST IS MY FRIE";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY FRIE")
				{
					item.Label = "LOCALHOST IS MY FRIEN";
					item.Description = "LOCALHOST IS MY FRIEN";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY FRIEN")
				{
					item.Label = "LOCALHOST IS MY FRIEND";
					item.Description = "LOCALHOST IS MY FRIEND";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY FRIEND")
				{
					item.Label = "LOCALHOST IS MY FRIEND.";
					item.Description = "LOCALHOST IS MY FRIEND.";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY FRIEND.")
				{
					item.Label = "LOCALHOST IS MY FRIEND..";
					item.Description = "LOCALHOST IS MY FRIEND..";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY FRIEND..")
				{
					item.Label = "LOCALHOST IS MY FRIEND...";
					item.Description = "LOCALHOST IS MY FRIEND...";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				else if (item.Label == "LOCALHOST IS MY FRIEND...")
				{
					item.Label = "L";
					item.Description = "L";
					item.MainColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9));
					item.HighlightColor = (HudColor)Enum.GetValues(typeof(HudColor)).GetValue(item.Label.Length + Funzioni.GetRandomInt(5, 9)) + 10;
				}
				await BaseScript.Delay(50);

				/*
				switch (item)
                {
                    case UIMenuSliderItem:
                        {
							var it = (UIMenuSliderItem)item;
                        }
                        break;
                    case UIMenuProgressItem:
                        {

                        }
                        break;
                    case UIMenuListItem:
						{

						}
						break;
                    case UIMenuCheckboxItem:
						{

						}
						break;
					default :
						{

						}
						break;
				}
				*/
			}
        }

		private static void AttivaMenu()
		{
			JobSelectionData data = new();
			data.SetTitle("TEST");
			data.SetVotes(0, 3, "TEST");
			data.Cards = new List<JobSelectionCard>();
			for (int i=0; i<6; i++)
            {
				var card = new JobSelectionCard("Test", "test", "", "", 12, 15, JobSelectionCardIcon.CAPTURE_THE_FLAG, HudColor.HUD_COLOUR_FREEMODE, 2, new List<JobSelectionCardDetail>()
				 {
					 new JobSelectionCardDetail(JobSelectionCardDetailType.WITH_ICON, "Test Left", "Test Right"),
					 new JobSelectionCardDetail(JobSelectionCardDetailType.WITH_ICON, "Test Left", "Test Right"),
					 new JobSelectionCardDetail(JobSelectionCardDetailType.WITH_ICON, "Test Left", "Test Right")
				 });

				data.AddCard(card);
            }
			data.Buttons = new List<JobSelectionButton>()
			{
				new JobSelectionButton("Test1", null),
				new JobSelectionButton("Test2", null),
				new JobSelectionButton("Test3", null),
			};
            ScaleformUI.ScaleformUI.JobMissionSelection.JobData = data;
            ScaleformUI.ScaleformUI.JobMissionSelection.Enabled = true;
		}
	}
}