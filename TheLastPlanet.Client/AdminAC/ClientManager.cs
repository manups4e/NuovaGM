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
        public static void Init()
        {
            //Client.Instance.AddTick(AC);

            InputHandler.AddInput(adminMenu);
            InputHandler.AddInput(noclip);
            InputHandler.AddInput(teleport);
        }

        public static void Stop()
        {
            InputHandler.RemoveInput(adminMenu);
            InputHandler.RemoveInput(noclip);
            InputHandler.RemoveInput(teleport);
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
                HUD.ShowNotification("Teletrasportato!", NotificationColor.Blue, true);
            }
            else
            {
                HUD.ShowNotification("Punto in mappa non trovato, imposta un punto in mappa!", NotificationColor.Red, true);
            }
        }
    }
}