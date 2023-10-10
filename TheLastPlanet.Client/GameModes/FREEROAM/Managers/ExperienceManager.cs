using System;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.FREEROAM.Managers
{
    public static class ExperienceManager
    {
        public static void Init()
        {
            AccessingEvents.OnFreeRoamSpawn += (client) => EventDispatcher.Mount("worldEventsManage.Client.UpdateExperience", new Action<int, int, int, int, int, int, int, int, bool>(OnUpdateExperience));
            AccessingEvents.OnFreeRoamLeave += (client) => EventDispatcher.Unmount("worldEventsManage.Client.UpdateExperience");
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
                    BaseScript.TriggerEvent("worldeventsManage.Client:UpdatedLevel", updatedLevel, true); // TO BE UPDATED AS IT DOESN'T EXIST AT THE MOMENT

                    await HUD.ShowPlayerRankScoreAfterUpdate(currentRankLimit, nextRankLimit, currentXp, nextRankLimit, currentLevel);
                    await BaseScript.Delay(2000);
                    await HUD.ShowPlayerRankScoreAfterUpdate(updatedCurrentRankLimit, updatedNextRankLimit, 0, updatedXp - currentXp, updatedLevel);
                }
                EventDispatcher.Send("tlg:freeroam:salvapersonaggio");
            }
            catch (Exception e)
            {
                Client.Logger.Error(e.ToString());
            }

            await Task.FromResult(0);
        }
    }
}
