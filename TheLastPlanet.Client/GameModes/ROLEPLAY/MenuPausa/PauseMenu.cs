using System;
using System.Collections.Generic;
using TheLastPlanet.Client.Core.Ingresso;
using TheLastPlanet.Client.GameMode.ROLEPLAY;
using TheLastPlanet.Client.Handlers;


namespace TheLastPlanet.Client.RolePlay.MenuPausa
{
    internal static class PauseMenu
    {
        private static readonly List<dynamic> filtri = new()
        {
            "None",
            "Matrix",
            "Matrix 1",
            "Matrix 2",
            "Noir",
            "Noir vintage",
            "2019",
            "Black and White",
            "Black and White 1",
            "Grainy colored",
            "Purple Haze",
            "Kabuchiko",
            "Kabuchiko GLOOM",
            "Kabuchiko grainy",
            "Silent Hill",
            "Silent Hill 1"
        };

        private static InputController pauseMenu = new InputController(Control.DropWeapon, ServerMode.Roleplay, PadCheck.Keyboard, ControlModifier.Shift, new Action<Ped, object[]>(LastPlanetMenu));

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            InputHandler.RemoveInput(pauseMenu);
        }

        private static void Spawned(PlayerClient client)
        {
            InputHandler.AddInput(pauseMenu);
            string effect;

            switch (Main.ClientConfig.Filter)
            {
                case "Matrix":
                    effect = "AirRaceBoost02";

                    break;
                case "Matrix 1":
                    effect = "FranklinColorCodeBright";

                    break;
                case "Matrix 2":
                    effect = "FranklinColorCodeBasic";

                    break;
                case "Noir":
                    effect = "NG_filmnoir_BW01";

                    break;
                case "Noir vintage":
                    effect = "NG_filmnoir_BW01";

                    break;
                case "2019":
                    effect = "BikerFormFlash";

                    break;
                case "Black and White":
                    effect = "BikerSPLASH?";

                    break;
                case "Black and White 1":
                    effect = "MP_corona_heist_BW";

                    break;
                case "Grainy B/W":
                    effect = "CAMERA_BW";

                    break;
                case "Grainy colored":
                    effect = "Dont_tazeme_bro";

                    break;
                case "Purple Haze":
                    effect = "NG_filmic08";

                    break;
                case "Kabuchiko":
                    effect = "NG_filmic10";

                    break;
                case "Kabuchiko GLOOM":
                    effect = "NG_filmic23";

                    break;
                case "Kabuchiko grainy":
                    effect = "drug_flying_base";

                    break;
                case "Silent Hill":
                    effect = "NG_filmic25";

                    break;
                case "Silent Hill 1":
                    effect = "michealspliff";

                    break;
                default:
                    effect = "None";

                    break;
            }

            if (effect != "None")
                SetTransitionTimecycleModifier(effect, 1f);
            else
                SetTimecycleModifier(effect);
            SetTimecycleModifierStrength(Main.ClientConfig.FilterStrenght);
        }

