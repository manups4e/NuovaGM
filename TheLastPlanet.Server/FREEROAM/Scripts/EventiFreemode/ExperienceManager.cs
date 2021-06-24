using CitizenFX.Core;
using Logger;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode
{
    public class ExperienceManager
    {
        public static void OnAddExperience(ClientId client, int experiencePoints)
        {
            try
            {
                var leveledUp = false;
                var currentLevel = (BucketsHandler.FreeRoam.Bucket as FreeRoamBucket).GetCurrentLevel(client);
                var currentRankLimit = Experience.RankRequirement.Where(x => x.Key == currentLevel).First().Value;
                var nextRankLimit = Experience.NextLevelExperiencePoints(currentLevel);
                var currentXp = (BucketsHandler.FreeRoam.Bucket as FreeRoamBucket).GetCurrentExperiencePoints(client);

                (BucketsHandler.FreeRoam.Bucket as FreeRoamBucket).AddExperience(client, experiencePoints);

                var updatedLevel = (BucketsHandler.FreeRoam.Bucket as FreeRoamBucket).GetCurrentLevel(client);
                var updatedXp = (BucketsHandler.FreeRoam.Bucket as FreeRoamBucket).GetCurrentExperiencePoints(client);
                var updatedCurrentRankLimit = currentRankLimit;
                var updatedNextRankLimit = nextRankLimit;

                if (currentLevel != updatedLevel)
                {
                    updatedCurrentRankLimit = Experience.RankRequirement.Where(x => x.Key == updatedLevel).First().Value;
                    updatedNextRankLimit = Experience.NextLevelExperiencePoints(updatedLevel);

                    // assume you can only level up once
                    leveledUp = true;
                }

                Server.Instance.Events.Send(client, "worldEventsManage.Client.UpdateExperience", currentRankLimit, nextRankLimit, updatedCurrentRankLimit, updatedNextRankLimit, currentXp, updatedXp, currentLevel, updatedLevel, leveledUp);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }
    }
}
