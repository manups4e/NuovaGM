﻿using CitizenFX.Core;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.Buckets;

namespace TheLastPlanet.Server.Scripts
{
    static class TickHandler
    {
        public static void Init()
        {
            //Server.Instance.AddTick(Salvataggio);
        }

        private static async Task Salvataggio()
        {
            await BaseScript.Delay(600000);
            foreach (var p in BucketsHandler.FreeRoam.Bucket.Players)
            {
                var user = Funzioni.GetClientFromPlayerId(p.Handle);
                if (user != null && user.Status.PlayerStates.Spawned)
                    BucketsHandler.RolePlay.SalvaPersonaggioRoleplay(p);
            }
        }
    }
}
