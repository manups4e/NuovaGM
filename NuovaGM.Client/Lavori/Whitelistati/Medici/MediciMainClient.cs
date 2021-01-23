using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.MenuNativo;
using CitizenFX.Core.UI;

using Newtonsoft.Json;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.Lavori.Whitelistati.Medici
{
	static class MediciMainClient
	{
		public static Vehicle VeicoloAttuale;
		public static Vehicle ElicotteroAttuale;
		public static Dictionary<Ped, Blip> MedsBlips = new Dictionary<Ped, Blip>();
		public static Dictionary<Ped, Blip> Morti = new Dictionary<Ped, Blip>();
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			Client.Instance.AddEventHandler("lprp:medici:aggiungiPlayerAiMorti", new Action<int>(Aggiungi));
			Client.Instance.AddEventHandler("lprp:medici:rimuoviPlayerAiMorti", new Action<int>(Rimuovi));
		}

		private static async void Spawnato()
		{
			foreach (var ospedale in Client.Impostazioni.Lavori.Medici.Config.Ospedali)
			{
				Blip blip = World.CreateBlip(ospedale.Blip.Coords);
				blip.Sprite = (BlipSprite)ospedale.Blip.Sprite;
				blip.Scale = ospedale.Blip.Scale;
				blip.Color = (BlipColor)ospedale.Blip.Color;
				blip.IsShortRange = true;
				blip.Name = ospedale.Blip.Nome;
				SetBlipDisplay(blip.Handle, ospedale.Blip.Display);
			}
		}

		private static async void Aggiungi(int player)
		{
			Player pl = new Player(GetPlayerFromServerId(player));
			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "medico")
			{
				pl.Character.AttachBlip();
				pl.Character.AttachedBlip.Sprite = BlipSprite.Deathmatch;
				pl.Character.AttachedBlip.Color = BlipColor.Red;
				pl.Character.AttachedBlip.Scale = 1.4f;
				pl.Character.AttachedBlip.Name = "Ferito grave";
				SetBlipDisplay(pl.Character.AttachedBlip.Handle, 4);
				Morti.Add(pl.Character, pl.Character.AttachedBlip);
			}
		}

		private static async void Rimuovi(int player)
		{
			Player pl = new Player(GetPlayerFromServerId(player));
			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "medico")
			{
				if (Morti.ContainsKey(pl.Character))
				{
					foreach (Blip bl in pl.Character.AttachedBlips)
					{
						if (bl == Morti[pl.Character])
						{
							bl.Delete();
							Morti.Remove(pl.Character);
						}
					}
				}
			}
		}

		public static async Task MarkersMedici()
		{
			Ped p = Game.PlayerPed;
			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "medico")
			{
				foreach (var osp in Client.Impostazioni.Lavori.Medici.Config.Ospedali)
				{
					foreach (var vettore in osp.Spogliatoio)
					{
						if (p.IsInRangeOf(vettore,2f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~entrare~w~ / ~b~uscire~w~ in servizio");
							if (Input.IsControlJustPressed(Control.Context))
							{
								MenuMedici.MenuSpogliatoio();
							}
						}
					}

					foreach (var vettore in osp.Farmacia)
					{
						if (p.IsInRangeOf(vettore,1.5f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per usare la farmacia");
							if (Input.IsControlJustPressed(Control.Context))
							{
								// Menu farmacia
							}
						}
					}

					foreach (var vettore in osp.IngressoVisitatori)
					{
						if (p.IsInRangeOf(vettore,1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare in ospedale");
							if (Input.IsControlJustPressed(Control.Context))
							{
								Vector3 pos;
								if (osp.IngressoVisitatori.IndexOf(vettore) == 0 || osp.IngressoVisitatori.IndexOf(vettore) == 1)
									pos = new Vector3(272.8f, -1358.8f, 23.5f);
								else
									pos = new Vector3(254.301f, -1372.288f, 23.538f);

								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								p.Position = pos;
								Screen.Fading.FadeIn(800);
							}
						}
					}

					foreach (var vettore in osp.UscitaVisitatori)
					{
						if (p.IsInRangeOf(vettore,1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ospedale");
							if (Input.IsControlJustPressed(Control.Context))
							{
								Vector3 pos;
								if (osp.UscitaVisitatori.IndexOf(vettore) == 0)
									pos = osp.IngressoVisitatori[0];
								else
									pos = osp.IngressoVisitatori[2];

								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								p.Position = pos;
								Screen.Fading.FadeIn(800);
							}
						}
					}

					foreach (var vehicle in osp.Veicoli)
					{
						if (!p.IsInVehicle())
						{
							World.DrawMarker(MarkerType.CarSymbol, vehicle.SpawnerMenu, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Cyan, false, false, true);
							if (p.IsInRangeOf(vehicle.SpawnerMenu, 1.5f))
							{
								HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere il veicolo");
								if (Input.IsControlJustPressed(Control.Context))
								{
									Screen.Fading.FadeOut(800);
									await BaseScript.Delay(1000);
									MenuMedici.VehicleMenuNuovo(osp, vehicle);
								}
							}
						}
						for (int i = 0; i < vehicle.Deleters.Count; i++)
						{
							if (!Funzioni.IsSpawnPointClear(vehicle.Deleters[i], 2f))
								foreach (var veh in Funzioni.GetVehiclesInArea(vehicle.Deleters[i], 2f))
									if (!veh.HasDecor("VeicoloMedici") && !veh.HasDecor("VeicoloMedici"))
										veh.Delete();
							if (p.IsInVehicle())
							{
								World.DrawMarker(MarkerType.CarSymbol, vehicle.Deleters[i], new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Red, false, false, true);
								if (p.IsInRangeOf(vehicle.Deleters[i], 2f))
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");
									if (Input.IsControlJustPressed(Control.Context))
									{
										if (p.CurrentVehicle.HasDecor("VeicoloMedici"))
										{
											VeicoloPol vehicl = new VeicoloPol(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
											BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehMedici", vehicl.Serialize());
											p.CurrentVehicle.Delete();
											VeicoloAttuale = new Vehicle(0);
										}
										else
										{
											HUD.ShowNotification("Il veicolo che tenti di posare non è registrato a un medico!", NotificationColor.Red, true);
										}
									}
								}
							}
						}
					}

					foreach (var heli in osp.Elicotteri)
					{
						if (!p.IsInVehicle())
							World.DrawMarker(MarkerType.HelicopterSymbol, heli.SpawnerMenu, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Cyan, false, false, true);
						if (p.IsInRangeOf(heli.SpawnerMenu, 1.5f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere il veicolo");
							if (Input.IsControlJustPressed(Control.Context))
							{
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								MenuMedici.HeliMenu(osp, heli);
							}
						}
						for (int i = 0; i < heli.Deleters.Count; i++)
						{
							if (!Funzioni.IsSpawnPointClear(heli.Deleters[i], 2f))
								foreach (var veh in Funzioni.GetVehiclesInArea(heli.Deleters[i], 2f))
									if (!veh.HasDecor("VeicoloMedici") && !veh.HasDecor("VeicoloMedici"))
										veh.Delete();
							if (p.IsInVehicle())
							{
								World.DrawMarker(MarkerType.HelicopterSymbol, heli.Deleters[i], new Vector3(0), new Vector3(0), new Vector3(3f, 3f, 1.5f), Colors.Red, false, false, true);
								if (p.IsInRangeOf(heli.Deleters[i], 2f))
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");
									if (Input.IsControlJustPressed(Control.Context))
									{
										if (p.CurrentVehicle.HasDecor("VeicoloMedici"))
										{
											VeicoloPol veh = new VeicoloPol(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
											BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehMedici", veh.Serialize());
											p.CurrentVehicle.Delete();
											ElicotteroAttuale = new Vehicle(0);
										}
										else
											HUD.ShowNotification("L'elicottero che tenti di posare non è registrato a un medico!", NotificationColor.Red, true);
									}
								}
							}
						}
					}

				}
			}
		}

		public static async Task MarkersNonMedici()
		{
			Ped p = Game.PlayerPed;
			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() != "medico" || Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() != "medici")
			{
				foreach (var osp in Client.Impostazioni.Lavori.Medici.Config.Ospedali)
				{
					foreach (var vettore in osp.IngressoVisitatori)
					{
						if (p.IsInRangeOf(vettore,1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare in ospedale");
							if (Input.IsControlJustPressed(Control.Context))
							{
								Vector3 pos;
								if (osp.IngressoVisitatori.IndexOf(vettore) == 0 || osp.IngressoVisitatori.IndexOf(vettore) == 1)
									pos = new Vector3(272.8f, -1358.8f, 23.5f);
								else
									pos = new Vector3(254.301f, -1372.288f, 24.538f);

								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								p.Position = pos;
								Screen.Fading.FadeIn(800);
							}
						}
					}

					foreach (var vettore in osp.UscitaVisitatori)
					{
						if (p.IsInRangeOf(vettore, 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ospedale");
							if (Input.IsControlJustPressed(Control.Context))
							{
								Vector3 pos;
								if (osp.UscitaVisitatori.IndexOf(vettore) == 0)
									pos = osp.IngressoVisitatori[0];
								else
									pos = osp.IngressoVisitatori[2];

								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								p.Position = pos;
								Screen.Fading.FadeIn(800);
							}
						}
					}
				}
			}
		}


		public static async Task AbilitaBlipVolanti()
		{
			await BaseScript.Delay(1000);
			if (Client.Impostazioni.Lavori.Medici.Config.AbilitaBlipVolanti)
			{
				foreach (var p in Eventi.GiocatoriOnline)
				{
					if (p.Value.CurrentChar.job.name == "Medici")
					{
						int id = GetPlayerFromServerId(p.Value.source);
						if (NetworkIsPlayerActive(id) && GetPlayerPed(id) != PlayerPedId())
						{
							Ped playerPed = new Ped(GetPlayerPed(id));
							if (playerPed.IsInVehicle())
							{
								if (playerPed.CurrentVehicle.HasDecor("VeicoloMedici"))
								{
									if (!MedsBlips.ContainsKey(playerPed))
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
										MedsBlips.Add(playerPed, polblip);
									}
									else if (MedsBlips.ContainsKey(playerPed))
									{
										if (playerPed.AttachedBlip != null)
										{
											if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceHelicopter)
												if (playerPed.CurrentVehicle.IsEngineRunning)
													playerPed.AttachedBlip.Sprite = BlipSprite.PoliceHelicopterAnimated;
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
								if (MedsBlips.ContainsKey(playerPed))
								{
									foreach (Blip b in playerPed.AttachedBlips)
										if (b.Sprite == BlipSprite.PoliceCar || b.Sprite == BlipSprite.PoliceCarDot || b.Sprite == BlipSprite.PoliceHelicopter || b.Sprite == BlipSprite.PoliceHelicopterAnimated || b.Sprite == BlipSprite.PersonalVehicleBike || b.Sprite == BlipSprite.Boat)
											b.Delete();
									MedsBlips.Remove(playerPed);
								}
							}
						}
					}
				}
			}
			else
				Client.Instance.RemoveTick(AbilitaBlipVolanti);
		}

		public static async Task BlipMorti()
		{
			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "medico")
			{
				if (Game.Player.GetPlayerData().StatiPlayer.InServizio)
					foreach(var morto in Morti)
						morto.Value.Alpha = 255;
				else
					foreach (var morto in Morti)
						morto.Value.Alpha = 0;
			}
		}
	}
}
