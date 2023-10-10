using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.AdminAC
{
    internal static class ManagerMenu
    {
        private static bool spawnInVeh = false;
        private static bool deleteOldVeh = false;
        private static Vehicle savedVehicle;

        public static async void AdminMenu(UserGroup group_level)
        {
            UIMenu AdminMenu = new("Admin menu", "Like in the Matrix!", new PointF(950, 50), "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem playersItem = new UIMenuItem("Player management", "~r~Warning!!~w~ - Here you can handle players and their characters (money, jobs, inventory, weapons).~n~o~Be careful!~w~");
            UIMenu MenuPlayers = new UIMenu("Player management", "");
            UIMenuItem MenuVehiclesItem = new UIMenuItem("Vehicles menu");
            UIMenu MenuVehicles = new("Vehicles", "");
            UIMenuItem inventoryItem = new UIMenuItem("Inventory menu");
            UIMenu Inventory = new("Inventory", "");
            UIMenuItem MenuWeaponsItem = new UIMenuItem("Weapons menu");
            UIMenu MenuWeapons = new("Weapons", "");
            UIMenuItem WeatherItem = new UIMenuItem("Change weather");
            UIMenu Weather = new("Weather", "");
            UIMenuItem TimeItem = new UIMenuItem("Change server time");
            UIMenu TimeMenu = new("Server time", "");

            playersItem.Activated += async (a, b) => await a.SwitchTo(MenuPlayers, 0, true);
            MenuVehiclesItem.Activated += async (a, b) => await a.SwitchTo(MenuVehicles, 0, true);
            inventoryItem.Activated += async (a, b) => await a.SwitchTo(Inventory, 0, true);
            MenuWeaponsItem.Activated += async (a, b) => await a.SwitchTo(MenuWeapons, 0, true);
            WeatherItem.Activated += async (a, b) => await a.SwitchTo(Weather, 0, true);
            TimeItem.Activated += async (a, b) => await a.SwitchTo(TimeMenu, 0, true);

            AdminMenu.AddItem(playersItem);
            AdminMenu.AddItem(MenuVehiclesItem);
            AdminMenu.AddItem(inventoryItem);
            AdminMenu.AddItem(MenuWeaponsItem);
            AdminMenu.AddItem(WeatherItem);
            AdminMenu.AddItem(TimeItem);

            #region Players

            MenuPlayers.OnMenuOpen += async (a, b) =>
            {
                //if (state != MenuState.Opened) return;
                MenuPlayers.Clear();

                /*// COMMENT TO TEST ON YOURSELF IF NO PLAYERS IN GAME
					if (Client.Instance.GetPlayers.Count() == 1)
					{
						UIMenuItem nessuno = new UIMenuItem("Non ci sono player oltre te!");
						_menu.AddItem(nessuno);
						return;
					}
					*/

                List<Player> players = await EventDispatcher.Get<List<Player>>("tlg:getPlayers");

                foreach (PlayerClient p in Client.Instance.Clients)
                {
                    //if (p == Cache.Player) continue; // COMMENT TO TEST ON YOURSELF IF NO PLAYERS IN GAME
                    User player = p.User;
                    string charscount;
                    if (player.Characters.Count == 1)
                        charscount = "1 character";
                    else
                        charscount = player.Characters.Count + " characters";
                    UIMenuItem playerItem = new UIMenuItem(p.Player.Name, charscount);
                    MenuPlayers.AddItem(playerItem);
                    UIMenu Client = new(p.Player.Name, "");
                    playerItem.Activated += async (a, b) => await MenuPlayers.SwitchTo(Client, 0, true);
                    UIMenuItem teleport_me = new("Teleport to player's position");
                    UIMenuItem teleport_him = new("Teleport player to your position");
                    UIMenuItem Specta = new("Spectate Player");
                    Client.AddItem(teleport_me);
                    Client.AddItem(teleport_him);
                    Client.AddItem(Specta);
                    Client.OnItemSelect += async (menu, item, index) =>
                    {
                        if (item == teleport_me)
                        {
                            Cache.PlayerCache.MyPlayer.Ped.Position = p.Ped.Position;
                        }
                        else if (item == teleport_him)
                        {
                            EventDispatcher.Send("manager:TeletrasportaDaMe", p.Handle);
                        }
                        else if (item == Specta)
                        {
                            if (p == Cache.PlayerCache.MyPlayer) return;
                            Cache.PlayerCache.MyPlayer.Status.PlayerStates.AdminSpectating = true;
                            RequestCollisionAtCoord(p.Ped.Position.X, p.Ped.Position.Y, p.Ped.Position.Z);
                            NetworkSetInSpectatorMode(true, p.Ped.Handle);
                            TheLastPlanet.Client.Client.Instance.AddTick(SpectatorMode);
                        }
                    };

                    #region Ban Player

                    DateTime TempoDiBan = DateTime.Now.AddMinutes(10);
                    string Motivazione = "";
                    List<dynamic> tempiban = new()
                    {
                        "10 min",
                        "20 min",
                        "30 min",
                        "1 hr",
                        "2 hrs",
                        "3 hrs",
                        "12 hrs",
                        "1 day",
                        "2 days",
                        "3 days",
                        "1 week",
                        "2 weeks",
                        "3 weeks",
                        "1 month",
                        "2 months",
                        "3 months",
                        "6 months",
                        "1 year",
                        "100 years (Perma-ban)"
                    };
                    UIMenuItem banItem = new UIMenuItem("~r~Ban Player~w~");
                    UIMenu Ban = new UIMenu("Ban Player " + p.Player.Name, "");
                    banItem.Activated += async (a, b) => await Client.SwitchTo(Ban, 0, true);
                    Client.AddItem(banItem);
                    UIMenuItem reasonBan = new("Reason", "");
                    UIMenuListItem timeBan = new("Ban time", tempiban, 0, "⚠️: ONCE BAN IS CONFIRMED, TIME ~h~~r~CANNOT~w~ BE CHANGED");
                    UIMenuItem confirmBan = new("Confirm ban", "⚠️:~r~ BAN IS YOUR RESPONSABILITY', SINCE IT IS YOUR NAME THAT WILL BE ADDED TO THE BAN REASON~W~!", SColor.HUD_Reddark, SColor.HUD_Red);
                    UIMenuCheckboxItem temp = new("Temp", UIMenuCheckboxStyle.Tick, false, "Temp?");
                    Ban.AddItem(reasonBan);
                    Ban.AddItem(timeBan);
                    Ban.AddItem(temp);
                    Ban.AddItem(confirmBan);
                    Ban.OnItemSelect += async (menu, item, index) =>
                    {
                        if (item == reasonBan)
                        {
                            Motivazione = await HUD.GetUserInput("Ban reason (max 175 chars)", "", 175);
                            reasonBan.Description = Motivazione;
                            //reasonBan.SetRightLabel(Motivazione.Length > 15 ? Motivazione.Substring(0, 15) + "..." : Motivazione);
                            menu.UpdateDescription();
                        }
                        else if (item == confirmBan)
                        {
                            BaseScript.TriggerServerEvent("lprp:bannaPlayer", p.Handle, Motivazione, temp.Checked, TempoDiBan.Ticks, Cache.PlayerCache.MyPlayer.Player.ServerId);
                        }

                        // string target, string motivazione, int tempodiban, string banner  - banner e target sono i serverid.. comodo eh?
                    };
                    timeBan.OnListChanged += async (item, index) =>
                    {
                        DateTime time = DateTime.Now;

                        switch (index)
                        {
                            case 0:
                                TempoDiBan = time.AddMinutes(10);
                                break;
                            case 1:
                                TempoDiBan = time.AddMinutes(20);
                                break;
                            case 2:
                                TempoDiBan = time.AddMinutes(30);
                                break;
                            case 3:
                                TempoDiBan = time.AddHours(1);
                                break;
                            case 4:
                                TempoDiBan = time.AddHours(2);
                                break;
                            case 5:
                                TempoDiBan = time.AddHours(3);
                                break;
                            case 6:
                                TempoDiBan = time.AddHours(12);
                                break;
                            case 7:
                                TempoDiBan = time.AddDays(1);
                                break;
                            case 8:
                                TempoDiBan = time.AddDays(2);
                                break;
                            case 9:
                                TempoDiBan = time.AddDays(3);
                                break;
                            case 10:
                                TempoDiBan = time.AddDays(7);
                                break;
                            case 11:
                                TempoDiBan = time.AddDays(14);
                                break;
                            case 12:
                                TempoDiBan = time.AddDays(21);
                                break;
                            case 13:
                                TempoDiBan = time.AddMonths(1);
                                break;
                            case 14:
                                TempoDiBan = time.AddMonths(2);
                                break;
                            case 15:
                                TempoDiBan = time.AddMonths(3);
                                break;
                            case 16:
                                TempoDiBan = time.AddMonths(6);
                                break;
                            case 17:
                                TempoDiBan = time.AddYears(1);
                                break;
                            case 18:
                                TempoDiBan = time.AddYears(100);
                                break;
                        }
                    };

                    #endregion

                    #region Kick

                    string kickReason = "";
                    UIMenuItem kickItem = new UIMenuItem("~y~Kick Player~w~");
                    UIMenu Kick = new UIMenu("Kick Player " + p.Player.Name, "");
                    Client.AddItem(kickItem);
                    kickItem.Activated += async (a, b) => await Client.SwitchTo(Kick, 0, true);

                    UIMenuItem motivazioneKick = new("Reason");
                    UIMenuItem Kicka = new("Confirm kick", "⚠️: YOUR NAME WILL BE ADDED TO THE KICK REASON!");
                    Kick.AddItem(motivazioneKick);
                    Kick.AddItem(Kicka);
                    Kick.OnItemSelect += async (menu, item, index) =>
                    {
                        if (item == motivazioneKick)
                        {
                            kickReason = await HUD.GetUserInput("Kick reason? (max 175)", "", 175);
                            motivazioneKick.Description = kickReason;
                            motivazioneKick.SetRightLabel(kickReason.Substring(0, 15) + "...");
                        }
                        else if (item == Kicka)
                        {
                            BaseScript.TriggerServerEvent("lprp:kickPlayer", p.Handle, kickReason, Cache.PlayerCache.MyPlayer.Player.ServerId);
                        }
                    };

                    #endregion

                    UIMenuItem persItem = new UIMenuItem("~b~Chars management~w~", charscount);
                    UIMenu Chars = new UIMenu("Chars management", "Player Management");
                    Client.AddItem(persItem);
                    persItem.Activated += async (a, b) => await Client.SwitchTo(Chars, 0, true);

                    foreach (Char_data chars in player.Characters)
                    {
                        UIMenu Character = new(chars.Info.Firstname + " " + chars.Info.Lastname, "");
                        UIMenu personalDataMenu = new("Personal data", player.Name + " Management");
                        UIMenu inventoryMenu = new("Inventory", player.Name + " Management");
                        UIMenu weaponsMenu = new("Weapons", player.Name + " Management");
                        UIMenu financesMenu = new("Finances", player.Name + " Management");
                        UIMenu vehiclesMenu = new("Vehicles", player.Name + " Management");
                        UIMenu propertiesMenu = new("Properties", player.Name + " Management");
                        UIMenu genericDataMenu = new("Generic Data", player.Name + " Management");

                        UIMenuItem CharacterItem = new UIMenuItem(chars.Info.Firstname + " " + chars.Info.Lastname);
                        UIMenuItem personalDataItem = new UIMenuItem("Personal data", "Name, job, gangs");
                        UIMenuItem inventoryItem = new UIMenuItem("Inventory");
                        UIMenuItem weaponsItem = new UIMenuItem("Weapons");
                        UIMenuItem financesItem = new UIMenuItem("Finances");
                        UIMenuItem vehiclesItem = new UIMenuItem("Vehicles");
                        UIMenuItem propertiesItem = new UIMenuItem("Properties");
                        UIMenuItem genericDataItem = new UIMenuItem("Generic data", "Criminal record, fines..");
                        Chars.AddItem(CharacterItem);
                        Chars.AddItem(personalDataItem);
                        Chars.AddItem(inventoryItem);
                        Chars.AddItem(weaponsItem);
                        Chars.AddItem(financesItem);
                        Chars.AddItem(vehiclesItem);
                        Chars.AddItem(propertiesItem);
                        Chars.AddItem(genericDataItem);

                        CharacterItem.Activated += async (a, b) => await Chars.SwitchTo(Character, 0, true);
                        personalDataItem.Activated += async (a, b) => await Chars.SwitchTo(personalDataMenu, 0, true);
                        inventoryItem.Activated += async (a, b) => await Chars.SwitchTo(inventoryMenu, 0, true);
                        weaponsItem.Activated += async (a, b) => await Chars.SwitchTo(weaponsMenu, 0, true);
                        financesItem.Activated += async (a, b) => await Chars.SwitchTo(financesMenu, 0, true);
                        vehiclesItem.Activated += async (a, b) => await Chars.SwitchTo(vehiclesMenu, 0, true);
                        propertiesItem.Activated += async (a, b) => await Chars.SwitchTo(propertiesMenu, 0, true);
                        genericDataItem.Activated += async (a, b) => await Chars.SwitchTo(genericDataMenu, 0, true);

                        #region Personal data

                        UIMenuItem fullName = new("Full name");
                        fullName.SetRightLabel(chars.Info.Firstname + " " + chars.Info.Lastname);
                        UIMenuItem dOB = new("Date of Birth");
                        dOB.SetRightLabel(chars.Info.DateOfBirth);
                        UIMenuItem sex = new("Sex");
                        sex.SetRightLabel(chars.Skin.Sex);
                        UIMenuItem height = new("Height");
                        height.SetRightLabel(chars.Info.Height + "cm");
                        UIMenuItem job = new("Current Job");
                        job.SetRightLabel(chars.Job.Name);
                        UIMenuItem phone = new("Phone");
                        phone.SetRightLabel("" + chars.Info.PhoneNumber);
                        UIMenuItem insurance = new("Insurance");
                        insurance.SetRightLabel("" + chars.Info.Insurance);
                        personalDataMenu.AddItem(fullName);
                        personalDataMenu.AddItem(dOB);
                        personalDataMenu.AddItem(sex);
                        personalDataMenu.AddItem(height);
                        personalDataMenu.AddItem(job);
                        personalDataMenu.AddItem(phone);
                        personalDataMenu.AddItem(insurance);

                        #endregion

                        #region Inventory

                        UIMenuItem addItem = new("Add to inventory", "You'll have to insert item code and quantity.");
                        inventoryMenu.AddItem(addItem);

                        if (chars.Inventory.Count > 0)
                            foreach (Inventory item in chars.Inventory)
                            {
                                if (item.Amount <= 0) continue;
                                UIMenuItem newItemMenuItem = new UIMenuItem(ConfigShared.SharedConfig.Main.Generics.ItemList[item.Item].label, "[Quantity: " + item.Amount.ToString() + "] " + ConfigShared.SharedConfig.Main.Generics.ItemList[item.Item].description);
                                UIMenu newItemMenu = new UIMenu(ConfigShared.SharedConfig.Main.Generics.ItemList[item.Item].label, "Inventory");
                                newItemMenuItem.Activated += async (a, b) => await inventoryMenu.SwitchTo(newItemMenu, 0, true);
                                inventoryMenu.AddItem(newItemMenuItem);

                                UIMenuItem add = new("Add", "How many?");
                                UIMenuItem rem = new("Remove", "How many?");
                                newItemMenu.AddItem(add);
                                newItemMenu.AddItem(rem);
                                Inventory item1 = item;
                                newItemMenu.OnItemSelect += async (menu, mitem, index) =>
                                {
                                    if (mitem == add)
                                    {
                                        int qtty = Convert.ToInt32(await HUD.GetUserInput("Quantity", "1", 2));
                                        if (qtty < 99 && qtty > 0)
                                            BaseScript.TriggerServerEvent("lprp:addIntenvoryItemtochar", p.Handle, chars.CharID, item1.Item, qtty);
                                        else
                                            HUD.ShowNotification("Quantity not valid!", ColoreNotifica.Red, true);
                                    }
                                    else if (mitem == rem)
                                    {
                                        int quantita = Convert.ToInt32(await HUD.GetUserInput("Quantity", "1", 2));
                                        if (quantita < 99 && quantita > 0)
                                            BaseScript.TriggerServerEvent("lprp:removeIntenvoryItemtochar", p.Handle, chars.CharID, item1.Item, quantita);
                                        else
                                            HUD.ShowNotification("Quantity not valid!", ColoreNotifica.Red, true);
                                    }
                                };
                            }

                        addItem.Activated += async (menu, item) =>
                        {
                            string _item = await HUD.GetUserInput("Item name", "", 10);
                            int qtty = Convert.ToInt32(await HUD.GetUserInput("Quantity", "1", 2));
                            if (qtty < 99 && qtty > 0)
                                BaseScript.TriggerServerEvent("lprp:addIntenvoryItemtochar", p.Handle, chars.CharID, _item, qtty);
                            else
                                HUD.ShowNotification("Quantity not valid!", ColoreNotifica.Red, true);
                        };

                        #endregion

                        #region Weapons

                        #endregion

                        #region Vehicles

                        #endregion

                        #region Properties

                        #endregion

                        #region Generic data

                        #endregion
                    }
                }
            };

            #endregion

            #region Veicoli

            UIMenuItem NomeVeh = new("Vehicle name");
            MenuVehicles.AddItem(NomeVeh);
            UIMenuItem vehOptionsItem = new UIMenuItem("Spawn options");
            UIMenu VehOptions = new("Spawn options", "");
            MenuVehicles.AddItem(vehOptionsItem);
            vehOptionsItem.Activated += async (a, b) => await MenuVehicles.SwitchTo(VehOptions, 0, true);
            UIMenuCheckboxItem spawn = new("Spawn inside vehicle", UIMenuCheckboxStyle.Tick, spawnInVeh, "");
            UIMenuCheckboxItem deletepreviousveh = new("Delete old vehicle", UIMenuCheckboxStyle.Tick, deleteOldVeh, "");
            VehOptions.AddItem(spawn);
            VehOptions.AddItem(deletepreviousveh);
            NomeVeh.Activated += async (menu, item) =>
            {
                string input = await HUD.GetUserInput("Model name", "", 15);

                if (UpdateOnscreenKeyboard() == 3) return;

                if (!input.All(o => char.IsDigit(o)) && !IsModelValid(Functions.HashUint(input)))
                {
                    HUD.ShowNotification("Wrong model!");

                    return;
                }

                if (!IsModelValid(Functions.HashUint(input)))
                {
                    HUD.ShowNotification("Wrong model!");

                    return;
                }

                if (deleteOldVeh)
                    if (savedVehicle != null)
                        savedVehicle.Delete();

                if (spawnInVeh)
                {
                    savedVehicle = await Functions.SpawnVehicle(input, Cache.PlayerCache.MyPlayer.Position.ToVector3, Cache.PlayerCache.MyPlayer.Position.Heading);
                    if (savedVehicle.Model.IsHelicopter || savedVehicle.Model.IsPlane) SetHeliBladesFullSpeed(savedVehicle.Handle);
                }
                else
                {
                    savedVehicle = await Functions.SpawnVehicleNoPlayerInside(input, GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 5f, 0), Cache.PlayerCache.MyPlayer.Ped.Heading);
                }

                savedVehicle.DirtLevel = 0;
                savedVehicle.NeedsToBeHotwired = false;
                savedVehicle.MarkAsNoLongerNeeded();
            };
            VehOptions.OnCheckboxChange += (menu, item, activated) =>
            {
                if (item == spawn)
                    spawnInVeh = activated;
                else if (item == deletepreviousveh) deleteOldVeh = activated;
            };

            #endregion

            #region Oggetti

            UIMenuItem itemToSpawn = new("Item to spawn");
            Inventory.AddItem(itemToSpawn);
            Inventory.OnItemSelect += async (menu, item, index) =>
            {
                string __item = "";

                if (item != itemToSpawn) return;
                __item = await HUD.GetUserInput("Insert item or its hash", "", 50);

                if (UpdateOnscreenKeyboard() == 3) return;

                if (!__item.All(o => char.IsDigit(o)) && !IsModelValid(Functions.HashUint(__item)))
                {
                    HUD.ShowNotification("Wrong model!");

                    return;
                }

                if (!IsModelValid((uint)Convert.ToInt32(__item)))
                {
                    HUD.ShowNotification("Wrong model!");

                    return;
                }

                //Prop obj = await World.CreateProp(oggettino.All(o => char.IsDigit(o)) ? new Model(Convert.ToInt32(oggettino)) : new Model(oggettino), GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 5f, 0), true, true);
                Prop obj = await Functions.CreateProp(__item.All(o => char.IsDigit(o)) ? new Model(Convert.ToInt32(__item)) : new Model(__item), GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 5f, 0), new Vector3(0, 0, Cache.PlayerCache.MyPlayer.Ped.Heading - 180f), true);
            };

            #endregion

            #region Meteo

            UIMenuItem weathersItem = new UIMenuItem("Select weather");
            UIMenu weathers = new("Select weather", "");
            Weather.AddItem(weathersItem);
            weathersItem.Activated += async (a, b) => await Weather.SwitchTo(weathers, 0, true);
            SharedWeather meteo = (Client.Instance.ServerState.Get("Weaher") as byte[]).FromBytes<SharedWeather>();
            UIMenuCheckboxItem blackout = new("General blackout", UIMenuCheckboxStyle.Tick, meteo.Blackout, "BlackOut of all map lights");
            UIMenuCheckboxItem dinamico = new("Dynamic weather", UIMenuCheckboxStyle.Tick, meteo.DynamicMeteo, "Enable/disable dynamic weather, if disabled.. weather changing will stop!");
            Weather.AddItem(blackout);
            Weather.AddItem(dinamico);
            UIMenuItem superSunny = new("Super sunny");
            UIMenuItem clearSky = new("Clear sky");
            UIMenuItem cloudy = new("Cloudy");
            UIMenuItem smog = new("Smog");
            UIMenuItem foggy = new("Foggy");
            UIMenuItem cloudy2 = new("Cloudy2");
            UIMenuItem rainy = new("Rainy");
            UIMenuItem stormy = new("Stormy");
            UIMenuItem clear = new("Clear");
            UIMenuItem neutral = new("Neutral");
            UIMenuItem snowy = new("Snowy");
            UIMenuItem snowstorm = new("Snow storm");
            UIMenuItem fogSnow = new("Fog snow");
            UIMenuItem xMas = new("XMas");
            UIMenuItem halloween = new("Halloween");
            weathers.AddItem(superSunny);
            weathers.AddItem(clearSky);
            weathers.AddItem(cloudy);
            weathers.AddItem(smog);
            weathers.AddItem(foggy);
            weathers.AddItem(cloudy2);
            weathers.AddItem(rainy);
            weathers.AddItem(stormy);
            weathers.AddItem(clear);
            weathers.AddItem(neutral);
            weathers.AddItem(snowy);
            weathers.AddItem(snowstorm);
            weathers.AddItem(fogSnow);
            weathers.AddItem(xMas);
            weathers.AddItem(halloween);
            Weather.OnCheckboxChange += async (menu, item, _checked) =>
            {
                if (item == blackout)
                {
                    EventDispatcher.Send("changeWeatherWithParams", meteo.CurrentWeather, _checked, false);
                    HUD.ShowNotification("Blackout ~b~" + (_checked ? "enabled" : "disabled") + "~w~.");
                }
                else if (item == dinamico)
                {
                    EventDispatcher.Send("changeWeatherDynamic", _checked);
                    HUD.ShowNotification("Dynamic weather ~b~" + (_checked ? "enabled" : "disabled") + "~w~.");
                }
            };
            /*
            weathers.OnItemSelect += async (menu, item, index) =>
            {
                EventDispatcher.Send("changeWeatherWithParams", index, meteo.Blackout, false);
                string m = "";

                switch (index)
                {
                    case 0:
                        m = "Super Soleggiato";

                        break;
                    case 1:
                        m = "Cielo Sgombro";

                        break;
                    case 2:
                        m = "Nuvoloso";

                        break;
                    case 3:
                        m = "Smog";

                        break;
                    case 4:
                        m = "Nebbioso";

                        break;
                    case 5:
                        m = "Nuvoloso";

                        break;
                    case 6:
                        m = "Piovoso";

                        break;
                    case 7:
                        m = "Tempestoso";

                        break;
                    case 8:
                        m = "Sereno";

                        break;
                    case 9:
                        m = "Neutrale";

                        break;
                    case 10:
                        m = "Nevoso";

                        break;
                    case 11:
                        m = "Bufera di neve";

                        break;
                    case 12:
                        m = "Nevoso con Nebbia";

                        break;
                    case 13:
                        m = "Natalizio";

                        break;
                    case 14:
                        m = "Halloween";

                        break;
                    default:
                        m = "Sconosciuto?";

                        break;
                }

                HUD.ShowNotification("Meteo in transizione verso ~b~" + m + "~w~.");
            };

            */
            #endregion

            #region Time

            UIMenuItem morning = new("Morning", "6:00am");
            UIMenuItem afternoon = new("Afternoon", "12:00pm");
            UIMenuItem evening = new("Evening", "Ore 18:00pm");
            UIMenuItem night = new("Night", "Ore 21:00pm");
            TimeMenu.AddItem(morning);
            TimeMenu.AddItem(afternoon);
            TimeMenu.AddItem(evening);
            TimeMenu.AddItem(night);
            TimeMenu.OnItemSelect += (menu, item, index) =>
            {
                if (item == morning)
                    API.NetworkOverrideClockTime(6, 0, 0);
                else if (item == afternoon)
                    API.NetworkOverrideClockTime(12, 0, 0);
                else if (item == evening)
                    API.NetworkOverrideClockTime(18, 0, 0);
                else if (item == night)
                    API.NetworkOverrideClockTime(21, 0, 0);
                //EventDispatcher.Send("UpdateFromCommandTime", secondOfDay);
            };

            #endregion

            if ((int)group_level < 2)
            {
                WeatherItem.Enabled = false;
                WeatherItem.Description = "Not allowed";
                WeatherItem.SetRightBadge(BadgeIcon.LOCK);
                TimeItem.Enabled = false;
                TimeItem.Description = "Not allowed";
                TimeItem.SetRightBadge(BadgeIcon.LOCK);
            }
            else
            {
                WeatherItem.Description = "⚠️ These changes apply to all players in common buckets (freeroam, rp)!";
                TimeItem.Description = "⚠️ These changes apply to all players in common buckets (freeroam, rp)!";
            }

            if ((int)group_level < 5)
            {
                inventoryItem.Enabled = false;
                inventoryItem.Description = "Not allowed";
            }

            AdminMenu.Visible = true;
        }

        private static async Task SpectatorMode()
        {
            // TODO: OLD CODE USE STATEBAGS
            if (Cache.PlayerCache.MyPlayer.Ped.HasDecor("AdminSpecta") && NetworkIsInSpectatorMode())
            {
                Game.DisableControlThisFrame(0, Control.Context);
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to stop spectating");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    Player p = new(Cache.PlayerCache.MyPlayer.Ped.GetDecor<int>("AdminSpecta"));
                    NetworkSetInSpectatorMode(false, p.Character.Model);
                    Cache.PlayerCache.MyPlayer.Ped.SetDecor("AdminSpecta", 0);
                    Client.Instance.RemoveTick(SpectatorMode);
                }
            }
            else
            {
                NetworkSetOverrideSpectatorMode(false);
            }

            await Task.FromResult(0);
        }
    }
}