using CitizenFX.Core;
using Logger;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastPlanet.Server.Scripts.EventiFreemode
{
    static class VehicleManager
    {
        public static List<int> SpawnedEventVehicles = new List<int>();

        public static void Init()
        {
            Server.Instance.AddEventHandler("worldEventsManage.Server:SpawnedEventVehicles", new Action<Player, List<dynamic>>(OnSpawnedEventVehicles));
        }

        private static void OnSpawnedEventVehicles([FromSource]Player player, List<dynamic> dynamicVehicles)
        {
            try
            {
                SpawnedEventVehicles.Clear();
                foreach (var v in dynamicVehicles)
                {
                    SpawnedEventVehicles.Add((int)v);
                }

                BaseScript.TriggerClientEvent("worldEventsManage.Client:SetVehicleBlips", SpawnedEventVehicles);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }
    }
}
