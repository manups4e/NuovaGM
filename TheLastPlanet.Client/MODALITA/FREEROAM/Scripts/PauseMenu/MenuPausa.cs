using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using ScaleformUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Core.Ingresso;
using CitizenFX.Core.UI;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Scripts.PauseMenu
{
    static class MenuPausa
    {
        private static readonly InputController pauseMenu = new(Control.DropWeapon, ModalitaServer.FreeRoam, PadCheck.Keyboard, ControlModifier.Shift, new Action<Ped, object[]>(FreeRoamMenu));

		public static void Init()
		{
			InputHandler.AddInput(pauseMenu);
		}

		public static void Stop()
		{
			InputHandler.RemoveInput(pauseMenu);
		}

		public static async void FreeRoamMenu(Ped me, object[] _unused)
        {
			if (HUD.MenuPool.IsAnyPauseMenuOpen) return;
			ClientId client = Cache.PlayerCache.MyPlayer;

			var day = GetClockDayOfWeek();
			string giorno = "Lunedì";
            switch (day)
            {
				case 0:
					giorno = "Domenica";
					break;
				case 1:
					giorno = "Lunedì";
					break;
				case 2:
					giorno = "Martedì";
					break;
				case 3:
					giorno = "Mercoledì";
					break;
				case 4:
					giorno = "Giovedì";
					break;
				case 5:
					giorno = "Venerdì";
					break;
				case 6:
					giorno = "Sabato";
					break;
			}

			TabView MainMenu = new("The Last Galaxy", "Pianeta FreeRoam")
			{
				SideStringTop = Cache.PlayerCache.MyPlayer.Player.Name,
				SideStringMiddle = $"{giorno.ToUpper()} {GetClockHours()}:{GetClockMinutes()}",
				SideStringBottom = "SOLDI: $" + client.User.Money + " BANCA: $" + client.User.Bank,
				DisplayHeader = true
			};
			var mugshot = await Funzioni.GetPedMugshotAsync(me);
			MainMenu.HeaderPicture = new(mugshot.Item2, mugshot.Item2);

			HUD.MenuPool.Add(MainMenu);
			TabSubmenuItem online = new("FREEROAM");
            TabLeftItem disc = new("disconnettiti", LeftItemType.Info)
            {
                TextTitle = "ATTENZIONE"
            };
            BasicTabItem _discinfo = new("Stai per tornare al pianeta Lobby, tutti i progressi non salvati verranno perduti!");
			disc.AddItem(_discinfo);
			online.AddLeftItem(disc);

			MainMenu.AddTab(online);

			disc.OnActivated += async (item, index) =>
			{
				Screen.Fading.FadeOut(1000);
				await BaseScript.Delay(1500);
				MainMenu.Visible = false;
				await Initializer.Stop();
				ServerJoining.ReturnToLobby();
				
			};

			MainMenu.Visible = true;
		}
	}
}
