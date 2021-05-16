using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Races.Creator
{
	internal static class RaceCreator
	{
		private static List<ObjectHash> placement = new()
		{
			ObjectHash.prop_mp_placement,
			ObjectHash.prop_mp_placement_lrg,
			ObjectHash.prop_mp_placement_maxd,
			ObjectHash.prop_mp_placement_med,
			ObjectHash.prop_mp_placement_red,
			ObjectHash.prop_mp_placement_sm
		};

		private static Camera enteringCamera;
		private static Prop cross;
		private static Marker placeMarker;
		private static Vector3 curLocation;
		private static float Height = 0;
		private static Vector3 curRotation;
		private static Vector3 cameraPosition;
		//private static Vehicle cameraVeh;
		private static RaceTrack data = new RaceTrack();
		private static Prop DummyProp; // prop temporaneo da posizionare
		private static List<Prop> Piazzati = new List<Prop>();
		private static float zoom = 75f;
		private static List<dynamic> grigliaVehStart = new List<dynamic>();

		public static async void CreatorPreparation()
		{
			Cache.MyPlayer.Player.CanControlCharacter = false;
			float height = GetHeightmapTopZForPosition(199.4f, -934.3f);
			Vector3 rot = new(-90f, 0f, 0f);
			enteringCamera = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));
			enteringCamera.Position = new Vector3(199.4f, -934.3f, height);
			enteringCamera.Rotation = rot;
			enteringCamera.IsActive = true;
			enteringCamera.FarClip = 1000;
			enteringCamera.FieldOfView = 30;
			RenderScriptCams(true, false, 3000, true, false);
			SetFrontendActive(false);
			/*
			cameraVeh = await Funzioni.SpawnLocalVehicle("NINEF", enteringCamera.Position, enteringCamera.Rotation.Z);
			cameraVeh.IsVisible = false;
			cameraVeh.IsCollisionEnabled = false;
			cameraVeh.IsPositionFrozen = true;
			enteringCamera.AttachTo(cameraVeh, Vector3.Zero);
			*/
			curRotation = enteringCamera.Rotation;
			Cache.MyPlayer.Ped.IsPositionFrozen = true;
			Cache.MyPlayer.Ped.IsVisible = false;
			Cache.MyPlayer.Ped.IsInvincible = true;
			Cache.MyPlayer.Ped.DiesInstantlyInWater = false;
			Screen.Hud.IsRadarVisible = false;
			placeMarker ??= new Marker(MarkerType.HorizontalCircleSkinny, WorldProbe.CrossairRenderingRaycastResult.HitPosition, new Vector3(6.7f), Colors.GreyDark);

			if (cross == null)
			{
				float pz = 0;
				GetGroundZFor_3dCoord(enteringCamera.Position.X, enteringCamera.Position.Y, enteringCamera.Position.Z, ref pz, false);
				cross = new Prop(CreateObjectNoOffset(Funzioni.HashUint("prop_mp_placement"), enteringCamera.Position.X, enteringCamera.Position.Y, pz, false, false, false));
				cross.IsVisible = true;
				SetEntityLoadCollisionFlag(cross.Handle, true);
				cross.LodDistance = 500;
				SetEntityCollision(cross.Handle, false, false);
				cameraPosition = enteringCamera.Position;
				Height = cross.Position.Z;
			}
			SetFocusEntity(cross.Handle);
			Client.Instance.AddTick(DrawMarker);
			Client.Instance.AddTick(MoveCamera);
			CreatorMainMenu();
			Screen.Fading.FadeIn(10);
			//cross = Funzioni.SpawnLocalProp("prop_mp_placement_sm", )
		}

		public static async void CreatorMainMenu()
		{
			UIMenu Creator = new("Creatore Gare", "Lo strumento dei creativi");
			HUD.MenuPool.Add(Creator);
			UIMenu Dettagli = Creator.AddSubMenu("Dettagli");
			UIMenu Posizionamento = Creator.AddSubMenu("Posizionamento");

			#region Dettagli

			UIMenuItem titolo = new("Titolo");
			Dettagli.AddItem(titolo);
			titolo.SetRightBadge(BadgeStyle.Alert);
			UIMenuItem descrizione = new("Descrizione");
			descrizione.SetRightBadge(BadgeStyle.Alert);
			Dettagli.AddItem(descrizione);
			UIMenuItem foto = new("Foto");
			Dettagli.AddItem(foto);
			UIMenuListItem tipoGara = new("Tipo di Gara", new List<dynamic>() { "Standard", "Senza contatto" }, data.TipoGara);
			Dettagli.AddItem(tipoGara);
			UIMenuListItem tipoDiGiri = new("Tipo di Giri", new List<dynamic>() { "Giri", "Da punto a punto" }, data.TipoGiri); // punto a punto disabilita numero laps
			Dettagli.AddItem(tipoDiGiri);
			UIMenuListItem numeroLaps = new("Numero di Giri", new List<dynamic>() { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, data.Laps);
			Dettagli.AddItem(numeroLaps);
			UIMenuListItem numPlayers = new("Max Giocatori", new List<dynamic>() { 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 64 }, data.MaxPlayers);
			Dettagli.AddItem(numPlayers);
			UIMenu veicoliDisponibili = Dettagli.AddSubMenu("Veicoli Disponibili", "Attenzione, se si sceglie la griglia piccola saranno disponibili solo le MOTO"); // aggiungere le classi dei veicoli e premendo espandi scegliamo i singoli veicoli
			veicoliDisponibili.ParentItem.SetRightBadge(BadgeStyle.Car);
			#region veicoliDisponibili
			UIMenuCheckboxItem Compacts = new UIMenuCheckboxItem("Compacts", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Compacts);
			UIMenuCheckboxItem Sedans = new UIMenuCheckboxItem("Sedans", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Sedans);
			UIMenuCheckboxItem SUVs = new UIMenuCheckboxItem("SUVs", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(SUVs);
			UIMenuCheckboxItem Coupes = new UIMenuCheckboxItem("Coupes", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Coupes);
			UIMenuCheckboxItem Muscle = new UIMenuCheckboxItem("Muscle", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Muscle);
			UIMenuCheckboxItem SportsClassics = new UIMenuCheckboxItem("SportsClassics", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(SportsClassics);
			UIMenuCheckboxItem Sports = new UIMenuCheckboxItem("Sports", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Sports);
			UIMenuCheckboxItem Super = new UIMenuCheckboxItem("Super", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Super);
			UIMenuCheckboxItem Motorcycles = new UIMenuCheckboxItem("Motorcycles", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Motorcycles);
			UIMenuCheckboxItem OffRoad = new UIMenuCheckboxItem("OffRoad", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(OffRoad);
			UIMenuCheckboxItem Industrial = new UIMenuCheckboxItem("Industrial", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Industrial);
			UIMenuCheckboxItem Utility = new UIMenuCheckboxItem("Utility", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Utility);
			UIMenuCheckboxItem Vans = new UIMenuCheckboxItem("Vans", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Vans);
			UIMenuCheckboxItem Cycles = new UIMenuCheckboxItem("Cycles", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Cycles);
			UIMenuCheckboxItem Boats = new UIMenuCheckboxItem("Boats", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Boats);
			UIMenuCheckboxItem Helicopters = new UIMenuCheckboxItem("Helicopters", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Helicopters);
			UIMenuCheckboxItem Planes = new UIMenuCheckboxItem("Planes", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Planes);
			UIMenuCheckboxItem Service = new UIMenuCheckboxItem("Service", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Service);
			UIMenuCheckboxItem Emergency = new UIMenuCheckboxItem("Emergency", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Emergency);
			UIMenuCheckboxItem Commercial = new UIMenuCheckboxItem("Commercial", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Commercial);
			#endregion
			UIMenuListItem classeDefault = new("Classe Default", new List<dynamic>() { "Super" }, 0); // da gestire in base alla griglia e alle classi permesse in veicoli disponibili
			Dettagli.AddItem(classeDefault);
			UIMenuListItem veicoloDefault = new("Veicolo Default", new List<dynamic>() { "Prototipo" }, 0); // inserire i veicoli in base al nome del modello e in base alle classi permesse
			Dettagli.AddItem(veicoloDefault);
			UIMenuListItem orario = new("Orario", new List<dynamic>() { "Mattino", "Pomeriggio", "Notte" }, 0); // cambiare l'orario e aggiungere tramonto e alba
			Dettagli.AddItem(orario);
			UIMenuListItem meteo = new("Meteo", new List<dynamic>() { "Soleggiato", "Pioggia", "Smog", "Sereno", "Nuvoloso", "Coperto", "Tempesta", "Nebbia" }, 0); // cambiare il meteo in base alla necesità e magari aggiungere alba e tramonto
			Dettagli.AddItem(meteo);
			UIMenuListItem traffico = new("Traffico", new List<dynamic>() { "Off", "Basso", "Medio", "Alto" }, 0, "Se la gara è a mezz'aria, è consigliato impostare su OFF"); // cambiare il traffico in base alla necesità
			Dettagli.AddItem(traffico);
			#region dettagli item process
			Dettagli.OnItemSelect += async (a, b, c) =>
			{
				if (b == titolo)
				{
					string title = await HUD.GetUserInput("Inserisci Titolo (Max 25 caratteri)", "", 25);
					data.Titolo = title;
					b.SetRightLabel(title.Length > 15 ? title.Substring(0, 13) + "..." : title);
				}
				else if (b == descrizione)
				{
					string desc = await HUD.GetUserInput("Inserisci descrizione (Max 70 caratteri)", "", 70);
					data.Descrizione = desc;
					b.SetRightLabel(desc.Length > 15 ? desc.Substring(0, 13) + "..." : desc);
				}
				if(!(string.IsNullOrWhiteSpace(data.Titolo) && string.IsNullOrWhiteSpace(data.Descrizione)))
				{
					// unlocks everything
					titolo.SetRightBadge(BadgeStyle.None);
					descrizione.SetRightBadge(BadgeStyle.None);
				}
			};

			Dettagli.OnListChange += async (a, b, c) =>
			{
				if (b == tipoGara)
				{
					data.TipoGara = c;
				}
				else if (b == tipoDiGiri)
				{
					data.TipoGiri = c;
				}
				else if (b == numeroLaps)
				{
					data.Laps = c;
				}
				else if (b == numPlayers)
				{
					data.MaxPlayers = c;
					grigliaVehStart.Clear();
					grigliaVehStart.Add("2 X " + data.MaxPlayers);
				}
				else if (b == orario)
				{
					data.Orario = c;
					PauseClock(true);
					switch (c)
					{
						case 0:
							NetworkOverrideClockTime(7, 0, 0);
							break;
						case 1:
							NetworkOverrideClockTime(17, 0, 0);
							break;
						case 2:
							NetworkOverrideClockTime(23, 59, 59);
							break;
					}
				}
				else if (b == meteo)
				{
					data.Meteo = c;
					//"Soleggiato", "Pioggia", "Smog", "Sereno", "Nuvoloso", "Coperto", "Tempesta", "Nebbia"
					switch (c)
					{
						case 0:
							World.TransitionToWeather(Weather.ExtraSunny, 1f);
							break;
						case 1:
							World.TransitionToWeather(Weather.Raining, 1f);
							break;
						case 2:
							World.TransitionToWeather(Weather.Smog, 1f);
							break;
						case 3:
							World.TransitionToWeather(Weather.Clear, 1f);
							break;
						case 4:
							World.TransitionToWeather(Weather.Clouds, 1f);
							break;
						case 5:
							World.TransitionToWeather(Weather.Overcast, 1f);
							break;
						case 6:
							World.TransitionToWeather(Weather.ThunderStorm, 1f);
							break;
						case 7:
							World.TransitionToWeather(Weather.Foggy, 1f);
							break;
					}
				}
				else if (b == traffico)
				{
					data.Traffico = c;
					switch (c)
					{
						case 0:
							break;
						case 1:
							break;
						case 2:
							break;
						case 3:
							break;
					}
				}
				else if (b == classeDefault)
				{
					var veicoli = Enum.GetValues(typeof(VehicleHash)).Cast<VehicleHash>().ToList();
					VehicleClass classe;
					bool success = Enum.TryParse(b.Items[c].ToString(), out classe);
					var disp = veicoli.Where(x => GetVehicleClassFromName((uint)x) == (int)classe).Except(data.VeicoliEsclusi).ToList();
					veicoloDefault.Items.Clear();
					foreach (var d in disp)
						veicoloDefault.Items.Add(d.ToString());
					VehicleClass pippo;
					VehicleHash poppo;
					Enum.TryParse((string)classeDefault.Items.Last(), out pippo);
					Enum.TryParse((string)veicoloDefault.Items.First(), out poppo);
					data.ClasseDefault = pippo;
					data.VeicoloDefault = poppo;
				}
				else if (b == veicoloDefault)
				{
					VehicleHash classe;
					bool success = Enum.TryParse(b.Items[c].ToString(), out classe);
					data.VeicoloDefault = classe;
				}
			};
			#endregion

			#region ClassiVeicoli Process
			veicoliDisponibili.OnCheckboxChange += (a, b, c) =>
			{
				try
				{
					VehicleClass classe;
					bool success = Enum.TryParse(b.Text, out classe);
					if (success)
					{
						if (c)
							data.ClassiPermesse.Add(classe);
						else
							data.ClassiPermesse.Remove(classe);
						classeDefault.Items.Clear();
						var veicoli = Enum.GetValues(typeof(VehicleHash)).Cast<VehicleHash>().ToList();
						foreach (var cla in data.ClassiPermesse)
							classeDefault.Items.Add(cla.ToString());
						var disp = veicoli.Where(x => GetVehicleClassFromName((uint)x) == (int)classe).Except(data.VeicoliEsclusi).ToList();
						veicoloDefault.Items.Clear();
						foreach (var d in disp)
							veicoloDefault.Items.Add(d.ToString());
						VehicleClass pippo;
						VehicleHash poppo;
						Enum.TryParse((string)classeDefault.Items.Last(), out pippo);
						Enum.TryParse((string)veicoloDefault.Items.First(), out poppo);
						data.ClasseDefault = pippo;
						data.VeicoloDefault = poppo;
						Dettagli.RefreshIndex();
					}
				}
				catch (Exception e)
				{
					Client.Logger.Error(e.ToString());
				}
			};

			#endregion
			#endregion

			#region Posizionamento

			UIMenu checkpointsEGriglia = Posizionamento.AddSubMenu("Checkpoints");
			UIMenu propPlacing = Posizionamento.AddSubMenu("Posizionamento tracciato");


			#region CHECKPOINTS E GRIGLIA

			UIMenu grigliaDiPartenza = checkpointsEGriglia.AddSubMenu("Griglia di partenza");
			UIMenu checkPoints = checkpointsEGriglia.AddSubMenu("Posiziona Checkpoint");
			#region GRIGLIA DI PARTENZA
			UIMenuListItem dimensioniGriglia = new UIMenuListItem("Dimensioni Griglia", grigliaVehStart, 0);
			grigliaDiPartenza.AddItem(dimensioniGriglia);
			#endregion

			#region CHECKPOINTS
			UIMenuItem piazzaCheck = new UIMenuItem("Piazza Checkpoint");
			UIMenuListItem tipoCheck = new UIMenuListItem("Tipo", new List<dynamic>() { "Primario", "Secondario" }, 0);
			UIMenuListItem stileCheck = new UIMenuListItem("Stile", new List<dynamic>() { "Classico", "Circolare" }, 0);
			checkPoints.AddItem(piazzaCheck);
			checkPoints.AddItem(tipoCheck);
			checkPoints.AddItem(stileCheck);
			#endregion

			#endregion

			#region PROPS E POSIZIONAMENTO

			#endregion

			#endregion

			UIMenuItem Esci = new("Esci");
			Creator.AddItem(Esci);
			Creator.Visible = true;
			Creator.RemoveInstructionalButton(Creator.Back);
		}

		public static void changeModel(ObjectHash iVar11)
		{
			cross.Delete();
			cross = null;
			ObjectHash model = ObjectHash.prop_mp_placement_sm;

			switch (iVar11)
			{
				case ObjectHash.prop_mp_placement_sm:
					model = ObjectHash.prop_mp_placement_sm;

					break;
				case ObjectHash.prop_mp_placement_lrg:
					model = ObjectHash.prop_mp_placement_lrg;

					break;
				case ObjectHash.prop_mp_cant_place_sm:
					model = ObjectHash.prop_mp_cant_place_sm;

					break;
				case ObjectHash.prop_mp_cant_place_lrg:
					model = ObjectHash.prop_mp_cant_place_lrg;

					break;
				case ObjectHash.prop_mp_max_out_sm:
					model = ObjectHash.prop_mp_max_out_sm;

					break;
				case ObjectHash.prop_mp_max_out_lrg:
					model = ObjectHash.prop_mp_max_out_lrg;

					break;
			}

			if (cross == null)
			{
				cross = new Prop(CreateObjectNoOffset((uint)model, WorldProbe.CrossairRenderingRaycastResult.HitPosition.X, WorldProbe.CrossairRenderingRaycastResult.HitPosition.Y, WorldProbe.CrossairRenderingRaycastResult.HitPosition.Z, false, false, false));
				cross.IsVisible = true;
				SetEntityLoadCollisionFlag(cross.Handle, true);
				cross.LodDistance = 500;
				SetEntityCollision(cross.Handle, false, false);
			}
		}

		private static bool func_9339(float iParam0, float fParam1)
		{
			if (IsInputDisabled(2))
				return true;
			if (iParam0 < -fParam1)
				return true;
			else if (iParam0 > fParam1)
				return true;
			return false;
		}

		private static int func_7449()
		{
			if (IsInputDisabled(2))
				return 251;
			return 210;
		}

		private static int func_7450()
		{
			if (IsInputDisabled(2))
				return 250;
			return 209;
		}

		private static int func_9341()
		{
			if (IsInputDisabled(2))
				return 204;
			return 217;
		}

		private static int func_9387()
		{
			if (IsInputDisabled(2))
				return 253;
			return 208;
		}

		private static int func_7660()
		{
			if (IsInputDisabled(2))
				return 252;
			return 207;
		}

		private static float func_909(float fParam0, float fParam1, float fParam2)
		{
			if (fParam0 > fParam2)
			{
				return fParam2;
			}
			else if (fParam0 < fParam1)
			{
				return fParam1;
			}
			return fParam0;
		}


		private static int func_7497(Vector3 vParam0, float fParam1, int iParam2, Vector3 vParam3, float fParam4)
		{
			Vector3 vVar0 = Vector3.Zero;
			Vector3 vVar1 = Vector3.Zero;
			Vector3 vVar2 = Vector3.Zero;
			Vector3 vVar3 = Vector3.Zero;

			GetModelDimensions((uint)iParam2, ref vVar0, ref vVar1);
			vVar2 = vVar1 - vVar0;
			vVar3.X = (vVar2.X / 2f) - vVar1.X;
			vVar3.Y = (vVar2.Y / 2f) - vVar1.Y;
			if (func_7498(vParam3, fParam4, vParam0.X + vVar3.X, vVar2, new	Vector3(fParam1), Vector3.Zero))
				return 1;
			return 0;
		}

		private static bool func_7498(Vector3 Param0, float uParam1, float fParam2, Vector3 Param3, Vector3 vParam4, Vector3 Param5, float uParam6 = 0, float fParam7 = 0)
		{
			float fVar0;
			float fVar1;
			float fVar2;
			float fVar3;
			float fVar4;
			float fVar5;
			float fVar6;
			float fVar7;
			float fVar8;
			float fVar9;
			float fVar10;

			fVar0 = (Cos(fParam7) * (Param0.X - Param3.X)) - (Sin(fParam7) * (Param0.Y - Param3.Y)) + Param3.X;
			fVar1 = (((Sin(fParam7) * (Param0.X - Param3.X)) + (Cos(fParam7) * (Param0.Y - Param3.Y))) + Param3.Y);
			fVar2 = (Param3.X - (Param5.X / 2f));
			fVar3 = (Param3.Y - (Param5.Y / 2f));
			fVar4 = (Param3.X + (Param5.X / 2f));
			fVar5 = (Param3.Y + (Param5.Y / 2f));
			fVar6 = func_909(fVar0, fVar2, fVar4);
			fVar7 = func_909(fVar1, fVar3, fVar5);
			fVar8 = (fVar0 - fVar6);
			fVar9 = (fVar1 - fVar7);
			fVar10 = ((fVar8 * fVar8) + (fVar9 * fVar9));
			return fVar10 < (fParam2 * fParam2);
		}


		#region Ticks

		private static async Task DrawMarker()
		{
		}

		private static async Task MoveCamera()
		{
			#region	MOVIMENTI TELECAMERA
			if (enteringCamera.Exists())
			{
				DisableInputGroup(2);
				float fVar0 = GetDisabledControlNormal(2, 218);
				float fVar1 = GetDisabledControlNormal(2, 219);
				float fVar2 = GetDisabledControlNormal(2, 220);
				float fVar3 = GetDisabledControlNormal(2, 221);
				float ltNorm = GetDisabledControlNormal(2, 252);
				float rtNorm = GetDisabledControlNormal(2, 253);
				if (!IsLookInverted())
				{
					fVar1 = -fVar1;
					fVar3 = -fVar3;
				}
				if (!IsInputDisabled(2))
				{
					N_0xc8b5c4a79cc18b94(enteringCamera.Handle);
				}
				else if (Input.IsDisabledControlPressed(Control.CreatorLT) || Input.IsDisabledControlPressed(Control.CreatorRT))
				{
					N_0xc8b5c4a79cc18b94(enteringCamera.Handle);
				}
				if (IsInputDisabled(2))
				{
					fVar2 = (float)Math.Floor(GetControlUnboundNormal(2, 1)) * 2;
					fVar3 = (float)Math.Floor(GetControlUnboundNormal(2, 2)) * -1;
				}


				/*
				curLocation += (fVar0 * cross.RightVector) + (fVar1 * cross.ForwardVector);
				cameraPosition += (fVar2 * cameraVeh.RightVector) + (fVar3 * cameraVeh.UpVector); // LT e RT fanno solo avanti e indietro..
				*/


				float xVectFwd = -fVar1 * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z));
				float yVectFwd = fVar1 * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
				float xVectLat = fVar0 * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
				float yVectLat = fVar0 * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z));


				curRotation = new(fVar3 + enteringCamera.Rotation.X, 0, -fVar2 + enteringCamera.Rotation.Z);

				zoom = (float)Math.Sqrt(curLocation.DistanceToSquared(cameraPosition)); // da migliorare assolutamente

				curLocation = new((cameraPosition + zoom * enteringCamera.CamForwardVector()).X, (cameraPosition + zoom * enteringCamera.CamForwardVector()).Y, Height); // fixare che cross si muove in avanti e indietro sulla base della X della telecamera e ho finito <3

				if (ltNorm > 0 || rtNorm > 0)
				{
					if (zoom < 351f)
						zoom += ltNorm;
					if (zoom > 16f)
						zoom -= rtNorm;
					cameraPosition = curLocation - zoom * enteringCamera.CamForwardVector();
				}

				if (DummyProp == null)
				{
					if (IsDisabledControlPressed(2, func_7450()))
					{
						cameraPosition.Z += 1f;
						Height += 1f;
					}
					if (IsDisabledControlPressed(2, func_7449()))
					{
						cameraPosition.Z -= 1f;
						Height -= 1f;
					}
				}

				cameraPosition.X += xVectFwd + xVectLat;
				cameraPosition.Y += yVectFwd + yVectLat;

				//cross.Heading = curRot; // aggiungere un bool quando vogliamo ruotare un prop o un impostazione con LB o RB su asse Z


				float z = 0;
				GetGroundZFor_3dCoord(curLocation.X, curLocation.Y, Height + 300, ref z, false);
				if (Height <= z + 0.3f)
					Height = z + 0.3f;
				cross.Position = curLocation;
				if(DummyProp == null)
					cross.Rotation = new(0, 0, curRotation.Z);
				placeMarker.Position = curLocation + new Vector3(0, 0, 0.1f);
				placeMarker.Draw();

				if (curRotation.X >= -11.5f)
					curRotation.X = -11.5f;
				if (curRotation.X <= -89.9f)
					curRotation.X = -89.9f;
				//enteringCamera.PointAt(curLocation);
				enteringCamera.Position = cameraPosition;
				enteringCamera.Rotation = curRotation;

				HUD.DrawText(0.3f, 0.7f, $"enteringCamera Position => {cameraPosition}");
				HUD.DrawText(0.3f, 0.725f, $"enteringCamera Rotation=> {curRotation}");
				HUD.DrawText(0.3f, 0.75f, $"curLocation => {curLocation}");
				HUD.DrawText(0.3f, 0.775f, $"Zoom => {zoom}");
				/*
				HUD.DrawText(0.3f, 0.7f, $"Corretti fVar0 = {fVar0}");
				HUD.DrawText(0.3f, 0.725f, $"Corretti fVar1 = {fVar1}");
				HUD.DrawText(0.3f, 0.75f, $"Corretti fVar2 = {fVar2}");
				HUD.DrawText(0.3f, 0.775f, $"Corretti fVar3 = {fVar3}");
				*/
			}
			#endregion
			#region MOVIMENTI PROP
			if (DummyProp != null)
			{
				float curRot = cross.Heading;
				if (IsDisabledControlPressed(2, 226))
					curRot -= 5f % 360f;
				if (IsDisabledControlPressed(2, 227))
					curRot += 5f % 360f;
				if (curRot < 0f)
					curRot += 360f;
			}

			#endregion
			// PER LO SNAP CERCARE "Creator_Snap", "DLC_Stunt_Race_Frontend_Sounds"
		}
		#endregion
	}
}