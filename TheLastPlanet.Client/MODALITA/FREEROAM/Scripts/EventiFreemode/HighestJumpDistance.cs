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
                    Screen.ShowSubtitle($"Trova un veicolo di terra e preparati per la sfida ~b~{Name}~w~.", 50);
                else
                {
                    Screen.ShowSubtitle("Effettua il salto più alto su un veicolo di terra.", 50);

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

