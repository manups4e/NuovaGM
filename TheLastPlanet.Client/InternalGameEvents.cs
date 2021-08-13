﻿using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client
{
	public class InternalGameEvents
	{
		public const string damageEventName = "DamageEvents";

		public static void Init()
		{
			Client.Instance.AddEventHandler("gameEventTriggered", new Action<string, List<object>>(GameEventTriggered));
		}

		/// <summary>
		/// Event gets triggered whenever a vehicle is destroyed.
		/// </summary>
		/// <param name="vehicle">The vehicle that got destroyed.</param>
		/// <param name="attacker">The attacker handle of what destroyed the vehicle.</param>
		/// <param name="weaponHash">The weapon hash that was used to destroy the vehicle.</param>
		/// <param name="isMeleeDamage">True if the damage dealt was using any melee weapon (including unarmed).</param>
		/// <param name="vehicleDamageTypeFlag">Vehicle damage type flag, 93 is vehicle tires damaged, others unknown.</param>
		private static void VehicleDestroyed(int vehicle, int attacker, uint weaponHash, bool isMeleeDamage, int vehicleDamageTypeFlag) 
		{ 
			BaseScript.TriggerEvent(damageEventName + ":VehicleDestroyed", vehicle, attacker, weaponHash, isMeleeDamage, vehicleDamageTypeFlag);
			Client.Logger.Debug($"[{damageEventName}:VehicleDestroyed] vehicle: {vehicle}, attacker: {attacker}, weaponHash: {weaponHash}, isMeleeDamage: {isMeleeDamage}, vehicleDamageTypeFlag: {vehicleDamageTypeFlag}");
		}

		/// <summary>
		/// Event gets triggered whenever a ped was killed by a vehicle without a driver.
		/// </summary>
		/// <param name="ped">Ped that got killed.</param>
		/// <param name="vehicle">Vehicle that was used to kill the ped.</param>
		private static void PedKilledByVehicle(int ped, int vehicle) 
		{ 
			BaseScript.TriggerEvent(damageEventName + ":PedKilledByVehicle", ped, vehicle);
			Client.Logger.Debug($"[{damageEventName}:PedKilledByVehicle] ped: {ped}, vehicle: {vehicle}");
		}

		/// <summary>
		/// Event gets triggered whenever a ped is killed by a player.
		/// </summary>
		/// <param name="ped">The ped that got killed.</param>
		/// <param name="player">The player that killed the ped.</param>
		/// <param name="weaponHash">The weapon hash used to kill the ped.</param>
		/// <param name="isMeleeDamage">True if the ped was killed with a melee weapon (including unarmed).</param>
		private static void PedKilledByPlayer(int ped, int player, uint weaponHash, bool isMeleeDamage) 
		{ 
			BaseScript.TriggerEvent(damageEventName + ":PedKilledByPlayer", ped, player, weaponHash, isMeleeDamage);
			Client.Logger.Debug($"[{damageEventName}:PedKilledByPlayer] ped: {ped}, player: {player}, weaponHash: {weaponHash}, isMeleeDamage: {isMeleeDamage}");
		}

		/// <summary>
		/// Event gets triggered whenever a ped is killed by another (non-player) ped.
		/// </summary>
		/// <param name="ped">Ped that got killed.</param>
		/// <param name="attackerPed">Ped that killed the victim ped.</param>
		/// <param name="weaponHash">Weapon hash used to kill the ped.</param>
		/// <param name="isMeleeDamage">True if the ped was killed using a melee weapon (including unarmed).</param>
		private static void PedKilledByPed(int ped, int attackerPed, uint weaponHash, bool isMeleeDamage) 
		{ 
			BaseScript.TriggerEvent(damageEventName + ":PedKilledByPed", ped, attackerPed, weaponHash, isMeleeDamage);
			Client.Logger.Debug($"[{damageEventName}:PedKilledByPed] ped: {ped}, attackerPed: {attackerPed}, weaponHash: {weaponHash}, isMeleeDamage: {isMeleeDamage}");
		}

		/// <summary>
		/// Event gets triggered whenever a ped died, but only if the other (more detailed) events weren't triggered.
		/// </summary>
		/// <param name="ped">The ped that died.</param>
		/// <param name="attacker">The attacker (can be the same as the ped that died).</param>
		/// <param name="weaponHash">Weapon hash used to kill the ped.</param>
		/// <param name="isMeleeDamage">True whenever the ped was killed using a melee weapon (including unarmed).</param>
		private static void PedDied(int ped, int attacker, uint weaponHash, bool isMeleeDamage) 
		{ 
			BaseScript.TriggerEvent(damageEventName + ":PedDied", ped, attacker, weaponHash, isMeleeDamage);
			Client.Logger.Debug($"[{damageEventName}:PedDied] ped: {ped}, attacker: {attacker}, weaponHash: {weaponHash}, isMeleeDamage: {isMeleeDamage}");
		}

		/// <summary>
		/// Gets triggered whenever an entity died, that's not a vehicle, or a ped.
		/// </summary>
		/// <param name="entity">Entity that was killed/destroyed.</param>
		/// <param name="attacker">The attacker that destroyed/killed the entity.</param>
		/// <param name="weaponHash">The weapon hash used to kill/destroy the entity.</param>
		/// <param name="isMeleeDamage">True whenever the entity was killed/destroyed with a melee weapon.</param>
		private static void EntityKilled(int entity, int attacker, uint weaponHash, bool isMeleeDamage) 
		{ 
			BaseScript.TriggerEvent(damageEventName + ":EntityKilled", entity, attacker, weaponHash, isMeleeDamage);
			Client.Logger.Debug($"[{damageEventName}:EntityKilled] entity: {entity}, attacker: {attacker}, weaponHash: {weaponHash}, isMeleeDamage: {isMeleeDamage}");
		}

		/// <summary>
		/// Event gets triggered whenever a vehicle is damaged, but not destroyed.
		/// </summary>
		/// <param name="vehicle">Vehicle that got damaged.</param>
		/// <param name="attacker">Attacker that damaged the vehicle.</param>
		/// <param name="weaponHash">Weapon hash used to damage the vehicle.</param>
		/// <param name="isMeleeDamage">True whenever the vehicle was damaged using a melee weapon (including unarmed).</param>
		/// <param name="vehicleDamageTypeFlag">Vehicle damage type flag, 93 is vehicle tire damage, others are unknown.</param>
		private static void VehicleDamaged(int vehicle, int attacker, uint weaponHash, bool isMeleeDamage, int vehicleDamageTypeFlag) 
		{ 
			BaseScript.TriggerEvent(damageEventName + ":VehicleDamaged", vehicle, attacker, weaponHash, isMeleeDamage, vehicleDamageTypeFlag);
			Client.Logger.Debug($"[{damageEventName}:VehicleDamaged] vehicle: {vehicle}, attacker: {attacker}, weaponHash: {weaponHash}, vehicleDamageTypeFlag: {vehicleDamageTypeFlag}");
		}

		/// <summary>
		/// Event gets triggered whenever an entity is damaged but hasn't died from the damage.
		/// </summary>
		/// <param name="entity">Entity that got damaged.</param>
		/// <param name="attacker">The attacker that damaged the entity.</param>
		/// <param name="weaponHash">The weapon hash used to damage the entity.</param>
		/// <param name="isMeleeDamage">True if the damage was done using a melee weapon (including unarmed).</param>
		private static void EntityDamaged(int entity, int attacker, uint weaponHash, bool isMeleeDamage) 
		{ 
			BaseScript.TriggerEvent(damageEventName + ":EntityDamaged", entity, attacker, weaponHash, isMeleeDamage);
			Client.Logger.Debug($"[{damageEventName}:EntityDamaged] entity: {entity}, attacker: {attacker}, weaponHash: {weaponHash}, isMeleeDamage: {isMeleeDamage}");
		}

		/// <summary>
		/// Used internally to trigger the other events.
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="data"></param>
		private static void GameEventTriggered(string eventName, List<object> data)
		{
			Debug.WriteLine($"game event {eventName} ({String.Join(", ", data.ToArray())})");
			if (eventName == "CEventNetworkEntityDamage")
			{
				Entity victim = Entity.FromHandle(int.Parse(data[0].ToString()));
				Entity attacker = Entity.FromHandle(int.Parse(data[1].ToString()));
				int attackerint = int.Parse(data[1].ToString());

				bool victimDied = int.Parse(data[5].ToString()) == 1;

				uint weaponHash = (uint)int.Parse(data[6].ToString());

				bool isMeleeDamage = int.Parse(data[9].ToString()) != 0;
				int vehicleDamageTypeFlag = int.Parse(data[10].ToString());

				if (victim == null) return;

				if (attacker != null && attackerint != -1)
				{
					if (victimDied)
					{
						// victim died
						// vehicle destroyed
						if (victim.Model.IsVehicle)
						{
							VehicleDestroyed(victim.Handle, attacker.Handle, weaponHash, isMeleeDamage, vehicleDamageTypeFlag);
						}
						// other entity died
						else
						{
							// victim is a ped
							if (victim is Ped ped)
								switch (attacker)
								{
									case Vehicle veh:
										PedKilledByVehicle(victim.Handle, veh.Handle);

										break;
									case Ped p when p.IsPlayer:
										int player = NetworkGetPlayerIndexFromPed(p.Handle);
										PedKilledByPlayer(ped.Handle, player, weaponHash, isMeleeDamage);

										break;
									case Ped p:
										PedKilledByPed(ped.Handle, p.Handle, weaponHash, isMeleeDamage);

										break;
									default:
										PedDied(ped.Handle, attacker.Handle, weaponHash, isMeleeDamage);

										break;
								}
							// victim is not a ped
							else
								EntityKilled(victim.Handle, attacker.Handle, weaponHash, isMeleeDamage);
						}
					}
					else
					{
						// only damaged
						if (!victim.Model.IsVehicle)
							EntityDamaged(victim.Handle, attacker.Handle, weaponHash, isMeleeDamage);
						else
							VehicleDamaged(victim.Handle, attacker.Handle, weaponHash, isMeleeDamage, vehicleDamageTypeFlag);
					}
				}
				else
				{
					if (victimDied) 
						PedDied(victim.Handle, -1, weaponHash, isMeleeDamage);
					else
					{
						// only damaged
						if (!victim.Model.IsVehicle)
							EntityDamaged(victim.Handle, -1, weaponHash, isMeleeDamage);
						else
							VehicleDamaged(victim.Handle, -1, weaponHash, isMeleeDamage, vehicleDamageTypeFlag);
					}
				}
			}
		}
	}
}