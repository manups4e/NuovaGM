
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Handlers;
using ScaleformUI;
using ScaleformUI.PauseMenu;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Personale;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.ROLEPLAY;
using TheLastPlanet.Client.Core.Ingresso;

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
			InputHandler.AddInput(pauseMenu);
			Client.Instance.AddEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));
		}

		public static void Stop()
		{
			InputHandler.RemoveInput(pauseMenu);
			Client.Instance.RemoveEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));
		}

		private static async void Spawnato()
		{
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
			var mugshot = await Funzioni.GetPedMugshotAsync(playerPed);
			MainMenu.HeaderPicture = new(mugshot.Item2, mugshot.Item2);

			HUD.MenuPool.Add(MainMenu);

			TabTextItem intro = new("INTRODUZIONE", "Benvenuto su The Last Galaxy");
			BasicTabItem a1 = new("Questa pagina ti accompagnerà nella gestione delle tue ~y~impostazioni personali~w~ e come ~y~enciclopedia~w~ nel server.");
			BasicTabItem a2 = new("Qui potrai trovare i comandi che il nostro server utilizza per farti giocare.");
			BasicTabItem a3 = new("Potrai in ogni mento riaprire questo menu di pausa premendo i tasti SHIFT+F9 oppure con il comando /help.");
			intro.AddItem(a1);
			intro.AddItem(a2);
			intro.AddItem(a3);

			TabSubmenuItem online = new("FREEROAM");
			TabLeftItem disc = new("Torna alla Lobby", LeftItemType.Info)
			{
				TextTitle = "ATTENZIONE"
			};
			BasicTabItem _discinfo = new("Stai per tornare al pianeta Lobby, tutti i progressi non salvati verranno perduti!");
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


			TabSubmenuItem impostazioni = new("IMPOSTAZIONI");
			TabLeftItem hud = new("HUD", LeftItemType.Settings);
			TabLeftItem Telecamere = new("Telecamere", LeftItemType.Settings);
			impostazioni.AddLeftItem(hud);
			impostazioni.AddLeftItem(Telecamere);

			#region HUD MenuItems

			#region cinema

			SettingsTabItem aa = new("Modalità Cinema", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ModCinema);
			SettingsTabItem ab = new("Spessore LetterBox", 100, (int)Main.ImpostazioniClient.LetterBox, false);
			SettingsTabItem ac = new("Filtro cinema", filtri, filtri.IndexOf(Main.ImpostazioniClient.Filtro));
			SettingsTabItem ad = new("Intensita filtro", 100, (int)(Main.ImpostazioniClient.FiltroStrenght * 100), false);
			aa.OnCheckboxChange += (item, attiva) => Main.ImpostazioniClient.ModCinema = attiva;
			ab.OnBarChanged += (item, index) => Main.ImpostazioniClient.LetterBox = index;
			ad.OnBarChanged += (item, index) =>
			{
				Main.ImpostazioniClient.FiltroStrenght = index / 100f;
				SetTimecycleModifierStrength(Main.ImpostazioniClient.FiltroStrenght);
			};
			ac.OnListItemChange += (item, index, label) =>
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

			SettingsTabItem ae = new("", ""); // SEPARATORE

			#region Minimappa

			SettingsTabItem af = new("Minimappa attiva", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.MiniMappaAttiva);
			SettingsTabItem ag = new("Dimensioni Minimappa", new List<dynamic>() { "Normale", "Grande" }, Main.ImpostazioniClient.DimensioniMinimappa);
			SettingsTabItem ah = new("Gps in macchina", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.MiniMappaInAuto);
			af.OnCheckboxChange += (item, check) => Main.ImpostazioniClient.MiniMappaAttiva = check;
			ag.OnListItemChange += (item, index, label) => Main.ImpostazioniClient.DimensioniMinimappa = index;
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

			SettingsTabItem ba = new("Mira in soggettiva", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ForzaPrimaPersona_Mira);
			SettingsTabItem bb = new("Copertura in soggettiva (sovrascrive la mira in soggettiva)", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ForzaPrimaPersona_InCopertura);
			SettingsTabItem bc = new("Soggettiva nei veicoli (sovrascrive la mira in soggettiva)", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ForzaPrimaPersona_InAuto);
			ba.OnCheckboxChange += (item, check) => Main.ImpostazioniClient.ForzaPrimaPersona_Mira = check;
			bb.OnCheckboxChange += (item, check) => Main.ImpostazioniClient.ForzaPrimaPersona_InCopertura = check;
			bc.OnCheckboxChange += (item, check) => Main.ImpostazioniClient.ForzaPrimaPersona_InAuto = check;

			Telecamere.AddItem(ba);
			Telecamere.AddItem(bb);
			Telecamere.AddItem(bc);

			#endregion

			#region comandi
			TabSubmenuItem comandi = new("COMANDI");

			TabLeftItem generici = new("Generici (sempre validi)", LeftItemType.Statistics);
			StatsTabItem g1 = new("Menu Personale", "Tasto M / Select");
			StatsTabItem g2 = new("Lista Giocatori", "Tasto Z / Dpad giù");
			StatsTabItem g3 = new("Portafoglio", "Tasto Z / Dpad giù");
			generici.AddItem(g1);
			generici.AddItem(g2);
			generici.AddItem(g3);

			TabLeftItem piedi = new("A piedi", LeftItemType.Statistics);
			StatsTabItem p1 = new("Azione", "Tasto E / Dpad destra");
			piedi.AddItem(p1);

			TabLeftItem veicolo = new("Su veicolo", LeftItemType.Statistics);
			StatsTabItem v1 = new("Accendi veicolo", "Tasto F10 / LB+A");
			StatsTabItem v2 = new("Allaccia / Slaccia cintura", "Tasto X / LB+X");
			veicolo.AddItem(v1);
			veicolo.AddItem(v2);

			TabLeftItem Lavoro = new("Lavoro", LeftItemType.Statistics);
			StatsTabItem l1 = new StatsTabItem("Menu lavorativo (solo alcuni lavori)", "Tasto F6 / Menu personale");
			StatsTabItem l2 = new StatsTabItem("In servizio / Fuori servizio (solo alcuni lavori)", "Tasto F6 / Menu personale");
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
				Client.Logger.Debug( Funzioni.CaricaKvpString("SettingsClient"));
			};

			MainMenu.Visible = true;
		}
	}
}