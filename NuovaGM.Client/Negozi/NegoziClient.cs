using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Negozi
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
			foreach (var v in NegoziGenerici.tfs)
			{
				Blip bliptfs = World.CreateBlip(v);
				bliptfs.Sprite = BlipSprite.Store;
				SetBlipDisplay(bliptfs.Handle, 4);
				bliptfs.Scale = 1f;
				bliptfs.Color = BlipColor.Green;
				bliptfs.IsShortRange = true;
				bliptfs.Name = "24/7";
			}
			foreach (var v in NegoziGenerici.rq)
			{
				Blip bliptrq = World.CreateBlip(v);
				bliptrq.Sprite = BlipSprite.Store;
				SetBlipDisplay(bliptrq.Handle, 4);
				bliptrq.Scale = 1f;
				bliptrq.Color = BlipColor.Green;
				bliptrq.IsShortRange = true;
				bliptrq.Name = "Robs Liquor";
			}
			foreach (var v in NegoziGenerici.ltd)
			{
				Blip blipltd = World.CreateBlip(v);
				blipltd.Sprite = BlipSprite.Store;
				SetBlipDisplay(blipltd.Handle, 4);
				blipltd.Scale = 1f;
				blipltd.Color = BlipColor.Green;
				blipltd.IsShortRange = true;
				blipltd.Name = "Limited Gasoline";
			}
			foreach (var v in NegoziGenerici.armerie)
			{
				Blip bliparmi = World.CreateBlip(v);
				bliparmi.Sprite = BlipSprite.AmmuNation;
				SetBlipDisplay(bliparmi.Handle, 4);
				bliparmi.Scale = 1f;
				bliparmi.Color = BlipColor.Green;
				bliparmi.IsShortRange = true;
				bliparmi.Name = "Armeria";
			}
		}

		public static async Task OnTick()
		{
			foreach (var v in NegoziGenerici.tfs)
			{
				if (Game.PlayerPed.IsInRangeOf(v, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
						NegoziBusiness.NegozioPubblico("247");
				}
			}
			foreach (var v in NegoziGenerici.rq)
			{
				if (Game.PlayerPed.IsInRangeOf(v, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
						NegoziBusiness.NegozioPubblico("rq");
				}
			}
			foreach (var v in NegoziGenerici.ltd)
			{
				if (Game.PlayerPed.IsInRangeOf(v, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
						NegoziBusiness.NegozioPubblico("ltd");
				}
			}
			foreach (var v in NegoziGenerici.armerie)
			{
				if (Game.PlayerPed.IsInRangeOf(v, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
						Armerie.NuovaArmeria();/*ArmeriaMenu*/
				}
			}
			await Task.FromResult(0);
		}
	}
}
