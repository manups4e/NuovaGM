using System;
using System.Drawing;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Veicoli
{
    internal static class VehHud
    {
        private static bool overwriteAlpha;
        private static VHudSkin cst = new VHudSkin();
        private static bool lightson;
        private static bool highbeams;
        private static int blinkerstate;
        private static bool blinkerleft;
        private static bool blinkerright;
        private static bool showDamageYellow;
        private static bool showDamageRed;
        private static bool showLowFuelYellow;
        private static bool showLowFuelRed;
        private static bool showHighBeams;
        private static bool showLowBeams;
        private static string curNeedle;
        private static string curTachometer;
        private static string curSpeedometer;
        private static string curFuelGauge;
        public static int curAlpha;
        private static float RPM;
        private static float degree;
        private static bool showBlinker;
        private static float engineHealth;
        private static float OilLevel;
        private static float FuelLevel;
        private static float MaxFuelLevel;
        private static bool IsEngineOn;
        private static bool showBlinkerBelt;
        private static bool UIOpen;
        private static bool beltOn;
        private static float[] speedBuffer = new float[2];
        private static Vector3[] velBuffer = new Vector3[2];
        private static Notifica a;
        private static int _timer1;
        private static int _timer2;

        public static async void Init() { TickController.TickVeicolo.Add(OnTickSpeedo3); }

        public static async void Stop()
        {
            TickController.TickVeicolo.Remove(OnTickSpeedo3);
        }

        private static async Task OnTickSpeedo3()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;
            Vehicle veh = playerPed.CurrentVehicle;

            if (Game.GameTime - _timer1 > 100)
            {
                if (veh != null && veh.Exists() && !veh.IsDead && !playerPed.IsDead)
                {
                    IsEngineOn = veh.IsEngineRunning;
                    engineHealth = veh.EngineHealth;
                    OilLevel = veh.OilLevel;
                    FuelLevel = veh.vehicleFuelLevel();
                    lightson = veh.AreLightsOn;
                    highbeams = veh.AreHighBeamsOn;
                    MaxFuelLevel = GetVehicleHandlingFloat(veh.Handle, "CHandlingData", "fPetrolTankVolume");
                }

                _timer1 = Game.GameTime;
            }

            if (blinkerleft || blinkerright)
                if (Game.GameTime - _timer1 > 500)
                {
                    _timer1 = Game.GameTime;
                    showBlinker = !showBlinker;
                }

            if (!beltOn)
                if (Game.GameTime - _timer2 > 500)
                {
                    _timer2 = Game.GameTime;
                    showBlinkerBelt = !showBlinkerBelt;
                }

            if (overwriteAlpha) curAlpha = 0;

            if (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo && playerPed.SeatIndex == VehicleSeat.Driver)
            {
                if (curAlpha >= 255)
                    curAlpha = 255;
                else
                    curAlpha += 5;
            }
            else if (!Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo)
            {
                if (curAlpha <= 0)
                    curAlpha = 0;
                else
                    curAlpha -= 5;
            }

            if (!HasStreamedTextureDictLoaded(cst.ytdName))
            {
                RequestStreamedTextureDict(cst.ytdName, true);
                while (!HasStreamedTextureDictLoaded(cst.ytdName)) await BaseScript.Delay(0);
            }
            else
            {
                if (playerPed.CurrentVehicle != null && playerPed.CurrentVehicle.Exists() && !playerPed.CurrentVehicle.IsDead && !playerPed.IsDead)
                {
                    RPM = playerPed.CurrentVehicle.CurrentRPM;
                    if (!IsEngineOn) RPM = 0;

                    if (RPM > 0.999f)
                    {
                        RPM *= 100f;
                        RPM += Funzioni.GetRandomFloat(-2f, 2f);
                        RPM /= 100f;
                    }

                    if (playerPed.CurrentVehicle.Speed > 0) degree = playerPed.CurrentVehicle.Speed * 2.036936f * cst.RotStep;
                    if (degree > 290) degree = 290f;
                    blinkerstate = GetVehicleIndicatorLights(playerPed.CurrentVehicle.Handle);

                    switch (blinkerstate)
                    {
                        case 0:
                            blinkerleft = false;
                            blinkerright = false;

                            break;
                        case 1:
                            blinkerleft = true;
                            blinkerright = false;

                            break;
                        case 2:
                            blinkerleft = false;
                            blinkerright = true;

                            break;
                        case 3:
                            blinkerleft = true;
                            blinkerright = true;

                            break;
                    }

                    if (engineHealth <= 550 && engineHealth > 100)
                    {
                        showDamageYellow = true;
                        showDamageRed = false;
                    }
                    else if (engineHealth <= 100)
                    {
                        showDamageYellow = false;
                        showDamageRed = true;
                    }
                    else
                    {
                        showDamageYellow = false;
                        showDamageRed = false;
                    }

                    if (FuelLevel <= MaxFuelLevel * 0.25f && FuelLevel > MaxFuelLevel * 0.13f)
                    {
                        showLowFuelYellow = true;
                        showLowFuelRed = false;
                    }
                    else if (FuelLevel <= MaxFuelLevel * 0.2f)
                    {
                        showLowFuelYellow = false;
                        showLowFuelRed = true;
                    }
                    else
                    {
                        showLowFuelYellow = false;
                        showLowFuelRed = false;
                    }

                    if (lightson || highbeams)
                    {
                        curNeedle = "needle";
                        curTachometer = "tachometer";
                        curSpeedometer = "speedometer";
                        curFuelGauge = "fuelgauge";

                        if (highbeams)
                        {
                            showHighBeams = true;
                            showLowBeams = false;
                        }
                        else if (lightson && !highbeams)
                        {
                            showHighBeams = false;
                            showLowBeams = true;
                        }
                    }
                    else
                    {
                        curNeedle = "needle_day";
                        curTachometer = "tachometer_day";
                        curSpeedometer = "speedometer_day";
                        curFuelGauge = "fuelgauge_day";
                        showHighBeams = false;
                        showLowBeams = false;
                    }

                    if (((int)playerPed.CurrentVehicle.ClassType < 0 || (int)playerPed.CurrentVehicle.ClassType >= 13) && (int)playerPed.CurrentVehicle.ClassType < 17) curAlpha = 0;
                    if (RPM < 0.119999997317791f || RPM == 0) RPM = 0.12f;
                    if (showHighBeams)
                        DrawSprite(cst.ytdName, "lights", cst.centerCoords.X + cst.lightsLoc.X, cst.centerCoords.Y + cst.lightsLoc.Y, cst.lightsLoc.Z, cst.lightsLoc.W, 0.0f, 0, 50, 240, curAlpha);
                    else if (showLowBeams) DrawSprite(cst.ytdName, "lights", cst.centerCoords.X + cst.lightsLoc.X, cst.centerCoords.Y + cst.lightsLoc.Y, cst.lightsLoc.Z, cst.lightsLoc.W, 0.0f, 0, 255, 0, curAlpha);
                    if (blinkerleft && showBlinker) DrawSprite(cst.ytdName, "blinker", cst.centerCoords.X + cst.blinkerLoc.X, cst.centerCoords.Y + cst.blinkerLoc.Y, cst.blinkerLoc.Z, cst.blinkerLoc.W, 180f, 124, 252, 0, curAlpha);
                    if (blinkerright && showBlinker) DrawSprite(cst.ytdName, "blinker", cst.centerCoords.X + cst.blinkerLoc.X + 0.0299999993294477f, cst.centerCoords.Y + cst.blinkerLoc.Y - 1.0f / 1000.0f, cst.blinkerLoc.Z, cst.blinkerLoc.W, 0.0f, 124, 252, 0, curAlpha);

                    if (MaxFuelLevel != 0)
                    {
                        if (showLowFuelYellow)
                            DrawSprite(cst.ytdName, "fuel", cst.centerCoords.X + cst.fuelLoc.X, cst.centerCoords.Y + cst.fuelLoc.Y, cst.fuelLoc.Z, cst.fuelLoc.W, 0.0f, 255, 191, 0, curAlpha);
                        else if (showLowFuelRed) DrawSprite(cst.ytdName, "fuel", cst.centerCoords.X + cst.fuelLoc.X, cst.centerCoords.Y + cst.fuelLoc.Y, cst.fuelLoc.Z, cst.fuelLoc.W, 0.0f, 255, 0, 0, curAlpha);
                        if (OilLevel <= 0.5f) DrawSprite(cst.ytdName, "oil", cst.centerCoords.X + cst.oilLoc.X, cst.centerCoords.Y + cst.oilLoc.Y, cst.oilLoc.Z, cst.oilLoc.W, 0.0f, 255, 0, 0, curAlpha);
                        DrawSprite(cst.ytdName, curTachometer, cst.centerCoords.X + cst.TachoBGloc.X, cst.centerCoords.Y + cst.TachoBGloc.Y, cst.TachoBGloc.Z, cst.TachoBGloc.W, 0.0f, 255, 255, 255, curAlpha);
                        DrawSprite(cst.ytdName, curNeedle, cst.centerCoords.X + cst.TachoNeedleLoc.X, cst.centerCoords.Y + cst.TachoNeedleLoc.Y, cst.TachoNeedleLoc.Z, cst.TachoNeedleLoc.W, RPM * cst.rpmScale - cst.rpmScaleDecrease, 255, 255, 255, curAlpha);
                    }

                    if (showDamageYellow)
                        DrawSprite(cst.ytdName, "engine", cst.centerCoords.X + cst.engineLoc.X, cst.centerCoords.Y + cst.engineLoc.Y, cst.engineLoc.Z, cst.engineLoc.W, 0.0f, 255, 191, 0, curAlpha);
                    else if (showDamageRed) DrawSprite(cst.ytdName, "engine", cst.centerCoords.X + cst.engineLoc.X, cst.centerCoords.Y + cst.engineLoc.Y, cst.engineLoc.Z, cst.engineLoc.W, 0.0f, 255, 0, 0, curAlpha);
                    DrawSprite(cst.ytdName, curSpeedometer, cst.centerCoords.X + cst.SpeedoBGLoc.X, cst.centerCoords.Y + cst.SpeedoBGLoc.Y, cst.SpeedoBGLoc.Z, cst.SpeedoBGLoc.W, 0.0f, 255, 255, 255, curAlpha);
                    DrawSprite(cst.ytdName, curNeedle, cst.centerCoords.X + cst.SpeedoNeedleLoc.X, cst.centerCoords.Y + cst.SpeedoNeedleLoc.Y, cst.SpeedoNeedleLoc.Z, cst.SpeedoNeedleLoc.W, degree - 5.00001f, 255, 255, 255, curAlpha);

                    if (FuelLevel > -1.0f && MaxFuelLevel != 0)
                    {
                        DrawSprite(cst.ytdName, curFuelGauge, cst.centerCoords.X + cst.FuelBGLoc.X, cst.centerCoords.Y + cst.FuelBGLoc.Y, cst.FuelBGLoc.Z, cst.FuelBGLoc.W, 0.0f, 255, 255, 255, curAlpha);
                        DrawSprite(cst.ytdName, curNeedle, cst.centerCoords.X + cst.FuelGaugeLoc.X, cst.centerCoords.Y + cst.FuelGaugeLoc.Y, cst.FuelGaugeLoc.Z, cst.FuelGaugeLoc.W, 80.0f + playerPed.CurrentVehicle.FuelLevel / GetVehicleHandlingFloat(playerPed.CurrentVehicle.Handle, "CHandlingData", "fPetrolTankVolume") * 110.0f, 255, 255, 255, curAlpha);
                    }

                    if (!beltOn && showBlinkerBelt) DrawSprite(cst.ytdName, "seatbelt", cst.centerCoords.X + cst.seatbeltLoc.X, cst.centerCoords.Y + cst.seatbeltLoc.Y, cst.seatbeltLoc.Z, cst.seatbeltLoc.W, 0.0f, 255, 0, 0, curAlpha);

                    if (IsCar(playerPed.CurrentVehicle.Handle))
                    {
                        if (!UIOpen)
                        {
                            UIOpen = true;
                            NUIBuckled(beltOn);
                        }

                        if (beltOn)
                        {
                            Game.DisableControlThisFrame(0, Control.VehicleExit);
                            if (Input.IsDisabledControlJustPressed(Control.VehicleExit)) HUD.ShowNotification("Hai la cintura allacciata!!", NotificationColor.Red, true);
                        }

                        speedBuffer[1] = speedBuffer[0];
                        speedBuffer[0] = playerPed.CurrentVehicle.Speed;

                        if (speedBuffer[1] > 0 && !beltOn && GetEntitySpeedVector(playerPed.CurrentVehicle.Handle, true).Y > 1 && speedBuffer[0] > 15 && speedBuffer[1] - speedBuffer[0] > speedBuffer[0] * 0.254999995231628f)
                        {
                            Vector3 coords = playerPed.Position;
                            float[] fw = ForwardVelocity(PlayerPedId());
                            playerPed.Position = new Vector3(coords.X + fw[0], coords.Y + fw[1], coords.Z - 0.469999998807907f);
                            playerPed.Velocity = new Vector3(velBuffer[1].X, velBuffer[1].Y, velBuffer[1].Z);
                            await BaseScript.Delay(1);
                            playerPed.Ragdoll(3000, RagdollType.Normal);
                        }

                        velBuffer[1] = velBuffer[0];
                        velBuffer[0] = playerPed.CurrentVehicle.Velocity;

                        if (Input.IsControlJustPressed(Control.ReplayTimelinePickupClip, PadCheck.Keyboard) || Input.IsControlPressed(Control.FrontendLb, PadCheck.Controller) && Input.IsControlJustPressed(Control.FrontendX, PadCheck.Controller))
                        {
                            if (a != null) a.Hide();
                            beltOn = !beltOn;
                            a = HUD.ShowNotification(beltOn ? "Cintura di sicurezza ~y~allacciata~w~." : "Cintura di sicurezza ~y~slacciata~w~.");
                            NUIBuckled(beltOn);
                        }
                    }
                }
                else
                {
                    beltOn = false;
                    speedBuffer[0] = speedBuffer[1] = 0.0f;
                    if (UIOpen) UIOpen = false;
                    NUIBuckled(beltOn);
                }
            }
        }

        private static bool IsCar(int vehicle)
        {
            int vehicleClass = GetVehicleClass(vehicle);

            return vehicleClass >= 0 && vehicleClass <= 7 || vehicleClass >= 9 && vehicleClass <= 12 || vehicleClass >= 17 && vehicleClass <= 20;
        }

        public static void NUIBuckled(bool value)
        {
            Client.Instance.NuiManager.SendMessage("buckle:seatbelts", new { transactionValue = value, inCar = Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo });
        }

        private static float[] ForwardVelocity(int ent)
        {
            float entityHeading = GetEntityHeading(ent);
            if (entityHeading < 0) entityHeading += 360f;
            float num = entityHeading * 0.0174533f;

            return new float[2] { (float)Math.Cos(num) * 2f, (float)Math.Sin(num) * 2f };
        }
    }

    internal class VHudSkin
    {
        public string skinName = "default";
        public string ytdName = "default";
        public Vector4 lightsLoc = new Vector4(0.01f, 0.092f, 0.018f, 0.02f);
        public Vector4 blinkerLoc = new Vector4(0.105f, 0.034f, 0.022f, 0.03f);
        public Vector4 fuelLoc = new Vector4(0.105f, 0.09f, 0.012f, 0.025f);
        public Vector4 oilLoc = new Vector4(0.1f, 0.062f, 0.02f, 0.025f);
        public Vector4 engineLoc = new Vector4(0.13f, 0.092f, 0.02f, 0.025f);
        public Vector4 seatbeltLoc = new Vector4(0.14f, 0.062f, 0.025f, 0.03f);
        public Vector4 SpeedoBGLoc = new Vector4(0.0f, 0.06f, 0.12f, 0.185f);
        public Vector4 SpeedoNeedleLoc = new Vector4(0.0f, 0.062f, 0.076f, 0.15f);
        public Vector4 TachoBGloc = new Vector4(0.12f, 0.06f, 0.12f, 0.185f);
        public Vector4 TachoNeedleLoc = new Vector4(0.12f, 0.062f, 0.076f, 0.15f);
        public Vector4 FuelBGLoc = new Vector4(0.06f, -0.02f, 0.04f, 0.04f);
        public Vector4 FuelGaugeLoc = new Vector4(0.06f, 0.0f, 0.04f, 0.08f);
        public float RotMult = 2.036936f;
        public float RotStep = 2.32833f;
        public PointF centerCoords = new PointF(0.8f, 0.8f);
        public float rpmScale = 270f;
        public int rpmScaleDecrease = 30;
    }
}