﻿namespace TheLastPlanet.Server.Scripts
{
    static class TickHandler
    {
        public static void Init()
        {
            //Server.Instance.AddTick(Salvataggio);
        }

        /*
        private static async Task Save()
        {
            await BaseScript.Delay(600000);
            foreach (PlayerClient p in BucketsHandler.FreeRoam.Bucket.Players)
            {
                PlayerClient user = Functions.GetClientFromPlayerId(p.Handle);
                if (user != null && user.Status.PlayerStates.Spawned)
                    BucketsHandler.RolePlay.SalvaPersonaggioRoleplay(p);
            }
        }
        */
    }
}
