﻿using System;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Veicoli
{
    internal static class MenuAffittoVeicoli
    {
        public static VeicoloAffitto veicoloInAffitto = null;
        public static Impostazioni.Client.Configurazione.Veicoli.VeicoliAffitto veicoliAff = Client.Impostazioni.RolePlay.Veicoli.veicoliAff;
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


            for (int i = 0; i < veicoliAff.biciclette.Count; i++)
            {
                UIMenuItem veicoloB = new UIMenuItem(veicoliAff.biciclette[i].name, veicoliAff.biciclette[i].description);
                veicoloB.SetRightLabel("~g~$" + veicoliAff.biciclette[i].price + " ~b~ogni 20 mins.");
                Bikes.AddItem(veicoloB);
            }

            for (int i = 0; i < veicoliAff.macchineGeneric.Count; i++)
            {
                UIMenuItem veicoloGC = new UIMenuItem(veicoliAff.macchineGeneric[i].name, veicoliAff.macchineGeneric[i].description);
                veicoloGC.SetRightLabel("~g~ $" + veicoliAff.macchineGeneric[i].price + " ~b~ogni 20 mins.");
                GenericCars.AddItem(veicoloGC);
                GenericCars.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.macchineGeneric[_index].price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.macchineGeneric[_index].model, veicoliAff.macchineGeneric[_index].name, veicoliAff.macchineGeneric[_index].price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.macchineGeneric[_index].price);
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
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.macchineGeneric[_index].price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.macchineGeneric[_index].model, veicoliAff.macchineGeneric[_index].name, veicoliAff.macchineGeneric[_index].price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.macchineGeneric[_index].price);
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

            for (int i = 0; i < veicoliAff.macchineMedium.Count; i++)
            {
                UIMenuItem veicoloMC = new UIMenuItem(veicoliAff.macchineMedium[i].name, veicoliAff.macchineMedium[i].description);
                veicoloMC.SetRightLabel("~g~$" + veicoliAff.macchineMedium[i].price + " ~b~ogni 20 mins.");
                MediumCars.AddItem(veicoloMC);
                MediumCars.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.macchineMedium[_index].price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.macchineMedium[_index].model, veicoliAff.macchineMedium[_index].name, veicoliAff.macchineMedium[_index].price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.macchineMedium[_index].price);
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
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.macchineMedium[_index].price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.macchineMedium[_index].model, veicoliAff.macchineMedium[_index].name, veicoliAff.macchineMedium[_index].price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.macchineMedium[_index].price);
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

            for (int i = 0; i < veicoliAff.macchineSuper.Count; i++)
            {
                UIMenuItem veicoloSC = new UIMenuItem(veicoliAff.macchineSuper[i].name, veicoliAff.macchineSuper[i].description);
                veicoloSC.SetRightLabel("~g~$" + veicoliAff.macchineSuper[i].price + " ~b~ogni 20 mins.");
                SuperCars.AddItem(veicoloSC);
                SuperCars.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.macchineSuper[_index].price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.macchineSuper[_index].model, veicoliAff.macchineSuper[_index].name, veicoliAff.macchineSuper[_index].price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.macchineSuper[_index].price);
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
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.macchineSuper[_index].price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.macchineSuper[_index].model, veicoliAff.macchineSuper[_index].name, veicoliAff.macchineSuper[_index].price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.macchineSuper[_index].price);
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

            for (int i = 0; i < veicoliAff.motoGeneric.Count; i++)
            {
                UIMenuItem veicoloMG = new UIMenuItem(veicoliAff.motoGeneric[i].name, veicoliAff.motoGeneric[i].description);
                veicoloMG.SetRightLabel("~g~$" + veicoliAff.motoGeneric[i].price + " ~b~ogni 20 mins.");
                GenericMoto.AddItem(veicoloMG);
                GenericMoto.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.motoGeneric[_index].price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.motoGeneric[_index].model, veicoliAff.motoGeneric[_index].name, veicoliAff.motoGeneric[_index].price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.motoGeneric[_index].price);
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
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.motoGeneric[_index].price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.motoGeneric[_index].model, veicoliAff.motoGeneric[_index].name, veicoliAff.motoGeneric[_index].price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.motoGeneric[_index].price);
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

            for (int i = 0; i < veicoliAff.motoMedium.Count; i++)
            {
                UIMenuItem veicoloMM = new UIMenuItem(veicoliAff.motoMedium[i].name, veicoliAff.motoMedium[i].description);
                veicoloMM.SetRightLabel("~g~$" + veicoliAff.motoMedium[i].price + " ~b~ogni 20 mins.");
                MediumMoto.AddItem(veicoloMM);
                MediumMoto.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.motoMedium[_index].price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.motoMedium[_index].model, veicoliAff.motoMedium[_index].name, veicoliAff.motoMedium[_index].price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.motoMedium[_index].price);
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
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.motoMedium[_index].price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.motoMedium[_index].model, veicoliAff.motoMedium[_index].name, veicoliAff.motoMedium[_index].price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.motoMedium[_index].price);
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

            for (int i = 0; i < veicoliAff.motoSuper.Count; i++)
            {
                UIMenuItem veicoloMS = new UIMenuItem(veicoliAff.motoSuper[i].name, veicoliAff.motoSuper[i].description);
                veicoloMS.SetRightLabel("~g~$" + veicoliAff.motoSuper[i].price + " ~b~ogni 20 mins.");
                SuperMoto.AddItem(veicoloMS);
                SuperMoto.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.motoSuper[_index].price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.motoSuper[_index].model, veicoliAff.motoSuper[_index].name, veicoliAff.motoSuper[_index].price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.motoSuper[_index].price);
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
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.motoSuper[_index].price)
                        {
                            if (veicoloInAffitto == null)
                            {
                                veicoloInAffitto = new VeicoloAffitto(veicoliAff.motoSuper[_index].model, veicoliAff.motoSuper[_index].name, veicoliAff.motoSuper[_index].price);
                                VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                                Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                                VeicoliClient.previewedVehicle.Delete();
                                VeicoliClient.setupGarageCamera(false, 0);
                                BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.motoSuper[_index].price);
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
                if (Cache.PlayerCache.MyPlayer.User.Money >= veicoliAff.biciclette[_index].price)
                {
                    if (veicoloInAffitto == null)
                    {
                        veicoloInAffitto = new VeicoloAffitto(veicoliAff.biciclette[_index].model, veicoliAff.biciclette[_index].name, veicoliAff.biciclette[_index].price);
                        VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                        Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                        VeicoliClient.previewedVehicle.Delete();
                        VeicoliClient.setupGarageCamera(false, 0);
                        BaseScript.TriggerServerEvent("lprp:removemoney", veicoliAff.biciclette[_index].price);
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
                    if (Cache.PlayerCache.MyPlayer.User.Bank >= veicoliAff.biciclette[_index].price)
                    {
                        if (veicoloInAffitto == null)
                        {
                            veicoloInAffitto = new VeicoloAffitto(veicoliAff.biciclette[_index].model, veicoliAff.biciclette[_index].name, veicoliAff.biciclette[_index].price);
                            VeicoliClient.spawnRentVehicle(veicoloInAffitto.model, num);
                            Client.Instance.AddTick(VeicoliClient.AffittoInCorso);
                            VeicoliClient.previewedVehicle.Delete();
                            VeicoliClient.setupGarageCamera(false, 0);
                            BaseScript.TriggerServerEvent("lprp:removebank", veicoliAff.biciclette[_index].price);
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
                VeicoliClient.SpawnVehiclePreview(veicoliAff.biciclette[_newIndex].model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            GenericCars.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.macchineGeneric[_newIndex].model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            MediumCars.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.macchineMedium[_newIndex].model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            SuperCars.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.macchineSuper[_newIndex].model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            GenericMoto.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.motoGeneric[_newIndex].model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            MediumMoto.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.motoMedium[_newIndex].model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
            };
            SuperMoto.OnIndexChange += async (_menu, _newIndex) =>
            {
                VeicoliClient.SpawnVehiclePreview(veicoliAff.motoSuper[_newIndex].model, new Vector3(VeicoliClient.carGaragePrev[num].X, VeicoliClient.carGaragePrev[num].Y, VeicoliClient.carGaragePrev[num].Z), VeicoliClient.carGaragePrev[num].W);
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