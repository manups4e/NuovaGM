using Settings.Shared.Roleplay.Jobs.WhiteList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using TheLastPlanet.Shared.Vehicles;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.CarDealer
{
    // void func_747(int iParam0) in carmod_shop.c
    internal static class CarDealer
    {
        private static ConfigCarDealer carDealer;
        private static Vehicle PreviewVeh;
        private static Blip vend;

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            carDealer = Client.Settings.RolePlay.Jobs.CarDealer;
            Client.Instance.AddEventHandler("lprp:cardealer:catalogoAlcuni", new Action<bool, List<int>>(CatalogueSome));
            Client.Instance.AddEventHandler("lprp:cardealer:cambiaVehCatalogo", new Action<bool, string>(ChangeVehCatalogue));
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            carDealer = null;
            Client.Instance.RemoveEventHandler("lprp:cardealer:catalogoAlcuni", new Action<bool, List<int>>(CatalogueSome));
            Client.Instance.RemoveEventHandler("lprp:cardealer:cambiaVehCatalogo", new Action<bool, string>(ChangeVehCatalogue));
            vend.Delete();
        }

        private static void Spawned(PlayerClient client)
        {
            vend = World.CreateBlip(carDealer.Config.SellMenu.ToVector3);
            vend.Sprite = BlipSprite.PersonalVehicleCar;
            vend.Color = BlipColor.Green;
            vend.IsShortRange = true;
            vend.Name = "Car dealer";
        }

        public static async Task Markers()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "cardealer")
            {
                // TODO: CHANGE WITH SITTING DOWN AND SHOWING THE CLIENT
                if (p.IsInRangeOf(carDealer.Config.SellMenu.ToVector3, 1.375f))
                {
                    HUD.ShowHelp("Press ~INPUT_CONTEXT~ to open seller menu");
                    if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) MenuVenditore();
                }
            }

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade > 1)
            {
                // TODO: CHANGE WITH SITTING ON THE DESK 
                if (p.IsInRangeOf(carDealer.Config.BossActions.ToVector3, 1.375f))
                {
                    HUD.ShowHelp("Press ~INPUT_CONTEXT~ to open the boss menu");
                    if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) MenuBoss();
                }
            }
        }

        private static async void MenuVenditore()
        {
            UIMenu dealerMenu = new UIMenu("Dealer Menu", "Everything you need just a click away", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);

            // TODO: CHECK WILL BE AUTOMATIC WHEN BYUING, MAYBE NO CHECK BUT AUTOMATIC BYUING
            // WON'T BE AVAILABLE FOR REND THAT WILL STARTT AS SOON AS THE VEHICLE IS GIVEN
            // la fattura sarà automatica all'acquisto da parte dell'acquirente (magari non ci sarà fattura ma acquisto automatico)
            // non sarà disponibile per l'affitto che partirà dalla consegna del veicolo
            UIMenuItem catalogPriv = new UIMenuItem("Check catalog", "Only for you");
            dealerMenu.AddItem(catalogPriv);
            catalogPriv.Activated += async (menu, item) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                ShowMe();
            };
            UIMenuItem showCatalogItem = new UIMenuItem("Show catalog", "Choose who");
            UIMenu showCatalog = new("Catalog", "Everything you need just a click away");
            dealerMenu.AddItem(showCatalogItem);
            showCatalogItem.Activated += async (a, b) => await dealerMenu.SwitchTo(showCatalog, 0, true);
            List<Player> players = new();
            showCatalog.OnMenuOpen += (a, b) =>
            {
                a.Clear();
                players.Clear();
                players = Functions.GetPlayersInArea(Cache.PlayerCache.MyPlayer.Position.ToVector3, 3f);
                List<string> texts = players.Select(x => x.GetPlayerData().FullName).ToList();
                string txt = "";
                foreach (string t in texts) txt = t + "~n~";
                UIMenuItem ShowCatalogSomeItem = new UIMenuItem("Show by choice");
                UIMenu showCatalogSome = new("Show by choice", "Everything you need just a click away");
                showCatalog.AddItem(ShowCatalogSomeItem);
                ShowCatalogSomeItem.Activated += async (a, b) => await showCatalog.SwitchTo(showCatalogSome, 0, true);
                if (players.Count == 0)
                {
                    ShowCatalogSomeItem.Enabled = false;
                    ShowCatalogSomeItem.Description = "No one near you!";
                }

                showCatalog.OnMenuOpen += async (a, b) =>
                {
                    a.Clear();
                    List<int> people = new List<int>();

                    foreach (Player p in players)
                    {
                        UIMenuCheckboxItem persona = new(p.GetPlayerData().FullName, false);
                        persona.CheckboxEvent += (_item, _activated) =>
                        {
                            if (!_activated) return;
                            if (!people.Contains(p.ServerId))
                                people.Add(p.ServerId);
                            else if (people.Contains(p.ServerId)) people.Remove(p.ServerId);
                        };
                    }

                    UIMenuItem show = new UIMenuItem("Show selected", "", SColor.HUD_Greendark, SColor.HUD_Green);
                    a.AddItem(show);
                    show.Activated += (menu, item) =>
                    {
                        if (people.Count == 0)
                        {
                            HUD.ShowNotification("No one selected!");

                            return;
                        }

                        BaseScript.TriggerEvent("lprp:cardealer:attivaCatalogoAlcuni", people);
                    };
                };
            };

            UIMenuItem buyItem = new UIMenuItem("Buy used vehicle");
            UIMenu buy = new("Buy used vehicle", "Everything you need just a click away");
            dealerMenu.AddItem(buyItem);
            buyItem.Activated += async (a, b) => await dealerMenu.SwitchTo(buy, 0, true);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade < 2)
            {
                buyItem.Enabled = false;
                buyItem.Description = "Only the boss can buy used vehicles!";
            }

            dealerMenu.Visible = true;
        }

        private static async void MenuBoss() { }

        private static async void ShowMe()
        {
            MenuHandler.CloseAndClearHistory();
            LoadInterior(146433);
            SetInteriorActive(146433, true);
            RequestCollisionAtCoord(230.2893f, -996.1444f, -96.08697f);
            Camera cam = World.CreateCamera(new Vector3(230.2893f, -996.1444f, -96.08697f), Vector3.Zero, 45f);
            World.RenderingCamera = cam;
            cam.Position = new Vector3(230.2893f, -996.1444f, -98.08697f);
            cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
            UIMenu catalogue = new UIMenu("Dealer catalogue", "Your trusted catalogue", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            Dictionary<string, List<VehiclesCatalog>> Catalogue = new Dictionary<string, List<VehiclesCatalog>>();
            string SelectedVeh = "";

            foreach (KeyValuePair<string, List<VehiclesCatalog>> p in carDealer.Catalog.Keys.OrderBy(k => k).ToDictionary(k => k, T1 => carDealer.Catalog[T1]))
            {
                UIMenuItem sezioneItem = new UIMenuItem(p.Key);
                UIMenu section = new(p.Key, "");
                sezioneItem.Activated += async (a, b) => await catalogue.SwitchTo(section, 0, true);
                catalogue.AddItem(sezioneItem);
                section.InstructionalButtons.Add(new InstructionalButton(Control.ParachuteBrakeLeft, "Open/Close Vehicle"));
                List<VehiclesCatalog> vehs = new();

                foreach (VehiclesCatalog i in p.Value.OrderBy(x => x.Price))
                {
                    UIMenuItem vahItem = new UIMenuItem(Game.GetGXTEntry(i.Name), i.Description);
                    vahItem.SetRightLabel("~g~$" + i.Price);
                    UIMenu vah = new(Game.GetGXTEntry(i.Name), "");
                    vahItem.Activated += async (a, b) => await section.SwitchTo(vah, 0, true);
                    section.AddItem(vahItem);
                    vehs.Add(i);

                    UIMenuItem color1Item = new("Main Color");
                    UIMenuItem color2Item = new("Secondary Color");

                    UIMenu color1 = new("Main Color", "");
                    UIMenu color2 = new("Secondary Color", "");

                    vah.AddItem(color1Item);
                    vah.AddItem(color2Item);

                    color1Item.Activated += async (a, b) => await vah.SwitchTo(color1, 0, true);
                    color2Item.Activated += async (a, b) => await vah.SwitchTo(color2, 0, true);

                    for (int l = 0; l < Enum.GetValues(typeof(VehicleColor)).Length; l++)
                    {
                        UIMenuItem colo = new UIMenuItem(Functions.GetVehColorLabel(l));
                        color1.AddItem(colo);
                        color2.AddItem(colo);
                    }

                    color1.OnIndexChange += (menu, index) => PreviewVeh.Mods.PrimaryColor = (VehicleColor)index;
                    color2.OnIndexChange += (menu, index) => PreviewVeh.Mods.SecondaryColor = (VehicleColor)index;

                    UIMenuItem prendiItem = new("Prendi");
                    UIMenu prendi = new("Prendi", "");
                    vah.AddItem(prendiItem);
                    prendiItem.Activated += async (a, b) => await vah.SwitchTo(prendi, 0, true);
                    prendi.OnMenuOpen += async (_newsubmenu, b) =>
                    {
                        _newsubmenu.Clear();

                        if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Properties.Any(x => Client.Settings.RolePlay.Properties.Garages.Garages.ContainsKey(x) || Client.Settings.RolePlay.Properties.Apartments.ContainsKey(x)))
                        {
                            foreach (KeyValuePair<string, ConfigHouses> pro in Client.Settings.RolePlay.Properties.Apartments)
                                if (pro.Value.GarageIncluded)
                                    foreach (UIMenuItem c in from a in Cache.PlayerCache.MyPlayer.User.CurrentChar.Properties where a == pro.Key select new UIMenuItem(pro.Value.Label))
                                    {
                                        c.SetRightLabel("" + Cache.PlayerCache.MyPlayer.User.CurrentChar.Vehicles.Where(x => x.Garage.Garage == pro.Key).ToList().Count + "/" + Client.Settings.RolePlay.Properties.Apartments[pro.Key].VehCapacity);
                                        prendi.AddItem(c);
                                        c.Activated += async (_menu_, _item_) =>
                                        {
                                            string s1 = SharedMath.GetRandomString(2);
                                            await BaseScript.Delay(100);
                                            string s2 = SharedMath.GetRandomString(2);
                                            string plate = s1 + " " + SharedMath.GetRandomInt(001, 999).ToString("000") + s2;
                                            PreviewVeh.Mods.LicensePlate = plate;
                                            VehProp prop = await PreviewVeh.GetVehicleProperties();
                                            OwnedVehicle veicolo = new OwnedVehicle(PreviewVeh, plate, new VehicleData(Cache.PlayerCache.MyPlayer.User.CurrentChar.Info.Insurance, prop, false), new VehGarage(true, pro.Key, Cache.PlayerCache.MyPlayer.User.CurrentChar.Vehicles.Where(x => x.Garage.Garage == pro.Key).ToList().Count), "Normale");
                                            BaseScript.TriggerServerEvent("lprp:cardealer:vendiVehAMe", veicolo.ToJson(settings: JsonHelper.IgnoreJsonIgnoreAttributes));
                                            MenuHandler.CloseAndClearHistory();
                                            HUD.ShowNotification($"You bought: ~y~{veicolo.VehData.Props.Name}~w~ for ~g~${prendiItem.RightLabel}~w~.");
                                            Screen.Fading.FadeOut(800);
                                            await BaseScript.Delay(1000);
                                            World.RenderingCamera = null;
                                            cam.Delete();
                                            Screen.Fading.FadeIn(800);
                                        };
                                    }
                        }
                        else
                        {
                            UIMenuItem no = new UIMenuItem("you have no garages!!");
                            prendi.AddItem(no);
                        }
                    };
                }

                section.OnIndexChange += async (menu, index) =>
                {
                    PreviewVeh = await Functions.SpawnLocalVehicle(vehs[index].Name, new Vector3(228.9409f, -989.8207f, -99.99992f), -180f);
                    cam.PointAt(PreviewVeh);
                    PreviewVeh.IsEngineRunning = true;
                    PreviewVeh.AreLightsOn = true;
                    PreviewVeh.IsInteriorLightOn = true;
                    PreviewVeh.IsPositionFrozen = true;
                    PreviewVeh.IsLeftIndicatorLightOn = true;
                    PreviewVeh.IsRightIndicatorLightOn = true;
                };
                section.OnItemSelect += async (menu, item, index) => SelectedVeh = vehs[index].Name;

                section.OnMenuOpen += (a, b) => Client.Instance.AddTick(RotateVeh);
                section.OnMenuClose += async (a) =>
                {
                    await BaseScript.Delay(100);

                    if (catalogue.Visible)
                    {
                        Client.Instance.RemoveTick(RotateVeh);
                        PreviewVeh.Delete();
                        cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
                    }
                };
            }

            catalogue.OnMenuClose += async (a) =>
            {
                await BaseScript.Delay(100);

                if (MenuHandler.IsAnyMenuOpen) return;
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                World.RenderingCamera = null;
                cam.Delete();
                Screen.Fading.FadeIn(800);

            };
            Screen.Fading.FadeIn(800);
            catalogue.Visible = true;
        }

        private static async void CatalogueSome(bool seller, List<int> players)
        {
            MenuHandler.CloseAndClearHistory();
            LoadInterior(146433);
            SetInteriorActive(146433, true);
            RequestCollisionAtCoord(230.2893f, -996.1444f, -96.08697f);
            Camera cam = World.CreateCamera(new Vector3(230.2893f, -996.1444f, -96.08697f), Vector3.Zero, 45f);
            World.RenderingCamera = cam;
            cam.Position = new Vector3(230.2893f, -996.1444f, -98.08697f);
            cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));

            if (seller)
            {
                UIMenu catalogue = new UIMenu("Dealer catalogue", "Your trusted catalogue", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
                Dictionary<string, List<VehiclesCatalog>> Catalogue = new Dictionary<string, List<VehiclesCatalog>>();

                foreach (KeyValuePair<string, List<VehiclesCatalog>> p in carDealer.Catalog.Keys.OrderBy(k => k).ToDictionary(k => k, T1 => carDealer.Catalog[T1]))
                {

                    UIMenuItem sezioneItem = new UIMenuItem(p.Key);
                    UIMenu section = new(p.Key, "");
                    sezioneItem.Activated += async (a, b) => await catalogue.SwitchTo(section, 0, true);
                    catalogue.AddItem(sezioneItem);
                    section.InstructionalButtons.Add(new InstructionalButton(Control.ParachuteBrakeLeft, "Open/Close Vehicle"));
                    List<VehiclesCatalog> vehs = new List<VehiclesCatalog>();

                    foreach (VehiclesCatalog i in p.Value.OrderBy(x => x.Price))
                    {
                        UIMenuItem vahItem = new UIMenuItem(Game.GetGXTEntry(i.Name), i.Description);
                        vahItem.SetRightLabel("~g~$" + i.Price);
                        UIMenu vah = new(Game.GetGXTEntry(i.Name), "");
                        vahItem.Activated += async (a, b) => await section.SwitchTo(vah, 0, true);
                        section.AddItem(vahItem);
                        vehs.Add(i);

                        foreach (int pl in players)
                        {
                            Player user = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == pl);
                            UIMenuItem playerItem = new(user.GetPlayerData().FullName);
                            UIMenu player = new(user.GetPlayerData().FullName, "");
                            vah.AddItem(playerItem);
                            playerItem.Activated += async (a, b) => vah.SwitchTo(player, 0, true);

                            if (user.GetPlayerData().CurrentChar.Properties.Any(x => Client.Settings.RolePlay.Properties.Garages.Garages.ContainsKey(x) || (Client.Settings.RolePlay.Properties.Apartments.GroupBy(l => l.Value.GarageIncluded == true) as Dictionary<string, ConfigHouses>).ContainsKey(x)))
                            {
                                List<string> prop = new();

                                foreach (string gar in user.GetPlayerData().CurrentChar.Properties)
                                {
                                    if (Client.Settings.RolePlay.Properties.Garages.Garages.ContainsKey(gar))
                                    {
                                        UIMenuItem posto = new(Client.Settings.RolePlay.Properties.Garages.Garages[gar].Label);
                                        player.AddItem(posto);
                                    }
                                    else if (Client.Settings.RolePlay.Properties.Apartments.ContainsKey(gar) && Client.Settings.RolePlay.Properties.Apartments[gar].GarageIncluded)
                                    {
                                        UIMenuItem posto = new(Client.Settings.RolePlay.Properties.Apartments[gar].Label);
                                        player.AddItem(posto);
                                    }

                                    player.OnItemSelect += (menu, item, index) => BaseScript.TriggerServerEvent("lprp:carDealer:vendi", user.ServerId, item.Label);
                                }
                            }
                            else
                            {
                                playerItem.Enabled = false;
                                playerItem.Description = "This person doesn't have a garage!";
                            }
                        }
                    }

                    section.OnIndexChange += (menu, index) => BaseScript.TriggerServerEvent("lprp:cardealer:cambiaVehCatalogo", players, vehs[index].Name);
                    section.OnMenuClose += (a) =>
                    {
                        Client.Instance.RemoveTick(RotateVeh);
                        PreviewVeh.Delete();
                        cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
                    };
                }

                catalogue.OnMenuClose += async (a) =>
                {
                    await BaseScript.Delay(100);

                    if (MenuHandler.IsAnyMenuOpen) return;
                    Screen.Fading.FadeOut(800);
                    await BaseScript.Delay(1000);
                    World.RenderingCamera = null;
                    cam.Delete();
                    Screen.Fading.FadeIn(800);
                };
                catalogue.Visible = true;
            }

            Client.Instance.AddTick(RotateVeh);
            Screen.Fading.FadeIn(800);
        }

        private static async void ChangeVehCatalogue(bool venditore, string name)
        {
            PreviewVeh = await Functions.SpawnLocalVehicle(name, new Vector3(228.9409f, -989.8207f, -99.99992f), -180f);
            PreviewVeh.IsEngineRunning = true;
            PreviewVeh.AreLightsOn = true;
            PreviewVeh.IsInteriorLightOn = true;
            PreviewVeh.IsPositionFrozen = true;
            PreviewVeh.IsLeftIndicatorLightOn = true;
            PreviewVeh.IsRightIndicatorLightOn = true;
        }

        private static async Task RotateVeh()
        {
            if (PreviewVeh != null && PreviewVeh.Exists())
            {
                Game.DisableControlThisFrame(0, Control.ParachuteBrakeLeft);
                PreviewVeh.Heading += 0.2f;

                if (Input.IsDisabledControlJustPressed(Control.ParachuteBrakeLeft))
                {
                    if (PreviewVeh.Model.IsCar)
                    {
                        PreviewVeh.IsPositionFrozen = false;

                        foreach (VehicleDoor d in PreviewVeh.Doors.GetAll())
                        {
                            d.CanBeBroken = false;
                            if (d.IsOpen)
                                d.Close();
                            else
                                d.Open();
                            await BaseScript.Delay(500);
                        }

                        PreviewVeh.IsPositionFrozen = true;
                    }
                }
            }
        }
    }
}