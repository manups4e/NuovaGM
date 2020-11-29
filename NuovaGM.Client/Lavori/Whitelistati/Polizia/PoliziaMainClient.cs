using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.MenuNativo;
using CitizenFX.Core.UI;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.Lavori.Whitelistati.Polizia
{
	static class PoliziaMainClient
	{
		public static List<Vehicle> VeicoliPolizia = new List<Vehicle>();
		public static Vehicle VeicoloAttuale = new Vehicle(0);
		public static Vehicle ElicotteroAttuale = new Vehicle(0);
		public static Dictionary<Ped, Blip> CopsBlips = new Dictionary<Ped, Blip>();
		public static bool InServizioDaPilota = false;
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			Client.Instance.AddEventHandler("lprp:polizia:ammanetta_smanetta", new Action(AmmanettaSmanetta));
			Client.Instance.AddEventHandler("lprp:polizia:accompagna", new Action<int>(Accompagna));
			Client.Instance.AddEventHandler("lprp:polizia:mettiVeh", new Action(MettiVeh));
			Client.Instance.AddEventHandler("lprp:polizia:togliVeh", new Action(TogliVeh));
		}

		public static void Spawnato()
		{
			foreach (var stazione in Client.Impostazioni.Lavori.Polizia.Config.Stazioni)
			{
				Blip blip = new Blip(AddBlipForCoord(stazione.Blip.Coords[0], stazione.Blip.Coords[1], stazione.Blip.Coords[2]))
				{
					Sprite = (BlipSprite)stazione.Blip.Sprite,
					Scale = stazione.Blip.Scale,
					Color = (BlipColor)stazione.Blip.Color,
					IsShortRange = true,
					Name = "Stazione di Polizia"
				};
				SetBlipDisplay(blip.Handle, stazione.Blip.Display);
			}
		}

		private static async void AmmanettaSmanetta()
		{
			Game.PlayerPed.SetDecor("PlayerAmmanettato", !Game.PlayerPed.GetDecor<bool>("PlayerAmmanettato"));
			RequestAnimDict("mp_arresting");
			while (!HasAnimDictLoaded("mp_arresting")) await BaseScript.Delay(1);
			if (Game.PlayerPed.GetDecor<bool>("PlayerAmmanettato"))
			{
				Game.PlayerPed.Task.ClearAll();
				Game.PlayerPed.Task.PlayAnimation("mp_arrestring", "idle", 8f, -1, (AnimationFlags)49);
				Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
				SetEnableHandcuffs(PlayerPedId(), true);
				DisablePlayerFiring(PlayerId(), true);
				Game.PlayerPed.CanPlayGestures = false;
				if (Game.Player.GetPlayerData().CurrentChar.skin.sex.ToLower() == "femmina")
					SetPedComponentVariation(Game.PlayerPed.Handle, 7, 25, 0, 0);
				else
					SetPedComponentVariation(Game.PlayerPed.Handle, 7, 41, 0, 0);
				Game.Player.CanControlCharacter = false;
				Client.Instance.AddTick(Ammanettato);
			}
			else
			{
				Client.Instance.RemoveTick(Ammanettato);
				Game.PlayerPed.Task.ClearAll();
				SetEnableHandcuffs(PlayerPedId(), false);
				UncuffPed(PlayerPedId());
				SetPedComponentVariation(Game.PlayerPed.Handle, Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Accessori, Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Accessori, 0, 0);
				SetEnableHandcuffs(PlayerPedId(), false);
				DisablePlayerFiring(PlayerId(), false);
				Game.PlayerPed.CanPlayGestures = true;
				Game.Player.CanControlCharacter = true;
			}
		}

		private static async void Accompagna(int ped)
		{
			Ped pol = (Ped)Entity.FromNetworkId(ped);
			if (Game.PlayerPed.GetDecor<bool>("PlayerAmmanettato"))
				Game.PlayerPed.Task.FollowToOffsetFromEntity(pol, new Vector3(1f, 1f, 0), 3f, -1, 1f, true);
		}
		private static async void TogliVeh()
		{
			if (Game.PlayerPed.GetDecor<bool>("PlayerAmmanettato"))
			{
				if(Game.PlayerPed.IsInVehicle())
					Game.PlayerPed.Task.LeaveVehicle(LeaveVehicleFlags.None);	
			}

		}
		private static async void MettiVeh()
		{
			if (Game.PlayerPed.GetDecor<bool>("PlayerAmmanettato"))
			{
				Vehicle closestVeh = Game.PlayerPed.GetClosestVehicle();
				if(closestVeh.IsSeatFree(VehicleSeat.LeftRear))
					Game.PlayerPed.Task.EnterVehicle(closestVeh, VehicleSeat.LeftRear);
				else if (Game.PlayerPed.LastVehicle.IsSeatFree(VehicleSeat.RightRear))
					Game.PlayerPed.Task.EnterVehicle(closestVeh, VehicleSeat.LeftRear);
			}
		}

		public static async Task MarkersPolizia()
		{
			Ped p = Game.PlayerPed;
			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "polizia")
			{
				for (int stazione=0; stazione < Client.Impostazioni.Lavori.Polizia.Config.Stazioni.Count; stazione++)
				{
					for (int spoglio = 0; spoglio < Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Spogliatoio.Count; spoglio++)
					{
						World.DrawMarker(MarkerType.HorizontalCircleSkinny, Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Spogliatoio[spoglio], new Vector3(0), new Vector3(0), new Vector3(2f, 2f, .5f), Colors.Blue, false, false, true);
						if (p.IsInRangeOf(Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Spogliatoio[spoglio], 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per cambiarti ed entrare/uscire in ~g~Servizio~w~");
							if (Input.IsControlJustPressed(Control.Context))
								MenuPolizia.CloakRoomMenu();
						}
					}

					for (int arm = 0; arm < Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Armerie.Count; arm++)
						World.DrawMarker(MarkerType.HorizontalCircleSkinny, Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Armerie[arm], new Vector3(0), new Vector3(0), new Vector3(2f, 2f, .5f), Colors.Red, false, false, true);

					for (int veh = 0; veh< Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Veicoli.Count; veh++)
					{
						World.DrawMarker(MarkerType.CarSymbol, Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Veicoli[veh].SpawnerMenu, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Blue, false, false, true);
						if (p.IsInRangeOf(Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Veicoli[veh].SpawnerMenu,  1.375f) && !HUD.MenuPool.IsAnyMenuOpen)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere il veicolo");
							if (Input.IsControlJustPressed(Control.Context))
							{
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								MenuPolizia.VehicleMenuNuovo(Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione], Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Veicoli[veh]);
							}
						}
						for (int del=0;del< Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Veicoli[veh].Deleters.Count; del++)
						{
							if (p.IsInVehicle())
							{
								World.DrawMarker(MarkerType.CarSymbol, Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Veicoli[veh].Deleters[del], new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Red, false, false, true);
								if (p.IsInRangeOf(Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Veicoli[veh].Deleters[del], 1.375f) && p.IsInVehicle() && !HUD.MenuPool.IsAnyMenuOpen)
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");
									if (Input.IsControlJustPressed(Control.Context))
									{
										if (p.CurrentVehicle.HasDecor("VeicoloPolizia"))
										{
											VeicoloPol vehicle = new VeicoloPol(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
											BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehPolizia", vehicle.Serialize());
											p.CurrentVehicle.Delete();
											VeicoloAttuale = new Vehicle(0);
										}
										else
											HUD.ShowNotification("Il veicolo che tenti di posare non è della polizia!", NotificationColor.Red, true);
									}
								}
							}
						}
					}
					for (int eli = 0; eli < Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Elicotteri.Count; eli++)
					{
						World.DrawMarker(MarkerType.HelicopterSymbol, Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Elicotteri[eli].SpawnerMenu, new Vector3(0), new Vector3(0), new Vector3(3f, 3f, 1.5f), Colors.Blue, false, false, true);
						if (p.IsInRangeOf(Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Elicotteri[eli].SpawnerMenu, 1.375f) && !HUD.MenuPool.IsAnyMenuOpen)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere l'elicottero");
							if (Input.IsControlJustPressed(Control.Context))
							{
								CitizenFX.Core.UI.Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								MenuPolizia.HeliMenu(Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione], Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Elicotteri[eli]);
							}
						}
						for (int del = 0; del < Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Elicotteri[eli].Deleters.Count; del++)
						{
							if (!Funzioni.IsSpawnPointClear(Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Elicotteri[eli].Deleters[del], 2f))
							{
								foreach (var veh in Funzioni.GetVehiclesInArea(Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Elicotteri[eli].Deleters[del], 2f))
								{
									if (!veh.HasDecor("VeicoloPolizia"))
										veh.Delete();
								}
							}

							if (p.IsInHeli)
							{
								World.DrawMarker(MarkerType.HelicopterSymbol, Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Elicotteri[eli].Deleters[del], new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Red, false, false, true);
								if (p.IsInRangeOf(Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].Elicotteri[eli].Deleters[del], 3.375f) && p.IsInHeli && !HUD.MenuPool.IsAnyMenuOpen)
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare l'elicottero nel deposito");
									if (Input.IsControlJustPressed(Control.Context))
									{
										if (p.CurrentVehicle.HasDecor("VeicoloPolizia"))
										{
											VeicoloPol veh = new VeicoloPol(p.CurrentVehicle.Mods.LicensePlate, Game.PlayerPed.CurrentVehicle.Model.Hash, Game.PlayerPed.CurrentVehicle.Handle);
											BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehPolizia", veh.Serialize());
											Game.PlayerPed.CurrentVehicle.Delete();
											ElicotteroAttuale = new Vehicle(0);
										}
										else
											HUD.ShowNotification("Il veicolo che tenti di posare non è della polizia!", NotificationColor.Red, true);
									}
								}
							}
						}
					}
					if (Game.Player.GetPlayerData().CurrentChar.job.grade == Client.Impostazioni.Lavori.Polizia.Gradi.Count - 1)
					{
						for (int boss = 0; boss < Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].AzioniCapo.Count; boss++)
							World.DrawMarker(MarkerType.HorizontalCircleSkinny, Client.Impostazioni.Lavori.Polizia.Config.Stazioni[stazione].AzioniCapo[boss], new Vector3(0), new Vector3(0), new Vector3(2f, 2f, .5f), Colors.Blue, false, false, true);
					}
				}
			}
			else
				await BaseScript.Delay(5000);
			await Task.FromResult(0);
		}

		public static async Task AbilitaBlipVolanti()
		{
			await BaseScript.Delay(1000);
			if (Client.Impostazioni.Lavori.Polizia.Config.AbilitaBlipVolanti)
			{
				foreach (var p in Eventi.GiocatoriOnline)
				{
					if (p.Value.CurrentChar.job.name == "Polizia")
					{
						int id = GetPlayerFromServerId(p.Value.source);
						if (NetworkIsPlayerActive(id) && GetPlayerPed(id) != PlayerPedId())
						{
							Ped playerPed = new Ped(GetPlayerPed(id));
							if (playerPed.IsInVehicle())
							{
								if (playerPed.CurrentVehicle.HasDecor("VeicoloPolizia"))
								{
									if (!CopsBlips.ContainsKey(playerPed))
									{
										if (playerPed.AttachedBlips.Length > 0)
											playerPed.AttachedBlip.Delete();

										Blip polblip = playerPed.AttachBlip();
										if (playerPed.CurrentVehicle.Model.IsCar)
											polblip.Sprite = BlipSprite.PoliceCar;
										else if (playerPed.CurrentVehicle.Model.IsBike)
											polblip.Sprite = BlipSprite.PersonalVehicleBike;
										else if (playerPed.CurrentVehicle.Model.IsBoat)
											polblip.Sprite = BlipSprite.Boat;
										else if (playerPed.CurrentVehicle.Model.IsHelicopter)
											polblip.Sprite = BlipSprite.PoliceHelicopter;

										polblip.Scale = 0.8f;
										SetBlipCategory(polblip.Handle, 7);
										SetBlipDisplay(polblip.Handle, 4);
										SetBlipAsShortRange(polblip.Handle, true);
										SetBlipNameToPlayerName(polblip.Handle, id);
										ShowHeadingIndicatorOnBlip(polblip.Handle, true);
										CopsBlips.Add(playerPed, polblip);
									}
									else if (CopsBlips.ContainsKey(playerPed))
									{
										if (playerPed.AttachedBlip != null)
										{
											if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceHelicopter)
											{
												if (playerPed.CurrentVehicle.IsEngineRunning)
													playerPed.AttachedBlip.Sprite = BlipSprite.PoliceHelicopterAnimated;
											}
											if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceHelicopterAnimated)
											{
												if (playerPed.CurrentVehicle.HeightAboveGround > 5f)
													SetBlipShowCone(playerPed.AttachedBlip.Handle, true);
												else
													SetBlipShowCone(playerPed.AttachedBlip.Handle, false);

												if (!playerPed.CurrentVehicle.IsEngineRunning)
													playerPed.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
											}
											if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceCar || playerPed.AttachedBlip.Sprite == BlipSprite.Boat || playerPed.AttachedBlip.Sprite == BlipSprite.PersonalVehicleBike)
											{
												if (playerPed.CurrentVehicle.HasSiren && playerPed.CurrentVehicle.IsSirenActive)
												{
													playerPed.AttachedBlip.Sprite = BlipSprite.PoliceCarDot;
													SetBlipShowCone(playerPed.AttachedBlip.Handle, true);
												}
											}
											if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceCarDot)
											{
												if (playerPed.CurrentVehicle.HasSiren && !playerPed.CurrentVehicle.IsSirenActive)
												{
													SetBlipShowCone(playerPed.AttachedBlip.Handle, false);
													if (playerPed.CurrentVehicle.Model.IsCar)
														playerPed.AttachedBlip.Sprite = BlipSprite.PoliceCar;
													else if (playerPed.CurrentVehicle.Model.IsBike)
														playerPed.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
													else if (playerPed.CurrentVehicle.Model.IsBoat)
														playerPed.AttachedBlip.Sprite = BlipSprite.Boat;
												}
											}
										}
									}
								}
							}
							else
							{
								if (CopsBlips.ContainsKey(playerPed))
								{
									foreach (Blip b in playerPed.AttachedBlips)
									{
										if (b.Sprite == BlipSprite.PoliceCar || b.Sprite == BlipSprite.PoliceCarDot || b.Sprite == BlipSprite.PoliceHelicopter || b.Sprite == BlipSprite.PoliceHelicopterAnimated || b.Sprite == BlipSprite.PersonalVehicleBike || b.Sprite == BlipSprite.Boat)
											b.Delete();
									}
									CopsBlips.Remove(playerPed);
								}
							}
						}
					}
				}
			}
			else
			{
				Client.Instance.RemoveTick(AbilitaBlipVolanti);
			}
		}

		public static async Task MainTickPolizia()
		{
			if (Game.Player.GetPlayerData().CurrentChar.job.name == "Polizia")
				if (Input.IsControlJustPressed(Control.SelectCharacterFranklin, PadCheck.Keyboard) && !HUD.MenuPool.IsAnyMenuOpen)
					MenuPolizia.MainMenu();
			await Task.FromResult(0);
		}

		public static async Task Ammanettato()
		{
			Ped p = Game.PlayerPed;

			if (Game.Player.CanControlCharacter)
				Game.Player.CanControlCharacter = false;
			if(!p.IsCuffed)
				SetEnableHandcuffs(p.Handle, true);
			if(Game.Player.CanControlCharacter)
				Game.Player.CanControlCharacter = false;

			if(!IsEntityPlayingAnim(p.Handle, "mp_arresting", "idle", 3))
			{
				if(!HasAnimDictLoaded("mp_arresting"))
				{
					RequestAnimDict("mp_arresting");
					while (!HasAnimDictLoaded("mp_arresting")) await BaseScript.Delay(10);
					p.Task.PlayAnimation("mp_arrestring", "idle", 8f, -1, (AnimationFlags)49);
				}
			}
		}
	}
}



