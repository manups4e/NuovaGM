using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;

using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client
{
	static class ClasseDiTest
	{
		public static void Init()
		{
			Client.Instance.AddTick(MenuMessaggi);
			Client.Instance.AddCommand("participants", new Action<int, List<dynamic>, string>(partecipanti));
		}

		private static async void AttivaMenu()
		{
			UIMenu Test = new UIMenu("Test", "test", new System.Drawing.PointF(700, 300));
			HUD.MenuPool.Add(Test);
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

			Test.OnItemSelect += async (menu, item, index) =>
			{
				if (item == b)
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
			};
			Test.Visible = true;
		}

		public static async Task MenuMessaggi()
		{
			if (Input.IsControlJustPressed(Control.DropWeapon, PadCheck.Any, ControlModifier.Shift))
				AttivaMenu();
		}

		private static void partecipanti(int source, List<dynamic> args, string rawcommand)
		{
			Log.Printa(LogType.Debug, "NumParticipants = " + NetworkGetNumParticipants());
			Log.Printa(LogType.Debug, "ParticipantId = " + ParticipantId());
			Log.Printa(LogType.Debug, $"sono io host? = {NetworkIsHost()} ");
			
			foreach (var player in Client.Instance.GetPlayers)
			{
				Log.Printa(LogType.Debug, $"Nome = {player.Name}, PlayerId = {player.Handle}, IsPlayerAParticipant = {NetworkIsPlayerAParticipant(IntToParticipantindex(player.Handle))}");
			}
		}
	}
}
