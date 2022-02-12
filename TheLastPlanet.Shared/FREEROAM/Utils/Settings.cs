using System.Collections.Generic;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
    [Serialization]
    public partial class FreeRoamSettings
    {
    }

    [Serialization]
    public partial class WeaponTintSettings
    {
        public int Index;
        public string Name;
        public int Kills;
        public int Cash;
    }


    [Serialization]
    public partial class CratesSettings
    {
        public int Cash;
        public int NthRank;
        public EventReward Reward = new EventReward();
        public float Radius;
        public List<CratesWeapon> Weapons = new List<CratesWeapon>();
        public List<CrateLocations> Locations = new List<CrateLocations>();
    }
    [Serialization]
    public partial class CratesWeapon
    {
        public string Id;
        public string Name;
        public int Ammo;
    }
    [Serialization]
    public partial class CrateLocations
    {
        public float[] Blip = new float[3];
        public List<float[]> Positions = new List<float[]>();
    }

    [Serialization]
    public partial class HeadHunterSettings
    {
        public int Time;
        public float Radius;
        public List<HeadHunterTarget> Targets = new List<HeadHunterTarget>();
        public List<string> Weapons = new List<string>();
        public int WantedLevel;
        public AssetRecoveryRewards Rewards = new AssetRecoveryRewards();
    }
    [Serialization]
    public partial class HeadHunterTarget
    {
        public string PedModel;
        public float[] Location = new float[3];
    }

    [Serialization]
    public partial class AssetRecoveryMissionSettings
    {
        public int Time;
        public List<AssetRecoveryVariant> Variants = new List<AssetRecoveryVariant>();
        public float DropRadius;
    }
    [Serialization]
    public partial class AssetRecoveryVariant
    {
        public string Vehicle;
        public float[] VehicleLocation = new float[4];
        public float[] DropOffLocation = new float[3];
    }
    [Serialization]
    public partial class AssetRecoveryRewards
    {
        public AssetRecoveryReward Cash = new AssetRecoveryReward();
        public AssetRecoveryReward Exp = new AssetRecoveryReward();
    }
    [Serialization]
    public partial class AssetRecoveryReward
    {
        public int Min;
        public int Max;
    }


    [Serialization]
    public partial class MostWantedSettings
    {
        public int Time;
        public MostWantedRewards Rewards = new MostWantedRewards();
    }
    [Serialization]
    public partial class MostWantedRewards
    {
        public int MaxCash;
        public int MaxExp;
    }

    [Serialization]
    public partial class VelocityMissionSettings
    {
        public int EnterVehicleTime;
        public int PrepationTime;
        public int DriveTime;
        public int DetonationTime;
        public List<float[]> Locations = new List<float[]>();
        public int MinSpeed;
        public VelocityMissionRewards Rewards = new VelocityMissionRewards();
    }

    [Serialization]
    public partial class VelocityMissionRewards
    {
        public VelocityMissionReward Cash = new VelocityMissionReward();
        public VelocityMissionReward Exp = new VelocityMissionReward();
    }

    [Serialization]
    public partial class VelocityMissionReward
    {
        public int Min;
        public int Max;
        public int PerAboutToDetonate;
    }

    [Serialization]
    public partial class HeistMissionSettings
    {
        public int Time;
        public List<float[]> Places = new List<float[]>();
        public float Radius;
        public HeistTake Take = new HeistTake();
    }

    [Serialization]
    public partial class HeistTake
    {
        public int Inverval;
        public HeistRate Rate = new HeistRate();
    }
    [Serialization]
    public partial class HeistRate
    {
        public HeistCash Cash = new HeistCash();
        public float Exp;
    }
    [Serialization]
    public partial class HeistCash
    {
        public int Min;
        public int Max;
        public int Limit;
    }

    [Serialization]
    public partial class SpecialCargoMissionSettings
    {
        public string MissionName;
        public int Time;
        public int WantedLevel;
        public EventReward RewardPerPlayer = new EventReward();
        public float DeliveryRadius;
        public List<string> Goods = new List<string>();
        public List<MissionCrate> Crates = new List<MissionCrate>();
        public List<string> Vehicles = new List<string>();
        public List<CargoMissionLocation> Locations = new List<CargoMissionLocation>();
        public List<float[]> WareHouses = new List<float[]>();
    }

    [Serialization]
    public partial class CargoMissionLocation
    {
        public float[] Pos = new float[3];
        public float Heading;
        public bool Wanted;
    }

    [Serialization]
    public partial class MissionCrate
    {
        public string Name;
        public int Price;
        public EventReward Reward = new EventReward();
    }

    [Serialization]
    public partial class MissionsSettings
    {
        public int ResetTimeInterval;
        public List<float[]> Places = new List<float[]>();
        public EventReward FailedRewards = new EventReward();
        public EventReward FactionRewards = new EventReward();
    }

    [Serialization]
    public partial class AmmunationSpecialAmmoSettings
    {
        public int Ammo;
        public int Price;
        public string Type;
    }
    [Serialization]
    public partial class AmmunationRefillingWeaponsSettings
    {
        public List<string> Weapons = new List<string>();
        public int Ammo;
        public int Price;
    }
    [Serialization]
    public partial class HuntTheBeastSettings
    {
        public int Duration;
        public int Lives;
        public int TargetLandmarks;
        public List<float[]> Landmarks = new List<float[]>();
        public float Radius;
        public BeastRewardsSettings Rewards = new BeastRewardsSettings();
    }
    [Serialization]
    public partial class BeastRewardsSettings
    {
        public EventReward BeastLandmark = new EventReward();
        public EventReward Killer = new EventReward();
    }

    [Serialization]
    public partial class HotPropertySettings
    {
        public int Duration;
        public List<float[]> Places = new List<float[]>();
        public EventRewards Rewards = new EventRewards();
    }

    [Serialization]
    public partial class KingOfTheCastleSettings
    {
        public int Duration;
        public List<float[]> Places = new List<float[]>();
        public float Radius;
        public EventRewards Rewards = new EventRewards();
    }

    [Serialization]
    public partial class SharpShooterSettings
    {
        public int Duration;
        public EventRewards Rewards = new EventRewards();
    }

    [Serialization]
    public partial class StockPilingSettings
    {
        public int Duration;
        public List<float[]> CheckPoints = new List<float[]>();
        public float Radius;
        public EventRewards Rewards = new EventRewards();
    }

    [Serialization]
    public partial class GunSettings
    {
        public int Duration;
        public List<string> Categories = new List<string>();
        public EventRewards Rewards = new EventRewards();
    }
    [Serialization]
    public partial class EventRewards
    {
        public List<EventReward> Top = new List<EventReward>();
        public EventReward Point = new EventReward();
    }
    [Serialization]
    public partial class EventReward
    {
        public int Cash;
        public int Exp;
    }
    [Serialization]
    public partial class EventSettings
    {
        public int Interval;
        public int MinPlayers;
    }
    [Serialization]
    public partial class MarkerSettings
    {
        public float Radius;
        public int Opacity;
    }
    [Serialization]
    public partial class GTA2Cam
    {
        public float Min;
        public float Max;
        public float Step;
        public float MinSpeed;
        public int Key = 212; // home // INPUT_FRONTEND_SOCIAL_CLUB
    }

    [Serialization]
    public partial class SettingsSpawn
    {
        public List<float[]> SpawnPoints = new List<float[]>();
        public int DeathTime;
        public int Timeout;
        public int RespawnFasterPerControlPressed;
        public int TryCount;
        public SpawnRadius Radius = new SpawnRadius();
    }

    [Serialization]
    public partial class SpawnRadius
    {
        public float Min;
        public float Max;
        public float MinDiscanceToPlayer;
    }
}
