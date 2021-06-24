using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core.Buckets
{
	public class FreeRoamBucket : Bucket
	{
		public FreeRoamBucket(int id, string name) : base(id, name) { }

		public override void AddPlayer(ClientId client)
		{
			base.AddPlayer(client);
			var highscores = new List<PlayerScore>();
			foreach (var worldEvent in WorldEventsManager.WorldEvents)
				highscores.Add(new PlayerScore { EventId = worldEvent.Id, BestAttempt = 0, CurrentAttempt = 0, EventXpMultiplier = worldEvent.EventXpMultiplier });
			client.User.PlayerScores = highscores;
		}

        public void UpdateCurrentAttempt(ClientId client, int eventId, float currentAttempt)
        {
            try
            {
                var player = Players.Where(x => x.Id == client.Id).FirstOrDefault();
                if (player != null)
                {
                    var data = player.User.PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
                    if (data != null)
                    {
                        data.CurrentAttempt = currentAttempt;
                        if (currentAttempt > data.BestAttempt)
                            data.BestAttempt = currentAttempt;
                    }
                    else
                        Server.Logger.Warning($"Data for Event {eventId} does not exist for Player {client.Player.Name}");
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public void ResetAllCurrentAttempts(int eventId)
        {
            try
            {
                foreach (var player in Players)
                    player.User.PlayerScores.First(x => x.EventId == eventId).CurrentAttempt = 0;
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public void UpdateBestAttempt(ClientId client, int eventId, int bestAttempt)
        {
            try
            {
                var player = Players.Where(x => x.Id == client.Id).FirstOrDefault();
                if (player != null)
                {
                    var score = player.User.PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
                    if (score.BestAttempt < bestAttempt)
                        score.BestAttempt = bestAttempt;
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }


        public void SendFinalTop3Players(int eventId, float eventMultiplier)
        {
            try
            {
                if (Players.Count == 0) { return; }

                var tempDictionary = new Dictionary<string, float>();

                foreach (var player in Players)
                {
                    var score = player.User.PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
                    if (score != null)
                    {
                        var xpGain = (int)Math.Min(score.CurrentAttempt * eventMultiplier, Experience.RankRequirement[player.User.FreeRoamChar.Level + 1] - Experience.RankRequirement[player.User.FreeRoamChar.Level]);

                        if (xpGain != 0)
                            ExperienceManager.OnAddExperience(player, xpGain);

                        tempDictionary.Add(player.Player.Name, score.CurrentAttempt);
                    }
                }

                if (tempDictionary.Count == 0)
                {
                    tempDictionary.Add("-1", 0);
                    tempDictionary.Add("-2", 0);
                    tempDictionary.Add("-3", 0);
                }
                else if (tempDictionary.Count == 1)
                {
                    tempDictionary.Add("-1", 0);
                    tempDictionary.Add("-2", 0);
                }
                else if (tempDictionary.Count == 2)
                {
                    tempDictionary.Add("-1", 0);
                }

                tempDictionary.OrderByDescending(x => x.Value);

                var newerDict = tempDictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                Server.Instance.Events.Send(Players, "worldEventsManage.Client:FinalTop3", eventId, newerDict.ToJson());
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public void SendTop3Players(int eventId)
        {
            try
            {
                if (Players.Count == 0) { return; } // should never happen but :shrug:

                var tempDictionary = new Dictionary<string, float>();

                foreach (var player in Players)
                {
                    var score = player.User.PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
                    if (score != null)
                    {
                        if(!tempDictionary.ContainsKey(player.Player.Name))
                            tempDictionary.Add(player.Player.Name, score.CurrentAttempt);
                    }
                }

                if (tempDictionary.Count == 1)
                {
                    tempDictionary.Add("Giocatore 1", 0);
                    tempDictionary.Add("Giocatore 2", 0);
                }
                else if (tempDictionary.Count == 2)
                {
                    tempDictionary.Add("Player 1", 0);
                }

                var newDict = tempDictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                if (newDict.Count < 3) { return; }

                Server.Instance.Events.Send(Players, "worldEventsManage.Client:GetTop3", newDict.ToJson());
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public void SendEventData(int eventId)
        {
            try
            {
                foreach (var p in Players)
                {
                    if (p != null)
                    {
                        var eventData = p.User.PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
                        p.Player.TriggerEvent("worldeventsManager.Client:GetEventData", eventData.EventId, eventData.CurrentAttempt, eventData.BestAttempt);
                    }
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public int GetCurrentLevel(ClientId client)
        {
            try
            {
                var player = Players.Where(x => x.Identifiers.License == client.Identifiers.License).FirstOrDefault();

                if (player != null)
                {
                    return player.User.FreeRoamChar.Level;
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }

            return -1;
        }

        public int GetCurrentExperiencePoints(ClientId client)
        {
            try
            {
                var player = Players.Where(x => x.Identifiers.License == client.Identifiers.License).FirstOrDefault();

                if (player != null)
                {
                    return player.User.FreeRoamChar.TotalXp;
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }

            return 0;
        }

        public void AddExperience(ClientId client, int experiencePoints)
        {
            try
            {
                var player = Players.Where(x => x.Identifiers.License == client.Identifiers.License).FirstOrDefault();
                if (player != null)
                {
                    var nextLevelTotalXp = Experience.NextLevelExperiencePoints(player.User.FreeRoamChar.Level);

                    if (player.User.FreeRoamChar.TotalXp + experiencePoints >= nextLevelTotalXp)
                    {
                        int remainder = player.User.FreeRoamChar.TotalXp + experiencePoints - nextLevelTotalXp;

                        player.User.FreeRoamChar.Level++;

                        if (remainder > 0)
                            AddExperience(client, remainder);
                    }
                    else
                        player.User.FreeRoamChar.TotalXp += experiencePoints;
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public async void PlayerExit(ClientId client)
        {
            try
            {
                if (!Players.Contains(client)) return;
                await Players.ForEachAsync(async (x) => { x.User.showNotification($"Il player {x.Player.Name} è uscito."); await Task.FromResult(0); });
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

    }
}
