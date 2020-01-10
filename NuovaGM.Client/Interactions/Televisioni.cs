using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;

namespace NuovaGM.Client.Interactions
{
	public enum TVChannel
	{
		Spenta = 0,
		TV = 1,
		Resto = 2
	}
	static class Televisioni
	{
		private static bool VicinoTV;
		private static Prop FakeTV;
		private static Prop Telecomando;
		private static Televisione TV = new Televisione();
		private static int RenderTarget;
		private static List<ObjectHash> TVHashes = new List<ObjectHash>()
		{
			ObjectHash.prop_monitor_01b,
			ObjectHash.prop_laptop_lester2,
			ObjectHash.prop_tv_flat_01,
			ObjectHash.prop_tv_flat_02,
			ObjectHash.prop_tv_flat_02b,
			ObjectHash.prop_tv_flat_03,
			ObjectHash.prop_tv_flat_03b,
			ObjectHash.xm_prop_x17_tv_flat_01,
			ObjectHash.vw_prop_vw_tv_rt_01a,
			ObjectHash.vw_prop_vw_cinema_tv_01,
			ObjectHash.sm_prop_smug_tv_flat_01,
			ObjectHash.prop_tv_03,
			ObjectHash.des_tvsmash_start,
			ObjectHash.v_ilev_mm_scre_off,
			ObjectHash.v_ilev_mm_screen2,
			ObjectHash.v_ilev_mm_screen2_vl,
			ObjectHash.prop_trev_tv_01,
			ObjectHash.prop_tt_screenstatic,
			ObjectHash.prop_tv_flat_michael,
			ObjectHash.apa_mp_h_str_avunits_01,
			ObjectHash.apa_mp_h_str_avunitm_01,
			ObjectHash.apa_mp_h_str_avunitm_03,
			ObjectHash.apa_mp_h_str_avunitl_01_b,
			ObjectHash.apa_mp_h_str_avunitl_04,
			ObjectHash.apa_mp_h_str_avunits_04,
			ObjectHash.ex_prop_ex_tv_flat_01,
		};

		private static List<string> CanaliTV = new List<string>
		{
			"PL_STD_CNT",
			"PL_STD_WZL",
			"PL_LO_CNT",
			"PL_LO_WZL",
			"PL_SP_WORKOUT",
			"PL_SP_INV",
			"PL_SP_INV_EXP",
			"PL_LO_RS",
			"PL_LO_RS_CUTSCENE",
			"PL_SP_PLSH1_INTRO",
			"PL_LES1_FAME_OR_SHAME",
			"PL_STD_WZL_FOS_EP2",
			"PL_MP_WEAZEL",
			"PL_MP_CCTV",
			"PL_CINEMA_ACTION",
			"PL_CINEMA_ARTHOUSE",
			"PL_CINEMA_MULTIPLAYER",
			"PL_WEB_HOWITZER",
			"PL_WEB_RANGERS"
		};

		private static List<string> canaliInternet = new List<string>
		{
			"PL_CINEMA_ACTION",
			"PL_CINEMA_ARTHOUSE",
			"PL_CINEMA_MULTIPLAYER",
			"PL_WEB_HOWITZER",
			"PL_WEB_RANGERS",
			"PL_WEB_PRB2",
			"PL_WEB_KFLF",
			"PL_WEB_RS",
		};

		public static async void Init()
		{
			Client.GetInstance.RegisterTickHandler(ControlloTv);
			Client.GetInstance.RegisterTickHandler(Televisione);
		}

