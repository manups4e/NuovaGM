using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Properties.Appartamenti.Case;

using TheLastPlanet.Shared.Vehicles;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Properties
{
    internal static class Manager
    {
        private static ConfigPropertiesRP Properties;
        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }
        public static void Spawned(PlayerClient client)
        {
            Properties = Client.Settings.RolePlay.Properties;
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            Properties = null;
        }

        public static async Task MarkerOutside()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            foreach (KeyValuePair<string, ConfigHouses> app in Properties.Apartments)
            {
                if (playerPed.IsInRangeOf(app.Value.MarkerEntrance.ToVector3, 1.375f))
                {
                    HUD.ShowHelp("Press ~INPUT_CONTEXT~ to ~y~enter or buzz~w~.");
                    if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen)
                        ApartmentsClient.EnterMenu(app);
                    // TODO: TO BE ADDED A CHECK IF IT'S MY HOUSE OR NOT IN THE MENU AND LET THE PLAYER DECIDE WHAT TO DO
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
                    Cache.PlayerCache.MyPlayer.Status.Instance.InstancePlayer(app.Key);
                    await BaseScript.Delay(1000);

                    if (playerPed.CurrentVehicle.PassengerCount > 0)
                    {
                        foreach (Ped p in playerPed.CurrentVehicle.Passengers)
                        {
                            Player pl = Functions.GetPlayerFromPed(p);
                            PlayerClient pp = Functions.GetPlayerClientFromServerId(pl.ServerId);
                            pp.Status.Instance.Istanzia(Cache.PlayerCache.MyPlayer.Player.ServerId, Cache.PlayerCache.MyPlayer.Status.Instance.Instance);
                            BaseScript.TriggerServerEvent("lprp:entraGarageConProprietario", pl.ServerId, app.Value.SpawnGarageWalkInside);
                        }
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
                        Vehicle veic = await Functions.SpawnLocalVehicle(veh.VehData.Props.Model, new Vector3(Client.Settings.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].X, Client.Settings.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Y, Client.Settings.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Z), Client.Settings.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Heading);
                        await veic.SetVehicleProperties(veh.VehData.Props);
                        ApartmentsClient.VeicoliParcheggio.Add(veic);
                    }

                    NetworkFadeInEntity(playerPed.Handle, true);
                    playerPed.IsPositionFrozen = false;
                    DoScreenFadeIn(500);
                    SetGameplayCamRelativePitch(0.0f, 1.0f);
                    Client.Instance.AddTick(ApartmentsClient.Garage);
                }
                else
                {
                    // TODO: CODE FOR ENTERING BY FOOT, DO WE WANT THE PLAYER TO ENTER BY FOOT HERE?
                }
            }

            foreach (KeyValuePair<string, Garages> gar in Properties.Garages.Garages.Where(gar => playerPed.IsInRangeOf(gar.Value.MarkerEntrance.ToVector3, 1.5f)).Where(gar => playerPed.IsOnFoot))
            {
                // TODO: GET IN THE GARAGES
            }
        }

        public static async Task MarkerInside()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.Instance.Instanced)
            {
                if (Properties.Apartments.ContainsKey(Cache.PlayerCache.MyPlayer.Status.Instance.Instance))
                {
                    ConfigHouses app = Properties.Apartments[Cache.PlayerCache.MyPlayer.Status.Instance.Instance];

                    if (playerPed.IsInRangeOf(app.MarkerExit.ToVector3, 1.375f) || playerPed.IsInRangeOf(app.MarkerGarageInternal.ToVector3, 1.375f))
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to ~y~exit~w~.");
                        if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) ApartmentsClient.ExitMenu(app, playerPed.IsInRangeOf(app.MarkerGarageInternal.ToVector3, 1.375f));
                    }
                }
            }
        }
    }
}