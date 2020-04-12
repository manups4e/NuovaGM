using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.MenuGm;
using NuovaGM.Shared;
using NuovaGM.Client.gmPrincipale.Utility.HUD;

namespace NuovaGM.Client.gmPrincipale.NuovoIngresso
{
	static class NuovoIngresso
	{
		public static bool guiEnabled = false;
		static List<string> scenari = new List<string>
		{
			"WORLD_HUMAN_AA_SMOKE", "WORLD_HUMAN_BINOCULARS", "WORLD_HUMAN_BUM_FREEWAY",
			"WORLD_HUMAN_BUM_SLUMPED", "WORLD_HUMAN_BUM_STANDING", "WORLD_HUMAN_BUM_WASH",
			"WORLD_HUMAN_CAR_PARK_ATTENDANT", "WORLD_HUMAN_CHEERING", "WORLD_HUMAN_CLIPBOARD",
			"WORLD_HUMAN_CONST_DRILL", "WORLD_HUMAN_COP_IDLES", "WORLD_HUMAN_DRINKING",
			"WORLD_HUMAN_DRUG_DEALER", "WORLD_HUMAN_DRUG_DEALER_HARD", "WORLD_HUMAN_MOBILE_FILM_SHOCKING",
			"WORLD_HUMAN_GARDENER_LEAF_BLOWER", "WORLD_HUMAN_GARDENER_PLANT", "WORLD_HUMAN_GOLF_PLAYER",
			"WORLD_HUMAN_GUARD_PATROL", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_GUARD_STAND_ARMY",
			"WORLD_HUMAN_HAMMERING", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_HUMAN_STATUE",
			"WORLD_HUMAN_JANITOR", "WORLD_HUMAN_JOG_STANDING", "WORLD_HUMAN_LEANING",
			"WORLD_HUMAN_MAID_CLEAN", "WORLD_HUMAN_MUSCLE_FLEX", "WORLD_HUMAN_MUSCLE_FREE_WEIGHTS",
			"WORLD_HUMAN_MUSICIAN",	"WORLD_HUMAN_PAPARAZZI", "WORLD_HUMAN_PARTYING",
			"WORLD_HUMAN_PROSTITUTE_HIGH_CLASS", "WORLD_HUMAN_PUSH_UPS", "WORLD_HUMAN_SECURITY_SHINE_TORCH",
			"WORLD_HUMAN_SIT_UPS", "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_SMOKING_POT",
			"WORLD_HUMAN_STAND_FISHING", "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STRIP_WATCH_STAND",
			"WORLD_HUMAN_TENNIS_PLAYER", "WORLD_HUMAN_TOURIST_MAP", "WORLD_HUMAN_TOURIST_MOBILE",
			"WORLD_HUMAN_WELDING", "WORLD_HUMAN_YOGA", "CODE_HUMAN_MEDIC_KNEEL",
			"CODE_HUMAN_MEDIC_TEND_TO_DEAD", "CODE_HUMAN_MEDIC_TIME_OF_DEATH",
		};

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("attiva", new Action(Attiva));
			Client.GetInstance.RegisterNuiEventHandler("back-indietro", new Action(() => { ToggleMenu(false, ""); ToggleMenu(true, "flex");}));
			Client.GetInstance.RegisterNuiEventHandler("previewChar", new Action<dynamic>(SelezionatoPreview));
			Client.GetInstance.RegisterNuiEventHandler("char-select", new Action<dynamic>(Selezionato));
			Client.GetInstance.RegisterNuiEventHandler("disconnect", new Action<dynamic>(Disconnetti));
			Client.GetInstance.RegisterNuiEventHandler("new-character", new Action<dynamic>(NuovoPersonaggio));
			Client.GetInstance.RegisterEventHandler("lprp:sceltaCharSelect", new Action<string>(Scelta));
			RequestModel((uint)PedHash.FreemodeMale01);
			RequestModel((uint)PedHash.FreemodeFemale01);
		}

