using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Core.Utility;
using ScaleformUI;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.PlayerChar;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Whitelistati.Medici
{
	internal static class MediciMainClient
	{
		public static Vehicle VeicoloAttuale;
		public static Vehicle ElicotteroAttuale;
		public static Dictionary<Ped, Blip> MedsBlips = new Dictionary<Ped, Blip>();
		public static Dictionary<Ped, Blip> Morti = new Dictionary<Ped, Blip>();

		public static void Init()
		{
			Client.Instance.AddEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));
			Client.Instance.AddEventHandler("lprp:medici:aggiungiPlayerAiMorti", new Action<int>(Aggiungi));
			Client.Instance.AddEventHandler("lprp:medici:rimuoviPlayerAiMorti", new Action<int>(Rimuovi));
		}

		public static void Stop()
		{
			Client.Instance.RemoveEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));
			Client.Instance.RemoveEventHandler("lprp:medici:aggiungiPlayerAiMorti", new Action<int>(Aggiungi));
			Client.Instance.RemoveEventHandler("lprp:medici:rimuoviPlayerAiMorti", new Action<int>(Rimuovi));
		}

		private static async void Spawnato()
		{
			foreach (Ospedale ospedale in Client.Impostazioni.RolePlay.Lavori.Medici.Config.Ospedali)
			{
				Blip blip = World.CreateBlip(ospedale.Blip.Coords.ToVector3);
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

			if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "medico")
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

			if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "medico")
				if (Morti.ContainsKey(pl.Character))
					foreach (Blip bl in pl.Character.AttachedBlips)
						if (bl == Morti[pl.Character])
						{
							bl.Delete();
							Morti.Remove(pl.Character);
						}
		}

		public static async Task MarkersMedici()
		{
			Ped p = Cache.PlayerCache.MyPlayer.Ped;

			if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "medico")
				foreach (Ospedale osp in Client.Impostazioni.RolePlay.Lavori.Medici.Config.Ospedali)
				{
					foreach (Position vettore in osp.Spogliatoio)
						if (p.IsInRangeOf(vettore.ToVector3, 2f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~entrare~w~ / ~b~uscire~w~ in servizio");
							if (Input.IsControlJustPressed(Control.Context)) MenuMedici.MenuSpogliatoio();
						}

					foreach (Position vettore in osp.Farmacia)
						if (p.IsInRangeOf(vettore.ToVector3, 1.5f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per usare la farmacia");

							if (Input.IsControlJustPressed(Control.Context))
							{
								// Menu farmacia
							}
						}

					foreach (Position vettore in osp.IngressoVisitatori)
						if (p.IsInRangeOf(vettore.ToVector3, 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare in ospedale");

							if (Input.IsControlJustPressed(Control.Context))
							{
								Position pos;
								if (osp.IngressoVisitatori.IndexOf(vettore) == 0 || osp.IngressoVisitatori.IndexOf(vettore) == 1)
									pos = new Position(272.8f, -1358.8f, 23.5f);
								else
									pos = new Position(254.301f, -1372.288f, 23.538f);
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								p.Position = pos.ToVector3;
								Screen.Fading.FadeIn(800);
							}
						}

					foreach (Position vettore in osp.UscitaVisitatori)
						if (p.IsInRangeOf(vettore.ToVector3, 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ospedale");

							if (Input.IsControlJustPressed(Control.Context))
							{
								Position pos;
								if (osp.UscitaVisitatori.IndexOf(vettore) == 0)
									pos = osp.IngressoVisitatori[0];
								else
									pos = osp.IngressoVisitatori[2];
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								p.Position = pos.ToVector3;
								Screen.Fading.FadeIn(800);
							}
						}

					foreach (SpawnerSpawn vehicle in osp.Veicoli)
					{
						if (!Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InVeicolo)
						{
							World.DrawMarker(MarkerType.CarSymbol, vehicle.SpawnerMenu.ToVector3, Position.Zero.ToVector3, Position.Zero.ToVector3, new Position(2f, 2f, 1.5f).ToVector3, Colors.Cyan, false, false, true);

							if (p.IsInRangeOf(vehicle.SpawnerMenu.ToVector3, 1.5f))
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
							if (!Funzioni.IsSpawnPointClear(vehicle.Deleters[i].ToVector3, 2f))
								foreach (Vehicle veh in Funzioni.GetVehiclesInArea(vehicle.Deleters[i].ToVector3, 2f))
									if (!veh.HasDecor("VeicoloMedici") && !veh.HasDecor("VeicoloMedici"))
										veh.Delete();

							if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InVeicolo)
							{
								World.DrawMarker(MarkerType.CarSymbol, vehicle.Deleters[i].ToVector3, Position.Zero.ToVector3, Position.Zero.ToVector3, new Position(2f, 2f, 1.5f).ToVector3, Colors.Red, false, false, true);

								if (p.IsInRangeOf(vehicle.Deleters[i].ToVector3, 2f))
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");

									if (Input.IsControlJustPressed(Control.Context))
									{
										if (p.CurrentVehicle.HasDecor("VeicoloMedici"))
										{
											VeicoloPol vehicl = new VeicoloPol(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
											BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehMedici", vehicl.ToJson());
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

					foreach (SpawnerSpawn heli in osp.Elicotteri)
					{
						if (!Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InVeicolo) World.DrawMarker(MarkerType.HelicopterSymbol, heli.SpawnerMenu.ToVector3, Position.Zero.ToVector3, Position.Zero.ToVector3, new Position(2f, 2f, 1.5f).ToVector3, Colors.Cyan, false, false, true);

						if (p.IsInRangeOf(heli.SpawnerMenu.ToVector3, 1.5f))
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
							if (!Funzioni.IsSpawnPointClear(heli.Deleters[i].ToVector3, 2f))
								foreach (Vehicle veh in Funzioni.GetVehiclesInArea(heli.Deleters[i].ToVector3, 2f))
									if (!veh.HasDecor("VeicoloMedici") && !veh.HasDecor("VeicoloMedici"))
										veh.Delete();

							if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InVeicolo)
							{
								World.DrawMarker(MarkerType.HelicopterSymbol, heli.Deleters[i].ToVector3, Position.Zero.ToVector3, Position.Zero.ToVector3, new Position(3f, 3f, 1.5f).ToVector3, Colors.Red, false, false, true);

								if (p.IsInRangeOf(heli.Deleters[i].ToVector3, 2f))
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");

									if (Input.IsControlJustPressed(Control.Context))
									{
										if (p.CurrentVehicle.HasDecor("VeicoloMedici"))
										{
											VeicoloPol veh = new VeicoloPol(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
											BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehMedici", veh.ToJson());
											p.CurrentVehicle.Delete();
											ElicotteroAttuale = new Vehicle(0);
										}
										else
										{
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
			Ped p = Cache.PlayerCache.MyPlayer.Ped;

			if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() != "medico" || Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() != "medici")
				foreach (Ospedale osp in Client.Impostazioni.RolePlay.Lavori.Medici.Config.Ospedali)
				{
					foreach (Position vettore in osp.IngressoVisitatori.Where(vettore => p.IsInRangeOf(vettore.ToVector3, 1.375f)))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare in ospedale");

						if (!Input.IsControlJustPressed(Control.Context)) continue;
						Position pos;
						if (osp.IngressoVisitatori.IndexOf(vettore) == 0 || osp.IngressoVisitatori.IndexOf(vettore) == 1)
							pos = new Position(272.8f, -1358.8f, 23.5f);
						else
							pos = new Position(254.301f, -1372.288f, 24.538f);
						Screen.Fading.FadeOut(800);
						await BaseScript.Delay(1000);
						p.Position = pos.ToVector3;
						Screen.Fading.FadeIn(800);
					}

					foreach (Position vettore in osp.UscitaVisitatori.Where(vettore => p.IsInRangeOf(vettore.ToVector3, 1.375f)))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ospedale");

						if (!Input.IsControlJustPressed(Control.Context)) continue;
						Position pos = osp.UscitaVisitatori.IndexOf(vettore) == 0 ? osp.IngressoVisitatori[0] : osp.IngressoVisitatori[2];
						Screen.Fading.FadeOut(800);
						await BaseScript.Delay(1000);
						p.Position = pos.ToVector3;
						Screen.Fading.FadeIn(800);
					}
				}
		}

		public static async Task AbilitaBlipVolanti()
		{
			await BaseScript.Delay(1000);

			if (Client.Impostazioni.RolePlay.Lavori.Medici.Config.AbilitaBlipVolanti)
			{
				foreach (var p in Cache.PlayerCache.GiocatoriOnline)
					if (p.User.CurrentChar.Job.Name == "Medici")
					{
						int id = GetPlayerFromServerId(p.Player.ServerId);

						if (!NetworkIsPlayerActive(id) || GetPlayerPed(id) == PlayerPedId()) continue;
						Ped playerPed = new(GetPlayerPed(id));

						if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InVeicolo)
						{
							if (!playerPed.CurrentVehicle.HasDecor("VeicoloMedici")) continue;

							if (!MedsBlips.ContainsKey(playerPed))
							{
								if (playerPed.AttachedBlips.Length > 0) playerPed.AttachedBlip.Delete();
								Blip polblip = playerPed.AttachBlip();
								if (playerPed.CurrentVehicle.Model.IsCar)
									polblip.Sprite = BlipSprite.PoliceCar;
								else if (playerPed.CurrentVehicle.Model.IsBike)
									polblip.Sprite = BlipSprite.PersonalVehicleBike;
								else if (playerPed.CurrentVehicle.Model.IsBoat)
									polblip.Sprite = BlipSprite.Boat;
								else if (playerPed.CurrentVehicle.Model.IsHelicopter) polblip.Sprite = BlipSprite.PoliceHelicopter;
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
								if (playerPed.AttachedBlip == null) continue;
								if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceHelicopter)
									if (playerPed.CurrentVehicle.IsEngineRunning)
										playerPed.AttachedBlip.Sprite = BlipSprite.PoliceHelicopterAnimated;

								if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceHelicopterAnimated)
								{
									SetBlipShowCone(playerPed.AttachedBlip.Handle, playerPed.CurrentVehicle.HeightAboveGround > 5f);
									if (!playerPed.CurrentVehicle.IsEngineRunning) playerPed.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
								}

								if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceCar || playerPed.AttachedBlip.Sprite == BlipSprite.Boat || playerPed.AttachedBlip.Sprite == BlipSprite.PersonalVehicleBike)
									if (playerPed.CurrentVehicle.HasSiren && playerPed.CurrentVehicle.IsSirenActive)
									{
										playerPed.AttachedBlip.Sprite = BlipSprite.PoliceCarDot;
										SetBlipShowCone(playerPed.AttachedBlip.Handle, true);
									}

								if (playerPed.AttachedBlip.Sprite != BlipSprite.PoliceCarDot) continue;
								if (!playerPed.CurrentVehicle.HasSiren || playerPed.CurrentVehicle.IsSirenActive) continue;
								SetBlipShowCone(playerPed.AttachedBlip.Handle, false);
								if (playerPed.CurrentVehicle.Model.IsCar)
									playerPed.AttachedBlip.Sprite = BlipSprite.PoliceCar;
								else if (playerPed.CurrentVehicle.Model.IsBike)
									playerPed.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
								else if (playerPed.CurrentVehicle.Model.IsBoat) playerPed.AttachedBlip.Sprite = BlipSprite.Boat;
							}
						}
						else
						{
							if (!MedsBlips.ContainsKey(playerPed)) continue;
							foreach (Blip b in playerPed.AttachedBlips)
								if (b.Sprite == BlipSprite.PoliceCar || b.Sprite == BlipSprite.PoliceCarDot || b.Sprite == BlipSprite.PoliceHelicopter || b.Sprite == BlipSprite.PoliceHelicopterAnimated || b.Sprite == BlipSprite.PersonalVehicleBike || b.Sprite == BlipSprite.Boat)
									b.Delete();
							MedsBlips.Remove(playerPed);
						}
					}
			}
			else
			{
				Client.Instance.RemoveTick(AbilitaBlipVolanti);
			}
		}

		public static async Task BlipMorti()
		{
			if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "medico")
			{
				if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InServizio)
					foreach (KeyValuePair<Ped, Blip> morto in Morti)
						morto.Value.Alpha = 255;
				else
					foreach (KeyValuePair<Ped, Blip> morto in Morti)
						morto.Value.Alpha = 0;
			}
		}
	}
}