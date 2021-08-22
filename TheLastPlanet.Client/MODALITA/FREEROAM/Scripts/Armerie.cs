using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Shared;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Scripts
{
	static class Armerie
	{
		private static List<Blip> blips = new();
		private static List<Position> negozi = new()
		{
			new(-662.1f, -935.3f, 20.8f),
			new(810.2f, -2157.3f, 28.6f),
			new(1693.4f, 3759.5f, 33.7f),
			new(-330.2f, 6083.8f, 30.4f),
			new(252.3f, -50.0f, 68.9f),
			new(22.0f, -1107.2f, 28.8f),
			new(2567.6f, 294.3f, 107.7f),
			new(-1117.5f, 2698.6f, 17.5f),
			new(842.4f, -1033.4f, 27.1f),
			new(-1306.2f, -394.0f, 35.6f),
		};

		private static string GetWeaponByName(int i)
		{
			switch (i)
			{
				case 0:	return string.Empty;
			}
			return string.Empty;
		}

		public static void Init()
		{
			foreach(var v in negozi)
			{
				Blip bliparmi = World.CreateBlip(v.ToVector3);
				bliparmi.Sprite = BlipSprite.AmmuNation;
				SetBlipDisplay(bliparmi.Handle, 4);
				bliparmi.Scale = 1f;
				bliparmi.Color = BlipColor.Green;
				bliparmi.IsShortRange = true;
				bliparmi.Name = "Armeria";
				blips.Add(bliparmi);
				//InputHandler.ListaInput.Add(new InputController(Control.Context, v, "Premi ~INPUT_CONTEXT~ per accedere all'armeria", null, action: new Action<Ped, object[]>()));
			}
		}

		public static void Stop()
		{
			blips.ForEach(v => v.Delete());
			blips.Clear();
		}
	}
}
