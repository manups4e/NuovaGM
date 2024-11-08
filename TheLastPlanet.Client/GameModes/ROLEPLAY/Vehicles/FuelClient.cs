﻿using Settings.Shared.Config.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Vehicles
{
    public static class FuelClient
    {
        public static bool curVehInit = false;
        public static Vehicle LastVehicle = new Vehicle(0);
        public static bool justPumped = false;
        public static bool fuelChecked = false;
        public static float FuelCapacity;
        public static float FuelRpmImpact;
        public static float FuelAccelImpact;
        public static float FuelTractionImpact;
        public static float addedFuel = 0.0f;
        public static int lastStation = 0;
        private static readonly string DecorName = "lprp_fuel";
        public static Vehicle jobTruck = new Vehicle(0);
        public static Vehicle jobTrailer = new Vehicle(0);
        public static Vehicle lastVehicle = new Vehicle(0);
        public static List<Blip> gsblips = new List<Blip>();
        public static int animState = 3;
        public static List<Blip> registryBlips = new List<Blip>();
        public static bool canRegisterForTanker = true;
        public static bool canPickupTanker = true;
        public static bool canUnloadFuel = true;
        public static int curRegPickup = 0;
        public static List<Blip> pickupBlip = new List<Blip>();
        public static bool hasTanker = false;
        public static int tankerfuel = 0;
        public static List<Blip> refuelBlips = new List<Blip>();
        public static List<string> trucks;
        public static string tanker;
        public static int maxtankerfuel;
        public static float refuelCost;
        public static bool canBuyFuel = true;
        public static bool distwarn = false;
        public static bool avviso1 = false;

        public static List<Vector3> registrySpots = new List<Vector3>() { new Vector3(42.683f, -2634.747f, 6.037f) };
        public static List<Vector3> refuelspots = new List<Vector3>() { new Vector3(75.688f, -2645.164f, 6.012f), new Vector3(1492.691f, -1939.486f, 70.837f) };

        public static List<Tanker> tankerSpots = new List<Tanker>() { new Tanker(new Vector3(-284.762f, 6054.931f, 31.515f), 43.926f, new Vector3(-277.335f, 6047.611f, 31.552f), 40.544f, 0.75f, 2100), new Tanker(new Vector3(113.258f, -2644.929f, 6.011f), 171.418f, new Vector3(114.388f, -2637.694f, 6.025f), 170.051f, 0.6f, 1100), new Tanker(new Vector3(342.647f, 3412.522f, 36.597f), 109.447f, new Vector3(350.287f, 3415.289f, 36.479f), 109.447f, 0.75f, 2600) };

        public static SingleStation lastStationFuel = new SingleStation();

        public static void Init()
        {
            Client.Instance.AddEventHandler("lprp:fuel:checkfuelforstation", new Action<int, int>(checkfuel));
            Client.Instance.AddEventHandler("lprp:fuel:addfueltovehicle", new Action<bool, string, int>(AddFuelToVeh));
            Client.Instance.AddEventHandler("lprp:fuel:fillFuel", new Action(FillFuel));
            Client.Instance.AddEventHandler("lprp:fuel:fuelLevel", new Action<float>(FuelLevel));
            Client.Instance.AddEventHandler("frfuel:filltankForVeh", new Action<int>(FillTankForVeh));
            Client.Instance.AddEventHandler("lprp:fuel:depositfuel", new Action<bool, string, string, string>(DepositFuel));
            Client.Instance.AddEventHandler("lprp:fuel:depositfuelnotowned", new Action<bool, string, string, string, string>(DepositFuelNotOwned));
            Client.Instance.AddEventHandler("lprp:fuel:buytanker", new Action<bool, string>(BuyTanker));
            Client.Instance.AddEventHandler("lprp:fuel:stationnotowned", new Action(StationNotOwned));
            Client.Instance.AddEventHandler("lprp:fuel:buyfuelfortanker", new Action<bool, string>(BuyFuelForTanker));
            Client.Instance.AddEventHandler("lprp:fuel:settankerfuel", new Action<int>(SetTankerFuel));
            Client.Instance.AddEventHandler("lprp:fuel:saddfuel", new Action<int>(SAddFuel));
            Client.Instance.AddEventHandler("lprp:fuel:saddmoney", new Action<int>(SAddMoney));
            Client.Instance.AddEventHandler("lprp:fuel:sresetmanage", new Action<int>(SResetManage));
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("lprp:fuel:checkfuelforstation", new Action<int, int>(checkfuel));
            Client.Instance.RemoveEventHandler("lprp:fuel:addfueltovehicle", new Action<bool, string, int>(AddFuelToVeh));
            Client.Instance.RemoveEventHandler("lprp:fuel:fillFuel", new Action(FillFuel));
            Client.Instance.RemoveEventHandler("lprp:fuel:fuelLevel", new Action<float>(FuelLevel));
            Client.Instance.RemoveEventHandler("frfuel:filltankForVeh", new Action<int>(FillTankForVeh));
            Client.Instance.RemoveEventHandler("lprp:fuel:depositfuel", new Action<bool, string, string, string>(DepositFuel));
            Client.Instance.RemoveEventHandler("lprp:fuel:depositfuelnotowned", new Action<bool, string, string, string, string>(DepositFuelNotOwned));
            Client.Instance.RemoveEventHandler("lprp:fuel:buytanker", new Action<bool, string>(BuyTanker));
            Client.Instance.RemoveEventHandler("lprp:fuel:stationnotowned", new Action(StationNotOwned));
            Client.Instance.RemoveEventHandler("lprp:fuel:buyfuelfortanker", new Action<bool, string>(BuyFuelForTanker));
            Client.Instance.RemoveEventHandler("lprp:fuel:settankerfuel", new Action<int>(SetTankerFuel));
            Client.Instance.RemoveEventHandler("lprp:fuel:saddfuel", new Action<int>(SAddFuel));
            Client.Instance.RemoveEventHandler("lprp:fuel:saddmoney", new Action<int>(SAddMoney));
            Client.Instance.RemoveEventHandler("lprp:fuel:sresetmanage", new Action<int>(SResetManage));
            registryBlips.ForEach(x => x.Delete());
            gsblips.ForEach(x => x.Delete());
            gsblips.Clear();
            registryBlips.Clear();
        }

        public static void Spawned(PlayerClient client)
        {
            FuelCapacity = Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity;
            FuelRpmImpact = Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelRpmImpact;
            FuelAccelImpact = Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelAccelImpact;
            FuelTractionImpact = Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelTractionImpact;
            trucks = Client.Settings.RolePlay.Vehicles.VehiclesDamages.Trucks;
            tanker = Client.Settings.RolePlay.Vehicles.VehiclesDamages.Tanker;
            maxtankerfuel = Client.Settings.RolePlay.Vehicles.VehiclesDamages.Maxtankerfuel;
            refuelCost = Client.Settings.RolePlay.Vehicles.VehiclesDamages.RefuelCost;
            LoadStationsBlips();
        }

        public static string getRandomPlate()
        {
            List<char> charset = new List<char>();
            Random random = new Random(GetGameTimer());
            for (int i = 65; i < 90; i++) charset.Add((char)i);
            for (int i = 97; i < 122; i++) charset.Add((char)i);
            string rndstr = "";
            for (int i = 1; i < 6; i++) rndstr += charset[random.Next(1, charset.Count)];

            return rndstr;
        }

        public static void checkfuel(int fuel, int price) { lastStationFuel = new SingleStation(fuel, price); }

        public static async void LoadStationsBlips()
        {
            List<GasStation> stations = await EventDispatcher.Get<List<GasStation>>("tlg:roleplay:getStations");
            foreach (GasStation p in stations)
            {
                Blip blip = new Blip(AddBlipForCoord(p.pos[0], p.pos[1], p.pos[2])) { Sprite = BlipSprite.JerryCan, Scale = 0.85f, IsShortRange = true, Name = "Stazione di Rifornimento" };
                SetBlipDisplay(blip.Handle, 4);
                gsblips.Add(blip);
            }

            foreach (Vector3 v in registrySpots)
            {
                Blip blip = new Blip(AddBlipForCoord(v.X, v.Y, v.Z))
                {
                    Sprite = (BlipSprite)67, // sennò (BlipSprite)(67)
                    Scale = 0.88f,
                    IsShortRange = true,
                    Name = "Tank registration"
                };
                SetBlipColour(blip.Handle, 17);
                SetBlipDisplay(blip.Handle, 4);
                registryBlips.Add(blip);
            }
        }

        public static void ShowRefuelBlips()
        {
            foreach (Vector3 v in refuelspots)
            {
                Blip blip = new Blip(AddBlipForCoord(v.X, v.Y, v.Z))
                {
                    Sprite = BlipSprite.JerryCan,
                    Scale = 0.85f,
                    Color = (BlipColor)35,
                    IsShortRange = true,
                    Name = "Tank refueling station"
                };
                SetBlipDisplay(blip.Handle, 4);
                refuelBlips.Add(blip);
            }
        }

        public static void HideRefuelBlips()
        {
            for (int i = 0; i < refuelBlips.Count; i++) refuelBlips[i].Delete();
            refuelBlips.Clear();
        }

        public static float RandomFuelLevel(float cap)
        {
            float min = cap / 3.0f;
            float max = cap - cap / 4;

            return new Random(GetGameTimer()).NextFloat() * (max - min) + min;
        }

        public static void SetVehicleFuelLevel(this Vehicle veh, float fuel)
        {
            float maxfuel = Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity;
            if (fuel > maxfuel) fuel = maxfuel;
            veh.SetDecor(DecorName, fuel);
            veh.FuelLevel = veh.GetDecor<float>(DecorName);
        }

        public static void AddFuelToVeh(bool success, string msg, int fuelval)
        {
            if (success)
            {
                SetVehicleFuelLevel(LastVehicle, fuelval);
                if (LastVehicle.PassengerCount > 0) LastVehicle.Passengers.ToList().ForEach(x => SetVehicleFuelLevel(x.CurrentVehicle, fuelval));
            }

            if (msg != null) HUD.ShowNotification(msg, true);
        }

        public static void initFuel(Vehicle veh)
        {
            curVehInit = true;
            float fuelCapacity = Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity;
            if (!veh.HasDecor(DecorName)) veh.SetDecor(DecorName, RandomFuelLevel(fuelCapacity));
            veh.FuelLevel = veh.GetDecor<float>(DecorName);
        }

        public static bool modelValid(Vehicle veh) { return veh.Model.IsBike | veh.Model.IsCar | veh.Model.IsQuadbike; }

        public static float vehicleFuelLevel(this Vehicle veh) { return veh.HasDecor(DecorName) ? veh.GetDecor<float>(DecorName) : Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity; }

        public static void ConsumeFuel(Vehicle veh)
        {
            float fuel = vehicleFuelLevel(veh);

            if (fuel > 0 && veh.IsEngineRunning)
            {
                float rpm = (float)Math.Pow(veh.CurrentRPM, 2.5f);
                fuel -= rpm * FuelRpmImpact;
                fuel -= veh.Acceleration * FuelAccelImpact;
                fuel -= veh.MaxTraction * FuelTractionImpact;
                if (fuel < 0.0f) fuel = 0f;
                SetVehicleFuelLevel(veh, fuel);
            }
        }

        public static bool withinDist(Vector3 pos, Entity ent)
        {
            Vector3 epos = ent.Position;
            float dist = Vector3.Distance(pos, epos);

            return dist <= 2.05f ? true : false;
        }

        public static async void spawnTanker(Tanker spot, string plate)
        {
            Random random = new Random();
            string rnd = trucks[random.Next(trucks.Count)];
            Model truck = new Model(rnd);
            if (!truck.IsLoaded) await truck.Request(3000); // for when you stream resources.
            Model trailer = new Model(tanker);
            if (!trailer.IsLoaded) await trailer.Request(3000); // for when you stream resources.
            jobTruck = await World.CreateVehicle(truck, spot.pos, spot.heading);
            jobTruck.PlaceOnGround();
            Cache.PlayerCache.MyPlayer.Ped.SetIntoVehicle(jobTruck, VehicleSeat.Driver);
            SetVehicleNumberPlateText(jobTruck.Handle, plate);
            BaseScript.TriggerEvent("frfuel:filltankForVeh", jobTruck.Handle);
            string plat = GetVehicleNumberPlateText(jobTruck.Handle).ToLower();
            BaseScript.TriggerServerEvent("lprp:vehicles:registerJobVehicle", plat);
            jobTrailer = await World.CreateVehicle(trailer, spot.t, spot.theading);
            jobTrailer.PlaceOnGround();
            SetVehicleNumberPlateText(jobTrailer.Handle, plate);
            AttachVehicleToTrailer(jobTruck.Handle, jobTrailer.Handle, 10.0f);
            hasTanker = true;
        }

        public static void FillFuel()
        {
            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
            {
                SetVehicleFuelLevel(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle, Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity);
                HUD.ShowNotification("Your fuel has been filled. Use it ONLY in case of ~r~EMERGENCIES~w~!");
            }
        }

        public static void FuelLevel(float level)
        {
            if (level > Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity)
                level = Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity;
            else if (level < 0f) level = 0f;
            SetVehicleFuelLevel(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle, level);
            HUD.ShowNotification("Fuel set, Use it ONLY in case of ~r~EMERGENCY~w~!");
        }

        public static void FillTankForVeh(int veh) { SetVehicleFuelLevel(new Vehicle(veh), Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity); }

        public static void DepositFuel(bool success, string tankerful, string stationfuel, string overflow)
        {
            if (success)
            {
                int over = Convert.ToInt32(overflow);
                int stationf = Convert.ToInt32(stationfuel);
                if (over > 0)
                    tankerfuel = over;
                else
                    tankerfuel = 0;
                HUD.ShowNotification($"You delivered ~b~{tankerful}~w~ gallons of fuel. The station now has ~b~{stationf}~b~ liters.\nYour tanker has ~b~{tankerfuel}~w~ liters of fuel remaining.");
            }
            else
            {
                HUD.ShowNotification(tankerful);
            }

            canUnloadFuel = true;
            ShowRefuelBlips();
        }

        public static void DepositFuelNotOwned(bool success, string tankerful, string stationfuel, string pay, string overflow)
        {
            if (success)
            {
                int over = Convert.ToInt32(overflow);
                int pai = Convert.ToInt32(pay);
                if (over > 0)
                    tankerfuel = over;
                else
                    tankerfuel = 0;
                if (pai == 0)
                    HUD.ShowNotification($"You delivered ~b~{tankerful}~w~ gallons of fuel. The station now has ~b~{stationfuel}~w~ liters of fuel.\nYour tanker has ~b~{tankerfuel}~w~ liters of fuel remaining.");
                else
                    HUD.ShowNotification($"You delivered ~b~{tankerful}~w~ gallons of fuel. The station now has ~b~{stationfuel}~w~ gallons of fuel.\nYou have been paid ~g~{pay}$~w~ for delivery.<br><br>Your tanker has ~b~{tankerfuel}~w~ liters of fuel remaining.");
            }
            else
            {
                HUD.ShowNotification(tankerful);
            }

            canUnloadFuel = true;
            ShowRefuelBlips();
        }

        public static void BuyTanker(bool success, string JSON)
        {
            if (success)
            {
                Tanker t = JSON.FromJson<Tanker>();
                tankerfuel = t.fuelForTanker;
                spawnTanker(t, getRandomPlate());
                HUD.ShowNotification("You bought a tank full of fuel.");
            }
            else
            {
                HUD.ShowNotification(JSON);
                canPickupTanker = true;
            }

            if (pickupBlip.Count > 0)
            {
                foreach (Blip b in pickupBlip) b.Delete();
                pickupBlip.Clear();
            }
        }

        public static void StationNotOwned()
        {
            canUnloadFuel = true;
            HUD.ShowNotification("This station doesn't belong to anyone. Maybe you should consider buying it...");
        }

        public static void BuyFuelForTanker(bool success, string msg)
        {
            if (success)
            {
                tankerfuel += Convert.ToInt32(msg);
                HUD.ShowNotification("The tanker was filled with fuel.");
            }
            else
            {
                HUD.ShowNotification(msg);
            }

            canBuyFuel = true;
        }

        public static void SetTankerFuel(int level)
        {
            tankerfuel = level;
            HUD.ShowNotification("Upgraded fuel tank.");
        }

        public static void SAddFuel(int amount)
        {
            int cl = 0;

            for (int i = 0; i < ConfigShared.SharedConfig.Main.Vehicles.gasstations.Count; i++)
            {
                float dist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pos);
                if (dist < 100) cl = i;
                if (cl > 0)
                    BaseScript.TriggerServerEvent("lprp:businesses:saddfuel", cl, amount);
                else
                    HUD.ShowNotification("No gas station nearby.");
            }
        }

        public static void SAddMoney(int amount)
        {
            int cl = 0;

            for (int i = 0; i < ConfigShared.SharedConfig.Main.Vehicles.gasstations.Count; i++)
            {
                float dist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pos);
                if (dist < 100) cl = i;
                if (cl > 0)
                    BaseScript.TriggerServerEvent("lprp:businesses:saddmoney", cl, amount);
                else
                    HUD.ShowNotification("No gas station nearby.");
            }
        }

        public static void SResetManage(int amount)
        {
            int cl = 0;

            for (int i = 0; i < ConfigShared.SharedConfig.Main.Vehicles.gasstations.Count; i++)
            {
                float dist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pos);
                if (dist < 100) cl = i;
                if (cl > 0)
                    BaseScript.TriggerServerEvent("lprp:businesses:sresetmanage", cl);
                else
                    HUD.ShowNotification("No gas station nearby.");
            }
        }

        private static Vehicle veh = new Vehicle(0);
        private static Vehicle lastveh = new Vehicle(0);

        public static async Task FuelCount()
        {
            try
            {
                Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

                if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                {
                    if (playerPed.SeatIndex == VehicleSeat.Driver) veh = playerPed.CurrentVehicle;
                    if (playerPed.LastVehicle != null) lastveh = playerPed.LastVehicle;
                }

                if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle && veh.Driver == playerPed && modelValid(veh) && !veh.IsDead)
                {
                    if (LastVehicle != veh)
                    {
                        LastVehicle = veh;
                        curVehInit = false;
                    }

                    if (!curVehInit) initFuel(veh);
                    ConsumeFuel(veh);
                    if (!Main.ClientConfig.CinemaMode) HUD.DrawText(0.195f, 0.96f, $"Fuel: {(int)Math.Floor(veh.FuelLevel / FuelCapacity * 100)}%", Color.FromArgb(255, 135, 206, 250));

                    if (vehicleFuelLevel(veh) < 0.99f)
                    {
                        veh.IsEngineRunning = false;
                        veh.IsDriveable = false;
                    }
                }
                else if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle && veh.Driver != playerPed && modelValid(veh) && !veh.IsDead)
                {
                    veh.FuelLevel = veh.HasDecor(DecorName) ? veh.GetDecor<float>(DecorName) : Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity;
                    curVehInit = false;
                }

                if (veh.Exists() || lastveh.Exists())
                {
                    for (int i = 0; i < ConfigShared.SharedConfig.Main.Vehicles.gasstations.Count; i++)
                    {
                        float dist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pos);

                        if (dist <= 80f)
                        {
                            lastStation = i + 1;

                            if (fuelChecked == false && Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                            {
                                fuelChecked = true;
                                BaseScript.TriggerServerEvent("lprp:businesses:checkfuelforstation", lastStation);
                            }

                            for (int j = 0; j < ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps.Count; j++)
                            {
                                if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                                {
                                    if (veh.ClassType == VehicleClass.Industrial || lastveh.ClassType == VehicleClass.Industrial || veh.ClassType == VehicleClass.Commercial || lastveh.ClassType == VehicleClass.Commercial)
                                        World.DrawMarker(MarkerType.TruckSymbol, new Vector3(ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j].X, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j].Y, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j].Z + 1), new Vector3(0), new Vector3(0), new Vector3(2.0f, 2.0f, 1.8f), Color.FromArgb(180, 255, 255, 0), false, false, true);
                                    else if (veh.Model.IsCar || lastveh.Model.IsCar)
                                        World.DrawMarker(MarkerType.CarSymbol, new Vector3(ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j].X, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j].Y, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j].Z + 1), new Vector3(0), new Vector3(0), new Vector3(2.0f, 2.0f, 1.8f), Color.FromArgb(180, 255, 255, 0), false, false, true);
                                    else if (veh.Model.IsBike || lastveh.Model.IsBike) World.DrawMarker(MarkerType.BikeSymbol, new Vector3(ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j].X, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j].Y, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j].Z + 1), new Vector3(0), new Vector3(0), new Vector3(2.0f, 2.0f, 1.8f), Color.FromArgb(180, 255, 255, 0), false, false, true);
                                }

                                float pdist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pumps[j]);

                                if (pdist < 3.05 && LastVehicle.Exists() && withinDist(Cache.PlayerCache.MyPlayer.Position.ToVector3, LastVehicle) && !Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                                {
                                    DisableControlAction(2, 22, true);

                                    if (lastStationFuel.stationfuel > 0)
                                    {
                                        int benz = lastStationFuel.stationfuel;
                                        HUD.ShowHelp("~w~Keep pressed ~INPUT_CONTEXT~ to fill the ~b~tank~w~.~n~Fuel: " + benz + " liters, Price: $ " + lastStationFuel.stationprice + "/Liter");

                                        if (Input.IsControlPressed(Control.Context) || Input.IsDisabledControlPressed(Control.Context))
                                        {
                                            if (!LastVehicle.IsEngineRunning)
                                            {
                                                if (!IsEntityPlayingAnim(PlayerPedId(), "timetable@gardener@filling_can", "gar_ig_5_filling_can", 3))
                                                {
                                                    TaskTurnPedToFaceEntity(PlayerPedId(), LastVehicle.Handle, 1000);
                                                    await BaseScript.Delay(1000);
                                                    await playerPed.Task.PlayAnimation("timetable@gardener@filling_can", "gar_ig_5_filling_can", 2f, 8f, -1, (AnimationFlags)50, 0);
                                                }

                                                if (LastVehicle.FuelLevel < 100)
                                                {
                                                    justPumped = true;
                                                    float fuel = LastVehicle.FuelLevel;
                                                    float maxfuel = Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity;
                                                    float afuel = fuel + addedFuel;

                                                    if (afuel <= maxfuel)
                                                    {
                                                        addedFuel += 0.1f;
                                                        HUD.DrawText(0.195f, 0.96f, $"Fuel: {(int)Math.Floor(fuel + addedFuel / maxfuel * 100)}%", Color.FromArgb(255, 135, 206, 250));
                                                    }
                                                }
                                                else
                                                {
                                                    HUD.ShowNotification("Your vehicle is full!", ColoreNotifica.Red, true);
                                                }
                                            }
                                            else
                                            {
                                                HUD.ShowNotification("You vehicle must be turned OFF", ColoreNotifica.Red, true);
                                            }
                                        }

                                        if (Input.IsControlJustReleased(Control.Context) || Input.IsDisabledControlJustReleased(Control.Context))
                                        {
                                            if (IsEntityPlayingAnim(PlayerPedId(), "timetable@gardener@filling_can", "gar_ig_5_filling_can", 3)) playerPed.Task.ClearAll();

                                            if (justPumped)
                                            {
                                                justPumped = false;
                                                float fuel = vehicleFuelLevel(LastVehicle);
                                                BaseScript.TriggerServerEvent("lprp:fuel:payForFuel", i + 1, addedFuel, fuel + addedFuel);
                                                addedFuel = 0.0f;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        HUD.ShowNotification("This gas station is out of fuel. Try another one.", true);
                                    }
                                }
                            }
                        }

                        if (lastStation > 0)
                        {
                            float dista = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, ConfigShared.SharedConfig.Main.Vehicles.gasstations[lastStation - 1].pos);

                            if (dista > 80f)
                            {
                                lastStation = 0;
                                fuelChecked = false;
                            }
                        }
                    }
                }

                CitizenFX.Core.Weapon wep = playerPed.Weapons.Current;

                if (wep.Hash == WeaponHash.PetrolCan && LastVehicle.Exists())
                {
                    float dist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, LastVehicle.Position);

                    if (dist < 2 && LastVehicle.HasDecor(DecorName))
                    {
                        float max = Client.Settings.RolePlay.Vehicles.VehiclesDamages.FuelCapacity;
                        float fuel = vehicleFuelLevel(lastVehicle);
                        if (max - fuel < 0.5)
                            HUD.ShowNotification("Tanker already full.");
                        else
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to fill the tank with the jerry can");

                        if (Input.IsControlPressed(Control.Context))
                        {
                            if (animState == 3)
                            {
                                playerPed.Task.PlayAnimation("weapon@w_sp_jerrycan", "fire_intro");
                                animState = 1;
                            }
                            else if (animState == 1)
                            {
                                if (!IsEntityPlayingAnim(playerPed.Handle, "weapon@w_sp_jerrycan", "fire_intro", 3))
                                {
                                    playerPed.Task.PlayAnimation("weapon@w_sp_jerrycan", "fire");
                                    animState = 2;
                                }
                            }

                            if (fuel < max)
                            {
                                HUD.DrawText(0.195f, 0.96f, $"Fuel: {(int)Math.Floor(fuel / max * 100)}%", Color.FromArgb(255, 135, 206, 250));
                                if (fuel + 0.1 >= max)
                                    SetVehicleFuelLevel(lastVehicle, max);
                                else
                                    SetVehicleFuelLevel(lastVehicle, fuel + 0.2f);
                            }
                        }

                        if (Input.IsControlJustReleased(Control.Context))
                        {
                            StopEntityAnim(playerPed.Handle, "fire", "weapon@w_sp_jerrycan", 3);
                            playerPed.Task.PlayAnimation("weapon@w_sp_jerrycan", "fire_outro");
                            animState = 3;
                        }
                    }
                }

                await Task.FromResult(0);
            }
            catch (Exception e)
            {
                Client.Logger.Error("Error in fuelClient = " + e);
            }
        }

        public static async Task FuelTruck()
        {
            if (jobTruck.Handle == 0)
                for (int i = 0; i < registrySpots.Count; i++)
                {
                    float dist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, registrySpots[i]);

                    if (dist < 80)
                    {
                        World.DrawMarker(MarkerType.TruckSymbol, registrySpots[i], new Vector3(0), new Vector3(0), new Vector3(1.1f, 1.1f, 1.3f), Color.FromArgb(170, 0, 0, 255), false, false, true);

                        if (dist < 1.15)
                            if (canRegisterForTanker)
                            {
                                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to record the withdrawal of a tank.");

                                if (Input.IsControlJustPressed(Control.Context))
                                {
                                    canRegisterForTanker = false;
                                    curRegPickup = SharedMath.GetRandomInt(1, tankerSpots.Count);
                                    SetNewWaypoint(tankerSpots[curRegPickup].pos.X, tankerSpots[curRegPickup].pos.Y);

                                    if (pickupBlip.Count > 0)
                                    {
                                        foreach (Blip a in pickupBlip) a.Delete();
                                        pickupBlip.Clear();
                                    }

                                    Blip b = new Blip(AddBlipForCoord(tankerSpots[curRegPickup].pos.X, tankerSpots[curRegPickup].pos.Y, tankerSpots[curRegPickup].pos.Z))
                                    {
                                        Sprite = (BlipSprite)67,
                                        Scale = 0.85f,
                                        Color = (BlipColor)14,
                                        IsShortRange = true,
                                        Name = "Tanker collection"
                                    };
                                    SetBlipDisplay(b.Handle, 4);
                                    pickupBlip.Add(b);
                                    HUD.ShowNotification("The tanker collection point has been set on your ~b~GPS~w~.");
                                }
                            }
                    }
                }

            if (curRegPickup > 0)
            {
                Vector3 spot = tankerSpots[curRegPickup].pos;
                World.DrawMarker(MarkerType.TruckSymbol, new Vector3(spot.X, spot.Y, spot.Z), new Vector3(0), new Vector3(0), new Vector3(2.1f, 2.1f, 1.3f), Color.FromArgb(170, 0, 255, 0), false, false, true);
                float dist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, spot);

                if (dist < 2.1)
                {
                    Tanker info = tankerSpots[curRegPickup];

                    if (canPickupTanker)
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to take the tank. It will cost you " + info.ppu * info.fuelForTanker + " for fuel.");

                        if (Input.IsControlJustPressed(Control.Context))
                        {
                            canPickupTanker = false;
                            BaseScript.TriggerServerEvent("lprp:fuel:buytanker", info.ToJson());
                        }
                    }
                }
            }

            if (jobTruck.Handle != 0 && jobTrailer.Handle != 0)
            {
                Vehicle vehicle = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle;

                if (vehicle == jobTruck && IsVehicleAttachedToTrailer(jobTruck.Handle))
                {
                    HUD.DrawText(0.9f, 0.935f, $"Tanker fuel: {tankerfuel}", Color.FromArgb(255, 135, 206, 235));

                    for (int i = 0; i < ConfigShared.SharedConfig.Main.Vehicles.gasstations.Count; i++)
                    {
                        float dis = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pos);

                        if (dis < 80)
                        {
                            World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pos.X, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pos.Y, ConfigShared.SharedConfig.Main.Vehicles.gasstations[i].pos.Z - 1.00001f), new Vector3(0), new Vector3(0), new Vector3(10.1f, 10.1f, 1.3f), Color.FromArgb(170, 0, 255, 0));

                            if (dis < 10.1)
                            {
                                if (tankerfuel > 0)
                                {
                                    HUD.ShowHelp("Press ~INPUT_CONTEXT~ to unload fuel at the station.");

                                    if (canUnloadFuel)
                                        if (Input.IsControlJustPressed(Control.Context))
                                        {
                                            canUnloadFuel = false;
                                            BaseScript.TriggerServerEvent("lprp:businesses:depositfuel", i, tankerfuel);
                                        }
                                }
                                else
                                {
                                    HUD.ShowNotification("Your tank is empty. You can purchase fuel at the Tank Refueling Station.", ColoreNotifica.Red, true);
                                    HUD.ShowHelp("To stop working, walk away from your vehicle.");
                                }
                            }
                        }
                    }

                    for (int i = 0; i < refuelspots.Count; i++)
                    {
                        float di = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, refuelspots[i]);

                        if (di < 80)
                        {
                            World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(refuelspots[i].X, refuelspots[i].Y, refuelspots[i].Z - 1.00001f), new Vector3(0), new Vector3(0), new Vector3(3.1f, 3.1f, 1.3f), Color.FromArgb(170, 255, 165, 0));

                            if (di < 3.1)
                            {
                                int fuel = 500;
                                int maxfuel = maxtankerfuel;
                                if (tankerfuel + fuel <= maxfuel)
                                    maxfuel = fuel;
                                else
                                    maxfuel = maxtankerfuel - tankerfuel;

                                if (maxfuel > 0)
                                {
                                    HUD.ShowHelp("Press ~INPUT_CONTEXT~ to fill the tank.~n~It will cost you " + refuelCost * maxfuel + " for " + maxfuel + " liters of fuel.");

                                    if (canBuyFuel)
                                        if (Input.IsControlJustPressed(Control.Context))
                                        {
                                            canBuyFuel = false;
                                            BaseScript.TriggerServerEvent("lprp:fuel:buyfuelfortanker", (int)(maxfuel * refuelCost), maxfuel);
                                        }
                                }
                                else
                                {
                                    HUD.ShowNotification("Your tanker is already full!");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!avviso1)
                    {
                        HUD.ShowNotification("You must be in a work truck and have a loaded tank attached to do this job!");
                        avviso1 = true;
                    }
                }

                float dist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, jobTruck.Position);

                if (dist > 40f)
                {
                    if (!distwarn)
                    {
                        distwarn = true;
                        HUD.ShowNotification("If you move too far from your vehicle your employment contract will be cancelled.", ColoreNotifica.Yellow);
                    }

                    if (dist > 60f)
                    {
                        jobTruck.Delete();
                        jobTrailer.Delete();
                        jobTruck = new Vehicle(0);
                        jobTrailer = new Vehicle(0);
                        curRegPickup = 0;
                        canRegisterForTanker = true;
                        canPickupTanker = true;
                        canUnloadFuel = true;
                        distwarn = false;
                        hasTanker = false;
                        HideRefuelBlips();
                        HUD.ShowNotification("You went too far from your truck and it was towed.", ColoreNotifica.Red, true);
                    }
                }

                if (hasTanker)
                    if (!jobTruck.Exists())
                    {
                        string plate = GetVehicleNumberPlateText(jobTruck.Handle);
                        jobTruck.Delete();
                        jobTrailer.Delete();
                        jobTruck = new Vehicle(0);
                        jobTrailer = new Vehicle(0);
                        if (plate != "") BaseScript.TriggerServerEvent("lprp:vehicles:unregisterJobVehicle", plate);
                        HUD.ShowNotification("You have lost your truck or your tank. The delivery has been cancelled.", ColoreNotifica.Red);
                        curRegPickup = 0;
                        canRegisterForTanker = true;
                        canPickupTanker = true;
                        canUnloadFuel = true;
                        hasTanker = false;
                        HideRefuelBlips();
                    }
            }

            await Task.FromResult(0);
        }
    }

    public class SingleStation
    {
        public int stationfuel = 0;
        public int stationprice = 0;
        public SingleStation() { }

        public SingleStation(int fuel, int price)
        {
            stationfuel = fuel;
            stationprice = price;
        }
    }

    public class Tanker
    {
        public Vector3 pos;
        public float heading;
        public Vector3 t;
        public float theading;
        public float ppu;
        public int fuelForTanker;

        public Tanker(Vector3 p, float h, Vector3 t, float th, float ppu, int fuel)
        {
            pos = p;
            heading = h;
            this.t = t;
            theading = th;
            this.ppu = ppu;
            fuelForTanker = fuel;
        }
    }
}