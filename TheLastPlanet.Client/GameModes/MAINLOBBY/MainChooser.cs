using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Ingresso;

namespace TheLastPlanet.Client.GameMode.MAINLOBBY
{
    internal static class MainChooser
    {
        public static Dictionary<ServerMode, int> Bucket_n_Players { get; set; }

        private static BucketMarker RP_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1266.863f, -3013.068f, -49.0f), new Vector3(10f, 10f, 1f), SColor.RoyalBlue), "", "mp_mission_name_freemode_199");
        private static BucketMarker Mini_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1280.206f, -3021.234f, -49.0f), new Vector3(10f, 10f, 1f), SColor.ForestGreen), "", "mp_mission_name_freemode_1999");
        private static BucketMarker Gare_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1267.147f, -3032.353f, -49.0f), new Vector3(10f, 10f, 1f), SColor.MediumPurple), "", "mp_mission_name_freemode_19999");
        private static BucketMarker Nego_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1251.566f, -3032.304f, -49.0f), new Vector3(10f, 10f, 1f), SColor.Orange), "", "mp_mission_name_freemode_199999");
        private static BucketMarker Roam_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1250.61f, -3007.73f, -49.0f), new Vector3(10f, 10f, 1f), SColor.Indigo), "", "mp_mission_name_freemode_1999999");

        private static ParticleEffectsAssetNetworked DespawnParticle = new("scr_powerplay");
        public static void Init()
        {
            Client.Instance.AddTick(DrawMarkers);
            Client.Instance.StateBagsHandler.OnPlayerStateBagChange += PassiveMode;
        }

        public static void Stop()
        {
            Client.Instance.RemoveTick(DrawMarkers);
            Client.Instance.StateBagsHandler.OnPlayerStateBagChange -= PassiveMode;
        }

        private static Position _posRp = Position.Zero;
        private static Position _posMini = Position.Zero;
        private static Position _posGare = Position.Zero;
        private static Position _posNego = Position.Zero;
        private static Position _posRoam = Position.Zero;
        private static bool firstTick = true;

        private static async Task DrawMarkers()
        {
            await Cache.PlayerCache.Loaded();
            if (firstTick)
            {
                StopPlayerSwitch();
                await BaseScript.Delay(1000);
                long txd = CreateRuntimeTxd("thelastgalaxy");
                long _titledui = CreateDui("https://c.tenor.com/2jV0hjUDz6QAAAAC/galaxy-stars.gif", 498, 290);
                long _logodui = CreateDui("https://giphy.com/embed/VI2UC13hwWin1MIfmi", 80, 80);
                CreateRuntimeTextureFromDuiHandle(txd, "bannerbackground", GetDuiHandle(_titledui));
                CreateRuntimeTextureFromDuiHandle(txd, "serverlogo", GetDuiHandle(_logodui));
                firstTick = false;
            }

            if (_posRp == Position.Zero)
            {
                _posRp = await RP_Marker.Marker.Position.GetPositionWithGroundZ();
                RP_Marker.Marker.Position = _posRp;
            }

            if (_posMini == Position.Zero)
            {
                _posMini = await Mini_Marker.Marker.Position.GetPositionWithGroundZ();
                Mini_Marker.Marker.Position = _posMini;
            }

            if (_posGare == Position.Zero)
            {
                _posGare = await Gare_Marker.Marker.Position.GetPositionWithGroundZ();
                Gare_Marker.Marker.Position = _posGare;
            }

            if (_posNego == Position.Zero)
            {
                _posNego = await Nego_Marker.Marker.Position.GetPositionWithGroundZ();
                Nego_Marker.Marker.Position = _posNego;
            }

            if (_posRoam == Position.Zero)
            {
                _posRoam = await Roam_Marker.Marker.Position.GetPositionWithGroundZ();
                Roam_Marker.Marker.Position = _posRoam;
            }

            if (!Game.IsPaused)
            {
                RP_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Immerse yourself in the simulation!", "~b~RolePlay Planet~w~", "", "", "", "", $"{Bucket_n_Players[ServerMode.Roleplay]} / 512", "", "", "");
                Mini_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Minigames in team or single!", "~g~Minigiochi Planet~w~", "", "", "", "", $"{Bucket_n_Players[ServerMode.Minigames]} / 64", "", "", "");
                Gare_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Race against humanity!", "~p~Race Planet~w~", "", "", "", "", $"{Bucket_n_Players[ServerMode.Races]} / 64", "", "", "");
                Nego_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "It does not affect the RolePlay server!", "~o~Store~w~", "", "", "", "", "", "", "", "");
                Roam_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "PVPVE freedom!", "~f~FreeRoam Planet~w~", "", "", "", "", $"{Bucket_n_Players[ServerMode.FreeRoam]} / 512", "", "", "");

                RP_Marker.Draw();
                Mini_Marker.Draw();
                Gare_Marker.Draw();
                Nego_Marker.Draw();
                Roam_Marker.Draw();
            }
            if (RP_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ To join the ~b~RolePlay~w~ world");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    if (Bucket_n_Players[ServerMode.Roleplay] >= 512)
                    {
                        HUD.ShowNotification("RolePlay planet is full, try again later!", ColoreNotifica.Red, true);

                        return;
                    }

                    await ChangeBucket("~b~RolePlay Planet~w~", ServerMode.Roleplay);
                    await ROLEPLAY.Initializer.Init();
                    Stop();
                }
            }

            if (Mini_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ ~g~Minigiochi~w~ world.");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    // TODO: to be removed
                    if (PlayerCache.MyPlayer.User.group_level >= UserGroup.Founder)
                    {
                        /*
                        if (Bucket_n_Players[2] == 64)
                        {
                            HUD.ShowNotification("Minigames world is full at the moment, try again later!", ColoreNotifica.Red, true);

                            return;
                        }
                        */
                        await ChangeBucket("~g~Server Minigiochi~w~", ServerMode.Minigames);
                        Screen.Fading.FadeIn(1000);
                    }
                    else
                    {
                        HUD.ShowNotification("This planet cannot be reached for the moment!!", ColoreNotifica.Red, true);
                    }
                }
            }

            if (Gare_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ ~p~Gare~w~ world.");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    // TODO: to be removed
                    if (PlayerCache.MyPlayer.User.group_level >= UserGroup.Founder)
                    {
                        /*
                        if (Bucket_n_Players[3] == 64)
                        {
                            HUD.ShowNotification("Races world is full at the moment, try again later!", ColoreNotifica.Red, true);

                            return;
                        }
                        */
                        await ChangeBucket("~p~Races world~w~", ServerMode.Races);
                        Races.Creator.RaceCreator.CreatorPreparation();
                        await Task.FromResult(0);
                    }
                    else
                    {
                        HUD.ShowNotification("This planet cannot be reached for the moment!!", ColoreNotifica.Red, true);
                    }
                }
            }

            if (Nego_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to enter the ~o~Store~w~");

                if (PlayerCache.MyPlayer.User.group_level >= UserGroup.Founder)
                {
                    if (Input.IsControlJustPressed(Control.Context))
                    {
                        await ChangeBucket("~o~Store~w~", ServerMode.Store);
                        Screen.Fading.FadeIn(1000);
                    }
                }
                else
                {
                    HUD.ShowNotification("This planet cannot be reached for the moment!!", ColoreNotifica.Red, true);
                }
            }

            if (Roam_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Press ~INPUT_CONTEXT~ to enter ~f~FreeRoam Planet~w~");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    if (Bucket_n_Players[ServerMode.FreeRoam] >= 512)
                    {
                        HUD.ShowNotification("FreeRoam world is full at the moment, try again later!", ColoreNotifica.Red, true);

                        return;
                    }

                    await ChangeBucket("~f~FreeRoam Planet~w~", ServerMode.FreeRoam);
                    //Screen.Fading.FadeIn(1000);
                    await FREEROAM.Initializer.Init();
                    Stop();
                }
            }
        }

        private static async Task ChangeBucket(string nome, ServerMode modalita)
        {
            Screen.Fading.FadeOut(500);
            await BaseScript.Delay(600);
            ScaleformUI.Main.Warning.ShowWarning(nome, "Joining in progress...", "Wait...");
            await BaseScript.Delay(100);
            Screen.Fading.FadeIn(0);
            await BaseScript.Delay(3000);
            bool alreadyInside = await EventDispatcher.Get<bool>("tlg:checkSeGiaDentro", modalita);

            if (alreadyInside)
            {
                ScaleformUI.Main.Warning.UpdateWarning(nome, "Loading error...", "Going back to Lobby!");
                ServerJoining.ReturnToLobby();
                await BaseScript.Delay(3000);
                ScaleformUI.Main.Warning.Dispose();

                return;
            }

            string settings = await EventDispatcher.Get<string>("Config.CallClientConfig", modalita);
            if (modalita == ServerMode.Roleplay)
            {
                string sharedSettings = await EventDispatcher.Get<string>("Config.CallSharedConfig", modalita);
                ConfigShared.SharedConfig = sharedSettings.FromJson<SharedConfig>();
            }
            Client.Settings.LoadConfig(modalita, settings);
            ScaleformUI.Main.Warning.UpdateWarning(nome, "Loading completed!");
            Cache.PlayerCache.MyPlayer.Status.PlayerStates.Mode = modalita;
            Cache.PlayerCache.ActualMode = modalita;
            Cache.PlayerCache.MyPlayer.Status.PlayerStates.PassiveMode = false;
            await BaseScript.Delay(2000);
            Screen.Fading.FadeOut(0);
            await BaseScript.Delay(100);
            ScaleformUI.Main.Warning.Dispose();
        }

        private static void PassiveMode(int userId, string type, bool active)
        {
            if (type == "PassiveMode")
            {
                if (userId == PlayerCache.MyPlayer.Handle)
                {
                    Ped ped = PlayerCache.MyPlayer.Ped;
                    if (active)
                    {
                        ped.CanBeDraggedOutOfVehicle = false;
                        ped.Weapons.Select(WeaponHash.Unarmed);
                        ped.SetConfigFlag(342, true);
                        ped.SetConfigFlag(122, true);
                        SetPlayerVehicleDefenseModifier(PlayerCache.MyPlayer.Player.Handle, 0.5f);
                        NetworkSetPlayerIsPassive(true);
                        NetworkSetFriendlyFireOption(false);
                        SetCanAttackFriendly(PlayerPedId(), false, false);
                    }
                    else
                    {
                        ped.CanBeDraggedOutOfVehicle = true;
                        ped.SetConfigFlag(342, false);
                        ped.SetConfigFlag(122, false);
                        SetPlayerVehicleDefenseModifier(PlayerCache.MyPlayer.Player.Handle, 1f);
                        NetworkSetPlayerIsPassive(false);
                        NetworkSetFriendlyFireOption(true);
                        SetCanAttackFriendly(PlayerPedId(), true, false);
                    }
                }
            }
        }
    }
}