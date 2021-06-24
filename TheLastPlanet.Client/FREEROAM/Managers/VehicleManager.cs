using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.SessionCache;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.FreeRoam.Managers
{
    static class VehicleManager
    {
        private static List<Blip> ActiveBlips = new List<Blip>();
        private static List<int> ActiveVehicles = new List<int>();
        private static bool justDestroyed = false;
        private static int start = 0;

        public static void Init()
        {
            Client.Instance.Eventi.Mount("worldEventsManage.Client:DestroyEventVehicles", new Action(OnDestroySpawnedEventVehicles));
            Client.Instance.AddTick(OnTick);

            DecorRegister("weEventVehicle", 2);
        }

        public static async Task SpawnEventVehicles(Dictionary<Vector4, VehicleHash> spawnLocations)
        {
            try
            {
                var veh = World.GetAllVehicles();
                if (veh.Count() != 0)
                {
                    foreach (var v in veh)
                    {
                        if (DecorExistOn(v.Handle, "weOwnedVeh") || v.Driver == Cache.MyPlayer.Ped) continue;
                        v.Delete();
                    }
                }
                foreach (var location in spawnLocations.Keys)
                {
                    ClearAreaOfVehicles(location.X, location.Y, location.Z, 10000f, false, false, false, false, false);
                }


                if (NetworkIsHost())
                {
                    var temp = new List<int>();

                    foreach (var activeVehicle in ActiveVehicles)
                    {
                        var reffie = activeVehicle;
                        SetEntityAsNoLongerNeeded(ref reffie);
                    }

                    foreach (var vehicle in spawnLocations)
                    {
                        var vehPos = new Vector3(vehicle.Key.X, vehicle.Key.Y, vehicle.Key.Z);
                        var playerInArea = false;
                        for (int i = 0; i < 64; i++)
                        {
                            if (NetworkIsPlayerActive(i))
                            {
                                var ped = GetPlayerPed(i);
                                var pos = GetEntityCoords(ped, true);
                                if (pos.DistanceToSquared(vehPos) < 100f)
                                {
                                    playerInArea = true;
                                }
                            }
                        }

                        if (playerInArea == false)
                        {
                            var spawnedVehicle = await ClearAndSpawnVehicles(vehicle);
                            if (spawnedVehicle != 0)
                            {
                                temp.Add(spawnedVehicle);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Client.Logger.Error(e.ToString());
            }

            await Task.FromResult(0);
        }

        private static async Task OnTick()
        {
            if (justDestroyed)
            {
                if(GetGameTimer() - start < 25000)
                    Cache.MyPlayer.Player.WantedLevel = 0;
                else
                    justDestroyed = false;
            }

            await BaseScript.Delay(0);
        }


        private static async void OnDestroySpawnedEventVehicles()
        {
            if (!Cache.MyPlayer.Ped.IsInVehicle())
            {
                Client.Logger.Debug("Player is not in a vehicle.");
                return;
            }

            var currentVehicle = Cache.MyPlayer.Ped.CurrentVehicle;

            if (!DecorExistOn(currentVehicle.Handle, "weEventVehicle"))
            {
                Client.Logger.Debug("Not in an Event Vehicle");
                return;
            }

            HUD.ShowAdvancedNotification("Il tuo veicolo verrà distrutto in 10 secondi", "Attenzione!");
            await BaseScript.Delay(7000);

            if (DecorExistOn(currentVehicle.Handle, "weEventVehicle"))
            {
                Client.Logger.Debug("Still in an event vehicle, destroying..");
                Audio.PlaySoundFrontend("BOATS_PLANES_HELIS_BOOM", "MP_LOBBY_SOUNDS");
                if (Cache.MyPlayer.Ped.IsInFlyingVehicle)
                {
                    HUD.ShowAdvancedNotification("Premi ~INPUT_PARACHUTE_DEPLOY~ per usare il paracadute!", "Emergenza Paracadute");
                    Cache.MyPlayer.Ped.Weapons.Give(WeaponHash.Parachute, 999, true, true);
                }

                Cache.MyPlayer.Ped.Task.LeaveVehicle(LeaveVehicleFlags.BailOut);

                await BaseScript.Delay(5000);
                currentVehicle.ExplodeNetworked();

                if (Cache.MyPlayer.Ped.IsFalling)
                    Cache.MyPlayer.Ped.OpenParachute();

                HUD.ShowAdvancedNotification("Sei stato cacciato dal tuo veicolo", "Attenzione!");
            }

            justDestroyed = true;
            start = GetGameTimer();

            foreach (var activeVehicle in ActiveVehicles)
            {
                if (DoesEntityExist(activeVehicle))
                {
                    var veh = new Vehicle(activeVehicle);
                    veh.Delete();
                }
            }
        }

        private static async Task<int> ClearAndSpawnVehicles(KeyValuePair<Vector4, VehicleHash> vehicle)
        {
            try
            {
                ClearAreaOfVehicles(vehicle.Key.X, vehicle.Key.Y, vehicle.Key.Z, 2500f, false, false, false, false, false);
                var attempts = 0;
                do
                {
                    ClearAreaOfVehicles(vehicle.Key.X, vehicle.Key.Y, vehicle.Key.Z, 2500f, false, false, false, false, false);
                    attempts++;
                } while (attempts != 20);

                var newVeh = await World.CreateVehicle(vehicle.Value, new Vector3(vehicle.Key.X, vehicle.Key.Y, vehicle.Key.Z - 0.55f), vehicle.Key.W);
                if (newVeh == null)
                {
                    Client.Logger.Debug("Something went wrong while creating a vehicle.");
                    return 0;
                }

                DecorSetBool(newVeh.Handle, "weEventVehicle", true);
                if (!DecorExistOn(newVeh.Handle, "weEventVehicle"))
                {
                    return 0;
                }
                return newVeh.Handle;
            }
            catch (Exception e)
            {
                Client.Logger.Error(e.ToString());
            }

            return 0;
        }
    }
}
