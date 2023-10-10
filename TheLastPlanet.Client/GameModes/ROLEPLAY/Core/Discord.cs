using System;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.CharCreation;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Core
{
    static class Discord
    {
        public static void Init()
        {
            SetDiscordAppId(Client.Settings.RolePlay.Main.DiscordAppId);
            SetDiscordRichPresenceAsset(Client.Settings.RolePlay.Main.DiscordRichPresenceAsset);
            SetDiscordRichPresenceAssetText("Discord.gg/n4ep9Fq"); // todo: change with the right server
            Client.Instance.AddTick(RichPresence);
        }

        private static async Task RichPresence()
        {
            await Cache.PlayerCache.Loaded();
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;
            Player player = Cache.PlayerCache.MyPlayer.Player;
            PlayerClient client = PlayerCache.MyPlayer;
            SetDiscordAppId(Client.Settings.RolePlay.Main.DiscordAppId);
            SetDiscordRichPresenceAsset(Client.Settings.RolePlay.Main.DiscordRichPresenceAsset);
            Vector3 PedCoords = !Cache.PlayerCache.MyPlayer.Status.PlayerStates.Spawned ? playerPed.Position : Cache.PlayerCache.MyPlayer.Position.ToVector3;
            uint StreetName = 0;
            uint StreetName_2 = 0;
            GetStreetNameAtCoord(PedCoords.X, PedCoords.Y, PedCoords.Z, ref StreetName, ref StreetName_2);
            string street = GetStreetNameFromHashKey(StreetName);
            string street_2 = GetStreetNameFromHashKey(StreetName_2);

            if (!Main.spawned)
            {
                if (!GameMode.ROLEPLAY.LogIn.LogIn.GuiEnabled)
                {
                    if (Creator.Creation.Visible || Creator.Apparel.Visible || Creator.Appearances.Visible || Creator.Details.Visible || Creator.Parents.Visible || Creator.Info.Visible) SetRichPresence("Creating a Character");
                }
                else
                {
                    SetRichPresence("Selecting a character");
                }
            }
            else
            {
                // TODO: POSSIBLE METAGAME?
                if (playerPed.IsOnFoot && !playerPed.IsInWater /*&& !Lavori.Generici.Pescatore.PescatoreClient.Pescando*/ && !Jobs.Generics.Hunt.HunterClient.Hunting)
                {
                    if (playerPed.IsSprinting)
                    {
                        if (StreetName_2 != 0)
                            SetRichPresence("Runnin in " + street + " near " + street_2);
                        else
                            SetRichPresence("Runnin in " + street);
                    }
                    else if (playerPed.IsRunning)
                    {
                        if (StreetName_2 != 0)
                            SetRichPresence("In a hurry in " + street + " near " + street_2);
                        else
                            SetRichPresence("In a hurry in " + street);
                    }
                    else if (playerPed.IsWalking)
                    {
                        if (StreetName_2 != 0)
                            SetRichPresence("Walking in " + street + " near " + street_2);
                        else
                            SetRichPresence("Walking in " + street);
                    }
                    else if (IsPedStill(PlayerPedId()))
                    {
                        if (StreetName_2 != 0)
                            SetRichPresence("Stopped on foot in " + street + " near " + street_2);
                        else
                            SetRichPresence("Stopped on foot in " + street);
                    }
                }
                else if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle && !playerPed.IsInHeli && !playerPed.IsInPlane && !playerPed.IsOnFoot && !playerPed.IsInSub && !playerPed.IsInBoat)
                {
                    float KMH = (float)Math.Round(playerPed.CurrentVehicle.Speed * 3.6, 2);
                    string VehName = playerPed.CurrentVehicle.LocalizedName;

                    if (KMH > 50)
                    {
                        if (StreetName_2 != 0)
                            SetRichPresence("Speeding in " + street + " near " + street_2);
                        else
                            SetRichPresence("Speeding in " + street);
                    }
                    else if (KMH <= 50 && KMH > 0)
                    {
                        if (StreetName_2 != 0)
                            SetRichPresence("Driving in " + street + " near " + street_2);
                        else
                            SetRichPresence("Driving in " + street);
                    }
                    else if (KMH == 0)
                    {
                        if (StreetName_2 != 0)
                            SetRichPresence("Parked in " + street + " near " + street_2);
                        else
                            SetRichPresence("Parked in " + street);
                    }
                }
                else if (playerPed.IsInHeli || playerPed.IsInPlane)
                {
                    string VehName = playerPed.CurrentVehicle.LocalizedName;

                    if (playerPed.CurrentVehicle.IsInAir || GetEntityHeightAboveGround(playerPed.CurrentVehicle.Handle) > 2f)
                    {
                        if (StreetName_2 != 0)
                            SetRichPresence("Flying over " + street + " near " + street_2);
                        else
                            SetRichPresence("Landed in " + street);
                    }
                }
                else if (playerPed.IsSwimming)
                {
                    SetRichPresence("Swimming");
                }
                else if (playerPed.IsSwimmingUnderWater || playerPed.IsDiving)
                {
                    SetRichPresence("Swimming underwater");
                }
                else if (playerPed.IsInBoat && playerPed.CurrentVehicle.IsInWater)
                {
                    SetRichPresence("Sailing on a boat: " + playerPed.CurrentVehicle.LocalizedName);
                }
                else if (playerPed.IsInSub && playerPed.CurrentVehicle.IsInWater)
                {
                    SetRichPresence("Exploring the seabed in a submarine");
                }
                else if (playerPed.IsAiming || playerPed.IsAimingFromCover || playerPed.IsShooting /*&& !Lavori.Generici.Pescatore.PescatoreClient.Pescando*/ && !Jobs.Generics.Hunt.HunterClient.Hunting)
                {
                    SetRichPresence("In a firefight");
                }
                else if (client.Status.RolePlayStates.Cuffed)
                {
                    SetRichPresence("Cuffed");
                }
                else if (Main.IsDead)
                {
                    SetRichPresence("Dying");
                }
                else if (playerPed.IsDoingDriveBy)
                {
                    SetRichPresence("In a driveby");
                }
                else if (playerPed.IsInParachuteFreeFall || playerPed.ParachuteState == ParachuteState.FreeFalling)
                {
                    SetRichPresence("Parachuting");
                }
                else if (IsPedStill(PlayerPedId()) || Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle && playerPed.CurrentVehicle.Speed == 0 && (int)Math.Floor(GetTimeSinceLastInput(0) / 1000f) > (int)Math.Floor(Client.Settings.RolePlay.Main.AFKCheckTime / 2f))
                {
                    SetRichPresence("AFK in game");
                }
                else if (client.Status.PlayerStates.Paused)
                {
                    SetRichPresence("Paused");
                }
                /*
                else if (Lavori.Generici.Pescatore.PescatoreClient.Pescando)
                {
                    SetRichPresence("Sta pescando");
                }
                */
                else if (Jobs.Generics.Hunt.HunterClient.Hunting)
                {
                    SetRichPresence("Hunting");
                }
            }

            await BaseScript.Delay(1000);
        }
    }
}
