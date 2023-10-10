using Settings.Client.Configuration.Negozi.Barbieri;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Shops
{
    internal static class BarberClient
    {
        private static Ped CurrentBarber = new Ped(0);
        private static Vector3 S1 = new Vector3(0);
        private static Vector3 S2 = new Vector3(0);
        private static Vector3 S3 = new Vector3(0);
        private static Camera Camm = new Camera(0);
        private static bool MadeKuts = false;
        private static bool MadeHawick = false;
        private static bool MadeOsheas = false;
        private static bool MadeCombo = false;
        private static List<Blip> blips = new List<Blip>();

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            blips.ForEach(x => x.Delete());
            blips.Clear();
        }

        public static void Spawned(PlayerClient client)
        {
            foreach (BarberShop barbiere in ConfigBarbieri.Kuts)
            {
                Blip kuts = World.CreateBlip(barbiere.Coord);
                kuts.Sprite = (BlipSprite)71;
                kuts.Color = (BlipColor)12;
                kuts.IsShortRange = true;
                kuts.Name = "Herr Kuts";
                blips.Add(kuts);
            }

            Blip Hawick = new Blip(AddBlipForCoord(ConfigBarbieri.Hawick.Coord.X, ConfigBarbieri.Hawick.Coord.Y, ConfigBarbieri.Hawick.Coord.Z)) { Sprite = (BlipSprite)71, Color = (BlipColor)17, IsShortRange = true, Name = "Barbieri Hair On Hawick" };
            Blip Combo = new Blip(AddBlipForCoord(ConfigBarbieri.Combo.Coord.X, ConfigBarbieri.Combo.Coord.Y, ConfigBarbieri.Combo.Coord.Z)) { Sprite = (BlipSprite)71, Color = (BlipColor)66, IsShortRange = true, Name = "Barbieri Beachcombover" };
            Blip Osheas = new Blip(AddBlipForCoord(ConfigBarbieri.Osheas.Coord.X, ConfigBarbieri.Osheas.Coord.Y, ConfigBarbieri.Osheas.Coord.Z)) { Sprite = (BlipSprite)71, Color = (BlipColor)38, IsShortRange = true, Name = "Barbieri Oshea's" };
            blips.Add(Hawick);
            blips.Add(Osheas);
            blips.Add(Combo);
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
                HUD.ShowHelp("Remember you can always use ~b~MOUSE~w~ to select colors and opacity.");
                ShowCam(S, Ch, C);
                BarberMenu(Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male" ? Client.Settings.RolePlay.Shops.Barbers.Male : Client.Settings.RolePlay.Shops.Barbers.Female, Menu);
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

        public static async Task Chairs()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            foreach (BarberShop t in ConfigBarbieri.Kuts)
                if (p.IsInRangeOf(t.Coord, 50f) && !MadeKuts)
                {
                    S1 = t.Chair1;
                    S2 = t.Chair2;
                    S3 = t.Chair3;
                    CurrentBarber = await CreateBarber(t.Model);
                    MadeKuts = true;
                }
                else if (!p.IsInRangeOf(t.Coord, 50))
                {
                    if (!MadeKuts) continue;
                    CurrentBarber.Delete();
                    MadeKuts = false;
                }
                else if (p.IsInRangeOf(CurrentBarber.Position, 2f))
                {
                    HUD.ShowHelp("Press ~INPUT_CONTEXT~ to go sit");

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
                                HUD.ShowNotification("Wait... no free seats at the moment.", true);
                            }
                        }
                    }
                }

            if (p.IsInRangeOf(ConfigBarbieri.Hawick.Coord, 50) && !MadeHawick)
            {
                CurrentBarber = await CreateBarber(ConfigBarbieri.Hawick.Model);
                MadeHawick = true;
            }
            else if (!p.IsInRangeOf(ConfigBarbieri.Hawick.Coord, 50))
            {
                if (MadeHawick)
                {
                    CurrentBarber.Delete();
                    MadeHawick = false;
                }
            }
            else if (p.IsInRangeOf(CurrentBarber.Position, 2))
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to go sit");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    TaskSetBlockingOfNonTemporaryEvents(PlayerPedId(), true);

                    if (!IsAnyPedNearPoint(ConfigBarbieri.Hawick.Chair1.X, ConfigBarbieri.Hawick.Chair1.Y, ConfigBarbieri.Hawick.Chair1.Z, 0.35f))
                    {
                        TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Hawick.Chair1.X, ConfigBarbieri.Hawick.Chair1.Y, ConfigBarbieri.Hawick.Chair1.Z, ConfigBarbieri.Hawick.Heading, 0, true, false);
                        Controlla(ConfigBarbieri.Hawick.Chair1, ConfigBarbieri.Hawick.Cam, ConfigBarbieri.Hawick.CXYZ, "Hawick");
                    }
                    else
                    {
                        if (!IsAnyPedNearPoint(ConfigBarbieri.Hawick.Chair2.X, ConfigBarbieri.Hawick.Chair2.Y, ConfigBarbieri.Hawick.Chair2.Z, 0.35f))
                        {
                            TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Hawick.Chair2.X, ConfigBarbieri.Hawick.Chair2.Y, ConfigBarbieri.Hawick.Chair2.Z, ConfigBarbieri.Hawick.Heading, 0, true, false);
                            Controlla(ConfigBarbieri.Hawick.Chair2, ConfigBarbieri.Hawick.Cam, ConfigBarbieri.Hawick.CXYZ, "Hawick");
                        }
                        else
                        {
                            if (!IsAnyPedNearPoint(ConfigBarbieri.Hawick.Chair3.X, ConfigBarbieri.Hawick.Chair3.Y, ConfigBarbieri.Hawick.Chair3.Z, 0.35f))
                            {
                                TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Hawick.Chair3.X, ConfigBarbieri.Hawick.Chair3.Y, ConfigBarbieri.Hawick.Chair3.Z, ConfigBarbieri.Hawick.Heading, 0, true, false);
                                Controlla(ConfigBarbieri.Hawick.Chair3, ConfigBarbieri.Hawick.Cam, ConfigBarbieri.Hawick.CXYZ, "Hawick");
                            }
                            else
                            {
                                HUD.ShowNotification("Wait... no free seats at the moment.", true);
                            }
                        }
                    }
                }
            }

            if (p.IsInRangeOf(ConfigBarbieri.Osheas.Coord, 50) && !MadeOsheas)
            {
                CurrentBarber = await CreateBarber(ConfigBarbieri.Osheas.Model);
                MadeOsheas = true;
            }
            else if (!p.IsInRangeOf(ConfigBarbieri.Osheas.Coord, 50))
            {
                if (MadeOsheas)
                {
                    CurrentBarber.Delete();
                    MadeOsheas = false;
                }
            }
            else if (p.IsInRangeOf(CurrentBarber.Position, 2))
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to go sit");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    TaskSetBlockingOfNonTemporaryEvents(PlayerPedId(), true);

                    if (!IsAnyPedNearPoint(ConfigBarbieri.Osheas.Chair1.X, ConfigBarbieri.Osheas.Chair1.Y, ConfigBarbieri.Osheas.Chair1.Z, 0.35f))
                    {
                        TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Osheas.Chair1.X, ConfigBarbieri.Osheas.Chair1.Y, ConfigBarbieri.Osheas.Chair1.Z, ConfigBarbieri.Osheas.Heading, 0, true, false);
                        Controlla(ConfigBarbieri.Osheas.Chair1, ConfigBarbieri.Osheas.Cam, ConfigBarbieri.Osheas.CXYZ, "Osheas");
                    }
                    else
                    {
                        if (!IsAnyPedNearPoint(ConfigBarbieri.Osheas.Chair2.X, ConfigBarbieri.Osheas.Chair2.Y, ConfigBarbieri.Osheas.Chair2.Z, 0.35f))
                        {
                            TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Osheas.Chair2.X, ConfigBarbieri.Osheas.Chair2.Y, ConfigBarbieri.Osheas.Chair2.Z, ConfigBarbieri.Osheas.Heading, 0, true, false);
                            Controlla(ConfigBarbieri.Osheas.Chair2, ConfigBarbieri.Osheas.Cam, ConfigBarbieri.Osheas.CXYZ, "Osheas");
                        }
                        else
                        {
                            if (!IsAnyPedNearPoint(ConfigBarbieri.Osheas.Chair3.X, ConfigBarbieri.Osheas.Chair3.Y, ConfigBarbieri.Osheas.Chair3.Z, 0.35f))
                            {
                                TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Osheas.Chair3.X, ConfigBarbieri.Osheas.Chair3.Y, ConfigBarbieri.Osheas.Chair3.Z, ConfigBarbieri.Osheas.Heading, 0, true, false);
                                Controlla(ConfigBarbieri.Osheas.Chair3, ConfigBarbieri.Osheas.Cam, ConfigBarbieri.Osheas.CXYZ, "Osheas");
                            }
                            else
                            {
                                HUD.ShowNotification("Wait... no free seats at the moment.", true);
                            }
                        }
                    }
                }
            }

            if (p.IsInRangeOf(ConfigBarbieri.Combo.Coord, 50) && !MadeCombo)
            {
                CurrentBarber = await CreateBarber(ConfigBarbieri.Combo.Model);
                MadeCombo = true;
            }
            else if (!p.IsInRangeOf(ConfigBarbieri.Combo.Coord, 50))
            {
                if (MadeCombo)
                {
                    CurrentBarber.Delete();
                    MadeCombo = false;
                }
            }
            else if (p.IsInRangeOf(CurrentBarber.Position, 2))
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to go sit");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    TaskSetBlockingOfNonTemporaryEvents(PlayerPedId(), true);

                    if (!IsAnyPedNearPoint(ConfigBarbieri.Combo.Chair1.X, ConfigBarbieri.Combo.Chair1.Y, ConfigBarbieri.Combo.Chair1.Z, 0.35f))
                    {
                        TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Combo.Chair1.X, ConfigBarbieri.Combo.Chair1.Y, ConfigBarbieri.Combo.Chair1.Z, ConfigBarbieri.Combo.Heading, 0, true, false);
                        Controlla(ConfigBarbieri.Combo.Chair1, ConfigBarbieri.Combo.Cam, ConfigBarbieri.Combo.CXYZ, "Combo");
                    }
                    else
                    {
                        if (!IsAnyPedNearPoint(ConfigBarbieri.Combo.Chair2.X, ConfigBarbieri.Combo.Chair2.Y, ConfigBarbieri.Combo.Chair2.Z, 0.35f))
                        {
                            TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Combo.Chair2.X, ConfigBarbieri.Combo.Chair2.Y, ConfigBarbieri.Combo.Chair2.Z, ConfigBarbieri.Combo.Heading, 0, true, false);
                            Controlla(ConfigBarbieri.Combo.Chair2, ConfigBarbieri.Combo.Cam, ConfigBarbieri.Combo.CXYZ, "Combo");
                        }
                        else
                        {
                            if (!IsAnyPedNearPoint(ConfigBarbieri.Combo.Chair3.X, ConfigBarbieri.Combo.Chair3.Y, ConfigBarbieri.Combo.Chair3.Z, 0.35f))
                            {
                                TaskStartScenarioAtPosition(PlayerPedId(), "PROP_HUMAN_SEAT_CHAIR_MP_PLAYER", ConfigBarbieri.Combo.Chair3.X, ConfigBarbieri.Combo.Chair3.Y, ConfigBarbieri.Combo.Chair3.Z, ConfigBarbieri.Combo.Heading, 0, true, false);
                                Controlla(ConfigBarbieri.Combo.Chair3, ConfigBarbieri.Combo.Cam, ConfigBarbieri.Combo.CXYZ, "Combo");
                            }
                            else
                            {
                                HUD.ShowNotification("Wait... no free seats at the moment.", true);
                            }
                        }
                    }
                }
            }
        }

        private static UIMenu MainMenu = new UIMenu("", "");

        private static async void BarberMenu(HeadBarbers type, string shopName)
        {
            #region DICHIARAZIONE

            System.Drawing.Point pos = new System.Drawing.Point(50, 100);
            Skin skin = Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin;
            int currentHair = skin.Hair.Style;
            int currentHairCol1 = skin.Hair.Color[0];
            int currenthairCol2 = skin.Hair.Color[1];
            int currentLipst = skin.Lipstick.Style;
            int currentLipstCol1 = skin.Lipstick.Color[0];
            int currentLipstCol2 = skin.Lipstick.Color[1];
            float currentLipstOp = skin.Lipstick.Opacity;
            int currentMakup = skin.Makeup.Style;
            float currentMakupOp = skin.Makeup.Opacity;
            int currentEyebr = skin.FacialHair.Eyebrow.Style;
            int currentEyebrCol1 = skin.FacialHair.Eyebrow.Color[0];
            int currentEyebrCol2 = skin.FacialHair.Eyebrow.Color[1];
            float currentEyebrOp = skin.FacialHair.Eyebrow.Opacity;
            int currentBeard = skin.FacialHair.Beard.Style;
            int currentBeardCol1 = skin.FacialHair.Beard.Color[0];
            int currentBeardCol2 = skin.FacialHair.Beard.Color[1];
            float currentBeardOp = skin.FacialHair.Beard.Opacity;
            string desc = "";
            string title = "";
            desc = skin.Sex == "Male" ? "Really?!?!" : "Choose the perfect makeup!";

            switch (shopName)
            {
                case "Kuts":
                    title = "Welcome to Herr Kuts!";

                    break;
                case "Hawick":
                    title = "Welcome to Hair On Hawick!";

                    break;
                case "Osheas":
                    title = "Welcome to O'Sheas!";

                    break;
                case "Combo":
                    title = "Welcome to Beachcombover!";

                    break;
                case "Mulet":
                    title = "Welcome to Bob Mulet!";

                    break;
            }

            List<dynamic> beards = type.Beard.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> makeup = type.Makeup.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> eyebrows = type.Eyebrows.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> lipstick = type.Lipstick.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> hairkuts = type.Hair.kuts.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> hairwick = type.Hair.hawick.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> hairosheas = type.Hair.osheas.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> haircombo = type.Hair.beach.Select(b => b.Name).Cast<dynamic>().ToList();
            List<dynamic> hairmulet = type.Hair.mulet.Select(b => b.Name).Cast<dynamic>().ToList();
            MainMenu = new UIMenu("", title, pos, Main.Textures[shopName].Key, Main.Textures[shopName].Value);
            MainMenu.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            UIMenuListItem Hair = new UIMenuListItem("Hair", new List<dynamic>() { 0 }, 0);

            switch (shopName)
            {
                case "Kuts":
                    Hair = new UIMenuListItem("Hair", hairkuts, 0);

                    break;
                case "Hawick":
                    Hair = new UIMenuListItem("Hair", hairwick, 0);

                    break;
                case "Osheas":
                    Hair = new UIMenuListItem("Hair", hairosheas, 0);

                    break;
                case "Combo":
                    Hair = new UIMenuListItem("Hair", haircombo, 0);

                    break;
                case "Mulet":
                    Hair = new UIMenuListItem("Hair", hairmulet, 0);

                    break;
            }

            UIMenuColorPanel hairCol1 = new UIMenuColorPanel("Main Color", ColorPanelType.Hair);
            UIMenuColorPanel hairCol2 = new UIMenuColorPanel("Secondary Color", ColorPanelType.Hair);
            Hair.AddPanel(hairCol1);
            Hair.AddPanel(hairCol2);
            hairCol1.Enabled = false;
            hairCol2.Enabled = false;
            hairCol1.CurrentSelection = currentHairCol1;
            hairCol2.CurrentSelection = currenthairCol2;
            MainMenu.AddItem(Hair);
            int haircol1 = 0;
            int haircol2 = 0;
            int hairvar;
            UIMenuListItem eyebrowsItem = new UIMenuListItem("eyebrowsItem", eyebrows, 0);
            UIMenuColorPanel eyebrowsBase = new UIMenuColorPanel("Main Color", ColorPanelType.Hair);
            UIMenuColorPanel eyebrowsSec = new UIMenuColorPanel("Secondary Color", ColorPanelType.Hair);
            UIMenuPercentagePanel eyebrowsOp = new UIMenuPercentagePanel("Opacity", "0%", "100%");
            eyebrowsItem.AddPanel(eyebrowsBase);
            eyebrowsItem.AddPanel(eyebrowsSec);
            eyebrowsItem.AddPanel(eyebrowsOp);
            eyebrowsBase.Enabled = false;
            eyebrowsSec.Enabled = false;
            eyebrowsOp.Enabled = false;
            eyebrowsBase.CurrentSelection = currentEyebrCol1;
            eyebrowsSec.CurrentSelection = currentEyebrCol2;
            eyebrowsOp.Percentage = currentEyebrOp;
            MainMenu.AddItem(eyebrowsItem);
            float eyebrop;
            int eyebrcol1;
            int eyebrcol2;
            int eyebrvar;
            UIMenuListItem beardItem = null;
            UIMenuColorPanel beardBase = null;
            UIMenuColorPanel beardSec = null;
            UIMenuPercentagePanel beardOp = null;
            float brdop;
            int brdcol1;
            int brdcol2;
            int brdvar;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
            {
                beardItem = new UIMenuListItem("Select beard", beards, 0);
                beardBase = new UIMenuColorPanel("Main Color", ColorPanelType.Hair);
                beardSec = new UIMenuColorPanel("Secondary Color", ColorPanelType.Hair);
                beardOp = new UIMenuPercentagePanel("Opacity", "0%", "100%");
                beardOp.Percentage = 1.0f;
                beardItem.AddPanel(beardBase);
                beardItem.AddPanel(beardSec);
                beardItem.AddPanel(beardOp);
                if (skin.Sex == "Male") MainMenu.AddItem(beardItem);
                beardOp.Enabled = false;
                beardBase.Enabled = false;
                beardSec.Enabled = false;
                beardOp.Percentage = currentBeardOp;
                beardBase.CurrentSelection = currentBeardCol1;
                beardSec.CurrentSelection = currentBeardCol2;
            }

            UIMenuListItem makeupItem = new UIMenuListItem("MakeUp", makeup, 0, desc);
            UIMenuPercentagePanel mkuOp = new UIMenuPercentagePanel("Opacity", "0%", "100%");
            makeupItem.AddPanel(mkuOp);
            mkuOp.Enabled = false;
            mkuOp.Percentage = currentMakupOp;
            int mku = 0;
            MainMenu.AddItem(makeupItem);
            UIMenuListItem lipstickItem = new UIMenuListItem("Lipstick", lipstick, 0, desc);
            UIMenuColorPanel lipstickColBase = new UIMenuColorPanel("Main Color", ColorPanelType.Makeup);
            UIMenuColorPanel lipstickColSec = new UIMenuColorPanel("Secondary Color", ColorPanelType.Makeup);
            UIMenuPercentagePanel lipstickOp = new UIMenuPercentagePanel("Opacity", "0%", "100%");
            lipstickItem.AddPanel(lipstickColBase);
            lipstickItem.AddPanel(lipstickColSec);
            lipstickItem.AddPanel(lipstickOp);
            lipstickColBase.Enabled = false;
            lipstickColSec.Enabled = false;
            lipstickOp.Enabled = false;
            lipstickColBase.CurrentSelection = currentLipstCol1;
            lipstickColSec.CurrentSelection = currentLipstCol2;
            lipstickOp.Percentage = currentLipstOp;
            int lipstickVal = 0;
            MainMenu.AddItem(lipstickItem);

            //UIMenuListItem Fard = new UIMenuListItem("Fard", trucco, 0, desc);

            #endregion

            #region Menu Body

            #region ON_LIST_CHANGED

            MainMenu.OnListChange += (menu, item, index) =>
            {
                if (item == Hair)
                {
                    List<BarberStyle> obj = new List<BarberStyle>();

                    switch (shopName)
                    {
                        case "Kuts":
                            obj = type.Hair.kuts;

                            break;
                        case "Hawick":
                            obj = type.Hair.hawick;

                            break;
                        case "Osheas":
                            obj = type.Hair.osheas;

                            break;
                        case "Combo":
                            obj = type.Hair.beach;

                            break;
                        case "Mulet":
                            obj = type.Hair.mulet;

                            break;
                    }

                    if (obj[index].var == 0)
                    {
                        hairCol1.Enabled = false;
                        hairCol2.Enabled = false;
                    }
                    else
                    {
                        hairCol1.Enabled = true;
                        hairCol2.Enabled = true;
                    }

                    if (currentHair == obj[index].var)
                        item.Description = "Current Style";
                    else
                        item.Description = obj[index].Description + " - Price: ~g~$" + obj[index].price;
                    haircol1 = hairCol1.CurrentSelection;
                    haircol2 = hairCol2.CurrentSelection;
                    hairvar = obj[index].var;
                    SetPedComponentVariation(PlayerPedId(), 2, obj[index].var, 0, 2);
                    SetPedHairColor(PlayerPedId(), haircol1, haircol2);
                    item.Parent.UpdateDescription();
                }
                else if (item == eyebrowsItem)
                {
                    eyebrvar = type.Eyebrows[index].var;

                    if (eyebrvar == -1)
                    {
                        eyebrowsOp.Enabled = false;
                        eyebrowsBase.Enabled = false;
                        eyebrowsSec.Enabled = false;
                    }
                    else
                    {
                        eyebrowsOp.Enabled = true;
                        eyebrowsBase.Enabled = true;
                        eyebrowsSec.Enabled = true;
                    }

                    eyebrcol1 = (item.Panels[0] as UIMenuColorPanel).CurrentSelection;
                    eyebrcol2 = (item.Panels[1] as UIMenuColorPanel).CurrentSelection;
                    eyebrop = (item.Panels[2] as UIMenuPercentagePanel).Percentage;
                    if (currentEyebr == eyebrvar && currentEyebrOp == eyebrop && eyebrcol1 == currentEyebrCol1 && eyebrcol2 == currentEyebrCol2)
                        item.Description = "Current Style";
                    else
                        item.Description = type.Eyebrows[index].Description + " - Price: ~g~$" + type.Eyebrows[index].price;
                    SetPedHeadOverlay(PlayerPedId(), 2, eyebrvar, eyebrop);
                    SetPedHeadOverlayColor(PlayerPedId(), 2, 1, eyebrcol1, eyebrcol2);
                    item.Parent.UpdateDescription();
                }
                else if (item == beardItem)
                {
                    if (type.Beard[index].var == -1)
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

                    brdcol1 = (beardItem.Panels[0] as UIMenuColorPanel).CurrentSelection;
                    brdcol2 = (beardItem.Panels[1] as UIMenuColorPanel).CurrentSelection;
                    brdop = (beardItem.Panels[2] as UIMenuPercentagePanel).Percentage;
                    brdvar = type.Beard[index].var;
                    if (currentBeard == brdvar && currentBeardOp == brdop && brdcol1 == currentBeardCol1 && brdcol2 == currentBeardCol2)
                        item.Description = "Current Style";
                    else
                        item.Description = type.Beard[index].Description + " - Price: ~g~$" + type.Beard[index].price;
                    item.Parent.UpdateDescription();
                    SetPedHeadOverlay(PlayerPedId(), 1, brdvar, brdop);
                    SetPedHeadOverlayColor(PlayerPedId(), 1, 1, brdcol1, brdcol2);
                }
                else if (item == makeupItem)
                {
                    mkuOp.Enabled = type.Makeup[index].var != -1;
                    mku = type.Makeup[index].var;
                    item.Description = currentMakup == mku && currentMakupOp == (item.Panels[0] as UIMenuPercentagePanel).Percentage ? "Current Style" : type.Makeup[index].Description + " - Price: ~g~$" + type.Makeup[index].price;
                    SetPedHeadOverlay(PlayerPedId(), 4, mku, (item.Panels[0] as UIMenuPercentagePanel).Percentage);
                    item.Parent.UpdateDescription();
                }
                else if (item == lipstickItem)
                {
                    if (type.Lipstick[index].var == -1)
                    {
                        lipstickOp.Enabled = false;
                        lipstickColBase.Enabled = false;
                        lipstickColSec.Enabled = false;
                    }
                    else
                    {
                        lipstickOp.Enabled = true;
                        lipstickColBase.Enabled = true;
                        lipstickColSec.Enabled = true;
                    }

                    lipstickVal = type.Lipstick[index].var;
                    item.Description = currentLipst == lipstickVal && currentLipstCol1 == (item.Panels[0] as UIMenuColorPanel).CurrentSelection && currentLipstOp == (item.Panels[2] as UIMenuPercentagePanel).Percentage ? "Current Style" : type.Lipstick[index].Description + " - Price: ~g~$" + type.Lipstick[index].price;
                    SetPedHeadOverlay(PlayerPedId(), 8, mku, (item.Panels[2] as UIMenuPercentagePanel).Percentage);
                    SetPedHeadOverlayColor(PlayerPedId(), 8, 1, (item.Panels[0] as UIMenuColorPanel).CurrentSelection, (item.Panels[1] as UIMenuColorPanel).CurrentSelection);
                    item.Parent.UpdateDescription();
                }

                //				else if (item == )
                //				{
                //				}
            };

            #endregion

            #region ON_ITEM_SELECT

            MainMenu.OnListSelect += async (menu, _listItem, itemindex) =>
            {
                if (_listItem == Hair)
                {
                    BarberStyle obj = new BarberStyle();

                    switch (shopName)
                    {
                        case "Kuts":
                            obj = type.Hair.kuts[_listItem.Index];

                            break;
                        case "Hawick":
                            obj = type.Hair.hawick[_listItem.Index];

                            break;
                        case "Osheas":
                            obj = type.Hair.osheas[_listItem.Index];

                            break;
                        case "Combo":
                            obj = type.Hair.beach[_listItem.Index];

                            break;
                        case "Mulet":
                            obj = type.Hair.mulet[_listItem.Index];

                            break;
                    }

                    if (obj.var == 0 && obj.var == currentHair)
                    {
                        HUD.ShowNotification("You cannot shave your hair twice!!", ColoreNotifica.Red, true);
                    }
                    else if (obj.var == currentHair && currentHairCol1 == hairCol1.CurrentSelection && currenthairCol2 == hairCol2.CurrentSelection)
                    {
                        HUD.ShowNotification("You can't buy the style you already have! Try changing Color", ColoreNotifica.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.Hair.Style = obj.var;
                            skin.Hair.Color[0] = hairCol1.CurrentSelection;
                            skin.Hair.Color[1] = hairCol2.CurrentSelection;
                            currentHairCol1 = hairCol1.CurrentSelection;
                            currenthairCol2 = hairCol2.CurrentSelection;
                            currentHair = obj.var;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("You paid by cash", ColoreNotifica.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.Hair.Style = obj.var;
                                skin.Hair.Color[0] = hairCol1.CurrentSelection;
                                skin.Hair.Color[1] = hairCol2.CurrentSelection;
                                currentHairCol1 = hairCol1.CurrentSelection;
                                currenthairCol2 = hairCol2.CurrentSelection;
                                currentHair = obj.var;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("You paid with bank", ColoreNotifica.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("You do NOT have enough money to buy this hair!", ColoreNotifica.Red, true);
                            }
                        }
                    }
                }
                else if (_listItem == eyebrowsItem)
                {
                    BarberStyle obj = type.Eyebrows[_listItem.Index];

                    if (obj.var == -1 && obj.var == currentEyebr)
                    {
                        HUD.ShowNotification("You can't remove the eyelashes twice!!", ColoreNotifica.Red, true);
                    }
                    else if (obj.var == currentEyebr && eyebrowsBase.CurrentSelection == currentEyebrCol1 && eyebrowsSec.CurrentSelection == currentEyebrCol2 && eyebrowsOp.Percentage == currentEyebrOp)
                    {
                        HUD.ShowNotification("You can't buy the style you already have! Try changing the style or changing the color!", ColoreNotifica.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.FacialHair.Eyebrow.Style = obj.var;
                            skin.FacialHair.Eyebrow.Color[0] = eyebrowsBase.CurrentSelection;
                            skin.FacialHair.Eyebrow.Color[1] = eyebrowsSec.CurrentSelection;
                            skin.FacialHair.Eyebrow.Opacity = eyebrowsOp.Percentage;
                            currentEyebr = obj.var;
                            currentEyebrCol1 = eyebrowsBase.CurrentSelection;
                            currentEyebrCol2 = eyebrowsSec.CurrentSelection;
                            currentEyebrOp = eyebrowsOp.Percentage;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("You paid with cash", ColoreNotifica.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.FacialHair.Eyebrow.Style = obj.var;
                                skin.FacialHair.Eyebrow.Color[0] = eyebrowsBase.CurrentSelection;
                                skin.FacialHair.Eyebrow.Color[1] = eyebrowsSec.CurrentSelection;
                                skin.FacialHair.Eyebrow.Opacity = eyebrowsOp.Percentage;
                                currentEyebr = obj.var;
                                currentEyebrCol1 = eyebrowsBase.CurrentSelection;
                                currentEyebrCol2 = eyebrowsSec.CurrentSelection;
                                currentEyebrOp = eyebrowsOp.Percentage;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("You paid by bank", ColoreNotifica.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("You do NOT have enough money to buy these eyebrows!", ColoreNotifica.Red, true);
                            }
                        }
                    }
                }
                else if (_listItem == beardItem)
                {
                    BarberStyle obj = type.Beard[_listItem.Index];

                    if (obj.var == -1 && obj.var == currentBeard)
                    {
                        HUD.ShowNotification("You can't shave your beard twice!!", ColoreNotifica.Red, true);
                    }
                    else if (obj.var == currentBeard && beardBase.CurrentSelection == currentBeardCol1 && beardSec.CurrentSelection == currentBeardCol2 && beardOp.Percentage == currentBeardOp)
                    {
                        HUD.ShowNotification("You can't buy the style you already have! Try changing the style or changing the color!", ColoreNotifica.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.FacialHair.Beard.Style = obj.var;
                            skin.FacialHair.Beard.Color[0] = beardBase.CurrentSelection;
                            skin.FacialHair.Beard.Color[1] = beardSec.CurrentSelection;
                            skin.FacialHair.Beard.Opacity = beardOp.Percentage;
                            currentBeard = obj.var;
                            currentBeardCol1 = beardBase.CurrentSelection;
                            currentBeardCol2 = beardSec.CurrentSelection;
                            currentBeardOp = beardOp.Percentage;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("You paid with cash", ColoreNotifica.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.FacialHair.Beard.Style = obj.var;
                                skin.FacialHair.Beard.Color[0] = beardBase.CurrentSelection;
                                skin.FacialHair.Beard.Color[1] = beardSec.CurrentSelection;
                                skin.FacialHair.Beard.Opacity = beardOp.Percentage;
                                currentBeard = obj.var;
                                currentBeardCol1 = beardBase.CurrentSelection;
                                currentBeardCol2 = beardSec.CurrentSelection;
                                currentBeardOp = beardOp.Percentage;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("You paid by bank", ColoreNotifica.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("You do NOT have enough money to buy this beard style!", ColoreNotifica.Red, true);
                            }
                        }
                    }
                }
                else if (_listItem == makeupItem)
                {
                    BarberStyle obj = type.Makeup[_listItem.Index];

                    if (obj.var == -1 && obj.var == currentBeard)
                    {
                        HUD.ShowNotification("You can't remove makeup 2 times!!", ColoreNotifica.Red, true);
                    }
                    else if (obj.var == currentMakup && mkuOp.Percentage == currentMakupOp)
                    {
                        HUD.ShowNotification("You can't buy the style you already have! Try changing the style or changing the color!", ColoreNotifica.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.Makeup.Style = obj.var;
                            skin.Makeup.Opacity = mkuOp.Percentage;
                            currentMakup = obj.var;
                            currentMakupOp = mkuOp.Percentage;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("You paid with cash", ColoreNotifica.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.Makeup.Style = obj.var;
                                skin.Makeup.Opacity = mkuOp.Percentage;
                                currentMakup = obj.var;
                                currentMakupOp = mkuOp.Percentage;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("You paid by bank", ColoreNotifica.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("You do NOT have enough money to purchase this makeup!", ColoreNotifica.Red, true);
                            }
                        }
                    }
                }
                else if (_listItem == lipstickItem)
                {
                    BarberStyle obj = type.Lipstick[_listItem.Index];

                    if (obj.var == -1 && obj.var == currentLipst)
                    {
                        HUD.ShowNotification("You can't remove your lipstick twice!!", ColoreNotifica.Red, true);
                    }
                    else if (obj.var == currentLipst && lipstickColBase.CurrentSelection == currentLipstCol1 && lipstickColSec.CurrentSelection == currentLipstCol2 && lipstickOp.Percentage == currentLipstOp)
                    {
                        HUD.ShowNotification("You can't buy the style you already have! Try changing the style or changing the color!", ColoreNotifica.Red, true);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= obj.price)
                        {
                            skin.Lipstick.Style = obj.var;
                            skin.Lipstick.Color[0] = lipstickColBase.CurrentSelection;
                            skin.Lipstick.Color[1] = lipstickColSec.CurrentSelection;
                            skin.Lipstick.Opacity = lipstickOp.Percentage;
                            currentLipst = obj.var;
                            currentLipstCol1 = lipstickColBase.CurrentSelection;
                            currentLipstCol2 = lipstickColSec.CurrentSelection;
                            currentLipstOp = lipstickOp.Percentage;
                            BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 1);
                            await BaseScript.Delay(100);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                            HUD.ShowNotification("You paid with cash", ColoreNotifica.GreenDark);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= obj.price)
                            {
                                skin.Lipstick.Style = obj.var;
                                skin.Lipstick.Color[0] = lipstickColBase.CurrentSelection;
                                skin.Lipstick.Color[1] = lipstickColSec.CurrentSelection;
                                skin.Lipstick.Opacity = lipstickOp.Percentage;
                                currentLipst = obj.var;
                                currentLipstCol1 = lipstickColBase.CurrentSelection;
                                currentLipstCol2 = lipstickColSec.CurrentSelection;
                                currentLipstOp = lipstickOp.Percentage;
                                BaseScript.TriggerServerEvent("lprp:barbiere:compra", obj.price, 2);
                                await BaseScript.Delay(100);
                                //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skin", skin.ToJson());
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin = skin;
                                HUD.ShowNotification("You paid by bank", ColoreNotifica.GreenDark);
                            }
                            else
                            {
                                HUD.ShowNotification("You DO NOT have enough money to buy this lipstick!", ColoreNotifica.Red, true);
                            }
                        }
                    }
                }

                //				else if (_listItem == )
                //				{
                //				}
            };

            #endregion

            MainMenu.OnMenuClose += (a) =>
            {
                ClearPedTasks(PlayerPedId());
                Functions.UpdateFace(PlayerPedId(), skin);
                RenderScriptCams(false, true, 1000, true, false);
                Client.Instance.RemoveTick(newCam);
            };

            #endregion

            MainMenu.Visible = true;
            Client.Instance.AddTick(newCam);
        }

        private static float fov = 0;

        private static async Task newCam()
        {
            if (MainMenu.Visible)
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
 *              TODO: TO BE ADDED TO A NEW TICK WITH MENU OPEN... FOR ALL THE OTHER STYLES TOO
 *				if (beardItem.Hovered)
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