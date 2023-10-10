using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Vehicles;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.Police
{
    public static class MenuPolice
    {
        #region LockerMenu

        public static async void CloakRoomMenu()
        {
            Uniform PilotaMaschio = new() { Dress = new ComponentDrawables(-1, 0, -1, 96, 41, -1, 24, 40, 15, 0, 0, 54), TextureDress = new ComponentDrawables(-1, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0), Accessories = new PropIndices(47, -1, -1, -1, -1, -1, -1, -1, -1), TexturesAccessories = new PropIndices(0, -1, -1, -1, -1, -1, -1, -1, -1) };
            Uniform PilotaFemmina = new() { Dress = new ComponentDrawables(-1, 0, -1, 25, 0, -1, 24, 0, 13, 0, 0, 1), TextureDress = new ComponentDrawables(-1, 0, -1, 0, 1, -1, 0, 0, 0, 0, 0, 4), Accessories = new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1), TexturesAccessories = new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1) };
            UIMenu locker = new UIMenu("Police Locker", "Go on / off duty", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);

            UIMenuItem uniform = new UIMenuItem("");
            UIMenuItem vest = new UIMenuItem("");
            UIMenuItem pilot = new UIMenuItem("");

            switch (Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty)
            {
                case false when !PoliceMainClient.OnDutyAsPilot:
                    uniform = new UIMenuItem("Wear your uniform", "If you change you automatically go on duty!");
                    pilot = new UIMenuItem("Wear the pilot suit", "Today you fly the freedom helicopter!");

                    break;
                case false when PoliceMainClient.OnDutyAsPilot:
                    uniform = new UIMenuItem("Wear your uniform", "If you change you automatically go on duty!");
                    pilot = new UIMenuItem("Remove the pilot suit", "If you change you will automatically leave the service and the weapons taken from the police will be returned!");

                    break;
                case true when !PoliceMainClient.OnDutyAsPilot:
                    uniform = new UIMenuItem("Rimuovi l'uniforme", "If you change you will automatically leave the service and the weapons taken from the police will be returned!");
                    pilot = new UIMenuItem("Wear the pilot suit", "Today you fly the freedom helicopter!");

                    break;
                default:
                    {
                        if (Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty || PoliceMainClient.OnDutyAsPilot)
                        {
                            if (Cache.PlayerCache.MyPlayer.Ped.Armor < 1)
                            {
                                vest = new UIMenuItem("Wear the Bulletproof Vest", "It could save your life");
                            }
                            else
                            {
                                vest = new UIMenuItem("Remove Bulletproof Vest", "We hope this was helpful");
                            }
                            locker.AddItem(vest);
                        }

                        break;
                    }
            }

            locker.AddItem(uniform);
            locker.AddItem(pilot);
            locker.OnItemSelect += async (menu, item, index) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                MenuHandler.CloseAndClearHistory();
                NetworkFadeOutEntity(PlayerPedId(), true, false);

                if (item == uniform)
                {
                    if (!Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty)
                    {
                        foreach (KeyValuePair<string, JobGrade> Grado in Client.Settings.RolePlay.Jobs.Police.Grades.Where(Grado => Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name == "Polizia").Where(Grado => Grado.Value.Id == Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade))
                            switch (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex)
                            {
                                case "Male":
                                    GetChanged(Grado.Value.WorkUniforms.Male);

                                    break;
                                case "Female":
                                    GetChanged(Grado.Value.WorkUniforms.Female);

                                    break;
                            }

                        Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty = true;
                    }
                    else
                    {
                        Functions.UpdateDress(PlayerPedId(), Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                        Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty = false;
                    }
                }
                else if (item == pilot)
                {
                    switch (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex)
                    {
                        case "Male":
                            GetChanged(new Uniform() { Dress = new ComponentDrawables(-1, 0, -1, 96, 41, -1, 24, 40, 15, 0, 0, 54), TextureDress = new ComponentDrawables(-1, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0), Accessories = new PropIndices(47, -1, -1, -1, -1, -1, -1, -1, -1), TexturesAccessories = new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1) });

                            break;
                        case "Female":
                            GetChanged(new Uniform() { Dress = new ComponentDrawables(-1, 0, -1, 111, 42, -1, 24, 24, 3, 0, 0, 47), TextureDress = new ComponentDrawables(-1, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0), Accessories = new PropIndices(46, 0, 0, 0, 0, 0, 0, 0, 0), TexturesAccessories = new PropIndices(0, -1, -1, -1, -1, -1, -1, -1, -1) });

                            break;
                    }
                }
                else if (item == vest)
                {
                    if (Cache.PlayerCache.MyPlayer.Ped.Armor < 1)
                    {
                        SetPedComponentVariation(PlayerPedId(), 9, 4, 1, 2);
                        Cache.PlayerCache.MyPlayer.Ped.Armor = 30;
                    }
                    else
                    {
                        SetPedComponentVariation(PlayerPedId(), 9, 0, 1, 2);
                        Cache.PlayerCache.MyPlayer.Ped.Armor = 0;
                    }
                }

                NetworkFadeInEntity(PlayerPedId(), true);
                await BaseScript.Delay(500);
                Screen.Fading.FadeIn(800);
                await Task.FromResult(0);
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

        // TODO: THIS MENU SUCKS.. TO BE REWRITTEN
        #region MenuF6
        private static UIMenuItem CivilInteractionItem;
        private static UIMenuItem VehicleInteractionItem;
        private static UIMenuItem RemotePersonCheckItem;
        private static UIMenuItem RemoveVehicleCheckItem;
        private static UIMenuItem ObjectsItem;

        private static UIMenu PoliceMainMenu = new UIMenu("", "");
        private static UIMenu CivilInteraction = new UIMenu("", "");
        private static UIMenu VehicleInteraction = new UIMenu("", "");
        private static UIMenu RemoteCivilCheck = new UIMenu("", "");
        private static UIMenu RemoveCehicleCheck = new UIMenu("", "");
        private static UIMenu Items = new UIMenu("", "");
        private static uint StreetA = 0;
        private static uint StreetB = 0;
        private static UIMenu findInfo = new UIMenu("", "");
        private static UIMenuItem Pos = new UIMenuItem("");

        public static async void MainMenu()
        {
            string name = "";
            string lastname = "";
            string number = "";
            PoliceMainMenu = new UIMenu("Police Menu", "I AM THE LAW!", new Point(50, 200), "thelastgalaxy", "bannerbackground", false, true);
            CivilInteractionItem = new("Interactions with the Citizen", "Show me your ID!");
            CivilInteraction = new("Interactions with the Citizen", "I AM THE LAW!");
            VehicleInteractionItem = new("Vehicle interactions", "Mi faccia controllare il veicolo!");
            VehicleInteraction = new("Vehicle interactions", "I AM THE LAW!");
            RemotePersonCheckItem = new("O.P.C.S.", "Online People Control System!");
            RemoteCivilCheck = new("O.P.C.S.", "I AM THE LAW!");
            RemoveVehicleCheckItem = new("O.V.C.S.", "Online Vehicles Control System!");
            RemoveCehicleCheck = new("S.O.C.T.", "I AM THE LAW!");
            ObjectsItem = new("Place an item on the ground", "We also have spiked bands!");
            Items = new("Place an item on the ground", "I AM THE LAW!");

            CivilInteractionItem.BindItemToMenu(CivilInteraction);
            VehicleInteractionItem.BindItemToMenu(VehicleInteraction);
            RemotePersonCheckItem.BindItemToMenu(RemoteCivilCheck);
            RemoveVehicleCheckItem.BindItemToMenu(RemoveCehicleCheck);
            ObjectsItem.BindItemToMenu(Items);

            PoliceMainMenu.AddItem(CivilInteractionItem);
            PoliceMainMenu.AddItem(VehicleInteractionItem);
            PoliceMainMenu.AddItem(RemotePersonCheckItem);
            PoliceMainMenu.AddItem(RemoveVehicleCheckItem);
            PoliceMainMenu.AddItem(ObjectsItem);


            #region CIVILE

            UIMenuItem PlayerDataItem = new("Identity card", "Check");
            UIMenu PlayerData = new("Identity card", "Check");
            UIMenuItem SearchItem = new("Search", "Check");
            UIMenu Search = new("Search", "Check");
            UIMenuItem handcuff = new UIMenuItem("Handcuff / Uncuff"); // WITH ANIMATION -- test
            UIMenuItem accompanies = new UIMenuItem("Accompany"); // IF I CAN EVEN MAKE THE PERSON'S PED WALK -- test
            UIMenuItem CriminalRecordItem = new("Criminal Record Check");
            UIMenu CriminalRecord = new("Record Check", "");
            UIMenuItem putVehicle = new UIMenuItem("Seat in vehicle");
            UIMenuItem removeVehicle = new UIMenuItem("Remove vehicle"); // LET OUT LET IN WITH ANIMATION -- test
            UIMenuItem fineItem = new("Make a fine"); // CREATE FINE SYSTEM and add custom fine
            UIMenu fine = new("Make a fine", "");
            UIMenuItem invoicesItem = new("Checking pending payments"); // SAVING INVOICES IN THE DB
            UIMenu Invoice = new("Pending Payment Check", "");
            UIMenuItem incarcerate = new UIMenuItem("Incarcerate"); //YOU MUST BE NEAR THE CELL!
            UIMenuItem LicensesItem = new("Check Licenses"); // license and driving license control
            UIMenu Licenses = new("Check Licenses", "");
            PlayerDataItem.BindItemToMenu(PlayerData);

            SearchItem.BindItemToMenu(Search);
            CriminalRecordItem.BindItemToMenu(CriminalRecord);
            fineItem.BindItemToMenu(fine);
            invoicesItem.BindItemToMenu(Invoice);
            LicensesItem.BindItemToMenu(Licenses);

            CivilInteraction.AddItem(PlayerDataItem);
            CivilInteraction.AddItem(SearchItem);
            CivilInteraction.AddItem(handcuff);
            CivilInteraction.AddItem(accompanies);
            CivilInteraction.AddItem(CriminalRecordItem);
            CivilInteraction.AddItem(putVehicle);

            CivilInteraction.AddItem(removeVehicle);
            CivilInteraction.AddItem(fineItem);
            CivilInteraction.AddItem(invoicesItem);
            CivilInteraction.AddItem(incarcerate);
            CivilInteraction.AddItem(LicensesItem);

            PlayerData.OnMenuOpen += (_newMenu, b) =>
            {
                if (Client.Instance.GetPlayers.ToList().Count > 1)
                {
                    Tuple<Player, float> Player_Distance = Functions.GetClosestPlayer();
                    Ped ClosestPed = Player_Distance.Item1.Character;
                    int playerServerId = GetPlayerServerId(Player_Distance.Item1.Handle);
                    User player = Functions.GetPlayerCharFromServerId(playerServerId);
                    float distance = Player_Distance.Item2;

                    if (distance < 3f && ClosestPed != null)
                    {
                        UIMenuItem nomeLastName = new("Name e LastName");
                        nomeLastName.SetRightLabel(player.FullName);
                        UIMenuItem dDN = new("Data di Nascita");
                        dDN.SetRightLabel(player.DoB);
                        UIMenuItem sesso = new("Sesso");
                        sesso.SetRightLabel(Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex);
                        UIMenuItem altezza = new("Altezza");
                        altezza.SetRightLabel(Cache.PlayerCache.MyPlayer.User.CurrentChar.Info.Height + "cm");
                        UIMenuItem job = new("Occupazione Attuale");
                        job.SetRightLabel(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name);
                        UIMenuItem telefono = new("N° di Telefono");
                        telefono.SetRightLabel("" + Cache.PlayerCache.MyPlayer.User.CurrentChar.Info.PhoneNumber);
                        UIMenuItem assicurazione = new("N° di Assicurazione");
                        assicurazione.SetRightLabel("" + Cache.PlayerCache.MyPlayer.User.CurrentChar.Info.Insurance);
                        UIMenuItem nomePlayer = new("Name Player", "~r~ATTENZIONE!!~w~ - Da usare solo in caso di necessità~n~Un uso sbagliato verrà considerato metagame!");
                        nomePlayer.SetRightLabel(Player_Distance.Item1.Name);
                        PlayerData.AddItem(nomeLastName);
                        PlayerData.AddItem(dDN);
                        PlayerData.AddItem(sesso);
                        PlayerData.AddItem(altezza);
                        PlayerData.AddItem(job);
                        PlayerData.AddItem(telefono);
                        PlayerData.AddItem(assicurazione);
                        PlayerData.AddItem(nomePlayer);
                    }
                    else
                    {
                        UIMenuItem noPlayers = new("No players around!");
                        PlayerData.AddItem(noPlayers);
                    }
                }
                else
                {
                    UIMenuItem noPlayers = new("Non ci sono altri Player nel server!");
                    PlayerData.AddItem(noPlayers);
                }
            };

            Search.OnMenuOpen += (_newMenu, b) =>
            {
                if (_newMenu.MenuItems.Count > 0) _newMenu.Clear();

                if (Client.Instance.GetPlayers.ToList().Count() > 1)
                {
                    Tuple<Player, float> Player_Distance = Functions.GetClosestPlayer();
                    Ped ClosestPed = Player_Distance.Item1.Character;
                    int GetPlayerserverId = GetPlayerServerId(Player_Distance.Item1.Handle);
                    User player = Functions.GetPlayerCharFromServerId(GetPlayerserverId);
                    float distance = Player_Distance.Item2;

                    if (distance < 3f)
                    {
                        List<Shared.Inventory> inv = Cache.PlayerCache.MyPlayer.User.Inventory;

                        if (inv.Count > 0)
                        {
                            foreach (Shared.Inventory it in inv)
                                if (it.Amount > 0)
                                {
                                    UIMenuItem item = new(it.Item);
                                    item.SetRightLabel($"Quantity: {it.Amount}");
                                    _newMenu.AddItem(item);
                                    item.Activated += async (_menu, item) =>
                                    {
                                        BaseScript.TriggerServerEvent("lprp:polizia:confisca", it.Item, it.Amount);
                                    };
                                }
                        }
                        else
                        {
                            Search.AddItem(new UIMenuItem("This person has no objects with him!"));
                        }
                    }
                    else
                    {
                        Search.AddItem(new UIMenuItem("No players around!"));
                    }
                }
                else
                {
                    Search.AddItem(new UIMenuItem("No players around"));
                }
            };
            findInfo.OnMenuOpen += async (_newMenu, b) =>
            {
                Dictionary<string, User> players = await Functions.GetAllPlayersAndTheirData();
                _newMenu.Clear();

                foreach (KeyValuePair<string, User> pers in players.OrderBy(x => x.Key))
                {
                    foreach (Char_data data in pers.Value.Characters)
                    {
                        if (!string.IsNullOrEmpty(name) && data.Info.Firstname.Contains(name) || !string.IsNullOrEmpty(lastname) && data.Info.Lastname.Contains(lastname) || number != "" && number != null && data.Info.PhoneNumber.ToString().Contains(number))
                        {
                            int source = 0;
                            foreach (Player p in Client.Instance.GetPlayers.ToList().Where(p => p.Name == pers.Key)) source = p.ServerId;
                            UIMenuItem charItem = new UIMenuItem(data.Info.Firstname + " " + data.Info.Lastname + " [" + pers.Key + "]");
                            UIMenu @char = new(data.Info.Firstname + " " + data.Info.Lastname + " [" + pers.Key + "]", "");
                            charItem.BindItemToMenu(@char);
                            findInfo.AddItem(charItem);
                            UIMenuItem fullName = new("Name:");
                            fullName.SetRightLabel(data.Info.Firstname + " " + data.Info.Lastname);
                            @char.AddItem(fullName);
                            UIMenuItem dib = new("Date Of Birth:");
                            dib.SetRightLabel(data.Info.DateOfBirth);
                            @char.AddItem(dib);
                            //UIMenuItem Altez = new UIMenuItem("Altezza:");
                            UIMenuItem job = new("Job:");
                            job.SetRightLabel(data.Job.Name);
                            @char.AddItem(job);
                            //UIMenuItem gang = new UIMenuItem("Gang: ", "Le affiliazioni");
                            UIMenuItem bank = new("Bank: ");
                            bank.SetRightLabel("$" + data.Finance.Bank);
                            @char.AddItem(bank);
                            Pos = new UIMenuItem("Last known position");
                            if (source != 0)
                                GetStreetNameAtCoord(Client.Instance.GetPlayers[source].Character.Position.X, Client.Instance.GetPlayers[source].Character.Position.Y, Client.Instance.GetPlayers[source].Character.Position.Z, ref StreetA, ref StreetB);
                            else
                                GetStreetNameAtCoord(data.Position.X, data.Position.Y, data.Position.Z, ref StreetA, ref StreetB);
                            Pos.Description = GetStreetNameFromHashKey(StreetA);
                            if (StreetB != 0) Pos.Description = Pos.Description + ", near " + GetStreetNameFromHashKey(StreetB);
                            @char.AddItem(Pos);
                            UIMenuItem recordItem = new UIMenuItem("Criminal record");
                            UIMenuItem finesItem = new UIMenuItem("Fines / Unpaid");
                            UIMenu records = new("Criminal record", "I AM THE LAW!");
                            UIMenu fines = new("Fines / Unpaid", "I AM THE LAW!");
                            recordItem.BindItemToMenu(records);
                            finesItem.BindItemToMenu(fines);
                            @char.AddItem(recordItem);
                            @char.AddItem(finesItem);

                        }
                    }
                }
            };
            PoliceMainMenu.OnMenuOpen += (a, b) => Client.Instance.AddTick(MenuControl);
            PoliceMainMenu.OnMenuClose += (a) => Client.Instance.RemoveTick(MenuControl);

            CivilInteraction.OnItemSelect += async (menu, item, index) =>
            {
                if (Client.Instance.Clients.ToList().Count() > 1)
                {
                    Tuple<Player, float> Player_Distance = Functions.GetClosestPlayer();
                    Ped ClosestPed = Player_Distance.Item1.Character;
                    int playerServerId = Player_Distance.Item1.ServerId;
                    PlayerClient player = Functions.GetPlayerClientFromServerId(playerServerId);
                    float distance = Player_Distance.Item2;

                    if (distance < 3f && ClosestPed != null)
                        switch (item)
                        {
                            case UIMenuItem i when i == handcuff:
                                BaseScript.TriggerServerEvent("lprp:polizia:ammanetta_smanetta", playerServerId);

                                break;
                            case UIMenuItem i when i == accompanies:
                                if (player.Status.RolePlayStates.Cuffed) // rifare client-->server-->client
                                    BaseScript.TriggerServerEvent("lprp:polizia:accompagna", playerServerId, Cache.PlayerCache.MyPlayer.Ped.NetworkId);
                                else
                                    HUD.ShowNotification("Non è ammanettato!!");

                                break;
                            case UIMenuItem i when i == putVehicle:
                                if (player.Status.RolePlayStates.Cuffed) // rifare client-->server-->client
                                    BaseScript.TriggerServerEvent("lprp:polizia:mettiVeicolo", playerServerId);
                                else
                                    HUD.ShowNotification("Non è ammanettato!!");

                                break;
                            case UIMenuItem i when i == removeVehicle: // rifare client-->server-->client
                                if (player.Status.RolePlayStates.Cuffed)
                                    BaseScript.TriggerServerEvent("lprp:polizia:esciVeicolo", playerServerId);
                                else
                                    HUD.ShowNotification("Non è ammanettato!!");

                                break;
                            case UIMenuItem i when i == incarcerate: break;
                        }
                    else
                        HUD.ShowNotification("no one found near you..~n~try getting closer!", ColoreNotifica.Red, true);
                }
                else
                {
                    HUD.ShowNotification("No other players found!", ColoreNotifica.Red, true);
                }
            };

            #endregion

            #region VEICOLO

            UIMenuItem checkItem = new("Check status vehicle");
            UIMenu check = new("Check status vehicle", "I AM THE LAW!");
            checkItem.BindItemToMenu(check);
            VehicleInteraction.AddItem(checkItem);
            UIMenuItem PickLock = new UIMenuItem("Open locked vehicle");
            UIMenuItem sizeVeh = new UIMenuItem("Seize Vehicle");
            VehicleInteraction.AddItem(PickLock);
            VehicleInteraction.AddItem(sizeVeh);
            VehicleInteraction.OnItemSelect += async (menu, item, index) =>
            {
                switch (item)
                {
                    case UIMenuItem n when n == PickLock:
                        // TODO: CHECK VEHICLE IS LOCKED
                        RequestAnimDict("anim@amb@clubhouse@tutorial@bkr_tut_ig3@");
                        while (!HasAnimDictLoaded("anim@amb@clubhouse@tutorial@bkr_tut_ig3@")) await BaseScript.Delay(0);
                        Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation("anim@amb@clubhouse@tutorial@bkr_tut_ig3@", "machinic_loop_mechandplayer", 8f, -1, AnimationFlags.Loop);
                        await BaseScript.Delay(5000);
                        // VEHICLE IS NOW OPENED
                        Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();
                        break;
                    case UIMenuItem n when n == sizeVeh:
                        TaskStartScenarioInPlace(PlayerPedId(), "CODE_HUMAN_MEDIC_TIME_OF_DEATH", 0, true);
                        await BaseScript.Delay(5000);
                        // TODO: VEHICLE IS DELETED AND REGISTERED IN DEPOSIT
                        // TODO: OR OS MARKED AS NO LONGER NEEDED AND THEN DESPAWNED
                        // TODO: OR WE CALL THE TOWING PEOPLE IF ANY
                        Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();

                        break;
                }
            };

            #endregion

            #region research

            UIMenuItem findName = new UIMenuItem("Find by Name");
            UIMenuItem findLastName = new UIMenuItem("Find by Last Name");
            UIMenuItem findPhoneN = new UIMenuItem("Find by Phone Number");
            // TODO: OPEN A DYNAMIC MENU CHECK ALL PLAYERS FROM SERVER.. IS IT WORTH IT?
            // TODO: IF FOUND I SHOW THEM AND THEIR DATA IF SELECTED
            RemoteCivilCheck.AddItem(findName);
            RemoteCivilCheck.AddItem(findLastName);
            RemoteCivilCheck.AddItem(findPhoneN);

            UIMenuItem findCharItem = new("Research");
            findInfo = new("Research", "I AM THE LAW!");
            findCharItem.BindItemToMenu(check);
            RemoteCivilCheck.AddItem(findCharItem);

            UIMenuItem findModel = new UIMenuItem("Find by Model");
            UIMenuItem findPlate = new UIMenuItem("find by Plate");
            // COME SOPRA
            RemoveCehicleCheck.AddItem(findModel);
            RemoveCehicleCheck.AddItem(findPlate);
            RemoteCivilCheck.OnItemSelect += async (menu, item, index) =>
            {
                if (item == findName)
                {
                    name = await HUD.GetUserInput("Insert name to find", "", 30);
                    if (!name.Any(char.IsDigit))
                        item.SetRightLabel(name);
                    else
                        HUD.ShowNotification("Cannot contain numbers!", ColoreNotifica.Red, true);
                }
                else if (item == findLastName)
                {
                    lastname = await HUD.GetUserInput("Insert last name to find", "", 30);
                    if (!lastname.Any(char.IsDigit))
                        item.SetRightLabel(lastname);
                    else
                        HUD.ShowNotification("Cannot contain numbers!", ColoreNotifica.Red, true);
                }
                else if (item == findPhoneN)
                {
                    number = await HUD.GetUserInput("Insert number to find", "", 30);
                    if (number.All(char.IsDigit))
                        item.SetRightLabel(number);
                    else
                        HUD.ShowNotification("Numbers cannot contain characters!", ColoreNotifica.Red, true);
                }
            };

            #endregion

            #region OGGETTI

            #endregion

            PoliceMainMenu.Visible = true;
        }

        #endregion

        #region MenuSpawnVeicoli

        ///////////////////////// ELICOTTERI
        private static Vehicle PreviewHeli = new Vehicle(0);
        private static Camera HeliCam = new Camera(0);

        public static async void HeliMenu(PoliceStation Stazione, SpawnerSpawn Punto)
        {
            LoadInterior(GetInteriorAtCoords(-1267.0f, -3013.135f, -48.5f));
            RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
            RequestAdditionalCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
            HeliCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { Position = new Vector3(-1268.174f, -2999.561f, -44.215f), IsActive = true };
            await BaseScript.Delay(1000);
            UIMenu MenuElicotteri = new UIMenu("Police Copters", "Justice from above!", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);

            foreach (Authorized t in Stazione.AuthorizedHelicopters)
            {
                UIMenuItem veh = new UIMenuItem(t.Name);
                MenuElicotteri.AddItem(veh);
            }

            MenuElicotteri.OnIndexChange += async (menu, index) =>
            {
                PreviewHeli = await Functions.SpawnLocalVehicle(Stazione.AuthorizedHelicopters[index].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), Punto.SpawnPoints[0].Heading);
                PreviewHeli.IsCollisionEnabled = false;
                PreviewHeli.IsPersistent = true;
                PreviewHeli.PlaceOnGround();
                PreviewHeli.IsPositionFrozen = true;
                if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 0); // per il polmav della pula.. funziona solo per i veicoli d'emergenza!
                PreviewHeli.LockStatus = VehicleLockStatus.Locked;
                SetHeliBladesFullSpeed(PreviewHeli.Handle);
                PreviewHeli.IsInvincible = true;
                PreviewHeli.IsEngineRunning = true;
                PreviewHeli.IsDriveable = false;
                if (HeliCam.IsActive && PreviewHeli.Exists()) HeliCam.PointAt(PreviewHeli);
            };
            MenuElicotteri.OnItemSelect += async (menu, item, index) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                HeliCam.IsActive = false;
                RenderScriptCams(false, false, 0, false, false);

                foreach (SpawnPoints t in Punto.SpawnPoints)
                    if (!Functions.IsSpawnPointClear(t.Coords.ToVector3, 2f))
                    {
                        continue;
                    }
                    else if (Functions.IsSpawnPointClear(t.Coords.ToVector3, 2f))
                    {
                        PoliceMainClient.CurrentHelicopter = await Functions.SpawnVehicle(Stazione.AuthorizedHelicopters[index].Model, t.Coords.ToVector3, t.Heading);

                        break;
                    }
                    else
                    {
                        PoliceMainClient.CurrentHelicopter = await Functions.SpawnVehicle(Stazione.AuthorizedHelicopters[index].Model, Punto.SpawnPoints[0].Coords.ToVector3, Punto.SpawnPoints[0].Heading);

                        break;
                    }

                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.SetVehicleFuelLevel(100f);
                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsDriveable = true;
                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate = SharedMath.GetRandomInt(99) + "POL" + SharedMath.GetRandomInt(999);
                if (Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash == 353883353) SetVehicleLivery(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle, 0);
                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.SetDecor("VehiclePolice", SharedMath.GetRandomInt(100));
                VehiclePolice veh = new VehiclePolice(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle);
                BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehPolizia", veh.ToJson());
                MenuHandler.CloseAndClearHistory();
                PreviewHeli.MarkAsNoLongerNeeded();
                PreviewHeli.Delete();
            };
            MenuElicotteri.OnMenuOpen += async (a, b) =>
            {
                PreviewHeli = await Functions.SpawnLocalVehicle(Stazione.AuthorizedHelicopters[0].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), 0);
                PreviewHeli.IsCollisionEnabled = false;
                PreviewHeli.IsPersistent = true;
                PreviewHeli.PlaceOnGround();
                PreviewHeli.IsPositionFrozen = true;
                if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 0); // 0 per pula, 1 per medici.. funziona solo per i veicoli d'emergenza!
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
            MenuElicotteri.OnMenuClose += async (a) =>
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
            MenuElicotteri.Visible = true;
        }

        #endregion

        #region NewVehicleMenu

        private static List<Vehicle> parkVehicles = new List<Vehicle>();
        private static PoliceStation actualStation = new PoliceStation();
        private static int garageLevel = 0;
        private static List<Vector4> parkings = new List<Vector4>()
        {
            new Vector4(224.500f, -998.695f, -99.6f, 225.0f),
            new Vector4(224.500f, -994.630f, -99.6f, 225.0f),
            new Vector4(224.500f, -990.255f, -99.6f, 225.0f),
            new Vector4(224.500f, -986.628f, -99.6f, 225.0f),
            new Vector4(224.500f, -982.496f, -99.6f, 225.0f),
            new Vector4(232.500f, -982.496f, -99.6f, 135.0f),
            new Vector4(232.500f, -986.628f, -99.6f, 135.0f),
            new Vector4(232.500f, -990.255f, -99.6f, 135.0f),
            new Vector4(232.500f, -994.630f, -99.6f, 135.0f),
            new Vector4(232.500f, -998.695f, -99.6f, 135.0f)
        };
        private static SpawnerSpawn currentPoint = new SpawnerSpawn();
        private static bool InGarage = false;

        public static async void VehicleMenu(PoliceStation station, SpawnerSpawn point)
        {
            Cache.PlayerCache.MyPlayer.Status.Instance.InstancePlayer("PoliceVehicleGarage");
            actualStation = station;
            currentPoint = point;
            Cache.PlayerCache.MyPlayer.Ped.Position = new Vector3(236.349f, -1005.013f, -100f);
            Cache.PlayerCache.MyPlayer.Ped.Heading = 85.162f;
            InGarage = true;

            if (station.AuthorizedVehicles.Count(o => o.AuthorizedGrades[0] == -1 || o.AuthorizedGrades.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade)) <= 10)
            {
                for (int i = 0; i < station.AuthorizedVehicles.Count(o => o.AuthorizedGrades[0] == -1 || o.AuthorizedGrades.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade)); i++)
                {
                    parkVehicles.Add(await Functions.SpawnLocalVehicle(station.AuthorizedVehicles[i].Model, new Vector3(parkings[i].X, parkings[i].Y, parkings[i].Z), parkings[i].W));
                    parkVehicles[i].PlaceOnGround();
                    parkVehicles[i].IsPersistent = true;
                    parkVehicles[i].LockStatus = VehicleLockStatus.Unlocked;
                    parkVehicles[i].IsInvincible = true;
                    parkVehicles[i].IsCollisionEnabled = true;
                    parkVehicles[i].IsEngineRunning = false;
                    parkVehicles[i].IsDriveable = false;
                    parkVehicles[i].IsSirenActive = true;
                    parkVehicles[i].IsSirenSilent = true;
                    parkVehicles[i].SetDecor("VehiclePolice", SharedMath.GetRandomInt(100));
                }
            }
            else
                await GarageWithMoreVehicles(station.AuthorizedVehicles, garageLevel);

            await BaseScript.Delay(1000);
            Screen.Fading.FadeIn(800);
            Client.Instance.AddTick(ControlsGarageNew);
        }

        private static async Task GarageWithMoreVehicles(List<Authorized> authorized, int garageLevel)
        {
            foreach (Vehicle veh in parkVehicles) veh.Delete();
            parkVehicles.Clear();
            int total = authorized.Count(o => o.AuthorizedGrades[0] == -1 || o.AuthorizedGrades.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade));
            int currentGarageLevel = total - garageLevel * 10 > garageLevel * 10 ? 10 : total - garageLevel * 10;

            for (int i = 0; i < currentGarageLevel; i++)
            {
                parkVehicles.Add(await Functions.SpawnLocalVehicle(authorized[i + garageLevel * 10].Model, new Vector3(parkings[i].X, parkings[i].Y, parkings[i].Z), parkings[i].W));
                parkVehicles[i].PlaceOnGround();
                parkVehicles[i].IsPersistent = true;
                parkVehicles[i].LockStatus = VehicleLockStatus.Unlocked;
                parkVehicles[i].IsInvincible = true;
                parkVehicles[i].IsCollisionEnabled = true;
                parkVehicles[i].IsEngineRunning = false;
                parkVehicles[i].IsDriveable = false;
                parkVehicles[i].IsSirenActive = true;
                parkVehicles[i].IsSirenSilent = true;
                parkVehicles[i].SetDecor("VehiclePolice", SharedMath.GetRandomInt(100));
            }
        }

        #endregion

        #region MenuArmouries

        #endregion

        #region ControlsAndTask

        private static async Task ControlsGarageNew()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.Instance.Instanced)
            {
                if (InGarage)
                {
                    if (p.IsInRangeOf(new Vector3(240.317f, -1004.901f, -99f), 3f))
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to change floor");
                        if (Input.IsControlJustPressed(Control.Context)) MenuPiano();
                    }

                    if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                    {
                        if (p.CurrentVehicle.HasDecor("VehiclePolice"))
                        {
                            HUD.ShowHelp("To choose this vehicle and exit the garage,~n~~y~turn on the engine~w~ and ~y~accelerate~w~.");

                            if (Input.IsControlJustPressed(Control.VehicleAccelerate) && p.CurrentVehicle.IsEngineRunning)
                            {
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                int model = p.CurrentVehicle.Model.Hash;
                                foreach (Vehicle vehicle in parkVehicles) vehicle.Delete();
                                parkVehicles.Clear();

                                for (int i = 0; i < currentPoint.SpawnPoints.Count; i++)
                                {
                                    if (!Functions.IsSpawnPointClear(currentPoint.SpawnPoints[i].Coords.ToVector3, 2f))
                                    {
                                        continue;
                                    }
                                    else if (Functions.IsSpawnPointClear(currentPoint.SpawnPoints[i].Coords.ToVector3, 2f))
                                    {
                                        PoliceMainClient.CurrentVehicle = await Functions.SpawnVehicle(model, currentPoint.SpawnPoints[i].Coords.ToVector3, currentPoint.SpawnPoints[i].Heading);

                                        break;
                                    }
                                    else
                                    {
                                        PoliceMainClient.CurrentVehicle = await Functions.SpawnVehicle(model, currentPoint.SpawnPoints[0].Coords.ToVector3, currentPoint.SpawnPoints[0].Heading);

                                        break;
                                    }
                                }

                                p.CurrentVehicle.SetVehicleFuelLevel(100f);
                                p.CurrentVehicle.IsEngineRunning = true;
                                p.CurrentVehicle.IsDriveable = true;
                                p.CurrentVehicle.Mods.LicensePlate = SharedMath.GetRandomInt(99) + "POL" + SharedMath.GetRandomInt(999);
                                p.CurrentVehicle.SetDecor("VehiclePolice", SharedMath.GetRandomInt(100));
                                VehiclePolice veh = new VehiclePolice(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
                                BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehPolizia", veh.ToJson());
                                InGarage = false;
                                actualStation = null;
                                currentPoint = null;
                                parkVehicles.Clear();
                                Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
                                await BaseScript.Delay(1000);
                                Screen.Fading.FadeIn(800);
                                Client.Instance.RemoveTick(ControlsGarageNew);
                            }
                        }
                    }
                }
            }
        }

        private static async void MenuPiano()
        {
            UIMenu elevator = new("Select floor", "Upstairs or downstairs?", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem exit = new("Exit the Garage");
            elevator.AddItem(exit);
            int count = actualStation.AuthorizedVehicles.Count(o => o.AuthorizedGrades[0] == -1 || o.AuthorizedGrades.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade));
            int floors = 1;
            for (int i = 1; i < count + 1; i++)
            {
                if (i % 10 == 0)
                {
                    floors++;
                }
            }

            for (int i = 0; i < floors; i++)
            {
                UIMenuItem floor = new UIMenuItem($"{i + 1}° floor");
                elevator.AddItem(floor);
                if (i == garageLevel) floor.SetRightBadge(BadgeIcon.CAR);
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
                        Cache.PlayerCache.MyPlayer.Ped.Position = actualStation.Vehicles[actualStation.Vehicles.IndexOf(currentPoint)].SpawnerMenu.ToVector3;
                        InGarage = false;
                        actualStation = null;
                        currentPoint = null;
                        Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
                        parkVehicles.Clear();
                        Client.Instance.RemoveTick(ControlsGarageNew);
                    }
                    else
                    {
                        garageLevel = index - 1;
                        await GarageWithMoreVehicles(actualStation.AuthorizedVehicles, garageLevel);
                    }

                    await BaseScript.Delay(1000);
                    Screen.Fading.FadeIn(800);
                }
            };
            elevator.Visible = true;
        }

        private static async Task MenuControl()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
            {
                if (p.CurrentVehicle.Driver == Cache.PlayerCache.MyPlayer.Ped && p.CurrentVehicle.Speed < 2 || p.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Cache.PlayerCache.MyPlayer.Ped)
                {
                    if (CivilInteractionItem.Enabled)
                    {
                        CivilInteractionItem.Enabled = false;
                        CivilInteractionItem.SetRightBadge(BadgeIcon.LOCK);
                        CivilInteractionItem.Description += " - ~r~NOT~w~ available in a vehicle";
                    }

                    if (VehicleInteractionItem.Enabled)
                    {
                        VehicleInteractionItem.Enabled = false;
                        VehicleInteractionItem.SetRightBadge(BadgeIcon.LOCK);
                        VehicleInteractionItem.Description += " - ~r~NOT~w~ available in a vehicle";
                    }

                    if (ObjectsItem.Enabled)
                    {
                        ObjectsItem.Enabled = false;
                        ObjectsItem.SetRightBadge(BadgeIcon.LOCK);
                        ObjectsItem.Description += " - ~r~NOT~w~ available in a vehicle";
                    }

                    if (!RemotePersonCheckItem.Enabled)
                    {
                        RemotePersonCheckItem.Enabled = true;
                        RemotePersonCheckItem.SetRightBadge(BadgeIcon.NONE);
                        RemotePersonCheckItem.Description = "People Online Control System!";
                    }

                    if (!RemoveVehicleCheckItem.Enabled)
                    {
                        RemoveVehicleCheckItem.Enabled = true;
                        RemoveVehicleCheckItem.SetRightBadge(BadgeIcon.NONE);
                        RemoveVehicleCheckItem.Description = "Vehicle Online Control System!";
                    }
                }
            }
            else
            {
                if (!CivilInteractionItem.Enabled)
                {
                    CivilInteractionItem.Enabled = true;
                    CivilInteractionItem.SetRightBadge(BadgeIcon.NONE);
                    CivilInteractionItem.Description = "Show me your info!";
                }

                if (!VehicleInteractionItem.Enabled)
                {
                    VehicleInteractionItem.Enabled = true;
                    VehicleInteractionItem.SetRightBadge(BadgeIcon.NONE);
                    VehicleInteractionItem.Description = "Let me check your vehicle!";
                }

                if (!ObjectsItem.Enabled)
                {
                    ObjectsItem.Enabled = true;
                    ObjectsItem.SetRightBadge(BadgeIcon.NONE);
                    ObjectsItem.Description = "We also have spiked bands!";
                }

                if (RemotePersonCheckItem.Enabled)
                {
                    RemotePersonCheckItem.Enabled = false;
                    RemotePersonCheckItem.SetRightBadge(BadgeIcon.LOCK);
                    RemotePersonCheckItem.Description = RemotePersonCheckItem.Description + " - ~r~NOT~w~ available ouside a police vehicle or far from a police computer.";
                }

                if (RemoveVehicleCheckItem.Enabled)
                {
                    RemoveVehicleCheckItem.Enabled = false;
                    RemoveVehicleCheckItem.SetRightBadge(BadgeIcon.LOCK);
                    RemoveVehicleCheckItem.Description = RemoveVehicleCheckItem.Description + " - ~r~NOT~w~ available ouside a police vehicle or far from a police computer.";
                }
            }

            if (Client.Instance.GetPlayers.ToList().Count() > 1)
            {
                Tuple<Player, float> Player_Distance = Functions.GetClosestPlayer();
                float distance = Player_Distance.Item2;

                if (distance < 3)
                {
                    if (!CivilInteractionItem.Enabled)
                    {
                        CivilInteractionItem.Enabled = true;
                        CivilInteractionItem.SetRightBadge(BadgeIcon.NONE);
                        CivilInteractionItem.Description = "Show me your ID!";
                    }
                }
                else
                {
                    if (CivilInteractionItem.Enabled)
                    {
                        CivilInteractionItem.Enabled = false;
                        CivilInteractionItem.SetRightBadge(BadgeIcon.LOCK);
                        CivilInteractionItem.Description += "- ~r~NOT~w~ available in a vehicle";
                        if (CivilInteraction.Visible) CivilInteraction.Visible = false;
                    }
                }
            }
            else
            {
                if (CivilInteractionItem.Enabled)
                {
                    CivilInteractionItem.Enabled = false;
                    CivilInteractionItem.SetRightBadge(BadgeIcon.LOCK);
                    CivilInteractionItem.Description += "- ~r~NOT~w~ available in a vehicle";
                    if (CivilInteraction.Visible) CivilInteraction.Visible = false;
                }
            }

            PoliceMainMenu.UpdateDescription();
            await BaseScript.Delay(250);
        }

        private static async Task Heading()
        {
            if (PreviewHeli.Exists())
            {
                RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
                PreviewHeli.Heading += 1;
                if (!PreviewHeli.IsEngineRunning) SetHeliBladesFullSpeed(PreviewHeli.Handle);
            }

            await Task.FromResult(0);
        }

        #endregion
    }
}