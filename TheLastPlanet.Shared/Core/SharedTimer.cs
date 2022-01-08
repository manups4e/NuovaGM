using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Shared
{
    public class SharedTimer
    {
        private long Timer = 0;
        private readonly long _awaitable = 0;
        private bool isPassed;

        public bool IsPassed
        {
            get
            {
                isPassed = API.GetGameTimer() - Timer > _awaitable;
                if (isPassed) ResetTimer();
                return isPassed;
            }
            private set
            {
                isPassed = value;
            }
        }

        public SharedTimer(long time)
        {
            _awaitable = time;
            ResetTimer();
        }

        public void ResetTimer()
        {
            Timer = API.GetGameTimer();
        }
    }
}
