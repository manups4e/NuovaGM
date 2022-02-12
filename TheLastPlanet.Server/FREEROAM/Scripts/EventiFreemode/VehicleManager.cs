using System;
using System.Collections.Generic;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode
{
    static class VehicleManager
    {
        public static List<int> SpawnedEventVehicles = new List<int>();

        public static void Init()
        {
            Server.Instance.Events.Mount("worldEventsManage.Server:SpawnedEventVehicles", new Action<ClientId, List<int>>(OnSpawnedEventVehicles));
        }

        private static void OnSpawnedEventVehicles(ClientId client, List<int> dynamicVehicles)
        {
            try
            {
                SpawnedEventVehicles.Clear();
                foreach (var v in dynamicVehicles)
                {
                    SpawnedEventVehicles.Add(v);
                }

                Server.Instance.Events.Send(BucketsHandler.FreeRoam.Bucket.Players, "worldEventsManage.Client:SetVehicleBlips", SpawnedEventVehicles);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }
    }
}
