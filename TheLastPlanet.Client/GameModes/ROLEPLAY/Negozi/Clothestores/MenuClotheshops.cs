using Settings.Client.Configuration.Negozi.Abiti;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Shops
{
    internal static class MenuClotheshops
    {
        private static bool accessoriesEnabled = false;
        private static List<UIMenu> _clothesMenu = new();

        #region Utils

        private static async void StartAnim(string lib, string anim)
        {
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(lib, anim, 8f, -8f, -1, AnimationFlags.Loop, 0);
        }

        private static async void StartAnimN(string lib, string anim)
        {
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(lib, anim, 4.0f, -2.0f, -1, AnimationFlags.None, 0);
        }

        private static void StartScenario(string anim)
        {
            Cache.PlayerCache.MyPlayer.Ped.Task.StartScenario(anim, Cache.PlayerCache.MyPlayer.Position.ToVector3);
        }

        public static async Task UpdateDress(dynamic dress)
        {
            int id = Cache.PlayerCache.MyPlayer.Ped.Handle;
            for (int i = 0; i < 9; i++) ClearPedProp(id, i);
            SetPedComponentVariation(id, (int)DrawableIndexes.Face, dress.ComponentDrawables.Faccia, dress.ComponentTextures.Faccia, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Mask, dress.ComponentDrawables.Maschera, dress.ComponentTextures.Maschera, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Torso, dress.ComponentDrawables.Torso, dress.ComponentTextures.Torso, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Pants, dress.ComponentDrawables.Pantaloni, dress.ComponentTextures.Pantaloni, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Bag_Parachute, dress.ComponentDrawables.Borsa_Paracadute, dress.ComponentTextures.Borsa_Paracadute, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Shoes, dress.ComponentDrawables.Scarpe, dress.ComponentTextures.Scarpe, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Accessories, dress.ComponentDrawables.Accessori, dress.ComponentTextures.Accessori, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Undershirt, dress.ComponentDrawables.Sottomaglia, dress.ComponentTextures.Sottomaglia, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Kevlar, dress.ComponentDrawables.Kevlar, dress.ComponentTextures.Kevlar, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Badge, dress.ComponentDrawables.Badge, dress.ComponentTextures.Badge, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Torso_2, dress.ComponentDrawables.Torso_2, dress.ComponentTextures.Torso_2, 2);
            SetPedPropIndex(id, (int)PropIndexes.Hats_Masks, dress.PropIndices.Cappelli_Maschere, dress.PropTextures.Cappelli_Maschere, true);
            SetPedPropIndex(id, (int)PropIndexes.Ears, dress.PropIndices.Orecchie, dress.PropTextures.Orecchie, true);
            SetPedPropIndex(id, (int)PropIndexes.Glasses, dress.PropIndices.Occhiali_Occhi, dress.PropTextures.Occhiali_Occhi, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_3, dress.PropIndices.Unk_3, dress.PropTextures.Unk_3, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_4, dress.PropIndices.Unk_4, dress.PropTextures.Unk_4, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_5, dress.PropIndices.Unk_5, dress.PropTextures.Unk_5, true);
            SetPedPropIndex(id, (int)PropIndexes.Watches, dress.PropIndices.Orologi, dress.PropTextures.Orologi, true);
            SetPedPropIndex(id, (int)PropIndexes.Bracelets, dress.PropIndices.Bracciali, dress.PropTextures.Bracciali, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_8, dress.PropIndices.Unk_8, dress.PropTextures.Unk_8, true);
            await Task.FromResult(0);
        }

        public static string GetRandomAnim(string lib, bool toggle)
        {
            int a;
            int b;
            a = GetRandomIntInRange(0, 3);

            switch (lib)
            {
                case "clothingshirt" when a == 0:
                    {
                        b = GetRandomIntInRange(0, 4);

                        switch (b)
                        {
                            case 0: return "try_shirt_negative_a";
                            case 1: return "try_shirt_negative_b";
                            case 2: return "try_shirt_negative_c";
                            case 3: return "try_shirt_negative_d";
                        }

                        break;
                    }
                case "clothingshirt" when a == 1:
                    {
                        b = GetRandomIntInRange(0, 3);

                        switch (b)
                        {
                            case 0: return "try_shirt_neutral_a";
                            case 1: return "try_shirt_neutral_b";
                            case 2: return "try_shirt_neutral_c";
                        }

                        break;
                    }
                case "clothingshirt":
                    {
                        if (a == 2)
                        {
                            b = GetRandomIntInRange(0, 3);

                            switch (b)
                            {
                                case 0: return "try_shirt_positive_a";
                                case 1: return "try_shirt_positive_b";
                                case 2: return "try_shirt_positive_c";
                                case 3: return "try_shirt_positive_d";
                            }
                        }

                        break;
                    }
                case "clothingshoes" when a == 0:
                    {
                        b = GetRandomIntInRange(0, 4);

                        switch (b)
                        {
                            case 0: return "try_shoes_negative_a";
                            case 1: return "try_shoes_negative_b";
                            case 2: return "try_shoes_negative_c";
                            case 3: return "try_shoes_negative_d";
                        }

                        break;
                    }
                case "clothingshoes" when a == 1:
                    {
                        b = GetRandomIntInRange(0, 4);

                        switch (b)
                        {
                            case 0: return "try_shoes_neutral_a";
                            case 1: return "try_shoes_neutral_b";
                            case 2: return "try_shoes_neutral_c";
                            case 3: return "try_shoes_neutral_d";
                        }

                        break;
                    }
                case "clothingshoes":
                    {
                        if (a == 2)
                        {
                            b = GetRandomIntInRange(0, 4);

                            switch (b)
                            {
                                case 0: return "try_shoes_positive_a";
                                case 1: return "try_shoes_positive_b";
                                case 2: return "try_shoes_positive_c";
                                case 3: return "try_shoes_positive_d";
                            }
                        }

                        break;
                    }
                case "clothingspecs" when toggle:
                    {
                        if (a == 0)
                        {
                            b = GetRandomIntInRange(0, 1);

                            switch (b)
                            {
                                case 0: return "try_glasses_negative_a";
                                case 1: return "try_glasses_negative_c";
                                default:
                                    {
                                        switch (a)
                                        {
                                            case 1: return "try_glasses_neutral_a";
                                            case 2: return "try_glasses_positive_c";
                                        }

                                        break;
                                    }
                            }
                        }

                        break;
                    }
                case "clothingspecs":
                    {
                        b = GetRandomIntInRange(0, 3);

                        switch (a)
                        {
                            case 0 when b == 0: return "try_glasses_negative_a";
                            case 0 when b == 1: return "try_glasses_negative_b";
                            case 0 when b == 2: return "try_glasses_negative_c";
                            case 1 when b == 0: return "try_glasses_neutral_a";
                            case 1 when b == 1: return "try_glasses_neutral_b";
                            case 1 when b == 2: return "try_glasses_neutral_c";
                            case 2 when b == 0: return "try_glasses_positive_a";
                            case 2 when b == 1: return "try_glasses_positive_b";
                            case 2 when b == 2: return "try_glasses_positive_c";
                        }

                        break;
                    }
                case "clothingtie":
                    {
                        b = GetRandomIntInRange(0, 4);

                        switch (a)
                        {
                            case 0:
                                {
                                    b = GetRandomIntInRange(0, 4);

                                    switch (b)
                                    {
                                        case 0: return "try_tie_negative_a";
                                        case 1: return "try_tie_negative_b";
                                        case 2: return "try_tie_negative_c";
                                        case 3: return "try_tie_negative_d";
                                    }

                                    break;
                                }
                            case 1:
                                {
                                    b = GetRandomIntInRange(0, 4);

                                    switch (b)
                                    {
                                        case 0: return "try_tie_neutral_a";
                                        case 1: return "try_tie_neutral_b";
                                        case 2: return "try_tie_neutral_c";
                                        case 3: return "try_tie_neutral_d";
                                    }

                                    break;
                                }
                            case 2:
                                {
                                    b = GetRandomIntInRange(0, 4);

                                    switch (b)
                                    {
                                        case 0: return "try_tie_positive_a";
                                        case 1: return "try_tie_positive_b";
                                        case 2: return "try_tie_positive_c";
                                        case 3: return "try_tie_positive_d";
                                    }

                                    break;
                                }
                        }

                        break;
                    }
                case "clothingtrousers" when a == 0:
                    {
                        b = GetRandomIntInRange(0, 4);

                        switch (b)
                        {
                            case 0: return "try_trousers_negative_a";
                            case 1: return "try_trousers_negative_b";
                            case 2: return "try_trousers_negative_c";
                            case 3: return "try_trousers_negative_d";
                        }

                        break;
                    }
                case "clothingtrousers" when a == 1:
                    {
                        b = GetRandomIntInRange(0, 3);

                        switch (b)
                        {
                            case 0: return "try_trousers_neutral_a";
                            case 1: return "try_trousers_neutral_b";
                            case 2: return "try_trousers_neutral_c";
                            case 3: return "try_trousers_neutral_d";
                        }

                        break;
                    }
                case "clothingtrousers":
                    {
                        if (a == 2)
                        {
                            b = GetRandomIntInRange(0, 4);

                            switch (b)
                            {
                                case 0: return "try_trousers_positive_a";
                                case 1: return "try_trousers_positive_b";
                                case 2: return "try_trousers_positive_c";
                                case 3: return "try_trousers_positive_d";
                            }
                        }

                        break;
                    }
                case "mp_clothing@female@shirt":
                    {
                        b = GetRandomIntInRange(0, 2);

                        switch (b)
                        {
                            case 0: return "try_shirt_negative_a";
                            case 1: return "try_shirt_neutral_a";
                            case 2: return "try_shirt_positive_a";
                        }

                        break;
                    }
                case "mp_clothing@female@trousers":
                    {
                        b = GetRandomIntInRange(0, 2);

                        switch (b)
                        {
                            case 0: return "try_trousers_negative_a";
                            case 1: return "try_trousers_neutral_a";
                            case 2: return "try_trousers_positive_a";
                        }

                        break;
                    }
                case "mp_clothing@female@shoes":
                    {
                        b = GetRandomIntInRange(0, 2);

                        switch (b)
                        {
                            case 0: return "try_shoes_negative_a";
                            case 1: return "try_shoes_neutral_a";
                            case 2: return "try_shoes_positive_a";
                        }

                        break;
                    }
                case "mp_clothing@female@glasses":
                    {
                        b = GetRandomIntInRange(0, 2);

                        switch (b)
                        {
                            case 0: return "try_glasses_negative_a";
                            case 1: return "try_glasses_neutral_a";
                            case 2: return "try_glasses_positive_a";
                        }

                        break;
                    }
            }

            //  else if (lib == "anim@random@shop_clothes@watches" )
            //      return
            return "";
        }

        private static async void TaskLookLeft(Ped p, string anim)
        {
            int sequence = 0;
            OpenSequenceTask(ref sequence);
            TaskPlayAnim(p.Handle, anim, "Profile_L_Intro", 4.0f, -4.0f, -1, 512, 0, false, false, false);
            TaskPlayAnim(p.Handle, anim, "Profile_L_Loop", 4.0f, -4.0f, -1, 513, 0, false, false, false);
            CloseSequenceTask(sequence);
            TaskPerformSequence(p.Handle, sequence);
            ClearSequenceTask(ref sequence);
            await Task.FromResult(0);
        }

        private static async void TaskStopLookLeft(Ped p, string anim)
        {
            int sequence = 0;
            OpenSequenceTask(ref sequence);
            TaskPlayAnim(p.Handle, anim, "Profile_L_Outro", 4.0f, -4.0f, -1, 512, 0, false, false, false);
            TaskPlayAnim(p.Handle, anim, "Loop", 4.0f, -4.0f, -1, 513, 0, false, false, false);
            CloseSequenceTask(sequence);
            TaskPerformSequence(p.Handle, sequence);
            ClearSequenceTask(ref sequence);
            await Task.FromResult(0);
        }

        #endregion

        #region MenuClothes

        public static async void MenuClothes(List<Suit> Completi, string anim, string nome)
        {
            _clothesMenu.Clear();
            UIMenuItem ciao = new("");
            UIMenu MenuVest = new(" ", "~y~Welcome to " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
            MenuVest.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "try_shirt_base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "try_shirt_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            List<Suit> completi = Completi.OrderBy(x => x.Price).ToList();
            _clothesMenu.Add(MenuVest);

            foreach (Suit t in completi)
            {
                UIMenuItem vest = new(t.Name, t.Description);
                MenuVest.AddItem(vest);

                if (Cache.PlayerCache.MyPlayer.User.Money >= t.Price)
                {
                    vest.SetRightLabel("~g~$" + t.Price);
                }
                else
                {
                    if (Cache.PlayerCache.MyPlayer.User.Bank >= t.Price)
                        vest.SetRightLabel("~g~$" + t.Price);
                    else
                        vest.SetRightLabel("~r~$" + t.Price);
                }

                if (t.Name != Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name) continue;
                vest.SetRightBadge(BadgeIcon.CLOTHING); // TODO: CHANGE WITH CLOTHES COLLECTION
                ciao = vest;
            }

            MenuVest.OnIndexChange += async (sender, index) =>
            {
                string random = GetRandomAnim(anim, false);
                await UpdateDress(completi[index]);
                await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
            };
            MenuVest.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name == completi[_index].Name && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description == completi[_index].Description)
                {
                    HUD.ShowNotification("You already bought this clothes!", true);
                }
                else
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= completi[_index].Price)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", completi[_index].Price, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing = completi[_index];
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing = new Dressing(completi[_index].Name, completi[_index].Description, completi[_index].ComponentDrawables, completi[_index].ComponentTextures, completi[_index].PropIndices, completi[_index].PropTextures);
                        ciao.SetRightBadge(BadgeIcon.NONE);
                        ciao = _item;
                        ciao.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("you spent ~g~" + completi[_index].Price + "$~w~, by cash");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= completi[_index].Price)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", completi[_index].Price, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing = completi[_index];
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing = new Dressing(completi[_index].Name, completi[_index].Description, completi[_index].ComponentDrawables, completi[_index].ComponentTextures, completi[_index].PropIndices, completi[_index].PropTextures);
                            ciao.SetRightBadge(BadgeIcon.NONE);
                            ciao = _item;
                            ciao.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("you spent ~g~" + completi[_index].Price + "$~w~, by credit card");
                        }
                        else
                        {
                            HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };
            MenuVest.OnMenuClose += async (a) =>
            {
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                ClotheShopsMain.Exit();
                Client.Instance.RemoveTick(CameraVest);
            };

            MenuVest.Visible = true;
            Client.Instance.AddTick(CameraVest);
        }

        #endregion

        #region PantMenu

        public static async void MenuPants(List<Single> Completi, string anim, string nome)
        {
            _clothesMenu.Clear();
            UIMenuItem ciao = new("");
            UIMenuItem ciaone = new("");
            UIMenu MenuPant = new(" ", "~y~Welcome to " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
            MenuPant.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            _clothesMenu.Add(MenuPant);
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "try_trousers_base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "try_trousers_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            List<Single> completi = Completi.OrderBy(x => x.Price).ToList();
            int money = 0;
            int mod = 0;
            int text = 0;

            foreach (Single v in completi)
            {
                UIMenuItem pantItem = new(v.Title, v.Description);
                UIMenu Pant = new(v.Title, "");
                pantItem.Activated += async (a, b) => await MenuPant.SwitchTo(Pant, 0, true);
                MenuPant.AddItem(pantItem);
                Pant.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                _clothesMenu.Add(Pant);

                foreach (int texture in v.Text)
                {
                    UIMenuItem pant = new("Model " + v.Text.IndexOf(texture));
                    Pant.AddItem(pant);

                    switch (nome)
                    {
                        case "Binco":
                            money = v.Price + v.Text.IndexOf(texture) * 47;

                            break;
                        case "Discount":
                            money = v.Price + v.Text.IndexOf(texture) * 62;

                            break;
                        case "Suburban":
                            money = v.Price + v.Text.IndexOf(texture) * 75;

                            break;
                        case "Ponsombys":
                            money = v.Price + v.Text.IndexOf(texture) * 89;

                            break;
                    }

                    if (Cache.PlayerCache.MyPlayer.User.Money >= money)
                    {
                        pant.SetRightLabel("~g~$" + money);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= money)
                            pant.SetRightLabel("~g~$" + money);
                        else
                            pant.SetRightLabel("~r~$" + money);
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Pants != v.Model || Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Pants != texture) continue;
                    pantItem.SetRightBadge(BadgeIcon.CLOTHING);
                    ciao = pantItem;
                    pant.SetRightBadge(BadgeIcon.CLOTHING); // cambiare con la collezione di abiti
                    ciaone = pant;
                }

                Pant.OnMenuOpen += async (a, b) =>
                {
                    SetPedComponentVariation(PlayerPedId(), 4, v.Model, v.Text[0], 2);
                    string random = GetRandomAnim(anim, false);
                    SetPedComponentVariation(PlayerPedId(), 4, v.Model, v.Text[0], 2);
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                    mod = v.Model;
                    text = v.Text[0];
                };
                Pant.OnMenuClose += async (a) => await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);

                Pant.OnIndexChange += async (sender, index) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedComponentVariation(PlayerPedId(), 4, v.Model, v.Text[index], 2);
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                    mod = v.Model;
                    text = v.Text[index];
                };
                Pant.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Pants == mod && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Pants == text)
                    {
                        HUD.ShowNotification("You already bought these pants!!", true);

                        return;
                    }

                    string m = string.Empty;
                    int val = 0;
                    for (int i = 0; i < _item.RightLabel.Length; i++)
                        if (char.IsDigit(_item.RightLabel[i]))
                            m += _item.RightLabel[i];
                    if (m.Length > 0) val = int.Parse(m);

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Pants = mod;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Pants = text;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        ciao.SetRightBadge(BadgeIcon.NONE);
                        ciao = MenuPant.MenuItems[MenuPant.CurrentSelection];
                        ciao.SetRightBadge(BadgeIcon.CLOTHING);
                        ciaone.SetRightBadge(BadgeIcon.NONE);
                        ciaone = _item;
                        ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Pants = mod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Pants = text;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            ciao.SetRightBadge(BadgeIcon.NONE);
                            ciao = MenuPant.MenuItems[MenuPant.CurrentSelection];
                            ciao.SetRightBadge(BadgeIcon.CLOTHING);
                            ciaone.SetRightBadge(BadgeIcon.NONE);
                            ciaone = _item;
                            ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                        }
                        else
                        {
                            HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            MenuPant.OnMenuClose += async (a) =>
            {
                await BaseScript.Delay(100);

                if (_clothesMenu.Any(t => t.Visible)) return;
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                ClotheShopsMain.Exit();
                Client.Instance.RemoveTick(CameraVest);
            };
            MenuPant.Visible = true;
            Client.Instance.AddTick(CameraVest);
        }

        #endregion

        #region ShoesMenu

        public static async void MenuShoes(List<Single> Completi, string anim, string nome)
        {
            _clothesMenu.Clear();
            UIMenuItem ciao = new("");
            UIMenuItem ciaone = new("");
            UIMenu MenuScarpe = new(" ", "~y~Welcome to " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
            _clothesMenu.Add(MenuScarpe);
            MenuScarpe.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "try_shoes_base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "try_shoes_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            List<Single> completi = Completi.OrderBy(x => x.Price).ToList();
            int money = 0;
            int mod = 0;
            int text = 0;

            foreach (Single v in completi)
            {
                UIMenuItem ScarpItem = new(v.Title, v.Description);
                UIMenu Scarp = new(v.Title, "");
                ScarpItem.BindItemToMenu(Scarp);
                MenuScarpe.AddItem(ScarpItem);
                Scarp.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                _clothesMenu.Add(Scarp);

                foreach (int texture in v.Text)
                {
                    UIMenuItem pant = new("Model " + v.Text.IndexOf(texture));
                    Scarp.AddItem(pant);

                    switch (nome)
                    {
                        case "Binco":
                            money = v.Price + v.Text.IndexOf(texture) * 47;

                            break;
                        case "Discount":
                            money = v.Price + v.Text.IndexOf(texture) * 62;

                            break;
                        case "Suburban":
                            money = v.Price + v.Text.IndexOf(texture) * 75;

                            break;
                        case "Ponsombys":
                            money = v.Price + v.Text.IndexOf(texture) * 89;

                            break;
                    }

                    if (Cache.PlayerCache.MyPlayer.User.Money >= money)
                    {
                        pant.SetRightLabel("~g~$" + money);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= money)
                            pant.SetRightLabel("~g~$" + money);
                        else
                            pant.SetRightLabel("~r~$" + money);
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Shoes != v.Model || Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Shoes != texture) continue;
                    ScarpItem.SetRightBadge(BadgeIcon.CLOTHING);
                    ciao = ScarpItem;
                    pant.SetRightBadge(BadgeIcon.CLOTHING); // cambiare con la collezione di abiti
                    ciaone = pant;
                }

                Scarp.OnMenuOpen += async (a, b) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedComponentVariation(PlayerPedId(), 6, v.Model, v.Text[0], 2);
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                    mod = v.Model;
                    text = v.Text[0];
                };
                Scarp.OnMenuClose += async (a) => await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);

                Scarp.OnIndexChange += async (sender, index) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedComponentVariation(PlayerPedId(), 6, v.Model, v.Text[index], 2);
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                    mod = v.Model;
                    text = v.Text[index];
                };
                Scarp.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Shoes == mod && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Shoes == text)
                    {
                        HUD.ShowNotification("You already bought these shoes!!", true);

                        return;
                    }

                    string m = string.Empty;
                    int val = 0;
                    foreach (char t in _item.RightLabel)
                        if (char.IsDigit(t))
                            m += t;
                    if (m.Length > 0) val = int.Parse(m);

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Shoes = mod;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Shoes = text;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        ciao.SetRightBadge(BadgeIcon.NONE);
                        ciao = MenuScarpe.MenuItems[MenuScarpe.CurrentSelection];
                        ciao.SetRightBadge(BadgeIcon.CLOTHING);
                        ciaone.SetRightBadge(BadgeIcon.NONE);
                        ciaone = _item;
                        ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Shoes = mod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Shoes = text;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            ciao.SetRightBadge(BadgeIcon.NONE);
                            ciao = MenuScarpe.MenuItems[MenuScarpe.CurrentSelection];
                            ciao.SetRightBadge(BadgeIcon.CLOTHING);
                            ciaone.SetRightBadge(BadgeIcon.NONE);
                            ciaone = _item;
                            ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                        }
                        else
                        {
                            HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            MenuScarpe.OnMenuClose += async (a) =>
            {
                await BaseScript.Delay(100);

                foreach (UIMenu t in _clothesMenu)
                    if (t.Visible)
                        return;
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                ClotheShopsMain.Exit();
                Client.Instance.RemoveTick(CameraVest);
            };
            MenuScarpe.Visible = true;
            Client.Instance.AddTick(CameraVest);
        }

        #endregion

        #region OcchialiMenu

        public static async void MenuOcchiali(List<Single> Completi, string anim, string nome)
        {
            _clothesMenu.Clear();
            UIMenuItem ciao = new("");
            UIMenuItem ciaone = new("");
            UIMenu MenuOcchiali = new(" ", "~y~Welcome to " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
            _clothesMenu.Add(MenuOcchiali);
            MenuOcchiali.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "Try_Glasses_Base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "Try_Glasses_Base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            List<Single> completi = Completi.OrderBy(x => x.Price).ToList();
            int money = 0;
            int mod = 0;
            int text = 0;

            foreach (Single v in completi)
            {
                UIMenuItem ScarpItem = new(v.Title, v.Description);
                UIMenu Scarp = new(v.Title, "");
                ScarpItem.BindItemToMenu(Scarp);
                MenuOcchiali.AddItem(ScarpItem);
                Scarp.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                _clothesMenu.Add(Scarp);

                foreach (int texture in v.Text)
                {
                    UIMenuItem pant = new("Colore " + v.Text.IndexOf(texture));
                    Scarp.AddItem(pant);

                    switch (nome)
                    {
                        case "Binco":
                            money = v.Price + v.Text.IndexOf(texture) * 47;

                            break;
                        case "Discount":
                            money = v.Price + v.Text.IndexOf(texture) * 62;

                            break;
                        case "Suburban":
                            money = v.Price + v.Text.IndexOf(texture) * 75;

                            break;
                        case "Ponsombys":
                            money = v.Price + v.Text.IndexOf(texture) * 89;

                            break;
                    }

                    if (Cache.PlayerCache.MyPlayer.User.Money >= money)
                    {
                        pant.SetRightLabel("~g~$" + money);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= money)
                            pant.SetRightLabel("~g~$" + money);
                        else
                            pant.SetRightLabel("~r~$" + money);
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears != v.Model || Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Ears != texture) continue;
                    ScarpItem.SetRightBadge(BadgeIcon.CLOTHING);
                    ciao = ScarpItem;
                    pant.SetRightBadge(BadgeIcon.CLOTHING); // cambiare con la collezione di abiti
                    ciaone = pant;
                }

                Scarp.OnMenuOpen += async (a, b) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedPropIndex(PlayerPedId(), 1, v.Model, v.Text[0], false);
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                    mod = v.Model;
                    text = v.Text[0];
                };

                Scarp.OnMenuClose += async (a) => await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);

                Scarp.OnIndexChange += async (menu, index) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedPropIndex(PlayerPedId(), 1, v.Model, v.Text[index], false);
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                    mod = v.Model;
                    text = v.Text[index];
                };
                Scarp.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears == mod && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Ears == text)
                    {
                        HUD.ShowNotification("You already bought these glasses!!", true);

                        return;
                    }

                    string m = string.Empty;
                    int val = 0;
                    for (int i = 0; i < _item.RightLabel.Length; i++)
                        if (char.IsDigit(_item.RightLabel[i]))
                            m += _item.RightLabel[i];
                    if (m.Length > 0) val = int.Parse(m);

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears = mod;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Ears = text;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        ciao.SetRightBadge(BadgeIcon.NONE);
                        ciao = MenuOcchiali.MenuItems[MenuOcchiali.CurrentSelection];
                        ciao.SetRightBadge(BadgeIcon.CLOTHING);
                        ciaone.SetRightBadge(BadgeIcon.NONE);
                        ciaone = _item;
                        ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears = mod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Ears = text;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            ciao.SetRightBadge(BadgeIcon.NONE);
                            ciao = MenuOcchiali.MenuItems[MenuOcchiali.CurrentSelection];
                            ciao.SetRightBadge(BadgeIcon.CLOTHING);
                            ciaone.SetRightBadge(BadgeIcon.NONE);
                            ciaone = _item;
                            ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                        }
                        else
                        {
                            HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            MenuOcchiali.OnMenuClose += async (a) =>
            {
                await BaseScript.Delay(100);

                foreach (UIMenu t in _clothesMenu)
                    if (t.Visible)
                        return;
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                ClotheShopsMain.Exit();
                Client.Instance.RemoveTick(CameraVest);
            };
            MenuOcchiali.Visible = true;
            Client.Instance.AddTick(CameraVest);
        }

        #endregion

        #region AccessoriMenu

        private static UIMenu bags = new(" ", "Bags");
        private static UIMenu earr = new(" ", "Earrings and earphones");
        private static UIMenu hats = new(" ", "Hats");
        private static UIMenu wrist = new(" ", "Watches and bracelets");
        private static UIMenu watch = new(" ", "Watches");
        private static UIMenu brace = new(" ", "Bracelets");
        private static UIMenu tempwatch = new(" ", " ");
        private static List<UIMenu> SubMenusHats = new();
        private static List<UIMenu> SubMenusWrist = new();

        private static float fov = 0;

        public static async void AccessoriesMenu(Accessories accessory, string anim, string name)
        {
            accessoriesEnabled = true;
            UIMenuItem currentEarr = new("");
            UIMenuItem currentEarrCol = new("");
            UIMenuItem currentWatch = new("");
            UIMenuItem currentWatchMod = new("");
            UIMenuItem currentBrace = new("");
            UIMenuItem currentHat = new("");
            UIMenuItem currentHatMod = new("");
            UIMenuItem currentBag = new("");
            UIMenuItem bagRim = new("");
            UIMenu accessoriesMenu = new(" ", "~y~Welcome to " + name + "!", new System.Drawing.Point(0, 0), Main.Textures[name].Key, Main.Textures[name].Value);

            accessoriesMenu.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "try_shirt_base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "try_shirt_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            int money = 0;
            int IntEarrAtt = Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Style;
            int IntColoEarrAtt = Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Color;
            int IntWatchAtt = GetPedPropIndex(PlayerPedId(), 6);
            int IntWatchMod = GetPedPropTextureIndex(PlayerPedId(), 6);
            int IntBraceAtt = GetPedPropIndex(PlayerPedId(), 7);
            int IntHatAtt = GetPedPropIndex(PlayerPedId(), 0);
            int IntHatAttMod = GetPedPropTextureIndex(PlayerPedId(), 0);
            int IntBag = GetPedDrawableVariation(PlayerPedId(), 5);
            int mod = 0;
            int text = 0;

            #region sottomenu

            UIMenuItem bagsItem = new("Bags");
            bags = new("Bags", "");
            bags.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            UIMenuItem hatsItem = new("Hats");
            hats = new("Hats", "");
            hats.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            UIMenuItem earItem = new("Earrings and Earphones");
            MenuClotheshops.earr = new("Earrings and Earphones", "");
            MenuClotheshops.earr.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            UIMenuItem wristItem = new("Watches and Bracelets");
            wrist = new("Watches and Bracelets", "");
            wrist.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            UIMenuItem watchItem = new("Watches");
            watch = new("Watches", "");
            watch.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            UIMenuItem bracItem = new("Braceletes");
            brace = new("Braceletes", "");
            brace.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));

            bagsItem.BindItemToMenu(accessoriesMenu);
            hatsItem.BindItemToMenu(accessoriesMenu);
            earItem.BindItemToMenu(accessoriesMenu);
            wristItem.BindItemToMenu(accessoriesMenu);
            watchItem.BindItemToMenu(wrist);
            bracItem.BindItemToMenu(wrist);


            accessoriesMenu.AddItem(bagsItem);
            accessoriesMenu.AddItem(hatsItem);
            accessoriesMenu.AddItem(earItem);
            accessoriesMenu.AddItem(wristItem);
            wrist.AddItem(watchItem);
            wrist.AddItem(bracItem);



            SubMenusWrist.Add(watch);
            SubMenusWrist.Add(brace);

            #endregion

            List<dynamic> earr = accessory.Head.Earpieces.OrderBy(x => x.Price).ToList().Select(x => x.Title).ToList().Cast<dynamic>().ToList();
            List<dynamic> bracel = accessory.Bracelets.OrderBy(x => x.Price).ToList().Select(x => x.Title).ToList().Cast<dynamic>().ToList();
            UIMenuListItem earrings = new("Earrings", earr, 0);
            MenuClotheshops.earr.AddItem(earrings);
            UIMenuListItem bracelets = new("Bracelets", bracel, 0);
            brace.AddItem(bracelets);

            foreach (Single borsa in accessory.Bags)
            {
                money = borsa.Price;
                UIMenuItem bors = new(borsa.Title, borsa.Description);

                if (Cache.PlayerCache.MyPlayer.User.Money >= money)
                {
                    bors.SetRightLabel("~g~$" + money);
                }
                else
                {
                    if (Cache.PlayerCache.MyPlayer.User.Bank >= money)
                        bors.SetRightLabel("~g~$" + money);
                    else
                        bors.SetRightLabel("~r~$" + money);
                }

                bags.AddItem(bors);

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Bag_Parachute != borsa.Model) continue;
                bors.SetRightBadge(BadgeIcon.CLOTHING);
                currentBag = bors;
            }

            bags.OnIndexChange += async (_menu, index) =>
            {
                IntBag = accessory.Bags[index].Model;
                string random = GetRandomAnim(anim, false);
                SetPedComponentVariation(PlayerPedId(), 5, IntBag, 0, 2);
                await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
            };
            bags.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Bag_Parachute == IntBag)
                {
                    HUD.ShowNotification("You already bought this bag!!", true);

                    return;
                }

                string m = string.Empty;
                int val = 0;
                for (int i = 0; i < _item.RightLabel.Length; i++)
                    if (char.IsDigit(_item.RightLabel[i]))
                        m += _item.RightLabel[i];
                if (m.Length > 0) val = int.Parse(m);

                if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                {
                    BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Bag_Parachute = IntBag;
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Bag_Parachute = 0;
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                    currentBag.SetRightBadge(BadgeIcon.NONE);
                    currentBag = _menu.MenuItems[_menu.CurrentSelection];
                    currentBag.SetRightBadge(BadgeIcon.CLOTHING);
                    HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                }
                else
                {
                    if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Bag_Parachute = IntBag;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Bag_Parachute = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        currentBag.SetRightBadge(BadgeIcon.NONE);
                        currentBag = _menu.MenuItems[_menu.CurrentSelection];
                        currentBag.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                    }
                    else
                    {
                        HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                    }
                }
            };
            UIMenuItem currentHat1 = new("");
            UIMenuItem newHat = new("");
            UIMenuItem hatTmpItem = new("");
            UIMenu hatTmp = new("", "");

            foreach (Single _hat in accessory.Head.Hats)
            {
                if (accessory.Head.Hats.IndexOf(_hat) != 0)
                {
                    UIMenuItem _hatItem = new UIMenuItem(_hat.Title + " " + accessory.Head.Hats.IndexOf(_hat), _hat.Description);
                    hatTmp = new(_hat.Title + " " + accessory.Head.Hats.IndexOf(_hat), "");
                    _hatItem.BindItemToMenu(hatTmp);
                    hats.AddItem(_hatItem);
                    hatTmp.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                    SubMenusHats.Add(hatTmp);
                }

                if (_hat.Model == -1)
                {
                    bagRim = new UIMenuItem(_hat.Title, _hat.Description);
                    hats.AddItem(bagRim);
                    bagRim.SetRightLabel("~g~$0");
                }

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks == -1)
                {
                    bagRim.SetRightBadge(BadgeIcon.CLOTHING);
                    currentHat = bagRim;
                }

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks != -1 && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks == _hat.Model)
                {
                    hatTmpItem.SetRightBadge(BadgeIcon.CLOTHING);
                    currentHat = bagRim;
                }

                foreach (int texture in _hat.Text)
                {
                    newHat = new UIMenuItem("Model " + _hat.Text.IndexOf(texture), _hat.Description);
                    hatTmp.AddItem(newHat);

                    switch (name)
                    {
                        case "Binco":
                            money = _hat.Price + _hat.Text.IndexOf(texture) * 47;

                            break;
                        case "Discount":
                            money = _hat.Price + _hat.Text.IndexOf(texture) * 62;

                            break;
                        case "Suburban":
                            money = _hat.Price + _hat.Text.IndexOf(texture) * 75;

                            break;
                        case "Ponsombys":
                            money = _hat.Price + _hat.Text.IndexOf(texture) * 89;

                            break;
                    }

                    if (Cache.PlayerCache.MyPlayer.User.Money >= money)
                    {
                        newHat.SetRightLabel("~g~$" + money);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= money)
                            newHat.SetRightLabel("~g~$" + money);
                        else
                            newHat.SetRightLabel("~r~$" + money);
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks != _hat.Model || Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Hats_masks != texture) continue;
                    newHat.SetRightBadge(BadgeIcon.CLOTHING);
                    currentHat1 = newHat;
                }

                hatTmp.OnIndexChange += async (_menu, _newIndex) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedPropIndex(PlayerPedId(), 0, _hat.Model, _hat.Text[_newIndex], false);
                    IntHatAtt = _hat.Text[_newIndex];
                    IntHatAttMod = _hat.Text[_newIndex];
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                };
                hatTmp.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks == IntHatAtt && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Hats_masks == IntHatAttMod)
                    {
                        HUD.ShowNotification("You already bought this hat model!");
                    }
                    else
                    {
                        string m = string.Empty;
                        int val = 0;
                        foreach (char t in _item.RightLabel)
                            if (char.IsDigit(t))
                                m += t;
                        if (m.Length > 0) val = int.Parse(m);

                        if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks = IntHatAtt;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Hats_masks = IntHatAttMod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            currentHat.SetRightBadge(BadgeIcon.NONE);
                            currentHat = hats.MenuItems[hats.CurrentSelection];
                            currentHat.SetRightBadge(BadgeIcon.CLOTHING);
                            currentHat1.SetRightBadge(BadgeIcon.NONE);
                            currentHat1 = _menu.MenuItems[_menu.CurrentSelection];
                            currentHat1.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                            {
                                BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks = IntHatAtt;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Hats_masks = IntHatAttMod;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                                currentHat.SetRightBadge(BadgeIcon.NONE);
                                currentHat = hats.MenuItems[hats.CurrentSelection];
                                currentHat.SetRightBadge(BadgeIcon.CLOTHING);
                                currentHat1.SetRightBadge(BadgeIcon.NONE);
                                currentHat1 = _menu.MenuItems[_menu.CurrentSelection];
                                currentHat1.SetRightBadge(BadgeIcon.CLOTHING);
                                HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                            }
                            else
                            {
                                HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                            }
                        }
                    }
                };
                hatTmp.OnMenuOpen += async (a, b) =>
                {
                    PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                    await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                };
            }

            hats.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item != bagRim) return;

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks == -1)
                {
                    HUD.ShowNotification("You can't remove twice your hat!!", true);
                }
                else
                {
                    string m = string.Empty;
                    int val = 0;
                    for (int i = 0; i < _item.RightLabel.Length; i++)
                        if (char.IsDigit(_item.RightLabel[i]))
                            m += _item.RightLabel[i];
                    if (m.Length > 0) val = int.Parse(m);

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks = -1;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Hats_masks = -1;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        currentHat.SetRightBadge(BadgeIcon.NONE);
                        currentHat = _menu.MenuItems[_menu.CurrentSelection];
                        currentHat.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Hats_masks = -1;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Hats_masks = -1;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            currentHat.SetRightBadge(BadgeIcon.NONE);
                            currentHat = _menu.MenuItems[_menu.CurrentSelection];
                            currentHat.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                        }
                        else
                        {
                            HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };
            MenuClotheshops.earr.OnListChange += (_menu, _listItem, _newIndex) =>
            {
                string ActiveItem = _listItem.Items[_newIndex].ToString();

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears == accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_newIndex].Model)
                {
                    //					_listItem.SetRightBadge(BadgeIcon.CLOTHING);
                }
                else
                {
                    //					_listItem.SetRightBadge(BadgeIcon.NONE);
                }

                _listItem.Description = accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_newIndex].Description + ", Prezzo: $" + accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_newIndex].Price;
                SetPedPropIndex(PlayerPedId(), 2, accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_newIndex].Model, 0, false);
                IntEarrAtt = accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_newIndex].Model;
                _menu.UpdateDescription();
            };
            MenuClotheshops.earr.OnItemSelect += (_menu, _listItem, _listIndex) =>
            {
                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears == accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_listIndex].Model && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears == -1)
                {
                    HUD.ShowNotification("You can't remove twice your earrings!!", true);
                }
                else if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears == accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_listIndex].Model && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears != -1)
                {
                    HUD.ShowNotification("You already bought this glasses model!");
                }
                else
                {
                    int val = accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_listIndex].Price;

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears = accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_listIndex].Model;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Ears = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Style = accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_listIndex].Model;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Color = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears = accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_listIndex].Model;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Ears = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Style = accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_listIndex].Model;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Color = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                        }
                        else
                        {
                            HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };

            foreach (Single _earph in accessory.Head.Earphones.OrderBy(x => x.Price).ToList())
            {
                UIMenuItem _earpiece = new(_earph.Title, _earph.Description);

                if (Cache.PlayerCache.MyPlayer.User.Money >= _earph.Price)
                {
                    _earpiece.SetRightLabel("~g~$" + _earph.Price);
                }
                else
                {
                    if (Cache.PlayerCache.MyPlayer.User.Bank >= _earph.Price)
                        _earpiece.SetRightLabel("~g~$" + _earph.Price);
                    else
                        _earpiece.SetRightLabel("~r~$" + _earph.Price);
                }

                MenuClotheshops.earr.AddItem(_earpiece);
            }

            MenuClotheshops.earr.OnIndexChange += async (_menu, _newIndex) =>
            {
                if (_newIndex == 0) return;
                if (accessory.Head.Earphones.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_newIndex - 1].Model == -1)
                    ClearPedProp(PlayerPedId(), 2);
                else
                    SetPedPropIndex(PlayerPedId(), 2, accessory.Head.Earphones.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_newIndex - 1].Model, 0, false);
            };
            MenuClotheshops.earr.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (_index == 0) return;

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears == accessory.Head.Earphones.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_index - 1].Model && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears == -1)
                {
                    HUD.ShowNotification("You can't remove your earphone twice!!", true);
                }
                else if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears == accessory.Head.Earphones.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_index - 1].Model && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears != -1)
                {
                    HUD.ShowNotification("You already bought this earphone!");
                }
                else
                {
                    int val = accessory.Head.Earpieces.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_index - 1].Price;

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears = accessory.Head.Earphones.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_index - 1].Model;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Ears = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Style = accessory.Head.Earphones.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_index - 1].Model;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Color = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Ears = accessory.Head.Earphones.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_index - 1].Model;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Ears = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Style = accessory.Head.Earphones.OrderBy<Single, int>(x => x.Price).ToList<Single>()[_index - 1].Model;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Color = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                        }
                        else
                        {
                            HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };
            UIMenuItem WatchItem = new("");
            UIMenuItem noWatch = new("");
            UIMenuItem newWatch = new("");

            foreach (Single _watch in accessory.Watches)
            {
                if (accessory.Watches.IndexOf(_watch) != 0)
                {
                    UIMenuItem _watchItem = new(_watch.Title, _watch.Description);
                    tempwatch = new(_watch.Title, "");
                    _watchItem.BindItemToMenu(tempwatch);
                    watch.AddItem(_watchItem);
                    tempwatch.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                    SubMenusWrist.Add(tempwatch);
                }

                if (_watch.Model == -1)
                {
                    noWatch = new UIMenuItem(_watch.Title, _watch.Description);
                    watch.AddItem(noWatch);

                    if (Cache.PlayerCache.MyPlayer.User.Money >= _watch.Price)
                    {
                        noWatch.SetRightLabel("~g~$" + _watch.Price);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= _watch.Price)
                            noWatch.SetRightLabel("~g~$" + _watch.Price);
                        else
                            noWatch.SetRightLabel("~r~$" + _watch.Price);
                    }
                }

                foreach (int v in _watch.Text)
                {
                    newWatch = new UIMenuItem("Model " + _watch.Text.IndexOf(v), _watch.Description);
                    tempwatch.AddItem(newWatch);

                    switch (name)
                    {
                        case "Binco":
                            money = _watch.Price + _watch.Text.IndexOf(v) * 47;

                            break;
                        case "Discount":
                            money = _watch.Price + _watch.Text.IndexOf(v) * 62;

                            break;
                        case "Suburban":
                            money = _watch.Price + _watch.Text.IndexOf(v) * 75;

                            break;
                        case "Ponsombys":
                            money = _watch.Price + _watch.Text.IndexOf(v) * 89;

                            break;
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Watches == _watch.Model && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Watches == v)
                    {
                        if (_watch.Model == -1)
                        {
                            noWatch.SetRightBadge(BadgeIcon.CLOTHING);
                            currentWatch = noWatch;
                        }
                        else
                        {
                            WatchItem.SetRightBadge(BadgeIcon.CLOTHING);
                            currentWatch = WatchItem;
                        }

                        newWatch.SetRightBadge(BadgeIcon.CLOTHING);
                        currentWatchMod = newWatch;
                    }

                    if (Cache.PlayerCache.MyPlayer.User.Money >= money)
                    {
                        newWatch.SetRightLabel("~g~$" + money);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= money)
                            newWatch.SetRightLabel("~g~$" + money);
                        else
                            newWatch.SetRightLabel("~r~$" + money);
                    }

                    tempwatch.OnMenuOpen += (a, b) =>
                    {
                        SetPedPropIndex(PlayerPedId(), 6, _watch.Model, 0, false);
                        IntWatchAtt = _watch.Model;
                        IntWatchMod = 0;
                    };
                    tempwatch.OnIndexChange += async (_menu, _newIndex) =>
                    {
                        SetPedPropIndex(PlayerPedId(), 6, _watch.Model, _watch.Text[_newIndex], false);
                        IntWatchAtt = _watch.Model;
                        IntWatchMod = _watch.Text[_newIndex];
                    };
                }

                tempwatch.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Watches == IntWatchAtt && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Watches == IntWatchMod)
                    {
                        HUD.ShowNotification("You already bouth this watch model!", true);
                    }
                    else
                    {
                        string m = string.Empty;
                        int val = 0;
                        for (int i = 0; i < _item.RightLabel.Length; i++)
                            if (char.IsDigit(_item.RightLabel[i]))
                                m += _item.RightLabel[i];
                        if (m.Length > 0) val = int.Parse(m);

                        if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Watches = IntWatchAtt;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Watches = IntWatchMod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            currentWatch.SetRightBadge(BadgeIcon.NONE);
                            currentWatch = watch.MenuItems[watch.CurrentSelection];
                            currentWatch.SetRightBadge(BadgeIcon.CLOTHING);
                            currentWatchMod.SetRightBadge(BadgeIcon.NONE);
                            currentWatchMod = _menu.MenuItems[_menu.CurrentSelection];
                            currentWatchMod.SetRightBadge(BadgeIcon.CLOTHING);
                            if (val > 0) HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                            {
                                BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Watches = IntWatchAtt;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Watches = IntWatchMod;
                                currentWatch.SetRightBadge(BadgeIcon.NONE);
                                currentWatch = watch.MenuItems[watch.CurrentSelection];
                                currentWatch.SetRightBadge(BadgeIcon.CLOTHING);
                                currentWatchMod.SetRightBadge(BadgeIcon.NONE);
                                currentWatchMod = _menu.MenuItems[_menu.CurrentSelection];
                                currentWatchMod.SetRightBadge(BadgeIcon.CLOTHING);
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                                if (val > 0) HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                            }
                            else
                            {
                                HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                            }
                        }
                    }
                };
            }

            watch.OnIndexChange += async (_menu, _newIndex) =>
            {
                if (_menu.MenuItems[_newIndex] == noWatch) ClearPedProp(PlayerPedId(), 6);
            };
            watch.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (_item != noWatch) return;

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Watches == -1 && IntWatchAtt == -1)
                {
                    HUD.ShowNotification("You can't remove a watch twice!!", true);
                }
                else
                {
                    string m = string.Empty;
                    int val = 0;
                    for (int i = 0; i < _item.RightLabel.Length; i++)
                        if (char.IsDigit(_item.RightLabel[i]))
                            m += _item.RightLabel[i];
                    if (m.Length > 0) val = int.Parse(m);

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Watches = -1;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Watches = -1;
                        currentWatch.SetRightBadge(BadgeIcon.NONE);
                        currentWatch = _menu.MenuItems[_menu.CurrentSelection];
                        currentWatch.SetRightBadge(BadgeIcon.CLOTHING);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Ears.Style = accessory.Watches.OrderBy(x => x.Price).ToList()[_index].Model;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        if (val > 0) HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Watches = -1;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Watches = -1;
                            currentWatch.SetRightBadge(BadgeIcon.NONE);
                            currentWatch = _menu.MenuItems[_menu.CurrentSelection];
                            currentWatch.SetRightBadge(BadgeIcon.CLOTHING);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            if (val > 0) HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                        }
                        else
                        {
                            HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };
            brace.OnListChange += (_menu, _listItem, _newIndex) =>
            {
                if (_listItem == bracelets)
                {
                    string ActiveItem = _listItem.Items[_newIndex].ToString();
                    _listItem.Description = accessory.Bracelets.OrderBy(x => x.Price).ToList()[_newIndex].Description + ", Prezzo: $" + accessory.Bracelets.OrderBy(x => x.Price).ToList()[_newIndex].Price;
                    if (accessory.Bracelets.OrderBy(x => x.Price).ToList()[_newIndex].Model != -1)
                        SetPedPropIndex(PlayerPedId(), 7, accessory.Bracelets.OrderBy(x => x.Price).ToList()[_newIndex].Model, 0, false);
                    else
                        ClearPedProp(PlayerPedId(), 7);
                    IntEarrAtt = accessory.Bracelets.OrderBy(x => x.Price).ToList()[_newIndex].Model;
                    _menu.UpdateDescription();
                }
            };
            brace.OnItemSelect += (_menu, _listItem, _listIndex) =>
            {
                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracelets == accessory.Bracelets.OrderBy(x => x.Price).ToList()[_listIndex].Model && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracelets == -1)
                {
                    HUD.ShowNotification("Non puoi rimuovere 2 volte un bracciale!!", true);
                }
                else if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracelets == accessory.Bracelets.OrderBy(x => x.Price).ToList()[_listIndex].Model && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracelets != -1)
                {
                    HUD.ShowNotification("Non puoi acquistare di nuovo il braccialetto che hai già!");
                }
                else
                {
                    int val = accessory.Bracelets.OrderBy(x => x.Price).ToList()[_listIndex].Price;

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracelets = accessory.Bracelets.OrderBy(x => x.Price).ToList()[_listIndex].Model;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Bracelets = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        HUD.ShowNotification("you spent ~g~" + val + "$~w~, by cash");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracelets = accessory.Bracelets.OrderBy(x => x.Price).ToList()[_listIndex].Model;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Bracelets = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            HUD.ShowNotification("you spent ~g~" + val + "$~w~, by credit card");
                        }
                        else
                        {
                            HUD.ShowNotification("You don't have enough money for these clothes!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };

            accessoriesMenu.OnMenuClose += async (a) =>
            {
                await BaseScript.Delay(200);

                for (int i = 0; i < SubMenusHats.Count; i++)
                    if (SubMenusHats[i].Visible)
                        return;

                for (int i = 0; i < SubMenusWrist.Count; i++)
                    if (SubMenusWrist[i].Visible)
                        return;

                if (bags.Visible || MenuClotheshops.earr.Visible || brace.Visible || wrist.Visible || watch.Visible || hats.Visible || tempwatch.Visible) return;
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                ClotheShopsMain.Exit();
                accessoriesEnabled = false;
                Client.Instance.RemoveTick(CameraAcc);
            };

            brace.OnMenuOpen += async (a, b) => PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 28422, 0.0f, 0.0f, 0.0f, true);
            bags.OnMenuOpen += async (a, b) =>
            {
                float newheading = Cache.PlayerCache.MyPlayer.Ped.Heading - 180f;
                Cache.PlayerCache.MyPlayer.Ped.Task.AchieveHeading(newheading, 1000);
                PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
            };
            MenuClotheshops.earr.OnMenuOpen += async (a, b) =>
            {
                ClearPedProp(PlayerPedId(), 2);
                PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 19336, 0.0f, 0.0f, 0.0f, true);

                do
                {
                    await BaseScript.Delay(0);
                    fov -= .7f;
                    if (fov < 15f) fov = 15f;
                    ClotheShopsMain.camm.FieldOfView = fov;
                } while (fov > 15f);

                Cache.PlayerCache.MyPlayer.Ped.Task.LookAt(new Vector3(ClotheShopsMain.camm.Position.X + 5f, ClotheShopsMain.camm.Position.Y, ClotheShopsMain.camm.Position.Z));
            };
            watch.OnMenuOpen += async (a, b) =>
            {
                StartAnim("anim@random@shop_clothes@watches", "base");
                PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 60309, 0.0f, 0.0f, 0.0f, true);

                do
                {
                    await BaseScript.Delay(0);
                    fov -= .7f;
                    if (fov < 15f) fov = 15f;
                    ClotheShopsMain.camm.FieldOfView = fov;
                } while (fov > 15f);
            };


            bags.OnMenuClose += async (a) =>
            {
                PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                float newheading = Cache.PlayerCache.MyPlayer.Ped.Heading + 180f;
                Cache.PlayerCache.MyPlayer.Ped.Task.AchieveHeading(newheading, 1000);
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
            };
            MenuClotheshops.earr.OnMenuClose += async (a) =>
            {
                PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);

                do
                {
                    await BaseScript.Delay(0);
                    fov += .7f;
                    if (fov > 45f) fov = 45f;
                    ClotheShopsMain.camm.FieldOfView = fov;
                } while (fov < 45f);

                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                Cache.PlayerCache.MyPlayer.Ped.Task.LookAt(ClotheShopsMain.camm.Position);
            };
            brace.OnMenuClose += async (a) =>
            {
                PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                StartAnim(anim, "try_shirt_base");
            };
            watch.OnMenuClose += async (a) =>
            {
                await BaseScript.Delay(200);

                for (int i = 0; i < SubMenusWrist.Count; i++)
                    if (SubMenusWrist[i].Visible)
                        return;
                PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                StartAnim(anim, "try_shirt_base");
            };
            wrist.OnMenuClose += async (a) =>
            {
                PointCamAtPedBone(ClotheShopsMain.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                await BaseScript.Delay(100);

                for (int i = 0; i < SubMenusWrist.Count; i++)
                    if (SubMenusWrist[i].Visible)
                        return;

                do
                {
                    await BaseScript.Delay(0);
                    fov += .7f;
                    if (fov > 45f) fov = 45f;
                    ClotheShopsMain.camm.FieldOfView = fov;
                } while (fov < 45f);

                StartAnim(anim, "try_shirt_base");
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
            };
            accessoriesMenu.Visible = true;
            Client.Instance.AddTick(CameraAcc);
        }

        #endregion

        private static async Task CameraVest()
        {
            if (_clothesMenu.Any(o => o.Visible))
            {
                if (Input.IsControlPressed(Control.FrontendLt))
                {
                    fov -= .7f;
                    if (fov <= 23f) fov = 23f;
                    ClotheShopsMain.camm.FieldOfView = fov;
                }
                else if (Input.IsControlJustReleased(Control.FrontendLt))
                {
                    do
                    {
                        await BaseScript.Delay(0);
                        fov += .7f;
                        if (fov >= 45f) fov = 45f;
                        ClotheShopsMain.camm.FieldOfView = fov;
                    } while (fov != 23f && !Input.IsControlPressed(Control.FrontendLt));
                }
            }
        }

        private static async Task CameraAcc()
        {
            if (earr.Visible || SubMenusWrist.Any(o => o.Visible))
            {
                if (Input.IsControlPressed(Control.FrontendLt))
                {
                    fov -= .7f;
                    if (fov <= 5.0f) fov = 5.0f;
                    ClotheShopsMain.camm.FieldOfView = fov;
                }
                else if (Input.IsControlJustReleased(Control.FrontendLt))
                {
                    do
                    {
                        await BaseScript.Delay(0);
                        fov += .7f;
                        if (fov >= 15f) fov = 15f;
                        ClotheShopsMain.camm.FieldOfView = fov;
                    } while (fov != 15f && !Input.IsControlPressed(Control.FrontendLt));
                }
            }
            else if (bags.Visible)
            {
                if (Input.IsControlPressed(Control.FrontendLt))
                {
                    fov -= .7f;
                    if (fov <= 23f) fov = 23f;
                    ClotheShopsMain.camm.FieldOfView = fov;
                }
                else if (Input.IsControlJustReleased(Control.FrontendLt))
                {
                    do
                    {
                        await BaseScript.Delay(0);
                        fov += .7f;
                        if (fov >= 45f) fov = 45f;
                        ClotheShopsMain.camm.FieldOfView = fov;
                    } while (fov != 23f && !Input.IsControlPressed(Control.FrontendLt));
                }
            }
            else if (ClotheShopsMain.camm.IsActive)
            {
                if (Input.IsControlPressed(Control.FrontendLt))
                {
                    fov -= 0.7f;
                    if (fov <= 23.0f) fov = 23.0f;
                    ClotheShopsMain.camm.FieldOfView = fov;
                }
                else if (Input.IsControlJustReleased(Control.FrontendLt))
                {
                    do
                    {
                        await BaseScript.Delay(0);
                        fov += 0.7f;
                        if (fov >= 45.0f) fov = 45.0f;
                        ClotheShopsMain.camm.FieldOfView = fov;
                    } while (fov != 45.0f && !Input.IsControlPressed(Control.FrontendLt));
                }
            }

            await Task.FromResult(0);
        }
    }
}