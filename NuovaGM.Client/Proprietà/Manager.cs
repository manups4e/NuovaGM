using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.Proprietà.Appartamenti.Case;
using CitizenFX.Core.UI;

namespace TheLastPlanet.Client.Proprietà
{
	static class Manager
	{
		private static ConfigProprieta Proprietà;
		public static void Init()
		{
			Proprietà = Client.Impostazioni.Proprieta;
		}

		public static async Task MarkerFuori()
		{
			Ped playerPed = new Ped(PlayerPedId());
			foreach (var app in Proprietà.Appartamenti)
			{
				if (playerPed.IsInRangeOf(app.Value.MarkerEntrata, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~entrare o citofonare~w~.");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
						AppartamentiClient.EntraMenu(app); // da fare e agg. controllo se è casa mia o no per il menu
				}
				if (playerPed.IsInRangeOf(app.Value.MarkerGarageEsterno, 3f))
				{
					if (Eventi.Player.CurrentChar.Proprietà.Contains(app.Key))
					{
						if (playerPed.IsInVehicle())
						{
							string plate = playerPed.CurrentVehicle.Mods.LicensePlate;
							var model = playerPed.CurrentVehicle.Model.Hash;
							if (Eventi.Player.CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate && x.DatiVeicolo.props.Model == model && x.DatiVeicolo.Assicurazione == Eventi.Player.CurrentChar.info.insurance) != null)
							{
								if (playerPed.IsVisible)
									NetworkFadeOutEntity(playerPed.CurrentVehicle.Handle, true, false);
								Screen.Fading.FadeOut(500);
								await BaseScript.Delay(1000);
								var pr = await playerPed.CurrentVehicle.GetVehicleProperties();
								BaseScript.TriggerServerEvent("lprp:vehInGarage", plate, true, pr.Serialize(includeEverything: true));
								Eventi.Player.StatiPlayer.Istanza.Istanzia(app.Key);
								await BaseScript.Delay(1000);
								if (playerPed.CurrentVehicle.PassengerCount > 0)
								{
									foreach (var p in playerPed.CurrentVehicle.Passengers)
									{
										var pl = Funzioni.GetPlayerFromPed(p);
										pl.GetPlayerData().StatiPlayer.Istanza.Istanzia(Game.Player.ServerId, Eventi.Player.StatiPlayer.Istanza.Instance);
										BaseScript.TriggerServerEvent("lprp:entraGarageConProprietario", pl.ServerId, app.Value.SpawnGarageAPiediDentro);
									}
								}
								playerPed.CurrentVehicle.Delete();
								RequestCollisionAtCoord(app.Value.SpawnGarageAPiediDentro.X, app.Value.SpawnGarageAPiediDentro.Y, app.Value.SpawnGarageAPiediDentro.Z);
								NewLoadSceneStart(app.Value.SpawnGarageAPiediDentro.X, app.Value.SpawnGarageAPiediDentro.Y, app.Value.SpawnGarageAPiediDentro.Z, app.Value.SpawnGarageAPiediDentro.X, app.Value.SpawnGarageAPiediDentro.Y, app.Value.SpawnGarageAPiediDentro.Z, 50f, 0);
								int tempTimer = GetGameTimer();

								// Wait for the new scene to be loaded.
								while (IsNetworkLoadingScene())
								{
									// If this takes longer than 1 second, just abort. It's not worth waiting that long.
									if (GetGameTimer() - tempTimer > 1000)
									{
										Log.Printa(LogType.Debug, "Waiting for the scene to load is taking too long (more than 1s). Breaking from wait loop.");
										break;
									}
									await BaseScript.Delay(0);
								}
								SetEntityCoords(PlayerPedId(), app.Value.SpawnGarageAPiediDentro.X, app.Value.SpawnGarageAPiediDentro.Y, app.Value.SpawnGarageAPiediDentro.Z, false, false, false, false);
								tempTimer = GetGameTimer();

								// Wait for the collision to be loaded around the entity in this new location.
								while (!HasCollisionLoadedAroundEntity(playerPed.Handle))
								{
									// If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
									if (GetGameTimer() - tempTimer > 1000)
									{
										Log.Printa(LogType.Debug, "Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");
										break;
									}
									await BaseScript.Delay(0);
								}
								foreach (var veh in Eventi.Player.CurrentChar.Veicoli)
								{
									if (veh.Garage.Garage == Eventi.Player.StatiPlayer.Istanza.Instance)
									{
										if (veh.Garage.InGarage)
										{
											var veic = await Funzioni.SpawnLocalVehicle(veh.DatiVeicolo.props.Model, new Vector3(Client.Impostazioni.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].X, Client.Impostazioni.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].Y, Client.Impostazioni.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].Z), Client.Impostazioni.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].W);
											await veic.SetVehicleProperties(veh.DatiVeicolo.props);
											AppartamentiClient.VeicoliParcheggio.Add(veic);
										}
									}
								}
								NetworkFadeInEntity(playerPed.Handle, true);
								playerPed.IsPositionFrozen = false;
								DoScreenFadeIn(500);
								SetGameplayCamRelativePitch(0.0f, 1.0f);
								Client.Instance.AddTick(AppartamentiClient.Garage);
							}
						}
						else
						{
							// codice per entrare a piedi.. lo vogliamo far entrare a piedi?
						}
					}
				}
			}
			foreach (var gar in Proprietà.Garages.Garages)
			{
				if(playerPed.IsInRangeOf(gar.Value.MarkerEntrata, 1.5f))
				{
					if (playerPed.IsOnFoot)
					{
						// ENTRARE NEI GARAGES
					}
				}
			}
		}

		public static async Task MarkerDentro()
		{
			Ped playerPed = new Ped(PlayerPedId());
			if (Eventi.Player.StatiPlayer.Istanza.Stanziato)
			{
				if (Proprietà.Appartamenti.ContainsKey(Eventi.Player.StatiPlayer.Istanza.Instance))
				{
					var app = Proprietà.Appartamenti[Eventi.Player.StatiPlayer.Istanza.Instance];
					if (playerPed.IsInRangeOf(app.MarkerUscita, 1.375f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~uscire~w~.");
						if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
							AppartamentiClient.EsciMenu(app);
					}
					if (playerPed.IsInRangeOf(app.MarkerGarageInterno, 1.375f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~uscire~w~.");
						if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
							AppartamentiClient.EsciMenu(app, inGarage: true);
					}
				}
			}
		}


	}
}
