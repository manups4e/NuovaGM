﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.MODALITA.FREEROAM.Spawner;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.CharCreation
{
    internal static class FreeRoamCamerasFirstTime
    {
        private static Scaleform Credits = new Scaleform("OPENING_CREDITS");
        private static List<Vector4> SpawnPoints = new List<Vector4>
        {
            new Vector4(232.412f, -878.302f, 29.492f, 312.905f),
            new Vector4(216.779f, -1040.771f, 29.140f, 60.056f),
            new Vector4(766.765f, -1024.438f, 24.924f, 323.522f),
            new Vector4(-952.488f, -414.614f, 37.807f, 186.983f)
        };

        public static void Init()
        {
            PrepareMusicEvent("GLOBAL_KILL_MUSIC");
            PrepareMusicEvent("FM_INTRO_START");
            PrepareMusicEvent("FM_INTRO_END");
        }

        public static void Stop()
        {
            CancelMusicEvent("GLOBAL_KILL_MUSIC");
            CancelMusicEvent("FM_INTRO_START");
            CancelMusicEvent("FM_INTRO_END");
        }

        public static async void FirstTimeTransition()
        {
            PlayerCache.MyPlayer.Ped.Detach();
            ScaleformUI.ScaleformUI.Warning.ShowWarningWithButtons("Vuoi vedere il video introduttivo?", "Premi NO e potrai entrare direttamente nel mondo di gioco.", "", new List<InstructionalButton>
            {
                new InstructionalButton(Control.FrontendCancel, "No"),
                new InstructionalButton(Control.FrontendAccept, "Si"),
            });
            await BaseScript.Delay(100);
            Screen.Fading.FadeIn(0);
            ScaleformUI.ScaleformUI.Warning.OnButtonPressed += async (a) =>
            {
                if (a.GamepadButton == Control.FrontendCancel)
                    SiFinisce();
                else if (a.GamepadButton == Control.FrontendAccept)
                {
                    Screen.Fading.FadeOut(500);
                    await BaseScript.Delay(1001);
                    await SiComincia();
                }
            };
        }

        public static async Task SiComincia()
        {
            Screen.Fading.FadeIn(1000);
            Ped playerPed = PlayerCache.MyPlayer.Ped;
            TriggerMusicEvent("FM_INTRO_START");
            playerPed.Detach();
            Client.Instance.AddTick(Controllo);
            Client.Instance.AddTick(Crediti);
            playerPed.IsPositionFrozen = true;
            playerPed.IsVisible = false;
            PlayerCache.MyPlayer.Status.Istanza.Istanzia("IngressoPlayer");
            playerPed.Position = new Vector3(745.877f, 1215.591f, 359.405f);
            Camera Cam1 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f, Position = new Vector3(745.877f, 1215.591f, 359.405f) };
            Cam1.IsActive = true;
            Cam1.PointAt(new Vector3(657.620f, 906.617f, 276.418f));
            while (!HasCollisionLoadedAroundEntity(playerPed.Handle)) await BaseScript.Delay(1000);
            Camera Cam2 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f, Position = new Vector3(738.474f, 1188.959f, 347.068f) };
            Cam2.PointAt(new Vector3(657.620f, 906.617f, 276.418f));
            Cam1.InterpTo(Cam2, 10000, 0, 0);
            Screen.Fading.FadeIn(800);
            await BaseScript.Delay(2000);
            sub_12019("artdir", 0.0f, 350.0f, "left", 0.3f, 0.3f);
            sub_11fbe("artdir", "Founders & Admin", 175.0f, "HUD_COLOUR_FRIENDLY", true);
            sub_11f63("artdir", "NOME1|NOME2|NOME3|NOME4|", 195.0f, "|", true);
            sub_11f32("artdir", 0.16f);
            await BaseScript.Delay(6000);
            sub_11f01("artdir", 0.16f);
            await BaseScript.Delay(1000);
            Screen.Fading.FadeOut(800);
            await BaseScript.Delay(1000);
            playerPed.Position = new Vector3(-241.502f, -534.627f, 148.902f);
            while (!HasCollisionLoadedAroundEntity(playerPed.Handle)) await BaseScript.Delay(1000);
            Cam1.IsActive = false;
            Cam2.IsActive = false;
            Cam1.Delete();
            Cam2.Delete();
            Camera Cam3 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f, Position = new Vector3(-264.303f, -567.568f, 148.302f) };
            Cam3.IsActive = true;
            Cam3.PointAt(new Vector3(-165.131f, -704.744f, 196.705f));
            Camera Cam4 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f, Position = new Vector3(-264.563f, -599.100f, 148.302f) };
            Cam4.PointAt(new Vector3(-165.131f, -704.744f, 196.705f));
            Screen.Fading.FadeIn(800);
            Cam3.InterpTo(Cam4, 7000, 0, 1);
            await BaseScript.Delay(2000);
            sub_12019("animdir", 50.0f, 90.0f, "right", 0.8f, 0.8f);
            sub_11fbe("animdir", "Staff e Moderatori", 0.0f, "HUD_COLOUR_NET_PLAYER1", true);
            sub_11f63("animdir", "Nome1|Nome2|Nome3|Nome4|", 90.0f, "|", true);
            sub_11f32("animdir", 0.16f);
            await BaseScript.Delay(4000);
            sub_11f01("animdir", 0.16f);
            await BaseScript.Delay(1000);
            Screen.Fading.FadeOut(800);
            await BaseScript.Delay(1000);
            Cam3.Delete();
            Cam4.Delete();
            playerPed.Position = new Vector3(-1604.552f, -1048.718f, 17.027f);
            while (!HasCollisionLoadedAroundEntity(playerPed.Handle)) await BaseScript.Delay(1000);
            Camera Cam5 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f, Position = new Vector3(-1593.578f, -1042.522f, 12.527f) };
            Cam5.PointAt(new Vector3(-1604.552f, -1048.718f, 17.027f));
            Cam5.IsActive = true;
            Screen.Fading.FadeIn(800);
            Camera Cam6 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f, Position = new Vector3(-1604.543f, -1037.229f, 12.527f) };
            Cam6.PointAt(new Vector3(-1604.552f, -1048.718f, 17.027f));
            Cam5.InterpTo(Cam6, 7000, 0, 1);
            await BaseScript.Delay(2000);
            sub_12019("senprog", 30.0f, 50.0f, "right", 0.8f, 0.8f);
            sub_11fbe("senprog", "Ideato Scritto e Realizzato da", 0.0f, "HUD_COLOUR_NET_PLAYER2", true);
            sub_11f63("senprog", "MANUPS4E|", 145.0f, "|", true);
            sub_11f32("senprog", 0.16f);
            await BaseScript.Delay(5000);
            sub_11f01("senprog", 0.16f);
            await BaseScript.Delay(1000);
            Screen.Fading.FadeOut(800);
            await BaseScript.Delay(1000);
            playerPed.Position = new Vector3(-552.468f, -513.632f, 30.427f);
            while (!HasCollisionLoadedAroundEntity(playerPed.Handle)) await BaseScript.Delay(1000);
            Cam5.Delete();
            Cam6.Delete();
            Camera Cam7 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f, Position = new Vector3(-552.468f, -513.632f, 30.427f) };
            Cam7.PointAt(new Vector3(-133.448f, -512.632f, 30.427f));
            Cam7.IsActive = true;
            Screen.Fading.FadeIn(800);
            Camera Cam8 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f, Position = new Vector3(-552.468f, -513.632f, 150.427f) };
            Cam8.PointAt(new Vector3(-133.448f, -512.632f, 30.427f));
            Cam7.InterpTo(Cam8, 10000, 0, 1);
            await BaseScript.Delay(3000);
            ScaleformUI.ScaleformUI.BigMessageInstance.ShowMpWastedMessage("The Last Galaxy!", "Benvenuto, nel Pianeta del Free-Roam!");
            await BaseScript.Delay(4000);
            await BaseScript.Delay(1000);
            Cam7.Delete();
            Client.Instance.RemoveTick(Controllo);
            Client.Instance.RemoveTick(Crediti);
            SiContinua(Cam8);
        }

        public static async void SiContinua(Camera Cam)
        {
            Ped playerPed = PlayerCache.MyPlayer.Ped;
            Client.Instance.AddTick(Controllo);
            Client.Instance.AddTick(Crediti);
            Camera Cam9 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f };
            Model tassista = new(PedHash.Stlat01AMY);
            tassista.Request();
            while (!tassista.IsLoaded) await tassista.Request(1);

            if (!new Vector3(-640.411f, -525.006f, 25.331f).IsSpawnPointClear(2f))
            {
                Vehicle[] vehs = new Vector3(-640.411f, -525.006f, 25.331f).GetVehiclesInArea(2f);

                foreach (Vehicle v in vehs)
                {
                    foreach (Player p in Client.Instance.GetPlayers.ToList())
                        if (v.Driver != new Ped(GetPlayerPed(p.Handle)))
                            v.Delete();
                }
            }

            Vehicle taxi = await Funzioni.SpawnLocalVehicle("taxi", new Vector3(-640.411f, -525.006f, 25.331f), 268.927f);
            while (!taxi.Exists()) await BaseScript.Delay(1);
            Ped Tax = await Funzioni.CreatePedLocally(tassista.Hash, Vector3.Zero, 0, PedTypes.CivMale);
            Tax.SetIntoVehicle(taxi, VehicleSeat.Driver);
            Tax.BlockPermanentEvents = true;
            Tax.VehicleDrivingFlags = VehicleDrivingFlags.FollowTraffic;
            if (!Tax.IsInVehicle(taxi)) Tax.Task.WarpIntoVehicle(taxi, VehicleSeat.Driver);
            playerPed.SetIntoVehicle(taxi, VehicleSeat.RightRear);
            playerPed.IsVisible = true;
            //Tassista.Task.WanderAround();
            taxi.IsEngineRunning = true;
            taxi.CanEngineDegrade = false;
            taxi.IsDriveable = true;
            taxi.IsRadioEnabled = false;
            taxi.RadioStation = RadioStation.RadioOff;
            Tax.Task.DriveTo(taxi, new Vector3(829.409f, -2608.958f, 52.407f), 3.0f, 20f, 786603);
            await BaseScript.Delay(6000);
            Cam9.AttachTo(taxi, new Vector3(0, -6f, 3f));
            Cam9.Rotation = taxi.Rotation;
            Cam.InterpTo(Cam9, 2000, 1, 1);
            await BaseScript.Delay(2000);
            int i = 800;
            sub_12019("prog", 0.0f, 200.0f, "left", 0.8f, 0.8f);
            sub_11fbe("prog", "Con l'aiuto e il supporto di", 35.0f, "HUD_COLOUR_NET_PLAYER2", true);
            sub_11f63("prog", "THESTRONGHT|VITTORIO|", 100.0f, "|", true);
            sub_11f32("prog", 0.16f);

            while (i > 0)
            {
                await BaseScript.Delay(0);
                Cam9.Rotation = taxi.Rotation;
                --i;
            }

            sub_11f01("prog", 0.16f);
            Cam9.Detach();
            Cam9.AttachTo(playerPed.Bones[Bone.SKEL_Head], new Vector3(1f, 1f, 1f));
            Cam9.PointAt(playerPed.Bones[Bone.SKEL_Head]);
            await BaseScript.Delay(1000);
            sub_12019("scrlead", 50.0f, 185.0f, "right", 0.8f, 0.8f);
            sub_11fbe("scrlead", "Grazie di averci scelto!", 0.0f, "HUD_COLOUR_NET_PLAYER1", true);
            sub_11f63("scrlead", "PREPARATI...|", 60.0f, "|", true);
            sub_11f32("scrlead", 0.16f);
            await BaseScript.Delay(4000);
            sub_11f01("scrlead", 0.16f);
            Client.Instance.RemoveTick(Crediti);
            Camera cam10 = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { FieldOfView = 60f, Position = new Vector3(Cam9.Position.X, Cam9.Position.Y, Cam9.Position.Z + 50) };
            cam10.PointAt(taxi);
            Cam9.InterpTo(cam10, 5000, 1, 1);
            await BaseScript.Delay(7000);
            Screen.Fading.FadeOut(800);
            await BaseScript.Delay(2000);
            Tax.Task.ClearAllImmediately();
            Tax.Delete();
            taxi.Delete();
            playerPed.Position = new Vector3(0, 0, 0);
            Cam9.Delete();
            cam10.Delete();
            DestroyAllCams(false);
            RenderScriptCams(false, false, 0, false, false);
            SiFinisce();
        }

        private static async void SiFinisce()
        {
            Ped playerPed = PlayerCache.MyPlayer.Ped;
            TriggerMusicEvent("GLOBAL_KILL_MUSIC");
            playerPed.Position = new Vector3(262.687f, -875.486f, 29.153f);
            RenderScriptCams(false, false, 0, false, false);
            playerPed.IsVisible = true;
            PlayerCache.MyPlayer.Status.Istanza.RimuoviIstanza();
            playerPed.IsPositionFrozen = false;
            NetworkClearClockTimeOverride();
            await BaseScript.Delay(1000);

            EventDispatcher.Send("SyncWeatherForMe", true);
            Screen.Fading.FadeIn(800);
            Client.Instance.RemoveTick(Controllo);
            EventDispatcher.Send("worldEventsManage.Server:AddParticipant");
            AccessingEvents.FreeRoamSpawn(PlayerCache.MyPlayer);
            PlayerCache.MyPlayer.Status.PlayerStates.Spawned = true;
            PlayerCache.MyPlayer.Player.CanControlCharacter = true;
        }

        private static async Task Controllo()
        {
            Game.DisableAllControlsThisFrame(0);
            Game.DisableAllControlsThisFrame(1);
            Game.DisableAllControlsThisFrame(2);
            await Task.FromResult(0);
        }

        private static async Task Crediti()
        {
            Credits.Render2D();
            await Task.FromResult(0);
        }

        private static void func_381()
        {
            if (Credits.IsLoaded) Credits.CallFunction("REMOVE_ALL");
        }

        private static void func_382(string sParam0, float fParam1)
        {
            PushScaleformMovieFunction(Credits.Handle, "HIDE");
            BeginTextComponent("STRING");
            AddTextComponentSubstringPlayerName(sParam0);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(fParam1);
            PopScaleformMovieFunction();
        }

        private static async void func_383(string sParam0, float fParam1)
        {
            PushScaleformMovieFunction(Credits.Handle, "SHOW_SINGLE_LINE");
            BeginTextComponent("STRING");
            AddTextComponentSubstringPlayerName(sParam0);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(fParam1);
            PopScaleformMovieFunction();
            await Task.FromResult(0);
        }

        private static async void func_384(string sParam0, string sParam1, string sParam2, string sParam3, bool iParam4)
        {
            PushScaleformMovieFunction(Credits.Handle, "ADD_TEXT_TO_SINGLE_LINE");
            BeginTextComponent("STRING");
            AddTextComponentSubstringPlayerName(sParam0);
            EndTextComponent();
            BeginTextComponent("STRING");
            AddTextComponentSubstringPlayerName(sParam1);
            EndTextComponent();
            BeginTextComponent("STRING");
            AddTextComponentSubstringPlayerName(sParam2);
            EndTextComponent();
            BeginTextComponent("STRING");
            AddTextComponentSubstringPlayerName(sParam3);
            EndTextComponent();
            PushScaleformMovieFunctionParameterBool(iParam4);
            PopScaleformMovieFunction();
            await Task.FromResult(0);
        }

        private static async void func_385(string sParam0, float fParam1, float fParam2, float fParam3, float fParam4, string sParam5)
        {
            PushScaleformMovieFunction(Credits.Handle, "SETUP_SINGLE_LINE");
            BeginTextComponent("STRING");
            AddTextComponentSubstringPlayerName(sParam0);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(fParam1);
            PushScaleformMovieFunctionParameterFloat(fParam2);
            PushScaleformMovieFunctionParameterFloat(fParam3);
            PushScaleformMovieFunctionParameterFloat(fParam4);
            BeginTextComponent("STRING");
            AddTextComponentSubstringPlayerName(sParam5);
            EndTextComponent();
            PopScaleformMovieFunction();
            await Task.FromResult(0);
        }

        private static async void sub_200(string a_0, float a_1, float a_2, float a_3, float a_4, float a_5, float a_6, float a_7)
        {
            PushScaleformMovieFunction(Credits.Handle, "SHOW_LOGO");
            BeginTextComponent("STRING");
            AddTextComponentString(a_0);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(a_1);
            PushScaleformMovieFunctionParameterFloat(a_2);
            PushScaleformMovieFunctionParameterFloat(a_3);
            PushScaleformMovieFunctionParameterFloat(a_4);
            PushScaleformMovieFunctionParameterFloat(a_5);
            PushScaleformMovieFunctionParameterFloat(a_6);
            PushScaleformMovieFunctionParameterFloat(a_7);
            PopScaleformMovieFunction();
            await Task.FromResult(0);
        }

        private static void sub_11f01(string a_0, float a_1)
        {
            PushScaleformMovieFunction(Credits.Handle, "HIDE");
            BeginTextComponent("STRING");
            AddTextComponentSubstringPlayerName(a_0);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(a_1);
            PopScaleformMovieFunction();
        }

        private static void sub_11f32(string a_0, float a_1)
        {
            PushScaleformMovieFunction(Credits.Handle, "SHOW_CREDIT_BLOCK");
            BeginTextComponent("STRING");
            AddTextComponentString(a_0);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(a_1);
            PopScaleformMovieFunction();
        }

        private static void sub_11f63(string a_0, string a_1, float a_2, string a_3, bool a_4)
        {
            PushScaleformMovieFunction(Credits.Handle, "ADD_NAMES_TO_CREDIT_BLOCK");
            BeginTextComponent("STRING");
            AddTextComponentString(a_0);
            EndTextComponent();
            BeginTextComponent("STRING");
            AddTextComponentString(a_1);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(a_2);
            BeginTextComponent("STRING");
            AddTextComponentString(a_3);
            EndTextComponent();
            PushScaleformMovieFunctionParameterBool(a_4);
            PopScaleformMovieFunction();
        }

        private static void sub_11fbe(string a_0, string a_1, float a_2, string a_3, bool a_4)
        {
            PushScaleformMovieFunction(Credits.Handle, "ADD_ROLE_TO_CREDIT_BLOCK");
            BeginTextComponent("STRING");
            AddTextComponentString(a_0);
            EndTextComponent();
            BeginTextComponent("STRING");
            AddTextComponentString(a_1);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(a_2);
            BeginTextComponent("STRING");
            AddTextComponentString(a_3);
            EndTextComponent();
            PushScaleformMovieFunctionParameterBool(a_4);
            PopScaleformMovieFunction();
        }

        private static void sub_12019(string a_0, float a_1, float a_2, string a_3, float a_4, float a_5)
        {
            PushScaleformMovieFunction(Credits.Handle, "SETUP_CREDIT_BLOCK");
            BeginTextComponent("STRING");
            AddTextComponentString(a_0);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(a_1);
            PushScaleformMovieFunctionParameterFloat(a_2);
            BeginTextComponent("STRING");
            AddTextComponentString(a_3);
            EndTextComponent();
            PushScaleformMovieFunctionParameterFloat(a_4);
            PushScaleformMovieFunctionParameterFloat(a_5);
            PopScaleformMovieFunction();
        }
    }
}