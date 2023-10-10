﻿using System;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.FREEROAM.Scripts.EventiFreemode
{
    class FastestSpeedInCar : IWorldEvent
    {
        private float tentativoCorrente = 0;
        public FastestSpeedInCar(int id, string name, double countdownTime, double seconds) : base(id, name, countdownTime, seconds, true, "Raggiungi la velocità più alta su un veicolo di terra", PlayerStats.FastestSpeedInCar, "km/h", PlayerStatType.Float)
        {
        }

        public override void OnEventActivated()
        {
            FirstStartedTick = true;
            base.OnEventActivated();
            Client.Instance.AddTick(OnTick);
        }
        private async Task OnTick()
        {
            try
            {
                if (!IsActive) { return; }

                if (!IsStarted)
                    Screen.ShowSubtitle($"Find a land vehicle and prepare for the ~b~{Name}~w~ challenge.", 1);
                else
                {
                    Screen.ShowSubtitle(Game.GetGXTEntry("AMCH_2"), 1);
                    if (Cache.PlayerCache.MyPlayer.Ped.IsInVehicle() && Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Speed > 0)
                    {
                        float speed = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Speed;
                        float speedKM = speed * 3.6f;
                        CurrentAttempt = speedKM;
                        StatGetFloat(unchecked((uint)PlayerStat), ref tentativoCorrente, -1);
                        if (tentativoCorrente != 0)
                            CurrentAttempt = tentativoCorrente;
                        if (CurrentAttempt > BestAttempt)
                            BestAttempt = CurrentAttempt;
                    }
                }
            }
            catch (Exception ex)
            {
                Client.Logger.Error(ex.ToString());
            }

            await Task.FromResult(0);
        }

        public override void ResetEvent()
        {
            FirstStartedTick = true;
            if (FirstStartedTick)
            {
                FirstStartedTick = false;
                uint hash = unchecked((uint)PlayerStats.FastestSpeedInCar);
                StatSetFloat(hash, 0, true);
            }
            base.ResetEvent();
            Client.Instance.RemoveTick(OnTick);
        }
    }
}
