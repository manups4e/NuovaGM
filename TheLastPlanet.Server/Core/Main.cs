﻿using CitizenFX.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core
{
    public static class Main
    {
        private static DateTime Now;
        public static void Init()
        {
            Server.Instance.AddTick(Orario_Playtime);
            Now = DateTime.UtcNow;
        }

        private static async Task Orario_Playtime()
        {
            try
            {
                // gestione tempo players da rifare.. sicuramente!
                await BaseScript.Delay(600000);
                if (Server.Instance.Clients.Count > 0)
                {
                    foreach (var user in from user in Server.Instance.Clients where user.User is not null where (user.Status is not null && user.Status.PlayerStates.Spawned) select user)
                    {
                        user.User.playTime += 600;
                    }
                }
                var ora = DateTime.UtcNow - Now;
                SetConvarServerInfo("Attivo da:", $"{ora.Days} giorni {ora.Hours} Ore {ora.Minutes} Minuti");
                //Server.Logger.Debug($"Attivo da: {ora.Days} giorni {ora.Hours} Ore {ora.Minutes} Minuti {ora.Seconds} secondi");
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString() + e.StackTrace);
            }
        }
    }
}