		public static async Task ControlloTv()
		{
			VicinoTV = World.GetAllProps().Select(o => new Prop(o.Handle)).Where(o => TVHashes.Contains((ObjectHash)(uint)o.Model.Hash)).Any(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * 3f, 2));
			if (VicinoTV)
				TV.Entity = World.GetAllProps().Select(o => new Prop(o.Handle)).Where(o => TVHashes.Contains((ObjectHash)(uint)o.Model.Hash)).First(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * 3f, 2));
			await BaseScript.Delay(500);
		}

		public static async Task Televisione()
		{
			if (VicinoTV)
			{
				if (!TV.Accesa)
				{
					HUD.ShowHelp(GetLabelText("TV_HLP1"));
					if (Game.IsControlJustPressed(0, Control.Context))
					{
						Vector3 pos = TV.Entity.Position;
						Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
						Client.GetInstance.RegisterTickHandler(DrawTV);
						Client.GetInstance.DeregisterTickHandler(Televisione);
					}
				}
			}
		}

		public static async Task DrawTV()
		{
			if (TV.Canale == 0 && !TV.Accesa)
			{
				if (TV.Entity.Model.Hash == (int)ObjectHash.prop_tv_flat_01 || TV.Entity.Model.Hash == (int)ObjectHash.prop_tv_flat_02 || TV.Entity.Model.Hash == (int)ObjectHash.prop_tv_flat_03)
					FakeTV = await World.CreateProp(new Model("prop_tv_flat_01_screen"), TV.Entity.Position + new Vector3(0, 0, -0.13f), TV.Entity.Rotation, true, false);
				else if(TV.Entity.Model.Hash == (int)ObjectHash.prop_tv_03)
					FakeTV = await World.CreateProp(new Model("prop_tv_03_overlay"), TV.Entity.Position + new Vector3(0, 0, -0.21f), TV.Entity.Rotation, true, false);
				else if (TV.Entity.Model.Hash == (int)ObjectHash.apa_mp_h_str_avunitm_01)
					FakeTV = await World.CreateProp(new Model("ex_prop_ex_tv_flat_01"), TV.Entity.Position, TV.Entity.Rotation, true, false);
				else if (TV.Entity.Model.Hash == (int)ObjectHash.apa_mp_h_str_avunits_04)
					FakeTV = await World.CreateProp(new Model("ex_prop_ex_tv_flat_01"), TV.Entity.Position, TV.Entity.Rotation, true, false);
//				else if (GetInteriorFromGameplayCam() == 145921)
//					FakeTV = await World.CreateProp(new Model("ex_tvscreen"), new Vector3(-1469.104f, -548.509f, -73.244f), TV.Entity.Rotation, true, false);
				RenderTarget = RenderTargets.CreateNamedRenderTargetForModel(TV.Entity.Model.Hash == (int)ObjectHash.apa_mp_h_str_avunitm_01 || TV.Entity.Model.Hash == (int)ObjectHash.apa_mp_h_str_avunits_04 ? "ex_tvscreen" : "tvscreen", (uint)FakeTV.Model.Hash);
				Debug.WriteLine("TV Hash = " + TV.Entity.Model.Hash);
				Debug.WriteLine("TV Pos = " + TV.Entity.Position);
				Debug.WriteLine("FakeTV Rotation = " + FakeTV.Rotation);

				RegisterScriptWithAudio(0);
				SetTvAudioFrontend(false);
				AttachTvAudioToEntity(TV.Entity.Handle);
				SetTvChannel(-1);
				SetTvChannelPlaylist(1, CanaliTV[0], false);
				TV.Canale = 1;
				SetTvChannel((int)TVChannel.TV);
				SetTvVolume(-4f);
				EnableMovieSubtitles(true);
				TV.Accesa = true;
			}
			Game.DisableControlThisFrame(0, Control.FrontendLeft);
			Game.DisableControlThisFrame(0, Control.FrontendRight);
			Game.DisableControlThisFrame(0, Control.FrontendUp);
			Game.DisableControlThisFrame(0, Control.MultiplayerInfo);
			Game.DisableControlThisFrame(0, Control.FrontendDown);
			Game.DisableControlThisFrame(0, Control.VehicleExit);
			if (TV.Accesa)
			{
				HUD.ShowHelp("~INPUTGROUP_FRONTEND_DPAD_LR~ per cambiare ~y~canale~w~.\n~INPUTGROUP_FRONTEND_DPAD_UD~ per cambiare il ~b~volume~w~.\n~INPUT_VEH_EXIT~ per spegnere la TV");
				if (Game.IsDisabledControlJustPressed(0, Control.FrontendLeft)) // canale-
				{
					TV.Canale--;
					if (TV.Canale < 0) TV.Canale = CanaliTV.Count - 1;
					SetTvChannelPlaylist(1, CanaliTV[TV.Canale], false);
					SetTvChannel((int)TVChannel.TV);
					Game.PlaySound("SAFEHOUSE_MICHAEL_SIT_SOFA", "MICHAEL_SOFA_TV_CHANGE_CHANNEL_MASTER");
				}

				if (Game.IsDisabledControlJustPressed(0, Control.FrontendRight)) // canale+ 
				{
					TV.Canale++;
					if (TV.Canale > 18) TV.Canale = 0;
					SetTvChannelPlaylist(1, CanaliTV[TV.Canale], false);
					SetTvChannel((int)TVChannel.TV);
					Game.PlaySound("SAFEHOUSE_MICHAEL_SIT_SOFA", "MICHAEL_SOFA_TV_CHANGE_CHANNEL_MASTER");
				}
				if (Game.IsDisabledControlJustPressed(0, Control.FrontendUp)) // volume su
				{
					TV.Volume += 0.5f;
					if (TV.Volume > 0) TV.Volume = 0; 
					SetTvVolume(TV.Volume);
					if (TV.Volume > -36 && TV.Volume < 0)
						Game.PlaySound("SAFEHOUSE_MICHAEL_SIT_SOFA", "MICHAEL_SOFA_REMOTE_CLICK_VOLUME_MASTER");
				}
				if (Game.IsDisabledControlJustPressed(0, Control.FrontendDown)) // volume giu
				{
					TV.Volume -= 0.5f;
					if (TV.Volume < -36) TV.Volume = -36;
					SetTvVolume(TV.Volume);
					if (TV.Volume > -36 && TV.Volume < 0)
						Game.PlaySound("SAFEHOUSE_MICHAEL_SIT_SOFA", "MICHAEL_SOFA_REMOTE_CLICK_VOLUME_MASTER");
				}
				if (Game.IsDisabledControlJustPressed(0, Control.VehicleExit))
				{
					ClearTvChannelPlaylist(1);
					SetTvChannel(-1);
					Game.PlayerPed.Task.ClearAll();
					if (IsNamedRendertargetRegistered("tvscreen"))
						ReleaseNamedRendertarget("tvscreen");
					else if (IsNamedRendertargetRegistered("ex_tvscreen"))
						ReleaseNamedRendertarget("ex_tvscreen");
					RenderTarget = -1;
					SetTextRenderId(GetDefaultScriptRendertargetRenderId());
					TV.Accesa = false;
					TV.Canale = 0;
					EnableMovieSubtitles(false);
					FakeTV.Delete();
					int intnum = -1;
					uint something = (uint)intnum;
					Client.GetInstance.RegisterTickHandler(Televisione);
					Client.GetInstance.DeregisterTickHandler(DrawTV);
				}
			}
			float fVar0 = 1f;
			CalcolaAspectRatioRenderTarget(ref fVar0);
			SetTextRenderId(RenderTarget);
			Set_2dLayer(4);
			SetScriptGfxDrawBehindPausemenu(true);
			DrawTvChannel(0.5f, 0.5f, fVar0, 1.0f, 0.0f, 255, 255, 255, 255);
			SetTextRenderId(GetDefaultScriptRendertargetRenderId());
			SetScriptGfxDrawBehindPausemenu(false);
		}

		static void CalcolaAspectRatioRenderTarget(ref float fParam0)
		{
			float fVar0;
			float fVar1;
			float fVar2;

			fVar0 = GetAspectRatio(false);
			if (fVar0 <= (16f / 9f))
			{
				fVar1 = (fVar0 / (16f / 9f));
				fVar2 = fParam0;
				fParam0 = (fVar2 * fVar1);
			}
		}

		static Vector3 func_1015(Vector3 vParam0, float fParam3)
		{
			Vector3 vVar0;
			float fVar3;
			float fVar4;

			fVar3 = Sin(fParam3);
			fVar4 = Cos(fParam3);
			vVar0.X = ((vParam0.X * fVar4) - (vParam0.Y * fVar3));
			vVar0.Y = ((vParam0.X * fVar3) + (vParam0.Y * fVar4));
			vVar0.Z = vParam0.Z;
			return vVar0;
		}
	}

	public class Televisione
	{
		public int Canale = 0;
		public Prop Entity;
		public bool Accesa = false;
		public float Volume = 0.5f;
	}
}



