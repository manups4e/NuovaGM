using CitizenFX.Core;
using Logger;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheLastPlanet.Server.Core;

namespace TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode
{
    public class ExperienceManager
    {
        public static void Init()
        {
            Server.Instance.AddEventHandler("worldEventsManage.Internal:AddExperience", new Action<string, int>(OnAddExperience));
        }

        private static void OnAddExperience(string identifier, int experiencePoints)
        {
            try
            {
                var user = Funzioni.GetUserFromPlayerId(identifier);
                var leveledUp = false;
                var currentLevel = user.FreeRoamChar.Level;
                var currentRankLimit = Experience.RankRequirement.Where(x => x.Key == currentLevel).First().Value;
                var nextRankLimit = Experience.NextLevelExperiencePoints(currentLevel);
                var currentXp = user.FreeRoamChar.TotalXp;

                user.AddExperience(experiencePoints);

                var updatedLevel = user.FreeRoamChar.Level;
                var updatedXp = user.FreeRoamChar.TotalXp;
                var updatedCurrentRankLimit = currentRankLimit;
                var updatedNextRankLimit = nextRankLimit;

                if (currentLevel != updatedLevel)
                {
                    updatedCurrentRankLimit = Experience.RankRequirement.Where(x => x.Key == updatedLevel).First().Value;
                    updatedNextRankLimit = Experience.NextLevelExperiencePoints(updatedLevel);

                    // assume you can only level up once
                    leveledUp = true;
                }

                foreach (var playerino in Server.Instance.GetPlayers)
                {
                    if (playerino.Identifiers["license"] == identifier)
                    {
                        playerino.TriggerEvent("worldEventsManage.Client.UpdateExperience", currentRankLimit, nextRankLimit, updatedCurrentRankLimit, updatedNextRankLimit, currentXp, updatedXp, currentLevel, updatedLevel, leveledUp);
                    }
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }
    }
}
