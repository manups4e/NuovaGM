using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Scripts.EventiFreemode
{
    public class NumberOfNearMisses : IWorldEvent
    {
        public NumberOfNearMisses(int id, string name, double countdownTime, double seconds) : base(id, name, countdownTime, seconds, false, "AMCH_8", PlayerStats.NumberNearMissesNoCrash)
        {
            Client.Instance.AddEventHandler("DamageEvents:VehicleDamaged", new Action<int, int, uint, bool, int>(VehicleDamaged));
        }

        public override void OnEventActivated()
        {
            API.StatSetInt(unchecked((uint)PlayerStats.NumberNearMisses), 0, true);
            API.StatSetInt(unchecked((uint)PlayerStats.NumberNearMissesNoCrash), 0, true);
            Client.Instance.AddTick(OnTick);
            base.OnEventActivated();
        }

        private void VehicleDamaged(int vehicle, int attacker, uint weaponHash, bool isMeleeDamage, int vehicleDamageTypeFlag)
        {
            if (IsStarted) 
            {   
                Vehicle veh = new(vehicle);
                if (veh == Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle)
                {
                    API.StatSetInt(unchecked((uint)PlayerStats.NumberNearMisses), 0, true);
                    CurrentAttempt = 0;
                }
            }
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
                    Screen.ShowSubtitle("Effettua il maggior numero di schivate con gli altri veicoli usando un veicolo di terra.", 50);
                    var x = 0;
                    var p = 0;
                    API.StatGetInt(unchecked((uint)PlayerStats.NumberNearMisses), ref x, -1);
                    if(x != 0)
                        CurrentAttempt = x;
                    API.StatGetInt(unchecked((uint)PlayerStats.NumberNearMissesNoCrash), ref p, -1);
                    BestAttempt = p;
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
            base.ResetEvent();
            Client.Instance.RemoveTick(OnTick);
        }
    }
}
