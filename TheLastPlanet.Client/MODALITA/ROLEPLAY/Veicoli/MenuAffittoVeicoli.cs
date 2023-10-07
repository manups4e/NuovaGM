using System;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Veicoli
{
    internal static class MenuAffittoVeicoli
    {
        public static VeicoloAffitto veicoloInAffitto = null;
        public static Settings.Client.Configurazione.Vehicles.VehicleRentClasses veicoliAff = Client.Impostazioni.RolePlay.Vehicles.RentVehicles;
        public static bool affittato = false;

        public static async void MenuAffitto(int num)
        {
            Vehicle[] vehs = Funzioni.GetVehiclesInArea(new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), 5f);
            foreach (Vehicle v in vehs) v.Delete();
            UIMenu MenuAffitto = new UIMenu("LastPlanet Garages", "~b~Affittasi veicoli di tutte le marche!!", new System.Drawing.Point(0, 0), Main.Textures["ModGarage"].Key, Main.Textures["ModGarage"].Value);
            UIMenuItem rest = new UIMenuItem("Restituisci Veicolo", "E' inutile pagarlo se non ti serve più... ridaccelo!");
            UIMenuItem affittaVehItem = new("Affitta Veicolo", "Si lo abbiamo!");
            UIMenu affittaVeh = new("Affitta Veicolo", "Si lo abbiamo!");
            affittaVehItem.BindItemToMenu(affittaVeh);
            MenuAffitto.AddItem(affittaVehItem);
            MenuAffitto.AddItem(rest);
            UIMenuItem BikesItem = new("Biciclette", "Per tenersi in forma.. funziona davvero!");
            UIMenu Bikes = new("Biciclette", "Per tenersi in forma.. funziona davvero!");
            UIMenuItem GenericCarsItem = new("Car Sharing Generico", "Auto generiche per il cittadino di tutti i giorni.");
            UIMenu GenericCars = new("Car Sharing Generico", "Auto generiche per il cittadino di tutti i giorni.");
            UIMenuItem MediumCarsItem = new("Car Sharing Medio", "Auto di media portata per il cittadino che vuole di più.");
            UIMenu MediumCars = new("Car Sharing Medio", "Auto di media portata per il cittadino che vuole di più.");
            UIMenuItem SuperCarsItem = new("Car Sharing Super", "Super Auto per il cittadino che vuole tutto.");
            UIMenu SuperCars = new("Car Sharing Super", "Super Auto per il cittadino che vuole tutto.");
            UIMenuItem GenericMotoItem = new("Bike Sharing Generico", "Moto generiche per il cittadino di tutti i giorni");
            UIMenu GenericMoto = new("Bike Sharing Generico", "Moto generiche per il cittadino di tutti i giorni");
            UIMenuItem MediumMotoItem = new("Bike Sharing Medio", "Moto di media portata per il cittadino che vuole di più");
            UIMenu MediumMoto = new("Bike Sharing Medio", "Moto di media portata per il cittadino che vuole di più");
            UIMenuItem SuperMotoItem = new("Bike Sharing Super", "Super Moto per il cittadino che vuole tutto");
            UIMenu SuperMoto = new("Bike Sharing Super", "Super Moto per il cittadino che vuole tutto");

            BikesItem.BindItemToMenu(Bikes);
            GenericCarsItem.BindItemToMenu(GenericCars);
            MediumCarsItem.BindItemToMenu(MediumCars);
            SuperCarsItem.BindItemToMenu(SuperCars);
            GenericMotoItem.BindItemToMenu(GenericMoto);
            MediumMotoItem.BindItemToMenu(MediumMoto);
            SuperMotoItem.BindItemToMenu(SuperMoto);

            affittaVeh.AddItem(BikesItem);
            affittaVeh.AddItem(GenericCarsItem);
            affittaVeh.AddItem(MediumCarsItem);
            affittaVeh.AddItem(SuperCarsItem);
            affittaVeh.AddItem(GenericMotoItem);
            affittaVeh.AddItem(MediumMotoItem);
            affittaVeh.AddItem(SuperMotoItem);


            for (int i = 0; i < veicoliAff.Bycicles.Count; i++)
            {
                UIMenuItem veicoloB = new UIMenuItem(veicoliAff.Bycicles[i].Name, veicoliAff.Bycicles[i].Description);
                veicoloB.SetRightLabel("~g~$" + veicoliAff.Bycicles[i].Price + " ~b~ogni 20 mins.");
                Bikes.AddItem(veicoloB);
            }

            for (int i = 0; i < veicoliAff.CarsGeneric.Count; i++)
            {
                UIMenuItem veicoloGC = new UIMenuItem(veicoliAff.CarsGeneric[i].Name, veicoliAff.CarsGeneric[i].Description);
                veicoloGC.SetRightLabel("~g~ $" + veicoliAff.CarsGeneric[i].Price + " ~b~ogni 20 mins.");
                GenericCars.AddItem(veicoloGC);
                GenericCars.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.CarsGeneric[_index].Price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.CarsGeneric[_index].Model, veicoliAff.CarsGeneric[_index].Name, veicoliAff.CarsGeneric[_index].Price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.CarsGeneric[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.CarsGeneric[_index].Price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.CarsGeneric[_index].Model, veicoliAff.CarsGeneric[_index].Name, veicoliAff.CarsGeneric[_index].Price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.CarsGeneric[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("NON hai abbastanza ~b~Soldi~w~ per coprire la prima ora di affitto del veicolo!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < veicoliAff.CarsMedium.Count; i++)
            {
                UIMenuItem veicoloMC = new UIMenuItem(veicoliAff.CarsMedium[i].Name, veicoliAff.CarsMedium[i].Description);
                veicoloMC.SetRightLabel("~g~$" + veicoliAff.CarsMedium[i].Price + " ~b~ogni 20 mins.");
                MediumCars.AddItem(veicoloMC);
                MediumCars.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.CarsMedium[_index].Price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.CarsMedium[_index].Model, veicoliAff.CarsMedium[_index].Name, veicoliAff.CarsMedium[_index].Price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.CarsMedium[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.CarsMedium[_index].Price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.CarsMedium[_index].Model, veicoliAff.CarsMedium[_index].Name, veicoliAff.CarsMedium[_index].Price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.CarsMedium[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("NON hai abbastanza ~b~Soldi~w~ per coprire la prima ora di affitto del veicolo!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < veicoliAff.CarsSuper.Count; i++)
            {
                UIMenuItem veicoloSC = new UIMenuItem(veicoliAff.CarsSuper[i].Name, veicoliAff.CarsSuper[i].Description);
                veicoloSC.SetRightLabel("~g~$" + veicoliAff.CarsSuper[i].Price + " ~b~ogni 20 mins.");
                SuperCars.AddItem(veicoloSC);
                SuperCars.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.CarsSuper[_index].Price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.CarsSuper[_index].Model, veicoliAff.CarsSuper[_index].Name, veicoliAff.CarsSuper[_index].Price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.CarsSuper[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.CarsSuper[_index].Price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.CarsSuper[_index].Model, veicoliAff.CarsSuper[_index].Name, veicoliAff.CarsSuper[_index].Price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.CarsSuper[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("NON hai abbastanza ~b~Soldi~w~ per coprire la prima ora di affitto del veicolo!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < veicoliAff.BykesGeneric.Count; i++)
            {
                UIMenuItem veicoloMG = new UIMenuItem(veicoliAff.BykesGeneric[i].Name, veicoliAff.BykesGeneric[i].Description);
                veicoloMG.SetRightLabel("~g~$" + veicoliAff.BykesGeneric[i].Price + " ~b~ogni 20 mins.");
                GenericMoto.AddItem(veicoloMG);
                GenericMoto.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.BykesGeneric[_index].Price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.BykesGeneric[_index].Model, veicoliAff.BykesGeneric[_index].Name, veicoliAff.BykesGeneric[_index].Price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.BykesGeneric[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.BykesGeneric[_index].Price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.BykesGeneric[_index].Model, veicoliAff.BykesGeneric[_index].Name, veicoliAff.BykesGeneric[_index].Price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.BykesGeneric[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("NON hai abbastanza ~b~Soldi~w~ per coprire la prima ora di affitto del veicolo!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < veicoliAff.BykesMedium.Count; i++)
            {
                UIMenuItem veicoloMM = new UIMenuItem(veicoliAff.BykesMedium[i].Name, veicoliAff.BykesMedium[i].Description);
                veicoloMM.SetRightLabel("~g~$" + veicoliAff.BykesMedium[i].Price + " ~b~ogni 20 mins.");
                MediumMoto.AddItem(veicoloMM);
                MediumMoto.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.BykesMedium[_index].Price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.BykesMedium[_index].Model, veicoliAff.BykesMedium[_index].Name, veicoliAff.BykesMedium[_index].Price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.BykesMedium[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.BykesMedium[_index].Price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.BykesMedium[_index].Model, veicoliAff.BykesMedium[_index].Name, veicoliAff.BykesMedium[_index].Price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.BykesMedium[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("NON hai abbastanza ~b~Soldi~w~ per coprire la prima ora di affitto del veicolo!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            for (int i = 0; i < veicoliAff.BykesSuper.Count; i++)
            {
                UIMenuItem veicoloMS = new UIMenuItem(veicoliAff.BykesSuper[i].Name, veicoliAff.BykesSuper[i].Description);
                veicoloMS.SetRightLabel("~g~$" + veicoliAff.BykesSuper[i].Price + " ~b~ogni 20 mins.");
                SuperMoto.AddItem(veicoloMS);
                SuperMoto.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.BykesSuper[_index].Price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.BykesSuper[_index].Model, veicoliAff.BykesSuper[_index].Name, veicoliAff.BykesSuper[_index].Price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.BykesSuper[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.BykesSuper[_index].Price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.BykesSuper[_index].Model, veicoliAff.BykesSuper[_index].Name, veicoliAff.BykesSuper[_index].Price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.BykesSuper[_index].Price);
                                BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                            }
                            else
                            {
                                HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                                await BaseScript.Delay(500);
                                HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("NON hai abbastanza ~b~Soldi~w~ per coprire la prima ora di affitto del veicolo!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            Bikes.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.Bycicles[_index].Price)
                {
                    if (veicoloInAffitto == null)
                    {
                        veicoloInAffitto = new VeicoloAffitto(veicoliAff.Bycicles[_index].Model, veicoliAff.Bycicles[_index].Name, veicoliAff.Bycicles[_index].Price);
                        VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                        Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                        VeicoliClient.previewedVehicle.Delete();
                        VeicoliClient.setupGarageCamera(false, 0);
                        BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.Bycicles[_index].Price);
                        BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                    }
                    else
                    {
                        HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                        await BaseScript.Delay(500);
                        HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                    }
                }
                else
                {
                    if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.Bycicles[_index].Price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.Bycicles[_index].Model, veicoliAff.Bycicles[_index].Name, veicoliAff.Bycicles[_index].Price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.Bycicles[_index].Price);
                            BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena affittato un/una {veicoloInAffitto.name} al prezzo di ${veicoloInAffitto.price} ");
                        }
                        else
                        {
                            HUD.ShowNotification("Hai già affitato un ~b~veicolo~w~ modello ~o~" + veicoloInAffitto.name + "~w~!");
                            await BaseScript.Delay(500);
                            HUD.ShowNotification("Devi prima restituirlo per affittarne un altro!");
                        }
                    }
                    else
                    {
                        HUD.ShowNotification("NON hai abbastanza ~b~Soldi~w~ per coprire la prima ora di affitto del veicolo!", ColoreNotifica.Red, true);
                    }
                }
            };
            Bikes.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.Bycicles[_newIndex].Model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            GenericCars.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.CarsGeneric[_newIndex].Model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            MediumCars.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.CarsMedium[_newIndex].Model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            SuperCars.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.CarsSuper[_newIndex].Model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            GenericMoto.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.BykesGeneric[_newIndex].Model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            MediumMoto.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.BykesMedium[_newIndex].Model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            SuperMoto.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.BykesSuper[_newIndex].Model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };

            affittaVeh.OnMenuOpen += async (a, b) =>
            {
                await BaseScript.Delay(100);
                Vehicle[] ves = Funzioni.GetVehiclesInArea(new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), 1.0f);
                foreach (Vehicle v in ves) v.Delete();
                VeicoliClient.setupGarageCamera(true, num);
            };
            MenuAffitto.OnMenuClose += async (a) =>
            {
                await BaseScript.Delay(100);

                if (!Bikes.Visible && !GenericCars.Visible && !MediumCars.Visible && !SuperCars.Visible && !GenericMoto.Visible && !MediumMoto.Visible && !SuperMoto.Visible)
                {
                    if (!affittaVeh.Visible) VeicoliClient.setupGarageCamera(false, 0);
                    Vehicle[] ves = Funzioni.GetVehiclesInArea(new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), 1.0f);
                    foreach (Vehicle v in ves) v.Delete();
                }
            };

            MenuAffitto.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (_item == rest)
                {
                    HUD.ShowNotification("Grazie di aver utilizzato LastPlanet Affitto Veicoli!", ColoreNotifica.GreenLight);
                    BaseScript.TriggerServerEvent("lprp:serverlog DA CAMBIARE PER LOG!", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha appena appena restituito il veicolo {veicoloInAffitto.name} affittato");
                    veicoloInAffitto = null;
                    VeicoliClient.veicoloinaffitto.Delete();
                    Client.Instance.RemoveTick(VeicoliClient.AffittoInCorso);
                    await BaseScript.Delay(1000);
                    HUD.ShowNotification("Il veicolo che hai affittato è stato riportato al garage di competenza.");
                }
            };
            MenuAffitto.Visible = true;
        }
    }

    internal class VeicoloAffitto
    {
        public int price;
        public string name;
        public string model;

        public VeicoloAffitto(string model, string name, int price)
        {
            this.model = model;
            this.name = name;
            this.price = price;
        }
    }
}