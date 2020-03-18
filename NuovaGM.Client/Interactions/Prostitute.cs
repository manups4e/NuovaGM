﻿using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;

namespace NuovaGM.Client.Interactions
{
	public static class Prostitute
	{
		private static Ped Prostituta;
		private static float ProstDistance = 10f;
		private static List<string> Scenarios = new List<string>
		{
			
		};
		public static void Init()
		{
			CaricaTutto();
		}

		private static void CaricaTutto()
		{
			RequestAnimDict("mini@prostitutes@sexlow_veh");
			RequestAnimDict("mini@prostitutes@sexnorm_veh");
			RequestAnimDict("mini@prostitutes@sexlow_veh_first_person");
			RequestAnimDict("mini@prostitutes@sexnorm_veh_first_person");
		}

		public static async Task ControlloProstitute()
		{
			Prostituta = World.GetAllPeds().Select(o => new Ped(o.Handle)).Where(o => IsPedUsingScenario(o.Handle, "WORLD_HUMAN_PROSTITUTE_LOW_CLASS") || IsPedUsingScenario(o.Handle, "WORLD_HUMAN_PROSTITUTE_HIGH_CLASS")).FirstOrDefault(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * ProstDistance, 2));
			Debug.WriteLine("Id Prostituta = " + Prostituta.Handle);
			await BaseScript.Delay(200);
		}


