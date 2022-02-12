using TheLastPlanet.Client.Core.Utility;

namespace TheLastPlanet.Client.Races.Creator
{
    static class RaceCreatorHelper
    {

        #region GESTIONE PROPS E CATEGORIE

        public static bool func_8935()
        {
            var dlc = -1762644250;
            return IsDlcPresent((uint)dlc);
        }

        public static int GetModel(int iParam0, int iParam1) // provvisorio
        {
            switch (iParam0)
            {
                case 0:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_const_fence02b;

                        case 1:
                            return (int)RacingProps.prop_offroad_bale03;

                        case 2:
                            return (int)RacingProps.prop_offroad_bale02;

                        case 3:
                            return (int)RacingProps.prop_offroad_bale01;

                        case 4:
                            return (int)RacingProps.lts_prop_lts_offroad_tyres01;

                        case 5:
                            return (int)RacingProps.prop_offroad_tyres02;

                        case 6:
                            return (int)RacingProps.prop_barier_conc_02a;

                        case 7:
                            return (int)RacingProps.prop_barier_conc_05c;

                        case 8:
                            return (int)RacingProps.prop_barier_conc_05a;

                        case 9:
                            return (int)RacingProps.prop_barier_conc_05b;

                        case 10:
                            return (int)RacingProps.prop_barier_conc_01a;

                        case 11:
                            return (int)RacingProps.prop_barrier_work05;

                        case 12:
                            return (int)RacingProps.prop_fnclink_03gate5;

                        case 13:
                            return (int)RacingProps.prop_fnclink_02gate3;

                        case 14:
                            return (int)RacingProps.prop_mb_sandblock_01;

                        case 15:
                            return (int)RacingProps.prop_mb_sandblock_02;

                        case 16:
                            return (int)RacingProps.prop_mb_sandblock_05_cr;

                        case 17:
                            return (int)RacingProps.prop_mb_sandblock_04;

                        case 18:
                            return (int)RacingProps.prop_mb_sandblock_03_cr;

                        case 19:
                            return (int)RacingProps.prop_mb_hesco_06;

                        case 20:
                            return (int)RacingProps.prop_plas_barier_01a;

                        case 21:
                            return (int)RacingProps.prop_barier_conc_02b;

                        case 22:
                            return (int)RacingProps.prop_barrier_work06a;

                        case 23:
                            return (int)RacingProps.prop_barrier_work04a;

                        case 24:
                            return (int)RacingProps.prop_fnclink_06a;

                        case 25:
                            return (int)RacingProps.prop_fnclink_06b;

                        case 26:
                            return (int)RacingProps.prop_fnclink_06c;

                        case 27:
                            return (int)RacingProps.prop_fnclink_06d;

                        case 28:
                            return (int)RacingProps.prop_fnccorgm_03a;

                        case 29:
                            return (int)RacingProps.prop_fnccorgm_03b;

                        case 30:
                            return (int)RacingProps.prop_fnccorgm_03c;

                        case 31:
                            return (int)RacingProps.prop_fnccorgm_02a;

                        case 32:
                            return (int)RacingProps.prop_fnccorgm_02b;

                        case 33:
                            return (int)RacingProps.prop_fnccorgm_02c;

                        case 34:
                            return (int)RacingProps.prop_fnccorgm_02d;

                        case 35:
                            return (int)RacingProps.prop_fnccorgm_02e;

                        case 36:
                            return (int)RacingProps.prop_const_fence03a_cr;

                        case 37:
                            return (int)RacingProps.prop_gate_cult_01_l;

                        case 38:
                            return (int)RacingProps.prop_gate_cult_01_r;

                        case 39:
                            return (int)RacingProps.prop_const_fence03b_cr;

                        case 40:
                            return (int)RacingProps.prop_const_fence02a;

                        case 41:
                            return (int)RacingProps.prop_const_fence01b_cr;

                        case 42:
                            return (int)RacingProps.prop_fncwood_16b;

                        case 43:
                            return (int)RacingProps.prop_fncwood_16c;

                        case 44:
                            return (int)RacingProps.prop_fnc_farm_01b;

                        case 45:
                            return (int)RacingProps.prop_fnc_farm_01c;

                        case 46:
                            return (int)RacingProps.prop_fnc_farm_01d;

                        case 47:
                            return (int)RacingProps.prop_fnc_farm_01e;

                        case 48:
                            return (int)RacingProps.prop_fnc_farm_01f;

                        case 49:
                            return (int)RacingProps.prop_hayb_st_01_cr;

                        case 50:
                            return (int)RacingProps.prop_haybale_03;

                        case 51:
                            return (int)RacingProps.prop_haybale_02;

                        case 52:
                            return (int)RacingProps.prop_haybale_01;

                        case 53:
                            return (int)RacingProps.prop_tyre_wall_01;

                        case 54:
                            return (int)RacingProps.prop_tyre_wall_02;

                        case 55:
                            return (int)RacingProps.prop_tyre_wall_03;

                        case 56:
                            return (int)RacingProps.prop_tyre_wall_04;

                        case 57:
                            return (int)RacingProps.prop_tyre_wall_05;

                        case 58:
                            return (int)RacingProps.prop_tyre_wall_01b;

                        case 59:
                            return (int)RacingProps.prop_tyre_wall_02b;

                        case 60:
                            return (int)RacingProps.prop_tyre_wall_03b;

                        case 61:
                            return (int)RacingProps.prop_tyre_wall_01c;

                        case 62:
                            return (int)RacingProps.prop_tyre_wall_02c;

                        case 63:
                            return (int)RacingProps.prop_tyre_wall_03c;

                        case 64:
                            return (int)RacingProps.prop_start_gate_01b;

                        case 65:
                            return (int)RacingProps.prop_beachf_01_cr;

                        case 66:
                            return (int)RacingProps.prop_sec_gate_01d;

                        case 67:
                            return (int)RacingProps.prop_vault_shutter;

                        case 68:
                            return (int)RacingProps.vw_prop_vw_barrier_rope_01a;

                        case 69:
                            return (int)RacingProps.vw_prop_vw_barrier_rope_02a;

                        case 70:
                            return (int)RacingProps.ch_prop_ch_casino_shutter01x;

                        case 71:
                            return (int)RacingProps.ch_prop_ch_tunnel_door01a;

                        case 72:
                            return (int)RacingProps.h4_prop_h4_fence_seg_x1_01a;

                        case 73:
                            return (int)RacingProps.h4_prop_h4_fence_seg_x3_01a;

                        case 74:
                            return (int)RacingProps.h4_prop_h4_fence_seg_x5_01a;

                        case 75:
                            return (int)RacingProps.h4_prop_h4_fence_arches_x2_01a;

                        case 76:
                            return (int)RacingProps.h4_prop_h4_fence_arches_x3_01a;

                    }
                    break;

                case 1:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_bench_05;

                        case 1:
                            return (int)RacingProps.prop_bench_07;

                        case 2:
                            return (int)RacingProps.prop_bench_01a;

                        case 3:
                            return (int)RacingProps.prop_bench_08;

                        case 4:
                            return (int)RacingProps.prop_bleachers_04_cr;

                        case 5:
                            return (int)RacingProps.prop_bleachers_05_cr;

                    }
                    break;

                case 2:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_dock_bouy_1;

                        case 1:
                            return (int)RacingProps.prop_dock_bouy_2;

                        case 2:
                            return (int)RacingProps.prop_byard_float_02;

                        case 3:
                            return (int)RacingProps.prop_ind_barge_01_cr;

                        case 4:
                            if (func_8935())
                            {
                                return (int)RacingProps.xm_prop_x17_barge_01;
                            }
                            break;
                    }
                    break;

                case 3:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_elecbox_24;

                        case 1:
                            return (int)RacingProps.prop_elecbox_24b;

                        case 2:
                            return (int)RacingProps.prop_portacabin01;

                        case 3:
                            return (int)RacingProps.prop_air_monhut_03_cr;

                        case 4:
                            return (int)RacingProps.prop_air_sechut_01;

                        case 5:
                            return (int)RacingProps.prop_tollbooth_1;

                        case 6:
                            return (int)RacingProps.prop_parking_hut_2;

                        case 7:
                            return (int)RacingProps.prop_makeup_trail_02_cr;

                        case 8:
                            return (int)RacingProps.prop_makeup_trail_01_cr;

                        case 9:
                            if (func_8935())
                            {
                                return (int)RacingProps.xm_prop_x17_trail_01a;
                            }
                            break;

                        case 10:
                            if (func_8935())
                            {
                                return (int)RacingProps.xm_prop_x17_trail_02a;
                            }
                            break;
                    }
                    break;

                case 4:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_cementbags01;

                        case 1:
                            return (int)RacingProps.prop_conc_blocks01a;

                        case 2:
                            return (int)RacingProps.prop_cons_crate;

                        case 3:
                            return (int)RacingProps.prop_jyard_block_01a;

                        case 4:
                            return (int)RacingProps.prop_conc_sacks_02a;

                        case 5:
                            return (int)RacingProps.prop_byard_sleeper01;

                        case 6:
                            return (int)RacingProps.prop_shuttering01;

                        case 7:
                            return (int)RacingProps.prop_shuttering02;

                        case 8:
                            return (int)RacingProps.prop_shuttering03;

                        case 9:
                            return (int)RacingProps.prop_shuttering04;

                        case 10:
                            return (int)RacingProps.prop_woodpile_01a;

                        case 11:
                            return (int)RacingProps.prop_woodpile_01b;

                        case 12:
                            return (int)RacingProps.prop_woodpile_01c;

                        case 13:
                            return (int)RacingProps.prop_woodpile_03a;

                        case 14:
                            return (int)RacingProps.prop_conc_blocks01c;

                        case 15:
                            return (int)RacingProps.prop_cons_cements01;

                        case 16:
                            return (int)RacingProps.prop_pipes_conc_01;

                        case 17:
                            if (func_8935())
                            {
                                return (int)RacingProps.gr_prop_gr_bunkeddoor;
                            }
                            break;

                        case 18:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_worklight_01a;
                            }
                            break;

                        case 19:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_worklight_02a;
                            }
                            break;

                        case 20:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_worklight_03a;
                            }
                            break;

                        case 21:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_worklight_03b;
                            }
                            break;

                        case 22:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_worklight_04b;
                            }
                            break;

                        case 23:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_worklight_04d;
                            }
                            break;

                        case 24:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_ind_coalcar_02;
                            }
                            break;

                        case 25:
                            if (func_8935())
                            {
                                return (int)RacingProps.xm_prop_x17_osphatch_27m;
                            }
                            break;

                        case 26:
                            if (func_8935())
                            {
                                return (int)RacingProps.gr_prop_gr_bench_03a;
                            }
                            break;

                        case 27:
                            if (func_8935())
                            {
                                return (int)RacingProps.gr_prop_gr_bench_04a;
                            }
                            break;

                        case 28:
                            if (func_8935())
                            {
                                return (int)RacingProps.gr_prop_gr_bench_04b;
                            }
                            break;

                        case 29:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_pallettruck_01;
                            }
                            break;

                        case 30:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_rub_cardpile_04;
                            }
                            break;

                        case 31:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_rub_cardpile_06;
                            }
                            break;

                        case 32:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_skid_box_01;
                            }
                            break;

                        case 33:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_cons_plyboard_01;
                            }
                            break;

                        case 34:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_cons_plank;
                            }
                            break;

                        case 35:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_barrier_work01c;
                            }
                            break;

                        case 36:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_food_cb_tray_01;
                            }
                            break;

                        case 37:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_food_bs_tray_06;
                            }
                            break;

                        case 38:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_cs_envolope_01;
                            }
                            break;

                        case 39:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_cs_binder_01;
                            }
                            break;
                    }
                    break;

                case 5:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_rub_cont_01b;

                        case 1:
                            return (int)RacingProps.prop_rail_boxcar4;

                        case 2:
                            return (int)RacingProps.prop_rub_railwreck_2;

                        case 3:
                            return (int)RacingProps.prop_contr_03b_ld;

                        case 4:
                            return (int)RacingProps.prop_container_ld2;

                        case 5:
                            return (int)RacingProps.prop_rail_boxcar;

                        case 6:
                            return (int)RacingProps.prop_rail_boxcar3;

                        case 7:
                            return (int)RacingProps.prop_container_01mb;

                        case 8:
                            return (int)RacingProps.prop_container_03mb;

                        case 9:
                            return (int)RacingProps.sm_prop_smug_cont_01a;

                    }
                    break;

                case 6:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_byard_floatpile;

                        case 1:
                            return (int)RacingProps.prop_boxpile_07a;

                        case 2:
                            return (int)RacingProps.prop_watercrate_01;

                        case 3:
                            return (int)RacingProps.prop_box_wood01a;

                        case 4:
                            return (int)RacingProps.prop_box_wood03a;

                        case 5:
                            return (int)RacingProps.prop_box_wood04a;

                        case 6:
                            return (int)RacingProps.prop_cash_crate_01;

                        case 7:
                            return (int)RacingProps.prop_mb_cargo_03a;

                        case 8:
                            return (int)RacingProps.prop_mb_cargo_04a;

                        case 9:
                            return (int)RacingProps.prop_air_cargo_04a;

                        case 10:
                            return (int)RacingProps.prop_mb_crate_01b;

                        case 11:
                            return (int)RacingProps.prop_air_cargo_01a;

                        case 12:
                            return (int)RacingProps.prop_mb_cargo_04b;

                        case 13:
                            return (int)RacingProps.prop_mb_cargo_02a;

                        case 14:
                            return (int)RacingProps.prop_pallet_pile_02;

                        case 15:
                            return (int)RacingProps.prop_pallet_pile_01;

                        case 16:
                            return (int)RacingProps.prop_cratepile_07a;

                        case 17:
                            return (int)RacingProps.prop_boxpile_04a;

                        case 18:
                            return (int)RacingProps.vw_prop_vw_barrel_01a;

                        case 19:
                            return (int)RacingProps.vw_prop_vw_barrel_pile_01a;

                        case 20:
                            return (int)RacingProps.vw_prop_vw_barrel_pile_02a;

                        case 21:
                            if (func_8935())
                            {
                                return (int)RacingProps.imp_prop_impexp_boxpile_01;
                            }
                            break;

                        case 22:
                            if (func_8935())
                            {
                                return (int)RacingProps.hei_prop_carrier_cargo_01a;
                            }
                            break;

                        case 23:
                            if (func_8935())
                            {
                                return (int)RacingProps.sr_prop_sr_boxpile_03;
                            }
                            break;

                        case 24:
                            if (func_8935())
                            {
                                return (int)RacingProps.xm_prop_rsply_crate04a;
                            }
                            break;

                        case 25:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_mb_crate_01a;
                            }
                            break;
                    }
                    break;

                case 7:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_bin_13a;

                        case 1:
                            return (int)RacingProps.prop_bin_14a;

                        case 2:
                            return (int)RacingProps.prop_dumpster_3a;

                        case 3:
                            return (int)RacingProps.prop_dumpster_4b;

                        case 4:
                            return (int)RacingProps.prop_dumpster_01a;

                        case 5:
                            return (int)RacingProps.prop_dumpster_02a;

                        case 6:
                            return (int)RacingProps.prop_dumpster_02b;

                        case 7:
                            return (int)RacingProps.prop_skip_06a;

                    }
                    break;

                case 8:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_elecbox_02a;

                        case 1:
                            return (int)RacingProps.prop_elecbox_16;

                        case 2:
                            return (int)RacingProps.prop_elecbox_14;

                        case 3:
                            return (int)RacingProps.prop_elecbox_10_cr;

                        case 4:
                            return (int)RacingProps.prop_elecbox_15_cr;

                        case 5:
                            return (int)RacingProps.prop_elecbox_17_cr;

                        case 6:
                            return (int)RacingProps.prop_ind_deiseltank;

                        case 7:
                            return (int)RacingProps.prop_ind_mech_02a;

                        case 8:
                            return (int)RacingProps.prop_ind_mech_02b;

                        case 9:
                            return (int)RacingProps.prop_sub_trans_01a;

                        case 10:
                            return (int)RacingProps.prop_sub_trans_02a;

                        case 11:
                            return (int)RacingProps.prop_sub_trans_04a;

                        case 12:
                            return (int)RacingProps.prop_generator_03b;

                        case 13:
                            return (int)RacingProps.prop_feeder1_cr;

                        case 14:
                            return (int)RacingProps.prop_set_generator_01_cr;

                        case 15:
                            return (int)RacingProps.sm_prop_smug_jammer;

                    }
                    break;

                case 9:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.lts_prop_lts_ramp_01;

                        case 1:
                            return (int)RacingProps.prop_skip_04;

                        case 2:
                            return (int)RacingProps.lts_prop_lts_ramp_02;

                        case 3:
                            return (int)RacingProps.lts_prop_lts_ramp_03;

                        case 4:
                            return (int)RacingProps.prop_skip_08a;

                        case 5:
                            return (int)RacingProps.prop_skip_03;

                        case 6:
                            return (int)RacingProps.prop_jetski_ramp_01;

                        case 7:
                            return (int)RacingProps.ar_prop_ar_jetski_ramp_01_dev;

                        case 8:
                            return (int)RacingProps.prop_byard_rampold_cr;

                        case 9:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_water_ramp_01;
                            }
                            break;

                        case 10:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_water_ramp_02;
                            }
                            break;

                        case 11:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_water_ramp_03;
                            }
                            break;
                    }
                    break;

                case 10:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_trafficdiv_01;

                        case 1:
                            return (int)RacingProps.prop_sign_road_09a;

                        case 2:
                            return (int)RacingProps.prop_sign_road_09b;

                        case 3:
                            return (int)RacingProps.prop_sign_road_09c;

                        case 4:
                            return (int)RacingProps.prop_sign_road_09d;

                        case 5:
                            return (int)RacingProps.prop_sign_road_06q;

                        case 6:
                            return (int)RacingProps.prop_sign_road_06r;

                        case 7:
                            return (int)RacingProps.prop_sign_road_05c;

                        case 8:
                            return (int)RacingProps.prop_sign_road_05d;

                        case 9:
                            return (int)RacingProps.prop_sign_road_05e;

                        case 10:
                            return (int)RacingProps.prop_sign_road_05f;

                        case 11:
                            return (int)RacingProps.prop_air_taxisign_01a;

                    }
                    break;

                case 11:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_food_van_01;

                        case 1:
                            return (int)RacingProps.prop_food_van_02;

                        case 2:
                            return (int)RacingProps.prop_tanktrailer_01a;

                        case 3:
                            return (int)RacingProps.prop_truktrailer_01a;

                        case 4:
                            return (int)RacingProps.prop_old_farm_03;

                    }
                    break;

                case 12:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_rub_buswreck_01;

                        case 1:
                            return (int)RacingProps.prop_rub_buswreck_06;

                        case 2:
                            return (int)RacingProps.prop_rub_carwreck_11;

                        case 3:
                            return (int)RacingProps.prop_rub_carwreck_12;

                        case 4:
                            return (int)RacingProps.prop_rub_carwreck_13;

                        case 5:
                            return (int)RacingProps.prop_rub_carwreck_14;

                        case 6:
                            return (int)RacingProps.prop_rub_carwreck_9;

                        case 7:
                            return (int)RacingProps.prop_rub_couch02;

                        case 8:
                            if (func_8935())
                            {
                                return (int)RacingProps.prop_crashed_heli;
                            }
                            break;

                        case 9:
                            return (int)RacingProps.prop_rub_carwreck_2;

                        case 10:
                            return (int)RacingProps.prop_rub_carwreck_3;

                        case 11:
                            return (int)RacingProps.prop_rub_carwreck_5;

                        case 12:
                            return (int)RacingProps.prop_rub_carwreck_7;

                        case 13:
                            return (int)RacingProps.prop_rub_carwreck_8;

                        case 14:
                            return (int)RacingProps.prop_rub_carwreck_10;

                        case 15:
                            return (int)RacingProps.prop_rub_carwreck_15;

                        case 16:
                            return (int)RacingProps.prop_rub_carwreck_16;
                    }
                    break;

                case 13:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_plant_group_04_cr;

                        case 1:
                            return (int)RacingProps.prop_bush_lrg_01c_cr;

                        case 2:
                            return (int)RacingProps.prop_bush_lrg_01e_cr2;

                        case 3:
                            return (int)RacingProps.prop_bush_med_03_cr2;

                        case 4:
                            return (int)RacingProps.prop_joshua_tree_01a;

                        case 5:
                            return (int)RacingProps.prop_cactus_01e;

                        case 6:
                            return (int)RacingProps.prop_pot_plant_05c;

                        case 7:
                            return (int)RacingProps.prop_pot_plant_04c;

                        case 8:
                            return (int)RacingProps.prop_pot_plant_05d;

                        case 9:
                            return (int)RacingProps.prop_pot_plant_03b_cr2;

                        case 10:
                            return (int)RacingProps.prop_pot_plant_04b;

                        case 11:
                            return (int)RacingProps.prop_rock_4_big2;

                        case 12:
                            return (int)RacingProps.prop_rock_4_big;

                        case 13:
                            return (int)RacingProps.prop_rock_4_c_2;

                        case 14:
                            return (int)RacingProps.prop_tree_olive_cr2;

                        case 15:
                            return (int)RacingProps.prop_tree_eng_oak_cr2;

                        case 16:
                            return (int)RacingProps.prop_log_break_01;

                        case 17:
                            return (int)RacingProps.prop_tree_fallen_02;

                        case 18:
                            return (int)RacingProps.sum_prop_ac_rock_01a;

                        case 19:
                            return (int)RacingProps.sum_prop_ac_rock_01b;

                        case 20:
                            return (int)RacingProps.sum_prop_ac_rock_01c;

                        case 21:
                            return (int)RacingProps.sum_prop_ac_rock_01d;

                        case 22:
                            return (int)RacingProps.sum_prop_ac_rock_01e;

                    }
                    break;

                case 14:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_offroad_barrel01;

                        case 1:
                            return (int)RacingProps.prop_offroad_barrel02;

                        case 2:
                            return (int)RacingProps.prop_barrel_exp_01a;

                        case 3:
                            return (int)RacingProps.prop_fire_exting_1b;

                        case 4:
                            return (int)RacingProps.prop_roadcone02c;

                        case 5:
                            return (int)RacingProps.prop_roadcone02a;

                        case 6:
                            return (int)RacingProps.prop_roadcone01a;

                        case 7:
                            return (int)RacingProps.prop_roadpole_01a;

                        case 8:
                            return (int)RacingProps.prop_postbox_01a;

                        case 9:
                            return (int)RacingProps.prop_news_disp_02d;

                        case 10:
                            return (int)RacingProps.prop_vend_water_01;

                        case 11:
                            return (int)RacingProps.prop_vend_snak_01_tu;

                        case 12:
                            return (int)RacingProps.prop_train_ticket_02_tu;

                        case 13:
                            return (int)RacingProps.prop_boxpile_02b;

                        case 14:
                            return (int)RacingProps.prop_mc_conc_barrier_01;

                        case 15:
                            return (int)RacingProps.prop_fncsec_03b;

                        case 16:
                            return (int)RacingProps.prop_table_08_side;

                        case 17:
                            return (int)RacingProps.prop_container_ld_pu;

                        case 18:
                            return (int)RacingProps.prop_mb_ordnance_02;

                        case 19:
                            return (int)RacingProps.prop_storagetank_02b;

                        case 20:
                            return (int)RacingProps.prop_logpile_01;

                        case 21:
                            return (int)RacingProps.prop_logpile_03;

                        case 22:
                            return (int)RacingProps.prop_pipes_02b;

                        case 23:
                            return (int)RacingProps.prop_barrel_pile_01;

                        case 24:
                            return (int)RacingProps.prop_barrel_exp_01b;

                        case 25:
                            return (int)RacingProps.prop_barrel_exp_01c;

                        case 26:
                            return (int)RacingProps.prop_gas_tank_02a;

                        case 27:
                            return (int)RacingProps.prop_gas_tank_04a;

                        case 28:
                            return (int)RacingProps.prop_gas_tank_02b;

                        case 29:
                            return (int)RacingProps.prop_jerrycan_01a;

                        case 30:
                            return (int)RacingProps.prop_gascyl_01a;

                        case 31:
                            return (int)RacingProps.prop_gascyl_04a;

                        case 32:
                            return (int)RacingProps.prop_gascyl_03a;

                        case 33:
                            return (int)RacingProps.prop_gascyl_03b;

                        case 34:
                            return (int)RacingProps.prop_gascyl_02a;

                        case 35:
                            return (int)RacingProps.prop_gascyl_02b;

                        case 36:
                            return (int)RacingProps.prop_fruitstand_b;

                        case 37:
                            return (int)RacingProps.prop_rub_tyre_03;

                        case 38:
                            return (int)RacingProps.stt_prop_stunt_bowling_pin;

                        case 39:
                            return (int)RacingProps.stt_prop_stunt_bowling_ball;

                        case 40:
                            return (int)RacingProps.stt_prop_stunt_soccer_sball;

                        case 41:
                            return (int)RacingProps.stt_prop_stunt_soccer_ball;

                        case 42:
                            return (int)RacingProps.stt_prop_stunt_soccer_lball;

                        case 43:
                            return (int)RacingProps.imp_prop_impexp_boxpile_01;

                        case 44:
                            return (int)RacingProps.imp_prop_groupbarrel_01;

                        case 45:
                            return (int)RacingProps.imp_prop_groupbarrel_02;

                        case 46:
                            return (int)RacingProps.imp_prop_groupbarrel_03;

                        case 47:
                            return (int)RacingProps.sr_prop_sr_boxwood_01;

                        case 48:
                            return (int)RacingProps.sr_prop_sr_boxpile_02;

                        case 49:
                            return (int)RacingProps.sr_prop_sr_boxpile_03;

                        case 50:
                            return (int)RacingProps.sr_prop_sr_track_wall;

                        case 51:
                            return (int)RacingProps.sr_prop_sr_tube_wall;

                        case 52:
                            return (int)RacingProps.sr_mp_spec_races_ammu_sign;

                        case 53:
                            return (int)RacingProps.ar_prop_ar_ammu_sign;

                        case 54:
                            return (int)RacingProps.xm_prop_x17_mine_01a;

                        case 55:
                            return (int)RacingProps.xm_prop_x17_mine_02a;

                        case 56:
                            return (int)RacingProps.xm_prop_x17_mine_03a;

                        case 57:
                            return (int)RacingProps.p_ld_stinger_s;

                        case 58:
                            return (int)RacingProps.xs_prop_arena_bomb_s;

                        case 59:
                            return (int)RacingProps.xs_prop_arena_bomb_m;

                        case 60:
                            return (int)RacingProps.xs_prop_arena_bomb_l;

                        case 61:
                            return (int)RacingProps.prop_wine_red;

                        case 62:
                            return (int)RacingProps.prop_drink_redwine;

                        case 63:
                            return (int)RacingProps.prop_bikerset;

                        case 64:
                            return (int)RacingProps.ch_prop_ch_room_trolly_01a;

                        case 65:
                            return (int)RacingProps.h4_prop_h4_boxpile_01a;

                        case 66:
                            return (int)RacingProps.h4_prop_office_desk_01;

                        case 67:
                            return (int)RacingProps.prop_box_wood02a_pu;

                    }
                    break;

                case 15:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_ld_alarm_01;

                        case 1:
                            return (int)RacingProps.prop_barrier_work05;

                        case 2:
                            return (int)RacingProps.ind_prop_dlc_flag_02;

                        case 3:
                            return (int)RacingProps.prop_flare_01;

                        case 4:
                            return (int)RacingProps.hei_prop_bank_plug;

                        case 5:
                            return (int)RacingProps.hei_prop_wall_alarm_on;

                        case 6:
                            return (int)RacingProps.hei_prop_wall_alarm_off;

                        case 7:
                            return (int)RacingProps.hei_prop_hei_cash_trolly_03;

                        case 8:
                            return (int)RacingProps.hei_prop_carrier_docklight_01;

                        case 9:
                            return (int)RacingProps.hei_prop_carrier_docklight_02;

                        case 10:
                            return (int)RacingProps.hei_prop_wall_light_10a_cr;

                        case 11:
                            return (int)RacingProps.hei_prop_heist_apecrate;

                        case 12:
                            return (int)RacingProps.hei_prop_cc_metalcover_01;

                        case 13:
                            return (int)RacingProps.hei_prop_bank_alarm_01;

                        case 14:
                            return (int)RacingProps.prop_road_memorial_02;

                        case 15:
                            return (int)RacingProps.prop_boombox_01;

                        case 16:
                            return (int)RacingProps.prop_ghettoblast_02;

                        case 17:
                            return (int)RacingProps.prop_tapeplayer_01;

                        case 18:
                            return (int)RacingProps.prop_radio_01;

                        case 19:
                            return (int)RacingProps.prop_shamal_crash;

                        case 20:
                            return (int)RacingProps.apa_mp_apa_crashed_usaf_01a;

                        case 21:
                            return (int)RacingProps.ind_prop_firework_01;

                        case 22:
                            return -1305574636;

                        case 23:
                            return (int)RacingProps.ba_prop_battle_mast_01a;

                        case 24:
                            return (int)RacingProps.sr_prop_spec_target_s_01a;

                        case 25:
                            return (int)RacingProps.sr_prop_spec_target_m_01a;

                        case 26:
                            return (int)RacingProps.sr_prop_spec_target_b_01a;

                        case 27:
                            return (int)RacingProps.w_ar_railgun;

                        case 28:
                            return (int)RacingProps.prop_cs_heist_bag_01;

                        case 29:
                            return (int)RacingProps.prop_champset;

                        case 30:
                            return (int)RacingProps.xm_prop_x17_desk_cover_01a;

                        case 31:
                            return (int)RacingProps.xm_prop_x17_tool_draw_01a;

                        case 32:
                            return (int)RacingProps.xm_prop_x17_filecab_01a;

                        case 33:
                            return (int)RacingProps.xm_prop_x17_labvats;

                        case 34:
                            return (int)RacingProps.xm_prop_x17_seat_cover_01a;

                        case 35:
                            return (int)RacingProps.xm_prop_x17_tv_stand_01a;

                        case 36:
                            return (int)RacingProps.xm_prop_x17_bag_01a;

                        case 37:
                            return (int)RacingProps.xm_prop_x17_bag_med_01a;

                        case 38:
                            return (int)RacingProps.xm_prop_x17_sub;

                        case 39:
                            return (int)RacingProps.xm_prop_base_cabinet_door_01;

                        case 40:
                            return (int)RacingProps.xm_prop_x17_corpse_01;

                        case 41:
                            return (int)RacingProps.xm_prop_x17_corpse_02;

                        case 42:
                            return (int)RacingProps.xm_prop_x17_corpse_03;

                        case 43:
                            return (int)RacingProps.xm_prop_x17_shovel_01b;

                        case 44:
                            return (int)RacingProps.xm_prop_x17_shovel_01a;

                        case 45:
                            return (int)RacingProps.xm_prop_x17_note_paper_01a;

                        case 46:
                            return (int)RacingProps.xm_prop_x17_chest_closed;

                        case 47:
                            return (int)RacingProps.xm_prop_x17_chest_open;

                        case 48:
                            return (int)RacingProps.xm_prop_gr_console_01;

                        case 49:
                            return (int)RacingProps.prop_cabinet_01b;

                        case 50:
                            return (int)RacingProps.xm_prop_base_jet_01;

                        case 51:
                            return (int)RacingProps.xm_prop_base_jet_02;

                        case 52:
                            return (int)RacingProps.xm_prop_x17_shamal_crash;

                        case 53:
                            return (int)RacingProps.w_pi_pistol;

                        case 54:
                            return (int)RacingProps.sm_prop_smug_cover_01a;

                        case 55:
                            return (int)RacingProps.xm_prop_x17_cover_01;

                        case 56:
                            return (int)RacingProps.xm_prop_x17_bunker_door;

                        case 57:
                            return (int)RacingProps.prop_sm1_11_garaged;

                    }
                    break;

                case 29:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.stt_prop_stunt_jump_s;

                        case 1:
                            return (int)RacingProps.bkr_prop_biker_jump_s;

                        case 2:
                            return (int)RacingProps.stt_prop_stunt_jump_m;

                        case 3:
                            return (int)RacingProps.bkr_prop_biker_jump_m;

                        case 4:
                            return (int)RacingProps.stt_prop_stunt_jump_l;

                        case 5:
                            return (int)RacingProps.bkr_prop_biker_jump_l;

                        case 6:
                            return (int)RacingProps.stt_prop_stunt_jump_sb;

                        case 7:
                            return (int)RacingProps.bkr_prop_biker_jump_sb;

                        case 8:
                            return (int)RacingProps.stt_prop_stunt_jump_mb;

                        case 9:
                            return (int)RacingProps.bkr_prop_biker_jump_mb;

                        case 10:
                            return (int)RacingProps.stt_prop_stunt_jump_lb;

                        case 11:
                            return (int)RacingProps.bkr_prop_biker_jump_lb;

                        case 12:
                            return (int)RacingProps.stt_prop_ramp_jump_xs;

                        case 13:
                            return (int)RacingProps.stt_prop_ramp_jump_s;

                        case 14:
                            return (int)RacingProps.stt_prop_ramp_jump_m;

                        case 15:
                            return (int)RacingProps.stt_prop_ramp_jump_l;

                        case 16:
                            return (int)RacingProps.stt_prop_ramp_jump_xl;

                        case 17:
                            return (int)RacingProps.stt_prop_ramp_jump_xxl;

                        case 18:
                            return (int)RacingProps.stt_prop_track_jump_01a;

                        case 19:
                            return (int)RacingProps.bkr_prop_biker_jump_01a;

                        case 20:
                            return (int)RacingProps.stt_prop_track_jump_01b;

                        case 21:
                            return (int)RacingProps.bkr_prop_biker_jump_01b;

                        case 22:
                            return (int)RacingProps.stt_prop_track_jump_01c;

                        case 23:
                            return (int)RacingProps.bkr_prop_biker_jump_01c;

                        case 24:
                            return (int)RacingProps.stt_prop_track_jump_02a;

                        case 25:
                            return (int)RacingProps.bkr_prop_biker_jump_02a;

                        case 26:
                            return (int)RacingProps.stt_prop_track_jump_02b;

                        case 27:
                            return (int)RacingProps.bkr_prop_biker_jump_02b;

                        case 28:
                            return (int)RacingProps.stt_prop_track_jump_02c;

                        case 29:
                            return (int)RacingProps.bkr_prop_biker_jump_02c;

                        case 30:
                            return (int)RacingProps.stt_prop_ramp_adj_flip_mb;

                        case 31:
                            return (int)RacingProps.stt_prop_ramp_adj_flip_s;

                        case 32:
                            return (int)RacingProps.stt_prop_ramp_adj_flip_sb;

                        case 33:
                            return (int)RacingProps.stt_prop_ramp_adj_flip_m;

                        case 34:
                            return (int)RacingProps.stt_prop_stunt_ramp;

                        case 35:
                            return (int)RacingProps.stt_prop_stunt_wideramp;

                        case 36:
                            return (int)RacingProps.stt_prop_stunt_bblock_qp3;

                        case 37:
                            return (int)RacingProps.imp_prop_impexp_bblock_qp3;

                        case 38:
                            return (int)RacingProps.stt_prop_stunt_bblock_qp2;

                        case 39:
                            return (int)RacingProps.bkr_prop_biker_bblock_qp2;

                        case 40:
                            return (int)RacingProps.stt_prop_stunt_bblock_qp;

                        case 41:
                            return (int)RacingProps.bkr_prop_biker_bblock_qp;

                        case 42:
                            return (int)RacingProps.stt_prop_stunt_bblock_hump_01;

                        case 43:
                            return (int)RacingProps.bkr_prop_biker_bblock_hump_01;

                        case 44:
                            return (int)RacingProps.stt_prop_stunt_bblock_hump_02;

                        case 45:
                            return (int)RacingProps.bkr_prop_biker_bblock_hump_02;

                        case 46:
                            return (int)RacingProps.bkr_prop_biker_bblock_cor_03;

                        case 47:
                            return (int)RacingProps.bkr_prop_biker_bblock_cor_02;

                        case 48:
                            return (int)RacingProps.bkr_prop_biker_bblock_cor;

                    }
                    break;

                case 27:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.stt_prop_stunt_bblock_sml1;

                        case 1:
                            return (int)RacingProps.bkr_prop_biker_bblock_sml1;

                        case 2:
                            return (int)RacingProps.stt_prop_stunt_bblock_sml2;

                        case 3:
                            return (int)RacingProps.bkr_prop_biker_bblock_sml2;

                        case 4:
                            return (int)RacingProps.stt_prop_stunt_bblock_sml3;

                        case 5:
                            return (int)RacingProps.bkr_prop_biker_bblock_sml3;

                        case 6:
                            return (int)RacingProps.stt_prop_stunt_bblock_mdm1;

                        case 7:
                            return (int)RacingProps.bkr_prop_biker_bblock_mdm1;

                        case 8:
                            return (int)RacingProps.stt_prop_stunt_bblock_mdm2;

                        case 9:
                            return (int)RacingProps.bkr_prop_biker_bblock_mdm2;

                        case 10:
                            return (int)RacingProps.stt_prop_stunt_bblock_mdm3;

                        case 11:
                            return (int)RacingProps.bkr_prop_biker_bblock_mdm3;

                        case 12:
                            return (int)RacingProps.stt_prop_stunt_bblock_lrg1;

                        case 13:
                            return (int)RacingProps.bkr_prop_biker_bblock_lrg1;

                        case 14:
                            return (int)RacingProps.stt_prop_stunt_bblock_lrg2;

                        case 15:
                            return (int)RacingProps.bkr_prop_biker_bblock_lrg2;

                        case 16:
                            return (int)RacingProps.stt_prop_stunt_bblock_lrg3;

                        case 17:
                            return (int)RacingProps.bkr_prop_biker_bblock_lrg3;

                        case 18:
                            return (int)RacingProps.stt_prop_stunt_bblock_xl1;

                        case 19:
                            return (int)RacingProps.bkr_prop_biker_bblock_xl1;

                        case 20:
                            return (int)RacingProps.stt_prop_stunt_bblock_xl2;

                        case 21:
                            return (int)RacingProps.bkr_prop_biker_bblock_xl2;

                        case 22:
                            return (int)RacingProps.stt_prop_stunt_bblock_xl3;

                        case 23:
                            return (int)RacingProps.bkr_prop_biker_bblock_xl3;

                        case 24:
                            return (int)RacingProps.stt_prop_stunt_bblock_huge_01;

                        case 25:
                            return (int)RacingProps.bkr_prop_biker_bblock_huge_01;

                        case 26:
                            return (int)RacingProps.stt_prop_stunt_bblock_huge_02;

                        case 27:
                            return (int)RacingProps.bkr_prop_biker_bblock_huge_02;

                        case 28:
                            return (int)RacingProps.stt_prop_stunt_bblock_huge_03;

                        case 29:
                            return (int)RacingProps.bkr_prop_biker_bblock_huge_03;

                        case 30:
                            return (int)RacingProps.stt_prop_stunt_bblock_huge_04;

                        case 31:
                            return (int)RacingProps.bkr_prop_biker_bblock_huge_04;

                        case 32:
                            return (int)RacingProps.stt_prop_stunt_bblock_huge_05;

                        case 33:
                            return (int)RacingProps.bkr_prop_biker_bblock_huge_05;

                        case 34:
                            return (int)RacingProps.imp_prop_impexp_bblock_huge_01;

                        case 35:
                            return (int)RacingProps.imp_prop_impexp_bblock_sml1;

                        case 36:
                            return (int)RacingProps.imp_prop_impexp_bblock_mdm1;

                        case 37:
                            return (int)RacingProps.imp_prop_impexp_bblock_lrg1;

                        case 38:
                            return (int)RacingProps.imp_prop_impexp_bblock_xl1;

                    }
                    break;

                case 30:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.stt_prop_ramp_adj_hloop;

                        case 1:
                            return (int)RacingProps.stt_prop_ramp_adj_loop;

                        case 2:
                            return (int)RacingProps.stt_prop_ramp_multi_loop_rb;

                        case 3:
                            return (int)RacingProps.stt_prop_stunt_jump_loop;

                        case 4:
                            return (int)RacingProps.ar_prop_ar_jump_loop;

                        case 5:
                            return (int)RacingProps.stt_prop_ramp_spiral_s;

                        case 6:
                            return (int)RacingProps.stt_prop_ramp_spiral_l_s;

                        case 7:
                            return (int)RacingProps.stt_prop_ramp_spiral_m;

                        case 8:
                            return (int)RacingProps.stt_prop_ramp_spiral_l_m;

                        case 9:
                            return (int)RacingProps.stt_prop_ramp_spiral_l;

                        case 10:
                            return (int)RacingProps.stt_prop_ramp_spiral_l_l;

                        case 11:
                            return (int)RacingProps.stt_prop_ramp_spiral_xxl;

                        case 12:
                            return (int)RacingProps.stt_prop_ramp_spiral_l_xxl;

                        case 13:
                            return (int)RacingProps.stt_prop_wallride_45r;

                        case 14:
                            return (int)RacingProps.stt_prop_wallride_45ra;

                        case 15:
                            return (int)RacingProps.stt_prop_wallride_45l;

                        case 16:
                            return (int)RacingProps.stt_prop_wallride_45la;

                        case 17:
                            return (int)RacingProps.stt_prop_wallride_90r;

                        case 18:
                            return (int)RacingProps.stt_prop_wallride_90rb;

                        case 19:
                            return (int)RacingProps.stt_prop_wallride_90l;

                        case 20:
                            return (int)RacingProps.stt_prop_wallride_90lb;

                        case 21:
                            return (int)RacingProps.stt_prop_wallride_04;

                        case 22:
                            return (int)RacingProps.stt_prop_wallride_01;

                        case 23:
                            return (int)RacingProps.stt_prop_wallride_01b;

                        case 24:
                            return (int)RacingProps.stt_prop_wallride_05;

                        case 25:
                            return (int)RacingProps.stt_prop_wallride_05b;

                        case 26:
                            return (int)RacingProps.stt_prop_wallride_02;

                        case 27:
                            return (int)RacingProps.stt_prop_wallride_02b;

                        case 28:
                            return (int)RacingProps.stt_prop_stunt_target_small;

                        case 29:
                            return (int)RacingProps.bkr_prop_biker_target_small;

                        case 30:
                            return (int)RacingProps.stt_prop_stunt_target;

                        case 31:
                            return (int)RacingProps.bkr_prop_biker_target;

                        case 32:
                            return (int)RacingProps.stt_prop_stunt_bowlpin_stand;

                        case 33:
                            return (int)RacingProps.bkr_prop_biker_bowlpin_stand;

                        case 34:
                            return (int)RacingProps.stt_prop_stunt_soccer_goal;

                        case 35:
                            return (int)RacingProps.stt_prop_stunt_landing_zone_01;

                        case 36:
                            return (int)RacingProps.bkr_prop_biker_landing_zone_01;

                        case 37:
                            return (int)RacingProps.ch_prop_stunt_landing_zone_01a;

                        case 38:
                            return (int)RacingProps.stt_prop_hoop_tyre_01a;

                        case 39:
                            return (int)RacingProps.sr_prop_sr_track_jumpwall;

                        case 40:
                            return (int)RacingProps.ar_prop_ar_start_01a;

                    }
                    break;

                case 32:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.stt_prop_track_speedup;

                        case 1:
                            return (int)RacingProps.stt_prop_track_speedup_t2;

                        case 2:
                            return (int)RacingProps.stt_prop_track_speedup_t1;

                        case 3:
                            return (int)RacingProps.stt_prop_track_slowdown;

                        case 4:
                            return (int)RacingProps.stt_prop_track_slowdown_t2;

                        case 5:
                            return (int)RacingProps.stt_prop_track_slowdown_t1;

                        case 6:
                            return (int)RacingProps.sr_prop_track_refill;

                        case 7:
                            return (int)RacingProps.sr_prop_track_refill_t2;

                        case 8:
                            return (int)RacingProps.sr_prop_track_refill_t1;

                        case 9:
                            return (int)RacingProps.ar_prop_ar_speed_ring;

                        case 10:
                            return (int)RacingProps.ind_prop_firework_03;

                        case 11:
                            return (int)RacingProps.stt_prop_hoop_small_01;

                        case 12:
                            return (int)RacingProps.ar_prop_ar_hoop_med_01;

                        case 13:
                            return (int)RacingProps.stt_prop_hoop_constraction_01a;

                        case 14:
                            return (int)RacingProps.stt_prop_race_tannoy;

                        case 15:
                            return (int)RacingProps.stt_prop_speakerstack_01a;

                    }
                    break;

                case 34:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.as_prop_as_target_small_02;

                        case 1:
                            return (int)RacingProps.as_prop_as_target_small;

                        case 2:
                            return (int)RacingProps.as_prop_as_target_medium;

                        case 3:
                            return (int)RacingProps.as_prop_as_target_big;

                        case 4:
                            return (int)RacingProps.as_prop_as_target_scaffold_01a;

                        case 5:
                            return (int)RacingProps.as_prop_as_target_scaffold_01b;

                        case 6:
                            return (int)RacingProps.as_prop_as_target_scaffold_02a;

                        case 7:
                            return (int)RacingProps.as_prop_as_target_scaffold_02b;

                    }
                    break;

                case 31:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.stt_prop_track_stop_sign;

                        case 1:
                            return (int)RacingProps.stt_prop_corner_sign_01;

                        case 2:
                            return (int)RacingProps.stt_prop_corner_sign_02;

                        case 3:
                            return (int)RacingProps.stt_prop_corner_sign_03;

                        case 4:
                            return (int)RacingProps.stt_prop_corner_sign_04;

                        case 5:
                            return (int)RacingProps.stt_prop_corner_sign_05;

                        case 6:
                            return (int)RacingProps.stt_prop_corner_sign_06;

                        case 7:
                            return (int)RacingProps.stt_prop_corner_sign_07;

                        case 8:
                            return (int)RacingProps.stt_prop_corner_sign_08;

                        case 9:
                            return (int)RacingProps.stt_prop_corner_sign_09;

                        case 10:
                            return (int)RacingProps.stt_prop_corner_sign_10;

                        case 11:
                            return (int)RacingProps.stt_prop_corner_sign_11;

                        case 12:
                            return (int)RacingProps.stt_prop_corner_sign_12;

                        case 13:
                            return (int)RacingProps.stt_prop_corner_sign_13;

                        case 14:
                            return (int)RacingProps.stt_prop_corner_sign_14;

                        case 15:
                            return (int)RacingProps.ch_prop_pit_sign_01a;

                        case 16:
                            return (int)RacingProps.sum_prop_ac_pit_sign_left;

                        case 17:
                            return (int)RacingProps.stt_prop_sign_circuit_01;

                        case 18:
                            return (int)RacingProps.stt_prop_sign_circuit_02;

                        case 19:
                            return (int)RacingProps.stt_prop_sign_circuit_03;

                        case 20:
                            return (int)RacingProps.stt_prop_sign_circuit_05;

                        case 21:
                            return (int)RacingProps.stt_prop_sign_circuit_04;

                        case 22:
                            return (int)RacingProps.stt_prop_sign_circuit_06;

                        case 23:
                            return (int)RacingProps.stt_prop_sign_circuit_07;

                        case 24:
                            return (int)RacingProps.stt_prop_sign_circuit_08;

                        case 25:
                            return (int)RacingProps.stt_prop_sign_circuit_09;

                        case 26:
                            return (int)RacingProps.stt_prop_sign_circuit_10;

                        case 27:
                            return (int)RacingProps.stt_prop_sign_circuit_11;

                        case 28:
                            return (int)RacingProps.stt_prop_sign_circuit_12;

                        case 29:
                            return (int)RacingProps.stt_prop_sign_circuit_13;

                        case 30:
                            return (int)RacingProps.stt_prop_sign_circuit_14;

                        case 31:
                            return (int)RacingProps.stt_prop_sign_circuit_15;

                        case 32:
                            return (int)RacingProps.sum_prop_ac_wall_sign_01;

                        case 33:
                            return (int)RacingProps.sum_prop_ac_wall_sign_0l1;

                        case 34:
                            return (int)RacingProps.sum_prop_ac_wall_sign_0r1;

                        case 35:
                            return (int)RacingProps.sum_prop_ac_wall_sign_02;

                        case 36:
                            return (int)RacingProps.sum_prop_ac_wall_sign_04;

                        case 37:
                            return (int)RacingProps.sum_prop_ac_wall_sign_03;

                        case 38:
                            return (int)RacingProps.sum_prop_ac_wall_sign_05;

                        case 39:
                            return (int)RacingProps.sum_prop_ac_tyre_wall_lit_01;

                        case 40:
                            return (int)RacingProps.sum_prop_ac_tyre_wall_lit_0r1;

                        case 41:
                            return (int)RacingProps.sum_prop_ac_tyre_wall_lit_0l1;

                        case 42:
                            return (int)RacingProps.sum_prop_ac_tyre_wall_pit_l;

                        case 43:
                            return (int)RacingProps.sum_prop_ac_tyre_wall_pit_r;

                        case 44:
                            return (int)RacingProps.sum_prop_ac_tyre_wall_u_l;

                        case 45:
                            return (int)RacingProps.sum_prop_ac_tyre_wall_u_r;

                        case 46:
                            return (int)RacingProps.stt_prop_tyre_wall_02;

                        case 47:
                            return (int)RacingProps.stt_prop_tyre_wall_03;

                        case 48:
                            return (int)RacingProps.stt_prop_tyre_wall_04;

                        case 49:
                            return (int)RacingProps.stt_prop_tyre_wall_05;

                        case 50:
                            return (int)RacingProps.stt_prop_tyre_wall_06;

                        case 51:
                            return (int)RacingProps.stt_prop_tyre_wall_07;

                        case 52:
                            return (int)RacingProps.stt_prop_tyre_wall_08;

                        case 53:
                            return (int)RacingProps.stt_prop_tyre_wall_09;

                        case 54:
                            return (int)RacingProps.stt_prop_tyre_wall_010;

                        case 55:
                            return (int)RacingProps.stt_prop_tyre_wall_011;

                        case 56:
                            return (int)RacingProps.stt_prop_tyre_wall_012;

                        case 57:
                            return (int)RacingProps.stt_prop_tyre_wall_013;

                        case 58:
                            return (int)RacingProps.stt_prop_tyre_wall_014;

                        case 59:
                            return (int)RacingProps.stt_prop_tyre_wall_015;

                        case 60:
                            return (int)RacingProps.stt_prop_tyre_wall_0r2;

                        case 61:
                            return (int)RacingProps.stt_prop_tyre_wall_0r06;

                        case 62:
                            return (int)RacingProps.stt_prop_tyre_wall_0r07;

                        case 63:
                            return (int)RacingProps.stt_prop_tyre_wall_0r011;

                        case 64:
                            return (int)RacingProps.stt_prop_tyre_wall_0r012;

                        case 65:
                            return (int)RacingProps.stt_prop_tyre_wall_0r013;

                        case 66:
                            return (int)RacingProps.stt_prop_tyre_wall_0r014;

                        case 67:
                            return (int)RacingProps.stt_prop_tyre_wall_0r019;

                        case 68:
                            return (int)RacingProps.stt_prop_tyre_wall_0r3;

                        case 69:
                            return (int)RacingProps.stt_prop_tyre_wall_0r04;

                        case 70:
                            return (int)RacingProps.stt_prop_tyre_wall_0r05;

                        case 71:
                            return (int)RacingProps.stt_prop_tyre_wall_0r08;

                        case 72:
                            return (int)RacingProps.stt_prop_tyre_wall_0r09;

                        case 73:
                            return (int)RacingProps.stt_prop_tyre_wall_0r010;

                        case 74:
                            return (int)RacingProps.stt_prop_tyre_wall_0r015;

                        case 75:
                            return (int)RacingProps.stt_prop_tyre_wall_0r016;

                        case 76:
                            return (int)RacingProps.stt_prop_tyre_wall_0r017;

                        case 77:
                            return (int)RacingProps.stt_prop_tyre_wall_0r018;

                        case 78:
                            return (int)RacingProps.stt_prop_tyre_wall_0l2;

                        case 79:
                            return (int)RacingProps.stt_prop_tyre_wall_0l06;

                        case 80:
                            return (int)RacingProps.stt_prop_tyre_wall_0l07;

                        case 81:
                            return (int)RacingProps.stt_prop_tyre_wall_0l013;

                        case 82:
                            return (int)RacingProps.stt_prop_tyre_wall_0l014;

                        case 83:
                            return (int)RacingProps.stt_prop_tyre_wall_0l015;

                        case 84:
                            return (int)RacingProps.stt_prop_tyre_wall_0l020;

                        case 85:
                            return (int)RacingProps.stt_prop_tyre_wall_0l3;

                        case 86:
                            return (int)RacingProps.stt_prop_tyre_wall_0l04;

                        case 87:
                            return (int)RacingProps.stt_prop_tyre_wall_0l05;

                        case 88:
                            return (int)RacingProps.stt_prop_tyre_wall_0l08;

                        case 89:
                            return (int)RacingProps.stt_prop_tyre_wall_0l010;

                        case 90:
                            return (int)RacingProps.stt_prop_tyre_wall_0l012;

                        case 91:
                            return (int)RacingProps.stt_prop_tyre_wall_0l16;

                        case 92:
                            return (int)RacingProps.stt_prop_tyre_wall_0l17;

                        case 93:
                            return (int)RacingProps.stt_prop_tyre_wall_0l018;

                        case 94:
                            return (int)RacingProps.stt_prop_tyre_wall_0l019;

                        case 95:
                            return (int)RacingProps.stt_prop_race_gantry_01;

                        case 96:
                            return (int)RacingProps.ch_prop_ch_race_gantry_02;

                        case 97:
                            return (int)RacingProps.ch_prop_ch_race_gantry_03;

                        case 98:
                            return (int)RacingProps.ch_prop_ch_race_gantry_04;

                        case 99:
                            return (int)RacingProps.ch_prop_ch_race_gantry_05;

                        case 100:
                            return (int)RacingProps.sr_mp_spec_races_blimp_sign;

                        case 101:
                            return (int)RacingProps.sr_mp_spec_races_take_flight_sign;

                        case 102:
                            return (int)RacingProps.sr_mp_spec_races_ron_sign;

                        case 103:
                            return (int)RacingProps.sr_mp_spec_races_xero_sign;

                        case 104:
                            return (int)RacingProps.sum_prop_archway_01;

                        case 105:
                            return (int)RacingProps.sum_prop_archway_02;

                        case 106:
                            return (int)RacingProps.sum_prop_archway_03;

                        case 107:
                            return (int)RacingProps.sum_prop_ac_pit_sign_r_01a;

                        case 108:
                            return (int)RacingProps.sum_prop_ac_pit_sign_l_01a;

                    }
                    break;

                case 22:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.ar_prop_ar_arrow_thin_m;

                        case 1:
                            return (int)RacingProps.ar_prop_ar_arrow_wide_m;

                        case 2:
                            return (int)RacingProps.ar_prop_ar_arrow_thin_l;

                        case 3:
                            return (int)RacingProps.ar_prop_ar_arrow_wide_l;

                        case 4:
                            return (int)RacingProps.ar_prop_ar_arrow_thin_xl;

                        case 5:
                            return (int)RacingProps.ar_prop_ar_arrow_wide_xl;

                    }
                    break;

                case 16:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.stt_prop_track_start;

                        case 1:
                            return (int)RacingProps.stt_prop_track_start_02;

                        case 2:
                            return (int)RacingProps.stt_prop_race_start_line_01;

                        case 3:
                            return (int)RacingProps.stt_prop_race_start_line_01b;

                        case 4:
                            return (int)RacingProps.sr_prop_sr_start_line_02;

                        case 5:
                            return (int)RacingProps.stt_prop_race_start_line_02b;

                        case 6:
                            return (int)RacingProps.stt_prop_race_start_line_03;

                        case 7:
                            return (int)RacingProps.stt_prop_race_start_line_03b;

                        case 8:
                            return (int)RacingProps.sum_prop_ac_track_pit_stop_16l;

                        case 9:
                            return (int)RacingProps.sum_prop_ac_track_pit_stop_16r;

                        case 10:
                            return (int)RacingProps.sum_prop_ac_track_pit_stop_30l;

                        case 11:
                            return (int)RacingProps.ch_prop_track_pit_stop_01;

                        case 12:
                            return (int)RacingProps.sum_prop_ac_track_pit_stop_30r;

                        case 13:
                            return (int)RacingProps.stt_prop_track_straight_s;

                        case 14:
                            return (int)RacingProps.stt_prop_track_straight_m;

                        case 15:
                            return (int)RacingProps.ba_prop_track_straight_lm;

                        case 16:
                            return (int)RacingProps.stt_prop_track_straight_l;

                        case 17:
                            return (int)RacingProps.sum_prop_ac_track_pit_stop_01;

                        case 18:
                            return (int)RacingProps.stt_prop_track_bend_m;

                        case 19:
                            return (int)RacingProps.stt_prop_track_bend_l;

                        case 20:
                            return (int)RacingProps.ba_prop_track_bend_l_b;

                        case 21:
                            return (int)RacingProps.stt_prop_track_bend2_l;

                        case 22:
                            return (int)RacingProps.stt_prop_track_bend2_l_b;

                        case 23:
                            return (int)RacingProps.stt_prop_track_bend_5d;

                        case 24:
                            return (int)RacingProps.stt_prop_track_bend_15d;

                        case 25:
                            return (int)RacingProps.stt_prop_track_bend_30d;

                        case 26:
                            return (int)RacingProps.ch_prop_track_ch_bend_45;

                        case 27:
                            return (int)RacingProps.sum_prop_track_ac_bend_45;

                        case 28:
                            return (int)RacingProps.sum_prop_track_ac_bend_lc;

                        case 29:
                            return (int)RacingProps.ch_prop_track_ch_bend_135;

                        case 30:
                            return (int)RacingProps.sum_prop_track_ac_bend_135;

                        case 31:
                            return (int)RacingProps.stt_prop_track_bend_180d;

                        case 32:
                            return (int)RacingProps.sum_prop_track_ac_bend_180d;

                        case 33:
                            return (int)RacingProps.stt_prop_track_fork;

                        case 34:
                            return (int)RacingProps.stt_prop_track_cross;

                        case 35:
                            return (int)RacingProps.stt_prop_track_link;

                        case 36:
                            return (int)RacingProps.stt_prop_track_chicane_l;

                        case 37:
                            return (int)RacingProps.stt_prop_track_chicane_l_02;

                        case 38:
                            return (int)RacingProps.stt_prop_track_chicane_r;

                        case 39:
                            return (int)RacingProps.stt_prop_track_chicane_r_02;

                        case 40:
                            return (int)RacingProps.stt_prop_track_block_03;

                        case 41:
                            return (int)RacingProps.sr_prop_track_straight_l_d5;

                        case 42:
                            return (int)RacingProps.sr_prop_track_straight_l_d15;

                        case 43:
                            return (int)RacingProps.sr_prop_track_straight_l_d30;

                        case 44:
                            return (int)RacingProps.sr_prop_track_straight_l_d45;

                        case 45:
                            return (int)RacingProps.sr_prop_track_straight_l_u5;

                        case 46:
                            return (int)RacingProps.sr_prop_track_straight_l_u15;

                        case 47:
                            return (int)RacingProps.sr_prop_track_straight_l_u30;

                        case 48:
                            return (int)RacingProps.sr_prop_track_straight_l_u45;

                    }
                    break;

                case 17:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.stt_prop_track_straight_bar_s;

                        case 1:
                            return (int)RacingProps.stt_prop_track_straight_bar_m;

                        case 2:
                            return (int)RacingProps.stt_prop_track_straight_lm_bar;

                        case 3:
                            return (int)RacingProps.stt_prop_track_straight_bar_l;

                        case 4:
                            return (int)RacingProps.stt_prop_track_bend_bar_m;

                        case 5:
                            return (int)RacingProps.stt_prop_track_bend_bar_l;

                        case 6:
                            return (int)RacingProps.stt_prop_track_bend_bar_l_b;

                        case 7:
                            return (int)RacingProps.stt_prop_track_bend2_bar_l;

                        case 8:
                            return (int)RacingProps.stt_prop_track_bend2_bar_l_b;

                        case 9:
                            return (int)RacingProps.stt_prop_track_bend_5d_bar;

                        case 10:
                            return (int)RacingProps.stt_prop_track_bend_15d_bar;

                        case 11:
                            return (int)RacingProps.stt_prop_track_bend_30d_bar;

                        case 12:
                            return (int)RacingProps.ch_prop_track_ch_bend_bar_45d;

                        case 13:
                            return (int)RacingProps.sum_prop_track_ac_bend_bar_45;

                        case 14:
                            return (int)RacingProps.ch_prop_track_bend_bar_lc;

                        case 15:
                            return (int)RacingProps.ch_prop_track_ch_bend_bar_135;

                        case 16:
                            return (int)RacingProps.sum_prop_track_ac_bend_bar_135;

                        case 17:
                            return (int)RacingProps.stt_prop_track_bend_180d_bar;

                        case 18:
                            return (int)RacingProps.sum_prop_track_ac_bend_bar_180d;

                        case 19:
                            return (int)RacingProps.stt_prop_track_fork_bar;

                        case 20:
                            return (int)RacingProps.stt_prop_track_cross_bar;

                        case 21:
                            return (int)RacingProps.stt_prop_track_funnel;

                        case 22:
                            return (int)RacingProps.stt_prop_track_funnel_ads_01a;

                        case 23:
                            return (int)RacingProps.stt_prop_track_funnel_ads_01b;

                        case 24:
                            return (int)RacingProps.stt_prop_track_funnel_ads_01c;

                        case 25:
                            return (int)RacingProps.stt_prop_track_block_01;

                        case 26:
                            return (int)RacingProps.stt_prop_track_block_02;

                    }
                    break;

                case 18:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.stt_prop_stunt_track_start_02;

                        case 1:
                            return (int)RacingProps.stt_prop_stunt_track_st_02;

                        case 2:
                            return (int)RacingProps.stt_prop_stunt_track_start;

                        case 3:
                            return (int)RacingProps.stt_prop_stunt_track_st_01;

                        case 4:
                            return (int)RacingProps.stt_prop_stunt_track_exshort;

                        case 5:
                            return (int)RacingProps.stt_prop_stunt_track_short;

                        case 6:
                            return (int)RacingProps.stt_prop_stunt_track_straight;

                        case 7:
                            return (int)RacingProps.stt_prop_stunt_track_turn;

                        case 8:
                            return (int)RacingProps.stt_prop_stunt_track_sh15;

                        case 9:
                            return (int)RacingProps.stt_prop_stunt_track_sh30;

                        case 10:
                            return (int)RacingProps.stt_prop_stunt_track_sh45;

                        case 11:
                            return (int)RacingProps.stt_prop_stunt_track_sh45_a;

                        case 12:
                            return (int)RacingProps.stt_prop_stunt_track_uturn;

                        case 13:
                            return (int)RacingProps.stt_prop_stunt_track_cutout;

                        case 14:
                            return (int)RacingProps.stt_prop_stunt_track_otake;

                        case 15:
                            return (int)RacingProps.stt_prop_stunt_track_fork;

                        case 16:
                            return (int)RacingProps.stt_prop_stunt_track_funnel;

                        case 17:
                            return (int)RacingProps.stt_prop_stunt_track_funlng;

                        case 18:
                            return (int)RacingProps.stt_prop_stunt_track_slope15;

                        case 19:
                            return (int)RacingProps.stt_prop_stunt_track_slope30;

                        case 20:
                            return (int)RacingProps.stt_prop_stunt_track_slope45;

                        case 21:
                            return (int)RacingProps.stt_prop_stunt_track_hill;

                        case 22:
                            return (int)RacingProps.stt_prop_stunt_track_hill2;

                        case 23:
                            return (int)RacingProps.stt_prop_stunt_track_bumps;

                        case 24:
                            return (int)RacingProps.stt_prop_stunt_track_jump;

                        case 25:
                            return (int)RacingProps.stt_prop_stunt_jump15;

                        case 26:
                            return (int)RacingProps.stt_prop_stunt_jump30;

                        case 27:
                            return (int)RacingProps.stt_prop_stunt_jump45;

                        case 28:
                            return (int)RacingProps.stt_prop_stunt_track_link;

                        case 29:
                            return (int)RacingProps.stt_prop_stunt_track_dwlink;

                        case 30:
                            return (int)RacingProps.stt_prop_stunt_track_dwlink_02;

                        case 31:
                            return (int)RacingProps.stt_prop_stunt_track_dwshort;

                        case 32:
                            return (int)RacingProps.stt_prop_stunt_track_dwsh15;

                        case 33:
                            return (int)RacingProps.stt_prop_stunt_track_dwturn;

                        case 34:
                            return (int)RacingProps.stt_prop_stunt_track_dwuturn;

                        case 35:
                            return (int)RacingProps.stt_prop_stunt_track_dwslope15;

                        case 36:
                            return (int)RacingProps.as_prop_as_dwslope30;

                        case 37:
                            return (int)RacingProps.stt_prop_stunt_track_dwslope45;

                        case 38:
                            return (int)RacingProps.stt_prop_track_tube_02;

                        case 39:
                            return (int)RacingProps.ba_prop_battle_track_exshort;

                        case 40:
                            return (int)RacingProps.ba_prop_battle_track_short;
                    }
                    break;

                case 19:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.sum_prop_race_barrier_01_sec;

                        case 1:
                            return (int)RacingProps.sum_prop_race_barrier_02_sec;

                        case 2:
                            return (int)RacingProps.sum_prop_race_barrier_04_sec;

                        case 3:
                            return (int)RacingProps.sum_prop_race_barrier_08_sec;

                        case 4:
                            return (int)RacingProps.sum_prop_race_barrier_16_sec;

                        case 5:
                            return (int)RacingProps.sum_prop_ac_short_barrier_05d;

                        case 6:
                            return (int)RacingProps.sum_prop_ac_short_barrier_15d;

                        case 7:
                            return (int)RacingProps.sum_prop_ac_short_barrier_30d;

                        case 8:
                            return (int)RacingProps.sum_prop_ac_short_barrier_45d;

                        case 9:
                            return (int)RacingProps.sum_prop_ac_short_barrier_90d;

                        case 10:
                            return (int)RacingProps.sum_prop_ac_long_barrier_05d;

                        case 11:
                            return (int)RacingProps.sum_prop_ac_long_barrier_15d;

                        case 12:
                            return (int)RacingProps.sum_prop_ac_long_barrier_30d;

                        case 13:
                            return (int)RacingProps.sum_prop_ac_long_barrier_45d;

                        case 14:
                            return (int)RacingProps.sum_prop_ac_long_barrier_90d;

                        case 15:
                            return (int)RacingProps.sum_prop_barrier_ac_bend_05d;

                        case 16:
                            return (int)RacingProps.sum_prop_barrier_ac_bend_15d;

                        case 17:
                            return (int)RacingProps.sum_prop_barrier_ac_bend_30d;

                        case 18:
                            return (int)RacingProps.sum_prop_barrier_ac_bend_45d;

                        case 19:
                            return (int)RacingProps.sum_prop_barrier_ac_bend_90d;

                        case 20:
                            return (int)RacingProps.sum_prop_track_ac_straight_bar_s_s;

                        case 21:
                            return (int)RacingProps.sum_prop_track_ac_straight_bar_s;

                        case 22:
                            return (int)RacingProps.ch_prop_track_ch_straight_bar_m;

                        case 23:
                            return (int)RacingProps.sum_prop_track_ac_bend_bar_m_out;

                        case 24:
                            return (int)RacingProps.sum_prop_track_ac_bend_bar_m_in;

                        case 25:
                            return (int)RacingProps.sum_prop_track_ac_bend_bar_l_out;

                        case 26:
                            return (int)RacingProps.sum_prop_track_ac_bend_bar_l_b;

                    }
                    break;

                case 20:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.stt_prop_stunt_tube_xxs;

                        case 1:
                            return (int)RacingProps.bkr_prop_biker_tube_xxs;

                        case 2:
                            return (int)RacingProps.stt_prop_stunt_tube_xs;

                        case 3:
                            return (int)RacingProps.bkr_prop_biker_tube_xs;

                        case 4:
                            return (int)RacingProps.stt_prop_stunt_tube_s;

                        case 5:
                            return (int)RacingProps.bkr_prop_biker_tube_s;

                        case 6:
                            return (int)RacingProps.stt_prop_stunt_tube_m;

                        case 7:
                            return (int)RacingProps.bkr_prop_biker_tube_m;

                        case 8:
                            return (int)RacingProps.stt_prop_stunt_tube_l;

                        case 9:
                            return (int)RacingProps.bkr_prop_biker_tube_l;

                        case 10:
                            return (int)RacingProps.stt_prop_stunt_tube_crn;

                        case 11:
                            return (int)RacingProps.bkr_prop_biker_tube_crn;

                        case 12:
                            return (int)RacingProps.stt_prop_stunt_tube_crn_5d;

                        case 13:
                            return (int)RacingProps.stt_prop_stunt_tube_crn_15d;

                        case 14:
                            return (int)RacingProps.stt_prop_stunt_tube_crn_30d;

                        case 15:
                            return (int)RacingProps.stt_prop_stunt_tube_crn2;

                        case 16:
                            return (int)RacingProps.bkr_prop_biker_tube_crn2;

                        case 17:
                            return (int)RacingProps.stt_prop_stunt_tube_fork;

                        case 18:
                            return (int)RacingProps.stt_prop_stunt_tube_cross;

                        case 19:
                            return (int)RacingProps.bkr_prop_biker_tube_cross;

                        case 20:
                            return (int)RacingProps.stt_prop_stunt_tube_gap_01;

                        case 21:
                            return (int)RacingProps.bkr_prop_biker_tube_gap_01;

                        case 22:
                            return (int)RacingProps.stt_prop_stunt_tube_gap_02;

                        case 23:
                            return (int)RacingProps.bkr_prop_biker_tube_gap_02;

                        case 24:
                            return (int)RacingProps.stt_prop_stunt_tube_gap_03;

                        case 25:
                            return (int)RacingProps.bkr_prop_biker_tube_gap_03;

                        case 26:
                            return (int)RacingProps.stt_prop_stunt_tube_qg;

                        case 27:
                            return (int)RacingProps.stt_prop_stunt_tube_hg;

                        case 28:
                            return (int)RacingProps.stt_prop_stunt_tube_jmp;

                        case 29:
                            return (int)RacingProps.stt_prop_stunt_tube_jmp2;

                        case 30:
                            return (int)RacingProps.ba_prop_battle_tube_fn_01;

                        case 31:
                            return (int)RacingProps.ba_prop_battle_tube_fn_02;

                        case 32:
                            return (int)RacingProps.ba_prop_battle_tube_fn_03;

                        case 33:
                            return (int)RacingProps.ba_prop_battle_tube_fn_04;

                        case 34:
                            return (int)RacingProps.ba_prop_battle_tube_fn_05;

                        case 35:
                            return (int)RacingProps.stt_prop_stunt_tube_ent;

                        case 36:
                            return (int)RacingProps.stt_prop_stunt_tube_end;

                        case 37:
                            return (int)RacingProps.sr_prop_sr_tube_end;

                        case 38:
                            return (int)RacingProps.stt_prop_stunt_tube_speed;

                        case 39:
                            return (int)RacingProps.sr_prop_spec_tube_refill;

                        case 40:
                            return (int)RacingProps.stt_prop_track_tube_01;

                        case 41:
                            return (int)RacingProps.as_prop_as_tube_gap_03;
                        case 42:
                            return (int)RacingProps.as_prop_as_tube_xxs;
                    }
                    break;

                case 36:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.bkr_prop_weed_bigbag_01a;

                        case 1:
                            return (int)RacingProps.bkr_prop_meth_smallbag_01a;

                        case 2:
                            return (int)RacingProps.bkr_prop_weed_bucket_01a;

                        case 3:
                            return (int)RacingProps.bkr_prop_coke_boxeddoll;

                        case 4:
                            return (int)RacingProps.prop_keg_01;

                        case 5:
                            return (int)RacingProps.bkr_prop_coke_table01a;

                        case 6:
                            return (int)RacingProps.bkr_prop_meth_table01a;

                        case 7:
                            return (int)RacingProps.bkr_prop_meth_phosphorus;

                        case 8:
                            return (int)RacingProps.prop_meth_setup_01;

                        case 9:
                            return (int)RacingProps.bkr_prop_meth_pseudoephedrine;

                        case 10:
                            return (int)RacingProps.bkr_prop_weed_table_01a;

                        case 11:
                            return (int)RacingProps.bkr_prop_weed_bigbag_open_01a;

                        case 12:
                            return (int)RacingProps.bkr_prop_weed_scales_01a;

                        case 13:
                            return (int)RacingProps.bkr_prop_weed_lrg_01a;

                        case 14:
                            return (int)RacingProps.bkr_prop_weed_lrg_01b;

                        case 15:
                            return (int)RacingProps.bkr_prop_weed_med_01a;

                        case 16:
                            return (int)RacingProps.bkr_prop_weed_med_01b;

                        case 17:
                            return (int)RacingProps.bkr_prop_weed_01_small_01a;

                        case 18:
                            return (int)RacingProps.bkr_prop_weed_smallbag_01a;

                    }
                    break;

                case 37:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.gr_prop_gr_crates_pistols_01a;

                        case 1:
                            return (int)RacingProps.gr_prop_gr_crates_rifles_01a;

                        case 2:
                            return (int)RacingProps.gr_prop_gr_crates_rifles_02a;

                        case 3:
                            return (int)RacingProps.gr_prop_gr_crates_rifles_03a;

                        case 4:
                            return (int)RacingProps.gr_prop_gr_crates_rifles_04a;

                        case 5:
                            return (int)RacingProps.gr_prop_gr_crates_sam_01a;

                        case 6:
                            return (int)RacingProps.gr_prop_gr_crates_weapon_mix_01a;

                        case 7:
                            return (int)RacingProps.gr_prop_gr_gunsmithsupl_01a;

                        case 8:
                            return (int)RacingProps.gr_prop_gr_gunsmithsupl_02a;

                        case 9:
                            return (int)RacingProps.gr_prop_gr_gunsmithsupl_03a;

                        case 10:
                            return (int)RacingProps.gr_prop_gr_rsply_crate01a;

                        case 11:
                            return (int)RacingProps.gr_prop_gr_rsply_crate02a;

                        case 12:
                            return (int)RacingProps.gr_prop_gr_rsply_crate03a;

                        case 13:
                            return (int)RacingProps.hei_heist_apart2_door;

                        case 14:
                            return (int)RacingProps.prop_target_ora_purp_01;

                        case 15:
                            return (int)RacingProps.gr_prop_gr_target_01a;

                        case 16:
                            return (int)RacingProps.gr_prop_gr_target_01b;

                        case 17:
                            return (int)RacingProps.gr_prop_gr_target_02a;

                        case 18:
                            return (int)RacingProps.gr_prop_gr_target_02b;

                        case 19:
                            return (int)RacingProps.gr_prop_gr_bench_01a;

                        case 20:
                            return (int)RacingProps.gr_prop_gr_bench_01b;

                        case 21:
                            return (int)RacingProps.gr_prop_gr_bench_02a;

                        case 22:
                            return (int)RacingProps.gr_prop_gr_bench_02b;

                        case 23:
                            return (int)RacingProps.gr_prop_gr_speeddrill_01a;

                        case 24:
                            return (int)RacingProps.gr_prop_gr_vertmill_01a;

                        case 25:
                            return (int)RacingProps.gr_prop_gr_cratespile_01a;

                        case 26:
                            return (int)RacingProps.imp_prop_covered_vehicle_01a;

                        case 27:
                            return (int)RacingProps.imp_prop_covered_vehicle_04a;

                        case 28:
                            return (int)RacingProps.imp_prop_covered_vehicle_02a;

                        case 29:
                            return (int)RacingProps.imp_prop_covered_vehicle_05a;

                        case 30:
                            return (int)RacingProps.imp_prop_covered_vehicle_03a;

                        case 31:
                            return (int)RacingProps.imp_prop_covered_vehicle_06a;

                    }
                    break;

                case 21:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.sr_prop_spec_tube_xxs_01a;

                        case 1:
                            return (int)RacingProps.sr_prop_stunt_tube_xs_01a;

                        case 2:
                            return (int)RacingProps.sr_prop_spec_tube_s_01a;

                        case 3:
                            return (int)RacingProps.sr_prop_spec_tube_m_01a;

                        case 4:
                            return (int)RacingProps.sr_prop_spec_tube_l_01a;

                        case 5:
                            return (int)RacingProps.sr_prop_spec_tube_crn_01a;

                        case 6:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_5d_01a;

                        case 7:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_15d_01a;

                        case 8:
                            return (int)RacingProps.sr_prop_spec_tube_crn_30d_01a;

                        case 9:
                            return (int)RacingProps.sr_prop_stunt_tube_crn2_01a;

                        case 10:
                            return (int)RacingProps.sr_prop_spec_tube_xxs_02a;

                        case 11:
                            return (int)RacingProps.sr_prop_stunt_tube_xs_02a;

                        case 12:
                            return (int)RacingProps.sr_prop_spec_tube_s_02a;

                        case 13:
                            return (int)RacingProps.sr_prop_spec_tube_m_02a;

                        case 14:
                            return (int)RacingProps.sr_prop_spec_tube_l_02a;

                        case 15:
                            return (int)RacingProps.sr_prop_spec_tube_crn_02a;

                        case 16:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_5d_02a;

                        case 17:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_15d_02a;

                        case 18:
                            return (int)RacingProps.sr_prop_spec_tube_crn_30d_02a;

                        case 19:
                            return (int)RacingProps.sr_prop_stunt_tube_crn2_02a;

                        case 20:
                            return (int)RacingProps.sr_prop_spec_tube_xxs_03a;

                        case 21:
                            return (int)RacingProps.sr_prop_stunt_tube_xs_03a;

                        case 22:
                            return (int)RacingProps.sr_prop_spec_tube_s_03a;

                        case 23:
                            return (int)RacingProps.sr_prop_spec_tube_m_03a;

                        case 24:
                            return (int)RacingProps.sr_prop_spec_tube_l_03a;

                        case 25:
                            return (int)RacingProps.sr_prop_spec_tube_crn_03a;

                        case 26:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_5d_03a;

                        case 27:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_15d_03a;

                        case 28:
                            return (int)RacingProps.sr_prop_spec_tube_crn_30d_03a;

                        case 29:
                            return (int)RacingProps.sr_prop_stunt_tube_crn2_03a;

                        case 30:
                            return (int)RacingProps.sr_prop_spec_tube_xxs_04a;

                        case 31:
                            return (int)RacingProps.sr_prop_stunt_tube_xs_04a;

                        case 32:
                            return (int)RacingProps.sr_prop_spec_tube_s_04a;

                        case 33:
                            return (int)RacingProps.sr_prop_spec_tube_m_04a;

                        case 34:
                            return (int)RacingProps.sr_prop_spec_tube_l_04a;

                        case 35:
                            return (int)RacingProps.sr_prop_spec_tube_crn_04a;

                        case 36:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_5d_04a;

                        case 37:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_15d_04a;

                        case 38:
                            return (int)RacingProps.sr_prop_spec_tube_crn_30d_04a;

                        case 39:
                            return (int)RacingProps.sr_prop_stunt_tube_crn2_04a;

                        case 40:
                            return (int)RacingProps.sr_prop_spec_tube_xxs_05a;

                        case 41:
                            return (int)RacingProps.sr_prop_stunt_tube_xs_05a;

                        case 42:
                            return (int)RacingProps.sr_prop_spec_tube_s_05a;

                        case 43:
                            return (int)RacingProps.sr_prop_spec_tube_m_05a;

                        case 44:
                            return (int)RacingProps.sr_prop_spec_tube_l_05a;

                        case 45:
                            return (int)RacingProps.sr_prop_spec_tube_crn_05a;

                        case 46:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_5d_05a;

                        case 47:
                            return (int)RacingProps.sr_prop_stunt_tube_crn_15d_05a;

                        case 48:
                            return (int)RacingProps.sr_prop_spec_tube_crn_30d_05a;

                        case 49:
                            return (int)RacingProps.sr_prop_stunt_tube_crn2_05a;

                    }
                    break;

                case 28:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.sr_prop_special_bblock_sml1;

                        case 1:
                            return (int)RacingProps.sr_prop_special_bblock_mdm1;

                        case 2:
                            return (int)RacingProps.sr_prop_special_bblock_lrg11;

                        case 3:
                            return (int)RacingProps.sr_prop_special_bblock_xl1;

                        case 4:
                            return (int)RacingProps.sr_prop_special_bblock_sml2;

                        case 5:
                            return (int)RacingProps.sr_prop_special_bblock_mdm2;

                        case 6:
                            return (int)RacingProps.sr_prop_special_bblock_lrg2;

                        case 7:
                            return (int)RacingProps.sr_prop_special_bblock_xl2;

                        case 8:
                            return (int)RacingProps.sr_prop_special_bblock_sml3;

                        case 9:
                            return (int)RacingProps.sr_prop_special_bblock_mdm3;

                        case 10:
                            return (int)RacingProps.sr_prop_special_bblock_lrg3;

                        case 11:
                            return (int)RacingProps.sr_prop_special_bblock_xl3;

                        case 12:
                            return (int)RacingProps.sr_prop_special_bblock_xl3_fixed;

                        case 13:
                            return (int)RacingProps.vw_prop_vw_bblock_huge_01;

                        case 14:
                            return (int)RacingProps.vw_prop_vw_bblock_huge_02;

                        case 15:
                            return (int)RacingProps.vw_prop_vw_bblock_huge_03;

                        case 16:
                            return (int)RacingProps.vw_prop_vw_bblock_huge_04;

                        case 17:
                            return (int)RacingProps.vw_prop_vw_bblock_huge_05;

                        case 18:
                            return (int)RacingProps.ar_prop_ar_stunt_block_01a;

                        case 19:
                            return (int)RacingProps.ar_prop_ar_stunt_block_01b;

                    }
                    break;

                case 33:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.gr_prop_gr_target_1_01a;

                        case 1:
                            return (int)RacingProps.gr_prop_gr_target_1_01b;

                        case 2:
                            return (int)RacingProps.gr_prop_gr_target_2_04a;

                        case 3:
                            return (int)RacingProps.gr_prop_gr_target_2_04b;

                        case 4:
                            return (int)RacingProps.gr_prop_gr_target_3_03a;

                        case 5:
                            return (int)RacingProps.gr_prop_gr_target_3_03b;

                        case 6:
                            return (int)RacingProps.gr_prop_gr_target_4_01a;

                        case 7:
                            return (int)RacingProps.gr_prop_gr_target_4_01b;

                        case 8:
                            return (int)RacingProps.gr_prop_gr_target_5_01a;

                        case 9:
                            return (int)RacingProps.gr_prop_gr_target_5_01b;

                        case 10:
                            return (int)RacingProps.gr_prop_gr_target_small_01a;

                        case 11:
                            return (int)RacingProps.gr_prop_gr_target_small_01b;

                        case 12:
                            return (int)RacingProps.gr_prop_gr_target_small_03a;

                        case 13:
                            return (int)RacingProps.gr_prop_gr_target_small_02a;

                        case 14:
                            return (int)RacingProps.gr_prop_gr_target_small_06a;

                        case 15:
                            return (int)RacingProps.gr_prop_gr_target_small_07a;

                        case 16:
                            return (int)RacingProps.gr_prop_gr_target_small_04a;

                        case 17:
                            return (int)RacingProps.gr_prop_gr_target_small_05a;

                        case 18:
                            return (int)RacingProps.gr_prop_gr_target_long_01a;

                        case 19:
                            return (int)RacingProps.gr_prop_gr_target_large_01a;

                        case 20:
                            return (int)RacingProps.gr_prop_gr_target_large_01b;

                        case 21:
                            return (int)RacingProps.gr_prop_gr_target_trap_01a;

                        case 22:
                            return (int)RacingProps.gr_prop_gr_target_trap_02a;

                        case 23:
                            if (func_8935())
                            {
                                return (int)RacingProps.as_prop_as_stunt_target;
                            }
                            break;

                        case 24:
                            if (func_8935())
                            {
                                return (int)RacingProps.as_prop_as_stunt_target_small;
                            }
                            break;
                    }
                    break;

                case 23:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.ar_prop_ar_tube_xxs;

                        case 1:
                            return (int)RacingProps.ar_prop_ar_tube_xs;

                        case 2:
                            return (int)RacingProps.ar_prop_ar_tube_s;

                        case 3:
                            return (int)RacingProps.ar_prop_ar_tube_m;

                        case 4:
                            return (int)RacingProps.ar_prop_ar_tube_l;

                        case 5:
                            return (int)RacingProps.ar_prop_ar_tube_crn;

                        case 6:
                            return (int)RacingProps.ar_prop_ar_tube_crn_5d;

                        case 7:
                            return (int)RacingProps.ar_prop_ar_tube_crn_15d;

                        case 8:
                            return (int)RacingProps.ar_prop_ar_tube_crn_30d;

                        case 9:
                            return (int)RacingProps.ar_prop_ar_tube_crn2;

                        case 10:
                            return (int)RacingProps.ar_prop_ar_tube_qg;

                        case 11:
                            return (int)RacingProps.ar_prop_ar_tube_hg;

                        case 12:
                            return (int)RacingProps.ar_prop_ar_tube_jmp;

                        case 13:
                            return (int)RacingProps.as_prop_as_tube_gap_02;

                        case 14:
                            return (int)RacingProps.ar_prop_ar_tube_speed;

                        case 15:
                            return (int)RacingProps.ar_prop_ar_tube_fork;

                        case 16:
                            return (int)RacingProps.ar_prop_ar_tube_cross;

                        case 17:
                            return (int)RacingProps.ar_prop_ar_tube_2x_xxs;

                        case 18:
                            return (int)RacingProps.ar_prop_ar_tube_2x_xs;

                        case 19:
                            return (int)RacingProps.ar_prop_ar_tube_2x_s;

                        case 20:
                            return (int)RacingProps.ar_prop_ar_tube_2x_m;

                        case 21:
                            return (int)RacingProps.ar_prop_ar_tube_2x_l;

                        case 22:
                            return (int)RacingProps.ar_prop_ar_tube_2x_gap_02;

                        case 23:
                            return (int)RacingProps.ar_prop_ar_tube_2x_crn;

                        case 24:
                            return (int)RacingProps.ar_prop_ar_tube_2x_crn2;

                        case 25:
                            return (int)RacingProps.ar_prop_ar_tube_2x_crn_30d;

                        case 26:
                            return (int)RacingProps.ar_prop_ar_tube_2x_crn_15d;

                        case 27:
                            return (int)RacingProps.ar_prop_ar_tube_2x_crn_5d;

                        case 28:
                            return (int)RacingProps.ar_prop_ar_tube_2x_speed;

                        case 29:
                            return (int)RacingProps.ar_prop_ar_tube_4x_xxs;

                        case 30:
                            return (int)RacingProps.ar_prop_ar_tube_4x_xs;

                        case 31:
                            return (int)RacingProps.ar_prop_ar_tube_4x_s;

                        case 32:
                            return (int)RacingProps.ar_prop_ar_tube_4x_m;

                        case 33:
                            return (int)RacingProps.ar_prop_ar_tube_4x_l;

                        case 34:
                            return (int)RacingProps.ar_prop_ar_tube_4x_gap_02;

                        case 35:
                            return (int)RacingProps.ar_prop_ar_tube_4x_crn;

                        case 36:
                            return (int)RacingProps.ar_prop_ar_tube_4x_crn2;

                        case 37:
                            return (int)RacingProps.ar_prop_ar_tube_4x_crn_30d;

                        case 38:
                            return (int)RacingProps.ar_prop_ar_tube_4x_crn_15d;

                        case 39:
                            return (int)RacingProps.ar_prop_ar_tube_4x_crn_5d;

                        case 40:
                            return (int)RacingProps.ar_prop_ar_tube_4x_speed;

                    }
                    break;

                case 24:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.ar_prop_ar_checkpoint_xxs;

                        case 1:
                            return (int)RacingProps.ar_prop_ar_checkpoint_xs;

                        case 2:
                            return (int)RacingProps.ar_prop_ar_checkpoint_s;

                        case 3:
                            return (int)RacingProps.ar_prop_ar_checkpoint_m;

                        case 4:
                            return (int)RacingProps.ar_prop_ar_checkpoint_l;

                        case 5:
                            return (int)RacingProps.ar_prop_ar_checkpoint_crn;

                        case 6:
                            return (int)RacingProps.ar_prop_ar_checkpoints_crn_5d;

                        case 7:
                            return (int)RacingProps.ar_prop_ar_checkpoint_crn_15d;

                        case 8:
                            return (int)RacingProps.ar_prop_ar_checkpoint_crn_30d;

                        case 9:
                            return (int)RacingProps.ar_prop_ar_checkpoint_crn02;

                        case 10:
                            return (int)RacingProps.ar_prop_ar_checkpoint_fork;

                    }
                    break;

                case 25:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.ar_prop_ar_neon_gate_01a;

                        case 1:
                            return (int)RacingProps.ar_prop_ar_neon_gate_01b;

                        case 2:
                            return (int)RacingProps.ar_prop_ar_neon_gate_02a;

                        case 3:
                            return (int)RacingProps.ar_prop_ar_neon_gate_02b;

                        case 4:
                            return (int)RacingProps.ar_prop_ar_neon_gate_03a;

                        case 5:
                            return (int)RacingProps.ar_prop_ar_neon_gate_04a;

                        case 6:
                            return (int)RacingProps.ar_prop_ar_neon_gate_05a;

                    }
                    break;

                case 26:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.ar_prop_inflategates_cp;

                        case 1:
                            return (int)RacingProps.ar_prop_inflategates_cp_h1;

                        case 2:
                            return (int)RacingProps.ar_prop_inflategates_cp_h2;

                        case 3:
                            return (int)RacingProps.ar_prop_inflategates_cp_loop;

                        case 4:
                            return (int)RacingProps.ar_prop_inflategates_cp_loop_h1;

                        case 5:
                            return (int)RacingProps.ar_prop_inflategates_cp_loop_h2;

                        case 6:
                            return (int)RacingProps.ar_prop_inflategates_cp_loop_01a;

                        case 7:
                            return (int)RacingProps.ar_prop_inflategates_cp_loop_01b;

                        case 8:
                            return (int)RacingProps.ar_prop_inflategates_cp_loop_01c;

                        case 9:
                            return (int)RacingProps.ar_prop_gate_cp_90d;

                        case 10:
                            return (int)RacingProps.ar_prop_gate_cp_90d_h1;

                        case 11:
                            return (int)RacingProps.ar_prop_gate_cp_90d_h2;

                        case 12:
                            return (int)RacingProps.ar_prop_gate_cp_90d_01a;

                        case 13:
                            return (int)RacingProps.ar_prop_gate_cp_90d_01b;

                        case 14:
                            return (int)RacingProps.ar_prop_gate_cp_90d_01c;

                        case 15:
                            return (int)RacingProps.ar_prop_ig_sprunk_cp_single;

                        case 16:
                            return (int)RacingProps.ar_prop_ig_raine_cp_single;

                        case 17:
                            return (int)RacingProps.ar_prop_ig_flow_cp_single;

                        case 18:
                            return (int)RacingProps.ar_prop_ig_shark_cp_single;

                        case 19:
                            return (int)RacingProps.ar_prop_ig_jackal_cp_single;

                        case 20:
                            return (int)RacingProps.ar_prop_ig_metv_cp_single;

                        case 21:
                            return (int)RacingProps.ar_prop_ig_sprunk_cp_b;

                        case 22:
                            return (int)RacingProps.ar_prop_ig_raine_cp_b;

                        case 23:
                            return (int)RacingProps.ar_prop_ig_flow_cp_b;

                        case 24:
                            return (int)RacingProps.ar_prop_ig_shark_cp_b;

                        case 25:
                            return (int)RacingProps.ar_prop_ig_jackal_cp_b;

                        case 26:
                            return (int)RacingProps.ar_prop_ig_metv_cp_b;

                    }
                    break;

                case 35:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.sum_prop_track_pit_garage_05a;

                        case 1:
                            return (int)RacingProps.sum_prop_track_pit_garage_04a;

                        case 2:
                            return (int)RacingProps.sum_prop_track_pit_garage_03a;

                        case 3:
                            return (int)RacingProps.sum_prop_track_pit_garage_02a;

                        case 4:
                            return (int)RacingProps.ch_prop_track_pit_garage_01a;

                        case 5:
                            return (int)RacingProps.sum_prop_ac_track_paddock_01;

                        case 6:
                            return (int)RacingProps.sum_prop_ac_pit_garage_01a;

                        case 7:
                            return (int)RacingProps.sum_prop_ac_grandstand_01a;

                    }
                    break;

                case 38:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.prop_crate_11e;

                        case 1:
                            return (int)RacingProps.v_ret_ml_beerpat1;

                        case 2:
                            return (int)RacingProps.v_ret_ml_beerpis2;

                        case 3:
                            return (int)RacingProps.ba_prop_battle_crate_beer_01;

                        case 4:
                            return (int)RacingProps.ba_prop_battle_crate_beer_02;

                        case 5:
                            return (int)RacingProps.ba_prop_battle_crate_beer_03;

                        case 6:
                            return (int)RacingProps.ba_prop_battle_crate_beer_04;

                        case 7:
                            return (int)RacingProps.prop_dyn_pc;

                        case 8:
                            return (int)RacingProps.prop_dummy_01;

                        case 9:
                            return (int)RacingProps.prop_ped_gib_01;

                        case 10:
                            return (int)RacingProps.ba_prop_battle_ps_box_01;

                        case 11:
                            return (int)RacingProps.prop_wall_light_09a;

                        case 12:
                            return (int)RacingProps.ba_prop_battle_emis_rig_01;

                        case 13:
                            return (int)RacingProps.ba_prop_battle_emis_rig_02;

                        case 14:
                            return (int)RacingProps.ba_prop_battle_emis_rig_03;

                        case 15:
                            return (int)RacingProps.ba_prop_battle_emis_rig_04;

                        case 16:
                            return (int)RacingProps.vw_prop_vw_radiomast_01a;

                        case 17:
                            return (int)RacingProps.imp_prop_ship_01a;

                        case 18:
                            return (int)RacingProps.gr_prop_damship_01a;

                        case 19:
                            return (int)RacingProps.p_spinning_anus_s;

                        case 20:
                            return (int)RacingProps.vw_prop_vw_valet_01a;

                        case 21:
                            return (int)RacingProps.ch_prop_boring_machine_01a;

                        case 22:
                            return (int)RacingProps.ch_prop_boring_machine_01b;

                        case 23:
                            return (int)RacingProps.ch_prop_ch_cctv_wall_atta_01a;

                        case 24:
                            return (int)RacingProps.ch_prop_fingerprint_scanner_01a;

                        case 25:
                            return (int)RacingProps.ch_prop_fingerprint_scanner_01b;

                        case 26:
                            return (int)RacingProps.ch_prop_fingerprint_scanner_01c;

                        case 27:
                            return (int)RacingProps.ch_prop_fingerprint_scanner_01d;

                        case 28:
                            return (int)RacingProps.ch_prop_fingerprint_scanner_error_01b;

                        case 29:
                            return (int)RacingProps.ch_prop_ch_sec_cabinet_02a;

                        case 30:
                            return (int)RacingProps.ch_prop_ch_trolly_01a;

                        case 31:
                            return (int)RacingProps.ch_prop_ch_service_trolley_01a;

                        case 32:
                            return (int)RacingProps.ch_prop_ch_laundry_trolley_01a;

                        case 33:
                            return (int)RacingProps.ch_prop_ch_laundry_trolley_01b;

                        case 34:
                            return (int)RacingProps.v_corp_banktrolley;

                        case 35:
                            return (int)RacingProps.ch_prop_ch_maint_sign_01;

                        case 36:
                            return (int)RacingProps.sum_prop_ac_barge_01;

                        case 37:
                            return (int)RacingProps.sum_prop_ac_ind_light_02a;

                        case 38:
                            return (int)RacingProps.sum_prop_ac_ind_light_03c;

                        case 39:
                            return (int)RacingProps.sum_prop_ac_ind_light_04;

                        case 40:
                            return (int)RacingProps.sum_prop_ac_wall_light_09a;

                        case 41:
                            return (int)RacingProps.h4_prop_h4_tannoy_01a;

                        case 42:
                            return (int)RacingProps.h4_prop_h4_sign_cctv_01a;

                        case 43:
                            return (int)RacingProps.h4_prop_h4_sub_kos;

                        case 44:
                            return (int)RacingProps.h4_prop_h4_sec_cabinet_dum;

                        case 45:
                            return (int)RacingProps.h4_prop_h4_loch_monster;

                        case 46:
                            return (int)RacingProps.h4_prop_h4_cctv_pole_04;

                        case 47:
                            return (int)RacingProps.h4_prop_h4_t_bottle_02a;

                        case 48:
                            return (int)RacingProps.h4_prop_h4_neck_disp_01a;

                        case 49:
                            return (int)RacingProps.h4_prop_h4_necklace_01a;

                        case 50:
                            return (int)RacingProps.h4_prop_h4_art_pant_01a;

                        case 51:
                            return (int)RacingProps.h4_prop_h4_diamond_disp_01a;

                        case 52:
                            return (int)RacingProps.h4_prop_h4_diamond_01a;

                    }
                    break;

                case 39:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.xs_prop_arena_pit_fire_01a;

                        case 1:
                            return (int)RacingProps.xs_prop_arena_pit_fire_02a;

                        case 2:
                            return (int)RacingProps.xs_prop_arena_pit_fire_03a;

                        case 3:
                            return (int)RacingProps.xs_prop_arena_pit_fire_04a;

                        case 4:
                            return (int)RacingProps.xs_prop_arena_flipper_small_01a;

                        case 5:
                            return (int)RacingProps.xs_prop_arena_flipper_large_01a;

                        case 6:
                            return (int)RacingProps.xs_prop_arena_flipper_xl_01a;

                        case 7:
                            return (int)RacingProps.xs_prop_arena_bollard_rising_01a;

                        case 8:
                            return (int)RacingProps.xs_prop_arena_bollard_rising_01b;

                        case 9:
                            return (int)RacingProps.xs_prop_arena_bollard_side_01a;

                        case 10:
                            return (int)RacingProps.xs_prop_arena_pit_double_01b;

                        case 11:
                            return (int)RacingProps.xs_prop_arena_landmine_01a;

                        case 12:
                            return (int)RacingProps.xs_prop_arena_barrel_01a;

                        case 13:
                            return (int)RacingProps.xs_prop_arena_landmine_03a;

                        case 14:
                            return (int)RacingProps.xs_prop_arena_landmine_01c;

                        case 15:
                            return (int)RacingProps.xs_prop_arena_turntable_01a;

                        case 16:
                            return (int)RacingProps.xs_prop_arena_turntable_02a;

                        case 17:
                            return (int)RacingProps.xs_prop_arena_turntable_03a;

                        case 18:
                            return (int)RacingProps.xs_prop_arena_wall_rising_01a;

                        case 19:
                            return (int)RacingProps.xs_prop_arena_wall_rising_02a;

                    }
                    break;

                case 40:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.xs_prop_arena_pit_fire_01a_sf;

                        case 1:
                            return (int)RacingProps.xs_prop_arena_pit_fire_02a_sf;

                        case 2:
                            return (int)RacingProps.xs_prop_arena_pit_fire_03a_sf;

                        case 3:
                            return (int)RacingProps.xs_prop_arena_pit_fire_04a_sf;

                        case 4:
                            return (int)RacingProps.xs_prop_arena_flipper_small_01a_sf;

                        case 5:
                            return (int)RacingProps.xs_prop_arena_flipper_large_01a_sf;

                        case 6:
                            return (int)RacingProps.xs_prop_arena_flipper_xl_01a_sf;

                        case 7:
                            return (int)RacingProps.xs_prop_arena_bollard_rising_01a_sf;

                        case 8:
                            return (int)RacingProps.xs_prop_arena_bollard_rising_01b_sf;

                        case 9:
                            return (int)RacingProps.xs_prop_arena_bollard_side_01a_sf;

                        case 10:
                            return (int)RacingProps.xs_prop_arena_pit_double_01b_sf;

                        case 11:
                            return (int)RacingProps.xs_prop_arena_landmine_01c_sf;

                        case 12:
                            return (int)RacingProps.xs_prop_arena_barrel_01a_sf;

                        case 13:
                            return (int)RacingProps.xs_prop_arena_landmine_03a_sf;

                        case 14:
                            return (int)RacingProps.xs_prop_arena_turntable_01a_sf;

                        case 15:
                            return (int)RacingProps.vw_prop_arena_turntable_02f_sf;

                        case 16:
                            return (int)RacingProps.xs_prop_arena_turntable_03a_sf;

                        case 17:
                            return (int)RacingProps.xs_prop_arena_wall_rising_01a_sf;

                        case 18:
                            return (int)RacingProps.xs_prop_arena_wall_rising_02a_sf;

                    }
                    break;

                case 41:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.xs_prop_arena_pit_fire_01a_wl;

                        case 1:
                            return (int)RacingProps.xs_prop_arena_pit_fire_02a_wl;

                        case 2:
                            return (int)RacingProps.xs_prop_arena_pit_fire_03a_wl;

                        case 3:
                            return (int)RacingProps.xs_prop_arena_pit_fire_04a_wl;

                        case 4:
                            return (int)RacingProps.xs_prop_arena_flipper_small_01a_wl;

                        case 5:
                            return (int)RacingProps.xs_prop_arena_flipper_large_01a_wl;

                        case 6:
                            return (int)RacingProps.xs_prop_arena_flipper_xl_01a_wl;

                        case 7:
                            return (int)RacingProps.xs_prop_arena_bollard_rising_01a_wl;

                        case 8:
                            return (int)RacingProps.xs_prop_arena_bollard_rising_01b_wl;

                        case 9:
                            return (int)RacingProps.xs_prop_arena_bollard_side_01a_wl;

                        case 10:
                            return (int)RacingProps.xs_prop_arena_pit_double_01b_wl;

                        case 11:
                            return (int)RacingProps.xs_prop_arena_landmine_01c_wl;

                        case 12:
                            return (int)RacingProps.xs_prop_arena_barrel_01a_wl;

                        case 13:
                            return (int)RacingProps.xs_prop_arena_landmine_03a_wl;

                        case 14:
                            return (int)RacingProps.xs_prop_arena_turntable_01a_wl;

                        case 15:
                            return (int)RacingProps.xs_prop_arena_turntable_02a_wl;

                        case 16:
                            return (int)RacingProps.xs_prop_arena_turntable_03a_wl;

                        case 17:
                            return (int)RacingProps.xs_prop_arena_wall_rising_01a_wl;

                        case 18:
                            return (int)RacingProps.xs_prop_arena_wall_rising_02a_wl;

                    }
                    break;

                case 42:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.xs_prop_arena_pressure_plate_01a;

                        case 1:
                            return (int)RacingProps.xs_prop_arena_car_wall_01a;

                        case 2:
                            return (int)RacingProps.xs_prop_arena_car_wall_02a;

                        case 3:
                            return (int)RacingProps.xs_prop_arena_car_wall_03a;

                        case 4:
                            return (int)RacingProps.xs_prop_arena_station_01a;

                        case 5:
                            return (int)RacingProps.xs_prop_arena_station_02a;

                        case 6:
                            return (int)RacingProps.xs_prop_arena_wedge_01a;

                        case 7:
                            return (int)RacingProps.xs_prop_arena_oil_jack_01a;

                        case 8:
                            return (int)RacingProps.xs_prop_arena_oil_jack_02a;

                        case 9:
                            return (int)RacingProps.xs_prop_arena_wall_01c;

                        case 10:
                            return (int)RacingProps.xs_prop_arena_wall_01b;

                        case 11:
                            return (int)RacingProps.xs_prop_arena_wall_01a;

                        case 12:
                            return (int)RacingProps.xs_prop_arena_building_01a;

                        case 13:
                            return (int)RacingProps.xs_prop_arena_industrial_a;

                        case 14:
                            return (int)RacingProps.xs_prop_arena_industrial_b;

                        case 15:
                            return (int)RacingProps.xs_prop_arena_industrial_c;

                        case 16:
                            return (int)RacingProps.xs_prop_arena_industrial_d;

                        case 17:
                            return (int)RacingProps.xs_prop_arena_industrial_e;

                        case 18:
                            return (int)RacingProps.xs_prop_arena_1bay_01a;

                        case 19:
                            return (int)RacingProps.xs_prop_arena_2bay_01a;

                        case 20:
                            return (int)RacingProps.xs_prop_arena_3bay_01a;

                        case 21:
                            return (int)RacingProps.xs_prop_arena_goal;

                        case 22:
                            return (int)RacingProps.xs_prop_arena_gate_01a;

                        case 23:
                            return (int)RacingProps.xs_prop_arena_tower_01a;

                        case 24:
                            return (int)RacingProps.xs_prop_arena_tower_02a;

                        case 25:
                            return (int)RacingProps.xs_prop_arena_startgate_01a;

                        case 26:
                            return (int)RacingProps.xs_prop_arena_arrow_01a;

                        case 27:
                            return (int)RacingProps.xs_prop_arena_wall_02a;

                        case 28:
                            return (int)RacingProps.xs_prop_arena_tower_04a;

                        case 29:
                            return (int)RacingProps.xs_prop_wall_tyre_01a;

                        case 30:
                            return (int)RacingProps.xs_prop_wall_tyre_l_01a;

                        case 31:
                            return (int)RacingProps.xs_prop_wall_tyre_start_01a;

                        case 32:
                            return (int)RacingProps.xs_prop_wall_tyre_end_01a;

                        case 33:
                            return (int)RacingProps.xs_prop_arrow_tyre_01a;

                        case 34:
                            return (int)RacingProps.xs_prop_arrow_tyre_01b;

                        case 35:
                            return 11714146;

                        case 36:
                            return (int)RacingProps.xs_prop_arena_pipe_track_c_01a;

                        case 37:
                            return (int)RacingProps.xs_prop_arena_pipe_track_c_01b;

                        case 38:
                            return (int)RacingProps.xs_prop_arena_pipe_track_c_01c;

                        case 39:
                            return (int)RacingProps.xs_prop_arena_pipe_track_c_01d;

                        case 40:
                            return (int)RacingProps.xs_prop_arena_pipe_track_s_01a;

                        case 41:
                            return (int)RacingProps.xs_prop_arena_pipe_track_s_01b;

                        case 42:
                            return (int)RacingProps.xs_prop_arena_pipe_bend_01a;

                        case 43:
                            return (int)RacingProps.xs_prop_arena_pipe_bend_01b;

                        case 44:
                            return (int)RacingProps.xs_prop_arena_pipe_bend_01c;

                        case 45:
                            return (int)RacingProps.xs_prop_arena_pipe_bend_02a;

                        case 46:
                            return (int)RacingProps.xs_prop_arena_pipe_bend_02b;

                        case 47:
                            return (int)RacingProps.xs_prop_arena_pipe_bend_02c;

                        case 48:
                            return (int)RacingProps.xs_prop_arena_pipe_end_01a;

                        case 49:
                            return (int)RacingProps.xs_prop_arena_pipe_end_02a;

                        case 50:
                            return (int)RacingProps.xs_prop_arena_pipe_machine_01a;

                        case 51:
                            return (int)RacingProps.xs_prop_arena_pipe_machine_02a;

                        case 52:
                            return (int)RacingProps.xs_prop_arena_pipe_straight_01a;

                        case 53:
                            return (int)RacingProps.xs_prop_arena_pipe_straight_01b;

                        case 54:
                            return (int)RacingProps.xs_prop_arena_pipe_straight_02a;

                        case 55:
                            return (int)RacingProps.xs_prop_arena_pipe_straight_02b;

                        case 56:
                            return (int)RacingProps.xs_prop_arena_pipe_straight_02c;

                        case 57:
                            return (int)RacingProps.xs_prop_arena_pipe_straight_02d;

                        case 58:
                            return (int)RacingProps.xs_prop_arena_pipe_transition_01a;

                        case 59:
                            return (int)RacingProps.xs_prop_arena_pipe_transition_01b;

                        case 60:
                            return (int)RacingProps.xs_prop_arena_pipe_transition_01c;

                        case 61:
                            return (int)RacingProps.xs_prop_arena_pipe_transition_02a;

                        case 62:
                            return (int)RacingProps.xs_prop_arena_pipe_transition_02b;

                        case 63:
                            return (int)RacingProps.xs_prop_barrier_5m_01a;

                        case 64:
                            return (int)RacingProps.xs_prop_barrier_10m_01a;

                        case 65:
                            return (int)RacingProps.xs_prop_barrier_15m_01a;

                        case 66:
                            return (int)RacingProps.xs_prop_ar_tunnel_01a;

                        case 67:
                            return (int)RacingProps.xs_prop_arena_spikes_01a;

                    }
                    break;

                case 45:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.xs_prop_arena_jump_xs_01a;

                        case 1:
                            return (int)RacingProps.xs_prop_arena_jump_s_01a;

                        case 2:
                            return (int)RacingProps.xs_prop_arena_jump_m_01a;

                        case 3:
                            return (int)RacingProps.xs_prop_arena_jump_l_01a;

                        case 4:
                            return (int)RacingProps.xs_prop_arena_jump_xl_01a;

                        case 5:
                            return (int)RacingProps.xs_prop_arena_jump_xxl_01a;

                        case 6:
                            return (int)RacingProps.xs_prop_arena_adj_hloop;

                        case 7:
                            return (int)RacingProps.xs_prop_arena_jump_02b;

                        case 8:
                            return (int)RacingProps.xs_prop_arena_pipe_ramp_01a;

                        case 9:
                            return (int)RacingProps.xs_prop_arena_jump_xs_01a_sf;

                        case 10:
                            return (int)RacingProps.xs_prop_arena_jump_s_01a_sf;

                        case 11:
                            return (int)RacingProps.xs_prop_arena_jump_m_01a_sf;

                        case 12:
                            return (int)RacingProps.xs_prop_arena_jump_l_01a_sf;

                        case 13:
                            return (int)RacingProps.xs_prop_arena_jump_xl_01a_sf;

                        case 14:
                            return (int)RacingProps.xs_prop_arena_jump_xxl_01a_sf;

                        case 15:
                            return (int)RacingProps.xs_prop_arena_adj_hloop_sf;

                        case 16:
                            return (int)RacingProps.xs_prop_arena_jump_xs_01a_wl;

                        case 17:
                            return (int)RacingProps.xs_prop_arena_jump_s_01a_wl;

                        case 18:
                            return (int)RacingProps.xs_prop_arena_jump_m_01a_wl;

                        case 19:
                            return (int)RacingProps.xs_prop_arena_jump_l_01a_wl;

                        case 20:
                            return (int)RacingProps.xs_prop_arena_jump_xl_01a_wl;

                        case 21:
                            return (int)RacingProps.xs_prop_arena_jump_xxl_01a_wl;

                        case 22:
                            return (int)RacingProps.xs_prop_arena_adj_hloop_wl;

                    }
                    break;

                case 43:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.xs_prop_arena_pressure_plate_01a_sf;

                        case 1:
                            return (int)RacingProps.xs_prop_arena_wedge_01a_sf;

                        case 2:
                            return (int)RacingProps.xs_prop_arena_goal_sf;

                        case 3:
                            return (int)RacingProps.xs_prop_arena_wall_02a_sf;

                        case 4:
                            return (int)RacingProps.xs_prop_arrow_tyre_01a_sf;

                        case 5:
                            return (int)RacingProps.xs_prop_arrow_tyre_01b_sf;

                        case 6:
                            return (int)RacingProps.xs_prop_arena_arrow_01a_sf;

                        case 7:
                            return (int)RacingProps.xs_prop_arena_startgate_01a_sf;

                        case 8:
                            return (int)RacingProps.xs_prop_ar_planter_s_45a_sf;

                        case 9:
                            return (int)RacingProps.xs_prop_ar_planter_s_90a_sf;

                        case 10:
                            return (int)RacingProps.xs_prop_ar_planter_s_180a_sf;

                        case 11:
                            return (int)RacingProps.xs_prop_ar_planter_m_30a_sf;

                        case 12:
                            return (int)RacingProps.xs_prop_ar_planter_m_30b_sf;

                        case 13:
                            return (int)RacingProps.xs_prop_ar_planter_m_60a_sf;

                        case 14:
                            return (int)RacingProps.xs_prop_ar_planter_m_60b_sf;

                        case 15:
                            return (int)RacingProps.xs_prop_ar_planter_m_90a_sf;

                        case 16:
                            return (int)RacingProps.xs_prop_ar_tower_01a_sf;

                        case 17:
                            return (int)RacingProps.xs_prop_ar_pipe_conn_01a_sf;

                        case 18:
                            return (int)RacingProps.xs_prop_ar_stand_thick_01a_sf;

                        case 19:
                            return (int)RacingProps.xs_prop_ar_pipe_01a_sf;

                        case 20:
                            return (int)RacingProps.xs_prop_ar_gate_01a_sf;

                        case 21:
                            return (int)RacingProps.xs_prop_ar_planter_xl_01a_sf;

                        case 22:
                            return (int)RacingProps.xs_prop_ar_planter_m_01a_sf;

                        case 23:
                            return (int)RacingProps.xs_prop_ar_planter_s_01a_sf;

                        case 24:
                            return (int)RacingProps.xs_prop_ar_planter_c_01a_sf;

                        case 25:
                            return (int)RacingProps.xs_prop_ar_planter_c_02a_sf;

                        case 26:
                            return (int)RacingProps.xs_prop_ar_planter_c_03a_sf;

                        case 27:
                            return (int)RacingProps.xs_prop_ar_buildingx_01a_sf;

                        case 28:
                            return (int)RacingProps.xs_prop_ar_tunnel_01a_sf;

                        case 29:
                            return (int)RacingProps.xs_prop_arena_spikes_01a_sf;

                    }
                    break;

                case 44:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.xs_prop_arena_pressure_plate_01a_wl;

                        case 1:
                            return (int)RacingProps.xs_prop_arena_wedge_01a_wl;

                        case 2:
                            return (int)RacingProps.xs_prop_arena_wall_02a_wl;

                        case 3:
                            return (int)RacingProps.xs_prop_arrow_tyre_01a_wl;

                        case 4:
                            return (int)RacingProps.xs_prop_arrow_tyre_01b_wl;

                        case 5:
                            return (int)RacingProps.xs_prop_arena_arrow_01a_wl;

                        case 6:
                            return (int)RacingProps.xs_prop_ar_tunnel_01a_wl;

                        case 7:
                            return (int)RacingProps.xs_prop_lplate_01a_wl;

                        case 8:
                            return (int)RacingProps.xs_prop_lplate_bend_01a_wl;

                        case 9:
                            return (int)RacingProps.xs_prop_lplate_wall_01a_wl;

                        case 10:
                            return (int)RacingProps.xs_prop_lplate_wall_01b_wl;

                        case 11:
                            return (int)RacingProps.xs_prop_lplate_wall_01c_wl;

                        case 12:
                            return (int)RacingProps.xs_prop_beer_bottle_wl;

                        case 13:
                            return (int)RacingProps.xs_prop_burger_meat_wl;

                        case 14:
                            return (int)RacingProps.xs_prop_can_tunnel_wl;

                        case 15:
                            return (int)RacingProps.xs_prop_can_wl;

                        case 16:
                            return (int)RacingProps.xs_prop_chips_tube_wl;

                        case 17:
                            return (int)RacingProps.xs_prop_chopstick_wl;

                        case 18:
                            return (int)RacingProps.xs_prop_gate_tyre_01a_wl;

                        case 19:
                            return (int)RacingProps.xs_prop_hamburgher_wl;

                        case 20:
                            return (int)RacingProps.xs_prop_nacho_wl;

                    }
                    break;

                case 46:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.xs_prop_arena_fence_01a;

                        case 1:
                            return (int)RacingProps.xs_prop_arena_fence_01a_sf;

                        case 2:
                            return (int)RacingProps.xs_prop_arena_fence_01a_wl;

                    }
                    break;

                case 47:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.xs_prop_arena_turret_01a;

                        case 1:
                            return (int)RacingProps.xs_prop_arena_turret_01a_sf;

                        case 2:
                            return (int)RacingProps.xs_prop_arena_turret_01a_wl;

                        case 3:
                            return (int)RacingProps.xs_prop_arena_finish_line;

                    }
                    break;

                case 48:
                    switch (iParam1)
                    {
                        case 0:
                            return (int)RacingProps.ch_prop_ch_cctv_cam_01a;

                        case 1:
                            return (int)RacingProps.ch_prop_ch_cctv_cam_02a;

                        case 2:
                            return (int)RacingProps.xm_prop_x17_server_farm_cctv_01;

                        case 3:
                            return (int)RacingProps.hei_prop_bank_cctv_01;

                        case 4:
                            return (int)RacingProps.hei_prop_bank_cctv_02;

                        case 5:
                            return (int)RacingProps.prop_cctv_cam_05a;

                    }
                    break;

                case 49:
                    switch (iParam1)
                    {
                        case 0:
                            return 820707985;

                        case 1:
                            return 11747150;

                        case 2:
                            return 1444283207;

                        case 3:
                            return -664841990;

                        case 4:
                            return 1811127662;

                        case 5:
                            return -2132967520;

                        case 6:
                            return -1730847084;

                        case 7:
                            return -37343207;

                        case 8:
                            return -1371067497;

                        case 9:
                            return 719882629;

                        case 10:
                            return 581945895;

                        case 11:
                            return -1639321128;

                        case 12:
                            return 669536853;

                        case 13:
                            return 1510880928;

                        case 14:
                            return 1439575588;

                        case 15:
                            return 1074266776;

                        case 16:
                            return 1900799263;

                        case 17:
                            return 1803344257;

                        case 18:
                            return -1133355695;

                        case 19:
                            return -1955916202;

                        case 20:
                            return 251420015;

                        case 21:
                            return -451866472;

                        case 22:
                            return -654667398;

                        case 23:
                            return 896512151;

                        case 24:
                            return 940227121;

                        case 25:
                            return -337605007;

                        case 26:
                            return 1281409573;

                    }
                    break;
            }
            return 0;
        }

        #region Names

        public static string GetPropName(int iParam0)
        {
            if (iParam0 == (int)RacingProps.prop_const_fence02b)
                return Game.GetGXTEntry("FMMC_PR_0");
            if (iParam0 == (int)RacingProps.prop_offroad_bale03)
                return Game.GetGXTEntry("FMMC_PR_1");
            if (iParam0 == (int)RacingProps.prop_offroad_bale02)
                return Game.GetGXTEntry("FMMC_PR_2");
            if (iParam0 == (int)RacingProps.prop_offroad_bale01)
                return Game.GetGXTEntry("FMMC_PR_3");
            if (iParam0 == (int)RacingProps.prop_offroad_tyres01)
                return Game.GetGXTEntry("FMMC_PR_4");
            if (iParam0 == (int)RacingProps.lts_prop_lts_offroad_tyres01)
                return Game.GetGXTEntry("FMMC_PR_4");
            if (iParam0 == (int)RacingProps.prop_offroad_tyres02)
                return Game.GetGXTEntry("FMMC_PR_5");
            if (iParam0 == (int)RacingProps.prop_barier_conc_02a)
                return Game.GetGXTEntry("FMMC_PR_6");
            if (iParam0 == (int)RacingProps.prop_barier_conc_05c)
                return Game.GetGXTEntry("FMMC_PR_7");
            if (iParam0 == (int)RacingProps.prop_barier_conc_05a)
                return Game.GetGXTEntry("FMMC_PR_8");
            if (iParam0 == (int)RacingProps.prop_barier_conc_05b)
                return Game.GetGXTEntry("FMMC_PR_9");
            if (iParam0 == (int)RacingProps.prop_barier_conc_01a)
                return Game.GetGXTEntry("FMMC_PR_10");
            if (iParam0 == (int)RacingProps.prop_bench_05)
                return Game.GetGXTEntry("FMMC_PR_12");
            if (iParam0 == (int)RacingProps.prop_bench_07)
                return Game.GetGXTEntry("FMMC_PR_13");
            if (iParam0 == (int)RacingProps.prop_bench_01a)
                return Game.GetGXTEntry("FMMC_PR_14");
            if (iParam0 == (int)RacingProps.prop_bench_08)
                return Game.GetGXTEntry("FMMC_PR_BNBLUE");
            if (iParam0 == (int)RacingProps.prop_dock_bouy_1)
                return Game.GetGXTEntry("FMMC_PR_15");
            if (iParam0 == (int)RacingProps.prop_dock_bouy_2)
                return Game.GetGXTEntry("FMMC_PR_16");
            if (iParam0 == (int)RacingProps.prop_elecbox_24)
                return Game.GetGXTEntry("FMMC_PR_17");
            if (iParam0 == (int)RacingProps.prop_elecbox_24b)
                return Game.GetGXTEntry("FMMC_PR_18");
            if (iParam0 == (int)RacingProps.prop_portacabin01)
                return Game.GetGXTEntry("FMMC_PR_19");
            if (iParam0 == (int)RacingProps.prop_cementbags01)
                return Game.GetGXTEntry("FMMC_PR_20");
            if (iParam0 == (int)RacingProps.prop_conc_blocks01a)
                return Game.GetGXTEntry("FMMC_PR_21");
            if (iParam0 == (int)RacingProps.prop_cons_crate)
                return Game.GetGXTEntry("FMMC_PR_22");
            if (iParam0 == (int)RacingProps.prop_jyard_block_01a)
                return Game.GetGXTEntry("FMMC_PR_23");
            if (iParam0 == (int)RacingProps.prop_conc_sacks_02a)
                return Game.GetGXTEntry("FMMC_PR_24");
            if (iParam0 == (int)RacingProps.prop_byard_sleeper01)
                return Game.GetGXTEntry("FMMC_PR_25");
            if (iParam0 == (int)RacingProps.prop_shuttering01)
                return Game.GetGXTEntry("FMMC_PR_26");
            if (iParam0 == (int)RacingProps.prop_shuttering02)
                return Game.GetGXTEntry("FMMC_PR_27");
            if (iParam0 == (int)RacingProps.prop_shuttering03)
                return Game.GetGXTEntry("FMMC_PR_28");
            if (iParam0 == (int)RacingProps.prop_shuttering04)
                return Game.GetGXTEntry("FMMC_PR_29");
            if (iParam0 == (int)RacingProps.prop_woodpile_01a)
                return Game.GetGXTEntry("FMMC_PR_30");
            if (iParam0 == (int)RacingProps.prop_woodpile_01c)
                return Game.GetGXTEntry("FMMC_PR_31");
            if (iParam0 == (int)RacingProps.prop_rub_cont_01b)
                return Game.GetGXTEntry("FMMC_PR_32");
            if (iParam0 == (int)RacingProps.prop_rail_boxcar4)
                return Game.GetGXTEntry("FMMC_PR_33");
            if (iParam0 == (int)RacingProps.prop_rub_railwreck_2)
                return Game.GetGXTEntry("FMMC_PR_34");
            if (iParam0 == (int)RacingProps.prop_contr_03b_ld)
                return Game.GetGXTEntry("FMMC_PR_35");
            if (iParam0 == (int)RacingProps.prop_container_ld2)
                return Game.GetGXTEntry("FMMC_PR_36");
            if (iParam0 == (int)RacingProps.prop_rail_boxcar)
                return Game.GetGXTEntry("FMMC_PR_37");
            if (iParam0 == (int)RacingProps.prop_rail_boxcar3)
                return Game.GetGXTEntry("FMMC_PR_38");
            if (iParam0 == (int)RacingProps.prop_byard_floatpile)
                return Game.GetGXTEntry("FMMC_PR_39");
            if (iParam0 == (int)RacingProps.prop_boxpile_07a)
                return Game.GetGXTEntry("FMMC_PR_40");
            if (iParam0 == (int)RacingProps.prop_watercrate_01)
                return Game.GetGXTEntry("FMMC_PR_41");
            if (iParam0 == (int)RacingProps.prop_box_wood01a)
                return Game.GetGXTEntry("FMMC_PR_42");
            if (iParam0 == (int)RacingProps.prop_box_wood03a)
                return Game.GetGXTEntry("FMMC_PR_43");
            if (iParam0 == (int)RacingProps.prop_box_wood04a)
                return Game.GetGXTEntry("FMMC_PR_44");
            if (iParam0 == (int)RacingProps.prop_cash_crate_01)
                return Game.GetGXTEntry("FMMC_PR_45");
            if (iParam0 == (int)RacingProps.prop_bin_13a)
                return Game.GetGXTEntry("FMMC_PR_46");
            if (iParam0 == (int)RacingProps.prop_bin_14a)
                return Game.GetGXTEntry("FMMC_PR_47");
            if (iParam0 == (int)RacingProps.prop_dumpster_3a)
                return Game.GetGXTEntry("FMMC_PR_48");
            if (iParam0 == (int)RacingProps.prop_dumpster_4b)
                return Game.GetGXTEntry("FMMC_PR_49");
            if (iParam0 == (int)RacingProps.prop_dumpster_01a)
                return Game.GetGXTEntry("FMMC_PR_50");
            if (iParam0 == (int)RacingProps.prop_skip_06a)
                return Game.GetGXTEntry("FMMC_PR_51");
            if (iParam0 == (int)RacingProps.prop_elecbox_02a)
                return Game.GetGXTEntry("FMMC_PR_52");
            if (iParam0 == (int)RacingProps.prop_elecbox_16)
                return Game.GetGXTEntry("FMMC_PR_53");
            if (iParam0 == (int)RacingProps.prop_elecbox_14)
                return Game.GetGXTEntry("FMMC_PR_54");
            if (iParam0 == (int)RacingProps.prop_ind_deiseltank)
                return Game.GetGXTEntry("FMMC_PR_55");
            if (iParam0 == (int)RacingProps.prop_ind_mech_02a)
                return Game.GetGXTEntry("FMMC_PR_56");
            if (iParam0 == (int)RacingProps.prop_ind_mech_02b)
                return Game.GetGXTEntry("FMMC_PR_57");
            if (iParam0 == (int)RacingProps.prop_sub_trans_01a)
                return Game.GetGXTEntry("FMMC_PR_58");
            if (iParam0 == (int)RacingProps.prop_sub_trans_02a)
                return Game.GetGXTEntry("FMMC_PR_59");
            if (iParam0 == (int)RacingProps.prop_sub_trans_04a)
                return Game.GetGXTEntry("FMMC_PR_60");
            if (iParam0 == (int)RacingProps.prop_skip_04)
                return Game.GetGXTEntry("FMMC_PR_62");
            if (iParam0 == (int)RacingProps.prop_mp_ramp_01 || iParam0 == (int)RacingProps.lts_prop_lts_ramp_01)
                return Game.GetGXTEntry("FMMC_PR_61");
            if (iParam0 == (int)RacingProps.prop_mp_ramp_02 || iParam0 == (int)RacingProps.lts_prop_lts_ramp_02)
                return Game.GetGXTEntry("FMMC_PR_63");
            if (iParam0 == (int)RacingProps.prop_mp_ramp_03 || iParam0 == (int)RacingProps.lts_prop_lts_ramp_03)
                return Game.GetGXTEntry("FMMC_PR_64");
            if (iParam0 == (int)RacingProps.prop_skip_08a)
                return Game.GetGXTEntry("FMMC_PR_65");
            if (iParam0 == (int)RacingProps.prop_jetski_ramp_01)
                return Game.GetGXTEntry("FMMC_PR_666");
            if (iParam0 == (int)RacingProps.ar_prop_ar_jetski_ramp_01_dev)
                return Game.GetGXTEntry("FMMC_PR_66");
            if (iParam0 == (int)RacingProps.prop_trafficdiv_01)
                return Game.GetGXTEntry("FMMC_PR_67");
            if (iParam0 == (int)RacingProps.prop_sign_road_09a)
                return Game.GetGXTEntry("FMMC_PR_68");
            if (iParam0 == (int)RacingProps.prop_sign_road_09b)
                return Game.GetGXTEntry("FMMC_PR_69");
            if (iParam0 == (int)RacingProps.prop_sign_road_09c)
                return Game.GetGXTEntry("FMMC_PR_70");
            if (iParam0 == (int)RacingProps.prop_sign_road_09d)
                return Game.GetGXTEntry("FMMC_PR_71");
            if (iParam0 == (int)RacingProps.prop_sign_road_06q)
                return Game.GetGXTEntry("FMMC_PR_72");
            if (iParam0 == (int)RacingProps.prop_sign_road_06r)
                return Game.GetGXTEntry("FMMC_PR_73");
            if (iParam0 == (int)RacingProps.prop_sign_road_05c)
                return Game.GetGXTEntry("FMMC_PR_74");
            if (iParam0 == (int)RacingProps.prop_sign_road_05d)
                return Game.GetGXTEntry("FMMC_PR_75");
            if (iParam0 == (int)RacingProps.prop_sign_road_05e)
                return Game.GetGXTEntry("FMMC_PR_76");
            if (iParam0 == (int)RacingProps.prop_sign_road_05f)
                return Game.GetGXTEntry("FMMC_PR_77");
            if (iParam0 == (int)RacingProps.prop_food_van_01)
                return Game.GetGXTEntry("FMMC_PR_78");
            if (iParam0 == (int)RacingProps.prop_food_van_02)
                return Game.GetGXTEntry("FMMC_PR_79");
            if (iParam0 == (int)RacingProps.prop_tanktrailer_01a)
                return Game.GetGXTEntry("FMMC_PR_80");
            if (iParam0 == (int)RacingProps.prop_truktrailer_01a)
                return Game.GetGXTEntry("FMMC_PR_81");
            if (iParam0 == (int)RacingProps.prop_rub_buswreck_01)
                return Game.GetGXTEntry("FMMC_PR_82");
            if (iParam0 == (int)RacingProps.prop_rub_buswreck_06)
                return Game.GetGXTEntry("FMMC_PR_83");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_11)
                return Game.GetGXTEntry("FMMC_PR_84");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_12)
                return Game.GetGXTEntry("FMMC_PR_85");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_13)
                return Game.GetGXTEntry("FMMC_PR_86");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_9)
                return Game.GetGXTEntry("FMMC_PR_87");
            if (iParam0 == (int)RacingProps.prop_shamal_crash)
                return Game.GetGXTEntry("FMMC_PR_89");
            if (iParam0 == (int)RacingProps.apa_mp_apa_crashed_usaf_01a)
                return Game.GetGXTEntry("FMMC_PR_90");
            if (iParam0 == -1305574636)
                return Game.GetGXTEntry("FMMC_PR_91");
            if (iParam0 == (int)RacingProps.imp_prop_impexp_boxpile_01)
                return Game.GetGXTEntry("FMMC_PR_92");
            if (iParam0 == (int)RacingProps.imp_prop_impexp_boxpile_02)
                return Game.GetGXTEntry("FMMC_PR_93");
            if (iParam0 == (int)RacingProps.sr_prop_sr_boxpile_02)
                return Game.GetGXTEntry("FMMC_PR_111");
            if (iParam0 == (int)RacingProps.sr_prop_sr_boxpile_03)
                return Game.GetGXTEntry("FMMC_PR_112");
            if (iParam0 == (int)RacingProps.sr_prop_sr_track_wall)
                return Game.GetGXTEntry("FMMC_PR_114");
            if (iParam0 == (int)RacingProps.sr_prop_sr_tube_wall)
                return Game.GetGXTEntry("FMMC_PR_115");
            if (iParam0 == (int)RacingProps.ar_prop_ar_ammu_sign)
                return Game.GetGXTEntry("FMMC_PR_116");
            if (iParam0 == (int)RacingProps.sr_mp_spec_races_ammu_sign)
                return Game.GetGXTEntry("FMMC_PR_116");
            if (iParam0 == (int)RacingProps.imp_prop_groupbarrel_01)
                return Game.GetGXTEntry("FMMC_PR_94");
            if (iParam0 == (int)RacingProps.imp_prop_groupbarrel_02)
                return Game.GetGXTEntry("FMMC_PR_95");
            if (iParam0 == (int)RacingProps.imp_prop_groupbarrel_03)
                return Game.GetGXTEntry("FMMC_PR_96");
            if (iParam0 == (int)RacingProps.sr_prop_sr_boxwood_01)
                return Game.GetGXTEntry("FMMC_PR_97");
            if (iParam0 == (int)RacingProps.prop_sec_gate_01d)
                return Game.GetGXTEntry("FMMC_PR_98");
            if (iParam0 == (int)RacingProps.prop_vault_shutter)
                return Game.GetGXTEntry("FMMC_PR_100");
            if (iParam0 == (int)RacingProps.ba_prop_battle_track_exshort)
                return Game.GetGXTEntry("FMMC_BB_DEST");
            if (iParam0 == (int)RacingProps.ba_prop_battle_track_short)
                return Game.GetGXTEntry("FMMC_BB_DST");
            if (iParam0 == (int)RacingProps.prop_fnclink_03gate5)
                return Game.GetGXTEntry("FMMC_PR_FNCMGTS");
            if (iParam0 == (int)RacingProps.prop_fnclink_02gate3)
                return Game.GetGXTEntry("FMMC_PR_FNCMGTD");
            if (iParam0 == (int)RacingProps.prop_plas_barier_01a)
                return Game.GetGXTEntry("FMMC_PR_BARPRED");
            if (iParam0 == (int)RacingProps.prop_barier_conc_02b)
                return Game.GetGXTEntry("FMMC_PR_BARCANW");
            if (iParam0 == (int)RacingProps.prop_barrier_work06a)
                return Game.GetGXTEntry("FMMC_PR_BARWRKP");
            if (iParam0 == (int)RacingProps.prop_barrier_work04a)
                return Game.GetGXTEntry("FMMC_PR_BARWRKW");
            if (iParam0 == (int)RacingProps.prop_fnclink_06a)
                return Game.GetGXTEntry("FMMC_PR_FNCBWSG");
            if (iParam0 == (int)RacingProps.prop_fnclink_06b)
                return Game.GetGXTEntry("FMMC_PR_FNCBWLN");
            if (iParam0 == (int)RacingProps.prop_fnclink_06c)
                return Game.GetGXTEntry("FMMC_PR_FNCBWWD");
            if (iParam0 == (int)RacingProps.prop_fnclink_06d)
                return Game.GetGXTEntry("FMMC_PR_FNCBWWL");
            if (iParam0 == (int)RacingProps.prop_fnccorgm_03a)
                return Game.GetGXTEntry("FMMC_PR_FNCCDCF");
            if (iParam0 == (int)RacingProps.prop_fnccorgm_03b)
                return Game.GetGXTEntry("FMMC_PR_FNCCPDF");
            if (iParam0 == (int)RacingProps.prop_fnccorgm_03c)
                return Game.GetGXTEntry("FMMC_PR_FNCCSCF");
            if (iParam0 == (int)RacingProps.prop_fnccorgm_02a)
                return Game.GetGXTEntry("FMMC_PR_FNCCRDF");
            if (iParam0 == (int)RacingProps.prop_fnccorgm_02b)
                return Game.GetGXTEntry("FMMC_PR_FNCCPRF");
            if (iParam0 == (int)RacingProps.prop_fnccorgm_02c)
                return Game.GetGXTEntry("FMMC_PR_FNCCBPF");
            if (iParam0 == (int)RacingProps.prop_fnccorgm_02d)
                return Game.GetGXTEntry("FMMC_PR_FNCCTCF");
            if (iParam0 == (int)RacingProps.prop_fnccorgm_02e)
                return Game.GetGXTEntry("FMMC_PR_FNCCSRF");
            if (iParam0 == (int)RacingProps.prop_gate_cult_01_l)
                return Game.GetGXTEntry("FMMC_PR_FNCGTAL");
            if (iParam0 == (int)RacingProps.prop_gate_cult_01_r)
                return Game.GetGXTEntry("FMMC_PR_FNCGTAR");
            if (iParam0 == (int)RacingProps.prop_const_fence03b_cr)
                return Game.GetGXTEntry("FMMC_PR_BARQADB");
            if (iParam0 == (int)RacingProps.prop_const_fence02a)
                return Game.GetGXTEntry("FMMC_PR_BARDUBU");
            if (iParam0 == (int)RacingProps.prop_const_fence01b_cr)
                return Game.GetGXTEntry("FMMC_PR_BARSINB");
            if (iParam0 == (int)RacingProps.prop_fncwood_16b)
                return Game.GetGXTEntry("FMMC_PR_FNCPKOD");
            if (iParam0 == (int)RacingProps.prop_fncwood_16c)
                return Game.GetGXTEntry("FMMC_PR_FNCPKOS");
            if (iParam0 == (int)RacingProps.prop_fnc_farm_01b)
                return Game.GetGXTEntry("FMMC_PR_FNCFMS");
            if (iParam0 == (int)RacingProps.prop_fnc_farm_01c)
                return Game.GetGXTEntry("FMMC_PR_FNCFMSL");
            if (iParam0 == (int)RacingProps.prop_fnc_farm_01d)
                return Game.GetGXTEntry("FMMC_PR_FNCFMD");
            if (iParam0 == (int)RacingProps.prop_fnc_farm_01e)
                return Game.GetGXTEntry("FMMC_PR_FNCFMT");
            if (iParam0 == (int)RacingProps.prop_fnc_farm_01f)
                return Game.GetGXTEntry("FMMC_PR_FNCFMSX");
            if (iParam0 == (int)RacingProps.prop_ind_barge_01_cr)
                return Game.GetGXTEntry("FMMC_PR_BARGE");
            if (iParam0 == (int)RacingProps.prop_tollbooth_1)
                return Game.GetGXTEntry("FMMC_PR_CABTBTH");
            if (iParam0 == (int)RacingProps.prop_parking_hut_2)
                return Game.GetGXTEntry("FMMC_PR_CABPHUT");
            if (iParam0 == (int)RacingProps.prop_woodpile_01b)
                return Game.GetGXTEntry("FMMC_PR_WODPLSM");
            if (iParam0 == (int)RacingProps.prop_woodpile_03a)
                return Game.GetGXTEntry("FMMC_PR_WODPLUT");
            if (iParam0 == (int)RacingProps.prop_conc_blocks01c)
                return Game.GetGXTEntry("FMMC_PR_CONCCND");
            if (iParam0 == (int)RacingProps.prop_cons_cements01)
                return Game.GetGXTEntry("FMMC_PR_CONCSAK");
            if (iParam0 == (int)RacingProps.prop_container_01mb)
                return Game.GetGXTEntry("FMMC_PR_CNTLNGG");
            if (iParam0 == (int)RacingProps.prop_container_03mb)
                return Game.GetGXTEntry("FMMC_PR_CNTSHTG");
            if (iParam0 == (int)RacingProps.prop_pallet_pile_02)
                return Game.GetGXTEntry("FMMC_PR_PLTPILL");
            if (iParam0 == (int)RacingProps.prop_pallet_pile_01)
                return Game.GetGXTEntry("FMMC_PR_PLTPILS");
            if (iParam0 == (int)RacingProps.prop_cratepile_07a)
                return Game.GetGXTEntry("FMMC_PR_CRTPILL");
            if (iParam0 == (int)RacingProps.prop_boxpile_04a)
                return Game.GetGXTEntry("FMMC_PR_BOXPILW");
            if (iParam0 == (int)RacingProps.prop_dumpster_02a)
                return Game.GetGXTEntry("FMMC_PR_DMPCLDB");
            if (iParam0 == (int)RacingProps.prop_dumpster_02b)
                return Game.GetGXTEntry("FMMC_PR_DMPCLDM");
            if (iParam0 == (int)RacingProps.prop_elecbox_10_cr)
                return Game.GetGXTEntry("FMMC_PR_ELECBXW");
            if (iParam0 == (int)RacingProps.prop_elecbox_15_cr)
                return Game.GetGXTEntry("FMMC_PR_ELCBXGN");
            if (iParam0 == (int)RacingProps.prop_elecbox_17_cr)
                return Game.GetGXTEntry("FMMC_PR_ELCBXGY");
            if (iParam0 == (int)RacingProps.prop_generator_03b)
                return Game.GetGXTEntry("FMMC_PR_GANDLMP");
            if (iParam0 == (int)RacingProps.prop_feeder1_cr)
                return Game.GetGXTEntry("FMMC_PR_FEEDER");
            if (iParam0 == (int)RacingProps.prop_skip_03)
                return Game.GetGXTEntry("FMMC_PR_RMPDMPM");
            if (iParam0 == (int)RacingProps.prop_byard_rampold_cr)
                return Game.GetGXTEntry("FMMC_PR_RMPOLD");
            if (iParam0 == (int)RacingProps.prop_skate_halfpipe_cr)
                return Game.GetGXTEntry("FMMC_PR_RMPHP");
            if (iParam0 == (int)RacingProps.prop_skate_quartpipe_cr)
                return Game.GetGXTEntry("FMMC_PR_RMPQP");
            if (iParam0 == (int)RacingProps.prop_skate_spiner_cr)
                return Game.GetGXTEntry("FMMC_PR_RMPFRS");
            if (iParam0 == (int)RacingProps.prop_skate_flatramp_cr)
                return Game.GetGXTEntry("FMMC_PR_RMPLFR");
            if (iParam0 == (int)RacingProps.prop_skate_kickers_cr)
                return Game.GetGXTEntry("FMMC_PR_RMPFRK");
            if (iParam0 == (int)RacingProps.prop_skate_funbox_cr)
                return Game.GetGXTEntry("FMMC_PR_RMPFBP");
            if (iParam0 == (int)RacingProps.prop_pile_dirt_07_cr)
                return Game.GetGXTEntry("FMMC_PR_RMPPILE");
            if (iParam0 == (int)RacingProps.prop_old_farm_03)
                return Game.GetGXTEntry("FMMC_PR_FRMOLDT");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_14)
                return Game.GetGXTEntry("FMMC_PR_WRKCRRD");
            if (iParam0 == (int)RacingProps.prop_rub_couch02)
                return Game.GetGXTEntry("FMMC_PR_WRKCCH");
            if (iParam0 == (int)RacingProps.prop_tree_olive_cr2)
                return Game.GetGXTEntry("FMMC_PR_NTROLVT");
            if (iParam0 == (int)RacingProps.prop_tree_eng_oak_cr2)
                return Game.GetGXTEntry("FMMC_PR_NTROAKT");
            if (iParam0 == (int)RacingProps.prop_tree_fallen_02)
                return Game.GetGXTEntry("FMMC_PR_NTRFLNT");
            if (iParam0 == (int)RacingProps.prop_hayb_st_01_cr)
                return Game.GetGXTEntry("FMMC_PR_HBSTK");
            if (iParam0 == (int)RacingProps.prop_haybale_03)
                return Game.GetGXTEntry("FMMC_PR_HBRND");
            if (iParam0 == (int)RacingProps.prop_haybale_02)
                return Game.GetGXTEntry("FMMC_PR_HBSSTK");
            if (iParam0 == (int)RacingProps.prop_haybale_01)
                return Game.GetGXTEntry("FMMC_PR_HBSML");
            if (iParam0 == (int)RacingProps.prop_byard_float_02)
                return Game.GetGXTEntry("FMMC_PR_FLOATD");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_01)
                return Game.GetGXTEntry("FMMC_PR_TYR1");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_02)
                return Game.GetGXTEntry("FMMC_PR_TYR2");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_03)
                return Game.GetGXTEntry("FMMC_PR_TYR3");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_04)
                return Game.GetGXTEntry("FMMC_PR_TYR4");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_05)
                return Game.GetGXTEntry("FMMC_PR_TYR5");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_01b)
                return Game.GetGXTEntry("FMMC_PR_TYR1B");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_02b)
                return Game.GetGXTEntry("FMMC_PR_TYR2B");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_03b)
                return Game.GetGXTEntry("FMMC_PR_TYR3B");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_01c)
                return Game.GetGXTEntry("FMMC_PR_TYR1C");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_02c)
                return Game.GetGXTEntry("FMMC_PR_TYR2C");
            if (iParam0 == (int)RacingProps.prop_tyre_wall_03c)
                return Game.GetGXTEntry("FMMC_PR_TYR3C");
            if (iParam0 == (int)RacingProps.lts_prop_lts_offroad_tyres01)
                return Game.GetGXTEntry("FMMC_PR_ORT1");
            if (iParam0 == (int)RacingProps.prop_offroad_tyres01)
                return Game.GetGXTEntry("FMMC_PR_ORT1");
            if (iParam0 == (int)RacingProps.prop_offroad_tyres02)
                return Game.GetGXTEntry("FMMC_PR_ORT2");
            if (iParam0 == (int)RacingProps.prop_pipes_conc_01)
                return Game.GetGXTEntry("FMMC_PR_CNPPE");
            if (iParam0 == (int)RacingProps.prop_start_gate_01b)
                return Game.GetGXTEntry("FMMC_PR_SGTE");
            if (iParam0 == (int)RacingProps.prop_makeup_trail_01_cr)
                return Game.GetGXTEntry("FMMC_PR_SMTR");
            if (iParam0 == (int)RacingProps.prop_makeup_trail_02_cr)
                return Game.GetGXTEntry("FMMC_PR_MUTR");
            if (iParam0 == (int)RacingProps.prop_bleachers_04_cr)
                return Game.GetGXTEntry("FMMC_PR_SBLE");
            if (iParam0 == (int)RacingProps.prop_bleachers_05_cr)
                return Game.GetGXTEntry("FMMC_PR_BLEA");
            if (iParam0 == (int)RacingProps.prop_beachf_01_cr)
                return Game.GetGXTEntry("FMMC_PR_BEAF");
            if (iParam0 == (int)RacingProps.prop_set_generator_01_cr)
                return Game.GetGXTEntry("FMMC_PR_SEGE");
            if (iParam0 == (int)RacingProps.sm_prop_smug_jammer)
                return Game.GetGXTEntry("FMMC_PR_SMJA");
            if (iParam0 == (int)RacingProps.prop_air_blastfence_01)
                return Game.GetGXTEntry("FMMC_PR_FNCBLST");
            if (iParam0 == (int)RacingProps.prop_mb_sandblock_01)
                return Game.GetGXTEntry("FMMC_PR_SNDBKSI");
            if (iParam0 == (int)RacingProps.prop_mb_sandblock_02)
                return Game.GetGXTEntry("FMMC_PR_SNDBKTR");
            if (iParam0 == (int)RacingProps.prop_mb_sandblock_05_cr)
                return Game.GetGXTEntry("FMMC_PR_SNDBKED");
            if (iParam0 == (int)RacingProps.prop_mb_sandblock_04)
                return Game.GetGXTEntry("FMMC_PR_SNDBKCR");
            if (iParam0 == (int)RacingProps.prop_mb_sandblock_03_cr)
                return Game.GetGXTEntry("FMMC_PR_SNDBKST");
            if (iParam0 == (int)RacingProps.prop_mb_hesco_06)
                return Game.GetGXTEntry("FMMC_PR_SNDBKFT");
            if (iParam0 == (int)RacingProps.prop_const_fence03a_cr)
                return Game.GetGXTEntry("FMMC_PR_BARWDQU");
            if (iParam0 == (int)RacingProps.prop_air_monhut_03_cr)
                return Game.GetGXTEntry("FMMC_PR_CABAPHT");
            if (iParam0 == (int)RacingProps.prop_air_sechut_01)
                return Game.GetGXTEntry("FMMC_PR_CABSCHT");
            if (iParam0 == (int)RacingProps.prop_mb_cargo_03a)
                return Game.GetGXTEntry("FMMC_PR_CRG01");
            if (iParam0 == (int)RacingProps.prop_mb_cargo_04a)
                return Game.GetGXTEntry("FMMC_PR_CRG02");
            if (iParam0 == (int)RacingProps.prop_air_cargo_04a)
                return Game.GetGXTEntry("FMMC_PR_CRG03");
            if (iParam0 == (int)RacingProps.prop_mb_crate_01b)
                return Game.GetGXTEntry("FMMC_PR_CRG04");
            if (iParam0 == (int)RacingProps.prop_air_cargo_01a)
                return Game.GetGXTEntry("FMMC_PR_CRGAIR");
            if (iParam0 == (int)RacingProps.prop_mb_cargo_04b)
                return Game.GetGXTEntry("FMMC_PR_CRG05");
            if (iParam0 == (int)RacingProps.prop_mb_cargo_02a)
                return Game.GetGXTEntry("FMMC_PR_CRG06");
            if (iParam0 == (int)RacingProps.prop_air_taxisign_01a)
                return Game.GetGXTEntry("FMMC_PR_SINAPTX");
            if (iParam0 == (int)RacingProps.prop_air_stair_01)
                return Game.GetGXTEntry("FMMC_PR_STRFLY");
            if (iParam0 == (int)RacingProps.prop_air_stair_04a_cr)
                return Game.GetGXTEntry("FMMC_PR_STRRSD");
            if (iParam0 == (int)RacingProps.prop_air_stair_04b_cr)
                return Game.GetGXTEntry("FMMC_PR_STRLWR");
            if (iParam0 == (int)RacingProps.prop_air_bagloader)
                return Game.GetGXTEntry("FMMC_PR_BAGLDL");
            if (iParam0 == (int)RacingProps.prop_air_bagloader2_cr)
                return Game.GetGXTEntry("FMMC_PR_BAGLDH");
            if (iParam0 == (int)RacingProps.prop_plant_group_04_cr)
                return Game.GetGXTEntry("FMMC_PR_PLNTGP");
            if (iParam0 == (int)RacingProps.prop_bush_lrg_01c_cr)
                return Game.GetGXTEntry("FMMC_PR_BUSHLD");
            if (iParam0 == (int)RacingProps.prop_bush_lrg_01e_cr2)
                return Game.GetGXTEntry("FMMC_PR_BUSHM");
            if (iParam0 == (int)RacingProps.prop_bush_med_03_cr2)
                return Game.GetGXTEntry("FMMC_PR_BUSHS");
            if (iParam0 == (int)RacingProps.prop_joshua_tree_01a)
                return Game.GetGXTEntry("FMMC_PR_JOTREE");
            if (iParam0 == (int)RacingProps.prop_cactus_01e)
                return Game.GetGXTEntry("FMMC_PR_CACTUS");
            if (iParam0 == (int)RacingProps.prop_log_break_01)
                return Game.GetGXTEntry("FMMC_PR_TREFLN");
            if (iParam0 == (int)RacingProps.prop_pot_plant_05c)
                return Game.GetGXTEntry("FMMC_PR_PLNTAP");
            if (iParam0 == (int)RacingProps.prop_pot_plant_04c)
                return Game.GetGXTEntry("FMMC_PR_PLNTPP");
            if (iParam0 == (int)RacingProps.prop_pot_plant_05d)
                return Game.GetGXTEntry("FMMC_PR_PLNTFW");
            if (iParam0 == (int)RacingProps.prop_pot_plant_03b_cr2)
                return Game.GetGXTEntry("FMMC_PR_PLNTCF");
            if (iParam0 == (int)RacingProps.prop_pot_plant_04b)
                return Game.GetGXTEntry("FMMC_PR_PLNTTC");
            if (iParam0 == (int)RacingProps.prop_rock_4_big2)
                return Game.GetGXTEntry("FMMC_PR_RCKBGR");
            if (iParam0 == (int)RacingProps.prop_rock_4_c_2)
                return Game.GetGXTEntry("FMMC_PR_RCKMDF");
            if (iParam0 == (int)RacingProps.prop_rock_4_big)
                return Game.GetGXTEntry("FMMC_PR_RCKBGF");
            if (iParam0 == (int)RacingProps.prop_rock_4_big)
                return Game.GetGXTEntry("FMMC_PR_RCKBGF");
            if (iParam0 == (int)RacingProps.sum_prop_ac_rock_01a)
                return Game.GetGXTEntry("MC_PR_RCK1");
            if (iParam0 == (int)RacingProps.sum_prop_ac_rock_01b)
                return Game.GetGXTEntry("MC_PR_RCK2");
            if (iParam0 == (int)RacingProps.sum_prop_ac_rock_01c)
                return Game.GetGXTEntry("MC_PR_RCK3");
            if (iParam0 == (int)RacingProps.sum_prop_ac_rock_01d)
                return Game.GetGXTEntry("MC_PR_RCK4");
            if (iParam0 == (int)RacingProps.sum_prop_ac_rock_01e)
                return Game.GetGXTEntry("MC_PR_RCK5");
            if (iParam0 == (int)RacingProps.prop_worklight_01a)
                return Game.GetGXTEntry("FMMC_PR_WKLGHT1a");
            if (iParam0 == (int)RacingProps.prop_worklight_02a)
                return Game.GetGXTEntry("FMMC_PR_WKLGHT2a");
            if (iParam0 == (int)RacingProps.prop_worklight_03a)
                return Game.GetGXTEntry("FMMC_PR_WKLGHT3a");
            if (iParam0 == (int)RacingProps.prop_worklight_03b)
                return Game.GetGXTEntry("FMMC_PR_WKLGHT3b");
            if (iParam0 == (int)RacingProps.prop_worklight_04b)
                return Game.GetGXTEntry("FMMC_PR_WKLGHT4b");
            if (iParam0 == (int)RacingProps.prop_worklight_04d)
                return Game.GetGXTEntry("FMMC_PR_WKLGHT4d");
            if (iParam0 == (int)RacingProps.prop_ind_coalcar_02)
                return Game.GetGXTEntry("FMMC_PR_COALCAR");
            if (iParam0 == (int)RacingProps.prop_crashed_heli)
                return Game.GetGXTEntry("FMMC_PR_CRSHHELI");
            if (iParam0 == (int)RacingProps.prop_water_ramp_01)
                return Game.GetGXTEntry("FMMC_PR_WTRRAMP1");
            if (iParam0 == (int)RacingProps.prop_water_ramp_02)
                return Game.GetGXTEntry("FMMC_PR_WTRRAMP2");
            if (iParam0 == (int)RacingProps.prop_water_ramp_03)
                return Game.GetGXTEntry("FMMC_PR_WTRRAMP3");
            if (iParam0 == (int)RacingProps.vw_prop_vw_barrel_01a)
                return Game.GetGXTEntry("FMMC_PR_WBARR1");
            if (iParam0 == (int)RacingProps.vw_prop_vw_barrel_pile_01a)
                return Game.GetGXTEntry("FMMC_PR_WBARR2");
            if (iParam0 == (int)RacingProps.vw_prop_vw_barrel_pile_02a)
                return Game.GetGXTEntry("FMMC_PR_WBARR3");
            if (iParam0 == (int)RacingProps.vw_prop_vw_barrier_rope_01a)
                return Game.GetGXTEntry("FMMC_PR_ROPEB1");
            if (iParam0 == (int)RacingProps.vw_prop_vw_barrier_rope_02a)
                return Game.GetGXTEntry("FMMC_PR_ROPEB2");
            if (iParam0 == (int)RacingProps.prop_offroad_barrel01)
                return Game.GetGXTEntry("FMMC_DPR_BARREL");
            if (iParam0 == (int)RacingProps.prop_offroad_barrel02)
                return Game.GetGXTEntry("FMMC_DPR_BRLLNE");
            if (iParam0 == (int)RacingProps.prop_barrel_exp_01a)
                return Game.GetGXTEntry("FMMC_DPR_EXPBRL");
            if (iParam0 == (int)RacingProps.prop_fire_exting_1b)
                return Game.GetGXTEntry("FMMC_DPR_FIREXT");
            if (iParam0 == (int)RacingProps.prop_roadcone02c)
                return Game.GetGXTEntry("FMMC_DPR_CONE");
            if (iParam0 == (int)RacingProps.prop_roadcone02a)
                return Game.GetGXTEntry("FMMC_DPR_TRFCNE");
            if (iParam0 == (int)RacingProps.prop_roadcone01a)
                return Game.GetGXTEntry("FMMC_DPR_LTRFCN");
            if (iParam0 == (int)RacingProps.prop_roadpole_01a)
                return Game.GetGXTEntry("FMMC_DPR_TRFPLE");
            if (iParam0 == (int)RacingProps.prop_postbox_01a)
                return Game.GetGXTEntry("FMMC_DPR_MALBOX");
            if (iParam0 == (int)RacingProps.prop_news_disp_02d)
                return Game.GetGXTEntry("FMMC_DPR_NPVND");
            if (iParam0 == (int)RacingProps.prop_vend_water_01)
                return Game.GetGXTEntry("FMMC_DPR_WVND");
            if (iParam0 == (int)RacingProps.prop_vend_snak_01_tu)
                return Game.GetGXTEntry("FMMC_DPR_MCHSNK");
            if (iParam0 == (int)RacingProps.prop_train_ticket_02_tu)
                return Game.GetGXTEntry("FMMC_DPR_MCHTCK");
            if (iParam0 == (int)RacingProps.prop_boxpile_02b)
                return Game.GetGXTEntry("FMMC_DPR_BOXPIL");
            if (iParam0 == (int)RacingProps.prop_mc_conc_barrier_01)
                return Game.GetGXTEntry("FMMC_DPR_DESBAR");
            if (iParam0 == (int)RacingProps.prop_fncsec_03b)
                return Game.GetGXTEntry("FMMC_DPR_SECFEN");
            if (iParam0 == (int)RacingProps.prop_table_08_side)
                return Game.GetGXTEntry("FMMC_DPR_UPTTBL");
            if (iParam0 == (int)RacingProps.prop_container_ld_pu)
                return Game.GetGXTEntry("FMMC_DPR_AMOCRT");
            if (iParam0 == (int)RacingProps.prop_mb_ordnance_02)
                return Game.GetGXTEntry("FMMC_DPR_ORDNAN");
            if (iParam0 == (int)RacingProps.prop_storagetank_02b)
                return Game.GetGXTEntry("FMMC_DPR_TKRDEX");
            if (iParam0 == (int)RacingProps.prop_logpile_01)
                return Game.GetGXTEntry("FMMC_DPR_LGPLEL");
            if (iParam0 == (int)RacingProps.prop_logpile_03)
                return Game.GetGXTEntry("FMMC_DPR_LGPLES");
            if (iParam0 == (int)RacingProps.prop_pipes_02b)
                return Game.GetGXTEntry("FMMC_DPR_PIPPLE");
            if (iParam0 == (int)RacingProps.prop_barrel_pile_01)
                return Game.GetGXTEntry("FMMC_DPR_BRLPLE");
            if (iParam0 == (int)RacingProps.prop_barrel_exp_01b)
                return Game.GetGXTEntry("FMMC_DPR_BRGOEX");
            if (iParam0 == (int)RacingProps.prop_barrel_exp_01c)
                return Game.GetGXTEntry("FMMC_DPR_BRFLEX");
            if (iParam0 == (int)RacingProps.prop_gas_tank_02a)
                return Game.GetGXTEntry("FMMC_DPR_TKLRGW");
            if (iParam0 == (int)RacingProps.prop_gas_tank_04a)
                return Game.GetGXTEntry("FMMC_DPR_TKLRGG");
            if (iParam0 == (int)RacingProps.prop_gas_tank_02b)
                return Game.GetGXTEntry("FMMC_DPR_TKLRGY");
            if (iParam0 == (int)RacingProps.prop_jerrycan_01a)
                return Game.GetGXTEntry("FMMC_DPR_JRYCAN");
            if (iParam0 == (int)RacingProps.prop_gascyl_01a)
                return Game.GetGXTEntry("FMMC_DPR_SGC");
            if (iParam0 == (int)RacingProps.prop_gascyl_04a)
                return Game.GetGXTEntry("FMMC_DPR_LGC");
            if (iParam0 == (int)RacingProps.prop_gascyl_03a)
                return Game.GetGXTEntry("FMMC_DPR_TBGAS");
            if (iParam0 == (int)RacingProps.prop_gascyl_03b)
                return Game.GetGXTEntry("FMMC_DPR_TRGAS");
            if (iParam0 == (int)RacingProps.prop_gascyl_02a)
                return Game.GetGXTEntry("FMMC_DPR_OLGAS");
            if (iParam0 == (int)RacingProps.prop_gascyl_02b)
                return Game.GetGXTEntry("FMMC_DPR_OSGAS");
            if (iParam0 == (int)RacingProps.prop_fruitstand_b)
                return Game.GetGXTEntry("FMMC_DPR_FSTND");
            if (iParam0 == (int)RacingProps.prop_rub_tyre_03)
                return Game.GetGXTEntry("FMMC_DPR_RBRTYR");
            if (iParam0 == (int)RacingProps.prop_barrel_02b)
                return Game.GetGXTEntry("FMMC_DPR_BAR2");
            if (iParam0 == (int)RacingProps.prop_barrel_01a)
                return Game.GetGXTEntry("FMMC_DPR_BAR3");
            if (iParam0 == (int)RacingProps.imp_prop_impexp_boxpile_01)
                return Game.GetGXTEntry("FMMC_DPR_BP3");
            if (iParam0 == (int)RacingProps.sr_prop_sr_boxpile_02)
                return Game.GetGXTEntry("FMMC_DPR_CP1");
            if (iParam0 == (int)RacingProps.sr_prop_sr_boxpile_03)
                return Game.GetGXTEntry("FMMC_DPR_CP2");
            if (iParam0 == (int)RacingProps.xm_prop_rsply_crate04a)
                return Game.GetGXTEntry("FMMC_PR_GCRTC");
            if (iParam0 == (int)RacingProps.prop_mb_crate_01a)
                return Game.GetGXTEntry("FMMC_PR_GCRTL");
            if (iParam0 == (int)RacingProps.prop_pallettruck_01)
                return Game.GetGXTEntry("FMMC_PR_PLTTRK");
            if (iParam0 == (int)RacingProps.prop_rub_cardpile_04)
                return Game.GetGXTEntry("FMMC_PR_CCP1");
            if (iParam0 == (int)RacingProps.prop_rub_cardpile_06)
                return Game.GetGXTEntry("FMMC_PR_CCP2");
            if (iParam0 == (int)RacingProps.prop_skid_box_01)
                return Game.GetGXTEntry("FMMC_PR_CSB");
            if (iParam0 == (int)RacingProps.prop_cons_plyboard_01)
                return Game.GetGXTEntry("FMMC_PR_CPB");
            if (iParam0 == (int)RacingProps.prop_cons_plank)
                return Game.GetGXTEntry("FMMC_PR_CP");
            if (iParam0 == (int)RacingProps.prop_barrier_work01c)
                return Game.GetGXTEntry("FMMC_PR_CWB");
            if (iParam0 == (int)RacingProps.prop_food_cb_tray_01)
                return Game.GetGXTEntry("FMMC_PR_CFT1");
            if (iParam0 == (int)RacingProps.prop_food_bs_tray_06)
                return Game.GetGXTEntry("FMMC_PR_CFT2");
            if (iParam0 == (int)RacingProps.prop_cs_envolope_01)
                return Game.GetGXTEntry("FMMC_PR_CE");
            if (iParam0 == (int)RacingProps.prop_cs_binder_01)
                return Game.GetGXTEntry("FMMC_PR_CB");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bowling_pin)
                return Game.GetGXTEntry("MC_PR_STNT63");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bowling_ball)
                return Game.GetGXTEntry("MC_PR_STNT261");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_soccer_lball)
                return Game.GetGXTEntry("MC_PR_STNT65");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_soccer_sball)
                return Game.GetGXTEntry("MC_PR_STNT66");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_soccer_ball)
                return Game.GetGXTEntry("MC_PR_STNT68");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bomb_s)
                return Game.GetGXTEntry("ARNAP_BMB_S");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bomb_m)
                return Game.GetGXTEntry("ARNAP_BMB_M");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bomb_l)
                return Game.GetGXTEntry("ARNAP_BMB_L");
            if (iParam0 == (int)RacingProps.prop_ld_alarm_01)
                return Game.GetGXTEntry("FMMC_PR_ALARM");
            if (iParam0 == (int)RacingProps.prop_flare_01)
                return Game.GetGXTEntry("FMMC_PR_FLARE");
            if (iParam0 == (int)RacingProps.ind_prop_firework_03)
                return Game.GetGXTEntry("FMMC_PR_FIREW");
            if (iParam0 == (int)RacingProps.prop_barrier_work05)
                return Game.GetGXTEntry("FMMC_PR_PBARR");
            if (iParam0 == (int)RacingProps.ind_prop_dlc_flag_02)
                return Game.GetGXTEntry("FMMC_PR_AMFLG");
            if (iParam0 == (int)RacingProps.sm_prop_smug_cover_01a)
                return Game.GetGXTEntry("FMMC_PR_RACOV");
            if (iParam0 == (int)RacingProps.xm_prop_x17_cover_01)
                return Game.GetGXTEntry("FMMC_PR_RACOVS");
            if (iParam0 == (int)RacingProps.xm_prop_x17_bunker_door)
                return Game.GetGXTEntry("FMMC_PR_IAADR");
            if (iParam0 == (int)RacingProps.prop_sm1_11_garaged)
                return Game.GetGXTEntry("FMMC_PR_GRGDR");
            if (iParam0 == (int)RacingProps.ba_prop_battle_ps_box_01)
                return Game.GetGXTEntry("FMMC_PR_PONSON");
            if (iParam0 == (int)RacingProps.hei_prop_bank_plug)
                return Game.GetGXTEntry("FMMC_PR_BPLUG");
            if (iParam0 == (int)RacingProps.hei_prop_wall_alarm_on)
                return Game.GetGXTEntry("FMMC_PR_AMLG1");
            if (iParam0 == (int)RacingProps.hei_prop_wall_alarm_off)
                return Game.GetGXTEntry("FMMC_PR_AMLG2");
            if (iParam0 == (int)RacingProps.hei_prop_hei_cash_trolly_03)
                return Game.GetGXTEntry("FMMC_PR_CASHT");
            if (iParam0 == (int)RacingProps.hei_prop_carrier_docklight_01)
                return Game.GetGXTEntry("FMMC_PR_CARL2");
            if (iParam0 == (int)RacingProps.hei_prop_carrier_docklight_02)
                return Game.GetGXTEntry("FMMC_PR_CARL5");
            if (iParam0 == (int)RacingProps.hei_prop_wall_light_10a_cr)
                return Game.GetGXTEntry("FMMC_PR_WALLL");
            if (iParam0 == (int)RacingProps.hei_prop_heist_apecrate)
                return Game.GetGXTEntry("FMMC_PR_MNKCR");
            if (iParam0 == (int)RacingProps.hei_prop_cc_metalcover_01)
                return Game.GetGXTEntry("FMMC_PR_METCV");
            if (iParam0 == (int)RacingProps.hei_prop_bank_alarm_01)
                return Game.GetGXTEntry("FMMC_PR_BNKAL");
            if (iParam0 == (int)RacingProps.prop_road_memorial_02)
                return Game.GetGXTEntry("FMMC_PR_RDMEM");
            if (iParam0 == (int)RacingProps.prop_boombox_01)
                return Game.GetGXTEntry("FMMC_PR_BBRE");
            if (iParam0 == (int)RacingProps.prop_ghettoblast_02)
                return Game.GetGXTEntry("FMMC_PR_GHBL");
            if (iParam0 == (int)RacingProps.prop_tapeplayer_01)
                return Game.GetGXTEntry("FMMC_PR_TAPL");
            if (iParam0 == (int)RacingProps.prop_radio_01)
                return Game.GetGXTEntry("FMMC_PR_RADI");
            if (iParam0 == (int)RacingProps.ind_prop_firework_01)
                return Game.GetGXTEntry("FMMC_PR_FRWK");
            if (iParam0 == (int)RacingProps.prop_cs_heist_bag_01)
                return Game.GetGXTEntry("FMMC_PR_HB01");
            if (iParam0 == (int)RacingProps.prop_bikerset)
                return Game.GetGXTEntry("FMMC_PR_BISET");
            if (iParam0 == (int)RacingProps.prop_champset)
                return Game.GetGXTEntry("FMMC_PR_CHSET");
            if (iParam0 == (int)RacingProps.xm_prop_body_bag)
                return Game.GetGXTEntry("FMMC_OM_54");
            if (iParam0 == (int)RacingProps.xm_prop_x17_corpse_01)
                return Game.GetGXTEntry("FMMC_OM_56");
            if (iParam0 == (int)RacingProps.xm_prop_x17_corpse_02)
                return Game.GetGXTEntry("FMMC_OM_57");
            if (iParam0 == (int)RacingProps.xm_prop_x17_corpse_03)
                return Game.GetGXTEntry("FMMC_OM_58");
            if (iParam0 == (int)RacingProps.xm_prop_x17_shovel_01b)
                return Game.GetGXTEntry("FMMC_OM_59");
            if (iParam0 == (int)RacingProps.xm_prop_x17_shovel_01a)
                return Game.GetGXTEntry("FMMC_OM_60");
            if (iParam0 == (int)RacingProps.xm_prop_x17_note_paper_01a)
                return Game.GetGXTEntry("FMMC_OM_61");
            if (iParam0 == (int)RacingProps.xm_prop_x17_chest_closed)
                return Game.GetGXTEntry("FMMC_OM_62");
            if (iParam0 == (int)RacingProps.xm_prop_x17_chest_open)
                return Game.GetGXTEntry("FMMC_OM_63");
            if (iParam0 == (int)RacingProps.xm_prop_gr_console_01)
                return Game.GetGXTEntry("FMMC_OM_H2CON");
            if (iParam0 == (int)RacingProps.prop_cabinet_01b)
                return Game.GetGXTEntry("FMMC_OM_64");
            if (iParam0 == (int)RacingProps.xm_prop_base_jet_01)
                return Game.GetGXTEntry("FMMC_OM_JET1");
            if (iParam0 == (int)RacingProps.xm_prop_base_jet_02)
                return Game.GetGXTEntry("FMMC_OM_JET2");
            if (iParam0 == (int)RacingProps.xm_prop_x17_desk_cover_01a)
                return Game.GetGXTEntry("FMMC_OM_42");
            if (iParam0 == (int)RacingProps.xm_prop_x17_tv_stand_01a)
                return Game.GetGXTEntry("FMMC_OM_47");
            if (iParam0 == (int)RacingProps.xm_prop_x17_tool_draw_01a)
                return Game.GetGXTEntry("FMMC_OM_46");
            if (iParam0 == (int)RacingProps.xm_prop_x17_filecab_01a)
                return Game.GetGXTEntry("FMMC_OM_45");
            if (iParam0 == (int)RacingProps.xm_prop_x17_labvats)
                return Game.GetGXTEntry("FMMC_OM_44");
            if (iParam0 == (int)RacingProps.xm_prop_x17_seat_cover_01a)
                return Game.GetGXTEntry("FMMC_OM_41");
            if (iParam0 == (int)RacingProps.ba_prop_battle_mast_01a)
                return Game.GetGXTEntry("FMMC_PR_ARMS");
            if (iParam0 == (int)RacingProps.imp_prop_impexp_bblock_lrg1)
                return Game.GetGXTEntry("FMMC_PR_LHB");
            if (iParam0 == (int)RacingProps.imp_prop_impexp_bblock_mdm1)
                return Game.GetGXTEntry("FMMC_PR_MHB");
            if (iParam0 == (int)RacingProps.imp_prop_impexp_bblock_sml1)
                return Game.GetGXTEntry("FMMC_PR_SHB");
            if (iParam0 == (int)RacingProps.imp_prop_impexp_bblock_xl1)
                return Game.GetGXTEntry("FMMC_PR_XLHB");
            if (iParam0 == (int)RacingProps.sr_prop_spec_target_s_01a)
                return Game.GetGXTEntry("FMMC_PR_BJT1");
            if (iParam0 == (int)RacingProps.sr_prop_spec_target_m_01a)
                return Game.GetGXTEntry("FMMC_PR_BJT2");
            if (iParam0 == (int)RacingProps.sr_prop_spec_target_b_01a)
                return Game.GetGXTEntry("FMMC_PR_BJT3");
            if (iParam0 == (int)RacingProps.w_ar_railgun)
                return Game.GetGXTEntry("FMMC_STR_WP_52");
            if (iParam0 == (int)RacingProps.xm_prop_x17_bag_01a)
                return Game.GetGXTEntry("FMMC_OM_48");
            if (iParam0 == (int)RacingProps.xm_prop_x17_bag_med_01a)
                return Game.GetGXTEntry("FMMC_OM_49");
            if (iParam0 == (int)RacingProps.xm_prop_x17_barge_01)
                return Game.GetGXTEntry("FMMC_OM_50");
            if (iParam0 == (int)RacingProps.xm_prop_x17_trail_01a)
                return Game.GetGXTEntry("FMMC_OM_51");
            if (iParam0 == (int)RacingProps.xm_prop_x17_trail_02a)
                return Game.GetGXTEntry("FMMC_OM_52");
            if (iParam0 == (int)RacingProps.xm_prop_x17_sub)
                return Game.GetGXTEntry("FMMC_OM_SUBMAR");
            if (iParam0 == (int)RacingProps.xm_prop_base_cabinet_door_01)
                return Game.GetGXTEntry("FMMC_OM_53");
            if (iParam0 == (int)RacingProps.xm_prop_x17_shamal_crash)
                return Game.GetGXTEntry("FMMC_PR_117");
            if (iParam0 == (int)RacingProps.v_corp_filecablow)
                return Game.GetGXTEntry("FMMC_PR_118");
            if (iParam0 == (int)RacingProps.w_pi_pistol)
                return Game.GetGXTEntry("FMMC_PR_PST");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_adj_flip_m)
                return Game.GetGXTEntry("MC_PR_STNT173");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_adj_flip_mb)
                return Game.GetGXTEntry("MC_PR_STNT32");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_adj_flip_s)
                return Game.GetGXTEntry("MC_PR_STNT193");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_adj_flip_sb)
                return Game.GetGXTEntry("MC_PR_STNT31");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_adj_hloop)
                return Game.GetGXTEntry("MC_PR_STNT19");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_adj_loop)
                return Game.GetGXTEntry("MC_PR_STNT20");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_jump_xs)
                return Game.GetGXTEntry("MC_PR_STNT25");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_jump_s)
                return Game.GetGXTEntry("MC_PR_STNT26");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_jump_m)
                return Game.GetGXTEntry("MC_PR_STNT27");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_jump_l)
                return Game.GetGXTEntry("MC_PR_STNT28");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_jump_xl)
                return Game.GetGXTEntry("MC_PR_STNT29");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_jump_xxl)
                return Game.GetGXTEntry("MC_PR_STNT30");
            if (iParam0 == (int)RacingProps.stt_prop_track_jump_01a)
                return Game.GetGXTEntry("MC_PR_STNT156");
            if (iParam0 == (int)RacingProps.stt_prop_track_jump_01b)
                return Game.GetGXTEntry("MC_PR_STNT157");
            if (iParam0 == (int)RacingProps.stt_prop_track_jump_01c)
                return Game.GetGXTEntry("MC_PR_STNT158");
            if (iParam0 == (int)RacingProps.stt_prop_track_jump_02a)
                return Game.GetGXTEntry("MC_PR_STNT159");
            if (iParam0 == (int)RacingProps.stt_prop_track_jump_02b)
                return Game.GetGXTEntry("MC_PR_STNT160");
            if (iParam0 == (int)RacingProps.stt_prop_track_jump_02c)
                return Game.GetGXTEntry("MC_PR_STNT161");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_01a)
                return Game.GetGXTEntry("MC_PR_STNT156");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_01b)
                return Game.GetGXTEntry("MC_PR_STNT157");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_01c)
                return Game.GetGXTEntry("MC_PR_STNT158");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_02a)
                return Game.GetGXTEntry("MC_PR_STNT159");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_02b)
                return Game.GetGXTEntry("MC_PR_STNT160");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_02c)
                return Game.GetGXTEntry("MC_PR_STNT161");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_multi_loop_rb)
                return Game.GetGXTEntry("MC_PR_STNT21");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump_loop)
                return Game.GetGXTEntry("MC_PR_STNT319");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_spiral_s)
                return Game.GetGXTEntry("MC_PR_STNT24");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_spiral_l_s)
                return Game.GetGXTEntry("MC_PR_STNT150");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_spiral_m)
                return Game.GetGXTEntry("MC_PR_STNT23");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_spiral_l_m)
                return Game.GetGXTEntry("MC_PR_STNT151");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_spiral_l)
                return Game.GetGXTEntry("MC_PR_STNT22");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_spiral_l_l)
                return Game.GetGXTEntry("MC_PR_STNT152");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_spiral_xxl)
                return Game.GetGXTEntry("MC_PR_STNT155");
            if (iParam0 == (int)RacingProps.stt_prop_ramp_spiral_l_xxl)
                return Game.GetGXTEntry("MC_PR_STNT153");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bowlpin_stand)
                return Game.GetGXTEntry("MC_PR_STNT64");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump_s)
                return Game.GetGXTEntry("MC_PR_STNT88");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump_m)
                return Game.GetGXTEntry("MC_PR_STNT89");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump_l)
                return Game.GetGXTEntry("MC_PR_STNT90");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_s)
                return Game.GetGXTEntry("MC_PR_STNT88");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_m)
                return Game.GetGXTEntry("MC_PR_STNT89");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_l)
                return Game.GetGXTEntry("MC_PR_STNT90");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump_sb)
                return Game.GetGXTEntry("MC_PR_STNT110");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump_mb)
                return Game.GetGXTEntry("MC_PR_STNT111");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump_lb)
                return Game.GetGXTEntry("MC_PR_STNT112");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_sb)
                return Game.GetGXTEntry("MC_PR_STNT110");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_mb)
                return Game.GetGXTEntry("MC_PR_STNT111");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_jump_lb)
                return Game.GetGXTEntry("MC_PR_STNT112");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_ramp)
                return Game.GetGXTEntry("MC_PR_STNT33");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_wideramp)
                return Game.GetGXTEntry("MC_PR_STNT135");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_qp3)
                return Game.GetGXTEntry("MC_PR_STNT162");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_qp2)
                return Game.GetGXTEntry("MC_PR_STNT163");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_qp)
                return Game.GetGXTEntry("MC_PR_STNT164");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_hump_01)
                return Game.GetGXTEntry("MC_PR_STNT163s");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_hump_02)
                return Game.GetGXTEntry("MC_PR_STNT163m");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_cor)
                return Game.GetGXTEntry("MC_PR_BKRQC");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_cor_02)
                return Game.GetGXTEntry("MC_PR_BKRQCM");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_cor_03)
                return Game.GetGXTEntry("MC_PR_BKRQCL");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_soccer_goal)
                return Game.GetGXTEntry("MC_PR_STNT67");
            if (iParam0 == (int)RacingProps.stt_prop_race_start_line_01)
                return Game.GetGXTEntry("MC_PR_STNT190");
            if (iParam0 == (int)RacingProps.stt_prop_race_start_line_01b)
                return Game.GetGXTEntry("MC_PR_STNT190b");
            if (iParam0 == (int)RacingProps.sr_prop_sr_start_line_02)
                return Game.GetGXTEntry("MC_PR_STNT191");
            if (iParam0 == (int)RacingProps.stt_prop_race_start_line_02b)
                return Game.GetGXTEntry("MC_PR_STNT243");
            if (iParam0 == (int)RacingProps.stt_prop_race_start_line_03)
                return Game.GetGXTEntry("MC_PR_STNT192");
            if (iParam0 == (int)RacingProps.stt_prop_race_start_line_03b)
                return Game.GetGXTEntry("MC_PR_STNT244");
            if (iParam0 == (int)RacingProps.ch_prop_track_pit_stop_01)
                return Game.GetGXTEntry("MC_PR_STNT325");
            if (iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_30r)
                return Game.GetGXTEntry("MC_PR_STNT325");
            if (iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_16l)
                return Game.GetGXTEntry("MC_PR_SLPL2");
            if (iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_16r)
                return Game.GetGXTEntry("MC_PR_SLPR");
            if (iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_30l)
                return Game.GetGXTEntry("MC_PR_SLPL");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_target_small)
                return Game.GetGXTEntry("MC_PR_STNT320");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_target)
                return Game.GetGXTEntry("MC_PR_STNT34");
            if (iParam0 == (int)RacingProps.as_prop_as_stunt_target)
                return Game.GetGXTEntry("MC_PR_ASC01");
            if (iParam0 == (int)RacingProps.as_prop_as_stunt_target_small)
                return Game.GetGXTEntry("MC_PR_ASC02");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_landing_zone_01)
                return Game.GetGXTEntry("MC_PR_STNT171");
            if (iParam0 == (int)RacingProps.stt_prop_hoop_tyre_01a)
                return Game.GetGXTEntry("MC_PR_STNT194");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn)
                return Game.GetGXTEntry("MC_PR_STNT46");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn2)
                return Game.GetGXTEntry("MC_PR_STNT102");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn_5d)
                return Game.GetGXTEntry("MC_PR_STNT226");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn_15d)
                return Game.GetGXTEntry("MC_PR_STNT227");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn_30d)
                return Game.GetGXTEntry("MC_PR_STNT228");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_fork)
                return Game.GetGXTEntry("MC_PR_STNT134");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_gap_01)
                return Game.GetGXTEntry("MC_PR_STNT165");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_gap_02)
                return Game.GetGXTEntry("MC_PR_STNT166");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_gap_03)
                return Game.GetGXTEntry("MC_PR_STNT262");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_cross)
                return Game.GetGXTEntry("MC_PR_STNT40");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_end)
                return Game.GetGXTEntry("MC_PR_STNT47");
            if (iParam0 == (int)RacingProps.sr_prop_sr_tube_end)
                return Game.GetGXTEntry("MC_PR_STNT47b");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_speed)
                return Game.GetGXTEntry("MC_PR_STNT248");
            if (iParam0 == (int)RacingProps.stt_prop_track_tube_02)
                return Game.GetGXTEntry("MC_PR_STNT170");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_qg)
                return Game.GetGXTEntry("MC_PR_STNT41");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_hg)
                return Game.GetGXTEntry("MC_PR_STNT42");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_xxs)
                return Game.GetGXTEntry("MC_PR_STNT104");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_xs)
                return Game.GetGXTEntry("MC_PR_STNT37");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_s)
                return Game.GetGXTEntry("MC_PR_STNT38");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_m)
                return Game.GetGXTEntry("MC_PR_STNT39");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_l)
                return Game.GetGXTEntry("MC_PR_STNT100");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_jmp)
                return Game.GetGXTEntry("MC_PR_STNT44");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_jmp2)
                return Game.GetGXTEntry("MC_PR_STNT82");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_ent)
                return Game.GetGXTEntry("MC_PR_STNT93");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_01)
                return Game.GetGXTEntry("MC_PR_STNT83");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_02)
                return Game.GetGXTEntry("MC_PR_STNT84");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_03)
                return Game.GetGXTEntry("MC_PR_STNT85");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_04)
                return Game.GetGXTEntry("MC_PR_STNT86");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_05)
                return Game.GetGXTEntry("MC_PR_STNT87");
            if (iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_01)
                return Game.GetGXTEntry("MC_PR_STNT83");
            if (iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_02)
                return Game.GetGXTEntry("MC_PR_STNT84");
            if (iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_03)
                return Game.GetGXTEntry("MC_PR_STNT85");
            if (iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_04)
                return Game.GetGXTEntry("MC_PR_STNT86");
            if (iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_05)
                return Game.GetGXTEntry("MC_PR_STNT87");
            if (iParam0 == (int)RacingProps.as_prop_as_tube_gap_03)
                return Game.GetGXTEntry("MC_PR_ASC03");
            if (iParam0 == (int)RacingProps.as_prop_as_tube_xxs)
                return Game.GetGXTEntry("MC_PR_ASC04");
            if (iParam0 == (int)RacingProps.stt_prop_track_stop_sign)
                return Game.GetGXTEntry("MC_PR_STNT101");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_01)
                return Game.GetGXTEntry("MC_PR_STNT174");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_02)
                return Game.GetGXTEntry("MC_PR_STNT175");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_03)
                return Game.GetGXTEntry("MC_PR_STNT176");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_04)
                return Game.GetGXTEntry("MC_PR_STNT177");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_05)
                return Game.GetGXTEntry("MC_PR_STNT178");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_06)
                return Game.GetGXTEntry("MC_PR_STNT179");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_07)
                return Game.GetGXTEntry("MC_PR_STNT180");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_08)
                return Game.GetGXTEntry("MC_PR_STNT181");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_09)
                return Game.GetGXTEntry("MC_PR_STNT182");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_10)
                return Game.GetGXTEntry("MC_PR_STNT183");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_11)
                return Game.GetGXTEntry("MC_PR_STNT184");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_12)
                return Game.GetGXTEntry("MC_PR_STNT185");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_13)
                return Game.GetGXTEntry("MC_PR_STNT186");
            if (iParam0 == (int)RacingProps.stt_prop_corner_sign_14)
                return Game.GetGXTEntry("MC_PR_STNT187");
            if (iParam0 == (int)RacingProps.ch_prop_pit_sign_01a)
                return Game.GetGXTEntry("FMMC_PIT_SIGN");
            if (iParam0 == (int)RacingProps.sum_prop_ac_pit_sign_left)
                return Game.GetGXTEntry("MC_PR_S_PTL");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_01)
                return Game.GetGXTEntry("MC_PR_STNT198");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_02)
                return Game.GetGXTEntry("MC_PR_STNT199");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_03)
                return Game.GetGXTEntry("MC_PR_STNT200");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_04)
                return Game.GetGXTEntry("MC_PR_STNT201");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_05)
                return Game.GetGXTEntry("MC_PR_STNT202");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_06)
                return Game.GetGXTEntry("MC_PR_STNT203");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_07)
                return Game.GetGXTEntry("MC_PR_STNT204");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_08)
                return Game.GetGXTEntry("MC_PR_STNT205");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_09)
                return Game.GetGXTEntry("MC_PR_STNT206");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_10)
                return Game.GetGXTEntry("MC_PR_STNT207");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_11)
                return Game.GetGXTEntry("MC_PR_STNT208");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_12)
                return Game.GetGXTEntry("MC_PR_STNT209");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_13)
                return Game.GetGXTEntry("MC_PR_STNT210");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_14)
                return Game.GetGXTEntry("MC_PR_STNT211");
            if (iParam0 == (int)RacingProps.stt_prop_sign_circuit_15)
                return Game.GetGXTEntry("MC_PR_STNT212");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_bar_m)
                return Game.GetGXTEntry("MC_PR_STNT52");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_5d_bar)
                return Game.GetGXTEntry("MC_PR_STNT264");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_15d_bar)
                return Game.GetGXTEntry("MC_PR_STNT265");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_30d_bar)
                return Game.GetGXTEntry("MC_PR_STNT266");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_180d_bar)
                return Game.GetGXTEntry("MC_PR_STNT267");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_m)
                return Game.GetGXTEntry("MC_PR_STNT48");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_5d)
                return Game.GetGXTEntry("MC_PR_STNT222");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_15d)
                return Game.GetGXTEntry("MC_PR_STNT223");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_30d)
                return Game.GetGXTEntry("MC_PR_STNT224");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_180d)
                return Game.GetGXTEntry("MC_PR_STNT225");
            if (iParam0 == (int)RacingProps.sum_prop_track_ac_bend_180d)
                return Game.GetGXTEntry("MC_PR_CRN_180h");
            if (iParam0 == (int)RacingProps.stt_prop_track_fork)
                return Game.GetGXTEntry("MC_PR_STNT137");
            if (iParam0 == (int)RacingProps.stt_prop_track_cross)
                return Game.GetGXTEntry("MC_PR_STNT56");
            if (iParam0 == (int)RacingProps.stt_prop_track_fork_bar)
                return Game.GetGXTEntry("MC_PR_STNT138");
            if (iParam0 == (int)RacingProps.stt_prop_track_cross_bar)
                return Game.GetGXTEntry("MC_PR_STNT57");
            if (iParam0 == (int)RacingProps.stt_prop_track_chicane_l)
                return Game.GetGXTEntry("MC_PR_STNT167");
            if (iParam0 == (int)RacingProps.stt_prop_track_tube_01)
                return Game.GetGXTEntry("MC_PR_STNT169");
            if (iParam0 == (int)RacingProps.stt_prop_track_chicane_r)
                return Game.GetGXTEntry("MC_PR_STNT168");
            if (iParam0 == (int)RacingProps.stt_prop_track_chicane_l_02)
                return Game.GetGXTEntry("MC_PR_STNT196");
            if (iParam0 == (int)RacingProps.stt_prop_track_chicane_r_02)
                return Game.GetGXTEntry("MC_PR_STNT197");
            if (iParam0 == (int)RacingProps.stt_prop_track_block_01)
                return Game.GetGXTEntry("MC_PR_STNT236");
            if (iParam0 == (int)RacingProps.stt_prop_track_block_02)
                return Game.GetGXTEntry("MC_PR_STNT237");
            if (iParam0 == (int)RacingProps.stt_prop_track_block_03)
                return Game.GetGXTEntry("MC_PR_STNT238");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_l)
                return Game.GetGXTEntry("MC_PR_STNT239");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend2_l)
                return Game.GetGXTEntry("MC_PR_STNT240");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_l_b)
                return Game.GetGXTEntry("MC_PR_STNT49D");
            if (iParam0 == (int)RacingProps.ba_prop_track_bend_l_b)
                return Game.GetGXTEntry("MC_PR_STNT49");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend2_l_b)
                return Game.GetGXTEntry("MC_PR_STNT141");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_bar_l)
                return Game.GetGXTEntry("MC_PR_STNT241");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend2_bar_l)
                return Game.GetGXTEntry("MC_PR_STNT259");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend_bar_l_b)
                return Game.GetGXTEntry("MC_PR_STNT53");
            if (iParam0 == (int)RacingProps.stt_prop_track_bend2_bar_l_b)
                return Game.GetGXTEntry("MC_PR_STNT142");
            if (iParam0 == (int)RacingProps.stt_prop_race_gantry_01)
                return Game.GetGXTEntry("MC_PR_STNT242");
            if (iParam0 == (int)RacingProps.ch_prop_ch_race_gantry_02)
                return Game.GetGXTEntry("MC_PR_STNT242b");
            if (iParam0 == (int)RacingProps.ch_prop_ch_race_gantry_03)
                return Game.GetGXTEntry("MC_PR_STNT242c");
            if (iParam0 == (int)RacingProps.ch_prop_ch_race_gantry_04)
                return Game.GetGXTEntry("MC_PR_STNT242d");
            if (iParam0 == (int)RacingProps.ch_prop_ch_race_gantry_05)
                return Game.GetGXTEntry("MC_PR_STNT242e");
            if (iParam0 == (int)RacingProps.stt_prop_track_funnel)
                return Game.GetGXTEntry("MC_PR_STNT58");
            if (iParam0 == (int)RacingProps.stt_prop_track_funnel_ads_01a)
                return Game.GetGXTEntry("MC_PR_STNT235");
            if (iParam0 == (int)RacingProps.stt_prop_track_funnel_ads_01b)
                return Game.GetGXTEntry("MC_PR_STNT253");
            if (iParam0 == (int)RacingProps.stt_prop_track_funnel_ads_01c)
                return Game.GetGXTEntry("MC_PR_STNT254");
            if (iParam0 == (int)RacingProps.stt_prop_track_link)
                return Game.GetGXTEntry("MC_PR_STNT115");
            if (iParam0 == (int)RacingProps.stt_prop_track_start)
                return Game.GetGXTEntry("MC_PR_STNT59");
            if (iParam0 == (int)RacingProps.stt_prop_track_start_02)
                return Game.GetGXTEntry("MC_PR_STNT2");
            if (iParam0 == (int)RacingProps.stt_prop_track_straight_bar_l)
                return Game.GetGXTEntry("MC_PR_STNT55");
            if (iParam0 == (int)RacingProps.stt_prop_track_straight_lm_bar)
                return Game.GetGXTEntry("MC_PR_STNT55a");
            if (iParam0 == (int)RacingProps.stt_prop_track_straight_bar_m)
                return Game.GetGXTEntry("MC_PR_STNT54");
            if (iParam0 == (int)RacingProps.stt_prop_track_straight_bar_s)
                return Game.GetGXTEntry("MC_PR_STNT106");
            if (iParam0 == (int)RacingProps.stt_prop_track_straight_l)
                return Game.GetGXTEntry("MC_PR_STNT51");
            if (iParam0 == (int)RacingProps.stt_prop_track_straight_lm)
                return Game.GetGXTEntry("MC_O_STNT51a");
            if (iParam0 == (int)RacingProps.ba_prop_track_straight_lm)
                return Game.GetGXTEntry("MC_PR_STNT51a");
            if (iParam0 == (int)RacingProps.stt_prop_track_straight_m)
                return Game.GetGXTEntry("MC_PR_STNT50");
            if (iParam0 == (int)RacingProps.stt_prop_track_straight_s)
                return Game.GetGXTEntry("MC_PR_STNT105");
            if (iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_01)
                return Game.GetGXTEntry("MC_PR_STPS");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_01)
                return Game.GetGXTEntry("MC_PR_STNT256");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_01b)
                return Game.GetGXTEntry("MC_PR_STNT91");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_04)
                return Game.GetGXTEntry("MC_PR_STNT60");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_45r)
                return Game.GetGXTEntry("MC_PR_STNT143");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_45ra)
                return Game.GetGXTEntry("MC_PR_STNT249");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_45l)
                return Game.GetGXTEntry("MC_PR_STNT144");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_45la)
                return Game.GetGXTEntry("MC_PR_STNT250");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_90r)
                return Game.GetGXTEntry("MC_PR_STNT145");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_90rb)
                return Game.GetGXTEntry("MC_PR_STNT251");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_90l)
                return Game.GetGXTEntry("MC_PR_STNT146");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_90lb)
                return Game.GetGXTEntry("MC_PR_STNT252");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_02)
                return Game.GetGXTEntry("MC_PR_STNT257");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_02b)
                return Game.GetGXTEntry("MC_PR_STNT92");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_05)
                return Game.GetGXTEntry("MC_PR_STNT258");
            if (iParam0 == (int)RacingProps.stt_prop_wallride_05b)
                return Game.GetGXTEntry("MC_PR_STNT61");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_exshort)
                return Game.GetGXTEntry("MC_PR_STNT154");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_short)
                return Game.GetGXTEntry("MC_PR_STNT103");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_straight)
                return Game.GetGXTEntry("MC_PR_STNT80");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_cutout)
                return Game.GetGXTEntry("MC_PR_STNT113");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_otake)
                return Game.GetGXTEntry("MC_PR_STNT69");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_fork)
                return Game.GetGXTEntry("MC_PR_STNT139");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_funnel)
                return Game.GetGXTEntry("MC_PR_STNT70");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_funlng)
                return Game.GetGXTEntry("MC_PR_STNT140");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_slope15)
                return Game.GetGXTEntry("MC_PR_STNT74");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_slope30)
                return Game.GetGXTEntry("MC_PR_STNT75");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_slope45)
                return Game.GetGXTEntry("MC_PR_STNT76");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_hill)
                return Game.GetGXTEntry("MC_PR_STNT77");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_hill2)
                return Game.GetGXTEntry("MC_PR_STNT78");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_bumps)
                return Game.GetGXTEntry("MC_PR_STNT136");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_jump)
                return Game.GetGXTEntry("MC_PR_STNT79");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump15)
                return Game.GetGXTEntry("MC_PR_STNT116");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump30)
                return Game.GetGXTEntry("MC_PR_STNT117");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_jump45)
                return Game.GetGXTEntry("MC_PR_STNT118");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_start)
                return Game.GetGXTEntry("MC_PR_STNT71");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_start_02)
                return Game.GetGXTEntry("MC_PR_STNT1");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_st_01)
                return Game.GetGXTEntry("MC_PR_STNT246");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_st_02)
                return Game.GetGXTEntry("MC_PR_STNT245");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_turn)
                return Game.GetGXTEntry("MC_PR_STNT73");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_sh15)
                return Game.GetGXTEntry("MC_PR_STNT107");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_sh30)
                return Game.GetGXTEntry("MC_PR_STNT108");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_sh45)
                return Game.GetGXTEntry("MC_PR_STNT109");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_sh45_a)
                return Game.GetGXTEntry("MC_PR_STNT109A");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_uturn)
                return Game.GetGXTEntry("MC_PR_STNT81");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_link)
                return Game.GetGXTEntry("MC_PR_STNT114");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_dwlink)
                return Game.GetGXTEntry("MC_PR_STNT3");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_dwlink_02)
                return Game.GetGXTEntry("MC_PR_STNT247");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_dwshort)
                return Game.GetGXTEntry("MC_PR_STNT4");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_dwsh15)
                return Game.GetGXTEntry("MC_PR_STNT5");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_dwturn)
                return Game.GetGXTEntry("MC_PR_STNT6");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_dwuturn)
                return Game.GetGXTEntry("MC_PR_STNT7");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_dwslope15)
                return Game.GetGXTEntry("MC_PR_STNT8");
            if (iParam0 == (int)RacingProps.as_prop_as_dwslope30)
                return Game.GetGXTEntry("MC_PR_STNT9");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_track_dwslope45)
                return Game.GetGXTEntry("MC_PR_STNT10");
            if (iParam0 == (int)RacingProps.stt_prop_track_speedup)
                return Game.GetGXTEntry("MC_PR_STNT132");
            if (iParam0 == (int)RacingProps.stt_prop_track_speedup_t1)
                return Game.GetGXTEntry("MC_PR_STNT231");
            if (iParam0 == (int)RacingProps.stt_prop_track_speedup_t2)
                return Game.GetGXTEntry("MC_PR_STNT232");
            if (iParam0 == (int)RacingProps.stt_prop_track_slowdown)
                return Game.GetGXTEntry("MC_PR_STNT133");
            if (iParam0 == (int)RacingProps.stt_prop_track_slowdown_t1)
                return Game.GetGXTEntry("MC_PR_STNT233");
            if (iParam0 == (int)RacingProps.stt_prop_track_slowdown_t2)
                return Game.GetGXTEntry("MC_PR_STNT234");
            if (iParam0 == -583990942)
                return Game.GetGXTEntry("MC_PR_STNT6");
            if (iParam0 == 1601693814)
                return Game.GetGXTEntry("MC_PR_STNT7");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_sml1)
                return Game.GetGXTEntry("MC_PR_STNT120");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_sml2)
                return Game.GetGXTEntry("MC_PR_STNT121");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_sml3)
                return Game.GetGXTEntry("MC_PR_STNT122");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_mdm1)
                return Game.GetGXTEntry("MC_PR_STNT123");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_mdm2)
                return Game.GetGXTEntry("MC_PR_STNT124");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_mdm3)
                return Game.GetGXTEntry("MC_PR_STNT125");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_lrg1)
                return Game.GetGXTEntry("MC_PR_STNT126");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_lrg2)
                return Game.GetGXTEntry("MC_PR_STNT127");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_lrg3)
                return Game.GetGXTEntry("MC_PR_STNT128");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_xl1)
                return Game.GetGXTEntry("MC_PR_STNT129");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_xl2)
                return Game.GetGXTEntry("MC_PR_STNT130");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_xl3)
                return Game.GetGXTEntry("MC_PR_STNT131");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_huge_01)
                return Game.GetGXTEntry("MC_PR_STNT147");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_huge_02)
                return Game.GetGXTEntry("MC_PR_STNT148");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_huge_03)
                return Game.GetGXTEntry("MC_PR_STNT149");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_huge_04)
                return Game.GetGXTEntry("MC_PR_STNT229");
            if (iParam0 == (int)RacingProps.stt_prop_stunt_bblock_huge_05)
                return Game.GetGXTEntry("MC_PR_STNT230");
            if (iParam0 == (int)RacingProps.stt_prop_hoop_small_01)
                return Game.GetGXTEntry("MC_PR_STNT188");
            if (iParam0 == (int)RacingProps.ar_prop_ar_hoop_med_01)
                return Game.GetGXTEntry("MC_AR_PROP_60");
            if (iParam0 == (int)RacingProps.stt_prop_hoop_constraction_01a)
                return Game.GetGXTEntry("MC_PR_STNT189");
            if (iParam0 == (int)RacingProps.stt_prop_race_tannoy)
                return Game.GetGXTEntry("MC_PR_STNT255");
            if (iParam0 == (int)RacingProps.sum_prop_ac_wall_sign_01)
                return Game.GetGXTEntry("MC_PR_AS_F");
            if (iParam0 == (int)RacingProps.sum_prop_ac_wall_sign_02)
                return Game.GetGXTEntry("MC_PR_AS_LM");
            if (iParam0 == (int)RacingProps.sum_prop_ac_wall_sign_03)
                return Game.GetGXTEntry("MC_PR_AS_RM");
            if (iParam0 == (int)RacingProps.sum_prop_ac_wall_sign_04)
                return Game.GetGXTEntry("MC_PR_AS_LL");
            if (iParam0 == (int)RacingProps.sum_prop_ac_wall_sign_05)
                return Game.GetGXTEntry("MC_PR_AS_RL");
            if (iParam0 == (int)RacingProps.sum_prop_ac_wall_sign_0l1)
                return Game.GetGXTEntry("MC_PR_AS_L");
            if (iParam0 == (int)RacingProps.sum_prop_ac_wall_sign_0r1)
                return Game.GetGXTEntry("MC_PR_AS_R");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_01)
                return Game.GetGXTEntry("MC_PR_STNT213");
            if (iParam0 == (int)RacingProps.sum_prop_ac_tyre_wall_lit_01)
                return Game.GetGXTEntry("MC_PR_STNT213");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_02)
                return Game.GetGXTEntry("MC_PR_STNT214");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_03)
                return Game.GetGXTEntry("MC_PR_STNT215");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_04)
                return Game.GetGXTEntry("MC_PR_STNT274");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_05)
                return Game.GetGXTEntry("MC_PR_STNT275");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_06)
                return Game.GetGXTEntry("MC_PR_STNT276");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_07)
                return Game.GetGXTEntry("MC_PR_STNT277");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_08)
                return Game.GetGXTEntry("MC_PR_STNT278");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_09)
                return Game.GetGXTEntry("MC_PR_STNT279");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_010)
                return Game.GetGXTEntry("MC_PR_STNT280");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_011)
                return Game.GetGXTEntry("MC_PR_STNT281");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_012)
                return Game.GetGXTEntry("MC_PR_STNT282");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_013)
                return Game.GetGXTEntry("MC_PR_STNT283");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_014)
                return Game.GetGXTEntry("MC_PR_STNT316");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_015)
                return Game.GetGXTEntry("MC_PR_STNT284");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r1)
                return Game.GetGXTEntry("MC_PR_STNT216");
            if (iParam0 == (int)RacingProps.sum_prop_ac_tyre_wall_lit_0r1)
                return Game.GetGXTEntry("MC_PR_STNT216");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r2)
                return Game.GetGXTEntry("MC_PR_STNT217");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r06)
                return Game.GetGXTEntry("MC_PR_STNT285");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r07)
                return Game.GetGXTEntry("MC_PR_STNT286");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r011)
                return Game.GetGXTEntry("MC_PR_STNT287");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r012)
                return Game.GetGXTEntry("MC_PR_STNT288");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r013)
                return Game.GetGXTEntry("MC_PR_STNT289");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r014)
                return Game.GetGXTEntry("MC_PR_STNT290");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r019)
                return Game.GetGXTEntry("MC_PR_STNT291");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r3)
                return Game.GetGXTEntry("MC_PR_STNT218");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r04)
                return Game.GetGXTEntry("MC_PR_STNT292");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r05)
                return Game.GetGXTEntry("MC_PR_STNT293");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r08)
                return Game.GetGXTEntry("MC_PR_STNT294");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r09)
                return Game.GetGXTEntry("MC_PR_STNT295");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r010)
                return Game.GetGXTEntry("MC_PR_STNT296");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r015)
                return Game.GetGXTEntry("MC_PR_STNT297");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r016)
                return Game.GetGXTEntry("MC_PR_STNT298");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r017)
                return Game.GetGXTEntry("MC_PR_STNT299");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0r018)
                return Game.GetGXTEntry("MC_PR_STNT300");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l1)
                return Game.GetGXTEntry("MC_PR_STNT219");
            if (iParam0 == (int)RacingProps.sum_prop_ac_tyre_wall_lit_0l1)
                return Game.GetGXTEntry("MC_PR_STNT219");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l2)
                return Game.GetGXTEntry("MC_PR_STNT220");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l06)
                return Game.GetGXTEntry("MC_PR_STNT301");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l07)
                return Game.GetGXTEntry("MC_PR_STNT302");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l013)
                return Game.GetGXTEntry("MC_PR_STNT303");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l014)
                return Game.GetGXTEntry("MC_PR_STNT304");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l015)
                return Game.GetGXTEntry("MC_PR_STNT305");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l020)
                return Game.GetGXTEntry("MC_PR_STNT306");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l3)
                return Game.GetGXTEntry("MC_PR_STNT221");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l04)
                return Game.GetGXTEntry("MC_PR_STNT307");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l05)
                return Game.GetGXTEntry("MC_PR_STNT308");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l08)
                return Game.GetGXTEntry("MC_PR_STNT309");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l010)
                return Game.GetGXTEntry("MC_PR_STNT310");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l012)
                return Game.GetGXTEntry("MC_PR_STNT311");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l16)
                return Game.GetGXTEntry("MC_PR_STNT312");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l17)
                return Game.GetGXTEntry("MC_PR_STNT313");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l018)
                return Game.GetGXTEntry("MC_PR_STNT314");
            if (iParam0 == (int)RacingProps.stt_prop_tyre_wall_0l019)
                return Game.GetGXTEntry("MC_PR_STNT315");
            if (iParam0 == (int)RacingProps.sum_prop_ac_tyre_wall_pit_l)
                return Game.GetGXTEntry("MC_PR_TW_PL");
            if (iParam0 == (int)RacingProps.sum_prop_ac_tyre_wall_pit_r)
                return Game.GetGXTEntry("MC_PR_TW_PR");
            if (iParam0 == (int)RacingProps.sum_prop_ac_tyre_wall_u_l)
                return Game.GetGXTEntry("MC_PR_TW_UTL");
            if (iParam0 == (int)RacingProps.sum_prop_ac_tyre_wall_u_r)
                return Game.GetGXTEntry("MC_PR_TW_UTR");
            if (iParam0 == (int)RacingProps.stt_prop_speakerstack_01a)
                return Game.GetGXTEntry("MC_PR_STNT263");
            if (iParam0 == (int)RacingProps.ar_prop_ar_stunt_block_01a)
                return Game.GetGXTEntry("MC_PR_STNT323");
            if (iParam0 == (int)RacingProps.ar_prop_ar_stunt_block_01b)
                return Game.GetGXTEntry("MC_PR_STNT324");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_straight_bar_s_s)
                return Game.GetGXTEntry("MC_PR_STNT326");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_straight_bar_s)
                return Game.GetGXTEntry("MC_PR_STNT327");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_l_out)
                return Game.GetGXTEntry("MC_PR_STNT328");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_l_b)
                return Game.GetGXTEntry("MC_PR_STNT329");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_m_out)
                return Game.GetGXTEntry("MC_PR_STNT330");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_m_in)
                return Game.GetGXTEntry("MC_PR_STNT331");
            if (iParam0 == (int)RacingProps.sum_prop_track_ac_straight_bar_s_s)
                return Game.GetGXTEntry("MC_PR_STNT326");
            if (iParam0 == (int)RacingProps.sum_prop_track_ac_straight_bar_s)
                return Game.GetGXTEntry("MC_PR_STNT327");
            if (iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_m_out)
                return Game.GetGXTEntry("MC_PR_STNT330");
            if (iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_m_in)
                return Game.GetGXTEntry("MC_PR_STNT331");
            if (iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_l_out)
                return Game.GetGXTEntry("MC_PR_STNT328");
            if (iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_l_b)
                return Game.GetGXTEntry("MC_PR_STNT329");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_straight_bar_m)
                return Game.GetGXTEntry("MC_PR_STNT332");
            if (iParam0 == (int)RacingProps.ch_prop_stunt_landing_zone_01a)
                return Game.GetGXTEntry("MC_PR_STNT333");
            if (iParam0 == (int)RacingProps.ch_prop_track_bend_bar_lc)
                return Game.GetGXTEntry("MC_PR_STNT335b");
            if (iParam0 == (int)RacingProps.sum_prop_track_ac_bend_lc)
                return Game.GetGXTEntry("MC_PR_STNT335");
            if (iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_180d)
                return Game.GetGXTEntry("MC_PR_STNT336");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_45d || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_45)
                return Game.GetGXTEntry("MC_PR_STNT339");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_bend_45 || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_45)
                return Game.GetGXTEntry("MC_PR_STNT340");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_135 || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_135)
                return Game.GetGXTEntry("MC_PR_STNT341");
            if (iParam0 == (int)RacingProps.ch_prop_track_ch_bend_135 || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_135)
                return Game.GetGXTEntry("MC_PR_STNT342");
            if (iParam0 == (int)RacingProps.as_prop_as_target_small_02)
                return Game.GetGXTEntry("MC_TGT_TS0");
            if (iParam0 == (int)RacingProps.as_prop_as_target_small)
                return Game.GetGXTEntry("MC_TGT_TS1");
            if (iParam0 == (int)RacingProps.as_prop_as_target_medium)
                return Game.GetGXTEntry("MC_TGT_TM");
            if (iParam0 == (int)RacingProps.as_prop_as_target_big)
                return Game.GetGXTEntry("MC_TGT_TB");
            if (iParam0 == (int)RacingProps.as_prop_as_target_scaffold_01a)
                return Game.GetGXTEntry("MC_TGT_SF0");
            if (iParam0 == (int)RacingProps.as_prop_as_target_scaffold_01b)
                return Game.GetGXTEntry("MC_TGT_SF1");
            if (iParam0 == (int)RacingProps.as_prop_as_target_scaffold_02a)
                return Game.GetGXTEntry("MC_TGT_SF2");
            if (iParam0 == (int)RacingProps.as_prop_as_target_scaffold_02b)
                return Game.GetGXTEntry("MC_TGT_SF3");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_bigbag_01a)
                return Game.GetGXTEntry("MC_BKR_DG_0");
            if (iParam0 == (int)RacingProps.bkr_prop_meth_smallbag_01a)
                return Game.GetGXTEntry("MC_BKR_DG_1");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_bucket_01a)
                return Game.GetGXTEntry("MC_BKR_DG_2");
            if (iParam0 == (int)RacingProps.bkr_prop_coke_boxeddoll)
                return Game.GetGXTEntry("MC_BKR_DG_3");
            if (iParam0 == (int)RacingProps.prop_keg_01)
                return Game.GetGXTEntry("MC_BKR_DG_4");
            if (iParam0 == (int)RacingProps.bkr_prop_coke_table01a)
                return Game.GetGXTEntry("MC_BKR_DG_5");
            if (iParam0 == (int)RacingProps.bkr_prop_meth_table01a)
                return Game.GetGXTEntry("MC_BKR_DG_6");
            if (iParam0 == (int)RacingProps.bkr_prop_meth_phosphorus)
                return Game.GetGXTEntry("MC_BKR_DG_7");
            if (iParam0 == (int)RacingProps.prop_meth_setup_01)
                return Game.GetGXTEntry("MC_BKR_DG_8");
            if (iParam0 == (int)RacingProps.bkr_prop_meth_pseudoephedrine)
                return Game.GetGXTEntry("MC_BKR_DG_9");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_table_01a)
                return Game.GetGXTEntry("MC_BKR_DG_10");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_bigbag_open_01a)
                return Game.GetGXTEntry("MC_BKR_DG_11");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_scales_01a)
                return Game.GetGXTEntry("MC_BKR_DG_13");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_lrg_01a)
                return Game.GetGXTEntry("MC_BKR_DG_14");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_lrg_01b)
                return Game.GetGXTEntry("MC_BKR_DG_15");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_med_01a)
                return Game.GetGXTEntry("MC_BKR_DG_16");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_med_01b)
                return Game.GetGXTEntry("MC_BKR_DG_17");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_01_small_01a)
                return Game.GetGXTEntry("MC_BKR_DG_18");
            if (iParam0 == (int)RacingProps.bkr_prop_weed_smallbag_01a)
                return Game.GetGXTEntry("MC_BKR_DG_19");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_sml1)
                return Game.GetGXTEntry("MC_BKR_DG_20");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_sml2)
                return Game.GetGXTEntry("MC_BKR_DG_21");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_sml3)
                return Game.GetGXTEntry("MC_BKR_DG_22");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_mdm1)
                return Game.GetGXTEntry("MC_BKR_DG_23");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_mdm2)
                return Game.GetGXTEntry("MC_BKR_DG_24");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_mdm3)
                return Game.GetGXTEntry("MC_BKR_DG_25");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_lrg1)
                return Game.GetGXTEntry("MC_BKR_DG_26");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_lrg2)
                return Game.GetGXTEntry("MC_BKR_DG_27");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_lrg3)
                return Game.GetGXTEntry("MC_BKR_DG_28");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_xl1)
                return Game.GetGXTEntry("MC_BKR_DG_29");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_xl2)
                return Game.GetGXTEntry("MC_BKR_DG_30");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_xl3)
                return Game.GetGXTEntry("MC_BKR_DG_31");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_huge_01)
                return Game.GetGXTEntry("MC_BKR_DG_32");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_huge_02)
                return Game.GetGXTEntry("MC_BKR_DG_33");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_huge_03)
                return Game.GetGXTEntry("MC_BKR_DG_34");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_huge_04)
                return Game.GetGXTEntry("MC_BKR_DG_35");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_huge_05)
                return Game.GetGXTEntry("MC_BKR_DG_36");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_qp)
                return Game.GetGXTEntry("MC_BKR_DG_37");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_qp2)
                return Game.GetGXTEntry("MC_BKR_DG_38");
            if (iParam0 == (int)RacingProps.imp_prop_impexp_bblock_qp3)
                return Game.GetGXTEntry("MC_BKR_DG_39");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_hump_01)
                return Game.GetGXTEntry("MC_BKR_DG_40");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bblock_hump_02)
                return Game.GetGXTEntry("MC_BKR_DG_41");
            if (iParam0 == (int)RacingProps.imp_prop_impexp_bblock_huge_01)
                return Game.GetGXTEntry("MC_IE_PROP_01");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_landing_zone_01)
                return Game.GetGXTEntry("MC_BKR_DG_42");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_bowlpin_stand)
                return Game.GetGXTEntry("MC_BKR_DG_43");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_target_small)
                return Game.GetGXTEntry("MC_BKR_DG_44");
            if (iParam0 == (int)RacingProps.bkr_prop_biker_target)
                return Game.GetGXTEntry("MC_BKR_DG_45");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_01a)
                return Game.GetGXTEntry("MC_SR_PROP_01");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_02a)
                return Game.GetGXTEntry("MC_SR_PROP_01");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_03a)
                return Game.GetGXTEntry("MC_SR_PROP_01");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_04a)
                return Game.GetGXTEntry("MC_SR_PROP_01");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_05a)
                return Game.GetGXTEntry("MC_SR_PROP_01");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_30d_01a)
                return Game.GetGXTEntry("MC_SR_PROP_02");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_30d_02a)
                return Game.GetGXTEntry("MC_SR_PROP_02");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_30d_03a)
                return Game.GetGXTEntry("MC_SR_PROP_02");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_30d_04a)
                return Game.GetGXTEntry("MC_SR_PROP_02");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_crn_30d_05a)
                return Game.GetGXTEntry("MC_SR_PROP_02");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_s_01a)
                return Game.GetGXTEntry("MC_SR_PROP_03");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_s_02a)
                return Game.GetGXTEntry("MC_SR_PROP_03");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_s_03a)
                return Game.GetGXTEntry("MC_SR_PROP_03");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_s_04a)
                return Game.GetGXTEntry("MC_SR_PROP_03");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_s_05a)
                return Game.GetGXTEntry("MC_SR_PROP_03");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_m_01a)
                return Game.GetGXTEntry("MC_SR_PROP_04");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_m_02a)
                return Game.GetGXTEntry("MC_SR_PROP_04");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_m_03a)
                return Game.GetGXTEntry("MC_SR_PROP_04");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_m_04a)
                return Game.GetGXTEntry("MC_SR_PROP_04");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_m_05a)
                return Game.GetGXTEntry("MC_SR_PROP_04");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_l_01a)
                return Game.GetGXTEntry("MC_SR_PROP_05");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_l_02a)
                return Game.GetGXTEntry("MC_SR_PROP_05");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_l_03a)
                return Game.GetGXTEntry("MC_SR_PROP_05");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_l_04a)
                return Game.GetGXTEntry("MC_SR_PROP_05");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_l_05a)
                return Game.GetGXTEntry("MC_SR_PROP_05");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_xxs_01a)
                return Game.GetGXTEntry("MC_SR_PROP_06");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_xxs_02a)
                return Game.GetGXTEntry("MC_SR_PROP_06");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_xxs_03a)
                return Game.GetGXTEntry("MC_SR_PROP_06");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_xxs_04a)
                return Game.GetGXTEntry("MC_SR_PROP_06");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_xxs_05a)
                return Game.GetGXTEntry("MC_SR_PROP_06");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_5d_01a)
                return Game.GetGXTEntry("MC_SR_PROP_07");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_5d_02a)
                return Game.GetGXTEntry("MC_SR_PROP_07");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_5d_03a)
                return Game.GetGXTEntry("MC_SR_PROP_07");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_5d_04a)
                return Game.GetGXTEntry("MC_SR_PROP_07");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_5d_05a)
                return Game.GetGXTEntry("MC_SR_PROP_07");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_15d_01a)
                return Game.GetGXTEntry("MC_SR_PROP_08");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_15d_02a)
                return Game.GetGXTEntry("MC_SR_PROP_08");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_15d_03a)
                return Game.GetGXTEntry("MC_SR_PROP_08");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_15d_04a)
                return Game.GetGXTEntry("MC_SR_PROP_08");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn_15d_05a)
                return Game.GetGXTEntry("MC_SR_PROP_08");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn2_01a)
                return Game.GetGXTEntry("MC_SR_PROP_09");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn2_02a)
                return Game.GetGXTEntry("MC_SR_PROP_09");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn2_03a)
                return Game.GetGXTEntry("MC_SR_PROP_09");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn2_04a)
                return Game.GetGXTEntry("MC_SR_PROP_09");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_crn2_05a)
                return Game.GetGXTEntry("MC_SR_PROP_09");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_xs_01a)
                return Game.GetGXTEntry("MC_SR_PROP_10");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_xs_02a)
                return Game.GetGXTEntry("MC_SR_PROP_10");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_xs_03a)
                return Game.GetGXTEntry("MC_SR_PROP_10");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_xs_04a)
                return Game.GetGXTEntry("MC_SR_PROP_10");
            if (iParam0 == (int)RacingProps.sr_prop_stunt_tube_xs_05a)
                return Game.GetGXTEntry("MC_SR_PROP_10");
            if (iParam0 == (int)RacingProps.sr_prop_track_straight_l_d5)
                return Game.GetGXTEntry("MC_SR_PROP_31");
            if (iParam0 == (int)RacingProps.sr_prop_track_straight_l_d15)
                return Game.GetGXTEntry("MC_SR_PROP_32");
            if (iParam0 == (int)RacingProps.sr_prop_track_straight_l_d30)
                return Game.GetGXTEntry("MC_SR_PROP_33");
            if (iParam0 == (int)RacingProps.sr_prop_track_straight_l_d45)
                return Game.GetGXTEntry("MC_SR_PROP_34");
            if (iParam0 == (int)RacingProps.sr_prop_track_straight_l_u5)
                return Game.GetGXTEntry("MC_SR_PROP_35");
            if (iParam0 == (int)RacingProps.sr_prop_track_straight_l_u15)
                return Game.GetGXTEntry("MC_SR_PROP_36");
            if (iParam0 == (int)RacingProps.sr_prop_track_straight_l_u30)
                return Game.GetGXTEntry("MC_SR_PROP_37");
            if (iParam0 == (int)RacingProps.sr_prop_track_straight_l_u45)
                return Game.GetGXTEntry("MC_SR_PROP_38");
            if (iParam0 == (int)RacingProps.sr_prop_track_refill)
                return Game.GetGXTEntry("MC_SR_PROP_39");
            if (iParam0 == (int)RacingProps.sr_prop_track_refill_t2)
                return Game.GetGXTEntry("MC_SR_PROP_40");
            if (iParam0 == (int)RacingProps.sr_prop_track_refill_t1)
                return Game.GetGXTEntry("MC_SR_PROP_41");
            if (iParam0 == (int)RacingProps.sr_prop_sr_track_jumpwall)
                return Game.GetGXTEntry("MC_SR_PROP_42");
            if (iParam0 == (int)RacingProps.sr_prop_spec_tube_refill)
                return Game.GetGXTEntry("MC_SR_PROP_44");
            if (iParam0 == (int)RacingProps.sr_mp_spec_races_blimp_sign)
                return Game.GetGXTEntry("MC_SR_PROP_45");
            if (iParam0 == (int)RacingProps.sr_mp_spec_races_take_flight_sign)
                return Game.GetGXTEntry("MC_SR_PROP_46");
            if (iParam0 == (int)RacingProps.sr_mp_spec_races_ron_sign)
                return Game.GetGXTEntry("MC_SR_PROP_47");
            if (iParam0 == (int)RacingProps.sr_mp_spec_races_xero_sign)
                return Game.GetGXTEntry("MC_SR_PROP_48");
            if (iParam0 == (int)RacingProps.ar_prop_ar_arrow_thin_m)
                return Game.GetGXTEntry("MC_SM_PROP_0");
            if (iParam0 == (int)RacingProps.ar_prop_ar_arrow_wide_m)
                return Game.GetGXTEntry("MC_SM_PROP_1");
            if (iParam0 == (int)RacingProps.ar_prop_ar_arrow_thin_l)
                return Game.GetGXTEntry("MC_SM_PROP_2");
            if (iParam0 == (int)RacingProps.ar_prop_ar_arrow_wide_l)
                return Game.GetGXTEntry("MC_SM_PROP_3");
            if (iParam0 == (int)RacingProps.ar_prop_ar_arrow_thin_xl)
                return Game.GetGXTEntry("MC_SM_PROP_4");
            if (iParam0 == (int)RacingProps.ar_prop_ar_arrow_wide_xl)
                return Game.GetGXTEntry("MC_SM_PROP_5");
            if (iParam0 == (int)RacingProps.sum_prop_archway_01)
                return Game.GetGXTEntry("MC_PR_SIGN_1");
            if (iParam0 == (int)RacingProps.sum_prop_archway_02)
                return Game.GetGXTEntry("MC_PR_SIGN_2");
            if (iParam0 == (int)RacingProps.sum_prop_archway_03)
                return Game.GetGXTEntry("MC_PR_SIGN_3");
            if (iParam0 == (int)RacingProps.sum_prop_ac_pit_sign_r_01a)
                return Game.GetGXTEntry("MC_PR_SIGN_4");
            if (iParam0 == (int)RacingProps.sum_prop_ac_pit_sign_l_01a)
                return Game.GetGXTEntry("MC_PR_SIGN_5");
            if (iParam0 == (int)RacingProps.gr_prop_gr_bunkeddoor)
                return Game.GetGXTEntry("MC_GR_BNKR_DR");
            if (iParam0 == (int)RacingProps.xm_prop_x17_osphatch_27m)
                return Game.GetGXTEntry("MC_H2_FAC_HAT");
            if (iParam0 == (int)RacingProps.gr_prop_gr_bench_03a)
                return Game.GetGXTEntry("FMMC_PR_LTBL");
            if (iParam0 == (int)RacingProps.gr_prop_gr_bench_04a)
                return Game.GetGXTEntry("FMMC_PR_STBL");
            if (iParam0 == (int)RacingProps.gr_prop_gr_bench_04b)
                return Game.GetGXTEntry("FMMC_PR_STBD");
            if (iParam0 == (int)RacingProps.v_res_pctower)
                return Game.GetGXTEntry("FMMC_PR_PCTW");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_sml1)
                return Game.GetGXTEntry("MC_SR_PROP_49");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_mdm1)
                return Game.GetGXTEntry("MC_SR_PROP_50");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_lrg11)
                return Game.GetGXTEntry("MC_SR_PROP_51");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_xl1)
                return Game.GetGXTEntry("MC_SR_PROP_52");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_sml2)
                return Game.GetGXTEntry("MC_SR_PROP_53");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_mdm2)
                return Game.GetGXTEntry("MC_SR_PROP_54");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_lrg2)
                return Game.GetGXTEntry("MC_SR_PROP_55");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_xl2)
                return Game.GetGXTEntry("MC_SR_PROP_56");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_sml3)
                return Game.GetGXTEntry("MC_SR_PROP_57");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_mdm3)
                return Game.GetGXTEntry("MC_SR_PROP_58");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_lrg3)
                return Game.GetGXTEntry("MC_SR_PROP_59");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_xl3)
                return Game.GetGXTEntry("MC_SR_PROP_60x");
            if (iParam0 == (int)RacingProps.sr_prop_special_bblock_xl3_fixed)
                return Game.GetGXTEntry("MC_SR_PROP_60");
            if (iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_01)
                return Game.GetGXTEntry("MC_AR_PROP_61");
            if (iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_02)
                return Game.GetGXTEntry("MC_AR_PROP_62");
            if (iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_03)
                return Game.GetGXTEntry("MC_AR_PROP_63");
            if (iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_04)
                return Game.GetGXTEntry("MC_AR_PROP_64");
            if (iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_05)
                return Game.GetGXTEntry("MC_AR_PROP_65");
            if (iParam0 == (int)RacingProps.ar_prop_ar_stunt_block_01a)
                return Game.GetGXTEntry("MC_AR_PROP_97");
            if (iParam0 == (int)RacingProps.ar_prop_ar_stunt_block_01b)
                return Game.GetGXTEntry("MC_AR_PROP_98");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_1_01a)
                return Game.GetGXTEntry("MC_SR_PROP_61");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_1_01b)
                return Game.GetGXTEntry("MC_SR_PROP_61b");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_2_04a)
                return Game.GetGXTEntry("MC_SR_PROP_62");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_2_04b)
                return Game.GetGXTEntry("MC_SR_PROP_62b");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_3_03a)
                return Game.GetGXTEntry("MC_SR_PROP_63");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_3_03b)
                return Game.GetGXTEntry("MC_SR_PROP_63b");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_4_01a)
                return Game.GetGXTEntry("MC_SR_PROP_64");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_4_01b)
                return Game.GetGXTEntry("MC_SR_PROP_64b");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_5_01a)
                return Game.GetGXTEntry("MC_SR_PROP_65");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_5_01b)
                return Game.GetGXTEntry("MC_SR_PROP_65b");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_large_01a)
                return Game.GetGXTEntry("MC_SR_PROP_66");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_large_01b)
                return Game.GetGXTEntry("MC_SR_PROP_66b");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_long_01a)
                return Game.GetGXTEntry("MC_SR_PROP_67");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_small_01a)
                return Game.GetGXTEntry("MC_SR_PROP_68");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_small_01b)
                return Game.GetGXTEntry("MC_SR_PROP_68b");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_small_02a)
                return Game.GetGXTEntry("MC_SR_PROP_72");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_small_03a)
                return Game.GetGXTEntry("MC_SR_PROP_73");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_small_04a)
                return Game.GetGXTEntry("MC_SR_PROP_74");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_small_05a)
                return Game.GetGXTEntry("MC_SR_PROP_69");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_small_06a)
                return Game.GetGXTEntry("MC_SR_PROP_75");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_small_07a)
                return Game.GetGXTEntry("MC_SR_PROP_76");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_trap_01a)
                return Game.GetGXTEntry("MC_SR_PROP_70");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_trap_02a)
                return Game.GetGXTEntry("MC_SR_PROP_71");
            if (iParam0 == (int)RacingProps.gr_prop_gr_crates_pistols_01a)
                return Game.GetGXTEntry("MC_GR_PROP_01");
            if (iParam0 == (int)RacingProps.gr_prop_gr_crates_rifles_01a)
                return Game.GetGXTEntry("MC_GR_PROP_02");
            if (iParam0 == (int)RacingProps.gr_prop_gr_crates_rifles_02a)
                return Game.GetGXTEntry("MC_GR_PROP_03");
            if (iParam0 == (int)RacingProps.gr_prop_gr_crates_rifles_03a)
                return Game.GetGXTEntry("MC_GR_PROP_04");
            if (iParam0 == (int)RacingProps.gr_prop_gr_crates_rifles_04a)
                return Game.GetGXTEntry("MC_GR_PROP_05");
            if (iParam0 == (int)RacingProps.gr_prop_gr_crates_sam_01a)
                return Game.GetGXTEntry("MC_GR_PROP_06");
            if (iParam0 == (int)RacingProps.gr_prop_gr_crates_weapon_mix_01a)
                return Game.GetGXTEntry("MC_GR_PROP_07");
            if (iParam0 == (int)RacingProps.gr_prop_gr_gunsmithsupl_01a)
                return Game.GetGXTEntry("MC_GR_PROP_08");
            if (iParam0 == (int)RacingProps.gr_prop_gr_gunsmithsupl_02a)
                return Game.GetGXTEntry("MC_GR_PROP_09");
            if (iParam0 == (int)RacingProps.gr_prop_gr_gunsmithsupl_03a)
                return Game.GetGXTEntry("MC_GR_PROP_10");
            if (iParam0 == (int)RacingProps.gr_prop_gr_rsply_crate01a)
                return Game.GetGXTEntry("MC_GR_PROP_11");
            if (iParam0 == (int)RacingProps.gr_prop_gr_rsply_crate02a)
                return Game.GetGXTEntry("MC_GR_PROP_12");
            if (iParam0 == (int)RacingProps.gr_prop_gr_rsply_crate03a)
                return Game.GetGXTEntry("MC_GR_PROP_13");
            if (iParam0 == (int)RacingProps.hei_heist_apart2_door)
                return Game.GetGXTEntry("MC_GR_PROP_14");
            if (iParam0 == (int)RacingProps.prop_target_ora_purp_01)
                return Game.GetGXTEntry("MC_GR_PROP_15");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_01a)
                return Game.GetGXTEntry("MC_GR_PROP_16");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_01b)
                return Game.GetGXTEntry("MC_GR_PROP_17");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_02a)
                return Game.GetGXTEntry("MC_GR_PROP_18");
            if (iParam0 == (int)RacingProps.gr_prop_gr_target_02b)
                return Game.GetGXTEntry("MC_GR_PROP_19");
            if (iParam0 == (int)RacingProps.gr_prop_gr_bench_01a)
                return Game.GetGXTEntry("MC_GR_PROP_20");
            if (iParam0 == (int)RacingProps.gr_prop_gr_bench_01b)
                return Game.GetGXTEntry("MC_GR_PROP_21");
            if (iParam0 == (int)RacingProps.gr_prop_gr_bench_02a)
                return Game.GetGXTEntry("MC_GR_PROP_22");
            if (iParam0 == (int)RacingProps.gr_prop_gr_bench_02b)
                return Game.GetGXTEntry("MC_GR_PROP_23");
            if (iParam0 == (int)RacingProps.gr_prop_gr_speeddrill_01a)
                return Game.GetGXTEntry("MC_GR_PROP_24");
            if (iParam0 == (int)RacingProps.gr_prop_gr_vertmill_01a)
                return Game.GetGXTEntry("MC_GR_PROP_25");
            if (iParam0 == (int)RacingProps.gr_prop_gr_cratespile_01a)
                return Game.GetGXTEntry("MC_GR_PROP_26");
            if (iParam0 == (int)RacingProps.imp_prop_covered_vehicle_01a)
                return Game.GetGXTEntry("MC_NH_PROP_01");
            if (iParam0 == (int)RacingProps.imp_prop_covered_vehicle_04a)
                return Game.GetGXTEntry("MC_NH_PROP_02");
            if (iParam0 == (int)RacingProps.imp_prop_covered_vehicle_02a)
                return Game.GetGXTEntry("MC_NH_PROP_03");
            if (iParam0 == (int)RacingProps.imp_prop_covered_vehicle_05a)
                return Game.GetGXTEntry("MC_NH_PROP_04");
            if (iParam0 == (int)RacingProps.imp_prop_covered_vehicle_03a)
                return Game.GetGXTEntry("MC_NH_PROP_05");
            if (iParam0 == (int)RacingProps.imp_prop_covered_vehicle_06a)
                return Game.GetGXTEntry("MC_NH_PROP_06");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_crn2)
                return Game.GetGXTEntry("MC_AR_PROP2_15");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_crn_30d)
                return Game.GetGXTEntry("MC_AR_PROP2_16");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_crn_15d)
                return Game.GetGXTEntry("MC_AR_PROP2_17");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_crn_5d)
                return Game.GetGXTEntry("MC_AR_PROP2_18");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_crn2)
                return Game.GetGXTEntry("MC_AR_PROP2_19");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_crn_30d)
                return Game.GetGXTEntry("MC_AR_PROP2_20");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_crn_15d)
                return Game.GetGXTEntry("MC_AR_PROP2_21");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_crn_5d)
                return Game.GetGXTEntry("MC_AR_PROP2_22");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_crn)
                return Game.GetGXTEntry("MC_AR_PROP_00");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_crn_15d)
                return Game.GetGXTEntry("MC_AR_PROP_01");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_crn_30d)
                return Game.GetGXTEntry("MC_AR_PROP_02");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_crn_5d)
                return Game.GetGXTEntry("MC_AR_PROP_03");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_crn2)
                return Game.GetGXTEntry("MC_AR_PROP_04");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_cross)
                return Game.GetGXTEntry("MC_AR_PROP_05");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_fork)
                return Game.GetGXTEntry("MC_AR_PROP_06");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_hg)
                return Game.GetGXTEntry("MC_AR_PROP_07");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_jmp)
                return Game.GetGXTEntry("MC_AR_PROP_08");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_l)
                return Game.GetGXTEntry("MC_AR_PROP_09");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_m)
                return Game.GetGXTEntry("MC_AR_PROP_10");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_qg)
                return Game.GetGXTEntry("MC_AR_PROP_11");
            if (iParam0 == (int)RacingProps.as_prop_as_tube_gap_02)
                return Game.GetGXTEntry("MC_AR_PROP_38");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_speed)
                return Game.GetGXTEntry("MC_AR_PROP_54");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_speed)
                return Game.GetGXTEntry("MC_AR_PROP_55");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_speed)
                return Game.GetGXTEntry("MC_AR_PROP_56");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_s)
                return Game.GetGXTEntry("MC_AR_PROP_12");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_xxs)
                return Game.GetGXTEntry("MC_AR_PROP_13");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_xs)
                return Game.GetGXTEntry("MC_AR_PROP_14");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_l)
                return Game.GetGXTEntry("MC_AR_PROP4_09");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_m)
                return Game.GetGXTEntry("MC_AR_PROP4_10");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_s)
                return Game.GetGXTEntry("MC_AR_PROP4_12");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_xs)
                return Game.GetGXTEntry("MC_AR_PROP4_14");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_xxs)
                return Game.GetGXTEntry("MC_AR_PROP4_13");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_crn)
                return Game.GetGXTEntry("MC_AR_PROP4_00");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_4x_gap_02)
                return Game.GetGXTEntry("MC_AR_PROP_39");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_l)
                return Game.GetGXTEntry("MC_AR_PROP2_09");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_m)
                return Game.GetGXTEntry("MC_AR_PROP2_10");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_s)
                return Game.GetGXTEntry("MC_AR_PROP2_12");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_xs)
                return Game.GetGXTEntry("MC_AR_PROP2_14");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_xxs)
                return Game.GetGXTEntry("MC_AR_PROP2_13");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_crn)
                return Game.GetGXTEntry("MC_AR_PROP2_00");
            if (iParam0 == (int)RacingProps.ar_prop_ar_tube_2x_gap_02)
                return Game.GetGXTEntry("MC_AR_PROP_40");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_crn)
                return Game.GetGXTEntry("MC_AR_PROP_15");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_crn_15d)
                return Game.GetGXTEntry("MC_AR_PROP_16");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_crn_30d)
                return Game.GetGXTEntry("MC_AR_PROP_17");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_crn02)
                return Game.GetGXTEntry("MC_AR_PROP_18");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_fork)
                return Game.GetGXTEntry("MC_AR_PROP_19");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_xxs)
                return Game.GetGXTEntry("MC_AR_PROP_20");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_xs)
                return Game.GetGXTEntry("MC_AR_PROP_21");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_s)
                return Game.GetGXTEntry("MC_AR_PROP_22");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_m)
                return Game.GetGXTEntry("MC_AR_PROP_23");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoint_l)
                return Game.GetGXTEntry("MC_AR_PROP_24");
            if (iParam0 == (int)RacingProps.ar_prop_ar_checkpoints_crn_5d)
                return Game.GetGXTEntry("MC_AR_PROP_25");
            if (iParam0 == (int)RacingProps.ar_prop_ar_neon_gate_01a)
                return Game.GetGXTEntry("MC_AR_PROP_26");
            if (iParam0 == (int)RacingProps.ar_prop_ar_neon_gate_01b)
                return Game.GetGXTEntry("MC_AR_PROP_26B");
            if (iParam0 == (int)RacingProps.ar_prop_ar_neon_gate_02a)
                return Game.GetGXTEntry("MC_AR_PROP_27");
            if (iParam0 == (int)RacingProps.ar_prop_ar_neon_gate_02b)
                return Game.GetGXTEntry("MC_AR_PROP_27B");
            if (iParam0 == (int)RacingProps.ar_prop_ar_neon_gate_03a)
                return Game.GetGXTEntry("MC_AR_PROP_28");
            if (iParam0 == (int)RacingProps.ar_prop_ar_neon_gate_04a)
                return Game.GetGXTEntry("MC_AR_PROP_29");
            if (iParam0 == (int)RacingProps.ar_prop_ar_neon_gate_05a)
                return Game.GetGXTEntry("MC_AR_PROP_30");
            if (iParam0 == (int)RacingProps.ar_prop_ar_start_01a)
                return Game.GetGXTEntry("MC_AR_PROP_31");
            if (iParam0 == (int)RacingProps.ar_prop_inflategates_cp)
                return Game.GetGXTEntry("MC_AR_PROP_37");
            if (iParam0 == (int)RacingProps.ar_prop_inflategates_cp_h1)
                return Game.GetGXTEntry("MC_AR_PROP_41");
            if (iParam0 == (int)RacingProps.ar_prop_inflategates_cp_h2)
                return Game.GetGXTEntry("MC_AR_PROP_42");
            if (iParam0 == (int)RacingProps.ar_prop_inflategates_cp_loop)
                return Game.GetGXTEntry("MC_AR_PROP_79");
            if (iParam0 == (int)RacingProps.ar_prop_inflategates_cp_loop_h1)
                return Game.GetGXTEntry("MC_AR_PROP_80");
            if (iParam0 == (int)RacingProps.ar_prop_inflategates_cp_loop_h2)
                return Game.GetGXTEntry("MC_AR_PROP_81");
            if (iParam0 == (int)RacingProps.ar_prop_inflategates_cp_loop_01a)
                return Game.GetGXTEntry("MC_AR_PROP_82");
            if (iParam0 == (int)RacingProps.ar_prop_inflategates_cp_loop_01b)
                return Game.GetGXTEntry("MC_AR_PROP_83");
            if (iParam0 == (int)RacingProps.ar_prop_inflategates_cp_loop_01c)
                return Game.GetGXTEntry("MC_AR_PROP_84");
            if (iParam0 == (int)RacingProps.ar_prop_gate_cp_90d)
                return Game.GetGXTEntry("MC_AR_PROP_91");
            if (iParam0 == (int)RacingProps.ar_prop_gate_cp_90d_h1)
                return Game.GetGXTEntry("MC_AR_PROP_92");
            if (iParam0 == (int)RacingProps.ar_prop_gate_cp_90d_h2)
                return Game.GetGXTEntry("MC_AR_PROP_93");
            if (iParam0 == (int)RacingProps.ar_prop_gate_cp_90d_01a)
                return Game.GetGXTEntry("MC_AR_PROP_94");
            if (iParam0 == (int)RacingProps.ar_prop_gate_cp_90d_01b)
                return Game.GetGXTEntry("MC_AR_PROP_95");
            if (iParam0 == (int)RacingProps.ar_prop_gate_cp_90d_01c)
                return Game.GetGXTEntry("MC_AR_PROP_96");
            if (iParam0 == (int)RacingProps.ar_prop_ar_speed_ring)
                return Game.GetGXTEntry("MC_AR_PROP_57");
            if (iParam0 == (int)RacingProps.ar_prop_ar_jump_loop)
                return Game.GetGXTEntry("MC_AR_PROP_58");
            if (iParam0 == (int)RacingProps.sm_prop_smug_cont_01a)
                return Game.GetGXTEntry("MC_AR_PROP_59");
            if (iParam0 == (int)RacingProps.ar_prop_ig_sprunk_cp_b)
                return Game.GetGXTEntry("MC_AR_PROP_66");
            if (iParam0 == (int)RacingProps.ar_prop_ig_raine_cp_b)
                return Game.GetGXTEntry("MC_AR_PROP_67");
            if (iParam0 == (int)RacingProps.ar_prop_ig_flow_cp_b)
                return Game.GetGXTEntry("MC_AR_PROP_68");
            if (iParam0 == (int)RacingProps.ar_prop_ig_shark_cp_b)
                return Game.GetGXTEntry("MC_AR_PROP_69");
            if (iParam0 == (int)RacingProps.ar_prop_ig_jackal_cp_b)
                return Game.GetGXTEntry("MC_AR_PROP_70");
            if (iParam0 == (int)RacingProps.ar_prop_ig_metv_cp_b)
                return Game.GetGXTEntry("MC_AR_PROP_71");
            if (iParam0 == (int)RacingProps.ar_prop_ig_metv_cp_single)
                return Game.GetGXTEntry("MC_AR_PROP_72");
            if (iParam0 == (int)RacingProps.ar_prop_ig_jackal_cp_single)
                return Game.GetGXTEntry("MC_AR_PROP_73");
            if (iParam0 == (int)RacingProps.ar_prop_ig_shark_cp_single)
                return Game.GetGXTEntry("MC_AR_PROP_74");
            if (iParam0 == (int)RacingProps.ar_prop_ig_flow_cp_single)
                return Game.GetGXTEntry("MC_AR_PROP_75");
            if (iParam0 == (int)RacingProps.ar_prop_ig_raine_cp_single)
                return Game.GetGXTEntry("MC_AR_PROP_76");
            if (iParam0 == (int)RacingProps.ar_prop_ig_sprunk_cp_single)
                return Game.GetGXTEntry("MC_AR_PROP_77");
            if (iParam0 == (int)RacingProps.xm_prop_x17_mine_01a)
                return Game.GetGXTEntry("FMMC_OM_MN1");
            if (iParam0 == (int)RacingProps.xm_prop_x17_mine_02a)
                return Game.GetGXTEntry("FMMC_OM_MN2");
            if (iParam0 == (int)RacingProps.xm_prop_x17_mine_03a)
                return Game.GetGXTEntry("FMMC_OM_MN3");
            if (func_8935())
            {
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_crn)
                {
                    return Game.GetGXTEntry("MC_DO_STNT46");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_crn2)
                {
                    return Game.GetGXTEntry("MC_DO_STNT102");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_cross)
                {
                    return Game.GetGXTEntry("MC_DO_STNT40");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_gap_01)
                {
                    return Game.GetGXTEntry("MC_DO_STNT165");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_gap_02)
                {
                    return Game.GetGXTEntry("MC_DO_STNT166");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_gap_03)
                {
                    return Game.GetGXTEntry("MC_DO_STNT262");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_xxs)
                {
                    return Game.GetGXTEntry("MC_DO_STNT104");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_xs)
                {
                    return Game.GetGXTEntry("MC_DO_STNT37");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_s)
                {
                    return Game.GetGXTEntry("MC_DO_STNT38");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_m)
                {
                    return Game.GetGXTEntry("MC_DO_STNT39");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_l)
                {
                    return Game.GetGXTEntry("MC_DO_STNT100");
                }
                if (iParam0 == (int)RacingProps.p_ld_stinger_s)
                {
                    return Game.GetGXTEntry("FMMC_PR_STING");
                }
                if (iParam0 == (int)RacingProps.prop_wall_light_09a)
                {
                    return Game.GetGXTEntry("FMMC_BB_LGT");
                }
                if (iParam0 == (int)RacingProps.ba_prop_battle_emis_rig_01)
                {
                    return Game.GetGXTEntry("FMMC_LIG_MN1");
                }
                if (iParam0 == (int)RacingProps.ba_prop_battle_emis_rig_02)
                {
                    return Game.GetGXTEntry("FMMC_LIG_MN2");
                }
                if (iParam0 == (int)RacingProps.ba_prop_battle_emis_rig_03)
                {
                    return Game.GetGXTEntry("FMMC_LIG_MN3");
                }
                if (iParam0 == (int)RacingProps.ba_prop_battle_emis_rig_04)
                {
                    return Game.GetGXTEntry("FMMC_LIG_MN4");
                }
                if (iParam0 == (int)RacingProps.vw_prop_vw_elecbox_01a)
                {
                    return Game.GetGXTEntry("FMMC_P_ELCB");
                }
                if (iParam0 == (int)RacingProps.vw_prop_vw_radiomast_01a)
                {
                    return Game.GetGXTEntry("FMMC_P_RDTW");
                }
            }
            else
            {
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_crn)
                {
                    return Game.GetGXTEntry("MC_PR_STNT46");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_crn2)
                {
                    return Game.GetGXTEntry("MC_PR_STNT102");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_cross)
                {
                    return Game.GetGXTEntry("MC_PR_STNT40");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_gap_01)
                {
                    return Game.GetGXTEntry("MC_PR_STNT165");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_gap_02)
                {
                    return Game.GetGXTEntry("MC_PR_STNT166");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_gap_03)
                {
                    return Game.GetGXTEntry("MC_PR_STNT262");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_xxs)
                {
                    return Game.GetGXTEntry("MC_PR_STNT104");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_xs)
                {
                    return Game.GetGXTEntry("MC_PR_STNT37");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_s)
                {
                    return Game.GetGXTEntry("MC_PR_STNT38");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_m)
                {
                    return Game.GetGXTEntry("MC_PR_STNT39");
                }
                if (iParam0 == (int)RacingProps.bkr_prop_biker_tube_l)
                {
                    return Game.GetGXTEntry("MC_PR_STNT100");
                }
            }
            if (iParam0 == (int)RacingProps.prop_crate_11e)
                return Game.GetGXTEntry("FMMC_BB_OTC");
            if (iParam0 == (int)RacingProps.v_ret_ml_beerpat1)
                return Game.GetGXTEntry("FMMC_BB_PC");
            if (iParam0 == (int)RacingProps.v_ret_ml_beerpis2)
                return Game.GetGXTEntry("FMMC_BB_PWC");
            if (iParam0 == (int)RacingProps.ba_prop_battle_crate_beer_01)
                return Game.GetGXTEntry("FMMC_BB_POC1");
            if (iParam0 == (int)RacingProps.ba_prop_battle_crate_beer_02)
                return Game.GetGXTEntry("FMMC_BB_POC2");
            if (iParam0 == (int)RacingProps.ba_prop_battle_crate_beer_03)
                return Game.GetGXTEntry("FMMC_BB_POC3");
            if (iParam0 == (int)RacingProps.ba_prop_battle_crate_beer_04)
                return Game.GetGXTEntry("FMMC_BB_POC4");
            if (iParam0 == (int)RacingProps.prop_dyn_pc)
                return Game.GetGXTEntry("FMMC_BB_PCU");
            if (iParam0 == (int)RacingProps.prop_dummy_01)
                return Game.GetGXTEntry("FMMC_BB_CPM");
            if (iParam0 == (int)RacingProps.prop_ped_gib_01)
                return Game.GetGXTEntry("FMMC_BB_DED");
            if (iParam0 == (int)RacingProps.vw_prop_vw_valet_01a)
                return Game.GetGXTEntry("FMMC_BB_VAL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_01a)
                return Game.GetGXTEntry("ARNAT_FREPT");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_02a)
                return Game.GetGXTEntry("ARNAT_FREPTS");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_03a)
                return Game.GetGXTEntry("ARNAT_FREPTL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_04a)
                return Game.GetGXTEntry("ARNAT_FREPTXL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_flipper_small_01a)
                return Game.GetGXTEntry("ARNAT_SMFLIP");
            if (iParam0 == (int)RacingProps.xs_prop_arena_flipper_large_01a)
                return Game.GetGXTEntry("ARNAT_LGFLIP");
            if (iParam0 == (int)RacingProps.xs_prop_arena_flipper_xl_01a)
                return Game.GetGXTEntry("ARNAT_XLFLIP");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_rising_01a)
                return Game.GetGXTEntry("ARNAT_WALLSA");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_rising_02a)
                return Game.GetGXTEntry("ARNAT_WALLSXL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bollard_side_01a)
                return Game.GetGXTEntry("ARNAT_SDBLD");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bollard_rising_01a)
                return Game.GetGXTEntry("ARNAT_BOLLRD");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bollard_rising_01b)
                return Game.GetGXTEntry("ARNAT_BLDRW");
            if (iParam0 == (int)RacingProps.xs_prop_arena_spikes_01a)
                return Game.GetGXTEntry("ARNAT_TYSPK");
            if (iParam0 == (int)RacingProps.xs_prop_arena_landmine_01a)
                return Game.GetGXTEntry("ARNAT_LNDMN");
            if (iParam0 == (int)RacingProps.xs_prop_arena_landmine_03a)
                return Game.GetGXTEntry("ARNAT_SEAMN");
            if (iParam0 == (int)RacingProps.xs_prop_arena_barrel_01a)
                return Game.GetGXTEntry("ARNAT_BRBMB");
            if (iParam0 == (int)RacingProps.xs_prop_arena_landmine_01c)
                return Game.GetGXTEntry("ARNAT_LNDMNXL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_01a)
                return Game.GetGXTEntry("ARNAT_TRNTBA");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_b_01a)
                return Game.GetGXTEntry("ARNAT_TRNTB_BS");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_02a)
                return Game.GetGXTEntry("ARNAT_TRNTBB");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_03a)
                return Game.GetGXTEntry("ARNAT_TRNTBC");
            if (iParam0 == -1255963777)
                return Game.GetGXTEntry("ARNAT_TRNTXL");
            if (iParam0 == 11714146)
                return Game.GetGXTEntry("ARNAT_TRNTXLB");
            if (iParam0 == 620156114)
                return Game.GetGXTEntry("ARNAT_DBPIT_A");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_double_01b)
                return Game.GetGXTEntry("ARNAT_DBPIT_B");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_01a_sf)
                return Game.GetGXTEntry("ARNAT_FREPT");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_02a_sf)
                return Game.GetGXTEntry("ARNAT_FREPTS");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_03a_sf)
                return Game.GetGXTEntry("ARNAT_FREPTL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_04a_sf)
                return Game.GetGXTEntry("ARNAT_FREPTXL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bollard_side_01a_sf)
                return Game.GetGXTEntry("ARNAT_SDBLD");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bollard_rising_01a_sf)
                return Game.GetGXTEntry("ARNAT_BOLLRD");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bollard_rising_01b_sf)
                return Game.GetGXTEntry("ARNAT_BLDRW");
            if (iParam0 == (int)RacingProps.xs_prop_arena_landmine_01c_sf)
                return Game.GetGXTEntry("ARNAT_LNDMN_SF");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_rising_01a_sf)
                return Game.GetGXTEntry("ARNAT_WALLSA");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_rising_02a_sf)
                return Game.GetGXTEntry("ARNAT_WALLSXL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_landmine_03a_sf)
                return Game.GetGXTEntry("ARNAT_SEAMN");
            if (iParam0 == (int)RacingProps.xs_prop_arena_barrel_01a_sf)
                return Game.GetGXTEntry("ARNAT_BRBMB");
            if (iParam0 == (int)RacingProps.xs_prop_arena_flipper_small_01a_sf)
                return Game.GetGXTEntry("ARNAT_SMFLIP");
            if (iParam0 == (int)RacingProps.xs_prop_arena_flipper_large_01a_sf)
                return Game.GetGXTEntry("ARNAT_LGFLIP");
            if (iParam0 == (int)RacingProps.xs_prop_arena_flipper_xl_01a_sf)
                return Game.GetGXTEntry("ARNAT_XLFLIP");
            if (iParam0 == 1076615480)
                return Game.GetGXTEntry("ARNAT_TRNTBA");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_double_01a_sf)
                return Game.GetGXTEntry("ARNAT_DBPIT_A");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_double_01b_sf)
                return Game.GetGXTEntry("ARNAT_DBPIT_B");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_01a_sf)
                return Game.GetGXTEntry("ARNAT_TRNTBA");
            if (iParam0 == (int)RacingProps.vw_prop_arena_turntable_02f_sf)
                return Game.GetGXTEntry("ARNAT_TRNTBB");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_03a_sf)
                return Game.GetGXTEntry("ARNAT_TRNTBC");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_b_01a_sf)
                return Game.GetGXTEntry("ARNAT_TRNTB_BS");
            if (iParam0 == (int)RacingProps.xs_prop_arena_spikes_01a_sf)
                return Game.GetGXTEntry("ARNAT_S_TYSPK");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_01a_wl)
                return Game.GetGXTEntry("ARNAT_FREPT");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_02a_wl)
                return Game.GetGXTEntry("ARNAT_FREPTS");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_03a_wl)
                return Game.GetGXTEntry("ARNAT_FREPTL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_fire_04a_wl)
                return Game.GetGXTEntry("ARNAT_FREPTXL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bollard_side_01a_wl)
                return Game.GetGXTEntry("ARNAT_SDBLD");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bollard_rising_01a_wl)
                return Game.GetGXTEntry("ARNAT_BOLLRD");
            if (iParam0 == (int)RacingProps.xs_prop_arena_bollard_rising_01b_wl)
                return Game.GetGXTEntry("ARNAT_BLDRW");
            if (iParam0 == -2003814848)
                return Game.GetGXTEntry("ARNAT_DBPIT");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_01a_wl)
                return Game.GetGXTEntry("ARNAT_TRNTBA");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_02a_wl)
                return Game.GetGXTEntry("ARNAT_TRNTBB");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_03a_wl)
                return Game.GetGXTEntry("ARNAT_TRNTBC");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turntable_b_01a_wl)
                return Game.GetGXTEntry("ARNAT_TRNTB_BS");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_rising_01a_wl)
                return Game.GetGXTEntry("ARNAT_WALLSA");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_rising_02a_wl)
                return Game.GetGXTEntry("ARNAT_WALLSXL");
            if (iParam0 == (int)RacingProps.xs_prop_arena_flipper_small_01a_wl)
                return Game.GetGXTEntry("ARNAT_SMFLIP");
            if (iParam0 == (int)RacingProps.xs_prop_arena_flipper_large_01a_wl)
                return Game.GetGXTEntry("ARNAT_LGFLIP");
            if (iParam0 == (int)RacingProps.xs_prop_arena_flipper_xl_01a_wl)
                return Game.GetGXTEntry("ARNAT_XLFLIP");
            if (iParam0 == (int)RacingProps.xs_prop_arena_landmine_01c_wl)
                return Game.GetGXTEntry("ARNAT_LNDMN_W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_landmine_03a_wl)
                return Game.GetGXTEntry("ARNAT_SEAMN");
            if (iParam0 == (int)RacingProps.xs_prop_arena_barrel_01a_wl)
                return Game.GetGXTEntry("ARNAT_BRBMB");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_double_01a_wl)
                return Game.GetGXTEntry("ARNAT_DBPIT_A");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pit_double_01b_wl)
                return Game.GetGXTEntry("ARNAT_DBPIT_B");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pressure_plate_01a)
                return Game.GetGXTEntry("ARNAP_PRPAD");
            if (iParam0 == (int)RacingProps.xs_prop_arena_car_wall_01a)
                return Game.GetGXTEntry("ARNAP_CRWL_A");
            if (iParam0 == (int)RacingProps.xs_prop_arena_car_wall_02a)
                return Game.GetGXTEntry("ARNAP_CRWL_B");
            if (iParam0 == (int)RacingProps.xs_prop_arena_car_wall_03a)
                return Game.GetGXTEntry("ARNAP_CRWL_C");
            if (iParam0 == (int)RacingProps.xs_prop_arena_station_01a)
                return Game.GetGXTEntry("ARNAP_BUILD_A");
            if (iParam0 == (int)RacingProps.xs_prop_arena_station_02a)
                return Game.GetGXTEntry("ARNAP_BUILD_B");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wedge_01a)
                return Game.GetGXTEntry("ARNAP_WEDGE");
            if (iParam0 == (int)RacingProps.xs_prop_arena_oil_jack_01a)
                return Game.GetGXTEntry("ARNAP_PR_001");
            if (iParam0 == (int)RacingProps.xs_prop_arena_oil_jack_02a)
                return Game.GetGXTEntry("ARNAP_PR_002");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_01c)
                return Game.GetGXTEntry("ARNAP_PR_003");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_01b)
                return Game.GetGXTEntry("ARNAP_PR_004");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_01a)
                return Game.GetGXTEntry("ARNAP_PR_005");
            if (iParam0 == (int)RacingProps.xs_prop_arena_building_01a)
                return Game.GetGXTEntry("ARNAP_PR_032");
            if (iParam0 == (int)RacingProps.xs_prop_arena_industrial_a)
                return Game.GetGXTEntry("ARNAP_PR_006");
            if (iParam0 == (int)RacingProps.xs_prop_arena_industrial_b)
                return Game.GetGXTEntry("ARNAP_PR_007");
            if (iParam0 == (int)RacingProps.xs_prop_arena_industrial_c)
                return Game.GetGXTEntry("ARNAP_PR_008");
            if (iParam0 == (int)RacingProps.xs_prop_arena_industrial_d)
                return Game.GetGXTEntry("ARNAP_PR_009");
            if (iParam0 == (int)RacingProps.xs_prop_arena_industrial_e)
                return Game.GetGXTEntry("ARNAP_PR_010");
            if (iParam0 == (int)RacingProps.xs_prop_arena_1bay_01a)
                return Game.GetGXTEntry("ARNAP_PR_011");
            if (iParam0 == (int)RacingProps.xs_prop_arena_2bay_01a)
                return Game.GetGXTEntry("ARNAP_PR_012");
            if (iParam0 == (int)RacingProps.xs_prop_arena_3bay_01a)
                return Game.GetGXTEntry("ARNAP_PR_013");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_xs_01a)
                return Game.GetGXTEntry("ARNAP_PR_014");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_s_01a)
                return Game.GetGXTEntry("ARNAP_PR_015");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_m_01a)
                return Game.GetGXTEntry("ARNAP_PR_016");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_l_01a)
                return Game.GetGXTEntry("ARNAP_PR_017");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_xl_01a)
                return Game.GetGXTEntry("ARNAP_PR_018");
            if (iParam0 == (int)RacingProps.xs_prop_arena_adj_hloop)
                return Game.GetGXTEntry("ARNAP_PR_033");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_02b)
                return Game.GetGXTEntry("ARNAP_PR_TJ2");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_xs_01a_wl)
                return Game.GetGXTEntry("ARNAP_PR_014W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_s_01a_wl)
                return Game.GetGXTEntry("ARNAP_PR_015W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_m_01a_wl)
                return Game.GetGXTEntry("ARNAP_PR_016W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_l_01a_wl)
                return Game.GetGXTEntry("ARNAP_PR_017W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_xl_01a_wl)
                return Game.GetGXTEntry("ARNAP_PR_018W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_xxl_01a_wl)
                return Game.GetGXTEntry("ARNAP_PR_019W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_adj_hloop_wl)
                return Game.GetGXTEntry("ARNAP_PR_033W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_xs_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_014SF");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_s_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_015SF");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_m_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_016SF");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_l_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_017SF");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_xl_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_018SF");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_xxl_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_019SF");
            if (iParam0 == (int)RacingProps.xs_prop_arena_adj_hloop_sf)
                return Game.GetGXTEntry("ARNAP_PR_033SF");
            if (iParam0 == (int)RacingProps.xs_prop_arena_jump_xxl_01a)
                return Game.GetGXTEntry("ARNAP_PR_019");
            if (iParam0 == (int)RacingProps.xs_prop_arena_goal)
                return Game.GetGXTEntry("ARNAP_PR_020");
            if (iParam0 == (int)RacingProps.xs_prop_arena_gate_01a)
                return Game.GetGXTEntry("ARNAP_PR_021");
            if (iParam0 == (int)RacingProps.xs_prop_arena_tower_01a)
                return Game.GetGXTEntry("ARNAP_PR_022");
            if (iParam0 == (int)RacingProps.xs_prop_arena_tower_02a)
                return Game.GetGXTEntry("ARNAP_PR_023");
            if (iParam0 == (int)RacingProps.xs_prop_arena_startgate_01a)
                return Game.GetGXTEntry("ARNAP_PR_024");
            if (iParam0 == (int)RacingProps.xs_prop_arena_arrow_01a)
                return Game.GetGXTEntry("ARNAP_PR_025");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_02a)
                return Game.GetGXTEntry("ARNAP_PR_026");
            if (iParam0 == (int)RacingProps.xs_prop_arena_tower_04a)
                return Game.GetGXTEntry("ARNAP_PR_027");
            if (iParam0 == (int)RacingProps.xs_prop_wall_tyre_01a)
                return Game.GetGXTEntry("ARNAP_PR_028");
            if (iParam0 == (int)RacingProps.xs_prop_wall_tyre_l_01a)
                return Game.GetGXTEntry("ARNAP_PR_029");
            if (iParam0 == (int)RacingProps.xs_prop_wall_tyre_start_01a)
                return Game.GetGXTEntry("ARNAP_PR_030");
            if (iParam0 == (int)RacingProps.xs_prop_wall_tyre_end_01a)
                return Game.GetGXTEntry("ARNAP_PR_031");
            if (iParam0 == (int)RacingProps.xs_prop_arrow_tyre_01a)
                return Game.GetGXTEntry("ARNAP_PR_ARTY_A");
            if (iParam0 == (int)RacingProps.xs_prop_arrow_tyre_01b)
                return Game.GetGXTEntry("ARNAP_PR_ARTY_B");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turret_01a)
                return Game.GetGXTEntry("ARNAP_PR_TWR_D");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turret_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_TWR_S");
            if (iParam0 == (int)RacingProps.xs_prop_arena_turret_01a_wl)
                return Game.GetGXTEntry("ARNAP_PR_TWR_W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_finish_line)
                return Game.GetGXTEntry("ARNAP_PR_FNSH");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pressure_plate_01a_sf)
                return Game.GetGXTEntry("ARNAP_PRPAD");
            if (iParam0 == -24212519)
                return Game.GetGXTEntry("ARNAP_SF_001");
            if (iParam0 == 602461837)
                return Game.GetGXTEntry("ARNAP_SF_002");
            if (iParam0 == -7893553)
                return Game.GetGXTEntry("ARNAP_SF_003");
            if (iParam0 == 1164711295)
                return Game.GetGXTEntry("ARNAP_SF_004");
            if (iParam0 == 1600014691)
                return Game.GetGXTEntry("ARNAP_SF_005");
            if (iParam0 == -1570254564)
                return Game.GetGXTEntry("ARNAP_SF_006");
            if (iParam0 == -280401182)
                return Game.GetGXTEntry("ARNAP_SF_007");
            if (iParam0 == 1362955823)
                return Game.GetGXTEntry("ARNAP_SF_008");
            if (iParam0 == 2081645531)
                return Game.GetGXTEntry("ARNAP_SF_009");
            if (iParam0 == -76356964)
                return Game.GetGXTEntry("ARNAP_SF_010");
            if (iParam0 == -312654223)
                return Game.GetGXTEntry("ARNAP_SF_011");
            if (iParam0 == -1728275023)
                return Game.GetGXTEntry("ARNAP_SF_012");
            if (iParam0 == -739296408)
                return Game.GetGXTEntry("ARNAP_SF_013");
            if (iParam0 == 559478677)
                return Game.GetGXTEntry("ARNAP_SF_014");
            if (iParam0 == 301586647)
                return Game.GetGXTEntry("ARNAP_SF_015");
            if (iParam0 == -992952702)
                return Game.GetGXTEntry("ARNAP_SF_016");
            if (iParam0 == 654168077)
                return Game.GetGXTEntry("ARNAP_SF_017S");
            if (iParam0 == -2032906695)
                return Game.GetGXTEntry("ARNAP_SF_019");
            if (iParam0 == -1671722781)
                return Game.GetGXTEntry("ARNAP_SF_020");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wedge_01a_sf)
                return Game.GetGXTEntry("ARNAP_WEDGE");
            if (iParam0 == (int)RacingProps.xs_prop_arena_goal_sf)
                return Game.GetGXTEntry("ARNAP_PR_020S");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_02a_sf)
                return Game.GetGXTEntry("ARNAP_PR_003");
            if (iParam0 == (int)RacingProps.xs_prop_arrow_tyre_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_ARTY_A");
            if (iParam0 == (int)RacingProps.xs_prop_arrow_tyre_01b_sf)
                return Game.GetGXTEntry("ARNAP_PR_ARTY_B");
            if (iParam0 == -1154284456)
                return Game.GetGXTEntry("ARNAP_PR_FLBA_A");
            if (iParam0 == 148271330)
                return Game.GetGXTEntry("ARNAP_PR_FLBA_B");
            if (iParam0 == (int)RacingProps.xs_prop_arena_arrow_01a_sf)
                return Game.GetGXTEntry("ARNAP_AN_ARRW");
            if (iParam0 == (int)RacingProps.xs_prop_arena_startgate_01a_sf)
                return Game.GetGXTEntry("ARNAP_AN_SFSG");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_s_45a_sf)
                return Game.GetGXTEntry("AP_SF_PLTS45");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_s_90a_sf)
                return Game.GetGXTEntry("AP_SF_PLTS90");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_s_180a_sf)
                return Game.GetGXTEntry("AP_SF_PLTS180");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_m_30a_sf)
                return Game.GetGXTEntry("AP_SF_PLTM30A");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_m_30b_sf)
                return Game.GetGXTEntry("AP_SF_PLTM30B");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_m_60a_sf)
                return Game.GetGXTEntry("AP_SF_PLTM60A");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_m_60b_sf)
                return Game.GetGXTEntry("AP_SF_PLTM60B");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_m_90a_sf)
                return Game.GetGXTEntry("AP_SF_PLTM90");
            if (iParam0 == (int)RacingProps.xs_prop_ar_tower_01a_sf)
                return Game.GetGXTEntry("AP_SF_TWR");
            if (iParam0 == (int)RacingProps.xs_prop_ar_pipe_conn_01a_sf)
                return Game.GetGXTEntry("AP_SF_PPC");
            if (iParam0 == (int)RacingProps.xs_prop_ar_stand_thick_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_STND");
            if (iParam0 == (int)RacingProps.xs_prop_ar_pipe_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_PP");
            if (iParam0 == (int)RacingProps.xs_prop_ar_gate_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_GT");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_xl_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_XLPLT");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_m_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_MPLT");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_s_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_SPLT");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_c_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_CRNRA");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_c_02a_sf)
                return Game.GetGXTEntry("ARNAP_PR_CRNRB");
            if (iParam0 == (int)RacingProps.xs_prop_ar_planter_c_03a_sf)
                return Game.GetGXTEntry("ARNAP_PR_CNRC");
            if (iParam0 == (int)RacingProps.xs_prop_ar_tunnel_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_TUNNN");
            if (iParam0 == (int)RacingProps.xs_prop_ar_buildingx_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_BLDX");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_2)
                return Game.GetGXTEntry("FMMC_PR_119");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_3)
                return Game.GetGXTEntry("FMMC_PR_120");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_5)
                return Game.GetGXTEntry("FMMC_PR_121");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_7)
                return Game.GetGXTEntry("FMMC_PR_122");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_8)
                return Game.GetGXTEntry("FMMC_PR_123");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_10)
                return Game.GetGXTEntry("FMMC_PR_124");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_15)
                return Game.GetGXTEntry("FMMC_PR_125");
            if (iParam0 == (int)RacingProps.prop_rub_carwreck_16)
                return Game.GetGXTEntry("FMMC_PR_126");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pressure_plate_01a_wl)
                return Game.GetGXTEntry("ARNAP_PRPAD");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_track_c_01a)
                return Game.GetGXTEntry("ARNAP_PIP_TA");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_track_c_01b)
                return Game.GetGXTEntry("ARNAP_PIP_TB");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_track_c_01c)
                return Game.GetGXTEntry("ARNAP_PIP_TC");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_track_c_01d)
                return Game.GetGXTEntry("ARNAP_PIP_TD");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_track_s_01a)
                return Game.GetGXTEntry("ARNAP_PIP_TE");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_track_s_01b)
                return Game.GetGXTEntry("ARNAP_PIP_TF");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_bend_01a)
                return Game.GetGXTEntry("ARNAP_MM_001");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_bend_01b)
                return Game.GetGXTEntry("ARNAP_MM_002");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_bend_01c)
                return Game.GetGXTEntry("ARNAP_MM_003");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_bend_02a)
                return Game.GetGXTEntry("ARNAP_MM_004");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_bend_02b)
                return Game.GetGXTEntry("ARNAP_MM_005");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_bend_02c)
                return Game.GetGXTEntry("ARNAP_MM_006");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_end_01a)
                return Game.GetGXTEntry("ARNAP_MM_007");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_end_02a)
                return Game.GetGXTEntry("ARNAP_MM_008");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_machine_01a)
                return Game.GetGXTEntry("ARNAP_MM_009");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_machine_02a)
                return Game.GetGXTEntry("ARNAP_MM_010");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_straight_01a)
                return Game.GetGXTEntry("ARNAP_MM_011");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_straight_01b)
                return Game.GetGXTEntry("ARNAP_MM_012");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_straight_02a)
                return Game.GetGXTEntry("ARNAP_MM_013");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_straight_02b)
                return Game.GetGXTEntry("ARNAP_MM_014");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_straight_02c)
                return Game.GetGXTEntry("ARNAP_MM_015");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_straight_02d)
                return Game.GetGXTEntry("ARNAP_MM_016");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_transition_01a)
                return Game.GetGXTEntry("ARNAP_MM_017");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_transition_01b)
                return Game.GetGXTEntry("ARNAP_MM_018");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_transition_01c)
                return Game.GetGXTEntry("ARNAP_MM_019");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_transition_02a)
                return Game.GetGXTEntry("ARNAP_MM_020");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_transition_02b)
                return Game.GetGXTEntry("ARNAP_MM_021");
            if (iParam0 == (int)RacingProps.xs_prop_arena_pipe_ramp_01a)
                return Game.GetGXTEntry("ARNAP_MM_022");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wedge_01a_wl)
                return Game.GetGXTEntry("ARNAP_WEDGE");
            if (iParam0 == -296431790)
                return Game.GetGXTEntry("ARNAP_PR_020W");
            if (iParam0 == (int)RacingProps.xs_prop_arena_wall_02a_wl)
                return Game.GetGXTEntry("ARNAP_PR_003");
            if (iParam0 == (int)RacingProps.xs_prop_arrow_tyre_01a_wl)
                return Game.GetGXTEntry("ARNAP_PR_ARTY_A");
            if (iParam0 == (int)RacingProps.xs_prop_arrow_tyre_01b_wl)
                return Game.GetGXTEntry("ARNAP_PR_ARTY_B");
            if (iParam0 == -566149868)
                return Game.GetGXTEntry("ARNAP_PR_FLBA_A");
            if (iParam0 == -683942222)
                return Game.GetGXTEntry("ARNAP_PR_FLBA_B");
            if (iParam0 == (int)RacingProps.xs_prop_arena_arrow_01a_wl)
                return Game.GetGXTEntry("ARNAP_AN_ARRW");
            if (iParam0 == (int)RacingProps.xs_prop_arena_fence_01a)
                return Game.GetGXTEntry("ARNAP_PR_FNC_D");
            if (iParam0 == (int)RacingProps.xs_prop_arena_fence_01a_sf)
                return Game.GetGXTEntry("ARNAP_PR_FNC_S");
            if (iParam0 == (int)RacingProps.xs_prop_arena_fence_01a_wl)
                return Game.GetGXTEntry("ARNAP_PR_FNC_W");
            if (iParam0 == (int)RacingProps.xs_prop_barrier_5m_01a)
                return Game.GetGXTEntry("AP_WL_BR5");
            if (iParam0 == (int)RacingProps.xs_prop_barrier_10m_01a)
                return Game.GetGXTEntry("AP_WL_BR10");
            if (iParam0 == (int)RacingProps.xs_prop_barrier_15m_01a)
                return Game.GetGXTEntry("AP_WL_BR15");
            if (iParam0 == (int)RacingProps.xs_prop_ar_tunnel_01a)
                return Game.GetGXTEntry("AP_WL_TUNNL");
            if (iParam0 == (int)RacingProps.xs_prop_ar_tunnel_01a_wl)
                return Game.GetGXTEntry("AP_WL_TUNNL");
            if (iParam0 == (int)RacingProps.xs_prop_lplate_01a_wl)
                return Game.GetGXTEntry("AP_WL_LPT1");
            if (iParam0 == (int)RacingProps.xs_prop_lplate_bend_01a_wl)
                return Game.GetGXTEntry("AP_WL_LPT2");
            if (iParam0 == (int)RacingProps.xs_prop_lplate_wall_01a_wl)
                return Game.GetGXTEntry("AP_WL_LPT3");
            if (iParam0 == (int)RacingProps.xs_prop_lplate_wall_01b_wl)
                return Game.GetGXTEntry("AP_WL_LPT4");
            if (iParam0 == (int)RacingProps.xs_prop_lplate_wall_01c_wl)
                return Game.GetGXTEntry("AP_WL_LPT5");
            if (iParam0 == (int)RacingProps.xs_prop_beer_bottle_wl)
                return Game.GetGXTEntry("AP_WL_MSC1");
            if (iParam0 == (int)RacingProps.xs_prop_burger_meat_wl)
                return Game.GetGXTEntry("AP_WL_MSC2");
            if (iParam0 == (int)RacingProps.xs_prop_can_tunnel_wl)
                return Game.GetGXTEntry("AP_WL_MSC3");
            if (iParam0 == (int)RacingProps.xs_prop_can_wl)
                return Game.GetGXTEntry("AP_WL_MSC4");
            if (iParam0 == (int)RacingProps.xs_prop_chips_tube_wl)
                return Game.GetGXTEntry("AP_WL_MSC5");
            if (iParam0 == (int)RacingProps.xs_prop_chopstick_wl)
                return Game.GetGXTEntry("AP_WL_MSC6");
            if (iParam0 == (int)RacingProps.xs_prop_gate_tyre_01a_wl)
                return Game.GetGXTEntry("AP_WL_MSC7");
            if (iParam0 == (int)RacingProps.xs_prop_hamburgher_wl)
                return Game.GetGXTEntry("AP_WL_MSC8");
            if (iParam0 == (int)RacingProps.xs_prop_nacho_wl)
                return Game.GetGXTEntry("AP_WL_MSC9");
            if (iParam0 == (int)RacingProps.xs_prop_plastic_bottle_wl)
                return Game.GetGXTEntry("AP_WL_MSC10");
            if (iParam0 == (int)RacingProps.prop_wine_red)
                return Game.GetGXTEntry("FMMC_PR_127");
            if (iParam0 == (int)RacingProps.prop_drink_redwine)
                return Game.GetGXTEntry("FMMC_PR_128");
            if (iParam0 == (int)RacingProps.ch_prop_boring_machine_01a)
                return Game.GetGXTEntry("HEI3_PR_1");
            if (iParam0 == (int)RacingProps.ch_prop_boring_machine_01b)
                return Game.GetGXTEntry("HEI3_PR_2");
            if (iParam0 == (int)RacingProps.ch_prop_ch_casino_shutter01x)
                return Game.GetGXTEntry("HEI3_PR_3");
            if (iParam0 == (int)RacingProps.ch_prop_ch_cctv_wall_atta_01a)
                return Game.GetGXTEntry("HEI3_PR_4");
            if (iParam0 == (int)RacingProps.ch_prop_ch_tunnel_door01a)
                return Game.GetGXTEntry("HEI3_PR_5");
            if (iParam0 == (int)RacingProps.ch_prop_ch_room_trolly_01a)
                return Game.GetGXTEntry("HEI3_PR_6");
            if (iParam0 == (int)RacingProps.ch_prop_fingerprint_scanner_01a)
                return Game.GetGXTEntry("HEI3_PR_7");
            if (iParam0 == (int)RacingProps.ch_prop_fingerprint_scanner_01b)
                return Game.GetGXTEntry("HEI3_PR_8");
            if (iParam0 == (int)RacingProps.ch_prop_fingerprint_scanner_01c)
                return Game.GetGXTEntry("HEI3_PR_9");
            if (iParam0 == (int)RacingProps.ch_prop_fingerprint_scanner_01d)
                return Game.GetGXTEntry("HEI3_PR_10");
            if (iParam0 == (int)RacingProps.ch_prop_ch_sec_cabinet_02a)
                return Game.GetGXTEntry("HEI3_PR_11");
            if (iParam0 == (int)RacingProps.ch_prop_ch_trolly_01a)
                return Game.GetGXTEntry("HEI3_PR_12");
            if (iParam0 == (int)RacingProps.ch_prop_ch_service_trolley_01a)
                return Game.GetGXTEntry("HEI3_PR_13");
            if (iParam0 == (int)RacingProps.ch_prop_fingerprint_scanner_error_01b)
                return Game.GetGXTEntry("HEI3_PR_14");
            if (iParam0 == (int)RacingProps.ch_prop_ch_laundry_trolley_01a)
                return Game.GetGXTEntry("HEI3_PR_15");
            if (iParam0 == (int)RacingProps.ch_prop_ch_laundry_trolley_01b)
                return Game.GetGXTEntry("HEI3_PR_16");
            if (iParam0 == (int)RacingProps.v_corp_banktrolley)
                return Game.GetGXTEntry("HEI3_PR_17");
            if (iParam0 == (int)RacingProps.ch_prop_ch_maint_sign_01)
                return Game.GetGXTEntry("HEI3_PR_18");
            if (iParam0 == (int)RacingProps.sum_prop_ac_track_paddock_01)
                return Game.GetGXTEntry("MC_PR_STNT337");
            if (iParam0 == (int)RacingProps.ch_prop_track_pit_garage_01a)
                return Game.GetGXTEntry("MC_PR_STNT338D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_pit_garage_01a)
                return Game.GetGXTEntry("MC_PR_STNT338");
            if (iParam0 == (int)RacingProps.sum_prop_ac_grandstand_01a)
                return Game.GetGXTEntry("MC_PR_RGS");
            if (iParam0 == (int)RacingProps.sum_prop_track_pit_garage_02a)
                return Game.GetGXTEntry("MC_PR_GAR_2");
            if (iParam0 == (int)RacingProps.sum_prop_track_pit_garage_03a)
                return Game.GetGXTEntry("MC_PR_GAR_3");
            if (iParam0 == (int)RacingProps.sum_prop_track_pit_garage_04a)
                return Game.GetGXTEntry("MC_PR_GAR_4");
            if (iParam0 == (int)RacingProps.sum_prop_track_pit_garage_05a)
                return Game.GetGXTEntry("MC_PR_GAR_5");
            if (iParam0 == (int)RacingProps.sum_prop_race_barrier_01_sec)
                return Game.GetGXTEntry("MC_PR_BRSEC_1");
            if (iParam0 == (int)RacingProps.sum_prop_race_barrier_02_sec)
                return Game.GetGXTEntry("MC_PR_BRSEC_2");
            if (iParam0 == (int)RacingProps.sum_prop_race_barrier_04_sec)
                return Game.GetGXTEntry("MC_PR_BRSEC_4");
            if (iParam0 == (int)RacingProps.sum_prop_race_barrier_08_sec)
                return Game.GetGXTEntry("MC_PR_BRSEC_8");
            if (iParam0 == (int)RacingProps.sum_prop_race_barrier_16_sec)
                return Game.GetGXTEntry("MC_PR_BRSEC_16");
            if (iParam0 == (int)RacingProps.sum_prop_barrier_ac_bend_05d)
                return Game.GetGXTEntry("MC_PR_BRBND_5");
            if (iParam0 == (int)RacingProps.sum_prop_barrier_ac_bend_15d)
                return Game.GetGXTEntry("MC_PR_BRBND_15");
            if (iParam0 == (int)RacingProps.sum_prop_barrier_ac_bend_30d)
                return Game.GetGXTEntry("MC_PR_BRBND_30");
            if (iParam0 == (int)RacingProps.sum_prop_barrier_ac_bend_45d)
                return Game.GetGXTEntry("MC_PR_BRBND_45");
            if (iParam0 == (int)RacingProps.sum_prop_barrier_ac_bend_90d)
                return Game.GetGXTEntry("MC_PR_BRBND_90");
            if (iParam0 == (int)RacingProps.sum_prop_ac_long_barrier_05d)
                return Game.GetGXTEntry("MC_PR_BRSEC_5D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_long_barrier_15d)
                return Game.GetGXTEntry("MC_PR_BRSEC_15D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_long_barrier_30d)
                return Game.GetGXTEntry("MC_PR_BRSEC_30D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_long_barrier_45d)
                return Game.GetGXTEntry("MC_PR_BRSEC_45D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_long_barrier_90d)
                return Game.GetGXTEntry("MC_PR_BRSEC_90D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_short_barrier_05d)
                return Game.GetGXTEntry("MC_PR_BRSCT_5D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_short_barrier_15d)
                return Game.GetGXTEntry("MC_PR_BRSCT_15D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_short_barrier_30d)
                return Game.GetGXTEntry("MC_PR_BRSCT_30D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_short_barrier_45d)
                return Game.GetGXTEntry("MC_PR_BRSCT_45D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_short_barrier_90d)
                return Game.GetGXTEntry("MC_PR_BRSCT_90D");
            if (iParam0 == (int)RacingProps.sum_prop_ac_barge_01)
                return Game.GetGXTEntry("YCT_PR_BRG");
            if (iParam0 == (int)RacingProps.sum_prop_ac_ind_light_02a)
                return Game.GetGXTEntry("MC_PR_LGT1");
            if (iParam0 == (int)RacingProps.sum_prop_ac_ind_light_03c)
                return Game.GetGXTEntry("MC_PR_LGT2");
            if (iParam0 == (int)RacingProps.sum_prop_ac_ind_light_04)
                return Game.GetGXTEntry("MC_PR_LGT3");
            if (iParam0 == (int)RacingProps.sum_prop_ac_wall_light_09a)
                return Game.GetGXTEntry("FMMC_BB_LGTL");
            if (iParam0 == (int)RacingProps.ch_prop_ch_cctv_cam_01a)
                return Game.GetGXTEntry("FMMC_OBJ_CWA");
            if (iParam0 == (int)RacingProps.ch_prop_ch_cctv_cam_02a)
                return Game.GetGXTEntry("FMMC_OBJ_CWA2");
            if (iParam0 == (int)RacingProps.xm_prop_x17_server_farm_cctv_01)
                return Game.GetGXTEntry("FMMC_OBJ_CRL");
            if (iParam0 == (int)RacingProps.hei_prop_bank_cctv_01)
                return Game.GetGXTEntry("FMMC_OM_CCTV");
            if (iParam0 == (int)RacingProps.hei_prop_bank_cctv_02)
                return Game.GetGXTEntry("FMMC_OM_DOMC");
            if (iParam0 == (int)RacingProps.prop_cctv_cam_05a)
                return Game.GetGXTEntry("FMMC_OBJ_CWA3");
            if (iParam0 == 820707985)
                return Game.GetGXTEntry("MC_EX3D_2X1");
            if (iParam0 == 11747150)
                return Game.GetGXTEntry("MC_EX3D_2X3");
            if (iParam0 == 1444283207)
                return Game.GetGXTEntry("MC_EX3D_2X6");
            if (iParam0 == -664841990)
                return Game.GetGXTEntry("MC_EX3D_3X3Z");
            if (iParam0 == 1811127662)
                return Game.GetGXTEntry("MC_EX3D_3X3ZD");
            if (iParam0 == -2132967520)
                return Game.GetGXTEntry("MC_EX3D_6x10");
            if (iParam0 == -1730847084)
                return Game.GetGXTEntry("MC_EX3D_CJ");
            if (iParam0 == -37343207)
                return Game.GetGXTEntry("MC_EX3D_TJ");
            if (iParam0 == -1371067497)
                return Game.GetGXTEntry("MC_EX3D_LJR");
            if (iParam0 == 719882629)
                return Game.GetGXTEntry("MC_EX3D_LJL");
            if (iParam0 == 581945895)
                return Game.GetGXTEntry("MC_EX3D_LJLD");
            if (iParam0 == -1639321128)
                return Game.GetGXTEntry("MC_EX3D_3X3");
            if (iParam0 == 669536853)
                return Game.GetGXTEntry("MC_EX3D_4X4A");
            if (iParam0 == 1510880928)
                return Game.GetGXTEntry("MC_EX3D_4X4B");
            if (iParam0 == 1439575588)
                return Game.GetGXTEntry("MC_EX3D_4X4C");
            if (iParam0 == 1074266776)
                return Game.GetGXTEntry("MC_EX3D_4X4D");
            if (iParam0 == 1900799263)
                return Game.GetGXTEntry("MC_EX3D_4X4E");
            if (iParam0 == 1803344257)
                return Game.GetGXTEntry("MC_EX3D_4X4F");
            if (iParam0 == -1133355695)
                return Game.GetGXTEntry("MC_EX3D_6X6");
            if (iParam0 == -1955916202)
                return Game.GetGXTEntry("MC_EX3D_8X8");
            if (iParam0 == 251420015)
                return Game.GetGXTEntry("MC_EX3D_12");
            if (iParam0 == -451866472)
                return Game.GetGXTEntry("MC_EX3D_4X6");
            if (iParam0 == -654667398)
                return Game.GetGXTEntry("MC_EX3D_7X8");
            if (iParam0 == 896512151)
                return Game.GetGXTEntry("MC_EX3D_14X14");
            if (iParam0 == 940227121)
                return Game.GetGXTEntry("MC_EX3D_DG");
            if (iParam0 == -337605007)
                return Game.GetGXTEntry("MC_EX3D_W2M");
            if (iParam0 == 1281409573)
                return Game.GetGXTEntry("MC_EX3D_DRSW");
            if (iParam0 == (int)RacingProps.h4_prop_h4_boxpile_01a)
                return Game.GetGXTEntry("H4_PR_FLB");
            if (iParam0 == (int)RacingProps.h4_prop_h4_tannoy_01a)
                return Game.GetGXTEntry("FMMC_BB_CLSN");
            if (iParam0 == (int)RacingProps.h4_prop_h4_sign_cctv_01a)
                return Game.GetGXTEntry("H4_PR_CTV");
            if (iParam0 == (int)RacingProps.h4_prop_h4_sec_cabinet_dum)
                return Game.GetGXTEntry("H4_PR_PDM");
            if (iParam0 == (int)RacingProps.h4_prop_h4_sub_kos)
                return Game.GetGXTEntry("H4_PR_SUB");
            if (iParam0 == (int)RacingProps.h4_prop_office_desk_01)
                return Game.GetGXTEntry("H4_PR_OFDSK");
            if (iParam0 == (int)RacingProps.h4_prop_h4_fence_seg_x1_01a)
                return Game.GetGXTEntry("H4_PR_FS1");
            if (iParam0 == (int)RacingProps.h4_prop_h4_fence_seg_x3_01a)
                return Game.GetGXTEntry("H4_PR_FS3");
            if (iParam0 == (int)RacingProps.h4_prop_h4_fence_seg_x5_01a)
                return Game.GetGXTEntry("H4_PR_FS5");
            if (iParam0 == (int)RacingProps.h4_prop_h4_fence_arches_x2_01a)
                return Game.GetGXTEntry("H4_PR_FA2");
            if (iParam0 == (int)RacingProps.h4_prop_h4_fence_arches_x3_01a)
                return Game.GetGXTEntry("H4_PR_FA3");
            if (iParam0 == (int)RacingProps.h4_prop_h4_loch_monster)
                return Game.GetGXTEntry("H4_PR_LSM");
            if (iParam0 == (int)RacingProps.prop_box_wood02a_pu)
                return Game.GetGXTEntry("H4_PR_SCR");
            if (iParam0 == (int)RacingProps.h4_prop_h4_cctv_pole_04)
                return Game.GetGXTEntry("H4_PR_CTVP");
            if (iParam0 == (int)RacingProps.h4_prop_h4_t_bottle_02a)
                return Game.GetGXTEntry("H4_PR_GCLT1");
            if (iParam0 == (int)RacingProps.h4_prop_h4_neck_disp_01a)
                return Game.GetGXTEntry("H4_PR_GCLT2A");
            if (iParam0 == (int)RacingProps.h4_prop_h4_necklace_01a)
                return Game.GetGXTEntry("H4_PR_GCLT2B");
            if (iParam0 == (int)RacingProps.h4_prop_h4_art_pant_01a)
                return Game.GetGXTEntry("H4_PR_GCLT3");
            if (iParam0 == (int)RacingProps.h4_prop_h4_diamond_disp_01a)
                return Game.GetGXTEntry("H4_PR_GCLT4A");
            if (iParam0 == (int)RacingProps.h4_prop_h4_diamond_01a)
                return Game.GetGXTEntry("H4_PR_GCLT4B");
            return Game.GetGXTEntry("");
        }

        public static string GetCategoryName(int iParam0)
        {
            switch (iParam0)
            {
                case 0:
                    return Game.GetGXTEntry("FMMC_PLIB_0");

                case 1:
                    return Game.GetGXTEntry("FMMC_PLIB_1");

                case 2:
                    return Game.GetGXTEntry("FMMC_PLIB_2");

                case 3:
                    return Game.GetGXTEntry("FMMC_PLIB_3");

                case 4:
                    return Game.GetGXTEntry("FMMC_PLIB_4");

                case 5:
                    return Game.GetGXTEntry("FMMC_PLIB_5");

                case 6:
                    return Game.GetGXTEntry("FMMC_PLIB_6");

                case 7:
                    return Game.GetGXTEntry("FMMC_PLIB_7");

                case 8:
                    return Game.GetGXTEntry("FMMC_PLIB_8");

                case 9:
                    return Game.GetGXTEntry("FMMC_PLIB_9");

                case 10:
                    return Game.GetGXTEntry("FMMC_PLIB_10");

                case 11:
                    return Game.GetGXTEntry("FMMC_PLIB_11");

                case 12:
                    return Game.GetGXTEntry("FMMC_PLIB_12");

                case 13:
                    return Game.GetGXTEntry("FMMC_PLIB_TNR");

                case 14:
                    return Game.GetGXTEntry("FMMC_PLIB_13");

                case 15:
                    return Game.GetGXTEntry("FMMC_PLIB_14");

                case 16:
                    return Game.GetGXTEntry("MC_PRSTNT_T0");

                case 17:
                    return Game.GetGXTEntry("MC_PRPSTNT_TWB");

                case 18:
                    return Game.GetGXTEntry("MC_PRSTNT_T7");

                case 19:
                    return Game.GetGXTEntry("MC_PRPSTNT_BAR");

                case 20:
                    return Game.GetGXTEntry("MC_PRSTNT_T8");

                case 21:
                    return Game.GetGXTEntry("FMMC_PLIB_19");

                case 27:
                    return Game.GetGXTEntry("MC_PRSTNT_T3");

                case 28:
                    return Game.GetGXTEntry("FMMC_PLIB_20");

                case 29:
                    return Game.GetGXTEntry("MC_PRSTNT_T4");

                case 30:
                    return Game.GetGXTEntry("MC_PRSTNT_T5");

                case 31:
                    return Game.GetGXTEntry("MC_PRSTNT_T6");

                case 32:
                    return Game.GetGXTEntry("MC_PRSTNT_T2");

                case 34:
                    return Game.GetGXTEntry("MC_PRSTNT_T9");

                case 35:
                    return Game.GetGXTEntry("MC_PRPOWB");

                case 22:
                    return Game.GetGXTEntry("MC_PRSTNT_T10");

                case 33:
                    return Game.GetGXTEntry("FMMC_PLIB_21");

                case 36:
                    return Game.GetGXTEntry("FMMC_PLIB_16");

                case 37:
                    return Game.GetGXTEntry("FMMC_PLIB_GR");

                case 51:
                    return Game.GetGXTEntry("FMMC_PLIB_TMP");

                case 23:
                    return Game.GetGXTEntry("FMMC_PLIB_ART");

                case 24:
                    return Game.GetGXTEntry("FMMC_PLIB_ARC");

                case 25:
                    return Game.GetGXTEntry("FMMC_PLIB_SAG");

                case 26:
                    return Game.GetGXTEntry("FMMC_PLIB_IFG");

                case 38:
                    return Game.GetGXTEntry("FMMC_PLIB_BB");

                case 50:
                    return Game.GetGXTEntry("FMSTP_CUSTOM");

                case 39:
                    return Game.GetGXTEntry("FMMC_PLIB_ARNAT");

                case 40:
                    return Game.GetGXTEntry("FMMC_PLIB_ARNAT2");

                case 41:
                    return Game.GetGXTEntry("FMMC_PLIB_ARNAT3");

                case 42:
                    return Game.GetGXTEntry("FMMC_PLIB_ARNAP");

                case 43:
                    return Game.GetGXTEntry("FMMC_PLIB_SF1");

                case 44:
                    return Game.GetGXTEntry("FMMC_PLIB_MMX");

                case 45:
                    return Game.GetGXTEntry("FMMC_PLIB_ARRMP");

                case 46:
                    return Game.GetGXTEntry("FMMC_PLIB_ARRDY");

                case 47:
                    return Game.GetGXTEntry("FMMC_PLIB_ARRMSC");

                case 48:
                    return Game.GetGXTEntry("FMMC_PLIB_CCTV");

                case 49:
                    return Game.GetGXTEntry("FMMC_PLIB_EXP3D");
            }
            return "";
        }
        #endregion

        public static int GetFinalInCategory(int category)
        {
            switch (category)
            {
                case 0:
                    return 76;
                case 1:
                    return 5;

                case 2:
                    return 4;

                case 3:
                    return 10;

                case 4:
                    return 39;

                case 5:
                    return 9;

                case 6:
                    return 25;

                case 7:
                    return 7;
                case 8:
                    return 15;

                case 9:
                    return 11;

                case 10:
                    return 11;

                case 11:
                    return 4;

                case 12:
                    return 16;

                case 13:
                    return 22;

                case 14:
                    return 67;

                case 15:
                    return 57;

                case 29:
                    return 48;

                case 27:
                    return 38;

                case 30:
                    return 40;

                case 32:
                    return 15;

                case 34:
                    return 7;

                case 31:
                    return 108;

                case 22:
                    return 5;

                case 16:
                    return 48;

                case 17:
                    return 26;

                case 18:
                    return 40;

                case 19:
                    return 26;

                case 20:
                    return 42;

                case 37:
                    return 31;

                case 21:
                    return 49;

                case 28:
                    return 19;

                case 33:
                    return 24;

                case 23:
                    return 40;

                case 24:
                    return 10;

                case 25:
                    return 6;

                case 26:
                    return 26;

                case 35:
                    return 7;

                case 38:
                    return 52;

                case 39:
                    return 19;

                case 36:
                case 40:
                case 41:
                    return 18;

                case 42:
                    return 67;

                case 45:
                    return 22;

                case 43:
                    return 29;

                case 44:
                    return 20;

                case 46:
                    return 2;

                case 47:
                    return 3;

                case 48:
                    return 5;
            }
            return 0;
        }

        public static string GetPropColor(int iParam0, int iParam1)
        {
            if (iParam0 == (int)RacingProps.stt_prop_track_start || iParam0 == (int)RacingProps.stt_prop_track_start_02 || iParam0 == (int)RacingProps.stt_prop_track_straight_s || iParam0 == (int)RacingProps.stt_prop_track_straight_m || iParam0 == (int)RacingProps.stt_prop_track_straight_lm || iParam0 == (int)RacingProps.ba_prop_track_straight_lm || iParam0 == (int)RacingProps.stt_prop_track_straight_l || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_01 || iParam0 == (int)RacingProps.stt_prop_track_bend_m || iParam0 == (int)RacingProps.stt_prop_track_bend_l || iParam0 == (int)RacingProps.stt_prop_track_bend2_l || iParam0 == (int)RacingProps.stt_prop_track_bend_5d || iParam0 == (int)RacingProps.stt_prop_track_bend_15d || iParam0 == (int)RacingProps.stt_prop_track_bend_30d || iParam0 == (int)RacingProps.stt_prop_track_bend_180d || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_180d || iParam0 == (int)RacingProps.stt_prop_track_fork || iParam0 == (int)RacingProps.stt_prop_track_cross || iParam0 == (int)RacingProps.stt_prop_track_straight_bar_s || iParam0 == (int)RacingProps.stt_prop_track_straight_bar_m || iParam0 == (int)RacingProps.stt_prop_track_straight_lm_bar || iParam0 == (int)RacingProps.stt_prop_track_straight_bar_l || iParam0 == (int)RacingProps.stt_prop_track_bend_bar_m || iParam0 == (int)RacingProps.stt_prop_track_bend_bar_l || iParam0 == (int)RacingProps.stt_prop_track_bend2_bar_l || iParam0 == (int)RacingProps.stt_prop_track_bend_5d_bar || iParam0 == (int)RacingProps.stt_prop_track_bend_15d_bar || iParam0 == (int)RacingProps.stt_prop_track_bend_30d_bar || iParam0 == (int)RacingProps.stt_prop_track_bend_180d_bar || iParam0 == (int)RacingProps.stt_prop_track_fork_bar || iParam0 == (int)RacingProps.stt_prop_track_cross_bar || iParam0 == (int)RacingProps.stt_prop_track_funnel || iParam0 == (int)RacingProps.stt_prop_track_funnel_ads_01a || iParam0 == (int)RacingProps.stt_prop_track_funnel_ads_01b || iParam0 == (int)RacingProps.stt_prop_track_funnel_ads_01c || iParam0 == (int)RacingProps.stt_prop_track_link || iParam0 == (int)RacingProps.stt_prop_track_chicane_l || iParam0 == (int)RacingProps.stt_prop_track_chicane_r || iParam0 == (int)RacingProps.stt_prop_track_chicane_l_02 || iParam0 == (int)RacingProps.stt_prop_track_chicane_r_02 || iParam0 == (int)RacingProps.stt_prop_race_start_line_01 || iParam0 == (int)RacingProps.stt_prop_race_start_line_01b || iParam0 == (int)RacingProps.sr_prop_sr_start_line_02 || iParam0 == (int)RacingProps.stt_prop_race_start_line_02b || iParam0 == (int)RacingProps.stt_prop_race_start_line_03 || iParam0 == (int)RacingProps.stt_prop_race_start_line_03b || iParam0 == (int)RacingProps.ch_prop_track_pit_stop_01 || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_30r || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_16l || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_16r || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_30l || iParam0 == (int)RacingProps.stt_prop_track_block_03 || iParam0 == 901501250 || iParam0 == -297099405 || iParam0 == (int)RacingProps.stt_prop_track_bend_bar_l_b || iParam0 == (int)RacingProps.stt_prop_track_bend2_bar_l_b || iParam0 == (int)RacingProps.stt_prop_track_bend_l_b || iParam0 == Funzioni.HashInt("ba_prop_track_bend_l_b") || iParam0 == Funzioni.HashInt("stt_prop_track_bend2_l_b") || iParam0 == Funzioni.HashInt("stt_prop_track_block_01") || iParam0 == Funzioni.HashInt("stt_prop_track_block_02") || iParam0 == Funzioni.HashInt("sr_prop_track_straight_l_d5") || iParam0 == Funzioni.HashInt("sr_prop_track_straight_l_d15") || iParam0 == Funzioni.HashInt("sr_prop_track_straight_l_d30") || iParam0 == Funzioni.HashInt("sr_prop_track_straight_l_d45") || iParam0 == Funzioni.HashInt("sr_prop_track_straight_l_u5") || iParam0 == Funzioni.HashInt("sr_prop_track_straight_l_u15") || iParam0 == Funzioni.HashInt("sr_prop_track_straight_l_u30") || iParam0 == Funzioni.HashInt("sr_prop_track_straight_l_u45") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_bar_45d") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_bar_45") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_45") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_45") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_bar_135") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_bar_135") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_135") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_135") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_xxs") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_xxs") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_xs") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_xs") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_s") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_s") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_m") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_m") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_l") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_l") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_crn") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_crn") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_crn_5d") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_crn_15d") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_crn_30d") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_crn2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_crn2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_fork") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_cross") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_cross") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_gap_01") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_gap_01") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_gap_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_gap_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_gap_03") || iParam0 == Funzioni.HashInt("bkr_prop_biker_tube_gap_03") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_qg") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_hg") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_jmp") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_jmp2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_fn_01") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_fn_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_fn_03") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_fn_04") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_fn_05") || iParam0 == Funzioni.HashInt("ba_prop_battle_tube_fn_01") || iParam0 == Funzioni.HashInt("ba_prop_battle_tube_fn_02") || iParam0 == Funzioni.HashInt("ba_prop_battle_tube_fn_03") || iParam0 == Funzioni.HashInt("ba_prop_battle_tube_fn_04") || iParam0 == Funzioni.HashInt("ba_prop_battle_tube_fn_05") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_ent") || iParam0 == Funzioni.HashInt("stt_prop_stunt_tube_end") || iParam0 == Funzioni.HashInt("sr_prop_sr_tube_end") || iParam0 == Funzioni.HashInt("stt_prop_track_tube_01") || iParam0 == Funzioni.HashInt("as_prop_as_tube_gap_03") || iParam0 == Funzioni.HashInt("as_prop_as_tube_xxs") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_01") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_lit_01") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_02") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_03") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_04") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_05") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_06") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_07") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_08") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_09") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_010") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_011") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_012") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_013") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_014") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_015") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r1") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_lit_0r1") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r2") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r3") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r04") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r05") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r06") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r07") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r08") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r09") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r010") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r011") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r012") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r013") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r014") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r015") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r016") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r017") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r018") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r019") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l1") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_lit_0l1") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l2") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l3") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l04") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l05") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l06") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l07") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l08") || iParam0 == 1784795865 || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l010") || iParam0 == 1307595665 || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l012") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l013") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l014") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l015") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l16") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l17") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l018") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l019") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l020") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_pit_l") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_pit_r") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_u_l") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_u_r") || iParam0 == Funzioni.HashInt("stt_prop_track_stop_sign") || iParam0 == Funzioni.HashInt("stt_prop_stunt_target_small") || iParam0 == Funzioni.HashInt("stt_prop_stunt_target") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bowlpin_stand") || iParam0 == Funzioni.HashInt("stt_prop_stunt_landing_zone_01") || iParam0 == Funzioni.HashInt("stt_prop_hoop_tyre_01a") || iParam0 == Funzioni.HashInt("as_prop_as_stunt_target") || iParam0 == Funzioni.HashInt("as_prop_as_stunt_target_small") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_sml1") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_sml2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_sml3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_mdm1") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_mdm2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_mdm3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_lrg1") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_lrg2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_lrg3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_xl1") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_xl2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_xl3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_01") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_03") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_04") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_05") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bowling_ball") || iParam0 == Funzioni.HashInt("ar_prop_ar_ammu_sign") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_ammu_sign") || iParam0 == Funzioni.HashInt("stt_prop_stunt_soccer_goal") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_start") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_start_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_st_01") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_st_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_exshort") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_short") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_straight") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_turn") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_sh15") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_sh30") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_sh45") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_sh45_a") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_uturn") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_cutout") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_otake") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_fork") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_funnel") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_funlng") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_slope15") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_slope30") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_slope45") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_link") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwlink") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwlink_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_hill") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_hill2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_bumps") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_jump") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump15") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump30") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump45") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwshort") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwsh15") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwturn") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwuturn") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwslope15") || iParam0 == Funzioni.HashInt("as_prop_as_dwslope30") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwslope45") || iParam0 == Funzioni.HashInt("stt_prop_track_tube_02") || iParam0 == Funzioni.HashInt("ba_prop_battle_track_exshort") || iParam0 == Funzioni.HashInt("ba_prop_battle_track_short") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_s") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_m") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_l") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_sb") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_mb") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_lb") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_xs") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_s") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_m") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_l") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_xl") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_xxl") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_01a") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_01b") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_01c") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_02a") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_02b") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_02c") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_flip_s") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_flip_sb") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_flip_m") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_flip_mb") || iParam0 == Funzioni.HashInt("stt_prop_stunt_ramp") || iParam0 == Funzioni.HashInt("stt_prop_stunt_wideramp") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_qp") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_qp2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_qp3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_hump_01") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_hump_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_s") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_m") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_l") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_sb") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_mb") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_lb") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_01a") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_01b") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_01c") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_02a") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_02b") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_02c") || iParam0 == Funzioni.HashInt("stt_prop_stunt_wideramp") || iParam0 == Funzioni.HashInt("imp_prop_impexp_bblock_qp3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_qp2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_qp") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_hump_01") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_hump_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_cor") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_cor_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_cor_03") || iParam0 == Funzioni.HashInt("stt_prop_wallride_05") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_loop") || iParam0 == Funzioni.HashInt("stt_prop_ramp_multi_loop_rb") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_loop") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_s") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l_s") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_m") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l_m") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l_l") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_xxl") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l_xxl") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_hloop") || iParam0 == Funzioni.HashInt("stt_prop_wallride_01") || iParam0 == Funzioni.HashInt("stt_prop_wallride_01b") || iParam0 == Funzioni.HashInt("stt_prop_wallride_04") || iParam0 == Funzioni.HashInt("stt_prop_wallride_45r") || iParam0 == Funzioni.HashInt("stt_prop_wallride_45ra") || iParam0 == Funzioni.HashInt("stt_prop_wallride_45l") || iParam0 == Funzioni.HashInt("stt_prop_wallride_45la") || iParam0 == Funzioni.HashInt("stt_prop_wallride_90r") || iParam0 == Funzioni.HashInt("stt_prop_wallride_90rb") || iParam0 == Funzioni.HashInt("stt_prop_wallride_90l") || iParam0 == Funzioni.HashInt("stt_prop_wallride_90lb") || iParam0 == Funzioni.HashInt("stt_prop_wallride_02") || iParam0 == Funzioni.HashInt("stt_prop_wallride_02b") || iParam0 == Funzioni.HashInt("stt_prop_wallride_05b") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_sml1") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_sml2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_sml3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_mdm1") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_mdm2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_mdm3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_lrg1") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_lrg2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_lrg3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_xl1") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_xl2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_xl3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_01") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_03") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_04") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_05") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_qp") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_qp2") || iParam0 == Funzioni.HashInt("imp_prop_impexp_bblock_qp3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_hump_01") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_hump_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_landing_zone_01") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bowlpin_stand") || iParam0 == Funzioni.HashInt("bkr_prop_biker_target_small") || iParam0 == Funzioni.HashInt("bkr_prop_biker_target") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_blimp_sign") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_take_flight_sign") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_ron_sign") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_xero_sign") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn_15d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn_30d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn_5d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn2") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_cross") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_fork") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_hg") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_jmp") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_l") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_m") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_qg") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_s") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_xxs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_xs") || iParam0 == Funzioni.HashInt("as_prop_as_tube_gap_02") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_l") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_m") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_s") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_xs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_xxs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn2") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn_30d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn_15d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn_5d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_gap_02") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_l") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_m") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_s") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_xs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_xxs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn2") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn_30d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn_15d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn_5d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_gap_02") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_01a") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_01b") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_02a") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_02b") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_03a") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_04a") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_05a") || iParam0 == Funzioni.HashInt("ar_prop_ar_start_01a") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_h1") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_h2") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_h2") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_h1") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_h1") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_h2") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_01a") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_01b") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_01c") || iParam0 == Funzioni.HashInt("ar_prop_ig_sprunk_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_raine_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_flow_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_shark_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_jackal_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_metv_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_metv_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_jackal_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_shark_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_flow_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_raine_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_sprunk_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_01a") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_01b") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_01c") || iParam0 == Funzioni.HashInt("ar_prop_ar_jump_loop") || iParam0 == Funzioni.HashInt("gr_prop_gr_target_5_01b") || iParam0 == Funzioni.HashInt("ch_prop_stunt_landing_zone_01a") || iParam0 == Funzioni.HashInt("sum_prop_archway_02") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_01_sec") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_02_sec") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_04_sec") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_08_sec") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_16_sec") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_05d") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_15d") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_30d") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_45d") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_90d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_05d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_15d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_30d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_45d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_90d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_05d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_15d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_30d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_45d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_90d") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_180d") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_bar_180d") || iParam0 == Funzioni.HashInt("ch_prop_track_bend_bar_lc") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_lc"))
            {
                switch (iParam1)
                {
                    case 0:
                        return "MC_STNT_USA";

                    case 1:
                        return "MC_STNT_RED";

                    case 2:
                        return "MC_STNT_BLU";

                    case 3:
                        return "MC_STNT_PRP";

                    case 4:
                        return "MC_STNT_BLK";

                    case 5:
                        return "MC_STNT_WHT";

                    case 6:
                        return "MC_STNT_GRY";

                    case 7:
                        return "MC_STNT_YLW";

                    case 8:
                        return "MC_STNT_ORG";

                    case 9:
                        return "MC_STNT_GRN";

                    case 10:
                        return "MC_STNT_PNK";

                    case 11:
                        return "MC_STNT_RAC";

                    case 12:
                        return "MC_STNT_BNY";

                    case 13:
                        return "MC_STNT_ONB";

                    case 14:
                        return "MC_STNT_GNY";

                    case 15:
                        return "MC_STNT_PNG";
                }
            }
            else if (iParam0 == (int)RacingProps.ch_prop_track_ch_straight_bar_s_s || iParam0 == (int)RacingProps.ch_prop_track_ch_straight_bar_s || iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_l_out || iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_l_b || iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_m_out || iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_m_in || iParam0 == (int)RacingProps.ch_prop_track_ch_straight_bar_m || iParam0 == (int)RacingProps.sum_prop_track_ac_straight_bar_s_s || iParam0 == (int)RacingProps.sum_prop_track_ac_straight_bar_s || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_m_out || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_m_in || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_l_out || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_l_b || iParam0 == (int)RacingProps.ch_prop_track_ch_straight_bar_m)
            {
                switch (iParam1)
                {
                    case 0:
                        return "MC_STNT_USA";

                    case 1:
                        return "MC_STNT_RED";

                    case 2:
                        return "MC_STNT_WHT";

                    case 3:
                        return "MC_STNT_PRP";

                    case 4:
                        return "MC_STNT_BWCH";

                    case 5:
                        return "";

                    case 6:
                        return "MC_STNT_BWH";

                    case 7:
                        return "MC_STNT_YLW";

                    case 8:
                        return "MC_STNT_BWV";

                    case 9:
                        return "";

                    case 10:
                        return "MC_STNT_BWVA";

                    case 11:
                        return "MC_STNT_RAC";

                    case 12:
                        return "MC_STNT_BNY";

                    case 13:
                        return "MC_STNT_ONB";

                    case 14:
                        return "MC_STNT_GNY";

                    case 15:
                        return "MC_STNT_PNG";
                }
            }
            else if (iParam0 == (int)RacingProps.sum_prop_ac_pit_sign_r_01a || iParam0 == (int)RacingProps.sum_prop_ac_pit_sign_l_01a)
            {
                switch (iParam1)
                {
                    case 0:
                        return "MC_STNT_USA";

                    case 1:
                        return "MC_STNT_RED";

                    case 2:
                        return "MC_STNT_BLU";

                    case 3:
                        return "MC_STNT_PRP";

                    case 4:
                        return "MC_STNT_BLK";

                    case 5:
                        return "MC_STNT_WHT";

                    case 6:
                        return "MC_STNT_GRY";

                    case 7:
                        return "MC_STNT_YLW";

                    case 8:
                        return "MC_STNT_ORG";

                    case 9:
                        return "MC_STNT_GRN";

                    case 10:
                        return "MC_STNT_PNK";

                    case 11:
                        return "MC_STNT_RCEB";

                    case 12:
                        return "MC_STNT_BNY";

                    case 13:
                        return "MC_STNT_OBLK";

                    case 14:
                        return "MC_STNT_GNY";

                    case 15:
                        return "MC_STNT_PNG";
                }
            }
            else if (iParam0 == (int)RacingProps.stt_prop_hoop_tyre_01a || iParam0 == (int)RacingProps.sr_prop_special_bblock_sml1 || iParam0 == (int)RacingProps.sr_prop_special_bblock_mdm1 || iParam0 == (int)RacingProps.sr_prop_special_bblock_lrg11 || iParam0 == (int)RacingProps.sr_prop_special_bblock_xl1 || iParam0 == (int)RacingProps.sr_prop_special_bblock_sml2 || iParam0 == (int)RacingProps.sr_prop_special_bblock_mdm2 || iParam0 == (int)RacingProps.sr_prop_special_bblock_lrg2 || iParam0 == (int)RacingProps.sr_prop_special_bblock_xl2 || iParam0 == (int)RacingProps.sr_prop_special_bblock_sml3 || iParam0 == (int)RacingProps.sr_prop_special_bblock_mdm3 || iParam0 == (int)RacingProps.sr_prop_special_bblock_lrg3 || iParam0 == (int)RacingProps.sr_prop_special_bblock_xl3 || iParam0 == (int)RacingProps.sr_prop_special_bblock_xl3_fixed || iParam0 == (int)RacingProps.ar_prop_ar_bblock_huge_01 || iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_01 || iParam0 == (int)RacingProps.ar_prop_ar_bblock_huge_02 || iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_02 || iParam0 == (int)RacingProps.ar_prop_ar_bblock_huge_03 || iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_03 || iParam0 == (int)RacingProps.as_prop_as_bblock_huge_04 || iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_04 || iParam0 == (int)RacingProps.as_prop_as_bblock_huge_05 || iParam0 == (int)RacingProps.vw_prop_vw_bblock_huge_05 || iParam0 == (int)RacingProps.ar_prop_ar_stunt_block_01a || iParam0 == (int)RacingProps.ar_prop_ar_stunt_block_01b || iParam0 == (int)RacingProps.ar_prop_ar_arrow_thin_m || iParam0 == (int)RacingProps.ar_prop_ar_arrow_wide_m || iParam0 == (int)RacingProps.ar_prop_ar_arrow_thin_l || iParam0 == (int)RacingProps.ar_prop_ar_arrow_wide_l || iParam0 == (int)RacingProps.ar_prop_ar_arrow_thin_xl || iParam0 == (int)RacingProps.ar_prop_ar_arrow_wide_xl || iParam0 == (int)RacingProps.ar_prop_ar_hoop_med_01)
            {
                switch (iParam1)
                {
                    case 1:
                        return "MC_STNT_RED";

                    case 2:
                        return "MC_STNT_BLU";

                    case 3:
                        return "MC_STNT_PRP";

                    case 4:
                        return "MC_STNT_BLK";

                    case 5:
                        return "MC_STNT_WHT";

                    case 6:
                        return "MC_STNT_GRY";

                    case 7:
                        return "MC_STNT_YLW";

                    case 8:
                        return "MC_STNT_ORG";

                    case 9:
                        return "MC_STNT_GRN";

                    case 10:
                        return "MC_STNT_PNK";
                }
            }
            else if (iParam0 == (int)RacingProps.xs_prop_arena_goal || iParam0 == -296431790)
            {
                switch (iParam1)
                {
                    case 1:
                        return "MC_STNT_RED";

                    case 2:
                        return "MC_STNT_PRP";

                    case 3:
                        return "MC_STNT_BLK";

                    case 4:
                        return "MC_STNT_YLW";

                    case 5:
                        return "MC_STNT_ORG";

                    case 6:
                        return "MC_STNT_GRN";

                    case 7:
                        return "MC_STNT_PNK";
                }
            }
            else if (iParam0 == (int)RacingProps.xs_prop_arena_goal_sf)
            {
                switch (iParam1)
                {
                    case 1:
                        return "MC_STNT_BLU";

                    case 2:
                        return "MC_STNT_PRP";

                    case 3:
                        return "MC_STNT_BLK";

                    case 4:
                        return "MC_STNT_YLW";

                    case 5:
                        return "MC_STNT_ORG";

                    case 6:
                        return "MC_STNT_GRN";

                    case 7:
                        return "MC_STNT_PNK";
                }
            }
            else if (iParam0 == -1154284456 || iParam0 == -566149868 || iParam0 == -1532981448 || iParam0 == 1132021903 || iParam0 == 2086127383 || iParam0 == 1218934150 || iParam0 == 1992218160)
            {
                switch (iParam1)
                {
                    case 1:
                        return "MC_STNT_RED";

                    case 2:
                        return "MC_STNT_BLU";

                    case 3:
                        return "MC_STNT_PRP";

                    case 4:
                        return "MC_STNT_BLK";

                    case 5:
                        return "MC_STNT_YLW";

                    case 6:
                        return "MC_STNT_ORG";

                    case 7:
                        return "MC_STNT_GRN";

                    case 8:
                        return "MC_STNT_PNK";
                }
            }
            else if (iParam0 == (int)RacingProps.xs_prop_arena_tower_02a)
            {
                switch (iParam1)
                {
                    case 1:
                        return "MC_STNT_BLK";

                    case 2:
                        return "MC_STNT_PRP";

                    case 3:
                        return "MC_STNT_GRY";

                    case 4:
                        return "MC_STNT_YLW";

                    case 5:
                        return "MC_STNT_ORG";

                    case 6:
                        return "MC_STNT_GRN";

                    case 7:
                        return "MC_STNT_PNK";
                }
            }
            else if (iParam0 == (int)RacingProps.stt_prop_corner_sign_01 || iParam0 == (int)RacingProps.stt_prop_corner_sign_02 || iParam0 == (int)RacingProps.stt_prop_corner_sign_03 || iParam0 == (int)RacingProps.stt_prop_corner_sign_04 || iParam0 == (int)RacingProps.stt_prop_corner_sign_05 || iParam0 == (int)RacingProps.stt_prop_corner_sign_06 || iParam0 == (int)RacingProps.stt_prop_corner_sign_07 || iParam0 == (int)RacingProps.stt_prop_corner_sign_08 || iParam0 == (int)RacingProps.stt_prop_corner_sign_09 || iParam0 == (int)RacingProps.stt_prop_corner_sign_10 || iParam0 == (int)RacingProps.stt_prop_corner_sign_11 || iParam0 == (int)RacingProps.stt_prop_corner_sign_12 || iParam0 == (int)RacingProps.stt_prop_corner_sign_13 || iParam0 == (int)RacingProps.stt_prop_corner_sign_14 || iParam0 == (int)RacingProps.ch_prop_pit_sign_01a || iParam0 == (int)RacingProps.sum_prop_ac_pit_sign_left)
            {
                switch (iParam1)
                {
                    case 0:
                        return "MC_STNT_BNW";

                    case 1:
                        return "MC_STNT_YNR";

                    case 2:
                        return "MC_STNT_RNW";

                    case 3:
                        return "MC_STNT_YNB";
                }
            }
            else if (iParam0 == (int)RacingProps.sr_prop_sr_track_jumpwall)
            {
                switch (iParam1)
                {
                    case 0:
                        return "MC_STNT_USA";

                    case 1:
                        return "MC_STNT_RNW";

                    case 2:
                        return "MC_STNT_BLU";

                    case 3:
                        return "MC_STNT_PRP";

                    case 4:
                        return "MC_STNT_BLK";

                    case 5:
                        return "MC_STNT_WHT";

                    case 6:
                        return "MC_STNT_GRY";

                    case 7:
                        return "MC_STNT_YLW";

                    case 8:
                        return "MC_STNT_ORG";

                    case 9:
                        return "MC_STNT_GRN";

                    case 10:
                        return "MC_STNT_GNP";

                    case 11:
                        return "MC_STNT_RAC";

                    case 12:
                        return "MC_STNT_BNY";

                    case 13:
                        return "MC_STNT_ONB";

                    case 14:
                        return "MC_STNT_GNY";

                    case 15:
                        return "MC_STNT_PNG";
                }
            }
            else if (iParam0 == (int)RacingProps.sm_prop_smug_cover_01a)
            {
                switch (iParam1)
                {
                    case 0:
                        return "MC_STNT_BYLW";

                    case 1:
                        return "MC_STNT_YLW";

                    case 3:
                        return "MC_STNT_ORG";

                    case 5:
                        return "MC_STNT_PRP";

                    case 6:
                        return "MC_STNT_BLU";

                    case 7:
                        return "MC_STNT_BRO";

                    case 8:
                        return "MC_STNT_BLK";

                    case 9:
                        return "MC_STNT_TAN";
                }
            }
            else if (iParam0 == (int)RacingProps.xm_prop_x17_cover_01)
            {
                switch (iParam1)
                {
                    case 0:
                        return "MC_STNT_BYLW";

                    case 1:
                        return "MC_STNT_DGRY";

                    case 2:
                        return "MC_STNT_LGRY";

                    case 3:
                        return "MC_STNT_YLW";
                }
                if (iParam1 == -1)
                {
                    return "FMMC_SEL_OFF";
                }
                return "FMMC_SEL_DEF";
            }
            return "FMMC_SEL_OFF";
        }

        public static int GetColorCount(int iParam0)
        {
            if ((iParam0 == (int)RacingProps.stt_prop_track_start || iParam0 == (int)RacingProps.stt_prop_track_start_02 || iParam0 == (int)RacingProps.stt_prop_track_straight_s || iParam0 == (int)RacingProps.stt_prop_track_straight_m || iParam0 == (int)RacingProps.stt_prop_track_straight_lm || iParam0 == (int)RacingProps.ba_prop_track_straight_lm || iParam0 == (int)RacingProps.stt_prop_track_straight_l || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_01 || iParam0 == (int)RacingProps.stt_prop_track_bend_m || iParam0 == (int)RacingProps.stt_prop_track_bend_l || iParam0 == (int)RacingProps.stt_prop_track_bend2_l || iParam0 == (int)RacingProps.stt_prop_track_bend_5d || iParam0 == (int)RacingProps.stt_prop_track_bend_15d || iParam0 == (int)RacingProps.stt_prop_track_bend_30d || iParam0 == (int)RacingProps.stt_prop_track_bend_180d || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_180d || iParam0 == (int)RacingProps.stt_prop_track_fork || iParam0 == (int)RacingProps.stt_prop_track_cross || iParam0 == (int)RacingProps.stt_prop_track_straight_bar_s || iParam0 == (int)RacingProps.stt_prop_track_straight_bar_m || iParam0 == (int)RacingProps.stt_prop_track_straight_lm_bar || iParam0 == (int)RacingProps.stt_prop_track_straight_bar_l || iParam0 == (int)RacingProps.stt_prop_track_bend_bar_m || iParam0 == (int)RacingProps.stt_prop_track_bend_bar_l || iParam0 == (int)RacingProps.stt_prop_track_bend2_bar_l || iParam0 == (int)RacingProps.stt_prop_track_bend_5d_bar || iParam0 == (int)RacingProps.stt_prop_track_bend_15d_bar || iParam0 == (int)RacingProps.stt_prop_track_bend_30d_bar || iParam0 == (int)RacingProps.stt_prop_track_bend_180d_bar || iParam0 == (int)RacingProps.stt_prop_track_fork_bar || iParam0 == (int)RacingProps.stt_prop_track_cross_bar || iParam0 == (int)RacingProps.stt_prop_track_funnel || iParam0 == (int)RacingProps.stt_prop_track_funnel_ads_01a || iParam0 == (int)RacingProps.stt_prop_track_funnel_ads_01b || iParam0 == (int)RacingProps.stt_prop_track_funnel_ads_01c || iParam0 == (int)RacingProps.stt_prop_track_link || iParam0 == (int)RacingProps.stt_prop_track_chicane_l || iParam0 == (int)RacingProps.stt_prop_track_chicane_r || iParam0 == (int)RacingProps.stt_prop_track_chicane_l_02 || iParam0 == (int)RacingProps.stt_prop_track_chicane_r_02 || iParam0 == (int)RacingProps.stt_prop_race_start_line_01 || iParam0 == (int)RacingProps.stt_prop_race_start_line_01b || iParam0 == (int)RacingProps.sr_prop_sr_start_line_02 || iParam0 == (int)RacingProps.stt_prop_race_start_line_02b || iParam0 == (int)RacingProps.stt_prop_race_start_line_03 || iParam0 == (int)RacingProps.stt_prop_race_start_line_03b || iParam0 == (int)RacingProps.ch_prop_track_pit_stop_01 || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_30r || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_16l || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_16r || iParam0 == (int)RacingProps.sum_prop_ac_track_pit_stop_30l || iParam0 == (int)RacingProps.stt_prop_track_block_03 || iParam0 == 901501250 || iParam0 == -297099405 || iParam0 == (int)RacingProps.stt_prop_track_bend_bar_l_b || iParam0 == (int)RacingProps.stt_prop_track_bend2_bar_l_b || iParam0 == (int)RacingProps.stt_prop_track_bend_l_b || iParam0 == (int)RacingProps.ba_prop_track_bend_l_b || iParam0 == (int)RacingProps.stt_prop_track_bend2_l_b || iParam0 == (int)RacingProps.stt_prop_track_block_01 || iParam0 == (int)RacingProps.stt_prop_track_block_02 || iParam0 == (int)RacingProps.sr_prop_track_straight_l_d5 || iParam0 == (int)RacingProps.sr_prop_track_straight_l_d15 || iParam0 == (int)RacingProps.sr_prop_track_straight_l_d30 || iParam0 == (int)RacingProps.sr_prop_track_straight_l_d45 || iParam0 == (int)RacingProps.sr_prop_track_straight_l_u5 || iParam0 == (int)RacingProps.sr_prop_track_straight_l_u15 || iParam0 == (int)RacingProps.sr_prop_track_straight_l_u30 || iParam0 == (int)RacingProps.sr_prop_track_straight_l_u45 || iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_45d || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_45 || iParam0 == (int)RacingProps.ch_prop_track_ch_bend_45 || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_45 || iParam0 == (int)RacingProps.ch_prop_track_ch_bend_bar_135 || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_bar_135 || iParam0 == (int)RacingProps.ch_prop_track_ch_bend_135 || iParam0 == (int)RacingProps.sum_prop_track_ac_bend_135 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_xxs || iParam0 == (int)RacingProps.bkr_prop_biker_tube_xxs || iParam0 == (int)RacingProps.stt_prop_stunt_tube_xs || iParam0 == (int)RacingProps.bkr_prop_biker_tube_xs || iParam0 == (int)RacingProps.stt_prop_stunt_tube_s || iParam0 == (int)RacingProps.bkr_prop_biker_tube_s || iParam0 == (int)RacingProps.stt_prop_stunt_tube_m || iParam0 == (int)RacingProps.bkr_prop_biker_tube_m || iParam0 == (int)RacingProps.stt_prop_stunt_tube_l || iParam0 == (int)RacingProps.bkr_prop_biker_tube_l || iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn || iParam0 == (int)RacingProps.bkr_prop_biker_tube_crn || iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn_5d || iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn_15d || iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn_30d || iParam0 == (int)RacingProps.stt_prop_stunt_tube_crn2 || iParam0 == (int)RacingProps.bkr_prop_biker_tube_crn2 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_fork || iParam0 == (int)RacingProps.stt_prop_stunt_tube_cross || iParam0 == (int)RacingProps.bkr_prop_biker_tube_cross || iParam0 == (int)RacingProps.stt_prop_stunt_tube_gap_01 || iParam0 == (int)RacingProps.bkr_prop_biker_tube_gap_01 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_gap_02 || iParam0 == (int)RacingProps.bkr_prop_biker_tube_gap_02 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_gap_03 || iParam0 == (int)RacingProps.bkr_prop_biker_tube_gap_03 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_qg || iParam0 == (int)RacingProps.stt_prop_stunt_tube_hg || iParam0 == (int)RacingProps.stt_prop_stunt_tube_jmp || iParam0 == (int)RacingProps.stt_prop_stunt_tube_jmp2 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_01 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_02 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_03 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_04 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_fn_05 || iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_01 || iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_02 || iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_03 || iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_04 || iParam0 == (int)RacingProps.ba_prop_battle_tube_fn_05 || iParam0 == (int)RacingProps.stt_prop_stunt_tube_ent || iParam0 == (int)RacingProps.stt_prop_stunt_tube_end || iParam0 == (int)RacingProps.sr_prop_sr_tube_end || iParam0 == (int)RacingProps.stt_prop_track_tube_01 || iParam0 == (int)RacingProps.as_prop_as_tube_gap_03 || iParam0 == (int)RacingProps.as_prop_as_tube_xxs || iParam0 == (int)RacingProps.stt_prop_tyre_wall_01 || iParam0 == (int)RacingProps.sum_prop_ac_tyre_wall_lit_01 || iParam0 == (int)RacingProps.stt_prop_tyre_wall_02 || iParam0 == (int)RacingProps.stt_prop_tyre_wall_03 || iParam0 == (int)RacingProps.stt_prop_tyre_wall_04 || iParam0 == (int)RacingProps.stt_prop_tyre_wall_05 || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_06") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_07") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_08") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_09") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_010") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_011") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_012") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_013") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_014") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_015") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r1") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_lit_0r1") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r2") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r3") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r04") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r05") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r06") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r07") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r08") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r09") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r010") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r011") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r012") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r013") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r014") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r015") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r016") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r017") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r018") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0r019") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l1") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_lit_0l1") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l2") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l3") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l04") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l05") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l06") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l07") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l08") || iParam0 == 1784795865 || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l010") || iParam0 == 1307595665 || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l012") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l013") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l014") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l015") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l16") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l17") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l018") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l019") || iParam0 == Funzioni.HashInt("stt_prop_tyre_wall_0l020") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_pit_l") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_pit_r") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_u_l") || iParam0 == Funzioni.HashInt("sum_prop_ac_tyre_wall_u_r") || iParam0 == Funzioni.HashInt("stt_prop_track_stop_sign") || iParam0 == Funzioni.HashInt("stt_prop_stunt_target_small") || iParam0 == Funzioni.HashInt("stt_prop_stunt_target") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bowlpin_stand") || iParam0 == Funzioni.HashInt("stt_prop_stunt_landing_zone_01") || iParam0 == Funzioni.HashInt("stt_prop_hoop_tyre_01a") || iParam0 == Funzioni.HashInt("as_prop_as_stunt_target") || iParam0 == Funzioni.HashInt("as_prop_as_stunt_target_small") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_sml1") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_sml2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_sml3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_mdm1") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_mdm2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_mdm3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_lrg1") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_lrg2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_lrg3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_xl1") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_xl2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_xl3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_01") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_03") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_04") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_huge_05") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bowling_ball") || iParam0 == Funzioni.HashInt("ar_prop_ar_ammu_sign") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_ammu_sign") || iParam0 == Funzioni.HashInt("stt_prop_stunt_soccer_goal") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_start") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_start_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_st_01") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_st_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_exshort") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_short") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_straight") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_turn") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_sh15") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_sh30") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_sh45") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_sh45_a") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_uturn") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_cutout") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_otake") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_fork") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_funnel") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_funlng") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_slope15") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_slope30") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_slope45") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_link") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwlink") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwlink_02") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_hill") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_hill2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_bumps") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_jump") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump15") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump30") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump45") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwshort") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwsh15") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwturn") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwuturn") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwslope15") || iParam0 == Funzioni.HashInt("as_prop_as_dwslope30") || iParam0 == Funzioni.HashInt("stt_prop_stunt_track_dwslope45") || iParam0 == Funzioni.HashInt("stt_prop_track_tube_02") || iParam0 == Funzioni.HashInt("ba_prop_battle_track_exshort") || iParam0 == Funzioni.HashInt("ba_prop_battle_track_short") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_s") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_m") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_l") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_sb") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_mb") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_lb") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_xs") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_s") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_m") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_l") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_xl") || iParam0 == Funzioni.HashInt("stt_prop_ramp_jump_xxl") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_01a") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_01b") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_01c") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_02a") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_02b") || iParam0 == Funzioni.HashInt("stt_prop_track_jump_02c") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_flip_s") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_flip_sb") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_flip_m") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_flip_mb") || iParam0 == Funzioni.HashInt("stt_prop_stunt_ramp") || iParam0 == Funzioni.HashInt("stt_prop_stunt_wideramp") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_qp") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_qp2") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_qp3") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_hump_01") || iParam0 == Funzioni.HashInt("stt_prop_stunt_bblock_hump_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_s") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_m") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_l") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_sb") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_mb") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_lb") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_01a") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_01b") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_01c") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_02a") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_02b") || iParam0 == Funzioni.HashInt("bkr_prop_biker_jump_02c") || iParam0 == Funzioni.HashInt("stt_prop_stunt_wideramp") || iParam0 == Funzioni.HashInt("imp_prop_impexp_bblock_qp3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_qp2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_qp") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_hump_01") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_hump_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_cor") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_cor_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_cor_03") || iParam0 == Funzioni.HashInt("stt_prop_wallride_05") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_loop") || iParam0 == Funzioni.HashInt("stt_prop_ramp_multi_loop_rb") || iParam0 == Funzioni.HashInt("stt_prop_stunt_jump_loop") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_s") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l_s") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_m") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l_m") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l_l") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_xxl") || iParam0 == Funzioni.HashInt("stt_prop_ramp_spiral_l_xxl") || iParam0 == Funzioni.HashInt("stt_prop_ramp_adj_hloop") || iParam0 == Funzioni.HashInt("stt_prop_wallride_01") || iParam0 == Funzioni.HashInt("stt_prop_wallride_01b") || iParam0 == Funzioni.HashInt("stt_prop_wallride_04") || iParam0 == Funzioni.HashInt("stt_prop_wallride_45r") || iParam0 == Funzioni.HashInt("stt_prop_wallride_45ra") || iParam0 == Funzioni.HashInt("stt_prop_wallride_45l") || iParam0 == Funzioni.HashInt("stt_prop_wallride_45la") || iParam0 == Funzioni.HashInt("stt_prop_wallride_90r") || iParam0 == Funzioni.HashInt("stt_prop_wallride_90rb") || iParam0 == Funzioni.HashInt("stt_prop_wallride_90l") || iParam0 == Funzioni.HashInt("stt_prop_wallride_90lb") || iParam0 == Funzioni.HashInt("stt_prop_wallride_02") || iParam0 == Funzioni.HashInt("stt_prop_wallride_02b") || iParam0 == Funzioni.HashInt("stt_prop_wallride_05b") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_sml1") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_sml2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_sml3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_mdm1") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_mdm2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_mdm3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_lrg1") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_lrg2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_lrg3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_xl1") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_xl2") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_xl3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_01") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_03") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_04") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_huge_05") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_qp") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_qp2") || iParam0 == Funzioni.HashInt("imp_prop_impexp_bblock_qp3") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_hump_01") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bblock_hump_02") || iParam0 == Funzioni.HashInt("bkr_prop_biker_landing_zone_01") || iParam0 == Funzioni.HashInt("bkr_prop_biker_bowlpin_stand") || iParam0 == Funzioni.HashInt("bkr_prop_biker_target_small") || iParam0 == Funzioni.HashInt("bkr_prop_biker_target") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_blimp_sign") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_take_flight_sign") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_ron_sign") || iParam0 == Funzioni.HashInt("sr_mp_spec_races_xero_sign") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn_15d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn_30d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn_5d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_crn2") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_cross") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_fork") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_hg") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_jmp") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_l") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_m") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_qg") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_s") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_xxs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_xs") || iParam0 == Funzioni.HashInt("as_prop_as_tube_gap_02") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_l") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_m") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_s") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_xs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_xxs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn2") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn_30d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn_15d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_crn_5d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_4x_gap_02") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_l") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_m") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_s") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_xs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_xxs") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn2") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn_30d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn_15d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_crn_5d") || iParam0 == Funzioni.HashInt("ar_prop_ar_tube_2x_gap_02") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_01a") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_01b") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_02a") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_02b") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_03a") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_04a") || iParam0 == Funzioni.HashInt("ar_prop_ar_neon_gate_05a") || iParam0 == Funzioni.HashInt("ar_prop_ar_start_01a") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_h1") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_h2") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_h2") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_h1") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_h1") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_h2") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_01a") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_01b") || iParam0 == Funzioni.HashInt("ar_prop_inflategates_cp_loop_01c") || iParam0 == Funzioni.HashInt("ar_prop_ig_sprunk_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_raine_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_flow_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_shark_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_jackal_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_metv_cp_b") || iParam0 == Funzioni.HashInt("ar_prop_ig_metv_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_jackal_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_shark_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_flow_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_raine_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_ig_sprunk_cp_single") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_01a") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_01b") || iParam0 == Funzioni.HashInt("ar_prop_gate_cp_90d_01c") || iParam0 == Funzioni.HashInt("ar_prop_ar_jump_loop") || iParam0 == Funzioni.HashInt("gr_prop_gr_target_5_01b") || iParam0 == Funzioni.HashInt("ch_prop_stunt_landing_zone_01a") || iParam0 == Funzioni.HashInt("sum_prop_archway_02") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_01_sec") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_02_sec") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_04_sec") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_08_sec") || iParam0 == Funzioni.HashInt("sum_prop_race_barrier_16_sec") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_05d") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_15d") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_30d") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_45d") || iParam0 == Funzioni.HashInt("sum_prop_barrier_ac_bend_90d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_05d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_15d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_30d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_45d") || iParam0 == Funzioni.HashInt("sum_prop_ac_long_barrier_90d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_05d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_15d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_30d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_45d") || iParam0 == Funzioni.HashInt("sum_prop_ac_short_barrier_90d") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_180d") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_bar_180d") || iParam0 == Funzioni.HashInt("ch_prop_track_bend_bar_lc") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_lc"))
                || (iParam0 == Funzioni.HashInt("ch_prop_track_ch_straight_bar_s_s") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_straight_bar_s") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_bar_l_out") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_bar_l_b") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_bar_m_out") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_bend_bar_m_in") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_straight_bar_m") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_straight_bar_s_s") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_straight_bar_s") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_bar_m_out") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_bar_m_in") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_bar_l_out") || iParam0 == Funzioni.HashInt("sum_prop_track_ac_bend_bar_l_b") || iParam0 == Funzioni.HashInt("ch_prop_track_ch_straight_bar_m"))
                || (iParam0 == Funzioni.HashInt("sum_prop_ac_pit_sign_r_01a") || iParam0 == Funzioni.HashInt("sum_prop_ac_pit_sign_l_01a"))
                || (iParam0 == Funzioni.HashInt("sr_prop_sr_track_jumpwall")))
                return 15;
            else if (iParam0 == Funzioni.HashInt("stt_prop_hoop_tyre_01a") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_sml1") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_mdm1") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_lrg11") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_xl1") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_sml2") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_mdm2") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_lrg2") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_xl2") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_sml3") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_mdm3") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_lrg3") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_xl3") || iParam0 == Funzioni.HashInt("sr_prop_special_bblock_xl3_fixed") || iParam0 == Funzioni.HashInt("ar_prop_ar_bblock_huge_01") || iParam0 == Funzioni.HashInt("vw_prop_vw_bblock_huge_01") || iParam0 == Funzioni.HashInt("ar_prop_ar_bblock_huge_02") || iParam0 == Funzioni.HashInt("vw_prop_vw_bblock_huge_02") || iParam0 == Funzioni.HashInt("ar_prop_ar_bblock_huge_03") || iParam0 == Funzioni.HashInt("vw_prop_vw_bblock_huge_03") || iParam0 == Funzioni.HashInt("as_prop_as_bblock_huge_04") || iParam0 == Funzioni.HashInt("vw_prop_vw_bblock_huge_04") || iParam0 == Funzioni.HashInt("as_prop_as_bblock_huge_05") || iParam0 == Funzioni.HashInt("vw_prop_vw_bblock_huge_05") || iParam0 == Funzioni.HashInt("ar_prop_ar_stunt_block_01a") || iParam0 == Funzioni.HashInt("ar_prop_ar_stunt_block_01b") || iParam0 == Funzioni.HashInt("ar_prop_ar_arrow_thin_m") || iParam0 == Funzioni.HashInt("ar_prop_ar_arrow_wide_m") || iParam0 == Funzioni.HashInt("ar_prop_ar_arrow_thin_l") || iParam0 == Funzioni.HashInt("ar_prop_ar_arrow_wide_l") || iParam0 == Funzioni.HashInt("ar_prop_ar_arrow_thin_xl") || iParam0 == Funzioni.HashInt("ar_prop_ar_arrow_wide_xl") || iParam0 == Funzioni.HashInt("ar_prop_ar_hoop_med_01"))
                return 10;
            else if ((iParam0 == Funzioni.HashInt("xs_prop_arena_goal") || iParam0 == -296431790) || (iParam0 == Funzioni.HashInt("xs_prop_arena_goal_sf")) || (iParam0 == Funzioni.HashInt("xs_prop_arena_tower_02a")))
                return 7;
            else if (iParam0 == -1154284456 || iParam0 == -566149868 || iParam0 == -1532981448 || iParam0 == 1132021903 || iParam0 == 2086127383 || iParam0 == 1218934150 || iParam0 == 1992218160)
                return 8;
            else if (iParam0 == Funzioni.HashInt("stt_prop_corner_sign_01") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_02") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_03") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_04") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_05") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_06") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_07") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_08") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_09") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_10") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_11") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_12") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_13") || iParam0 == Funzioni.HashInt("stt_prop_corner_sign_14") || iParam0 == Funzioni.HashInt("ch_prop_pit_sign_01a") || iParam0 == Funzioni.HashInt("sum_prop_ac_pit_sign_left")
                || (iParam0 == Funzioni.HashInt("xm_prop_x17_cover_01")))
                return 3;
            else if (iParam0 == Funzioni.HashInt("sm_prop_smug_cover_01a"))
                return 9;
            return 0;
        }
        #endregion
    }
}
