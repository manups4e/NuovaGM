using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Whitelistati.VenditoreCase
{
    internal static class MenuCreazioneCasa
    {
        private static Camera MainCamera;
        private static int travelSpeed = 0;
        private static Position curLocation;
        private static Position curRotation;
        private static Position CameraPosIngresso;
        private static Position CameraRotIngresso;
        private static string travelSpeedStr = "Media";
        private static int checkTimer = 0;
        private static MarkerEx markerIngrPiedi;
        private static MarkerEx markerIngrGarage;
        private static MarkerEx markerIngrTetto;
        private static UIMenuItem markerIngressoCasa;
        private static UIMenuItem markerIngressoGarage;
        private static UIMenuItem markerIngressoTetto;
        private static UIMenuItem posCamera;
        private static UIMenuColorPanel blipColor;
        private static Prop renderCamObject;
        private static MarkerEx dummyMarker = new(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.WhiteSmoke);
        private static int interno = 0;
        private static string abbreviazione;

        private enum TipoImmobile
        {
            Casa,
            Garage
        }

        public static async void MenuCreazioneCase()
        {
            MainCamera = null;
            travelSpeed = 0;
            curLocation = Position.Zero;
            curRotation = Position.Zero;
            CameraPosIngresso = Position.Zero;
            CameraRotIngresso = Position.Zero;
            travelSpeedStr = "Media";
            checkTimer = 0;
            markerIngrPiedi = null;
            markerIngrGarage = null;
            markerIngrTetto = null;
            markerIngressoCasa = null;
            markerIngressoGarage = null;
            markerIngressoTetto = null;
            posCamera = null;
            blipColor = null;
            renderCamObject = null;
            dummyMarker = new MarkerEx(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.WhiteSmoke);
            interno = 0;
            abbreviazione = "";
            InstanceBags oldInstance = new();
            ConfigHouses casaDummy = new();
            casaDummy.VehCapacity = 2;
            Garages garageDummy = new();
            bool includiGarage = false;
            bool includiTetto = false;
            TipoImmobile immobile = TipoImmobile.Casa;

            #region dichiarazione

            UIMenu creazione = new("Creatore Immobiliare", "Usare con cautela!", new PointF(1450f, 0), "thelastgalaxy", "bannerbackground", false, true);
            creazione.MouseControlsEnabled = false;
            UIMenuListItem tipo = new("Tipo di immobile", new List<dynamic>() { "Casa", "Garage" }, 0);
            creazione.AddItem(tipo);
            tipo.OnListChanged += (item, index) => immobile = (TipoImmobile)index;
            UIMenuItem datiCasaItem = new("1. Dati dell'immobile"); // NB: nome provvisorio
            UIMenu datiCasa = new("1. Dati dell'immobile", "");
            datiCasa.MouseControlsEnabled = false;
            UIMenuItem selezionePuntoItem = new("2. Gestione esterni"); // NB: nome provvisorio
            UIMenu selezionePunto = new("2. Gestione esterni", "");
            selezionePunto.MouseControlsEnabled = false;
            UIMenuItem gestioneInteriorCasaItem = new("3. Gestione interni"); // NB: nome provvisorio
            UIMenu gestioneInteriorCasa = new("3. Gestione interni", "");
            gestioneInteriorCasa.MouseControlsEnabled = false;
            UIMenu opzioniInterior = null;

            datiCasaItem.Activated += async (a, b) => await creazione.SwitchTo(datiCasa, 0, true);
            selezionePuntoItem.Activated += async (a, b) => await creazione.SwitchTo(selezionePunto, 0, true);
            gestioneInteriorCasaItem.Activated += async (a, b) => await creazione.SwitchTo(gestioneInteriorCasa, 0, true);
            creazione.AddItem(datiCasaItem);
            creazione.AddItem(selezionePuntoItem);
            creazione.AddItem(gestioneInteriorCasaItem);
            #endregion

            #region Dati immobile

            datiCasa.OnMenuOpen += async (a, b) =>
            {
                datiCasa.Clear();
                UIMenuItem nomeImmobile = new("Nome dell'immobile", "Sarebbe preferibile inserire la via e il numero civico nel nome, ma puoi decidere tu il nome come vuoi!");
                UIMenuItem nomeAbbreviato = new("Nome Abbreviato", "Per questioni di salvataggio, serve un nome abbreviato per indicizzare l'immobile nel catabase del giocatore.. ad esempio: 0232 Paleto Boulevard => 0232PB");

                switch (immobile)
                {
                    case TipoImmobile.Casa:
                        {
                            if (casaDummy.Label != null) nomeAbbreviato.SetRightLabel(abbreviazione);

                            break;
                        }
                    case TipoImmobile.Garage:
                        {
                            if (garageDummy.Label != null)
                            {
                                nomeImmobile.SetRightLabel(garageDummy.Label.Length > 15 ? garageDummy.Label.Substring(0, 15) + "..." : garageDummy.Label);
                                nomeAbbreviato.SetRightLabel(abbreviazione);
                            }

                            break;
                        }
                }

                datiCasa.AddItem(nomeImmobile);
                datiCasa.AddItem(nomeAbbreviato);

                if (immobile == TipoImmobile.Casa)
                {
                    UIMenuCheckboxItem garageIncluso = new("Garage incluso", UIMenuCheckboxStyle.Tick, includiGarage, "Se attivato l'immobile avrà il garage incluso");
                    UIMenuCheckboxItem tettoIncluso = new("Tetto incluso", UIMenuCheckboxStyle.Tick, includiTetto, "Se attivato l'immobile avrà il tetto raggiungibile");
                    datiCasa.AddItem(garageIncluso);
                    datiCasa.AddItem(tettoIncluso);
                    garageIncluso.CheckboxEvent += (item, _checked) =>
                    {
                        includiGarage = _checked;
                        casaDummy.GarageIncluded = _checked;
                    };
                    tettoIncluso.CheckboxEvent += (item, _checked) =>
                    {
                        includiTetto = _checked;
                        casaDummy.HasRoof = _checked;
                    };
                }

                UIMenuItem prezzo = new("Prezzo di vendita", "Inserisci un prezzo base di vendita, in modo che tutti abbiate un'idea di quanto costa comprandolo");

                switch (immobile)
                {
                    case TipoImmobile.Casa:
                        prezzo.SetRightLabel("$" + casaDummy.Price);

                        break;
                    case TipoImmobile.Garage:
                        prezzo.SetRightLabel("$" + garageDummy.Price);

                        break;
                }

                datiCasa.AddItem(prezzo);
                datiCasa.OnItemSelect += async (menu, item, index) =>
                {
                    if (item == nomeImmobile)
                    {
                        string valore = await HUD.GetUserInput("Nome dell'immobile", "", 30);
                        item.SetRightLabel(valore.Length > 15 ? valore.Substring(0, 15) + "..." : valore);

                        switch (immobile)
                        {
                            case TipoImmobile.Casa:
                                casaDummy.Label = valore;

                                break;
                            case TipoImmobile.Garage:
                                garageDummy.Label = valore;

                                break;
                        }
                    }
                    else if (item == nomeAbbreviato)
                    {
                        string valore = await HUD.GetUserInput("Nome abbreviato", "", 7);
                        item.SetRightLabel(valore);
                        abbreviazione = valore;
                    }
                    else if (item == prezzo)
                    {
                        string valore = await HUD.GetUserInput("Prezzo", "", 10);
                        item.SetRightLabel("$" + valore);

                        switch (immobile)
                        {
                            case TipoImmobile.Casa:
                                casaDummy.Price = Convert.ToInt32(valore);

                                break;
                            case TipoImmobile.Garage:
                                garageDummy.Price = Convert.ToInt32(valore);

                                break;
                        }
                    }
                };
            };

            #endregion

            #region Gestione esterni

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

            UIMenuItem markerItem = new("Gestione markers");
            UIMenu marker = new("Gestione markers", "");
            markerItem.Activated += async (a, b) => await selezionePunto.SwitchTo(marker, 0, true);
            selezionePunto.AddItem(markerItem);
            marker.MouseControlsEnabled = false;
            marker.SetKey(UIMenu.MenuControls.Select, Control.Attack);
            markerIngressoCasa = new UIMenuItem("Punto di ingresso a piedi", "Il marker è puramente di guida, NON SARA' VISIBILE IN GIOCO", SColor.HUD_Reddark, SColor.HUD_Red);
            markerIngressoGarage = new UIMenuItem("Punto di ingresso per il garage", "Il marker è puramente di guida, NON SARA' VISIBILE IN GIOCO", SColor.HUD_Reddark, SColor.HUD_Red);
            markerIngressoTetto = new UIMenuItem("Punto di ingresso dal tetto", "Il marker è puramente di guida, NON SARA' VISIBILE IN GIOCO", SColor.HUD_Reddark, SColor.HUD_Red);
            posCamera = new UIMenuItem("Posizione della telecamera in anteprima", "Imposta la posizione e la rotazione della telecamera quando il cittadino torna a casa o citofona", SColor.HUD_Reddark, SColor.HUD_Red);
            marker.OnItemSelect += async (menu, item, index) =>
            {
                if (item == markerIngressoCasa)
                {
                    markerIngrPiedi = new MarkerEx(dummyMarker.MarkerType, dummyMarker.Position, SColor.Green);

                    if (immobile == TipoImmobile.Casa)
                    {
                        casaDummy.MarkerEntrance = markerIngrPiedi.Position;
                        casaDummy.SpawnOutside = markerIngrPiedi.Position;
                    }
                }
                else if (item == markerIngressoGarage)
                {
                    markerIngrGarage = new MarkerEx(dummyMarker.MarkerType, dummyMarker.Position, SColor.Green);

                    switch (immobile)
                    {
                        case TipoImmobile.Casa:
                            casaDummy.MarkerGarageExtern = markerIngrGarage.Position;
                            casaDummy.SpawnGarageVehicleOutside = markerIngrGarage.Position;

                            break;
                        case TipoImmobile.Garage:
                            garageDummy.MarkerEntrance = markerIngrGarage.Position;
                            garageDummy.SpawnOutside = markerIngrGarage.Position;

                            break;
                    }
                }
                else if (item == markerIngressoTetto)
                {
                    markerIngrTetto = new MarkerEx(dummyMarker.MarkerType, dummyMarker.Position, SColor.Green);

                    if (immobile == TipoImmobile.Casa)
                    {
                        casaDummy.MarkerRoof = markerIngrTetto.Position;
                        casaDummy.SpawnRoof = markerIngrTetto.Position;
                    }
                }
                else if (item == posCamera)
                {
                    CameraPosIngresso = MainCamera.Position.ToPosition();
                    CameraRotIngresso = (await MainCamera.CrosshairRaycast(1000)).HitPosition.ToPosition();

                    switch (immobile)
                    {
                        case TipoImmobile.Casa:
                            casaDummy.CameraOutside.Pos = CameraPosIngresso;
                            casaDummy.CameraOutside.Rotation = CameraRotIngresso;

                            break;
                        case TipoImmobile.Garage:
                            garageDummy.CameraOutside.Pos = CameraPosIngresso;
                            garageDummy.CameraOutside.Rotation = CameraRotIngresso;

                            break;
                    }
                }

                item.MainColor = SColor.HUD_Greendark;
                item.HighlightColor = SColor.HUD_Greenlight;
                item.SetRightBadge(BadgeIcon.STAR);
            };

            #endregion

            #endregion

            #region Gestione interni

            gestioneInteriorCasa.OnMenuOpen += async (a, b) =>
            {
                UIMenuListItem interior = new("", new List<dynamic>() { "" }, 0);
                a.Clear();
                Screen.Fading.FadeOut(800);
                while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
                SetPlayerControl(Cache.PlayerCache.MyPlayer.Player.Handle, false, 256);

                switch (immobile)
                {
                    case TipoImmobile.Casa:
                        {
                            interior = new UIMenuListItem("Interno preferito", new List<dynamic>()
                                {
                                    "Base",
                                    "Medio",
                                    "Alto [HighLife]",
                                    "Alto [2 piani]",
                                    "Alto [3 piani]",
                                    "Alto [Executive]"
                                }, 0);

                            if (includiGarage)
                            {
                                UIMenuListItem garageInterior = new("Tipo di Garage", new List<dynamic>() { "Base", "Medio [4]", "Medio [6]", "Alto [10]" }, 0);
                                gestioneInteriorCasa.AddItem(garageInterior);
                                garageInterior.OnListChanged += (item, index) =>
                                {
                                    switch (index)
                                    {
                                        case 0:
                                            casaDummy.VehCapacity = 2;

                                            break;
                                        case 1:
                                            casaDummy.VehCapacity = 4;

                                            break;
                                        case 2:
                                            casaDummy.VehCapacity = 6;

                                            break;
                                        case 3:
                                            casaDummy.VehCapacity = 10;

                                            break;
                                    }
                                };
                            }

                            break;
                        }
                    case TipoImmobile.Garage:
                        interior = new UIMenuListItem("Interno preferito", new List<dynamic>() { "Base", "Medio [4]", "Medio [6]", "Alto [10]" }, 0);

                        break;
                }

                gestioneInteriorCasa.AddItem(interior);
                if (immobile == TipoImmobile.Casa)
                {
                    UIMenuItem opzioniInteriorItem = new UIMenuItem("Opzioni interno selezionato");
                    opzioniInterior = new("Opzioni interno selezionato", "");
                    gestioneInteriorCasa.AddItem(opzioniInteriorItem);
                    opzioniInteriorItem.Activated += async (a, b) => await gestioneInteriorCasa.SwitchTo(opzioniInterior, 0, true);
                }
                if (MainCamera == null) MainCamera = World.CreateCamera(Cache.PlayerCache.MyPlayer.Posizione.ToVector3 + new Vector3(0, 0, 100), new Vector3(0, 0, 0), 45f);
                MainCamera.IsActive = true;
                RenderScriptCams(true, false, 1000, true, true);
                if (renderCamObject == null) renderCamObject = await Funzioni.SpawnLocalProp(Funzioni.HashInt("prop_ld_test_01"), Vector3.Zero, false, false);
                renderCamObject.IsVisible = false;

                switch (immobile)
                {
                    case TipoImmobile.Casa:
                        SetFocusEntity(renderCamObject.Handle);
                        renderCamObject.Position = new Vector3(266.8514f, -998.9061f, -97.92068f);
                        PlaceObjectOnGroundProperly(renderCamObject.Handle);
                        MainCamera.Position = new Vector3(266.8514f, -998.9061f, -97.92068f);
                        MainCamera.PointAt(new Vector3(259.7751f, -998.6475f, -100.0068f));
                        casaDummy.MarkerExit = new Position(266.094f, -1007.487f, -101.800f);
                        casaDummy.SpawnInside = new Position(266.094f, -1007.487f, -101.800f);

                        break;
                    case TipoImmobile.Garage:
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
                    if (renderCamObject == null) renderCamObject = await Funzioni.SpawnLocalProp(Funzioni.HashInt("prop_ld_test_01"), Vector3.Zero, false, false);
                    renderCamObject.IsVisible = false;
                    if (MainCamera == null) MainCamera = World.CreateCamera(Cache.PlayerCache.MyPlayer.Posizione.ToVector3 + new Vector3(0, 0, 100), new Vector3(0, 0, 0), 45f);
                    MainCamera.IsActive = true;
                    RenderScriptCams(true, false, 1000, true, true);
                    Vector3 pos = Vector3.Zero;
                    Vector3 lookAt = Vector3.Zero;
                    interno = index;

                    switch (immobile)
                    {
                        case TipoImmobile.Casa:
                            casaDummy.Type = index;

                            break;
                        case TipoImmobile.Garage:
                            garageDummy.Type = index;

                            break;
                    }

                    switch (index)
                    {
                        case 0:
                            switch (immobile)
                            {
                                case TipoImmobile.Casa:
                                    pos = new Vector3(266.8514f, -998.9061f, -97.92068f);
                                    lookAt = new Vector3(259.7751f, -998.6475f, -100.0068f);
                                    casaDummy.MarkerExit = new Position(266.094f, -1007.487f, -101.800f);
                                    casaDummy.SpawnInside = new Position(266.094f, -1007.487f, -101.800f);

                                    break;
                                case TipoImmobile.Garage:
                                    pos = new Vector3(177.8964f, -1008.719f, -98.03687f);
                                    lookAt = new Vector3(168.3609f, -1002.193f, -99.99992f);
                                    garageDummy.SpawnInside = new Position(179.015f, -1000.326f, -100f);
                                    garageDummy.MarkerExit = new Position(179.015f, -1000.326f, -100f);

                                    break;
                            }

                            break;
                        case 1:
                            switch (immobile)
                            {
                                case TipoImmobile.Casa:
                                    pos = new Vector3(339.3684f, -992.7239f, -98.21723f);
                                    lookAt = new Vector3(341.4973f, -999.5391f, -100.1962f);
                                    casaDummy.MarkerExit = new Position(346.493f, -1013.031f, -99.196f);
                                    casaDummy.SpawnInside = new Position(346.493f, -1013.031f, -99.196f);

                                    break;
                                case TipoImmobile.Garage:
                                    pos = new Vector3(190.6334f, -1027.276f, -98.94763f);
                                    lookAt = new Vector3(193.8157f, -1024.415f, -99.99996f);
                                    garageDummy.SpawnInside = new Position(207.1461f, -1018.326f, -98.999f);
                                    garageDummy.MarkerExit = new Position(207.1461f, -1018.326f, -98.999f);

                                    break;
                            }

                            break;
                        case 2:
                            switch (immobile)
                            {
                                case TipoImmobile.Casa:
                                    pos = new Vector3(-1465.857f, -535.3416f, 74.20998f);
                                    lookAt = new Vector3(-1467.427f, -544.514f, 72.46823f);
                                    casaDummy.SpawnInside = new Position(-1452.841f, -539.489f, 74.044f);
                                    casaDummy.MarkerExit = new Position(-1452.164f, -540.640f, 74.044f);

                                    break;
                                case TipoImmobile.Garage:
                                    pos = new Vector3(206.7423f, -993.4413f, -98.09858f);
                                    lookAt = new Vector3(190.6937f, -1008.027f, -99.62811f);
                                    garageDummy.SpawnInside = new Position(210.759f, -999.0323f, -98.99997f);
                                    garageDummy.MarkerExit = new Position(210.759f, -999.0323f, -98.99997f);

                                    break;
                            }

                            break;
                        case 3:
                            switch (immobile)
                            {
                                case TipoImmobile.Casa:
                                    pos = new Vector3(-42.78862f, -571.4902f, 89.38699f);
                                    lookAt = new Vector3(-35.83893f, -583.5001f, 88.47382f);
                                    casaDummy.SpawnInside = new Position(-17.54766f, -589.1531f, 90.11485f);
                                    casaDummy.MarkerExit = new Position(-17.54766f, -589.1531f, 90.11485f);

                                    break;
                                case TipoImmobile.Garage:
                                    pos = new Vector3(220.5728f, -1007.01f, -98.10276f);
                                    lookAt = new Vector3(225.9477f, -996.6439f, -99.9992f);
                                    garageDummy.SpawnInside = new Position(238.103f, -1004.813f, -98.99992f);
                                    garageDummy.MarkerExit = new Position(238.103f, -1004.813f, -98.99992f);

                                    break;
                            }

                            break;
                        case 4:
                            if (immobile == TipoImmobile.Casa)
                            {
                                pos = new Vector3(-169.7948f, 478.3921f, 138.4392f);
                                lookAt = new Vector3(-166.9105f, 485.8192f, 136.8266f);
                                casaDummy.SpawnInside = new Position(-173.9128f, 496.8375f, 137.667f);
                                casaDummy.MarkerExit = new Position(-173.9128f, 496.8375f, 137.667f);
                            }

                            break;
                        case 5:
                            if (immobile == TipoImmobile.Casa)
                            {
                                pos = new Vector3(-791.5707f, 343.7827f, 217.8111f);
                                lookAt = new Vector3(-784.6417f, 330.4529f, 216.0382f);
                                casaDummy.SpawnInside = new Position(-786.5125f, 315.8108f, 217.6385f);
                                casaDummy.MarkerExit = new Position(-786.5125f, 315.8108f, 217.6385f);
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

            opzioniInterior.OnMenuOpen += (_new, b) =>
            {
                _new.Clear();

                switch (interno)
                {
                    #region Low

                    case 0: // low
                        if (immobile == TipoImmobile.Casa)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    casaDummy.Strip = check;
                                    IPLs.IPLInstance.GTAOHouseLow1.Strip.Enable(IPLs.IPLInstance.GTAOHouseLow1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Strip.Enable(IPLs.IPLInstance.GTAOHouseLow1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Strip.Enable(IPLs.IPLInstance.GTAOHouseLow1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    casaDummy.Booze = check;
                                    IPLs.IPLInstance.GTAOHouseLow1.Booze.Enable(IPLs.IPLInstance.GTAOHouseLow1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Booze.Enable(IPLs.IPLInstance.GTAOHouseLow1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseLow1.Booze.Enable(IPLs.IPLInstance.GTAOHouseLow1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    casaDummy.Smoke = check;
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
                        if (immobile == TipoImmobile.Casa)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    casaDummy.Strip = check;
                                    IPLs.IPLInstance.GTAOHouseMid1.Strip.Enable(IPLs.IPLInstance.GTAOHouseMid1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Strip.Enable(IPLs.IPLInstance.GTAOHouseMid1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Strip.Enable(IPLs.IPLInstance.GTAOHouseMid1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    casaDummy.Booze = check;
                                    IPLs.IPLInstance.GTAOHouseMid1.Booze.Enable(IPLs.IPLInstance.GTAOHouseMid1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Booze.Enable(IPLs.IPLInstance.GTAOHouseMid1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseMid1.Booze.Enable(IPLs.IPLInstance.GTAOHouseMid1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    casaDummy.Smoke = check;
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
                        if (immobile == TipoImmobile.Casa)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    casaDummy.Strip = check;
                                    IPLs.IPLInstance.HLApartment1.Strip.Enable(IPLs.IPLInstance.HLApartment1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.HLApartment1.Strip.Enable(IPLs.IPLInstance.HLApartment1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.HLApartment1.Strip.Enable(IPLs.IPLInstance.HLApartment1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    casaDummy.Booze = check;
                                    IPLs.IPLInstance.HLApartment1.Booze.Enable(IPLs.IPLInstance.HLApartment1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.HLApartment1.Booze.Enable(IPLs.IPLInstance.HLApartment1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.HLApartment1.Booze.Enable(IPLs.IPLInstance.HLApartment1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    casaDummy.Smoke = check;
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
                        if (immobile == TipoImmobile.Casa)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    casaDummy.Strip = check;
                                    IPLs.IPLInstance.GTAOApartmentHi1.Strip.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Strip.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Strip.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    casaDummy.Booze = check;
                                    IPLs.IPLInstance.GTAOApartmentHi1.Booze.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Booze.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOApartmentHi1.Booze.Enable(IPLs.IPLInstance.GTAOApartmentHi1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    casaDummy.Smoke = check;
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
                        if (immobile == TipoImmobile.Casa)
                        {
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            _new.OnCheckboxChange += async (menu, item, check) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);

                                if (item == strip)
                                {
                                    casaDummy.Strip = check;
                                    IPLs.IPLInstance.GTAOHouseHi1.Strip.Enable(IPLs.IPLInstance.GTAOHouseHi1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Strip.Enable(IPLs.IPLInstance.GTAOHouseHi1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Strip.Enable(IPLs.IPLInstance.GTAOHouseHi1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    casaDummy.Booze = check;
                                    IPLs.IPLInstance.GTAOHouseHi1.Booze.Enable(IPLs.IPLInstance.GTAOHouseHi1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Booze.Enable(IPLs.IPLInstance.GTAOHouseHi1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.GTAOHouseHi1.Booze.Enable(IPLs.IPLInstance.GTAOHouseHi1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    casaDummy.Smoke = check;
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
                        if (immobile == TipoImmobile.Casa)
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
                                    }, casaDummy.Style);                                                  // cambiare index
                            UIMenuCheckboxItem strip = new("Biancheria sparsa", casaDummy.Strip); // cambiare checked
                            UIMenuCheckboxItem booze = new("Bottiglie sparse", casaDummy.Booze);  // cambiare checked
                            UIMenuCheckboxItem smoke = new("Posacenere sparsi", casaDummy.Smoke); // cambiare checked
                            _new.AddItem(tema);
                            _new.AddItem(strip);
                            _new.AddItem(booze);
                            _new.AddItem(smoke);
                            tema.OnListChanged += async (item, index) =>
                            {
                                Screen.Fading.FadeOut(250);
                                await BaseScript.Delay(300);
                                idx = index;
                                casaDummy.Style = idx;

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
                                    casaDummy.Strip = check;
                                    IPLs.IPLInstance.ExecApartment1.Strip.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Strip.Stage1, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Strip.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Strip.Stage2, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Strip.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Strip.Stage3, check, true);
                                }
                                else if (item == booze)
                                {
                                    casaDummy.Booze = check;
                                    IPLs.IPLInstance.ExecApartment1.Booze.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Booze.Stage1, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Booze.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Booze.Stage2, check, true);
                                    IPLs.IPLInstance.ExecApartment1.Booze.Enable(IPLs.IPLInstance.ExecApartment1.CurrentInteriorId, IPLs.IPLInstance.ExecApartment1.Booze.Stage3, check, true);
                                }
                                else if (item == smoke)
                                {
                                    casaDummy.Smoke = check;
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

            creazione.OnMenuOpen += (a, b) =>
            {
                oldInstance = Cache.PlayerCache.MyPlayer.Status.Instance;
                Cache.PlayerCache.MyPlayer.Status.Instance.Istanzia("Creatore Immobiliare");
            };
            creazione.OnMenuClose += (a) =>
            {
                if (Cache.PlayerCache.MyPlayer.Status.Instance.Instance == "Creatore Immobiliare") Cache.PlayerCache.MyPlayer.Status.Instance.RimuoviIstanza();
                Cache.PlayerCache.MyPlayer.Status.Instance = oldInstance;
            };
            selezionePunto.OnMenuOpen += async (a, b) =>
            {
                SetPlayerControl(Cache.PlayerCache.MyPlayer.Player.Handle, false, 256);
                Screen.Fading.FadeOut(800);
                while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
                if (MainCamera == null) MainCamera = World.CreateCamera(Vector3.Zero, new Vector3(0, 0, 0), 45f);
                MainCamera.Position = Cache.PlayerCache.MyPlayer.Posizione.ToVector3 + new Vector3(0, 0, 100);
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

                switch (immobile)
                {
                    case TipoImmobile.Casa:
                        marker.AddItem(markerIngressoCasa);
                        if (includiGarage) marker.AddItem(markerIngressoGarage);
                        if (includiTetto) marker.AddItem(markerIngressoTetto);
                        marker.AddItem(posCamera);

                        break;
                    case TipoImmobile.Garage:
                        marker.AddItem(markerIngressoGarage);
                        marker.AddItem(posCamera);

                        break;
                }

                Client.Instance.AddTick(MarkerTick);
                if (markerIngrPiedi == null) markerIngrPiedi = new MarkerEx(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.Red);
                if (markerIngrGarage == null) markerIngrGarage = new MarkerEx(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.Red);
                if (markerIngrTetto == null) markerIngrTetto = new MarkerEx(MarkerType.VerticalCylinder, Position.Zero, new Vector3(1.5f), SColor.Red);
            };

            marker.OnMenuClose += (a) => Client.Instance.RemoveTick(MarkerTick);

            selezionePunto.OnMenuClose += async (a) =>
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

            gestioneInteriorCasa.OnMenuClose += async (a) =>
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

            UIMenuItem Salva = new("Salva immobile", "Attenzione la cosa non sarà reversibile se non contattando un ADMIN!");
            creazione.AddItem(Salva);
            Salva.Activated += async (menu, item) =>
            {
                switch (immobile)
                {
                    case TipoImmobile.Casa when !string.IsNullOrWhiteSpace(casaDummy.Label):
                        {
                            if (!string.IsNullOrWhiteSpace(abbreviazione))
                            {
                                if (casaDummy.Price > 0)
                                {
                                    if (casaDummy.MarkerEntrance != Position.Zero)
                                    {
                                        if (casaDummy.CameraOutside.Pos != Position.Zero && casaDummy.CameraOutside.Rotation != Position.Zero)
                                        {
                                            if (casaDummy.GarageIncluded)
                                            {
                                                if (casaDummy.MarkerGarageExtern != Position.Zero && casaDummy.SpawnGarageVehicleOutside != Position.Zero)
                                                {
                                                    if (casaDummy.HasRoof)
                                                    {
                                                        if (casaDummy.MarkerRoof != Position.Zero)
                                                        {
                                                            BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "casa", casaDummy.ToJson(), abbreviazione);
                                                            MenuHandler.CloseAndClearHistory();
                                                        }
                                                        else
                                                        {
                                                            HUD.ShowNotification("Hai incluso il tetto ma manca il marker del tetto!", ColoreNotifica.Red, true);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // non tetto incluso
                                                        BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "casa", casaDummy.ToJson(), abbreviazione);
                                                        MenuHandler.CloseAndClearHistory();
                                                    }
                                                }
                                                else
                                                {
                                                    HUD.ShowNotification("Hai incluso il garage ma mancano i marker del garage!", ColoreNotifica.Red, true);
                                                }
                                            }
                                            else // non garage incluso
                                            {
                                                if (casaDummy.HasRoof)
                                                {
                                                    if (casaDummy.MarkerRoof != Position.Zero)
                                                    {
                                                        BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "casa", casaDummy.ToJson(), abbreviazione);
                                                        Client.Impostazioni.RolePlay.Properties.Apartments.Add(abbreviazione, casaDummy);
                                                        MenuHandler.CloseAndClearHistory();
                                                    }
                                                    else
                                                    {
                                                        HUD.ShowNotification("Hai incluso il tetto ma manca il marker del tetto!", ColoreNotifica.Red, true);
                                                    }
                                                }
                                                else
                                                {
                                                    // non tetto incluso
                                                    BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "casa", casaDummy.ToJson(), abbreviazione);
                                                    Client.Impostazioni.RolePlay.Properties.Apartments.Add(abbreviazione, casaDummy);
                                                    MenuHandler.CloseAndClearHistory();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            HUD.ShowNotification("Non hai settato la telecamera d'ingresso!", ColoreNotifica.Red, true);
                                        }
                                    }
                                    else
                                    {
                                        HUD.ShowNotification("Non hai impostato il marker d'entrata!", ColoreNotifica.Red, true);
                                    }
                                }
                                else
                                {
                                    HUD.ShowNotification("Non hai inserito un prezzo!", ColoreNotifica.Red, true);
                                }
                            }
                            else
                            {
                                HUD.ShowNotification("Non hai inserito l'abbreviazione!", ColoreNotifica.Red, true);
                            }

                            break;
                        }
                    case TipoImmobile.Casa:
                        HUD.ShowNotification("Non hai specificato il nome dell'immobile!", ColoreNotifica.Red, true);

                        break;
                    case TipoImmobile.Garage when !string.IsNullOrWhiteSpace(garageDummy.Label):
                        {
                            if (!string.IsNullOrWhiteSpace(abbreviazione))
                            {
                                if (garageDummy.Price > 0)
                                {
                                    if (garageDummy.MarkerEntrance != Position.Zero)
                                    {
                                        if (garageDummy.CameraOutside.Pos != Position.Zero && garageDummy.CameraOutside.Rotation != Position.Zero)
                                        {
                                            BaseScript.TriggerServerEvent("lprp:agenteimmobiliare:salvaAppartamento", "garage", garageDummy.ToJson(), abbreviazione);
                                            Client.Impostazioni.RolePlay.Properties.Garages.Garages.Add(abbreviazione, garageDummy);
                                            MenuHandler.CloseAndClearHistory();
                                        }
                                        else
                                        {
                                            HUD.ShowNotification("Non hai settato la telecamera d'ingresso!", ColoreNotifica.Red, true);
                                        }
                                    }
                                    else
                                    {
                                        HUD.ShowNotification("Non hai impostato il marker d'entrata!", ColoreNotifica.Red, true);
                                    }
                                }
                                else
                                {
                                    HUD.ShowNotification("Non hai inserito un prezzo!", ColoreNotifica.Red, true);
                                }
                            }
                            else
                            {
                                HUD.ShowNotification("Non hai inserito l'abbreviazione!", ColoreNotifica.Red, true);
                            }

                            break;
                        }
                    case TipoImmobile.Garage:
                        HUD.ShowNotification("Non hai specificato il nome dell'immobile!", ColoreNotifica.Red, true);

                        break;
                }
            };
            creazione.Visible = true;
        }

        private static async Task CreatorCameraControl()
        {
            float forwardPush = 0.8f;
            if (GetGameTimer() - checkTimer > (int)Math.Ceiling(1000 / forwardPush)) curLocation.SetFocus();
            string tast = $"~INPUTGROUP_MOVE~ per muoverti.\n~INPUTGROUP_LOOK~ per girare la telecamera.\n~INPUT_COVER~ ~INPUT_VEH_HORN~ per salire e scendere.\n~INPUT_FRONTEND_X~ per cambiare velocità.\nVelocità attuale: ~y~{travelSpeedStr}~w~.";
            string gampa = $"~INPUTGROUP_MOVE~ per muoverti.\n~INPUTGROUP_LOOK~ per girare la telecamera.\n~INPUTGROUP_FRONTEND_TRIGGERS~ per salire e scendere.\n~INPUT_FRONTEND_X~ per cambiare velocità.\nVelocità attuale: ~y~{travelSpeedStr}~w~.";
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
                    travelSpeedStr = "Media";

                    break;
                case 1:
                    forwardPush = 1.8f; //fast
                    travelSpeedStr = "Veloce";

                    break;
                case 2:
                    forwardPush = 2.6f; //very fast
                    travelSpeedStr = "Molto veloce";

                    break;
                case 3:
                    forwardPush = 0.025f; //very slow
                    travelSpeedStr = "Estremamente lenta";

                    break;
                case 4:
                    forwardPush = 0.05f; //very slow
                    travelSpeedStr = "Molto lenta";

                    break;
                case 5:
                    forwardPush = 0.2f; //slow
                    travelSpeedStr = "Lenta";

                    break;
            }

            float zVect = forwardPush / 3;

            if (Input.IsDisabledControlJustPressed(Control.FrontendX))
            {
                travelSpeed++;
                if (travelSpeed > 5) travelSpeed = 0;
            }

            float xVectFwd = forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z)) * -1.0f;
            float yVectFwd = forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
            float xVectLat = forwardPush * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
            float yVectLat = forwardPush * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z));
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

            /* Ci serve davvero ruotare la telecamera? :thinking:
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
            if (markerIngressoCasa.Selected)
                if (markerIngressoCasa.RightBadge != BadgeIcon.NONE)
                    markerIngrPiedi.Draw();
            if (markerIngressoGarage.Selected)
                if (markerIngressoGarage.RightBadge != BadgeIcon.NONE)
                    markerIngrGarage.Draw();
            if (markerIngressoTetto.Selected)
                if (markerIngressoTetto.RightBadge != BadgeIcon.NONE)
                    markerIngrTetto.Draw();
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