		public static async Task LoopProstitute()
		{
			if (Prostituta != null)
			{
				if (Prostituta.IsPlayer) return;
				if (Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) != Prostituta)
				{
					HUD.ShowHelp(GetLabelText("PROS_ACCEPT"));
					if (Game.IsControlJustPressed(0, Control.VehicleHorn))
					{
						if (Eventi.Player.Money > 5f)
						{
							if (Game.PlayerPed.CurrentVehicle.ClassType != VehicleClass.Boats
							&& Game.PlayerPed.CurrentVehicle.ClassType != VehicleClass.Cycles
							&& Game.PlayerPed.CurrentVehicle.ClassType != VehicleClass.Motorcycles
							&& Game.PlayerPed.CurrentVehicle.ClassType != VehicleClass.Helicopters
							&& Game.PlayerPed.CurrentVehicle.ClassType != VehicleClass.Military
							&& Game.PlayerPed.CurrentVehicle.ClassType != VehicleClass.Motorcycles
							&& Game.PlayerPed.CurrentVehicle.ClassType != VehicleClass.Planes
							&& Game.PlayerPed.CurrentVehicle.ClassType != VehicleClass.Trains)
							{
								if (Game.PlayerPed.CurrentVehicle.Speed < 1)
								{
									if (Game.PlayerPed.CurrentVehicle.IsSeatFree(VehicleSeat.Passenger))
									{
										Prostituta.Task.EnterVehicle(Game.PlayerPed.CurrentVehicle, VehicleSeat.Passenger);
										while (!Prostituta.IsInVehicle(Game.PlayerPed.CurrentVehicle)) await BaseScript.Delay(0);
										SetPedInVehicleContext(Prostituta.Handle, Funzioni.HashUint("MINI_PROSTITUTE_LOW_PASSENGER"));
										Prostituta.BlockPermanentEvents = true;
									}
									else
										HUD.ShowNotification("Non può salire dietro!", NotificationColor.Red, true);
									if (Prostituta.IsInVehicle(Game.PlayerPed.CurrentVehicle))
									{
										if (!IsInputDisabled(2)) 
											HUD.ShowHelp("Tesoro cosa desideri?~n~~INPUT_FRONTEND_RB~ + ~INPUT_CONTEXT~ per la manina.~n~~INPUT_FRONTEND_RB~ + ~INPUT_DETONATE~ per la boccuccia.~n~~INPUT_FRONTEND_RB~ + ~INPUT_FRONTEND_UP~ per darci dentro.");
										else
											HUD.ShowHelp("Tesoro cosa desideri?~n~~INPUT_SELECT_WEAPON_UNARMED~ per la manina.~n~~INPUT_SELECT_WEAPON_MELEE~ per la boccuccia.~n~~INPUT_SELECT_WEAPON_SHOTGUN~ per darci dentro.");
										if (Game.IsControlPressed(0, Control.FrontendRb) && !IsInputDisabled(2))
										{
											Game.DisableControlThisFrame(0, Control.Context);
											Game.DisableControlThisFrame(0, Control.Detonate);
											Game.DisableControlThisFrame(0, Control.FrontendUp);
											if (Game.IsDisabledControlJustPressed(0, Control.Context))
											{
												ProstitutaMano(Prostituta);
											}
											else if (Game.IsDisabledControlJustPressed(0, Control.Detonate))
											{
												ProstitutaBocca(Prostituta);
											}
											else if (Game.IsDisabledControlJustPressed(0, Control.FrontendUp))
											{
												ProstitutaSesso(Prostituta);
											}
										}
										else if (IsInputDisabled(2))
										{
											Game.DisableControlThisFrame(0, Control.SelectWeaponUnarmed);
											Game.DisableControlThisFrame(0, Control.SelectWeaponMelee);
											Game.DisableControlThisFrame(0, Control.SelectWeaponShotgun);
											if (Game.IsDisabledControlJustPressed(0, Control.SelectWeaponUnarmed))
											{
												ProstitutaMano(Prostituta);
											}
											else if (Game.IsDisabledControlJustPressed(0, Control.SelectWeaponMelee))
											{
												ProstitutaBocca(Prostituta);
											}
											else if (Game.IsDisabledControlJustPressed(0, Control.SelectWeaponShotgun))
											{
												ProstitutaSesso(Prostituta);
											}
										}
									}
								}
								else
									HUD.ShowNotification("Non puoi con il veicolo in movimento!", NotificationColor.Red, true);
							}
							else
								HUD.ShowNotification("Non può salire su questo veicolo!!", NotificationColor.Red, true);
						}
						else
							HUD.ShowNotification(GetLabelText("PROS_NO_MONEY"), NotificationColor.Red, true);
					}
				}
			}
		}

		static string func_39(int iParam0)
		{
			string sVar0;

			switch (iParam0)
			{
				case 0:
					sVar0 = "into_proposition_male";
					break;

				case 1:
					sVar0 = "into_proposition_prostitute";
					break;

				case 2:
					sVar0 = "proposition_loop_male";
					break;

				case 3:
					sVar0 = "proposition_loop_prostitute";
					break;

				case 4:
					sVar0 = "proposition_to_exit_male";
					break;

				case 5:
					sVar0 = "prop_to_sit_alt_prostitute";
					break;

				case 6:
					sVar0 = "prop_to_sit_male";
					break;

				case 7:
					sVar0 = "prop_to_sit_prostitute";
					break;

				case 8:
					sVar0 = "proposition_to_sex_p1_prostitute";
					break;

				case 9:
					sVar0 = "proposition_to_sex_p2_prostitute";
					break;

				case 10:
					sVar0 = "sex_loop_prostitute";
					break;

				case 11:
					sVar0 = "sex_to_proposition_p1_prostitute";
					break;

				case 12:
					sVar0 = "sex_to_proposition_p2_prostitute";
					break;

				case 13:
					sVar0 = "proposition_to_sex_p1_male";
					break;

				case 14:
					sVar0 = "proposition_to_sex_p2_male";
					break;

				case 15:
					sVar0 = "sex_loop_male";
					break;

				case 16:
					sVar0 = "sex_to_proposition_p1_male";
					break;

				case 17:
					sVar0 = "sex_to_proposition_p2_male";
					break;

				case 18:
					sVar0 = "proposition_to_BJ_p1_prostitute";
					break;

				case 19:
					sVar0 = "proposition_to_BJ_p2_prostitute";
					break;

				case 20:
					sVar0 = "BJ_loop_prostitute";
					break;

				case 21:
					sVar0 = "BJ_to_proposition_p1_prostitute";
					break;

				case 22:
					sVar0 = "BJ_to_proposition_p2_prostitute";
					break;

				case 23:
					sVar0 = "proposition_to_BJ_p1_male";
					break;

				case 24:
					sVar0 = "proposition_to_BJ_p2_male";
					break;

				case 25:
					sVar0 = "BJ_loop_male";
					break;

				case 26:
					sVar0 = "BJ_to_proposition_p1_male";
					break;

				case 27:
					sVar0 = "BJ_to_proposition_p2_male";
					break;

				default:
					sVar0 = "";
					break;
			}
			return sVar0;
		}

		static string func_40(int iParam0)
		{
			string sVar0;

			switch (iParam0)
			{
				case 0:
					sVar0 = "low_car_sit_to_prop_player";
					break;

				case 1:
					sVar0 = "low_car_sit_to_prop_female";
					break;

				case 2:
					sVar0 = "low_car_prop_loop_player";
					break;

				case 3:
					sVar0 = "low_car_prop_loop_female";
					break;

				case 4:
					sVar0 = "low_car_prop_to_leave_player";
					break;

				case 5:
					sVar0 = "low_car_prop_to_sit_alt_female";
					break;

				case 6:
					sVar0 = "low_car_prop_to_sit_player";
					break;

				case 7:
					sVar0 = "low_car_prop_to_sit_female";
					break;

				case 8:
					sVar0 = "low_car_prop_to_sex_p1_female";
					break;

				case 9:
					sVar0 = "low_car_prop_to_sex_p2_female";
					break;

				case 10:
					sVar0 = "low_car_sex_loop_female";
					break;

				case 11:
					sVar0 = "low_car_sex_to_prop_p1_female";
					break;

				case 12:
					sVar0 = "low_car_sex_to_prop_p2_female";
					break;

				case 13:
					sVar0 = "low_car_prop_to_sex_p1_player";
					break;

				case 14:
					sVar0 = "low_car_prop_to_sex_p2_player";
					break;

				case 15:
					sVar0 = "low_car_sex_loop_player";
					break;

				case 16:
					sVar0 = "low_car_sex_to_prop_p1_player";
					break;

				case 17:
					sVar0 = "low_car_sex_to_prop_p2_player";
					break;

				case 18:
					sVar0 = "low_car_prop_to_bj_p1_female";
					break;

				case 19:
					sVar0 = "low_car_prop_to_bj_p2_female";
					break;

				case 20:
					sVar0 = "low_car_bj_loop_female";
					break;

				case 21:
					sVar0 = "low_car_bj_to_prop_p1_female";
					break;

				case 22:
					sVar0 = "low_car_bj_to_prop_p2_female";
					break;

				case 23:
					sVar0 = "low_car_prop_to_bj_p1_player";
					break;

				case 24:
					sVar0 = "low_car_prop_to_bj_p2_player";
					break;

				case 25:
					sVar0 = "low_car_bj_loop_player";
					break;

				case 26:
					sVar0 = "low_car_bj_to_prop_p1_player";
					break;

				case 27:
					sVar0 = "low_car_bj_to_prop_p2_player";
					break;

				default:
					sVar0 = "";
					break;
			}
			return sVar0;
		}

		static bool func_41(int iParam0)
		{
			if (IsVehicleDriveable(iParam0, false))
			{
				switch (GetVehicleLayoutHash(iParam0))
				{
					case -2066252141:
					case -38413156:
					case -782720499:
					case 542797648:
					case 68566729:
					case -1887744178:
					case -463340997:
					case 2033852426:
					case -1820894825:
					case 1697345049:
					case -635697407:
					case -1453479140:
					case 1837596901:
					case 2013836096:
					case 2071837743:
					case 2130662788:
					case -1546132012:
					case 1192783831:
					case -317944664:
					case 570040040:
					case 1212243433:
					case -193022774:
					case 510359473:
					case -988377294:
					case 1240573865:
					case -627376728:
					case 986556497:
						return true;
				}
			}
			return false;
		}

		static bool func_42(bool bParam0, bool bParam1)
		{
			bool bVar0;

			bVar0 = false;
			if (bParam0)
			{
				bVar0 = N_0xee778f8c7e1142e2(0) == 4;
			}
			if (bParam1)
			{
				if (!bVar0)
				{
					bVar0 = N_0xee778f8c7e1142e2(1) == 4;
				}
			}
			return bVar0;
		}



		static string func_44(int iParam0, bool bParam1, int iParam2)
		{
			int iVar0;

			if (IsEntityDead(iParam0))
			{
				iVar0 = GetVehiclePedIsIn(iParam0, true);
				if (IsVehicleDriveable(iVar0, false))
				{
					if (func_41(iVar0))
					{
						if ((!func_42(false, true) || iParam2 == 1) && !bParam1)
							return func_48();
						else
							return func_47();
					}
				}
			}
			if ((!func_42(false, true) || iParam2 == 1) && !bParam1)
				return func_46();
			return func_45();
		}


		static string func_60(int iParam0)
		{
			switch (iParam0)
			{
				case 0:
					return "idle_intro";
				case 1:
					return "idle_a";
				case 2:
					return "idle_b";
				case 3:
					return "idle_c";
				case 4:
					return "idle_wait";
				case 5:
					return "idle_reject";
				case 8:
					return "idle_reject_loop_a";
				case 9:
					return "idle_reject_loop_b";
				case 10:
					return "idle_reject_loop_c";
				case 11:
					return "idle_outro";
				case 6:
					return "reject_2_idle";
				case 7:
					return "reject_outro";
			}
			return "";
		}

		static string func_61(int iParam0)
		{
			string sVar0;
			switch (iParam0)
			{
				case 0:
					sVar0 = "mini@hookers_spcrackhead";
					break;
				case 1:
					sVar0 = "mini@hookers_spcokehead";
					break;
				case 3:
				case 4:
					sVar0 = "mini@hookers_spfrench";
					break;
				case 2:
				default:
					sVar0 = "mini@hookers_spvanilla";
					break;
			}
			return sVar0;
		}


		static string func_45()
		{
			return "mini@prostitutes@sexnorm_veh_first_person";
		}

		static string func_46()
		{
			return "mini@prostitutes@sexnorm_veh";
		}

		static string func_47()
		{
			return "mini@prostitutes@sexlow_veh_first_person";
		}

		static string func_48()
		{
			return "mini@prostitutes@sexlow_veh";
		}

		static async void ProstitutaMano(Ped prostituta)
		{

		}
		static async void ProstitutaBocca(Ped prostituta)
		{

		}
		static async void ProstitutaSesso(Ped prostituta)
		{

		}

	}
}
