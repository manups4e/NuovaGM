﻿using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Veicoli
{
	static class VehicleDamage
	{
		static int deformationMultiplier;
		static float deformationExponent;
		static float collisionDamageExponent;
		static float damageFactorEngine;
		static float damageFactorBody;
		static float damageFactorPetrolTank;
		static float engineDamageExponent;
		static float weaponsDamageMultiplier;
		static int degradingHealthSpeedFactor;
		static float cascadingFailureSpeedFactor;
		static float degradingFailureThreshold;
		static float cascadingFailureThreshold;
		static float engineSafeGuard;
		public static bool torqueMultiplierEnabled;
		public static bool limpMode;
		static float limpModeMultiplier;
		public static bool preventVehicleFlip;
		static bool sundayDriver;
		static bool displayMessage;
		static bool compatibilityMode;
		static int randomTireBurstInterval;

		static List<float> classDamageMultiplier;

		static bool pedInSameVehicleLast = false;
		static Vehicle vehicle = new Vehicle(0);
		static Vehicle lastVehicle = new Vehicle(0);
		static VehicleClass vehicleClass;
		static float fCollisionDamageMult = 0.0f;
		static float fDeformationDamageMult = 0.0f;
		static float fEngineDamageMult = 0.0f;
		static float fBrakeForce = 1.0f;

		static float healthEngineLast = 1000.0f;
		static float healthEngineCurrent = 1000.0f;
		static float healthEngineNew = 1000.0f;
		static float healthEngineDelta = 0.0f;
		static float healthEngineDeltaScaled = 0.0f;

		static float healthBodyLast = 1000.0f;
		static float healthBodyCurrent = 1000.0f;
		static float healthBodyNew = 1000.0f;
		static float healthBodyDelta = 0.0f;
		static float healthBodyDeltaScaled = 0.0f;

		static float healthPetrolTankLast = 1000.0f;
		static float healthPetrolTankCurrent = 1000.0f;
		static float healthPetrolTankNew = 1000.0f;
		static float healthPetrolTankDelta = 0.0f;
		static float healthPetrolTankDeltaScaled = 0.0f;
		static int tireBurstLuckyNumber;
		static int tireBurstMaxNumber;
		public static void Init()
		{
			tireBurstMaxNumber = randomTireBurstInterval * 1200;
			if (randomTireBurstInterval != 0)
			{
				tireBurstLuckyNumber = Funzioni.GetRandomInt(tireBurstMaxNumber);
			}

			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		public static bool isPedDrivingAVehicle()
		{
			if (Game.PlayerPed.IsInVehicle())
			{
				if (Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed)
				{
					VehicleClass classe = Game.PlayerPed.CurrentVehicle.ClassType;
					if (classe != VehicleClass.Cycles && classe != VehicleClass.Helicopters && classe != VehicleClass.Boats && classe != VehicleClass.Planes && classe != VehicleClass.Trains)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static float fscale(float inputValue, float originalMin, float originalMax, float newBegin, float newEnd, float curve)
		{
			float OriginalRange = 0.0f;
			float NewRange = 0.0f;
			float zeroRefCurVal = 0.0f;
			float normalizedCurVal = 0.0f;
			float rangedValue = 0.0f;
			int invFlag = 0;
			if (curve > 10.0)
			{
				curve = 10.0f;
			}

			if (curve < -10.0)
			{
				curve = -10.0f;
			}

			curve = (curve * -0.1f);
			curve = (float)Math.Pow(10.0f, curve);
			if (inputValue < originalMin)
			{
				inputValue = originalMin;
			}

			if (inputValue > originalMax)
			{
				inputValue = originalMax;
			}

			OriginalRange = originalMax - originalMin;
			if (newEnd > newBegin)
			{
				NewRange = newEnd - newBegin;
			}
			else
			{
				NewRange = newBegin - newEnd;
				invFlag = 1;
			}
			zeroRefCurVal = inputValue - originalMin;
			normalizedCurVal = zeroRefCurVal / OriginalRange;
			if (originalMin > originalMax)
			{
				return 0;
			}

			if (invFlag == 0)
			{
				rangedValue = ((float)Math.Pow(normalizedCurVal, curve) * NewRange) + newBegin;
			}
			else
			{
				rangedValue = newBegin - ((float)Math.Pow(normalizedCurVal, curve) * NewRange);
			}

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
				if (vehicle.CanTiresBurst == false)
				{
					return;
				}

				int numWheels = GetVehicleNumberOfWheels(vehicle.Handle);
				int affectedTire;
				if (numWheels == 2)
				{
					affectedTire = (Funzioni.GetRandomInt(1, 2) - 1) * 4; // wheel 0 or 4;
				}
				else if (numWheels == 4)
				{
					affectedTire = (Funzioni.GetRandomInt(1, 4) - 1);
					if (affectedTire > 1)
					{
						affectedTire += 2; // 0, 1, 4, 5
					}
				}
				else if (numWheels == 6)
				{
					affectedTire = (Funzioni.GetRandomInt(1, 6) - 1);
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
			{
				if (pedInSameVehicleLast)
				{
					float factor = 1.0f;
					if (torqueMultiplierEnabled && healthEngineNew < 900)
					{
						factor = (healthEngineNew + 200.0f) / 1100;
					}

					if (limpMode == true && healthEngineNew < engineSafeGuard + 5)
					{
						factor = limpModeMultiplier;
					}

					vehicle.EngineTorqueMultiplier = factor;
				}
			}
			if (preventVehicleFlip)
			{
				float roll = GetEntityRoll(vehicle.Handle);
				if ((roll > 75.0 || roll < -75.0) && vehicle.Speed < 2 || (vehicle.IsInAir && vehicle.DisplayName != "DELUXO" && vehicle.DisplayName != "OPPRESSOR2"))
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
			if (randomTireBurstInterval != 0)
				tireBurstLuckyNumber = Funzioni.GetRandomInt(tireBurstMaxNumber);
		}

		public static async Task OnTick()
		{
			if (Game.PlayerPed.IsInVehicle())
			{
				vehicle = Game.PlayerPed.CurrentVehicle;
				vehicleClass = vehicle.ClassType;
				healthEngineCurrent = vehicle.EngineHealth;
				if (healthEngineCurrent == 1000f)
				{
					healthEngineLast = 1000f;
				}

				healthEngineNew = healthEngineCurrent;
				healthEngineDelta = healthEngineLast - healthEngineCurrent;
				healthEngineDeltaScaled = healthEngineDelta * damageFactorEngine * classDamageMultiplier[(int)vehicleClass];
				healthBodyCurrent = vehicle.BodyHealth;
				if (healthBodyCurrent == 1000f)
				{
					healthBodyLast = 1000f;
				}

				healthBodyNew = healthBodyCurrent;
				healthBodyDelta = healthBodyLast - healthBodyCurrent;
				healthBodyDeltaScaled = healthBodyDelta * damageFactorBody * classDamageMultiplier[(int)vehicleClass];
				healthPetrolTankCurrent = vehicle.PetrolTankHealth;
				if (compatibilityMode && healthPetrolTankCurrent < 1)
				{
					healthPetrolTankLast = healthPetrolTankCurrent;
				}

				if (healthPetrolTankCurrent == 1000f)
				{
					healthPetrolTankLast = 1000f;
				}

				healthPetrolTankNew = healthPetrolTankCurrent;
				healthPetrolTankDelta = healthPetrolTankLast - healthPetrolTankCurrent;
				healthPetrolTankDeltaScaled = healthPetrolTankDelta * damageFactorPetrolTank * classDamageMultiplier[(int)vehicleClass];
				if (healthEngineCurrent > engineSafeGuard + 1)
				{
					if (vehicle.IsEngineRunning)
					{
						vehicle.IsDriveable = true;
					}
					else
					{
						vehicle.IsDriveable = false;
					}
				}

				if (healthEngineCurrent <= engineSafeGuard + 1 && limpMode == false)
				{
					vehicle.IsDriveable = false;
					if (displayMessage)
					{
						HUD.ShowNotification("Il motore si è guastato!");
						displayMessage = false;
					}
				}
				if (vehicle != lastVehicle)
				{
					pedInSameVehicleLast = false;
				}

				if (pedInSameVehicleLast == true)
				{
					if (healthEngineCurrent != 1000.0 || healthBodyCurrent != 1000.0 || healthPetrolTankCurrent != 1000.0)
					{
						float healthEngineCombinedDelta = Math.Max(healthEngineDeltaScaled, Math.Max(healthBodyDeltaScaled, healthPetrolTankDeltaScaled));
						if (healthEngineCombinedDelta > (healthEngineCurrent - engineSafeGuard))
						{
							healthEngineCombinedDelta *= 0.7f;
						}

						if (healthEngineCombinedDelta > healthEngineCurrent)
						{
							healthEngineCombinedDelta = healthEngineCurrent - (cascadingFailureThreshold / 5);
						}

						healthEngineNew = healthEngineLast - healthEngineCombinedDelta;
						if (healthEngineNew > (cascadingFailureThreshold + 5) && healthEngineNew < degradingFailureThreshold)
						{
							healthEngineNew -= (0.038f * degradingHealthSpeedFactor);
						}

						if (healthEngineNew < cascadingFailureThreshold)
						{
							healthEngineNew -= (0.1f * cascadingFailureSpeedFactor);
						}

						if (healthEngineNew < engineSafeGuard)
						{
							healthEngineNew = engineSafeGuard;
						}

						if (compatibilityMode == false && healthPetrolTankCurrent < 750)
						{
							healthPetrolTankNew = 750.0f;
						}

						if (healthBodyNew < 0)
						{
							healthBodyNew = 0.0f;
						}
					}
				}
				else
				{
					fDeformationDamageMult = GetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fDeformationDamageMult");
					fBrakeForce = GetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fBrakeForce");
					float newFDeformationDamageMult = (float)Math.Pow(fDeformationDamageMult, deformationExponent);
					if (deformationMultiplier != -1)
					{
						SetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fDeformationDamageMult", newFDeformationDamageMult * deformationMultiplier); //Multiply by our factor
					}

					if (weaponsDamageMultiplier != -1)
					{
						SetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fWeaponDamageMult", weaponsDamageMultiplier / damageFactorBody); //Set weaponsDamageMultiplier and compensate for damageFactorBody
					}

					fCollisionDamageMult = GetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fCollisionDamageMult");
					float newFCollisionDamageMultiplier = (float)Math.Pow(fCollisionDamageMult, collisionDamageExponent);
					SetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fCollisionDamageMult", newFCollisionDamageMultiplier);
					fEngineDamageMult = GetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fEngineDamageMult");
					float newFEngineDamageMult = (float)Math.Pow(fEngineDamageMult, engineDamageExponent);
					SetVehicleHandlingFloat(vehicle.Handle, "CHandlingData", "fEngineDamageMult", newFEngineDamageMult);
					if (healthBodyCurrent < cascadingFailureThreshold)
					{
						healthBodyNew = cascadingFailureThreshold;
					}

					pedInSameVehicleLast = true;
				}
				if (healthEngineNew != healthEngineCurrent)
				{
					vehicle.EngineHealth = healthEngineNew;
				}

				if (healthBodyNew != healthBodyCurrent)
				{
					vehicle.BodyHealth = healthBodyNew;
				}

				if (healthPetrolTankNew != healthPetrolTankCurrent)
				{
					vehicle.PetrolTankHealth = healthPetrolTankNew;
				}

				healthEngineLast = healthEngineNew;
				healthBodyLast = healthBodyNew;
				healthPetrolTankLast = healthPetrolTankNew;
				lastVehicle = vehicle;
				if (randomTireBurstInterval != 0 && vehicle.Speed > 10)
				{
					tireBurstLottery();
				}
			}
			else
			{
				if (pedInSameVehicleLast == true)
				{
					if (Game.PlayerPed.LastVehicle != null)
					{
						lastVehicle = Game.PlayerPed.LastVehicle;
						if (deformationMultiplier != -1)
						{
							SetVehicleHandlingFloat(lastVehicle.Handle, "CHandlingData", "fDeformationDamageMult", fDeformationDamageMult);
						}

						SetVehicleHandlingFloat(lastVehicle.Handle, "CHandlingData", "fBrakeForce", fBrakeForce);
						if (weaponsDamageMultiplier != -1)
						{
							SetVehicleHandlingFloat(lastVehicle.Handle, "CHandlingData", "fWeaponDamageMult", weaponsDamageMultiplier);
						}

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
