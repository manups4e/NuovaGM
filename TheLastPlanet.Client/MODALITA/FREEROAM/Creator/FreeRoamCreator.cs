using FxEvents.Shared.Snowflakes;
using Impostazioni.Client.Configurazione.Negozi.Abiti;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Ingresso;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.CharCreation
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

        private static List<ShopPed.PedComponentData> MaleHats = new List<ShopPed.PedComponentData>()
        {
            new (0,-1257185072,63,20,0,0,0,0,3,"CLO_BBM_H_0_0"),
            new (0,-75666008,63,20,1,0,0,0,3,"CLO_BBM_H_0_1"),
            new (0,-451657514,63,20,2,0,0,0,3,"CLO_BBM_H_0_2"),
            new (0,-1339992339,63,20,3,0,0,0,3,"CLO_BBM_H_0_3"),
            new (0,-1987638855,63,20,4,0,0,0,3,"CLO_BBM_H_0_4"),
            new (0,1148780446,63,20,5,0,0,0,3,"CLO_BBM_H_0_5"),
            new (0,-1633579233,69,21,0,0,0,0,3,"CLO_BBM_H_1_0"),
            new (0,-1875283381,69,21,1,0,0,0,3,"CLO_BBM_H_1_1"),
            new (0,-2112956938,69,21,2,0,0,0,3,"CLO_BBM_H_1_2"),
            new (0,1941813588,69,21,3,0,0,0,3,"CLO_BBM_H_1_3"),
            new (0,-175555347,69,21,4,0,0,0,3,"CLO_BBM_H_1_4"),
            new (0,-952049571,69,21,5,0,0,0,3,"CLO_BBM_H_1_5"),
            new (0,-1193131104,69,21,6,0,0,0,3,"CLO_BBM_H_1_6"),
            new (0,-1435228476,69,21,7,0,0,0,3,"CLO_BBM_H_1_7"),
            new (1498456532,1961752738,70,25,0,3950,0,0,3,"CLO_VALM_H_0_0"),
            new (1498456532,-2021286443,70,25,1,4115,0,0,3,"CLO_VALM_H_0_1"),
            new (1498456532,1082888158,70,25,2,3600,0,0,3,"CLO_VALM_H_0_2"),
            new (0,-214003082,68,26,0,3500,0,0,3,"CLO_BUS_P_0_0"),
            new (0,97007497,68,26,1,3515,0,0,3,"CLO_BUS_P_0_1"),
            new (0,246106447,68,26,2,3530,0,0,3,"CLO_BUS_P_0_2"),
            new (0,560852692,68,26,3,3545,0,0,3,"CLO_BUS_P_0_3"),
            new (0,-1438449536,68,26,4,3560,0,0,3,"CLO_BUS_P_0_4"),
            new (0,-1138580417,68,26,5,3575,0,0,3,"CLO_BUS_P_0_5"),
            new (0,-1431961294,68,26,6,3590,0,0,3,"CLO_BUS_P_0_6"),
            new (0,-1126849135,68,26,7,3605,0,0,3,"CLO_BUS_P_0_7"),
            new (0,-2062567910,68,26,8,3620,0,0,3,"CLO_BUS_P_0_8"),
            new (0,-1725279696,68,27,0,5000,0,0,3,"CLO_BUS_P_1_0"),
            new (0,1664968271,68,27,1,5015,0,0,3,"CLO_BUS_P_1_1"),
            new (0,1973979949,68,27,2,5030,0,0,3,"CLO_BUS_P_1_2"),
            new (0,-776912067,68,27,3,5045,0,0,3,"CLO_BUS_P_1_3"),
            new (0,-472651902,68,27,4,5060,0,0,3,"CLO_BUS_P_1_4"),
            new (0,-1440812007,68,27,5,5075,0,0,3,"CLO_BUS_P_1_5"),
            new (0,-1130620653,68,27,6,5090,0,0,3,"CLO_BUS_P_1_6"),
            new (0,419221971,68,27,7,5105,0,0,3,"CLO_BUS_P_1_7"),
            new (0,725710428,68,27,8,5120,0,0,3,"CLO_BUS_P_1_8"),
            new (0,-1461972223,62,28,0,9999,0,0,3,"CLO_HP_H_0_0"),
            new (0,1616380410,62,28,1,9999,0,0,3,"CLO_HP_H_0_1"),
            new (0,1987915332,62,28,2,9999,0,0,3,"CLO_HP_H_0_2"),
            new (0,1269061779,62,28,3,9999,0,0,3,"CLO_HP_H_0_3"),
            new (0,1373660427,62,28,4,9999,0,0,3,"CLO_HP_H_0_4"),
            new (0,-314795075,62,28,5,9999,0,0,3,"CLO_HP_H_0_5"),
            new (0,-2013625452,68,29,0,9999,0,0,3,"CLO_HP_H_1_0"),
            new (0,-818048515,68,29,1,9999,0,0,3,"CLO_HP_H_1_1"),
            new (0,-453526159,68,29,2,9999,0,0,3,"CLO_HP_H_1_2"),
            new (0,-297480181,68,29,3,9999,0,0,3,"CLO_HP_H_1_3"),
            new (0,76217495,68,29,4,9999,0,0,3,"CLO_HP_H_1_4"),
            new (0,197167874,68,29,5,9999,0,0,3,"CLO_HP_H_1_5"),
            new (0,572372924,68,29,6,9999,0,0,3,"CLO_HP_H_1_6"),
            new (-1115780512,-726329981,64,55,0,1260,0,0,3,"CLO_S1M_PH_0_0"),
            new (-1115780512,-908656697,64,55,1,1265,0,0,3,"CLO_S1M_PH_0_1"),
            new (-1115780512,-1220060504,64,55,2,1270,0,0,3,"CLO_S1M_PH_0_2")
        };
        private static List<ShopPed.PedComponentData> FemaleHats = new List<ShopPed.PedComponentData>()
            {
                new(0,1720281330,71,20,0,0,0,0,4,"CLO_BBF_P2_0"),
                new(0,968593239,71,20,1,0,0,0,4,"CLO_BBF_P2_1"),
                new(0,-479862111,71,20,2,0,0,0,4,"CLO_BBF_P2_2"),
                new(0,-1254685116,71,20,3,0,0,0,4,"CLO_BBF_P2_3"),
                new(0,-990861897,71,20,4,0,0,0,4,"CLO_BBF_P2_4"),
                new(0,421482003,71,20,5,0,0,0,4,"CLO_BBF_P2_5"),
                new(0,-1735340808,71,20,6,0,0,0,4,"CLO_BBF_P2_6"),
                new(0,2003858863,63,21,0,0,0,0,4,"CLO_BBF_P3_0"),
                new(0,1697239330,63,21,1,0,0,0,4,"CLO_BBF_P3_1"),
                new(0,1093765426,63,21,2,0,0,0,4,"CLO_BBF_P3_2"),
                new(0,-1344510330,63,21,3,0,0,0,4,"CLO_BBF_P3_3"),
                new(0,-1636580427,63,21,4,0,0,0,4,"CLO_BBF_P3_4"),
                new(0,-1684357629,63,21,5,0,0,0,4,"CLO_BBF_P3_5"),
                new(0,-2010147027,63,21,6,0,0,0,4,"CLO_BBF_P3_6"),
                new(0,607828228,63,22,0,0,0,0,4,"CLO_BBF_P4_0"),
                new(0,1577266424,63,22,1,0,0,0,4,"CLO_BBF_P4_1"),
                new(0,1132328842,63,22,2,0,0,0,4,"CLO_BBF_P4_2"),
                new(0,-344209529,63,22,3,0,0,0,4,"CLO_BBF_P4_3"),
                new(0,-575034365,63,22,4,0,0,0,4,"CLO_BBF_P4_4"),
                new(0,338532586,63,22,5,0,0,0,4,"CLO_BBF_P4_5"),
                new(0,-92904068,63,22,6,0,0,0,4,"CLO_BBF_P4_6"),
                new(0,-341686545,68,26,0,3500,0,0,4,"CLO_BUS_P_0_0"),
                new(0,-568448021,68,26,1,3515,0,0,4,"CLO_BUS_P_0_1"),
                new(0,-204318893,68,26,2,3530,0,0,4,"CLO_BUS_P_0_2"),
                new(0,44037358,68,26,3,3545,0,0,4,"CLO_BUS_P_0_3"),
                new(0,272076825,68,26,4,3560,0,0,4,"CLO_BUS_P_0_4"),
                new(0,653114757,68,26,5,3575,0,0,4,"CLO_BUS_P_0_5"),
                new(0,883841286,68,26,6,3590,0,0,4,"CLO_BUS_P_0_6"),
                new(0,1268614884,68,26,7,3605,0,0,4,"CLO_BUS_P_0_7"),
                new(0,1499538027,68,26,8,3620,0,0,4,"CLO_BUS_P_0_8"),
                new(0,-1197753210,68,27,0,5000,0,0,4,"CLO_BUS_P_1_0"),
                new(0,-1337283612,68,27,1,5015,0,0,4,"CLO_BUS_P_1_1"),
                new(0,-730074042,68,27,2,5030,0,0,4,"CLO_BUS_P_1_2"),
                new(0,-1028304711,68,27,3,5045,0,0,4,"CLO_BUS_P_1_3"),
                new(0,-1188348515,68,27,4,5060,0,0,4,"CLO_BUS_P_1_4"),
                new(0,-1495295738,68,27,5,5075,0,0,4,"CLO_BUS_P_1_5"),
                new(0,-678528413,68,27,6,5090,0,0,4,"CLO_BUS_P_1_6"),
                new(0,-1102297121,68,27,7,5105,0,0,4,"CLO_BUS_P_1_7"),
                new(0,1810965290,68,27,8,5120,0,0,4,"CLO_BUS_P_1_8"),
                new(0,760877684,70,28,0,9999,0,0,4,"CLO_HP_F_H_0_0"),
                new(0,-1618381103,70,28,1,9999,0,0,4,"CLO_HP_F_H_0_1"),
                new(0,-2000074415,70,28,2,9999,0,0,4,"CLO_HP_F_H_0_2"),
                new(0,2064297428,70,28,3,9999,0,0,4,"CLO_HP_F_H_0_3"),
                new(0,-82235921,70,28,4,9999,0,0,4,"CLO_HP_F_H_0_4"),
                new(0,-464322461,70,28,5,9999,0,0,4,"CLO_HP_F_H_0_5"),
                new(0,1450206368,70,28,6,9999,0,0,4,"CLO_HP_F_H_0_6"),
                new(0,1135689506,70,28,7,9999,0,0,4,"CLO_HP_F_H_0_7"),
                new(0,1608505817,62,29,0,9999,0,0,4,"CLO_HP_F_H_1_0"),
                new(0,1832973467,62,29,1,9999,0,0,4,"CLO_HP_F_H_1_1"),
                new(0,993136766,62,29,2,9999,0,0,4,"CLO_HP_F_H_1_2"),
                new(0,1308866081,62,29,3,9999,0,0,4,"CLO_HP_F_H_1_3"),
            };

        private static List<ShopPed.PedComponentData> MaleGlasses = new List<ShopPed.PedComponentData>()
            {
                new(0,-729343262,81,16,0,0,1,0,3,"CLO_BBM_E_0_0"),
                new(0,-489343106,81,16,1,0,1,0,3,"CLO_BBM_E_0_1"),
                new(0,-820867051,81,16,2,0,1,0,3,"CLO_BBM_E_0_2"),
                new(0,-557994133,81,16,3,0,1,0,3,"CLO_BBM_E_0_3"),
                new(0,-328316212,81,16,4,0,1,0,3,"CLO_BBM_E_0_4"),
                new(0,-97491376,81,16,5,0,1,0,3,"CLO_BBM_E_0_5"),
                new(0,136708667,81,16,6,0,1,0,3,"CLO_BBM_E_0_6"),
                new(0,869620121,81,16,7,935,1,0,3,"CLO_EXM_G_16_7"),
                new(0,1099724039,81,16,8,550,1,0,3,"CLO_EXM_G_16_8"),
                new(0,1356862354,81,16,9,615,1,0,3,"CLO_EXM_G_16_9"),
                new(0,-1465678599,85,17,0,650,1,0,3,"CLO_BUSM_G0_0"),
                new(0,-712712517,85,17,1,690,1,0,3,"CLO_BUSM_G0_1"),
                new(0,-4836579,85,17,2,515,1,0,3,"CLO_BUSM_G0_2"),
                new(0,359816857,85,17,3,520,1,0,3,"CLO_BUSM_G0_3"),
                new(0,1071756155,85,17,4,525,1,0,3,"CLO_BUSM_G0_4"),
                new(0,822023606,85,17,5,780,1,0,3,"CLO_BUSM_G0_5"),
                new(0,1531275842,85,17,6,535,1,0,3,"CLO_BUSM_G0_6"),
                new(0,1282133135,85,17,7,540,1,0,3,"CLO_BUSM_G0_7"),
                new(0,1988042933,85,17,8,3180,1,0,3,"CLO_EXM_G_17_8"),
                new(0,-1549239545,85,17,9,2795,1,0,3,"CLO_EXM_G_17_9"),
                new(0,-16367277,85,17,10,2860,1,0,3,"CLO_EXM_G_17_10"),
                new(0,1941866665,85,18,0,650,1,0,3,"CLO_BUSM_G1_0"),
                new(0,-2055918566,85,18,1,635,1,0,3,"CLO_BUSM_G1_1"),
                new(0,1941670055,85,18,2,850,1,0,3,"CLO_BUSM_G1_2"),
                new(0,1646552433,85,18,3,675,1,0,3,"CLO_BUSM_G1_3"),
                new(0,1348583916,85,18,4,930,1,0,3,"CLO_BUSM_G1_4"),
                new(0,1330757580,85,18,5,1000,1,0,3,"CLO_BUSM_G1_5"),
                new(0,1019484849,85,18,6,620,1,0,3,"CLO_BUSM_G1_6"),
                new(0,667873479,85,18,7,600,1,0,3,"CLO_BUSM_G1_7"),
                new(0,421024602,85,18,8,1250,1,0,3,"CLO_EXM_G_18_8"),
                new(0,278970991,85,18,9,865,1,0,3,"CLO_EXM_G_18_9"),
                new(0,1761624451,85,18,10,930,1,0,3,"CLO_EXM_G_18_10"),
                new(0,860255687,84,19,0,9999,1,0,3,"CLO_HP_G_0_0"),
                new(0,-99908782,84,19,1,9999,1,0,3,"CLO_HP_G_0_1"),
                new(0,382778588,84,19,2,9999,1,0,3,"CLO_HP_G_0_2"),
                new(0,1990786139,84,19,3,9999,1,0,3,"CLO_HP_G_0_3"),
                new(0,1516618709,84,19,4,9999,1,0,3,"CLO_HP_G_0_4"),
                new(0,1260135746,84,19,5,9999,1,0,3,"CLO_HP_G_0_5"),
                new(0,1021446350,84,19,6,9999,1,0,3,"CLO_HP_G_0_6"),
                new(0,564515410,84,19,7,9999,1,0,3,"CLO_HP_G_0_7"),
                new(0,333461191,84,19,8,2845,1,0,3,"CLO_EXM_G_19_8"),
                new(0,103881577,84,19,9,2460,1,0,3,"CLO_EXM_G_19_9"),
                new(0,611661108,84,19,10,2525,1,0,3,"CLO_EXM_G_19_10"),
                new(0,-1991976427,84,20,0,9999,1,0,3,"CLO_HP_G_1_0"),
                new(0,-1727628904,84,20,1,9999,1,0,3,"CLO_HP_G_1_1"),
                new(0,1722750186,84,20,2,9999,1,0,3,"CLO_HP_G_1_2"),
                new(0,1953345639,84,20,3,9999,1,0,3,"CLO_HP_G_1_3"),
                new(0,1346201607,84,20,4,9999,1,0,3,"CLO_HP_G_1_4"),
                new(0,-402548847,84,20,5,9999,1,0,3,"CLO_HP_G_1_5"),
                new(0,764519088,84,20,6,9999,1,0,3,"CLO_HP_G_1_6"),
                new(0,1129041444,84,20,7,9999,1,0,3,"CLO_HP_G_1_7"),
                new(0,-1623489018,84,20,8,1390,1,0,3,"CLO_EXM_G_20_8"),
                new(0,-1357503045,84,20,9,1005,1,0,3,"CLO_EXM_G_20_9"),
                new(0,-1432441645,84,20,10,1070,1,0,3,"CLO_EXM_G_20_10"),
                new(1320779089,-391281267,82,21,0,1500,1,0,3,"CLO_INDM_G_0_0"),
                new(1320779089,-1080701017,82,22,0,1990,1,0,3,"CLO_INDM_G_1_0"),
                new(1320214525,-554175663,81,24,0,0,1,0,3,"CLO_BIM_PE_0_0"),
                new(1320214525,1608086806,81,24,1,0,1,0,3,"CLO_BIM_PE_0_1"),
                new(1320214525,-97932876,81,24,2,0,1,0,3,"CLO_BIM_PE_0_2"),
                new(1320214525,-360904101,81,24,3,0,1,0,3,"CLO_BIM_PE_0_3"),
                new(1320214525,480702126,81,24,4,0,1,0,3,"CLO_BIM_PE_0_4"),
                new(1320214525,1124449147,81,24,5,0,1,0,3,"CLO_BIM_PE_0_5"),
                new(1320214525,-1699932676,81,25,0,0,1,0,3,"CLO_BIM_PE_1_0"),
                new(1320214525,-1964280199,81,25,1,0,1,0,3,"CLO_BIM_PE_1_1"),
                new(1320214525,2032784118,81,25,2,0,1,0,3,"CLO_BIM_PE_1_2"),
                new(1320214525,1830796002,81,25,3,0,1,0,3,"CLO_BIM_PE_1_3"),
                new(1320214525,1396672290,81,25,4,0,1,0,3,"CLO_BIM_PE_1_4"),
                new(1320214525,1099555767,81,25,5,0,1,0,3,"CLO_BIM_PE_1_5"),
                new(1320214525,903007305,81,25,6,0,1,0,3,"CLO_BIM_PE_1_6"),
                new(1320214525,967693315,81,25,7,0,1,0,3,"CLO_BIM_PE_1_7"),
                new(867164096,-1253464949,163,28,0,0,1,0,3,"CLO_VWM_PEY_0_0"),
                new(867164096,2133080081,163,28,1,0,1,0,3,"CLO_VWM_PEY_0_1"),
                new(867164096,1902156938,163,28,2,0,1,0,3,"CLO_VWM_PEY_0_2"),
                new(867164096,283794379,163,28,3,0,1,0,3,"CLO_VWM_PEY_0_3"),
                new(867164096,46317436,163,28,4,0,1,0,3,"CLO_VWM_PEY_0_4"),
                new(867164096,-1402727744,163,28,5,0,1,0,3,"CLO_VWM_PEY_0_5"),
                new(867164096,468873691,163,28,6,0,1,0,3,"CLO_VWM_PEY_0_6"),
                new(867164096,898704664,163,28,7,0,1,0,3,"CLO_VWM_PEY_0_7"),
                new(867164096,659556502,163,28,8,0,1,0,3,"CLO_VWM_PEY_0_8"),
                new(867164096,-249357251,163,28,9,0,1,0,3,"CLO_VWM_PEY_0_9"),
                new(867164096,2065673869,163,28,10,0,1,0,3,"CLO_VWM_PEY_010"),
                new(867164096,1691976193,163,28,11,0,1,0,3,"CLO_VWM_PEY_011"),
                new(867164096,1611498483,163,29,0,0,1,0,3,"CLO_VWM_PEY_1_0"),
                new(867164096,-1974478729,163,29,1,0,1,0,3,"CLO_VWM_PEY_1_1"),
                new(867164096,1154075964,163,29,2,0,1,0,3,"CLO_VWM_PEY_1_2"),
                new(867164096,1907959626,163,29,3,0,1,0,3,"CLO_VWM_PEY_1_3"),
                new(867164096,694031973,163,29,4,0,1,0,3,"CLO_VWM_PEY_1_4"),
                new(867164096,1390045533,163,29,5,0,1,0,3,"CLO_VWM_PEY_1_5"),
                new(867164096,171726882,163,29,6,0,1,0,3,"CLO_VWM_PEY_1_6"),
                new(867164096,1015954629,163,29,7,0,1,0,3,"CLO_VWM_PEY_1_7"),
                new(867164096,-238377153,163,29,8,0,1,0,3,"CLO_VWM_PEY_1_8"),
                new(867164096,469826475,163,29,9,0,1,0,3,"CLO_VWM_PEY_1_9"),
                new(867164096,1698971674,163,29,10,0,1,0,3,"CLO_VWM_PEY_110"),
                new(867164096,2005591207,163,29,11,0,1,0,3,"CLO_VWM_PEY_111"),
                new(2141921740,-2133234397,246,30,0,0,1,0,3,"CLO_H4M_PEY_0_0"),
                new(2141921740,1863698844,246,30,1,0,1,0,3,"CLO_H4M_PEY_0_1"),
                new(2141921740,330175182,246,30,2,0,1,0,3,"CLO_H4M_PEY_0_2"),
                new(2141921740,1229553156,246,30,3,0,1,0,3,"CLO_H4M_PEY_0_3"),
                new(2141921740,758400474,246,30,4,0,1,0,3,"CLO_H4M_PEY_0_4"),
                new(2141921740,-623173331,246,30,5,0,1,0,3,"CLO_H4M_PEY_0_5"),
                new(2141921740,-894664496,246,30,6,0,1,0,3,"CLO_H4M_PEY_0_6"),
                new(2141921740,17690002,246,30,7,0,1,0,3,"CLO_H4M_PEY_0_7"),
                new(2141921740,-265532465,246,30,8,0,1,0,3,"CLO_H4M_PEY_0_8"),
                new(2141921740,-1779198113,246,30,9,0,1,0,3,"CLO_H4M_PEY_0_9"),
                new(2141921740,357509846,246,30,10,0,1,0,3,"CLO_H4M_PEY_010"),
                new(2141921740,636341267,246,30,11,0,1,0,3,"CLO_H4M_PEY_011"),
                new(2141921740,-36224791,164,31,0,0,1,0,3,"CLO_H4M_PEY_1_0"),
                new(2141921740,-1139163793,164,31,1,0,1,0,3,"CLO_H4M_PEY_1_1"),
                new(2141921740,-1842419302,164,31,2,0,1,0,3,"CLO_H4M_PEY_1_2"),
                new(2141921740,2121581094,164,31,3,0,1,0,3,"CLO_H4M_PEY_1_3"),
                new(2141921740,1957801632,164,31,4,0,1,0,3,"CLO_H4M_PEY_1_4"),
                new(2141921740,1660947261,164,31,5,0,1,0,3,"CLO_H4M_PEY_1_5"),
                new(2141921740,997276700,164,31,6,0,1,0,3,"CLO_H4M_PEY_1_6"),
                new(2141921740,724278161,164,31,7,0,1,0,3,"CLO_H4M_PEY_1_7"),
                new(2141921740,-1880201963,164,31,8,0,1,0,3,"CLO_H4M_PEY_1_8"),
                new(2141921740,2108670104,164,31,9,0,1,0,3,"CLO_H4M_PEY_1_9"),
                new(2141921740,1149517553,164,31,10,0,1,0,3,"CLO_H4M_PEY_110"),
                new(2141921740,-1842521530,164,31,11,0,1,0,3,"CLO_H4M_PEY_111"),
                new(2141921740,-1661989803,164,32,0,0,1,0,3,"CLO_H4M_PEY_2_0"),
                new(2141921740,315029509,164,32,1,0,1,0,3,"CLO_H4M_PEY_2_1"),
                new(2141921740,76045192,164,32,2,0,1,0,3,"CLO_H4M_PEY_2_2"),
                new(2141921740,-231524642,164,32,3,0,1,0,3,"CLO_H4M_PEY_2_3"),
                new(2141921740,-538078637,164,32,4,0,1,0,3,"CLO_H4M_PEY_2_4"),
                new(2141921740,1280600755,164,32,5,0,1,0,3,"CLO_H4M_PEY_2_5"),
                new(2141921740,1050267454,164,32,6,0,1,0,3,"CLO_H4M_PEY_2_6"),
                new(2141921740,686728168,164,32,7,0,1,0,3,"CLO_H4M_PEY_2_7"),
                new(2141921740,388825189,164,32,8,0,1,0,3,"CLO_H4M_PEY_2_8"),
                new(2141921740,-1765539843,164,32,9,0,1,0,3,"CLO_H4M_PEY_2_9"),
                new(2141921740,-332552075,164,32,10,0,1,0,3,"CLO_H4M_PEY_210"),
                new(2141921740,-624917093,164,32,11,0,1,0,3,"CLO_H4M_PEY_211"),
                new(2141921740,-1463507504,164,33,0,0,1,0,3,"CLO_H4M_PEY_3_0"),
                new(2141921740,-744686720,164,33,1,0,1,0,3,"CLO_H4M_PEY_3_1"),
                new(2141921740,-982294739,164,33,2,0,1,0,3,"CLO_H4M_PEY_3_2"),
                new(2141921740,1101354891,164,33,3,0,1,0,3,"CLO_H4M_PEY_3_3"),
                new(2141921740,-352998867,164,33,4,0,1,0,3,"CLO_H4M_PEY_3_4"),
                new(2141921740,-629143230,164,33,5,0,1,0,3,"CLO_H4M_PEY_3_5"),
                new(2141921740,214625751,164,33,6,0,1,0,3,"CLO_H4M_PEY_3_6"),
                new(2141921740,1809853456,164,33,7,0,1,0,3,"CLO_H4M_PEY_3_7"),
                new(2141921740,1473119212,164,33,8,0,1,0,3,"CLO_H4M_PEY_3_8"),
                new(2141921740,-1988237493,164,33,9,0,1,0,3,"CLO_H4M_PEY_3_9"),
                new(2141921740,-308780352,164,33,10,0,1,0,3,"CLO_H4M_PEY_310"),
                new(2141921740,-539048115,164,33,11,0,1,0,3,"CLO_H4M_PEY_311")
            };
        private static List<ShopPed.PedComponentData> FemaleGlasses = new List<ShopPed.PedComponentData>()
        {
            new(0,-723493963,84,16,0,0,1,0,4,"CLO_BBF_P0_0"),
            new(0,-500992453,84,16,1,0,1,0,4,"CLO_BBF_P0_1"),
            new(0,-1739758960,84,16,2,0,1,0,4,"CLO_BBF_P0_2"),
            new(0,-980697844,84,16,3,0,1,0,4,"CLO_BBF_P0_3"),
            new(0,2034246774,84,16,4,0,1,0,4,"CLO_BBF_P0_4"),
            new(0,-2017083007,84,16,5,0,1,0,4,"CLO_BBF_P0_5"),
            new(0,1620439838,84,16,6,0,1,0,4,"CLO_BBF_P0_6"),
            new(0,1994753042,84,17,0,0,1,0,4,"CLO_BBF_P1_0"),
            new(0,303093168,84,17,1,0,1,0,4,"CLO_BBF_P1_1"),
            new(0,571438509,84,17,2,0,1,0,4,"CLO_BBF_P1_2"),
            new(0,776834589,84,17,3,0,1,0,4,"CLO_BBF_P1_3"),
            new(0,957326241,84,17,4,0,1,0,4,"CLO_BBF_P1_4"),
            new(0,1255294758,84,17,5,0,1,0,4,"CLO_BBF_P1_5"),
            new(0,1524426555,84,17,6,0,1,0,4,"CLO_BBF_P1_6"),
            new(0,-1901945713,85,18,0,350,1,0,4,"CLO_BUSF_G0_0"),
            new(0,-1679968507,85,18,1,345,1,0,4,"CLO_BUSF_G0_1"),
            new(0,959869364,85,18,2,380,1,0,4,"CLO_BUSF_G0_2"),
            new(0,1257116963,85,18,3,400,1,0,4,"CLO_BUSF_G0_3"),
            new(0,1705331345,85,18,4,395,1,0,4,"CLO_BUSF_G0_4"),
            new(0,2009984738,85,18,5,380,1,0,4,"CLO_BUSF_G0_5"),
            new(0,-211753462,85,18,6,595,1,0,4,"CLO_BUSF_G0_6"),
            new(0,235084622,85,18,7,550,1,0,4,"CLO_BUSF_G0_7"),
            new(0,2129325700,85,19,0,360,1,0,4,"CLO_BUSF_G1_0"),
            new(0,1828670121,85,19,1,350,1,0,4,"CLO_BUSF_G1_1"),
            new(0,2065360608,85,19,2,345,1,0,4,"CLO_BUSF_G1_2"),
            new(0,-1976859387,85,19,3,395,1,0,4,"CLO_BUSF_G1_3"),
            new(0,-1536312951,85,19,4,400,1,0,4,"CLO_BUSF_G1_4"),
            new(0,-1243128708,85,19,5,590,1,0,4,"CLO_BUSF_G1_5"),
            new(0,-1052478666,85,19,6,410,1,0,4,"CLO_BUSF_G1_6"),
            new(0,-746383437,85,19,7,780,1,0,4,"CLO_BUSF_G1_7"),
            new(0,-322745805,85,19,8,2845,1,0,4,"CLO_EXF_G_19_8"),
            new(0,266244209,85,19,9,2460,1,0,4,"CLO_EXF_G_19_9"),
            new(0,-284371143,85,19,10,2525,1,0,4,"CLO_EXF_G_19_10"),
            new(0,187575916,84,20,0,9999,1,0,4,"CLO_HP_F_G_0_0"),
            new(0,-418716130,84,20,1,9999,1,0,4,"CLO_HP_F_G_0_1"),
            new(0,2043153306,84,20,2,9999,1,0,4,"CLO_HP_F_G_0_2"),
            new(0,-1067771713,84,20,3,9999,1,0,4,"CLO_HP_F_G_0_3"),
            new(0,-770393038,84,20,4,9999,1,0,4,"CLO_HP_F_G_0_4"),
            new(0,-1374456784,84,20,5,9999,1,0,4,"CLO_HP_F_G_0_5"),
            new(0,1086101892,84,20,6,9999,1,0,4,"CLO_HP_F_G_0_6"),
            new(0,-1969115827,84,20,7,9999,1,0,4,"CLO_HP_F_G_0_7"),
            new(0,-1200758446,84,21,0,9999,1,0,4,"CLO_HP_F_G_1_0"),
            new(0,-1413199873,84,21,1,9999,1,0,4,"CLO_HP_F_G_1_1"),
            new(0,-1813112749,84,21,2,9999,1,0,4,"CLO_HP_F_G_1_2"),
            new(0,-1528186290,84,21,3,9999,1,0,4,"CLO_HP_F_G_1_3"),
            new(0,-1756160223,84,21,4,9999,1,0,4,"CLO_HP_F_G_1_4"),
            new(0,-2107312827,84,21,5,9999,1,0,4,"CLO_HP_F_G_1_5"),
            new(0,1926845998,84,21,6,9999,1,0,4,"CLO_HP_F_G_1_6"),
            new(0,1555966456,84,21,7,9999,1,0,4,"CLO_HP_F_G_1_7"),
            new(1320779089,534992240,82,22,0,1500,1,0,4,"CLO_INDF_G_0_0"),
            new(1320779089,-1207676099,82,23,0,1990,1,0,4,"CLO_INDF_G_1_0"),
            new(0,756477607,82,24,0,180,1,0,4,"CLO_L2F_G_0_0"),
            new(0,1051922895,82,24,1,500,1,0,4,"CLO_L2F_G_0_1"),
            new(0,1408056387,82,24,2,115,1,0,4,"CLO_L2F_G_0_2"),
            new(0,1530317526,82,24,3,220,1,0,4,"CLO_L2F_G_0_3"),
            new(0,1886909784,82,24,4,185,1,0,4,"CLO_L2F_G_0_4"),
            new(0,-142113927,82,24,5,1445,1,0,4,"CLO_L2F_G_0_5"),
            new(0,231059445,82,24,6,530,1,0,4,"CLO_L2F_G_0_6"),
            new(0,-740901860,82,24,7,650,1,0,4,"CLO_L2F_G_0_7"),
            new(0,574609641,82,24,8,1390,1,0,4,"CLO_EXF_G_24_8"),
            new(0,-201458582,82,24,9,1005,1,0,4,"CLO_EXF_G_24_9"),
            new(0,-1800817877,82,24,10,1070,1,0,4,"CLO_EXF_G_24_10"),
            new(1320214525,-314372800,81,26,0,0,1,0,4,"CLO_BIF_PE_0_0"),
            new(1320214525,-83187505,81,26,1,0,1,0,4,"CLO_BIF_PE_0_1"),
            new(1320214525,1354257449,81,26,2,0,1,0,4,"CLO_BIF_PE_0_2"),
            new(1320214525,1614345002,81,26,3,0,1,0,4,"CLO_BIF_PE_0_3"),
            new(1320214525,904568462,81,26,4,0,1,0,4,"CLO_BIF_PE_0_4"),
            new(1320214525,1136409137,81,26,5,0,1,0,4,"CLO_BIF_PE_0_5"),
            new(1320214525,-788523881,81,27,0,0,1,0,4,"CLO_BIF_PE_1_0"),
            new(1320214525,894360887,81,27,1,0,1,0,4,"CLO_BIF_PE_1_1"),
            new(1320214525,1595158721,81,27,2,0,1,0,4,"CLO_BIF_PE_1_2"),
            new(1320214525,164234798,81,27,3,0,1,0,4,"CLO_BIF_PE_1_3"),
            new(1320214525,1134197198,81,27,4,0,1,0,4,"CLO_BIF_PE_1_4"),
            new(1320214525,-1743772996,81,27,5,0,1,0,4,"CLO_BIF_PE_1_5"),
            new(1320214525,-1984199149,81,27,6,0,1,0,4,"CLO_BIF_PE_1_6"),
            new(1320214525,2088725399,81,27,7,0,1,0,4,"CLO_BIF_PE_1_7"),
            new(867164096,-2114381102,163,30,0,0,1,0,4,"CLO_VWF_PEY_0_0"),
            new(867164096,-1816150433,163,30,1,0,1,0,4,"CLO_VWF_PEY_0_1"),
            new(867164096,-893014934,163,30,2,0,1,0,4,"CLO_VWF_PEY_0_2"),
            new(867164096,-259196936,163,30,3,0,1,0,4,"CLO_VWF_PEY_0_3"),
            new(867164096,-780420710,163,30,4,0,1,0,4,"CLO_VWF_PEY_0_4"),
            new(867164096,-674970068,163,30,5,0,1,0,4,"CLO_VWF_PEY_0_5"),
            new(867164096,812382133,163,30,6,0,1,0,4,"CLO_VWF_PEY_0_6"),
            new(867164096,-1021895471,163,30,7,0,1,0,4,"CLO_VWF_PEY_0_7"),
            new(867164096,-118159160,163,30,8,0,1,0,4,"CLO_VWF_PEY_0_8"),
            new(867164096,179022901,163,30,9,0,1,0,4,"CLO_VWF_PEY_0_9"),
            new(867164096,484987416,163,30,10,0,1,0,4,"CLO_VWF_PEY_010"),
            new(867164096,240301293,163,30,11,0,1,0,4,"CLO_VWF_PEY_011"),
            new(867164096,2007194744,163,31,0,0,1,0,4,"CLO_VWF_PEY_1_0"),
            new(867164096,-1534020014,163,31,1,0,1,0,4,"CLO_VWF_PEY_1_1"),
            new(867164096,-1828252865,163,31,2,0,1,0,4,"CLO_VWF_PEY_1_2"),
            new(867164096,-179710113,163,31,3,0,1,0,4,"CLO_VWF_PEY_1_3"),
            new(867164096,51606258,163,31,4,0,1,0,4,"CLO_VWF_PEY_1_4"),
            new(867164096,161742867,163,31,5,0,1,0,4,"CLO_VWF_PEY_1_5"),
            new(867164096,662256573,163,31,6,0,1,0,4,"CLO_VWF_PEY_1_6"),
            new(867164096,743392617,163,31,7,0,1,0,4,"CLO_VWF_PEY_1_7"),
            new(867164096,966582276,163,31,8,0,1,0,4,"CLO_VWF_PEY_1_8"),
            new(867164096,1340345486,163,31,9,0,1,0,4,"CLO_VWF_PEY_1_9"),
            new(867164096,-2025884136,163,31,10,0,1,0,4,"CLO_VWF_PEY_110"),
            new(867164096,1954336919,163,31,11,0,1,0,4,"CLO_VWF_PEY_111"),
            new(2141921740,-1364447384,246,32,0,0,1,0,4,"CLO_H4F_PEY_0_0"),
            new(2141921740,1480295044,246,32,1,0,1,0,4,"CLO_H4F_PEY_0_1"),
            new(2141921740,-1977784761,246,32,2,0,1,0,4,"CLO_H4F_PEY_0_2"),
            new(2141921740,1372550572,246,32,3,0,1,0,4,"CLO_H4F_PEY_0_3"),
            new(2141921740,1133140258,246,32,4,0,1,0,4,"CLO_H4F_PEY_0_4"),
            new(2141921740,783527797,246,32,5,0,1,0,4,"CLO_H4F_PEY_0_5"),
            new(2141921740,525570229,246,32,6,0,1,0,4,"CLO_H4F_PEY_0_6"),
            new(2141921740,710420154,246,32,7,0,1,0,4,"CLO_H4F_PEY_0_7"),
            new(2141921740,484248516,246,32,8,0,1,0,4,"CLO_H4F_PEY_0_8"),
            new(2141921740,-470279681,246,32,9,0,1,0,4,"CLO_H4F_PEY_0_9"),
            new(2141921740,624976017,246,32,10,0,1,0,4,"CLO_H4F_PEY_010"),
            new(2141921740,393266418,246,32,11,0,1,0,4,"CLO_H4F_PEY_011"),
            new(2141921740,327104734,164,33,0,0,1,0,4,"CLO_H4F_PEY_1_0"),
            new(2141921740,99196339,164,33,1,0,1,0,4,"CLO_H4F_PEY_1_1"),
            new(2141921740,804581833,164,33,2,0,1,0,4,"CLO_H4F_PEY_1_2"),
            new(2141921740,566908276,164,33,3,0,1,0,4,"CLO_H4F_PEY_1_3"),
            new(2141921740,-1535223074,164,33,4,0,1,0,4,"CLO_H4F_PEY_1_4"),
            new(2141921740,-1766539445,164,33,5,0,1,0,4,"CLO_H4F_PEY_1_5"),
            new(2141921740,-1107784238,164,33,6,0,1,0,4,"CLO_H4F_PEY_1_6"),
            new(2141921740,-1301711180,164,33,7,0,1,0,4,"CLO_H4F_PEY_1_7"),
            new(2141921740,1229923461,164,33,8,0,1,0,4,"CLO_H4F_PEY_1_8"),
            new(2141921740,999885081,164,33,9,0,1,0,4,"CLO_H4F_PEY_1_9"),
            new(2141921740,-1409944646,164,33,10,0,1,0,4,"CLO_H4F_PEY_110"),
            new(2141921740,-1678486601,164,33,11,0,1,0,4,"CLO_H4F_PEY_111"),
            new(2141921740,-1630379289,164,34,0,0,1,0,4,"CLO_H4F_PEY_2_0"),
            new(2141921740,-442077042,164,34,1,0,1,0,4,"CLO_H4F_PEY_2_1"),
            new(2141921740,-2097370308,164,34,2,0,1,0,4,"CLO_H4F_PEY_2_2"),
            new(2141921740,-900220431,164,34,3,0,1,0,4,"CLO_H4F_PEY_2_3"),
            new(2141921740,-1147331460,164,34,4,0,1,0,4,"CLO_H4F_PEY_2_4"),
            new(2141921740,511435320,164,34,5,0,1,0,4,"CLO_H4F_PEY_2_5"),
            new(2141921740,280643253,164,34,6,0,1,0,4,"CLO_H4F_PEY_2_6"),
            new(2141921740,52898703,164,34,7,0,1,0,4,"CLO_H4F_PEY_2_7"),
            new(2141921740,-182939790,164,34,8,0,1,0,4,"CLO_H4F_PEY_2_8"),
            new(2141921740,-380995642,164,34,9,0,1,0,4,"CLO_H4F_PEY_2_9"),
            new(2141921740,-321839039,164,34,10,0,1,0,4,"CLO_H4F_PEY_210"),
            new(2141921740,1940860411,164,34,11,0,1,0,4,"CLO_H4F_PEY_211"),
            new(2141921740,1073312767,164,35,0,0,1,0,4,"CLO_H4F_PEY_3_0"),
            new(2141921740,315136414,164,35,1,0,1,0,4,"CLO_H4F_PEY_3_1"),
            new(2141921740,590559859,164,35,2,0,1,0,4,"CLO_H4F_PEY_3_2"),
            new(2141921740,833640293,164,35,3,0,1,0,4,"CLO_H4F_PEY_3_3"),
            new(2141921740,1113749705,164,35,4,0,1,0,4,"CLO_H4F_PEY_3_4"),
            new(2141921740,349871546,164,35,5,0,1,0,4,"CLO_H4F_PEY_3_5"),
            new(2141921740,1720729896,164,35,6,0,1,0,4,"CLO_H4F_PEY_3_6"),
            new(2141921740,2002248375,164,35,7,0,1,0,4,"CLO_H4F_PEY_3_7"),
            new(2141921740,1227163218,164,35,8,0,1,0,4,"CLO_H4F_PEY_3_8"),
            new(2141921740,1529621088,164,35,9,0,1,0,4,"CLO_H4F_PEY_3_9"),
            new(2141921740,-488766247,164,35,10,0,1,0,4,"CLO_H4F_PEY_310"),
            new(2141921740,1676314352,164,35,11,0,1,0,4,"CLO_H4F_PEY_311"),
        };

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
            uint hash = plpl.Skin.model;
            RequestModel(hash);
            while (!HasModelLoaded(hash)) await BaseScript.Delay(1);
            SetPlayerModel(PlayerId(), hash);
            int id = PlayerCache.MyPlayer.Ped.Handle;
            int[][] aa = func_1278(_selezionato == "Maschio", 0);
            ComponentDrawables comp = new ComponentDrawables(aa[0][0], aa[0][1], aa[0][2], aa[0][3], aa[0][4], aa[0][5], aa[0][6], aa[0][7], aa[0][8], aa[0][9], aa[0][10], aa[0][11]);
            ComponentDrawables text = new ComponentDrawables(aa[1][0], aa[1][1], aa[1][2], aa[1][3], aa[1][4], aa[1][5], aa[1][6], aa[1][7], aa[1][8], aa[1][9], aa[1][10], aa[1][11]);
            PropIndices _prop = new PropIndices(GetPedPropIndex(id, 0), GetPedPropIndex(id, 1), GetPedPropIndex(id, 2), GetPedPropIndex(id, 3), GetPedPropIndex(id, 4), GetPedPropIndex(id, 5), GetPedPropIndex(id, 6), GetPedPropIndex(id, 7), GetPedPropIndex(id, 8));
            PropIndices _proptxt = new PropIndices(GetPedPropTextureIndex(id, 0), GetPedPropTextureIndex(id, 1), GetPedPropTextureIndex(id, 2), GetPedPropTextureIndex(id, 3), GetPedPropTextureIndex(id, 4), GetPedPropTextureIndex(id, 5), GetPedPropTextureIndex(id, 6), GetPedPropTextureIndex(id, 7), GetPedPropTextureIndex(id, 8));
            plpl.Dressing = new("", "", comp, text, _prop, _proptxt);
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
                long assicurazione = SharedMath.GetRandomLong(999999999999999);
                //Vector3 spawna = new Vector3(Main.charCreateCoords.X, Main.charCreateCoords.Y, Main.charCreateCoords.Z);
                if (IsValidInterior(94722)) LoadInterior(94722);
                while (!IsInteriorReady(94722)) await BaseScript.Delay(1000);
                sub_8d2b2();
                Cache.PlayerCache.MyPlayer.Status.Istanza.Istanzia("CreazionePersonaggio");
                _dataMaschio = new FreeRoamChar(SnowflakeGenerator.Instance.Next().ToInt64(), new Finance(1000, 3000, 0), new Gang("Incensurato", 0), new Skin("Maschio", (uint)PedHash.FreemodeMale01, 0, GetRandomFloatInRange(.5f, 1f), new Face(0, 0, new float[20] { Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(1f, -1, 1) }), new A2(GetRandomIntInRange(0, Ageing.Count), GetRandomFloatInRange(0f, 1f)), new A2(255, 0f), new A2(GetRandomIntInRange(0, blemishes.Count), GetRandomFloatInRange(0f, 1f)), new A2(GetRandomIntInRange(0, Complexions.Count), GetRandomFloatInRange(0f, 1f)), new A2(GetRandomIntInRange(0, Danni_Pelle.Count), GetRandomFloatInRange(0f, 1f)), new A2(GetRandomIntInRange(0, Nei_e_Porri.Count), GetRandomFloatInRange(0f, 1f)), new A3(255, 0f, new int[2] { 0, 0 }), new A3(255, 0f, new int[2] { 0, 0 }), new Facial(new A3(GetRandomIntInRange(0, Beards.Count), GetRandomFloatInRange(0f, 1f), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) }), new A3(GetRandomIntInRange(0, eyebrow.Count), GetRandomFloatInRange(0f, 1f), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) })), new Hair(GetRandomIntInRange(0, HairUomo.Count), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) }), new Eye(GetRandomIntInRange(0, Colore_Occhi.Count)), new Ears(255, 0)), new Dressing("Iniziale", "Per cominciare", new ComponentDrawables(-1, 0, GetPedDrawableVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), 0, 0, -1, 15, 0, 15, 0, 0, 56), new ComponentDrawables(-1, 0, GetPedTextureVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), 0, 4, -1, 14, 0, 0, 0, 0, 0), new PropIndices(-1, GetPedPropIndex(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, GetPedPropTextureIndex(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), -1, -1, -1, -1, -1, -1, -1)), new List<Weapons>(), new FreeRoamStats());
                _dataFemmina = new FreeRoamChar(SnowflakeGenerator.Instance.Next().ToInt64(), new Finance(1000, 3000, 0), new Gang("Incensurato", 0), new Skin("Femmina", (uint)PedHash.FreemodeFemale01, 0, GetRandomFloatInRange(.5f, 1f), new Face(21, 0, new float[20] { Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1), Funzioni.Normalize(0f, -1, 1) }), new A2(GetRandomIntInRange(0, Ageing.Count), GetRandomFloatInRange(0f, 1f)), new A2(255, 0f), new A2(GetRandomIntInRange(0, blemishes.Count), GetRandomFloatInRange(0f, 1f)), new A2(GetRandomIntInRange(0, Complexions.Count), GetRandomFloatInRange(0f, 1f)), new A2(GetRandomIntInRange(0, Danni_Pelle.Count), GetRandomFloatInRange(0f, 1f)), new A2(GetRandomIntInRange(0, Nei_e_Porri.Count), GetRandomFloatInRange(0f, 1f)), new A3(255, 0f, new int[2] { 0, 0 }), new A3(255, 0f, new int[2] { 0, 0 }), new Facial(new A3(255, 0f, new int[2] { 0, 0 }), new A3(GetRandomIntInRange(0, eyebrow.Count), GetRandomFloatInRange(0f, 1f), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) })), new Hair(GetRandomIntInRange(0, HairUomo.Count), new int[2] { GetRandomIntInRange(0, 63), GetRandomIntInRange(0, 63) }), new Eye(GetRandomIntInRange(0, Colore_Occhi.Count)), new Ears(255, 0)), new Dressing("Iniziale", "Per cominciare", new ComponentDrawables(-1, 0, GetPedDrawableVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), 0, 0, -1, 15, 0, 15, 0, 0, 56), new ComponentDrawables(-1, 0, GetPedTextureVariation(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), 0, 4, -1, 14, 0, 0, 0, 0, 0), new PropIndices(-1, GetPedPropIndex(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), -1, -1, -1, -1, -1, -1, -1), new PropIndices(-1, GetPedPropTextureIndex(Cache.PlayerCache.MyPlayer.Ped.Handle, 2), -1, -1, -1, -1, -1, -1, -1)), new List<Weapons>(), new FreeRoamStats());
                _data = _selezionato.ToLower() == "maschio" ? _dataMaschio : _dataFemmina;
                AggiornaModel(_data.ToJson());
                await BaseScript.Delay(1000);
                Cache.PlayerCache.MyPlayer.Ped.Position = new Vector3(402.91f, -996.74f, -180.00025f);
                while (!HasCollisionLoadedAroundEntity(PlayerCache.MyPlayer.Ped.Handle)) await BaseScript.Delay(1);
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

        public static UIMenu Creazione = new UIMenu("", "", true);
        public static UIMenu Genitori = new UIMenu("", "", false);
        public static UIMenu Dettagli = new UIMenu("", "", false);
        public static UIMenu Apparenze = new UIMenu("", "", false);
        public static UIMenu Apparel = new UIMenu("", "", false);
        public static UIMenu Statistiche = new UIMenu("", "", false);
        public static UIMenuItem Salva = new("", "");
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
                Creazione = new("FreeRoam Creator", "Crea un nuovo Personaggio", offset, "thelastgalaxy", "bannerbackground", false, true)
                {
                    ControlDisablingEnabled = true
                };
                UIMenuListItem Sesso = _selezionato == "Maschio" ? new UIMenuListItem("Sesso", new List<dynamic>() { "Maschio", "Femmina" }, 0, "Decidi il Sesso") : new UIMenuListItem("Sesso", new List<dynamic>() { "Maschio", "Femmina" }, 1, "Decidi il Sesso");
                Creazione.AddItem(Sesso);
                UIMenuItem GenitoriItem = new UIMenuItem(GetLabelText("FACE_HERI"), GetLabelText("FACE_MM_H3"));
                Genitori = new(GetLabelText("FACE_HERI"), "Crea un nuovo Personaggio")
                {
                    ControlDisablingEnabled = true
                };
                UIMenuItem DettagliItem = new UIMenuItem(GetLabelText("FACE_FEAT"), GetLabelText("FACE_MM_H4"));
                Dettagli = new(GetLabelText("FACE_FEAT"), "Crea un nuovo Personaggio")
                {
                    ControlDisablingEnabled = true
                };
                UIMenuItem ApparenzeItem = new UIMenuItem(GetLabelText("FACE_APP"), GetLabelText("FACE_MM_H6"));
                Apparenze = new(GetLabelText("FACE_APP"), "Crea un nuovo Personaggio")
                {
                    ControlDisablingEnabled = true
                };
                UIMenuItem ApparelItem = new UIMenuItem(GetLabelText("FACE_APPA"), GetLabelText("FACE_APPA_H"));
                Apparel = new(GetLabelText("FACE_APPA"), "Crea un nuovo Personaggio")
                {
                    ControlDisablingEnabled = true
                };
                UIMenuItem StatisticheItem = new UIMenuItem(GetLabelText("FACE_STATS"), GetLabelText("FACE_MM_H5"));
                Statistiche = new(GetLabelText("FACE_STATS"), "Crea un nuovo Personaggio");

                GenitoriItem.Activated += async (a, b) => await Creazione.SwitchTo(Genitori, 0, true);
                DettagliItem.Activated += async (a, b) => await Creazione.SwitchTo(Dettagli, 0, true);
                ApparenzeItem.Activated += async (a, b) => await Creazione.SwitchTo(Apparenze, 0, true);
                ApparelItem.Activated += async (a, b) => await Creazione.SwitchTo(Apparel, 0, true);
                StatisticheItem.Activated += async (a, b) => await Creazione.SwitchTo(Statistiche, 0, true);

                Creazione.AddItem(GenitoriItem);
                Creazione.AddItem(DettagliItem);
                Creazione.AddItem(ApparenzeItem);
                Creazione.AddItem(ApparelItem);
                Creazione.AddItem(StatisticheItem);

                InstructionalButton button1 = new InstructionalButton(Control.LookLeftRight, "Guarda a Destra/Sinistra");
                InstructionalButton button2 = new InstructionalButton(Control.FrontendLb, "Guarda a Sinistra");
                InstructionalButton button3 = new InstructionalButton(Control.FrontendRb, "Guarda a Destra");
                InstructionalButton button4 = new InstructionalButton(InputGroup.INPUTGROUP_LOOK, "Cambia dettagli");
                InstructionalButton button5 = new InstructionalButton(InputGroup.INPUTGROUP_LOOK, "Gestisci Panelli", ScaleformUI.Scaleforms.PadCheck.Keyboard);
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
                #endregion

                #region VESTITI
                List<dynamic> styleList = new List<dynamic>();
                for (int i = 0; i < 8; i++) styleList.Add(GetLabelText("FACE_A_STY_" + i));

                List<dynamic> outfitList = new List<dynamic>();
                for (int i = 0; i < 8; i++) outfitList.Add(GetLabelText(getOutfit(i, _selezionato == "Maschio")));

                List<dynamic> hatList = new() { GetLabelText("FACE_OFF") };

                List<dynamic> glassesList = new List<dynamic>() { GetLabelText("FACE_OFF") };

                if (_selezionato == "Maschio")
                {
                    foreach (ShopPed.PedComponentData _hat in MaleHats) hatList.Add(GetLabelText(_hat.Label));
                    foreach (ShopPed.PedComponentData _glas in MaleGlasses) glassesList.Add(GetLabelText(_glas.Label));
                }
                else
                {
                    foreach (ShopPed.PedComponentData _hat in FemaleHats) hatList.Add(GetLabelText(_hat.Label));
                    foreach (ShopPed.PedComponentData _glas in FemaleGlasses) glassesList.Add(GetLabelText(_glas.Label));
                }

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

                #region Stats
                int StatMax = 100;
                UIMenuStatsItem stamina = new UIMenuStatsItem(GetLabelText("FACE_STAM"), GetLabelText("FACE_H_STA"), 0, SColor.HUD_Freemode);
                UIMenuPercentagePanel maxstat = new("Punti Rimanenti", "0", "100", StatMax);
                stamina.AddPanel(maxstat);
                UIMenuStatsItem shooting = new UIMenuStatsItem(GetLabelText("FACE_SHOOT"), GetLabelText("FACE_H_SHO"), 0, SColor.HUD_Freemode);
                UIMenuPercentagePanel maxstat1 = new("Punti Rimanenti", "0", "100", StatMax);
                shooting.AddPanel(maxstat1);
                UIMenuStatsItem strength = new UIMenuStatsItem(GetLabelText("FACE_STR"), GetLabelText("FACE_H_STR"), 0, SColor.HUD_Freemode);
                UIMenuPercentagePanel maxstat2 = new("Punti Rimanenti", "0", "100", StatMax);
                strength.AddPanel(maxstat2);
                UIMenuStatsItem stealth = new UIMenuStatsItem(GetLabelText("FACE_STEALTH"), GetLabelText("FACE_H_STE"), 0, SColor.HUD_Freemode);
                UIMenuPercentagePanel maxstat3 = new("Punti Rimanenti", "0", "100", StatMax);
                stealth.AddPanel(maxstat3);
                UIMenuStatsItem flying = new UIMenuStatsItem(GetLabelText("FACE_FLY"), GetLabelText("FACE_H_FLY"), 0, SColor.HUD_Freemode);
                UIMenuPercentagePanel maxstat4 = new("Punti Rimanenti", "0", "100", StatMax);
                flying.AddPanel(maxstat4);
                UIMenuStatsItem driving = new UIMenuStatsItem(GetLabelText("FACE_DRIV"), GetLabelText("FACE_H_DRI"), 0, SColor.HUD_Freemode);
                UIMenuPercentagePanel maxstat5 = new("Punti Rimanenti", "0", "100", StatMax);
                driving.AddPanel(maxstat5);
                UIMenuStatsItem lungs = new UIMenuStatsItem(GetLabelText("FACE_LUNG"), GetLabelText("FACE_H_LCP"), 0, SColor.HUD_Freemode);
                UIMenuPercentagePanel maxstat6 = new("Punti Rimanenti", "0", "100", StatMax);
                lungs.AddPanel(maxstat6);

                Statistiche.AddItem(stamina);
                Statistiche.AddItem(shooting);
                Statistiche.AddItem(strength);
                Statistiche.AddItem(stealth);
                Statistiche.AddItem(flying);
                Statistiche.AddItem(driving);
                Statistiche.AddItem(lungs);

                #endregion

                #endregion

                #region CorpoMenu

                #region Sesso

                Sesso.OnListChanged += async (item, _newIndex) =>
                {
                    /*
                    MenuHandler.CloseAndClearHistory();
                    Screen.Effects.Start(ScreenEffect.MpCelebWin);
                    await BaseScript.Delay(1000);
                    Screen.Fading.FadeOut(1000);
                    await BaseScript.Delay(1000);
                    MenuHandler.CloseAndClearHistory();
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
                    */

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
                    PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;
                    BD1.AttachTo(Cache.PlayerCache.MyPlayer.Ped.Bones[Bone.PH_R_Hand], Vector3.Zero, Vector3.Zero);
                    TaskPlayAnim(PlayerPedId(), sub_7dd83(1, 0, _selezionato), "Loop", 8f, -4f, -1, 513, 0, false, false, false);
                };

                #endregion

                #region Genitori

                Genitori.OnListChange += (_sender, _listItem, _newIndex) =>
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
                    float perc = c / 100;
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
                                        _data.Skin.face.tratti[6] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[11] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[0] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[2] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[5] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[8] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[19] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[12] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[14] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[15] = Funzioni.Denormalize(0f, -1f, 1);
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
                                        _data.Skin.face.tratti[17] = Funzioni.Denormalize(0f, -1f, 1);
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

                int first = 0;
                Apparel.OnListChange += async (sender, item, index) =>
                {
                    int id = PlayerPedId();
                    if (item == stile)
                    {
                        List<dynamic> list = new();
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 0) first = index * 8;
                            list.Add(GetLabelText(getOutfit(first + i, _selezionato == "Maschio")));
                        }
                        outfit.ChangeList(list, 0);
                        int[][] aa = func_1278(_selezionato == "Maschio", first);
                        ComponentDrawables comp = new ComponentDrawables(aa[0][0], aa[0][1], aa[0][2], aa[0][3], aa[0][4], aa[0][5], aa[0][6], aa[0][7], aa[0][8], aa[0][9], aa[0][10], aa[0][11]);
                        ComponentDrawables text = new ComponentDrawables(aa[1][0], aa[1][1], aa[1][2], aa[1][3], aa[1][4], aa[1][5], aa[1][6], aa[1][7], aa[1][8], aa[1][9], aa[1][10], aa[1][11]);
                        PropIndices _prop = new PropIndices(GetPedPropIndex(id, 0), GetPedPropIndex(id, 1), GetPedPropIndex(id, 2), GetPedPropIndex(id, 3), GetPedPropIndex(id, 4), GetPedPropIndex(id, 5), GetPedPropIndex(id, 6), GetPedPropIndex(id, 7), GetPedPropIndex(id, 8));
                        PropIndices _proptxt = new PropIndices(GetPedPropTextureIndex(id, 0), GetPedPropTextureIndex(id, 1), GetPedPropTextureIndex(id, 2), GetPedPropTextureIndex(id, 3), GetPedPropTextureIndex(id, 4), GetPedPropTextureIndex(id, 5), GetPedPropTextureIndex(id, 6), GetPedPropTextureIndex(id, 7), GetPedPropTextureIndex(id, 8));
                        _data.Dressing = new("", "", comp, text, _prop, _proptxt);

                    }
                    else if (item == outfit)
                    {
                        int[][] aa = func_1278(_selezionato == "Maschio", (index + first));
                        ComponentDrawables comp = new ComponentDrawables(aa[0][0], aa[0][1], aa[0][2], aa[0][3], aa[0][4], aa[0][5], aa[0][6], aa[0][7], aa[0][8], aa[0][9], aa[0][10], aa[0][11]);
                        ComponentDrawables text = new ComponentDrawables(aa[1][0], aa[1][1], aa[1][2], aa[1][3], aa[1][4], aa[1][5], aa[1][6], aa[1][7], aa[1][8], aa[1][9], aa[1][10], aa[1][11]);
                        PropIndices _prop = new PropIndices(GetPedPropIndex(id, 0), GetPedPropIndex(id, 1), GetPedPropIndex(id, 2), GetPedPropIndex(id, 3), GetPedPropIndex(id, 4), GetPedPropIndex(id, 5), GetPedPropIndex(id, 6), GetPedPropIndex(id, 7), GetPedPropIndex(id, 8));
                        PropIndices _proptxt = new PropIndices(GetPedPropTextureIndex(id, 0), GetPedPropTextureIndex(id, 1), GetPedPropTextureIndex(id, 2), GetPedPropTextureIndex(id, 3), GetPedPropTextureIndex(id, 4), GetPedPropTextureIndex(id, 5), GetPedPropTextureIndex(id, 6), GetPedPropTextureIndex(id, 7), GetPedPropTextureIndex(id, 8));
                        _data.Dressing = new("", "", comp, text, _prop, _proptxt);
                    }

                    else if (item == hat)
                    {
                        Client.Logger.Debug("hat id = " + index);
                        if (index == 0) ClearPedProp(id, 0);
                        else
                        {
                            ShopPed.PedComponentData prop = new();
                            if (_selezionato == "Maschio")
                                prop = MaleHats[index - 1];
                            else
                                prop = FemaleHats[index - 1];
                            ComponentDrawables comp = new ComponentDrawables(GetPedDrawableVariation(id, 0), GetPedDrawableVariation(id, 1), GetPedDrawableVariation(id, 2), GetPedDrawableVariation(id, 3), GetPedDrawableVariation(id, 4), GetPedDrawableVariation(id, 5), GetPedDrawableVariation(id, 6), GetPedDrawableVariation(id, 7), GetPedDrawableVariation(id, 8), GetPedDrawableVariation(id, 9), GetPedDrawableVariation(id, 10), GetPedDrawableVariation(id, 11));
                            ComponentDrawables text = new ComponentDrawables(GetPedTextureVariation(id, 0), GetPedTextureVariation(id, 1), GetPedTextureVariation(id, 2), GetPedTextureVariation(id, 3), GetPedTextureVariation(id, 4), GetPedTextureVariation(id, 5), GetPedTextureVariation(id, 6), GetPedTextureVariation(id, 7), GetPedTextureVariation(id, 8), GetPedTextureVariation(id, 9), GetPedTextureVariation(id, 10), GetPedTextureVariation(id, 11));
                            PropIndices _prop = new PropIndices(prop.Drawable, GetPedPropIndex(id, 1), GetPedPropIndex(id, 2), GetPedPropIndex(id, 3), GetPedPropIndex(id, 4), GetPedPropIndex(id, 5), GetPedPropIndex(id, 6), GetPedPropIndex(id, 7), GetPedPropIndex(id, 8));
                            PropIndices _proptxt = new PropIndices(prop.Texture, GetPedPropTextureIndex(id, 1), GetPedPropTextureIndex(id, 2), GetPedPropTextureIndex(id, 3), GetPedPropTextureIndex(id, 4), GetPedPropTextureIndex(id, 5), GetPedPropTextureIndex(id, 6), GetPedPropTextureIndex(id, 7), GetPedPropTextureIndex(id, 8));
                            _data.Dressing = new("", "", comp, text, _prop, _proptxt);
                        }
                    }
                    else if (item == glasses)
                    {
                        if (index == 0) ClearPedProp(id, 1);
                        else
                        {
                            ShopPed.PedComponentData prop = new();
                            if (_selezionato == "Maschio")
                                prop = MaleGlasses[index - 1];
                            else
                            {
                                prop = FemaleGlasses[index - 1];
                            }

                            ComponentDrawables comp = new ComponentDrawables(GetPedDrawableVariation(id, 0), GetPedDrawableVariation(id, 1), GetPedDrawableVariation(id, 2), GetPedDrawableVariation(id, 3), GetPedDrawableVariation(id, 4), GetPedDrawableVariation(id, 5), GetPedDrawableVariation(id, 6), GetPedDrawableVariation(id, 7), GetPedDrawableVariation(id, 8), GetPedDrawableVariation(id, 9), GetPedDrawableVariation(id, 10), GetPedDrawableVariation(id, 11));
                            ComponentDrawables text = new ComponentDrawables(GetPedTextureVariation(id, 0), GetPedTextureVariation(id, 1), GetPedTextureVariation(id, 2), GetPedTextureVariation(id, 3), GetPedTextureVariation(id, 4), GetPedTextureVariation(id, 5), GetPedTextureVariation(id, 6), GetPedTextureVariation(id, 7), GetPedTextureVariation(id, 8), GetPedTextureVariation(id, 9), GetPedTextureVariation(id, 10), GetPedTextureVariation(id, 11));
                            PropIndices _prop = new PropIndices(GetPedPropIndex(id, 0), prop.Drawable, GetPedPropIndex(id, 2), GetPedPropIndex(id, 3), GetPedPropIndex(id, 4), GetPedPropIndex(id, 5), GetPedPropIndex(id, 6), GetPedPropIndex(id, 7), GetPedPropIndex(id, 8));
                            PropIndices _proptxt = new PropIndices(GetPedPropTextureIndex(id, 0), prop.Texture, GetPedPropTextureIndex(id, 2), GetPedPropTextureIndex(id, 3), GetPedPropTextureIndex(id, 4), GetPedPropTextureIndex(id, 5), GetPedPropTextureIndex(id, 6), GetPedPropTextureIndex(id, 7), GetPedPropTextureIndex(id, 8));
                            _data.Dressing = new("", "", comp, text, _prop, _proptxt);
                        }
                    }
                    UpdateDress(Cache.PlayerCache.MyPlayer.Ped.Handle, _data.Dressing);
                    TaskProvaClothes(Cache.PlayerCache.MyPlayer.Ped, sub_7dd83(1, 0, _data.Skin.sex));
                };

                #endregion

                #region Stats
                int _stamina = 0, _shooting = 0, _strength = 0, _stealth = 0, _flying = 0, _driving = 0, _lungs = 0;
                Statistiche.OnStatsItemChanged += (a, b, c) =>
                {
                    int max = 0;
                    foreach (UIMenuItem item in a.MenuItems)
                    {
                        max += (item as UIMenuStatsItem).Value;
                    }
                    StatMax = 100 - max;
                    foreach (UIMenuItem item in a.MenuItems)
                    {
                        (item.Panels[0] as UIMenuPercentagePanel).Percentage = StatMax;
                    }
                    if (b == stamina)
                    {
                        if (StatMax > 0)
                            _stamina = c;
                        else b.Value = _stamina;
                    }
                    else if (b == shooting)
                    {
                        if (StatMax > 0)
                            _shooting = c;
                        else b.Value = _shooting;
                    }
                    else if (b == strength)
                    {
                        if (StatMax > 0)
                            _strength = c;
                        else b.Value = _strength;
                    }
                    else if (b == stealth)
                    {
                        if (StatMax > 0)
                            _stealth = c;
                        else b.Value = _stealth;
                    }
                    else if (b == flying)
                    {
                        if (StatMax > 0)
                            _flying = c;
                        else b.Value = _flying;
                    }
                    else if (b == driving)
                    {
                        if (StatMax > 0)
                            _driving = c;
                        else b.Value = _driving;
                    }
                    else if (b == lungs)
                    {
                        if (StatMax > 0)
                            _lungs = c;
                        else b.Value = _lungs;
                    }
                    _data.Statistiche = new()
                    {
                        STAMINA = _stamina,
                        STRENGTH = _strength,
                        LUNG_CAPACITY = _lungs,
                        STEALTH_ABILITY = _stealth,
                        SHOOTING_ABILITY = _shooting,
                        WHEELIE_ABILITY = _driving,
                        FLYING_ABILITY = _flying,
                        Experience = 0,
                        Prestige = 0,
                        Kills = 0,
                        Deaths = 0,
                        Headshots = 0,
                        MaxKillStreak = 0,
                        MissionsDone = 0,
                        EventsWon = 0,
                    };
                };
                #endregion

                #endregion

                #region ControlloAperturaChiusura

                Apparenze.OnMenuOpen += (a, b) =>
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
                };

                Apparel.OnMenuOpen += (a, b) =>
                {
                    TaskCreaClothes(Cache.PlayerCache.MyPlayer.Ped, sub_7dd83(1, 0, _selezionato));
                };

                Genitori.OnMenuOpen += (a, _) =>
                {
                    AnimateGameplayCamZoom(true, _ncam);
                };
                Dettagli.OnMenuOpen += (a, _) =>
                {
                    AnimateGameplayCamZoom(true, _ncam);
                };
                Apparenze.OnMenuOpen += (a, _) =>
                {
                    AnimateGameplayCamZoom(true, _ncam);
                };

                Genitori.OnMenuClose += (a) =>
                {
                    AnimateGameplayCamZoom(false, _ncam);
                };
                Dettagli.OnMenuClose += (a) =>
                {
                    AnimateGameplayCamZoom(false, _ncam);
                };
                Apparenze.OnMenuClose += (a) =>
                {
                    AnimateGameplayCamZoom(false, _ncam);
                };
                Apparel.OnMenuClose += (a) =>
                {
                    TaskClothesALoop(Cache.PlayerCache.MyPlayer.Ped, sub_7dd83(1, 0, _selezionato));
                };

                #endregion

                #region CREA_BUTTON_FINISH

                Salva = new UIMenuItem("Salva Personaggio", "Pronto per ~y~entrare in gioco~w~?", SColor.HUD_Freemode_dark, SColor.HUD_Freemode);
                Salva.SetRightBadge(BadgeIcon.TICK);
                Salva.Activated += async (_selectedItem, _index) =>
                {
                    Screen.Fading.FadeOut(800);
                    await BaseScript.Delay(1000);
                    if (_dummyPed != null)
                        if (_dummyPed.Exists())
                            _dummyPed.Delete();
                    Creazione.Visible = false;
                    MenuHandler.CloseAndClearHistory();
                    BD1.Detach();
                    BD1.Delete();
                    PlayerCache.MyPlayer.Ped.Detach();

                    EventDispatcher.Send("tlg:freeroam:finishCharServer", _data);
                    //PlayerCache.MyPlayer.User.FreeRoamChar = await EventDispatcher.Get<FreeRoamChar>("tlg:freeroam:Select_Char", _data.CharID);
                    PlayerCache.MyPlayer.User.FreeRoamChar = _data;

                    Client.Instance.RemoveTick(Controllo);
                    Client.Instance.RemoveTick(Scaleform);
                    Client.Instance.RemoveTick(TastiMenu);
                    //TODO: GESTIRE CAMFIRSTTIME
                    //CamerasFirstTime.FirstTimeTransition(_data.CharID == 1);

                    //TODO: CONTINUARE DA QUI
                    FreeRoamCamerasFirstTime.FirstTimeTransition();

                    RemoveAnimDict("mp_character_creation@lineup@male_a");
                    RemoveAnimDict("mp_character_creation@lineup@male_b");
                    RemoveAnimDict("mp_character_creation@lineup@female_a");
                    RemoveAnimDict("mp_character_creation@lineup@female_b");
                    RemoveAnimDict("mp_character_creation@customise@male_a");
                    RemoveAnimDict("mp_character_creation@customise@female_a");
                };
                Creazione.AddItem(Salva);

                #endregion

                Creazione.Visible = true;
                Client.Logger.Debug(Creazione.MenuItems.Count.ToString());
                Client.Instance.AddTick(TastiMenu);
            }
            catch (Exception e)
            {
                Client.Logger.Error("MenuCreazione => " + e.ToString());
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

                            _data.Skin.face.tratti[7] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[7] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[6] = Funzioni.Denormalize(CoordY, -1, 1);

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[6] = Funzioni.Denormalize(CoordY, -1, 1);
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

                            _data.Skin.face.tratti[11] = Funzioni.Denormalize(-CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[11] = Funzioni.Denormalize(-CoordX, -1, 1);
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

                            _data.Skin.face.tratti[10] = Funzioni.Denormalize(-CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[10] = Funzioni.Denormalize(-CoordX, -1, 1);
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

                            _data.Skin.face.tratti[12] = Funzioni.Denormalize(-CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[12] = Funzioni.Denormalize(-CoordX, -1, 1);
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

                            _data.Skin.face.tratti[19] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[19] = Funzioni.Denormalize(CoordX, -1, 1);
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

                            _data.Skin.face.tratti[0] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[0] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[1] = Funzioni.Denormalize(CoordY, -1, 1);

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[1] = Funzioni.Denormalize(CoordY, -1, 1);
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

                            _data.Skin.face.tratti[2] = Funzioni.Denormalize(-CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[2] = Funzioni.Denormalize(-CoordX, -1, 1);

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[3] = Funzioni.Denormalize(CoordY, -1, 1);

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[3] = Funzioni.Denormalize(CoordY, -1, 1);
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

                            _data.Skin.face.tratti[5] = Funzioni.Denormalize(-CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[5] = Funzioni.Denormalize(-CoordX, -1, 1);

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[4] = Funzioni.Denormalize(CoordY, -1, 1);

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[4] = Funzioni.Denormalize(CoordY, -1, 1);
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

                            _data.Skin.face.tratti[9] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[9] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[8] = Funzioni.Denormalize(CoordY, -1, 1);

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[8] = Funzioni.Denormalize(CoordY, -1, 1);
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

                            _data.Skin.face.tratti[13] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[13] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[14] = Funzioni.Denormalize(CoordY, -1, 1);

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[14] = Funzioni.Denormalize(CoordY, -1, 1);
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

                            _data.Skin.face.tratti[16] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[16] = Funzioni.Denormalize(CoordX, -1, 1);

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[15] = Funzioni.Denormalize(CoordY, -1, 1);

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[15] = Funzioni.Denormalize(CoordY, -1, 1);
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

                            _data.Skin.face.tratti[18] = Funzioni.Denormalize(-CoordX, -1, 1);

                            if (IsControlPressed(2, 5) || IsDisabledControlPressed(2, 5) && !IsInputDisabled(2))
                            {
                                CoordX -= 0.03f;
                                if (CoordX < 0) CoordX = 0;
                            }

                            _data.Skin.face.tratti[18] = Funzioni.Denormalize(-CoordX, -1, 1);

                            if (IsControlPressed(2, 4) || IsDisabledControlPressed(2, 4) && !IsInputDisabled(2))
                            {
                                CoordY += 0.03f;
                                if (CoordY > 1f) CoordY = 1f;
                            }

                            _data.Skin.face.tratti[17] = Funzioni.Denormalize(CoordY, -1, 1);

                            if (IsControlPressed(2, 3) || IsDisabledControlPressed(2, 3) && !IsInputDisabled(2))
                            {
                                CoordY -= 0.03f;
                                if (CoordY < 0) CoordY = 0;
                            }

                            _data.Skin.face.tratti[17] = Funzioni.Denormalize(CoordY, -1, 1);
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
                MenuHandler.CloseAndClearHistory();
                ScaleformUI.Main.Warning.ShowWarningWithButtons("La creazione verrà annullata", "Vuoi annullare la creazione del personaggio?", "Tornerai alla schermata di selezione.", new List<InstructionalButton>
                {
                    new InstructionalButton(Control.FrontendCancel, "No"),
                    new InstructionalButton(Control.FrontendAccept, "Si"),
                });
                ScaleformUI.Main.Warning.OnButtonPressed += async (a) =>
                {
                    if (a.GamepadButton == Control.FrontendCancel)
                    {
                        Screen.Fading.FadeOut(0);
                        await BaseScript.Delay(100);
                        MenuCreazione(_d);
                    }
                    else if (a.GamepadButton == Control.FrontendAccept)
                    {
                        Screen.Fading.FadeOut(1000);
                        await BaseScript.Delay(1000);
                        if (_dummyPed != null)
                            if (_dummyPed.Exists())
                                _dummyPed.Delete();
                        Creazione.Visible = false;
                        MenuHandler.CloseAndClearHistory();
                        BD1.Detach();
                        BD1.Delete();
                        Cache.PlayerCache.MyPlayer.Ped.Detach();
                        Client.Instance.RemoveTick(Controllo);
                        Client.Instance.RemoveTick(Scaleform);
                        Client.Instance.RemoveTick(TastiMenu);
                        ServerJoining.ReturnToLobby();
                        RenderScriptCams(false, false, 300, false, false);
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
            string v_3 = sub_7ce29(SharedMath.GetRandomInt(0, 7));
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

        public static void UpdateFace(int Handle, Skin skin)
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

        public static void UpdateDress(int Handle, Dressing dress)
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

        static int[][] func_1278(bool isMale, int iParam1)
        {
            ShopPed.PedComponentData Var1;

            int[] components = new int[12];
            int[] textures = new int[12];

            for (int i = 0; i < 12; i++)
            {
                components[i] = -1;
                textures[i] = -1;
            }

            switch (iParam1)
            {
                case 56:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 0;
                        textures[4] = 6;
                        components[6] = 0;
                        textures[6] = 10;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 1;
                        textures[11] = 0;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 9;
                        textures[3] = 0;
                        components[4] = 4;
                        textures[4] = 9;
                        components[6] = 13;
                        textures[6] = 12;
                        components[7] = 1;
                        textures[7] = 2;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 9;
                        textures[11] = 9;
                    }
                    break;

                case 57:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 1;
                        textures[4] = 15;
                        components[6] = 1;
                        textures[6] = 9;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 14;
                        textures[11] = 11;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 2;
                        textures[3] = 0;
                        components[4] = 2;
                        textures[4] = 2;
                        components[6] = 2;
                        textures[6] = 14;
                        components[7] = 5;
                        textures[7] = 4;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 2;
                        textures[11] = 6;
                    }
                    break;

                case 58:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 1;
                        textures[3] = 0;
                        components[4] = 4;
                        textures[4] = 2;
                        components[6] = 0;
                        textures[6] = 10;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 1;
                        textures[8] = 5;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 7;
                        textures[11] = 15;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 3;
                        textures[3] = 0;
                        components[4] = 3;
                        textures[4] = 11;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_FEET001"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 1;
                        textures[7] = 3;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 3;
                        textures[11] = 10;
                    }
                    break;

                case 59:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 1;
                        textures[4] = 0;
                        components[6] = 1;
                        textures[6] = 1;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 0;
                        textures[11] = 2;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        components[4] = 8;
                        textures[4] = 8;
                        components[6] = 15;
                        textures[6] = 1;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 4;
                        textures[8] = 13;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 1;
                        textures[11] = 5;
                    }
                    break;

                case 60:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 12;
                        textures[3] = 0;
                        components[4] = 1;
                        textures[4] = 14;
                        components[6] = 1;
                        textures[6] = 4;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB8_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 2;
                        textures[3] = 0;
                        components[4] = 3;
                        textures[4] = 7;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_FEET011"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 1;
                        textures[7] = 0;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 2;
                        textures[11] = 15;
                    }
                    break;

                case 61:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 8;
                        textures[3] = 0;
                        components[4] = 4;
                        textures[4] = 4;
                        components[6] = 4;
                        textures[6] = 1;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB5_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 3;
                        textures[3] = 0;
                        components[4] = 2;
                        textures[4] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_FEET006"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 2;
                        textures[7] = 1;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 3;
                        textures[11] = 11;
                    }
                    break;

                case 62:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 8;
                        textures[4] = 3;
                        components[6] = 1;
                        textures[6] = 8;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 14;
                        textures[11] = 3;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_LOWR4"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 2;
                        textures[6] = 5;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 0;
                        textures[11] = 11;
                    }
                    break;

                case 63:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 0;
                        textures[4] = 5;
                        components[6] = 1;
                        textures[6] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_TEETH1_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB0_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 2;
                        textures[3] = 0;
                        components[4] = 0;
                        textures[4] = 8;
                        components[6] = 6;
                        textures[6] = 2;
                        components[7] = 6;
                        textures[7] = 2;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 2;
                        textures[11] = 7;
                    }
                    break;

                case 0:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 15;
                        textures[4] = 9;
                        components[6] = 12;
                        textures[6] = 12;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_TEETH1_1"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 0;
                        textures[11] = 2;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 3;
                        textures[3] = 0;
                        components[4] = 11;
                        textures[4] = 10;
                        components[6] = 7;
                        textures[6] = 6;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_LTS_F_JBIB_1_4"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 1:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 1;
                        textures[3] = 0;
                        components[4] = 7;
                        textures[4] = 15;
                        components[6] = 1;
                        textures[6] = 1;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 1;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 7;
                        textures[11] = 1;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 3;
                        textures[4] = 8;
                        components[6] = 4;
                        textures[6] = 2;
                        components[7] = 1;
                        textures[7] = 0;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB1_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 2:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 5;
                        textures[4] = 1;
                        components[6] = 6;
                        textures[6] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_TEETH1_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 1;
                        textures[11] = 0;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 3;
                        textures[3] = 0;
                        components[4] = 3;
                        textures[4] = 15;
                        components[6] = 3;
                        textures[6] = 15;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 3;
                        textures[11] = 1;
                    }
                    break;

                case 3:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        components[4] = 7;
                        textures[4] = 4;
                        components[6] = 12;
                        textures[6] = 4;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_TEETH1_1"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 5;
                        textures[11] = 0;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_0"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 11;
                        textures[6] = 1;
                        components[7] = 2;
                        textures[7] = 4;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 5;
                        textures[11] = 9;
                    }
                    break;

                case 4:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 6;
                        textures[3] = 0;
                        components[4] = 5;
                        textures[4] = 12;
                        components[6] = 12;
                        textures[6] = 12;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_TEETH1_2"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 5;
                        textures[8] = 2;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 3;
                        textures[11] = 12;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 3;
                        textures[3] = 0;
                        components[4] = 11;
                        textures[4] = 0;
                        components[6] = 3;
                        textures[6] = 1;
                        components[7] = 1;
                        textures[7] = 0;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 3;
                        textures[11] = 12;
                    }
                    break;

                case 5:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 1;
                        textures[3] = 0;
                        components[4] = 15;
                        textures[4] = 12;
                        components[6] = 6;
                        textures[6] = 0;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 0;
                        textures[8] = 2;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 7;
                        textures[11] = 1;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 3;
                        textures[4] = 1;
                        components[6] = 4;
                        textures[6] = 1;
                        components[7] = 2;
                        textures[7] = 1;
                        components[8] = 2;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 5;
                        textures[11] = 0;
                    }
                    break;

                case 6:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 1;
                        textures[4] = 15;
                        components[6] = 7;
                        textures[6] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_TEETH1_1"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 0;
                        textures[11] = 2;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 11;
                        textures[4] = 14;
                        components[6] = 3;
                        textures[6] = 8;
                        components[7] = 2;
                        textures[7] = 2;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB1_1"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 7:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 6;
                        textures[3] = 0;
                        components[4] = 12;
                        textures[4] = 7;
                        components[6] = 7;
                        textures[6] = 0;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 5;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 3;
                        textures[11] = 0;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_LTS_F_LEGS_0_1"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_PILOT_F_FEET_0_0"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 5;
                        textures[7] = 4;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 4;
                        textures[11] = 14;
                    }
                    break;

                case 8:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_LEGS0_2"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 10;
                        textures[6] = 0;
                        components[7] = 12;
                        textures[7] = 2;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_ACCS0_0"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_JBIB2_2"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        components[4] = 8;
                        textures[4] = 4;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET0_3"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH0_3"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 13;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB4_8"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 9:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 4;
                        textures[4] = 2;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_FEET0_5"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH4_2"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 4;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB1_4"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 9;
                        textures[4] = 15;
                        components[6] = 6;
                        textures[6] = 0;
                        components[7] = 4;
                        textures[7] = 3;
                        components[8] = 2;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 13;
                        textures[11] = 6;
                    }
                    break;

                case 10:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 4;
                        textures[4] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_FEET0_7"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH4_2"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 4;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB1_3"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_LEGS0_0"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET1_2"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 7;
                        textures[7] = 1;
                        components[8] = 13;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB0_9"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 11:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 12;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_LEGS0_0"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 10;
                        textures[6] = 0;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_ACCS1_0"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_JBIB0_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        components[4] = 8;
                        textures[4] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET0_4"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 6;
                        textures[7] = 1;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_ACCS0_1"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB0_6"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 12:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 6;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_0"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_FEET0_3"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 5;
                        textures[8] = 2;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 4;
                        textures[11] = 14;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        components[4] = 9;
                        textures[4] = 6;
                        components[6] = 14;
                        textures[6] = 11;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 13;
                        textures[8] = 3;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 8;
                        textures[11] = 0;
                    }
                    break;

                case 13:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 12;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_LEGS0_5"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 10;
                        textures[6] = 0;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_ACCS1_0"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_JBIB0_5"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 6;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_LEGS1_8"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 0;
                        textures[6] = 0;
                        components[7] = 1;
                        textures[7] = 0;
                        components[8] = 13;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB1_9"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 14:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 6;
                        textures[3] = 0;
                        components[4] = 4;
                        textures[4] = 0;
                        components[6] = 10;
                        textures[6] = 0;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 5;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 4;
                        textures[11] = 0;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        components[4] = 8;
                        textures[4] = 12;
                        components[6] = 8;
                        textures[6] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH1_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 5;
                        textures[8] = 7;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 8;
                        textures[11] = 1;
                    }
                    break;

                case 15:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 12;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_LEGS0_0"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 10;
                        textures[6] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_TEETH1_2"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_ACCS4_1"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_JBIB1_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        components[4] = 9;
                        textures[4] = 8;
                        components[6] = 6;
                        textures[6] = 0;
                        components[7] = 6;
                        textures[7] = 0;
                        components[8] = 0;
                        textures[8] = 10;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 6;
                        textures[11] = 0;
                    }
                    break;

                case 16:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 11;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_3"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET0_7"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 11;
                        textures[7] = 2;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB9_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_6"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_FEET1_5"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_TEETH0_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_ACCS1_0"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB0_6"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 17:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 1;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_11"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET0_8"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_ACCS1_0"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB2_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 8;
                        textures[4] = 11;
                        components[6] = 7;
                        textures[6] = 0;
                        components[7] = 9;
                        textures[7] = 0;
                        components[8] = 2;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 4;
                        textures[11] = 14;
                    }
                    break;

                case 18:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS1_4"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 1;
                        textures[6] = 9;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_VAL_M_JBIB2_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_10"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_FEET1_10"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_TEETH0_2"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUS2_F_ACCS0_1"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB0_3"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 19:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 1;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_7"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET1_6"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_ACCS6_3"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB2_3"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 0;
                        textures[4] = 1;
                        components[6] = 6;
                        textures[6] = 0;
                        components[7] = 2;
                        textures[7] = 0;
                        components[8] = 2;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 13;
                        textures[11] = 7;
                    }
                    break;

                case 20:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 8;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_0"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET0_3"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_TEETH0_3"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB5_4"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 6;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_2"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 3;
                        textures[6] = 10;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_ACCS1_3"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB3_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 21:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 1;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_6"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET0_0"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_ACCS1_1"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB2_2"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_11"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_FEET1_4"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_TEETH0_3"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_ACCS1_6"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB4_3"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 22:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_1"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 7;
                        textures[6] = 0;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB11_3"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_7"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET0_1"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 4;
                        textures[7] = 2;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_ACCS1_1"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB0_6"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 23:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 14;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_8"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET0_11"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_VAL_M_ACCS2_0"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB2_6"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 0;
                        textures[4] = 7;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET1_5"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH1_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB2_4"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 24:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        components[4] = 15;
                        textures[4] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_FEET0_5"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_JBIB1_4"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 15;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS0_2"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_FEET009"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 1;
                        textures[7] = 2;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_JBIB2_10"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 25:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_LOWR2_2"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 1;
                        textures[6] = 3;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 1;
                        textures[11] = 0;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_LOWR0"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 15;
                        textures[6] = 1;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_TEETH0_1"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_JBIB1_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 26:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_LOWR0_7"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_FEET0_6"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_JBIB1_5"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 11;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_LOWR107"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_FEET008"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH1_2"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB5_3"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 27:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 15;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_LOWR2_11"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 5;
                        textures[6] = 3;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 15;
                        textures[11] = 0;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 15;
                        textures[3] = 0;
                        components[4] = 12;
                        textures[4] = 14;
                        components[6] = 3;
                        textures[6] = 13;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH1_1"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_JBIB2_9"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 28:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_LOWR0_2"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_FEET0_8"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_TEETH1_1"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_JBIB1_3"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 9;
                        textures[3] = 0;
                        components[4] = 14;
                        textures[4] = 8;
                        components[6] = 13;
                        textures[6] = 5;
                        components[7] = 4;
                        textures[7] = 3;
                        components[8] = 2;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 9;
                        textures[11] = 3;
                    }
                    break;

                case 29:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_LOWR2_3"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_FEET0_4"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 5;
                        textures[11] = 7;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 15;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_LOWR8"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_FEET001"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 9;
                        textures[7] = 0;
                        components[8] = 2;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_JBIB2_6"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 30:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 15;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_LOWR0_1"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 1;
                        textures[6] = 7;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_TEETH0_1"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 15;
                        textures[11] = 0;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 11;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_LOWR104"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_FEET007"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 3;
                        textures[7] = 1;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 11;
                        textures[11] = 10;
                    }
                    break;

                case 31:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 6;
                        textures[4] = 10;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_FEET0_6"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB11_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_LOWR10"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_FEET003"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH0_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_ACCS0_4"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB0_5"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 32:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 12;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_LEGS0_5"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_FEET1_10"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH0_12"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_ACCS3_13"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB1_5"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_LEGS0_4"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 0;
                        textures[6] = 0;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 13;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB0_2"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 33:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 12;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_LEGS1_1"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 10;
                        textures[6] = 0;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_ACCS1_0"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI2_M_JBIB2_1"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_LEGS1_4"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET0_0"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_TEETH0_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 2;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB3_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 34:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 11;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_LEGS0_7"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_FEET0_2"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH4_10"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 6;
                        textures[8] = 11;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB2_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_LEGS0_0"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET0_3"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 1;
                        textures[7] = 1;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_ACCS2_5"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB0_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 35:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 10;
                        textures[4] = 0;
                        components[6] = 10;
                        textures[6] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH4_2"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 4;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB5_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 6;
                        textures[3] = 0;
                        components[4] = 6;
                        textures[4] = 0;
                        components[6] = 13;
                        textures[6] = 0;
                        components[7] = 6;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_ACCS0_3"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 7;
                        textures[11] = 0;
                    }
                    break;

                case 36:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 11;
                        textures[3] = 0;
                        components[4] = 10;
                        textures[4] = 0;
                        components[6] = 11;
                        textures[6] = 12;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH0_11"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB3_2"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 7;
                        textures[4] = 2;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET0_9"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 1;
                        textures[7] = 1;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_ACCS1_3"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB4_10"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 37:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_LEGS1_8"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_FEET0_8"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 12;
                        textures[7] = 2;
                        components[8] = 10;
                        textures[8] = 2;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB0_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        components[4] = 6;
                        textures[4] = 2;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET1_7"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 6;
                        textures[7] = 4;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_ACCS2_9"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 6;
                        textures[11] = 2;
                    }
                    break;

                case 38:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        components[4] = 10;
                        textures[4] = 2;
                        components[6] = 10;
                        textures[6] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH4_14"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 4;
                        textures[8] = 2;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB5_2"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 6;
                        textures[3] = 0;
                        components[4] = 7;
                        textures[4] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET1_0"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 6;
                        textures[7] = 0;
                        components[8] = 13;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB1_2"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 39:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 11;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_LEGS1_8"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_FEET1_6"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH4_13"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 6;
                        textures[8] = 12;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB2_9"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_LEGS0_4"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET0_8"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_TEETH0_2"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 2;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB3_4"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 40:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 1;
                        textures[3] = 0;
                        components[4] = 3;
                        textures[4] = 15;
                        components[6] = 2;
                        textures[6] = 6;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 0;
                        textures[8] = 3;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 3;
                        textures[11] = 15;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 14;
                        textures[3] = 0;
                        components[4] = 2;
                        textures[4] = 0;
                        components[6] = 10;
                        textures[6] = 2;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 14;
                        textures[11] = 7;
                    }
                    break;

                case 41:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 5;
                        textures[4] = 6;
                        components[6] = 2;
                        textures[6] = 6;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 1;
                        textures[11] = 0;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 11;
                        textures[3] = 0;
                        components[4] = 2;
                        textures[4] = 2;
                        components[6] = 10;
                        textures[6] = 2;
                        components[7] = 3;
                        textures[7] = 3;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 11;
                        textures[11] = 0;
                    }
                    break;

                case 42:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 15;
                        textures[4] = 8;
                        components[6] = 2;
                        textures[6] = 13;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 9;
                        textures[11] = 4;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 14;
                        textures[3] = 0;
                        components[4] = 12;
                        textures[4] = 8;
                        components[6] = 10;
                        textures[6] = 3;
                        components[7] = 3;
                        textures[7] = 4;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 14;
                        textures[11] = 10;
                    }
                    break;

                case 43:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 8;
                        textures[3] = 0;
                        components[4] = 14;
                        textures[4] = 1;
                        components[6] = 2;
                        textures[6] = 13;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB5_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 7;
                        textures[3] = 0;
                        components[4] = 14;
                        textures[4] = 9;
                        components[6] = 11;
                        textures[6] = 0;
                        components[7] = 2;
                        textures[7] = 4;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 10;
                        textures[11] = 0;
                    }
                    break;

                case 44:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 1;
                        textures[3] = 0;
                        components[4] = 7;
                        textures[4] = 4;
                        components[6] = 7;
                        textures[6] = 1;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 0;
                        textures[8] = 7;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 7;
                        textures[11] = 5;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 14;
                        textures[3] = 0;
                        components[4] = 14;
                        textures[4] = 8;
                        components[6] = 3;
                        textures[6] = 1;
                        components[7] = 3;
                        textures[7] = 5;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 14;
                        textures[11] = 0;
                    }
                    break;

                case 45:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 1;
                        textures[3] = 0;
                        components[4] = 3;
                        textures[4] = 4;
                        components[6] = 7;
                        textures[6] = 15;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_ACCS4_3"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 7;
                        textures[11] = 4;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 7;
                        textures[3] = 0;
                        components[4] = 10;
                        textures[4] = 2;
                        components[6] = 1;
                        textures[6] = 13;
                        components[7] = 1;
                        textures[7] = 1;
                        components[8] = 5;
                        textures[8] = 9;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 10;
                        textures[11] = 10;
                    }
                    break;

                case 46:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_M_LOWR2_1"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 9;
                        textures[6] = 7;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB6_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 14;
                        textures[3] = 0;
                        components[4] = 12;
                        textures[4] = 0;
                        components[6] = 4;
                        textures[6] = 1;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 14;
                        textures[11] = 4;
                    }
                    break;

                case 47:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        components[4] = 6;
                        textures[4] = 0;
                        components[6] = 9;
                        textures[6] = 0;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 9;
                        textures[11] = 10;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 7;
                        textures[3] = 0;
                        components[4] = 2;
                        textures[4] = 2;
                        components[6] = 11;
                        textures[6] = 1;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 1;
                        textures[8] = 8;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 10;
                        textures[11] = 7;
                    }
                    break;

                case 48:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 11;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_9"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET1_3"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 11;
                        textures[7] = 2;
                        components[8] = 6;
                        textures[8] = 12;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB7_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_LEGS1_0"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET1_9"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 6;
                        textures[7] = 1;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_ACCS0_3"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB0_10"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 49:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS2_2"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET1_14"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_TEETH0_4"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB0_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_5"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 8;
                        textures[6] = 3;
                        components[7] = 2;
                        textures[7] = 5;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 13;
                        textures[11] = 8;
                    }
                    break;

                case 50:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_LEGS0_11"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_FEET0_6"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH4_4"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 4;
                        textures[8] = 1;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB1_0"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_13"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_FEET1_3"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_TEETH0_2"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_ACCS1_8"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB4_1"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 51:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 0;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS1_6"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 12;
                        textures[6] = 15;
                        components[7] = 0;
                        textures[7] = 0;
                        components[8] = 15;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_VAL_M_JBIB2_1"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_8"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_FEET1_7"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH1_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 2;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB2_2"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 52:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_LEGS0_12"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 10;
                        textures[6] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH4_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_ACCS1_12"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB0_3"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 7;
                        textures[3] = 0;
                        components[4] = 0;
                        textures[4] = 14;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET0_4"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 1;
                        textures[7] = 0;
                        components[8] = 1;
                        textures[8] = 8;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        components[11] = 10;
                        textures[11] = 15;
                    }
                    break;

                case 53:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS2_12"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_FEET0_2"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 12;
                        textures[7] = 2;
                        components[8] = 10;
                        textures[8] = 14;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB2_4"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 5;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_LEGS1_5"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET1_7"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH1_3"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 0;
                        textures[8] = 15;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB0_8"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 54:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 12;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS1_1"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET1_10"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_TEETH1_6"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_ACCS1_6"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_JBIB2_2"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 12;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_LEGS2_9"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_FEET1_8"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH0_1"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_JBIB2_11"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;

                case 55:
                    if (isMale)
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 14;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_LEGS0_10"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_M_FEET1_8"));
                        components[6] = Var1.Drawable;
                        textures[6] = Var1.Texture;
                        components[7] = 0;
                        textures[7] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_VAL_M_ACCS2_0"));
                        components[8] = Var1.Drawable;
                        textures[8] = Var1.Texture;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_M_JBIB1_2"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    else
                    {
                        components[1] = 0;
                        textures[1] = 0;
                        components[3] = 4;
                        textures[3] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BUSI_F_LEGS1_9"));
                        components[4] = Var1.Drawable;
                        textures[4] = Var1.Texture;
                        components[6] = 8;
                        textures[6] = 8;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_BEACH_F_TEETH1_0"));
                        components[7] = Var1.Drawable;
                        textures[7] = Var1.Texture;
                        components[8] = 3;
                        textures[8] = 0;
                        components[9] = 0;
                        textures[9] = 0;
                        components[10] = 0;
                        textures[10] = 0;
                        Var1 = ShopPed.GetShopPedComponent(Funzioni.HashUint("DLC_MP_HIPS_F_JBIB2_8"));
                        components[11] = Var1.Drawable;
                        textures[11] = Var1.Texture;
                    }
                    break;
            }
            return new int[][] { new int[12] { components[0], components[1], components[2], components[3], components[4], components[5], components[6], components[7], components[8], components[9], components[10], components[11] }, new int[12] { textures[0], textures[1], textures[2], textures[3], textures[4], textures[5], textures[6], textures[7], textures[8], textures[9], textures[10], textures[11] } };
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