/*
 struct<6> func_1757(int iParam0, bool bParam1)
{
	struct<6> Var0;

	switch (iParam0)
	{
		case -1:
			Var0 = { -794.9184f, 339.6266f, 200.4135f };
			Var0.f_3 = { 0f, 0f, 180f };
			break;
		case 1:
			Var0 = { -794.9184f, 339.6266f, 200.4135f };
			Var0.f_3 = { 0f, 0f, 180f };
			break;
		case 2:
			Var0 = { -761.0982f, 317.6259f, 169.5963f };
			Var0.f_3 = { 0f, 0f, 0f };
			break;
		case 3:
			Var0 = { -761.1888f, 317.6295f, 216.0503f };
			Var0.f_3 = { 0f, 0f, 0f };
			break;
		case 4:
			Var0 = { -795.3856f, 340.0188f, 152.7941f };
			Var0.f_3 = { 0f, 0f, 180f };
			break;
		case 61:
			Var0 = { -778.5056f, 332.3779f, 212.1968f };
			Var0.f_3 = { 0f, 0f, 90f };
			break;
		case 5:
			Var0 = { -258.1807f, -950.6853f, 70.0239f };
			Var0.f_3 = { 0f, 0f, 70f };
			break;
		case 6:
			Var0 = { -285.0051f, -957.6552f, 85.3035f };
			Var0.f_3 = { 0f, 0f, -110f };
			break;
		case 7:
			Var0 = { -1471.882f, -530.7484f, 62.34918f };
			Var0.f_3 = { 0f, 0f, -145f };
			break;
		case 34:
			Var0 = { -1471.882f, -530.7484f, 49.72156f };
			Var0.f_3 = { 0f, 0f, -145f };
			break;
		case 62:
			Var0 = { -1463.15f, -540.2369f, 74.2439f };
			Var0.f_3 = { 0f, 0f, -145f };
			break;
		case 35:
			Var0 = { -885.3702f, -451.4775f, 119.327f };
			Var0.f_3 = { 0f, 0f, 27.55617f };
			break;
		case 36:
			Var0 = { -913.0385f, -438.4284f, 114.3997f };
			Var0.f_3 = { 0f, 0f, -153.3093f };
			break;
		case 37:
			Var0 = { -892.5499f, -430.4789f, 88.25368f };
			Var0.f_3 = { 0f, 0f, 116.9193f };
			break;
		case 38:
			Var0 = { -35.0462f, -576.317f, 82.90739f };
			Var0.f_3 = { 0f, 0f, 160f };
			break;
		case 39:
			Var0 = { -10.3788f, -590.7431f, 93.02542f };
			Var0.f_3 = { 0f, 0f, 70f };
			break;
		case 65:
			Var0 = { -22.2487f, -589.1461f, 80.2305f };
			Var0.f_3 = { 0f, 0f, 69.88f };
			break;
		case 40:
			Var0 = { -900.6311f, -376.7462f, 78.27306f };
			Var0.f_3 = { 0f, 0f, 26.92611f };
			break;
		case 41:
			Var0 = { -929.483f, -374.5104f, 102.2329f };
			Var0.f_3 = { 0f, 0f, -152.5531f };
			break;
		case 63:
			Var0 = { -914.4202f, -375.8189f, 114.4743f };
			Var0.f_3 = { 0f, 0f, -63f };
			break;
		case 42:
			Var0 = { -617.1647f, 64.6042f, 100.8196f };
			Var0.f_3 = { 0f, 0f, 180f };
			break;
		case 43:
			Var0 = { -584.2015f, 42.7133f, 86.4187f };
			Var0.f_3 = { 0f, 0f, 0f };
			break;
		case 64:
			Var0 = { -609.5665f, 50.2203f, 98.3998f };
			Var0.f_3 = { 0f, 0f, -90f };
			break;
		case 73:
			Var0 = { -171.3969f, 494.2671f, 134.4935f };
			Var0.f_3 = { 0f, 0f, 11f };
			break;
		case 74:
			Var0 = { 339.4982f, 434.0887f, 146.2206f };
			Var0.f_3 = { 0f, 0f, -63.5f };
			break;
		case 75:
			Var0 = { -761.3884f, 615.7333f, 140.9805f };
			Var0.f_3 = { 0f, 0f, -71.5f };
			break;
		case 76:
			Var0 = { -678.1752f, 591.0076f, 142.2196f };
			Var0.f_3 = { 0f, 0f, 40.5f };
			break;
		case 77:
			Var0 = { 120.0541f, 553.793f, 181.0943f };
			Var0.f_3 = { 0f, 0f, 6f };
			break;
		case 78:
			Var0 = { -571.4039f, 655.2008f, 142.6293f };
			Var0.f_3 = { 0f, 0f, -14.5f };
			break;
		case 79:
			Var0 = { -742.2565f, 587.6547f, 143.0577f };
			Var0.f_3 = { 0f, 0f, -29f };
			break;
		case 80:
			Var0 = { -857.2222f, 685.051f, 149.6502f };
			Var0.f_3 = { 0f, 0f, 4.5f };
			break;
		case 81:
			Var0 = { -1287.65f, 443.2707f, 94.6919f };
			Var0.f_3 = { 0f, 0f, 0f };
			break;
		case 82:
			Var0 = { 374.2012f, 416.9688f, 142.6977f };
			Var0.f_3 = { 0f, 0f, -14f };
			break;
		case 83:
			Var0 = { -787.7805f, 334.9232f, 186.1134f };
			Var0.f_3 = { 0f, 0f, 90f };
			break;
		case 84:
			Var0 = { -787.7805f, 334.9232f, 215.8384f };
			Var0.f_3 = { 0f, 0f, 90f };
			break;
		case 85:
			Var0 = { -773.2258f, 322.8252f, 194.8862f };
			Var0.f_3 = { 0f, 0f, -90f };
			break;
		case 86:
			Var0 = { -1573.098f, -4085.806f, 9.7851f };
			Var0.f_3 = { 0f, 0f, 162f };
			break;
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 66:
		case 67:
		case 68:
		case 69:
			Var0 = { 342.8157f, -997.4288f, -100f };
			Var0.f_3 = { 0f, 0f, 0f };
			break;
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 70:
		case 71:
		case 72:
			Var0 = { 260.3297f, -997.4288f, -100f };
			Var0.f_3 = { 0f, 0f, 0f };
			break;
		case 87:
			Var0 = { -1572.187f, -570.8315f, 109.9879f };
			Var0.f_3 = { 0f, 0f, -54f };
			break;
		case 88:
			Var0 = { -1383.954f, -476.7112f, 73.507f };
			Var0.f_3 = { 0f, 0f, 8f };
			break;
		case 89:
			Var0 = { -138.0029f, -629.739f, 170.2854f };
			Var0.f_3 = { 0f, 0f, -84f };
			break;
		case 90:
			Var0 = { -74.8895f, -817.6883f, 244.8508f };
			Var0.f_3 = { 0f, 0f, 70f };
			break;
		case 91:
		case 92:
		case 93:
		case 94:
		case 95:
		case 96:
			Var0 = { 1100.764f, -3159.384f, -34.9342f };
			Var0.f_3 = { 0f, 0f, 0f };
			break;
		case 97:
		case 98:
		case 99:
		case 100:
		case 101:
		case 102:
			Var0 = { 1005.806f, -3157.67f, -36.0897f };
			Var0.f_3 = { 0f, 0f, 0f };
			break;
		case 103:
			if (!bParam1)
			{
				Var0 = { -1576.571f, -569.7595f, 85.5f };
				Var0.f_3 = { 0f, 0f, 36.1f };
			}
			else
			{
				Var0 = { -1578.022f, -576.4251f, 104.2f };
				Var0.f_3 = { 0f, 0f, -144.04f };
			}
			break;
		case 104:
			if (!bParam1)
			{
				Var0 = { -1571.254f, -566.5865f, 85.5f };
				Var0.f_3 = { 0f, 0f, -53.9f };
			}
			else
			{
				Var0 = { -1578.022f, -576.4251f, 104.2f };
				Var0.f_3 = { 0f, 0f, -144.04f };
			}
			break;
		case 105:
			if (!bParam1)
			{
				Var0 = { -1568.098f, -571.9171f, 85.5f };
				Var0.f_3 = { 0f, 0f, -143.9f };
			}
			else
			{
				Var0 = { -1578.022f, -576.4251f, 104.2f };
				Var0.f_3 = { 0f, 0f, -144.04f };
			}
			break;
		case 106:
			if (!bParam1)
			{
				Var0 = { -1384.518f, -475.8657f, 56.1f };
				Var0.f_3 = { 0f, 0f, 98.7f };
			}
			else
			{
				Var0 = { -1391.245f, -473.9638f, 77.2f };
				Var0.f_3 = { 0f, 0f, 98.86f };
			}
			break;
		case 107:
			if (!bParam1)
			{
				Var0 = { -1384.538f, -475.8829f, 48.1f };
				Var0.f_3 = { 0f, 0f, 98.7f };
			}
			else
			{
				Var0 = { -1391.245f, -473.9638f, 77.2f };
				Var0.f_3 = { 0f, 0f, 98.86f };
			}
			break;
		case 108:
			if (!bParam1)
			{
				Var0 = { -1378.994f, -477.2481f, 56.1f };
				Var0.f_3 = { 0f, 0f, -81.1f };
			}
			else
			{
				Var0 = { -1391.245f, -473.9638f, 77.2f };
				Var0.f_3 = { 0f, 0f, 98.86f };
			}
			break;
		case 109:
			if (!bParam1)
			{
				Var0 = { -186.5683f, -576.4624f, 135f };
				Var0.f_3 = { 0f, 0f, 96.16f };
			}
			else
			{
				Var0 = { -146.6167f, -596.6301f, 166f };
				Var0.f_3 = { 0f, 0f, -140f };
			}
			break;
		case 110:
			if (!bParam1)
			{
				Var0 = { -113.886f, -564.3862f, 135f };
				Var0.f_3 = { 0f, 0f, 110.96f };
			}
			else
			{
				Var0 = { -146.6167f, -596.6301f, 166f };
				Var0.f_3 = { 0f, 0f, -140f };
			}
			break;
		case 111:
			if (!bParam1)
			{
				Var0 = { -134.6568f, -635.1774f, 135f };
				Var0.f_3 = { 0f, 0f, -9.04f };
			}
			else
			{
				Var0 = { -146.6167f, -596.6301f, 166f };
				Var0.f_3 = { 0f, 0f, -140f };
			}
			break;
		case 112:
			if (!bParam1)
			{
				Var0 = { -79.0479f, -822.6393f, 221f };
				Var0.f_3 = { 0f, 0f, 70f };
			}
			else
			{
				Var0 = { -73.904f, -821.6204f, 284f };
				Var0.f_3 = { 0f, 0f, -110f };
			}
			break;
		case 113:
			if (!bParam1)
			{
				Var0 = { -70.3086f, -819.5784f, 221f };
				Var0.f_3 = { 0f, 0f, 160f };
			}
			else
			{
				Var0 = { -73.904f, -821.6204f, 284f };
				Var0.f_3 = { 0f, 0f, -110f };
			}
			break;
		case 114:
			if (!bParam1)
			{
				Var0 = { -79.9861f, -818.425f, 221f };
				Var0.f_3 = { 0f, 0f, -20f };
			}
			else
			{
				Var0 = { -73.904f, -821.6204f, 284f };
				Var0.f_3 = { 0f, 0f, -110f };
			}
			break;
	}
	return Var0;
}
*/