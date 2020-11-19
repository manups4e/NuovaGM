﻿using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Logger;

namespace NuovaGM.Client.Veicoli
{
	static class EffettiRuote
	{
		private static float heat_front = 0.0f;
		private static bool glow_front = false;
		private static float heat_rear = 0.0f;
		private static bool glow_rear = false;

		private static List<Vehicle> rearvehicles = new List<Vehicle>();
		private static List<Vehicle> frontvehicles = new List<Vehicle>();

		public static async void Init()
		{
			Client.Instance.AddEventHandler("cBrakes:add_rear", new Action<int>(AddRear));
			Client.Instance.AddEventHandler("cBrakes:add_front", new Action<int>(AddFront));
			Client.Instance.AddEventHandler("cBrakes:rem_rear", new Action<int>(RemRear));
			Client.Instance.AddEventHandler("cBrakes:rem_front", new Action<int>(RemFront));
		}

		private static async void AddRear(int NetVeh)
		{
			Vehicle veh = new Vehicle(NetToVeh(NetVeh));
			rearvehicles.Add(veh);
		}

		private static async void AddFront(int NetVeh)
		{
			Vehicle veh = new Vehicle(NetToVeh(NetVeh));
			frontvehicles.Add(veh);
		}

		private static async void RemRear(int NetVeh)
		{
			foreach (var veh in rearvehicles.ToList()) 
				if (veh.NetworkId == NetVeh)
					rearvehicles.Remove(veh);
		}

		private static async void RemFront(int NetVeh)
		{
			foreach (var veh in frontvehicles.ToList())
				if (veh.NetworkId == NetVeh)
					frontvehicles.Remove(veh);
		}

		public static async Task ControlloRuote()
		{
			try
			{
				Ped playerPed = Game.PlayerPed;
				if (playerPed.IsInVehicle())
				{
					if (playerPed.CurrentVehicle.Driver == playerPed)
					{
						if (playerPed.CurrentVehicle.IsHandbrakeForcedOn && playerPed.CurrentVehicle.Speed > 2f)
						{
							if (heat_rear < 300f)
								heat_rear += 2f;
						}
						if (playerPed.CurrentVehicle.IsInBurnout)
						{
							if (heat_rear < 300f)
								heat_rear += 2f;
						}
						if (playerPed.CurrentVehicle.Speed > 2f && playerPed.CurrentVehicle.CurrentGear != 0)
						{
							if (Game.IsControlPressed(27, Control.VehicleBrake))
							{
								if (heat_rear < 300f)
									heat_rear += 2f;
								if (heat_front < 300f)
									heat_front += 2f;
							}
						}
					}
					else
					{
						glow_rear = false;
						glow_front = false;
						heat_rear = 0f;
						heat_front = 0f;
					}
				}
				if (!glow_rear)
				{
					if (heat_rear > 30f)
					{
						glow_rear = true;
						BaseScript.TriggerServerEvent("brakes:add_rear", VehToNet(playerPed.CurrentVehicle.Handle));
					}
				}
				else
				{
					if (heat_rear < 30f)
					{
						glow_rear = false;
						BaseScript.TriggerServerEvent("brakes:rem_rear", VehToNet(playerPed.CurrentVehicle.Handle));
					}
				}
				if (!glow_front)
				{
					if (heat_front > 30f)
					{
						glow_front = true;
						BaseScript.TriggerServerEvent("brakes:add_front", VehToNet(playerPed.CurrentVehicle.Handle));
					}
				}
				else
				{
					if (heat_front < 30f)
					{
						glow_front = false;
						BaseScript.TriggerServerEvent("brakes:rem_front", VehToNet(playerPed.CurrentVehicle.Handle));
					}
				}
				if (heat_rear > 1)
					heat_rear -= 1;
				if (heat_front > 1)
					heat_front -= 1;
			}
			catch(Exception e)
			{
				Log.Printa(LogType.Warning, e.ToString());
			}
		}

		public static async Task WheelGlow()
		{
			foreach (var veh in rearvehicles.ToList())
			{
				if (veh.IsSeatFree(VehicleSeat.Driver))
					rearvehicles.Remove(veh);
				else
				{
					UseParticleFxAsset("core");
					var disc_LR = StartParticleFxLoopedOnEntityBone("veh_exhaust_afterburner", veh.Handle, 0 - 0.03f, 0, 0, 0, 0, 90.0f, GetEntityBoneIndexByName(veh.Handle, "wheel_lr"), 0.45f, false, false, false);
					StopParticleFxLooped(disc_LR, true);
					UseParticleFxAsset("core");
					var disc_RR = StartParticleFxLoopedOnEntityBone("veh_exhaust_afterburner", veh.Handle, 0 - 0.03f, 0, 0, 0, 0, 90.0f, GetEntityBoneIndexByName(veh.Handle, "wheel_rr"), 0.45f, false, false, false);
					StopParticleFxLooped(disc_RR, true);
				}
			}
			foreach (var veh in frontvehicles.ToList())
			{
				if (veh.IsSeatFree(VehicleSeat.Driver))
					frontvehicles.Remove(veh);
				else
				{
					UseParticleFxAsset("core");
					var disc_LF = StartParticleFxLoopedOnEntityBone("veh_exhaust_afterburner", veh.Handle, 0 - 0.03f, 0, 0, 0, 0, 90.0f, GetEntityBoneIndexByName(veh.Handle, "wheel_lf"), 0.45f, false, false, false);
					StopParticleFxLooped(disc_LF, true);
					UseParticleFxAsset("core");
					var disc_RF = StartParticleFxLoopedOnEntityBone("veh_exhaust_afterburner", veh.Handle, 0 - 0.03f, 0, 0, 0, 0, 90.0f, GetEntityBoneIndexByName(veh.Handle, "wheel_rf"), 0.45f, false, false, false);
					StopParticleFxLooped(disc_RF, true);
				}
			}
		}
	}
}
