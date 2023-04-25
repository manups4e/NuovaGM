using Impostazioni.Client.Configurazione.Negozi.Abiti;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Negozi
{
    internal static class MenuNegoziAbiti
    {
        private static MenuPool pool = HUD.MenuPool;
        private static bool AccessoriAttivo = false;
        private static List<UIMenu> _menuVestiti = new();

        #region Utili

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
            Cache.PlayerCache.MyPlayer.Ped.Task.StartScenario(anim, Cache.PlayerCache.MyPlayer.Posizione.ToVector3);
        }

        public static async Task UpdateDress(dynamic dress)
        {
            int id = Cache.PlayerCache.MyPlayer.Ped.Handle;
            for (int i = 0; i < 9; i++) ClearPedProp(id, i);
            SetPedComponentVariation(id, (int)DrawableIndexes.Faccia, dress.ComponentDrawables.Faccia, dress.ComponentTextures.Faccia, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Maschera, dress.ComponentDrawables.Maschera, dress.ComponentTextures.Maschera, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Torso, dress.ComponentDrawables.Torso, dress.ComponentTextures.Torso, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Pantaloni, dress.ComponentDrawables.Pantaloni, dress.ComponentTextures.Pantaloni, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Borsa_Paracadute, dress.ComponentDrawables.Borsa_Paracadute, dress.ComponentTextures.Borsa_Paracadute, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Scarpe, dress.ComponentDrawables.Scarpe, dress.ComponentTextures.Scarpe, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Accessori, dress.ComponentDrawables.Accessori, dress.ComponentTextures.Accessori, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Sottomaglia, dress.ComponentDrawables.Sottomaglia, dress.ComponentTextures.Sottomaglia, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Kevlar, dress.ComponentDrawables.Kevlar, dress.ComponentTextures.Kevlar, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Badge, dress.ComponentDrawables.Badge, dress.ComponentTextures.Badge, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Torso_2, dress.ComponentDrawables.Torso_2, dress.ComponentTextures.Torso_2, 2);
            SetPedPropIndex(id, (int)PropIndexes.Cappelli_Maschere, dress.PropIndices.Cappelli_Maschere, dress.PropTextures.Cappelli_Maschere, true);
            SetPedPropIndex(id, (int)PropIndexes.Orecchie, dress.PropIndices.Orecchie, dress.PropTextures.Orecchie, true);
            SetPedPropIndex(id, (int)PropIndexes.Occhiali_Occhi, dress.PropIndices.Occhiali_Occhi, dress.PropTextures.Occhiali_Occhi, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_3, dress.PropIndices.Unk_3, dress.PropTextures.Unk_3, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_4, dress.PropIndices.Unk_4, dress.PropTextures.Unk_4, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_5, dress.PropIndices.Unk_5, dress.PropTextures.Unk_5, true);
            SetPedPropIndex(id, (int)PropIndexes.Orologi, dress.PropIndices.Orologi, dress.PropTextures.Orologi, true);
            SetPedPropIndex(id, (int)PropIndexes.Bracciali, dress.PropIndices.Bracciali, dress.PropTextures.Bracciali, true);
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

        #region MenuVest

        public static async void MenuVest(List<Completo> Completi, string anim, string nome)
        {
            _menuVestiti.Clear();
            UIMenuItem ciao = new("");
            UIMenu MenuVest = new(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
            pool.Add(MenuVest);
            MenuVest.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "try_shirt_base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "try_shirt_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            List<Completo> completi = Completi.OrderBy(x => x.Price).ToList();
            _menuVestiti.Add(MenuVest);

            foreach (Completo t in completi)
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
                vest.SetRightBadge(BadgeIcon.CLOTHING); // cambiare con la collezione di abiti
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
                    HUD.ShowNotification("Possiedi già quest'abito!", true);
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
                        HUD.ShowNotification("Hai speso ~g~" + completi[_index].Price + "$~w~, in contanti");
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
                            HUD.ShowNotification("Hai speso ~g~" + completi[_index].Price + "$~w~, con carta di credito");
                        }
                        else
                        {
                            HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };
            MenuVest.OnMenuStateChanged += async (oldmenu, _menu, state) =>
            {
                if (state != MenuState.Closed) return;
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                NegozioAbitiClient.Esci();
                Client.Instance.RemoveTick(CameraVest);
            };
            MenuVest.Visible = true;
            Client.Instance.AddTick(CameraVest);
        }

        #endregion

        #region PantMenu

        public static async void MenuPant(List<Singolo> Completi, string anim, string nome)
        {
            _menuVestiti.Clear();
            UIMenuItem ciao = new("");
            UIMenuItem ciaone = new("");
            UIMenu MenuPant = new(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
            pool.Add(MenuPant);
            MenuPant.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            _menuVestiti.Add(MenuPant);
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "try_trousers_base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "try_trousers_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            List<Singolo> completi = Completi.OrderBy(x => x.Price).ToList();
            int money = 0;
            int mod = 0;
            int text = 0;

            foreach (Singolo v in completi)
            {
                UIMenu Pant = pool.AddSubMenu(MenuPant, v.Title, v.Description);
                Pant.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                _menuVestiti.Add(Pant);

                foreach (int texture in v.Text)
                {
                    UIMenuItem pant = new("Modello " + v.Text.IndexOf(texture));
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

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Pantaloni != v.Modello || Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Pantaloni != texture) continue;
                    Pant.ParentItem.SetRightBadge(BadgeIcon.CLOTHING);
                    ciao = Pant.ParentItem;
                    pant.SetRightBadge(BadgeIcon.CLOTHING); // cambiare con la collezione di abiti
                    ciaone = pant;
                }

                Pant.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
                {
                    switch (state)
                    {
                        case MenuState.ChangeForward:
                            {
                                SetPedComponentVariation(PlayerPedId(), 4, v.Modello, v.Text[0], 2);
                                string random = GetRandomAnim(anim, false);
                                SetPedComponentVariation(PlayerPedId(), 4, v.Modello, v.Text[0], 2);
                                await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                                mod = v.Modello;
                                text = v.Text[0];

                                break;
                            }
                        case MenuState.ChangeBackward:
                            await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);

                            break;
                    }
                };
                Pant.OnIndexChange += async (sender, index) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedComponentVariation(PlayerPedId(), 4, v.Modello, v.Text[index], 2);
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                    mod = v.Modello;
                    text = v.Text[index];
                };
                Pant.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Pantaloni == mod && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Pantaloni == text)
                    {
                        HUD.ShowNotification("Hai già acquistato questo pantalone!!", true);

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
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Pantaloni = mod;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Pantaloni = text;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        ciao.SetRightBadge(BadgeIcon.NONE);
                        ciao = MenuPant.MenuItems[MenuPant.CurrentSelection];
                        ciao.SetRightBadge(BadgeIcon.CLOTHING);
                        ciaone.SetRightBadge(BadgeIcon.NONE);
                        ciaone = _item;
                        ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Pantaloni = mod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Pantaloni = text;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            ciao.SetRightBadge(BadgeIcon.NONE);
                            ciao = MenuPant.MenuItems[MenuPant.CurrentSelection];
                            ciao.SetRightBadge(BadgeIcon.CLOTHING);
                            ciaone.SetRightBadge(BadgeIcon.NONE);
                            ciaone = _item;
                            ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                        }
                        else
                        {
                            HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            MenuPant.OnMenuStateChanged += async (oldmenu, _menu, state) =>
            {
                if (state != MenuState.Closed) return;
                await BaseScript.Delay(100);

                if (_menuVestiti.Any(t => t.Visible)) return;
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                NegozioAbitiClient.Esci();
                Client.Instance.RemoveTick(CameraVest);
            };
            MenuPant.Visible = true;
            Client.Instance.AddTick(CameraVest);
        }

        #endregion

        #region ScarpeMenu

        public static async void MenuScarpe(List<Singolo> Completi, string anim, string nome)
        {
            _menuVestiti.Clear();
            UIMenuItem ciao = new("");
            UIMenuItem ciaone = new("");
            UIMenu MenuScarpe = new(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
            pool.Add(MenuScarpe);
            _menuVestiti.Add(MenuScarpe);
            MenuScarpe.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "try_shoes_base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "try_shoes_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            List<Singolo> completi = Completi.OrderBy(x => x.Price).ToList();
            int money = 0;
            int mod = 0;
            int text = 0;

            foreach (Singolo v in completi)
            {
                UIMenu Scarp = pool.AddSubMenu(MenuScarpe, v.Title, v.Description);
                Scarp.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                _menuVestiti.Add(Scarp);

                foreach (int texture in v.Text)
                {
                    UIMenuItem pant = new("Modello " + v.Text.IndexOf(texture));
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

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Scarpe != v.Modello || Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Scarpe != texture) continue;
                    Scarp.ParentItem.SetRightBadge(BadgeIcon.CLOTHING);
                    ciao = Scarp.ParentItem;
                    pant.SetRightBadge(BadgeIcon.CLOTHING); // cambiare con la collezione di abiti
                    ciaone = pant;
                }

                Scarp.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
                {
                    switch (state)
                    {
                        case MenuState.ChangeForward:
                            {
                                string random = GetRandomAnim(anim, false);
                                SetPedComponentVariation(PlayerPedId(), 6, v.Modello, v.Text[0], 2);
                                await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                                mod = v.Modello;
                                text = v.Text[0];

                                break;
                            }
                        case MenuState.ChangeBackward:
                            await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);

                            break;
                    }
                };
                Scarp.OnIndexChange += async (sender, index) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedComponentVariation(PlayerPedId(), 6, v.Modello, v.Text[index], 2);
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                    mod = v.Modello;
                    text = v.Text[index];
                };
                Scarp.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Scarpe == mod && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Scarpe == text)
                    {
                        HUD.ShowNotification("Hai già acquistato queste scarpe!!", true);

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
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Scarpe = mod;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Scarpe = text;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        ciao.SetRightBadge(BadgeIcon.NONE);
                        ciao = MenuScarpe.MenuItems[MenuScarpe.CurrentSelection];
                        ciao.SetRightBadge(BadgeIcon.CLOTHING);
                        ciaone.SetRightBadge(BadgeIcon.NONE);
                        ciaone = _item;
                        ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Scarpe = mod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Scarpe = text;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            ciao.SetRightBadge(BadgeIcon.NONE);
                            ciao = MenuScarpe.MenuItems[MenuScarpe.CurrentSelection];
                            ciao.SetRightBadge(BadgeIcon.CLOTHING);
                            ciaone.SetRightBadge(BadgeIcon.NONE);
                            ciaone = _item;
                            ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                        }
                        else
                        {
                            HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            MenuScarpe.OnMenuStateChanged += async (oldmenu, _menu, state) =>
            {
                if (state != MenuState.Closed) return;
                await BaseScript.Delay(100);

                foreach (UIMenu t in _menuVestiti)
                    if (t.Visible)
                        return;
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                NegozioAbitiClient.Esci();
                Client.Instance.RemoveTick(CameraVest);
            };
            MenuScarpe.Visible = true;
            Client.Instance.AddTick(CameraVest);
        }

        #endregion

        #region OcchialiMenu

        public static async void MenuOcchiali(List<Singolo> Completi, string anim, string nome)
        {
            _menuVestiti.Clear();
            UIMenuItem ciao = new("");
            UIMenuItem ciaone = new("");
            UIMenu MenuOcchiali = new(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
            pool.Add(MenuOcchiali);
            _menuVestiti.Add(MenuOcchiali);
            MenuOcchiali.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "Try_Glasses_Base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "Try_Glasses_Base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            List<Singolo> completi = Completi.OrderBy(x => x.Price).ToList();
            int money = 0;
            int mod = 0;
            int text = 0;

            foreach (Singolo v in completi)
            {
                UIMenu Scarp = pool.AddSubMenu(MenuOcchiali, v.Title, v.Description);
                Scarp.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                _menuVestiti.Add(Scarp);

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

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie != v.Modello || Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orecchie != texture) continue;
                    Scarp.ParentItem.SetRightBadge(BadgeIcon.CLOTHING);
                    ciao = Scarp.ParentItem;
                    pant.SetRightBadge(BadgeIcon.CLOTHING); // cambiare con la collezione di abiti
                    ciaone = pant;
                }

                Scarp.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
                {
                    switch (state)
                    {
                        case MenuState.ChangeForward:
                            {
                                string random = GetRandomAnim(anim, false);
                                SetPedPropIndex(PlayerPedId(), 1, v.Modello, v.Text[0], false);
                                await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                                mod = v.Modello;
                                text = v.Text[0];

                                break;
                            }
                        case MenuState.ChangeBackward:
                            await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);

                            break;
                    }
                };
                Scarp.OnIndexChange += async (menu, index) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedPropIndex(PlayerPedId(), 1, v.Modello, v.Text[index], false);
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                    mod = v.Modello;
                    text = v.Text[index];
                };
                Scarp.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie == mod && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orecchie == text)
                    {
                        HUD.ShowNotification("Hai già acquistato questi occhiali!!", true);

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
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie = mod;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orecchie = text;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        ciao.SetRightBadge(BadgeIcon.NONE);
                        ciao = MenuOcchiali.MenuItems[MenuOcchiali.CurrentSelection];
                        ciao.SetRightBadge(BadgeIcon.CLOTHING);
                        ciaone.SetRightBadge(BadgeIcon.NONE);
                        ciaone = _item;
                        ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie = mod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orecchie = text;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            ciao.SetRightBadge(BadgeIcon.NONE);
                            ciao = MenuOcchiali.MenuItems[MenuOcchiali.CurrentSelection];
                            ciao.SetRightBadge(BadgeIcon.CLOTHING);
                            ciaone.SetRightBadge(BadgeIcon.NONE);
                            ciaone = _item;
                            ciaone.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                        }
                        else
                        {
                            HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                        }
                    }
                };
            }

            MenuOcchiali.OnMenuStateChanged += async (oldmenu, _menu, state) =>
            {
                if (state != MenuState.Closed) return;
                await BaseScript.Delay(100);

                foreach (UIMenu t in _menuVestiti)
                    if (t.Visible)
                        return;
                await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                NegozioAbitiClient.Esci();
                Client.Instance.RemoveTick(CameraVest);
            };
            MenuOcchiali.Visible = true;
            Client.Instance.AddTick(CameraVest);
        }

        #endregion

        #region AccessoriMenu

        private static UIMenu Borse = new(" ", "Borse");
        private static UIMenu Orecc = new(" ", "Orecchini e Auricolari");
        private static UIMenu Capp = new(" ", "Orologi");
        private static UIMenu Polso = new(" ", "Orologi e Braccialetti");
        private static UIMenu Orol = new(" ", "Orologi");
        private static UIMenu Brac = new(" ", "Orologi");
        private static UIMenu Orologino = new(" ", " ");
        private static List<UIMenu> SubMenusCapp = new();
        private static List<UIMenu> SubMenusPolso = new();

        private static float fov = 0;

        public static async void MenuAccessori(Accessori Accessorio, string anim, string nome)
        {
            AccessoriAttivo = true;
            UIMenuItem orecAtt = new("");
            UIMenuItem coloOreccAtt = new("");
            UIMenuItem OrolAtt = new("");
            UIMenuItem OrolMod = new("");
            UIMenuItem BraccAtt = new("");
            UIMenuItem CappAtt = new("");
            UIMenuItem CappAttMod = new("");
            UIMenuItem BorsAtt = new("");
            UIMenuItem CappRim = new("");
            UIMenu MenuAccessori = new(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
            pool.Add(MenuAccessori);
            MenuAccessori.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            SetPedAlternateMovementAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, 0, anim, "try_shirt_base", 4.0f, true);
            await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, "try_shirt_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
            int money = 0;
            int IntorecAtt = Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.style;
            int IntcoloOreccAtt = Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.color;
            int IntOrolAtt = GetPedPropIndex(PlayerPedId(), 6);
            int IntOrolMod = GetPedPropTextureIndex(PlayerPedId(), 6);
            int IntBraccAtt = GetPedPropIndex(PlayerPedId(), 7);
            int IntCappAtt = GetPedPropIndex(PlayerPedId(), 0);
            int IntCappAttMod = GetPedPropTextureIndex(PlayerPedId(), 0);
            int IntBors = GetPedDrawableVariation(PlayerPedId(), 5);
            int mod = 0;
            int text = 0;

            #region sottomenu

            Borse = pool.AddSubMenu(MenuAccessori, "Borse", "Scegli qui tra le borse disponibili!");
            Borse.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Capp = pool.AddSubMenu(MenuAccessori, "Cappellini", "Scegli qui i tuoi accessori preferiti!");
            Capp.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Orecc = pool.AddSubMenu(MenuAccessori, "Orecchini e Auricolari", "Scegli qui i tuoi pendagli preferiti!");
            Orecc.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Polso = pool.AddSubMenu(MenuAccessori, "Orologi e Braccialetti", "Scegli qui tra orologi e braccialetti!");
            Polso.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Orol = pool.AddSubMenu(Polso, "Orologi", "Scegli qui i tuoi orologi!");
            Orol.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            Brac = pool.AddSubMenu(Polso, "Braccialetti", "Scegli qui i tuoi braccialetti!");
            Brac.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
            SubMenusPolso.Add(Orol);
            SubMenusPolso.Add(Brac);

            #endregion

            List<dynamic> ore = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList().Select(x => x.Title).ToList().Cast<dynamic>().ToList();
            List<dynamic> bra = Accessorio.Bracciali.OrderBy(x => x.Price).ToList().Select(x => x.Title).ToList().Cast<dynamic>().ToList();
            UIMenuListItem orecchini = new("Orecchini", ore, 0, "Scegli qui i tuoi orecchini preferiti");
            Orecc.AddItem(orecchini);
            UIMenuListItem braccialetti = new("Braccialetti", bra, 0, "Scegli qui il tuo bracciale preferito");
            Brac.AddItem(braccialetti);

            foreach (Singolo borsa in Accessorio.Borse)
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

                Borse.AddItem(bors);

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Borsa_Paracadute != borsa.Modello) continue;
                bors.SetRightBadge(BadgeIcon.CLOTHING);
                BorsAtt = bors;
            }

            Borse.OnIndexChange += async (_menu, index) =>
            {
                IntBors = Accessorio.Borse[index].Modello;
                string random = GetRandomAnim(anim, false);
                SetPedComponentVariation(PlayerPedId(), 5, IntBors, 0, 2);
                await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
            };
            Borse.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Borsa_Paracadute == IntBors)
                {
                    HUD.ShowNotification("Hai già acquistato questa borsa!!", true);

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
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Borsa_Paracadute = IntBors;
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Borsa_Paracadute = 0;
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                    Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                    BorsAtt.SetRightBadge(BadgeIcon.NONE);
                    BorsAtt = _menu.MenuItems[_menu.CurrentSelection];
                    BorsAtt.SetRightBadge(BadgeIcon.CLOTHING);
                    HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                }
                else
                {
                    if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentDrawables.Borsa_Paracadute = IntBors;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.ComponentTextures.Borsa_Paracadute = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        BorsAtt.SetRightBadge(BadgeIcon.NONE);
                        BorsAtt = _menu.MenuItems[_menu.CurrentSelection];
                        BorsAtt.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                    }
                    else
                    {
                        HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                    }
                }
            };
            UIMenuItem CappAtt1 = new("");
            UIMenuItem newCap = new("");
            UIMenuItem capelino = new("");
            UIMenu Capelino = new("", "");

            foreach (Singolo cappellino in Accessorio.Testa.Cappellini)
            {
                if (Accessorio.Testa.Cappellini.IndexOf(cappellino) != 0)
                {
                    Capelino = pool.AddSubMenu(Capp, cappellino.Title + " " + Accessorio.Testa.Cappellini.IndexOf(cappellino), cappellino.Description);
                    Capelino.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                    SubMenusCapp.Add(Capelino);
                }

                if (cappellino.Modello == -1)
                {
                    CappRim = new UIMenuItem(cappellino.Title, cappellino.Description);
                    Capp.AddItem(CappRim);
                    CappRim.SetRightLabel("~g~$0");
                }

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere == -1)
                {
                    CappRim.SetRightBadge(BadgeIcon.CLOTHING);
                    CappAtt = CappRim;
                }

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere != -1 && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere == cappellino.Modello)
                {
                    capelino.SetRightBadge(BadgeIcon.CLOTHING);
                    CappAtt = CappRim;
                }

                foreach (int texture in cappellino.Text)
                {
                    newCap = new UIMenuItem("Modello " + cappellino.Text.IndexOf(texture), cappellino.Description);
                    Capelino.AddItem(newCap);

                    switch (nome)
                    {
                        case "Binco":
                            money = cappellino.Price + cappellino.Text.IndexOf(texture) * 47;

                            break;
                        case "Discount":
                            money = cappellino.Price + cappellino.Text.IndexOf(texture) * 62;

                            break;
                        case "Suburban":
                            money = cappellino.Price + cappellino.Text.IndexOf(texture) * 75;

                            break;
                        case "Ponsombys":
                            money = cappellino.Price + cappellino.Text.IndexOf(texture) * 89;

                            break;
                    }

                    if (Cache.PlayerCache.MyPlayer.User.Money >= money)
                    {
                        newCap.SetRightLabel("~g~$" + money);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= money)
                            newCap.SetRightLabel("~g~$" + money);
                        else
                            newCap.SetRightLabel("~r~$" + money);
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere != cappellino.Modello || Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Cappelli_Maschere != texture) continue;
                    newCap.SetRightBadge(BadgeIcon.CLOTHING);
                    CappAtt1 = newCap;
                }

                Capelino.OnIndexChange += async (_menu, _newIndex) =>
                {
                    string random = GetRandomAnim(anim, false);
                    SetPedPropIndex(PlayerPedId(), 0, cappellino.Modello, cappellino.Text[_newIndex], false);
                    IntCappAtt = cappellino.Text[_newIndex];
                    IntCappAttMod = cappellino.Text[_newIndex];
                    await Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
                };
                Capelino.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere == IntCappAtt && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Cappelli_Maschere == IntCappAttMod)
                    {
                        HUD.ShowNotification("Non puoi acquistare lo stesso cappello che hai già! Prova a cambiare modello!");
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
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere = IntCappAtt;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Cappelli_Maschere = IntCappAttMod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            CappAtt.SetRightBadge(BadgeIcon.NONE);
                            CappAtt = Capp.MenuItems[Capp.CurrentSelection];
                            CappAtt.SetRightBadge(BadgeIcon.CLOTHING);
                            CappAtt1.SetRightBadge(BadgeIcon.NONE);
                            CappAtt1 = _menu.MenuItems[_menu.CurrentSelection];
                            CappAtt1.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                            {
                                BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere = IntCappAtt;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Cappelli_Maschere = IntCappAttMod;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                                CappAtt.SetRightBadge(BadgeIcon.NONE);
                                CappAtt = Capp.MenuItems[Capp.CurrentSelection];
                                CappAtt.SetRightBadge(BadgeIcon.CLOTHING);
                                CappAtt1.SetRightBadge(BadgeIcon.NONE);
                                CappAtt1 = _menu.MenuItems[_menu.CurrentSelection];
                                CappAtt1.SetRightBadge(BadgeIcon.CLOTHING);
                                HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                            }
                            else
                            {
                                HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                            }
                        }
                    }
                };
                Capelino.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
                {
                    if (state != MenuState.ChangeBackward || newmenu != Capp) return;
                    PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                    await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                };
            }

            Capp.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item != CappRim) return;

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere == -1)
                {
                    HUD.ShowNotification("Non puoi rimuovere 2 volte un Cappello!!", true);
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
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere = -1;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Cappelli_Maschere = -1;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        CappAtt.SetRightBadge(BadgeIcon.NONE);
                        CappAtt = _menu.MenuItems[_menu.CurrentSelection];
                        CappAtt.SetRightBadge(BadgeIcon.CLOTHING);
                        HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Cappelli_Maschere = -1;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Cappelli_Maschere = -1;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            CappAtt.SetRightBadge(BadgeIcon.NONE);
                            CappAtt = _menu.MenuItems[_menu.CurrentSelection];
                            CappAtt.SetRightBadge(BadgeIcon.CLOTHING);
                            HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                        }
                        else
                        {
                            HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };
            Orecc.OnListChange += (_menu, _listItem, _newIndex) =>
            {
                string ActiveItem = _listItem.Items[_newIndex].ToString();

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie == Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Modello)
                {
                    //					_listItem.SetRightBadge(BadgeIcon.CLOTHING);
                }
                else
                {
                    //					_listItem.SetRightBadge(BadgeIcon.NONE);
                }

                _listItem.Description = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Description + ", Prezzo: $" + Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Price;
                SetPedPropIndex(PlayerPedId(), 2, Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Modello, 0, false);
                IntorecAtt = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Modello;
                _menu.UpdateDescription();
            };
            Orecc.OnItemSelect += (_menu, _listItem, _listIndex) =>
            {
                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie == Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie == -1)
                {
                    HUD.ShowNotification("Non puoi rimuovere 2 volte gli orecchini!!", true);
                }
                else if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie == Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie != -1)
                {
                    HUD.ShowNotification("Non puoi acquistare di nuovo gli orecchini che hai già!");
                }
                else
                {
                    int val = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Price;

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orecchie = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.style = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.color = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orecchie = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.style = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.color = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                        }
                        else
                        {
                            HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };

            foreach (Singolo aurico in Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList())
            {
                UIMenuItem auricolari = new(aurico.Title, aurico.Description);

                if (Cache.PlayerCache.MyPlayer.User.Money >= aurico.Price)
                {
                    auricolari.SetRightLabel("~g~$" + aurico.Price);
                }
                else
                {
                    if (Cache.PlayerCache.MyPlayer.User.Bank >= aurico.Price)
                        auricolari.SetRightLabel("~g~$" + aurico.Price);
                    else
                        auricolari.SetRightLabel("~r~$" + aurico.Price);
                }

                Orecc.AddItem(auricolari);
            }

            Orecc.OnIndexChange += async (_menu, _newIndex) =>
            {
                if (_newIndex == 0) return;
                if (Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_newIndex - 1].Modello == -1)
                    ClearPedProp(PlayerPedId(), 2);
                else
                    SetPedPropIndex(PlayerPedId(), 2, Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_newIndex - 1].Modello, 0, false);
            };
            Orecc.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (_index == 0) return;

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie == Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie == -1)
                {
                    HUD.ShowNotification("Non puoi rimuovere 2 volte l'auricolare!!", true);
                }
                else if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie == Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie != -1)
                {
                    HUD.ShowNotification("Non puoi acquistare l'auricolare che hai già!");
                }
                else
                {
                    int val = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_index - 1].Price;

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orecchie = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.style = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.color = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orecchie = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orecchie = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.style = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.color = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                        }
                        else
                        {
                            HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };
            UIMenuItem OrologinoItem = new("");
            UIMenuItem NoOrol = new("");
            UIMenuItem NewOrol = new("");

            foreach (Singolo orologio in Accessorio.Orologi)
            {
                if (Accessorio.Orologi.IndexOf(orologio) != 0)
                {
                    Orologino = pool.AddSubMenu(Orol, orologio.Title, orologio.Description);
                    Orologino.InstructionalButtons.Add(new InstructionalButton(Control.FrontendLt, "Zoom"));
                    SubMenusPolso.Add(Orologino);
                }

                if (orologio.Modello == -1)
                {
                    NoOrol = new UIMenuItem(orologio.Title, orologio.Description);
                    Orol.AddItem(NoOrol);

                    if (Cache.PlayerCache.MyPlayer.User.Money >= orologio.Price)
                    {
                        NoOrol.SetRightLabel("~g~$" + orologio.Price);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= orologio.Price)
                            NoOrol.SetRightLabel("~g~$" + orologio.Price);
                        else
                            NoOrol.SetRightLabel("~r~$" + orologio.Price);
                    }
                }

                foreach (int v in orologio.Text)
                {
                    NewOrol = new UIMenuItem("Modello " + orologio.Text.IndexOf(v), orologio.Description);
                    Orologino.AddItem(NewOrol);

                    switch (nome)
                    {
                        case "Binco":
                            money = orologio.Price + orologio.Text.IndexOf(v) * 47;

                            break;
                        case "Discount":
                            money = orologio.Price + orologio.Text.IndexOf(v) * 62;

                            break;
                        case "Suburban":
                            money = orologio.Price + orologio.Text.IndexOf(v) * 75;

                            break;
                        case "Ponsombys":
                            money = orologio.Price + orologio.Text.IndexOf(v) * 89;

                            break;
                    }

                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orologi == orologio.Modello && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orologi == v)
                    {
                        if (orologio.Modello == -1)
                        {
                            NoOrol.SetRightBadge(BadgeIcon.CLOTHING);
                            OrolAtt = NoOrol;
                        }
                        else
                        {
                            OrologinoItem.SetRightBadge(BadgeIcon.CLOTHING);
                            OrolAtt = OrologinoItem;
                        }

                        NewOrol.SetRightBadge(BadgeIcon.CLOTHING);
                        OrolMod = NewOrol;
                    }

                    if (Cache.PlayerCache.MyPlayer.User.Money >= money)
                    {
                        NewOrol.SetRightLabel("~g~$" + money);
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= money)
                            NewOrol.SetRightLabel("~g~$" + money);
                        else
                            NewOrol.SetRightLabel("~r~$" + money);
                    }

                    Orologino.OnMenuStateChanged += (oldmenu, newmenu, state) =>
                    {
                        if (state == MenuState.ChangeForward)
                        {
                            SetPedPropIndex(PlayerPedId(), 6, orologio.Modello, 0, false);
                            IntOrolAtt = orologio.Modello;
                            IntOrolMod = 0;
                        }
                    };
                    Orologino.OnIndexChange += async (_menu, _newIndex) =>
                    {
                        SetPedPropIndex(PlayerPedId(), 6, orologio.Modello, orologio.Text[_newIndex], false);
                        IntOrolAtt = orologio.Modello;
                        IntOrolMod = orologio.Text[_newIndex];
                    };
                }

                Orologino.OnItemSelect += async (_menu, _item, _index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orologi == IntOrolAtt && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orologi == IntOrolMod)
                    {
                        HUD.ShowNotification("Non puoi acquistare lo stesso orologio che hai già! Prova a cambiare modello!", true);
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
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orologi = IntOrolAtt;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orologi = IntOrolMod;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            OrolAtt.SetRightBadge(BadgeIcon.NONE);
                            OrolAtt = Orol.MenuItems[Orol.CurrentSelection];
                            OrolAtt.SetRightBadge(BadgeIcon.CLOTHING);
                            OrolMod.SetRightBadge(BadgeIcon.NONE);
                            OrolMod = _menu.MenuItems[_menu.CurrentSelection];
                            OrolMod.SetRightBadge(BadgeIcon.CLOTHING);
                            if (val > 0) HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                        }
                        else
                        {
                            if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                            {
                                BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orologi = IntOrolAtt;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orologi = IntOrolMod;
                                OrolAtt.SetRightBadge(BadgeIcon.NONE);
                                OrolAtt = Orol.MenuItems[Orol.CurrentSelection];
                                OrolAtt.SetRightBadge(BadgeIcon.CLOTHING);
                                OrolMod.SetRightBadge(BadgeIcon.NONE);
                                OrolMod = _menu.MenuItems[_menu.CurrentSelection];
                                OrolMod.SetRightBadge(BadgeIcon.CLOTHING);
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                                Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                                if (val > 0) HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                            }
                            else
                            {
                                HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                            }
                        }
                    }
                };
            }

            Orol.OnIndexChange += async (_menu, _newIndex) =>
            {
                if (_menu.MenuItems[_newIndex] == NoOrol) ClearPedProp(PlayerPedId(), 6);
            };
            Orol.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (_item != NoOrol) return;

                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orologi == -1 && IntOrolAtt == -1)
                {
                    HUD.ShowNotification("Non puoi rimuovere 2 volte un orologio!!", true);
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
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orologi = -1;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orologi = -1;
                        OrolAtt.SetRightBadge(BadgeIcon.NONE);
                        OrolAtt = _menu.MenuItems[_menu.CurrentSelection];
                        OrolAtt.SetRightBadge(BadgeIcon.CLOTHING);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.ears.style = Accessorio.Orologi.OrderBy(x => x.Price).ToList()[_index].Modello;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        if (val > 0) HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Orologi = -1;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Orologi = -1;
                            OrolAtt.SetRightBadge(BadgeIcon.NONE);
                            OrolAtt = _menu.MenuItems[_menu.CurrentSelection];
                            OrolAtt.SetRightBadge(BadgeIcon.CLOTHING);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            if (val > 0) HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                        }
                        else
                        {
                            HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };
            Brac.OnListChange += (_menu, _listItem, _newIndex) =>
            {
                if (_listItem == braccialetti)
                {
                    string ActiveItem = _listItem.Items[_newIndex].ToString();
                    _listItem.Description = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Description + ", Prezzo: $" + Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Price;
                    if (Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Modello != -1)
                        SetPedPropIndex(PlayerPedId(), 7, Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Modello, 0, false);
                    else
                        ClearPedProp(PlayerPedId(), 7);
                    IntorecAtt = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Modello;
                    _menu.UpdateDescription();
                }
            };
            Brac.OnItemSelect += (_menu, _listItem, _listIndex) =>
            {
                if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracciali == Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracciali == -1)
                {
                    HUD.ShowNotification("Non puoi rimuovere 2 volte un bracciale!!", true);
                }
                else if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracciali == Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracciali != -1)
                {
                    HUD.ShowNotification("Non puoi acquistare di nuovo il braccialetto che hai già!");
                }
                else
                {
                    int val = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Price;

                    if (Cache.PlayerCache.MyPlayer.User.Money >= val)
                    {
                        BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracciali = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Bracciali = 0;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                        Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                        HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
                    }
                    else
                    {
                        if (Cache.PlayerCache.MyPlayer.User.Bank >= val)
                        {
                            BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropIndices.Bracciali = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.PropTextures.Bracciali = 0;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Name = null;
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing.Description = null;
                            HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
                        }
                        else
                        {
                            HUD.ShowNotification("Non hai i soldi per questo abito!", ColoreNotifica.Red, true);
                        }
                    }
                }
            };
            HUD.MenuPool.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
            {
                if (state == MenuState.Closed && oldmenu == MenuAccessori)
                {
                    await BaseScript.Delay(200);

                    for (int i = 0; i < SubMenusCapp.Count; i++)
                        if (SubMenusCapp[i].Visible)
                            return;

                    for (int i = 0; i < SubMenusPolso.Count; i++)
                        if (SubMenusPolso[i].Visible)
                            return;

                    if (Borse.Visible || Orecc.Visible || Brac.Visible || Polso.Visible || Orol.Visible || Capp.Visible || Orologino.Visible) return;
                    await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                    NegozioAbitiClient.Esci();
                    AccessoriAttivo = false;
                    Client.Instance.RemoveTick(CameraAcc);
                }
                else if (state == MenuState.ChangeForward)
                {
                    if (newmenu == Brac)
                    {
                        PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 28422, 0.0f, 0.0f, 0.0f, true);
                    }
                    else if (newmenu == Borse)
                    {
                        float newheading = Cache.PlayerCache.MyPlayer.Ped.Heading - 180f;
                        Cache.PlayerCache.MyPlayer.Ped.Task.AchieveHeading(newheading, 1000);
                        PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                    }
                    else if (newmenu == Orecc)
                    {
                        ClearPedProp(PlayerPedId(), 2);
                        PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 19336, 0.0f, 0.0f, 0.0f, true);

                        do
                        {
                            await BaseScript.Delay(0);
                            fov -= .7f;
                            if (fov < 15f) fov = 15f;
                            NegozioAbitiClient.camm.FieldOfView = fov;
                        } while (fov > 15f);

                        Cache.PlayerCache.MyPlayer.Ped.Task.LookAt(new Vector3(NegozioAbitiClient.camm.Position.X + 5f, NegozioAbitiClient.camm.Position.Y, NegozioAbitiClient.camm.Position.Z));
                    }
                    else if (newmenu == Orol)
                    {
                        StartAnim("anim@random@shop_clothes@watches", "base");
                        PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 60309, 0.0f, 0.0f, 0.0f, true);

                        do
                        {
                            await BaseScript.Delay(0);
                            fov -= .7f;
                            if (fov < 15f) fov = 15f;
                            NegozioAbitiClient.camm.FieldOfView = fov;
                        } while (fov > 15f);
                    }
                }
                else if (state == MenuState.ChangeBackward)
                {
                    if (oldmenu == Borse)
                    {
                        PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                        float newheading = Cache.PlayerCache.MyPlayer.Ped.Heading + 180f;
                        Cache.PlayerCache.MyPlayer.Ped.Task.AchieveHeading(newheading, 1000);
                        await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                    }
                    else if (oldmenu == Orecc)
                    {
                        PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);

                        do
                        {
                            await BaseScript.Delay(0);
                            fov += .7f;
                            if (fov > 45f) fov = 45f;
                            NegozioAbitiClient.camm.FieldOfView = fov;
                        } while (fov < 45f);

                        await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                        Cache.PlayerCache.MyPlayer.Ped.Task.LookAt(NegozioAbitiClient.camm.Position);
                    }
                    else if (oldmenu == Brac)
                    {
                        PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                        await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                        StartAnim(anim, "try_shirt_base");
                    }
                    else if (oldmenu == Orol)
                    {
                        await BaseScript.Delay(200);

                        for (int i = 0; i < SubMenusPolso.Count; i++)
                            if (SubMenusPolso[i].Visible)
                                return;
                        PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                        await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                        StartAnim(anim, "try_shirt_base");
                    }
                    else if (oldmenu == Polso)
                    {
                        PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
                        await BaseScript.Delay(100);

                        for (int i = 0; i < SubMenusPolso.Count; i++)
                            if (SubMenusPolso[i].Visible)
                                return;

                        do
                        {
                            await BaseScript.Delay(0);
                            fov += .7f;
                            if (fov > 45f) fov = 45f;
                            NegozioAbitiClient.camm.FieldOfView = fov;
                        } while (fov < 45f);

                        StartAnim(anim, "try_shirt_base");
                        await UpdateDress(Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                    }
                }
            };
            MenuAccessori.Visible = true;
            Client.Instance.AddTick(CameraAcc);
        }

        #endregion

        private static async Task CameraVest()
        {
            if (_menuVestiti.Any(o => o.Visible))
            {
                if (Input.IsControlPressed(Control.FrontendLt))
                {
                    fov -= .7f;
                    if (fov <= 23f) fov = 23f;
                    NegozioAbitiClient.camm.FieldOfView = fov;
                }
                else if (Input.IsControlJustReleased(Control.FrontendLt))
                {
                    do
                    {
                        await BaseScript.Delay(0);
                        fov += .7f;
                        if (fov >= 45f) fov = 45f;
                        NegozioAbitiClient.camm.FieldOfView = fov;
                    } while (fov != 23f && !Input.IsControlPressed(Control.FrontendLt));
                }
            }
        }

        private static async Task CameraAcc()
        {
            if (Orecc.Visible || SubMenusPolso.Any(o => o.Visible))
            {
                if (Input.IsControlPressed(Control.FrontendLt))
                {
                    fov -= .7f;
                    if (fov <= 5.0f) fov = 5.0f;
                    NegozioAbitiClient.camm.FieldOfView = fov;
                }
                else if (Input.IsControlJustReleased(Control.FrontendLt))
                {
                    do
                    {
                        await BaseScript.Delay(0);
                        fov += .7f;
                        if (fov >= 15f) fov = 15f;
                        NegozioAbitiClient.camm.FieldOfView = fov;
                    } while (fov != 15f && !Input.IsControlPressed(Control.FrontendLt));
                }
            }
            else if (Borse.Visible)
            {
                if (Input.IsControlPressed(Control.FrontendLt))
                {
                    fov -= .7f;
                    if (fov <= 23f) fov = 23f;
                    NegozioAbitiClient.camm.FieldOfView = fov;
                }
                else if (Input.IsControlJustReleased(Control.FrontendLt))
                {
                    do
                    {
                        await BaseScript.Delay(0);
                        fov += .7f;
                        if (fov >= 45f) fov = 45f;
                        NegozioAbitiClient.camm.FieldOfView = fov;
                    } while (fov != 23f && !Input.IsControlPressed(Control.FrontendLt));
                }
            }
            else if (NegozioAbitiClient.camm.IsActive)
            {
                if (Input.IsControlPressed(Control.FrontendLt))
                {
                    fov -= 0.7f;
                    if (fov <= 23.0f) fov = 23.0f;
                    NegozioAbitiClient.camm.FieldOfView = fov;
                }
                else if (Input.IsControlJustReleased(Control.FrontendLt))
                {
                    do
                    {
                        await BaseScript.Delay(0);
                        fov += 0.7f;
                        if (fov >= 45.0f) fov = 45.0f;
                        NegozioAbitiClient.camm.FieldOfView = fov;
                    } while (fov != 45.0f && !Input.IsControlPressed(Control.FrontendLt));
                }
            }

            await Task.FromResult(0);
        }
    }
}