using System;
using System.Collections.Generic;
using System.Drawing;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Shops
{
    internal static class WeaponShops
    {
        private static List<WeaponLicense> weapons1 = new List<WeaponLicense>()
        {
            new WeaponLicense("WEAPON_PISTOL", 300),
            new WeaponLicense("WEAPON_FLASHLIGHT", 60),
            new WeaponLicense("WEAPON_MACHETE", 90),
            new WeaponLicense("WEAPON_NIGHTSTICK", 150),
            new WeaponLicense("WEAPON_BAT", 100),
            new WeaponLicense("WEAPON_FIREEXTINGUISHER", 100),
            new WeaponLicense("WEAPON_BALL", 50),
            new WeaponLicense("WEAPON_SMOKEGRENADE", 100)
        };

        private static List<WeaponLicense> weapons2 = new List<WeaponLicense>()
        {
            new WeaponLicense("WEAPON_MICROSMG", 1400),
            new WeaponLicense("WEAPON_PUMPSHOTGUN", 3400),
            new WeaponLicense("WEAPON_ASSAULTRIFLE", 10000),
            new WeaponLicense("WEAPON_SPECIALCARBINE", 15000),
            new WeaponLicense("WEAPON_SNIPERRIFLE", 22000)
        };

        private static List<WeaponLicense> weapons3 = new List<WeaponLicense>()
        {
            new WeaponLicense("WEAPON_APPISTOL", 1100),
            new WeaponLicense("WEAPON_CARBINERIFLE", 12000),
            new WeaponLicense("WEAPON_HEAVYSNIPER", 30000),
            new WeaponLicense("WEAPON_MINIGUN", 45000),
            new WeaponLicense("WEAPON_RAILGUN", 50000),
            new WeaponLicense("WEAPON_STICKYBOMB", 500)
        };

        private static List<WeaponLicense> components = new List<WeaponLicense>() { new WeaponLicense("COMPONENT_AT_SCOPE_MACRO", 1000), new WeaponLicense("COMPONENT_AT_PI_FLSH", 500) };

        public static List<WeaponLicense> tints = new List<WeaponLicense>()
        {
            new WeaponLicense("WM_TINT0", 500),
            new WeaponLicense("WM_TINT1", 500),
            new WeaponLicense("WM_TINT2", 500),
            new WeaponLicense("WM_TINT3", 500),
            new WeaponLicense("WM_TINT4", 500),
            new WeaponLicense("WM_TINT5", 500),
            new WeaponLicense("WM_TINT6", 500),
            new WeaponLicense("WM_TINT7", 500)
        };

        private static UIMenuItem attTi = null;

        public static async void NewWeaponShop(Ped playerPed, object[] args)
        {
            UIMenu weapShop = new UIMenu(" ", "Your favourite weapon shop", new PointF(0, 0), Main.Textures["AmmuNation"].Key, Main.Textures["AmmuNation"].Value);

            UIMenuItem weapLic1Item = new("Base License Weapons", "Choose your favourite weapon");
            UIMenu weapLic1 = new("Base License Weapons", "");
            UIMenuItem weapLic2Item = new("Intermediate License Weapons", "Choose your favourite weapon");
            UIMenu weapLic2 = new("Intermediate License Weapons", "");
            UIMenuItem weapLic3Item = new("Advcanced License Weapons", "Choose your favourite weapon");
            UIMenu weapLic3 = new("Advcanced License Weapons", "");
            UIMenuItem componentItem = new("Componenti", "Choose the right component!\n⚠️: Advanced magazines, silencers and sights are not on sell!");
            UIMenu component = new("Componenti", "");
            UIMenuItem TintsItem = new("Colors", "Scegli il colore per le tue armi!");
            UIMenu Tints = new("Colors", "");

            weapLic1Item.Activated += async (a, b) => await a.SwitchTo(weapLic1, 0, true);
            weapLic2Item.Activated += async (a, b) => await a.SwitchTo(weapLic2, 0, true);
            weapLic3Item.Activated += async (a, b) => await a.SwitchTo(weapLic3, 0, true);
            componentItem.Activated += async (a, b) => await a.SwitchTo(component, 0, true);
            TintsItem.Activated += async (a, b) => await a.SwitchTo(Tints, 0, true);

            weapShop.AddItem(weapLic1Item);
            weapShop.AddItem(weapLic2Item);
            weapShop.AddItem(weapLic3Item);
            weapShop.AddItem(componentItem);
            weapShop.AddItem(TintsItem);

            weapShop.OnMenuOpen += (a, b) =>
            {
                if (!Cache.PlayerCache.MyPlayer.User.HasLicense("Weapons1"))
                {
                    weapLic1Item.Enabled = false;
                    weapLic1Item.SetRightBadge(BadgeIcon.LOCK);
                }

                if (!Cache.PlayerCache.MyPlayer.User.HasLicense("Weapons2"))
                {
                    weapLic2Item.Enabled = false;
                    weapLic2Item.SetRightBadge(BadgeIcon.LOCK);
                }

                if (!Cache.PlayerCache.MyPlayer.User.HasLicense("Weapons3"))
                {
                    weapLic3Item.Enabled = false;
                    weapLic3Item.SetRightBadge(BadgeIcon.LOCK);
                }

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Weapons.Count != 0) return;
                componentItem.Enabled = false;
                componentItem.SetRightBadge(BadgeIcon.LOCK);
                TintsItem.Enabled = false;
                TintsItem.SetRightBadge(BadgeIcon.LOCK);
            };

            weapLic1.OnMenuOpen += (a, b) =>
            {
                weapLic1.Clear();

                for (int i = 0; i < weapons1.Count; i++)
                {
                    UIMenuItem arma = new UIMenuItem(Functions.GetWeaponLabel(Functions.HashUint(weapons1[i].name)));
                    if (Cache.PlayerCache.MyPlayer.User.Money >= weapons1[i].price || Cache.PlayerCache.MyPlayer.User.Bank >= weapons1[i].price)
                        arma.SetRightLabel("~g~" + weapons1[i].price + "$");
                    else
                        arma.SetRightLabel("~r~" + weapons1[i].price + "$");
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Functions.HashUint(weapons1[i].name))) arma.SetRightBadge(BadgeIcon.GUN);
                    weapLic1.AddItem(arma);
                }

                weapLic1.OnItemSelect += (_menu, _item, _index) =>
                {
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Functions.HashUint(weapons1[_index].name)))
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= 150)
                        {
                            AddAmmoToPed(playerPed.Handle, Functions.HashUint(weapons1[_index].name), 250);
                            HUD.ShowNotification("Since you already own this weapon, you have been reloaded with ammunition at the cost of $150");
                            EventDispatcher.Send("lprp:removemoney", 150);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= 150)
                            {
                                AddAmmoToPed(playerPed.Handle, Functions.HashUint(weapons1[_index].name), 250);
                                HUD.ShowNotification("Since you already own this weapon, you have been reloaded with ammunition at the cost of $150");
                                EventDispatcher.Send("lprp:removebank", 150);
                            }
                            else
                            {
                                HUD.ShowNotification("You do not have the money to purchase the ammunition for this weapon!");
                            }
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= weapons1[_index].price)
                        {
                            EventDispatcher.Send("lprp:addWeapon", weapons1[_index].name, 250);
                            EventDispatcher.Send("lprp:removemoney", weapons1[_index].price);
                            HUD.ShowNotification("You bought a ~y~" + Functions.GetWeaponLabel(Functions.HashUint(weapons1[_index].name)));
                            _item.SetRightBadge(BadgeIcon.GUN);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Money >= weapons1[_index].price)
                            {
                                EventDispatcher.Send("lprp:addWeapon", weapons1[_index].name, 250);
                                EventDispatcher.Send("lprp:removebank", weapons1[_index].price);
                                HUD.ShowNotification("You bought a ~y~" + Functions.GetWeaponLabel(Functions.HashUint(weapons1[_index].name)));
                                _item.SetRightBadge(BadgeIcon.GUN);
                            }
                            else
                            {
                                HUD.ShowNotification("You do not have the money to purchase the ammunition for this weapon!");
                            }
                        }
                    }
                };
            };

            weapLic2.OnMenuOpen += (a, b) =>
            {
                weapLic2.Clear();

                for (int i = 0; i < weapons2.Count; i++)
                {
                    UIMenuItem arma = new UIMenuItem(Functions.GetWeaponLabel(Functions.HashUint(weapons2[i].name)));
                    if (Cache.PlayerCache.MyPlayer.User.Money >= weapons2[i].price || Cache.PlayerCache.MyPlayer.User.Bank >= weapons2[i].price)
                        arma.SetRightLabel("~g~" + weapons2[i].price + "$");
                    else
                        arma.SetRightLabel("~r~" + weapons2[i].price + "$");
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Functions.HashUint(weapons2[i].name))) arma.SetRightBadge(BadgeIcon.GUN);
                    weapLic2.AddItem(arma);
                }

                weapLic2.OnItemSelect += (_menu, _item, _index) =>
                {
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Functions.HashUint(weapons2[_index].name)))
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= 150)
                        {
                            AddAmmoToPed(playerPed.Handle, Functions.HashUint(weapons2[_index].name), 250);
                            HUD.ShowNotification("Since you already own this weapon, you have been reloaded with ammunition at the cost of $150");
                            EventDispatcher.Send("lprp:removemoney", 150);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= 150)
                            {
                                AddAmmoToPed(playerPed.Handle, Functions.HashUint(weapons2[_index].name), 250);
                                HUD.ShowNotification("Since you already own this weapon, you have been reloaded with ammunition at the cost of $150");
                                EventDispatcher.Send("lprp:removebank", 150);
                            }
                            else
                            {
                                HUD.ShowNotification("You do not have the money to purchase the ammunition for this weapon!");
                            }
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= weapons2[_index].price)
                        {
                            EventDispatcher.Send("lprp:addWeapon", weapons2[_index].name, 250);
                            EventDispatcher.Send("lprp:removemoney", weapons2[_index].price);
                            HUD.ShowNotification("You bought a ~y~" + Functions.GetWeaponLabel(Functions.HashUint(weapons2[_index].name)));
                            _item.SetRightBadge(BadgeIcon.GUN);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Money >= weapons2[_index].price)
                            {
                                EventDispatcher.Send("lprp:addWeapon", weapons2[_index].name, 250);
                                EventDispatcher.Send("lprp:removebank", weapons2[_index].price);
                                HUD.ShowNotification("You bought a ~y~" + Functions.GetWeaponLabel(Functions.HashUint(weapons2[_index].name)));
                                _item.SetRightBadge(BadgeIcon.GUN);
                            }
                            else
                            {
                                HUD.ShowNotification("You do not have the money to purchase the ammunition for this weapon!");
                            }
                        }
                    }
                };
            };

            weapLic3.OnMenuOpen += (a, b) =>
            {
                weapLic3.Clear();

                for (int i = 0; i < weapons3.Count; i++)
                {
                    UIMenuItem arma = new UIMenuItem(Functions.GetWeaponLabel(Functions.HashUint(weapons3[i].name)));
                    if (Cache.PlayerCache.MyPlayer.User.Money >= weapons3[i].price || Cache.PlayerCache.MyPlayer.User.Bank >= weapons3[i].price)
                        arma.SetRightLabel("~g~" + weapons3[i].price + "$");
                    else
                        arma.SetRightLabel("~r~" + weapons3[i].price + "$");
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Functions.HashUint(weapons3[i].name))) arma.SetRightBadge(BadgeIcon.GUN);
                    weapLic3.AddItem(arma);
                }

                weapLic3.OnItemSelect += (_menu, _item, _index) =>
                {
                    if (playerPed.Weapons.HasWeapon((WeaponHash)Functions.HashUint(weapons3[_index].name)))
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= 150)
                        {
                            AddAmmoToPed(playerPed.Handle, Functions.HashUint(weapons3[_index].name), 250);
                            HUD.ShowNotification("Since you already own this weapon, you have been reloaded with ammunition at the cost of $150");
                            EventDispatcher.Send("lprp:removemoney", 150);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= 150)
                            {
                                AddAmmoToPed(playerPed.Handle, Functions.HashUint(weapons3[_index].name), 250);
                                HUD.ShowNotification("Since you already own this weapon, you have been reloaded with ammunition at the cost of $150");
                                EventDispatcher.Send("lprp:removebank", 150);
                            }
                            else
                            {
                                HUD.ShowNotification("You do not have the money to purchase the ammunition for this weapon!");
                            }
                        }
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Money >= weapons3[_index].price)
                        {
                            EventDispatcher.Send("lprp:addWeapon", weapons3[_index].name, 250);
                            EventDispatcher.Send("lprp:removemoney", weapons3[_index].price);
                            HUD.ShowNotification("You bought a ~y~" + Functions.GetWeaponLabel(Functions.HashUint(weapons3[_index].name)));
                            _item.SetRightBadge(BadgeIcon.GUN);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Money >= weapons3[_index].price)
                            {
                                EventDispatcher.Send("lprp:addWeapon", weapons3[_index].name, 250);
                                EventDispatcher.Send("lprp:removebank", weapons3[_index].price);
                                HUD.ShowNotification("You bought a ~y~" + Functions.GetWeaponLabel(Functions.HashUint(weapons3[_index].name)));
                                _item.SetRightBadge(BadgeIcon.GUN);
                            }
                            else
                            {
                                HUD.ShowNotification("You do not have the money needed to purchase this weapon!");
                            }
                        }
                    }
                };
            };

            Tints.OnMenuOpen += (a, b) =>
            {
                Tints.Clear();

                foreach (Weapons armi in Cache.PlayerCache.MyPlayer.User.CurrentChar.Weapons)
                {
                    bool Hastints = SharedScript.hasTints(armi.Name);

                    if (!Hastints) continue;
                    UIMenuItem TntItem = new UIMenuItem(Functions.GetWeaponLabel(Functions.HashUint(armi.Name)), "Vedi qui i colori acquistabili per la tua arma");
                    UIMenu Tnt = new(Functions.GetWeaponLabel(Functions.HashUint(armi.Name)), "");
                    TntItem.Activated += async (a, b) => await Tints.SwitchTo(Tnt, 0, true);
                    Tints.AddItem(TntItem);

                    foreach (WeaponLicense tin in tints)
                    {
                        UIMenuItem tintina = new UIMenuItem(Functions.GetWeaponLabel(Functions.HashUint(tin.name)));
                        Tnt.AddItem(tintina);
                        if (Cache.PlayerCache.MyPlayer.User.Money >= tin.price || Cache.PlayerCache.MyPlayer.User.Bank >= tin.price)
                            tintina.SetRightLabel("~g~" + tin.price + "$");
                        else
                            tintina.SetRightLabel("~r~" + tin.price + "$");
                        if (Cache.PlayerCache.MyPlayer.User.HasWeaponTint(armi.Name, Convert.ToInt32(tin.name.Substring(7)))) tintina.SetRightBadge(BadgeIcon.AMMO);
                    }

                    Tnt.OnItemSelect += async (_menu, _item, _index) =>
                    {
                        if (_item.RightBadge == BadgeIcon.AMMO)
                        {
                            HUD.ShowNotification("You already bought this color!!", true);
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Money >= tints[_index].price)
                            {
                                EventDispatcher.Send("lprp:removemoney", tints[_index].price);
                                EventDispatcher.Send("lprp:addWeaponTint", armi.Name, _index);
                                HUD.ShowNotification("You bought a ~y~" + Functions.GetWeaponLabel(Functions.HashUint(tints[_index].name)));
                                _menu.MenuItems.ForEach(x => x.SetRightBadge(BadgeIcon.NONE));
                                //attTi.SetRightBadge(BadgeIcon.NONE);
                                //attTi = _item;
                                _item.SetRightBadge(BadgeIcon.AMMO);
                                armi.Tint = _index;
                            }
                            else
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Bank >= tints[_index].price)
                                {
                                    EventDispatcher.Send("lprp:removebank", tints[_index].price);
                                    EventDispatcher.Send("lprp:addWeaponTint", armi.Name, _index);
                                    HUD.ShowNotification("You bought a ~y~" + Functions.GetWeaponLabel(Functions.HashUint(tints[_index].name)));
                                    _menu.MenuItems.ForEach(x => x.SetRightBadge(BadgeIcon.NONE));
                                    //attTi.SetRightBadge(BadgeIcon.NONE);
                                    //attTi = _item;
                                    _item.SetRightBadge(BadgeIcon.AMMO);
                                    armi.Tint = _index;
                                }
                                else
                                {
                                    HUD.ShowNotification("You don't have the money to buy this color!", true);
                                }
                            }
                        }
                    };
                }
            };

            weapShop.Visible = true;
        }
    }

    public class WeaponLicense
    {
        public string name;
        public int price;

        public WeaponLicense(string name, int price)
        {
            this.name = name;
            this.price = price;
        }
    }
}
