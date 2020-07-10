using CitizenFX.Core;
using CitizenFX.Core.UI;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Giostre
{
	static class RuotaPanoramica
	{
		static RuotaPan Ruota = new RuotaPan(null, 0, "IDLE");
		public static bool GiroFinito = true;
		static bool Scaleform = false;
		static Scaleform Buttons = new Scaleform("instructional_buttons");
		static Camera Cam1 = new Camera(0);
		static RuotaPanoramicaCamTastiera Cam2Tastiera = new RuotaPanoramicaCamTastiera();
		static RuotaPanoramicaCamGamePad Cam2GamePad = new RuotaPanoramicaCamGamePad();
		static int iLocal_355 = 0;
		static CabinaPan CabinaAttuale;
		static CabinaPan[] Cabine = new CabinaPan[16]
		{
			new CabinaPan(0),
			new CabinaPan(1),
			new CabinaPan(2),
			new CabinaPan(3),
			new CabinaPan(4),
			new CabinaPan(5),
			new CabinaPan(6),
			new CabinaPan(7),
			new CabinaPan(8),
			new CabinaPan(9),
			new CabinaPan(10),
			new CabinaPan(11),
			new CabinaPan(12),
			new CabinaPan(13),
			new CabinaPan(14),
			new CabinaPan(15),
	};
		public static void Init()
		{
			Client.Instance.AddEventHandler("onResourceStop", new Action<string>(OnStop));
			Client.Instance.AddEventHandler("lprp:ruotapanoramica:forceState", new Action<string>(ForceState));
			Client.Instance.AddEventHandler("lprp:ruotapanoramica:aggiornaCabine", new Action<int, int>(AggiornaCabine));
			Client.Instance.AddEventHandler("lprp:ruotapanoramica:FermaRuota", new Action<bool>(StatoRuota));
			Client.Instance.AddEventHandler("lprp:ruotapanoramica:playerSale", new Action<int, int>(PlayerSale));
			Client.Instance.AddEventHandler("lprp:ruotapanoramica:playerScende", new Action<int, int>(PlayerScende));
			Client.Instance.AddEventHandler("lprp:ruotapanoramica:aggiornaGradient", new Action<int>(AggiornaGradient));
			Blip Ferris = new Blip(AddBlipForCoord(-1663.97f, -1126.7f, 30.7f))
			{

				Sprite = BlipSprite.Fairground,
				IsShortRange = true,
				Name = "Ruota Panoramica"
			};
			SetBlipDisplay(Ferris.Handle, 4);
			CaricaTutto();
		}

		private static async void AggiornaGradient(int gradient)
		{
			Ruota.Gradient = gradient;
		}

		private static async Task SpawnaRuota()
		{
			Ruota.Entity = new Prop(CreateObject(GetHashKey("prop_ld_ferris_wheel"), 0f, 1f, 2f, false, false, false))
			{
				Position = new Vector3(-1663.97f, -1126.7f, 30.7f),
				Rotation = new Vector3(360, 0, 0),
				IsPositionFrozen = true,
				LodDistance = 1000,
				IsInvincible = true,
			};
			if (!IsAudioSceneActive("FAIRGROUND_RIDES_FERRIS_WHALE"))
				StartAudioScene("FAIRGROUND_RIDES_FERRIS_WHALE");
			int i = 0;
			while (i < 16)
			{
				await BaseScript.Delay(0);
				Cabine[i].Entity = new Prop(CreateObject(GetHashKey("prop_ferris_car_01"), 0f, 1f, 2f, false, false, false))
				{
					IsInvincible = true,
					Position = func_147(i),
					LodDistance = 1000,
					IsPositionFrozen = true,
				};
				Cabine[i].Index = i;
				i++;
			}
			await Task.FromResult(0);
		}

		private static void AggiornaCabine(int index, int players)
		{
			Cabine[index].NPlayer = players;
		}

		private static void StatoRuota(bool stato)
		{
			Ruota.Ferma = stato;
		}

		private static async void CaricaTutto()
		{
			RequestModel((uint)GetHashKey("prop_ld_ferris_wheel"));
			while (!HasModelLoaded((uint)GetHashKey("prop_ld_ferris_wheel"))) await BaseScript.Delay(100);
			RequestModel((uint)GetHashKey("prop_ferris_car_01"));
			while (!HasModelLoaded((uint)GetHashKey("prop_ferris_car_01"))) await BaseScript.Delay(100);
			RequestAnimDict("anim@mp_ferris_wheel");
			while (!HasAnimDictLoaded("anim@mp_ferris_wheel")) await BaseScript.Delay(100);
			RequestScriptAudioBank("SCRIPT\\FERRIS_WHALE_01", false);
			RequestScriptAudioBank("SCRIPT\\FERRIS_WHALE_02", false);
			RequestScriptAudioBank("THE_FERRIS_WHALE_SOUNDSET", false);
			await SpawnaRuota();
			Client.Instance.AddTick(MuoviRuota);
			//Client.Instance.AddTick(ControlloPlayer);
		}

		private static async Task MuoviRuota()
		{
			if (!Ruota.Ferma && Ruota.Entity != null)
			{
				float fVar2 = 0;
				if (iLocal_355 != 0)
					fVar2 = GetTimeDifference(GetNetworkTimeAccurate(), iLocal_355) / 800f;
				iLocal_355 = GetNetworkTimeAccurate();
				float speed = Ruota.Velocità * fVar2;
				Ruota.Rotazione += speed;

				if (Ruota.Rotazione >= 360f)
					Ruota.Rotazione -= 360f;

				if (IsAudioSceneActive("FAIRGROUND_RIDES_FERRIS_WHALE"))
				{
					Vector3 vVar1 = Game.PlayerPed.Position;
					SetAudioSceneVariable("FAIRGROUND_RIDES_FERRIS_WHALE", "HEIGHT", vVar1.Z - 13f);
				}

/*				if (CabinaAttuale != null)
					HUD.DrawText(0.2f, 0.925f, "CabinaAttuale = " + CabinaAttuale.Index);
				HUD.DrawText(0.2f, 0.95f, "Ruota.Gradient = " + Ruota.Gradient);
*/
				foreach (var cab in Cabine)
				{
//					HUD.DrawText(0.4f, 0.5f + (cab.Index * 0.025f), "Index = " + cab.Index + ", " + (Math.Abs(Ruota.Rotazione - cab.Gradient) <0.1f));
//					HUD.DrawText(0.2f, 0.5f + (cab.Index * 0.025f), "Index = " + cab.Index + ", " + Math.Abs(Ruota.Rotazione - cab.Gradient));
					if (Math.Abs(Ruota.Rotazione - cab.Gradient) < 0.05f)
					{
						Ruota.Gradient = cab.Index + 1 > 15 ? 0 : cab.Index + 1;
						BaseScript.TriggerServerEvent("lprp:ruotapanoramica:aggiornaGradient", Ruota.Gradient);
						switch (Ruota.State)
						{
							case "FACCIO_SALIRE":
								CabinaAttuale = Cabine[Ruota.Gradient];
								BaseScript.TriggerServerEvent("lprp:ruotapanoramica:playerSale",
									Game.PlayerPed.NetworkId, Ruota.Gradient);
								break;
							case "FACCIO_SCENDERE":
								BaseScript.TriggerServerEvent("lprp:ruotapanoramica:playerScende",
									Game.PlayerPed.NetworkId, Ruota.Gradient);
								break;
						}
					}
				}

				Vector3 pitch = new Vector3(-Ruota.Rotazione - (360f / 16f),0,0);
				Ruota.Entity.Rotation = pitch;

				Cabine.ToList().ForEach(o => func_145(Cabine.ToList().IndexOf(o)));
				SetAudioSceneVariable("FAIRGROUND_RIDES_FERRIS_WHALE", "HEIGHT", Game.PlayerPed.Position.Z - 13f);
/*
				int i;
				for (i = 0; i < 16; i++)
				{
					Vector3 offset = func_147(i);
					SetEntityCoordsNoOffset(Cabine[i].Entity.Handle, offset.X, offset.Y, offset.Z, true, false, false);
				}
*/
			}
			await Task.FromResult(0);
		}

		private static async void PlayerSale(int player, int cabina)
		{
			Ped Personaggio = (Ped)Entity.FromNetworkId(player);
			CabinaPan Cabina = Cabine[cabina];
			if (IsEntityAtCoord(Personaggio.Handle, -1661.95f, -1127.011f, 12.6973f, 1f, 1f, 1f, false, true, 0))
			{
				if (Personaggio.NetworkId != Game.PlayerPed.NetworkId)
					if (!NetworkHasControlOfNetworkId(player))
						while (!NetworkRequestControlOfNetworkId(player)) await BaseScript.Delay(0);
				BaseScript.TriggerServerEvent("lprp:ruotapanoramica:RuotaFerma", true);
				Ruota.Ferma = true;
				await BaseScript.Delay(100);
				Vector3 coord = GetOffsetFromEntityInWorldCoords(Cabina.Entity.Handle, 0, 0, 0);
				int uLocal_376 = NetworkCreateSynchronisedScene(coord.X, coord.Y, coord.Z, 0f, 0f, 0f, 2, true, false, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(Personaggio.Handle, uLocal_376, "anim@mp_ferris_wheel", "enter_player_one", 8f, -8f, 131072, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(uLocal_376);
				int iVar2 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_376);
				if (GetSynchronizedScenePhase(iVar2) > 0.99f)
				{
					uLocal_376 = NetworkCreateSynchronisedScene(coord.X, coord.Y, coord.Z, 0f, 0f, 0f, 2, true, false, 1065353216, 0, 1065353216);
					NetworkAddPedToSynchronisedScene(Personaggio.Handle, uLocal_376, "anim@mp_ferris_wheel", "enter_player_one", 8f, -8f, 131072, 0, 1148846080, 0);
					NetworkStartSynchronisedScene(uLocal_376);
				}
				await BaseScript.Delay(7000);
				Vector3 attCoords = GetOffsetFromEntityGivenWorldCoords(Cabina.Entity.Handle, Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z);
				AttachEntityToEntity(Personaggio.Handle, Cabina.Entity.Handle, 0, attCoords.X, attCoords.Y, attCoords.Z, 0f, 0f, Game.PlayerPed.Heading, false, false, false, false, 2, true);
				N_0x267c78c60e806b9a(Personaggio.Handle, true);
				BaseScript.TriggerServerEvent("lprp:ruotapanoramica:aggiornaCabine", Cabina.Index, Cabina.NPlayer);
				if (Personaggio.Handle == PlayerPedId())
					GiroFinito = false;
				Ruota.State = "IDLE";
				BaseScript.TriggerServerEvent("lprp:ruotapanoramica:RuotaFerma", false);
				int iLocal_297 = GetSoundId();
				PlaySoundFromEntity(iLocal_297, "GENERATOR", Ruota.Entity.Handle, "THE_FERRIS_WHALE_SOUNDSET", false, 0);
				int iLocal_299 = GetSoundId();
				PlaySoundFromEntity(iLocal_299, "SLOW_SQUEAK", Ruota.Entity.Handle, "THE_FERRIS_WHALE_SOUNDSET", false, 0);
				int iLocal_300 = GetSoundId();
				PlaySoundFromEntity(iLocal_300, "SLOW_SQUEAK", Cabine[1].Entity.Handle, "THE_FERRIS_WHALE_SOUNDSET", false, 0);
				int iLocal_298 = GetSoundId();
				PlaySoundFromEntity(iLocal_298, "CARRIAGE", Cabine[1].Entity.Handle, "THE_FERRIS_WHALE_SOUNDSET", false, 0);
				if (Personaggio.Handle == PlayerPedId())
					CreaCam();
			}
		}

		private static async void PlayerScende(int player, int cabina)
		{
			Ped Personaggio = (Ped)Entity.FromNetworkId(player);
			CabinaPan Cabina = Cabine[cabina];
			if (Personaggio == Game.PlayerPed)
			{
				while (CabinaAttuale != Cabina) await BaseScript.Delay(0);
				RenderScriptCams(false, false, 1000, false, false);
				BaseScript.TriggerServerEvent("lprp:ruotapanoramica:RuotaFerma", true);
				Vector3 offset = GetOffsetFromEntityInWorldCoords(Cabina.Entity.Handle, 0f, 0f, 0f);
				Cam1.Delete();
				DestroyAllCams(false);
				int uLocal_377 = NetworkCreateSynchronisedScene(offset.X, offset.Y, offset.Z, 0f, 0f, 0f, 2, false, false, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(Personaggio.Handle, uLocal_377, "anim@mp_ferris_wheel", "exit_player_one", 8f, -8f, 131072, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(uLocal_377);
				Personaggio.Detach();
				await BaseScript.Delay(5000);
				Cabina.NPlayer = 0;
				BaseScript.TriggerServerEvent("lprp:ruotapanoramica:aggiornaCabine", Cabina.Index, Cabina.NPlayer);
				if (IsAudioSceneActive("FAIRGROUND_RIDES_FERRIS_WHALE"))
					StopAudioScene("FAIRGROUND_RIDES_FERRIS_WHALE");
				if (IsAudioSceneActive("FAIRGROUND_RIDES_FERRIS_WHALE_ALTERNATIVE_VIEW"))
					StopAudioScene("FAIRGROUND_RIDES_FERRIS_WHALE_ALTERNATIVE_VIEW");
				if (Personaggio.Handle == PlayerPedId())
					GiroFinito = true;
				BaseScript.TriggerServerEvent("lprp:ruotapanoramica:RuotaFerma", false);
				Ruota.State = "IDLE";
				CabinaAttuale = null;
			}
			else
			{
				if (Personaggio.NetworkId != Game.PlayerPed.NetworkId)
					if (!NetworkHasControlOfNetworkId(player))
						while (!NetworkRequestControlOfNetworkId(player)) await BaseScript.Delay(0);
				Vector3 offset = GetOffsetFromEntityInWorldCoords(Cabina.Entity.Handle, 0f, 0f, 0f);
				int uLocal_377 = NetworkCreateSynchronisedScene(offset.X, offset.Y, offset.Z, 0f, 0f, 0f, 2, false, false, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(Personaggio.Handle, uLocal_377, "anim@mp_ferris_wheel", "exit_player_one", 8f, -8f, 131072, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(uLocal_377);
				Personaggio.Detach();
				await BaseScript.Delay(5000);
				Cabina.NPlayer = 0;
				BaseScript.TriggerServerEvent("lprp:ruotapanoramica:aggiornaCabine", Cabina.Index, Cabina.NPlayer);
			}
		}

		private static async Task ControlloPlayer()
		{
			if (Game.PlayerPed.IsInRangeOf(new Vector3(-1661.95f, -1127.011f, 12.6973f), 20f))
			{
				if (!IsAudioSceneActive("FAIRGROUND_RIDES_FERRIS_WHALE"))
					StartAudioScene("FAIRGROUND_RIDES_FERRIS_WHALE");

				if (Game.PlayerPed.IsInRangeOf(new Vector3(-1661.95f, -1127.011f, 12.6973f), 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per salire sulla prima gondola libera");
					if (Input.IsControlJustPressed(Control.Context))
					{
						HUD.ShowNotification("Attendi, la prima cabina libera sta arrivando");
						BaseScript.TriggerServerEvent("lprp:ruotapanoramica:syncState", "FACCIO_SALIRE");
					}
				}
			}
			else
				StopAudioScene("FAIRGROUND_RIDES_FERRIS_WHALE");
			if (!GiroFinito)
			{
				if (GetFollowPedCamViewMode() == 4) SetFollowPedCamViewMode(2);
				Game.DisableControlThisFrame(0, Control.NextCamera);
				UpdateTasti();
				if (Input.IsControlJustPressed(Control.FrontendY))
				{
					HUD.ShowNotification("La giostra si fermerà appena la tua cabina tocca terra per farti scendere");
					Ruota.State = "FACCIO_SCENDERE";
				}
				if (Input.IsControlJustPressed(Control.ScriptSelect))
					CambiaCam();
			}
			await Task.FromResult(0);
		}

		static private async void UpdateTasti()
		{
			if (!Scaleform)
			{
				Buttons = new Scaleform("instructional_buttons");
				while (!HasScaleformMovieLoaded(Buttons.Handle)) await BaseScript.Delay(0);

				Buttons.CallFunction("CLEAR_ALL");
				Buttons.CallFunction("TOGGLE_MOUSE_BUTTONS", false);


				Buttons.CallFunction("CLEAR_ALL");

				Buttons.CallFunction("SET_DATA_SLOT", 0, GetControlInstructionalButton(2, 236, 1), "Per cambiare visuale");
				Buttons.CallFunction("SET_DATA_SLOT", 1, GetControlInstructionalButton(2, 204, 1), "Per scendere dalla ruota");

				Buttons.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				Scaleform = true;
			}
			if (Scaleform)
				Buttons.Render2D();
		}

		private static async void func_145(int i)
		{
			Vector3 offset = func_147(i);
			SetEntityCoordsNoOffset(Cabine[i].Entity.Handle, offset.X, offset.Y, offset.Z, true, false, false);
		}



		private static void func_79()
		{
			if (IsAudioSceneActive("FAIRGROUND_RIDES_FERRIS_WHALE"))
				StopAudioScene("FAIRGROUND_RIDES_FERRIS_WHALE");
			StartAudioScene("FAIRGROUND_RIDES_FERRIS_WHALE_ALTERNATIVE_VIEW");
		}

		private static async void CreaCam()
		{
			Cam1 = new Camera(CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -1703.854f, -1082.222f, 42.006f, -8.3096f, 0f, -111.8213f, 50f, false, 0));
			Cam1.PointAt(Ruota.Entity);
			Cam1.IsActive = true;
			Screen.Fading.FadeOut(500);
			await BaseScript.Delay(800);
			RenderScriptCams(true, false, 1000, false, false);
			Screen.Fading.FadeIn(500);
			func_79();
			SetLocalPlayerInvisibleLocally(false);
		}

		static bool Cambia = true;
		static void CambiaCam()
		{
			Cambia = !Cambia;
			if (Cambia)
			{
				Cam1 = new Camera(CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -1703.854f, -1082.222f, 42.006f, -8.3096f, 0f, -111.8213f, 50f, false, 0));
				Cam1.PointAt(Ruota.Entity);
				func_79();
				Cam1.IsActive = true;
				Client.Instance.RemoveTick(Cam2Controller);
				if (IsInputDisabled(2))
				{
					Cam2Tastiera.CamEntity.Delete();
					Cam2Tastiera.Valore1 = new Vector3(0f, 0f, 0f);
					Cam2Tastiera.Valore4 = new Vector3(0f, 0f, 0f);
					Cam2Tastiera.Valore7 = 0;
					Cam2Tastiera.Valore20 = 0;
					Cam2Tastiera.Valore21 = 0;
					Cam2Tastiera.Valore22 = 0;
					Cam2Tastiera.Valore8 = new Vector3(0f, 0f, 0f);
					Cam2Tastiera.Valore11 = new Vector3(0f, 0f, 0f);
					Cam2Tastiera.Valore14 = new Vector3(0f, 0f, 0f);
					Cam2Tastiera.Valore17 = 0;
					Cam2Tastiera.Valore18 = 0;
					Cam2Tastiera.Valore23 = 0;
					Cam2Tastiera.Valore19 = 0;
					Cam2Tastiera.Valore24 = 0;
					Cam2Tastiera.Valore25 = 0;
					Cam2Tastiera.Valore29 = 0f;
					Cam2Tastiera.Valore30 = 0f;
					Cam2Tastiera.Valore26 = false;
					Cam2Tastiera.Valore28 = false;
					Cam2Tastiera.Valore27 = 0;
				}
			}
			else
			{
				if (IsInputDisabled(2))
				{
					Vector3 PedCoords = GetPedBoneCoords(PlayerPedId(), 31086, 0f, 0.2f, 0f);
					Vector3 Rot = GetEntityRotation(PlayerPedId(), 2);
					Cam2Tastiera.CamEntity = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));
					SetCamParams(Cam2Tastiera.CamEntity.Handle, PedCoords.X, PedCoords.Y, PedCoords.Z, Rot.X, Rot.Y, Rot.Z, 50f, 0, 1, 1, 2);
					Cam2Tastiera.CamEntity.IsActive = true;
					Cam2Tastiera.CamEntity.Shake(CameraShake.Hand, 0.19f);
					SetCamNearClip(Cam2Tastiera.CamEntity.Handle, -1082130432f);
					AttachCamToPedBone(Cam2Tastiera.CamEntity.Handle, PlayerPedId(), 31086, 0f, 0.2f, 0f, true);
					SetLocalPlayerInvisibleLocally(false);

					Cam2Tastiera.Valore1 = PedCoords;
					Cam2Tastiera.Valore4 = Rot;
					Cam2Tastiera.Valore7 = 50f;
					Cam2Tastiera.Valore20 = 160;
					Cam2Tastiera.Valore21 = 20;
					Cam2Tastiera.Valore22 = 3;
					Cam2Tastiera.Valore8 = new Vector3(0f, 0f, 0f);
					Cam2Tastiera.Valore11 = new Vector3(0f, 0f, 0f);
					Cam2Tastiera.Valore14 = new Vector3(0f, 0f, 0f);
					Cam2Tastiera.Valore17 = 50f;
					Cam2Tastiera.Valore18 = 50f;
					Cam2Tastiera.Valore23 = 0;
					Cam2Tastiera.Valore19 = 1101004800;
					Cam2Tastiera.Valore24 = 0;
					Cam2Tastiera.Valore25 = 0;
					Cam2Tastiera.Valore29 = 0f;
					Cam2Tastiera.Valore30 = 0f;
					Cam2Tastiera.Valore26 = false;
					Cam2Tastiera.Valore28 = false;
					Cam2Tastiera.Valore27 = 0;
				}
				else
				{
					func_110(Cam2GamePad, true);
					func_109(Cam2GamePad, 0, 3000);
				}
				Client.Instance.AddTick(Cam2Controller);
				Cam1.Delete();
			}
			RenderScriptCams(true, false, 3000, true, false);
		}

		private static async Task Cam2Controller()
		{
			if (IsInputDisabled(2))
				func_101(Cam2Tastiera, true, true, false, false, 0.1f, false, 1065353216f, false);
			else
				func_105(Cam2GamePad);
		}

		static async void func_105(RuotaPanoramicaCamGamePad uParam0)
		{
			float uVar0 = 0;
			float uVar1 = 0;
			float fVar2;

			if (!uParam0.Valore1) return;
			DisableInputGroup(2);
			if (uParam0.Valore0)
			{
				if (Absf(GetControlNormal(2, 220)) > 0.1f)
				{
					uParam0.Valore12 = uParam0.Valore12 - (GetControlNormal(2, 220) * 60f * Timestep());
					if (uParam0.Valore15)
					{
						if (uParam0.Valore12 < -110)
							uParam0.Valore12 = -110;
						if (uParam0.Valore12 > 110)
							uParam0.Valore12 = 110;
					}
					else uParam0.Valore12 = func_102(uParam0.Valore12, -80f, 80f);
				}
				if (Absf(GetControlNormal(2, 221)) > 0.1f)
				{
					fVar2 = ((GetControlNormal(2, 221) * 60f) * Timestep());
					if (IsLookInverted())
						fVar2 = (fVar2 * -1f);
					uParam0.Valore11 -= fVar2;
					if (uParam0.Valore14)
					{
						if (uParam0.Valore11 < -30)
							uParam0.Valore11 = -30;
						if (uParam0.Valore11 > 30)
							uParam0.Valore11 = 30;
					}
					else
						uParam0.Valore11 = func_102(uParam0.Valore11, -30f, 30f);
				}
				if (IsControlJustPressed(2, 231))
				{
					uParam0.Valore11 = 0f;
					uParam0.Valore12 = 0f;
				}
				if (Absf(GetControlNormal(2, 219)) > 0.1f)
				{
					fVar2 = GetControlNormal(2, 219) * (60f / 2f) * Timestep();
					uParam0.Valore13 = (uParam0.Valore13 + fVar2);
					uParam0.Valore13 = func_102(uParam0.Valore13, 20f, 50f);
				}
				if (DoesCamExist(uParam0.CamEntity.Handle))
				{
					SetCamFov(uParam0.CamEntity.Handle, uParam0.Valore13);
					if (IsEntityDead(uParam0.Valore8) && !IsEntityDead(PlayerPedId()))
						SetCamRot(uParam0.CamEntity.Handle, (Game.PlayerPed.Rotation + new Vector3(uParam0.Valore11, 0f, uParam0.Valore12)).X, (Game.PlayerPed.Rotation + new Vector3(uParam0.Valore11, 0f, uParam0.Valore12)).Y, (Game.PlayerPed.Rotation + new Vector3(uParam0.Valore11, 0f, uParam0.Valore12)).Z, 2);
					else if (!IsEntityDead(uParam0.Valore8) && !IsEntityDead(PlayerPedId()))
					{
						func_106(GetEntityCoords(uParam0.Valore8, true), GetEntityCoords(uParam0.Valore9, true), ref uVar0, ref uVar1, 1);
						SetCamRot(uParam0.CamEntity.Handle, (new Vector3(uVar1, 0f, uVar0) + new Vector3(uParam0.Valore11, 0f, uParam0.Valore12)).X, (new Vector3(uVar1, 0f, uVar0) + new Vector3(uParam0.Valore11, 0f, uParam0.Valore12)).Y, (new Vector3(uVar1, 0f, uVar0) + new Vector3(uParam0.Valore11, 0f, uParam0.Valore12)).Z, 2);
					}
				}
			}
		}

		static void func_106(Vector3 vParam0, Vector3 vParam1, ref float uParam2, ref float uParam3, int iParam4)
		{
			Vector3 vVar0;
			vVar0 = vParam1 - vParam0;
			func_107(vVar0, ref uParam2, ref uParam3, iParam4);
		}

		static void func_107(Vector3 vParam0, ref float uParam1, ref float uParam2, int iParam3)
		{
			float fVar0;
			if (vParam0.Y != 0f)
				uParam2 = Atan2(vParam0.X, vParam0.Y);
			else if (vParam0.X < 0f)
				uParam2 = -90f;
			else
				uParam2 = 90f;
			if (iParam3 == 1)
			{
				uParam2 *= -1f;
				if (uParam2 < 0f) uParam2 += 360f;
			}
			fVar0 = Sqrt(((vParam0.X * vParam0.X) + (vParam0.Y * vParam0.Y)));
			if (fVar0 != 0f)
				uParam1 = Atan2(vParam0.Z, fVar0);
			else if (vParam0.Z < 0f)
				uParam1 = -90f;
			else
				uParam1 = 90f;
		}

		static int func_109(RuotaPanoramicaCamGamePad uParam0, int iParam1, int iParam2)
		{
			if (!uParam0.Valore1) return 0;
			uParam0.Valore13 = 50f;
			if (!DoesCamExist(uParam0.CamEntity.Handle))
				uParam0.CamEntity = new Camera(CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", uParam0.Valore2.X, uParam0.Valore2.Y, uParam0.Valore2.Z, uParam0.Valore5.X, uParam0.Valore5.Y, uParam0.Valore5.Z, 50f, true, 2));
			if (uParam0.Valore0)
			{
				AttachCamToPedBone(uParam0.CamEntity.Handle, PlayerPedId(), 31086, 0f, 0.2f, 0f, true);
				uParam0.Valore11 = 0f;
				uParam0.Valore12 = 0f;
			}
			if (DoesCamExist(uParam0.CamEntity.Handle))
				SetCamActive(uParam0.CamEntity.Handle, true);
			return 1;
		}

		static void func_110(RuotaPanoramicaCamGamePad uParam0, bool bParam1)
		{
			uParam0.Valore0 = true;
			uParam0.Valore1 = true;
			uParam0.Valore9 = PlayerPedId();
			uParam0.Valore11 = 0f;
			uParam0.Valore12 = 0f;
			if (bParam1)
				uParam0.Valore15 = true;
		}


		static async void func_101(RuotaPanoramicaCamTastiera uParam0, bool bParam1, bool bParam2, bool bParam3, bool bParam4, float fParam5, bool bParam6, float fParam7, bool bParam8)
		{
			int[] iVar0 = new int[4];
			float fVar1;
			float fVar2;
			float fVar3;
			float fVar4;
			float fVar5;
			Vector3 vVar6;
			int iVar7;
			int iVar8;

			DisableInputGroup(2);
			func_104(ref iVar0[0], ref iVar0[1], ref iVar0[2], ref iVar0[3], false, false);
			if (IsLookInverted())
				iVar0[3] = (iVar0[3] * -1);
			if (IsInputDisabled(2))
			{
				fVar1 = GetControlUnboundNormal(2, 239);
				fVar2 = GetControlUnboundNormal(2, 240);
				fVar3 = (fVar1 - uParam0.Valore29);
				fVar4 = (fVar2 - uParam0.Valore30);
				uParam0.Valore29 = fVar1;
				uParam0.Valore30 = fVar2;
				if (bParam4)
				{
					iVar0[2] = -(int)Math.Round(((fVar3 * fParam5) * 127f));
					iVar0[3] = -(int)Math.Round(((fVar4 * fParam5) * 127f));
				}
				else
				{
					iVar0[2] = (int)Math.Round(((GetControlUnboundNormal(2, 290) * fParam5) * 127f));
					iVar0[3] = (int)Math.Round(((GetControlUnboundNormal(2, 291) * fParam5) * 127f));
				}
				iVar0[2] = func_103((iVar0[2] + uParam0.Valore24), -127, 127);
				iVar0[3] = func_103((iVar0[3] + uParam0.Valore25), -127, 127);
			}
			if (uParam0.Valore24 == iVar0[2] && uParam0.Valore25 == iVar0[3])
			{
				if (uParam0.Valore27 < GetGameTimer())
				{
					uParam0.Valore24 = 0;
					uParam0.Valore25 = 0;
					if (IsInputDisabled(2))
					{
						iVar0[2] = 0;
						iVar0[3] = 0;
						uParam0.Valore28 = true;
					}
				}
			}
			else
			{
				uParam0.Valore24 = iVar0[2];
				uParam0.Valore25 = iVar0[3];
				uParam0.Valore27 = GetGameTimer() + 4000;
				uParam0.Valore28 = false;
			}
			if (bParam2)
			{
				uParam0.Valore8.Z = -(ToFloat(iVar0[2]) / 127f) * uParam0.Valore20;
				uParam0.Valore8.Y = -uParam0.Valore8.Z * uParam0.Valore22 / (uParam0.Valore20);
				uParam0.Valore8.X = -(ToFloat(iVar0[3]) / 127f) * uParam0.Valore21;
			}
			else
			{
				uParam0.Valore8 = new Vector3(0f, 0f, 0f);
				uParam0.Valore24 = 0;
				uParam0.Valore25 = 0;
			}
			fVar5 = 30f * Timestep();
			vVar6 = uParam0.Valore8 + uParam0.Valore11;
			if (IsInputDisabled(2) && bParam2 && !uParam0.Valore28)
			{
				uParam0.Valore14.X = vVar6.X;
				uParam0.Valore14.Y = vVar6.Y;
				uParam0.Valore14.Z = vVar6.Z;
			}
			else
			{
				uParam0.Valore14.X += func_102((vVar6.X - uParam0.Valore14.X) * 0.05f * fVar5 * fParam7, -3f, 3f);
				uParam0.Valore14.Y += func_102((vVar6.Y - uParam0.Valore14.Y) * 0.05f * fVar5 * fParam7, -3f, 3f);
				uParam0.Valore14.Z += func_102((vVar6.Z - uParam0.Valore14.Z) * 0.05f * fVar5 * fParam7, -3f, 3f);
			}
			if (uParam0.Valore26)
			{
				uParam0.Valore14.X = func_102(uParam0.Valore14.X, -uParam0.Valore21, uParam0.Valore21);
				uParam0.Valore14.Y = func_102(uParam0.Valore14.Y, -uParam0.Valore22, uParam0.Valore22);
				uParam0.Valore14.Z = func_102(uParam0.Valore14.Z, -uParam0.Valore20, uParam0.Valore20);
			}
			if (IsInputDisabled(0) && bParam1)
			{
				if (uParam0.Valore28)
					uParam0.Valore17 = uParam0.Valore7;
			}
			else
				uParam0.Valore17 = uParam0.Valore7;
			if (bParam1)
			{
				if (IsInputDisabled(0))
				{
					iVar7 = 40;
					iVar8 = 41;
					if (bParam6)
					{
						iVar7 = 241;
						iVar8 = 242;
					}
					if (IsDisabledControlJustPressed(0, iVar7))
					{
						uParam0.Valore17 -= 5f;
						uParam0.Valore27 = GetGameTimer() + 4000;
						uParam0.Valore28 = false;
					}
					else if (IsDisabledControlJustPressed(0, iVar8))
					{
						uParam0.Valore17 += 5f;
						uParam0.Valore27 = GetGameTimer() + 4000;
						uParam0.Valore28 = false;
					}
					if (bParam3)
						uParam0.Valore17 = func_102(uParam0.Valore17, (uParam0.Valore7 - uParam0.Valore19), uParam0.Valore7);
					else
						uParam0.Valore17 = func_102(uParam0.Valore17, (uParam0.Valore7 - uParam0.Valore19), (uParam0.Valore7 + uParam0.Valore19));
				}
				else if (bParam8)
				{
					iVar0[1] = GetControlValue(2, 207);
					iVar0[3] = GetControlValue(2, 208);
					if (bParam3)
					{
						if (ToFloat(iVar0[3]) > 127f)
							uParam0.Valore17 -= (int)Math.Round(iVar0[3] / 128f * (uParam0.Valore19 / 2f));
					}
					else
					{
						uParam0.Valore17 += (int)Math.Round(iVar0[1] / 128f * uParam0.Valore19);
						uParam0.Valore17 -= (int)Math.Round(iVar0[3] / 128f * uParam0.Valore19);
					}
				}
				else if (bParam3)
				{
					if ((iVar0[1]) < 0f)
						uParam0.Valore17 += (int)Math.Round(iVar0[1] / 128f * uParam0.Valore19);
				}
				else
					uParam0.Valore17 += (Round(((ToFloat(iVar0[1]) / 128f) * uParam0.Valore19)));
			}
			uParam0.Valore18 += ((((uParam0.Valore17 - uParam0.Valore18) * 0.06f) * fVar5));
			SetCamParams(uParam0.CamEntity.Handle, uParam0.Valore1.X, uParam0.Valore1.Y, uParam0.Valore1.Z, (uParam0.Valore4 + uParam0.Valore14).X, (uParam0.Valore4 + uParam0.Valore14).Y, (uParam0.Valore4 + uParam0.Valore14).Z, uParam0.Valore18, 0, 1, 1, 2);
			uParam0.CamEntity.Rotation = (Game.PlayerPed.Rotation + Cam2Tastiera.Valore14);
		}

		static float func_102(float fParam0, float fParam1, float fParam2)
		{
			return (fParam0 > fParam2) ? fParam2 : (fParam0 < fParam1) ? fParam1 : fParam0;
		}


		static int func_103(int iParam0, int iParam1, int iParam2)
		{
			return (iParam0 > iParam2) ? iParam2 : (iParam0 < iParam1) ? iParam1 : iParam0;
		}

		static void func_104(ref int uParam0, ref int uParam1, ref int uParam2, ref int uParam3, bool bParam4, bool bParam5)
		{
			uParam0 = Floor((GetControlUnboundNormal(2, 218) * 127f));
			uParam1 = Floor((GetControlUnboundNormal(2, 219) * 127f));
			uParam2 = Floor((GetControlUnboundNormal(2, 220) * 127f));
			uParam3 = Floor((GetControlUnboundNormal(2, 221) * 127f));
			if (bParam4)
			{
				if (!IsControlEnabled(2, 218))
					uParam0 = Floor((GetDisabledControlUnboundNormal(2, 218) * 127f));
				if (!IsControlEnabled(2, 219))
					uParam1 = Floor((GetDisabledControlUnboundNormal(2, 219) * 127f));
				if (!IsControlEnabled(2, 220))
					uParam2 = Floor((GetDisabledControlUnboundNormal(2, 220) * 127f));
				if (!IsControlEnabled(2, 221))
					uParam3 = Floor((GetDisabledControlUnboundNormal(2, 221) * 127f));
			}
			if (IsInputDisabled(2))
			{
				if (bParam5)
				{
					if (IsLookInverted())
						uParam3 *= -1;
					if (N_0xe1615ec03b3bb4fd())
						uParam3 *= -1;
				}
			}
		}

		private static void ForceState(string state) => Ruota.State = state;

		static Vector3 func_147(int iParam0)
		{
			float fVar0 = 6.28319f / 16 * iParam0;
			return GetOffsetFromEntityInWorldCoords(Ruota.Entity.Handle, 0f, Funzioni.Deg2rad(15.3f) * Funzioni.Rad2deg((float)Math.Sin(fVar0)), Funzioni.Deg2rad(-15.3f) * Funzioni.Rad2deg((float)Math.Cos(fVar0)));
		}

		private static void OnStop(string name)
		{
			if (name == GetCurrentResourceName())
			{
				if (Ruota.Entity != null)
				{
					Ruota.Entity.Delete();
					for (int i = 0; i < 16; i++)
						Cabine[i].Entity.Delete();
				}
			}
		}

		private static int GetCabIndex(Prop cab)
		{
			foreach (var cabina in Cabine)
			{
				if (cab == cabina.Entity)
					return cabina.Index;
			}

			return -1;
		}


	}

	internal class RuotaPan
	{
		public Prop Entity;
		public int Gradient;
		public float Rotazione;
		public string State = "IDLE";
		public float Velocità = 5f; // 2f
		public bool Ferma = false;
		public RuotaPan(Prop entity, int gradient, string state)
		{
			Entity = entity;
			Gradient = gradient;
			Rotazione = 0f;
			State = state;
		}
	}

	internal class RuotaPanoramicaCamTastiera
	{
		public Camera CamEntity = new Camera(0);
		public float Valore0 = 0;
		public Vector3 Valore1;
		public float Valore2 = 0;
		public float Valore3 = 0;
		public Vector3 Valore4;
		public float Valore5 = 0;
		public float Valore6 = 0;
		public float Valore7 = 0;
		public Vector3 Valore8;
		public float Valore9 = 0;
		public float Valore10 = 0;
		public Vector3 Valore11;
		public float Valore12 = 0;
		public float Valore13 = 0;
		public Vector3 Valore14;
		public float Valore15 = 0;
		public float Valore16 = 0;
		public float Valore17 = 0;
		public float Valore18 = 0;
		public int Valore19 = 0;
		public int Valore20 = 0;
		public int Valore21 = 0;
		public int Valore22 = 0;
		public int Valore23 = 0;
		public int Valore24 = 0;
		public int Valore25 = 0;
		public bool Valore26 = false;
		public float Valore27 = 0;
		public bool Valore28 = false;
		public float Valore29 = 0;
		public float Valore30 = 0;
	}

	internal class RuotaPanoramicaCamGamePad
	{
		public Camera CamEntity = new Camera(0);
		public bool Valore0 = false;
		public bool Valore1 = false;
		public Vector3 Valore2 = new Vector3(0);
		public float Valore3 = 0;
		public float Valore4;
		public Vector3 Valore5 = new Vector3(0);
		public float Valore6 = 0;
		public float Valore7 = 0;
		public int Valore8;
		public int Valore9 = 0;
		public float Valore10 = 0;
		public float Valore11;
		public float Valore12 = 0;
		public float Valore13 = 0;
		public bool Valore14 = true;
		public bool Valore15 = true;
		public float Valore16 = 0;
		public float Valore17 = 0;
		public float Valore18 = 0;
		public int Valore19 = 0;
		public int Valore20 = 0;
		public int Valore21 = 0;
		public int Valore22 = 0;
		public int Valore23 = 0;
		public int Valore24 = 0;
		public int Valore25 = 0;
		public float Valore26 = 0;
		public float Valore27 = 0;
		public float Valore28 = 0;
		public float Valore29 = 0;
		public float Valore30 = 0;
	}

	internal class CabinaPan
	{
		public Prop Entity = new Prop(0);
		public int Index;
		public bool PlayerSeduto = false;
		public int NPlayer = 0;
		public float Gradient;
		public CabinaPan(int index) 
		{
			Index = index;
			Gradient = (360f / 16) * Index;
		}
	}
}
