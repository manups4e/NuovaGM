using System;
using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.RolePlay.Core;

namespace Impostazioni.Client.Configurazione.Main
{
    public class ConfPrincipale
    {
        public string NomeServer = "The Last Planet";
        public string DiscordAppId = "625350962776637491";
        public string DiscordRichPresenceAsset = "majoras2";
        public bool PassengerDriveBy = true;
        public bool KickWarning = true;
        public int AFKCheckTime = 600;
        public Vector4 Firstcoords = new Vector4(1f);
        public int ReviveReward = 700;
        public bool EarlyRespawn = true;
        public bool EarlyRespawnFine = false;
        public int EarlyRespawnFineAmount = 5000;
        public int EarlySpawnTimer = 300;
        public int BleedoutTimer = 300;
        public bool EscludiMirino = true;

        public List<WeaponHash> ScopedWeapons = new List<WeaponHash>()
        {
            WeaponHash.SniperRifle,
            WeaponHash.HeavySniper,
            WeaponHash.MarksmanRifle
        };

        public Dictionary<long, float> recoils = new Dictionary<long, float>()
        {
            [453432689] = 0.3f,
            [3219281620] = 0.3f,
            [1593441988] = 0.2f,
            [584646201] = 0.1f,
            [2578377531] = 0.6f,
            [324215364] = 0.2f,
            [736523883] = 0.1f,
            [2024373456] = 0.1f,
            [4024951519] = 0.1f,
            [3220176749] = 0.2f,
            [961495388] = 0.2f,
            [2210333304] = 0.1f,
            [4208062921] = 0.1f,
            [2937143193] = 0.1f,
            [2634544996] = 0.1f,
            [2144741730] = 0.1f,
            [3686625920] = 0.1f,
            [487013001] = 0.4f,
            [1432025498] = 0.4f,
            [2017895192] = 0.7f,
            [3800352039] = 0.4f,
            [2640438543] = 0.2f,
            [911657153] = 0.1f,
            [100416529] = 0.5f,
            [205991906] = 0.3f,
            [177293209] = 0.7f,
            [856002082] = 1.2f,
            [2726580491] = 1.0f,
            [1305664598] = 1.0f,
            [2982836145] = 0.0f,
            [1752584910] = 0.0f,
            [1119849093] = 0.01f,
            [3218215474] = 0.2f,
            [2009644972] = 0.25f,
            [1627465347] = 0.1f,
            [3231910285] = 0.2f,
            [-1768145561] = 0.25f,
            [3523564046] = 0.5f,
            [2132975508] = 0.2f,
            [-2066285827] = 0.25f,
            [137902532] = 0.4f,
            [-1746263880] = 0.4f,
            [2828843422] = 0.7f,
            [984333226] = 0.2f,
            [3342088282] = 0.3f,
            [1785463520] = 0.35f,
            [1672152130] = 0f,
            [1198879012] = 0.9f,
            [171789620] = 0.2f,
            [3696079510] = 0.9f,
            [1834241177] = 2.4f,
            [3675956304] = 0.3f,
            [3249783761] = 0.6f,
            [-879347409] = 0.65f,
            [4019527611] = 0.7f,
            [1649403952] = 0.3f,
            [317205821] = 0.2f,
            [125959754] = 0.5f,
            [3173288789] = 0.1f,
        };

