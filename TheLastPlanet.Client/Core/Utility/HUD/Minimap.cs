using System.Threading.Tasks;


namespace TheLastPlanet.Client.Core.Utility.HUD
{
    internal static class Minimap
    {
        //public static Scaleform minimap = new Scaleform("MINIMAP");
        public static async void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            AccessingEvents.OnFreeRoamSpawn += Spawned;
            AccessingEvents.OnFreeRoamLeave += onPlayerLeft;

        }
        public static void Spawned(PlayerClient client)
        {
            //Client.Instance.AddTick(MinimapDrawing);
            Screen.Hud.IsRadarVisible = true;
            DisplayRadar(true);
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            //Client.Instance.RemoveTick(MinimapDrawing);
        }

        public static async Task MinimapDrawing()
        {
            await PlayerCache.Loaded();

            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            switch (Cache.PlayerCache.ActualMode)
            {
                case ServerMode.Lobby:
                    if (Screen.Hud.IsRadarVisible)
                        Screen.Hud.IsRadarVisible = false;
                    break;
                case ServerMode.FreeRoam:
                    if (PlayerCache.MyPlayer.Status.Instance.Instance == "CreazionePersonaggio")
                    {
                        if (Screen.Hud.IsRadarVisible)
                            Screen.Hud.IsRadarVisible = false;
                        break;
                    }
                    if (!Screen.Hud.IsRadarVisible)
                        Screen.Hud.IsRadarVisible = true;
                    break;
                case ServerMode.Roleplay:
                    // IF I'M NOT HIDING HUD (cinematic)
                    if (!GameMode.ROLEPLAY.Core.Main.ClientConfig.CinemaMode)
                    {
                        if (GameMode.ROLEPLAY.Core.Main.ClientConfig.EnableMinimap)
                        {
                            if (!IsRadarEnabled()) Screen.Hud.IsRadarVisible = true;

                            switch (GameMode.ROLEPLAY.Core.Main.ClientConfig.MinimapSize)
                            {
                                // CHOOSE TINY MAP
                                case 0:
                                    {
                                        if (IsBigmapActive())
                                            SetBigmapActive(false, false);
                                        break;
                                    }
                                // ELSE
                                case 1:
                                    {
                                        if (!IsBigmapActive())
                                            SetBigmapActive(true, false);
                                        break;
                                    }
                            }

                            switch (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                            {
                                //IF I'M NOT IN A VEH AND PAUSE MENU IS NOT ACTIVE
                                case false when !IsPauseMenuActive():
                                    ////DisableRadarThisFrame(); // THIS DISABLES RADAR BUT KEEPS MINIMAP ACTIVE

                                    break;
                                case true when GameMode.ROLEPLAY.Core.Main.ClientConfig.InCarMinimap:
                                    {
                                        Vehicle veh = p.CurrentVehicle;

                                        if (veh == null) return;
                                        if (veh.Model.IsBicycle || IsThisModelAJetski((uint)veh.Model.Hash) || veh.Model.IsQuadbike || !veh.IsEngineRunning)
                                        {
                                            //DisableRadarThisFrame();
                                        }

                                        break;
                                    }
                                case true:
                                    //DisableRadarThisFrame();
                                    break;
                            }
                        }
                        else
                        {
                            if (IsRadarEnabled()) Screen.Hud.IsRadarVisible = false;
                        }
                    }
                    else
                    {
                        if (IsRadarEnabled()) Screen.Hud.IsRadarVisible = false;
                    }
                    break;
            }
            await Task.FromResult(0);
        }
    }
}