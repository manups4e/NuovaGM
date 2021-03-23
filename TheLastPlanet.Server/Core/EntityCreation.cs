using System;
using System.Threading.Tasks;
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
           Server.Instance.Events.Mount("lprp:entity:spawnVehicle", new Func<uint, Position, float, Task<int>>(async (a, b, c) => 
           {
               try
               {
                   var mod = a;
                   var coords = b;
                   var head = c;
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

            Server.Instance.Events.Mount("lprp:entity:spawnPed", new Func<uint, Position, float, int, Task<int>>(async (a, b, c, d) =>
            {
                try
                {
                    var mod = a;
                    var coords = b;
                    var head = c;
                    var type = d;
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