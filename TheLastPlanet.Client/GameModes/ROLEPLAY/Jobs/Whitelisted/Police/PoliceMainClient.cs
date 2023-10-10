using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.Police
{
    internal static class PoliceMainClient
    {
        public static List<Vehicle> PoliceVehicles = new List<Vehicle>();
        public static Vehicle CurrentVehicle = new Vehicle(0);
        public static Vehicle CurrentHelicopter = new Vehicle(0);
        public static Dictionary<Ped, Blip> CopsBlips = new Dictionary<Ped, Blip>();
        public static bool OnDutyAsPilot = false;

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            Client.Instance.AddEventHandler("lprp:polizia:ammanetta_smanetta", new Action(CuffUncuff));
            Client.Instance.AddEventHandler("lprp:polizia:accompagna", new Action<int>(Drag));
            Client.Instance.AddEventHandler("lprp:polizia:mettiVeh", new Action(PutInVeh));
            Client.Instance.AddEventHandler("lprp:polizia:togliVeh", new Action(RemoveFromVeh));
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("lprp:polizia:ammanetta_smanetta", new Action(CuffUncuff));
            Client.Instance.RemoveEventHandler("lprp:polizia:accompagna", new Action<int>(Drag));
            Client.Instance.RemoveEventHandler("lprp:polizia:mettiVeh", new Action(PutInVeh));
            Client.Instance.RemoveEventHandler("lprp:polizia:togliVeh", new Action(RemoveFromVeh));
        }

        public static void Spawned(PlayerClient client)
        {
            foreach (PoliceStation stazione in Client.Settings.RolePlay.Jobs.Police.Config.Stations)
            {
                Blip blip = new Blip(AddBlipForCoord(stazione.Blip.Coords.X, stazione.Blip.Coords.Y, stazione.Blip.Coords.Z))
                {
                    Sprite = (BlipSprite)stazione.Blip.Sprite,
                    Scale = stazione.Blip.Scale,
                    Color = (BlipColor)stazione.Blip.Color,
                    IsShortRange = true,
                    Name = "Police Station"
                };
                SetBlipDisplay(blip.Handle, stazione.Blip.Display);
            }
        }

        private static async void CuffUncuff()
        {
            PlayerCache.MyPlayer.Status.RolePlayStates.Cuffed = !PlayerCache.MyPlayer.Status.RolePlayStates.Cuffed;
            RequestAnimDict("mp_arresting");
            while (!HasAnimDictLoaded("mp_arresting")) await BaseScript.Delay(1);

            if (PlayerCache.MyPlayer.Status.RolePlayStates.Cuffed)
            {
                PlayerCache.MyPlayer.Ped.Task.ClearAll();
                PlayerCache.MyPlayer.Ped.Task.PlayAnimation("mp_arrestring", "idle", 8f, -1, (AnimationFlags)49);
                PlayerCache.MyPlayer.Ped.Weapons.Select(WeaponHash.Unarmed);
                SetEnableHandcuffs(PlayerPedId(), true);
                DisablePlayerFiring(PlayerId(), true);
                PlayerCache.MyPlayer.Ped.CanPlayGestures = false;
                if (PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex.ToLower() == "Female")
                    SetPedComponentVariation(PlayerCache.MyPlayer.Ped.Handle, 7, 25, 0, 0);
                else
                    SetPedComponentVariation(PlayerCache.MyPlayer.Ped.Handle, 7, 41, 0, 0);
                PlayerCache.MyPlayer.Player.CanControlCharacter = false;
                Client.Instance.AddTick(Cuffed);
            }
            else
            {
                Client.Instance.RemoveTick(Cuffed);
                Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();
                SetEnableHandcuffs(PlayerPedId(), false);
                UncuffPed(PlayerPedId());
                SetPedComponentVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Accessories, Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Accessories, 0, 0);
                SetEnableHandcuffs(PlayerPedId(), false);
                DisablePlayerFiring(PlayerId(), false);
                Cache.PlayerCache.MyPlayer.Ped.CanPlayGestures = true;
                Cache.PlayerCache.MyPlayer.Player.CanControlCharacter = true;
            }
        }

        private static async void Drag(int ped)
        {
            Ped pol = (Ped)Entity.FromNetworkId(ped);
            if (PlayerCache.MyPlayer.Status.RolePlayStates.Cuffed) PlayerCache.MyPlayer.Ped.Task.FollowToOffsetFromEntity(pol, new Vector3(1f, 1f, 0), 3f, -1, 1f, true);
        }

        private static async void RemoveFromVeh()
        {
            if (PlayerCache.MyPlayer.Status.RolePlayStates.Cuffed)
                if (PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                    PlayerCache.MyPlayer.Ped.Task.LeaveVehicle();
        }

        private static async void PutInVeh()
        {
            if (Cache.PlayerCache.MyPlayer.Status.RolePlayStates.Cuffed)
            {
                Vehicle closestVeh = Cache.PlayerCache.MyPlayer.Ped.GetClosestVehicle();
                if (closestVeh.IsSeatFree(VehicleSeat.LeftRear))
                    Cache.PlayerCache.MyPlayer.Ped.Task.EnterVehicle(closestVeh, VehicleSeat.LeftRear);
                else if (Cache.PlayerCache.MyPlayer.Ped.LastVehicle.IsSeatFree(VehicleSeat.RightRear)) Cache.PlayerCache.MyPlayer.Ped.Task.EnterVehicle(closestVeh, VehicleSeat.LeftRear);
            }
        }

        public static async Task MarkersPolice()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "police")
                foreach (PoliceStation t2 in Client.Settings.RolePlay.Jobs.Police.Config.Stations)
                {
                    foreach (Position t in t2.LockerRoom)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, .5f), Colors.Blue, false, false, true);

                        if (!p.IsInRangeOf(t.ToVector3, 1.375f)) continue;
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to go on/off ~g~duty~w~");
                        if (Input.IsControlJustPressed(Control.Context)) MenuPolice.CloakRoomMenu();
                    }

                    foreach (Position t in t2.Armouries) World.DrawMarker(MarkerType.HorizontalCircleSkinny, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, .5f), Colors.Red, false, false, true);

                    foreach (SpawnerSpawn t1 in t2.Vehicles)
                    {
                        World.DrawMarker(MarkerType.CarSymbol, t1.SpawnerMenu.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Blue, false, false, true);

                        if (p.IsInRangeOf(t1.SpawnerMenu.ToVector3, 1.375f) && !MenuHandler.IsAnyMenuOpen)
                        {
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to choose the vehicle");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                MenuPolice.VehicleMenu(t2, t1);
                            }
                        }

                        foreach (Position t in t1.Deleters)
                            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                            {
                                World.DrawMarker(MarkerType.CarSymbol, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Red, false, false, true);

                                if (!p.IsInRangeOf(t.ToVector3, 1.375f) || MenuHandler.IsAnyMenuOpen) continue;
                                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to park your vehicle");

                                if (Input.IsControlJustPressed(Control.Context))
                                {
                                    if (p.CurrentVehicle.HasDecor("VehiclePolice"))
                                    {
                                        VehiclePolice vehicle = new VehiclePolice(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
                                        BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehPolice", vehicle.ToJson());
                                        p.CurrentVehicle.Delete();
                                        CurrentVehicle = new Vehicle(0);
                                    }
                                    else
                                    {
                                        HUD.ShowNotification("The vehicle you are trying to park is not a police vehicle!", ColoreNotifica.Red, true);
                                    }
                                }
                            }
                    }

                    foreach (SpawnerSpawn t1 in t2.Copters)
                    {
                        World.DrawMarker(MarkerType.HelicopterSymbol, t1.SpawnerMenu.ToVector3, new Vector3(0), new Vector3(0), new Vector3(3f, 3f, 1.5f), Colors.Blue, false, false, true);

                        if (p.IsInRangeOf(t1.SpawnerMenu.ToVector3, 1.375f) && !MenuHandler.IsAnyMenuOpen)
                        {
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to choose the helicopter");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                MenuPolice.HeliMenu(t2, t1);
                            }
                        }

                        foreach (Position t in t1.Deleters)
                        {
                            if (!Functions.IsSpawnPointClear(t.ToVector3, 2f))
                                foreach (Vehicle veh in Functions.GetVehiclesInArea(t.ToVector3, 2f))
                                    if (!veh.HasDecor("VehiclePolice"))
                                        veh.Delete();

                            if (!p.IsInHeli) continue;
                            {
                                World.DrawMarker(MarkerType.HelicopterSymbol, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Red, false, false, true);

                                if (!p.IsInRangeOf(t.ToVector3, 3.375f) || !p.IsInHeli || MenuHandler.IsAnyMenuOpen) continue;
                                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to park the helicopter");

                                if (Input.IsControlJustPressed(Control.Context))
                                {
                                    if (p.CurrentVehicle.HasDecor("VehiclePolice"))
                                    {
                                        VehiclePolice veh = new VehiclePolice(p.CurrentVehicle.Mods.LicensePlate, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle);
                                        BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehPolice", veh.ToJson());
                                        Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Delete();
                                        CurrentHelicopter = new Vehicle(0);
                                    }
                                    else
                                    {
                                        HUD.ShowNotification("The vehicle you are trying to park is not a police vehicle!", ColoreNotifica.Red, true);
                                    }
                                }
                            }
                        }
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade != Client.Settings.RolePlay.Jobs.Police.Grades.Count - 1) continue;
                    foreach (Position t in t2.BossActions) World.DrawMarker(MarkerType.HorizontalCircleSkinny, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, .5f), Colors.Blue, false, false, true);
                }
            else
                await BaseScript.Delay(5000);

            await Task.FromResult(0);
        }

        public static async Task EnableBlipPolice()
        {
            await BaseScript.Delay(1000);

            if (Client.Settings.RolePlay.Jobs.Police.Config.EnableBlipsPolice)
            {
                foreach (PlayerClient p in Client.Instance.Clients)
                {
                    if (p.User.CurrentChar.Job.Name == "Police")
                    {
                        int id = GetPlayerFromServerId(p.Player.ServerId);
                        Ped playerPed = new(GetPlayerPed(id));

                        if (!NetworkIsPlayerActive(id) || playerPed.Handle == Cache.PlayerCache.MyPlayer.Ped.Handle) continue;

                        if (playerPed.IsInVehicle())
                        {
                            if (!playerPed.CurrentVehicle.HasDecor("VehiclePolice")) continue;

                            if (!CopsBlips.ContainsKey(playerPed))
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
                                CopsBlips.Add(playerPed, polblip);
                            }
                            else if (CopsBlips.ContainsKey(playerPed))
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
                            if (!CopsBlips.ContainsKey(playerPed)) continue;
                            foreach (Blip b in playerPed.AttachedBlips)
                                if (b.Sprite == BlipSprite.PoliceCar || b.Sprite == BlipSprite.PoliceCarDot || b.Sprite == BlipSprite.PoliceHelicopter || b.Sprite == BlipSprite.PoliceHelicopterAnimated || b.Sprite == BlipSprite.PersonalVehicleBike || b.Sprite == BlipSprite.Boat)
                                    b.Delete();
                            CopsBlips.Remove(playerPed);
                        }
                    }
                }
            }
            else
            {
                Client.Instance.RemoveTick(EnableBlipPolice);
            }
        }

        public static async Task MainTickPolice()
        {
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name == "Police")
                if (Input.IsControlJustPressed(Control.SelectCharacterFranklin, PadCheck.Keyboard) && !MenuHandler.IsAnyMenuOpen)
                    MenuPolice.MainMenu();
            await Task.FromResult(0);
        }

        public static async Task Cuffed()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;
            if (Cache.PlayerCache.MyPlayer.Player.CanControlCharacter) Cache.PlayerCache.MyPlayer.Player.CanControlCharacter = false;
            if (!p.IsCuffed) SetEnableHandcuffs(p.Handle, true);
            if (Cache.PlayerCache.MyPlayer.Player.CanControlCharacter) Cache.PlayerCache.MyPlayer.Player.CanControlCharacter = false;

            if (!IsEntityPlayingAnim(p.Handle, "mp_arresting", "idle", 3))
                if (!HasAnimDictLoaded("mp_arresting"))
                {
                    RequestAnimDict("mp_arresting");
                    while (!HasAnimDictLoaded("mp_arresting")) await BaseScript.Delay(10);
                    p.Task.PlayAnimation("mp_arrestring", "idle", 8f, -1, (AnimationFlags)49);
                }
        }
    }
}