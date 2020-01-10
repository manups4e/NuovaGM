using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Manager
{
	static class ClientManager
	{
		public enum Tipi_Di_Bottone
		{
			NONE = 0,
			SELECT = 1,
			OK = 2,
			YES = 4,
			BACK = 8,
			BACK_SELECT = 9,
			BACK_OK = 10,
			BACK_YES = 12,
			CANCEL = 16,
			CANCEL_SELECT = 17,
			CANCEL_OK = 18,
			CANCEL_YES = 20,
			NO = 32,
			NO_SELECT = 33,
			NO_OK = 34,
			YES_NO = 36,
			RETRY = 64,
			RETRY_SELECT = 65,
			RETRY_OK = 66,
			RETRY_YES = 68,
			RETRY_BACK = 72,
			RETRY_BACK_SELECT = 73,
			RETRY_BACK_OK = 74,
			RETRY_BACK_YES = 76,
			RETRY_CANCEL = 80,
			RETRY_CANCEL_SELECT = 81,
			RETRY_CANCEL_OK = 82,
			RETRY_CANCEL_YES = 84,
			SKIP = 256,
			SKIP_SELECT = 257,
			SKIP_OK = 258,
			SKIP_YES = 260,
			SKIP_BACK = 264,
			SKIP_BACK_SELECT = 265,
			SKIP_BACK_OK = 266,
			SKIP_BACK_YES = 268,
			SKIP_CANCEL = 272,
			SKIP_CANCEL_SELECT = 273,
			SKIP_CANCEL_OK = 274,
			SKIP_CANCEL_YES = 276,
			CONTINUE = 16384,
			BACK_CONTINUE = 16392,
			CANCEL_CONTINUE = 16400,
			LOADING_SPINNER = 134217728,
			SELECT_LOADING_SPINNER = 134217729,
			OK_LOADING_SPINNER = 134217730,
			YES_LOADING_SPINNER = 134217732,
			BACK_LOADING_SPINNER = 134217736,
			BACK_SELECT_LOADING_SPINNER = 134217737,
			BACK_OK_LOADING_SPINNER = 134217738,
			BACK_YES_LOADING_SPINNER = 134217740,
			CANCEL_LOADING_SPINNER = 134217744,
			CANCEL_SELECT_LOADING_SPINNER = 134217745,
			CANCEL_OK_LOADING_SPINNER = 134217746,
			CANCEL_YES_LOADING_SPINNER = 134217748
		}

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:manager:warningMessage", new Action<string, string, int, string>(WarningMessage));
			Client.GetInstance.RegisterTickHandler(AC);
		}

		// SUGGERISCO 16384 (PERMETTE SOLO IL TASTO CONTINUA)
		// 16392 (permette continua e indietro)
		static int instructionalKey;
		static string event1;
		public static async void WarningMessage(string title, string sub, int key, string evento1)
		{
			AddTextEntry("warning_message_first_line", title);
			AddTextEntry("warning_message_second_line", sub);
			instructionalKey = key;
			event1 = evento1;
			Client.GetInstance.RegisterTickHandler(WarningMessageTick);
			await Task.FromResult(0);
		}

		public static async void WarningMessage(string title, string sub, string sub2, int key, string evento1)
		{
			AddTextEntry("FACES_WARNH2", title);
			AddTextEntry("QM_NO_0", sub);
			AddTextEntry("QM_NO_3", sub2);
			instructionalKey = key;
			event1 = evento1;
			Client.GetInstance.RegisterTickHandler(FrontendAlertTick);
			await Task.FromResult(0);
		}

		public static async Task FrontendAlertTick()
		{
			int bg = 1;
			int p6 = 1;
			DrawFrontendAlert("FACES_WARNH2", "QM_NO_0", 3, 3, "QM_NO_3", 2, -1, 0, "FM_NXT_RAC", "QM_NO_1", true, 10);
			if (IsControlJustPressed(2, 201) || IsControlJustPressed(2, 217) || IsDisabledControlJustPressed(2, 201) || IsDisabledControlJustPressed(2, 217))
			{
				Client.GetInstance.DeregisterTickHandler(FrontendAlertTick);
				BaseScript.TriggerEvent(event1, "select");
				instructionalKey = 0;
			}
			else if (IsControlJustPressed(2, 202) || IsDisabledControlJustPressed(2, 202))
			{
				Client.GetInstance.DeregisterTickHandler(FrontendAlertTick);
				BaseScript.TriggerEvent(event1, "back");
				instructionalKey = 0;
			}
			else if (Game.IsControlPressed(2, Control.FrontendX) || Game.IsDisabledControlJustPressed(2, Control.FrontendX))
			{
				Client.GetInstance.DeregisterTickHandler(FrontendAlertTick);
				BaseScript.TriggerEvent(event1, "alternative");
				instructionalKey = 0;
			}
			await Task.FromResult(0);
		}


		public static async Task WarningMessageTick()
		{
			int bg = 1;
			int p6 = 1;
			SetWarningMessage("warning_message_first_line", instructionalKey, "warning_message_second_line", true, -1, ref bg, ref p6, true, 0);
			if (IsControlJustPressed(2, 201) || IsControlJustPressed(2, 217) || IsDisabledControlJustPressed(2, 201) || IsDisabledControlJustPressed(2, 217))
			{
				Client.GetInstance.DeregisterTickHandler(WarningMessageTick);
				BaseScript.TriggerEvent(event1, "select");
				instructionalKey = 0;
			}
			else if (IsControlJustPressed(2, 202) || IsDisabledControlJustPressed(2, 202))
			{
				Client.GetInstance.DeregisterTickHandler(WarningMessageTick);
				BaseScript.TriggerEvent(event1, "back");
				instructionalKey = 0;
			}
			else if (Game.IsControlPressed(2, Control.FrontendX) || Game.IsDisabledControlJustPressed(2, Control.FrontendX))
			{
				Client.GetInstance.DeregisterTickHandler(WarningMessageTick);
				BaseScript.TriggerEvent(event1, "alternative");
				instructionalKey = 0;
			}
			await Task.FromResult(0);
		}

		public static bool NoClip = false;
		private static string noclip_ANIM_A = "amb@world_human_stand_impatient@male@no_sign@base";
		private static string noclip_ANIM_B = "base";
		private static int travelSpeed = 0;
		private static Vector3 curLocation;
		private static float curHeading;
		private static string travelSpeedStr = "Media";

		public static async Task AC()
		{
			if (Game.IsControlPressed(0, Control.Sprint) && IsInputDisabled(2))
			{
				Game.DisableControlThisFrame(0, Control.DropAmmo);
				if (Game.IsDisabledControlJustPressed(0, Control.DropAmmo) && !HUD.MenuPool.IsAnyMenuOpen() && IsInputDisabled(2))
					ManagerMenu.AdminMenu(Eventi.Player.group_level);
			}
			if (Eventi.Player != null && Eventi.Player.group_level > 1)
			{
				if (Game.IsControlJustPressed(0, Control.SaveReplayClip) && IsInputDisabled(2))
				{
					TeleportToMarker();
				}
			}
			if (Eventi.Player != null && Eventi.Player.group_level > 3)
			{
				if (Game.IsControlJustPressed(0, Control.ReplayStartStopRecordingSecondary) && IsInputDisabled(2))
				{
					if (!NoClip)
					{
						RequestAnimDict(noclip_ANIM_A);
						while (!HasAnimDictLoaded(noclip_ANIM_A)) await BaseScript.Delay(0);
						curLocation = Game.PlayerPed.Position;
						curHeading = Game.PlayerPed.Heading;
						TaskPlayAnim(PlayerPedId(), noclip_ANIM_A, noclip_ANIM_B, 8.0f, 0.0f, -1, 9, 0, false, false, false);
						Client.GetInstance.RegisterTickHandler(noClip);
						NoClip = true;
					}
					else
					{
						if (!Game.PlayerPed.IsInVehicle())
						{
							ClearPedTasksImmediately(PlayerPedId());
							SetUserRadioControlEnabled(true);
							Game.PlayerPed.IsInvincible = false;
						}
						else
						{
							SetUserRadioControlEnabled(true);
							Game.PlayerPed.IsInvincible = false;
							Vehicle veh = Game.PlayerPed.CurrentVehicle;
							veh.IsInvincible = false;
						}
						Client.GetInstance.DeregisterTickHandler(noClip);
						ClearAllHelpMessages();
						NoClip = false;
					}
				}
			}
		}

		private static async Task noClip()
		{
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
			string helpTextTastiera = "~INPUT_COVER~ / ~INPUT_HUD_SPECIAL~ - Sali / Scendi\n~INPUT_MOVE_LEFT_ONLY~ / ~INPUT_MOVE_RIGHT_ONLY~ - Ruota destra / sinistra\n~INPUT_MOVE_UP_ONLY~ / ~INPUT_MOVE_DOWN_ONLY~ Muovi avanti / indietro\n~INPUT_SPRINT~ Cambia velocità\n\nVelocità attuale: ~y~" + travelSpeedStr + "~w~.";
			string helpTextPad = "~INPUT_FRONTEND_LT~ / ~INPUT_FRONTEND_RT~ - Sali / Scendi\n~INPUT_MOVE_LEFT_ONLY~ / ~INPUT_MOVE_RIGHT_ONLY~ - Ruota destra / sinistra\n~INPUT_MOVE_UP_ONLY~ / ~INPUT_MOVE_DOWN_ONLY~ Muovi avanti / indietro\n~INPUT_FRONTEND_X~ Cambia velocità\n\nVelocità attuale: ~y~" + travelSpeedStr + "~w~.";
			if (IsInputDisabled(2))
				HUD.ShowHelp(helpTextTastiera);
			else
				HUD.ShowHelp(helpTextPad);

			float rotationSpeed = 2.5f;
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
					forwardPush = 0.05f; //very slow
					travelSpeedStr = "Molto lenta";
					break;
				case 5:
					forwardPush = 0.2f; //slow
					travelSpeedStr = "Lenta";
					break;
			}

			float xVect = forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curHeading)) * -1.0f;
			float yVect = forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curHeading));

			Entity target = Game.PlayerPed;
			if (Game.PlayerPed.IsInVehicle())
				target = Game.PlayerPed.CurrentVehicle;

			Game.PlayerPed.Velocity = new Vector3(0);
			Game.PlayerPed.Rotation = new Vector3(0);

			if (!Game.PlayerPed.IsInVehicle())
			{
				SetUserRadioControlEnabled(false);
				Game.PlayerPed.IsInvincible = true;
			}
			else
			{
				SetUserRadioControlEnabled(false);
				Game.PlayerPed.IsInvincible = true;
				Vehicle veh = Game.PlayerPed.CurrentVehicle;
				veh.IsInvincible= true;
			}

			if (Game.IsDisabledControlJustPressed(0, Control.Sprint) && IsInputDisabled(2) || Game.IsDisabledControlJustPressed(0, Control.FrontendX) && !IsInputDisabled(2))
			{
				travelSpeed++;
				if (travelSpeed > 5)
					travelSpeed = 0;
			}
			if (Game.IsDisabledControlPressed(0, Control.Cover) && IsInputDisabled(2) || Game.IsDisabledControlPressed(0, Control.FrontendLt) && !IsInputDisabled(2))
				curLocation.Z += forwardPush / 2;
			if (Game.IsDisabledControlPressed(0, Control.HUDSpecial) && IsInputDisabled(2) || Game.IsDisabledControlPressed(0, Control.FrontendRt) && !IsInputDisabled(2))
				curLocation.Z -= forwardPush / 2;
			if (Game.IsDisabledControlPressed(0, Control.MoveUpOnly))
			{
				curLocation.X += xVect;
				curLocation.Y += yVect;
			}
			if (Game.IsDisabledControlPressed(0, Control.MoveDownOnly))
			{
				curLocation.X -= xVect;
				curLocation.Y -= yVect;
			}
			if (Game.IsDisabledControlPressed(0, Control.MoveLeftOnly))
				curHeading += rotationSpeed;
			if (Game.IsControlPressed(0, Control.MoveRightOnly))
				curHeading -= rotationSpeed;
			SetEntityCoordsNoOffset(target.Handle, curLocation.X, curLocation.Y, curLocation.Z, true, true, true);
			SetEntityHeading(target.Handle, curHeading - rotationSpeed);
		}

		private static async void TeleportToMarker()
		{
			Vector3 coords = Game.PlayerPed.Position;
			bool success = false;
			bool blipFound = false;
			// search for marker blip

			int blipIterator = GetBlipInfoIdIterator();
			for (Blip i = new Blip(GetFirstBlipInfoId(blipIterator)); i.Exists() != false; i = new Blip(GetNextBlipInfoId(blipIterator)))
			{
				if (i.Type == 4)
				{
					coords = i.Position;
					blipFound = true;
					break;
				}
			}
			if (blipFound)
			{
				// get entity to teleport
				Entity ent = Game.PlayerPed;
				if (Game.PlayerPed.IsInVehicle())
					ent = Game.PlayerPed.CurrentVehicle;

				// load needed map region and check height levels for ground existence
				bool groundFound = false;
				float[] groundCheckHeight = new float[17] {
					100.0f, 150.0f, 50.0f, 0.0f, 200.0f, 250.0f, 300.0f, 350.0f, 400.0f,
					450.0f, 500.0f, 550.0f, 600.0f, 650.0f, 700.0f, 750.0f, 800.0f
				};
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
