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

		private static Vector3 coord = new Vector3();
		private static Vector3 rot = new Vector3();

		public static async void Init()
		{
			RequestAnimDict("mp_bedmid");
			while (!HasAnimDictLoaded("mp_bedmid")) await BaseScript.Delay(1);
			Client.GetInstance.RegisterTickHandler(Letto);
		}

		public static async Task Letto()
		{
			if (!IsEntityPlayingAnim(PlayerPedId(), "mp_bedmid", "f_getin_l_bighouse", 2) &&
				!IsEntityPlayingAnim(PlayerPedId(), "mp_bedmid", "f_getin_r_bighouse", 2) &&
				!IsEntityPlayingAnim(PlayerPedId(), "mp_bedmid", "f_getout_l_bighouse", 2) &&
					!IsEntityPlayingAnim(PlayerPedId(), "mp_bedmid", "f_getout_r_bighouse", 2))
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

				for (int i = 0; i < LettoHigh.Lista.Count; i++)
				{
					if (World.GetDistance(Game.PlayerPed.Position, LettoHigh.Lista[i].Destra1) < 2f && !LettoHigh.ALettoDestra && !LettoHigh.ALettoSinistra)
					{
						LettoHigh.Lista[i].RotAnim = LettoHigh.Lista[i].RotAnimStaticDestra;
						LettoHigh.Lista[i].CoordAnim = LettoHigh.Lista[i].CoordsAnimStaticDestra;
						coord = LettoHigh.Lista[i].CoordAnim;
						rot = LettoHigh.Lista[i].RotAnim;
						HUD.ShowHelp(GetLabelText("SA_BED_IN"));
						if (Game.IsControlJustPressed(0, Control.Context))
							LettoHigh.Sdraiati(LettoHigh.Lista[i], true);
					}
					else if (World.GetDistance(Game.PlayerPed.Position, LettoHigh.Lista[i].Sinistra1) < 2f && !LettoHigh.ALettoDestra && !LettoHigh.ALettoSinistra)
					{
						LettoHigh.Lista[i].RotAnim = LettoHigh.Lista[i].RotAnimStaticSinistra;
						LettoHigh.Lista[i].CoordAnim = LettoHigh.Lista[i].CoordsAnimStaticSinistra;
						coord = LettoHigh.Lista[i].CoordAnim;
						rot = LettoHigh.Lista[i].RotAnim;
						HUD.ShowHelp(GetLabelText("SA_BED_IN"));
						if (Game.IsControlJustPressed(0, Control.Context))
							LettoHigh.Sdraiati(LettoHigh.Lista[i], false);
					}
					else if (LettoHigh.ALettoDestra || LettoHigh.ALettoSinistra)
					{
						HUD.ShowHelp(GetLabelText("SA_BED_OUT") + "Premi ~INPUT_FRONTEND_X~ per cambiare personaggio.");
						if (Game.IsControlJustPressed(0, Control.ScriptRUp))
							LettoHigh.ScendiDalLetto(new Vector3[2] { coord, rot }, LettoHigh.ALettoDestra ? true : false);
						else if (Game.IsControlJustPressed(0, Control.FrontendX))
							HUD.ShowNotification("Cambia personaggio");
					}
				}
			}
		}
	}

	internal class LettoBase
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

	internal class LettoMid : LettoBase
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

	internal class LettoLow : LettoBase
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

	internal class LettoHigh : LettoBase
	{
		public string sLocal_413 = "mp_bedmid";
		public bool ALettoDestra = false;
		public bool ALettoSinistra = false;

		private string func_234(bool iParam0) => !iParam0 ? "f_getin_l_bighouse" : "f_getin_r_bighouse";
		private string func_9(bool iParam0) => !iParam0 ? "f_getout_l_bighouse" : "f_getout_r_bighouse";
		private string func_272(bool iParam0) => !iParam0 ? "f_sleep_l_loop_bighouse" : "f_sleep_r_loop_bighouse";

		public class LettiCoordsAnim
		{
			public Vector3 Sinistra1;
			public Vector3 Sinistra2;
			public Vector3 CoordsAnimStaticDestra;
			public Vector3 RotAnimStaticDestra;
			public Vector3 CoordsAnimStaticSinistra;
			public Vector3 RotAnimStaticSinistra;
			public Vector3 Destra1;
			public Vector3 Destra2;
			public Vector3 CoordAnim;
			public Vector3 RotAnim;
			public Vector3 vLocal_438;

			public float fLocal_402;

			public int uLocal_409;
			public int uLocal_410;
		}

		public List<LettiCoordsAnim> Lista = new List<LettiCoordsAnim>()
		{
			new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(),
			new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(),
		};

		public LettoHigh()
		{
			#region letto 1
			Lista[0].Sinistra1 = new Vector3(-796.3056f, 337.3367f, 202.4136f);
			Lista[0].Sinistra2 = new Vector3(-793.9697f, 337.3367f, 200.4136f);

			Lista[0].CoordsAnimStaticSinistra = new Vector3(-795.8910f, 338.6630f, 200.8270f);
			Lista[0].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -5.7600f);

			Lista[0].Destra1 = new Vector3(-794.1949f, 339.9690f, 200.4136f);
			Lista[0].Destra2 = new Vector3(-796.4470f, 339.9556f, 202.4136f);

			Lista[0].CoordsAnimStaticDestra = new Vector3(-794.9370f, 340.6300f, 201.4280f);
			Lista[0].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 129.2400f);
			#endregion

			#region letto 2
			Lista[1].Sinistra1 = new Vector3(126.4232f, 545.7162f, 179.5227f);
			Lista[1].Sinistra2 = new Vector3(125.1505f, 546.0822f, 180.5208f);

			Lista[1].CoordsAnimStaticSinistra = new Vector3(125.6500f, 544.4750f, 179.9700f);
			Lista[1].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -177.8400f);

			Lista[1].Destra1 = new Vector3(126.7189f, 542.8882f, 179.5227f);
			Lista[1].Destra2 = new Vector3(124.6794f, 542.6829f, 181.5227f);

			Lista[1].CoordsAnimStaticDestra = new Vector3(124.8870f, 541.9250f, 180.5120f);
			Lista[1].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, -41.7600f);
			#endregion

			#region letto 3
			Lista[2].Sinistra1 = new Vector3(-796.3056f, 337.3367f, 202.4136f);
			Lista[2].Sinistra2 = new Vector3(-793.9697f, 337.3367f, 200.4136f);

			Lista[2].CoordsAnimStaticSinistra = new Vector3(-795.8910f, 338.6630f, 200.8270f);
			Lista[2].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -5.7600f);

			Lista[2].Destra1 = new Vector3(-794.1949f, 339.9690f, 200.4136f);
			Lista[2].Destra2 = new Vector3(-796.4470f, 339.9556f, 202.4136f);

			Lista[2].CoordsAnimStaticDestra = new Vector3(-794.9370f, 340.6300f, 201.4280f);
			Lista[2].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 129.2400f);
			#endregion
			
			#region letto 4

			Lista[3].Sinistra1 = new Vector3(-792.4382f, 332.6826f, 209.7966f);
			Lista[3].Sinistra2 = new Vector3(-794.6772f, 332.6597f, 211.7966f);

			Lista[3].CoordsAnimStaticSinistra = new Vector3(-793.5940f, 333.7590f, 210.2250f);
			Lista[3].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -1.0000f);

			Lista[3].Destra1 = new Vector3(-794.6525f, 335.1653f, 209.7966f);
			Lista[3].Destra2 = new Vector3(-792.5333f, 335.1687f, 211.7966f);

			Lista[3].CoordsAnimStaticDestra = new Vector3(-792.6250f, 335.8620f, 210.8130f);
			Lista[3].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 133.2000f);
			#endregion

			#region letto 5
			Lista[4].Sinistra1 = new Vector3(-162.6263f, 484.8703f, 132.8697f);
			Lista[4].Sinistra2 = new Vector3(-164.7478f, 484.4771f, 134.8697f);

			Lista[4].CoordsAnimStaticSinistra = new Vector3(-163.4570f, 483.4740f, 133.2820f);
			Lista[4].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -166.6800f);

			Lista[4].Destra1 = new Vector3(-162.3055f, 482.5231f, 132.8697f);
			Lista[4].Destra2 = new Vector3(-164.4513f, 482.0749f, 134.8697f);

			Lista[4].CoordsAnimStaticDestra = new Vector3(-163.9880f, 481.3370f, 133.8630f);
			Lista[4].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, -31.6800f);
			#endregion

			#region letto 6 
			Lista[5].Sinistra1 = new Vector3(-796.5303f, 334.3555f, 189.7135f);
			Lista[5].Sinistra2 = new Vector3(-796.5093f, 336.8433f, 191.7135f);

			Lista[5].CoordsAnimStaticSinistra = new Vector3(-797.6820f, 335.6830f, 190.1550f);
			Lista[5].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 90f);

			Lista[5].Destra1 = new Vector3(-799.1369f, 334.6384f, 189.7135f);
			Lista[5].Destra2 = new Vector3(-799.1272f, 336.8023f, 191.7135f);

			Lista[5].CoordsAnimStaticDestra = new Vector3(-799.7870f, 336.5630f, 190.7500f);
			Lista[5].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, -133.2000f);
			#endregion

			#region letto 7 
			Lista[6].Sinistra1 = new Vector3(-1486.2543f, -3750.0811f, 4.9114f);
			Lista[6].Sinistra2 = new Vector3(-1484.0914f, -3750.7080f, 6.9114f);

			Lista[6].CoordsAnimStaticSinistra = new Vector3(-1485.1100f, -3749.3369f, 5.3490f);
			Lista[6].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -20.1600f);

			Lista[6].Destra1 = new Vector3(-1485.3296f, -3747.1462f, 4.9114f);
			Lista[6].Destra2 = new Vector3(-1483.2997f, -3747.8845f, 6.9114f);

			Lista[6].CoordsAnimStaticDestra = new Vector3(-1483.2990f, -3747.2151f, 5.9150f);
			Lista[6].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 110.5200f);
			#endregion



			/*
				#region letto 
				Lista[].Sinistra1 = new Vector3();
				Lista[].Sinistra2 = new Vector3();

				Lista[].CoordsAnimStaticSinistra = new Vector3();
				Lista[].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, );

				Lista[].Destra1 = new Vector3();
				Lista[].Destra2 = new Vector3();

				Lista[].CoordsAnimStaticDestra = new Vector3();
				Lista[].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, );
				#endregion
			*/



		}

		public async override void Sdraiati() { }
		public async void Sdraiati(LettiCoordsAnim lato, bool destra)
		{
			Vector3 var0 = lato.CoordAnim;
			Vector3 var1 = lato.RotAnim;

			Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
			Vector3 vVar3;
			if (GetFollowPedCamViewMode() == 4)
				SetFollowPedCamViewMode(1);

			lato.vLocal_438 = GetAnimInitialOffsetPosition(sLocal_413, func_234(destra), var0.X, var0.Y, var0.Z, var1.X, var1.Y, var1.Z, 0, 2);
			vVar3 = GetAnimInitialOffsetRotation(sLocal_413, func_234(destra), var0.X, var0.Y, var0.Z, var1.X, var1.Y, var1.Z, 0, 2);
			lato.fLocal_402 = vVar3.Z;
			TaskGoStraightToCoord(PlayerPedId(), var0.X, var0.Y, var0.Z, 1f, 5000, lato.fLocal_402, 0.05f);
			await BaseScript.Delay(2000);

			lato.uLocal_410 = NetworkCreateSynchronisedScene(var0.X, var0.Y, var0.Z, var1.X, var1.Y, var1.Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), lato.uLocal_410, sLocal_413, func_234(destra), 4f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(lato.uLocal_410);

			await BaseScript.Delay(1000);

			lato.uLocal_409 = NetworkConvertSynchronisedSceneToSynchronizedScene(lato.uLocal_410);

			while (GetSynchronizedScenePhase(lato.uLocal_409) < 0.9f) await BaseScript.Delay(1);

			lato.uLocal_410 = NetworkCreateSynchronisedScene(var0.X, var0.Y, var0.Z, var1.X, var1.Y, var1.Z, 2, false, true, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), lato.uLocal_410, sLocal_413, func_272(destra), 8f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(lato.uLocal_410);

			lato.uLocal_409 = NetworkConvertSynchronisedSceneToSynchronizedScene(lato.uLocal_410);
			SetSynchronizedSceneLooped(lato.uLocal_409, true);
			ALettoDestra = destra;
			ALettoSinistra = !destra;
		}

		public async override void ScendiDalLetto() { }
		public async void ScendiDalLetto(Vector3[] coords, bool destra)
		{
			int uLocal_410 = NetworkCreateSynchronisedScene(coords[0].X, coords[0].Y, coords[0].Z, coords[1].X, coords[1].Y, coords[1].Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_410, sLocal_413, func_9(destra), 2f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(uLocal_410);

			uLocal_410 = NetworkCreateSynchronisedScene(coords[0].X, coords[0].Y, coords[0].Z, coords[1].X, coords[1].Y, coords[1].Z, 2, false, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_410, sLocal_413, func_9(destra), 1000f, -2f, 261, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(uLocal_410);

			ALettoDestra = false;
			ALettoSinistra = false;
		}
	}
}
