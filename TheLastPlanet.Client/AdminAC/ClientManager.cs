﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Client.RolePlay.Core;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.AdminAC
{
	internal static class ClientManager
	{

		public static void Init()
		{
			//Client.Instance.AddTick(AC);
			Handlers.InputHandler.ListaInput.Add(new InputController(Control.DropAmmo, PadCheck.Keyboard, ControlModifier.Shift, new Action<Ped, object[]>(AdminMenu)));
			Handlers.InputHandler.ListaInput.Add(new InputController(Control.ReplayStartStopRecordingSecondary, PadCheck.Keyboard, action: new Action<Ped, object[]>(_NoClip)));
			Handlers.InputHandler.ListaInput.Add(new InputController(Control.SaveReplayClip, PadCheck.Keyboard, action: new Action<Ped, object[]>(Teleport)));
		}

		private static void AdminMenu(Ped p, object[] args)
		{
			if (!HUD.MenuPool.IsAnyMenuOpen) ManagerMenu.AdminMenu(Cache.MyPlayer.User.group_level);
		}

		private static void Teleport(Ped p, object[] args)
		{
			if (Cache.MyPlayer.User != null && (int)Cache.MyPlayer.User.group_level > 1) TeleportToMarker();
		}

		private static async void _NoClip(Ped p, object[] args)
		{
			if (Cache.MyPlayer.User == null || (int)Cache.MyPlayer.User.group_level < 4) return;

			if (!NoClip)
			{
				if (!Cache.MyPlayer.User.StatiPlayer.InVeicolo)
				{
					RequestAnimDict(noclip_ANIM_A);
					while (!HasAnimDictLoaded(noclip_ANIM_A)) await BaseScript.Delay(0);
					curLocation = Cache.MyPlayer.Posizione.ToVector3;
					curRotation = p.Rotation;
					curHeading = Cache.MyPlayer.Posizione.Heading;
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
				List<InstructionalButton> istr = new List<InstructionalButton>() 
				{
					new InstructionalButton(Control.FrontendLt, Control.Cover, "Sali"),
					new InstructionalButton(Control.FrontendRt, Control.HUDSpecial, "Scendi"),
					new InstructionalButton(Control.MoveLeftRight, "Ruota Dx / Sx"),
					new InstructionalButton(Control.MoveUpDown, "Muovi avanti / indietro"),
					new InstructionalButton(Control.FrontendX, "Cambia velocità"),
				};
				InstructionalButtonsHandler.InstructionalButtons.Enabled = true;
				InstructionalButtonsHandler.InstructionalButtons.SetInstructionalButtons(istr);

			}
			else
			{
				InstructionalButtonsHandler.InstructionalButtons.Enabled = false;
				Client.Instance.RemoveTick(noClip);

				while (p.IsInvincible)
				{
					p.IsInvincible = false;
					await BaseScript.Delay(0);
				}

				if (!Cache.MyPlayer.User.StatiPlayer.InVeicolo)
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

		public static async Task AC() { }

		private static async Task noClip()
		{
			Ped p = Cache.MyPlayer.Ped;
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

			Vector2 vect = new Vector2(forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curHeading)) * -1.0f, forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curHeading)));
			Entity target = p;
			if (Cache.MyPlayer.User.StatiPlayer.InVeicolo) target = p.CurrentVehicle;
			p.Velocity = new Vector3(0);

			if (!Cache.MyPlayer.User.StatiPlayer.InVeicolo)
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
			if (Input.IsDisabledControlPressed(Control.MoveUpOnly)) curLocation = Vector3.Subtract(curLocation, new Vector3(vect, 0));
			if (Input.IsDisabledControlPressed(Control.MoveDownOnly)) curLocation = Vector3.Add(curLocation, new Vector3(vect, 0));
			if (Input.IsDisabledControlPressed(Control.MoveLeftOnly)) curHeading += rotationSpeed;
			if (Input.IsControlPressed(Control.MoveRightOnly)) curHeading -= rotationSpeed;
			target.Position = curLocation;
			target.Heading = curHeading - rotationSpeed;
		}

		private static async void TeleportToMarker()
		{
			Position coords = Cache.MyPlayer.Posizione;
			bool blipFound = false;
			// search for marker blip
			int blipIterator = GetBlipInfoIdIterator();

			for (Blip i = new Blip(GetFirstBlipInfoId(blipIterator)); i.Exists() != false; i = new Blip(GetNextBlipInfoId(blipIterator)))
				if (i.Type == 4)
				{
					coords = i.Position.ToPosition();
					blipFound = true;

					break;
				}

			if (blipFound)
			{
				// get entity to teleport
				Entity ent = Cache.MyPlayer.Ped;
				if (Cache.MyPlayer.User.StatiPlayer.InVeicolo) ent = Cache.MyPlayer.Ped.CurrentVehicle;

				// load needed map region and check height levels for ground existence
				bool groundFound = false;
				float[] groundCheckHeight = new float[17] { 100.0f, 150.0f, 50.0f, 0.0f, 200.0f, 250.0f, 300.0f, 350.0f, 400.0f, 450.0f, 500.0f, 550.0f, 600.0f, 650.0f, 700.0f, 750.0f, 800.0f };
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
				HUD.ShowNotification("Punto in mappa non trovato, imposta un punto in mappa!", NotificationColor.Red, true);
		}
	}
}