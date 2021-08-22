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
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Client.NativeUI.PauseMenu;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Personale;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	internal static class PauseMenu
	{
		public static TabView MainMenu = new TabView("The Last Planet", "Full RP");
		private static readonly List<dynamic> filtri = new List<dynamic>()
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
			PlayerChar.User pl = Cache.PlayerCache.MyPlayer.User;
			TabInteractiveListItem HUD = null;
			TabInteractiveListItem Telecamere = null;

			#region HUD MenuItems

			#region cinema

			UIMenuCheckboxItem aa = new UIMenuCheckboxItem("Modalità Cinema", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ModCinema, "");
			UIMenuSliderProgressItem ab = new UIMenuSliderProgressItem("Spessore LetterBox", 100, (int)Main.ImpostazioniClient.LetterBox);
			UIMenuListItem ac = new UIMenuListItem("Filtro cinema", filtri, filtri.IndexOf(Main.ImpostazioniClient.Filtro));
			UIMenuSliderProgressItem ad = new UIMenuSliderProgressItem("Intensita filtro", 100, (int)(Main.ImpostazioniClient.FiltroStrenght * 100));
			aa.CheckboxEvent += (item, attiva) => Main.ImpostazioniClient.ModCinema = attiva;
			ab.OnSliderChanged += (item, index) => Main.ImpostazioniClient.LetterBox = index;
			ad.OnSliderChanged += (item, index) =>
			{
				Main.ImpostazioniClient.FiltroStrenght = index / 100f;
				SetTimecycleModifierStrength(Main.ImpostazioniClient.FiltroStrenght);
			};
			ac.OnListChanged += (item, index) =>
			{
				string activeItem = item.Items[index].ToString();
				string effect;

				switch (activeItem)
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
				Main.ImpostazioniClient.Filtro = activeItem;
			};

			#endregion

			UIMenuSeparatorItem ae = new UIMenuSeparatorItem(); // SEPARATORE

			#region Minimappa

			UIMenuCheckboxItem af = new UIMenuCheckboxItem("Minimappa attiva", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.MiniMappaAttiva, "");
			UIMenuListItem ag = new UIMenuListItem("Dimensioni Minimappa", new List<dynamic>() { "Normale", "Grande" }, Main.ImpostazioniClient.DimensioniMinimappa);
			UIMenuCheckboxItem ah = new UIMenuCheckboxItem("Gps in macchina", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.MiniMappaInAuto, "");
			af.CheckboxEvent += (item, check) => Main.ImpostazioniClient.MiniMappaAttiva = check;
			ag.OnListChanged += (item, index) => Main.ImpostazioniClient.DimensioniMinimappa = index;
			ah.CheckboxEvent += (item, check) => Main.ImpostazioniClient.MiniMappaInAuto = check;

			#endregion

			List<UIMenuItem> hudList = new List<UIMenuItem>()
			{
				aa,
				ab,
				ac,
				ad,
				ae,
				af,
				ag,
				ah
			};
			HUD = new TabInteractiveListItem("HUD", hudList);

			#endregion

			#region Telecamere

			UIMenuCheckboxItem ba = new UIMenuCheckboxItem("Mira in soggettiva", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ForzaPrimaPersona_Mira, "");
			UIMenuCheckboxItem bb = new UIMenuCheckboxItem("Copertura in soggettiva (sovrascrive la mira in soggettiva)", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ForzaPrimaPersona_InCopertura, "");
			UIMenuCheckboxItem bc = new UIMenuCheckboxItem("Soggettiva nei veicoli (sovrascrive la mira in soggettiva)", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ForzaPrimaPersona_InAuto, "");
			ba.CheckboxEvent += (item, check) => Main.ImpostazioniClient.ForzaPrimaPersona_Mira = check;
			bb.CheckboxEvent += (item, check) => Main.ImpostazioniClient.ForzaPrimaPersona_InCopertura = check;
			bc.CheckboxEvent += (item, check) => Main.ImpostazioniClient.ForzaPrimaPersona_InAuto = check;
			List<UIMenuItem> camereList = new List<UIMenuItem>() { ba, bb, bc };
			Telecamere = new TabInteractiveListItem("Telecamere", camereList);

			#endregion

			MainMenu = new TabView("The Last Planet", "Full RP") { SideStringTop = Cache.PlayerCache.MyPlayer.Player.Name, SideStringMiddle = DateTime.Now.ToString(), SideStringBottom = "Portafoglio: $" + pl.Money + " Soldi Sporchi: $" + pl.DirtyCash, DisplayHeader = true };
			TabTextItem intro = new TabTextItem("Introduzione", "Benvenuto su The Last Planet");
			intro.Text = "Questa pagina ti accompagnerà nella gestione delle tue ~y~impostazioni personali~w~ e come ~y~enciclopedia~w~ nel server.\n" + "Qui potrai trovare i comandi che il nostro server utilizza per farti giocare.\n" + "Potrai in ogni mento riaprire questo menu di pausa premendo i tasti SHIFT+F9 oppure con il comando /help";
			TabSubmenuItem comandi = new TabSubmenuItem("Guida ai Comandi [Tastiera / Joypad]", new List<TabItem>() { new TabItemSimpleList("Generici (sempre validi)", new Dictionary<string, string>() { ["Menu Personale"] = "Tasto M / Select", ["Lista giocatori"] = "Tasto Z / Dpad giù", ["Portafoglio"] = "Tasto Z / Dpad giù" }), new TabItemSimpleList("A piedi", new Dictionary<string, string>() { ["Azione"] = "Tasto E / Dpad destra" }), new TabItemSimpleList("Su veicolo", new Dictionary<string, string>() { ["Accendi veicolo"] = "Tasto F10 / LB+A", ["Allaccia / Slaccia cintura"] = "Tasto X / LB+X" }), new TabItemSimpleList("Lavoro", new Dictionary<string, string>() { ["Menu lavorativo (solo alcuni lavori)"] = "Tasto F6 / Menu personale", ["In servizio / Fuori servizio (solo alcuni lavori)"] = "Tasto F6 / Menu personale" }) });
			TabSubmenuItem impostazioni = new TabSubmenuItem("Impostazioni", new List<TabItem>() { HUD, Telecamere });
			MainMenu.AddTab(intro);
			MainMenu.AddTab(comandi);
			MainMenu.AddTab(impostazioni);
			intro.Active = true;
			intro.Focused = true;
			intro.Visible = true;
			MainMenu.OnMenuClose += () =>
			{
				Funzioni.SalvaKvpString("SettingsClient", Main.ImpostazioniClient.ToJson());
				Client.Logger.Debug( Funzioni.CaricaKvpString("SettingsClient"));
			};
			MainMenu.Visible = true;
		}
	}
}