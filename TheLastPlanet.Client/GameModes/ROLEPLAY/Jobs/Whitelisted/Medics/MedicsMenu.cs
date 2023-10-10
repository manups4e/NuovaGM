using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Vehicles;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.Medics
{
    internal static class MedicsMenu
    {
        #region Locker

        public static async void LockerMenu()
        {
            UIMenu locker = new("Locker Menu", "Go on / off duty", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem change;
            if (!PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty)
            {
                change = new UIMenuItem("Go on duty", "You took an oath.");
            }
            else
            {
                change = new UIMenuItem("Go off duty", "Turn is over");
            }
            locker.AddItem(change);
            change.Activated += async (item, index) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                MenuHandler.CloseAndClearHistory();
                NetworkFadeOutEntity(PlayerPedId(), true, false);

                if (!Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty)
                {
                    foreach (KeyValuePair<string, JobGrade> Grado in Client.Settings.RolePlay.Jobs.Medics.Grades.Where(Grado => Grado.Value.Id == Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade))
                    {
                        switch (PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex)
                        {
                            case "Male":
                                GetChanged(Grado.Value.WorkUniforms.Male);
                                break;
                            case "Female":
                                GetChanged(Grado.Value.WorkUniforms.Female);
                                break;
                        }
                    }

                    Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty = true;
                }
                else
                {
                    Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty = false;
                    Functions.UpdateDress(PlayerPedId(), Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                }

                await BaseScript.Delay(500);
                Screen.Fading.FadeIn(800);
                NetworkFadeInEntity(PlayerPedId(), true);
            };
            locker.Visible = true;
        }

        public static async void GetChanged(Uniform dress)
        {
            int id = PlayerPedId();
            SetPedComponentVariation(id, (int)DrawableIndexes.Face, dress.Dress.Face, dress.TextureDress.Face, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Mask, dress.Dress.Mask, dress.TextureDress.Mask, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Torso, dress.Dress.Torso, dress.TextureDress.Torso, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Pants, dress.Dress.Pants, dress.TextureDress.Pants, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Bag_Parachute, dress.Dress.Bag_Parachute, dress.TextureDress.Bag_Parachute, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Shoes, dress.Dress.Shoes, dress.TextureDress.Shoes, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Accessories, dress.Dress.Accessories, dress.TextureDress.Accessories, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Undershirt, dress.Dress.Undershirt, dress.TextureDress.Undershirt, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Kevlar, dress.Dress.Kevlar, dress.TextureDress.Kevlar, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Badge, dress.Dress.Badge, dress.TextureDress.Badge, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Torso_2, dress.Dress.Torso_2, dress.TextureDress.Torso_2, 2);
            SetPedPropIndex(id, (int)PropIndexes.Hats_Masks, dress.Accessories.Hats_masks, dress.TexturesAccessories.Hats_masks, true);
            SetPedPropIndex(id, (int)PropIndexes.Ears, dress.Accessories.Ears, dress.TexturesAccessories.Ears, true);
            SetPedPropIndex(id, (int)PropIndexes.Glasses, dress.Accessories.Glasses, dress.TexturesAccessories.Glasses, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_3, dress.Accessories.Unk_3, dress.TexturesAccessories.Unk_3, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_4, dress.Accessories.Unk_4, dress.TexturesAccessories.Unk_4, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_5, dress.Accessories.Unk_5, dress.TexturesAccessories.Unk_5, true);
            SetPedPropIndex(id, (int)PropIndexes.Watches, dress.Accessories.Watches, dress.TexturesAccessories.Watches, true);
            SetPedPropIndex(id, (int)PropIndexes.Bracelets, dress.Accessories.Bracelets, dress.TexturesAccessories.Bracelets, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_8, dress.Accessories.Unk_8, dress.TexturesAccessories.Unk_8, true);
        }

        #endregion

        //TODO: INCOMPLETE MENU
        #region Pharmacy
        public static async void PharmacyMenu()
        {
            UIMenu pharmacy = new("Pharmacy and Medicines", "With or without prescription?", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            pharmacy.Visible = true;
        }

        #endregion

        //TODO: INCOMPLETE MENU
        #region Interaction Menu

        public static async void InteractionMenu()
        {
            UIMenu MenuMedico = new("Interaction Menu", "Let's save some lives!", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem controlloFerite = new("Check wounds", "Where does it hurt?");
            UIMenuItem cpr = new("Try CPR", "Warning: it might fail!");
            MenuMedico.AddItem(controlloFerite);
            MenuMedico.AddItem(cpr);
            MenuMedico.Visible = true;
        }

        #endregion

        #region MenuVehicles

        private static List<Vehicle> ParkedVehicles = new();
        private static Hospital ActualStation = new();
        private static int GarageLevel = 0;
        private static List<Vector4> parkingPoints = new()
        {
            new(224.500f, -998.695f, -99.6f, 225.0f),
            new(224.500f, -994.630f, -99.6f, 225.0f),
            new(224.500f, -990.255f, -99.6f, 225.0f),
            new(224.500f, -986.628f, -99.6f, 225.0f),
            new(224.500f, -982.496f, -99.6f, 225.0f),
            new(232.500f, -982.496f, -99.6f, 135.0f),
            new(232.500f, -986.628f, -99.6f, 135.0f),
            new(232.500f, -990.255f, -99.6f, 135.0f),
            new(232.500f, -994.630f, -99.6f, 135.0f),
            new(232.500f, -998.695f, -99.6f, 135.0f)
        };
        private static SpawnerSpawn actualPoint = new();
        private static bool InGarage = false;

        public static async void VehicleMenuNew(Hospital station, SpawnerSpawn point)
        {
            Cache.PlayerCache.MyPlayer.Status.Instance.InstancePlayer("MedicsVehiclesGarage");
            ActualStation = station;
            actualPoint = point;
            Cache.PlayerCache.MyPlayer.Ped.Position = new Vector3(236.349f, -1005.013f, -100f);
            Cache.PlayerCache.MyPlayer.Ped.Heading = 85.162f;
            InGarage = true;

            if (station.AuthorizedVehicles.Count(o => o.AuthorizedGrades[0] == -1 || o.AuthorizedGrades.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade)) <= 10)
            {
                for (int i = 0; i < station.AuthorizedVehicles.Count(o => o.AuthorizedGrades[0] == -1 || o.AuthorizedGrades.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade)); i++)
                {
                    ParkedVehicles.Add(await Functions.SpawnLocalVehicle(station.AuthorizedVehicles[i].Model, new Vector3(parkingPoints[i].X, parkingPoints[i].Y, parkingPoints[i].Z), parkingPoints[i].W));
                    ParkedVehicles[i].PlaceOnGround();
                    ParkedVehicles[i].IsPersistent = true;
                    ParkedVehicles[i].LockStatus = VehicleLockStatus.Unlocked;
                    ParkedVehicles[i].IsInvincible = true;
                    ParkedVehicles[i].IsCollisionEnabled = true;
                    ParkedVehicles[i].IsEngineRunning = false;
                    ParkedVehicles[i].IsDriveable = false;
                    ParkedVehicles[i].IsSirenActive = true;
                    ParkedVehicles[i].IsSirenSilent = true;
                    ParkedVehicles[i].SetDecor("MedicsVehicle", SharedMath.GetRandomInt(100));
                }
            }
            else
            {
                await GarageWithMoreVehicles(station.AuthorizedVehicles, GarageLevel);
            }

            await BaseScript.Delay(1000);
            Screen.Fading.FadeIn(800);
            Client.Instance.AddTick(GarageControl);
        }

        private static async Task GarageWithMoreVehicles(List<Authorized> authorized, int garageLevel)
        {
            foreach (Vehicle veh in ParkedVehicles) veh.Delete();
            ParkedVehicles.Clear();
            int total = authorized.Count(o => o.AuthorizedGrades[0] == -1 || o.AuthorizedGrades.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade));
            int currentGarageLevel = total - garageLevel * 10 > garageLevel * 10 ? 10 : total - garageLevel * 10;

            for (int i = 0; i < currentGarageLevel; i++)
            {
                ParkedVehicles.Add(await Functions.SpawnLocalVehicle(authorized[i + garageLevel * 10].Model, new Vector3(parkingPoints[i].X, parkingPoints[i].Y, parkingPoints[i].Z), parkingPoints[i].W));
                ParkedVehicles[i].PlaceOnGround();
                ParkedVehicles[i].IsPersistent = true;
                ParkedVehicles[i].LockStatus = VehicleLockStatus.Unlocked;
                ParkedVehicles[i].IsInvincible = true;
                ParkedVehicles[i].IsCollisionEnabled = true;
                ParkedVehicles[i].IsEngineRunning = false;
                ParkedVehicles[i].IsDriveable = false;
                ParkedVehicles[i].IsSirenActive = true;
                ParkedVehicles[i].IsSirenSilent = true;
                ParkedVehicles[i].SetDecor("MedicsVehicle", SharedMath.GetRandomInt(100));
            }
        }

        private static async Task GarageControl()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.Instance.Instanced)
            {
                if (InGarage)
                {
                    if (p.IsInRangeOf(new Vector3(240.317f, -1004.901f, -99f), 3f))
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to change floor");
                        if (Input.IsControlJustPressed(Control.Context)) MenuFloor();
                    }

                    if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                    {
                        if (p.CurrentVehicle.HasDecor("MedicsVehicle"))
                        {
                            HUD.ShowHelp("To choose this vehicle and exit the garage,~n~~y~turn on the engine~w~ and ~y~accelerate~w~.");

                            if (Input.IsControlJustPressed(Control.VehicleAccelerate) && p.CurrentVehicle.IsEngineRunning)
                            {
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                int model = p.CurrentVehicle.Model.Hash;
                                foreach (Vehicle vehicle in ParkedVehicles) vehicle.Delete();
                                ParkedVehicles.Clear();

                                for (int i = 0; i < actualPoint.SpawnPoints.Count; i++)
                                    if (!Functions.IsSpawnPointClear(actualPoint.SpawnPoints[i].Coords.ToVector3, 2f))
                                    {
                                        continue;
                                    }
                                    else if (Functions.IsSpawnPointClear(actualPoint.SpawnPoints[i].Coords.ToVector3, 2f))
                                    {
                                        MedicsMainClient.ActualVehicle = await Functions.SpawnVehicle(model, actualPoint.SpawnPoints[i].Coords.ToVector3, actualPoint.SpawnPoints[i].Heading);

                                        break;
                                    }
                                    else
                                    {
                                        MedicsMainClient.ActualVehicle = await Functions.SpawnVehicle(model, actualPoint.SpawnPoints[0].Coords.ToVector3, actualPoint.SpawnPoints[0].Heading);

                                        break;
                                    }

                                p.CurrentVehicle.SetVehicleFuelLevel(100f);
                                p.CurrentVehicle.IsEngineRunning = true;
                                p.CurrentVehicle.IsDriveable = true;
                                //TODO: ADD A CHECK FOR COUNTED VEHICLE SO THAT PLATES ARE BASED ON COUNT AND UNIQUE
                                p.CurrentVehicle.Mods.LicensePlate = SharedMath.GetRandomInt(99) + "MED" + SharedMath.GetRandomInt(999);
                                p.CurrentVehicle.SetDecor("MedicsVehicle", SharedMath.GetRandomInt(100));
                                VehiclePolice veh = new(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
                                BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehMedici", veh.ToJson());
                                InGarage = false;
                                ActualStation = null;
                                actualPoint = null;
                                ParkedVehicles.Clear();
                                Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
                                await BaseScript.Delay(1000);
                                Screen.Fading.FadeIn(800);
                                Client.Instance.RemoveTick(GarageControl);
                            }
                        }
                    }
                }
            }
        }

        private static async void MenuFloor()
        {
            UIMenu elevator = new("Select floor", "Upstairs or downstairs?", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem exit = new("Exit the Garage");
            elevator.AddItem(exit);
            int count = ActualStation.AuthorizedVehicles.Count(o => o.AuthorizedGrades[0] == -1 || o.AuthorizedGrades.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade));
            int floors = 1;
            for (int i = 1; i < count + 1; i++)
                if (i % 10 == 0)
                    floors++;

            for (int i = 0; i < floors; i++)
            {
                UIMenuItem floor = new($"{i + 1}° floor");
                elevator.AddItem(floor);
                if (i == GarageLevel) floor.SetRightBadge(BadgeIcon.CAR);
            }

            elevator.OnItemSelect += async (menu, item, index) =>
            {
                if (item.RightBadge == BadgeIcon.CAR)
                {
                    HUD.ShowNotification("This is the current garage!!", true);
                }
                else
                {
                    MenuHandler.CloseAndClearHistory();
                    Screen.Fading.FadeOut(800);
                    await BaseScript.Delay(1000);

                    if (item == exit)
                    {
                        Cache.PlayerCache.MyPlayer.Ped.Position = ActualStation.Vehicles[ActualStation.Vehicles.IndexOf(actualPoint)].SpawnerMenu.ToVector3;
                        InGarage = false;
                        ActualStation = null;
                        actualPoint = null;
                        Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
                        ParkedVehicles.Clear();
                        Client.Instance.RemoveTick(GarageControl);
                    }
                    else
                    {
                        GarageLevel = index - 1;
                        await GarageWithMoreVehicles(ActualStation.AuthorizedVehicles, GarageLevel);
                    }

                    await BaseScript.Delay(1000);
                    Screen.Fading.FadeIn(800);
                }
            };
            elevator.Visible = true;
        }

        #endregion

        #region MenuHeli

        private static Vehicle PreviewHeli = new(0);
        private static Camera HeliCam = new(0);

        public static async void HeliMenu(Hospital station, SpawnerSpawn point)
        {
            LoadInterior(GetInteriorAtCoords(-1267.0f, -3013.135f, -48.5f));
            RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
            RequestAdditionalCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
            HeliCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
            HeliCam.Position = new Vector3(-1268.174f, -2999.561f, -44.215f);
            HeliCam.IsActive = true;
            await BaseScript.Delay(1000);
            UIMenu menuHeli = new("Medics Copters", "Healing from above!", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);

            for (int i = 0; i < station.AutorizedHelicopters.Count; i++)
            {
                UIMenuItem veh = new(station.AutorizedHelicopters[i].Name);
                menuHeli.AddItem(veh);
            }

            menuHeli.OnIndexChange += async (menu, index) =>
            {
                PreviewHeli = await Functions.SpawnLocalVehicle(station.AutorizedHelicopters[index].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), point.SpawnPoints[0].Heading);
                PreviewHeli.IsCollisionEnabled = false;
                PreviewHeli.IsPersistent = true;
                PreviewHeli.PlaceOnGround();
                PreviewHeli.IsPositionFrozen = true;
                if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 1);
                PreviewHeli.LockStatus = VehicleLockStatus.Locked;
                SetHeliBladesFullSpeed(PreviewHeli.Handle);
                PreviewHeli.IsInvincible = true;
                PreviewHeli.IsEngineRunning = true;
                PreviewHeli.IsDriveable = false;
                if (HeliCam.IsActive && PreviewHeli.Exists()) HeliCam.PointAt(PreviewHeli);
            };
            menuHeli.OnItemSelect += async (menu, item, index) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                HeliCam.IsActive = false;
                RenderScriptCams(false, false, 0, false, false);

                foreach (SpawnPoints t in point.SpawnPoints)
                    if (!Functions.IsSpawnPointClear(t.Coords.ToVector3, 2f))
                    {
                        continue;
                    }
                    else if (Functions.IsSpawnPointClear(t.Coords.ToVector3, 2f))
                    {
                        MedicsMainClient.ActualHeli = await Functions.SpawnVehicle(station.AutorizedHelicopters[index].Model, t.Coords.ToVector3, t.Heading);

                        break;
                    }
                    else
                    {
                        MedicsMainClient.ActualHeli = await Functions.SpawnVehicle(station.AutorizedHelicopters[index].Model, point.SpawnPoints[0].Coords.ToVector3, point.SpawnPoints[0].Heading);

                        break;
                    }

                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.SetVehicleFuelLevel(100f);
                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsDriveable = true;
                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate = SharedMath.GetRandomInt(99) + "MED" + SharedMath.GetRandomInt(999);
                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.SetDecor("MedicsVehicle", SharedMath.GetRandomInt(100));
                if (Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash == 353883353) SetVehicleLivery(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle, 1);
                VehiclePolice veh = new(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle);
                BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehMedici", veh.ToJson());
                MenuHandler.CloseAndClearHistory();
                PreviewHeli.MarkAsNoLongerNeeded();
                PreviewHeli.Delete();
            };
            menuHeli.OnMenuOpen += async (a, b) =>
            {
                PreviewHeli = await Functions.SpawnLocalVehicle(station.AutorizedHelicopters[0].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), 0);
                PreviewHeli.IsCollisionEnabled = false;
                PreviewHeli.IsPersistent = true;
                PreviewHeli.PlaceOnGround();
                PreviewHeli.IsPositionFrozen = true;
                if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 1); // 0 for police, 1 for medics.. only for emergency copters!
                PreviewHeli.LockStatus = VehicleLockStatus.Locked;
                PreviewHeli.IsInvincible = true;
                PreviewHeli.IsEngineRunning = true;
                PreviewHeli.IsDriveable = false;
                Client.Instance.AddTick(Heading);
                HeliCam.PointAt(PreviewHeli);
                if (GetInteriorFromEntity(PreviewHeli.Handle) != 0) SetFocusEntity(PreviewHeli.Handle);
                while (!HasCollisionLoadedAroundEntity(PreviewHeli.Handle)) await BaseScript.Delay(1000);
                RenderScriptCams(true, false, 0, false, false);
                Screen.Fading.FadeIn(800);
            };
            menuHeli.OnMenuClose += async (a) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                Client.Instance.RemoveTick(Heading);
                HeliCam.IsActive = false;
                RenderScriptCams(false, false, 0, false, false);
                ClearFocus();
                await BaseScript.Delay(1000);
                Screen.Fading.FadeIn(800);
                PreviewHeli.MarkAsNoLongerNeeded();
                PreviewHeli.Delete();
            };
            menuHeli.Visible = true;
        }

        #endregion

        private static async Task Heading()
        {
            if (PreviewHeli.Exists())
            {
                RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
                PreviewHeli.Heading += 1;
                if (!PreviewHeli.IsEngineRunning) SetHeliBladesFullSpeed(PreviewHeli.Handle);
            }
        }
    }
}