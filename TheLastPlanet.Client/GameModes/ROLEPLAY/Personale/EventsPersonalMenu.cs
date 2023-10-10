using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Core.Status;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Personale
{
    internal static class EventsPersonalMenu
    {
        public static bool WindowsDown = false;
        public static bool ShowStatus = true;
        public static bool ShowMoney = true;
        public static Vehicle saveVehicle = null;
        public static float CinematicHeight = 0;
        /*				if (value)
					Client.Instance.AddTick(CinematicMode);
				else
					Client.Instance.RemoveTick(CinematicMode);
*/
        private static List<HudComponent> hideComponents = new List<HudComponent>()
        {
            HudComponent.WantedStars,
            HudComponent.WeaponIcon,
            HudComponent.Cash,
            HudComponent.MpCash,
            HudComponent.MpMessage,
            HudComponent.VehicleName,
            HudComponent.AreaName,
            HudComponent.Unused,
            HudComponent.StreetName,
            HudComponent.HelpText,
            HudComponent.FloatingHelpText1,
            HudComponent.FloatingHelpText2,
            HudComponent.CashChange,
            HudComponent.Reticle,
            HudComponent.SubtitleText,
            HudComponent.RadioStationsWheel,
            HudComponent.Saving,
            HudComponent.GamingStreamUnusde,
			//HudComponent.WeaponWheel, // forse per questo non potevo cambiare arma???
			HudComponent.WeaponWheelStats
        };

        public static async Task CinematicMode()
        {
            hideComponents.ForEach(c => Screen.Hud.HideComponentThisFrame(c));

            if (Main.ClientConfig.LetterBox > 0f)
            {
                DrawRect(0.5f, Main.ClientConfig.LetterBox / 1000 / 2, 1f, Main.ClientConfig.LetterBox / 1000, 0, 0, 0, 255);
                DrawRect(0.5f, 1 - Main.ClientConfig.LetterBox / 1000 / 2, 1f, Main.ClientConfig.LetterBox / 1000, 0, 0, 0, 255);
            }

            await Task.FromResult(0);
        }

        public static void VehDorrs(string door)
        {
            int port = 0;

            switch (door)
            {
                case "Anteriore Sinistra":
                    port = 0;

                    break;
                case "Anteriore Right":
                    port = 1;

                    break;
                case "Rear Left":
                    port = 2;

                    break;
                case "Rear Right":
                    port = 3;

                    break;
                case "Hood":
                    port = 4;

                    break;
                case "Trunk":
                    port = 5;

                    break;
            }

            Vehicle vehicle = saveVehicle;

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
            {
                Vehicle veh = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle;

                if (veh.Doors.HasDoor((VehicleDoorIndex)port))
                {
                    if (veh.Doors[(VehicleDoorIndex)port].IsOpen)
                    {
                        veh.Doors[(VehicleDoorIndex)port].Close();
                        HUD.ShowNotification("You closed the ~y~ " + door + "~w~.", ColoreNotifica.Cyan);
                    }
                    else
                    {
                        veh.Doors[(VehicleDoorIndex)port].Open();
                        HUD.ShowNotification("You opened the ~y~ " + door + "~w~.", ColoreNotifica.Cyan);
                    }
                }
                else
                {
                    HUD.ShowNotification("This vehicle doens't have a ~b~ " + door + "~w~!", ColoreNotifica.Red, true);
                }
            }
            else
            {
                if (vehicle == null || !vehicle.Exists()) return;
                float distanceToVeh = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, vehicle.Position);

                if (distanceToVeh <= 20f)
                {
                    if (vehicle.Doors.HasDoor((VehicleDoorIndex)port))
                    {
                        if (vehicle.Doors[(VehicleDoorIndex)port].IsOpen)
                        {
                            vehicle.Doors[(VehicleDoorIndex)port].Close();
                            if (door == "Hood" || door == "Trunk")
                                HUD.ShowNotification("You closed the ~y~" + door + "~w~ door.", ColoreNotifica.Cyan);
                        }
                        else
                        {
                            vehicle.Doors[(VehicleDoorIndex)port].Open();
                            HUD.ShowNotification("You opened~y~" + door + "~w~ door.", ColoreNotifica.Cyan);
                        }
                    }
                    else
                    {
                        if (door == "Hood" || door == "Trunk")
                            HUD.ShowNotification("this vehicles doesn't have a ~b~ " + door + "~w~!", ColoreNotifica.Red, true);
                        else
                            HUD.ShowNotification("this vehicle doesn't have a ~b~ " + door + "~w~ door!", ColoreNotifica.Red, true);
                    }
                }
                else
                {
                    HUD.ShowNotification("You're too far from your vehicle!", ColoreNotifica.Red, true);
                }
            }
        }

        private static bool window0up = true;
        private static bool window1up = true;
        private static bool window2up = true;
        private static bool window3up = true;

        public static void Windows(string finestrini)
        {
            if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle) return;
            if (Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Driver != Cache.PlayerCache.MyPlayer.Ped) return;

            switch (finestrini)
            {
                case "Front Left" when window1up:
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.FrontLeftWindow].RollDown();
                    window1up = false;
                    WindowsDown = true;

                    break;
                case "Front Left":
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.FrontLeftWindow].RollUp();
                    window1up = true;
                    WindowsDown = false;

                    break;
                case "Front Right" when window0up:
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.FrontRightWindow].RollDown();
                    window0up = false;
                    WindowsDown = true;

                    break;
                case "Front Right":
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.FrontRightWindow].RollUp();
                    window0up = true;
                    WindowsDown = false;

                    break;
                case "Rear Left" when window3up:
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.BackLeftWindow].RollDown();
                    window3up = false;
                    WindowsDown = true;

                    break;
                case "Rear Left":
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.BackLeftWindow].RollUp();
                    window3up = true;
                    WindowsDown = false;

                    break;
                case "Right Rear" when window2up:
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.BackRightWindow].RollDown();
                    window2up = false;
                    WindowsDown = true;

                    break;
                case "Right Rear":
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.BackRightWindow].RollUp();
                    window2up = true;
                    WindowsDown = false;

                    break;
            }
        }

        public static void Save(bool saved)
        {
            if (!Cache.PlayerCache.MyPlayer.Ped.IsSittingInVehicle() || Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Driver != Cache.PlayerCache.MyPlayer.Ped) return;

            if (!saved)
            {
                saveVehicle.AttachedBlip.Delete();
                saveVehicle = null;
                HUD.ShowNotification("Saved vehicle ~r~removed~w~ be careful if you get too far it will be towed.", ColoreNotifica.Blue);
                PersonalMenu.saved = true;
            }
            else
            {
                saveVehicle = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle;
                saveVehicle.AttachBlip();
                saveVehicle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
                saveVehicle.AttachedBlip.Color = BlipColor.Green;
                HUD.ShowNotification("This ~y~" + saveVehicle.LocalizedName + "~w~ has been ~g~daved~w~ and won't be deleted if you get too far from it.", ColoreNotifica.GreenDark);
                PersonalMenu.saved = true;
            }
        }

        public static async void engine(bool toggle)
        {
            Vehicle vehicle = saveVehicle;
            vehicle.IsEngineRunning = toggle;
            vehicle.IsDriveable = toggle;
        }

        public static async void Lock(bool toggle)
        {
            try
            {
                Vehicle vehicle = saveVehicle;
                VehicleLockStatus islocked = vehicle.LockStatus;
                float distanceToVeh = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, vehicle.Position);

                if (toggle)
                {
                    if (vehicle.Exists())
                    {
                        if (vehicle.Driver == Cache.PlayerCache.MyPlayer.Ped)
                        {
                            vehicle.LockStatus = VehicleLockStatus.Locked;
                            SetVehicleDoorsLockedForAllPlayers(vehicle.Handle, true);
                            HUD.ShowNotification("You locked your ~y~" + vehicle.LocalizedName + "~w~.", ColoreNotifica.Cyan, true);
                            PlayVehicleDoorCloseSound(vehicle.Handle, 1);
                            PersonalMenu.closed = true;
                        }
                        else if (distanceToVeh <= 20f)
                        {
                            if (islocked == VehicleLockStatus.Unlocked)
                            {
                                vehicle.LockStatus = VehicleLockStatus.Locked;
                                SetVehicleDoorsLockedForAllPlayers(vehicle.Handle, true);
                                HUD.ShowNotification("You locked your ~y~" + vehicle.LocalizedName + "~w~.", ColoreNotifica.Cyan, true);
                                await LockLightsAsync(vehicle);
                                PlayVehicleDoorCloseSound(vehicle.Handle, 1);
                                PersonalMenu.closed = true;
                            }
                            else
                            {
                                HUD.ShowNotification("Already locked.", ColoreNotifica.Red);
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("You must be near your vehicle to lock it.", ColoreNotifica.Red);
                        }
                    }
                    else
                    {
                        HUD.ShowNotification("No saved vehicle!", ColoreNotifica.Red);
                    }
                }
                else
                {
                    if (vehicle.Exists())
                    {
                        if (vehicle.Driver == Cache.PlayerCache.MyPlayer.Ped)
                        {
                            vehicle.LockStatus = VehicleLockStatus.Unlocked;
                            SetVehicleDoorsLockedForAllPlayers(vehicle.Handle, false);
                            HUD.ShowNotification("You unlocked ~y~" + vehicle.LocalizedName + "~w~.", ColoreNotifica.Cyan, true);
                            PlayVehicleDoorOpenSound(vehicle.Handle, 0);
                            PersonalMenu.closed = false;
                        }
                        else if (distanceToVeh <= 20f)
                        {
                            if (islocked == VehicleLockStatus.Locked)
                            {
                                vehicle.LockStatus = VehicleLockStatus.Unlocked;
                                SetVehicleDoorsLockedForAllPlayers(vehicle.Handle, false);
                                HUD.ShowNotification("You unlocked ~y~" + vehicle.LocalizedName + "~w~.", ColoreNotifica.Cyan, true);
                                await LockLightsAsync(vehicle);
                                PlayVehicleDoorOpenSound(vehicle.Handle, 0);
                                PersonalMenu.closed = false;
                            }
                            else
                            {
                                HUD.ShowNotification("Already unlocked.", ColoreNotifica.Red);
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("You must be near your vehicle to unlock it.", ColoreNotifica.Red);
                        }
                    }
                    else
                    {
                        HUD.ShowNotification("No saved vehicle!", ColoreNotifica.Red);
                    }
                }
            }
            catch
            {
                HUD.ShowNotification("Devi prima salvare un veicolo!!");
            }
        }

        public static async Task LockLightsAsync(Vehicle vehicle)
        {
            //vehicle.SoundHorn(200);
            vehicle.IsLeftIndicatorLightOn = true;
            vehicle.IsRightIndicatorLightOn = true;
            await BaseScript.Delay(500);
            //vehicle.SoundHorn(200);
            vehicle.IsLeftIndicatorLightOn = false;
            vehicle.IsRightIndicatorLightOn = false;
            await BaseScript.Delay(500);
            vehicle.IsLeftIndicatorLightOn = true;
            vehicle.IsRightIndicatorLightOn = true;
            await BaseScript.Delay(400);
            vehicle.IsLeftIndicatorLightOn = false;
            vehicle.IsRightIndicatorLightOn = false;
        }

        public static async Task ShowMeStatus()
        {
            if (!MenuHandler.IsAnyMenuOpen && !IsHelpMessageBeingDisplayed() && !Main.ClientConfig.CinemaMode)
                if (!IsPedRunningMobilePhoneTask(PlayerPedId()) && Main.spawned && ShowStatus)
                {
                    if (Input.IsControlPressed(Control.FrontendRight, PadCheck.Controller) && !Input.IsControlPressed(Control.FrontendLb, PadCheck.Controller) || Input.IsControlPressed(Control.SelectCharacterFranklin, PadCheck.Keyboard) && (!Game.IsPaused || IsPedStill(PlayerPedId()) || Cache.PlayerCache.MyPlayer.Ped.IsWalking || Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle && !Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsEngineRunning))
                    {
                        Game.DisableControlThisFrame(2, Control.FrontendLeft);
                        if (StatsNeeds.Needs["Hunger"].GetPercent() > 30f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "HUNGER = ~y~" + Math.Round(StatsNeeds.Needs["Hunger"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Hunger"].GetPercent() > 60f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "HUNGER = ~o~" + Math.Round(StatsNeeds.Needs["Hunger"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Hunger"].GetPercent() > 90f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "HUNGER = ~r~" + Math.Round(StatsNeeds.Needs["Hunger"].GetPercent(), 2) + "%");
                        else
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "HUNGER = ~g~" + Math.Round(StatsNeeds.Needs["Hunger"].GetPercent(), 2) + "%");
                        if (StatsNeeds.Needs["Thirst"].GetPercent() > 30f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "THIRST = ~y~" + Math.Round(StatsNeeds.Needs["Thirst"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Thirst"].GetPercent() > 60f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "THIRST = ~o~" + Math.Round(StatsNeeds.Needs["Thirst"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Thirst"].GetPercent() > 90f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "THIRST = ~r~" + Math.Round(StatsNeeds.Needs["Thirst"].GetPercent(), 2) + "%");
                        else
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "THIRST = ~g~" + Math.Round(StatsNeeds.Needs["Thirst"].GetPercent(), 2) + "%");
                        if (StatsNeeds.Needs["Tireness"].GetPercent() > 30f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "TIRENESS. = ~y~" + Math.Round(StatsNeeds.Needs["Tireness"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Tireness"].GetPercent() > 60f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "TIRENESS. = ~o~" + Math.Round(StatsNeeds.Needs["Tireness"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Tireness"].GetPercent() > 90f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "TIRENESS. = ~r~" + Math.Round(StatsNeeds.Needs["Tireness"].GetPercent(), 2) + "%");
                        else
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "TIRENESS. = ~g~" + Math.Round(StatsNeeds.Needs["Tireness"].GetPercent(), 2) + "%");
                    }

                    if (Game.IsControlJustPressed(1, Control.VehicleDuck) && Game.CurrentInputMode == InputMode.MouseAndKeyboard) Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();
                }

            await Task.FromResult(0);
        }
    }
}