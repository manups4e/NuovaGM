﻿using System;
using TheLastPlanet.Client.Core.Ingresso;
using TheLastPlanet.Client.Handlers;


namespace TheLastPlanet.Client.MODALITA.FREEROAM.Scripts.PauseMenu
{
    static class MenuPausa
    {
        private static readonly InputController pauseMenu = new(Control.DropWeapon, ServerMode.FreeRoam, PadCheck.Keyboard, ControlModifier.Shift, new Action<Ped, object[]>(FreeRoamMenu));

        public static void Init()
        {
            AccessingEvents.OnFreeRoamSpawn += (client) => InputHandler.AddInput(pauseMenu);
            AccessingEvents.OnFreeRoamLeave += (client) => InputHandler.RemoveInput(pauseMenu);
        }

        public static async void FreeRoamMenu(Ped me, object[] _unused)
        {
            if (MenuHandler.IsAnyPauseMenuOpen) return;
            PlayerClient client = Cache.PlayerCache.MyPlayer;

            int day = GetClockDayOfWeek();
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
            Tuple<int, string> mugshot = await Funzioni.GetPedMugshotAsync(me);
            MainMenu.HeaderPicture = new(mugshot.Item2, mugshot.Item2);
            MainMenu.CrewPicture = new("thelastgalaxy", "serverlogo");

            SubmenuTab online = new("FREEROAM", SColor.HUD_Freemode);
            TabLeftItem disc = new("disconnettiti", LeftItemType.Info)
            {
                Label = "ATTENZIONE"
            };
            BasicTabItem _discinfo = new("⚠ Stai per tornare al pianeta Lobby, tutti i progressi non salvati verranno perduti!");
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
