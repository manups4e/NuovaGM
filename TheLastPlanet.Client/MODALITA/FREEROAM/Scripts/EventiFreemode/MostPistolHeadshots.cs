using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.FreeRoam.Scripts.EventiFreemode
{
    class MostPistolHeadshots : IWorldEvent
    {
        public MostPistolHeadshots(int id, string name, double countdownTime, double seconds) : base(id, name, countdownTime, seconds, true, "Effettua più colpi alla testa possibile con una pistola", PlayerStats.MostPistolHeadshots)
        {
        }
        public override void OnEventActivated()
        {
            FirstStartedTick = true;
            Cache.PlayerCache.MyPlayer.Ped.Weapons.RemoveAll();
            Cache.PlayerCache.MyPlayer.Ped.Weapons.Give(WeaponHash.Pistol, 255, true, false);
            HUD.ShowAdvancedNotification($"Ti è stata data una pistola per la sfida ~b~{Name}~w~", "Arma equipaggiata", "", "CHAR_AMMUNATION", "CHAR_AMMUNATION", HudColor.HUD_COLOUR_REDDARK, default,false, NotificationType.Bubble);
            Client.Instance.AddTick(OnTick);
            base.OnEventActivated();
        }

        public override void ResetEvent()
        {
            Cache.PlayerCache.MyPlayer.Player.WantedLevel = 0;
            Client.Instance.RemoveTick(OnTick);
            base.ResetEvent();
        }
        private async Task OnTick()
        {
            try
            {
                if (!IsActive) { return; }

                if (!IsStarted)
                    Screen.ShowSubtitle($"Preparati per la sfida ~b~{Name}~w~ usando solo la tua Pistola.", 50);
                else
                {
                    Screen.ShowSubtitle("Effettua più colpi alla testa che puoi con la tua Pistola.", 50);
                    var x = 0;
                    API.StatGetInt(unchecked((uint)PlayerStat), ref x, -1);
                    CurrentAttempt = x;
                    if (CurrentAttempt > BestAttempt)
                        BestAttempt = CurrentAttempt;
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

