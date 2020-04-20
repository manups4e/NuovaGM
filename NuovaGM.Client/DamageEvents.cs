using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client
{
	public class DamageEvents
	{

		public const string eventName = "DamageEvents";
		public static void Init()
		{
			Client.Instance.AddEventHandler("gameEventTriggered", new Action<string, List<Object>>(GameEventTriggered));
			if (GetResourceMetadata(GetCurrentResourceName(), "enable_debug_prints_for_events", 0).ToLower() == "true")
			{
				Client.Instance.AddEventHandler(eventName + ":VehicleDestroyed", new Action<int, int, uint, bool, int>((a, b, c, d, e) =>
				{
					Log.Printa(LogType.Debug, "event: VehicleDestroyed");
					Log.Printa(LogType.Debug,$"vehicle: {a}");
					Log.Printa(LogType.Debug,$"attacker: {b}");
					Log.Printa(LogType.Debug,$"weapon hash: {c}");
					Log.Printa(LogType.Debug,$"was melee damage?: {d}");
					Log.Printa(LogType.Debug,$"vehicle damage flag: {e}");
				}));
				Client.Instance.AddEventHandler(eventName + ":PedKilledByVehicle", new Action<int, int>((a, b) =>
				{
					Log.Printa(LogType.Debug,"event: PedKilledByVehicle");
					Log.Printa(LogType.Debug,$"victim: {a}");
					Log.Printa(LogType.Debug,$"vehicle: {b}");
				}));
				Client.Instance.AddEventHandler(eventName + ":PedKilledByPlayer", new Action<int, int, uint, bool>((a, b, c, d) =>
				{
					Log.Printa(LogType.Debug,"event: PedKilledByPlayer");
					Log.Printa(LogType.Debug,$"victim: {a}");
					Log.Printa(LogType.Debug,$"player: {b}");
					Log.Printa(LogType.Debug,$"weapon hash: {c}");
					Log.Printa(LogType.Debug,$"was melee damage?: {d}");
				}));
				Client.Instance.AddEventHandler(eventName + ":PedKilledByPed", new Action<int, int, uint, bool>((a, b, c, d) =>
				{
					Log.Printa(LogType.Debug,"event: PedKilledByPed");
					Log.Printa(LogType.Debug,$"victim: {a}");
					Log.Printa(LogType.Debug,$"attacker: {b}");
					Log.Printa(LogType.Debug,$"weapon hash: {c}");
					Log.Printa(LogType.Debug,$"was melee damage?: {d}");
				}));
				Client.Instance.AddEventHandler(eventName + ":PedDied", new Action<int, int, uint, bool>((a, b, c, d) =>
				{
					Log.Printa(LogType.Debug,"event: PedDied");
					Log.Printa(LogType.Debug,$"victim: {a}");
					Log.Printa(LogType.Debug,$"attacker: {b}");
					Log.Printa(LogType.Debug,$"weapon hash: {c}");
					Log.Printa(LogType.Debug,$"was melee damage?: {d}");
				}));
				Client.Instance.AddEventHandler(eventName + ":EntityKilled", new Action<int, int, uint, bool>((a, b, c, d) =>
				{
					Log.Printa(LogType.Debug,"event: EntityKilled");
					Log.Printa(LogType.Debug,$"victim: {a}");
					Log.Printa(LogType.Debug,$"attacker: {b}");
					Log.Printa(LogType.Debug,$"weapon hash: {c}");
					Log.Printa(LogType.Debug,$"was melee damage?: {d}");
				}));
				Client.Instance.AddEventHandler(eventName + ":VehicleDamaged", new Action<int, int, uint, bool, int>((a, b, c, d, e) =>
				{
					Log.Printa(LogType.Debug,"event: VehicleDamaged");
					Log.Printa(LogType.Debug,$"vehicle: {a}");
					Log.Printa(LogType.Debug,$"attacker: {b}");
					Log.Printa(LogType.Debug,$"weapon hash: {c}");
					Log.Printa(LogType.Debug,$"was melee damage?: {d}");
					Log.Printa(LogType.Debug,$"vehicle damage flag: {e}");
				}));
				Client.Instance.AddEventHandler(eventName + ":EntityDamaged", new Action<int, int, uint, bool>((a, b, c, d) =>
				{
					Log.Printa(LogType.Debug,"event: EntityDamaged");
					Log.Printa(LogType.Debug,$"victim: {a}");
					Log.Printa(LogType.Debug,$"attacker: {b}");
					Log.Printa(LogType.Debug,$"weapon hash: {c}");
					Log.Printa(LogType.Debug,$"was melee damage?: {d}");
				}));
			}
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
			BaseScript.TriggerEvent(eventName + ":VehicleDestroyed", vehicle, attacker, weaponHash, isMeleeDamage, vehicleDamageTypeFlag);
		}

		/// <summary>
		/// Event gets triggered whenever a ped was killed by a vehicle without a driver.
		/// </summary>
		/// <param name="ped">Ped that got killed.</param>
		/// <param name="vehicle">Vehicle that was used to kill the ped.</param>
		private static void PedKilledByVehicle(int ped, int vehicle)
		{
			BaseScript.TriggerEvent(eventName + ":PedKilledByVehicle", ped, vehicle);
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
			BaseScript.TriggerEvent(eventName + ":PedKilledByPlayer", ped, player, weaponHash, isMeleeDamage);
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
			BaseScript.TriggerEvent(eventName + ":PedKilledByPed", ped, attackerPed, weaponHash, isMeleeDamage);
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
			BaseScript.TriggerEvent(eventName + ":PedDied", ped, attacker, weaponHash, isMeleeDamage);
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
			BaseScript.TriggerEvent(eventName + ":EntityKilled", entity, attacker, weaponHash, isMeleeDamage);
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
			BaseScript.TriggerEvent(eventName + ":VehicleDamaged", vehicle, attacker, weaponHash, isMeleeDamage, vehicleDamageTypeFlag);
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
			BaseScript.TriggerEvent(eventName + ":EntityDamaged", entity, attacker, weaponHash, isMeleeDamage);
		}


		/// <summary>
		/// Used internally to trigger the other events.
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="data"></param>
		private static void GameEventTriggered(string eventName, List<object> data)
		{
			if (eventName == "CEventNetworkEntityDamage")
			{
				Entity victim = Entity.FromHandle(int.Parse(data[0].ToString()));
				Entity attacker = Entity.FromHandle(int.Parse(data[1].ToString()));
				bool victimDied = int.Parse(data[3].ToString()) == 1;
				uint weaponHash = (uint)int.Parse(data[4].ToString());
				bool isMeleeDamage = int.Parse(data[9].ToString()) != 0;
				int vehicleDamageTypeFlag = int.Parse(data[10].ToString());
				Log.Printa(LogType.Debug,"Dati Json = " + JsonConvert.SerializeObject(data));

				if (victim != null && attacker != null)
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
							{
								if (attacker is Vehicle veh)
								{
									PedKilledByVehicle(victim.Handle, attacker.Handle);
								}
								else if (attacker is Ped p)
								{
									if (p.IsPlayer)
									{
										int player = NetworkGetPlayerIndexFromPed(p.Handle);
										PedKilledByPlayer(victim.Handle, player, weaponHash, isMeleeDamage);
									}
									else
									{
										PedKilledByPed(victim.Handle, attacker.Handle, weaponHash, isMeleeDamage);
									}
								}
								else
								{
									PedDied(victim.Handle, attacker.Handle, weaponHash, isMeleeDamage);
								}
							}
							// victim is not a ped
							else
							{
								EntityKilled(victim.Handle, attacker.Handle, weaponHash, isMeleeDamage);
							}
						}
					}
					else
					{
						// only damaged
						if (!victim.Model.IsVehicle)
						{
							EntityDamaged(victim.Handle, attacker.Handle, weaponHash, isMeleeDamage);
						}
						else
						{
							VehicleDamaged(victim.Handle, attacker.Handle, weaponHash, isMeleeDamage, vehicleDamageTypeFlag);
						}
					}
				}
			}
		}

	}

}
