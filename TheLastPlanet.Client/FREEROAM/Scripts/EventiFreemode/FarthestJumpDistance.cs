using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Scripts.EventiFreemode
{
    public class FarthestJumpDistance : IWorldEvent
    {
        private float tentativoCorrente = 0;
        public FarthestJumpDistance(int id, string name, double countdownTime, double seconds) : base(id, name, countdownTime, seconds, false, "AMCH_BIG_0", PlayerStats.FarthestJumpDistance, "m", PlayerStatType.Float)
        {
        }

        public override void OnEventActivated()
        {
            FirstStartedTick = true;
            Cache.MyPlayer.Ped.Weapons.RemoveAll();
            base.OnEventActivated();
            Client.Instance.AddTick(OnTick);
        }

        public override void ResetEvent()
        {
            Cache.MyPlayer.Player.WantedLevel = 0;
            base.ResetEvent();
            Client.Instance.RemoveTick(OnTick);
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
                    Screen.ShowSubtitle(GetLabelText("AMCH_BIG_0"), 50);
                    if (Cache.MyPlayer.Ped.IsInVehicle())
                    {
                        await BaseScript.Delay(300);
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

