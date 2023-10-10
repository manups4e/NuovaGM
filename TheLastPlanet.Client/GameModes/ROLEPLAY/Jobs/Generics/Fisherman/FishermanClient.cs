using Settings.Shared.Roleplay.Jobs.Generics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.Lavoro;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Generics.Fisherman
{
    // TODO: THIS IS NOT A JOB ANYMORE, IT'S FREE FOR ANYONE, I'M MISSING RENTING POINTS FOR BOATS (FOR THOSE WHO WANT THEM)
    // TODO: TO BE MADE GENERIC POINTS FOR SELLING FISH AND GENERIC POINTS FOR PARKING BOATS FOR THOSE WHO HAVE THEM

    public class FishermanClient : GenericJob
    {
        private Fishermen FishingPoints;
        private string scenario = "WORLD_HUMAN_STAND_FISHING";
        private string AnimDict = "amb@world_human_stand_fishing@base";
        private bool JobAccepted = false;
        private Vehicle lastVehicle;
        public bool Fishing = false;
        public bool CaneInHand = false;
        private int CaneType = -1;
        private Prop FishingRod;
        private bool showBlip = false;
        private List<string> ToSellFish = new List<string>()
        {
            "europeanbass",
            "mackerel",
            "sole",
            "seabream",
            "tuna",
            "salmon",
            "codfish",
            "swordfish",
            "shark",
            "carp",
            "pike",
            "perch",
            "catfish",
            "speckledcatfish",
            "seabass",
            "trout",
            "goby",
            "zander",
            "bleak",
            "cruciancarp",
            "goldencruciancarp",
            "twaiteshad",
            "rove",
            "stickleback",
            "cobiansturgeon",
            "sturgeon",
            "sturgeonlabdanum"
        };
        private List<Blip> sellingFishBlip = new List<Blip>();

        private Vector3 FishingPoint = new Vector3(0);
        // TODO: ITEMS: FISHING ROD, FISHES, MAYBE ALSO SEAFOOD AND CRUSTACEANS
        // TODO: TO BE CONSIDERED A LOCKER ROOM.. OPTIONAL OR FORCED

        // N_0xc54a08c85ae4d410 while fishing (0.0 normal, 1.0 calm waters, 3.0 rough sea)
        // benson to transoprt the fish

        public FishermanClient() : base("Fisherman", Employment.Fisherman)
        {
            OnJobSet += Init;
            OnJobFired += Stop;
        }
        public async void Init()
        {
            FishingPoints = Client.Settings.RolePlay.Jobs.Generics.Fisherman;
            ConfigShared.SharedConfig.Main.Generics.ItemList["fishingrodbasic"].Use += async (item, index) =>
            {
                RequestAnimDict(AnimDict);
                RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
                MenuHandler.CloseAndClearHistory();
                FishingRod = new Prop(CreateObject((int)item.prop, 1729.73f, 6403.90f, 34.56f, true, true, true));
                AttachEntityToEntity(FishingRod.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
                TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
                CaneInHand = true;
                CaneType = 0;
                Client.Instance.AddTick(FishingTick);
                RemoveAnimDict("amb@code_human_wander_drinking@beer@male@base");
            };
            ConfigShared.SharedConfig.Main.Generics.ItemList["fishingrodinter"].Use += async (item, index) =>
            {
                RequestAnimDict(AnimDict);
                RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
                MenuHandler.CloseAndClearHistory();
                FishingRod = new Prop(CreateObject((int)item.prop, 1729.73f, 6403.90f, 34.56f, true, true, true));
                AttachEntityToEntity(FishingRod.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
                TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
                CaneInHand = true;
                CaneType = 1;
                Client.Instance.AddTick(FishingTick);
                RemoveAnimDict("amb@code_human_wander_drinking@beer@male@base");
            };
            ConfigShared.SharedConfig.Main.Generics.ItemList["fishingrodexpert"].Use += async (item, index) =>
            {
                RequestAnimDict(AnimDict);
                RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
                MenuHandler.CloseAndClearHistory();
                FishingRod = new Prop(CreateObject((int)item.prop, 1729.73f, 6403.90f, 34.56f, true, true, true));
                AttachEntityToEntity(FishingRod.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
                TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
                CaneInHand = true;
                CaneType = 2;
                Client.Instance.AddTick(FishingTick);
                RemoveAnimDict("amb@code_human_wander_drinking@beer@male@base");
            };
        }

        public async void Stop()
        {
            FishingPoints = null;
            ConfigShared.SharedConfig.Main.Generics.ItemList["fishingrodbasic"].Use -= async (item, index) =>
            {
                RequestAnimDict(AnimDict);
                RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
                MenuHandler.CloseAndClearHistory();
                FishingRod = new Prop(CreateObject((int)item.prop, 1729.73f, 6403.90f, 34.56f, true, true, true));
                AttachEntityToEntity(FishingRod.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
                TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
                CaneInHand = true;
                CaneType = 0;
                Client.Instance.AddTick(FishingTick);
                RemoveAnimDict("amb@code_human_wander_drinking@beer@male@base");
            };
            ConfigShared.SharedConfig.Main.Generics.ItemList["fishingrodinter"].Use -= async (item, index) =>
            {
                RequestAnimDict(AnimDict);
                RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
                MenuHandler.CloseAndClearHistory();
                FishingRod = new Prop(CreateObject((int)item.prop, 1729.73f, 6403.90f, 34.56f, true, true, true));
                AttachEntityToEntity(FishingRod.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
                TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
                CaneInHand = true;
                CaneType = 1;
                Client.Instance.AddTick(FishingTick);
                RemoveAnimDict("amb@code_human_wander_drinking@beer@male@base");
            };
            ConfigShared.SharedConfig.Main.Generics.ItemList["fishingrodexpert"].Use -= async (item, index) =>
            {
                RequestAnimDict(AnimDict);
                RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
                MenuHandler.CloseAndClearHistory();
                FishingRod = new Prop(CreateObject((int)item.prop, 1729.73f, 6403.90f, 34.56f, true, true, true));
                AttachEntityToEntity(FishingRod.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
                TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
                CaneInHand = true;
                CaneType = 2;
                Client.Instance.AddTick(FishingTick);
                RemoveAnimDict("amb@code_human_wander_drinking@beer@male@base");
            };
        }

        public void FishingControl()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Main.spawned)
            {
                if (ToSellFish.Any(o => Cache.PlayerCache.MyPlayer.User.GetInventoryItem(o).Item1))
                {
                    if (!showBlip)
                    {
                        foreach (Vector3 points in FishingPoints.SellingPoints)
                        {
                            Blip sellingPoint = new Blip(AddBlipForCoord(points[0], points[1], points[2]))
                            {
                                Sprite = BlipSprite.TowTruck,
                                Name = "Vendita pesce",
                                Color = BlipColor.MichaelBlue,
                                Scale = 0.8f,
                                IsShortRange = true
                            };
                            SetBlipDisplay(sellingPoint.Handle, 4);
                            sellingFishBlip.Add(sellingPoint);
                        }

                        showBlip = true;
                    }

                    foreach (Vector3 punto in FishingPoints.SellingPoints.Where(punto => p.IsInRangeOf(punto, 80)))
                    {
                        World.DrawMarker(MarkerType.DollarSign, punto, new Vector3(0), new Vector3(0), new Vector3(2.0f, 2.0f, 2.0f), Colors.DarkSeaGreen, false, false, true);

                        if (!p.IsInRangeOf(punto, 2)) continue;
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to sell the fish in your possession");
                        if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen) OpenFishSellingMenu();
                    }
                }
                else
                {
                    if (showBlip)
                    {
                        foreach (Blip blip in sellingFishBlip.Where(blip => blip.Exists())) blip.Delete();
                        sellingFishBlip.Clear();
                        showBlip = false;
                    }
                }

                /*			if (Vector3.Distance(Cache.Char.posizione.ToVector3(), PuntiPesca.AffittoBarca) < 2f && !Cache.Char.Status.PlayerStates.InVeicolo)
							{
								HUD.ShowHelp("Press ~INPUT_CONTEXT~ per scegliere una ~b~barca~w~.");
								if (Input.IsControlJustPressed(Control.Context))
								{
									MenuBarche();
								}
							}
				*/
            }
        }

        private void OpenFishSellingMenu()
        {
            DateTime oggi = new DateTime();

            if (oggi.DayOfWeek == DayOfWeek.Monday || oggi.DayOfWeek == DayOfWeek.Wednesday || oggi.DayOfWeek == DayOfWeek.Friday)
            {
                UIMenu venditaPesce = new UIMenu("Fresh fish selling", "Sell here to gain more", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
                List<Shared.Inventory> inventory = Cache.PlayerCache.MyPlayer.User.CurrentChar.Inventory;

                foreach (Shared.Inventory inv in inventory)
                {
                    foreach (List<dynamic> _amount in from s in ToSellFish where inv.Item == s select new List<dynamic>())
                    {
                        for (int j = 0; j < inv.Amount; j++) _amount.Add((j + 1).ToString());
                        UIMenuListItem fish = new UIMenuListItem(ConfigShared.SharedConfig.Main.Generics.ItemList[inv.Item].label, _amount, 0, ConfigShared.SharedConfig.Main.Generics.ItemList[inv.Item].description);
                        venditaPesce.AddItem(fish);
                        fish.OnListSelected += async (item, index) =>
                        {
                            string quantita = item.Items[item.Index].ToString();
                            int perc = 0;
                            if (Convert.ToInt32(quantita) > 9 && Convert.ToInt32(quantita) < 20)
                                perc = 2;
                            else if (Convert.ToInt32(quantita) > 19 && Convert.ToInt32(quantita) < 30)
                                perc = 4;
                            else if (Convert.ToInt32(quantita) > 29 && Convert.ToInt32(quantita) < 40)
                                perc = 6;
                            else if (Convert.ToInt32(quantita) > 39 && Convert.ToInt32(quantita) < 50)
                                perc = 8;
                            else if (Convert.ToInt32(quantita) > 49 && Convert.ToInt32(quantita) < 60)
                                perc = 10;
                            else if (Convert.ToInt32(quantita) > 59 && Convert.ToInt32(quantita) < 70)
                                perc = 12;
                            else if (Convert.ToInt32(quantita) > 69 && Convert.ToInt32(quantita) < 80)
                                perc = 14;
                            else if (Convert.ToInt32(quantita) > 79 && Convert.ToInt32(quantita) < 90)
                                perc = 16;
                            else if (Convert.ToInt32(quantita) > 89 && Convert.ToInt32(quantita) < 100)
                                perc = 18;
                            else if (Convert.ToInt32(quantita) > 99) perc = 20;
                            int valoreAggiunto = ConfigShared.SharedConfig.Main.Generics.ItemList[inv.Item].sellPrice + ConfigShared.SharedConfig.Main.Generics.ItemList[inv.Item].sellPrice * (perc + (int)Math.Round(Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.FISHING / 10)) / 100;
                            BaseScript.TriggerServerEvent("lprp:removeIntenvoryItem", inv.Item, Convert.ToInt32(quantita));
                            BaseScript.TriggerServerEvent("lprp:givemoney", valoreAggiunto * Convert.ToInt32(quantita));
                        };
                    }
                }

                venditaPesce.Visible = true;
            }
            else
            {
                HUD.ShowNotification("The fish market is closed today, come back another day!!");
            }
        }

        public async Task FishingTick()
        {
            if (CaneInHand)
            {
                Game.DisableControlThisFrame(0, Control.FrontendX);
                Game.DisableControlThisFrame(0, Control.FrontendY);
                if (!Fishing) HUD.ShowHelp("Press ~INPUT_FRONTEND_X~ to start fishing.~n~Press ~INPUT_FRONTEND_Y~ to stop");

                if (Input.IsDisabledControlJustPressed(Control.FrontendX))
                {
                    float altezza = 0;

                    if (GetWaterHeightNoWaves(Cache.PlayerCache.MyPlayer.Position.ToVector3.X, Cache.PlayerCache.MyPlayer.Position.ToVector3.Y, Cache.PlayerCache.MyPlayer.Position.ToVector3.Z, ref altezza))
                    {
                        Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;
                        SetEnableHandcuffs(PlayerPedId(), true);
                        FishingRod.Detach();
                        AttachEntityToEntity(FishingRod.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), 60309), 0, 0, 0, 0, 0, 0, false, false, false, false, 2, true);
                        TaskPlayAnim(PlayerPedId(), AnimDict, "base", 8.0f, -8, -1, 34, 0, false, false, false);
                        Fishing = true;
                    }
                    else
                    {
                        HUD.ShowNotification("You can't fish here.. Try going into the water a bit.", ColoreNotifica.Red, true);
                    }
                }

                if (Input.IsDisabledControlJustPressed(Control.FrontendY))
                {
                    FishingRod.Delete();
                    Client.Instance.RemoveTick(FishingTick);
                    Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();
                    CaneInHand = false;
                    CaneType = -1;
                }
            }

            if (Fishing)
            {
                if (FishingPoints.DynamicFishingTime)
                    await BaseScript.Delay(SharedMath.GetRandomInt(30000, 120000));
                else
                    await BaseScript.Delay(FishingPoints.FixedTime * 1000);
                await ControlsAndFish();
            }
        }

        private async Task ControlsAndFish()
        {
            int totalTouches = SharedMath.GetRandomInt(20, 40);
            int doneTouches = 0;
            int genericCount = 0;
            int maxCount = 0;

            if (CaneType != -1)
            {
                switch (CaneType)
                {
                    case 0:
                        totalTouches = SharedMath.GetRandomInt(20, 40);
                        maxCount = 1500;

                        break;
                    case 1:
                        totalTouches = SharedMath.GetRandomInt(30, 50);
                        maxCount = 1000;

                        break;
                    default:
                        {
                            if (CaneType == 0)
                            {
                                totalTouches = SharedMath.GetRandomInt(40, 60);
                                maxCount = 800;
                            }

                            break;
                        }
                }

                while (doneTouches < totalTouches)
                {
                    await BaseScript.Delay(0);
                    Game.DisableControlThisFrame(0, Control.Attack);
                    Game.DisableAllControlsThisFrame(1);

                    if (Game.CurrentInputMode == InputMode.GamePad)
                    {
                        HUD.ShowHelp("Something has taken the bait!! Turn ~INPUT_LOOK_UD~ to fish it!");
                        if (Input.IsDisabledControlJustPressed(Control.LookUpOnly) || Input.IsControlJustPressed(Control.LookDownOnly)) doneTouches += 1;
                    }
                    else
                    {
                        HUD.ShowHelp("Something has taken the bait!! Press rapidly ~INPUT_ATTACK~ to fish it!");
                        if (Input.IsDisabledControlJustPressed(Control.Attack)) doneTouches += 1;
                    }

                    genericCount += 1;

                    if (genericCount > maxCount) break;
                }

                if (SharedMath.GetRandomInt(0, 100) < 90 && genericCount < maxCount)
                {
                    string fish = "";

                    switch (CaneType)
                    {
                        case 0:
                            fish = FishingPoints.Fishes.Easy[SharedMath.GetRandomInt(0, FishingPoints.Fishes.Easy.Count - 1)];

                            break;
                        case 1:
                            fish = FishingPoints.Fishes.Intermediate[SharedMath.GetRandomInt(0, FishingPoints.Fishes.Intermediate.Count - 1)];

                            break;
                        case 2:
                            fish = FishingPoints.Fishes.Advanced[SharedMath.GetRandomInt(0, FishingPoints.Fishes.Advanced.Count - 1)];

                            break;
                    }

                    float peso = SharedMath.GetRandomFloat(0f, ConfigShared.SharedConfig.Main.Generics.ItemList[fish].weight);
                    HUD.ShowNotification($"You caught a nice specimen of {ConfigShared.SharedConfig.Main.Generics.ItemList[fish].label}, by the weight of {peso}Kg");
                    BaseScript.TriggerServerEvent("lprp:addIntenvoryItem", fish, 1, peso);
                }
                else
                {
                    HUD.ShowNotification("The fish escaped! It'll be better next time..", true);
                }

                Fishing = false;
                Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();
                FishingRod.Detach();
                AttachEntityToEntity(FishingRod.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
                TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
                Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
                SetEnableHandcuffs(PlayerPedId(), false);
                await Task.FromResult(0);
            }
        }

        private async void BoatsMenu()
        {
            UIMenu Boats = new UIMenu("Fisherman", "Choose your boat", new System.Drawing.PointF(50, 50), "thelastgalaxy", "bannerbackground", false, true);
            foreach (UIMenuItem boat in FishingPoints.Boats.Select(barca => new UIMenuItem(GetLabelText(barca), "~y~If you're not alone~w~ you can use a single boat together and save on rent!"))) Boats.AddItem(boat);
            Vehicle veh = new Vehicle(0);
            Boats.OnIndexChange += async (menu, index) =>
            {
                if (veh.Exists()) veh.Delete();
                veh = await Functions.SpawnLocalVehicle(FishingPoints.Boats[index], new Vector3(FishingPoints.BoatSpawn[0], FishingPoints.BoatSpawn[1], FishingPoints.BoatSpawn[2]), FishingPoints.BoatSpawn[3]);
            };
            Boats.OnItemSelect += async (menu, item, index) =>
            {
                MenuHandler.CloseAndClearHistory();
                if (veh.Exists()) veh.Delete();
                Vehicle newveh = await Functions.SpawnVehicleNoPlayerInside(FishingPoints.Boats[index], new Vector3(FishingPoints.BoatSpawn[0], FishingPoints.BoatSpawn[1], FishingPoints.BoatSpawn[2]), FishingPoints.BoatSpawn[3]);
                JobVeh_Rent vehlav = new JobVeh_Rent(newveh, Cache.PlayerCache.MyPlayer.User.FullName);
                BaseScript.TriggerServerEvent("lprp:registraVeicoloLavorativoENon", vehlav.ToJson());
            };
            Boats.Visible = true;
        }
    }
}