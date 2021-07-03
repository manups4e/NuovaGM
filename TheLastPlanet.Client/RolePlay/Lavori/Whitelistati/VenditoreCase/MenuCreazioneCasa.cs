using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Shared;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core;
using Logger;
using System.Drawing;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.SessionCache;

namespace TheLastPlanet.Client.RolePlay.Lavori.Whitelistati.VenditoreCase
{
	internal static class MenuCreazioneCasa
	{
		private static Camera MainCamera;
		private static int travelSpeed = 0;
		private static Position curLocation;
		private static Position curRotation;
		private static Position CameraPosIngresso;
		private static Position CameraRotIngresso;
		private static string travelSpeedStr = "Media";
		private static int checkTimer = 0;
		private static Marker markerIngrPiedi;
		private static Marker markerIngrGarage;
		private static Marker markerIngrTetto;
		private static UIMenuItem markerIngressoCasa;
		private static UIMenuItem markerIngressoGarage;
		private static UIMenuItem markerIngressoTetto;
		private static UIMenuItem posCamera;
		private static UIMenuColorPanel blipColor;
		private static Prop renderCamObject;
		private static Marker dummyMarker = new(MarkerType.VerticalCylinder, Position.Zero, new Position(1.5f), Colors.WhiteSmoke);
		private static int interno = 0;
		private static string abbreviazione;

		private enum TipoImmobile
		{
			Casa,
			Garage
		}

