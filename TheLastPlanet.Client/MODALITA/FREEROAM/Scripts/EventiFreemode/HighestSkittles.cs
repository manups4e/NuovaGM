using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Scripts.EventiFreemode
{
    public class HighestSkittles : IWorldEvent
    {
        public HighestSkittles(int id, string name, double countdownTime, double seconds) : base(id, name, countdownTime, seconds, true, "Effettua più uccisioni possibile con un veicolo di terra", PlayerStats.HighestSkittles)
        {
        }

        public override void OnEventActivated()
        {
            FirstStartedTick = true;
            base.OnEventActivated();
            Client.Instance.AddTick(OnTick);
        }

        public override void ResetEvent()
        {
            base.ResetEvent();
            Client.Instance.AddTick(OnTick);
        }
        private async Task OnTick()
        {
            try
            {
                if (!IsActive) { return; }

                if (!IsStarted)
                    Screen.ShowSubtitle($"Trova un veicolo e preparati per la sfida ~b~{Name}~w~.", 50);
                else
                {
                    Screen.ShowSubtitle("Effettua il maggior numero di uccisioni su di un veicolo.", 50);
                    // aggiungere controllo se il ped è player?
                    if (Cache.PlayerCache.MyPlayer.Ped.IsInVehicle())
                    {
                        var x = 0;
                        StatGetInt(unchecked((uint)PlayerStat), ref x, -1);
                        CurrentAttempt = x;
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

    }
}
