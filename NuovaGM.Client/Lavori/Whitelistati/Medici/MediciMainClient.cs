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

namespace NuovaGM.Client.Lavori.Whitelistati.Medici
{
	static class MediciMainClient
	{
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
			Client.GetInstance.RegisterTickHandler(MarkersMedici);
			Client.GetInstance.RegisterTickHandler(MarkersNonMedici);
		}

		public static async Task MarkersMedici()
		{
			if (Eventi.Player.CurrentChar.job.name.ToLower() == "medico" || Eventi.Player.CurrentChar.job.name.ToLower() == "medici")
			{
				foreach (var osp in ConfigClient.Conf.Lavori.Medici.Config.Ospedali)
				{
					foreach (float[] vettore in osp.Spogliatoio)
					{
						if (World.GetDistance(Game.PlayerPed.Position, vettore.ToVector3()) < 3f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~entrare~w~ / ~b~uscire~w~ in servizio");
							// Menu spogliatoio
						}
					}

					foreach (float[] vettore in osp.Farmacia)
					{
						if (World.GetDistance(Game.PlayerPed.Position, vettore.ToVector3()) < 3f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per usare la farmacia");
							// Menu farmacia
						}
					}

					foreach (float[] vettore in osp.IngressoVisitatori)
					{
						if (World.GetDistance(Game.PlayerPed.Position, vettore.ToVector3()) < 1.375f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare in ospedale");
							// Ingresso visitatori
						}
					}

					foreach (var vehicle in osp.Veicoli)
					{
						if (!Game.PlayerPed.IsInVehicle())
							World.DrawMarker(MarkerType.CarSymbol, vehicle.SpawnerMenu.ToVector3(), new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Cyan, false, false, true);
						if (World.GetDistance(Game.PlayerPed.Position, vehicle.SpawnerMenu.ToVector3()) < 3f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere il veicolo");
							if (Game.IsControlJustPressed(0, Control.Context))
							{
								// Menu spawn veicoli
							}
						}
						for (int i = 0; i < vehicle.Deleters.Count; i++)
						{
							if (Game.PlayerPed.IsInVehicle())
							{
								World.DrawMarker(MarkerType.CarSymbol, vehicle.Deleters[i].ToVector3(), new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Red, false, false, true);
								if (World.GetDistance(Game.PlayerPed.Position, vehicle.Deleters[i].ToVector3()) < 3f)
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");
									if (Game.IsControlJustPressed(0, Control.Context))
									{
										if (Game.PlayerPed.CurrentVehicle.HasDecor("VeicoloMedici"))
										{
											// poso il veicolo
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
						if (World.GetDistance(Game.PlayerPed.Position, heli.SpawnerMenu.ToVector3()) < 3f)
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere il veicolo");
							if (Game.IsControlJustPressed(0, Control.Context))
							{
								// Menu spawn veicoli
							}
						}
						for (int i = 0; i < heli.Deleters.Count; i++)
						{
							if (Game.PlayerPed.IsInVehicle())
							{
								World.DrawMarker(MarkerType.HelicopterSymbol, heli.Deleters[i].ToVector3(), new Vector3(0), new Vector3(0), new Vector3(3f, 3f, 1.5f), Colors.Red, false, false, true);
								if (World.GetDistance(Game.PlayerPed.Position, heli.Deleters[i].ToVector3()) < 3f)
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");
									if (Game.IsControlJustPressed(0, Control.Context))
									{
										if (Game.PlayerPed.CurrentVehicle.HasDecor("VeicoloMedici"))
										{
											// poso il veicolo
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
							// ingresso visitatori
						}
					}
				}
			}
		}
	}
}
