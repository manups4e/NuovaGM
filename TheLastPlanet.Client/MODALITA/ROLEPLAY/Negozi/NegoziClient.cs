using CitizenFX.Core;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostazioni.Client.Configurazione.Negozi.Generici;
using static CitizenFX.Core.Native.API;
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

		public static void Init()
		{
			Client.Instance.AddEventHandler("tlg:roleplay:onPlayerSpawn", new Action(NegoziSpawn));
			NegoziGenerici = Client.Impostazioni.RolePlay.Negozi.NegoziGenerici;
		}

		public static void Stop()
		{
			Client.Instance.RemoveEventHandler("tlg:roleplay:onPlayerSpawn", new Action(NegoziSpawn));
			NegoziGenerici = null;
			InputHandler.RemoveInputList(tfsInputs);
			InputHandler.RemoveInputList(rqInputs);
			InputHandler.RemoveInputList(ltdInputs);
			InputHandler.RemoveInputList(armerieInputs);
			tfsInputs.Clear();
			rqInputs.Clear();
			ltdInputs.Clear();
			armerieInputs.Clear();
		}

		public static void NegoziSpawn()
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
				tfsInputs.Add(new InputController(Control.Context, v.ToPosition(), "Premi ~INPUT_CONTEXT~ per accedere al negozio", null, ModalitaServer.Roleplay, action: new Action<Ped, object[]>(tfs)));
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
				rqInputs.Add(new InputController(Control.Context, v.ToPosition(), "Premi ~INPUT_CONTEXT~ per accedere al negozio", null, ModalitaServer.Roleplay, action: new Action<Ped, object[]>(rq)));
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
				ltdInputs.Add(new InputController(Control.Context, v.ToPosition(), "Premi ~INPUT_CONTEXT~ per accedere al negozio", null, ModalitaServer.Roleplay, action: new Action<Ped, object[]>(ltd)));
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
				armerieInputs.Add(new InputController(Control.Context, v.ToPosition(), "Premi ~INPUT_CONTEXT~ per accedere all'armeria", null,ModalitaServer.Roleplay,  action: new Action<Ped, object[]>(Armerie.NuovaArmeria)));
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