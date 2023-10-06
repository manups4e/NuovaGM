using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Handlers;


namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Negozi
{
    internal static class NegozioAbitiClient
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
            AccessingEvents.OnRoleplaySpawn += Spawnato;
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

        public static async void Spawnato(PlayerClient client)
        {
            foreach (NegozioAbiti v in ConfigClothes.Binco)
            {
                Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z)) { Sprite = (BlipSprite)v.BlipId, Color = (BlipColor)v.BlipColor, IsShortRange = true, Name = "Binco's" };
                SetBlipDisplay(blip.Handle, 4);
                bincoInputs.Add(new InputController(Control.Context, v.Vestiti.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare i vestiti", new((MarkerType)(-1), v.Vestiti.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoVest), v.Vestiti.W));
                bincoInputs.Add(new InputController(Control.Context, v.Scarpe.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare le scarpe", new((MarkerType)(-1), v.Scarpe.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoScarpe), v.Scarpe.W));
                bincoInputs.Add(new InputController(Control.Context, v.Pantaloni.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare i pantaloni", new((MarkerType)(-1), v.Pantaloni.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoPant), v.Pantaloni.W));
                bincoInputs.Add(new InputController(Control.Context, v.Occhiali.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare gli occhiali", new((MarkerType)(-1), v.Occhiali.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoOcchiali), v.Occhiali.W));
                bincoInputs.Add(new InputController(Control.Context, v.Accessori.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare gli accessori", new((MarkerType)(-1), v.Accessori.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(BincoAccessori), v.Accessori.W));
                blips.Add(blip);
            }

            foreach (NegozioAbiti v in ConfigClothes.Discount)
            {
                Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z)) { Sprite = (BlipSprite)v.BlipId, Color = (BlipColor)v.BlipColor, IsShortRange = true, Name = "Discount's" };
                SetBlipDisplay(blip.Handle, 4);
                discountInputs.Add(new InputController(Control.Context, v.Vestiti.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare i vestiti", new((MarkerType)(-1), v.Vestiti.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountVest), v.Vestiti.W));
                discountInputs.Add(new InputController(Control.Context, v.Scarpe.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare le scarpe", new((MarkerType)(-1), v.Scarpe.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountScarpe), v.Scarpe.W));
                discountInputs.Add(new InputController(Control.Context, v.Pantaloni.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare i pantaloni", new((MarkerType)(-1), v.Pantaloni.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountPant), v.Pantaloni.W));
                discountInputs.Add(new InputController(Control.Context, v.Occhiali.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare gli occhiali", new((MarkerType)(-1), v.Occhiali.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountOcchiali), v.Occhiali.W));
                discountInputs.Add(new InputController(Control.Context, v.Accessori.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare gli accessori", new((MarkerType)(-1), v.Accessori.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(DiscountAccessori), v.Accessori.W));
                blips.Add(blip);
            }

            foreach (NegozioAbiti v in ConfigClothes.Suburban)
            {
                Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z)) { Sprite = (BlipSprite)v.BlipId, Color = (BlipColor)v.BlipColor, IsShortRange = true, Name = "Suburban's" };
                SetBlipDisplay(blip.Handle, 4);
                suburbanInputs.Add(new InputController(Control.Context, v.Vestiti.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare i vestiti", new((MarkerType)(-1), v.Vestiti.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanVest), v.Vestiti.W));
                suburbanInputs.Add(new InputController(Control.Context, v.Scarpe.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare le scarpe", new((MarkerType)(-1), v.Scarpe.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanScarpe), v.Scarpe.W));
                suburbanInputs.Add(new InputController(Control.Context, v.Pantaloni.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare i pantaloni", new((MarkerType)(-1), v.Pantaloni.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanPant), v.Pantaloni.W));
                suburbanInputs.Add(new InputController(Control.Context, v.Occhiali.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare gli occhiali", new((MarkerType)(-1), v.Occhiali.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanOcchiali), v.Occhiali.W));
                suburbanInputs.Add(new InputController(Control.Context, v.Accessori.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare gli accessori", new((MarkerType)(-1), v.Accessori.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(SuburbanAccessori), v.Accessori.W));
                blips.Add(blip);
            }

            foreach (NegozioAbiti v in ConfigClothes.Ponsombys)
            {
                Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z)) { Sprite = (BlipSprite)v.BlipId, Color = (BlipColor)v.BlipColor, IsShortRange = true, Name = "Ponsombys" };
                SetBlipDisplay(blip.Handle, 4);
                ponsombysInputs.Add(new InputController(Control.Context, v.Vestiti.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare i vestiti", new((MarkerType)(-1), v.Vestiti.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysVest), v.Vestiti.W));
                ponsombysInputs.Add(new InputController(Control.Context, v.Scarpe.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare le scarpe", new((MarkerType)(-1), v.Scarpe.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysScarpe), v.Scarpe.W));
                ponsombysInputs.Add(new InputController(Control.Context, v.Pantaloni.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare i pantaloni", new((MarkerType)(-1), v.Pantaloni.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysPant), v.Pantaloni.W));
                ponsombysInputs.Add(new InputController(Control.Context, v.Occhiali.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare gli occhiali", new((MarkerType)(-1), v.Occhiali.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysOcchiali), v.Occhiali.W));
                ponsombysInputs.Add(new InputController(Control.Context, v.Accessori.ToPosition(), "Premi ~INPUT_CONTEXT~ per guardare gli accessori", new((MarkerType)(-1), v.Accessori.ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(PonsombysAccessori), v.Accessori.W));
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
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuVest(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.BincoVest, "clothingshirt", "Binco");
            else
                MenuNegoziAbiti.MenuVest(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.BincoVest, "mp_clothing@female@shirt", "Binco");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0, 0, 0), true, new Vector3(0));
            menu = true;
        }

        private static async void BincoScarpe(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuScarpe(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.BincoScarpe, "clothingshoes", "Binco");
            else
                MenuNegoziAbiti.MenuScarpe(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.BincoScarpe, "mp_clothing@female@Scarpe", "Binco");
            cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
            menu = true;
        }

        private static async void BincoPant(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuPant(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.BincoPant, "clothingtrousers", "Binco");
            else
                MenuNegoziAbiti.MenuPant(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.BincoPant, "mp_clothing@female@trousers", "Binco");
            cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
            menu = true;
        }

        private static async void BincoOcchiali(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuOcchiali(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.Occhiali, "clothingspecs", "Binco");
            else
                MenuNegoziAbiti.MenuOcchiali(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.Occhiali, "mp_clothing@female@glasses", "Binco");
            cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        private static async void BincoAccessori(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuAccessori(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.Accessori, "clothingshirt", "Binco");
            else
                MenuNegoziAbiti.MenuAccessori(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.Accessori, "mp_clothing@female@shirt", "Binco");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        #endregion

        #region Discount Events

        private static async void DiscountVest(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuVest(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.DiscVest, "clothingshirt", "Discount");
            else
                MenuNegoziAbiti.MenuVest(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.DiscVest, "mp_clothing@female@shirt", "Discount");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0, 0, 0), true, new Vector3(0));
            menu = true;
        }

        private static async void DiscountScarpe(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuScarpe(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.DiscScarpe, "clothingshoes", "Discount");
            else
                MenuNegoziAbiti.MenuScarpe(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.DiscScarpe, "mp_clothing@female@Scarpe", "Discount");
            cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
            menu = true;
        }

        private static async void DiscountPant(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuPant(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.DiscPant, "clothingtrousers", "Discount");
            else
                MenuNegoziAbiti.MenuPant(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.DiscPant, "mp_clothing@female@trousers", "Discount");
            cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
            menu = true;
        }

        private static async void DiscountOcchiali(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuOcchiali(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.Occhiali, "clothingspecs", "Discount");
            else
                MenuNegoziAbiti.MenuOcchiali(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.Occhiali, "mp_clothing@female@glasses", "Discount");
            cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        private static async void DiscountAccessori(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuAccessori(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.Accessori, "clothingshirt", "Discount");
            else
                MenuNegoziAbiti.MenuAccessori(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.Accessori, "mp_clothing@female@shirt", "Discount");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        #endregion

        #region Suburban Events

        private static async void SuburbanVest(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuVest(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.SubVest, "clothingshirt", "Suburban");
            else
                MenuNegoziAbiti.MenuVest(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.SubVest, "mp_clothing@female@shirt", "Suburban");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0, 0, 0), true, new Vector3(0));
            menu = true;
        }

        private static async void SuburbanScarpe(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuScarpe(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.SubScarpe, "clothingshoes", "Suburban");
            else
                MenuNegoziAbiti.MenuScarpe(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.SubScarpe, "mp_clothing@female@Scarpe", "Suburban");
            cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
            menu = true;
        }

        private static async void SuburbanPant(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuPant(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.SubPant, "clothingtrousers", "Suburban");
            else
                MenuNegoziAbiti.MenuPant(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.SubPant, "mp_clothing@female@trousers", "Suburban");
            cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
            menu = true;
        }

        private static async void SuburbanOcchiali(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuOcchiali(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.Occhiali, "clothingspecs", "Suburban");
            else
                MenuNegoziAbiti.MenuOcchiali(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.Occhiali, "mp_clothing@female@glasses", "Suburban");
            cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        private static async void SuburbanAccessori(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuAccessori(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.Accessori, "clothingshirt", "Suburban");
            else
                MenuNegoziAbiti.MenuAccessori(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.Accessori, "mp_clothing@female@shirt", "Suburban");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        #endregion

        #region Ponsombys Events

        private static async void PonsombysVest(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuVest(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.PonsVest, "clothingshirt", "Ponsombys");
            else
                MenuNegoziAbiti.MenuVest(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.PonsVest, "mp_clothing@female@shirt", "Ponsombys");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0, 0, 0), true, new Vector3(0));
            menu = true;
        }

        private static async void PonsombysScarpe(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuScarpe(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.PonsScarpe, "clothingshoes", "Ponsombys");
            else
                MenuNegoziAbiti.MenuScarpe(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.PonsScarpe, "mp_clothing@female@Scarpe", "Ponsombys");
            cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
            menu = true;
        }

        private static async void PonsombysPant(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuPant(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.PonsPant, "clothingtrousers", "Ponsombys");
            else
                MenuNegoziAbiti.MenuPant(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.PonsPant, "mp_clothing@female@trousers", "Ponsombys");
            cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
            menu = true;
        }

        private static async void PonsombysOcchiali(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuOcchiali(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.Occhiali, "clothingspecs", "Ponsombys");
            else
                MenuNegoziAbiti.MenuOcchiali(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.Occhiali, "mp_clothing@female@glasses", "Ponsombys");
            cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        private static async void PonsombysAccessori(Ped p, object[] args)
        {
            p.Task.AchieveHeading((float)args[0]);
            while (p.Heading > (float)args[0] + 5f || p.Heading < (float)args[0] - 5f) await BaseScript.Delay(0);
            if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex == "Maschio")
                MenuNegoziAbiti.MenuAccessori(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio.Accessori, "clothingshirt", "Ponsombys");
            else
                MenuNegoziAbiti.MenuAccessori(Client.Impostazioni.RolePlay.Negozi.Abiti.Femmina.Accessori, "mp_clothing@female@shirt", "Ponsombys");
            cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
            menu = true;
        }

        #endregion

        public static async Task Esci()
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
						Funzioni.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare le Maglie");
						if (Input.IsControlJustPressed(Control.Context))
						{
							p.Task.AchieveHeading(v.Maglie.W);
							if (Cache.Char.CurrentChar.skin.sex == "Maschio")
								//ApriMenu(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio);
								Debug.WriteLine("Maschio");
							else
								//ApriMenu(Client.Impostazioni.RolePlay.Negozi.Abiti.Maschio);
								Debug.WriteLine("Femmina");
							//cam(24818, new Vector3(0), new Vector3(0), true, new Vector3(0));
							//menu = true;
						}
					}
*/