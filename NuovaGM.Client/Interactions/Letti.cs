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
		private static LettoLow LettoLow = new LettoLow();
		private static LettoHigh LettoHigh = new LettoHigh();

		public static async void Init()
		{
			RequestAnimDict("mp_bedmid");
			while (!HasAnimDictLoaded("mp_bedmid")) await BaseScript.Delay(1);

			LettoHigh.Lista[0].fLocal_397 = 1.75f;
			LettoHigh.Lista[0].fLocal_464 = 0;
			LettoHigh.Lista[0].vLocal_414 = new Vector3(-796.3056f, 337.3367f, 202.4136f);
			LettoHigh.Lista[0].vLocal_417 = new Vector3(-793.9697f, 337.3367f, 200.4136f);
			LettoHigh.Lista[0].vLocal_468 = new Vector3(-795.8910f, 338.6630f, 200.8270f);
			LettoHigh.Lista[0].vLocal_471 = new Vector3(0.0000f, 0.0000f, -5.7600f);
			LettoHigh.Lista[0].vLocal_426 = new Vector3(-794.1949f, 339.9690f, 200.4136f);
			LettoHigh.Lista[0].vLocal_429 = new Vector3(-796.4470f, 339.9556f, 202.4136f);
			LettoHigh.Lista[0].vLocal_468 = new Vector3(-794.9370f, 340.6300f, 201.4280f);
			LettoHigh.Lista[0].vLocal_471 = new Vector3(0.0000f, 0.0000f, 129.2400f);

			LettoHigh.Lista[0].vLocal_449 = LettoHigh.Lista[0].vLocal_471;
			LettoHigh.Lista[0].vLocal_446 = LettoHigh.Lista[0].vLocal_468;

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
						HUD.ShowNotification("Cambia personaggio");
				}
			}
			if (World.GetDistance(Game.PlayerPed.Position, LettoLow.vLocal_343) < 3f)
			{
				if (!LettoLow.ALetto)
				{
					HUD.ShowHelp(GetLabelText("SA_BED_IN"));
					if (Game.IsControlJustPressed(0, Control.Context))
						LettoLow.Sdraiati();
				}
				else
				{
					HUD.ShowHelp(GetLabelText("SA_BED_OUT") + "Premi ~INPUT_FRONTEND_X~ per cambiare personaggio.");
					if (Game.IsControlJustPressed(0, Control.ScriptRUp))
						LettoLow.ScendiDalLetto();
					else if (Game.IsControlJustPressed(0, Control.FrontendX))
						HUD.ShowNotification("Cambia personaggio");
				}
			}
			if (World.GetDistance(Game.PlayerPed.Position, LettoHigh.Lista[0].vLocal_446) < 3f)
			{
				if (!LettoHigh.ALetto)
				{
					HUD.ShowHelp(GetLabelText("SA_BED_IN"));
					if (Game.IsControlJustPressed(0, Control.Context))
						LettoHigh.Sdraiati();
				}
				else
				{
					HUD.ShowHelp(GetLabelText("SA_BED_OUT") + "Premi ~INPUT_FRONTEND_X~ per cambiare personaggio.");
					if (Game.IsControlJustPressed(0, Control.ScriptRUp))
						LettoHigh.ScendiDalLetto();
					else if (Game.IsControlJustPressed(0, Control.FrontendX))
						HUD.ShowNotification("Cambia personaggio");
				}

			}
		}
	}

	internal class LettoMidLowHigh
	{
		public virtual async void Sdraiati()
		{

		}
		public virtual async void ScendiDalLetto()
		{

		}

		protected bool NoPGVicini(Vector3 n1, Vector3 n2)
		{
			Ped[] peds = World.GetAllPeds();
			if (peds.Length > 0)
			{
				int i;
				for (i = 0; i <= 7; i++)
					if (!peds[i].IsInjured)
						if (IsEntityInAngledArea(peds[i].Handle, n1.X, n1.Y, n1.Z, n2.X, n2.Y, n2.Z, 2f, false, true, 0))
							return false;
			}
			return true;
		}

		protected bool Controllo1(Vector3 vParam0, float fParam3)
		{
			if (IsPlayerPlaying(PlayerId()))
				if (!IsPedInAnyVehicle(PlayerPedId(), false) && !IsEntityOnFire(PlayerPedId()) && IsPlayerControlOn(PlayerId()))
					if (!IsExplosionInSphere(-1, vParam0.X, vParam0.Y, vParam0.Z, 2f))
						if (IsGameplayCamRendering() && !IsCinematicCamRendering())
							if (Controllo2(Game.PlayerPed.Position, vParam0, fParam3, false))
								return true;
			return false;
		}

		protected bool Controllo2(Vector3 vParam0, Vector3 vParam3, float fParam6, bool bParam7)
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

	internal class LettoMid : LettoMidLowHigh
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

		public override async void Sdraiati()
		{
			Vector3 vVar0 = new Vector3(1.5f);
			Vector3 vVar3;
			if (GetFollowPedCamViewMode() == 4)
				SetFollowPedCamViewMode(1);

			if (Controllo1(vLocal_338, vVar0.X) && IsEntityInAngledArea(PlayerPedId(), vLocal_342.X, vLocal_342.Y, vLocal_342.Z, vLocal_345.X, vLocal_345.Y, vLocal_345.Z, 2f, false, true, 0) && NoPGVicini(vLocal_342, vLocal_345))
			{
				Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
				vLocal_338 = GetAnimInitialOffsetPosition(sLocal_334, sLocal_335, vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 0, 2);
				vVar3 = GetAnimInitialOffsetRotation(sLocal_334, sLocal_335, vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 0, 2) ;
				fLocal_341 = vVar3.Z;
				TaskGoStraightToCoord(PlayerPedId(), vLocal_338.X, vLocal_338.Y, vLocal_338.Z, 1f, 5000, fLocal_341, 0.05f);

				await BaseScript.Delay(2000);

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

		public override async void ScendiDalLetto()
		{
			uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, true, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_336, 8f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(uLocal_331);

			uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_337, 1000f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(uLocal_331);

			ALetto = false;
		}
	}

	internal class LettoLow : LettoMidLowHigh
	{
		public string sLocal_333 = "mp_bedmid";
		public string sLocal_334 = "f_getin_l_bighouse";
		public string sLocal_335 = "f_sleep_l_loop_bighouse";
		public string sLocal_336 = "f_getout_l_bighouse";
		public Vector3 vLocal_337 = new Vector3(262.9207f, -1002.98f, -100.0086f);
		public Vector3 vLocal_340 = new Vector3(261.0173f, -1002.98f, -98.0086f);
		public Vector3 vLocal_343 = new Vector3(261.8297f, -1002.928f, -99.0062f);
		public float fLocal_346 = 230.5943f;
		public Vector3 vLocal_347 = new Vector3(262.74f, -1004.344f, -99.575f);
		public Vector3 vLocal_350 = new Vector3(0f, 0f, -162.36f);
		public int uLocal_330 = 0;
		public int uLocal_331 = 0;
		public bool ALetto = false;

		public async void Sdraiati()
		{
			Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
			Vector3 vVar0 = new Vector3(1.5f);
			Vector3 vVar3;
			if (GetFollowPedCamViewMode() == 4)
				SetFollowPedCamViewMode(1);
			if (Controllo1(vLocal_343, vVar0.X) && IsEntityInAngledArea(PlayerPedId(), vLocal_337.X, vLocal_337.Y, vLocal_337.Z, vLocal_340.X, vLocal_340.Y, vLocal_340.Z, 2f, false, true, 0) && NoPGVicini(vLocal_337, vLocal_340))
			{
				vLocal_343 =  GetAnimInitialOffsetPosition(sLocal_333, sLocal_334, vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 0, 2);
				vVar3 = GetAnimInitialOffsetRotation(sLocal_333, sLocal_334, vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 0, 2);
				fLocal_346 = vVar3.Z;
				TaskGoStraightToCoord(PlayerPedId(), vLocal_343.X, vLocal_343.Y, vLocal_343.Z, 1f, 5000, fLocal_346, 0.05f);
				await BaseScript.Delay(2000);

				uLocal_330 = NetworkCreateSynchronisedScene(vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 2, false, false, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_330, sLocal_333, sLocal_334, 4f, -2f, 261, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(uLocal_330);

				await BaseScript.Delay(1000);

				uLocal_331 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_331);

				while (GetSynchronizedScenePhase(uLocal_331) < 0.9f) await BaseScript.Delay(1);

				uLocal_330 = NetworkCreateSynchronisedScene(vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 2, false, true, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_330, sLocal_333, sLocal_335, 8f, -2f, 261, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(uLocal_330);

				uLocal_331 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_331);
				SetSynchronizedSceneLooped(uLocal_331, true);
				ALetto = true;

			}
		}

		public async void ScendiDalLetto()
		{
			uLocal_330 = NetworkCreateSynchronisedScene(vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_330, sLocal_333, sLocal_336, 2f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(uLocal_330);

			uLocal_330 = NetworkCreateSynchronisedScene(vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_330, sLocal_333, sLocal_336, 1000f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(uLocal_330);

			ALetto = false;
		}

	}

	internal class LettoHigh : LettoMidLowHigh
	{
		public string sLocal_413 = "mp_bedmid";
		public bool ALetto = false;

		private string func_234(bool iParam0) => !iParam0 ? "f_getin_l_bighouse" : "f_getin_r_bighouse";
		private string func_9(bool iParam0) => !iParam0 ? "f_getout_l_bighouse" : "f_getout_r_bighouse";
		private string func_272(bool iParam0) => !iParam0 ? "f_sleep_l_loop_bighouse" : "f_sleep_r_loop_bighouse";

		public class LettiCoordsAnim
		{
			public Vector3 vLocal_414;
			public Vector3 vLocal_417;
			public Vector3 vLocal_452;
			public Vector3 vLocal_455;
			public Vector3 vLocal_420;
			public Vector3 vLocal_423;
			public Vector3 vLocal_458;
			public Vector3 vLocal_461;
			public Vector3 vLocal_468;
			public Vector3 vLocal_471;
			public Vector3 vLocal_426;
			public Vector3 vLocal_429;
			public Vector3 vLocal_446;
			public Vector3 vLocal_449;
			public Vector3 vLocal_438;

			public float fLocal_397;
			public float fLocal_464;
			public float fLocal_402;

			public int uLocal_409;
			public int uLocal_410;
		}

		public List<LettiCoordsAnim> Lista = new List<LettiCoordsAnim>()
		{
			new LettiCoordsAnim()
		};

		public async override void Sdraiati()
		{
			Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
			Vector3 vVar3;
			if (GetFollowPedCamViewMode() == 4)
				SetFollowPedCamViewMode(1);

			Lista[0].vLocal_438 = GetAnimInitialOffsetPosition(sLocal_413, func_234(true), Lista[0].vLocal_446.X, Lista[0].vLocal_446.Y, Lista[0].vLocal_446.Z, Lista[0].vLocal_449.X, Lista[0].vLocal_449.Y, Lista[0].vLocal_449.Z, 0, 2);
			vVar3 = GetAnimInitialOffsetRotation(sLocal_413, func_234(true), Lista[0].vLocal_446.X, Lista[0].vLocal_446.Y, Lista[0].vLocal_446.Z, Lista[0].vLocal_449.X, Lista[0].vLocal_449.Y, Lista[0].vLocal_449.Z, 0, 2);
			Lista[0].fLocal_402 = vVar3.Z;
			TaskGoStraightToCoord(PlayerPedId(), Lista[0].vLocal_446.X, Lista[0].vLocal_446.Y, Lista[0].vLocal_446.Z, 1f, 5000, Lista[0].fLocal_402, 0.05f);
			await BaseScript.Delay(2000);

			Lista[0].uLocal_410 = NetworkCreateSynchronisedScene(Lista[0].vLocal_446.X, Lista[0].vLocal_446.Y, Lista[0].vLocal_446.Z, Lista[0].vLocal_449.X, Lista[0].vLocal_449.Y, Lista[0].vLocal_449.Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), Lista[0].uLocal_410, sLocal_413, func_234(true), 4f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(Lista[0].uLocal_410);

			await BaseScript.Delay(1000);

			Lista[0].uLocal_409 = NetworkConvertSynchronisedSceneToSynchronizedScene(Lista[0].uLocal_410);

			while (GetSynchronizedScenePhase(Lista[0].uLocal_409) < 0.9f) await BaseScript.Delay(1);

			Lista[0].uLocal_410 = NetworkCreateSynchronisedScene(Lista[0].vLocal_446.X, Lista[0].vLocal_446.Y, Lista[0].vLocal_446.Z, Lista[0].vLocal_449.X, Lista[0].vLocal_449.Y, Lista[0].vLocal_449.Z, 2, false, true, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), Lista[0].uLocal_410, sLocal_413, func_272(true), 8f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(Lista[0].uLocal_410);

			Lista[0].uLocal_409 = NetworkConvertSynchronisedSceneToSynchronizedScene(Lista[0].uLocal_410);
			SetSynchronizedSceneLooped(Lista[0].uLocal_409, true);
			ALetto = true;
		}

		public async override void ScendiDalLetto()
		{
			Lista[0].uLocal_410 = NetworkCreateSynchronisedScene(Lista[0].vLocal_446.X, Lista[0].vLocal_446.Y, Lista[0].vLocal_446.Z, Lista[0].vLocal_449.X, Lista[0].vLocal_449.Y, Lista[0].vLocal_449.Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), Lista[0].uLocal_410, sLocal_413, func_9(true), 2f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(Lista[0].uLocal_410);

			Lista[0].uLocal_410 = NetworkCreateSynchronisedScene(Lista[0].vLocal_446.X, Lista[0].vLocal_446.Y, Lista[0].vLocal_446.Z, Lista[0].vLocal_449.X, Lista[0].vLocal_449.Y, Lista[0].vLocal_449.Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), Lista[0].uLocal_410, sLocal_413, func_9(true), 1000f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(Lista[0].uLocal_410);

			ALetto = false;

		}
	}
}
