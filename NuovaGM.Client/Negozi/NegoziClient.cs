using CitizenFX.Core;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.Negozi
{
	static class NegoziClient
	{
		private static ConfigNegoziGenerici NegoziGenerici;
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(NegoziSpawn));
			NegoziGenerici = Client.Impostazioni.Negozi.NegoziGenerici;
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
				InputHandler.ListaInput.Add(new InputController(Control.Context, v, new Radius(1.375f, 1.4f), "Premi ~INPUT_CONTEXT~ per accedere al negozio", null, action: new Action<Ped, object[]>(tfs)));
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
				InputHandler.ListaInput.Add(new InputController(Control.Context, v, new Radius(1.375f, 1.4f), "Premi ~INPUT_CONTEXT~ per accedere al negozio", null, action: new Action<Ped, object[]>(rq)));
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
				InputHandler.ListaInput.Add(new InputController(Control.Context, v, new Radius(1.375f, 1.4f), "Premi ~INPUT_CONTEXT~ per accedere al negozio", null, action: new Action<Ped, object[]>(ltd)));
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
				InputHandler.ListaInput.Add(new InputController(Control.Context, v, new Radius(1.375f, 1.4f), "Premi ~INPUT_CONTEXT~ per accedere all'armeria", null, action: new Action<Ped, object[]>(Armerie.NuovaArmeria)));
			}
		}

		private static void tfs(Ped _, object[] args)
		{
			NegoziBusiness.NegozioPubblico("247");
		}
		private static void rq(Ped _, object[] args)
		{
			NegoziBusiness.NegozioPubblico("rq");
		}
		private static void ltd(Ped _, object[] args)
		{
			NegoziBusiness.NegozioPubblico("ltd");
		}
	}
}
