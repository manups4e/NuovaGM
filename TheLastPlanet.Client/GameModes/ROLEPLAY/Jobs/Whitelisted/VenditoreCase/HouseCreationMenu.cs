using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.HouseDealer
{
    internal static class HouseCreationMenu
    {
        private static Camera MainCamera;
        private static int travelSpeed = 0;
        private static Position curLocation;
        private static Position curRotation;
        private static Position CameraPosEntrance;
        private static Position CameraRotEntrance;
        private static string travelSpeedStr = "Medium";
        private static int checkTimer = 0;
        private static MarkerEx markerEnterFoot;
        private static MarkerEx markerEnterGarage;
        private static MarkerEx markerEnterRoof;
        private static UIMenuItem markerEntranceHome;
        private static UIMenuItem markerEntranceGarage;
        private static UIMenuItem markerEntranceRoof;
        private static UIMenuItem posCamera;
        private static UIMenuColorPanel blipColor;
        private static Prop renderCamObject;
        private static MarkerEx dummyMarker = new(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.WhiteSmoke);
        private static int intern = 0;
        private static string abbreviation;

        private enum TypeProperty
        {
            House,
            Garage
        }

        public static async void MenuHousesCreation()
        {
            MainCamera = null;
            travelSpeed = 0;
            curLocation = Position.Zero;
            curRotation = Position.Zero;
            CameraPosEntrance = Position.Zero;
            CameraRotEntrance = Position.Zero;
            travelSpeedStr = "Medium";
            checkTimer = 0;
            markerEnterFoot = null;
            markerEnterGarage = null;
            markerEnterRoof = null;
            markerEntranceHome = null;
            markerEntranceGarage = null;
            markerEntranceRoof = null;
            posCamera = null;
            blipColor = null;
            renderCamObject = null;
            dummyMarker = new MarkerEx(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.WhiteSmoke);
            intern = 0;
            abbreviation = "";
            InstanceBags oldInstance = new();
            ConfigHouses homeDummy = new();
            homeDummy.VehCapacity = 2;
            Garages garageDummy = new();
            bool includeGarage = false;
            bool includeRoof = false;
            TypeProperty property = TypeProperty.House;

            #region declaration

            UIMenu creation = new("Real Estate Creator", "Use with Caution!", new PointF(1450f, 0), "thelastgalaxy", "bannerbackground", false, true);
            creation.MouseControlsEnabled = false;
            UIMenuListItem type = new("Property type", new List<dynamic>() { "House", "Garage" }, 0);
            creation.AddItem(type);
            type.OnListChanged += (item, index) => property = (TypeProperty)index;
            UIMenuItem propertyDataItem = new("1. Property data"); // NB: temporary Name
            UIMenu propertyData = new("1. Property data", "");
            propertyData.MouseControlsEnabled = false;
            UIMenuItem selectPointItem = new("2. Exteriors management"); // NB: temporary Name
            UIMenu selectPoint = new("2. Exteriors management", "");
            selectPoint.MouseControlsEnabled = false;
            UIMenuItem interiorHandlingHomeItem = new("3. Interiors management"); // NB: temporary Name
            UIMenu interiorHandlingHome = new("3. Interiors management", "");
            interiorHandlingHome.MouseControlsEnabled = false;
            UIMenu interiorOptions = null;

            propertyDataItem.Activated += async (a, b) => await creation.SwitchTo(propertyData, 0, true);
            selectPointItem.Activated += async (a, b) => await creation.SwitchTo(selectPoint, 0, true);
            interiorHandlingHomeItem.Activated += async (a, b) => await creation.SwitchTo(interiorHandlingHome, 0, true);
            creation.AddItem(propertyDataItem);
            creation.AddItem(selectPointItem);
            creation.AddItem(interiorHandlingHomeItem);
            #endregion

            #region Property Data

            propertyData.OnMenuOpen += async (a, b) =>
            {
                propertyData.Clear();
                UIMenuItem propertyName = new("Name of the property", "It would be preferable to include the street and house number in the name, but you can decide the name however you want!");
                UIMenuItem abbreviatedName = new("Abbreviated Name", "For saving purposes, an abbreviated name is needed to index the property in the player's catabase.. for example: 0232 Paleto Boulevard => 0232PB");

                switch (property)
                {
                    case TypeProperty.House:
                        {
                            if (homeDummy.Label != null) abbreviatedName.SetRightLabel(abbreviation);

                            break;
                        }
                    case TypeProperty.Garage:
                        {
                            if (garageDummy.Label != null)
                            {
                                propertyName.SetRightLabel(garageDummy.Label.Length > 15 ? garageDummy.Label.Substring(0, 15) + "..." : garageDummy.Label);
                                abbreviatedName.SetRightLabel(abbreviation);
                            }

                            break;
                        }
                }

                propertyData.AddItem(propertyName);
                propertyData.AddItem(abbreviatedName);

                if (property == TypeProperty.House)
                {
                    UIMenuCheckboxItem garageIncluded = new("Include Garage", UIMenuCheckboxStyle.Tick, includeGarage, "If activated the property will have a garage included");
                    UIMenuCheckboxItem tettoIncluded = new("Include Roof", UIMenuCheckboxStyle.Tick, includeRoof, "If activated the property will have a reachable roof");
                    propertyData.AddItem(garageIncluded);
                    propertyData.AddItem(tettoIncluded);
                    garageIncluded.CheckboxEvent += (item, _checked) =>
                    {
                        includeGarage = _checked;
                        homeDummy.GarageIncluded = _checked;
                    };
                    tettoIncluded.CheckboxEvent += (item, _checked) =>
                    {
                        includeRoof = _checked;
                        homeDummy.HasRoof = _checked;
                    };
                }

                UIMenuItem price = new("Selling price", "Enter a base selling price, so everyone has an idea of how much it costs to buy it");

                switch (property)
                {
                    case TypeProperty.House:
                        price.SetRightLabel("$" + homeDummy.Price);

                        break;
                    case TypeProperty.Garage:
                        price.SetRightLabel("$" + garageDummy.Price);

                        break;
                }

                propertyData.AddItem(price);
                propertyData.OnItemSelect += async (menu, item, index) =>
                {
                    if (item == propertyName)
                    {
                        string valore = await HUD.GetUserInput("Property Name", "", 30);
                        item.SetRightLabel(valore.Length > 15 ? valore.Substring(0, 15) + "..." : valore);

                        switch (property)
                        {
                            case TypeProperty.House:
                                homeDummy.Label = valore;

                                break;
                            case TypeProperty.Garage:
                                garageDummy.Label = valore;

                                break;
                        }
                    }
                    else if (item == abbreviatedName)
                    {
                        string valore = await HUD.GetUserInput("Abbreviated Name", "", 7);
                        item.SetRightLabel(valore);
                        abbreviation = valore;
                    }
                    else if (item == price)
                    {
                        string valore = await HUD.GetUserInput("Price", "", 10);
                        item.SetRightLabel("$" + valore);

                        switch (property)
                        {
                            case TypeProperty.House:
                                homeDummy.Price = Convert.ToInt32(valore);

                                break;
                            case TypeProperty.Garage:
                                garageDummy.Price = Convert.ToInt32(valore);

                                break;
                        }
                    }
                };
            };

            #endregion

            #region Exteriors

            /*
			#region blip
			UIMenu blip = selezionePunto.AddSubMenu("Posiziona Blip");
			blip.AddInstructionalButton(Velocità);
			blip.AddInstructionalButton(Scendi);
			blip.AddInstructionalButton(Sali);
			blip.AddInstructionalButton(GSG);
			blip.AddInstructionalButton(GDS);
			blip.AddInstructionalButton(MuoviSG);
			blip.AddInstructionalButton(MuoviSD);
			blip.AddInstructionalButton(BlipColoreDX);
			blip.AddInstructionalButton(BlipColoreSX);
			
			blip.MouseControlsEnabled = false;
			UIMenuListItem blipType = new UIMenuListItem("Modello", new List<dynamic>() { "~BLIP_40~" }, 0);
			blipColor = new UIMenuColorPanel("Colore Blip", ColorPanelType.Hair);
			blipType.AddPanel(blipColor);
			UIMenuSliderProgressItem blipDimensions = new UIMenuSliderProgressItem("Dimensioni", 100, 0);
			UIMenuItem blipName = new UIMenuItem("Nome", "Se lasci il campo vuoto, prenderà il nome dell'abitazione automaticamente");
			blip.AddItem(blipType);
			blip.AddItem(blipDimensions);
			blip.AddItem(blipName);

			blipType.OnListChanged += (item, index) =>
			{
				// aggiungere salvataggio sprite blip (rimuovere blip_ dal nome e lasciare il numero)
				Color a = (item.Panels[0] as UIMenuColorPanel).CurrentColor;
				item._itemSprite.Color = a;
			};
			#endregion
			*/

            #region marker

            UIMenuItem markerItem = new("Marker handler");
            UIMenu marker = new("Marker handler", "");
            markerItem.Activated += async (a, b) => await selectPoint.SwitchTo(marker, 0, true);
            selectPoint.AddItem(markerItem);
            marker.MouseControlsEnabled = false;
            marker.SetKey(UIMenu.MenuControls.Select, Control.Attack);
            markerEntranceHome = new UIMenuItem("Entry point on foot", "The marker is purely for guidance, IT WILL NOT BE VISIBLE IN GAME", SColor.HUD_Reddark, SColor.HUD_Red);
            markerEntranceGarage = new UIMenuItem("Entry point for the garage", "The marker is purely for guidance, IT WILL NOT BE VISIBLE IN GAME", SColor.HUD_Reddark, SColor.HUD_Red);
            markerEntranceRoof = new UIMenuItem("Roof entry point", "The marker is purely for guidance, IT WILL NOT BE VISIBLE IN GAME", SColor.HUD_Reddark, SColor.HUD_Red);
            posCamera = new UIMenuItem("Preview camera position", "Set the position and rotation of the camera when the citizen returns home or calls the intercom", SColor.HUD_Reddark, SColor.HUD_Red);
            marker.OnItemSelect += async (menu, item, index) =>
            {
                if (item == markerEntranceHome)
                {
                    markerEnterFoot = new MarkerEx(dummyMarker.MarkerType, dummyMarker.Position, SColor.Green);

                    if (property == TypeProperty.House)
                    {
                        homeDummy.MarkerEntrance = markerEnterFoot.Position;
                        homeDummy.SpawnOutside = markerEnterFoot.Position;
                    }
                }
                else if (item == markerEntranceGarage)
                {
                    markerEnterGarage = new MarkerEx(dummyMarker.MarkerType, dummyMarker.Position, SColor.Green);

                    switch (property)
                    {
                        case TypeProperty.House:
                            homeDummy.MarkerGarageExtern = markerEnterGarage.Position;
                            homeDummy.SpawnGarageVehicleOutside = markerEnterGarage.Position;

                            break;
                        case TypeProperty.Garage:
                            garageDummy.MarkerEntrance = markerEnterGarage.Position;
                            garageDummy.SpawnOutside = markerEnterGarage.Position;

                            break;
                    }
                }
                else if (item == markerEntranceRoof)
                {
                    markerEnterRoof = new MarkerEx(dummyMarker.MarkerType, dummyMarker.Position, SColor.Green);

                    if (property == TypeProperty.House)
                    {
                        homeDummy.MarkerRoof = markerEnterRoof.Position;
                        homeDummy.SpawnRoof = markerEnterRoof.Position;
                    }
                }
                else if (item == posCamera)
                {
                    CameraPosEntrance = MainCamera.Position.ToPosition();
                    CameraRotEntrance = (await MainCamera.CrosshairRaycast(1000)).HitPosition.ToPosition();

                    switch (property)
                    {
                        case TypeProperty.House:
                            homeDummy.CameraOutside.Pos = CameraPosEntrance;
                            homeDummy.CameraOutside.Rotation = CameraRotEntrance;

                            break;
                        case TypeProperty.Garage:
                            garageDummy.CameraOutside.Pos = CameraPosEntrance;
                            garageDummy.CameraOutside.Rotation = CameraRotEntrance;

                            break;
                    }
                }

                item.MainColor = SColor.HUD_Greendark;
                item.HighlightColor = SColor.HUD_Greenlight;
                item.SetRightBadge(BadgeIcon.STAR);
            };

            #endregion

            #endregion

            #region Interiors

            interiorHandlingHome.OnMenuOpen += async (a, b) =>
            {
                UIMenuListItem interior = new("", new List<dynamic>() { "" }, 0);
                a.Clear();
                Screen.Fading.FadeOut(800);
                while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
                SetPlayerControl(Cache.PlayerCache.MyPlayer.Player.Handle, false, 256);

                switch (property)
                {
                    case TypeProperty.House:
                        {
                            interior = new UIMenuListItem("Favourite Interior", new List<dynamic>()
                                {
                                    "Base",
                                    "Mid",
                                    "High [HighLife]",
                                    "High [2 floors]",
                                    "High [3 floors]",
                                    "High [Executive]"
                                }, 0);

                            if (includeGarage)
                            {
                                UIMenuListItem garageInterior = new("Garage type", new List<dynamic>() { "Base", "Mid [4]", "Mid [6]", "High [10]" }, 0);
                                interiorHandlingHome.AddItem(garageInterior);
                                garageInterior.OnListChanged += (item, index) =>
                                {
                                    switch (index)
                                    {
                                        case 0:
                                            homeDummy.VehCapacity = 2;

                                            break;
                                        case 1:
                                            homeDummy.VehCapacity = 4;

                                            break;
                                        case 2:
                                            homeDummy.VehCapacity = 6;

                                            break;
                                        case 3:
                                            homeDummy.VehCapacity = 10;

                                            break;
                                    }
                                };
                            }

                            break;
                        }
                    case TypeProperty.Garage:
                        interior = new UIMenuListItem("Favourite interior", new List<dynamic>() { "Base", "Mid [4]", "Mid [6]", "High [10]" }, 0);

                        break;
                }

                interiorHandlingHome.AddItem(interior);
                if (property == TypeProperty.House)
                {
                    UIMenuItem opzioniInteriorItem = new UIMenuItem("Selected interior's options");
                    interiorOptions = new("Selected interior's options", "");
                    interiorHandlingHome.AddItem(opzioniInteriorItem);
                    opzioniInteriorItem.Activated += async (a, b) => await interiorHandlingHome.SwitchTo(interiorOptions, 0, true);
                }
                if (MainCamera == null) MainCamera = World.CreateCamera(Cache.PlayerCache.MyPlayer.Position.ToVector3 + new Vector3(0, 0, 100), new Vector3(0, 0, 0), 45f);
                MainCamera.IsActive = true;
                RenderScriptCams(true, false, 1000, true, true);
                if (renderCamObject == null) renderCamObject = await Functions.SpawnLocalProp(Functions.HashInt("prop_ld_test_01"), Vector3.Zero, false, false);
                renderCamObject.IsVisible = false;

                switch (property)
                {
                    case TypeProperty.House:
                        SetFocusEntity(renderCamObject.Handle);
                        renderCamObject.Position = new Vector3(266.8514f, -998.9061f, -97.92068f);
                        PlaceObjectOnGroundProperly(renderCamObject.Handle);
                        MainCamera.Position = new Vector3(266.8514f, -998.9061f, -97.92068f);
                        MainCamera.PointAt(new Vector3(259.7751f, -998.6475f, -100.0068f));
                        homeDummy.MarkerExit = new Position(266.094f, -1007.487f, -101.800f);
                        homeDummy.SpawnInside = new Position(266.094f, -1007.487f, -101.800f);

                        break;
                    case TypeProperty.Garage:
                        SetFocusEntity(renderCamObject.Handle);
                        renderCamObject.Position = new Vector3(177.8964f, -1008.719f, -98.03687f);
                        PlaceObjectOnGroundProperly(renderCamObject.Handle);
                        MainCamera.Position = new Vector3(177.8964f, -1008.719f, -98.03687f);
                        MainCamera.PointAt(new Vector3(168.3609f, -1002.193f, -99.99992f));

                        break;
                }

                Screen.Fading.FadeIn(500);

                interior.OnListChanged += async (item, index) =>
                {
                    Screen.Fading.FadeOut(800);
                    while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
                    if (renderCamObject == null) renderCamObject = await Functions.SpawnLocalProp(Functions.HashInt("prop_ld_test_01"), Vector3.Zero, false, false);
                    renderCamObject.IsVisible = false;
                    if (MainCamera == null) MainCamera = World.CreateCamera(Cache.PlayerCache.MyPlayer.Position.ToVector3 + new Vector3(0, 0, 100), new Vector3(0, 0, 0), 45f);
                    MainCamera.IsActive = true;
                    RenderScriptCams(true, false, 1000, true, true);
                    Vector3 pos = Vector3.Zero;
                    Vector3 lookAt = Vector3.Zero;
                    intern = index;

                    switch (property)
                    {
                        case TypeProperty.House:
                            homeDummy.Type = index;

                            break;
                        case TypeProperty.Garage:
                            garageDummy.Type = index;

                            break;
                    }

                    switch (index)
                    {
                        case 0:
                            switch (property)
                            {
                                case TypeProperty.House:
                                    pos = new Vector3(266.8514f, -998.9061f, -97.92068f);
                                    lookAt = new Vector3(259.7751f, -998.6475f, -100.0068f);
                                    homeDummy.MarkerExit = new Position(266.094f, -1007.487f, -101.800f);
                                    homeDummy.SpawnInside = new Position(266.094f, -1007.487f, -101.800f);

                                    break;
                                case TypeProperty.Garage:
                                    pos = new Vector3(177.8964f, -1008.719f, -98.03687f);
                                    lookAt = new Vector3(168.3609f, -1002.193f, -99.99992f);
                                    garageDummy.SpawnInside = new Position(179.015f, -1000.326f, -100f);
                                    garageDummy.MarkerExit = new Position(179.015f, -1000.326f, -100f);

                                    break;
                            }

                            break;
                        case 1:
                            switch (property)
                            {
                                case TypeProperty.House:
                                    pos = new Vector3(339.3684f, -992.7239f, -98.21723f);
                                    lookAt = new Vector3(341.4973f, -999.5391f, -100.1962f);
                                    homeDummy.MarkerExit = new Position(346.493f, -1013.031f, -99.196f);
                                    homeDummy.SpawnInside = new Position(346.493f, -1013.031f, -99.196f);

                                    break;
                                case TypeProperty.Garage:
                                    pos = new Vector3(190.6334f, -1027.276f, -98.94763f);
                                    lookAt = new Vector3(193.8157f, -1024.415f, -99.99996f);
                                    garageDummy.SpawnInside = new Position(207.1461f, -1018.326f, -98.999f);
                                    garageDummy.MarkerExit = new Position(207.1461f, -1018.326f, -98.999f);

                                    break;
                            }

                            break;
                        case 2:
                            switch (property)
                            {
                                case TypeProperty.House:
                                    pos = new Vector3(-1465.857f, -535.3416f, 74.20998f);
                                    lookAt = new Vector3(-1467.427f, -544.514f, 72.46823f);
                                    homeDummy.SpawnInside = new Position(-1452.841f, -539.489f, 74.044f);
                                    homeDummy.MarkerExit = new Position(-1452.164f, -540.640f, 74.044f);

                                    break;
                                case TypeProperty.Garage:
                                    pos = new Vector3(206.7423f, -993.4413f, -98.09858f);
                                    lookAt = new Vector3(190.6937f, -1008.027f, -99.62811f);
                                    garageDummy.SpawnInside = new Position(210.759f, -999.0323f, -98.99997f);
                                    garageDummy.MarkerExit = new Position(210.759f, -999.0323f, -98.99997f);

                                    break;
                            }

                            break;
                        case 3:
                            switch (property)
                            {
                                case TypeProperty.House:
                                    pos = new Vector3(-42.78862f, -571.4902f, 89.38699f);
                                    lookAt = new Vector3(-35.83893f, -583.5001f, 88.47382f);
                                    homeDummy.SpawnInside = new Position(-17.54766f, -589.1531f, 90.11485f);
                                    homeDummy.MarkerExit = new Position(-17.54766f, -589.1531f, 90.11485f);

                                    break;
                                case TypeProperty.Garage:
                                    pos = new Vector3(220.5728f, -1007.01f, -98.10276f);
                                    lookAt = new Vector3(225.9477f, -996.6439f, -99.9992f);
                                    garageDummy.SpawnInside = new Position(238.103f, -1004.813f, -98.99992f);
                                    garageDummy.MarkerExit = new Position(238.103f, -1004.813f, -98.99992f);

                                    break;
                            }

                            break;
                        case 4:
                            if (property == TypeProperty.House)
                            {
                                pos = new Vector3(-169.7948f, 478.3921f, 138.4392f);
                                lookAt = new Vector3(-166.9105f, 485.8192f, 136.8266f);
                                homeDummy.SpawnInside = new Position(-173.9128f, 496.8375f, 137.667f);
                                homeDummy.MarkerExit = new Position(-173.9128f, 496.8375f, 137.667f);
                            }

                            break;
                        case 5:
                            if (property == TypeProperty.House)
                            {
                                pos = new Vector3(-791.5707f, 343.7827f, 217.8111f);
                                lookAt = new Vector3(-784.6417f, 330.4529f, 216.0382f);
                                homeDummy.SpawnInside = new Position(-786.5125f, 315.8108f, 217.6385f);
                                homeDummy.MarkerExit = new Position(-786.5125f, 315.8108f, 217.6385f);
                            }

                            break;
                    }

                    renderCamObject.Position = pos;
                    PlaceObjectOnGroundProperly(renderCamObject.Handle);
                    MainCamera.Position = pos;
                    MainCamera.PointAt(lookAt);
                    await BaseScript.Delay(1000); // non voglio che i giocatori si ritrovino ad aspettare che carica.. così aspettano a schermo nero
                    Screen.Fading.FadeIn(500);
                };
            };

            interiorOptions.OnMenuOpen += (_new, b) =>
            {
                _new.Clear();

                switch (intern)
                {
                    #region Low

                    case 0: // low
                        if (property == TypeProperty.House)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", homeDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", homeDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", homeDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    homeDummy.Strip = check;
                                    IPLs.IPLInstance.GTAOHouseLow1.Strip.Enable(IPLs.IPLInstance.GTAOHouseLow1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Strip.Enable(IPLs.IPLInstance.GTAOHouseLow1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Strip.Enable(IPLs.IPLInstance.GTAOHouseLow1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    homeDummy.Booze = check;
                                    IPLs.IPLInstance.GTAOHouseLow1.Booze.Enable(IPLs.IPLInstance.GTAOHouseLow1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Booze.Enable(IPLs.IPLInstance.GTAOHouseLow1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Booze.Enable(IPLs.IPLInstance.GTAOHouseLow1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    homeDummy.Smoke = check;
                                    IPLs.IPLInstance.GTAOHouseLow1.Smoke.Enable(IPLs.IPLInstance.GTAOHouseLow1.Smoke.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Smoke.Enable(IPLs.IPLInstance.GTAOHouseLow1.Smoke.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Smoke.Enable(IPLs.IPLInstance.GTAOHouseLow1.Smoke.Stage3, check, true);
                                }

                                await BaseScript.Delay(500);
                                Screen.Fading.FadeIn(500);
                            };
                        }

                        break;

                    #endregion

                    #region Mid

                    case 1:
                        if (property == TypeProperty.House)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", homeDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", homeDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", homeDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    homeDummy.Strip = check;
                                    IPLs.IPLInstance.GTAOHouseMid1.Strip.Enable(IPLs.IPLInstance.GTAOHouseMid1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Strip.Enable(IPLs.IPLInstance.GTAOHouseMid1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Strip.Enable(IPLs.IPLInstance.GTAOHouseMid1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    homeDummy.Booze = check;
                                    IPLs.IPLInstance.GTAOHouseMid1.Booze.Enable(IPLs.IPLInstance.GTAOHouseMid1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Booze.Enable(IPLs.IPLInstance.GTAOHouseMid1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Booze.Enable(IPLs.IPLInstance.GTAOHouseMid1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    homeDummy.Smoke = check;
                                    IPLs.IPLInstance.GTAOHouseMid1.Smoke.Enable(IPLs.IPLInstance.GTAOHouseMid1.Smoke.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Smoke.Enable(IPLs.IPLInstance.GTAOHouseMid1.Smoke.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Smoke.Enable(IPLs.IPLInstance.GTAOHouseMid1.Smoke.Stage3, check, true);
                                }

                                await BaseScript.Delay(500);
                                Screen.Fading.FadeIn(500);
                            };
                        }

                        break;

                    #endregion

                    #region HighLife

                    case 2:
                        if (property == TypeProperty.House)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", homeDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", homeDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", homeDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    homeDummy.Strip = check;
                                    IPLs.IPLInstance.HLApartment1.Strip.Enable(IPLs.IPLInstance.HLApartment1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.HLApartment1.Strip.Enable(IPLs.IPLInstance.HLApartment1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.HLApartment1.Strip.Enable(IPLs.IPLInstance.HLApartment1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    homeDummy.Booze = check;
                                    IPLs.IPLInstance.HLApartment1.Booze.Enable(IPLs.IPLInstance.HLApartment1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.HLApartment1.Booze.Enable(IPLs.IPLInstance.HLApartment1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.HLApartment1.Booze.Enable(IPLs.IPLInstance.HLApartment1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    homeDummy.Smoke = check;
                                    IPLs.IPLInstance.HLApartment1.Smoke.Enable(IPLs.IPLInstance.HLApartment1.Smoke.Stage1, check, true);
                                    IPLs.IPLInstance.HLApartment1.Smoke.Enable(IPLs.IPLInstance.HLApartment1.Smoke.Stage2, check, true);
                                    IPLs.IPLInstance.HLApartment1.Smoke.Enable(IPLs.IPLInstance.HLApartment1.Smoke.Stage3, check, true);
                                }

                                await BaseScript.Delay(500);
                                Screen.Fading.FadeIn(500);
                            };
                        }

                        break;

                    #endregion

                    #region OnlineHi

                    case 3:
                        if (property == TypeProperty.House)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", homeDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", homeDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", homeDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    homeDummy.Strip = check;
                                    IPLs.IPLInstance.GTAOApartmentHi1.Strip.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Strip.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Strip.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    homeDummy.Booze = check;
                                    IPLs.IPLInstance.GTAOApartmentHi1.Booze.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Booze.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Booze.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    homeDummy.Smoke = check;
                                    IPLs.IPLInstance.GTAOApartmentHi1.Smoke.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Smoke.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Smoke.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Smoke.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Smoke.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Smoke.Stage3, check, true);
                                }

                                await BaseScript.Delay(500);
                                Screen.Fading.FadeIn(500);
                            };
                        }

                        break;

                    #endregion

                    #region OnlineHouseHi

                    case 4:
                        if (property == TypeProperty.House)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", homeDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", homeDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", homeDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    homeDummy.Strip = check;
                                    IPLs.IPLInstance.GTAOHouseHi1.Strip.Enable(IPLs.IPLInstance.GTAOHouseHi1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Strip.Enable(IPLs.IPLInstance.GTAOHouseHi1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Strip.Enable(IPLs.IPLInstance.GTAOHouseHi1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    homeDummy.Booze = check;
                                    IPLs.IPLInstance.GTAOHouseHi1.Booze.Enable(IPLs.IPLInstance.GTAOHouseHi1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Booze.Enable(IPLs.IPLInstance.GTAOHouseHi1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Booze.Enable(IPLs.IPLInstance.GTAOHouseHi1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    homeDummy.Smoke = check;
                                    IPLs.IPLInstance.GTAOHouseHi1.Smoke.Enable(IPLs.IPLInstance.GTAOHouseHi1.Smoke.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Smoke.Enable(IPLs.IPLInstance.GTAOHouseHi1.Smoke.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Smoke.Enable(IPLs.IPLInstance.GTAOHouseHi1.Smoke.Stage3, check, true);
                                }

                                await BaseScript.Delay(500);
                                Screen.Fading.FadeIn(500);
                            };
                        }

                        break;

                    #endregion

                    #region Executive

                    case 5:
                        if (property == TypeProperty.House)
                        {
                            int idx = 0;
                            UIMenuListItem tema = new("Stile appartamento", new List<dynamic>()
                                    {
                                    "Modern",
                                    "Moody",
                                    "Vibrant",
                                    "Sharp",
                                    "Monochrome",
                                    "Seductive",
                                    "Regal",
                                    "Aqua"
                                    }, homeDummy.Style);                                                  // cambiare index
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", homeDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", homeDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", homeDummy.Smoke); // cambiare checked
                            _new.AddItem(tema);
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            tema.OnListChanged += async (item, index) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);
                                idx = index;
                                homeDummy.Style = idx;

                                switch (index)
                                {
                                    case 0:
                                        IPLs.IPLInstance.ExecApartment1.Style.Set(IPLs.IPLInstance.ExecApartment1.Style.Theme.Modern, ref IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, true);

                                        break;
                                    case 1:
                                        IPLs.IPLInstance.ExecApartment1.Style.Set(IPLs.IPLInstance.ExecApartment1.Style.Theme.Moody, ref IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, true);

                                        break;
                                    case 2:
                                        IPLs.IPLInstance.ExecApartment1.Style.Set(IPLs.IPLInstance.ExecApartment1.Style.Theme.Vibrant, ref IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, true);

                                        break;
                                    case 3:
                                        IPLs.IPLInstance.ExecApartment1.Style.Set(IPLs.IPLInstance.ExecApartment1.Style.Theme.Sharp, ref IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, true);

                                        break;
                                    case 4:
                                        IPLs.IPLInstance.ExecApartment1.Style.Set(IPLs.IPLInstance.ExecApartment1.Style.Theme.Monochrome, ref IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, true);

                                        break;
                                    case 5:
                                        IPLs.IPLInstance.ExecApartment1.Style.Set(IPLs.IPLInstance.ExecApartment1.Style.Theme.Seductive, ref IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, true);

                                        break;
                                    case 6:
                                        IPLs.IPLInstance.ExecApartment1.Style.Set(IPLs.IPLInstance.ExecApartment1.Style.Theme.Regal, ref IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, true);

                                        break;
                                    case 7:
                                        IPLs.IPLInstance.ExecApartment1.Style.Set(IPLs.IPLInstance.ExecApartment1.Style.Theme.Aqua, ref IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, true);

                                        break;
                                }

                                await BaseScript.Delay(500);
                                Screen.Fading.FadeIn(500);
                            };
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    homeDummy.Strip = check;
                                    IPLs.IPLInstance.ExecApartment1.Strip.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Strip.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Strip.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    homeDummy.Booze = check;
                                    IPLs.IPLInstance.ExecApartment1.Booze.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Booze.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Booze.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    homeDummy.Smoke = check;
                                    IPLs.IPLInstance.ExecApartment1.Smoke.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Smoke.Stage1, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Smoke.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Smoke.Stage2, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Smoke.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Smoke.Stage3, check, true);
                                }

                                await BaseScript.Delay(500);
                                Screen.Fading.FadeIn(500);
                            };
                        }

                        break;

                        #endregion
                }


            };


            #endregion

            #region StateChanged

            creation.OnMenuOpen += (a, b) =>
            {
                oldInstance = Cache.PlayerCache.MyPlayer.Status.Instance;
                Cache.PlayerCache.MyPlayer.Status.Instance.InstancePlayer("Properties Creator");
            };
            creation.OnMenuClose += (a) =>
            {
                if (Cache.PlayerCache.MyPlayer.Status.Instance.Instance == "Properties Creator") Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
                Cache.PlayerCache.MyPlayer.Status.Instance = oldInstance;
            };
            selectPoint.OnMenuOpen += async (a, b) =>
            {
                SetPlayerControl(Cache.PlayerCache.MyPlayer.Player.Handle, false, 256);
                Screen.Fading.FadeOut(800);
                while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
                if (MainCamera == null) MainCamera = World.CreateCamera(Vector3.Zero, new Vector3(0, 0, 0), 45f);
                MainCamera.Position = Cache.PlayerCache.MyPlayer.Position.ToVector3 + new Vector3(0, 0, 100);
                MainCamera.IsActive = true;
                RenderScriptCams(true, false, 1000, true, true);
                curLocation = MainCamera.Position.ToPosition();
                curRotation = MainCamera.Rotation.ToPosition();
                checkTimer = GetGameTimer();
                Client.Instance.AddTick(CreatorCameraControl);
                Screen.Fading.FadeIn(500);

            };

            marker.OnMenuOpen += (a, b) =>
            {
                marker.Clear();

                switch (property)
                {
                    case TypeProperty.House:
                        marker.AddItem(markerEntranceHome);
                        if (includeGarage) marker.AddItem(markerEntranceGarage);
                        if (includeRoof) marker.AddItem(markerEntranceRoof);
                        marker.AddItem(posCamera);
                        break;
                    case TypeProperty.Garage:
                        marker.AddItem(markerEntranceGarage);
                        marker.AddItem(posCamera);
                        break;
                }

                Client.Instance.AddTick(MarkerTick);
                markerEnterFoot ??= new MarkerEx(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.Red);
                markerEnterGarage ??= new MarkerEx(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.Red);
                markerEnterRoof ??= new MarkerEx(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.Red);
            };

            marker.OnMenuClose += (a) => Client.Instance.RemoveTick(MarkerTick);

            selectPoint.OnMenuClose += async (a) =>
            {
                Client.Instance.RemoveTick(CreatorCameraControl);
                Screen.Fading.FadeOut(800);
                while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
                ClearFocus();
                await BaseScript.Delay(100);

                if (MainCamera.Exists() && World.RenderingCamera == MainCamera)
                {
                    RenderScriptCams(false, false, 1000, false, false);
                    MainCamera.IsActive = false;
                }

                SetPlayerControl(Cache.PlayerCache.MyPlayer.Player.Handle, true, 256);
                Screen.Fading.FadeIn(500);
            };

            interiorHandlingHome.OnMenuClose += async (a) =>
            {
                Screen.Fading.FadeOut(800);
                while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
                MainCamera.StopPointing();
                ClearFocus();

                if (MainCamera.Exists() && World.RenderingCamera == MainCamera)
                {
                    RenderScriptCams(false, false, 1000, false, false);
                    MainCamera.IsActive = false;
                }

                SetPlayerControl(Cache.PlayerCache.MyPlayer.Player.Handle, true, 256);
                Screen.Fading.FadeIn(500);

            };
            #endregion

            UIMenuItem Save = new("Save property", "⚠️ This is not reversible if not calling an Admin!");
            creation.AddItem(Save);
            Save.Activated += async (menu, item) =>
            {
                switch (property)
                {
                    case TypeProperty.House when !string.IsNullOrWhiteSpace(homeDummy.Label):
                        {
                            if (!string.IsNullOrWhiteSpace(abbreviation))
                            {
                                if (homeDummy.Price > 0)
                                {
                                    if (homeDummy.MarkerEntrance != Position.Zero)
                                    {
                                        if (homeDummy.CameraOutside.Pos != Position.Zero && homeDummy.CameraOutside.Rotation != Position.Zero)
                                        {
                                            if (homeDummy.GarageIncluded)
                                            {
                                                if (homeDummy.MarkerGarageExtern != Position.Zero && homeDummy.SpawnGarageVehicleOutside != Position.Zero)
                                                {
                                                    if (homeDummy.HasRoof)
                                                    {
                                                        if (homeDummy.MarkerRoof != Position.Zero)
                                                        {
                                                            BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "home", homeDummy.ToJson(), abbreviation);
                                                            MenuHandler.CloseAndClearHistory();
                                                        }
                                                        else
                                                        {
                                                            HUD.ShowNotification("You included a Roof but the Roof Marker is missing!", ColoreNotifica.Red, true);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // NO INCLUDED ROOF
                                                        BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "home", homeDummy.ToJson(), abbreviation);
                                                        MenuHandler.CloseAndClearHistory();
                                                    }
                                                }
                                                else
                                                {
                                                    HUD.ShowNotification("You included the Garage but the garage's Markers are missing!", ColoreNotifica.Red, true);
                                                }
                                            }
                                            else // non garage incluso
                                            {
                                                if (homeDummy.HasRoof)
                                                {
                                                    if (homeDummy.MarkerRoof != Position.Zero)
                                                    {
                                                        BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "home", homeDummy.ToJson(), abbreviation);
                                                        Client.Settings.RolePlay.Properties.Apartments.Add(abbreviation, homeDummy);
                                                        MenuHandler.CloseAndClearHistory();
                                                    }
                                                    else
                                                    {
                                                        HUD.ShowNotification("You included the roof but there's no roof entrance Marker!", ColoreNotifica.Red, true);
                                                    }
                                                }
                                                else
                                                {
                                                    // non tetto incluso
                                                    BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "home", homeDummy.ToJson(), abbreviation);
                                                    Client.Settings.RolePlay.Properties.Apartments.Add(abbreviation, homeDummy);
                                                    MenuHandler.CloseAndClearHistory();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            HUD.ShowNotification("You didn't set an entrance Camera!", ColoreNotifica.Red, true);
                                        }
                                    }
                                    else
                                    {
                                        HUD.ShowNotification("You didn't set an entrance marker!", ColoreNotifica.Red, true);
                                    }
                                }
                                else
                                {
                                    HUD.ShowNotification("You didn't give it a price!", ColoreNotifica.Red, true);
                                }
                            }
                            else
                            {
                                HUD.ShowNotification("You didn't give an abbreviation!", ColoreNotifica.Red, true);
                            }

                            break;
                        }
                    case TypeProperty.House:
                        HUD.ShowNotification("Yo didn't give a name to the property!", ColoreNotifica.Red, true);

                        break;
                    case TypeProperty.Garage when !string.IsNullOrWhiteSpace(garageDummy.Label):
                        {
                            if (!string.IsNullOrWhiteSpace(abbreviation))
                            {
                                if (garageDummy.Price > 0)
                                {
                                    if (garageDummy.MarkerEntrance != Position.Zero)
                                    {
                                        if (garageDummy.CameraOutside.Pos != Position.Zero && garageDummy.CameraOutside.Rotation != Position.Zero)
                                        {
                                            BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "garage", garageDummy.ToJson(), abbreviation);
                                            Client.Settings.RolePlay.Properties.Garages.Garages.Add(abbreviation, garageDummy);
                                            MenuHandler.CloseAndClearHistory();
                                        }
                                        else
                                        {
                                            HUD.ShowNotification("You didn't set an entrance Camera!", ColoreNotifica.Red, true);
                                        }
                                    }
                                    else
                                    {
                                        HUD.ShowNotification("You didn't set an entrance marker!", ColoreNotifica.Red, true);
                                    }
                                }
                                else
                                {
                                    HUD.ShowNotification("You didn't give it a price!", ColoreNotifica.Red, true);
                                }
                            }
                            else
                            {
                                HUD.ShowNotification("You didn't give an abbreviation!", ColoreNotifica.Red, true);
                            }

                            break;
                        }
                    case TypeProperty.Garage:
                        HUD.ShowNotification("Yo didn't give a name to the property!", ColoreNotifica.Red, true);

                        break;
                }
            };
            creation.Visible = true;
        }

        private static async Task CreatorCameraControl()
        {
            float forwardPush = 0.8f;
            if (GetGameTimer() - checkTimer > (int)Math.Ceiling(1000 / forwardPush)) curLocation.SetFocus();
            string tast = $"~INPUTGROUP_MOVE~ to move.\n~INPUTGROUP_LOOK~ to rotate camera.\n~INPUT_COVER~ ~INPUT_VEH_HORN~ to move updwards/downwards.\n~INPUT_FRONTEND_X~ to change speed.\nActual speed: ~y~{travelSpeedStr}~w~.";
            string gampa = $"~INPUTGROUP_MOVE~ to move.\n~INPUTGROUP_LOOK~ to rotate camera.\n~INPUTGROUP_FRONTEND_TRIGGERS~ to move updwards/downwards.\n~INPUT_FRONTEND_X~ to change speed.\nActual speed: ~y~{travelSpeedStr}~w~.";
            HUD.ShowHelpNoMenu(IsInputDisabled(2) ? tast : gampa);

            if (blipColor != null)
                if (blipColor.ParentItem.Parent.Visible)
                {
                    if (Input.IsControlJustPressed(Control.FrontendLb)) blipColor.CurrentSelection++;
                    if (Input.IsControlJustPressed(Control.FrontendRb)) blipColor.CurrentSelection--;
                }

            Game.DisableAllControlsThisFrame(0);
            Game.DisableControlThisFrame(0, Control.LookLeftRight);
            Game.DisableControlThisFrame(0, Control.LookUpDown);
            Game.DisableControlThisFrame(0, Control.LookDown);
            Game.DisableControlThisFrame(0, Control.LookUp);
            Game.DisableControlThisFrame(0, Control.LookLeft);
            Game.DisableControlThisFrame(0, Control.LookRight);
            Game.DisableControlThisFrame(0, Control.LookDownOnly);
            Game.DisableControlThisFrame(0, Control.LookUpOnly);
            Game.DisableControlThisFrame(0, Control.LookLeftOnly);
            Game.DisableControlThisFrame(0, Control.LookRightOnly);
            float rotationSpeed = 1f;

            switch (travelSpeed)
            {
                case 0:
                    forwardPush = 0.8f; //medium
                    travelSpeedStr = "Medium";

                    break;
                case 1:
                    forwardPush = 1.8f; //fast
                    travelSpeedStr = "Fast";

                    break;
                case 2:
                    forwardPush = 2.6f; //very fast
                    travelSpeedStr = "Very fast";

                    break;
                case 3:
                    forwardPush = 0.025f; //very slow
                    travelSpeedStr = "Very slow";

                    break;
                case 4:
                    forwardPush = 0.05f; //very slow
                    travelSpeedStr = "Very slow";

                    break;
                case 5:
                    forwardPush = 0.2f; //slow
                    travelSpeedStr = "Slow";

                    break;
            }

            float zVect = forwardPush / 3;

            if (Input.IsDisabledControlJustPressed(Control.FrontendX))
            {
                travelSpeed++;
                if (travelSpeed > 5) travelSpeed = 0;
            }

            float xVectFwd = forwardPush * (float)Math.Sin(Functions.Deg2rad(curRotation.Z)) * -1.0f;
            float yVectFwd = forwardPush * (float)Math.Cos(Functions.Deg2rad(curRotation.Z));
            float xVectLat = forwardPush * (float)Math.Cos(Functions.Deg2rad(curRotation.Z));
            float yVectLat = forwardPush * (float)Math.Sin(Functions.Deg2rad(curRotation.Z));
            float z = 0;
            GetGroundZFor_3dCoord(curLocation.X, curLocation.Y, curLocation.Z, ref z, false);
            if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;

            if (Input.IsDisabledControlPressed(Control.MoveUpOnly))
            {
                curLocation.X += xVectFwd;
                curLocation.Y += yVectFwd;
                if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;
            }

            if (Input.IsDisabledControlPressed(Control.MoveDownOnly))
            {
                curLocation.X -= xVectFwd;
                curLocation.Y -= yVectFwd;
                if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;
            }

            if (Input.IsDisabledControlPressed(Control.MoveLeftOnly))
            {
                curLocation.X -= xVectLat;
                curLocation.Y -= yVectLat;
                if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;
            }

            if (Input.IsDisabledControlPressed(Control.MoveRightOnly))
            {
                curLocation.X += xVectLat;
                curLocation.Y += yVectLat;
                if (z != 0 && curLocation.Z < z + 0.5f) curLocation.Z = z + 0.5f;
            }

            if (Input.IsControlPressed(Control.FrontendLt, PadCheck.Controller) || Input.IsControlPressed(Control.Cover, PadCheck.Keyboard))
            {
                curLocation.Z += zVect;
                if (z != 0 && curLocation.Z > z + 300) curLocation.Z = z + 300;
            }

            if (Input.IsControlPressed(Control.FrontendRt, PadCheck.Controller) || Input.IsControlPressed(Control.VehicleHorn, PadCheck.Keyboard))
            {
                curLocation.Z -= zVect;
                if (curLocation.Z > z + 0.5f) curLocation.Z -= zVect;
            }

            /* TODO: DO WE NEED TO ROTATE CAMERA? 🤔🤔
             * 
                if (Input.IsDisabledControlPressed(Control.Cover, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendLb, PadCheck.Controller))
                {
                    curRotation.Y += rotationSpeed; // rotazione verso sinistra
                    if (curRotation.Y > 179.999999999f) curRotation.Y = -180f;
                }
                if (Input.IsDisabledControlPressed(Control.HUDSpecial, PadCheck.Keyboard) || Input.IsDisabledControlPressed(Control.FrontendRb, PadCheck.Controller))
                {
                    curRotation.Y -= rotationSpeed; // rotazione verso destra
                    if (curRotation.Y < -179.999999999f) curRotation.Y = 180f;
                }
            */
            if (!IsInputDisabled(2))
            {
                if (Input.IsControlPressed(Control.LookDownOnly))
                {
                    curRotation.X -= rotationSpeed;
                    if (curRotation.X < -75f) curRotation.X = -75f;
                }

                if (Input.IsControlPressed(Control.LookUpOnly))
                {
                    curRotation.X += rotationSpeed;
                    if (curRotation.X > 75f) curRotation.X = 75f;
                }

                if (Input.IsControlPressed(Control.LookLeftOnly))
                {
                    curRotation.Z += rotationSpeed;
                    if (curRotation.Z > 179.999999999f) curRotation.Z = -180f;
                }

                if (Input.IsControlPressed(Control.LookRightOnly))
                {
                    curRotation.Z -= rotationSpeed;
                    if (curRotation.Z < -179.999999999f) curRotation.Z = 180f;
                }
            }
            else
            {
                curRotation.X -= GetDisabledControlNormal(1, 2) * rotationSpeed * 8.0f;
                if (curRotation.X < -75f) curRotation.X = -75f;
                if (curRotation.X > 75f) curRotation.X = 75f;
                curRotation.Z -= GetDisabledControlNormal(1, 1) * rotationSpeed * 8.0f;
                if (curRotation.Z > 179.999999999f) curRotation.Z = -180f;
                if (curRotation.Z < -179.999999999f) curRotation.Z = 180f;
            }

            MainCamera.Position = curLocation.ToVector3;
            MainCamera.Rotation = curRotation.ToVector3;
        }

        public static async Task MarkerTick()
        {
            RaycastResult res = await MainCamera.CrosshairRaycast(150f);
            Vector3 direction = res.HitPosition;
            dummyMarker.Color = SColor.Red;
            if (!posCamera.Selected) dummyMarker.Draw();
            dummyMarker.Position = direction.ToPosition();
            float z = 0;
            GetGroundZFor_3dCoord(direction.X, direction.Y, direction.Z, ref z, false);
            if (z != 0 && res.DitHit)
                dummyMarker.Position = new(dummyMarker.Position.X, dummyMarker.Position.Y, z);
            if (markerEntranceHome.Selected)
                if (markerEntranceHome.RightBadge != BadgeIcon.NONE)
                    markerEnterFoot.Draw();
            if (markerEntranceGarage.Selected)
                if (markerEntranceGarage.RightBadge != BadgeIcon.NONE)
                    markerEnterGarage.Draw();
            if (markerEntranceRoof.Selected)
                if (markerEntranceRoof.RightBadge != BadgeIcon.NONE)
                    markerEnterRoof.Draw();
        }

        private static async Task BlipMarker()
        {
            if (blipColor != null)
            {
                RaycastResult res = await MainCamera.CrosshairRaycast(IntersectOptions.Everything, 150f);
                Vector3 direction = res.HitPosition;
                Vector3 pos = new(direction.X, direction.Y, curLocation.Z);
                string val = (blipColor.ParentItem as UIMenuListItem).Items[(blipColor.ParentItem as UIMenuListItem).Index] as string;
                int r = 0, g = 0, b = 0, a = 0;
                GetHudColour(blipColor.CurrentSelection, ref r, ref b, ref g, ref a);
                World.DrawMarker((MarkerType)8, pos, Vector3.Zero, new Vector3(90, 90, 0), new Vector3(5f), Color.FromArgb(a, r, g, b), true, false, true, "blips", val.Substring(1, val.Length - 2).ToLower());
                //metto un marker sotto
                float z = 0;
                GetGroundZFor_3dCoord(pos.X, pos.Y, pos.Z, ref z, false);
                World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(direction.X, direction.Y, z), Vector3.Zero, Vector3.Zero, new Vector3(2f, 2f, pos.Z - z), SColor.White.ToColor());
            }
        }
    }
}