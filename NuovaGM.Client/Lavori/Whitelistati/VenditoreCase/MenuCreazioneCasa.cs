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
using System.Drawing;

namespace TheLastPlanet.Client.Lavori.Whitelistati.VenditoreCase
{
	static class MenuCreazioneCasa
	{
		private static ConfigCase casaFinale = new ConfigCase();
		private static Camera MainCamera;
		private static int travelSpeed = 0;
		private static Vector3 curLocation;
		private static Vector3 curRotation;
		private static Vector3 CameraPosIngresso;
		private static Vector3 CameraRotIngresso;
		private static string travelSpeedStr = "Media";
		private static int checkTimer = 0;
		private static Marker markerIngrPiedi;
		private static Marker markerIngrGarage;
		private static Marker markerIngrTetto;
		private static UIMenuItem markerIngressoCasa;
		private static UIMenuItem markerIngressoGarage;
		private static UIMenuItem markerIngressoTetto;
		private static UIMenuColorPanel blipColor;
		private static Marker dummyMarker = new Marker(MarkerType.VerticalCylinder, Vector3.Zero, new Vector3(1.5f), Colors.WhiteSmoke);

		public static async void MenuCreazioneCase()
		{
			InstructionalButton MuoviSD = new InstructionalButton(Control.MoveLeftRight, "Muovi laterale");
			InstructionalButton MuoviSG = new InstructionalButton(Control.MoveUpDown, "Muovi avanti / indietro");
			InstructionalButton GDS = new InstructionalButton(Control.LookLeftRight, "Guarda sx / dx");
			InstructionalButton GSG = new InstructionalButton(Control.LookUpDown, "Guarda su / giù");
			InstructionalButton Sali = new InstructionalButton(Control.FrontendLt, "Sali");
			InstructionalButton Scendi = new InstructionalButton(Control.FrontendRt, "Scendi");
			InstructionalButton Velocità = new InstructionalButton(Control.FrontendX, "Cambia velocità");

			InstructionalButton BlipColoreDX = new InstructionalButton(Control.FrontendRb, "Colore Dx");
			InstructionalButton BlipColoreSX = new InstructionalButton(Control.FrontendLb, "Colore Sx");

			UIMenu creazione = new UIMenu("Creatore Immobiliare", "Usare con cautela!", new PointF(1450f, 0));
			HUD.MenuPool.Add(creazione);
			creazione.MouseControlsEnabled = false;
			UIMenu selezionePunto = creazione.AddSubMenu("1. Gestione esterni"); // NB: nome provvisorio
			selezionePunto.MouseControlsEnabled = false;
			selezionePunto.AddInstructionalButton(Velocità);
			selezionePunto.AddInstructionalButton(Scendi);
			selezionePunto.AddInstructionalButton(Sali);
			selezionePunto.AddInstructionalButton(GSG);
			selezionePunto.AddInstructionalButton(GDS);
			selezionePunto.AddInstructionalButton(MuoviSG);
			selezionePunto.AddInstructionalButton(MuoviSD);
			UIMenu gestioneInteriorCasa = creazione.AddSubMenu("2. Gestione interni"); // NB: nome provvisorio
			gestioneInteriorCasa.MouseControlsEnabled = false;
			UIMenu datiCasa = creazione.AddSubMenu("3. Dati della casa"); // NB: nome provvisorio
			datiCasa.MouseControlsEnabled = false;

			// CONTINUARE (Finire gestione blip, marker.. )
			#region selezionePunto

			#region blip
			UIMenu blip = selezionePunto.AddSubMenu("Posiziona Blip");
			blip.AddInstructionalButton(Velocità);
			blip.AddInstructionalButton(Scendi);
			blip.AddInstructionalButton(Sali);
			blip.AddInstructionalButton(GSG);
			blip.AddInstructionalButton(GDS);
			blip.AddInstructionalButton(MuoviSG);
			blip.AddInstructionalButton(MuoviSD);
			blip.AddInstructionalButton(BlipColoreDX);
			blip.AddInstructionalButton(BlipColoreSX);
			
			blip.MouseControlsEnabled = false;
			UIMenuListItem blipType = new UIMenuListItem("Modello", new List<dynamic>() { "~BLIP_40~" }, 0);
			blipColor = new UIMenuColorPanel("Colore Blip", ColorPanelType.Hair);
			blipType.AddPanel(blipColor);
			UIMenuSliderProgressItem blipDimensions = new UIMenuSliderProgressItem("Dimensioni", 100, 0);
			UIMenuItem blipName = new UIMenuItem("Nome", "Se lasci il campo vuoto, prenderà il nome dell'abitazione automaticamente");
			blip.AddItem(blipType);
			blip.AddItem(blipDimensions);
			blip.AddItem(blipName);

			blipType.OnListChanged += (item, index) =>
			{
				// aggiungere salvataggio sprite blip (rimuovere blip_ dal nome e lasciare il numero)
				Color a = (item.Panels[0] as UIMenuColorPanel).CurrentColor;
				item._itemSprite.Color = a;
			};
			#endregion

			#region marker
			UIMenu marker = selezionePunto.AddSubMenu("Gestione markers");
			marker.MouseControlsEnabled = false;
			marker.AddInstructionalButton(Velocità);
			marker.AddInstructionalButton(Scendi);
			marker.AddInstructionalButton(Sali);
			marker.AddInstructionalButton(GSG);
			marker.AddInstructionalButton(GDS);
			marker.AddInstructionalButton(MuoviSG);
			marker.AddInstructionalButton(MuoviSD);

			markerIngressoCasa = new UIMenuItem("Punto di ingresso a piedi", "Il marker è puramente di guida, NON SARA' VISIBILE IN GIOCO", Colors.DarkRed, Colors.RedLight);
			markerIngressoGarage = new UIMenuItem("Punto di ingresso per il garage", "Il marker è puramente di guida, NON SARA' VISIBILE IN GIOCO", Colors.DarkRed, Colors.RedLight);
			markerIngressoTetto = new UIMenuItem("Punto di ingresso dal tetto", "Il marker è puramente di guida, NON SARA' VISIBILE IN GIOCO", Colors.DarkRed, Colors.RedLight);
			UIMenuItem posCamera = new UIMenuItem("Posizione della telecamera in anteprima", "Imposta la posizione e la rotazione della telecamera quando il cittadino torna a casa o citofona", Colors.DarkRed, Colors.RedLight);

			marker.AddItem(markerIngressoCasa);
			marker.AddItem(markerIngressoGarage);
			marker.AddItem(markerIngressoTetto);
			marker.AddItem(posCamera);

			marker.OnItemSelect += (menu, item, index) =>
			{
				if (item == markerIngressoCasa)
				{
					markerIngrPiedi = new Marker(dummyMarker.MarkerType, dummyMarker.Position, Colors.Green);
				}
				else if (item == markerIngressoGarage)
				{
					markerIngrGarage = new Marker(dummyMarker.MarkerType, dummyMarker.Position, Colors.Green);
				}
				else if (item == markerIngressoTetto)
				{
					markerIngrTetto = new Marker(dummyMarker.MarkerType, dummyMarker.Position, Colors.Green);
				}
				else if (item == posCamera)
				{
					CameraPosIngresso = MainCamera.Position;
					CameraRotIngresso = MainCamera.CrosshairRaycast().HitPosition;
				}
				item.MainColor = Colors.DarkGreen;
				item.HighlightColor = Colors.GreenLight;
				item.SetRightBadge(BadgeStyle.Star);
			};

			#endregion

			#endregion

			HUD.MenuPool.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
			{
				if(state == MenuState.Opened)
				{
					Game.Player.GetPlayerData().Istanza.Istanzia("Creatore Immobiliare");
				}
				else if(state == MenuState.Closed)
				{
					Game.Player.GetPlayerData().Istanza.RimuoviIstanza();
				}
				else if (state == MenuState.ChangeForward)
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
						checkTimer = GetGameTimer();
						Client.Instance.AddTick(CreatorCameraControl);
						Screen.Fading.FadeIn(500);
					}
					else if (newmenu == marker)
					{
						Client.Instance.AddTick(MarkerTick);
						if (markerIngrPiedi == null)
							markerIngrPiedi = new Marker(MarkerType.VerticalCylinder, Vector3.Zero, new Vector3(1.5f), Colors.Red);
						if (markerIngrGarage == null) 
							 markerIngrGarage = new Marker(MarkerType.VerticalCylinder, Vector3.Zero, new Vector3(1.5f), Colors.Red);
						if (markerIngrTetto == null)
							 markerIngrTetto = new Marker(MarkerType.VerticalCylinder, Vector3.Zero, new Vector3(1.5f), Colors.Red);
						// aggiungere tick di gestione marker (ho creato gli item Marker)
						// gestire Marker in base a dove punta la cam ma sempre per terra..
					}
				}
				else if (state == MenuState.ChangeBackward)
				{
					if(oldmenu == marker)
					{
						Client.Instance.RemoveTick(MarkerTick);
					}
					else if (oldmenu == selezionePunto)
					{
						Client.Instance.RemoveTick(CreatorCameraControl);
						Screen.Fading.FadeOut(800);
						while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
						ClearFocus();
						await BaseScript.Delay(100);
						if (MainCamera.Exists() && World.RenderingCamera == MainCamera)
						{
							RenderScriptCams(false, false, 1000, false, false);
							MainCamera.IsActive = false;
						}
						SetPlayerControl(Game.Player.Handle, true, 256);
						Screen.Fading.FadeIn(500);
					}
				}
			};
			creazione.Visible = true;
		}

		static bool changed = false;
		private static async Task CreatorCameraControl()
		{

			float forwardPush = 0.8f;
			if (GetGameTimer() - checkTimer > (int)Math.Ceiling(1000 / forwardPush))
				SetFocusPosAndVel(curLocation.X, curLocation.Y, curLocation.Z, 0, 0, 0);


			HUD.ShowHelp("Velocità attuale: ~y~" + travelSpeedStr + "~w~.");

			if (blipColor != null)
			{
				if (blipColor.ParentItem.Parent.Visible)
				{
					if (Input.IsControlJustPressed(Control.FrontendLb))
					{
						blipColor.GoLeft();
					}
					if (Input.IsControlJustPressed(Control.FrontendRb))
					{
						blipColor.GoRight();
					}
				}
			}

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

			float rotationSpeed = 1f;

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
					forwardPush = 2.6f; //very fast
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

			float zVect = forwardPush / 3;
			if (Input.IsDisabledControlJustPressed(Control.FrontendX))
			{
				travelSpeed++;
				if (travelSpeed > 5)
					travelSpeed = 0;
			}

			float xVectFwd = forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z)) * -1.0f;
			float yVectFwd = forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
			float xVectLat = forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
			float yVectLat = forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z));
			float z = 0;
			GetGroundZFor_3dCoord(curLocation.X, curLocation.Y, curLocation.Z, ref z, false);
			if (z != 0 && curLocation.Z < z + 0.5f)
				curLocation.Z = z + 0.5f;
			if (Input.IsDisabledControlPressed(Control.MoveUpOnly))
			{
				curLocation.X += xVectFwd;
				curLocation.Y += yVectFwd;
				if (z != 0 && curLocation.Z < z + 0.5f)
					curLocation.Z = z + 0.5f;
			}
			if (Input.IsDisabledControlPressed(Control.MoveDownOnly))
			{
				curLocation.X -= xVectFwd;
				curLocation.Y -= yVectFwd;
				if (z != 0 && curLocation.Z < z + 0.5f)
					curLocation.Z = z + 0.5f;
			}

			if (Input.IsDisabledControlPressed(Control.MoveLeftOnly))
			{
				curLocation.X -= xVectLat;
				curLocation.Y -= yVectLat;
				if (z != 0 && curLocation.Z < z + 0.5f)
					curLocation.Z = z + 0.5f;
			}
			if (Input.IsDisabledControlPressed(Control.MoveRightOnly))
			{
				curLocation.X += xVectLat;
				curLocation.Y += yVectLat;
				if (z != 0 && curLocation.Z < z + 0.5f)
					curLocation.Z = z + 0.5f;
			}
			if (Input.IsControlPressed(Control.FrontendLt))
			{
				curLocation.Z += zVect;
				if (z != 0 && curLocation.Z > 300)
					curLocation.Z = 300;
						
			}
			if (Input.IsControlPressed(Control.FrontendRt))
			{
				curLocation.Z -= zVect;
				if (curLocation.Z > z + 0.5f)
					curLocation.Z -= zVect;
			}

			/* Ci serve davvero ruotare la telecamera? :thinking:
			if (Input.IsDisabledControlPressed(Control.Cover, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendLb, PadCheck.Controller))
			{
				curRotation.Y += rotationSpeed; // rotazione verso sinistra
				if (curRotation.Y > 179.999999999f) curRotation.Y = -180f;
			}
			if (Input.IsDisabledControlPressed(Control.HUDSpecial, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendRb, PadCheck.Controller))
			{
				curRotation.Y -= rotationSpeed; // rotazione verso destra
				if (curRotation.Y < -179.999999999f) curRotation.Y = 180f;
			}
			*/
			if (!IsInputDisabled(2))
			{
				if (Input.IsControlPressed(Control.LookDownOnly))
				{
					curRotation.X -= rotationSpeed;
					if (curRotation.X < -75f) curRotation.X = -75f;
				}
				if (Input.IsControlPressed(Control.LookUpOnly))
				{
					curRotation.X += rotationSpeed;
					if (curRotation.X > 75f) curRotation.X = 75f;
				}
				if (Input.IsControlPressed(Control.LookLeftOnly))
				{
					curRotation.Z += rotationSpeed;
					if (curRotation.Z > 179.999999999f) curRotation.Z = -180f;
				}
				if (Input.IsControlPressed(Control.LookRightOnly))
				{
					curRotation.Z -= rotationSpeed;
					if (curRotation.Z < -179.999999999f) curRotation.Z = 180f;
				}
			}
			else
			{
				curRotation.X -= (GetDisabledControlNormal(1, 2) * rotationSpeed * 8.0f);
				if (curRotation.X < -75f) curRotation.X = -75f;
				if (curRotation.X > 75f) curRotation.X = 75f;
				curRotation.Z -= (GetDisabledControlNormal(1, 1) * rotationSpeed * 8.0f);
				if (curRotation.Z > 179.999999999f) curRotation.Z = -180f;
				if (curRotation.Z < -179.999999999f) curRotation.Z = 180f;
			}
			MainCamera.Position = curLocation;
			MainCamera.Rotation = curRotation;
		}

		private static async Task MarkerTick()
		{
			RaycastResult res = MainCamera.CrosshairRaycast(150f);
			Vector3 direction = res.HitPosition;
			dummyMarker.Color = Colors.Red;
			dummyMarker.Draw();
			dummyMarker.Position = direction;
			float z = 0;
			GetGroundZFor_3dCoord(direction.X, direction.Y, direction.Z, ref z, false);
			if (z != 0 && res.DitHit)
				dummyMarker.Position.Z = z;
			if (markerIngressoCasa.Selected)
			{
				if (markerIngressoCasa.RightBadge != BadgeStyle.None)
					markerIngrPiedi.Draw();
			}
			if (markerIngressoGarage.Selected)
			{
				if (markerIngressoGarage.RightBadge != BadgeStyle.None)
					markerIngrGarage.Draw();
			}
			if (markerIngressoTetto.Selected)
			{
				if (markerIngressoTetto.RightBadge != BadgeStyle.None)
					markerIngrTetto.Draw();
			}
		}
	}
}
