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
		private static bool switchButtons = false;
		private static int iLocal_450 = 0;
		private static int iLocal_451 = 0;

		private static int Paramf_29 = 0;
		private static int Paramf_31 = 0;
		private static int Paramf_30 = 0;
		private static int Paramf_32 = 0;

		private static string[] Sequenza1 = new string[7]
		{
			"start_to_a1", "a1_pose", "a1_to_a2",
			"a2_pose", "a2_to_a3", "a3_pose",
			"a3_to_start",
		};

		private static string[] Sequenza2 = new string[9]
		{
			"start_to_a1", "a1_pose", "a1_to_a2",
			"a2_pose", "a2_to_a3", "a3_pose",
			"a3_to_b4", "b4_pose", "b4_to_start",
		};

		private static string[] Sequenza3 = new string[15]
		{
			"start_to_c1", "c1_pose", "c1_to_c2",
			"c2_pose", "c2_to_c3", "c3_pose",
			"c3_to_c4", "c4_pose", "c4_to_c5",
			"c5_pose", "c5_to_c6", "c6_pose",
			"c6_to_c7",	"c7_pose", "c7_to_start",
		};


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
				Materasso = await World.CreateProp(new Model(MaterassoYoga), GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 2f, 0), true, true);
				//rimuovere da inventario
				YogaButtons = new Scaleform("yoga_buttons");
				YogaKeys = new Scaleform("yoga_keys");
			};
		}

		private static void Spawnato()
		{
			RequestAnimDict(YogaAnim);
			RequestAnimDict(YogaAnimUnknown);
			RequestAnimDict("missfam5_yoga");
			RequestAdditionalText("YOGA", 3);
			Client.Instance.AddTick(Materassino);
		}

		private static async Task Animations()
		{
			SetPlayerControl(PlayerId(), false, 0);
			while (!YogaButtons.IsLoaded) await BaseScript.Delay(1);
			while (!YogaKeys.IsLoaded) await BaseScript.Delay(1);
			if(IsControlJustPressed(0, 223))
			{
				switchButtons = !switchButtons;
			}
			if (IsHelpMessageDisplayed("STICKS_KM") || IsHelpMessageDisplayed("STICKS"))
			{
				if (IsInputDisabled(2))
				{
					func_346(ref Paramf_29, ref Paramf_31, 0, 180, 180);
					func_332(YogaKeys);
				}
				else
				{
					func_346(ref Paramf_30, ref Paramf_32, 1, 180, 180);
					func_332(YogaButtons);
				}
			}
			if (IsInputDisabled(2))
				DrawScaleformMovie(YogaKeys.Handle, 0.5f, 0.88f, 0.609375f * 1.2f, 0.266666f * 1.2f, 100, 100, 100, 255, 0);
				//uLocal_440
			else
				DrawScaleformMovie(YogaButtons.Handle, 0.5f, 0.88f, 0.609375f * 1.2f, 0.266666f * 1.2f, 100, 100, 100, 255, 0);
				//uLocal_439
		}

		private static void func_332(Scaleform scale)
		{
			int r = 255;
			int g = 255;
			int b = 255;
			int a = 255;
			GetHudColour(129, ref r, ref g, ref b, ref a);
			if (IsInputDisabled(2))
			{
				if (switchButtons)
				{
					scale.CallFunction("REPLACE_KEYS_WITH_STICK", 0);
					scale.CallFunction("REPLACE_STICK_WITH_KEYS", 1);
				}
				else
				{
					scale.CallFunction("REPLACE_STICK_WITH_KEYS", 0);
					scale.CallFunction("REPLACE_KEYS_WITH_STICK", 1);
				}
			}
			scale.CallFunction("SET_STICK_POINTER_ANGLE", 0, 180);
			scale.CallFunction("SET_STICK_POINTER_ANGLE", 1, 180);
			scale.CallFunction("SET_STICK_POINTER_RGB", 0, r, g, b);
			scale.CallFunction("SET_STICK_POINTER_RGB", 1, r, g, b);

			if(Paramf_31 == 1) 
				scale.CallFunction("HIDE_STICK_POINTER", 0);
			else
				scale.CallFunction("SET_STICK_POINTER_HIGHLIGHT_ANGLE", 0, Paramf_29);
			if (Paramf_31 == 1)
				scale.CallFunction("HIDE_STICK_POINTER", 1);
			else
				scale.CallFunction("SET_STICK_POINTER_HIGHLIGHT_ANGLE", 1, Paramf_30);
		}

		private static async Task Materassino()
		{
			if (Materasso != null && Materasso.Exists())
			{
				if (World.GetDistance(Game.PlayerPed.Position, Materasso.Position) < 1.2f)
				{
					HUD.ShowHelp("~INPUT_CONTEXT~ per praticare lo Yoga\n~INPUT_FRONTEND_CANCEL~ per ritirare il materassino");
					if (Input.IsControlJustPressed(Control.Context))
					{
						Client.Instance.AddTick(Animations);
						Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
						ApplyPedDamageDecal(PlayerPedId(), 1, 0.5f, 0.513f, 0f, 1f, 0, 0, true, "blushing");
						func_351(0);
						if(IsInputDisabled(2))
							HUD.ShowHelp(Game.GetGXTEntry("STICKS_KM"), 1000);
						else
							HUD.ShowHelp(Game.GetGXTEntry("STICKS"), 1000);

						int seq0 = -1;
						OpenSequenceTask(ref seq0);
						TaskPlayAnimAdvanced(0, "missfam5_yoga", Sequenza1[0], Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, Game.PlayerPed.Rotation.X, Game.PlayerPed.Rotation.Y, Game.PlayerPed.Rotation.Z, 4f, -4f, -1, 528384, 0f, 2, 1);
						TaskPlayAnimAdvanced(0, "missfam5_yoga", Sequenza1[1], Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, Game.PlayerPed.Rotation.X, Game.PlayerPed.Rotation.Y, Game.PlayerPed.Rotation.Z, 4f, -4f, -1, 528384, 0f, 2, 1);
						CloseSequenceTask(seq0);
						TaskPerformSequence(PlayerPedId(), seq0);
						ClearSequenceTask(ref seq0);
						await BaseScript.Delay(5000);
						if (!IsHelpMessageDisplayed("INHALE_NEW"))
						{
							HUD.ShowHelp(Game.GetGXTEntry("INHALE_NEW"), 3000);
						}
						while (!IsControlPressed(2, 228) && !IsControlPressed(2, 229))
						{
							await BaseScript.Delay(0);
							if (Input.IsControlJustPressed(Control.FrontendCancel))
								return;
						}
						if (IsControlPressed(2, 228) && IsControlPressed(2, 229))
						{
							PlayFacialAnim(PlayerPedId(), "michael_breathing_face", "missfam5_yoga");
							PlaySoundFromEntity(0, "YOGA_INHALE", PlayerPedId(), "FAMILY_5_SOUNDS", false, 0);
						}
						HUD.ShowHelp(Game.GetGXTEntry("EXHALE_NEW"), 3000);
						if (!IsControlPressed(2, 228) && !IsControlPressed(2, 229))
						{
							int iVar1 = Funzioni.GetRandomInt(1, 6);
							switch (iVar1)
							{
								case 1:
								case 4:
								case 5:
									PlaySoundFromEntity(0, "YOGA_EXHALE", PlayerPedId(), "FAMILY_5_SOUNDS", false, 0);
									PlayFacialAnim(PlayerPedId(), "michael_breathing_face_exhale", "missfam5_yoga");
									break;
								case 2:
								case 3:
									PlaySoundFromEntity(0, "YOGA_EXHALE", PlayerPedId(), "FAMILY_5_SOUNDS", false, 0);
									PlayFacialAnim(PlayerPedId(), "michael_breathing_face_exhale_oow", "missfam5_yoga");
									break;
							}
						}
						await BaseScript.Delay(2000);
					}
					SetPlayerControl(PlayerId(), true, 0);
					Client.Instance.RemoveTick(Animations);
				}
			}
		}

		private static bool IsHelpMessageDisplayed(string message)
		{
			BeginTextCommandIsThisHelpMessageBeingDisplayed(message);
			return EndTextCommandIsThisHelpMessageBeingDisplayed(0);
		}

		private static void func_351(int iParam1)
		{
			int uVar0 = 0;

			if (!IsPedInjured(PlayerPedId()))
			{
				if (!IsEntityPlayingAnim(PlayerPedId(), "missfam5_yoga", "start_pose", 3))
				{
					if (iParam1 == 0)
						TaskPlayAnim(PlayerPedId(), "missfam5_yoga", "start_pose", 4f, -8f, -1, 1, 0f, false, true, false);
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

		private static int func_347(int iParam0)
		{
			int iVar0 = 0;
			int iVar1 = 0;
			int iVar2 = 0;
			int iVar3 = 0;
			float fVar4;
			var uVar5 = 0;
			var uVar6 = 0;

			func_348(ref iVar0, ref iVar1, ref iVar2, ref iVar3, false);
			if (IsInputDisabled(0))
			{
				if (switchButtons)
				{
					uVar5 = iVar2;
					uVar6 = iVar3;
					iVar2 = iVar0;
					iVar3 = iVar1;
					iVar0 = uVar5;
					iVar1 = uVar6;
				}
			}
			switch (iParam0)
			{
				case 0:
					if (((IsInputDisabled(2) && iLocal_450 != 0) && iLocal_451 != 0) && switchButtons)
					{
						return 0;
					}
					fVar4 = Vmag((iVar0), (iVar1), 0f);
					if (fVar4 < 100f)
					{
						return 1;
					}
					break;

				case 1:
					if (((IsInputDisabled(2) && iLocal_450 != 0) && iLocal_451 != 0) && !switchButtons)
					{
						return 0;
					}
					fVar4 = Vmag((iVar2), (iVar3), 0f);
					if (fVar4 < 100f)
					{
						return 1;
					}
					break;
			}
			return 0;
		}

		private static void func_348(ref int uParam0, ref int uParam1, ref int uParam2, ref int uParam3, bool bParam4)
		{
			uParam0 = Floor((GetControlNormal(2, 218) * 127f));
			uParam1 = Floor((GetControlNormal(2, 219) * 127f));
			uParam2 = Floor((GetControlNormal(2, 220) * 127f));
			uParam3 = Floor((GetControlNormal(2, 221) * 127f));
			if (bParam4)
			{
				if ((uParam0) == 0f && (uParam1) == 0f)
				{
					uParam0 = Floor((GetDisabledControlNormal(2, 218) * 127f));
					uParam1 = Floor((GetDisabledControlNormal(2, 219) * 127f));
				}
				if ((uParam2) == 0f && (uParam3) == 0f)
				{
					uParam2 = Floor((GetDisabledControlNormal(2, 220) * 127f));
					uParam3 = Floor((GetDisabledControlNormal(2, 221) * 127f));
				}
			}
		}


		private static int func_346(ref int uParam0, ref int uParam1, int iParam2, int iParam3, int iParam4)
		{
			uParam0 = func_349(iParam2);
			uParam1 = func_347(iParam2);
			if (uParam1 == 0)
			{
				switch (iParam4)
				{
					case 0:
						switch (iParam3)
						{
							case 0:
								if (uParam0 >= 345 || uParam0 <= 15)
								{
									return 1;
								}
								break;

							case 45:
								if (uParam0 >= 30 && uParam0 <= 60)
								{
									return 1;
								}
								break;

							case 90:
								if (uParam0 >= 75 && uParam0 <= 105)
								{
									return 1;
								}
								break;

							case 135:
								if (uParam0 >= 120 && uParam0 <= 150)
								{
									return 1;
								}
								break;

							case 180:
								if (uParam0 >= 165 && uParam0 <= 195)
								{
									return 1;
								}
								break;

							case 225:
								if (uParam0 >= 210 && uParam0 <= 240)
								{
									return 1;
								}
								break;

							case 270:
								if (uParam0 >= 255 && uParam0 <= 285)
								{
									return 1;
								}
								break;

							case 315:
								if (uParam0 >= 300 && uParam0 <= 330)
								{
									return 1;
								}
								break;
						}
						break;

					case 1:
						switch (iParam3)
						{
							case 0:
								if (uParam0 >= 305 || uParam0 <= 55)
								{
									return 1;
								}
								break;

							case 45:
								if (uParam0 >= 350 || uParam0 <= 100)
								{
									return 1;
								}
								break;

							case 90:
								if (uParam0 >= 35 && uParam0 <= 145)
								{
									return 1;
								}
								break;

							case 135:
								if (uParam0 >= 80 && uParam0 <= 190)
								{
									return 1;
								}
								break;

							case 180:
								if (uParam0 >= 125 && uParam0 <= 235)
								{
									return 1;
								}
								break;

							case 225:
								if (uParam0 >= 170 && uParam0 <= 280)
								{
									return 1;
								}
								break;

							case 270:
								if (uParam0 >= 215 && uParam0 <= 325)
								{
									return 1;
								}
								break;

							case 315:
								if (uParam0 >= 260 || uParam0 <= 10)
								{
									return 1;
								}
								break;
						}
						break;
				}
			}
			return 0;
		}

		private static int func_349(int iParam0)
		{
			int iVar0 = 0;
			int iVar1 = 0;
			int iVar2 = 0;
			int iVar3 = 0;
			int iVar4 = 0;
			int iVar5 = 0;
			var uVar6 = 0;
			var uVar7 = 0;

			func_350(ref iVar2, ref iVar3, ref iVar4, ref iVar5, false, false);
			if (IsInputDisabled(2))
			{
				if (switchButtons)
				{
					uVar6 = iVar4;
					uVar7 = iVar5;
					iVar4 = iVar2;
					iVar5 = iVar3;
					iVar2 = uVar6;
					iVar3 = uVar7;
					iVar2 = (iVar2 / 4);
					iVar3 = (iVar3 / 4);
					if (iVar2 == 0 || iVar3 == 0)
					{
						iVar2 = iLocal_450;
						iVar3 = iLocal_451;
					}
					iLocal_450 = iVar2;
					iLocal_451 = iVar3;
				}
				else
				{
					iVar4 = (iVar4 / 4);
					iVar5 = (iVar5 / 4);
					if (iVar4 == 0 || iVar5 == 0)
					{
						iVar4 = iLocal_450;
						iVar5 = iLocal_451;
					}
					iLocal_450 = iVar4;
					iLocal_451 = iVar5;
				}
			}
			iVar0 = Round(GetAngleBetween_2dVectors(0f, -127f, ToFloat(iVar2), ToFloat(iVar3)));
			iVar1 = Round(GetAngleBetween_2dVectors(0f, -127f, ToFloat(iVar4), ToFloat(iVar5)));
			if (iVar2 < 0)
			{
				iVar0 = (180 + (180 - iVar0));
			}
			if (iVar4 < 0)
			{
				iVar1 = (180 + (180 - iVar1));
			}
			switch (iParam0)
			{
				case 0:
					return iVar0;
				case 1:
					return iVar1;
			}
			return 0;
		}


		private static void func_350(ref int uParam0, ref int uParam1, ref int uParam2, ref int uParam3, bool bParam4, bool bParam5)
		{
			uParam0 = Floor((GetControlUnboundNormal(2, 218) * 127f));
			uParam1 = Floor((GetControlUnboundNormal(2, 219) * 127f));
			uParam2 = Floor((GetControlUnboundNormal(2, 220) * 127f));
			uParam3 = Floor((GetControlUnboundNormal(2, 221) * 127f));
			if (bParam4)
			{
				if (!IsControlEnabled(2, 218))
				{
					uParam0 = Floor((GetDisabledControlUnboundNormal(2, 218) * 127f));
				}
				if (!IsControlEnabled(2, 219))
				{
					uParam1 = Floor((GetDisabledControlUnboundNormal(2, 219) * 127f));
				}
				if (!IsControlEnabled(2, 220))
				{
					uParam2 = Floor((GetDisabledControlUnboundNormal(2, 220) * 127f));
				}
				if (!IsControlEnabled(2, 221))
				{
					uParam3 = Floor((GetDisabledControlUnboundNormal(2, 221) * 127f));
				}
			}
			if (IsInputDisabled(2))
			{
				if (bParam5)
				{
					if (IsLookInverted())
					{
						uParam3 *= -1;
					}
					if (N_0xe1615ec03b3bb4fd())
					{
						uParam3 *= -1;
					}
				}
			}
		}

	}
}
