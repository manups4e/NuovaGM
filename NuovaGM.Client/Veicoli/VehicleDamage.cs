using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Veicoli
{
	internal static class VehicleDamage
	{
		private static int deformationMultiplier;
		private static float deformationExponent;
		private static float collisionDamageExponent;
		private static float damageFactorEngine;
		private static float damageFactorBody;
		private static float damageFactorPetrolTank;
		private static float engineDamageExponent;
		private static float weaponsDamageMultiplier;
		private static int degradingHealthSpeedFactor;
		private static float cascadingFailureSpeedFactor;
		private static float degradingFailureThreshold;
		private static float cascadingFailureThreshold;
		private static float engineSafeGuard;
		public static bool torqueMultiplierEnabled;
		public static bool limpMode;
		private static float limpModeMultiplier;
		public static bool preventVehicleFlip;
		private static bool sundayDriver;
		private static bool displayMessage;
		private static bool compatibilityMode;
		private static int randomTireBurstInterval;

		private static List<float> classDamageMultiplier;

		private static bool pedInSameVehicleLast = false;
		private static Vehicle vehicle = new Vehicle(0);
		private static Vehicle lastVehicle = new Vehicle(0);
		private static VehicleClass vehicleClass;
		private static float fCollisionDamageMult = 0.0f;
		private static float fDeformationDamageMult = 0.0f;
		private static float fEngineDamageMult = 0.0f;
		private static float fBrakeForce = 1.0f;

		private static float healthEngineLast = 1000.0f;
		private static float healthEngineCurrent = 1000.0f;
		private static float healthEngineNew = 1000.0f;
		private static float healthEngineDelta = 0.0f;
		private static float healthEngineDeltaScaled = 0.0f;

		private static float healthBodyLast = 1000.0f;
		private static float healthBodyCurrent = 1000.0f;
		private static float healthBodyNew = 1000.0f;
		private static float healthBodyDelta = 0.0f;
		private static float healthBodyDeltaScaled = 0.0f;

		private static float healthPetrolTankLast = 1000.0f;
		private static float healthPetrolTankCurrent = 1000.0f;
		private static float healthPetrolTankNew = 1000.0f;
		private static float healthPetrolTankDelta = 0.0f;
		private static float healthPetrolTankDeltaScaled = 0.0f;
		private static int tireBurstLuckyNumber;
		private static int tireBurstMaxNumber;

		public static void Init()
		{
			tireBurstMaxNumber = randomTireBurstInterval * 1200;
			if (randomTireBurstInterval != 0) tireBurstLuckyNumber = Funzioni.GetRandomInt(tireBurstMaxNumber);
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		public static bool isPedDrivingAVehicle()
		{
			if (Cache.Cache.MyPlayer.Character.StatiPlayer.InVeicolo)
				if (Cache.Cache.MyPlayer.Ped.CurrentVehicle.Driver == Cache.Cache.MyPlayer.Ped)
				{
					VehicleClass classe = Cache.Cache.MyPlayer.Ped.CurrentVehicle.ClassType;

					if (classe != VehicleClass.Cycles && classe != VehicleClass.Helicopters && classe != VehicleClass.Boats && classe != VehicleClass.Planes && classe != VehicleClass.Trains) return true;
				}

			return false;
		}

		public static float fscale(float inputValue, float originalMin, float originalMax, float newBegin, float newEnd, float curve)
		{
			int invFlag = 0;
			if (curve > 10.0) curve = 10.0f;
			if (curve < -10.0) curve = -10.0f;
			curve *= -0.1f;
			curve = (float)Math.Pow(10.0f, curve);
			if (inputValue < originalMin) inputValue = originalMin;
			if (inputValue > originalMax) inputValue = originalMax;
			float OriginalRange = originalMax - originalMin;
			float NewRange;

			if (newEnd > newBegin)
			{
				NewRange = newEnd - newBegin;
			}
			else
			{
				NewRange = newBegin - newEnd;
				invFlag = 1;
			}

			float zeroRefCurVal = inputValue - originalMin;
			float normalizedCurVal = zeroRefCurVal / OriginalRange;

			if (originalMin > originalMax) return 0;
			float rangedValue;
			if (invFlag == 0)
				rangedValue = (float)Math.Pow(normalizedCurVal, curve) * NewRange + newBegin;
			else
				rangedValue = newBegin - (float)Math.Pow(normalizedCurVal, curve) * NewRange;

			return rangedValue;
		}

		public static void repairVehicle(Vehicle veh)
		{
			veh.IsDriveable = true;
			veh.EngineHealth = 1000f;
			healthEngineLast = 1000.0f;
			HUD.ShowNotification("Veicolo riparato!");
		}

		public static void tireBurstLottery()
		{
			int tireBurstNumber = Funzioni.GetRandomInt(tireBurstMaxNumber);

			if (tireBurstNumber == tireBurstLuckyNumber)
			{
				if (vehicle.CanTiresBurst == false) return;
				int numWheels = GetVehicleNumberOfWheels(vehicle.Handle);
				int affectedTire;

				if (numWheels == 2)
				{
					affectedTire = (Funzioni.GetRandomInt(1, 2) - 1) * 4; // wheel 0 or 4;
				}
				else if (numWheels == 4)
				{
					affectedTire = Funzioni.GetRandomInt(1, 4) - 1;
					if (affectedTire > 1) affectedTire += 2; // 0, 1, 4, 5
				}
				else if (numWheels == 6)
				{
					affectedTire = Funzioni.GetRandomInt(1, 6) - 1;
				}
				else
				{
					affectedTire = 0;
				}

				if (!IsVehicleTyreBurst(vehicle.Handle, affectedTire, false))
				{
					SetVehicleTyreBurst(vehicle.Handle, affectedTire, false, 1000.0F);
					HUD.ShowNotification("Hai bucato!\nCerca un meccanico!");
				}

				tireBurstLuckyNumber = Funzioni.GetRandomInt(tireBurstMaxNumber);
			}
		}

		public static async Task IfNeeded()
		{
			if (torqueMultiplierEnabled || sundayDriver || limpMode)
				if (pedInSameVehicleLast)
				{
					float factor = 1.0f;
					if (torqueMultiplierEnabled && healthEngineNew < 900) factor = (healthEngineNew + 200.0f) / 1100;
					if (limpMode == true && healthEngineNew < engineSafeGuard + 5) factor = limpModeMultiplier;
					vehicle.EngineTorqueMultiplier = factor;
				}

			if (preventVehicleFlip)
			{
				float roll = GetEntityRoll(vehicle.Handle);

				if ((roll > 75.0 || roll < -75.0) && vehicle.Speed < 2 || vehicle.IsInAir && vehicle.DisplayName != "DELUXO" && vehicle.DisplayName != "OPPRESSOR2")
				{
					Game.DisableControlThisFrame(0, Control.VehicleMoveLeftRight);
					Game.DisableControlThisFrame(0, Control.VehicleMoveUpDown);
				}
			}

			await Task.FromResult(0);
		}

		public static void Spawnato()
		{
			deformationMultiplier = Client.Impostazioni.Veicoli.DanniVeicoli.deformationMultiplier;
			deformationExponent = Client.Impostazioni.Veicoli.DanniVeicoli.deformationExponent;
			collisionDamageExponent = Client.Impostazioni.Veicoli.DanniVeicoli.collisionDamageExponent;
			damageFactorEngine = Client.Impostazioni.Veicoli.DanniVeicoli.damageFactorEngine;
			damageFactorBody = Client.Impostazioni.Veicoli.DanniVeicoli.damageFactorBody;
			damageFactorPetrolTank = Client.Impostazioni.Veicoli.DanniVeicoli.damageFactorPetrolTank;
			engineDamageExponent = Client.Impostazioni.Veicoli.DanniVeicoli.engineDamageExponent;
			weaponsDamageMultiplier = Client.Impostazioni.Veicoli.DanniVeicoli.weaponsDamageMultiplier;
			degradingHealthSpeedFactor = Client.Impostazioni.Veicoli.DanniVeicoli.degradingHealthSpeedFactor;
			cascadingFailureSpeedFactor = Client.Impostazioni.Veicoli.DanniVeicoli.cascadingFailureSpeedFactor;
			degradingFailureThreshold = Client.Impostazioni.Veicoli.DanniVeicoli.degradingFailureThreshold;
			cascadingFailureThreshold = Client.Impostazioni.Veicoli.DanniVeicoli.cascadingFailureThreshold;
			engineSafeGuard = Client.Impostazioni.Veicoli.DanniVeicoli.engineSafeGuard;
			torqueMultiplierEnabled = Client.Impostazioni.Veicoli.DanniVeicoli.torqueMultiplierEnabled;
			limpMode = Client.Impostazioni.Veicoli.DanniVeicoli.limpMode;
			limpModeMultiplier = Client.Impostazioni.Veicoli.DanniVeicoli.limpModeMultiplier;
			preventVehicleFlip = Client.Impostazioni.Veicoli.DanniVeicoli.preventVehicleFlip;
			sundayDriver = Client.Impostazioni.Veicoli.DanniVeicoli.sundayDriver;
			displayMessage = Client.Impostazioni.Veicoli.DanniVeicoli.displayMessage;
			compatibilityMode = Client.Impostazioni.Veicoli.DanniVeicoli.compatibilityMode;
			randomTireBurstInterval = Client.Impostazioni.Veicoli.DanniVeicoli.randomTireBurstInterval;
			classDamageMultiplier = Client.Impostazioni.Veicoli.DanniVeicoli.classDamageMultiplier;
			tireBurstMaxNumber = randomTireBurstInterval * 1200;
			if (randomTireBurstInterval != 0) tireBurstLuckyNumber = Funzioni.GetRandomInt(tireBurstMaxNumber);
		}

		public static async Task OnTick()
		{
			Ped playerPed = Cache.Cache.MyPlayer.Ped;

			if (Cache.Cache.MyPlayer.Character.StatiPlayer.InVeicolo)
			{
				vehicle = playerPed.CurrentVehicle;

				if (vehicle == null) return;
				vehicleClass = vehicle.ClassType;
				healthEngineCurrent = vehicle.EngineHealth;
				if (healthEngineCurrent == 1000f) healthEngineLast = 1000f;
				healthEngineNew = healthEngineCurrent;
				healthEngineDelta = healthEngineLast - healthEngineCurrent;
				healthEngineDeltaScaled = healthEngineDelta * damageFactorEngine * classDamageMultiplier[(int)vehicleClass];
				healthBodyCurrent = vehicle.BodyHealth;
				if (healthBodyCurrent == 1000f) healthBodyLast = 1000f;
				healthBodyNew = healthBodyCurrent;
				healthBodyDelta = healthBodyLast - healthBodyCurrent;
				healthBodyDeltaScaled = healthBodyDelta * damageFactorBody * classDamageMultiplier[(int)vehicleClass];
				healthPetrolTankCurrent = vehicle.PetrolTankHealth;
				if (compatibilityMode && healthPetrolTankCurrent < 1) healthPetrolTankLast = healthPetrolTankCurrent;
				if (healthPetrolTankCurrent == 1000f) healthPetrolTankLast = 1000f;
				healthPetrolTankNew = healthPetrolTankCurrent;
				healthPetrolTankDelta = healthPetrolTankLast - healthPetrolTankCurrent;
				healthPetrolTankDeltaScaled = healthPetrolTankDelta * damageFactorPetrolTank * classDamageMultiplier[(int)vehicleClass];
				if (healthEngineCurrent > engineSafeGuard + 1) vehicle.IsDriveable = vehicle.IsEngineRunning;

				if (healthEngineCurrent <= engineSafeGuard + 1 && limpMode == false)
				{
					vehicle.IsDriveable = false;

					if (displayMessage)
					{
						HUD.ShowNotification("Il motore si è guastato!");
						displayMessage = false;
					}
				}

				if (vehicle != lastVehicle) pedInSameVehicleLast = false;

				if (pedInSameVehicleLast)
				{
					if (healthEngineCurrent < 1000.0f || healthBodyCurrent < 1000.0f || healthPetrolTankCurrent < 1000.0f)
					{
						float healthEngineCombinedDelta = Math.Max(healthEngineDeltaScaled, Math.Max(healthBodyDeltaScaled, healthPetrolTankDeltaScaled));
						if (healthEngineCombinedDelta > healthEngineCurrent - engineSafeGuard) healthEngineCombinedDelta *= 0.7f;
						if (healthEngineCombinedDelta > healthEngineCurrent) healthEngineCombinedDelta = healthEngineCurrent - cascadingFailureThreshold / 5;
						healthEngineNew = healthEngineLast - healthEngineCombinedDelta;
						if (healthEngineNew > cascadingFailureThreshold + 5 && healthEngineNew < degradingFailureThreshold) healthEngineNew -= 0.038f * degradingHealthSpeedFactor;
						if (healthEngineNew < cascadingFailureThreshold) healthEngineNew -= 0.1f * cascadingFailureSpeedFactor;
						if (healthEngineNew < engineSafeGuard) healthEngineNew = engineSafeGuard;
						if (compatibilityMode == false && healthPetrolTankCurrent < 750) healthPetrolTankNew = 750.0f;
						if (healthBodyNew < 0) healthBodyNew = 0.0f;
					}
				}
				else
				{
					fDeformationDamageMult = GetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fDeformationDamageMult");
					fBrakeForce = GetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fBrakeForce");
					float newFDeformationDamageMult = (float)Math.Pow(fDeformationDamageMult, deformationExponent);
					if (deformationMultiplier != -1) SetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fDeformationDamageMult", newFDeformationDamageMult * deformationMultiplier); //Multiply by our factor
					if (weaponsDamageMultiplier != -1) SetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fWeaponDamageMult", weaponsDamageMultiplier / damageFactorBody);           //Set weaponsDamageMultiplier and compensate for damageFactorBody
					fCollisionDamageMult = GetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fCollisionDamageMult");
					float newFCollisionDamageMultiplier = (float)Math.Pow(fCollisionDamageMult, collisionDamageExponent);
					SetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fCollisionDamageMult", newFCollisionDamageMultiplier);
					fEngineDamageMult = GetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fEngineDamageMult");
					float newFEngineDamageMult = (float)Math.Pow(fEngineDamageMult, engineDamageExponent);
					SetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fEngineDamageMult", newFEngineDamageMult);
					if (healthBodyCurrent < cascadingFailureThreshold) healthBodyNew = cascadingFailureThreshold;
					pedInSameVehicleLast = true;
				}

				if (healthEngineNew != healthEngineCurrent) vehicle.EngineHealth = healthEngineNew;
				if (healthBodyNew != healthBodyCurrent) vehicle.BodyHealth = healthBodyNew;
				if (healthPetrolTankNew != healthPetrolTankCurrent) vehicle.PetrolTankHealth = healthPetrolTankNew;
				healthEngineLast = healthEngineNew;
				healthBodyLast = healthBodyNew;
				healthPetrolTankLast = healthPetrolTankNew;
				lastVehicle = vehicle;
				if (randomTireBurstInterval != 0 && vehicle.Speed > 20) tireBurstLottery();
			}
			else
			{
				if (pedInSameVehicleLast)
				{
					if (playerPed.LastVehicle != null)
					{
						lastVehicle = playerPed.LastVehicle;
						if (deformationMultiplier != -1) SetVehicleHandlingFloat(lastVehicle.Handle, "CHandlingData", "fDeformationDamageMult", fDeformationDamageMult);
						SetVehicleHandlingFloat(lastVehicle.Handle, "CHandlingData", "fBrakeForce", fBrakeForce);
						if (weaponsDamageMultiplier != -1) SetVehicleHandlingFloat(lastVehicle.Handle, "CHandlingData", "fWeaponDamageMult", weaponsDamageMultiplier);
						SetVehicleHandlingFloat(lastVehicle.Handle, "CHandlingData", "fCollisionDamageMult", fCollisionDamageMult);
						SetVehicleHandlingFloat(lastVehicle.Handle, "CHandlingData", "fEngineDamageMult", fEngineDamageMult);
						displayMessage = true;
					}

					pedInSameVehicleLast = false;
				}
			}
		}
	}
}