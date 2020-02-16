using CitizenFX.Core;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.MenuGm;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.Veicoli;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.gmPrincipale
{
	static class Main
	{
		public static string decorName;
		public static int decorInt;
		private static int HostId;
		public static bool spawned = false;

		public static List<Gang> GangsAttive = new List<Gang>();

		// NetworkConcealPlayer(Player player, bool toggle, bool p2); **This is what R* uses to hide players in MP interiors.


		static List<Vector4> SelectFirstCoords = new List<Vector4>
		{
			new Vector4(-1503.000f, -1143.462f, 34.670f, 64.692f),
			new Vector4(747.339f, 525.837f, 345.395f, 39.975f),
			new Vector4(-2162.689f, -469.343f, 3.396f, 150.975f),
			new Vector4(-170.256f, -2357.418f, 100.596f, 95.975f),
			new Vector4(2126.171f, 3014.593f, 59.196f, 115.975f),
			new Vector4(-103.310f, -1215.578f, 53.796f, 270.975f),
			new Vector4(-3032.130f, 22.216f, 11.118f, 0f),
		};

		public static bool ispointing = false;
		private static RelationshipGroup g = new RelationshipGroup(GetHashKey("PLAYER"));
		public static Vector4 charSelectCoords;
		public static Vector4 charCreateCoords = new Vector4(402.91f, -996.74f, -100.00025f, 180.086f);
		public static Vector4 firstSpawnCoords = new Vector4(ConfigClient.Conf.Main.Firstcoords[0], ConfigClient.Conf.Main.Firstcoords[1], ConfigClient.Conf.Main.Firstcoords[2], ConfigClient.Conf.Main.Firstcoords[3]);
		public static Camera charSelectionCam;
		public static Camera charCreationCam;

		static List<string> tipi = new List<string>() { "CIVMALE", "CIVFEMALE", "COP", "SECURITY_GUARD", "PRIVATE_SECURITY", "FIREMAN", "GANG_1", "GANG_2", "GANG_9", "GANG_10", "AMBIENT_GANG_LOST", "AMBIENT_GANG_MEXICAN", "AMBIENT_GANG_FAMILY", "AMBIENT_GANG_BALLAS", "AMBIENT_GANG_MARABUNTE", "AMBIENT_GANG_CULT", "AMBIENT_GANG_SALVA", "AMBIENT_GANG_WEICHENG", "AMBIENT_GANG_HILLBILLY", "DEALER", "HATES_PLAYER", "HEN", "NO_RELATIONSHIP", "SPECIAL", "MISSION2", "MISSION3", "MISSION4", "MISSION5", "MISSION6", "MISSION7", "MISSION8", "AGGRESSIVE_INVESTIGATE", "MEDIC" };
		static List<int> pickupList = new List<int>() { GetHashKey("PICKUP_AMMO_BULLET_MP"), GetHashKey("PICKUP_AMMO_FIREWORK"), GetHashKey("PICKUP_AMMO_FLAREGUN"), GetHashKey("PICKUP_AMMO_GRENADELAUNCHER"), GetHashKey("PICKUP_AMMO_GRENADELAUNCHER_MP"), GetHashKey("PICKUP_AMMO_HOMINGLAUNCHER"), GetHashKey("PICKUP_AMMO_MG"), GetHashKey("PICKUP_AMMO_MINIGUN"), GetHashKey("PICKUP_AMMO_MISSILE_MP"), GetHashKey("PICKUP_AMMO_PISTOL"), GetHashKey("PICKUP_AMMO_RIFLE"), GetHashKey("PICKUP_AMMO_RPG"), GetHashKey("PICKUP_AMMO_SHOTGUN"), GetHashKey("PICKUP_AMMO_SMG"), GetHashKey("PICKUP_AMMO_SNIPER"), GetHashKey("PICKUP_ARMOUR_STANDARD"), GetHashKey("PICKUP_CAMERA"), GetHashKey("PICKUP_CUSTOM_SCRIPT"), GetHashKey("PICKUP_GANG_ATTACK_MONEY"), GetHashKey("PICKUP_HEALTH_SNACK"), GetHashKey("PICKUP_HEALTH_STANDARD"), GetHashKey("PICKUP_MONEY_CASE"), GetHashKey("PICKUP_MONEY_DEP_BAG"), GetHashKey("PICKUP_MONEY_MED_BAG"), GetHashKey("PICKUP_MONEY_PAPER_BAG"), GetHashKey("PICKUP_MONEY_PURSE"), GetHashKey("PICKUP_MONEY_SECURITY_CASE"), GetHashKey("PICKUP_MONEY_VARIABLE"), GetHashKey("PICKUP_MONEY_WALLET"), GetHashKey("PICKUP_PARACHUTE"), GetHashKey("PICKUP_PORTABLE_CRATE_FIXED_INCAR"), GetHashKey("PICKUP_PORTABLE_CRATE_UNFIXED"), GetHashKey("PICKUP_PORTABLE_CRATE_UNFIXED_INCAR"), GetHashKey("PICKUP_PORTABLE_CRATE_UNFIXED_INCAR_SMALL"), GetHashKey("PICKUP_PORTABLE_CRATE_UNFIXED_LOW_GLOW"), GetHashKey("PICKUP_PORTABLE_DLC_VEHICLE_PACKAGE"), GetHashKey("PICKUP_PORTABLE_PACKAGE"), GetHashKey("PICKUP_SUBMARINE"), GetHashKey("PICKUP_VEHICLE_ARMOUR_STANDARD"), GetHashKey("PICKUP_VEHICLE_CUSTOM_SCRIPT"), GetHashKey("PICKUP_VEHICLE_CUSTOM_SCRIPT_LOW_GLOW"), GetHashKey("PICKUP_VEHICLE_HEALTH_STANDARD"), GetHashKey("PICKUP_VEHICLE_HEALTH_STANDARD_LOW_GLOW"), GetHashKey("PICKUP_VEHICLE_MONEY_VARIABLE"), GetHashKey("PICKUP_VEHICLE_WEAPON_APPISTOL"), GetHashKey("PICKUP_VEHICLE_WEAPON_ASSAULTSMG"), GetHashKey("PICKUP_VEHICLE_WEAPON_COMBATPISTOL"), GetHashKey("PICKUP_VEHICLE_WEAPON_GRENADE"), GetHashKey("PICKUP_VEHICLE_WEAPON_MICROSMG"), GetHashKey("PICKUP_VEHICLE_WEAPON_MOLOTOV"), GetHashKey("PICKUP_VEHICLE_WEAPON_PISTOL"), GetHashKey("PICKUP_VEHICLE_WEAPON_PISTOL50"), GetHashKey("PICKUP_VEHICLE_WEAPON_SAWNOFF"), GetHashKey("PICKUP_VEHICLE_WEAPON_SMG"), GetHashKey("PICKUP_VEHICLE_WEAPON_SMOKEGRENADE"), GetHashKey("PICKUP_VEHICLE_WEAPON_STICKYBOMB"), GetHashKey("PICKUP_WEAPON_ADVANCEDRIFLE"), GetHashKey("PICKUP_WEAPON_APPISTOL"), GetHashKey("PICKUP_WEAPON_ASSAULTRIFLE"), GetHashKey("PICKUP_WEAPON_ASSAULTSHOTGUN"), GetHashKey("PICKUP_WEAPON_ASSAULTSMG"), GetHashKey("PICKUP_WEAPON_AUTOSHOTGUN"), GetHashKey("PICKUP_WEAPON_BAT"), GetHashKey("PICKUP_WEAPON_BATTLEAXE"), GetHashKey("PICKUP_WEAPON_BOTTLE"), GetHashKey("PICKUP_WEAPON_BULLPUPRIFLE"), GetHashKey("PICKUP_WEAPON_BULLPUPSHOTGUN"), GetHashKey("PICKUP_WEAPON_CARBINERIFLE"), GetHashKey("PICKUP_WEAPON_COMBATMG"), GetHashKey("PICKUP_WEAPON_COMBATPDW"), GetHashKey("PICKUP_WEAPON_COMBATPISTOL"), GetHashKey("PICKUP_WEAPON_COMPACTLAUNCHER"), GetHashKey("PICKUP_WEAPON_COMPACTRIFLE"), GetHashKey("PICKUP_WEAPON_CROWBAR"), GetHashKey("PICKUP_WEAPON_DAGGER"), GetHashKey("PICKUP_WEAPON_DBSHOTGUN"), GetHashKey("PICKUP_WEAPON_FIREWORK"), GetHashKey("PICKUP_WEAPON_FLAREGUN"), GetHashKey("PICKUP_WEAPON_FLASHLIGHT"), GetHashKey("PICKUP_WEAPON_GRENADE"), GetHashKey("PICKUP_WEAPON_GRENADELAUNCHER"), GetHashKey("PICKUP_WEAPON_GUSENBERG"), GetHashKey("PICKUP_WEAPON_GOLFCLUB"), GetHashKey("PICKUP_WEAPON_HAMMER"), GetHashKey("PICKUP_WEAPON_HATCHET"), GetHashKey("PICKUP_WEAPON_HEAVYPISTOL"), GetHashKey("PICKUP_WEAPON_HEAVYSHOTGUN"), GetHashKey("PICKUP_WEAPON_HEAVYSNIPER"), GetHashKey("PICKUP_WEAPON_HOMINGLAUNCHER"), GetHashKey("PICKUP_WEAPON_KNIFE"), GetHashKey("PICKUP_WEAPON_KNUCKLE"), GetHashKey("PICKUP_WEAPON_MACHETE"), GetHashKey("PICKUP_WEAPON_MACHINEPISTOL"), GetHashKey("PICKUP_WEAPON_MARKSMANPISTOL"), GetHashKey("PICKUP_WEAPON_MARKSMANRIFLE"), GetHashKey("PICKUP_WEAPON_MG"), GetHashKey("PICKUP_WEAPON_MICROSMG"), GetHashKey("PICKUP_WEAPON_MINIGUN"), GetHashKey("PICKUP_WEAPON_MINISMG"), GetHashKey("PICKUP_WEAPON_MOLOTOV"), GetHashKey("PICKUP_WEAPON_MUSKET"), GetHashKey("PICKUP_WEAPON_NIGHTSTICK"), GetHashKey("PICKUP_WEAPON_PETROLCAN"), GetHashKey("PICKUP_WEAPON_PIPEBOMB"), GetHashKey("PICKUP_WEAPON_PISTOL"), GetHashKey("PICKUP_WEAPON_PISTOL50"), GetHashKey("PICKUP_WEAPON_POOLCUE"), GetHashKey("PICKUP_WEAPON_PROXMINE"), GetHashKey("PICKUP_WEAPON_PUMPSHOTGUN"), GetHashKey("PICKUP_WEAPON_RAILGUN"), GetHashKey("PICKUP_WEAPON_REVOLVER"), GetHashKey("PICKUP_WEAPON_RPG"), GetHashKey("PICKUP_WEAPON_SAWNOFfsHOTGUN"), GetHashKey("PICKUP_WEAPON_SMG"), GetHashKey("PICKUP_WEAPON_SMOKEGRENADE"), GetHashKey("PICKUP_WEAPON_SNIPERRIFLE"), GetHashKey("PICKUP_WEAPON_SNSPISTOL"), GetHashKey("PICKUP_WEAPON_SPECIALCARBINE"), GetHashKey("PICKUP_WEAPON_STICKYBOMB"), GetHashKey("PICKUP_WEAPON_STUNGUN"), GetHashKey("PICKUP_WEAPON_SWITCHBLADE"), GetHashKey("PICKUP_WEAPON_VINTAGEPISTOL"), GetHashKey("PICKUP_WEAPON_WRENCH") };
		static List<uint> scopedWeapons = ConfigClient.Conf.Main.ScopedWeapons;

		static Dictionary<string, int> LastLoadout = new Dictionary<string, int>();
		static private bool kickWarning;
		static Dictionary<long, float> recoils = new Dictionary<long, float>()
		{
			[453432689] = 0.3F,//PISTOL
			[3219281620] = 0.3F,//PISTOL MK2
			[1593441988] = 0.2F,//COMBAT PISTOL
			[584646201] = 0.1F,//AP PISTOL
			[2578377531] = 0.6F,//PISTOL .50
			[324215364] = 0.2F,//MICRO SMG
			[736523883] = 0.1F,//SMG
			[2024373456] = 0.1F,//SMG MK2
			[4024951519] = 0.1F,//ASSAULT SMG
			[3220176749] = 0.2F,//ASSAULT RIFLE
			[961495388] = 0.2F,//ASSAULT RIFLE MK2
			[2210333304] = 0.1F,//CARBINE RIFLE
			[4208062921] = 0.1F,//CARBINE RIFLE MK2
			[2937143193] = 0.1F,//ADVANCED RIFLE
			[2634544996] = 0.1F,//MG
			[2144741730] = 0.1F,//COMBAT MG
			[3686625920] = 0.1F,//COMBAT MG MK2
			[487013001] = 0.4F,//PUMP SHOTGUN
			[1432025498] = 0.4F,//PUMP SHOTGUN MK2
			[2017895192] = 0.7F,//SAWNOFF SHOTGUN
			[3800352039] = 0.4F,//ASSAULT SHOTGUN
			[2640438543] = 0.2F,//BULLPUP SHOTGUN
			[911657153] = 0.1F,//STUN GUN
			[100416529] = 0.5F,//SNIPER RIFLE
			[205991906] = 0.3F,//HEAVY SNIPER
			[177293209] = 0.7F,//HEAVY SNIPER MK2
			[856002082] = 1.2F,//REMOTE SNIPER
			[2726580491] = 1.0F,//GRENADE LAUNCHER
			[1305664598] = 1.0F,//GRENADE LAUNCHER SMOKE
			[2982836145] = 0.0F,//RPG
			[1752584910] = 0.0F,//STINGER
			[1119849093] = 0.01F,//MINIGUN
			[3218215474] = 0.2F,//SNS PISTOL
			[2009644972] = 0.25F,//SNS PISTOL MK2
			[1627465347] = 0.1F,//GUSENBERG
			[3231910285] = 0.2F,//SPECIAL CARBINE
			[-1768145561] = 0.25F,//SPECIAL CARBINE MK2
			[3523564046] = 0.5F,//HEAVY PISTOL
			[2132975508] = 0.2F,//BULLPUP RIFLE
			[-2066285827] = 0.25F,//BULLPUP RIFLE MK2
			[137902532] = 0.4F,//VINTAGE PISTOL
			[-1746263880] = 0.4F,//float ACTION REVOLVER
			[2828843422] = 0.7F,//MUSKET
			[984333226] = 0.2F,//HEAVY SHOTGUN
			[3342088282] = 0.3F,//MARKSMAN RIFLE
			[1785463520] = 0.35F,//MARKSMAN RIFLE MK2
			[1672152130] = 0F,//HOMING LAUNCHER
			[1198879012] = 0.9F,//FLARE GUN
			[171789620] = 0.2F,//COMBAT PDW
			[3696079510] = 0.9F,//MARKSMAN PISTOL
			[1834241177] = 2.4F,//RAILGUN
			[3675956304] = 0.3F,//MACHINE PISTOL
			[3249783761] = 0.6F,//REVOLVER
			[-879347409] = 0.65F,//REVOLVER MK2
			[4019527611] = 0.7F,//float BARREL SHOTGUN
			[1649403952] = 0.3F,//COMPACT RIFLE
			[317205821] = 0.2F,//AUTO SHOTGUN
			[125959754] = 0.5F,//COMPACT LAUNCHER
			[3173288789] = 0.1F,//MINI SMG
		};

		public static Dictionary<string, KeyValuePair<String, string>> Textures = new Dictionary<string, KeyValuePair<string, string>>()
		{
			["ModGarage"] = new KeyValuePair<string, string>("shopui_title_ie_modgarage", "shopui_title_ie_modgarage"),
			["Michael"] = new KeyValuePair<string, string>("shopui_title_graphics_michael", "shopui_title_graphics_michael"),
			["AmmuNation"] = new KeyValuePair<string, string>("shopui_title_gunclub", "shopui_title_gunclub"),
			["Binco"] = new KeyValuePair<string, string>("shopui_title_lowendfashion2", "shopui_title_lowendfashion2"),
			["Discount"] = new KeyValuePair<string, string>("shopui_title_lowendfashion", "shopui_title_lowendfashion"),
			["Suburban"] = new KeyValuePair<string, string>("shopui_title_midfashion", "shopui_title_midfashion"),
			["Ponsombys"] = new KeyValuePair<string, string>("shopui_title_highendfashion", "shopui_title_highendfashion"),
			["Kuts"] = new KeyValuePair<string, string>("shopui_title_barber", "shopui_title_barber"),
			["Hawick"] = new KeyValuePair<string, string>("shopui_title_barber4", "shopui_title_barber4"),
			["Osheas"] = new KeyValuePair<string, string>("shopui_title_barber3", "shopui_title_barber3"),
			["Combo"] = new KeyValuePair<string, string>("shopui_title_barber2", "shopui_title_barber2"),
			["Mulet"] = new KeyValuePair<string, string>("shopui_title_highendsalon", "shopui_title_highendsalon"),
		};

		public static bool IsDead = false;
		public static bool isAdmin = false;
		public static bool LoadoutLoaded = false;
		public static int MaxPlayers = 256;
		private static bool passengerDriveBy = ConfigClient.Conf.Main.PassengerDriveBy;
		public static int SecondsBeforeKick = ConfigClient.Conf.Main.AFKCheckTime;
		public static void SetDecor(string _name, int _int)
		{
			decorName = _name;
			decorInt = _int;
			DecorRegister(decorName, (int)DecorationType.Int);
			DecorRegister(FuelClient.DecorName, (int)DecorationType.Float);
			DecorRegister("VeicoloPolizia", (int)DecorationType.Int);
			DecorRegister("VeicoloMedici", (int)DecorationType.Int);
			DecorRegisterLock();
		}

		public static void Init()
		{
			Client.GetInstance.RegisterTickHandler(Istanza);
			Client.GetInstance.RegisterTickHandler(AFK);
			Client.GetInstance.RegisterTickHandler(HUD.Menus);
			Client.GetInstance.RegisterTickHandler(Entra);
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(onPlayerSpawn));
			Client.GetInstance.RegisterEventHandler("playerSpawned", new Action(playerSpawned));
			Client.GetInstance.RegisterEventHandler("lprp:setDecor", new Action<string, int>(SetDecor));
			Client.GetInstance.RegisterEventHandler("onClientResourceStart", new Action<string>(OnClientResourceStart));
			Client.GetInstance.RegisterEventHandler("onClientResourceStop", new Action<string>(OnClientResourceStop));
			Client.GetInstance.RegisterEventHandler("lprp:getHost", new Action<int>(GetHost));
			Client.GetInstance.RegisterEventHandler("lprp:AFKScelta", new Action<string>(AFKScelta));
			SetNuiFocus(false, false);
			BaseScript.TriggerServerEvent("lprp:getDecor");
			BaseScript.TriggerServerEvent("lprp:giostre:spawna");
			Screen.Fading.FadeOut(800);
		}
		
		private static async void OnClientResourceStart(string resourceName)
		{
			if (resourceName == "config")
			{
				BaseScript.TriggerServerEvent("lprp:riavvioApp");
				return;
			}
			else if (resourceName != GetCurrentResourceName() || resourceName != "config")
				return;
		}

		private static async void OnClientResourceStop(string resourceName)
		{
			if (resourceName == GetCurrentResourceName())
				Screen.Fading.FadeOut(800);
		}

		public static async void playerSpawned()
		{
			int a = Funzioni.GetRandomInt(SelectFirstCoords.Count);
			charSelectCoords = SelectFirstCoords[a];
			RequestCollisionAtCoord(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z);
			Game.PlayerPed.Position = new Vector3(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z - 1);
			Game.PlayerPed.Heading = charSelectCoords.W;
			Screen.Fading.FadeOut(800);
			while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
			await Game.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			Game.PlayerPed.IsVisible = false;
			Game.PlayerPed.IsPositionFrozen = true;
			Game.Player.IgnoredByPolice = true;
			Game.Player.DispatchsCops = false;
			NetworkSetTalkerProximity(0f);
			Game.PlayerPed.Position = new Vector3(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z - 1);
			Game.PlayerPed.Heading = charSelectCoords.W;
			Client.GetInstance.GetExports["spawnmanager"].setAutoSpawn(false);
			Screen.Hud.IsRadarVisible = false;
			SetCanAttackFriendly(PlayerPedId(), true, true);
			NetworkSetFriendlyFireOption(true);
			SetEnablePedEnveffScale(PlayerPedId(), true);
			SetPlayerTargetingMode(2);
			SetMaxWantedLevel(0);
		}

		public static async void onPlayerSpawn()
		{
			AddTextEntry("FE_THDR_GTAO", ConfigClient.Conf.Main.NomeServer);
			scopedWeapons = ConfigClient.Conf.Main.ScopedWeapons;
			passengerDriveBy = ConfigClient.Conf.Main.PassengerDriveBy;
			SecondsBeforeKick = ConfigClient.Conf.Main.AFKCheckTime;
			kickWarning = ConfigClient.Conf.Main.KickWarning;
			BaseScript.TriggerEvent("chat:addMessage", new { color = new[] { 71, 255, 95 }, multiline = true, args = new[] { "^4Benvenuto nel server test di Manups4e" } });
			BaseScript.TriggerEvent("chat:addMessage", new { color = new[] { 71, 255, 95 }, multiline = true, args = new[] { "^4QUESTO SERVER E' IN FASE ALPHA" } });
			SetPlayerHealthRechargeMultiplier(PlayerId(), -1.0f);
//          BaseScript.TriggerEvent("lprp:toggleCompass", true);
			Eventi.Player.Stanziato = false;
			Game.PlayerPed.IsVisible = true;
			spawned = true;
			Eventi.Player.status.spawned = true;
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", Eventi.Player.CurrentChar.id);
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "status", true);
			if (Eventi.Player.DeathStatus)
			{
				HUD.ShowNotification("Sei stato ucciso perche ti sei disconnesso da morto!", NotificationColor.Red, true);
				var now = DateTime.Now;
				BaseScript.TriggerServerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- " + Eventi.Player.FullName + " e' spawnato morto poiché è sloggato da morto");
				Game.PlayerPed.Health = -100;
			}
			Peds();
			for (int i = 0; i < tipi.Count; i++)
			{
				RelationshipGroup rr = new RelationshipGroup(GetHashKey(tipi[i]));
				g.SetRelationshipBetweenGroups(rr, Relationship.Respect, true);
			}
			Screen.Hud.IsRadarVisible = true;
		}

		public static async void charSelect()
		{
			int a = Funzioni.GetRandomInt(SelectFirstCoords.Count-1);
			charSelectCoords = SelectFirstCoords[a];
			RequestCollisionAtCoord(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z);
			Game.PlayerPed.Position = new Vector3(charSelectCoords.X, charSelectCoords.Y, charSelectCoords.Z - 1);
			Game.PlayerPed.Heading = charSelectCoords.W;
			await Game.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			Game.PlayerPed.Style.SetDefaultClothes();
			while (!await Game.Player.ChangeModel(new Model(PedHash.FreemodeMale01))) { await BaseScript.Delay(50); }

			if (Game.PlayerPed.Model == new Model(PedHash.FreemodeMale01))
			{
				Game.PlayerPed.Style.SetDefaultClothes();
				Game.PlayerPed.SetDecor(Main.decorName, Main.decorInt);
				Game.PlayerPed.IsVisible = false;
				Eventi.Player.Stanziato = true;
				Game.PlayerPed.IsPositionFrozen = true;
				RequestCollisionAtCoord(charCreateCoords.X, charCreateCoords.Y, charCreateCoords.Z - 1);
				charSelectionCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
				SetGameplayCamRelativeHeading(0);
				charSelectionCam.Position = GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0f, -2, 0);
				charSelectionCam.PointAt(Game.PlayerPed);
				charSelectionCam.IsActive = true;
				RenderScriptCams(true, false, 0, false, false);
				Menus.CharSelectionMenu();
			}
			else charSelect();
		}

		public static async void charCreate()
		{
			RequestCollisionAtCoord(charCreateCoords.X, charCreateCoords.Y, charCreateCoords.Z - 1);
			SetEntityCoords(PlayerPedId(), charCreateCoords.X, charCreateCoords.Y, charCreateCoords.Z - 1, false, false, false, false);
			SetEntityHeading(PlayerPedId(), charCreateCoords.W);
			Vector3 h = GetPedBoneCoords(PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f);
			Vector3 offCoords = GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0.0f, 2.0f, 0.8f);
			charCreationCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true))
			{
				Position = new Vector3(offCoords.X, offCoords.Y, h.Z + 0.2f)
			};
			charCreationCam.PointAt(h);
			charCreationCam.IsActive = true;
			RenderScriptCams(true, false, 0, false, false);
			await Task.FromResult(0);
		}

		private static void GetHost(int hostid)
		{
			HostId = hostid;
		}

		public static async Task Entra()
		{
			if (NetworkIsSessionStarted())
			{
				BaseScript.TriggerServerEvent("lprp:setupUser");
				Client.GetInstance.DeregisterTickHandler(Entra);
			}
		}

		private static float OttieniShake
		{
			get
			{
				switch (Game.PlayerPed.Weapons.Current.Hash)
				{
					case WeaponHash.StunGun:
					case WeaponHash.FlareGun:
						return 0.01f;
					case WeaponHash.SNSPistol:
						return 0.02f;
					case WeaponHash.SNSPistolMk2:
					case WeaponHash.VintagePistol:
					case WeaponHash.DoubleAction:
					case WeaponHash.Pistol:
						return 0.025f;
					case WeaponHash.PistolMk2:
					case WeaponHash.CombatPistol:
					case WeaponHash.HeavyPistol:
					case WeaponHash.MarksmanPistol:
						return 0.03f;
					case WeaponHash.MicroSMG:
					case WeaponHash.MachinePistol:
					case WeaponHash.MiniSMG:
						return 0.035f;
					case WeaponHash.Musket:
						return 0.04f;
					case WeaponHash.Revolver:
					case WeaponHash.SMG:
						return 0.045f;
					case WeaponHash.APPistol:
					case WeaponHash.Pistol50:
					case WeaponHash.Gusenberg:
					case WeaponHash.BullpupRifle:
					case WeaponHash.CompactRifle:
					case WeaponHash.DoubleBarrelShotgun:
					case WeaponHash.AssaultSMG:
						return 0.05f;
					case WeaponHash.RevolverMk2:
					case WeaponHash.CombatPDW:
					case WeaponHash.SMGMk2:
						return 0.055f;
					case WeaponHash.CarbineRifle:
					case WeaponHash.AdvancedRifle:
					case WeaponHash.SawnOffShotgun:
					case WeaponHash.SpecialCarbine:
						return 0.06f;
					case WeaponHash.CarbineRifleMk2:
					case WeaponHash.BullpupRifleMk2:
						return 0.065f;
					case WeaponHash.MG:
					case WeaponHash.PumpShotgun:
					case WeaponHash.AssaultRifle:
						return 0.07f;
					case WeaponHash.SpecialCarbineMk2:
						return 0.075f;
					case WeaponHash.CombatMG:
					case WeaponHash.CompactGrenadeLauncher:
					case WeaponHash.BullpupShotgun:
					case WeaponHash.GrenadeLauncher:
					case WeaponHash.SweeperShotgun:
						return 0.08f;
					case WeaponHash.CombatMGMk2:
					case WeaponHash.AssaultRifleMk2:
					case WeaponHash.PumpShotgunMk2:
						return 0.085f;
					case WeaponHash.MarksmanRifle:
					case WeaponHash.MarksmanRifleMk2:
						return 0.1f;
					case WeaponHash.AssaultShotgun:
						return 0.12f;
					case WeaponHash.HeavyShotgun:
						return 0.13f;
					case WeaponHash.Minigun:
					case WeaponHash.SniperRifle:
						return 0.2f;
					case WeaponHash.HeavySniper:
						return 0.3f;
					case WeaponHash.HeavySniperMk2:
						return 0.35f;
					case WeaponHash.Firework:
						return 0.5f;
					case WeaponHash.RPG:
					case WeaponHash.HomingLauncher:
						return 0.9f;
					case WeaponHash.Railgun:
						return 1.0f;
					default: return 0;
				}
			}
		}

		public static async Task NewTick()
		{
			Game.DisableControlThisFrame(0, Control.SpecialAbilitySecondary);
			if (IsDead)
			{
				Game.DisableAllControlsThisFrame(0);
				EnableControlAction(0, 47, true);
				EnableControlAction(0, 245, true);
				EnableControlAction(0, 38, true);
				Game.EnableControlThisFrame(0, Control.FrontendPause);
			}
		}

		public static async Task MainTick()
		{
			if (Game.PlayerPed.IsJumping && Game.IsControlJustPressed(0, Control.Jump))
				Game.PlayerPed.Ragdoll(1);
			if (Game.Player.WantedLevel != 0)
				Game.Player.WantedLevel = 0;
			DisablePlayerVehicleRewards(PlayerId());
			SetPedMinGroundTimeForStungun(PlayerPedId(), 8000);
			if (Game.PlayerPed.IsInVehicle())
			{
				Vehicle veh = Game.PlayerPed.CurrentVehicle;
				if (veh.Driver == Game.PlayerPed)
					SetPlayerCanDoDriveBy(Game.Player.Handle, false);
				else if (passengerDriveBy)
					SetPlayerCanDoDriveBy(Game.Player.Handle, true);
				else
					SetPlayerCanDoDriveBy(Game.Player.Handle, false);
			}
			Weapon weapon = Game.PlayerPed.Weapons.Current;
			ManageReticle(weapon);
			if (IsPedArmed(PlayerPedId(), 6))
			{
				Game.DisableControlThisFrame(0, Control.MeleeAttackLight);
				Game.DisableControlThisFrame(0, Control.MeleeAttackHeavy);
				Game.DisableControlThisFrame(0, Control.MeleeAttackAlternate);
				Game.DisableControlThisFrame(0, Control.MeleeAttack1);
				Game.DisableControlThisFrame(0, Control.MeleeAttack2);
			}
			if(Game.PlayerPed.IsAiming || Game.PlayerPed.IsAimingFromCover || Game.PlayerPed.IsShooting)
				DisplayAmmoThisFrame(false);
			if (Game.PlayerPed.IsShooting)
			{
				ShakeGameplayCam("SMALL_EXPLOSION_SHAKE", OttieniShake);
				if (weapon.Hash == WeaponHash.FireExtinguisher)
					weapon.InfiniteAmmo = true;
			}
			if ((Game.IsControlJustPressed(0, Control.MpTextChatTeam) || Game.IsDisabledControlJustPressed(0, Control.MpTextChatTeam)) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
			{
				if (!IsEntityPlayingAnim(Game.PlayerPed.Handle, "mp_arresting", "idle", 3))
					startPointing();
			}
			else if (Game.IsControlJustReleased(0, Control.MpTextChatTeam) || Game.IsDisabledControlJustReleased(0, Control.MpTextChatTeam))
				StopPointing();
			if (ispointing)
			{
				float camPitch = GetGameplayCamRelativePitch();
				if (camPitch < -70.0)
					camPitch = -70.0f;
				else if (camPitch > 42.0)
					camPitch = 42.0f;
				camPitch = (camPitch + 70.0f) / 112.0f;
				float camHeading = GetGameplayCamRelativeHeading();
				float cosCamHeading = Cos(camHeading);
				float sinCamHeading = Sin(camHeading);
				if (camHeading < -180.0)
					camHeading = -180.0f;
				else if (camHeading > 180.0)
					camHeading = 180.0f;
				camHeading = (camHeading + 180.0f) / 360.0f;
				bool blocked = false;
				int nn = 0;
				Vector3 coords = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, (cosCamHeading * -0.2f) - (sinCamHeading * (0.4f * camHeading + 0.3f)), (sinCamHeading * -0.2f) + (cosCamHeading * (0.4f * camHeading + 0.3f)), 0.6f);
				int ray = StartShapeTestCapsule(coords.X, coords.Y, coords.Z - 0.2f, coords.X, coords.Y, coords.Z + 0.2f, 0.4f, 95, Game.PlayerPed.Handle, 7);
				GetShapeTestResult(ray, ref blocked, ref coords, ref coords, ref nn);
				SetTaskPropertyFloat(Game.PlayerPed.Handle, "Pitch", camPitch);
				SetTaskPropertyFloat(Game.PlayerPed.Handle, "Heading", camHeading * -1.0f + 1.0f);
				SetTaskPropertyBool(Game.PlayerPed.Handle, "isBlocked", blocked);
				SetTaskPropertyBool(Game.PlayerPed.Handle, "isFirstPerson", N_0xee778f8c7e1142e2(N_0x19cafa3c87f7c2ff()) == 4);
			}
			if (!Game.PlayerPed.IsInVehicle() && (!IsPauseMenuActive()))
				DisableRadarThisFrame();
			if (Game.PlayerPed.IsInVehicle())
			{
				Vehicle veh = Game.PlayerPed.CurrentVehicle;
				if (veh.Model.IsBicycle || IsThisModelAJetski((uint)veh.Model.Hash) || veh.Model.IsQuadbike || !veh.IsEngineRunning)
//				if (IsThisModelABicycle((uint)GetEntityModel(GetVehiclePedIsUsing(PlayerPedId()))) || IsThisModelAJetski((uint)GetEntityModel(GetVehiclePedIsUsing(PlayerPedId()))) || IsThisModelAQuadbike((uint)GetEntityModel(GetVehiclePedIsUsing(PlayerPedId()))) && (!GetIsVehicleEngineRunning(GetVehiclePedIsIn(PlayerPedId(), false))))
					DisableRadarThisFrame();
			}
			if (Game.IsPaused)
				HUD.DrawText3D(Game.PlayerPed.Bones[Bone.SKEL_Head].Position.X, Game.PlayerPed.Bones[Bone.SKEL_Head].Position.Y, Game.PlayerPed.Bones[Bone.SKEL_Head].Position.Z + .85f, Colors.White, "IN PAUSA");
			bool isnear = World.GetAllPickups().Select(o => new Pickup(o.Handle)).Where(o=>pickupList.Contains(o.Handle)).Any(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * 10f, 2));
			if (isnear) World.GetAllPickups().Select(o => new Pickup(o.Handle)).Where(o => pickupList.Contains(o.Handle)).First(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * 10f, 2)).Delete();
