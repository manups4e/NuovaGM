using Impostazioni.Shared.Configurazione.Generici;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core.Status;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Giostre;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Veicoli;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Personale
{
    internal static class PersonalMenu
    {
        private static UIMenuItem fa = new UIMenuItem("Fame");
        private static UIMenuItem se = new UIMenuItem("Sete");
        private static UIMenuItem st = new UIMenuItem("Stanchezza");
        private static UIMenuItem ma = new UIMenuItem("Stato Clinico");

        public static List<dynamic> personaggi = new List<dynamic>();
        public static float interactionDistance = 3.5f;
        public static int lockDistance = 25;
        public static string itemgps = "nessuno";
        public static bool salvato = false;
        public static bool acceso = false;
        public static bool chiuso = false;
        public static bool attivo = false;
        public static bool GranaCinema = false;
        private static bool aperto = false;
        private static float fuelint = 0;
        public static List<dynamic> gps = new List<dynamic>()
        {
            "Nessuno",
            "Centro per l'Impiego",
            "Stazione di polizia",
            "Ospedale",
            "Banca",
            "Negozio",
            "Negozio d'armi",
            "Negozio di animali",
            "Concessionario",
            "Parcheggio Affitto Auto",
            "Negozio di Abbigliamento",
            "Barbiere",
            "Autoscuola"
        };
        private static List<dynamic> finestrini = new List<dynamic>() { "Anteriore Sinistro", "Anteriore Destro", "Posteriore Sinistro", "Posteriore Destro" };
        private static List<dynamic> portiere = new List<dynamic>()
        {
            "Anteriore Sinistra",
            "Anteriore Destra",
            "Posteriore Sinistra",
            "Posteriore Destra",
            "Cofano",
            "Bagagliaio"
        };
        private static List<dynamic> chiusure = new List<dynamic>() { "Chiusa", "Aperta" };
        public static Blip b;
        public static bool blipFound = false;

        public static async void menuPersonal()
        {
            MenuHandler.CloseAndClearHistory();
            Ped playerPed = PlayerCache.MyPlayer.Ped;
            Player me = PlayerCache.MyPlayer.Player;
            Point pos = new Point(50, 50);
            UIMenu PersonalMenu = new UIMenu(Game.Player.Name, "~g~A portata di mano~w~", pos, "thelastgalaxy", "bannerbackground", false, true);

            #region GPS Veloce

            UIMenuListItem gpsItem = new UIMenuListItem("GPS Veloce", gps, 0);
            PersonalMenu.AddItem(gpsItem);
            PersonalMenu.OnListSelect += async (menu, _item, _itemIndex) =>
            {
                if (_item != gpsItem) return;
                int var;

                switch (_item.Items[_item.Index])
                {
                    case "Centro per l'Impiego":
                        var = 407;

                        break;
                    case "Stazione di polizia":
                        var = 60;

                        break;
                    case "Ospedale":
                        var = 80;

                        break;
                    case "Banca":
                        var = 108;

                        break;
                    case "Negozio":
                        var = 52;

                        break;
                    case "Negozio d'armi":
                        var = 154;

                        break;
                    case "Negozio di animali":
                        var = 463;

                        break;
                    case "Concessionario":
                        var = 225;

                        break;
                    case "Parcheggio Affitto Auto":
                        var = 50;

                        break;
                    case "Negozio di Abbigliamento":
                        var = 73;

                        break;
                    case "Barbiere":
                        var = 71;

                        break;
                    case "Autoscuola":
                        var = 408;

                        break;
                    default:
                        var = 0;

                        break;
                }

                if ((string)_item.Items[_item.Index] == "Nessuno")
                {
                    try
                    {
                        if (b.Exists())
                        {
                            SetWaypointOff();
                            b.ShowRoute = false;
                            Client.Instance.RemoveTick(routeColor);
                            HUD.ShowNotification("GPS: Destinazione rimossa.", true);

                            return;
                        }
                        else
                        {
                            HUD.ShowNotification("GPS: Nessuna destinazione impostata!!", true);
                        }
                    }
                    catch (Exception e)
                    {
                        Client.Logger.Debug(e.ToString() + e.StackTrace);
                        HUD.ShowNotification("Nessuna destinazione impostata!!", true);
                    }
                }
                else
                {
                    Blip[] test = World.GetAllBlips((BlipSprite)var);
                    HUD.ShowNotification("GPS: Calcolo..");
                    await BaseScript.Delay(1000);
                    b = test.ToList().OrderBy(x => PlayerCache.MyPlayer.Posizione.Distance(x.Position)).FirstOrDefault();
                    HUD.ShowNotification("GPS: Calcolo..").Hide();

                    if (b != null)
                    {
                        b.ShowRoute = true;
                        HUD.ShowNotification($"Destinazione più vicina impostata per {_item.Items[_item.Index]}.");
                        Client.Instance.AddTick(routeColor);
                    }
                    else
                    {
                        HUD.ShowNotification("Destinazione non trovata!");
                    }
                }
            };

            #endregion

            #region Controlli Veicolo
            UIMenuItem vehContrItem = new UIMenuItem("Controlli Veicolo", "Controlla il tuo veicolo con questi comandi rapidi");
            UIMenu vehContr = new("Controlli Veicolo", "");
            vehContrItem.BindItemToMenu(vehContr);
            PersonalMenu.AddItem(vehContrItem);
            UIMenuItem fuel = new UIMenuItem("Carburante veicolo salvato", "Facciamo il pieno?");
            UIMenuCheckboxItem salva = new UIMenuCheckboxItem("Salva Veicolo", salvato, "Salva qui il tuo veicolo!");
            UIMenuCheckboxItem chiudi = new UIMenuCheckboxItem("Serratura", chiuso, "Apri o chiudi il tuo veicolo (Salvalo prima!)");
            UIMenuListItem fin = new UIMenuListItem("Alza/Abbassa finestrini", finestrini, 0, "Alza e abbassa i finestrini del veicolo");
            UIMenuListItem port = new UIMenuListItem("Apri/Chiudi Portiere", portiere, 0, "Apri e chiudi le portiere della tua macchina");
            UIMenuCheckboxItem motore = new UIMenuCheckboxItem("Accensione Remota", EventiPersonalMenu.saveVehicle != null ? EventiPersonalMenu.saveVehicle.IsEngineRunning : false, "Accensione del motore remota!");
            vehContr.AddItem(fuel);
            vehContr.AddItem(salva);
            vehContr.AddItem(chiudi);
            vehContr.AddItem(fin);
            vehContr.AddItem(port);
            vehContr.AddItem(motore);

            if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo)
            {
                chiudi.Enabled = false;
                fin.Enabled = false;
                port.Enabled = false;
                motore.Enabled = false;
            }

            vehContr.OnCheckboxChange += async (_menu, _item, _checked) =>
            {
                if (_item == chiudi)
                    EventiPersonalMenu.Lock(_checked);
                else if (_item == salva)
                    switch (_checked)
                    {
                        case true when Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo:
                            {
                                EventiPersonalMenu.Save(_checked);

                                if (_checked)
                                {
                                    chiudi.Enabled = true;
                                    fin.Enabled = true;
                                    port.Enabled = true;
                                    motore.Enabled = true;
                                    fuel.SetRightLabel(fuelint + "%");
                                }
                                else
                                {
                                    chiudi.Enabled = false;
                                    fin.Enabled = false;
                                    port.Enabled = false;
                                    motore.Enabled = false;
                                    fuel.SetRightLabel("nessun veicolo salvato");
                                }

                                break;
                            }
                        case false:
                            EventiPersonalMenu.Save(_checked);
                            chiudi.Enabled = false;
                            fin.Enabled = false;
                            port.Enabled = false;
                            motore.Enabled = false;
                            fuel.SetRightLabel("nessun veicolo salvato");

                            break;
                        default:
                            HUD.ShowNotification("Devi essere in un veicolo per attivare la funzione di salvataggio", true);

                            break;
                    }
                else if (_item == motore) EventiPersonalMenu.motore(_checked);
            };
            vehContr.OnListSelect += (_menu, _listItem, _itemIndex) =>
            {
                if (_listItem == fin)
                    EventiPersonalMenu.Finestrini(_listItem.Items[_listItem.Index].ToString());
                else if (_listItem == port) EventiPersonalMenu.Portiere(_listItem.Items[_listItem.Index].ToString());
            };

            #endregion

            #region Personaggio e dati personali
            UIMenuItem persMenuItem = new("Personaggio Attuale");
            UIMenuItem datiPersItem = new("Dati Personaggio", "Imparati");

            UIMenu persMenu = new("Personaggio Attuale", "");
            UIMenu datiPers = new("Dati Personaggio", "Imparati");

            persMenuItem.BindItemToMenu(persMenu);
            datiPersItem.BindItemToMenu(datiPers);

            PersonalMenu.AddItem(persMenuItem);
            persMenu.AddItem(datiPersItem);

            datiPers.OnMenuOpen += async (a, b) =>
            {
                datiPers.Clear();
                UIMenuItem name = new UIMenuItem("Nome: ", "Il suo nome");
                UIMenuItem dob = new UIMenuItem("Data di Nascita: ", "La sua Data di nascita");
                UIMenuItem alt = new UIMenuItem("Altezza: ", "La sua Altezza");
                UIMenuItem nTel = new UIMenuItem("N° Telefono: ", "Segnatelo se vuoi");
                UIMenuItem nAss = new UIMenuItem("N° Assicurazione: ", "Segnatelo se vuoi");
                UIMenuItem job = new UIMenuItem("Lavoro: ", "Il suo lavoro");
                UIMenuItem gang = new UIMenuItem("Gang: ", "Le affiliazioni");
                UIMenuItem bank = new UIMenuItem("Banca: ", "I soldi in banca");
                name.SetRightLabel(me.GetPlayerData().FullName);
                dob.SetRightLabel(me.GetPlayerData().DoB);
                alt.SetRightLabel("" + me.GetPlayerData().CurrentChar.Info.height);
                nTel.SetRightLabel("" + me.GetPlayerData().CurrentChar.Info.phoneNumber);
                nAss.SetRightLabel("" + me.GetPlayerData().CurrentChar.Info.insurance);
                //
                //job.Label = SharedScript.Jobs[me.GetPlayerData().CurrentChar.job.name].label;
                //gang.Label = SharedScript.Gangs[me.GetPlayerData().CurrentChar.job.name].label;
                //
                job.SetRightLabel(me.GetPlayerData().CurrentChar.Job.Name);
                gang.SetRightLabel(me.GetPlayerData().CurrentChar.Gang.Name);
                bank.SetRightLabel("~g~$" + me.GetPlayerData().Bank);
                datiPers.AddItem(name);
                datiPers.AddItem(dob);
                datiPers.AddItem(alt);
                datiPers.AddItem(nTel);
                datiPers.AddItem(nAss);
                datiPers.AddItem(job);
                datiPers.AddItem(gang);
                UIMenuItem moneyItem = new("Soldi: ", "I suoi soldi");
                UIMenu money = new("Soldi: ", "I suoi soldi");
                moneyItem.BindItemToMenu(money);
                moneyItem.SetRightLabel("~g~$" + me.GetPlayerData().Money);
                moneyItem.SetRightBadge(BadgeIcon.MP_CASH);
                datiPers.AddItem(bank);
                UIMenuItem dirtyItem = new("Soldi Sporchi: ", "I soldi sporchi");
                UIMenu dirty = new("Soldi Sporchi: ", "I soldi sporchi");
                dirtyItem.BindItemToMenu(dirty);
                dirtyItem.SetRightLabel("~r~$" + me.GetPlayerData().DirtyCash);
                dirtyItem.SetRightBadge(BadgeIcon.MP_CASH);
                UIMenuItem daiMoneyItem = new("Dai a qualcuno", "A chi?");
                UIMenu daiMoney = new("Dai a qualcuno", "A chi?");
                daiMoneyItem.BindItemToMenu(daiMoney);
                UIMenuItem daiDirtyItem = new("Dai a qualcuno", "A chi?");
                UIMenu daiDirty = new("Dai a qualcuno", "A chi?");
                daiDirtyItem.BindItemToMenu(daiDirty);
                daiMoneyItem.SetRightBadge(BadgeIcon.MP_CASH);
                daiDirtyItem.SetRightBadge(BadgeIcon.MP_CASH);
                UIMenuItem getta = new UIMenuItem("Butta via", "perché?");
                money.AddItem(getta);
                dirty.AddItem(getta);
                money.OnItemSelect += async (menu, item, index) =>
                {
                    if (item == getta)
                    {
                        int amount = -1;

                        do
                        {
                            await BaseScript.Delay(0);
                            string am = await HUD.GetUserInput("Quanto vuoi buttare?", "0", 10);

                            if (string.IsNullOrEmpty(am)) break;
                            amount = Convert.ToInt32(am);
                            if (amount < 1 || amount > me.GetPlayerData().Money) HUD.ShowNotification("Quantità non valida!", ColoreNotifica.Red, true);
                        } while (amount <= 1);

                        if (amount == -1) return;
                        playerPed.Task.PlayAnimation("weapons@first_person@aim_rng@generic@projectile@sticky_bomb@", "plant_floor");
                        BaseScript.TriggerServerEvent("lprp:removeAccountWithPickup", "soldi", amount);
                        await BaseScript.Delay(1000);
                        menu.GoBack();
                    }
                };
                dirty.OnItemSelect += async (menu, item, index) =>
                {
                    if (item == getta)
                    {
                        int amount = -1;

                        do
                        {
                            await BaseScript.Delay(0);
                            string am = await HUD.GetUserInput("Quanto vuoi buttare?", "0", 10);

                            if (string.IsNullOrEmpty(am)) break;
                            amount = Convert.ToInt32(am);
                            if (amount < 1 || amount > me.GetPlayerData().DirtyCash) HUD.ShowNotification("Quantità non valida!", ColoreNotifica.Red, true);
                        } while (amount < 1);

                        if (amount == -1) return;
                        playerPed.Task.PlayAnimation("weapons@first_person@aim_rng@generic@projectile@sticky_bomb@", "plant_floor");
                        BaseScript.TriggerServerEvent("lprp:removeAccountWithPickup", "soldi_sporchi", amount);
                        await BaseScript.Delay(1000);
                        menu.GoBack();
                    }
                };
            };
            UIMenuItem saluteItem = new UIMenuItem("Salute", "Fame, sete..");
            UIMenu salute = new("Salute", "Fame, sete..");
            saluteItem.BindItemToMenu(salute);
            persMenu.AddItem(saluteItem);
            fa.SetRightLabel("" + Math.Round(StatsNeeds.Needs["Fame"].GetPercent(), 2) + "%");
            se.SetRightLabel("" + Math.Round(StatsNeeds.Needs["Sete"].GetPercent(), 2) + "%");
            st.SetRightLabel("" + Math.Round(StatsNeeds.Needs["Stanchezza"].GetPercent(), 2) + "%");
            ma.SetRightLabel(me.GetPlayerData().CurrentChar.Needs.Malattia ? "Malato" : "In Salute");
            salute.AddItem(fa);
            salute.AddItem(se);
            salute.AddItem(st);
            salute.AddItem(ma);
            salute.OnMenuOpen += async (a, b) => Client.Instance.AddTick(AggiornaSalute);
            salute.OnMenuClose += async (a) => Client.Instance.RemoveTick(AggiornaSalute);

            UIMenuItem InventoryItem = new UIMenuItem("Inventario Personale", "Tasche");
            UIMenu Inventory = new("Inventario Personale", "Tasche");
            InventoryItem.BindItemToMenu(Inventory);
            persMenu.AddItem(InventoryItem);

            Inventory.OnMenuOpen += async (a, b) =>
            {
                Inventory.Clear();
                List<Inventory> inv = me.GetPlayerData().Inventory;

                if (inv.Count > 0)
                {
                    foreach (Inventory it in inv)
                        if (it.Amount > 0)
                        {
                            UIMenuItem newItemMenuItem = new UIMenuItem(ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].label, "[Quantità: " + it.Amount.ToString() + "] " + ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].description);
                            UIMenu newItemMenu = new(ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].label, "");
                            newItemMenuItem.BindItemToMenu(newItemMenu);
                            Inventory.AddItem(newItemMenuItem);

                            if (ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].use.use)
                            {
                                UIMenuItem useButton = new UIMenuItem(ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].use.label, ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].use.description, SColor.HUD_Greenlight, SColor.HUD_Green);
                                newItemMenu.AddItem(useButton);
                                newItemMenu.OnItemSelect += (_menu, _item, _index) =>
                                {
                                    /*
									 * BaseScript.TriggerServerEvent("lprp:useItem", ConfigShared.SharedConfig.Main.Generici.ItemList[it.item]);
									 */ //DA GESTIRE 
                                    if (_item == useButton) ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].UsaOggettoEvent(1);
                                };
                            }

                            if (ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].give.give)
                            {
                                List<dynamic> amountino = new List<dynamic>();
                                for (int j = 0; j < it.Amount; j++) amountino.Add((j + 1).ToString());
                                UIMenuItem giveButtonItem = new UIMenuItem(ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].give.label, ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].give.description);
                                UIMenu giveButton = new(ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].give.label, "");
                                giveButtonItem.BindItemToMenu(giveButton);
                                newItemMenu.AddItem(giveButtonItem);
                                giveButtonItem.HighlightColor = SColor.HUD_Freemode;
                                giveButtonItem.HighlightedTextColor = SColor.HUD_Freemode_dark;
                                List<int> playerId = new List<int>();
                                List<Player> players = Funzioni.GetPlayersInArea(PlayerCache.MyPlayer.Posizione.ToVector3, 3f);

                                if (players.Count > 0)
                                {
                                    foreach (Player player in players)
                                    {
                                        UIMenuListItem playerItem = new(Funzioni.GetPlayerCharFromServerId(player.ServerId).FullName, amountino, 0, "Scegli la quantità e procedi..");
                                        playerId.Add(player.ServerId);
                                        giveButton.AddItem(playerItem);
                                        playerPed.Task.PlayAnimation("mp_common", "givetake1_a");
                                    }

                                    giveButton.OnListSelect += async (_menu, _listItem, _index) =>
                                    {
                                        BaseScript.TriggerServerEvent("lprp:giveInventoryItemToPlayer", playerId[_index], it.Item, int.Parse(amountino[_listItem.Index]));
                                    };
                                }
                                else
                                {
                                    giveButton.AddItem(new UIMenuItem("Nessun player nelle vicinanze.", "Trova qualcuno!!"));
                                }
                            }

                            if (ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].drop.drop)
                            {
                                List<dynamic> amountino = new List<dynamic>();
                                for (int j = 0; j < it.Amount; j++) amountino.Add((j + 1).ToString());
                                UIMenuListItem dropButton = new UIMenuListItem(ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].drop.label, amountino, 0, ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].drop.description, SColor.HUD_Redlight, SColor.HUD_Red);
                                newItemMenu.AddItem(dropButton);
                                newItemMenu.OnListSelect += async (_menu, _listItem, Index) =>
                                {
                                    if (_listItem == dropButton)
                                    {
                                        playerPed.Task.PlayAnimation("weapons@first_person@aim_rng@generic@projectile@sticky_bomb@", "plant_floor");
                                        BaseScript.TriggerServerEvent("lprp:removeInventoryItemWithPickup", it.Item, int.Parse(amountino[_listItem.Index]));
                                        await BaseScript.Delay(1000);
                                        //										Inventory.RemoveItemAt(Inventory.MenuItems.IndexOf(newItemMenu.ParentItem));
                                        _menu.GoBack();
                                    }
                                };
                            }
                        }
                }
                else
                {
                    Inventory.AddItem(new UIMenuItem("Non hai niente nell'inventario.", "Datti da fare!!", SColor.HUD_Reddark, SColor.HUD_Red));
                }
            };

            UIMenuItem weapMenuItem = new UIMenuItem("Inventario Armi", "Le tue armi");
            UIMenu weapMenu = new("Inventario Armi", "Le tue armi");
            weapMenuItem.BindItemToMenu(weapMenu);
            persMenu.AddItem(weapMenuItem);
            weapMenu.OnMenuOpen += async (a, b) =>
            {
                weapMenu.Clear();

                if (me.GetPlayerData().GetCharWeapons().Count > 0)
                {
                    for (int i = 0; i < me.GetPlayerData().GetCharWeapons().Count; i++)
                    {
                        Weapons armi = me.GetPlayerData().GetCharWeapons()[i];
                        UIMenuItem armaItem = new UIMenuItem(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name)), "Munizioni: " + armi.ammo);
                        UIMenu arma = new(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name)), "");
                        armaItem.BindItemToMenu(arma);
                        weapMenu.AddItem(armaItem);
                        if (armi.components.Count > 0)
                        {
                            UIMenuItem componentiItem = new UIMenuItem("Componenti " + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name)), "Scegli qui quali abilitare tra quelli che hai!");
                            UIMenu componenti = new("Componenti " + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name)), "");
                            componentiItem.BindItemToMenu(componenti);
                            arma.AddItem(componentiItem);

                            for (int j = 0; j < armi.components.Count; j++)
                            {
                                Components comp = armi.components[j];
                                bool attivo = comp.active;

                                if (Funzioni.GetWeaponLabel(Funzioni.HashUint(comp.name)) != "Caricatore standard")
                                {
                                    UIMenuCheckboxItem componente = new UIMenuCheckboxItem(Funzioni.GetWeaponLabel(Funzioni.HashUint(comp.name)), comp.active, "Attiva/Disattiva questo componente!");
                                    componenti.AddItem(componente);
                                    componenti.OnCheckboxChange += (_menu, _item, _checked) =>
                                    {
                                        if (_item.Label == Funzioni.GetWeaponLabel(Funzioni.HashUint(comp.name)))
                                        {
                                            uint componentHash = Funzioni.HashUint(comp.name);
                                            List<Weapons> armiAgg = new List<Weapons>();
                                            List<Components> weaponComponents = new List<Components> { new Components(comp.name, comp.active) };
                                            armiAgg.Add(new Weapons(armi.name, armi.ammo, weaponComponents, armi.tint));
                                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Weapons = armiAgg;

                                            if (_checked)
                                            {
                                                GiveWeaponComponentToPed(PlayerPedId(), Funzioni.HashUint(armi.name), componentHash);
                                                HUD.ShowNotification("~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(comp.name)) + " Attivato/a.", true);
                                            }
                                            else if (!_checked)
                                            {
                                                RemoveWeaponComponentFromPed(PlayerPedId(), Funzioni.HashUint(armi.name), componentHash);
                                                HUD.ShowNotification("~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(comp.name)) + " Disattivato/a.");
                                            }
                                        }
                                    };
                                }
                            }
                        }

                        /* CODICE PER DARE UN ARMA E SI AGGIUNGERE ANCHE LE MUNIZIONI
							UIMenuItem giveButton = new UIMenuItem($"Dai {Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name))}", "", Colors.Cyan, Colors.DarkCyan);
							arma.AddItem(giveButton);
							*/
                        UIMenuItem dropButton = new UIMenuItem($"Gettare {Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name))}", "", SColor.HUD_Redlight, SColor.HUD_Red);
                        arma.AddItem(dropButton);
                        dropButton.Activated += async (_menu, item) =>
                        {
                            playerPed.Task.PlayAnimation("weapons@first_person@aim_rng@generic@projectile@sticky_bomb@", "plant_floor");
                            BaseScript.TriggerServerEvent("lprp:removeWeaponWithPickup", armi.name);
                            await BaseScript.Delay(1000);
                            //							weapMenu.RemoveItemAt(weapMenu.MenuItems.IndexOf(arma.ParentItem));
                            _menu.GoBack();
                        };
                    }
                }
                else
                {
                    UIMenuItem noItemsButton = new UIMenuItem("Non hai niente nell'inventario.", "Datti da fare!!", SColor.HUD_Reddark, SColor.HUD_Red);
                    weapMenu.AddItem(noItemsButton);
                }
            };

            #endregion

            #region Lavoro

            /*
			UIMenu WorkMenu = MenuHandler.AddSubMenu(PersonalMenu, "Menu Lavoro", "Menu dei Lavori", pos);
			for (int i = 0; i < 20; i++)
			{
				WorkMenu.AddItem(new UIMenuItem("Fregati!"));
			}
			*/

            #endregion

            #region animazioni e stile

            List<dynamic> umori = new List<dynamic>()
            {
                "Determinato",
                "Triste",
                "Depresso",
                "Annoiato",
                "Impaziente",
                "Timido",
                "Lunatico",
                "Stressato",
                "Pigro"
            };
            List<dynamic> attegg = new List<dynamic>()
            {
                "Fiero",
                "Cattivo",
                "Gangster",
                "Freddo",
                "Vuoto Dentro",
                "Borioso",
                "Perso",
                "Intimidatorio",
                "Ricco",
                "Aggressivo",
                "Imponente",
                "Esibizionista"
            };
            List<dynamic> donn = new List<dynamic>()
            {
                "Arrogante",
                "Di Classe",
                "Fragile",
                "Femme Fatale",
                "Assorbente senza Ali",
                "Triste",
                "Rebelle",
                "Sexy",
                "Con la ciccia ai glutei",
                "Fiera"
            };

            UIMenuItem ANeStilItem = new("Animazioni e Stile");
            UIMenu ANeStil = new("Animazioni e Stile", "");
            UIMenuItem umoreItem = new("Stile Camminata", "Le emozioni contano");
            UIMenu umore = new("Stile Camminata", "Le emozioni contano");
            ANeStilItem.BindItemToMenu(ANeStil);
            umoreItem.BindItemToMenu(umore);
            PersonalMenu.AddItem(ANeStilItem);
            ANeStil.AddItem(umoreItem);

            UIMenuListItem Item1 = new UIMenuListItem("Umori", umori, 0, "Come si sente il tuo personaggio oggi?");
            UIMenuListItem Item2 = new UIMenuListItem("Stile", attegg, 0, "Che atteggiamento ha il tuo personaggio?");
            UIMenuListItem Item3 = new UIMenuListItem("Femminili", donn, 0, "Perche a noi importa del gentil sesso!");
            UIMenuItem Item4 = new UIMenuItem("~r~Reset", "Perche atteggiarti quando puoi camminare normalmente?", SColor.HUD_Reddark, SColor.HUD_Red);
            umore.AddItem(Item1);
            umore.AddItem(Item2);
            umore.AddItem(Item3);
            umore.AddItem(Item4);
            umore.OnListSelect += (_menu, _listItem, _itemIndex) =>
            {
                if (_listItem == Item1 && !Death.ferito)
                {
                    string ActiveItem = _listItem.Items[_listItem.Index].ToString();

                    switch (ActiveItem)
                    {
                        case "Determinato":
                            playerPed.MovementAnimationSet = "move_m@brave";

                            break;
                        case "Triste":
                            playerPed.MovementAnimationSet = "move_m@sad@a";

                            break;
                        case "Depresso":
                            playerPed.MovementAnimationSet = "move_m@depressed@a";

                            break;
                        case "Annoiato":
                            playerPed.MovementAnimationSet = "move_characters@michael@fire";

                            break;
                        case "Impaziente":
                            playerPed.MovementAnimationSet = "move_m@quick";

                            break;
                        case "Timido":
                            playerPed.MovementAnimationSet = "move_m@confident";

                            break;
                        case "Lunatico":
                        case "Stressato":
                            playerPed.MovementAnimationSet = "move_m@hurry@a";

                            break;
                        case "Pigro":
                            playerPed.MovementAnimationSet = "move_characters@jimmy@slow@";

                            break;
                    }
                }
                else if (_listItem == Item2 && !Death.ferito)
                {
                    string atteg = _listItem.Items[_listItem.Index].ToString();

                    switch (atteg)
                    {
                        case "Fiero":
                            playerPed.MovementAnimationSet = "move_m@business@a";

                            break;
                        case "Cattivo":
                            playerPed.MovementAnimationSet = "move_m@casual@d";

                            break;
                        case "Gangster":
                            playerPed.MovementAnimationSet = "move_m@casual@e";

                            break;
                        case "Freddo":
                            playerPed.MovementAnimationSet = "move_m@buzzed";

                            break;
                        case "Vuoto Dentro":
                            playerPed.MovementAnimationSet = "move_m@depressed@b";

                            break;
                        case "Borioso":
                            playerPed.MovementAnimationSet = "move_m@sassy";

                            break;
                        case "Perso":
                            playerPed.MovementAnimationSet = "move_m@hobo@b";

                            break;
                        case "Intimidatorio":
                            playerPed.MovementAnimationSet = "move_m@intimidation@1h";

                            break;
                        case "Ricco":
                            playerPed.MovementAnimationSet = "move_m@money";

                            break;
                        case "Aggressivo":
                            playerPed.MovementAnimationSet = "move_m@fire";

                            break;
                        case "Imponente":
                            playerPed.MovementAnimationSet = "move_m@gangster@generic";

                            break;
                        case "Esibizionista":
                            playerPed.MovementAnimationSet = "move_m@swagger";

                            break;
                    }
                }
                else if (_listItem == Item3 && !Death.ferito)
                {
                    string donna = _listItem.Items[_listItem.Index].ToString();

                    switch (donna)
                    {
                        case "Arrogante":
                            playerPed.MovementAnimationSet = "move_f@arrogant@a";

                            break;
                        case "Di Classe":
                            playerPed.MovementAnimationSet = "move_characters@amanda@bag";

                            break;
                        case "Fragile":
                            playerPed.MovementAnimationSet = "move_f@femme@";

                            break;
                        case "Femme Fatale":
                            playerPed.MovementAnimationSet = "move_f@gangster@ng";

                            break;
                        case "Assorbente senza Ali":
                            playerPed.MovementAnimationSet = "move_f@heels@c";

                            break;
                        case "Triste":
                            playerPed.MovementAnimationSet = "move_f@sad@a";

                            break;
                        case "Ribelle":
                            playerPed.MovementAnimationSet = "move_f@sassy";

                            break;
                        case "Sexy":
                            playerPed.MovementAnimationSet = "move_f@sexy@a";

                            break;
                        case "Con la ciccia ai glutei":
                            playerPed.MovementAnimationSet = "move_f@tough_guy@";

                            break;
                        case "Fiera":
                            playerPed.MovementAnimationSet = "move_f@tool_belt@a";

                            break;
                    }
                }
                else if (_listItem == Item4 && !Death.ferito)
                {
                    playerPed.MovementAnimationSet = null;
                }
                else
                {
                    HUD.ShowNotification("Sei ferito! Non puoi fare quest'azione al momento!", ColoreNotifica.Red, true);
                }
            };
            UIMenuItem animMenuItem = new UIMenuItem("Menu Animazioni", "~g~Quando il RolePlay diventa anche divertente");
            UIMenuItem animMenu1Item = new UIMenuItem("Festa", "~g~Per divertirci");
            UIMenu animMenu = new("Menu Animazioni", "~g~Quando il RolePlay diventa anche divertente");
            UIMenu animMenu1 = new("Festa", "~g~Per divertirci");
            animMenuItem.BindItemToMenu(animMenu);
            animMenu1Item.BindItemToMenu(animMenu1);
            ANeStil.AddItem(animMenuItem);
            animMenu.AddItem(animMenu1Item);
            UIMenuItem item1 = new UIMenuItem("Suonare", "Che bella musica!");
            UIMenuItem item2 = new UIMenuItem("Dj", "Che bella musica!");
            UIMenuItem item3 = new UIMenuItem("Bere una bibita", "Ancora meglio se in compagnia!");
            UIMenuItem item4 = new UIMenuItem("Bere una birra", "Ancora meglio con una bella partita!");
            UIMenuItem item5 = new UIMenuItem("Air Guitar", "Per chi, una vera, non può permettersela!");
            UIMenuItem item6 = new UIMenuItem("Air Shagging", "Questo è divertirsi!");
            UIMenuItem item7 = new UIMenuItem("Rock 'n Roll", "L'anima del Metal vive in te!");
            UIMenuItem item8 = new UIMenuItem("Fumare una canna", "Per calmare i nervi!");
            UIMenuItem item9 = new UIMenuItem("Ubriaco", "Vacci piano!");
            animMenu1.OnItemSelect += (_menu, _item, _index) =>
            {
                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && playerPed.IsAlive)
                {
                    if (_item == item1)
                        playerPed.Task.StartScenario("WORLD_HUMAN_MUSICIAN", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item2)
                        playerPed.Task.PlayAnimation("anim@mp_player_intcelebrationmale@dj", "dj");
                    else if (_item == item3)
                        playerPed.Task.StartScenario("WORLD_HUMAN_DRINKING", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item4)
                        playerPed.Task.StartScenario("WORLD_HUMAN_PARTYING", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item5)
                        playerPed.Task.PlayAnimation("anim@mp_player_intcelebrationmale@air_guitar", "air_guitar");
                    else if (_item == item6)
                        playerPed.Task.PlayAnimation("anim@mp_player_intcelebrationfemale@air_shagging", "air_shagging");
                    else if (_item == item7)
                        playerPed.Task.PlayAnimation("mp_player_int_upperrock", "mp_player_int_rock");
                    else if (_item == item8)
                        playerPed.Task.StartScenario("WORLD_HUMAN_SMOKING_POT", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item9) playerPed.Task.PlayAnimation("amb@world_human_bum_standing@drunk@idle_a", "idle_a");
                }
                else
                {
                    HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", ColoreNotifica.Red);
                }
            };
            animMenu1.AddItem(item1);
            animMenu1.AddItem(item2);
            animMenu1.AddItem(item3);
            animMenu1.AddItem(item4);
            animMenu1.AddItem(item5);
            animMenu1.AddItem(item6);
            animMenu1.AddItem(item7);
            animMenu1.AddItem(item8);
            animMenu1.AddItem(item9);
            UIMenuItem animMenu2Item = new UIMenuItem("Saluti", "~g~Fagli sapere che li stai chiamando");
            UIMenu animMenu2 = new("Saluti", "~g~Fagli sapere che li stai chiamando");
            animMenu2Item.BindItemToMenu(animMenu2);
            animMenu.AddItem(animMenu2Item);
            UIMenuItem item10 = new UIMenuItem("Salutare", "Fai ciao con la manina!");
            UIMenuItem item11 = new UIMenuItem("Saluto tra Amici", "Lo ami quasi come un cugino!");
            UIMenuItem item12 = new UIMenuItem("Saluto in Gang", "Lo ami come se fossi te stesso!");
            UIMenuItem item13 = new UIMenuItem("Saluto Militare", "AAAAAAAAAATTENTI!");
            animMenu2.AddItem(item10);
            animMenu2.AddItem(item11);
            animMenu2.AddItem(item12);
            animMenu2.AddItem(item13);
            animMenu2.OnItemSelect += (_menu, _item, _index) =>
            {
                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && playerPed.IsAlive)
                {
                    if (_item == item10)
                        playerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_hello");
                    else if (_item == item11)
                        playerPed.Task.PlayAnimation("mp_ped_interaction", "handshake_guy_a");
                    else if (_item == item12)
                        playerPed.Task.PlayAnimation("mp_ped_interaction", "hugs_guy_a");
                    else if (_item == item13) playerPed.Task.PlayAnimation("mp_player_int_uppersalute", "mp_player_int_salute");
                }
                else
                {
                    HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", ColoreNotifica.Red);
                }
            };
            UIMenuItem animMenu3Item = new UIMenuItem("Lavori", "~g~Il RolePlay non è solo un gioco!");
            UIMenu animMenu3 = new("Lavori", "~g~Il RolePlay non è solo un gioco!");
            animMenu3Item.BindItemToMenu(animMenu3);
            animMenu.AddItem(animMenu3Item);
            UIMenuItem item14 = new UIMenuItem("Arrendersi alla polizia", "Prima che sparino!");
            UIMenuItem item15 = new UIMenuItem("Pescatore", "Per rilassarsi al porto!");
            UIMenuItem item16 = new UIMenuItem("Agricoltore - Vendemmia", "Per chi ama la natura!");
            UIMenuItem item17 = new UIMenuItem("Meccanico - Ripara il veicolo", "Porta con te lo straccetto per le mani!");
            UIMenuItem item18 = new UIMenuItem("Meccanico - Ripara il motore", "Che mal di schiena!");
            UIMenuItem item19 = new UIMenuItem("Polizia - Gestire il Traffico", "Usa il Fischietto!");
            UIMenuItem item20 = new UIMenuItem("Polizia - Indagare", "Fammi vedere meglio...");
            UIMenuItem item21 = new UIMenuItem("Binocolo", "Per vederci chiaro!");
            UIMenuItem item22 = new UIMenuItem("Medico - Controlla il paziente", "Potrà ancora suonare il piano?!");
            UIMenuItem item23 = new UIMenuItem("Taxi - Parlare al cliente", "Conversare aiuta a rilassarsi!");
            UIMenuItem item24 = new UIMenuItem("Taxi - Dare il conto", "Ecco a lei il resto!");
            UIMenuItem item25 = new UIMenuItem("Drogheria - dare la roba", "Ecco a lei Signore..");
            UIMenuItem item26 = new UIMenuItem("Fotografo", "Immortala i momenti migliori!");
            UIMenuItem item27 = new UIMenuItem("Annotare", "Spero non sia la lista della spesa!");
            UIMenuItem item28 = new UIMenuItem("Martellare", "Che male al dito!");
            UIMenuItem item29 = new UIMenuItem("Chiedere l'elemosina", "Siamo tutti poveri..");
            UIMenuItem item30 = new UIMenuItem("Fare la Statua", "Fermo... fermo...");
            //UIMenuItem item31 = new UIMenuItem();
            animMenu3.AddItem(item14);
            animMenu3.AddItem(item15);
            animMenu3.AddItem(item16);
            animMenu3.AddItem(item17);
            animMenu3.AddItem(item18);
            animMenu3.AddItem(item19);
            animMenu3.AddItem(item20);
            animMenu3.AddItem(item21);
            animMenu3.AddItem(item22);
            animMenu3.AddItem(item23);
            animMenu3.AddItem(item24);
            animMenu3.AddItem(item25);
            animMenu3.AddItem(item26);
            animMenu3.AddItem(item27);
            animMenu3.AddItem(item28);
            animMenu3.AddItem(item29);
            animMenu3.AddItem(item30);
            animMenu3.OnItemSelect += (_menu, _item, _index) =>
            {
                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && playerPed.IsAlive)
                {
                    if (_item == item14)
                        playerPed.Task.PlayAnimation("random@arrests@busted", "idle_c");
                    else if (_item == item15)
                        playerPed.Task.StartScenario("world_human_stand_fishing", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item16)
                        playerPed.Task.StartScenario("world_human_gardener_plant", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item17)
                        playerPed.Task.StartScenario("world_human_vehicle_mechanic", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item18)
                        playerPed.Task.PlayAnimation("mini@repair", "fixing_a_ped");
                    else if (_item == item19)
                        playerPed.Task.StartScenario("WORLD_HUMAN_CAR_PARK_ATTENDANT", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item20)
                        playerPed.Task.PlayAnimation("amb@code_human_police_investigate@idle_b", "idle_f");
                    else if (_item == item21)
                        playerPed.Task.StartScenario("WORLD_HUMAN_BINOCULARS", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item22)
                        playerPed.Task.StartScenario("CODE_HUMAN_MEDIC_KNEEL", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item23)
                        playerPed.Task.PlayAnimation("oddjobs@taxi@driver", "leanover_idle");
                    else if (_item == item24)
                        playerPed.Task.PlayAnimation("oddjobs@taxi@cyi", "std_hand_off_ps_passenger");
                    else if (_item == item25)
                        playerPed.Task.PlayAnimation("mp_am_hold_up", "purchase_beerbox_shopkeeper");
                    else if (_item == item26)
                        playerPed.Task.StartScenario("WORLD_HUMAN_PAPARAZZI", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item27)
                        playerPed.Task.StartScenario("WORLD_HUMAN_CLIPBOARD", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item28)
                        playerPed.Task.StartScenario("WORLD_HUMAN_HAMMERING", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item29)
                        playerPed.Task.StartScenario("WORLD_HUMAN_BUM_FREEWAY", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item30) playerPed.Task.StartScenario("WORLD_HUMAN_HUMAN_STATUE", PlayerCache.MyPlayer.Posizione.ToVector3);
                }
                else
                {
                    HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", ColoreNotifica.Red);
                }
            };
            UIMenuItem animMenu4Item = new UIMenuItem("Umore", "~g~Cosa vuoi dirgli col corpo?");
            UIMenu animMenu4 = new("Umore", "~g~Cosa vuoi dirgli col corpo?");
            animMenu4Item.BindItemToMenu(animMenu3);
            animMenu.AddItem(animMenu4Item);
            UIMenuItem item31 = new UIMenuItem("Congratularsi", "Meglio di Cosi!");
            UIMenuItem item32 = new UIMenuItem("Super", "Pollice in SU!");
            UIMenuItem item33 = new UIMenuItem("Indicare", "TU!");
            UIMenuItem item34 = new UIMenuItem("Che vuoi?", "");
            UIMenuItem item35 = new UIMenuItem("Lo sapevo, cazzo!", "");
            UIMenuItem item36 = new UIMenuItem("Facepalm", "");
            UIMenuItem item37 = new UIMenuItem("Calmati", "Meglio ragionarci");
            UIMenuItem item38 = new UIMenuItem("Spavento", "");
            UIMenuItem item39 = new UIMenuItem("Sottomesso", "E porta rispetto!");
            UIMenuItem item40 = new UIMenuItem("Preparati", "Un po' di riscaldamento");
            UIMenuItem item41 = new UIMenuItem("Non è possibile!", "Non ci credo!");
            UIMenuItem item42 = new UIMenuItem("Abbracciare", "Un po' di affetto");
            UIMenuItem item43 = new UIMenuItem("Dito Medio", "Il meglio per mandare a quel paese con dolcezza e decisione!");
            UIMenuItem item44 = new UIMenuItem("Segaiolo", "Per far capire che hai molto tempo libero!");
            UIMenuItem item45 = new UIMenuItem("Suicidio", "Spaventa i tuoi amici e familiari!");
            //UIMenuItem item46 = new UIMenuItem();
            animMenu4.AddItem(item31);
            animMenu4.AddItem(item32);
            animMenu4.AddItem(item33);
            animMenu4.AddItem(item34);
            animMenu4.AddItem(item35);
            animMenu4.AddItem(item36);
            animMenu4.AddItem(item37);
            animMenu4.AddItem(item38);
            animMenu4.AddItem(item39);
            animMenu4.AddItem(item40);
            animMenu4.AddItem(item41);
            animMenu4.AddItem(item42);
            animMenu4.AddItem(item43);
            animMenu4.AddItem(item44);
            animMenu4.AddItem(item45);
            animMenu4.OnItemSelect += (_menu, _item, _index) =>
            {
                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && playerPed.IsAlive)
                {
                    if (_item == item31)
                        playerPed.Task.StartScenario("WORLD_HUMAN_CHEERING", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item32)
                        playerPed.Task.PlayAnimation("mp_action", "thanks_male_06");
                    else if (_item == item33)
                        playerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_point");
                    else if (_item == item34)
                        playerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_bring_it_on");
                    else if (_item == item35)
                        playerPed.Task.PlayAnimation("anim@am_hold_up@male", "shoplift_high");
                    else if (_item == item36)
                        playerPed.Task.PlayAnimation("anim@mp_player_intcelebrationmale@face_palm", "face_palm");
                    else if (_item == item37)
                        playerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_easy_now");
                    else if (_item == item38)
                        playerPed.Task.PlayAnimation("oddjobs@assassinate@multi@", "react_big_variations_a");
                    else if (_item == item39)
                        playerPed.Task.PlayAnimation("amb@code_human_cower_stand@male@react_cowering", "base_right");
                    else if (_item == item40)
                        playerPed.Task.PlayAnimation("anim@deathmatch_intros@unarmed", "intro_male_unarmed_e");
                    else if (_item == item41)
                        playerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_damn");
                    else if (_item == item42)
                        playerPed.Task.PlayAnimation("mp_ped_interaction", "kisses_guy_a");
                    else if (_item == item43)
                        playerPed.Task.PlayAnimation("mp_player_int_upperfinger", "mp_player_int_finger_01_enter");
                    else if (_item == item44)
                        playerPed.Task.PlayAnimation("mp_player_int_upperwank", "mp_player_int_wank_01");
                    else if (_item == item45) playerPed.Task.PlayAnimation("mp_suicide", "pistol");
                }
                else
                {
                    HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", ColoreNotifica.Red);
                }
            };
            UIMenuItem animMenu5Item = new UIMenuItem("Sports", "~g~Tenersi in forma è importante..");
            UIMenu animMenu5 = new("Sports", "~g~Tenersi in forma è importante..");
            animMenu5Item.BindItemToMenu(animMenu3);
            animMenu.AddItem(animMenu5Item);
            UIMenuItem item46 = new UIMenuItem("Mostra i tuoi muscoli", "Le donne ti vorranno e gli uomini vorranno essere te!");
            UIMenuItem item47 = new UIMenuItem("Fare i pesi", "Per bicipiti da urlo!");
            UIMenuItem item48 = new UIMenuItem("Fare le flessioni", "oh yeah!");
            UIMenuItem item49 = new UIMenuItem("Fare gli addominali", "Scolpito nella roccia");
            UIMenuItem item50 = new UIMenuItem("Yoga", "Per chi si depila l'inguine");
            animMenu5.AddItem(item46);
            animMenu5.AddItem(item47);
            animMenu5.AddItem(item48);
            animMenu5.AddItem(item49);
            animMenu5.AddItem(item50);
            animMenu5.OnItemSelect += (_menu, _item, _index) =>
            {
                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && playerPed.IsAlive)
                {
                    if (_item == item46)
                        playerPed.Task.PlayAnimation("amb@world_human_muscle_flex@arms_at_side@base", "base");
                    else if (_item == item47)
                        playerPed.Task.PlayAnimation("amb@world_human_muscle_free_weights@male@barbell@base", "base");
                    else if (_item == item48)
                        playerPed.Task.PlayAnimation("amb@world_human_push_ups@male@base", "base");
                    else if (_item == item49)
                        playerPed.Task.PlayAnimation("amb@world_human_sit_ups@male@base", "base");
                    else if (_item == item50) playerPed.Task.PlayAnimation("amb@world_human_yoga@male@base", "base_a");
                }
                else
                {
                    HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", ColoreNotifica.Red);
                }
            };
            UIMenuItem animMenu6Item = new UIMenuItem("Varie", "~g~Per la vita quotidiana");
            UIMenu animMenu6 = new("Varie", "~g~Per la vita quotidiana");
            animMenu6Item.BindItemToMenu(animMenu3);
            animMenu.AddItem(animMenu6Item);
            UIMenuItem item51 = new UIMenuItem("Bere un caffè", "Non Dormire!");
            UIMenuItem item52 = new UIMenuItem("Sedersi", "Il tuo sedere ringrazia!");
            UIMenuItem item53 = new UIMenuItem("Sedersi per terra", "Ci sono le panchine eh!");
            UIMenuItem item54 = new UIMenuItem("Appoggiati al muro", "Come un tamarro!");
            UIMenuItem item55 = new UIMenuItem("Prendere il sole", "Hai preso la crema?");
            UIMenuItem item56 = new UIMenuItem("Sdraiato a pancia in giu", "Occhio a chi arriva da dietro..");
            UIMenuItem item57 = new UIMenuItem("Pulisci", "Sguattero!");
            UIMenuItem item58 = new UIMenuItem("Fai un selfie", "Il popolo di Instagram ringrazia");
            animMenu6.AddItem(item51);
            animMenu6.AddItem(item52);
            animMenu6.AddItem(item53);
            animMenu6.AddItem(item54);
            animMenu6.AddItem(item55);
            animMenu6.AddItem(item56);
            animMenu6.AddItem(item57);
            animMenu6.AddItem(item58);
            animMenu6.OnItemSelect += (_menu, _item, _index) =>
            {
                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && playerPed.IsAlive)
                {
                    if (_item == item51)
                        playerPed.Task.PlayAnimation("amb@world_human_aa_coffee@idle_a", "idle_a");
                    else if (_item == item52)
                        playerPed.Task.PlayAnimation("anim@heists@prison_heistunfinished_biztarget_idle", "target_idle");
                    else if (_item == item53)
                        playerPed.Task.StartScenario("WORLD_HUMAN_PICNIC", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item54)
                        playerPed.Task.StartScenario("world_human_leaning", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item55)
                        playerPed.Task.StartScenario("WORLD_HUMAN_SUNBATHE_BACK", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item56)
                        playerPed.Task.StartScenario("WORLD_HUMAN_SUNBATHE", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item57)
                        playerPed.Task.StartScenario("world_human_maid_clean", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item58) playerPed.Task.StartScenario("world_human_tourist_mobile", PlayerCache.MyPlayer.Posizione.ToVector3);
                }
                else
                {
                    HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", ColoreNotifica.Red);
                }
            };
            UIMenuItem animMenu7Item = new UIMenuItem("Hard", "~g~La vita può prendere delle pieghe inaspettate nel RolePlay");
            UIMenu animMenu7 = new("Hard", "~g~La vita può prendere delle pieghe inaspettate nel RolePlay");
            animMenu7Item.BindItemToMenu(animMenu3);
            animMenu.AddItem(animMenu7Item);
            UIMenuItem item59 = new UIMenuItem("L'uomo si fa fare un b******* in auto", "Meglio di Cosi!");
            UIMenuItem item60 = new UIMenuItem("La donna fa un b******* in auto", "Pollice in SU! ..e non solo quello ;)");
            UIMenuItem item61 = new UIMenuItem("Uomo sesso in auto", "Grande!");
            UIMenuItem item62 = new UIMenuItem("Donna sesso in auto", "Complimentoni!");
            UIMenuItem item63 = new UIMenuItem("Panterona?", "");
            UIMenuItem item64 = new UIMenuItem("Fare l'affascinante!", "");
            UIMenuItem item65 = new UIMenuItem("Quanto sono bella/o??", "");
            UIMenuItem item66 = new UIMenuItem("Mostra il seno", "");
            UIMenuItem item67 = new UIMenuItem("Strip Tease 1", "");
            UIMenuItem item68 = new UIMenuItem("Strip Tease 2", "");
            UIMenuItem item69 = new UIMenuItem("Strip Tease 3(A terra)", "");
            animMenu7.AddItem(item59);
            animMenu7.AddItem(item60);
            animMenu7.AddItem(item61);
            animMenu7.AddItem(item62);
            animMenu7.AddItem(item63);
            animMenu7.AddItem(item64);
            animMenu7.AddItem(item65);
            animMenu7.AddItem(item66);
            animMenu7.AddItem(item67);
            animMenu7.AddItem(item68);
            animMenu7.AddItem(item69);
            animMenu7.OnItemSelect += (_menu, _item, _index) =>
            {
                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && playerPed.IsAlive)
                {
                    if (_item == item59)
                        playerPed.Task.PlayAnimation("oddjobs@towing", "m_blow_job_loop");
                    else if (_item == item60)
                        playerPed.Task.PlayAnimation("oddjobs@towing", "f_blow_job_loop");
                    else if (_item == item61)
                        playerPed.Task.PlayAnimation("mini@prostitutes@sexlow_veh", "low_car_sex_loop_player");
                    else if (_item == item62)
                        playerPed.Task.PlayAnimation("mini@prostitutes@sexlow_veh", "low_car_sex_loop_female");
                    else if (_item == item63)
                        playerPed.Task.PlayAnimation("mp_player_int_uppergrab_crotch", "mp_player_int_grab_crotch");
                    else if (_item == item64)
                        playerPed.Task.PlayAnimation("mini@strip_club@idles@stripper", "stripper_idle_02");
                    else if (_item == item65)
                        playerPed.Task.StartScenario("WORLD_HUMAN_PROSTITUTE_HIGH_CLASS", PlayerCache.MyPlayer.Posizione.ToVector3);
                    else if (_item == item66)
                        playerPed.Task.PlayAnimation("mini@strip_club@backroom@", "stripper_b_backroom_idle_b");
                    else if (_item == item67)
                        playerPed.Task.PlayAnimation("mini@strip_club@lap_dance@ld_girl_a_song_a_p1", "ld_girl_a_song_a_p1_f");
                    else if (_item == item68)
                        playerPed.Task.PlayAnimation("mini@strip_club@private_dance@part2", "priv_dance_p2");
                    else if (_item == item69) playerPed.Task.PlayAnimation("mini@strip_club@private_dance@part3", "priv_dance_p3");
                }
                else
                {
                    HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", ColoreNotifica.Red);
                }
            };
            UIMenuItem animMenu8Item = new UIMenuItem("Nuove", "~g~Perche ci siamo evoluti~w~");
            UIMenu animMenu8 = new("Nuove", "~g~Perche ci siamo evoluti~w~");
            animMenu8Item.BindItemToMenu(animMenu3);
            animMenu.AddItem(animMenu8Item);
            UIMenuItem item70 = new UIMenuItem("Facepalm", "La mamma degli idioti è sempre incinta");
            UIMenuItem item71 = new UIMenuItem("Incrocia le braccia", "Serio!");
            UIMenuItem item72 = new UIMenuItem("Dannazione", "");
            UIMenuItem item73 = new UIMenuItem("Fallimento", "");
            UIMenuItem item74 = new UIMenuItem("Incrocia le braccia2", "Serio!");
            UIMenuItem item75 = new UIMenuItem("batti le mani sarcastico", "");
            UIMenuItem item76 = new UIMenuItem("Tieni la folla", "calmi calmi");
            UIMenuItem item77 = new UIMenuItem("Tieni la folla2", "Ho detto calmi");
            animMenu7.OnItemSelect += (_menu, _item, _index) =>
            {
                if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && playerPed.IsAlive)
                {
                    if (_item == item70)
                        playerPed.Task.PlayAnimation("anim@mp_player_intupperface_palm", "idle_a");
                    else if (_item == item71)
                        playerPed.Task.PlayAnimation("oddjobs@assassinate@construction@", "unarmed_fold_arms");
                    else if (_item == item72)
                        playerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_damn");
                    else if (_item == item73)
                        playerPed.Task.PlayAnimation("random@car_thief@agitated@idle_a", "agitated_idle_a");
                    else if (_item == item74)
                        playerPed.Task.PlayAnimation("oddjobs@assassinate@construction@", "idle_a");
                    else if (_item == item75)
                        playerPed.Task.PlayAnimation("anim@mp_player_intcelebrationmale@slow_clap", "slow_clap");
                    else if (_item == item76)
                        playerPed.Task.PlayAnimation("amb@code_human_police_crowd_control@idle_a", "idle_a");
                    else if (_item == item77) playerPed.Task.PlayAnimation("amb@code_human_police_crowd_control@idle_b", "idle_d");
                }
                else
                {
                    HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", ColoreNotifica.Red);
                }
            };
            animMenu8.AddItem(item70);
            animMenu8.AddItem(item71);
            animMenu8.AddItem(item72);
            animMenu8.AddItem(item73);
            animMenu8.AddItem(item74);
            animMenu8.AddItem(item75);
            animMenu8.AddItem(item76);
            animMenu8.AddItem(item77);

            #endregion

            #region Gangs

            UIMenuItem bandeCriminaliItem = new("Bande criminali", "Fonda e gestisti la tua banda criminale!");
            UIMenu bandeCriminali = new("Bande criminali", "Fonda e gestisti la tua banda criminale!");
            bandeCriminaliItem.BindItemToMenu(bandeCriminali);
            PersonalMenu.AddItem(bandeCriminaliItem);

            if (me.GetPlayerData().CurrentChar.Gang.Name == "Incensurato")
            {
                UIMenuItem diventaBoss = new UIMenuItem("Diventa Boss di una Banda!", "Baciamo le mani.");
                List<dynamic> lavoro = new List<dynamic>() { "No", "Si" };
                UIMenuListItem incerca = new UIMenuListItem("In cerca di \"Lavoro\"", lavoro, 0, GetLabelText("PIM_MAGH0D"));
                bandeCriminali.AddItem(diventaBoss);
                bandeCriminali.AddItem(incerca);
                // GB_BECOMEB = You are now the CEO of ~a~~s~
                // GB_GOON_OPEN = Hold ~INPUT_VEH_EXIT~to open the door for your Boss
                diventaBoss.Activated += async (menu, item) =>
                {
                    if (me.GetPlayerData().Bank > 5000)
                    {
                        if (Main.GangsAttive.Count < 3)
                        {
                            string gname = await HUD.GetUserInput("Nome della Banda", "", 15);
                            MenuHandler.CloseAndClearHistory();
                            ScaleformUI.Main.BigMessageInstance.ShowSimpleShard("Boss", $"Sei diventato il Boss della banda ~o~{gname}~w~.");
                            Game.PlaySound("Boss_Message_Orange", "GTAO_Boss_Goons_FM_Soundset");
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Gang = new Gang(gname, 5);
                            Main.GangsAttive.Add(new Gang(gname, Main.GangsAttive.Count + 1));
                        }
                        else
                        {
                            HUD.ShowNotification("Ci sono già troppe Bande Criminali attive in sessione.~n~Riprova in un altro momento.", ColoreNotifica.Red, true);
                        }
                    }
                    else
                    {
                        HUD.ShowNotification("Non possiedi abbastanza fondi bancari per diventare un Boss!", ColoreNotifica.Red, true);
                    }
                };
            }
            else
            {
                if (me.GetPlayerData().CurrentChar.Gang.Grade > 4)
                {
                    UIMenuItem assumiItem = new UIMenuItem("Assumi membri");
                    UIMenu assumi = new("Assumi membri", "");
                    UIMenuItem gestioneItem = new UIMenuItem("Gestione banda");
                    UIMenu gestione = new("Gestione banda", "");
                    UIMenuItem abilitàBossItem = new UIMenuItem("Abilità Boss");
                    UIMenu abilitàBoss = new("Abilità Boss", "");
                    assumiItem.BindItemToMenu(assumi);
                    gestioneItem.BindItemToMenu(gestione);
                    abilitàBossItem.BindItemToMenu(abilitàBoss);
                    bandeCriminali.AddItem(assumiItem);
                    bandeCriminali.AddItem(gestioneItem);
                    bandeCriminali.AddItem(abilitàBossItem);


                    UIMenuItem ritirati = new UIMenuItem("Ritirati", "Attenzione.. non potrai fondare una banda prima di altre 6 ore!");
                    bandeCriminali.AddItem(ritirati);
                    ritirati.Activated += (menu, item) =>
                    {
                        MenuHandler.CloseAndClearHistory();
                        Main.GangsAttive.Remove(me.GetPlayerData().CurrentChar.Gang);
                        ScaleformUI.Main.BigMessageInstance.ShowSimpleShard("Ritirato", $"Non sei più il boss della banda ~o~{me.GetPlayerData().CurrentChar.Gang.Name}~w~.");
                        Game.PlaySound("Boss_Message_Orange", "GTAO_Boss_Goons_FM_Soundset");
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Gang = new Gang("Incensurato", 0);
                    };
                }
                else
                {
                    UIMenuItem ritirati = new UIMenuItem("Ritirati", "Smetti di lavorare per il tuo boss attuale");
                    bandeCriminali.AddItem(ritirati);
                    ritirati.Activated += (menu, item) =>
                    {
                        MenuHandler.CloseAndClearHistory();
                        ScaleformUI.Main.BigMessageInstance.ShowSimpleShard("Ritirato", $"Non fai più parte della banda ~o~{me.GetPlayerData().CurrentChar.Gang.Name}~w~.");
                        Game.PlaySound("Boss_Message_Orange", "GTAO_Boss_Goons_FM_Soundset");
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Gang = new Gang("Incensurato", 0);
                    };
                }
            }

            #endregion

            #region Suicidio

            UIMenuListItem suicidio = new UIMenuListItem("Suicidati", new List<dynamic>() { "Medicine", "Pistola" }, 0, "~r~ATTENZIONE~w~, il suicidio assistito è un azzardo RP molto pericoloso.\nIl tempo di respawn sara' molto meno del solito e perderai tutto!");
            PersonalMenu.AddItem(suicidio);
            suicidio.OnListSelected += async (item, index) =>
            {
                RequestAnimDict("mp_suicide");
                while (!HasAnimDictLoaded("mp_suicide")) await BaseScript.Delay(0);
                string var = item.Items[index] as string;
                string Anim = "";

                //controllare se ha una pistola o le pillole con se
                switch (var)
                {
                    case "Medicine":
                        Anim = me.GetPlayerData().CurrentChar.Skin.sex == "Maschio" ? "pill" : "pill_fp";

                        break;
                    case "Pistola":
                        Anim = me.GetPlayerData().CurrentChar.Skin.sex == "Maschio" ? "PISTOL" : "PISTOL_FP";

                        break;
                }

                if (Anim == "PISTOL" || Anim == "PISTOL_FP")
                {
                    if (playerPed.Weapons.HasWeapon(WeaponHash.Pistol))
                    {
                        playerPed.Weapons.Select(WeaponHash.Pistol);
                    }
                    else
                    {
                        HUD.ShowNotification("Non puoi suicidarti con la pistola senza avere una pistola!");

                        return;
                    }
                }

                MenuHandler.CloseAndClearHistory();
                TaskPlayAnim(PlayerPedId(), "MP_SUICIDE", Anim, 8f, -8f, -1, 270540800, 0, false, false, false);
                while (GetEntityAnimCurrentTime(PlayerPedId(), "MP_SUICIDE", Anim) < 0.99f) await BaseScript.Delay(0);
                playerPed.Weapons.Select(WeaponHash.Unarmed);
                BaseScript.TriggerEvent("DamageEvents:PedDied", playerPed.Handle, playerPed.Handle, 3452007600, false);
            };

            #endregion

            PersonalMenu.OnMenuClose += (a) =>
            {
                aperto = false;
            };
            PersonalMenu.Visible = true;
        }

        private static async Task AggiornaSalute()
        {
            Player me = Cache.PlayerCache.MyPlayer.Player;
            if (StatsNeeds.Needs["Fame"].Val > 30f)
                fa.SetRightLabel("~y~" + Math.Round(StatsNeeds.Needs["Fame"].Val, 2) + "%");
            else if (StatsNeeds.Needs["Fame"].Val > 60f)
                fa.SetRightLabel("~o~" + Math.Round(StatsNeeds.Needs["Fame"].Val, 2) + "%");
            else if (StatsNeeds.Needs["Fame"].Val > 90f)
                fa.SetRightLabel("~r~" + Math.Round(StatsNeeds.Needs["Fame"].Val, 2) + "%");
            else
                fa.SetRightLabel("~g~" + Math.Round(StatsNeeds.Needs["Fame"].Val, 2) + "%");
            if (StatsNeeds.Needs["Sete"].Val > 30f)
                se.SetRightLabel("~y~" + Math.Round(StatsNeeds.Needs["Sete"].Val, 2) + "%");
            else if (StatsNeeds.Needs["Sete"].Val > 60f)
                se.SetRightLabel("~o~" + Math.Round(StatsNeeds.Needs["Sete"].Val, 2) + "%");
            else if (StatsNeeds.Needs["Sete"].Val > 90f)
                se.SetRightLabel("~r~" + Math.Round(StatsNeeds.Needs["Sete"].Val, 2) + "%");
            else
                se.SetRightLabel("~g~" + Math.Round(StatsNeeds.Needs["Sete"].Val, 2) + "%");
            if (StatsNeeds.Needs["Stanchezza"].Val > 30f)
                st.SetRightLabel("~y~" + Math.Round(StatsNeeds.Needs["Stanchezza"].Val, 2) + "%");
            else if (StatsNeeds.Needs["Stanchezza"].Val > 60f)
                st.SetRightLabel("~o~" + Math.Round(StatsNeeds.Needs["Stanchezza"].Val, 2) + "%");
            else if (StatsNeeds.Needs["Stanchezza"].Val > 90f)
                st.SetRightLabel("~r~" + Math.Round(StatsNeeds.Needs["Stanchezza"].Val, 2) + "%");
            else
                st.SetRightLabel("~g~" + Math.Round(StatsNeeds.Needs["Stanchezza"].Val, 2) + "%");
            if (me.GetPlayerData().CurrentChar.Needs.Malattia)
                ma.SetRightLabel("~r~In malattia");
            else
                ma.SetRightLabel("~g~In Salute");
            await BaseScript.Delay(3000);
        }

        public static async Task attiva()
        {
            if (EventiPersonalMenu.saveVehicle != null) fuelint = (int)Math.Floor(FuelClient.vehicleFuelLevel(EventiPersonalMenu.saveVehicle) / 65f * 100);

            if (Input.IsControlPressed(Control.InteractionMenu) && !MenuHandler.IsAnyMenuOpen)
            {
                bool tasto = await Input.IsControlStillPressedAsync(Control.InteractionMenu);

                if (tasto && !aperto)
                {
                    if (!MontagneRusse.SonoSeduto && RuotaPanoramica.GiroFinito)
                    {
                        menuPersonal();
                        aperto = true;
                    }
                    else
                    {
                        HUD.ShowNotification("Non puoi aprire il menu sulle giostre!", ColoreNotifica.Red, true);
                    }
                }
            }

            await Task.FromResult(0);
        }

        public enum RouteColor
        {
            Red = 1,
            Green = 2,
            Blue = 3,
            Yellow = 5
        }

        public static async Task routeColor()
        {
            Player me = Cache.PlayerCache.MyPlayer.Player;
            if (Vector3.Distance(PlayerCache.MyPlayer.Posizione.ToVector3, b.Position) > 5000f)
                SetBlipRouteColour(b.Handle, (int)RouteColor.Red);
            else if (Vector3.Distance(PlayerCache.MyPlayer.Posizione.ToVector3, b.Position) < 5000f && Vector3.Distance(PlayerCache.MyPlayer.Posizione.ToVector3, b.Position) > 4500f)
                SetBlipRouteColour(b.Handle, (int)RouteColor.Blue);
            else if (Vector3.Distance(PlayerCache.MyPlayer.Posizione.ToVector3, b.Position) < 4500f && Vector3.Distance(PlayerCache.MyPlayer.Posizione.ToVector3, b.Position) > 2500f)
                SetBlipRouteColour(b.Handle, (int)RouteColor.Yellow);
            else if (Vector3.Distance(PlayerCache.MyPlayer.Posizione.ToVector3, b.Position) < 2500f && Vector3.Distance(PlayerCache.MyPlayer.Posizione.ToVector3, b.Position) > 1500f)
                SetBlipRouteColour(b.Handle, (int)RouteColor.Yellow);
            else if (Vector3.Distance(PlayerCache.MyPlayer.Posizione.ToVector3, b.Position) < 1500f) SetBlipRouteColour(b.Handle, (int)RouteColor.Green);

            if (Vector3.Distance(PlayerCache.MyPlayer.Posizione.ToVector3, b.Position) < 20)
            {
                HUD.ShowNotification("GPS: Sei arrivato a ~b~Destinazione~w~!", ColoreNotifica.GreenDark, true);
                b.ShowRoute = false;
                Client.Instance.RemoveTick(routeColor);
            }

            await Task.FromResult(0);
        }

        public static string getStat(string name)
        {
            int val = 0;
            StatGetInt((uint)Game.GenerateHash(name), ref val, -1);

            return val.ToString();
        }
    }
}