		public static async void MenuCreazioneCase()
		{
			MainCamera = null;
			travelSpeed = 0;
			curLocation = Position.Zero;
			curRotation = Position.Zero;
			CameraPosIngresso = Position.Zero;
			CameraRotIngresso = Position.Zero;
			travelSpeedStr = "Media";
			checkTimer = 0;
			markerIngrPiedi = null;
			markerIngrGarage = null;
			markerIngrTetto = null;
			markerIngressoCasa = null;
			markerIngressoGarage = null;
			markerIngressoTetto = null;
			posCamera = null;
			blipColor = null;
			renderCamObject = null;
			dummyMarker = new Marker(MarkerType.VerticalCylinder, Position.Zero, new Position(1.5f), Colors.WhiteSmoke);
			interno = 0;
			abbreviazione = "";
			Istanza oldInstance = new();
			ConfigCase casaDummy = new();
			casaDummy.VehCapacity = 2;
			Garages garageDummy = new();
			bool includiGarage = false;
			bool includiTetto = false;
			TipoImmobile immobile = TipoImmobile.Casa;

			#region dichiarazione

			UIMenu creazione = new("Creatore Immobiliare", "Usare con cautela!", new PointF(1450f, 0));
			HUD.MenuPool.Add(creazione);
			creazione.MouseControlsEnabled = false;
			UIMenuListItem tipo = new("Tipo di immobile", new List<dynamic>() { "Casa", "Garage" }, 0);
			creazione.AddItem(tipo);
			tipo.OnListChanged += (item, index) => immobile = (TipoImmobile)index;
			UIMenu datiCasa = creazione.AddSubMenu("1. Dati dell'immobile"); // NB: nome provvisorio
			datiCasa.MouseControlsEnabled = false;
			UIMenu selezionePunto = creazione.AddSubMenu("2. Gestione esterni"); // NB: nome provvisorio
			selezionePunto.MouseControlsEnabled = false;
			UIMenu gestioneInteriorCasa = creazione.AddSubMenu("3. Gestione interni"); // NB: nome provvisorio
			gestioneInteriorCasa.MouseControlsEnabled = false;
			UIMenu opzioniInterior = null;

			#endregion

			#region Dati immobile

			datiCasa.OnMenuStateChanged += async (_old, _new, _state) =>
			{
				if (_new == datiCasa)
				{
					datiCasa.Clear();
					UIMenuItem nomeImmobile = new("Nome dell'immobile", "Sarebbe preferibile inserire la via e il numero civico nel nome, ma puoi decidere tu il nome come vuoi!");
					UIMenuItem nomeAbbreviato = new("Nome Abbreviato", "Per questioni di salvataggio, serve un nome abbreviato per indicizzare l'immobile nel catabase del giocatore.. ad esempio: 0232 Paleto Boulevard => 0232PB");

					switch (immobile)
					{
						case TipoImmobile.Casa:
						{
							if (casaDummy.Label != null) nomeAbbreviato.SetRightLabel(abbreviazione);

							break;
						}
						case TipoImmobile.Garage:
						{
							if (garageDummy.Label != null)
							{
								nomeImmobile.SetRightLabel(garageDummy.Label.Length > 15 ? garageDummy.Label.Substring(0, 15) + "..." : garageDummy.Label);
								nomeAbbreviato.SetRightLabel(abbreviazione);
							}

							break;
						}
					}

					datiCasa.AddItem(nomeImmobile);
					datiCasa.AddItem(nomeAbbreviato);

					if (immobile == TipoImmobile.Casa)
					{
						UIMenuCheckboxItem garageIncluso = new("Garage incluso", UIMenuCheckboxStyle.Tick, includiGarage, "Se attivato l'immobile avrà il garage incluso");
						UIMenuCheckboxItem tettoIncluso = new("Tetto incluso", UIMenuCheckboxStyle.Tick, includiTetto, "Se attivato l'immobile avrà il tetto raggiungibile");
						datiCasa.AddItem(garageIncluso);
						datiCasa.AddItem(tettoIncluso);
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
					}

					UIMenuItem prezzo = new("Prezzo di vendita", "Inserisci un prezzo base di vendita, in modo che tutti abbiate un'idea di quanto costa comprandolo");

					switch (immobile)
					{
						case TipoImmobile.Casa:
							prezzo.SetRightLabel("$" + casaDummy.Price);

							break;
						case TipoImmobile.Garage:
							prezzo.SetRightLabel("$" + garageDummy.Price);

							break;
					}

					datiCasa.AddItem(prezzo);
					datiCasa.OnItemSelect += async (menu, item, index) =>
					{
						if (item == nomeImmobile)
						{
							string valore = await HUD.GetUserInput("Nome dell'immobile", "", 30);
							item.SetRightLabel(valore.Length > 15 ? valore.Substring(0, 15) + "..." : valore);

							switch (immobile)
							{
								case TipoImmobile.Casa:
									casaDummy.Label = valore;

									break;
								case TipoImmobile.Garage:
									garageDummy.Label = valore;

									break;
							}
						}
						else if (item == nomeAbbreviato)
						{
							string valore = await HUD.GetUserInput("Nome abbreviato", "", 7);
							item.SetRightLabel(valore);
							abbreviazione = valore;
						}
						else if (item == prezzo)
						{
							string valore = await HUD.GetUserInput("Prezzo", "", 10);
							item.SetRightLabel("$" + valore);

							switch (immobile)
							{
								case TipoImmobile.Casa:
									casaDummy.Price = Convert.ToInt32(valore);

									break;
								case TipoImmobile.Garage:
									garageDummy.Price = Convert.ToInt32(valore);

									break;
							}
						}
					};
				}
			};

			#endregion

			#region Gestione esterni

			/*
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
			*/

			#region marker

			UIMenu marker = selezionePunto.AddSubMenu("Gestione markers");
			marker.MouseControlsEnabled = false;
			marker.SetKey(UIMenu.MenuControls.Select, Control.Attack);
			markerIngressoCasa = new UIMenuItem("Punto di ingresso a piedi", "Il marker è puramente di guida, NON SARA' VISIBILE IN GIOCO", Colors.DarkRed, Colors.RedLight);
			markerIngressoGarage = new UIMenuItem("Punto di ingresso per il garage", "Il marker è puramente di guida, NON SARA' VISIBILE IN GIOCO", Colors.DarkRed, Colors.RedLight);
			markerIngressoTetto = new UIMenuItem("Punto di ingresso dal tetto", "Il marker è puramente di guida, NON SARA' VISIBILE IN GIOCO", Colors.DarkRed, Colors.RedLight);
			posCamera = new UIMenuItem("Posizione della telecamera in anteprima", "Imposta la posizione e la rotazione della telecamera quando il cittadino torna a casa o citofona", Colors.DarkRed, Colors.RedLight);
			marker.OnItemSelect += async (menu, item, index) =>
			{
				if (item == markerIngressoCasa)
				{
					markerIngrPiedi = new Marker(dummyMarker.MarkerType, dummyMarker.Position, Colors.Green);

					if (immobile == TipoImmobile.Casa)
					{
						casaDummy.MarkerEntrata = markerIngrPiedi.Position;
						casaDummy.SpawnFuori = markerIngrPiedi.Position;
					}
				}
				else if (item == markerIngressoGarage)
				{
					markerIngrGarage = new Marker(dummyMarker.MarkerType, dummyMarker.Position, Colors.Green);

					switch (immobile)
					{
						case TipoImmobile.Casa:
							casaDummy.MarkerGarageEsterno = markerIngrGarage.Position;
							casaDummy.SpawnGarageInVehFuori = new Position(markerIngrGarage.Position, 0);

							break;
						case TipoImmobile.Garage:
							garageDummy.MarkerEntrata = markerIngrGarage.Position;
							garageDummy.SpawnFuori = new Position(markerIngrGarage.Position, 0);

							break;
					}
				}
				else if (item == markerIngressoTetto)
				{
					markerIngrTetto = new Marker(dummyMarker.MarkerType, dummyMarker.Position, Colors.Green);

					if (immobile == TipoImmobile.Casa)
					{
						casaDummy.MarkerTetto = markerIngrTetto.Position;
						casaDummy.SpawnTetto = markerIngrTetto.Position;
					}
				}
				else if (item == posCamera)
				{
					CameraPosIngresso = MainCamera.Position.ToPosition();
					CameraRotIngresso = (await MainCamera.CrosshairRaycast(1000)).HitPosition.ToPosition();

					switch (immobile)
					{
						case TipoImmobile.Casa:
							casaDummy.TelecameraFuori.pos = CameraPosIngresso;
							casaDummy.TelecameraFuori.guarda = CameraRotIngresso;

							break;
						case TipoImmobile.Garage:
							garageDummy.TelecameraFuori.pos = CameraPosIngresso;
							garageDummy.TelecameraFuori.guarda = CameraRotIngresso;

							break;
					}
				}

				item.MainColor = Colors.DarkGreen;
				item.HighlightColor = Colors.GreenLight;
				item.SetRightBadge(BadgeStyle.Star);
			};

			#endregion

			#endregion

			#region Gestione interni

			gestioneInteriorCasa.OnMenuStateChanged += async (_old, _new, _state) =>
			{
				UIMenuListItem interior = new("", new List<dynamic>() { "" }, 0);

				if (_new == gestioneInteriorCasa)
				{
					if (_state == MenuState.ChangeForward)
					{
						_new.Clear();
						Screen.Fading.FadeOut(800);
						while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
						SetPlayerControl(Cache.MyPlayer.Player.Handle, false, 256);

						switch (immobile)
						{
							case TipoImmobile.Casa:
							{
								interior = new UIMenuListItem("Interno preferito", new List<dynamic>()
								{
									"Base",
									"Medio",
									"Alto [HighLife]",
									"Alto [2 piani]",
									"Alto [3 piani]",
									"Alto [Executive]"
								}, 0);

								if (includiGarage)
								{
									UIMenuListItem garageInterior = new("Tipo di Garage", new List<dynamic>() { "Base", "Medio [4]", "Medio [6]", "Alto [10]" }, 0);
									gestioneInteriorCasa.AddItem(garageInterior);
									garageInterior.OnListChanged += (item, index) =>
									{
										switch (index)
										{
											case 0:
												casaDummy.VehCapacity = 2;

												break;
											case 1:
												casaDummy.VehCapacity = 4;

												break;
											case 2:
												casaDummy.VehCapacity = 6;

												break;
											case 3:
												casaDummy.VehCapacity = 10;

												break;
										}
									};
								}

								break;
							}
							case TipoImmobile.Garage:
								interior = new UIMenuListItem("Interno preferito", new List<dynamic>() { "Base", "Medio [4]", "Medio [6]", "Alto [10]" }, 0);

								break;
						}

						gestioneInteriorCasa.AddItem(interior);
						if (immobile == TipoImmobile.Casa) opzioniInterior = gestioneInteriorCasa.AddSubMenu("Opzioni interno selezionato");
						if (MainCamera == null) MainCamera = World.CreateCamera(Cache.MyPlayer.User.Posizione.ToVector3 + new Vector3(0, 0, 100), new Vector3(0, 0, 0), 45f);
						MainCamera.IsActive = true;
						RenderScriptCams(true, false, 1000, true, true);
						if (renderCamObject == null) renderCamObject = await Funzioni.SpawnLocalProp("prop_ld_test_01", Vector3.Zero, false, false);
						renderCamObject.IsVisible = false;

						switch (immobile)
						{
							case TipoImmobile.Casa:
								SetFocusEntity(renderCamObject.Handle);
								renderCamObject.Position = new Vector3(266.8514f, -998.9061f, -97.92068f);
								PlaceObjectOnGroundProperly(renderCamObject.Handle);
								MainCamera.Position = new Vector3(266.8514f, -998.9061f, -97.92068f);
								MainCamera.PointAt(new Vector3(259.7751f, -998.6475f, -100.0068f));
								casaDummy.MarkerUscita = new Position(266.094f, -1007.487f, -101.800f);
								casaDummy.SpawnDentro = new Position(266.094f, -1007.487f, -101.800f);

								break;
							case TipoImmobile.Garage:
								SetFocusEntity(renderCamObject.Handle);
								renderCamObject.Position = new Vector3(177.8964f, -1008.719f, -98.03687f);
								PlaceObjectOnGroundProperly(renderCamObject.Handle);
								MainCamera.Position = new Vector3(177.8964f, -1008.719f, -98.03687f);
								MainCamera.PointAt(new Vector3(168.3609f, -1002.193f, -99.99992f));

								break;
						}

						Screen.Fading.FadeIn(500);
					}

					interior.OnListChanged += async (item, index) =>
					{
						Screen.Fading.FadeOut(800);
						while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
						if (renderCamObject == null) renderCamObject = await Funzioni.SpawnLocalProp("prop_ld_test_01", Vector3.Zero, false, false);
						renderCamObject.IsVisible = false;
						if (MainCamera == null) MainCamera = World.CreateCamera(Cache.MyPlayer.User.Posizione.ToVector3 + new Vector3(0, 0, 100), new Vector3(0, 0, 0), 45f);
						MainCamera.IsActive = true;
						RenderScriptCams(true, false, 1000, true, true);
						Vector3 pos = Vector3.Zero;
						Vector3 lookAt = Vector3.Zero;
						interno = index;

						switch (immobile)
						{
							case TipoImmobile.Casa:
								casaDummy.Tipo = index;

								break;
							case TipoImmobile.Garage:
								garageDummy.tipo = index;

								break;
						}

						switch (index)
						{
							case 0:
								switch (immobile)
								{
									case TipoImmobile.Casa:
										pos = new Vector3(266.8514f, -998.9061f, -97.92068f);
										lookAt = new Vector3(259.7751f, -998.6475f, -100.0068f);
										casaDummy.MarkerUscita = new Position(266.094f, -1007.487f, -101.800f);
										casaDummy.SpawnDentro = new Position(266.094f, -1007.487f, -101.800f);

										break;
									case TipoImmobile.Garage:
										pos = new Vector3(177.8964f, -1008.719f, -98.03687f);
										lookAt = new Vector3(168.3609f, -1002.193f, -99.99992f);
										garageDummy.SpawnDentro = new Position(179.015f, -1000.326f, -100f);
										garageDummy.MarkerUscita = new Position(179.015f, -1000.326f, -100f);

										break;
								}

								break;
							case 1:
								switch (immobile)
								{
									case TipoImmobile.Casa:
										pos = new Vector3(339.3684f, -992.7239f, -98.21723f);
										lookAt = new Vector3(341.4973f, -999.5391f, -100.1962f);
										casaDummy.MarkerUscita = new Position(346.493f, -1013.031f, -99.196f);
										casaDummy.SpawnDentro = new Position(346.493f, -1013.031f, -99.196f);

										break;
									case TipoImmobile.Garage:
										pos = new Vector3(190.6334f, -1027.276f, -98.94763f);
										lookAt = new Vector3(193.8157f, -1024.415f, -99.99996f);
										garageDummy.SpawnDentro = new Position(207.1461f, -1018.326f, -98.999f);
										garageDummy.MarkerUscita = new Position(207.1461f, -1018.326f, -98.999f);

										break;
								}

								break;
							case 2:
								switch (immobile)
								{
									case TipoImmobile.Casa:
										pos = new Vector3(-1465.857f, -535.3416f, 74.20998f);
										lookAt = new Vector3(-1467.427f, -544.514f, 72.46823f);
										casaDummy.SpawnDentro = new Position(-1452.841f, -539.489f, 74.044f);
										casaDummy.MarkerUscita = new Position(-1452.164f, -540.640f, 74.044f);

										break;
									case TipoImmobile.Garage:
										pos = new Vector3(206.7423f, -993.4413f, -98.09858f);
										lookAt = new Vector3(190.6937f, -1008.027f, -99.62811f);
										garageDummy.SpawnDentro = new Position(210.759f, -999.0323f, -98.99997f);
										garageDummy.MarkerUscita = new Position(210.759f, -999.0323f, -98.99997f);

										break;
								}

								break;
							case 3:
								switch (immobile)
								{
									case TipoImmobile.Casa:
										pos = new Vector3(-42.78862f, -571.4902f, 89.38699f);
										lookAt = new Vector3(-35.83893f, -583.5001f, 88.47382f);
										casaDummy.SpawnDentro = new Position(-17.54766f, -589.1531f, 90.11485f);
										casaDummy.MarkerUscita = new Position(-17.54766f, -589.1531f, 90.11485f);

										break;
									case TipoImmobile.Garage:
										pos = new Vector3(220.5728f, -1007.01f, -98.10276f);
										lookAt = new Vector3(225.9477f, -996.6439f, -99.9992f);
										garageDummy.SpawnDentro = new Position(238.103f, -1004.813f, -98.99992f);
										garageDummy.MarkerUscita = new Position(238.103f, -1004.813f, -98.99992f);

										break;
								}

								break;
							case 4:
								if (immobile == TipoImmobile.Casa)
								{
									pos = new Vector3(-169.7948f, 478.3921f, 138.4392f);
									lookAt = new Vector3(-166.9105f, 485.8192f, 136.8266f);
									casaDummy.SpawnDentro = new Position(-173.9128f, 496.8375f, 137.667f);
									casaDummy.MarkerUscita = new Position(-173.9128f, 496.8375f, 137.667f);
								}

								break;
							case 5:
								if (immobile == TipoImmobile.Casa)
								{
									pos = new Vector3(-791.5707f, 343.7827f, 217.8111f);
									lookAt = new Vector3(-784.6417f, 330.4529f, 216.0382f);
									casaDummy.SpawnDentro = new Position(-786.5125f, 315.8108f, 217.6385f);
									casaDummy.MarkerUscita = new Position(-786.5125f, 315.8108f, 217.6385f);
								}

								break;
						}

						renderCamObject.Position = pos;
						PlaceObjectOnGroundProperly(renderCamObject.Handle);
						MainCamera.Position = pos;
						MainCamera.PointAt(lookAt);
						await BaseScript.Delay(1000); // non voglio che i giocatori si ritrovino ad aspettare che carica.. così aspettano a schermo nero
						Screen.Fading.FadeIn(500);
					};
				}
				else if (_new == opzioniInterior && _state == MenuState.ChangeForward)
				{
					_new.Clear();

					switch (interno)
					{
						#region Low

						case 0: // low
							if (immobile == TipoImmobile.Casa)
							{
								UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
								UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
								UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
								_new.AddItem(strip);
								_new.AddItem(booze);
								_new.AddItem(smoke);
								_new.OnCheckboxChange += async (menu, item, check) =>
								{
									Screen.Fading.FadeOut(250);
									await BaseScript.Delay(300);

									if (item == strip)
									{
										casaDummy.Strip = check;
										IPLs.gta_online.GTAOHouseLow1.Strip.Enable(IPLs.gta_online.GTAOHouseLow1.Strip.A, check, true);
										IPLs.gta_online.GTAOHouseLow1.Strip.Enable(IPLs.gta_online.GTAOHouseLow1.Strip.B, check, true);
										IPLs.gta_online.GTAOHouseLow1.Strip.Enable(IPLs.gta_online.GTAOHouseLow1.Strip.C, check, true);
									}
									else if (item == booze)
									{
										casaDummy.Booze = check;
										IPLs.gta_online.GTAOHouseLow1.Booze.Enable(IPLs.gta_online.GTAOHouseLow1.Booze.A, check, true);
										IPLs.gta_online.GTAOHouseLow1.Booze.Enable(IPLs.gta_online.GTAOHouseLow1.Booze.B, check, true);
										IPLs.gta_online.GTAOHouseLow1.Booze.Enable(IPLs.gta_online.GTAOHouseLow1.Booze.C, check, true);
									}
									else if (item == smoke)
									{
										casaDummy.Smoke = check;
										IPLs.gta_online.GTAOHouseLow1.Smoke.Enable(IPLs.gta_online.GTAOHouseLow1.Smoke.A, check, true);
										IPLs.gta_online.GTAOHouseLow1.Smoke.Enable(IPLs.gta_online.GTAOHouseLow1.Smoke.B, check, true);
										IPLs.gta_online.GTAOHouseLow1.Smoke.Enable(IPLs.gta_online.GTAOHouseLow1.Smoke.C, check, true);
									}

									await BaseScript.Delay(500);
									Screen.Fading.FadeIn(500);
								};
							}

							break;

						#endregion

						#region Mid

						case 1:
							if (immobile == TipoImmobile.Casa)
							{
								UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
								UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
								UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
								_new.AddItem(strip);
								_new.AddItem(booze);
								_new.AddItem(smoke);
								_new.OnCheckboxChange += async (menu, item, check) =>
								{
									Screen.Fading.FadeOut(250);
									await BaseScript.Delay(300);

									if (item == strip)
									{
										casaDummy.Strip = check;
										IPLs.gta_online.GTAOHouseMid1.Strip.Enable(IPLs.gta_online.GTAOHouseMid1.Strip.A, check, true);
										IPLs.gta_online.GTAOHouseMid1.Strip.Enable(IPLs.gta_online.GTAOHouseMid1.Strip.B, check, true);
										IPLs.gta_online.GTAOHouseMid1.Strip.Enable(IPLs.gta_online.GTAOHouseMid1.Strip.C, check, true);
									}
									else if (item == booze)
									{
										casaDummy.Booze = check;
										IPLs.gta_online.GTAOHouseMid1.Booze.Enable(IPLs.gta_online.GTAOHouseMid1.Booze.A, check, true);
										IPLs.gta_online.GTAOHouseMid1.Booze.Enable(IPLs.gta_online.GTAOHouseMid1.Booze.B, check, true);
										IPLs.gta_online.GTAOHouseMid1.Booze.Enable(IPLs.gta_online.GTAOHouseMid1.Booze.C, check, true);
									}
									else if (item == smoke)
									{
										casaDummy.Smoke = check;
										IPLs.gta_online.GTAOHouseMid1.Smoke.Enable(IPLs.gta_online.GTAOHouseMid1.Smoke.A, check, true);
										IPLs.gta_online.GTAOHouseMid1.Smoke.Enable(IPLs.gta_online.GTAOHouseMid1.Smoke.B, check, true);
										IPLs.gta_online.GTAOHouseMid1.Smoke.Enable(IPLs.gta_online.GTAOHouseMid1.Smoke.C, check, true);
									}

									await BaseScript.Delay(500);
									Screen.Fading.FadeIn(500);
								};
							}

							break;

						#endregion

						#region HighLife

						case 2:
							if (immobile == TipoImmobile.Casa)
							{
								UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
								UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
								UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
								_new.AddItem(strip);
								_new.AddItem(booze);
								_new.AddItem(smoke);
								_new.OnCheckboxChange += async (menu, item, check) =>
								{
									Screen.Fading.FadeOut(250);
									await BaseScript.Delay(300);

									if (item == strip)
									{
										casaDummy.Strip = check;
										IPLs.gta_online.HLApartment1.Strip.Enable(IPLs.gta_online.HLApartment1.Strip.A, check, true);
										IPLs.gta_online.HLApartment1.Strip.Enable(IPLs.gta_online.HLApartment1.Strip.B, check, true);
										IPLs.gta_online.HLApartment1.Strip.Enable(IPLs.gta_online.HLApartment1.Strip.C, check, true);
									}
									else if (item == booze)
									{
										casaDummy.Booze = check;
										IPLs.gta_online.HLApartment1.Booze.Enable(IPLs.gta_online.HLApartment1.Booze.A, check, true);
										IPLs.gta_online.HLApartment1.Booze.Enable(IPLs.gta_online.HLApartment1.Booze.B, check, true);
										IPLs.gta_online.HLApartment1.Booze.Enable(IPLs.gta_online.HLApartment1.Booze.C, check, true);
									}
									else if (item == smoke)
									{
										casaDummy.Smoke = check;
										IPLs.gta_online.HLApartment1.Smoke.Enable(IPLs.gta_online.HLApartment1.Smoke.A, check, true);
										IPLs.gta_online.HLApartment1.Smoke.Enable(IPLs.gta_online.HLApartment1.Smoke.B, check, true);
										IPLs.gta_online.HLApartment1.Smoke.Enable(IPLs.gta_online.HLApartment1.Smoke.C, check, true);
									}

									await BaseScript.Delay(500);
									Screen.Fading.FadeIn(500);
								};
							}

							break;

						#endregion

						#region OnlineHi

						case 3:
							if (immobile == TipoImmobile.Casa)
							{
								UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
								UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
								UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
								_new.AddItem(strip);
								_new.AddItem(booze);
								_new.AddItem(smoke);
								_new.OnCheckboxChange += async (menu, item, check) =>
								{
									Screen.Fading.FadeOut(250);
									await BaseScript.Delay(300);

									if (item == strip)
									{
										casaDummy.Strip = check;
										IPLs.gta_online.GTAOApartmentHi1.Strip.Enable(IPLs.gta_online.GTAOApartmentHi1.Strip.A, check, true);
										IPLs.gta_online.GTAOApartmentHi1.Strip.Enable(IPLs.gta_online.GTAOApartmentHi1.Strip.B, check, true);
										IPLs.gta_online.GTAOApartmentHi1.Strip.Enable(IPLs.gta_online.GTAOApartmentHi1.Strip.C, check, true);
									}
									else if (item == booze)
									{
										casaDummy.Booze = check;
										IPLs.gta_online.GTAOApartmentHi1.Booze.Enable(IPLs.gta_online.GTAOApartmentHi1.Booze.A, check, true);
										IPLs.gta_online.GTAOApartmentHi1.Booze.Enable(IPLs.gta_online.GTAOApartmentHi1.Booze.B, check, true);
										IPLs.gta_online.GTAOApartmentHi1.Booze.Enable(IPLs.gta_online.GTAOApartmentHi1.Booze.C, check, true);
									}
									else if (item == smoke)
									{
										casaDummy.Smoke = check;
										IPLs.gta_online.GTAOApartmentHi1.Smoke.Enable(IPLs.gta_online.GTAOApartmentHi1.Smoke.A, check, true);
										IPLs.gta_online.GTAOApartmentHi1.Smoke.Enable(IPLs.gta_online.GTAOApartmentHi1.Smoke.B, check, true);
										IPLs.gta_online.GTAOApartmentHi1.Smoke.Enable(IPLs.gta_online.GTAOApartmentHi1.Smoke.C, check, true);
									}

									await BaseScript.Delay(500);
									Screen.Fading.FadeIn(500);
								};
							}

							break;

						#endregion

						#region OnlineHouseHi

						case 4:
							if (immobile == TipoImmobile.Casa)
							{
								UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
								UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
								UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
								_new.AddItem(strip);
								_new.AddItem(booze);
								_new.AddItem(smoke);
								_new.OnCheckboxChange += async (menu, item, check) =>
								{
									Screen.Fading.FadeOut(250);
									await BaseScript.Delay(300);

									if (item == strip)
									{
										casaDummy.Strip = check;
										IPLs.gta_online.GTAOHouseHi1.Strip.Enable(IPLs.gta_online.GTAOHouseHi1.Strip.A, check, true);
										IPLs.gta_online.GTAOHouseHi1.Strip.Enable(IPLs.gta_online.GTAOHouseHi1.Strip.B, check, true);
										IPLs.gta_online.GTAOHouseHi1.Strip.Enable(IPLs.gta_online.GTAOHouseHi1.Strip.C, check, true);
									}
									else if (item == booze)
									{
										casaDummy.Booze = check;
										IPLs.gta_online.GTAOHouseHi1.Booze.Enable(IPLs.gta_online.GTAOHouseHi1.Booze.A, check, true);
										IPLs.gta_online.GTAOHouseHi1.Booze.Enable(IPLs.gta_online.GTAOHouseHi1.Booze.B, check, true);
										IPLs.gta_online.GTAOHouseHi1.Booze.Enable(IPLs.gta_online.GTAOHouseHi1.Booze.C, check, true);
									}
									else if (item == smoke)
									{
										casaDummy.Smoke = check;
										IPLs.gta_online.GTAOHouseHi1.Smoke.Enable(IPLs.gta_online.GTAOHouseHi1.Smoke.A, check, true);
										IPLs.gta_online.GTAOHouseHi1.Smoke.Enable(IPLs.gta_online.GTAOHouseHi1.Smoke.B, check, true);
										IPLs.gta_online.GTAOHouseHi1.Smoke.Enable(IPLs.gta_online.GTAOHouseHi1.Smoke.C, check, true);
									}

									await BaseScript.Delay(500);
									Screen.Fading.FadeIn(500);
								};
							}

							break;

						#endregion

						#region Executive

						case 5:
							if (immobile == TipoImmobile.Casa)
							{
								int idx = 0;
								UIMenuListItem tema = new("Stile appartamento", new List<dynamic>()
								{
									"Modern",
									"Moody",
									"Vibrant",
									"Sharp",
									"Monochrome",
									"Seductive",
									"Regal",
									"Aqua"
								}, casaDummy.Stile);                                                  // cambiare index
								UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
								UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
								UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
								_new.AddItem(tema);
								_new.AddItem(strip);
								_new.AddItem(booze);
								_new.AddItem(smoke);
								tema.OnListChanged += async (item, index) =>
								{
									Screen.Fading.FadeOut(250);
									await BaseScript.Delay(300);
									idx = index;
									casaDummy.Stile = idx;

									switch (index)
									{
										case 0:
											IPLs.dlc_executive.ExecApartment1.Style.Set(IPLs.dlc_executive.ExecApartment1.StyleExec.ExecApartTheme.Modern, true);

											break;
										case 1:
											IPLs.dlc_executive.ExecApartment1.Style.Set(IPLs.dlc_executive.ExecApartment1.StyleExec.ExecApartTheme.Moody, true);

											break;
										case 2:
											IPLs.dlc_executive.ExecApartment1.Style.Set(IPLs.dlc_executive.ExecApartment1.StyleExec.ExecApartTheme.Vibrant, true);

											break;
										case 3:
											IPLs.dlc_executive.ExecApartment1.Style.Set(IPLs.dlc_executive.ExecApartment1.StyleExec.ExecApartTheme.Sharp, true);

											break;
										case 4:
											IPLs.dlc_executive.ExecApartment1.Style.Set(IPLs.dlc_executive.ExecApartment1.StyleExec.ExecApartTheme.Monochrome, true);

											break;
										case 5:
											IPLs.dlc_executive.ExecApartment1.Style.Set(IPLs.dlc_executive.ExecApartment1.StyleExec.ExecApartTheme.Seductive, true);

											break;
										case 6:
											IPLs.dlc_executive.ExecApartment1.Style.Set(IPLs.dlc_executive.ExecApartment1.StyleExec.ExecApartTheme.Regal, true);

											break;
										case 7:
											IPLs.dlc_executive.ExecApartment1.Style.Set(IPLs.dlc_executive.ExecApartment1.StyleExec.ExecApartTheme.Aqua, true);

											break;
									}

									await BaseScript.Delay(500);
									Screen.Fading.FadeIn(500);
								};
								_new.OnCheckboxChange += async (menu, item, check) =>
								{
									Screen.Fading.FadeOut(250);
									await BaseScript.Delay(300);

									if (item == strip)
									{
										casaDummy.Strip = check;
										IPLs.dlc_executive.ExecApartment1.Strip.Enable(IPLs.dlc_executive.ExecApartment1.Strip.A, check, true);
										IPLs.dlc_executive.ExecApartment1.Strip.Enable(IPLs.dlc_executive.ExecApartment1.Strip.B, check, true);
										IPLs.dlc_executive.ExecApartment1.Strip.Enable(IPLs.dlc_executive.ExecApartment1.Strip.C, check, true);
									}
									else if (item == booze)
									{
										casaDummy.Booze = check;
										IPLs.dlc_executive.ExecApartment1.Booze.Enable(IPLs.dlc_executive.ExecApartment1.Booze.A, check, true);
										IPLs.dlc_executive.ExecApartment1.Booze.Enable(IPLs.dlc_executive.ExecApartment1.Booze.B, check, true);
										IPLs.dlc_executive.ExecApartment1.Booze.Enable(IPLs.dlc_executive.ExecApartment1.Booze.C, check, true);
									}
									else if (item == smoke)
									{
										casaDummy.Smoke = check;
										IPLs.dlc_executive.ExecApartment1.Smoke.Enable(IPLs.dlc_executive.ExecApartment1.Smoke.A, check, true);
										IPLs.dlc_executive.ExecApartment1.Smoke.Enable(IPLs.dlc_executive.ExecApartment1.Smoke.B, check, true);
										IPLs.dlc_executive.ExecApartment1.Smoke.Enable(IPLs.dlc_executive.ExecApartment1.Smoke.C, check, true);
									}

									await BaseScript.Delay(500);
									Screen.Fading.FadeIn(500);
								};
							}

							break;

						#endregion
					}
				}
			};

			#endregion

			#region StateChanged

			HUD.MenuPool.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
			{
				switch (state)
				{
					case MenuState.Opened:
						oldInstance = Cache.MyPlayer.User.StatiPlayer.Istanza;
						Cache.MyPlayer.User.StatiPlayer.Istanza.Istanzia("Creatore Immobiliare");

						break;
					case MenuState.Closed:
					{
						if (Cache.MyPlayer.User.StatiPlayer.Istanza.Instance == "Creatore Immobiliare") Cache.MyPlayer.User.StatiPlayer.Istanza.RimuoviIstanza();
						Cache.MyPlayer.User.StatiPlayer.Istanza = oldInstance;

						break;
					}
					case MenuState.ChangeForward when newmenu == selezionePunto:
					{
						SetPlayerControl(Cache.MyPlayer.Player.Handle, false, 256);
						Screen.Fading.FadeOut(800);
						while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
						if (MainCamera == null) MainCamera = World.CreateCamera(Vector3.Zero, new Vector3(0, 0, 0), 45f);
						MainCamera.Position = Cache.MyPlayer.User.Posizione.ToVector3 + new Vector3(0, 0, 100);
						MainCamera.IsActive = true;
						RenderScriptCams(true, false, 1000, true, true);
						curLocation = MainCamera.Position.ToPosition();
						curRotation = MainCamera.Rotation.ToPosition();
						checkTimer = GetGameTimer();
						Client.Instance.AddTick(CreatorCameraControl);
						Screen.Fading.FadeIn(500);

						break;
					}
					case MenuState.ChangeForward:
					{
						if (newmenu == marker)
						{
							marker.Clear();

							switch (immobile)
							{
								case TipoImmobile.Casa:
									marker.AddItem(markerIngressoCasa);
									if (includiGarage) marker.AddItem(markerIngressoGarage);
									if (includiTetto) marker.AddItem(markerIngressoTetto);
									marker.AddItem(posCamera);

									break;
								case TipoImmobile.Garage:
									marker.AddItem(markerIngressoGarage);
									marker.AddItem(posCamera);

									break;
							}

							Client.Instance.AddTick(MarkerTick);
							if (markerIngrPiedi == null) markerIngrPiedi = new Marker(MarkerType.VerticalCylinder, Position.Zero, new Position(1.5f), Colors.Red);
							if (markerIngrGarage == null) markerIngrGarage = new Marker(MarkerType.VerticalCylinder, Position.Zero, new Position(1.5f), Colors.Red);
							if (markerIngrTetto == null) markerIngrTetto = new Marker(MarkerType.VerticalCylinder, Position.Zero, new Position(1.5f), Colors.Red);
						}

						break;
					}
					case MenuState.ChangeBackward when oldmenu == marker:
						Client.Instance.RemoveTick(MarkerTick);

						break;
					case MenuState.ChangeBackward when oldmenu == selezionePunto:
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

						SetPlayerControl(Cache.MyPlayer.Player.Handle, true, 256);
						Screen.Fading.FadeIn(500);

						break;
					}
					case MenuState.ChangeBackward:
					{
						if (oldmenu == gestioneInteriorCasa)
						{
							Screen.Fading.FadeOut(800);
							while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
							MainCamera.StopPointing();
							ClearFocus();

							if (MainCamera.Exists() && World.RenderingCamera == MainCamera)
							{
								RenderScriptCams(false, false, 1000, false, false);
								MainCamera.IsActive = false;
							}

							SetPlayerControl(Cache.MyPlayer.Player.Handle, true, 256);
							Screen.Fading.FadeIn(500);
						}

						break;
					}
				}
			};

