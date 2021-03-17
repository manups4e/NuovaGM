using CitizenFX.Core;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.SistemaEventi;
using static CitizenFX.Core.Native.API;
namespace TheLastPlanet.Server.Core
{
    static class EntityCreation
    {
        public static void Init()
        {
            ServerSession.Instance.SistemaEventi.Attach("lprp:entity:spawnVehicle", new EventCallback( metadata =>
            {
                Logger.Log.Printa(Logger.LogType.Debug, metadata.Datapack.SerializeToJson(Newtonsoft.Json.Formatting.Indented));
                var mod = metadata.Find<uint>(0);
                var coords = metadata.Find<Vector3>(1);
                var head = metadata.Find<float>(2);
                Vehicle veh = new (CreateVehicle(mod, coords.X, coords.Y, coords.Z, head, true, true));
                SetEntityDistanceCullingRadius(veh.Handle, 5000f);
                return veh.NetworkId;
            }));
            ServerSession.Instance.SistemaEventi.Attach("lprp:entity:spawnPed", new EventCallback( metadata =>
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