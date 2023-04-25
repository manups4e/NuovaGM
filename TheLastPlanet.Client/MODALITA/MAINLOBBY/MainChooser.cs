using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Ingresso;

namespace TheLastPlanet.Client.MODALITA.MAINLOBBY
{
    internal static class MainChooser
    {
        public static Dictionary<ModalitaServer, int> Bucket_n_Players { get; set; }

        private static BucketMarker RP_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1266.863f, -3013.068f, -49.0f), new Vector3(10f, 10f, 1f), Colors.RoyalBlue), "", "mp_mission_name_freemode_199");
        private static BucketMarker Mini_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1280.206f, -3021.234f, -49.0f), new Vector3(10f, 10f, 1f), Colors.ForestGreen), "", "mp_mission_name_freemode_1999");
        private static BucketMarker Gare_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1267.147f, -3032.353f, -49.0f), new Vector3(10f, 10f, 1f), Colors.MediumPurple), "", "mp_mission_name_freemode_19999");
        private static BucketMarker Nego_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1251.566f, -3032.304f, -49.0f), new Vector3(10f, 10f, 1f), Colors.Orange), "", "mp_mission_name_freemode_199999");
        private static BucketMarker Roam_Marker = new(new MarkerEx(MarkerType.VerticalCylinder, new Position(-1250.61f, -3007.73f, -49.0f), new Vector3(10f, 10f, 1f), Colors.Indigo), "", "mp_mission_name_freemode_1999999");

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
                RP_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Immergiti nella simulazione!", "~b~Pianeta RolePlay~w~", "", "", "", "", $"{Bucket_n_Players[ModalitaServer.Roleplay]} / 512", "", "", "");
                Mini_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Minigiochi a squadre o singoli!", "~g~Pianeta Minigiochi~w~", "", "", "", "", $"{Bucket_n_Players[ModalitaServer.Minigiochi]} / 64", "", "", "");
                Gare_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Gareggia contro tutti!", "~p~Pianeta Gare~w~", "", "", "", "", $"{Bucket_n_Players[ModalitaServer.Gare]} / 64", "", "", "");
                Nego_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Non influisce sul server RolePlay!", "~o~Negozio~w~", "", "", "", "", "", "", "", "");
                Roam_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "PVP in piena libertà!", "~f~Pianeta FreeRoam~w~", "", "", "", "", $"{Bucket_n_Players[ModalitaServer.FreeRoam]} / 512", "", "", "");

                RP_Marker.Draw();
                Mini_Marker.Draw();
                Gare_Marker.Draw();
                Nego_Marker.Draw();
                Roam_Marker.Draw();
            }
            if (RP_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo del ~b~RolePlay~w~");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    if (Bucket_n_Players[ModalitaServer.Roleplay] >= 512)
                    {
                        HUD.ShowNotification("Il Pianeta RolePlay è pieno al momento, riprova più tardi!", ColoreNotifica.Red, true);

                        return;
                    }

                    await CambiaBucket("~b~Pianeta RolePlay~w~", ModalitaServer.Roleplay);
                    await ROLEPLAY.Initializer.Init();
                    Stop();
                }
            }

            if (Mini_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo dei ~g~Minigiochi~w~");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    if (PlayerCache.MyPlayer.User.group_level >= UserGroup.Founder)
                    {
                        /*
                        if (Bucket_n_Players[2] == 64)
                        {
                            HUD.ShowNotification("Il mondo dei Minigiochi è pieno al momento, riprova più tardi!", ColoreNotifica.Red, true);

                            return;
                        }
                        */
                        await CambiaBucket("~g~Server Minigiochi~w~", ModalitaServer.Minigiochi);
                        Screen.Fading.FadeIn(1000);
                    }
                    else
                    {
                        HUD.ShowNotification("Questo pianeta è al momento irraggiungibile!!", ColoreNotifica.Red, true);
                    }
                }
            }

            if (Gare_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo delle ~p~Gare~w~");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    if (PlayerCache.MyPlayer.User.group_level >= UserGroup.Founder)
                    {
                        /*
                        if (Bucket_n_Players[3] == 64)
                        {
                            HUD.ShowNotification("Il mondo delle Gare è pieno al momento, riprova più tardi!", ColoreNotifica.Red, true);

                            return;
                        }
                        */
                        await CambiaBucket("~p~Server Gare~w~", ModalitaServer.Gare);
                        Races.Creator.RaceCreator.CreatorPreparation();
                        await Task.FromResult(0);
                    }
                    else
                    {
                        HUD.ShowNotification("Questo pianeta è al momento irraggiungibile!!", ColoreNotifica.Red, true);
                    }
                }
            }

            if (Nego_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel ~o~Negozio~w~");

                if (PlayerCache.MyPlayer.User.group_level >= UserGroup.Founder)
                {
                    if (Input.IsControlJustPressed(Control.Context))
                    {
                        await CambiaBucket("~o~Negozio~w~", ModalitaServer.Negozio);
                        Screen.Fading.FadeIn(1000);
                    }
                }
                else
                {
                    HUD.ShowNotification("Questo pianeta è al momento irraggiungibile!!", ColoreNotifica.Red, true);
                }
            }

            if (Roam_Marker.Marker.IsInMarker)
            {
                HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel ~f~Pianeta FreeRoam~w~");

                if (Input.IsControlJustPressed(Control.Context))
                {
                    if (Bucket_n_Players[ModalitaServer.FreeRoam] >= 512)
                    {
                        HUD.ShowNotification("Il pianeta FreeRoam è pieno al momento, riprova più tardi!", ColoreNotifica.Red, true);

                        return;
                    }

                    await CambiaBucket("~f~Pianeta FreeRoam~w~", ModalitaServer.FreeRoam);
                    //Screen.Fading.FadeIn(1000);
                    await FREEROAM.Initializer.Init();
                    Stop();
                }
            }
        }

        private static async Task CambiaBucket(string nome, ModalitaServer modalita)
        {
            Screen.Fading.FadeOut(500);
            await BaseScript.Delay(600);
            ScaleformUI.ScaleformUI.Warning.ShowWarning(nome, "Ingresso nella sezione in corso...", "Attendi...");
            await BaseScript.Delay(100);
            Screen.Fading.FadeIn(0);
            await BaseScript.Delay(3000);
            bool dentro = await EventDispatcher.Get<bool>("tlg:checkSeGiaDentro", modalita);

            if (dentro)
            {
                ScaleformUI.ScaleformUI.Warning.UpdateWarning(nome, "Errore nel caricamento...", "Ritorno alla lobby!");
                ServerJoining.ReturnToLobby();
                await BaseScript.Delay(3000);
                ScaleformUI.ScaleformUI.Warning.Dispose();

                return;
            }

            string settings = await EventDispatcher.Get<string>("Config.CallClientConfig", modalita);
            if (modalita == ModalitaServer.Roleplay)
            {
                string sharedSettings = await EventDispatcher.Get<string>("Config.CallSharedConfig", modalita);
                ConfigShared.SharedConfig = sharedSettings.FromJson<SharedConfig>();
            }
            Client.Impostazioni.LoadConfig(modalita, settings);
            ScaleformUI.ScaleformUI.Warning.UpdateWarning(nome, "Caricamento completato!");
            Cache.PlayerCache.MyPlayer.Status.PlayerStates.Modalita = modalita;
            Cache.PlayerCache.ModalitàAttuale = modalita;
            Cache.PlayerCache.MyPlayer.Status.PlayerStates.ModalitaPassiva = false;
            await BaseScript.Delay(2000);
            Screen.Fading.FadeOut(0);
            await BaseScript.Delay(100);
            ScaleformUI.ScaleformUI.Warning.Dispose();
        }

        private static void PassiveMode(int userId, string type, bool active)
        {
            if (type == "ModPassiva")
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