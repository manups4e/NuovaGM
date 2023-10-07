using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà.Appartamenti.Case;

using TheLastPlanet.Shared.Vehicles;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà
{
    internal static class Manager
    {
        private static ConfigPropertiesRP Proprietà;
        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }
        public static void Spawnato(PlayerClient client)
        {
            Proprietà = Client.Impostazioni.RolePlay.Properties;
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            Proprietà = null;
        }

        public static async Task MarkerFuori()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            foreach (KeyValuePair<string, ConfigHouses> app in Proprietà.Apartments)
            {
                if (playerPed.IsInRangeOf(app.Value.MarkerEntrance.ToVector3, 1.375f))
                {
                    HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~entrare o citofonare~w~.");
                    if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) AppartamentiClient.EntraMenu(app); // da fare e agg. controllo se è casa mia o no per il menu
                }

                if (!playerPed.IsInRangeOf(app.Value.MarkerGarageExtern.ToVector3, 3f)) continue;
                if (!Cache.PlayerCache.MyPlayer.User.CurrentChar.Properties.Contains(app.Key)) continue;

                if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                {
                    string plate = playerPed.CurrentVehicle.Mods.LicensePlate;
                    int model = playerPed.CurrentVehicle.Model.Hash;

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Vehicles.FirstOrDefault(x => x.Plate == plate && x.VehData.Props.Model == model && x.VehData.Insurance == Cache.PlayerCache.MyPlayer.User.CurrentChar.Info.Insurance) == null) continue;
                    if (playerPed.IsVisible) NetworkFadeOutEntity(playerPed.CurrentVehicle.Handle, true, false);
                    Screen.Fading.FadeOut(500);
                    await BaseScript.Delay(1000);
                    VehProp pr = await playerPed.CurrentVehicle.GetVehicleProperties();
                    BaseScript.TriggerServerEvent("lprp:vehInGarage", plate, true, pr.ToJson(settings: JsonHelper.IgnoreJsonIgnoreAttributes));
                    Cache.PlayerCache.MyPlayer.Status.Instance.Istanzia(app.Key);
                    await BaseScript.Delay(1000);

                    if (playerPed.CurrentVehicle.PassengerCount > 0)
                        foreach (Ped p in playerPed.CurrentVehicle.Passengers)
                        {
                            Player pl = Funzioni.GetPlayerFromPed(p);
                            var pp = Funzioni.GetPlayerClientFromServerId(pl.ServerId);
                            pp.Status.Instance.Istanzia(Cache.PlayerCache.MyPlayer.Player.ServerId, Cache.PlayerCache.MyPlayer.Status.Instance.Instance);
                            BaseScript.TriggerServerEvent("lprp:entraGarageConProprietario", pl.ServerId, app.Value.SpawnGarageWalkInside);
                        }

                    playerPed.CurrentVehicle.Delete();
                    RequestCollisionAtCoord(app.Value.SpawnGarageWalkInside.X, app.Value.SpawnGarageWalkInside.Y, app.Value.SpawnGarageWalkInside.Z);
                    NewLoadSceneStart(app.Value.SpawnGarageWalkInside.X, app.Value.SpawnGarageWalkInside.Y, app.Value.SpawnGarageWalkInside.Z, app.Value.SpawnGarageWalkInside.X, app.Value.SpawnGarageWalkInside.Y, app.Value.SpawnGarageWalkInside.Z, 50f, 0);
                    int tempTimer = GetGameTimer();

                    // Wait for the new scene to be loaded.
                    while (IsNetworkLoadingScene())
                    {
                        // If this takes longer than 1 second, just abort. It's not worth waiting that long.
                        if (GetGameTimer() - tempTimer > 1000)
                        {
                            Client.Logger.Debug("Waiting for the scene to load is taking too long (more than 1s). Breaking from wait loop.");

                            break;
                        }

                        await BaseScript.Delay(0);
                    }

                    SetEntityCoords(PlayerPedId(), app.Value.SpawnGarageWalkInside.X, app.Value.SpawnGarageWalkInside.Y, app.Value.SpawnGarageWalkInside.Z, false, false, false, false);
                    tempTimer = GetGameTimer();

                    // Wait for the collision to be loaded around the entity in this new location.
                    while (!HasCollisionLoadedAroundEntity(playerPed.Handle))
                    {
                        // If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
                        if (GetGameTimer() - tempTimer > 1000)
                        {
                            Client.Logger.Debug("Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");

                            break;
                        }

                        await BaseScript.Delay(0);
                    }

                    foreach (OwnedVehicle veh in Cache.PlayerCache.MyPlayer.User.CurrentChar.Vehicles.Where(veh => veh.Garage.Garage == Cache.PlayerCache.MyPlayer.Status.Instance.Instance).Where(veh => veh.Garage.InGarage))
                    {
                        Vehicle veic = await Funzioni.SpawnLocalVehicle(veh.VehData.Props.Model, new Vector3(Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].X, Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Y, Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Z), Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Heading);
                        await veic.SetVehicleProperties(veh.VehData.Props);
                        AppartamentiClient.VeicoliParcheggio.Add(veic);
                    }

                    NetworkFadeInEntity(playerPed.Handle, true);
                    playerPed.IsPositionFrozen = false;
                    DoScreenFadeIn(500);
                    SetGameplayCamRelativePitch(0.0f, 1.0f);
                    Client.Instance.AddTick(AppartamentiClient.Garage);
                }
                else
                {
                    // codice per entrare a piedi.. lo vogliamo far entrare a piedi?
                }
            }

            foreach (KeyValuePair<string, Garages> gar in Proprietà.Garages.Garages.Where(gar => playerPed.IsInRangeOf(gar.Value.MarkerEntrance.ToVector3, 1.5f)).Where(gar => playerPed.IsOnFoot))
            {
                // ENTRARE NEI GARAGES
            }
        }

        public static async Task MarkerDentro()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.Instance.Instanced)
                if (Proprietà.Apartments.ContainsKey(Cache.PlayerCache.MyPlayer.Status.Instance.Instance))
                {
                    ConfigHouses app = Proprietà.Apartments[Cache.PlayerCache.MyPlayer.Status.Instance.Instance];

                    if (playerPed.IsInRangeOf(app.MarkerExit.ToVector3, 1.375f))
                    {
                        HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~uscire~w~.");
                        if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) AppartamentiClient.EsciMenu(app);
                    }

                    if (playerPed.IsInRangeOf(app.MarkerGarageInternal.ToVector3, 1.375f))
                    {
                        HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~uscire~w~.");
                        if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) AppartamentiClient.EsciMenu(app, true);
                    }
                }
        }
    }
}