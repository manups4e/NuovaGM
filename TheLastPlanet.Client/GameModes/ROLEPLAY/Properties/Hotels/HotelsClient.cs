using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.Handlers;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Properties.Hotel
{
    internal static class HotelsClient
    {
        private static Vector3 OldPos;
        private static bool IsInPiccola;
        private static bool IsInMedia;
        private static bool IsInAppartamento;
        private static List<InputController> hotelInputs = new();
        private static List<Blip> blips = new();

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }

        public static void Spawned(PlayerClient client)
        {
            foreach (Hotel t in Client.Settings.RolePlay.Properties.hotels) hotelInputs.Add(new InputController(Control.Context, t.Coords.ToPosition(), $"~INPUT_CONTEXT~ to stay at ~b~{t.Name}~w~.", new((MarkerType)(-1), t.Coords.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(MenuHotel), t));
            InputHandler.AddInputList(hotelInputs);
            /*
            RegisterCommand("hash", new Action<int, List<dynamic>, string>((id, hash, comando) =>
            {
                Client.Logger.Debug("Hash = " + GetHashKey(hash[0] + ""));
            }), false);
            RegisterCommand("weaphash", new Action<int, List<dynamic>, string>(async (id, hash, comando) =>
            {
                RequestWeaponAsset(Functions.HashUint(hash[0]), 31, 0);
                while (!HasWeaponAssetLoaded(Functions.HashUint(hash[0]))) await BaseScript.Delay(0);
                Prop pickupObject = new Prop(CreateWeaponObject(Functions.HashUint(hash[0]), 50, Cache.PlayerCache.MyPlayer.Position.X, Cache.PlayerCache.MyPlayer.Position.Y, Cache.PlayerCache.MyPlayer.Position.Z, true, 1.0f, 0));
                Client.Logger.Debug("Hash = " + pickupObject.Model.Hash);
            }), false);
            */

            foreach (Blip p in Client.Settings.RolePlay.Properties.hotels.Select(hotel => new Blip(AddBlipForCoord(hotel.Coords[0], hotel.Coords[1], hotel.Coords[2]))
            {
                Sprite = BlipSprite.Heist,
                Scale = 1.0f,
                Color = BlipColor.Yellow,
                IsShortRange = true,
                Name = "Hotel"
            })) { blips.Add(p); }
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            InputHandler.RemoveInputList(hotelInputs);
            blips.ForEach(x => x.Delete());
            blips.Clear();
            hotelInputs.Clear();
        }

        private static async void MenuHotel(Ped _, object[] args)
        {
            Hotel hotel = (Hotel)args[0];
            UIMenu HotelMenu = new UIMenu(hotel.Name, "Welcome.", new System.Drawing.PointF(50, 50), "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem smallRoom = new UIMenuItem("Small room", "It's cheap.. and has a bed..");
            smallRoom.SetRightLabel((Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prices.SmallRoom || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prices.SmallRoom ? "~g~$" : "~r~$") + hotel.Prices.SmallRoom);
            UIMenuItem midRoom = new UIMenuItem("Medium room", "It costs a little more.. and is a little more comfortable");
            midRoom.SetRightLabel((Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prices.MidRoom || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prices.MidRoom ? "~g~$" : "~r~$") + hotel.Prices.MidRoom);
            UIMenuItem apartment = new UIMenuItem("Apartment", "You would like to live there... but sooner or later you will have to leave!");
            apartment.SetRightLabel((Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prices.Apartment || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prices.Apartment ? "~g~$" : "~r~$") + hotel.Prices.Apartment);
            HotelMenu.AddItem(smallRoom);
            HotelMenu.AddItem(midRoom);
            HotelMenu.AddItem(apartment);
            HotelMenu.OnItemSelect += async (menu, item, index) =>
            {
                Vector3 pos = new Vector3(0);

                if (item == smallRoom)
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prices.SmallRoom || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prices.SmallRoom)
                    {
                        BaseScript.TriggerServerEvent(Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prices.SmallRoom ? "lprp:removemoney" : "lprp:removebank", hotel.Prices.SmallRoom);
                        pos = new Vector3(266.094f, -1007.487f, -101.800f);
                        IsInPiccola = true;
                    }
                    else
                    {
                        HUD.ShowNotification("Not enough funds!", ColoreNotifica.Red, true);
                    }
                }
                else if (item == midRoom)
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prices.MidRoom || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prices.MidRoom)
                    {
                        BaseScript.TriggerServerEvent(Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prices.MidRoom ? "lprp:removemoney" : "lprp:removebank", hotel.Prices.MidRoom);
                        pos = new Vector3(346.493f, -1013.031f, -99.196f);
                        IsInMedia = true;
                    }
                    else
                    {
                        HUD.ShowNotification("Not enough funds!", ColoreNotifica.Red, true);
                    }
                }
                else if (item == apartment)
                {
                    if (Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prices.Apartment || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prices.Apartment)
                    {
                        BaseScript.TriggerServerEvent(Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prices.Apartment ? "lprp:removemoney" : "lprp:removebank", hotel.Prices.Apartment);
                        pos = new Vector3(-1452.841f, -539.489f, 74.044f);
                        IsInAppartamento = true;
                    }
                    else
                    {
                        HUD.ShowNotification("Not enough funds!", ColoreNotifica.Red, true);
                    }
                }

                menu.Visible = false;
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                OldPos = Cache.PlayerCache.MyPlayer.Position.ToVector3;
                RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
                Cache.PlayerCache.MyPlayer.Ped.Position = pos;
                await BaseScript.Delay(2000);
                //TODO: INSTANCE MUST BE UNIQUE OR ALL PLAYERS WILL FIND THEMSELVES IN THE SAME HOTEL INSTANCE
                Cache.PlayerCache.MyPlayer.Status.Instance.InstancePlayer("Hotel");
                Cache.PlayerCache.MyPlayer.Status.RolePlayStates.InHome = true;
                Screen.Fading.FadeIn(800);
                Client.Instance.AddTick(HotelHandler);
            };
            HotelMenu.Visible = true;
        }

        public static async Task HotelHandler()
        {
            if (!MenuHandler.IsAnyMenuOpen)
            {
                if (IsInPiccola)
                    if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(new Vector3(266.094f, -1007.487f, -101.800f), 1.3f))
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to exit your room");

                        if (Input.IsControlJustPressed(Control.Context))
                        {
                            Screen.Fading.FadeOut(800);
                            await BaseScript.Delay(1000);
                            RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
                            Cache.PlayerCache.MyPlayer.Ped.Position = OldPos;
                            await BaseScript.Delay(2000);
                            Functions.RevealAllPlayers();
                            Screen.Fading.FadeIn(800);
                            IsInPiccola = false;
                            Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
                            Cache.PlayerCache.MyPlayer.Status.RolePlayStates.InHome = false;
                            Client.Instance.RemoveTick(HotelHandler);
                            BaseScript.TriggerEvent("lprp:StartLocationSave");
                        }
                    }

                if (IsInMedia)
                    if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(new Vector3(346.493f, -1013.031f, -99.196f), 1.3f))
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to exit your room");

                        if (Input.IsControlJustPressed(Control.Context))
                        {
                            Screen.Fading.FadeOut(800);
                            await BaseScript.Delay(1000);
                            RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
                            Cache.PlayerCache.MyPlayer.Ped.Position = OldPos;
                            await BaseScript.Delay(2000);
                            Functions.RevealAllPlayers();
                            Screen.Fading.FadeIn(800);
                            IsInMedia = false;
                            Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
                            Cache.PlayerCache.MyPlayer.Status.RolePlayStates.InHome = false;
                            Client.Instance.RemoveTick(HotelHandler);
                            BaseScript.TriggerEvent("lprp:StartLocationSave");
                        }
                    }

                if (IsInAppartamento)
                    if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(new Vector3(-1452.164f, -540.640f, 74.044f), 1.3f))
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ per uscire dall'appartamento");

                        if (Input.IsControlJustPressed(Control.Context))
                        {
                            Screen.Fading.FadeOut(800);
                            await BaseScript.Delay(1000);
                            RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
                            Cache.PlayerCache.MyPlayer.Ped.Position = OldPos;
                            await BaseScript.Delay(2000);
                            Functions.RevealAllPlayers();
                            Screen.Fading.FadeIn(800);
                            IsInAppartamento = false;
                            Cache.PlayerCache.MyPlayer.Status.Instance.RemoveInstance();
                            Cache.PlayerCache.MyPlayer.Status.RolePlayStates.InHome = false;
                            Client.Instance.RemoveTick(HotelHandler);
                            BaseScript.TriggerEvent("lprp:StartLocationSave");
                        }
                    }
            }
        }
    }
}