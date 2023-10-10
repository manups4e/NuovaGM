﻿using System;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.FREEROAM.Scripts.EventiFreemode
{
    public class HighestJumpDistance : IWorldEvent
    {
        private float tentativoCorrente = 0;
        public HighestJumpDistance(int id, string name, double countdownTime, double seconds) : base(id, name, countdownTime, seconds, true, "Raggiungi l'altezza più elevata possibile durante un salto con un veicolo di terra", PlayerStats.HighestJumpDistance, "m", PlayerStatType.Float)
        {
        }
        public override void OnEventActivated()
        {
            Cache.PlayerCache.MyPlayer.Ped.Weapons.RemoveAll();
            Client.Instance.AddTick(OnTick);
            base.OnEventActivated();
        }

        public override void ResetEvent()
        {
            base.ResetEvent();
            Game.Player.WantedLevel = 0;

            Client.Instance.RemoveTick(OnTick);
            StatSetFloat((uint)PlayerStat, 0f, true);
            tentativoCorrente = 0;
        }
        private async Task OnTick()
        {
            try
            {
                if (!IsActive) { return; }

                if (!IsStarted)
                    Screen.ShowSubtitle($"Find a land vehicle and prepare for the ~b~{Name}~w~ challenge.", 50);
                else
                {
                    Screen.ShowSubtitle("Make the highest jump with a land vehicle.", 50);

                    if (Cache.PlayerCache.MyPlayer.Ped.IsInVehicle())
                    {
                        StatGetFloat(unchecked((uint)PlayerStat), ref tentativoCorrente, -1);

                        if (tentativoCorrente != 0)
                            CurrentAttempt = tentativoCorrente;
                        if (CurrentAttempt > BestAttempt)
                            BestAttempt = CurrentAttempt;

                        if (tentativoCorrente == CurrentAttempt)
                        {
                            StatSetFloat((uint)PlayerStat, 0f, true);
                            tentativoCorrente = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Client.Logger.Error(ex.ToString());
            }

            await Task.FromResult(0);
        }
    }
}

