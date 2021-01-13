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
		private enum TipoImmobile
		{
			Casa,
			Condominio,
			Ufficio,
			Garage
		}
		public static async void MenuCreazioneCase()
		{
			ConfigCase casaDummy = new ConfigCase();
			#region InstructionalButtons
		InstructionalButton MuoviSD = new InstructionalButton(Control.MoveLeftRight, "Muovi laterale");
			InstructionalButton MuoviSG = new InstructionalButton(Control.MoveUpDown, "Muovi avanti / indietro");
			InstructionalButton GDS = new InstructionalButton(Control.LookLeftRight, "Guarda sx / dx");
			InstructionalButton GSG = new InstructionalButton(Control.LookUpDown, "Guarda su / giù");
			InstructionalButton Sali = new InstructionalButton(Control.FrontendLt, "Sali");
			InstructionalButton Scendi = new InstructionalButton(Control.FrontendRt, "Scendi");
			InstructionalButton Velocità = new InstructionalButton(Control.FrontendX, "Cambia velocità");
			InstructionalButton BlipColoreDX = new InstructionalButton(Control.FrontendRb, "Colore Dx");
			InstructionalButton BlipColoreSX = new InstructionalButton(Control.FrontendLb, "Colore Sx");
			#endregion
			bool includiGarage = false;
			bool includiTetto = false;
			TipoImmobile immobile = TipoImmobile.Casa;

			#region dichiarazione
			UIMenu creazione = new UIMenu("Creatore Immobiliare", "Usare con cautela!", new PointF(1450f, 0));
			HUD.MenuPool.Add(creazione);
			creazione.MouseControlsEnabled = false;

			UIMenuListItem tipo = new UIMenuListItem("Tipo di immobile", new List<dynamic>() { "Casa", "Condominio", "Ufficio", "Garage" }, 0);
			creazione.AddItem(tipo);
			tipo.OnListChanged += (item, index) =>
			{
				immobile = (TipoImmobile)index;
			};

			UIMenu datiCasa = creazione.AddSubMenu("1. Dati dell'immobile"); // NB: nome provvisorio
			datiCasa.MouseControlsEnabled = false;
			

			UIMenu selezionePunto = creazione.AddSubMenu("2. Gestione esterni"); // NB: nome provvisorio
			selezionePunto.MouseControlsEnabled = false;
			selezionePunto.AddInstructionalButton(Velocità);
			selezionePunto.AddInstructionalButton(Scendi);
			selezionePunto.AddInstructionalButton(Sali);
			selezionePunto.AddInstructionalButton(GSG);
			selezionePunto.AddInstructionalButton(GDS);
			selezionePunto.AddInstructionalButton(MuoviSG);
			selezionePunto.AddInstructionalButton(MuoviSD);

			UIMenu gestioneInteriorCasa = creazione.AddSubMenu("3. Gestione interni"); // NB: nome provvisorio
			gestioneInteriorCasa.MouseControlsEnabled = false;

			#endregion

			#region Dati immobile
			UIMenuItem nomeImmobile = new UIMenuItem("Nome dell'immobile", "Sarebbe preferibile inserire la via e il numero civico nel nome, ma puoi decidere tu il nome come vuoi!");
			UIMenuItem nomeAbbreviato = new UIMenuItem("Nome Abbreviato", "Per questioni di salvataggio, serve un nome abbreviato per indicizzare l'immobile nel catabase del giocatore.. ad esempio: 0232 Paleto Boulevard => 0232PB");
			UIMenuCheckboxItem garageIncluso = new UIMenuCheckboxItem("Garage incluso", UIMenuCheckboxStyle.Tick, includiGarage, "Se attivato l'immobile avrà il garage incluso");
			UIMenuCheckboxItem tettoIncluso = new UIMenuCheckboxItem("Tetto incluso", UIMenuCheckboxStyle.Tick, includiTetto, "Se attivato l'immobile avrà il tetto raggiungibile");
			UIMenuItem prezzo = new UIMenuItem("Prezzo di vendita", "Inserisci un prezzo base di vendita, in modo che tutti abbiate un'idea di quanto costa comprandolo");



			datiCasa.AddItem(nomeImmobile);
			datiCasa.AddItem(nomeAbbreviato);
			datiCasa.AddItem(garageIncluso);
			datiCasa.AddItem(tettoIncluso);
			datiCasa.AddItem(prezzo);
			garageIncluso.CheckboxEvent += (item, _checked) =>
			{
				includiGarage = _checked;
				casaDummy.GarageIncluso = _checked;
			};
			tettoIncluso.CheckboxEvent += (item, _checked) =>
			{
				includiTetto = _checked;
				casaDummy.TettoIncluso = _checked;
			};
			datiCasa.OnItemSelect += async (menu, item, index) =>
			{
				if(item == nomeImmobile)
				{
					string valore = await HUD.GetUserInput("Nome dell'immobile", "", 30);
					item.SetRightLabel(valore.Length > 15 ? valore.Substring(0, 15) + "..." : valore);
					casaDummy.Label = valore;
				}
				else if (item == nomeAbbreviato)
				{
					string valore = await HUD.GetUserInput("Nome abbreviato", "", 7);
					item.SetRightLabel(valore);
				}
				else if (item == prezzo)
				{
					string valore = await HUD.GetUserInput("Prezzo", "", 10);
					item.SetRightLabel("$"+valore);
					casaDummy.Price = Convert.ToInt32(prezzo);
				}
			};
			#endregion

			#region Gestione esterni

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


			marker.OnItemSelect += (menu, item, index) =>
			{
				if (item == markerIngressoCasa)
				{
					markerIngrPiedi = new Marker(dummyMarker.MarkerType, dummyMarker.Position, Colors.Green);
					casaDummy.MarkerEntrata = markerIngrPiedi.Position;
				}
				else if (item == markerIngressoGarage)
				{
					markerIngrGarage = new Marker(dummyMarker.MarkerType, dummyMarker.Position, Colors.Green);
					casaDummy.MarkerGarageEsterno = markerIngrGarage.Position;
				}
				else if (item == markerIngressoTetto)
				{
					markerIngrTetto = new Marker(dummyMarker.MarkerType, dummyMarker.Position, Colors.Green);
					casaDummy.MarkerTetto = markerIngrTetto.Position;
				}
				else if (item == posCamera)
				{
					CameraPosIngresso = MainCamera.Position;
					CameraRotIngresso = MainCamera.CrosshairRaycast().HitPosition;
					casaDummy.TelecameraFuori.pos = CameraPosIngresso;
					casaDummy.TelecameraFuori.guarda = CameraRotIngresso;
				}
				item.MainColor = Colors.DarkGreen;
				item.HighlightColor = Colors.GreenLight;
				item.SetRightBadge(BadgeStyle.Star);
			};

			#endregion

			#endregion

			#region Gestione interni

			UIMenuListItem interior = new UIMenuListItem("Interno preferito", new List<dynamic>() { "low", "mid", "high" }, 0);
			gestioneInteriorCasa.AddItem(interior);
			interior.OnListChanged += async (item, index) =>
			{

			};
			#endregion

			#region StateChanged
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
						marker.Clear();
						switch (immobile)
						{
							case TipoImmobile.Casa:
							case TipoImmobile.Condominio:
							case TipoImmobile.Ufficio:
								marker.AddItem(markerIngressoCasa);
								marker.AddItem(markerIngressoGarage);
								marker.AddItem(markerIngressoTetto);
								marker.AddItem(posCamera);
								break;
							case TipoImmobile.Garage:
								marker.AddItem(markerIngressoGarage);
								marker.AddItem(posCamera);
								break;
						}
						Client.Instance.AddTick(MarkerTick);
						if (markerIngrPiedi == null)
							markerIngrPiedi = new Marker(MarkerType.VerticalCylinder, Vector3.Zero, new Vector3(1.5f), Colors.Red);
						if (markerIngrGarage == null) 
							 markerIngrGarage = new Marker(MarkerType.VerticalCylinder, Vector3.Zero, new Vector3(1.5f), Colors.Red);
						if (markerIngrTetto == null)
							 markerIngrTetto = new Marker(MarkerType.VerticalCylinder, Vector3.Zero, new Vector3(1.5f), Colors.Red);
					}
					else if(newmenu == gestioneInteriorCasa)
					{
						Screen.Fading.FadeOut(800);
						while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
						SetPlayerControl(Game.Player.Handle, false, 256);
						if (MainCamera == null)
							MainCamera = World.CreateCamera(Game.Player.GetPlayerData().posizione.ToVector3() + new Vector3(0, 0, 100), new Vector3(0, 0, 0), 45f);
						MainCamera.IsActive = true;
						RenderScriptCams(true, false, 1000, true, true);
						MainCamera.Position = new Vector3(266.8514f, -998.9061f, -97.92068f);
						MainCamera.PointAt(new Vector3(259.7751f, -998.6475f, -100.0068f));
						SetFocusPosAndVel(266.8514f, -998.9061f, -97.92068f, 0, 0, 0);
						Screen.Fading.FadeIn(500);
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
					else if (oldmenu == gestioneInteriorCasa)
					{
						Screen.Fading.FadeOut(800);
						while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
						if (MainCamera.Exists() && World.RenderingCamera == MainCamera)
						{
							RenderScriptCams(false, false, 1000, false, false);
							MainCamera.IsActive = false;
							MainCamera.Delete();
						}
						SetPlayerControl(Game.Player.Handle, true, 256);
						Screen.Fading.FadeIn(500);
					}
				}
			};
			#endregion

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
			 * 
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
