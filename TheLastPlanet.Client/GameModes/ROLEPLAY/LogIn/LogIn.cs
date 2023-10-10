using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Ingresso;
using TheLastPlanet.Client.GameMode.ROLEPLAY.CharCreation;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.LogIn
{
    public static class LogIn
    {
        public static bool GuiEnabled = false;
        public static Camera attuale;
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
            Client.Instance.NuiManager.RegisterCallback("chars:preview", new Action<string>(SelezionatoPreview));
            Client.Instance.NuiManager.RegisterCallback("chars:select", new Action<string>(Selezionato));
            Client.Instance.NuiManager.RegisterCallback("chars:disconnect", Disconnect);
            Client.Instance.NuiManager.RegisterCallback("chars:new", new Action<NewChar>(NewChar));
            RequestModel((uint)PedHash.FreemodeMale01);
            RequestModel((uint)PedHash.FreemodeFemale01);
            Screen.Hud.IsRadarVisible = false;
            Initialize();
        }

        public static void Stop()
        {
            ClearFocus();
            Screen.Hud.IsRadarVisible = false;
        }

        public static async void Initialize()
        {
            while (true)
            {
                await BaseScript.Delay(0);
                Cache.PlayerCache.MyPlayer.Player.CanControlCharacter = false;
                if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true, false);
                Vector4 charSelectCoords = SelectFirstCoords[SharedMath.GetRandomInt(SelectFirstCoords.Count - 1)];
                RequestCollisionAtCoord(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z);
                Cache.PlayerCache.MyPlayer.Ped.Position = new Vector3(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z - 1);
                Cache.PlayerCache.MyPlayer.Ped.Heading = charSelectCoords.W;
                await Cache.PlayerCache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
                Cache.PlayerCache.MyPlayer.Ped.Style.SetDefaultClothes();

                if (Cache.PlayerCache.MyPlayer.Ped.Model == new Model(PedHash.FreemodeMale01))
                {
                    Ped p = Cache.PlayerCache.MyPlayer.Ped;
                    p.Style.SetDefaultClothes();
                    //p.SetDecor("TheLastPlanet2019fighissimo!yeah!", p.Handle);
                    await Cache.PlayerCache.Loaded();
                    Cache.PlayerCache.MyPlayer.Status.Instance.InstancePlayer("IngressoRoleplay");
                    await BaseScript.Delay(100);
                    //Cache.PlayerCache.MyPlayer.Player.State.Set("Pausa", new { Attivo = false }, true);
                    p.IsVisible = false;
                    p.IsPositionFrozen = true;
                    RequestCollisionAtCoord(charCreateCoords.X, charCreateCoords.Y, charCreateCoords.Z - 1);
                    charSelectionCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
                    SetGameplayCamRelativeHeading(0);
                    charSelectionCam.Position = GetOffsetFromEntityInWorldCoords(p.Handle, 0f, -2, 0);
                    charSelectionCam.PointAt(p);
                    charSelectionCam.IsActive = true;
                    attuale = charSelectionCam;
                    RenderScriptCams(true, false, 0, false, false);
                    Enable();
                }
                else { continue; }

                break;
            }
        }

        public static async void Enable()
        {
            dummyPed = await Functions.CreatePedLocally(PedHash.FreemodeFemale01, Cache.PlayerCache.MyPlayer.Ped.Position + new Vector3(10));
            dummyPed.IsVisible = false;
            dummyPed.IsPositionFrozen = false;
            dummyPed.IsCollisionEnabled = false;
            dummyPed.IsCollisionProof = false;
            dummyPed.BlockPermanentEvents = true;
            GuiEnabled = true;
            TimeWeather.WeatherClient.SetMeteoForMe((int)CitizenFX.Core.Weather.ExtraSunny, false, true);
            NetworkOverrideClockTime(SharedMath.GetRandomInt(0, 23), SharedMath.GetRandomInt(0, 59), SharedMath.GetRandomInt(0, 59));
            await Cache.PlayerCache.Loaded();
            List<LogInInfo> data = await EventDispatcher.Get<List<LogInInfo>>("lprp:RequestLoginInfo");
            ToggleMenu(true, "charloading", data);
            ShutdownLoadingScreen();
            ShutdownLoadingScreenNui();
            Screen.Fading.FadeIn(1000);
            Client.Instance.AddTick(Main.AFK);
        }

        private static void ToggleMenu(bool menuOpen, string menu = "", List<LogInInfo> data = null)
        {
            data ??= new List<LogInInfo>();
            Client.Instance.NuiManager.SendMessage("chars:toggleMenu", new { menuStatus = menuOpen, menu, data });
            Client.Instance.NuiManager.SetFocus(menuOpen, menuOpen);
            DisplayHud(!menuOpen);
            SetEnableHandcuffs(Cache.PlayerCache.MyPlayer.Ped.Handle, menuOpen);
        }

        private static Ped p1;

        private static async void SelezionatoPreview(string data)
        {
            try
            {
                if (GetRenderingCam() != attuale.Handle) attuale = new Camera(GetRenderingCam());
                cambiato = false;
                PedHash m = PedHash.FreemodeMale01;
                PedHash f = PedHash.FreemodeFemale01;
                SkinAndDress pers = await EventDispatcher.Get<SkinAndDress>("lprp:anteprimaChar", Convert.ToUInt64(data));
                if (p1 != null) p1.Delete();
                p1 = await Functions.CreatePedLocally(pers.Skin.Sex == "Male" ? m : f, pers.Position.ToVector3, pers.Position.Heading);
                p1.Style.SetDefaultClothes();
                await SetSkinAndClothes(p1, pers);
                while (!cambiato) await BaseScript.Delay(0);
                /*
				Camera aggiuntiva = World.CreateCamera(attuale.Position + new Vector3(0, 0, 50f), Vector3.Zero, GameplayCamera.FieldOfView);
				aggiuntiva.Direction = pers.Position.ToVector3;
				Camera newCam = World.CreateCamera(p1.GetOffsetPosition(new Vector3(0, -2f, 2f)), Vector3.Zero, GameplayCamera.FieldOfView);
				newCam.PointAt(p1);
				newCam.AttachTo(p1, new Vector3(0, 3, 1.2f));
				attuale.InterpTo(aggiuntiva, 3000, 1, 1);

				while (aggiuntiva.IsInterpolating)
				{
					await BaseScript.Delay(50);
					aggiuntiva.Position.SetFocus();
				}

				float dis = Vector3.Distance(aggiuntiva.Position, newCam.Position);
				aggiuntiva.InterpTo(newCam, 3000, 0, 1); // calcolare il tempo

				while (newCam.IsInterpolating)
				{
					await BaseScript.Delay(50);
					newCam.Position.SetFocus();
				}

				p1.Task.WanderAround();
				await BaseScript.Delay(5000);
				*/
                int switchType = GetIdealPlayerSwitchType(attuale.Position.X, attuale.Position.Y, attuale.Position.Z, pers.Position.X, pers.Position.Y, pers.Position.Z);
                SwitchOutPlayer(PlayerPedId(), 1 | 32 | 128 | 16384, switchType);
                await BaseScript.Delay(2000);
                Camera newCam = World.CreateCamera(p1.GetOffsetPosition(new Vector3(0, -2f, 2f)), Vector3.Zero, GameplayCamera.FieldOfView);
                newCam.PointAt(p1);
                newCam.AttachTo(p1, new Vector3(0, 3, 1.2f));
                newCam.IsActive = true;
                attuale.Delete();
                p1.Task.WanderAround();
                await BaseScript.Delay(2000);
                SwitchInPlayer(p1.Handle);
                while (IsPlayerSwitchInProgress()) await BaseScript.Delay(0);
                p1.Task.WanderAround();
                SetFocusEntity(p1.Handle);
                /*
				string scena = scenari[SharedMath.GetRandomInt(scenari.Count)];
				p1.Task.StartScenario(scena, p1.Position);
				//Client.Instance.AddTick(Controllo);
				*/
            }
            catch (Exception e)
            {
                Client.Logger.Error(e.ToString());
            }
        }

        private static async void Selezionato(string data)
        {
            ulong ID = Convert.ToUInt64(data);
            GuiEnabled = false;
            ToggleMenu(false);
            Screen.Fading.FadeOut(800);
            await BaseScript.Delay(1000);
            MenuHandler.CloseAndClearHistory();
            Screen.LoadingPrompt.Show("Loading", LoadingSpinnerType.Clockwise1);
            await BaseScript.Delay(3000);
            /*
			Cache.MyPlayer.User.char_current = Convert.ToUInt32(data["id"]);
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", Cache.MyPlayer.User.char_current);
			*/
            Cache.PlayerCache.MyPlayer.User.CurrentChar = await EventDispatcher.Get<Char_data>("lprp:Select_Char", ID);
            Char_data Data = Cache.PlayerCache.MyPlayer.User.CurrentChar;
            DestroyAllCams(true);
            EnableGameplayCam(true);
            await BaseScript.Delay(5000);
            RenderScriptCams(false, false, 0, false, false);
            StatSetInt(Functions.HashUint("MP0_WALLET_BALANCE"), Cache.PlayerCache.MyPlayer.User.Money, true);
            StatSetInt(Functions.HashUint("BANK_BALANCE"), Cache.PlayerCache.MyPlayer.User.DirtyCash, true);
            await BaseScript.Delay(6000);
            Screen.Fading.FadeIn(800);
            await BaseScript.Delay(4000);
            Cache.PlayerCache.MyPlayer.Ped.IsInvincible = false;
            await BaseScript.Delay(1000);
            if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
            Screen.LoadingPrompt.Show("Loading Character", LoadingSpinnerType.Clockwise1);
            Cache.PlayerCache.MyPlayer.Ped.Position = await Cache.PlayerCache.MyPlayer.User.CurrentChar.Position.ToVector3.GetVector3WithGroundZ();
            if (p1 != null)
                if (p1.Exists())
                    p1.Delete();
            if (dummyPed != null)
                if (dummyPed.Exists())
                    dummyPed.Delete();
            Events.LoadModel();
            if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerPedId(), true, false);
            Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
            Cache.PlayerCache.MyPlayer.Ped.SetDecor("TheLastPlanet2019fighissimo!yeah!", Cache.PlayerCache.MyPlayer.Ped.Handle);
            Cache.PlayerCache.MyPlayer.Status.Instance.InstancePlayer("Joining");
            if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
            Screen.LoadingPrompt.Show("Server Synchronization", LoadingSpinnerType.Clockwise1);
            NetworkClearClockTimeOverride();

            if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true, false);
            await BaseScript.Delay(7000);
            EventDispatcher.Send("SyncWeatherForMe", true);
            if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
            Screen.LoadingPrompt.Show("Setting custom configuration", LoadingSpinnerType.RegularClockwise);
            await BaseScript.Delay(5000);
            if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
            Screen.LoadingPrompt.Show("Server joining", LoadingSpinnerType.RegularClockwise);
            //Cache.PlayerCache.MyPlayer.User.CurrentChar.Veicoli = await EventDispatcher.Get<List<OwnedVehicle>>("lprp:caricaVeicoli", Data.CharID);
            //EnableSwitchPauseBeforeDescent();
            SwitchInPlayer(Cache.PlayerCache.MyPlayer.Ped.Handle);
            //Position pos = await Data.Posizione.FindGroundZ();
            //Cache.MyPlayer.Ped.Position = pos.ToVector3;
            while (IsPlayerSwitchInProgress()) await BaseScript.Delay(1000);
            if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
            Client.Instance.RemoveTick(Control);
            if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true, false);
            await BaseScript.Delay(1000);
            Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
            Cache.PlayerCache.MyPlayer.Ped.Weapons.Select(WeaponHash.Unarmed);
            Cache.PlayerCache.MyPlayer.Status.PlayerStates.Spawned = true;

            AccessingEvents.RoleplaySpawn(PlayerCache.MyPlayer);

            EventDispatcher.Send("tlg:roleplay:onPlayerSpawn");
            ClearFocus();
            NetworkFadeInEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true);
            Cache.PlayerCache.MyPlayer.Ped.IsVisible = true;
            Cache.PlayerCache.MyPlayer.Ped.IsCollisionEnabled = true;
            //			Client.Instance.RemoveTick(Scaleform);
            //			Client.Instance.RemoveTick(TastiMenu);
            Cache.PlayerCache.MyPlayer.Player.CanControlCharacter = true;
        }

        public static async Task SetSkinAndClothes(Ped p, SkinAndDress data)
        {
            SetPedHeadBlendData(p.Handle, data.Skin.Face.Mom, data.Skin.Face.Dad, 0, data.Skin.Face.Mom, data.Skin.Face.Dad, 0, data.Skin.Resemblance, data.Skin.Skinmix, 0f, false);
            SetPedHeadOverlay(p.Handle, 0, data.Skin.Blemishes.Style, data.Skin.Blemishes.Opacity);
            SetPedHeadOverlay(p.Handle, 1, data.Skin.FacialHair.Beard.Style, data.Skin.FacialHair.Beard.Opacity);
            SetPedHeadOverlayColor(p.Handle, 1, 1, data.Skin.FacialHair.Beard.Color[0], data.Skin.FacialHair.Beard.Color[1]);
            SetPedHeadOverlay(p.Handle, 2, data.Skin.FacialHair.Eyebrow.Style, data.Skin.FacialHair.Eyebrow.Opacity);
            SetPedHeadOverlayColor(p.Handle, 2, 1, data.Skin.FacialHair.Eyebrow.Color[0], data.Skin.FacialHair.Eyebrow.Color[1]);
            SetPedHeadOverlay(p.Handle, 3, data.Skin.Ageing.Style, data.Skin.Ageing.Opacity);
            SetPedHeadOverlay(p.Handle, 4, data.Skin.Makeup.Style, data.Skin.Makeup.Opacity);
            SetPedHeadOverlay(p.Handle, 5, data.Skin.Blusher.Style, data.Skin.Blusher.Opacity);
            SetPedHeadOverlayColor(p.Handle, 5, 2, data.Skin.Blusher.Color[0], data.Skin.Blusher.Color[1]);
            SetPedHeadOverlay(p.Handle, 6, data.Skin.Complexion.Style, data.Skin.Complexion.Opacity);
            SetPedHeadOverlay(p.Handle, 7, data.Skin.SkinDamage.Style, data.Skin.SkinDamage.Opacity);
            SetPedHeadOverlay(p.Handle, 8, data.Skin.Lipstick.Style, data.Skin.Lipstick.Opacity);
            SetPedHeadOverlayColor(p.Handle, 8, 2, data.Skin.Lipstick.Color[0], data.Skin.Lipstick.Color[1]);
            SetPedHeadOverlay(p.Handle, 9, data.Skin.Freckles.Style, data.Skin.Freckles.Opacity);
            SetPedEyeColor(p.Handle, data.Skin.Eye.Style);
            SetPedComponentVariation(p.Handle, 2, data.Skin.Hair.Style, 0, 0);
            SetPedHairColor(p.Handle, data.Skin.Hair.Color[0], data.Skin.Hair.Color[1]);
            SetPedPropIndex(p.Handle, 2, data.Skin.Ears.Style, data.Skin.Ears.Color, false);
            for (int i = 0; i < data.Skin.Face.Traits.Length; i++) SetPedFaceFeature(p.Handle, i, data.Skin.Face.Traits[i]);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Face, data.Dressing.ComponentDrawables.Face, data.Dressing.ComponentTextures.Face, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Mask, data.Dressing.ComponentDrawables.Mask, data.Dressing.ComponentTextures.Mask, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Torso, data.Dressing.ComponentDrawables.Torso, data.Dressing.ComponentTextures.Torso, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Pants, data.Dressing.ComponentDrawables.Pants, data.Dressing.ComponentTextures.Pants, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Bag_Parachute, data.Dressing.ComponentDrawables.Bag_Parachute, data.Dressing.ComponentTextures.Bag_Parachute, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Shoes, data.Dressing.ComponentDrawables.Shoes, data.Dressing.ComponentTextures.Shoes, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Accessories, data.Dressing.ComponentDrawables.Accessories, data.Dressing.ComponentTextures.Accessories, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Undershirt, data.Dressing.ComponentDrawables.Undershirt, data.Dressing.ComponentTextures.Undershirt, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Kevlar, data.Dressing.ComponentDrawables.Kevlar, data.Dressing.ComponentTextures.Kevlar, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Badge, data.Dressing.ComponentDrawables.Badge, data.Dressing.ComponentTextures.Badge, 2);
            SetPedComponentVariation(p.Handle, (int)DrawableIndexes.Torso_2, data.Dressing.ComponentDrawables.Torso_2, data.Dressing.ComponentTextures.Torso_2, 2);
            if (data.Dressing.PropIndices.Hats_masks == -1)
                ClearPedProp(p.Handle, 0);
            else
                SetPedPropIndex(p.Handle, (int)PropIndexes.Hats_Masks, data.Dressing.PropIndices.Hats_masks, data.Dressing.PropTextures.Hats_masks, false);
            if (data.Dressing.PropIndices.Ears == -1)
                ClearPedProp(p.Handle, 2);
            else
                SetPedPropIndex(p.Handle, (int)PropIndexes.Ears, data.Dressing.PropIndices.Ears, data.Dressing.PropTextures.Ears, false);
            if (data.Dressing.PropIndices.Glasses == -1)
                ClearPedProp(p.Handle, 1);
            else
                SetPedPropIndex(p.Handle, (int)PropIndexes.Glasses, data.Dressing.PropIndices.Glasses, data.Dressing.PropTextures.Glasses, true);
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
            if (data.Dressing.PropIndices.Watches == -1)
                ClearPedProp(p.Handle, 6);
            else
                SetPedPropIndex(p.Handle, (int)PropIndexes.Watches, data.Dressing.PropIndices.Watches, data.Dressing.PropTextures.Watches, true);
            if (data.Dressing.PropIndices.Bracelets == -1)
                ClearPedProp(p.Handle, 7);
            else
                SetPedPropIndex(p.Handle, (int)PropIndexes.Bracelets, data.Dressing.PropIndices.Bracelets, data.Dressing.PropTextures.Bracelets, true);
            if (data.Dressing.PropIndices.Unk_8 == -1)
                ClearPedProp(p.Handle, 8);
            else
                SetPedPropIndex(p.Handle, (int)PropIndexes.Unk_8, data.Dressing.PropIndices.Unk_8, data.Dressing.PropTextures.Unk_8, true);
            while (GetPedDrawableVariation(p.Handle, (int)DrawableIndexes.Torso_2) != data.Dressing.ComponentDrawables.Torso_2) await BaseScript.Delay(0);
            cambiato = true;
            await Task.FromResult(0);
        }

        private static async void Disconnect()
        {
            GuiEnabled = false;
            ToggleMenu(false);
            ScaleformUI.Main.Warning.ShowWarningWithButtons("Are you sure?", "You're disconnecting from the Roleplay Mode without choosing a character", "", new List<InstructionalButton> { new(CitizenFX.Core.Control.FrontendCancel, Game.GetGXTEntry("FE_HLP31")), new(CitizenFX.Core.Control.FrontendAccept, Game.GetGXTEntry("FE_HLP29")) }, "", WarningPopupType.Classic);
            ScaleformUI.Main.Warning.OnButtonPressed += async (a) =>
            {
                if (a.GamepadButton == CitizenFX.Core.Control.FrontendCancel)
                    Enable();
                else if (a.GamepadButton == CitizenFX.Core.Control.FrontendAccept)
                {
                    await Initializer.Stop();
                    ServerJoining.ReturnToLobby();
                    World.RenderingCamera = null;
                }
            };
        }

        private static async Task Control()
        {
            if (p1.Exists()) p1.Heading += 1.2f;
        }

        private static async void NewChar(NewChar data)
        {
            Screen.Fading.FadeOut(800);
            await BaseScript.Delay(1000);
            GuiEnabled = false;
            ToggleMenu(false);
            await Creator.CharCreationMenu(data);
        }
    }
}