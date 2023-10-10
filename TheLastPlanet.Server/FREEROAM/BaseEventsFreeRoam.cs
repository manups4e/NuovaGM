using System;
using System.Collections.Generic;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.Buckets;


namespace TheLastPlanet.Server
{
    static class BaseEventsFreeRoam
    {
        private static List<string> killedMessages = new List<string>()
        {
            "killed",
            "destroyed",
            "erased",
            "slaughtered",
            "annihilated",
            "dissolved",
            "pulverized",
            "devastated",
            "executed",
            "atomized",
            "murdered",
            "flattened",
            "scrapped",
        };

        public static void Init()
        {
            EventDispatcher.Mount("lpop:onPlayerDied", new Action<PlayerClient, int, int, Position>(OnPlayerDied));
        }

        private static void OnPlayerDied([FromSource] PlayerClient player, int tipo, int killer, Position victimCoords)
        {
            string morte = "";
            Player Killer = Functions.GetPlayerFromId(killer);
            switch (tipo)
            {
                case 0:
                    morte = $"~h~{player.Player.Name}~h~ killed himself.";
                    break;
                case 1:
                    morte = $"~h~{Killer.Name}~h~ {killedMessages[new Random().Next(killedMessages.Count - 1)]} {player.Player.Name}";
                    break;
                case -1:
                    morte = $"~h~{player.Player.Name}~h~ died.";
                    break;
            }
            EventDispatcher.Send(BucketsHandler.FreeRoam.Bucket.Players, "lpop:ShowNotification", morte);
        }
    }
}
