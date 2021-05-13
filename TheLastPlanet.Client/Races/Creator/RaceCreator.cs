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
		private static Vector3 curRotation;

		public static async void CreatorPreparation()
		{
			Cache.MyPlayer.Player.CanControlCharacter = false;
			float height = GetHeightmapTopZForPosition(199.4f, -934.3f);
			Vector3 rot = new(-90f, 0f, 0f);
			enteringCamera = new Camera(CreateCam("DEFAULT_SCRIPTED_FLY_CAMERA", false));
			SetFlyCamMaxHeight(enteringCamera.Handle, 1200f);
			enteringCamera.Position = new Vector3(199.4f, -934.3f, height);
			enteringCamera.Rotation = rot;
			enteringCamera.IsActive = true;
			RenderScriptCams(true, false, 3000, true, false);
			SetFrontendActive(false);
			Cache.MyPlayer.Ped.IsVisible = false;
			Cache.MyPlayer.Ped.IsInvincible = true;
			Cache.MyPlayer.Ped.DiesInstantlyInWater = false;
			Screen.Hud.IsRadarVisible = false;
			placeMarker ??= new Marker(MarkerType.HorizontalCircleSkinny, WorldProbe.CrossairRenderingRaycastResult.HitPosition, new Vector3(6.7f), Colors.GreyDark);

			if (cross == null)
			{
				float pz = 0;
				GetGroundZFor_3dCoord(enteringCamera.Position.X, enteringCamera.Position.Y, enteringCamera.Position.Z, ref pz, false);
				cross = new Prop(CreateObjectNoOffset(Funzioni.HashUint("prop_mp_placement_sm"), enteringCamera.Position.X, enteringCamera.Position.Y, pz, false, false, false));
				cross.IsVisible = true;
				SetEntityLoadCollisionFlag(cross.Handle, true);
				cross.LodDistance = 500;
				SetEntityCollision(cross.Handle, false, false);
				SetFocusEntity(cross.Handle);
				curLocation = cross.Position;
				enteringCamera.PointAt(cross);
			}
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
			UIMenuItem descrizione = new("Descrizione");
			Dettagli.AddItem(descrizione);
			UIMenuItem foto = new("Foto");
			Dettagli.AddItem(foto);
			UIMenuListItem tipoGara = new("Tipo di Gara", new List<dynamic>() { "Standard", "Senza contatto" }, 0);
			Dettagli.AddItem(tipoGara);
			UIMenuListItem tipoDiGiri = new("Tipo di Giri", new List<dynamic>() { "Giri", "Da punto a punto" }, 0); // punto a punto disabilita numero laps
			Dettagli.AddItem(tipoDiGiri);
			UIMenuListItem numeroLaps = new("Numero di Giri", new List<dynamic>()
			{
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15,
				16,
				17,
				18,
				19,
				20
			}, 0);
			Dettagli.AddItem(numeroLaps);
			UIMenuListItem numPlayers = new("Max Giocatori", new List<dynamic>()
			{
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				15,
				20,
				25,
				30,
				35,
				40,
				45,
				50,
				55,
				60,
				64
			}, 0);
			Dettagli.AddItem(numPlayers);
			UIMenu veicoliDisponibili = Dettagli.AddSubMenu("Veicoli Disponibili");                   // aggiungere le classi dei veicoli e premendo espandi scegliamo i singoli veicoli
			UIMenuListItem classeDefault = new("Classe Default", new List<dynamic>() { "Super" }, 0); // da gestire in base alla griglia e alle classi permesse in veicoli disponibili
			Dettagli.AddItem(classeDefault);
			UIMenuListItem veicoloDefault = new("Veicolo Default", new List<dynamic>() { "Prototipo" }, 0); // inserire i veicoli in base al nome del modello e in base alle classi permesse
			Dettagli.AddItem(veicoloDefault);
			UIMenuListItem orario = new("Orario", new List<dynamic>() { "Mattino", "Pomeriggio", "Notte" }, 0); // cambiare l'orario e aggiungere tramonto e alba
			Dettagli.AddItem(orario);
			UIMenuListItem meteo = new("Meteo", new List<dynamic>()
			{
				"Soleggiato",
				"Pioggia",
				"Smog",
				"Sereno",
				"Nuvoloso",
				"Coperto",
				"Tempesta",
				"Nebbia"
			}, 0); // cambiare il meteo in base alla necesità e magari aggiungere alba e tramonto
			Dettagli.AddItem(meteo);
			UIMenuListItem traffico = new("Traffico", new List<dynamic>()
			{
				"Default",
				"Off",
				"Basso",
				"Medio",
				"Alto"
			}, 0); // cambiare il traffico in base alla necesità
			Dettagli.AddItem(traffico);

			#endregion

			#region Posizionamento

			UIMenu checkpointsEGriglia = Posizionamento.AddSubMenu("Checkpoints");
			UIMenu start = checkpointsEGriglia.AddSubMenu("Griglia di partenza");
			UIMenuListItem grigliaIniziale = new("Griglia Iniziale", new List<dynamic>() { "Griglia Piccola", "Griglia Media", "Griglia Larga" }, 0); // griglia piccola solo moto..
			start.AddItem(grigliaIniziale);
			UIMenuListItem disposizioneVeicoli = new("Griglia Iniziale", new List<dynamic>() { "Griglia Piccola", "Griglia Media", "Griglia Larga" }, 0); // griglia piccola solo moto..
			UIMenu checkPoints = checkpointsEGriglia.AddSubMenu("Posiziona Checkpoint");

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

		private static int f_1470 = -1;
		private static async Task MoveCamera()
		{


			DisableInputGroup(2);
			float iVar0 = GetControlNormal(2, 218);
			float iVar1 = GetControlNormal(2, 219);
			float iVar2 = GetControlNormal(2, 220) * 127f;
			float iVar3 = GetControlNormal(2, 221) * 127f;
			
			HUD.DrawText(0.3f, 0.7f, $"iVar0 = {iVar0}");
			HUD.DrawText(0.3f, 0.725f, $"iVar1 = {iVar1}");
			HUD.DrawText(0.3f, 0.75f, $"iVar2 = {iVar2}");
			HUD.DrawText(0.3f, 0.775f, $"iVar3 = {iVar3}");
			
			if (iVar0 == 0f && iVar1 == 0f)
			{
				iVar0 = GetDisabledControlNormal(2, 218);
				iVar1 = GetDisabledControlNormal(2, 219);
			}
			if (iVar2 == 0f && iVar3 == 0f)
			{
				iVar2 = GetDisabledControlNormal(2, 220) * 127f;
				iVar3 = GetDisabledControlNormal(2, 221) * 127f;
			}
			
			HUD.DrawText(0.6f, 0.7f, $"Corretti iVar0 = {iVar0}");
			HUD.DrawText(0.6f, 0.725f, $"Corretti iVar1 = {iVar1}");
			HUD.DrawText(0.6f, 0.75f, $"Corretti iVar2 = {iVar2}");
			HUD.DrawText(0.6f, 0.775f, $"Corretti iVar3 = {iVar3}");

			if (!IsLookInverted())
			{
				iVar1 = -iVar1;
				iVar3 = -iVar3;
			}
			if (enteringCamera.Exists())
			{
				var vVar4 = enteringCamera.Rotation;
				if (!IsInputDisabled(2))
				{
					N_0xc8b5c4a79cc18b94(enteringCamera.Handle);
				}
				else if (Input.IsControlPressed(Control.CreatorLT) || Input.IsControlPressed(Control.CreatorRT))
				{
					N_0xc8b5c4a79cc18b94(enteringCamera.Handle);
				}
				if (IsInputDisabled(2))
				{
					iVar2 = (float)Math.Floor(GetControlUnboundNormal(2, 1) * 127f) * 2;
					iVar3 = (float)Math.Floor(GetControlUnboundNormal(2, 2) * 127f) * -1;
				}

				EnableControlAction(2, func_7450(), true);
				EnableControlAction(2, func_7449(), true); //func_9340
				if (IsControlPressed(2, func_9341()))
				{
					enteringCamera.FieldOfView = 40f;
				}
				if (enteringCamera.FieldOfView > 30f)
				{
					if (IsControlPressed(2, func_7449()))
					{
						float fVar5 = (enteringCamera.FieldOfView - 0.2f);
						if (fVar5 <= 30f)
							fVar5 = 30f;
						enteringCamera.FieldOfView = fVar5;
					}
				}
				if (enteringCamera.FieldOfView < 65f)
				{
					if (IsControlPressed(2, func_7450()))
					{
						float fVar5 = (enteringCamera.FieldOfView + 0.2f);
						if (fVar5 >= 65f)
							fVar5 = 65f;
						enteringCamera.FieldOfView = fVar5;
					}
				}

				HUD.DrawText(0.3f, 0.8f, $"enteringCamera Rotation => {enteringCamera.Rotation}");

				if (vVar4.X < -89.9f)
				{
					vVar4.X += 3f;
					if (vVar4.X > -89.9f)
						vVar4.X = (-89.9f + 0.01f);
				}
				else if (vVar4.X > (79f + 0.01f))
				{
					vVar4.X -= 3f;
					if (vVar4.X < 79f)
						vVar4.X = 79f;
				}
				else if (func_9339(iVar3, 30f))
				{
					vVar4 += new Vector3(0f, 0f, ((iVar3) * (0.01f + 0))) ;
					if (vVar4.X < -89.9f)
						vVar4.X = -89.9f;
					if (vVar4.X > 79f)
						vVar4.X = 79f;
				}
				if (func_9339(iVar2, 30f))
				{
					vVar4 -= new Vector3(((iVar2 * (0.01f + 0)) * 2f), 0f, 0f);
					enteringCamera.Rotation = vVar4;
				}
				enteringCamera.Rotation = vVar4;
			}
			float speed = 0.2f;
			float curRot = cross.Heading;
			if (IsDisabledControlPressed(2, 206))
				curRot -= 5f % 360f;
			if (IsDisabledControlPressed(2, 205))
				curRot += 5f % 360f;
			if (curRot < 0f)
				curRot += 360f;

			Vector3 camRot = enteringCamera.Rotation;

			//cross.Heading = curRot; // aggiungere un bool quando vogliamo ruotare un prop o un impostazione con LB o RB

			curLocation += (iVar0 * cross.RightVector) + (iVar1 * cross.ForwardVector);
			float z = 0;
			GetGroundZFor_3dCoord(curLocation.X, curLocation.Y, curLocation.Z, ref z, false);
			curLocation.Z = z;
			if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;
			cross.Position = curLocation;
			cross.Rotation = new(0, 0, camRot.Z);
			placeMarker.Position = curLocation + new Vector3(0, 0, 0.1f);
			placeMarker.Draw();

			/* 
				speed = 0.2f --> premo Y e aumenta
				fVar2 = 0.2f; uParam0->f_525
				if (uParam0->f_1470 != -1)
				{
					fVar2 = (fVar2 + (SYSTEM::TO_FLOAT((MISC::GET_GAME_TIMER() - uParam0->f_1470)) / 2000f));
					fVar2 = func_909((fVar2 * fVar2), 0.2f, 3f);
				}
				// SE PREMO L3 O R3 MUOVO SU E GIU LA TELECAMERA INSIEME ALL'OGGETTO.. CREDO
				if (PAD::IS_CONTROL_PRESSED(2, func_7449()) || PAD::IS_CONTROL_PRESSED(2, func_7450()))
				{
					if (PAD::IS_CONTROL_PRESSED(2, 204))
					{
						fVar2 = (fVar2 * 5f); // PROVARE ANCHE CON 10f
					}
					if (PAD::IS_CONTROL_JUST_PRESSED(2, func_7449()) || PAD::IS_CONTROL_PRESSED(2, func_7449()))
					{
						if (uParam2->f_770 != 1 && uParam0->f_1479 != 0f)
						{
							if (CAM::DOES_CAM_EXIST(*iParam1))
							{
								CAM::SET_FLY_CAM_COORD_AND_CONSTRAIN(*iParam1, CAM::GET_CAM_COORD(*iParam1) - Vector(fVar2, 0f, 0f));
							}
							uParam0->f_1479 = (uParam0->f_1479 - fVar2);
							if ((uParam2->f_3.f_2 - fVar0) < 1f)
							{
								uParam0->f_1479 = 0f;
							}
						}
					}
					else if (PAD::IS_CONTROL_JUST_PRESSED(2, func_7450()) || PAD::IS_CONTROL_PRESSED(2, func_7450()))
					{
						if ((uParam0->f_1479 < fParam6 && uParam2->f_524 != 46) && uParam0->f_258 != 15)
						{
							if (CAM::DOES_CAM_EXIST(*iParam1))
							{
								CAM::SET_FLY_CAM_COORD_AND_CONSTRAIN(*iParam1, CAM::GET_CAM_COORD(*iParam1) + Vector(fVar2, 0f, 0f));
							}
							func_7658(uParam0, uParam2, fVar2, fParam6, fVar0);
						}
					}
				}
			 
			 */
			// PER LO SNAP CERCARE "Creator_Snap", "DLC_Stunt_Race_Frontend_Sounds"
		}

		#endregion
	}
}