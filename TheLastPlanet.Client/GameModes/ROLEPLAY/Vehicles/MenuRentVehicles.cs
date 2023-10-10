using System;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Vehicles
{
    internal static class MenuRentVehicles
    {
        public static RentVehicle rentedVeh = null;
        public static Settings.Client.Configuration.Vehicles.VehicleRentClasses rentVehs = Client.Settings.RolePlay.Vehicles.RentVehicles;
        public static bool rented = false;

        public static async void RentMenu(int num)
        {
            Vehicle[] vehs = Functions.GetVehiclesInArea(new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), 5f);
            foreach (Vehicle v in vehs) v.Delete();
            UIMenu rentMenu = new UIMenu("LastPlanet Garages", "~b~We rent vehicles of all brands!!", new System.Drawing.Point(0, 0), Main.Textures["ModGarage"].Key, Main.Textures["ModGarage"].Value);
            UIMenuItem @return = new UIMenuItem("Return Vehicle", "There's no point paying for it if you no longer need it... give it back to us!");
            UIMenuItem rentVehItem = new("Rent Vehicle", "Yes we have it!");
            UIMenu rentVehMenu = new("Rent Vehicle", "We rent vehicles of all brands!!");
            rentVehItem.BindItemToMenu(rentVehMenu);
            rentMenu.AddItem(rentVehItem);
            rentMenu.AddItem(@return);
            UIMenuItem BikesItem = new("Bicycles", "To keep fit.. it really works!");
            UIMenu Bikes = new("Bicycles", "We rent vehicles of all brands!!");
            UIMenuItem GenericCarsItem = new("Generic Car Sharing", "Generic cars for the everyday citizen.");
            UIMenu GenericCars = new("Generic Car Sharing", "We rent vehicles of all brands!!");
            UIMenuItem MediumCarsItem = new("Medium Car Sharing", "Medium-range car for the citizen who wants more.");
            UIMenu MediumCars = new("Car Sharing Medium", "We rent vehicles of all brands!!");
            UIMenuItem SuperCarsItem = new("Car Sharing Super", "Super Car for the citizen who wants everything.");
            UIMenu SuperCars = new("Car Sharing Super", "We rent vehicles of all brands!!");
            UIMenuItem GenericBykeItem = new("Generic Bike Sharing", "Generic motorbikes for the everyday citizen");
            UIMenu GenericByke = new("Bike Sharing Generico", "We rent vehicles of all brands!!");
            UIMenuItem MediumBykeItem = new("Medium Bike Sharing", "Medium-range bike for the citizen who wants more");
            UIMenu MediumByke = new("Bike Sharing Medium", "We rent vehicles of all brands!!");
            UIMenuItem SuperBykeItem = new("Bike Sharing Super", "Super Moto for the citizen who wants everything");
            UIMenu SuperByke = new("Bike Sharing Super", "We rent vehicles of all brands!!");
            BikesItem.BindItemToMenu(Bikes);
            GenericCarsItem.BindItemToMenu(GenericCars);
            MediumCarsItem.BindItemToMenu(MediumCars);
            SuperCarsItem.BindItemToMenu(SuperCars);
            GenericBykeItem.BindItemToMenu(GenericByke);
            MediumBykeItem.BindItemToMenu(MediumByke);
            SuperBykeItem.BindItemToMenu(SuperByke);

            rentVehMenu.AddItem(BikesItem);
            rentVehMenu.AddItem(GenericCarsItem);
            rentVehMenu.AddItem(MediumCarsItem);
            rentVehMenu.AddItem(SuperCarsItem);
            rentVehMenu.AddItem(GenericBykeItem);
            rentVehMenu.AddItem(MediumBykeItem);
            rentVehMenu.AddItem(SuperBykeItem);


            for (int i = 0; i < rentVehs.Bycicles.Count; i++)
            {
                UIMenuItem vehicleB = new UIMenuItem(rentVehs.Bycicles[i].Name, rentVehs.Bycicles[i].Description);
                vehicleB.SetRightLabel("~g~$" + rentVehs.Bycicles[i].Price + " ~b~ every 20 minutes.");
                Bikes.AddItem(vehicleB);
            }

            for (int i = 0; i < rentVehs.CarsGeneric.Count; i++)
            {
                UIMenuItem vehicleGC = new UIMenuItem(rentVehs.CarsGeneric[i].Name, rentVehs.CarsGeneric[i].Description);
                vehicleGC.SetRightLabel("~g~ $" + rentVehs.CarsGeneric[i].Price + " ~b~ every 20 minutes.");
                GenericCars.AddItem(vehicleGC);
                GenericCars.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= rentVehs.CarsGeneric[_index].Price)
                    {
                        if (rentedVeh == null)
                        {
                            rentedVeh = new RentVehicle(rentVehs.CarsGeneric[_index].Model, rentVehs.CarsGeneric[_index].Name, rentVehs.CarsGeneric[_index].Price);
                            VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                            Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                            VehiclesClient.previewedVehicle.Delete();
                            VehiclesClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", rentVehs.CarsGeneric[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("You must first return it to rent another one!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= rentVehs.CarsGeneric[_index].Price)
                        {
                            if (rentedVeh == null)
                            {
                                rentedVeh = new RentVehicle(rentVehs.CarsGeneric[_index].Model, rentVehs.CarsGeneric[_index].Name, rentVehs.CarsGeneric[_index].Price);
                                VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                                Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                                VehiclesClient.previewedVehicle.Delete();
                                VehiclesClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", rentVehs.CarsGeneric[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("You must first return it to rent another one!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("You do NOT have enough ~b~Money~w~ to cover the first hour of vehicle rental!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < rentVehs.CarsMedium.Count; i++)
            {
                UIMenuItem vehicleMC = new UIMenuItem(rentVehs.CarsMedium[i].Name, rentVehs.CarsMedium[i].Description);
                vehicleMC.SetRightLabel("~g~$" + rentVehs.CarsMedium[i].Price + " ~b~ every 20 minutes.");
                MediumCars.AddItem(vehicleMC);
                MediumCars.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= rentVehs.CarsMedium[_index].Price)
                    {
                        if (rentedVeh == null)
                        {
                            rentedVeh = new RentVehicle(rentVehs.CarsMedium[_index].Model, rentVehs.CarsMedium[_index].Name, rentVehs.CarsMedium[_index].Price);
                            VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                            Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                            VehiclesClient.previewedVehicle.Delete();
                            VehiclesClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", rentVehs.CarsMedium[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("You must first return it to rent another one!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= rentVehs.CarsMedium[_index].Price)
                        {
                            if (rentedVeh == null)
                            {
                                rentedVeh = new RentVehicle(rentVehs.CarsMedium[_index].Model, rentVehs.CarsMedium[_index].Name, rentVehs.CarsMedium[_index].Price);
                                VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                                Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                                VehiclesClient.previewedVehicle.Delete();
                                VehiclesClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", rentVehs.CarsMedium[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("You must first return it to rent another one!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("You do NOT have enough ~b~Money~w~ to cover the first hour of vehicle rental!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < rentVehs.CarsSuper.Count; i++)
            {
                UIMenuItem vehicleSC = new UIMenuItem(rentVehs.CarsSuper[i].Name, rentVehs.CarsSuper[i].Description);
                vehicleSC.SetRightLabel("~g~$" + rentVehs.CarsSuper[i].Price + " ~b~ every 20 minutes.");
                SuperCars.AddItem(vehicleSC);
                SuperCars.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= rentVehs.CarsSuper[_index].Price)
                    {
                        if (rentedVeh == null)
                        {
                            rentedVeh = new RentVehicle(rentVehs.CarsSuper[_index].Model, rentVehs.CarsSuper[_index].Name, rentVehs.CarsSuper[_index].Price);
                            VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                            Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                            VehiclesClient.previewedVehicle.Delete();
                            VehiclesClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", rentVehs.CarsSuper[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("You must first return it to rent another one!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= rentVehs.CarsSuper[_index].Price)
                        {
                            if (rentedVeh == null)
                            {
                                rentedVeh = new RentVehicle(rentVehs.CarsSuper[_index].Model, rentVehs.CarsSuper[_index].Name, rentVehs.CarsSuper[_index].Price);
                                VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                                Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                                VehiclesClient.previewedVehicle.Delete();
                                VehiclesClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", rentVehs.CarsSuper[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("You must first return it to rent another one!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("You do NOT have enough ~b~Money~w~ to cover the first hour of vehicle rental!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < rentVehs.BykesGeneric.Count; i++)
            {
                UIMenuItem vehicleMG = new UIMenuItem(rentVehs.BykesGeneric[i].Name, rentVehs.BykesGeneric[i].Description);
                vehicleMG.SetRightLabel("~g~$" + rentVehs.BykesGeneric[i].Price + " ~b~ every 20 minutes.");
                GenericByke.AddItem(vehicleMG);
                GenericByke.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= rentVehs.BykesGeneric[_index].Price)
                    {
                        if (rentedVeh == null)
                        {
                            rentedVeh = new RentVehicle(rentVehs.BykesGeneric[_index].Model, rentVehs.BykesGeneric[_index].Name, rentVehs.BykesGeneric[_index].Price);
                            VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                            Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                            VehiclesClient.previewedVehicle.Delete();
                            VehiclesClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", rentVehs.BykesGeneric[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("You must first return it to rent another one!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= rentVehs.BykesGeneric[_index].Price)
                        {
                            if (rentedVeh == null)
                            {
                                rentedVeh = new RentVehicle(rentVehs.BykesGeneric[_index].Model, rentVehs.BykesGeneric[_index].Name, rentVehs.BykesGeneric[_index].Price);
                                VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                                Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                                VehiclesClient.previewedVehicle.Delete();
                                VehiclesClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", rentVehs.BykesGeneric[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("You must first return it to rent another one!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("You do NOT have enough ~b~Money~w~ to cover the first hour of vehicle rental!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < rentVehs.BykesMedium.Count; i++)
            {
                UIMenuItem vehicleMM = new UIMenuItem(rentVehs.BykesMedium[i].Name, rentVehs.BykesMedium[i].Description);
                vehicleMM.SetRightLabel("~g~$" + rentVehs.BykesMedium[i].Price + " ~b~ every 20 minutes.");
                MediumByke.AddItem(vehicleMM);
                MediumByke.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= rentVehs.BykesMedium[_index].Price)
                    {
                        if (rentedVeh == null)
                        {
                            rentedVeh = new RentVehicle(rentVehs.BykesMedium[_index].Model, rentVehs.BykesMedium[_index].Name, rentVehs.BykesMedium[_index].Price);
                            VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                            Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                            VehiclesClient.previewedVehicle.Delete();
                            VehiclesClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", rentVehs.BykesMedium[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("You must first return it to rent another one!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= rentVehs.BykesMedium[_index].Price)
                        {
                            if (rentedVeh == null)
                            {
                                rentedVeh = new RentVehicle(rentVehs.BykesMedium[_index].Model, rentVehs.BykesMedium[_index].Name, rentVehs.BykesMedium[_index].Price);
                                VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                                Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                                VehiclesClient.previewedVehicle.Delete();
                                VehiclesClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", rentVehs.BykesMedium[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("You must first return it to rent another one!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("You do NOT have enough ~b~Money~w~ to cover the first hour of vehicle rental!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < rentVehs.BykesSuper.Count; i++)
            {
                UIMenuItem vehicleMS = new UIMenuItem(rentVehs.BykesSuper[i].Name, rentVehs.BykesSuper[i].Description);
                vehicleMS.SetRightLabel("~g~$" + rentVehs.BykesSuper[i].Price + " ~b~ every 20 minutes.");
                SuperByke.AddItem(vehicleMS);
                SuperByke.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= rentVehs.BykesSuper[_index].Price)
                    {
                        if (rentedVeh == null)
                        {
                            rentedVeh = new RentVehicle(rentVehs.BykesSuper[_index].Model, rentVehs.BykesSuper[_index].Name, rentVehs.BykesSuper[_index].Price);
                            VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                            Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                            VehiclesClient.previewedVehicle.Delete();
                            VehiclesClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", rentVehs.BykesSuper[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("You must first return it to rent another one!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= rentVehs.BykesSuper[_index].Price)
                        {
                            if (rentedVeh == null)
                            {
                                rentedVeh = new RentVehicle(rentVehs.BykesSuper[_index].Model, rentVehs.BykesSuper[_index].Name, rentVehs.BykesSuper[_index].Price);
                                VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                                Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                                VehiclesClient.previewedVehicle.Delete();
                                VehiclesClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", rentVehs.BykesSuper[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("You must first return it to rent another one!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("You do NOT have enough ~b~Money~w~ to cover the first hour of vehicle rental!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            Bikes.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (Cache.PlayerCache.MyPlayer.User.Money >= rentVehs.Bycicles[_index].Price)
                {
                    if (rentedVeh == null)
                    {
                        rentedVeh = new RentVehicle(rentVehs.Bycicles[_index].Model, rentVehs.Bycicles[_index].Name, rentVehs.Bycicles[_index].Price);
                        VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                        Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                        VehiclesClient.previewedVehicle.Delete();
                        VehiclesClient.setupGarageCamera(false, 0);
                        BaseScript.TriggerServerEvent("lprp:removemoney", rentVehs.Bycicles[_index].Price);
                        BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                    }
                    else
                    {
                        HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                        await BaseScript.Delay(500);
                        HUD.ShowNotification("You must first return it to rent another one!");
                    }
                }
                else
                {
                    if (Cache.PlayerCache.MyPlayer.User.Bank >= rentVehs.Bycicles[_index].Price)
                    {
                        if (rentedVeh == null)
                        {
                            rentedVeh = new RentVehicle(rentVehs.Bycicles[_index].Model, rentVehs.Bycicles[_index].Name, rentVehs.Bycicles[_index].Price);
                            VehiclesClient.spawnRentVehicle(rentedVeh.model, num);
                            Client.Instance.AddTick(VehiclesClient.AffittoInCorso);
                            VehiclesClient.previewedVehicle.Delete();
                            VehiclesClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removebank", rentVehs.Bycicles[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {rentedVeh.name} al prezzo di ${rentedVeh.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("You have already rented a ~b~vehicle~w~ model ~o~" + rentedVeh.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("You must first return it to rent another one!");
                        }
                    }
                    else
                    {
                        HUD.ShowNotification("You do NOT have enough ~b~Money~w~ to cover the first hour of vehicle rental!", ColoreNotifica.Red, true);
                    }
                }
            };
            Bikes.OnIndexChange += async (_menu, _newIndex) =>
            {
                VehiclesClient.SpawnVehiclePreview(rentVehs.Bycicles[_newIndex].Model, new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), VehiclesClient.carGaragePrev[num].W);
            };
            GenericCars.OnIndexChange += async (_menu, _newIndex) =>
            {
                VehiclesClient.SpawnVehiclePreview(rentVehs.CarsGeneric[_newIndex].Model, new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), VehiclesClient.carGaragePrev[num].W);
            };
            MediumCars.OnIndexChange += async (_menu, _newIndex) =>
            {
                VehiclesClient.SpawnVehiclePreview(rentVehs.CarsMedium[_newIndex].Model, new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), VehiclesClient.carGaragePrev[num].W);
            };
            SuperCars.OnIndexChange += async (_menu, _newIndex) =>
            {
                VehiclesClient.SpawnVehiclePreview(rentVehs.CarsSuper[_newIndex].Model, new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), VehiclesClient.carGaragePrev[num].W);
            };
            GenericByke.OnIndexChange += async (_menu, _newIndex) =>
            {
                VehiclesClient.SpawnVehiclePreview(rentVehs.BykesGeneric[_newIndex].Model, new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), VehiclesClient.carGaragePrev[num].W);
            };
            MediumByke.OnIndexChange += async (_menu, _newIndex) =>
            {
                VehiclesClient.SpawnVehiclePreview(rentVehs.BykesMedium[_newIndex].Model, new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), VehiclesClient.carGaragePrev[num].W);
            };
            SuperByke.OnIndexChange += async (_menu, _newIndex) =>
            {
                VehiclesClient.SpawnVehiclePreview(rentVehs.BykesSuper[_newIndex].Model, new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), VehiclesClient.carGaragePrev[num].W);
            };

            rentVehMenu.OnMenuOpen += async (a, b) =>
            {
                await BaseScript.Delay(100);
                Vehicle[] ves = Functions.GetVehiclesInArea(new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), 1.0f);
                foreach (Vehicle v in ves) v.Delete();
                VehiclesClient.setupGarageCamera(true, num);
            };
            rentMenu.OnMenuClose += async (a) =>
            {
                await BaseScript.Delay(100);

                if (!Bikes.Visible && !GenericCars.Visible && !MediumCars.Visible && !SuperCars.Visible && !GenericByke.Visible && !MediumByke.Visible && !SuperByke.Visible)
                {
                    if (!rentVehMenu.Visible) VehiclesClient.setupGarageCamera(false, 0);
                    Vehicle[] ves = Functions.GetVehiclesInArea(new Vector3(VehiclesClient.carGaragePrev[num].X, VehiclesClient.carGaragePrev[num].Y, VehiclesClient.carGaragePrev[num].Z), 1.0f);
                    foreach (Vehicle v in ves) v.Delete();
                }
            };

            rentMenu.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (_item == @return)
                {
                    HUD.ShowNotification("Grazie di aver utilizzato LastPlanet Affitto Veicoli!", ColoreNotifica.GreenLight);
                    BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena appena restituito il vehicle {rentedVeh.name} affittato");
                    rentedVeh = null;
                    VehiclesClient.rentVehicle.Delete();
                    Client.Instance.RemoveTick(VehiclesClient.AffittoInCorso);
                    await BaseScript.Delay(1000);
                    HUD.ShowNotification("Il vehicle che hai affittato è stato riportato al garage di competenza.");
                }
            };
            rentMenu.Visible = true;
        }
    }

    internal class RentVehicle
    {
        public int price;
        public string name;
        public string model;

        public RentVehicle(string model, string name, int price)
        {
            this.model = model;
            this.name = name;
            this.price = price;
        }
    }
}