using System;
using System.Collections.Generic;
using TheLastPlanet.Server.Core.Buckets;


namespace TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode
{
    static class VehicleManager
    {
        public static List<int> SpawnedEventVehicles = new List<int>();

        public static void Init()
        {
            EventDispatcher.Mount("worldEventsManage.Server:SpawnedEventVehicles", new Action<PlayerClient, List<int>>(OnSpawnedEventVehicles));
        }

        private static void OnSpawnedEventVehicles([FromSource] PlayerClient client, List<int> dynamicVehicles)
        {
            try
            {
                SpawnedEventVehicles.Clear();
                foreach (var v in dynamicVehicles)
                {
                    SpawnedEventVehicles.Add(v);
                }

                EventDispatcher.Send(BucketsHandler.FreeRoam.Bucket.Players, "worldEventsManage.Client:SetVehicleBlips", SpawnedEventVehicles);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }
    }
}
