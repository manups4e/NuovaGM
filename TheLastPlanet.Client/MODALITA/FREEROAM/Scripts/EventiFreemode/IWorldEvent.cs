using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.NativeUI;

namespace TheLastPlanet.Client.FreeRoam.Scripts.EventiFreemode
{
    public abstract class IWorldEvent
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ChallengeStartedLabelText { get; private set; }
        public string StatUnit { get; private set; }
        public PlayerStats PlayerStat { get; private set; }
        public PlayerStatType PlayerStatType { get; private set; }
        public bool IsActive { get; set; }
        public bool IsStarted { get; set; }
        public bool CountdownStarted { get; set; }
        public double EventTime { get; set; } = 300;
        public double EventCountdownTime { get; set; } = 90;
        public TimeSpan CountdownTime { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        public TextTimerBar CountdownTimerBar = new TextTimerBar("Nuovo Evento", "");
        public TextTimerBar TimeRemainingTimerBar = new TextTimerBar("Fine evento", "");
        public TextTimerBar CurrentAttemptTimerBar = new TextTimerBar("Tentativo attuale", "0");
        public TextTimerBar YourBestTimerBar = new TextTimerBar("Il tuo meglio", "0");
        public TextTimerBar FirstPlaceTimerBar = new TextTimerBar("~y~1°: Player 1", "~y~0");
        public TextTimerBar SecondPlaceTimerBar = new TextTimerBar("~c~2°: Player 2", "~c~0");
        public TextTimerBar ThirdPlaceTimerBar = new TextTimerBar("~o~3°: Player 3", "~o~0");
        public TextTimerBar EventNameTimerBar = new TextTimerBar("Nome Evento:", "");

        public virtual Dictionary<Vector4, VehicleHash> VehicleSpawnLocations { get; set; }

        public List<TimerBarBase> TimerBars = new List<TimerBarBase>();
        public TimerBarPool TimerBarPool = new TimerBarPool();

        public float CurrentAttempt = 0;
        public float BestAttempt = 0;
        public bool FirstStartedTick = true;

        public IWorldEvent(int id, string name, double countdownTime, double eventTime, bool customLabel, string challengeStartedLabel, PlayerStats playerStat, string statUnit = "", PlayerStatType playerStatType = PlayerStatType.Int, bool isTimeEvent = false)
        {
            Id = id;
            Name = name;

            if (!customLabel)
                ChallengeStartedLabelText = GetLabelText(challengeStartedLabel);
            else
                ChallengeStartedLabelText = challengeStartedLabel;

            PlayerStat = playerStat;
            PlayerStatType = playerStatType;
            StatUnit = statUnit;
            IsActive = false;
            IsStarted = false;
            EventTime = eventTime;
            EventCountdownTime = countdownTime;
            CountdownTime = TimeSpan.FromSeconds(EventCountdownTime);
            TimeRemaining = TimeSpan.FromSeconds(EventTime);

            Client.Instance.AddEventHandler("OnClientResourceStart", new Action<string>(OnClientResourceStart));

            Client.Logger.Info($"Added Event [{name}]");
        }

        public virtual void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName)
            {
                return;
            }
        }

        public virtual void OnEventActivated()
        {
            if (PlayerStatType == PlayerStatType.Int)
                StatSetInt((uint)PlayerStat, 0, true);
            else if (PlayerStatType == PlayerStatType.Float)
                StatSetFloat((uint)PlayerStat, 0f, true);

            Client.Logger.Info($"{Name} Event Activated");
        }

        public void Activate(bool active)
        {
            Client.Logger.Info($"{(active ? "Activated" : "Deactivated")} Event {Name}.");
            IsActive = active;

            if (IsActive)
            {
                OnEventActivated();
                Screen.LoadingPrompt.Hide();
                Client.Instance.AddTick(OnWorldEventTick);
                Client.Instance.AddTick(OnDrawUiTick);

                ActivateEventTimerBars();

                CountdownTimerBar.Text = CountdownTime.ToString(@"mm\:ss");
                TimeRemainingTimerBar.Text = TimeRemaining.ToString(@"mm\:ss");
                EventNameTimerBar.Label = Name;
            }
            else
            {
                Client.Instance.RemoveTick(OnWorldEventTick);
                Client.Instance.RemoveTick(OnDrawUiTick);
                ResetEvent();
            }
        }

        private async Task OnWorldEventTick()
        {
            try
            {
                if (!IsActive) { return; }
                if (Screen.LoadingPrompt.IsActive) { Screen.LoadingPrompt.Hide(); }
                if (IsStarted)
                {
                    await BaseScript.Delay(1000);
                    if (TimeRemaining == TimeSpan.Zero)
                    {
                        Client.Instance.Events.Send("worldEventsManage.Server:EventEnded", Id, CurrentAttempt, BestAttempt);
                        await BaseScript.Delay(5000);
                        ResetEvent();
                        return;
                    }

                    TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(1));

                    if (TimeRemaining.TotalSeconds < 10 && !CountdownStarted)
                    {
                        Game.PlaySound("10s", "MP_MISSION_COUNTDOWN_SOUNDSET");
                        CountdownStarted = true;
                    }

                    var x = 0;
                    switch (PlayerStatType)
                    {
                        case PlayerStatType.Int:
                            StatGetInt(unchecked((uint)PlayerStat), ref x, 1);
                            CurrentAttempt = x;
                            break;
                        case PlayerStatType.Float:
                            var f = 0f;
                            StatGetFloat(unchecked((uint)PlayerStat), ref f, 1);
                            CurrentAttempt = f;
                            break;
                        default:
                            StatGetInt(unchecked((uint)PlayerStat), ref x, 1);
                            break;
                    }

                    if (CurrentAttempt > BestAttempt)
                    {
                        BestAttempt = CurrentAttempt;
                    }

                    Client.Instance.Events.Send("worldEventsManage.Server:UpdateCurrentEvent", Id, CurrentAttempt);

                    CurrentAttemptTimerBar.Text = Math.Round(CurrentAttempt, 2).ToString() + " " + StatUnit;
                    YourBestTimerBar.Text = Math.Round(BestAttempt, 2).ToString() + " " + StatUnit;

                    TimeRemainingTimerBar.Text = $"{(TimeRemaining.TotalSeconds > 10 ? "~s~" : "~r~")} {TimeRemaining.ToString(@"mm\:ss")}";
                    return;
                }

                await BaseScript.Delay(1000);
                if (CountdownTime == TimeSpan.Zero)
                {
                    StartEventTimerBars();
                    MediumMessageBase.MessageInstance.ShowColoredShard("Sfida Iniziata", ChallengeStartedLabelText, HudColor.HUD_COLOUR_PURPLE, true);
                    Audio.PlaySoundFrontend("FLIGHT_SCHOOL_LESSON_PASSED", "HUD_AWARDS");
                    IsStarted = true;
                    CountdownStarted = false;
                    return;
                }

                CountdownTime = CountdownTime.Subtract(TimeSpan.FromSeconds(1));

                if (CountdownTime.TotalSeconds < 6 && !CountdownStarted)
                {
                    Game.PlaySound("5s_To_Event_Start_Countdown", "GTAO_FM_Events_Soundset");
                    CountdownStarted = true;
                }

                CountdownTimerBar.Text = $"{(CountdownTime.TotalSeconds > 5 ? "~s~" : "~r~")} {CountdownTime:mm\\:ss}";
            }
            catch (Exception ex)
            {
                Client.Logger.Error(ex.ToString());
            }
        }

        private async Task OnDrawUiTick()
        {
            try
            {
                if (!IsActive) { return; }

                if (TimerBars.Count != 0)
                    TimerBarPool.Draw();
            }
            catch (Exception ex)
            {
                Client.Logger.Error(ex.ToString());
            }

            await Task.FromResult(0);
        }

        public void StartEventTimerBars()
        {
            StatSetInt((uint)PlayerStat, 0, true);
            ActivateEventTimerBars(false);
        }

        public void ActivateEventTimerBars(bool isCountdown = true)
        {
            foreach (var bar in TimerBars)
            {
                TimerBarPool.Remove(bar);
            }

            TimerBars.Clear();

            if (isCountdown)
            {
                TimerBarPool.Add(CountdownTimerBar);
                TimerBars.Add(CountdownTimerBar);
            }
            else
            {
                TimerBarPool.Add(TimeRemainingTimerBar);
                TimerBars.Add(TimeRemainingTimerBar);

                TimerBarPool.Add(YourBestTimerBar);
                TimerBarPool.Add(CurrentAttemptTimerBar);

                TimerBarPool.Add(ThirdPlaceTimerBar);
                TimerBarPool.Add(SecondPlaceTimerBar);
                TimerBarPool.Add(FirstPlaceTimerBar);

                TimerBarPool.Add(EventNameTimerBar);

                TimerBars.Add(YourBestTimerBar);
                TimerBars.Add(CurrentAttemptTimerBar);

                TimerBars.Add(ThirdPlaceTimerBar);
                TimerBars.Add(SecondPlaceTimerBar);
                TimerBars.Add(FirstPlaceTimerBar);

                TimerBars.Add(EventNameTimerBar);
            }
        }

        public virtual void ResetEvent()
        {
            foreach (var bar in TimerBars)
            {
                TimerBarPool.Remove(bar);
            }

            CountdownTime = TimeSpan.FromSeconds(EventCountdownTime);
            TimeRemaining = TimeSpan.FromSeconds(EventTime);
            CurrentAttemptTimerBar.Text = "0" + StatUnit;
            FirstPlaceTimerBar.Text = "~y~0" + StatUnit;
            SecondPlaceTimerBar.Text = "~c~0" + StatUnit;
            ThirdPlaceTimerBar.Text = "~o~0" + StatUnit;
            TimerBars.Clear();

            IsActive = false;
            IsStarted = false;
            CountdownStarted = false;

            if (PlayerStatType == PlayerStatType.Int)
                StatSetInt((uint)PlayerStat, 0, true);
            else if (PlayerStatType == PlayerStatType.Float)
                StatSetFloat((uint)PlayerStat, 0f, true);

            CurrentAttempt = 0;
        }
    }
}
