using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.FREEROAM
{
    static class PlayerBlipsHandler
    {
        private static List<FRBlipsInfo> _blipsInfos = new();
        private static SharedTimer blipTimer;
        private static readonly List<int> JetHashes = new() { 970385471, -1281684762, 1824333165 };

        public static void Init()
        {
            blipTimer = new(500);
            Server.Instance.AddTick(UpdatePlayersBlips);
            AccessingEvents.OnFreeRoamSpawn += (client) =>
            {
                var serverId = client.Handle;
                var pos = client.Ped.Position;
                var rot = client.Ped.Rotation;
                var netId = client.Ped.NetworkId;
                FRBlipsInfo user = new(client.Player.Name, new Position(pos, rot), netId, serverId);
                _blipsInfos.Add(user);
            };

            AccessingEvents.OnFreeRoamLeave += (client) =>
            {
                _blipsInfos.RemoveAll(x => client.Handle == x.ServerId);
            };
        }

        private static async Task UpdatePlayersBlips()
        {
            try
            {
                await BaseScript.Delay(500);
                if (BucketsHandler.FreeRoam.GetTotalPlayers() < 1) return; // cambiare con <= ... se sono da solo non ha senso (se non per testare)

                foreach (var client in BucketsHandler.FreeRoam.Bucket.Players)
                {
                    if (!client.Status.PlayerStates.Spawned) continue;
                    var serverId = Convert.ToInt32(client.Player.Handle);
                    var pos = client.Ped.Position;
                    var rot = client.Ped.Rotation;
                    var netId = client.Ped.NetworkId;
                    Ped p = client.Ped;
                    if (_blipsInfos.Any(x => x.ServerId == serverId))
                    {
                        var blip = _blipsInfos.SingleOrDefault(x => x.ServerId == serverId);
                        blip.Pos = new(pos, rot);
                        var veh = GetVehiclePedIsIn(p.Handle, false);
                        var model = GetEntityModel(veh);
                        int sprite = 1;
                        if (veh != 0)
                        {
                            switch (GetVehicleType(veh))
                            {
                                case "heli":
                                    sprite = 422;
                                    break;
                                case "plane":
                                    sprite = 423;
                                    if (JetHashes.Contains(model))
                                        sprite = 424;
                                    break;
                                case "boat":
                                    sprite = 427;
                                    break;
                                case "bike":
                                    sprite = 226;
                                    break;
                                case "automobile":
                                    sprite = 225;
                                    break;
                            }
                        }
                        blip.Sprite = sprite;
                    }
                    client.User.FreeRoamChar.Posizione = new(pos, rot);
                }
                Server.Instance.Events.Send(BucketsHandler.FreeRoam.Bucket.Players, "freeroam.UpdatePlayerBlipInfos", _blipsInfos);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }
    }
}
