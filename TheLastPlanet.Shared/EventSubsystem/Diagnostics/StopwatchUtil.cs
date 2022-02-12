using CitizenFX.Core.Native;
using System;
using TheLastPlanet.Shared.Internal.Events.Diagnostics.Impl;

namespace TheLastPlanet.Shared.Internal.Events.Diagnostics
{
    public abstract class StopwatchUtil
    {
        private static bool IsServer = API.IsDuplicityVersion();

        public abstract TimeSpan Elapsed { get; }
        public abstract void Stop();
        public abstract void Start();


        public static long Timestamp
        {
            get
            {
                if (IsServer)
                {
                    return ServerStopwatch.GetTimestamp();
                }
                else
                {
                    return ClientStopwatch.GetTimestamp();
                }
            }
        }

        public static StopwatchUtil StartNew()
        {
            if (IsServer)
            {
                return new ServerStopwatch();
            }

            return new ClientStopwatch();
        }
    }
}