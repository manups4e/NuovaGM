using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using TheLastPlanet.Shared.Vehicles;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà.Appartamenti.Case
{
    internal static class AppartamentiClient
    {
        public static List<Vehicle> VeicoliParcheggio = new List<Vehicle>();

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }
        public static void Spawnato(PlayerClient client)
        {
            Client.Instance.AddEventHandler("lprp:richiestaDiEntrare", new Action<int, string>(Richiesta));
            Client.Instance.AddEventHandler("lprp:citofono:puoiEntrare", new Action<int, string>(PuoiEntrare));
            Client.Instance.AddEventHandler("lprp:entraGarageConProprietario", new Action<Vector3>(EntraGarageConProprietario));
            Client.Instance.AddEventHandler("lprp:housedealer:caricaImmobiliDaDB", new Action<string, string>(CaricaCaseDaDb));
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("lprp:richiestaDiEntrare", new Action<int, string>(Richiesta));
            Client.Instance.RemoveEventHandler("lprp:citofono:puoiEntrare", new Action<int, string>(PuoiEntrare));
            Client.Instance.RemoveEventHandler("lprp:entraGarageConProprietario", new Action<Vector3>(EntraGarageConProprietario));
            Client.Instance.RemoveEventHandler("lprp:housedealer:caricaImmobiliDaDB", new Action<string, string>(CaricaCaseDaDb));
            Client.Impostazioni.RolePlay.Properties.Apartments.Clear();
            Client.Impostazioni.RolePlay.Properties.Garages.Garages.Clear();
        }

        private static async void CaricaCaseDaDb(string JsonCase, string jsonGarage)
        {
            Dictionary<string, string> aparts = JsonCase.FromJson<Dictionary<string, string>>();
            Dictionary<string, string> garages = jsonGarage.FromJson<Dictionary<string, string>>();
            foreach (KeyValuePair<string, string> a in aparts) Client.Impostazioni.RolePlay.Properties.Apartments.Add(a.Key, a.Value.FromJson<ConfigHouses>());
            foreach (KeyValuePair<string, string> a in garages) Client.Impostazioni.RolePlay.Properties.Garages.Garages.Add(a.Key, a.Value.FromJson<Garages>());
        }

        public static async void EntraMenu(KeyValuePair<string, ConfigHouses> app)
        {
            Camera dummycam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
            World.RenderingCamera = dummycam;
            Camera cam = World.CreateCamera(app.Value.CameraOutside.Pos.ToVector3, new Vector3(0), GameplayCamera.FieldOfView);
            cam.PointAt(app.Value.CameraOutside.Rotation.ToVector3);
            RenderScriptCams(true, true, 1500, true, false);
            dummycam.InterpTo(cam, 1500, 1, 1);
            UIMenu casa = new UIMenu(app.Value.Label, "Appartamenti", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem CitofonaItem = new UIMenuItem("Citofona ai residenti");
            UIMenu Citofona = new("Citofona ai residenti", "");
            CitofonaItem.BindItemToMenu(Citofona);
            casa.AddItem(CitofonaItem);
            UIMenuItem entra;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Properties.Contains(app.Key))
            {
                entra = new UIMenuItem("Entra in casa");
                casa.AddItem(entra);
                entra.Activated += async (_submenu, _subitem) =>
                {
                    Cache.PlayerCache.MyPlayer.Status.Instance.Istanzia(app.Key);
                    Screen.Fading.FadeOut(500);
                    while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
                    MenuHandler.CloseAndClearHistory();

                    while (cam.IsActive && cam.Exists() && cam != null)
                    {
                        RenderScriptCams(false, false, 1500, true, false);
                        World.RenderingCamera = null;
                        cam.IsActive = false;
                        cam.Delete();
                    }

                    RequestCollisionAtCoord(app.Value.SpawnInside.X, app.Value.SpawnInside.Y, app.Value.SpawnInside.Z);
                    Cache.PlayerCache.MyPlayer.Ped.Position = app.Value.SpawnInside.ToVector3;
                    while (!HasCollisionLoadedAroundEntity(PlayerPedId())) await BaseScript.Delay(1000);
                    await BaseScript.Delay(2000);
                    Screen.Fading.FadeIn(500);
                    NetworkFadeInEntity(PlayerPedId(), true);
                };
            }

            Citofona.OnMenuOpen += (_menu, b) =>
            {
                _menu.Clear();
                List<PlayerClient> gioc = (from p in Client.Instance.Clients.ToList() where p != PlayerCache.MyPlayer let pl = p where pl.Status.Instance.Instanced where pl.Status.Instance.IsOwner where pl.Status.Instance.Instance == app.Key select p).ToList();

                if (gioc.Count > 0)
                {
                    foreach (PlayerClient p in gioc.ToList())
                    {
                        UIMenuItem it = new(p.User.FullName);
                        _menu.AddItem(it);
                        it.Activated += (_submenu, _subitem) =>
                        {
                            Game.PlaySound("DOOR_BUZZ", "MP_PLAYER_APARTMENT");
                            BaseScript.TriggerServerEvent("lprp:citofonaAlPlayer", p.Handle.ToString(), app.ToJson()); // params: personaincasa.serverid, fromsource chi suona
                            MenuHandler.CloseAndClearHistory();
                        };
                    }
                }
                else
                    _menu.AddItem(new UIMenuItem("Non ci sono persone in casa al momento!"));
            };

            casa.OnMenuClose += async (a) =>
            {
                await BaseScript.Delay(100);

                if (MenuHandler.IsAnyMenuOpen) return;
                if (cam.IsActive) RenderScriptCams(false, true, 1500, true, false);
                dummycam.Delete();
                cam.Delete();
            };

            while (dummycam.IsInterpolating) await BaseScript.Delay(0);
            while (cam.IsInterpolating) await BaseScript.Delay(0);
            casa.Visible = true;
        }

        public static async void EsciMenu(ConfigHouses app, bool inGarage = false, bool inTetto = false)
        {
            UIMenu esci = new UIMenu(app.Label, "Appartamenti", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem escisci = new UIMenuItem("Esci dall'appartamento");
            esci.AddItem(escisci);
            UIMenuItem garage = new UIMenuItem("", "");
            UIMenuItem tetto = new UIMenuItem("", "");
            UIMenuItem casa = new UIMenuItem("", "");

            if (inGarage || inTetto)
            {
                casa = new UIMenuItem("Entra in casa");
                esci.AddItem(casa);
            }

            if (app.GarageIncluded && !inGarage)
            {
                garage = new UIMenuItem("Vai al garage");
                esci.AddItem(garage);
            }

            if (app.HasRoof && !inTetto)
            {
                tetto = new UIMenuItem("Vai sul tetto");
                esci.AddItem(tetto);
            }

            esci.OnItemSelect += async (_menu, _item, _index) =>
            {
                MenuHandler.CloseAndClearHistory();
                if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerPedId(), true, false);
                Screen.Fading.FadeOut(500);
                while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);

                if (_item == escisci)
                {
                    Funzioni.Teleport(app.SpawnOutside.ToVector3);
                    Cache.PlayerCache.MyPlayer.Status.Instance.RimuoviIstanza();
                }
                else if (_item == casa)
                {
                    Funzioni.Teleport(app.SpawnInside.ToVector3);
                }
                else if (_item == garage)
                {
                    ClearPedTasksImmediately(Cache.PlayerCache.MyPlayer.Ped.Handle);
                    Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;
                    if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerPedId(), true, false);
                    DoScreenFadeOut(500);
                    while (!IsScreenFadedOut()) await BaseScript.Delay(0);
                    RequestCollisionAtCoord(app.SpawnGarageWalkInside.X, app.SpawnGarageWalkInside.Y, app.SpawnGarageWalkInside.Z);
                    NewLoadSceneStart(app.SpawnGarageWalkInside.X, app.SpawnGarageWalkInside.Y, app.SpawnGarageWalkInside.Z, app.SpawnGarageWalkInside.X, app.SpawnGarageWalkInside.Y, app.SpawnGarageWalkInside.Z, 50f, 0);
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

                    SetEntityCoords(PlayerPedId(), app.SpawnGarageWalkInside.X, app.SpawnGarageWalkInside.Y, app.SpawnGarageWalkInside.Z, false, false, false, false);
                    tempTimer = GetGameTimer();

                    // Wait for the collision to be loaded around the entity in this new location.
                    while (!HasCollisionLoadedAroundEntity(Cache.PlayerCache.MyPlayer.Ped.Handle))
                    {
                        // If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
                        if (GetGameTimer() - tempTimer > 1000)
                        {
                            Client.Logger.Debug("Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");

                            break;
                        }

                        await BaseScript.Delay(0);
                    }

                    foreach (OwnedVehicle veh in Cache.PlayerCache.MyPlayer.User.CurrentChar.Vehicles)
                        if (veh.Garage.Garage == Cache.PlayerCache.MyPlayer.Status.Instance.Instance)
                            if (veh.Garage.InGarage)
                            {
                                Vehicle veic = await Funzioni.SpawnLocalVehicle(veh.VehData.Props.Model, new Vector3(Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].X, Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Y, Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Z), Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Heading);
                                await veic.SetVehicleProperties(veh.VehData.Props);
                                VeicoliParcheggio.Add(veic);
                            }

                    NetworkFadeInEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true);
                    Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
                    DoScreenFadeIn(500);
                    SetGameplayCamRelativePitch(0.0f, 1.0f);
                    Client.Instance.AddTick(Garage);
                }
                else if (_item == tetto)
                {
                    Funzioni.Teleport(app.SpawnRoof.ToVector3);
                    Cache.PlayerCache.MyPlayer.Status.Instance.RimuoviIstanza();
                }

                await BaseScript.Delay(2000);
                Screen.Fading.FadeIn(500);
                NetworkFadeInEntity(PlayerPedId(), true);
            };
            esci.Visible = true;
        }

        private static string nome;
        private static string appa;
        private static int serverIdRic;
        private static int tempo;

        public static void Richiesta(int serverIdRichiedente, string app)
        {
            Game.PlaySound("DOOR_BUZZ", "MP_PLAYER_APARTMENT");
            nome = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == serverIdRichiedente).GetPlayerData().FullName;
            appa = app;
            serverIdRic = serverIdRichiedente;
            tempo = GetGameTimer();
            Client.Instance.AddTick(AccRif);
        }

        private static async Task AccRif()
        {
            HUD.ShowHelp($"{nome} ti ha citofonato.\n~INPUT_VEH_EXIT~ per accettare");

            if (GetGameTimer() - tempo < 30000)
            {
                if (Input.IsControlJustPressed(Control.VehicleExit))
                {
                    BaseScript.TriggerServerEvent("lprp:citofono:puoEntrare", serverIdRic, appa);
                    Client.Instance.RemoveTick(AccRif);
                    nome = null;
                    appa = null;
                    serverIdRic = 0;
                    tempo = 0;
                }
            }
            else
            {
                Client.Instance.RemoveTick(AccRif);
                nome = null;
                appa = null;
                serverIdRic = 0;
                tempo = 0;
            }
        }

        public static void PuoiEntrare(int serverIdInCasa, string appartamento)
        {
            KeyValuePair<string, ConfigHouses> app = appartamento.FromJson<KeyValuePair<string, ConfigHouses>>();
            Player InCasa = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == serverIdInCasa);

            if (InCasa == null) return;
            if (!Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(app.Value.MarkerEntrance.ToVector3, 3f)) return;
            if (Cache.PlayerCache.MyPlayer.Status.Instance.Instanced) return;
            Cache.PlayerCache.MyPlayer.Status.Instance.Istanzia(InCasa.ServerId, app.Key);
            Funzioni.Teleport(app.Value.SpawnInside.ToVector3);
        }

        public static async Task Garage()
        {
            if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.ModifyMarker.ToVector3, 1.375f))
            {
                // gestire
            }

            if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(Client.Impostazioni.RolePlay.Properties.Garages.MidEnd4.ModifyMarker.ToVector3, 1.375f))
            {
                // gestire
            }

            if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(Client.Impostazioni.RolePlay.Properties.Garages.MidEnd6.ModifyMarker.ToVector3, 1.375f))
            {
                // gestire
            }

            if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(Client.Impostazioni.RolePlay.Properties.Garages.HighEnd.ModifyMarker.ToVector3, 1.375f))
            {
                // gestire
            }

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
            {
                HUD.ShowHelp("Per selezionare questo veicolo e uscire~n~~y~Accendi il motore~w~ e ~y~accelera~w~.");

                if (Input.IsControlJustPressed(Control.VehicleAccelerate) && Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsEngineRunning)
                {
                    Screen.Fading.FadeOut(800);
                    await BaseScript.Delay(1000);
                    string plate = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate;
                    foreach (Vehicle vehicle in VeicoliParcheggio) vehicle.Delete();
                    VeicoliParcheggio.Clear();
                    Vector4 exit = Vector4.Zero;
                    if (Client.Impostazioni.RolePlay.Properties.Apartments.ContainsKey(Cache.PlayerCache.MyPlayer.Status.Instance.Instance))
                        exit = Client.Impostazioni.RolePlay.Properties.Apartments[Cache.PlayerCache.MyPlayer.Status.Instance.Instance].SpawnGarageVehicleOutside.ToVector4;
                    else
                        exit = Client.Impostazioni.RolePlay.Properties.Garages.Garages[Cache.PlayerCache.MyPlayer.Status.Instance.Instance].SpawnOutside.ToVector4;
                    int tempo = GetGameTimer();
                    Vector3 newPos = exit.ToVector3();
                    float Head = exit.W;

                    while (!Funzioni.IsSpawnPointClear(exit.ToVector3(), 2f))
                    {
                        if (GetGameTimer() - tempo > 5000)
                        {
                            Client.Logger.Debug("Punto di spawn fuori dal garage occupato, trovato nuovo punto");

                            break;
                        }

                        await BaseScript.Delay(0);
                    }

                    if (!Funzioni.IsSpawnPointClear(exit.ToVector3(), 2f)) GetClosestVehicleNodeWithHeading(exit.X, exit.Y, exit.Z, ref newPos, ref Head, 1, 3, 0);
                    Vehicle vehi = await Funzioni.SpawnVehicle(Cache.PlayerCache.MyPlayer.User.CurrentChar.Vehicles.FirstOrDefault(x => x.Plate == plate).VehData.Props.Model, newPos, Head);
                    await vehi.SetVehicleProperties(Cache.PlayerCache.MyPlayer.User.CurrentChar.Vehicles.FirstOrDefault(x => x.Plate == plate).VehData.Props);
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsEngineRunning = true;
                    Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsDriveable = true;
                    BaseScript.TriggerServerEvent("lprp:vehInGarage", plate, false);
                    Cache.PlayerCache.MyPlayer.Status.Instance.RimuoviIstanza();
                    await BaseScript.Delay(1000);
                    Screen.Fading.FadeIn(800);
                    Client.Instance.RemoveTick(Garage);
                }
            }
        }

        private static async void EntraGarageConProprietario(Vector3 pos)
        {
            if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle, true, false);
            Screen.Fading.FadeOut(500);
            await BaseScript.Delay(1000);
            RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
            NewLoadSceneStart(pos.X, pos.Y, pos.Z, pos.X, pos.Y, pos.Z, 50f, 0);
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

            SetEntityCoords(PlayerPedId(), pos.X, pos.Y, pos.Z, false, false, false, false);
            tempTimer = GetGameTimer();

            // Wait for the collision to be loaded around the entity in this new location.
            while (!HasCollisionLoadedAroundEntity(Cache.PlayerCache.MyPlayer.Ped.Handle))
            {
                // If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
                if (GetGameTimer() - tempTimer > 1000)
                {
                    Client.Logger.Debug("Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");

                    break;
                }

                await BaseScript.Delay(0);
            }

            foreach (OwnedVehicle veh in Cache.PlayerCache.MyPlayer.User.CurrentChar.Vehicles)
                if (veh.Garage.Garage == Cache.PlayerCache.MyPlayer.Status.Instance.Instance)
                    if (veh.Garage.InGarage)
                    {
                        Vehicle veic = await Funzioni.SpawnLocalVehicle(veh.VehData.Props.Model, new Vector3(Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].X, Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Y, Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Z), Client.Impostazioni.RolePlay.Properties.Garages.LowEnd.PosVehs[veh.Garage.Posto].Heading);
                        await veic.SetVehicleProperties(veh.VehData.Props);
                        VeicoliParcheggio.Add(veic);
                    }

            NetworkFadeInEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true);
            Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
            DoScreenFadeIn(500);
            SetGameplayCamRelativePitch(0.0f, 1.0f);
            Client.Instance.AddTick(Garage);
        }
    }
}