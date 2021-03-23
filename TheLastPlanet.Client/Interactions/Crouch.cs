﻿using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.Interactions
{
	static class Crouch
	{
		public static bool IsCrouching { get; set; }

		public static void Init()
		{
			InputHandler.ListaInput.Add(new InputController(Control.Duck, action: new Action<Ped>(Crouching)));
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