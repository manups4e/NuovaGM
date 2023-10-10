using Settings.Shared.Roleplay.Jobs.Generics;
using System;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Interactions;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Vehicles;
using TheLastPlanet.Client.Handlers;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Generics.Taxi
{
    internal static class TaxiClient
    {
        private static TaxiFlags jobs = new TaxiFlags();
        private static TaxiMeter taximeter = new TaxiMeter();
        private static Tassisti taxi;
        private static Vehicle serviceVehicle;
        private static Ped NPCPassenger;
        private static bool OnDuty = false;

        public static void Init()
        {
            taxi = Client.Settings.RolePlay.Jobs.Generics.TaxiDriver;
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            TickController.TickOnFoot.Add(Markers);
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            taxi = null;
            TickController.TickOnFoot.Remove(Markers);
            Client.Instance.RemoveTick(Markers);
            Blip p = World.GetAllBlips().FirstOrDefault(x => x.Position == taxi.PosAcceptance);
            if (p != null) p.Delete();
        }

        private static void Spawned(PlayerClient client)
        {
            Blip Tax = World.CreateBlip(taxi.PosAcceptance);
            Tax.Sprite = BlipSprite.PersonalVehicleCar;
            Tax.Color = BlipColor.Yellow;
            Tax.Name = "Taxi";
            Tax.IsShortRange = true;
            SetBlipDisplay(Tax.Handle, 4);
        }

        private static async Task TaximeterTick()
        {
            if (OnDuty)
            {
                taximeter.CreateTaxiMeter(serviceVehicle);
                taximeter.RenderMeter();
            }
        }

        public static async Task Markers()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (p.IsInRangeOf(taxi.PosAcceptance, 100))
            {
                World.DrawMarker(MarkerType.ChevronUpx2, taxi.PosAcceptance, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Yellow, rotateY: true);

                if (p.IsInRangeOf(taxi.PosAcceptance, 1.375f))
                {
                    if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() != "taxi")
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to accept a job as a taxi driver.");

                        if (Input.IsControlJustPressed(Control.Context))
                        {
                            Job tass = new Job("Taxi", 0);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "job", tass.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Job = tass;
                        }
                    }
                    else
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to stop working.");

                        if (Input.IsControlJustPressed(Control.Context))
                        {
                            Job disoc = new Job("Unemployed", 0);
                            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "job", disoc.ToJson());
                            Cache.PlayerCache.MyPlayer.User.CurrentChar.Job = disoc;
                        }
                    }
                }
            }

            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "taxi")
            {
                if (p.IsInRangeOf(taxi.PosVehicleCollection, 100))
                {
                    if (serviceVehicle == null || serviceVehicle != null && !serviceVehicle.Exists() || serviceVehicle.IsDead)
                    {
                        World.DrawMarker(MarkerType.CarSymbol, taxi.PosVehicleCollection, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Yellow, rotateY: true);

                        if (p.IsInRangeOf(taxi.PosVehicleCollection, 1.375f))
                        {
                            // TODO: ADD A CHECK TICK FOR PEOPLE DAMAGING THEI TAXI, A FINE AT THE END OF THE JOB WILL BE GIVEN BASED ON DAMAGES
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to take your service vehicle.");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                if (p.IsVisible) NetworkFadeOutEntity(p.Handle, true, false);
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                serviceVehicle = await Functions.SpawnVehicle("taxi", taxi.PosSpawnVeicolo.ToVector3(), taxi.PosSpawnVeicolo.W);
                                serviceVehicle.SetVehicleFuelLevel(100f);
                                // TODO: PLATES SHOULD BE IN ORDER OF TAXIS... LIKE.. IF THERE ARE 3 TAXIS OUT.. 4TH ONE'S PLATE WILL BE TAXI_004
                                serviceVehicle.Mods.LicensePlate = "TAXI_" + SharedMath.GetRandomInt(000, 999).ToString("000");
                                serviceVehicle.IsEngineRunning = true;
                                serviceVehicle.IsDriveable = true;
                                NetworkFadeInEntity(PlayerPedId(), true);
                                Screen.Fading.FadeIn(500);
                            }
                        }
                    }
                }

                if (p.IsInRangeOf(taxi.PosVehicleReturn, 100))
                {
                    if (serviceVehicle != null && !serviceVehicle.IsDead && serviceVehicle.Exists())
                    {
                        World.DrawMarker(MarkerType.CarSymbol, taxi.PosVehicleReturn, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Red, rotateY: true);

                        if (p.IsInRangeOf(taxi.PosVehicleReturn, 1.375f))
                        {
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to return your service vehicle.");

                            if (Input.IsControlJustPressed(Control.Context))
                            {
                                if (p.CurrentVehicle.IsVisible) NetworkFadeOutEntity(p.CurrentVehicle.Handle, true, false);
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                taximeter.meter_entity.Delete();
                                serviceVehicle.Delete();
                                serviceVehicle = null;
                                p.Position = taxi.PosVehicleCollection;
                                NetworkFadeInEntity(PlayerPedId(), true);
                                Screen.Fading.FadeIn(500);
                                if (OnDuty) GoOffDuty(1);
                            }
                        }
                    }
                }

                if (Input.IsControlJustPressed(Control.SelectCharacterFranklin, PadCheck.Keyboard))
                {
                    if (serviceVehicle != null && serviceVehicle.IsAlive && serviceVehicle.Exists())
                    {
                        SetTaxiLights(serviceVehicle.Handle, OnDuty);
                        OnDuty = !OnDuty;

                        if (OnDuty)
                        {
                            taximeter.Taximeter = new Scaleform("taxi_display");
                            while (!taximeter.Taximeter.IsLoaded) await BaseScript.Delay(0);
                            taximeter.meter_rt = RenderTargets.CreateNamedRenderTargetForModel("taxi", (uint)GetHashKey("prop_taxi_meter_2"));
                            Client.Instance.AddTick(TaxiService);
                            HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "You entered service. Drive the streets looking for customers.", NotificationIcon.Taxi, TipoIcona.ChatBox);
                            if (p.IsInVehicle(serviceVehicle)) Client.Instance.AddTick(TaximeterTick);
                        }
                        else
                        {
                            Client.Instance.RemoveTick(TaxiService);
                            HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "You are now off duty.", NotificationIcon.Taxi, TipoIcona.ChatBox);
                            GoOffDuty(1);
                        }
                    }
                    else
                    {
                        HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "You cannot go on duty without your work vehicle!.", NotificationIcon.Taxi, TipoIcona.ChatBox);
                    }
                }
            }
        }

        private static async Task TaxiService()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (OnDuty && jobs.onJob == 0)
            {
                if (p.IsInVehicle(serviceVehicle))
                {
                    if (p.CurrentVehicle.Driver == Cache.PlayerCache.MyPlayer.Ped)
                    {
                        jobs.flag[0] = 0;
                        jobs.flag[1] = 59 + SharedMath.GetRandomInt(1, 61);
                        jobs.onJob = 1;
                    }
                }
            }
            else if (jobs.onJob == 1 && OnDuty)
            {
                if (serviceVehicle.Exists() && serviceVehicle.IsDriveable)
                {
                    if (p.IsSittingInVehicle(serviceVehicle))
                    {
                        if (NPCPassenger != null && NPCPassenger.Exists())
                        {
                            if (IsPedFatallyInjured(NPCPassenger.Handle))
                            {
                                if (NPCPassenger.AttachedBlip != null && NPCPassenger.AttachedBlip.Exists()) NPCPassenger.AttachedBlip.Delete();
                                NPCPassenger.MarkAsNoLongerNeeded();
                                NPCPassenger = null;
                                jobs.flag[0] = 0;
                                jobs.flag[1] = 59 + SharedMath.GetRandomInt(1, 61);

                                if (jobs.blip != null && jobs.blip.Exists())
                                {
                                    jobs.blip.Delete();
                                    jobs.blip = null;
                                }

                                HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "Your client has ~r~died~w~. Find another customer, we've already notified the police", NotificationIcon.Taxi, TipoIcona.ChatBox);
                            }
                            else
                            {
                                if (jobs.flag[0] == 1 && jobs.flag[1] > 0)
                                {
                                    await BaseScript.Delay(1000);
                                    jobs.flag[1]--;

                                    if (jobs.flag[1] == 0)
                                    {
                                        if (NPCPassenger.AttachedBlip != null && NPCPassenger.AttachedBlip.Exists()) NPCPassenger.AttachedBlip.Delete();
                                        NPCPassenger.Task.ClearAllImmediately();
                                        NPCPassenger.MarkAsNoLongerNeeded();
                                        NPCPassenger = null;
                                        HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "Your customer got impatient and canceled the service. Find ~y~another one~w.", NotificationIcon.Taxi, TipoIcona.ChatBox);
                                        jobs.flag[0] = 0;
                                        jobs.flag[1] = 59 + SharedMath.GetRandomInt(1, 61);
                                    }
                                    else
                                    {
                                        if (p.IsSittingInVehicle(serviceVehicle))
                                        {
                                            if (Cache.PlayerCache.MyPlayer.Position.Distance(NPCPassenger.Position) < 8.0001f)
                                            {
                                                NPCPassenger.IsPersistent = true;
                                                NPCPassenger.BlockPermanentEvents = true;
                                                SetPedCombatAttributes(NPCPassenger.Handle, 17, true);
                                                Vector3 offs = GetOffsetFromEntityInWorldCoords(serviceVehicle.Handle, 1.5f, 0.0f, 0.0f);
                                                Vector3 offs2 = GetOffsetFromEntityInWorldCoords(serviceVehicle.Handle, -1.5f, 0.0f, 0.0f);
                                                if (Vector3.Distance(offs, NPCPassenger.Position) < Vector3.Distance(offs2, NPCPassenger.Position))
                                                    TaskEnterVehicle(NPCPassenger.Handle, serviceVehicle.Handle, -1, 2, 1.0001f, 1, 0);
                                                else
                                                    TaskEnterVehicle(NPCPassenger.Handle, serviceVehicle.Handle, -1, 1, 1.0001f, 1, 0);
                                                NPCPassenger.AlwaysKeepTask = true;
                                                while (!NPCPassenger.IsInVehicle(serviceVehicle)) await BaseScript.Delay(0);
                                                serviceVehicle.LockStatus = VehicleLockStatus.Locked;
                                                NPCPassenger.BlockPermanentEvents = true;
                                                if (NPCPassenger.AttachedBlip != null && NPCPassenger.AttachedBlip.Exists()) NPCPassenger.AttachedBlip.Delete();
                                                jobs.pedentpos = NPCPassenger.Position;
                                                jobs.flag[0] = 2;
                                                jobs.flag[1] = 30;
                                            }
                                        }
                                    }
                                }

                                if (jobs.flag[0] == 2 && jobs.flag[1] > 0)
                                {
                                    await BaseScript.Delay(1000);
                                    jobs.flag[1]--;

                                    if (jobs.flag[1] == 0)
                                    {
                                        if (NPCPassenger.AttachedBlip != null && NPCPassenger.AttachedBlip.Exists()) NPCPassenger.AttachedBlip.Delete();
                                        NPCPassenger.Task.ClearAll();
                                        NPCPassenger.MarkAsNoLongerNeeded();
                                        NPCPassenger = null;
                                        HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "Your customer doesn't feel safe with you, stop and let him off, you'll have to find ~y~another one~w~.", NotificationIcon.Taxi, TipoIcona.ChatBox);
                                        while (serviceVehicle.Speed > 0) await BaseScript.Delay(1000);
                                        NPCPassenger.Task.LeaveVehicle(serviceVehicle, true);
                                        jobs.flag[0] = 0;
                                        jobs.flag[1] = 59 + SharedMath.GetRandomInt(1, 61);
                                        taximeter.ClearDisplay();
                                    }
                                    else
                                    {
                                        if (p.IsSittingInVehicle(serviceVehicle))
                                        {
                                            if (NPCPassenger.AttachedBlip != null && NPCPassenger.AttachedBlip.Exists()) NPCPassenger.AttachedBlip.Delete();
                                            jobs.flag[0] = 3;
                                            jobs.flag[1] = SharedMath.GetRandomInt(0, 92);
                                            uint str = 0;
                                            uint cross = 0;
                                            Vector3 pos = taxi.jobCoords[jobs.flag[1]];
                                            GetStreetNameAtCoord(pos.X, pos.Y, pos.Z, ref str, ref cross);
                                            string street = "";
                                            if (cross > 0)
                                                street = $"Take me to {GetStreetNameFromHashKey(str)}, near {GetStreetNameFromHashKey(cross)}";
                                            else
                                                street = $"Take me to {GetStreetNameFromHashKey(str)}";
                                            Screen.ShowSubtitle(street, 5000);
                                            float totalDist = CalculateTravelDistanceBetweenPoints(serviceVehicle.Position.X, serviceVehicle.Position.Y, serviceVehicle.Position.Z, pos.X, pos.Y, pos.Z);
                                            jobs.jobPay = (int)Math.Round(totalDist * taxi.PriceModifier);
                                            jobs.blip = World.CreateBlip(pos);
                                            jobs.blip.Name = GetStreetNameFromHashKey(str);
                                            jobs.blip.ShowRoute = true;
                                            taximeter.AddDestination(0, 2, 0, 0, 255, GetStreetNameFromHashKey(str), GetLabelText(GetNameOfZone(pos.X, pos.Y, pos.Z)), cross > 0 ? GetStreetNameFromHashKey(cross) : "", false);
                                            taximeter.SetPrice(jobs.jobPay);
                                            taximeter.HighlightDestination(false);
                                            taximeter.ShowDestination();
                                        }
                                    }
                                }

                                if (jobs.flag[0] == 3)
                                {
                                    if (Vector3.Distance(serviceVehicle.Position, taxi.jobCoords[jobs.flag[1]]) > 2f)
                                    {
                                        World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(taxi.jobCoords[jobs.flag[1]].X, taxi.jobCoords[jobs.flag[1]].Y, taxi.jobCoords[jobs.flag[1]].Z - 1f), Vector3.Zero, Vector3.Zero, new Vector3(4f, 4f, 2f), Colors.Gold);

                                        if (serviceVehicle.Speed > 130 * 3.6f)
                                        {
                                            if (jobs.flag[1] > 0)
                                            {
                                                await BaseScript.Delay(1000);
                                                jobs.flag[1]--;
                                                Screen.ShowSubtitle("Your driving ~y~too fast~w~! Slow down or your passenger will get too ~r~scared~w~!", 1000);
                                            }

                                            if (jobs.flag[1] == 0)
                                            {
                                                if (NPCPassenger.AttachedBlip != null && NPCPassenger.AttachedBlip.Exists()) NPCPassenger.AttachedBlip.Delete();
                                                NPCPassenger.Task.ClearAll();
                                                NPCPassenger.MarkAsNoLongerNeeded();
                                                NPCPassenger = null;
                                                HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "Your customer doesn't feel safe with you, stop and let him off, you'll have to find ~y~another one~w~..", NotificationIcon.Taxi, TipoIcona.ChatBox);
                                                while (serviceVehicle.Speed > 0) await BaseScript.Delay(1000);
                                                NPCPassenger.Task.LeaveVehicle(serviceVehicle, true);
                                                jobs.flag[0] = 0;
                                                jobs.flag[1] = 59 + SharedMath.GetRandomInt(1, 61);
                                                taximeter.ClearDisplay();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (serviceVehicle.Speed == 0)
                                        {
                                            if (jobs.blip != null && jobs.blip.Exists())
                                            {
                                                jobs.blip.Delete();
                                                jobs.blip = null;
                                            }

                                            if (NPCPassenger.AttachedBlip != null && NPCPassenger.AttachedBlip.Exists()) NPCPassenger.AttachedBlip.Delete();
                                            NPCPassenger.Task.LeaveVehicle(LeaveVehicleFlags.None);
                                            while (!NPCPassenger.IsInVehicle(serviceVehicle)) await BaseScript.Delay(0);
                                            NPCPassenger.Task.ClearAll();
                                            NPCPassenger.MarkAsNoLongerNeeded();
                                            NPCPassenger = null;
                                            await BaseScript.Delay(2500);
                                            HUD.ShowNotification("You have successfully brought your customer to their destination.", ColoreNotifica.GreenDark, true);
                                            BaseScript.TriggerServerEvent("lprp:givemoney", jobs.jobPay);
                                            HUD.ShowNotification("You earned $" + jobs.jobPay);
                                            await BaseScript.Delay(8000);
                                            HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "Drive through the streets looking for a passenger.", NotificationIcon.Taxi, TipoIcona.ChatBox);
                                            jobs.flag[0] = 0;
                                            jobs.flag[1] = 59 + SharedMath.GetRandomInt(1, 61);
                                            taximeter.ClearDisplay();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (jobs.flag[0] > 0)
                            {
                                jobs.flag[0] = 0;
                                jobs.flag[1] = 59 + SharedMath.GetRandomInt(1, 61);
                                HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "Drive through the streets looking for a passenger.", NotificationIcon.Taxi, TipoIcona.ChatBox);

                                if (jobs.blip != null && jobs.blip.Exists())
                                {
                                    jobs.blip.Delete();
                                    jobs.blip = null;
                                }
                            }

                            if (jobs.flag[0] == 0 && jobs.flag[1] > 0)
                            {
                                await BaseScript.Delay(1000);
                                jobs.flag[1]--;
                                {
                                    if (jobs.flag[1] == 0)
                                    {
                                        Vector3 pos = Cache.PlayerCache.MyPlayer.Position.ToVector3;
                                        Ped rand = new Ped(GetRandomPedAtCoord(pos.X, pos.Y, pos.Z, taxi.pickupRange, taxi.pickupRange, taxi.pickupRange, 26));

                                        if (rand.Exists())
                                        {
                                            NPCPassenger = rand;
                                            jobs.flag[0] = 1;
                                            jobs.flag[1] = 19 + SharedMath.GetRandomInt(21);
                                            NPCPassenger.Task.ClearAll();
                                            NPCPassenger.BlockPermanentEvents = true;
                                            NPCPassenger.Task.StandStill(1000 * jobs.flag[1]);
                                            HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "We have a passenger, go get them.", NotificationIcon.Taxi, TipoIcona.ChatBox);
                                            Blip lblip = NPCPassenger.AttachBlip();
                                            lblip.IsFriendly = true;
                                            lblip.Sprite = BlipSprite.Friend;
                                            lblip.Color = (BlipColor)3;
                                            SetBlipCategory(lblip.Handle, 3);
                                        }
                                        else
                                        {
                                            jobs.flag[0] = 0;
                                            jobs.flag[1] = 59 + SharedMath.GetRandomInt(1, 61);
                                            HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "Drive through the streets looking for a passenger.", NotificationIcon.Taxi, TipoIcona.ChatBox);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(Cache.PlayerCache.MyPlayer.Position.ToVector3, serviceVehicle.Position) > 30f)
                            GoOffDuty(1);
                        else
                            Screen.ShowSubtitle("Go back to your car to ~g~continue~w~ or ~r~walk away~w~ from the taxi to stop working");
                    }
                }
                else
                {
                    GoOffDuty(1);
                    HUD.ShowAdvancedNotification("Taxi driver switchboard", "Message to the driver", "Your taxi broke! You will be fined!.", NotificationIcon.Taxi, TipoIcona.ChatBox);
                }
            }
            else
            {
                await BaseScript.Delay(1000);
            }
        }

        private static void GoOffDuty(int id)
        {
            if (NPCPassenger != null && NPCPassenger.Exists())
            {
                if (NPCPassenger.AttachedBlip != null && NPCPassenger.AttachedBlip.Exists())
                {
                    NPCPassenger.AttachedBlip.Sprite = (BlipSprite)2;
                    SetBlipDisplay(NPCPassenger.AttachedBlip.Handle, 3);
                }

                NPCPassenger.Task.ClearAllImmediately();
                if (serviceVehicle != null && serviceVehicle.Exists() && serviceVehicle.IsDriveable)
                    if (NPCPassenger.IsSittingInVehicle(serviceVehicle))
                        NPCPassenger.Task.LeaveVehicle(LeaveVehicleFlags.None);
                NPCPassenger.MarkAsNoLongerNeeded();
                NPCPassenger = null;

                if (jobs.blip != null && jobs.blip.Exists())
                {
                    jobs.blip.Delete();
                    jobs.blip = null;
                }

                jobs = new TaxiFlags();
            }

            Client.Instance.RemoveTick(TaximeterTick);
        }
    }

    public class TaxiFlags
    {
        public int[] flag = new int[2];
        public int onJob = 0;
        public Blip blip;
        public Vector3 pedentpos;
        public int jobPay;
    }

    internal class TaxiMeter
    {
        public int meter_rt = 0;
        public Prop meter_entity;
        public Scaleform Taximeter = new Scaleform("taxi_display");
        public void AddDestination(int index, int blipIndex, int blipR, int blipG, int blipB, string destinationStr, string addressStr1, string addressStr2, bool isAsian) { Taximeter.CallFunction("ADD_TAXI_DESTINATION", index, blipIndex, blipR, blipG, blipB, destinationStr, addressStr1, addressStr2, false); } // isasian sempre false.. tanto vale

        public void ClearDisplay() { Taximeter.CallFunction("CLEAR_TAXI_DISPLAY"); }

        public void ShowDestination() { Taximeter.CallFunction("SHOW_TAXI_DESTINATION"); }

        public void SetPrice(int price) { Taximeter.CallFunction("SET_TAXI_PRICE", price.ToString(), false); }

        public void HighlightDestination(bool force) { Taximeter.CallFunction("HIGHLIGHT_DESTINATION", force); }

        // TODO: TRY MAYBE DRAWING THE TAXIMETER SCALEFORM?
        public void RenderMeter()
        {
            SetTextRenderId(meter_rt);
            Set_2dLayer(4);
            SetScriptGfxDrawBehindPausemenu(true);
            DrawScaleformMovie(Taximeter.Handle, 0.201000005f, 0.351f, 0.4f, 0.6f, 0, 0, 0, 255, 0);
            SetTextRenderId(1);
        }

        public Prop CreateTaxiMeter(Vehicle veh)
        {
            Vector3 c = veh.Position;
            int meter = GetClosestObjectOfType(c.X, c.Y, c.Z, 2.0f, (uint)GetHashKey("prop_taxi_meter_2"), false, false, false);

            if (DoesEntityExist(meter)) return new Prop(meter);
            meter = CreateObject(GetHashKey("prop_taxi_meter_2"), c.X, c.Y, c.Z, true, true, false);
            AttachEntityToEntity(meter, veh.Handle, GetEntityBoneIndexByName(veh.Handle, "Chassis"), -0.01f, 0.6f, 0.24f, -5.0f, 0.0f, 0.0f, false, false, false, false, 2, true);

            return new Prop(meter);
        }
    }
}