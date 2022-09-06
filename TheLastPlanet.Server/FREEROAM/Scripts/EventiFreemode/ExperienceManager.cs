using System;
using System.Linq;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Shared;


namespace TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode
{
    public class ExperienceManager
    {
        public static void OnAddExperience(PlayerClient client, int experiencePoints)
        {
            try
            {
                var leveledUp = false;
                var currentLevel = BucketsHandler.FreeRoam.GetCurrentLevel(client);
                var currentRankLimit = Experience.RankRequirement.Where(x => x.Key == currentLevel).First().Value;
                var nextRankLimit = Experience.NextLevelExperiencePoints(currentLevel);
                var currentXp = BucketsHandler.FreeRoam.GetCurrentExperiencePoints(client);

                BucketsHandler.FreeRoam.AddExperience(client, experiencePoints);

                var updatedLevel = BucketsHandler.FreeRoam.GetCurrentLevel(client);
                var updatedXp = BucketsHandler.FreeRoam.GetCurrentExperiencePoints(client);
                var updatedCurrentRankLimit = currentRankLimit;
                var updatedNextRankLimit = nextRankLimit;

                if (currentLevel != updatedLevel)
                {
                    updatedCurrentRankLimit = Experience.RankRequirement.Where(x => x.Key == updatedLevel).First().Value;
                    updatedNextRankLimit = Experience.NextLevelExperiencePoints(updatedLevel);

                    // assume you can only level up once
                    leveledUp = true;
                }

                EventDispatcher.Send(client, "worldEventsManage.Client.UpdateExperience", currentRankLimit, nextRankLimit, updatedCurrentRankLimit, updatedNextRankLimit, currentXp, updatedXp, currentLevel, updatedLevel, leveledUp);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }
    }
}
