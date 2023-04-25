using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.AdminAC
{
    internal static class ClientManager
    {
        private static InputController adminMenu = new(Control.DropAmmo, ModalitaServer.UNKNOWN, PadCheck.Keyboard, ControlModifier.Shift, new Action<Ped, object[]>(AdminMenu));
        private static InputController noclip = new(Control.ReplayStartStopRecordingSecondary, ModalitaServer.UNKNOWN, PadCheck.Keyboard, action: new Action<Ped, object[]>(_NoClip));
        private static InputController teleport = new(Control.SaveReplayClip, ModalitaServer.UNKNOWN, PadCheck.Keyboard, action: new Action<Ped, object[]>(Teleport));
        private static InputController camera = new(Control.ReplayStartStopRecording, ModalitaServer.UNKNOWN, PadCheck.Keyboard, action: new Action<Ped, object[]>(Telecamera));
        private static Camera noClipCamera;
        private static Vector3 cameraPosition;
        private static float zoom = 75f;
        private static float Height = 0;

        public static void Init()
        {
            //Client.Instance.AddTick(AC);

            InputHandler.AddInput(adminMenu);
            InputHandler.AddInput(noclip);
            InputHandler.AddInput(teleport);
            InputHandler.AddInput(camera);
        }

        public static void Stop()
        {
            InputHandler.RemoveInput(adminMenu);
            InputHandler.RemoveInput(noclip);
            InputHandler.RemoveInput(teleport);
            InputHandler.RemoveInput(camera);
        }

        private static void AdminMenu(Ped p, object[] args)
        {
            if (!HUD.MenuPool.IsAnyMenuOpen) ManagerMenu.AdminMenu(Cache.PlayerCache.MyPlayer.User.group_level);
        }

        private static void Teleport(Ped p, object[] args)
        {
            if (Cache.PlayerCache.MyPlayer.User != null && (int)Cache.PlayerCache.MyPlayer.User.group_level > 1) TeleportToMarker();
        }

        private static async void _NoClip(Ped p, object[] args)
        {
            if (Cache.PlayerCache.MyPlayer.User == null || (int)Cache.PlayerCache.MyPlayer.User.group_level < 4) return;

            if (!NoClip)
            {
                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo)
                {
                    RequestAnimDict(noclip_ANIM_A);
                    while (!HasAnimDictLoaded(noclip_ANIM_A)) await BaseScript.Delay(0);
                    curLocation = Cache.PlayerCache.MyPlayer.Posizione.ToVector3;
                    curRotation = p.Rotation;
                    curHeading = Cache.PlayerCache.MyPlayer.Posizione.Heading;
                    TaskPlayAnim(PlayerPedId(), noclip_ANIM_A, noclip_ANIM_B, 8.0f, 0.0f, -1, 9, 0, false, false, false);
                }
                else
                {
                    curLocation = p.CurrentVehicle.Position;
                    curRotation = p.CurrentVehicle.Rotation;
                    curHeading = p.CurrentVehicle.Heading;
                }

                p.Rotation = new Vector3(0);
                Client.Instance.AddTick(noClip);
                NoClip = true;
                List<InstructionalButton> istr = new()
                {
                    new InstructionalButton(Control.FrontendLt, Control.Cover, "Sali"),
                    new InstructionalButton(Control.FrontendRt, Control.HUDSpecial, "Scendi"),
                    new InstructionalButton(Control.MoveLeftRight, "Ruota Dx / Sx"),
                    new InstructionalButton(Control.MoveUpDown, "Muovi avanti / indietro"),
                    new InstructionalButton(Control.FrontendX, "Cambia velocità")
                };
                ScaleformUI.ScaleformUI.InstructionalButtons.Enabled = true;
                ScaleformUI.ScaleformUI.InstructionalButtons.SetInstructionalButtons(istr);
            }
            else
            {
                ScaleformUI.ScaleformUI.InstructionalButtons.Enabled = false;
                Client.Instance.RemoveTick(noClip);

                while (p.IsInvincible)
                {
                    p.IsInvincible = false;
                    await BaseScript.Delay(0);
                }

                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo)
                {
                    ClearPedTasksImmediately(PlayerPedId());
                    SetUserRadioControlEnabled(true);
                    p.IsInvincible = false;
                }
                else
                {
                    SetUserRadioControlEnabled(true);
                    p.IsInvincible = false;
                    Vehicle veh = p.CurrentVehicle;
                    veh.IsInvincible = false;
                }

                ClearAllHelpMessages();
                NoClip = false;
            }
        }

        public static bool NoClip = false;
        private static string noclip_ANIM_A = "amb@world_human_stand_impatient@male@no_sign@base";
        private static string noclip_ANIM_B = "base";
        private static int travelSpeed = 0;
        private static Vector3 curLocation;
        private static Vector3 curRotation;
        private static float curHeading;
        private static string travelSpeedStr = "Media";

        public static async Task AC()
        {
        }

        private static async Task noClip()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;
            Game.DisableAllControlsThisFrame(0);
            Game.EnableControlThisFrame(0, Control.LookLeftRight);
            Game.EnableControlThisFrame(0, Control.LookUpDown);
            Game.EnableControlThisFrame(0, Control.LookDown);
            Game.EnableControlThisFrame(0, Control.LookUp);
            Game.EnableControlThisFrame(0, Control.LookLeft);
            Game.EnableControlThisFrame(0, Control.LookRight);
            Game.EnableControlThisFrame(0, Control.LookDownOnly);
            Game.EnableControlThisFrame(0, Control.LookUpOnly);
            Game.EnableControlThisFrame(0, Control.LookLeftOnly);
            Game.EnableControlThisFrame(0, Control.LookRightOnly);
            HUD.ShowHelp("Velocità attuale: ~y~" + travelSpeedStr + "~w~.");
            const float rotationSpeed = 2.5f;
            float forwardPush = 0.8f;

            switch (travelSpeed)
            {
                case 0:
                    forwardPush = 0.8f; //medium
                    travelSpeedStr = "Media";

                    break;
                case 1:
                    forwardPush = 1.8f; //fast
                    travelSpeedStr = "Veloce";

                    break;
                case 2:
                    forwardPush = 3.6f; //very fast
                    travelSpeedStr = "Molto veloce";

                    break;
                case 3:
                    forwardPush = 5.4f; //extremely fast
                    travelSpeedStr = "Estremamente veloce";

                    break;
                case 4:
                    forwardPush = 0.025f; //very slow
                    travelSpeedStr = "Estremamente lenta";

                    break;
                case 5:
                    forwardPush = 0.05f; //very slow
                    travelSpeedStr = "Molto lenta";

                    break;
                case 6:
                    forwardPush = 0.2f; //slow
                    travelSpeedStr = "Lenta";

                    break;
            }

            Vector2 vect = new(forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curHeading)) * -1.0f, forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curHeading)));
            Entity target = p;
            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo) target = p.CurrentVehicle;
            p.Velocity = new Vector3(0);

            if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo)
            {
                SetUserRadioControlEnabled(false);
                p.IsInvincible = true;
            }
            else
            {
                SetUserRadioControlEnabled(false);
                p.IsInvincible = true;
                Vehicle veh = p.CurrentVehicle;
                veh.IsInvincible = true;
            }

            if (Input.IsDisabledControlJustPressed(Control.FrontendX))
            {
                travelSpeed++;
                if (travelSpeed > 6) travelSpeed = 0;
            }

            if (Input.IsDisabledControlPressed(Control.Cover, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendLt, PadCheck.Controller)) curLocation.Z += forwardPush / 2;
            if (Input.IsDisabledControlPressed(Control.HUDSpecial, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendRt, PadCheck.Controller)) curLocation.Z -= forwardPush / 2;
            if (Input.IsDisabledControlPressed(Control.MoveUpOnly)) curLocation = target is Vehicle ? Vector3.Add(curLocation, new Vector3(vect, 0)) : Vector3.Subtract(curLocation, new Vector3(vect, 0));
            if (Input.IsDisabledControlPressed(Control.MoveDownOnly)) curLocation = target is Vehicle ? Vector3.Subtract(curLocation, new Vector3(vect, 0)) : Vector3.Add(curLocation, new Vector3(vect, 0));
            if (Input.IsDisabledControlPressed(Control.MoveLeftOnly)) curHeading += rotationSpeed;
            if (Input.IsControlPressed(Control.MoveRightOnly)) curHeading -= rotationSpeed;
            target.Position = curLocation;
            target.Heading = curHeading - rotationSpeed;
        }

        private static async void Telecamera(Ped p, object[] args)
        {
            if (Cache.PlayerCache.MyPlayer.User == null || (int)Cache.PlayerCache.MyPlayer.User.group_level < 4) return;

            if (!NoClip)
            {
                noClipCamera = World.CreateCamera(PlayerCache.MyPlayer.Ped.Bones[Bone.SKEL_Head].Position, PlayerCache.MyPlayer.Ped.Rotation, GameplayCamera.FieldOfView);
                curLocation = PlayerCache.MyPlayer.Posizione.ToVector3;
                cameraPosition = curLocation;
                curRotation = p.Rotation;
                curHeading = PlayerCache.MyPlayer.Posizione.Heading;
                zoom = GameplayCamera.FieldOfView;
                p.Rotation = new Vector3(0);
                Client.Instance.AddTick(NoClipCamera);
                NoClip = true;
                List<InstructionalButton> istr = new()
                {
                    new InstructionalButton(Control.FrontendLt, "Sali"),
                    new InstructionalButton(Control.FrontendRt, "Scendi"),
                    new InstructionalButton(Control.FrontendLb, "Zoom+"),
                    new InstructionalButton(Control.FrontendRb, "Zoom-"),
                    new InstructionalButton(Control.MoveLeftRight, "Ruota Dx / Sx"),
                    new InstructionalButton(Control.MoveUpDown, "Muovi avanti / indietro"),
                    new InstructionalButton(Control.FrontendX, "Cambia velocità"),
                    new InstructionalButton(Control.NextCamera, "Save Camera")
                };
                ScaleformUI.ScaleformUI.InstructionalButtons.Enabled = true;
                ScaleformUI.ScaleformUI.InstructionalButtons.SetInstructionalButtons(istr);
                RenderScriptCams(true, true, 2000, true, false);
            }
            else
            {
                ScaleformUI.ScaleformUI.InstructionalButtons.Enabled = false;
                Client.Instance.RemoveTick(NoClipCamera);
                RenderScriptCams(false, true, 2000, true, false);
                noClipCamera = null;

                while (p.IsInvincible)
                {
                    p.IsInvincible = false;
                    await BaseScript.Delay(0);
                }

                if (!PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo)
                {
                    ClearPedTasksImmediately(PlayerPedId());
                    SetUserRadioControlEnabled(true);
                    p.IsInvincible = false;
                }
                else
                {
                    SetUserRadioControlEnabled(true);
                    p.IsInvincible = false;
                    Vehicle veh = p.CurrentVehicle;
                    veh.IsInvincible = false;
                }

                ClearAllHelpMessages();
                NoClip = false;
            }
        }

        static float forwardPush = 0.8f;

        private static async Task NoClipCamera()
        {
            Game.DisableAllControlsThisFrame(0);
            Game.DisableAllControlsThisFrame(1);
            Game.DisableAllControlsThisFrame(2);
            HUD.ShowHelp("Velocità attuale: ~y~" + travelSpeedStr + "~w~.");
            const float rotationSpeed = 2.5f;
            float fVar0 = GetDisabledControlNormal(2, 218) * forwardPush;
            float fVar1 = GetDisabledControlNormal(2, 219) * forwardPush;
            float fVar2 = GetDisabledControlNormal(2, 220);
            float fVar3 = GetDisabledControlNormal(2, 221);
            float ltNorm = GetDisabledControlNormal(2, 252);
            float rtNorm = GetDisabledControlNormal(2, 253);
            float zoomingWheel = 0f;
            if (!IsLookInverted())
            {
                fVar1 = -fVar1;
                fVar3 = -fVar3;
            }
            if (!IsInputDisabled(2))
            {
                N_0xc8b5c4a79cc18b94(noClipCamera.Handle);
            }
            else if (Input.IsDisabledControlPressed(Control.CreatorLT) || Input.IsDisabledControlPressed(Control.CreatorRT))
            {
                N_0xc8b5c4a79cc18b94(noClipCamera.Handle);
            }
            if (IsInputDisabled(2))
            {
                fVar2 = GetDisabledControlUnboundNormal(2, 1) * 2;
                fVar3 = GetDisabledControlUnboundNormal(2, 2) * -1 * 2;
                if (GetDisabledControlNormal(2, 241) > 0.25f)
                    zoomingWheel = 3f;
                if (GetDisabledControlNormal(2, 242) > 0.25f)
                    zoomingWheel = -3f;
            }

            float xVectFwd = -fVar1 * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z));
            float yVectFwd = fVar1 * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
            float xVectLat = fVar0 * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
            float yVectLat = fVar0 * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z));


            cameraPosition.X += xVectFwd + xVectLat;
            cameraPosition.Y += yVectFwd + yVectLat;
            curRotation = new(fVar3 + noClipCamera.Rotation.X, 0, -fVar2 + noClipCamera.Rotation.Z);

            curLocation = new((cameraPosition + zoom * noClipCamera.CamForwardVector()).X, (cameraPosition + zoom * noClipCamera.CamForwardVector()).Y, Height);

            if (ltNorm > 0 || rtNorm > 0 || zoomingWheel != 0)
            {
                zoom += rtNorm;
                zoom -= ltNorm;
                zoom += zoomingWheel;
                if (zoom >= 130f)
                    zoom = 130f;
                if (zoom <= 16f)
                    zoom = 16f;
                noClipCamera.FieldOfView = zoom;
            }

            if (IsDisabledControlPressed(2, func_7450()))
            {
                cameraPosition.Z += 1f * forwardPush;
                Height += 1f * forwardPush;
            }
            if (IsDisabledControlPressed(2, func_7449()))
            {
                cameraPosition.Z -= 1f * forwardPush;
                Height -= 1f * forwardPush;
            }


            switch (travelSpeed)
            {
                case 0:
                    forwardPush = 0.1f; //medium
                    travelSpeedStr = "Media";

                    break;
                case 1:
                    forwardPush = 0.2f; //fast
                    travelSpeedStr = "Veloce";

                    break;
                case 2:
                    forwardPush = 0.4f; //very fast
                    travelSpeedStr = "Molto veloce";

                    break;
                case 3:
                    forwardPush = 0.8f; //extremely fast
                    travelSpeedStr = "Estremamente veloce";

                    break;
                case 6:
                    forwardPush = 0.02f; //very slow
                    travelSpeedStr = "Estremamente lenta";

                    break;
                case 5:
                    forwardPush = 0.05f; //very slow
                    travelSpeedStr = "Molto lenta";

                    break;
                case 4:
                    forwardPush = 0.08f; //slow
                    travelSpeedStr = "Lenta";

                    break;
            }

            if (Input.IsControlJustPressed(Control.FrontendX))
            {
                travelSpeed++;
                if (travelSpeed > 6) travelSpeed = 0;
            }

            if (Input.IsDisabledControlPressed(Control.FrontendLt)) zoom -= 1f;
            if (Input.IsDisabledControlPressed(Control.FrontendRt)) zoom += 1f;
            if (Input.IsControlJustPressed(Control.NextCamera)) Client.Logger.Debug(noClipCamera.ToJson());


            float z = 0;
            GetGroundZFor_3dCoord(curLocation.X, curLocation.Y, Height + 300, ref z, false);
            if (Height <= z + 0.3f)
                Height = z + 0.3f;

            noClipCamera.Position = cameraPosition;
            noClipCamera.Rotation = curRotation;
            noClipCamera.FieldOfView = zoom;

            HUD.DrawText(0.4f, 0.825f, $"~o~Posizione~w~: {cameraPosition}");
            HUD.DrawText(0.4f, 0.85f, $"Rotazione: {curRotation}");
            HUD.DrawText(0.4f, 0.80f, $"Fov = {zoom}");
        }

        private static int func_7449()
        {
            if (IsInputDisabled(2))
                return 251;
            return 206;
        }

        private static int func_7450()
        {
            if (IsInputDisabled(2))
                return 250;
            return 205;
        }

        private static async void TeleportToMarker()
        {
            Position coords = Cache.PlayerCache.MyPlayer.Posizione;
            bool blipFound = false;
            // search for marker blip
            int blipIterator = GetBlipInfoIdIterator();

            for (Blip i = new(GetFirstBlipInfoId(blipIterator)); i.Exists(); i = new Blip(GetNextBlipInfoId(blipIterator)))
                if (i.Type == 4)
                {
                    coords = i.Position.ToPosition();
                    blipFound = true;

                    break;
                }

            if (blipFound)
            {
                // get entity to teleport
                Entity ent = Cache.PlayerCache.MyPlayer.Ped;
                if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo) ent = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle;

                // load needed map region and check height levels for ground existence
                bool groundFound = false;
                float[] groundCheckHeight = { 100.0f, 150.0f, 50.0f, 0.0f, 200.0f, 250.0f, 300.0f, 350.0f, 400.0f, 450.0f, 500.0f, 550.0f, 600.0f, 650.0f, 700.0f, 750.0f, 800.0f };
                float ground = 0;

                for (int i = 0; i < groundCheckHeight.Length; i++)
                {
                    ent.PositionNoOffset = new Vector3(coords.X, coords.Y, groundCheckHeight[i]);
                    await BaseScript.Delay(100);

                    if (GetGroundZFor_3dCoord(coords.X, coords.Y, groundCheckHeight[i], ref ground, false))
                    {
                        groundFound = true;
                        ground += 3.0f;

                        break;
                    }
                }

                // if ground not found then set Z in air and give player a parachute
                if (!groundFound)
                {
                    ground = 1000.0f;
                    GiveDelayedWeaponToPed(PlayerPedId(), 0xFBAB5776, 1, false);
                }

                //do it
                ent.PositionNoOffset = new Vector3(coords.X, coords.Y, ground);
                HUD.ShowNotification("Teletrasportato!", ColoreNotifica.Blue, true);
            }
            else
            {
                HUD.ShowNotification("Punto in mappa non trovato, imposta un punto in mappa!", ColoreNotifica.Red, true);
            }
        }
    }
}