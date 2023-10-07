using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions
{
    static class Letti
    {
        private static LettoMid LettoMid = new LettoMid();
        private static LettoLow LettoLow = new LettoLow();
        private static LettoHigh LettoHigh = new LettoHigh();

        private static Vector3 coord = new Vector3();
        private static Vector3 rot = new Vector3();

        public static async Task ControlloLetti()
        {

            Ped p = Cache.PlayerCache.MyPlayer.Ped;
            if (!IsEntityPlayingAnim(PlayerPedId(), "mp_bedmid", "f_getin_l_bighouse", 2) &&
                !IsEntityPlayingAnim(PlayerPedId(), "mp_bedmid", "f_getin_r_bighouse", 2) &&
                !IsEntityPlayingAnim(PlayerPedId(), "mp_bedmid", "f_getout_l_bighouse", 2) &&
                !IsEntityPlayingAnim(PlayerPedId(), "mp_bedmid", "f_getout_r_bighouse", 2))
            {
                if (p.IsInRangeOf(LettoMid.vLocal_338, 1.3f))
                {
                    if (!LettoMid.ALetto)
                    {
                        HUD.ShowHelp(GetLabelText("SA_BED_IN"));
                        if (Input.IsControlJustPressed(Control.Context))
                            LettoMid.Sdraiati();
                    }
                    else
                    {
                        HUD.ShowHelp(GetLabelText("SA_BED_OUT") + "Premi ~INPUT_FRONTEND_X~ per cambiare personaggio.");
                        if (Input.IsControlJustPressed(Control.ScriptRUp))
                            LettoMid.ScendiDalLetto();
                        else if (Input.IsControlJustPressed(Control.FrontendX))
                            CambiaPers();
                    }
                }
                if (p.IsInRangeOf(LettoLow.vLocal_343, 1.3f))
                {
                    if (!LettoLow.ALetto)
                    {
                        HUD.ShowHelp(GetLabelText("SA_BED_IN"));
                        if (Input.IsControlJustPressed(Control.Context))
                            LettoLow.Sdraiati();
                    }
                    else
                    {
                        HUD.ShowHelp(GetLabelText("SA_BED_OUT") + "Premi ~INPUT_FRONTEND_X~ per cambiare personaggio.");
                        if (Input.IsControlJustPressed(Control.ScriptRUp))
                            LettoLow.ScendiDalLetto();
                        else if (Input.IsControlJustPressed(Control.FrontendX))
                            CambiaPers();
                    }
                }

                for (int i = 0; i < LettoHigh.Lista.Count; i++)
                {
                    if (p.IsInRangeOf(LettoHigh.Lista[i].Destra1, 2f) && !LettoHigh.ALettoDestra && !LettoHigh.ALettoSinistra)
                    {
                        LettoHigh.Lista[i].RotAnim = LettoHigh.Lista[i].RotAnimStaticDestra;
                        LettoHigh.Lista[i].CoordAnim = LettoHigh.Lista[i].CoordsAnimStaticDestra;
                        coord = LettoHigh.Lista[i].CoordAnim;
                        rot = LettoHigh.Lista[i].RotAnim;
                        HUD.ShowHelp(GetLabelText("SA_BED_IN"));
                        if (Input.IsControlJustPressed(Control.Context))
                            LettoHigh.Sdraiati(LettoHigh.Lista[i], true);
                    }
                    else if (p.IsInRangeOf(LettoHigh.Lista[i].Sinistra1, 2f) && !LettoHigh.ALettoDestra && !LettoHigh.ALettoSinistra)
                    {
                        LettoHigh.Lista[i].RotAnim = LettoHigh.Lista[i].RotAnimStaticSinistra;
                        LettoHigh.Lista[i].CoordAnim = LettoHigh.Lista[i].CoordsAnimStaticSinistra;
                        coord = LettoHigh.Lista[i].CoordAnim;
                        rot = LettoHigh.Lista[i].RotAnim;
                        HUD.ShowHelp(GetLabelText("SA_BED_IN"));
                        if (Input.IsControlJustPressed(Control.Context))
                            LettoHigh.Sdraiati(LettoHigh.Lista[i], false);
                    }
                    else if (LettoHigh.ALettoDestra || LettoHigh.ALettoSinistra)
                    {
                        HUD.ShowHelp(GetLabelText("SA_BED_OUT") + "Premi ~INPUT_FRONTEND_X~ per cambiare personaggio.");
                        if (Input.IsControlJustPressed(Control.ScriptRUp))
                            LettoHigh.ScendiDalLetto(new Vector3[2] { coord, rot }, LettoHigh.ALettoDestra ? true : false);
                        else if (Input.IsControlJustPressed(Control.FrontendX))
                            CambiaPers();
                    }
                }
            }
        }

        private static async void CambiaPers()
        {
            MenuHandler.CloseAndClearHistory();
            Vector4 Random = LogIn.LogIn.SelectFirstCoords[new Random(GetGameTimer()).Next(LogIn.LogIn.SelectFirstCoords.Count - 1)];
            int switchType = GetIdealPlayerSwitchType(Cache.PlayerCache.MyPlayer.Ped.Position.X, Cache.PlayerCache.MyPlayer.Ped.Position.Y, Cache.PlayerCache.MyPlayer.Ped.Position.Z, Random.X, Random.Y, Random.Z);
            SwitchOutPlayer(PlayerPedId(), 1 | 32 | 128 | 16384, switchType);
            Screen.LoadingPrompt.Show("Caricamento", LoadingSpinnerType.Clockwise1);
            await BaseScript.Delay(3000);

            Cache.PlayerCache.MyPlayer.Ped.Position = new Vector3(Random.X, Random.Y, Random.Z - 1);
            Cache.PlayerCache.MyPlayer.Ped.Heading = Random.W;
            await Cache.PlayerCache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
            Cache.PlayerCache.MyPlayer.Ped.Style.SetDefaultClothes();
            while (!await Cache.PlayerCache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01))) await BaseScript.Delay(50);
            if (Screen.LoadingPrompt.IsActive)
                Screen.LoadingPrompt.Hide();

            SwitchInPlayer(PlayerPedId());

            Cache.PlayerCache.MyPlayer.Status.Instance.Istanzia("Ingresso");
            await BaseScript.Delay(100);
            //Cache.PlayerCache.MyPlayer.Player.State.Set("Pausa", new { Attivo = false }, true);
            Cache.PlayerCache.MyPlayer.Ped.IsVisible = false;
            Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;
            Camera charSelectionCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
            SetGameplayCamRelativeHeading(0);
            charSelectionCam.Position = GetOffsetFromEntityInWorldCoords(Cache.PlayerCache.MyPlayer.Ped.Handle, 0f, -2, 0);
            charSelectionCam.PointAt(Cache.PlayerCache.MyPlayer.Ped);
            charSelectionCam.IsActive = true;
            RenderScriptCams(true, false, 0, false, false);
            while (IsPlayerSwitchInProgress()) await BaseScript.Delay(10);
            LogIn.LogIn.Attiva();
        }
    }

    internal class LettoBase
    {
        public virtual async void Sdraiati()
        {

        }
        public virtual async void ScendiDalLetto()
        {

        }

        protected bool NoPGVicini(Vector3 n1, Vector3 n2)
        {
            Ped[] peds = World.GetAllPeds();
            if (peds.Length > 0)
            {
                int i;
                for (i = 0; i <= 7; i++)
                    if (!peds[i].IsInjured)
                        if (IsEntityInAngledArea(peds[i].Handle, n1.X, n1.Y, n1.Z, n2.X, n2.Y, n2.Z, 2f, false, true, 0) && peds[i].Handle != PlayerPedId())
                            return false;
            }
            return true;
        }

        protected bool Controllo1(Vector3 vParam0, float fParam3)
        {
            if (IsPlayerPlaying(PlayerId()))
                if (!IsPedInAnyVehicle(PlayerPedId(), false) && !IsEntityOnFire(PlayerPedId()) && IsPlayerControlOn(PlayerId()))
                    if (!IsExplosionInSphere(-1, vParam0.X, vParam0.Y, vParam0.Z, 2f))
                        if (IsGameplayCamRendering() && !IsCinematicCamRendering())
                            if (Controllo2(Cache.PlayerCache.MyPlayer.Ped.Position, vParam0, fParam3, false))
                                return true;
            return false;
        }

        protected bool Controllo2(Vector3 vParam0, Vector3 vParam3, float fParam6, bool bParam7)
        {
            if (fParam6 < 0f)
                fParam6 = 0f;
            if (!bParam7)
            {
                if (Math.Abs(vParam0.X - vParam3.X) <= fParam6)
                    if (Math.Abs(vParam0.Y - vParam3.Y) <= fParam6)
                        if (Math.Abs(vParam0.Z - vParam3.Z) <= fParam6)
                            return true;
            }
            else if (Math.Abs(vParam0.X - vParam3.X) <= fParam6)
            {
                if (Math.Abs(vParam0.Y - vParam3.Y) <= fParam6)
                    return true;
            }
            return false;
        }
    }

    internal class LettoMid : LettoBase
    {
        public string sLocal_334 = "mp_bedmid";
        public string sLocal_335 = "f_getin_l_bighouse";
        public string sLocal_336 = "f_sleep_l_loop_bighouse";
        public string sLocal_337 = "f_getout_l_bighouse";
        public Vector3 vLocal_338 = new Vector3(349.9853f, -997.8344f, -99.1952f);
        public Vector3 vLocal_342 = new Vector3(349f, -997.3587f, -100.5f);
        public Vector3 vLocal_345 = new Vector3(351.74f, -997.3587f, -97f);
        public Vector3 vLocal_348 = new Vector3(349.66f, -996.183f, -99.764f);
        public Vector3 vLocal_351 = new Vector3(0f, 0f, -3.96f);
        public int uLocal_331 = 0;
        public int uLocal_332 = 0;
        public float fLocal_341 = 43.8287f;
        public bool ALetto = false;

        public override async void Sdraiati()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;
            RequestAnimDict("mp_bedmid");
            while (!HasAnimDictLoaded("mp_bedmid")) await BaseScript.Delay(0);
            Vector3 vVar0 = new Vector3(1.5f);
            Vector3 vVar3;
            if (GetFollowPedCamViewMode() == 4)
                SetFollowPedCamViewMode(1);


            if (Controllo1(vLocal_338, vVar0.X) && IsEntityInAngledArea(PlayerPedId(), vLocal_342.X, vLocal_342.Y, vLocal_342.Z, vLocal_345.X, vLocal_345.Y, vLocal_345.Z, 2f, false, true, 0) && NoPGVicini(vLocal_342, vLocal_345))
            {
                p.Weapons.Select(WeaponHash.Unarmed);
                vLocal_338 = GetAnimInitialOffsetPosition(sLocal_334, sLocal_335, vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 0, 2);
                vVar3 = GetAnimInitialOffsetRotation(sLocal_334, sLocal_335, vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 0, 2);
                fLocal_341 = vVar3.Z;
                TaskGoStraightToCoord(PlayerPedId(), vLocal_338.X, vLocal_338.Y, vLocal_338.Z, 1f, 5000, fLocal_341, 0.05f);

                await BaseScript.Delay(2000);

                if (GetFollowPedCamViewMode() == 4)
                    SetFollowPedCamViewMode(1);

                uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, false, 1065353216, 0, 1065353216);
                NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_335, 4f, -2f, 261, 0, 1148846080, 0);
                NetworkStartSynchronisedScene(uLocal_331);

                await BaseScript.Delay(1000);

                uLocal_332 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_331);

                while (GetSynchronizedScenePhase(uLocal_332) < 0.9f) await BaseScript.Delay(1);

                uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, true, 1065353216, 0, 1065353216);
                NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_336, 8f, -2f, 261, 0, 1148846080, 0);
                NetworkStartSynchronisedScene(uLocal_331);

                uLocal_332 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_331);
                SetSynchronizedSceneLooped(uLocal_332, true);
                ALetto = true;
                RemoveAnimDict("mp_bedmid");
            }
            else
            {
                HUD.ShowNotification("errore nello script letti\"LettoMid\", segnalalo allo scripter", ColoreNotifica.Red, true);
            }
        }

        public override async void ScendiDalLetto()
        {
            RequestAnimDict("mp_bedmid");
            while (!HasAnimDictLoaded("mp_bedmid")) await BaseScript.Delay(0);
            uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, true, 1065353216, 0, 1065353216);
            NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_336, 8f, -2f, 261, 0, 1148846080, 0);
            NetworkStartSynchronisedScene(uLocal_331);

            uLocal_331 = NetworkCreateSynchronisedScene(vLocal_348.X, vLocal_348.Y, vLocal_348.Z, vLocal_351.X, vLocal_351.Y, vLocal_351.Z, 2, false, false, 1065353216, 0, 1065353216);
            NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_331, sLocal_334, sLocal_337, 1000f, -2f, 261, 0, 1148846080, 0);
            NetworkStartSynchronisedScene(uLocal_331);

            ALetto = false;
            RemoveAnimDict("mp_bedmid");
        }
    }

    internal class LettoLow : LettoBase
    {
        public string sLocal_333 = "mp_bedmid";
        public string sLocal_334 = "f_getin_l_bighouse";
        public string sLocal_335 = "f_sleep_l_loop_bighouse";
        public string sLocal_336 = "f_getout_l_bighouse";
        public Vector3 vLocal_337 = new Vector3(262.9207f, -1002.98f, -100.0086f);
        public Vector3 vLocal_340 = new Vector3(261.0173f, -1002.98f, -98.0086f);
        public Vector3 vLocal_343 = new Vector3(261.8297f, -1002.928f, -99.0062f);
        public float fLocal_346 = 230.5943f;
        public Vector3 vLocal_347 = new Vector3(262.74f, -1004.344f, -99.575f);
        public Vector3 vLocal_350 = new Vector3(0f, 0f, -162.36f);
        public int uLocal_330 = 0;
        public int uLocal_331 = 0;
        public bool ALetto = false;

        public async override void Sdraiati()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;
            RequestAnimDict("mp_bedmid");
            while (!HasAnimDictLoaded("mp_bedmid")) await BaseScript.Delay(0);
            p.Weapons.Select(WeaponHash.Unarmed);
            Vector3 vVar0 = new Vector3(1.5f);
            Vector3 vVar3;
            if (GetFollowPedCamViewMode() == 4)
                SetFollowPedCamViewMode(1);

            if (Controllo1(vLocal_343, vVar0.X) && IsEntityInAngledArea(PlayerPedId(), vLocal_337.X, vLocal_337.Y, vLocal_337.Z, vLocal_340.X, vLocal_340.Y, vLocal_340.Z, 2f, false, true, 0) && NoPGVicini(vLocal_337, vLocal_340))
            {
                vLocal_343 = GetAnimInitialOffsetPosition(sLocal_333, sLocal_334, vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 0, 2);
                vVar3 = GetAnimInitialOffsetRotation(sLocal_333, sLocal_334, vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 0, 2);
                fLocal_346 = vVar3.Z;
                TaskGoStraightToCoord(PlayerPedId(), vLocal_343.X, vLocal_343.Y, vLocal_343.Z, 1f, 5000, fLocal_346, 0.05f);
                await BaseScript.Delay(2000);

                uLocal_330 = NetworkCreateSynchronisedScene(vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 2, false, false, 1065353216, 0, 1065353216);
                NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_330, sLocal_333, sLocal_334, 4f, -2f, 261, 0, 1148846080, 0);
                NetworkStartSynchronisedScene(uLocal_330);

                await BaseScript.Delay(1000);

                uLocal_331 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_330);

                while (GetSynchronizedScenePhase(uLocal_331) < 0.9f) await BaseScript.Delay(1);

                uLocal_330 = NetworkCreateSynchronisedScene(vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 2, false, true, 1065353216, 0, 1065353216);
                NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_330, sLocal_333, sLocal_335, 8f, -2f, 261, 0, 1148846080, 0);
                NetworkStartSynchronisedScene(uLocal_330);

                uLocal_331 = NetworkConvertSynchronisedSceneToSynchronizedScene(uLocal_330);
                SetSynchronizedSceneLooped(uLocal_331, true);
                ALetto = true;
                RemoveAnimDict("mp_bedmid");
            }
            else
                HUD.ShowNotification("errore nello script letti \"LettoLow\", segnalalo allo scripter", ColoreNotifica.Red, true);
        }

        public async override void ScendiDalLetto()
        {
            RequestAnimDict("mp_bedmid");
            while (!HasAnimDictLoaded("mp_bedmid")) await BaseScript.Delay(0);
            uLocal_330 = NetworkCreateSynchronisedScene(vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 2, false, false, 1065353216, 0, 1065353216);
            NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_330, sLocal_333, sLocal_336, 2f, -2f, 261, 0, 1148846080, 0);
            NetworkStartSynchronisedScene(uLocal_330);

            uLocal_330 = NetworkCreateSynchronisedScene(vLocal_347.X, vLocal_347.Y, vLocal_347.Z, vLocal_350.X, vLocal_350.Y, vLocal_350.Z, 2, false, false, 1065353216, 0, 1065353216);
            NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_330, sLocal_333, sLocal_336, 1000f, -2f, 261, 0, 1148846080, 0);
            NetworkStartSynchronisedScene(uLocal_330);

            ALetto = false;
            RemoveAnimDict("mp_bedmid");
        }
    }

    internal class LettoHigh : LettoBase
    {
        public string sLocal_413 = "mp_bedmid";
        public bool ALettoDestra = false;
        public bool ALettoSinistra = false;

        private string func_234(bool iParam0) => !iParam0 ? "f_getin_l_bighouse" : "f_getin_r_bighouse";
        private string func_9(bool iParam0) => !iParam0 ? "f_getout_l_bighouse" : "f_getout_r_bighouse";
        private string func_272(bool iParam0) => !iParam0 ? "f_sleep_l_loop_bighouse" : "f_sleep_r_loop_bighouse";

        public class LettiCoordsAnim
        {
            public Vector3 Sinistra1 = new Vector3(0);
            public Vector3 Sinistra2 = new Vector3(0);
            public Vector3 CoordsAnimStaticDestra = new Vector3(0);
            public Vector3 RotAnimStaticDestra = new Vector3(0);
            public Vector3 CoordsAnimStaticSinistra = new Vector3(0);
            public Vector3 RotAnimStaticSinistra = new Vector3(0);
            public Vector3 Destra1 = new Vector3(0);
            public Vector3 Destra2 = new Vector3(0);
            public Vector3 CoordAnim = new Vector3(0);
            public Vector3 RotAnim = new Vector3(0);
            public Vector3 vLocal_438 = new Vector3(0);

            public float fLocal_402;

            public int uLocal_409;
            public int uLocal_410;
        }

        public List<LettiCoordsAnim> Lista = new List<LettiCoordsAnim>()
        {
            new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(),
            new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(),
            new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(),
            new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(),
            new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(),
            new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(), new LettiCoordsAnim(),
        };

        public LettoHigh()
        {
            #region letto 1
            Lista[0].Sinistra1 = new Vector3(-796.3056f, 337.3367f, 202.4136f);
            Lista[0].Sinistra2 = new Vector3(-793.9697f, 337.3367f, 200.4136f);

            Lista[0].CoordsAnimStaticSinistra = new Vector3(-795.8910f, 338.6630f, 200.8270f);
            Lista[0].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -5.7600f);

            //////////////////////////////////////////////////////////////////

            Lista[0].Destra1 = new Vector3(-794.1949f, 339.9690f, 200.4136f);
            Lista[0].Destra2 = new Vector3(-796.4470f, 339.9556f, 202.4136f);

            Lista[0].CoordsAnimStaticDestra = new Vector3(-794.9370f, 340.6300f, 201.4280f);
            Lista[0].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 129.2400f);
            #endregion

            #region letto 2
            Lista[1].Sinistra1 = new Vector3(126.4232f, 545.7162f, 179.5227f);
            Lista[1].Sinistra2 = new Vector3(125.1505f, 546.0822f, 180.5208f);

            Lista[1].CoordsAnimStaticSinistra = new Vector3(125.6500f, 544.4750f, 179.9700f);
            Lista[1].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -177.8400f);

            //////////////////////////////////////////////////////////////////

            Lista[1].Destra1 = new Vector3(126.7189f, 542.8882f, 179.5227f);
            Lista[1].Destra2 = new Vector3(124.6794f, 542.6829f, 181.5227f);

            Lista[1].CoordsAnimStaticDestra = new Vector3(124.8870f, 541.9250f, 180.5120f);
            Lista[1].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, -41.7600f);
            #endregion

            #region letto 3
            Lista[2].Sinistra1 = new Vector3(-796.3056f, 337.3367f, 202.4136f);
            Lista[2].Sinistra2 = new Vector3(-793.9697f, 337.3367f, 200.4136f);

            Lista[2].CoordsAnimStaticSinistra = new Vector3(-795.8910f, 338.6630f, 200.8270f);
            Lista[2].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -5.7600f);

            //////////////////////////////////////////////////////////////////

            Lista[2].Destra1 = new Vector3(-794.1949f, 339.9690f, 200.4136f);
            Lista[2].Destra2 = new Vector3(-796.4470f, 339.9556f, 202.4136f);

            Lista[2].CoordsAnimStaticDestra = new Vector3(-794.9370f, 340.6300f, 201.4280f);
            Lista[2].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 129.2400f);
            #endregion

            #region letto 4

            Lista[3].Sinistra1 = new Vector3(-792.4382f, 332.6826f, 209.7966f);
            Lista[3].Sinistra2 = new Vector3(-794.6772f, 332.6597f, 211.7966f);

            Lista[3].CoordsAnimStaticSinistra = new Vector3(-793.5940f, 333.7590f, 210.2250f);
            Lista[3].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -1.0000f);

            //////////////////////////////////////////////////////////////////

            Lista[3].Destra1 = new Vector3(-794.6525f, 335.1653f, 209.7966f);
            Lista[3].Destra2 = new Vector3(-792.5333f, 335.1687f, 211.7966f);

            Lista[3].CoordsAnimStaticDestra = new Vector3(-792.6250f, 335.8620f, 210.8130f);
            Lista[3].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 133.2000f);
            #endregion

            #region letto 5
            Lista[4].Sinistra1 = new Vector3(-162.6263f, 484.8703f, 132.8697f);
            Lista[4].Sinistra2 = new Vector3(-164.7478f, 484.4771f, 134.8697f);

            Lista[4].CoordsAnimStaticSinistra = new Vector3(-163.4570f, 483.4740f, 133.2820f);
            Lista[4].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -166.6800f);

            //////////////////////////////////////////////////////////////////

            Lista[4].Destra1 = new Vector3(-162.3055f, 482.5231f, 132.8697f);
            Lista[4].Destra2 = new Vector3(-164.4513f, 482.0749f, 134.8697f);

            Lista[4].CoordsAnimStaticDestra = new Vector3(-163.9880f, 481.3370f, 133.8630f);
            Lista[4].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, -31.6800f);
            #endregion

            #region letto 6 
            Lista[5].Sinistra1 = new Vector3(-796.5303f, 334.3555f, 189.7135f);
            Lista[5].Sinistra2 = new Vector3(-796.5093f, 336.8433f, 191.7135f);

            Lista[5].CoordsAnimStaticSinistra = new Vector3(-797.6820f, 335.6830f, 190.1550f);
            Lista[5].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 90f);

            //////////////////////////////////////////////////////////////////

            Lista[5].Destra1 = new Vector3(-799.1369f, 334.6384f, 189.7135f);
            Lista[5].Destra2 = new Vector3(-799.1272f, 336.8023f, 191.7135f);

            Lista[5].CoordsAnimStaticDestra = new Vector3(-799.7870f, 336.5630f, 190.7500f);
            Lista[5].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, -133.2000f);
            #endregion

            #region letto 7 
            Lista[6].Sinistra1 = new Vector3(-1486.2543f, -3750.0811f, 4.9114f);
            Lista[6].Sinistra2 = new Vector3(-1484.0914f, -3750.7080f, 6.9114f);

            Lista[6].CoordsAnimStaticSinistra = new Vector3(-1485.1100f, -3749.3369f, 5.3490f);
            Lista[6].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -20.1600f);

            //////////////////////////////////////////////////////////////////

            Lista[6].Destra1 = new Vector3(-1485.3296f, -3747.1462f, 4.9114f);
            Lista[6].Destra2 = new Vector3(-1483.2997f, -3747.8845f, 6.9114f);

            Lista[6].CoordsAnimStaticDestra = new Vector3(-1483.2990f, -3747.2151f, 5.9150f);
            Lista[6].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 110.5200f);
            #endregion

            #region letto 8
            Lista[7].Sinistra1 = new Vector3(-37.348f, -577.829f, 83.908f);
            Lista[7].Sinistra2 = new Vector3(0);

            Lista[7].CoordsAnimStaticSinistra = new Vector3(-36.505f, -576.744f, 83.325f);
            Lista[7].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -5.7600f);

            //////////////////////////////////////////////////////////////////

            Lista[7].Destra1 = new Vector3(-36.378f, -575.181f, 83.908f);
            Lista[7].Destra2 = new Vector3(0);

            Lista[7].CoordsAnimStaticDestra = new Vector3(-35.212f, -574.873f, 83.908f);
            Lista[7].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 129.2400f);
            #endregion

            #region letto 9
            Lista[8].Sinistra1 = new Vector3(-1471.651f, -533.368f, 50.722f);
            Lista[8].Sinistra2 = new Vector3();

            Lista[8].CoordsAnimStaticSinistra = new Vector3(-1472.191f, -532.086f, 50.180f);
            Lista[8].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 25.108f);

            //////////////////////////////////////////////////////////////////

            Lista[8].Destra1 = new Vector3(-1473.288f, -530.835f, 50.722f);
            Lista[8].Destra2 = new Vector3();

            Lista[8].CoordsAnimStaticDestra = new Vector3(-1472.49f, -530.079f, 50.722f);
            Lista[8].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 160.108f);
            #endregion

            #region letto 10
            Lista[9].Sinistra1 = new Vector3(332.739f, 423.586f, 145.597f);
            Lista[9].Sinistra2 = new Vector3();

            Lista[9].CoordsAnimStaticSinistra = new Vector3(331.201f, 423.663f, 145.027f);
            Lista[9].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 110.858f);

            //////////////////////////////////////////////////////////////////

            Lista[9].Destra1 = new Vector3(330.177f, 422.396f, 145.597f);
            Lista[9].Destra2 = new Vector3();

            Lista[9].CoordsAnimStaticDestra = new Vector3(329.052f, 423.713f, 145.597f);
            Lista[9].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 245.858f);
            #endregion

            #region letto 11
            Lista[10].Sinistra1 = new Vector3(-666.368f, 586.985f, 141.596f);
            Lista[10].Sinistra2 = new Vector3();

            Lista[10].CoordsAnimStaticSinistra = new Vector3(-666.108f, 585.521f, 140.980f);
            Lista[10].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -148.091f);

            //////////////////////////////////////////////////////////////////

            Lista[10].Destra1 = new Vector3(-664.561f, 584.795f, 141.596f);
            Lista[10].Destra2 = new Vector3();

            Lista[10].CoordsAnimStaticDestra = new Vector3(-665.775f, 583.444f, 141.596f);
            Lista[10].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, -13.091f);
            #endregion

            #region letto 12
            Lista[11].Sinistra1 = new Vector3(-769.533f, 606.518f, 140.357f);
            Lista[11].Sinistra2 = new Vector3();

            Lista[11].CoordsAnimStaticSinistra = new Vector3(-771.068f, 606.61f, 139.7f);
            Lista[11].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 100.65f);

            //////////////////////////////////////////////////////////////////

            Lista[11].Destra1 = new Vector3(-772.254f, 605.558f, 140.357f);
            Lista[11].Destra2 = new Vector3();

            Lista[11].CoordsAnimStaticDestra = new Vector3(-773.032f, 607.057f, 140.357f);
            Lista[11].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 235.7f);
            #endregion

            #region letto 13
            Lista[12].Sinistra1 = new Vector3(377.11f, 407.497f, 142.126f);
            Lista[12].Sinistra2 = new Vector3();

            Lista[12].CoordsAnimStaticSinistra = new Vector3(376.15f, 406.189f, 141.55f);
            Lista[12].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 158.254f);

            //////////////////////////////////////////////////////////////////

            Lista[12].Destra1 = new Vector3(376.384f, 404.328f, 142.126f);
            Lista[12].Destra2 = new Vector3();

            Lista[12].CoordsAnimStaticDestra = new Vector3(374.698f, 404.407f, 142.126f);
            Lista[12].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 293.254f);
            #endregion

            #region letto 14
            Lista[13].Sinistra1 = new Vector3(-851.342f, 677.081f, 149.078f);
            Lista[13].Sinistra2 = new Vector3();

            Lista[13].CoordsAnimStaticSinistra = new Vector3(-851.938f, 675.505f, 148.48f);
            Lista[13].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 175.652f);

            //////////////////////////////////////////////////////////////////

            Lista[13].Destra1 = new Vector3(-851.172f, 673.765f, 149.078f);
            Lista[13].Destra2 = new Vector3();

            Lista[13].CoordsAnimStaticDestra = new Vector3(-852.672f, 673.127f, 149.078f);
            Lista[13].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 310.652f);
            #endregion

            #region letto 15
            Lista[14].Sinistra1 = new Vector3(-1282.379f, 434.784f, 94.12f);
            Lista[14].Sinistra2 = new Vector3();

            Lista[14].CoordsAnimStaticSinistra = new Vector3(-1283.074f, 433.368f, 93.55f);
            Lista[14].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 173.296f);

            //////////////////////////////////////////////////////////////////

            Lista[14].Destra1 = new Vector3(-1282.58f, 431.521f, 94.12f);
            Lista[14].Destra2 = new Vector3();

            Lista[14].CoordsAnimStaticDestra = new Vector3(-1283.985f, 431.201f, 94.12f);
            Lista[14].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 308.296f);
            #endregion

            #region letto 16
            Lista[15].Sinistra1 = new Vector3(-1454.105f, -553.051f, 72.844f);
            Lista[15].Sinistra2 = new Vector3();

            Lista[15].CoordsAnimStaticSinistra = new Vector3(-1455.779f, -553.443f, 72.244f);
            Lista[15].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 116.586f);

            //////////////////////////////////////////////////////////////////

            Lista[15].Destra1 = new Vector3(-1456.505f, -554.852f, 72.844f);
            Lista[15].Destra2 = new Vector3();

            Lista[15].CoordsAnimStaticDestra = new Vector3(-1457.986f, -553.75f, 72.844f);
            Lista[15].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 251.586f);
            #endregion

            #region letto 17
            Lista[16].Sinistra1 = new Vector3(-900.271f, -368.65f, 113.074f);
            Lista[16].Sinistra2 = new Vector3();

            Lista[16].CoordsAnimStaticSinistra = new Vector3(-900.311f, -370.319f, 112.41f);
            Lista[16].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -161.872f);

            //////////////////////////////////////////////////////////////////

            Lista[16].Destra1 = new Vector3(-898.859f, -371.413f, 113.074f);
            Lista[16].Destra2 = new Vector3();

            Lista[16].CoordsAnimStaticDestra = new Vector3(-900.623f, -372.368f, 113.074f);
            Lista[16].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, -26.872f);
            #endregion

            #region letto 18
            Lista[17].Sinistra1 = new Vector3(-593.632f, 50.102f, 97.000f);
            Lista[17].Sinistra2 = new Vector3();

            Lista[17].CoordsAnimStaticSinistra = new Vector3(-594.598f, 48.648f, 96.42f);
            Lista[17].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 173.93f);

            //////////////////////////////////////////////////////////////////

            Lista[17].Destra1 = new Vector3(-593.862f, 47.186f, 97.000f);
            Lista[17].Destra2 = new Vector3();

            Lista[17].CoordsAnimStaticDestra = new Vector3(-595.587f, 47.005f, 97.000f);
            Lista[17].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 308.93f);
            #endregion

            #region letto 19
            Lista[18].Sinistra1 = new Vector3(-794.366f, 332.413f, 210.797f);
            Lista[18].Sinistra2 = new Vector3();

            Lista[18].CoordsAnimStaticSinistra = new Vector3(-793.489f, 333.92f, 210.15f);
            Lista[18].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -5.7600f);

            //////////////////////////////////////////////////////////////////

            Lista[18].Destra1 = new Vector3(-794.36f, 333.798f, 210.796f);
            Lista[18].Destra2 = new Vector3();

            Lista[18].CoordsAnimStaticDestra = new Vector3(-792.535f, 335.515f, 210.796f);
            Lista[18].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 129.2400f);
            #endregion

            #region letto 20
            Lista[19].Sinistra1 = new Vector3(-37.216f, -583.865f, 78.83f);
            Lista[19].Sinistra2 = new Vector3();

            Lista[19].CoordsAnimStaticSinistra = new Vector3(-35.778f, -582.484f, 78.19f);
            Lista[19].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -26.482f);

            //////////////////////////////////////////////////////////////////

            Lista[19].Destra1 = new Vector3(-36.00f, -580.439f, 78.83f);
            Lista[19].Destra2 = new Vector3();

            Lista[19].CoordsAnimStaticDestra = new Vector3(-34.211f, -581.406f, 78.83f);
            Lista[19].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 109.482f); // 173.482f
            #endregion

            #region letto 21
            Lista[20].Sinistra1 = new Vector3(-593.862f, 50.457f, -183.581f);
            Lista[20].Sinistra2 = new Vector3();

            Lista[20].CoordsAnimStaticSinistra = new Vector3(-594.556f, 48.673f, -184.24f);
            Lista[20].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 176.435f);

            //////////////////////////////////////////////////////////////////

            Lista[20].Destra1 = new Vector3(-593.975f, 46.781f, -183.582f);
            Lista[20].Destra2 = new Vector3();

            Lista[20].CoordsAnimStaticDestra = new Vector3(-595.67f, 46.941f, -183.582f);
            Lista[20].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 311.435f);
            #endregion

            #region letto 22
            Lista[21].Sinistra1 = new Vector3(-796.183f, 334.538f, 220.438f);
            Lista[21].Sinistra2 = new Vector3();

            Lista[21].CoordsAnimStaticSinistra = new Vector3(-797.814f, 335.773f, 219.860f);
            Lista[21].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 90f);

            //////////////////////////////////////////////////////////////////

            Lista[21].Destra1 = new Vector3(-799.276f, 334.683f, 220.438f);
            Lista[21].Destra2 = new Vector3();

            Lista[21].CoordsAnimStaticDestra = new Vector3(-799.754f, 336.443f, 220.438f);
            Lista[21].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, -133.2000f);
            #endregion

            #region letto 23
            Lista[22].Sinistra1 = new Vector3(-764.761f, 323.155f, 199.487f);
            Lista[22].Sinistra2 = new Vector3();

            Lista[22].CoordsAnimStaticSinistra = new Vector3(-763.37f, 322.115f, 198.887f);
            Lista[22].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, -90f);

            //////////////////////////////////////////////////////////////////

            Lista[22].Destra1 = new Vector3(-761.745f, 322.758f, 199.486f);
            Lista[22].Destra2 = new Vector3();

            Lista[22].CoordsAnimStaticDestra = new Vector3(-761.27f, 321.345f, 199.486f);
            Lista[22].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 45f);
            #endregion

            #region letto 24
            Lista[23].Sinistra1 = new Vector3(-759.477f, 319.868f, 170.596f);
            Lista[23].Sinistra2 = new Vector3();

            Lista[23].CoordsAnimStaticSinistra = new Vector3(-759.983f, 318.388f, 169.956f);
            Lista[23].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, 175.966f);

            //////////////////////////////////////////////////////////////////

            Lista[23].Destra1 = new Vector3(-759.535f, 317.094f, 170.596f);
            Lista[23].Destra2 = new Vector3();

            Lista[23].CoordsAnimStaticDestra = new Vector3(-761.023f, 316.715f, 170.596f);
            Lista[23].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, 310.966f);
            #endregion

            /*
				#region letto 
				Lista[].Sinistra1 = new Vector3();
				Lista[].Sinistra2 = new Vector3();

				Lista[].CoordsAnimStaticSinistra = new Vector3();
				Lista[].RotAnimStaticSinistra = new Vector3(0.0000f, 0.0000f, );

				//////////////////////////////////////////////////////////////////

				Lista[].Destra1 = new Vector3();
				Lista[].Destra2 = new Vector3();

				Lista[].CoordsAnimStaticDestra = new Vector3();
				Lista[].RotAnimStaticDestra = new Vector3(0.0000f, 0.0000f, );
				#endregion
			*/



        }

        public async override void Sdraiati() { }
        public async void Sdraiati(LettiCoordsAnim lato, bool destra)
        {
            RequestAnimDict("mp_bedmid");
            while (!HasAnimDictLoaded("mp_bedmid")) await BaseScript.Delay(0);
            Vector3 var0 = lato.CoordAnim;
            Vector3 var1 = lato.RotAnim;

            Cache.PlayerCache.MyPlayer.Ped.Weapons.Select(WeaponHash.Unarmed);
            Vector3 vVar3;
            if (GetFollowPedCamViewMode() == 4)
                SetFollowPedCamViewMode(1);

            lato.vLocal_438 = GetAnimInitialOffsetPosition(sLocal_413, func_234(destra), var0.X, var0.Y, var0.Z, var1.X, var1.Y, var1.Z, 0, 2);
            vVar3 = GetAnimInitialOffsetRotation(sLocal_413, func_234(destra), var0.X, var0.Y, var0.Z, var1.X, var1.Y, var1.Z, 0, 2);
            lato.fLocal_402 = vVar3.Z;
            TaskGoStraightToCoord(PlayerPedId(), var0.X, var0.Y, var0.Z, 1f, 5000, lato.fLocal_402, 0.05f);
            await BaseScript.Delay(2000);

            lato.uLocal_410 = NetworkCreateSynchronisedScene(var0.X, var0.Y, var0.Z, var1.X, var1.Y, var1.Z, 2, false, false, 1065353216, 0, 1065353216);
            NetworkAddPedToSynchronisedScene(PlayerPedId(), lato.uLocal_410, sLocal_413, func_234(destra), 4f, -2f, 261, 0, 1148846080, 0);
            NetworkStartSynchronisedScene(lato.uLocal_410);

            await BaseScript.Delay(1000);

            lato.uLocal_409 = NetworkConvertSynchronisedSceneToSynchronizedScene(lato.uLocal_410);

            while (GetSynchronizedScenePhase(lato.uLocal_409) < 0.9f) await BaseScript.Delay(1);

            lato.uLocal_410 = NetworkCreateSynchronisedScene(var0.X, var0.Y, var0.Z, var1.X, var1.Y, var1.Z, 2, false, true, 1065353216, 0, 1065353216);
            NetworkAddPedToSynchronisedScene(PlayerPedId(), lato.uLocal_410, sLocal_413, func_272(destra), 8f, -2f, 261, 0, 1148846080, 0);
            NetworkStartSynchronisedScene(lato.uLocal_410);

            lato.uLocal_409 = NetworkConvertSynchronisedSceneToSynchronizedScene(lato.uLocal_410);
            SetSynchronizedSceneLooped(lato.uLocal_409, true);
            ALettoDestra = destra;
            ALettoSinistra = !destra;
            RemoveAnimDict("mp_bedmid");
        }

        public async override void ScendiDalLetto() { }
        public async void ScendiDalLetto(Vector3[] coords, bool destra)
        {
            RequestAnimDict("mp_bedmid");
            while (!HasAnimDictLoaded("mp_bedmid")) await BaseScript.Delay(0);
            int uLocal_410 = NetworkCreateSynchronisedScene(coords[0].X, coords[0].Y, coords[0].Z, coords[1].X, coords[1].Y, coords[1].Z, 2, false, false, 1065353216, 0, 1065353216);
            NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_410, sLocal_413, func_9(destra), 2f, -2f, 261, 0, 1148846080, 0);
            NetworkStartSynchronisedScene(uLocal_410);

            uLocal_410 = NetworkCreateSynchronisedScene(coords[0].X, coords[0].Y, coords[0].Z, coords[1].X, coords[1].Y, coords[1].Z, 2, false, false, 1065353216, 0, 1065353216);
            NetworkAddPedToSynchronisedScene(PlayerPedId(), uLocal_410, sLocal_413, func_9(destra), 1000f, -2f, 261, 0, 1148846080, 0);
            NetworkStartSynchronisedScene(uLocal_410);

            ALettoDestra = false;
            ALettoSinistra = false;
            RemoveAnimDict("mp_bedmid");
        }
    }
}
