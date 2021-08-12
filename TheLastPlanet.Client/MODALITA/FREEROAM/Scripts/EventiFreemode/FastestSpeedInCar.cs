using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Scripts.EventiFreemode
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
                    Screen.ShowSubtitle($"Recupera un veicolo da strada e preparati per la sfida ~b~{Name}~w~.", 50);
                else
                {
                    Screen.ShowSubtitle("Raggiungi la velocità più alta su un veicolo da strada.", 50);
                    if (Cache.PlayerCache.MyPlayer.Ped.IsInVehicle() && Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Speed > 0)
                    {
                        var speed = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Speed;
                        var speedKM = speed * 3.6f;
                        CurrentAttempt = speedKM;
                        /*
                        StatGetFloat(unchecked((uint)PlayerStat), ref tentativoCorrente, -1);
                        if (tentativoCorrente != 0)
                            CurrentAttempt = tentativoCorrente;
                        */
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
                var hash = unchecked((uint)PlayerStats.FastestSpeedInCar);
                StatSetFloat(hash, 0, true);
            }
            base.ResetEvent();
            Client.Instance.RemoveTick(OnTick);
        }
    }
}
