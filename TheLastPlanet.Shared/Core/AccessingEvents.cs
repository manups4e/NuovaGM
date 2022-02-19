using System;
using System.Collections.Generic;
using System.Text;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Shared
{
    public delegate void OnFreeRoamSpawn(ClientId client);
    public delegate void OnRoleplaySpawn(ClientId client);
    public delegate void OnFreeRoamLeave(ClientId client);
    public delegate void OnRoleplayLeave(ClientId client);
    public class AccessingEvents
    {
        public static event OnFreeRoamSpawn OnFreeRoamSpawn;
        public static event OnRoleplaySpawn OnRoleplaySpawn;
        public static event OnFreeRoamLeave OnFreeRoamLeave;
        public static event OnRoleplayLeave OnRoleplayLeave;

        public static void FreeRoamSpawn(ClientId client)
        {
            OnFreeRoamSpawn?.Invoke(client);
        }
        public static void RoleplaySpawn(ClientId client)
        {
            OnRoleplaySpawn?.Invoke(client);
        }
        public static void FreeRoamLeave(ClientId client)
        {
            OnFreeRoamLeave?.Invoke(client);
        }
        public static void RoleplayLeave(ClientId client)
        {
            OnRoleplayLeave?.Invoke(client);
        }

    }
}
