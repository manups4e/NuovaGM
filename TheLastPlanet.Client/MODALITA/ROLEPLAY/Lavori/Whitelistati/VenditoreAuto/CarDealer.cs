using Settings.Shared.Roleplay.Jobs.WhiteList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using TheLastPlanet.Shared.Vehicles;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Whitelistati.VenditoreAuto
{
    // void func_747(int iParam0) in carmod_shop.c
    internal static class CarDealer
    {
        private static ConfigCarDealer carDealer;
        private static Vehicle PreviewVeh;
        private static Blip vend;

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            carDealer = Client.Impostazioni.RolePlay.Jobs.CarDealer;
            Client.Instance.AddEventHandler("lprp:cardealer:catalogoAlcuni", new Action<bool, List<int>>(CatalogoAlcuni));
            Client.Instance.AddEventHandler("lprp:cardealer:cambiaVehCatalogo", new Action<bool, string>(CambiaVehCatalogo));
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            carDealer = null;
            Client.Instance.RemoveEventHandler("lprp:cardealer:catalogoAlcuni", new Action<bool, List<int>>(CatalogoAlcuni));
            Client.Instance.RemoveEventHandler("lprp:cardealer:cambiaVehCatalogo", new Action<bool, string>(CambiaVehCatalogo));
            vend.Delete();
        }

        private static void Spawnato(PlayerClient client)
        {
            vend = World.CreateBlip(carDealer.Config.MenuVendita.ToVector3);
            vend.Sprite = BlipSprite.PersonalVehicleCar;
            vend.Color = BlipColor.Green;
            vend.IsShortRange = true;
            vend.Name = "Concessionaria";
        }

        public static async Task Markers()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "cardealer")
                // verrà sostiuito con il sedersi alla scrivania e mostrare al cliente
                if (p.IsInRangeOf(carDealer.Config.MenuVendita.ToVector3, 1.375f))
                {
                    HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu del venditore");
                    if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) MenuVenditore();
                }

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade > 1)
                // verrà sostiuito con il sedersi alla scrivania 
                if (p.IsInRangeOf(carDealer.Config.BossActions.ToVector3, 1.375f))
                {
                    HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu boss");
                    if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) MenuBoss();
                }
        }

        private static async void MenuVenditore()
        {
            UIMenu menuVenditore = new UIMenu("Menu Venditore", "Tutto l'occorrente a portata di click", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);

            // la fattura sarà automatica all'acquisto da parte dell'acquirente (magari non ci sarà fattura ma acquisto automatico)
            // non sarà disponibile per l'affitto che partirà dalla consegna del veicolo
            UIMenuItem catalogoPriv = new UIMenuItem("Guarda catalogo", "Solo per te");
            menuVenditore.AddItem(catalogoPriv);
            catalogoPriv.Activated += async (menu, item) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                MostraAMe();
            };
            UIMenuItem mostraCatalogoItem = new UIMenuItem("Mostra catalogo", "Scegli a chi");
            UIMenu mostraCatalogo = new("Catalogo", "");
            menuVenditore.AddItem(mostraCatalogoItem);
            mostraCatalogoItem.Activated += async (a, b) => await menuVenditore.SwitchTo(mostraCatalogo, 0, true);
            List<Player> players = new();
            mostraCatalogo.OnMenuOpen += (a, b) =>
            {
                a.Clear();
                players.Clear();
                players = Funzioni.GetPlayersInArea(Cache.PlayerCache.MyPlayer.Posizione.ToVector3, 3f);
                List<string> texts = players.Select(x => x.GetPlayerData().FullName).ToList();
                string txt = "";
                foreach (string t in texts) txt = t + "~n~";
                UIMenuItem mostraCatalogoAlcuniItem = new UIMenuItem("Mostra a scelta");
                UIMenu mostraCatalogoAlcuni = new("Mostra a scelta", "");
                mostraCatalogo.AddItem(mostraCatalogoAlcuniItem);
                mostraCatalogoAlcuniItem.Activated += async (a, b) => await mostraCatalogo.SwitchTo(mostraCatalogoAlcuni, 0, true);
                if (players.Count == 0)
                {
                    mostraCatalogoAlcuniItem.Enabled = false;
                    mostraCatalogoAlcuniItem.Description = "Non hai persone vicino!";
                }

                mostraCatalogo.OnMenuOpen += async (a, b) =>
                {
                    a.Clear();
                    List<int> persone = new List<int>();

                    foreach (Player p in players)
                    {
                        UIMenuCheckboxItem persona = new(p.GetPlayerData().FullName, false);
                        persona.CheckboxEvent += (_item, _activated) =>
                        {
                            if (!_activated) return;
                            if (!persone.Contains(p.ServerId))
                                persone.Add(p.ServerId);
                            else if (persone.Contains(p.ServerId)) persone.Remove(p.ServerId);
                        };
                    }

                    UIMenuItem mostra = new UIMenuItem("Mostra a selezionati", "", SColor.HUD_Greendark, SColor.HUD_Green);
                    a.AddItem(mostra);
                    mostra.Activated += (menu, item) =>
                    {
                        if (persone.Count == 0)
                        {
                            HUD.ShowNotification("Non hai selezionato nessuno!");

                            return;
                        }

                        BaseScript.TriggerEvent("lprp:cardealer:attivaCatalogoAlcuni", persone);
                    };
                };
            };

            UIMenuItem riacquistaItem = new UIMenuItem("Acquista veicolo usato");
            UIMenu riacquista = new("Acquista veicolo usato", "");
            menuVenditore.AddItem(riacquistaItem);
            riacquistaItem.Activated += async (a, b) => await menuVenditore.SwitchTo(riacquista, 0, true);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade < 2)
            {
                riacquistaItem.Enabled = false;
                riacquistaItem.Description = "Solo i capi possono acquistare i veicoli usati!";
            }

            menuVenditore.Visible = true;
        }

        private static async void MenuBoss() { }

        private static async void MostraAMe()
        {
            MenuHandler.CloseAndClearHistory();
            LoadInterior(146433);
            SetInteriorActive(146433, true);
            RequestCollisionAtCoord(230.2893f, -996.1444f, -96.08697f);
            Camera cam = World.CreateCamera(new Vector3(230.2893f, -996.1444f, -96.08697f), Vector3.Zero, 45f);
            World.RenderingCamera = cam;
            cam.Position = new Vector3(230.2893f, -996.1444f, -98.08697f);
            cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
            UIMenu catalogo = new UIMenu("Catalogo concessionaria", "Il tuo catalogo di fiducia", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            Dictionary<string, List<VehiclesCatalog>> Catalogo = new Dictionary<string, List<VehiclesCatalog>>();
            string SelectedVeh = "";

            foreach (KeyValuePair<string, List<VehiclesCatalog>> p in carDealer.Catalog.Keys.OrderBy(k => k).ToDictionary(k => k, T1 => carDealer.Catalog[T1]))
            {
                UIMenuItem sezioneItem = new UIMenuItem(p.Key);
                UIMenu sezione = new(p.Key, "");
                sezioneItem.Activated += async (a, b) => await catalogo.SwitchTo(sezione, 0, true);
                catalogo.AddItem(sezioneItem);
                sezione.InstructionalButtons.Add(new InstructionalButton(Control.ParachuteBrakeLeft, "Apri/Chiudi veicolo"));
                List<VehiclesCatalog> vehs = new();

                foreach (VehiclesCatalog i in p.Value.OrderBy(x => x.price))
                {
                    UIMenuItem vahItem = new UIMenuItem(Game.GetGXTEntry(i.name), i.description);
                    vahItem.SetRightLabel("~g~$" + i.price);
                    UIMenu vah = new(Game.GetGXTEntry(i.name), "");
                    vahItem.Activated += async (a, b) => await sezione.SwitchTo(vah, 0, true);
                    sezione.AddItem(vahItem);
                    vehs.Add(i);

                    UIMenuItem colore1Item = new("Colore primario");
                    UIMenuItem colore2Item = new("Colore secondario");

                    UIMenu colore1 = new("Colore primario", "");
                    UIMenu colore2 = new("Colore secondario", "");

                    vah.AddItem(colore1Item);
                    vah.AddItem(colore2Item);

                    colore1Item.Activated += async (a, b) => await vah.SwitchTo(colore1, 0, true);
                    colore2Item.Activated += async (a, b) => await vah.SwitchTo(colore2, 0, true);

                    for (int l = 0; l < Enum.GetValues(typeof(VehicleColor)).Length; l++)
                    {
                        UIMenuItem colo = new UIMenuItem(Funzioni.GetVehColorLabel(l));
                        colore1.AddItem(colo);
                        colore2.AddItem(colo);
                    }

                    colore1.OnIndexChange += (menu, index) => PreviewVeh.Mods.PrimaryColor = (VehicleColor)index;
                    colore2.OnIndexChange += (menu, index) => PreviewVeh.Mods.SecondaryColor = (VehicleColor)index;

                    UIMenuItem prendiItem = new("Prendi");
                    UIMenu prendi = new("Prendi", "");
                    vah.AddItem(prendiItem);
                    prendiItem.Activated += async (a, b) => vah.SwitchTo(prendi, 0, true);
                    prendi.OnMenuOpen += async (_newsubmenu, b) =>
                    {
                        _newsubmenu.Clear();

                        if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Properties.Any(x => Client.Impostazioni.RolePlay.Properties.Garages.Garages.ContainsKey(x) || Client.Impostazioni.RolePlay.Properties.Apartments.ContainsKey(x)))
                        {
                            foreach (KeyValuePair<string, ConfigHouses> pro in Client.Impostazioni.RolePlay.Properties.Apartments)
                                if (pro.Value.GarageIncluded)
                                    foreach (UIMenuItem c in from a in Cache.PlayerCache.MyPlayer.User.CurrentChar.Properties where a == pro.Key select new UIMenuItem(pro.Value.Label))
                                    {
                                        c.SetRightLabel("" + Cache.PlayerCache.MyPlayer.User.CurrentChar.Vehicles.Where(x => x.Garage.Garage == pro.Key).ToList().Count + "/" + Client.Impostazioni.RolePlay.Properties.Apartments[pro.Key].VehCapacity);
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
                                            HUD.ShowNotification($"Hai comprato il veicolo: ~y~{veicolo.VehData.Props.Name}~w~ al prezzo di ~g~${prendiItem.RightLabel}~w~.");
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
                            UIMenuItem no = new UIMenuItem("Non hai appartamenti o garage!!");
                            prendi.AddItem(no);
                        }
                    };
                }

                sezione.OnIndexChange += async (menu, index) =>
                {
                    PreviewVeh = await Funzioni.SpawnLocalVehicle(vehs[index].name, new Vector3(228.9409f, -989.8207f, -99.99992f), -180f);
                    cam.PointAt(PreviewVeh);
                    PreviewVeh.IsEngineRunning = true;
                    PreviewVeh.AreLightsOn = true;
                    PreviewVeh.IsInteriorLightOn = true;
                    PreviewVeh.IsPositionFrozen = true;
                    PreviewVeh.IsLeftIndicatorLightOn = true;
                    PreviewVeh.IsRightIndicatorLightOn = true;
                };
                sezione.OnItemSelect += async (menu, item, index) => SelectedVeh = vehs[index].name;

                sezione.OnMenuOpen += (a, b) => Client.Instance.AddTick(RuotaVeh);
                sezione.OnMenuClose += async (a) =>
                {
                    await BaseScript.Delay(100);

                    if (catalogo.Visible)
                    {
                        Client.Instance.RemoveTick(RuotaVeh);
                        PreviewVeh.Delete();
                        cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
                    }
                };
            }

            catalogo.OnMenuClose += async (a) =>
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
            catalogo.Visible = true;
        }

        private static async void CatalogoAlcuni(bool venditore, List<int> players)
        {
            MenuHandler.CloseAndClearHistory();
            LoadInterior(146433);
            SetInteriorActive(146433, true);
            RequestCollisionAtCoord(230.2893f, -996.1444f, -96.08697f);
            Camera cam = World.CreateCamera(new Vector3(230.2893f, -996.1444f, -96.08697f), Vector3.Zero, 45f);
            World.RenderingCamera = cam;
            cam.Position = new Vector3(230.2893f, -996.1444f, -98.08697f);
            cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));

            if (venditore)
            {
                UIMenu catalogo = new UIMenu("Catalogo concessionaria", "Il tuo catalogo di fiducia", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
                Dictionary<string, List<VehiclesCatalog>> Catalogo = new Dictionary<string, List<VehiclesCatalog>>();

                foreach (KeyValuePair<string, List<VehiclesCatalog>> p in carDealer.Catalog.Keys.OrderBy(k => k).ToDictionary(k => k, T1 => carDealer.Catalog[T1]))
                {

                    UIMenuItem sezioneItem = new UIMenuItem(p.Key);
                    UIMenu sezione = new(p.Key, "");
                    sezioneItem.Activated += async (a, b) => await catalogo.SwitchTo(sezione, 0, true);
                    catalogo.AddItem(sezioneItem);
                    sezione.InstructionalButtons.Add(new InstructionalButton(Control.ParachuteBrakeLeft, "Apri/Chiudi veicolo"));
                    List<VehiclesCatalog> vehs = new List<VehiclesCatalog>();

                    foreach (VehiclesCatalog i in p.Value.OrderBy(x => x.price))
                    {
                        UIMenuItem vahItem = new UIMenuItem(Game.GetGXTEntry(i.name), i.description);
                        vahItem.SetRightLabel("~g~$" + i.price);
                        UIMenu vah = new(Game.GetGXTEntry(i.name), "");
                        vahItem.Activated += async (a, b) => await sezione.SwitchTo(vah, 0, true);
                        sezione.AddItem(vahItem);
                        vehs.Add(i);

                        foreach (int pl in players)
                        {
                            Player user = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == pl);
                            UIMenuItem playerItem = new(user.GetPlayerData().FullName);
                            UIMenu player = new(user.GetPlayerData().FullName, "");
                            vah.AddItem(playerItem);
                            playerItem.Activated += async (a, b) => vah.SwitchTo(player, 0, true);

                            if (user.GetPlayerData().CurrentChar.Properties.Any(x => Client.Impostazioni.RolePlay.Properties.Garages.Garages.ContainsKey(x) || (Client.Impostazioni.RolePlay.Properties.Apartments.GroupBy(l => l.Value.GarageIncluded == true) as Dictionary<string, ConfigHouses>).ContainsKey(x)))
                            {
                                List<string> prop = new();

                                foreach (string gar in user.GetPlayerData().CurrentChar.Properties)
                                {
                                    if (Client.Impostazioni.RolePlay.Properties.Garages.Garages.ContainsKey(gar))
                                    {
                                        UIMenuItem posto = new(Client.Impostazioni.RolePlay.Properties.Garages.Garages[gar].Label);
                                        player.AddItem(posto);
                                    }
                                    else if (Client.Impostazioni.RolePlay.Properties.Apartments.ContainsKey(gar) && Client.Impostazioni.RolePlay.Properties.Apartments[gar].GarageIncluded)
                                    {
                                        UIMenuItem posto = new(Client.Impostazioni.RolePlay.Properties.Apartments[gar].Label);
                                        player.AddItem(posto);
                                    }

                                    player.OnItemSelect += (menu, item, index) => BaseScript.TriggerServerEvent("lprp:carDealer:vendi", user.ServerId, item.Label);
                                }
                            }
                            else
                            {
                                playerItem.Enabled = false;
                                playerItem.Description = "Questa persona non ha proprietà con garage!";
                            }
                        }
                    }

                    sezione.OnIndexChange += (menu, index) => BaseScript.TriggerServerEvent("lprp:cardealer:cambiaVehCatalogo", players, vehs[index].name);
                    sezione.OnMenuClose += (a) =>
                    {
                        Client.Instance.RemoveTick(RuotaVeh);
                        PreviewVeh.Delete();
                        cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
                    };
                }

                catalogo.OnMenuClose += async (a) =>
                {
                    await BaseScript.Delay(100);

                    if (MenuHandler.IsAnyMenuOpen) return;
                    Screen.Fading.FadeOut(800);
                    await BaseScript.Delay(1000);
                    World.RenderingCamera = null;
                    cam.Delete();
                    Screen.Fading.FadeIn(800);
                };
                catalogo.Visible = true;
            }

            Client.Instance.AddTick(RuotaVeh);
            Screen.Fading.FadeIn(800);
        }

        private static async void CambiaVehCatalogo(bool venditore, string name)
        {
            PreviewVeh = await Funzioni.SpawnLocalVehicle(name, new Vector3(228.9409f, -989.8207f, -99.99992f), -180f);
            PreviewVeh.IsEngineRunning = true;
            PreviewVeh.AreLightsOn = true;
            PreviewVeh.IsInteriorLightOn = true;
            PreviewVeh.IsPositionFrozen = true;
            PreviewVeh.IsLeftIndicatorLightOn = true;
            PreviewVeh.IsRightIndicatorLightOn = true;
        }

        private static async Task RuotaVeh()
        {
            if (PreviewVeh != null && PreviewVeh.Exists())
            {
                Game.DisableControlThisFrame(0, Control.ParachuteBrakeLeft);
                PreviewVeh.Heading += 0.2f;

                if (Input.IsDisabledControlJustPressed(Control.ParachuteBrakeLeft))
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