        public List<string> pickupList = new()
        {
            "PICKUP_AMMO_BULLET_MP",
            "PICKUP_AMMO_FIREWORK",
            "PICKUP_AMMO_FLAREGUN",
            "PICKUP_AMMO_GRENADELAUNCHER",
            "PICKUP_AMMO_GRENADELAUNCHER_MP",
            "PICKUP_AMMO_HOMINGLAUNCHER",
            "PICKUP_AMMO_MG",
            "PICKUP_AMMO_MINIGUN",
            "PICKUP_AMMO_MISSILE_MP",
            "PICKUP_AMMO_PISTOL",
            "PICKUP_AMMO_RIFLE",
            "PICKUP_AMMO_RPG",
            "PICKUP_AMMO_SHOTGUN",
            "PICKUP_AMMO_SMG",
            "PICKUP_AMMO_SNIPER",
            "PICKUP_ARMOUR_STANDARD",
            "PICKUP_CAMERA",
            "PICKUP_CUSTOM_SCRIPT",
            "PICKUP_GANG_ATTACK_MONEY",
            "PICKUP_HEALTH_SNACK",
            "PICKUP_HEALTH_STANDARD",
            "PICKUP_MONEY_CASE",
            "PICKUP_MONEY_DEP_BAG",
            "PICKUP_MONEY_MED_BAG",
            "PICKUP_MONEY_PAPER_BAG",
            "PICKUP_MONEY_PURSE",
            "PICKUP_MONEY_SECURITY_CASE",
            "PICKUP_MONEY_VARIABLE",
            "PICKUP_MONEY_WALLET",
            "PICKUP_PARACHUTE",
            "PICKUP_PORTABLE_CRATE_FIXED_INCAR",
            "PICKUP_PORTABLE_CRATE_UNFIXED",
            "PICKUP_PORTABLE_CRATE_UNFIXED_INCAR",
            "PICKUP_PORTABLE_CRATE_UNFIXED_INCAR_SMALL",
            "PICKUP_PORTABLE_CRATE_UNFIXED_LOW_GLOW",
            "PICKUP_PORTABLE_DLC_VEHICLE_PACKAGE",
            "PICKUP_PORTABLE_PACKAGE",
            "PICKUP_SUBMARINE",
            "PICKUP_VEHICLE_ARMOUR_STANDARD",
            "PICKUP_VEHICLE_CUSTOM_SCRIPT",
            "PICKUP_VEHICLE_CUSTOM_SCRIPT_LOW_GLOW",
            "PICKUP_VEHICLE_HEALTH_STANDARD",
            "PICKUP_VEHICLE_HEALTH_STANDARD_LOW_GLOW",
            "PICKUP_VEHICLE_MONEY_VARIABLE",
            "PICKUP_VEHICLE_WEAPON_APPISTOL",
            "PICKUP_VEHICLE_WEAPON_ASSAULTSMG",
            "PICKUP_VEHICLE_WEAPON_COMBATPISTOL",
            "PICKUP_VEHICLE_WEAPON_GRENADE",
            "PICKUP_VEHICLE_WEAPON_MICROSMG",
            "PICKUP_VEHICLE_WEAPON_MOLOTOV",
            "PICKUP_VEHICLE_WEAPON_PISTOL",
            "PICKUP_VEHICLE_WEAPON_PISTOL50",
            "PICKUP_VEHICLE_WEAPON_SAWNOFF",
            "PICKUP_VEHICLE_WEAPON_SMG",
            "PICKUP_VEHICLE_WEAPON_SMOKEGRENADE",
            "PICKUP_VEHICLE_WEAPON_STICKYBOMB",
            "PICKUP_WEAPON_ADVANCEDRIFLE",
            "PICKUP_WEAPON_APPISTOL",
            "PICKUP_WEAPON_ASSAULTRIFLE",
            "PICKUP_WEAPON_ASSAULTSHOTGUN",
            "PICKUP_WEAPON_ASSAULTSMG",
            "PICKUP_WEAPON_AUTOSHOTGUN",
            "PICKUP_WEAPON_BAT",
            "PICKUP_WEAPON_BATTLEAXE",
            "PICKUP_WEAPON_BOTTLE",
            "PICKUP_WEAPON_BULLPUPRIFLE",
            "PICKUP_WEAPON_BULLPUPSHOTGUN",
            "PICKUP_WEAPON_CARBINERIFLE",
            "PICKUP_WEAPON_COMBATMG",
            "PICKUP_WEAPON_COMBATPDW",
            "PICKUP_WEAPON_COMBATPISTOL",
            "PICKUP_WEAPON_COMPACTLAUNCHER",
            "PICKUP_WEAPON_COMPACTRIFLE",
            "PICKUP_WEAPON_CROWBAR",
            "PICKUP_WEAPON_DAGGER",
            "PICKUP_WEAPON_DBSHOTGUN",
            "PICKUP_WEAPON_FIREWORK",
            "PICKUP_WEAPON_FLAREGUN",
            "PICKUP_WEAPON_FLASHLIGHT",
            "PICKUP_WEAPON_GRENADE",
            "PICKUP_WEAPON_GRENADELAUNCHER",
            "PICKUP_WEAPON_GUSENBERG",
            "PICKUP_WEAPON_GOLFCLUB",
            "PICKUP_WEAPON_HAMMER",
            "PICKUP_WEAPON_HATCHET",
            "PICKUP_WEAPON_HEAVYPISTOL",
            "PICKUP_WEAPON_HEAVYSHOTGUN",
            "PICKUP_WEAPON_HEAVYSNIPER",
            "PICKUP_WEAPON_HOMINGLAUNCHER",
            "PICKUP_WEAPON_KNIFE",
            "PICKUP_WEAPON_KNUCKLE",
            "PICKUP_WEAPON_MACHETE",
            "PICKUP_WEAPON_MACHINEPISTOL",
            "PICKUP_WEAPON_MARKSMANPISTOL",
            "PICKUP_WEAPON_MARKSMANRIFLE",
            "PICKUP_WEAPON_MG",
            "PICKUP_WEAPON_MICROSMG",
            "PICKUP_WEAPON_MINIGUN",
            "PICKUP_WEAPON_MINISMG",
            "PICKUP_WEAPON_MOLOTOV",
            "PICKUP_WEAPON_MUSKET",
            "PICKUP_WEAPON_NIGHTSTICK",
            "PICKUP_WEAPON_PETROLCAN",
            "PICKUP_WEAPON_PIPEBOMB",
            "PICKUP_WEAPON_PISTOL",
            "PICKUP_WEAPON_PISTOL50",
            "PICKUP_WEAPON_POOLCUE",
            "PICKUP_WEAPON_PROXMINE",
            "PICKUP_WEAPON_PUMPSHOTGUN",
            "PICKUP_WEAPON_RAILGUN",
            "PICKUP_WEAPON_REVOLVER",
            "PICKUP_WEAPON_RPG",
            "PICKUP_WEAPON_SAWNOFfsHOTGUN",
            "PICKUP_WEAPON_SMG",
            "PICKUP_WEAPON_SMOKEGRENADE",
            "PICKUP_WEAPON_SNIPERRIFLE",
            "PICKUP_WEAPON_SNSPISTOL",
            "PICKUP_WEAPON_SPECIALCARBINE",
            "PICKUP_WEAPON_STICKYBOMB",
            "PICKUP_WEAPON_STUNGUN",
            "PICKUP_WEAPON_SWITCHBLADE",
            "PICKUP_WEAPON_VINTAGEPISTOL",
            "PICKUP_WEAPON_WRENCH"
        };

