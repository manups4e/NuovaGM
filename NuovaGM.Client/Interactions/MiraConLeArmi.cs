using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.MenuNativo.PauseMenu;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.Personale;

namespace TheLastPlanet.Client.Interactions
{
	public enum TipoDiMira
	{
		PrimaPersona,
		TerzaPersona
	}
	static class MiraConLeArmi
	{
		public static TipoDiMira Mira = TipoDiMira.PrimaPersona;
		private static bool Switched = false;
		private static int vecchiaMod = 2;
		public static void Init()
		{
			Client.Instance.AddTick(WeaponHandling);
		}

		//aggiungere controlli impostazioni.. V
		//migliorare coordinate testa e aggiungere effetto grafico..
		//migliorare precisione in corsa e in genera in copertura e non..
		private static async Task WeaponHandling()
		{
			Ped p = Game.PlayerPed;
			if (Main.ImpostazioniClient.ForzaPrimaPersona_Mira)
			{
				if (Input.IsControlPressed(Control.Aim))
				{
					if (p.IsAiming || p.IsAimingFromCover)
					{
						if (GetFollowPedCamViewMode() != 4)
						{
							if (Mira == TipoDiMira.PrimaPersona)
							{
								if (!Switched)
								{
									Switched = true;
									vecchiaMod = GetFollowPedCamViewMode();
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
						}
					}
				}
				else
				{
					if (!p.IsAiming && !p.IsAimingFromCover && (!p.IsInCover()))
					{
						if (Switched)
						{
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
			}
			if (Main.ImpostazioniClient.ForzaPrimaPersona_InCopertura)
			{
				if (p.IsGoingIntoCover)
				{
					if (!Switched)
					{
						Switched = true;
						vecchiaMod = GetFollowPedCamViewMode();
						while (!p.IsInCover()) await BaseScript.Delay(0);
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
					if (Switched && !p.IsInCover() && !Input.IsControlPressed(Control.Aim))
					{
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
		}
	}
}
