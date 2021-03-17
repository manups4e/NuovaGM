using System;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.SistemaEventi;
using static CitizenFX.Core.Native.API;
namespace TheLastPlanet.Server.Core
{
    static class EntityCreation
    {
        public static void Init()
        {
           ServerSession.Instance.SistemaEventi.Attach("lprp:entity:spawnVehicle", new AsyncEventCallback( async metadata =>
           {
               try
               {
                   var mod = metadata.Find<uint>(0);
                   var coords = new Vector3(metadata.Find<float>(1), metadata.Find<float>(2), metadata.Find<float>(3));
                   var head = metadata.Find<float>(4);
                   Vehicle veh = new(CreateVehicle(mod, coords.X, coords.Y, coords.Z, head, true, true));
                   while (!DoesEntityExist(veh.Handle)) await BaseScript.Delay(0);
                   SetEntityDistanceCullingRadius(veh.Handle, 5000f);
                   return veh.NetworkId;
               }
               catch (Exception e)
               {
                   Log.Printa(LogType.Error, e.ToString());
                   return 0;
               }
           }));

            ServerSession.Instance.SistemaEventi.Attach("lprp:entity:spawnPed", new AsyncEventCallback(async metadata =>
            {
                try
                {
                    var mod = metadata.Find<uint>(0);
                    var coords = new Vector3(metadata.Find<float>(1), metadata.Find<float>(2), metadata.Find<float>(3));
                    var head = metadata.Find<float>(4);
                    var type = metadata.Find<int>(5);
                    Ped ped = new(CreatePed(type, mod, coords.X, coords.Y, coords.Z, head, true, true));
                   while (!DoesEntityExist(ped.Handle)) await BaseScript.Delay(0);
                    SetEntityDistanceCullingRadius(ped.Handle, 5000f);
                    return ped.NetworkId;
                }
                catch (Exception e)
                {
                    Log.Printa(LogType.Error, e.ToString());
                    return 0;
                }
            }));
        }
    }
}