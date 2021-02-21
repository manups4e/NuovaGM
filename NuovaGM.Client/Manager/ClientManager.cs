using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Manager
{
	static class ClientManager
	{
		private static string title;
		private static string sub;
		private static string sub2;
		private static Scaleform _instructionalButtonsScaleform;
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
			Client.Instance.AddEventHandler("lprp:manager:warningMessage", new Action<string, string, int, string>(WarningMessage));
			Client.Instance.AddEventHandler("lprp:manager:updateText", new Action<string, string>(UpdateText));
			Client.Instance.AddEventHandler("lprp:manager:TeletrasportaDaMe", new Action<Vector3>(TippaDaMe));
			//Client.Instance.AddTick(AC);
			Handlers.InputHandler.ListaInput.Add(new InputController(Control.DropAmmo, PadCheck.Keyboard, ControlModifier.Shift, new Action<Ped, object[]>(AdminMenu)));
			Handlers.InputHandler.ListaInput.Add(new InputController(Control.ReplayStartStopRecordingSecondary, PadCheck.Keyboard, action: new Action<Ped, object[]>(_NoClip)));
			Handlers.InputHandler.ListaInput.Add(new InputController(Control.SaveReplayClip, PadCheck.Keyboard, action: new Action<Ped, object[]>(Teleport)));
		}

		private static void AdminMenu(Ped p, object[] args)
		{
			if (!HUD.MenuPool.IsAnyMenuOpen)
				ManagerMenu.AdminMenu(Eventi.Player.group_level);
		}
		private static void Teleport(Ped p, object[] args)
		{
			if (Eventi.Player != null && (int)Eventi.Player.group_level > 1)
				TeleportToMarker();
		}

		private static async void _NoClip(Ped p, object[] args)
		{
			if (Eventi.Player != null && (int)Eventi.Player.group_level > 3)
			{
				if (!NoClip)
				{
					if (!p.IsInVehicle())
					{
						RequestAnimDict(noclip_ANIM_A);
						while (!HasAnimDictLoaded(noclip_ANIM_A)) await BaseScript.Delay(0);
						curLocation = Eventi.Player.posizione.ToVector3();
						curRotation = p.Rotation;
						curHeading = Eventi.Player.posizione.W;
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

					_instructionalButtonsScaleform = new Scaleform("instructional_buttons");
				}
				else
				{
					_instructionalButtonsScaleform.Dispose();
					Client.Instance.RemoveTick(noClip);
					while (p.IsInvincible)
					{
						p.IsInvincible = false;
						await BaseScript.Delay(0);
					}
					if (!p.IsInVehicle())
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
		}

		private static void UpdateScaleform()
		{
			_instructionalButtonsScaleform.CallFunction("CLEAR_ALL");
			_instructionalButtonsScaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			_instructionalButtonsScaleform.CallFunction("CREATE_CONTAINER");

			InstructionalButton Sali = new InstructionalButton(IsInputDisabled(2) ? Control.Cover : Control.FrontendLt, "Sali");
			InstructionalButton Scendi = new InstructionalButton(IsInputDisabled(2) ? Control.HUDSpecial : Control.FrontendRt, "Scendi");
			InstructionalButton Ruota = new InstructionalButton(Control.MoveLeftRight, "Ruota Dx / Sx");
			InstructionalButton Muovi = new InstructionalButton(Control.MoveUpDown, "Muovi avanti / indietro");
			InstructionalButton Velocità = new InstructionalButton(Control.FrontendX, "Cambia velocità");

			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 0, Scendi.GetButtonId(), Scendi.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 1, Sali.GetButtonId(), Sali.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 2, Ruota.GetButtonId(), Ruota.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 3, Muovi.GetButtonId(), Muovi.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 4, Velocità.GetButtonId(), Velocità.Text);

			_instructionalButtonsScaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
		}


		private static void TippaDaMe(Vector3 coords)
		{
			Game.PlayerPed.Position = coords;
		}

		public static void UpdateText(string txt, string txt2)
		{
			sub = txt;
			if (txt2 != null)
				sub2 = txt2;
		}

		// SUGGERISCO 16384 (PERMETTE SOLO IL TASTO CONTINUA)
		// 16392 (permette continua e indietro)
		static int instructionalKey;
		static string event1;
		public static async void WarningMessage(string ttl, string _sub, int key, string evento1)
		{
			title = ttl;
			sub = _sub;
			instructionalKey = key;
			event1 = evento1;
			Client.Instance.AddTick(WarningMessageTick);
			await Task.FromResult(0);
		}

		public static async void WarningMessage(string ttl, string _sub, string _sub2, int key, string evento1)
		{
			title = ttl;
			sub = _sub;
			sub2 = _sub2;
			instructionalKey = key;
			event1 = evento1;
			Client.Instance.AddTick(FrontendAlertTick);
			await Task.FromResult(0);
		}

		public static async Task FrontendAlertTick()
		{
			int bg = 1;
			int p6 = 1;
			AddTextEntry("FACES_WARNH2", title);
			AddTextEntry("QM_NO_0", sub);
			AddTextEntry("QM_NO_3", sub2);
			DrawFrontendAlert("FACES_WARNH2", "QM_NO_0", 3, 3, "QM_NO_3", 2, -1, 0, "FM_NXT_RAC", "QM_NO_1", true, 10);
			if (IsControlJustPressed(2, 201) || IsControlJustPressed(2, 217) || IsDisabledControlJustPressed(2, 201) || IsDisabledControlJustPressed(2, 217))
			{
				Client.Instance.RemoveTick(FrontendAlertTick);
				BaseScript.TriggerEvent(event1, "select");
				instructionalKey = 0;
			}
			else if (IsControlJustPressed(2, 202) || IsDisabledControlJustPressed(2, 202))
			{
				Client.Instance.RemoveTick(FrontendAlertTick);
				BaseScript.TriggerEvent(event1, "back");
				instructionalKey = 0;
			}
			else if (Game.IsControlPressed(2, Control.FrontendX) || Game.IsDisabledControlJustPressed(2, Control.FrontendX))
			{
				Client.Instance.RemoveTick(FrontendAlertTick);
				BaseScript.TriggerEvent(event1, "alternative");
				instructionalKey = 0;
			}
			await Task.FromResult(0);
		}


		public static async Task WarningMessageTick()
		{
			int bg = 1;
			int p6 = 1;
			AddTextEntry("warning_message_first_line", title);
			AddTextEntry("warning_message_second_line", sub);
			SetWarningMessage("warning_message_first_line", instructionalKey, "warning_message_second_line", true, -1, ref bg, ref p6, true, 0);
			if (IsControlJustPressed(2, 201) || IsControlJustPressed(2, 217) || IsDisabledControlJustPressed(2, 201) || IsDisabledControlJustPressed(2, 217))
			{
				Client.Instance.RemoveTick(WarningMessageTick);
				BaseScript.TriggerEvent(event1, "select");
				instructionalKey = 0;
			}
			else if (IsControlJustPressed(2, 202) || IsDisabledControlJustPressed(2, 202))
			{
				Client.Instance.RemoveTick(WarningMessageTick);
				BaseScript.TriggerEvent(event1, "back");
				instructionalKey = 0;
			}
			else if (Game.IsControlPressed(2, Control.FrontendX) || Game.IsDisabledControlJustPressed(2, Control.FrontendX))
			{
				Client.Instance.RemoveTick(WarningMessageTick);
				BaseScript.TriggerEvent(event1, "alternative");
				instructionalKey = 0;
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
			Ped p = new Ped(PlayerPedId());
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

			UpdateScaleform();
			if (!Main.ImpostazioniClient.ModCinema)
				_instructionalButtonsScaleform.Render2D();
			else
				DrawScaleformMovie(_instructionalButtonsScaleform.Handle, 0.5f, 0.5f - (Main.ImpostazioniClient.LetterBox / 1000), 1f, 1f, 255, 255, 255, 255, 0);
			HUD.ShowHelp("Velocità attuale: ~y~" + travelSpeedStr + "~w~.");

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

			float xVect = forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curHeading)) * -1.0f;
			float yVect = forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curHeading));

			Entity target = p;
			if (p.IsInVehicle())
				target = p.CurrentVehicle;

			p.Velocity = new Vector3(0);

			if (!p.IsInVehicle())
			{
				SetUserRadioControlEnabled(false);
				p.IsInvincible = true;
			}
			else
			{
				SetUserRadioControlEnabled(false);
				p.IsInvincible = true;
				Vehicle veh = p.CurrentVehicle;
				veh.IsInvincible= true;
			}

			if (Input.IsDisabledControlJustPressed(Control.FrontendX))
			{
				travelSpeed++;
				if (travelSpeed > 6)
					travelSpeed = 0;
			}
			if (Input.IsDisabledControlPressed(Control.Cover, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendLt, PadCheck.Controller))
				curLocation.Z += forwardPush / 2;
			if (Input.IsDisabledControlPressed(Control.HUDSpecial, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendRt, PadCheck.Controller))
				curLocation.Z -= forwardPush / 2;
			if (Input.IsDisabledControlPressed(Control.MoveUpOnly))
			{
				curLocation.X += xVect;
				curLocation.Y += yVect;
			}
			if (Input.IsDisabledControlPressed(Control.MoveDownOnly))
			{
				curLocation.X -= xVect;
				curLocation.Y -= yVect;
			}
			if (Input.IsDisabledControlPressed(Control.MoveLeftOnly))
				curHeading += rotationSpeed;
			if (Input.IsControlPressed(Control.MoveRightOnly))
				curHeading -= rotationSpeed;
			if (Input.IsDisabledControlPressed(Control.FrontendLb))
			{
				Game.DisableControlThisFrame(0, Control.LookLeftRight);
				Game.DisableControlThisFrame(0, Control.LookUpDown);
				Game.DisableControlThisFrame(0, Control.LookDown);
				Game.DisableControlThisFrame(0, Control.LookUp);
				Game.DisableControlThisFrame(0, Control.LookLeft);
				Game.DisableControlThisFrame(0, Control.LookRight);
				Game.DisableControlThisFrame(0, Control.LookDownOnly);
				Game.DisableControlThisFrame(0, Control.LookUpOnly);
				Game.DisableControlThisFrame(0, Control.LookLeftOnly);
				Game.DisableControlThisFrame(0, Control.LookRightOnly);

				if (Input.IsDisabledControlPressed(Control.LookDownOnly))
					curRotation.Y += rotationSpeed;
				if (Input.IsDisabledControlPressed(Control.LookUpOnly))
					curRotation.Y -= rotationSpeed;
				if (Input.IsDisabledControlPressed(Control.LookLeftOnly))
					curRotation.Z += rotationSpeed;
				if (Input.IsDisabledControlPressed(Control.LookRightOnly))
					curRotation.Z -= rotationSpeed;
			}
			SetEntityCoordsNoOffset(target.Handle, curLocation.X, curLocation.Y, curLocation.Z, true, true, true);
			SetEntityRotation(target.Handle, curRotation.X, curRotation.Y, curRotation.Z, 2, true);
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
				Entity ent = new Ped(PlayerPedId());
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
