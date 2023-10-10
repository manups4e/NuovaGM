﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.Handlers.EntityHandling
{
    static class EntityHandler
    {
        public static List<EntityHandle> Entità = new List<EntityHandle>();
        public static void Init()
        {

        }

        public static async Task EntityHandlings()
        {
            foreach (var entity in Entità)
            {
                if (entity.Position.Distance(Cache.PlayerCache.MyPlayer.Position) < 100)
                {
                    if (entity.GetType() == typeof(PedHandle))
                    {
                        await ((PedHandle)entity).Spawn();
                    }
                    else if (entity.GetType() == typeof(VehicleHandle))
                    {
                        await ((VehicleHandle)entity).Spawn();
                    }
                    else if (entity.GetType() == typeof(PropHandle))
                    {
                        await ((PropHandle)entity).Spawn();
                    }
                }
                else { }
            }
        }
    }
}
