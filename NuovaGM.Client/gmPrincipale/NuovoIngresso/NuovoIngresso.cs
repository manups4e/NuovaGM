using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.MenuGm;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Shared;
using Logger;

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
			Client.Instance.AddEventHandler("attiva", new Action(Attiva));
			Client.Instance.RegisterNuiEventHandler("back-indietro", new Action(() => ToggleMenu(true, "charloading")));
			Client.Instance.RegisterNuiEventHandler("previewChar", new Action<dynamic>(SelezionatoPreview));
			Client.Instance.RegisterNuiEventHandler("char-select", new Action<dynamic>(Selezionato));
			Client.Instance.RegisterNuiEventHandler("disconnect", new Action<dynamic>(Disconnetti));
			Client.Instance.RegisterNuiEventHandler("new-character", new Action<dynamic>(NuovoPersonaggio));
			Client.Instance.AddEventHandler("lprp:sceltaCharSelect", new Action<string>(Scelta));
			RequestModel((uint)PedHash.FreemodeMale01);
			RequestModel((uint)PedHash.FreemodeFemale01);
			Ped test = new Ped(CreatePed(26, (uint)PedHash.FreemodeMale01, Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y + 0.5f, Game.PlayerPed.Position.Z - 1, 0, false, false));
			test.Delete();
		}

		static Ped femmi;
		private static async void Attiva()
		{
			guiEnabled = true;
			Screen.Fading.FadeIn(1000);
			RequestModel((uint)PedHash.FreemodeFemale01);
			while (!HasModelLoaded((uint)PedHash.FreemodeFemale01))await BaseScript.Delay(1);

			femmi = new Ped(CreatePed(26, (uint)PedHash.FreemodeFemale01, Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y + 0.5f, 199f, 0, true, false));
			femmi.IsPositionFrozen = true;
			femmi.IsVisible = false;
			femmi.IsCollisionEnabled = false;
			TimeWeather.Meteo.SetMeteo((int)Weather.ExtraSunny, false, true);
			NetworkOverrideClockTime(Funzioni.GetRandomInt(0, 23), Funzioni.GetRandomInt(0, 59), Funzioni.GetRandomInt(0, 59));
			ShutdownLoadingScreenNui();
			await BaseScript.Delay(1000);
			ToggleMenu(true, "charloading");
		}
		private static void ToggleMenu(bool menuOpen, string menu)
		{
			if (menuOpen)
				Funzioni.SendNuiMessage(new { type = "toggleMenu", menuStatus = menuOpen, menu = menu, data = JsonConvert.SerializeObject(Game.Player.GetPlayerData().char_data) });
			DisplayHud(!menuOpen);
			DisplayRadar(!menuOpen);
			SetEnableHandcuffs(PlayerPedId(), menuOpen);
			SetNuiFocus(menuOpen, menuOpen);
		}

		static Ped p1;
		private static async void SelezionatoPreview(dynamic data)
		{
			Char_data pers = Game.Player.GetPlayerData().char_data.FirstOrDefault(x => x.id-1 == Convert.ToInt32(data.slot));
			if (p1 != null) p1.Delete();
			if (pers.skin.sex == "Maschio")
				p1 = await Funzioni.CreatePedLocally(new Model(PedHash.FreemodeMale01), Game.PlayerPed.Position + new Vector3(0, 0.5f, -1f));
			else
				p1 = await Funzioni.CreatePedLocally(new Model(PedHash.FreemodeFemale01), Game.PlayerPed.Position + new Vector3(0, 0.5f, -1f));
			p1.Style.SetDefaultClothes();
			SetSkinAndClothes(p1, pers);
			p1.IsPositionFrozen = true;
			p1.BlockPermanentEvents = true;
			string scena = scenari[Funzioni.GetRandomInt(scenari.Count)];
			p1.Task.StartScenario(scena, p1.Position);
			Client.Instance.AddTick(Controllo);
		}

		private static async void Selezionato(dynamic data)
		{
			guiEnabled = false;
			if (femmi != null) if (femmi.Exists()) femmi.Delete();
			if (p1 != null) if (p1.Exists()) p1.Delete();
			ToggleMenu(false, "close");
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
			Game.Player.GetPlayerData().char_current = Convert.ToInt32(data.slot) + 1;
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", Game.Player.GetPlayerData().char_current);
			Char_data Data = Game.Player.GetPlayerData().CurrentChar;
			Log.Printa(LogType.Debug, $"Slot = {Game.Player.GetPlayerData().char_current}, {Convert.ToInt32(data.slot) + 1}");
			Log.Printa(LogType.Debug, $"CharData = {JsonConvert.SerializeObject(Data)}");
			if (!Data.location.position.IsZero)
			{
				RequestCollisionAtCoord(Data.location.position.X, Data.location.position.Y, Data.location.position.Z);
				Game.PlayerPed.Position = Data.location.position;
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
			Game.PlayerPed.SetDecor("PlayerStanziato", false);
			Game.PlayerPed.IsVisible = true;
			Game.PlayerPed.IsCollisionEnabled = true;
			NetworkClearClockTimeOverride();
			AdvanceClockTimeTo(TimeWeather.Orario.h, TimeWeather.Orario.m, TimeWeather.Orario.s);
			await BaseScript.Delay(7000);
			Client.Instance.AddTick(TimeWeather.Orario.AggiornaTempo);
			BaseScript.TriggerServerEvent("changeWeatherForMe", true);
			await BaseScript.Delay(5000);
			if (Screen.LoadingPrompt.IsActive)
				Screen.LoadingPrompt.Hide();
			SwitchInPlayer(Game.PlayerPed.Handle);
			await BaseScript.Delay(1000);
			Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
			BaseScript.TriggerEvent("lprp:onPlayerSpawn");
			BaseScript.TriggerServerEvent("lprp:onPlayerSpawn");
			Client.Instance.RemoveTick(Controllo);
//			Client.Instance.RemoveTick(Scaleform);
//			Client.Instance.RemoveTick(TastiMenu);
		}

		private static void SetSkinAndClothes(Ped p, Char_data data)
		{
			SetPedHeadBlendData(p.Handle, data.skin.face.mom, data.skin.face.dad, 0, data.skin.face.mom, data.skin.face.dad, 0, data.skin.resemblance, data.skin.skinmix, 0f, false);
			SetPedHeadOverlay(p.Handle, 0, data.skin.blemishes.style, data.skin.blemishes.opacity);
			SetPedHeadOverlay(p.Handle, 1, data.skin.facialHair.beard.style, data.skin.facialHair.beard.opacity);
			SetPedHeadOverlayColor(p.Handle, 1, 1, data.skin.facialHair.beard.color[0], data.skin.facialHair.beard.color[1]);
			SetPedHeadOverlay(p.Handle, 2, data.skin.facialHair.eyebrow.style, data.skin.facialHair.eyebrow.opacity);
			SetPedHeadOverlayColor(p.Handle, 2, 1, data.skin.facialHair.eyebrow.color[0], data.skin.facialHair.eyebrow.color[1]);
			SetPedHeadOverlay(p.Handle, 3, data.skin.ageing.style, data.skin.ageing.opacity);
			SetPedHeadOverlay(p.Handle, 4, data.skin.makeup.style, data.skin.makeup.opacity);
			SetPedHeadOverlay(p.Handle, 5, data.skin.blusher.style, data.skin.blusher.opacity);
			SetPedHeadOverlayColor(p.Handle, 5, 2, data.skin.blusher.color[0], data.skin.blusher.color[1]);
			SetPedHeadOverlay(p.Handle, 6, data.skin.complexion.style, data.skin.complexion.opacity);
			SetPedHeadOverlay(p.Handle, 7, data.skin.skinDamage.style, data.skin.skinDamage.opacity);
			SetPedHeadOverlay(p.Handle, 8, data.skin.lipstick.style, data.skin.lipstick.opacity);
			SetPedHeadOverlayColor(p.Handle, 8, 2, data.skin.lipstick.color[0], data.skin.lipstick.color[1]);
			SetPedHeadOverlay(p.Handle, 9, data.skin.freckles.style, data.skin.freckles.opacity);
			SetPedEyeColor(p.Handle, data.skin.eye.style);
			SetPedComponentVariation(p.Handle, 2, data.skin.hair.style, 0, 0);
			SetPedHairColor(p.Handle, data.skin.hair.color[0], data.skin.hair.color[1]);
			SetPedPropIndex(p.Handle, 2, data.skin.ears.style, data.skin.ears.color, false);
			for (int i = 0; i < data.skin.face.tratti.Length; i++)
				SetPedFaceFeature(p.Handle, i, data.skin.face.tratti[i]);

			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Faccia, data.dressing.ComponentDrawables.Faccia, data.dressing.ComponentTextures.Faccia, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Maschera, data.dressing.ComponentDrawables.Maschera, data.dressing.ComponentTextures.Maschera, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Torso, data.dressing.ComponentDrawables.Torso, data.dressing.ComponentTextures.Torso, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Pantaloni, data.dressing.ComponentDrawables.Pantaloni, data.dressing.ComponentTextures.Pantaloni, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Borsa_Paracadute, data.dressing.ComponentDrawables.Borsa_Paracadute, data.dressing.ComponentTextures.Borsa_Paracadute, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Scarpe, data.dressing.ComponentDrawables.Scarpe, data.dressing.ComponentTextures.Scarpe, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Accessori, data.dressing.ComponentDrawables.Accessori, data.dressing.ComponentTextures.Accessori, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Sottomaglia, data.dressing.ComponentDrawables.Sottomaglia, data.dressing.ComponentTextures.Sottomaglia, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Kevlar, data.dressing.ComponentDrawables.Kevlar, data.dressing.ComponentTextures.Kevlar, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Badge, data.dressing.ComponentDrawables.Badge, data.dressing.ComponentTextures.Badge, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Torso_2, data.dressing.ComponentDrawables.Torso_2, data.dressing.ComponentTextures.Torso_2, 2);

			if (data.dressing.PropIndices.Cappelli_Maschere == -1)
				ClearPedProp(PlayerPedId(), 0);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Cappelli_Maschere, data.dressing.PropIndices.Cappelli_Maschere, data.dressing.PropTextures.Cappelli_Maschere, false);

			if (data.dressing.PropIndices.Orecchie == -1)
				ClearPedProp(PlayerPedId(), 2);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Orecchie, data.dressing.PropIndices.Orecchie, data.dressing.PropTextures.Orecchie, false);

			if (data.dressing.PropIndices.Occhiali_Occhi == -1)
				ClearPedProp(PlayerPedId(), 1);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Occhiali_Occhi, data.dressing.PropIndices.Occhiali_Occhi, data.dressing.PropTextures.Occhiali_Occhi, true);

			if (data.dressing.PropIndices.Unk_3 == -1)
				ClearPedProp(PlayerPedId(), 3);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_3, data.dressing.PropIndices.Unk_3, data.dressing.PropTextures.Unk_3, true);

			if (data.dressing.PropIndices.Unk_4 == -1)
				ClearPedProp(PlayerPedId(), 4);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_4, data.dressing.PropIndices.Unk_4, data.dressing.PropTextures.Unk_4, true);

			if (data.dressing.PropIndices.Unk_5 == -1)
				ClearPedProp(PlayerPedId(), 5);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_5, data.dressing.PropIndices.Unk_5, data.dressing.PropTextures.Unk_5, true);

			if (data.dressing.PropIndices.Orologi == -1)
				ClearPedProp(PlayerPedId(), 6);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Orologi, data.dressing.PropIndices.Orologi, data.dressing.PropTextures.Orologi, true);

			if (data.dressing.PropIndices.Bracciali == -1)
				ClearPedProp(PlayerPedId(), 7);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Bracciali, data.dressing.PropIndices.Bracciali, data.dressing.PropTextures.Bracciali, true);

			if (data.dressing.PropIndices.Unk_8 == -1)
				ClearPedProp(PlayerPedId(), 8);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_8, data.dressing.PropIndices.Unk_8, data.dressing.PropTextures.Unk_8, true);


		}

		private static void Disconnetti(dynamic data)
		{
			ToggleMenu(false, "close");
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
			ToggleMenu(false, "close");
			Menus.CharCreationMenu(data.nome, data.cogn, data.dob, data.sesso);
		}

		public static async void Scelta(string param)
		{
			if (param == "select")
				BaseScript.TriggerServerEvent("lprp:dropPlayer", "Grazie di essere passato da " + Client.Impostazioni.Main.NomeServer + "!");
			else
				ToggleMenu(true, "charloading");
		}

	}
}
