using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Client.RolePlay.Core.CharCreation;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Veicoli;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.RolePlay.Core.LogIn
{
	internal static class NewLogIn
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
			Client.Instance.NuiManager.RegisterCallback("chars:preview", new Action<string>(SelezionatoPreview));
			Client.Instance.NuiManager.RegisterCallback("chars:select", new Action<string>(Selezionato));
			Client.Instance.NuiManager.RegisterCallback("chars:disconnect", Disconnetti);
			Client.Instance.NuiManager.RegisterCallback("chars:new", new Action<NewChar>(NuovoPersonaggio));
			Client.Instance.AddEventHandler("lprp:sceltaCharSelect", new Action<string>(Scelta));
			RequestModel((uint)PedHash.FreemodeMale01);
			RequestModel((uint)PedHash.FreemodeFemale01);
			Screen.Hud.IsRadarVisible = false;
		}

		#region INGRESSO NEL SERVER

		private static async Task Entra()
		{
			if (NetworkIsSessionStarted())
			{
				PlayerSpawned();
				Client.Instance.RemoveTick(Entra);
			}
		}

		private static async void PlayerSpawned()
		{
			Screen.Fading.FadeOut(800);
			while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
			await Cache.InitPlayer();
			while (!NetworkIsPlayerActive(Cache.MyPlayer.Player.Handle)) await BaseScript.Delay(0);
			BaseScript.TriggerServerEvent("lprp:coda: playerConnected");
			Client.Instance.NuiManager.SendMessage(new { resname = GetCurrentResourceName() });
			await Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			Cache.MyPlayer.UpdatePedId();
			Cache.MyPlayer.Ped.IsVisible = false;
			Cache.MyPlayer.Ped.IsPositionFrozen = true;
			Cache.MyPlayer.Player.IgnoredByPolice = true;
			Cache.MyPlayer.Player.DispatchsCops = false;
			NetworkSetTalkerProximity(-1000f);
			Screen.Hud.IsRadarVisible = false;
			CharSelect();
		}

		public static async void CharSelect()
		{
			Cache.MyPlayer.Player.CanControlCharacter = false;
			if (Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.MyPlayer.Ped.Handle, true, false);
			Vector4 charSelectCoords = SelectFirstCoords[Funzioni.GetRandomInt(SelectFirstCoords.Count - 1)];
			RequestCollisionAtCoord(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z);
			Cache.MyPlayer.Ped.Position = new Vector3(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z - 1);
			Cache.MyPlayer.Ped.Heading = charSelectCoords.W;
			await Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			Cache.MyPlayer.UpdatePedId();
			Cache.MyPlayer.Ped.Style.SetDefaultClothes();
			while (!await Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01))) await BaseScript.Delay(50);
			Cache.MyPlayer.UpdatePedId();

			if (Cache.MyPlayer.Ped.Model == new Model(PedHash.FreemodeMale01))
			{
				Ped p = Cache.MyPlayer.Ped;
				p.Style.SetDefaultClothes();
				p.SetDecor("TheLastPlanet2019fighissimo!yeah!", p.Handle);
				await Cache.Loaded();
				Cache.MyPlayer.User.StatiPlayer.Istanza.Istanzia("Ingresso");
				await BaseScript.Delay(100);
				Cache.MyPlayer.Player.State.Set("Pausa", new { Attivo = false }, true);
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

		#endregion

		public static async void Attiva()
		{
			dummyPed = await Funzioni.CreatePedLocally(PedHash.FreemodeFemale01, Cache.MyPlayer.Ped.Position + new Vector3(10));
			dummyPed.IsVisible = false;
			dummyPed.IsPositionFrozen = false;
			dummyPed.IsCollisionEnabled = false;
			dummyPed.IsCollisionProof = false;
			dummyPed.BlockPermanentEvents = true;
			GuiEnabled = true;
			TimeWeather.Meteo.SetMeteo((int)Weather.ExtraSunny, false, true);
			NetworkOverrideClockTime(Funzioni.GetRandomInt(0, 23), Funzioni.GetRandomInt(0, 59), Funzioni.GetRandomInt(0, 59));
			await Cache.Loaded();
			List<LogInInfo> data = await Client.Instance.Eventi.Get<List<LogInInfo>>("lprp:RequestLoginInfo", Cache.MyPlayer.User.ID);
			ToggleMenu(true, "charloading", data);
			ShutdownLoadingScreen();
			ShutdownLoadingScreenNui();
			Screen.Fading.FadeIn(1000);
			Client.Instance.AddTick(Main.AFK);
		}

		private static void ToggleMenu(bool menuOpen, string menu = "", List<LogInInfo> data = null)
		{
			data ??= new List<LogInInfo>();
			Client.Instance.NuiManager.SendMessage("chars:toggleMenu", new { hidden = !menuOpen, menuStatus = menuOpen, menu, data });
			Client.Instance.NuiManager.SetFocus(menuOpen, menuOpen);
			DisplayHud(!menuOpen);
			SetEnableHandcuffs(Cache.MyPlayer.Ped.Handle, menuOpen);
		}

		private static Ped p1;

		private static async void SelezionatoPreview(string data)
		{
			cambiato = false;
			PedHash m = PedHash.FreemodeMale01;
			PedHash f = PedHash.FreemodeFemale01;
			Ped ped = Cache.MyPlayer.Ped;
			SkinAndDress pers = await Client.Instance.Eventi.Get<SkinAndDress>("lprp:anteprimaChar", Convert.ToUInt64(data));

			if (p1 != null)
			{
				Client.Instance.RemoveTick(Controllo);
				p1.Delete();
			}

			p1 = await Funzioni.CreatePedLocally(pers.Skin.sex == "Maschio" ? m : f, ped.Position + new Vector3(0, 0.5f, -1f));
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
		}

		private static async void Selezionato(string data)
		{
			ulong ID = Convert.ToUInt64(data);
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
			/*
			Cache.MyPlayer.User.char_current = Convert.ToUInt32(data["id"]);
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", Cache.MyPlayer.User.char_current);
			*/
			Cache.MyPlayer.User.CurrentChar = await Client.Instance.Eventi.Get<Char_data>("lprp:Select_Char", ID);
			Char_data Data = Cache.MyPlayer.User.CurrentChar;
			int switchType = !Data.Posizione.IsZero ? GetIdealPlayerSwitchType(Cache.MyPlayer.Ped.Position.X, Cache.MyPlayer.Ped.Position.Y, Cache.MyPlayer.Ped.Position.Z, Data.Posizione.X, Data.Posizione.Y, Data.Posizione.Z) : GetIdealPlayerSwitchType(Cache.MyPlayer.Ped.Position.X, Cache.MyPlayer.Ped.Position.Y, Cache.MyPlayer.Ped.Position.Z, Main.firstSpawnCoords.X, Main.firstSpawnCoords.Y, Main.firstSpawnCoords.Z);
			SwitchOutPlayer(PlayerPedId(), 1 | 32 | 128 | 16384, switchType);
			DestroyAllCams(true);
			EnableGameplayCam(true);
			await BaseScript.Delay(5000);
			RenderScriptCams(false, false, 0, false, false);
			StatSetInt(Funzioni.HashUint("MP0_WALLET_BALANCE"), Cache.MyPlayer.User.Money, true);
			StatSetInt(Funzioni.HashUint("BANK_BALANCE"), Cache.MyPlayer.User.DirtyCash, true);
			await BaseScript.Delay(6000);
			Screen.Fading.FadeIn(800);
			await BaseScript.Delay(4000);
			Cache.MyPlayer.Ped.IsInvincible = false;
			await BaseScript.Delay(1000);
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Screen.LoadingPrompt.Show("Caricamento personaggio", LoadingSpinnerType.Clockwise1);

			if (Data.Posizione.IsZero)
			{
				Cache.MyPlayer.Ped.Position = Main.firstSpawnCoords.ToVector3();
				Cache.MyPlayer.Ped.Heading = Main.firstSpawnCoords.W;
				await BaseScript.Delay(2000);
			}
			else
			{
				Cache.MyPlayer.Ped.Position = Data.Posizione.ToVector3;
				Cache.MyPlayer.Ped.Heading = Data.Posizione.Heading;
				await BaseScript.Delay(2000);
			}

			Eventi.LoadModel();
			if (Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerPedId(), true, false);
			Cache.MyPlayer.User.StatiPlayer.Istanza.RimuoviIstanza();
			Cache.MyPlayer.Ped.SetDecor("TheLastPlanet2019fighissimo!yeah!", Cache.MyPlayer.Ped.Handle);
			Cache.MyPlayer.User.StatiPlayer.Istanza.Istanzia("Ingresso");
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Screen.LoadingPrompt.Show("Sincronizzazione col server", LoadingSpinnerType.Clockwise1);
			NetworkClearClockTimeOverride();
			AdvanceClockTimeTo(TimeWeather.Orario.h, TimeWeather.Orario.m, TimeWeather.Orario.s);
			if (Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.MyPlayer.Ped.Handle, true, false);
			await BaseScript.Delay(7000);
			Client.Instance.AddTick(TimeWeather.Orario.AggiornaTempo);
			BaseScript.TriggerServerEvent("changeWeatherForMe", true);
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Screen.LoadingPrompt.Show("Applicazione impostazioni personalizzate", LoadingSpinnerType.RegularClockwise);
			await BaseScript.Delay(5000);
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Screen.LoadingPrompt.Show("Ingresso nel server", LoadingSpinnerType.RegularClockwise);
			Cache.MyPlayer.User.CurrentChar.Veicoli = await Client.Instance.Eventi.Get<List<OwnedVehicle>>("lprp:caricaVeicoli", Data.CharID);
			//EnableSwitchPauseBeforeDescent();
			SwitchInPlayer(Cache.MyPlayer.Ped.Handle);
			Position pos = await Data.Posizione.FindGroundZ();
			Cache.MyPlayer.Ped.Position = pos.ToVector3;
			while (IsPlayerSwitchInProgress()) await BaseScript.Delay(1000);
			if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
			Client.Instance.RemoveTick(Controllo);
			if (Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.MyPlayer.Ped.Handle, true, false);
			await BaseScript.Delay(1000);
			Cache.MyPlayer.Ped.IsPositionFrozen = false;
			Cache.MyPlayer.Ped.Weapons.Select(WeaponHash.Unarmed);
			Cache.MyPlayer.User.status.Spawned = true;
			BaseScript.TriggerEvent("lprp:onPlayerSpawn");
			BaseScript.TriggerServerEvent("lprp:onPlayerSpawn");
			ClearFocus();
			NetworkFadeInEntity(Cache.MyPlayer.Ped.Handle, true);
			Cache.MyPlayer.Ped.IsVisible = true;
			Cache.MyPlayer.Ped.IsCollisionEnabled = true;
			//			Client.Instance.RemoveTick(Scaleform);
			//			Client.Instance.RemoveTick(TastiMenu);
			Cache.MyPlayer.Player.CanControlCharacter = true;
		}

		public static async Task SetSkinAndClothes(Ped p, SkinAndDress data)
		{
			SetPedHeadBlendData(p.Handle, data.Skin.face.mom, data.Skin.face.dad, 0, data.Skin.face.mom, data.Skin.face.dad, 0, data.Skin.resemblance, data.Skin.skinmix, 0f, false);
			SetPedHeadOverlay(p.Handle, 0, data.Skin.blemishes.style, data.Skin.blemishes.opacity);
			SetPedHeadOverlay(p.Handle, 1, data.Skin.facialHair.beard.style, data.Skin.facialHair.beard.opacity);
			SetPedHeadOverlayColor(p.Handle, 1, 1, data.Skin.facialHair.beard.color[0], data.Skin.facialHair.beard.color[1]);
			SetPedHeadOverlay(p.Handle, 2, data.Skin.facialHair.eyebrow.style, data.Skin.facialHair.eyebrow.opacity);
			SetPedHeadOverlayColor(p.Handle, 2, 1, data.Skin.facialHair.eyebrow.color[0], data.Skin.facialHair.eyebrow.color[1]);
			SetPedHeadOverlay(p.Handle, 3, data.Skin.ageing.style, data.Skin.ageing.opacity);
			SetPedHeadOverlay(p.Handle, 4, data.Skin.makeup.style, data.Skin.makeup.opacity);
			SetPedHeadOverlay(p.Handle, 5, data.Skin.blusher.style, data.Skin.blusher.opacity);
			SetPedHeadOverlayColor(p.Handle, 5, 2, data.Skin.blusher.color[0], data.Skin.blusher.color[1]);
			SetPedHeadOverlay(p.Handle, 6, data.Skin.complexion.style, data.Skin.complexion.opacity);
			SetPedHeadOverlay(p.Handle, 7, data.Skin.skinDamage.style, data.Skin.skinDamage.opacity);
			SetPedHeadOverlay(p.Handle, 8, data.Skin.lipstick.style, data.Skin.lipstick.opacity);
			SetPedHeadOverlayColor(p.Handle, 8, 2, data.Skin.lipstick.color[0], data.Skin.lipstick.color[1]);
			SetPedHeadOverlay(p.Handle, 9, data.Skin.freckles.style, data.Skin.freckles.opacity);
			SetPedEyeColor(p.Handle, data.Skin.eye.style);
			SetPedComponentVariation(p.Handle, 2, data.Skin.hair.style, 0, 0);
			SetPedHairColor(p.Handle, data.Skin.hair.color[0], data.Skin.hair.color[1]);
			SetPedPropIndex(p.Handle, 2, data.Skin.ears.style, data.Skin.ears.color, false);
			for (int i = 0; i < data.Skin.face.tratti.Length; i++) SetPedFaceFeature(p.Handle, i, data.Skin.face.tratti[i]);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Faccia, data.Dressing.ComponentDrawables.Faccia, data.Dressing.ComponentTextures.Faccia, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Maschera, data.Dressing.ComponentDrawables.Maschera, data.Dressing.ComponentTextures.Maschera, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Torso, data.Dressing.ComponentDrawables.Torso, data.Dressing.ComponentTextures.Torso, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Pantaloni, data.Dressing.ComponentDrawables.Pantaloni, data.Dressing.ComponentTextures.Pantaloni, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Borsa_Paracadute, data.Dressing.ComponentDrawables.Borsa_Paracadute, data.Dressing.ComponentTextures.Borsa_Paracadute, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Scarpe, data.Dressing.ComponentDrawables.Scarpe, data.Dressing.ComponentTextures.Scarpe, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Accessori, data.Dressing.ComponentDrawables.Accessori, data.Dressing.ComponentTextures.Accessori, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Sottomaglia, data.Dressing.ComponentDrawables.Sottomaglia, data.Dressing.ComponentTextures.Sottomaglia, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Kevlar, data.Dressing.ComponentDrawables.Kevlar, data.Dressing.ComponentTextures.Kevlar, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Badge, data.Dressing.ComponentDrawables.Badge, data.Dressing.ComponentTextures.Badge, 2);
			SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Torso_2, data.Dressing.ComponentDrawables.Torso_2, data.Dressing.ComponentTextures.Torso_2, 2);
			if (data.Dressing.PropIndices.Cappelli_Maschere == -1)
				ClearPedProp(p.Handle, 0);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Cappelli_Maschere, data.Dressing.PropIndices.Cappelli_Maschere, data.Dressing.PropTextures.Cappelli_Maschere, false);
			if (data.Dressing.PropIndices.Orecchie == -1)
				ClearPedProp(p.Handle, 2);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Orecchie, data.Dressing.PropIndices.Orecchie, data.Dressing.PropTextures.Orecchie, false);
			if (data.Dressing.PropIndices.Occhiali_Occhi == -1)
				ClearPedProp(p.Handle, 1);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Occhiali_Occhi, data.Dressing.PropIndices.Occhiali_Occhi, data.Dressing.PropTextures.Occhiali_Occhi, true);
			if (data.Dressing.PropIndices.Unk_3 == -1)
				ClearPedProp(p.Handle, 3);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_3, data.Dressing.PropIndices.Unk_3, data.Dressing.PropTextures.Unk_3, true);
			if (data.Dressing.PropIndices.Unk_4 == -1)
				ClearPedProp(p.Handle, 4);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_4, data.Dressing.PropIndices.Unk_4, data.Dressing.PropTextures.Unk_4, true);
			if (data.Dressing.PropIndices.Unk_5 == -1)
				ClearPedProp(p.Handle, 5);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_5, data.Dressing.PropIndices.Unk_5, data.Dressing.PropTextures.Unk_5, true);
			if (data.Dressing.PropIndices.Orologi == -1)
				ClearPedProp(p.Handle, 6);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Orologi, data.Dressing.PropIndices.Orologi, data.Dressing.PropTextures.Orologi, true);
			if (data.Dressing.PropIndices.Bracciali == -1)
				ClearPedProp(p.Handle, 7);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Bracciali, data.Dressing.PropIndices.Bracciali, data.Dressing.PropTextures.Bracciali, true);
			if (data.Dressing.PropIndices.Unk_8 == -1)
				ClearPedProp(p.Handle, 8);
			else
				SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_8, data.Dressing.PropIndices.Unk_8, data.Dressing.PropTextures.Unk_8, true);
			while (GetPedDrawableVariation(p.Handle, (int)DrawableIndexes.Torso_2) != data.Dressing.ComponentDrawables.Torso_2) await BaseScript.Delay(0);
			cambiato = true;
			await Task.FromResult(0);
		}

		private static void Disconnetti()
		{
			GuiEnabled = false;
			ToggleMenu(false);
			BaseScript.TriggerEvent("lprp:manager:warningMessage", "Stai uscendo dal gioco senza aver selezionato un personaggio", "Sei sicuro?", 16392, "lprp:sceltaCharSelect");
		}

		private static async Task Controllo()
		{
			if (p1.Exists()) p1.Heading += 1.2f;
		}

		private static async void NuovoPersonaggio(NewChar data)
		{
			Screen.Fading.FadeOut(800);
			await BaseScript.Delay(1000);
			GuiEnabled = false;
			ToggleMenu(false);
			await Creator.CharCreationMenu(data);
		}

		public static async void Scelta(string param)
		{
			if (param == "select")
				BaseScript.TriggerServerEvent("lprp:dropPlayer", "Grazie di essere passato da " + Client.Impostazioni.Main.NomeServer + "!");
			else
				Attiva();
		}
	}
}