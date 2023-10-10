using System;
using System.Linq;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Interactions
{
    internal static class LootingDead
    {
        private static int _checkTimer;

        public static void Init()
        {
            // TODO: Everyone loots or only who killed me?
        }

        public static async Task Looting()
        {
            // Remove weight from data
            //Ped playerPed = Cache.PlayerPed;

            Tuple<Player, float> closest = new(new Player(0), -1);
            if (Game.GameTime - _checkTimer > 1000)
            {
                closest = Functions.GetClosestPlayer();
                _checkTimer = Game.GameTime;
            }

            if (!Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty)
            {
                if (closest.Item2 > -1 && closest.Item2 < 3f)
                {
                    PlayerClient client = Functions.GetPlayerClientFromServerId(closest.Item1.ServerId);
                    if (client.Status.RolePlayStates.Fainted || client.Status.RolePlayStates.Dying)
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to Loot");
                        if (Input.IsControlJustPressed(Control.Context)) LootMenu(closest.Item1);
                    }
                }
            }

            await Task.FromResult(0);
        }

        private static async void LootMenu(Player target)
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;
            User targetData = target.GetPlayerData();
            UIMenu loot = new UIMenu(targetData.FullName, "Looting Menu");
            UIMenuItem walletItem = new("Wallet");
            UIMenu wallet = new("Wallet", "");
            UIMenuItem InventoryItem = new("Inventory");
            UIMenu Inventory = new("Inventory", "");
            UIMenuItem WeaponsItem = new("Weapons");
            UIMenu Weapons = new("Weapons", "");
            walletItem.BindItemToMenu(wallet);
            InventoryItem.BindItemToMenu(Inventory);
            WeaponsItem.BindItemToMenu(Weapons);
            loot.AddItem(walletItem);
            loot.AddItem(InventoryItem);
            loot.AddItem(WeaponsItem);

            loot.OnMenuOpen += (a, b) =>
            {
                playerPed.Task.PlayAnimation("amb@medic@standing@kneel@base", "base");
                playerPed.Task.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 8f, -1, AnimationFlags.Loop);
            };
            loot.OnMenuClose += (a) =>
            {
                if (IsEntityPlayingAnim(playerPed.Handle, "anim@gangops@facility@servers@bodysearch@", "player_search", 3))
                    StopAnimTask(playerPed.Handle, "anim@gangops@facility@servers@bodysearch@", "player_search", 1f);
            };
            wallet.OnMenuOpen += (_new, b) =>
            {
                if (targetData.Money > 0)
                {
                    UIMenuItem cash = new UIMenuItem("Cash");
                    cash.SetRightLabel($"${targetData.Money}");
                    _new.AddItem(cash);
                    cash.Activated += async (a, b) =>
                    {
                        string val = await HUD.GetUserInput("Insert Quantity", "", 10);

                        if (val.All(x => char.IsDigit(x)))
                        {
                            int qt = Convert.ToInt32(val);

                            if (qt <= targetData.Money)
                            {
                                if (qt >= 1)
                                {
                                    BaseScript.TriggerServerEvent("lprp:removemoneytochar", target.ServerId, 0, qt);
                                    BaseScript.TriggerServerEvent("lprp:givemoneytochar", PlayerId(), 0, qt);
                                }
                                else
                                {
                                    HUD.ShowNotification("Can't insert 0 or less!");
                                    return;
                                }
                            }
                            else
                            {
                                HUD.ShowNotification("Can't get more than the victim owns!");
                                return;
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("Only numbers allowed!");
                            return;
                        }
                    };
                }

                if (targetData.DirtyCash > 0)
                {
                    UIMenuItem dirty = new UIMenuItem("Dirty Money");
                    dirty.SetRightLabel($"${targetData.DirtyCash}");
                    _new.AddItem(dirty);
                    dirty.Activated += async (a, b) =>
                    {
                        string val = await HUD.GetUserInput("Insert Quantity", "", 10);

                        if (val.All(x => char.IsDigit(x)))
                        {
                            int qt = Convert.ToInt32(val);

                            if (qt <= targetData.Money)
                            {
                                if (qt >= 1)
                                {
                                    BaseScript.TriggerServerEvent("lprp:removedirtytochar", target.ServerId, 0, qt);
                                    BaseScript.TriggerServerEvent("lprp:givedirtytochar", PlayerId(), 0, qt);
                                }
                                else
                                {
                                    HUD.ShowNotification("Can't insert 0 or less!");
                                    return;
                                }
                            }
                            else
                            {
                                HUD.ShowNotification("Can't get more than the victim owns!");
                                return;
                            }
                        }
                        else
                        {
                            HUD.ShowNotification("Only numbers allowed!");
                            return;
                        }
                    };
                }
            };
        }
    }
}