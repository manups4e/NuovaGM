﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.CharCreation;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Core
{
    static class Main
    {
        public static string decorName;
        public static int decorInt;
        private static int HostId;
        public static bool spawned = false;
        public static ClientConfigKVP ClientConfig = new();

        public static List<Gang> ActiveGangs = new List<Gang>();

        // NetworkConcealPlayer(Player player, bool toggle, bool p2); **This is what R* uses to hide players in MP interiors.

        private static List<Vector4> SelectFirstCoords = new List<Vector4>
        {
            new Vector4(-1503.000f, -1143.462f, 34.670f, 64.692f),
            new Vector4(747.339f, 525.837f, 345.395f, 39.975f),
            new Vector4(-2162.689f, -469.343f, 3.396f, 150.975f),
            new Vector4(-170.256f, -2357.418f, 100.596f, 95.975f),
            new Vector4(2126.171f, 3014.593f, 59.196f, 115.975f),
            new Vector4(-103.310f, -1215.578f, 53.796f, 270.975f),
            new Vector4(-3032.130f, 22.216f, 11.118f, 0f)
        };

        public static bool ispointing = false;
        private static RelationshipGroup player = new(Functions.HashInt("PLAYER"));
        public static Vector4 charSelectCoords;
        public static Vector4 charCreateCoords = new Vector4(402.91f, -996.74f, -100.00025f, 180.086f);

        private static List<string> types = new List<string>()
        {
            "CIVMALE",
            "CIVFEMALE",
            "COP",
            "WILD_ANIMAL",
            "SHARK",
            "COUGAR",
            "GUARD_DOG",
            "DOMESTIC_ANIMAL",
            "DEER",
            "SECURITY_GUARD",
            "PRIVATE_SECURITY",
            "FIREMAN",
            "GANG_1",
            "GANG_2",
            "GANG_9",
            "GANG_10",
            "AMBIENT_GANG_LOST",
            "AMBIENT_GANG_MEXICAN",
            "AMBIENT_GANG_FAMILY",
            "AMBIENT_GANG_BALLAS",
            "AMBIENT_GANG_MARABUNTE",
            "AMBIENT_GANG_CULT",
            "AMBIENT_GANG_SALVA",
            "AMBIENT_GANG_WEICHENG",
            "AMBIENT_GANG_HILLBILLY",
            "DEALER",
            "HATES_PLAYER",
            "HEN",
            "NO_RELATIONSHIP",
            "SPECIAL",
            "MISSION2",
            "MISSION3",
            "MISSION4",
            "MISSION5",
            "MISSION6",
            "MISSION7",
            "MISSION8",
            "AGGRESSIVE_INVESTIGATE",
            "MEDIC"
        };
        private static List<string> pickupList = new List<string>();
        private static List<WeaponHash> scopedWeapons { get; set; }

        private static bool kickWarning;
        private static Dictionary<long, float> recoils = new Dictionary<long, float>();

        public static Dictionary<string, KeyValuePair<string, string>> Textures = new Dictionary<string, KeyValuePair<string, string>>()
        {
            ["ModGarage"] = new KeyValuePair<string, string>("shopui_title_ie_modgarage", "shopui_title_ie_modgarage"),
            ["Michael"] = new KeyValuePair<string, string>("shopui_title_graphics_michael", "shopui_title_graphics_michael"),
            ["AmmuNation"] = new KeyValuePair<string, string>("shopui_title_gunclub", "shopui_title_gunclub"),
            ["Binco"] = new KeyValuePair<string, string>("shopui_title_lowendfashion2", "shopui_title_lowendfashion2"),
            ["Discount"] = new KeyValuePair<string, string>("shopui_title_lowendfashion", "shopui_title_lowendfashion"),
            ["Suburban"] = new KeyValuePair<string, string>("shopui_title_midfashion", "shopui_title_midfashion"),
            ["Ponsombys"] = new KeyValuePair<string, string>("shopui_title_highendfashion", "shopui_title_highendfashion"),
            ["Kuts"] = new KeyValuePair<string, string>("shopui_title_barber", "shopui_title_barber"),
            ["Hawick"] = new KeyValuePair<string, string>("shopui_title_barber4", "shopui_title_barber4"),
            ["Osheas"] = new KeyValuePair<string, string>("shopui_title_barber3", "shopui_title_barber3"),
            ["Combo"] = new KeyValuePair<string, string>("shopui_title_barber2", "shopui_title_barber2"),
            ["Mulet"] = new KeyValuePair<string, string>("shopui_title_highendsalon", "shopui_title_highendsalon"),
            ["247"] = new KeyValuePair<string, string>("shopui_title_conveniencestore", "shopui_title_conveniencestore"),
            ["ltd"] = new KeyValuePair<string, string>("shopui_title_gasstation", "shopui_title_gasstation"),
            ["rq"] = new KeyValuePair<string, string>("shopui_title_liquorstore2", "shopui_title_liquorstore2")
        };

        public static bool IsDead = false;
        public static bool isAdmin = false;
        public static bool LoadoutLoaded = false;
        private static bool passengerDriveBy = false;
        private static int _shootingTimer;
        private static int _generalTimer;

        public static void Init()
        {
            LoadMain();
            scopedWeapons = Client.Settings.RolePlay.Main.ScopedWeapons;
            passengerDriveBy = Client.Settings.RolePlay.Main.PassengerDriveBy;
            //Client.Instance.AddTick(Connesso);
            AccessingEvents.OnRoleplaySpawn += onPlayerSpawn;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            Client.Instance.AddEventHandler("onClientResourceStop", new Action<string>(OnClientResourceStop));
            Screen.Fading.FadeOut(800);
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("onClientResourceStop", new Action<string>(OnClientResourceStop));
            ClientConfig = null;
        }

        private static void LoadMain()
        {
            try
            {
                ClientConfig = Functions.LoadKvp<ClientConfigKVP>("SettingsClient");
            }
            catch (Exception _)
            {
                Client.Logger.Debug("No settings found, revert to default");
                ClientConfig = new ClientConfigKVP();
            }

            SetNuiFocus(false, false);
        }

        private static async void OnClientResourceStop(string resourceName)
        {
            if (resourceName == GetCurrentResourceName()) Screen.Fading.FadeOut(800);
        }

        public static async void onPlayerSpawn(PlayerClient client)
        {
            Ped playerPed = client.Ped;
            SetEnablePedEnveffScale(playerPed.Handle, true);
            SetPlayerTargetingMode(2);
            Game.MaxWantedLevel = 0;
            SetCanAttackFriendly(playerPed.Handle, true, true);
            NetworkSetFriendlyFireOption(true);
            AddTextEntry("FE_THDR_GTAO", Client.Settings.RolePlay.Main.ServerName);
            scopedWeapons = Client.Settings.RolePlay.Main.ScopedWeapons;
            passengerDriveBy = Client.Settings.RolePlay.Main.PassengerDriveBy;
            kickWarning = Client.Settings.RolePlay.Main.KickWarning;
            recoils = Client.Settings.RolePlay.Main.Recoils;
            pickupList = Client.Settings.RolePlay.Main.PickupList;
            SetPlayerHealthRechargeMultiplier(PlayerId(), -1.0f);
            client.Status.Instance.RemoveInstance();
            playerPed.IsVisible = true;
            client.Status.PlayerStates.Spawned = true;
            spawned = client.Status.PlayerStates.Spawned;

            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", Cache.MyPlayer.User.CurrentChar.CharID);
            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "status", true);

            if (client.User.DeathStatus)
            {
                HUD.ShowNotification("You were killes because you disconnected while you were dying or dead!", ColoreNotifica.Red, true);
                DateTime now = DateTime.Now;
                BaseScript.TriggerServerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- " + client.User.FullName + " spawned dead because disconnected while dead or dying");
                playerPed.Health = 0;
                client.Status.RolePlayStates.Dying = false;
            }

            //Peds();
            foreach (string t in types) player.SetRelationshipBetweenGroups(new RelationshipGroup(Functions.HashInt(t)), Relationship.Neutral, true);
            // test per vedere se va chiamato ogni tick o no
            pickupList.ForEach(x => N_0x616093ec6b139dd9(client.Player.Handle, Functions.HashUint(x), false));
            for (int i = 1; i <= 15; i++) EnableDispatchService(i, false);
            await BaseScript.Delay(0);
        }

        private static float ShakeCam(WeaponHash weap)
        {
            switch (Cache.PlayerCache.MyPlayer.Ped.Weapons.Current.Hash)
            {
                case WeaponHash.StunGun:
                case WeaponHash.FlareGun:
                    return 0.01f;
                case WeaponHash.SNSPistol:
                    return 0.02f;
                case WeaponHash.SNSPistolMk2:
                case WeaponHash.VintagePistol:
                case WeaponHash.DoubleAction:
                case WeaponHash.Pistol:
                    return 0.025f;
                case WeaponHash.PistolMk2:
                case WeaponHash.CombatPistol:
                case WeaponHash.HeavyPistol:
                case WeaponHash.MarksmanPistol:
                    return 0.03f;
                case WeaponHash.MicroSMG:
                case WeaponHash.MachinePistol:
                case WeaponHash.MiniSMG:
                    return 0.035f;
                case WeaponHash.Musket:
                    return 0.04f;
                case WeaponHash.Revolver:
                case WeaponHash.SMG:
                    return 0.045f;
                case WeaponHash.APPistol:
                case WeaponHash.Pistol50:
                case WeaponHash.Gusenberg:
                case WeaponHash.BullpupRifle:
                case WeaponHash.CompactRifle:
                case WeaponHash.DoubleBarrelShotgun:
                case WeaponHash.AssaultSMG:
                    return 0.05f;
                case WeaponHash.RevolverMk2:
                case WeaponHash.CombatPDW:
                case WeaponHash.SMGMk2:
                    return 0.055f;
                case WeaponHash.CarbineRifle:
                case WeaponHash.AdvancedRifle:
                case WeaponHash.SawnOffShotgun:
                case WeaponHash.SpecialCarbine:
                    return 0.06f;
                case WeaponHash.CarbineRifleMk2:
                case WeaponHash.BullpupRifleMk2:
                    return 0.065f;
                case WeaponHash.MG:
                case WeaponHash.PumpShotgun:
                case WeaponHash.AssaultRifle:
                    return 0.07f;
                case WeaponHash.SpecialCarbineMk2:
                    return 0.075f;
                case WeaponHash.CombatMG:
                case WeaponHash.CompactGrenadeLauncher:
                case WeaponHash.BullpupShotgun:
                case WeaponHash.GrenadeLauncher:
                case WeaponHash.SweeperShotgun:
                    return 0.08f;
                case WeaponHash.CombatMGMk2:
                case WeaponHash.AssaultRifleMk2:
                case WeaponHash.PumpShotgunMk2:
                    return 0.085f;
                case WeaponHash.MarksmanRifle:
                case WeaponHash.MarksmanRifleMk2:
                    return 0.1f;
                case WeaponHash.AssaultShotgun:
                    return 0.12f;
                case WeaponHash.HeavyShotgun:
                    return 0.13f;
                case WeaponHash.Minigun:
                case WeaponHash.SniperRifle:
                    return 0.2f;
                case WeaponHash.HeavySniper:
                    return 0.3f;
                case WeaponHash.HeavySniperMk2:
                    return 0.35f;
                case WeaponHash.Firework:
                    return 0.5f;
                case WeaponHash.RPG:
                case WeaponHash.HomingLauncher:
                    return 0.9f;
                case WeaponHash.Railgun:
                    return 1.0f;
                default:
                    return 0;
            }
        }

        private static int _timerDriveBy;

        public static async Task MainTick()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;
            Player pl = Cache.PlayerCache.MyPlayer.Player;
            int gameTime = Game.GameTime;

            #region death?andweapon

            Game.DisableControlThisFrame(0, Control.SpecialAbilitySecondary);

            if (IsDead)
            {
                Game.DisableAllControlsThisFrame(0);
                EnableControlAction(0, 47, true);
                EnableControlAction(0, 245, true);
                EnableControlAction(0, 38, true);
                Game.EnableControlThisFrame(0, Control.FrontendPause);
            }

            if (IsPedArmed(p.Handle, 6))
            {
                Game.DisableControlThisFrame(0, Control.MeleeAttackLight);
                Game.DisableControlThisFrame(0, Control.MeleeAttackHeavy);
                Game.DisableControlThisFrame(0, Control.MeleeAttackAlternate);
                Game.DisableControlThisFrame(0, Control.MeleeAttack1);
                Game.DisableControlThisFrame(0, Control.MeleeAttack2);
            }

            Weapon weapon = p.Weapons.Current;

            if (weapon.Hash != WeaponHash.Unarmed)
            {
                ManageReticle(weapon);
                if (p.IsAiming || p.IsAimingFromCover || p.IsShooting) DisplayAmmoThisFrame(false);

                if (p.IsShooting)
                {
                    int ammo = weapon.Ammo;
                    _shootingTimer = Game.GameTime;
                    if (Game.GameTime - _shootingTimer > 500) BaseScript.TriggerServerEvent("lprp:aggiornaAmmo", weapon.Hash, ammo);
                    // To be handled all weapons
                    ShakeGameplayCam("SMALL_EXPLOSION_SHAKE", ShakeCam(p.Weapons.Current.Hash));
                    if (weapon.Hash == WeaponHash.FireExtinguisher) weapon.InfiniteAmmo = true;

                    if (!p.IsDoingDriveBy)
                        if (recoils.ContainsKey((uint)weapon.Hash))
                            if (recoils[(uint)weapon.Hash] != 0)
                            {
                                float tv = 0;

                                if (GetFollowPedCamViewMode() != 4)
                                    do
                                    {
                                        await BaseScript.Delay(0);
                                        SetGameplayCamRelativePitch(GetGameplayCamRelativePitch() + 0.1f, 0.2f);
                                        tv += 0.1f;
                                    } while (tv < recoils[(uint)weapon.Hash]);
                                else
                                    do
                                    {
                                        await BaseScript.Delay(0);

                                        if (recoils[(uint)weapon.Hash] > 0.1)
                                        {
                                            SetGameplayCamRelativePitch(GetGameplayCamRelativePitch() + 0.6f, 1.2f);
                                            tv += 0.6f;
                                        }
                                        else
                                        {
                                            SetGameplayCamRelativePitch(GetGameplayCamRelativePitch() + 0.016f, 0.333f);
                                            tv += 0.1f;
                                        }
                                    } while (tv < recoils[(uint)weapon.Hash]);
                            }
                }
            }

            #endregion

            #region Generics

            if (p.IsJumping && Input.IsControlJustPressed(Control.Jump)) p.Ragdoll(1);
            pl.WantedLevel = 0;
            DisablePlayerVehicleRewards(pl.Handle);
            SetPedMinGroundTimeForStungun(p.Handle, 8000);

            #region DriveBy

            if (gameTime - _timerDriveBy > 1000)
                if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                {
                    if (p.SeatIndex == VehicleSeat.Driver)
                        SetPlayerCanDoDriveBy(pl.Handle, weapon.Hash == WeaponHash.Unarmed);
                    else
                        SetPlayerCanDoDriveBy(pl.Handle, passengerDriveBy || weapon.Hash == WeaponHash.Unarmed);
                }

            #endregion

            if ((Input.IsControlJustPressed(Control.MpTextChatTeam) || Input.IsDisabledControlJustPressed(Control.MpTextChatTeam)) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                if (!IsEntityPlayingAnim(p.Handle, "mp_arresting", "idle", 3)) startPointing();
            }
            else if (Input.IsControlJustReleased(Control.MpTextChatTeam) || Input.IsDisabledControlJustReleased(Control.MpTextChatTeam))
            {
                StopPointing();
            }

            if (ispointing)
            {
                float camPitch = GetGameplayCamRelativePitch();
                if (camPitch < -70.0)
                    camPitch = -70.0f;
                else if (camPitch > 42.0) camPitch = 42.0f;
                camPitch = (camPitch + 70.0f) / 112.0f;
                float camHeading = GetGameplayCamRelativeHeading();
                float cosCamHeading = Cos(camHeading);
                float sinCamHeading = Sin(camHeading);
                if (camHeading < -180.0)
                    camHeading = -180.0f;
                else if (camHeading > 180.0) camHeading = 180.0f;
                camHeading = (camHeading + 180.0f) / 360.0f;
                bool blocked = false;
                int nn = 0;
                Vector3 coords = GetOffsetFromEntityInWorldCoords(p.Handle, cosCamHeading * -0.2f - sinCamHeading * (0.4f * camHeading + 0.3f), sinCamHeading * -0.2f + cosCamHeading * (0.4f * camHeading + 0.3f), 0.6f);
                int ray = StartShapeTestCapsule(coords.X, coords.Y, coords.Z - 0.2f, coords.X, coords.Y, coords.Z + 0.2f, 0.4f, 95, p.Handle, 7);
                GetShapeTestResult(ray, ref blocked, ref coords, ref coords, ref nn);
                SetTaskPropertyFloat(p.Handle, "Pitch", camPitch);
                SetTaskPropertyFloat(p.Handle, "Heading", camHeading * -1.0f + 1.0f);
                SetTaskPropertyBool(p.Handle, "isBlocked", blocked);
                SetTaskPropertyBool(p.Handle, "isFirstPerson", N_0xee778f8c7e1142e2(N_0x19cafa3c87f7c2ff()) == 4);
            }

            #endregion
        }

        private static async void startPointing()
        {
            ispointing = true;
            RequestAnimDict("anim@mp_point");
            while (!HasAnimDictLoaded("anim@mp_point")) await BaseScript.Delay(0);
            SetPedCurrentWeaponVisible(Cache.PlayerCache.MyPlayer.Ped.Handle, false, true, true, true);
            SetPedConfigFlag(Cache.PlayerCache.MyPlayer.Ped.Handle, 36, true);
            TaskMoveNetwork(Cache.PlayerCache.MyPlayer.Ped.Handle, "task_mp_pointing", 0.5f, false, "anim@mp_point", 24);
            RemoveAnimDict("anim@mp_point");
        }

        private static void StopPointing()
        {
            ispointing = false;
            N_0xd01015c7316ae176(Cache.PlayerCache.MyPlayer.Ped.Handle, "Stop");
            if (!Cache.PlayerCache.MyPlayer.Ped.IsInjured) Cache.PlayerCache.MyPlayer.Ped.Task.ClearSecondary();
            if (!Cache.PlayerCache.MyPlayer.Ped.IsInVehicle()) SetPedCurrentWeaponVisible(Cache.PlayerCache.MyPlayer.Ped.Handle, true, true, true, true);
            SetPedConfigFlag(Cache.PlayerCache.MyPlayer.Ped.Handle, 36, false);
            Cache.PlayerCache.MyPlayer.Ped.Task.ClearSecondary();
        }

        public static int AFKTime = 600;
        public static bool abort = false;
        private static bool triggerato = false;
        private static Vector3 currentPosition;

        public static async Task AFK()
        {
            if ((int)Cache.PlayerCache.MyPlayer.User.group_level < 3 && !(Creator.Creation.Visible || Creator.Apparel.Visible || Creator.Appearances.Visible || Creator.Details.Visible || Creator.Parents.Visible || Creator.Info.Visible)) // helper e moderatori sono inclusi (gradi 0,1,2)
            {
                //				if (Ingresso.Ingresso.guiEnabled)
                //				else if (Menus.Creazione.Visible || Menus.Apparel.Visible || Menus.Apparenze.Visible || Menus.Dettagli.Visible || Menus.Genitori.Visible || Menus.Info.Visible)
                currentPosition = Cache.PlayerCache.MyPlayer.User == null ? Cache.PlayerCache.MyPlayer.Ped.Position : Cache.PlayerCache.MyPlayer.Position.ToVector3;
                int t = (int)Math.Floor(GetTimeSinceLastInput(0) / 1000f);
                if (Functions.IsAnyControlJustPressed())
                {
                    if (!ScaleformUI.Main.Warning.IsShowing) return;
                    ScaleformUI.Main.Warning.Dispose();
                    triggerato = false;
                    return;
                }
                Client.Logger.Debug("Time => " + t);

                if (t >= Client.Settings.RolePlay.Main.AFKCheckTime)
                {
                    if (Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, currentPosition) < 3f) BaseScript.TriggerServerEvent("lprp:dropPlayer", "Last Galaxy Shield 2.0:\nSei stato rilevato per troppo tempo AFK");
                }
                else
                {
                    if (t > Client.Settings.RolePlay.Main.AFKCheckTime - (int)Math.Floor(Client.Settings.RolePlay.Main.AFKCheckTime / 4f))
                    {
                        if (kickWarning)
                        {
                            string Text = $"You've been afk too much\nYou'll be kicked in {Client.Settings.RolePlay.Main.AFKCheckTime - t} seconds!";

                            if (!triggerato)
                            {
                                ScaleformUI.Main.Warning.ShowWarning("Last Galaxy Shield 2.0", Text, "");
                                triggerato = true;
                            }
                            ScaleformUI.Main.Warning.UpdateWarning("Last Galaxy Shield 2.0", Text, "", "", WarningPopupType.Serious);
                        }
                    }
                }
            }
        }

        private static void ManageReticle(Weapon weapon)
        {
            if (!scopedWeapons.Contains(weapon.Hash)) Screen.Hud.HideComponentThisFrame(HudComponent.Reticle);
        }

        public static void RespawnPed(Position coords)
        {
            IsDead = false;
            Cache.PlayerCache.MyPlayer.Ped.Position = coords.ToVector3;
            NetworkResurrectLocalPlayer(coords.X, coords.Y, coords.Z, coords.Heading, true, false);
            Cache.PlayerCache.MyPlayer.Ped.Health = 100;
            Cache.PlayerCache.MyPlayer.Ped.IsInvincible = false;
            Cache.PlayerCache.MyPlayer.Ped.ClearBloodDamage();
        }

        /*
		private static async void Peds()
		{
			await BaseScript.Delay(30000);
			while (Client.Impostazioni.RolePlay.Main.stripClub == null) await BaseScript.Delay(0);

			foreach (GenericPeds stripper in Client.Impostazioni.RolePlay.Main.stripClub)
			{
				Ped ped = await Funzioni.SpawnPed(stripper.model, stripper.coords, stripper.heading);
				ped.CanRagdoll = false;
				ped.BlockPermanentEvents = true;
				SetEntityCanBeDamaged(ped.Handle, false);
				SetPedFleeAttributes(ped.Handle, 0, false);
				SetPedCombatAttributes(ped.Handle, 17, true);
				ped.Task.PlayAnimation(stripper.animDict, stripper.animName, -1, -1, AnimationFlags.Loop);
			}

			foreach (GenericPeds market in Client.Impostazioni.RolePlay.Main.blackMarket)
			{
				Ped ped1 = await Funzioni.SpawnPed(market.model, market.coords, market.heading);
				ped1.CanRagdoll = false;
				ped1.BlockPermanentEvents = true;
				SetEntityCanBeDamaged(ped1.Handle, false);
				SetPedFleeAttributes(ped1.Handle, 0, false);
				SetPedCombatAttributes(ped1.Handle, 17, true);
				ped1.Task.StartScenario(market.animName, GetEntityCoords(ped1.Handle, true));
			}

			foreach (GenericPeds illegal in Client.Impostazioni.RolePlay.Main.illegal_weapon_extra_shop)
			{
				Ped ped2 = await Funzioni.SpawnPed(illegal.model, illegal.coords, illegal.heading);
				ped2.CanRagdoll = false;
				ped2.BlockPermanentEvents = true;
				SetEntityCanBeDamaged(ped2.Handle, false);
				SetPedFleeAttributes(ped2.Handle, 0, false);
				SetPedCombatAttributes(ped2.Handle, 17, true);
				ped2.Task.StartScenario(illegal.animName, GetEntityCoords(ped2.Handle, true));
			}
		}
		*/
    }

    public class GenericPeds
    {
        public Vector3 coords;
        public string animDict;
        public string animName;
        public string model;
        public float heading;
        public string scenario;

        public GenericPeds() { }

        public GenericPeds(Vector3 _c, string _ad, string _an, string _ml, float _h)
        {
            coords = _c;
            animDict = _ad;
            animName = _an;
            model = _ml;
            heading = _h;
        }
        public GenericPeds(Vector3 _c, string _ad, string _an, string _ml, string _sc, float _h)
        {
            coords = _c;
            animDict = _ad;
            animName = _an;
            model = _ml;
            scenario = _sc;
            heading = _h;
        }
    }
}