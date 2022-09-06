using Impostazioni.Client.Configurazione.Negozi.Generici;
using System;
using System.Collections.Generic;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Businesses;


namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Negozi
{
    internal static class NegoziClient
    {
        private static ConfigNegoziGenerici NegoziGenerici;
        private static List<InputController> tfsInputs = new();
        private static List<InputController> rqInputs = new();
        private static List<InputController> ltdInputs = new();
        private static List<InputController> armerieInputs = new();
        private static List<Blip> blips = new List<Blip>();


        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += NegoziSpawn;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            NegoziGenerici = Client.Impostazioni.RolePlay.Negozi.NegoziGenerici;
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            NegoziGenerici = null;
            InputHandler.RemoveInputList(tfsInputs);
            InputHandler.RemoveInputList(rqInputs);
            InputHandler.RemoveInputList(ltdInputs);
            InputHandler.RemoveInputList(armerieInputs);
            blips.ForEach(x => x.Delete());
            blips.Clear();
            tfsInputs.Clear();
            rqInputs.Clear();
            ltdInputs.Clear();
            armerieInputs.Clear();
        }

        public static void NegoziSpawn(PlayerClient client)
        {
            foreach (Vector3 v in NegoziGenerici.tfs)
            {
                Blip bliptfs = World.CreateBlip(v);
                bliptfs.Sprite = BlipSprite.Store;
                SetBlipDisplay(bliptfs.Handle, 4);
                bliptfs.Scale = 1f;
                bliptfs.Color = BlipColor.Green;
                bliptfs.IsShortRange = true;
                bliptfs.Name = "24/7";
                tfsInputs.Add(new InputController(Control.Context, v.ToPosition(), "Premi ~INPUT_CONTEXT~ per accedere al negozio", new((MarkerType)(-1), v.ToPosition(), ScaleformUI.Colors.Transparent), ModalitaServer.Roleplay, action: new Action<Ped, object[]>(tfs)));
                blips.Add(bliptfs);
            }

            foreach (Vector3 v in NegoziGenerici.rq)
            {
                Blip bliptrq = World.CreateBlip(v);
                bliptrq.Sprite = BlipSprite.Store;
                SetBlipDisplay(bliptrq.Handle, 4);
                bliptrq.Scale = 1f;
                bliptrq.Color = BlipColor.Green;
                bliptrq.IsShortRange = true;
                bliptrq.Name = "Robs Liquor";
                rqInputs.Add(new InputController(Control.Context, v.ToPosition(), "Premi ~INPUT_CONTEXT~ per accedere al negozio", new((MarkerType)(-1), v.ToPosition(), ScaleformUI.Colors.Transparent), ModalitaServer.Roleplay, action: new Action<Ped, object[]>(rq)));
                blips.Add(bliptrq);
            }

            foreach (Vector3 v in NegoziGenerici.ltd)
            {
                Blip blipltd = World.CreateBlip(v);
                blipltd.Sprite = BlipSprite.Store;
                SetBlipDisplay(blipltd.Handle, 4);
                blipltd.Scale = 1f;
                blipltd.Color = BlipColor.Green;
                blipltd.IsShortRange = true;
                blipltd.Name = "Limited Gasoline";
                ltdInputs.Add(new InputController(Control.Context, v.ToPosition(), "Premi ~INPUT_CONTEXT~ per accedere al negozio", new((MarkerType)(-1), v.ToPosition(), ScaleformUI.Colors.Transparent), ModalitaServer.Roleplay, action: new Action<Ped, object[]>(ltd)));
                blips.Add(blipltd);
            }

            foreach (Vector3 v in NegoziGenerici.armerie)
            {
                Blip bliparmi = World.CreateBlip(v);
                bliparmi.Sprite = BlipSprite.AmmuNation;
                SetBlipDisplay(bliparmi.Handle, 4);
                bliparmi.Scale = 1f;
                bliparmi.Color = BlipColor.Green;
                bliparmi.IsShortRange = true;
                bliparmi.Name = "Armeria";
                armerieInputs.Add(new InputController(Control.Context, v.ToPosition(), "Premi ~INPUT_CONTEXT~ per accedere all'armeria", new((MarkerType)(-1), v.ToPosition(), ScaleformUI.Colors.Transparent), ModalitaServer.Roleplay, action: new Action<Ped, object[]>(Armerie.NuovaArmeria)));
                blips.Add(bliparmi);
            }

            InputHandler.AddInputList(tfsInputs);
            InputHandler.AddInputList(rqInputs);
            InputHandler.AddInputList(ltdInputs);
            InputHandler.AddInputList(armerieInputs);

        }

        private static void tfs(Ped _, object[] args) { NegoziBusiness.NegozioPubblico("247"); }
        private static void rq(Ped _, object[] args) { NegoziBusiness.NegozioPubblico("rq"); }
        private static void ltd(Ped _, object[] args) { NegoziBusiness.NegozioPubblico("ltd"); }
    }
}