        public List<GenericPeds> stripClub = new List<GenericPeds>()
        {
            new GenericPeds(new Vector3(102.423f, -1290.594f, 28.2587f), "mini@strip_club@private_dance@part1", "priv_dance_p1", "CSB_Stripper_02", new Random().Next(50, 360) * 1.0f),
            new GenericPeds(new Vector3(104.256f, -1294.67f, 28.2587f), "mini@strip_club@private_dance@part3", "priv_dance_p3", "CSB_Stripper_02", new Random().Next(50, 360) * 1.0f),
            new GenericPeds(new Vector3(112.480f, -1287.032f, 27.586f), "mini@strip_club@private_dance@part2", "priv_dance_p2", "CSB_Stripper_02", new Random().Next(50, 360) * 1.0f),
            new GenericPeds(new Vector3(113.111f, -1287.755f, 27.586f), "mini@strip_club@private_dance@part1", "priv_dance_p1", "CSB_Stripper_02", new Random().Next(50, 360) * 1.0f),
            new GenericPeds(new Vector3(113.375f, -1286.546f, 27.586f), "mini@strip_club@private_dance@part2", "priv_dance_p2", "CSB_Stripper_02", new Random().Next(50, 360) * 1.0f),
            new GenericPeds(new Vector3(129.442f, -1283.407f, 28.272f), "missfbi3_party_d", "stand_talk_loop_a_female", "S_F_Y_Bartender_01", 122.471f),
        };
        public List<GenericPeds> blackMarket = new List<GenericPeds>()
        {
            new GenericPeds(new Vector3(-2166.786f, 5197.684f, 15.880f), "", "", "G_M_Y_SALVABOSS_01", "WORLD_HUMAN_SMOKING", 122.471f),
        };
        public List<GenericPeds> illegal_weapon_extra_shop = new List<GenericPeds>()
        {
            new GenericPeds(new Vector3(181.4f, 2792.8f, 45.7f), "", "", "G_M_Y_SALVABOSS_01", "WORLD_HUMAN_SMOKING", 302.471f),
        };
        public float baseTraffic = 0.9999999f;
        public float divMultiplier = 2f;
    }
}