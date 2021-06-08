using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Server.Core;

namespace TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode
{
	static class WorldEventsManager
	{
        public static readonly List<WorldEvent> WorldEvents = new List<WorldEvent>
        {
            new WorldEvent{ Id = 1, Name = "Schivate mortali", CountdownTime = TimeSpan.FromSeconds(60), EventTime = TimeSpan.FromSeconds(300), EventXpMultiplier = 13.5f },
            new WorldEvent{ Id = 2, Name = "Volando sotto i ponti", CountdownTime = TimeSpan.FromSeconds(90), EventTime = TimeSpan.FromSeconds(300), EventXpMultiplier = 35.25f },
            new WorldEvent{ Id = 3, Name = "Fast & Furious", CountdownTime = TimeSpan.FromSeconds(60), EventTime = TimeSpan.FromSeconds(270), EventXpMultiplier = 1.5f},
            new WorldEvent{ Id = 4, Name = "Il miglior Pistolero", CountdownTime = TimeSpan.FromSeconds(60), EventTime = TimeSpan.FromSeconds(300), EventXpMultiplier = 20.5f},
            new WorldEvent{ Id = 5, Name = "Caduta Libera", CountdownTime = TimeSpan.FromSeconds(60), EventTime = TimeSpan.FromSeconds(240), EventXpMultiplier = 100.5f},
            new WorldEvent{ Id = 6, Name = "Auto-scontro", CountdownTime = TimeSpan.FromSeconds(60), EventTime = TimeSpan.FromSeconds(300), EventXpMultiplier = 13.5f},
            new WorldEvent{ Id = 7, Name = "Salto in lungo", CountdownTime = TimeSpan.FromSeconds(60), EventTime = TimeSpan.FromSeconds(270), EventXpMultiplier = 20.5f},
            new WorldEvent{ Id = 8, Name = "Salto in alto", CountdownTime = TimeSpan.FromSeconds(60), EventTime = TimeSpan.FromSeconds(270), EventXpMultiplier = 25.5f},
            new WorldEvent{ Id = 9, Name = "Re del Castello", CountdownTime = TimeSpan.FromSeconds(60), EventTime = TimeSpan.FromSeconds(300), EventXpMultiplier = 25.5f},
        };

        private static readonly int SaveInterval = 5;

        private static WorldEvent CurrentEvent;
        private static WorldEvent NextEvent;
        private static Random rnd = new Random();
        private static TimeSpan TimeUntilNextEvent = TimeSpan.FromSeconds(5);//TimeSpan.FromMinutes(rnd.Next(20, 30));

        private static bool IsAnyEventActive = false;

        public static void Init()
        {
            Server.Instance.AddEventHandler("worldEventsManage.Server:AddParticipant", new Action<Player>(OnAddParticipant));
            Server.Instance.AddEventHandler("worldEventsManage.Server:EventEnded", new Action<Player, int, int, int>(OnEventEnded));
            Server.Instance.AddEventHandler("worldEventsManage.Server:UpdateCurrentEvent", new Action<Player, int, float>(OnUpdateCurrentEvent));
            Server.Instance.AddEventHandler("worldEventsManage.Server:GetStatus", new Action<Player>(OnGetStatus));

            Server.Instance.AddTick(OnPeriodicTick);
            Server.Instance.AddTick(OnEventTick);
            ChooseNextEvent();
        }

        private static async void OnUpdateCurrentEvent([FromSource]Player player, int eventId, float currentAttempt)
        {
            if (CurrentEvent == null) { return; }
            if (CurrentEvent.Id != eventId) { return; }

            while (Funzioni.GetUserFromPlayerId(player.Handle) == null) await BaseScript.Delay(0);
            Funzioni.GetUserFromPlayerId(player.Handle).UpdateCurrentAttempt(eventId, currentAttempt);
        }

        private static async Task OnPeriodicTick()
        {
            try
            {
                await BaseScript.Delay(1000);
                if (CurrentEvent.CountdownTime > TimeSpan.Zero)
                {
                    BaseScript.TriggerClientEvent("worldEventsManage.Client:PeriodicSync", (int)CurrentEvent.CountdownTime.TotalSeconds, false);
                    return;
                }
                // RIMUOVERE SE SI TORNA SOTTO CON I 1000 ANZICHE' 100
                BaseScript.TriggerClientEvent("worldEventsManage.Client:PeriodicSync", (int)CurrentEvent.EventTime.TotalSeconds, true);
            }
            catch (Exception e)
            {
                Server.Logger.Error($"{e.Source}.{e.TargetSite}()");
            }

            await Task.FromResult(0);
        }

