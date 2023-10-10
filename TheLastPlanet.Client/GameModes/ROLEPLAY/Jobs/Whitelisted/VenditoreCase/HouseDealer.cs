using Settings.Shared.Roleplay.Jobs.WhiteList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.HouseDealer
{
    internal static class HouseDealer
    {
        private static ConfigHouseDealer house;
        private static InputController input = new InputController(Control.Context, ServerMode.Roleplay, PadCheck.Keyboard, ControlModifier.Shift, new Action<Ped, object[]>(Test));
        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            house = Client.Settings.RolePlay.Jobs.RealEstate;
            Handlers.InputHandler.AddInput(input);
        }

        private static void Spawned(PlayerClient client)
        {
            Client.Instance.AddTick(Markers);
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveTick(Markers);
            house = null;
            Handlers.InputHandler.RemoveInput(input);
        }

        private static void Test(Ped playerPed, object[] args)
        {
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "realestate") HouseCreationMenu.MenuHousesCreation();
        }

        private static async Task Markers()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (!PlayerCache.MyPlayer.Status.Instance.Instanced)
            {
                World.DrawMarker(MarkerType.VerticalCylinder, house.Config.Entrance.ToVector3, Vector3.Zero, Vector3.Zero, new Vector3(1.375f, 1.375f, 0.4f), Colors.Blue);

                if (p.IsInRangeOf(house.Config.Entrance.ToVector3, 1.375f))
                {
                    HUD.ShowHelp(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "realestate" ?
                        "Press ~INPUT_CONTEXT~ to enter the office" :
                        "Press ~INPUT_CONTEXT~ to enter the Real Estate office");

                    if (Input.IsControlJustPressed(Control.Context))
                    {
                        Functions.Teleport(house.Config.Inside.ToVector3);
                        Cache.PlayerCache.MyPlayer.Status.Instance.InstancePlayer("RealEstate");
                    }
                }
            }
            else
            {
                if (Cache.PlayerCache.MyPlayer.Status.Instance.Instance == "RealEstate")
                {
                    World.DrawMarker(MarkerType.VerticalCylinder, house.Config.Exit.ToVector3, Vector3.Zero, Vector3.Zero, new Vector3(1.375f, 1.375f, 0.4f), Colors.Red);

                    if (p.IsInRangeOf(house.Config.Exit.ToVector3, 1.375f))
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to exit");

                        if (Input.IsControlJustPressed(Control.Context))
                        {
                            Functions.Teleport(house.Config.Outside.ToVector3);
                            Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
                        }
                    }
                }
            }

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "realestate")
            {
                // TODO: CHANGE IT WITH SITTING ON THE DESK
                // verrà cambiato con il sedersi alla scrivania
                if (p.IsInRangeOf(house.Config.Actions.ToVector3, 1.375f))
                {
                    HUD.ShowHelp("~INPUT_CONTEXT~ open selling menu");
                    if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) MenuRealEstate();
                }
            }

            await Task.FromResult(0);
        }

        private static async void MenuRealEstate()
        {
            UIMenu seller = new UIMenu("Real Estate", "We have the house for all your needs!", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            Dictionary<string, ConfigHouses> apartments = Client.Settings.RolePlay.Properties.Apartments;
            Dictionary<string, Garages> garages = Client.Settings.RolePlay.Properties.Garages.Garages;
            UIMenuItem appartItem = new("Apartments");
            UIMenu appart = new("Apartments", "");
            UIMenuItem garaItem = new("Garages");
            UIMenu gara = new("Garages", "");
            Camera cam = World.CreateCamera(Vector3.Zero, Vector3.Zero, GameplayCamera.FieldOfView);

            appartItem.BindItemToMenu(appart);
            garaItem.BindItemToMenu(gara);

            seller.AddItem(appartItem);
            seller.AddItem(garaItem);


            foreach (KeyValuePair<string, ConfigHouses> app in apartments.OrderBy(x => x.Value.Price))
            {
                UIMenuItem appartamentoItem = new(app.Value.Label);
                UIMenu appartamento = new(app.Value.Label, "");
                appartamentoItem.BindItemToMenu(appartamento);
                appart.AddItem(appartamentoItem);
                appartamentoItem.SetRightLabel("Rif. ~g~$" + app.Value.Price);
                appartamento.OnMenuOpen += async (newmenu, b) =>
                {
                    newmenu.Clear();
                    List<Player> players = Functions.GetPlayersInArea(Cache.PlayerCache.MyPlayer.Position.ToVector3, 3.5f, false);

                    foreach (Player p in players)
                    {
                        UIMenuItem personaItem = new(p.GetPlayerData().FullName);
                        UIMenu Person = new(p.GetPlayerData().FullName, "");
                        personaItem.BindItemToMenu(Person);
                        newmenu.AddItem(personaItem);
                        UIMenuListItem show = new("Show apartment", new List<dynamic>()
                        {
                            "Nothing",
                            "Ouside",
                            "Inside",
                            "Bath",
                            "Garage"
                        }, 0);
                        UIMenuItem rent = new("Rent");
                        UIMenuItem sell = new("Sell");
                        Person.AddItem(show);
                        Person.AddItem(rent);
                        Person.AddItem(sell);
                        cam = World.CreateCamera(Vector3.Zero, Vector3.Zero, GameplayCamera.FieldOfView);
                        show.OnListChanged += async (_item, _index) =>
                        {
                            switch (_index)
                            {
                                case 0:
                                    Screen.Fading.FadeOut(800);
                                    await BaseScript.Delay(1000);
                                    cam.Position = Vector3.Zero;
                                    cam.Rotation = Vector3.Zero;
                                    RenderScriptCams(false, false, 1000, false, false);
                                    Screen.Fading.FadeIn(500);

                                    break;
                                case 1:
                                    Screen.Fading.FadeOut(500);
                                    await BaseScript.Delay(600);
                                    RequestCollisionAtCoord(app.Value.CameraOutside.Pos.X, app.Value.CameraOutside.Pos.Y, app.Value.CameraOutside.Pos.Z);
                                    RequestAdditionalCollisionAtCoord(app.Value.CameraOutside.Pos.X, app.Value.CameraOutside.Pos.Y, app.Value.CameraOutside.Pos.Z);
                                    NewLoadSceneStart(app.Value.CameraOutside.Pos.X, app.Value.CameraOutside.Pos.Y, app.Value.CameraOutside.Pos.Z, app.Value.CameraOutside.Pos.X, app.Value.CameraOutside.Pos.Y, app.Value.CameraOutside.Pos.Z, 50f, 0);
                                    int tempTimer0 = GetGameTimer();

                                    while (IsNetworkLoadingScene())
                                    {
                                        if (GetGameTimer() - tempTimer0 > 3000) break;
                                        await BaseScript.Delay(0);
                                    }

                                    cam.Position = app.Value.CameraOutside.Pos.ToVector3;
                                    cam.PointAt(app.Value.CameraOutside.Rotation.ToVector3);
                                    RenderScriptCams(true, false, 1000, false, false);
                                    Screen.Fading.FadeIn(500);

                                    break;
                                case 2:
                                    Screen.Fading.FadeOut(500);
                                    await BaseScript.Delay(600);
                                    RequestCollisionAtCoord(app.Value.CameraInside.Inside.Pos.X, app.Value.CameraInside.Inside.Pos.Y, app.Value.CameraInside.Inside.Pos.Z);
                                    RequestAdditionalCollisionAtCoord(app.Value.CameraInside.Inside.Pos.X, app.Value.CameraInside.Inside.Pos.Y, app.Value.CameraInside.Inside.Pos.Z);
                                    NewLoadSceneStart(app.Value.CameraInside.Inside.Pos.X, app.Value.CameraInside.Inside.Pos.Y, app.Value.CameraInside.Inside.Pos.Z, app.Value.CameraInside.Inside.Pos.X, app.Value.CameraInside.Inside.Pos.Y, app.Value.CameraInside.Inside.Pos.Z, 50f, 0);
                                    int tempTimer1 = GetGameTimer();

                                    while (IsNetworkLoadingScene())
                                    {
                                        if (GetGameTimer() - tempTimer1 > 3000) break;
                                        await BaseScript.Delay(0);
                                    }

                                    cam.Position = Vector3.Add(app.Value.CameraInside.Inside.Pos.ToVector3, new Vector3(0, 0, 1f));
                                    cam.PointAt(app.Value.CameraInside.Inside.Rotation.ToVector3);
                                    Screen.Fading.FadeIn(500);

                                    break;
                                case 3:
                                    Screen.Fading.FadeOut(500);
                                    await BaseScript.Delay(600);
                                    RequestCollisionAtCoord(app.Value.CameraInside.Bathroom.Pos.X, app.Value.CameraInside.Bathroom.Pos.Y, app.Value.CameraInside.Bathroom.Pos.Z);
                                    RequestAdditionalCollisionAtCoord(app.Value.CameraInside.Bathroom.Pos.X, app.Value.CameraInside.Bathroom.Pos.Y, app.Value.CameraInside.Bathroom.Pos.Z);
                                    NewLoadSceneStart(app.Value.CameraInside.Bathroom.Pos.X, app.Value.CameraInside.Bathroom.Pos.Y, app.Value.CameraInside.Bathroom.Pos.Z, app.Value.CameraInside.Bathroom.Pos.X, app.Value.CameraInside.Bathroom.Pos.Y, app.Value.CameraInside.Bathroom.Pos.Z, 50f, 0);
                                    int tempTimer2 = GetGameTimer();

                                    while (IsNetworkLoadingScene())
                                    {
                                        if (GetGameTimer() - tempTimer2 > 3000) break;
                                        await BaseScript.Delay(0);
                                    }

                                    cam.Position = Vector3.Add(app.Value.CameraInside.Bathroom.Pos.ToVector3, new Vector3(0, 0, 1f));
                                    cam.PointAt(app.Value.CameraInside.Bathroom.Rotation.ToVector3);
                                    RenderScriptCams(true, false, 1000, false, false);
                                    Screen.Fading.FadeIn(500);

                                    break;
                                case 4:
                                    Screen.Fading.FadeOut(500);
                                    await BaseScript.Delay(600);
                                    RequestCollisionAtCoord(app.Value.CameraInside.Garage.Pos.X, app.Value.CameraInside.Garage.Pos.Y, app.Value.CameraInside.Garage.Pos.Z);
                                    RequestAdditionalCollisionAtCoord(app.Value.CameraInside.Garage.Pos.X, app.Value.CameraInside.Garage.Pos.Y, app.Value.CameraInside.Garage.Pos.Z);
                                    NewLoadSceneStart(app.Value.CameraInside.Garage.Pos.X, app.Value.CameraInside.Garage.Pos.Y, app.Value.CameraInside.Garage.Pos.Z, app.Value.CameraInside.Garage.Pos.X, app.Value.CameraInside.Garage.Pos.Y, app.Value.CameraInside.Garage.Pos.Z, 50f, 0);
                                    int tempTimer3 = GetGameTimer();

                                    while (IsNetworkLoadingScene())
                                    {
                                        if (GetGameTimer() - tempTimer3 > 3000) break;
                                        await BaseScript.Delay(0);
                                    }

                                    cam.Position = app.Value.CameraInside.Garage.Pos.ToVector3;
                                    cam.PointAt(app.Value.CameraInside.Garage.Rotation.ToVector3);
                                    RenderScriptCams(true, false, 1000, false, false);
                                    Screen.Fading.FadeIn(500);

                                    break;
                            }
                        };
                        rent.Activated += async (_menu, _item) =>
                        {
                            string res = await HUD.GetUserInput("Add rent prince", "" + app.Value.Price, 10);

                            if (string.IsNullOrEmpty(res) || string.IsNullOrWhiteSpace(res))
                            {
                                HUD.ShowNotification("You must add a value!", ColoreNotifica.Red, true);

                                return;
                            }

                            int aff = Convert.ToInt32(res);

                            if (aff <= 0)
                            {
                                HUD.ShowNotification("Value must be positive!", ColoreNotifica.Red, true);

                                return;
                            }

                            BaseScript.TriggerServerEvent("housedealer:vendi", false, p.ServerId, app.ToJson(), aff);
                        };
                        sell.Activated += async (_menu, _item) =>
                        {
                            string res = await HUD.GetUserInput("Insert selling price", "" + app.Value.Price, 10);

                            if (string.IsNullOrEmpty(res) || string.IsNullOrWhiteSpace(res))
                            {
                                HUD.ShowNotification("You must add a value!", ColoreNotifica.Red, true);

                                return;
                            }

                            int aff = Convert.ToInt32(res);

                            if (aff <= 0)
                            {
                                HUD.ShowNotification("Value must be positive!", ColoreNotifica.Red, true);

                                return;
                            }

                            BaseScript.TriggerServerEvent("housedealer:vendi", true, p.ServerId, app.ToJson(), aff);
                        };
                        Person.OnMenuClose += async (_menu) =>
                        {
                            if ((!cam.IsActive || GetRenderingCam() != cam.Handle) && cam.Position == Vector3.Zero) return;
                            Screen.Fading.FadeOut(800);
                            await BaseScript.Delay(1000);
                            RenderScriptCams(false, false, 1000, false, false);
                            cam.Delete();
                            Screen.Fading.FadeIn(500);
                        };
                    }
                };
            }

            /*
                UIMenuItem item = new UIMenuItem(gar.Value.Label);
                UIMenu menu = new UIMenu(gar.Value.Label, "");
                item.BindItemToMenu(menu);
                gara.AddItem(item);
             */
            //foreach (UIMenu garage in Garages.Select(gar => gara.AddSubMenu(gar.Value.Label))) { }

            seller.Visible = true;
        }

    }
}