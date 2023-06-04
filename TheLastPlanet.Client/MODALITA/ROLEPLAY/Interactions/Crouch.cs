using System;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions
{
    static class Crouch
    {
        public static bool IsCrouching { get; set; }
        private static InputController crouchInput = new InputController(Control.Duck, ModalitaServer.Roleplay, action: new Action<Ped>(Crouching));

        public static void Init()
        {
            InputHandler.AddInput(crouchInput);
        }

        public static void Stop()
        {
            InputHandler.RemoveInput(crouchInput);
        }

        public static void Crouching(Ped me)
        {
            API.DisableControlAction(0, 36, true);
            API.SetPedStealthMovement(me.Handle, false, "");
            if (IsCrouching) me.MovementAnimationSet = "move_ped_crouched";
            else API.ResetPedMovementClipset(me.Handle, 0);
        }
    }
}