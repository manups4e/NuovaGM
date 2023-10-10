using Settings.Shared.Roleplay.Jobs.Generics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Core.Status;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Generics.Hunt
{
    internal static class HunterClient
    {
        public static bool Hunting = false;
        private static Blip HuntingPoint;
        public static Hunters Hunter;
        private static List<string> animalGroups = new List<string>
        {
            "WILD_ANIMAL",
            "SHARK",
            "COUGAR",
            "GUARD_DOG",
            "DOMESTIC_ANIMAL",
            "DEER"
        };
        private static string fireWeapon = "WEAPON_SNIPERRIFLE";
        private static string whiteWeapon = "weapon_knife";
        private static bool rentFireWeapon = false;
        private static bool rentWhiteWeapon = false;
        private static Dictionary<int, AnimalToHunt> killedAnimals = new Dictionary<int, AnimalToHunt>();
        private static List<PedHash> animals = new List<PedHash>()
        {
            PedHash.Deer,
            PedHash.Boar,
            PedHash.Coyote,
            PedHash.Rabbit,
            PedHash.ChickenHawk,
            PedHash.MountainLion
        };
        private static Blip HuntingZone;

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            Client.Instance.AddEventHandler("DamageEvents:PedKilledByPlayer", new Action<int, int, uint, bool>(AnimalControl));
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("DamageEvents:PedKilledByPlayer", new Action<int, int, uint, bool>(AnimalControl));
            Hunter = null;
            HuntingPoint.Delete();
        }

        private static void Spawned(PlayerClient client)
        {
            Hunter = Client.Settings.RolePlay.Jobs.Generics.Hunter;
            HuntingPoint = World.CreateBlip(Hunter.startHunt);
            HuntingPoint.Sprite = BlipSprite.Hunting;
            HuntingPoint.Color = BlipColor.TrevorOrange;
            HuntingPoint.Scale = 1.0f;
            HuntingPoint.Name = "Hunting Zone";
            HuntingPoint.IsShortRange = true;
            SetBlipDisplay(HuntingPoint.Handle, 4);
        }

        public static async Task HuntCheck()
        {
            if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(Hunter.startHunt, 1.375f))
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to start the hunting menu");
                if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) OpenWeaponRentMenu();
            }

            await Task.FromResult(0);
        }

        public static async Task BordersCheck()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (!p.IsInRangeOf(Hunter.HuntingZone, Hunter.AreaLimit))
            {
                if (rentWhiteWeapon) p.Weapons.Remove(WeaponHash.Knife);
                if (rentFireWeapon) p.Weapons.Remove(WeaponHash.SniperRifle);
                HUD.ShowNotification("You left the hunting area without returning your weapons! You will pay a fine!", ColoreNotifica.Red, true);
                Hunting = false;
                BaseScript.TriggerServerEvent("lprp:removeBank", 1000);
                if (HuntingZone.Exists()) HuntingZone.Delete();
                foreach (string s in animalGroups) p.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Functions.HashInt(s)), Relationship.Neutral, true);
                killedAnimals.Clear();
                p.Weapons.Select(WeaponHash.Unarmed);
                Client.Instance.RemoveTick(BordersCheck);
                Client.Instance.RemoveTick(KilledCheck);
            }

            await BaseScript.Delay(1000);
        }

        private static async void AnimalControl(int victim, int player, uint weaponHash, bool wasMeleeDamage)
        {
            if (!Hunting) return;
            Ped animale = new Ped(victim);

            if (!animals.Contains((PedHash)animale.Model.Hash)) return;
            Player killer = new Player(player);
            AnimalToHunt animal = new AnimalToHunt() { Entity = animale, Prized = false };

            if (killer.Character != Cache.PlayerCache.MyPlayer.Ped) return;
            if (!killedAnimals.ContainsKey(animale.Handle)) killedAnimals.Add(animale.Handle, animal);
            int hash = animale.Model.Hash;
            float addStatValue = 0;

            switch ((uint)hash)
            {
                case 3630914197: // deer
                    addStatValue = IsPedMale(animale.Handle) ? 0.003f : 0.002f;

                    break;
                case 3462393972: // boar
                    addStatValue = 0.0025f;

                    break;
                case 1682622302: // coyote
                    addStatValue = 0.003f;

                    break;
                case 3753204865: // coniglio
                    addStatValue = 0.006f;

                    break;
                case 2864127842: // aquila
                    addStatValue = 0.004f;

                    break;
                case 307287994: // leone di montagna
                    addStatValue = 0.005f;

                    break;
            }

            StatsNeeds.RegisterStats(Skills.HUNTING, addStatValue);
        }

        public static async Task KilledCheck()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            foreach (KeyValuePair<int, AnimalToHunt> anim in killedAnimals.Where(anim => p.IsNearEntity(anim.Value.Entity, new Vector3(2, 2, 2)) && anim.Value.Entity.Model.Hash != (int)PedHash.MountainLion).Where(anim => p.Weapons.HasWeapon(WeaponHash.Knife)))
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to loot");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    p.Weapons.Select(WeaponHash.Knife);
                    TaskStartScenarioInPlace(PlayerPedId(), "CODE_HUMAN_MEDIC_TEND_TO_DEAD", 0, true); // oppure CODE_HUMAN_MEDIC_KNEEL 
                    Screen.Fading.FadeOut(2000);
                    await BaseScript.Delay(2001);
                    int hash = anim.Value.Entity.Model.Hash;
                    string msg = "";
                    float statValue = 0;
                    string meat = "";

                    switch ((uint)hash)
                    {
                        case 3630914197:
                            if (IsPedMale(anim.Key))
                            {
                                msg = " Derr";
                                statValue = 0.002f;
                            }
                            else
                            {
                                msg = " Doe";
                                statValue = 0.001f;
                            }

                            meat = "deermeat";

                            break;
                        case 3462393972:
                            msg = " Boar";
                            statValue = 0.002f;
                            meat = "boarmeat";

                            break;
                        case 1682622302:
                            msg = " Coyote";
                            statValue = 0.002f;
                            meat = "coyotemeat";

                            break;
                        case 3753204865:
                            msg = " Rabbit";
                            statValue = 0.004f;
                            meat = "rabbitmeat";

                            break;
                        case 2864127842:
                            msg = "'Eagle";
                            statValue = 0.003f;
                            meat = "eaglemeat";

                            break;
                        case 307287994: // mountainLion no meat
                            statValue = 0.003f;

                            break;
                    }

                    StatsNeeds.RegisterStats(Skills.HUNTING, statValue);
                    killedAnimals.ToList().Remove(anim);
                    anim.Value.Entity.Delete();
                    await BaseScript.Delay(1000);
                    Screen.Fading.FadeIn(500);
                    await BaseScript.Delay(501);
                    p.Task.ClearAll();
                    HUD.ShowNotification($"You killed and slaughtered a ~y~{msg}~w~ and got 2 pieces of ~b~{ConfigShared.SharedConfig.Main.Generics.ItemList[meat].label}~w~.", ColoreNotifica.GreenDark, true);
                    BaseScript.TriggerServerEvent("lprp:addIntenvoryItem", meat, 2, 0.5f);
                }
            }

            await Task.FromResult(0);
        }

        private static async void OpenWeaponRentMenu()
        {
            UIMenu weaponsRent = new UIMenu("Hunters menu", "For true lovers of a noble sport", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);

            if (!Hunting)
            {
                UIMenuItem armi = new UIMenuItem("Rent hunting weapons", "If you don't have them, you will be given a sniper rifle and a knife\nPrice: 250 rifle, 50 knife");
                UIMenuItem start = new UIMenuItem("Start hunting", "Let's go!");
                weaponsRent.AddItem(armi);
                weaponsRent.AddItem(start);
                weaponsRent.OnItemSelect += async (menu, item, index) =>
                {
                    if (Cache.PlayerCache.MyPlayer.User.HasLicense("Hunting"))
                    {
                        if (item == armi)
                        {
                            if ((Cache.PlayerCache.MyPlayer.User.HasWeapon(fireWeapon) || rentFireWeapon) && (Cache.PlayerCache.MyPlayer.User.HasWeapon(whiteWeapon) || rentWhiteWeapon))
                            {
                                HUD.ShowNotification("You already have the weapons we rent.", ColoreNotifica.Red, true);

                                return;
                            }

                            int prezzo = 0;

                            if (!Cache.PlayerCache.MyPlayer.User.HasWeapon(fireWeapon))
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Money >= 250 || Cache.PlayerCache.MyPlayer.User.Bank >= 250)
                                {
                                    Cache.PlayerCache.MyPlayer.Ped.Weapons.Give(WeaponHash.SniperRifle, 100, false, true);
                                    prezzo += 250;
                                    rentFireWeapon = true;
                                }
                            }

                            if (!Cache.PlayerCache.MyPlayer.User.HasWeapon(whiteWeapon))
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Money >= 50 || Cache.PlayerCache.MyPlayer.User.Bank >= 50)
                                {
                                    Cache.PlayerCache.MyPlayer.Ped.Weapons.Give(WeaponHash.Knife, 1, false, true);
                                    prezzo += 50;
                                    rentWhiteWeapon = true;
                                }
                            }

                            if (Cache.PlayerCache.MyPlayer.User.Money >= prezzo)
                            {
                                BaseScript.TriggerServerEvent("lprp:removeMoney", prezzo);
                                HUD.ShowNotification("The weapons you didnt have were given to you by rent");
                            }
                            else
                            {
                                if (Cache.PlayerCache.MyPlayer.User.Bank >= prezzo)
                                {
                                    BaseScript.TriggerServerEvent("lprp:removeBank", prezzo);
                                    HUD.ShowNotification("The weapons you didnt have were given to you by rent");
                                }
                                else
                                {
                                    HUD.ShowNotification("Not enough money to rent weapons!");
                                }
                            }
                        }
                        else if (item == start)
                        {
                            Hunting = true;
                            HuntingZone = World.CreateBlip(Hunter.HuntingZone, Hunter.AreaLimit);
                            HuntingZone.Name = "Zona di caccia";
                            HuntingZone.Alpha = 25;
                            HuntingZone.Color = BlipColor.TrevorOrange;
                            foreach (string s in animalGroups) Cache.PlayerCache.MyPlayer.Ped.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Functions.HashInt(s)), Relationship.Dislike, true);
                            Client.Instance.AddTick(BordersCheck);
                            Client.Instance.AddTick(KilledCheck);
                            SetBlipDisplay(HuntingZone.Handle, 4);
                            MenuHandler.CloseAndClearHistory();
                            await BaseScript.Delay(10000);
                            HUD.ShowHelp("You have started hunting, don't move away from the area marked on the map or you will lose your rented weapons and pay a fine!!", 5000);
                            await BaseScript.Delay(10000);
                            HUD.ShowHelp("You can hunt the animals you find in the area, if you have the knife and get close to the killed animal, you can take its meat!", 5000);
                        }
                    }
                    else
                    {
                        HUD.ShowNotification("You don't have a hunting license!", ColoreNotifica.Red, true);
                    }
                };
            }
            else
            {
                UIMenuItem end = new UIMenuItem("Stop Hunting", "Return your weapons");
                weaponsRent.AddItem(end);
                end.Activated += (menu, item) =>
                {
                    if (rentWhiteWeapon) BaseScript.TriggerServerEvent("lprp:removeWeapon", whiteWeapon);
                    if (rentFireWeapon) BaseScript.TriggerServerEvent("lprp:removeWeapon", fireWeapon);
                    HUD.ShowNotification("Thanks for choosing our hunting reserve!\nCome back soon!", ColoreNotifica.GreenDark);
                    Hunting = false;
                    Client.Instance.RemoveTick(BordersCheck);
                    if (HuntingZone.Exists()) HuntingZone.Delete();
                    foreach (string s in animalGroups) Cache.PlayerCache.MyPlayer.Ped.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Functions.HashInt(s)), Relationship.Neutral, true);
                    killedAnimals.Clear();
                    Cache.PlayerCache.MyPlayer.Ped.Weapons.Select(WeaponHash.Unarmed);
                    MenuHandler.CloseAndClearHistory();
                };
            }

            weaponsRent.Visible = true;
        }
    }

    internal class AnimalToHunt
    {
        public Ped Entity;
        public bool Prized = false;
    }
}