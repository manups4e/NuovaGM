﻿namespace TheLastPlanet.Shared
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
                long await = _awaitable;
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
                bool passed = GetGameTimer() - Timer > await;
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
            Timer = GetGameTimer();
        }
    }
}
