using System.Threading.Tasks;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;

//using ScaleformUI.PauseMenu;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions
{
    internal static class PrimaPersonaObbligatoria
    {
        private static bool Switched = false;
        private static int vecchiaMod = 2;
        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }
        public static void Spawnato(PlayerClient client)
        {
            Client.Instance.AddTick(WeaponHandling);
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveTick(WeaponHandling);
        }


        //aggiungere controlli impostazioni.. V
        //migliorare coordinate testa e aggiungere effetto grafico.. V
        //migliorare precisione in corsa e in genera in copertura e non.. V 
        private static async Task WeaponHandling()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            #region MiraPrimaPersona

            if (Main.ImpostazioniClient.ForceFirstPersonAiming)
            {
                if (Input.IsControlPressed(Control.Aim))
                {
                    if (p.IsAiming || p.IsAimingFromCover)
                        if ((Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle ? GetFollowPedCamViewMode() : GetFollowVehicleCamViewMode()) != 4)
                            if (!Switched)
                            {
                                Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                                Switched = true;
                                vecchiaMod = Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle ? GetFollowVehicleCamViewMode() : GetFollowPedCamViewMode();
                                Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                                Camera CamFinale = World.CreateCamera(p.Bones[Bone.SKEL_Head].Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                                World.RenderingCamera = CamIniziale;
                                CamIniziale.InterpTo(CamFinale, 500, 1, 1);
                                while (CamFinale.IsInterpolating) await BaseScript.Delay(0);
                                if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                                    SetFollowVehicleCamViewMode(4);
                                else
                                    SetFollowPedCamViewMode(4);
                                RenderScriptCams(false, false, 500, true, true);
                                CamIniziale.Delete();
                                CamFinale.Delete();
                            }
                }
                else
                {
                    if (!p.IsAiming && !p.IsAimingFromCover && !p.IsInCover() && !Main.ImpostazioniClient.ForceFirstPersonInCar)
                        if (Switched)
                        {
                            Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                            Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            World.RenderingCamera = CamIniziale;
                            await BaseScript.Delay(100);
                            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                                SetFollowVehicleCamViewMode(vecchiaMod);
                            else
                                SetFollowPedCamViewMode(vecchiaMod);
                            await BaseScript.Delay(100);
                            Camera CamFinale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            CamIniziale.InterpTo(CamFinale, 500, 1, 1);
                            while (CamFinale.IsInterpolating) await BaseScript.Delay(10);
                            RenderScriptCams(false, true, 500, true, true);
                            CamIniziale.Delete();
                            CamFinale.Delete();
                            Switched = false;
                        }
                }
            }

            #endregion

            #region CoperturaPrimaPersona

            if (Main.ImpostazioniClient.ForceFirstPersonCover)
            {
                if (p.IsGoingIntoCover)
                {
                    if (!Switched)
                    {
                        Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                        Switched = true;
                        vecchiaMod = GetFollowPedCamViewMode();
                        int timer = GetGameTimer();

                        while (!p.IsInCover())
                        {
                            await BaseScript.Delay(20);

                            if (GetGameTimer() - timer > 10000)
                            {
                                Client.Logger.Debug("No veh vicini.. break");

                                return;
                            }
                        }

                        Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                        Camera CamFinale = World.CreateCamera(p.Bones[Bone.SKEL_Head].Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                        World.RenderingCamera = CamIniziale;
                        CamIniziale.InterpTo(CamFinale, 500, 1, 1);
                        while (CamFinale.IsInterpolating) await BaseScript.Delay(0);
                        RenderScriptCams(false, false, 500, true, true);
                        CamIniziale.Delete();
                        CamFinale.Delete();
                        SetFollowPedCamViewMode(4);
                    }
                }
                else
                {
                    if (Switched && !p.IsInCover() && !Input.IsControlPressed(Control.Aim) && !(Main.ImpostazioniClient.ForceFirstPersonInCar && Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle))
                    {
                        Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                        Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                        World.RenderingCamera = CamIniziale;
                        await BaseScript.Delay(100);
                        SetFollowPedCamViewMode(vecchiaMod);
                        await BaseScript.Delay(100);
                        Camera CamFinale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                        CamIniziale.InterpTo(CamFinale, 500, 1, 1);
                        while (CamFinale.IsInterpolating) await BaseScript.Delay(10);
                        RenderScriptCams(false, true, 500, true, true);
                        CamIniziale.Delete();
                        CamFinale.Delete();
                        Switched = false;
                    }
                }
            }

            #endregion

            #region GuidaPrimaPersona

            if (Main.ImpostazioniClient.ForceFirstPersonInCar)
                if (Input.IsControlJustPressed(Control.VehicleExit))
                {
                    if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                    {
                        while (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle) await BaseScript.Delay(0);

                        if (Switched)
                        {
                            Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                            Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            World.RenderingCamera = CamIniziale;
                            await BaseScript.Delay(100);
                            SetFollowPedCamViewMode(vecchiaMod);
                            await BaseScript.Delay(100);
                            Camera CamFinale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            CamIniziale.InterpTo(CamFinale, 500, 1, 1);
                            while (CamFinale.IsInterpolating) await BaseScript.Delay(10);
                            RenderScriptCams(false, true, 500, true, true);
                            CamIniziale.Delete();
                            CamFinale.Delete();
                            Switched = false;
                        }
                    }
                    else
                    {
                        vecchiaMod = GetFollowPedCamViewMode();
                        int timer = GetGameTimer();

                        while (!p.IsSittingInVehicle())
                        {
                            await BaseScript.Delay(20);

                            if (GetGameTimer() - timer > 10000)
                            {
                                Client.Logger.Debug("No veh vicini.. break");

                                return;
                            }
                        }

                        if (!Switched)
                        {
                            Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
                            Switched = true;
                            Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            Camera CamFinale = World.CreateCamera(p.Bones[Bone.SKEL_Head].Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
                            World.RenderingCamera = CamIniziale;
                            CamIniziale.InterpTo(CamFinale, 500, 1, 1);
                            while (CamFinale.IsInterpolating) await BaseScript.Delay(0);
                            RenderScriptCams(false, false, 500, true, true);
                            CamIniziale.Delete();
                            CamFinale.Delete();
                            SetFollowVehicleCamViewMode(4);
                        }
                    }
                }

            #endregion
        }
    }
}