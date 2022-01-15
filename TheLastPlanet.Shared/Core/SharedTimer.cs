using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Shared
{
    public enum TimerType
    {
        Milliseconds,
        Seconds,
        Minutes,
    }
    public class SharedTimer
    {
        private long Timer = 0;
        private readonly long _awaitable = 0;
        private TimerType timerType;

        public bool IsPassed
        {
            get
            {
                var await = _awaitable;
                switch (timerType)
                {
                    case TimerType.Milliseconds:
                        await = _awaitable;
                        break;
                    case TimerType.Seconds:
                        await = _awaitable * 1000;
                        break;
                    case TimerType.Minutes:
                        await = _awaitable * 1000 * 60;
                        break;
                }
                bool passed = API.GetGameTimer() - Timer > await;
                if (passed) ResetTimer();
                return passed;
            }
        }

        public SharedTimer(long time, TimerType type = TimerType.Milliseconds)
        {
            _awaitable = time;
            timerType = type;
            ResetTimer();
        }

        public void ResetTimer()
        {
            Timer = API.GetGameTimer();
        }
    }
}
