using System.Collections.Generic;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Core;

namespace Settings.Client.Configuration.Main
{
    public class ConfMainRP
    {
        public string ServerName { get; set; }
        public string DiscordAppId { get; set; }
        public string DiscordRichPresenceAsset { get; set; }
        public bool PassengerDriveBy { get; set; }
        public bool KickWarning { get; set; }
        public int AFKCheckTime { get; set; }
        public Vector4 Firstcoords { get; set; }
        public int ReviveReward { get; set; }
        public bool EarlyRespawn { get; set; }
        public bool EarlyRespawnFine { get; set; }
        public int EarlyRespawnFineAmount { get; set; }
        public int EarlySpawnTimer { get; set; }
        public int BleedoutTimer { get; set; }
        public bool ExcludeCrosshair { get; set; }
        public List<WeaponHash> ScopedWeapons { get; set; }
        public Dictionary<long, float> Recoils { get; set; }

        public List<string> PickupList { get; set; }

        public List<GenericPeds> StripClub { get; set; }
        public List<GenericPeds> BlackMarket { get; set; }
        public List<GenericPeds> Illegal_weapon_extra_shop { get; set; }
        public float BaseTraffic { get; set; }
        public float DivMultiplier { get; set; }
    }

    public class ClientConfigKVP
    {
        public bool CinemaMode { get; set; }
        public float LetterBox { get; set; }
        public string Filter { get; set; }
        public float FilterStrenght { get; set; }
        public bool MinimapEnabled { get; set; }
        public int MinimapSize { get; set; }
        public bool MinimapInCar { get; set; }
        public bool ShowPhoneContactsInMap { get; set; } // da valutare
        public bool ForceFirstPersonAiming { get; set; }
        public bool ForceFirstPersonCover { get; set; }
        public bool ForceFirstPersonDriving { get; set; }

        public ClientConfigKVP()
        {
            CinemaMode = false;
            LetterBox = 0f;
            Filter = "None";
            FilterStrenght = 0.5f;
            MinimapEnabled = true;
            MinimapSize = 0;
            MinimapInCar = true;
            ShowPhoneContactsInMap = false;
            ForceFirstPersonAiming = false;
            ForceFirstPersonCover = false;
            ForceFirstPersonDriving = false;
        }
    }
}