		static Ped femmi;
		private static async void Attiva()
		{
			guiEnabled = true;
			Screen.Fading.FadeIn(1000);
			RequestModel((uint)PedHash.FreemodeFemale01);
			while (!HasModelLoaded((uint)PedHash.FreemodeFemale01))
			{
				await BaseScript.Delay(1);
			}

			femmi = new Ped(CreatePed(26, (uint)PedHash.FreemodeFemale01, Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y + 0.5f, 199f, 0, true, false));
			femmi.IsPositionFrozen = true;
			femmi.IsVisible = false;
			femmi.IsCollisionEnabled = false;
			TimeWeather.Meteo.SetMeteo((int)Weather.ExtraSunny, false, true);
			NetworkOverrideClockTime(Funzioni.GetRandomInt(0, 23), Funzioni.GetRandomInt(0, 59), Funzioni.GetRandomInt(0, 59));
			ShutdownLoadingScreenNui();
			await BaseScript.Delay(1000);
			ToggleMenu(true, "flex");
		}
		private static void ToggleMenu(bool menuOpen, string menu)
		{
			if (menuOpen)
			{
				DisplayHud(false);
				DisplayRadar(false);
				SetEnableHandcuffs(PlayerPedId(), true);
				RemoveAllPedWeapons(PlayerPedId(), true);
				SetNuiFocus(menuOpen, menuOpen);
				Funzioni.SendNuiMessage(new { type = "toggleMenu", menuStatus = menu, menu = "charloading", data = JsonConvert.SerializeObject(Game.Player.GetPlayerData().char_data) });
			}
			else
			{
				DisplayHud(true);
				DisplayRadar(true);
				SetEnableHandcuffs(PlayerPedId(), false);
				SetNuiFocus(menuOpen, menuOpen);
				Funzioni.SendNuiMessage(new { type = "toggleMenu", menuStatus = menuOpen });
			}
		}

		static Ped p1;
		private static async void SelezionatoPreview(dynamic data)
		{
			Char_data pers = Game.Player.GetPlayerData().char_data.FirstOrDefault(x => x.id-1 == Convert.ToInt32(data.slot));
			if (p1 != null) p1.Delete();
			if (pers.skin.sex == "Maschio")
				p1 = new Ped(CreatePed(26, (uint)PedHash.FreemodeMale01, Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y + 0.5f, Game.PlayerPed.Position.Z - 1, 0, false, false));
			else
				p1 = new Ped(CreatePed(26, (uint)PedHash.FreemodeFemale01, Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y + 0.5f, Game.PlayerPed.Position.Z - 1, 0, false, false));
			await Menus.UpdateDress(p1, pers.dressing);
			await Menus.UpdateFace(p1, pers.skin);
			p1.IsPositionFrozen = true;
			p1.BlockPermanentEvents = true;
			string scena = scenari[Funzioni.GetRandomInt(scenari.Count)];
			p1.Task.StartScenario(scena, p1.Position);
			Client.GetInstance.RegisterTickHandler(Controllo);
		}

		private static async void Selezionato(dynamic data)
		{
			guiEnabled = false;
			if (femmi != null) if (femmi.Exists()) femmi.Delete();
			if (p1 != null) if (p1.Exists()) p1.Delete();
			ToggleMenu(false, "");
			Screen.Fading.FadeOut(800);
			await BaseScript.Delay(1000);
			HUD.MenuPool.CloseAllMenus();
			Screen.LoadingPrompt.Show("Caricamento del Personaggio", LoadingSpinnerType.Clockwise1);
			await BaseScript.Delay(3000);
			SwitchOutPlayer(PlayerPedId(), 0, 1);
			DestroyAllCams(true);
			EnableGameplayCam(true);
			await BaseScript.Delay(5000);
			RenderScriptCams(false, false, 0, false, false);
			Screen.Fading.FadeIn(800);
			await BaseScript.Delay(4000);
			Game.PlayerPed.IsInvincible = false;
			await BaseScript.Delay(1000);
			Game.Player.GetPlayerData().char_current = Convert.ToInt32(data.slot+1);
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", Game.Player.GetPlayerData().char_current);
			Char_data Data = Game.Player.GetPlayerData().CurrentChar;
			if (Data.location.x != 0.0 && Data.location.y != 0.0 && Data.location.z != 0.0)
			{
				RequestCollisionAtCoord(Data.location.x, Data.location.y, Data.location.z);
				Game.PlayerPed.Position = new Vector3(Data.location.x, Data.location.y, Data.location.z + 1f);
				Game.PlayerPed.Heading = Data.location.h;
			}
			else
			{
				RequestCollisionAtCoord(Main.firstSpawnCoords.X, Main.firstSpawnCoords.Y, Main.firstSpawnCoords.Z);
				Game.PlayerPed.Position = new Vector3(Main.firstSpawnCoords.X, Main.firstSpawnCoords.Y, Main.firstSpawnCoords.Z);
				Game.PlayerPed.Heading = Main.firstSpawnCoords.W;

			}
			Eventi.LoadModel();
			Game.PlayerPed.IsPositionFrozen = false;
			Game.Player.GetPlayerData().Stanziato = false;
			Game.PlayerPed.IsVisible = true;
			Game.PlayerPed.IsCollisionEnabled = true;
			NetworkClearClockTimeOverride();
			AdvanceClockTimeTo(TimeWeather.Orario.h, TimeWeather.Orario.m, TimeWeather.Orario.s);
			await BaseScript.Delay(7000);
			Client.GetInstance.RegisterTickHandler(TimeWeather.Orario.AggiornaTempo);
			BaseScript.TriggerServerEvent("changeWeatherForMe", true);
			await BaseScript.Delay(5000);
			if (Screen.LoadingPrompt.IsActive)
				Screen.LoadingPrompt.Hide();
			SwitchInPlayer(Game.PlayerPed.Handle);
			await BaseScript.Delay(1000);
			Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
			BaseScript.TriggerEvent("lprp:onPlayerSpawn");
			BaseScript.TriggerServerEvent("lprp:onPlayerSpawn");
			Client.GetInstance.DeregisterTickHandler(Controllo);
//			Client.GetInstance.DeregisterTickHandler(Scaleform);
//			Client.GetInstance.DeregisterTickHandler(TastiMenu);
		}

