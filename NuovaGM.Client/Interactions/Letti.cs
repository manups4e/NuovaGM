using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;

namespace NuovaGM.Client.Interactions
{
	static class Letti
	{
		private static LettoMid LettoMid = new LettoMid();
		public static async void Init()
		{
			RequestAnimDict("mp_bedmid");
			while (!HasAnimDictLoaded("mp_bedmid")) await BaseScript.Delay(1);
			Client.GetInstance.RegisterTickHandler(Letto);
		}

		public static async Task Letto()
		{
			if (World.GetDistance(Game.PlayerPed.Position, LettoMid.vLocal_338) < 3f)
			{
				if (!LettoMid.ALetto)
				{
					HUD.ShowHelp(GetLabelText("SA_BED_IN"));
					if (Game.IsControlJustPressed(0, Control.Context))
						LettoMid.Sdraiati();
				}
				else
				{
					HUD.ShowHelp(GetLabelText("SA_BED_OUT") + "Premi ~INPUT_FRONTEND_X~ per cambiare personaggio.");
					if (Game.IsControlJustPressed(0, Control.ScriptRUp))
						LettoMid.ScendiDalLetto();
					else if (Game.IsControlJustPressed(0, Control.FrontendX))
					{
						HUD.ShowNotification("Cambia personaggio <3");
					}
				}
			}
		}
	}


	internal class LettoMid
	{
		public string sLocal_334 = "mp_bedmid";
		public string sLocal_335 = "f_getin_l_bighouse";
		public string sLocal_336 = "f_sleep_l_loop_bighouse";
		public string sLocal_337 = "f_getout_l_bighouse";
		public Vector3 vLocal_338 = new Vector3(349.9853f, -997.8344f, -99.1952f);
		public Vector3 vLocal_342 = new Vector3(349f, -997.3587f, -100.5f);
		public Vector3 vLocal_345 = new Vector3(351.74f, -997.3587f, -97f);
		public Vector3 vLocal_348 = new Vector3(349.66f, -996.183f, -99.764f);
		public Vector3 vLocal_351 = new Vector3(0f, 0f, -3.96f);
		public int uLocal_331 = 0;
		public int uLocal_332 = 0;
		public float fLocal_341 = 43.8287f;
		public bool ALetto = false;

		public async void Sdraiati()
		{
			Vector3 vVar0 = new Vector3(1.5f);
			Vector3 vVar3;
			if (GetFollowPedCamViewMode() == 4)
				SetFollowPedCamViewMode(1);

			if (func_47(vLocal_338, vVar0.X) && IsEntityInAngledArea(PlayerPedId(), vLocal_342.X, vLocal_342.Y, vLocal_342.Z, vLocal_345.X, vLocal_345.Y, vLocal_345.Z, 2f, false, true, 0) && func_46())
			{
				Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
				vLocal_338 = GetAnimInitialOffsetPosition(sLocal_334, sLocal_335, vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 0, 2);
				vVar3 = GetAnimInitialOffsetRotation(sLocal_334, sLocal_335, vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 0, 2) ;
				fLocal_341 = vVar3.Z;
				TaskGoStraightToCoord(PlayerPedId(), vLocal_338.X, vLocal_338.Y, vLocal_338.Z, 1f, 5000, fLocal_341, 0.05f);

				if (GetFollowPedCamViewMode() == 4)
					SetFollowPedCamViewMode(1);

				uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, false, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_335, 4f, -2f, 261, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(uLocal_331);

				await BaseScript.Delay(1000);

				uLocal_332 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_331);

				while (GetSynchronizedScenePhase(uLocal_332) < 0.9f) await BaseScript.Delay(1);

				uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, true, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_336, 8f, -2f, 261, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(uLocal_331);

				uLocal_332 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_331);
				SetSynchronizedSceneLooped(uLocal_332, true);
				ALetto = true;
			}
		}

		public async void ScendiDalLetto()
		{
			uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, true, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_336, 8f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(uLocal_331);

			uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_337, 1000f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(uLocal_331);

			uLocal_332 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_331);

			if (IsSynchronizedSceneRunning(uLocal_332))
			{
				Debug.WriteLine("Ciao");
			}
			ALetto = false;
		}

		private bool func_46()
		{
			Ped[] peds = World.GetAllPeds();
			if (peds.Length > 0)
			{
				int i;
				for (i = 0; i<=7; i++)
					if (!peds[i].IsInjured)
						if (IsEntityInAngledArea(peds[i].Handle, vLocal_342.X, vLocal_342.Y, vLocal_342.Z, vLocal_345.X, vLocal_345.Y, vLocal_345.Z, 2f, false, true, 0))
							return false;
			}
			return true;
		}

		private bool func_47(Vector3 vParam0, float fParam3)
		{
			if (IsPlayerPlaying(PlayerId()))
				if (!IsPedInAnyVehicle(PlayerPedId(), false) && !IsEntityOnFire(PlayerPedId()) && IsPlayerControlOn(PlayerId()))
					if (!IsExplosionInSphere(-1, vParam0.X, vParam0.Y, vParam0.Z, 2f))
						if (IsGameplayCamRendering() && !IsCinematicCamRendering())
							if (func_48(Game.PlayerPed.Position, vParam0, fParam3, false))
								return true;
			return false;
		}

		private bool func_48(Vector3 vParam0, Vector3 vParam3, float fParam6, bool bParam7)
		{
			if (fParam6 < 0f)
				fParam6 = 0f;
			if (!bParam7)
			{
				if (Absf(vParam0.X - vParam3.X) <= fParam6)
					if (Absf(vParam0.Y - vParam3.Y) <= fParam6)
						if (Absf(vParam0.Z - vParam3.Z) <= fParam6)
							return true;
			}
			else if (Absf(vParam0.X - vParam3.X) <= fParam6)
				if (Absf(vParam0.Y - vParam3.Y) <= fParam6)
					return true;
			return false;
		}



	}
}
