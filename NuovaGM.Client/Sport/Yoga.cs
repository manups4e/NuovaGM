using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Shared;
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
		private static int iLocal_613 = -1;
		private static Camera uLocal_612;
		private static bool someBool = true;

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

		public static async void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			SharedScript.ItemList["materassinoyoga"].Usa += async (item, index) =>
			{
				Materasso = await World.CreateProp(new Model(MaterassoYoga), Game.PlayerPed.Position + new Vector3(0, 3f, 0), true, true);
			};
		}

		private static void Spawnato()
		{
			RequestAnimDict(YogaAnim);
			RequestAnimDict(YogaAnimUnknown);
			RequestAnimDict("missfam5_yoga");
			RequestAdditionalText("YOGA", 3);
			//Materasso = new Prop(CreateObject(GetHashKey(MaterassoYoga), Coords.X, Coords.Y, Coords.Z, false, false, false));
			Client.Instance.AddTick(Materassino);
		}


		private static async Task Materassino()
		{
			if (Materasso != null && Materasso.Exists())
			{
				if (World.GetDistance(Game.PlayerPed.Position, Materasso.Position) < 1.2f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per praticare lo Yoga");
					if (Input.IsControlJustPressed(Control.Context))
					{
						Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
						YogaButtons = new Scaleform("yoga_buttons");
						YogaKeys = new Scaleform("yoga_keys");
						while (!YogaButtons.IsLoaded) await BaseScript.Delay(1);
						while (!YogaKeys.IsLoaded) await BaseScript.Delay(1);
					}
				}
			}
		}

		private static void func_351(int iParam1)
		{
			int uVar0 = 0;

			if (!IsPedInjured(PlayerPedId()))
			{
				if (!IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "start_pose", 3))
				{
					if (iParam1 == 0)
					{
						TaskPlayAnim(PlayerPedId(), "missfam5_yoga", "start_pose", 4f, -8f, -1, 1, 0f, false, true, false);
					}
					else
					{
						Vector3 coord = Game.PlayerPed.Position;
						Vector3 rot = Game.PlayerPed.Rotation;
						ClearSequenceTask(ref uVar0);
						OpenSequenceTask(ref uVar0);
						switch (Funzioni.GetRandomInt(0, 3))
						{
							case 0:
								TaskPlayAnimAdvanced(0, "missfam5_yoga", "fail_to_start_a", coord.X, coord.Y, coord.Z, rot.X, rot.Y, rot.Z, 1000f, -4f, -1, 528384, 0f, 2, 1);
								break;

							case 1:
								TaskPlayAnimAdvanced(0, "missfam5_yoga", "fail_to_start_b", coord.X, coord.Y, coord.Z, rot.X, rot.Y, rot.Z, 1000f, -4f, -1, 528384, 0f, 2, 1);
								break;

							case 2:
								TaskPlayAnimAdvanced(0, "missfam5_yoga", "fail_to_start_c", coord.X, coord.Y, coord.Z, rot.X, rot.Y, rot.Z, 1000f, -4f, -1, 528384, 0f, 2, 1);
								break;
						}
						TaskPlayAnimAdvanced(0, "missfam5_yoga", "start_pose", coord.X, coord.Y, coord.Z, rot.X, rot.Y, rot.Z, 4f, -8f, -1, 528385, 0f, 2, 1);
						CloseSequenceTask(uVar0);
						TaskPerformSequence(PlayerPedId(), uVar0);
						ClearSequenceTask(ref uVar0);
						ClearFacialIdleAnimOverride(PlayerPedId());
						PlayFacialAnim(PlayerPedId(), "fail_face", "missfam5_yoga");
						N_0x2208438012482a1a(PlayerPedId(), false, false);
					}
				}
			}
		}

		private static void func_355()
		{
			if (!IsPedInjured(PlayerPedId()))
			{
				if (someBool == true)
				{
					if (IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "A1_POSE", 3) || IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "C1_POSE", 3))
					{
						SetFacialIdleAnimOverride(PlayerPedId(), "A1ANDC1_FACE", "missfam5_yoga");
						someBool = false;
					}
					if (IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "A2_POSE", 3) || IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "C2_POSE", 3))
					{
						SetFacialIdleAnimOverride(PlayerPedId(), "A2ANDC2_FACE", "missfam5_yoga");
						someBool = false;
					}
					if (IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "A3_POSE", 3) || IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "C3_POSE", 3))
					{
						SetFacialIdleAnimOverride(PlayerPedId(), "A3ANDC3_FACE", "missfam5_yoga");
						someBool = false;
					}
					if (IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "B4_POSE", 3) || IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "C4_POSE", 3))
					{
						SetFacialIdleAnimOverride(PlayerPedId(), "B4ANDC4_FACE", "missfam5_yoga");
						someBool = false;
					}
					if (IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "C5_POSE", 3))
					{
						SetFacialIdleAnimOverride(PlayerPedId(), "C5_FACE", "missfam5_yoga");
						someBool = false;
					}
					if (IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "C6_POSE", 3))
					{
						SetFacialIdleAnimOverride(PlayerPedId(), "C6_FACE", "missfam5_yoga");
						someBool = false;
					}
					if (IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "C7_POSE", 3))
					{
						SetFacialIdleAnimOverride(PlayerPedId(), "C7_FACE", "missfam5_yoga");
						someBool = false;
					}
					if (IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "C8_POSE", 3))
					{
						SetFacialIdleAnimOverride(PlayerPedId(), "C8_FACE", "missfam5_yoga");
						someBool = false;
					}
				}
			}
		}


	}
}
