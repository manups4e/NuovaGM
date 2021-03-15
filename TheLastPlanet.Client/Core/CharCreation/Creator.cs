using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Impostazioni.Client.Configurazione.Negozi.Abiti;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core.CharCreation
{
	internal static class Creator
	{
		private static MenuPool pool = HUD.MenuPool;
		private static List<dynamic> momfaces = new List<dynamic>()
		{
			"Hannah",
			"Audrey",
			"Jasmine",
			"Giselle",
			"Amelia",
			"Isabella",
			"Zoe",
			"Ava",
			"Camilla",
			"Violet",
			"Sophia",
			"Eveline",
			"Nicole",
			"Ashley",
			"Grace",
			"Brianna",
			"Natalie",
			"Olivia",
			"Elizabeth",
			"Charlotte",
			"Emma",
			"Misty"
		};
		private static List<dynamic> dadfaces = new List<dynamic>()
		{
			"Benjamin",
			"Daniel",
			"Joshua",
			"Noah",
			"Andrew",
			"Joan",
			"Alex",
			"Isaac",
			"Evan",
			"Ethan",
			"Vincent",
			"Angel",
			"Diego",
			"Adrian",
			"Gabriel",
			"Michael",
			"Santiago",
			"Kevin",
			"Louis",
			"Samuel",
			"Anthony",
			"Claude",
			"Niko",
			"John"
		};
		private static List<dynamic> HairUomo = new List<dynamic>()
		{
			GetLabelText("CC_M_HS_0"),
			GetLabelText("CC_M_HS_1"),
			GetLabelText("CC_M_HS_2"),
			GetLabelText("CC_M_HS_3"),
			GetLabelText("CC_M_HS_4"),
			GetLabelText("CC_M_HS_5"),
			GetLabelText("CC_M_HS_6"),
			GetLabelText("CC_M_HS_7"),
			GetLabelText("CC_M_HS_8"),
			GetLabelText("CC_M_HS_9"),
			GetLabelText("CC_M_HS_10"),
			GetLabelText("CC_M_HS_11"),
			GetLabelText("CC_M_HS_12"),
			GetLabelText("CC_M_HS_13"),
			GetLabelText("CC_M_HS_14"),
			GetLabelText("CC_M_HS_15"),
			GetLabelText("CC_M_HS_16"),
			GetLabelText("CC_M_HS_17"),
			GetLabelText("CC_M_HS_18"),
			GetLabelText("CC_M_HS_19"),
			GetLabelText("CC_M_HS_20"),
			GetLabelText("CC_M_HS_21"),
			GetLabelText("CC_M_HS_22")
		};
		private static List<dynamic> HairDonna = new List<dynamic>()
		{
			GetLabelText("CC_F_HS_0"),
			GetLabelText("CC_F_HS_1"),
			GetLabelText("CC_F_HS_2"),
			GetLabelText("CC_F_HS_3"),
			GetLabelText("CC_F_HS_4"),
			GetLabelText("CC_F_HS_5"),
			GetLabelText("CC_F_HS_6"),
			GetLabelText("CC_F_HS_7"),
			GetLabelText("CC_F_HS_8"),
			GetLabelText("CC_F_HS_9"),
			GetLabelText("CC_F_HS_10"),
			GetLabelText("CC_F_HS_11"),
			GetLabelText("CC_F_HS_12"),
			GetLabelText("CC_F_HS_13"),
			GetLabelText("CC_F_HS_14"),
			GetLabelText("CC_F_HS_15"),
			GetLabelText("CC_F_HS_16"),
			GetLabelText("CC_F_HS_17"),
			GetLabelText("CC_F_HS_18"),
			GetLabelText("CC_F_HS_19"),
			GetLabelText("CC_F_HS_20"),
			GetLabelText("CC_F_HS_21"),
			GetLabelText("CC_F_HS_22"),
			GetLabelText("CC_F_HS_23")
		};
		private static List<dynamic> Beards = new List<dynamic>()
		{
			GetLabelText("FACE_F_P_OFF"),
			GetLabelText("CC_BEARD_0"),
			GetLabelText("CC_BEARD_1"),
			GetLabelText("CC_BEARD_2"),
			GetLabelText("CC_BEARD_3"),
			GetLabelText("CC_BEARD_4"),
			GetLabelText("CC_BEARD_5"),
			GetLabelText("CC_BEARD_6"),
			GetLabelText("CC_BEARD_7"),
			GetLabelText("CC_BEARD_8"),
			GetLabelText("CC_BEARD_9"),
			GetLabelText("CC_BEARD_10"),
			GetLabelText("CC_BEARD_11"),
			GetLabelText("CC_BEARD_12"),
			GetLabelText("CC_BEARD_13"),
			GetLabelText("CC_BEARD_14"),
			GetLabelText("CC_BEARD_15"),
			GetLabelText("CC_BEARD_16"),
			GetLabelText("CC_BEARD_17"),
			GetLabelText("CC_BEARD_18"),
			GetLabelText("CC_BEARD_19"),
			GetLabelText("CC_BEARD_20"),
			GetLabelText("CC_BEARD_21"),
			GetLabelText("CC_BEARD_22"),
			GetLabelText("CC_BEARD_23"),
			GetLabelText("CC_BEARD_24"),
			GetLabelText("CC_BEARD_25"),
			GetLabelText("CC_BEARD_26"),
			GetLabelText("CC_BEARD_27"),
			GetLabelText("CC_BEARD_28")
		};
		private static List<dynamic> eyebrow = new List<dynamic>()
		{
			GetLabelText("CC_EYEBRW_0"),
			GetLabelText("CC_EYEBRW_1"),
			GetLabelText("CC_EYEBRW_2"),
			GetLabelText("CC_EYEBRW_3"),
			GetLabelText("CC_EYEBRW_4"),
			GetLabelText("CC_EYEBRW_5"),
			GetLabelText("CC_EYEBRW_6"),
			GetLabelText("CC_EYEBRW_7"),
			GetLabelText("CC_EYEBRW_8"),
			GetLabelText("CC_EYEBRW_9"),
			GetLabelText("CC_EYEBRW_10"),
			GetLabelText("CC_EYEBRW_11"),
			GetLabelText("CC_EYEBRW_12"),
			GetLabelText("CC_EYEBRW_13"),
			GetLabelText("CC_EYEBRW_14"),
			GetLabelText("CC_EYEBRW_15"),
			GetLabelText("CC_EYEBRW_16"),
			GetLabelText("CC_EYEBRW_17"),
			GetLabelText("CC_EYEBRW_18"),
			GetLabelText("CC_EYEBRW_19"),
			GetLabelText("CC_EYEBRW_20"),
			GetLabelText("CC_EYEBRW_21"),
			GetLabelText("CC_EYEBRW_22"),
			GetLabelText("CC_EYEBRW_23"),
			GetLabelText("CC_EYEBRW_24"),
			GetLabelText("CC_EYEBRW_25"),
			GetLabelText("CC_EYEBRW_26"),
			GetLabelText("CC_EYEBRW_27"),
			GetLabelText("CC_EYEBRW_28"),
			GetLabelText("CC_EYEBRW_29"),
			GetLabelText("CC_EYEBRW_30"),
			GetLabelText("CC_EYEBRW_31"),
			GetLabelText("CC_EYEBRW_32"),
			GetLabelText("CC_EYEBRW_33")
		};
		private static List<dynamic> blemishes = new List<dynamic>()
		{
			GetLabelText("FACE_F_P_OFF"),
			GetLabelText("CC_SKINBLEM_0"),
			GetLabelText("CC_SKINBLEM_1"),
			GetLabelText("CC_SKINBLEM_2"),
			GetLabelText("CC_SKINBLEM_3"),
			GetLabelText("CC_SKINBLEM_4"),
			GetLabelText("CC_SKINBLEM_5"),
			GetLabelText("CC_SKINBLEM_6"),
			GetLabelText("CC_SKINBLEM_7"),
			GetLabelText("CC_SKINBLEM_8"),
			GetLabelText("CC_SKINBLEM_9"),
			GetLabelText("CC_SKINBLEM_10"),
			GetLabelText("CC_SKINBLEM_11"),
			GetLabelText("CC_SKINBLEM_11"),
			GetLabelText("CC_SKINBLEM_13"),
			GetLabelText("CC_SKINBLEM_14"),
			GetLabelText("CC_SKINBLEM_15"),
			GetLabelText("CC_SKINBLEM_16"),
			GetLabelText("CC_SKINBLEM_17"),
			GetLabelText("CC_SKINBLEM_18"),
			GetLabelText("CC_SKINBLEM_19"),
			GetLabelText("CC_SKINBLEM_20"),
			GetLabelText("CC_SKINBLEM_21"),
			GetLabelText("CC_SKINBLEM_22"),
			GetLabelText("CC_SKINBLEM_23")
		};
		private static List<dynamic> Ageing = new List<dynamic>()
		{
			GetLabelText("FACE_F_P_OFF"),
			GetLabelText("CC_SKINAGE_0"),
			GetLabelText("CC_SKINAGE_2"),
			GetLabelText("CC_SKINAGE_3"),
			GetLabelText("CC_SKINAGE_4"),
			GetLabelText("CC_SKINAGE_5"),
			GetLabelText("CC_SKINAGE_6"),
			GetLabelText("CC_SKINAGE_7"),
			GetLabelText("CC_SKINAGE_8"),
			GetLabelText("CC_SKINAGE_9"),
			GetLabelText("CC_SKINAGE_10"),
			GetLabelText("CC_SKINAGE_11"),
			GetLabelText("CC_SKINAGE_12"),
			GetLabelText("CC_SKINAGE_13"),
			GetLabelText("CC_SKINAGE_14")
		};
		private static List<dynamic> Complexions = new List<dynamic>()
		{
			GetLabelText("FACE_F_P_OFF"),
			GetLabelText("CC_SKINCOM_0"),
			GetLabelText("CC_SKINCOM_1"),
			GetLabelText("CC_SKINCOM_2"),
			GetLabelText("CC_SKINCOM_3"),
			GetLabelText("CC_SKINCOM_4"),
			GetLabelText("CC_SKINCOM_5"),
			GetLabelText("CC_SKINCOM_6"),
			GetLabelText("CC_SKINCOM_7"),
			GetLabelText("CC_SKINCOM_8"),
			GetLabelText("CC_SKINCOM_9"),
			GetLabelText("CC_SKINCOM_10"),
			GetLabelText("CC_SKINCOM_11")
		};
		private static List<dynamic> Nei_e_Porri = new List<dynamic>()
		{
			GetLabelText("FACE_F_P_OFF"),
			GetLabelText("CC_MOLEFRECK_0"),
			GetLabelText("CC_MOLEFRECK_1"),
			GetLabelText("CC_MOLEFRECK_2"),
			GetLabelText("CC_MOLEFRECK_3"),
			GetLabelText("CC_MOLEFRECK_4"),
			GetLabelText("CC_MOLEFRECK_5"),
			GetLabelText("CC_MOLEFRECK_6"),
			GetLabelText("CC_MOLEFRECK_7"),
			GetLabelText("CC_MOLEFRECK_8"),
			GetLabelText("CC_MOLEFRECK_9"),
			GetLabelText("CC_MOLEFRECK_10"),
			GetLabelText("CC_MOLEFRECK_11"),
			GetLabelText("CC_MOLEFRECK_12"),
			GetLabelText("CC_MOLEFRECK_13"),
			GetLabelText("CC_MOLEFRECK_14"),
			GetLabelText("CC_MOLEFRECK_15"),
			GetLabelText("CC_MOLEFRECK_16"),
			GetLabelText("CC_MOLEFRECK_17")
		};
		private static List<dynamic> Danni_Pelle = new List<dynamic>()
		{
			GetLabelText("FACE_F_P_OFF"),
			GetLabelText("CC_SUND_0"),
			GetLabelText("CC_SUND_1"),
			GetLabelText("CC_SUND_2"),
			GetLabelText("CC_SUND_3"),
			GetLabelText("CC_SUND_4"),
			GetLabelText("CC_SUND_5"),
			GetLabelText("CC_SUND_6"),
			GetLabelText("CC_SUND_7"),
			GetLabelText("CC_SUND_8"),
			GetLabelText("CC_SUND_9"),
			GetLabelText("CC_SUND_10")
		};
		private static List<dynamic> Colore_Occhi = new List<dynamic>()
		{
			GetLabelText("FACE_E_C_0"),
			GetLabelText("FACE_E_C_1"),
			GetLabelText("FACE_E_C_2"),
			GetLabelText("FACE_E_C_3"),
			GetLabelText("FACE_E_C_4"),
			GetLabelText("FACE_E_C_5"),
			GetLabelText("FACE_E_C_6"),
			GetLabelText("FACE_E_C_7"),
			GetLabelText("FACE_E_C_8")
		};
		private static List<dynamic> Trucco_Occhi = new List<dynamic>()
		{
			GetLabelText("FACE_F_P_OFF"),
			GetLabelText("CC_MKUP_0"),
			GetLabelText("CC_MKUP_1"),
			GetLabelText("CC_MKUP_2"),
			GetLabelText("CC_MKUP_3"),
			GetLabelText("CC_MKUP_4"),
			GetLabelText("CC_MKUP_5"),
			GetLabelText("CC_MKUP_6"),
			GetLabelText("CC_MKUP_7"),
			GetLabelText("CC_MKUP_8"),
			GetLabelText("CC_MKUP_9"),
			GetLabelText("CC_MKUP_10"),
			GetLabelText("CC_MKUP_11"),
			GetLabelText("CC_MKUP_12"),
			GetLabelText("CC_MKUP_13"),
			GetLabelText("CC_MKUP_14"),
			GetLabelText("CC_MKUP_15"),
			GetLabelText("CC_MKUP_32"),
			GetLabelText("CC_MKUP_34"),
			GetLabelText("CC_MKUP_35"),
			GetLabelText("CC_MKUP_36"),
			GetLabelText("CC_MKUP_37"),
			GetLabelText("CC_MKUP_38"),
			GetLabelText("CC_MKUP_39"),
			GetLabelText("CC_MKUP_40"),
			GetLabelText("CC_MKUP_41")
		};
		private static List<dynamic> BlusherDonna = new List<dynamic>()
		{
			GetLabelText("FACE_F_P_OFF"),
			GetLabelText("CC_BLUSH_0"),
			GetLabelText("CC_BLUSH_1"),
			GetLabelText("CC_BLUSH_2"),
			GetLabelText("CC_BLUSH_3"),
			GetLabelText("CC_BLUSH_4"),
			GetLabelText("CC_BLUSH_5"),
			GetLabelText("CC_BLUSH_6")
		};
		private static List<dynamic> Lipstick = new List<dynamic>()
		{
			GetLabelText("FACE_F_P_OFF"),
			GetLabelText("CC_LIPSTICK_0"),
			GetLabelText("CC_LIPSTICK_1"),
			GetLabelText("CC_LIPSTICK_2"),
			GetLabelText("CC_LIPSTICK_3"),
			GetLabelText("CC_LIPSTICK_4"),
			GetLabelText("CC_LIPSTICK_5"),
			GetLabelText("CC_LIPSTICK_6"),
			GetLabelText("CC_LIPSTICK_7"),
			GetLabelText("CC_LIPSTICK_8"),
			GetLabelText("CC_LIPSTICK_9")
		};

		private static List<Completo> CompletiMaschio = new List<Completo> { new Completo("Lo spacciatore", "Per la produzione settimanale", 0, new ComponentDrawables(-1, 0, -1, 0, 0, -1, 15, 0, 15, 0, 0, 56), new ComponentDrawables(-1, 0, -1, 0, 4, -1, 14, 0, 0, 0, 0, 0), new PropIndices(13, -1, -1, -1, -1, -1, -1, -1, -1), new PropIndices(1, -1, -1, -1, -1, -1, -1, -1, -1)), new Completo("L'Elegante", "Ma non troppo!", 0, new ComponentDrawables(-1, 0, -1, 1, 4, -1, 10, 0, 12, 0, 0, 4), new ComponentDrawables(-1, 0, -1, 0, 1, -1, 12, 0, 10, 0, 0, 0), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1)), new Completo("L'Hipster", "Non mi guardate..", 0, new ComponentDrawables(-1, 0, -1, 11, 26, -1, 22, 11, 15, 0, 0, 42), new ComponentDrawables(-1, 0, -1, 0, 3, -1, 7, 2, 0, 0, 0, 0), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1)) };
		private static List<Completo> CompletiFemmina = new List<Completo> { new Completo("La Rancher", "Muuuuuh!", 0, new ComponentDrawables(-1, 0, -1, 9, 1, -1, 3, 5, 3, 0, 0, 9), new ComponentDrawables(-1, 0, -1, 0, 10, -1, 8, 4, 0, 0, 0, 13), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1)), new Completo("La Stracciona Elegante", "Law and Order", 0, new ComponentDrawables(-1, 0, -1, 88, 1, -1, 29, 20, 37, 0, 0, 52), new ComponentDrawables(-1, 0, -1, 0, 6, -1, 0, 5, 0, 0, 0, 0), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1)), new Completo("Casual", "Per ogni giorno dell'anno", 0, new ComponentDrawables(-1, 0, -1, 3, 0, -1, 10, 1, 3, 0, 0, 3), new ComponentDrawables(-1, 0, -1, 0, 0, -1, 2, 1, 0, 0, 0, 1), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1)) };
		//DA SPOSTARE NELLA SEZIONE DEL TELEFONO FORSE
		private static string[] Prefisso = new string[6] { "333", "347", "338", "345", "329", "361" };
		private static float currentH = 0;
		private static List<string> scenari = new List<string>
		{
			"WORLD_HUMAN_AA_SMOKE",
			"WORLD_HUMAN_BINOCULARS",
			"WORLD_HUMAN_BUM_FREEWAY",
			"WORLD_HUMAN_BUM_SLUMPED",
			"WORLD_HUMAN_BUM_STANDING",
			"WORLD_HUMAN_BUM_WASH",
			"WORLD_HUMAN_CAR_PARK_ATTENDANT",
			"WORLD_HUMAN_CHEERING",
			"WORLD_HUMAN_CLIPBOARD",
			"WORLD_HUMAN_CONST_DRILL",
			"WORLD_HUMAN_COP_IDLES",
			"WORLD_HUMAN_DRINKING",
			"WORLD_HUMAN_DRUG_DEALER",
			"WORLD_HUMAN_DRUG_DEALER_HARD",
			"WORLD_HUMAN_MOBILE_FILM_SHOCKING",
			"WORLD_HUMAN_GARDENER_LEAF_BLOWER",
			"WORLD_HUMAN_GARDENER_PLANT",
			"WORLD_HUMAN_GOLF_PLAYER",
			"WORLD_HUMAN_GUARD_PATROL",
			"WORLD_HUMAN_GUARD_STAND",
			"WORLD_HUMAN_GUARD_STAND_ARMY",
			"WORLD_HUMAN_HAMMERING",
			"WORLD_HUMAN_HANG_OUT_STREET",
			"WORLD_HUMAN_HUMAN_STATUE",
			"WORLD_HUMAN_JANITOR",
			"WORLD_HUMAN_JOG_STANDING",
			"WORLD_HUMAN_LEANING",
			"WORLD_HUMAN_MAID_CLEAN",
			"WORLD_HUMAN_MUSCLE_FLEX",
			"WORLD_HUMAN_MUSCLE_FREE_WEIGHTS",
			"WORLD_HUMAN_MUSICIAN",
			"WORLD_HUMAN_PAPARAZZI",
			"WORLD_HUMAN_PARTYING",
			"WORLD_HUMAN_PROSTITUTE_HIGH_CLASS",
			"WORLD_HUMAN_PUSH_UPS",
			"WORLD_HUMAN_SECURITY_SHINE_TORCH",
			"WORLD_HUMAN_SIT_UPS",
			"WORLD_HUMAN_SMOKING",
			"WORLD_HUMAN_SMOKING_POT",
			"WORLD_HUMAN_STAND_FISHING",
			"WORLD_HUMAN_STAND_MOBILE",
			"WORLD_HUMAN_STRIP_WATCH_STAND",
			"WORLD_HUMAN_TENNIS_PLAYER",
			"WORLD_HUMAN_TOURIST_MAP",
			"WORLD_HUMAN_TOURIST_MOBILE",
			"WORLD_HUMAN_WELDING",
			"WORLD_HUMAN_YOGA",
			"CODE_HUMAN_MEDIC_KNEEL",
			"CODE_HUMAN_MEDIC_TEND_TO_DEAD",
			"CODE_HUMAN_MEDIC_TIME_OF_DEATH"
		};
		private static Char_data _dataMaschio;
		private static Char_data _dataFemmina;
		private static readonly Ped p1 = new Ped(0);
		private static string _selezionato = "";
		private static readonly Camera cam2 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
		private static Scaleform _boardScalep1 = new Scaleform("mugshot_board_01");
		private static int _handle1;
		private static string _a;
		private static string _b;
		private static string _c;
		private static string _d;
		private static Char_data _data;
		private static Camera _ncam;
		private static Ped _dummyPed;

		public static void Init()
		{
			ClientSession.Instance.AddEventHandler("lprp:sceltaCharCreation", new Action<string>(SceltaCreatoreAsync));
			ClientSession.Instance.AddEventHandler("lprp:aggiornaModel", new Action<string>(AggiornaModel));
			ClientSession.Instance.AddTick(Scaleform);
			sub_8d2b2();
		}

		private static async void AggiornaModel(string JsonData)
		{
			Char_data plpl = JsonData.DeserializeFromJson<Char_data>();
			uint hash = (uint)GetHashKey(plpl.skin.model);
			RequestModel(hash);
			while (!HasModelLoaded(hash)) await BaseScript.Delay(1);
			SetPlayerModel(PlayerId(), hash);
			CachePlayer.Cache.MyPlayer.UpdatePedId();
			await UpdateFace(PlayerPedId(), plpl.skin);
			await UpdateDress(PlayerPedId(), plpl.dressing);
		}

		#region Creazione

		#region PRE CREAZIONE

		public static async void CharCreationMenu(IDictionary<string, object> nuiData)
		{
			try
			{
				_dummyPed = await Funzioni.CreatePedLocally(PedHash.FreemodeFemale01, CachePlayer.Cache.MyPlayer.Ped.Position + new Vector3(10));
				_dummyPed.IsVisible = false;
				_dummyPed.IsPositionFrozen = false;
				_dummyPed.IsCollisionEnabled = false;
				_dummyPed.IsCollisionProof = false;
				_dummyPed.BlockPermanentEvents = true;
				string nome = nuiData["nome"] as string;
				string cognome = nuiData["cogn"] as string;
				string dob = nuiData["dob"] as string;
				string sesso = nuiData["sesso"] as string;
				ClientSession.Instance.AddTick(Controllo);
				_selezionato = sesso;
				long assicurazione = Funzioni.GetRandomLong(999999999999999);
				//Vector3 spawna = new Vector3(Main.charCreateCoords.X, Main.charCreateCoords.Y, Main.charCreateCoords.Z);
				if (IsValidInterior(94722)) LoadInterior(94722);
				while (!IsInteriorReady(94722)) await BaseScript.Delay(1000);
				sub_8d2b2();
				CachePlayer.Cache.MyPlayer.User.StatiPlayer.Istanza.Istanzia("CreazionePersonaggio");
				_dataMaschio = new Char_data((uint)CachePlayer.Cache.MyPlayer.User.Characters.Count + 1, new Info(nome, cognome, dob, 180, Convert.ToInt64(Prefisso[Funzioni.GetRandomInt(Prefisso.Length)] + Funzioni.GetRandomInt(1000000, 9999999)), assicurazione), new Finance(1000, 3000, 0), new Job("Disoccupato", 0), new Gang("Incensurato", 0), new Skin(sesso, "mp_m_freemode_01", 0.9f, (float)Math.Round(GetRandomFloatInRange(.5f, 1f), 1), new Face(GetRandomIntInRange(0, momfaces.Count), GetRandomIntInRange(0, dadfaces.Count), new float[20] { (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1) }), new A2(GetRandomIntInRange(0, Ageing.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A2(255, 0f), new A2(GetRandomIntInRange(0, blemishes.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A2(GetRandomIntInRange(0, Complexions.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A2(GetRandomIntInRange(0, Danni_Pelle.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A2(GetRandomIntInRange(0, Nei_e_Porri.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A3(255, 0f, new int[2] { 0, 0 }), new A3(255, 0f, new int[2] { 0, 0 }), new Facial(new A3(GetRandomIntInRange(0, Beards.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) }), new A3(GetRandomIntInRange(0, eyebrow.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) })), new Hair(GetRandomIntInRange(0, HairUomo.Count), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) }), new Eye(GetRandomIntInRange(0, Colore_Occhi.Count)), new Ears(255, 0)), new Dressing("Iniziale", "Per cominciare", new ComponentDrawables(-1, 0, GetPedDrawableVariation(PlayerPedId(), 2), 0, 0, -1, 15, 0, 15, 0, 0, 56), new ComponentDrawables(-1, 0, GetPedTextureVariation(PlayerPedId(), 2), 0, 4, -1, 14, 0, 0, 0, 0, 0), new PropIndices(-1, GetPedPropIndex(PlayerPedId(), 2), -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, GetPedPropTextureIndex(PlayerPedId(), 2), -1, -1, -1, -1, -1, -1, -1)), new List<Weapons>(), new List<Inventory>(), new Needs(), new Statistiche(), false);
				_dataFemmina = new Char_data((uint)CachePlayer.Cache.MyPlayer.User.Characters.Count + 1, new Info(nome, cognome, dob, 160, Convert.ToInt64(Prefisso[Funzioni.GetRandomInt(Prefisso.Length)] + Funzioni.GetRandomInt(1000000, 9999999)), assicurazione), new Finance(1000, 3000, 0), new Job("Disoccupato", 0), new Gang("Incensurato", 0), new Skin(sesso, "mp_f_freemode_01", 0.1f, (float)Math.Round(GetRandomFloatInRange(0f, .5f), 1), new Face(GetRandomIntInRange(0, momfaces.Count), GetRandomIntInRange(0, dadfaces.Count), new float[20] { (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1), (float)Math.Round(GetRandomFloatInRange(0, 1f), 1) }), new A2(GetRandomIntInRange(0, Ageing.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A2(255, 0f), new A2(GetRandomIntInRange(0, blemishes.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A2(GetRandomIntInRange(0, Complexions.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A2(GetRandomIntInRange(0, Danni_Pelle.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A2(GetRandomIntInRange(0, Nei_e_Porri.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1)), new A3(GetRandomIntInRange(0, Lipstick.Count), 100f, new int[2] { 0, 0 }), new A3(GetRandomIntInRange(0, BlusherDonna.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) }), new Facial(new A3(255, 0f, new int[2] { 0, 0 }), new A3(GetRandomIntInRange(0, eyebrow.Count), (float)Math.Round(GetRandomFloatInRange(0f, 1f), 1), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) })), new Hair(GetRandomIntInRange(0, HairDonna.Count), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) }), new Eye(GetRandomIntInRange(0, Colore_Occhi.Count)), new Ears(255, 0)), new Dressing("Iniziale", "Per cominciare", new ComponentDrawables(-1, 0, GetPedDrawableVariation(PlayerPedId(), 2), 3, 0, -1, 10, 1, 3, 0, 0, 3), new ComponentDrawables(-1, 0, GetPedTextureVariation(PlayerPedId(), 2), 0, 0, -1, 2, 1, 0, 0, 0, 1), new PropIndices(-1, GetPedPropIndex(PlayerPedId(), 2), -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, GetPedPropTextureIndex(PlayerPedId(), 2), -1, -1, -1, -1, -1, -1, -1)), new List<Weapons>(), new List<Inventory>(), new Needs(), new Statistiche(), false);
				_data = _selezionato.ToLower() == "maschio" ? _dataMaschio : _dataFemmina;
				BaseScript.TriggerEvent("lprp:aggiornaModel", _data.SerializeToJson());
				await BaseScript.Delay(1000);
				CachePlayer.Cache.MyPlayer.Ped.Position = new Vector3(402.91f, -996.74f, -180.00025f);
				while (!HasCollisionLoadedAroundEntity(CachePlayer.Cache.MyPlayer.Ped.Handle)) await BaseScript.Delay(1);
				CachePlayer.Cache.MyPlayer.Ped.IsVisible = true;
				CachePlayer.Cache.MyPlayer.Ped.IsPositionFrozen = false;
				CachePlayer.Cache.MyPlayer.Ped.BlockPermanentEvents = true;
				ped_cre_board(_data);
				TaskWalkInToRoom(CachePlayer.Cache.MyPlayer.Ped, _selezionato == "Maschio" ? sub_7dd83(1, 0, "Maschio") : sub_7dd83(1, 0, "Femmina"));
				await BaseScript.Delay(2000);
				RenderScriptCams(true, true, 0, false, false);
				cam2.Delete();
				_ncam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
				_ncam.IsActive = true;
				_ncam.Position = new Vector3(402.7553f, -1000.622f, -98.48412f);
				_ncam.Rotation = new Vector3(-6.716503f, 0f, -0.276376f);
				_ncam.FieldOfView = 36.95373f;
				_ncam.StopShaking();
				N_0xf55e4046f6f831dc(_ncam.Handle, 3f);
				N_0xe111a7c0d200cbc5(_ncam.Handle, 1f);
				SetCamDofFnumberOfLens(_ncam.Handle, 1.2f);
				SetCamDofMaxNearInFocusDistanceBlendLevel(_ncam.Handle, 1f);
				Camera cam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
				cam.Position = new Vector3(402.7391f, -1003.981f, -98.43439f);
				cam.Rotation = new Vector3(-3.589798f, 0f, -0.276381f);
				cam.FieldOfView = 36.95373f;
				cam.StopShaking();
				N_0xf55e4046f6f831dc(cam.Handle, 7f);
				N_0xe111a7c0d200cbc5(cam.Handle, 1f);
				SetCamDofFnumberOfLens(cam.Handle, 1.2f);
				SetCamDofMaxNearInFocusDistanceBlendLevel(cam.Handle, 1f);
				cam.InterpTo(_ncam, 5000, 1, 1);
				Screen.Fading.FadeIn(800);
				await BaseScript.Delay(5000);
				cam.Delete();
				if (!Creazione.Visible) MenuCreazione(nome, cognome, dob, sesso);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, "CharCreationMenu = " + ex);
			}
		}

		public static UIMenu Creazione = new UIMenu("", "");
		public static UIMenu Info = new UIMenu("", "");
		public static UIMenu Genitori = new UIMenu("", "");
		public static UIMenu Dettagli = new UIMenu("", "");
		public static UIMenu Apparenze = new UIMenu("", "");
		public static UIMenu Apparel = new UIMenu("", "");
		private static List<dynamic> _arcSop = new List<dynamic> { "Standard", "Alte", "Basse" };
		private static List<dynamic> _occ = new List<dynamic> { "Standard", "Grandi", "Stretti" };
		private static List<dynamic> _nas = new List<dynamic> { "Standard", "Grande", "Piccolo" };
		private static UIMenuListItem _arcSopr = new UIMenuListItem("Arcate Sopraccigliari", _arcSop, 0);
		private static UIMenuListItem _occhi = new UIMenuListItem("Occhi", _occ, 0, "Guarda bene le palpebre!");
		private static UIMenuListItem _naso = new UIMenuListItem("Naso", _nas, 0);
		private static UIMenuListItem _nasoPro = new UIMenuListItem("Profilo del Naso", new List<dynamic>() { "Standard", "Breve", "Lungo" }, 0);
		private static UIMenuListItem _nasoPun = new UIMenuListItem("Punta del Naso", new List<dynamic>() { "Standard", "Punta su", "Punta giù" }, 0);
		private static UIMenuListItem _zigo = new UIMenuListItem("Zigomi", new List<dynamic>() { "Standard", "In dentro", "In fuori" }, 0);
		private static UIMenuListItem _guance = new UIMenuListItem("Guance", new List<dynamic>() { "Standard", "Magre", "Paffute" }, 0);
		private static UIMenuListItem _labbra = new UIMenuListItem("Labbra", new List<dynamic>() { "Standard", "Sottili", "Carnose" }, 0);
		private static UIMenuListItem _masce = new UIMenuListItem("Mascella", new List<dynamic>() { "Standard", "Stretta", "Larga" }, 0);
		private static UIMenuListItem _mentoPro = new UIMenuListItem("Profilo del mento", new List<dynamic>() { "Standard", "In dentro", "In fuori" }, 0);
		private static UIMenuListItem _mentoFor = new UIMenuListItem("Forma del mento", new List<dynamic>() { "Standard", "Squadrato", "A punta" }, 0);
		private static UIMenuListItem _collo = new UIMenuListItem("Collo", new List<dynamic>() { "Standard", "Stretto", "Largo" }, 0);

		#endregion

		#region CREZIONEVERA

		public static void MenuCreazione(string nome, string cognome, string datadinascita, string sesso)
		{
			try
			{
				#region Dichiarazione

				#region Menu Principale

				Screen.Fading.FadeIn(800);
				Point offset = new Point(50, 50);
				pool.MouseEdgeEnabled = false;
				InstructionalButton gzmgp = new InstructionalButton(Control.FrontendLt, "ZOOM");
				Creazione = new UIMenu("TLP Creator", "Crea un nuovo Personaggio", offset);
				Creazione.ControlDisablingEnabled = true;
				pool.Add(Creazione);
				UIMenuListItem Sesso;
				Sesso = _selezionato == "Maschio" ? new UIMenuListItem("Sesso", new List<dynamic>() { "Maschio", "Femmina" }, 0, "Decidi il Sesso") : new UIMenuListItem("Sesso", new List<dynamic>() { "Maschio", "Femmina" }, 1, "Decidi il Sesso");
				Creazione.AddItem(Sesso);
				Info = pool.AddSubMenu(Creazione, "Info Personaggio", "Nome.. Cognome..");
				Info.ControlDisablingEnabled = true;
				Info.AddInstructionalButton(gzmgp);
				Genitori = pool.AddSubMenu(Creazione, GetLabelText("FACE_HERI"), GetLabelText("FACE_MM_H3"));
				Genitori.ControlDisablingEnabled = true;
				Genitori.AddInstructionalButton(gzmgp);
				Dettagli = pool.AddSubMenu(Creazione, GetLabelText("FACE_FEAT"), GetLabelText("FACE_MM_H4"));
				Dettagli.ControlDisablingEnabled = true;
				Dettagli.AddInstructionalButton(gzmgp);
				Apparenze = pool.AddSubMenu(Creazione, GetLabelText("FACE_APP"), GetLabelText("FACE_MM_H6"));
				Apparenze.ControlDisablingEnabled = true;
				Apparenze.AddInstructionalButton(gzmgp);
				Apparel = pool.AddSubMenu(Creazione, GetLabelText("FACE_APPA"), GetLabelText("FACE_APPA_H"));
				Apparel.ControlDisablingEnabled = true;
				Apparel.AddInstructionalButton(gzmgp);
				InstructionalButton button1 = new InstructionalButton(Control.LookLeftRight, "Guarda a Destra/Sinistra");
				InstructionalButton button2 = new InstructionalButton(Control.FrontendLb, "Guarda a Sinistra");
				InstructionalButton button3 = new InstructionalButton(Control.FrontendRb, "Guarda a Destra");
				InstructionalButton button4 = new InstructionalButton(Control.LookLeftRight, "Cambia dettagli");
				InstructionalButton button5 = new InstructionalButton(Control.LookUpDown, "Cambia dettagli");
				Creazione.AddInstructionalButton(button3);
				Creazione.AddInstructionalButton(button2);
				Genitori.AddInstructionalButton(button3);
				Genitori.AddInstructionalButton(button2);
				Apparenze.AddInstructionalButton(button3);
				Apparenze.AddInstructionalButton(button2);
				Dettagli.AddInstructionalButton(button3);
				Dettagli.AddInstructionalButton(button2);
				Dettagli.AddInstructionalButton(button4);
				Dettagli.AddInstructionalButton(button5);

				#endregion

				#region Info

				UIMenuItem Nome = new UIMenuItem("Nome", "Nome Personaggio");
				Nome.SetRightLabel(nome);
				UIMenuItem Cognome = new UIMenuItem("Cognome", "Cognome Personaggio");
				Cognome.SetRightLabel(cognome);
				UIMenuItem DDN = new UIMenuItem("Data di Nascita", "Data di nascita Personaggio");
				DDN.SetRightLabel(datadinascita);
				UIMenuItem Altezza = new UIMenuItem("Altezza", "Altezza Personaggio");
				Altezza.SetRightLabel("" + _data.info.height);
				Info.AddItem(Nome);
				Info.AddItem(Cognome);
				Info.AddItem(DDN);
				Info.AddItem(Altezza);

				#endregion

				#region Genitori

				UIMenuHeritageWindow heritageWindow = new UIMenuHeritageWindow(_data.skin.face.mom, _data.skin.face.dad);
				Genitori.AddWindow(heritageWindow);
				List<dynamic> lista = new List<dynamic>();
				for (int i = 0; i < 101; i++) lista.Add(i);
				UIMenuListItem mamma = new UIMenuListItem("Mamma", momfaces, _data.skin.face.mom);
				UIMenuListItem papa = new UIMenuListItem("Papà", dadfaces, _data.skin.face.dad);
				UIMenuSliderHeritageItem resemblance = new UIMenuSliderHeritageItem(GetLabelText("FACE_H_DOM"), "", true) { Multiplier = 2, Value = (int)Math.Round(_data.skin.resemblance * 100) };
				UIMenuSliderHeritageItem skinmix = new UIMenuSliderHeritageItem(GetLabelText("FACE_H_STON"), "", true) { Multiplier = 2, Value = (int)Math.Round(_data.skin.skinmix * 100) };
				Genitori.AddItem(mamma);
				Genitori.AddItem(papa);
				Genitori.AddItem(resemblance);
				Genitori.AddItem(skinmix);

				#endregion

				#region Dettagli

				UIMenuGridPanel GridSopr = new UIMenuGridPanel("Su", "In dentro", "In fuori", "Giù", new PointF(_data.skin.face.tratti[7], _data.skin.face.tratti[6]));
				UIMenuHorizontalOneLineGridPanel GridOcch = new UIMenuHorizontalOneLineGridPanel("Stretti", "Grandi", _data.skin.face.tratti[11]);
				UIMenuGridPanel GridNaso = new UIMenuGridPanel("Su", "Stretto", "Largo", "Giù", new PointF(_data.skin.face.tratti[0], _data.skin.face.tratti[1]));
				UIMenuGridPanel GridNasoPro = new UIMenuGridPanel("Convesso", "Breve", "Lungo", "Infossato", new PointF(_data.skin.face.tratti[2], _data.skin.face.tratti[3]));
				UIMenuGridPanel GridNasoPun = new UIMenuGridPanel("Punta in su", "Rotta SX", "Rotta DX", "Punta in giù", new PointF(_data.skin.face.tratti[5], _data.skin.face.tratti[4]));
				UIMenuGridPanel GridZigo = new UIMenuGridPanel("Su", "In dentro", "In fuori", "Giù", new PointF(_data.skin.face.tratti[9], _data.skin.face.tratti[8]));
				UIMenuHorizontalOneLineGridPanel GridGuance = new UIMenuHorizontalOneLineGridPanel("Magre", "Paffute", _data.skin.face.tratti[10]);
				UIMenuHorizontalOneLineGridPanel GridLabbra = new UIMenuHorizontalOneLineGridPanel("Sottili", "Carnose", _data.skin.face.tratti[12]);
				UIMenuGridPanel GridMasce = new UIMenuGridPanel("Arrotondata", "Stretta", "Larga", "Squadrata", new PointF(_data.skin.face.tratti[13], _data.skin.face.tratti[14]));
				UIMenuGridPanel GridMentoPro = new UIMenuGridPanel("Su", "In dentro", "In fuori", "Giù", new PointF(_data.skin.face.tratti[16], _data.skin.face.tratti[15]));
				UIMenuGridPanel GridMentoFor = new UIMenuGridPanel("Arrotondato", "Squadrato", "A punta", "Fossetta", new PointF(_data.skin.face.tratti[18], _data.skin.face.tratti[17]));
				UIMenuHorizontalOneLineGridPanel GridCollo = new UIMenuHorizontalOneLineGridPanel("Stretto", "Largo", _data.skin.face.tratti[19]);
				_arcSopr.AddPanel(GridSopr);
				_occhi.AddPanel(GridOcch);
				_naso.AddPanel(GridNaso);
				_nasoPro.AddPanel(GridNasoPro);
				_nasoPun.AddPanel(GridNasoPun);
				_zigo.AddPanel(GridZigo);
				_guance.AddPanel(GridGuance);
				_labbra.AddPanel(GridLabbra);
				_masce.AddPanel(GridMasce);
				_mentoPro.AddPanel(GridMentoPro);
				_mentoFor.AddPanel(GridMentoFor);
				_collo.AddPanel(GridCollo);
				Dettagli.AddItem(_arcSopr);
				Dettagli.AddItem(_occhi);
				Dettagli.AddItem(_naso);
				Dettagli.AddItem(_nasoPro);
				Dettagli.AddItem(_nasoPun);
				Dettagli.AddItem(_zigo);
				Dettagli.AddItem(_guance);
				Dettagli.AddItem(_labbra);
				Dettagli.AddItem(_masce);
				Dettagli.AddItem(_mentoPro);
				Dettagli.AddItem(_mentoFor);
				Dettagli.AddItem(_collo);

				#endregion

				#region Apparenze

				UIMenuListItem Capelli = new UIMenuListItem("", HairUomo, _data.skin.hair.style);
				UIMenuListItem sopracciglia = new UIMenuListItem(GetLabelText("FACE_F_EYEBR"), eyebrow, _data.skin.facialHair.eyebrow.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuColorPanel soprCol1 = new UIMenuColorPanel("Colore principale", ColorPanelType.Hair);
				UIMenuColorPanel soprCol2 = new UIMenuColorPanel("Colore secondario", ColorPanelType.Hair);
				UIMenuPercentagePanel soprOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				sopracciglia.AddPanel(soprCol1);
				sopracciglia.AddPanel(soprCol2);
				sopracciglia.AddPanel(soprOp);
				UIMenuListItem Barba = new UIMenuListItem(GetLabelText("FACE_F_BEARD"), Beards, _data.skin.facialHair.beard.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuColorPanel BarbaCol1 = new UIMenuColorPanel("Colore principale", ColorPanelType.Hair);
				UIMenuColorPanel BarbaCol2 = new UIMenuColorPanel("Colore secondario", ColorPanelType.Hair);
				UIMenuPercentagePanel BarbaOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				Barba.AddPanel(BarbaCol1);
				Barba.AddPanel(BarbaCol2);
				Barba.AddPanel(BarbaOp);
				UIMenuListItem SkinBlemishes = new UIMenuListItem(GetLabelText("FACE_F_SKINB"), blemishes, _data.skin.blemishes.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuPercentagePanel BlemOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				SkinBlemishes.AddPanel(BlemOp);
				UIMenuListItem SkinAgeing = new UIMenuListItem(GetLabelText("FACE_F_SKINA"), Ageing, _data.skin.ageing.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuPercentagePanel AgeOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				SkinAgeing.AddPanel(AgeOp);
				UIMenuListItem SkinComplexion = new UIMenuListItem(GetLabelText("FACE_F_SKC"), Complexions, _data.skin.complexion.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuPercentagePanel CompOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				SkinComplexion.AddPanel(CompOp);
				UIMenuListItem SkinMoles = new UIMenuListItem(GetLabelText("FACE_F_MOLE"), Nei_e_Porri, _data.skin.freckles.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuPercentagePanel FrecOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				SkinMoles.AddPanel(FrecOp);
				UIMenuListItem SkinDamage = new UIMenuListItem(GetLabelText("FACE_F_SUND"), Danni_Pelle, _data.skin.skinDamage.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuPercentagePanel DamageOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				SkinDamage.AddPanel(DamageOp);
				UIMenuListItem EyeColor = new UIMenuListItem(GetLabelText("FACE_APP_EYE"), Colore_Occhi, _data.skin.eye.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuListItem EyeMakup = new UIMenuListItem(GetLabelText("FACE_F_EYEM"), Trucco_Occhi, _data.skin.makeup.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuPercentagePanel MakupOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				EyeMakup.AddPanel(MakupOp);
				UIMenuListItem Blusher = new UIMenuListItem(GetLabelText("FACE_F_BLUSH"), BlusherDonna, _data.skin.blusher.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuColorPanel BlushCol1 = new UIMenuColorPanel("Colore principale", ColorPanelType.Makeup);
				UIMenuColorPanel BlushCol2 = new UIMenuColorPanel("Colore secondario", ColorPanelType.Makeup);
				UIMenuPercentagePanel BlushOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				Blusher.AddPanel(BlushCol1);
				Blusher.AddPanel(BlushCol2);
				Blusher.AddPanel(BlushOp);
				UIMenuListItem LipStick = new UIMenuListItem(GetLabelText("FACE_F_LIPST"), Lipstick, _data.skin.lipstick.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
				UIMenuColorPanel LipCol1 = new UIMenuColorPanel("Colore principale", ColorPanelType.Makeup);
				UIMenuColorPanel LipCol2 = new UIMenuColorPanel("Colore secondario", ColorPanelType.Makeup);
				UIMenuPercentagePanel LipOp = new UIMenuPercentagePanel("Opacità", "0%", "100%");
				LipStick.AddPanel(LipCol1);
				LipStick.AddPanel(LipCol2);
				LipStick.AddPanel(LipOp);
				//	SetPedHeadOverlay			(playerPed, 10,		Character['chest_1'],			(Character['chest_2'] / 10) + 0.0)			-- Chest Hair + opacity
				//  SetPedHeadOverlayColor(playerPed, 10, 1, Character['chest_3'])-- Torso Color
				//	SetPedHeadOverlay(playerPed, 11, Character['bodyb_1'], (Character['bodyb_2'] / 10) + 0.0)-- Body Blemishes +opacity

				#endregion

				#endregion

				#region CorpoMenu

				#region Sesso

				Sesso.OnListChanged += async (item, _newIndex) =>
				{
					pool.CloseAllMenus();
					Screen.Effects.Start(ScreenEffect.MpCelebWin);
					await BaseScript.Delay(1000);
					Screen.Fading.FadeOut(1000);
					await BaseScript.Delay(1000);
					Screen.Effects.Stop(ScreenEffect.MpCelebWin);

					switch (_newIndex)
					{
						case 0:
							_dataFemmina = _data;
							_data = _dataMaschio;
							_boardScalep1.CallFunction("SET_BOARD", ClientSession.Impostazioni.Main.NomeServer, _data.info.firstname + " " + _data.info.lastname, "Personaggio N°", "Powered by Manups4e", 0, _data.id, 0);
							_selezionato = "Maschio";

							break;
						case 1:
							_dataMaschio = _data;
							_data = _dataFemmina;
							_boardScalep1.CallFunction("SET_BOARD", ClientSession.Impostazioni.Main.NomeServer, _data.info.firstname + " " + _data.info.lastname, "Personaggio N°", "Powered by Manups4e", 0, _data.id, 0);
							_selezionato = "Femmina";

							break;
					}

					BaseScript.TriggerEvent("lprp:aggiornaModel", _data.SerializeToJson());
					foreach (Prop obj in World.GetAllProps())
						if (obj.Model.Hash == GetHashKey("prop_police_id_board") || obj.Model.Hash == GetHashKey("prop_police_id_text"))
							Nome.SetRightLabel(_data.info.firstname);
					Cognome.SetRightLabel(_data.info.lastname);
					DDN.SetRightLabel(_data.info.dateOfBirth);
					IDictionary<string, object> a = new Dictionary<string, object> { ["nome"] = _data.info.firstname, ["cogn"] = _data.info.lastname, ["dob"] = _data.info.dateOfBirth, ["sesso"] = _selezionato };
					CharCreationMenu(a);
				};

				#endregion

				#region Info

				Info.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (_item == Nome)
					{
						N_0x3ed1438c1f5c6612(2);
						string result = await HUD.GetUserInput("Inserisci il Nome", _data.info.firstname, 30);

						if (result != null)
						{
							if (result == "Nuovo" || result == "Nome")
							{
								HUD.ShowNotification("Hai inserito un Nome non accettabile! Riprova", NotificationColor.Red, true);
							}
							else if (result.Length < 4)
							{
								HUD.ShowNotification("Hai inserito un Nome troppo corto!", NotificationColor.Red, true);
							}
							else
							{
								_data.info.firstname = result;
								Nome.SetRightLabel(result);
							}
						}
					}
					else if (_item == Cognome)
					{
						N_0x3ed1438c1f5c6612(2);
						string result = await HUD.GetUserInput("Inserisci il Cognome", _data.info.lastname, 30);

						if (result != null)
						{
							if (result == "Personaggio" || result == "Cognome")
							{
								HUD.ShowNotification("Hai inserito un Cognome non accetabile! Riprova", NotificationColor.Red, true);
							}
							else if (result.Length < 4)
							{
								HUD.ShowNotification("Hai inserito un Cognome troppo corto!", NotificationColor.Red, true);
							}
							else
							{
								_data.info.lastname = result;
								Cognome.SetRightLabel(result);
							}
						}
					}
					else if (_item == DDN)
					{
						string result = await HUD.GetUserInput("Inserisci la Data di Nascita", _data.info.dateOfBirth, 30);

						if (result != null)
						{
							if (result == "01/12/199cambiami" || result == "gg/mm/yyyy")
							{
								HUD.ShowNotification("Hai inserito la data in formato errato!\nIl ~b~formato~w~ giusto è 'gg/mm/yyyy'.", NotificationColor.Red, true);
							}
							else if (result.ToCharArray()[2] != '/' || result.ToCharArray()[5] != '/' || result.Length < 10 || result.Length > 10)
							{
								HUD.ShowNotification("Hai inserito la data in formato errato!\nIl ~b~formato~w~ giusto è 'gg/mm/yyyy'.", NotificationColor.Red, true);
							}
							else
							{
								_data.info.dateOfBirth = result;
								DDN.SetRightLabel(result);
							}
						}
					}

					if (_selezionato == "Maschio")
						_dataMaschio = _data;
					else
						_dataFemmina = _data;
					_boardScalep1.CallFunction("SET_BOARD", ClientSession.Impostazioni.Main.NomeServer, _data.info.firstname + " " + _data.info.lastname, "Personaggio N°", "Powered by Manups4e", 0, CachePlayer.Cache.MyPlayer.User.Characters.Count + 1, 0);
				};

				#endregion

				#region Genitori

				Genitori.OnListChange += async (_sender, _listItem, _newIndex) =>
				{
					if (_listItem == mamma)
					{
						_data.skin.face.mom = _newIndex;
						heritageWindow.Index(_data.skin.face.mom, _data.skin.face.dad);
					}
					else if (_listItem == papa)
					{
						_data.skin.face.dad = _newIndex;
						heritageWindow.Index(_data.skin.face.mom, _data.skin.face.dad);
					}

					if (_data.skin.sex == "Maschio")
						_dataMaschio = _data;
					else
						_dataFemmina = _data;
					UpdateFace(PlayerPedId(), _data.skin);
				};
				Genitori.OnSliderChange += async (_sender, _item, _newIndex) =>
				{
					if (_item == resemblance)
						_data.skin.resemblance = _newIndex / 100f;
					else if (_item == skinmix) _data.skin.skinmix = _newIndex / 100f;
					if (_data.skin.sex == "Maschio")
						_dataMaschio = _data;
					else
						_dataFemmina = _data;
					UpdateFace(PlayerPedId(), _data.skin);
				};

				#endregion

				#region Apparenze

				Apparenze.OnListChange += async (_sender, _listItem, _newIndex) =>
				{
					if (_listItem == Capelli)
					{
						_data.skin.hair.style = _newIndex;
						_data.skin.hair.color[0] = (_listItem.Panels[0] as UIMenuColorPanel).CurrentSelection;
						_data.skin.hair.color[1] = (_listItem.Panels[1] as UIMenuColorPanel).CurrentSelection;
					}

					if (_listItem == sopracciglia)
					{
						_data.skin.facialHair.eyebrow.style = _newIndex;
						_data.skin.facialHair.eyebrow.color[0] = (_listItem.Panels[0] as UIMenuColorPanel).CurrentSelection;
						_data.skin.facialHair.eyebrow.color[1] = (_listItem.Panels[1] as UIMenuColorPanel).CurrentSelection;
						_data.skin.facialHair.eyebrow.opacity = (_listItem.Panels[2] as UIMenuPercentagePanel).Percentage;
					}

					if (_listItem == Barba)
					{
						if (_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF"))
							_data.skin.facialHair.beard.style = 255;
						else
							_data.skin.facialHair.beard.style = _newIndex - 1;
						_data.skin.facialHair.beard.color[0] = (_listItem.Panels[0] as UIMenuColorPanel).CurrentSelection;
						_data.skin.facialHair.beard.color[1] = (_listItem.Panels[1] as UIMenuColorPanel).CurrentSelection;
						_data.skin.facialHair.beard.opacity = (_listItem.Panels[2] as UIMenuPercentagePanel).Percentage;
					}

					if (_listItem == Blusher)
					{
						if (_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF"))
							_data.skin.blusher.style = 255;
						else
							_data.skin.blusher.style = _newIndex - 1;
						_data.skin.blusher.color[0] = (_listItem.Panels[0] as UIMenuColorPanel).CurrentSelection;
						_data.skin.blusher.color[1] = (_listItem.Panels[1] as UIMenuColorPanel).CurrentSelection;
						_data.skin.blusher.opacity = (_listItem.Panels[2] as UIMenuPercentagePanel).Percentage;
					}

					if (_listItem == LipStick)
					{
						if (_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF"))
							_data.skin.lipstick.style = 255;
						else
							_data.skin.lipstick.style = _newIndex - 1;
						_data.skin.lipstick.color[0] = (_listItem.Panels[0] as UIMenuColorPanel).CurrentSelection;
						_data.skin.lipstick.color[1] = (_listItem.Panels[1] as UIMenuColorPanel).CurrentSelection;
						_data.skin.lipstick.opacity = (_listItem.Panels[2] as UIMenuPercentagePanel).Percentage;
					}

					if (_listItem == SkinBlemishes)
					{
						if (_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF"))
							_data.skin.blemishes.style = 255;
						else
							_data.skin.blemishes.style = _newIndex - 1;
						_data.skin.blemishes.opacity = (_listItem.Panels[0] as UIMenuPercentagePanel).Percentage;
					}

					if (_listItem == SkinAgeing)
					{
						if (_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF"))
							_data.skin.ageing.style = 255;
						else
							_data.skin.ageing.style = _newIndex - 1;
						_data.skin.ageing.opacity = (_listItem.Panels[0] as UIMenuPercentagePanel).Percentage;
					}

					if (_listItem == SkinComplexion)
					{
						if (_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF"))
							_data.skin.complexion.style = 255;
						else
							_data.skin.complexion.style = _newIndex - 1;
						_data.skin.complexion.opacity = (_listItem.Panels[0] as UIMenuPercentagePanel).Percentage;
					}

					if (_listItem == SkinMoles)
					{
						if (_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF"))
							_data.skin.freckles.style = 255;
						else
							_data.skin.freckles.style = _newIndex - 1;
						_data.skin.freckles.opacity = (_listItem.Panels[0] as UIMenuPercentagePanel).Percentage;
					}

					if (_listItem == SkinDamage)
					{
						if (_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF"))
							_data.skin.skinDamage.style = 255;
						else
							_data.skin.skinDamage.style = _newIndex - 1;
						_data.skin.skinDamage.opacity = (_listItem.Panels[0] as UIMenuPercentagePanel).Percentage;
					}

					if (_listItem == EyeColor) _data.skin.eye.style = _newIndex;

					if (_listItem == EyeMakup)
					{
						if (_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF"))
							_data.skin.makeup.style = 255;
						else
							_data.skin.makeup.style = _newIndex - 1;
						_data.skin.makeup.opacity = (_listItem.Panels[0] as UIMenuPercentagePanel).Percentage;
					}

					UpdateFace(PlayerPedId(), _data.skin);
				};

				#endregion

				#region Dettagli

				int oldIndexarcSopr = 0;
				int oldIndexOcchi = 0;
				int oldIndexNaso = 0;
				int oldIndexNasoPro = 0;
				int oldIndexNasoPun = 0;
				int oldIndexZigo = 0;
				int oldIndexGuance = 0;
				int oldIndexCollo = 0;
				int oldIndexLabbra = 0;
				int oldIndexMasce = 0;
				int oldIndexMentoPro = 0;
				int oldIndexMentoFor = 0;
				Dettagli.OnListChange += async (_sender, _listItem, _newIndex) =>
				{
					if (_listItem == _arcSopr)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexarcSopr != _newIndex)
							{
								switch (_listItem.Items[_newIndex])
								{
									case "Alte":
										_data.skin.face.tratti[6] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.00001f);

										break;
									case "Basse":
										_data.skin.face.tratti[6] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.999999f);

										break;
									case "Standard":
										_data.skin.face.tratti[6] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.5f);

										break;
								}

								oldIndexarcSopr = _newIndex;
							}

						PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
						_data.skin.face.tratti[7] = var.X * 2f - 1f;
						_data.skin.face.tratti[6] = var.Y * 2f - 1f;
					}

					if (_listItem == _occhi)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexOcchi != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "Grandi":
										_data.skin.face.tratti[11] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
									case "Stretti":
										_data.skin.face.tratti[11] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[11] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition;
						_data.skin.face.tratti[11] = (var.X * 2f - 1f) / -1;
					}

					if (_listItem == _naso)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexNaso != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "Piccolo":
										_data.skin.face.tratti[0] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "Grande":
										_data.skin.face.tratti[0] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[0] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
						_data.skin.face.tratti[0] = var.X * 2f - 1f;
						_data.skin.face.tratti[1] = var.Y * 2f - 1f;
					}

					if (_listItem == _nasoPro)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexNasoPro != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "Breve":
										_data.skin.face.tratti[2] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "Lungo":
										_data.skin.face.tratti[2] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[2] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
						_data.skin.face.tratti[3] = (var.Y * 2f - 1f) / 1;
						_data.skin.face.tratti[2] = var.X * 2f - 1f;
					}

					if (_listItem == _nasoPun)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexNasoPun != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "Punta su":
										_data.skin.face.tratti[5] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.00001f);

										break;
									case "Punta giù":
										_data.skin.face.tratti[5] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.999999f);

										break;
									case "Standard":
										_data.skin.face.tratti[5] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.5f);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
						_data.skin.face.tratti[5] = (var.X * 2f - 1f) / -1;
						_data.skin.face.tratti[4] = var.Y * 2f - 1f;
					}

					if (_listItem == _zigo)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexZigo != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "In dentro":
										_data.skin.face.tratti[8] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "In fuori":
										_data.skin.face.tratti[8] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[8] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
						_data.skin.face.tratti[9] = var.X * 2f - 1f;
						_data.skin.face.tratti[8] = var.Y * 2f - 1f;
					}

					if (_listItem == _guance)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexGuance != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "Paffute":
										_data.skin.face.tratti[10] = 0.00001f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
									case "Magre":
										_data.skin.face.tratti[10] = 0.999999f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[10] = 0.5f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition;
						_data.skin.face.tratti[10] = var.X;
					}

					if (_listItem == _collo)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexCollo != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "Stretto":
										_data.skin.face.tratti[19] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
									case "Largo":
										_data.skin.face.tratti[19] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[19] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition;
						_data.skin.face.tratti[19] = var.X * 2f - 1f;
					}

					if (_listItem == _labbra)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexLabbra != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "Sottili":
										_data.skin.face.tratti[12] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
									case "Carnose":
										_data.skin.face.tratti[12] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[12] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition;
						_data.skin.face.tratti[12] = (var.X * 2f - 1f) / -1;
					}

					if (_listItem == _masce)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexMasce != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "Stretta":
										_data.skin.face.tratti[14] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "Larga":
										_data.skin.face.tratti[14] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[14] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
						_data.skin.face.tratti[13] = (var.X * 2f - 1f) / -1;
						_data.skin.face.tratti[14] = var.Y * 2f - 1f;
					}

					if (_listItem == _mentoPro)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexMentoPro != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "In dentro":
										_data.skin.face.tratti[15] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "In fuori":
										_data.skin.face.tratti[15] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[15] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
						_data.skin.face.tratti[16] = var.X * 2f - 1f;
						_data.skin.face.tratti[15] = var.Y * 2f - 1f;
					}

					if (_listItem == _mentoFor)
					{
						if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
							if (oldIndexMentoFor != _newIndex)
								switch (_listItem.Items[_newIndex])
								{
									case "Squadrato":
										_data.skin.face.tratti[17] = 0.00001f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "A punta":
										_data.skin.face.tratti[17] = 0.999999f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
									case "Standard":
										_data.skin.face.tratti[17] = 0.5f * 2f - 1f;
										(_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

										break;
								}

						PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
						_data.skin.face.tratti[18] = (var.X * 2f - 1f) / -1;
						_data.skin.face.tratti[17] = var.Y * 2f - 1f;
					}

					UpdateFace(PlayerPedId(), _data.skin);
				};

				#endregion

				#region VESTITI

				Apparel.OnIndexChange += async (sender, index) =>
				{
					if (_data.skin.sex == "Maschio")
					{
						Dressing dress = new Dressing(CompletiMaschio[index].Name, CompletiMaschio[index].Description, CompletiMaschio[index].ComponentDrawables, CompletiMaschio[index].ComponentTextures, CompletiMaschio[index].PropIndices, CompletiMaschio[index].PropTextures);
						_data.dressing = dress;
						_dataMaschio = _data;
					}
					else
					{
						Dressing dress = new Dressing(CompletiFemmina[index].Name, CompletiFemmina[index].Description, CompletiFemmina[index].ComponentDrawables, CompletiFemmina[index].ComponentTextures, CompletiFemmina[index].PropIndices, CompletiFemmina[index].PropTextures);
						_data.dressing = dress;
						_dataFemmina = _data;
					}

					UpdateDress(PlayerPedId(), _data.dressing);
					TaskProvaClothes(CachePlayer.Cache.MyPlayer.Ped, sub_7dd83(1, 0, _data.skin.sex));
				};

				#endregion

				#endregion

				#region ControlloAperturaChiusura

				HUD.MenuPool.OnMenuStateChanged += async (_oldMenu, _newMenu, state) =>
				{
					switch (state)
					{
						case MenuState.ChangeForward:
						{
							if (_newMenu == Info || _newMenu == Genitori || _newMenu == Dettagli || _newMenu == Apparenze) AnimateGameplayCamZoom(true, _ncam);

							if (_newMenu == Apparenze)
							{
								if (!IsHelpMessageBeingDisplayed()) HUD.ShowHelp("Usa il ~INPUTGROUP_LOOK~ per controllare i pannelli");
								Apparenze.Clear();

								if (_selezionato == "Maschio")
								{
									Capelli = new UIMenuListItem(GetLabelText("FACE_HAIR"), HairUomo, _data.skin.hair.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
									Apparenze.AddItem(Capelli);
									Apparenze.AddItem(sopracciglia);
									Apparenze.AddItem(Barba);
									Apparenze.AddItem(SkinBlemishes);
									Apparenze.AddItem(SkinAgeing);
									Apparenze.AddItem(SkinComplexion);
									Apparenze.AddItem(SkinMoles);
									Apparenze.AddItem(SkinDamage);
									Apparenze.AddItem(EyeColor);
									Apparenze.AddItem(EyeMakup);
									Apparenze.AddItem(LipStick);
									UIMenuColorPanel CapelCol1 = new UIMenuColorPanel("Colore Principale", ColorPanelType.Hair);
									UIMenuColorPanel CapelCol2 = new UIMenuColorPanel("Colore Secondario", ColorPanelType.Hair);
									Capelli.AddPanel(CapelCol1);
									Capelli.AddPanel(CapelCol2);
									CapelCol1.CurrentSelection = _data.skin.hair.color[0];
									CapelCol2.CurrentSelection = _data.skin.hair.color[1];
									soprCol1.CurrentSelection = _data.skin.facialHair.eyebrow.color[0];
									soprCol2.CurrentSelection = _data.skin.facialHair.eyebrow.color[1];
									soprOp.Percentage = _data.skin.facialHair.eyebrow.opacity;
									BarbaCol1.CurrentSelection = _data.skin.facialHair.beard.color[0];
									BarbaCol2.CurrentSelection = _data.skin.facialHair.beard.color[1];
									BarbaOp.Percentage = _data.skin.facialHair.beard.opacity;
									BlemOp.Percentage = _data.skin.blemishes.opacity;
									AgeOp.Percentage = _data.skin.ageing.opacity;
									CompOp.Percentage = _data.skin.complexion.opacity;
									FrecOp.Percentage = _data.skin.freckles.opacity;
									DamageOp.Percentage = _data.skin.skinDamage.opacity;
									MakupOp.Percentage = _data.skin.makeup.opacity;
									LipCol1.CurrentSelection = _data.skin.lipstick.color[0];
									LipCol2.CurrentSelection = _data.skin.lipstick.color[1];
									LipOp.Percentage = _data.skin.lipstick.opacity;
								}
								else
								{
									Capelli = new UIMenuListItem(GetLabelText("FACE_HAIR"), HairDonna, _data.skin.hair.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
									Apparenze.AddItem(Capelli);
									Apparenze.AddItem(sopracciglia);
									Apparenze.AddItem(SkinBlemishes);
									Apparenze.AddItem(SkinAgeing);
									Apparenze.AddItem(SkinComplexion);
									Apparenze.AddItem(SkinMoles);
									Apparenze.AddItem(SkinDamage);
									Apparenze.AddItem(EyeColor);
									Apparenze.AddItem(EyeMakup);
									Apparenze.AddItem(Blusher);
									Apparenze.AddItem(LipStick);
									UIMenuColorPanel CapelCol1 = new UIMenuColorPanel("Colore Principale", ColorPanelType.Hair);
									UIMenuColorPanel CapelCol2 = new UIMenuColorPanel("Colore Secondario", ColorPanelType.Hair);
									Capelli.AddPanel(CapelCol1);
									Capelli.AddPanel(CapelCol2);
									CapelCol1.CurrentSelection = _data.skin.hair.color[0];
									CapelCol2.CurrentSelection = _data.skin.hair.color[1];
									soprCol1.CurrentSelection = _data.skin.facialHair.eyebrow.color[0];
									soprCol2.CurrentSelection = _data.skin.facialHair.eyebrow.color[1];
									soprOp.Percentage = _data.skin.facialHair.eyebrow.opacity;
									BlemOp.Percentage = _data.skin.blemishes.opacity;
									AgeOp.Percentage = _data.skin.ageing.opacity;
									CompOp.Percentage = _data.skin.complexion.opacity;
									FrecOp.Percentage = _data.skin.freckles.opacity;
									DamageOp.Percentage = _data.skin.skinDamage.opacity;
									MakupOp.Percentage = _data.skin.makeup.opacity;
									BlushCol1.CurrentSelection = _data.skin.blusher.color[0];
									BlushCol2.CurrentSelection = _data.skin.blusher.color[1];
									BlushOp.Percentage = _data.skin.blusher.opacity;
									LipCol1.CurrentSelection = _data.skin.lipstick.color[0];
									LipCol2.CurrentSelection = _data.skin.lipstick.color[1];
									LipOp.Percentage = _data.skin.lipstick.opacity;
								}
							}
							else if (_newMenu == Apparel)
							{
								TaskCreaClothes(CachePlayer.Cache.MyPlayer.Ped, sub_7dd83(1, 0, _selezionato));

								if (_selezionato == "Maschio")
								{
									for (int i = 0; i < CompletiMaschio.Count; i++)
									{
										UIMenuItem abito = new UIMenuItem(CompletiMaschio[i].Name, CompletiMaschio[i].Description);
										Apparel.AddItem(abito);
									}

									UpdateDress(PlayerPedId(), CompletiMaschio[0]);
								}
								else
								{
									for (int i = 0; i < CompletiFemmina.Count; i++)
									{
										UIMenuItem abito = new UIMenuItem(CompletiFemmina[i].Name, CompletiFemmina[i].Description);
										Apparel.AddItem(abito);
									}

									UpdateDress(PlayerPedId(), CompletiFemmina[0]);
								}
							}

							break;
						}
						case MenuState.ChangeBackward when _oldMenu == Info || _oldMenu == Genitori || _oldMenu == Dettagli || _oldMenu == Apparenze:
							AnimateGameplayCamZoom(false, _ncam);

							break;
						case MenuState.ChangeBackward:
						{
							if (_oldMenu == Apparel)
							{
								Apparel.Clear();
								TaskClothesALoop(CachePlayer.Cache.MyPlayer.Ped, sub_7dd83(1, 0, _selezionato));
							}

							break;
						}
					}
				};

				#endregion

				#region CREA_BUTTON_FINISH

				UIMenuItem Salva = new UIMenuItem("Salva Personaggio", "Pronto per ~y~entrare in gioco~w~?", Color.FromArgb(100, 0, 139, 139), Color.FromArgb(255, 0, 255, 255));
				Salva.SetRightBadge(BadgeStyle.Tick);
				Creazione.AddItem(Salva);
				Salva.Activated += async (_selectedItem, _index) =>
				{
					Screen.Fading.FadeOut(800);
					await BaseScript.Delay(1000);
					if (_dummyPed != null)
						if (_dummyPed.Exists())
							_dummyPed.Delete();
					Creazione.Visible = false;
					pool.CloseAllMenus();
					BD1.Detach();
					BD1.Delete();
					CachePlayer.Cache.MyPlayer.Ped.Detach();
					BaseScript.TriggerServerEvent("lprp:finishCharServer", _data.SerializeToJson());
					CachePlayer.Cache.MyPlayer.User.char_current = _data.id;
					BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", CachePlayer.Cache.MyPlayer.User.char_current);
					ClientSession.Instance.RemoveTick(Controllo);
					ClientSession.Instance.RemoveTick(Scaleform);
					ClientSession.Instance.RemoveTick(TastiMenu);
					CamerasFirstTime.FirstTimeTransition(_data.id == 1);
					RemoveAnimDict("mp_character_creation@lineup@male_a");
					RemoveAnimDict("mp_character_creation@lineup@male_b");
					RemoveAnimDict("mp_character_creation@lineup@female_a");
					RemoveAnimDict("mp_character_creation@lineup@female_b");
					RemoveAnimDict("mp_character_creation@customise@male_a");
					RemoveAnimDict("mp_character_creation@customise@female_a");
				};

				#endregion

				Creazione.Visible = true;
				ClientSession.Instance.AddTick(TastiMenu);
			}
			catch
			{
				Log.Printa(LogType.Error, "MenuCreazione");
			}
		}

		#endregion

		#region Controllo tasti

		private static float fov = 10.00255f;
		private static float CoordX;
		private static bool left = false;
		private static bool right = false;
		private static float CoordY;

		public static async Task TastiMenu()
		{
			Ped playerPed = CachePlayer.Cache.MyPlayer.Ped;

			if (Creazione.Visible || Dettagli.Visible || Apparenze.Visible || Genitori.Visible)
			{
				if ((IsControlPressed(0, 205) || IsDisabledControlPressed(0, 205)) && IsInputDisabled(2) || (IsControlPressed(2, 205) || IsDisabledControlPressed(2, 205)) && !IsInputDisabled(2))
				{
					if (!left)
					{
						left = true;
						TaskLookLeft(playerPed, sub_7dd83(1, 0, _selezionato));
						//TaskLookAtCoord(playerPed.Handle, 401.48f, -997.13f, -98.5f, 1.0f, 0, 2);
					}
				}
				else if ((IsControlPressed(0, 206) || IsDisabledControlPressed(0, 206)) && IsInputDisabled(2) || (IsControlPressed(2, 206) || IsDisabledControlPressed(2, 206)) && !IsInputDisabled(2))
				{
					if (!right)
					{
						right = true;
						TaskLookRight(playerPed, sub_7dd83(1, 0, _selezionato));
						//TaskLookAtCoord(playerPed.Handle, 403.89f, -996.86f, -98.5f, 1.0f, 0, 2);
					}
				}
				else
				{
					if (right)
						TaskStopLookRight(playerPed, sub_7dd83(1, 0, _selezionato));
					else if (left) TaskStopLookLeft(playerPed, sub_7dd83(1, 0, _selezionato));
					right = false;
					left = false;
					//TaskClearLookAt(playerPed.Handle);
				}
			}
			else if ((Dettagli.Visible || Apparenze.Visible || Genitori.Visible) && !Creazione.Visible)
			{
				if (IsDisabledControlPressed(0, 207) && IsInputDisabled(2) || IsDisabledControlPressed(2, 207) && !IsInputDisabled(2) || IsControlPressed(0, 207) && IsInputDisabled(2) || IsControlPressed(2, 207) && !IsInputDisabled(2))
				{
					fov -= .3f;
					if (fov < 8.0f) fov = 8.0f;
					ncamm.FieldOfView = fov;
				}

				if (IsDisabledControlJustReleased(0, 207) || IsControlJustReleased(0, 207))
					do
					{
						await BaseScript.Delay(0);
						fov += .3f;
						if (fov > 10.00255f) fov = 10.00255f;
						ncamm.FieldOfView = fov;
					} while (fov < 10.00255f && !IsDisabledControlJustReleased(0, 207) && !IsControlJustReleased(0, 207));
			}

			if (!IsInputDisabled(2))
				if (Dettagli.Visible)
				{
					if (_arcSopr.Selected)
					{
						PointF var = (_arcSopr.Panels[0] as UIMenuGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) || IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) || IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[7] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[7] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
							{
								CoordY += 0.03f;
								if (CoordY > 1f) CoordY = 1f;
							}

							_data.skin.face.tratti[6] = CoordY * 2f - 1f;

							if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
							{
								CoordY -= 0.03f;
								if (CoordY < 0) CoordY = 0;
							}

							_data.skin.face.tratti[6] = CoordY * 2f - 1f;
							(_arcSopr.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
						}
					}

					if (_occhi.Selected)
					{
						PointF var = (_occhi.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[11] = (CoordX * 2f - 1f) / -1;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[11] = (CoordX * 2f - 1f) / -1;
							(_occhi.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(CoordX, .5f);
						}
					}

					if (_guance.Selected)
					{
						PointF var = (_guance.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[10] = (CoordX * 2f - 1f) / -1;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[10] = (CoordX * 2f - 1f) / -1;
							(_guance.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(CoordX, .5f);
						}
					}

					if (_labbra.Selected)
					{
						PointF var = (_labbra.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[12] = (CoordX * 2f - 1f) / -1;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[12] = (CoordX * 2f - 1f) / -1;
							(_labbra.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(CoordX, .5f);
						}
					}

					if (_collo.Selected)
					{
						PointF var = (_collo.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[19] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[19] = CoordX * 2f - 1f;
							(_collo.Panels[0] as UIMenuHorizontalOneLineGridPanel).CirclePosition = new PointF(CoordX, .5f);
						}
					}

					if (_naso.Selected)
					{
						PointF var = (_naso.Panels[0] as UIMenuGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) || IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) || IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[0] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[0] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
							{
								CoordY += 0.03f;
								if (CoordY > 1f) CoordY = 1f;
							}

							_data.skin.face.tratti[1] = CoordY * 2f - 1f;

							if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
							{
								CoordY -= 0.03f;
								if (CoordY < 0) CoordY = 0;
							}

							_data.skin.face.tratti[1] = CoordY * 2f - 1f;
							(_naso.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
						}
					}

					if (_nasoPro.Selected)
					{
						PointF var = (_nasoPro.Panels[0] as UIMenuGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) || IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) || IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[2] = (CoordX * 2f - 1f) / -1;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[2] = (CoordX * 2f - 1f) / -1;

							if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
							{
								CoordY += 0.03f;
								if (CoordY > 1f) CoordY = 1f;
							}

							_data.skin.face.tratti[3] = CoordY * 2f - 1f;

							if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
							{
								CoordY -= 0.03f;
								if (CoordY < 0) CoordY = 0;
							}

							_data.skin.face.tratti[3] = CoordY * 2f - 1f;
							(_nasoPro.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
						}
					}

					if (_nasoPun.Selected)
					{
						PointF var = (_nasoPun.Panels[0] as UIMenuGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) || IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) || IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[5] = (CoordX * 2f - 1f) / -1;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[5] = (CoordX * 2f - 1f) / -1;

							if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
							{
								CoordY += 0.03f;
								if (CoordY > 1f) CoordY = 1f;
							}

							_data.skin.face.tratti[4] = CoordY * 2f - 1f;

							if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
							{
								CoordY -= 0.03f;
								if (CoordY < 0) CoordY = 0;
							}

							_data.skin.face.tratti[4] = CoordY * 2f - 1f;
							(_nasoPun.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
						}
					}

					if (_zigo.Selected)
					{
						PointF var = (_zigo.Panels[0] as UIMenuGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) || IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) || IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[9] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[9] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
							{
								CoordY += 0.03f;
								if (CoordY > 1f) CoordY = 1f;
							}

							_data.skin.face.tratti[8] = CoordY * 2f - 1f;

							if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
							{
								CoordY -= 0.03f;
								if (CoordY < 0) CoordY = 0;
							}

							_data.skin.face.tratti[8] = CoordY * 2f - 1f;
							(_zigo.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
						}
					}

					if (_masce.Selected)
					{
						PointF var = (_masce.Panels[0] as UIMenuGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) || IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) || IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[13] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[13] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
							{
								CoordY += 0.03f;
								if (CoordY > 1f) CoordY = 1f;
							}

							_data.skin.face.tratti[14] = CoordY * 2f - 1f;

							if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
							{
								CoordY -= 0.03f;
								if (CoordY < 0) CoordY = 0;
							}

							_data.skin.face.tratti[14] = CoordY * 2f - 1f;
							(_masce.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
						}
					}

					if (_mentoPro.Selected)
					{
						PointF var = (_mentoPro.Panels[0] as UIMenuGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) || IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) || IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[16] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[16] = CoordX * 2f - 1f;

							if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
							{
								CoordY += 0.03f;
								if (CoordY > 1f) CoordY = 1f;
							}

							_data.skin.face.tratti[15] = CoordY * 2f - 1f;

							if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
							{
								CoordY -= 0.03f;
								if (CoordY < 0) CoordY = 0;
							}

							_data.skin.face.tratti[15] = CoordY * 2f - 1f;
							(_mentoPro.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
						}
					}

					if (_mentoFor.Selected)
					{
						PointF var = (_mentoFor.Panels[0] as UIMenuGridPanel).CirclePosition;
						CoordX = var.X;
						CoordY = var.Y;

						if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) || IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) || IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3))
						{
							if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
							{
								CoordX += 0.03f;
								if (CoordX > 1f) CoordX = 1f;
							}

							_data.skin.face.tratti[18] = (CoordX * 2f - 1f) / -1;

							if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
							{
								CoordX -= 0.03f;
								if (CoordX < 0) CoordX = 0;
							}

							_data.skin.face.tratti[18] = (CoordX * 2f - 1f) / -1;

							if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
							{
								CoordY += 0.03f;
								if (CoordY > 1f) CoordY = 1f;
							}

							_data.skin.face.tratti[17] = CoordY * 2f - 1f;

							if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
							{
								CoordY -= 0.03f;
								if (CoordY < 0) CoordY = 0;
							}

							_data.skin.face.tratti[17] = CoordY * 2f - 1f;
							(_mentoFor.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
						}
					}

					if (_data.skin.sex == "Maschio")
						_dataFemmina = _data;
					else
						_dataMaschio = _data;
					UpdateFace(playerPed.Handle, _data.skin);
				}

			await Task.FromResult(0);
		}

		#endregion

		#endregion

		private static Camera ncamm = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));

		public static async void AnimateGameplayCamZoom(bool toggle, Camera ncam)
		{
			if (toggle)
			{
				ncamm = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));
				ncamm.Position = new Vector3(402.6746f, -1000.129f, -98.46554f);
				ncamm.Rotation = new Vector3(0.861356f, 0f, -2.348183f);
				CachePlayer.Cache.MyPlayer.Ped.IsVisible = true;
				ncamm.FieldOfView = 10.00255f;
				ncamm.IsActive = true;
				N_0xf55e4046f6f831dc(ncamm.Handle, 4f);
				N_0xe111a7c0d200cbc5(ncamm.Handle, 1f);
				SetCamDofFnumberOfLens(ncamm.Handle, 1.2f);
				SetCamDofMaxNearInFocusDistanceBlendLevel(ncamm.Handle, 1f);
				ncam.InterpTo(ncamm, 300, 1, 1);
				Game.PlaySound("Zoom_In", "MUGSHOT_CHARACTER_CREATION_SOUNDS");
				while (ncam.IsInterpolating) await BaseScript.Delay(0);
			}
			else
			{
				ncamm.InterpTo(ncam, 300, 1, 1);
				ncamm.Delete();
				Game.PlaySound("Zoom_Out", "MUGSHOT_CHARACTER_CREATION_SOUNDS");
				while (ncam.IsInterpolating) await BaseScript.Delay(0);
			}

			await Task.FromResult(0);
		}

		private static Camera ncamm2 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));

		public static async void ZoomCam(bool toggle)
		{
			if (toggle)
			{
				if (!ncamm.Exists()) return;
				ncamm2 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));
				ncamm2 = ncamm;
				ncamm2.FieldOfView = 9f;
				ncamm.InterpTo(ncamm2, 1000, 0, 0);
			}
			else
			{
				ncamm2.InterpTo(ncamm, 1000, 0, 0);
				ncamm2.Delete();
			}
		}

		public static async void ped_cre_board(Char_data data)
		{
			CachePlayer.Cache.MyPlayer.Ped.BlockPermanentEvents = true;
			sub_7cddb();
			Pol_Board2(data);
		}

		private static Prop BD1;
		private static Prop Overlay1;

		public static async void Pol_Board2(Char_data data)
		{
			Model bd1 = new Model("prop_police_id_board");
			bd1.Request();
			Model overlay1 = new Model("prop_police_id_text");
			overlay1.Request();
			while (!bd1.IsLoaded) await BaseScript.Delay(0);
			while (!overlay1.IsLoaded) await BaseScript.Delay(0);
			BD1 = new Prop(CreateObject(bd1.Hash, 402.91f, -996.74f, -180.00025f, false, true, true));
			Overlay1 = new Prop(CreateObject(overlay1.Hash, 402.91f, -996.74f, -180.00025f, false, true, false));
			while (!BD1.Exists()) await BaseScript.Delay(0);
			while (!Overlay1.Exists()) await BaseScript.Delay(0);
			Overlay1.AttachTo(BD1);
			BD1.AttachTo(CachePlayer.Cache.MyPlayer.Ped.Bones[Bone.PH_R_Hand], Vector3.Zero, Vector3.Zero);
			CreaScaleform_Cre(data, overlay1);
			Overlay1.MarkAsNoLongerNeeded();
			bd1.MarkAsNoLongerNeeded();
			overlay1.MarkAsNoLongerNeeded();
		}

		private static async void CreaScaleform_Cre(Char_data data, Model overlay)
		{
			_boardScalep1 = new Scaleform("mugshot_board_01");
			while (!_boardScalep1.IsLoaded) await BaseScript.Delay(0);
			_boardScalep1.CallFunction("SET_BOARD", "Nuova GM", data.info.firstname + " " + data.info.lastname, "Personaggio N°", "Powered by Manups4e", 0, CachePlayer.Cache.MyPlayer.User.Characters.Count + 1, 0);
			_handle1 = CreateNamedRenderTargetForModel("ID_Text", (uint)overlay.Hash);
		}

		public static async Task Scaleform()
		{
			if (CachePlayer.Cache.MyPlayer.Ped.Exists())
			{
				SetTextRenderId(_handle1);
				Function.Call((Hash)0x40332D115A898AF5, _boardScalep1.Handle, true);
				SetScriptGfxDrawOrder(4);
				SetScriptGfxDrawBehindPausemenu(true);
				DrawScaleformMovie(_boardScalep1.Handle, 0.4f, 0.35f, 0.8f, 0.75f, 255, 255, 255, 255, 255);
				SetTextRenderId(GetDefaultScriptRendertargetRenderId());
				Function.Call((Hash)0x40332D115A898AF5, _boardScalep1.Handle, false);
				SetScriptGfxDrawBehindPausemenu(false);
			}

			await Task.FromResult(0);
		}

		public static async Task Controllo()
		{
			for (int i = 0; i < 32; i++) Game.DisableAllControlsThisFrame(i);
			if (p1.Exists()) p1.Heading += 1f;

			if (Creazione.Visible && Creazione.HasControlJustBeenPressed(UIMenu.MenuControls.Back))
			{
				HUD.MenuPool.CloseAllMenus();
				BaseScript.TriggerEvent("lprp:manager:warningMessage", "Vuoi annullare la creazione del personaggio?", "Tornerai alla selezione del personaggio e la creazione verrà annullata", 16392, "lprp:sceltaCharCreation");
			}

			await Task.FromResult(0);
		}

		public static async void SceltaCreatoreAsync(string param)
		{
			switch (param)
			{
				case "select":
				{
					Screen.Fading.FadeOut(0);
					await BaseScript.Delay(100);
					if (_dummyPed != null)
						if (_dummyPed.Exists())
							_dummyPed.Delete();
					LogIn.LogIn.CharSelect();

					break;
				}
				case "back":
					Screen.Fading.FadeOut(0);
					await BaseScript.Delay(100);
					MenuCreazione(_a, _b, _c, _d);

					break;
			}
		}

		private static int CreateNamedRenderTargetForModel(string name, uint model)
		{
			int handle = 0;
			if (!IsNamedRendertargetRegistered(name)) RegisterNamedRendertarget(name, false);
			if (!IsNamedRendertargetLinked(model)) LinkNamedRendertarget(model);
			if (IsNamedRendertargetRegistered(name)) handle = GetNamedRendertargetRenderId(name);

			return handle;
		}

		private static void sub_7cddb()
		{
			string v_3 = sub_7ce29(Funzioni.GetRandomInt(0, 7));
			if (AreStringsEqual(v_3, "mood_smug_1")) v_3 = "mood_Happy_1";
			if (AreStringsEqual(v_3, "mood_sulk_1")) v_3 = "mood_Angry_1";
			if (!CachePlayer.Cache.MyPlayer.Ped.IsInjured) SetFacialIdleAnimOverride(CachePlayer.Cache.MyPlayer.Ped.Handle, v_3, "0");
		}

		public static string sub_7ce29(int a_0)
		{
			switch (a_0)
			{
				case 0:
					return "mood_Aiming_1";
				case 1:
					return "mood_Angry_1";
				case 2:
					return "mood_Happy_1";
				case 3:
					return "mood_Injured_1";
				case 4:
					return "mood_Normal_1";
				case 5:
					return "mood_stressed_1";
				case 6:
					return "mood_smug_1";
				case 7:
					return "mood_sulk_1";
			}

			return "mood_Normal_1";
		}

		private static string sub_7dd83(int LineOrCustom, int AltAn, string Sex)
		{
			switch (LineOrCustom)
			{
				case 0 when AltAn == 0:
					return Sex == "Maschio" ? "mp_character_creation@lineup@male_b" : "mp_character_creation@lineup@female_b";
				case 0:
					return Sex == "Maschio" ? "mp_character_creation@lineup@male_a" : "mp_character_creation@lineup@female_a";
				case 1:
					return Sex == "Maschio" ? "mp_character_creation@customise@male_a" : "mp_character_creation@customise@female_a";
			}

			return "mp_character_creation@lineup@male_a";
		}

		public static async Task UpdateFace(int Handle, Skin skin)
		{
			SetPedHeadBlendData(Handle, skin.face.mom, skin.face.dad, 0, skin.face.mom, skin.face.dad, 0, skin.resemblance, skin.skinmix, 0f, false);
			SetPedHeadOverlay(Handle, 0, skin.blemishes.style, skin.blemishes.opacity);
			SetPedHeadOverlay(Handle, 1, skin.facialHair.beard.style, skin.facialHair.beard.opacity);
			SetPedHeadOverlayColor(Handle, 1, 1, skin.facialHair.beard.color[0], skin.facialHair.beard.color[1]);
			SetPedHeadOverlay(Handle, 2, skin.facialHair.eyebrow.style, skin.facialHair.eyebrow.opacity);
			SetPedHeadOverlayColor(Handle, 2, 1, skin.facialHair.eyebrow.color[0], skin.facialHair.eyebrow.color[1]);
			SetPedHeadOverlay(Handle, 3, skin.ageing.style, skin.ageing.opacity);
			SetPedHeadOverlay(Handle, 4, skin.makeup.style, skin.makeup.opacity);
			SetPedHeadOverlay(Handle, 5, skin.blusher.style, skin.blusher.opacity);
			SetPedHeadOverlayColor(Handle, 5, 2, skin.blusher.color[0], skin.blusher.color[1]);
			SetPedHeadOverlay(Handle, 6, skin.complexion.style, skin.complexion.opacity);
			SetPedHeadOverlay(Handle, 7, skin.skinDamage.style, skin.skinDamage.opacity);
			SetPedHeadOverlay(Handle, 8, skin.lipstick.style, skin.lipstick.opacity);
			SetPedHeadOverlayColor(Handle, 8, 2, skin.lipstick.color[0], skin.lipstick.color[1]);
			SetPedHeadOverlay(Handle, 9, skin.freckles.style, skin.freckles.opacity);
			SetPedEyeColor(Handle, skin.eye.style);
			SetPedComponentVariation(Handle, 2, skin.hair.style, 0, 0);
			SetPedHairColor(Handle, skin.hair.color[0], skin.hair.color[1]);
			SetPedPropIndex(Handle, 2, skin.ears.style, skin.ears.color, false);
			for (int i = 0; i < skin.face.tratti.Length; i++) SetPedFaceFeature(Handle, i, skin.face.tratti[i]);
			await Task.FromResult(0);
		}

		public static async Task UpdateDress(int Handle, Dressing dress)
		{
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Faccia, dress.ComponentDrawables.Faccia, dress.ComponentTextures.Faccia, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Maschera, dress.ComponentDrawables.Maschera, dress.ComponentTextures.Maschera, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Torso, dress.ComponentDrawables.Torso, dress.ComponentTextures.Torso, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Pantaloni, dress.ComponentDrawables.Pantaloni, dress.ComponentTextures.Pantaloni, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Borsa_Paracadute, dress.ComponentDrawables.Borsa_Paracadute, dress.ComponentTextures.Borsa_Paracadute, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Scarpe, dress.ComponentDrawables.Scarpe, dress.ComponentTextures.Scarpe, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Accessori, dress.ComponentDrawables.Accessori, dress.ComponentTextures.Accessori, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Sottomaglia, dress.ComponentDrawables.Sottomaglia, dress.ComponentTextures.Sottomaglia, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Kevlar, dress.ComponentDrawables.Kevlar, dress.ComponentTextures.Kevlar, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Badge, dress.ComponentDrawables.Badge, dress.ComponentTextures.Badge, 2);
			SetPedComponentVariation(Handle, (int)DrawableIndexes.Torso_2, dress.ComponentDrawables.Torso_2, dress.ComponentTextures.Torso_2, 2);
			if (dress.PropIndices.Cappelli_Maschere == -1)
				ClearPedProp(Handle, 0);
			else
				SetPedPropIndex(Handle, (int)PropIndexes.Cappelli_Maschere, dress.PropIndices.Cappelli_Maschere, dress.PropTextures.Cappelli_Maschere, false);
			if (dress.PropIndices.Orecchie == -1)
				ClearPedProp(Handle, 2);
			else
				SetPedPropIndex(Handle, (int)PropIndexes.Orecchie, dress.PropIndices.Orecchie, dress.PropTextures.Orecchie, false);
			if (dress.PropIndices.Occhiali_Occhi == -1)
				ClearPedProp(Handle, 1);
			else
				SetPedPropIndex(Handle, (int)PropIndexes.Occhiali_Occhi, dress.PropIndices.Occhiali_Occhi, dress.PropTextures.Occhiali_Occhi, true);
			if (dress.PropIndices.Unk_3 == -1)
				ClearPedProp(Handle, 3);
			else
				SetPedPropIndex(Handle, (int)PropIndexes.Unk_3, dress.PropIndices.Unk_3, dress.PropTextures.Unk_3, true);
			if (dress.PropIndices.Unk_4 == -1)
				ClearPedProp(Handle, 4);
			else
				SetPedPropIndex(Handle, (int)PropIndexes.Unk_4, dress.PropIndices.Unk_4, dress.PropTextures.Unk_4, true);
			if (dress.PropIndices.Unk_5 == -1)
				ClearPedProp(Handle, 5);
			else
				SetPedPropIndex(Handle, (int)PropIndexes.Unk_5, dress.PropIndices.Unk_5, dress.PropTextures.Unk_5, true);
			if (dress.PropIndices.Orologi == -1)
				ClearPedProp(Handle, 6);
			else
				SetPedPropIndex(Handle, (int)PropIndexes.Orologi, dress.PropIndices.Orologi, dress.PropTextures.Orologi, true);
			if (dress.PropIndices.Bracciali == -1)
				ClearPedProp(Handle, 7);
			else
				SetPedPropIndex(Handle, (int)PropIndexes.Bracciali, dress.PropIndices.Bracciali, dress.PropTextures.Bracciali, true);
			if (dress.PropIndices.Unk_8 == -1)
				ClearPedProp(Handle, 8);
			else
				SetPedPropIndex(Handle, (int)PropIndexes.Unk_8, dress.PropIndices.Unk_8, dress.PropTextures.Unk_8, true);
			await Task.FromResult(0);
		}

		private static void sub_8d2b2()
		{
			RequestAnimDict("mp_character_creation@lineup@male_a");
			RequestAnimDict("mp_character_creation@lineup@male_b");
			RequestAnimDict("mp_character_creation@lineup@female_a");
			RequestAnimDict("mp_character_creation@lineup@female_b");
			RequestAnimDict("mp_character_creation@customise@male_a");
			RequestAnimDict("mp_character_creation@customise@female_a");
			RequestModel((uint)GetHashKey("prop_police_id_board"));
			RequestModel((uint)GetHashKey("prop_police_id_text"));
			RequestModel((uint)GetHashKey("prop_police_id_text_02"));

			if (N_0x544810ed9db6bbe6() != true) return;
			RequestScriptAudioBank("Mugshot_Character_Creator", false);
			RequestScriptAudioBank("DLC_GTAO/MUGSHOT_ROOM", false);
		}

		private static async void TaskLookLeft(Ped p, string an)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(0, an, "Profile_L_Intro", 4.0f, -4.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(0, an, "Profile_L_Loop", 4.0f, -4.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
		}

		private static async void TaskLookRight(Ped p, string an)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(0, an, "Profile_R_Intro", 4.0f, -4.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(0, an, "Profile_R_Loop", 4.0f, -4.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
		}

		private static async void TaskStopLookLeft(Ped p, string an)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(0, an, "Profile_L_Outro", 4.0f, -4.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(0, an, "Loop", 4.0f, -4.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
		}

		private static async void TaskStopLookRight(Ped p, string an)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(0, an, "Profile_R_Outro", 4.0f, -4.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(0, an, "Loop", 4.0f, -4.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
		}

		private static void TaskCreaClothes(Ped p, string an)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(0, an, "DROP_INTRO", 8.0f, -8.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(0, an, "DROP_LOOP", 4.0f, -4.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
		}

		private static void TaskProvaClothes(Ped p, string an)
		{
			string anim = "";
			int a = GetRandomIntInRange(0, 2);

			switch (a)
			{
				case 0:
					anim = "DROP_CLOTHES_A";

					break;
				case 1:
					anim = "DROP_CLOTHES_B";

					break;
				case 2:
					anim = "DROP_CLOTHES_C";

					break;
			}

			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(0, an, anim, 8.0f, -8.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(0, an, "DROP_LOOP", 8.0f, -8.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
		}

		private static void TaskClothesALoop(Ped p, string an)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(0, an, "DROP_OUTRO", 8.0f, -8.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(0, an, "Loop", 8.0f, -8.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
		}

		private static async Task TaskPlayOutro(Ped p, string an)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(p.Handle, an, "outro", 8.0f, -8.0f, -1, 512, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
			await BaseScript.Delay(5000);
		}

		private static async Task TaskHoldBoard()
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(PlayerPedId(), sub_7dd83(1, 0, _selezionato), "react_light", 8.0f, -8.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(PlayerPedId(), sub_7dd83(1, 0, _selezionato), "Loop", 8.0f, -8.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(PlayerPedId(), sequence);
			ClearSequenceTask(ref sequence);
		}

		private static async Task TaskRaiseBoard(Ped p, string an)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(0, an, "low_to_high", 4.0f, -4.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(0, an, "Loop_raised", 8.0f, -8.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
		}

		private static async Task TaskLowBoard(Ped p, string an)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(0, an, "high_to_low", 4.0f, -4.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(0, an, "Loop", 8.0f, -8.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
		}

		public static async Task TaskWalkInToRoom(Ped p, string an)
		{
			int sequence = 0;
			Vector3 pos = new Vector3(404.834f, -997.838f, -97.841f);
			OpenSequenceTask(ref sequence);
			TaskPlayAnimAdvanced(0, an, "Intro", pos.X, pos.Y, pos.Z - 1f, 0.0f, 0.0f, -40.0f, 8.0f, -8.0f, -1, 4608, 0f, 2, 0);
			TaskPlayAnim(0, an, "Loop", 8f, -4f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
			await Task.FromResult(0);
		}
	}

	public class ParentFaces
	{
		public string Name;
		public int Value;

		public ParentFaces(string name, int value)
		{
			Name = name;
			Value = value;
		}
	}
}