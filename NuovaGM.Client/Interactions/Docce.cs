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
	static class Docce
	{
		private static string sLocal_436;
		private static string sLocal_437;
		private static string sLocal_438;
		private static string sLocal_439;
		private static string sLocal_440;
		private static string sLocal_441;
		private static string sLocal_442;
		private static string sLocal_443;
		private static string sLocal_444;
		private static string sLocal_445;
		private static float Global_2499242_f_20 = 0;
		private static float Global_2499242_f_21 = 0;
		private static float Global_2499242_f_22 = 0;
		private static float Global_2499242_f_23 = 0;
		private static float Global_2499242_f_15 = 0;
		private static float Global_2499242_f_16 = 0;
		private static float Global_2499242_f_17 = 0;
		private static float Global_2499242_f_18 = 0;
		private static bool VicinoDoccia = false;
		private static Prop DocciaPorta;
		private static bool InDoccia = false;
		private static string sLocal_448 = "dlc_EXEC1/MP_APARTMENT_SHOWER_01";
		private static int Scena1;
		private static int Scena2;
		private static int Scena3;
		private static int Scena4;
		private static int Scena5;
		private static int Scena6;
		private static int Scena7;
		private static int Scena8;

		private static List<int> Doccie = new List<int>()
		{
			Funzioni.HashInt("p_mp_showerdoor_s"),
			1358716892,
			879181614,
			-553740697,
		};

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private async static void Spawnato()
		{
			if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
			{
				sLocal_436 = "mp_safehouseshower@male@";
				sLocal_437 = "male_shower_undress_&_turn_on_water";
				sLocal_438 = "male_shower_enter_into_idle";
				sLocal_439 = "male_shower_idle_a";
				sLocal_440 = "male_shower_idle_b";
				sLocal_441 = "male_shower_idle_c";
				sLocal_442 = "male_shower_idle_d";
				sLocal_443 = "Male_Shower_Exit_To_Idle";
				sLocal_444 = "male_shower_undress_&_turn_on_water_door";
				sLocal_445 = "Male_Shower_Exit_To_Idle_Door";
				Global_2499242_f_20 = 0.5f;
				Global_2499242_f_21 = 0.55f;
				Global_2499242_f_22 = 0.833f;
				Global_2499242_f_23 = 0.25f;
				Global_2499242_f_15 = 0.26f;
				Global_2499242_f_16 = 0.9f;
				Global_2499242_f_17 = 0.3f;
				Global_2499242_f_18 = 0.79f;
			}
			else
			{
				sLocal_436 = "mp_safehouseshower@female@";
				sLocal_437 = "shower_undress_&_turn_on_water";
				sLocal_438 = "shower_enter_into_idle";
				sLocal_439 = "shower_idle_a";
				sLocal_440 = "shower_idle_b";
				sLocal_441 = "shower_idle_b";
				sLocal_442 = "shower_idle_a";
				sLocal_443 = "shower_Exit_To_Idle";
				sLocal_444 = "shower_undress_&_turn_on_water_door";
				sLocal_445 = "shower_Exit_To_Idle_Door";
				Global_2499242_f_20 = 0.5f;
				Global_2499242_f_21 = 0.5f;
				Global_2499242_f_22 = 0.384f;
				Global_2499242_f_23 = 0.166f;
				Global_2499242_f_15 = 0.26f;
				Global_2499242_f_16 = 0.9f;
				Global_2499242_f_17 = 0.3f;
				Global_2499242_f_18 = 0.75f;
			}
			RequestAnimDict(sLocal_436);
			while (!HasAnimDictLoaded(sLocal_436)) await BaseScript.Delay(0);
			RequestAmbientAudioBank(sLocal_448, false);
			Client.GetInstance.RegisterTickHandler(ControlloDocceVicino);
			Client.GetInstance.RegisterTickHandler(Docceeee);
		}

		private async static Task ControlloDocceVicino()
		{
			VicinoDoccia = World.GetAllProps().Select(o => new Prop(o.Handle)).Where(o => Doccie.Contains(o.Model.Hash)).Any(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * 1.3f, 2));
			if (VicinoDoccia)
				DocciaPorta = World.GetAllProps().Select(o => new Prop(o.Handle)).Where(o => Doccie.Contains(o.Model.Hash)).First(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * 1.3f, 2));
		}

		private static async Task Docceeee()
		{
			if (VicinoDoccia)
			{
				if (!InDoccia)
				{
					int val = -1750863300;
					DoorSystemSetDoorState((uint)val, 0, false, true);
					HUD.ShowHelp(GetLabelText("SA_SHWR_IN"));
					if (Game.IsControlJustPressed(0, Control.Context))
					{
						func_314();
						//CreaCam(CameraCoords(GetInteriorFromGameplayCam()));
						var doccia = CaricaDoccia(GetInteriorFromEntity(PlayerPedId()));
						Scena1 = CreateSynchronizedScene(DocciaPorta.Position.X, DocciaPorta.Position.Y, DocciaPorta.Position.Z, 0, 0, DocciaPorta.Heading, 0);
						TaskSynchronizedScene(PlayerPedId(), Scena1, sLocal_436, sLocal_437, 1000f, -8f, 1, 0, 1000f, 4);
						if (DoesEntityHaveDrawable(DocciaPorta.Handle))
							PlaySynchronizedEntityAnim(DocciaPorta.Handle, Scena1, sLocal_444, sLocal_436, 2f, -8f, 1, 1148846080);

						InDoccia = true;
					}
				}
				else
				{
					HUD.ShowHelp(GetLabelText("SA_SHWR_OUT"));
					if (Game.IsControlJustPressed(0, Control.VehicleExit))
					{
						InDoccia = false;
					}
				}
			}
		}

		static void func_314()
		{
//			func_315(PlayerPedId(), 4, -1, -1);
			SetPedComponentVariation(PlayerPedId(), 11, 15, 0, 0);
			if (GetPedDrawableVariation(PlayerPedId(), 3) != 15)
			{
				SetPedComponentVariation(PlayerPedId(), 3, 15, 0, 0);
			}
			SetPedComponentVariation(PlayerPedId(), 8, 15, 0, 0);
			SetPedComponentVariation(PlayerPedId(), 9, 0, 0, 0);
			SetPedComponentVariation(PlayerPedId(), 7, 0, 0, 0);
			SetPedComponentVariation(PlayerPedId(), 5, 0, 0, 0);
			SetPedComponentVariation(PlayerPedId(), 1, 0, 0, 0);
			SetPedComponentVariation(PlayerPedId(), 10, 0, 0, 0);
			if (GetPedDrawableVariation(PlayerPedId(), 6) != 5)
			{
				SetPedComponentVariation(PlayerPedId(), 6, 5, 0, 0);
			}
			ClearAllPedProps(PlayerPedId());
			if (GetEntityModel(PlayerPedId()) == GetHashKey("mp_m_freemode_01"))
			{
				if (GetPedDrawableVariation(PlayerPedId(), 4) != 14)
				{
					SetPedComponentVariation(PlayerPedId(), 4, 14, 0, 0);
				}
			}
			else if (GetPedDrawableVariation(PlayerPedId(), 4) != 15)
			{
				SetPedComponentVariation(PlayerPedId(), 4, 15, 0, 0);
			}
			if (HasPedHeadBlendFinished(PlayerPedId()) && HasStreamedPedAssetsLoaded(PlayerPedId()))
			{
				N_0x4668d80430d6c299(PlayerPedId());
			}
		}

		private static KeyValuePair<Vector3, Vector3> CaricaDoccia(int interior)
		{
			if (interior == 145921)
				return new KeyValuePair<Vector3, Vector3>(new Vector3(-1453.325f, -556.203f, 71.881f), new Vector3(0, 0, 29.242f));
			else if (interior == IPLs.gta_online.GTAOApartmentHi2.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseHi1.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseHi2.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseHi3.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseHi4.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseHi5.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseHi6.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseHi7.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseHi8.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseLow1.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.GTAOHouseMid1.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.HLApartment1.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.HLApartment2.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.HLApartment3.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.HLApartment4.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.HLApartment5.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.gta_online.HLApartment6.InteriorId)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.dlc_executive.ExecApartment1.CurrentInteriorId && IPLs.dlc_executive.ExecApartment1.CurrentInteriorId != -1)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.dlc_executive.ExecApartment2.CurrentInteriorId && IPLs.dlc_executive.ExecApartment2.CurrentInteriorId != -1)
				return new KeyValuePair<Vector3, Vector3>();
			else if (interior == IPLs.dlc_executive.ExecApartment3.CurrentInteriorId && IPLs.dlc_executive.ExecApartment3.CurrentInteriorId != -1)
				return new KeyValuePair<Vector3, Vector3>();
			return new KeyValuePair<Vector3, Vector3>();
		}

	}
}

/*		private async static Task ControlloDocceVicino()
		{
			if (GetInteriorFromGameplayCam() != 0)
				HUD.ShowHelp("Interior = " + GetInteriorFromGameplayCam());
			if (GetInteriorFromGameplayCam() == 149761)
			{
//				if (World.GetDistance(Game.PlayerPed.Position, new Vector3()))
			}
			else if (GetInteriorFromGameplayCam() == 149761)
			{
				if (World.GetDistance(Game.PlayerPed.Position, new Vector3(347.681f, -995.201f, -99.112f)) < 2f)
				{
					VicinoDoccia = true;
					HUD.ShowHelp("Vicino la doccia");
				}
			}
			else if (GetInteriorFromGameplayCam() == 145921)
			{

			}
//			else if (GetInteriorFromGameplayCam() == )
//			else if (GetInteriorFromGameplayCam() == )
//			else if (GetInteriorFromGameplayCam() == )
//			else if (GetInteriorFromGameplayCam() == )
//			else if (GetInteriorFromGameplayCam() == )
		}
*/



