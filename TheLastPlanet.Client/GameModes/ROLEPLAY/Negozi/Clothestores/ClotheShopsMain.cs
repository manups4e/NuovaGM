using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Handlers;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Shops
{
    internal static class ClotheShopsMain
    {
        private static bool menu = false;
        public static Camera camm = new Camera(0);
        private static List<InputController> bincoInputs = new();
        private static List<InputController> discountInputs = new();
        private static List<InputController> suburbanInputs = new();
        private static List<InputController> ponsombysInputs = new();
        private static List<Blip> blips = new();


        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            InputHandler.RemoveInputList(bincoInputs);
            InputHandler.RemoveInputList(discountInputs);
            InputHandler.RemoveInputList(suburbanInputs);
            InputHandler.RemoveInputList(ponsombysInputs);
            blips.ForEach(x => x.Delete());
            bincoInputs.Clear();
            discountInputs.Clear();
            suburbanInputs.Clear();
            ponsombysInputs.Clear();
            blips.Clear();
        }

        public static async void Spawned(PlayerClient client)
        {
            foreach (ClotheShop v in ConfigClothes.Binco)
            {
                Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z)) { Sprite = (BlipSprite)v.BlipId, Color = (BlipColor)v.BlipColor, IsShortRange = true, Name = "Binco's" };
                SetBlipDisplay(blip.Handle, 4);
                bincoInputs.Add(new InputController(Control.Context, v.Clothes.ToPosition(), "Press ~INPUT_CONTEXT~ to check the clothes", new((MarkerType)(-1), v.Clothes.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoVest), v.Clothes.W));
                bincoInputs.Add(new InputController(Control.Context, v.Shoes.ToPosition(), "Press ~INPUT_CONTEXT~ to check the shoes", new((MarkerType)(-1), v.Shoes.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoScarpe), v.Shoes.W));
                bincoInputs.Add(new InputController(Control.Context, v.Pants.ToPosition(), "Press ~INPUT_CONTEXT~ to check the pants", new((MarkerType)(-1), v.Pants.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoPant), v.Pants.W));
                bincoInputs.Add(new InputController(Control.Context, v.Glasses.ToPosition(), "Press ~INPUT_CONTEXT~ to check the glasses", new((MarkerType)(-1), v.Glasses.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoOcchiali), v.Glasses.W));
                bincoInputs.Add(new InputController(Control.Context, v.Accessories.ToPosition(), "Press ~INPUT_CONTEXT~ to check the accessories", new((MarkerType)(-1), v.Accessories.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoAccessori), v.Accessories.W));
                blips.Add(blip);
            }

            foreach (ClotheShop v in ConfigClothes.Discount)
            {
                Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z)) { Sprite = (BlipSprite)v.BlipId, Color = (BlipColor)v.BlipColor, IsShortRange = true, Name = "Discount's" };
                SetBlipDisplay(blip.Handle, 4);
                discountInputs.Add(new InputController(Control.Context, v.Clothes.ToPosition(), "Press ~INPUT_CONTEXT~ to check the clothes", new((MarkerType)(-1), v.Clothes.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountVest), v.Clothes.W));
                discountInputs.Add(new InputController(Control.Context, v.Shoes.ToPosition(), "Press ~INPUT_CONTEXT~ to check the shoes", new((MarkerType)(-1), v.Shoes.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountScarpe), v.Shoes.W));
                discountInputs.Add(new InputController(Control.Context, v.Pants.ToPosition(), "Press ~INPUT_CONTEXT~ to check the pants", new((MarkerType)(-1), v.Pants.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountPant), v.Pants.W));
                discountInputs.Add(new InputController(Control.Context, v.Glasses.ToPosition(), "Press ~INPUT_CONTEXT~ to check the glasses", new((MarkerType)(-1), v.Glasses.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountOcchiali), v.Glasses.W));
                discountInputs.Add(new InputController(Control.Context, v.Accessories.ToPosition(), "Press ~INPUT_CONTEXT~ to check the accessories", new((MarkerType)(-1), v.Accessories.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountAccessori), v.Accessories.W));
                blips.Add(blip);
            }

            foreach (ClotheShop v in ConfigClothes.Suburban)
            {
                Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z)) { Sprite = (BlipSprite)v.BlipId, Color = (BlipColor)v.BlipColor, IsShortRange = true, Name = "Suburban's" };
                SetBlipDisplay(blip.Handle, 4);
                suburbanInputs.Add(new InputController(Control.Context, v.Clothes.ToPosition(), "Press ~INPUT_CONTEXT~ to check the clothes", new((MarkerType)(-1), v.Clothes.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanVest), v.Clothes.W));
                suburbanInputs.Add(new InputController(Control.Context, v.Shoes.ToPosition(), "Press ~INPUT_CONTEXT~ to check the shoes", new((MarkerType)(-1), v.Shoes.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanScarpe), v.Shoes.W));
                suburbanInputs.Add(new InputController(Control.Context, v.Pants.ToPosition(), "Press ~INPUT_CONTEXT~ to check the pants" +
                    "", new((MarkerType)(-1), v.Pants.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanPant), v.Pants.W));
                suburbanInputs.Add(new InputController(Control.Context, v.Glasses.ToPosition(), "Press ~INPUT_CONTEXT~ to check the glasses", new((MarkerType)(-1), v.Glasses.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanOcchiali), v.Glasses.W));
                suburbanInputs.Add(new InputController(Control.Context, v.Accessories.ToPosition(), "Press ~INPUT_CONTEXT~ to check the accessories", new((MarkerType)(-1), v.Accessories.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanAccessori), v.Accessories.W));
                blips.Add(blip);
            }

            foreach (ClotheShop v in ConfigClothes.Ponsombys)
            {
                Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z)) { Sprite = (BlipSprite)v.BlipId, Color = (BlipColor)v.BlipColor, IsShortRange = true, Name = "Ponsombys" };
                SetBlipDisplay(blip.Handle, 4);
                ponsombysInputs.Add(new InputController(Control.Context, v.Clothes.ToPosition(), "Press ~INPUT_CONTEXT~ to check the clothes", new((MarkerType)(-1), v.Clothes.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysVest), v.Clothes.W));
                ponsombysInputs.Add(new InputController(Control.Context, v.Shoes.ToPosition(), "Press ~INPUT_CONTEXT~ to check the shoes", new((MarkerType)(-1), v.Shoes.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysScarpe), v.Shoes.W));
                ponsombysInputs.Add(new InputController(Control.Context, v.Pants.ToPosition(), "Press ~INPUT_CONTEXT~ to check the pants" +
                    "", new((MarkerType)(-1), v.Pants.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysPant), v.Pants.W));
                ponsombysInputs.Add(new InputController(Control.Context, v.Glasses.ToPosition(), "Press ~INPUT_CONTEXT~ to check the glasses", new((MarkerType)(-1), v.Glasses.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysOcchiali), v.Glasses.W));
                ponsombysInputs.Add(new InputController(Control.Context, v.Accessories.ToPosition(), "Press ~INPUT_CONTEXT~ to check the accessories", new((MarkerType)(-1), v.Accessories.ToPosition(), SColor.Transparent), ServerMode.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysAccessori), v.Accessories.W));
                blips.Add(blip);
            }
            InputHandler.AddInputList(bincoInputs);
            InputHandler.AddInputList(discountInputs);
            InputHandler.AddInputList(suburbanInputs);
            InputHandler.AddInputList(ponsombysInputs);
        }

        public static async void cam(int bone, Vector3 off, Vector3 c, bool toggle, Vector3 z)
        {
            if (camm.Exists() && camm.IsActive) return;
            Vector3 xyz = Cache.PlayerCache.MyPlayer.Ped.Bones[(Bone)bone].Position;
            Vector3 offC = Cache.PlayerCache.MyPlayer.Ped.GetOffsetPosition(off);
            camm = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
            camm.Position = new Vector3(c.X + offC.X, c.Y + offC.Y, xyz.Z + c.Z);
            if (toggle)
                camm.PointAt(Cache.PlayerCache.MyPlayer.Ped.Bones[bone], new Vector3(0));
            else
                camm.PointAt(new Vector3(xyz.X - z.X, xyz.Y - z.Y, xyz.Z - z.Z));
            camm.FieldOfView = 45.0f;
            SetCamParams(camm.Handle, offC.X + c.X, offC.Y + c.Y, xyz.Z + c.Z, 0.0f, 0.0f, 0.0f, 45.0f, 0, 1, 1, 2);
            camm.IsActive = true;
            RenderScriptCams(true, true, 1500, true, false);
        }

        #region BincoEvents

        private static async void BincoVest(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuClothes(Client.Settings.RolePlay.Shops.Clothes.Male.BincoSuit, "clothingshirt", "Binco");
            else
                MenuClotheshops.MenuClothes(Client.Settings.RolePlay.Shops.Clothes.Female.BincoSuit, "mp_clothing@female@shirt", "Binco");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0, 0, 0), true, new Vector3(0));
            menu = true;
        }

        private static async void BincoScarpe(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuShoes(Client.Settings.RolePlay.Shops.Clothes.Male.BincoShoes, "clothingshoes", "Binco");
            else
                MenuClotheshops.MenuShoes(Client.Settings.RolePlay.Shops.Clothes.Female.BincoShoes, "mp_clothing@female@Scarpe", "Binco");
            cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
            menu = true;
        }

        private static async void BincoPant(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuPants(Client.Settings.RolePlay.Shops.Clothes.Male.BincoPants, "clothingtrousers", "Binco");
            else
                MenuClotheshops.MenuPants(Client.Settings.RolePlay.Shops.Clothes.Female.BincoPants, "mp_clothing@female@trousers", "Binco");
            cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
            menu = true;
        }

        private static async void BincoOcchiali(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuOcchiali(Client.Settings.RolePlay.Shops.Clothes.Male.Glasses, "clothingspecs", "Binco");
            else
                MenuClotheshops.MenuOcchiali(Client.Settings.RolePlay.Shops.Clothes.Female.Glasses, "mp_clothing@female@glasses", "Binco");
            cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        private static async void BincoAccessori(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.AccessoriesMenu(Client.Settings.RolePlay.Shops.Clothes.Male.Accessories, "clothingshirt", "Binco");
            else
                MenuClotheshops.AccessoriesMenu(Client.Settings.RolePlay.Shops.Clothes.Female.Accessories, "mp_clothing@female@shirt", "Binco");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        #endregion

        #region Discount Events

        private static async void DiscountVest(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuClothes(Client.Settings.RolePlay.Shops.Clothes.Male.DiscSuit, "clothingshirt", "Discount");
            else
                MenuClotheshops.MenuClothes(Client.Settings.RolePlay.Shops.Clothes.Female.DiscSuit, "mp_clothing@female@shirt", "Discount");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0, 0, 0), true, new Vector3(0));
            menu = true;
        }

        private static async void DiscountScarpe(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuShoes(Client.Settings.RolePlay.Shops.Clothes.Male.DiscShoes, "clothingshoes", "Discount");
            else
                MenuClotheshops.MenuShoes(Client.Settings.RolePlay.Shops.Clothes.Female.DiscShoes, "mp_clothing@female@Scarpe", "Discount");
            cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
            menu = true;
        }

        private static async void DiscountPant(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuPants(Client.Settings.RolePlay.Shops.Clothes.Male.DiscPants, "clothingtrousers", "Discount");
            else
                MenuClotheshops.MenuPants(Client.Settings.RolePlay.Shops.Clothes.Female.DiscPants, "mp_clothing@female@trousers", "Discount");
            cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
            menu = true;
        }

        private static async void DiscountOcchiali(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuOcchiali(Client.Settings.RolePlay.Shops.Clothes.Male.Glasses, "clothingspecs", "Discount");
            else
                MenuClotheshops.MenuOcchiali(Client.Settings.RolePlay.Shops.Clothes.Female.Glasses, "mp_clothing@female@glasses", "Discount");
            cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        private static async void DiscountAccessori(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.AccessoriesMenu(Client.Settings.RolePlay.Shops.Clothes.Male.Accessories, "clothingshirt", "Discount");
            else
                MenuClotheshops.AccessoriesMenu(Client.Settings.RolePlay.Shops.Clothes.Female.Accessories, "mp_clothing@female@shirt", "Discount");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        #endregion

        #region Suburban Events

        private static async void SuburbanVest(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuClothes(Client.Settings.RolePlay.Shops.Clothes.Male.SubSuit, "clothingshirt", "Suburban");
            else
                MenuClotheshops.MenuClothes(Client.Settings.RolePlay.Shops.Clothes.Female.SubSuit, "mp_clothing@female@shirt", "Suburban");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0, 0, 0), true, new Vector3(0));
            menu = true;
        }

        private static async void SuburbanScarpe(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuShoes(Client.Settings.RolePlay.Shops.Clothes.Male.SubShoes, "clothingshoes", "Suburban");
            else
                MenuClotheshops.MenuShoes(Client.Settings.RolePlay.Shops.Clothes.Female.SubShoes, "mp_clothing@female@Scarpe", "Suburban");
            cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
            menu = true;
        }

        private static async void SuburbanPant(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuPants(Client.Settings.RolePlay.Shops.Clothes.Male.SubPants, "clothingtrousers", "Suburban");
            else
                MenuClotheshops.MenuPants(Client.Settings.RolePlay.Shops.Clothes.Female.SubPants, "mp_clothing@female@trousers", "Suburban");
            cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
            menu = true;
        }

        private static async void SuburbanOcchiali(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuOcchiali(Client.Settings.RolePlay.Shops.Clothes.Male.Glasses, "clothingspecs", "Suburban");
            else
                MenuClotheshops.MenuOcchiali(Client.Settings.RolePlay.Shops.Clothes.Female.Glasses, "mp_clothing@female@glasses", "Suburban");
            cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        private static async void SuburbanAccessori(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.AccessoriesMenu(Client.Settings.RolePlay.Shops.Clothes.Male.Accessories, "clothingshirt", "Suburban");
            else
                MenuClotheshops.AccessoriesMenu(Client.Settings.RolePlay.Shops.Clothes.Female.Accessories, "mp_clothing@female@shirt", "Suburban");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        #endregion

        #region Ponsombys Events

        private static async void PonsombysVest(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuClothes(Client.Settings.RolePlay.Shops.Clothes.Male.PonsSuit, "clothingshirt", "Ponsombys");
            else
                MenuClotheshops.MenuClothes(Client.Settings.RolePlay.Shops.Clothes.Female.PonsSuit, "mp_clothing@female@shirt", "Ponsombys");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0, 0, 0), true, new Vector3(0));
            menu = true;
        }

        private static async void PonsombysScarpe(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuShoes(Client.Settings.RolePlay.Shops.Clothes.Male.PonsShoes, "clothingshoes", "Ponsombys");
            else
                MenuClotheshops.MenuShoes(Client.Settings.RolePlay.Shops.Clothes.Female.PonsShoes, "mp_clothing@female@Scarpe", "Ponsombys");
            cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
            menu = true;
        }

        private static async void PonsombysPant(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuPants(Client.Settings.RolePlay.Shops.Clothes.Male.PonsPants, "clothingtrousers", "Ponsombys");
            else
                MenuClotheshops.MenuPants(Client.Settings.RolePlay.Shops.Clothes.Female.PonsPants, "mp_clothing@female@trousers", "Ponsombys");
            cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
            menu = true;
        }

        private static async void PonsombysOcchiali(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.MenuOcchiali(Client.Settings.RolePlay.Shops.Clothes.Male.Glasses, "clothingspecs", "Ponsombys");
            else
                MenuClotheshops.MenuOcchiali(Client.Settings.RolePlay.Shops.Clothes.Female.Glasses, "mp_clothing@female@glasses", "Ponsombys");
            cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        private static async void PonsombysAccessori(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex == "Male")
                MenuClotheshops.AccessoriesMenu(Client.Settings.RolePlay.Shops.Clothes.Male.Accessories, "clothingshirt", "Ponsombys");
            else
                MenuClotheshops.AccessoriesMenu(Client.Settings.RolePlay.Shops.Clothes.Female.Accessories, "mp_clothing@female@shirt", "Ponsombys");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        #endregion

        public static async Task Exit()
        {
            int id = PlayerPedId();
            menu = false;
            ClearPedTasks(id);
            ClearPedSecondaryTask(id);
            SetBlockingOfNonTemporaryEvents(id, false);
            ClearPedAlternateMovementAnim(id, 0, 4.0f);

            if (!menu)
            {
                RenderScriptCams(false, true, 1500, true, false);
                camm.IsActive = false;
                DestroyAllCams(false);
                SetFrozenRenderingDisabled(true);
            }

            await Task.FromResult(0);
        }
    }
}

// PER LE MAGLIE

/*					else if (p.IsInRangeOf(new Vector3(v.Maglie.X, v.Maglie.Y, v.Maglie.Z), 0.8f))
					{
						Funzioni.ShowHelp("Press ~INPUT_CONTEXT~ to check le Maglie");
						if (Input.IsControlJustPressed(Control.Context))
						{
							p.Task.AchieveHeading(v.Maglie.W);
							if (Cache.Char.CurrentChar.skin.sex == "Male")
								//ApriMenu(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio);
								Debug.WriteLine("Male");
							else
								//ApriMenu(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio);
								Debug.WriteLine("Female");
							//cam(24818, new Vector3(0), new Vector3(0), true, new Vector3(0));
							//menu = true;
						}
					}
*/