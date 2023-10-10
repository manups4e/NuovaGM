using System;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Vehicles;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Generics.Towing
{
    public class TowingClient
    {
        private static Settings.Shared.Roleplay.Jobs.Generics.Towing Towing;
        private static Vehicle workVehicle;
        private static Vehicle towableVehicle;
        private static Blip blipTowableVehicle;
        private static Blip deliveryPoint;
        private static Vector4 spawnPoint;
        private static TextTimerBar vehicleTimer = new TextTimerBar("Vehicle to be towed", "");
        private static int towingTime;
        private static bool distwarn = false;
        private static Blip rem;
        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }

        public static void Spawned(PlayerClient client)
        {
            Towing = Client.Settings.RolePlay.Jobs.Generics.Towing;
            //RequestAnimDict("oddjobs@towing");

            //IsVehicleAttachedToTowTruck(int towtruck, int vehicle);
            //GetEntityAttachedToTowTruck(int towtruck);
            //SetVehicleSiren(int towtruck, bool toggle); // quando stai agganciando / trasportando
            //DetachVehicleFromTowTruck
            //animazioni
            //"oddjobs@towingcome_here", "come_here_idle_a"
            //"oddjobs@towing", "Start_Engine_Loop"
            //"oddjobs@towing", "Start_Engine_Exit"
            //"oddjobs@towingpleadingidle_b", "idle_d"
            /*

			func_130(Local_2996[0].f_6, &uLocal_3042, &uLocal_3044);
			SET_FORCE_HD_VEHICLE(Local_2996[0].f_6, 1);
			SET_VEHICLE_TYRES_CAN_BURST(Local_2996[0].f_6, 0);
			SET_ENTITY_LOAD_COLLISION_FLAG(Local_2996[0].f_6, 1, 1);
			SET_VEHICLE_HAS_STRONG_AXLES(Local_2996[0].f_6, 1);
			return 1;
			*/

            // cercare "hint" per le telecamere.. fiiiiiigo
            rem = World.CreateBlip(Towing.JobStart);
            rem.Sprite = BlipSprite.TowTruck;
            rem.Name = "Roadside assistance";
            rem.IsShortRange = true;
            SetBlipDisplay(rem.Handle, 4);
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            Towing = Client.Settings.RolePlay.Jobs.Generics.Towing;
            rem.Delete();
        }

        public static async Task StartJob()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name != "Rimozione forzata")
            {
                if (p.IsInRangeOf(Towing.JobStart, 50)) World.DrawMarker(MarkerType.TruckSymbol, Towing.JobStart, new Vector3(0), new Vector3(0), new Vector3(2.5f, 2.5f, 2.5f), Colors.Brown, true, false, true);

                if (p.IsInRangeOf(Towing.JobStart, 1.375f))
                {
                    HUD.ShowHelp("Do you want to work in the magical world of ~y~roadside assistance~w~?\nPress ~INPUT_CONTEXT~ to accept a working contract!");

                    if (Input.IsControlJustPressed(Control.Context))
                    {
                        Screen.Fading.FadeOut(800);
                        await BaseScript.Delay(1000);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name = "Forced removal";
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade = 0;
                        workVehicle = await Functions.SpawnVehicle("towtruck", new Vector3(401.55f, -1631.309f, 29.3f), 140);
                        workVehicle.SetDecor("WorkVehicle", workVehicle.Handle);
                        workVehicle.PlaceOnGround();
                        workVehicle.PreviouslyOwnedByPlayer = true;
                        workVehicle.Repair();
                        workVehicle.SetVehicleFuelLevel(100f);
                        Client.Instance.AddTick(TowingJob);
                        Client.Instance.AddTick(ControlloRimozione);
                        Client.Instance.RemoveTick(StartJob);
                        Screen.Fading.FadeIn(800);
                    }
                }
            }

            await Task.FromResult(0);
        }

        public static async Task ControlloRimozione()
        {
            if (workVehicle != null)
            {
                float dist = Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, workVehicle.Position);
                if (dist > 48 && dist < 80) Screen.ShowSubtitle("~r~Warning~w~!! You are straying too far from your work vehicle!!", 1);

                if (dist > 80)
                {
                    Client.Instance.RemoveTick(TowingJob);
                    Client.Instance.RemoveTick(ControlloRimozione);
                    Client.Instance.AddTick(StartJob);
                    if (HUD.TimerBarPool.ToList().Contains(vehicleTimer)) HUD.TimerBarPool.Remove(vehicleTimer);
                    HUD.ShowNotification("You strayed too far from your vehicle, the vehicle was taken back to the company and you lost your job!", ColoreNotifica.Red, true);
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name = "Unemployed";
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade = 0;

                    if (workVehicle != null && workVehicle.Exists())
                    {
                        workVehicle.Delete();
                        workVehicle = null;
                    }

                    if (blipTowableVehicle != null && blipTowableVehicle.Exists())
                    {
                        blipTowableVehicle.Delete();
                        blipTowableVehicle = null;
                    }

                    if (deliveryPoint != null && deliveryPoint.Exists())
                    {
                        deliveryPoint.Delete();
                        deliveryPoint = null;
                    }

                    if (towableVehicle != null && towableVehicle.Exists())
                    {
                        towableVehicle.Delete();
                        towableVehicle = null;
                    }
                }
            }
        }

        public static async Task TowingJob()
        {
            if (towableVehicle == null && towableVehicle.Exists())
            {
                await BaseScript.Delay(10000);
                spawnPoint = Towing.SpawnVehicles[SharedMath.GetRandomInt(Towing.SpawnVehicles.Count - 1)];

                while (Functions.GetVehiclesInArea(new Vector3(spawnPoint.X, spawnPoint.Y, spawnPoint.Z), 3f).ToList().FirstOrDefault(x => x.HasDecor("VeicoloRimozione")) != null)
                {
                    await BaseScript.Delay(0);
                    spawnPoint = Towing.SpawnVehicles[SharedMath.GetRandomInt(Towing.SpawnVehicles.Count - 1)];
                }

                // DEBUG
                Client.Logger.Debug("SpawnPoint = " + spawnPoint.ToString());
                uint streename = 0;
                uint crossing = 0;
                GetStreetNameAtCoord(spawnPoint.X, spawnPoint.Y, spawnPoint.Z, ref streename, ref crossing);
                string str = GetStreetNameFromHashKey(streename);
                string veicolo = Towing.TowableVehicles[SharedMath.GetRandomInt(Towing.TowableVehicles.Count - 1)];
                RequestCollisionAtCoord(spawnPoint.X, spawnPoint.Y, spawnPoint.Z);
                HUD.ShowAdvancedNotification("Vehicle", "To be removed", $"Vehicle to be removed in {str}", "CHAR_CALL911", TipoIcona.DollarIcon);
                blipTowableVehicle = World.CreateBlip(new Vector3(spawnPoint.X, spawnPoint.Y, spawnPoint.Z));
                blipTowableVehicle.Sprite = BlipSprite.PersonalVehicleCar;
                blipTowableVehicle.Color = BlipColor.Red;
                blipTowableVehicle.Name = "Vehicle to be towed";
                towingTime = Vector3.Distance(new Vector3(spawnPoint.X, spawnPoint.Y, spawnPoint.Z), Cache.PlayerCache.MyPlayer.Position.ToVector3) < 1000 ? SharedMath.GetRandomInt(60, 120) : SharedMath.GetRandomInt(120, 240);
                HUD.TimerBarPool.Add(vehicleTimer);
                Client.Instance.AddTick(TimerVehicle);

                while (Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, new Vector3(spawnPoint.X, spawnPoint.Y, spawnPoint.Z)) > 200 && towingTime > 0)
                {
                    if (workVehicle == null) return;
                    await BaseScript.Delay(0);
                }

                if (towingTime > 0)
                {
                    towableVehicle = await Functions.SpawnVehicleNoPlayerInside(veicolo, new Vector3(spawnPoint.X, spawnPoint.Y, spawnPoint.Z), spawnPoint.W);
                    while (towableVehicle == null) await BaseScript.Delay(0);
                    towableVehicle.IsPersistent = true;
                    towableVehicle.PlaceOnGround();
                    towableVehicle.PreviouslyOwnedByPlayer = true;
                    towableVehicle.Repair();
                    towableVehicle.LockStatus = VehicleLockStatus.Locked;
                    towableVehicle.SetDecor("TowVehicle", towableVehicle.Handle);
                    if (blipTowableVehicle.Exists()) blipTowableVehicle.Delete();
                    blipTowableVehicle = towableVehicle.AttachBlip();
                    blipTowableVehicle.Sprite = BlipSprite.PersonalVehicleCar;
                    blipTowableVehicle.Color = BlipColor.Red;
                    blipTowableVehicle.Name = "Vehicle to be towed";
                    HUD.ShowAdvancedNotification("Vehicle", "Vehicle to be towed", $"The vehicle to be removed is a ~y~{towableVehicle.LocalizedName}~w~ model with plate: ~y~{towableVehicle.Mods.LicensePlate}~w~", "CHAR_CALL911", TipoIcona.DollarIcon);
                }
            }

            if (workVehicle == null) return;
            while (Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, towableVehicle.Position) > 20 && towingTime > 0 && towableVehicle != null) await BaseScript.Delay(0);
            if (!IsVehicleAttachedToTowTruck(workVehicle.Handle, workVehicle.Handle) && Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(towableVehicle.Position, 10)) HUD.ShowHelp("~INPUT_VEH_MOVE_UD~ to control the hook.\n~INPUT_VEH_ROOF~ (keep pressed) to unhook the vehicle");
            if (GetEntityAttachedToTowTruck(workVehicle.Handle) != 0 && GetEntityAttachedToTowTruck(workVehicle.Handle) != towableVehicle.Handle) HUD.ShowHelp("You hooked the wrong vehicle!");

            //while (GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) != VeicoloDaRimuovere.Handle && Vector3.Distance(VeicoloDaRimuovere.Position, PuntoDiConsegna.Position) > 25) await BaseScript.Delay(0);
            if (GetEntityAttachedToTowTruck(workVehicle.Handle) == towableVehicle.Handle)
            {
                if (deliveryPoint == null)
                {
                    SetVehicleSiren(workVehicle.Handle, true);
                    SetForceHdVehicle(towableVehicle.Handle, true);
                    towableVehicle.CanTiresBurst = false;
                    SetEntityLoadCollisionFlag(towableVehicle.Handle, true);
                    towableVehicle.IsAxlesStrong = true;
                    deliveryPoint = World.CreateBlip(Towing.DespawnPoint[SharedMath.GetRandomInt(Towing.DespawnPoint.Count - 1)]);
                    deliveryPoint.ShowRoute = true;
                }
            }
            else
            {
                if (deliveryPoint != null && deliveryPoint.Exists() && !towableVehicle.IsInRangeOf(deliveryPoint.Position, 25))
                {
                    deliveryPoint.Delete();
                    deliveryPoint = null;
                }
            }

            if (deliveryPoint != null && towableVehicle.IsInRangeOf(deliveryPoint.Position, 25))
            {
                if (IsVehicleAttachedToTowTruck(workVehicle.Handle, towableVehicle.Handle))
                {
                    HUD.DrawText3D(deliveryPoint.Position.ToPosition(), Colors.WhiteSmoke, "Unhook the vehicle here to park it!");
                }
                else
                {
                    float money = 200 + towableVehicle.BodyHealth / 10;
                    BaseScript.TriggerServerEvent("lprp:givebank", money);
                    blipTowableVehicle.Delete();
                    blipTowableVehicle = null;
                    deliveryPoint.Delete();
                    deliveryPoint = null;
                    await BaseScript.Delay(2000);
                    towableVehicle.Delete();
                    towableVehicle = null;
                }
            }
        }

        private static async Task TimerVehicle()
        {
            while (towingTime > 0)
            {
                string time = towingTime > 59 ? towingTime - (int)Math.Floor(towingTime / 60f) * 60 < 10 ? $"{(int)Math.Floor(towingTime / 60f)}:0{towingTime - (int)Math.Floor(towingTime / 60f) * 60}" : $"{(int)Math.Floor(towingTime / 60f)}:{towingTime - (int)Math.Floor(towingTime / 60f) * 60}" : towingTime > 9 ? $"{towingTime}" : $"0{towingTime}";
                vehicleTimer.Caption = time;
                await BaseScript.Delay(1000);
                towingTime--;

                if (workVehicle != null && workVehicle.Exists() && GetEntityAttachedToTowTruck(workVehicle.Handle) != 0 && GetEntityAttachedToTowTruck(workVehicle.Handle) == towableVehicle.Handle)
                {
                    HUD.TimerBarPool.Remove(vehicleTimer);
                    Client.Instance.RemoveTick(TimerVehicle);

                    break;
                }

                if (workVehicle != null && !workVehicle.Exists() || workVehicle == null)
                {
                    Client.Instance.RemoveTick(TimerVehicle);

                    return;
                }

                await BaseScript.Delay(0);
            }

            if (towingTime == 0)
                if (GetEntityAttachedToTowTruck(workVehicle.Handle) == 0)
                {
                    HUD.TimerBarPool.Remove(vehicleTimer);
                    vehicleTimer = null;
                    HUD.ShowNotification("The vehicle to be removed is gone.", ColoreNotifica.Red, true);

                    if (towableVehicle != null)
                    {
                        towableVehicle.IsPersistent = false;
                        towableVehicle.PreviouslyOwnedByPlayer = false;
                        towableVehicle.Delete();
                        towableVehicle = null;
                    }

                    blipTowableVehicle.Delete();
                    blipTowableVehicle = null;
                    Client.Instance.RemoveTick(TimerVehicle);
                }
        }
    }
}