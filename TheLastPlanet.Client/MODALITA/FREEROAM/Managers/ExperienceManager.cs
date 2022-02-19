using System;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.FREEROAM.Spawner;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Managers
{
    public static class ExperienceManager
    {
        public static void Init()
        {
            AccessingEvents.OnFreeRoamSpawn += (client) => Client.Instance.Events.Mount("worldEventsManage.Client.UpdateExperience", new Action<int, int, int, int, int, int, int, int, bool>(OnUpdateExperience));
            AccessingEvents.OnFreeRoamLeave += (client) => Client.Instance.Events.Unmount("worldEventsManage.Client.UpdateExperience");
        }

        private static async void OnUpdateExperience(int currentRankLimit, int nextRankLimit, int updatedCurrentRankLimit, int updatedNextRankLimit, int currentXp, int updatedXp, int currentLevel, int updatedLevel, bool leveledUp)
        {
            try
            {
                Cache.PlayerCache.MyPlayer.User.FreeRoamChar.Level = updatedLevel;
                Cache.PlayerCache.MyPlayer.User.FreeRoamChar.TotalXp = updatedXp;

                if (!leveledUp)
                {
                    await HUD.ShowPlayerRankScoreAfterUpdate(currentRankLimit, nextRankLimit, currentXp, updatedXp, currentLevel);
                }
                else
                {
                    BaseScript.TriggerEvent("worldeventsManage.Client:UpdatedLevel", updatedLevel, true); // da aggiornare perché non esiste nel codice

                    await HUD.ShowPlayerRankScoreAfterUpdate(currentRankLimit, nextRankLimit, currentXp, nextRankLimit, currentLevel);
                    await BaseScript.Delay(2000);
                    await HUD.ShowPlayerRankScoreAfterUpdate(updatedCurrentRankLimit, updatedNextRankLimit, 0, updatedXp - currentXp, updatedLevel);
                }
                Client.Instance.Events.Send("tlg:freeroam:salvapersonaggio");
            }
            catch (Exception e)
            {
                Client.Logger.Error(e.ToString());
            }

            await Task.FromResult(0);
        }
    }
}
