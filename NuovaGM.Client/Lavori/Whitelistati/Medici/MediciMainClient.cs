using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.MenuNativo;
using CitizenFX.Core.UI;
using NuovaGM.Shared;
using Newtonsoft.Json;

namespace NuovaGM.Client.Lavori.Whitelistati.Medici
{
	static class MediciMainClient
	{
		public static Vehicle VeicoloAttuale;
		public static Vehicle ElicotteroAttuale;
		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static async void Spawnato()
		{
			foreach (var ospedale in ConfigClient.Conf.Lavori.Medici.Config.Ospedali)
			{
				Blip blip = World.CreateBlip(ospedale.Blip.Coords.ToVector3());
				blip.Sprite = (BlipSprite)ospedale.Blip.Sprite;
				blip.Scale = ospedale.Blip.Scale;
				blip.Color = (BlipColor)ospedale.Blip.Color;
				blip.IsShortRange = true;
				blip.Name = ospedale.Blip.Nome;
				SetBlipDisplay(blip.Handle, ospedale.Blip.Display);
			}
			// da fixare col gestore dei lavori per i tick
			Client.GetInstance.RegisterTickHandler(MarkersMedici);
			Client.GetInstance.RegisterTickHandler(MarkersNonMedici);
		}

		public static async Task MarkersMedici()
		{
			if (Eventi.Player.CurrentChar.job.name.ToLower() == "medico")
			{
				foreach (var osp in ConfigClient.Conf.Lavori.Medici.Config.Ospedali)
				{
					foreach (float[] vettore in osp.Spogliatoio)
					{
						if (World.GetDistance(Game.PlayerPed.Position, vettore.ToVector3()) < 2f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~entrare~w~ / ~b~uscire~w~ in servizio");
							if (Game.IsControlJustPressed(0, Control.Context))
							{
								// Menu spogliatoio
							}
						}
					}

					foreach (float[] vettore in osp.Farmacia)
					{
						if (World.GetDistance(Game.PlayerPed.Position, vettore.ToVector3()) < 1.5f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per usare la farmacia");
							if (Game.IsControlJustPressed(0, Control.Context))
							{
								// Menu farmacia
							}
						}
					}

					foreach (float[] vettore in osp.IngressoVisitatori)
					{
						if (World.GetDistance(Game.PlayerPed.Position, vettore.ToVector3()) < 1.375f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare in ospedale");
							if (Game.IsControlJustPressed(0, Control.Context))
							{
								Vector3 pos;
								if (osp.IngressoVisitatori.IndexOf(vettore) == 0 || osp.IngressoVisitatori.IndexOf(vettore) == 1)
									pos = new Vector3(272.8f, -1358.8f, 23.5f);
								else
									pos = new Vector3(254.301f, -1372.288f, 23.538f);

								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								Game.PlayerPed.Position = pos;
								Screen.Fading.FadeIn(800);
							}
						}
					}

					foreach (float[] vettore in osp.UscitaVisitatori)
					{
						if (World.GetDistance(Game.PlayerPed.Position, vettore.ToVector3()) < 1.375f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ospedale");
							if (Game.IsControlJustPressed(0, Control.Context))
							{
								Vector3 pos;
								if (osp.UscitaVisitatori.IndexOf(vettore) == 0)
									pos = osp.IngressoVisitatori[0].ToVector3();
								else
									pos = osp.IngressoVisitatori[2].ToVector3();

								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								Game.PlayerPed.Position = pos;
								Screen.Fading.FadeIn(800);
							}
						}
					}

					foreach (var vehicle in osp.Veicoli)
					{
						if (!Game.PlayerPed.IsInVehicle())
						{
							World.DrawMarker(MarkerType.CarSymbol, vehicle.SpawnerMenu.ToVector3(), new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Cyan, false, false, true);
							if (World.GetDistance(Game.PlayerPed.Position, vehicle.SpawnerMenu.ToVector3()) < 1.5f)
							{
								HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere il veicolo");
								if (Game.IsControlJustPressed(0, Control.Context))
								{
									Screen.Fading.FadeOut(800);
									await BaseScript.Delay(1000);
									MenuMedici.VehicleMenuNuovo(osp, vehicle);
								}
							}
						}
						for (int i = 0; i < vehicle.Deleters.Count; i++)
						{
							if (!Funzioni.IsSpawnPointClear(vehicle.Deleters[i].ToVector3(), 2f))
								foreach (var veh in Funzioni.GetVehiclesInArea(vehicle.Deleters[i].ToVector3(), 2f))
									if (!veh.HasDecor("VeicoloMedici") && !veh.HasDecor("VeicoloPolizia"))
										veh.Delete();
							if (Game.PlayerPed.IsInVehicle())
							{
								World.DrawMarker(MarkerType.CarSymbol, vehicle.Deleters[i].ToVector3(), new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Red, false, false, true);
								if (World.GetDistance(Game.PlayerPed.Position, vehicle.Deleters[i].ToVector3()) < 2f)
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");
									if (Game.IsControlJustPressed(0, Control.Context))
									{
										if (Game.PlayerPed.CurrentVehicle.HasDecor("VeicoloMedici"))
										{
											VeicoloPol vehicl = new VeicoloPol(Game.PlayerPed.CurrentVehicle.Mods.LicensePlate, Game.PlayerPed.CurrentVehicle.Model.Hash, Game.PlayerPed.CurrentVehicle.Handle);
											BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehMedici", JsonConvert.SerializeObject(vehicl));
											Game.PlayerPed.CurrentVehicle.Delete();
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
						if (!Game.PlayerPed.IsInVehicle())
							World.DrawMarker(MarkerType.HelicopterSymbol, heli.SpawnerMenu.ToVector3(), new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Cyan, false, false, true);
						if (World.GetDistance(Game.PlayerPed.Position, heli.SpawnerMenu.ToVector3()) < 1.5f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere il veicolo");
							if (Game.IsControlJustPressed(0, Control.Context))
							{
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								MenuMedici.HeliMenu(osp, heli);
							}
						}
						for (int i = 0; i < heli.Deleters.Count; i++)
						{
							if (!Funzioni.IsSpawnPointClear(heli.Deleters[i].ToVector3(), 2f))
								foreach (var veh in Funzioni.GetVehiclesInArea(heli.Deleters[i].ToVector3(), 2f))
									if (!veh.HasDecor("VeicoloMedici") && !veh.HasDecor("VeicoloPolizia"))
										veh.Delete();
							if (Game.PlayerPed.IsInVehicle())
							{
								World.DrawMarker(MarkerType.HelicopterSymbol, heli.Deleters[i].ToVector3(), new Vector3(0), new Vector3(0), new Vector3(3f, 3f, 1.5f), Colors.Red, false, false, true);
								if (World.GetDistance(Game.PlayerPed.Position, heli.Deleters[i].ToVector3()) < 2f)
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");
									if (Game.IsControlJustPressed(0, Control.Context))
									{
										if (Game.PlayerPed.CurrentVehicle.HasDecor("VeicoloMedici"))
										{
											VeicoloPol veh = new VeicoloPol(Game.PlayerPed.CurrentVehicle.Mods.LicensePlate, Game.PlayerPed.CurrentVehicle.Model.Hash, Game.PlayerPed.CurrentVehicle.Handle);
											BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehMedici", JsonConvert.SerializeObject(veh));
											Game.PlayerPed.CurrentVehicle.Delete();
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
			if (Eventi.Player.CurrentChar.job.name.ToLower() != "medico" || Eventi.Player.CurrentChar.job.name.ToLower() != "medici")
			{
				foreach (var osp in ConfigClient.Conf.Lavori.Medici.Config.Ospedali)
				{
					foreach (float[] vettore in osp.IngressoVisitatori)
					{
						if (World.GetDistance(Game.PlayerPed.Position, vettore.ToVector3()) < 1.375f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare in ospedale");
							if (Game.IsControlJustPressed(0, Control.Context))
							{
								Vector3 pos;
								if (osp.IngressoVisitatori.IndexOf(vettore) == 0 || osp.IngressoVisitatori.IndexOf(vettore) == 1)
									pos = new Vector3(272.8f, -1358.8f, 23.5f);
								else
									pos = new Vector3(254.301f, -1372.288f, 24.538f);

								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								Game.PlayerPed.Position = pos;
								Screen.Fading.FadeIn(800);
							}
						}
					}

					foreach (float[] vettore in osp.UscitaVisitatori)
					{
						if (World.GetDistance(Game.PlayerPed.Position, vettore.ToVector3()) < 1.375f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ospedale");
							if (Game.IsControlJustPressed(0, Control.Context))
							{
								Vector3 pos;
								if (osp.UscitaVisitatori.IndexOf(vettore) == 0)
									pos = osp.IngressoVisitatori[0].ToVector3();
								else
									pos = osp.IngressoVisitatori[2].ToVector3();

								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								Game.PlayerPed.Position = pos;
								Screen.Fading.FadeIn(800);
							}
						}
					}
				}
			}
		}
	}
}
