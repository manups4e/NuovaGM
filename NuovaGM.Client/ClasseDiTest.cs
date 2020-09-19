﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Logger;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.NuovoIngresso;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.MenuNativo.PauseMenu;
using NuovaGM.Shared;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client
{
	static class ClasseDiTest
	{
		public static void Init()
		{
			Client.Instance.AddTick(TabsPauseMenu);
		}

		private static async void AttivaMenu()
		{
			UIMenu Test = new UIMenu("Test", "test", new System.Drawing.PointF(700, 300));
			HUD.MenuPool.Add(Test);
			UIMenuItem m = new UIMenuItem("dettagli veicolo");
			Test.AddItem(m);
/*
			UIMenuItem b = new UIMenuItem("ShowColoredShard");
			UIMenuItem c = new UIMenuItem("ShowOldMessage");
			UIMenuItem d = new UIMenuItem("ShowSimpleShard");
			UIMenuItem e = new UIMenuItem("ShowRankupMessage");
			UIMenuItem f = new UIMenuItem("ShowWeaponPurchasedMessage");
			UIMenuItem g = new UIMenuItem("ShowMpMessageLarge");
			UIMenuItem h = new UIMenuItem("ShowMpWastedMessage");
			Test.AddItem(b);
			Test.AddItem(c);
			Test.AddItem(d);
			Test.AddItem(e);
			Test.AddItem(f);
			Test.AddItem(g);
			Test.AddItem(h);
*/

			Test.OnItemSelect += async (menu, item, index) =>
			{
				if(item == m)
				{
					var p = await Game.PlayerPed.CurrentVehicle.GetVehicleProperties();
					Log.Printa(LogType.Debug, p.Serialize());
					await BaseScript.Delay(1000);
					Game.PlayerPed.CurrentVehicle.Mods.InstallModKit();
					await BaseScript.Delay(1000);
					Game.PlayerPed.CurrentVehicle.Mods[VehicleModType.Spoilers].Index = 1;
					int i = (int)Game.PlayerPed.CurrentVehicle.Mods[VehicleModType.Spoilers].ModType;
					var pippo = await Game.PlayerPed.CurrentVehicle.GetVehicleProperties();
					Log.Printa(LogType.Debug, pippo.Serialize());
				}
				/*				if (item == b)
									BigMessageThread.MessageInstance.ShowColoredShard("Test1", "Test2", HudColor.HUD_COLOUR_BLUELIGHT, HudColor.HUD_COLOUR_MENU_YELLOW);
								else if (item == c)
									BigMessageThread.MessageInstance.ShowOldMessage("Test1");
								else if (item == d)
									BigMessageThread.MessageInstance.ShowSimpleShard("Test1", "Test2");
								else if (item == e)
									BigMessageThread.MessageInstance.ShowRankupMessage("Test1", "Test2", 15);
								else if (item == f)
									BigMessageThread.MessageInstance.ShowWeaponPurchasedMessage("Test1", "WEAPON_PISTOL", WeaponHash.Pistol);
								else if (item == g)
									BigMessageThread.MessageInstance.ShowMpMessageLarge("~g~MISSIONE COMPIUTA", "Veicolo recuperato!");
								else if (item == h)
									BigMessageThread.MessageInstance.ShowMpWastedMessage("Test 1", "Test 2");
				*/

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
		public static async Task TabsPauseMenu()
		{
			/*
			b.ProcessControls();
			b.Update();
			item2.ProcessControls();
			*/
			if (Input.IsControlJustPressed(Control.DropWeapon, PadCheck.Any, ControlModifier.Shift))
			{
				AttivaMenu();
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
