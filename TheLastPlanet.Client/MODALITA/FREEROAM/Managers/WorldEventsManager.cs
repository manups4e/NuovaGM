using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using Logger;
using TheLastPlanet.Shared;
using ScaleformUI;
using TheLastPlanet.Client.MODALITA.FREEROAM.Scripts.EventiFreemode;
using TheLastPlanet.Client.MODALITA.FREEROAM.Spawner;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Managers
{
    static class WorldEventsManager
    {
        private static readonly List<IWorldEvent> WorldEvents = new();

        public static IWorldEvent ActiveWorldEvent;
        public static IWorldEvent NextWorldEvent;

        private static int JoinWaitTime = 0;
        private static int DownTime = 0;
        public static bool WarningMessageDisplayed = false;
        private static bool FirstSpawn = true;

        public static void Init() => FreeRoamLogin.OnPlayerJoined += FreeRoamLogin_OnPlayerJoined;
        private static async void FreeRoamLogin_OnPlayerJoined()
        {
            //Client.Instance.AddTick(OnDefaultTick);
            //Client.Instance.AddTick(OnDiscordTick);
            Client.Instance.AddTick(OnWaitTick);
            Client.Instance.AddTick(SaveMe);

            Client.Instance.Events.Mount("worldEventsManage.Client:EventActivate", new Action<int, int>(OnActivateEvent));
            Client.Instance.Events.Mount("worldeventsManager.Client:GetEventData", new Action<int, float, float>(OnGetEventData));
            Client.Instance.Events.Mount("worldEventsManage.Client:NextEventIn", new Action<int>(OnNextEventIn));
            Client.Instance.Events.Mount("worldEventsManage.Client:GetTop3", new Action<string>(OnGetTop3));
            Client.Instance.Events.Mount("worldEventsManage.Client:FinalTop3", new Action<int, string>(OnGetFinalTop3));
            Client.Instance.Events.Mount("worldEventsManage.Client:PeriodicSync", new Action<int, bool>(OnPeriodicSync));
            Client.Instance.Events.Mount("tlg:freeroam:showLoading", new Action<int, string, int>(ShowDialog));

            WorldEvents.Add(new NumberOfNearMisses(1, "Schivate mortali", 60, 300));
            WorldEvents.Add(new FlyingUnderBridges(2, "Volando sotto i ponti", 90, 300));
            WorldEvents.Add(new FastestSpeedInCar(3, "Fast & Furious", 60, 270));
            WorldEvents.Add(new MostPistolHeadshots(4, "Il miglior Pistolero", 60, 300));
            WorldEvents.Add(new LongestSurvivedFreefall(5, "Caduta Libera", 60, 240));
            WorldEvents.Add(new HighestSkittles(6, "Auto-scontro", 60, 300));
            WorldEvents.Add(new FarthestJumpDistance(7, "Salto in lungo", 60, 270));
            WorldEvents.Add(new HighestJumpDistance(8, "Salto in alto", 60, 270));
            WorldEvents.Add(new KingOfTheCastle(9, "Re del Castello", 60, 300));
            var status = await Client.Instance.Events.Get<Tuple<int, int, int, int, bool>>("worldEventsManage.Server:GetStatus");
            OnGetStatus(status.Item1, status.Item2, status.Item3, status.Item4, status.Item5);
        }

        public static void Stop()
        {
            FreeRoamLogin.OnPlayerJoined -= FreeRoamLogin_OnPlayerJoined;
            Client.Instance.RemoveTick(OnWaitTick);
            Client.Instance.RemoveTick(SaveMe);

            Client.Instance.Events.Unmount("worldEventsManage.Client:EventActivate");
            Client.Instance.Events.Unmount("worldeventsManager.Client:GetEventData");
            Client.Instance.Events.Unmount("worldEventsManage.Client:NextEventIn");
            Client.Instance.Events.Unmount("worldEventsManage.Client:GetTop3");
            Client.Instance.Events.Unmount("worldEventsManage.Client:FinalTop3");
            Client.Instance.Events.Unmount("worldEventsManage.Client:PeriodicSync");
            Client.Instance.Events.Unmount("tlg:freeroam:showLoading");
            WorldEvents.Clear();
            ActiveWorldEvent.ResetEvent();
        }

        public static async Task SaveMe()
        {
            await BaseScript.Delay(600000); // 600000
            Client.Instance.Events.Send("tlg:freeroam:SaveMe");
            if (!NativeUIScaleform.InstructionalButtons.IsSaving)
                NativeUIScaleform.InstructionalButtons.AddSavingText(LoadingSpinnerType.SocialClubSaving, "Sincronizzazione", 5000);

        }

        public static void ShowDialog(int type, string txt, int time)
        {
            if (!NativeUIScaleform.InstructionalButtons.IsSaving)
                NativeUIScaleform.InstructionalButtons.AddSavingText((LoadingSpinnerType)type, txt, time);
        }

        private static async Task OnWaitTick()
        {
            try
            {
                await BaseScript.Delay(1000);

                if(JoinWaitTime != 0)
                    JoinWaitTime--;

                if (DownTime != 0)
                    DownTime--;
            }
            catch (Exception e)
            {
                Client.Logger.Error(e.ToString());
            }

            await Task.FromResult(0);
        }

        private static void OnPeriodicSync(int eventTime, bool isStarted)
        {
            if (ActiveWorldEvent == null) { return; }

            if (isStarted)
            {
                if(ActiveWorldEvent.TimeRemaining.TotalSeconds != eventTime)
                    ActiveWorldEvent.TimeRemaining = TimeSpan.FromSeconds(eventTime);
            }
            else
            {
                if(ActiveWorldEvent.CountdownTime.TotalSeconds != eventTime)
                    ActiveWorldEvent.CountdownTime = TimeSpan.FromSeconds(eventTime);
            }
        }

        private static void OnGetEventData(int eventId, float currentAttempt, float bestAttempt)
        {
            if (ActiveWorldEvent == null){ return; }

            ActiveWorldEvent.CurrentAttempt = currentAttempt;
            ActiveWorldEvent.BestAttempt = bestAttempt;
        }

        private static async void OnGetFinalTop3(int eventId, string json)
        {
            try
            {
                Client.Logger.Debug("Primi 3  = " + json);
                var top3 = JsonConvert.DeserializeObject<Dictionary<string, float>>(json);

                var xp = 0;
                var place = 0;

                // Winner
                if (top3.ElementAt(0).Key == Cache.PlayerCache.MyPlayer.Player.Name)
                {
                    if (top3.ElementAt(0).Value != 0)
                    {
                        xp = Convert.ToInt32(top3.ElementAt(0).Value * 1.75);
                        place = 1;
                    }
                }
                // 2nd Place
                else if (top3.ElementAt(1).Key == Cache.PlayerCache.MyPlayer.Player.Name)
                {
                    if (top3.ElementAt(1).Value != 0)
                    {
                        xp = Convert.ToInt32(top3.ElementAt(1).Value * 1.5);
                        place = 2;
                    }
                }
                // 3rd Place
                else if (top3.ElementAt(2).Key == Cache.PlayerCache.MyPlayer.Player.Name)
                {
                    if (top3.ElementAt(2).Value != 0)
                    {
                        xp = Convert.ToInt32(top3.ElementAt(2).Value * 1.25);
                        place = 3;
                    }
                }
                // Out of top 3
                else
                {
                    place = 0;
                }

                var title = "Fine";
                var score = "";
                if (ActiveWorldEvent.PlayerStatType == PlayerStatType.Int)
                    score = Math.Round(ActiveWorldEvent.CurrentAttempt, 0).ToString();
                else if (ActiveWorldEvent.PlayerStatType == PlayerStatType.Float)
                    score = Math.Round(ActiveWorldEvent.CurrentAttempt, 2).ToString();

                var description = $"{top3.ElementAt(0).Key} ha vinto la sfida {ActiveWorldEvent.Name} con un punteggio di {top3.ElementAt(2).Value}{ActiveWorldEvent.StatUnit}";
                if (top3.All(x => x.Key.StartsWith("-")))
                    description = $"Nessuno ha partecipato alla sfida, non ci sono vincitori!";
                switch (place)
                {
                    case 1:
                        title = "Vincitore";
                        description = $"Sei il vincitore della Sfida {ActiveWorldEvent.Name} con un punteggio di {score}{ActiveWorldEvent.StatUnit}!";
                        Audio.PlaySoundFrontend("FIRST_PLACE", "HUD_MINI_GAME_SOUNDSET");
                        break;
                    case 2:
                        title = "Secondo";
                        description = $"Così vicino!!, Eppure così lontano... Sei arrivato secondo nella Sfida {ActiveWorldEvent.Name} con un punteggio di {score}{ActiveWorldEvent.StatUnit}";
                        Audio.PlaySoundFrontend("RACE_PLACED", "HUD_AWARDS");
                        break;
                    case 3:
                        title = "Terzo";
                        description = $"Sei arrivato terzo nella Sfida {ActiveWorldEvent.Name} con un punteggio di {score}{ActiveWorldEvent.StatUnit}";
                        Audio.PlaySoundFrontend("RACE_PLACED", "HUD_AWARDS");
                        break;
                    default:
                        Audio.PlaySoundFrontend("RACE_PLACED", "HUD_AWARDS");
                        break;
                }

                NativeUIScaleform.MedMessageInstance.ShowColoredShard(title, description, HudColor.HUD_COLOUR_PURPLE, true, false, 7500);
            }
            catch (Exception e)
            {
                Client.Logger.Error(e.ToString());
            }

            await Task.FromResult(0);
        }

        private static void OnGetTop3(string top3)
        {
            try
            {
                if (ActiveWorldEvent == null)
                {
                    return;
                }

                var top3Players = JsonConvert.DeserializeObject<Dictionary<string, float>>(top3);

                if (top3Players.Count < 3) { return; }

                ActiveWorldEvent.FirstPlaceTimerBar.Label = $"~y~1°: {top3Players.ElementAt(0).Key}";
                ActiveWorldEvent.FirstPlaceTimerBar.Caption = $"~y~{Math.Round(top3Players.ElementAt(0).Value, 2)} {ActiveWorldEvent.StatUnit}";
                ActiveWorldEvent.SecondPlaceTimerBar.Label = $"~c~2°: {top3Players.ElementAt(1).Key}";
                ActiveWorldEvent.SecondPlaceTimerBar.Caption = $"~c~{Math.Round(top3Players.ElementAt(1).Value, 2)} {ActiveWorldEvent.StatUnit}";
                ActiveWorldEvent.ThirdPlaceTimerBar.Label = $"~o~3°: {top3Players.ElementAt(2).Key}";
                ActiveWorldEvent.ThirdPlaceTimerBar.Caption = $"~o~{Math.Round(top3Players.ElementAt(2).Value, 2)} {ActiveWorldEvent.StatUnit}";
            }
            catch (Exception e)
            {
                Client.Logger.Error(e.ToString());
            }
        }

        private static void OnActivateEvent(int currentId, int nextId)
        {
            JoinWaitTime = 0;
            ActiveWorldEvent = WorldEvents.Where(x => x.Id == currentId).FirstOrDefault();
            NextWorldEvent = WorldEvents.Where(x => x.Id == nextId).FirstOrDefault();

            if (ActiveWorldEvent != null)
                ActiveWorldEvent.Activate(true);
        }

        private static void OnNextEventIn(int seconds)
        {
            DownTime = seconds;

            if (DownTime > 0 && DownTime < 60) { Screen.LoadingPrompt.Show("Preparazione prossimo evento"); }
        }


        private static void OnGetStatus(int currentId, int nextId, int downTime, int joinWaitTime, bool isStarted)
        {
            ActiveWorldEvent = WorldEvents.Where(x => x.Id == currentId).FirstOrDefault();
            NextWorldEvent = WorldEvents.Where(x => x.Id == nextId).FirstOrDefault();

            DownTime = downTime;
            JoinWaitTime = joinWaitTime;

            if (DownTime > 0 && DownTime < 60)
            {
                Screen.LoadingPrompt.Show("Preparazione prossimo evento");
                return;
            }

            if (FirstSpawn)
            {
                ActiveWorldEvent.ResetEvent();

                if (JoinWaitTime > 0)
                {
                    ActiveWorldEvent.Activate(true);
                    ActiveWorldEvent.IsStarted = isStarted;

                    if (ActiveWorldEvent.IsStarted)
                    {
                        ActiveWorldEvent.ActivateEventTimerBars(false);
                        ActiveWorldEvent.TimeRemaining = TimeSpan.FromSeconds(JoinWaitTime);
                        ActiveWorldEvent.CountdownStarted = false;
                    }
                    else
                    {
                        ActiveWorldEvent.ActivateEventTimerBars(true);
                        ActiveWorldEvent.CountdownTime = TimeSpan.FromSeconds(JoinWaitTime);
                        ActiveWorldEvent.CountdownStarted = true;
                    }
                }
                FirstSpawn = false;
            }
            else
            {
                ActiveWorldEvent.IsStarted = isStarted;

                if (ActiveWorldEvent.IsStarted)
                    ActiveWorldEvent.TimeRemaining = TimeSpan.FromSeconds(JoinWaitTime);
                else
                    ActiveWorldEvent.CountdownTime = TimeSpan.FromSeconds(JoinWaitTime);
            }
        }
    }
}