		private static void Disconnetti(dynamic data)
		{
			ToggleMenu(false, "");
			BaseScript.TriggerEvent("lprp:manager:warningMessage", "Stai uscendo dal gioco senza aver selezionato un personaggio", "Sei sicuro?", 16392, "lprp:sceltaCharSelect");
		}

		private static async Task Controllo()
		{
			Game.DisableAllControlsThisFrame(0);
			Game.DisableAllControlsThisFrame(1);
			Game.DisableAllControlsThisFrame(2);
			Game.DisableAllControlsThisFrame(3);
			Game.DisableAllControlsThisFrame(4);
			Game.DisableAllControlsThisFrame(5);
			Game.DisableAllControlsThisFrame(6);
			Game.DisableAllControlsThisFrame(7);
			Game.DisableAllControlsThisFrame(8);
			Game.DisableAllControlsThisFrame(9);
			Game.DisableAllControlsThisFrame(10);
			Game.DisableAllControlsThisFrame(11);
			Game.DisableAllControlsThisFrame(12);
			Game.DisableAllControlsThisFrame(13);
			Game.DisableAllControlsThisFrame(14);
			Game.DisableAllControlsThisFrame(15);
			Game.DisableAllControlsThisFrame(16);
			Game.DisableAllControlsThisFrame(17);
			Game.DisableAllControlsThisFrame(18);
			Game.DisableAllControlsThisFrame(19);
			Game.DisableAllControlsThisFrame(20);
			Game.DisableAllControlsThisFrame(21);
			Game.DisableAllControlsThisFrame(22);
			Game.DisableAllControlsThisFrame(23);
			Game.DisableAllControlsThisFrame(24);
			Game.DisableAllControlsThisFrame(25);
			Game.DisableAllControlsThisFrame(26);
			Game.DisableAllControlsThisFrame(27);
			Game.DisableAllControlsThisFrame(28);
			Game.DisableAllControlsThisFrame(29);
			Game.DisableAllControlsThisFrame(30);
			Game.DisableAllControlsThisFrame(31);
			if (p1.Exists())
				p1.Heading += 1.2f;
		}

		private static async void NuovoPersonaggio(dynamic data)
		{
			Screen.Fading.FadeOut(800);
			await BaseScript.Delay(1000);
			ToggleMenu(false, "");
			Menus.CharCreationMenu(data.nome, data.cogn, data.dob, data.sesso);
		}

		public static async void Scelta(string param)
		{
			if (param == "select")
				BaseScript.TriggerServerEvent("lprp:dropPlayer", "Grazie di essere passato da " + Client.Impostazioni.Main.NomeServer + "!");
			else
				ToggleMenu(true, "flex");
		}

	}
}
