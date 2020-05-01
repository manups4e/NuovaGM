using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NuovaGM.Client.MenuNativo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.gmPrincipale.Utility.HUD
{
	public sealed class Notifica
	{
		#region Fields
		int _handle;
		#endregion

		internal Notifica(int handle)
		{
			_handle = handle;
		}

		/// <summary>
		/// Hides this <see cref="Notification"/> instantly
		/// </summary>
		public void Hide()
		{
			RemoveNotification(_handle);
		}

	}

	public enum NotificationColor
	{
		Red = 27,
		Yellow = 50,
		Gold = 12,
		GreenLight = 46,
		GreenDark = 47,
		Cyan = 48,
		Blue = 51,
		Purple = 49,
		Rose = 45,
		RedDifferent = 52,
	}

	public enum IconType
	{
		ChatBox = 1,
		Email = 2,
		AdDFriendRequest = 3,
		RightJumpingArrow = 7,
		RPIcon = 8,
		DollarIcon = 9
	}

	public static class HUD
	{
		public static UITimerBarPool TimerBarPool = new UITimerBarPool();
		public static MenuPool MenuPool = new MenuPool();

		public static void Init()
		{
			MenuPool.RefreshIndex();
		}

		public static async Task Menus()
		{
			MenuPool.ProcessMenus();
			if (TimerBarPool.TimerBars.Count > 0)
				TimerBarPool.Draw();
			await Task.FromResult(0);
		}

		/// <summary>
		/// Mostra la notifica stile GTA
		/// </summary>
		/// <param name="msg"> Il Messaggio da mostrare </param>
		/// <param name="blink"> Se vuoi che lampeggi </param>
		/// <returns></returns>
		public static Notifica ShowNotification(string msg, bool blink = false)
		{
			string[] strings = Screen.StringToArray(msg);
			BeginTextCommandThefeedPost("CELL_EMAIL_BCON");
			foreach (string s in strings)
			{
				AddTextComponentSubstringPlayerName(s);
			}
			return new Notifica(EndTextCommandThefeedPostTicker(blink, true));
		}

		/// <summary>
		/// Mostra la notifica stile GTA
		/// </summary>
		/// <param name="msg"> Il Messaggio da mostrare </param>
		/// <param name="color">Il colore di sfondo della notifica </param>
		/// <param name="blink"> Se vuoi che lampeggi </param>
		/// <returns></returns>
		public static Notifica ShowNotification(string msg, NotificationColor color, bool blink = false)
		{
			string[] strings = Screen.StringToArray(msg);
			BeginTextCommandThefeedPost("CELL_EMAIL_BCON");
			foreach (string s in strings)
				AddTextComponentSubstringPlayerName(s);
			ThefeedNextPostBackgroundColor((int)color);
			return new Notifica(EndTextCommandThefeedPostTicker(blink, true));
		}

		/// <summary>
		/// Il testo che viene mostrato in alto a destra dello schermo
		/// </summary>
		/// <param name="helpText">Testo da mostrare</param>
		public static void ShowHelp(string helpText)
		{
			if (!IsPlayerSwitchInProgress() && !MenuPool.IsAnyMenuOpen())
			{
				string[] strings = Screen.StringToArray(helpText);
				BeginTextCommandDisplayHelp("CELL_EMAIL_BCON");
				foreach (string s in strings)
					AddTextComponentSubstringPlayerName(s);
				EndTextCommandDisplayHelp(0, false, true, 1);
			}
		}

		/// <summary>
		/// Il testo che viene mostrato in alto a destra dello schermo
		/// </summary>
		/// <param name="helpText">Testo da mostrare</param>
		/// <param name="tempo">Tempo di permanenza su schermo in millisecondi</param>
		public static void ShowHelp(string helpText, int tempo)
		{
			if (tempo > 5000) tempo = 5000;
			if (!IsPlayerSwitchInProgress() && !MenuPool.IsAnyMenuOpen())
			{
				string[] strings = Screen.StringToArray(helpText);
				BeginTextCommandDisplayHelp("CELL_EMAIL_BCON");
				foreach (string s in strings)
					AddTextComponentSubstringPlayerName(s);
				EndTextCommandDisplayHelp(0, false, true, tempo);
			}
		}

		public static void ShowFloatingHelpNotification(string msg, Vector3 coords)
		{
			SetFloatingHelpTextWorldPosition(1, coords.X, coords.Y, coords.Z);
			SetFloatingHelpTextStyle(1, 1, 2, -1, 3, 0);
			string[] strings = Screen.StringToArray(msg);
			BeginTextCommandDisplayHelp("CELL_EMAIL_BCON");
			foreach (string s in strings)
				AddTextComponentSubstringPlayerName(s);
			EndTextCommandDisplayHelp(2, false, false, -1);
		}

		public static async void ShowStatNotification(int value, string title)
		{
			var mug = await Funzioni.GetPedMugshotAsync(Game.PlayerPed);
			BeginTextCommandThefeedPost("PS_UPDATE");
			AddTextComponentInteger(value);
			Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_STATS, title, 2, value, value - 1, false, mug.Item2, mug.Item2);
			EndTextCommandThefeedPostTicker(false, true);
			UnregisterPedheadshot(mug.Item1);
			/*
				PSF_DRIVING = Driving +
				PSF_FLYING = Flying +
				PSF_LUNG = Lung Capacity +
				PSF_SHOOTING = Shooting +
				PSF_SPEC_AB = Special capacity +
				PSF_STAMINA = Stamina +
				PSF_STEALTH = Stealth +
				PSF_STRENGTH = Strength +
			*/
		}

		/// <summary>
		/// Notifica con immagine (stile sms / mms)
		/// </summary>
		/// <param name="titolo"></param>
		/// <param name="sottotitolo"></param>
		/// <param name="testo"></param>
		/// <param name="immagine"></param>
		/// <param name="iconType"></param>
		/// <param name="lampeggia"></param>
		public static void ShowAdvancedNotification(string titolo, string sottotitolo, string testo, string immagine, IconType iconType, bool lampeggia = false)
		{
			BeginTextCommandThefeedPost("jamyfafi");                                              //icontype 1 → Chat Box --2 → Email
			AddTextComponentSubstringPlayerName(testo);                                        //3 → Add Friend Request--7 → Right Jumping Arrow
			EndTextCommandThefeedPostMessagetext(immagine, immagine, false, (int)iconType, titolo, sottotitolo);             //8 → RP Icon --9 → $ Icon
			EndTextCommandThefeedPostTicker(lampeggia, true);
		}

		/*enum LoadingPromptTypes
        {
            LOADING_PROMPT_LEFT = 1,
            LOADING_PROMPT_LEFT_2 = 2,
            LOADING_PROMPT_LEFT_3 = 3,
            SAVE_PROMPT_LEFT = 4,
            LOADING_PROMPT_RIGHT = 5,
        };
        */

		/// <summary>
		/// Attiva l'attesa di inserimento testo da parte del giocatore
		/// </summary>
		/// <param name="windowTitle">Titolo della finestra</param>
		/// <param name="defaultText">Test di default se c'è</param>
		/// <param name="maxLength">Lunghezza dell'input</param>
		/// <returns></returns>
		public static async Task<string> GetUserInput(string windowTitle, string defaultText, int maxLength)
		{
			ClearKeyboard(windowTitle, defaultText, maxLength);
			while (UpdateOnscreenKeyboard() == 0)
				await BaseScript.Delay(0);
			return GetOnscreenKeyboardResult();
		}
		private static void ClearKeyboard(string windowTitle, string defaultText, int maxLength)
		{
			AddTextEntry("FMlprp_KEY_TIP1", windowTitle);
			DisplayOnscreenKeyboard(1, "FMlprp_KEY_TIP1", null, defaultText, null, null, null, maxLength + 1);
		}

		/// <summary>
		/// Mostra la notifica di salvataggio
		/// </summary>
		/// <param name="text">Testo della notifica</param>
		/// <param name="enumtype">Tipo di rotellina</param>
		/// <param name="msecs">Quanto tempo passa prima che sparisca</param>
		public static async void ShowLoadingSavingNotificationWithTime(string text, LoadingSpinnerType enumtype, int msecs)
		{
			Screen.LoadingPrompt.Show(text, enumtype);
			await BaseScript.Delay(msecs);
			if (Screen.LoadingPrompt.IsActive)
			{
				Screen.LoadingPrompt.Hide();
			}
		}

		public static void DrawText3D(float x, float y, float z, Color c, string text)
		{
			SetTextScale(0.45f, 0.45f);
			SetTextFont(4);
			SetTextProportional(true);
			SetTextColour(c.R, c.G, c.B, 255);
			SetTextDropshadow(0, 0, 0, 0, 255);
			SetTextEdge(2, 0, 0, 0, 150);
			SetTextDropShadow();
			SetTextOutline();
			SetTextCentre(true);
			SetDrawOrigin(x, y, z, 0);
			BeginTextCommandDisplayText("STRING");
			AddTextComponentSubstringPlayerName(text);
			EndTextCommandDisplayText(0, 0);
			ClearDrawOrigin();
		}

		public static void DrawText3D(Vector3 coord, Color c, string text)
		{
			SetTextScale(0.45f, 0.45f);
			SetTextFont(4);
			SetTextProportional(true);
			SetTextColour(c.R, c.G, c.B, 255);
			SetTextDropshadow(0, 0, 0, 0, 255);
			SetTextEdge(2, 0, 0, 0, 150);
			SetTextDropShadow();
			SetTextOutline();
			SetTextCentre(true);
			SetDrawOrigin(coord.X, coord.Y, coord.Z, 0);
			BeginTextCommandDisplayText("STRING");
			AddTextComponentSubstringPlayerName(text);
			EndTextCommandDisplayText(0, 0);
			ClearDrawOrigin();
		}

		public static void DrawText3D(Vector3 coord, Color c, string text, int font)
		{
			SetTextScale(0.45f, 0.45f);
			SetTextFont(font);
			SetTextProportional(true);
			SetTextColour(c.R, c.G, c.B, 255);
			SetTextDropshadow(0, 0, 0, 0, 255);
			SetTextEdge(2, 0, 0, 0, 150);
			SetTextDropShadow();
			SetTextOutline();
			SetTextCentre(true);
			SetDrawOrigin(coord.X, coord.Y, coord.Z, 0);
			BeginTextCommandDisplayText("STRING");
			AddTextComponentSubstringPlayerName(text);
			EndTextCommandDisplayText(0, 0);
			ClearDrawOrigin();
		}

		public static void DrawText(string text)
		{
			SetTextFont(4);
			SetTextProportional(false);
			SetTextScale(0.0f, 0.5f);
			SetTextColour(255, 255, 255, 255);
			SetTextDropshadow(0, 0, 0, 0, 255);
			SetTextEdge(1, 0, 0, 0, 255);
			SetTextDropShadow();
			SetTextOutline();
			SetTextCentre(true);
			SetTextEntry("STRING");
			AddTextComponentSubstringPlayerName(text);
			EndTextCommandDisplayText(0.5f, 0.8f);
		}

		public static void DrawText(float x, float y, string text)
		{
			SetTextFont(4);
			SetTextProportional(false);
			SetTextScale(0.0f, 0.5f);
			SetTextColour(255, 255, 255, 255);
			SetTextDropshadow(0, 0, 0, 0, 255);
			SetTextEdge(1, 0, 0, 0, 255);
			SetTextDropShadow();
			SetTextOutline();
			SetTextCentre(true);
			SetTextEntry("STRING");
			AddTextComponentSubstringPlayerName(text);
			EndTextCommandDisplayText(x, y);
		}

		public static void DrawText(float x, float y, string text, Color color)
		{
			SetTextFont(4);
			SetTextProportional(false);
			SetTextScale(0.0f, 0.5f);
			SetTextColour(color.R, color.G, color.B, color.A);
			SetTextDropshadow(0, 0, 0, 0, 255);
			SetTextEdge(1, 0, 0, 0, 255);
			SetTextDropShadow();
			SetTextOutline();
			SetTextCentre(true);
			SetTextEntry("STRING");
			AddTextComponentSubstringPlayerName(text);
			EndTextCommandDisplayText(x, y);
		}

		public static void DrawText(float x, float y, string text, Color color, CitizenFX.Core.UI.Font font)
		{
			SetTextFont((int)font);
			SetTextScale(0.0f, 0.5f);
			SetTextColour(color.R, color.G, color.B, color.A);
			SetTextDropShadow();
			SetTextOutline();
			SetTextCentre(true);
			SetTextEntry("jamyfafi");
			AddTextComponentSubstringPlayerName(text);
			EndTextCommandDisplayText(x, y);
		}
		public static void DrawText(float x, float y, string text, Color color, CitizenFX.Core.UI.Font font, Alignment TextAlignment)
		{
			SetTextFont((int)font);
			SetTextScale(0.0f, 0.5f);
			SetTextColour(color.R, color.G, color.B, color.A);
			SetTextDropShadow();
			SetTextOutline();
			switch (TextAlignment)
			{
				case Alignment.Center:
					SetTextCentre(true);
					break;
				case Alignment.Right:
					SetTextRightJustify(true);
					SetTextWrap(0, x);
					break;
			}
			SetTextEntry("jamyfafi");
			AddTextComponentSubstringPlayerName(text);
			EndTextCommandDisplayText(x, y);
		}

		public static void DrawText(float x, float y, string text, Color color, CitizenFX.Core.UI.Font font, Alignment TextAlignment, bool Shadow = false, bool Outline = false, float Wrap = 0)
		{
			int screenw = Screen.Resolution.Width;
			int screenh = Screen.Resolution.Height;
			const float height = 1080f;
			float ratio = (float)screenw / screenh;
			var width = height * ratio;

			SetTextFont((int)font);
			SetTextScale(0.0f, 0.5f);
			SetTextColour(color.R, color.G, color.B, color.A);
			if (Shadow)
				API.SetTextDropShadow();
			if (Outline)
				API.SetTextOutline();
			if (Wrap != 0)
			{
				float xsize = (x + Wrap) / width;
				API.SetTextWrap(x, xsize);
			}
			switch (TextAlignment)
			{
				case Alignment.Center:
					SetTextCentre(true);
					break;
				case Alignment.Right:
					SetTextRightJustify(true);
					SetTextWrap(0, x);
					break;
			}
			SetTextEntry("jamyfafi");
			AddTextComponentSubstringPlayerName(text);
			EndTextCommandDisplayText(x, y);
		}
	}


	public static class NotificationIcon
	{
		public static string Abigail = "CHAR_ABIGAIL";
		public static string Amanda = "CHAR_AMANDA";
		public static string Ammunation = "CHAR_AMMUNATION";
		public static string Andreas = "CHAR_ANDREAS";
		public static string Antonia = "CHAR_ANTONIA";
		public static string Ashley = "CHAR_ASHLEY";
		public static string BankOfLiberty = "CHAR_BANK_BOL";
		public static string BankFleeca = "CHAR_BANK_FLEECA";
		public static string BankMaze = "CHAR_BANK_MAZE";
		public static string Barry = "CHAR_BARRY";
		public static string Beverly = "CHAR_BEVERLY";
		public static string BikeSite = "CHAR_BIKESITE";
		public static string BlankEntry = "CHAR_BLANK_ENTRY";
		public static string Blimp = "CHAR_BLIMP";
		public static string Blocked = "CHAR_BLOCKED";
		public static string BoatSite = "CHAR_BOATSITE";
		public static string BrokenDownGirl = "CHAR_BROKEN_DOWN_GIRL";
		public static string BugStars = "CHAR_BUGSTARS";
		public static string Call911 = "CHAR_CALL911";
		public static string LegendaryMotorsport = "CHAR_CARSITE";
		public static string SSASuperAutos = "CHAR_CARSITE2";
		public static string Castro = "CHAR_CASTRO";
		public static string ChatCall = "CHAR_CHAT_CALL";
		public static string Chef = "CHAR_CHEF";
		public static string Cheng = "CHAR_CHENG";
		public static string ChengSenior = "CHAR_CHENGSR";
		public static string Chop = "CHAR_CHOP";
		public static string Cris = "CHAR_CRIS";
		public static string Dave = "CHAR_DAVE";
		public static string Default = "CHAR_DEFAULT";
		public static string Denise = "CHAR_DENISE";
		public static string DetonateBomb = "CHAR_DETONATEBOMB";
		public static string DetonatePhone = "CHAR_DETONATEPHONE";
		public static string Devin = "CHAR_DEVIN";
		public static string SubMarine = "CHAR_DIAL_A_SUB";
		public static string Dom = "CHAR_DOM";
		public static string DomesticGirl = "CHAR_DOMESTIC_GIRL";
		public static string Dreyfuss = "CHAR_DREYFUSS";
		public static string DrFriedlander = "CHAR_DR_FRIEDLANDER";
		public static string Epsilon = "CHAR_EPSILON";
		public static string EstateAgent = "CHAR_ESTATE_AGENT";
		public static string Facebook = "CHAR_FACEBOOK";
		public static string FilmNoire = "CHAR_FILMNOIR";
		public static string Floyd = "CHAR_FLOYD";
		public static string Franklin = "CHAR_FRANKLIN";
		public static string FranklinTrevor = "CHAR_FRANK_TREV_CONF";
		public static string GayMilitary = "CHAR_GAYMILITARY";
		public static string Hao = "CHAR_HAO";
		public static string HitcherGirl = "CHAR_HITCHER_GIRL";
		public static string Hunter = "CHAR_HUNTER";
		public static string Jimmy = "CHAR_JIMMY";
		public static string JimmyBoston = "CHAR_JIMMY_BOSTON";
		public static string Joe = "CHAR_JOE";
		public static string Josef = "CHAR_JOSEF";
		public static string Josh = "CHAR_JOSH";
		public static string LamarDog = "CHAR_LAMAR";
		public static string Lester = "CHAR_LESTER";
		public static string Skull = "CHAR_LESTER_DEATHWISH";
		public static string LesterFranklin = "CHAR_LEST_FRANK_CONF";
		public static string LesterMichael = "CHAR_LEST_MIKE_CONF";
		public static string LifeInvader = "CHAR_LIFEINVADER";
		public static string LsCustoms = "CHAR_LS_CUSTOMS";
		public static string LSTI = "CHAR_LS_TOURIST_BOARD";
		public static string Manuel = "CHAR_MANUEL";
		public static string Marnie = "CHAR_MARNIE";
		public static string Martin = "CHAR_MARTIN";
		public static string MaryAnn = "CHAR_MARY_ANN";
		public static string Maude = "CHAR_MAUDE";
		public static string Mechanic = "CHAR_MECHANIC";
		public static string Michael = "CHAR_MICHAEL";
		public static string MichaelFranklin = "CHAR_MIKE_FRANK_CONF";
		public static string MichaelTrevor = "CHAR_MIKE_TREV_CONF";
		public static string WarStock = "CHAR_MILSITE";
		public static string Minotaur = "CHAR_MINOTAUR";
		public static string Molly = "CHAR_MOLLY";
		public static string MorsMutual = "CHAR_MP_MORS_MUTUAL";
		public static string ArmyContact = "CHAR_MP_ARMY_CONTACT";
		public static string Brucie = "CHAR_MP_BRUCIE";
		public static string FibContact = "CHAR_MP_FIB_CONTACT";
		public static string RockStarLogo = "CHAR_MP_FM_CONTACT";
		public static string Gerald = "CHAR_MP_GERALD";
		public static string Julio = "CHAR_MP_JULIO";
		public static string MechanicChinese = "CHAR_MP_MECHANIC";
		public static string MerryWeather = "CHAR_MP_MERRYWEATHER";
		public static string Unicorn = "CHAR_MP_STRIPCLUB_PR";
		public static string Mom = "CHAR_MRS_THORNHILL";
		public static string MrsThornhill = "CHAR_MRS_THORNHILL";
		public static string PatriciaTrevor = "CHAR_PATRICIA";
		public static string PegasusDelivery = "CHAR_PEGASUS_DELIVERY";
		public static string ElitasTravel = "CHAR_PLANESITE";
		public static string Sasquatch = "CHAR_SASQUATCH";
		public static string Simeon = "CHAR_SIMEON";
		public static string SocialClub = "CHAR_SOCIAL_CLUB";
		public static string Solomon = "CHAR_SOLOMON";
		public static string Taxi = "CHAR_TAXI";
		public static string Trevor = "CHAR_TREVOR";
		public static string YouTube = "CHAR_YOUTUBE";
		public static string Wade = "CHAR_WADE";
	}
}
