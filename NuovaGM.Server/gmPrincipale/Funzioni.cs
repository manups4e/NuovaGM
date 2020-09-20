using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Logger;
using NuovaGM.Shared;

namespace NuovaGM.Server.gmPrincipale
{
	public static class Funzioni
	{
		public static void Init()
		{
			Server.Instance.AddTick(Salvataggio);
		}

		public static Player GetPlayerFromId(int id)
		{
			foreach (Player p in Server.Instance.GetPlayers.ToList())
			{
				if (p.Handle == id.ToString())
					return p;
			}
			return null;
		}
		public static Player GetPlayerFromId(string id)
		{
			foreach (Player p in Server.Instance.GetPlayers.ToList())
			{
				if (p.Handle == id)
					return p;
			}
			return null;
		}

		public static ConcurrentDictionary<string, string> HASH_TO_LABEL = new ConcurrentDictionary<string, string>()
		{
			[Convert.ToString((uint)GetHashKey("WEAPON_UNARMED"))] = "WT_UNARMED",
			[Convert.ToString((uint)GetHashKey("WEAPON_ANIMAL"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_COUGAR"))] = "WT_RAGE",
			[Convert.ToString((uint)GetHashKey("WEAPON_KNIFE"))] = "WT_KNIFE",
			[Convert.ToString((uint)GetHashKey("WEAPON_NIGHTSTICK"))] = "WT_NGTSTK",
			[Convert.ToString((uint)GetHashKey("WEAPON_HAMMER"))] = "WT_HAMMER",
			[Convert.ToString((uint)GetHashKey("WEAPON_BAT"))] = "WT_BAT",
			[Convert.ToString((uint)GetHashKey("WEAPON_GOLFCLUB"))] = "WT_GOLFCLUB",
			[Convert.ToString((uint)GetHashKey("WEAPON_CROWBAR"))] = "WT_CROWBAR",
			[Convert.ToString((uint)GetHashKey("WEAPON_PISTOL"))] = "WT_PIST",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMBATPISTOL"))] = "WT_PIST_CBT",
			[Convert.ToString((uint)GetHashKey("WEAPON_APPISTOL"))] = "WT_PIST_AP",
			[Convert.ToString((uint)GetHashKey("WEAPON_PISTOL50"))] = "WT_PIST_50",
			[Convert.ToString((uint)GetHashKey("WEAPON_MICROSMG"))] = "WT_SMG_MCR",
			[Convert.ToString((uint)GetHashKey("WEAPON_SMG"))] = "WT_SMG",
			[Convert.ToString((uint)GetHashKey("WEAPON_ASSAULTSMG"))] = "WT_SMG_ASL",
			[Convert.ToString((uint)GetHashKey("WEAPON_ASSAULTRIFLE"))] = "WT_RIFLE_ASL",
			[Convert.ToString((uint)GetHashKey("WEAPON_CARBINERIFLE"))] = "WT_RIFLE_CBN",
			[Convert.ToString((uint)GetHashKey("WEAPON_ADVANCEDRIFLE"))] = "WT_RIFLE_ADV",
			[Convert.ToString((uint)GetHashKey("WEAPON_MG"))] = "WT_MG",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMBATMG"))] = "WT_MG_CBT",
			[Convert.ToString((uint)GetHashKey("WEAPON_PUMPSHOTGUN"))] = "WT_SG_PMP",
			[Convert.ToString((uint)GetHashKey("WEAPON_SAWNOFfsHOTGUN"))] = "WT_SG_SOF",
			[Convert.ToString((uint)GetHashKey("WEAPON_ASSAULTSHOTGUN"))] = "WT_SG_ASL",
			[Convert.ToString((uint)GetHashKey("WEAPON_BULLPUPSHOTGUN"))] = "WT_SG_BLP",
			[Convert.ToString((uint)GetHashKey("WEAPON_STUNGUN"))] = "WT_STUN",
			[Convert.ToString((uint)GetHashKey("WEAPON_SNIPERRIFLE"))] = "WT_SNIP_RIF",
			[Convert.ToString((uint)GetHashKey("WEAPON_HEAVYSNIPER"))] = "WT_SNIP_HVY",
			[Convert.ToString((uint)GetHashKey("WEAPON_REMOTESNIPER"))] = "WT_SNIP_RMT",
			[Convert.ToString((uint)GetHashKey("WEAPON_GRENADELAUNCHER"))] = "WT_GL",
			[Convert.ToString((uint)GetHashKey("WEAPON_GRENADELAUNCHER_SMOKE"))] = "WT_GL_SMOKE",
			[Convert.ToString((uint)GetHashKey("WEAPON_RPG"))] = "WT_RPG",
			[Convert.ToString((uint)GetHashKey("WEAPON_PASSENGER_ROCKET"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_AIRSTRIKE_ROCKET"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_STINGER"))] = "WT_RPG",
			[Convert.ToString((uint)GetHashKey("WEAPON_MINIGUN"))] = "WT_MINIGUN",
			[Convert.ToString((uint)GetHashKey("WEAPON_GRENADE"))] = "WT_GNADE",
			[Convert.ToString((uint)GetHashKey("WEAPON_STICKYBOMB"))] = "WT_GNADE_STK",
			[Convert.ToString((uint)GetHashKey("WEAPON_SMOKEGRENADE"))] = "WT_GNADE_SMK",
			[Convert.ToString((uint)GetHashKey("WEAPON_BZGAS"))] = "WT_BZGAS",
			[Convert.ToString((uint)GetHashKey("WEAPON_MOLOTOV"))] = "WT_MOLOTOV",
			[Convert.ToString((uint)GetHashKey("WEAPON_FIREEXTINGUISHER"))] = "WT_FIRE",
			[Convert.ToString((uint)GetHashKey("WEAPON_PETROLCAN"))] = "WT_PETROL",
			[Convert.ToString((uint)GetHashKey("WEAPON_DIGISCANNER"))] = "WT_DIGI",
			[Convert.ToString((uint)GetHashKey("GADGET_NIGHTVISION"))] = "WT_NV",
			[Convert.ToString((uint)GetHashKey("OBJECT"))] = "WT_OBJECT",
			[Convert.ToString((uint)GetHashKey("WEAPON_BRIEFCASE"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_BRIEFCASE_02"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_BALL"))] = "WT_BALL",
			[Convert.ToString((uint)GetHashKey("WEAPON_FLARE"))] = "WT_FLARE",
			[Convert.ToString((uint)GetHashKey("WEAPON_ELECTRIC_FENCE"))] = "WT_ELCFEN",
			[Convert.ToString((uint)GetHashKey("VEHICLE_WEAPON_TANK"))] = "WT_V_TANK",
			[Convert.ToString((uint)GetHashKey("VEHICLE_WEAPON_SPACE_ROCKET"))] = "WT_V_SPACERKT",
			[Convert.ToString((uint)GetHashKey("VEHICLE_WEAPON_PLAYER_LASER"))] = "WT_V_PLRLSR",
			[Convert.ToString((uint)GetHashKey("AMMO_RPG"))] = "WT_A_RPG",
			[Convert.ToString((uint)GetHashKey("AMMO_TANK"))] = "WT_A_TANK",
			[Convert.ToString((uint)GetHashKey("AMMO_SPACE_ROCKET"))] = "WT_A_SPACERKT",
			[Convert.ToString((uint)GetHashKey("AMMO_PLAYER_LASER"))] = "WT_A_PLRLSR",
			[Convert.ToString((uint)GetHashKey("AMMO_ENEMY_LASER"))] = "WT_A_ENMYLSR",
			[Convert.ToString((uint)GetHashKey("WEAPON_RAMMED_BY_CAR"))] = "WT_PIST",
			[Convert.ToString((uint)GetHashKey("WEAPON_BOTTLE"))] = "WT_BOTTLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_GUSENBERG"))] = "WT_GUSENBERG",
			[Convert.ToString((uint)GetHashKey("WEAPON_SNSPISTOL"))] = "WT_SNSPISTOL",
			[Convert.ToString((uint)GetHashKey("WEAPON_VINTAGEPISTOL"))] = "WT_VPISTOL",
			[Convert.ToString((uint)GetHashKey("WEAPON_DAGGER"))] = "WT_DAGGER",
			[Convert.ToString((uint)GetHashKey("WEAPON_FLAREGUN"))] = "WT_FLAREGUN",
			[Convert.ToString((uint)GetHashKey("WEAPON_HEAVYPISTOL"))] = "WT_HEAVYPSTL",
			[Convert.ToString((uint)GetHashKey("WEAPON_SPECIALCARBINE"))] = "WT_RIFLE_SCBN",
			[Convert.ToString((uint)GetHashKey("WEAPON_MUSKET"))] = "WT_MUSKET",
			[Convert.ToString((uint)GetHashKey("WEAPON_FIREWORK"))] = "WT_FWRKLNCHR",
			[Convert.ToString((uint)GetHashKey("WEAPON_MARKSMANRIFLE"))] = "WT_MKRIFLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_HEAVYSHOTGUN"))] = "WT_HVYSHOT",
			[Convert.ToString((uint)GetHashKey("WEAPON_PROXMINE"))] = "WT_PRXMINE",
			[Convert.ToString((uint)GetHashKey("WEAPON_HOMINGLAUNCHER"))] = "WT_HOMLNCH",
			[Convert.ToString((uint)GetHashKey("WEAPON_HATCHET"))] = "WT_HATCHET",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMBATPDW"))] = "WT_COMBATPDW",
			[Convert.ToString((uint)GetHashKey("WEAPON_KNUCKLE"))] = "WT_KNUCKLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_MARKSMANPISTOL"))] = "WT_MKPISTOL",
			[Convert.ToString((uint)GetHashKey("WEAPON_MACHETE"))] = "WT_MACHETE",
			[Convert.ToString((uint)GetHashKey("WEAPON_MACHINEPISTOL"))] = "WT_MCHPIST",
			[Convert.ToString((uint)GetHashKey("WEAPON_FLASHLIGHT"))] = "WT_FLASHLIGHT",
			[Convert.ToString((uint)GetHashKey("WEAPON_DBSHOTGUN"))] = "WT_DBSHGN",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMPACTRIFLE"))] = "WT_CMPRIFLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_SWITCHBLADE"))] = "WT_SWBLADE",
			[Convert.ToString((uint)GetHashKey("WEAPON_REVOLVER"))] = "WT_REVOLVER",
			[Convert.ToString((uint)GetHashKey("WEAPON_FIRE"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_HELI_CRASH"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_RUN_OVER_BY_CAR"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_HIT_BY_WATER_CANNON"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_EXHAUSTION"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_FALL"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_EXPLOSION"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_BLEEDING"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_DROWNING_IN_VEHICLE"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_DROWNING"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_BARBED_WIRE"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_VEHICLE_ROCKET"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_SNSPISTOL_MK2"))] = "WT_SNSPISTOL2",
			[Convert.ToString((uint)GetHashKey("WEAPON_REVOLVER_MK2"))] = "WT_REVOLVER2",
			[Convert.ToString((uint)GetHashKey("WEAPON_DOUBLEACTION"))] = "WT_REV_DA",
			[Convert.ToString((uint)GetHashKey("WEAPON_SPECIALCARBINE_MK2"))] = "WT_SPCARBINE2",
			[Convert.ToString((uint)GetHashKey("WEAPON_BULLPUPRIFLE_MK2"))] = "WT_BULLRIFLE2",
			[Convert.ToString((uint)GetHashKey("WEAPON_PUMPSHOTGUN_MK2"))] = "WT_SG_PMP2",
			[Convert.ToString((uint)GetHashKey("WEAPON_MARKSMANRIFLE_MK2"))] = "WT_MKRIFLE2",
			[Convert.ToString((uint)GetHashKey("WEAPON_POOLCUE"))] = "WT_POOLCUE",
			[Convert.ToString((uint)GetHashKey("WEAPON_WRENCH"))] = "WT_WRENCH",
			[Convert.ToString((uint)GetHashKey("WEAPON_BATTLEAXE"))] = "WT_BATTLEAXE",
			[Convert.ToString((uint)GetHashKey("WEAPON_MINISMG"))] = "WT_MINISMG",
			[Convert.ToString((uint)GetHashKey("WEAPON_BULLPUPRIFLE"))] = "WT_BULLRIFLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_AUTOSHOTGUN"))] = "WT_AUTOSHGN",
			[Convert.ToString((uint)GetHashKey("WEAPON_RAILGUN"))] = "WT_RAILGUN",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMPACTLAUNCHER"))] = "WT_CMPGL",
			[Convert.ToString((uint)GetHashKey("WEAPON_SNOWBALL"))] = "WT_SNWBALL",
			[Convert.ToString((uint)GetHashKey("WEAPON_PIPEBOMB"))] = "WT_PIPEBOMB",
			[Convert.ToString((uint)GetHashKey("GADGET_NIGHTVISION"))] = "WT_NV",
			[Convert.ToString((uint)GetHashKey("GADGET_PARACHUTE"))] = "WT_PARA",
			[Convert.ToString((uint)GetHashKey("WEAPON_STONE_HATCHET"))] = "WT_SHATCHET",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_PI_FLSH"))] = "WCT_FLASH",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_PI_SUPP_02"))] = "WCT_SUPP",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_PI_SUPP"))] = "WCT_SUPP",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPISTOL_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_APPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_APPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_APPISTOL_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL50_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL50_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_AR_SUPP_02"))] = "WCT_SUPP",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL50_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SNSPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SNSPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SNSPISTOL_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_HEAVYPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_HEAVYPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_HEAVYPISTOL_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_VINTAGEPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_VINTAGEPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MICROSMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MICROSMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_MACRO"))] = "WCT_SCOPE_MAC",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MICROSMG_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SMG_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_MACRO_02"))] = "WCT_SCOPE_MAC",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SMG_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSMG_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MINISMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MINISMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MACHINEPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MACHINEPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MACHINEPISTOL_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPDW_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPDW_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPDW_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_AR_AFGRIP"))] = "WCT_GRIP",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_SMALL"))] = "WCT_SCOPE_SML",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PUMPSHOTGUN_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SAWNOFfsHOTGUN_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSHOTGUN_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSHOTGUN_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTRIFLE_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTRIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_CARBINERIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_CARBINERIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_CARBINERIFLE_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_MEDIUM"))] = "WCT_SCOPE_MED",
			[Convert.ToString((uint)GetHashKey("COMPONENT_CARBINERIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ADVANCEDRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ADVANCEDRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ADVANCEDRIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SPECIALCARBINE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SPECIALCARBINE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SPECIALCARBINE_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SPECIALCARBINE_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_BULLPUPRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_BULLPUPRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_BULLPUPRIFLE_VARMOD_LOW"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMPACTRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMPACTRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMPACTRIFLE_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MG_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATMG_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_GUSENBERG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_GUSENBERG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_LARGE"))] = "WCT_SCOPE_LRG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_MAX"))] = "WCT_SCOPE_MAX",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SNIPERRIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MARKSMANRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MARKSMANRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM"))] = "WCT_SCOPE_LRG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MARKSMANRIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("WM_TINT0"))] = "WM_TINT0",
			[Convert.ToString((uint)GetHashKey("WM_TINT1"))] = "WM_TINT1",
			[Convert.ToString((uint)GetHashKey("WM_TINT2"))] = "WM_TINT2",
			[Convert.ToString((uint)GetHashKey("WM_TINT3"))] = "WM_TINT3",
			[Convert.ToString((uint)GetHashKey("WM_TINT4"))] = "WM_TINT4",
			[Convert.ToString((uint)GetHashKey("WM_TINT5"))] = "WM_TINT5",
			[Convert.ToString((uint)GetHashKey("WM_TINT6"))] = "WM_TINT6",
			[Convert.ToString((uint)GetHashKey("WM_TINT7"))] = "WM_TINT7",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_BASE"))] = "WCT_KNUCK_01",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_PIMP"))] = "WCT_KNUCK_02",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_BALLAS"))] = "WCT_KNUCK_BG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_DOLLAR"))] = "WCT_KNUCK_DLR",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_DIAMOND"))] = "WCT_KNUCK_DMD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_HATE"))] = "WCT_KNUCK_HT",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_LOVE"))] = "WCD_VAR_DESC",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_PLAYER"))] = "WCT_KNUCK_PC",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_KING"))] = "WCT_KNUCK_SLG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_VAGOS"))] = "WCT_KNUCK_VG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SWITCHBLADE_VARMOD_BASE"))] = "WCT_SB_BASE",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SWITCHBLADE_VARMOD_VAR1"))] = "WCT_SB_VAR1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SWITCHBLADE_VARMOD_VAR2"))] = "WCT_SB_VAR2"
		};

		public static string GetWeaponLabel(uint hash)
		{
			if (HASH_TO_LABEL.ContainsKey(hash.ToString()))
			{
				string label = HASH_TO_LABEL[hash.ToString()];
				if (label != null)
				{
					return label;
				}
			}
			else
			{
				Log.Printa(LogType.Error, "Errore nell'hash /" + hash.ToString() + "/ per arma/componente. forse non è mai stato aggiunto?");
			}

			return "WT_INVALID";
		}

		public static async Task SalvaPersonaggio(Player player)
		{
			var ped = GetUserFromPlayerId(player.Handle);
			await Server.Instance.Execute("UPDATE `users` SET `Name` = @name, `group` = @gr, `group_level` = @level, `playTime` = @time, `char_current` = @current, `char_data` = @data WHERE `discord` = @id", new
			{
				name = player.Name,
				gr = ped.group,
				level = ped.group_level,
				time = ped.playTime,
				current = ped.char_current,
				data = ped.char_data.Serialize(),
				id = ped.identifiers.discord
			});
			await BaseScript.Delay(0);
			await Task.FromResult(0);
		}

		public static async Task Salvataggio()
		{
			try
			{
				if (Server.PlayerList.Count > 0)
				{
					await BaseScript.Delay(Server.Impostazioni.Main.SalvataggioTutti * 60000);
					foreach (Player player in Server.Instance.GetPlayers.ToList())
					{
						string name = player.Name;
						if (Server.PlayerList.ContainsKey(player.Handle))
						{
							var ped = Funzioni.GetUserFromPlayerId(player.Handle);
							if (ped.status.spawned)
							{
								BaseScript.TriggerClientEvent(player, "lprp:mostrasalvataggio");
								await SalvaPersonaggio(player);
								Log.Printa(LogType.Info, "Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' - " + ped.identifiers.discord);
								BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' - " + ped.identifiers.discord);
								await Task.FromResult(0);
							}
						}
					}
					BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", Server.PlayerList.Serialize());
				}
				else
					await BaseScript.Delay(10000);
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e.ToString() + e.StackTrace);
			}
		}

		public static bool IsPlayerAndHasPermission(int player, int level)
		{
			if (player != 0)
			{
				Player p = GetPlayerFromId(player);
				User Char = GetUserFromPlayerId(p.Handle);
				if (Char.group_level >= level) return true;
			}
			return false;
		}


		public static DateTime TimeStamp2DateTime(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		public static double DateTime2TimeStamp(DateTime dateTime)
		{
			return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		public static User GetUserFromPlayerId(string id)
		{
			User user;
			if (Server.PlayerList.TryGetValue(id, out user))
				return user;
			return null;
		}

		public static User GetUserFromPlayerId(int id)
		{
			User user;
			if (Server.PlayerList.TryGetValue(id.ToString(), out user))
				return user;
			return null;
		}

		public static User GetCurrentChar(this Player player)
		{
			User user;
			if (Server.PlayerList.TryGetValue(player.Handle, out user))
				return user;
			return null;
		}

		private static Random random = new Random();
		public static float RandomFloatInRange(float minimum, float maximum)
		{
			return (float)random.NextDouble() * (maximum - minimum) + minimum;
		}
	}
}