        private static async Task OnEventTick()
        {
            try
            {
                if (!IsAnyEventActive)
                {
                    await BaseScript.Delay(1000);

                    TimeUntilNextEvent = TimeUntilNextEvent.Subtract(TimeSpan.FromSeconds(1));

                    if (TimeUntilNextEvent == TimeSpan.Zero)
                    {
                        NextEvent.IsActive = true;
                        NextEvent.IsStarted = false;
                        IsAnyEventActive = NextEvent.IsActive;
                        CurrentEvent = NextEvent;
                        foreach (var p in Server.Instance.GetPlayers)
                        {
                            var player = Funzioni.GetUserFromPlayerId(p.Handle);
                            if (player != null)
                            {
                                WorldEvents.ForEach(x => player.PlayerScores.Add(new PlayerScore { EventId = x.Id, BestAttempt = 0, CurrentAttempt = 0, EventXpMultiplier = x.EventXpMultiplier }));
                                var eventData = player.PlayerScores.Where(x => x.EventId == CurrentEvent.Id).FirstOrDefault();
                                p.TriggerEvent("worldeventsManager.Client:GetEventData", eventData.EventId, eventData.CurrentAttempt, eventData.BestAttempt);
                            }
                        }
                        ChooseNextEvent();

                        Server.Logger.Info($"Current Event [{CurrentEvent.Name}] | Next Event [{NextEvent.Name}]");
                        BaseScript.TriggerClientEvent("worldEventsManage.Client:EventActivate", CurrentEvent.Id, NextEvent.Id);
                    }
                }
                else
                {
                    if (!CurrentEvent.IsStarted)
                    {
                        await BaseScript.Delay(1000);
                        CurrentEvent.CountdownTime = CurrentEvent.CountdownTime.Subtract(TimeSpan.FromSeconds(1));
                        if (CurrentEvent.CountdownTime == TimeSpan.Zero)
                        {
                            CurrentEvent.IsStarted = true;
                            BaseScript.TriggerClientEvent("worldEventsManage.Client:EventStart", CurrentEvent.Id);
                        }
                    }
                    else
                    {
                        if (CurrentEvent.EventTime == TimeSpan.Zero)
                        {
                            IsAnyEventActive = false;
                            await BaseScript.Delay(1500); // Delay to let everyone send in their results

                            var tempDictionary = new Dictionary<string, float>();
                            foreach (var player in Server.Instance.Clients)
                            {
                                var score = player.User.PlayerScores.Where(x => x.EventId == CurrentEvent.Id).FirstOrDefault();
                                if (score != null)
                                {
                                    foreach (var p in Server.Instance.GetPlayers)
                                    {
                                        if (p.Identifiers["license"] == player.Identifiers.License)
                                        {
                                            var xpGain = (int)Math.Min(score.CurrentAttempt * CurrentEvent.EventXpMultiplier, Experience.RankRequirement[player.User.FreeRoamChar.Level + 1] - Experience.RankRequirement[player.User.FreeRoamChar.Level]);

                                            if (xpGain != 0)
                                                BaseScript.TriggerEvent("worldEventsManage.Internal:AddExperience", player.Handle.ToString(), xpGain);
                                            tempDictionary.Add(p.Name, score.BestAttempt);
                                        }
                                    }
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
                                tempDictionary.Add("-1", 0);

                            tempDictionary.OrderByDescending(x => x.Value);

                            var newerDict = tempDictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                            BaseScript.TriggerClientEvent("worldEventsManage.Client:FinalTop3", CurrentEvent.Id, JsonConvert.SerializeObject(newerDict));

                            var cE = JsonConvert.SerializeObject(WorldEvents.Where(x => x.Id == CurrentEvent.Id).FirstOrDefault());
                            CurrentEvent = JsonConvert.DeserializeObject<WorldEvent>(cE); // reset this element. it's actually the next event
                            CurrentEvent.IsStarted = false;
                            CurrentEvent.IsActive = false;
                            TimeUntilNextEvent = TimeSpan.FromSeconds(5);//TimeSpan.FromMinutes(rnd.Next(20, 30));
                            await BaseScript.Delay(3500); // Wait until we start the next event (total 5 seconds)
                            BaseScript.TriggerClientEvent("worldEventsManage.Client:DestroyEventVehicles");
                            BaseScript.TriggerClientEvent("worldEventsManage.Client:NextEventIn", (int)TimeUntilNextEvent.TotalSeconds);
                        }
                        else
                        {
                            await BaseScript.Delay(1000);
                            CurrentEvent.EventTime = CurrentEvent.EventTime.Subtract(TimeSpan.FromSeconds(1));

                            var tempDictionary = new Dictionary<string, float>();
                            foreach (var player in Server.Instance.Clients)
                            {
                                if (player.User.status.Spawned)
                                {
                                    var score = player.User.PlayerScores.Where(x => x.EventId == CurrentEvent.Id).FirstOrDefault();
                                    if (score != null)
                                    {
                                        tempDictionary.Add(player.User.Player.Name, score.BestAttempt);
                                    }
                                }
                            }
                            if (tempDictionary.Count == 1)
                            {
                                tempDictionary.Add("Player 1", 0);
                                tempDictionary.Add("Player 2", 0);
                            }
                            else if (tempDictionary.Count == 2)
                            {
                                tempDictionary.Add("Player 1", 0);
                            }
                            var newDict = tempDictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                            if (newDict.Count < 3) { return; }
                            BaseScript.TriggerClientEvent("worldEventsManage.Client:GetTop3", JsonConvert.SerializeObject(newDict));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error($"{e}.{e.Source}.{e.TargetSite}()");
            }

            await Task.FromResult(0);
        }

        private static void OnEventEnded([FromSource]Player player, int eventId, int currentAttempt, int bestAttempt)
        {
            try
            {
                if (eventId != CurrentEvent.Id) { return; }


                var identifier = player.Identifiers["license"];

                var playerino = Server.Instance.Clients.Where(x => x.Identifiers.License == identifier).FirstOrDefault();
                if (playerino != null)
                {
                    var data = playerino.User.PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
                    if (data != null)
                    {
                        data.CurrentAttempt = currentAttempt;
                        if (currentAttempt > data.BestAttempt)
                            data.BestAttempt = currentAttempt;
                        if (data.BestAttempt < bestAttempt)
                            data.BestAttempt = bestAttempt;
                    }
                    playerino.User.PlayerScores.Clear();
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error($"{e.Source}.{e.TargetSite}()");
            }
        }

        private static void OnGetStatus([FromSource] Player player)
        {
            try
            {
                Server.Logger.Debug($"{player.Name} want status");
                var joinWaitTime = 0;
                var isStarted = CurrentEvent.IsStarted;
                if (!CurrentEvent.IsActive)
                {
                    player.TriggerEvent("worldEventsManage.Client:Status", CurrentEvent.Id, NextEvent.Id, (int)TimeUntilNextEvent.TotalSeconds, joinWaitTime, isStarted);

                    return;
                }

                if (CurrentEvent.IsStarted)
                    joinWaitTime = (int)CurrentEvent.EventTime.TotalSeconds;
                else
                    joinWaitTime = (int)CurrentEvent.CountdownTime.TotalSeconds;

                player.TriggerEvent("worldEventsManage.Client:Status", CurrentEvent.Id, NextEvent.Id, (int)TimeUntilNextEvent.TotalSeconds, joinWaitTime, isStarted);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        private static void OnAddParticipant([FromSource] Player player)
        {
            try
            {
                var identifier = player.Identifiers["license"];

                var xp = Funzioni.GetUserFromPlayerId(player.Handle).FreeRoamChar.TotalXp;
                var level = Funzioni.GetUserFromPlayerId(player.Handle).FreeRoamChar.Level;

                player.TriggerEvent("worldeventsManage.Client:GetLevelXp", level, xp);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        private static void ChooseNextEvent()
        {
            try
            {
                if (CurrentEvent == null)
                {
                    var cE = JsonConvert.SerializeObject(WorldEvents[7]/*WorldEvents.OrderBy(x => rnd.NextDouble()).First()*/);
                    CurrentEvent = JsonConvert.DeserializeObject<WorldEvent>(cE);
                }

                var nE = JsonConvert.SerializeObject(WorldEvents[7]/*WorldEvents.Where(x => x.Id != CurrentEvent.Id).OrderBy(x => rnd.NextDouble()).First()*/);
                NextEvent = JsonConvert.DeserializeObject<WorldEvent>(nE);
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

    }
}
