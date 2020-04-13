using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
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
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(NegoziSpawn));
			NegoziGenerici = Client.Impostazioni.Negozi.NegoziGenerici;
		}

		public static void NegoziSpawn()
		{
			foreach (float[] v in NegoziGenerici.tfs)
			{
				Blip bliptfs = World.CreateBlip(v.ToVector3());
				bliptfs.Sprite = BlipSprite.Store;
				SetBlipDisplay(bliptfs.Handle, 4);
				bliptfs.Scale = 1f;
				bliptfs.Color = BlipColor.Green;
				bliptfs.IsShortRange = true;
				bliptfs.Name = "Negozio";
			}
			foreach (float[] v in NegoziGenerici.rq)
			{
				Blip bliptrq = World.CreateBlip(v.ToVector3());
				bliptrq.Sprite = BlipSprite.Store;
				SetBlipDisplay(bliptrq.Handle, 4);
				bliptrq.Scale = 1f;
				bliptrq.Color = BlipColor.Green;
				bliptrq.IsShortRange = true;
				bliptrq.Name = "Negozio";
			}
			foreach (float[] v in NegoziGenerici.ltd)
			{
				Blip blipltd = World.CreateBlip(v.ToVector3());
				blipltd.Sprite = BlipSprite.Store;
				SetBlipDisplay(blipltd.Handle, 4);
				blipltd.Scale = 1f;
				blipltd.Color = BlipColor.Green;
				blipltd.IsShortRange = true;
				blipltd.Name = "Negozio";
			}
			foreach (float[] v in NegoziGenerici.armerie)
			{
				Blip bliparmi = World.CreateBlip(v.ToVector3());
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
			foreach (float[] v in NegoziGenerici.tfs)
			{
				if (World.GetDistance(Game.PlayerPed.Position, v.ToVector3()) < 1.375f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context))
					{
						HUD.ShowNotification("Placeholder negozi");
					}
				}
			}
			foreach (float[] v in NegoziGenerici.rq)
			{
				if (World.GetDistance(Game.PlayerPed.Position, v.ToVector3()) < 1.375f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context))
					{
						HUD.ShowNotification("Placeholder negozi");
					}
				}
			}
			foreach (float[] v in NegoziGenerici.ltd)
			{
				if (World.GetDistance(Game.PlayerPed.Position, v.ToVector3()) < 1.375f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context))
					{
						HUD.ShowNotification("Placeholder negozi");
					}
				}
			}
			foreach (float[] v in NegoziGenerici.armerie)
			{
				if (World.GetDistance(Game.PlayerPed.Position, v.ToVector3()) < 1.375f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context))
						Armerie.NuovaArmeria();/*ArmeriaMenu*/
				}
			}
			await Task.FromResult(0);
		}
	}
}