			#endregion

			UIMenuItem Salva = new("Salva immobile", "Attenzione la cosa non sarà reversibile se non contattando un ADMIN!");
			creazione.AddItem(Salva);
			Salva.Activated += async (menu, item) =>
			{
				switch (immobile)
				{
					case TipoImmobile.Casa when !string.IsNullOrWhiteSpace(casaDummy.Label):
					{
						if (!string.IsNullOrWhiteSpace(abbreviazione))
						{
							if (casaDummy.Price > 0)
							{
								if (casaDummy.MarkerEntrata != Position.Zero)
								{
									if (casaDummy.TelecameraFuori.pos != Position.Zero && casaDummy.TelecameraFuori.guarda != Position.Zero)
									{
										if (casaDummy.GarageIncluso)
										{
											if (casaDummy.MarkerGarageEsterno != Position.Zero && casaDummy.SpawnGarageInVehFuori != Position.Zero)
											{
												if (casaDummy.TettoIncluso)
												{
													if (casaDummy.MarkerTetto != Position.Zero)
													{
														BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "casa", casaDummy.ToJson(), abbreviazione);
														HUD.MenuPool.CloseAllMenus();
													}
													else
													{
														HUD.ShowNotification("Hai incluso il tetto ma manca il marker del tetto!", NotificationColor.Red, true);
													}
												}
												else
												{
													// non tetto incluso
													BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "casa", casaDummy.ToJson(), abbreviazione);
													HUD.MenuPool.CloseAllMenus();
												}
											}
											else
											{
												HUD.ShowNotification("Hai incluso il garage ma mancano i marker del garage!", NotificationColor.Red, true);
											}
										}
										else // non garage incluso
										{
											if (casaDummy.TettoIncluso)
											{
												if (casaDummy.MarkerTetto != Position.Zero)
												{
													BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "casa", casaDummy.ToJson(), abbreviazione);
													Client.Impostazioni.RolePlay.Proprieta.Appartamenti.Add(abbreviazione, casaDummy);
													HUD.MenuPool.CloseAllMenus();
												}
												else
												{
													HUD.ShowNotification("Hai incluso il tetto ma manca il marker del tetto!", NotificationColor.Red, true);
												}
											}
											else
											{
												// non tetto incluso
												BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "casa", casaDummy.ToJson(), abbreviazione);
												Client.Impostazioni.RolePlay.Proprieta.Appartamenti.Add(abbreviazione, casaDummy);
												HUD.MenuPool.CloseAllMenus();
											}
										}
									}
									else
									{
										HUD.ShowNotification("Non hai settato la telecamera d'ingresso!", NotificationColor.Red, true);
									}
								}
								else
								{
									HUD.ShowNotification("Non hai impostato il marker d'entrata!", NotificationColor.Red, true);
								}
							}
							else
							{
								HUD.ShowNotification("Non hai inserito un prezzo!", NotificationColor.Red, true);
							}
						}
						else
						{
							HUD.ShowNotification("Non hai inserito l'abbreviazione!", NotificationColor.Red, true);
						}

