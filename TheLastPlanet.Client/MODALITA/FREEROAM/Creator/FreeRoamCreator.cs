﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Impostazioni.Client.Configurazione.Negozi.Abiti;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using ScaleformUI;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Snowflakes;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Creator
{
    internal static class FreeRoamCreator
    {
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

        private static FreeRoamChar _dataMaschio;
        private static FreeRoamChar _dataFemmina;

        private static string _selezionato = "";
        private static readonly Camera cam2 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
        private static Scaleform _boardScalep1 = new("mugshot_board_01");
        private static int _handle1;
        private static string _d;
        private static FreeRoamChar _data;
        private static Camera _ncam;
        private static Ped _dummyPed;

        public static void Init()
        {
            Client.Instance.AddTick(Scaleform);
            sub_8d2b2();
        }

        public static void Stop()
        {
            Client.Instance.RemoveTick(Scaleform);
            RemoveAnimDict("mp_character_creation@lineup@male_a");
            RemoveAnimDict("mp_character_creation@lineup@male_b");
            RemoveAnimDict("mp_character_creation@lineup@female_a");
            RemoveAnimDict("mp_character_creation@lineup@female_b");
            RemoveAnimDict("mp_character_creation@customise@male_a");
            RemoveAnimDict("mp_character_creation@customise@female_a");
            SetModelAsNoLongerNeeded((uint)GetHashKey("prop_police_id_board"));
            SetModelAsNoLongerNeeded((uint)GetHashKey("prop_police_id_text"));
            SetModelAsNoLongerNeeded((uint)GetHashKey("prop_police_id_text_02"));
            ReleaseNamedScriptAudioBank("Mugshot_Character_Creator");
            ReleaseNamedScriptAudioBank("DLC_GTAO/MUGSHOT_ROOM");
        }

        private static async void AggiornaModel(string JsonData)
        {
            FreeRoamChar plpl = JsonData.FromJson<FreeRoamChar>();
            uint hash = (uint)GetHashKey(plpl.Skin.model);
            RequestModel(hash);
            while (!HasModelLoaded(hash)) await BaseScript.Delay(1);
            SetPlayerModel(PlayerId(), hash);
            UpdateFace(Cache.PlayerCache.MyPlayer.Ped.Handle, plpl.Skin);
            UpdateDress(Cache.PlayerCache.MyPlayer.Ped.Handle, plpl.Dressing);
        }

        #region Creazione

        #region PRE CREAZIONE

        public static async Task CharCreationMenu(string sesso)
        {
            try
            {
                _dummyPed = await Funzioni.CreatePedLocally(PedHash.FreemodeFemale01, Cache.PlayerCache.MyPlayer.Ped.Position + new Vector3(10));
                _dummyPed.IsVisible = false;
                _dummyPed.IsPositionFrozen = false;
                _dummyPed.IsCollisionEnabled = true;
                _dummyPed.IsCollisionProof = false;
                _dummyPed.BlockPermanentEvents = true;
                _selezionato = sesso;
                Client.Instance.AddTick(Controllo);
                long assicurazione = Funzioni.GetRandomLong(999999999999999);
                //Vector3 spawna = new Vector3(Main.charCreateCoords.X, Main.charCreateCoords.Y, Main.charCreateCoords.Z);
                if (IsValidInterior(94722)) LoadInterior(94722);
                while (!IsInteriorReady(94722)) await BaseScript.Delay(1000);
                sub_8d2b2();
                Cache.PlayerCache.MyPlayer.User.Status.Istanza.Istanzia("CreazionePersonaggio");
                _dataMaschio = new FreeRoamChar(SnowflakeGenerator.Instance.Next().ToInt64(),new Finance(1000,3000,0),new Gang("Incensurato", 0),new Skin("Maschio", "mp_m_freemode_01", 0, GetRandomFloatInRange(.5f,1f),new Face(0,0,new float[20] { Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1) }),new A2(GetRandomIntInRange(0, Ageing.Count),GetRandomFloatInRange(0f,1f)),new A2(255,0f),new A2(GetRandomIntInRange(0,blemishes.Count),GetRandomFloatInRange(0f,1f)),new A2(GetRandomIntInRange(0,Complexions.Count),GetRandomFloatInRange(0f,1f)),new A2(GetRandomIntInRange(0,Danni_Pelle.Count),GetRandomFloatInRange(0f,1f)),new A2(GetRandomIntInRange(0,Nei_e_Porri.Count),GetRandomFloatInRange(0f,1f)),new A3(255,0f,new int[2] { 0,0 }),new A3(255,0f,new int[2] { 0,0 }),new Facial(new A3(GetRandomIntInRange(0,Beards.Count),GetRandomFloatInRange(0f,1f),new int[2] { GetRandomIntInRange(0,63),GetRandomIntInRange(0,63) }),new A3(GetRandomIntInRange(0,eyebrow.Count),GetRandomFloatInRange(0f,1f),new int[2] { GetRandomIntInRange(0,63),GetRandomIntInRange(0,63) })),new Hair(GetRandomIntInRange(0,HairUomo.Count),new int[2] { GetRandomIntInRange(0,63),GetRandomIntInRange(0,63) }),new Eye(GetRandomIntInRange(0,Colore_Occhi.Count)),new Ears(255,0)),new Dressing("Iniziale","Per cominciare",new ComponentDrawables(-1,0,GetPedDrawableVariation(Cache.PlayerCache.MyPlayer.Ped.Handle,2),0,0,-1,15,0,15,0,0,56),new ComponentDrawables(-1,0,GetPedTextureVariation(Cache.PlayerCache.MyPlayer.Ped.Handle,2),0,4,-1,14,0,0,0,0,0),new PropIndices(-1,GetPedPropIndex(Cache.PlayerCache.MyPlayer.Ped.Handle,2),-1,-1,-1,-1,-1,-1,-1),new PropIndices(-1,GetPedPropTextureIndex(Cache.PlayerCache.MyPlayer.Ped.Handle,2),-1,-1,-1,-1,-1,-1,-1)),new List<Weapons>(),new FreeRoamStats());
                _dataFemmina = new FreeRoamChar(SnowflakeGenerator.Instance.Next().ToInt64(), new Finance(1000, 3000, 0), new Gang("Incensurato", 0), new Skin("Femmina", "mp_f_freemode_01", 0, GetRandomFloatInRange(.5f, 1f), new Face(21, 0, new float[20] { Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1) }), new A2(GetRandomIntInRange(0, Ageing.Count), GetRandomFloatInRange(0f, 1f)), new A2(255, 0f), new A2(GetRandomIntInRange(0, blemishes.Count), GetRandomFloatInRange(0f, 1f)), new A2(GetRandomIntInRange(0, Complexions.Count), GetRandomFloatInRange(0f, 1f)), new A2(GetRandomIntInRange(0, Danni_Pelle.Count), GetRandomFloatInRange(0f, 1f)), new A2(GetRandomIntInRange(0, Nei_e_Porri.Count), GetRandomFloatInRange(0f, 1f)), new A3(255, 0f, new int[2] { 0, 0 }), new A3(255, 0f, new int[2] { 0, 0 }), new Facial(new A3(255, 0f, new int[2] { 0, 0 }), new A3(GetRandomIntInRange(0, eyebrow.Count), GetRandomFloatInRange(0f, 1f), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) })), new Hair(GetRandomIntInRange(0, HairUomo.Count), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) }), new Eye(GetRandomIntInRange(0, Colore_Occhi.Count)), new Ears(255, 0)), new Dressing("Iniziale", "Per cominciare", new ComponentDrawables(-1, 0, GetPedDrawableVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), 0, 0, -1, 15, 0, 15, 0, 0, 56), new ComponentDrawables(-1, 0, GetPedTextureVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), 0, 4, -1, 14, 0, 0, 0, 0, 0), new PropIndices(-1, GetPedPropIndex(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, GetPedPropTextureIndex(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), -1, -1, -1, -1, -1, -1, -1)), new List<Weapons>(), new FreeRoamStats());
                _data = _selezionato.ToLower() == "maschio" ? _dataMaschio : _dataFemmina;
                AggiornaModel(_data.ToJson());
                await BaseScript.Delay(1000);
                Cache.PlayerCache.MyPlayer.Ped.Position = new Vector3(402.91f, -996.74f, -180.00025f);
                while (!HasCollisionLoadedAroundEntity(Cache.PlayerCache.MyPlayer.Ped.Handle)) await BaseScript.Delay(1);
                Cache.PlayerCache.MyPlayer.Ped.IsVisible = true;
                Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
                Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
                ped_cre_board(_data);
                await TaskWalkInToRoom(Cache.PlayerCache.MyPlayer.Ped, _selezionato == "Maschio" ? sub_7dd83(1, 0, "Maschio") : sub_7dd83(1, 0, "Femmina"));
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
                if (!Creazione.Visible) MenuCreazione(_selezionato);
                await Task.FromResult(0);
            }
            catch (Exception ex)
            {
                Client.Logger.Error("CharCreationMenu = " + ex);
            }
        }

        public static UIMenu Creazione = new UIMenu("", "");
        public static UIMenu Genitori = new UIMenu("", "");
        public static UIMenu Dettagli = new UIMenu("", "");
        public static UIMenu Apparenze = new UIMenu("", "");
        public static UIMenu Apparel = new UIMenu("", "");
        public static UIMenu Statistiche = new UIMenu("", "");
        private static List<dynamic> _arcSop = new List<dynamic> { "Standard", "Alte", "Basse" };
        private static List<dynamic> _occ = new List<dynamic> { "Standard", "Grandi", "Stretti" };
        private static List<dynamic> _nas = new List<dynamic> { "Standard", "Grande", "Piccolo" };
        private static UIMenuListItem _arcSopr = null;
        private static UIMenuListItem _occhi = null;
        private static UIMenuListItem _naso = null;
        private static UIMenuListItem _nasoPro = null;
        private static UIMenuListItem _nasoPun = null;
        private static UIMenuListItem _zigo = null;
        private static UIMenuListItem _guance = null;
        private static UIMenuListItem _labbra = null;
        private static UIMenuListItem _masce = null;
        private static UIMenuListItem _mentoPro = null;
        private static UIMenuListItem _mentoFor = null;
        private static UIMenuListItem _collo = null;

        #endregion

        #region CREZIONEVERA

        public static void MenuCreazione(string sesso)
        {
            try
            {
                #region Dichiarazione

                #region Menu Principale

                Screen.Fading.FadeIn(800);
                Point offset = new Point(50, 50);
                HUD.MenuPool.MouseEdgeEnabled = false;
                Creazione = new UIMenu("TLP Creator", "Crea un nuovo Personaggio", offset);
                Creazione.ControlDisablingEnabled = true;
                HUD.MenuPool.Add(Creazione);
                UIMenuListItem Sesso;
                Sesso = _selezionato == "Maschio" ? new UIMenuListItem("Sesso", new List<dynamic>() { "Maschio", "Femmina" }, 0, "Decidi il Sesso") : new UIMenuListItem("Sesso", new List<dynamic>() { "Maschio", "Femmina" }, 1, "Decidi il Sesso");
                Creazione.AddItem(Sesso);
                Genitori = HUD.MenuPool.AddSubMenu(Creazione, GetLabelText("FACE_HERI"), "~INPUT_CONTEXT~ " + GetLabelText("FACE_MM_H3"));
                Genitori.ControlDisablingEnabled = true;
                Dettagli = HUD.MenuPool.AddSubMenu(Creazione, GetLabelText("FACE_FEAT"), GetLabelText("FACE_MM_H4"));
                Dettagli.ControlDisablingEnabled = true;
                Apparenze = HUD.MenuPool.AddSubMenu(Creazione, GetLabelText("FACE_APP"), GetLabelText("FACE_MM_H6"));
                Apparenze.ControlDisablingEnabled = true;
                Apparel = HUD.MenuPool.AddSubMenu(Creazione, GetLabelText("FACE_APPA"), GetLabelText("FACE_APPA_H"));
                Apparel.ControlDisablingEnabled = true;
                Statistiche = HUD.MenuPool.AddSubMenu(Creazione, GetLabelText("FACE_STATS"), GetLabelText("FACE_MM_H5"));
                InstructionalButton button1 = new InstructionalButton(Control.LookLeftRight, "Guarda a Destra/Sinistra");
                InstructionalButton button2 = new InstructionalButton(Control.FrontendLb, "Guarda a Sinistra");
                InstructionalButton button3 = new InstructionalButton(Control.FrontendRb, "Guarda a Destra");
                InstructionalButton button4 = new InstructionalButton(InputGroup.INPUTGROUP_LOOK, "Cambia dettagli");
                InstructionalButton button5 = new InstructionalButton(InputGroup.INPUTGROUP_LOOK, "Gestisci Panelli", ScaleformUI.PadCheck.Keyboard);
                Creazione.InstructionalButtons.Add(button3);
                Creazione.InstructionalButtons.Add(button2);
                Genitori.InstructionalButtons.Add(button3);
                Genitori.InstructionalButtons.Add(button2);
                Apparenze.InstructionalButtons.Add(button3);
                Apparenze.InstructionalButtons.Add(button2);
                Apparenze.InstructionalButtons.Add(button5);
                Dettagli.InstructionalButtons.Add(button3);
                Dettagli.InstructionalButtons.Add(button2);
                Dettagli.InstructionalButtons.Add(button4);

                #endregion

                #region Genitori

                UIMenuHeritageWindow heritageWindow = new UIMenuHeritageWindow(_data.Skin.face.mom, _data.Skin.face.dad);
                Genitori.AddWindow(heritageWindow);
                List<dynamic> lista = new List<dynamic>();
                for (int i = 0; i < 101; i++) lista.Add(i);
                UIMenuListItem mamma = new UIMenuListItem("Mamma", momfaces, _data.Skin.face.mom);
                UIMenuListItem papa = new UIMenuListItem("Papà", dadfaces, _data.Skin.face.dad);
                UIMenuSliderItem resemblance = new UIMenuSliderItem(GetLabelText("FACE_H_DOM"), "", true) { Multiplier = 2, Value = (int)Math.Round(_data.Skin.resemblance * 100) };
                UIMenuSliderItem skinmix = new UIMenuSliderItem(GetLabelText("FACE_H_STON"), "", true) { Multiplier = 2, Value = (int)Math.Round(_data.Skin.skinmix * 100) };
                Genitori.AddItem(mamma);
                Genitori.AddItem(papa);
                Genitori.AddItem(resemblance);
                Genitori.AddItem(skinmix);

                #endregion

                #region Dettagli

                _arcSopr = new UIMenuListItem(GetLabelText("FACE_F_BROW"), _arcSop, 0);
                _occhi = new UIMenuListItem(GetLabelText("FACE_F_EYES"), _occ, 0, "Guarda bene le palpebre!");
                _naso = new UIMenuListItem(GetLabelText("FACE_F_NOSE"), _nas, 0);
                _nasoPro = new UIMenuListItem(GetLabelText("FACE_F_NOSEP"), new List<dynamic>() { "Standard", "Breve", "Lungo" }, 0);
                _nasoPun = new UIMenuListItem(GetLabelText("FACE_F_NOSET"), new List<dynamic>() { "Standard", "Punta su", "Punta giù" }, 0);
                _zigo = new UIMenuListItem(GetLabelText("FACE_F_CHEEK"), new List<dynamic>() { "Standard", "In dentro", "In fuori" }, 0);
                _guance = new UIMenuListItem(GetLabelText("FACE_F_CHEEKS"), new List<dynamic>() { "Standard", "Magre", "Paffute" }, 0);
                _labbra = new UIMenuListItem(GetLabelText("FACE_F_LIPS"), new List<dynamic>() { "Standard", "Sottili", "Carnose" }, 0);
                _masce = new UIMenuListItem(GetLabelText("FACE_F_JAW"), new List<dynamic>() { "Standard", "Stretta", "Larga" }, 0);
                _mentoPro = new UIMenuListItem(GetLabelText("FACE_F_CHIN"), new List<dynamic>() { "Standard", "In dentro", "In fuori" }, 0);
                _mentoFor = new UIMenuListItem(GetLabelText("FACE_F_CHINS"), new List<dynamic>() { "Standard", "Squadrato", "A punta" }, 0);
                _collo = new UIMenuListItem("Collo", new List<dynamic>() { "Standard", "Stretto", "Largo" }, 0);

                UIMenuGridPanel GridSopr = new(GetLabelText("FACE_F_UP_B"), GetLabelText("FACE_F_IN_B"), GetLabelText("FACE_F_OUT_B"), GetLabelText("FACE_F_DOWN_B"), new PointF(_data.Skin.face.tratti[7], _data.Skin.face.tratti[6]));
                UIMenuGridPanel GridOcch = new(GetLabelText("FACE_F_SQUINT"), GetLabelText("FACE_F_WIDE_E"), new PointF(_data.Skin.face.tratti[11], 0));
                UIMenuGridPanel GridNaso = new(GetLabelText("FACE_F_UP_N"), GetLabelText("FACE_F_NAR_N"), GetLabelText("FACE_F_WIDE_N"), GetLabelText("FACE_F_DOWN_N"), new PointF(_data.Skin.face.tratti[0], _data.Skin.face.tratti[1]));
                UIMenuGridPanel GridNasoPro = new(GetLabelText("FACE_F_CROOK"), GetLabelText("FACE_F_SHORT"), GetLabelText("FACE_F_LONG"), GetLabelText("FACE_F_CURV"), new PointF(_data.Skin.face.tratti[2], _data.Skin.face.tratti[3]));
                UIMenuGridPanel GridNasoPun = new(GetLabelText("FACE_F_TIPU"), GetLabelText("FACE_F_BRL"), GetLabelText("FACE_F_BRR"), GetLabelText("FACE_F_TIPD"), new PointF(_data.Skin.face.tratti[5], _data.Skin.face.tratti[4]));
                UIMenuGridPanel GridZigo = new(GetLabelText("FACE_F_UP_CHEE"), GetLabelText("FACE_F_IN_C"), GetLabelText("FACE_F_OUT_C"), GetLabelText("FACE_F_DOWN_C"), new PointF(_data.Skin.face.tratti[9], _data.Skin.face.tratti[8]));
                UIMenuGridPanel GridGuance = new(GetLabelText("FACE_F_GAUNT"), GetLabelText("FACE_F_PUFF"), new PointF(_data.Skin.face.tratti[10], 0));
                UIMenuGridPanel GridLabbra = new(GetLabelText("FACE_F_THIN"), GetLabelText("FACE_F_FAT"), new PointF(_data.Skin.face.tratti[12], 0));
                UIMenuGridPanel GridMasce = new(GetLabelText("FACE_F_RND"), GetLabelText("FACE_F_NAR_J"), GetLabelText("FACE_F_WIDE_J"), GetLabelText("FACE_F_SQ_J"), new PointF(_data.Skin.face.tratti[13], _data.Skin.face.tratti[14]));
                UIMenuGridPanel GridMentoPro = new(GetLabelText("FACE_F_UP_CHIN"), GetLabelText("FACE_F_IN_CH"), GetLabelText("FACE_F_OUT_CH"), GetLabelText("FACE_F_DOWN_CH"), new PointF(_data.Skin.face.tratti[16], _data.Skin.face.tratti[15]));
                UIMenuGridPanel GridMentoFor = new(GetLabelText("FACE_F_RDD"), GetLabelText("FACE_F_SQ_CH"), GetLabelText("FACE_F_PTD"), GetLabelText("FACE_F_BUM"), new PointF(_data.Skin.face.tratti[18], _data.Skin.face.tratti[17]));
                UIMenuGridPanel GridCollo = new("Stretto", "Largo", new PointF(_data.Skin.face.tratti[19], 0));

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

                UIMenuListItem Capelli = new("", HairUomo, _data.Skin.hair.style);
                UIMenuListItem sopracciglia = new(GetLabelText("FACE_F_EYEBR"), eyebrow, _data.Skin.facialHair.eyebrow.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuColorPanel soprCol1 = new("Colore principale", ColorPanelType.Hair);
                UIMenuColorPanel soprCol2 = new("Colore secondario", ColorPanelType.Hair);
                UIMenuPercentagePanel soprOp = new("Opacità", "0%", "100%");
                sopracciglia.AddPanel(soprCol1);
                sopracciglia.AddPanel(soprCol2);
                sopracciglia.AddPanel(soprOp);
                UIMenuListItem Barba = new(GetLabelText("FACE_F_BEARD"), Beards, _data.Skin.facialHair.beard.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuColorPanel BarbaCol1 = new("Colore principale", ColorPanelType.Hair);
                UIMenuColorPanel BarbaCol2 = new("Colore secondario", ColorPanelType.Hair);
                UIMenuPercentagePanel BarbaOp = new("Opacità", "0%", "100%");
                Barba.AddPanel(BarbaCol1);
                Barba.AddPanel(BarbaCol2);
                Barba.AddPanel(BarbaOp);
                UIMenuListItem SkinBlemishes = new(GetLabelText("FACE_F_SKINB"), blemishes, _data.Skin.blemishes.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuPercentagePanel BlemOp = new("Opacità", "0%", "100%");
                SkinBlemishes.AddPanel(BlemOp);
                UIMenuListItem SkinAgeing = new(GetLabelText("FACE_F_SKINA"), Ageing, _data.Skin.ageing.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuPercentagePanel AgeOp = new("Opacità", "0%", "100%");
                SkinAgeing.AddPanel(AgeOp);
                UIMenuListItem SkinComplexion = new(GetLabelText("FACE_F_SKC"), Complexions, _data.Skin.complexion.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuPercentagePanel CompOp = new("Opacità", "0%", "100%");
                SkinComplexion.AddPanel(CompOp);
                UIMenuListItem SkinMoles = new(GetLabelText("FACE_F_MOLE"), Nei_e_Porri, _data.Skin.freckles.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuPercentagePanel FrecOp = new("Opacità", "0%", "100%");
                SkinMoles.AddPanel(FrecOp);
                UIMenuListItem SkinDamage = new(GetLabelText("FACE_F_SUND"), Danni_Pelle, _data.Skin.skinDamage.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuPercentagePanel DamageOp = new("Opacità", "0%", "100%");
                SkinDamage.AddPanel(DamageOp);
                UIMenuListItem EyeColor = new(GetLabelText("FACE_APP_EYE"), Colore_Occhi, _data.Skin.eye.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuListItem EyeMakup = new(GetLabelText("FACE_F_EYEM"), Trucco_Occhi, _data.Skin.makeup.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuPercentagePanel MakupOp = new("Opacità", "0%", "100%");
                EyeMakup.AddPanel(MakupOp);
                UIMenuListItem Blusher = new(GetLabelText("FACE_F_BLUSH"), BlusherDonna, _data.Skin.blusher.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
                UIMenuColorPanel BlushCol1 = new("Colore principale", ColorPanelType.Makeup);
                UIMenuColorPanel BlushCol2 = new("Colore secondario", ColorPanelType.Makeup);
                UIMenuPercentagePanel BlushOp = new("Opacità", "0%", "100%");
                Blusher.AddPanel(BlushCol1);
                Blusher.AddPanel(BlushCol2);
                Blusher.AddPanel(BlushOp);
                UIMenuListItem LipStick = new UIMenuListItem(GetLabelText("FACE_F_LIPST"), Lipstick, _data.Skin.lipstick.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
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

                #region Stats

                UIMenuStatsItem stamina = new UIMenuStatsItem(GetLabelText("FACE_STAM"), GetLabelText("FACE_H_STA"), 0, HudColor.HUD_COLOUR_FREEMODE);
                UIMenuStatsItem shooting = new UIMenuStatsItem(GetLabelText("FACE_SHOOT"), GetLabelText("FACE_H_SHO"), 0, HudColor.HUD_COLOUR_FREEMODE);
                UIMenuStatsItem strength = new UIMenuStatsItem(GetLabelText("FACE_STR"), GetLabelText("FACE_H_STR"), 0, HudColor.HUD_COLOUR_FREEMODE);
                UIMenuStatsItem stealth = new UIMenuStatsItem(GetLabelText("FACE_STEALTH"), GetLabelText("FACE_H_STE"), 0, HudColor.HUD_COLOUR_FREEMODE);
                UIMenuStatsItem flying = new UIMenuStatsItem(GetLabelText("FACE_FLY"), GetLabelText("FACE_H_FLY"), 0, HudColor.HUD_COLOUR_FREEMODE);
                UIMenuStatsItem driving = new UIMenuStatsItem(GetLabelText("FACE_DRIV"), GetLabelText("FACE_H_DRI"), 0, HudColor.HUD_COLOUR_FREEMODE);
                UIMenuStatsItem lungs = new UIMenuStatsItem(GetLabelText("FACE_LUNG"), GetLabelText("FACE_H_LCP"), 0, HudColor.HUD_COLOUR_FREEMODE);

                Statistiche.AddItem(stamina);
                Statistiche.AddItem(shooting);
                Statistiche.AddItem(strength);
                Statistiche.AddItem(stealth);
                Statistiche.AddItem(flying);
                Statistiche.AddItem(driving);
                Statistiche.AddItem(lungs);

                #endregion

                #region VESTITI
                var styleList = new List<dynamic>();
                for (int i = 0; i < 8; i++)styleList.Add(GetLabelText("FACE_A_STY_" + i));

                var outfitList = new List<dynamic>();
                for (int i = 0; i < 8; i++) outfitList.Add(GetLabelText(getOutfit(i, _selezionato == "Maschio")));

                var hatList = new List<dynamic>() { GetLabelText("FACE_OFF") };

                var glassesList = new List<dynamic>() { GetLabelText("FACE_OFF") };


                UIMenuListItem stile = new(GetLabelText("FACE_APP_STY"), styleList, 0, GetLabelText("FACE_APPA_H"));
                UIMenuListItem outfit = new(GetLabelText("FACE_APP_OUT"), outfitList, 0, GetLabelText("FACE_APPA_H"));

                UIMenuListItem hat = new(GetLabelText("FACE_HAT"), hatList, 0, GetLabelText("FACE_APPA_H"));
                UIMenuListItem glasses = new(GetLabelText("FACE_GLS"), glassesList, 0, GetLabelText("FACE_APPA_H"));

                //UIMenuListItem outfit = new UIMenuListItem(GetLabelText("FACE_APP_OUT"));
                Apparel.AddItem(stile);
                Apparel.AddItem(outfit);
                Apparel.AddItem(hat);
                Apparel.AddItem(glasses);

                #endregion

                #endregion

                #region CorpoMenu

                #region Sesso

                Sesso.OnListChanged += async (item, _newIndex) =>
                {
                    HUD.MenuPool.CloseAllMenus();
                    Screen.Effects.Start(ScreenEffect.MpCelebWin);
                    await BaseScript.Delay(1000);
                    Screen.Fading.FadeOut(1000);
                    await BaseScript.Delay(1000);
                    HUD.MenuPool.CloseAllMenus();
                    Creazione.Clear();
                    Screen.Effects.Stop(ScreenEffect.MpCelebWin);

                    switch (_newIndex)
                    {
                        case 0:
                            _dataFemmina = _data;
                            _data = _dataMaschio;
                            _boardScalep1.CallFunction("SET_BOARD", GetLabelText("FACE_N_CHAR"), _data.CharID.ToString(), "THE LAST GALAXY BY MANUPS4E", "", 0, 1, 0);
                            _selezionato = "Maschio";

                            break;
                        case 1:
                            _dataMaschio = _data;
                            _data = _dataFemmina;
                            _boardScalep1.CallFunction("SET_BOARD", GetLabelText("FACE_N_CHAR"), _data.CharID.ToString(), "THE LAST GALAXY BY MANUPS4E", "", 0, 1, 0);
                            _selezionato = "Femmina";

                            break;
                    }

                    AggiornaModel(_data.ToJson());
                    foreach (Prop obj in World.GetAllProps())
                        if (obj.Model.Hash == GetHashKey("prop_police_id_board") || obj.Model.Hash == GetHashKey("prop_police_id_text"))
                    CharCreationMenu(_data.Skin.sex);
                };

                #endregion

                #region Genitori

                Genitori.OnListChange += async (_sender, _listItem, _newIndex) =>
                {
                    if (_listItem == mamma)
                    {
                        _data.Skin.face.mom = _newIndex;
                        heritageWindow.Index(_data.Skin.face.mom, _data.Skin.face.dad);
                    }
                    else if (_listItem == papa)
                    {
                        _data.Skin.face.dad = _newIndex;
                        heritageWindow.Index(_data.Skin.face.mom, _data.Skin.face.dad);
                    }

                    if (_data.Skin.sex == "Maschio")
                        _dataMaschio = _data;
                    else
                        _dataFemmina = _data;
                    UpdateFace(Cache.PlayerCache.MyPlayer.Ped.Handle, _data.Skin);
                };
                Genitori.OnSliderChange += async (_sender, _item, _newIndex) =>
                {
                    if (_item == resemblance)
                        _data.Skin.resemblance = _newIndex / 100f;
                    else if (_item == skinmix)
                        _data.Skin.skinmix = _newIndex / 100f;
                    if (_data.Skin.sex == "Maschio")
                        _dataMaschio = _data;
                    else
                        _dataFemmina = _data;
                    UpdateFace(Cache.PlayerCache.MyPlayer.Ped.Handle, _data.Skin);
                };

                #endregion

                #region Apparenze
                Apparenze.OnColorPanelChange += (a, b, c) =>
                {
                    if (a == Capelli)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.hair.color[0] = c;
                        else if (b == a.Panels[1])
                            _data.Skin.hair.color[1] = c;
                    }
                    else if (a == sopracciglia)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.facialHair.eyebrow.color[0] = c;
                        else if (b == a.Panels[1])
                            _data.Skin.facialHair.eyebrow.color[1] = c;
                    }
                    else if (a == Barba)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.facialHair.beard.color[0] = c;
                        else if (b == a.Panels[1])
                            _data.Skin.facialHair.beard.color[1] = c;
                    }
                    else if (a == Blusher)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.blusher.color[0] = c;
                        else if (b == a.Panels[1])
                            _data.Skin.blusher.color[1] = c;
                    }
                    else if (a == LipStick)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.lipstick.color[0] = c;
                        else if (b == a.Panels[1])
                            _data.Skin.lipstick.color[1] = c;
                    }
                    UpdateFace(Cache.PlayerCache.MyPlayer.Ped.Handle, _data.Skin);
                };
                Apparenze.OnPercentagePanelChange += (a, b, c) =>
                {
                    var perc = c / 100;
                    if (a == sopracciglia)
                    {
                        if (b == a.Panels[2])
                            _data.Skin.facialHair.eyebrow.opacity = perc;
                    }
                    else if (a == Barba)
                    {
                        if (b == a.Panels[2])
                            _data.Skin.facialHair.beard.opacity = perc;
                    }
                    else if (a == Blusher)
                    {
                        if (b == a.Panels[2])
                            _data.Skin.blusher.opacity = perc;
                    }
                    else if (a == LipStick)
                    {
                        if (b == a.Panels[2])
                            _data.Skin.lipstick.opacity = perc;
                    }
                    else if (a == SkinBlemishes)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.blemishes.opacity = perc;
                    }
                    else if (a == SkinAgeing)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.ageing.opacity = perc;
                    }
                    else if (a == SkinComplexion)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.complexion.opacity = perc;
                    }
                    else if (a == SkinMoles)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.freckles.opacity = perc;
                    }
                    else if (a == SkinDamage)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.skinDamage.opacity = perc;
                    }

                    else if (a == EyeMakup)
                    {
                        if (b == a.Panels[0])
                            _data.Skin.makeup.opacity = perc;
                    }
                    UpdateFace(Cache.PlayerCache.MyPlayer.Ped.Handle, _data.Skin);
                };

                Apparenze.OnListChange += async (_sender, _listItem, _newIndex) =>
                {
                    if (_listItem == Capelli)
                        _data.Skin.hair.style = _newIndex;

                    else if (_listItem == sopracciglia)
                        _data.Skin.facialHair.eyebrow.style = _newIndex;

                    else if (_listItem == Barba)
                        _data.Skin.facialHair.beard.style = (string)_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF") ? 255 : _newIndex - 1;

                    else if (_listItem == Blusher)
                        _data.Skin.blusher.style = (string)_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF") ? 255 : _newIndex - 1;

                    else if (_listItem == LipStick)
                        _data.Skin.lipstick.style = (string)_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF") ? 255 : _newIndex - 1;

                    else if (_listItem == SkinBlemishes)
                        _data.Skin.blemishes.style = (string)_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF") ? 255 : _newIndex - 1;

                    else if (_listItem == SkinAgeing)
                        _data.Skin.ageing.style = (string)_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF") ? 255 : _newIndex - 1;

                    else if (_listItem == SkinComplexion)
                        _data.Skin.complexion.style = (string)_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF") ? 255 : _newIndex - 1;

                    else if (_listItem == SkinMoles)
                        _data.Skin.freckles.style = (string)_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF") ? 255 : _newIndex - 1;

                    else if (_listItem == SkinDamage)
                        _data.Skin.skinDamage.style = (string)_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF") ? 255 : _newIndex - 1;

                    else if (_listItem == EyeColor) _data.Skin.eye.style = _newIndex;

                    else if (_listItem == EyeMakup)
                        _data.Skin.makeup.style = (string)_listItem.Items[_newIndex] == GetLabelText("FACE_F_P_OFF") ? 255 : _newIndex - 1;

                    UpdateFace(Cache.PlayerCache.MyPlayer.Ped.Handle, _data.Skin);
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

                Dettagli.OnGridPanelChange += (a, b, c) =>
                {
                    Client.Logger.Debug("C => " + c.ToString());
                    if (a == _arcSopr)
                    {
                        _data.Skin.face.tratti[7] = Funzioni.Denormalize(c.X, -1f, 1f);
                        _data.Skin.face.tratti[6] = Funzioni.Denormalize(c.Y, -1f, 1f);
                    }
                    else if (a == _occhi)
                    {
                        _data.Skin.face.tratti[11] = Funzioni.Denormalize(-c.X, -1f, 1f);
                    }
                    else if (a == _naso)
                    {
                        _data.Skin.face.tratti[0] = Funzioni.Denormalize(c.X, -1f, 1f);
                        _data.Skin.face.tratti[1] = Funzioni.Denormalize(c.Y, -1f, 1f);
                    }
                    else if (a == _nasoPro)
                    {
                        _data.Skin.face.tratti[2] = Funzioni.Denormalize(-c.X, -1f, 1f);
                        _data.Skin.face.tratti[3] = Funzioni.Denormalize(c.Y, -1f, 1f);
                    }
                    else if (a == _nasoPun)
                    {
                        _data.Skin.face.tratti[5] = Funzioni.Denormalize(-c.X, -1f, 1f);
                        _data.Skin.face.tratti[4] = Funzioni.Denormalize(c.Y, -1f, 1f);
                    }
                    else if (a == _zigo)
                    {
                        _data.Skin.face.tratti[9] = Funzioni.Denormalize(c.X, -1f, 1f);
                        _data.Skin.face.tratti[8] = Funzioni.Denormalize(c.Y, -1f, 1f);
                    }
                    else if (a == _guance)
                    {
                        _data.Skin.face.tratti[10] = Funzioni.Denormalize(-c.X, -1f, 1f);
                    }
                    else if (a == _collo)
                    {
                        _data.Skin.face.tratti[19] = Funzioni.Denormalize(c.X, -1f, 1f);
                    }
                    else if (a == _labbra)
                    {
                        _data.Skin.face.tratti[12] = Funzioni.Denormalize(-c.X, -1f, 1f);
                    }
                    else if (a == _masce)
                    {
                        _data.Skin.face.tratti[13] = Funzioni.Denormalize(c.X, -1f, 1f);
                        _data.Skin.face.tratti[14] = Funzioni.Denormalize(c.Y, -1f, 1f);
                    }
                    else if (a == _mentoPro)
                    {
                        _data.Skin.face.tratti[16] = Funzioni.Denormalize(c.X, -1f, 1f);
                        _data.Skin.face.tratti[15] = Funzioni.Denormalize(c.Y, -1f, 1f);
                    }
                    else if (a == _mentoFor)
                    {
                        _data.Skin.face.tratti[17] = Funzioni.Denormalize(-c.X, -1f, 1f);
                        _data.Skin.face.tratti[18] = Funzioni.Denormalize(c.Y, -1f, 1f);
                    }
                    UpdateFace(Cache.PlayerCache.MyPlayer.Ped.Handle, _data.Skin);
                };

                Dettagli.OnListChange += async (_sender, _listItem, _newIndex) =>
                {
                    if (_listItem == _arcSopr)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                        {
                            if (oldIndexarcSopr != _newIndex)
                            {
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Alte":
                                        _data.Skin.face.tratti[6] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.00001f);
                                        break;
                                    case "Basse":
                                        _data.Skin.face.tratti[6] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.999999f);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[6] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.5f);

                                        break;
                                }

                                oldIndexarcSopr = _newIndex;
                            }
                        }
                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[7] = Funzioni.Denormalize(var.X, -1, 1);
                        _data.Skin.face.tratti[6] = Funzioni.Denormalize(var.Y, -1, 1);
                    }

                    if (_listItem == _occhi)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexOcchi != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Grandi":
                                        _data.Skin.face.tratti[11] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Stretti":
                                        _data.Skin.face.tratti[11] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[11] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[11] = Funzioni.Denormalize(-var.X, -1, 1);
                    }

                    if (_listItem == _naso)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexNaso != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Piccolo":
                                        _data.Skin.face.tratti[0] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Grande":
                                        _data.Skin.face.tratti[0] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[0] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[0] = Funzioni.Denormalize(var.X, -1, 1);
                        _data.Skin.face.tratti[1] = Funzioni.Denormalize(var.Y, -1, 1);
                    }

                    if (_listItem == _nasoPro)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexNasoPro != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Breve":
                                        _data.Skin.face.tratti[2] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Lungo":
                                        _data.Skin.face.tratti[2] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[2] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[3] = Funzioni.Denormalize(var.Y, -1, 1);
                        _data.Skin.face.tratti[2] = Funzioni.Denormalize(var.X, -1, 1);
                    }

                    if (_listItem == _nasoPun)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexNasoPun != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Punta su":
                                        _data.Skin.face.tratti[5] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.00001f);

                                        break;
                                    case "Punta giù":
                                        _data.Skin.face.tratti[5] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.999999f);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[5] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF((_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.X, 0.5f);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[5] = Funzioni.Denormalize(-var.X, -1, 1);
                        _data.Skin.face.tratti[4] = Funzioni.Denormalize(var.Y, -1, 1);
                    }

                    if (_listItem == _zigo)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexZigo != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "In dentro":
                                        _data.Skin.face.tratti[8] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "In fuori":
                                        _data.Skin.face.tratti[8] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[8] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[9] = Funzioni.Denormalize(var.X, -1, 1);
                        _data.Skin.face.tratti[8] = Funzioni.Denormalize(var.Y, -1, 1);
                    }

                    if (_listItem == _guance)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexGuance != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Paffute":
                                        _data.Skin.face.tratti[10] = 0.00001f;
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Magre":
                                        _data.Skin.face.tratti[10] = 0.999999f;
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[10] = 0.5f;
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[10] = Funzioni.Denormalize(-var.X, -1, 1);
                    }

                    if (_listItem == _collo)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexCollo != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Stretto":
                                        _data.Skin.face.tratti[19] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Largo":
                                        _data.Skin.face.tratti[19] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[19] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[19] = Funzioni.Denormalize(var.X, -1, 1);
                    }

                    if (_listItem == _labbra)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexLabbra != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Sottili":
                                        _data.Skin.face.tratti[12] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Carnose":
                                        _data.Skin.face.tratti[12] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[12] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[12] = Funzioni.Denormalize(-var.X, -1, 1);
                    }

                    if (_listItem == _masce)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexMasce != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Stretta":
                                        _data.Skin.face.tratti[14] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Larga":
                                        _data.Skin.face.tratti[14] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[14] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[13] = Funzioni.Denormalize(-var.X, -1, 1);
                        _data.Skin.face.tratti[14] = Funzioni.Denormalize(var.Y, -1, 1);
                    }

                    if (_listItem == _mentoPro)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexMentoPro != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "In dentro":
                                        _data.Skin.face.tratti[15] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "In fuori":
                                        _data.Skin.face.tratti[15] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[15] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[16] = Funzioni.Denormalize(var.X, -1, 1);
                        _data.Skin.face.tratti[15] = Funzioni.Denormalize(var.Y, -1, 1);
                    }

                    if (_listItem == _mentoFor)
                    {
                        if (!(IsControlPressed(0, 24) || IsDisabledControlPressed(0, 24)))
                            if (oldIndexMentoFor != _newIndex)
                                switch (_listItem.Items[_newIndex])
                                {
                                    case "Squadrato":
                                        _data.Skin.face.tratti[17] = Funzioni.Denormalize(0f, - 1f, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.00001f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "A punta":
                                        _data.Skin.face.tratti[17] = Funzioni.Denormalize(1, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.999999f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                    case "Standard":
                                        _data.Skin.face.tratti[17] = Funzioni.Denormalize(0.5f, -1, 1);
                                        (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(0.5f, (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition.Y);

                                        break;
                                }

                        PointF var = (_listItem.Panels[0] as UIMenuGridPanel).CirclePosition;
                        _data.Skin.face.tratti[18] = Funzioni.Denormalize(-var.X, -1, 1);
                        _data.Skin.face.tratti[17] = Funzioni.Denormalize(var.Y, -1, 1);
                    }

                    UpdateFace(Cache.PlayerCache.MyPlayer.Ped.Handle, _data.Skin);
                };

                #endregion

                #region VESTITI

                Apparel.OnListChange += async (sender, item, index) =>
                {
                    if (item == stile)
                    {
                        List<dynamic> list = new();
                        for(int i=0+index; i<8+index; i++)
                            list.Add(GetLabelText(getOutfit(i, _selezionato == "Maschio")));
                        outfit.ChangeList(list, 0);
                    }
                    /*
                    if (_data.Skin.sex == "Maschio")
                    {
                        Dressing dress = new Dressing(CompletiMaschio[index].Name, CompletiMaschio[index].Description, CompletiMaschio[index].ComponentDrawables, CompletiMaschio[index].ComponentTextures, CompletiMaschio[index].PropIndices, CompletiMaschio[index].PropTextures);
                        _data.Dressing = dress;
                        _dataMaschio = _data;
                    }
                    else
                    {
                        Dressing dress = new Dressing(CompletiFemmina[index].Name, CompletiFemmina[index].Description, CompletiFemmina[index].ComponentDrawables, CompletiFemmina[index].ComponentTextures, CompletiFemmina[index].PropIndices, CompletiFemmina[index].PropTextures);
                        _data.Dressing = dress;
                        _dataFemmina = _data;
                    }

                    UpdateDress(Cache.PlayerCache.MyPlayer.Ped.Handle, _data.Dressing);
                    TaskProvaClothes(Cache.PlayerCache.MyPlayer.Ped, sub_7dd83(1, 0, _data.Skin.sex));
                    */
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
                                if (_newMenu == Genitori || _newMenu == Dettagli || _newMenu == Apparenze) AnimateGameplayCamZoom(true, _ncam);

                                if (_newMenu == Apparenze)
                                {
                                    Apparenze.Clear();

                                    if (_selezionato == "Maschio")
                                    {
                                        Capelli = new UIMenuListItem(GetLabelText("FACE_HAIR"), HairUomo, _data.Skin.hair.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
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
                                        CapelCol1.CurrentSelection = _data.Skin.hair.color[0];
                                        CapelCol2.CurrentSelection = _data.Skin.hair.color[1];
                                        soprCol1.CurrentSelection = _data.Skin.facialHair.eyebrow.color[0];
                                        soprCol2.CurrentSelection = _data.Skin.facialHair.eyebrow.color[1];
                                        soprOp.Percentage = _data.Skin.facialHair.eyebrow.opacity * 100;
                                        BarbaCol1.CurrentSelection = _data.Skin.facialHair.beard.color[0];
                                        BarbaCol2.CurrentSelection = _data.Skin.facialHair.beard.color[1];
                                        BarbaOp.Percentage = _data.Skin.facialHair.beard.opacity * 100;
                                        BlemOp.Percentage = _data.Skin.blemishes.opacity * 100;
                                        AgeOp.Percentage = _data.Skin.ageing.opacity * 100;
                                        CompOp.Percentage = _data.Skin.complexion.opacity * 100;
                                        FrecOp.Percentage = _data.Skin.freckles.opacity * 100;
                                        DamageOp.Percentage = _data.Skin.skinDamage.opacity * 100;
                                        MakupOp.Percentage = _data.Skin.makeup.opacity * 100;
                                        LipCol1.CurrentSelection = _data.Skin.lipstick.color[0];
                                        LipCol2.CurrentSelection = _data.Skin.lipstick.color[1];
                                        LipOp.Percentage = _data.Skin.lipstick.opacity * 100;
                                    }
                                    else
                                    {
                                        Capelli = new UIMenuListItem(GetLabelText("FACE_HAIR"), HairDonna, _data.Skin.hair.style, "Modifica il tuo aspetto, usa il ~y~mouse~w~ per modificare i pannelli");
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
                                        CapelCol1.CurrentSelection = _data.Skin.hair.color[0];
                                        CapelCol2.CurrentSelection = _data.Skin.hair.color[1];
                                        soprCol1.CurrentSelection = _data.Skin.facialHair.eyebrow.color[0];
                                        soprCol2.CurrentSelection = _data.Skin.facialHair.eyebrow.color[1];
                                        soprOp.Percentage = _data.Skin.facialHair.eyebrow.opacity * 100;
                                        BlemOp.Percentage = _data.Skin.blemishes.opacity * 100;
                                        AgeOp.Percentage = _data.Skin.ageing.opacity * 100;
                                        CompOp.Percentage = _data.Skin.complexion.opacity * 100;
                                        FrecOp.Percentage = _data.Skin.freckles.opacity * 100;
                                        DamageOp.Percentage = _data.Skin.skinDamage.opacity * 100;
                                        MakupOp.Percentage = _data.Skin.makeup.opacity * 100;
                                        BlushCol1.CurrentSelection = _data.Skin.blusher.color[0];
                                        BlushCol2.CurrentSelection = _data.Skin.blusher.color[1];
                                        BlushOp.Percentage = _data.Skin.blusher.opacity * 100;
                                        LipCol1.CurrentSelection = _data.Skin.lipstick.color[0];
                                        LipCol2.CurrentSelection = _data.Skin.lipstick.color[1];
                                        LipOp.Percentage = _data.Skin.lipstick.opacity * 100;
                                    }
                                }
                                else if (_newMenu == Apparel)
                                {
                                    TaskCreaClothes(Cache.PlayerCache.MyPlayer.Ped, sub_7dd83(1, 0, _selezionato));
                                    //UpdateDress(Cache.PlayerCache.MyPlayer.Ped.Handle, CompletiFemmina[0]);
                                }

                                break;
                            }
                        case MenuState.ChangeBackward when _oldMenu == Genitori || _oldMenu == Dettagli || _oldMenu == Apparenze:
                            AnimateGameplayCamZoom(false, _ncam);

                            break;
                        case MenuState.ChangeBackward:
                            {
                                if (_oldMenu == Apparel)
                                {
                                    TaskClothesALoop(Cache.PlayerCache.MyPlayer.Ped, sub_7dd83(1, 0, _selezionato));
                                }

                                break;
                            }
                    }
                };

                #endregion

                #region CREA_BUTTON_FINISH

                UIMenuItem Salva = new UIMenuItem("Salva Personaggio", "Pronto per ~y~entrare in gioco~w~?", HudColor.HUD_COLOUR_FREEMODE_DARK, HudColor.HUD_COLOUR_FREEMODE);
                Salva.SetRightBadge(BadgeIcon.TICK);
                Creazione.AddItem(Salva);
                Salva.Activated += async (_selectedItem, _index) =>
                {
                    Screen.Fading.FadeOut(800);
                    await BaseScript.Delay(1000);
                    if (_dummyPed != null)
                        if (_dummyPed.Exists())
                            _dummyPed.Delete();
                    Creazione.Visible = false;
                    HUD.MenuPool.CloseAllMenus();
                    BD1.Detach();
                    BD1.Delete();
                    Cache.PlayerCache.MyPlayer.Ped.Detach();
                    Client.Instance.Events.Send("tlg:roleplay:finishCharServer", _data.ToJson());

                    //Cache.PlayerCache.MyPlayer.User.FreeRoamChar = await Client.Instance.Events.Get<FreeRoamChar>("lprp:Select_Char", _data.CharID);
                    //BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", Cache.MyPlayer.User.char_current);

                    Client.Instance.RemoveTick(Controllo);
                    Client.Instance.RemoveTick(Scaleform);
                    Client.Instance.RemoveTick(TastiMenu);
                    //TODO: GESTIRE CAMFIRSTTIME
                    //CamerasFirstTime.FirstTimeTransition(_data.CharID == 1);
                    RemoveAnimDict("mp_character_creation@lineup@male_a");
                    RemoveAnimDict("mp_character_creation@lineup@male_b");
                    RemoveAnimDict("mp_character_creation@lineup@female_a");
                    RemoveAnimDict("mp_character_creation@lineup@female_b");
                    RemoveAnimDict("mp_character_creation@customise@male_a");
                    RemoveAnimDict("mp_character_creation@customise@female_a");
                };

                #endregion

                Creazione.Visible = true;
                Client.Instance.AddTick(TastiMenu);
            }
            catch
            {
                Client.Logger.Error("MenuCreazione");
            }
        }

        #endregion

        #region Controllo tasti

        private static float CoordX;
        private static bool left = false;
        private static bool right = false;
        private static float CoordY;

        public static async Task TastiMenu()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;
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

                            _data.Skin.face.tratti[7] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[7] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[6] = CoordY * 2f - 1f;

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[6] = CoordY * 2f - 1f;
                            (_arcSopr.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
                        }
                    }

                    if (_occhi.Selected)
                    {
                        PointF var = (_occhi.Panels[0] as UIMenuGridPanel).CirclePosition;
                        CoordX = var.X;
                        CoordY = var.Y;

                        if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5))
                        {
                            if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
                            {
                                CoordX += 0.03f;
                                if (CoordX > 1f) CoordX = 1f;
                            }

                            _data.Skin.face.tratti[11] = (CoordX * 2f - 1f) / -1;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[11] = (CoordX * 2f - 1f) / -1;
                            (_occhi.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, .5f);
                        }
                    }

                    if (_guance.Selected)
                    {
                        PointF var = (_guance.Panels[0] as UIMenuGridPanel).CirclePosition;
                        CoordX = var.X;
                        CoordY = var.Y;

                        if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5))
                        {
                            if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
                            {
                                CoordX += 0.03f;
                                if (CoordX > 1f) CoordX = 1f;
                            }

                            _data.Skin.face.tratti[10] = (CoordX * 2f - 1f) / -1;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[10] = (CoordX * 2f - 1f) / -1;
                            (_guance.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, .5f);
                        }
                    }

                    if (_labbra.Selected)
                    {
                        PointF var = (_labbra.Panels[0] as UIMenuGridPanel).CirclePosition;
                        CoordX = var.X;
                        CoordY = var.Y;

                        if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5))
                        {
                            if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
                            {
                                CoordX += 0.03f;
                                if (CoordX > 1f) CoordX = 1f;
                            }

                            _data.Skin.face.tratti[12] = (CoordX * 2f - 1f) / -1;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[12] = (CoordX * 2f - 1f) / -1;
                            (_labbra.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, .5f);
                        }
                    }

                    if (_collo.Selected)
                    {
                        PointF var = (_collo.Panels[0] as UIMenuGridPanel).CirclePosition;
                        CoordX = var.X;
                        CoordY = var.Y;

                        if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) || IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5))
                        {
                            if (IsControlPressed(2, 6) || IsDisabledControlPressed(2, 6) && !IsInputDisabled(2))
                            {
                                CoordX += 0.03f;
                                if (CoordX > 1f) CoordX = 1f;
                            }

                            _data.Skin.face.tratti[19] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[19] = CoordX * 2f - 1f;
                            (_collo.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, .5f);
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

                            _data.Skin.face.tratti[0] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[0] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[1] = CoordY * 2f - 1f;

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[1] = CoordY * 2f - 1f;
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

                            _data.Skin.face.tratti[2] = (CoordX * 2f - 1f) / -1;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[2] = (CoordX * 2f - 1f) / -1;

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[3] = CoordY * 2f - 1f;

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[3] = CoordY * 2f - 1f;
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

                            _data.Skin.face.tratti[5] = (CoordX * 2f - 1f) / -1;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[5] = (CoordX * 2f - 1f) / -1;

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[4] = CoordY * 2f - 1f;

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[4] = CoordY * 2f - 1f;
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

                            _data.Skin.face.tratti[9] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[9] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[8] = CoordY * 2f - 1f;

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[8] = CoordY * 2f - 1f;
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

                            _data.Skin.face.tratti[13] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[13] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[14] = CoordY * 2f - 1f;

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[14] = CoordY * 2f - 1f;
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

                            _data.Skin.face.tratti[16] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[16] = CoordX * 2f - 1f;

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[15] = CoordY * 2f - 1f;

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[15] = CoordY * 2f - 1f;
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

                            _data.Skin.face.tratti[18] = (CoordX * 2f - 1f) / -1;

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[18] = (CoordX * 2f - 1f) / -1;

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[17] = CoordY * 2f - 1f;

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[17] = CoordY * 2f - 1f;
                            (_mentoFor.Panels[0] as UIMenuGridPanel).CirclePosition = new PointF(CoordX, CoordY);
                        }
                    }

                    if (_data.Skin.sex == "Maschio")
                        _dataFemmina = _data;
                    else
                        _dataMaschio = _data;
                    UpdateFace(playerPed.Handle, _data.Skin);
                }

            await Task.FromResult(0);
        }

        #endregion

        #endregion

        private static Camera ncamm = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));

        static void func_1711(int uParam0, float fParam1, float fParam2, float fParam3, float fParam4)
        {
            N_0xf55e4046f6f831dc(uParam0, fParam1);
            N_0xe111a7c0d200cbc5(uParam0, fParam2);
            SetCamDofFnumberOfLens(uParam0, fParam3);
            SetCamDofMaxNearInFocusDistanceBlendLevel(uParam0, fParam4);
        }

        public static async void AnimateGameplayCamZoom(bool toggle, Camera ncam)
        {

            if (toggle)
            {
                func_1711(ncam.Handle, 3f, 1f, 1.2f, 1f);
                ncamm = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));
                ncamm.Position = new Vector3(402.6746f, -1000.129f, -98.46554f);
                ncamm.Rotation = new Vector3(0.861356f, 0f, -2.348183f);
                Cache.PlayerCache.MyPlayer.Ped.IsVisible = true;
                ncamm.FieldOfView = 10.00255f;
                ncamm.IsActive = true;
                func_1711(ncamm.Handle, 3.8f, 1f, 1.2f, 1f);
                ncam.InterpTo(ncamm, 300, 1, 1);
                Game.PlaySound("Zoom_In", "MUGSHOT_CHARACTER_CREATION_SOUNDS");
                while (ncam.IsInterpolating) await BaseScript.Delay(0);
            }
            else
            {
                func_1711(ncamm.Handle, 3.8f, 1f, 1.2f, 1f);
                func_1711(ncam.Handle, 3f, 1f, 1.2f, 1f);
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

        public static async void ped_cre_board(FreeRoamChar data)
        {
            Cache.PlayerCache.MyPlayer.Ped.BlockPermanentEvents = true;
            sub_7cddb();
            Pol_Board2(data);
        }

        private static Prop BD1;
        private static Prop Overlay1;

        public static async void Pol_Board2(FreeRoamChar data)
        {
            Model bd1 = new Model("prop_police_id_board");
            bd1.Request();
            Model overlay1 = new Model("prop_police_id_text");
            overlay1.Request();
            while (!bd1.IsLoaded) await BaseScript.Delay(0);
            while (!overlay1.IsLoaded) await BaseScript.Delay(0);
            BD1 = await Funzioni.SpawnLocalProp(bd1.Hash, new Vector3(402.91f, -996.74f, -180.00025f), true, true);
            Overlay1 = await Funzioni.SpawnLocalProp(overlay1.Hash, new(402.91f, -996.74f, -180.00025f), true, false);
            while (!BD1.Exists()) await BaseScript.Delay(0);
            while (!Overlay1.Exists()) await BaseScript.Delay(0);
            Overlay1.AttachTo(BD1);
            BD1.AttachTo(Cache.PlayerCache.MyPlayer.Ped.Bones[Bone.PH_R_Hand], Vector3.Zero, Vector3.Zero);
            CreaScaleform_Cre(data, overlay1);
            Overlay1.MarkAsNoLongerNeeded();
            bd1.MarkAsNoLongerNeeded();
            overlay1.MarkAsNoLongerNeeded();
        }

        private static async void CreaScaleform_Cre(FreeRoamChar data, Model overlay)
        {
            _boardScalep1 = new Scaleform("mugshot_board_01");
            while (!_boardScalep1.IsLoaded) await BaseScript.Delay(0);
            _boardScalep1.CallFunction("SET_BOARD", GetLabelText("FACE_N_CHAR"), data.CharID.ToString(), "THE LAST GALAXY BY MANUPS4E", "", 0, 1, 0);
            _handle1 = CreateNamedRenderTargetForModel("ID_Text", (uint)overlay.Hash);
        }

        public static async Task Scaleform()
        {
            if (Cache.PlayerCache.MyPlayer.Ped.Exists())
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

            if (Creazione.Visible && Creazione.HasControlJustBeenPressed(UIMenu.MenuControls.Back))
            {
                HUD.MenuPool.CloseAllMenus();
                NativeUIScaleform.Warning.ShowWarningWithButtons("La creazione verrà annullata", "Vuoi annullare la creazione del personaggio?", "Tornerai alla schermata di selezione.", new List<InstructionalButton>
                {
                    new InstructionalButton(Control.FrontendCancel, "No"),
                    new InstructionalButton(Control.FrontendAccept, "Si"),
                });
                NativeUIScaleform.Warning.OnButtonPressed += async (a) =>
                {
                    if (a.GamepadButton == Control.FrontendCancel)
                    {
                        Screen.Fading.FadeOut(0);
                        await BaseScript.Delay(100);
                        MenuCreazione(_d);
                    }
                    else if (a.GamepadButton == Control.FrontendAccept)
                    {
                        Screen.Fading.FadeOut(0);
                        await BaseScript.Delay(100);
                        if (_dummyPed != null)
                            if (_dummyPed.Exists())
                                _dummyPed.Delete();
                        //TODO: CAMBIARE SE TORNO INDIETRO
                        //LogIn.LogIn.Inizializza();
                    }
                };
            }
            await Task.FromResult(0);
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
            if (!Cache.PlayerCache.MyPlayer.Ped.IsInjured) SetFacialIdleAnimOverride(Cache.PlayerCache.MyPlayer.Ped.Handle, v_3, "0");
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

        public static async void UpdateFace(int Handle, Skin skin)
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
        }

        public static async void UpdateDress(int Handle, Dressing dress)
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
            TaskPlayAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, sub_7dd83(1, 0, _selezionato), "react_light", 8.0f, -8.0f, -1, 512, 0, false, false, false);
            TaskPlayAnim(Cache.PlayerCache.MyPlayer.Ped.Handle, sub_7dd83(1, 0, _selezionato), "Loop", 8.0f, -8.0f, -1, 513, 0, false, false, false);
            CloseSequenceTask(sequence);
            TaskPerformSequence(Cache.PlayerCache.MyPlayer.Ped.Handle, sequence);
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

        static string getOutfit(int iParam0, bool bParam1)
        {
            switch (iParam0)
            {
                case 0:
                    if (bParam1)
                        return "FACE_O_M_STR0";
                    else
                        return "FACE_O_F_STR0";
                case 1:
                    if (bParam1)
                        return "FACE_O_M_STR1";
                    else
                        return "FACE_O_F_STR1";
                case 2:
                    if (bParam1)
                        return "FACE_O_M_STR2";
                    else
                        return "FACE_O_F_STR2";
                case 3:
                    if (bParam1)
                        return "FACE_O_M_STR3";
                    else
                        return "FACE_O_F_STR3";
                case 4:
                    if (bParam1)
                        return "FACE_O_M_STR4";
                    else
                        return "FACE_O_F_STR4";
                case 5:
                    if (bParam1)
                        return "FACE_O_M_STR5";
                    else
                        return "FACE_O_F_STR5";
                case 6:
                    if (bParam1)
                        return "FACE_O_M_STR6";
                    else
                        return "FACE_O_F_STR6";
                case 7:
                    if (bParam1)
                        return "FACE_O_M_STR7";
                    else
                        return "FACE_O_F_STR7";
                case 8:
                    if (bParam1)
                        return "FACE_O_M_FLS0";
                    else
                        return "FACE_O_F_FLS0";
                case 9:
                    if (bParam1)
                        return "FACE_O_M_FLS1";
                    else
                        return "FACE_O_F_FLS1";
                case 10:
                    if (bParam1)
                        return "FACE_O_M_FLS2";
                    else
                        return "FACE_O_F_FLS2";
                case 11:
                    if (bParam1)
                        return "FACE_O_M_FLS3";
                    else
                        return "FACE_O_F_FLS3";
                case 12:
                    if (bParam1)
                        return "FACE_O_M_FLS4";
                    else
                        return "FACE_O_F_FLS4";
                case 13:
                    if (bParam1)
                        return "FACE_O_M_FLS5";
                    else
                        return "FACE_O_F_FLS5";
                case 14:
                    if (bParam1)
                        return "FACE_O_M_FLS7";
                    else
                        return "FACE_O_F_FLS6";
                case 15:
                    if (bParam1)
                        return "FACE_O_M_FLS6";
                    else
                        return "FACE_O_F_FLS7";
                case 16:
                    if (bParam1)
                        return "FACE_O_M_PAR0";
                    else
                        return "FACE_O_F_PAR0";
                case 17:
                    if (bParam1)
                        return "FACE_O_M_PAR1";
                    else
                        return "FACE_O_F_PAR1";
                case 18:
                    if (bParam1)
                        return "FACE_O_M_PAR2";
                    else
                        return "FACE_O_F_PAR2";
                case 19:
                    if (bParam1)
                        return "FACE_O_M_PAR3";
                    else
                        return "FACE_O_F_PAR3";
                case 20:
                    if (bParam1)
                        return "FACE_O_M_PAR4";
                    else
                        return "FACE_O_F_PAR4";
                case 21:
                    if (bParam1)
                        return "FACE_O_M_PAR5";
                    else
                        return "FACE_O_F_PAR5";
                case 22:
                    if (bParam1)
                        return "FACE_O_M_PAR6";
                    else
                        return "FACE_O_F_PAR6";
                case 23:
                    if (bParam1)
                        return "FACE_O_M_PAR7";
                    else
                        return "FACE_O_F_PAR7";
                case 24:
                    if (bParam1)
                        return "FACE_O_M_BEA0";
                    else
                        return "FACE_O_F_BEA0";
                case 25:
                    if (bParam1)
                        return "FACE_O_M_BEA1";
                    else
                        return "FACE_O_F_BEA1";
                case 26:
                    if (bParam1)
                        return "FACE_O_M_BEA2";
                    else
                        return "FACE_O_F_BEA2";
                case 27:
                    if (bParam1)
                        return "FACE_O_M_BEA3";
                    else
                        return "FACE_O_F_BEA3";
                case 28:
                    if (bParam1)
                        return "FACE_O_M_BEA4";
                    else
                        return "FACE_O_F_BEA4";
                case 29:
                    if (bParam1)
                        return "FACE_O_M_BEA5";
                    else
                        return "FACE_O_F_BEA5";
                case 30:
                    if (bParam1)
                        return "FACE_O_M_BEA6";
                    else
                        return "FACE_O_F_BEA6";
                case 31:
                    if (bParam1)
                        return "FACE_O_M_BEA7";
                    else
                        return "FACE_O_F_BEA7";
                case 32:
                    if (bParam1)
                        return "FACE_O_M_SMA0";
                    else
                        return "FACE_O_F_SMA0";
                case 33:
                    if (bParam1)
                        return "FACE_O_M_SMA1";
                    else
                        return "FACE_O_F_SMA1";
                case 34:
                    if (bParam1)
                        return "FACE_O_M_SMA2";
                    else
                        return "FACE_O_F_SMA2";
                case 35:
                    if (bParam1)
                        return "FACE_O_M_SMA3";
                    else
                        return "FACE_O_F_SMA3";
                case 36:
                    if (bParam1)
                        return "FACE_O_M_SMA4";
                    else
                        return "FACE_O_F_SMA4";
                case 37:
                    if (bParam1)
                        return "FACE_O_M_SMA5";
                    else
                        return "FACE_O_F_SMA5";
                case 38:
                    if (bParam1)
                        return "FACE_O_M_SMA6";
                    else
                        return "FACE_O_F_SMA6";
                case 39:
                    if (bParam1)
                        return "FACE_O_M_SMA7";
                    else
                        return "FACE_O_F_SMA7";
                case 40:
                    if (bParam1)
                        return "FACE_O_M_SPO0";
                    else
                        return "FACE_O_F_SPO0";
                case 41:
                    if (bParam1)
                        return "FACE_O_M_SPO1";
                    else
                        return "FACE_O_F_SPO1";
                case 42:
                    if (bParam1)
                        return "FACE_O_M_SPO2";
                    else
                        return "FACE_O_F_SPO2";
                case 43:
                    if (bParam1)
                        return "FACE_O_M_SPO3";
                    else
                        return "FACE_O_F_SPO3";
                case 44:
                    if (bParam1)
                        return "FACE_O_M_SPO4";
                    else
                        return "FACE_O_F_SPO4";
                case 45:
                    if (bParam1)
                        return "FACE_O_M_SPO5";
                    else
                        return "FACE_O_F_SPO5";
                case 46:
                    if (bParam1)
                        return "FACE_O_M_SPO6";
                    else
                        return "FACE_O_F_SPO6";
                case 47:
                    if (bParam1)
                        return "FACE_O_M_SPO7";
                    else
                        return "FACE_O_F_SPO7";
                case 48:
                    if (bParam1)
                        return "FACE_O_M_ECC0";
                    else
                        return "FACE_O_F_ECC0";
                case 49:
                    if (bParam1)
                        return "FACE_O_M_ECC1";
                    else
                        return "FACE_O_F_ECC1";
                case 50:
                    if (bParam1)
                        return "FACE_O_M_ECC2";
                    else
                        return "FACE_O_F_ECC2";
                case 51:
                    if (bParam1)
                        return "FACE_O_M_ECC3";
                    else
                        return "FACE_O_F_ECC3";
                case 52:
                    if (bParam1)
                        return "FACE_O_M_ECC4";
                    else
                        return "FACE_O_F_ECC4";
                case 53:
                    if (bParam1)
                        return "FACE_O_M_ECC5";
                    else
                        return "FACE_O_F_ECC5";
                case 54:
                    if (bParam1)
                        return "FACE_O_M_ECC6";
                    else
                        return "FACE_O_F_ECC6";
                case 55:
                    if (bParam1)
                        return "FACE_O_M_ECC7";
                    else
                        return "FACE_O_F_ECC7";
                case 56:
                    if (bParam1)
                        return "FACE_O_M_CAS0";
                    else
                        return "FACE_O_F_CAS0";
                case 57:
                    if (bParam1)
                        return "FACE_O_M_CAS1";
                    else
                        return "FACE_O_F_CAS1";
                case 58:
                    if (bParam1)
                        return "FACE_O_M_CAS2";
                    else
                        return "FACE_O_F_CAS2";
                case 59:
                    if (bParam1)
                        return "FACE_O_M_CAS3";
                    else
                        return "FACE_O_F_CAS4";
                case 60:
                    if (bParam1)
                        return "FACE_O_M_CAS4";
                    else
                        return "FACE_O_F_CAS3";
                case 61:
                    if (bParam1)
                        return "FACE_O_M_CAS5";
                    else
                        return "FACE_O_F_CAS5";
                case 62:
                    if (bParam1)
                        return "FACE_O_M_CAS6";
                    else
                        return "FACE_O_F_CAS6";
                case 63:
                    if (bParam1)
                        return "FACE_O_M_CAS7";
                    else
                        return "FACE_O_F_CAS7";
            }
            return "";
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