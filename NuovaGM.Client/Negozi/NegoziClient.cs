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
		static List<Vector3> tfs = new List<Vector3>()
		{
			new Vector3(373.875f,    325.896f,  102.566f),
			new Vector3(2557.458f,   382.282f,  107.622f),
			new Vector3(-3038.939f,  585.954f,  6.908f),
			new Vector3(-3241.927f,  1001.462f, 11.830f),
			new Vector3(547.431f,    2671.710f, 41.156f),
			new Vector3(1961.464f,   3740.672f, 31.343f),
			new Vector3(2678.916f,   3280.671f, 54.241f),
			new Vector3(1729.216f,   6414.131f, 34.037f)
		};

		static List<Vector3> rq = new List<Vector3>()
		{
			new Vector3(1135.808f,   -982.281f,  45.415f),
			new Vector3(-1222.915f,  -906.983f,  11.326f),
			new Vector3(-1487.553f,  -379.107f,  39.163f),
			new Vector3(-2968.243f,  390.910f,   14.043f),
			new Vector3(1166.024f,   2708.930f,  37.157f),
			new Vector3(1392.562f,   3604.684f,  33.980f),
		};

		static List<Vector3> ltd = new List<Vector3>()
		{
			new Vector3(-48.519f,    -1757.514f, 28.421f),
			new Vector3(1163.373f,   -323.801f,  68.205f),
			new Vector3(-707.501f,   -914.260f,  18.215f),
			new Vector3(-1820.523f,  792.518f,   137.118f),
			new Vector3(1698.388f,   4924.404f,  41.06f),
		};

		static List<Vector3> armerie = new List<Vector3>()
		{
			new Vector3(-662.1f, -935.3f, 20.8f),
			new Vector3(810.2f, -2157.3f, 28.6f),
			new Vector3(1693.4f, 3759.5f, 33.7f),
			new Vector3(-330.2f, 6083.8f, 30.4f),
			new Vector3(252.3f, -50.0f, 68.9f),
			new Vector3(22.0f, -1107.2f, 28.8f),
			new Vector3(2567.6f, 294.3f, 107.7f),
			new Vector3(-1117.5f, 2698.6f, 17.5f),
			new Vector3(842.4f, -1033.4f, 27.1f),
			new Vector3(-1306.2f, -394.0f, 35.6f),
		};

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(NegoziSpawn));
		}

		public static void NegoziSpawn()
		{
			foreach (Vector3 v in tfs)
			{
				Blip bliptfs = new Blip(AddBlipForCoord(v.X, v.Y, v.Z));
				bliptfs.Sprite = BlipSprite.Store;
				SetBlipDisplay(bliptfs.Handle, 4);
				bliptfs.Scale = 1f;
				bliptfs.Color = BlipColor.Green;
				bliptfs.IsShortRange = true;
				bliptfs.Name = "Negozio";
			}
			foreach (Vector3 v in rq)
			{
				Blip bliptrq = new Blip(AddBlipForCoord(v.X, v.Y, v.Z));
				bliptrq.Sprite = BlipSprite.Store;
				SetBlipDisplay(bliptrq.Handle, 4);
				bliptrq.Scale = 1f;
				bliptrq.Color = BlipColor.Green;
				bliptrq.IsShortRange = true;
				bliptrq.Name = "Negozio";
			}
			foreach (Vector3 v in ltd)
			{
				Blip blipltd = new Blip(AddBlipForCoord(v.X, v.Y, v.Z));
				blipltd.Sprite = BlipSprite.Store;
				SetBlipDisplay(blipltd.Handle, 4);
				blipltd.Scale = 1f;
				blipltd.Color = BlipColor.Green;
				blipltd.IsShortRange = true;
				blipltd.Name = "Negozio";
			}
			foreach (Vector3 v in armerie)
			{
				Blip bliparmi = new Blip(AddBlipForCoord(v.X, v.Y, v.Z));
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
			foreach (Vector3 v in tfs)
			{
				if (World.GetDistance(Game.PlayerPed.Position, v) < 1.375f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context))
					{
						HUD.ShowNotification("Placeholder negozi");
					}
				}
			}
			foreach (Vector3 v in rq)
			{
				if (World.GetDistance(Game.PlayerPed.Position, v) < 1.375f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context))
					{
						HUD.ShowNotification("Placeholder negozi");
					}
				}
			}
			foreach (Vector3 v in ltd)
			{
				if (World.GetDistance(Game.PlayerPed.Position, v) < 1.375f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context))
					{
						HUD.ShowNotification("Placeholder negozi");
					}
				}
			}
			foreach (Vector3 v in armerie)
			{
				if (World.GetDistance(Game.PlayerPed.Position, v) < 1.375f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accedere al negozio");
					if (Input.IsControlJustPressed(Control.Context))
					{
						Armerie.ArmeriaMenu();
					}
				}
			}
			await Task.FromResult(0);
		}
	}
}
