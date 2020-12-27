using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.MenuNativo.PauseMenu;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.Personale;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	static class PauseMenu
	{
		public static void Init()
		{
			InputHandler.ListaInput.Add(new InputController(Control.DropWeapon, PadCheck.Keyboard, ControlModifier.Shift, action: new Action<Ped>(LastPlanetMenu)));
		}
		private static async void LastPlanetMenu(Ped playerPed)
		{
			var pl = Game.Player.GetPlayerData();
			TabInteractiveListItem HUD = null;
			#region HUD MenuItems
			#region cinema
			UIMenuCheckboxItem a = new UIMenuCheckboxItem("Modalità Cinema", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.ModCinema, "");
			UIMenuSliderProgressItem b = new UIMenuSliderProgressItem("Spessore LetterBox", 100, (int)Main.ImpostazioniClient.LetterBox);
			UIMenuListItem c = new UIMenuListItem("Filtro cinema", new List<dynamic>() { "Nessuno", "Matrix", "Matrix 1", "Matrix 2", "Noir", "Noir vintage", "2019", "Bianco e Nero", "Bianco e Nero 1", "Sgranato a colori", "Purple Haze", "Kabuchiko", "Kabuchiko GLOOM", "Kabuchiko sgranato", "Silent Hill", "Silent Hill 1" }, 0);
			UIMenuSliderProgressItem ca = new UIMenuSliderProgressItem("Intensita filtro", 100, (int)Main.ImpostazioniClient.FiltroStrenght*100);
			a.CheckboxEvent += async (item, attiva) =>
			{
				Main.ImpostazioniClient.ModCinema = attiva;
			};
			b.OnSliderChanged += async (item, index) =>
			{
				Main.ImpostazioniClient.LetterBox = index;
			};
			ca.OnSliderChanged += async (item, index) =>
			{
				Main.ImpostazioniClient.FiltroStrenght = index/100;
				SetTimecycleModifierStrength(Main.ImpostazioniClient.FiltroStrenght);
			};
			c.OnListChanged += (item, index) =>
			{
				string ActiveItem = item.Items[index].ToString();
				string effect = "None";
				switch (ActiveItem)
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
				SetTimecycleModifier(effect);
				Main.ImpostazioniClient.FiltroStrenght = 50;
				ca.Value = 50;
			};
			#endregion
			UIMenuSeparatorItem d = new UIMenuSeparatorItem(); // SEPARATORE
			#region Minimappa
			UIMenuCheckboxItem e = new UIMenuCheckboxItem("Minimappa attiva", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.MiniMappaAttiva, "");
			UIMenuListItem f = new UIMenuListItem("Dimensioni Minimappa", new List<dynamic>() { "Normale", "Grande" }, Main.ImpostazioniClient.DimensioniMinimappa);
			UIMenuCheckboxItem g = new UIMenuCheckboxItem("Gps in macchina", UIMenuCheckboxStyle.Tick, Main.ImpostazioniClient.MiniMappaInAuto, "");

			e.CheckboxEvent += (item, check) =>
			{
				Main.ImpostazioniClient.MiniMappaAttiva = check;
			};
			f.OnListChanged += (item, index) =>
			{
				Main.ImpostazioniClient.DimensioniMinimappa = index;
			};
			g.CheckboxEvent += (item, check) =>
			{
				Main.ImpostazioniClient.MiniMappaInAuto = check;
			};
			#endregion

			List<UIMenuItem> hudList = new List<UIMenuItem>() { a, b, c, ca, d, e, f, g};
			HUD = new TabInteractiveListItem("HUD", hudList);

			#endregion

			TabView MainMenu = new TabView("The Last Planet", "Full RP")
			{
				SideStringTop = Game.Player.Name,
				SideStringMiddle = DateTime.Now.ToString(),
				SideStringBottom = "Portafoglio: $" + pl.Money + " Soldi Sporchi: $" + pl.DirtyMoney,
				DisplayHeader = true
			};
			TabTextItem intro = new TabTextItem("Introduzione", "Benvenuto su The Last Planet");
			intro.Text = "Questa pagina ti accompagnerà nella gestione delle tue ~y~impostazioni personali~w~ e come ~y~enciclopedia~w~ nel server.\n" +
				"Qui potrai trovare i comandi che il nostro server utilizza per farti giocare.\n" +
				"Potrai in ogni mento riaprire questo menu di pausa premendo i tasti SHIFT+F9 oppure con il comando /help";
			TabSubmenuItem comandi = new TabSubmenuItem("Guida ai Comandi [Tastiera / Joypad]", new List<TabItem>()
			{
				new TabItemSimpleList("Generici (sempre validi)", new Dictionary<string, string>()
				{
					["Menu Personale"] = "Tasto M / Select",
					["Lista giocatori"] = "Tasto Z / Dpad giù",
					["Portafoglio"] = "Tasto Z / Dpad giù",
				}),
				new TabItemSimpleList("A piedi", new Dictionary<string, string>()
				{
					["Azione"] = "Tasto E / Dpad destra",
				}),
				new TabItemSimpleList("Su veicolo", new Dictionary<string, string>()
				{
					["Accendi veicolo"] = "Tasto F10 / LB+A",
					["Allaccia / Slaccia cintura"] = "Tasto X / LB+X",
				}),
				new TabItemSimpleList("Lavoro", new Dictionary<string, string>()
				{
					["Menu lavorativo (solo alcuni lavori)"] = "Tasto F6 / Menu personale",
					["In servizio / Fuori servizio (solo alcuni lavori)"] = "Tasto F6 / Menu personale",
				})
			});

			TabSubmenuItem impostazioni = new TabSubmenuItem("Guida ai Comandi [Tastiera / Joypad]", new List<TabItem>()
			{
				HUD,
			});

			MainMenu.AddTab(intro);
			MainMenu.AddTab(comandi);
			MainMenu.AddTab(impostazioni);
			intro.Active = true;
			intro.Focused = true;
			intro.Visible = true;

			MainMenu.OnMenuClose += () =>
			{
				Funzioni.SalvaKVPString("SettingsClient", Main.ImpostazioniClient.Serialize());
				Log.Printa(LogType.Debug, Funzioni.CaricaKVPString("SettingsClient"));
			};
			MainMenu.Visible = true;

		}
	}
}
