using System;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Client.RolePlay.LogIn;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;
using Font = CitizenFX.Core.UI.Font;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	public enum NotificationType : int
	{
		Default = 0,
		Bubble = 1,
		Mail = 2,
		FriendRequest = 3,
		Default2 = 4,
		Reply = 7,
		ReputationPoints = 8,
		Money = 9
	}

	public sealed class Notifica
	{
		#region Fields

		private int _handle;

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
		RedDifferent = 52
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
		public static TimerBarPool TimerBarPool = new();
		public static MenuPool MenuPool = new();

		public static void Init()
		{
			MenuPool.RefreshIndex();
			ProximityChat.Init();
			Client.Instance.AddTick(Menus);
		}

		public static async Task Menus()
		{
			MenuPool.ProcessMenus();
			if (TimerBarPool.ToList().Count > 0) TimerBarPool.Draw();
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
			AddTextEntry("LprpNotification", msg);
			BeginTextCommandThefeedPost("LprpNotification");

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
			AddTextEntry("LprpNotification", msg);
			BeginTextCommandThefeedPost("LprpNotification");
			ThefeedNextPostBackgroundColor((int)color);

			return new Notifica(EndTextCommandThefeedPostTicker(blink, true));
		}

		/// <summary>
		/// Il testo che viene mostrato in alto a destra dello schermo
		/// </summary>
		/// <param name="helpText">Testo da mostrare</param>
		public static void ShowHelp(string helpText)
		{
			if (!IsPlayerSwitchInProgress() && !MenuPool.IsAnyMenuOpen && !LogIn.GuiEnabled)
			{
				AddTextEntry("LastPlanetHelpText", helpText);
				DisplayHelpTextThisFrame("LastPlanetHelpText", false);
			}
		}

		public static void ShowHelpNoMenu(string helpText)
		{
			if (!IsPlayerSwitchInProgress() && !LogIn.GuiEnabled)
			{
				AddTextEntry("LastPlanetHelpText", helpText);
				DisplayHelpTextThisFrame("LastPlanetHelpText", false);
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

			if (!IsPlayerSwitchInProgress() && !MenuPool.IsAnyMenuOpen)
			{
				AddTextEntry("LastPlanetHelpText", helpText);
				BeginTextCommandDisplayHelp("LastPlanetHelpText");
				EndTextCommandDisplayHelp(0, false, true, tempo);
			}
		}

		public static void ShowFloatingHelpNotification(string msg, Position coords, int tempo = -1)
		{
			//if (IsFloatingHelpTextOnScreen(0)) ClearFloatingHelp(0, true);
			//if (IsFloatingHelpTextOnScreen(1)) ClearFloatingHelp(1, true);
			AddTextEntry("LprpFloatingHelpText", msg);
			SetFloatingHelpTextWorldPosition(1, coords.X, coords.Y, coords.Z);
			SetFloatingHelpTextStyle(1, 1, 2, -1, 3, 0);
			BeginTextCommandDisplayHelp("LprpFloatingHelpText");
			EndTextCommandDisplayHelp(2, false, false, tempo);
		}

		public static async void ShowStatNotification(int value, string title)
		{
			Tuple<int, string> mug = await Funzioni.GetPedMugshotAsync(Cache.PlayerCache.MyPlayer.Ped);
			BeginTextCommandThefeedPost("PS_UPDATE");
			AddTextComponentInteger(value);
			Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_STATS, title, 2, value, value - 1, false, mug.Item2, mug.Item2);
			EndTextCommandThefeedPostTicker(false, true);
			UnregisterPedheadshot(mug.Item1);
		}

		public static async void ShowVSNotification(Ped otherPed, HudColor color1, HudColor color2)
		{
			Tuple<int, string> mug = await Funzioni.GetPedMugshotAsync(Cache.PlayerCache.MyPlayer.Ped);
			Tuple<int, string> otherMug = await Funzioni.GetPedMugshotAsync(otherPed);
			BeginTextCommandThefeedPost("");
			Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_VERSUS_TU, mug.Item2, mug.Item2, 12, otherMug.Item2, otherMug.Item2, 1, color1, color2);
		}

		public static async void ShowVSNotification(Ped otherPed1, Ped otherPed2, HudColor color1, HudColor color2)
		{
			Tuple<int, string> mug = await Funzioni.GetPedMugshotAsync(otherPed1);
			Tuple<int, string> otherMug = await Funzioni.GetPedMugshotAsync(otherPed2);
			BeginTextCommandThefeedPost("");
			Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_VERSUS_TU, mug.Item2, mug.Item2, 12, otherMug.Item2, otherMug.Item2, 1, color1, color2);
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
			BeginTextCommandThefeedPost("jamyfafi");                                                             //icontype 1 → Chat Box --2 → Email
			AddTextComponentSubstringPlayerName(testo);                                                          //3 → Add Friend Request--7 → Right Jumping Arrow
			EndTextCommandThefeedPostMessagetext(immagine, immagine, false, (int)iconType, titolo, sottotitolo); //8 → RP Icon --9 → $ Icon
			EndTextCommandThefeedPostTicker(lampeggia, true);
		}

		public static void ShowAdvancedNotification(string text, string title, string subtitle = "", string iconSet = "CHAR_WE", string icon = "REBOOTBOTTOM", HudColor bgColor = HudColor.NONE, Color flashColor = new(), bool blink = false, NotificationType type = NotificationType.Default, bool showInBrief = true, bool sound = true)
		{
			BeginTextCommandThefeedPost("STRING");
			AddTextComponentSubstringPlayerName(text);
			if (bgColor != HudColor.NONE) SetNotificationBackgroundColor((int)bgColor);
			if (!flashColor.IsEmpty && blink) SetNotificationFlashColor(flashColor.R, flashColor.G, flashColor.B, flashColor.A);
			EndTextCommandThefeedPostMessagetext(iconSet, icon, true, (int)type, title, subtitle);
			EndTextCommandThefeedPostTicker(blink, showInBrief);
			if (sound) Audio.PlaySoundFrontend("DELETE", "HUD_DEATHMATCH_SOUNDSET");
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
			while (UpdateOnscreenKeyboard() == 0) await BaseScript.Delay(0);

			return UpdateOnscreenKeyboard() == 2 ? "" : GetOnscreenKeyboardResult();
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
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
		}

		public static void CallFunctionFrontendHeader(string function, params object[] arguments)
		{
			BeginScaleformMovieMethodOnFrontendHeader(function);
			foreach (object argument in arguments)
				if (argument is int)
					PushScaleformMovieMethodParameterInt((int)argument);
				else if (argument is string)
					PushScaleformMovieMethodParameterString((string)argument);
				else if (argument is char)
					PushScaleformMovieMethodParameterString(argument.ToString());
				else if (argument is float)
					PushScaleformMovieMethodParameterFloat((float)argument);
				else if (argument is double)
					PushScaleformMovieMethodParameterFloat((float)(double)argument);
				else if (argument is bool) PushScaleformMovieMethodParameterBool((bool)argument);
			EndScaleformMovieMethod();
		}

		public static void DrawText3D(Position coord, Color c, string text, Font font = Font.ChaletComprimeCologne, float scale = 17)
		{
			Vector3 cam = GameplayCamera.Position;
			float dist = coord.Distance(cam);
			float scaleInternal = 1 / dist * scale;
			float fov = 1 / GameplayCamera.FieldOfView * 100;
			float _scale = scaleInternal * fov;
			SetTextScale(0.1f * _scale, 0.15f * _scale);
			SetTextFont((int)font);
			SetTextProportional(true);
			SetTextColour(c.R, c.G, c.B, c.A);
			SetTextDropshadow(5, 0, 0, 0, 255);
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

		public static void DrawText3D(Camera camera, Position coord, Color c, string text, Font font = Font.ChaletComprimeCologne, float scale = 17)
		{
			Vector3 cam = camera.Position;
			float dist = coord.Distance(cam);
			float scaleInternal = 1 / dist * scale;
			float fov = 1 / camera.FieldOfView * 100;
			float _scale = scaleInternal * fov;
			SetTextScale(0.1f * _scale, 0.15f * _scale);
			SetTextFont((int)font);
			SetTextProportional(true);
			SetTextColour(c.R, c.G, c.B, c.A);
			SetTextDropshadow(5, 0, 0, 0, 255);
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
			SetTextCentre(false);
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

		public static void DrawText(float x, float y, string text, Color color, Font font)
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

		public static void DrawText(float x, float y, string text, Color color, Font font, Alignment TextAlignment)
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

		public static void DrawText(float x, float y, string text, Color color, Font font, Alignment TextAlignment, bool Shadow = false, bool Outline = false, float Wrap = 0)
		{
			int screenw = Screen.Resolution.Width;
			int screenh = Screen.Resolution.Height;
			const float height = 1080f;
			float ratio = (float)screenw / screenh;
			float width = height * ratio;
			SetTextFont((int)font);
			SetTextScale(0.0f, 0.5f);
			SetTextColour(color.R, color.G, color.B, color.A);
			if (Shadow) SetTextDropShadow();
			if (Outline) SetTextOutline();

			if (Wrap != 0)
			{
				float xsize = (x + Wrap) / width;
				SetTextWrap(x, xsize);
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

		public static async Task ShowPlayerRankScoreAfterUpdate(int currentRankLimit, int nextRankLimit, int playersPreviousXP, int playersCurrentXP, int rank)
		{
			RequestHudScaleform(19);
			while (!HasHudScaleformLoaded(19)) await BaseScript.Delay(0);
			PushScaleformMovieFunctionFromHudComponent(19, "OVERRIDE_ANIMATION_SPEED");
			PushScaleformMovieFunctionParameterInt(2000);
			PopScaleformMovieFunctionVoid();
			PushScaleformMovieFunctionFromHudComponent(19, "SET_COLOUR");
			PushScaleformMovieFunctionParameterInt(116);
			PushScaleformMovieFunctionParameterInt(123);
			PopScaleformMovieFunctionVoid();
			BeginScaleformMovieMethodHudComponent(19, "SET_RANK_SCORES");
			PushScaleformMovieFunctionParameterInt(currentRankLimit);
			PushScaleformMovieFunctionParameterInt(nextRankLimit);
			PushScaleformMovieFunctionParameterInt(playersPreviousXP);
			PushScaleformMovieFunctionParameterInt(playersCurrentXP);
			PushScaleformMovieFunctionParameterInt(rank);
			PopScaleformMovieFunctionVoid();
		}

		public static void StayOnScreenPlayerRank()
		{
			PushScaleformMovieFunctionFromHudComponent(19, "SHOW");
			PopScaleformMovieFunctionVoid();
		}

		public static async Task ShowPlayerRank(bool show)
		{
			if (!show)
			{
				PushScaleformMovieFunctionFromHudComponent(19, "HIDE");
				PopScaleformMovieFunctionVoid();
			}
			else
			{
				PushScaleformMovieFunctionFromHudComponent(19, "SHOW");
				PopScaleformMovieFunctionVoid();
			}

			if (HasHudScaleformLoaded(19)) return;
			RequestHudScaleform(19);
			while (!HasHudScaleformLoaded(19)) await BaseScript.Delay(0);
			int rank = Cache.PlayerCache.MyPlayer.User.FreeRoamChar.Level;
			int xp = Cache.PlayerCache.MyPlayer.User.FreeRoamChar.TotalXp;
			int nowMaxXp = Experience.RankRequirement[rank];
			int maxXp = Experience.NextLevelExperiencePoints(rank);
			PushScaleformMovieFunctionFromHudComponent(19, "SET_COLOUR");
			PushScaleformMovieFunctionParameterInt(116);
			PushScaleformMovieFunctionParameterInt(123);
			PopScaleformMovieFunctionVoid();
			BeginScaleformMovieMethodHudComponent(19, "SET_RANK_SCORES");
			PushScaleformMovieFunctionParameterInt(nowMaxXp);
			PushScaleformMovieFunctionParameterInt(maxXp);
			PushScaleformMovieFunctionParameterInt(xp);
			PushScaleformMovieFunctionParameterInt(xp);
			PushScaleformMovieFunctionParameterInt(rank);
			PopScaleformMovieFunctionVoid();
			PushScaleformMovieFunctionFromHudComponent(19, "STAY_ON_SCREEN");
			PopScaleformMovieFunctionVoid();
		}

		public static async Task ShowFinishScaleform(bool lost = true)
		{
			Scaleform bg = new("MP_CELEBRATION_BG");
			Scaleform fg = new("MP_CELEBRATION_FG");
			Scaleform cb = new("MP_CELEBRATION");
			RequestScaleformMovie("MP_CELEBRATION_BG");
			RequestScaleformMovie("MP_CELEBRATION_FG");
			RequestScaleformMovie("MP_CELEBRATION");
			while (!bg.IsLoaded || !fg.IsLoaded || !cb.IsLoaded) await BaseScript.Delay(0);

			// Setting up colors.
			bg.CallFunction("CREATE_STAT_WALL", "ch", "HUD_COLOUR_BLACK", -1);
			fg.CallFunction("CREATE_STAT_WALL", "ch", "HUD_COLOUR_RED", -1);
			cb.CallFunction("CREATE_STAT_WALL", "ch", "HUD_COLOUR_BLUE", -1);

			// Setting up pause duration.
			bg.CallFunction("SET_PAUSE_DURATION", 3.0f);
			fg.CallFunction("SET_PAUSE_DURATION", 3.0f);
			cb.CallFunction("SET_PAUSE_DURATION", 3.0f);

			//bool won = new Random().Next(0, 2) == 0;
			//bool won = true;
			string win_lose = lost ? "CELEB_LOSER" : "CELEB_WINNER";
			bg.CallFunction("ADD_WINNER_TO_WALL", "ch", win_lose, GetPlayerName(PlayerId()), "", 0, false, "", false);
			fg.CallFunction("ADD_WINNER_TO_WALL", "ch", win_lose, GetPlayerName(PlayerId()), "", 0, false, "", false);
			cb.CallFunction("ADD_WINNER_TO_WALL", "ch", win_lose, GetPlayerName(PlayerId()), "", 0, false, "", false);

			// Setting up background.
			bg.CallFunction("ADD_BACKGROUND_TO_WALL", "ch");
			fg.CallFunction("ADD_BACKGROUND_TO_WALL", "ch");
			cb.CallFunction("ADD_BACKGROUND_TO_WALL", "ch");

			// Preparing to show the wall.
			bg.CallFunction("SHOW_STAT_WALL", "ch");
			fg.CallFunction("SHOW_STAT_WALL", "ch");
			cb.CallFunction("SHOW_STAT_WALL", "ch");

			// Drawing the wall on screen for 3 seconds + 1 seconds (for outro animation druation).
			int timer = GetGameTimer();

			//DisableDrawing = true;
			while (GetGameTimer() - timer <= 3000 + 1000)
			{
				await BaseScript.Delay(0);
				DrawScaleformMovieFullscreenMasked(bg.Handle, fg.Handle, 255, 255, 255, 255);
				DrawScaleformMovieFullscreen(cb.Handle, 255, 255, 255, 255, 0);
				HideHudAndRadarThisFrame();
			}
			//DisableDrawing = false;

			// Playing effect when it's over.
			StartScreenEffect("MinigameEndNeutral", 0, false);
			PlaySoundFrontend(-1, "SCREEN_FLASH", "CELEBRATION_SOUNDSET", false);

			// Cleaning up.
			bg.CallFunction("CLEANUP");
			fg.CallFunction("CLEANUP");
			cb.CallFunction("CLEANUP");
			bg.Dispose();
			fg.Dispose();
			cb.Dispose();
			//GameController.gameRestarting = false;
			//GameController.Go();
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