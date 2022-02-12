﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Whitelistati.Polizia
{
    internal static class PoliziaMainClient
    {
        public static List<Vehicle> VeicoliPolizia = new List<Vehicle>();
        public static Vehicle VeicoloAttuale = new Vehicle(0);
        public static Vehicle ElicotteroAttuale = new Vehicle(0);
        public static Dictionary<Ped, Blip> CopsBlips = new Dictionary<Ped, Blip>();
        public static bool InServizioDaPilota = false;

        public static void Init()
        {
            Client.Instance.AddEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));
            Client.Instance.AddEventHandler("lprp:polizia:ammanetta_smanetta", new Action(AmmanettaSmanetta));
            Client.Instance.AddEventHandler("lprp:polizia:accompagna", new Action<int>(Accompagna));
            Client.Instance.AddEventHandler("lprp:polizia:mettiVeh", new Action(MettiVeh));
            Client.Instance.AddEventHandler("lprp:polizia:togliVeh", new Action(TogliVeh));
        }

        public static void Stop()
        {
            Client.Instance.RemoveEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));
            Client.Instance.RemoveEventHandler("lprp:polizia:ammanetta_smanetta", new Action(AmmanettaSmanetta));
            Client.Instance.RemoveEventHandler("lprp:polizia:accompagna", new Action<int>(Accompagna));
            Client.Instance.RemoveEventHandler("lprp:polizia:mettiVeh", new Action(MettiVeh));
            Client.Instance.RemoveEventHandler("lprp:polizia:togliVeh", new Action(TogliVeh));
        }

        public static void Spawnato()
        {
            foreach (StazioniDiPolizia stazione in Client.Impostazioni.RolePlay.Lavori.Polizia.Config.Stazioni)
            {
                Blip blip = new Blip(AddBlipForCoord(stazione.Blip.Coords.X, stazione.Blip.Coords.Y, stazione.Blip.Coords.Z))
                {
                    Sprite = (BlipSprite)stazione.Blip.Sprite,
                    Scale = stazione.Blip.Scale,
                    Color = (BlipColor)stazione.Blip.Color,
                    IsShortRange = true,
                    Name = "Stazione di Polizia"
                };
                SetBlipDisplay(blip.Handle, stazione.Blip.Display);
            }
        }

        private static async void AmmanettaSmanetta()
        {
            Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.Ammanettato = !Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.Ammanettato;
            RequestAnimDict("mp_arresting");
            while (!HasAnimDictLoaded("mp_arresting")) await BaseScript.Delay(1);

            if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.Ammanettato)
            {
                Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();
                Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation("mp_arrestring", "idle", 8f, -1, (AnimationFlags)49);
                Cache.PlayerCache.MyPlayer.Ped.Weapons.Select(WeaponHash.Unarmed);
                SetEnableHandcuffs(PlayerPedId(), true);
                DisablePlayerFiring(PlayerId(), true);
                Cache.PlayerCache.MyPlayer.Ped.CanPlayGestures = false;
                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex.ToLower() == "femmina")
                    SetPedComponentVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, 7, 25, 0, 0);
                else
                    SetPedComponentVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, 7, 41, 0, 0);
                Cache.PlayerCache.MyPlayer.Player.CanControlCharacter = false;
                Client.Instance.AddTick(Ammanettato);
            }
            else
            {
                Client.Instance.RemoveTick(Ammanettato);
                Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();
                SetEnableHandcuffs(PlayerPedId(), false);
                UncuffPed(PlayerPedId());
                SetPedComponentVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Accessori, Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Accessori, 0, 0);
                SetEnableHandcuffs(PlayerPedId(), false);
                DisablePlayerFiring(PlayerId(), false);
                Cache.PlayerCache.MyPlayer.Ped.CanPlayGestures = true;
                Cache.PlayerCache.MyPlayer.Player.CanControlCharacter = true;
            }
        }

        private static async void Accompagna(int ped)
        {
            Ped pol = (Ped)Entity.FromNetworkId(ped);
            if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.Ammanettato) Cache.PlayerCache.MyPlayer.Ped.Task.FollowToOffsetFromEntity(pol, new Vector3(1f, 1f, 0), 3f, -1, 1f, true);
        }

        private static async void TogliVeh()
        {
            if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.Ammanettato)
                if (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo)
                    Cache.PlayerCache.MyPlayer.Ped.Task.LeaveVehicle();
        }

        private static async void MettiVeh()
        {
            if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.Ammanettato)
            {
                Vehicle closestVeh = Cache.PlayerCache.MyPlayer.Ped.GetClosestVehicle();
                if (closestVeh.IsSeatFree(VehicleSeat.LeftRear))
                    Cache.PlayerCache.MyPlayer.Ped.Task.EnterVehicle(closestVeh, VehicleSeat.LeftRear);
                else if (Cache.PlayerCache.MyPlayer.Ped.LastVehicle.IsSeatFree(VehicleSeat.RightRear)) Cache.PlayerCache.MyPlayer.Ped.Task.EnterVehicle(closestVeh, VehicleSeat.LeftRear);
            }
        }

        public static async Task MarkersPolizia()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "polizia")
                foreach (StazioniDiPolizia t2 in Client.Impostazioni.RolePlay.Lavori.Polizia.Config.Stazioni)
                {
                    foreach (Position t in t2.Spogliatoio)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, .5f), Colors.Blue, false, false, true);

                        if (!p.IsInRangeOf(t.ToVector3, 1.375f)) continue;
                        HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per cambiarti ed entrare/uscire in ~g~Servizio~w~");
                        if (Input.IsControlJustPressed(Control.Context)) MenuPolizia.CloakRoomMenu();
                    }

                    foreach (Position t in t2.Armerie) World.DrawMarker(MarkerType.HorizontalCircleSkinny, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, .5f), Colors.Red, false, false, true);

                    foreach (SpawnerSpawn t1 in t2.Veicoli)
                    {
                        World.DrawMarker(MarkerType.CarSymbol, t1.SpawnerMenu.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Blue, false, false, true);

                        if (p.IsInRangeOf(t1.SpawnerMenu.ToVector3, 1.375f) && !HUD.MenuPool.IsAnyMenuOpen)
                        {
                            HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere il veicolo");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                MenuPolizia.VehicleMenuNuovo(t2, t1);
                            }
                        }

                        foreach (Position t in t1.Deleters)
                            if (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo)
                            {
                                World.DrawMarker(MarkerType.CarSymbol, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Red, false, false, true);

                                if (!p.IsInRangeOf(t.ToVector3, 1.375f) || HUD.MenuPool.IsAnyMenuOpen) continue;
                                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo nel deposito");

                                if (Input.IsControlJustPressed(Control.Context))
                                {
                                    if (p.CurrentVehicle.HasDecor("VeicoloPolizia"))
                                    {
                                        VeicoloPol vehicle = new VeicoloPol(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
                                        BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehPolizia", vehicle.ToJson());
                                        p.CurrentVehicle.Delete();
                                        VeicoloAttuale = new Vehicle(0);
                                    }
                                    else
                                    {
                                        HUD.ShowNotification("Il veicolo che tenti di posare non è della polizia!", NotificationColor.Red, true);
                                    }
                                }
                            }
                    }

                    foreach (SpawnerSpawn t1 in t2.Elicotteri)
                    {
                        World.DrawMarker(MarkerType.HelicopterSymbol, t1.SpawnerMenu.ToVector3, new Vector3(0), new Vector3(0), new Vector3(3f, 3f, 1.5f), Colors.Blue, false, false, true);

                        if (p.IsInRangeOf(t1.SpawnerMenu.ToVector3, 1.375f) && !HUD.MenuPool.IsAnyMenuOpen)
                        {
                            HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere l'elicottero");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                MenuPolizia.HeliMenu(t2, t1);
                            }
                        }

                        foreach (Position t in t1.Deleters)
                        {
                            if (!Funzioni.IsSpawnPointClear(t.ToVector3, 2f))
                                foreach (Vehicle veh in Funzioni.GetVehiclesInArea(t.ToVector3, 2f))
                                    if (!veh.HasDecor("VeicoloPolizia"))
                                        veh.Delete();

                            if (!p.IsInHeli) continue;
                            {
                                World.DrawMarker(MarkerType.HelicopterSymbol, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, 1.5f), Colors.Red, false, false, true);

                                if (!p.IsInRangeOf(t.ToVector3, 3.375f) || !p.IsInHeli || HUD.MenuPool.IsAnyMenuOpen) continue;
                                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare l'elicottero nel deposito");

                                if (Input.IsControlJustPressed(Control.Context))
                                {
                                    if (p.CurrentVehicle.HasDecor("VeicoloPolizia"))
                                    {
                                        VeicoloPol veh = new VeicoloPol(p.CurrentVehicle.Mods.LicensePlate, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle);
                                        BaseScript.TriggerServerEvent("lprp:polizia:RimuoviVehPolizia", veh.ToJson());
                                        Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Delete();
                                        ElicotteroAttuale = new Vehicle(0);
                                    }
                                    else
                                    {
                                        HUD.ShowNotification("Il veicolo che tenti di posare non è della polizia!", NotificationColor.Red, true);
                                    }
                                }
                            }
                        }
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade != Client.Impostazioni.RolePlay.Lavori.Polizia.Gradi.Count - 1) continue;
                    foreach (Position t in t2.BossActions) World.DrawMarker(MarkerType.HorizontalCircleSkinny, t.ToVector3, new Vector3(0), new Vector3(0), new Vector3(2f, 2f, .5f), Colors.Blue, false, false, true);
                }
            else
                await BaseScript.Delay(5000);

            await Task.FromResult(0);
        }

        public static async Task AbilitaBlipVolanti()
        {
            await BaseScript.Delay(1000);

            if (Client.Impostazioni.RolePlay.Lavori.Polizia.Config.AbilitaBlipVolanti)
            {
                foreach (var p in Cache.PlayerCache.GiocatoriOnline)
                {
                    if (p.User.CurrentChar.Job.Name == "Polizia")
                    {
                        int id = GetPlayerFromServerId(p.Player.ServerId);
                        Ped playerPed = new(GetPlayerPed(id));

                        if (!NetworkIsPlayerActive(id) || playerPed.Handle == Cache.PlayerCache.MyPlayer.Ped.Handle) continue;

                        if (playerPed.IsInVehicle())
                        {
                            if (!playerPed.CurrentVehicle.HasDecor("VeicoloPolizia")) continue;

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
                Client.Instance.RemoveTick(AbilitaBlipVolanti);
            }
        }

        public static async Task MainTickPolizia()
        {
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name == "Polizia")
                if (Input.IsControlJustPressed(Control.SelectCharacterFranklin, PadCheck.Keyboard) && !HUD.MenuPool.IsAnyMenuOpen)
                    MenuPolizia.MainMenu();
            await Task.FromResult(0);
        }

        public static async Task Ammanettato()
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