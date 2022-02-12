using Impostazioni.Client.Configurazione.Negozi.Barbieri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Negozi
{
    internal static class BarberClient
    {
        private static Ped CurrentBarber = new Ped(0);
        private static Vector3 S1 = new Vector3(0);
        private static Vector3 S2 = new Vector3(0);
        private static Vector3 S3 = new Vector3(0);
        private static Camera Camm = new Camera(0);
        private static bool CreatoKuts = false;
        private static bool CreatoHawick = false;
        private static bool CreatoOsheas = false;
        private static bool CreatoCombo = false;

        public static void Init()
        {
            Client.Instance.AddEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));
        }

        public static void Stop()
        {
            Client.Instance.RemoveEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));
        }

        public static async void Spawnato()
        {
            foreach (NegozioBarbiere barbiere in ConfigBarbieri.Kuts)
            {
                Blip kuts = World.CreateBlip(barbiere.Coord);
                kuts.Sprite = (BlipSprite)71;
                kuts.Color = (BlipColor)12;
                kuts.IsShortRange = true;
                kuts.Name = "Herr Kuts";
            }

            Blip Hawick = new Blip(AddBlipForCoord(ConfigBarbieri.Hawick.Coord.X, ConfigBarbieri.Hawick.Coord.Y, ConfigBarbieri.Hawick.Coord.Z)) { Sprite = (BlipSprite)71, Color = (BlipColor)17, IsShortRange = true, Name = "Barbieri Hair On Hawick" };
            Blip Combo = new Blip(AddBlipForCoord(ConfigBarbieri.Combo.Coord.X, ConfigBarbieri.Combo.Coord.Y, ConfigBarbieri.Combo.Coord.Z)) { Sprite = (BlipSprite)71, Color = (BlipColor)66, IsShortRange = true, Name = "Barbieri Beachcombover" };
            Blip Osheas = new Blip(AddBlipForCoord(ConfigBarbieri.Osheas.Coord.X, ConfigBarbieri.Osheas.Coord.Y, ConfigBarbieri.Osheas.Coord.Z)) { Sprite = (BlipSprite)71, Color = (BlipColor)38, IsShortRange = true, Name = "Barbieri Oshea's" };
        }

        public static async Task<Ped> CreateBarber(BarberModel ped)
        {
            Model model = new Model(ped.Model);
            model.Request();
            while (!model.IsLoaded) await BaseScript.Delay(1);
            Ped barber = new Ped(CreatePed(4, (uint)model.Hash, ped.Coords.X, ped.Coords.Y, ped.Coords.Z, ped.Coords.W, false, false));
            barber.BlockPermanentEvents = true;
            barber.Voice = ped.Voice;
            barber.Task.StartScenario("WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT", barber.Position);
            model.MarkAsNoLongerNeeded();
            SetPedCanPlayAmbientAnims(barber.Handle, true);

            return barber;
        }

        public static async void Controlla(Vector3 S, float Ch, Vector3 C, string Menu)
        {
            while (!Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(S, 1f)) await BaseScript.Delay(100);

            if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(S, 1f) && IsPedUsingScenario(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER"))
            {
                HUD.ShowHelp("Ricorda che puoi anche usare il ~b~MOUSE~w~ per selezionare i colori e l'opacità.");
                ShowCam(S, Ch, C);
                BarberMenu(Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio" ? Client.Impostazioni.RolePlay.Negozi.Barbieri.Maschio : Client.Impostazioni.RolePlay.Negozi.Barbieri.Femmina, Menu);
            }
        }

        public static async void ShowCam(Vector3 C, float Ch, Vector3 O)
        {
            Vector3 Coords = GetObjectOffsetFromCoords(C.X, C.Y, C.Z, Ch, O.X, O.Y, O.Z);
            Camm = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));
            Camm.Position = new Vector3(Coords.X, Coords.Y, Coords.Z + 0.85f);
            Camm.PointAt(Cache.PlayerCache.MyPlayer.Ped.Bones[Bone.SKEL_Head].Position);
            Camm.FieldOfView = 35f;
            Camm.IsActive = true;
            RenderScriptCams(true, true, 1000, true, false);
        }

        public static async Task Sedie()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            foreach (NegozioBarbiere t in ConfigBarbieri.Kuts)
                if (p.IsInRangeOf(t.Coord, 50f) && !CreatoKuts)
                {
                    S1 = t.Sedia1;
                    S2 = t.Sedia2;
                    S3 = t.Sedia3;
                    CurrentBarber = await CreateBarber(t.Model);
                    CreatoKuts = true;
                }
                else if (!p.IsInRangeOf(t.Coord, 50))
                {
                    if (!CreatoKuts) continue;
                    CurrentBarber.Delete();
                    CreatoKuts = false;
                }
                else if (p.IsInRangeOf(CurrentBarber.Position, 2f))
                {
                    HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per andare a sederti al salone");

                    if (!Input.IsControlJustPressed(Control.Context)) continue;
                    TaskSetBlockingOfNonTemporaryEvents(PlayerPedId(), true);

                    if (!IsAnyPedNearPoint(S1.X, S1.Y, S1.Z, 0.35f))
                    {
                        TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", S1.X, S1.Y, S1.Z, t.Heading, 0, true, false);
                        Controlla(S1, t.Cam, t.CXYZ, "Kuts");
                    }
                    else
                    {
                        if (!IsAnyPedNearPoint(S2.X, S2.Y, S2.Z, 0.35f))
                        {
                            TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", S2.X, S2.Y, S2.Z, t.Heading, 0, true, false);
                            Controlla(S2, t.Cam, t.CXYZ, "Kuts");
                        }
                        else
                        {
                            if (!IsAnyPedNearPoint(S3.X, S3.Y, S3.Z, 0.35f))
                            {
                                TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", S3.X, S3.Y, S3.Z, t.Heading, 0, true, false);
                                Controlla(S3, t.Cam, t.CXYZ, "Kuts");
                            }
                            else
                            {
                                HUD.ShowNotification("Attendi... Non ci sono posti liberi.", true);
                            }
                        }
                    }
                }

            if (p.IsInRangeOf(ConfigBarbieri.Hawick.Coord, 50) && !CreatoHawick)
            {
                CurrentBarber = await CreateBarber(ConfigBarbieri.Hawick.Model);
                CreatoHawick = true;
            }
            else if (!p.IsInRangeOf(ConfigBarbieri.Hawick.Coord, 50))
            {
                if (CreatoHawick)
                {
                    CurrentBarber.Delete();
                    CreatoHawick = false;
                }
            }
            else if (p.IsInRangeOf(CurrentBarber.Position, 2))
            {
                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per andare a sederti al salone");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    TaskSetBlockingOfNonTemporaryEvents(PlayerPedId(), true);

                    if (!IsAnyPedNearPoint(ConfigBarbieri.Hawick.Sedia1.X, ConfigBarbieri.Hawick.Sedia1.Y, ConfigBarbieri.Hawick.Sedia1.Z, 0.35f))
                    {
                        TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Hawick.Sedia1.X, ConfigBarbieri.Hawick.Sedia1.Y, ConfigBarbieri.Hawick.Sedia1.Z, ConfigBarbieri.Hawick.Heading, 0, true, false);
                        Controlla(ConfigBarbieri.Hawick.Sedia1, ConfigBarbieri.Hawick.Cam, ConfigBarbieri.Hawick.CXYZ, "Hawick");
                    }
                    else
                    {
                        if (!IsAnyPedNearPoint(ConfigBarbieri.Hawick.Sedia2.X, ConfigBarbieri.Hawick.Sedia2.Y, ConfigBarbieri.Hawick.Sedia2.Z, 0.35f))
                        {
                            TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Hawick.Sedia2.X, ConfigBarbieri.Hawick.Sedia2.Y, ConfigBarbieri.Hawick.Sedia2.Z, ConfigBarbieri.Hawick.Heading, 0, true, false);
                            Controlla(ConfigBarbieri.Hawick.Sedia2, ConfigBarbieri.Hawick.Cam, ConfigBarbieri.Hawick.CXYZ, "Hawick");
                        }
                        else
                        {
                            if (!IsAnyPedNearPoint(ConfigBarbieri.Hawick.Sedia3.X, ConfigBarbieri.Hawick.Sedia3.Y, ConfigBarbieri.Hawick.Sedia3.Z, 0.35f))
                            {
                                TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Hawick.Sedia3.X, ConfigBarbieri.Hawick.Sedia3.Y, ConfigBarbieri.Hawick.Sedia3.Z, ConfigBarbieri.Hawick.Heading, 0, true, false);
                                Controlla(ConfigBarbieri.Hawick.Sedia3, ConfigBarbieri.Hawick.Cam, ConfigBarbieri.Hawick.CXYZ, "Hawick");
                            }
                            else
                            {
                                HUD.ShowNotification("Attendi... Non ci sono posti liberi.", true);
                            }
                        }
                    }
                }
            }

            if (p.IsInRangeOf(ConfigBarbieri.Osheas.Coord, 50) && !CreatoOsheas)
            {
                CurrentBarber = await CreateBarber(ConfigBarbieri.Osheas.Model);
                CreatoOsheas = true;
            }
            else if (!p.IsInRangeOf(ConfigBarbieri.Osheas.Coord, 50))
            {
                if (CreatoOsheas)
                {
                    CurrentBarber.Delete();
                    CreatoOsheas = false;
                }
            }
            else if (p.IsInRangeOf(CurrentBarber.Position, 2))
            {
                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per andare a sederti al salone");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    TaskSetBlockingOfNonTemporaryEvents(PlayerPedId(), true);

                    if (!IsAnyPedNearPoint(ConfigBarbieri.Osheas.Sedia1.X, ConfigBarbieri.Osheas.Sedia1.Y, ConfigBarbieri.Osheas.Sedia1.Z, 0.35f))
                    {
                        TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Osheas.Sedia1.X, ConfigBarbieri.Osheas.Sedia1.Y, ConfigBarbieri.Osheas.Sedia1.Z, ConfigBarbieri.Osheas.Heading, 0, true, false);
                        Controlla(ConfigBarbieri.Osheas.Sedia1, ConfigBarbieri.Osheas.Cam, ConfigBarbieri.Osheas.CXYZ, "Osheas");
                    }
                    else
                    {
                        if (!IsAnyPedNearPoint(ConfigBarbieri.Osheas.Sedia2.X, ConfigBarbieri.Osheas.Sedia2.Y, ConfigBarbieri.Osheas.Sedia2.Z, 0.35f))
                        {
                            TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Osheas.Sedia2.X, ConfigBarbieri.Osheas.Sedia2.Y, ConfigBarbieri.Osheas.Sedia2.Z, ConfigBarbieri.Osheas.Heading, 0, true, false);
                            Controlla(ConfigBarbieri.Osheas.Sedia2, ConfigBarbieri.Osheas.Cam, ConfigBarbieri.Osheas.CXYZ, "Osheas");
                        }
                        else
                        {
                            if (!IsAnyPedNearPoint(ConfigBarbieri.Osheas.Sedia3.X, ConfigBarbieri.Osheas.Sedia3.Y, ConfigBarbieri.Osheas.Sedia3.Z, 0.35f))
                            {
                                TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Osheas.Sedia3.X, ConfigBarbieri.Osheas.Sedia3.Y, ConfigBarbieri.Osheas.Sedia3.Z, ConfigBarbieri.Osheas.Heading, 0, true, false);
                                Controlla(ConfigBarbieri.Osheas.Sedia3, ConfigBarbieri.Osheas.Cam, ConfigBarbieri.Osheas.CXYZ, "Osheas");
                            }
                            else
                            {
                                HUD.ShowNotification("Attendi... Non ci sono posti liberi.", true);
                            }
                        }
                    }
                }
            }

            if (p.IsInRangeOf(ConfigBarbieri.Combo.Coord, 50) && !CreatoCombo)
            {
                CurrentBarber = await CreateBarber(ConfigBarbieri.Combo.Model);
                CreatoCombo = true;
            }
            else if (!p.IsInRangeOf(ConfigBarbieri.Combo.Coord, 50))
            {
                if (CreatoCombo)
                {
                    CurrentBarber.Delete();
                    CreatoCombo = false;
                }
            }
            else if (p.IsInRangeOf(CurrentBarber.Position, 2))
            {
                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per andare a sederti al salone");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    TaskSetBlockingOfNonTemporaryEvents(PlayerPedId(), true);

                    if (!IsAnyPedNearPoint(ConfigBarbieri.Combo.Sedia1.X, ConfigBarbieri.Combo.Sedia1.Y, ConfigBarbieri.Combo.Sedia1.Z, 0.35f))
                    {
                        TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Combo.Sedia1.X, ConfigBarbieri.Combo.Sedia1.Y, ConfigBarbieri.Combo.Sedia1.Z, ConfigBarbieri.Combo.Heading, 0, true, false);
                        Controlla(ConfigBarbieri.Combo.Sedia1, ConfigBarbieri.Combo.Cam, ConfigBarbieri.Combo.CXYZ, "Combo");
                    }
                    else
                    {
                        if (!IsAnyPedNearPoint(ConfigBarbieri.Combo.Sedia2.X, ConfigBarbieri.Combo.Sedia2.Y, ConfigBarbieri.Combo.Sedia2.Z, 0.35f))
                        {
                            TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Combo.Sedia2.X, ConfigBarbieri.Combo.Sedia2.Y, ConfigBarbieri.Combo.Sedia2.Z, ConfigBarbieri.Combo.Heading, 0, true, false);
                            Controlla(ConfigBarbieri.Combo.Sedia2, ConfigBarbieri.Combo.Cam, ConfigBarbieri.Combo.CXYZ, "Combo");
                        }
                        else
                        {
                            if (!IsAnyPedNearPoint(ConfigBarbieri.Combo.Sedia3.X, ConfigBarbieri.Combo.Sedia3.Y, ConfigBarbieri.Combo.Sedia3.Z, 0.35f))
                            {
                                TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Combo.Sedia3.X, ConfigBarbieri.Combo.Sedia3.Y, ConfigBarbieri.Combo.Sedia3.Z, ConfigBarbieri.Combo.Heading, 0, true, false);
                                Controlla(ConfigBarbieri.Combo.Sedia3, ConfigBarbieri.Combo.Cam, ConfigBarbieri.Combo.CXYZ, "Combo");
                            }
                            else
                            {
                                HUD.ShowNotification("Attendi... Non ci sono posti liberi.", true);
                            }
                        }
                    }
                }
            }
        }

        private static UIMenu MenuPrincipale = new UIMenu("", "");

        private static async void BarberMenu(BarbieriTesta Tipo, string NomeNegozio)
        {
            #region DICHIARAZIONE

            System.Drawing.Point pos = new System.Drawing.Point(50, 100);
            Skin skin = Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin;
            int capAttuali = skin.hair.style;
            int colAttuale1 = skin.hair.color[0];
            int colAttuale2 = skin.hair.color[1];
            int rossAttuali = skin.lipstick.style;
            int rossAttualiC = skin.lipstick.color[0];
            int rossAttualiC2 = skin.lipstick.color[1];
            float rossAttualiO = skin.lipstick.opacity;
            int trcAtt = skin.makeup.style;
            float trcOpAtt = skin.makeup.opacity;
            int soprAtt = skin.facialHair.eyebrow.style;
            int soprCAtt = skin.facialHair.eyebrow.color[0];
            int soprC1Att = skin.facialHair.eyebrow.color[1];
            float soprOpAtt = skin.facialHair.eyebrow.opacity;
            int brbAtt = skin.facialHair.beard.style;
            int brbAttC1 = skin.facialHair.beard.color[0];
            int brbAttC2 = skin.facialHair.beard.color[1];
            float brbAttOp = skin.facialHair.beard.opacity;
            string desc = "";
            string title = "";
            desc = skin.sex == "Maschio" ? "Ma davvero?!?!" : "Scegli il trucco perfetto!";

            switch (NomeNegozio)
            {
                case "Kuts":
                    title = "Benvenuto da Herr Kuts!";

                    break;
                case "Hawick":
                    title = "Benvenuto da Hair On Hawick!";

                    break;
                case "Osheas":
                    title = "Benvenuto da O'Sheas!";

                    break;
                case "Combo":
                    title = "Benvenuto da Beachcombover!";

                    break;
                case "Mulet":
                    title = "Benvenuto da Bob Mulet!";

                    break;
            }

            List<dynamic> barbe = Tipo.barba.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> trucco = Tipo.trucco.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> sopr = Tipo.sopr.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> ross = Tipo.ross.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> capkuts = Tipo.capelli.kuts.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> caphawick = Tipo.capelli.hawick.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> caposheas = Tipo.capelli.osheas.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> capcombo = Tipo.capelli.beach.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> capmulet = Tipo.capelli.mulet.Select(b => b.Name).Cast<dynamic>().ToList();
            MenuPrincipale = new UIMenu("", title, pos, Main.Textures[NomeNegozio].Key, Main.Textures[NomeNegozio].Value);
            HUD.MenuPool.Add(MenuPrincipale);
            MenuPrincipale.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            UIMenuListItem Capelli = new UIMenuListItem("Capelli", new List<dynamic>() { 0 }, 0);

            switch (NomeNegozio)
            {
                case "Kuts":
                    Capelli = new UIMenuListItem("Capelli", capkuts, 0);

                    break;
                case "Hawick":
                    Capelli = new UIMenuListItem("Capelli", caphawick, 0);

                    break;
                case "Osheas":
                    Capelli = new UIMenuListItem("Capelli", caposheas, 0);

                    break;
                case "Combo":
                    Capelli = new UIMenuListItem("Capelli", capcombo, 0);

                    break;
                case "Mulet":
                    Capelli = new UIMenuListItem("Capelli", capmulet, 0);

                    break;
            }

            UIMenuColorPanel capCol1 = new UIMenuColorPanel("Colore Base", ColorPanelType.Hair);
            UIMenuColorPanel capCol2 = new UIMenuColorPanel("Colore Secondario", ColorPanelType.Hair);
            Capelli.AddPanel(capCol1);
            Capelli.AddPanel(capCol2);
            capCol1.Enabled = false;
            capCol2.Enabled = false;
            capCol1.CurrentSelection = colAttuale1;
            capCol2.CurrentSelection = colAttuale2;
            MenuPrincipale.AddItem(Capelli);
            int colorecap1 = 0;
            int colorecap2 = 0;
            int capvar;
            UIMenuListItem Sopracciglia = new UIMenuListItem("Sopracciglia", sopr, 0);
            UIMenuColorPanel soprBase = new UIMenuColorPanel("Colore Base", ColorPanelType.Hair);
            UIMenuColorPanel soprSec = new UIMenuColorPanel("Colore Secondario", ColorPanelType.Hair);
            UIMenuPercentagePanel soprOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
            Sopracciglia.AddPanel(soprBase);
            Sopracciglia.AddPanel(soprSec);
            Sopracciglia.AddPanel(soprOp);
            soprBase.Enabled = false;
            soprSec.Enabled = false;
            soprOp.Enabled = false;
            soprBase.CurrentSelection = soprCAtt;
            soprSec.CurrentSelection = soprC1Att;
            soprOp.Percentage = soprOpAtt;
            MenuPrincipale.AddItem(Sopracciglia);
            float soprop;
            int soprcol1;
            int soprcol2;
            int soprvar;
            UIMenuListItem Barba = null;
            UIMenuColorPanel beardBase = null;
            UIMenuColorPanel beardSec = null;
            UIMenuPercentagePanel beardOp = null;
            float brbop;
            int brbcol1;
            int brbcol2;
            int brbvar;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
            {
                Barba = new UIMenuListItem("Seleziona Barba", barbe, 0);
                beardBase = new UIMenuColorPanel("Colore Base", ColorPanelType.Hair);
                beardSec = new UIMenuColorPanel("Colore Secondario", ColorPanelType.Hair);
                beardOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
                beardOp.Percentage = 1.0f;
                Barba.AddPanel(beardBase);
                Barba.AddPanel(beardSec);
                Barba.AddPanel(beardOp);
                if (skin.sex == "Maschio") MenuPrincipale.AddItem(Barba);
                beardOp.Enabled = false;
                beardBase.Enabled = false;
                beardSec.Enabled = false;
                beardOp.Percentage = brbAttOp;
                beardBase.CurrentSelection = brbAttC1;
                beardSec.CurrentSelection = brbAttC2;
            }

            UIMenuListItem Trucco = new UIMenuListItem("Trucco", trucco, 0, desc);
            UIMenuPercentagePanel trOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
            Trucco.AddPanel(trOp);
            trOp.Enabled = false;
            trOp.Percentage = trcOpAtt;
            int tru = 0;
            MenuPrincipale.AddItem(Trucco);
            UIMenuListItem Rossetto = new UIMenuListItem("Rossetto", ross, 0, desc);
            UIMenuColorPanel rossColBase = new UIMenuColorPanel("Colore Base", ColorPanelType.Makeup);
            UIMenuColorPanel rossColSec = new UIMenuColorPanel("Colore Secondario", ColorPanelType.Makeup);
            UIMenuPercentagePanel rossOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
            Rossetto.AddPanel(rossColBase);
            Rossetto.AddPanel(rossColSec);
            Rossetto.AddPanel(rossOp);
            rossColBase.Enabled = false;
            rossColSec.Enabled = false;
            rossOp.Enabled = false;
            rossColBase.CurrentSelection = rossAttualiC;
            rossColSec.CurrentSelection = rossAttualiC2;
            rossOp.Percentage = rossAttualiO;
            int rosset = 0;
            MenuPrincipale.AddItem(Rossetto);

            //UIMenuListItem Fard = new UIMenuListItem("Fard", trucco, 0, desc);

            #endregion

            #region CORPO MENU

            #region ON_LIST_CHANGED

            MenuPrincipale.OnListChange += (menu, item, index) =>
            {
                if (item == Capelli)
                {
                    List<Capigliature> obj = new List<Capigliature>();

                    switch (NomeNegozio)
                    {
                        case "Kuts":
                            obj = Tipo.capelli.kuts;

                            break;
                        case "Hawick":
                            obj = Tipo.capelli.hawick;

                            break;
                        case "Osheas":
                            obj = Tipo.capelli.osheas;

                            break;
                        case "Combo":
                            obj = Tipo.capelli.beach;

                            break;
                        case "Mulet":
                            obj = Tipo.capelli.mulet;

                            break;
                    }

                    if (obj[index].var == 0)
                    {
                        capCol1.Enabled = false;
                        capCol2.Enabled = false;
                    }
                    else
                    {
                        capCol1.Enabled = true;
                        capCol2.Enabled = true;
                    }

                    if (capAttuali == obj[index].var)
                        item.Description = "Stile ~b~Attuale~w~!";
                    else
                        item.Description = obj[index].Description + " - Prezzo: ~g~$" + obj[index].price;
                    colorecap1 = (capCol1 as UIMenuColorPanel).CurrentSelection;
                    colorecap2 = (capCol2 as UIMenuColorPanel).CurrentSelection;
                    capvar = obj[index].var;
                    SetPedComponentVariation(PlayerPedId(), 2, obj[index].var, 0, 2);
                    SetPedHairColor(PlayerPedId(), colorecap1, colorecap2);
                    item.Parent.UpdateDescription();
                }
                else if (item == Sopracciglia)
                {
                    soprvar = Tipo.sopr[index].var;

                    if (soprvar == -1)
                    {
                        soprOp.Enabled = false;
                        soprBase.Enabled = false;
                        soprSec.Enabled = false;
                    }
                    else
                    {
                        soprOp.Enabled = true;
                        soprBase.Enabled = true;
                        soprSec.Enabled = true;
                    }

                    soprcol1 = (item.Panels[0] as UIMenuColorPanel).CurrentSelection;
                    soprcol2 = (item.Panels[1] as UIMenuColorPanel).CurrentSelection;
                    soprop = (item.Panels[2] as UIMenuPercentagePanel).Percentage;
                    if (soprAtt == soprvar && soprOpAtt == soprop && soprcol1 == soprCAtt && soprcol2 == soprC1Att)
                        item.Description = "Stile ~b~Attuale~w~!";
                    else
                        item.Description = Tipo.sopr[index].Description + " - Prezzo: ~g~$" + Tipo.sopr[index].price;
                    SetPedHeadOverlay(PlayerPedId(), 2, soprvar, soprop);
                    SetPedHeadOverlayColor(PlayerPedId(), 2, 1, soprcol1, soprcol2);
                    item.Parent.UpdateDescription();
                }
                else if (item == Barba)
                {
                    if (Tipo.barba[index].var == -1)
                    {
                        beardOp.Enabled = false;
                        beardBase.Enabled = false;
                        beardSec.Enabled = false;
                    }
                    else
                    {
                        beardOp.Enabled = true;
                        beardBase.Enabled = true;
                        beardSec.Enabled = true;
                    }

                    brbcol1 = (Barba.Panels[0] as UIMenuColorPanel).CurrentSelection;
                    brbcol2 = (Barba.Panels[1] as UIMenuColorPanel).CurrentSelection;
                    brbop = (Barba.Panels[2] as UIMenuPercentagePanel).Percentage;
                    brbvar = Tipo.barba[index].var;
                    if (brbAtt == brbvar && brbAttOp == brbop && brbcol1 == brbAttC1 && brbcol2 == brbAttC2)
                        item.Description = "Stile ~b~Attuale~w~!";
                    else
                        item.Description = Tipo.barba[index].Description + " - Prezzo: ~g~$" + Tipo.barba[index].price;
                    item.Parent.UpdateDescription();
                    SetPedHeadOverlay(PlayerPedId(), 1, brbvar, brbop);
                    SetPedHeadOverlayColor(PlayerPedId(), 1, 1, brbcol1, brbcol2);
                }
                else if (item == Trucco)
                {
                    trOp.Enabled = Tipo.trucco[index].var != -1;
                    tru = Tipo.trucco[index].var;
                    item.Description = trcAtt == tru && trcOpAtt == (item.Panels[0] as UIMenuPercentagePanel).Percentage ? "Stile ~b~Attuale~w~!" : Tipo.trucco[index].Description + " - Prezzo: ~g~$" + Tipo.trucco[index].price;
                    SetPedHeadOverlay(PlayerPedId(), 4, tru, (item.Panels[0] as UIMenuPercentagePanel).Percentage);
                    item.Parent.UpdateDescription();
                }
                else if (item == Rossetto)
                {
                    if (Tipo.ross[index].var == -1)
                    {
                        rossOp.Enabled = false;
                        rossColBase.Enabled = false;
                        rossColSec.Enabled = false;
                    }
                    else
                    {
                        rossOp.Enabled = true;
                        rossColBase.Enabled = true;
                        rossColSec.Enabled = true;
                    }

                    rosset = Tipo.ross[index].var;
                    item.Description = rossAttuali == rosset && rossAttualiC == (item.Panels[0] as UIMenuColorPanel).CurrentSelection && rossAttualiO == (item.Panels[2] as UIMenuPercentagePanel).Percentage ? "Stile ~b~Attuale~w~!" : Tipo.ross[index].Description + " - Prezzo: ~g~$" + Tipo.ross[index].price;
                    SetPedHeadOverlay(PlayerPedId(), 8, tru, (item.Panels[2] as UIMenuPercentagePanel).Percentage);
                    SetPedHeadOverlayColor(PlayerPedId(), 8, 1, (item.Panels[0] as UIMenuColorPanel).CurrentSelection, (item.Panels[1] as UIMenuColorPanel).CurrentSelection);
                    item.Parent.UpdateDescription();
                }

                //				else if (item == )
                //				{
                //				}
            };

            #endregion

            #region ON_ITEM_SELECT

            MenuPrincipale.OnListSelect += async (menu, _listItem, itemindex) =>
            {
                if (_listItem == Capelli)
                {
                    Capigliature obj = new Capigliature();

                    switch (NomeNegozio)
                    {
                        case "Kuts":
                            obj = Tipo.capelli.kuts[_listItem.Index];

                            break;
                        case "Hawick":
                            obj = Tipo.capelli.hawick[_listItem.Index];

                            break;
                        case "Osheas":
                            obj = Tipo.capelli.osheas[_listItem.Index];

                            break;
                        case "Combo":
                            obj = Tipo.capelli.beach[_listItem.Index];

                            break;
                        case "Mulet":
                            obj = Tipo.capelli.mulet[_listItem.Index];

                            break;
                    }

                    if (obj.var == 0 && obj.var == capAttuali)
                    {
                        HUD.ShowNotification("Non puoi rasare i capelli 2 volte!!", NotificationColor.Red, true);
                    }
                    else if (obj.var == capAttuali && colAttuale1 == capCol1.CurrentSelection && colAttuale2 == capCol2.CurrentSelection)
                    {
                        HUD.ShowNotification("Non puoi acquistare lo stile che già hai! Prova a cambiare Colore", NotificationColor.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.hair.style = obj.var;
                            skin.hair.color[0] = capCol1.CurrentSelection;
                            skin.hair.color[1] = capCol2.CurrentSelection;
                            colAttuale1 = capCol1.CurrentSelection;
                            colAttuale2 = capCol2.CurrentSelection;
                            capAttuali = obj.var;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("Hai pagato in contanti", NotificationColor.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.hair.style = obj.var;
                                skin.hair.color[0] = capCol1.CurrentSelection;
                                skin.hair.color[1] = capCol2.CurrentSelection;
                                colAttuale1 = capCol1.CurrentSelection;
                                colAttuale2 = capCol2.CurrentSelection;
                                capAttuali = obj.var;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("Hai pagato con carta", NotificationColor.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("NON hai abbastanza soldi per acquistare questa capigliatura!", NotificationColor.Red, true);
                            }
                        }
                    }
                }
                else if (_listItem == Sopracciglia)
                {
                    Capigliature obj = Tipo.sopr[_listItem.Index];

                    if (obj.var == -1 && obj.var == soprAtt)
                    {
                        HUD.ShowNotification("Non puoi rimuovere le sopracciglia 2 volte!!", NotificationColor.Red, true);
                    }
                    else if (obj.var == soprAtt && soprBase.CurrentSelection == soprCAtt && soprSec.CurrentSelection == soprC1Att && soprOp.Percentage == soprOpAtt)
                    {
                        HUD.ShowNotification("Non puoi acquistare lo stile che già hai! Prova a cambiare stile o cambia colore!", NotificationColor.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.facialHair.eyebrow.style = obj.var;
                            skin.facialHair.eyebrow.color[0] = soprBase.CurrentSelection;
                            skin.facialHair.eyebrow.color[1] = soprSec.CurrentSelection;
                            skin.facialHair.eyebrow.opacity = soprOp.Percentage;
                            soprAtt = obj.var;
                            soprCAtt = soprBase.CurrentSelection;
                            soprC1Att = soprSec.CurrentSelection;
                            soprOpAtt = soprOp.Percentage;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("Hai pagato in contanti", NotificationColor.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.facialHair.eyebrow.style = obj.var;
                                skin.facialHair.eyebrow.color[0] = soprBase.CurrentSelection;
                                skin.facialHair.eyebrow.color[1] = soprSec.CurrentSelection;
                                skin.facialHair.eyebrow.opacity = soprOp.Percentage;
                                soprAtt = obj.var;
                                soprCAtt = soprBase.CurrentSelection;
                                soprC1Att = soprSec.CurrentSelection;
                                soprOpAtt = soprOp.Percentage;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("Hai pagato con carta", NotificationColor.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("NON hai abbastanza soldi per acquistare queste sopracciglia!", NotificationColor.Red, true);
                            }
                        }
                    }
                }
                else if (_listItem == Barba)
                {
                    Capigliature obj = Tipo.barba[_listItem.Index];

                    if (obj.var == -1 && obj.var == brbAtt)
                    {
                        HUD.ShowNotification("Non puoi rasare la barba 2 volte!!", NotificationColor.Red, true);
                    }
                    else if (obj.var == brbAtt && beardBase.CurrentSelection == brbAttC1 && beardSec.CurrentSelection == brbAttC2 && beardOp.Percentage == brbAttOp)
                    {
                        HUD.ShowNotification("Non puoi acquistare lo stile che già hai! Prova a cambiare stile o cambia colore!", NotificationColor.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.facialHair.beard.style = obj.var;
                            skin.facialHair.beard.color[0] = beardBase.CurrentSelection;
                            skin.facialHair.beard.color[1] = beardSec.CurrentSelection;
                            skin.facialHair.beard.opacity = beardOp.Percentage;
                            brbAtt = obj.var;
                            brbAttC1 = beardBase.CurrentSelection;
                            brbAttC2 = beardSec.CurrentSelection;
                            brbAttOp = beardOp.Percentage;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("Hai pagato in contanti", NotificationColor.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.facialHair.beard.style = obj.var;
                                skin.facialHair.beard.color[0] = beardBase.CurrentSelection;
                                skin.facialHair.beard.color[1] = beardSec.CurrentSelection;
                                skin.facialHair.beard.opacity = beardOp.Percentage;
                                brbAtt = obj.var;
                                brbAttC1 = beardBase.CurrentSelection;
                                brbAttC2 = beardSec.CurrentSelection;
                                brbAttOp = beardOp.Percentage;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("Hai pagato con carta", NotificationColor.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("NON hai abbastanza soldi per acquistare questa rasatura!", NotificationColor.Red, true);
                            }
                        }
                    }
                }
                else if (_listItem == Trucco)
                {
                    Capigliature obj = Tipo.trucco[_listItem.Index];

                    if (obj.var == -1 && obj.var == brbAtt)
                    {
                        HUD.ShowNotification("Non puoi rimuovere il trucco 2 volte!!", NotificationColor.Red, true);
                    }
                    else if (obj.var == trcAtt && trOp.Percentage == trcOpAtt)
                    {
                        HUD.ShowNotification("Non puoi acquistare lo stile che già hai! Prova a cambiare stile o cambia colore!", NotificationColor.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.makeup.style = obj.var;
                            skin.makeup.opacity = trOp.Percentage;
                            trcAtt = obj.var;
                            trcOpAtt = trOp.Percentage;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("Hai pagato in contanti", NotificationColor.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.makeup.style = obj.var;
                                skin.makeup.opacity = trOp.Percentage;
                                trcAtt = obj.var;
                                trcOpAtt = trOp.Percentage;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("Hai pagato con carta", NotificationColor.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("NON hai abbastanza soldi per acquistare questo trucco!", NotificationColor.Red, true);
                            }
                        }
                    }
                }
                else if (_listItem == Rossetto)
                {
                    Capigliature obj = Tipo.ross[_listItem.Index];

                    if (obj.var == -1 && obj.var == rossAttuali)
                    {
                        HUD.ShowNotification("Non puoi rimuovere lo stesso rossetto 2 volte!!", NotificationColor.Red, true);
                    }
                    else if (obj.var == rossAttuali && rossColBase.CurrentSelection == rossAttualiC && rossColSec.CurrentSelection == rossAttualiC2 && rossOp.Percentage == rossAttualiO)
                    {
                        HUD.ShowNotification("Non puoi acquistare lo stile che già hai! Prova a cambiare stile o cambia colore!", NotificationColor.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.lipstick.style = obj.var;
                            skin.lipstick.color[0] = rossColBase.CurrentSelection;
                            skin.lipstick.color[1] = rossColSec.CurrentSelection;
                            skin.lipstick.opacity = rossOp.Percentage;
                            rossAttuali = obj.var;
                            rossAttualiC = rossColBase.CurrentSelection;
                            rossAttualiC2 = rossColSec.CurrentSelection;
                            rossAttualiO = rossOp.Percentage;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("Hai pagato in contanti", NotificationColor.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.lipstick.style = obj.var;
                                skin.lipstick.color[0] = rossColBase.CurrentSelection;
                                skin.lipstick.color[1] = rossColSec.CurrentSelection;
                                skin.lipstick.opacity = rossOp.Percentage;
                                rossAttuali = obj.var;
                                rossAttualiC = rossColBase.CurrentSelection;
                                rossAttualiC2 = rossColSec.CurrentSelection;
                                rossAttualiO = rossOp.Percentage;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("Hai pagato con carta", NotificationColor.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("NON hai abbastanza soldi per acquistare questo rossetto!", NotificationColor.Red, true);
                            }
                        }
                    }
                }

                //				else if (_listItem == )
                //				{
                //				}
            };

            #endregion

            MenuPrincipale.OnMenuStateChanged += async (a, b, c) =>
            {
                if (c != MenuState.Closed || a != MenuPrincipale) return;
                ClearPedTasks(PlayerPedId());
                Funzioni.UpdateFace(PlayerPedId(), skin);
                RenderScriptCams(false, true, 1000, true, false);
                Client.Instance.RemoveTick(NuovaCam);
            };

            #endregion

            MenuPrincipale.Visible = true;
            Client.Instance.AddTick(NuovaCam);
        }

        private static float fov = 0;

        private static async Task NuovaCam()
        {
            if (MenuPrincipale.Visible)
            {
                if (Input.IsControlPressed(Control.FrontendLt))
                {
                    fov -= .7f;
                    if (fov <= 23f) fov = 23f;
                    Camm.FieldOfView = fov;
                }
                else if (Input.IsControlJustReleased(Control.FrontendLt))
                {
                    do
                    {
                        await BaseScript.Delay(0);
                        fov += .7f;
                        if (fov >= 35f) fov = 35f;
                        Camm.FieldOfView = fov;
                    } while (fov != 23f && !Input.IsControlPressed(Control.FrontendLt));
                }
            }
        }
    }
}

/*
 *				DA AGGIUNGERE IN UN NUOVO TICK A MENU APERTO! ANCHE PER CAPELLI E ALTRO
 *				if (Barba.Hovered)
				{
					MenuPrincipale.AddInstructionalButton(buttonRt);
					MenuPrincipale.AddInstructionalButton(buttonLt);
					MenuPrincipale.AddInstructionalButton(buttonRb);
					MenuPrincipale.AddInstructionalButton(buttonLb);
				}
				else
				{
					MenuPrincipale.RemoveInstructionalButton(buttonRt);
					MenuPrincipale.RemoveInstructionalButton(buttonLt);
					MenuPrincipale.RemoveInstructionalButton(buttonRb);
					MenuPrincipale.RemoveInstructionalButton(buttonLb);
				}
*/