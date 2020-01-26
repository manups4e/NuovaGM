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
using Newtonsoft.Json;

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

		private static List<TvCoord> Coords = new List<TvCoord>()
		{
			new TvCoord(new Vector3(-800.3020f, 342.9110f, 207.2540f), new Vector3(0)),
			new TvCoord(new Vector3(-781.8525f, 342.0280f, 211.1880f), new Vector3(0)),
			new TvCoord(new Vector3(-161.6548f, 482.8907f, 136.2438f), new Vector3(0)),
			new TvCoord(new Vector3(127.2918f, 543.4010f, 182.9871f), new Vector3(0.0000f, 0.0000f, -84.0000f)),
			new TvCoord(new Vector3(-780.4050f, 338.4678f, 186.1134f), new Vector3(0.0000f, 0.0000f, -90.0000f)),
			new TvCoord(new Vector3(-1464.4001f, -3758.1221f, 9.4872f), new Vector3(0.0000f, 0.0000f, 72.0000f)),
			new TvCoord(new Vector3(-1373.8730f, -476.7940f, 72.4570f), new Vector3(0.0000f, 0.0000f, 98.0000f)),
			new TvCoord(new Vector3(-143.1880f, -605.2324f, 167.6455f), new Vector3(0.0000f, 0.0000f, 130.0000f)),
			new TvCoord(new Vector3(-780.4701f, 338.4247f, 187.5084f), new Vector3(0.0000f, 0.0000f, -90.0000f)),
			new TvCoord(new Vector3(337.2845f, -996.6658f, -99.0276f), new Vector3(0f, 0f, 90f)),
			new TvCoord(new Vector3(256.7323f, -995.4481f, -98.8606f), new Vector3( 0f, 0f, 45f)),
			new TvCoord(new Vector3(-800.302f, 342.911f, 207.254f), new Vector3(0.0f, 0.0f, 0.0f)),
			new TvCoord(new Vector3(-755.7146f, 314.341461f, 176.4368f), new Vector3(0.0f, 0.0f, -180.0f)),
			new TvCoord(new Vector3(-755.8052f, 314.345062f, 222.8908f), new Vector3(0.0f, 0.0f, -180.0f)),
			new TvCoord(new Vector3(-800.7692f, 343.303223f, 159.6346f), new Vector3(0.0f, 0.0f, 0.0f)),
			new TvCoord(new Vector3(-253.253036f, -946.7497f, 76.8644f), new Vector3(0.0f, 0.0f, -110.0f)),
			new TvCoord(new Vector3(-289.932739f, -961.5908f, 92.144f), new Vector3(0.0f, 0.0f, 70.0f)),
			new TvCoord(new Vector3(-1478.17578f, -531.1459f, 69.18968f), new Vector3(0.0f, 0.0f, 35.0f)),
			new TvCoord(new Vector3(265.713318f, -1000.7132f, -93.1595f), new Vector3(0.0f, 0.0f, -180.0f)),
			new TvCoord(new Vector3(-1478.17578f, -531.1459f, 56.56206f), new Vector3(0.0f, 0.0f, 35.0f)),
			new TvCoord(new Vector3(-879.0779f, -451.898773f, 126.1675f), new Vector3(0.0f, 0.0f, -152.443832f)),
			new TvCoord(new Vector3(-919.3237f, -437.91214f, 121.2402f), new Vector3(0.0f, 0.0f, 26.6907043f)),
			new TvCoord(new Vector3(-892.0587f, -424.191681f, 95.09418f), new Vector3(0.0f, 0.0f, -63.0807037f)),
			new TvCoord(new Vector3(-38.9817963f, -571.389343f, 89.74789f), new Vector3(0.0f, 0.0f, -20.0f)),
			new TvCoord(new Vector3(-5.45114946f, -586.8075f, 99.86592f), new Vector3(0.0f, 0.0f, -110.0f)),
			new TvCoord(new Vector3(-894.3438f, -377.236633f, 85.11356f), new Vector3(0.0f, 0.0f, -153.073883f)),
			new TvCoord(new Vector3(-935.7745f, -374.077118f, 109.0734f), new Vector3(0.0f, 0.0f, 27.4469f)),
			new TvCoord(new Vector3(-622.5483f, 67.8886261f, 107.6601f), new Vector3(0.0f, 0.0f, 0.0f)),
			new TvCoord(new Vector3(-578.8179f, 39.428875f, 93.2592f), new Vector3(0.0f, 0.0f, -180.0f)),
			new TvCoord(new Vector3(-775.2212f, 337.7615f, 219.0373f), new Vector3(0.0f, 0.0f, -90.0f)), // da sistemare
			new TvCoord(new Vector3(-1469.44385f, -540.634338f, 81.0844f), new Vector3(0.0f, 0.0f, 35.0f)),
			new TvCoord(new Vector3(-914.9026f, -382.106842f, 121.3148f), new Vector3(0.0f, 0.0f, 117.0f)), // da sistemare
			new TvCoord(new Vector3(-612.850952f, 44.8366928f, 105.2403f), new Vector3(0.0f, 0.0f, 90.0f)), // da sistemare
			new TvCoord(new Vector3(-17.3128166f, -585.2208f, 87.071f), new Vector3(0.0f, 0.0f, -110.12f)), // da sistemare 
			new TvCoord(new Vector3(-165.4855f, 492.070251f, 141.334f), new Vector3(0.0f, 0.0f, -169.0f)), // da sistemare
			new TvCoord(new Vector3(338.961f, 427.805237f, 153.0611f), new Vector3(0.0f, 0.0f, 116.5f)), // da sistemare
			new TvCoord(new Vector3(-762.794861f, 609.5857f, 147.821f), new Vector3(0.0f, 0.0f, 108.5f)), // da sistemare
			new TvCoord(new Vector3(-671.948364f, 592.006531f, 149.0601f), new Vector3(0.0f, 0.0f, -139.5f)), // da sistemare
			new TvCoord(new Vector3(125.751526f, 551.089355f, 187.9348f), new Vector3(0.0f, 0.0f, -174.0f)), // da sistemare
			new TvCoord(new Vector3(-567.0141f, 650.673035f, 149.4698f), new Vector3(0.0f, 0.0f, 165.5f)), // sistemare
			new TvCoord(new Vector3(-739.1402f, 582.172058f, 149.8982f), new Vector3(0.0f, 0.0f, 151.0f)), // sistemare
			new TvCoord(new Vector3(-851.597534f, 682.1991f, 156.4907f), new Vector3(0.0f, 0.0f, -175.5f)), // sistemare
			new TvCoord(new Vector3(-1282.26648f, 439.986267f, 101.5324f), new Vector3(0.0f, 0.0f, -180.0f)), // sistemare
			new TvCoord(new Vector3(378.6303f, 412.479523f, 149.5382f), new Vector3(0.0f, 0.0f, 166.0f)), // sistemare
			new TvCoord(new Vector3(-784.4961f, 340.3068f, 192.9539f), new Vector3(0.0f, 0.0f, -90.0f)), // sistemare
			new TvCoord(new Vector3(-776.510254f, 317.4416f, 201.7267f), new Vector3(0.0f, 0.0f, 90.0f)), // sistemare
		};

		private static List<TvCoord> Test = new List<TvCoord>();

		private static List<TvCoord> TvLocali = new List<TvCoord>()
		{
			new TvCoord(new Vector3(1120.1711f, -3144.5613f, -35.8581f), new Vector3(0.0000f, 0.0000f, 90.0000f)),
			new TvCoord(new Vector3(1010.6960f, -3164.1321f, -34.1590f), new Vector3(0.0000f, 0.0000f, -63.9800f)),
			new TvCoord(new Vector3(364.0089f, 4838.958f, -58.8157f), new Vector3(0f, 0f, 163f)),
			new TvCoord(new Vector3(228.8359f, -974.6591f, -98.3713f), new Vector3(0)),

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
			for (int i = 0; i < 500; i++)
			{
				prendicoords(i);
			}
			Debug.WriteLine("Ecco qui = " + JsonConvert.SerializeObject(Test));
		}

		public static async Task ControlloTv()
		{
			VicinoTV = World.GetAllProps().Select(o => new Prop(o.Handle)).Where(o => TVHashes.Contains((ObjectHash)(uint)o.Model.Hash)).Any(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * 3f, 2));
			if (VicinoTV && GetInteriorFromEntity(Game.PlayerPed.Handle) != 145921)
				TV.Entity = World.GetAllProps().Select(o => new Prop(o.Handle)).Where(o => TVHashes.Contains((ObjectHash)(uint)o.Model.Hash)).First(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * 3f, 2));
			if (GetInteriorFromGameplayCam() == 145921 && World.GetDistance(new Vector3(-1469.154f, -548.539f, 73.244f), Game.PlayerPed.Position) < 9f)
				VicinoTV = true;
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
						Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
						AccendiTV();
					}
				}
			}
		}

		private static async void AccendiTV()
		{
			if (TV.Canale == 0 && !TV.Accesa)
			{
				if (GetInteriorFromGameplayCam() == 145921)
					FakeTV = await World.CreateProp(new Model("prop_tv_flat_01_screen"), new Vector3(-1469.128f, -548.506f, 73.114f), new Vector3(0f, 0f, -235), true, false);
				else if (TV.Entity.Model.Hash == (int)ObjectHash.prop_tv_flat_01 || TV.Entity.Model.Hash == (int)ObjectHash.prop_tv_flat_02 || TV.Entity.Model.Hash == (int)ObjectHash.prop_tv_flat_03)
					FakeTV = await World.CreateProp(new Model("prop_tv_flat_01_screen"), TV.Entity.Position + new Vector3(0, 0, -0.13f), TV.Entity.Rotation, true, false);
				else if (TV.Entity.Model.Hash == (int)ObjectHash.prop_tv_03)
					FakeTV = await World.CreateProp(new Model("prop_tv_03_overlay"), TV.Entity.Position + new Vector3(0, 0, -0.21f), TV.Entity.Rotation, true, false);
				RenderTarget = RenderTargets.CreateNamedRenderTargetForModel(TV.Entity != null? TV.Entity.Model.Hash == (int)ObjectHash.apa_mp_h_str_avunitm_01 || TV.Entity.Model.Hash == (int)ObjectHash.apa_mp_h_str_avunits_04 ? "ex_tvscreen" : "tvscreen" : "tvscreen", (uint)FakeTV.Model.Hash);

				Debug.WriteLine("Posizione = " + TV.Entity.Position);
				Debug.WriteLine("Rotazione = " + TV.Entity.Rotation);

				RegisterScriptWithAudio(0);
				SetTvAudioFrontend(false);
				AttachTvAudioToEntity(TV.Entity != null ? TV.Entity.Handle : FakeTV.Handle);
				SetTvChannel(-1);
				SetTvChannelPlaylist(1, CanaliTV[0], false);
				TV.Canale = 1;
				SetTvChannel((int)TVChannel.TV);
				SetTvVolume(-4f);
				EnableMovieSubtitles(true);
				TV.Accesa = true;
				Client.GetInstance.RegisterTickHandler(DrawTV);
			}
		}

		public static async Task DrawTV()
		{
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
					--TV.Canale;
					if (TV.Canale < 0) TV.Canale = CanaliTV.Count - 1;
					SetTvChannelPlaylist(1, CanaliTV[TV.Canale], false);
					SetTvChannel((int)TVChannel.TV);
					Game.PlaySound("SAFEHOUSE_MICHAEL_SIT_SOFA", "MICHAEL_SOFA_TV_CHANGE_CHANNEL_MASTER");
				}

				if (Game.IsDisabledControlJustPressed(0, Control.FrontendRight)) // canale+ 
				{
					++TV.Canale;
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





		static void prendicoords(int iParam1)
		{
			TvCoord valore = new TvCoord();
			int iVar6;
			TvCoord test = new TvCoord();

			func_15(iParam1, 557, ref test, func_3774(iParam1), false);
			valore.Coord = test.Coord;
			valore.Rot = test.Rot;
			Test.Add(valore);
		}

		static int func_3774(int iParam0)
		{
			switch (iParam0)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					return 1;
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
				case 69:
				case 68:
				case 66:
				case 67:
					return 8;
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
					break;

				case 61:
				case 62:
				case 63:
				case 64:
				case 65:
					break;

				case 73:
				case 74:
				case 75:
				case 76:
					break;

				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
					break;

				case 83:
				case 84:
				case 85:
					break;

				case 86:
					break;

				case 87:
				case 88:
				case 89:
				case 90:
					break;

				case 91:
				case 92:
				case 93:
				case 94:
				case 95:
				case 96:
					break;

				case 97:
				case 98:
				case 99:
				case 100:
				case 101:
				case 102:
					break;

				case 103:
				case 106:
				case 109:
				case 112:
				case 104:
				case 107:
				case 110:
				case 113:
				case 105:
				case 108:
				case 111:
				case 114:
					break;
			}
			return -1;
		}

		private static void func_15(int iParam0, int iParam1, ref TvCoord uParam2, int iParam3, bool bParam4)
		{
			TvCoord[] Var0 = new TvCoord[2];
			Vector3 vVar13;

			Var0[0] = func_16(iParam3, bParam4);
			Var0[1] = func_16(iParam0, bParam4);
			uParam2 = func_7(iParam1, iParam3);
			vVar13 = uParam2.Coord - Var0[0].Coord;
			vVar13 = func_6(vVar13, -Var0[0 /*6*/].Rot.Z);
			vVar13 = func_6(vVar13, Var0[1 /*6*/].Rot.Z);
			uParam2.Coord = GetObjectOffsetFromCoords(Var0[1 /*6*/].Coord.X, Var0[1 /*6*/].Coord.Y, Var0[1 /*6*/].Coord.Z, 0f, vVar13.X, vVar13.Y, vVar13.Z);

			switch (iParam1)
			{
				case 6:
				case 2:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 34:
				case 35:
				case 36:
				case 38:
				case 39:
				case 120:
				case 121:
				case 125:
				case 40:
				case 41:
				case 43:
				case 44:
				case 45:
				case 42:
				case 46:
				case 47:
				case 48:
				case 49:
				case 50:
				case 51:
				case 52:
				case 53:
				case 54:
				case 55:
				case 56:
				case 64:
				case 65:
				case 57:
				case 58:
				case 59:
				case 60:
				case 61:
				case 62:
				case 63:
				case 66:
				case 67:
				case 68:
				case 109:
				case 69:
				case 70:
				case 71:
				case 72:
				case 73:
				case 74:
				case 75:
				case 76:
				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 83:
				case 85:
				case 84:
				case 89:
				case 90:
				case 91:
				case 92:
				case 94:
				case 95:
				case 96:
				case 97:
				case 98:
				case 93:
				case 99:
				case 100:
				case 106:
				case 107:
				case 108:
				case 119:
				case 122:
				case 123:
				case 124:
				case 138:
				case 139:
				case 140:
				case 130:
				case 128:
				case 141:
				case 142:
				case 143:
				case 144:
				case 145:
				case 146:
				case 147:
				case 148:
				case 153:
				case 154:
				case 155:
				case 157:
				case 234:
				case 174:
				case 175:
				case 176:
				case 177:
				case 178:
				case 179:
				case 180:
				case 235:
				case 149:
				case 150:
				case 151:
				case 152:
				case 172:
				case 187:
				case 173:
				case 201:
				case 188:
				case 189:
				case 190:
				case 191:
				case 192:
				case 193:
				case 194:
				case 195:
				case 196:
				case 197:
				case 198:
				case 199:
				case 200:
				case 284:
				case 285:
				case 286:
				case 287:
				case 202:
				case 203:
				case 204:
				case 205:
				case 206:
				case 207:
				case 208:
				case 209:
				case 210:
				case 211:
				case 212:
				case 213:
				case 214:
				case 216:
				case 217:
				case 182:
				case 183:
				case 181:
				case 156:
				case 236:
				case 237:
				case 238:
				case 239:
				case 240:
				case 241:
				case 242:
				case 243:
				case 244:
				case 245:
				case 246:
				case 247:
				case 248:
				case 249:
				case 250:
				case 251:
				case 252:
				case 253:
				case 254:
				case 255:
				case 256:
				case 257:
				case 258:
				case 259:
				case 260:
				case 261:
				case 262:
				case 263:
				case 264:
				case 265:
				case 266:
				case 267:
				case 268:
				case 269:
				case 270:
				case 271:
				case 272:
				case 273:
				case 274:
				case 275:
					while (Var0[0 /*6*/].Rot.Z > 180f)
					{
						Var0[0 /*6*/].Rot.Z = (Var0[0 /*6*/].Rot.Z - 360f);
					}
					while (Var0[0 /*6*/].Rot.Z < -180f)
					{
						Var0[0 /*6*/].Rot.Z = (Var0[0 /*6*/].Rot.Z + 360f);
					}
					while (Var0[1 /*6*/].Rot.Z > 180f)
					{
						Var0[1 /*6*/].Rot.Z = (Var0[1 /*6*/].Rot.Z - 360f);
					}
					while (Var0[1 /*6*/].Rot.Z < -180f)
					{
						Var0[1 /*6*/].Rot.Z = (Var0[1 /*6*/].Rot.Z + 360f);
					}
					uParam2.Rot.Z += (Var0[1 /*6*/].Rot.Z - Var0[0 /*6*/].Rot.Z);
					while (uParam2.Rot.Z > 180f)
					{
						uParam2.Rot.Z = (uParam2.Rot.Z - 360f);
					}
					while (uParam2.Rot.Z < -180f)
					{
						uParam2.Rot.Z = (uParam2.Rot.Z + 360f);
					}
					break;
			}
			switch (iParam1)
			{
				case 278:
				case 279:
				case 280:
				case 281:
				case 282:
				case 283:
				case 300:
				case 301:
				case 302:
				case 303:
				case 304:
				case 306:
				case 305:
				case 308:
				case 309:
				case 312:
				case 313:
				case 314:
				case 315:
				case 316:
				case 317:
				case 318:
				case 319:
				case 320:
				case 321:
				case 322:
				case 323:
				case 324:
				case 325:
				case 326:
				case 327:
				case 328:
				case 329:
				case 307:
				case 331:
				case 332:
				case 333:
				case 334:
				case 310:
				case 335:
				case 336:
				case 337:
				case 338:
				case 339:
				case 311:
				case 340:
				case 435:
				case 436:
				case 437:
				case 470:
				case 477:
				case 504:
				case 507:
				case 510:
				case 513:
				case 528:
				case 531:
				case 534:
				case 537:
				case 540:
				case 547:
				case 445:
				case 446:
				case 447:
				case 448:
				case 449:
				case 450:
				case 471:
				case 472:
				case 478:
				case 479:
				case 505:
				case 506:
				case 508:
				case 509:
				case 511:
				case 512:
				case 514:
				case 515:
				case 529:
				case 530:
				case 532:
				case 533:
				case 535:
				case 536:
				case 538:
				case 539:
				case 541:
				case 542:
				case 548:
				case 549:
				case 432:
				case 433:
				case 434:
				case 451:
				case 452:
				case 453:
				case 454:
				case 455:
				case 456:
				case 459:
				case 460:
				case 461:
				case 462:
				case 463:
				case 464:
				case 560:
				case 546:
				case 587:
				case 588:
				case 589:
				case 590:
				case 591:
				case 592:
				case 501:
				case 502:
				case 500:
				case 616:
				case 615:
				case 612:
				case 629:
				case 630:
				case 631:
				case 714:
				case 633:
				case 634:
				case 635:
				case 636:
				case 637:
				case 638:
				case 639:
				case 640:
				case 643:
				case 644:
				case 641:
				case 642:
				case 646:
				case 645:
				case 647:
				case 648:
				case 649:
				case 650:
				case 666:
				case 667:
				case 668:
				case 669:
				case 670:
				case 671:
				case 673:
				case 674:
				case 675:
				case 676:
				case 677:
				case 689:
				case 690:
				case 691:
				case 692:
				case 693:
				case 694:
				case 695:
				case 696:
				case 697:
				case 698:
				case 699:
				case 700:
				case 701:
				case 702:
				case 703:
				case 704:
				case 705:
				case 706:
				case 707:
				case 708:
				case 709:
				case 710:
				case 711:
				case 712:
				case 713:
				case 632:
				case 738:
				case 739:
				case 740:
				case 741:
				case 742:
				case 743:
				case 744:
				case 745:
				case 746:
					while (Var0[0 /*6*/].Rot.Z > 180f)
					{
						Var0[0 /*6*/].Rot.Z = (Var0[0 /*6*/].Rot.Z - 360f);
					}
					while (Var0[0 /*6*/].Rot.Z < -180f)
					{
						Var0[0 /*6*/].Rot.Z = (Var0[0 /*6*/].Rot.Z + 360f);
					}
					while (Var0[1 /*6*/].Rot.Z > 180f)
					{
						Var0[1 /*6*/].Rot.Z = (Var0[1 /*6*/].Rot.Z - 360f);
					}
					while (Var0[1 /*6*/].Rot.Z < -180f)
					{
						Var0[1 /*6*/].Rot.Z = (Var0[1 /*6*/].Rot.Z + 360f);
					}
					uParam2.Rot.Z = (uParam2.Rot.Z + (Var0[1 /*6*/].Rot.Z - Var0[0 /*6*/].Rot.Z));
					while (uParam2.Rot.Z > 180f)
					{
						uParam2.Rot.Z = (uParam2.Rot.Z - 360f);
					}
					while (uParam2.Rot.Z < -180f)
					{
						uParam2.Rot.Z = (uParam2.Rot.Z + 360f);
					}
					break;
			}
			switch (iParam1)
			{
				case 715:
				case 716:
				case 717:
				case 718:
				case 719:
				case 720:
				case 721:
				case 722:
				case 723:
				case 724:
				case 725:
				case 726:
				case 727:
				case 728:
				case 729:
				case 730:
				case 731:
				case 732:
				case 733:
				case 734:
				case 735:
				case 736:
				case 737:
				case 747:
				case 748:
				case 749:
				case 750:
				case 751:
				case 752:
				case 753:
				case 754:
				case 755:
				case 756:
				case 757:
				case 758:
				case 759:
				case 760:
				case 761:
				case 762:
				case 763:
				case 764:
				case 765:
				case 766:
				case 767:
				case 768:
				case 769:
				case 770:
				case 771:
				case 772:
				case 773:
				case 774:
				case 775:
				case 776:
				case 777:
				case 778:
				case 779:
				case 780:
				case 781:
				case 782:
				case 783:
				case 784:
				case 785:
				case 786:
				case 787:
				case 788:
				case 789:
				case 790:
				case 791:
				case 792:
				case 793:
				case 794:
				case 795:
				case 796:
				case 797:
				case 798:
				case 799:
				case 800:
				case 802:
				case 801:
				case 803:
				case 804:
				case 805:
				case 806:
				case 807:
				case 808:
				case 809:
				case 678:
				case 679:
				case 680:
				case 681:
				case 682:
				case 683:
				case 684:
				case 685:
				case 810:
				case 811:
				case 812:
				case 813:
				case 814:
				case 815:
				case 817:
				case 816:
				case 819:
				case 818:
				case 820:
				case 821:
				case 822:
				case 823:
				case 824:
				case 825:
				case 826:
				case 827:
				case 828:
				case 829:
				case 830:
				case 831:
				case 832:
				case 833:
				case 834:
				case 835:
				case 836:
				case 837:
				case 838:
				case 839:
				case 840:
				case 841:
				case 842:
				case 843:
				case 844:
				case 845:
				case 846:
				case 847:
				case 848:
				case 849:
				case 850:
				case 851:
				case 852:
				case 853:
				case 854:
				case 855:
				case 856:
				case 686:
				case 687:
				case 688:
					while (Var0[0 /*6*/].Rot.Z > 180f)
					{
						Var0[0 /*6*/].Rot.Z = (Var0[0 /*6*/].Rot.Z - 360f);
					}
					while (Var0[0 /*6*/].Rot.Z < -180f)
					{
						Var0[0 /*6*/].Rot.Z = (Var0[0 /*6*/].Rot.Z + 360f);
					}
					while (Var0[1 /*6*/].Rot.Z > 180f)
					{
						Var0[1 /*6*/].Rot.Z = (Var0[1 /*6*/].Rot.Z - 360f);
					}
					while (Var0[1 /*6*/].Rot.Z < -180f)
					{
						Var0[1 /*6*/].Rot.Z = (Var0[1 /*6*/].Rot.Z + 360f);
					}
					uParam2.Rot.Z = (uParam2.Rot.Z + (Var0[1 /*6*/].Rot.Z - Var0[0 /*6*/].Rot.Z));
					while (uParam2.Rot.Z > 180f)
					{
						uParam2.Rot.Z = (uParam2.Rot.Z - 360f);
					}
					while (uParam2.Rot.Z < -180f)
					{
						uParam2.Rot.Z = (uParam2.Rot.Z + 360f);
					}
					break;
			}
			switch (iParam1)
			{
				case 857:
				case 858:
				case 859:
				case 860:
				case 861:
				case 862:
				case 863:
				case 864:
				case 865:
				case 866:
				case 867:
				case 868:
				case 869:
				case 870:
				case 871:
				case 872:
				case 873:
				case 874:
				case 875:
				case 876:
				case 877:
				case 878:
				case 879:
				case 880:
				case 881:
				case 882:
				case 883:
				case 884:
				case 888:
				case 889:
				case 890:
				case 891:
				case 651:
				case 652:
				case 653:
				case 654:
				case 655:
				case 656:
				case 657:
				case 658:
				case 659:
				case 660:
				case 661:
				case 662:
				case 663:
				case 664:
				case 665:
				case 887:
				case 894:
				case 895:
				case 896:
				case 897:
				case 898:
				case 899:
				case 900:
				case 901:
				case 902:
				case 903:
				case 904:
				case 905:
				case 906:
				case 907:
				case 908:
				case 911:
				case 913:
				case 914:
				case 915:
				case 916:
				case 917:
				case 918:
				case 919:
				case 920:
				case 921:
				case 922:
				case 923:
				case 924:
				case 925:
				case 926:
				case 927:
				case 928:
				case 929:
				case 930:
				case 931:
				case 932:
				case 954:
				case 955:
				case 956:
				case 957:
				case 958:
				case 959:
				case 968:
				case 969:
				case 970:
				case 973:
				case 974:
				case 975:
				case 976:
				case 977:
				case 978:
				case 979:
				case 980:
				case 981:
				case 982:
				case 983:
				case 984:
				case 985:
				case 986:
				case 987:
				case 988:
				case 989:
				case 990:
				case 1008:
				case 1009:
				case 1010:
				case 991:
				case 992:
				case 993:
				case 994:
				case 995:
				case 996:
				case 997:
				case 998:
				case 999:
				case 960:
				case 961:
				case 962:
				case 963:
				case 964:
				case 965:
				case 966:
				case 967:
				case 1000:
				case 1001:
				case 1002:
				case 1003:
				case 1004:
				case 1005:
				case 1006:
				case 1007:
				case 1011:
				case 1012:
				case 1013:
					while (Var0[0 /*6*/].Rot.Z > 180f)
					{
						Var0[0 /*6*/].Rot.Z = (Var0[0 /*6*/].Rot.Z - 360f);
					}
					while (Var0[0 /*6*/].Rot.Z < -180f)
					{
						Var0[0 /*6*/].Rot.Z = (Var0[0 /*6*/].Rot.Z + 360f);
					}
					while (Var0[1 /*6*/].Rot.Z > 180f)
					{
						Var0[1 /*6*/].Rot.Z = (Var0[1 /*6*/].Rot.Z - 360f);
					}
					while (Var0[1 /*6*/].Rot.Z < -180f)
					{
						Var0[1 /*6*/].Rot.Z = (Var0[1 /*6*/].Rot.Z + 360f);
					}
					uParam2.Rot.Z = (uParam2.Rot.Z + (Var0[1 /*6*/].Rot.Z - Var0[0 /*6*/].Rot.Z));
					while (uParam2.Rot.Z > 180f)
					{
						uParam2.Rot.Z = (uParam2.Rot.Z - 360f);
					}
					while (uParam2.Rot.Z < -180f)
					{
						uParam2.Rot.Z = (uParam2.Rot.Z + 360f);
					}
					break;
			}

		}

		static Vector3 func_6(Vector3 vParam0, float fParam3)
		{
			Vector3 vVar0;
			float fVar3;
			float fVar4;

			fVar3 = Sin(fParam3);
			fVar4 = Cos(fParam3);
			vVar0.X = (vParam0.X * fVar4) - (vParam0.Y * fVar3);
			vVar0.Y = (vParam0.X * fVar3) + (vParam0.Y * fVar4);
			vVar0.Z = vParam0.Z;
			return vVar0;
		}


		static TvCoord func_16(int iParam0, bool bParam1)
		{
			TvCoord Var0 = new TvCoord();

			switch (iParam0)
			{
				case -1:
					Var0.Coord = new Vector3(-794.9184f, 339.6266f, 200.4135f);
					Var0.Rot = new Vector3(0f, 0f, 180f);
					break;

				case 1:
					Var0.Coord = new Vector3(-794.9184f, 339.6266f, 200.4135f);
					Var0.Rot = new Vector3(0f, 0f, 180f);
					break;

				case 2:
					Var0.Coord = new Vector3(-761.0982f, 317.6259f, 169.5963f);
					Var0.Rot = new Vector3(0f, 0f, 0f);
					break;

				case 3:
					Var0.Coord = new Vector3(-761.1888f, 317.6295f, 216.0503f);
					Var0.Rot = new Vector3(0f, 0f, 0f);
					break;

				case 4:
					Var0.Coord = new Vector3(-795.3856f, 340.0188f, 152.7941f);
					Var0.Rot = new Vector3(0f, 0f, 180f);
					break;

				case 61:
					Var0.Coord = new Vector3(-778.5056f, 332.3779f, 212.1968f);
					Var0.Rot = new Vector3(0f, 0f, 90f);
					break;

				case 5:
					Var0.Coord = new Vector3(-258.1807f, -950.6853f, 70.0239f);
					Var0.Rot = new Vector3(0f, 0f, 70f);
					break;

				case 6:
					Var0.Coord = new Vector3(-285.0051f, -957.6552f, 85.3035f);
					Var0.Rot = new Vector3(0f, 0f, -110f);
					break;

				case 7:
					Var0.Coord = new Vector3(-1471.882f, -530.7484f, 62.34918f);
					Var0.Rot = new Vector3(0f, 0f, -145f);
					break;

				case 34:
					Var0.Coord = new Vector3(-1471.882f, -530.7484f, 49.72156f);
					Var0.Rot = new Vector3(0f, 0f, -145f);
					break;

				case 62:
					Var0.Coord = new Vector3(-1463.15f, -540.2369f, 74.2439f);
					Var0.Rot = new Vector3(0f, 0f, -145f);
					break;

				case 35:
					Var0.Coord = new Vector3(-885.3702f, -451.4775f, 119.327f);
					Var0.Rot = new Vector3(0f, 0f, 27.55617f);
					break;

				case 36:
					Var0.Coord = new Vector3(-913.0385f, -438.4284f, 114.3997f);
					Var0.Rot = new Vector3(0f, 0f, -153.3093f);
					break;

				case 37:
					Var0.Coord = new Vector3(-892.5499f, -430.4789f, 88.25368f);
					Var0.Rot = new Vector3(0f, 0f, 116.9193f);
					break;

				case 38:
					Var0.Coord = new Vector3(-35.0462f, -576.317f, 82.90739f);
					Var0.Rot = new Vector3(0f, 0f, 160f);
					break;

				case 39:
					Var0.Coord = new Vector3(-10.3788f, -590.7431f, 93.02542f);
					Var0.Rot = new Vector3(0f, 0f, 70f);
					break;

				case 65:
					Var0.Coord = new Vector3(-22.2487f, -589.1461f, 80.2305f);
					Var0.Rot = new Vector3(0f, 0f, 69.88f);
					break;

				case 40:
					Var0.Coord = new Vector3(-900.6311f, -376.7462f, 78.27306f);
					Var0.Rot = new Vector3(0f, 0f, 26.92611f);
					break;

				case 41:
					Var0.Coord = new Vector3(-929.483f, -374.5104f, 102.2329f);
					Var0.Rot = new Vector3(0f, 0f, -152.5531f);
					break;

				case 63:
					Var0.Coord = new Vector3(-914.4202f, -375.8189f, 114.4743f);
					Var0.Rot = new Vector3(0f, 0f, -63f);
					break;

				case 42:
					Var0.Coord = new Vector3(-617.1647f, 64.6042f, 100.8196f);
					Var0.Rot = new Vector3(0f, 0f, 180f);
					break;

				case 43:
					Var0.Coord = new Vector3(-584.2015f, 42.7133f, 86.4187f);
					Var0.Rot = new Vector3(0f, 0f, 0f);
					break;

				case 64:
					Var0.Coord = new Vector3(-609.5665f, 50.2203f, 98.3998f);
					Var0.Rot = new Vector3(0f, 0f, -90f);
					break;

				case 73:
					Var0.Coord = new Vector3(-171.3969f, 494.2671f, 134.4935f);
					Var0.Rot = new Vector3(0f, 0f, 11f);
					break;

				case 74:
					Var0.Coord = new Vector3(339.4982f, 434.0887f, 146.2206f);
					Var0.Rot = new Vector3(0f, 0f, -63.5f);
					break;

				case 75:
					Var0.Coord = new Vector3(-761.3884f, 615.7333f, 140.9805f);
					Var0.Rot = new Vector3(0f, 0f, -71.5f);
					break;

				case 76:
					Var0.Coord = new Vector3(-678.1752f, 591.0076f, 142.2196f);
					Var0.Rot = new Vector3(0f, 0f, 40.5f);
					break;

				case 77:
					Var0.Coord = new Vector3(120.0541f, 553.793f, 181.0943f);
					Var0.Rot = new Vector3(0f, 0f, 6f);
					break;

				case 78:
					Var0.Coord = new Vector3(-571.4039f, 655.2008f, 142.6293f);
					Var0.Rot = new Vector3(0f, 0f, -14.5f);
					break;

				case 79:
					Var0.Coord = new Vector3(-742.2565f, 587.6547f, 143.0577f);
					Var0.Rot = new Vector3(0f, 0f, -29f);
					break;

				case 80:
					Var0.Coord = new Vector3(-857.2222f, 685.051f, 149.6502f);
					Var0.Rot = new Vector3(0f, 0f, 4.5f);
					break;

				case 81:
					Var0.Coord = new Vector3(-1287.65f, 443.2707f, 94.6919f);
					Var0.Rot = new Vector3(0f, 0f, 0f);
					break;

				case 82:
					Var0.Coord = new Vector3(374.2012f, 416.9688f, 142.6977f);
					Var0.Rot = new Vector3(0f, 0f, -14f);
					break;

				case 83:
					Var0.Coord = new Vector3(-787.7805f, 334.9232f, 186.1134f);
					Var0.Rot = new Vector3(0f, 0f, 90f);
					break;

				case 84:
					Var0.Coord = new Vector3(-787.7805f, 334.9232f, 215.8384f);
					Var0.Rot = new Vector3(0f, 0f, 90f);
					break;

				case 85:
					Var0.Coord = new Vector3(-773.2258f, 322.8252f, 194.8862f);
					Var0.Rot = new Vector3(0f, 0f, -90f);
					break;

				case 86:
					Var0.Coord = new Vector3(-1573.098f, -4085.806f, 9.7851f);
					Var0.Rot = new Vector3(0f, 0f, 162f);
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
					Var0.Coord = new Vector3(342.8157f, -997.4288f, -100f);
					Var0.Rot = new Vector3(0f, 0f, 0f);
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
					Var0.Coord = new Vector3(260.3297f, -997.4288f, -100f);
					Var0.Rot = new Vector3(0f, 0f, 0f);
					break;

				case 87:
					Var0.Coord = new Vector3(-1572.187f, -570.8315f, 109.9879f);
					Var0.Rot = new Vector3(0f, 0f, -54f);
					break;

				case 88:
					Var0.Coord = new Vector3(-1383.954f, -476.7112f, 73.507f);
					Var0.Rot = new Vector3(0f, 0f, 8f);
					break;

				case 89:
					Var0.Coord = new Vector3(-138.0029f, -629.739f, 170.2854f);
					Var0.Rot = new Vector3(0f, 0f, -84f);
					break;

				case 90:
					Var0.Coord = new Vector3(-74.8895f, -817.6883f, 244.8508f);
					Var0.Rot = new Vector3(0f, 0f, 70f);
					break;

				case 91:
				case 92:
				case 93:
				case 94:
				case 95:
				case 96:
					Var0.Coord = new Vector3(1100.764f, -3159.384f, -34.9342f);
					Var0.Rot = new Vector3(0f, 0f, 0f);
					break;

				case 97:
				case 98:
				case 99:
				case 100:
				case 101:
				case 102:
					Var0.Coord = new Vector3(1005.806f, -3157.67f, -36.0897f);
					Var0.Rot = new Vector3(0f, 0f, 0f);
					break;

				case 103:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-1576.571f, -569.7595f, 85.5f);
						Var0.Rot = new Vector3(0f, 0f, 36.1f);
					}
					else
					{
						Var0.Coord = new Vector3(-1578.022f, -576.4251f, 104.2f);
						Var0.Rot = new Vector3(0f, 0f, -144.04f);
					}
					break;

				case 104:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-1571.254f, -566.5865f, 85.5f);
						Var0.Rot = new Vector3(0f, 0f, -53.9f);
					}
					else
					{
						Var0.Coord = new Vector3(-1578.022f, -576.4251f, 104.2f);
						Var0.Rot = new Vector3(0f, 0f, -144.04f);
					}
					break;

				case 105:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-1568.098f, -571.9171f, 85.5f);
						Var0.Rot = new Vector3(0f, 0f, -143.9f);
					}
					else
					{
						Var0.Coord = new Vector3(-1578.022f, -576.4251f, 104.2f);
						Var0.Rot = new Vector3(0f, 0f, -144.04f);
					}
					break;

				case 106:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-1384.518f, -475.8657f, 56.1f);
						Var0.Rot = new Vector3(0f, 0f, 98.7f);
					}
					else
					{
						Var0.Coord = new Vector3(-1391.245f, -473.9638f, 77.2f);
						Var0.Rot = new Vector3(0f, 0f, 98.86f);
					}
					break;

				case 107:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-1384.538f, -475.8829f, 48.1f);
						Var0.Rot = new Vector3(0f, 0f, 98.7f);
					}
					else
					{
						Var0.Coord = new Vector3(-1391.245f, -473.9638f, 77.2f);
						Var0.Rot = new Vector3(0f, 0f, 98.86f);
					}
					break;

				case 108:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-1378.994f, -477.2481f, 56.1f);
						Var0.Rot = new Vector3(0f, 0f, -81.1f);
					}
					else
					{
						Var0.Coord = new Vector3(-1391.245f, -473.9638f, 77.2f);
						Var0.Rot = new Vector3(0f, 0f, 98.86f);
					}
					break;

				case 109:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-186.5683f, -576.4624f, 135f);
						Var0.Rot = new Vector3(0f, 0f, 96.16f);
					}
					else
					{
						Var0.Coord = new Vector3(-146.6167f, -596.6301f, 166f);
						Var0.Rot = new Vector3(0f, 0f, -140f);
					}
					break;

				case 110:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-113.886f, -564.3862f, 135f);
						Var0.Rot = new Vector3(0f, 0f, 110.96f);
					}
					else
					{
						Var0.Coord = new Vector3(-146.6167f, -596.6301f, 166f);
						Var0.Rot = new Vector3(0f, 0f, -140f);
					}
					break;

				case 111:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-134.6568f, -635.1774f, 135f);
						Var0.Rot = new Vector3(0f, 0f, -9.04f);
					}
					else
					{
						Var0.Coord = new Vector3(-146.6167f, -596.6301f, 166f);
						Var0.Rot = new Vector3(0f, 0f, -140f);
					}
					break;

				case 112:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-79.0479f, -822.6393f, 221f);
						Var0.Rot = new Vector3(0f, 0f, 70f);
					}
					else
					{
						Var0.Coord = new Vector3(-73.904f, -821.6204f, 284f);
						Var0.Rot = new Vector3(0f, 0f, -110f);
					}
					break;

				case 113:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-70.3086f, -819.5784f, 221f);
						Var0.Rot = new Vector3(0f, 0f, 160f);
					}
					else
					{
						Var0.Coord = new Vector3(-73.904f, -821.6204f, 284f);
						Var0.Rot = new Vector3(0f, 0f, -110f);
					}
					break;

				case 114:
					if (!bParam1)
					{
						Var0.Coord = new Vector3(-79.9861f, -818.425f, 221f);
						Var0.Rot = new Vector3(0f, 0f, -20f);
					}
					else
					{
						Var0.Coord = new Vector3(-73.904f, -821.6204f, 284f);
						Var0.Rot = new Vector3(0f, 0f, -110f);
					}
					break;
			}
			return Var0;
		}


		static TvCoord func_7(int uParam0, int iParam1)
		{
			TvCoord Var0 = new TvCoord();
			OutputArgument Coord = new OutputArgument();
			OutputArgument Rot = new OutputArgument();
			testino uVar6;

			if (Function.Call<bool>(Hash._GET_BASE_ELEMENT_METADATA, Coord, Rot, uParam0, false))
			{
				Var0.Coord = Coord.GetResult<Vector3>();
				Var0.Rot = Rot.GetResult<Vector3>();
				return Var0;
			}
			return Var0;
		}

		static int func_17(int iParam0)
		{
			switch (iParam0)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					return 1;
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 69:
				case 68:
				case 66:
				case 67:
					return 8;
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
					return 17;
				case 61:
				case 62:
				case 63:
				case 64:
				case 65:
					return 61;
				case 73:
				case 74:
				case 75:
				case 76:
					return 73;
				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
					return 77;
				case 83:
				case 84:
				case 85:
					return 83;
				case 86:
					return 86;
				case 87:
				case 88:
				case 89:
				case 90:
					return 88;
				case 91:
				case 92:
				case 93:
				case 94:
				case 95:
				case 96:
					return 91;
				case 97:
				case 98:
				case 99:
				case 100:
				case 101:
				case 102:
					return 97;
				case 103:
				case 106:
				case 109:
				case 112:
				case 104:
				case 107:
				case 110:
				case 113:
				case 105:
				case 108:
				case 111:
				case 114:
					return 109;
			}
			return -1;
		}

		static testino func_8(int iParam0)
		{
			testino uParam1 = new testino();
			switch (iParam0)
			{
				case -1:
				case 1:
					uParam1.val1 = 0;
					uParam1.val2 = "BaseElementLocationsMap";
					break;
				case 61:
					uParam1.val1 = 1;
					uParam1.val2 = "BaseElementLocationsMap_HighApt";
					break;
				case 73:
					uParam1.val1 = 2;
					uParam1.val2 = "ExtraBaseElementLocMap1";
					break;
				case 77:
					uParam1.val1 = 3;
					uParam1.val2 = "ExtraBaseElementLocMap2";
					break;
				case 83:
					uParam1.val1 = 4;
					uParam1.val2 = "ExtraBaseElementLocMap3";
					break;
				case 86:
					uParam1.val1 = 5;
					uParam1.val2 = "ExtraBaseElementLocMap4";
					break;
				case 88:
					uParam1.val1 = 6;
					uParam1.val2 = "ExtraBaseElementLocMap5";
					break;
				case 91:
					uParam1.val1 = 7;
					uParam1.val2 = "ExtraBaseElementLocMap6";
					break;
				case 97:
					uParam1.val1 = 8;
					uParam1.val2 = "ExtraBaseElementLocMap7";
					break;
				case 109:
					uParam1.val1 = 9;
					uParam1.val2 = "ExtraBaseElementLocMap8";
					break;
			}
			return uParam1;
		}











	}

	public class Televisione
	{
		public int Canale = 0;
		public Prop Entity;
		public bool Accesa = false;
		public float Volume = 0.5f;
	}

	internal class TvCoord
	{
		public Vector3 Coord = new Vector3();
		public Vector3 Rot = new Vector3();

		public TvCoord() { }
		public TvCoord(Vector3 coord, Vector3 rot)
		{
			Coord = coord;
			Rot = rot;
		}
	}
}