//			for (int i = 0; i < pickupList.Count; i++)
//				RemoveAllPickupsOfType((uint)GetHashKey(pickupList[i]));
			for (int i = 1; i < 16; i++) EnableDispatchService(i, false);
		}

		public static async Task Recoil()
		{
			if (Game.PlayerPed.IsShooting && !Game.PlayerPed.IsDoingDriveBy)
			{
				Weapon wep = Game.PlayerPed.Weapons.Current;
				int ammo = wep.AmmoInClip;
				if (recoils[(uint)wep.Hash] != 0)
				{
					float tv = 0;
					if (GetFollowPedCamViewMode() != 4)
					{
						do
						{
							SetGameplayCamRelativePitch(GetGameplayCamRelativePitch() + 0.1f, 0.2f);
							tv += 0.1f;
							await BaseScript.Delay(0);
						} while (tv < recoils[(uint)wep.Hash]);
					}
					else
					{
						do
						{
							await BaseScript.Delay(0);
							if (recoils[(uint)wep.Hash] > 0.1)
							{
								SetGameplayCamRelativePitch(GetGameplayCamRelativePitch() + 0.6f, 1.2f);
								tv += 0.6f;
							}
							else
							{
								SetGameplayCamRelativePitch(GetGameplayCamRelativePitch() + 0.016f, 0.333f);
								tv += 0.1f;
							}
						} while (tv < recoils[(uint)wep.Hash]);
					}
				}
			}

		}

		private static async void startPointing()
		{
			ispointing = true;
			RequestAnimDict("anim@mp_point");
			while (!HasAnimDictLoaded("anim@mp_point")) await BaseScript.Delay(0);
			SetPedCurrentWeaponVisible(Game.PlayerPed.Handle, false, true, true, true);
			SetPedConfigFlag(Game.PlayerPed.Handle, 36, true);
			TaskMoveNetwork(Game.PlayerPed.Handle, "task_mp_pointing", 0.5f, false, "anim@mp_point", 24);
			RemoveAnimDict("anim@mp_point");
		}

		private static async void StopPointing()
		{
			ispointing = false;
			N_0xd01015c7316ae176(Game.PlayerPed.Handle, "Stop");
			if (!Game.PlayerPed.IsInjured)
				Game.PlayerPed.Task.ClearSecondary();
			if (!Game.PlayerPed.IsInVehicle())
				SetPedCurrentWeaponVisible(Game.PlayerPed.Handle, true, true, true, true);
			SetPedConfigFlag(Game.PlayerPed.Handle, 36, false);
			Game.PlayerPed.Task.ClearSecondary();
			await Task.FromResult(0);
		}

		public static int AFKTime = 600;
		static int wait = 2000;
		public static bool abort = false;
		private static bool triggerato = false;
		private static Vector3 currentPosition;
		private static Vector3 previousPosition;

		public static async Task AFK()
		{
			await BaseScript.Delay(wait);
			if (Eventi.Player.group_level < 3 && !Menus.Creazione.Visible || !Menus.Apparel.Visible || !Menus.Apparenze.Visible || !Menus.Dettagli.Visible || !Menus.Genitori.Visible || !Menus.Info.Visible) // helper e moderatori sono inclusi (gradi 0,1,2)
			{
				currentPosition = Game.PlayerPed.Position;
				if (World.GetDistance(currentPosition, previousPosition) < .8f && !abort)
				{
					if (AFKTime > 0)
					{
						--AFKTime;
						if (kickWarning && AFKTime < (int)Math.Round(SecondsBeforeKick / 4f))
						{
							string Text = "Sei stato rilevato AFK per troppo tempo\nVerrai kickato tra alcuni istanti!";
							if (!triggerato)
							{
								BaseScript.TriggerEvent("lprp:manager:warningMessage", "Last Planet Shield 2.0", Text, 8, "lprp:AFKScelta");
								triggerato = true;
							}
							BaseScript.TriggerEvent("lprp:manager:updateText", Text);
						}
					}
					else
					{
						BaseScript.TriggerServerEvent("lprp:dropPlayer", "Last Planet Shield 2.0:\nSei stato rilevato per troppo tempo AFK");
					}
				}
				else
				{
					if (AFKTime != SecondsBeforeKick && SecondsBeforeKick > 0)
					{
						AFKTime = SecondsBeforeKick;
					}
				}
			}
			previousPosition = currentPosition;
			wait = 1000;
		}

		private static void AFKScelta(string scelta)
		{
			if (scelta == "select" || scelta == "back" || scelta == "alternative")
			{
				AFKTime = SecondsBeforeKick;
				triggerato = false;
			}
		}

		private static bool HashInTable(uint hash)
		{
			for (int i = 0; i < scopedWeapons.Count; i++)
				if (hash == scopedWeapons[i])
					return true;
			return false;
		}

		private static void ManageReticle(Weapon weapon)
		{
			if (!HashInTable((uint)weapon.Hash))
				Screen.Hud.HideComponentThisFrame(HudComponent.Reticle);
		}

		private static async Task Istanza()
		{
			if (Eventi.Player != null)
			{
				if (Eventi.Player.Stanziato) Funzioni.ConcealAllPlayers();
				else Funzioni.RevealAllPlayers();
			}
			await BaseScript.Delay(10000);
		}

		public static void RespawnPed(Vector3 coords)
		{
			IsDead = false;
			Game.PlayerPed.PositionNoOffset = coords;
			NetworkResurrectLocalPlayer(coords.X, coords.Y, coords.Z, Game.PlayerPed.Heading, true, false);
			Game.PlayerPed.Health = 100;
			Game.PlayerPed.IsInvincible = false;
			Game.PlayerPed.ClearBloodDamage();
		}

		private static async void Peds()
		{
			await BaseScript.Delay(30000);
			while (ConfigClient.Conf.Main.stripClub == null)
			{
				await BaseScript.Delay(0);
			}

			foreach (var stripper in ConfigClient.Conf.Main.stripClub)
			{
				Ped ped = await World.CreatePed(new Model(GetHashKey(stripper.model)), new Vector3(stripper.coords[0], stripper.coords[1], stripper.coords[2]), stripper.heading);
				ped.CanRagdoll = false;
				ped.BlockPermanentEvents = true;
				SetEntityCanBeDamaged(ped.Handle, false);
				SetPedFleeAttributes(ped.Handle, 0, false);
				SetPedCombatAttributes(ped.Handle, 17, true);
				ped.Task.PlayAnimation(stripper.animDict, stripper.animName, -1, -1, AnimationFlags.Loop);
			}
			foreach (var market in ConfigClient.Conf.Main.blackMarket)
			{
				Ped ped1 = await World.CreatePed(new Model(GetHashKey(market.model)), new Vector3(market.coords[0], market.coords[1], market.coords[2]), market.heading);
				ped1.CanRagdoll = false;
				ped1.BlockPermanentEvents = true;
				SetEntityCanBeDamaged(ped1.Handle, false);
				SetPedFleeAttributes(ped1.Handle, 0, false);
				SetPedCombatAttributes(ped1.Handle, 17, true);
				ped1.Task.StartScenario(market.animName, GetEntityCoords(ped1.Handle, true));
			}
			foreach (var illegal in ConfigClient.Conf.Main.illegal_weapon_extra_shop)
			{
				Ped ped2 = await World.CreatePed(new Model(GetHashKey(illegal.model)), new Vector3(illegal.coords[0], illegal.coords[1], illegal.coords[2]), illegal.heading);
				ped2.CanRagdoll = false;
				ped2.BlockPermanentEvents = true;
				SetEntityCanBeDamaged(ped2.Handle, false);
				SetPedFleeAttributes(ped2.Handle, 0, false);
				SetPedCombatAttributes(ped2.Handle, 17, true);
				ped2.Task.StartScenario(illegal.animName, GetEntityCoords(ped2.Handle, true));
			}
		}

		public static async Task Armi()
		{
			await BaseScript.Delay(30000);
			if (LoadoutLoaded)
			{
				List<Weapons> armiAgg = new List<Weapons>();
				bool loadoutChanged = false;
				if (Game.Player.IsDead)
				{
					LoadoutLoaded = false;
				}

				for (int i = 0; i < SharedScript.Armi.Count; i++)
				{
					string weaponName = SharedScript.Armi[i].name;
					WeaponHash weaponHash = (WeaponHash)GetHashKey(weaponName);
					List<Components> weaponComponents = new List<Components>();
					int tinta = 0;
					if (Game.PlayerPed.Weapons.HasWeapon(weaponHash) && weaponName != "WEAPON_UNARMED")
					{
						int ammo = GetAmmoInPedWeapon(PlayerPedId(), (uint)weaponHash);
						List<Components> components = SharedScript.Armi[i].components;
						if (components.Count > 0)
						{
							for (int j = 0; j < components.Count; j++)
							{
								if (HasPedGotWeaponComponent(PlayerPedId(), (uint)weaponHash, (uint)GetHashKey(components[j].name)))
								{
									weaponComponents.Add(new Components(components[j].name, components[j].active));
								}
							}
						}
						for (int l = 0; l < Eventi.Player.getCharWeapons(Eventi.Player.char_current).Count; l++)
						{
							Weapons arm = Eventi.Player.getCharWeapons(Eventi.Player.char_current)[l];
							tinta = arm.tint;
						}
						if (!LastLoadout.ContainsKey(weaponName) || LastLoadout[weaponName] != ammo)
						{
							loadoutChanged = true;
						}

						LastLoadout[weaponName] = ammo;
						armiAgg.Add(new Weapons(weaponName, ammo, weaponComponents, tinta));
					}
					else
					{
						if (LastLoadout.ContainsKey(weaponName))
						{
							loadoutChanged = true;
						}

						LastLoadout.Remove(weaponName);
					}
				}
				if (loadoutChanged && LoadoutLoaded)
				{
					Eventi.Player.CurrentChar.weapons.Clear();
					Eventi.Player.CurrentChar.weapons = armiAgg;
					try
					{
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "weapons", JsonConvert.SerializeObject(armiAgg));
					}
					catch (Exception e)
					{
						Debug.WriteLine("Errore in Main:767 = " + e);
					}

				}
			}
			foreach (Player p in Client.GetInstance.GetPlayers.ToList())
			{
				SetCanAttackFriendly(p.Character.Handle, true, true);
				NetworkSetFriendlyFireOption(true);
			}
		}
	}

	public class GenericPeds
	{
		public float[] coords = new float[3];
		public string animDict;
		public string animName;
		public string model;
		public float heading;
		public string scenario;

		public GenericPeds() { }
		public GenericPeds(float[] _c, string _ad, string _an, string _ml, float _h)
		{
			this.coords = _c;
			this.animDict = _ad;
			this.animName = _an;
			this.model = _ml;
			this.heading = _h;
		}
	}
}
