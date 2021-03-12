using System.Data;
using CitizenFX.Core;
using TheLastPlanet.Shared.SistemaEventi;
using static CitizenFX.Core.Native.API;
namespace TheLastPlanet.Server.Core
{
    static class EntityCreation
    {
        public static void Init()
        {
            Server.Instance.Eventi.Attach("lprp:entity:spawnVehicle", new EventCallback( metadata =>
            {
                var mod = metadata.Find<uint>(0);
                var coords = metadata.Find<Vector3>(1);
                var head = metadata.Find<float>(2);
                Vehicle veh = new (CreateVehicle(mod, coords.X, coords.Y, coords.Z, head, true, true));
                SetEntityDistanceCullingRadius(veh.Handle, 5000f);
                return veh.NetworkId;
            }));
            Server.Instance.Eventi.Attach("lprp:entity:spawnPed", new EventCallback( metadata =>
            {
                var mod = metadata.Find<uint>(0);
                var coords = metadata.Find<Vector3>(1);
                var head = metadata.Find<float>(2);
                var type = metadata.Find<int>(3);
                Ped ped = new (CreatePed(type, mod, coords.X, coords.Y, coords.Z, head, true, true));
                SetEntityDistanceCullingRadius(ped.Handle, 5000f);
                return ped.NetworkId;
            }));
        }
        
    }
}