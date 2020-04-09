using CitizenFX.Core;
using CitizenFX.Core.UI;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Personale
{
	static class EventiPersonalMenu
	{
		public static bool WindowsGiu = false;
		public static bool MostraStatus = true;
		public static bool MostraSoldi = true;
		public static Vehicle saveVehicle = null;
		public static float CinematicaHeight = 0;
		public static bool DoHideHud = false;
		static List<HudComponent> hideComponents = new List<HudComponent>()
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

		static public async Task CinematicMode()
		{
			hideComponents.ForEach(c => Screen.Hud.HideComponentThisFrame(c));
			Screen.Hud.IsRadarVisible = false;
			DisplayRadar(false);
			if (CinematicaHeight > 0f)
			{
				DrawRect(0.5f, (CinematicaHeight / 1000) / 2, 1f, (CinematicaHeight / 1000), 0, 0, 0, 255);
				DrawRect(0.5f, 1 - (CinematicaHeight / 1000) / 2, 1f, (CinematicaHeight / 1000), 0, 0, 0, 255);
			}
			await Task.FromResult(0);
		}


		public static void Portiere(string portiera)
		{
			int port = 0;

			if (portiera == "Anteriore Sinistra")
			{
				port = 0;
			}
			else if (portiera == "Anteriore Destra")
			{
				port = 1;
			}
			else if (portiera == "Posteriore Sinistra")
			{
				port = 2;
			}
			else if (portiera == "Posteriore Destra")
			{
				port = 3;
			}
			else if (portiera == "Cofano")
			{
				port = 4;
			}
			else if (portiera == "Bagagliaio")
			{
				port = 5;
			}

			Vehicle vehicle = saveVehicle;
			if (Game.PlayerPed.IsInVehicle())
			{
				Vehicle veh = Game.PlayerPed.CurrentVehicle;
				if (veh.Doors.HasDoor((VehicleDoorIndex)(port)))
				{
					if (veh.Doors[(VehicleDoorIndex)(port)].IsOpen)
					{
						veh.Doors[(VehicleDoorIndex)(port)].Close();
						if (portiera == "Cofano" || portiera == "Bagagliaio")
						{
							HUD.ShowNotification("Hai chiuso il ~y~ " + portiera + "~w~.", NotificationColor.Cyan);
						}
						else
						{
							HUD.ShowNotification("Hai chiuso la portiera~y~ " + portiera + "~w~.", NotificationColor.Cyan);
						}
					}
					else
					{
						veh.Doors[(VehicleDoorIndex)(port)].Open();
						if (portiera == "Cofano" || portiera == "Bagagliaio")
						{
							HUD.ShowNotification("Hai aperto il ~y~ " + portiera + "~w~.", NotificationColor.Cyan);
						}
						else
						{
							HUD.ShowNotification("Hai aperto la portiera~y~ " + portiera + "~w~.", NotificationColor.Cyan);
						}
					}
				}
				else
				{
					if (portiera == "Cofano" || portiera == "Bagagliaio")
					{
						HUD.ShowNotification("Questo veicolo non ha il ~b~ " + portiera + "~w~!", NotificationColor.Red, true);
					}
					else
					{
						HUD.ShowNotification("Questo veicolo non ha la portiera ~b~ " + portiera + "~w~!", NotificationColor.Red, true);
					}
				}
			}
			else
			{
				if (vehicle != null && vehicle.Exists())
				{
					float distanceToVeh = World.GetDistance(Game.PlayerPed.Position, vehicle.Position);
					if (distanceToVeh <= 20f)
					{
						if (vehicle.Doors.HasDoor((VehicleDoorIndex)port))
						{
							if (vehicle.Doors[(VehicleDoorIndex)(port)].IsOpen)
							{
								vehicle.Doors[(VehicleDoorIndex)(port)].Close();
								if (portiera == "Cofano" || portiera == "Bagagliaio")
								{
									HUD.ShowNotification("Hai chiuso il ~y~ " + portiera + "~w~.", NotificationColor.Cyan);
								}
								else
								{
									HUD.ShowNotification("Hai chiuso la portiera~y~ " + portiera + "~w~.", NotificationColor.Cyan);
								}
							}
							else
							{
								vehicle.Doors[(VehicleDoorIndex)(port)].Open();
								HUD.ShowNotification("Hai aperto la portiera~y~ " + portiera + "~w~.", NotificationColor.Cyan);
							}
						}
						else
						{
							if (portiera == "Cofano" || portiera == "Bagagliaio")
							{
								HUD.ShowNotification("Questo veicolo non ha il ~b~ " + portiera + "~w~!", NotificationColor.Red, true);
							}
							else
							{
								HUD.ShowNotification("Questo veicolo non ha la portiera ~b~ " + portiera + "~w~!", NotificationColor.Red, true);
							}
						}
					}
					else
					{
						HUD.ShowNotification("Sei troppo distante dal tuo veicolo salvato!", NotificationColor.Red, true);
					}
				}
			}
		}

		static bool window0up = true;
		static bool window1up = true;
		static bool window2up = true;
		static bool window3up = true;
		public static void Finestrini(string finestrini)
		{
			if (Game.PlayerPed.IsInVehicle())
			{
				if (Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed)
				{
					if (finestrini == "Anteriore Sinistro")
					{
						if (window1up)
						{
							Game.PlayerPed.CurrentVehicle.Windows[VehicleWindowIndex.FrontLeftWindow].RollDown();
							window1up = false;
							WindowsGiu = true;
						}
						else
						{
							Game.PlayerPed.CurrentVehicle.Windows[VehicleWindowIndex.FrontLeftWindow].RollUp();
							window1up = true;
							WindowsGiu = false;
						}
					}
					else if (finestrini == "Anteriore Destro")
					{
						if (window0up)
						{
							Game.PlayerPed.CurrentVehicle.Windows[VehicleWindowIndex.FrontRightWindow].RollDown();
							window0up = false;
							WindowsGiu = true;
						}
						else
						{
							Game.PlayerPed.CurrentVehicle.Windows[VehicleWindowIndex.FrontRightWindow].RollUp();
							window0up = true;
							WindowsGiu = false;
						}
					}
					else if (finestrini == "Posteriore Sinistro")
					{
						if (window3up)
						{
							Game.PlayerPed.CurrentVehicle.Windows[VehicleWindowIndex.BackLeftWindow].RollDown();
							window3up = false;
							WindowsGiu = true;
						}
						else
						{
							Game.PlayerPed.CurrentVehicle.Windows[VehicleWindowIndex.BackLeftWindow].RollUp();
							window3up = true;
							WindowsGiu = false;
						}
					}
					else if (finestrini == "Posteriore Destro")
					{
						if (window2up)
						{
							Game.PlayerPed.CurrentVehicle.Windows[VehicleWindowIndex.BackRightWindow].RollDown();
							window2up = false;
							WindowsGiu = true;
						}
						else
						{
							Game.PlayerPed.CurrentVehicle.Windows[VehicleWindowIndex.BackRightWindow].RollUp();
							window2up = true;
							WindowsGiu = false;
						}
					}
				}
			}
		}

		public static void Save(bool saved)
		{
			if (Game.PlayerPed.IsSittingInVehicle() && Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed)
			{
				if (!saved)
				{
					saveVehicle.AttachedBlip.Delete();
					saveVehicle = null;
					HUD.ShowNotification("Veicolo salvato ~r~rimosso~w~ attento se ti allontani verrà eliminato.", NotificationColor.Blue);
					PersonalMenu.salvato = true;
				}
				else
				{
					saveVehicle = Game.PlayerPed.CurrentVehicle;
					saveVehicle.AttachBlip();
					saveVehicle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
					saveVehicle.AttachedBlip.Color = BlipColor.Green;
					HUD.ShowNotification("Questa ~y~" + saveVehicle.LocalizedName + "~w~ è stata~g~ salvata ~w~e non verrà eliminata se ti allontani.", NotificationColor.GreenDark);
					PersonalMenu.salvato = true;
				}
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
				float distanceToVeh = World.GetDistance(Game.PlayerPed.Position, vehicle.Position);
				if (toggle)
				{
					if (vehicle.Exists())
					{
						if (vehicle.Driver == Game.PlayerPed)
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
						if (vehicle.Driver == Game.PlayerPed)
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
			if (!HUD.MenuPool.IsAnyMenuOpen() && !IsHelpMessageBeingDisplayed() && !DoHideHud)
			{
				if (!IsPedRunningMobilePhoneTask(PlayerPedId()) && Main.spawned && MostraStatus)
				{
					if (Input.IsControlPressed(Control.FrontendRight, PadCheck.Controller) && !Input.IsControlPressed(Control.FrontendLb, PadCheck.Controller) || (Input.IsControlPressed(Control.SelectCharacterFranklin, PadCheck.Keyboard) && (!Game.IsPaused || IsPedStill(PlayerPedId()) || Game.PlayerPed.IsWalking || Game.PlayerPed.IsInVehicle() && !Game.PlayerPed.CurrentVehicle.IsEngineRunning)))
					{
						Game.DisableControlThisFrame(2, Control.FrontendLeft);
						if (Game.Player.GetPlayerData().CurrentChar.needs.fame > 30f)
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)), Color.FromArgb(255, 255, 255, 255), "FAME = ~y~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.fame, 2) + "%");
						else if (Game.Player.GetPlayerData().CurrentChar.needs.fame > 60f)
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)), Color.FromArgb(255, 255, 255, 255), "FAME = ~o~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.fame, 2) + "%");
						else if (Game.Player.GetPlayerData().CurrentChar.needs.fame > 90f)
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)), Color.FromArgb(255, 255, 255, 255), "FAME = ~r~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.fame, 2) + "%");
						else
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.6f)), Color.FromArgb(255, 255, 255, 255), "FAME = ~g~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.fame, 2) + "%");

						if (Game.Player.GetPlayerData().CurrentChar.needs.sete > 30f)
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)), Color.FromArgb(255, 255, 255, 255), "SETE = ~y~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.sete, 2) + "%");
						else if (Game.Player.GetPlayerData().CurrentChar.needs.sete > 60f)
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)), Color.FromArgb(255, 255, 255, 255), "SETE = ~o~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.sete, 2) + "%");
						else if (Game.Player.GetPlayerData().CurrentChar.needs.sete > 90f)
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)), Color.FromArgb(255, 255, 255, 255), "SETE = ~r~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.sete, 2) + "%");
						else
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.4f)), Color.FromArgb(255, 255, 255, 255), "SETE = ~g~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.sete, 2) + "%");

						if (Game.Player.GetPlayerData().CurrentChar.needs.stanchezza > 30f)
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)), Color.FromArgb(255, 255, 255, 255), "STANCH. = ~y~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.stanchezza, 2) + "%");
						else if (Game.Player.GetPlayerData().CurrentChar.needs.stanchezza > 60f)
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)), Color.FromArgb(255, 255, 255, 255), "STANCH. = ~o~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.stanchezza, 2) + "%");
						else if (Game.Player.GetPlayerData().CurrentChar.needs.stanchezza > 90f)
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)), Color.FromArgb(255, 255, 255, 255), "STANCH. = ~r~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.stanchezza, 2) + "%");
						else
							HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(0.6f, 0f, 0.2f)), Color.FromArgb(255, 255, 255, 255), "STANCH. = ~g~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.stanchezza, 2) + "%");
					}
					if (Game.IsControlJustPressed(1, Control.VehicleDuck) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
						Game.PlayerPed.Task.ClearAll();
				}
			}

			await Task.FromResult(0);
		}

		public static async Task MostramiSoldi()
		{
			if (!HUD.MenuPool.IsAnyMenuOpen() && !IsHelpMessageBeingDisplayed())
			{
				if (!IsPedRunningMobilePhoneTask(PlayerPedId()) && Main.spawned && MostraStatus)
				{
					if (Input.IsControlPressed(Control.FrontendLeft, PadCheck.Controller) && !Input.IsControlPressed(Control.FrontendLb, PadCheck.Controller) || (Input.IsControlPressed(Control.SelectCharacterMichael, PadCheck.Keyboard) && (!Game.IsPaused || IsPedStill(PlayerPedId()) || Game.PlayerPed.IsWalking || Game.PlayerPed.IsInVehicle() && !Game.PlayerPed.CurrentVehicle.IsEngineRunning)))
					{
						Game.DisableControlThisFrame(2, Control.FrontendRight);
						HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(-0.6f, 0f, 0.6f)), Color.FromArgb(255, 255, 255, 255), "Portafoglio = ~g~" + Game.Player.GetPlayerData().CurrentChar.finance.cash + "$");
						HUD.DrawText3D(Game.PlayerPed.GetOffsetPosition(new Vector3(-0.6f, 0f, 0.4f)), Color.FromArgb(255, 255, 255, 255), "Soldi Sporchi = ~r~" + Game.Player.GetPlayerData().CurrentChar.finance.dirtyCash + "$");
					}
				}
			}
			await Task.FromResult(0);
		}
	}
}
