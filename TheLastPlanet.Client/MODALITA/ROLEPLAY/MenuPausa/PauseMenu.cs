using System;
using System.Collections.Generic;
using TheLastPlanet.Client.Core.Ingresso;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.MODALITA.ROLEPLAY;


namespace TheLastPlanet.Client.RolePlay.MenuPausa
{
    internal static class PauseMenu
    {
        private static readonly List<dynamic> filtri = new()
        {
            "Nessuno",
            "Matrix",
            "Matrix 1",
            "Matrix 2",
            "Noir",
            "Noir vintage",
            "2019",
            "Bianco e Nero",
            "Bianco e Nero 1",
            "Sgranato a colori",
            "Purple Haze",
            "Kabuchiko",
            "Kabuchiko GLOOM",
            "Kabuchiko sgranato",
            "Silent Hill",
            "Silent Hill 1"
        };

        private static InputController pauseMenu = new InputController(Control.DropWeapon, ModalitaServer.Roleplay, PadCheck.Keyboard, ControlModifier.Shift, new Action<Ped, object[]>(LastPlanetMenu));

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            InputHandler.RemoveInput(pauseMenu);
        }

        private static void Spawnato(PlayerClient client)
        {
            InputHandler.AddInput(pauseMenu);
            string effect;

            switch (Main.ImpostazioniClient.Filtro)
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
                case "Bianco e Nero":
                    effect = "BikerSPLASH?";

                    break;
                case "Bianco e Nero 1":
                    effect = "MP_corona_heist_BW";

                    break;
                case "Sgranato B/N":
                    effect = "CAMERA_BW";

                    break;
                case "Sgranato a colori":
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
                case "Kabuchiko sgranato":
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
            SetTimecycleModifierStrength(Main.ImpostazioniClient.FiltroStrenght);
        }

