﻿using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Snowflakes;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core
{
    internal static class EntityCreation
    {
        public static void Init()
        {
            Server.Instance.Events.Mount("lprp:entity:spawnVehicle", new Func<uint, Position, Task<int>>(async (a, b) =>
            {
                try
                {
                    uint mod = a;
                    Position coords = b;
                    Vehicle veh = new(CreateVehicle(mod, coords.X, coords.Y, coords.Z, coords.Heading, true, true));
                    while (!DoesEntityExist(veh.Handle)) await BaseScript.Delay(0);
                    var decor = new { decorator = Snowflake.Next().ToInt64() };
                    veh.State.Set("decor", decor, true);
                    SetEntityDistanceCullingRadius(veh.Handle, 5000f);

                    return veh.NetworkId;
                }
                catch (Exception e)
                {
                    Server.Logger.Error(e.ToString());

                    return 0;
                }
            }));
            Server.Instance.Events.Mount("lprp:entity:spawnPed", new Func<uint, Position, int, Task<int>>(async (a, b, c) =>
            {
                try
                {
                    uint mod = a;
                    Position coords = b;
                    int type = c;
                    Ped ped = new(CreatePed(type, mod, coords.X, coords.Y, coords.Z, coords.Heading, true, true));
                    while (!DoesEntityExist(ped.Handle)) await BaseScript.Delay(0);
                    object decor = new { decorator = Snowflake.Next().ToInt64() };
                    ped.State.Set("decor", decor, true);
                    SetEntityDistanceCullingRadius(ped.Handle, 5000f);

                    return ped.NetworkId;
                }
                catch (Exception e)
                {
                    Server.Logger.Error(e.ToString());

                    return 0;
                }
            }));
            Server.Instance.Events.Mount("lprp:entity:spawnProp", new Func<int, Position, Task<int>>(async (a, b) =>
            {
                try
                {
                    int mod = a;
                    Position coords = b;
                    Prop prop = new(CreateObject(mod, coords.X, coords.Y, coords.Z, true, true, true));
                    while (!DoesEntityExist(prop.Handle)) await BaseScript.Delay(0);
                    object decor = new { decorator = Snowflake.Next().ToInt64() };
                    prop.State.Set("decor", decor, true);
                    SetEntityDistanceCullingRadius(prop.Handle, 5000f);
                    prop.Rotation = coords.ToRotationVector;

                    return prop.NetworkId;
                }
                catch (Exception e)
                {
                    Server.Logger.Debug(e.ToString());

                    return 0;
                }
            }));
        }
    }
}