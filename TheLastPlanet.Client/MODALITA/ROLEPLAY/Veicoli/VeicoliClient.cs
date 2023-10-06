using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Veicoli
{
    internal static class VeicoliClient
    {
        public static Camera garageCam = null;
        public static Vehicle previewedVehicle = new Vehicle(0);
        public static Vehicle veicoloinaffitto = new Vehicle(0);
        public static List<List<Vector4>> carGarageSpawnAlt = new List<List<Vector4>>()
        {
            new List<Vector4>() { new Vector4(-339.909f, -778.990f, 33.746f, 160.660f), new Vector4(-329.797f, -771.134f, 33.744f, 107.957f), new Vector4(-342.096f, -767.308f, 33.749f, 271.592f), new Vector4(-342.646f, -764.257f, 33.749f, 271.592f) },
            new List<Vector4>() { new Vector4(293.001f, 74.975f, 94.354f, 333.886f), new Vector4(296.538f, 73.689f, 94.352f, 333.886f) },
            new List<Vector4>() { new Vector4(-73.149f, 6498.163f, 31.490f, 173.009f), new Vector4(-81.932f, 6483.518f, 31.4901f, 234.343f) },
            new List<Vector4>() { new Vector4(-797.935f, 319.881f, 85.685f, 172.755f) },
            new List<Vector4>() { new Vector4(356.994f, -1678.429f, 32.537f, 47.017f), new Vector4(359.275f, -1675.791f, 32.537f, 43.293f), new Vector4(361.651f, -1673.057f, 32.537f, 42.563f) },
            new List<Vector4>() { new Vector4(1218.22f, 2716.87f, 37.985f, 179.33f), new Vector4(1234.61f, 2716.66f, 37.985f, 182.76f) }
        };

        public static List<Vector3> carGarageSpots = new List<Vector3>()
        {
            new Vector3(-331.978f, -781.474f, 33.964f),
            new Vector3(286.633f, 78.898f, 94.362f),
            new Vector3(-82.643f, 6496.481f, 31.490f),
            new Vector3(346.343f, -1686.543f, 32.531f),
            new Vector3(1224.62f, 2727.73f, 38.005f),
            new Vector3(-796.141f, 335.102f, 85.701f)
        };

        public static List<Vector4> carGarageSpawn = new List<Vector4>()
        {
            new Vector4(-336.039f, -774.722f, 33.967f, 126.251f),
            new Vector4(285.069f, 74.425f, 94.361f, 66.031f),
            new Vector4(-79.148f, 6492.567f, 31.4901f, 216.051f),
            new Vector4(354.263f, -1681.598f, 32.129f, 44.309f),
            new Vector4(1226.28f, 2716.52f, 37.983f, 169.42f),
            new Vector4(-794.146f, 320.657f, 85.691f, 173.723f)
        };

        public static List<Vector4> carGaragePrev = new List<Vector4>()
        {
            new Vector4(-334.427f, -753.621f, 53.246f, 59.301f),
            new Vector4(282.460f, 68.339f, 99.231f, 70.391f),
            new Vector4(-62.595f, 6499.386f, 30.873f, 156.203f),
            new Vector4(378.928f, -1647.322f, 48.302f, 228.355f),
            new Vector4(1246.63f, 2716.68f, 38.142f, 354.568f),
            new Vector4(-800.336f, 332.529f, 85.701f, 174.162f)
        };

        public static List<Vector3> cargaragecamcoords = new List<Vector3>()
        {
            new Vector3(-334.532f, -747.769f, 53.246f),
            new Vector3(281.521f, 75.166f, 99.898f),
            new Vector3(-68.830f, 6499.263f, 31.491f),
            new Vector3(375.62f, -1652.83f, 48.302f),
            new Vector3(1241.36f, 2716.36f, 37.968f),
            new Vector3(-795.681f, 331.222f, 85.701f)
        };

        public static List<Vector3> carWashSpots = new List<Vector3>() { new Vector3(22.836f, -1392.1002f, 29.331f), new Vector3(-699.804f, -931.542f, 19.013f), new Vector3(2005.487f, 3798.109f, 32.181f), new Vector3(-100.058f, 6399.524f, 31.448f) };

        public static List<int> state = new List<int>();

        public static Vector3 insuranceSpots = new Vector3(-32.293f, -1111.828f, 26.422f);

        private static bool acceso = false;
        public static int ind_state_o = 0;
        public static int ind_state_l = 1;
        public static int ind_state_r = 2;
        public static int ind_state_h = 3;
        public static int count_ind_timer = 0;
        public static bool actv_ind_timer = false;
        public static bool SirenToggle = false;
        public static int count_bcast_timer = 0;
        public static int delay_bcast_timer = 200;
        public static int delay_ind_timer = 180;
        public static Dictionary<Vehicle, int> state_indic = new Dictionary<Vehicle, int>();

        private static List<InputController> inputs = new();
        private static List<Blip> blips = new();
        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            Client.Instance.AddEventHandler("lprp:lvc_TogIndicState_c", new Action<string, int>(lvc_TogIndicState_c));
            Client.Instance.AddEventHandler("lprp:updateSirens", new Action<string, bool>(updateSirens));
            for (int i = 0; i < carGarageSpots.Count; i++)
            {
                inputs.Add(new InputController(Control.Context, carGarageSpots[i].ToPosition(), "Premi ~INPUT_CONTEXT~ per affittare un veicolo", new((MarkerType)(-1), carGarageSpots[i].ToPosition(), SColor.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>((playerPed, a) =>
                {
                    MenuAffittoVeicoli.MenuAffitto((int)a[0]);
                }), i));
            }
            InputHandler.AddInputList(inputs);
        }

        public static void Spawnato(PlayerClient client)
        {
            foreach (Blip b in carGarageSpots.Select(v => new Blip(AddBlipForCoord(v.X, v.Y, v.Z))))
            {
                b.Sprite = BlipSprite.Garage2;
                SetBlipDisplay(b.Handle, 4);
                b.Scale = 0.9f;
                b.IsShortRange = true;
                b.Name = "Affitto / Restituzione Veicoli";
                blips.Add(b);
            }
        }

        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("lprp:lvc_TogIndicState_c", new Action<string, int>(lvc_TogIndicState_c));
            Client.Instance.RemoveEventHandler("lprp:updateSirens", new Action<string, bool>(updateSirens));
            InputHandler.RemoveInputList(inputs);
            inputs.Clear();
            blips.ForEach(x => x.Delete());
            blips.Clear();
        }

        public static async Task Lux()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && Main.spawned)
            {
                Vehicle veh = playerPed.CurrentVehicle;

                if (playerPed.SeatIndex == VehicleSeat.Driver)
                {
                    Game.DisableControlThisFrame(0, Control.VehicleSelectNextWeapon);
                    Game.DisableControlThisFrame(0, Control.VehicleSelectPrevWeapon);
                    Game.DisableControlThisFrame(0, Control.SelectWeapon);
                    if (!state_indic.ContainsKey(veh)) state_indic.Add(veh, ind_state_o);
                    if (state_indic[veh] != ind_state_o && state_indic[veh] != ind_state_l && state_indic[veh] != ind_state_r && state_indic[veh] != ind_state_h) state_indic[veh] = ind_state_o;

                    if (actv_ind_timer)
                        if (state_indic[veh] == ind_state_l || state_indic[veh] == ind_state_r)
                        {
                            if (veh.Speed < 6f)
                            {
                                count_ind_timer = 0;
                            }
                            else
                            {
                                if (count_ind_timer > delay_ind_timer)
                                {
                                    count_ind_timer = 0;
                                    actv_ind_timer = false;
                                    state_indic[veh] = ind_state_o;
                                    Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                    TogIndicStateForVeh(veh, state_indic[veh]);
                                    count_bcast_timer = delay_bcast_timer;
                                }
                                else
                                {
                                    count_ind_timer = count_ind_timer + 1;
                                }
                            }
                        }

                    if (Input.IsControlJustPressed(Control.DropAmmo, PadCheck.Keyboard))
                    {
                        if (veh.Speed < 10f && !veh.Model.IsBicycle)
                            engine();
                        else
                            HUD.ShowNotification(veh.Model.IsBicycle ? "Le Biciclette non hanno motore!" : "Non puoi spegnere il motore a questa velocità!");
                    }

                    if (Input.IsControlPressed(Control.FrontendLb, PadCheck.Controller))
                    {
                        Game.DisableControlThisFrame(0, Control.VehicleDuck);

                        if (Input.IsDisabledControlJustPressed(Control.VehicleDuck))
                        {
                            if (playerPed.CurrentVehicle.Speed < 10f && !veh.Model.IsBicycle)
                                engine();
                            else
                                HUD.ShowNotification(veh.Model.IsBicycle ? "Le Biciclette non hanno motore!" : "Non puoi spegnere il motore a questa velocità!");
                        }
                    }

                    if (veh.ClassType != VehicleClass.Boats && veh.ClassType != VehicleClass.Helicopters && veh.ClassType != VehicleClass.Planes && veh.ClassType != VehicleClass.Trains)
                    {
                        if (!Game.IsPaused)
                        {
                            if (Input.IsControlJustPressed(Control.ReplayAdvance, PadCheck.Keyboard))
                            {
                                int cstate = state_indic[veh];

                                if (cstate == ind_state_l)
                                {
                                    state_indic[veh] = ind_state_o;
                                    actv_ind_timer = false;
                                    Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                }
                                else
                                {
                                    state_indic[veh] = ind_state_l;
                                    actv_ind_timer = true;
                                    Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                }

                                TogIndicStateForVeh(veh, state_indic[veh]);
                                count_ind_timer = 0;
                                count_bcast_timer = delay_bcast_timer;
                            }
                            else if (Input.IsControlJustPressed(Control.ReplayBack, PadCheck.Keyboard))
                            {
                                int cstate = state_indic[veh];

                                if (cstate == ind_state_r)
                                {
                                    state_indic[veh] = ind_state_o;
                                    actv_ind_timer = false;
                                    Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                }
                                else
                                {
                                    state_indic[veh] = ind_state_r;
                                    actv_ind_timer = true;
                                    Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                }

                                TogIndicStateForVeh(veh, state_indic[veh]);
                                count_ind_timer = 0;
                                count_bcast_timer = delay_bcast_timer;
                            }
                            else if (Input.IsControlJustPressed(Control.ReplayFfwd, PadCheck.Keyboard))
                            {
                                int cstate = state_indic[veh];

                                if (cstate == ind_state_h)
                                {
                                    state_indic[veh] = ind_state_o;
                                    actv_ind_timer = false;
                                    Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                }
                                else
                                {
                                    state_indic[veh] = ind_state_h;
                                    Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                }

                                TogIndicStateForVeh(veh, state_indic[veh]);
                                actv_ind_timer = false;
                                count_ind_timer = 0;
                                count_bcast_timer = delay_bcast_timer;
                            }
                            else if (Input.IsControlJustPressed(Control.ThrowGrenade, PadCheck.Keyboard))
                            {
                                if (SirenToggle)
                                {
                                    Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                    SirenToggle = false;
                                    BaseScript.TriggerServerEvent("lprp:SilentSiren", SirenToggle);
                                }
                                else
                                {
                                    Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                    SirenToggle = true;
                                    BaseScript.TriggerServerEvent("lprp:SilentSiren", SirenToggle);
                                }
                            }
                            else if (Input.IsControlPressed(Control.FrontendLb, PadCheck.Controller))
                            {
                                Game.DisableControlThisFrame(2, Control.VehicleExit);
                                Game.DisableControlThisFrame(0, Control.ReplayScreenshot);
                                Game.DisableControlThisFrame(2, Control.VehicleHorn);
                                Game.DisableControlThisFrame(2, Control.VehicleRadioWheel);
                                Game.DisableControlThisFrame(2, Control.VehicleHeadlight);
                                Game.DisableControlThisFrame(2, Control.PhoneUp);

                                if (Input.IsDisabledControlJustPressed(Control.VehicleRadioWheel, PadCheck.Controller))
                                {
                                    int cstate = state_indic[veh];

                                    if (cstate == ind_state_r)
                                    {
                                        state_indic[veh] = ind_state_o;
                                        actv_ind_timer = false;
                                        Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                    }
                                    else
                                    {
                                        state_indic[veh] = ind_state_r;
                                        actv_ind_timer = true;
                                        Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                    }

                                    TogIndicStateForVeh(veh, state_indic[veh]);
                                    count_ind_timer = 0;
                                    count_bcast_timer = delay_bcast_timer;
                                }
                                else if (Input.IsDisabledControlJustPressed(Control.VehicleHeadlight, PadCheck.Controller))
                                {
                                    int cstate = state_indic[veh];

                                    if (cstate == ind_state_l)
                                    {
                                        state_indic[veh] = ind_state_o;
                                        actv_ind_timer = false;
                                        Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                    }
                                    else
                                    {
                                        state_indic[veh] = ind_state_l;
                                        actv_ind_timer = true;
                                        Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                    }

                                    TogIndicStateForVeh(veh, state_indic[veh]);
                                    count_ind_timer = 0;
                                    count_bcast_timer = delay_bcast_timer;
                                }
                                else if (Input.IsDisabledControlJustPressed(Control.PhoneUp, PadCheck.Controller))
                                {
                                    int cstate = state_indic[veh];

                                    if (cstate == ind_state_h)
                                    {
                                        state_indic[veh] = ind_state_o;
                                        actv_ind_timer = false;
                                        Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                    }
                                    else
                                    {
                                        state_indic[veh] = ind_state_h;
                                        Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                    }

                                    TogIndicStateForVeh(veh, state_indic[veh]);
                                    actv_ind_timer = false;
                                    count_ind_timer = 0;
                                    count_bcast_timer = delay_bcast_timer;
                                }
                                else if (Input.IsDisabledControlJustPressed(Control.VehicleHorn, PadCheck.Controller))
                                {
                                    if (SirenToggle)
                                    {
                                        Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                        SirenToggle = false;
                                        BaseScript.TriggerServerEvent("lprp:SilentSiren", SirenToggle);
                                    }
                                    else
                                    {
                                        Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                        SirenToggle = true;
                                        BaseScript.TriggerServerEvent("lprp:SilentSiren", SirenToggle);
                                    }
                                }
                            }
                        }

                        if (count_bcast_timer > delay_bcast_timer)
                        {
                            count_bcast_timer = 0;
                            BaseScript.TriggerServerEvent("lprp:lvc_TogIndicState_s", state_indic[veh]);
                        }
                        else
                        {
                            count_bcast_timer = count_bcast_timer + 1;
                        }
                    }
                }
            }

            await Task.FromResult(0);
        }

        public static void updateSirens(string pid, bool toggle)
        {
            Vehicle veh = new Ped(GetPlayerPed(GetPlayerFromServerId(int.Parse(pid)))).CurrentVehicle;
            veh.IsSirenSilent = toggle;
        }

        public static void TogIndicStateForVeh(Vehicle veh, int newstate)
        {
            if (!veh.Exists() || veh.IsDead) return;

            if (newstate == ind_state_o)
            {
                veh.IsLeftIndicatorLightOn = false;
                veh.IsRightIndicatorLightOn = false;
            }
            else if (newstate == ind_state_l)
            {
                veh.IsLeftIndicatorLightOn = false;
                veh.IsRightIndicatorLightOn = true;
            }
            else if (newstate == ind_state_r)
            {
                veh.IsLeftIndicatorLightOn = true;
                veh.IsRightIndicatorLightOn = false;
            }
            else if (newstate == ind_state_h)
            {
                veh.IsLeftIndicatorLightOn = true;
                veh.IsRightIndicatorLightOn = true;
            }

            state_indic[veh] = newstate;
        }

        public static void lvc_TogIndicState_c(string sender, int newstate)
        {
            int player_s = GetPlayerFromServerId(int.Parse(sender));
            Ped ped_s = new Ped(GetPlayerPed(player_s));

            if (!ped_s.Exists() || ped_s.IsDead) return;
            if (ped_s == Cache.PlayerCache.MyPlayer.Ped) return;
            if (!ped_s.IsInVehicle()) return;
            Vehicle veh = ped_s.CurrentVehicle;
            TogIndicStateForVeh(veh, newstate);
        }

        private static float angle = 0f;

        public static async Task gestioneVeh()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            if (Main.spawned)
            {
                DisableControlAction(2, 80, true);

                if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo)
                {
                    Vehicle veh = playerPed.CurrentVehicle;

                    if (veh == null) return;
                    float tangle = veh.SteeringAngle;
                    if (tangle > 10f || tangle < -10f) angle = tangle;
                    if (veh.Speed < 0.1f && veh.Exists() && !GetIsTaskActive(playerPed.Handle, 151) && !veh.IsEngineRunning) veh.SteeringAngle = angle;
                    if (playerPed.SeatIndex == VehicleSeat.Passenger)
                        if (GetIsTaskActive(playerPed.Handle, 165) && !playerPed.IsAiming)
                            playerPed.SetIntoVehicle(veh, VehicleSeat.Passenger);
                }

                if (!IsPedInAnyVehicle(playerPed.Handle, false) && playerPed.LastVehicle != null && playerPed.LastVehicle.Exists() && !playerPed.LastVehicle.IsEngineRunning && acceso) playerPed.LastVehicle.IsEngineRunning = true;
            }
        }

        public static async Task engine()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo)
            {
                Vehicle p = playerPed.CurrentVehicle;

                if (playerPed.SeatIndex == VehicleSeat.Driver)
                {
                    if (p.IsEngineRunning)
                    {
                        p.IsEngineRunning = false;
                        p.IsDriveable = false;
                        acceso = false;
                        HUD.ShowNotification("Motore spento.", ColoreNotifica.Cyan);
                    }
                    else
                    {
                        p.IsEngineRunning = true;
                        p.IsDriveable = true;
                        HUD.ShowNotification("Motore acceso.", ColoreNotifica.Cyan);
                        acceso = true;
                    }
                }
            }

            await Task.FromResult(0);
        }

        public static async void spawnRentVehicle(string model, int num)
        {
            Vector4 spawn = new Vector4();

            if (!Funzioni.IsSpawnPointClear(new Vector3(carGarageSpawn[num].X, carGarageSpawn[num].Y, carGarageSpawn[num].Z), 5f))
            {
                if (Funzioni.IsSpawnPointClear(new Vector3(carGarageSpawnAlt[num][0].X, carGarageSpawnAlt[num][0].Y, carGarageSpawnAlt[num][0].Z), 5f))
                    spawn = carGarageSpawnAlt[num][0];
                else if (Funzioni.IsSpawnPointClear(new Vector3(carGarageSpawnAlt[num][1].X, carGarageSpawnAlt[num][1].Y, carGarageSpawnAlt[num][1].Z), 5f))
                    spawn = carGarageSpawnAlt[num][1];
                else if (Funzioni.IsSpawnPointClear(new Vector3(carGarageSpawnAlt[num][2].X, carGarageSpawnAlt[num][2].Y, carGarageSpawnAlt[num][2].Z), 5f))
                    spawn = carGarageSpawnAlt[num][2];
                else if (Funzioni.IsSpawnPointClear(new Vector3(carGarageSpawnAlt[num][3].X, carGarageSpawnAlt[num][3].Y, carGarageSpawnAlt[num][3].Z), 5f)) spawn = carGarageSpawnAlt[num][3];
            }
            else
            {
                spawn = carGarageSpawn[num];
            }

            MenuHandler.CloseAndClearHistory();
            Vehicle veicolo = await Funzioni.SpawnVehicleNoPlayerInside(model, new Vector3(spawn.X, spawn.Y, spawn.Z), spawn.W);
            veicolo.DirtLevel = 0f;
            veicolo.MarkAsNoLongerNeeded();
            veicoloinaffitto = veicolo;
            SaveVehicle(veicolo);
        }

        public static async void SpawnVehiclePreview(string name, Vector3 coords, float heading)
        {
            Vehicle[] vehs = Funzioni.GetVehiclesInArea(coords, 3f);
            foreach (Vehicle v in vehs) v.Delete();
            RequestCollisionAtCoord(coords.X, coords.Y, coords.Z);

            if (previewedVehicle.Exists())
            {
                previewedVehicle.Delete();
                Client.Instance.RemoveTick(previewHeading);
            }

            if (Funzioni.IsSpawnPointClear(coords, 1f))
            {
                previewedVehicle = await Funzioni.SpawnLocalVehicle(name, coords, heading);
                previewedVehicle.IsPositionFrozen = true;
                previewedVehicle.IsInvincible = true;
                previewedVehicle.AreLightsOn = true;
                previewedVehicle.IsInteriorLightOn = true;
                previewedVehicle.LockStatus = VehicleLockStatus.Locked;
                SetEntityCollision(previewedVehicle.Handle, false, true);
                previewedVehicle.DirtLevel = 0f;
                previewedVehicle.PlaceOnGround();
                garageCam.PointAt(previewedVehicle);
                Client.Instance.AddTick(previewHeading);
            }
        }

        private static async Task previewHeading()
        {
            if (previewedVehicle.Exists())
            {
                float v = previewedVehicle.Heading;
                float k = 0;
                if (previewedVehicle.Model.IsBicycle)
                    k = 0.35f;
                else if (previewedVehicle.Model.IsCar)
                    k = 0.43f;
                else if (previewedVehicle.Model.IsBike) k = 0.38f;
                previewedVehicle.Heading = v - k;
            }
        }

        public static async void setupGarageCamera(bool toggle, int num)
        {
            if (toggle)
            {
                Screen.Fading.FadeOut(500);
                await BaseScript.Delay(500);
                RequestCollisionAtCoord(cargaragecamcoords[num].X, cargaragecamcoords[num].Y, cargaragecamcoords[num].Z);
                RequestAdditionalCollisionAtCoord(cargaragecamcoords[num].X, cargaragecamcoords[num].Y, cargaragecamcoords[num].Z);
                garageCam = new Camera(CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", cargaragecamcoords[num].X, cargaragecamcoords[num].Y, cargaragecamcoords[num].Z, 0, 0, 0, GetGameplayCamFov(), false, 2));
                garageCam.IsActive = true;
                RenderScriptCams(true, false, 1300, true, false);
                Screen.Fading.FadeIn(500);
                await BaseScript.Delay(500);
            }
            else
            {
                if (garageCam.IsActive)
                {
                    Screen.Fading.FadeOut(500);
                    await BaseScript.Delay(500);
                    RenderScriptCams(false, false, 1300, true, false);
                    garageCam.Delete();
                    await BaseScript.Delay(500);
                    Screen.Fading.FadeIn(500);
                }
            }
        }

        public static void SaveVehicle(Vehicle veh)
        {
            Blip blip = veh.AttachBlip();
            if (veh.Model.IsBicycle || veh.Model.IsBike)
                blip.Sprite = BlipSprite.PersonalVehicleBike;
            else if (veh.Model.IsCar)
                blip.Sprite = BlipSprite.PersonalVehicleCar;
            else if (veh.Model.IsHelicopter)
                blip.Sprite = BlipSprite.Helicopter;
            else if (veh.Model.IsBoat)
                blip.Sprite = BlipSprite.Speedboat;
            else if (veh.Model.IsPlane) blip.Sprite = BlipSprite.Plane;
            blip.IsShortRange = true;
            SetBlipDisplay(blip.Handle, 4);
            blip.Name = MenuAffittoVeicoli.veicoloInAffitto.name + " (In Affitto)";
        }

        /*
		public static async Task MostraMenuAffitto()
		{
			Ped playerPed = Cache.PlayerPed;
			if (!MenuHandler.IsAnyMenuOpen)
			{
				for (int i = 0; i < carGarageSpots.Count; i++)
				{
					if (playerPed.IsInRangeOf(carGarageSpots[i], 1.375f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per affittare un veicolo");
						if (Input.IsControlJustPressed(Control.Context) && !MenuHandler.IsAnyMenuOpen)
						{
							MenuAffittoVeicoli.MenuAffitto(i);
						}
					}
				}
			}
		}
		*/
        public static DateTime Affitto;

        public static async Task AffittoInCorso()
        {
            await BaseScript.Delay(1200000);

            //			await BaseScript.Delay(10000);
            if (MenuAffittoVeicoli.veicoloInAffitto != null)
            {
                if (Cache.PlayerCache.MyPlayer.User.Bank >= MenuAffittoVeicoli.veicoloInAffitto.price)
                {
                    BaseScript.TriggerServerEvent("lprp:removebank", MenuAffittoVeicoli.veicoloInAffitto.price);
                    HUD.ShowNotification("Hai pagato $" + MenuAffittoVeicoli.veicoloInAffitto.price + " per l'affitto del veicolo", ColoreNotifica.GreenLight, true);
                    BaseScript.TriggerServerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], ha pagato ${MenuAffittoVeicoli.veicoloInAffitto.price} per il rinnovo dell'affitto di un/a {MenuAffittoVeicoli.veicoloInAffitto.name}");
                }
                else
                {
                    HUD.ShowNotification("NON hai abbastanza ~b~Soldi~w~ per coprire la rata oraria di affitto del Veicolo!", ColoreNotifica.Red, true);
                    await BaseScript.Delay(100);
                    HUD.ShowNotification("Il Veicolo smetterà di funzionare e verrà riportato al garage di competenza!");
                    veicoloinaffitto.IsInvincible = true;
                    veicoloinaffitto.FuelLevel = 0f;
                    veicoloinaffitto.IsEngineRunning = false;
                    veicoloinaffitto.IsDriveable = false;
                    if (veicoloinaffitto.Model.IsBicycle) veicoloinaffitto.IsPositionFrozen = true;
                    await BaseScript.Delay(30000);
                    veicoloinaffitto.Delete(); // magari sostituire con soccorso
                    BaseScript.TriggerServerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"Il Signor {Cache.PlayerCache.MyPlayer.User.FullName}, [{Cache.PlayerCache.MyPlayer.Player.Name}], non ha potuto pagare l'affitto di ${MenuAffittoVeicoli.veicoloInAffitto.price} per il rinnovo di un/a {MenuAffittoVeicoli.veicoloInAffitto.name}, ed il veicolo è stato eliminato");
                    await BaseScript.Delay(100);
                    MenuAffittoVeicoli.veicoloInAffitto = null;
                    HUD.ShowNotification("Il veicolo che hai affittato è stato riportato al garage di competenza.");
                }
            }
        }
    }
}