using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.Medics
{
    internal static class MedicsMainClient
    {
        public static Vehicle ActualVehicle;
        public static Vehicle ActualHeli;
        private static List<Blip> Hospitals = new();
        public static Dictionary<Ped, Blip> MedsBlips = new Dictionary<Ped, Blip>();
        public static Dictionary<Ped, Blip> Dead = new Dictionary<Ped, Blip>();

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            Client.Instance.AddEventHandler("lprp:medici:aggiungiPlayerAiMorti", new Action<int>(Aggiungi));
            Client.Instance.AddEventHandler("lprp:medici:rimuoviPlayerAiMorti", new Action<int>(Rimuovi));
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("lprp:medici:aggiungiPlayerAiMorti", new Action<int>(Aggiungi));
            Client.Instance.RemoveEventHandler("lprp:medici:rimuoviPlayerAiMorti", new Action<int>(Rimuovi));
            Hospitals.ForEach(x => x.Delete());
        }

        private static void Spawned(PlayerClient client)
        {
            foreach (Hospital Hosp in Client.Settings.RolePlay.Jobs.Medics.Config.Hospitals)
            {
                Blip blip = World.CreateBlip(Hosp.Blip.Coords.ToVector3);
                blip.Sprite = (BlipSprite)Hosp.Blip.Sprite;
                blip.Scale = Hosp.Blip.Scale;
                blip.Color = (BlipColor)Hosp.Blip.Color;
                blip.IsShortRange = true;
                blip.Name = Hosp.Blip.Name;
                SetBlipDisplay(blip.Handle, Hosp.Blip.Display);
                Hospitals.Add(blip);
            }
        }

        private static void Aggiungi(int player)
        {
            Player pl = new Player(GetPlayerFromServerId(player));

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "medic")
            {
                pl.Character.AttachBlip();
                pl.Character.AttachedBlip.Sprite = BlipSprite.Deathmatch;
                pl.Character.AttachedBlip.Color = BlipColor.Red;
                pl.Character.AttachedBlip.Scale = 1.4f;
                pl.Character.AttachedBlip.Name = "Seriously injured";
                SetBlipDisplay(pl.Character.AttachedBlip.Handle, 4);
                Dead.Add(pl.Character, pl.Character.AttachedBlip);
            }
        }

        private static void Rimuovi(int player)
        {
            Player pl = new Player(GetPlayerFromServerId(player));

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "medic")
            {
                if (Dead.ContainsKey(pl.Character))
                {
                    foreach (Blip bl in pl.Character.AttachedBlips)
                    {
                        if (bl == Dead[pl.Character])
                        {
                            bl.Delete();
                            Dead.Remove(pl.Character);
                        }
                    }
                }
            }
        }

        public static async Task MarkersMedics()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "medic")
            {
                foreach (Hospital osp in Client.Settings.RolePlay.Jobs.Medics.Config.Hospitals)
                {
                    foreach (Position vector in osp.LockerRooms)
                    {
                        if (p.IsInRangeOf(vector.ToVector3, 2f))
                        {
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to go ~y~On~w~ / ~b~off~w~ duty");
                            if (Input.IsControlJustPressed(Control.Context)) MedicsMenu.LockerMenu();
                        }
                    }

                    foreach (Position vector in osp.Pharmacies)
                    {
                        if (p.IsInRangeOf(vector.ToVector3, 1.5f))
                        {
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to open the pharmacy");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                // Menu farmacia
                            }
                        }
                    }

                    foreach (Position vector in osp.VisitorsEntrances)
                    {
                        if (p.IsInRangeOf(vector.ToVector3, 1.375f))
                        {
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to enter");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                Position pos;
                                if (osp.VisitorsEntrances.IndexOf(vector) == 0 || osp.VisitorsEntrances.IndexOf(vector) == 1)
                                    pos = new Position(272.8f, -1358.8f, 23.5f);
                                else
                                    pos = new Position(254.301f, -1372.288f, 23.538f);
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                p.Position = pos.ToVector3;
                                Screen.Fading.FadeIn(800);
                            }
                        }
                    }

                    foreach (Position vettore in osp.VisitorsExits)
                    {
                        if (p.IsInRangeOf(vettore.ToVector3, 1.375f))
                        {
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to exit");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                Position pos;
                                if (osp.VisitorsExits.IndexOf(vettore) == 0)
                                    pos = osp.VisitorsEntrances[0];
                                else
                                    pos = osp.VisitorsEntrances[2];
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                p.Position = pos.ToVector3;
                                Screen.Fading.FadeIn(800);
                            }
                        }
                    }

                    foreach (SpawnerSpawn vehicle in osp.Vehicles)
                    {
                        if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                        {
                            World.DrawMarker(MarkerType.CarSymbol, vehicle.SpawnerMenu.ToVector3, Position.Zero.ToVector3, Position.Zero.ToVector3, new Position(2f, 2f, 1.5f).ToVector3, Colors.Cyan, false, false, true);

                            if (p.IsInRangeOf(vehicle.SpawnerMenu.ToVector3, 1.5f))
                            {
                                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to choose the vehicle");

                                if (Input.IsControlJustPressed(Control.Context))
                                {
                                    Screen.Fading.FadeOut(800);
                                    await BaseScript.Delay(1000);
                                    MedicsMenu.VehicleMenuNew(osp, vehicle);
                                }
                            }
                        }

                        for (int i = 0; i < vehicle.Deleters.Count; i++)
                        {
                            if (!Functions.IsSpawnPointClear(vehicle.Deleters[i].ToVector3, 2f))
                                foreach (Vehicle veh in Functions.GetVehiclesInArea(vehicle.Deleters[i].ToVector3, 2f))
                                    if (!veh.HasDecor("MedicsVehicle") && !veh.HasDecor("MedicsVehicle"))
                                        veh.Delete();

                            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                            {
                                World.DrawMarker(MarkerType.CarSymbol, vehicle.Deleters[i].ToVector3, Position.Zero.ToVector3, Position.Zero.ToVector3, new Position(2f, 2f, 1.5f).ToVector3, Colors.Red, false, false, true);

                                if (p.IsInRangeOf(vehicle.Deleters[i].ToVector3, 2f))
                                {
                                    HUD.ShowHelp("Press ~INPUT_CONTEXT~ to park the vehicle in the garage");

                                    if (Input.IsControlJustPressed(Control.Context))
                                    {
                                        if (p.CurrentVehicle.HasDecor("MedicsVehicle"))
                                        {
                                            VehiclePolice vehicl = new VehiclePolice(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
                                            BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehMedici", vehicl.ToJson());
                                            p.CurrentVehicle.Delete();
                                            ActualVehicle = new Vehicle(0);
                                        }
                                        else
                                        {
                                            HUD.ShowNotification("The vehicle you are attempting to park is not registered to a medic!", ColoreNotifica.Red, true);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (SpawnerSpawn heli in osp.Helicopters)
                    {
                        if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle) World.DrawMarker(MarkerType.HelicopterSymbol, heli.SpawnerMenu.ToVector3, Position.Zero.ToVector3, Position.Zero.ToVector3, new Position(2f, 2f, 1.5f).ToVector3, Colors.Cyan, false, false, true);

                        if (p.IsInRangeOf(heli.SpawnerMenu.ToVector3, 1.5f))
                        {
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to choose the helicopter");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                MedicsMenu.HeliMenu(osp, heli);
                            }
                        }

                        for (int i = 0; i < heli.Deleters.Count; i++)
                        {
                            if (!Functions.IsSpawnPointClear(heli.Deleters[i].ToVector3, 2f))
                                foreach (Vehicle veh in Functions.GetVehiclesInArea(heli.Deleters[i].ToVector3, 2f))
                                    if (!veh.HasDecor("MedicsVehicle") && !veh.HasDecor("MedicsVehicle"))
                                        veh.Delete();

                            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                            {
                                World.DrawMarker(MarkerType.HelicopterSymbol, heli.Deleters[i].ToVector3, Position.Zero.ToVector3, Position.Zero.ToVector3, new Position(3f, 3f, 1.5f).ToVector3, Colors.Red, false, false, true);

                                if (p.IsInRangeOf(heli.Deleters[i].ToVector3, 2f))
                                {
                                    HUD.ShowHelp("Press ~INPUT_CONTEXT~ to park the helicopter");

                                    if (Input.IsControlJustPressed(Control.Context))
                                    {
                                        if (p.CurrentVehicle.HasDecor("MedicsVehicle"))
                                        {
                                            VehiclePolice veh = new VehiclePolice(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
                                            BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehMedici", veh.ToJson());
                                            p.CurrentVehicle.Delete();
                                            ActualHeli = new Vehicle(0);
                                        }
                                        else
                                        {
                                            HUD.ShowNotification("The helicopter you are attempting to park is not registered to a medic!", ColoreNotifica.Red, true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static async Task MarkersNotMedics()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() != "medic" || Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() != "medici")
            {
                foreach (Hospital osp in Client.Settings.RolePlay.Jobs.Medics.Config.Hospitals)
                {
                    foreach (Position vettore in osp.VisitorsEntrances.Where(vettore => p.IsInRangeOf(vettore.ToVector3, 1.375f)))
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to enter");

                        if (!Input.IsControlJustPressed(Control.Context)) continue;
                        Position pos;
                        if (osp.VisitorsEntrances.IndexOf(vettore) == 0 || osp.VisitorsEntrances.IndexOf(vettore) == 1)
                            pos = new Position(272.8f, -1358.8f, 23.5f);
                        else
                            pos = new Position(254.301f, -1372.288f, 24.538f);
                        Screen.Fading.FadeOut(800);
                        await BaseScript.Delay(1000);
                        p.Position = pos.ToVector3;
                        Screen.Fading.FadeIn(800);
                    }

                    foreach (Position vettore in osp.VisitorsExits.Where(vettore => p.IsInRangeOf(vettore.ToVector3, 1.375f)))
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to exit");

                        if (!Input.IsControlJustPressed(Control.Context)) continue;
                        Position pos = osp.VisitorsExits.IndexOf(vettore) == 0 ? osp.VisitorsEntrances[0] : osp.VisitorsEntrances[2];
                        Screen.Fading.FadeOut(800);
                        await BaseScript.Delay(1000);
                        p.Position = pos.ToVector3;
                        Screen.Fading.FadeIn(800);
                    }
                }
            }
        }

        public static async Task EnableBlipsMedics()
        {
            await BaseScript.Delay(1000);

            if (Client.Settings.RolePlay.Jobs.Medics.Config.EnableBlipCars)
            {
                foreach (PlayerClient p in Client.Instance.Clients)
                {
                    if (p.User.CurrentChar.Job.Name == "Medic")
                    {
                        int id = GetPlayerFromServerId(p.Player.ServerId);

                        if (!NetworkIsPlayerActive(id) || GetPlayerPed(id) == PlayerPedId()) continue;
                        Ped playerPed = new(GetPlayerPed(id));

                        if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                        {
                            if (!playerPed.CurrentVehicle.HasDecor("MedicsVehicle")) continue;

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
                                {
                                    if (playerPed.CurrentVehicle.IsEngineRunning)
                                    {
                                        playerPed.AttachedBlip.Sprite = BlipSprite.PoliceHelicopterAnimated;
                                    }
                                }

                                if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceHelicopterAnimated)
                                {
                                    SetBlipShowCone(playerPed.AttachedBlip.Handle, playerPed.CurrentVehicle.HeightAboveGround > 5f);
                                    if (!playerPed.CurrentVehicle.IsEngineRunning) playerPed.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
                                }

                                if (playerPed.AttachedBlip.Sprite == BlipSprite.PoliceCar || playerPed.AttachedBlip.Sprite == BlipSprite.Boat || playerPed.AttachedBlip.Sprite == BlipSprite.PersonalVehicleBike)
                                {
                                    if (playerPed.CurrentVehicle.HasSiren && playerPed.CurrentVehicle.IsSirenActive)
                                    {
                                        playerPed.AttachedBlip.Sprite = BlipSprite.PoliceCarDot;
                                        SetBlipShowCone(playerPed.AttachedBlip.Handle, true);
                                    }
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
                            {
                                if (b.Sprite == BlipSprite.PoliceCar || b.Sprite == BlipSprite.PoliceCarDot || b.Sprite == BlipSprite.PoliceHelicopter || b.Sprite == BlipSprite.PoliceHelicopterAnimated || b.Sprite == BlipSprite.PersonalVehicleBike || b.Sprite == BlipSprite.Boat)
                                {
                                    b.Delete();
                                }
                            }

                            MedsBlips.Remove(playerPed);
                        }
                    }
                }
            }
            else
            {
                Client.Instance.RemoveTick(EnableBlipsMedics);
            }
        }

        public static async Task DeadBlips()
        {
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "medic")
            {
                if (Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty)
                    foreach (KeyValuePair<Ped, Blip> morto in Dead)
                        morto.Value.Alpha = 255;
                else
                    foreach (KeyValuePair<Ped, Blip> morto in Dead)
                        morto.Value.Alpha = 0;
            }
        }
    }
}