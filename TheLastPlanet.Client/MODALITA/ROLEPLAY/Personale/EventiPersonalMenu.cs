using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core.Status;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Personale
{
    internal static class EventiPersonalMenu
    {
        public static bool WindowsGiu = false;
        public static bool MostraStatus = true;
        public static bool MostraSoldi = true;
        public static Vehicle saveVehicle = null;
        public static float CinematicaHeight = 0;
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

            if (Main.ImpostazioniClient.LetterBox > 0f)
            {
                DrawRect(0.5f, Main.ImpostazioniClient.LetterBox / 1000 / 2, 1f, Main.ImpostazioniClient.LetterBox / 1000, 0, 0, 0, 255);
                DrawRect(0.5f, 1 - Main.ImpostazioniClient.LetterBox / 1000 / 2, 1f, Main.ImpostazioniClient.LetterBox / 1000, 0, 0, 0, 255);
            }

            await Task.FromResult(0);
        }

        public static void Portiere(string portiera)
        {
            int port = 0;

            switch (portiera)
            {
                case "Anteriore Sinistra":
                    port = 0;

                    break;
                case "Anteriore Destra":
                    port = 1;

                    break;
                case "Posteriore Sinistra":
                    port = 2;

                    break;
                case "Posteriore Destra":
                    port = 3;

                    break;
                case "Cofano":
                    port = 4;

                    break;
                case "Bagagliaio":
                    port = 5;

                    break;
            }

            Vehicle vehicle = saveVehicle;

            if (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo)
            {
                Vehicle veh = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle;

                if (veh.Doors.HasDoor((VehicleDoorIndex)port))
                {
                    if (veh.Doors[(VehicleDoorIndex)port].IsOpen)
                    {
                        veh.Doors[(VehicleDoorIndex)port].Close();
                        if (portiera == "Cofano" || portiera == "Bagagliaio")
                            HUD.ShowNotification("Hai chiuso il ~y~ " + portiera + "~w~.", NotificationColor.Cyan);
                        else
                            HUD.ShowNotification("Hai chiuso la portiera~y~ " + portiera + "~w~.", NotificationColor.Cyan);
                    }
                    else
                    {
                        veh.Doors[(VehicleDoorIndex)port].Open();
                        if (portiera == "Cofano" || portiera == "Bagagliaio")
                            HUD.ShowNotification("Hai aperto il ~y~ " + portiera + "~w~.", NotificationColor.Cyan);
                        else
                            HUD.ShowNotification("Hai aperto la portiera~y~ " + portiera + "~w~.", NotificationColor.Cyan);
                    }
                }
                else
                {
                    if (portiera == "Cofano" || portiera == "Bagagliaio")
                        HUD.ShowNotification("Questo veicolo non ha il ~b~ " + portiera + "~w~!", NotificationColor.Red, true);
                    else
                        HUD.ShowNotification("Questo veicolo non ha la portiera ~b~ " + portiera + "~w~!", NotificationColor.Red, true);
                }
            }
            else
            {
                if (vehicle == null || !vehicle.Exists()) return;
                float distanceToVeh = Vector3.Distance(Cache.PlayerCache.MyPlayer.Posizione.ToVector3, vehicle.Position);

                if (distanceToVeh <= 20f)
                {
                    if (vehicle.Doors.HasDoor((VehicleDoorIndex)port))
                    {
                        if (vehicle.Doors[(VehicleDoorIndex)port].IsOpen)
                        {
                            vehicle.Doors[(VehicleDoorIndex)port].Close();
                            if (portiera == "Cofano" || portiera == "Bagagliaio")
                                HUD.ShowNotification("Hai chiuso il ~y~ " + portiera + "~w~.", NotificationColor.Cyan);
                            else
                                HUD.ShowNotification("Hai chiuso la portiera~y~ " + portiera + "~w~.", NotificationColor.Cyan);
                        }
                        else
                        {
                            vehicle.Doors[(VehicleDoorIndex)port].Open();
                            HUD.ShowNotification("Hai aperto la portiera~y~ " + portiera + "~w~.", NotificationColor.Cyan);
                        }
                    }
                    else
                    {
                        if (portiera == "Cofano" || portiera == "Bagagliaio")
                            HUD.ShowNotification("Questo veicolo non ha il ~b~ " + portiera + "~w~!", NotificationColor.Red, true);
                        else
                            HUD.ShowNotification("Questo veicolo non ha la portiera ~b~ " + portiera + "~w~!", NotificationColor.Red, true);
                    }
                }
                else
                {
                    HUD.ShowNotification("Sei troppo distante dal tuo veicolo salvato!", NotificationColor.Red, true);
                }
            }
        }

        private static bool window0up = true;
        private static bool window1up = true;
        private static bool window2up = true;
        private static bool window3up = true;

        public static void Finestrini(string finestrini)
        {
            if (!Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo) return;
            if (Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Driver != Cache.PlayerCache.MyPlayer.Ped) return;

            switch (finestrini)
            {
                case "Anteriore Sinistro" when window1up:
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.FrontLeftWindow].RollDown();
                    window1up = false;
                    WindowsGiu = true;

                    break;
                case "Anteriore Sinistro":
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.FrontLeftWindow].RollUp();
                    window1up = true;
                    WindowsGiu = false;

                    break;
                case "Anteriore Destro" when window0up:
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.FrontRightWindow].RollDown();
                    window0up = false;
                    WindowsGiu = true;

                    break;
                case "Anteriore Destro":
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.FrontRightWindow].RollUp();
                    window0up = true;
                    WindowsGiu = false;

                    break;
                case "Posteriore Sinistro" when window3up:
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.BackLeftWindow].RollDown();
                    window3up = false;
                    WindowsGiu = true;

                    break;
                case "Posteriore Sinistro":
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.BackLeftWindow].RollUp();
                    window3up = true;
                    WindowsGiu = false;

                    break;
                case "Posteriore Destro" when window2up:
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.BackRightWindow].RollDown();
                    window2up = false;
                    WindowsGiu = true;

                    break;
                case "Posteriore Destro":
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Windows[VehicleWindowIndex.BackRightWindow].RollUp();
                    window2up = true;
                    WindowsGiu = false;

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
                HUD.ShowNotification("Veicolo salvato ~r~rimosso~w~ attento se ti allontani verrà eliminato.", NotificationColor.Blue);
                PersonalMenu.salvato = true;
            }
            else
            {
                saveVehicle = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle;
                saveVehicle.AttachBlip();
                saveVehicle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
                saveVehicle.AttachedBlip.Color = BlipColor.Green;
                HUD.ShowNotification("Questa ~y~" + saveVehicle.LocalizedName + "~w~ è stata~g~ salvata ~w~e non verrà eliminata se ti allontani.", NotificationColor.GreenDark);
                PersonalMenu.salvato = true;
            }
        }

        public static async void motore(bool toggle)
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
                float distanceToVeh = Vector3.Distance(Cache.PlayerCache.MyPlayer.Posizione.ToVector3, vehicle.Position);

                if (toggle)
                {
                    if (vehicle.Exists())
                    {
                        if (vehicle.Driver == Cache.PlayerCache.MyPlayer.Ped)
                        {
                            vehicle.LockStatus = VehicleLockStatus.Locked;
                            SetVehicleDoorsLockedForAllPlayers(vehicle.Handle, true);
                            HUD.ShowNotification("Hai chiuso la tua ~y~" + vehicle.LocalizedName + "~w~.", NotificationColor.Cyan, true);
                            PlayVehicleDoorCloseSound(vehicle.Handle, 1);
                            PersonalMenu.chiuso = true;
                        }
                        else if (distanceToVeh <= 20f)
                        {
                            if (islocked == VehicleLockStatus.Unlocked)
                            {
                                vehicle.LockStatus = VehicleLockStatus.Locked;
                                SetVehicleDoorsLockedForAllPlayers(vehicle.Handle, true);
                                HUD.ShowNotification("Hai chiuso la tua ~y~" + vehicle.LocalizedName + "~w~.", NotificationColor.Cyan, true);
                                await LockLightsAsync(vehicle);
                                PlayVehicleDoorCloseSound(vehicle.Handle, 1);
                                PersonalMenu.chiuso = true;
                            }
                            else
                            {
                                HUD.ShowNotification("Veicolo già chiuso.", NotificationColor.Red);
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("Devi essere entro 20mt dal tuo veicolo per chiuderlo.", NotificationColor.Red);
                        }
                    }
                    else
                    {
                        HUD.ShowNotification("Non hai un veicolo salvato!", NotificationColor.Red);
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
                            HUD.ShowNotification("Hai aperto la tua ~y~" + vehicle.LocalizedName + "~w~.", NotificationColor.Cyan, true);
                            PlayVehicleDoorOpenSound(vehicle.Handle, 0);
                            PersonalMenu.chiuso = false;
                        }
                        else if (distanceToVeh <= 20f)
                        {
                            if (islocked == VehicleLockStatus.Locked)
                            {
                                vehicle.LockStatus = VehicleLockStatus.Unlocked;
                                SetVehicleDoorsLockedForAllPlayers(vehicle.Handle, false);
                                HUD.ShowNotification("Hai aperto la tua ~y~" + vehicle.LocalizedName + "~w~.", NotificationColor.Cyan, true);
                                await LockLightsAsync(vehicle);
                                PlayVehicleDoorOpenSound(vehicle.Handle, 0);
                                PersonalMenu.chiuso = false;
                            }
                            else
                            {
                                HUD.ShowNotification("Veicolo già aperto.", NotificationColor.Red);
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("Devi essere entro 20mt dal tuo veicolo per sbloccarlo.", NotificationColor.Red);
                        }
                    }
                    else
                    {
                        HUD.ShowNotification("Non hai un veicolo salvato!", NotificationColor.Red);
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

        public static async Task MostramiStatus()
        {
            if (!HUD.MenuPool.IsAnyMenuOpen && !IsHelpMessageBeingDisplayed() && !Main.ImpostazioniClient.ModCinema)
                if (!IsPedRunningMobilePhoneTask(PlayerPedId()) && Main.spawned && MostraStatus)
                {
                    if (Input.IsControlPressed(Control.FrontendRight, PadCheck.Controller) && !Input.IsControlPressed(Control.FrontendLb, PadCheck.Controller) || Input.IsControlPressed(Control.SelectCharacterFranklin, PadCheck.Keyboard) && (!Game.IsPaused || IsPedStill(PlayerPedId()) || Cache.PlayerCache.MyPlayer.Ped.IsWalking || Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo && !Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsEngineRunning))
                    {
                        Game.DisableControlThisFrame(2, Control.FrontendLeft);
                        if (StatsNeeds.Needs["Fame"].GetPercent() > 30f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "FAME = ~y~" + Math.Round(StatsNeeds.Needs["Fame"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Fame"].GetPercent() > 60f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "FAME = ~o~" + Math.Round(StatsNeeds.Needs["Fame"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Fame"].GetPercent() > 90f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "FAME = ~r~" + Math.Round(StatsNeeds.Needs["Fame"].GetPercent(), 2) + "%");
                        else
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "FAME = ~g~" + Math.Round(StatsNeeds.Needs["Fame"].GetPercent(), 2) + "%");
                        if (StatsNeeds.Needs["Sete"].GetPercent() > 30f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "SETE = ~y~" + Math.Round(StatsNeeds.Needs["Sete"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Sete"].GetPercent() > 60f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "SETE = ~o~" + Math.Round(StatsNeeds.Needs["Sete"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Sete"].GetPercent() > 90f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "SETE = ~r~" + Math.Round(StatsNeeds.Needs["Sete"].GetPercent(), 2) + "%");
                        else
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "SETE = ~g~" + Math.Round(StatsNeeds.Needs["Sete"].GetPercent(), 2) + "%");
                        if (StatsNeeds.Needs["Stanchezza"].GetPercent() > 30f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "STANCH. = ~y~" + Math.Round(StatsNeeds.Needs["Stanchezza"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Stanchezza"].GetPercent() > 60f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "STANCH. = ~o~" + Math.Round(StatsNeeds.Needs["Stanchezza"].GetPercent(), 2) + "%");
                        else if (StatsNeeds.Needs["Stanchezza"].GetPercent() > 90f)
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "STANCH. = ~r~" + Math.Round(StatsNeeds.Needs["Stanchezza"].GetPercent(), 2) + "%");
                        else
                            HUD.DrawText3D(Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)).ToPosition(), Color.FromArgb(255, 255, 255, 255), "STANCH. = ~g~" + Math.Round(StatsNeeds.Needs["Stanchezza"].GetPercent(), 2) + "%");
                    }

                    if (Game.IsControlJustPressed(1, Control.VehicleDuck) && Game.CurrentInputMode == InputMode.MouseAndKeyboard) Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();
                }

            await Task.FromResult(0);
        }
    }
}