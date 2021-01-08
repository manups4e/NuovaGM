using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Shared;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core;
using Logger;

namespace TheLastPlanet.Client.Lavori.Whitelistati.VenditoreCase
{
	static class MenuCreazioneCasa
	{
		private static Camera MainCamera;
		private static int travelSpeed = 0;
		private static Vector3 curLocation;
		private static Vector3 curRotation;
		private static string travelSpeedStr = "Media";
		private static Scaleform _instructionalButtonsScaleform;
		private static int checkTimer = 0;
		public static async void MenuCreazioneCase()
		{
			UIMenu creazione = new UIMenu("Creatore Immobiliare", "Usare con cautela!");
			HUD.MenuPool.Add(creazione);

			UIMenu selezionePunto = creazione.AddSubMenu("1. Posizionare blip esterni"); // NB: nome provvisorio
			UIMenu gestioneInteriorCasa = creazione.AddSubMenu("2. Gestione Interior"); // NB: nome provvisorio
			UIMenu datiCasa = creazione.AddSubMenu("3. Dati della casa"); // NB: nome provvisorio


			#region selezionePunto
			UIMenu blip = selezionePunto.AddSubMenu("Posiziona Blip");
			UIMenuListItem blipType = new UIMenuListItem("Modello", new List<dynamic>() { BlipSprite.SafehouseForSale }, 0);
			UIMenuSliderProgressItem blipDimensions = new UIMenuSliderProgressItem("Dimensioni", 100, 0);
			UIMenuItem blipName = new UIMenuItem("Nome");
			blip.AddItem(blipType);
			blip.AddItem(blipDimensions);
			blip.AddItem(blipName);

			UIMenuListItem markerIngressoCasa;
			UIMenuListItem markerIngressoGarage;
			UIMenuCheckboxItem posCamera;

			HUD.MenuPool.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
			{
				if (state == MenuState.ChangeForward)
				{
					if (newmenu == selezionePunto)
					{
						SetPlayerControl(Game.Player.Handle, false, 256);
						Screen.Fading.FadeOut(800);
						while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
						if (MainCamera == null)
							MainCamera = World.CreateCamera(Game.Player.GetPlayerData().posizione.ToVector3() + new Vector3(0, 0, 100), new Vector3(0, 0, 0), 45f);
						MainCamera.IsActive = true;
						RenderScriptCams(true, false, 1000, true, true);
						curLocation = MainCamera.Position;
						curRotation = MainCamera.Rotation;
						_instructionalButtonsScaleform = new Scaleform("instructional_buttons");
						UpdateScaleform();
						checkTimer = GetGameTimer();
						Client.Instance.AddTick(CreatorCameraControl);
						Screen.Fading.FadeIn(500);
					}
				}
				else if (state == MenuState.ChangeBackward)
				{
					if (oldmenu == selezionePunto)
					{
						Client.Instance.RemoveTick(CreatorCameraControl);
						Screen.Fading.FadeOut(800);
						while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
						await BaseScript.Delay(100);
						if (MainCamera.Exists() && World.RenderingCamera == MainCamera)
						{
							RenderScriptCams(false, false, 1000, false, false);
							MainCamera.IsActive = false;
						}
						SetFocusArea(GameplayCamera.Position.X, GameplayCamera.Position.Y, GameplayCamera.Position.Z, 0, 0, 0);
						SetPlayerControl(Game.Player.Handle, true, 256);
						Screen.Fading.FadeIn(500);
					}
				}
			};
			#endregion

			creazione.Visible = true;
		}

		private static void UpdateScaleform()
		{
			_instructionalButtonsScaleform.CallFunction("CLEAR_ALL");
			_instructionalButtonsScaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			_instructionalButtonsScaleform.CallFunction("CREATE_CONTAINER");

			InstructionalButton RS = new InstructionalButton(IsInputDisabled(2) ? Control.Cover : Control.FrontendLt, "Ruota sx");
			InstructionalButton RD = new InstructionalButton(IsInputDisabled(2) ? Control.HUDSpecial : Control.FrontendRt, "Ruota dx");
			InstructionalButton MuoviSD = new InstructionalButton(Control.MoveLeftRight, "Muovi laterale");
			InstructionalButton MuoviSG = new InstructionalButton(Control.MoveUpDown, "Muovi avanti / indietro");
			InstructionalButton GDS = new InstructionalButton(Control.LookLeftRight, "Guarda sx / dx");
			InstructionalButton GSG = new InstructionalButton(Control.LookUpDown, "Guarda su / giù");
			InstructionalButton Velocità = new InstructionalButton(Control.FrontendX, "Cambia velocità");

			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 0, RS.GetButtonId(), RS.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 1, RD.GetButtonId(), RD.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 2, MuoviSD.GetButtonId(), MuoviSD.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 3, MuoviSG.GetButtonId(), MuoviSG.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 4, GDS.GetButtonId(), GDS.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 5, GSG.GetButtonId(), GSG.Text);
			_instructionalButtonsScaleform.CallFunction("SET_DATA_SLOT", 6, Velocità.GetButtonId(), Velocità.Text);

