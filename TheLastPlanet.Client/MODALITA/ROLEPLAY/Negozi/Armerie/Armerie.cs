using System;
using System.Collections.Generic;
using System.Drawing;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Negozi
{
    internal static class Armerie
    {
        private static List<ArmiLicenza> armi1 = new List<ArmiLicenza>()
        {
            new ArmiLicenza("WEAPON_PISTOL", 300),
            new ArmiLicenza("WEAPON_FLASHLIGHT", 60),
            new ArmiLicenza("WEAPON_MACHETE", 90),
            new ArmiLicenza("WEAPON_NIGHTSTICK", 150),
            new ArmiLicenza("WEAPON_BAT", 100),
            new ArmiLicenza("WEAPON_FIREEXTINGUISHER", 100),
            new ArmiLicenza("WEAPON_BALL", 50),
            new ArmiLicenza("WEAPON_SMOKEGRENADE", 100)
        };

        private static List<ArmiLicenza> armi2 = new List<ArmiLicenza>()
        {
            new ArmiLicenza("WEAPON_MICROSMG", 1400),
            new ArmiLicenza("WEAPON_PUMPSHOTGUN", 3400),
            new ArmiLicenza("WEAPON_ASSAULTRIFLE", 10000),
            new ArmiLicenza("WEAPON_SPECIALCARBINE", 15000),
            new ArmiLicenza("WEAPON_SNIPERRIFLE", 22000)
        };

        private static List<ArmiLicenza> armi3 = new List<ArmiLicenza>()
        {
            new ArmiLicenza("WEAPON_APPISTOL", 1100),
            new ArmiLicenza("WEAPON_CARBINERIFLE", 12000),
            new ArmiLicenza("WEAPON_HEAVYSNIPER", 30000),
            new ArmiLicenza("WEAPON_MINIGUN", 45000),
            new ArmiLicenza("WEAPON_RAILGUN", 50000),
            new ArmiLicenza("WEAPON_STICKYBOMB", 500)
        };

        private static List<ArmiLicenza> componenti = new List<ArmiLicenza>() { new ArmiLicenza("COMPONENT_AT_SCOPE_MACRO", 1000), new ArmiLicenza("COMPONENT_AT_PI_FLSH", 500) };

        public static List<ArmiLicenza> tinte = new List<ArmiLicenza>()
        {
            new ArmiLicenza("WM_TINT0", 500),
            new ArmiLicenza("WM_TINT1", 500),
            new ArmiLicenza("WM_TINT2", 500),
            new ArmiLicenza("WM_TINT3", 500),
            new ArmiLicenza("WM_TINT4", 500),
            new ArmiLicenza("WM_TINT5", 500),
            new ArmiLicenza("WM_TINT6", 500),
            new ArmiLicenza("WM_TINT7", 500)
        };

        private static UIMenuItem attTi = null;

        public static async void NuovaArmeria(Ped playerPed, object[] args)
        {
            UIMenu Armeria = new UIMenu(" ", "La drogheria delle armi", new PointF(0, 0), Main.Textures["AmmuNation"].Key, Main.Textures["AmmuNation"].Value);

            UIMenuItem ArmiLic1Item = new("Armi Licenza Base", "Scegli l'arma che preferisci");
            UIMenu ArmiLic1 = new("Armi Licenza Base", "");
            UIMenuItem ArmiLic2Item = new("Armi Licenza Intermedia", "Scegli l'arma che preferisci");
            UIMenu ArmiLic2 = new("Armi Licenza Intermedia", "");
            UIMenuItem ArmiLic3Item = new("Armi Licenza Avanzata", "Scegli l'arma che preferisci");
            UIMenu ArmiLic3 = new("Armi Licenza Avanzata", "");
            UIMenuItem componentItem = new("Componenti", "Scegli il componente giusto!\nNB: Caricatori, silenziatori e mirini avanzati non sono in vendita!");
            UIMenu component = new("Componenti", "");
            UIMenuItem TinteItem = new("Colori", "Scegli il colore per le tue armi!");
            UIMenu Tinte = new("Colori", "");

            ArmiLic1Item.Activated += async (a, b) => await a.SwitchTo(ArmiLic1, 0, true);
            ArmiLic2Item.Activated += async (a, b) => await a.SwitchTo(ArmiLic2, 0, true);
            ArmiLic3Item.Activated += async (a, b) => await a.SwitchTo(ArmiLic3, 0, true);
            componentItem.Activated += async (a, b) => await a.SwitchTo(component, 0, true);
            TinteItem.Activated += async (a, b) => await a.SwitchTo(Tinte, 0, true);

            Armeria.AddItem(ArmiLic1Item);
            Armeria.AddItem(ArmiLic2Item);
            Armeria.AddItem(ArmiLic3Item);
            Armeria.AddItem(componentItem);
            Armeria.AddItem(TinteItem);

            Armeria.OnMenuOpen += (a, b) =>
            {
                if (!Cache.PlayerCache.MyPlayer.User.HasLicense("Armi1"))
                {
                    ArmiLic1Item.Enabled = false;
                    ArmiLic1Item.SetRightBadge(BadgeIcon.LOCK);
                }

                if (!Cache.PlayerCache.MyPlayer.User.HasLicense("Armi2"))
                {
                    ArmiLic2Item.Enabled = false;
                    ArmiLic2Item.SetRightBadge(BadgeIcon.LOCK);
                }

                if (!Cache.PlayerCache.MyPlayer.User.HasLicense("Armi3"))
                {
                    ArmiLic3Item.Enabled = false;
                    ArmiLic3Item.SetRightBadge(BadgeIcon.LOCK);
                }

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Weapons.Count != 0) return;
                componentItem.Enabled = false;
                componentItem.SetRightBadge(BadgeIcon.LOCK);
                TinteItem.Enabled = false;
                TinteItem.SetRightBadge(BadgeIcon.LOCK);
            };

            ArmiLic1.OnMenuOpen += (a, b) =>
            {
                ArmiLic1.Clear();

                for (int i = 0; i < armi1.Count; i++)
                {
                    UIMenuItem arma = new UIMenuItem(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[i].name)));
                    if (Cache.PlayerCache.MyPlayer.User.Money >= armi1[i].price || Cache.PlayerCache.MyPlayer.User.Bank >= armi1[i].price)
                        arma.SetRightLabel("~g~" + armi1[i].price + "$");
                    else
                        arma.SetRightLabel("~r~" + armi1[i].price + "$");
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi1[i].name))) arma.SetRightBadge(BadgeIcon.GUN);
                    ArmiLic1.AddItem(arma);
                }

                ArmiLic1.OnItemSelect += (_menu, _item, _index) =>
                {
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi1[_index].name)))
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= 150)
                        {
                            AddAmmoToPed(playerPed.Handle, Funzioni.HashUint(armi1[_index].name), 250);
                            HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
                            EventDispatcher.Send("lprp:removemoney", 150);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= 150)
                            {
                                AddAmmoToPed(playerPed.Handle, Funzioni.HashUint(armi1[_index].name), 250);
                                HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
                                EventDispatcher.Send("lprp:removebank", 150);
                            }
                            else
                            {
                                HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare le munizioni per quest'arma!");
                            }
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= armi1[_index].price)
                        {
                            EventDispatcher.Send("lprp:addWeapon", armi1[_index].name, 250);
                            EventDispatcher.Send("lprp:removemoney", armi1[_index].price);
                            HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[_index].name)));
                            _item.SetRightBadge(BadgeIcon.GUN);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Money >= armi1[_index].price)
                            {
                                EventDispatcher.Send("lprp:addWeapon", armi1[_index].name, 250);
                                EventDispatcher.Send("lprp:removebank", armi1[_index].price);
                                HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[_index].name)));
                                _item.SetRightBadge(BadgeIcon.GUN);
                            }
                            else
                            {
                                HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare quest'arma!");
                            }
                        }
                    }
                };
            };

            ArmiLic2.OnMenuOpen += (a, b) =>
            {
                ArmiLic2.Clear();

                for (int i = 0; i < armi2.Count; i++)
                {
                    UIMenuItem arma = new UIMenuItem(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[i].name)));
                    if (Cache.PlayerCache.MyPlayer.User.Money >= armi2[i].price || Cache.PlayerCache.MyPlayer.User.Bank >= armi2[i].price)
                        arma.SetRightLabel("~g~" + armi2[i].price + "$");
                    else
                        arma.SetRightLabel("~r~" + armi2[i].price + "$");
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi2[i].name))) arma.SetRightBadge(BadgeIcon.GUN);
                    ArmiLic2.AddItem(arma);
                }

                ArmiLic2.OnItemSelect += (_menu, _item, _index) =>
                {
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi2[_index].name)))
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= 150)
                        {
                            AddAmmoToPed(playerPed.Handle, Funzioni.HashUint(armi2[_index].name), 250);
                            HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
                            EventDispatcher.Send("lprp:removemoney", 150);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= 150)
                            {
                                AddAmmoToPed(playerPed.Handle, Funzioni.HashUint(armi2[_index].name), 250);
                                HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
                                EventDispatcher.Send("lprp:removebank", 150);
                            }
                            else
                            {
                                HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare le munizioni per quest'arma!");
                            }
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= armi2[_index].price)
                        {
                            EventDispatcher.Send("lprp:addWeapon", armi2[_index].name, 250);
                            EventDispatcher.Send("lprp:removemoney", armi2[_index].price);
                            HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[_index].name)));
                            _item.SetRightBadge(BadgeIcon.GUN);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Money >= armi2[_index].price)
                            {
                                EventDispatcher.Send("lprp:addWeapon", armi2[_index].name, 250);
                                EventDispatcher.Send("lprp:removebank", armi2[_index].price);
                                HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[_index].name)));
                                _item.SetRightBadge(BadgeIcon.GUN);
                            }
                            else
                            {
                                HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare quest'arma!");
                            }
                        }
                    }
                };
            };

            ArmiLic3.OnMenuOpen += (a, b) =>
            {
                ArmiLic3.Clear();

                for (int i = 0; i < armi3.Count; i++)
                {
                    UIMenuItem arma = new UIMenuItem(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[i].name)));
                    if (Cache.PlayerCache.MyPlayer.User.Money >= armi3[i].price || Cache.PlayerCache.MyPlayer.User.Bank >= armi3[i].price)
                        arma.SetRightLabel("~g~" + armi3[i].price + "$");
                    else
                        arma.SetRightLabel("~r~" + armi3[i].price + "$");
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi3[i].name))) arma.SetRightBadge(BadgeIcon.GUN);
                    ArmiLic3.AddItem(arma);
                }

                ArmiLic3.OnItemSelect += (_menu, _item, _index) =>
                {
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi3[_index].name)))
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= 150)
                        {
                            AddAmmoToPed(playerPed.Handle, Funzioni.HashUint(armi3[_index].name), 250);
                            HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
                            EventDispatcher.Send("lprp:removemoney", 150);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= 150)
                            {
                                AddAmmoToPed(playerPed.Handle, Funzioni.HashUint(armi3[_index].name), 250);
                                HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
                                EventDispatcher.Send("lprp:removebank", 150);
                            }
                            else
                            {
                                HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare le munizioni per quest'arma!");
                            }
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= armi3[_index].price)
                        {
                            EventDispatcher.Send("lprp:addWeapon", armi3[_index].name, 250);
                            EventDispatcher.Send("lprp:removemoney", armi3[_index].price);
                            HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[_index].name)));
                            _item.SetRightBadge(BadgeIcon.GUN);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Money >= armi3[_index].price)
                            {
                                EventDispatcher.Send("lprp:addWeapon", armi3[_index].name, 250);
                                EventDispatcher.Send("lprp:removebank", armi3[_index].price);
                                HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[_index].name)));
                                _item.SetRightBadge(BadgeIcon.GUN);
                            }
                            else
                            {
                                HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare quest'arma!");
                            }
                        }
                    }
                };
            };

            Tinte.OnMenuOpen += (a, b) =>
            {
                Tinte.Clear();

                foreach (Weapons armi in Cache.PlayerCache.MyPlayer.User.CurrentChar.Weapons)
                {
                    bool Hastints = SharedScript.hasTints(armi.name);

                    if (!Hastints) continue;
                    UIMenuItem TntItem = new UIMenuItem(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name)), "Vedi qui i colori acquistabili per la tua arma");
                    UIMenu Tnt = new(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name)), "");
                    TntItem.Activated += async (a, b) => await Tinte.SwitchTo(Tnt, 0, true);
                    Tinte.AddItem(TntItem);

                    foreach (ArmiLicenza tin in tinte)
                    {
                        UIMenuItem tintina = new UIMenuItem(Funzioni.GetWeaponLabel(Funzioni.HashUint(tin.name)));
                        Tnt.AddItem(tintina);
                        if (Cache.PlayerCache.MyPlayer.User.Money >= tin.price || Cache.PlayerCache.MyPlayer.User.Bank >= tin.price)
                            tintina.SetRightLabel("~g~" + tin.price + "$");
                        else
                            tintina.SetRightLabel("~r~" + tin.price + "$");
                        if (Cache.PlayerCache.MyPlayer.User.HasWeaponTint(armi.name, Convert.ToInt32(tin.name.Substring(7)))) tintina.SetRightBadge(BadgeIcon.AMMO);
                    }

                    Tnt.OnItemSelect += async (_menu, _item, _index) =>
                    {
                        if (_item.RightBadge == BadgeIcon.AMMO)
                        {
                            HUD.ShowNotification("Hai già acquistato questo colore!!", true);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Money >= tinte[_index].price)
                            {
                                EventDispatcher.Send("lprp:removemoney", tinte[_index].price);
                                EventDispatcher.Send("lprp:addWeaponTint", armi.name, _index);
                                HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(tinte[_index].name)));
                                _menu.MenuItems.ForEach(x => x.SetRightBadge(BadgeIcon.NONE));
                                //attTi.SetRightBadge(BadgeIcon.NONE);
                                //attTi = _item;
                                _item.SetRightBadge(BadgeIcon.AMMO);
                                armi.tint = _index;
                            }
                            else
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Bank >= tinte[_index].price)
                                {
                                    EventDispatcher.Send("lprp:removebank", tinte[_index].price);
                                    EventDispatcher.Send("lprp:addWeaponTint", armi.name, _index);
                                    HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(tinte[_index].name)));
                                    _menu.MenuItems.ForEach(x => x.SetRightBadge(BadgeIcon.NONE));
                                    //attTi.SetRightBadge(BadgeIcon.NONE);
                                    //attTi = _item;
                                    _item.SetRightBadge(BadgeIcon.AMMO);
                                    armi.tint = _index;
                                }
                                else
                                {
                                    HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare questo colore!", true);
                                }
                            }
                        }
                    };
                }
            };

            Armeria.Visible = true;
        }
    }

    public class ArmiLicenza
    {
        public string name;
        public int price;

        public ArmiLicenza(string name, int price)
        {
            this.name = name;
            this.price = price;
        }
    }
}