        private static async void LastPlanetMenu(Ped playerPed, object[] args)
        {
            User pl = Cache.PlayerCache.MyPlayer.User;

            TabView MainMenu = new("The Last Galaxy", "Pianeta RP") { SideStringTop = Cache.PlayerCache.MyPlayer.Player.Name, SideStringMiddle = DateTime.Now.ToString(), SideStringBottom = "Portafoglio: $" + pl.Money + " Soldi Sporchi: $" + pl.DirtyCash, DisplayHeader = true };
            Tuple<int, string> mugshot = await Funzioni.GetPedMugshotAsync(playerPed);
            MainMenu.HeaderPicture = new(mugshot.Item2, mugshot.Item2);
            MainMenu.CrewPicture = new("thelastgalaxy", "serverlogo");

            TextTab intro = new("INTRODUZIONE", "Benvenuto su The Last Galaxy", SColor.HUD_Freemode);
            BasicTabItem a1 = new("~BLIP_INFO_ICON~ Questa pagina ti accompagnerà nella gestione delle tue ~y~impostazioni personali~w~ e come ~y~enciclopedia~w~ nel server.");
            BasicTabItem a2 = new("~BLIP_INFO_ICON~ Qui potrai trovare i comandi che il nostro server utilizza per farti giocare.");
            BasicTabItem a3 = new("~BLIP_INFO_ICON~ Potrai in ogni mento riaprire questo menu di pausa premendo i tasti ~INPUT_SPRINT~ + ~INPUT_DROP_WEAPON~ oppure con il comando /help.");
            intro.AddItem(a1);
            intro.AddItem(a2);
            intro.AddItem(a3);

            SubmenuTab online = new("ROLEPLAY", SColor.HUD_Freemode);
            TabLeftItem disc = new("Torna alla Lobby", LeftItemType.Info)
            {
                Label = "ATTENZIONE"
            };
            BasicTabItem _discinfo = new("⚠ Stai per tornare al pianeta Lobby, tutti i progressi non salvati verranno perduti!");
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


            SubmenuTab impostazioni = new("IMPOSTAZIONI", SColor.HUD_Freemode);
            TabLeftItem hud = new("HUD", LeftItemType.Settings);
            TabLeftItem Telecamere = new("Telecamere", LeftItemType.Settings);
            impostazioni.AddLeftItem(hud);
            impostazioni.AddLeftItem(Telecamere);

            #region HUD MenuItems

            #region cinema

            SettingsCheckboxItem aa = new("Modalità Cinema", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ModCinema);
            SettingsProgressItem ab = new("Spessore LetterBox", 100, (int)Main.ImpostazioniClient.LetterBox, false, SColor.HUD_Freemode);
            SettingsListItem ac = new("Filtro cinema", filtri, filtri.IndexOf(Main.ImpostazioniClient.Filtro));
            SettingsProgressItem ad = new("Intensita filtro", 100, (int)(Main.ImpostazioniClient.FiltroStrenght * 100), false, SColor.HUD_Freemode);
            aa.OnCheckboxChange += (item, attiva) => Main.ImpostazioniClient.ModCinema = attiva;
            ab.OnBarChanged += (item, index) => Main.ImpostazioniClient.LetterBox = index;
            ad.OnBarChanged += (item, index) =>
            {
                Main.ImpostazioniClient.FiltroStrenght = index / 100f;
                SetTimecycleModifierStrength(Main.ImpostazioniClient.FiltroStrenght);
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
                Main.ImpostazioniClient.Filtro = activeItem;
            };

            #endregion

            SettingsItem ae = new("", ""); // SEPARATORE

            #region Minimappa

            SettingsCheckboxItem af = new("Minimappa attiva", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.MiniMappaAttiva);
            SettingsListItem ag = new("Dimensioni Minimappa", new List<dynamic>() { "Normale", "Grande" }, Main.ImpostazioniClient.DimensioniMinimappa);
            SettingsCheckboxItem ah = new("Gps in macchina", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.MiniMappaInAuto);
            af.OnCheckboxChange += (item, check) => Main.ImpostazioniClient.MiniMappaAttiva = check;
            ag.OnListItemChanged += (item, index, label) => Main.ImpostazioniClient.DimensioniMinimappa = index;
            ah.OnCheckboxChange += (item, check) => Main.ImpostazioniClient.MiniMappaInAuto = check;

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

            SettingsCheckboxItem ba = new("Mira in soggettiva", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ForzaPrimaPersona_Mira);
            SettingsCheckboxItem bb = new("Copertura in soggettiva (sovrascrive la mira in soggettiva)", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ForzaPrimaPersona_InCopertura);
            SettingsCheckboxItem bc = new("Soggettiva nei veicoli (sovrascrive la mira in soggettiva)", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ForzaPrimaPersona_InAuto);
            ba.OnCheckboxChange += (item, check) => Main.ImpostazioniClient.ForzaPrimaPersona_Mira = check;
            bb.OnCheckboxChange += (item, check) => Main.ImpostazioniClient.ForzaPrimaPersona_InCopertura = check;
            bc.OnCheckboxChange += (item, check) => Main.ImpostazioniClient.ForzaPrimaPersona_InAuto = check;

            Telecamere.AddItem(ba);
            Telecamere.AddItem(bb);
            Telecamere.AddItem(bc);

            #endregion

            #region comandi
            SubmenuTab comandi = new("COMANDI", SColor.HUD_Freemode);

            TabLeftItem generici = new("Generici (sempre validi)", LeftItemType.Keymap);
            generici.Label = GetLabelText("MAPPING_HDR");
            generici.KeymapRightLabel_1 = "TASTIERA";
            generici.KeymapRightLabel_2 = "GAMEPAD";

            KeymapItem g1 = new("Menu Personale", "~INPUT_INTERACTION_MENU~", "", "", "~INPUT_INTERACTION_MENU~");
            KeymapItem g2 = new("Lista Giocatori e Portafoglio", "~INPUT_MULTIPLAYER_INFO~", "", "", "~INPUT_MULTIPLAYER_INFO~");
            generici.AddItem(g1);
            generici.AddItem(g2);

            TabLeftItem piedi = new("A piedi", LeftItemType.Keymap);
            piedi.Label = GetLabelText("MAPPING_HDR");
            piedi.KeymapRightLabel_1 = "TASTIERA";
            piedi.KeymapRightLabel_2 = "GAMEPAD";
            KeymapItem p1 = new("Azione", "~INPUT_CONTEXT~", "", "", "~INPUT_CONTEXT~");
            piedi.AddItem(p1);

            TabLeftItem veicolo = new("Su veicolo", LeftItemType.Keymap);
            veicolo.Label = GetLabelText("MAPPING_HDR");
            veicolo.KeymapRightLabel_1 = "TASTIERA";
            veicolo.KeymapRightLabel_2 = "GAMEPAD";
            KeymapItem v1 = new("Accendi veicolo", "~INPUT_DROP_AMMO~", "", "", "~INPUT_FRONTEND_LB~ + ~INPUT_FRONTEND_ACCEPT~");
            KeymapItem v2 = new("Allaccia / Slaccia cintura", "~INPUT_VEH_DUCK~", "", "", "~INPUT_FRONTEND_LB~ + ~INPUT_FRONTEND_X~");
            veicolo.AddItem(v1);
            veicolo.AddItem(v2);

            TabLeftItem Lavoro = new("Lavoro", LeftItemType.Keymap);
            Lavoro.Label = GetLabelText("MAPPING_HDR");
            Lavoro.KeymapRightLabel_1 = "TASTIERA";
            Lavoro.KeymapRightLabel_2 = "GAMEPAD";
            KeymapItem l1 = new("Menu lavorativo (solo alcuni lavori)", "~INPUT_SELECT_CHARACTER_FRANKLIN~", "", "", "Menu personale");
            KeymapItem l2 = new("In servizio / Fuori servizio (solo alcuni lavori)", "~INPUT_SELECT_CHARACTER_FRANKLIN~", "", "", "Menu personale");
            Lavoro.AddItem(l1);
            Lavoro.AddItem(l2);

            comandi.AddLeftItem(generici);
            comandi.AddLeftItem(piedi);
            comandi.AddLeftItem(veicolo);
            comandi.AddLeftItem(Lavoro);

            #endregion

            MainMenu.AddTab(intro);
            MainMenu.AddTab(online);
            MainMenu.AddTab(comandi);
            MainMenu.AddTab(impostazioni);
            intro.Active = true;
            intro.Focused = true;
            intro.Visible = true;

            MainMenu.OnPauseMenuClose += (menu) =>
            {
                ReleasePedheadshotImgUpload(mugshot.Item1);
                Funzioni.SalvaKvpString("SettingsClient", Main.ImpostazioniClient.ToJson());
                Client.Logger.Debug(Funzioni.CaricaKvpString("SettingsClient"));
            };

            MainMenu.Visible = true;
        }
    }
}