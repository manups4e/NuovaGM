using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Handlers;
using ScaleformUI;
//using ScaleformUI.PauseMenu;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Personale;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions
{
	internal static class PrimaPersonaObbligatoria
	{
		private static bool Switched = false;
		private static int vecchiaMod = 2;
		public static void Init() { Client.Instance.AddTick(WeaponHandling); }

		public static void Stop()
		{
			Client.Instance.RemoveTick(WeaponHandling);
		}

		//aggiungere controlli impostazioni.. V
		//migliorare coordinate testa e aggiungere effetto grafico.. V
		//migliorare precisione in corsa e in genera in copertura e non.. V 
		private static async Task WeaponHandling()
		{
			Ped p = Cache.PlayerCache.MyPlayer.Ped;

			#region MiraPrimaPersona

			if (Main.ImpostazioniClient.ForzaPrimaPersona_Mira)
			{
				if (Input.IsControlPressed(Control.Aim))
				{
					if (p.IsAiming || p.IsAimingFromCover)
						if ((Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo ? GetFollowPedCamViewMode() : GetFollowVehicleCamViewMode()) != 4)
							if (!Switched)
							{
								Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
								Switched = true;
								vecchiaMod = Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo ? GetFollowVehicleCamViewMode() : GetFollowPedCamViewMode();
								Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
								Camera CamFinale = World.CreateCamera(p.Bones[Bone.SKEL_Head].Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
								World.RenderingCamera = CamIniziale;
								CamIniziale.InterpTo(CamFinale, 500, 1, 1);
								while (CamFinale.IsInterpolating) await BaseScript.Delay(0);
								if (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo)
									SetFollowVehicleCamViewMode(4);
								else
									SetFollowPedCamViewMode(4);
								RenderScriptCams(false, false, 500, true, true);
								CamIniziale.Delete();
								CamFinale.Delete();
							}
				}
				else
				{
					if (!p.IsAiming && !p.IsAimingFromCover && !p.IsInCover() && !Main.ImpostazioniClient.ForzaPrimaPersona_InAuto)
						if (Switched)
						{
							Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
							Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
							World.RenderingCamera = CamIniziale;
							await BaseScript.Delay(100);
							if (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo)
								SetFollowVehicleCamViewMode(vecchiaMod);
							else
								SetFollowPedCamViewMode(vecchiaMod);
							await BaseScript.Delay(100);
							Camera CamFinale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
							CamIniziale.InterpTo(CamFinale, 500, 1, 1);
							while (CamFinale.IsInterpolating) await BaseScript.Delay(10);
							RenderScriptCams(false, true, 500, true, true);
							CamIniziale.Delete();
							CamFinale.Delete();
							Switched = false;
						}
				}
			}

			#endregion

			#region CoperturaPrimaPersona

			if (Main.ImpostazioniClient.ForzaPrimaPersona_InCopertura)
			{
				if (p.IsGoingIntoCover)
				{
					if (!Switched)
					{
						Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
						Switched = true;
						vecchiaMod = GetFollowPedCamViewMode();
						int timer = GetGameTimer();

						while (!p.IsInCover())
						{
							await BaseScript.Delay(20);

							if (GetGameTimer() - timer > 10000)
							{
								Client.Logger.Debug( "No veh vicini.. break");

								return;
							}
						}

						Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
						Camera CamFinale = World.CreateCamera(p.Bones[Bone.SKEL_Head].Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
						World.RenderingCamera = CamIniziale;
						CamIniziale.InterpTo(CamFinale, 500, 1, 1);
						while (CamFinale.IsInterpolating) await BaseScript.Delay(0);
						RenderScriptCams(false, false, 500, true, true);
						CamIniziale.Delete();
						CamFinale.Delete();
						SetFollowPedCamViewMode(4);
					}
				}
				else
				{
					if (Switched && !p.IsInCover() && !Input.IsControlPressed(Control.Aim) && !(Main.ImpostazioniClient.ForzaPrimaPersona_InAuto && Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo))
					{
						Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
						Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
						World.RenderingCamera = CamIniziale;
						await BaseScript.Delay(100);
						SetFollowPedCamViewMode(vecchiaMod);
						await BaseScript.Delay(100);
						Camera CamFinale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
						CamIniziale.InterpTo(CamFinale, 500, 1, 1);
						while (CamFinale.IsInterpolating) await BaseScript.Delay(10);
						RenderScriptCams(false, true, 500, true, true);
						CamIniziale.Delete();
						CamFinale.Delete();
						Switched = false;
					}
				}
			}

			#endregion

			#region GuidaPrimaPersona

			if (Main.ImpostazioniClient.ForzaPrimaPersona_InAuto)
				if (Input.IsControlJustPressed(Control.VehicleExit))
				{
					if (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo)
					{
						while (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo) await BaseScript.Delay(0);

						if (Switched)
						{
							Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
							Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
							World.RenderingCamera = CamIniziale;
							await BaseScript.Delay(100);
							SetFollowPedCamViewMode(vecchiaMod);
							await BaseScript.Delay(100);
							Camera CamFinale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
							CamIniziale.InterpTo(CamFinale, 500, 1, 1);
							while (CamFinale.IsInterpolating) await BaseScript.Delay(10);
							RenderScriptCams(false, true, 500, true, true);
							CamIniziale.Delete();
							CamFinale.Delete();
							Switched = false;
						}
					}
					else
					{
						vecchiaMod = GetFollowPedCamViewMode();
						int timer = GetGameTimer();

						while (!p.IsSittingInVehicle())
						{
							await BaseScript.Delay(20);

							if (GetGameTimer() - timer > 10000)
							{
								Client.Logger.Debug( "No veh vicini.. break");

								return;
							}
						}

						if (!Switched)
						{
							Screen.Effects.Start(ScreenEffect.CamPushInNeutral);
							Switched = true;
							Camera CamIniziale = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
							Camera CamFinale = World.CreateCamera(p.Bones[Bone.SKEL_Head].Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
							World.RenderingCamera = CamIniziale;
							CamIniziale.InterpTo(CamFinale, 500, 1, 1);
							while (CamFinale.IsInterpolating) await BaseScript.Delay(0);
							RenderScriptCams(false, false, 500, true, true);
							CamIniziale.Delete();
							CamFinale.Delete();
							SetFollowVehicleCamViewMode(4);
						}
					}
				}

			#endregion
		}
	}
}