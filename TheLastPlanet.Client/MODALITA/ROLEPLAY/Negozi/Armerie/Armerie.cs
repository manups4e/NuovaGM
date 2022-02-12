﻿using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Negozi
{
    internal static class Armerie
    {
        private static MenuPool pool = HUD.MenuPool;
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
            UIMenu Armeria = new UIMenu(" ", "La drogheria delle armi", new System.Drawing.Point(0, 0), Main.Textures["AmmuNation"].Key, Main.Textures["AmmuNation"].Value);
            HUD.MenuPool.Add(Armeria);
            UIMenu ArmiLic1 = pool.AddSubMenu(Armeria, "Armi Licenza Base", "Scegli l'arma che preferisci");
            UIMenu ArmiLic2 = pool.AddSubMenu(Armeria, "Armi Licenza Intermedia", "Scegli l'arma che preferisci");
            UIMenu ArmiLic3 = pool.AddSubMenu(Armeria, "Armi Licenza Avanzata", "Scegli l'arma che preferisci");
            UIMenu component = pool.AddSubMenu(Armeria, "Componenti", "Scegli il componente giusto!\nNB: Caricatori, silenziatori e mirini avanzati non sono in vendita!");
            UIMenu Tinte = pool.AddSubMenu(Armeria, "Colori", "Scegli il colore per le tue armi!");
            HUD.MenuPool.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
            {
                if (state == MenuState.Opened)
                {
                    if (!Cache.PlayerCache.MyPlayer.User.HasLicense("Armi1"))
                    {
                        ArmiLic1.ParentItem.Enabled = false;
                        ArmiLic1.ParentItem.SetRightBadge(BadgeIcon.LOCK);
                    }

                    if (!Cache.PlayerCache.MyPlayer.User.HasLicense("Armi2"))
                    {
                        ArmiLic2.ParentItem.Enabled = false;
                        ArmiLic2.ParentItem.SetRightBadge(BadgeIcon.LOCK);
                    }

                    if (!Cache.PlayerCache.MyPlayer.User.HasLicense("Armi3"))
                    {
                        ArmiLic3.ParentItem.Enabled = false;
                        ArmiLic3.ParentItem.SetRightBadge(BadgeIcon.LOCK);
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Weapons.Count != 0) return;
                    component.ParentItem.Enabled = false;
                    component.ParentItem.SetRightBadge(BadgeIcon.LOCK);
                    Tinte.ParentItem.Enabled = false;
                    Tinte.ParentItem.SetRightBadge(BadgeIcon.LOCK);
                }

                #region ArmiBase

                else if (state == MenuState.ChangeForward && newmenu == ArmiLic1)
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
                                Client.Instance.Events.Send("lprp:removemoney", 150);
                            }
                            else
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Bank >= 150)
                                {
                                    AddAmmoToPed(playerPed.Handle, Funzioni.HashUint(armi1[_index].name), 250);
                                    HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
                                    Client.Instance.Events.Send("lprp:removebank", 150);
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
                                Client.Instance.Events.Send("lprp:addWeapon", armi1[_index].name, 250);
                                Client.Instance.Events.Send("lprp:removemoney", armi1[_index].price);
                                HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[_index].name)));
                                _item.SetRightBadge(BadgeIcon.GUN);
                            }
                            else
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Money >= armi1[_index].price)
                                {
                                    Client.Instance.Events.Send("lprp:addWeapon", armi1[_index].name, 250);
                                    Client.Instance.Events.Send("lprp:removebank", armi1[_index].price);
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
                }

                #endregion

                #region ArmiMedie

                else if (state == MenuState.ChangeForward && newmenu == ArmiLic2)
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
                                Client.Instance.Events.Send("lprp:removemoney", 150);
                            }
                            else
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Bank >= 150)
                                {
                                    AddAmmoToPed(playerPed.Handle, Funzioni.HashUint(armi2[_index].name), 250);
                                    HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
                                    Client.Instance.Events.Send("lprp:removebank", 150);
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
                                Client.Instance.Events.Send("lprp:addWeapon", armi2[_index].name, 250);
                                Client.Instance.Events.Send("lprp:removemoney", armi2[_index].price);
                                HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[_index].name)));
                                _item.SetRightBadge(BadgeIcon.GUN);
                            }
                            else
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Money >= armi2[_index].price)
                                {
                                    Client.Instance.Events.Send("lprp:addWeapon", armi2[_index].name, 250);
                                    Client.Instance.Events.Send("lprp:removebank", armi2[_index].price);
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
                }

                #endregion

                #region ArmiAvanzate

                else if (state == MenuState.ChangeForward && newmenu == ArmiLic3)
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
                                Client.Instance.Events.Send("lprp:removemoney", 150);
                            }
                            else
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Bank >= 150)
                                {
                                    AddAmmoToPed(playerPed.Handle, Funzioni.HashUint(armi3[_index].name), 250);
                                    HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
                                    Client.Instance.Events.Send("lprp:removebank", 150);
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
                                Client.Instance.Events.Send("lprp:addWeapon", armi3[_index].name, 250);
                                Client.Instance.Events.Send("lprp:removemoney", armi3[_index].price);
                                HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[_index].name)));
                                _item.SetRightBadge(BadgeIcon.GUN);
                            }
                            else
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Money >= armi3[_index].price)
                                {
                                    Client.Instance.Events.Send("lprp:addWeapon", armi3[_index].name, 250);
                                    Client.Instance.Events.Send("lprp:removebank", armi3[_index].price);
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
                }

                #endregion

                #region Componenti

                else if (state == MenuState.ChangeForward && newmenu == component)
                {
                    component.Clear();

                    foreach (Weapons armi in Cache.PlayerCache.MyPlayer.User.CurrentChar.Weapons)
                        if (SharedScript.hasComponents(armi.name))
                        {
                            UIMenu Arma = pool.AddSubMenu(component, Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name)), "Vedi qui i componenti acquistabili per la tua arma");

                            foreach (ArmiLicenza co in componenti)
                                if (SharedScript.hasWeaponComponent(armi.name, co.name))
                                {
                                    UIMenuItem compon = new UIMenuItem(Funzioni.GetWeaponLabel(Funzioni.HashUint(co.name)));
                                    Arma.AddItem(compon);
                                    if (Cache.PlayerCache.MyPlayer.User.Money >= co.price || Cache.PlayerCache.MyPlayer.User.Bank >= co.price)
                                        compon.SetRightLabel("~g~" + co.price + "$");
                                    else
                                        compon.SetRightLabel("~r~" + co.price + "$");
                                    for (int k = 0; k < armi.components.Count; k++)
                                        if (armi.components[k].name == co.name)
                                            compon.SetRightBadge(BadgeIcon.AMMO);
                                }

                            Arma.OnItemSelect += async (_menu, _item, _index) =>
                            {
                                if (_item.RightBadge == BadgeIcon.AMMO)
                                {
                                    HUD.ShowNotification("Hai già acquistato questo componente!!", true);
                                }
                                else
                                {
                                    ArmiLicenza arm = componenti.FirstOrDefault(x => Funzioni.GetWeaponLabel(Funzioni.HashUint(x.name)) == _item.Label);
                                    Client.Logger.Debug("Prezzo = " + arm.price);
                                    Client.Logger.Debug("name = " + arm.name);

                                    if (Cache.PlayerCache.MyPlayer.User.Money >= arm.price)
                                    {
                                        Client.Instance.Events.Send("lprp:addWeaponComponent", armi.name, arm.name);
                                        Client.Instance.Events.Send("lprp:removemoney", arm.price);
                                        HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(arm.name)));
                                        _item.SetRightBadge(BadgeIcon.AMMO);
                                    }
                                    else
                                    {
                                        if (Cache.PlayerCache.MyPlayer.User.Bank >= arm.price)
                                        {
                                            Client.Instance.Events.Send("lprp:addWeaponComponent", armi.name, arm.name);
                                            Client.Instance.Events.Send("lprp:removebank", arm.price);
                                            HUD.ShowNotification("Hai acquistato un/a ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(arm.name)));
                                            _item.SetRightBadge(BadgeIcon.AMMO);
                                        }
                                        else
                                        {
                                            HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare questo componente!", true);
                                        }
                                    }
                                }
                            };
                        }
                }

                #endregion

                #region Tinte

                else if (state == MenuState.ChangeForward && newmenu == Tinte)
                {
                    Tinte.Clear();

                    foreach (Weapons armi in Cache.PlayerCache.MyPlayer.User.CurrentChar.Weapons)
                    {
                        bool Hastints = SharedScript.hasTints(armi.name);

                        if (!Hastints) continue;
                        UIMenu Tnt = pool.AddSubMenu(Tinte, Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name)), "Vedi qui i colori acquistabili per la tua arma");

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
                                    Client.Instance.Events.Send("lprp:removemoney", tinte[_index].price);
                                    Client.Instance.Events.Send("lprp:addWeaponTint", armi.name, _index);
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
                                        Client.Instance.Events.Send("lprp:removebank", tinte[_index].price);
                                        Client.Instance.Events.Send("lprp:addWeaponTint", armi.name, _index);
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
                }

                #endregion
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
