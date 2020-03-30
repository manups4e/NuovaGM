using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Sport
{
	static class Yoga
	{
		private static readonly string MaterassoYoga = "prop_yoga_mat_03";
		private static Prop Materasso;
		private static Scaleform YogaButtons;
		private static Scaleform YogaKeys;
		private static Camera YogaCam;
		private static Vector3 Coords = new Vector3(-791.0036f, 186.3552f, 71.8295f);
		private static readonly List<string> Props = new List<string>
		{
			"prop_mem_candle_04",
			"prop_mem_candle_05",
			"prop_mem_candle_06",
			"prop_mp3_dock",
			"prop_phone_ing"
		};
		private static readonly string YogaAnim = "mini@yoga";
		private static readonly string YogaAnimUnknown = "move_p_m_zero_idles@generic";

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			CaricaTutto();
		}

		private static void Spawnato()
		{
			Materasso = new Prop(CreateObject(GetHashKey(MaterassoYoga), Coords.X, Coords.Y, Coords.Z, false, false, false));
	//		Client.GetInstance.RegisterTickHandler(Materassino);
		}

		private static async void CaricaTutto()
		{
			RequestAnimDict(YogaAnim);
			while (!HasAnimDictLoaded(YogaAnim)) await BaseScript.Delay(100);
			RequestAnimDict(YogaAnimUnknown);
			while (!HasAnimDictLoaded(YogaAnimUnknown)) await BaseScript.Delay(100);
			RequestAnimDict("missfam5_yoga");
			while (!HasAnimDictLoaded("missfam5_yoga")) await BaseScript.Delay(100);
			RequestModel((uint)GetHashKey(MaterassoYoga));
			while (!HasModelLoaded((uint)GetHashKey(MaterassoYoga))) await BaseScript.Delay(100);
		}

		private static async Task Materassino()
		{
			if (World.GetDistance(Game.PlayerPed.Position, Coords) < 2)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per praticare lo Yoga");
				if (Input.IsControlJustPressed(Control.Context))
				{
					Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
					YogaButtons = new Scaleform("yoga_buttons");
					YogaKeys = new Scaleform("yoga_keys");
					while (!HasScaleformMovieLoaded(YogaButtons.Handle)) await BaseScript.Delay(100);
					while (!HasScaleformMovieLoaded(YogaKeys.Handle)) await BaseScript.Delay(100);

				}
			}
		}
	}
}
