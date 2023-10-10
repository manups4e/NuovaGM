using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.Races.Creator
{
    internal static class RaceCreator
    {
        private enum RotationDummyType
        {
            Heading,
            Roll,
            Pitch,
            Yaw
        }
        private static List<ObjectHash> placement = new()
        {
            ObjectHash.prop_mp_placement,
            ObjectHash.prop_mp_placement_lrg,
            ObjectHash.prop_mp_placement_maxd,
            ObjectHash.prop_mp_placement_med,
            ObjectHash.prop_mp_placement_red,
            ObjectHash.prop_mp_placement_sm
        };

        private static Camera enteringCamera;
        private static Prop cross;
        private static MarkerEx placeMarker;
        private static Vector3 curLocation;
        private static float Height = 0;
        private static Vector3 curRotation;
        private static Vector3 cameraPosition;
        //private static Vehicle cameraVeh;
        private static RaceTrack data = new RaceTrack();
        private static Prop DummyProp; // tempProp
        private static Vector3 dummyRot;
        private static RotationDummyType rotationDummyType = RotationDummyType.Heading;
        private static List<TrackPiece> Piazzati = new List<TrackPiece>();
        private static float zoom = 75f;
        private static List<dynamic> gridVehStart = new List<dynamic>() { "2 X 2" };
        private static int choosenCategory = 16;
        private static int propChoiceType = 0;
        private static int propColorChoosen = 0;
        private static int attachedBone;
        private static int popMultiplierSphere = 0;
        private static bool stacking = true;
        private static SnapOptions snapOptions = new SnapOptions();

        private static List<InstructionalButton> MainMenuButtons = new List<InstructionalButton>();
        private static List<InstructionalButton> CheckPointsButtons = new List<InstructionalButton>();
        private static List<InstructionalButton> PropsButtons = new List<InstructionalButton>();
        private static UIMenu Creator = new UIMenu("", "");
        public static async void CreatorPreparation()
        {
            PlayerCache.MyPlayer.Player.CanControlCharacter = false;
            float height = GetHeightmapTopZForPosition(199.4f, -934.3f);
            Vector3 rot = new(-90f, 0f, 0f);
            enteringCamera = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));
            enteringCamera.Position = new Vector3(199.4f, -934.3f, height);
            enteringCamera.Rotation = rot;
            enteringCamera.IsActive = true;
            enteringCamera.FarClip = 1000;
            enteringCamera.FieldOfView = 30;
            RenderScriptCams(true, false, 3000, true, false);
            SetFrontendActive(false);
            curRotation = enteringCamera.Rotation;
            PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;
            PlayerCache.MyPlayer.Ped.IsVisible = false;
            PlayerCache.MyPlayer.Ped.IsInvincible = true;
            PlayerCache.MyPlayer.Ped.DiesInstantlyInWater = false;
            PlayerCache.MyPlayer.Ped.Position = new Vector3(0, 0, 1000);
            Screen.Hud.IsRadarVisible = false;
            placeMarker ??= new MarkerEx(MarkerType.HorizontalCircleSkinny, WorldProbe.CrossairRenderingRaycastResult.HitPosition.ToPosition(), new Vector3(6.7f), 100f, SColor.HUD_Greydark);

            if (cross == null)
            {
                float pz = 0;
                GetGroundZFor_3dCoord(enteringCamera.Position.X, enteringCamera.Position.Y, enteringCamera.Position.Z, ref pz, false);
                cross = new Prop(CreateObjectNoOffset(Functions.HashUint("prop_mp_placement"), enteringCamera.Position.X, enteringCamera.Position.Y, pz, false, false, false));
                cross.IsVisible = true;
                SetEntityLoadCollisionFlag(cross.Handle, true);
                cross.LodDistance = 500;
                SetEntityCollision(cross.Handle, false, false);
                cameraPosition = enteringCamera.Position;
                Height = cross.Position.Z;
            }
            SetFocusEntity(cross.Handle);
            RequestAdditionalText("FMMC", 2);
            while (!HasAdditionalTextLoaded(2)) await BaseScript.Delay(0);
            Client.Instance.AddTick(MoveCamera);
            CreatorMainMenu();
            Screen.Fading.FadeIn(10);
        }

        static UIMenu propPlacing;
        public static async void CreatorMainMenu()
        {
            Creator = new("Race creator", "The Last Galaxy", new PointF(0, 0), "thelastgalaxy", "bannerbackground", false, true);
            Creator.MouseControlsEnabled = false;
            Creator.MouseWheelControlEnabled = false;
            UIMenuItem detailsiItem = new("Details");
            UIMenu detailsMenu = new("Details", "The Last Galaxy");
            UIMenuItem positioningItem = new("Positioning");
            UIMenu positioningMenu = new("Positioning", "The Last Galaxy");

            detailsiItem.Activated += async (a, b) => await Creator.SwitchTo(detailsMenu, 0, true);
            positioningItem.Activated += async (a, b) => await Creator.SwitchTo(positioningMenu, 0, true);
            Creator.AddItem(detailsiItem);
            Creator.AddItem(positioningItem);
            #region Dettagli

            UIMenuItem title = new("Title");
            title.SetRightBadge(BadgeIcon.WARNING);
            UIMenuItem description = new("Description");
            description.SetRightBadge(BadgeIcon.WARNING);
            UIMenuItem picture = new("Picture"); // TODO: HANDLE PICTURE TAKING.. HOW DOES INGAME CREATOR TO TAKE A PICTURE?
            UIMenuListItem raceType = new("Race Type", new List<dynamic>() { "Standard", "No contact" }, data.RaceType);
            UIMenuListItem lapType = new("Lap type", new List<dynamic>() { "Laps", "From point to point" }, data.LapType);
            UIMenuListItem lapsNumber = new("Number of Laps", new List<dynamic>() { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, data.Laps);
            UIMenuListItem playersNumber = new("Max Players", new List<dynamic>() { 32, 64 }, data.MaxPlayers);
            detailsMenu.AddItem(title);
            detailsMenu.AddItem(description);
            detailsMenu.AddItem(picture);
            detailsMenu.AddItem(raceType);
            detailsMenu.AddItem(lapType);
            detailsMenu.AddItem(lapsNumber);
            detailsMenu.AddItem(playersNumber);
            UIMenuItem availableVehsItem = new UIMenuItem("Available Vehicles", "Warning, for small grid only bykes will be available"); // TODO: ADD CLASSES AND SINGLE SELECTABLE VEHICLES
            UIMenu availableVehsMenu = new("Veicoli Disponibili", "The Last Galaxy");
            availableVehsItem.SetRightBadge(BadgeIcon.CAR);
            availableVehsItem.Activated += async (a, b) => await detailsMenu.SwitchTo(availableVehsMenu, 0, true);
            #region veicoliDisponibili
            UIMenuCheckboxItem Compacts = new UIMenuCheckboxItem("Compacts", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Compacts);
            UIMenuCheckboxItem Sedans = new UIMenuCheckboxItem("Sedans", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Sedans);
            UIMenuCheckboxItem SUVs = new UIMenuCheckboxItem("SUVs", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(SUVs);
            UIMenuCheckboxItem Coupes = new UIMenuCheckboxItem("Coupes", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Coupes);
            UIMenuCheckboxItem Muscle = new UIMenuCheckboxItem("Muscle", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Muscle);
            UIMenuCheckboxItem SportsClassics = new UIMenuCheckboxItem("SportsClassics", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(SportsClassics);
            UIMenuCheckboxItem Sports = new UIMenuCheckboxItem("Sports", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Sports);
            UIMenuCheckboxItem Super = new UIMenuCheckboxItem("Super", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Super);
            UIMenuCheckboxItem Motorcycles = new UIMenuCheckboxItem("Motorcycles", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Motorcycles);
            UIMenuCheckboxItem OffRoad = new UIMenuCheckboxItem("OffRoad", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(OffRoad);
            UIMenuCheckboxItem Industrial = new UIMenuCheckboxItem("Industrial", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Industrial);
            UIMenuCheckboxItem Utility = new UIMenuCheckboxItem("Utility", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Utility);
            UIMenuCheckboxItem Vans = new UIMenuCheckboxItem("Vans", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Vans);
            UIMenuCheckboxItem Cycles = new UIMenuCheckboxItem("Cycles", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Cycles);
            UIMenuCheckboxItem Boats = new UIMenuCheckboxItem("Boats", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Boats);
            UIMenuCheckboxItem Helicopters = new UIMenuCheckboxItem("Helicopters", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Helicopters);
            UIMenuCheckboxItem Planes = new UIMenuCheckboxItem("Planes", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Planes);
            UIMenuCheckboxItem Service = new UIMenuCheckboxItem("Service", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Service);
            UIMenuCheckboxItem Emergency = new UIMenuCheckboxItem("Emergency", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Emergency);
            UIMenuCheckboxItem Commercial = new UIMenuCheckboxItem("Commercial", UIMenuCheckboxStyle.Tick, false, "");
            availableVehsMenu.AddItem(Commercial);
            #endregion
            UIMenuListItem classDefault = new("Default class", new List<dynamic>() { "Super" }, 0); // TODO: HANDLE BASED ON GRID AND ALLOWED CLASSES/VEHICLES
            detailsMenu.AddItem(classDefault);
            UIMenuListItem defaultVeh = new("Default vehicle", new List<dynamic>() { "Prototipo" }, 0); // TODO: HANDLE BASED ON GRID AND ALLOWED CLASSES/VEHICLES
            detailsMenu.AddItem(defaultVeh);
            UIMenuListItem time = new("Time", new List<dynamic>() { "Mattino", "Pomeriggio", "Notte" }, 0); // TODO: WORK ON TIME AND ADD SUNRISE/SUNDOWN
            detailsMenu.AddItem(time);
            UIMenuListItem weather = new("Weather", new List<dynamic>() { "Sunny", "Rainy", "Smog", "Clear", "Cloudy", "Covered", "Storm", "Fog" }, 0); // TODO: WORK ON WEATHER
            detailsMenu.AddItem(weather);
            UIMenuListItem traffic = new("Traffic", new List<dynamic>() { "Off", "Low", "Everage", "High" }, 0, "For mid-air races, set this OFF"); // cambiare il traffico in base alla necesità
            detailsMenu.AddItem(traffic);

            #region dettagli item process
            detailsMenu.OnItemSelect += async (a, b, c) =>
            {
                if (b == title)
                {
                    string title = await HUD.GetUserInput("Insert Title (Max 25 chars)", "", 25);
                    data.Title = title;
                    b.SetRightLabel(title.Length > 15 ? title.Substring(0, 13) + "..." : title);
                }
                else if (b == description)
                {
                    string desc = await HUD.GetUserInput("Insert description (Max 70 chars)", "", 70);
                    data.Description = desc;
                    b.SetRightLabel(desc.Length > 15 ? desc.Substring(0, 13) + "..." : desc);
                }
                if (!(string.IsNullOrWhiteSpace(data.Title) && string.IsNullOrWhiteSpace(data.Description)))
                {
                    // TODO: Unlock everything
                    title.SetRightBadge(BadgeIcon.NONE);
                    description.SetRightBadge(BadgeIcon.NONE);
                }
            };

            detailsMenu.OnListChange += async (a, b, c) =>
            {
                if (b == raceType)
                {
                    data.RaceType = c;
                }
                else if (b == lapType)
                {
                    data.LapType = c;
                }
                else if (b == lapsNumber)
                {
                    data.Laps = c;
                }
                else if (b == playersNumber)
                {
                    data.MaxPlayers = c;
                    gridVehStart.Clear();
                    gridVehStart.Add("2 X " + data.MaxPlayers);
                }
                else if (b == time)
                {
                    data.Time = c;
                    PauseClock(true);
                    switch (c)
                    {
                        case 0:
                            NetworkOverrideClockTime(7, 0, 0);
                            break;
                        case 1:
                            NetworkOverrideClockTime(17, 0, 0);
                            break;
                        case 2:
                            NetworkOverrideClockTime(23, 59, 59);
                            break;
                    }
                }
                else if (b == weather)
                {
                    data.Weather = c;
                    switch (c)
                    {
                        case 0:
                            World.TransitionToWeather(Weather.ExtraSunny, 1f);
                            break;
                        case 1:
                            World.TransitionToWeather(Weather.Raining, 1f);
                            break;
                        case 2:
                            World.TransitionToWeather(Weather.Smog, 1f);
                            break;
                        case 3:
                            World.TransitionToWeather(Weather.Clear, 1f);
                            break;
                        case 4:
                            World.TransitionToWeather(Weather.Clouds, 1f);
                            break;
                        case 5:
                            World.TransitionToWeather(Weather.Overcast, 1f);
                            break;
                        case 6:
                            World.TransitionToWeather(Weather.ThunderStorm, 1f);
                            break;
                        case 7:
                            World.TransitionToWeather(Weather.Foggy, 1f);
                            break;
                    }
                }
                else if (b == traffic)
                {
                    data.Trafffic = c;
                    switch (c)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                }
                else if (b == classDefault)
                {
                    List<VehicleHash> veicoli = Enum.GetValues(typeof(VehicleHash)).Cast<VehicleHash>().ToList();
                    VehicleClass classe;
                    bool success = Enum.TryParse(b.Items[c].ToString(), out classe);
                    List<VehicleHash> disp = veicoli.Where(x => GetVehicleClassFromName((uint)x) == (int)classe).Except(data.ExcludedVehicles).ToList();
                    defaultVeh.Items.Clear();
                    foreach (VehicleHash d in disp)
                        defaultVeh.Items.Add(d.ToString());
                    VehicleClass pippo;
                    VehicleHash poppo;
                    Enum.TryParse((string)classDefault.Items.Last(), out pippo);
                    Enum.TryParse((string)defaultVeh.Items.First(), out poppo);
                    data.DefaultClass = pippo;
                    data.DefaultVehicle = poppo;
                }
                else if (b == defaultVeh)
                {
                    VehicleHash classe;
                    bool success = Enum.TryParse(b.Items[c].ToString(), out classe);
                    data.DefaultVehicle = classe;
                }
            };
            #endregion

            #region ClassVehs Process
            availableVehsMenu.OnCheckboxChange += (a, b, c) =>
            {
                try
                {
                    VehicleClass classe;
                    bool success = Enum.TryParse(b.Label, out classe);
                    if (success)
                    {
                        if (c)
                            data.AllowedClasses.Add(classe);
                        else
                            data.AllowedClasses.Remove(classe);
                        classDefault.Items.Clear();
                        List<VehicleHash> veicoli = Enum.GetValues(typeof(VehicleHash)).Cast<VehicleHash>().ToList();
                        foreach (VehicleClass cla in data.AllowedClasses)
                            classDefault.Items.Add(cla.ToString());
                        List<VehicleHash> disp = veicoli.Where(x => GetVehicleClassFromName((uint)x) == (int)classe).Except(data.ExcludedVehicles).ToList();
                        defaultVeh.Items.Clear();
                        foreach (VehicleHash d in disp)
                            defaultVeh.Items.Add(d.ToString());
                        VehicleClass pippo;
                        VehicleHash poppo;
                        Enum.TryParse((string)classDefault.Items.Last(), out pippo);
                        Enum.TryParse((string)defaultVeh.Items.First(), out poppo);
                        data.DefaultClass = pippo;
                        data.DefaultVehicle = poppo;
                    }
                }
                catch (Exception e)
                {
                    Client.Logger.Error(e.ToString());
                }
            };

            #endregion

            #endregion

            #region Positioning

            UIMenuItem checkpointGridItem = new("Checkpoints");
            UIMenu checkpointsGridMenu = new("Checkpoints", "The Last Galaxy");
            UIMenuItem propPlacingItem = new("Track placing");
            propPlacing = new("Track placing", "The Last Galaxy");

            checkpointGridItem.Activated += async (a, b) => await positioningMenu.SwitchTo(checkpointsGridMenu, 0, true);
            propPlacingItem.Activated += async (a, b) => await positioningMenu.SwitchTo(propPlacing, 0, true);

            positioningMenu.AddItem(checkpointGridItem);
            positioningMenu.AddItem(propPlacingItem);

            #region CHECKPOINTS AND GRID
            UIMenuItem startingGridItem = new("Start Grid");
            UIMenuItem checkPointsItem = new("Checkpoint positioning");

            UIMenu startingGridMenu = new("Griglia di partenza", "The Last Galaxy");
            UIMenu checkPointsMenu = new("Posiziona Checkpoint", "The Last Galaxy");

            startingGridItem.Activated += async (a, b) => await checkpointsGridMenu.SwitchTo(checkpointsGridMenu, 0, true);
            checkPointsItem.Activated += async (a, b) => await checkpointsGridMenu.SwitchTo(propPlacing, 0, true);

            checkpointsGridMenu.AddItem(startingGridItem);
            checkpointsGridMenu.AddItem(checkPointsItem);

            #region START GRID

            UIMenuListItem gridSize = new UIMenuListItem("Grid size", gridVehStart, 0);
            UIMenuListItem gridType = new UIMenuListItem("Grid type", new List<dynamic>() { "Big", "Medium", "Small" }, 0);
            startingGridMenu.AddItem(gridSize);
            startingGridMenu.AddItem(gridType);

            #endregion

            #region CHECKPOINTS
            UIMenuItem placeCheck = new UIMenuItem("Place checkpoint");
            UIMenuListItem tipoCheck = new UIMenuListItem("Type", new List<dynamic>() { "Primary", "Secondary" }, 0);
            UIMenuListItem stileCheck = new UIMenuListItem("Style", new List<dynamic>() { "Classic", "Circular" }, 0);
            checkPointsMenu.AddItem(placeCheck);
            checkPointsMenu.AddItem(tipoCheck);
            checkPointsMenu.AddItem(stileCheck);
            #endregion

            #endregion

            #region PROPS AND POSITIONING
            UIMenuDynamicListItem type = new("Type", RaceCreatorHelper.GetPropName(-248283675), async (sender, direction) =>
            {
                if (direction == UIMenuDynamicListItem.ChangeDirection.Right)
                {
                    propChoiceType += 1;
                    if (propChoiceType > RaceCreatorHelper.GetFinalInCategory(choosenCategory))
                        propChoiceType = 0;
                }
                else if (direction == UIMenuDynamicListItem.ChangeDirection.Left)
                {
                    propChoiceType -= 1;
                    if (propChoiceType < 0)
                        propChoiceType = RaceCreatorHelper.GetFinalInCategory(choosenCategory);
                }
                int m = RaceCreatorHelper.GetModel(choosenCategory, propChoiceType);
                if (m == 0)
                {
                    propChoiceType = 0;
                    m = RaceCreatorHelper.GetModel(choosenCategory, propChoiceType);
                }

                Vector3 pos = Vector3.Zero;
                if (DummyProp != null)
                    pos = DummyProp.Position;
                else
                    pos = cross.Position;
                if (DummyProp != null)
                    DummyProp.Delete();
                DummyProp = await Functions.SpawnLocalProp(m, pos, false, false);
                DummyProp.Heading = cross.Heading;
                DummyProp.IsCollisionEnabled = false;
                dummyRot = new(0, 0, dummyRot.Z);
                DummyProp.Rotation = dummyRot;
                SetObjectTextureVariation(DummyProp.Handle, propColorChoosen);
                AttachEntityToEntity(cross.Handle, DummyProp.Handle, 0, 0, 0, 1f, 0, 0, 0, false, false, false, false, 0, false);
                return RaceCreatorHelper.GetPropName(m);
            });
            UIMenuDynamicListItem color = new UIMenuDynamicListItem("Color", "", async (sender, direction) =>
            {
                if (direction == UIMenuDynamicListItem.ChangeDirection.Right)
                {
                    propColorChoosen++;
                    if (DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_straight_bar_s_s") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_straight_bar_s") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_bend_bar_l_out") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_bend_bar_l_b") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_bend_bar_m_out") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_bend_bar_m_in") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_straight_bar_m") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_straight_bar_s_s") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_straight_bar_s") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_bend_bar_m_out") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_bend_bar_m_in") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_bend_bar_l_out") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_bend_bar_l_b") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_straight_bar_m"))
                        if (propColorChoosen == 5 || propColorChoosen == 9)
                            propColorChoosen++;
                    if (propColorChoosen > RaceCreatorHelper.GetColorCount(DummyProp.Model.Hash))
                        propColorChoosen = 0;
                }
                else if (direction == UIMenuDynamicListItem.ChangeDirection.Left)
                {
                    propColorChoosen--;
                    if (DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_straight_bar_s_s") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_straight_bar_s") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_bend_bar_l_out") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_bend_bar_l_b") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_bend_bar_m_out") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_bend_bar_m_in") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_straight_bar_m") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_straight_bar_s_s") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_straight_bar_s") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_bend_bar_m_out") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_bend_bar_m_in") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_bend_bar_l_out") || DummyProp.Model.Hash == Functions.HashInt("sum_prop_track_ac_bend_bar_l_b") || DummyProp.Model.Hash == Functions.HashInt("ch_prop_track_ch_straight_bar_m"))
                        if (propColorChoosen == 5 || propColorChoosen == 9)
                            propColorChoosen--;
                    if (propColorChoosen < 0)
                        propColorChoosen = RaceCreatorHelper.GetColorCount(DummyProp.Model.Hash);
                }

                SetObjectTextureVariation(DummyProp.Handle, propColorChoosen);
                return Game.GetGXTEntry(RaceCreatorHelper.GetPropColor(DummyProp.Model.Hash, propColorChoosen));
            });
            UIMenuDynamicListItem category = new UIMenuDynamicListItem("Category", RaceCreatorHelper.GetCategoryName(choosenCategory), async (sender, direction) =>
            {
                if (direction == UIMenuDynamicListItem.ChangeDirection.Right)
                {
                    choosenCategory += 1;
                    if (choosenCategory > 47)
                        choosenCategory = 0;
                }
                else if (direction == UIMenuDynamicListItem.ChangeDirection.Left)
                {
                    choosenCategory -= 1;
                    if (choosenCategory < 0)
                        choosenCategory = 47;
                }
                propChoiceType = RaceCreatorHelper.GetFinalInCategory(choosenCategory);
                string pp = await type.Callback(type, UIMenuDynamicListItem.ChangeDirection.Right);
                propColorChoosen = RaceCreatorHelper.GetColorCount(DummyProp.Model.Hash);
                string col = await color.Callback(color, UIMenuDynamicListItem.ChangeDirection.Right);
                //TODO: DECOMMENTARE will be fixed in next release
                //tipo.CurrentListItem = pp;
                //color.CurrentListItem = col;
                return RaceCreatorHelper.GetCategoryName(choosenCategory);
            });
            propPlacing.AddItem(category);
            propPlacing.AddItem(type);
            UIMenuListItem typeRot = new UIMenuListItem("Rotation type", new List<dynamic>() { GetLabelText("FMMC_PROT_NORM"), "Roll", "Pitch", "Yaw" }, 0);
            propPlacing.AddItem(typeRot);
            propPlacing.AddItem(color);
            UIMenuListItem stacking = new("Enable prop stacking", new List<dynamic>() { Game.GetGXTEntry("FE_HLP29"), Game.GetGXTEntry("FE_HLP31") }, 0);
            propPlacing.AddItem(stacking);
            UIMenuListItem snapAtt = new(Game.GetGXTEntry("FMMC_PRP_SNP"), new List<dynamic>() { Game.GetGXTEntry("FE_HLP31"), Game.GetGXTEntry("FE_HLP29") }, 0);
            propPlacing.AddItem(snapAtt);
            propPlacing.OnListChange += (a, b, c) =>
            {
                if (b == typeRot)
                {
                    rotationDummyType = (RotationDummyType)c;
                }
                else if (b == stacking)
                {
                    RaceCreator.stacking = c == 0;
                }
                else if (b == snapAtt)
                {
                    RaceCreator.snapOptions.Enabled = c != 0;
                    RaceCreator.snapOptions.Proximity = c != 0;
                    if (!RaceCreator.snapOptions.Enabled)
                    {
                        DummyProp.Detach();
                        attachedBone = 0;
                    }
                }
            };


            UIMenuItem advanceOptItem = new UIMenuItem("Advanced Options");
            UIMenu advanceOptMenu = new("Advanced Options", "The Last Galaxy");
            advanceOptItem.Activated += async (a, b) => await propPlacing.SwitchTo(advanceOptMenu, 0, true);
            propPlacing.AddItem(advanceOptItem);

            #region Advanced Options

            UIMenuItem overridePosItem = new("Override Position", "Use a Free Camera for values X, Y, Z to ~y~position~w~ components");
            UIMenu overridePos = new("Override Position", "The Last Galaxy");
            UIMenuItem overrideRotItem = new("Override Rotation", "Use a Free Camera for values X, Y, Z to ~y~rotate~w~ components");
            UIMenu overrideRot = new("Override Rotation", "The Last Galaxy");
            UIMenuItem snapOptionsItem = new(Game.GetGXTEntry("FMMC_PRP_SNPO"));
            UIMenu snapOptions = new(Game.GetGXTEntry("FMMC_PRP_SNPO"), "The Last Galaxy");

            overridePosItem.Activated += async (a, b) => await advanceOptMenu.SwitchTo(overridePos, 0, true);
            overrideRotItem.Activated += async (a, b) => await advanceOptMenu.SwitchTo(overrideRot, 0, true);
            snapOptionsItem.Activated += async (a, b) => await advanceOptMenu.SwitchTo(snapOptions, 0, true);


            advanceOptMenu.AddItem(overridePosItem);
            advanceOptMenu.AddItem(overrideRotItem);
            advanceOptMenu.AddItem(snapOptionsItem);


            #region override pos and rot
            UIMenuCheckboxItem useOverride = new UIMenuCheckboxItem("Use Override", UIMenuCheckboxStyle.Tick, false, "");
            UIMenuListItem alignment = new UIMenuListItem("Alignment", new List<dynamic>() { "World", "Local" }, 0);
            UIMenuDynamicListItem posX = new UIMenuDynamicListItem("X", curLocation.X.ToString("F3"), async (sender, direction) =>
            {
                if (direction == UIMenuDynamicListItem.ChangeDirection.Left) curLocation.X -= 0.1f;
                else curLocation.X += 0.1f;
                return curLocation.X.ToString("F3");
            });
            UIMenuDynamicListItem posY = new UIMenuDynamicListItem("Y", curLocation.Y.ToString("F3"), async (sender, direction) =>
            {
                if (direction == UIMenuDynamicListItem.ChangeDirection.Left) curLocation.Y -= 0.1f;
                else curLocation.Y += 0.1f;
                return curLocation.Y.ToString("F3");
            });
            UIMenuDynamicListItem posZ = new UIMenuDynamicListItem("Z", curLocation.Z.ToString("F3"), async (sender, direction) =>
            {
                if (direction == UIMenuDynamicListItem.ChangeDirection.Left) curLocation.Z -= 0.1f;
                else curLocation.Z += 0.1f;
                return curLocation.Z.ToString("F3");
            });
            UIMenuDynamicListItem rotX = new UIMenuDynamicListItem("X", dummyRot.X.ToString("F3"), async (sender, direction) =>
            {
                if (direction == UIMenuDynamicListItem.ChangeDirection.Left) dummyRot.X -= 0.1f;
                else dummyRot.X += 0.1f;
                return dummyRot.X.ToString("F3");
            });
            UIMenuDynamicListItem rotY = new UIMenuDynamicListItem("Y", dummyRot.Y.ToString("F3"), async (sender, direction) =>
            {
                if (direction == UIMenuDynamicListItem.ChangeDirection.Left) dummyRot.Y -= 0.1f;
                else dummyRot.Y += 0.1f;
                return dummyRot.Y.ToString("F3");
            });
            UIMenuDynamicListItem rotZ = new UIMenuDynamicListItem("Z", dummyRot.Z.ToString("F3"), async (sender, direction) =>
            {
                if (direction == UIMenuDynamicListItem.ChangeDirection.Left) dummyRot.Z -= 0.1f;
                else dummyRot.Z += 0.1f;
                return dummyRot.Z.ToString("F3");
            });
            overridePos.AddItem(alignment);
            overridePos.AddItem(posX);
            overridePos.AddItem(posY);
            overridePos.AddItem(posZ);
            overrideRot.AddItem(alignment);
            overrideRot.AddItem(rotX);
            overrideRot.AddItem(rotY);
            overrideRot.AddItem(rotZ);
            #endregion

            #region SnapOptions

            UIMenuListItem proxSnap = new UIMenuListItem("", new List<dynamic>() { Game.GetGXTEntry("FE_HLP31"), Game.GetGXTEntry("FE_HLP29") }, 0);
            UIMenuListItem chainSnap = new UIMenuListItem("", new List<dynamic>() { Game.GetGXTEntry("FE_HLP31"), Game.GetGXTEntry("FE_HLP29") }, 0);
            snapOptions.AddItem(proxSnap);
            snapOptions.AddItem(chainSnap);

            #endregion

            #endregion

            UIMenuListItem speedPadIntensity = new UIMenuListItem("Speed Pad Intensity", new List<dynamic>() { "Weak", "Normal", "Strong", "Extra Strong", "Ultra Strong" }, 1);
            propPlacing.AddItem(speedPadIntensity);
            UIMenuListItem slowPadIntensity = new UIMenuListItem("Slow Pad Intensity", new List<dynamic>() { "Weak", "Normal", "Strong", "Extra Strong", "Ultra Strong" }, 1);
            propPlacing.AddItem(slowPadIntensity);

            UIMenuItem soundTriggerMenuItem = new UIMenuItem("Sound enabling");
            UIMenu soundTriggerMenu = new("Sound enabling", "The Last Galaxy");

            soundTriggerMenuItem.Activated += async (a, b) => await propPlacing.SwitchTo(soundTriggerMenu, 0, true);
            propPlacing.AddItem(soundTriggerMenuItem);

            #region soundTriggerMenu

            UIMenuListItem soundId = new UIMenuListItem("Sound ID", new List<dynamic>() { "Airhorn", "Roar", "Chitarra 01", "Chitarra 02", "Clacson", "Tuono", "Allarme" }, 0);
            UIMenuItem soundPreview = new UIMenuItem("Play Sound Preview");
            UIMenuListItem radius = new UIMenuListItem("Enabling distance", new List<dynamic> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 150 }, 0);
            UIMenuListItem timesPerLap = new UIMenuListItem("(Global) Times per lap", new List<dynamic>() { "Infinite", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70 }, 0);
            UIMenuCheckboxItem oncePerLap = new UIMenuCheckboxItem("Once per Lap", false);
            soundTriggerMenu.AddItem(soundId);
            soundTriggerMenu.AddItem(soundPreview);
            soundTriggerMenu.AddItem(radius);
            soundTriggerMenu.AddItem(timesPerLap);
            soundTriggerMenu.AddItem(oncePerLap);

            #endregion


            #endregion
            #endregion

            UIMenuItem exit = new(Game.GetGXTEntry("R2P_MENU_EXI"));
            Creator.AddItem(exit);
            Creator.Visible = true;

            propPlacing.OnMenuClose += (a) =>
            {
                if (DummyProp.Exists())
                {
                    DummyProp.Delete();
                    DummyProp = null;
                }
            };
            /*
            MenuHandler.OnMenuStateChanged += async (a, b, c) =>
            {
                if (c == MenuState.ChangeForward)
                {
                    if (b == propPlacing)
                    {
                        //var aa = await categoria.Callback(categoria, UIMenuDynamicListItem.ChangeDirection.None);
                    }
                }
            };
            */
        }

        #region	METHODS

        public static void changeModel(ObjectHash iVar11)
        {
            cross.Delete();
            cross = null;
            ObjectHash model = ObjectHash.prop_mp_placement_sm;

            switch (iVar11)
            {
                case ObjectHash.prop_mp_placement_sm:
                    model = ObjectHash.prop_mp_placement_sm;

                    break;
                case ObjectHash.prop_mp_placement_lrg:
                    model = ObjectHash.prop_mp_placement_lrg;

                    break;
                case ObjectHash.prop_mp_cant_place_sm:
                    model = ObjectHash.prop_mp_cant_place_sm;

                    break;
                case ObjectHash.prop_mp_cant_place_lrg:
                    model = ObjectHash.prop_mp_cant_place_lrg;

                    break;
                case ObjectHash.prop_mp_max_out_sm:
                    model = ObjectHash.prop_mp_max_out_sm;

                    break;
                case ObjectHash.prop_mp_max_out_lrg:
                    model = ObjectHash.prop_mp_max_out_lrg;

                    break;
            }

            if (cross == null)
            {
                cross = new Prop(CreateObjectNoOffset((uint)model, WorldProbe.CrossairRenderingRaycastResult.HitPosition.X, WorldProbe.CrossairRenderingRaycastResult.HitPosition.Y, WorldProbe.CrossairRenderingRaycastResult.HitPosition.Z, false, false, false))
                {
                    IsVisible = true,
                    LodDistance = 500,
                    IsCollisionEnabled = false,
                };
                SetEntityLoadCollisionFlag(cross.Handle, true);
            }
        }

        private static bool func_9339(float iParam0, float fParam1)
        {
            if (IsInputDisabled(2))
                return true;
            if (iParam0 < -fParam1)
                return true;
            else if (iParam0 > fParam1)
                return true;
            return false;
        }

        private static int func_7449()
        {
            if (IsInputDisabled(2))
                return 251;
            return 210;
        }

        private static int func_7450()
        {
            if (IsInputDisabled(2))
                return 250;
            return 209;
        }

        private static int func_9341()
        {
            if (IsInputDisabled(2))
                return 204;
            return 217;
        }

        private static int func_9387()
        {
            if (IsInputDisabled(2))
                return 253;
            return 208;
        }

        private static int func_7660()
        {
            if (IsInputDisabled(2))
                return 252;
            return 207;
        }

        private static float func_909(float fParam0, float fParam1, float fParam2)
        {
            if (fParam0 > fParam2)
            {
                return fParam2;
            }
            else if (fParam0 < fParam1)
            {
                return fParam1;
            }
            return fParam0;
        }

        private static bool func_277(int iParam0, int iParam1)
        {
            int iVar0;
            bool bVar1;
            int iVar2;

            for (iVar0 = 0; iVar0 < 41; iVar0++)
            {
                iVar2 = RaceCreatorHelper.GetModel(iParam1, iVar0);
                if (iVar2 != 0)
                {
                    if (iVar2 == iParam0)
                        return true;
                }
            }
            return false;
        }


        private static int func_7497(Vector3 vParam0, float fParam1, int iParam2, Vector3 vParam3, float fParam4)
        {
            Vector3 vVar0 = Vector3.Zero;
            Vector3 vVar1 = Vector3.Zero;
            Vector3 vVar2 = Vector3.Zero;
            Vector3 vVar3 = Vector3.Zero;

            GetModelDimensions((uint)iParam2, ref vVar0, ref vVar1);
            vVar2 = vVar1 - vVar0;
            vVar3.X = (vVar2.X / 2f) - vVar1.X;
            vVar3.Y = (vVar2.Y / 2f) - vVar1.Y;
            if (func_7498(vParam3, fParam4, vParam0.X + vVar3.X, vVar2, new Vector3(fParam1), Vector3.Zero))
                return 1;
            return 0;
        }

        private static bool func_7498(Vector3 Param0, float uParam1, float fParam2, Vector3 Param3, Vector3 vParam4, Vector3 Param5, float uParam6 = 0, float fParam7 = 0)
        {
            float fVar0;
            float fVar1;
            float fVar2;
            float fVar3;
            float fVar4;
            float fVar5;
            float fVar6;
            float fVar7;
            float fVar8;
            float fVar9;
            float fVar10;

            fVar0 = (Cos(fParam7) * (Param0.X - Param3.X)) - (Sin(fParam7) * (Param0.Y - Param3.Y)) + Param3.X;
            fVar1 = (Sin(fParam7) * (Param0.X - Param3.X)) + (Cos(fParam7) * (Param0.Y - Param3.Y)) + Param3.Y;
            fVar2 = Param3.X - (Param5.X / 2f);
            fVar3 = Param3.Y - (Param5.Y / 2f);
            fVar4 = Param3.X + (Param5.X / 2f);
            fVar5 = Param3.Y + (Param5.Y / 2f);
            fVar6 = func_909(fVar0, fVar2, fVar4);
            fVar7 = func_909(fVar1, fVar3, fVar5);
            fVar8 = fVar0 - fVar6;
            fVar9 = fVar1 - fVar7;
            fVar10 = (fVar8 * fVar8) + (fVar9 * fVar9);
            return fVar10 < (fParam2 * fParam2);
        }
        #endregion

        #region Ticks

        private static async Task MoveCamera()
        {
            #region	CAMERA MOVEMENTS
            if (enteringCamera.Exists())
            {
                DisableInputGroup(2);
                float fVar0 = GetDisabledControlNormal(2, 218);
                float fVar1 = GetDisabledControlNormal(2, 219);
                float fVar2 = GetDisabledControlNormal(2, 220);
                float fVar3 = GetDisabledControlNormal(2, 221);
                float ltNorm = GetDisabledControlNormal(2, 252);
                float rtNorm = GetDisabledControlNormal(2, 253);
                float zoomingWheel = 0f;
                if (!IsLookInverted())
                {
                    fVar1 = -fVar1;
                    fVar3 = -fVar3;
                }
                if (!IsInputDisabled(2))
                {
                    N_0xc8b5c4a79cc18b94(enteringCamera.Handle);
                }
                else if (Input.IsDisabledControlPressed(Control.CreatorLT) || Input.IsDisabledControlPressed(Control.CreatorRT))
                {
                    N_0xc8b5c4a79cc18b94(enteringCamera.Handle);
                }
                if (IsInputDisabled(2))
                {
                    fVar2 = GetDisabledControlUnboundNormal(2, 1) * 2;
                    fVar3 = GetDisabledControlUnboundNormal(2, 2) * -1 * 2;
                    if (GetDisabledControlNormal(2, 241) > 0.25f)
                        zoomingWheel = 3f;
                    if (GetDisabledControlNormal(2, 242) > 0.25f)
                        zoomingWheel = -3f;
                }

                float xVectFwd = -fVar1 * (float)Math.Sin(Functions.Deg2rad(curRotation.Z));
                float yVectFwd = fVar1 * (float)Math.Cos(Functions.Deg2rad(curRotation.Z));
                float xVectLat = fVar0 * (float)Math.Cos(Functions.Deg2rad(curRotation.Z));
                float yVectLat = fVar0 * (float)Math.Sin(Functions.Deg2rad(curRotation.Z));


                cameraPosition.X += xVectFwd + xVectLat;
                cameraPosition.Y += yVectFwd + yVectLat;
                curRotation = new(fVar3 + enteringCamera.Rotation.X, 0, -fVar2 + enteringCamera.Rotation.Z);

                zoom = Vector3.Distance(cameraPosition, curLocation); // da migliorare assolutamente

                curLocation = new((cameraPosition + zoom * enteringCamera.CamForwardVector()).X, (cameraPosition + zoom * enteringCamera.CamForwardVector()).Y, Height);

                if (ltNorm > 0 || rtNorm > 0 || zoomingWheel != 0)
                {
                    zoom += ltNorm;
                    zoom -= rtNorm;
                    zoom += zoomingWheel;
                    if (zoom >= 350f)
                        zoom = 350f;
                    if (zoom <= 16f)
                        zoom = 16f;
                    cameraPosition = curLocation - zoom * enteringCamera.CamForwardVector();
                }

                if (IsDisabledControlPressed(2, func_7450()))
                {
                    cameraPosition.Z += 1f;
                    Height += 1f;
                }
                if (IsDisabledControlPressed(2, func_7449()))
                {
                    cameraPosition.Z -= 1f;
                    Height -= 1f;
                }

                float z = 0;
                GetGroundZFor_3dCoord(curLocation.X, curLocation.Y, Height + 300, ref z, false);
                if (Height <= z + 0.3f)
                    Height = z + 0.3f;
                cross.Position = curLocation;
                cross.Rotation = new(0, 0, curRotation.Z);
                placeMarker.Position = (cross.Position + new Vector3(0, 0, 0.1f)).ToPosition();
                placeMarker.Draw();

                if (curRotation.X >= -11.5f)
                    curRotation.X = -11.5f;
                if (curRotation.X <= -89.9f)
                    curRotation.X = -89.9f;

                enteringCamera.Position = cameraPosition;
                enteringCamera.Rotation = curRotation;
            }
            #endregion

            #region PROP MOVEMENTS

            if (DummyProp != null)
            {
                DummyProp.Position = curLocation;
                if (IsDisabledControlPressed(2, 227))
                {
                    if (rotationDummyType == RotationDummyType.Heading || rotationDummyType == RotationDummyType.Yaw)
                    {
                        dummyRot.Z -= 3f % 360f;
                        if (dummyRot.Z < 360f)
                            dummyRot.Z += 360f;
                    }
                    if (rotationDummyType == RotationDummyType.Pitch)
                    {
                        dummyRot.X -= 3f % 360f;
                        if (dummyRot.X < 360f)
                            dummyRot.X += 360f;
                    }
                    if (rotationDummyType == RotationDummyType.Roll)
                    {
                        dummyRot.Y -= 3f % 360f;
                        if (dummyRot.Y < 360f)
                            dummyRot.Y += 360f;
                    }
                }
                if (IsDisabledControlPressed(2, 226))
                {
                    if (rotationDummyType == RotationDummyType.Heading || rotationDummyType == RotationDummyType.Yaw)
                    {
                        dummyRot.Z += 3f % 360f;
                        if (dummyRot.Z > 360f)
                            dummyRot.Z -= 360f;
                    }
                    if (rotationDummyType == RotationDummyType.Pitch)
                    {
                        dummyRot.X += 3f % 360f;
                        if (dummyRot.X > 360f)
                            dummyRot.X -= 360f;
                    }
                    if (rotationDummyType == RotationDummyType.Roll)
                    {
                        dummyRot.Y += 3f % 360f;
                        if (dummyRot.Y > 360f)
                            dummyRot.Y -= 360f;
                    }
                }

                if (rotationDummyType != RotationDummyType.Heading)
                {
                    Vector3 vVar2 = Vector3.Zero;
                    Vector3 vVar3 = Vector3.Zero;
                    Vector3 vVar6 = new(-90f, 0f, 0f);
                    Vector3 vVar7 = new(1f, 1f, 1f);
                    float fVar8 = 1.25f;
                    float fVar9 = 0;
                    GetModelDimensions((uint)DummyProp.Model.Hash, ref vVar2, ref vVar3);
                    Vector3 vVar4 = vVar3 - vVar2;
                    Vector3 vVar10 = new(Math.Abs(vVar4.X), Math.Abs(vVar4.Y), Math.Abs(vVar4.Z));
                    if (vVar10.X > vVar10.Y && vVar10.X > vVar10.Z)
                    {
                        fVar9 = vVar10.X;
                    }
                    else if (vVar10.Y > vVar10.X && vVar10.Y > vVar10.Z)
                    {
                        fVar9 = vVar10.Y;
                    }
                    else if (vVar10.Z > vVar10.X && vVar10.Z > vVar10.Y)
                    {
                        fVar9 = vVar10.Z;
                    }
                    if (fVar9 > 10f)
                    {
                        float fVar11 = fVar9 / 10f;
                        vVar7 *= new Vector3(fVar11);
                        fVar8 *= fVar11;
                    }

                    if (rotationDummyType == RotationDummyType.Pitch)
                    {
                        Vector3 vVar5 = GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 1f, 0f, 0f) - curLocation;
                        World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, vVar3.X + fVar8, 0f, 0f), vVar5, vVar6, vVar7, SColor.Purple.ToColor());
                        World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, vVar2.X - fVar8, 0f, 0f), -vVar5, vVar6, vVar7, SColor.Purple.ToColor());
                    }
                    if (rotationDummyType == RotationDummyType.Roll)
                    {
                        Vector3 vVar5 = GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, 1f, 0f) - curLocation;
                        World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, vVar3.Y + fVar8, 0f), vVar5, vVar6, vVar7, SColor.Purple.ToColor());
                        World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, vVar2.Y - fVar8, 0f), -vVar5, vVar6, vVar7, SColor.Purple.ToColor());
                    }
                    if (rotationDummyType == RotationDummyType.Yaw)
                    {
                        Vector3 vVar5 = GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, 0f, 1f) - curLocation;
                        World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, 0f, vVar3.Z + fVar8), vVar5, vVar6, vVar7, SColor.Purple.ToColor());
                        World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, 0f, vVar2.Z - fVar8), -vVar5, vVar6, vVar7, SColor.Purple.ToColor());
                    }
                }
                int timer = Game.GameTime;
                if (Input.IsControlPressed(Control.FrontendX))
                {
                    if (Game.GameTime - timer > 1000)
                        dummyRot = new Vector3(0, 0, dummyRot.Z);
                }
                DummyProp.Rotation = dummyRot;
                //TODO: FIND SOLUTION FOR Creator.Children!!!

                #region CreatingProp
                if (Input.IsControlJustPressed(Control.FrontendAccept))
                {
                    bool submenuselected = propPlacing.MenuItems.Any(x => x.Selected);

                    if (submenuselected) return;
                    // check i didn't select submenus
                    int model = RaceCreatorHelper.GetModel(choosenCategory, propChoiceType);
                    Prop prop = await Functions.SpawnLocalProp(model, curLocation, false, false);
                    prop.Rotation = dummyRot;
                    SetObjectTextureVariation(prop.Handle, propColorChoosen);
                    if (snapOptions.Enabled)
                    {
                        DummyProp.Detach();
                        attachedBone = 0;
                        Prop close = prop.GetClosestProp(new List<Entity> { prop, DummyProp, cross });
                        //ALWAYS BONE 2 O 3 ALWAYS!
                        Vector3 bone2 = GetWorldPositionOfEntityBone(close.Handle, 2);
                        Vector3 bone3 = GetWorldPositionOfEntityBone(close.Handle, 3);

                        if (Vector3.Distance(GetWorldPositionOfEntityBone(prop.Handle, 3), bone2) < 15f)
                        {
                            Vector3 vVar13 = close.Rotation;
                            prop.Rotation = new Vector3(0, 0, vVar13.Z);
                            prop.IsCollisionEnabled = false;
                            if (func_277(close.Model.Hash, 18))
                                AttachEntityBoneToEntityBonePhysically(prop.Handle, close.Handle, 3, 2, true, false);
                            else
                                AttachEntityBoneToEntityBone(prop.Handle, close.Handle, 3, 2, true, false);
                            Game.PlaySound("Creator_Snap", "DLC_Stunt_Race_Frontend_Sounds");
                        }
                        if (Vector3.Distance(GetWorldPositionOfEntityBone(prop.Handle, 2), bone3) < 15f)
                        {
                            Vector3 vVar13 = close.Rotation;
                            prop.Rotation = new Vector3(0, 0, vVar13.Z);
                            prop.IsCollisionEnabled = false;
                            if (func_277(close.Model.Hash, 18))
                                AttachEntityBoneToEntityBonePhysically(prop.Handle, close.Handle, 2, 3, true, false);
                            else
                                AttachEntityBoneToEntityBone(prop.Handle, close.Handle, 2, 3, true, false);
                        }
                    }
                    Piazzati.Add(new TrackPiece(prop, (RacingProps)(uint)model, curLocation, dummyRot, propColorChoosen));
                }
                #endregion

                #region SNAP
                // TODO: FINISH SNAP FEATURE
                if (snapOptions.Enabled && snapOptions.Proximity) // func_8363
                {
                    Prop close = DummyProp.GetClosestProp(new List<Entity> { DummyProp, cross });
                    //ALWAYS BONE 2 O 3 ALWAYS! (EXCEPT crossroads)
                    Vector3 bone2 = GetWorldPositionOfEntityBone(close.Handle, 2);
                    Vector3 bone3 = GetWorldPositionOfEntityBone(close.Handle, 3);
                    if (!IsEntityAttached(DummyProp.Handle))
                    {
                        if (attachedBone == 0)
                        {
                            if (Vector3.Distance(GetWorldPositionOfEntityBone(DummyProp.Handle, 3), bone2) < 15f)
                            {
                                Vector3 vVar13 = close.Rotation;
                                DummyProp.Rotation = new Vector3(0, 0, vVar13.Z);
                                DummyProp.IsCollisionEnabled = false;
                                if (func_277(close.Model.Hash, 18))
                                    AttachEntityBoneToEntityBonePhysically(DummyProp.Handle, close.Handle, 3, 2, true, false);
                                else
                                    AttachEntityBoneToEntityBone(DummyProp.Handle, close.Handle, 3, 2, true, false);
                                Game.PlaySound("Creator_Snap", "DLC_Stunt_Race_Frontend_Sounds");
                                attachedBone = 2;
                            }
                            else if (Vector3.Distance(GetWorldPositionOfEntityBone(DummyProp.Handle, 2), bone3) < 15f)
                            {
                                Vector3 vVar13 = close.Rotation;
                                DummyProp.Rotation = new Vector3(0, 0, vVar13.Z);
                                DummyProp.IsCollisionEnabled = false;
                                if (func_277(close.Model.Hash, 18))
                                    AttachEntityBoneToEntityBonePhysically(DummyProp.Handle, close.Handle, 2, 3, true, false);
                                else
                                    AttachEntityBoneToEntityBone(DummyProp.Handle, close.Handle, 2, 3, true, false);
                                Game.PlaySound("Creator_Snap", "DLC_Stunt_Race_Frontend_Sounds");
                                attachedBone = 3;
                            }
                        }
                    }
                    else
                    {
                        if (attachedBone == 2)
                        {
                            if (Vector3.Distance(curLocation, bone2) > 30f)
                            {
                                DummyProp.Detach();
                                attachedBone = 0;
                            }
                        }
                        else if (attachedBone == 3)
                        {
                            if (Vector3.Distance(curLocation, bone3) > 30f)
                            {
                                DummyProp.Detach();
                                attachedBone = 0;
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion
            // FOR SNAP LOOK FOR "Creator_Snap", "DLC_Stunt_Race_Frontend_Sounds"
        }
        #endregion
    }

    public class SnapOptions
    {
        public bool Enabled { get; set; }
        public bool Proximity { get; set; }
        public bool Chaining { get; set; }
    }
}
