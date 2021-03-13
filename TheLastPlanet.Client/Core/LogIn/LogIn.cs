﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core.CharCreation;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Veicoli;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core.LogIn
{
	internal static class LogIn
	{
		public static bool GuiEnabled = false;
		public static List<Vector4> SelectFirstCoords = new()
		{
			new Vector4(-1503.000f, -1143.462f, 34.670f, 64.692f),
			new Vector4(747.339f, 525.837f, 345.395f, 39.975f),
			new Vector4(-2162.689f, -469.343f, 3.396f, 150.975f),
			new Vector4(-170.256f, -2357.418f, 100.596f, 95.975f),
			new Vector4(2126.171f, 3014.593f, 59.196f, 115.975f),
			new Vector4(-103.310f, -1215.578f, 53.796f, 270.975f),
			new Vector4(-3032.130f, 22.216f, 11.118f, 0f)
		};
		public static Vector4 charCreateCoords = new(402.91f, -996.74f, -100.00025f, 180.086f);
		public static Camera charSelectionCam;
		public static Camera charCreationCam;
		private static bool cambiato = false;

		private static List<string> scenari = new()
		{
			"WORLD_HUMAN_AA_SMOKE",
			"WORLD_HUMAN_BINOCULARS",
			"WORLD_HUMAN_BUM_FREEWAY",
			"WORLD_HUMAN_BUM_SLUMPED",
			"WORLD_HUMAN_BUM_STANDING",
			"WORLD_HUMAN_BUM_WASH",
			"WORLD_HUMAN_CAR_PARK_ATTENDANT",
			"WORLD_HUMAN_CHEERING",
			"WORLD_HUMAN_CLIPBOARD",
			"WORLD_HUMAN_CONST_DRILL",
			"WORLD_HUMAN_COP_IDLES",
			"WORLD_HUMAN_DRINKING",
			"WORLD_HUMAN_DRUG_DEALER",
			"WORLD_HUMAN_DRUG_DEALER_HARD",
			"WORLD_HUMAN_MOBILE_FILM_SHOCKING",
			"WORLD_HUMAN_GARDENER_LEAF_BLOWER",
			"WORLD_HUMAN_GARDENER_PLANT",
			"WORLD_HUMAN_GOLF_PLAYER",
			"WORLD_HUMAN_GUARD_PATROL",
			"WORLD_HUMAN_GUARD_STAND",
			"WORLD_HUMAN_GUARD_STAND_ARMY",
			"WORLD_HUMAN_HAMMERING",
			"WORLD_HUMAN_HANG_OUT_STREET",
			"WORLD_HUMAN_HUMAN_STATUE",
			"WORLD_HUMAN_JANITOR",
			"WORLD_HUMAN_JOG_STANDING",
			"WORLD_HUMAN_LEANING",
			"WORLD_HUMAN_MAID_CLEAN",
			"WORLD_HUMAN_MUSCLE_FLEX",
			"WORLD_HUMAN_MUSCLE_FREE_WEIGHTS",
			"WORLD_HUMAN_MUSICIAN",
			"WORLD_HUMAN_PAPARAZZI",
			"WORLD_HUMAN_PARTYING",
			"WORLD_HUMAN_PROSTITUTE_HIGH_CLASS",
			"WORLD_HUMAN_PUSH_UPS",
			"WORLD_HUMAN_SECURITY_SHINE_TORCH",
			"WORLD_HUMAN_SIT_UPS",
			"WORLD_HUMAN_SMOKING",
			"WORLD_HUMAN_SMOKING_POT",
			"WORLD_HUMAN_STAND_FISHING",
			"WORLD_HUMAN_STAND_MOBILE",
			"WORLD_HUMAN_STRIP_WATCH_STAND",
			"WORLD_HUMAN_TENNIS_PLAYER",
			"WORLD_HUMAN_TOURIST_MAP",
			"WORLD_HUMAN_TOURIST_MOBILE",
			"WORLD_HUMAN_WELDING",
			"WORLD_HUMAN_YOGA",
			"CODE_HUMAN_MEDIC_KNEEL",
			"CODE_HUMAN_MEDIC_TEND_TO_DEAD",
			"CODE_HUMAN_MEDIC_TIME_OF_DEATH"
		};

		private static Ped dummyPed;

		public static void Init()
		{
			ClearFocus();
			Client.Instance.AddTick(Entra);
			Client.Instance.RegisterNuiEventHandler("back-indietro", new Action<CallbackDelegate>((cb) =>
			{
				ToggleMenu(true, "charloading");
				cb("ok");
			}));
			Client.Instance.RegisterNuiEventHandler("previewChar", new Action<IDictionary<string, object>, CallbackDelegate>(SelezionatoPreview));
			Client.Instance.RegisterNuiEventHandler("char-select", new Action<IDictionary<string, object>, CallbackDelegate>(Selezionato));
			Client.Instance.RegisterNuiEventHandler("disconnect", new Action<IDictionary<string, object>, CallbackDelegate>(Disconnetti));
			Client.Instance.RegisterNuiEventHandler("new-character", new Action<IDictionary<string, object>, CallbackDelegate>(NuovoPersonaggio));
			Client.Instance.AddEventHandler("lprp:sceltaCharSelect", new Action<string>(Scelta));
			Client.Instance.AddEventHandler("playerSpawned", new Action(PlayerSpawned));
			RequestModel((uint)PedHash.FreemodeMale01);
			RequestModel((uint)PedHash.FreemodeFemale01);
			Screen.Hud.IsRadarVisible = false;
		}

		#region INGRESSO NEL SERVER

		private static async Task Entra()
		{
			if (NetworkIsSessionStarted())
			{
				BaseScript.TriggerEvent("playerSpawned");
				Client.Instance.RemoveTick(Entra);
			}
		}

		private static async void PlayerSpawned()
		{
			Screen.Fading.FadeOut(800);
			while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
			await CachePlayer.Cache.InitPlayer();
			while (!NetworkIsPlayerActive(CachePlayer.Cache.MyPlayer.Player.Handle)) await BaseScript.Delay(500);
			BaseScript.TriggerServerEvent("lprp:coda: playerConnected");
			Funzioni.SendNuiMessage(new { resname = GetCurrentResourceName() });
			await CachePlayer.Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			CachePlayer.Cache.MyPlayer.UpdatePedId();
			CachePlayer.Cache.MyPlayer.Ped.IsVisible = false;
			CachePlayer.Cache.MyPlayer.Ped.IsPositionFrozen = true;
			CachePlayer.Cache.MyPlayer.Player.IgnoredByPolice = true;
			CachePlayer.Cache.MyPlayer.Player.DispatchsCops = false;
			NetworkSetTalkerProximity(-1000f);
			Screen.Hud.IsRadarVisible = false;
			CharSelect();
		}

		public static async void CharSelect()
		{
			CachePlayer.Cache.MyPlayer.Player.CanControlCharacter = false;
			if (CachePlayer.Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(CachePlayer.Cache.MyPlayer.Ped.Handle, true, false);
			var charSelectCoords = SelectFirstCoords[Funzioni.GetRandomInt(SelectFirstCoords.Count - 1)];
			RequestCollisionAtCoord(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z);
			CachePlayer.Cache.MyPlayer.Ped.Position = new Vector3(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z - 1);
			CachePlayer.Cache.MyPlayer.Ped.Heading = charSelectCoords.W;
			await CachePlayer.Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			CachePlayer.Cache.MyPlayer.UpdatePedId();
			CachePlayer.Cache.MyPlayer.Ped.Style.SetDefaultClothes();
			while (!await CachePlayer.Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01))) await BaseScript.Delay(50);
			CachePlayer.Cache.MyPlayer.UpdatePedId();

			if (CachePlayer.Cache.MyPlayer.Ped.Model == new Model(PedHash.FreemodeMale01))
			{
				var p = CachePlayer.Cache.MyPlayer.Ped;
				p.Style.SetDefaultClothes();
				p.SetDecor("TheLastPlanet2019fighissimo!yeah!", p.Handle);
				await CachePlayer.Cache.Loaded();
				CachePlayer.Cache.MyPlayer.User.StatiPlayer.Istanza.Istanzia("Ingresso");
				await BaseScript.Delay(100);
				CachePlayer.Cache.MyPlayer.Player.State.Set("Pausa", new { Attivo = false }, true);
				p.IsVisible = false;
				p.IsPositionFrozen = true;
				RequestCollisionAtCoord(charCreateCoords.X, charCreateCoords.Y, charCreateCoords.Z - 1);
				charSelectionCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
				SetGameplayCamRelativeHeading(0);
				charSelectionCam.Position = GetOffsetFromEntityInWorldCoords(p.Handle, 0f, -2, 0);
				charSelectionCam.PointAt(p);
				charSelectionCam.IsActive = true;
				RenderScriptCams(true, false, 0, false, false);
				Attiva();
			}
			else
			{
				CharSelect();
			}
		}

		public static async void CharCreate()
		{
			NetworkFadeInEntity(CachePlayer.Cache.MyPlayer.Ped.Handle, true);
			RequestCollisionAtCoord(charCreateCoords.X, charCreateCoords.Y, charCreateCoords.Z - 1);
			SetEntityCoords(CachePlayer.Cache.MyPlayer.Ped.Handle, charCreateCoords.X, charCreateCoords.Y, charCreateCoords.Z - 1, false, false, false, false);
			SetEntityHeading(CachePlayer.Cache.MyPlayer.Ped.Handle, charCreateCoords.W);
			Vector3 h = GetPedBoneCoords(CachePlayer.Cache.MyPlayer.Ped.Handle, 24818, 0.0f, 0.0f, 0.0f);
			Vector3 offCoords = GetOffsetFromEntityInWorldCoords(CachePlayer.Cache.MyPlayer.Ped.Handle, 0.0f, 2.0f, 0.8f);
			charCreationCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { Position = new Vector3(offCoords.X, offCoords.Y, h.Z + 0.2f) };
			charCreationCam.PointAt(h);
			charCreationCam.IsActive = true;
			RenderScriptCams(true, false, 0, false, false);
			await Task.FromResult(0);
		}

		#endregion

		public static async void Attiva()
		{
			dummyPed = await Funzioni.CreatePedLocally(PedHash.FreemodeFemale01, CachePlayer.Cache.MyPlayer.Ped.Position + new Vector3(10));
			dummyPed.IsVisible = false;
			dummyPed.IsPositionFrozen = false;
			dummyPed.IsCollisionEnabled = false;
			dummyPed.IsCollisionProof = false;
			dummyPed.BlockPermanentEvents = true;
			GuiEnabled = true;
			TimeWeather.Meteo.SetMeteo((int)Weather.ExtraSunny, false, true);
			NetworkOverrideClockTime(Funzioni.GetRandomInt(0, 23), Funzioni.GetRandomInt(0, 59), Funzioni.GetRandomInt(0, 59));
			ShutdownLoadingScreen();
			ShutdownLoadingScreenNui();
			Screen.Fading.FadeIn(1000);
			await BaseScript.Delay(1000);
			ToggleMenu(true, "charloading");
			await CachePlayer.Cache.Loaded();
			Client.Instance.AddTick(Main.AFK);
		}

		private static void ToggleMenu(bool menuOpen, string menu = "")
		{
			Funzioni.SendNuiMessage(new { type = "toggleMenu", menuStatus = menuOpen, menu, data = CachePlayer.Cache.MyPlayer.User.Characters.SerializeToJson() });
			SetNuiFocus(menuOpen, menuOpen);
			DisplayHud(!menuOpen);
			SetEnableHandcuffs(CachePlayer.Cache.MyPlayer.Ped.Handle, menuOpen);
		}

		private static Ped p1;

		private static async void SelezionatoPreview(IDictionary<string, object> data, CallbackDelegate cb)
		{
			cambiato = false;
			PedHash m = PedHash.FreemodeMale01;
			PedHash f = PedHash.FreemodeFemale01;
			Ped ped = CachePlayer.Cache.MyPlayer.Ped;
			Char_data pers = CachePlayer.Cache.MyPlayer.User.Characters.FirstOrDefault(x => x.id == (int)data["id"]);

			if (p1 != null)
			{
				Client.Instance.RemoveTick(Controllo);
				p1.Delete();
			}

			p1 = await Funzioni.CreatePedLocally(pers.skin.sex == "Maschio" ? m : f, ped.Position + new Vector3(0, 0.5f, -1f));
			p1.IsPositionFrozen = true;
			p1.BlockPermanentEvents = true;
			SetEntityAlpha(p1.Handle, 0, 0);
			await SetSkinAndClothes(p1, pers);
			while (!cambiato) await BaseScript.Delay(1000);
			string scena = scenari[Funzioni.GetRandomInt(scenari.Count)];
			p1.Task.StartScenario(scena, p1.Position);
			Client.Instance.AddTick(Controllo);
			int i = 0;

			while (i < 255)
			{
				await BaseScript.Delay(25);
				SetEntityAlpha(p1.Handle, i, 0);
				i += 25;
			}

			cb("ok");
		}

		private static async void Selezionato(IDictionary<string, object> data, CallbackDelegate cb)
		{
			if (p1 != null)
				if (p1.Exists())
					p1.Delete();
			if (dummyPed != null)
				if (dummyPed.Exists())
					dummyPed.Delete();
			GuiEnabled = false;
			ToggleMenu(false);
			Screen.Fading.FadeOut(800);
			await BaseScript.Delay(1000);
			HUD.MenuPool.CloseAllMenus();
			Screen.LoadingPrompt.Show("Caricamento", LoadingSpinnerType.Clockwise1);
			await BaseScript.Delay(3000);
			CachePlayer.Cache.MyPlayer.User.char_current = Convert.ToUInt32(data["id"]);
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", CachePlayer.Cache.MyPlayer.User.char_current);
			var Data = CachePlayer.Cache.MyPlayer.User.CurrentChar;
			var switchType = !Data.location.position.IsZero ? GetIdealPlayerSwitchType(CachePlayer.Cache.MyPlayer.Ped.Position.X, CachePlayer.Cache.MyPlayer.Ped.Position.Y, CachePlayer.Cache.MyPlayer.Ped.Position.Z, Data.location.position.X, Data.location.position.Y, Data.location.position.Z) : GetIdealPlayerSwitchType(CachePlayer.Cache.MyPlayer.Ped.Position.X, CachePlayer.Cache.MyPlayer.Ped.Position.Y, CachePlayer.Cache.MyPlayer.Ped.Position.Z, Main.firstSpawnCoords.X, Main.firstSpawnCoords.Y, Main.firstSpawnCoords.Z);
			SwitchOutPlayer(PlayerPedId(), 1 | 32 | 128 | 16384, switchType);
			DestroyAllCams(true);
			EnableGameplayCam(true);
			await BaseScript.Delay(5000);
			RenderScriptCams(false, false, 0, false, false);
			StatSetInt(Funzioni.HashUint("MP0_WALLET_BALANCE"), CachePlayer.Cache.MyPlayer.User.Money, true);
			StatSetInt(Funzioni.HashUint("BANK_BALANCE"), CachePlayer.Cache.MyPlayer.User.DirtyMoney, true);
			await BaseScript.Delay(6000);
			Screen.Fading.FadeIn(800);
			await BaseScript.Delay(4000);
			CachePlayer.Cache.MyPlayer.Ped.IsInvincible = false;
			await BaseScript.Delay(1000);
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Screen.LoadingPrompt.Show("Caricamento personaggio", LoadingSpinnerType.Clockwise1);

			if (Data.location.position.IsZero)
			{
				CachePlayer.Cache.MyPlayer.Ped.Position = Main.firstSpawnCoords.ToVector3();
				CachePlayer.Cache.MyPlayer.Ped.Heading = Main.firstSpawnCoords.W;
				await BaseScript.Delay(2000);
			}
			else
			{
				CachePlayer.Cache.MyPlayer.Ped.Position = Data.location.position;
				CachePlayer.Cache.MyPlayer.Ped.Heading = Data.location.h;
				await BaseScript.Delay(2000);
			}

			Eventi.LoadModel();
			if (CachePlayer.Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerPedId(), true, false);
			CachePlayer.Cache.MyPlayer.User.StatiPlayer.Istanza.RimuoviIstanza();
			CachePlayer.Cache.MyPlayer.Ped.SetDecor("TheLastPlanet2019fighissimo!yeah!", CachePlayer.Cache.MyPlayer.Ped.Handle);
			CachePlayer.Cache.MyPlayer.User.StatiPlayer.Istanza.Istanzia("Ingresso");
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Screen.LoadingPrompt.Show("Sincronizzazione col server", LoadingSpinnerType.Clockwise1);
			NetworkClearClockTimeOverride();
			AdvanceClockTimeTo(TimeWeather.Orario.h, TimeWeather.Orario.m, TimeWeather.Orario.s);
			if (CachePlayer.Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(CachePlayer.Cache.MyPlayer.Ped.Handle, true, false);
			await BaseScript.Delay(7000);
			Client.Instance.AddTick(TimeWeather.Orario.AggiornaTempo);
			BaseScript.TriggerServerEvent("changeWeatherForMe", true);
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Screen.LoadingPrompt.Show("Applicazione impostazioni personalizzate", LoadingSpinnerType.RegularClockwise);
			await BaseScript.Delay(5000);
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Screen.LoadingPrompt.Show("Ingresso nel server", LoadingSpinnerType.RegularClockwise);
			CachePlayer.Cache.MyPlayer.User.CurrentChar.Veicoli = await Client.Instance.Eventi.Request<List<OwnedVehicle>>("lprp:caricaVeicoli");
			//EnableSwitchPauseBeforeDescent();
			SwitchInPlayer(CachePlayer.Cache.MyPlayer.Ped.Handle);
			var pos = await CachePlayer.Cache.MyPlayer.Ped.Position.GetVector3WithGroundZ();
			CachePlayer.Cache.MyPlayer.Ped.Position = pos;
			while (IsPlayerSwitchInProgress()) await BaseScript.Delay(0);
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Client.Instance.RemoveTick(Controllo);
			if (CachePlayer.Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(CachePlayer.Cache.MyPlayer.Ped.Handle, true, false);
			await BaseScript.Delay(1000);
			CachePlayer.Cache.MyPlayer.Ped.IsPositionFrozen = false;
			CachePlayer.Cache.MyPlayer.Ped.Weapons.Select(WeaponHash.Unarmed);
			BaseScript.TriggerEvent("lprp:onPlayerSpawn");
			BaseScript.TriggerServerEvent("lprp:onPlayerSpawn");
			ClearFocus();
			NetworkFadeInEntity(CachePlayer.Cache.MyPlayer.Ped.Handle, true);
			CachePlayer.Cache.MyPlayer.Ped.IsVisible = true;
			CachePlayer.Cache.MyPlayer.Ped.IsCollisionEnabled = true;
			//			Client.Instance.RemoveTick(Scaleform);
			//			Client.Instance.RemoveTick(TastiMenu);
			CachePlayer.Cache.MyPlayer.Player.CanControlCharacter = true;
			cb("ok");
		}

		public static async Task SetSkinAndClothes(Ped p, Char_data data)
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
			for (int i = 0; i < data.skin.face.tratti.Length; i++) SetPedFaceFeature(p.Handle, i, data.skin.face.tratti[i]);
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
				ClearPedProp(p.Handle, 0);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Cappelli_Maschere, data.dressing.PropIndices.Cappelli_Maschere, data.dressing.PropTextures.Cappelli_Maschere, false);
			if (data.dressing.PropIndices.Orecchie == -1)
				ClearPedProp(p.Handle, 2);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Orecchie, data.dressing.PropIndices.Orecchie, data.dressing.PropTextures.Orecchie, false);
			if (data.dressing.PropIndices.Occhiali_Occhi == -1)
				ClearPedProp(p.Handle, 1);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Occhiali_Occhi, data.dressing.PropIndices.Occhiali_Occhi, data.dressing.PropTextures.Occhiali_Occhi, true);
			if (data.dressing.PropIndices.Unk_3 == -1)
				ClearPedProp(p.Handle, 3);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_3, data.dressing.PropIndices.Unk_3, data.dressing.PropTextures.Unk_3, true);
			if (data.dressing.PropIndices.Unk_4 == -1)
				ClearPedProp(p.Handle, 4);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_4, data.dressing.PropIndices.Unk_4, data.dressing.PropTextures.Unk_4, true);
			if (data.dressing.PropIndices.Unk_5 == -1)
				ClearPedProp(p.Handle, 5);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_5, data.dressing.PropIndices.Unk_5, data.dressing.PropTextures.Unk_5, true);
			if (data.dressing.PropIndices.Orologi == -1)
				ClearPedProp(p.Handle, 6);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Orologi, data.dressing.PropIndices.Orologi, data.dressing.PropTextures.Orologi, true);
			if (data.dressing.PropIndices.Bracciali == -1)
				ClearPedProp(p.Handle, 7);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Bracciali, data.dressing.PropIndices.Bracciali, data.dressing.PropTextures.Bracciali, true);
			if (data.dressing.PropIndices.Unk_8 == -1)
				ClearPedProp(p.Handle, 8);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_8, data.dressing.PropIndices.Unk_8, data.dressing.PropTextures.Unk_8, true);
			while (GetPedDrawableVariation(p.Handle, (int)DrawableIndexes.Torso_2) != data.dressing.ComponentDrawables.Torso_2) await BaseScript.Delay(0);
			cambiato = true;
			await Task.FromResult(0);
		}

		private static void Disconnetti(IDictionary<string, object> data, CallbackDelegate cb)
		{
			GuiEnabled = false;
			ToggleMenu(false);
			BaseScript.TriggerEvent("lprp:manager:warningMessage", "Stai uscendo dal gioco senza aver selezionato un personaggio", "Sei sicuro?", 16392, "lprp:sceltaCharSelect");
			cb("ok");
		}

		private static async Task Controllo()
		{
			if (p1.Exists()) p1.Heading += 1.2f;
		}

		private static async void NuovoPersonaggio(IDictionary<string, object> data, CallbackDelegate cb)
		{
			Screen.Fading.FadeOut(800);
			await BaseScript.Delay(1000);
			GuiEnabled = false;
			ToggleMenu(false);
			Creator.CharCreationMenu(data);
			cb("ok");
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