        private static async void LastPlanetMenu(Ped playerPed, object[] args)
        {
            User pl = Cache.PlayerCache.MyPlayer.User;

            TabView MainMenu = new("The Last Galaxy", "RolePlay") { SideStringTop = Cache.PlayerCache.MyPlayer.Player.Name, SideStringMiddle = DateTime.Now.ToString(), SideStringBottom = "Portafoglio: $" + pl.Money + " Soldi Sporchi: $" + pl.DirtyCash, DisplayHeader = true };
            Tuple<int, string> mugshot = await Functions.GetPedMugshotAsync(playerPed);
            MainMenu.HeaderPicture = new(mugshot.Item2, mugshot.Item2);
            MainMenu.CrewPicture = new("thelastgalaxy", "serverlogo");

            TextTab intro = new("Introduction", "Welcome to The Last Galaxy", SColor.HUD_Freemode);
            BasicTabItem a1 = new("~BLIP_INFO_ICON~ This page will help you manage your ~y~personal settings~w~ and as ~y~encyclopedia~w~ on the server.");
            BasicTabItem a2 = new("~BLIP_INFO_ICON~ Here you can find the commands that our server uses to let you play.");
            BasicTabItem a3 = new("~BLIP_INFO_ICON~ You can reopen this pause menu at any time by pressing the ~INPUT_SPRINT~ + ~INPUT_DROP_WEAPON~ keys or with the command /help.");
            intro.AddItem(a1);
            intro.AddItem(a2);
            intro.AddItem(a3);

            SubmenuTab online = new("ROLEPLAY", SColor.HUD_Freemode);
            TabLeftItem disc = new("Go back to Lobby", LeftItemType.Info)
            {
                Label = "WARNING"
            };
            BasicTabItem _discinfo = new("⚠ You're going to return to Lobby, all unsaved progress will be lost!");
            disc.AddItem(_discinfo);
            online.AddLeftItem(disc);
            disc.OnActivated += async (item, index) =>
            {
                Screen.Fading.FadeOut(1000);
                await BaseScript.Delay(1500);
                MainMenu.Visible = false;
                await Initializer.Stop();
                ServerJoining.ReturnToLobby();
            };


            SubmenuTab settings = new("SETTINGS", SColor.HUD_Freemode);
            TabLeftItem hud = new("HUD", LeftItemType.Settings);
            TabLeftItem cameras = new("Cameras", LeftItemType.Settings);
            settings.AddLeftItem(hud);
            settings.AddLeftItem(cameras);

            #region HUD MenuItems

            #region cinema

            SettingsCheckboxItem aa = new("Modalità Cinema", UIMenuCheckboxStyle.Tick, Main.ClientConfig.CinemaMode);
            SettingsProgressItem ab = new("Spessore LetterBox", 100, (int)Main.ClientConfig.LetterBox, false, SColor.HUD_Freemode);
            SettingsListItem ac = new("Filtro cinema", filtri, filtri.IndexOf(Main.ClientConfig.Filter));
            SettingsProgressItem ad = new("Intensita filtro", 100, (int)(Main.ClientConfig.FilterStrenght * 100), false, SColor.HUD_Freemode);
            aa.OnCheckboxChange += (item, attiva) => Main.ClientConfig.CinemaMode = attiva;
            ab.OnBarChanged += (item, index) => Main.ClientConfig.LetterBox = index;
            ad.OnBarChanged += (item, index) =>
            {
                Main.ClientConfig.FilterStrenght = index / 100f;
                SetTimecycleModifierStrength(Main.ClientConfig.FilterStrenght);
            };
            ac.OnListItemChanged += (item, index, label) =>
            {
                string activeItem = label;
                string effect = activeItem switch
                {
                    "Matrix" => "AirRaceBoost02",
                    "Matrix 1" => "FranklinColorCodeBright",
                    "Matrix 2" => "FranklinColorCodeBasic",
                    "Noir" => "NG_filmnoir_BW01",
                    "Noir vintage" => "NG_filmnoir_BW01",
                    "2019" => "BikerFormFlash",
                    "Bianco e Nero" => "BikerSPLASH?",
                    "Bianco e Nero 1" => "MP_corona_heist_BW",
                    "Sgranato B/N" => "CAMERA_BW",
                    "Sgranato a colori" => "Dont_tazeme_bro",
                    "Purple Haze" => "NG_filmic08",
                    "Kabuchiko" => "NG_filmic10",
                    "Kabuchiko GLOOM" => "NG_filmic23",
                    "Kabuchiko sgranato" => "drug_flying_base",
                    "Silent Hill" => "NG_filmic25",
                    "Silent Hill 1" => "michealspliff",
                    _ => "None",
                };
                if (effect != "None")
                    SetTransitionTimecycleModifier(effect, 1f);
                else
                    SetTimecycleModifier(effect);
                Main.ClientConfig.Filter = activeItem;
            };

            #endregion

            SettingsItem ae = new("", ""); // SEPARATORE

            #region Minimappa

            SettingsCheckboxItem af = new("Minimappa attiva", UIMenuCheckboxStyle.Tick, Main.ClientConfig.EnableMinimap);
            SettingsListItem ag = new("Dimensioni Minimappa", new List<dynamic>() { "Normale", "Grande" }, Main.ClientConfig.MinimapSize);
            SettingsCheckboxItem ah = new("Gps in macchina", UIMenuCheckboxStyle.Tick, Main.ClientConfig.InCarMinimap);
            af.OnCheckboxChange += (item, check) => Main.ClientConfig.EnableMinimap = check;
            ag.OnListItemChanged += (item, index, label) => Main.ClientConfig.MinimapSize = index;
            ah.OnCheckboxChange += (item, check) => Main.ClientConfig.InCarMinimap = check;

            #endregion

            hud.AddItem(aa);
            hud.AddItem(ab);
            hud.AddItem(ac);
            hud.AddItem(ad);
            hud.AddItem(ae);
            hud.AddItem(af);
            hud.AddItem(ag);
            hud.AddItem(ah);

            #endregion

            #region Telecamere

            SettingsCheckboxItem ba = new("Mira in soggettiva", UIMenuCheckboxStyle.Tick, Main.ClientConfig.ForceFirstPersonAiming);
            SettingsCheckboxItem bb = new("Copertura in soggettiva (sovrascrive la mira in soggettiva)", UIMenuCheckboxStyle.Tick, Main.ClientConfig.ForceFirstPersonCover);
            SettingsCheckboxItem bc = new("Soggettiva nei veicoli (sovrascrive la mira in soggettiva)", UIMenuCheckboxStyle.Tick, Main.ClientConfig.ForceFirstPersonInCar);
            ba.OnCheckboxChange += (item, check) => Main.ClientConfig.ForceFirstPersonAiming = check;
            bb.OnCheckboxChange += (item, check) => Main.ClientConfig.ForceFirstPersonCover = check;
            bc.OnCheckboxChange += (item, check) => Main.ClientConfig.ForceFirstPersonInCar = check;

            cameras.AddItem(ba);
            cameras.AddItem(bb);
            cameras.AddItem(bc);

            #endregion

            #region commands
            SubmenuTab commands = new("COMMANDS", SColor.HUD_Freemode);

            TabLeftItem generic = new("Generic (always valid)", LeftItemType.Keymap);
            generic.Label = GetLabelText("MAPPING_HDR");
            generic.KeymapRightLabel_1 = "KEYBOARD";
            generic.KeymapRightLabel_2 = "GAMEPAD";

            KeymapItem g1 = new("Main Menu", "~INPUT_INTERACTION_MENU~", "", "", "~INPUT_INTERACTION_MENU~");
            KeymapItem g2 = new("Player List and Wallet", "~INPUT_MULTIPLAYER_INFO~", "", "", "~INPUT_MULTIPLAYER_INFO~");
            generic.AddItem(g1);
            generic.AddItem(g2);

            TabLeftItem foot = new("On Foot", LeftItemType.Keymap);
            foot.Label = GetLabelText("MAPPING_HDR");
            foot.KeymapRightLabel_1 = "KEYBOARD";
            foot.KeymapRightLabel_2 = "GAMEPAD";
            KeymapItem p1 = new("Action", "~INPUT_CONTEXT~", "", "", "~INPUT_CONTEXT~");
            foot.AddItem(p1);

            TabLeftItem vehicle = new("On Vehicle", LeftItemType.Keymap);
            vehicle.Label = GetLabelText("MAPPING_HDR");
            vehicle.KeymapRightLabel_1 = "KEYBOARD";
            vehicle.KeymapRightLabel_2 = "GAMEPAD";
            KeymapItem v1 = new("Turn on Vehicles", "~INPUT_DROP_AMMO~", "", "", "~INPUT_FRONTEND_LB~ + ~INPUT_FRONTEND_ACCEPT~");
            KeymapItem v2 = new("Fasten / undo seatbelt", "~INPUT_VEH_DUCK~", "", "", "~INPUT_FRONTEND_LB~ + ~INPUT_FRONTEND_X~");
            vehicle.AddItem(v1);
            vehicle.AddItem(v2);

            TabLeftItem job = new("Job", LeftItemType.Keymap);
            job.Label = GetLabelText("MAPPING_HDR");
            job.KeymapRightLabel_1 = "KEYBOARD";
            job.KeymapRightLabel_2 = "GAMEPAD";
            KeymapItem l1 = new("Job menu (only certain jobs)", "~INPUT_SELECT_CHARACTER_FRANKLIN~", "", "", "Personal Menu");
            KeymapItem l2 = new("On / Off duty (only certain jobs)", "~INPUT_SELECT_CHARACTER_FRANKLIN~", "", "", "Personal Menu");
            job.AddItem(l1);
            job.AddItem(l2);

            commands.AddLeftItem(generic);
            commands.AddLeftItem(foot);
            commands.AddLeftItem(vehicle);
            commands.AddLeftItem(job);

            #endregion

            MainMenu.AddTab(intro);
            MainMenu.AddTab(online);
            MainMenu.AddTab(commands);
            MainMenu.AddTab(settings);
            intro.Active = true;
            intro.Focused = true;
            intro.Visible = true;

            MainMenu.OnPauseMenuClose += (menu) =>
            {
                ReleasePedheadshotImgUpload(mugshot.Item1);
                Functions.SaveKvpString("SettingsClient", Main.ClientConfig.ToJson());
                Client.Logger.Debug(Functions.LoadKvpString("SettingsClient"));
            };

            MainMenu.Visible = true;
        }
    }
}