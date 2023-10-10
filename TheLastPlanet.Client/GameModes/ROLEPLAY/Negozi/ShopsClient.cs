using Settings.Client.Configuration.Negozi.Generici;
using System;
using System.Collections.Generic;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Businesses;
using TheLastPlanet.Client.Handlers;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Shops
{
    internal static class ShopsClient
    {
        private static ConfigGenericShops GenericShops;
        private static List<InputController> tfsInputs = new();
        private static List<InputController> rqInputs = new();
        private static List<InputController> ltdInputs = new();
        private static List<InputController> weapShopsInputs = new();
        private static List<Blip> blips = new List<Blip>();


        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += ShopsSpawn;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            GenericShops = Client.Settings.RolePlay.Shops.GenericShops;
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            GenericShops = null;
            InputHandler.RemoveInputList(tfsInputs);
            InputHandler.RemoveInputList(rqInputs);
            InputHandler.RemoveInputList(ltdInputs);
            InputHandler.RemoveInputList(weapShopsInputs);
            blips.ForEach(x => x.Delete());
            blips.Clear();
            tfsInputs.Clear();
            rqInputs.Clear();
            ltdInputs.Clear();
            weapShopsInputs.Clear();
        }

        public static void ShopsSpawn(PlayerClient client)
        {
            foreach (Vector3 v in GenericShops.Tfs)
            {
                Blip bliptfs = World.CreateBlip(v);
                bliptfs.Sprite = BlipSprite.Store;
                SetBlipDisplay(bliptfs.Handle, 4);
                bliptfs.Scale = 1f;
                bliptfs.Color = BlipColor.Green;
                bliptfs.IsShortRange = true;
                bliptfs.Name = "24/7";
                tfsInputs.Add(new InputController(Control.Context, v.ToPosition(), "Press ~INPUT_CONTEXT~ to access the shop", new((MarkerType)(-1), v.ToPosition(), SColor.Transparent), ServerMode.Roleplay, action: new Action<Ped, object[]>(tfs)));
                blips.Add(bliptfs);
            }

            foreach (Vector3 v in GenericShops.Rq)
            {
                Blip bliptrq = World.CreateBlip(v);
                bliptrq.Sprite = BlipSprite.Store;
                SetBlipDisplay(bliptrq.Handle, 4);
                bliptrq.Scale = 1f;
                bliptrq.Color = BlipColor.Green;
                bliptrq.IsShortRange = true;
                bliptrq.Name = "Robs Liquor";
                rqInputs.Add(new InputController(Control.Context, v.ToPosition(), "Press ~INPUT_CONTEXT~ to access the shop", new((MarkerType)(-1), v.ToPosition(), SColor.Transparent), ServerMode.Roleplay, action: new Action<Ped, object[]>(rq)));
                blips.Add(bliptrq);
            }

            foreach (Vector3 v in GenericShops.Ltd)
            {
                Blip blipltd = World.CreateBlip(v);
                blipltd.Sprite = BlipSprite.Store;
                SetBlipDisplay(blipltd.Handle, 4);
                blipltd.Scale = 1f;
                blipltd.Color = BlipColor.Green;
                blipltd.IsShortRange = true;
                blipltd.Name = "Limited Gasoline";
                ltdInputs.Add(new InputController(Control.Context, v.ToPosition(), "Press ~INPUT_CONTEXT~ to access the shop", new((MarkerType)(-1), v.ToPosition(), SColor.Transparent), ServerMode.Roleplay, action: new Action<Ped, object[]>(ltd)));
                blips.Add(blipltd);
            }

            foreach (Vector3 v in GenericShops.WeaponShops)
            {
                Blip blipWeaps = World.CreateBlip(v);
                blipWeaps.Sprite = BlipSprite.AmmuNation;
                SetBlipDisplay(blipWeaps.Handle, 4);
                blipWeaps.Scale = 1f;
                blipWeaps.Color = BlipColor.Green;
                blipWeaps.IsShortRange = true;
                blipWeaps.Name = "Weapons Shop";
                weapShopsInputs.Add(new InputController(Control.Context, v.ToPosition(), "Press ~INPUT_CONTEXT~ to access the shop", new((MarkerType)(-1), v.ToPosition(), SColor.Transparent), ServerMode.Roleplay, action: new Action<Ped, object[]>(WeaponShops.NewWeaponShop)));
                blips.Add(blipWeaps);
            }

            InputHandler.AddInputList(tfsInputs);
            InputHandler.AddInputList(rqInputs);
            InputHandler.AddInputList(ltdInputs);
            InputHandler.AddInputList(weapShopsInputs);

        }

        private static void tfs(Ped _, object[] args) => BusinessShops.NegozioPubblico("247");
        private static void rq(Ped _, object[] args) => BusinessShops.NegozioPubblico("rq");
        private static void ltd(Ped _, object[] args) => BusinessShops.NegozioPubblico("ltd");
    }
}