using System.Threading.Tasks;

//using ScaleformUI.PauseMenu;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Interactions
{
    internal static class ForcedFirstPersonPOV
    {
        private static bool Switched = false;
        private static int oldMod = 2;
        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }
        public static void Spawned(PlayerClient client)
        {
            Client.Instance.AddTick(WeaponHandling);
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveTick(WeaponHandling);
        }


        // TODO: ADD SETTINGS CONTROLS
        // ENHANCE HEAD BONE COORDS AND ADD SOME GRAPHIC EFFECT
        // ENHANCE PRECISION WHILE RUNNING OR COVERING AND WHAT NOT
        private static async Task WeaponHandling()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            #region AimingFirstPerson

            if (Main.ClientConfig.ForceFirstPersonAiming)
            {
                if (Input.IsControlPressed(Control.Aim))
                {
                    if (p.IsAiming || p.IsAimingFromCover)
                    {
                        if ((Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle ? GetFollowPedCamViewMode() : GetFollowVehicleCamViewMode()) != 4)
                        {
                            if (!Switched)
                            {
                                Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                                Switched = true;
                                oldMod = Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle ? GetFollowVehicleCamViewMode() : GetFollowPedCamViewMode();
                                Camera startCam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                                Camera endingCam = World.CreateCamera(p.Bones[Bone.SKEL_Head].Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                                World.RenderingCamera = startCam;
                                startCam.InterpTo(endingCam, 500, 1, 1);
                                while (endingCam.IsInterpolating) await BaseScript.Delay(0);
                                if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                                    SetFollowVehicleCamViewMode(4);
                                else
                                    SetFollowPedCamViewMode(4);
                                RenderScriptCams(false, false, 500, true, true);
                                startCam.Delete();
                                endingCam.Delete();
                            }
                        }
                    }
                }
                else
                {
                    if (!p.IsAiming && !p.IsAimingFromCover && !p.IsInCover() && !Main.ClientConfig.ForceFirstPersonInCar)
                    {
                        if (Switched)
                        {
                            Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                            Camera startCam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            World.RenderingCamera = startCam;
                            await BaseScript.Delay(100);
                            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                                SetFollowVehicleCamViewMode(oldMod);
                            else
                                SetFollowPedCamViewMode(oldMod);
                            await BaseScript.Delay(100);
                            Camera endingCam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            startCam.InterpTo(endingCam, 500, 1, 1);
                            while (endingCam.IsInterpolating) await BaseScript.Delay(10);
                            RenderScriptCams(false, true, 500, true, true);
                            startCam.Delete();
                            endingCam.Delete();
                            Switched = false;
                        }
                    }
                }
            }

            #endregion

            #region CoverFirstPerson

            if (Main.ClientConfig.ForceFirstPersonCover)
            {
                if (p.IsGoingIntoCover)
                {
                    if (!Switched)
                    {
                        Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                        Switched = true;
                        oldMod = GetFollowPedCamViewMode();
                        int timer = GetGameTimer();

                        while (!p.IsInCover())
                        {
                            await BaseScript.Delay(20);

                            if (GetGameTimer() - timer > 10000)
                            {
                                Client.Logger.Debug("No veh near.. break");

                                return;
                            }
                        }

                        Camera startCam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                        Camera endCam = World.CreateCamera(p.Bones[Bone.SKEL_Head].Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                        World.RenderingCamera = startCam;
                        startCam.InterpTo(endCam, 500, 1, 1);
                        while (endCam.IsInterpolating) await BaseScript.Delay(0);
                        RenderScriptCams(false, false, 500, true, true);
                        startCam.Delete();
                        endCam.Delete();
                        SetFollowPedCamViewMode(4);
                    }
                }
                else
                {
                    if (Switched && !p.IsInCover() && !Input.IsControlPressed(Control.Aim) && !(Main.ClientConfig.ForceFirstPersonInCar && Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle))
                    {
                        Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                        Camera startCam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                        World.RenderingCamera = startCam;
                        await BaseScript.Delay(100);
                        SetFollowPedCamViewMode(oldMod);
                        await BaseScript.Delay(100);
                        Camera endingCam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                        startCam.InterpTo(endingCam, 500, 1, 1);
                        while (endingCam.IsInterpolating) await BaseScript.Delay(10);
                        RenderScriptCams(false, true, 500, true, true);
                        startCam.Delete();
                        endingCam.Delete();
                        Switched = false;
                    }
                }
            }

            #endregion

            #region DriveFirstPerson

            if (Main.ClientConfig.ForceFirstPersonInCar)
            {
                if (Input.IsControlJustPressed(Control.VehicleExit))
                {
                    if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                    {
                        while (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle) await BaseScript.Delay(0);

                        if (Switched)
                        {
                            Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                            Camera startCam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            World.RenderingCamera = startCam;
                            await BaseScript.Delay(100);
                            SetFollowPedCamViewMode(oldMod);
                            await BaseScript.Delay(100);
                            Camera endCam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            startCam.InterpTo(endCam, 500, 1, 1);
                            while (endCam.IsInterpolating) await BaseScript.Delay(10);
                            RenderScriptCams(false, true, 500, true, true);
                            startCam.Delete();
                            endCam.Delete();
                            Switched = false;
                        }
                    }
                    else
                    {
                        oldMod = GetFollowPedCamViewMode();
                        int timer = GetGameTimer();

                        while (!p.IsSittingInVehicle())
                        {
                            await BaseScript.Delay(20);

                            if (GetGameTimer() - timer > 10000)
                            {
                                Client.Logger.Debug("No veh near.. break");

                                return;
                            }
                        }

                        if (!Switched)
                        {
                            Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                            Switched = true;
                            Camera startCam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            Camera endCam = World.CreateCamera(p.Bones[Bone.SKEL_Head].Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            World.RenderingCamera = startCam;
                            startCam.InterpTo(endCam, 500, 1, 1);
                            while (endCam.IsInterpolating) await BaseScript.Delay(0);
                            RenderScriptCams(false, false, 500, true, true);
                            startCam.Delete();
                            endCam.Delete();
                            SetFollowVehicleCamViewMode(4);
                        }
                    }
                }
            }

            #endregion
        }
    }
}