			_instructionalButtonsScaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
		}

		static bool changed = false;
		private static async Task CreatorCameraControl()
		{

			HUD.DrawText(0.35f, 0.7f, "curLocation = " + curLocation, Colors.Cyan);
			HUD.DrawText(0.35f, 0.725f, "curRotation = " + curRotation, Colors.Orange);
			HUD.DrawText(0.35f, 0.75f, "IsInputJustDisabled(0) = " + IsInputJustDisabled(0), Colors.Yellow);
			HUD.DrawText(0.35f, 0.775f, "IsInputJustDisabled(2) = " + IsInputJustDisabled(2), Colors.Yellow);
			

			float forwardPush = 0.8f;
			if (GetGameTimer() - checkTimer > (int)Math.Ceiling(1000 / forwardPush))
				SetFocusPosAndVel(curLocation.X, curLocation.Y, curLocation.Z, 0, 0, 0);

			if (IsInputDisabled(2))
			{
				if (!changed)
				{
					changed = true;
					UpdateScaleform();
				}
			}
			else
			{
				if (changed)
				{
					changed = false;
					UpdateScaleform();
				}
			}
			if (!Main.ImpostazioniClient.ModCinema)
				_instructionalButtonsScaleform.Render2D();
			else
				DrawScaleformMovie(_instructionalButtonsScaleform.Handle, 0.5f, 0.5f - (Main.ImpostazioniClient.LetterBox / 1000), 1f, 1f, 255, 255, 255, 255, 0);
			HUD.ShowHelp("Velocità attuale: ~y~" + travelSpeedStr + "~w~.");


			Game.DisableAllControlsThisFrame(0);
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

			float rotationSpeed = 1.5f;

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
					forwardPush = 0.025f; //very slow
					travelSpeedStr = "Estremamente lenta";
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

			float xVect = forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z)) * -1.0f;
			float yVect = forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
			float zVect = forwardPush * (float)Math.Tan(Funzioni.Deg2rad(curRotation.X));
			if (zVect > 300) zVect = 300;
			if (Input.IsDisabledControlJustPressed(Control.FrontendX))
			{
				travelSpeed++;
				if (travelSpeed > 5)
					travelSpeed = 0;
			}

			if (Input.IsDisabledControlPressed(Control.Cover, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendLt, PadCheck.Controller))
			{
				curRotation.Y += rotationSpeed; // rotazione verso sinistra
				if (curRotation.Y > 179.999999999f) curRotation.Y = -180f;
			}
			if (Input.IsDisabledControlPressed(Control.HUDSpecial, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendRt, PadCheck.Controller))
			{
				curRotation.Y -= rotationSpeed; // rotazione verso destra
				if (curRotation.Y < -179.999999999f) curRotation.Y = 180f;
			}

			if (Input.IsDisabledControlPressed(Control.MoveUpOnly))
			{
				curLocation.X += xVect;
				curLocation.Y += yVect;
				float z = 0;
				GetGroundZFor_3dCoord(curLocation.X, curLocation.Y, curLocation.Z, ref z, false);
				if (z != 0 && curLocation.Z < z + 0.5f)
					curLocation.Z = z + 0.5f;
				else if (curLocation.Z > z + 0.5f)
					curLocation.Z += zVect;
			}
			if (Input.IsDisabledControlPressed(Control.MoveDownOnly))
			{
				curLocation.X -= xVect;
				curLocation.Y -= yVect;
				curLocation.Z -= zVect;
			}


			if (Input.IsControlPressed(Control.LookDownOnly, PadCheck.Controller))
			{
				curRotation.X -= rotationSpeed;
				if (curRotation.X < -75f) curRotation.X = -75f;
			}
			if (Input.IsControlPressed(Control.LookUpOnly, PadCheck.Controller))
			{
				curRotation.X += rotationSpeed;
				if (curRotation.X > 75f) curRotation.X = 75f;
			}
			if (Input.IsControlPressed(Control.LookLeftOnly, PadCheck.Controller))
			{
				curRotation.Z += rotationSpeed;
				if (curRotation.Z > 179.999999999f) curRotation.Z = -180f;
			}
			if (Input.IsControlPressed(Control.LookRightOnly, PadCheck.Controller))
			{
				curRotation.Z -= rotationSpeed;
				if (curRotation.Z < -179.999999999f) curRotation.Z = 180f;
			}
			MainCamera.Position = curLocation;
			MainCamera.Rotation = curRotation;
		}
	}
}