						break;
					}
					case TipoImmobile.Casa:
						HUD.ShowNotification("Non hai specificato il nome dell'immobile!", NotificationColor.Red, true);

						break;
					case TipoImmobile.Garage when !string.IsNullOrWhiteSpace(garageDummy.Label):
					{
						if (!string.IsNullOrWhiteSpace(abbreviazione))
						{
							if (garageDummy.Price > 0)
							{
								if (garageDummy.MarkerEntrata != Position.Zero)
								{
									if (garageDummy.TelecameraFuori.pos != Position.Zero && garageDummy.TelecameraFuori.guarda != Position.Zero)
									{
										BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "garage", garageDummy.ToJson(), abbreviazione);
										Client.Impostazioni.RolePlay.Proprieta.Garages.Garages.Add(abbreviazione, garageDummy);
										HUD.MenuPool.CloseAllMenus();
									}
									else
									{
										HUD.ShowNotification("Non hai settato la telecamera d'ingresso!", NotificationColor.Red, true);
									}
								}
								else
								{
									HUD.ShowNotification("Non hai impostato il marker d'entrata!", NotificationColor.Red, true);
								}
							}
							else
							{
								HUD.ShowNotification("Non hai inserito un prezzo!", NotificationColor.Red, true);
							}
						}
						else
						{
							HUD.ShowNotification("Non hai inserito l'abbreviazione!", NotificationColor.Red, true);
						}

						break;
					}
					case TipoImmobile.Garage:
						HUD.ShowNotification("Non hai specificato il nome dell'immobile!", NotificationColor.Red, true);

						break;
				}
			};
			creazione.Visible = true;
		}

		private static async Task CreatorCameraControl()
		{
			float forwardPush = 0.8f;
			if (GetGameTimer() - checkTimer > (int)Math.Ceiling(1000 / forwardPush)) curLocation.SetFocus();
			string tast = $"~INPUTGROUP_MOVE~ per muoverti.\n~INPUTGROUP_LOOK~ per girare la telecamera.\n~INPUT_COVER~ ~INPUT_VEH_HORN~ per salire e scendere.\n~INPUT_FRONTEND_X~ per cambiare velocità.\nVelocità attuale: ~y~{travelSpeedStr}~w~.";
			string gampa = $"~INPUTGROUP_MOVE~ per muoverti.\n~INPUTGROUP_LOOK~ per girare la telecamera.\n~INPUTGROUP_FRONTEND_TRIGGERS~ per salire e scendere.\n~INPUT_FRONTEND_X~ per cambiare velocità.\nVelocità attuale: ~y~{travelSpeedStr}~w~.";
			HUD.ShowHelpNoMenu(IsInputDisabled(2) ? tast : gampa);

			if (blipColor != null)
				if (blipColor.ParentItem.Parent.Visible)
				{
					if (Input.IsControlJustPressed(Control.FrontendLb)) blipColor.GoLeft();
					if (Input.IsControlJustPressed(Control.FrontendRb)) blipColor.GoRight();
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
				if (travelSpeed > 5) travelSpeed = 0;
			}

			float xVectFwd = forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z)) * -1.0f;
			float yVectFwd = forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
			float xVectLat = forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
			float yVectLat = forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z));
			float z = 0;
			GetGroundZFor_3dCoord(curLocation.X, curLocation.Y, curLocation.Z, ref z, false);
			if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;

			if (Input.IsDisabledControlPressed(Control.MoveUpOnly))
			{
				curLocation.X += xVectFwd;
				curLocation.Y += yVectFwd;
				if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;
			}

			if (Input.IsDisabledControlPressed(Control.MoveDownOnly))
			{
				curLocation.X -= xVectFwd;
				curLocation.Y -= yVectFwd;
				if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;
			}

			if (Input.IsDisabledControlPressed(Control.MoveLeftOnly))
			{
				curLocation.X -= xVectLat;
				curLocation.Y -= yVectLat;
				if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;
			}

			if (Input.IsDisabledControlPressed(Control.MoveRightOnly))
			{
				curLocation.X += xVectLat;
				curLocation.Y += yVectLat;
				if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;
			}

			if (Input.IsControlPressed(Control.FrontendLt, PadCheck.Controller) || Input.IsControlPressed(Control.Cover, PadCheck.Keyboard))
			{
				curLocation.Z += zVect;
				if (z != 0 && curLocation.Z > z + 300) curLocation.Z = z + 300;
			}

			if (Input.IsControlPressed(Control.FrontendRt, PadCheck.Controller) || Input.IsControlPressed(Control.VehicleHorn, PadCheck.Keyboard))
			{
				curLocation.Z -= zVect;
				if (curLocation.Z > z + 0.5f) curLocation.Z -= zVect;
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
				curRotation.X -= GetDisabledControlNormal(1, 2) * rotationSpeed * 8.0f;
				if (curRotation.X < -75f) curRotation.X = -75f;
				if (curRotation.X > 75f) curRotation.X = 75f;
				curRotation.Z -= GetDisabledControlNormal(1, 1) * rotationSpeed * 8.0f;
				if (curRotation.Z > 179.999999999f) curRotation.Z = -180f;
				if (curRotation.Z < -179.999999999f) curRotation.Z = 180f;
			}

			MainCamera.Position = curLocation.ToVector3;
			MainCamera.Rotation = curRotation.ToVector3;
		}

		private static async Task MarkerTick()
		{
			AsyncRaycastResult res = await MainCamera.CrosshairRaycast(150f);
			Vector3 direction = res.HitPosition;
			dummyMarker.Color = Colors.Red;
			if (!posCamera.Selected) dummyMarker.Draw();
			dummyMarker.Position = direction.ToPosition();
			float z = 0;
			GetGroundZFor_3dCoord(direction.X, direction.Y, direction.Z, ref z, false);
			if (z != 0 && res.DitHit) dummyMarker.Position.Z = z;
			if (markerIngressoCasa.Selected)
				if (markerIngressoCasa.RightBadge != BadgeStyle.None)
					markerIngrPiedi.Draw();
			if (markerIngressoGarage.Selected)
				if (markerIngressoGarage.RightBadge != BadgeStyle.None)
					markerIngrGarage.Draw();
			if (markerIngressoTetto.Selected)
				if (markerIngressoTetto.RightBadge != BadgeStyle.None)
					markerIngrTetto.Draw();
		}

		private static async Task BlipMarker()
		{
			if (blipColor != null)
			{
				AsyncRaycastResult res = await MainCamera.CrosshairRaycast(IntersectOptions.Everything, 150f);
				Vector3 direction = res.HitPosition;
				Vector3 pos = new(direction.X, direction.Y, curLocation.Z);
				string val = blipColor.ParentItem.Items[blipColor.ParentItem.Index] as string;
				World.DrawMarker((MarkerType)8, pos, Vector3.Zero, new Vector3(90, 90, 0), new Vector3(5f), blipColor.CurrentColor, true, false, true, "blips", val.Substring(1, val.Length - 2).ToLower());
				//metto un marker sotto
				float z = 0;
				GetGroundZFor_3dCoord(pos.X, pos.Y, pos.Z, ref z, false);
				World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(direction.X, direction.Y, z), Vector3.Zero, Vector3.Zero, new Vector3(2f, 2f, pos.Z - z), Colors.White);
			}
		}
	}
}