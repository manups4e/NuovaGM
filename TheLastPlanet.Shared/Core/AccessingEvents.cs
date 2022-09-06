using System;
using System.Collections.Generic;
using System.Text;


namespace TheLastPlanet.Shared
{
    public delegate void OnFreeRoamSpawn(PlayerClient client);
    public delegate void OnRoleplaySpawn(PlayerClient client);
    public delegate void OnFreeRoamLeave(PlayerClient client);
    public delegate void OnRoleplayLeave(PlayerClient client);
    public class AccessingEvents
    {
        public static event OnFreeRoamSpawn OnFreeRoamSpawn;
        public static event OnRoleplaySpawn OnRoleplaySpawn;
        public static event OnFreeRoamLeave OnFreeRoamLeave;
        public static event OnRoleplayLeave OnRoleplayLeave;

        public static void FreeRoamSpawn(PlayerClient client)
        {
            OnFreeRoamSpawn?.Invoke(client);
        }
        public static void RoleplaySpawn(PlayerClient client)
        {
            OnRoleplaySpawn?.Invoke(client);
        }
        public static void FreeRoamLeave(PlayerClient client)
        {
            OnFreeRoamLeave?.Invoke(client);
        }
        public static void RoleplayLeave(PlayerClient client)
        {
            OnRoleplayLeave?.Invoke(client);
        }

    }
}
